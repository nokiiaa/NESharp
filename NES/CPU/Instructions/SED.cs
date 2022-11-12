using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class SED
    {
        public static void Execute(object operand) => SetFlag(Flag.Decimal);
    }
}
