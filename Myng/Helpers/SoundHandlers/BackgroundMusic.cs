using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;

namespace Myng.Helpers.SoundHandlers
{
    public class BackgroundMusic : Sound
    {
        #region Fields

        private List<Song> songs;

        private int playingSongId = 0;

        #endregion

        #region Properties

        public float Volume
        {
            set
            {
                if (IsInVolumeRange(value))
                    MediaPlayer.Volume = value;
            }
        }        

        #endregion

        #region Constructor

        public BackgroundMusic(List<Song> songs)
        {
            this.songs = songs;

            Volume = 0.2f;
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(songs[0]);
            MediaPlayer.MediaStateChanged  += NextSong;
        }        

        #endregion

        #region Methods
        private bool IsInVolumeRange(float value)
        {
            return value < 1f || value > 0f;
        }

        private void NextSong(object sender, EventArgs e)
        {
            if (MediaPlayer.State == MediaState.Playing || MediaPlayer.State == MediaState.Paused) return;

            if (++playingSongId < songs.Count)
            {
                MediaPlayer.Play(songs[playingSongId]);
            }
            else
            {
                MediaPlayer.Play(songs[0]);
                playingSongId = 0;
            }
        }

        public override void Pause()
        {
            MediaPlayer.Pause();
        }

        public override void Resume()
        {
            MediaPlayer.Resume();
        }

        public override void Stop()
        {
            MediaPlayer.Stop();
        }

        public override void Play()
        {
            MediaPlayer.Play(songs[0]);
        }

        #endregion
    }
}
