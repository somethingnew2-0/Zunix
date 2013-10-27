//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

// TODO: Delete Me!!!

//namespace Zunix.Gameplay
//{
//    public abstract class AniObject : GameObject
//    {
//        private List<Rectangle> frames;

//        private bool isLooped;
//        private bool isStarted;
//        private float currentFrame;
//        private float framesPerSecond;

//        //private SpriteBatch spriteBatch;
//        //private GraphicsDevice device;
//        //private Texture2D texture;
//        //private Vector2 position;
//        //private Vector2 intialposition;
//        private Rectangle source;
//        //// All textures are invisible at first
//        //private Color color = new Color(0, 0, 0, 0);
//        //private Vector2 origin;
//        //private Vector2 scale;
//        //private float rotation;
//        //private SpriteEffects effects = SpriteEffects.None;
//        //private float layer = 0.5f;
//        //// The X and Y size of every source frame of a loadedTexture and with the scale involved
//        private Vector2 frameSize;
//        //// All objects should start with zero velocity, and
//        //// be "dead".
//        //private bool alive = false;    

//        public Vector2 FrameSize
//        {
//            get { return frameSize; }
//            set { frameSize = value; }
//        }

//        /// <summary>
//        /// Has the animation Started?  Default is true.
//        /// </summary>
//        public bool IsStarted
//        {
//            get { return isStarted; }
//            set { isStarted = value; }
//        }

//        /// <summary>
//        /// If true it starts the animation over every time it ends. Default it true.
//        /// </summary>
//        public bool IsLooped
//        {
//            get { return isLooped; }
//            set { isLooped = value; }
//        }

//        /// <summary>
//        /// Gets or sets the current frame of the animation.
//        /// </summary>
//        public float CurrentFrame
//        {
//            get { return currentFrame; }
//            set { currentFrame = value; }
//        }

//        /// <summary>
//        /// Gets or sets the FramePerSecond of the animation. Default is 1.0f.
//        /// </summary>
//        public float FramesPerSecond
//        {
//            get { return framesPerSecond; }
//            set { framesPerSecond = value; }
//        }


//        public AniObject(Vector2 frameSize)
//        {
//            this.frameSize = frameSize;

//            //this.scale = Vector2.One;
//        }
//        //public override void LoadContent(SpriteBatch spriteBatch, Texture2D loadedTexture, Vector2 sourceSize)
//        //{
//        //    this.spriteBatch = spriteBatch;
//        //    this.texture = loadedTexture;


//        //    // Find the actual size of the sprite after its been scaled
//        //    this.actualsize = new Vector2((int)(this.scale.X * this.texture.Width), (int)(this.scale.Y * this.texture.Height));

//        //    this.source = new Rectangle(0, 0, (int)this.texture.Width, (int)this.texture.Height);

//        //    // The "origin" of a sprite is the point
//        //    // halfway down its width and height.
//        //    this.origin = new Vector2(
//        //        (float)(this.source.Width) / 2, (float)(this.source.Height) / 2);

//        //    this.frames = new List<Rectangle>();

//        //    this.framesPerSecond = 1f;
//        //    this.currentFrame = 0f;
//        //    this.isLooped = false;
//        //    this.isStarted = false;

//        //    this.actualsize = sourceSize;
//        //    this.origin = new Vector2(this.actualsize.X / 2, this.actualsize.Y / 2);

//        //    int firstFrame = 1;
//        //    int lastFrame = 9;

//        //    for (int index = firstFrame; index <= lastFrame; index++)
//        //    {
//        //        this.frames.Add(FindSource(index));
//        //    }
//        //}
//        //public virtual void LoadContent(GraphicsDevice device, SpriteBatch spriteBatch, Texture2D loadedTexture, Vector2 sourceSize, Vector2 scale)
//        //{
//        //    this.device = device;

//        //    this.spriteBatch = spriteBatch;
//        //    this.texture = loadedTexture;

//        //    // If the X or Y is greater than 1 or less than 0 set the scale to 1
//        //    if (scale.X >= 0.0f & scale.X <= 1.0f)
//        //    {
//        //        if (scale.Y >= 0.0f & scale.Y <= 1.0f)
//        //        {
//        //            this.scale = (Vector2)(new Vector2(240, 320) / new Vector2(240, 320)) * scale;
//        //        }
//        //        else
//        //        {
//        //            this.scale = (Vector2)(new Vector2(240, 320) / new Vector2(240, 320));
//        //            this.scale.X *= scale.X;
//        //        }
//        //    }
//        //    else if (scale.Y >= 0.0f & scale.Y <= 1.0f)
//        //    {
//        //        this.scale = (Vector2)(new Vector2(240, 320) / new Vector2(240, 320));
//        //        this.scale.Y *= scale.Y;
//        //    }
//        //    else
//        //    {
//        //        this.scale = new Vector2(240, 320) / new Vector2(240, 320);
//        //    }

//        //    // Find the actual size of the sprite after its been scaled
//        //    this.actualsize = new Vector2((int)(this.scale.X * this.source.Width), (int)(this.scale.Y * this.source.Height));

//        //    this.source = new Rectangle(0, 0, (int)this.texture.Width, (int)this.texture.Height); ;

//        //    // The "origin" of a sprite is the point
//        //    // halfway down its width and height.
//        //    this.origin = new Vector2(
//        //        this.source.Width / 2, this.source.Height / 2);

//        //    this.frames = new List<Rectangle>();

//        //    this.framesPerSecond = 1f;
//        //    this.currentFrame = 0f;
//        //    this.isLooped = false;
//        //    this.isStarted = false;

//        //    this.actualsize = sourceSize;
//        //    this.origin = new Vector2(this.actualsize.X / 2, this.actualsize.Y / 2);

//        //    int firstFrame = 1;
//        //    int lastFrame = 9;

//        //    for (int index = firstFrame; index <= lastFrame; index++)
//        //    {
//        //        this.frames.Add(FindSource(index));
//        //    }
//        //}

//        public override void Update(float elapsedTime)
//        {
//            // Checks to see if there is animation
//            if (this.isStarted)
//            {
//                // Keeps track of which frame we are on with the time and framesPerSecond
//                this.currentFrame += this.framesPerSecond * (float)elapsedTime;

//                // Does the animation loop?
//                if (this.isLooped)
//                {
//                    // Start over the animation
//                    this.currentFrame %= this.frames.Count;
//                }
//                // If not stop on the last frame and stop
//                else if (this.currentFrame > this.frames.Count)
//                {
//                    this.currentFrame = this.frames.Count - 1;
//                    this.isStarted = false;
//                }
//            }
//            // Send the current frame to the Cell Class.
//            source = frames[(int)this.currentFrame];

//            //// Check to see if the loadedTexture is alive, if not make it invisible
//            //if (this.Alive == false)
//            //{
//            //    this.Color = Color.TransparentBlack;
//            //}
//            //else if (this.color != Color.TransparentBlack && this.color != Color.White && this.alive == true)
//            //{

//            //}
//            //else
//            //{
//            //    this.color = Color.White;
//            //}
//        }

//        //public override void Draw()
//        //{
//        //    this.spriteBatch.Draw(texture, position, source, color, rotation, origin, scale, effects, layer);
//        //}

//        //public override void Draw(SpriteBatch spriteBatch)
//        //{
//        //    spriteBatch.Draw(texture, position, source, color, rotation, origin, scale, effects, layer);
//        //}

//        public override void Draw(float elapsedTime, SpriteBatch spriteBatch, Texture2D sprite, Color color)
//        {
//            base.Draw(elapsedTime, spriteBatch, sprite, source, color);
//        }

//        /// <summary>
//        /// Finds the number of frames and where they're according to the index and SourceSize.
//        /// </summary>
//        /// <param name="index">An int greater than 0.</param>
//        /// <returns>The frame source according to the index number.</returns>
//        private Rectangle FindSource(Texture2D aniTexture, int index)
//        {
//            // If the entered index is less than 1 than show the whole loadedTexture
//            if (index <= 0)
//            {
//                return new Rectangle(0, 0, (int)aniTexture.Width, (int)aniTexture.Height);
//            }
//            else
//            {
//                int frameColumn = 1;
//                int frameRow = index;

//                int frameWidth = (int)frameSize.X;
//                int frameHeight = (int)frameSize.Y;

//                int totalColumns = aniTexture.Width / frameWidth;
//                int totalRows = aniTexture.Height / frameHeight;

//                while (frameRow > totalRows)
//                {
//                    frameColumn++;
//                    frameRow -= totalRows;
//                }

//                if (index > totalRows * totalColumns)
//                {
//                    frameColumn = totalColumns;
//                    frameRow = totalRows;
//                }

//                return new Rectangle(
//                    // Location.
//                    (frameColumn - 1) * frameWidth,
//                    (frameRow - 1) * frameHeight,
//                    //Size.
//                    frameWidth,
//                    frameHeight);
//            }
//        }
//    }
//}
