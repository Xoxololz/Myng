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
        public Keys Shoot { get; set; }
        public Keys Spell1 { get; set; }
        public Keys Spell2 { get; set; }
        public Keys Spell3 { get; set; }
        public Keys Spell4 { get; set; }

        public Input()
        {
            Left = Keys.A;
            Right = Keys.D;
            Up = Keys.W;
            Down = Keys.S;
            Shoot = Keys.Space;
            Spell1 = Keys.D1;
            Spell2 = Keys.D2;
            Spell3 = Keys.D3;
            Spell4 = Keys.D4;
        }
    }
}
