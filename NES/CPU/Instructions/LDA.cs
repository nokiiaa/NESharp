using System;
using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class LDA
    {
        public static void Immediate(object operand)
        {
            A = (byte)operand;
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
        
        public static void ZeroPage(object operand)
        {
            A = Memory[(byte)operand];
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
        
        public static void ZeroPageX(object operand)
        {
            A = Memory[((byte)operand + X) % 0x100];
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
        
        public static void Absolute(object operand)
        {
            A = Memory[(ushort)operand];
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
        
        public static void AbsoluteX(object operand)
        {
            if (DoPagesDiffer((ushort)(((ushort)operand + X) % 0x10000), (ushort)operand))
                TotalCycles++;
            A = Memory[(ushort)operand + X];
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
        
        public static void AbsoluteY(object operand)
        {
            if (DoPagesDiffer((ushort)(((ushort)operand + Y) % 0x10000), (ushort)operand))
                TotalCycles++;
            A = Memory[(ushort)operand + Y];
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
        
        public static void IndirectX(object operand)
        {
            A = Memory[Memory.ReadWord(((byte)operand + X) % 0x100)];
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
        
        public static void IndirectY(object operand)
        {
            ushort addr = Memory.ReadWord((byte)operand);
            if (DoPagesDiffer((byte)(addr + Y), addr))
                TotalCycles++;
            A = Memory[Memory.ReadWord((byte)operand) + Y];
            CheckAndSetNegative(A);
            CheckAndSetZero(A);
        }
    }
}
