using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myng.Controller;
using Myng.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Myng.Graphics
{
    public class Player : Character
    {
        #region Properties

        public List<Spell> Spells;

        public List<Item> Items;

        public Projectile Bullet { get; set; }

        #endregion

        #region Fields

        private Input input;

        private KeyboardState currentKey;

        private KeyboardState previousKey;

        private Vector2 velocity;

        #endregion

        #region Constructor

        public Player(Dictionary<string, Animation> animations, Vector2 position) : base(animations, position)
        {
            currentKey = Keyboard.GetState();
            previousKey = Keyboard.GetState();
            velocity = new Vector2(0f); 
            input = new Input();
            scale = 2f;
            origin = new Vector2(animations.First().Value.FrameWidth * scale / 2, animations.First().Value.FrameHeight * scale / 2);

            Items = new List<Item>();

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

            CastSpells(sprites);
            Move();
            if (animationManager != null)
            {
                HandleAnimation();
                animationManager.Update(gameTime);
            }
            Position += velocity;
            velocity = Vector2.Zero;
            ClearEmptyItems();
        }

        private void ClearEmptyItems()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Count <= 0)
                {
                    Items.RemoveAt(i);
                    i--;
                }
            }
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
                velocity.X -= speed;
            }
            if (currentKey.IsKeyDown(input.Right))
            {
                velocity.X += speed;
            }
            if (currentKey.IsKeyDown(input.Up))
            {
                velocity.Y -= speed;
            }
            if (currentKey.IsKeyDown(input.Down))
            {
                velocity.Y += speed;
            }
        }

        private void HandleAnimation()
        {
            animationManager.Animation.IsLooping = true;
            if (velocity.X > 0)
                animationManager.Animation.SetRow(2); //walking right
            else if (velocity.X < 0)
                animationManager.Animation.SetRow(1); //walking left
            else if (velocity.Y > 0)
                animationManager.Animation.SetRow(0); //walking down
            else if (velocity.Y < 0)
                animationManager.Animation.SetRow(3); //walking up
            else animationManager.Animation.IsLooping = false;
        }

        #endregion
    }
}
