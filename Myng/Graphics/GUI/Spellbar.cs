using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers;
using Myng.Helpers.Spells;
using Myng.States;
using System.Collections.Generic;

namespace Myng.Graphics.GUI
{
    public class Spellbar
    {
        #region Fields

        private int maxsize = 6;

        public List<Spell> spells;

        private Texture2D texture;

        private SpriteFont font;

        #endregion

        #region Constructors

        public Spellbar()
        {
            texture = State.Content.Load<Texture2D>("GUI/spellbar");
            spells = new List<Spell>();
            font = State.Content.Load<SpriteFont>("Fonts/Font");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks, whether a spell can be added to list of spells
        /// </summary>
        /// <param name="spell">Spell to be checked</param>
        /// <returns>False if the list is full. True otherwise</returns>
        public bool CanBeAdded(Spell spell)
        {
            if (spells.Count >= maxsize)
                return false;

            return true;
        }
        
        /// <summary>
        /// Adds spell to spell list
        /// </summary>
        /// <param name="spell">Spell to be added</param>
        /// <returns>True if spell was added successfuly, False otherwise</returns>
        public bool Add(Spell spell)
        {
            if(CanBeAdded(spell))
            {
                spells.Add(spell);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns Spell from list of spells with index <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Spell, if such a spell exists in the list, null otherwise.</returns>
        public Spell GetSpell(int index)
        {
            if (index >= spells.Count || index < 0) return null;

            return spells[index];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 Position = new Vector2()
            {
                X = Game1.ScreenWidth / 2 - Camera.ScreenOffset.X - texture.Width / 2,
                Y = Game1.ScreenHeight - Camera.ScreenOffset.Y - texture.Height
            };
            spriteBatch.Draw(texture: texture, position: Position, color: Color.White, layerDepth: Layers.Inventory);

            Position.Y += 16;

            for (int i = 0; i < maxsize; i++)
            {
                Position.X = Game1.ScreenWidth / 2 - Camera.ScreenOffset.X - texture.Width / 2 + 36 + (75 * i) + 4;
                spriteBatch.DrawString(font, (i+1).ToString(), new Vector2(Position.X - 4, Position.Y - 5) + font.MeasureString((i + 1).ToString()) / 2, Color.LightGoldenrodYellow, 0, font.MeasureString((i + 1).ToString()) / 2, 1f, SpriteEffects.None, Layers.InventoryItemFont);
            }

            for (int i = 0; i < spells.Count; i++)
            {
                Position.X = Game1.ScreenWidth / 2 - Camera.ScreenOffset.X - texture.Width / 2 + 36 + (75 * i) + 4;
                float scale = 40.0f / spells[i].Texture.Width;
                Vector2 origin = new Vector2(spells[i].Texture.Width / 2, spells[i].Texture.Height / 2);

                spriteBatch.Draw(texture: spells[i].Texture, position: Position + origin * scale, sourceRectangle: null, color: Color.White,
                   rotation: 0, origin: origin, scale: scale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);

                Vector2 textPosition = new Vector2(Position.X + 43 - 1.1f*font.MeasureString(spells[i].ManaCost.ToString()).X, Position.Y + 30);
                spriteBatch.DrawString(font, spells[i].ManaCost.ToString(), textPosition + 1.1f* font.MeasureString(spells[i].ManaCost.ToString())/2, Color.Blue, 0, font.MeasureString(spells[i].ManaCost.ToString()) / 2, 1.1f, SpriteEffects.None, Layers.InventoryItemFont);
            }
        }

        #endregion
    }
}
