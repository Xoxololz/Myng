using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Controller;

namespace Myng.Graphics
{
    abstract public class Character : Sprite
    {
        protected float rotation;
        protected Vector2 direction;
        public float rotationVelocity = 3f;
        public float speed = 4f;

        protected Vector2 origin; //point the character rotates around

        public Character(Texture2D texture2D, Vector2 position)
            :base(texture2D, position)
        {
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        new public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1, SpriteEffects.None, 0);
        }
    }
}
