using System;
using Myng.Graphics;
using Myng.Helpers.Enums;

namespace Myng.PlayerIdentity
{
    public class Mage : Identity
    {
        #region Properties
        public override Attributes PrimaryAttribute
        {
            get
            {
                return Attributes.INTELLIGENCE;
            }
        }

        public override int HPModifier
        {
            get
            {
                return 5;
            }
        }

        public override string Name
        {
            get
            {
                return "Mage";
            }
        }

        #endregion

        #region Constructors
        public Mage() : base()
        {
        }
        #endregion

        #region Methods

        //TODO Methods for class specific spells, etc.

        #endregion
    }
}
