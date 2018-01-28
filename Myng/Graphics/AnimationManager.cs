using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Myng.Graphics
{
    public class AnimationManager: ICloneable
    {
        #region Fields

        private Animation animation;

        private float timer;

        #endregion

        #region Properties

        public Vector2 Position { get; set; }

        #endregion

        #region Constructors

        public AnimationManager(Animation animation)
        {
            this.animation = animation;
        }

        #endregion

        #region Methods
        
        public Animation Animation
        {
            get
            {
                return animation;
            }
            set
            {
                animation = value;
            }
        }

        public void Play(Animation animation)
        {
            if (this.animation == animation)
                return;

            this.animation = animation;

            this.animation.CurrentFrame = new Vector2(0);

            timer = 0;
        }

        public void Stop()
        {
            animation.CurrentFrame = new Vector2(0);

            timer = 0;
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(timer >= animation.FrameSpeed)
            {
                timer = 0;
                if(Animation.IsLooping)
                    animation.SetColumn((int)animation.CurrentFrame.X+1);
                if (animation.CurrentFrame.X >= animation.FrameColumnCount)
                    animation.SetColumn(0);
            }
        }

        public void Draw(SpriteBatch spriteBatch, float scale, float layer)
        { 
            spriteBatch.Draw(texture: animation.Texture,
                             position: Position + animation.FrameOrigin,
                             sourceRectangle: new Rectangle((int)animation.CurrentFrame.X * animation.FrameWidth,
                                           (int)animation.CurrentFrame.Y * animation.FrameHeight,
                                           animation.FrameWidth,
                                           animation.FrameHeight),
                             color: Color.White,                             
                             rotation: 0f,
                             origin: animation.FrameOrigin,
                             scale: scale,
                             effects: SpriteEffects.None,
                             layerDepth: layer);
        }

        public void Draw(SpriteBatch spriteBatch, float scale, double angle, float layer)
        {
            spriteBatch.Draw(animation.Texture,
                             Position + animation.FrameOrigin*scale,
                             new Rectangle((int)animation.CurrentFrame.X * animation.FrameWidth,
                                           (int)animation.CurrentFrame.Y * animation.FrameHeight,
                                           animation.FrameWidth,
                                           animation.FrameHeight),
                             Color.White,
                             (float)angle,
                             animation.FrameOrigin,
                             scale,
                             SpriteEffects.None,
                             layer);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}