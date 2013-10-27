using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System;

namespace ACV
{
    public class Tank : GameObject
    {

        /// <summary>
        /// The full speed possible for the tank.
        /// </summary>
        const float fullSpeed = 320f;

        /// <summary>
        /// The amount of drag applied to velocity per second, 
        /// as a percentage of velocity.
        /// </summary>
        const float dragPerSecond = 0.9f;

        /// <summary>
        /// The number of radians that the tank can turn in a second at full left-stick.
        /// </summary>
        const float rotationRadiansPerSecond = 0.5f;

        /// <summary>
        /// The maximum length of the velocity vector on a tank.
        /// </summary>
        const float velocityMaximum = 320f;

        /// <summary>
        /// The amplitude of the shield pulse
        /// </summary>
        const float shieldPulseAmplitude = 0.15f;

        /// <summary>
        /// The rate of the shield pulse.
        /// </summary>
        const float shieldPulseRate = 0.2f;

        /// <summary>
        /// The maximum value of the "safe" timer.
        /// </summary>
        const float safeTimerMaximum = 4f;

        /// <summary>
        /// The maximum amount of life that a ship can have.
        /// </summary>
        const float lifeMaximum = 25f;

        /// <summary>
        /// The value of the spawn timer set when the ship dies.
        /// </summary>
        const float respawnTimerOnDeath = 5f;

        /// <summary>
        /// The number of variations in textures for tanks.
        /// </summary>
        const int variations = 1;

        #region Static Graphics Data

        private static RenderTarget2D tankRender;

        /// <summary>
        /// The primary tank textures.
        /// </summary>
        private static Texture2D[] chasisTextures = new Texture2D[variations];

        /// <summary>
        /// The overlay tank textures, which get tinted.
        /// </summary>
        private static Texture2D[] turretTextures = new Texture2D[variations];

        /// <summary>
        /// The tank shield texture.
        /// </summary>
        private static Texture2D invincibilityTexture;

        /// <summary>
        /// RenderTarget Texture for the Tank.
        /// </summary>
        private Texture2D texture;

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

        public static Texture2D[] ChasisTextures
        {
            get { return chasisTextures; }
            set { chasisTextures = value; }
        }

        public static Texture2D[] TurretTextures
        {
            get { return turretTextures; }
            set { turretTextures = value; }
        }

        #region Input Data


        /// <summary>
        /// The current player input for the tank.
        /// </summary>
        private TankInput tankInput;
        public TankInput TankInput
        {
            get { return tankInput; }
            set { tankInput = value; }
        }


        #endregion

        /// <summary>
        /// The variation of this tank.
        /// </summary>
        private int variation = 0;
        public int Variation
        {
            get { return variation; }
            set
            {
                if ((value < 0) || (value >= variations))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                variation = value;
            }
        }


        /// <summary>
        /// The score for this tank.
        /// </summary>
        private int score = 0;
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        /// <summary>
        /// The amount of damage that the tank can take before exploding.
        /// </summary>
        private float life;
        public float Life
        {
            get { return life; }
            set { life = value; }
        }

        public Laser Laser { get; set; }

        public AntiAircraft AntiAircraft { get; set; }

        public Missile Missile { get; set; }

        public AASide AASide { get; set; }

        ///// <summary>
        ///// The tank's primary weapon.
        ///// </summary>
        //private Weapon weapon;
        //public Weapon Weapon
        //{
        //    get { return weapon; }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            throw new ArgumentNullException("value");
        //        }
        //        weapon = value;
        //    }
        //}

        /// <summary>
        /// All of the projectiles fired by this tank.
        /// </summary>
        private BatchRemovalCollection<Projectile> projectiles =
            new BatchRemovalCollection<Projectile>();
        public BatchRemovalCollection<Projectile> Projectiles
        {
            get { return projectiles; }
        }

        ///// <summary>
        ///// The strength of the invincibilityShield.
        ///// </summary>
        //private float invincibilityShield;
        //public float InvincibilityShield
        //{
        //    get { return invincibilityShield; }
        //    set { invincibilityShield = value; }
        //}

        /// <summary>
        /// Timer for how long the invincibilityShield lasts.
        /// </summary>
        private float invincibilityShieldTimer;

        /// <summary>
        /// Timer for how long the player is safe after spawning.
        /// </summary>
        private float safeTimer;
        public bool Safe
        {
            get { return (safeTimer > 0f); }
            set
            {
                if (value)
                {
                    safeTimer = safeTimerMaximum;
                }
                else
                {
                    safeTimer = 0f;
                }
            }
        }

        /// <summary>
        /// The tint of the tank.
        /// </summary>
        private Color color = Color.White;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// The amount of time left before respawning the tank.
        /// </summary>
        private float respawnTimer;
        public float RespawnTimer
        {
            get { return respawnTimer; }
            set { respawnTimer = value; }
        }

        /// <summary>
        /// The last object to damage the tank.
        /// </summary>
        private GameObject lastDamagedBy = null;
        public GameObject LastDamagedBy
        {
            get { return lastDamagedBy; }
        }


        public Tank()
        {
            // Create the RenderTarget and find the origin of it
            tankRender = new RenderTarget2D(ChasisTextures[Variation].GraphicsDevice, (int)ChasisTextures[variation].Width + 10, (int)ChasisTextures[variation].Height + 10, 1, SurfaceFormat.Color);

            AASide = AASide.Left;

            base.Position = new Vector2(90, 100);
            base.Alive = true;

            base.Velocity = new Vector2(1.5f);

            Laser = new GreenLaser(this);
        }

        #region Updating Methods


        /// <summary>
        /// Update the tank.
        /// </summary>
        /// <param name="elapsedTime">The amount of elapsed time, in seconds.</param>
        public override void Update(float elapsedTime)
        {
            // calculate the current forward vector
            Vector2 forward = new Vector2((float)Math.Sin(Rotation),
                -(float)Math.Cos(Rotation));
            Vector2 right = new Vector2(-forward.Y, forward.X);

            // calculate the new forward vector with the left stick
            tankInput.LeftStick.Y *= -1f;
            if (tankInput.LeftStick.LengthSquared() > 0f)
            {
                // change the direction
                Vector2 wantedForward = Vector2.Normalize(tankInput.LeftStick);
                float angleDiff = (float)Math.Acos(
                    Vector2.Dot(wantedForward, forward));
                float facing = (Vector2.Dot(wantedForward, right) > 0f) ?
                    1f : -1f;
                if (angleDiff > 0.001f)
                {
                    Rotation += Math.Min(angleDiff, facing * elapsedTime *
                        rotationRadiansPerSecond);
                }

                // add velocity
                Velocity += tankInput.LeftStick * (elapsedTime * fullSpeed);
                if (Velocity.Length() > velocityMaximum)
                {
                    Velocity = Vector2.Normalize(Velocity) *
                        velocityMaximum;
                }
            }
            tankInput.LeftStick = Vector2.Zero;

            // apply drag to the velocity
            Velocity -= Velocity * (elapsedTime * dragPerSecond);
            if (Velocity.LengthSquared() <= 0f)
            {
                Velocity = Vector2.Zero;
            }

            // TODO: Fire weapons here

            //// check for firing with the right stick
            //tankInput.RightStick.Y *= -1f;
            //if (tankInput.RightStick.LengthSquared() > fireThresholdSquared)
            //{
            //    weapon.Fire(Vector2.Normalize(tankInput.RightStick));
            //}
            //tankInput.RightStick = Vector2.Zero;

            //// check for laying mines
            //if (tankInput.MineFired)
            //{
            //    // fire behind the tank
            //    mineWeapon.Fire(-forward);
            //}
            //tankInput.MineFired = false;

            // recharge the shields
            if (invincibilityShieldTimer > 0f)
            {
                invincibilityShieldTimer = Math.Max(invincibilityShieldTimer - elapsedTime,
                    0f);
            }
            else
            {
                invincibilityShieldTimer = 0f;
            }

            //// update the radius based on the shield
            //radius = (invincibilityShield > 0f) ? 24f : 20f;

            // update the weapons
            if (Laser != null)
            {
                Laser.Update(elapsedTime);
            }
            if (AntiAircraft != null)
            {
                AntiAircraft.Update(elapsedTime);
            }
            if (Missile != null)
            {
                Missile.Update(elapsedTime);
            }

            // decrement the safe timer
            if (safeTimer > 0f)
            {
                safeTimer = Math.Max(safeTimer - elapsedTime, 0f);
            }

            // update the projectiles
            foreach (Projectile projectile in projectiles)
            {
                if (projectile.Alive)
                {
                    projectile.Update(elapsedTime);
                }
                else
                {
                    projectiles.QueuePendingRemoval(projectile);
                }
            }
            projectiles.ApplyPendingRemovals();

            base.Update(elapsedTime);
        }


        #endregion

        public override void Draw(float elapsedTime, SpriteBatch spriteBatch, Texture2D sprite, Color color)
        {
            texture = CreateVehicle(elapsedTime, spriteBatch, chasisTextures[variation], turretTextures[variation]);

            base.Draw(elapsedTime, spriteBatch, texture, color);
        }

        private Texture2D CreateVehicle(float elapsedTime, SpriteBatch spriteBatch, Texture2D chasisTexture, Texture2D turretTexture)
        {
            // End the current call
            spriteBatch.End();

            // Set the RenderTarget we made in the device Device
            chasisTexture.GraphicsDevice.SetRenderTarget(0, tankRender);

            // Clear the GraphicsDevice which had the RenderTarget so it's usable and set the RenderTarget to a clear alpha.  
            chasisTexture.GraphicsDevice.Clear(ClearOptions.Target, new Color(0, 0, 0, 0), 0, 0);

            // Begin the draw on the RenderTarget
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            // Draw the bottom of the vehicle
            base.Draw(elapsedTime, spriteBatch, chasisTexture, Color.White);

            // Draw the top of the vehicle
            // Subtract 8 pixels from the middle to make the vehicle look better.
            base.Draw(elapsedTime, spriteBatch, turretTexture, Color.White);

            // End the draw sequence
            spriteBatch.End();

            // Flush the GraphicsDevice of RenderTargets
            chasisTexture.GraphicsDevice.SetRenderTarget(0, null);

            // Start another call
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            // Return the Texture2D of the FullTank.
            return tankRender.GetTexture();
        }

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

            // load each tank's texture
            for (int i = 0; i < variations; i++)
            {
                chasisTextures[i] = contentManager.Load<Texture2D>(
                    "Vehicles/Bottom/TankChasis" + i.ToString());
                turretTextures[i] = contentManager.Load<Texture2D>(
                    "Vehicles/Top/TankTurret" + i.ToString());
            }
        }

        /// <summary>
        /// Unload all of the static graphics content for this class.
        /// </summary>
        public static void UnloadContent()
        {
            for (int i = 0; i < variations; i++)
            {
                chasisTextures[i] = turretTextures[i] = null;
            }

            invincibilityTexture = null;
        }

//        private List<Vehicle> players = new List<Vehicle>();

//        public List<Vehicle> PlayersList
//        { 
//            get { return players;}
//            set { players = value;}
//        }


//        public Tank()
//        {
//            players.Add(new Vehicle());

//            // TODO: Just maybe read from Xml 
//            foreach (Vehicle player in players)
//            {
//                player.LaserSelect = Laser.Green;
//                player.AASelect = AA.Shell;
//                player.AASide = AASide.Left;

//                player.Position = new Vector2(90, 100);
//                player.Alive = true;

//                player.Speed = 1.5f;
//            }

//            //full.Position = new Vector2(90, 200);
//            //bottom.Alive = true;
//            //top.Alive = true;
//            //full.Alive = true;
//        }
//        public void LoadContent(GraphicsDevice device, SpriteBatch spriteBatch, Texture2D chasisTexture, Texture2D turretTexture)
//        {

//            foreach (Vehicle player in players)
//            {
//                player.LoadContent(device, spriteBatch, chasisTexture, turretTexture);
//            }

//            //// Create the RenderTarget and find the origin of it
//            //tankRender = new RenderTarget2D(device, (int)bottom.ActualSize.Height + 10, (int)bottom.ActualSize.Height + 10, 1, SurfaceFormat.Color);
//            //tankRenderOrigin = new Vector2(tankRender.Width / 2, tankRender.Height / 2);

//        }
//        public void Update(float elapsedTime)
//        {
//            foreach (Vehicle player in players)
//            {
//                player.Update(elapsedTime);
//            }

//            //// Update the bottom and top of the tank tank
//            //bottom.Update(elapsedTime);
//            //top.Update(elapsedTime);

//            //// Create the tank once again with a posible new rotation
//            //full.Sprite = CreateTank(full.SpriteBatch);

//            //// Update the full tank
//            //full.Update(elapsedTime);
//        }
//        public void Draw(float elapsedTime, SpriteBatch spriteBatch)
//        {
//            foreach (Vehicle player in players)
//            {
//                player.Draw();
//            }

//            //// Draw the full tank
//            //full.Draw(elapsedTime, spriteBatch);
//        }


    }
}
