using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class CPX
    {
        public static void Immediate(object operand)
        {
            byte arg = (byte)operand;
            Compare(X, arg);
        }
        
        public static void ZeroPage(object operand)
        {
            byte arg = Memory[(byte)operand];
            Compare(X, arg);
        }
        
        public static void Absolute(object operand)
        {
            byte arg = Memory[(ushort)operand];
            Compare(X, arg);
        }
    }
}
