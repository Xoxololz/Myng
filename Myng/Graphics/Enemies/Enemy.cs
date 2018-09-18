using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myng.Helpers;
using Myng.Helpers.SoundHandlers;
using Myng.AI.Movement;
using Myng.Graphics.Animations;
using Myng.Helpers.Enums;

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

        protected float attackRange = 500;

        protected Vector2 playerPosition;

        protected MovementAI movementAI;

        #endregion

        #region Constructors
        public Enemy(Dictionary<string, Animation> animations, Vector2 position) : base(animations, position)
        {
            InitAutoattack();
            Scale = 1.5f;
            baseSpeed = 1f;
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

                b.Initialize(bPosition, 30, DamageType.PHYSICAL, attackDirection, Faction, bAngle,
                    SoundsDepository.FireballFlying.CreateInstance(), SoundsDepository.FireballExplosion.CreateInstance(), this);

                sprites.Add(b);
            };
            Func<bool> canExecute = () =>
            {
                var AutoattackRange = (Position - playerPosition).Length() < attackRange;
                var coolDown = autoAttackTimer > AttackSpeed;
                if (coolDown)
                    autoAttackTimer = 0;
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
            autoAttackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void DetermineVelocity(List<Sprite> hittableSprites)
        {
            velocity = movementAI.GetVelocity();
            velocity *= Speed;
            if (CollidesWithNewPosition(hittableSprites))
            {
                if (!DealWithPrimitiveCollisions(hittableSprites))
                    DealWithTotalCollison(hittableSprites);
            }
            else
                Position += velocity;
        }


        protected override bool CollidesWithNewPosition(List<Sprite> hittableSprites)
        {
            Position += velocity;
            if (CheckCollisions(hittableSprites))
            {
                Position -= velocity;
                return true;
            }
            Position -= velocity;
            return false;
        }

        private void DealWithTotalCollison(List<Sprite> hittableSprites)
        {
            Position += velocity;
            var collidingPolygons = new List<Polygon>();
            foreach (var sprite in hittableSprites)
            {
                if (CheckCollision(sprite))
                {
                    collidingPolygons.Add(sprite.CollisionPolygon);
                }
            }
            if (CheckCollision(Game1.Player))
            {
                collidingPolygons.Add(Game1.Player.CollisionPolygon);
            }
            Position -= velocity;
            movementAI.FindNewPath(collidingPolygons);
        }

        protected override bool CheckCollisions(List<Sprite> sprites)
        {
            foreach (var sprite in sprites)
            {
                if (CheckCollision(sprite))
                {
                    return true;
                }
            }

            return CheckCollision(Game1.Player);
        }

        private bool CheckCollision(Sprite sprite)
        {
            return CollisionPolygon.Intersects(sprite.CollisionPolygon) && sprite!=this;
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
