using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Myng.States
{
    public abstract class State
    {

        protected ContentManager content;
        protected GraphicsDevice graphicsDevice;
        protected Game1 game;

        public State(ContentManager content, GraphicsDevice graphicsDevice, Game1 game)
        {
            this.content = content;
            this.graphicsDevice = graphicsDevice;
            this.game = game;
        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);

    }
}
