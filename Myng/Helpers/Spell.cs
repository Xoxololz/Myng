using Microsoft.Xna.Framework.Graphics;
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

        private int manaCost;

        private Texture2D texture;

        #endregion

        #region Properties

        public int ManaCost
        {
            get
            {
                return manaCost;
            }
        }

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

        #endregion

        #region Contructors

        public Spell(Action<List<Sprite>> action, Func<bool> canExecute, int cost, Texture2D texture)
        {
            this.action = action;
            this.canExecute = canExecute;
            this.manaCost = cost;
            this.texture = texture;
        }

        public Spell(Action<List<Sprite>> action, Func<bool> canExecute, int cost)
        {
            this.action = action;
            this.canExecute = canExecute;
            this.manaCost = cost;
            this.texture = null;
        }

        public Spell(Action<List<Sprite>> action, int cost, Texture2D texture)
            : this(action, null, cost, texture) { }

        public Spell(Action<List<Sprite>> action, int cost)
            : this(action, null, cost, null) { }

        #endregion

        #region Methods

        public bool CanCast()
        {
            if (Game1.Player.Mana < manaCost)
                return false;
            if(canExecute==null)
                return true;
            return canExecute();
        }

        public void Cast(List<Sprite> parameter)
        {
            if (!CanCast())
                return;

            Game1.Player.Mana -= manaCost;
            action(parameter);
        }

        #endregion
    }
}
