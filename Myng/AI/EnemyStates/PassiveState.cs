using Microsoft.Xna.Framework;
using Myng.Graphics;
using Myng.Graphics.Enemies;
using System.Collections.Generic;

namespace Myng.AI.EnemyStates
{
    public class PassiveState : EnemyState
    {
        #region Fields

        private int lastEnemyHealth;

        #endregion

        #region Constructors

        public PassiveState(Enemy controlledEnemy) : base(controlledEnemy)
        {
            controlledEnemy.Health = controlledEnemy.MaxHealth;
            controlledEnemy.SpeedMultiplier = 0.7f;
            controlledEnemy.IsAutoAttacking = false;
            lastEnemyHealth = controlledEnemy.Health;
        }

        #endregion

        #region Methods

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            if(Vector2.Distance(controlledEnemy.Position, Game1.Player.Position) < controlledEnemy.SightRange
                && controlledEnemy.CanSeePlayer())
            {
                controlledEnemy.NextState = new ChaseState(controlledEnemy);
            }
            if (lastEnemyHealth != controlledEnemy.Health)
                controlledEnemy.NextState = new ChaseState(controlledEnemy);
            lastEnemyHealth = controlledEnemy.Health;

            if (Vector2.Distance(controlledEnemy.Position, controlledEnemy.startingPosition) >
                controlledEnemy.CollisionPolygon.Radius)
            {
                controlledEnemy.SetGoalDestination(controlledEnemy.startingPosition);
            }
            
        }

        #endregion
    }
}
