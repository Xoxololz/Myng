using Microsoft.Xna.Framework;
using Myng.Graphics;
using Myng.Helpers;
using Myng.States;
using System.Collections.Generic;

namespace Myng.AI.Movement
{
    public static class NodeMapRepository
    {
        private static Dictionary<Polygon, NodeMap> nodeMaps = new Dictionary<Polygon, NodeMap>(new PolygonComparer());

        public static NodeMap GetNodeMap(SpritePolygon collisionPolygon)
        {           
            var polygon = (SpritePolygon)collisionPolygon.Clone();
            polygon.MoveTo(Vector2.Zero);

            if (nodeMaps.TryGetValue(polygon, out NodeMap nodeMap)) return nodeMap;

            var map = NodeMap.CreateFromTileMap(GameState.TileMap, polygon);
            polygon.MoveTo(Vector2.Zero);
            nodeMaps.Add(polygon , map);
            return map;
        }

        public static void Update(List<Sprite> sprites)
        {
            foreach (var nodeMap in nodeMaps.Values)
            {
                nodeMap.Update(sprites);
            }
        }
    }
}
