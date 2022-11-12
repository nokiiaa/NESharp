using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESharp.NES
{
    public abstract class Controller
    {
        public bool StrobeBit { get; set; }
        public abstract void Tick();
        public abstract byte GetData();
    }
}
