using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class BPL
    {
        public static void ZeroPage(object operand)
        {
            if (!IsFlagSet(Flag.Negative))
                BranchTo((byte)operand);
        }
    }
    public static class BMI
    {
        public static void ZeroPage(object operand)
        {
            if (IsFlagSet(Flag.Negative))
                BranchTo((byte)operand);
        }
    }
    public static class BVC
    {
        public static void ZeroPage(object operand)
        {
            if (!IsFlagSet(Flag.Overflow))
                BranchTo((byte)operand);
        }
    }
    public static class BVS
    {
        public static void ZeroPage(object operand)
        {
            if (IsFlagSet(Flag.Overflow))
                BranchTo((byte)operand);
        }
    }
    public static class BCC
    {
        public static void ZeroPage(object operand)
        {
            if (!IsFlagSet(Flag.Carry))
                BranchTo((byte)operand);
        }
    }
    public static class BCS
    {
        public static void ZeroPage(object operand)
        {
            if (IsFlagSet(Flag.Carry))
                BranchTo((byte)operand);
        }
    }
    public static class BNE
    {
        public static void ZeroPage(object operand)
        {
            if (!IsFlagSet(Flag.Zero))
                BranchTo((byte)operand);
        }
    }
    public static class BEQ
    {
        public static void ZeroPage(object operand)
        {
            if (IsFlagSet(Flag.Zero))
                BranchTo((byte)operand);
        }
    }
}
