using Microsoft.Xna.Framework;

using Zunix.Gameplay.Vehicles;
using Zunix.Gameplay.Projectiles;

namespace Zunix.Gameplay.Weapons
{
    public class AntiAircraft : Weapon
    {
        public AntiAircraft(Tank owner)
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
            AntiAircraftProjectile projectile = new AntiAircraftProjectile(base.Owner, direction);
            base.Owner.Projectiles.Add(projectile);

            if (base.Owner.AASide == AASide.Right)
            {
                base.Owner.AASide = AASide.Left;
                projectile.Position = new Vector2(base.Owner.Position.X - (Tank.ChasisTextures[base.Owner.Variation].Width / 6),
                                            base.Owner.Position.Y + (Tank.ChasisTextures[base.Owner.Variation].Height / 4));
            }
            else if (base.Owner.AASide == AASide.Left)
            {
                base.Owner.AASide = AASide.Right;
                projectile.Position = new Vector2(base.Owner.Position.X + (Tank.ChasisTextures[base.Owner.Variation].Width / 6),
                                            base.Owner.Position.Y + (Tank.ChasisTextures[base.Owner.Variation].Height / 4));
            }
        }
    }
    //    private List<Weapon> aas = new List<Weapon>();

    //    // Rectangle the size of the viewport to check if a laser is outside and alive
    //    private Rectangle viewportRect;

    //    private const int aaRange = 175;
    //    private const int maxAA = 4;
    //    private const float aaSpeed = 0.25f;
    //    private const int lasersPerAA = 5;

    //    private float millisecbetweenshots = 250f;

    //    private float time = 0f;

    //    // Figures when to shoot an AA
    //    private int numOfLasersShot = 0;

    //    // For the random rotation of the explosion
    //    private Random randomdegrees = new Random();

    //    public List<Weapon> AntiAircraftList
    //    {
    //        get { return aas; }
    //        set { aas = value; }
    //    }

    //    public AntiAircraft()
    //    {
    //        for (int i = 0; i < maxAA; i++)
    //        {
    //            aas.Add(new Weapon());
    //        }
    //    }

    //    public static void LoadContent(SpriteBatch spriteBatch, Texture2D antiAircraftAnimation, Vector2 sourceSize)
    //    {
    //        foreach (Weapon aa in aas)
    //        {
    //            aa.LoadContent(spriteBatch, antiAircraftAnimation, sourceSize);
    //        }

    //        // Create the viewportRect with a buffer for the antiaircraft shells
    //        viewportRect = new Rectangle(-20, 0,
    //            240 + 40,
    //            320 + 20);

    //    }
    //    public void Update(float elapsedTime, Vehicle tank, AAExplosions explosion)
    //    {
    //        // Checks to fire or not
    //        time += elapsedTime.ElapsedGameTime.Milliseconds;
    //        if (time > millisecbetweenshots)
    //        {
    //            FireAA(tank, elapsedTime);
    //            // Reset the time
    //            time = 0f;
    //        }

    //        // Check to see if the antiaircraft are still alive
    //        UpdateAA(elapsedTime, tank, explosion);

    //        foreach (Weapon aa in aas)
    //        {
    //            aa.Update(elapsedTime);
    //        }
    //    }
    //    public void Draw(float elapsedTime)
    //    {
    //        foreach (Weapon aa in aas)
    //        {
    //            aa.Draw();
    //        }
    //    }

    //    public void FireAA(Vehicle tank, float elapsedTime)
    //    {
    //        numOfLasersShot++;

    //        if (lasersPerAA == numOfLasersShot)
    //        {
    //            // TODO: Play the AAShot Sound

    //            foreach (Weapon aa in aas)
    //            {
    //                if (!aa.Alive)
    //                {
    //                    aa.Alive = true;

    //                    if (tank.AASide == AASide.Right)
    //                    {
    //                        tank.AASide = AASide.Left;
    //                        aa.Position = new Vector2(tank.Position.X - (tank.ActualSize.X / 6),
    //                                                    tank.Position.Y + (tank.ActualSize.Y / 4));
    //                    }
    //                    else if (tank.AASide == AASide.Left)
    //                    {
    //                        tank.AASide = AASide.Right;
    //                        aa.Position = new Vector2(tank.Position.X + (tank.ActualSize.X / 6),
    //                                                    tank.Position.Y + (tank.ActualSize.Y / 4));
    //                    }
    //                    aa.IntialPosition = new Vector2(aa.Position.X, aa.Position.Y);

    //                    numOfLasersShot = 0;

    //                    return;

    //                }
    //            }
    //        }
    //    }

    //    private void UpdateAA(float elapsedTime, Vehicle tank, AAExplosions explosion)
    //    {
    //        foreach (Weapon aa in aas)
    //        {
    //            if (aa.Alive)
    //            {
    //                if (tank.AASide == AASide.Left)
    //                {
    //                    // This moves the Shell in a parabola in the form X = -1.6Y^2 + 8.0Y
    //                    float squaredY = (float)(-1.6f) * (float)(Math.Pow(aa.YGraph, 2));
    //                    float regularY = 8f * aa.YGraph;
    //                    aa.Position = new Vector2(aa.IntialPosition.X - aa.YGraph, aa.IntialPosition.Y + squaredY + regularY);

    //                }
    //                else if (tank.AASide == AASide.Right)
    //                {
    //                    float squaredY = (float)(-1.6f) * (float)(Math.Pow(aa.YGraph, 2));
    //                    float regularY = 8f * aa.YGraph;
    //                    // This moves the Shell in an parabola in the form X = -1.6Y^2 + 8.0Y
    //                    aa.Position = new Vector2(aa.IntialPosition.X + aa.YGraph, aa.IntialPosition.Y + squaredY + regularY);
    //                }

    //                aa.YGraph += aaSpeed;

    //                //Check if the laser is contained within
    //                //the screen-sized rectangle. If not,
    //                //kill the laser.
    //                if (!viewportRect.Contains(new Point(
    //                    (int)(aa.Position.X), (int)(aa.Position.Y))))
    //                {
    //                    aa.Alive = false;
    //                    aa.YGraph = aaSpeed;
    //                }
    //                if (aa.IntialPosition.Y - aa.Position.Y >= aaRange)
    //                {
    //                    aa.Alive = false;
    //                    aa.YGraph = aaSpeed;

    //                    foreach (Explosion aaex in explosion.ExplosionsList)
    //                    {
    //                        if (!aaex.Alive == true)
    //                        {
    //                            // TODO: Play the antiaircraft sound

    //                            aaex.Alive = true;

    //                            // If there is an alive aaex set a random rotation before making it visible
    //                            aaex.Rotation = MathHelper.ToRadians(randomdegrees.Next(360));

    //                            aaex.Position = aa.Position;

    //                            // Make them visible
    //                            aaex.Update(elapsedTime);

    //                            return;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    //public enum AA
    //{
    //    Shell,
    //    Bomb,
    //    HeavyBomb
    //}

    public enum AASide
    {
        Left,
        Right
    }
}
