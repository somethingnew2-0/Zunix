using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;

using Zunix.Gameplay;
using Zunix.ScreensManager;
using Zunix.Gameplay.Vehicles;

namespace Zunix.Screens
{
    /// <summary>
    /// This screen implements the actual game logic.
    /// </summary>
    /// <remarks>
    /// This public class is somewhat similar to one of the same name in the 
    /// GameStateManagement sample.
    /// </remarks>
    public class OldGameplayScreen : GameScreen
    {
        #region Gameplay Data

        /// <summary>
        /// The primary gameplay object.
        /// </summary>
        private World world;

        /// <summary>
        /// The tank for the local player.
        /// </summary>
        private Tank localTank;

        /// <summary>
        /// The game-winner text.
        /// </summary>
        private string winnerString = String.Empty;

        /// <summary>
        /// The position of the game-winner text.
        /// </summary>
        private Vector2 winnerStringPosition;


        #endregion


        #region Graphics Data


        ///// <summary>
        ///// The bloom component, applied to part of the game world.
        ///// </summary>
        //private BloomComponent bloomComponent;

        ///// <summary>
        ///// The starfield, rendering behind the game.
        ///// </summary>
        //private Starfield starfield;


        #endregion


        #region Networking Data


        /// <summary>
        /// The network session used in this game.
        /// </summary>
        private NetworkSession networkSession;

        /// <summary>
        /// Event handler for the session-ended event.
        /// </summary>
        EventHandler<NetworkSessionEndedEventArgs> sessionEndedHandler;

        /// <summary>
        /// Event handler for the game-ended event.
        /// </summary>
        EventHandler<GameEndedEventArgs> gameEndedHandler;

        /// <summary>
        /// Event handler for the gamer-left event.
        /// </summary>
        EventHandler<GamerLeftEventArgs> gamerLeftHandler;


        #endregion


        #region Initialization Methods


        /// <summary>
        /// Construct a new GameplayScreen object.
        /// </summary>
        /// <param name="networkSession">The network session for this game.</param>
        /// <param name="world">The primary gameplay object.</param>
        public OldGameplayScreen(NetworkSession networkSession, World world)
        {
            // safety-check the parameters
            if (networkSession == null)
            {
                throw new ArgumentNullException("networkSession");
            }
            if (world == null)
            {
                throw new ArgumentNullException("world");
            }

            // apply the parameters
            this.networkSession = networkSession;
            this.world = world;

            // set up the network events
            sessionEndedHandler = new EventHandler<NetworkSessionEndedEventArgs>(
                networkSession_SessionEnded);
            networkSession.SessionEnded += sessionEndedHandler;
            gameEndedHandler = new EventHandler<GameEndedEventArgs>(
                networkSession_GameEnded);
            networkSession.GameEnded += gameEndedHandler;
            gamerLeftHandler = new EventHandler<GamerLeftEventArgs>(
                networkSession_GamerLeft);
            networkSession.GamerLeft += gamerLeftHandler;


            // cache the local player's tank object
            if (networkSession.LocalGamers.Count > 0)
            {
                PlayerData playerData = networkSession.LocalGamers[0].Tag as PlayerData;
                if (playerData != null)
                {
                    localTank = playerData.Tank;
                }
            }

            // set the transition times
            TransitionOnTime = TimeSpan.FromSeconds(1.0);
            TransitionOffTime = TimeSpan.FromSeconds(1.0);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            //// create and add the bloom effect
            //bloomComponent = new BloomComponent(ScreenManager.Game);
            //bloomComponent.Settings = BloomSettings.PresetSettings[0];
            //ScreenManager.Game.Components.Add(bloomComponent);
            //bloomComponent.Initialize();
            //// do not automatically draw the bloom component
            //bloomComponent.Visible = false;

            //// create the starfield
            //starfield = new Starfield(Vector2.Zero, Device,
            //    ScreenManager.Content);
            //starfield.LoadContent();

            base.LoadContent();
        }


        /// <summary>
        /// Release graphics data.
        /// </summary>
        public override void UnloadContent()
        {
            //if (starfield != null)
            //{
            //    starfield.UnloadContent();
            //}

            base.UnloadContent();
        }


        #endregion


        #region Updating Methods


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // if something else has canceled our game, then exit
            if ((networkSession == null) || (world == null))
            {
                if (!IsExiting)
                {
                    ExitScreen();
                }
                base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
                return;
            }

            // update the world
            if (world != null)
            {
                if (otherScreenHasFocus || coveredByOtherScreen)
                {
                    world.Update((float)gameTime.ElapsedGameTime.TotalSeconds, true);
                }
                else if (world.GameExited)
                {
                    if (!IsExiting)
                    {
                        ExitScreen();
                    }
                    networkSession = null;
                    base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
                    return;
                }
                else
                {
                    world.Update((float)gameTime.ElapsedGameTime.TotalSeconds, false);
                    // if the game was just won, then build the winner string
                    if (world.GameWon && String.IsNullOrEmpty(winnerString) &&
                        (world.WinnerIndex >= 0) &&
                        (world.WinnerIndex < networkSession.AllGamers.Count))
                    {
                        winnerString =
                            networkSession.AllGamers[world.WinnerIndex].Gamertag;
                        winnerString +=
                            " has won the game!\nPress A to return to the lobby.";
                        Vector2 winnerStringSize =
                            world.PlayerFont.MeasureString(winnerString);
                        winnerStringPosition = new Vector2(
                                240 / 2 -
                                (float)Math.Floor(winnerStringSize.X / 2),
                                320 / 2 -
                                (float)Math.Floor(winnerStringSize.Y / 2));
                    }
                }
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (!IsExiting)
            {
                if ((world != null) && !world.GameExited)
                {
                    if (input.PauseGame && !world.GameWon)
                    {
                        // If they pressed pause, bring up the pause menu screen.
                        const string message = "Exit the game?";
                        MessageBoxScreen messageBox = new MessageBoxScreen(message,
                            false);
                        messageBox.Accepted += ExitMessageBoxAccepted;
                        ScreenManager.AddScreen(messageBox);
                    }
                    if (input.MenuSelect && world.GameWon)
                    {
                        world.GameExited = true;
                        world = null;
                        if (!IsExiting)
                        {
                            ExitScreen();
                        }
                        networkSession = null;
                    }
                }
            }
        }


        /// <summary>
        /// Event handler for when the user selects "yes" on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        private void ExitMessageBoxAccepted(object sender, EventArgs e)
        {
            world.GameExited = true;
            world = null;
        }


        ///// <summary>
        ///// Exit this screen.
        ///// </summary>
        //public override void ExitScreen()
        //{
        //    if (!IsExiting && (networkSession != null))
        //    {
        //        networkSession.SessionEnded -= sessionEndedHandler;
        //        networkSession.GameEnded -= gameEndedHandler;
        //        networkSession.GamerLeft -= gamerLeftHandler;
        //    }
        //    base.ExitScreen();
        //}


        #endregion


        #region Drawing Methods


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // float elapsedTime = (float)elapsedTime.ElapsedGameTime.TotalSeconds;

            if (networkSession != null)
            {
                // make sure we know what the local tank is
                if ((localTank == null) && (networkSession.LocalGamers.Count > 0))
                {
                    PlayerData playerData = networkSession.LocalGamers[0].Tag
                        as PlayerData;
                    if (playerData.Tank != null)
                    {
                        localTank = playerData.Tank;
                        //starfield.Reset(localTank.Position);
                    }
                }

                // draw the world
                if ((world != null) && (localTank != null) && !IsExiting)
                {
                    Vector2 center = new Vector2(
                        localTank.Position.X  -
                           240 / 2,
                        localTank.Position.Y -
                           320 / 2);
                    //starfield.Draw(center);
                    world.Draw(gameTime.ElapsedGameTime.Seconds, center);
                }

                //// add the bloom
                //bloomComponent.Draw(elapsedTime);

                // draw the user-interface elements of the game (scores, etc.)
                DrawHud((float)gameTime.ElapsedGameTime.Seconds);
            }

            // If the game is transitioning on or off, fade it out to black.
            if (ScreenState == ScreenState.TransitionOn && (TransitionPosition > 0))
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }


        /// <summary>
        /// Draw the user interface elements of the game (scores, etc.).
        /// </summary>
        /// <param name="elapsedTime">The amount of elapsed time, in seconds.</param>
        private void DrawHud(float totalTime)
        {
            if ((networkSession != null) && (world != null))
            {
                ScreenManager.SpriteBatch.Begin();
                // draw players 0 - 3 at the top of the screen
                Vector2 position = new Vector2(
                    240 * 0.2f,
                    320 * 0.1f);
                for (int i = 0; i < Math.Min(4, networkSession.AllGamers.Count); i++)
                {
                    world.DrawPlayerData(totalTime, networkSession.AllGamers[i],
                        position, ScreenManager.SpriteBatch, false);
                    position.X += 240 * 0.2f;
                }
                // draw players 4 - 7 at the bottom of the screen
                position = new Vector2(
                    240 * 0.2f,
                    320 * 0.9f);
                for (int i = 4; i < Math.Min(8, networkSession.AllGamers.Count); i++)
                {
                    world.DrawPlayerData(totalTime, networkSession.AllGamers[i],
                        position, ScreenManager.SpriteBatch, false);
                    position.X += 240 * 0.2f;
                }
                // draw players 8 - 11 at the left of the screen
                position = new Vector2(
                    240 * 0.13f,
                    320 * 0.2f);
                for (int i = 8; i < Math.Min(12, networkSession.AllGamers.Count); i++)
                {
                    world.DrawPlayerData(totalTime, networkSession.AllGamers[i],
                        position, ScreenManager.SpriteBatch, false);
                    position.Y += 320 * 0.2f;
                }
                // draw players 12 - 15 at the right of the screen
                position = new Vector2(
                    240 * 0.9f,
                    320 * 0.2f);
                for (int i = 12; i < Math.Min(16, networkSession.AllGamers.Count); i++)
                {
                    world.DrawPlayerData(totalTime, networkSession.AllGamers[i],
                        position, ScreenManager.SpriteBatch, false);
                    position.Y += 320 * 0.2f;
                }
                // if the game is over, draw the winner text
                if (world.GameWon && !String.IsNullOrEmpty(winnerString))
                {
                    ScreenManager.SpriteBatch.DrawString(world.PlayerFont, winnerString,
                        winnerStringPosition, Color.White, 0f, Vector2.Zero, 1.3f,
                        SpriteEffects.None, 0f);
                }
                ScreenManager.SpriteBatch.End();
            }
        }


        #endregion


        #region Networking Event Handlers


        /// <summary>
        /// Handle the end of the game session.
        /// </summary>
        void networkSession_GameEnded(object sender, GameEndedEventArgs e)
        {
            if ((world != null) && !world.GameWon && !world.GameExited)
            {
                world.GameExited = true;
            }
            if (!IsExiting && ((world == null) || world.GameExited))
            {
                world = null;
                ExitScreen();
                networkSession = null;
            }
        }


        /// <summary>
        /// Handle the end of the session.
        /// </summary>
        void networkSession_SessionEnded(object sender, NetworkSessionEndedEventArgs e)
        {
            if ((world != null) && !world.GameExited)
            {
                world.GameExited = true;
                world = null;
            }
            if (!IsExiting)
            {
                ExitScreen();
            }
            networkSession = null;
        }


        /// <summary>
        /// Handle a player leaving the game.
        /// </summary>
        void networkSession_GamerLeft(object sender, GamerLeftEventArgs e)
        {
            PlayerData playerData = e.Gamer.Tag as PlayerData;
            if ((playerData != null) && (playerData.Tank != null))
            {
                playerData.Tank.Kill(null, true);
            }
        }


        #endregion

    }
}
