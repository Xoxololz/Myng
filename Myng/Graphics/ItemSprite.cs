using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers;

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
        public ItemSprite(Texture2D texture2D, Vector2 position) : base(texture2D, position)
        {
            item = new Item();
        }
        #endregion

        #region Methods

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Player player = null;

            foreach (var sprite in sprites)
            {
                player = (Player)sprite;

                if (player != null) break;
            }

            //exit if no player was found
            if (player == null) return;

            //count distance from player
            var dist = player.Position - this.Position;

            //if item is close enough to player pick it up and exit
            if (dist.Length() < pickUpRange)
            {
                AddItem(player);
                ToRemove = true;
                return;
            }

            // attract item if its close enough
            if (dist.Length() < attractRange)
            {
                Position += CountSpeed(dist);
            }                                

        }

        private Vector2 CountSpeed(Vector2 dist)
        {
            var speed = dist / 4;

            return speed;
        }

        private void AddItem(Player player)
        {
            if (!player.Items.Exists((p) => item.GetType() == p.GetType()))
            {
                item.Parent = player;
                player.Items.Add(item);
            }
            else
            {
                foreach (var item in player.Items)
                {
                    if (item.GetType() == item.GetType())
                    {
                        item.Count++;
                        break;
                    }
                }
            }
        }

        #endregion
    }
}
