using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using Myng.Helpers.Enums;

namespace Myng.Items
{
    //probly shoud be abstract in the future, but for now its good for testing to create Items
    public abstract class Item
    {
        #region Properties
        public ItemType ItemType { get; set; }

        //maximum amount of the same the same item to have in inventory
        public int MaxCount
        {
            get
            {
                return maxCount;
            }

            set
            {
                maxCount = value;
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

        public bool BeingDragged { get; set; }

        #endregion

        #region Fields

        protected Texture2D texture;

        protected int maxCount = 1;

        #endregion

        #region Constructors

        public Item(Texture2D texture, ItemType itemType)
        {
            this.texture = texture;
            this.ItemType = itemType;
            this.BeingDragged = false;
            Count = 1;
        }

        public abstract void UnequipItem();
        #endregion
    }
}
