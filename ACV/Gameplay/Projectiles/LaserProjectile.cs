using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ACV
{
    /// <summary>
    /// A laser bolt projectile.
    /// </summary>
    public class LaserProjectile : Projectile
    {
        #region Constants


        /// <summary>
        /// The length of the laser-bolt line, expressed as a percentage of velocity.
        /// </summary>
        const float initialSpeed = 640f;


        #endregion


        #region Static Graphics Data


        /// <summary>
        /// Texture for all laser projectiles.
        /// </summary>
        private static Texture2D texture;


        public override Texture2D Sprite
        {
            get { return texture; }
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
        /// Constructs a new laser projectile.
        /// </summary>
        /// <param name="owner">The tank that fired this projectile, if any.</param>
        /// <param name="direction">The initial direction for this projectile.</param>
        public LaserProjectile(Tank owner, Vector2 direction)
            : base(owner, direction)
        {
            // set the gameplay data
            Velocity = initialSpeed * direction;

            this.mass = 0.5f;

            // set the projectile data
            this.duration = 5f;
            this.damageAmount = 20f;
            this.damageRadius = 0f;
            this.damageOwner = false;
        }


        #endregion


        #region Drawing Methods


        /// <summary>
        /// Draw the laser projectile.
        /// </summary>
        /// <param name="elapsedTime">The amount of elapsed time, in seconds.</param>
        /// <param name="spriteBatch">The SpriteBatch object used to draw.</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            // ignore the parameter color if we have an owner
            base.Draw(elapsedTime, spriteBatch, texture, null,
                owner != null ? owner.Color : Color.White);
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
                // TODO: Display the laser explosion
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
            texture = contentManager.Load<Texture2D>("Textures/laser");
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
