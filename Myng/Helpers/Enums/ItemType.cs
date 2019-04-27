
namespace Myng.Helpers.Enums
{
    public enum ItemType
    {
        WEAPON = 0,
        SHIELD = 1,
        HELMET = 2,
        CHEST = 3,
        LEGS = 4,
        TRINKET = 5,
        POTION = 6
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
                    return "Boots";
                case ItemType.HELMET:
                    return "Helmet";
                case ItemType.CHEST:
                    return "Chest Armor"; 
                default: return "Unknown type";
            }
        }
    }
}
