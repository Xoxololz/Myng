namespace Myng.Helpers.Enums
{
    public enum ItemRarity
    {
        COMMON,
        RARE,
        EPIC,
        LEGENDARY
    }

    public static class ItemRarityMethods
    {
        public static string GetName(this ItemRarity itemRarity)
        {
            switch (itemRarity)
            {
                case ItemRarity.COMMON:
                    return "Common";
                case ItemRarity.RARE:
                    return "Rare";
                case ItemRarity.EPIC:
                    return "Epic";
                case ItemRarity.LEGENDARY:
                    return "Legendary";
                default: return "Unknown rarity";
            }
        }
    }
}
