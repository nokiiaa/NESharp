using System;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class STA
    {
        public static void ZeroPage(object operand)  => CPU.Memory[(byte)operand] = A;
        public static void ZeroPageX(object operand) => CPU.Memory[((byte)operand + X) % 0x100] = A;
        public static void Absolute(object operand)  => CPU.Memory[(ushort)operand] = A;
        public static void AbsoluteX(object operand) => CPU.Memory[(ushort)operand + X] = A;
        public static void AbsoluteY(object operand) => CPU.Memory[(ushort)operand + Y] = A;
        public static void IndirectX(object operand) => CPU.Memory[CPU.Memory.ReadWord(((byte)operand + X) % 0x100)] = A;
        public static void IndirectY(object operand) => CPU.Memory[CPU.Memory.ReadWord((byte)operand) + Y] = A;
    }
}
