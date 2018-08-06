﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myng.Helpers.Enums;
using Myng.Items;
using Myng.Items.Interfaces;
using Myng.States;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Myng.Helpers
{
    public class Inventory
    {
        #region Fields

        private int inventorySize = 20;

        //fonts
        private SpriteFont font;

        //potiones
        private Texture2D HPtexture;
        private Vector2 HPorigin;

        private Texture2D MPtexture;
        private Vector2 MPorigin;

        //inventory background and slots
        private Texture2D inventoryBackground;
        private Vector2 inventoryBackgroundOrigin;
        private Vector2 inventoryBackgroundPos;

        private Texture2D inventorySlots;
        private Vector2 inventorySlotsOrigin;

        //item slots
        private Vector2 itemSlotOrigin;

        private Texture2D helmetSlot;
        private Texture2D chestSlot;
        private Texture2D legsSlot;
        private Texture2D miscSlot;
        private Texture2D weaponSlot;
        private Texture2D shieldSlot;

        //scaling
        private float potionsScale = 1.5f;
        private float invScale = 1.75f;

        //items
        private List<Item> Items;

        private Item Helmet;
        private Item Chest;
        private Item Legs;
        private Item Misc;
        private Item Weapon;
        private Item Shield;
        #endregion

        #region Properties
        public HealthPotion HealthPotion;

        public ManaPotion ManaPotion;

        public Vector2 InventorySlotsPos { get; private set; }
        public Vector2 HelmetSlotPos { get; private set; }
        public Vector2 ChestSlotPos { get; private set; }
        public Vector2 LegsSlotPos { get; private set; }
        public Vector2 MiscSlotPos { get; private set; }
        public Vector2 WeaponSlotPos { get; private set; }
        public Vector2 ShieldSlotPos { get; private set; }

        public Vector2 ItemSlotSize
        {
            get
            {
                return new Vector2(helmetSlot.Width, helmetSlot.Height) * invScale;
            }
        }

        public Vector2 InventorySlotsSize
        {
            get
            {
                return new Vector2(inventorySlots.Width, inventorySlots.Height) * invScale;
            }
        }

        public float InventoryScale
        {
            get
            {
                return invScale;
            }
        }

        #endregion

        #region Constructors

        public Inventory()
        {
            //textures
            HPtexture = State.Content.Load<Texture2D>("GUI/health_pot_slot");
            MPtexture = State.Content.Load<Texture2D>("GUI/mana_pot_slot");
            inventoryBackground = State.Content.Load<Texture2D>("GUI/InventoryMenu");
            inventorySlots = State.Content.Load<Texture2D>("GUI/inventory_slots");
            helmetSlot = State.Content.Load<Texture2D>("GUI/helmet_slot");
            chestSlot = State.Content.Load<Texture2D>("GUI/chest_slot");
            legsSlot = State.Content.Load<Texture2D>("GUI/legs_slot");
            miscSlot = State.Content.Load<Texture2D>("GUI/misc_slot");
            weaponSlot = State.Content.Load<Texture2D>("GUI/weapon_slot");
            shieldSlot = State.Content.Load<Texture2D>("GUI/shield_slot");

            //origins
            HPorigin = new Vector2(HPtexture.Width / 2, HPtexture.Height / 2);
            MPorigin = new Vector2(MPtexture.Width / 2, MPtexture.Height / 2);
            inventoryBackgroundOrigin = new Vector2(inventoryBackground.Width / 2, inventoryBackground.Height / 2);
            inventorySlotsOrigin = new Vector2(inventorySlots.Width / 2, inventorySlots.Height / 2);
            itemSlotOrigin = new Vector2(helmetSlot.Width / 2, helmetSlot.Height / 2);

            //fonts
            font = State.Content.Load<SpriteFont>("Fonts/Font");

            //other
            Items = new List<Item>(inventorySize);
        }

        #endregion

        #region Methods

        public bool CanBeAdded(Item item)
        {
            if(item is HealthPotion)
            {
                if(HealthPotion == null || HealthPotion.Count < HealthPotion.MaxCount)
                {
                    return true;
                }
                return false;
            }
            if (item is ManaPotion)
            {
                if (ManaPotion == null || ManaPotion.Count < ManaPotion.MaxCount)
                {
                    return true;
                }
                return false;
            }
            if (Items.Count < inventorySize)
                return true;

            return false;
        }

        /// <summary>
        /// Finds item by its index
        /// </summary>
        /// <param name="pos">index</param>
        /// <returns>Item or null, if no Item was found</returns>
        private Item GetItem(int pos)
        {
            if (pos < 0 || pos >= Items.Count)
                return null;

            return Items[pos];
        }

        private ref Item GetEquippedItemByType(ItemType type)
        {
            switch (type)
            {
                case ItemType.HELMET:
                    return ref Helmet;
                case ItemType.CHEST:
                    return ref Chest;
                case ItemType.LEGS:
                    return ref Legs;
                case ItemType.MISC:
                    return ref Misc;
                case ItemType.WEAPON:
                    return ref Weapon;
                case ItemType.SHIELD:
                    return ref Shield;
                default: throw new ArgumentException();
            }
        }

        public bool Add(Item item)
        {
            if (!CanBeAdded(item)) return false;

            if (item is HealthPotion)
            {
                if (HealthPotion == null)
                {
                    HealthPotion = new HealthPotion(null) { Parent = Game1.Player };
                }
                else ++HealthPotion.Count;
            }
            else if (item is ManaPotion)
            {
                if (ManaPotion == null)
                {
                    ManaPotion = new ManaPotion(null) { Parent = Game1.Player };
                }
                else ++ManaPotion.Count;
            }
            else
            {
                Items.Add(item);
            }
            return true;
        }

        public Item GetItemByMousePosition(Point mousePos)
        {
            mousePos -= Camera.ScreenOffset.ToPoint();
            Rectangle rec = GetInventoryArea();

            //check clicking on unequipped items
            if (rec.Contains(mousePos)) 
            {
                for (int i = 0; i < 5; ++i) //rows
                {
                    for (int j = 0; j < 4; ++j) //columns
                    {
                        Rectangle itemRec = new Rectangle(InventorySlotsPos.ToPoint() + (new Vector2(4 + 6 * j + 30 * j, 4 + 6 * i + 30 * i) * invScale).ToPoint(),
                                                          new Point((int)(32 * invScale)));
                        if(itemRec.Contains(mousePos))
                            return GetItem(4 * i + j);
                    }
                }
            }

            //check clicking on equipped item
            foreach(ItemType type in Enum.GetValues(typeof(ItemType)))
            {
                if (type == ItemType.POTION) //special case to ingore
                    continue;

                if (GetEquipArea(type).Contains(mousePos))
                    return GetEquippedItemByType(type);
            }

            return null;
        }

        public Rectangle GetEquipArea(ItemType type)
        {
            switch (type)
            {
                case ItemType.HELMET:
                    return new Rectangle(HelmetSlotPos.ToPoint(), ItemSlotSize.ToPoint());
                case ItemType.CHEST:
                    return new Rectangle(ChestSlotPos.ToPoint(), ItemSlotSize.ToPoint());
                case ItemType.LEGS:
                    return new Rectangle(LegsSlotPos.ToPoint(), ItemSlotSize.ToPoint());
                case ItemType.MISC:
                    return new Rectangle(MiscSlotPos.ToPoint(), ItemSlotSize.ToPoint());
                case ItemType.WEAPON:
                    return new Rectangle(WeaponSlotPos.ToPoint(), ItemSlotSize.ToPoint());
                case ItemType.SHIELD:
                    return new Rectangle(ShieldSlotPos.ToPoint(), ItemSlotSize.ToPoint());
                default: throw new ArgumentException(); 
            }
        }

        public Rectangle GetInventoryArea()
        {
            return new Rectangle(InventorySlotsPos.ToPoint(), InventorySlotsSize.ToPoint());
        }

        public bool EquipItem(Item itemToEquip)
        {
            if (itemToEquip == null)
                return false;

            if (IsEquiped(itemToEquip))
                return true;

            //now we can remove the item from inventory and equip it / swap it with previously equipped item
            Items.Remove(itemToEquip);

            //temporary variable is used to avoid problems with full inventory while swapping items
            Item toUnequip = GetEquippedItemByType(itemToEquip.ItemType);
            GetEquippedItemByType(itemToEquip.ItemType) = itemToEquip;
            HandleEquippingItem(itemToEquip);

            //remove bonuses from unequiped item
            if (toUnequip != null)
            {
                toUnequip.UnequipItem();
                Items.Add(toUnequip);
            }
            return true;
        }

        public bool UnequipItem(Item itemToUnequip)
        {
            //check if item exists, is equiped a inventory is not full
            if (itemToUnequip == null || !IsEquiped(itemToUnequip) || Items.Count >= inventorySize)
                return false;

            //add item to inventory
            Add(itemToUnequip);

            //unequip it
            itemToUnequip.UnequipItem();
            GetEquippedItemByType(itemToUnequip.ItemType) = null;

            return true;
        }

        private void HandleEquippingItem(Item itemToEquip)
        {
            if (itemToEquip is IStatImprover i)
            {
                i.ImproveStats();
            }

            if (itemToEquip is IUsable)
            {
                //TODO ADD THE EFFECT OF THIS ITEM TO SPELLBAR
                //NOTHING TO TEST THIS ON YET
            }
        }

        public bool IsEquiped(Item item)
        {
            switch (item.ItemType)
            {
                case ItemType.HELMET:
                    return Helmet == null ? false : Helmet.Equals(item);
                case ItemType.CHEST:
                    return Chest == null ? false : Chest.Equals(item);
                case ItemType.LEGS:
                    return Legs == null ? false : Legs.Equals(item);
                case ItemType.MISC:
                    return Misc == null? false : Misc.Equals(item);
                case ItemType.WEAPON:
                    return Weapon == null ? false : Weapon.Equals(item);
                case ItemType.SHIELD:
                    return Shield == null ? false : Shield.Equals(item);
                default: return false;
            }
        }

        private bool IsInInventory(Item itemToFind)
        {
            foreach (Item item in Items)
            {
                if (item.Equals(itemToFind)) return true;
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            inventoryBackgroundPos = -Camera.ScreenOffset + new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2) - inventoryBackgroundOrigin * invScale;
            InventorySlotsPos = inventoryBackgroundPos + new Vector2(5, 71) * invScale + new Vector2(6, 15) * invScale;
            HelmetSlotPos = inventoryBackgroundPos + new Vector2(180, 71) * invScale + new Vector2(49, 30) * invScale;
            ChestSlotPos = HelmetSlotPos + new Vector2(0, 40 + 17) * invScale;
            LegsSlotPos = ChestSlotPos + new Vector2(0, 40 + 17) * invScale;
            MiscSlotPos = HelmetSlotPos + new Vector2(40 + 5, (20 + 17) / 2) * invScale;
            WeaponSlotPos = ChestSlotPos + new Vector2(-(40 + 4), (20 + 17) / 2) * invScale;
            ShieldSlotPos = ChestSlotPos + new Vector2(40 + 5, (20 + 17) / 2) * invScale;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //background picture
            spriteBatch.Draw(texture: inventoryBackground, position: inventoryBackgroundPos + inventoryBackgroundOrigin * invScale, sourceRectangle: null, color: Color.White,
               rotation: 0, origin: inventoryBackgroundOrigin, scale: invScale, effects: SpriteEffects.None, layerDepth: Layers.InventoryBackground);

            //slots
            spriteBatch.Draw(texture: inventorySlots, position: InventorySlotsPos + inventorySlotsOrigin * invScale, sourceRectangle: null, color: Color.White,
               rotation: 0, origin: inventorySlotsOrigin, scale: invScale, effects: SpriteEffects.None, layerDepth: Layers.Inventory);

            //helmet slot
            spriteBatch.Draw(texture: helmetSlot, position: HelmetSlotPos + itemSlotOrigin * invScale, sourceRectangle: null, color: Color.White,
               rotation: 0, origin: itemSlotOrigin, scale: invScale, effects: SpriteEffects.None, layerDepth: Layers.Inventory);

            //chest slot
            spriteBatch.Draw(texture: chestSlot, position: ChestSlotPos + itemSlotOrigin * invScale, sourceRectangle: null, color: Color.White,
               rotation: 0, origin: itemSlotOrigin, scale: invScale, effects: SpriteEffects.None, layerDepth: Layers.Inventory);

            //legs slot
            spriteBatch.Draw(texture: legsSlot, position: LegsSlotPos + itemSlotOrigin * invScale, sourceRectangle: null, color: Color.White,
               rotation: 0, origin: itemSlotOrigin, scale: invScale, effects: SpriteEffects.None, layerDepth: Layers.Inventory);

            //misc slot
            spriteBatch.Draw(texture: miscSlot, position: MiscSlotPos + itemSlotOrigin * invScale, sourceRectangle: null, color: Color.White,
               rotation: 0, origin: itemSlotOrigin, scale: invScale, effects: SpriteEffects.None, layerDepth: Layers.Inventory);

            //weapon slot
            spriteBatch.Draw(texture: weaponSlot, position: WeaponSlotPos + itemSlotOrigin * invScale, sourceRectangle: null, color: Color.White,
               rotation: 0, origin: itemSlotOrigin, scale: invScale, effects: SpriteEffects.None, layerDepth: Layers.Inventory);

            //shield slot
            spriteBatch.Draw(texture: shieldSlot, position: ShieldSlotPos + itemSlotOrigin * invScale, sourceRectangle: null, color: Color.White,
               rotation: 0, origin: itemSlotOrigin, scale: invScale, effects: SpriteEffects.None, layerDepth: Layers.Inventory);

            //items in inventory
            for (int i = 0; i < 5; ++i) //rows
            {
                for (int j = 0; j < 4; ++j) //columns
                {
                    Item item = GetItem(4 * i + j);
                    if (item == null || item.BeingDragged)
                        continue;

                    Vector2 itemPosition = InventorySlotsPos + new Vector2(4 + 6 * j + 30 * j, 4 + 6 * i + 30 * i) * invScale;

                    float itemScale = (32.0f * invScale) / (item.Texture.Width > item.Texture.Height ? item.Texture.Width : item.Texture.Height);
                    Vector2 itemOrigin = new Vector2(item.Texture.Width / 2, item.Texture.Height / 2);

                    spriteBatch.Draw(texture: item.Texture, position: itemPosition + itemOrigin * itemScale, sourceRectangle: null, color: Color.White,
                        rotation: 0, origin: itemOrigin, scale: itemScale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);
                }
            }

            //equipped items
            DrawEquippedItem(Helmet, spriteBatch);
            DrawEquippedItem(Chest, spriteBatch);
            DrawEquippedItem(Legs, spriteBatch);
            DrawEquippedItem(Misc, spriteBatch);
            DrawEquippedItem(Weapon, spriteBatch);
            DrawEquippedItem(Shield, spriteBatch);
        }

        private void DrawEquippedItem(Item item, SpriteBatch spriteBatch)
        {
            if (item == null || item.BeingDragged)
                return;

            Vector2 itemPosition = new Vector2(4, 4) * invScale; //(4,4) moves the item inside the slot boundaries

            switch (item.ItemType) //get the position based on item type
            {
                case ItemType.HELMET:
                    itemPosition += HelmetSlotPos;
                    break;
                case ItemType.CHEST:
                    itemPosition += ChestSlotPos;
                    break;
                case ItemType.LEGS:
                    itemPosition += LegsSlotPos;
                    break;
                case ItemType.MISC:
                    itemPosition += MiscSlotPos;
                    break;
                case ItemType.WEAPON:
                    itemPosition += WeaponSlotPos;
                    break;
                case ItemType.SHIELD:
                    itemPosition += ShieldSlotPos;
                    break;
                default: break;
            }
            
            float itemScale = (32.0f * invScale) / (item.Texture.Width > item.Texture.Height ? item.Texture.Width : item.Texture.Height);
            Vector2 itemOrigin = new Vector2(item.Texture.Width / 2, item.Texture.Height / 2);
            spriteBatch.Draw(texture: item.Texture, position: itemPosition + itemOrigin * itemScale, sourceRectangle: null, color: Color.White,
                    rotation: 0, origin: itemOrigin, scale: itemScale, effects: SpriteEffects.None, layerDepth: Layers.InventoryItem);
        }

        //Extra method for drawing potions, since they are drawn during gameState aswell
        public void DrawPotions(SpriteBatch spriteBatch)
        {
            //HP
            Vector2 HPPosition = -Camera.ScreenOffset + new Vector2(15, 110);

            spriteBatch.Draw(texture: HPtexture, position: HPPosition + HPorigin * potionsScale, sourceRectangle: null, color: Color.White,
               rotation: 0, origin: HPorigin, scale: potionsScale, effects: SpriteEffects.None, layerDepth: Layers.Inventory);

            spriteBatch.DrawString(font, "Q", new Vector2(HPPosition.X +  4*potionsScale, HPPosition.Y + 3*potionsScale) + font.MeasureString("Q")/2*0.8f, Color.LightGoldenrodYellow, 0, font.MeasureString("Q") / 2, 0.8f, SpriteEffects.None, Layers.InventoryItem);

            spriteBatch.DrawString(font, HealthPotion == null ? 0.ToString() : HealthPotion.Count.ToString(), new Vector2(HPPosition.X + 28*potionsScale, HPPosition.Y + 24*potionsScale), Color.Black);

            //MP
            Vector2 MPPosition = -Camera.ScreenOffset + new Vector2(15 + HPtexture.Width * potionsScale + 10, 110);

            spriteBatch.Draw(texture: MPtexture, position: MPPosition + MPorigin * potionsScale, sourceRectangle: null, color: Color.White,
               rotation: 0, origin: MPorigin, scale: potionsScale, effects: SpriteEffects.None, layerDepth: Layers.Inventory);

            spriteBatch.DrawString(font, "E", new Vector2(MPPosition.X + 4 * potionsScale, MPPosition.Y + 3 * potionsScale) + font.MeasureString("E") / 2 * 0.8f, Color.LightGoldenrodYellow, 0, font.MeasureString("E") / 2, 0.8f, SpriteEffects.None, Layers.InventoryItem);

            spriteBatch.DrawString(font, ManaPotion == null ? 0.ToString() : ManaPotion.Count.ToString(), new Vector2(MPPosition.X + 28 * potionsScale, MPPosition.Y + 24 * potionsScale), Color.Black);
        }

        #endregion
    }
}
