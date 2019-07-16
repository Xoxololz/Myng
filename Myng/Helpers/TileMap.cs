using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers.Enums;
using System;
using System.Collections.Generic;
using TiledSharp;

namespace Myng.Helpers
{
    public class TileMap
    {

        #region Fields

        private TmxMap map;

        List<Tileset> tilesets;

        private Dictionary<int, Tuple<Polygon, Collision>> CollisionPolygons = new Dictionary<int, Tuple<Polygon,Collision>>();

        private int leftColumn;
        private int upperRow;

        private int screenWidthTiles;
        private int screenHeightTiles;
        #endregion

        #region Variables

        public int MapWidth {
            get
            {
                return map.Width * map.Tilesets[0].TileWidth;
            }
        }
        public int MapHeight
        {
            get
            {
                return map.Height * map.Tilesets[0].TileHeight;
            }
        }        

        public int MapWidthTiles { get; private set; }

        public int MapHeightTiles { get; private set; }

        public int TileHeight
        {
            get
            {
                return tilesets[0].TileHeight;
            }
        }

        public int TileWidth
        {
            get
            {
                return tilesets[0].TileWidth;
            }
        }
        #endregion

        #region Constructor

        public TileMap(TmxMap map)
        {
            this.map = map;
            tilesets = new List<Tileset>();
            foreach (var tileset in map.Tilesets)
            {
                tilesets.Add(new Tileset(tileset));
            }

            screenWidthTiles = (int)Math.Floor((float)Game1.ScreenWidth / map.Tilesets[0].TileWidth) + 10;
            screenHeightTiles = (int)Math.Floor((float)Game1.ScreenHeight / map.Tilesets[0].TileWidth) + 10;

            InitCollisionPolygons();

            MapHeightTiles = map.Height;
            MapWidthTiles = map.Width;
        }

        #endregion

        #region Methods


        private void InitCollisionPolygons()
        {
            Tileset currentTileset;            

            for (var j = 0; j < map.Layers.Count; j++)
            {
                for (var i = 0; i < map.Layers[j].Tiles.Count; i++)
                {
                    int gid = map.Layers[j].Tiles[i].Gid;
                    currentTileset = tilesets[GetTilesetIndex(gid)];
                    int tileFrame = gid - currentTileset.FirstGid;

                    float x = (i % map.Width) * map.TileWidth;
                    float y = (float)Math.Floor(i / (double)map.Width) * map.TileHeight;


                    TmxTilesetTile collisionTile;
                    if (currentTileset.Tiles.TryGetValue(tileFrame, out collisionTile)
                        && collisionTile.ObjectGroups.Count > 0 && collisionTile.ObjectGroups[0].Objects.Count > 0)
                    {
                        Collision collisionType = GetCollisionType(collisionTile);
                        switch (collisionTile.ObjectGroups[0].Objects[0].ObjectType)
                        {
                            case TmxObjectType.Polygon:
                                if (!CollisionPolygons.ContainsKey(gid))
                                {
                                    AddPolygonToCollisionPolygons(collisionTile.ObjectGroups[0].Objects[0], gid, collisionType);
                                }                                
                                break;
                            case TmxObjectType.Basic:
                                if (!CollisionPolygons.ContainsKey(gid))
                                {
                                    AddRectangleToCollisionPolygons(collisionTile.ObjectGroups[0].Objects[0], gid, collisionType);
                                }                                
                                break;
                        }
                    }
                }
            }
        }

        private Collision GetCollisionType(TmxTilesetTile collisionTile)
        {
            string collisionType;
            if (!collisionTile.Properties.TryGetValue("Collision", out collisionType)) return Collision.Solid;
            if (collisionType == "Water") return Collision.Water;
            else return Collision.Solid;
        }

        private void AddPolygonToCollisionPolygons(TmxObject tmxObject, int gid, Collision collision)
        {
            var points = new Vector2[tmxObject.Points.Count];
            for (int i = 0; i < tmxObject.Points.Count; i++)
            {
                points[i].X = (float)(tmxObject.Points[i].X + tmxObject.X);
                points[i].Y = (float)(tmxObject.Points[i].Y + tmxObject.Y);
            }
            Tuple<Polygon, Collision> terrainCollision = new Tuple<Polygon, Collision>(new Polygon(points, new Vector2((float)tmxObject.X, (float)tmxObject.Y)), collision);
            CollisionPolygons.Add(gid, terrainCollision);
        }

        private void AddRectangleToCollisionPolygons(TmxObject tmxObject, int gid, Collision collision)
        {
            var rectangle = new Rectangle((int)(tmxObject.X), (int)(tmxObject.Y),
                (int)tmxObject.Width, (int)tmxObject.Height);
            Tuple<Polygon, Collision> terrainCollision = new Tuple<Polygon, Collision>(new Polygon(rectangle), collision);
            CollisionPolygons.Add(gid, terrainCollision);
        }

        public Collision CheckCollisionWithTerrain(SpritePolygon spritePolygon)
        {
            var collision = Collision.None;
            
            try
            {
                foreach (var layer in map.Layers)
                {
                    foreach (var point in spritePolygon.CollisionPoints)
                    {
                        var currentTile = GetCurrentTilesGid(point, layer);
                        if (CollisionPolygons.TryGetValue(currentTile, out Tuple<Polygon, Collision> terrainPolygon))
                        {
                            var tileOrigin = GetCurrentTileOrigin(point);
                            var polygonTileCoord = TransformPolygonToTileCoord(spritePolygon, tileOrigin);
                            if (polygonTileCoord.Intersects(terrainPolygon.Item1))
                            {
                                collision = terrainPolygon.Item2;
                            }
                        }
                    }
                }                
            }
            catch (ArgumentOutOfRangeException) //being out of map counts as collision
            {
                return Collision.Solid;
            }
            return collision;
        }                

        private Vector2 GetCurrentTileOrigin(Vector2 point)
        {
            Vector2 origin;
            origin.X = point.X - point.X % map.Tilesets[0].TileWidth;
            origin.Y = point.Y - point.Y % map.Tilesets[0].TileHeight;
            return origin;
        }

        private Polygon TransformPolygonToTileCoord(Polygon polygon, Vector2 tileOrigin)
        {
            Vector2[] points = new Vector2[polygon.Vertices.Length];
            var origin = polygon.Origin;
            for (int i = 0; i < polygon.Vertices.Length; i++)            
            {
                points[i] =polygon.Vertices[i] - tileOrigin;
            }
            return new Polygon(points, origin);
        }

        private int GetTilesetIndex(int gid)
        {
            for (int i = 0; i < tilesets.Count; i++)
            {
                if (i + 1 > tilesets.Count - 1) return i;

                if (tilesets[i + 1].FirstGid >= gid) return i;
            }
            throw new Exception("invalid gid passed");
        }

        private int GetCurrentTilesGid(Vector2 position, TmxLayer layer)
        {
            var row = (int)Math.Floor(position.Y / map.Tilesets[0].TileHeight) + 1;
            var column = (int)Math.Floor(position.X / map.Tilesets[0].TileWidth);
            if (column >= map.Width || column < 0) throw new ArgumentOutOfRangeException();
            var i = (row - 1) * map.Width + column;
            return layer.Tiles[i].Gid;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            leftColumn = (int)Math.Floor(-Camera.ScreenOffset.X / map.Tilesets[0].TileWidth);
            upperRow = (int)Math.Floor(-Camera.ScreenOffset.Y / map.Tilesets[0].TileHeight);
            Tileset currentTileset;

            for (var j = 0; j < map.Layers.Count; j++)
            {
                for (var i = 0; i < map.Layers[j].Tiles.Count; i++)
                {
                    int gid = map.Layers[j].Tiles[i].Gid;                                       
                    if (gid != 0 && TileIsOnScreen(i))                   
                    {
                        currentTileset = tilesets[GetTilesetIndex(gid)];
                        int tileFrame = gid - currentTileset.FirstGid;
                        int column = tileFrame % currentTileset.TilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / currentTileset.TilesetTilesWide);

                        float x = (i % map.Width) * map.TileWidth;
                        float y = (float)Math.Floor(i / (double)map.Width) * map.TileHeight;

                        Rectangle tilesetRec = new Rectangle(currentTileset.TileWidth * column, currentTileset.TileHeight * row, currentTileset.TileWidth, currentTileset.TileHeight);
                        float layer;
                        switch (j)
                        {
                            case 0:
                                layer = Layers.Background;
                                break;
                            case 1:
                                layer = Layers.Vegetation;
                                break;
                            case 2:
                                layer = Layers.Road;
                                break;
                            case 3:
                                layer = Layers.Accesories;
                                break;
                            case 4:
                                layer = Layers.Gates;
                                break;
                            default:
                                throw new Exception("Too many layers in tile map");
                        }

                        spriteBatch.Draw(texture: currentTileset.Texture, destinationRectangle: new Rectangle((int)x, (int)y,
                            currentTileset.TileWidth, currentTileset.TileHeight), sourceRectangle: tilesetRec, color: Color.White,
                            layerDepth: layer);
                    }
                }
            }
        }

        private bool TileIsOnScreen(int i)
        {
            var column = i % map.Width;
            var row = Math.Floor((float)i / map.Width) + 1;                        
            return (row >= upperRow
                && row <= upperRow + screenHeightTiles
                && column >= leftColumn
                && column <= leftColumn + screenWidthTiles);
        }

        #endregion

    }
}
