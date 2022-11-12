using static NESharp.NES.CPU;
using static NESharp.NES.CPU.Registers;

namespace NESharp.NES.Instructions
{
    public static class SBC
    {
        internal static void _result(byte operand) => ADC._result((byte)~operand);
        public static void Immediate(object operand) =>  _result((byte)operand);        
        public static void ZeroPage(object operand) => _result(Memory[(byte)operand]);
        public static void ZeroPageX(object operand) => _result(Memory[((byte)operand + X) % 0x100]);  
           
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
        
        public static void IndirectX(object operand) => _result(Memory[Memory.ReadWord(((byte)operand + X) % 0x100)]);
        
        public static void IndirectY(object operand)
        {
            ushort addr = Memory.ReadWord((byte)operand);
            if (DoPagesDiffer((ushort)(addr + Y), addr))
                TotalCycles++;
            _result(Memory[Memory.ReadWord((byte)operand + Y)]);
        }
    }
}
