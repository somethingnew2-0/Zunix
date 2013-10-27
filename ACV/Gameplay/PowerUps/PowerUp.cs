using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACV
{
    /// <summary>
    /// Base public class for all power-ups that exist in the game.
    /// </summary>
    abstract public class PowerUp : GameObject
    {
        #region Constant Data


        /// <summary>
        /// The speed of the rotation of the power-up, in radians/sec.
        /// </summary>
        const float rotationSpeed = 2f;

        /// <summary>
        /// The amplitude of the pulse
        /// </summary>
        const float pulseAmplitude = 0.1f;

        /// <summary>
        /// The rate of the pulse.
        /// </summary>
        const float pulseRate = 0.1f;


        public const float PowerUpRadius = 20f;

        #endregion


        #region Graphics Data


        /// <summary>
        /// The time accumulator for the power-up pulse.
        /// </summary>
        private float pulseTime = 0f;


        #endregion


        #region Initialization Methods


        /// <summary>
        /// Constructs a new power-up.
        /// </summary>
        protected PowerUp()
            : base()
        {
        }


        ///// <summary>
        ///// Initialize the power-up to it's default gameplay states.
        ///// </summary>
        //public override void Initialize()
        //{
        //    if (!active)
        //    {
        //        // play the spawn sound effect
        //        AudioManager.PlayCue("powerUpSpawn");
        //    }

        //    base.Initialize();
        //}


        #endregion


        #region Drawing Methods

        /// <summary>
        /// Draw the triple-laser power-up.
        /// </summary>
        /// <param name="elapsedTime">The amount of elapsed time, in seconds.</param>
        /// <param name="spriteBatch">The SpriteBatch object used to draw.</param>
        public abstract void Draw(float elapsedTime, SpriteBatch spriteBatch);


        /// <summary>
        /// Draw the power-up.
        /// </summary>
        /// <param name="elapsedTime">The amount of elapsed time, in seconds.</param>
        /// <param name="spriteBatch">The SpriteBatch object used to draw.</param>
        /// <param name="sprite">The texture used to draw this object.</param>
        /// <param name="color">The color of the sprite, ignored here.</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch,
            Texture2D sprite, Color color)
        {
            // update the rotation
            base.Rotation += rotationSpeed * elapsedTime;

            base.Draw(elapsedTime, spriteBatch, sprite, color);
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
            // if it touched a tank, then create a pseudosystem and play a sound
            Tank tank = target as Tank;
            if (tank != null)
            {
                // TODO: Play the "power-up picked up" cue
                
                // kill the power-up
                Kill(target, false);

                // the tank keeps going as if it didn't hit anything
                return false;
            }

            return base.Touch(target);
        }
        
        #endregion
    }
}
