using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class PLA
    {
        public static void Execute(object operand)
        {
            A = PopFromStack();
            CheckAndSetZero(A);
            CheckAndSetNegative(A);
        }
    }
}
