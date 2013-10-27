using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACV
{
    public class GreenLaser : Laser
    {
        //private List<Weapon> lasers = new List<Weapon>();

        //// Rectangle the size of the viewport to check if a laser is outside and alive
        //private Rectangle viewportRect;

        //private const int maxLasers = 8;
        //private const float greenLaserSpeed = 3.0f;
        //private const float redLaserSpeed = 4.0f;

        //private float millisecbetweenshots = 250f;

        //private float time = 0f;
        
        //public List<Weapon> LaserList
        //{
        //    get { return lasers; }
        //    set { lasers = value; }
        //}

        public GreenLaser(Tank owner)
            : base(owner)
        {
            //for (int i = 0; i < maxLasers; i++)
            //{
            //    lasers.Add(new Weapon());
            //}
        }

        #region Interaction Methods


        /// <summary>
        /// Create and spawn the projectile(s) from a firing from this weapon.
        /// </summary>
        /// <param name="direction">The direction that the projectile will move.</param>
        protected override void CreateProjectiles(Vector2 direction)
        {
            // create the new projectile
            LaserProjectile projectile = new LaserProjectile(Owner, direction);
            Owner.Projectiles.Add(projectile);
        }


        #endregion

        //public static void LoadContent(GraphicsDevice device, SpriteBatch spriteBatch, Texture2D laserAnimation, Vector2 sourceSize)
        //{
        //    // Add the laser animation into the list
        //    foreach (Weapon laser in lasers)
        //    {
        //        laser.LoadContent(device, spriteBatch, laserAnimation, sourceSize);
        //    }

        //    // Create the viewportRect
        //    viewportRect = new Rectangle(0, 0,
        //        240,
        //        320 + 10);
	        
        //}
        //public void Update(float elapsedTime, Vehicle tank)
        //{
        //    // Checks to fire or not
        //    time += elapsedTime.ElapsedGameTime.Milliseconds;
        //    if (time > millisecbetweenshots)
        //    {
        //        FireLaser(tank, elapsedTime);
        //        // Reset the time
        //        time = 0f;
        //    }
            
        //    // Check to see if the lasers are still alive
        //    UpdateLasers(elapsedTime, tank);

        //    // If they aren't make them invisible 
        //    //(this isn't really nesscessary because they're already off the viewport)
        //    foreach (Weapon laser in lasers)
        //    {
        //        laser.Update(elapsedTime);
        //    }
        //}
        //public void Draw(float elapsedTime)
        //{
        //    // Draw each laser
        //    foreach (Weapon laser in lasers)
        //    {
        //        laser.Draw();
        //    }
        //}

        //public void FireLaser(Vehicle tank, float elapsedTime)
        //{

        //    foreach (Weapon laser in lasers)
        //    {
        //        if (laser.Alive == false)
        //        {
        //            // TODO: Play the Laser Sound

        //            laser.Alive = true;

        //            laser.Position = new Vector2(tank.Position.X, tank.Position.Y - (float)(tank.ActualSize.Y / 2));

        //            //Determine velocity using sine and
        //            //cosine of the cannon's rotation angle,
        //            //then scale by 5 to speed up the ball.
        //            laser.Velocity = new Vector2(0, -1) * greenLaserSpeed;
        //            return;
        //        }
        //    }
        //}
        //private void UpdateLasers(float elapsedTime, Vehicle tank)
        //{
        //    foreach (Weapon laser in lasers)
        //    {
        //        laser.Update(elapsedTime);
        //        if (laser.Alive == true)
        //        {
        //            //Check if the laser is contained within
        //            //the screen-sized rectangle. If not,
        //            //kill the laser.
        //            laser.Position += laser.Velocity;
        //            if (!viewportRect.Contains(new Point(
        //                (int)(laser.Position.X), (int)(laser.Position.Y))))
        //            {
        //                laser.Alive = false;
        //            }
        //        }
        //    }
        //} 
    }


}
