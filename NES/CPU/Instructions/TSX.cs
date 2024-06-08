using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class TSX
    {
        public static void Execute(object operand)
        {
            X = S;
            CheckAndSetNegative(X);
            CheckAndSetZero(X);
        }
    }
}
