using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myng.Helpers;
using Myng.Helpers.SoundHandlers;
using Myng.AI.Movement;
using Myng.States;

namespace Myng.Graphics.Enemies
{
    public class Enemy : Character
    {
        #region Properties

        public List<Spell> Spells;

        public Projectile Bullet { get; set; }

        public int XPDrop { get; set; }

        #endregion

        #region Fields

        protected Spell autoAttack;

        protected float timer;

        protected float attackSpeed = 1f;

        protected float attackRange = 500;

        protected Vector2 playerPosition;

        protected MovementAI movementAI;

        #endregion

        #region Constructors
        public Enemy(Dictionary<string, Animation> animations, Vector2 position) : base(animations, position)
        {
            InitAutoattack();
            Scale = 1.5f;
            speed = 1f;
            timer = attackSpeed;
            Faction = Faction.ENEMY;
            XPDrop = 10;
            movementAI = new MovementAI(CollisionPolygon, this);
            //temporary for testing purposes
            movementAI.SetGoalDestination(new Vector2(3148,Position.Y));
        }

        #endregion

        #region Methods

        protected virtual void InitAutoattack()
        {
            Action<List<Sprite>> autoAttackAction = (sprites) =>
            {
                var b = Bullet.Clone() as Projectile;
                var bPosition = GlobalOrigin - Bullet.Origin * Bullet.Scale;

                 var attackDirection= -(Position - (playerPosition));
                double bAngle;
                if (attackDirection.X < 0)
                    bAngle = Math.Atan(attackDirection.Y / attackDirection.X) + MathHelper.ToRadians(45);
                else bAngle = Math.Atan(attackDirection.Y / attackDirection.X) + MathHelper.ToRadians(225);

                b.Initialize(bPosition, 5, attackDirection, Faction, bAngle,
                    SoundsDepository.FireballFlying.CreateInstance(), SoundsDepository.FireballExplosion.CreateInstance());

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

            autoAttack = new Spell(autoAttackAction, canExecute, 0);
        }

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            UpdateTimer(gameTime);
            playerPosition = Game1.Player.Position;
            DetermineVelocity(hittableSprites);
            if (velocity == Vector2.Zero)
            {
                movementAI.SetGoalDestination(new Vector2(1, Position.Y));
            }

            HandleAnimation();
            animationManager.Update(gameTime);
            CastAutoattack(otherSprites);
            base.Update(gameTime, otherSprites, hittableSprites);
        }

        private void UpdateTimer(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void DetermineVelocity(List<Sprite> hittableSprites)
        {
            velocity = movementAI.GetVelocity();
            velocity *= speed;
            if (CollidesWithNewPosition(hittableSprites))
                DealWithCollisions(hittableSprites);
        }

        private bool CollidesWithNewPosition(List<Sprite> hittableSprites)
        {
            Position += velocity;
            if (CheckCollisions(hittableSprites) == true)
            {
                Position -= velocity;
                return true;
            }
            return false;
        }

        private void DealWithCollisions(List<Sprite> hittableSprites)
        {
            Vector2 velocityCopy = new Vector2(velocity.X, velocity.Y);

            if (Math.Abs(velocityCopy.X) > Math.Abs(velocityCopy.Y))
            {
                ChangeVelocity(new Vector2(velocityCopy.X, 0));
                if (!CollidesWithNewPosition(hittableSprites))
                    return;
                else
                {
                    ChangeVelocity(new Vector2(0, velocityCopy.Y));
                    if (!CollidesWithNewPosition(hittableSprites))
                        return;
                    else
                    {
                        ChangeVelocity(new Vector2(-velocityCopy.X, 0));
                        if (!CollidesWithNewPosition(hittableSprites))
                            return;
                        else
                        {
                            ChangeVelocity(new Vector2(0, -velocityCopy.Y));
                            if (!CollidesWithNewPosition(hittableSprites))
                                return;
                        }
                    }
                }
            }
            else
            {
                ChangeVelocity(new Vector2(0, velocityCopy.Y));
                if (!CollidesWithNewPosition(hittableSprites))
                    return;
                else
                {
                    ChangeVelocity(new Vector2(velocityCopy.X, 0));
                    if (!CollidesWithNewPosition(hittableSprites))
                        return;
                    else
                    {
                        ChangeVelocity(new Vector2(0, -velocityCopy.Y));
                        if (!CollidesWithNewPosition(hittableSprites))
                            return;
                        else
                        {
                            ChangeVelocity(new Vector2(-velocityCopy.X, 0));
                            CollidesWithNewPosition(hittableSprites);
                        }
                    }
                }
            }
        }

        private void ChangeVelocity(Vector2 vector2)
        {
            velocity = vector2;
            if (vector2 != Vector2.Zero)
                velocity.Normalize();
            velocity *= speed;
        }

        private bool CheckCollisions(List<Sprite> sprites)
        {
            foreach (var sprite in sprites)
            {
                //TODO: somehow handle AIs crashing into each other
                //if (CheckCollision(sprite))
                //{
                //    //movementAI.RecalculatePath();
                //    return true;
                //}
            }
            if (CheckCollision(Game1.Player))
            {
                return true;
            }

            return false;
        }

        private bool CheckCollision(Sprite sprite)
        {
            int minDistance = 45;
            if (Vector2.Distance(CollisionPolygon.Origin, sprite.CollisionPolygon.Origin) < minDistance)
            {
                if (sprite != this)
                    return true;
            }
            return false;
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
