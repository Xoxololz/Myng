using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers.Enums;
using System;
using System.Collections.Generic;
using TiledSharp;

namespace Myng.Helpers.Map
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

        private List<NonCollidableTexture> opaqueTextures;

        private Vector2 playerPos;
        private int[,] visionMap;
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

        public Vector2 MapSize { get; set; }
        /// <summary>
        /// Radius of the player's circle of vision
        /// </summary>
        public int VisualRange { get; set; }

        /// <summary>
        /// List of points visible to the player
        /// </summary>
        public List<Point> VisiblePoints { get; private set; }  // Cells the player can see

        public Vector2 PlayerPos { get { return playerPos; } set { playerPos = value; } }
        #endregion

        #region Constructor

        public TileMap(TmxMap map)
        {
            this.map = map;
            tilesets = new List<Tileset>();
            opaqueTextures = new List<NonCollidableTexture>();
            foreach (var tileset in map.Tilesets)
            {
                tilesets.Add(new Tileset(tileset));
            }

            MapSize = new Vector2(map.Width, map.Height);
            VisualRange = 15;
            visionMap = new int[map.Width, map.Height];

            screenWidthTiles = (int)Math.Floor((float)Game1.ScreenWidth / map.Tilesets[0].TileWidth) + 10;
            screenHeightTiles = (int)Math.Floor((float)Game1.ScreenHeight / map.Tilesets[0].TileWidth) + 10;

            InitOpaqueTextures();
            InitCollisionPolygons();

            MapHeightTiles = map.Height;
            MapWidthTiles = map.Width;
        }

        #endregion

        #region Methods

        private void InitOpaqueTextures()
        {
            for (int i = 0; i < map.ObjectGroups.Count; i++)
            {
                AddOpaqueTexture(map.ObjectGroups[i]);
            }
        }

        private void AddOpaqueTexture(TmxObjectGroup tmxObjectGroup)
        {
            foreach (var rect in tmxObjectGroup.Objects)
            {
                Rectangle x = new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
                opaqueTextures.Add(new NonCollidableTexture(x, map.TileWidth, map.TileHeight));
            }
        }

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

                    TmxTilesetTile collisionTile;
                    if (currentTileset.Tiles.TryGetValue(tileFrame, out collisionTile)
                        && collisionTile.ObjectGroups.Count > 0 && collisionTile.ObjectGroups[0].Objects.Count > 0)
                    {
                        Collision collisionType = GetCollisionType(collisionTile);

                        if (collisionType == Collision.Solid) 
                        {
                            var x = map.Layers[j].Tiles[i].X;
                            var y = map.Layers[j].Tiles[i].Y;
                            Point_Set(x, y, 1);
                        }

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
                    //if (layer.Name == "accesories") continue;
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
            float opacity = 1;
            GetVisibleCells();

            for (var j = 0; j < map.Layers.Count; j++)
            {
                for (var i = 0; i < map.Layers[j].Tiles.Count; i++)
                {
                    opacity = 1f;
                    int gid = map.Layers[j].Tiles[i].Gid;
                    if (gid != 0 && TileIsOnScreen(i))
                    {
                        currentTileset = tilesets[GetTilesetIndex(gid)];
                        int tileFrame = gid - currentTileset.FirstGid;
                        int column = tileFrame % currentTileset.TilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / currentTileset.TilesetTilesWide);

                        int mapColumn = (i % map.Width);
                        int x = mapColumn * map.TileWidth;
                        int mapRow = (int)Math.Floor(i / (double)map.Width);
                        int y = mapRow * map.TileHeight;

                        if (map.Layers[j].Name == "semiCollidable")
                            opacity = CheckForOpacityForWalls(mapColumn, mapRow);

                        Rectangle tilesetRec = new Rectangle(currentTileset.TileWidth * column, currentTileset.TileHeight * row, currentTileset.TileWidth, currentTileset.TileHeight);
                        float layer;
                        switch (j)
                        {
                            case 0:
                                layer = Layers.Background;
                                break;
                            case 1:
                                layer = Layers.Ground;
                                break;
                            case 2:
                                layer = Layers.SemiCollidable;
                                break;
                            case 3:
                                layer = Layers.Collidable;
                                break;
                            default:
                                throw new Exception("Too many layers in tile map");
                        }
                        if (VisiblePoints.Contains(new Point(map.Layers[j].Tiles[i].X, map.Layers[j].Tiles[i].Y)))
                        {
                            spriteBatch.Draw(texture: currentTileset.Texture, destinationRectangle: new Rectangle(x, y,
                                currentTileset.TileWidth, currentTileset.TileHeight), sourceRectangle: tilesetRec, color: Color.White * opacity,
                                layerDepth: layer);
                        }
                        else
                        {
                            spriteBatch.Draw(texture: currentTileset.Texture, destinationRectangle: new Rectangle(x, y,
                                currentTileset.TileWidth, currentTileset.TileHeight), sourceRectangle: tilesetRec, color: Color.White * opacity,
                                layerDepth: layer);
                            spriteBatch.Draw(texture: currentTileset.Texture, destinationRectangle: new Rectangle(x, y,
                                currentTileset.TileWidth, currentTileset.TileHeight), sourceRectangle: tilesetRec, color: Color.Black*0.7f,
                                layerDepth: Layers.DarkVision);
                        }
                    }
                }
            }
        }

        private float CheckForOpacityForWalls(int mapColumn, int mapRow)
        {
            foreach (var texture in opaqueTextures)
            {
                if (texture.IsTileIn(mapRow, mapColumn) && texture.PlayerIsUnder())
                {
                    return 0.5f;
                }
            }

            return 1;
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

        //START OF FOV
        //source https://github.com/AndyStobirski/RogueLike/blob/master/FOVRecurse.cs

        /// <summary>
        /// The octants which a player can see
        /// </summary>
        List<int> VisibleOctants = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };

        #region map point code

        /// <summary>
        /// Check if the provided coordinate is within the bounds of the mapp array
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <returns></returns>
        private bool Point_Valid(int pX, int pY)
        {
            return pX >= 0 & pX < visionMap.GetLength(0)
                    & pY >= 0 & pY < visionMap.GetLength(1);
        }

        /// <summary>
        /// Get the value of the point at the specified location
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <returns>Cell value</returns>
        public int Point_Get(int _x, int _y)
        {
            return visionMap[_x, _y];
        }

        /// <summary>
        /// Set the map point to the specified value
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <param name="_val"></param>
        public void Point_Set(int _x, int _y, int _val)
        {
            if (Point_Valid(_x, _y))
                visionMap[_x, _y] = _val;
        }

        #endregion

        #region FOV algorithm

        //  Octant data
        //
        //    \ 1 | 2 /
        //   8 \  |  / 3
        //   -----+-----
        //   7 /  |  \ 4
        //    / 6 | 5 \
        //
        //  1 = NNW, 2 =NNE, 3=ENE, 4=ESE, 5=SSE, 6=SSW, 7=WSW, 8 = WNW

        /// <summary>
        /// Start here: go through all the octants which surround the player to
        /// determine which open cells are visible
        /// </summary>
        public void GetVisibleCells()
        {
            VisiblePoints = new List<Point>();
            playerPos = new Vector2(Game1.Player.GlobalOrigin.X / TileWidth, Game1.Player.GlobalOrigin.Y / TileHeight);
            VisiblePoints.Add(playerPos.ToPoint());
            foreach (int o in VisibleOctants)
                ScanOctant(1, o, 1.0, 0.0);

        }

        /// <summary>
        /// Examine the provided octant and calculate the visible cells within it.
        /// </summary>
        /// <param name="pDepth">Depth of the scan</param>
        /// <param name="pOctant">Octant being examined</param>
        /// <param name="pStartSlope">Start slope of the octant</param>
        /// <param name="pEndSlope">End slope of the octance</param>
        protected void ScanOctant(int pDepth, int pOctant, double pStartSlope, double pEndSlope)
        {

            int visrange2 = VisualRange * VisualRange;
            int x = 0;
            int y = 0;

            switch (pOctant)
            {

                case 1: //nnw
                    y = playerPos.ToPoint().Y - pDepth;
                    if (y < 0) return;

                    x = playerPos.ToPoint().X - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x < 0) x = 0;

                    while (GetSlope(x, y, playerPos.X, playerPos.Y, false) >= pEndSlope && Point_Valid(x, y))
                    {
                        if (GetVisDistance(x, y, playerPos.ToPoint().X, playerPos.ToPoint().Y) <= visrange2)
                        {
                            if (visionMap[x, y] == 1) //current cell blocked
                            {
                                if (x - 1 >= 0 && visionMap[x - 1, y] == 0) //prior cell within range AND open...
                                                                      //...incremenet the depth, adjust the endslope and recurse
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, playerPos.X, playerPos.Y, false));
                            }
                            else
                            {
                                if (x - 1 >= 0 && visionMap[x - 1, y] == 1) //prior cell within range AND open...
                                                                      //..adjust the startslope
                                    pStartSlope = GetSlope(x - 0.5, y - 0.5, playerPos.X, playerPos.Y, false);

                                VisiblePoints.Add(new Point(x, y));
                            }
                        }
                        x++;
                    }
                    x--;
                    break;

                case 2: //nne

                    y = playerPos.ToPoint().Y - pDepth;
                    if (y < 0) return;

                    x = playerPos.ToPoint().X + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x >= visionMap.GetLength(0)) x = visionMap.GetLength(0) - 1;

                    while (GetSlope(x, y, playerPos.X, playerPos.Y, false) <= pEndSlope && Point_Valid(x, y))
                    {
                        if (GetVisDistance(x, y, playerPos.ToPoint().X, playerPos.ToPoint().Y) <= visrange2)
                        {
                            if (visionMap[x, y] == 1)
                            {
                                if (x + 1 < visionMap.GetLength(0) && visionMap[x + 1, y] == 0)
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, playerPos.X, playerPos.Y, false));
                            }
                            else
                            {
                                if (x + 1 < visionMap.GetLength(0) && visionMap[x + 1, y] == 1)
                                    pStartSlope = -GetSlope(x + 0.5, y - 0.5, playerPos.X, playerPos.Y, false);

                                VisiblePoints.Add(new Point(x, y));
                            }
                        }
                        x--;
                    }
                    x++;
                    break;

                case 3:

                    x = playerPos.ToPoint().X + pDepth;
                    if (x >= visionMap.GetLength(0)) return;

                    y = playerPos.ToPoint().Y - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y < 0) y = 0;

                    while (GetSlope(x, y, playerPos.X, playerPos.Y, true) <= pEndSlope && Point_Valid(x, y))
                    {

                        if (GetVisDistance(x, y, playerPos.ToPoint().X, playerPos.ToPoint().Y) <= visrange2)
                        {

                            if (visionMap[x, y] == 1)
                            {
                                if (y - 1 >= 0 && visionMap[x, y - 1] == 0)
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, playerPos.X, playerPos.Y, true));
                            }
                            else
                            {
                                if (y - 1 >= 0 && visionMap[x, y - 1] == 1)
                                    pStartSlope = -GetSlope(x + 0.5, y - 0.5, playerPos.X, playerPos.Y, true);

                                VisiblePoints.Add(new Point(x, y));
                            }
                        }
                        y++;
                    }
                    y--;
                    break;

                case 4:

                    x = playerPos.ToPoint().X + pDepth;
                    if (x >= visionMap.GetLength(0)) return;

                    y = playerPos.ToPoint().Y + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y >= visionMap.GetLength(1)) y = visionMap.GetLength(1) - 1;

                    while (GetSlope(x, y, playerPos.X, playerPos.Y, true) >= pEndSlope && Point_Valid(x, y))
                    {

                        if (GetVisDistance(x, y, playerPos.ToPoint().X, playerPos.ToPoint().Y) <= visrange2)
                        {

                            if (visionMap[x, y] == 1)
                            {
                                if (y + 1 < visionMap.GetLength(1) && visionMap[x, y + 1] == 0)
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, playerPos.X, playerPos.Y, true));
                            }
                            else
                            {
                                if (y + 1 < visionMap.GetLength(1) && visionMap[x, y + 1] == 1)
                                    pStartSlope = GetSlope(x + 0.5, y + 0.5, playerPos.X, playerPos.Y, true);

                                VisiblePoints.Add(new Point(x, y));
                            }
                        }
                        y--;
                    }
                    y++;
                    break;

                case 5:

                    y = playerPos.ToPoint().Y + pDepth;
                    if (y >= visionMap.GetLength(1)) return;

                    x = playerPos.ToPoint().X + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x >= visionMap.GetLength(0)) x = visionMap.GetLength(0) - 1;

                    while (GetSlope(x, y, playerPos.X, playerPos.Y, false) >= pEndSlope && Point_Valid(x, y))
                    {
                        if (GetVisDistance(x, y, playerPos.ToPoint().X, playerPos.ToPoint().Y) <= visrange2)
                        {

                            if (visionMap[x, y] == 1)
                            {
                                if (x + 1 < visionMap.GetLength(1) && visionMap[x + 1, y] == 0)
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, playerPos.X, playerPos.Y, false));
                            }
                            else
                            {
                                if (x + 1 < visionMap.GetLength(1)
                                        && visionMap[x + 1, y] == 1)
                                    pStartSlope = GetSlope(x + 0.5, y + 0.5, playerPos.X, playerPos.Y, false);

                                VisiblePoints.Add(new Point(x, y));
                            }
                        }
                        x--;
                    }
                    x++;
                    break;

                case 6:

                    y = playerPos.ToPoint().Y + pDepth;
                    if (y >= visionMap.GetLength(1)) return;

                    x = playerPos.ToPoint().X - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x < 0) x = 0;

                    while (GetSlope(x, y, playerPos.X, playerPos.Y, false) <= pEndSlope && Point_Valid(x, y))
                    {
                        if (GetVisDistance(x, y, playerPos.ToPoint().X, playerPos.ToPoint().Y) <= visrange2)
                        {

                            if (visionMap[x, y] == 1)
                            {
                                if (x - 1 >= 0 && visionMap[x - 1, y] == 0)
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, playerPos.X, playerPos.Y, false));
                            }
                            else
                            {
                                if (x - 1 >= 0
                                        && visionMap[x - 1, y] == 1)
                                    pStartSlope = -GetSlope(x - 0.5, y + 0.5, playerPos.X, playerPos.Y, false);

                                VisiblePoints.Add(new Point(x, y));
                            }
                        }
                        x++;
                    }
                    x--;
                    break;

                case 7:

                    x = playerPos.ToPoint().X - pDepth;
                    if (x < 0) return;

                    y = playerPos.ToPoint().Y + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y >= visionMap.GetLength(1)) y = visionMap.GetLength(1) - 1;

                    while (GetSlope(x, y, playerPos.X, playerPos.Y, true) <= pEndSlope && Point_Valid(x, y))
                    {

                        if (GetVisDistance(x, y, playerPos.ToPoint().X, playerPos.ToPoint().Y) <= visrange2)
                        {

                            if (visionMap[x, y] == 1)
                            {
                                if (y + 1 < visionMap.GetLength(1) && visionMap[x, y + 1] == 0)
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, playerPos.X, playerPos.Y, true));
                            }
                            else
                            {
                                if (y + 1 < visionMap.GetLength(1) && visionMap[x, y + 1] == 1)
                                    pStartSlope = -GetSlope(x - 0.5, y + 0.5, playerPos.X, playerPos.Y, true);

                                VisiblePoints.Add(new Point(x, y));
                            }
                        }
                        y--;
                    }
                    y++;
                    break;

                case 8: //wnw

                    x = playerPos.ToPoint().X - pDepth;
                    if (x < 0) return;

                    y = playerPos.ToPoint().Y - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y < 0) y = 0;

                    while (GetSlope(x, y, playerPos.ToPoint().X, playerPos.ToPoint().Y, true) >= pEndSlope && Point_Valid(x, y))
                    {

                        if (GetVisDistance(x, y, playerPos.ToPoint().X, playerPos.ToPoint().Y) <= visrange2)
                        {

                            if (visionMap[x, y] == 1)
                            {
                                if (y - 1 >= 0 && visionMap[x, y - 1] == 0)
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, playerPos.X, playerPos.Y, true));

                            }
                            else
                            {
                                if (y - 1 >= 0 && visionMap[x, y - 1] == 1)
                                    pStartSlope = GetSlope(x - 0.5, y - 0.5, playerPos.X, playerPos.Y, true);

                                VisiblePoints.Add(new Point(x, y));
                            }
                        }
                        y++;
                    }
                    y--;
                    break;
            }


            if (x < 0)
                x = 0;
            else if (x >= visionMap.GetLength(0))
                x = visionMap.GetLength(0) - 1;

            if (y < 0)
                y = 0;
            else if (y >= visionMap.GetLength(1))
                y = visionMap.GetLength(1) - 1;

            if (pDepth < VisualRange & visionMap[x, y] == 0)
                ScanOctant(pDepth + 1, pOctant, pStartSlope, pEndSlope);

        }

        /// <summary>
        /// Get the gradient of the slope formed by the two points
        /// </summary>
        /// <param name="pX1"></param>
        /// <param name="pY1"></param>
        /// <param name="pX2"></param>
        /// <param name="pY2"></param>
        /// <param name="pInvert">Invert slope</param>
        /// <returns></returns>
        private double GetSlope(double pX1, double pY1, double pX2, double pY2, bool pInvert)
        {
            if (pInvert)
                return (pY1 - pY2) / (pX1 - pX2);
            else
                return (pX1 - pX2) / (pY1 - pY2);
        }


        /// <summary>
        /// Calculate the distance between the two points
        /// </summary>
        /// <param name="pX1"></param>
        /// <param name="pY1"></param>
        /// <param name="pX2"></param>
        /// <param name="pY2"></param>
        /// <returns>Distance</returns>
        private int GetVisDistance(int pX1, int pY1, int pX2, int pY2)
        {
            return ((pX1 - pX2) * (pX1 - pX2)) + ((pY1 - pY2) * (pY1 - pY2));
        }

        #endregion
    }
}
