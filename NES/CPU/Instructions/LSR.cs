using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class LSR
    {
        private static void _result(byte b0, byte result)
        {
            if (b0 == 1)
                SetFlag(Flag.Carry);
            else
                ClearFlag(Flag.Carry);
            CheckAndSetZero(result);
            CheckAndSetNegative(result);
        }

        public static void Accumulator(object operand)
        {
            byte oldBit0 = (byte)(A & 1);
            A >>= 1;
            A &= ~(1 << 7) & 0xFF;
            _result(oldBit0, A);
        }
        
        public static void ZeroPage(object operand)
        {
            byte oldBit0 = (byte)(Memory[(byte)operand] & 1);
            Memory[(byte)operand] >>= 1;
            Memory[(byte)operand] &= ~(1 << 7) & 0xFF;
            _result(oldBit0, Memory[(byte)operand]);
        }
        
        public static void ZeroPageX(object operand)
        {
            ushort addr = (ushort)(((byte)operand + X) % 0x100);
            byte oldBit0 = (byte)(Memory[addr] & 1);
            Memory[addr] >>= 1;
            Memory[addr] &= ~(1 << 7) & 0xFF;
            _result(oldBit0, Memory[addr]);
        }
        
        public static void Absolute(object operand)
        {
            byte oldBit0 = (byte)(Memory[(ushort)operand] & 1);
            Memory[(ushort)operand] >>= 1;
            Memory[(ushort)operand] &= ~(1 << 7) & 0xFF;
            _result(oldBit0, Memory[(ushort)operand]);
        }
        
        public static void AbsoluteX(object operand)
        {
            byte oldBit0 = (byte)(Memory[(ushort)operand + X] & 1);
            Memory[(ushort)operand + X] >>= 1;
            Memory[(ushort)operand + X] &= ~(1 << 7) & 0xFF;
            _result(oldBit0, Memory[(ushort)operand + X]);
        }
    }
}
