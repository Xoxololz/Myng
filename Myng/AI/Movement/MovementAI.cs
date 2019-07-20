using Microsoft.Xna.Framework;
using Myng.Graphics.Enemies;
using Myng.Helpers;
using Myng.Helpers.Enums;
using Myng.States;
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

        private int currentNodeIndex;

        private Node nextClearNode;

        private Enemy parent;

        private int tolerance = 30;

        private int sightRange;
        #endregion

        #region Properties


        public bool Stopped { get; set; }
        #endregion

        #region Constructors

        public MovementAI(SpritePolygon collisionPolygon, Enemy parent)
        {
            nodeMap = NodeMapRepository.GetNodeMap(collisionPolygon);
            pathFinder = new PathFinder();
            this.parent = parent;
            sightRange = 50;
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
            if(DistanceParentFrom(path[0]) < sightRange)
            {
                if (path[0] == nextClearNode && DistanceParentFrom(path[0]) > tolerance) return nextClearNode;
                if (CanGoStraightTo(path[0]))
                {
                    node = path[0];
                    //path.RemoveRange(1, path.Count - 1);
                    if (DistanceParentFrom(path[0]) < tolerance)
                    {
                        nextClearNode = null;
                        path = null;
                    }

                    return node;
                }
            }
            for (int i = currentNodeIndex - 1; i >= 0; i--)
            {
                var a = DistanceParentFrom(path[i]);
                if (a > sightRange || i == 0)
                {
                    if (i + 2 > path.Count) return path[path.Count - 1];
                    if (path[i + 1] == nextClearNode) return nextClearNode;
                    if (CanGoStraightTo(path[i + 1]))
                    { 
                        node = path[i + 1];
                        //path.RemoveRange(i + 2, path.Count - (i + 2));
                        currentNodeIndex = i + 1;
                        return node;
                    }
                    else
                    {
                        for (int j = i + 2; j < path.Count; j++)
                        {
                            if (j == path.Count - 1)
                            {
                                return path[currentNodeIndex - 1];
                            }
                            if (path[j] == nextClearNode) return nextClearNode;
                            if (CanGoStraightTo(path[j]))
                            {
                                //path.RemoveRange(j + 1, path.Count - (j + 1));
                                currentNodeIndex = j;
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
            bool canGo = false;
            Vector2 direction = new Vector2()
            {
                X = node.X - (float)Math.Floor(parent.Position.X),
                Y = node.Y - (float)Math.Floor(parent.Position.Y)
            };
            direction.Normalize();
            direction *= 8;
            SpritePolygon collisionPolygon = (SpritePolygon)parent.CollisionPolygon.Clone(); ;
            while (GameState.TileMap.CheckCollisionWithTerrain(collisionPolygon) == Collision.None)
            {
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

        private void RecalculatePath()
        {
            if(path != null)
                SetGoalDestination(new Vector2(path[0].X, path[0].Y));
        }

        public void FindNewPath(List<Polygon> collidingPolygons)
        {
            nodeMap.AddTemporaryTerrain(collidingPolygons);
            RecalculatePath();
            nodeMap.RemoveTemporaryTerrain();
        }

        public bool SetGoalDestination(Vector2 destination)
        {
            nodeMap.Clear();
            Node goalNode = nodeMap.FindClosestFreeNode(destination);
            if (goalNode == null) return false;
            Node startNode = nodeMap.FindClosestFreeNode(parent.Position);
            if (startNode == null) return false;
            path = pathFinder.FindPath(startNode, goalNode, nodeMap);
            if(path!=null)
                currentNodeIndex = path.Count;
            return path != null;
        }                     

        #endregion
    }
}
