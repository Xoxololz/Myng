using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myng.Graphics.GUI;
using Myng.Helpers;
using Myng.Helpers.Enums;

namespace Myng.States
{
    public class CharacterState : State
    {
        #region Fields

        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private Texture2D mouseTex;
        private Vector2 mousePos;
        private Vector2 mouseOrig;

        private GUI gui;

        private CharacterMenu characterMenu;
        #endregion

        #region Constructors
        public CharacterState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game) : base(content, graphicsDevice, game)
        {
        }
        #endregion

        #region Methods
        public override void Init()
        {
            //mouse
            mouseTex = Content.Load<Texture2D>("GUI/mouse_normal");
            currentMouseState = Mouse.GetState();
            mouseOrig = new Vector2(mouseTex.Width / 2, mouseTex.Height / 2);

            //menu
            characterMenu = new CharacterMenu();
            gui = new GUI();
        }

        public override void Update(GameTime gameTime)
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            mousePos = currentMouseState.Position.ToVector2();

            HandleMouseIcon();
            HandleMouse(gameTime);
            characterMenu.Update(gameTime);
            gui.Update(gameTime);
        }

        private void HandleMouseIcon()
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                mouseTex = Content.Load<Texture2D>("GUI/mouse_clicking");
            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                mouseTex = Content.Load<Texture2D>("GUI/mouse_normal");
        }

        /// <summary>
        /// This method handles clicking on character menu, leveling up, etc
        /// </summary>
        private void HandleMouse(GameTime gameTime)
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                //should exit?
                if (characterMenu.GetExitArea().Contains((mousePos - Camera.ScreenOffset).ToPoint()))
                {
                    this.toRemove = true;
                }

                foreach(Attributes attr in Enum.GetValues(typeof(Attributes)))
                {
                    if(characterMenu.GetAttributeButtonArea(attr).Contains((mousePos - Camera.ScreenOffset).ToPoint()))
                    {
                        Game1.Player.ImproveAttribute(attr, 1);
                    }
                }
            }
        }

        public override void Exit()
        {
            //do nothing
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Game1.Player.Spellbar.Draw(spriteBatch);
            Game1.Player.Inventory.DrawPotions(spriteBatch);
            gui.Draw(spriteBatch);
            characterMenu.Draw(spriteBatch);

            //mouse
            float mouseScale = 2;
            spriteBatch.Draw(texture: mouseTex, position: -Camera.ScreenOffset + mousePos + mouseOrig * mouseScale, sourceRectangle: null, color: Color.White,
                   rotation: 0, origin: mouseOrig, scale: mouseScale, effects: SpriteEffects.None, layerDepth: Layers.AlwaysOnTop);
        }
        #endregion

    }
}
