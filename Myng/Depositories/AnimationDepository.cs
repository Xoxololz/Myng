using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics.Animations;
using Myng.States;

namespace Myng.Depositories
{
    public static class AnimationDepository
    {
        #region Walking enemy chars animations

        public static Animation ZombieWalk()
        {
            return new Animation(State.Content.Load<Texture2D>("Characters/Zombie"), 4, 3);
        }

        public static Animation SkeletonWalk()
        {
            return new Animation(State.Content.Load<Texture2D>("Characters/Skeleton2"), 4, 3)
            {
                FrameSpeed = 0.15f
            };

        }

        #endregion

        #region Walking player classes chars animations

        public static Animation MageWalking()
        {
            return new Animation(State.Content.Load<Texture2D>("Characters/Mage"), 4, 3);
        }

        #endregion

        #region Projectile animations

        public static Animation FireballFlying()
        {
            return new Animation(State.Content.Load<Texture2D>("Projectiles/fireball"), 1, 6)
            {
                FrameSpeed = 0.05f
            };
        }

        public static Animation GreenArrowFlying()
        {
            return new Animation(State.Content.Load<Texture2D>("Projectiles/greenArrow"), 1, 1)
            {
                FrameSpeed = 1f
            };

        }

        public static Animation Explosion()
        {
            return new Animation(State.Content.Load<Texture2D>("Projectiles/explosion"), 1, 1)
            {
                FrameSpeed = 1f
            };
        }

        #endregion
    }
}
