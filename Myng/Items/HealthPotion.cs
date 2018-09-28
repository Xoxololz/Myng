
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using Myng.Helpers.Enums;
using Myng.Items.Interfaces;
using System;

namespace Myng.Items
{
    public class HealthPotion : Item, IUsable
    {
        #region Properties

        public int Power { get; private set; }

        #endregion

        #region Constructor

        public HealthPotion(Texture2D texture) : base(texture, ItemType.POTION)
        {
            Power = 40;
            MaxCount = 5;
        }

        #endregion

        public void Use(List<Sprite> sprites)
        {
            if (Count > 0)
            {
                Parent.Health += Power;
                Count--;
            }
        }
    }
}
