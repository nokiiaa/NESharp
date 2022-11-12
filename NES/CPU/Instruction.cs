using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESharp.NES
{
    public class Instruction : Attribute
    {
        public Instruction(byte len, int cycles, Action<object> code)
        {
            Length = len;
            Cycles = cycles;
            Code = code;
        }
        public byte Length { get; set; }
        public int Cycles { get; set; }
        public Action<object> Code { get; set; }
    }
}
