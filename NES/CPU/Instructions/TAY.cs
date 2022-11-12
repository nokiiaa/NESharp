using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class TAY
    {
        public static void Execute(object operand)
        {
            Y = A;
            CheckAndSetNegative(Y);
            CheckAndSetZero(Y);
        }
    }
}
