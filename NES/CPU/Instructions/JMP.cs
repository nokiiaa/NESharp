using System;
using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class JMP
    {
        public static void Absolute(object operand) { PC = (ushort)operand;                  PCModified = true; }
        public static void Indirect(object operand) { PC = Memory.ReadWord((ushort)operand); PCModified = true; }
    }
}
