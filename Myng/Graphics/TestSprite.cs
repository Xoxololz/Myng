using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Controller;
using Microsoft.Xna.Framework.Input;

namespace Myng.Graphics
{
    public class TestSprite : Character
    {
        private Input input;

        public TestSprite(Texture2D texture2D, Vector2 position) : base(texture2D, position)
        {
            input = new Input();
        }

        private void Move()
        {
            if (Keyboard.GetState().IsKeyDown(input.Left))
            {
                rotation -= MathHelper.ToRadians(rotationVelocity);
            }
            if (Keyboard.GetState().IsKeyDown(input.Right))
            {
                rotation += MathHelper.ToRadians(rotationVelocity);
            }

            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(180) - rotation), -(float)Math.Sin(MathHelper.ToRadians(180) - rotation));

            if (Keyboard.GetState().IsKeyDown(input.Up))
            {
                position += speed * direction;
            }
            if (Keyboard.GetState().IsKeyDown(input.Down))
            {
                position -= speed * direction;
            }
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Move();
        }

    }
}
