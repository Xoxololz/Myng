using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myng.Controller;
using Myng.Helpers;
using Myng.Helpers.SoundHandlers;
using Myng.Items.Interfaces;
using Myng.States;
using System;
using System.Collections.Generic;
using Myng.Graphics.Animations;
using Myng.Graphics.GUI;

namespace Myng.Graphics
{
    public class Player : Character
    {
        #region Properties

        public Spellbar Spellbar { get; private set; }

        public Inventory Inventory { get; private set; }

        public Projectile Bullet { get; set; }

        //The amount of time in seconds between attacks
        public float AttackSpeed { get; set; }

        public int XP { get; set; }

        public int NextLevelXP
        {
            get
            {
                return nextLevelXP;
            }
        }

        public int Level
        {
            get
            {
                return level;
            }
        }
        #endregion

        #region Fields

        private Input input;

        private KeyboardState currentKey;

        private KeyboardState previousKey;

        private Spell autoAttack;

        private float timer;

        private int nextLevelXP;

        private int level;

        private Vector2 attackDirection;
        private Dictionary<string, Animation> playerAnimations;
        private Vector2 vector2;

        #endregion

        #region Constructor

        public Player(Dictionary<string, Animation> animations, Vector2 position) : base(animations, position)
        {
            currentKey = Keyboard.GetState();
            previousKey = Keyboard.GetState();
            velocity = new Vector2(0f);
            input = new Input();
            Scale = 1.5f;
            AttackSpeed = 0.6f;
            timer = AttackSpeed;
            attackDirection = new Vector2(0, -1);
            Inventory = new Inventory();
            Spellbar = new Spellbar();
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
                var animation = new AnimationSprite(fireballAnimation, GlobalOrigin);
                animation.Position -= animation.Origin * animation.Scale;
                sprites.Add(animation);
            };

            Spellbar.Add(new Spell(spell, 10, State.Content.Load<Texture2D>("Projectiles/fireball_icon")));
            Spellbar.Add(new Spell(spell, 15, State.Content.Load<Texture2D>("Projectiles/projectile")));
            Spellbar.Add(new Spell(spell, 20, State.Content.Load<Texture2D>("Projectiles/fireball_icon")));
            Spellbar.Add(new Spell(spell, 25, State.Content.Load<Texture2D>("Projectiles/projectile")));
        }

        private void InitAutoattack()
        {
            Action<List<Sprite>> autoAttackAction = (sprites) =>
            {
                var b = Bullet.Clone() as Projectile;
                var bPosition = GlobalOrigin - Bullet.Origin*Bullet.Scale;
               
                double bAngle;
                if (attackDirection.X < 0)
                    bAngle = Math.Atan(attackDirection.Y / attackDirection.X) + MathHelper.ToRadians(45);
                else bAngle = Math.Atan(attackDirection.Y / attackDirection.X) + MathHelper.ToRadians(225);                

                b.Initialize(bPosition, 10, attackDirection, Faction, bAngle,
                    SoundsDepository.FireballFlying.CreateInstance(), SoundsDepository.FireballExplosion.CreateInstance());

                sprites.Add(b);
            };
            Func<bool> canExecute = () =>
            {                
                var coolDown = timer > AttackSpeed;
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

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
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

            Move(hittableSprites);
            HandleAnimation();
            animationManager.Update(gameTime);
            UsePotions(otherSprites);
            CastSpells(otherSprites);

            /* We will want to update only equiped items */
            //foreach(Item item in Inventory.Items)
            //{
            //    if (item is IUpdatable)
            //        ((IUpdatable)item).Update(otherSprites);
            //}
            //base.Update(gameTime, otherSprites, hittableSprites);
        }

        private void UsePotions(List<Sprite> sprites)
        {
            if (currentKey.IsKeyDown(input.HealthPotion) && !previousKey.IsKeyDown(input.HealthPotion))
            {
                ((IUsable)Inventory.HealthPotion)?.Use(sprites);
            }
            if (currentKey.IsKeyDown(input.ManaPotion) && !previousKey.IsKeyDown(input.ManaPotion))
            {
                ((IUsable)Inventory.ManaPotion)?.Use(sprites);
            }
        }

        private void CastSpells(List<Sprite> sprites)
        {
            if (currentKey.IsKeyDown(input.Spell1) && !previousKey.IsKeyDown(input.Spell1))
                Spellbar.GetSpell(0)?.Cast(sprites);
            if (currentKey.IsKeyDown(input.Spell2) && !previousKey.IsKeyDown(input.Spell2))
                Spellbar.GetSpell(1)?.Cast(sprites);
            if (currentKey.IsKeyDown(input.Spell3) && !previousKey.IsKeyDown(input.Spell3))
                Spellbar.GetSpell(2)?.Cast(sprites);
            if (currentKey.IsKeyDown(input.Spell4) && !previousKey.IsKeyDown(input.Spell4))
                Spellbar.GetSpell(3)?.Cast(sprites);
            if (currentKey.IsKeyDown(input.Spell5) && !previousKey.IsKeyDown(input.Spell5))
                Spellbar.GetSpell(4)?.Cast(sprites);
            if (currentKey.IsKeyDown(input.Spell6) && !previousKey.IsKeyDown(input.Spell6))
                Spellbar.GetSpell(5)?.Cast(sprites);

            if (currentKey.IsKeyDown(input.ShootUp) && currentKey.IsKeyUp(input.ShootDown) && currentKey.IsKeyUp(input.ShootLeft) && currentKey.IsKeyUp(input.ShootRight))
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

        private void Move(List<Sprite> hittableSprites)
        {
            velocity = Vector2.Zero;
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
            
            if (CollidesWithNewPosition(hittableSprites))
            {
                DealWithPrimitiveCollisions(hittableSprites);
            }
            else
            {
                Position += velocity;
            }
        }
        
        protected override bool CollidesWithNewPosition(List<Sprite> hittableSprites)
        {
            Position += velocity;
            if (CheckCollisions(hittableSprites))
            {
                Position -= velocity;
                return true;
            }
            if (CheckCollisionWithTerrain())
            {
                Position -= velocity;
                return true;
            }
            Position -= velocity;
            return false;
        }

        protected override bool CheckCollisions(List<Sprite> sprites)
        {
            foreach (var sprite in sprites)
            {
                if (CheckCollision(sprite))
                    return true;
            }
            if (CheckCollisionWithTerrain())
                return true;
            return false;
        }


        private bool CheckCollision(Sprite sprite)
        {
            //int minDistance = 35;
            //if (Vector2.Distance(CollisionPolygon.Origin, sprite.CollisionPolygon.Origin) < minDistance)
            //{
            //    return true;
            //}
            //return false;
            return CollisionPolygon.Intersects(sprite.CollisionPolygon);
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
            else
                animationManager.Animation.IsLooping = false;
            
        }

        #endregion
    }
}
