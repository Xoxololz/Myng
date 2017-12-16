

using Myng.Graphics;

namespace Myng.Helpers
{
    //probly shoud be abstract in the future, but for now its good for testing to create Items
    public /*abstract*/ class Item
    {
        #region Properties

        public int Count { get; set; }

        public Player Parent { get; set; }

        #endregion

        #region Constructors

        public Item()
        {
            Count = 1;
        }

        #endregion

        #region Methods
        /// <summary>
        /// shoud be used in any override method:
        /// takes care of decreasing count
        /// </summary>
        public virtual void Use()
        {

            Count--;
        }

        #endregion
    }
}
