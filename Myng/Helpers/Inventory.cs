using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Items;
using Myng.Items.Interfaces;
using Myng.States;
using System.Collections.Generic;

namespace Myng.Helpers
{
    public class Inventory
    {
        #region Properties

        public List<Item> Items;

        #endregion

        #region Fields

        private int maxsize=6;

        private Texture2D texture;

        #endregion

        #region Constructors

        public Inventory()
        {
            texture = State.Content.Load<Texture2D>("itembar");
            Items = new List<Item>();
        }

        #endregion

        #region Methods

        public bool CanBeAdded(Item item)
        {
            if (!Items.Exists((p) => item.GetType() == p.GetType()))
            {
                if (Items.Count >= maxsize) return false;

                return true;
            }
            else
            {
                foreach (var a in Items)
                {
                    if (a.GetType() == item.GetType())
                    {
                        if (a.Count >= a.MaxCount) return false;

                        return true;
                    }
                }
            }
            return false;
        }

        public bool Add(Item item)
        {
            if (!Items.Exists((p) => item.GetType() == p.GetType()))
            {
                if (Items.Count >= maxsize) return false;

                if (item is IStatImprover)
                    ((IStatImprover)item).ImproveStats();

                Items.Add(item);

                return true;
            }
            else
            {
                foreach (var a in Items)
                {
                    if (a.GetType() == item.GetType())
                    {
                        if (item.Count >= item.MaxCount) return false;

                        item.Count++;
                        return true;
                    }
                }
            }
            return false;
        }

        public void ClearEmptyItems()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Count <= 0)
                {
                    Items.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 Position = new Vector2()
            {
                X = GameState.ScreenWidth/2 - Camera.ScreenOffset.X - texture.Width/2,
                Y = GameState.ScreenHeight - Camera.ScreenOffset.Y - texture.Height
            };
            spriteBatch.Draw(texture: texture, position: Position, color: Color.White * 0.5f, layerDepth: Layers.Inventory);
        }

        #endregion
    }
}
