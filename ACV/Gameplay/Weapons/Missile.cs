using System;
using Microsoft.Xna.Framework;

namespace ACV
{
    public class Missile : Weapon
    {
        public Missile(Tank owner)
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
            MissileProjectile projectile = new MissileProjectile(base.Owner, direction);
            // projectile.Initialize();
            base.Owner.Projectiles.Add(projectile);
        }
    }
}
