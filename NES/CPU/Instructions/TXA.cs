using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class TXA
    {
        public static void Execute(object operand)
        {
            A = X;
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
    }
}
