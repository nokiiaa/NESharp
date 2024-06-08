using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class SEC
    {
        public static void Execute(object operand) => SetFlag(Flag.Carry);
    }
}
