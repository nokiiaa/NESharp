using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class LAX // This is an unofficial instruction.
    {
        public static void IndirectX(object operand)
        {
            LDA.IndirectX(operand);
            TAX.Execute(null);
        }

        public static void IndirectY(object operand)
        {
            LDA.IndirectY(operand);
            TAX.Execute(null);
        }

        public static void ZeroPage(object operand)
        {
            LDA.ZeroPage(operand);
            TAX.Execute(null);
        }

        public static void Absolute(object operand)
        {
            LDA.Absolute(operand);
            TAX.Execute(null);
        }

        public static void AbsoluteY(object operand)
        {
            LDA.AbsoluteY(operand);
            TAX.Execute(null);
        }

        public static void ZeroPageY(object operand)
        {
            A = Memory[((byte)operand + Y) % 0x100];
            CheckAndSetZero(A);
            CheckAndSetNegative(A);
            TAX.Execute(null);
        }
    }
}
