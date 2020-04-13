using Microsoft.Xna.Framework;
using Myng.Graphics;
using Myng.Graphics.Animations;
using Myng.PlayerIdentity;
using System.Collections.Generic;

namespace Myng.Depositories
{
    public static class PlayerIdentitiesDepository
    {
        #region Player identities

        public static Player Mage()
        {
            var fireballAnimation = new Dictionary<string, Animation>()
            {
                { "fireball", AnimationDepository.FireballFlying()}
            };


            var playerAnimations = new Dictionary<string, Animation>()
            {
                { "walking", AnimationDepository.MageWalking()}
            };

            var player = new Player(playerAnimations, new Vector2(2900, 900), new Mage())
            {
                Bullet = new Projectile(fireballAnimation, new Vector2(100f)),
            };

            player.Spellbar.Add(SpellDepository.TrippleFireball(player));
            player.AutoAttack = SpellDepository.RangeAutoAttack(player);

            return player;
        }

        #endregion
    }
}
