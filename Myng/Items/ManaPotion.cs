using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using Myng.Helpers.Enums;
using Myng.Items.Interfaces;
using System;
using System.Collections.Generic;

namespace Myng.Items
{
    public class ManaPotion : Item, IUsable
    {
        #region Properties

        public float Power { get; private set; }

        #endregion

        #region Constructor

        public ManaPotion(Texture2D texture) : base(texture, ItemType.POTION, ItemRarity.COMMON)
        {
            Power = 0.4f;
            MaxCount = 5;
        }

        #endregion

        public void Use(List<Sprite> sprites)
        {
            if (Count > 0)
            {
                Parent.Mana += (int)(Parent.MaxMana*Power);
                Count--;
            }
        }
    }
}
