using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myng.Controller;
using Myng.Helpers;
using Myng.Items;
using Myng.Items.Interfaces;
using Myng.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Myng.Graphics
{
    public class Player : Character
    {
        #region Properties

        public List<Spell> Spells;

        public Inventory Inventory;

        public Projectile Bullet { get; set; }

        public int XP { get; set; }

        public int NextLevelXP
        {
            get
            {
                return nextLevelXP;
            }
        }
        #endregion

        #region Fields

        private Input input;

        private KeyboardState currentKey;

        private KeyboardState previousKey;

        private Vector2 velocity;

        private Spell autoAttack;

        private float timer;

        private int nextLevelXP;

        private int level;

        private Vector2 attackDirection;

        //The amount of time in seconds between attacks
        private float attackSpeed;

        #endregion

        #region Constructor

        public Player(Dictionary<string, Animation> animations, Vector2 position) : base(animations, position)
        {
            currentKey = Keyboard.GetState();
            previousKey = Keyboard.GetState();
            velocity = new Vector2(0f);
            input = new Input();
            Scale = 2f;
            attackSpeed = 0.3f;
            timer = attackSpeed;
            origin = new Vector2(animations.First().Value.FrameWidth * Scale / 2, animations.First().Value.FrameHeight * Scale / 2);
            attackDirection = new Vector2(0, -1);
            Inventory = new Inventory();
            Faction = Faction.FRIENDLY;
            level = 1;
            XP = 0;
            nextLevelXP = 100;

            InitAutoattack();
            InitSpells();
        }        

        #endregion

        #region Methods

        private void InitSpells()
        {
            //testing function 
            Action<List<Sprite>> spell = (sprites) =>
            {
                var fireballAnimation = new Dictionary<string, Animation>()
                {
                    { "fireball", new Animation(State.Content.Load<Texture2D>("Projectiles/fireball"), 1, 6)
                        {
                            FrameSpeed = 0.05f
                        }
                    }
                };
                var animation = new AnimationSprite(fireballAnimation, CollisionPolygon.Origin);
                sprites.Add(animation);
            };

            Spells = new List<Spell>
            {
                new Spell(spell,15),
                new Spell(spell,15),
                new Spell(spell,15),
                new Spell(spell,15)
            };
        }

        private void InitAutoattack()
        {
            Action<List<Sprite>> autoAttackAction = (sprites) =>
            {
                var b = Bullet.Clone() as Projectile;
                b.Position = animationManager.Position + new Vector2(animationManager.Animation.FrameWidth, animationManager.Animation.FrameHeight)/2;

                b.Direction = attackDirection;
                if (b.Direction.X < 0)
                    b.Angle = Math.Atan(b.Direction.Y / b.Direction.X) + MathHelper.ToRadians(45);
                else b.Angle = Math.Atan(b.Direction.Y / b.Direction.X) + MathHelper.ToRadians(225);
                var lenght = b.Direction.Length();
                b.Direction.X = b.Direction.X / lenght;
                b.Direction.Y = b.Direction.Y / lenght;
                b.Faction = this.Faction;
                sprites.Add(b);
            };
            Func<bool> canExecute = () =>
            {                
                var coolDown = timer > attackSpeed;
                if (coolDown)
                    timer = 0;
                return coolDown;
            };

            autoAttack = new Spell(autoAttackAction,canExecute,0);
        }

        public void LevelUp()
        {
            ++level;
            XP = 0;
            nextLevelXP = (int) (100 * Math.Pow(1.25, level));
        }

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites, TileMap tileMap)
        {
            previousKey = currentKey;
            currentKey = Keyboard.GetState();
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // just temporary solution until we somehow handle player dying
            if (Health <= 0)
            {
                Health = MaxHealth;
            }

            if(XP >= nextLevelXP)
            {
                LevelUp();
            }

            Move();
            HandleAnimation();
            animationManager.Update(gameTime);
            UseItems(otherSprites);
            CastSpells(otherSprites);
            Position += velocity;
            velocity = Vector2.Zero;
            Inventory.ClearEmptyItems();

            foreach(Item item in Inventory.Items)
            {
                if (item is IUpdatable)
                    ((IUpdatable)item).Update(otherSprites);
            }
            base.Update(gameTime, otherSprites, hittableSprites, tileMap);
        }

        private void UseItem(List<Sprite> sprites, int position)
        {
            // early exit if there is no item in this slot
            if (Inventory.Items.Count < position) return;

            if (Inventory.Items[position - 1] is IUsable)
                ((IUsable)Inventory.Items[position - 1]).Use(sprites);
        }

        private void UseItems(List<Sprite> sprites)
        {
            
            if (currentKey.IsKeyDown(input.Item1) && !previousKey.IsKeyDown(input.Item1))
                UseItem(sprites, 1);
            if (currentKey.IsKeyDown(input.Item2) && !previousKey.IsKeyDown(input.Item2))
                UseItem(sprites, 2);
            if (currentKey.IsKeyDown(input.Item3) && !previousKey.IsKeyDown(input.Item3))
                UseItem(sprites, 3);
            if (currentKey.IsKeyDown(input.Item4) && !previousKey.IsKeyDown(input.Item4))
                UseItem(sprites, 4);
        }

        private void CastSpells(List<Sprite> sprites)
        {
            if (currentKey.IsKeyDown(input.Spell1) && !previousKey.IsKeyDown(input.Spell1))
                Spells[0].Cast(sprites);
            if (currentKey.IsKeyDown(input.Spell2) && !previousKey.IsKeyDown(input.Spell2))
                Spells[1].Cast(sprites);
            if (currentKey.IsKeyDown(input.Spell3) && !previousKey.IsKeyDown(input.Spell3))
                Spells[2].Cast(sprites);
            if (currentKey.IsKeyDown(input.Spell4) && !previousKey.IsKeyDown(input.Spell4))
                Spells[3].Cast(sprites);

            if(currentKey.IsKeyDown(input.ShootUp) && currentKey.IsKeyUp(input.ShootDown) && currentKey.IsKeyUp(input.ShootLeft) && currentKey.IsKeyUp(input.ShootRight))
            {
                attackDirection.X = 0;
                attackDirection.Y = -1;
                CastAutoAttack(sprites);
                animationManager.Animation.SetRow(3);
            }
            if (currentKey.IsKeyDown(input.ShootDown) && currentKey.IsKeyUp(input.ShootUp) && currentKey.IsKeyUp(input.ShootLeft) && currentKey.IsKeyUp(input.ShootRight))
            {
                attackDirection.X = 0;
                attackDirection.Y = 1;
                CastAutoAttack(sprites);
                animationManager.Animation.SetRow(0);
            }
            if (currentKey.IsKeyDown(input.ShootRight) && currentKey.IsKeyUp(input.ShootUp) && currentKey.IsKeyUp(input.ShootLeft) && currentKey.IsKeyUp(input.ShootDown))
            {
                attackDirection.X = 1;
                attackDirection.Y = 0;
                CastAutoAttack(sprites);
                animationManager.Animation.SetRow(2);
            }
            if (currentKey.IsKeyDown(input.ShootLeft) && currentKey.IsKeyUp(input.ShootUp) && currentKey.IsKeyUp(input.ShootDown) && currentKey.IsKeyUp(input.ShootRight))
            {
                attackDirection.X = -1;
                attackDirection.Y = 0;
                CastAutoAttack(sprites);
                animationManager.Animation.SetRow(1);
            }
        }

        private void CastAutoAttack(List<Sprite> sprites)
        {           
                autoAttack.Cast(sprites);
        }

        private void Move()
        {
            

            if (currentKey.IsKeyDown(input.Left))
            {
                velocity.X -= 1f;
            }
            if (currentKey.IsKeyDown(input.Right))
            {
                velocity.X += 1f;
            }
            if (currentKey.IsKeyDown(input.Up))
            {
                velocity.Y -= 1f;
            }
            if (currentKey.IsKeyDown(input.Down))
            {
                velocity.Y += 1f;
            }

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
                velocity *= speed;
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
