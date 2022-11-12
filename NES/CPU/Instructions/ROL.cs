using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class ROL
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
            if (IsFlagSet(Flag.Carry))
                A |= 1;
            else
                A &= 0xFE;
            _result(oldBit7, A);
        }
        
        public static void ZeroPage(object operand)
        {
            byte oldBit7 = (byte)(Memory[(byte)operand] >> 7);
            Memory[(byte)operand] <<= 1;
            if (IsFlagSet(Flag.Carry))
                Memory[(byte)operand] |= 1;
            else
                Memory[(byte)operand] &= 0xFE;
            _result(oldBit7, Memory[(byte)operand]);
        }

        public static void ZeroPageX(object operand)
        {
            ushort addr = (ushort)(((byte)operand + X) % 0x100);
            byte oldBit7 = (byte)(Memory[addr] >> 7);
            Memory[addr] <<= 1;
            if (IsFlagSet(Flag.Carry))
                Memory[addr] |= 1;
            else
                Memory[addr] &= 0xFE;
            _result(oldBit7, Memory[addr]);
        }
        
        public static void Absolute(object operand)
        {
            byte oldBit7 = (byte)(Memory[(ushort)operand] >> 7);
            Memory[(ushort)operand] <<= 1;
            if (IsFlagSet(Flag.Carry))
                Memory[(ushort)operand] |= 1;
            else
                Memory[(ushort)operand] &= 0xFE;
            _result(oldBit7, Memory[(ushort)operand]);
        }
        
        public static void AbsoluteX(object operand)
        {
            byte oldBit7 = (byte)(Memory[(ushort)operand + X] >> 7);
            Memory[(ushort)operand + X] <<= 1;
            if (IsFlagSet(Flag.Carry))
                Memory[(ushort)operand + X] |= 1;
            else
                Memory[(ushort)operand + X] &= 0xFE;
            _result(oldBit7, Memory[(ushort)operand + X]);
        }
    }
}
