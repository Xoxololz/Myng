using Microsoft.Xna.Framework;

namespace Myng.Helpers.Map
{
    public class NonCollidableTexture
    {
        #region Fields

        private int minRow = int.MaxValue;

        private int maxRow = int.MaxValue;

        private int maxColumn = int.MaxValue;

        private int minColumn = int.MaxValue;

        private Rectangle rectangle;

        #endregion

        #region Constructors

        public NonCollidableTexture(Rectangle rectangle, int tileWidth, int tileHeight)
        {
            this.rectangle = rectangle;
            ConvertPolygonToRowColumn(rectangle, tileWidth, tileHeight);
        }

        #endregion

        #region Methods

        private void ConvertPolygonToRowColumn(Rectangle rectangle, int tileWidth, int tileHeight)
        {
            minColumn = rectangle.X / tileWidth;
            minRow = rectangle.Y / tileHeight;
            maxColumn = (rectangle.X + rectangle.Width) / tileWidth;
            maxRow = (rectangle.Y + rectangle.Height) / tileHeight;
        }

        public bool IsTileIn(int row, int column)
        {
            return (row <= maxRow && row >= minRow && column <= maxColumn && column >= minColumn);
        }

        public bool ShouldBeTransparent()
        {
            if (Game1.Player.CollisionPolygon.Intersects(new Polygon(rectangle))) return true;
            return false;
        }

        #endregion
    }
}
