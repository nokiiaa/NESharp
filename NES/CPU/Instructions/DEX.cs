﻿using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class DEX
    {
        public static void Execute(object operand)
        {
            X--;
            CheckAndSetNegative(X);
            CheckAndSetZero(X);
        }
    }
}
