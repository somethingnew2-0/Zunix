using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Zunix.Gameplay.Vehicles;

namespace Zunix.Gameplay.Projectiles
{
    /// <summary>
    /// A missile projectile.
    /// </summary>
    public class MissileProjectile : Projectile
    {
        #region Constants


        /// <summary>
        /// The initial speed of the missile.
        /// </summary>
        const float initialSpeed = 650f;


        #endregion


        #region Static Graphics Data


        /// <summary>
        /// Texture for all missile projectiles.
        /// </summary>
        private static Texture2D texture;

        public override Texture2D Sprite
        {
            get
            {
                return texture;
            }
        }

        private static Color[] spriteData;

        public override Color[] SpriteData
        {
            get
            {
                return spriteData;
            }
        }

        #endregion


        #region Initialization


        /// <summary>
        /// Constructs a new missile projectile.
        /// </summary>
        /// <param name="owner">The tank that fired this projectile, if any.</param>
        /// <param name="direction">The initial direction for this projectile.</param>
        public MissileProjectile(Tank owner, Vector2 direction)
            : base(owner, direction)
        {
            // set the gameplay data
            this.velocity = initialSpeed * direction;

            // set the collision data
            this.radius = 14f;
            this.mass = 10f;

            // set the projectile data
            this.duration = 4f;
            this.damageAmount = 150f;
            this.damageOwner = false;
            this.damageRadius = 128f;
            this.rotation += MathHelper.Pi;
        }


        ///// <summary>
        ///// Initialize the missile projectile to it's default gameplay states.
        ///// </summary>
        //public override void Initialize()
        //{
        //    if (!active)
        //    {
        //        //// get and play the missile-flying cue
        //        //missileCue = AudioManager.GetCue("missile");
        //        //if (missileCue != null)
        //        //{
        //        //    missileCue.Play();
        //        //}

        //    }

        //    base.Initialize();
        //}


        #endregion


        #region Drawing Methods


        /// <summary>
        /// Draw the missile.
        /// </summary>
        /// <param name="elapsedTime">The amount of elapsed time, in seconds.</param>
        /// <param name="spriteBatch">The SpriteBatch object used to draw.</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            // ignore the parameter color if we have an owner
            base.Draw(elapsedTime, spriteBatch, texture, null, Color.White);
        }


        #endregion


        #region Interaction Methods


        /// <summary>
        /// Kills this object, in response to the given GameObject.
        /// </summary>
        /// <param name="source">The GameObject responsible for the kill.</param>
        /// <param name="cleanupOnly">
        /// If true, the object kills without any further effects.
        /// </param>
        public override void Kill(GameObject source, bool cleanupOnly)
        {
            if (Alive)
            {
                if (!cleanupOnly)
                {
                    //// play the explosion sound
                    //AudioManager.PlayCue("explosionMedium");
                }

                //// stop the missile-flying cue
                //if (missileCue != null)
                //{
                //    missileCue.Stop(AudioStopOptions.Immediate);
                //    missileCue.Dispose();
                //    missileCue = null;
                //}

            }

            base.Kill(source, cleanupOnly);
        }


        #endregion


        #region Static Graphics Methods


        /// <summary>
        /// Load all of the static graphics content for this class.
        /// </summary>
        /// <param name="contentManager">The content manager to load with.</param>
        public static void LoadContent(ContentManager contentManager)
        {
            // safety-check the parameters
            if (contentManager == null)
            {
                throw new ArgumentNullException("contentManager");
            }

            // load the texture
            texture = contentManager.Load<Texture2D>("Textures/missile");
        }

        /// <summary>
        /// Unload all of the static graphics content for this class.
        /// </summary>
        public static void UnloadContent()
        {
            texture = null;
        }


        #endregion
    }
}
