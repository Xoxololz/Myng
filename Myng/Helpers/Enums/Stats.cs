
namespace Myng.Helpers.Enums
{
    public enum Stats
    {
        CRIT = 0,
        ATTACK_SPEED = 1,
        MOVEMENT_SPEED = 2,
        BLOCK = 3,
        MAGIC_DEFENSE = 4,
        PHYSICAL_DEFENSE = 5,
        MAX_DAMAGE = 6,
        MIN_DAMAGE = 7
    }

    public static class StatsMethods
    {
        public static string GetName(this Stats stat)
        {
            switch (stat)
            {
                case Stats.ATTACK_SPEED:
                    return "Attack Speed";
                case Stats.BLOCK:
                    return "Block";
                case Stats.CRIT:
                    return "Critical chance";
                case Stats.MAGIC_DEFENSE:
                    return "Magic defense";
                case Stats.MAX_DAMAGE:
                    return "Max damage";
                case Stats.MIN_DAMAGE:
                    return "Min damage";
                case Stats.MOVEMENT_SPEED:
                    return "Movement speed";
                case Stats.PHYSICAL_DEFENSE:
                    return "Physical defense";
                default: return "Unknown stat";
            }
        }

        public static bool UsesPercentage(this Stats stat)
        {
            switch (stat)
            {
                case Stats.ATTACK_SPEED:
                    return true;
                case Stats.BLOCK:
                    return true;
                case Stats.CRIT:
                    return true;
                case Stats.MAGIC_DEFENSE:
                    return false;
                case Stats.MAX_DAMAGE:
                    return false;
                case Stats.MIN_DAMAGE:
                    return false;
                case Stats.MOVEMENT_SPEED:
                    return true;
                case Stats.PHYSICAL_DEFENSE:
                    return false;
                default: return false;
            }
        }
    }
}
