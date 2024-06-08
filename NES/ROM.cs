using NESharp.NES.Mappers;
using System;
using System.IO;

namespace NESharp.NES
{
    public class ROM
    {
        public int NametableMirroring { get; set; }
        public byte[] RawBytes  { get; set; }
        public string Filename  { get; set; }
        public byte[] PRGMemory { get; set; }
        public byte[] CHRMemory { get; set; }
        public Mapper Mapper    { get; set; } = new NROM();

        public static ROM Parse(string filename)
        {
            ROM rom = new ROM();
            rom.RawBytes = File.ReadAllBytes(filename);
            rom.Filename = filename;
            using (MemoryStream ms = new MemoryStream(rom.RawBytes))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                   if (br.ReadByte()  == 0x4E &&
                        br.ReadByte() == 0x45 &&
                        br.ReadByte() == 0x53 &&
                        br.ReadByte() == 0x1A) // Is file valid iNES ROM?
                    {
                        rom.PRGMemory = new byte[0x4000 * br.ReadByte()]; // Size of PRG ROM in 16KB units
                        rom.CHRMemory = new byte[0x2000 * br.ReadByte()]; // Size of CHR ROM in 8KB units
                        rom.NametableMirroring = br.ReadByte() & 1;
                        br.ReadBytes(9);
                        br.Read(rom.PRGMemory, 0, rom.PRGMemory.Length);
                        br.Read(rom.CHRMemory, 0, rom.CHRMemory.Length);
                    }
                    else
                        throw new Exception("Unknown NES ROM format.");
                }
            }
            return rom;
        }
    }
}
