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
        private Animation _animation;

        private float _timer;

        public AnimationManager(Animation animation)
        {
            _animation = animation;
        }

        public Animation Animation
        {
            get
            {
                return _animation;
            }
            set
            {
                _animation = value;
            }
        }

        public void Play(Animation animation)
        {
            if (_animation == animation)
                return;

            _animation = animation;

            _animation.CurrentFrame = new Vector2(0);

            _timer = 0;
        }

        public void Stop()
        {
            _animation.CurrentFrame = new Vector2(0);

            _timer = 0;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(_timer >= _animation.FrameSpeed)
            {
                _timer = 0;
                _animation.setColumn((int)_animation.CurrentFrame.X+1);
                if (_animation.CurrentFrame.X >= _animation.FrameColumnCount)
                    _animation.setColumn(0);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        { 
            spriteBatch.Draw(_animation.Parent.Texture,
                             _animation.Parent.Position,
                             new Rectangle((int)_animation.CurrentFrame.X * _animation.FrameWidth,
                                           (int)_animation.CurrentFrame.Y * _animation.FrameHeight,
                                           _animation.FrameWidth,
                                           _animation.FrameHeight),
                             Color.White);
        }
    }
}
