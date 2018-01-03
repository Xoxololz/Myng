using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Myng.States
{
    public abstract class State
    {
        #region Fields             

        protected GraphicsDevice graphicsDevice;

        protected Game1 game;

        #endregion

        #region Properties

        public static ContentManager Content;

        #endregion


        #region Constructors

        public State(ContentManager content, GraphicsDevice graphicsDevice, Game1 game)
        {
            State.Content = content;
            this.graphicsDevice = graphicsDevice;
            this.game = game;
        }

        #endregion

        #region Methods

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);

        #endregion
    }
}
