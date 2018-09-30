using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers;
using Myng.States;

namespace Myng.Graphics.GUI
{
    public class GUI
    {
        #region Fields
        private Texture2D gui;
        private Texture2D hpBar;
        private Texture2D manaBar;
        private Texture2D xpBar;
        private Texture2D menuIconNormal;
        private Texture2D menuIconClicked;
        private Texture2D characterIcon;
        private Texture2D inventoryIcon;

        private Texture2D charBackgroundIcon;
        private Texture2D invBackgroundIcon;

        private Vector2 guiOrigin;
        private Vector2 hpOrigin;
        private Vector2 manaOrigin;
        private Vector2 xpOrigin;
        private Vector2 menuIconOrigin;
        private Vector2 charIconOrigin;
        private Vector2 invIconOrigin;

        private SpriteFont font;

        private float scale = 1.6f;
        private float textScale = 1f;
        private float menuIconsScale = 1.2f;

        Vector2 guiPosition;

        Vector2 hpPosition;
        Rectangle hpSource;
        string hpText;
        Vector2 hpTextPosition;

        Vector2 manaPosition;
        Rectangle manaSource;
        string manaText;
        Vector2 manaTextPosition;

        Vector2 xpPosition;
        Rectangle xpSource;
        Vector2 levelTextPosition;

        Vector2 charMenuIconBgroundPos;
        Vector2 charMenuIconPos;
        string charMenuText = "C";
        Vector2 charMenuTextPos;

        Vector2 invMenuIconBgroundPos;
        Vector2 invMenuIconPos;
        string invMenuText = "I";
        Vector2 invMenuTextPos;
        #endregion

        #region Constructors
        public GUI()
        {
            gui = State.Content.Load<Texture2D>("GUI/MyngGUI");
            hpBar = State.Content.Load<Texture2D>("GUI/HPBar");
            manaBar = State.Content.Load<Texture2D>("GUI/ManaBar");
            xpBar = State.Content.Load<Texture2D>("GUI/XPBar");
            menuIconNormal = State.Content.Load<Texture2D>("GUI/MenuIconBackground");
            menuIconClicked = State.Content.Load<Texture2D>("GUI/MenuIconBackgroundClicked");
            characterIcon = State.Content.Load<Texture2D>("Icons/MageIcon");
            inventoryIcon = State.Content.Load<Texture2D>("Icons/ChestIcon");
            font = State.Content.Load<SpriteFont>("Fonts/Font");

            guiOrigin = new Vector2((float)gui.Width / 2, (float)gui.Height / 2);
            hpOrigin = new Vector2((float)hpBar.Width / 2, (float)hpBar.Height / 2);
            manaOrigin = new Vector2((float)manaBar.Width / 2, (float)manaBar.Height / 2);
            xpOrigin = new Vector2((float)xpBar.Width / 2, (float)xpBar.Height / 2);
            menuIconOrigin = new Vector2((float)menuIconNormal.Width / 2, (float)menuIconNormal.Height / 2);
            charIconOrigin = new Vector2((float)characterIcon.Width / 2, (float)characterIcon.Height / 2);
            invIconOrigin = new Vector2((float)inventoryIcon.Width / 2, (float)inventoryIcon.Height / 2);
        }
        #endregion

        #region Methods
        //always update this after camera was updated
        public void Update(GameTime gameTime)
        {
            guiPosition = -Camera.ScreenOffset + new Vector2(10, 10);

            hpPosition = guiPosition + new Vector2(73, 8) * scale;
            hpSource = new Rectangle(0, 0, (int)((hpBar.Width * Game1.Player.Health) / Game1.Player.MaxHealth), hpBar.Height);
            hpText = string.Format("{0} / {1}", Game1.Player.Health.ToString(), Game1.Player.MaxHealth.ToString());
            hpTextPosition = hpPosition + scale * new Vector2(hpBar.Width, hpBar.Height + 5) / 2 - textScale * font.MeasureString(hpText) / 2;

            manaPosition = guiPosition + new Vector2(73, 28) * scale;
            manaSource = new Rectangle(0, 0, (int)((manaBar.Width * Game1.Player.Mana) / Game1.Player.MaxMana), manaBar.Height);
            manaText = string.Format("{0} / {1}", Game1.Player.Mana.ToString(), Game1.Player.MaxMana.ToString());
            manaTextPosition = manaPosition + scale * new Vector2(manaBar.Width, manaBar.Height + 5) / 2 - textScale * font.MeasureString(manaText) / 2;

            xpPosition = guiPosition + new Vector2(73, 48) * scale;
            xpSource = new Rectangle(0, 0, (int)((xpBar.Width * Game1.Player.XP) / Game1.Player.NextLevelXP), xpBar.Height);

            levelTextPosition = guiPosition + new Vector2(57, 56) * scale - textScale * font.MeasureString(Game1.Player.Level.ToString()) / 2;

            charMenuIconBgroundPos = -Camera.ScreenOffset + new Vector2(Game1.ScreenWidth - menuIconNormal.Width * menuIconsScale, 0);
            charMenuIconPos = charMenuIconBgroundPos + (menuIconOrigin - charIconOrigin)*menuIconsScale;
            charMenuTextPos = charMenuIconBgroundPos + (menuIconNormal.Bounds.Size.ToVector2() - new Vector2(16, 18)) *menuIconsScale;

            invMenuIconBgroundPos = -Camera.ScreenOffset + new Vector2(Game1.ScreenWidth - menuIconNormal.Width * menuIconsScale, menuIconNormal.Height * menuIconsScale);
            invMenuIconPos = invMenuIconBgroundPos + (menuIconOrigin - invIconOrigin) * menuIconsScale;
            invMenuTextPos = invMenuIconBgroundPos + (menuIconNormal.Bounds.Size.ToVector2() - new Vector2(14, 18)) * menuIconsScale;

            charBackgroundIcon = Game1.CurrentState is CharacterState ? menuIconClicked : menuIconNormal;
            invBackgroundIcon = Game1.CurrentState is InventoryState ? menuIconClicked : menuIconNormal;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //backgrouund
            spriteBatch.Draw(texture: gui, position: guiPosition+scale*guiOrigin, sourceRectangle: null, color: Color.White,
                   rotation: 0, origin: guiOrigin, scale: scale, effects: SpriteEffects.None, layerDepth: Layers.Inventory);

            //HP bar
            spriteBatch.Draw(texture: hpBar, position: hpPosition + scale * hpOrigin, sourceRectangle: hpSource, color: Color.White,
                   rotation: 0, origin: hpOrigin, scale: scale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);

            //HP text
            spriteBatch.DrawString(spriteFont: font,text: hpText,position: hpTextPosition + textScale*font.MeasureString(hpText)/2,color: Color.Black, rotation: 0f,
                origin: font.MeasureString(hpText) / 2, scale: textScale,effects: SpriteEffects.None,layerDepth: Layers.InventoryItemFont);

            //Mana bar
            spriteBatch.Draw(texture: manaBar, position: manaPosition + scale * manaOrigin, sourceRectangle: manaSource, color: Color.White,
                   rotation: 0, origin: manaOrigin, scale: scale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);

            //Mana text
            spriteBatch.DrawString(spriteFont: font, text: manaText, position: manaTextPosition + textScale*font.MeasureString(manaText)/2, color: Color.Black, rotation: 0f,
                origin: font.MeasureString(manaText) / 2, scale: textScale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItemFont);
            
            //XP bar
            spriteBatch.Draw(texture: xpBar, position: xpPosition + scale * xpOrigin, sourceRectangle: xpSource, color: Color.White,
                   rotation: 0, origin: xpOrigin, scale: scale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);

            //level text
            spriteBatch.DrawString(font, Game1.Player.Level.ToString(), levelTextPosition, Color.Black);

            //menu icons
            spriteBatch.Draw(texture: charBackgroundIcon, position: charMenuIconBgroundPos + menuIconsScale * menuIconOrigin, sourceRectangle: null, color: Color.White,
                   rotation: 0, origin: menuIconOrigin, scale: menuIconsScale, effects: SpriteEffects.None, layerDepth: Layers.Inventory);
            spriteBatch.Draw(texture: characterIcon, position: charMenuIconPos + menuIconsScale * charIconOrigin, sourceRectangle: null, color: Color.White,
                   rotation: 0, origin: charIconOrigin, scale: menuIconsScale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);
            spriteBatch.DrawString(spriteFont: font, text: charMenuText, position: charMenuTextPos + textScale * font.MeasureString(charMenuText) / 2, color: Color.LightGoldenrodYellow, rotation: 0f,
                origin: font.MeasureString(charMenuText) / 2, scale: textScale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItemFont);

            spriteBatch.Draw(texture: invBackgroundIcon, position: invMenuIconBgroundPos + menuIconsScale * menuIconOrigin, sourceRectangle: null, color: Color.White,
                    rotation: 0, origin: menuIconOrigin, scale: menuIconsScale, effects: SpriteEffects.None, layerDepth: Layers.Inventory);
            spriteBatch.Draw(texture: inventoryIcon, position: invMenuIconPos + menuIconsScale * invIconOrigin, sourceRectangle: null, color: Color.White,
                   rotation: 0, origin: invIconOrigin, scale: menuIconsScale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);
            spriteBatch.DrawString(spriteFont: font, text: invMenuText, position: invMenuTextPos + textScale * font.MeasureString(invMenuText) / 2, color: Color.LightGoldenrodYellow, rotation: 0f,
                origin: font.MeasureString(invMenuText) / 2, scale: textScale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItemFont);
        }
        #endregion
    }
}
