using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class CLV
    {
        public static void Execute(object operand) => ClearFlag(Flag.Overflow);
    }
}
