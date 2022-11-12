using System;
using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class RTI
    {
        public static void Execute(object operand)
        {
            P = PopFromStack();
            PC = PopFromStack16();
            PCModified = true;
        }
    }
}
