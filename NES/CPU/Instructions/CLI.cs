using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class CLI
    {
        public static void Execute(object operand) => ClearFlag(Flag.InterruptDisable);
    }
}
