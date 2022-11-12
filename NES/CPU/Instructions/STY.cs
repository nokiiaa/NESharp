using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class STY
    { 
        public static void ZeroPage(object operand)  => Memory[(byte)operand] = Y;
        public static void ZeroPageX(object operand) => Memory[((byte)operand + X) % 0x100] = Y;
        public static void Absolute(object operand)  => Memory[(ushort)operand] = Y;
    }
}
