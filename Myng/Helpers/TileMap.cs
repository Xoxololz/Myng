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
        private Texture2D tileset;

        private int tileWidth;
        private int tileHeight;
        private int tilesetTilesWide;
        private int tilesetTilesHigh;

        #endregion

        #region Variables

        public int MapWidth {
            get
            {
                return map.Width * tileWidth;
            }
        }
        public int MapHeight
        {
            get
            {
                return map.Height * tileHeight;
            }
        }

        public List<Polygon> CollisionPolygons = new List<Polygon>();

        #endregion

        #region Constructor

        public TileMap(TmxMap map, Texture2D tileset)
        {
            this.map = map;
            this.tileset = tileset;

            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;

            tilesetTilesWide = tileset.Width / tileWidth;
            tilesetTilesHigh = tileset.Height / tileHeight;

            InitCollisionPolygons();
        }

        #endregion

        #region Methods


        private void InitCollisionPolygons()
        {
            for (var j = 0; j < map.Layers.Count; j++)
            {
                for (var i = 0; i < map.Layers[j].Tiles.Count; i++)
                {
                    int gid = map.Layers[j].Tiles[i].Gid;

                    int tileFrame = gid - 1;                    

                    float x = (i % map.Width) * map.TileWidth;
                    float y = (float)Math.Floor(i / (double)map.Width) * map.TileHeight;

                    TmxTilesetTile collisionTile;
                    if (map.Tilesets[0].Tiles.TryGetValue(tileFrame, out collisionTile))
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

        public void Draw(SpriteBatch spriteBatch)
        {

            for (var j = 0; j < map.Layers.Count; j++)
            {
                for (var i = 0; i < map.Layers[j].Tiles.Count; i++)
                {
                    int gid = map.Layers[j].Tiles[i].Gid;
                    
                    if (gid != 0)                   
                    {
                        int tileFrame = gid - 1;
                        int column = tileFrame % tilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);

                        float x = (i % map.Width) * map.TileWidth;
                        float y = (float)Math.Floor(i / (double)map.Width) * map.TileHeight;
 
                       Rectangle tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);
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
                                layer = Layers.Accesories;
                                break;                           
                            default:
                                throw new Exception("Too many layers in tile map");
                        }

                        spriteBatch.Draw(texture: tileset,destinationRectangle: new Rectangle((int)x, (int)y,
                            tileWidth, tileHeight),sourceRectangle: tilesetRec,color: Color.White,
                            layerDepth: layer);
                    }
                }
            }

        }

        #endregion

    }
}
