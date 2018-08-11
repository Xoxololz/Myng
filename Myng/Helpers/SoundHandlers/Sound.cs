
namespace Myng.Helpers.SoundHandlers
{
    public abstract class Sound
    {
        public Sound()
        {
            Game1.RegisterSound(this);
        }

        public abstract void Play();

        public abstract void Stop();

        public abstract void Pause();

        public abstract void Resume();
    }
}
