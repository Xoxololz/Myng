
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using Myng.Items.Interfaces;

namespace Myng.Items
{
    public class HealthPotion : Item, IUsable
    {
        #region Properties

        public int Power { get; private set; }

        #endregion

        #region Constructor

        public HealthPotion(Texture2D texture) : base(texture)
        {
            Power = 40;
        }

        #endregion

        public void Use(List<Sprite> sprites)
        {
            Parent.Health += Power;
            Count--;
        }
    }
}
