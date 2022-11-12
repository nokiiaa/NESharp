using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class SAX // This is an unofficial instruction.
    {
        public static void IndirectX(object operand) => Memory[Memory.ReadWord( ((byte)operand + X) % 0x100 )] = (byte)(A & X);
        public static void ZeroPage(object operand)  => Memory[(byte)operand] = (byte)(A & X);
        public static void Absolute(object operand)  => Memory[(ushort)operand] = (byte)(A & X);
        public static void ZeroPageY(object operand) => Memory[( ((byte)operand) + Y) % 0x100] = (byte)(A & X);
    }
}
