using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;

namespace Myng.Items
{
    //probly shoud be abstract in the future, but for now its good for testing to create Items
    public abstract class Item
    {
        #region Properties
        //maximum amount of the same the same item to have in inventory
        public int MaxCount
        {
            get
            {
                return 5;
            }
        }

        public int Count { get; set; }        

        public Player Parent { get; set; }

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

        #endregion

        #region Fields

        protected Texture2D texture;

        #endregion

        #region Constructors

        public Item(Texture2D texture)
        {
            this.texture = texture;
            Count = 1;
        }

        #endregion
    }
}
