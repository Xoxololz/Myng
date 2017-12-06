using Myng.Graphics;
using System;
using System.Collections.Generic;

namespace Myng.Helpers
{
    public class Spell
    {
        #region Fields

        private Action<List<Sprite>> action;

        private Func<bool> canExecute;

        #endregion

        #region Contructors

        public Spell(Action<List<Sprite>> action, Func<bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public Spell(Action<List<Sprite>> action)
            : this(action, null) { }

        #endregion

        #region Methods

        public bool CanCast(object parameter)
        {
            if(canExecute==null)
                return true;
            return canExecute();
        }

        public void Cast(List<Sprite> parameter)
        {
            if (!CanCast(null))
                return;
         
            action(parameter);
        }

        #endregion
    }
}
