using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Myng.Graphics
{
    /*abstract*/ public class Projectile : Sprite, ICloneable
    {
        protected Sprite parent;

        public Vector2 Direction;
        public float speed = 8f;
        public double Angle = 0;

        protected float timer = 0f;
        protected float lifespan = 3f;

        public Projectile(Texture2D texture2D, Vector2 position)
            : base(texture2D,position)
        {
            Angle = Math.Atan(Direction.Y / Direction.X);
        }

        public Projectile(Dictionary<string, Animation> animations, Vector2 position) : base(animations, position)
        {
            Angle = Math.Atan(Direction.Y / Direction.X);
        }
        
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer > lifespan)
            {
                ToRemove = true;
            }
            if(animationManager != null)
            {
                animationManager.Update(gameTime);
            }
            Position += Direction * speed;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
                spriteBatch.Draw(texture, Position, null, Color.White, (float)Angle, CollisionPolygon.Origin - Position, scale, SpriteEffects.None, 0);
            else animationManager.Draw(spriteBatch, scale, Angle, CollisionPolygon.Origin - Position);
        }

        public virtual object Clone()
        {
            var projectile = this.MemberwiseClone() as Projectile;
            projectile.animationManager = animationManager.Clone() as AnimationManager;

            return projectile;
        }
    }
}
