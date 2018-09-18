
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers.Enums;

namespace Myng.Items
{
    public class Ring :Item
    {
        public Ring(Texture2D texture) : base(texture, ItemType.TRINKET)
        {
            stats.Add(Stats.MOVEMENT_SPEED, 25);
            stats.Add(Stats.ATTACK_SPEED, 30);
        }
    }
}
