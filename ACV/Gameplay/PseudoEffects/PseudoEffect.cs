using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ACV
{
    public class PseudoEffect : GameObject
    {
        public override Texture2D Sprite
        {
            get { throw new NotImplementedException(); }
        }

        public override Color[] SpriteData
        {
            get { throw new NotImplementedException(); }
        }

        // The object that the pseudoeffect happened on.
        public GameObject GameObject { get; set; }

        public PseudoEffect()
        {
            Alive = true;
        }
    }
}