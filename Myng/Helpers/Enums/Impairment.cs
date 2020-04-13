
using Microsoft.Xna.Framework;

namespace Myng.Helpers.Enums
{
    public enum ImpairmentType
    {
        Stun,
        Silence,
        Snare
    }

    public class Impairment
    {
        #region Properties

        // time in seconds
        public double DurationLeft;

        public ImpairmentType Type;

        public float Snare;

        public string TextToDisplay;

        #endregion

        #region Constructor

        public Impairment(ImpairmentType type, double duration, float snare)
        {
            DurationLeft = duration;
            Type = type;
            Snare = snare;

            switch (type)
            {
                case ImpairmentType.Stun:
                    TextToDisplay = "STUNNED";
                    break;
                case ImpairmentType.Silence:
                    TextToDisplay = "SILENCED";
                    break;
                case ImpairmentType.Snare:
                    TextToDisplay = "SNARED";
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
