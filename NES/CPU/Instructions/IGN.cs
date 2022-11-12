using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class IGN
    {
        public static void Absolute(object operand)  => Memory.ReadByte((ushort)operand);
        public static void AbsoluteX(object operand) => Memory.ReadByte((ushort)operand + X);
    }
}
