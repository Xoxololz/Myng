
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using Myng.Helpers.Enums;
using Myng.Items.Interfaces;

namespace Myng.Items
{
    public class UpdatableTestItem : Item, IUpdatable
    {
        public UpdatableTestItem(Texture2D texture) : base(texture, ItemType.LEGS)
        {
        }

        public void Update(List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            throw new NotImplementedException();
        }
    }
}
