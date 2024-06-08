using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class ARR
    {
        public static void Immediate(object operand)
        {
            byte op = (byte)operand;
            AND.Immediate(op);
            if ((op & 0x40) > 1)
                SetFlag(Flag.Carry);
            else
                ClearFlag(Flag.Carry);

            if (((op & 0x40) ^ (op & 0x20)) > 1)
                SetFlag(Flag.Overflow);
            else
                ClearFlag(Flag.Overflow);
        }
    }
}
