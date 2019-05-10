using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using Myng.Graphics.Animations;
using System;
using System.Collections.Generic;

namespace Myng.Helpers.Spells
{
    public class Spell
    {
        #region Fields

        protected Action<List<Sprite>> action;

        protected Func<bool> canExecute;

        protected int manaCost;

        protected virtual double cooldown { get; set; }

        protected double cooldownTimer;

        protected Texture2D spellbarTexture;

        #endregion

        #region Properties

        public Dictionary<string, Animation> CastingAnimations;

        public double CastingTime = 1;

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
                return spellbarTexture;
            }
        }

        public int Range { get; set; }

        #endregion

        #region Contructors

        public Spell(Action<List<Sprite>> action, Func<bool> canExecute, int cost, Texture2D texture, double cooldown)
        {
            this.action = action;
            this.canExecute = canExecute;
            this.manaCost = cost;
            this.spellbarTexture = texture;
            this.cooldown = cooldown;
        }

        public Spell(Action<List<Sprite>> action, Func<bool> canExecute, int cost, double cooldown)
        {
            this.action = action;
            this.canExecute = canExecute;
            this.manaCost = cost;
            this.spellbarTexture = null;
            this.cooldown = cooldown;
        }

        public Spell(Action<List<Sprite>> action, int cost, Texture2D texture, double cooldown)
            : this(action, null, cost, texture, cooldown) { }

        public Spell(Action<List<Sprite>> action, int cost, double cooldown)
            : this(action, null, cost, null, cooldown) { }

        #endregion

        #region Methods

        public void UpdateCooldown(GameTime gameTime)
        {
            cooldownTimer += gameTime.ElapsedGameTime.TotalSeconds;
        }

        public bool CanCast()
        {
            if (cooldown > cooldownTimer)
                return false;

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

            cooldownTimer = 0;
            Game1.Player.Mana -= manaCost;
            action(parameter);
        }

        #endregion
    }
}
