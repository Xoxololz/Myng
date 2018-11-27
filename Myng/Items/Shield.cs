
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers.Enums;

namespace Myng.Items
{
    public class Shield :Item
    {
        public Shield(Texture2D texture, ItemRarity rarity) : base(texture, ItemType.SHIELD, rarity)
        {

        }
    }
}
