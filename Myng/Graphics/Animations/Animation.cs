using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Myng.Graphics
{
    public class Animation
    {
        #region Fields

        private Vector2 currentFrame;

        private Vector2 frameOrigin;

        #endregion

        #region Properties

        public int FrameRowCount { get; private set; }

        public int FrameColumnCount { get; private set; }

        public int FrameHeight { get { return Texture.Height / FrameRowCount; } }

        public float FrameSpeed { get; set; }

        public int FrameWidth { get { return Texture.Width / FrameColumnCount; } }

        public bool IsLooping { get; set; }

        public Texture2D Texture{ get; private set; }

        public Vector2 CurrentFrame
        {
            get
            {
                return currentFrame;
            }
            set
            {
                currentFrame = value;
            }
        }

        public Vector2 FrameOrigin
        {
            get
            {
                return frameOrigin;
            }
        }

        #endregion

        #region Constructors

        public Animation(Texture2D texture, int frameRowCount, int frameColumnCount)
        {
            Texture = texture;
            FrameRowCount = frameRowCount;
            FrameColumnCount = frameColumnCount;
            IsLooping = true;
            FrameSpeed = 0.2f;
            CurrentFrame = new Vector2(0);
            frameOrigin = new Vector2(Texture.Width / FrameColumnCount / 2, Texture.Height / FrameRowCount / 2);
        }

        #endregion

        #region Methods

        public void SetRow(int row)
        {
            currentFrame.Y = row;
        }

        public void SetColumn(int column)
        {
            currentFrame.X = column;
        }

        #endregion
    }

}
