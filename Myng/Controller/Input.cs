using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myng.Controller
{
    public class Input
    {
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public Keys Up { get; set; }
        public Keys Down { get; set; }

        public Keys ShootUp { get; set; }
        public Keys ShootDown { get; set; }
        public Keys ShootLeft { get; set; }
        public Keys ShootRight { get; set; }

        public Keys Spell1 { get; set; }
        public Keys Spell2 { get; set; }
        public Keys Spell3 { get; set; }
        public Keys Spell4 { get; set; }
        public Keys Spell5 { get; set; }
        public Keys Spell6 { get; set; }

        public Keys Item1 { get; set; }
        public Keys Item2 { get; set; }
        public Keys Item3 { get; set; }
        public Keys Item4 { get; set; }
        public Keys Item5 { get; set; }
        public Keys Item6 { get; set; }

        public Input()
        {
            Left = Keys.A;
            Right = Keys.D;
            Up = Keys.W;
            Down = Keys.S;

            ShootUp = Keys.Up;
            ShootDown = Keys.Down;
            ShootLeft = Keys.Left;
            ShootRight = Keys.Right;

            Spell1 = Keys.D1;
            Spell2 = Keys.D2;
            Spell3 = Keys.D3;
            Spell4 = Keys.D4;
            Spell5 = Keys.D5;
            Spell6 = Keys.D6;

            Item1 = Keys.NumPad1;
            Item2 = Keys.NumPad2;
            Item3 = Keys.NumPad3;
            Item4 = Keys.NumPad4;
            Item5 = Keys.NumPad5;
            Item6 = Keys.NumPad6;
        }
    }
}
