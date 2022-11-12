using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class LDX
    {
        public static void Immediate(object operand)
        {
            X = (byte)operand;
            CheckAndSetNegative(X);
            CheckAndSetZero(X);
        }
        
        public static void ZeroPage(object operand)
        {
            X = Memory[(byte)operand];
            CheckAndSetNegative(X);
            CheckAndSetZero(X);
        }
        
        public static void ZeroPageY(object operand)
        {
            X = Memory[((byte)operand + Y) % 0x100];
            CheckAndSetNegative(X);
            CheckAndSetZero(X);
        }
        
        public static void Absolute(object operand)
        {
            X = Memory[(ushort)operand];
            CheckAndSetNegative(X);
            CheckAndSetZero(X);
        }
        
        public static void AbsoluteY(object operand)
        {
            if (DoPagesDiffer((ushort)((ushort)operand + Y), (ushort)operand))
                TotalCycles++;
            X = Memory[(ushort)operand + Y];
            CheckAndSetNegative(X);
            CheckAndSetZero(X);
        }
    }
}
