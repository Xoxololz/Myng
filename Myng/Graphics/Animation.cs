using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myng.Graphics
{
    public class Animation
    {
        private Vector2 currentFrame;

        public int FrameRowCount { get; private set; }

        public int FrameColumnCount { get; private set; }

        public int FrameHeight { get { return Parent.Texture.Height / FrameRowCount; } }

        public float FrameSpeed { get; set; }

        public int FrameWidth { get { return Parent.Texture.Width / FrameColumnCount; } }

        public bool isLooping { get; set; }

        public Sprite Parent{ get; private set; }

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

        public Animation(Sprite parent, int frameRowCount, int frameColumnCount)
        {
            Parent = parent;
            FrameRowCount = frameRowCount;
            FrameColumnCount = frameColumnCount;
            isLooping = true;
            FrameSpeed = 0.2f;
            CurrentFrame = new Vector2(0);
        }

        public void setRow(int row)
        {
            currentFrame.Y = row;
        }

        public void setColumn(int column)
        {
            currentFrame.X = column;
        }
    }

}
