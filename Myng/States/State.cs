using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myng.Helpers.SoundHandlers;
using System.Collections.Generic;

namespace Myng.States
{
    public abstract class State
    {
        #region Fields             

        protected GraphicsDevice graphicsDevice;

        protected Game1 game;

        protected bool toRemove = false;

        #endregion

        #region Properties

        public static ContentManager Content;

        public List<Sound> Sounds { get; set; }

        public bool ToRemove
        {
            get
            {
                return toRemove;
            }
        }

        #endregion

        #region Constructors

        public State(ContentManager content, GraphicsDevice graphicsDevice, Game1 game)
        {
            State.Content = content;
            this.graphicsDevice = graphicsDevice;
            this.game = game;
            Sounds = new List<Sound>();
        }

        #endregion

        #region Methods
        public abstract void Init();

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);

        public abstract Keys GetStateInputKey();

        //method used to cleanup beofre being switched to another state
        public abstract void Exit();

        public void PauseSounds()
        {
            foreach(var sound in Sounds)
            {
                sound.Pause();
            }
        }

        public void ResumeSounds()
        {
            foreach (var sound in Sounds)
            {
                sound.Resume();
            }
        }

        public void StopSounds()
        {
            foreach (var sound in Sounds)
            {
                sound.Stop();
            }
        }

            #endregion
        }
}
