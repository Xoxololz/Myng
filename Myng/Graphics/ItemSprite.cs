using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers;
using Myng.Items;

namespace Myng.Graphics
{
    public class ItemSprite : Sprite
    {
        #region Fields

        private float pickUpRange = 20f;

        private float attractRange = 100f;

        private Item item;

        #endregion


        #region Constructors
        public ItemSprite(Texture2D texture2D, Vector2 position, Item item) : base(texture2D, position)
        {
            layer = Layers.Item;
            this.item = item;
        }
        #endregion

        #region Methods

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites, TileMap tileMap)
        {
            Player player = Game1.Player;

            //exit if no player was found
            if (player == null) return;

            item.Parent = player;

            //count distance from player
            var dist = player.Position - this.Position;

            //if item is close enough to player pick it up and exit
            if (dist.Length() < pickUpRange)
            {               
                if (player.Inventory.Add(item))
                {                    
                    ToRemove = true;
                    return;
                }                
            }

            // attract item if its close enough
            if (dist.Length() < attractRange)
            {
                if (player.Inventory.CanBeAdded(item))
                {
                    Position += CountSpeed(dist);
                }                
            }                                

        }

        private Vector2 CountSpeed(Vector2 dist)
        {
            var speed = dist / 4;

            return speed;
        }

        #endregion
    }
}
