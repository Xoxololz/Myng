
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers;
using Myng.Helpers.Enums;
using Myng.States;
using System;

namespace Myng.Graphics.GUI
{
    public class CharacterMenu
    {
        #region Fields
        //fonts
        private SpriteFont font;

        //character menu
        private Texture2D characterMenuBackground;
        private Vector2 characterMenuBackgroundOrigin;
        private Vector2 characterMenuBackgroundPos;

        //levelup button
        private Texture2D plusButton;
        private Vector2 plusButtonOrigin;
        private Vector2 plusButtonPos;

        //scaling
        private float stasTextScale = 1.12f;
        private float attrTextScale = 1.15f;
        private float infoTextScale = 1.15f;
        private float attrDescriptionTextScale = 0.75f;
        private float menuScale = 1.8f;

        //positions for text
        private Vector2 characterInfoPos;
        private Vector2 characterInfoValuePos;
        private Vector2 characterAttributesPos;
        private Vector2 characterAttributesValuePos;
        private Vector2 characterStatsPos;
        private Vector2 characterStatsValuePos;

        //colors
        private Color nameColor = Color.LightGoldenrodYellow;
        private Color descriptionColor = Color.GreenYellow;
        private Color valueColor = Color.White;
        #endregion

        #region Properties
        public float Scale
        {
            get
            {
                return menuScale;
            }
        }

        public Rectangle GetExitArea()
        {
            return new Rectangle((characterMenuBackgroundPos + new Vector2(353, 10) * Scale).ToPoint(), (new Vector2(38, 43) * Scale).ToPoint());
        }

        public Rectangle GetAttributeButtonArea(Attributes attr)
        {
            return new Rectangle((plusButtonPos + new Vector2(0, (font.MeasureString("text").Y + 22) * attrTextScale * (int)(attr))).ToPoint(), (plusButton.Bounds.Size.ToVector2() * attrTextScale).ToPoint());
        }
        #endregion

        #region Constructors
        public CharacterMenu()
        {
            //fonts
            font = State.Content.Load<SpriteFont>("Fonts/Font");

            //textures
            characterMenuBackground = State.Content.Load<Texture2D>("GUI/CharacterMenu");
            plusButton = State.Content.Load<Texture2D>("GUI/plus_button");

            //origins
            characterMenuBackgroundOrigin = new Vector2(characterMenuBackground.Width / 2, characterMenuBackground.Height / 2);
            plusButtonOrigin = new Vector2(plusButton.Width / 2, plusButton.Height / 2);
        }
        #endregion

        #region Methods
        public void Update(GameTime gameTime)
        {
            //background
            characterMenuBackgroundPos = -Camera.ScreenOffset + new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2) - characterMenuBackgroundOrigin * menuScale;
            //info
            characterInfoPos = characterMenuBackgroundPos + new Vector2(11, 110) * menuScale;
            characterInfoValuePos = characterInfoPos + new Vector2(140, 0) * infoTextScale;
            //attributes
            characterAttributesPos = characterMenuBackgroundPos + new Vector2(164, 110) * menuScale;
            characterAttributesValuePos = characterAttributesPos + new Vector2(180, 0) * attrTextScale;
            //stats
            characterStatsPos = characterMenuBackgroundPos + new Vector2(342, 110) * menuScale;
            characterStatsValuePos = characterStatsPos + new Vector2(150, 0) * stasTextScale;
            //button
            plusButtonPos = characterAttributesValuePos + new Vector2(40, -5) * attrTextScale;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //background picture
            spriteBatch.Draw(texture: characterMenuBackground, position: characterMenuBackgroundPos + characterMenuBackgroundOrigin * menuScale, sourceRectangle: null, color: Color.White,
               rotation: 0, origin: characterMenuBackgroundOrigin, scale: menuScale, effects: SpriteEffects.None, layerDepth: Layers.InventoryBackground);

            //stats
            string text = "";
            foreach(Stats stat in Enum.GetValues(typeof(Stats))){
                text = stat.GetName() + ":";
                switch (stat)
                {
                    case Stats.ATTACK_SPEED:
                        spriteBatch.DrawString(font, text, characterStatsPos + (font.MeasureString(text) / 2) * stasTextScale, nameColor, 0, font.MeasureString(text) / 2, stasTextScale, SpriteEffects.None, Layers.InventoryItemFont);
                        text = (1 / Game1.Player.AttackSpeed).ToString("0.00");
                        break;
                    case Stats.BLOCK:
                        spriteBatch.DrawString(font, text, characterStatsPos + (font.MeasureString(text) / 2) * stasTextScale, nameColor, 0, font.MeasureString(text) / 2, stasTextScale, SpriteEffects.None, Layers.InventoryItemFont);
                        text = Game1.Player.BlockChance.ToString("0.00") + "%";
                        break;
                    case Stats.CRIT:
                        spriteBatch.DrawString(font, text, characterStatsPos + (font.MeasureString(text) / 2) * stasTextScale, nameColor, 0, font.MeasureString(text) / 2, stasTextScale, SpriteEffects.None, Layers.InventoryItemFont);
                        text = Game1.Player.CritChance.ToString("0.00") + "%";
                        break;
                    case Stats.MAGIC_DEFENSE:
                        spriteBatch.DrawString(font, text, characterStatsPos + (font.MeasureString(text) / 2) * stasTextScale, nameColor, 0, font.MeasureString(text) / 2, stasTextScale, SpriteEffects.None, Layers.InventoryItemFont);
                        text = Game1.Player.MagicDefense.ToString();
                        break;
                    case Stats.MAX_DAMAGE:
                        continue;
                    case Stats.MIN_DAMAGE:
                        continue;
                    case Stats.MOVEMENT_SPEED:
                        spriteBatch.DrawString(font, text, characterStatsPos + (font.MeasureString(text) / 2) * stasTextScale, nameColor, 0, font.MeasureString(text) / 2, stasTextScale, SpriteEffects.None, Layers.InventoryItemFont);
                        text = Game1.Player.MovementSpeedBonus.ToString("0.00");
                        break;
                    case Stats.PHYSICAL_DEFENSE:
                        spriteBatch.DrawString(font, text, characterStatsPos + (font.MeasureString(text) / 2) * stasTextScale, nameColor, 0, font.MeasureString(text) / 2, stasTextScale, SpriteEffects.None, Layers.InventoryItemFont);
                        text = Game1.Player.PhysicalDefense.ToString();
                        break;
                    default: continue;
                }
                spriteBatch.DrawString(font, text, characterStatsValuePos + (font.MeasureString(text) / 2) * stasTextScale, valueColor, 0, font.MeasureString(text) / 2, stasTextScale, SpriteEffects.None, Layers.InventoryItemFont);
                characterStatsPos += new Vector2(0, (font.MeasureString(text).Y + 20) * stasTextScale);
                characterStatsValuePos += new Vector2(0, (font.MeasureString(text).Y + 20) * stasTextScale);
            }

            //attributes
            foreach (Attributes att in Enum.GetValues(typeof(Attributes)))
            {
                spriteBatch.Draw(texture: plusButton, position: plusButtonPos + new Vector2(0, (font.MeasureString("text").Y + 22) * attrTextScale * (int)(att)) + plusButtonOrigin * attrTextScale, sourceRectangle: null, color: Color.White,
                            rotation: 0, origin: plusButtonOrigin, scale: attrTextScale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);

                DrawLine(spriteBatch, att.GetName() + ":", Game1.Player.GetAttribute(att).ToString(), ref characterAttributesPos, ref characterAttributesValuePos, attrTextScale);
                spriteBatch.DrawString(font, att.GetDescription(), characterAttributesPos - new Vector2(0, (font.MeasureString("text").Y + 7) * attrTextScale) + (font.MeasureString(att.GetDescription()) / 2) * attrDescriptionTextScale, descriptionColor, 0, font.MeasureString(att.GetDescription()) / 2, attrDescriptionTextScale, SpriteEffects.None, Layers.InventoryItemFont);
            }

            //player info
            DrawLine(spriteBatch, "Class:", Game1.Player.Identity.Name, ref characterInfoPos, ref characterInfoValuePos, infoTextScale);
            DrawLine(spriteBatch, "Level:", Game1.Player.Level.ToString(), ref characterInfoPos, ref characterInfoValuePos, infoTextScale);
            DrawLine(spriteBatch, "Attr. points:", Game1.Player.CharacterPoints.ToString(), ref characterInfoPos, ref characterInfoValuePos, infoTextScale);
            DrawLine(spriteBatch, "Health:", string.Format("{0}/{1}", Game1.Player.Health, Game1.Player.MaxHealth), ref characterInfoPos, ref characterInfoValuePos, infoTextScale);
            DrawLine(spriteBatch, "Mana:", string.Format("{0}/{1}", Game1.Player.Mana, Game1.Player.MaxMana), ref characterInfoPos, ref characterInfoValuePos, infoTextScale);
        }

        /// <summary>
        /// Draws name of stat/attribute and value given their positions and changes position for next line 
        /// </summary>
        private void DrawLine(SpriteBatch spriteBatch, string name, string value, ref Vector2 namePos, ref Vector2 valuePos, float scale)
        {
            spriteBatch.DrawString(font, name, namePos + (font.MeasureString(name) / 2) * scale, nameColor, 0, font.MeasureString(name) / 2, scale, SpriteEffects.None, Layers.InventoryItemFont);
            spriteBatch.DrawString(font, value, valuePos + (font.MeasureString(value) / 2) * scale, valueColor, 0, font.MeasureString(value) / 2, scale, SpriteEffects.None, Layers.InventoryItemFont);
            namePos += new Vector2(0, (font.MeasureString(name).Y + 22) * scale);
            valuePos += new Vector2(0, (font.MeasureString(name).Y + 22) * scale);
        }
        #endregion
    }
}
