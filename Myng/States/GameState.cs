using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;

namespace Myng.States
{
    public class GameState : State
    {

        private List<Sprite> sprites;

        public GameState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game) : base(content, graphicsDevice, game)
        {
            //TODO: initialize sprites?
            sprites = new List<Sprite>();
            Sprite a = new TestSprite(content.Load<Texture2D>("worm"), new Vector2(100f));
            sprites.Add(a);
        }

        public override void Update(GameTime gameTime)
        {
            //update all sprites
            foreach (var sprite in sprites)
                sprite.Update(gameTime, sprites);

            //delete sprites that are marked for removal
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i].toRemove)
                {
                    sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {           
            foreach (var sprite in sprites)
                sprite.Draw(spriteBatch);
        }       
    }
}
