using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class BIT
    {
        public static void ZeroPage(object operand)
        {
            byte mem = Memory[(byte)operand];
            CheckAndSetZero((byte)(A & mem));
            if ((mem & 0x40) > 1)
                SetFlag(Flag.Overflow);
            else
                ClearFlag(Flag.Overflow);

            if ((mem & 0x80) > 1)
                SetFlag(Flag.Negative);
            else
                ClearFlag(Flag.Negative);
        }
        
        public static void Absolute(object operand)
        {
            byte mem = Memory[(ushort)operand];
            CheckAndSetZero((byte)(A & mem));
            if ((mem & 0x40) > 1)
                SetFlag(Flag.Overflow);
            else
                ClearFlag(Flag.Overflow);

            if ((mem & 0x80) > 1)
                SetFlag(Flag.Negative);
            else
                ClearFlag(Flag.Negative);
        }
    }
}
