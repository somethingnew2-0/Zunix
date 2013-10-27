using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Zunix.Gameplay.PseudoEffects;

namespace Zunix.Gameplay
{
    public abstract class GameObject
    {
        #region Status Properties

        private bool alive = false;
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }
 
        #endregion

        #region Graphics Properties

        //private SpriteBatch spriteBatch;
        //private Texture2D texture;
        private Vector2 position;
        private Vector2 intialposition;
        //private Rectangle source;
        //// All textures are invisible at first
        //private Color color = new Color(0, 0, 0, 0);
        //private Vector2 origin;
        //private Vector2 scale;
        private float rotation;
        //private SpriteEffects effects = SpriteEffects.None;
        //private float layer = 0.5f;
        //// The X and Y size of every source frame of a loadedTexture and with the scale involved
        //private Vector2 actualsize;
        private float speed;

        //public SpriteBatch SpriteBatch
        //{
        //    get { return spriteBatch; }
        //    set { spriteBatch = value; }
        //}

        //public Texture2D Sprite
        //{
        //    get { return texture; }
        //    set { texture = value; }
        //}

        public abstract Texture2D Sprite
        {
            get;
        }

        public abstract Color[] SpriteData
        {
            get;
        }
       
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 IntialPosition
        {
            get { return intialposition; }
            set { intialposition = value; }
        }

        public virtual float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        //public Rectangle Source
        //{
        //    get { return source; }
        //    set { source = value; }
        //}

        //public Color Color
        //{
        //    get { return color; }
        //    set { color = value; }
        //}

        ///// <summary>
        ///// The center coordinates for the sprite by itself, not in the playing field.
        ///// </summary>
        //public Vector2 Origin
        //{
        //    get { return origin; }
        //    set { origin = value; }
        //}

        ///// <summary>
        ///// Can't be changed from 1.0f unless it's set as an overload.
        ///// </summary>
        //public Vector2 Scale
        //{
        //    get { return scale; }
        //}

        //public SpriteEffects SpriteEffects
        //{
        //    get { return effects; }
        //    set { effects = value; }
        //}

        ///// <summary>
        ///// Default is 0.5f, but can be changed from 0.0f to 1.0f.
        ///// </summary>
        //public float Layer
        //{
        //    get { return layer; }
        //    set { layer = value; }
        //}

        ///// <summary>
        ///// The ActualSize is a Vector2 for the X being the width and Y the Height of the actual
        ///// sprite with scale change, but without source because SourceSize works just as well.
        ///// </summary>
        //public Vector2 ActualSize
        //{
        //    get { return actualsize; }
        //}

        public Vector2 Speed { get; set; }

        #endregion

        #region Collision Data

        protected float mass = 1f;
        public float Mass
        {
            get { return mass; }
        }

        protected bool collidedThisFrame = false;
        public bool CollidedThisFrame
        {
            get { return collidedThisFrame; }
            set { collidedThisFrame = value; }
        }

        //public bool UpdateHull { get; set; }

        //public List<Point> Hull { get; set; }

        //public Dictionary<Point, bool> HullTransformed { get; set; }

        //public int MinX { get; set; }
        //public int MinY { get; set; }
        //public int MaxX { get; set; }
        //public int MaxY { get; set; }

        //private BoundingSphere boundingSphere;
        //public BoundingSphere BoundingSphere
        //{
        //    get { return boundingSphere; }
        //    set { boundingSphere = value; }
        //}

        #endregion


        #region Main Methods

        public GameObject()
        {
            //this.scale = Vector2.One;

            if (!alive)
            {
                alive = true;
                
                if (this is PseudoEffect == false)
                {
                    CollisionManager.Collection.Add(this);  
                }
            }
            
        }
        //public virtual void LoadContent(SpriteBatch spriteBatch, Texture2D loadedTexture)
        //{
        //    //this.spriteBatch = spriteBatch;
        //    //this.texture = loadedTexture;


        //    //// Find the actual size of the sprite after its been scaled
        //    //this.actualsize = new Vector2((int)(this.scale.X * this.texture.Width), (int)(this.scale.Y * this.texture.Height));

        //    //this.source = new Rectangle(0, 0, (int)this.texture.Width, (int)this.texture.Height);

        //    //// The "origin" of a sprite is the point
        //    //// halfway down its width and height.
        //    //this.origin = new Vector2(
        //    //    (float)(loadedTexture.Width) / 2, (float)(loadedTexture.Height) / 2);

        //    MinX = texture.Width;
        //    MinY = texture.Height;
        //    MaxX = -texture.Width;
        //    MaxY = -texture.Height;



        //    //boundingSphere.Center = new Vector3(position, 0);
        //    //boundingSphere.Radius = (float)Math.Sqrt(texture.Width * texture.Width + texture.Height * texture.Height) / 2.0f;
        //}

        //public virtual void LoadContent(SpriteBatch spriteBatch, Texture2D loadedTexture, Vector2 inputedscale)
        //{
        //    //this.spriteBatch = spriteBatch;
        //    //this.texture = loadedTexture;

        //    //// If the X or Y is greater than 1 or less than 0 set the scale to 1
        //    //if (inputedscale.X >= 0.0f & inputedscale.X <= 1.0f)
        //    //{
        //    //    if (inputedscale.Y >= 0.0f & inputedscale.Y <= 1.0f)
        //    //    {
        //    //        this.scale = (Vector2)(new Vector2(240, 320) / new Vector2(240, 320)) * inputedscale;
        //    //    }
        //    //    else
        //    //    {
        //    //        this.scale = (Vector2)(new Vector2(240, 320) / new Vector2(240, 320));
        //    //        this.scale.X *= inputedscale.X;
        //    //    }
        //    //}
        //    //else if (inputedscale.Y >= 0.0f & inputedscale.Y <= 1.0f)
        //    //{
        //    //    this.scale = (Vector2)(new Vector2(240, 320) / new Vector2(240, 320));
        //    //    this.scale.Y *= inputedscale.Y;
        //    //}
        //    //else
        //    //{
        //    //    this.scale = new Vector2(240, 320) / new Vector2(240, 320);
        //    //}

        //    //// Find the actual size of the sprite after its been scaled
        //    //this.actualsize = new Vector2((int)(this.scale.X * this.source.Width), (int)(this.scale.Y * this.source.Height));

        //    //this.source = new Rectangle(0, 0, (int)this.texture.Width, (int)this.texture.Height); ;

        //    //// The "origin" of a sprite is the point
        //    //// halfway down its width and height.
        //    //this.origin = new Vector2(
        //    //    loadedTexture.Width / 2, loadedTexture.Height / 2);

        //    MinX = texture.Width;
        //    MinY = texture.Height;
        //    MaxX = -texture.Width;
        //    MaxY = -texture.Height;

        //    //CollisionMath.CalcHull(this);

        //    //boundingSphere.Center = new Vector3(position, 0);
        //    //boundingSphere.Radius = (float)Math.Sqrt(texture.Width * texture.Width + texture.Height * texture.Height) / 2.0f;
        //}


        #region Updating Methods


        /// <summary>
        /// Update the gameplay object.
        /// </summary>
        /// <param name="elapsedTime">The amount of elapsed time, in seconds.</param>
        public virtual void Update(float elapsedTime)
        {
            collidedThisFrame = false;
        }


        #endregion


        //public virtual void Draw()
        //{
        //    this.spriteBatch.Draw(this.texture, this.position, this.source, this.color, this.rotation, this.origin, this.scale, this.effects, this.layer);
        //}
        //public virtual void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(this.texture, this.position, this.source, this.color, this.rotation, this.origin, this.scale, this.effects, this.layer);
        //}

        #endregion

        #region Drawing Methods


        /// <summary>
        /// Draw the gameplay object.
        /// </summary>
        /// <param name="elapsedTime">The amount of elapsed time, in seconds.</param>
        /// <param name="spriteBatch">The SpriteBatch object used to draw.</param>
        /// <param name="sprite">The texture used to draw this object.</param>
        /// <param name="color">The color of the sprite.</param>
        public virtual void Draw(float elapsedTime, SpriteBatch spriteBatch,
            Texture2D sprite, Color color)
        {
            if ((spriteBatch != null) && (sprite != null))
            {
               spriteBatch.Draw(sprite, position, null, color, rotation,
                    new Vector2(sprite.Width / 2f, sprite.Height / 2f),
                    1f, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Draw the gameplay object.
        /// </summary>
        /// <param name="elapsedTime">The amount of elapsed time, in seconds.</param>
        /// <param name="spriteBatch">The SpriteBatch object used to draw.</param>
        /// <param name="sprite">The texture used to draw this object.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="color">The color of the sprite.</param>
        public virtual void Draw(float elapsedTime, SpriteBatch spriteBatch,
            Texture2D sprite, Rectangle? sourceRectangle, Color color)
        {
            if ((spriteBatch != null) && (sprite != null))
            {
                spriteBatch.Draw(sprite, position, sourceRectangle, color, rotation,
                    new Vector2(sprite.Width / 2f, sprite.Height / 2f),
                    1f, SpriteEffects.None, 0f);
            }
        }


        #endregion

        #region Interaction Methods

        /// <summary>
        /// Defines the interaction between this GameObject and 
        /// a target GameObject when they touch.
        /// </summary>
        /// <param name="target">The GameObject that is touching this one.</param>
        /// <returns>True if the objects meaningfully interacted.</returns>
        public virtual bool Touch(GameObject target)
        {
            return true;
        }


        /// <summary>
        /// Damage this object by the amount provided.
        /// </summary>
        /// <remarks>
        /// This function is provided in lieu of a Life mutation property to allow 
        /// classes of objects to restrict which kinds of objects may damage them,
        /// and under what circumstances they may be damaged.
        /// </remarks>
        /// <param name="source">The GameObject responsible for the damage.</param>
        /// <param name="damageAmount">The amount of damage.</param>
        /// <returns>If true, this object was damaged.</returns>
        public virtual bool Damage(GameObject source, float damageAmount)
        {
            return false;
        }


        /// <summary>
        /// Kills this object, in response to the given GameObject.
        /// </summary>
        /// <param name="source">The GameObject responsible for the kill.</param>
        /// <param name="cleanupOnly">
        /// If true, the object kills without any further effects.
        /// </param>
        public virtual void Kill(GameObject source, bool cleanupOnly)
        {
            // deactivate the object
            if (alive)
            {
                alive = false;
            }
        }

        #endregion
    }
}
