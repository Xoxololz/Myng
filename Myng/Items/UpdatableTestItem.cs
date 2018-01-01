
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using Myng.Items.Interfaces;

namespace Myng.Items
{
    public class UpdatableTestItem : Item, IUpdatable
    {
        public UpdatableTestItem(Texture2D texture) : base(texture)
        {
        }

        public void Update(List<Sprite> sprites)
        {
            //Parent.Health += 10;
        }
    }
}
