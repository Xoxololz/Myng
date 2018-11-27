﻿
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers.Enums;

namespace Myng.Items
{
    public class Armor :Item
    {
        public Armor(Texture2D texture, ItemRarity rarity) : base(texture, ItemType.CHEST, rarity)
        {

        }
    }
}
