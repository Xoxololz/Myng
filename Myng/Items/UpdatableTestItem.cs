
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

        public void Update(List<Sprite> sprites)
        {
            //Parent.Health += 10;
        }

        public override void UnequipItem()
        {
            throw new NotImplementedException();
        }
    }
}
