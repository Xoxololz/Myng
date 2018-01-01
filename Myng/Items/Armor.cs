
using Microsoft.Xna.Framework.Graphics;
using Myng.Items.Interfaces;

namespace Myng.Items
{
    public class Armor :Item ,IStatImprover
    {
        public Armor(Texture2D texture) : base(texture)
        {
        }

        public void ImproveStats()
        {
            Parent.Health += 50;
        }
    }
}
