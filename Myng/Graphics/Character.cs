using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Myng.Graphics
{
    abstract public class Character : Sprite
    {
        #region Fields

        protected float speed = 4f;

        //point the character rotates around
        protected Vector2 origin;

        #endregion

        #region Properties

        public int Health { get; set; }

        #endregion

        #region Constructor

        public Character(Texture2D texture2D, Vector2 position)
            :base(texture2D, position)
        {
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        #endregion

        #region Methods

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0, origin, 1f, SpriteEffects.None, 0);
        }

        #endregion
    }
}
