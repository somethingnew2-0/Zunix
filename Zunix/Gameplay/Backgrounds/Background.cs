using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Zunix.Gameplay;
using Microsoft.Xna.Framework.Content;
using System;

namespace Zunix.Gameplay.Backgrounds
{
    public abstract class Background : GameObject
    {
        private Vector2 modifiedorigin;
        private int screenheight, screenwidth;

        /// <summary>
        /// The number of variations in textures for backgrounds.
        /// </summary>
        const int variations = 1;
        public static int Variations
        {
            get { return variations; }
        }

        /// <summary>
        /// The background textures.
        /// </summary>
        private static Texture2D[] backgroundTextures = new Texture2D[variations];

        public static Texture2D[] BackgroundTextures
        {
            get { return backgroundTextures; }
            set { backgroundTextures = value; }
        }

        public Background()
        {

        }
        //public override void LoadContent(SpriteBatch spriteBatch, Texture2D backgroundTexture)
        //{
        //    base.LoadContent(spriteBatch, backgroundTexture);
            
        //    // Get the screen height and width
        //    screenheight = 320;
        //    screenwidth = 240;

        //    // Set the origin so that we're drawing from the 
        //    // center of the top edge.
        //    this.modifiedorigin = new Vector2(Sprite.Width / 2, 0);

        //    // Set the screen position to the center of the screen.
        //    yscreenpos = new Vector2(modifiedorigin.X, screenheight / 2);

        //    // Set some properties so it's visible
        //    Layer = 0f;
        //    Alive = true;

        //}

        public override void Update(float elapsedTime)
        {
            // The time since Update was called last.
            float deltaY = (float)elapsedTime * .001f;
            
            // Move the background down by the number of elapsed seconds.
            Position += new Vector2(0, deltaY);

            // Mod it and move it accordingly.
            Position = new Vector2(Position.X, Position.Y % backgroundTextures[0].Height); // TODO: Change so it picks the right backgroundTexture

            // Send to GameTexture Update
            base.Update(elapsedTime);
        }

        public override void Draw(float elapsedTime, SpriteBatch spriteBatch,
            Texture2D sprite, Color color)
        {
            // TODO: Make it so this background draw integrates with a background.Draw() method
            // Draw the texture, if it is still onscreen.
            if (this.Position.Y < screenheight)
            {
                base.Draw(elapsedTime, spriteBatch, sprite, color);

            //    SpriteBatch.Draw(sprite, yscreenpos, Source,
            //         Color.White, 0, this.modifiedorigin, 1, SpriteEffects.None, Layer + 0.1f);
            }

            // Draw the texture a second time, behind the first,
            // to create the scrolling illusion.
             
            Position -= new Vector2(0, sprite.Height);

            base.Draw(elapsedTime, spriteBatch, sprite, color);
            
            //SpriteBatch.Draw(Sprite, yscreenpos - new Vector2(0, Sprite.Height), Source,
            //     Color, Rotation, this.modifiedorigin, Scale, SpriteEffects, Layer);

            // Change the settings back
            Position += new Vector2(0, sprite.Height);
        }

        /// <summary>
        /// Load all of the static graphics content for this class.
        /// </summary>
        /// <param name="contentManager">The content manager to load with.</param>
        public static void LoadContent(ContentManager contentManager)
        {
            // safety-check the parameters
            if (contentManager == null)
            {
                throw new ArgumentNullException("contentManager");
            }

            // load each ship's texture
            for (int i = 0; i < variations; i++)
            {
                backgroundTextures[i] = contentManager.Load<Texture2D>(
                    "Backgrounds/Ground" + i.ToString());
            }
        }

        /// <summary>
        /// Unload all of the static graphics content for this class.
        /// </summary>
        public static void UnloadContent()
        {
            for (int i = 0; i < variations; i++)
            {
                backgroundTextures[i] = null;
            }
        }
    }
}
