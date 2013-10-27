using Zunix.Gameplay;

using Microsoft.Xna.Framework.Graphics;

namespace Zunix.Gameplay.PseudoEffects
{
    public class Explosion : PseudoEffect
    {
        #region Static Graphics Data


        /// <summary>
        /// Texture for all laser projectiles.
        /// </summary>
        private static Texture2D texture;


        public override Texture2D Sprite
        {
            get { return texture; }
        }

        private static Color[] spriteData;

        public override Color[] SpriteData
        {
            get
            {
                return null ;
            }
        }

        #endregion

    }
}