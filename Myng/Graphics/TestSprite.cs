using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Controller;
using Microsoft.Xna.Framework.Input;

namespace Myng.Graphics
{
    public class TestSprite : Sprite
    {
        private Input input;

        public TestSprite(Texture2D texture2D, Vector2 position) : base(texture2D, position)
        {
            input = new Input();
            position = new Vector2();
        }

        private void Move()
        {
            if (Keyboard.GetState().IsKeyDown(input.Left))
            {
                position.X -= 5;
            }
            if (Keyboard.GetState().IsKeyDown(input.Right))
            {
                position.X += 5;
            }
            if (Keyboard.GetState().IsKeyDown(input.Up))
            {
                position.Y -= 5;
            }
            if (Keyboard.GetState().IsKeyDown(input.Down))
            {
                position.Y +=5;
            }
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Move();
        }
    }
}
