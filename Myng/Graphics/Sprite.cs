﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers;
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

        protected Vector2 position;

        protected float layer=0f;

        protected Polygon collisionPolygon;
        #endregion

        #region Properties
        public float Scale { get; set; }
        //use this to mark sprite for removal
        public bool ToRemove = false;
        
        // Polygon to check collisions
        public Polygon CollisionPolygon
        {
            get
            {
                // if collisionPolygon was initialized return it 
                if (collisionPolygon != null)
                    return collisionPolygon;
                // if not return Polygon representing rectangle the same size as texture
                if (texture != null)
                {
                    return new Polygon(new Rectangle((int)Position.X,
                      (int)Position.Y,
                      (int)(texture.Width * Scale),
                      (int)(texture.Height * Scale)));
                }
                else
                {
                    return new Polygon(new Rectangle((int)Position.X,
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
            }
        }
        #endregion

        #region Constructors
        public Sprite(Texture2D texture2D, Vector2 position)
        {
            this.texture = texture2D;
            this.Position = position;
            Scale = 1f;
        }

        public Sprite(Dictionary<string, Animation> animations, Vector2 position)
        {
            this.animations = animations;
            
            Scale = 1f;
            //if you are changing this, there might be trouble in Character origin, so dont do that unless it is necessary
            animationManager = new AnimationManager(animations.First().Value);
            this.Position = position;
        }
        #endregion

        #region Methods
        //this method will take care of pretty much everything thats happening
        //should be overridden in every child class (unless the child has no functionality whatsoever)
        public virtual void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites, TileMap tileMap)
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
        #endregion
    }
}
