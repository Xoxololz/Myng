using Microsoft.Xna.Framework;
using Myng.Graphics;
using Myng.Graphics.Enemies;
using System;
using System.Collections.Generic;

namespace Myng.AI.EnemyStates
{
    public class WanderState : EnemyState
    {
        #region Constructors

        public WanderState(Enemy controlledEnemy) : base(controlledEnemy)
        {
            minDistance = -800;
            maxDistance = 800;

            random = new Random();
            SetRandomGoalDestination();
            controlledEnemy.SpeedMultiplier = 0.7f;
        }

        #endregion

        #region Fields

        private int minDistance;

        private int maxDistance;

        private Vector2 goalDestination;

        private Random random;

        #endregion

        #region Methods

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            if(Vector2.Distance(controlledEnemy.Position, Game1.Player.Position) < controlledEnemy.SightRange)
            {
                controlledEnemy.NextState = new ChaseState(controlledEnemy);
            }
            if(controlledEnemy.Velocity == Vector2.Zero)
            {
                SetRandomGoalDestination();
            }
        }

        private void SetRandomGoalDestination()
        {
            var set = false;
            Vector2 dest = new Vector2();
            while (!set)
            {
                dest = controlledEnemy.Position - new Vector2(random.Next(minDistance, maxDistance), random.Next(minDistance, maxDistance));
                set = controlledEnemy.SetGoalDestination(dest);
            }
            goalDestination = dest;
        }

        #endregion
    }
}
