using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class AXS // This is an unofficial instruction.
    {
        public static void Immediate(object operand)
        {
            byte op = (byte)operand;
            byte res = (byte)((A & X) - op);
            X = (byte)((A & X) - op);
            CheckAndSetNegative(X);
            CheckAndSetZero(X);
            if (res > 0xFF)
                SetFlag(Flag.Carry);
            else
                ClearFlag(Flag.Carry);
        }
    }
}
