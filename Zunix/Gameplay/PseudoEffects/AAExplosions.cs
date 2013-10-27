using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Zunix.Gameplay;

namespace Zunix.Gameplay.PseudoEffects
{
    public class AAExplosions
    {
        private List<Explosion> explosions = new List<Explosion>();

        private const int maxAA = 4;

        // Rectangle the size of the viewport to check if a laser is outside and alive
        private Rectangle viewportRect;

        public List<Explosion> ExplosionsList
        {
            get { return explosions; }
            set { explosions = value; }
        }
	
        public AAExplosions()
        {
            for (int i = 0; i < maxAA; i++)
            {
                explosions.Add(new Explosion());
            }
        }

        //public void LoadContent(GraphicsDevice device, SpriteBatch spriteBatch, Texture2D explosionAnimation, Vector2 sourceSize)
        //{

        //    foreach (Explosion aaex in explosions)
        //    {
        //        aaex.LoadContent(device, spriteBatch, explosionAnimation, sourceSize);
        //    }

        //}


        public void Update(float elapsedTime)
        {
            foreach (Explosion aaex in explosions)
            {                
                // If the explosion is alive slowly make the explosion invisble
                if (aaex.Alive == true)
                {
                    // If the alpha channel is less than 0 make it 'dead'
                    if (aaex.Color.A - (byte)(Math.Ceiling(elapsedTime.ElapsedGameTime.TotalSeconds * 700)) <= (byte)0)
                    {
                        aaex.Alive = false;
                    }
                    else
                    {
                        aaex.Color = new Color(255, 255, 255, (byte)(aaex.Color.A - (byte)(Math.Ceiling(elapsedTime.ElapsedGameTime.TotalSeconds * 700))));
                        aaex.Position = new Vector2(aaex.Position.X, aaex.Position.Y + (float)(elapsedTime.ElapsedGameTime.TotalSeconds * 100));
                    }
                }
                // Check to see if the texture is alive, if not make it invisible
                if (aaex.Alive == false)
                {
                    aaex.Color = new Color(0, 0, 0, 0);
                }
            }
        }
        public void Draw(float elapsedTime)
        {
            // Draw each explosion
            foreach (AniObject aaex in explosions)
            {
                aaex.Draw();
            }
        }
    }
}
