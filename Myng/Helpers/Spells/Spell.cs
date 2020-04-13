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

        protected Action<List<Sprite>, List<Sprite>> spellCastAction;

        protected Action<List<Sprite>,List<Sprite>, GameTime> updateAction;

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

        public Spell(Action<List<Sprite>, List<Sprite>> spellCastAction, Func<bool> canExecute, int cost, Texture2D texture
            , double cooldown, Action<List<Sprite>,List<Sprite>, GameTime> updateAction)
        {
            this.spellCastAction = spellCastAction;
            this.canExecute = canExecute;
            this.manaCost = cost;
            this.spellbarTexture = texture;
            this.cooldown = cooldown;
            this.updateAction = updateAction;
        }

        public Spell(Action<List<Sprite>, List<Sprite>> spellCastAction, Func<bool> canExecute, int cost, double cooldown)
            :this(spellCastAction, canExecute, cost, null, cooldown, null) { }

        public Spell(Action<List<Sprite>, List<Sprite>> spellCastAction, Func<bool> canExecute, int cost, Texture2D texture, double cooldown)
            :this(spellCastAction, canExecute, cost, texture, cooldown, null) { }

        public Spell(Action<List<Sprite>, List<Sprite>> spellCastAction, int cost, Texture2D texture, double cooldown)
            : this(spellCastAction, null, cost, texture, cooldown) { }

        public Spell(Action<List<Sprite>, List<Sprite>> spellCastAction, int cost, double cooldown)
            : this(spellCastAction, null, cost, null, cooldown) { }

        #endregion

        #region Methods

        public void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            cooldownTimer += gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void CastingUpdate(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            updateAction?.Invoke(otherSprites, hittableSprites, gameTime);
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

        public void Cast(List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            if (!CanCast())
                return;

            cooldownTimer = 0;
            Game1.Player.Mana -= manaCost;
            spellCastAction(otherSprites, hittableSprites);
        }

        #endregion
    }
}
