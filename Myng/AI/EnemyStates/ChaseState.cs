using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myng.Graphics;
using Myng.Graphics.Enemies;

namespace Myng.AI.EnemyStates
{
    public class ChaseState : State
    {
        #region Constructors

        public ChaseState(Enemy controlledEnemy) : base(controlledEnemy)
        {
            controlledEnemy.SpeedMultiplier = 1f;
            recalculationTimer = 0f;
            updateTime = 1.5f;
        }

        #endregion

        #region Fields

        private float recalculationTimer;

        private float updateTime;

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
        }

        #endregion
    }
}
