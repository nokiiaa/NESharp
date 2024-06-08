using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class DEY
    {
        public static void Execute(object operand)
        {
            Y--;
            CheckAndSetNegative(Y);
            CheckAndSetZero(Y);
        }
    }
}
