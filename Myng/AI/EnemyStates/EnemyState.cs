using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using Myng.Graphics.Enemies;
using System.Collections.Generic;

namespace Myng.AI.EnemyStates
{
    abstract public class EnemyState
    {
        #region Constructors

        public EnemyState(Enemy controlledEnemy)
        {
            this.controlledEnemy = controlledEnemy;
            //controlledEnemy.SpeedMultiplier = 1f;
        }

        #endregion
        #region Fields

        protected Enemy controlledEnemy;

        #endregion

        #region Methods

        public abstract void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites);

        #endregion
    }
}
