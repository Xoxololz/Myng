using Microsoft.Xna.Framework;
using Myng.Graphics;
using Myng.States;

namespace Myng.Helpers
{
    public class Camera
    {
        #region Variables

        public Matrix Transform { get; private set; }

        public Sprite Target { get; set; }

        #endregion

        #region Constructors

        public Camera(Sprite Target)
        {
            this.Target = Target;
        }

        #endregion

        #region Methods

        public void Focus()
        {
            Matrix x;
            Matrix y;
            if(Target.CollisionPolygon.Origin.X - GameState.ScreenWidth / 2 <= 0)
            {
                x = Matrix.CreateTranslation(-GameState.ScreenWidth/2,0,0);
            }
            else if (Target.CollisionPolygon.Origin.X + GameState.ScreenWidth / 2 >= GameState.MapWidth)
            {
                x = Matrix.CreateTranslation(-(GameState.MapWidth - GameState.ScreenWidth/2),0,0);
            }
            else
            {
                x = Matrix.CreateTranslation(-Target.CollisionPolygon.Origin.X, 0, 0);
            }
            if ( Target.CollisionPolygon.Origin.Y - GameState.ScreenHeight / 2 <= 0)
            {
                y = Matrix.CreateTranslation(0,-GameState.ScreenHeight/2,0);
            }
            else if(Target.CollisionPolygon.Origin.Y + GameState.ScreenHeight / 2 >= GameState.MapHeight)
            {
                y = Matrix.CreateTranslation(0, -(GameState.MapHeight - GameState.ScreenHeight/2),0);
            }
            else
            {
                y = Matrix.CreateTranslation(0, -Target.CollisionPolygon.Origin.Y, 0);
            }
            Transform = x * y * Matrix.CreateTranslation(GameState.ScreenWidth/2 , GameState.ScreenHeight/2 , 0);
        } 

        #endregion

    }
}
