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
using Myng.Graphics.Animations;
using Myng.Graphics.GUI;

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

            TmxMap map = new TmxMap("Content/Maps/map3.tmx");
            TileMap = new TileMap(map);

            var monsterAnimations = new Dictionary<string, Animation>()
            {
                { "walking", new Animation(Content.Load<Texture2D>("Characters/Zombie"), 4, 3) }
            };

            var monsterAnimations2 = new Dictionary<string, Animation>()
            {
                { "walking", new Animation(Content.Load<Texture2D>("Characters/Skeleton2"), 4, 3)
                    {
                        FrameSpeed = 0.15f
                    }
                }
            };

            var fireballAnimation = new Dictionary<string, Animation>()
            {
                { "fireball", new Animation(Content.Load<Texture2D>("Projectiles/fireball"), 1, 6)

                    {
                        FrameSpeed = 0.05f
                    }
                }
            };

            var playerAnimations = new Dictionary<string, Animation>()
            {
                { "walking", new Animation(Content.Load<Texture2D>("Characters/White_Male"), 4, 3) }
            };

            Player player = new Player(playerAnimations, new Vector2(2352.014f, 724))
            {
                Bullet = new Projectile(fireballAnimation, new Vector2(100f)),
            };

            Enemy monster = new Enemy(monsterAnimations, new Vector2(3050, 700))
            {
                Bullet = new Projectile(fireballAnimation, new Vector2(100f))
            };

            //Enemy monster2 = new Enemy(monsterAnimations2, new Vector2(100))
            //{
            //    Bullet = new Projectile(fireballAnimation, new Vector2(200f))
            //};

            otherSprites = new List<Sprite>
            {
                new ItemSprite(Content.Load<Texture2D>("Items/HealthPotion"), new Vector2(100f)
                    , new HealthPotion(Content.Load<Texture2D>("Items/HealthPotion"))),

                new ItemSprite(Content.Load<Texture2D>("Items/HealthPotion"), new Vector2(250f)
                    , new HealthPotion(Content.Load<Texture2D>("Items/HealthPotion"))),

                new ItemSprite(Content.Load<Texture2D>("Items/leather_armour1"), new Vector2(700f)
                    , new Armor(Content.Load<Texture2D>("Items/leather_armour1"))),

                new ItemSprite(Content.Load<Texture2D>("Items/iron_ring"), new Vector2(3050, 750)
                    , new Ring(Content.Load<Texture2D>("Items/iron_ring"))),

                new ItemSprite(Content.Load<Texture2D>("Items/iron_ring"), new Vector2(3050, 800)
                    , new Ring(Content.Load<Texture2D>("Items/iron_ring"))),

                new ItemSprite(Content.Load<Texture2D>("Items/iron_ring"), new Vector2(3050, 850)
                    , new Ring(Content.Load<Texture2D>("Items/iron_ring"))),

                new ItemSprite(Content.Load<Texture2D>("Items/iron_ring"), new Vector2(3050, 900)
                    , new Ring(Content.Load<Texture2D>("Items/iron_ring"))),

                new ItemSprite(Content.Load<Texture2D>("Items/leather_armour1"), new Vector2(3050, 600)
                    , new Armor(Content.Load<Texture2D>("Items/leather_armour1"))),

                new ItemSprite(Content.Load<Texture2D>("Items/leather_armour1"), new Vector2(3050, 550)
                    , new Armor(Content.Load<Texture2D>("Items/leather_armour1"))),

                new ItemSprite(Content.Load<Texture2D>("Items/leather_armour1"), new Vector2(3050, 500)
                    , new Armor(Content.Load<Texture2D>("Items/leather_armour1"))),

                new ItemSprite(Content.Load<Texture2D>("Items/leather_armour1"), new Vector2(3050, 600)
                    , new Armor(Content.Load<Texture2D>("Items/leather_armour1"))),

                new ItemSprite(Content.Load<Texture2D>("Items/leather_armour1"), new Vector2(3050, 550)
                    , new Armor(Content.Load<Texture2D>("Items/leather_armour1"))),

                new ItemSprite(Content.Load<Texture2D>("Items/leather_armour1"), new Vector2(3050, 500)
                    , new Armor(Content.Load<Texture2D>("Items/leather_armour1"))),

                new ItemSprite(Content.Load<Texture2D>("Items/leather_armour1"), new Vector2(3050, 600)
                    , new Armor(Content.Load<Texture2D>("Items/leather_armour1"))),

                new ItemSprite(Content.Load<Texture2D>("Items/leather_armour1"), new Vector2(3050, 550)
                    , new Armor(Content.Load<Texture2D>("Items/leather_armour1"))),

                new ItemSprite(Content.Load<Texture2D>("Items/leather_armour1"), new Vector2(3050, 500)
                    , new Armor(Content.Load<Texture2D>("Items/leather_armour1"))),

            };

            hittableSprites = new List<Sprite>
            {
              //  monster
                //monster2
            };
            for (int i = 240; i < 1500; i += 150)
            {
                var monsterAnimations3 = new Dictionary<string, Animation>()
                {
                    { "walking", new Animation(Content.Load<Texture2D>("Characters/Zombie"), 4, 3) }
                };

                Enemy monster3 = new Enemy(monsterAnimations3, new Vector2(1, i))
                {
                    Bullet = new Projectile(fireballAnimation, new Vector2(100f))
                };

                hittableSprites.Add(monster3);
            }
            Game1.Player = player;

            camera = new Camera(Game1.Player);

            gui = new GUI();

            var songs = new List<Song>
            {
                Content.Load<Song>("Sounds/LYbeat"),
                Content.Load<Song>("Sounds/NE"),
                Content.Load<Song>("Sounds/RM")
            };
           // backgroundMusic = new BackgroundMusic(songs);
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
                    hittableSprites.RemoveAt(i);
                    i--;
                }
            }
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

        #endregion
    }
}
