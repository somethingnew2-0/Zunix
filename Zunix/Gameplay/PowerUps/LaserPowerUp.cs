using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Zunix.Gameplay.Vehicles;

namespace Zunix.Gameplay.Powerups
{
    /// <summary>
    /// A power-up that gives a player a double-laser-shooting weapon.
    /// </summary>
    public class LaserPowerUp : PowerUp
    {
        #region Static Graphics Data


        /// <summary>
        /// Texture for the double-laser power-up.
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


        #region Initialization Methods


        /// <summary>
        /// Constructs a new DoubleLaserPowerUp.
        /// </summary>
        public LaserPowerUp()
            : base() { }


        #endregion


        #region Drawing Methods


        /// <summary>
        /// Draw the double-laser power-up.
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
        /// Defines the interaction between this power-up and a target GameObject
        /// when they touch.
        /// </summary>
        /// <param name="target">The GameObject that is touching this one.</param>
        /// <returns>True if the objects meaningfully interacted.</returns>
        public override bool Touch(GameObject target)
        {
            // if we hit a tank, give it the weapon
            Tank tank = target as Tank;
            if (tank != null)
            {
                // tank.Laser = new DoubleLaserWeapon(tank);
                throw new ApplicationException("Make a class to pick the next Laser weapon"); // TODO: Fix this!!!  
            }

            return base.Touch(target);
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
            texture = contentManager.Load<Texture2D>("PowerUp/LaserPowerUp");
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
