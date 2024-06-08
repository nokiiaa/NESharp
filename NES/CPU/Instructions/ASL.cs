using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class ASL
    {
        private static void _result(byte b7, byte result)
        {
            if (b7 == 1)
                SetFlag(Flag.Carry);
            else
                ClearFlag(Flag.Carry);
            CheckAndSetZero(result);
            CheckAndSetNegative(result);
        }

        public static void Accumulator(object operand)
        {
            byte oldBit7 = (byte)(A >> 7);
            A <<= 1;
            A &= ~1 & 0xFF;
            _result(oldBit7, A);
        }
        
        public static void ZeroPage(object operand)
        {
            byte oldBit7 = (byte)(Memory[(byte)operand] >> 7);
            Memory[(byte)operand] <<= 1;
            Memory[(byte)operand] &= ~1 & 0xFF;
            _result(oldBit7, Memory[(byte)operand]);
        }
        
        public static void ZeroPageX(object operand)
        {
            ushort addr = (ushort)( ( (byte)operand + X ) % 0x100 );
            byte oldBit7 = (byte)(Memory[(byte)operand + X] >> 7);
            Memory[(byte)operand + X] <<= 1;
            Memory[(byte)operand+ X] &= ~1 & 0xFF;
            _result(oldBit7, Memory[(byte)operand + X]);
        }
        
        public static void Absolute(object operand)
        {
            byte oldBit7 = (byte)(Memory[(ushort)operand] >> 7);
            Memory[(ushort)operand] <<= 1;
            Memory[(ushort)operand] &= ~1 & 0xFF;
            _result(oldBit7, Memory[(ushort)operand]);
        }
        
        public static void AbsoluteX(object operand)
        {
            byte oldBit7 = (byte)(Memory[(ushort)operand + X] >> 7);
            Memory[(ushort)operand + X] <<= 1;
            Memory[(ushort)operand + X] &= ~1 & 0xFF;
            _result(oldBit7, Memory[(ushort)operand + X]);
        }
    }
}
