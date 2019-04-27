using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myng.Helpers;
using Myng.Helpers.SoundHandlers;
using Myng.AI.Movement;
using Myng.Graphics.Animations;
using Myng.Helpers.Enums;
using Myng.AI.EnemyStates;

namespace Myng.Graphics.Enemies
{
    public class Enemy : Character
    {
        #region Properties

        public List<Spell> Spells;

        public Projectile Bullet { get; set; }

        public int XPDrop { get; set; }

        public EnemyType EnemyType { get; private set; }

        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }

        public int SightRange { get; private set; } //TODO: probly init in subclasses

        public float SpeedMultiplier { get; set; }

        public override float Speed
        {
            get
            {
                return baseSpeed * SpeedMultiplier;
            }
        }

        public State NextState
        {
            set
            {
                nextState = value;
            }
        }

        #endregion

        #region Fields

        protected Spell autoAttack;

        protected Vector2 playerPosition;

        protected MovementAI movementAI;

        protected State currentState;

        protected State nextState;

        #endregion

        #region Constructors
        public Enemy(Dictionary<string, Animation> animations, Vector2 position, EnemyType type) : base(animations, position)
        {
            InitAutoattack();
            Spells = new List<Spell>();
            InitSpells();
            Scale = 1.5f;
            baseSpeed = 1.5f;
            Faction = Faction.ENEMY;
            XPDrop = 20;
            movementAI = new MovementAI(CollisionPolygon, this);
            EnemyType = type;
            currentState = new WanderState(this);
            SightRange = 300;
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
                var AutoattackRange = (Position - playerPosition).Length() < SightRange;
                var coolDown = autoAttackTimer > AttackSpeed;
                if (coolDown)
                    autoAttackTimer = 0;
                return AutoattackRange && coolDown;                
            };

            autoAttack = new Spell(autoAttackAction, canExecute, 0);
        }

        protected virtual void InitSpells()
        {
            Action<List<Sprite>> blast = (sprites) =>
            {

                var bulletMid = Bullet.Clone() as Projectile;
                var bPosition = GlobalOrigin - Bullet.Origin * Bullet.Scale;

                 var attackDirection= -(Position - (playerPosition));
                double bAngle;
                if (attackDirection.X < 0)
                    bAngle = Math.Atan(attackDirection.Y / attackDirection.X) + MathHelper.ToRadians(45);
                else bAngle = Math.Atan(attackDirection.Y / attackDirection.X) + MathHelper.ToRadians(225);

                bulletMid.Initialize(bPosition, 40, DamageType.MAGIC, attackDirection, Faction, bAngle,
                    SoundsDepository.FireballFlying.CreateInstance(), SoundsDepository.FireballExplosion.CreateInstance(), this);
                var bulletBot = bulletMid.Clone() as Projectile;
                var bulletTop = bulletMid.Clone() as Projectile;
                bulletBot.Angle += MathHelper.ToRadians(10);
                bulletTop.Angle -= MathHelper.ToRadians(10);

                sprites.Add(bulletMid);
            };

            Func<bool> canExecute = () =>
            {
                return true;
            };
            Spells.Add(new Spell(blast, canExecute,0));

        }

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            HandleStateChange();
            currentState.Update(gameTime, otherSprites, hittableSprites);
            UpdateTimer(gameTime);
            playerPosition = Game1.Player.Position;
            DetermineVelocity(hittableSprites);
            HandleAnimation();
            animationManager.Update(gameTime);
            CastAutoattack(otherSprites);
            base.Update(gameTime, otherSprites, hittableSprites);
        }

        private void HandleStateChange()
        {
            if (nextState != null)
            {
                currentState = nextState;
                nextState = null;
            }
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

        public bool SetGoalDestination(Vector2 dest)
        {
            return movementAI.SetGoalDestination(dest);
        }

        #endregion
    }
}
