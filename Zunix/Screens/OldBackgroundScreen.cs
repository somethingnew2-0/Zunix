using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Zunix.ScreensManager;

namespace Zunix.Screens
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    /// <remarks>
    /// This public class is somewhat similar to one of the same name in the 
    /// GameStateManagement sample.
    /// </remarks>
    public class OldBackgroundScreen : GameScreen
    {
        #region Constants


        #endregion


        #region Graphics Data

        /// <summary>
        /// The application's background texture.
        /// </summary>
        private Texture backgoundTexture;

        /// <summary>
        /// The application's title texture.
        /// </summary>
        private Texture2D titleTexture;


        #endregion


        #region Initialization Methods


        /// <summary>
        /// Construct a new BackgroundScreen object.
        /// </summary>
        public OldBackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.0);
            TransitionOffTime = TimeSpan.FromSeconds(1.0);
        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the ScreenManager, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            // load the title texture
            titleTexture = ScreenManager.Content.Load<Texture2D>("Title/Title");

            base.LoadContent();
        }


        /// <summary>
        /// Release graphics data.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }


        #endregion


        #region Updating Methods


        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }


        #endregion


        #region Drawing Methods


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // draw the title texture
            if (titleTexture != null)
            {
                Vector2 titlePosition = new Vector2(
                    ScreenManager.TitleSafeArea.X +
                       (ScreenManager.TitleSafeArea.Width - titleTexture.Width) / 2f,
                    ScreenManager.TitleSafeArea.Y +
                       ScreenManager.TitleSafeArea.Height * 0.05f);
                titlePosition.Y -= (float)Math.Pow(TransitionPosition, 2) *
                    titlePosition.Y;
                ScreenManager.SpriteBatch.Begin();
                ScreenManager.SpriteBatch.Draw(titleTexture, titlePosition,
                    new Color(255, 255, 255, TransitionAlpha));
                ScreenManager.SpriteBatch.End();
            }
        }


        #endregion
    }
}
