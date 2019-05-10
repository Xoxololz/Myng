using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myng.Graphics;
using Myng.Graphics.Enemies;

namespace Myng.AI.EnemyStates
{
    public class ChaseState : EnemyState
    {

        #region Fields

        private float recalculationTimer;

        private float updateTime;

        #endregion

        #region Constructors

        public ChaseState(Enemy controlledEnemy) : base(controlledEnemy)
        {
            recalculationTimer = 0f;
            updateTime = 1.5f;
            controlledEnemy.SetGoalDestination(Game1.Player.Position);
        }

        #endregion

        #region Methods

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            if(Vector2.Distance(controlledEnemy.Position, Game1.Player.Position) > controlledEnemy.SightRange)
            {
                controlledEnemy.NextState = new WanderState(controlledEnemy);
            }

            recalculationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (recalculationTimer > updateTime)
            {
                recalculationTimer = 0;
                controlledEnemy.SetGoalDestination(Game1.Player.Position);
            }

            foreach (var spel in controlledEnemy.Spells)
            {
                if(DistanceFromPlayer() < spel.Range && spel.CanCast())
                {
                    controlledEnemy.NextState = new CastingState(controlledEnemy, spel);
                }
            }
        }

        private float DistanceFromPlayer()
        {
            return Vector2.Distance(controlledEnemy.Position, Game1.Player.Position);
        }

        #endregion
    }
}
