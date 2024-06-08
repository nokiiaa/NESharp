using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class PHA
    {
        public static void Execute(object operand) => PushToStack(A);
    }
}
