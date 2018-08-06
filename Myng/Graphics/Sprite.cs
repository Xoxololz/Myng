using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics.Animations;
using Myng.Helpers;
using Myng.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Myng.Graphics
{
    //every sprite will extend this class
    abstract public class Sprite
    {
        #region Fields
        protected AnimationManager animationManager;

        protected Dictionary<string, Animation> animations;

        protected Texture2D texture;

        protected Texture2D frame;

        protected Vector2 position;

        protected float layer=0f;

        protected SpritePolygon collisionPolygon;

        protected float scale = 1;
        #endregion

        #region Properties
        public virtual float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                if (collisionPolygon != null)
                    collisionPolygon.Scale(value / scale);
                scale = value;
            }
        }
        //use this to mark sprite for removal
        public bool ToRemove = false;
        
        // Polygon to check collisions
        public  SpritePolygon CollisionPolygon
        {
            get
            {
                // if collisionPolygon was initialized return it 
                if (collisionPolygon != null)
                    return collisionPolygon;
                // if not return Polygon representing rectangle the same size as texture
                if (texture != null)
                {
                    return new SpritePolygon(new Rectangle((int)Position.X,
                      (int)Position.Y,
                      (int)(texture.Width * Scale),
                      (int)(texture.Height * Scale)));
                }
                else
                {
                    return new SpritePolygon(new Rectangle((int)Position.X,
                      (int)Position.Y,
                      (int)(animationManager.Animation.FrameWidth * Scale),
                      (int)(animationManager.Animation.FrameHeight * Scale)));
                }
            }
            set
            {
                collisionPolygon = value;
            }
        }

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            private set
            {
                texture = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                if (animationManager != null)
                    animationManager.Position = value;
                collisionPolygon?.MoveTo(value);
            }
        }

        public Vector2 Origin
        {
            get
            {
                if(texture != null)
                {
                    return new Vector2(texture.Width / 2, texture.Height / 2);
                }
                if(animationManager != null)
                {
                    return animationManager.Animation.FrameOrigin;
                }
                return Vector2.Zero;
            }
        }

        public Vector2 GlobalOrigin
        {
            get
            {
                return Position + Origin * Scale;
            }
        }
        #endregion

        #region Constructors
        public Sprite(Texture2D texture2D, Vector2 position)
        {
            frame = State.Content.Load<Texture2D>("frame");
            this.texture = texture2D;
            this.Position = position;
            Scale = 1f;
            collisionPolygon = new SpritePolygon(new Rectangle((int)Position.X,
                      (int)Position.Y,
                      (int)(texture.Width * Scale),
                      (int)(texture.Height * Scale)));
        }

        public Sprite(Dictionary<string, Animation> animations, Vector2 position)
        {
            this.animations = animations;
            frame = State.Content.Load<Texture2D>("frame");
            Scale = 1f;
            //if you are changing this, there might be trouble in Character origin, so dont do that unless it is necessary
            animationManager = new AnimationManager(animations.First().Value);
            this.Position = position;
            collisionPolygon = new SpritePolygon(new Rectangle((int)Position.X,
                      (int)Position.Y,
                      (int)(animationManager.Animation.FrameWidth * Scale),
                      (int)(animationManager.Animation.FrameHeight * Scale)));
        }
        #endregion

        #region Methods
        //this method will take care of pretty much everything thats happening
        //should be overridden in every child class (unless the child has no functionality whatsoever)
        public virtual void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {

        }

        //default Draw method
        public virtual void Draw(SpriteBatch spriteBatch)
        {            
            if (animationManager != null)
                animationManager.Draw(spriteBatch, Scale,layer);
            else if (texture != null)
                spriteBatch.Draw(texture: texture,position: Position, color: Color.White, layerDepth: layer);
            else throw new Exception("No texture or animation manager set for Sprite");

        }

        //method just for debugging to see the frame around sprites
        protected void DrawFrame(SpriteBatch spriteBatch)
        {
            Rectangle rectangle = ConvertPolygonToRectangle(CollisionPolygon);
            spriteBatch.Draw(texture: frame, destinationRectangle: rectangle, layerDepth: Layers.AlwaysOnTop,color: Color.White);
        }

        private Rectangle ConvertPolygonToRectangle(Polygon collisionPolygon)
        {
            var x = (int)collisionPolygon.Vertices[0].X;
            var y = (int)collisionPolygon.Vertices[0].Y;
            var width = (int)collisionPolygon.Vertices[1].X - (int)collisionPolygon.Vertices[0].X;
            var height = (int)(collisionPolygon.Vertices[2].Y - collisionPolygon.Vertices[0].Y);
            var rectangle = new Rectangle(x, y, width, height);
            return rectangle;
        }
        #endregion
    }
}
