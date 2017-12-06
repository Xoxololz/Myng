using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myng.Controller;
using Myng.Helpers;
using System;
using System.Collections.Generic;

namespace Myng.Graphics
{
    public class Player : Character
    {
        #region Properties

        public List<Spell> Spells;

        public Projectile Bullet { get; set; }

        #endregion

        #region Fields

        private Input input;

        private KeyboardState currentKey;

        private KeyboardState previousKey;

        #endregion

        #region Constructor

        public Player(Texture2D texture2D, Vector2 position) : base(texture2D, position)
        {
            currentKey = Keyboard.GetState();
            previousKey = Keyboard.GetState();

            input = new Input();

            //testing function to shoot basic bullet
            Action<List<Sprite>> spell =(sprites)=> {
                var b = Bullet.Clone() as Projectile;
                b.Position = this.Position;

                var mousePos = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

                b.Direction = -(Position - (mousePos - Camera.ScreenOffset));
                var lenght = b.Direction.Length();
                b.Direction.X = b.Direction.X / lenght;
                b.Direction.Y = b.Direction.Y / lenght;
                sprites.Add(b);
            };

            Spells = new List<Spell>
            {
                new Spell(spell),
                new Spell(spell),
                new Spell(spell),
                new Spell(spell)
            };

            

        }

        #endregion

        #region Methods

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            previousKey = currentKey;
            currentKey = Keyboard.GetState();

            var time = gameTime.TotalGameTime;

            CastSpells(sprites);
            Move();
        }

        private void CastSpells(List<Sprite> sprites)
        {
            if (currentKey.IsKeyDown(input.Spell1) && !previousKey.IsKeyDown(input.Spell1))
                Spells[0].Cast(sprites);
            if (currentKey.IsKeyDown(input.Spell2) && !previousKey.IsKeyDown(input.Spell2))
                Spells[1].Cast(sprites); ;
            if (currentKey.IsKeyDown(input.Spell3) && !previousKey.IsKeyDown(input.Spell3))
                Spells[2].Cast(sprites);
            if (currentKey.IsKeyDown(input.Spell4) && !previousKey.IsKeyDown(input.Spell4))
                Spells[3].Cast(sprites);
        }

        private void Move()
        {
            if (currentKey.IsKeyDown(input.Left))
            {
                Position.X -= speed;
            }
            if (currentKey.IsKeyDown(input.Right))
            {
                Position.X += speed;
            }
            if (currentKey.IsKeyDown(input.Up))
            {
                Position.Y -= speed;
            }
            if (currentKey.IsKeyDown(input.Down))
            {
                Position.Y += speed;
            }
        }

        #endregion
    }
}
