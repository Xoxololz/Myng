using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myng.Graphics
{
    public class AnimationManager
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

        public void Draw(SpriteBatch spriteBatch)
        { 
            spriteBatch.Draw(animation.Texture,
                             Position,
                             new Rectangle((int)animation.CurrentFrame.X * animation.FrameWidth,
                                           (int)animation.CurrentFrame.Y * animation.FrameHeight,
                                           animation.FrameWidth,
                                           animation.FrameHeight),
                             Color.White);
        }
        
        #endregion
    }
}