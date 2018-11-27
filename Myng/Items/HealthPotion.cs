
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

        public float Power { get; private set; }

        #endregion

        #region Constructor

        public HealthPotion(Texture2D texture) : base(texture, ItemType.POTION, ItemRarity.COMMON)
        {
            Power = 0.4f;
            MaxCount = 5;
        }

        #endregion

        public void Use(List<Sprite> sprites)
        {
            if (Count > 0)
            {
                Parent.Health += (int)(Parent.MaxHealth*Power);
                Count--;
            }
        }
    }
}
