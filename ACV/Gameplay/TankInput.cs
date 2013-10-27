using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;


namespace ACV
{
    /// <summary>
    /// Player input abstraction for tanks.
    /// </summary>
    public struct TankInput
    {
        #region Static Constants


        /// <summary>
        /// The empty version of this structure.
        /// </summary>
        private static TankInput empty =
            new TankInput(Vector2.Zero, false);
        public static TankInput Empty
        {
            get { return empty; }
        }


        #endregion


        #region Input Data


        /// <summary>
        /// The left-stick value of the tank input, used for movement.
        /// </summary>
        public Vector2 LeftStick;

        /// <summary>
        /// If true, the player is trying to fire a mine.
        /// </summary>
        public bool MineFired;

        #endregion


        #region Initialization Methods


        /// <summary>
        /// Constructs a new TankInput object.
        /// </summary>
        /// <param name="leftStick">The left-stick value of the tank input.</param>
        /// <param name="mineFired">If true, the player is firing a mine.</param>
        public TankInput(Vector2 leftStick, bool mineFired)
        {
            this.LeftStick = leftStick;
            this.MineFired = mineFired;
        }


        /// <summary>
        /// Create a new TankInput object based on the data in the packet.
        /// </summary>
        /// <param name="packetReader">The packet with the data.</param>
        public TankInput(PacketReader packetReader)
        {
            // safety-check the parameters, as they must be valid
            if (packetReader == null)
            {
                throw new ArgumentNullException("packetReader");
            }

            // read the data
            LeftStick = packetReader.ReadVector2();
            MineFired = packetReader.ReadBoolean();
        }


        /// <summary>
        /// Create a new TankInput object based on local input state.
        /// </summary>
        /// <param name="gamePadState">The local gamepad state.</param>
        public TankInput(ZunePadState zunePadState)
        {
            // check for movement action
            LeftStick = Vector2.Zero;

            if (LeftStick.LengthSquared() > 0)
            {
                LeftStick.Normalize();
            }

             else
            {
                LeftStick = zunePadState.TouchPosition;
            }

            // check for firing a shot
            MineFired = false;

            MineFired = zunePadState.PlayButton == ButtonState.Pressed;
        }


        #endregion


        #region Networking Methods


        /// <summary>
        /// Serialize the object out to a packet.
        /// </summary>
        /// <param name="packetWriter">The packet to write to.</param>
        public void Serialize(PacketWriter packetWriter)
        {
            // safety-check the parameters, as they must be valid
            if (packetWriter == null)
            {
                throw new ArgumentNullException("packetWriter");
            }

            packetWriter.Write((int)World.PacketTypes.TankInput);

            packetWriter.Write(LeftStick);
            packetWriter.Write(MineFired);
        }


        #endregion
    }
}
