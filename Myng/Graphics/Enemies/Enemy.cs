using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myng.Helpers;

namespace Myng.Graphics.Enemies
{
    public class Enemy : Character
    {
        #region Properties

        public List<Spell> Spells;

        public Projectile Bullet { get; set; }

        #endregion

        #region Fields

        protected Spell autoAttack;

        protected float timer;

        protected float attackSpeed = 0.5f;

        protected Vector2 velocity;

        protected float attackRange=500;

        protected Vector2 playerPosition;

        #endregion

        #region Constructors
        public Enemy(Dictionary<string, Animation> animations, Vector2 position) : base(animations, position)
        {
            InitAutoattack();
            Scale = 2f;
            speed = 1f;
            timer = attackSpeed;
        }

        #endregion

        #region Methods

        protected virtual void InitAutoattack()
        {
            Action<List<Sprite>> autoAttackAction = (sprites) =>
            {
                var b = Bullet.Clone() as Projectile;
                b.Position = CollisionPolygon.Origin;

                b.Direction = -(Position - (playerPosition));
                b.Direction.Normalize();
                if (b.Direction.X < 0)
                    b.Angle = Math.Atan(b.Direction.Y / b.Direction.X) + MathHelper.ToRadians(45);
                else b.Angle = Math.Atan(b.Direction.Y / b.Direction.X) + MathHelper.ToRadians(225);
                b.Parent = this;
                sprites.Add(b);
            };
            Func<bool> canExecute = () =>
            {
                var AutoattackRange = (Position - playerPosition).Length() < attackRange;
                var coolDown = timer > attackSpeed;
                if (coolDown)
                    timer = 0;
                return AutoattackRange && coolDown;
            };

            autoAttack = new Spell(autoAttackAction, canExecute);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            UpdateTimer(gameTime);
            playerPosition = FindPlayer(sprites).Position;
            Move();
            HandleAnimation();
            animationManager.Update(gameTime);
            CastAutoattack(sprites);
        }

        private void UpdateTimer(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private Player FindPlayer(List<Sprite> sprites)
        {
            Player player = null;

            foreach (var sprite in sprites)
            {
                player = sprite as Player;
                if (player != null) break;
            }
            if (player == null) throw new Exception("player not found in sprites");

            return player;
        }


        private void Move()
        {
            velocity = playerPosition - Position;
            float tolerance = 5f;
            if (Math.Abs(velocity.X) < tolerance)
            {
                velocity.X = 0;
            }
            if (Math.Abs(velocity.Y) < tolerance)
            {
                velocity.Y = 0;
            }
            if (velocity.X > 0)
            {
                velocity.X = 1;
            }
            else if(velocity.X<0)
            {
                velocity.X = -1;
            }
            if (velocity.Y > 0)
            {
                velocity.Y = 1;
            }
            else if (velocity.Y < 0)
            {
                velocity.Y = -1;
            }
            velocity *= speed;
            Position += velocity;
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


        protected void CastAutoattack(List<Sprite> sprites)
        {
            autoAttack.Cast(sprites);
        }
        
        #endregion
    }
}
