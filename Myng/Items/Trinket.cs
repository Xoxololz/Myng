
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers.Enums;

namespace Myng.Items
{
    public class Trinket :Item
    {
        public Trinket(Texture2D texture, ItemRarity rarity) : base(texture, ItemType.TRINKET, rarity)
        {

        }
    }
}
