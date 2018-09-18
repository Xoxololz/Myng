
namespace Myng.Helpers.Enums
{
    public enum Attributes
    {
        STRENGTH,
        DEXTERITY,
        INTELLIGENCE,
        VITALITY,
        AURA,
        LUCK
    }

    public static class AttributesMethods
    {
        public static string GetName(this Attributes attribute)
        {
            switch (attribute)
            {
                case Attributes.STRENGTH:
                    return "Strength";
                case Attributes.DEXTERITY:
                    return "Dexterity";
                case Attributes.INTELLIGENCE:
                    return "Intelligence";
                case Attributes.VITALITY:
                    return "Vitality";
                case Attributes.AURA:
                    return "Aura";
                case Attributes.LUCK:
                    return "Luck";
                default: return "Unknown Attribute";
            }
        }
    }
}
