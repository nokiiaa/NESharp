using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class TAX
    {
        public static void Execute(object operand)
        {
            X = A;
            CheckAndSetNegative(X);
            CheckAndSetZero(X);
        }
    }
}
