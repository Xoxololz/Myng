using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers;
using Myng.States;

namespace Myng.Graphics
{
    public class GUI
    {
        #region Fields
        private Texture2D gui;
        private Texture2D hpBar;
        private Texture2D manaBar;
        private Texture2D xpBar;

        private Vector2 guiOrigin;
        private Vector2 hpOrigin;
        private Vector2 manaOrigin;
        private Vector2 xpOrigin;

        private SpriteFont font;

        private float scale;
        private float textScale;
        #endregion

        #region Constructors
        public GUI()
        {
            gui = State.Content.Load<Texture2D>("GUI/MyngGUI");
            hpBar = State.Content.Load<Texture2D>("GUI/HPBar");
            manaBar = State.Content.Load<Texture2D>("GUI/ManaBar");
            xpBar = State.Content.Load<Texture2D>("GUI/XPBar");
            font = State.Content.Load<SpriteFont>("Fonts/Font");
            scale = 1.5f;
            textScale = 0.9f;
            guiOrigin = new Vector2((float)gui.Width / 2, (float)gui.Height / 2);
            hpOrigin = new Vector2((float)hpBar.Width / 2, (float)hpBar.Height / 2);
            manaOrigin = new Vector2((float)manaBar.Width / 2, (float)manaBar.Height / 2);
            xpOrigin = new Vector2((float)xpBar.Width / 2, (float)xpBar.Height / 2);
        }
        #endregion

        #region Methods
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 guiPosition = -Camera.ScreenOffset + new Vector2(10, 10);
            spriteBatch.Draw(texture: gui, position: guiPosition+scale*guiOrigin, sourceRectangle: null, color: Color.White,
                   rotation: 0, origin: guiOrigin, scale: scale, effects: SpriteEffects.None, layerDepth: Layers.Inventory);

            Vector2 hpPosition = guiPosition + new Vector2(73, 8) * scale;
            Rectangle hpSource = new Rectangle(0, 0, (int) ((hpBar.Width*Game1.Player.Health)/Game1.Player.MaxHealth), hpBar.Height);
            spriteBatch.Draw(texture: hpBar, position: hpPosition + scale * hpOrigin, sourceRectangle: hpSource, color: Color.White,
                   rotation: 0, origin: hpOrigin, scale: scale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);            
            var hpText = string.Format("{0} / {1}", Game1.Player.Health.ToString(), Game1.Player.MaxHealth.ToString());
            Vector2 hpTextPosition = hpPosition + scale * new Vector2(hpBar.Width, hpBar.Height)/2 - textScale * font.MeasureString(hpText) / 2;
            spriteBatch.DrawString(spriteFont: font,text: hpText,position: hpTextPosition,color: Color.Black, rotation: 0f,
                origin:new Vector2(0),scale: textScale,effects: SpriteEffects.None,layerDepth: Layers.AlwaysOnTop);

            Vector2 manaPosition = guiPosition + new Vector2(73, 28)*scale;
            Rectangle manaSource = new Rectangle(0, 0, (int)((manaBar.Width * Game1.Player.Mana) / Game1.Player.MaxMana), manaBar.Height);
            spriteBatch.Draw(texture: manaBar, position: manaPosition + scale * manaOrigin, sourceRectangle: manaSource, color: Color.White,
                   rotation: 0, origin: manaOrigin, scale: scale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);            
            var manaText = string.Format("{0} / {1}", Game1.Player.Mana.ToString(), Game1.Player.MaxMana.ToString());
            Vector2 manaTextPosition = manaPosition + scale * new Vector2(manaBar.Width, manaBar.Height) / 2 - textScale * font.MeasureString(manaText) / 2;
            spriteBatch.DrawString(spriteFont: font, text: manaText, position: manaTextPosition, color: Color.Black, rotation: 0f,
                origin: new Vector2(0), scale: textScale, effects: SpriteEffects.None, layerDepth: Layers.AlwaysOnTop);

            Vector2 xpPosition = guiPosition + new Vector2(73, 48)*scale;
            Rectangle xpSource = new Rectangle(0, 0, (int)((xpBar.Width * Game1.Player.XP) / Game1.Player.NextLevelXP), xpBar.Height);
            spriteBatch.Draw(texture: xpBar, position: xpPosition + scale * xpOrigin, sourceRectangle: xpSource, color: Color.White,
                   rotation: 0, origin: xpOrigin, scale: scale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);

            if (Game1.Player.Level > 9)
            {
                Vector2 levelTextPosition = guiPosition + new Vector2(54, 47) * scale;
                spriteBatch.DrawString(font, Game1.Player.Level.ToString(), levelTextPosition - new Vector2(6, 0), Color.Black);
            }
            else
            {
                Vector2 levelTextPosition = guiPosition + new Vector2(54, 47) * scale;
                spriteBatch.DrawString(font, Game1.Player.Level.ToString(), levelTextPosition, Color.Black);
            }
        }
        #endregion
    }
}
