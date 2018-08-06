using Microsoft.Xna.Framework;
using Myng.Graphics;
using Myng.States;

namespace Myng.Helpers
{
    public class Camera
    {
        #region Properties

        public static Matrix Transform { get; private set; }
        /// <summary>
        /// target to focus on
        /// </summary>
        public Sprite Target { get; set; }

        public static Vector2 ScreenOffset
        {
            get
            {
                return new Vector2()
                {
                    X = Transform.M41,
                    Y = Transform.M42
                };
            }
        }

        #endregion

        #region Constructors

        public Camera(Sprite Target)
        {
            this.Target = Target;
        }

        #endregion

        #region Methods
        /// <summary>
        /// focus camera on target
        /// </summary>
        public void Focus()
        {
            Matrix x;
            Matrix y;
            if(Target.CollisionPolygon.Origin.X - Game1.ScreenWidth / 2 <= 0)
            {
                x = Matrix.CreateTranslation(-Game1.ScreenWidth/2,0,0);
            }
            else if (Target.CollisionPolygon.Origin.X + Game1.ScreenWidth / 2 >= GameState.TileMap.MapWidth)
            {
                x = Matrix.CreateTranslation(-(GameState.TileMap.MapWidth - Game1.ScreenWidth/2),0,0);
            }
            else
            {
                x = Matrix.CreateTranslation(-Target.CollisionPolygon.Origin.X, 0, 0);
            }
            if ( Target.CollisionPolygon.Origin.Y - Game1.ScreenHeight / 2 <= 0)
            {
                y = Matrix.CreateTranslation(0,-Game1.ScreenHeight/2,0);
            }
            else if(Target.CollisionPolygon.Origin.Y + Game1.ScreenHeight / 2 >= GameState.TileMap.MapHeight)
            {
                y = Matrix.CreateTranslation(0, -(GameState.TileMap.MapHeight - Game1.ScreenHeight/2),0);
            }
            else
            {
                y = Matrix.CreateTranslation(0, -Target.CollisionPolygon.Origin.Y, 0);
            }
            Transform = x * y * Matrix.CreateTranslation(Game1.ScreenWidth/2 , Game1.ScreenHeight/2 , 0);
        }         

        #endregion

    }
}
