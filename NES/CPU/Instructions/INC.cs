using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class INC
    {
        public static void ZeroPage(object operand)
        {
            Memory[(byte)operand]++;
            CheckAndSetNegative(Memory[(byte)operand]);
            CheckAndSetZero(Memory[(byte)operand]);
        }
        
        public static void ZeroPageX(object operand)
        {
            Memory[(byte)operand+ X]++;
            CheckAndSetNegative(Memory[(byte)operand + X]);
            CheckAndSetZero(Memory[(byte)operand + X]);
        }
        
        public static void Absolute(object operand)
        {
            Memory[(ushort)operand]++;
            CheckAndSetNegative(Memory[(ushort)operand]);
            CheckAndSetZero(Memory[(ushort)operand]);
        }
        
        public static void AbsoluteX(object operand)
        {
            Memory[(ushort)operand + X]++;
            CheckAndSetNegative(Memory[(ushort)operand + X]);
            CheckAndSetZero(Memory[(ushort)operand + X]);
        }
    }
}
