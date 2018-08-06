
using Microsoft.Xna.Framework.Graphics;
using Myng.Items.Interfaces;
using Myng.Helpers.Enums;

namespace Myng.Items
{
    public class Armor :Item, IStatImprover
    {
        public Armor(Texture2D texture) : base(texture, ItemType.CHEST)
        {
        }

        public void ImproveStats()
        {
            Parent.MaxHealth += 50;
        }

        public override void UnequipItem()
        {
            Parent.MaxHealth -= 50;
        }
    }
}
