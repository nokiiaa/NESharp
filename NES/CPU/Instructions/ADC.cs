using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class ADC
    {
        internal static void _result(byte operand)
        {
            int carry = (IsFlagSet(Flag.Carry) ? 1 : 0);
            int sum = A + operand + carry;
            byte res = (byte)(sum & 0xFF);
            if (((A ^ res) & (operand ^ res) & 0x80) >> 7 == 1)
                SetFlag(Flag.Overflow);
            else
                ClearFlag(Flag.Overflow);
            A = res;
            CheckAndSetNegative(A);
            if (sum > 0xFF)
                SetFlag(Flag.Carry);
            else
                ClearFlag(Flag.Carry);
            CheckAndSetZero(A);
        }

        public static void Immediate(object operand) => _result((byte)operand);
        public static void ZeroPage(object operand) => _result(Memory[(byte)operand]);
        public static void ZeroPageX(object operand) => _result(Memory[(byte)operand + X]);
        public static void Absolute(object operand) => _result(Memory[(ushort)operand]);

        public static void AbsoluteX(object operand)
        {
            if (DoPagesDiffer((ushort)((ushort)operand + X), (ushort)operand))
                TotalCycles++;
            _result(Memory[(ushort)operand + X]);
        }

        public static void AbsoluteY(object operand)
        {
            if (DoPagesDiffer((ushort)((ushort)operand + Y), (ushort)operand))
                TotalCycles++;
            _result(Memory[(ushort)operand + Y]);
        }

        public static void IndirectX(object operand) => _result(Memory[Memory.ReadWord( ((byte)operand + X) % 0x100 )]);

        public static void IndirectY(object operand)
        {
            ushort addr = Memory.ReadWord((byte)operand);
            if (DoPagesDiffer((ushort)(addr + Y), addr))
                TotalCycles++;
            _result(Memory[addr + Y]);
        }
    }
}
