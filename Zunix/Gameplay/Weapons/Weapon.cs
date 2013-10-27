using System;
using Microsoft.Xna.Framework;

using Zunix.Gameplay;
using Zunix.Gameplay.Vehicles;

namespace Zunix.Gameplay.Weapons
{
    public abstract class Weapon
    {
        protected Tank Owner { get; set; }

        /// <summary>
        /// The amount of time remaining before this weapon can fire again.
        /// </summary>
        public float TimeNextFire { get; set; }

        /// <summary>
        /// The minimum amount of time between each firing of this weapon.
        /// </summary>
        public float FireDelay { get; set; }

        /// <summary>
        /// The speed of the weapon.
        /// </summary>
        private Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        /// The Y Position of a "Graph" of the weapon (Good for the parabola in AntiAircraft).
        /// </summary>
        private float yGraph;
        public float YGraph
        {
            get { return yGraph; }
            set { yGraph = value; }
        }

        public Weapon(Tank owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            Owner = owner;
        }

        /// <summary>
        /// Update the weapon.
        /// </summary>
        /// <param name="elapsedTime">The float value.</param>
        public virtual void Update(float elapsedTime)
        {
            // count down to when the weapon can fire again
            if (TimeNextFire > 0f)
            {
                TimeNextFire = MathHelper.Max(TimeNextFire - elapsedTime, 0f);
            }
        }

        /// <summary>
        /// Fire the weapon in the direction given.
        /// </summary>
        /// <param name="direction">The direction that the weapon is firing in.</param>
        public virtual void Fire(Vector2 direction)
        {
            // if we can't fire yet, then we're done
            if (TimeNextFire > 0f)
            {
                return;
            }

            // the owner is no longer safe from damage
            Owner.Safe = false;

            // set the timer
            TimeNextFire = FireDelay;

            // create and spawn the projectile
            CreateProjectiles(direction);

            //// play the audio cue for firing
            //if (String.IsNullOrEmpty(fireCueName) == false)
            //{
            //    AudioManager.PlayCue(fireCueName);
            //}
        }


        /// <summary>
        /// Create and spawn the projectile(s) from a firing from this weapon.
        /// </summary>
        /// <param name="direction">The direction that the projectile will move.</param>
        protected abstract void CreateProjectiles(Vector2 direction);
    }
}