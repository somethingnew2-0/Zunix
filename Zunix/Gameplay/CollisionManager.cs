using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Zunix;

namespace Zunix.Gameplay
{
    /// <summary>
    /// Manages collisions and collision events between all gameplay objects.
    /// </summary>
    public class CollisionManager : BatchRemovalCollection<GameObject>
    {
        #region Constants


        /// <summary>
        /// The ratio of speed to damage applied, for explosions.
        /// </summary>
        private const float speedDamageRatio = 0.5f;

        /// <summary>
        /// The number of times that the FindSpawnPoint method will try to find a point.
        /// </summary>
        private const int findSpawnPointAttempts = 25;


        #endregion


        #region Helper Types


        /// <summary>
        /// The result of a collision query.
        /// </summary>
        struct CollisionResult
        {
            /// <summary>
            /// What caused the collison (what the source ran into)
            /// </summary>
            public GameObject GameObject;
        }


        #endregion


        #region Singleton


        /// <summary>
        /// Singleton for collision management.
        /// </summary>
        private static CollisionManager collisionManager = new CollisionManager();
        public static BatchRemovalCollection<GameObject> Collection
        {
            get { return collisionManager as BatchRemovalCollection<GameObject>; }
        }


        #endregion


        #region Collision Data


        /// <summary>
        /// The dimensions of the space in which collision occurs.
        /// </summary>
        private Rectangle dimensions = new Rectangle(0, 0, 240, 320);
        public static Rectangle Dimensions
        {
            get
            {
                return (collisionManager == null ? Rectangle.Empty :
              collisionManager.dimensions);
            }
            set
            {
                // safety-check the singleton
                if (collisionManager == null)
                {
                    throw new InvalidOperationException(
                        "The collision manager has not yet been initialized.");
                }
                collisionManager.dimensions = value;
            }
        }

        ///// <summary>
        ///// The list of barriers in the game world.
        ///// </summary>
        ///// <remarks>This list is not owned by this object.</remarks>
        //private List<Rectangle> barriers = new List<Rectangle>();
        //public static List<Rectangle> Barriers
        //{
        //    get
        //    {
        //        return (collisionManager == null ? null :
        //      collisionManager.barriers);
        //    }
        //}


        /// <summary>
        /// Cached list of collision results, for more optimal collision detection.
        /// </summary>
        List<CollisionResult> collisionResults = new List<CollisionResult>();


        #endregion


        #region Initialization Methods


        /// <summary>
        /// Constructs a new collision manager.
        /// </summary>
        private CollisionManager() { }


        #endregion


        #region Updating Methods


        /// <summary>
        /// Update the collision system.
        /// </summary>
        /// <param name="elapsedTime">The amount of elapsed time, in seconds.</param>
        public static void Update(float elapsedTime)
        {
            // safety-check the singleton
            if (collisionManager == null)
            {
                throw new InvalidOperationException(
                    "The collision manager has not yet been initialized.");
            }


            // move each object
            for (int i = 0; i < collisionManager.Count; ++i)
            {
                if (collisionManager[i].Alive)
                {
                    // determine how far they are going to move
                    Vector2 movement = collisionManager[i].Speed * elapsedTime;
                    // only allow collisionManager that have not collided yet 
                    // collisionManager frame to collide
                    // -- otherwise, objects can "double-hit" and trade their momentum
                    if (collisionManager[i].CollidedThisFrame == false)
                    {
                        movement = MoveAndCollide(collisionManager[i], movement);
                    }
                    // determine the new position
                    collisionManager[i].Position += movement;

                }
            }

            CollisionManager.Collection.ApplyPendingRemovals();
        }


        /// <summary>
        /// Move the given gameplayObject by the given movement, colliding and adjusting
        /// as necessary.
        /// </summary>
        /// <param name="gameplayObject">The gameplayObject who is moving.</param>
        /// <param name="movement">The desired movement vector for this update.</param>
        /// <returns>The movement vector after considering all collisions.</returns>
        private static Vector2 MoveAndCollide(GameObject gameObject,
            Vector2 movement)
        {
            // safety-check the singleton
            if (collisionManager == null)
            {
                throw new InvalidOperationException(
                    "The collision manager has not yet been initialized.");
            }

            if (gameObject == null)
            {
                throw new ArgumentNullException("gameplayObject");
            }
            // make sure we care about where this gameplayObject goes
            if (!gameObject.Alive)
            {
                return movement;
            }
            // make sure the movement is significant
            if (movement.LengthSquared() <= 0f)
            {
                return movement;
            }

            // generate the list of collisions
            Collide(gameObject, movement);

            // determine if we had any collisions
            if (collisionManager.collisionResults.Count > 0)
            {
                foreach (CollisionResult collision in collisionManager.collisionResults)
                {
                    // let the two objects touch each other, and see what happens
                    if (gameObject.Touch(collision.GameObject) &&
                        collision.GameObject.Touch(gameObject))
                    {
                        gameObject.CollidedThisFrame =
                            collision.GameObject.CollidedThisFrame = true;
                        // they should react to the other, even if they just killd
                        AdjustVelocities(gameObject, collision.GameObject);
                        return Vector2.Zero;
                    }
                }
            }

            return movement;
        }


        /// <summary>
        /// Determine all collisions that will happen as the given gameplayObject moves.
        /// </summary>
        /// <param name="gameplayObject">The gameplayObject that is moving.</param>
        /// <param name="movement">The gameplayObject's movement vector.</param>
        /// <remarks>The results are stored in the cached list.</remarks>
        public static void Collide(GameObject gameObject, Vector2 movement)
        {
            // safety-check the singleton
            if (collisionManager == null)
            {
                throw new InvalidOperationException(
                    "The collision manager has not yet been initialized.");
            }

            collisionManager.collisionResults.Clear();

            if (gameObject == null)
            {
                throw new ArgumentNullException("gameplayObject");
            }
            if (!gameObject.Alive)
            {
                return;
            } 

            // determine the movement direction and scalar
            float movementLength = movement.Length();
            if (movementLength <= 0f)
            {
                return;
            }

            // create the transform of the gameObject for the bounding Rectangle
            Matrix gameObjectTransform =
                Matrix.CreateTranslation(new Vector3(-gameObject.Sprite.Width, -gameObject.Sprite.Height, 0.0f)) *
                // Matrix.CreateScale(block.Scale) *  would go here
                Matrix.CreateRotationZ(gameObject.Rotation) *
                Matrix.CreateTranslation(new Vector3(gameObject.Position, 0.0f));

            // create the bounding rectangle of the gameObject
            Rectangle gameObjectRectangle = CollisionMath.CalculateBoundingRectangle(
                new Rectangle(0, 0, gameObject.Sprite.Width, gameObject.Sprite.Height),
                gameObjectTransform);

            // check each gameObject
            foreach (GameObject checkActor in collisionManager)
            {
                if ((gameObject == checkActor) || !checkActor.Alive)
                {
                    continue;
                }

                // create the transform of the checkActor for the bounding Rectangle
                Matrix checkActorTransform =
                    Matrix.CreateTranslation(new Vector3(-checkActor.Sprite.Width, -checkActor.Sprite.Height, 0.0f)) *
                    // Matrix.CreateScale(block.Scale) *  would go here
                    Matrix.CreateRotationZ(checkActor.Rotation) *
                    Matrix.CreateTranslation(new Vector3(checkActor.Position, 0.0f));

                // create the bounding rectangle of the checkActor
                Rectangle checkActorRectangle = CollisionMath.CalculateBoundingRectangle(
                    new Rectangle(0, 0, checkActor.Sprite.Width, checkActor.Sprite.Height),
                    checkActorTransform);

                // check first bounding rectangles because per-pixel hard
                if (!gameObjectRectangle.Intersects(checkActorRectangle))
                {
                    continue;
                }
                else
                {
                    // now do per-pixel to check for a collision
                    if (!CollisionMath.IntersectPixels(gameObjectRectangle, gameObject.SpriteData, checkActorRectangle, checkActor.SpriteData))
                    {
                        continue;
                    }
                }
                
                CollisionResult result = new CollisionResult();
                result.GameObject = checkActor;

                collisionManager.collisionResults.Add(result);
            }
        }


        /// <summary>
        /// Adjust the velocities of the two collisionManager as if they have collided,
        /// distributing their velocities according to their masses.
        /// </summary>
        /// <param name="actor1">The first gameplayObject.</param>
        /// <param name="actor2">The second gameplayObject.</param>
        private static void AdjustVelocities(GameObject actor1,
            GameObject actor2)
        {
            // don't adjust velocities if at least one has negative mass
            if ((actor1.Mass <= 0f) || (actor2.Mass <= 0f))
            {
                return;
            }

            // determine the vectors normal and tangent to the collision
            Vector2 collisionNormal = actor2.Position - actor1.Position;
            if (collisionNormal.LengthSquared() > 0f)
            {
                collisionNormal.Normalize();
            }
            else
            {
                return;
            }

            Vector2 collisionTangent = new Vector2(
                -collisionNormal.Y, collisionNormal.X);

            // determine the speed components along the normal and tangent vectors
            float velocityNormal1 = Vector2.Dot(actor1.Speed, collisionNormal);
            float velocityTangent1 = Vector2.Dot(actor1.Speed, collisionTangent);
            float velocityNormal2 = Vector2.Dot(actor2.Speed, collisionNormal);
            float velocityTangent2 = Vector2.Dot(actor2.Speed, collisionTangent);

            // determine the new velocities along the normal
            float velocityNormal1New = ((velocityNormal1 * (actor1.Mass - actor2.Mass))
                + (2f * actor2.Mass * velocityNormal2)) / (actor1.Mass + actor2.Mass);
            float velocityNormal2New = ((velocityNormal2 * (actor2.Mass - actor1.Mass))
                + (2f * actor1.Mass * velocityNormal1)) / (actor1.Mass + actor2.Mass);

            // determine the new total velocities
            actor1.Speed = (velocityNormal1New * collisionNormal) +
                (velocityTangent1 * collisionTangent);
            actor2.Speed = (velocityNormal2New * collisionNormal) +
                (velocityTangent2 * collisionTangent);
        }


        #endregion


        #region Interaction Methods


        /// <summary>
        /// Find a valid spawn point in the world.
        /// </summary>
        /// <param name="radius">The radius of the object to be spawned.</param>
        /// <param name="random">A persistent Random object.</param>
        /// <returns>The spawn point.</returns>
        public static Vector2 FindSpawnPoint(GameObject spawnedObject, float radius)
        {
            // safety-check the singleton
            if (collisionManager == null)
            {
                throw new InvalidOperationException(
                    "The collision manager has not yet been initialized.");
            }

            // safety-check the parameters
            if ((radius < 0f) || (radius > Dimensions.Width / 2))
            {
                throw new ArgumentOutOfRangeException("radius");
            }

            // create the transform of the spawnedObject for the bounding Rectangle
            Matrix spawnedObjectTransform =
                Matrix.CreateTranslation(new Vector3(-spawnedObject.Sprite.Width, -spawnedObject.Sprite.Height, 0.0f)) *
                // Matrix.CreateScale(block.Scale) *  would go here
                Matrix.CreateRotationZ(spawnedObject.Rotation) *
                Matrix.CreateTranslation(new Vector3(spawnedObject.Position, 0.0f));

            // create the bounding rectangle of the spawnedObject
            Rectangle spawnedObjectRectangle = CollisionMath.CalculateBoundingRectangle(
                new Rectangle(0, 0, spawnedObject.Sprite.Width, spawnedObject.Sprite.Height),
                spawnedObjectTransform);

            // keep trying to find a valid point
            spawnedObject.Position = new Vector2(
                   RandomMath.Random.Next(240),
                   RandomMath.Random.Next(100, 320));

            for (int i = 0; i < findSpawnPointAttempts; i++)
            {
                bool valid = true;

            //    // check the barriers
            //    if (Barriers != null)
            //    {
            //        CollisionMath.CircleLineCollisionResult result =
            //            new CollisionMath.CircleLineCollisionResult();
            //        foreach (Rectangle rectangle in Barriers)
            //        {
            //            if (CollisionMath.CircleRectangleCollide(spawnPoint, radius,
            //                rectangle, ref result))
            //            {
            //                valid = false;
            //                break;
            //            }
            //        }
            //    }

                // check the other objects
                if (valid)
                {
                    foreach (GameObject gameObject in collisionManager)
                    {
                        if (!gameObject.Alive || (gameObject == spawnedObject))
                        {
                            continue;
                        }

                        // create the transform of the gameObject for the bounding Rectangle
                        Matrix gameObjectTransform =
                            Matrix.CreateTranslation(new Vector3(-gameObject.Sprite.Width, -gameObject.Sprite.Height, 0.0f)) *
                            // Matrix.CreateScale(block.Scale) *  would go here
                            Matrix.CreateRotationZ(gameObject.Rotation) *
                            Matrix.CreateTranslation(new Vector3(gameObject.Position, 0.0f));

                        // create the bounding rectangle of the gameObject
                        Rectangle gameObjectRectangle = CollisionMath.CalculateBoundingRectangle(
                            new Rectangle(0, 0, gameObject.Sprite.Width, gameObject.Sprite.Height),
                            gameObjectTransform);

                        // Just check bounding rectangle no per-pixel
                        if (spawnedObjectRectangle.Intersects(gameObjectRectangle))
                        {
                            valid = false;
                            break;
                        }
                    }
                }
                if (valid)
                {
                    break;
                }
                spawnedObject.Position = new Vector2(
                                   RandomMath.Random.Next(240),
                                   RandomMath.Random.Next(100, 320));
            }

            return spawnedObject.Position;
        }


        /// <summary>
        /// Process an explosion in the world against the objects in it.
        /// </summary>
        /// <param name="source">The source of the explosion.</param>
        /// <param name="target">The target of the attack.</param>
        /// <param name="damageAmount">The amount of explosive damage.</param>
        /// <param name="position">The position of the explosion.</param>
        /// <param name="damageRadius">The radius of the explosion.</param>
        /// <param name="damageOwner">If true, it will hit the source.</param>
        public static void Explode(GameObject source, GameObject target,
            float damageAmount, Vector2 position, float damageRadius, bool damageOwner)
        {
            // safety-check the singleton
            if (collisionManager == null)
            {
                throw new InvalidOperationException(
                    "The collision manager has not yet been initialized.");
            }

            if (damageRadius <= 0f)
            {
                return;
            }
            float damageRadiusSquared = damageRadius * damageRadius;

            foreach (GameObject gameplayObject in collisionManager)
            {
                // don't bother if it's already dead
                if (!gameplayObject.Alive)
                {
                    continue;
                }
                // don't hurt the GameObject that the projectile hit, it's hurt
                if (gameplayObject == target)
                {
                    continue;
                }
                // don't hit the owner if the damageOwner flag is off
                if ((gameplayObject == source) && !damageOwner)
                {
                    continue;
                }
                // measure the distance to the GameObject and see if it's in range
                Vector2 direction = gameplayObject.Position - position;
                float distanceSquared = direction.LengthSquared();
                if ((distanceSquared > 0f) && (distanceSquared <= damageRadiusSquared))
                {
                    float distance = (float)Math.Sqrt((float)distanceSquared);
                    // adjust the amount of damage based on the distance
                    // -- note that damageRadius <= 0 is accounted for earlier
                    float adjustedDamage = damageAmount *
                        (damageRadius - distance) / damageRadius;
                    // if we're still damaging the GameObject, then apply it
                    if (adjustedDamage > 0f)
                    {
                        gameplayObject.Damage(source, adjustedDamage);
                    }
                    // move those affected by the blast
                    if (gameplayObject != source)
                    {
                        direction.Normalize();
                        gameplayObject.Speed += direction * adjustedDamage *
                            speedDamageRatio;
                    }
                }
            }
        }


        #endregion
    }
}
