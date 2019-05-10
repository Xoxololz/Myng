using System;
using System.Collections.Generic;
using Myng.Graphics;

namespace Myng.Helpers.Spells
{
    public class AutoAttack : Spell
    {
        #region Fields

        private Character owner;

        protected override double cooldown
        {
            get
            {
                return owner.AttackSpeed;
            }
        }

        #endregion

        #region Constructors

        public AutoAttack(Action<List<Sprite>> action, Func<bool> canExecute, Character owner)
            : base(action, canExecute, 0, 1)
        {
            this.owner = owner;
        }

        #endregion
    }
}
