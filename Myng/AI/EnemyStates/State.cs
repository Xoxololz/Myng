using Microsoft.Xna.Framework;
using Myng.Graphics;
using Myng.Graphics.Enemies;
using System.Collections.Generic;

namespace Myng.AI.EnemyStates
{
    abstract public class State
    {
        #region Constructors

        public State(Enemy controlledEnemy)
        {
            this.controlledEnemy = controlledEnemy;
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
