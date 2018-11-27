
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers.Enums;

namespace Myng.Items
{
    public class Weapon :Item
    {
        public Weapon(Texture2D texture, ItemRarity rarity) : base(texture, ItemType.WEAPON, rarity)
        {

        }
    }
}
