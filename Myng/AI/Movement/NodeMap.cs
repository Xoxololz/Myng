﻿using Microsoft.Xna.Framework;
using Myng.Helpers;
using Myng.Helpers.Enums;
using System.Collections.Generic;
using Myng.Graphics;
using System;
using Myng.Helpers.Map;

namespace Myng.AI.Movement
{
    public class NodeMap
    {
        #region Fields

        private List<Node> changedNodes = new List<Node>();

        private List<Node> temporalTerrain = new List<Node>();

        private SpritePolygon generationPolygon;

        #endregion

        #region Properties

        public Node[,] Nodes { get; private set; }
        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }
        public int XSize { get; private set; }
        public int YSize { get; private set; }

        #endregion

        #region Constructors

        private NodeMap(int xSize, int ySize, int gridHeight, int gridWidth)
        {
            GridHeight = gridHeight;
            GridWidth = gridWidth;
            XSize = xSize - 1;
            YSize = ySize - 1;

            Nodes = new Node[xSize,ySize];
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < ySize; j++)
                {
                    Nodes[i, j] = new Node(i * gridWidth, j * gridHeight, i, j);
                }

            }
        }        

        public static NodeMap CreateFromTileMap(TileMap tileMap, SpritePolygon collisionPolygon)
        {
            NodeMap nodeMap = new NodeMap(tileMap.MapWidthTiles, tileMap.MapHeightTiles, tileMap.TileHeight, tileMap.TileWidth);
            nodeMap.generationPolygon = (SpritePolygon)collisionPolygon.Clone();

            for (int i = 0; i < nodeMap.XSize; i++)
            {
                for (int j = 0; j < nodeMap.YSize; j++)
                {
                    Vector2 position = new Vector2
                    {
                        X = nodeMap.Nodes[i, j].X,
                        Y = nodeMap.Nodes[i, j].Y
                    };
                    collisionPolygon.MoveTo(position);
                    Collision collision = tileMap.CheckCollisionWithTerrain(collisionPolygon);
                    if (collision == Collision.Solid || collision == Collision.Water)
                    {
                        nodeMap.Nodes[i, j].IsFree = false;
                        nodeMap.Nodes[i, j].IsTerrain = true;
                    }
                }

            }

            return nodeMap;
        }

        #endregion

        #region Methods
        internal void Update(List<Sprite> sprites)
        {
            ClearSprites();
            foreach (var sprite in sprites)
            {
                WriteSprite(sprite);
            }
        }

        private void ClearSprites()
        {
            for (int i = 0; i < XSize; i++)
            {
                for (int j = 0; j < YSize; j++)
                {
                    if (!Nodes[i, j].IsTerrain) Nodes[i, j].IsFree = true;
                }
            }
        }

        private void WriteSprite(Sprite sprite)
        {
            
        }

        internal Node FindClosestFreeNode(Vector2 destination)
        {
            int i = GetClosestNodesI(destination);
            int j = GetClosestNodesJ(destination);

            if (IsInMap(i, j) && Nodes[i, j].IsFree) return Nodes[i, j];
            if (IsInMap(i + 1, j) && Nodes[i + 1, j].IsFree) return Nodes[i + 1, j];
            if (IsInMap(i, j + 1) && Nodes[i, j + 1].IsFree) return Nodes[i, j + 1];
            if (IsInMap(i + 1, j + 1) && Nodes[i + 1, j + 1].IsFree) return Nodes[i + 1, j + 1];
            if (IsInMap(i - 1, j) && Nodes[i - 1, j].IsFree) return Nodes[i - 1, j];
            if (IsInMap(i, j - 1) && Nodes[i, j - 1].IsFree) return Nodes[i, j - 1];
            if (IsInMap(i - 1, j - 1) && Nodes[i - 1, j - 1].IsFree) return Nodes[i - 1, j - 1];
            if (IsInMap(i + 1, j - 1) && Nodes[i + 1, j - 1].IsFree) return Nodes[i + 1, j - 1];
            if (IsInMap(i - 1, j + 1) && Nodes[i - 1, j + 1].IsFree) return Nodes[i - 1, j + 1];

            return null;
        }

        private int GetClosestNodesI(Vector2 destination)
        {
            int i = (int)Math.Floor(destination.X / GridWidth);
            int xModulo = (int)destination.X % GridWidth;
            if (xModulo > GridWidth / 2) i++;

            return i;
        }

        private int GetClosestNodesJ(Vector2 destination)
        {
            int j = (int)Math.Floor(destination.Y / GridHeight);
            int yModulo = (int)destination.Y % GridHeight;
            if (yModulo > GridHeight / 2) j++;

            return j;
        }

        public List<Node> GetNeighbours(int i, int j)
        {
            List<Node> neighbours = new List<Node>();
            if (IsInMap(i+1,j))
            {
                if (Nodes[i + 1, j].IsFree) neighbours.Add(Nodes[i + 1, j]);
            }
            if (IsInMap(i, j + 1))
            {
                if (Nodes[i, j + 1].IsFree) neighbours.Add(Nodes[i, j + 1]);
            }
            if (IsInMap(i - 1, j))
            {
                if (Nodes[i - 1, j].IsFree) neighbours.Add(Nodes[i - 1, j]);
            }
            if (IsInMap(i, j - 1))
            {
                if (Nodes[i, j - 1].IsFree) neighbours.Add(Nodes[i, j - 1]);
            }

            changedNodes.AddRange(neighbours);

            return neighbours;
        }

        private bool IsInMap(int i, int j)
        {
            bool xInBounds = i >= 0 && i < XSize;
            bool yInBounds = j >= 0 && j < YSize;
            return xInBounds && yInBounds;
        }

        public void Clear()
        {
            foreach(var node in changedNodes)
            {
                node.CameFrom = null;
                node.FScore = float.MaxValue;
                node.GScore = float.MaxValue;
            }
            changedNodes.Clear();
        }        

        public void AddTemporaryTerrain(List<Polygon> polygons)
        {
            var min = new Point(int.MaxValue);
            var max = new Point(0);
            foreach (var polygon in polygons)
            {
                foreach ( var point in polygon.Vertices)
                {
                    int i = (int)Math.Floor(point.X / GridHeight);
                    int j = (int)Math.Floor(point.Y / GridHeight);
                    if (i < min.X) min.X = i;
                    if (j < min.Y) min.Y = j;
                    if (i > max.X) max.X = i;
                    if (j > max.Y) max.Y = j;
                }
                if(min.X > 1 && min.Y >1)
                    min -= new Point(1);
                if(max.X < XSize && max.Y < YSize)
                    max += new Point(1);
                RegenerateRectangle(max, min, polygons);
            }
        }

        private void RegenerateRectangle(Point max, Point min, List<Polygon> polygons)
        {
            for (int i = min.X; i < max.X; i++)
            {
                for (int j = min.Y; j < max.Y; j++)
                {
                    Vector2 position = new Vector2
                    {
                        X = Nodes[i, j].X,
                        Y = Nodes[i, j].Y
                    };
                    generationPolygon.MoveTo(position);
                    var collision = false;
                    foreach (var polygon in polygons)
                    {
                        if (generationPolygon.Intersects(polygon)) collision = true;
                    }

                    if (collision)
                    {
                        Nodes[i, j].IsFree = false;
                        if (!Nodes[i, j].IsTerrain)
                            temporalTerrain.Add(Nodes[i, j]);
                    }
                }
            }
        }

        public void RemoveTemporaryTerrain()
        {
            foreach (var node in temporalTerrain)
            {
                node.IsFree = true;
            }
            temporalTerrain.Clear();
        }
        #endregion
    }
}
