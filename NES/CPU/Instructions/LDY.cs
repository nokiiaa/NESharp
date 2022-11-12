using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class LDY
    { 
        public static void Immediate(object operand)
        {
            Y = (byte)operand;
            CheckAndSetNegative(Y);
            CheckAndSetZero(Y);
        }
        
        public static void ZeroPage(object operand)
        {
            Y = Memory[(byte)operand];
            CheckAndSetNegative(Y);
            CheckAndSetZero(Y);
        }
        
        public static void ZeroPageX(object operand)
        {
            Y = Memory[((byte)operand + X) % 0x100];
            CheckAndSetNegative(Y);
            CheckAndSetZero(Y);
        }
        
        public static void Absolute(object operand)
        {
            Y = Memory[(ushort)operand];
            CheckAndSetNegative(Y);
            CheckAndSetZero(Y);
        }
        
        public static void AbsoluteX(object operand)
        {
            if (DoPagesDiffer((ushort)((ushort)operand + X), (ushort)operand))
                TotalCycles++;
            Y = Memory[(ushort)operand + X];
            CheckAndSetNegative(Y);
            CheckAndSetZero(Y);
        }
    }
}
