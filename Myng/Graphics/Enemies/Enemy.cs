using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myng.Helpers;
using Myng.AI.Movement;
using Myng.Graphics.Animations;
using Myng.Helpers.Enums;
using Myng.AI.EnemyStates;
using System.Diagnostics;
using Myng.Helpers.Spells;

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

        public override float Speed
        {
            get
            {
                return baseSpeed * SpeedMultiplier * ImpairmentSpeedMultiplier;
            }
        }

        public EnemyState NextState
        {
            set
            {
                nextState = value;
            }
        }

        public bool IsAutoAttacking { get; set; }

        public Vector2 startingPosition;

        #endregion

        #region Fields

        protected Vector2 playerPosition;

        protected MovementAI movementAI;

        protected EnemyState currentState;

        protected EnemyState nextState;

        protected int autoAttackRange;

        #endregion

        #region Constructors
        public Enemy(Dictionary<string, Animation> animations, Vector2 position, EnemyType type) : base(animations, position)
        {
            startingPosition = position;
            Spells = new List<Spell>();
            Scale = 1f;
            baseSpeed = 1.5f;
            Faction = Faction.ENEMY;
            XPDrop = 20;
            movementAI = new MovementAI(CollisionPolygon, this);
            EnemyType = type;
            currentState = new PassiveState(this);
            SightRange = 500;
            autoAttackRange = 150;
            baseAttackSpeed = 2f;
            IsAutoAttacking = false;
            Debug.Assert(SightRange > autoAttackRange, "Cant shoot target, when u dont see it.");
        }

        #endregion

        #region Methods

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            base.Update(gameTime, otherSprites, hittableSprites);
            UpdateSpells(gameTime, otherSprites, hittableSprites);
            UpdateTimer(gameTime);

            if (Stunned)
            {
                if (!(currentState is ChaseState))
                    nextState = new ChaseState(this);
                return;
            }
            HandleStateChange();
            currentState.Update(gameTime, otherSprites, hittableSprites);
            playerPosition = Game1.Player.Position;
            DetermineVelocity(hittableSprites);
            HandleAnimation();
            animationManager.Update(gameTime);

            if (Silenced)
            {
                if (!(currentState is ChaseState))
                    nextState = new ChaseState(this);
                return;
            }
            CastAutoattack(otherSprites, hittableSprites);
        }

        private void UpdateSpells(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            foreach (var spell in Spells)
            {
                spell.Update(gameTime, otherSprites, hittableSprites);
            }
            AutoAttack?.Update(gameTime, otherSprites, hittableSprites);
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

        public void CastAutoattack(List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            if(IsAutoAttacking)
                AutoAttack?.Cast(otherSprites, hittableSprites);
        }

        public bool SetGoalDestination(Vector2 dest)
        {
            return movementAI.SetGoalDestination(dest);
        }

        public bool CanSeePlayer()
        {
            return movementAI.CanSee(Game1.Player.Position);
        }

        #endregion
    }
}
