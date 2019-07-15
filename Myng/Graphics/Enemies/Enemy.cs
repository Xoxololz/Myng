using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myng.Helpers;
using Myng.AI.Movement;
using Myng.Graphics.Animations;
using Myng.Helpers.Enums;
using Myng.AI.EnemyStates;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Myng.States;
using Myng.Helpers.Spells;
using Myng.Depositories;

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

        public EnemyState NextState
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

        protected EnemyState currentState;

        protected EnemyState nextState;

        protected int autoAttackRange;

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
            SightRange = 600;
            autoAttackRange = 350;
            baseAttackSpeed = 2f;
            Debug.Assert(SightRange > autoAttackRange, "Cant shoot target, when u dont see it.");
        }

        #endregion

        #region Methods

        protected virtual void InitAutoattack()
        {
            //Action<List<Sprite>> autoAttackAction = (sprites) =>
            //{
            //    var b = Bullet.Clone() as Projectile;
            //    var bPosition = GlobalOrigin - Bullet.Origin * Bullet.Scale;

            //     var attackDirection= -(Position - (playerPosition));
            //    if (attackDirection.X < 0)
            //        b.AngleTextureOffset = MathHelper.ToRadians(180);

            //    b.Initialize(bPosition, 30, DamageType.PHYSICAL, attackDirection, Faction,
            //        SoundsDepository.FireballFlying.CreateInstance(), SoundsDepository.FireballExplosion.CreateInstance(), this);

            //    sprites.Add(b);
            //};
            //Func<bool> canExecute = () =>
            //{
            //    var AutoattackRange = (Position - playerPosition).Length() < SightRange;
            //    return AutoattackRange;                
            //};

            //autoAttack = new AutoAttack(autoAttackAction, canExecute, this);
            autoAttack = SpellDepository.RangeAutoAttack(this);
        }

        protected virtual void InitSpells()
        {
            Action<List<Sprite>> blast = (sprites) =>
            {

                var bulletMid = Bullet.Clone() as Projectile;
                var bPosition = GlobalOrigin - Bullet.Origin * Bullet.Scale;

                var attackDirection= -(Position - (playerPosition));
                if (attackDirection.X < 0)
                    bulletMid.AngleTextureOffset = MathHelper.ToRadians(180);

                bulletMid.Initialize(bPosition, 40, DamageType.MAGIC, attackDirection, Faction,
                    SoundsDepository.FireballFlying.CreateInstance(), SoundsDepository.FireballExplosion.CreateInstance(), this);
                var bulletBot = bulletMid.Clone() as Projectile;
                var bulletTop = bulletMid.Clone() as Projectile;

                var angleDiff = MathHelper.ToRadians(20);
                bulletBot.RotateDirection(-angleDiff);
                bulletTop.RotateDirection(angleDiff);

                sprites.Add(bulletMid);
                sprites.Add(bulletBot);
                sprites.Add(bulletTop);
            };

            var fireballAnimation = new Dictionary<string, Animation>()
            {
                { "fireball", new Animation(State.Content.Load<Texture2D>("Projectiles/greenArrow"), 1, 1)
                    {
                        FrameSpeed = 0.05f
                    }
                }
            };

            var spell = new Spell(blast, 0, 4)
            {
                Range = 250,
                CastingAnimations = fireballAnimation,
                CastingTime = 1.5
            };
            Spells.Add(spell);

        }

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            UpdateSpellCooldowns(gameTime);
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

        private void UpdateSpellCooldowns(GameTime gameTime)
        {
            foreach (var spell in Spells)
            {
                spell.UpdateCooldown(gameTime);
            }
            autoAttack.UpdateCooldown(gameTime);
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
