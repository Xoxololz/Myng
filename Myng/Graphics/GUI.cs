﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers;
using Myng.States;

namespace Myng.Graphics
{
    public class GUI
    {
        private Texture2D gui;
        private Texture2D hpBar;
        private Texture2D manaBar;
        private Texture2D xpBar;

        private Vector2 guiOrigin;
        private Vector2 hpOrigin;
        private Vector2 manaOrigin;
        private Vector2 xpOrigin;

        private float scale;

        public GUI()
        {
            gui = State.Content.Load<Texture2D>("GUI/MyngGUI");
            hpBar = State.Content.Load<Texture2D>("GUI/HPBar");
            manaBar = State.Content.Load<Texture2D>("GUI/ManaBar");
            xpBar = State.Content.Load<Texture2D>("GUI/XPBar");
            scale = 1.5f;
            guiOrigin = new Vector2((float)gui.Width / 2, (float)gui.Height / 2);
            hpOrigin = new Vector2((float)hpBar.Width / 2, (float)hpBar.Height / 2);
            manaOrigin = new Vector2((float)manaBar.Width / 2, (float)manaBar.Height / 2);
            xpOrigin = new Vector2((float)xpBar.Width / 2, (float)xpBar.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 guiPosition = -Camera.ScreenOffset + new Vector2(40,10);
            spriteBatch.Draw(texture: gui, position: guiPosition + guiOrigin, sourceRectangle: null, color: Color.White,
                   rotation: 0, origin: guiOrigin, scale: scale, effects: SpriteEffects.None, layerDepth: Layers.Inventory);

            Vector2 hpPosition = guiPosition + new Vector2(101, 11);
            Rectangle hpSource = new Rectangle(0, 0, (int) ((hpBar.Width*Game1.Player.Health)/Game1.Player.MaxHealth), hpBar.Height);
            spriteBatch.Draw(texture: hpBar, position: hpPosition + hpOrigin, sourceRectangle: hpSource, color: Color.White,
                   rotation: 0, origin: hpOrigin, scale: scale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);

            Vector2 manaPosition = guiPosition + new Vector2(101, 11 + 20*scale);
            Rectangle manaSource = new Rectangle(0, 0, (int)((manaBar.Width * Game1.Player.Mana) / Game1.Player.MaxMana), manaBar.Height);
            spriteBatch.Draw(texture: manaBar, position: manaPosition + manaOrigin, sourceRectangle: manaSource, color: Color.White,
                   rotation: 0, origin: manaOrigin, scale: scale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);

            Vector2 xpPosition = guiPosition + new Vector2(101, 11 + 40*scale);
            Rectangle xpSource = new Rectangle(0, 0, (int)((xpBar.Width * Game1.Player.XP) / Game1.Player.NextLevelXP), xpBar.Height);
            spriteBatch.Draw(texture: xpBar, position: xpPosition + xpOrigin, sourceRectangle: xpSource, color: Color.White,
                   rotation: 0, origin: xpOrigin, scale: scale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);
        }
    }
}
