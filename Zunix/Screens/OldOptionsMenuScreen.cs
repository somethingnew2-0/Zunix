using System;
using System.Collections.Generic;
using System.Text;

using Zunix.ScreensManager;
using Microsoft.Xna.Framework;

namespace Zunix.Screens
{
    class OldOptionsMenuScreen : MenuScreen
    {
        #region Initialization
        enum Difficulty { Easy, Medium, Hard };

        private bool touchpad;
        private Difficulty diff;

        /// <summary>
        /// Constructs a new SinglePlayerMenu object.
        /// </summary>
        public OldOptionsMenuScreen()
            : base()
        {
            // set the transition times
            TransitionOnTime = TimeSpan.FromSeconds(1.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.0);

            // Load from XML
            touchpad = true;
            diff = Difficulty.Medium;

            MenuEntries.Clear();
            MenuEntries.Add(touchpad ? "TouchPad - On" : "TouchPad - Off");
            MenuEntries.Add("Difficulty - " + diff.ToString());
            MenuEntries.Add("Back");
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

            base.Update(elapsedTime, otherScreenHasFocus, coveredByOtherScreen);
        }


        /// <summary>
        /// Responds to user menu selections.
        /// </summary>
        protected override void OnSelectEntry(int entryIndex)
        {
            switch (entryIndex)
            {
                case 0: // TouchPad
                    touchpad = !touchpad;
                    MenuEntries[0] = touchpad ? "TouchPad - On" : "TouchPad - Off";
                    break;

                case 1: // Difficulty
                    diff = diff == Difficulty.Hard ? Difficulty.Easy : (Difficulty)(diff + 1);
                    MenuEntries[1] = "Difficulty - " + diff.ToString();
                    break;

                case 2: // Back
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