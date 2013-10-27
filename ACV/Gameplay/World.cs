using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;

namespace ACV
{
    /// <summary>
    /// A container for the game-specific logic and code.
    /// </summary>
    public class World : IDisposable
    {
        #region Public Constants


        /// <summary>
        /// The maximum number of players in the game.
        /// </summary>
        public const int MaximumPlayers = 4;


        /// <summary>
        /// The different types of packets sent in the game.
        /// </summary>
        /// <remarks>Frequently used in packets to identify their type.</remarks>
        public enum PacketTypes
        {
            PlayerData,
            TankData,
            WorldSetup,
            WorldData,
            TankInput,
            PowerUpSpawn,
            TankDeath,
            TankSpawn,
            GameWon,
        };


        #endregion


        #region Constants


        /// <summary>
        /// The score required to win the game.
        /// </summary>
        const int winningScore = 10;

        ///// <summary>
        ///// The number of asteroids in the game.
        ///// </summary>
        //const int numberOfAsteroids = 15;

        ///// <summary>
        ///// The length of time it takes for another power-up to spawn.
        ///// </summary>
        //const float maximumPowerUpTimer = 10f;

        ///// <summary>
        ///// The size of all of the barriers in the game.
        ///// </summary>
        //const int barrierSize = 48;

        /// <summary>
        /// The number of updates between WorldData packets.
        /// </summary>
        const int updatesBetweenWorldDataSend = 30;

        /// <summary>
        /// The number of updates between tank status packets from this machine.
        /// </summary>
        const int updatesBetweenStatusPackets = MaximumPlayers;

        ///// <summary>
        ///// The number of barriers in each dimension.
        ///// </summary>
        //static readonly Point barrierCounts = new Point(50, 50);

        ///// <summary>
        ///// The dimensions of the game world.
        ///// </summary>
        //static readonly Rectangle dimensions = new Rectangle(0, 0,
        //    barrierCounts.X * barrierSize, barrierCounts.Y * barrierSize);


        #endregion


        #region State Data


        /// <summary>
        /// If true, the game has been initialized by receiving a WorldSetup packet.
        /// </summary>
        bool initialized = false;
        public bool Initialized
        {
            get { return initialized; }
        }

        /// <summary>
        /// If true, the game is over, and somebody has won.
        /// </summary>
        private bool gameWon = false;
        public bool GameWon
        {
            get { return gameWon; }
            set { gameWon = value; }
        }

        /// <summary>
        /// The index of the player who won the game.
        /// </summary>
        private int winnerIndex = -1;
        public int WinnerIndex
        {
            get { return winnerIndex; }
        }


        /// <summary>
        /// If true, the game is over, because the game ended before somebody won.
        /// </summary>
        /// <remarks></remarks>
        private bool gameExited = false;
        public bool GameExited
        {
            get { return gameExited; }
            set { gameExited = value; }
        }


        #endregion


        #region Gameplay Data


        ///// <summary>
        ///// The number of asteroids in the game.
        ///// </summary>
        //Asteroid[] asteroids = new Asteroid[numberOfAsteroids];

        ///// <summary>
        ///// The current power-up in the game.
        ///// </summary>
        //PowerUp powerUp = null;

        ///// <summary>
        ///// The amount of time left until the next power-up spawns.
        ///// </summary>
        //float powerUpTimer = maximumPowerUpTimer / 2f;


        #endregion


        #region Graphics Data


        /// <summary>
        /// The sprite batch used to draw the objects in the world.
        /// </summary>
        private SpriteBatch spriteBatch;

        ///// <summary>
        ///// The corner-barrier texture.
        ///// </summary>
        //private Texture2D cornerBarrierTexture;

        ///// <summary>
        ///// The vertical-barrier texture.
        ///// </summary>
        //private Texture2D verticalBarrierTexture;

        ///// <summary>
        ///// The horizontal-barrier texture.
        ///// </summary>
        //private Texture2D horizontalBarrierTexture;

        /// <summary>
        /// The texture signifying that the player can chat.
        /// </summary>
        private Texture2D chatAbleTexture;

        /// <summary>
        /// The texture signifying that the player has been muted.
        /// </summary>
        private Texture2D chatMuteTexture;

        /// <summary>
        /// The texture signifying that the player is talking right now.
        /// </summary>
        private Texture2D chatTalkingTexture;

        /// <summary>
        /// The texture signifying that the player is ready
        /// </summary>
        private Texture2D readyTexture;

        /// <summary>
        /// The sprite used to draw the player names.
        /// </summary>
        private SpriteFont playerFont;
        public SpriteFont PlayerFont
        {
            get { return playerFont; }
        }

        ///// <summary>
        ///// The list of corner barriers in the game world.
        ///// </summary>
        ///// <remarks>This list is not owned by this object.</remarks>
        //private List<Rectangle> cornerBarriers = new List<Rectangle>();

        ///// <summary>
        ///// The list of vertical barriers in the game world.
        ///// </summary>
        ///// <remarks>This list is not owned by this object.</remarks>
        //private List<Rectangle> verticalBarriers = new List<Rectangle>();

        ///// <summary>
        ///// The list of horizontal barriers in the game world.
        ///// </summary>
        ///// <remarks>This list is not owned by this object.</remarks>
        //private List<Rectangle> horizontalBarriers = new List<Rectangle>();

        ///// <summary>
        ///// The pseudo-effect manager for the game.
        ///// </summary>
        //ParticleEffectManager pseudoEffectManager;


        #endregion


        #region Networking Data


        /// <summary>
        /// The network session for the game.
        /// </summary>
        private NetworkSession networkSession;

        /// <summary>
        /// The packet writer for all of the data for the world.
        /// </summary>
        private PacketWriter packetWriter = new PacketWriter();

        /// <summary>
        /// The packet reader for all of the data for the world.
        /// </summary>
        private PacketReader packetReader = new PacketReader();

        /// <summary>
        /// The number of updates that have passed since the world data was sent.
        /// </summary>
        private int updatesSinceWorldDataSend = 0;

        /// <summary>
        /// The number of updates that have passed since a status packet was sent.
        /// </summary>
        private int updatesSinceStatusPacket = 0;


        #endregion


        #region Initialization


        /// <summary>
        /// Construct a new World object.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device used for this game.</param>
        /// <param name="networkSession">The network session for this game.</param>
        public World(GraphicsDevice graphicsDevice, ContentManager contentManager,
            NetworkSession networkSession)
        {
            // safety-check the parameters, as they must be valid
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice");
            }
            if (contentManager == null)
            {
                throw new ArgumentNullException("contentManager");
            }
            if (networkSession == null)
            {
                throw new ArgumentNullException("networkSession");
            }

            // apply the parameter values
            this.networkSession = networkSession;

            // set up the staggered status packet system
            // -- your first update happens based on where you are in the collection
            for (int i = 0; i < networkSession.AllGamers.Count; i++)
            {
                if (networkSession.AllGamers[i].IsLocal)
                {
                    updatesSinceStatusPacket = i;
                    break;
                }
            }

            // create the spritebatch
            spriteBatch = new SpriteBatch(graphicsDevice);

            // load the font
            //playerFont = contentManager.Load<SpriteFont>("Fonts/NetRumbleFont");

            Tank.LoadContent(contentManager);
            GreenLaser.LoadContent(contentManager);

            //// load the gameplay-object textures
            //Tank.LoadContent(contentManager);
            //LaserProjectile.LoadContent(contentManager);
            //MineProjectile.LoadContent(contentManager);
            //MissileProjectile.LoadContent(contentManager);
            //DoubleLaserPowerUp.LoadContent(contentManager);
            //TripleLaserPowerUp.LoadContent(contentManager);
            //MissilePowerUp.LoadContent(contentManager);

            // load the non-gameplay-object textures
            chatAbleTexture = contentManager.Load<Texture2D>("Chat/ChatAble");
            chatMuteTexture = contentManager.Load<Texture2D>("Chat/chatMute");
            chatTalkingTexture = contentManager.Load<Texture2D>("Chat/ChatTalking");
            readyTexture = contentManager.Load<Texture2D>("Chat/Ready");

            // clear the collision manager
            CollisionManager.Collection.Clear();

        }


        /// <summary>
        /// Generate the initial state of the game, and send it to everyone.
        /// </summary>
        public void GenerateWorld()
        {
            if ((networkSession != null) && (networkSession.LocalGamers.Count > 0))
            {
                // write the identification value
                packetWriter.Write((int)PacketTypes.WorldSetup);

                // place the tanks
                // -- we always write the maximum number of players, making the packet
                //    predictable, in case the player count changes on the client before 
                //    this packet is received
                for (int i = 0; i < MaximumPlayers; i++)
                {
                    Vector2 position = Vector2.Zero;
                    if (i < networkSession.AllGamers.Count)
                    {
                        PlayerData playerData = networkSession.AllGamers[i].Tag
                            as PlayerData;
                        if ((playerData != null) && (playerData.Tank != null))
                        {
                            playerData.Tank.Initialize();
                            position = playerData.Tank.Position =
                                CollisionManager.FindSpawnPoint(playerData.Tank,
                                playerData.Tank.Radius * 5f);
                            playerData.Tank.Score = 0;
                        }
                    }
                    // write the tank position
                    packetWriter.Write(position);
                }

                // place the asteroids
                // -- for simplicity, the same number of asteroids is always the same
                for (int i = 0; i < asteroids.Length; i++)
                {
                    // choose one of three radii
                    float radius = 32f;
                    switch (RandomMath.Random.Next(3))
                    {
                        case 0:
                            radius = 32f;
                            break;
                        case 1:
                            radius = 60f;
                            break;
                        case 2:
                            radius = 96f;
                            break;
                    }
                    // create the asteroid
                    asteroids[i] = new Asteroid(radius);
                    // write the radius
                    packetWriter.Write(asteroids[i].Radius);
                    // choose a variation
                    asteroids[i].Variation = i % Asteroid.Variations;
                    // write the variation
                    packetWriter.Write(asteroids[i].Variation);
                    // initialize the asteroid and it's starting position
                    asteroids[i].Initialize();
                    asteroids[i].Position =
                        CollisionManager.FindSpawnPoint(asteroids[i],
                        asteroids[i].Radius);
                    // write the starting position and velocity
                    packetWriter.Write(asteroids[i].Position);
                    packetWriter.Write(asteroids[i].Velocity);
                }

                // send the packet to everyone
                networkSession.LocalGamers[0].SendData(packetWriter,
                    SendDataOptions.ReliableInOrder);
            }
        }


        /// <summary>
        /// Initialize the world with the data from the WorldSetup packet.
        /// </summary>
        /// <param name="packetReader">The packet reader with the world data.</param>
        public void Initialize()
        {
            // reset the game status
            gameWon = false;
            winnerIndex = -1;
            gameExited = false;

            // initialize the tanks with the data from the packet
            for (int i = 0; i < MaximumPlayers; i++)
            {
                // read each of the positions
                Vector2 position = packetReader.ReadVector2();
                // use the position value if we know of that many players
                if (i < networkSession.AllGamers.Count)
                {
                    PlayerData playerData = networkSession.AllGamers[i].Tag
                        as PlayerData;
                    if ((playerData != null) && (playerData.Tank != null))
                    {
                        // initialize the tank with the provided position
                        playerData.Tank.Position = position;
                        playerData.Tank.Score = 0;
                        playerData.Tank.Initialize();
                    }
                }
            }

            // initialize the tanks with the data from the packet
            for (int i = 0; i < asteroids.Length; i++)
            {
                float radius = packetReader.ReadSingle();
                if (asteroids[i] == null)
                {
                    asteroids[i] = new Asteroid(radius);
                }
                asteroids[i].Variation = packetReader.ReadInt32();
                asteroids[i].Position = packetReader.ReadVector2();
                asteroids[i].Initialize();
                asteroids[i].Velocity = packetReader.ReadVector2();
            }

            // set the initialized state
            initialized = true;
        }


        #endregion


        #region Updating Methods


        /// <summary>
        /// Update the world.
        /// </summary>
        /// <param name="elapsedTime">The amount of elapsed time, in seconds.</param>
        /// <param name="paused">If true, the game is paused.</param>
        public void Update(float elapsedTime, bool paused)
        {
            if (gameWon)
            {
                // update the pseudo-effect manager
                pseudoEffectManager.Update(elapsedTime);

                // make sure the collision manager is empty
                CollisionManager.Collection.ApplyPendingRemovals();
                if (CollisionManager.Collection.Count > 0)
                {
                    CollisionManager.Collection.Clear();
                }
            }
            else
            {
                // process all incoming packets
                ProcessPackets();

                // if the game is in progress, update the state of it
                if (initialized && (networkSession != null) &&
                    (networkSession.SessionState == NetworkSessionState.Playing))
                {
                    // the host has singular responsibilities to the game world, 
                    // that need to be done once, by one authority
                    if (networkSession.IsHost)
                    {
                        // get the local player, for frequent re-use
                        LocalNetworkGamer localGamer = networkSession.Host
                            as LocalNetworkGamer;

                        // check for victory
                        int highScore = 0;
                        int highScoreIndex = -1;
                        for (int i = 0; i < networkSession.AllGamers.Count; i++)
                        {
                            NetworkGamer networkGamer = networkSession.AllGamers[i];
                            PlayerData playerData = networkGamer.Tag as PlayerData;
                            if ((playerData != null) && (playerData.Tank != null))
                            {
                                if (playerData.Tank.Score > highScore)
                                {
                                    highScore = playerData.Tank.Score;
                                    highScoreIndex = i;
                                }
                            }
                        }
                        // if victory has been achieved, send a packet to everyone
                        if (highScore >= winningScore)
                        {
                            packetWriter.Write((int)PacketTypes.GameWon);
                            packetWriter.Write(highScoreIndex);
                            localGamer.SendData(packetWriter,
                                SendDataOptions.ReliableInOrder);
                        }

                        // respawn each player, if it is time to do so
                        for (int i = 0; i < networkSession.AllGamers.Count; i++)
                        {
                            NetworkGamer networkGamer = networkSession.AllGamers[i];
                            PlayerData playerData = networkGamer.Tag as PlayerData;
                            if ((playerData != null) && (playerData.Tank != null) &&
                                !playerData.Tank.Alive &&
                                (playerData.Tank.RespawnTimer <= 0f))
                            {
                                // write the tank-spawn packet
                                packetWriter.Write((int)PacketTypes.TankSpawn);
                                packetWriter.Write(i);
                                packetWriter.Write(CollisionManager.FindSpawnPoint(
                                    playerData.Tank, playerData.Tank.Radius));
                                localGamer.SendData(packetWriter,
                                    SendDataOptions.ReliableInOrder);
                            }

                        }

                        // respawn the power-up if it is time to do so
                        if (powerUp == null)
                        {
                            powerUpTimer -= elapsedTime;
                            if (powerUpTimer < 0)
                            {
                                // write the power-up-spawn packet
                                packetWriter.Write((int)PacketTypes.PowerUpSpawn);
                                packetWriter.Write(RandomMath.Random.Next(3));
                                packetWriter.Write(CollisionManager.FindSpawnPoint(null,
                                    PowerUp.PowerUpRadius));
                                localGamer.SendData(packetWriter,
                                    SendDataOptions.ReliableInOrder);
                            }
                        }
                        else
                        {
                            powerUpTimer = maximumPowerUpTimer;
                        }

                        // send everyone an update on the state of the world
                        if (updatesSinceWorldDataSend >= updatesBetweenWorldDataSend)
                        {
                            packetWriter.Write((int)PacketTypes.WorldData);
                            // write each of the asteroids
                            for (int i = 0; i < asteroids.Length; i++)
                            {
                                packetWriter.Write(asteroids[i].Position);
                                packetWriter.Write(asteroids[i].Velocity);
                            }
                            localGamer.SendData(packetWriter,
                                SendDataOptions.InOrder);
                            updatesSinceWorldDataSend = 0;
                        }
                        else
                        {
                            updatesSinceWorldDataSend++;
                        }
                    }

                    // update each asteroid
                    foreach (Asteroid asteroid in asteroids)
                    {
                        if (asteroid.Alive)
                        {
                            asteroid.Update(elapsedTime);
                        }
                    }

                    // update the power-up
                    if (powerUp != null)
                    {
                        if (powerUp.Alive)
                        {
                            powerUp.Update(elapsedTime);
                        }
                        else
                        {
                            powerUp = null;
                        }
                    }

                    // process the local player's input
                    if (!paused)
                    {
                        ProcessLocalPlayerInput();
                    }

                    // update each tank
                    foreach (NetworkGamer networkGamer in networkSession.AllGamers)
                    {
                        PlayerData playerData = networkGamer.Tag as PlayerData;
                        if ((playerData != null) && (playerData.Tank != null))
                        {
                            if (playerData.Tank.Alive)
                            {
                                playerData.Tank.Update(elapsedTime);
                                // check for death 
                                // -- only check on local machines - the local player is
                                //    the authority on the death of their own tank
                                if (networkGamer.IsLocal && (playerData.Tank.Life < 0))
                                {
                                    SendLocalTankDeath();
                                }
                            }
                            else if (playerData.Tank.RespawnTimer > 0f)
                            {
                                playerData.Tank.RespawnTimer -= elapsedTime;
                                if (playerData.Tank.RespawnTimer < 0f)
                                {
                                    playerData.Tank.RespawnTimer = 0f;
                                }
                            }
                        }
                    }

                    // update the other players with the current state of the local tank
                    if (updatesSinceStatusPacket >= updatesBetweenStatusPackets)
                    {
                        updatesSinceStatusPacket = 0;
                        SendLocalTankData();
                    }
                    else
                    {
                        updatesSinceStatusPacket++;
                    }

                    // update the collision manager
                    CollisionManager.Update(elapsedTime);

                    //// update the pseudo-effect manager
                    //pseudoEffectManager.Update(elapsedTime);
                }
            }
        }


        /// <summary>
        /// Process the local player's input.
        /// </summary>
        private void ProcessLocalPlayerInput()
        {
            if ((networkSession != null) && (networkSession.LocalGamers.Count > 0))
            {
                // create the new input structure
                TankInput tankInput = new TankInput(
                    GamePad.GetState(
                       networkSession.LocalGamers[0].SignedInGamer.PlayerIndex),
                    Keyboard.GetState(
                       networkSession.LocalGamers[0].SignedInGamer.PlayerIndex));
                // send it out
                // -- the local machine will receive and apply it from the network just
                //    like the other clients
                tankInput.Serialize(packetWriter);
                networkSession.LocalGamers[0].SendData(packetWriter,
                    SendDataOptions.InOrder);
            }
        }


        /// <summary>
        /// Send the current state of the tank to the other players.
        /// </summary>
        private void SendLocalTankData()
        {
            if ((networkSession != null) && (networkSession.LocalGamers.Count > 0))
            {
                PlayerData playerData = networkSession.LocalGamers[0].Tag as PlayerData;
                if ((playerData != null) && (playerData.Tank != null))
                {
                    packetWriter.Write((int)World.PacketTypes.TankData);
                    packetWriter.Write(playerData.Tank.Position);
                    packetWriter.Write(playerData.Tank.Velocity);
                    packetWriter.Write(playerData.Tank.Rotation);
                    packetWriter.Write(playerData.Tank.Life);
                    packetWriter.Write(playerData.Tank.InvincibilityShield);
                    packetWriter.Write(playerData.Tank.Score);
                    networkSession.LocalGamers[0].SendData(packetWriter,
                        SendDataOptions.InOrder);
                }
            }
        }


        /// <summary>
        /// Send a notification of the death of the local tank to the other players.
        /// </summary>
        private void SendLocalTankDeath()
        {
            if ((networkSession != null) && (networkSession.LocalGamers.Count > 0))
            {
                LocalNetworkGamer localNetworkGamer = networkSession.LocalGamers[0]
                    as LocalNetworkGamer;
                PlayerData playerData = localNetworkGamer.Tag as PlayerData;
                if ((playerData != null) && (playerData.Tank != null))
                {
                    // send a tank-death notification
                    packetWriter.Write((int)PacketTypes.TankDeath);
                    // determine the player behind the last damage taken
                    int lastDamagedByPlayer = -1;
                    Tank lastDamagedByTank = playerData.Tank.LastDamagedBy as Tank;
                    if ((lastDamagedByTank != null) &&
                        (lastDamagedByTank != playerData.Tank))
                    {
                        for (int i = 0; i < networkSession.AllGamers.Count; i++)
                        {
                            PlayerData sourcePlayerData =
                                networkSession.AllGamers[i].Tag as PlayerData;
                            if ((sourcePlayerData != null) &&
                                (sourcePlayerData.Tank != null) &&
                                (sourcePlayerData.Tank == lastDamagedByTank))
                            {
                                lastDamagedByPlayer = i;
                                break;
                            }
                        }
                    }
                    packetWriter.Write(lastDamagedByPlayer);
                    localNetworkGamer.SendData(packetWriter,
                        SendDataOptions.ReliableInOrder);
                }
            }
        }


        #endregion


        #region Packet Handling Methods


        /// <summary>
        /// Process incoming packets on the local gamer.
        /// </summary>
        private void ProcessPackets()
        {
            if ((networkSession != null) && (networkSession.LocalGamers.Count > 0))
            {
                // process all packets found, every frame
                while (networkSession.LocalGamers[0].IsDataAvailable)
                {
                    NetworkGamer sender;
                    networkSession.LocalGamers[0].ReceiveData(packetReader, out sender);
                    // read the type of packet...
                    PacketTypes packetType = (PacketTypes)packetReader.ReadInt32();
                    // ... and dispatch appropriately
                    switch (packetType)
                    {
                        case PacketTypes.PlayerData:
                            UpdatePlayerData(sender);
                            break;

                        case PacketTypes.WorldSetup:
                            // apply the world setup data, but only once
                            if (!Initialized)
                            {
                                Initialize();
                            }
                            break;

                        case PacketTypes.TankData:
                            if ((sender != null) && !sender.IsLocal)
                            {
                                UpdateTankData(sender);
                            }
                            break;

                        case PacketTypes.WorldData:
                            if (!networkSession.IsHost && Initialized)
                            {
                                UpdateWorldData();
                            }
                            break;

                        case PacketTypes.TankInput:
                            if (sender != null)
                            {
                                PlayerData playerData = sender.Tag as PlayerData;
                                if ((playerData != null) && (playerData.Tank != null))
                                {
                                    playerData.Tank.TankInput =
                                        new TankInput(packetReader);
                                }
                            }
                            break;

                        case PacketTypes.TankSpawn:
                            SpawnTank();
                            break;

                        case PacketTypes.PowerUpSpawn:
                            SpawnPowerup();
                            break;

                        case PacketTypes.TankDeath:
                            KillTank(sender);
                            break;

                        case PacketTypes.GameWon:
                            gameWon = true;
                            winnerIndex = packetReader.ReadInt32();
                            if (networkSession.IsHost && (networkSession.SessionState ==
                                NetworkSessionState.Playing))
                            {
                                networkSession.EndGame();
                            }
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// Spawn a tank based on the data in the packet.
        /// </summary>
        private void SpawnTank()
        {
            int whichGamer = packetReader.ReadInt32();
            if (whichGamer < networkSession.AllGamers.Count)
            {
                NetworkGamer networkGamer = networkSession.AllGamers[whichGamer];
                PlayerData playerData = networkGamer.Tag as PlayerData;
                if ((playerData != null) && (playerData.Tank != null))
                {
                    playerData.Tank.Position = packetReader.ReadVector2();
                    playerData.Tank.Initialize();
                }
            }
        }


        /// <summary>
        /// Spawn a power-up based on the data in the packet.
        /// </summary>
        private void SpawnPowerup()
        {
            int whichPowerUp = packetReader.ReadInt32();
            if (powerUp == null)
            {
                switch (whichPowerUp)
                {
                    case 0:
                        powerUp = new DoubleLaserPowerUp();
                        break;
                    case 1:
                        powerUp = new TripleLaserPowerUp();
                        break;
                    case 2:
                        powerUp = new MissilePowerUp();
                        break;
                }
            }
            if (powerUp != null)
            {
                powerUp.Position = packetReader.ReadVector2();
                powerUp.Initialize();
            }
        }


        /// <summary>
        /// Kill the sender's tank based on data in the packet.
        /// </summary>
        /// <param name="sender">The sender of the packet.</param>
        private void KillTank(NetworkGamer sender)
        {
            if (sender != null)
            {
                PlayerData playerData = sender.Tag as PlayerData;
                if ((playerData != null) && (playerData.Tank != null) &&
                    playerData.Tank.Alive)
                {
                    GameObject source = null;
                    // read the index of the source of the last damage taken
                    int sourcePlayerIndex = packetReader.ReadInt32();
                    if ((sourcePlayerIndex >= 0) &&
                        (sourcePlayerIndex < networkSession.AllGamers.Count))
                    {
                        PlayerData sourcePlayerData =
                            networkSession.AllGamers[sourcePlayerIndex].Tag
                            as PlayerData;
                        source = sourcePlayerData != null ? sourcePlayerData.Tank :
                            null;
                    }
                    // kill the tank
                    playerData.Tank.Kill(source, false);
                }
            }
        }


        /// <summary>
        /// Update the player data for the sender based on the data in the packet.
        /// </summary>
        /// <param name="sender">The sender of the packet.</param>
        private void UpdatePlayerData(NetworkGamer sender)
        {
            if ((networkSession != null) && (networkSession.LocalGamers.Count > 0) &&
                (sender != null))
            {
                PlayerData playerData = sender.Tag as PlayerData;
                if (playerData != null)
                {
                    playerData.Deserialize(packetReader);
                    // see if we're still unique
                    // -- this can happen legitimately as we receive introductory data
                    foreach (LocalNetworkGamer localNetworkGamer in
                        networkSession.LocalGamers)
                    {
                        PlayerData localPlayerData =
                            localNetworkGamer.Tag as PlayerData;
                        if ((localPlayerData != null) &&
                            !Tank.HasUniqueColorIndex(localNetworkGamer,
                               networkSession))
                        {
                            localPlayerData.TankColor = Tank.GetNextUniqueColorIndex(
                                localPlayerData.TankColor, networkSession);
                            packetWriter.Write((int)World.PacketTypes.PlayerData);
                            localPlayerData.Serialize(packetWriter);
                            networkSession.LocalGamers[0].SendData(packetWriter,
                                SendDataOptions.ReliableInOrder);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Update tank state based on the data in the packet.
        /// </summary>
        /// <param name="sender">The sender of the packet.</param>
        private void UpdateTankData(NetworkGamer sender)
        {
            if (sender != null)
            {
                PlayerData playerData = sender.Tag as PlayerData;
                if ((playerData != null) && (playerData.Tank != null))
                {
                    playerData.Tank.Position = packetReader.ReadVector2();
                    playerData.Tank.Velocity = packetReader.ReadVector2();
                    playerData.Tank.Rotation = packetReader.ReadSingle();
                    playerData.Tank.Life = packetReader.ReadSingle();
                    playerData.Tank.InvincibilityShield = packetReader.ReadSingle();
                    playerData.Tank.Score = packetReader.ReadInt32();
                }
            }
        }


        /// <summary>
        /// Update the world data based on the data in the packet.
        /// </summary>
        private void UpdateWorldData()
        {
            // safety-check the parameters, as they must be valid
            if (packetReader == null)
            {
                throw new ArgumentNullException("packetReader");
            }

            for (int i = 0; i < asteroids.Length; i++)
            {
                asteroids[i].Position = packetReader.ReadVector2();
                asteroids[i].Velocity = packetReader.ReadVector2();
            }
        }


        #endregion


        #region Drawing Methods


        /// <summary>
        /// Draws the objects in the world.
        /// </summary>
        /// <param name="elapsedTime">The amount of elapsed time, in seconds.</param>
        /// <param name="center">The center of the current view.</param>
        public void Draw(float elapsedTime, Vector2 center)
        {
            Matrix transform = Matrix.CreateTranslation(
                new Vector3(-center.X, -center.Y, 0f));
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred,
                SaveStateMode.None, transform);

            // draw the barriers
            foreach (Rectangle rectangle in cornerBarriers)
            {
                spriteBatch.Draw(cornerBarrierTexture, rectangle, Color.White);
            }
            foreach (Rectangle rectangle in verticalBarriers)
            {
                spriteBatch.Draw(verticalBarrierTexture, rectangle, Color.White);
            }
            foreach (Rectangle rectangle in horizontalBarriers)
            {
                spriteBatch.Draw(horizontalBarrierTexture, rectangle, Color.White);
            }

            // draw the asteroids
            foreach (Asteroid asteroid in asteroids)
            {
                if (asteroid.Alive)
                {
                    asteroid.Draw(elapsedTime, spriteBatch);
                }
            }

            // draw the powerup
            if ((powerUp != null) && powerUp.Alive)
            {
                powerUp.Draw(elapsedTime, spriteBatch);
            }

            // draw the tanks
            foreach (NetworkGamer networkGamer in networkSession.AllGamers)
            {
                PlayerData playerData = networkGamer.Tag as PlayerData;
                if ((playerData != null) && (playerData.Tank != null) &&
                    playerData.Tank.Alive)
                {
                    playerData.Tank.Draw(elapsedTime, spriteBatch);
                }
            }

            // draw the alpha-blended pseudos
            pseudoEffectManager.Draw(spriteBatch, SpriteBlendMode.AlphaBlend);

            spriteBatch.End();

            // draw the additive pseudos
            spriteBatch.Begin(SpriteBlendMode.Additive, SpriteSortMode.Texture,
                SaveStateMode.None, transform);
            pseudoEffectManager.Draw(spriteBatch, SpriteBlendMode.Additive);
            spriteBatch.End();
        }


        /// <summary>
        /// Draw the specified player's data in the screen - gamertag, etc.
        /// </summary>
        /// <param name="totalTime">The total time spent in the game.</param>
        /// <param name="networkGamer">The player to be drawn.</param>
        /// <param name="position">The center of the desired location.</param>
        /// <param name="spriteBatch">The SpriteBatch object used to draw.</param>
        /// <param name="lobby">If true, drawn "lobby style"</param>
        public void DrawPlayerData(float totalTime, NetworkGamer networkGamer,
            Vector2 position, SpriteBatch spriteBatch, bool lobby)
        {
            // safety-check the parameters, as they must be valid
            if (networkGamer == null)
            {
                throw new ArgumentNullException("networkGamer");
            }
            if (spriteBatch == null)
            {
                throw new ArgumentNullException("spriteBatch");
            }

            // get the player data
            PlayerData playerData = networkGamer.Tag as PlayerData;
            if (playerData == null)
            {
                return;
            }

            // draw the gamertag
            float playerStringScale = 1.0f;
            if (networkGamer.IsLocal)
            {
                // pulse the scale of local gamers
                playerStringScale = 1f + 0.08f * (1f + (float)Math.Sin(totalTime * 4f));
            }
            string playerString = networkGamer.Gamertag;
            Color playerColor = playerData.Tank == null ?
                Tank.TankColors[playerData.TankColor] : playerData.Tank.Color;
            Vector2 playerStringSize = playerFont.MeasureString(playerString);
            Vector2 playerStringPosition = position;
            spriteBatch.DrawString(playerFont, playerString, playerStringPosition,
                playerColor, 0f,
                new Vector2(playerStringSize.X / 2f, playerStringSize.Y / 2f),
                playerStringScale, SpriteEffects.None, 0f);

            // draw the chat texture
            Texture2D chatTexture = null;
            if (networkGamer.IsMutedByLocalUser)
            {
                chatTexture = chatMuteTexture;
            }
            else if (networkGamer.IsTalking)
            {
                chatTexture = chatTalkingTexture;
            }
            else if (networkGamer.HasVoice)
            {
                chatTexture = chatAbleTexture;
            }
            if (chatTexture != null)
            {
                float chatTextureScale = 0.9f * playerStringSize.Y /
                    (float)chatTexture.Height;
                Vector2 chatTexturePosition = new Vector2(playerStringPosition.X -
                    1.2f * playerStringSize.X / 2f -
                    1.1f * chatTextureScale * (float)chatTexture.Width / 2f,
                    playerStringPosition.Y);
                spriteBatch.Draw(chatTexture, chatTexturePosition, null,
                    Color.White, 0f, new Vector2((float)chatTexture.Width / 2f,
                    (float)chatTexture.Height / 2f), chatTextureScale,
                    SpriteEffects.None, 0f);
            }

            // if we're in "lobby mode", draw a sample version of the tank,
            // and the ready texture
            if (lobby)
            {
                // draw the tank
                if (playerData.Tank != null)
                {
                    float oldTankShield = playerData.Tank.InvincibilityShield;
                    float oldTankRadius = playerData.Tank.Radius;
                    Vector2 oldTankPosition = playerData.Tank.Position;
                    float oldTankRotation = playerData.Tank.Rotation;
                    playerData.Tank.InvincibilityShield = 0f;
                    playerData.Tank.Radius = 0.6f * (float)playerStringSize.Y;
                    playerData.Tank.Position = new Vector2(playerStringPosition.X +
                        1.2f * playerStringSize.X / 2f + 1.1f * playerData.Tank.Radius,
                        playerStringPosition.Y);
                    playerData.Tank.Rotation = 0f;
                    playerData.Tank.Draw(0f, spriteBatch);
                    playerData.Tank.Rotation = oldTankRotation;
                    playerData.Tank.Position = oldTankPosition;
                    playerData.Tank.InvincibilityShield = oldTankShield;
                    playerData.Tank.Radius = oldTankRadius;
                }

                // draw the ready texture
                if ((readyTexture != null) && networkGamer.IsReady)
                {
                    float readyTextureScale = 0.9f * playerStringSize.Y /
                        (float)readyTexture.Height;
                    Vector2 readyTexturePosition = new Vector2(playerStringPosition.X +
                        1.2f * playerStringSize.X / 2f +
                        2.2f * playerData.Tank.Radius +
                        1.1f * readyTextureScale * (float)readyTexture.Width / 2f,
                        playerStringPosition.Y);
                    spriteBatch.Draw(readyTexture, readyTexturePosition, null,
                        Color.White, 0f, new Vector2((float)readyTexture.Width / 2f,
                        (float)readyTexture.Height / 2f), readyTextureScale,
                        SpriteEffects.None, 0f);
                }

            }
            else
            {
                // if we're not in "lobby mode", draw the score
                if (playerData.Tank != null)
                {
                    string scoreString = String.Empty;
                    if (playerData.Tank.Alive)
                    {
                        scoreString = playerData.Tank.Score.ToString();
                    }
                    else
                    {
                        int respawnTimer =
                            (int)Math.Ceiling(playerData.Tank.RespawnTimer);
                        scoreString = "Respawning in: " + respawnTimer.ToString();
                    }
                    Vector2 scoreStringSize = playerFont.MeasureString(scoreString);
                    Vector2 scoreStringPosition = new Vector2(position.X,
                        position.Y + 0.9f * playerStringSize.Y);
                    spriteBatch.DrawString(playerFont, scoreString, scoreStringPosition,
                        playerColor, 0f, new Vector2(scoreStringSize.X / 2f,
                        scoreStringSize.Y / 2f), 1f, SpriteEffects.None, 0f);
                }
            }
        }


        #endregion


        #region IDisposable Implementation


        /// <summary>
        /// Finalizes the World object, calls Dispose(false)
        /// </summary>
        ~World()
        {
            Dispose(false);
        }


        /// <summary>
        /// Disposes the World object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Disposes this object.
        /// </summary>
        /// <param name="disposing">
        /// True if this method was called as part of the Dispose method.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (this)
                {
                    if (packetReader != null)
                    {
                        packetReader.Close();
                        packetReader = null;
                    }

                    if (packetWriter != null)
                    {
                        packetWriter.Close();
                        packetWriter = null;
                    }

                    if (spriteBatch != null)
                    {
                        spriteBatch.Dispose();
                        spriteBatch = null;
                    }

                    cornerBarrierTexture = null;
                    verticalBarrierTexture = null;
                    horizontalBarrierTexture = null;

                    Tank.UnloadContent();
                    Asteroid.UnloadContent();
                    LaserProjectile.UnloadContent();
                    MineProjectile.UnloadContent();
                    MissileProjectile.UnloadContent();
                    DoubleLaserPowerUp.UnloadContent();
                    TripleLaserPowerUp.UnloadContent();

                    Tank.ParticleEffectManager = null;
                    MissileProjectile.ParticleEffectManager = null;
                    MineProjectile.ParticleEffectManager = null;
                    LaserProjectile.ParticleEffectManager = null;
                    pseudoEffectManager.UnregisterParticleEffect(
                        ParticleEffectType.MineExplosion);
                    pseudoEffectManager.UnregisterParticleEffect(
                        ParticleEffectType.MissileExplosion);
                    pseudoEffectManager.UnregisterParticleEffect(
                        ParticleEffectType.MissileTrail);
                    pseudoEffectManager.UnregisterParticleEffect(
                        ParticleEffectType.TankExplosion);
                    pseudoEffectManager.UnregisterParticleEffect(
                        ParticleEffectType.TankSpawn);
                }
            }
        }


        #endregion
    }
}
