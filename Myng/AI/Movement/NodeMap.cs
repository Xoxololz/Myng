using Microsoft.Xna.Framework;
using Myng.Helpers;
using Myng.Helpers.Enums;
using System.Collections.Generic;
using Myng.Graphics;
using System;

namespace Myng.AI.Movement
{
    public class NodeMap
    {
        #region Fields
        
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
            this.GridHeight = gridHeight;
            this.GridWidth = gridWidth;
            this.XSize = xSize;
            this.YSize = ySize;

            Nodes = new Node[xSize,ySize];
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < ySize; j++)
                {
                    Nodes[i, j] = new Node(i * gridWidth, j * gridHeight, i, j);
                }

            }
        }        

        public static NodeMap CreateFromTileMap(TileMap tileMap, Polygon collisionPolygon)
        {
            NodeMap nodeMap = new NodeMap(tileMap.MapWidthTiles, tileMap.MapHeightTiles, tileMap.TileHeight, tileMap.TileWidth);

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

            Node closestNode = null;

            if (Nodes[i, j].IsFree) closestNode = Nodes[i, j];
            if (Nodes[i + 1, j].IsFree) closestNode = Nodes[i + 1, j];
            if (Nodes[i, j + 1].IsFree) closestNode = Nodes[i, j + 1];
            if (Nodes[i + 1, j + 1].IsFree) closestNode = Nodes[i + 1, j + 1];

            return closestNode;
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
            for (int i = 0; i < XSize; i++)
            {
                for (int j = 0; j < YSize; j++)
                {
                    Nodes[i, j].CameFrom = null;
                    Nodes[i, j].FScore = float.MaxValue;
                    Nodes[i, j].GScore = float.MaxValue;
                }
            }
        }        
        #endregion
    }
}
