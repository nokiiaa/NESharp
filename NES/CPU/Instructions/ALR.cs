namespace NESharp.NES.Instructions
{
    public static class ALR // This is an unofficial instruction.
    {
        public static void Immediate(object operand)
        {
            AND.Immediate(operand);
            LSR.Accumulator(null);
        }
    }
}
