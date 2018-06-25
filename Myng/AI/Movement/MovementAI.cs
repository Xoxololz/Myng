using Microsoft.Xna.Framework;
using Myng.Graphics.Enemies;
using Myng.Helpers;
using Myng.Helpers.Enums;
using System;
using System.Collections.Generic;

namespace Myng.AI.Movement
{
    public class MovementAI
    {
        #region Fields

        private NodeMap nodeMap;

        private PathFinder pathFinder;

        private List<Node> path;

        private Node nextClearNode;

        private Enemy parent;

        private TileMap tileMap;

        private int tolerance = 5;        
        #endregion

        #region Properties

        public int SightRange { get; set; }

        public bool Stopped { get; set; }
        #endregion

        #region Constructors

        public MovementAI(TileMap tileMap, Polygon collisionPolygon, Enemy parent)
        {
            this.tileMap = tileMap;
            nodeMap = NodeMapRepository.GetNodeMap(collisionPolygon, tileMap);
            pathFinder = new PathFinder();
            this.parent = parent;
            SightRange = 150;
        }

        #endregion

        #region Methods

        public Vector2 GetVelocity()
        {
            if (path == null || Stopped) return Vector2.Zero;
            nextClearNode = FindFarthestNodeInRange();
            Vector2 velocity = new Vector2()
            {
                X = nextClearNode.X - parent.Position.X,
                Y = nextClearNode.Y - parent.Position.Y
            };
            if(velocity.Length() > 1)
            {
                velocity.Normalize();
            }

            return velocity;
        }

        private Node FindFarthestNodeInRange()
        {
            Node node = null;
            if(DistanceParentFrom(path[0]) < SightRange)
            {
                if (path[0] == nextClearNode) return nextClearNode;
                if (CanGoStraightTo(path[0]))
                {
                    node = path[0];
                    path.RemoveRange(1, path.Count - 1);
                    if (DistanceParentFrom(path[0]) < tolerance)
                    {
                        path = null;
                    }

                    return node;
                }
            }
            for (int i = path.Count - 1; i >= 0; i--)
            {
                var a = DistanceParentFrom(path[i]);
                if (a > SightRange || i == 0)
                {
                    if (path[i + 1] == nextClearNode) return nextClearNode;
                    if (CanGoStraightTo(path[i + 1]))
                    {                        
                        node = path[i + 1];
                        path.RemoveRange(i + 2, path.Count - (i + 2));
                        return node;
                    }
                    else
                    {
                        for (int j = i + 2; j < path.Count; j++)
                        {
                            if (path[j] == nextClearNode) return nextClearNode;
                            if (CanGoStraightTo(path[j]))
                            {
                                path.RemoveRange(j + 1, path.Count - (j + 1));
                                return path[j];
                            }
                        }
                    }
                }
            }

            throw new Exception("Too low SightRange");
        }        

        private float DistanceParentFrom(Node node)
        {
            Vector2 distance = new Vector2()
            {
                X = node.X - parent.Position.X,
                Y = node.Y - parent.Position.Y
            };
            return distance.Length();
        }

        private bool CanGoStraightTo(Node node)
        {
            //var endPolygon = new Polygon(parent.CollisionPolygon.Points, parent.Origin);
            //endPolygon.MoveTo(new Vector2(node.X, node.Y));
            //Vector2[] points = new Vector2[6]
            //{
            //    parent.CollisionPolygon.Points[0],
            //    parent.CollisionPolygon.Points[1],
            //    endPolygon.Points[1],
            //    endPolygon.Points[2],
            //    endPolygon.Points[3],
            //    parent.CollisionPolygon.Points[3]
            //};
            //var translatePolygon = new Polygon(points, points[0]);
            //return tileMap.CheckCollisionWithTerrain(translatePolygon) != Collision.None;
            bool canGo = false;
            Vector2 direction = new Vector2()
            {
                X = node.X - (float)Math.Floor(parent.Position.X),
                Y = node.Y - (float)Math.Floor(parent.Position.Y)
            };
            direction.Normalize();
            direction *= 8;            
            Polygon collisionPolygon = (Polygon)parent.CollisionPolygon.Clone(); ;
            while (tileMap.CheckCollisionWithTerrain(collisionPolygon) == Collision.None)
            {
                var a = DistanceFrom(node, collisionPolygon.Vertices[0]);
                if (DistanceFrom(node, collisionPolygon.Vertices[0]) <= direction.Length())
                {
                    canGo = true;
                    break;
                }
                collisionPolygon.Translate(direction);
            }
            return canGo;
        }

        private float DistanceFrom(Node node, Vector2 vector2)
        {
            Vector2 distance = new Vector2()
            {
                X = node.X - vector2.X,
                Y = node.Y - vector2.Y
            };
            return distance.Length();
        }

        public void RecalculatePath()
        {
            SetGoalDestination(new Vector2(path[0].X, path[0].Y));
        }

        public void SetGoalDestination(Vector2 destination)
        {
            nodeMap.Clear();
            Node goalNode = nodeMap.FindClosestFreeNode(destination);
            if (goalNode == null) return;
            Node startNode = nodeMap.FindClosestFreeNode(parent.Position);
            path = pathFinder.FindPath(startNode, goalNode, nodeMap);
        }                     

        #endregion
    }
}
