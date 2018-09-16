using Microsoft.Xna.Framework;
using Myng.States;
using System;
using System.Collections.Generic;

namespace Myng.Helpers
{
    /// <summary>
    /// Polygon, have to be oriented clockwise!!
    /// </summary>
    public class Polygon: ICloneable
    {
        #region Properties
        /// <summary>
        /// array of polygon vertices
        /// </summary>
        public Vector2[] Vertices { get; private set; }        

        /// <summary>
        /// point to rotate around
        /// </summary>
        public Vector2 Origin { get; private set; }

        public float Radius { get; private set; }
        #endregion

        #region Constructors        
        /// <summary>
        /// create general CONVEX polygon
        /// </summary>
        /// <param name="points">array of points, have to be oriented clockwise and </param>
        /// <param name="origin">point to rotate around</param>
        public Polygon(Vector2[] points, Vector2 origin)
        {
            this.Vertices = points;
            this.Origin = origin;
            InitRadius();
        }

        /// <summary>
        /// create polygon to represent rectangle rotating around center with angle=0
        /// </summary>
        /// <param name="rectangle">rectangle to represent</param>
        public Polygon(Rectangle rectangle)
            : this(rectangle, 0f) { }
        /// <summary>
        /// create polygon to represent rectangle rotated by angle around center
        /// </summary>
        /// <param name="rectangle">rectangle to represent</param>
        /// <param name="angle">angle to rotate by</param>
        public Polygon(Rectangle rectangle, float angle)
            :this(rectangle,angle,null)
        {
        }
        /// <summary>
        /// create polygon to represent rectangle rotated by angle around origin
        /// </summary>
        /// <param name="rectangle">rectangle to represent</param>
        /// <param name="angle">angle to rotate by</param>
        /// <param name="origin">point ot rotate around</param>
        public Polygon(Rectangle rectangle, float angle, Vector2? origin)
        {
            var c = origin * origin;
            if (origin.HasValue)
            {
                this.Origin = origin.Value;
                
            }
            else
            {
                Origin = new Vector2
                {
                    X = rectangle.X + rectangle.Width/2,
                    Y = rectangle.Y + rectangle.Height / 2
                };
            }
          
            Vertices = new Vector2[4];
            Vertices[0].X = rectangle.X;
            Vertices[0].Y = rectangle.Y;
            Vertices[1].X = rectangle.X + rectangle.Width;
            Vertices[1].Y = rectangle.Y;
            Vertices[2].X = rectangle.X + rectangle.Width;
            Vertices[2].Y = rectangle.Y + rectangle.Height;
            Vertices[3].X = rectangle.X;
            Vertices[3].Y = rectangle.Y + rectangle.Height;

            this.Rotate(angle);
            InitRadius();
        }

        #endregion

        #region Methods
        
        private void InitRadius()
        {
            Radius = 0;
            foreach (var point in Vertices)
            {
                var distance = (point - Origin).Length();
                if (distance > Radius)
                {
                    Radius = distance;
                }
            }
        }
        /// <summary>
        /// translate the polygon
        /// </summary>
        /// <param name="vector">translation vector</param>
        public virtual void Translate(Vector2 vector)
        {
            for (int i=0; i < Vertices.Length; i++)
            {
                Vertices[i] += vector;
            }

            Origin += vector;
        }
        /// <summary>
        /// moves to a certain location (Point[0])
        /// </summary>
        /// <param name="vector">coordinates to move to</param>
        public void MoveTo(Vector2 vector)
        {
            Vector2 translation = vector - Vertices[0];
            Translate(translation);
        }
        /// <summary>
        /// scale polygon, points[0] will remain in place
        /// </summary>
        /// <param name="scale">scalar</param>
        public virtual void Scale(float scale)
        {
            for (int i = 1; i < Vertices.Length; i++)
            {
                var scalingVector = Vertices[i] - Vertices[0];
                scalingVector *= scale;
                Vertices[i] = Vertices[0] + scalingVector;
            }
            Radius *= scale;
            var scaleVector = Origin - Vertices[0];
            scaleVector *= scale;
            Origin = Vertices[0] + scaleVector;
        }

        /// <summary>
        /// rotate the polygon around origin
        /// </summary>
        /// <param name="angle">angle to rotate by</param>
        public void Rotate(float angle)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] -= Origin;
                Vertices[i].X = Vertices[i].X * (float)Math.Cos(MathHelper.ToRadians(angle)) - Vertices[i].Y * (float)Math.Sin(MathHelper.ToRadians(angle));
                Vertices[i].Y = Vertices[i].X * (float)Math.Sin(MathHelper.ToRadians(angle)) + Vertices[i].Y * (float)Math.Cos(MathHelper.ToRadians(angle));
                Vertices[i] += Origin;
            }
        }
        /// <summary>
        /// checks if this polygon collides with the other
        /// </summary>
        /// <param name="a">polygon to check collision with</param>
        /// <returns>boolean that represents if polygons are in collision</returns>
        public bool Intersects(Polygon a)
        {
            var b = this;

            if ((a.Origin - Origin).Length() > a.Radius + Radius) return false;
            
            foreach (var polygon in new[] { a, b })
            {
                for (int i1 = 0; i1 < polygon.Vertices.Length; i1++)
                {
                    int i2 = (i1 + 1) % polygon.Vertices.Length;
                    var p1 = polygon.Vertices[i1];
                    var p2 = polygon.Vertices[i2];

                    var normal = new Vector2(p2.Y - p1.Y, p1.X - p2.X);

                    double? minA = null, maxA = null;
                    foreach (var p in a.Vertices)
                    {
                        var projected = normal.X * p.X + normal.Y * p.Y;
                        if (minA == null || projected < minA)
                            minA = projected;
                        if (maxA == null || projected > maxA)
                            maxA = projected;
                    }

                    double? minB = null, maxB = null;
                    foreach (var p in b.Vertices)
                    {
                        var projected = normal.X * p.X + normal.Y * p.Y;
                        if (minB == null || projected < minB)
                            minB = projected;
                        if (maxB == null || projected > maxB)
                            maxB = projected;
                    }

                    if (maxA < minB || maxB < minA)
                        return false;
                }
            }
            return true;
        }

        public object Clone()
        {
            var points = new Vector2[Vertices.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = Vertices[i];
            }
            var origin = new Vector2
            {
                X = Origin.X,
                Y = Origin.Y
            };

            return new Polygon(points, origin);
        }


        #endregion

    }

    public class PolygonComparer : IEqualityComparer<Polygon>
    {
        public bool Equals(Polygon x, Polygon y)
        {
            x.MoveTo(Vector2.Zero);
            y.MoveTo(Vector2.Zero);
            if (x.Vertices.Length != y.Vertices.Length) return false;
            var equals = true;
            for (int i = 0; i < x.Vertices.Length; i++)
            {
                if (x.Vertices[i] != y.Vertices[i])
                {
                    equals = false;
                    break;
                }
            }
            return equals;
        }

        public int GetHashCode(Polygon obj)
        {
            var polString = obj.Vertices.ToString();
            return polString.GetHashCode();
        }
    }
}


