using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class TYA
    {
        public static void Execute(object operand)
        {
            A = Y;
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
    }
}
