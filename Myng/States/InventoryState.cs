﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myng.Helpers;
using Myng.Items;
using Myng.Graphics.GUI;

namespace Myng.States
{
    class InventoryState : State
    {
        #region Fields

        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private double clickTimer;

        //maximum time in milliseconds between two clicks for it to be doubleclick
        private const double doubleClickTimerDelay = 400;

        private Texture2D mouseTex;
        private Vector2 mousePos;
        private Vector2 mouseOrig;

        private Item pickedUpItem;
        private GUI gui;

        private Inventory inventory;
        #endregion

        #region Constructors   
        public InventoryState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game) : base(content, graphicsDevice, game)
        {
            
        }
        #endregion

        #region Methods
        public override void Init()
        {
            mouseTex = Content.Load<Texture2D>("GUI/mouse_normal");
            currentMouseState = Mouse.GetState();
            mouseOrig = new Vector2(mouseTex.Width / 2, mouseTex.Height / 2);

            inventory = Game1.Player.Inventory;
            gui = new GUI();
        }

        private void HandleMouseIcon()
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                mouseTex = Content.Load<Texture2D>("GUI/mouse_clicking");
            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                mouseTex = Content.Load<Texture2D>("GUI/mouse_normal");
        }

        public override void Update(GameTime gameTime)
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            mousePos = currentMouseState.Position.ToVector2();
            clickTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            inventory.Update(gameTime);

            HandleMouseIcon();
            HandleMouse(gameTime);
        }

        /// <summary>
        /// This method handles clicking on inventory, dragging items, etc.
        /// </summary>
        private void HandleMouse(GameTime gameTime)
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                //should exit?
                if (inventory.GetExitArea().Contains(mousePos.ToPoint()))
                {
                    this.shouldBeRemoved = true;
                }

                if (clickTimer < doubleClickTimerDelay) //double click
                {
                    pickedUpItem = inventory.GetItemByMousePosition(mousePos.ToPoint());
                    if (pickedUpItem != null)
                    {
                        if (inventory.IsEquiped(pickedUpItem))
                            inventory.UnequipItem(pickedUpItem);
                        else inventory.EquipItem(pickedUpItem);

                        pickedUpItem.BeingDragged = false;
                        pickedUpItem = null;
                    }
                }
                else //normal click
                { 
                    //pick up item
                    pickedUpItem = inventory.GetItemByMousePosition(mousePos.ToPoint());
                    if (pickedUpItem != null)
                    {
                        pickedUpItem.BeingDragged = true;
                    }
                }

                clickTimer = 0;
            }
            else if(pickedUpItem != null && currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                //equip or unequip or drop item
                
                //equipping or putting back to inventory
                Rectangle rec = inventory.GetEquipArea(pickedUpItem.ItemType);
                if (rec.Contains((mousePos - Camera.ScreenOffset).ToPoint()))
                {
                    inventory.EquipItem(pickedUpItem);
                }
                else if (inventory.IsEquiped(pickedUpItem))
                {
                    rec = inventory.GetInventoryArea();
                    if(rec.Contains((mousePos - Camera.ScreenOffset).ToPoint()))
                    {
                        inventory.UnequipItem(pickedUpItem);
                    }
                }
                pickedUpItem.BeingDragged = false;
                pickedUpItem = null;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            inventory.Draw(spriteBatch);
            Game1.Player.Spellbar.Draw(spriteBatch);
            inventory.DrawPotions(spriteBatch);
            gui.Draw(spriteBatch);

            if(pickedUpItem != null)
            {
                inventory.HighlightSlot(spriteBatch, pickedUpItem.ItemType);
            }

            //mouse
            float mouseScale = 2;
            spriteBatch.Draw(texture: mouseTex, position: -Camera.ScreenOffset + mousePos + mouseOrig * mouseScale, sourceRectangle: null, color: Color.White,
                   rotation: 0, origin: mouseOrig, scale: mouseScale, effects: SpriteEffects.None, layerDepth: Layers.AlwaysOnTop);

            //dragged item
            if(pickedUpItem != null)
            {
                Vector2 itemOrigin = new Vector2(pickedUpItem.Texture.Width / 2, pickedUpItem.Texture.Height / 2);
                float itemScale = (32.0f * inventory.InventoryScale) / (pickedUpItem.Texture.Width > pickedUpItem.Texture.Height ? pickedUpItem.Texture.Width : pickedUpItem.Texture.Height);
                spriteBatch.Draw(texture: pickedUpItem.Texture, position: -Camera.ScreenOffset + mousePos, sourceRectangle: null, color: Color.White,
                    rotation: 0, origin: itemOrigin, scale: itemScale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);
            }
        }
        #endregion
    }
}
