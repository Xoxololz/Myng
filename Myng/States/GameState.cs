using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using TiledSharp;
using Myng.Helpers;
using Myng.Items;
using Myng.Graphics.Enemies;
using Myng.Helpers.SoundHandlers;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Myng.AI.Movement;
using Myng.Graphics.GUI;
using Myng.Helpers.Enums;
using Myng.Items.Interfaces;
using System;
using Myng.Depositories;
using Microsoft.Xna.Framework.Input;
using Myng.Helpers.Map;

namespace Myng.States
{
    public class GameState : State
    {
        #region Fields

        private SoundEffect soundEffect;

        //sprites, that can be colided with (like characters, spell-shield, etc)
        private List<Sprite> hittableSprites;

        //sprites, that don't collide with each other (items, projectile, spells, etc)
        private List<Sprite> otherSprites;        

        private Camera camera;

        private GUI gui;

        private BackgroundMusic backgroundMusic;

        private Texture2D blackBackground;

        private IItemFactory itemFactory;
        #endregion

        #region Properties

        public static TileMap TileMap { get; private set; }

        #endregion

        #region Constructor

        public GameState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game) : base(content, graphicsDevice, game)
        {
            
        }

        #endregion

        #region Methods

        public override void Init()
        {
            Color[] backgroundColor = new Color[Game1.ScreenWidth * Game1.ScreenHeight];
            for (int i = 0; i < backgroundColor.Length; ++i) backgroundColor[i] = Color.Black;
            blackBackground = new Texture2D(graphicsDevice, Game1.ScreenWidth, Game1.ScreenHeight);
            blackBackground.SetData(backgroundColor);

            TmxMap map = new TmxMap("Content/Maps/map_dungeon.tmx");
            TileMap = new TileMap(map);

            itemFactory = new ItemFactoryImpl();

            Game1.Player = PlayerIdentitiesDepository.Mage();
            Game1.Player.Position = new Vector2(2244, 1205);
            itemFactory.SetPlayer(Game1.Player);

            otherSprites = new List<Sprite>
            {
                itemFactory.CreateItemSprite(new Vector2(250f), itemFactory.CreateHealthPotion())
            };

            Game1.Player.Inventory.EquipItem(itemFactory.CreateRandomWeapon(ItemRarity.LEGENDARY));

            hittableSprites = new List<Sprite>();

//            for (int i = 240; i < 1500; i += 150)
//            {
//                Enemy monster3 = EnemyDepository.Zombie(new Vector2(1, i));
//                hittableSprites.Add(monster3);
//            }

            camera = new Camera(Game1.Player);

            gui = new GUI();

            var songs = new List<Song>
            {
                Content.Load<Song>("Sounds/LYbeat"),
                Content.Load<Song>("Sounds/NE"),
                Content.Load<Song>("Sounds/RM")
            };
            //backgroundMusic = new BackgroundMusic(songs);
        }

        public override void Update(GameTime gameTime)
        {
            //update Player
            Game1.Player.Update(gameTime, otherSprites, hittableSprites);
            NodeMapRepository.Update(hittableSprites);

            //update all sprites
            foreach (var sprite in hittableSprites.ToArray())
                sprite.Update(gameTime, otherSprites, hittableSprites);

            foreach (var sprite in otherSprites.ToArray())
                sprite.Update(gameTime, otherSprites, hittableSprites);

            camera.Focus();

            //delete sprites that are marked for removal
            for (int i = 0; i < otherSprites.Count; i++)
            {
                if (otherSprites[i].ToRemove)
                {
                    otherSprites.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < hittableSprites.Count; i++)
            {
                if (hittableSprites[i].ToRemove)
                {
                    if (hittableSprites[i] is Enemy e)
                        GenerateDrop(e);
                    hittableSprites.RemoveAt(i);
                    i--;
                }
            }
            gui.Update(gameTime);
        }

        private void GenerateDrop(Enemy e)
        {
            Random random = new Random();
            int dropRange = 55;
            switch (e.EnemyType)
            {
                case EnemyType.EASY:
                    if (random.NextDouble() < 0.8) break;
                    double r = random.NextDouble();
                    if (r < 0.5)
                        otherSprites.Add(itemFactory.CreateItemSprite(e.GlobalOrigin, itemFactory.CreateHealthPotion()));
                    else if (r < 0.8)
                        otherSprites.Add(itemFactory.CreateItemSprite(e.GlobalOrigin, itemFactory.CreateManaPotion()));
                    else otherSprites.Add(itemFactory.CreateItemSprite(e.GlobalOrigin, itemFactory.CreateRandomItem()));
                    break;
                case EnemyType.NORMAL:
                    if (random.NextDouble() < 0.7) break;
                    r = random.NextDouble();
                    if (r < 0.5)
                        otherSprites.Add(itemFactory.CreateItemSprite(e.GlobalOrigin, itemFactory.CreateHealthPotion()));
                    else if (r < 0.8)
                        otherSprites.Add(itemFactory.CreateItemSprite(e.GlobalOrigin, itemFactory.CreateManaPotion()));
                    else otherSprites.Add(itemFactory.CreateItemSprite(e.GlobalOrigin, itemFactory.CreateRandomItem()));
                    break;
                case EnemyType.ELITE:
                    double potsAmount = random.Next(1, 3);//up to 2 potions
                    for (int i = 0; i < potsAmount; i++)
                    {
                        r = random.NextDouble();
                        if (r < 0.6)
                            otherSprites.Add(itemFactory.CreateItemSprite(e.GlobalOrigin + new Vector2(random.Next(-dropRange, dropRange), random.Next(-dropRange, dropRange))
                                ,itemFactory.CreateHealthPotion()));
                        else otherSprites.Add(itemFactory.CreateItemSprite(e.GlobalOrigin + new Vector2(random.Next(-dropRange, dropRange), random.Next(-dropRange, dropRange))
                                ,itemFactory.CreateManaPotion()));
                    }

                    double itemsAmount = random.Next(1, 4);//up to 3 items
                    for (int i = 0; i < itemsAmount; i++)
                    {
                        //5% better chances for rare+ items
                        r = random.NextDouble();
                        ItemRarity rarity = ItemRarity.COMMON;
                        if (r > 0.9) rarity = ItemRarity.LEGENDARY;
                        else if (r > 0.7) rarity = ItemRarity.EPIC;
                        else if (r > 0.35) rarity = ItemRarity.RARE;
                        otherSprites.Add(itemFactory.CreateItemSprite(e.GlobalOrigin + new Vector2(random.Next(-dropRange, dropRange), random.Next(-dropRange, dropRange)), itemFactory.CreateRandomItem(rarity)));
                    }
                    break;
                case EnemyType.BOSS:
                    potsAmount = random.Next(2, 5);//up to 4 potions
                    for (int i = 0; i < potsAmount; i++)
                    {
                        r = random.NextDouble();
                        if (r < 0.6)
                            otherSprites.Add(itemFactory.CreateItemSprite(e.GlobalOrigin + new Vector2(random.Next(-dropRange, dropRange), random.Next(-dropRange, dropRange))
                                ,itemFactory.CreateHealthPotion()));
                        else otherSprites.Add(itemFactory.CreateItemSprite(e.GlobalOrigin + new Vector2(random.Next(-dropRange, dropRange), random.Next(-dropRange, dropRange))
                                ,itemFactory.CreateManaPotion()));
                    }

                    //guaranteed legendary item
                    otherSprites.Add(itemFactory.CreateItemSprite(e.GlobalOrigin, itemFactory.CreateRandomItem(ItemRarity.LEGENDARY)));
                    itemsAmount = random.Next(2, 6);//between 2 and 5 items RARE or better
                    for (int i = 0; i < itemsAmount; i++)
                    {
                        r = random.NextDouble();
                        ItemRarity rarity = ItemRarity.RARE;
                        if (r > 0.8) rarity = ItemRarity.LEGENDARY;
                        else if (r > 0.45) rarity = ItemRarity.EPIC;
                        otherSprites.Add(itemFactory.CreateItemSprite(e.GlobalOrigin + new Vector2(random.Next(-dropRange, dropRange), random.Next(-dropRange, dropRange))
                            ,itemFactory.CreateRandomItem(rarity)));
                    }
                    break;

            }
        }

        public override void Exit()
        {
            //do nothing (atleast for now)
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Game1.Player.Draw(spriteBatch);
            Game1.Player.Spellbar.Draw(spriteBatch);
            Game1.Player.Inventory.DrawPotions(spriteBatch);

            foreach (var sprite in otherSprites)
                sprite.Draw(spriteBatch);

            foreach (var sprite in hittableSprites)
                sprite.Draw(spriteBatch);

            gui.Draw(spriteBatch);
        }

        /// <summary>
        /// Method to draw Background. This method is called even when this state is paused, but still active (ie. during menu states).
        /// </summary>
        public void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, bool darkenBackground)
        {
            if (darkenBackground)
            {
                spriteBatch.Draw(texture: blackBackground, position: -Camera.ScreenOffset, color: Color.White*0.5f, effects: SpriteEffects.None, layerDepth: Layers.BackgroundTint);
            }
            TileMap.Draw(spriteBatch);
        }

        public override Keys GetStateInputKey()
        {
            return Keys.None;
        }
        #endregion
    }
}
