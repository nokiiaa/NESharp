using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class CLC
    {
        public static void Execute(object operand) => ClearFlag(Flag.Carry);
    }
}
