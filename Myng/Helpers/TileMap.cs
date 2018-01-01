using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
        }

        #endregion

        #region Methods

        public void Draw(SpriteBatch spriteBatch)
        {

            for (var j = 0; j < map.Layers.Count; j++)
            {
                for (var i = 0; i < map.Layers[j].Tiles.Count; i++)
                {
                    int gid = map.Layers[j].Tiles[i].Gid;

                    // Empty tile, do nothing
                    if (gid == 0)
                    {

                    }
                    else
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
                                layer = (int)Layers.BackGround*0.01f;
                                break;
                            case 1:
                                layer = (int)Layers.Vegatation * 0.01f;
                                break;
                            case 2:
                                layer = (int)Layers.Accesories * 0.01f;
                                break;                           
                            default:
                                layer = 1f;
                                break;
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
