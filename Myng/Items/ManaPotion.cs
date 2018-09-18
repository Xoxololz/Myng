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

        public int Power { get; private set; }

        #endregion

        #region Constructor

        public ManaPotion(Texture2D texture) : base(texture, ItemType.POTION)
        {
            Power = 30;
            MaxCount = 5;
        }

        #endregion

        public void Use(List<Sprite> sprites)
        {
            if (Count > 0)
            {
                Parent.Mana += Power;
                Count--;
            }
        }
    }
}
