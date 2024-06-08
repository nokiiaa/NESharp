using NESharp.NES.Input;

namespace NESharp.NES
{
    public enum OutputMode
    {
        NTSC,
        PAL
    }

    public class Game
    {
        public ROM ROM { get; set; }
        public Controller Controller { get; set; } = new Joypad();
        public OutputMode OutputMode { get; set; }
    }
}
