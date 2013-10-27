using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ACV
{
    public class Laser : Weapon
    {
        public Laser(Tank owner)
            : base(owner)
        {
        }

        /// <summary>
        /// Create and spawn the projectile(s) from a firing from this weapon.
        /// </summary>
        /// <param name="direction">The direction that the projectile will move.</param>
        protected override void CreateProjectiles(Vector2 direction)
        {
            // create the new projectile
            LaserProjectile projectile = new LaserProjectile(Owner, direction);
            
        }
    }
}