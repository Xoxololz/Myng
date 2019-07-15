using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using Myng.Graphics.Animations;
using Myng.Graphics.Enemies;
using Myng.Helpers.Enums;
using Myng.Helpers.Spells;
using Myng.States;
using System;
using System.Collections.Generic;

namespace Myng.Depositories
{
    public static class SpellDepository
    {
        #region Enemy Spells

        public static AutoAttack RangeAutoAttack(Enemy enemy)
        {
            Action<List<Sprite>> autoAttackAction = (sprites) =>
            {
                var b = enemy.Bullet.Clone() as Projectile;
                var bPosition = enemy.GlobalOrigin - enemy.Bullet.Origin * enemy.Bullet.Scale;

                 var attackDirection= -(enemy.Position - (Game1.Player.Position));
                if (attackDirection.X < 0)
                    b.AngleTextureOffset = MathHelper.ToRadians(180);

                b.Initialize(bPosition, 30, DamageType.PHYSICAL, attackDirection, enemy.Faction,
                    SoundsDepository.FireballFlying.CreateInstance(), SoundsDepository.FireballExplosion.CreateInstance(), enemy);

                sprites.Add(b);
            };
            Func<bool> canExecute = () =>
            {
                var AutoattackRange = (enemy.Position - Game1.Player.Position).Length() < enemy.SightRange;
                return AutoattackRange;                
            };

            return new AutoAttack(autoAttackAction, canExecute, enemy);
        }

        #endregion

        #region Mage Spells

        public static AutoAttack RangeAutoAttack(Player player)
        {
            Action<List<Sprite>> autoAttackAction = (sprites) =>
            {
                var b = player.Bullet.Clone() as Projectile;
                var bPosition = player.GlobalOrigin - player.Bullet.Origin*player.Bullet.Scale;
                Random random = new Random();

                if (player.AttackDirection.X < 0)
                    b.AngleTextureOffset = MathHelper.ToRadians(45);
                else b.AngleTextureOffset = MathHelper.ToRadians(225);

                b.Initialize(bPosition, random.Next(player.MinDamage, player.MaxDamage + 1), DamageType.PHYSICAL, player.AttackDirection, player.Faction,
                    SoundsDepository.FireballFlying.CreateInstance(), SoundsDepository.FireballExplosion.CreateInstance(), player);

                sprites.Add(b);
            };

            return new AutoAttack(autoAttackAction, null, player);
        }

        public static Spell TrippleFireball(Player player)
        {
            var fireballAnimation = new Dictionary<string, Animation>()
            {
                { "fireball", new Animation(State.Content.Load<Texture2D>("Projectiles/fireball"), 1, 6)

                    {
                        FrameSpeed = 0.05f
                    }
                }
            };
            var bullet = new Projectile(fireballAnimation, new Vector2(100f));

            Action<List<Sprite>> blast = (sprites) =>
            {
                var bulletMid = bullet.Clone() as Projectile;
                var bPosition = player.GlobalOrigin - player.Bullet.Origin * player.Bullet.Scale;

                Vector2 dir;
                if (player.FacingDirection == Direction.Down)
                    dir = new Vector2(0, 1);
                else if (player.FacingDirection == Direction.Left)
                    dir = new Vector2(-1, 0);
                else if (player.FacingDirection == Direction.Right)
                    dir = new Vector2(1, 0);
                else if (player.FacingDirection == Direction.Up)
                    dir = new Vector2(0, -1);
                else
                    dir = new Vector2(1, 0);

                if (dir.X < 0)
                    bulletMid.AngleTextureOffset = MathHelper.ToRadians(45);
                else bulletMid.AngleTextureOffset = MathHelper.ToRadians(225);
 

                bulletMid.Initialize(bPosition, (int)(player.GetAttribute(Attributes.INTELLIGENCE)*1.5f), DamageType.MAGIC, dir, player.Faction,
                    SoundsDepository.FireballFlying.CreateInstance(), SoundsDepository.FireballExplosion.CreateInstance(), player);
                var bulletBot = bulletMid.Clone() as Projectile;
                var bulletTop = bulletMid.Clone() as Projectile;

                var angleDiff = MathHelper.ToRadians(20);
                bulletBot.RotateDirection(-angleDiff);
                bulletTop.RotateDirection(angleDiff);

                sprites.Add(bulletMid);
                sprites.Add(bulletBot);
                sprites.Add(bulletTop);
            };

            return new Spell(blast, 15, State.Content.Load<Texture2D>("Projectiles/fireball_icon"), 1);
        }

        #endregion
    }
}
