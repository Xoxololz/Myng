using Myng.Graphics;
using Myng.Helpers.Enums;

namespace Myng.PlayerIdentity
{
    abstract public class Identity
    {
        #region Fields
        #endregion

        #region Properties
        public abstract Attributes PrimaryAttribute
        {
            get;
        }

        public abstract int HPModifier
        {
            get;
        }

        public abstract string Name
        {
            get;
        }
        #endregion

        #region Constructors
        public Identity()
        {
        }
        #endregion

        #region Methods

        //TODO Methods for class specific spells, etc.

        #endregion
        }
}
