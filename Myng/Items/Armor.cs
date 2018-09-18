
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers.Enums;

namespace Myng.Items
{
    public class Armor :Item
    {
        public Armor(Texture2D texture) : base(texture, ItemType.CHEST)
        {
            attributes.Add(Attributes.VITALITY, 4);
            attributes.Add(Attributes.AURA, 2);
            stats.Add(Stats.PHYSICAL_DEFENSE, 20);
            stats.Add(Stats.MOVEMENT_SPEED, 25);
        }
    }
}
