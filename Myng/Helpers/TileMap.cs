using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public List<Polygon> CollisionPolygons = new List<Polygon>();

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

            //tileWidth = map.Tilesets[0].TileWidth;
            //tileHeight = map.Tilesets[0].TileHeight;

            //tilesetTilesWide = tileset.Width / tileWidth;
            //tilesetTilesHigh = tileset.Height / tileHeight;

            InitCollisionPolygons();
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
                        && collisionTile.ObjectGroups.Count>0)
                    {
                        switch (collisionTile.ObjectGroups[0].Objects[0].ObjectType)
                        {
                            case TmxObjectType.Polygon:
                                AddPolygonToCollisionPolygons(collisionTile.ObjectGroups[0].Objects[0], new Vector2((int)x, (int)y));
                                break;
                            case TmxObjectType.Basic:
                                AddRectangleToCollisionPolygons(collisionTile.ObjectGroups[0].Objects[0], new Vector2((int)x, (int)y));
                                break;
                        }
                    }
                }
            }            
        }

        private void AddPolygonToCollisionPolygons(TmxObject tmxObject, Vector2 offset)
        {           
            var points = new Vector2[tmxObject.Points.Count];
            for(int i=0 ; i<tmxObject.Points.Count; i++)
            {
                points[i].X = (float)(tmxObject.Points[i].X + tmxObject.X);
                points[i].Y = (float)(tmxObject.Points[i].Y + tmxObject.Y);
                points[i] += offset;
            }
            CollisionPolygons.Add(new Polygon(points, new Vector2((float)tmxObject.X,(float)tmxObject.Y) + offset));
        }

        private void AddRectangleToCollisionPolygons(TmxObject tmxObject, Vector2 offset)
        {
            var rectangle = new Rectangle((int)(tmxObject.X + offset.X), (int)(tmxObject.Y + offset.Y),
                (int)tmxObject.Width, (int)tmxObject.Height); 
            CollisionPolygons.Add(new Polygon(rectangle));
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

        public void Draw(SpriteBatch spriteBatch)
        {
            Tileset currentTileset;

            for (var j = 0; j < map.Layers.Count; j++)
            {
                for (var i = 0; i < map.Layers[j].Tiles.Count; i++)
                {
                    int gid = map.Layers[j].Tiles[i].Gid;
                    currentTileset = tilesets[GetTilesetIndex(gid)];
                    
                    if (gid != 0)                   
                    {
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
                                layer = Layers.Vegatation;
                                break;
                            case 2:
                                layer = Layers.Road;
                                break;
                            case 3:
                                layer = Layers.Accesories;
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

        #endregion

    }
}
