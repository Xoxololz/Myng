using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myng.Graphics;
using Myng.Graphics.Enemies;
using Myng.Helpers.Spells;

namespace Myng.AI.EnemyStates
{
    public class CastingState : EnemyState
    {
        #region Fields

        private Spell spell;

        private double castTimer;

        private bool castingSpriteAdded;

        #endregion

        #region Constructors

        public CastingState(Enemy controlledEnemy, Spell spell) : base(controlledEnemy)
        {
            this.spell = spell;
            castTimer = 0;
            controlledEnemy.SpeedMultiplier = 0.6f;
            castingSpriteAdded = false;
        }

        #endregion

        #region Methods

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            castTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (!castingSpriteAdded && spell.CastingAnimations != null)
            {
                castingSpriteAdded = true;
                var castingSprite = new CastingEffectSprite(spell.CastingAnimations, controlledEnemy.Origin + controlledEnemy.Origin,
                    controlledEnemy)
                {
                    Lifespan = (float)spell.CastingTime
                };
                otherSprites.Add(castingSprite);
            }

            if (spell.CastingTime < castTimer)
            {
                spell.Cast(otherSprites);
                controlledEnemy.NextState = new ChaseState(controlledEnemy);
            }
        }

        #endregion
    }
}
