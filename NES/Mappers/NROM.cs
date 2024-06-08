using static NESharp.NESharpMain;

namespace NESharp.NES.Mappers
{
    public class NROM : Mapper
    {
        public override void Initialize()
        {
            if (CurrentGame.ROM.PRGMemory.Length == 2 * 16384) // NROM-256
                CPUReadHandlers.Add(new MemoryAccessHandler<ReadHandler>(0x8000, 0xFFFF, x => CurrentGame.ROM.PRGMemory[x - 0x8000]));
            else
            {
                CPUReadHandlers.Add(new MemoryAccessHandler<ReadHandler>(0x8000, 0xBFFF, x => CurrentGame.ROM.PRGMemory[x - 0x8000]));
                CPUReadHandlers.Add(new MemoryAccessHandler<ReadHandler>(0xC000, 0xFFFF, x => CurrentGame.ROM.PRGMemory[x - 0xC000]));
            }
            if(CurrentGame.ROM.CHRMemory.Length == 8192)
                PPUReadHandlers.Add    (new MemoryAccessHandler<ReadHandler>(0x0000, 0x1FFF, x => CurrentGame.ROM.CHRMemory[x         ]));
        }
    }
}
