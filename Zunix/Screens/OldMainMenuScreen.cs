using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;

using Zunix.ScreensManager;

namespace Zunix.Screens
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    public class OldMainMenuScreen : MenuScreen
    {

        #region Initialization


        /// <summary>
        /// Constructs a new MainMenu object.
        /// </summary>
        public OldMainMenuScreen()
            : base("")
        {
            // set the transition times
            TransitionOnTime = TimeSpan.FromSeconds(1.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.0);


            MenuEntries.Clear();
            MenuEntries.Add(new MenuEntry("Single Player"));
            MenuEntries.Add(new MenuEntry("Multiplayer"));
            MenuEntries.Add(new MenuEntry("Options"));
            MenuEntries.Add(new MenuEntry("Help"));
            MenuEntries.Add(new MenuEntry("Quit"));
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
                case 0: // Single Player
                    ScreenManager.AddScreen(new SinglePlayerMenuScreen());
                    break;

                case 1: // Multiplayer
                    ScreenManager.AddScreen(new MultiplayerMenuScreen());
                    break;

                case 2: // Options
                    ScreenManager.AddScreen(new OldOptionsMenuScreen());
                    break;

                case 3: // Help
                    OnHelp();
                    break;

                case 4: // Exit
                    OnCancel();
                    break;
            }
            
        }

        /// <summary>
        /// The help screen that shows up.
        /// </summary>
        private void OnHelp()
        {
            const string message = "Help";
            MessageBoxScreen messageBox = new MessageBoxScreen(message);
            ScreenManager.AddScreen(messageBox);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel()
        {
            const string message = "Exit Zunix?";
            MessageBoxScreen messageBox = new MessageBoxScreen(message);
            messageBox.Accepted += ExitMessageBoxAccepted;
            ScreenManager.AddScreen(messageBox);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        #endregion


        #region Networking Methods


        //private void QuickMatchSession()
        //{
        //    // start the search
        //    try
        //    {
        //        IAsyncResult asyncResult = NetworkSession.BeginFind(
        //            NetworkSessionType.PlayerMatch, 1, null, null, null);

        //        // create the busy screen
        //        NetworkBusyScreen busyScreen = new NetworkBusyScreen(
        //            "Searching for a session...", asyncResult);
        //        busyScreen.OperationCompleted += QuickMatchSearchCompleted;
        //        ScreenManager.AddScreen(busyScreen);
        //    }
        //    catch (NetworkException ne)
        //    {
        //        const string message = "Failed searching for the session.";
        //        MessageBoxScreen messageBox = new MessageBoxScreen(message);
        //        messageBox.Accepted += FailedMessageBox;
        //        messageBox.Cancelled += FailedMessageBox;
        //        ScreenManager.AddScreen(messageBox);

        //        System.Console.WriteLine("Failed to search for session:  " +
        //            ne.Message);
        //    }
        //    catch (GamerPrivilegeException gpe)
        //    {
        //        const string message =
        //            "You do not have permission to search for a session.";
        //        MessageBoxScreen messageBox = new MessageBoxScreen(message);
        //        messageBox.Accepted += FailedMessageBox;
        //        messageBox.Cancelled += FailedMessageBox;
        //        ScreenManager.AddScreen(messageBox);

        //        System.Console.WriteLine(
        //            "Insufficient privilege to search for session:  " + gpe.Message);
        //    }
        //}


        ///// <summary>
        ///// Start creating a session of the given type.
        ///// </summary>
        ///// <param name="sessionType">The type of session to create.</param>
        //void CreateSession(NetworkSessionType sessionType)
        //{
        //    // create the session
        //    try
        //    {
        //        IAsyncResult asyncResult = NetworkSession.BeginCreate(sessionType, 1,
        //            World.MaximumPlayers, null, null);

        //        // create the busy screen
        //        NetworkBusyScreen busyScreen = new NetworkBusyScreen(
        //            "Creating a session...", asyncResult);
        //        busyScreen.OperationCompleted += SessionCreated;
        //        ScreenManager.AddScreen(busyScreen);
        //    }
        //    catch (NetworkException ne)
        //    {
        //        const string message = "Failed creating the session.";
        //        MessageBoxScreen messageBox = new MessageBoxScreen(message);
        //        messageBox.Accepted += FailedMessageBox;
        //        messageBox.Cancelled += FailedMessageBox;
        //        ScreenManager.AddScreen(messageBox);

        //        System.Console.WriteLine("Failed to create session:  " +
        //            ne.Message);
        //    }
        //    catch (GamerPrivilegeException gpe)
        //    {
        //        const string message =
        //            "You do not have permission to create a session.";
        //        MessageBoxScreen messageBox = new MessageBoxScreen(message);
        //        messageBox.Accepted += FailedMessageBox;
        //        messageBox.Cancelled += FailedMessageBox;
        //        ScreenManager.AddScreen(messageBox);

        //        System.Console.WriteLine(
        //            "Insufficient privilege to create session:  " + gpe.Message);
        //    }
        //}


        ///// <summary>
        ///// Start searching for a session of the given type.
        ///// </summary>
        ///// <param name="sessionType">The type of session to look for.</param>
        //void FindSession(NetworkSessionType sessionType)
        //{
        //    // create the new screen
        //    SearchResultsScreen searchResultsScreen =
        //       new SearchResultsScreen(sessionType);
        //    searchResultsScreen.ScreenManager = this.ScreenManager;
        //    ScreenManager.AddScreen(searchResultsScreen);

        //    // start the search
        //    try
        //    {
        //        IAsyncResult asyncResult = NetworkSession.BeginFind(sessionType, 1, null,
        //            null, null);

        //        // create the busy screen
        //        NetworkBusyScreen busyScreen = new NetworkBusyScreen(
        //            "Searching for a session...", asyncResult);
        //        busyScreen.OperationCompleted += searchResultsScreen.SessionsFound;
        //        ScreenManager.AddScreen(busyScreen);
        //    }
        //    catch (NetworkException ne)
        //    {
        //        const string message = "Failed searching for the session.";
        //        MessageBoxScreen messageBox = new MessageBoxScreen(message);
        //        messageBox.Accepted += FailedMessageBox;
        //        messageBox.Cancelled += FailedMessageBox;
        //        ScreenManager.AddScreen(messageBox);

        //        System.Console.WriteLine("Failed to search for session:  " +
        //            ne.Message);
        //    }
        //    catch (GamerPrivilegeException gpe)
        //    {
        //        const string message =
        //            "You do not have permission to search for a session.";
        //        MessageBoxScreen messageBox = new MessageBoxScreen(message);
        //        messageBox.Accepted += FailedMessageBox;
        //        messageBox.Cancelled += FailedMessageBox;
        //        ScreenManager.AddScreen(messageBox);

        //        System.Console.WriteLine(
        //            "Insufficient privilege to search for session:  " + gpe.Message);
        //    }
        //}


        ///// <summary>
        ///// Callback to receive the network-session search results from quick-match.
        ///// </summary>
        //void QuickMatchSearchCompleted(object sender, OperationCompletedEventArgs e)
        //{
        //    AvailableNetworkSessionCollection availableSessions =
        //        NetworkSession.EndFind(e.AsyncResult);
        //    if ((availableSessions != null) && (availableSessions.Count > 0))
        //    {
        //        // join the session
        //        try
        //        {
        //            IAsyncResult asyncResult = NetworkSession.BeginJoin(
        //                availableSessions[0], null, null);

        //            // create the busy screen
        //            NetworkBusyScreen busyScreen = new NetworkBusyScreen(
        //                "Joining the session...", asyncResult);
        //            busyScreen.OperationCompleted += QuickMatchSessionJoined;
        //            ScreenManager.AddScreen(busyScreen);
        //        }
        //        catch (NetworkException ne)
        //        {
        //            const string message = "Failed joining the session.";
        //            MessageBoxScreen messageBox = new MessageBoxScreen(message);
        //            messageBox.Accepted += FailedMessageBox;
        //            messageBox.Cancelled += FailedMessageBox;
        //            ScreenManager.AddScreen(messageBox);

        //            System.Console.WriteLine("Failed to join session:  " +
        //                ne.Message);
        //        }
        //        catch (GamerPrivilegeException gpe)
        //        {
        //            const string message =
        //                "You do not have permission to join a session.";
        //            MessageBoxScreen messageBox = new MessageBoxScreen(message);
        //            messageBox.Accepted += FailedMessageBox;
        //            messageBox.Cancelled += FailedMessageBox;
        //            ScreenManager.AddScreen(messageBox);

        //            System.Console.WriteLine(
        //                "Insufficient privilege to join session:  " + gpe.Message);
        //        }
        //    }
        //    else
        //    {
        //        const string message = "No matches were found.";
        //        MessageBoxScreen messageBox = new MessageBoxScreen(message);
        //        messageBox.Accepted += FailedMessageBox;
        //        messageBox.Cancelled += FailedMessageBox;
        //        ScreenManager.AddScreen(messageBox);
        //    }
        //}


        ///// <summary>
        ///// Callback when a session is created.
        ///// </summary>
        //void SessionCreated(object sender, OperationCompletedEventArgs e)
        //{
        //    NetworkSession networkSession = null;
        //    try
        //    {
        //        networkSession = NetworkSession.EndCreate(e.AsyncResult);
        //    }
        //    catch (NetworkException ne)
        //    {
        //        const string message = "Failed creating the session.";
        //        MessageBoxScreen messageBox = new MessageBoxScreen(message);
        //        messageBox.Accepted += FailedMessageBox;
        //        messageBox.Cancelled += FailedMessageBox;
        //        ScreenManager.AddScreen(messageBox);

        //        System.Console.WriteLine("Failed to create session:  " +
        //            ne.Message);
        //    }
        //    catch (GamerPrivilegeException gpe)
        //    {
        //        const string message =
        //            "You do not have permission to create a session.";
        //        MessageBoxScreen messageBox = new MessageBoxScreen(message);
        //        messageBox.Accepted += FailedMessageBox;
        //        messageBox.Cancelled += FailedMessageBox;
        //        ScreenManager.AddScreen(messageBox);

        //        System.Console.WriteLine(
        //            "Insufficient privilege to create session:  " + gpe.Message);
        //    }
        //    if (networkSession != null)
        //    {
        //        networkSession.AllowHostMigration = true;
        //        networkSession.AllowJoinInProgress = false;
        //        LoadLobbyScreen(networkSession);
        //    }
        //}


        ///// <summary>
        ///// Callback when a session is quick-matched.
        ///// </summary>
        //void QuickMatchSessionJoined(object sender, OperationCompletedEventArgs e)
        //{
        //    NetworkSession networkSession = null;
        //    try
        //    {
        //        networkSession = NetworkSession.EndJoin(e.AsyncResult);
        //    }
        //    catch (NetworkException ne)
        //    {
        //        const string message = "Failed joining the session.";
        //        MessageBoxScreen messageBox = new MessageBoxScreen(message);
        //        messageBox.Accepted += FailedMessageBox;
        //        messageBox.Cancelled += FailedMessageBox;
        //        ScreenManager.AddScreen(messageBox);

        //        System.Console.WriteLine("Failed to join session:  " +
        //            ne.Message);
        //    }
        //    catch (GamerPrivilegeException gpe)
        //    {
        //        const string message =
        //            "You do not have permission to join a session.";
        //        MessageBoxScreen messageBox = new MessageBoxScreen(message);
        //        messageBox.Accepted += FailedMessageBox;
        //        messageBox.Cancelled += FailedMessageBox;
        //        ScreenManager.AddScreen(messageBox);

        //        System.Console.WriteLine(
        //            "Insufficient privilege to join session:  " + gpe.Message);
        //    }
        //    if (networkSession != null)
        //    {
        //        LoadLobbyScreen(networkSession);
        //    }
        //}


        ///// <summary>
        ///// Load the lobby screen with the new session.
        ///// </summary>
        //void LoadLobbyScreen(NetworkSession networkSession)
        //{
        //    if (networkSession != null)
        //    {
        //        LobbyScreen lobbyScreen = new LobbyScreen(networkSession);
        //        lobbyScreen.ScreenManager = this.ScreenManager;
        //        ScreenManager.AddScreen(lobbyScreen);
        //    }
        //}

        ///// <summary>
        ///// Event handler for when the user selects ok on the network-operation-failed
        ///// message box.
        ///// </summary>
        //void FailedMessageBox(object sender, EventArgs e) { }

        #endregion
    }
}
