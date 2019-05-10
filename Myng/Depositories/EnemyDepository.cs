using Microsoft.Xna.Framework;
using Myng.Graphics;
using Myng.Graphics.Animations;
using Myng.Graphics.Enemies;
using Myng.Helpers.Enums;
using System.Collections.Generic;

namespace Myng.Depositories
{
     public static class EnemyDepository
    {
        #region Enemies

        public static Enemy Zombie(Vector2 position)
        {
            var monsterAnimations = new Dictionary<string, Animation>()
            {
                { "walking", AnimationDepository.ZombieWalk() }
            };

            var greenArrowAnimation = new Dictionary<string, Animation>()
            {
                { "greenArrow",AnimationDepository.GreenArrowFlying()}
            };

            return new Enemy(monsterAnimations, position, EnemyType.ELITE)
            {
                Bullet = new Projectile(greenArrowAnimation, position)
            };
        }

        #endregion
    }
}
