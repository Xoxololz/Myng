using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using TiledSharp;
using System;
using Myng.Helpers;

namespace Myng.States
{
    public class GameState : State
    {

        private List<Sprite> sprites;

        private TileMap tileMap;

        private Camera camera;


        public static int ScreenHeight;
        public static int ScreenWidth;
        public static int MapHeight;
        public static int MapWidth;

        public GameState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game) : base(content, graphicsDevice, game)
        {
            //TODO: initialize sprites?
            sprites = new List<Sprite>
            {
                new TestSprite(content.Load<Texture2D>("worm"), new Vector2(100f))
            };

            TmxMap map = new TmxMap("Content/Maps/mapa.tmx");
            Texture2D tileset = content.Load<Texture2D>(map.Tilesets[0].Name.ToString());
            tileMap = new TileMap(map,tileset);

            MapHeight = tileMap.MapHeight;
            MapWidth = tileMap.MapWidth;

            ScreenHeight = graphicsDevice.DisplayMode.Height;
            ScreenWidth = graphicsDevice.DisplayMode.Width;

            camera = new Camera(sprites[0]);

        }   

        public override void Update(GameTime gameTime)
        {
            //update all sprites
            foreach (var sprite in sprites.ToArray())
                sprite.Update(gameTime, sprites);

            camera.Focus();

            //delete sprites that are marked for removal
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i].toRemove)
                {
                    sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: camera.Transform);

            tileMap.Draw(spriteBatch);

            foreach (var sprite in sprites)
                sprite.Draw(spriteBatch);

            spriteBatch.End();

        }       
    }
}
