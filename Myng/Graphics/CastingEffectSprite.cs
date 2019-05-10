using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myng.Graphics.Animations;
using Myng.Helpers;

namespace Myng.Graphics
{
    public class CastingEffectSprite : AnimationSprite
    {
        #region Fields

        private Character castingCharacter;

        #endregion

        #region Constructor

        public CastingEffectSprite(Dictionary<string, Animation> animations, Vector2 position, Character character) : base(animations, position)
        {
            castingCharacter = character;
            layer = Layers.AlwaysOnTop;
        }

        #endregion

        #region Methods

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            if (castingCharacter.ToRemove)
            {
                ToRemove = true;
                return;
            }
            Position = castingCharacter.Position + castingCharacter.Origin;
            base.Update(gameTime, otherSprites, hittableSprites);
        }

        #endregion

    }
}
