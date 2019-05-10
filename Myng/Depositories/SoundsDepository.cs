using Microsoft.Xna.Framework.Audio;
using Myng.States;

namespace Myng.Depositories
{
    public static class SoundsDepository
    {
        public static SoundEffect FireballFlying = State.Content.Load<SoundEffect>("Sounds/fireballFlying");

        public static SoundEffect FireballExplosion = State.Content.Load<SoundEffect>("Sounds/explosion");

        public static SoundEffect Walking = State.Content.Load<SoundEffect>("Sounds/walking");
    }
}
