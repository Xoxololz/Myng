using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using Myng.Graphics.Animations;
using Myng.Graphics.Enemies;
using Myng.Helpers;
using Myng.Helpers.Enums;
using Myng.Helpers.Spells;
using Myng.States;
using System;
using System.Collections.Generic;
using static Myng.Graphics.Character;

namespace Myng.Depositories
{
    internal static class SpellDepository
    {
        #region Enemy Spells

        public static AutoAttack RangeAutoAttack(Enemy enemy)
        {
            Action<List<Sprite>, List<Sprite>> autoAttackAction = (otherSprites, hittableprites) =>
            {
                var b = enemy.Bullet.Clone() as Projectile;
                var bPosition = enemy.GlobalOrigin - enemy.Bullet.Origin * enemy.Bullet.Scale;

                 var attackDirection= -(enemy.Position - (Game1.Player.Position));
                if (attackDirection.X < 0)
                    b.AngleTextureOffset = MathHelper.ToRadians(180);

                var imp = new Impairment(ImpairmentType.Silence, 1.5, 0);

                b.Initialize(bPosition, 30, DamageType.PHYSICAL, attackDirection, enemy.Faction,
                    SoundsDepository.FireballFlying.CreateInstance(), SoundsDepository.FireballExplosion.CreateInstance(), enemy, imp);

                otherSprites.Add(b);
            };
            Func<bool> canExecute = () =>
            {
                var AutoattackRange = (enemy.Position - Game1.Player.Position).Length() < enemy.SightRange;
                return AutoattackRange;                
            };

            return new AutoAttack(autoAttackAction, canExecute, enemy);
        }

        public static Spell TrippleGreenArrow(Enemy enemy)
        {
            var greenArrowAnimation = new Dictionary<string, Animation>()
            {
                 { "greenArrow",AnimationDepository.GreenArrowFlying()}
            };
            Action<List<Sprite>, List<Sprite>> blast = (otherSprites, hitableSprites) =>
            {

                var bulletMid = new Projectile(greenArrowAnimation, enemy.Position);
                var bPosition = enemy.GlobalOrigin - enemy.Bullet.Origin * enemy.Bullet.Scale;

                var attackDirection = -(enemy.Position - (Game1.Player.Position));
                if (attackDirection.X < 0)
                    bulletMid.AngleTextureOffset = MathHelper.ToRadians(180);

                bulletMid.Initialize(bPosition, 40, DamageType.MAGIC, attackDirection, enemy.Faction,
                    SoundsDepository.FireballFlying.CreateInstance(), SoundsDepository.FireballExplosion.CreateInstance(), enemy);
                var bulletBot = bulletMid.Clone() as Projectile;
                var bulletTop = bulletMid.Clone() as Projectile;

                var angleDiff = MathHelper.ToRadians(20);
                bulletBot.RotateDirection(-angleDiff);
                bulletTop.RotateDirection(angleDiff);

                otherSprites.Add(bulletMid);
                otherSprites.Add(bulletBot);
                otherSprites.Add(bulletTop);
            };

            return new Spell(blast, 0, 4)
            {
                Range = 250,
                CastingAnimations = greenArrowAnimation,
                CastingTime = 1.5
            };
        }

        public static Spell SuicideBombRun(Enemy enemy)
        {
            double timer = 1;
            double recalculate = 0.5;
            Action<List<Sprite>, List<Sprite>> Bum = (otherSprites, hittableSprites) =>
            {
                if (Vector2.Distance(enemy.Position, Game1.Player.Position) < 70)
                {
                    Game1.Player.Health -= 40;
                    Game1.Player.AddImpairement(new Impairment(ImpairmentType.Stun, 1, 0.5f));
                    Game1.Player.AddImpairement(new Impairment(ImpairmentType.Snare, 2, 0.5f));
                }
                var explosionAnimation = new Dictionary<string, Animation>()
                {
                     { "explosion",AnimationDepository.Explosion()}
                };
                otherSprites.Add(new AnimationSprite(explosionAnimation, enemy.Position, Layers.Collidable)
                {
                    Scale = 0.15f,
                    Origin = enemy.GlobalOrigin
                });
                enemy.Health = -1;
            };

            Action<List<Sprite>, List<Sprite>, GameTime> sprintToAction = (otherSprites, hittableSprites, gameTime) =>
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                if (timer > recalculate)
                {
                    enemy.SetGoalDestination(Game1.Player.Position);
                    timer = 0;
                }
                enemy.SpeedMultiplier = 2.5f;
                if (Vector2.Distance(enemy.Position, Game1.Player.Position) < 70)
                {
                    Bum(otherSprites, hittableSprites);
                }
            };


            return new Spell(Bum, null, 0, null, 0, sprintToAction)
            {
                Range = 500,
                CastingTime = 5
            };
        }

        #endregion

        #region Mage Spells

        public static AutoAttack RangeAutoAttack(Player player)
        {
            Action<List<Sprite>, List<Sprite>> autoAttackAction = (otherSprites, hittableprites) =>
            {
                var b = player.Bullet.Clone() as Projectile;
                var bPosition = player.GlobalOrigin - player.Bullet.Origin*player.Bullet.Scale;
                Random random = new Random();

                if (player.AttackDirection.X < 0)
                    b.AngleTextureOffset = MathHelper.ToRadians(45);
                else b.AngleTextureOffset = MathHelper.ToRadians(225);

                var imp = new Impairment(ImpairmentType.Silence, 1.5, 0.7f);

                b.Initialize(bPosition, random.Next(player.MinDamage, player.MaxDamage + 1), DamageType.PHYSICAL, player.AttackDirection, player.Faction,
                    SoundsDepository.FireballFlying.CreateInstance(), SoundsDepository.FireballExplosion.CreateInstance(), player, imp);

                otherSprites.Add(b);
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

            Action<List<Sprite>, List<Sprite>> blast = (otherSprites, hittableSprites) =>
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

                otherSprites.Add(bulletMid);
                otherSprites.Add(bulletBot);
                otherSprites.Add(bulletTop);
            };

            return new Spell(blast, 15, State.Content.Load<Texture2D>("Projectiles/fireball_icon"), 1);
        }

        #endregion
    }
}
