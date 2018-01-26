﻿using Microsoft.Xna.Framework;
using System;

namespace Myng.Helpers
{
    /// <summary>
    /// Polygon, have to be oriented clockwise!!
    /// </summary>
    public class Polygon
    {

        #region Properties
        /// <summary>
        /// array of polygon points
        /// </summary>
        public Vector2[] Points { get; private set; }
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
            this.Points = points;
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
            if (origin.HasValue)
            {
                this.Origin = origin.Value;
                
            }
            else
            {
                Vector2 tmp = new Vector2
                {
                    X = rectangle.X + rectangle.Width/2,
                    Y = rectangle.Y + rectangle.Height / 2
                };
                this.Origin = tmp;
            }
          
            Points = new Vector2[4];
            Points[0].X = rectangle.X;
            Points[0].Y = rectangle.Y;
            Points[1].X = rectangle.X + rectangle.Width;
            Points[1].Y = rectangle.Y;
            Points[2].X = rectangle.X + rectangle.Width;
            Points[2].Y = rectangle.Y + rectangle.Height;
            Points[3].X = rectangle.X;
            Points[3].Y = rectangle.Y + rectangle.Height;

            this.Rotate(angle);
            InitRadius();
        }

        #endregion

        #region Methods


        private void InitRadius()
        {
            Radius = 0;
            foreach (var point in Points)
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
        public void Translate(Vector2 vector)
        {
            for (int i=0; i < Points.Length; i++)
            {
                Points[i] += vector;
            }
        }
        /// <summary>
        /// rotate the polygon around origin
        /// </summary>
        /// <param name="angle">angle to rotate by</param>
        public void Rotate(float angle)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i] -= Origin;
                Points[i].X = Points[i].X * (float)Math.Cos(MathHelper.ToRadians(angle)) - Points[i].Y * (float)Math.Sin(MathHelper.ToRadians(angle));
                Points[i].Y = Points[i].X * (float)Math.Sin(MathHelper.ToRadians(angle)) + Points[i].Y * (float)Math.Cos(MathHelper.ToRadians(angle));
                Points[i] += Origin;
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

            //if ((a.Origin - Origin).Length() > a.Radius + Radius) return false;
            
            foreach (var polygon in new[] { a, b })
            {
                for (int i1 = 0; i1 < polygon.Points.Length; i1++)
                {
                    int i2 = (i1 + 1) % polygon.Points.Length;
                    var p1 = polygon.Points[i1];
                    var p2 = polygon.Points[i2];

                    var normal = new Vector2(p2.Y - p1.Y, p1.X - p2.X);

                    double? minA = null, maxA = null;
                    foreach (var p in a.Points)
                    {
                        var projected = normal.X * p.X + normal.Y * p.Y;
                        if (minA == null || projected < minA)
                            minA = projected;
                        if (maxA == null || projected > maxA)
                            maxA = projected;
                    }

                    double? minB = null, maxB = null;
                    foreach (var p in b.Points)
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

        #endregion

    }
}
