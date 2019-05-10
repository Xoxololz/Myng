using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myng.Helpers;
using Myng.Graphics.Animations;

namespace Myng.Graphics
{
    public class AnimationSprite : Sprite
    {
        #region Fields

        private float timer;

        #endregion

        #region Properties

        public float Lifespan = 2f;

        #endregion

        #region Constructors

        public AnimationSprite(Dictionary<string, Animation> animations, Vector2 position): base(animations, position)
        {
            layer = Layers.Projectile;
        }

        #endregion

        #region Methods

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            animationManager.Update(gameTime);

            if (timer >= Lifespan)
                ToRemove = true;
        }

        #endregion
    }
}
