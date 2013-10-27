using System;
using System.Collections.Generic;
using System.Text;

using Zunix.ScreensManager;
using Microsoft.Xna.Framework;

namespace Zunix.Screens
{
    class MultiplayerMenuScreen : MenuScreen
    {
        #region Initialization
        
        /// <summary>
        /// Constructs a new SinglePlayerMenu object.
        /// </summary>
        public MultiplayerMenuScreen()
            : base("Multiplayer")
        {
            // set the transition times
            TransitionOnTime = TimeSpan.FromSeconds(1.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.0);


            MenuEntries.Clear();
            MenuEntries.Add(new MenuEntry("Campaign"));
            MenuEntries.Add(new MenuEntry("Free Play"));
            MenuEntries.Add(new MenuEntry("Tutorial"));
            MenuEntries.Add(new MenuEntry("Scores"));
            MenuEntries.Add(new MenuEntry("Back"));
        }


        #endregion


        #region Updating Methods


        /// <summary>
        /// Updates the screen. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }


        /// <summary>
        /// Responds to user menu selections.
        /// </summary>
        protected override void OnSelectEntry(int entryIndex)
        {
            switch (entryIndex)
            {
                case 0: // Campaign
                    ScreenManager.AddScreen(new SinglePlayerMenuScreen());
                    break;

                case 1: // Free play
                    break;

                case 2: // Tutorial
                    break;

                case 3: // Scores
                    break;

                case 4: // Back
                    OnCancel();
                    break;
            }
            
        }

        protected override void OnCancel()
        {
            ExitScreen();
            ScreenManager.AddScreen(new OldMainMenuScreen());
        }

        #endregion
    }
}