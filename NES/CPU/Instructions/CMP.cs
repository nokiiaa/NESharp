using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class CMP
    {
        public static void Immediate(object operand) => Compare(A, (byte)operand);
        public static void ZeroPage(object operand) => Compare(A, Memory[(byte)operand]);
        public static void ZeroPageX(object operand) => Compare(A, Memory[(byte)operand + X]);
        public static void Absolute(object operand) => Compare(A, Memory[(ushort)operand]);
        
        public static void AbsoluteX(object operand)
        {
            if (DoPagesDiffer((ushort)((ushort)operand + X), (ushort)operand))
                TotalCycles++;
            Compare(A, Memory[(ushort)operand + X]);
        }
        
        public static void AbsoluteY(object operand)
        {
            if (DoPagesDiffer((ushort)((ushort)operand + Y), (ushort)operand))
                TotalCycles++;
            Compare(A, Memory[(ushort)operand + Y]);
        }
        
        public static void IndirectX(object operand) => Compare(A, Memory[Memory.ReadWord( ((byte)operand + X) % 0x100 )]);
        
        public static void IndirectY(object operand)
        {
            ushort addr = Memory.ReadWord((byte)operand);
            if (DoPagesDiffer((ushort)(addr + Y), addr))
                TotalCycles++;
            Compare(A, Memory[Memory.ReadWord((byte)operand) + Y]);
        }
    }
}
