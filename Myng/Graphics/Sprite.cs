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
        protected AnimationManager animationManager;
        protected Dictionary<string, Animation> animations;
        protected Texture2D texture;
        protected Vector2 position;
        //use this to mark sprite for removal
        public bool ToRemove = false;
        protected Polygon collisionPolygon;
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
                    return new Polygon(new Rectangle((int)(Position.X - texture.Width / 2),
                      (int)(Position.Y - texture.Height / 2),
                     texture.Width,
                     texture.Height));
                }
                else
                {
                    return new Polygon(new Rectangle((int)(Position.X - animationManager.Animation.FrameWidth / 2),
                      (int)(Position.Y - animationManager.Animation.FrameHeight / 2),
                     animationManager.Animation.FrameWidth,
                     animationManager.Animation.FrameHeight));
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
                if(animationManager != null)
                    animationManager.Position = value;
            }
        }

        public Sprite(Texture2D texture2D, Vector2 position)
        {
            this.texture = texture2D;
            this.Position = position;
        }

        public Sprite(Dictionary<string, Animation> animations, Vector2 position)
        {
            this.animations = animations;
            this.Position = position;
            animationManager = new AnimationManager(animations.First().Value); //if you are changing this, there might be trouble in Character origin, so dont do that unless it is necessary
        }

        //this method will take care of pretty much everything thats happening
        //should be overridden in every child class (unless the child has no functionality whatsoever)
        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {

        }

        //default Draw method
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (animationManager != null)
                animationManager.Draw(spriteBatch);
            else if (texture != null)
                spriteBatch.Draw(texture, Position, Color.White);
            else throw new Exception("No texture or animation manager set for Sprite");

        }

    }
}
