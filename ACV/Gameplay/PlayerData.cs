using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;

namespace ACV
{
    /// <summary>
    /// Data for each player in a network session.
    /// </summary>
    public class PlayerData
    {
        #region Gameplay Data


        ///// <summary>
        ///// The tank model to use.
        ///// </summary>
        //private byte tankVariation = 0;
        //public byte TankVariation
        //{
        //    get { return tankVariation; }
        //    set
        //    {
        //        if ((value < 0) || (value >= Tank.Variations))
        //        {
        //            throw new ArgumentOutOfRangeException("value");
        //        }
        //        tankVariation = value;
        //        // apply the change to the tank immediately 
        //        if (tank != null)
        //        {
        //            tank.Variation = tankVariation;
        //        }
        //    }
        //}


        /// <summary>
        /// The tank used by this player.
        /// </summary>
        private Tank tank = null;
        public Tank Tank
        {
            get { return tank; }
            set { tank = value; }
        }


        #endregion


        #region Initialization Methods


        /// <summary>
        /// Constructs a PlayerData object.
        /// </summary>
        public PlayerData()
        {
            Tank = new Tank();
        }


        #endregion


        #region Networking Methods


        /// <summary>
        /// Deserialize from the packet into the current object.
        /// </summary>
        /// <param name="packetReader">The packet reader that has the data.</param>
        public void Deserialize(PacketReader packetReader)
        {
            // safety-check the parameter, as it must be valid.
            if (packetReader == null)
            {
                throw new ArgumentNullException("packetReader");
            }

            //TankColor = packetReader.ReadByte();
            //TankVariation = packetReader.ReadByte();
        }


        /// <summary>
        /// Serialize the current object to a packet.
        /// </summary>
        /// <param name="packetWriter">The packet writer that receives the data.</param>
        public void Serialize(PacketWriter packetWriter)
        {
            // safety-check the parameter, as it must be valid.
            if (packetWriter == null)
            {
                throw new ArgumentNullException("packetWriter");
            }

            //packetWriter.Write(TankColor);
            //packetWriter.Write(TankVariation);
        }


        #endregion
    }
}
