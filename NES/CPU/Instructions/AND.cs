using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class AND
    {
        public static void Immediate(object operand)
        {
            A = (byte)(A & (byte)operand);
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
        
        public static void ZeroPage(object operand)
        {
            A = (byte)(A & Memory[(byte)operand]);
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
        
        public static void ZeroPageX(object operand)
        {
            A = (byte)(A & (Memory[((byte)operand + X) % 0x100]));
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
        
        public static void Absolute(object operand)
        {
            A = (byte)(A & Memory[(ushort)operand]);
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
        
        public static void AbsoluteX(object operand)
        {
            if (DoPagesDiffer((ushort)((ushort)operand + X), (ushort)operand))
                TotalCycles++;
            A = (byte)(A & Memory[(ushort)operand + X]);
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
        
        public static void AbsoluteY(object operand)
        {
            if (DoPagesDiffer((ushort)((ushort)operand + Y), (ushort)operand))
                TotalCycles++;
            A = (byte)(A & Memory[(ushort)operand + Y]);
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
        
        public static void IndirectX(object operand)
        {
            A = (byte)(A & Memory[Memory.ReadWord((byte)operand+ X)]);
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
        
        public static void IndirectY(object operand)
        {
            ushort addr = Memory.ReadWord((byte)operand);
            if (DoPagesDiffer((ushort)(addr + Y), addr))
                TotalCycles++;
            A = (byte)(A & Memory[addr + Y]);
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
    }
}
