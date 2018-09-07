﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers.SoundHandlers;
using System.Collections.Generic;

namespace Myng.States
{
    public abstract class State
    {
        #region Fields             

        protected GraphicsDevice graphicsDevice;

        protected Game1 game;

        protected bool shouldBeRemoved = false;

        #endregion

        #region Properties

        public static ContentManager Content;

        public List<Sound> Sounds { get; set; }

        public bool ShouldBeRemoved
        {
            get
            {
                return shouldBeRemoved;
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
