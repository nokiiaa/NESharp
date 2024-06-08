using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class BRK
    { 
        public static void Execute(object operand)
        {
            PC++;
            PushToStack16(PC);
            PushToStack((byte)(P | (1 << 4)));
            SetFlag(Flag.InterruptDisable);
            PC = Memory.ReadWord(IRQVector);
            PCModified = true;
        }
    }
}
