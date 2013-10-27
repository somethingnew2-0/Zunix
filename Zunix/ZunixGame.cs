using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;

using Zunix.Gameplay.Backgrounds;
using Zunix.Gameplay.PseudoEffects;
using Zunix.Gameplay.Weapons;
using Zunix.Gameplay;

using Zunix.ScreensManager;
using Zunix.Screens;

namespace Zunix
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ZunixGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        ScreenManager screenManager;

        //private Background roadBackground;

        //// So the inheirted input class can manipulate the tank
        //private Tank tank;
        //private Lasers lasers;
        //private AAExplosions explosions;
        //private AntiAircraft antiaircraft;
        //private Input tankInput;

        //private Rectangle viewportRect;

        public ZunixGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Components.Add(new GamerServicesComponent(this));

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //// TODO: Set up so it reads these things from XML
            //roadBackground = new Background();
            //tank = new Tank();
            //lasers = new Lasers();
            //explosions = new AAExplosions();
            //antiaircraft = new AntiAircraft();
            //tankInput = new Input(tank, lasers, antiaircraft);

            screenManager.AddScreen(new OldBackgroundScreen());
            screenManager.AddScreen(new OldMainMenuScreen());

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //// Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            //// Load the roadBackground
            //roadBackground.LoadContent(GraphicsDevice, spriteBatch, Content.Load<Texture2D>("Backgrounds/Tank-Game_Ground"));

            //// Load the badTank
            //tank.LoadContent(GraphicsDevice, spriteBatch, Content.Load<Texture2D>("Vehicles/Bottom/BadTankBottomSmall"), Content.Load<Texture2D>("Vehicles/Top/BadTankTopSmall"));

            //// Load the content for the tank input
            //tankInput.LoadContent(GraphicsDevice);

            //// Load the lasers
            //lasers.LoadContent(GraphicsDevice, spriteBatch, Content.Load<Texture2D>("Weapons/Lasers/LaserAnimation"), new Vector2(12f, 16f));

            //// Load the explosions
            //explosions.LoadContent(GraphicsDevice, spriteBatch, Content.Load<Texture2D>("Explosions/ExplosionAnimation"), new Vector2(60f, 80f));

            //// Load the antiaircraft
            //antiaircraft.LoadContent(GraphicsDevice, spriteBatch, Content.Load<Texture2D>("Weapons/AntiAircraft/AAAnimation"), new Vector2(12f, 16f));

            //viewportRect = new Rectangle(0, 0,
            //    graphics.Graphics240,
            //    graphics.Graphics320 + 10);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="elapsedTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            //// Update the background
            //roadBackground.Update(elapsedTime);

            //// Update the input and check to see if to quit
            //if (tankInput.Update(elapsedTime))
            //{
            //    this.Exit();
            //}
          
            //// Update the tank
            //tank.Update(elapsedTime);

            //lasers.Update(elapsedTime, tank);

            //explosions.Update(elapsedTime);

            //antiaircraft.Update(elapsedTime, tank, explosions);

            base.Update(gameTime);
        }




        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="elapsedTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.TransparentBlack);

            //spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            //roadBackground.Draw(elapsedTime);

            //tank.Draw(elapsedTime, spriteBatch);

            //lasers.Draw(elapsedTime);

            //explosions.Draw(elapsedTime);

            //antiaircraft.Draw(elapsedTime);

            //spriteBatch.End();

            base.Draw(gameTime);
        } 
    }
}
