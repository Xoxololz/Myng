using Microsoft.Xna.Framework;
using Myng.States;
using System;
using System.Collections.Generic;

namespace Myng.Helpers
{
    public class SpritePolygon : Polygon
    {
        #region Properties

        /// <summary>
        /// points that needs to be checked for collision with terrain
        /// </summary>
        public Vector2[] CollisionPoints { get; private set; }

        #endregion

        #region Constructors
        public SpritePolygon(Rectangle rectangle) : base(rectangle)
        {
            CalculateCollisionPoints();
        }

        public SpritePolygon(Vector2[] points, Vector2 origin) : base(points, origin)
        {
            CalculateCollisionPoints();
        }

        public SpritePolygon(Rectangle rectangle, float angle) : base(rectangle, angle)
        {
            CalculateCollisionPoints();
        }

        public SpritePolygon(Rectangle rectangle, float angle, Vector2? origin) : base(rectangle, angle, origin)
        {
            CalculateCollisionPoints();
        }
        #endregion

        #region Methods

        private void CalculateCollisionPoints()
        {
            var points = new List<Vector2>();
            //TODO: check if is big enought to need more points if so init them somehow
            for (int i = 0; i < Vertices.Length; i++)
            {
                if (i == Vertices.Length - 1)
                {
                    points.AddRange(FindCollisionPointsOnLine(Vertices[i], Vertices[0]));
                }
                else
                {
                    points.AddRange(FindCollisionPointsOnLine(Vertices[i], Vertices[i + 1]));
                }
            }
            CollisionPoints = points.ToArray();
        }

        private List<Vector2> FindCollisionPointsOnLine(Vector2 a, Vector2 b)
        {
            var pointsToCheck = new List<Vector2>();

            //assuming that tile is a square
            var tileSide = GameState.TileMap.TileWidth;
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (dx > 0)
                {
                    pointsToCheck.Add(a);
                    var x = a.X;
                    while (Math.Abs(x - b.X) >= tileSide)
                    {
                        pointsToCheck.Add(new Vector2(x + tileSide, FindYCoord(x, a, b)));
                        x += tileSide;
                    }
                }
                else
                {
                    pointsToCheck.Add(a);
                    var x = a.X;
                    while (Math.Abs(x - b.X) >= tileSide)
                    {
                        pointsToCheck.Add(new Vector2(x - tileSide, FindYCoord(x, a, b)));
                        x -= tileSide;
                    }
                }
            }
            else
            {
                if (dy > 0)
                {
                    pointsToCheck.Add(a);
                    var y = a.Y;
                    while (Math.Abs(y - b.Y) >= tileSide)
                    {
                        pointsToCheck.Add(new Vector2(FindXCoord(y, a, b), y + tileSide));
                        y += tileSide;
                    }
                }
                else
                {
                    pointsToCheck.Add(a);
                    var y = a.Y;
                    while (Math.Abs(y - b.Y) >= tileSide)
                    {
                        pointsToCheck.Add(new Vector2(FindXCoord(y, a, b), y - tileSide));
                        y -= tileSide;
                    }
                }
            }
            return pointsToCheck;
        }

        private float FindYCoord(float x, Vector2 a, Vector2 b)
        {
            var ab = b - a;
            return a.Y + ab.Y * ((x - a.X) / ab.X);
        }

        private float FindXCoord(float y, Vector2 a, Vector2 b)
        {
            var ab = b - a;
            return a.X + ab.X * ((y - a.Y) / ab.Y);
        }

        public override void Scale(float scale)
        {
            base.Scale(scale);
            CalculateCollisionPoints();
        }

         public new object Clone()
        {
            var vertices = new Vector2[Vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Vertices[i];
            }
            var origin = new Vector2
            {
                X = Origin.X,
                Y = Origin.Y
            };

            return new SpritePolygon(vertices, origin);
        }

        public override void Translate(Vector2 vector)
        {
            for (int i = 0; i < CollisionPoints.Length; i++)
            {
                CollisionPoints[i] += vector;
            }
            base.Translate(vector);
        }

        #endregion
    }
}
