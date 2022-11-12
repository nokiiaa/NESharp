using System;
using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class RTS
    {
        public static void Execute(object operand)
        {
            PC = (ushort)(PopFromStack16() + 1);
            PCModified = true;
        }
    }
}
