using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class CLD
    {
        public static void Execute(object operand) => ClearFlag(Flag.Decimal);
    }
}
