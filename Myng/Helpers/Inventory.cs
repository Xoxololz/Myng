using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Items;
using Myng.Items.Interfaces;
using Myng.States;
using System.Collections.Generic;
using System.Diagnostics;

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

        private SpriteFont font;

        #endregion

        #region Constructors

        public Inventory()
        {
            texture = State.Content.Load<Texture2D>("GUI/spellbar");
            Items = new List<Item>();
            font = State.Content.Load<SpriteFont>("Fonts/Font");
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

                        a.Count++;
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
                X = GameState.ScreenWidth / 2 - Camera.ScreenOffset.X - texture.Width / 2,
                Y = GameState.ScreenHeight - Camera.ScreenOffset.Y - texture.Height
            };
            spriteBatch.Draw(texture: texture, position: Position, color: Color.White, layerDepth: Layers.Inventory);

            Position.Y += 16;
            for (int i = 0; i < Items.Count; i++)
            {
                Position.X = GameState.ScreenWidth / 2 - Camera.ScreenOffset.X - texture.Width / 2 + 36 + ( 75 * i ) + 4;
                float scale = 40.0f / Items[i].Texture.Width;
                Vector2 origin = new Vector2(Items[i].Texture.Width/2, Items[i].Texture.Height/2);

                spriteBatch.Draw(texture: Items[i].Texture, position: Position + origin*scale, sourceRectangle: null, color: Color.White,
                   rotation: 0, origin: origin, scale: scale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);

                Vector2 textPosition = new Vector2(Position.X + 46, Position.Y + 26);
                spriteBatch.DrawString(font, Items[i].Count.ToString(), textPosition-new Vector2(Items[i].Count.ToString().Length*13,0), Color.Black);
            }
        }

        #endregion
    }
}
