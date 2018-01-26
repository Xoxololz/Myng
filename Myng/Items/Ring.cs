
using Microsoft.Xna.Framework.Graphics;
using Myng.Items.Interfaces;

namespace Myng.Items
{
    public class Ring :Item ,IStatImprover
    {
        public Ring(Texture2D texture) : base(texture)
        {
        }

        public void ImproveStats()
        {
            Parent.AttackSpeed *= 0.7f;
        }
    }
}
