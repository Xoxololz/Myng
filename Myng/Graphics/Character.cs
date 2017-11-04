using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myng.Graphics
{
    class Character : Sprite
    {
        protected float rotation;
        protected Vector2 direction;
        public float rotationVelocity = 3f;
        public float speed = 4f;

        protected Vector2 origin; //point the character rotates around

        public Character(Texture2D texture2D)
            :base(texture2D)
        {
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        new public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1, SpriteEffects.None, 0);
        }
    }
}
