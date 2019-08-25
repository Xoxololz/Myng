using Microsoft.Xna.Framework;
using Myng.Graphics;
using Myng.Graphics.Enemies;
using System.Collections.Generic;

namespace Myng.AI.EnemyStates
{
    public class PassiveState : EnemyState
    {
        #region Fields


        #endregion

        #region Constructors

        public PassiveState(Enemy controlledEnemy) : base(controlledEnemy)
        {
            controlledEnemy.SpeedMultiplier = 0.7f;
        }

        #endregion

        #region Methods

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            if(Vector2.Distance(controlledEnemy.Position, Game1.Player.Position) < controlledEnemy.SightRange)
            {
                controlledEnemy.NextState = new ChaseState(controlledEnemy);
            }

            if (Vector2.Distance(controlledEnemy.Position, controlledEnemy.startingPosition) > controlledEnemy.CollisionPolygon.Radius 
                && controlledEnemy.Velocity == Vector2.Zero)
                controlledEnemy.SetGoalDestination(controlledEnemy.startingPosition);
        }

        #endregion
    }
}
