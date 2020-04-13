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

            var enemy = new Enemy(monsterAnimations, position, EnemyType.ELITE)
            {
                Bullet = new Projectile(greenArrowAnimation, position),
            };

            enemy.AutoAttack = SpellDepository.RangeAutoAttack(enemy);
            enemy.Spells.Add(SpellDepository.TrippleGreenArrow(enemy));

            return enemy;
        }

        public static Enemy SuicideBomberSkeleton(Vector2 position)
        {
            var walkingAnimations = new Dictionary<string, Animation>()
             {
                 { "walking", AnimationDepository.SkeletonWalk() }
             };

            var greenArrowAnimation = new Dictionary<string, Animation>()
             {
                 { "greenArrow",AnimationDepository.GreenArrowFlying()}
             };

            var enemy = new Enemy(walkingAnimations, position, EnemyType.ELITE);

            enemy.Spells.Add(SpellDepository.SuicideBombRun(enemy));

            return enemy;
        }

        #endregion
    }
}
