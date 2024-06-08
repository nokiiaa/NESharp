using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class PHP
    {
        public static void Execute(object operand) => PushToStack((byte)(P | 0x30));
    }
}
