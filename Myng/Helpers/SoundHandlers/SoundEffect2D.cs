using Microsoft.Xna.Framework.Audio;
using Myng.Graphics;
using Microsoft.Xna.Framework;

namespace Myng.Helpers.SoundHandlers
{
    public class SoundEffect2D : Sound
    {
        #region Fields

        private SoundEffectInstance soundEffect;

        private Sprite Parent;

        private AudioListener audioListener
        {
            get
            {
                var listener = new AudioListener
                {
                    Position = new Vector3(Game1.Player.GlobalOrigin, 0f)/ DistanceDivider
                };
                return listener;
            }
        }

        private AudioEmitter audioEmitter
        {
            get
            {
                var emitter = new AudioEmitter
                {
                    Position = new Vector3(Parent.GlobalOrigin, 0f)/ DistanceDivider
                };
                return emitter;
            }
        }

        #endregion

        #region Properties

        public float Volume
        {
            set
            {
                if(IsInVolumeRange(value))
                    soundEffect.Volume = value;
            }
        }

        public bool IsLooping
        {
            set
            {
                soundEffect.IsLooped = value;
            }
        }

        public SoundState State
        {
            get
            {
                return soundEffect.State;
            }
        }

        public int DistanceDivider { get; set; }

        #endregion

        #region Constructor

        public SoundEffect2D(SoundEffectInstance soundEffect, Sprite parent)
        {
            this.soundEffect = soundEffect;
            Parent = parent;
            DistanceDivider = 50;
        }

        #endregion

        #region Methods

        private bool IsInVolumeRange(float value)
        {
            return value < 1f || value > 0f;
        }

        public override void Play()
        {
            soundEffect.Apply3D(audioListener, audioEmitter);
            soundEffect.Play();
        }

        public void Update3DEffect()
        {
            soundEffect.Apply3D(audioListener, audioEmitter);
        }

        public override void Stop()
        {
            soundEffect.Stop();
        }

        public override void Pause()
        {
            soundEffect.Pause();
        }

        public override void Resume()
        {
            if (soundEffect.State == SoundState.Paused)
                soundEffect.Resume();
        }
        #endregion
    }
}
