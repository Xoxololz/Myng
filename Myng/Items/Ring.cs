
using Microsoft.Xna.Framework.Graphics;
using Myng.Items.Interfaces;
using Myng.Helpers.Enums;

namespace Myng.Items
{
    public class Ring :Item, IStatImprover
    {
        public Ring(Texture2D texture) : base(texture, ItemType.MISC)
        {
        }

        public void ImproveStats()
        {
            Parent.AttackSpeed *= 0.7f;
        }


        public override void UnequipItem()
        {
            Parent.AttackSpeed /= 0.7f;
        }
    }
}
