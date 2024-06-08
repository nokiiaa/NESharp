using System;
using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class JSR
    {
        public static void Absolute(object operand)
        {
            PushToStack16((ushort)(PC + 2));
            PC = (ushort)operand;
            PCModified = true;
        }
    }
}
