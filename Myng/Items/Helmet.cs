
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers.Enums;

namespace Myng.Items
{
    public class Helmet :Item
    {
        public Helmet(Texture2D texture, ItemRarity rarity) : base(texture, ItemType.HELMET, rarity)
        {

        }
    }
}
