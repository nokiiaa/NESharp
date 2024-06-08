using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESharp.NES.Input
{
    public class Joypad : Controller
    {
        public Keyboard.Key ButtonA      = Keyboard.Key.Period;
        public Keyboard.Key ButtonB      = Keyboard.Key.Comma;
        public Keyboard.Key ButtonSelect = Keyboard.Key.S;
        public Keyboard.Key ButtonStart  = Keyboard.Key.Return;
        public Keyboard.Key ButtonUp     = Keyboard.Key.Up;
        public Keyboard.Key ButtonLeft   = Keyboard.Key.Left;
        public Keyboard.Key ButtonRight  = Keyboard.Key.Right;
        public Keyboard.Key ButtonDown   = Keyboard.Key.Down;
        private byte _shift;
        private byte _shiftIndex;

        public override byte GetData() => (byte)((_shift >> _shiftIndex++) & 1);

        public override void Tick()
        {
            if (StrobeBit)
            {
                _shiftIndex = 0;
                _shift = (byte)(((Keyboard.IsKeyPressed(ButtonA)      ? 1 : 0) << 0) |
                                ((Keyboard.IsKeyPressed(ButtonB)      ? 1 : 0) << 1) |
                                ((Keyboard.IsKeyPressed(ButtonSelect) ? 1 : 0) << 2) |
                                ((Keyboard.IsKeyPressed(ButtonStart)  ? 1 : 0) << 3) |
                                ((Keyboard.IsKeyPressed(ButtonUp)     ? 1 : 0) << 4) |
                                ((Keyboard.IsKeyPressed(ButtonDown)   ? 1 : 0) << 5) |
                                ((Keyboard.IsKeyPressed(ButtonLeft)   ? 1 : 0) << 6) |
                                ((Keyboard.IsKeyPressed(ButtonRight)  ? 1 : 0) << 7));
            }
        }
    }
}
