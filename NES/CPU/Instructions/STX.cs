using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class STX
    {
        public static void ZeroPage(object operand) => CPU.Memory[(byte)operand] = X;
        public static void ZeroPageY(object operand) => CPU.Memory[((byte)operand + Y) % 0x100] = X;
        public static void Absolute(object operand) => CPU.Memory[(ushort)operand] = X;
    }
}
