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

        private float timer = 0f;
        private float lifespan = 3f;
        private double angle = 0;

        public Projectile(Texture2D texture2D, Vector2 position)
            : base(texture2D,position)
        {
            angle = Math.Atan(Direction.Y / Direction.X);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            angle = Math.Atan(Direction.Y / Direction.X);

            if (timer > lifespan)
            {
                ToRemove = true;
            }

            Position += Direction * speed;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, (float)angle, CollisionPolygon.Origin - Position, 1f, SpriteEffects.None, 0);
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
