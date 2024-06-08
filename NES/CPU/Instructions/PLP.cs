using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class PLP
    {
        public static void Execute(object operand) => P = PopFromStack();
    }
}
