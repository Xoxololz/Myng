using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
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
            //helmets
            List<Texture2D> commonHelmets = LoadListContent("Items/mage/helmet/common");
            List<Texture2D> rareHelmets = LoadListContent("Items/mage/helmet/rare");
            List<Texture2D> epicHelmets = LoadListContent("Items/mage/helmet/epic");
            List<Texture2D> legendaryHelmets = LoadListContent("Items/mage/helmet/legendary");
            helmets.Add(ItemRarity.COMMON, commonHelmets);
            helmets.Add(ItemRarity.RARE, rareHelmets);
            helmets.Add(ItemRarity.EPIC, epicHelmets);
            helmets.Add(ItemRarity.LEGENDARY, legendaryHelmets);

            //chests
            List<Texture2D> commonChests = LoadListContent("Items/mage/armor/common");
            List<Texture2D> rareChests = LoadListContent("Items/mage/armor/rare");
            List<Texture2D> epicChests = LoadListContent("Items/mage/armor/epic");
            List<Texture2D> legendaryChests = LoadListContent("Items/mage/armor/legendary");
            armors.Add(ItemRarity.COMMON, commonChests);
            armors.Add(ItemRarity.RARE, rareChests);
            armors.Add(ItemRarity.EPIC, epicChests);
            armors.Add(ItemRarity.LEGENDARY, legendaryChests);

            //legs
            List<Texture2D> commonLegs = LoadListContent("Items/mage/boots/common");
            List<Texture2D> rareLegs = LoadListContent("Items/mage/boots/rare");
            List<Texture2D> epicLegs = LoadListContent("Items/mage/boots/epic");
            List<Texture2D> legendaryLegs = LoadListContent("Items/mage/boots/legendary");
            legs.Add(ItemRarity.COMMON, commonLegs);
            legs.Add(ItemRarity.RARE, rareLegs);
            legs.Add(ItemRarity.EPIC, epicLegs);
            legs.Add(ItemRarity.LEGENDARY, legendaryLegs);

            //weapons
            List<Texture2D> commonWeapons = LoadListContent("Items/mage/weapons/common");
            List<Texture2D> rareWeapons = LoadListContent("Items/mage/weapons/rare");
            List<Texture2D> epicWeapons = LoadListContent("Items/mage/weapons/epic");
            List<Texture2D> legendaryWeapons = LoadListContent("Items/mage/weapons/legendary");
            weapons.Add(ItemRarity.COMMON, commonWeapons);
            weapons.Add(ItemRarity.RARE, rareWeapons);
            weapons.Add(ItemRarity.EPIC, epicWeapons);
            weapons.Add(ItemRarity.LEGENDARY, legendaryWeapons);

            //shields
            List<Texture2D> commonShields = LoadListContent("Items/mage/shields/common");
            List<Texture2D> rareShields = LoadListContent("Items/mage/shields/rare");
            List<Texture2D> epicShields = LoadListContent("Items/mage/shields/epic");
            List<Texture2D> legendaryShields = LoadListContent("Items/mage/shields/legendary");
            shields.Add(ItemRarity.COMMON, commonShields);
            shields.Add(ItemRarity.RARE, rareShields);
            shields.Add(ItemRarity.EPIC, epicShields);
            shields.Add(ItemRarity.LEGENDARY, legendaryShields);
        }
        #endregion

        #region Methods

        //TODO Methods for class specific spells, etc.

        #endregion
    }
}
