using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class ANC // This is an unofficial instruction.
    {
        public static void Immediate(object operand)
        {
            AND.Immediate(operand);
            if (IsFlagSet(Flag.Negative))
                SetFlag(Flag.Negative);
            else
                ClearFlag(Flag.Negative);
        }
    }
}
