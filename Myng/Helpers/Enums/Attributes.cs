

namespace Myng.Helpers.Enums
{
    public enum Attributes
    {
        STRENGTH = 0,
        DEXTERITY = 1,
        INTELLIGENCE = 2,
        VITALITY = 3,
        AURA = 4,
        LUCK = 5
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

        public static string GetDescription(this Attributes attribute)
        {
            switch (attribute)
            {
                case Attributes.STRENGTH:
                    return "Warrior damage stat\nincreases " + Stats.PHYSICAL_DEFENSE.GetName();
                case Attributes.DEXTERITY:
                    return "Assassin damage stat\nincreases " + Stats.ATTACK_SPEED.GetName();
                case Attributes.INTELLIGENCE:
                    return "Mage damage stat\nincreases " + Stats.MAGIC_DEFENSE.GetName();
                case Attributes.VITALITY:
                    return "Increases max HP by " + Game1.Player.Identity.HPModifier + " per point";
                case Attributes.AURA:
                    return "Increases max MP by " + Game1.Player.ManaModifier +  " per point";
                case Attributes.LUCK:
                    return "Increases critical hit chance";
                default: return "no description";
            }
        }
    }
}
