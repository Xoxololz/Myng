using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myng.Graphics;
using Myng.Helpers;
using Myng.Helpers.SoundHandlers;
using Myng.States;
using System.Collections.Generic;

namespace Myng
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region Fields
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private static State currentState;
        private State nextState;

        private bool isPaused;
        private Stack<State> states;

        private KeyboardState keyboardCurrent;
        private KeyboardState keyboardPrevious;
        #endregion

        #region Properties
        public static Player Player;

        public static int ScreenHeight;
        public static int ScreenWidth;

        #endregion
  
        private void ChangeState(State state)
        {
            currentState.PauseSounds();
            nextState = state;
            states.Push(state);
            isPaused = nextState is GameState ? false : true;
        }

        private void ExitCurrentState()
        {
            currentState.StopSounds();
            states.Pop();
            currentState = states.Peek();
            currentState.ResumeSounds();
            isPaused = currentState is GameState ? false : true;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = false;
            isPaused = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            //setting window height and width
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            ScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            ScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

            base.Initialize();

            //sets state to be in after starting the app
            states = new Stack<State>();
            currentState = new GameState(Content, graphics.GraphicsDevice, this);
            currentState.Init();
            states.Push(currentState);

            keyboardCurrent = Keyboard.GetState();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to switch between states and update the current state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            keyboardPrevious = keyboardCurrent;
            keyboardCurrent = Keyboard.GetState();

            //TODO -- temporary solution untill we have main menu
            if (keyboardCurrent.IsKeyDown(Keys.Escape) && !keyboardPrevious.IsKeyDown(Keys.Escape) && !isPaused)
                this.Exit();

            //Escape handler for exiting states
            if (currentState.ToRemove || (keyboardCurrent.IsKeyDown(Keys.Escape) && !keyboardPrevious.IsKeyDown(Keys.Escape) && isPaused))
            {
                ExitCurrentState();
            }

            //Inventory state handler
            if (keyboardCurrent.IsKeyDown(Keys.I) && !keyboardPrevious.IsKeyDown(Keys.I))
            {
                if (currentState is InventoryState && nextState == null)
                {
                    ExitCurrentState();
                    return;
                }
                if (nextState == null)
                {
                    if (!(currentState is GameState))
                    {
                        ExitCurrentState();
                    }
                    Mouse.SetPosition(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
                    var inventoryState = new InventoryState(Content, graphics.GraphicsDevice, this);
                    inventoryState.Init();
                    ChangeState(inventoryState);
                }
            }

            //Character state handler
            if (keyboardCurrent.IsKeyDown(Keys.C) && !keyboardPrevious.IsKeyDown(Keys.C))
            {
                if (currentState is CharacterState && nextState == null)
                {
                    ExitCurrentState();
                    return;
                }
                if (nextState == null)
                {
                    if(!(currentState is GameState))
                    {
                        ExitCurrentState();
                    }
                    Mouse.SetPosition(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
                    var characterState = new CharacterState(Content, graphics.GraphicsDevice, this);
                    characterState.Init();
                    ChangeState(characterState);
                }
            }

            if (nextState != null)
            {
                currentState = nextState;
                nextState = null;
            }
            currentState.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: Camera.Transform, sortMode: SpriteSortMode.BackToFront, blendState: BlendState.AlphaBlend);
            base.Draw(gameTime);

            currentState.Draw(gameTime, spriteBatch);
            foreach (State s in states)
            {
                if (s is GameState gamestate)
                {
                    //id ispaused is true, the background is darkened
                    gamestate.DrawBackground(gameTime, spriteBatch, isPaused);               
                }
            }

            spriteBatch.End();
        }

        public static void RegisterSound(Sound sound)
        {
            currentState.Sounds.Add(sound);
        }
    }
}
