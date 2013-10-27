//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//using Zunix.Gameplay;
//using Zunix.Gameplay.Weapons;

// TODO: Delete me

//namespace Zunix.Gameplay.Vehicles
//{
//    public class Vehicle : GameObject
//    {
//        private Texture2D sprite;

//        private Chasis chasis;
//        private Turret turret;

//        private RenderTarget2D tankRender;


//        public override float Rotation
//        {
//            get { return chasis.Rotation; }
//            set { chasis.Rotation = value; }
//        }

//        public Laser Laser { get; set; }

//        public AntiAircraft AntiAircraft { get; set; }

//        public Missile Missile { get; set; }

//        private AASide aaSide;
//        public AASide AASide
//        {
//            get { return aaSide; }
//            set { aaSide = value; }
//        }

//        public Vehicle()
//        {
//            chasis = new Chasis();
//            turret = new Turret();

//            chasis.Alive = true;
//            turret.Alive = true;

//        }

//        //public void LoadContent(GraphicsDevice device, SpriteBatch spriteBatch, Texture2D chasisTexture, Texture2D turretTexture)
//        //{
//        //    this.device = device;

//        //    chasis.LoadContent(spriteBatch, chasisTexture);
//        //    turret.LoadContent(spriteBatch, turretTexture);

//        //    // Create the RenderTarget and find the origin of it
//        //    tankRender = new RenderTarget2D(device, (int)chasis.ActualSize.Y + 10, (int)chasis.ActualSize.Y + 10, 1, SurfaceFormat.Color);

//        //    base.LoadContent(spriteBatch, tankRender.GetTexture());

//        //    // Doesn't need to be loaded as XML
//        //    chasis.Position = Origin;
//        //    // Subtract 8 pixels so the turret looks better
//        //    turret.Position = new Vector2(Origin.X, Origin.Y - 8);

//        //}

//        public override void Update(float elapsedTime)
//        {
//            chasis.Update(elapsedTime);
//            turret.Update(elapsedTime);

//            base.Update(elapsedTime);
//        }

//        public void Draw(float elapsedTime, SpriteBatch spriteBatch, Texture2D chasisTexture, Texture2D turretTexture, Color color)
//        {
//            sprite = CreateVehicle(elapsedTime, spriteBatch, chasisTexture, turretTexture);

//            base.Draw(elapsedTime, spriteBatch, sprite, color);
//        }

//        private Texture2D CreateVehicle(float elapsedTime, SpriteBatch spriteBatch, Texture2D chasisTexture, Texture2D turretTexture)
//        {
//            // End the current call
//            spriteBatch.End();

//            // Set the RenderTarget we made in the device Device
//            chasisTexture.GraphicsDevice.SetRenderTarget(0, tankRender);

//            // Clear the GraphicsDevice which had the RenderTarget so it's usable and set the RenderTarget to a clear alpha.  
//            chasisTexture.GraphicsDevice.Clear(ClearOptions.Target, new Color(0, 0, 0, 0), 0, 0);

//            // Begin the draw on the RenderTarget
//            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

//            // Draw the bottom of the vehicle
//            chasis.Draw(elapsedTime, spriteBatch, chasisTexture, Color.White);

//            // Draw the top of the vehicle
//            // Subtract 8 pixels from the middle to make the vehicle look better.
//            turret.Draw(elapsedTime, spriteBatch, turretTexture, Color.White);

//            // End the draw sequence
//            spriteBatch.End();

//            // Flush the GraphicsDevice of RenderTargets
//            chasisTexture.GraphicsDevice.SetRenderTarget(0, null);

//            // Start another call
//            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

//            // Return the Texture2D of the FullTank.
//            return tankRender.GetTexture();
//        }
//    }

//}