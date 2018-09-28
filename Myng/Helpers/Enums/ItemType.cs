using System;

namespace Myng.Helpers.Enums
{
    public enum ItemType
    {
        WEAPON,
        SHIELD,
        HELMET,
        CHEST,
        LEGS,
        TRINKET,
        POTION
    }

    public static class ItemTypeMethods
    {
        public static string GetName(this ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.WEAPON:
                    return "Weapon";
                case ItemType.SHIELD:
                    return "Shield";
                case ItemType.POTION:
                    return "Potion";
                case ItemType.TRINKET:
                    return "Trinket";
                case ItemType.LEGS:
                    return "Leggings";
                case ItemType.HELMET:
                    return "Helmet";
                case ItemType.CHEST:
                    return "Chest Armor"; 
                default: return "Unknown stat";
            }
        }
    }
}
