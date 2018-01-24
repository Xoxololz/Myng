using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using TiledSharp;
using Myng.Helpers;
using Myng.Items;
using Myng.Graphics.Enemies;

namespace Myng.States
{
    public class GameState : State
    {
        #region Fields

        //sprites, that can be colided with (like characters, spell-shield, etc)
        private List<Sprite> hittableSprites;

        //sprites, that don't collide with each other (items, projectile, spells, etc)
        private List<Sprite> otherSprites;

        private TileMap tileMap;

        private Camera camera;

        #endregion

        #region Properties

        public static int ScreenHeight;
        public static int ScreenWidth;
        public static int MapHeight;
        public static int MapWidth;

        #endregion

        #region Constructor

        public GameState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game) : base(content, graphicsDevice, game)
        {
            var monsterAnimations = new Dictionary<string, Animation>()
            {
                { "walking", new Animation(content.Load<Texture2D>("./Characters/Zombie"), 4, 3) }
            };

            var monsterAnimations2 = new Dictionary<string, Animation>()
            {
                { "walking", new Animation(content.Load<Texture2D>("./Characters/Skeleton"), 4, 6) }
            };

            var fireballAnimation = new Dictionary<string, Animation>()
            {
                { "fireball", new Animation(content.Load<Texture2D>("fireball"), 1, 6)

                    {
                        FrameSpeed = 0.05f
                    }
                }
            };

            var fireballMonsterAnimation = new Dictionary<string, Animation>()
            {
                { "fireball", new Animation(content.Load<Texture2D>("fireball"), 1, 6)

                    {
                        FrameSpeed = 0.05f
                    }
                }
            };

            var playerAnimations = new Dictionary<string, Animation>()
            {
                { "walking", new Animation(Content.Load<Texture2D>("./Characters/White_Male"), 4, 3) }
            };

            Player player = new Player(playerAnimations, new Vector2(0f))
            {
                Bullet = new Projectile(fireballAnimation, new Vector2(100f))
            };

            Enemy monster = new Enemy(monsterAnimations, new Vector2(200))
            {
                Bullet = new Projectile(fireballMonsterAnimation, new Vector2(100f))
            };

            Enemy monster2 = new Enemy(monsterAnimations2, new Vector2(400))
            {
                Bullet = new Projectile(fireballMonsterAnimation, new Vector2(200f))
            };

            otherSprites = new List<Sprite>
            {
                new ItemSprite(content.Load<Texture2D>("HealthPotion"), new Vector2(1000f)
                    , new HealthPotion(content.Load<Texture2D>("HealthPotion"))),

                new ItemSprite(content.Load<Texture2D>("HealthPotion"), new Vector2(2500f)
                    , new HealthPotion(content.Load<Texture2D>("HealthPotion"))),

                new ItemSprite(content.Load<Texture2D>("projectile"), new Vector2(500f)
                    , new Armor(content.Load<Texture2D>("projectile"))),

                new ItemSprite(content.Load<Texture2D>("meta"), new Vector2(600f)
                    , new UpdatableTestItem(content.Load<Texture2D>("meta")))
            };

            hittableSprites = new List<Sprite>
            {
                monster,
                monster2
            };

            Game1.Player = player;

            TmxMap map = new TmxMap("Content/Maps/map2.tmx");            
            tileMap = new TileMap(map);

            MapHeight = tileMap.MapHeight;
            MapWidth = tileMap.MapWidth;

            ScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            ScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

            camera = new Camera(Game1.Player);

        }

        #endregion

        #region Methods

        public override void Update(GameTime gameTime)
        {
            //update Player
            Game1.Player.Update(gameTime, otherSprites, hittableSprites, tileMap.CollisionPolygons);

            //update all sprites
            foreach (var sprite in hittableSprites.ToArray())
                sprite.Update(gameTime, otherSprites, hittableSprites, tileMap.CollisionPolygons);

            foreach (var sprite in otherSprites.ToArray())
                sprite.Update(gameTime, otherSprites, hittableSprites, tileMap.CollisionPolygons);

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
            spriteBatch.Begin(transformMatrix: Camera.Transform, sortMode: SpriteSortMode.BackToFront);

            tileMap.Draw(spriteBatch);

            Game1.Player.Draw(spriteBatch);

            foreach (var sprite in otherSprites)
                sprite.Draw(spriteBatch);

            foreach (var sprite in hittableSprites)
                sprite.Draw(spriteBatch);

            spriteBatch.End();

        }

        #endregion
    }
}
