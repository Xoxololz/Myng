using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using TiledSharp;
using Myng.Helpers;

namespace Myng.States
{
    public class GameState : State
    {
        #region Fields

        private List<Sprite> sprites;

        private TileMap tileMap;

        private Camera camera;

        #endregion

        #region Variables

        public static int ScreenHeight;
        public static int ScreenWidth;
        public static int MapHeight;
        public static int MapWidth;

        #endregion

        #region Constructor

        public GameState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game) : base(content, graphicsDevice, game)
        {
            var playerAnimations = new Dictionary<string, Animation>()
            {
                { "walking", new Animation(content.Load<Texture2D>("White_Male"), 4, 3) }
            };

            var fireballAnimation = new Dictionary<string, Animation>()
            {
                { "fireball", new Animation(content.Load<Texture2D>("fireball"), 1, 6)

                    {
                        FrameSpeed = 0.05f
                    }
                }
            };

            Player player = new Player(playerAnimations, new Vector2(0f))
            {
                Bullet = new Projectile(fireballAnimation, new Vector2(100f))
            };

            sprites = new List<Sprite>
            {
                player,

                new ItemSprite(content.Load<Texture2D>("projectile"), new Vector2(1000f)),

                new ItemSprite(content.Load<Texture2D>("projectile"), new Vector2(500f))
            };

            TmxMap map = new TmxMap("Content/Maps/mapa.tmx");
            Texture2D tileset = content.Load<Texture2D>(map.Tilesets[0].Name.ToString());
            tileMap = new TileMap(map, tileset);

            MapHeight = tileMap.MapHeight;
            MapWidth = tileMap.MapWidth;

            ScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            ScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

            camera = new Camera(sprites[0]);

        }

        #endregion

        #region Methods

        public override void Update(GameTime gameTime)
        {
            //update all sprites
            foreach (var sprite in sprites.ToArray())
                sprite.Update(gameTime, sprites);

            camera.Focus();

            var type = sprites[0].GetType();

            //delete sprites that are marked for removal
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i].ToRemove)
                {
                    sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: Camera.Transform);

            tileMap.Draw(spriteBatch);

            foreach (var sprite in sprites)
                sprite.Draw(spriteBatch);

            spriteBatch.End();

        }

        #endregion
    }
}
