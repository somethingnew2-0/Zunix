using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Zunix;

namespace Zunix.ScreensManager
{
    /// <summary>
    /// Helper for reading input from keyboard and gamepad. This public class tracks
    /// the current and previous state of both input devices, and implements query
    /// properties for high level input actions such as "move up through the menu"
    /// or "pause the game".
    /// </summary>
    /// <remarks>
    /// This public class is similar to one in the GameStateManagement sample.
    /// </remarks>
    public class OldInputState
    {
        #region Fields

        public ZunePadState CurrentZunePadState;

        public ZunePadState LastZunePadState;

        #endregion

        #region Properties


        /// <summary>
        /// Checks for a "menu up" input action (on either keyboard or gamepad).
        /// </summary>
        public bool MenuUp
        {
            get
            {
                return (CurrentZunePadState.DPad.Up == ButtonState.Pressed &&
                        LastZunePadState.DPad.Up == ButtonState.Released) ||
                       (CurrentZunePadState.Flick.Y > 0 &&
                        LastZunePadState.Flick.Y <= 0);
            }
        }


        /// <summary>
        /// Checks for a "menu down" input action (on either keyboard or gamepad).
        /// </summary>
        public bool MenuDown
        {
            get
            {
                return (CurrentZunePadState.DPad.Down == ButtonState.Pressed &&
                        LastZunePadState.DPad.Down == ButtonState.Released) ||
                       (CurrentZunePadState.Flick.Y < 0 &&
                        LastZunePadState.Flick.Y >= 0);
            }
        }


        /// <summary>
        /// Checks for a "menu select" input action (on either keyboard or gamepad).
        /// </summary>
        public bool MenuSelect
        {
            get
            {
                return (CurrentZunePadState.A == ButtonState.Pressed &&
                        LastZunePadState.A == ButtonState.Released) ||
                        (CurrentZunePadState.PlayButton == ButtonState.Pressed &&
                        LastZunePadState.PlayButton == ButtonState.Released);
            }
        }


        /// <summary>
        /// Checks for a "menu cancel" input action (on either keyboard or gamepad).
        /// </summary>
        public bool MenuCancel
        {
            get
            {
                return (CurrentZunePadState.BackButton == ButtonState.Pressed &&
                        LastZunePadState.BackButton == ButtonState.Released);
            }
        }


        /// <summary>
        /// Checks for a "pause the game" input action (on either keyboard or gamepad).
        /// </summary>
        public bool PauseGame
        {
            get
            {
                return (CurrentZunePadState.BackButton == ButtonState.Pressed &&
                        LastZunePadState.BackButton == ButtonState.Released);
            }
        }


        /// <summary>
        /// Checks for a "mark ready" input action (on either keyboard or gamepad).
        /// </summary>
        public bool MarkReady
        {
            get
            {
                return (CurrentZunePadState.A == ButtonState.Pressed &&
                        LastZunePadState.A == ButtonState.Released) ||
                        (CurrentZunePadState.PlayButton == ButtonState.Pressed &&
                        LastZunePadState.PlayButton == ButtonState.Released);
            }
        }


        #endregion

        #region Methods


        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        public void Update()
        {
            LastZunePadState = CurrentZunePadState;

            CurrentZunePadState = ZunePad.GetState();
        }


        #endregion
    }
}
