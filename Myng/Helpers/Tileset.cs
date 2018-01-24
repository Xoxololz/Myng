using Microsoft.Xna.Framework.Graphics;
using Myng.States;
using System.Collections.Generic;
using TiledSharp;

namespace Myng.Helpers
{
    public class Tileset
    {
        #region Properties

        public int TileWidth
        {
            get
            {
                return tileset.TileWidth;
            }
        }

        public int TileHeight
        {
            get
            {
                return tileset.TileHeight;
            }
        }

        public int FirstGid
        {
            get
            {
                return tileset.FirstGid;
            }
        }

        public int TilesetTilesWide
        {
            get
            {
                return Texture.Width / tileset.TileWidth;
            }
        }

        public int TilesetTilesHigh
        {
            get
            {
                return Texture.Height / tileset.TileHeight;
            }
        }

        public Dictionary<int,TmxTilesetTile> Tiles
        {
            get
            {
                return tileset.Tiles;
            }
        }

        public Texture2D Texture;

        #endregion

        #region Fields

        TmxTileset tileset;

        #endregion

        #region Constructors

        public Tileset(TmxTileset tileset)
        {
            this.tileset = tileset;

            var texturePath = tileset.Image.Source.Substring(0, tileset.Image.Source.LastIndexOf('.'));
            texturePath = texturePath.Substring(texturePath.IndexOf('.') + 1);
            Texture = State.Content.Load<Texture2D>(texturePath);
        }

        #endregion
    }
}
