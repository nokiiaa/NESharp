using System;
using System.IO;
using static NESharp.NESharpMain;

namespace NESharp.NES
{
    public static class PPU
    {
        public class Sprite
        {
            public int Number { get; private set; }
            public byte TileNumber { get; set; }
            public byte Attributes { get; set; }
            public byte Y { get; set; }
            public byte X { get; set; }
            public byte Palette => (byte)(Attributes & 3);
            public byte Priority => (byte)((Attributes >> 5) & 1);
            public bool FlipX => ((Attributes >> 6) & 1) == 1;
            public bool FlipY => ((Attributes >> 7) & 1) == 1;

            public static Sprite FromOAM(int index)
            {
                Sprite s = new Sprite();
                s.Y = OAM[(index * 4) + 0];
                s.TileNumber = OAM[(index * 4) + 1];
                s.Attributes = OAM[(index * 4) + 2];
                s.X = OAM[(index * 4) + 3];
                s.Number = index;
                return s;
            }
        }

        public enum AutoIncrement : ushort
        {
            Add1 = 1,
            Add32 = 32
        }

        public static PPUMemory Memory { get; private set; } = new PPUMemory();
        public static byte[] OAM { get; private set; } = new byte[256];
        public static byte BGColor => Memory[0x3F00];
        public static ushort CurrentCycle { get; private set; }
        public static int CurrentScanline { get; private set; } = -1;
        public static byte OAMAddress { get; private set; }
        public static byte Nametable { get; private set; }
        public static AutoIncrement AutoIncrementMode { get; private set; } = AutoIncrement.Add1;
        public static ushort SpritePatternAddress { get; private set; }
        public static ushort BGPatternAddress { get; private set; }
        public static ushort AddressLatch { get; private set; } // Used by PPUADDR and PPUSCROLL.
        public static uint[] TextureData { get; private set; } = new uint[256 * 240]; // Frame data to be displayed by SFML.
        public static uint[] MasterPalette { get; private set; } = new uint[0x40]; // Main NES color palette. 
                                                                                   // Indexes into this palette are selected by the palettes in PPU memory.
        public static long TotalCycles { get; private set; }
        public static bool RenderingEnabled => RenderBG || RenderSprites;
        public static bool IsRendering { get; private set; }
        public static bool CauseNMI { get; private set; } // Whether to cause VBlank NMI or not.
        public static bool VBlanking { get; internal set; } // Set when VBlanking.
        public static bool SpriteZeroHit { get; private set; }
        public static bool SpriteOverflow { get; private set; }
        public static bool SpriteHeight { get; private set; } // Sprite height bit: if false, 8x8, if true, 8x16.
        public static bool RenderSprites { get; private set; }
        public static bool RenderBG { get; private set; }
        public static bool RenderSpritesInFirstTile { get; private set; }
        public static bool RenderBGInFirstTile { get; private set; }
        public static bool Grayscale { get; private set; }
        public static bool EmphasizeRed { get; private set; }
        public static bool EmphasizeGreen { get; private set; }
        public static bool EmphasizeBlue { get; private set; }
        public static bool NewFrame { get; internal set; }
        public static bool NMILow { get; internal set; }

        private static Sprite[] _secondaryOAM = new Sprite[8]; // Secondary OAM, used for 8 sprites on current scanline
        private static byte _readBuffer;
        private static ushort _busAddr;
        private static byte _tileLow;
        private static byte _tileHigh;
        private static ushort _v, _t;
        private static byte _x;
        private static byte _w;
        private static byte _atByte, _ntByte;
        public static ulong _tileShift;
        private static Sprite _lastSprite = new Sprite();

        public static byte FineX => _x;
        public static byte FineY => (byte)((_v >> 12) & 7);
        public static byte CoarseX => (byte)(_v & 0x1F);
        public static byte CoarseY => (byte)((_v >> 5) & 0x1F);

        public class PPUMemory : BaseMemory
        {
            public static byte[] NametableA { get; private set; } = new byte[0x400];
            public static byte[] NametableB { get; private set; } = new byte[0x400];
            public override byte this[int index]
            {
                get
                {
                    if (index >= 0x3F00 && index <= 0x3FFF)
                        return _memory[0x3F00 + ((index - 0x3F00) % 32)];
                    if (index >= 0x2000 && index <= 0x2FFF)
                    {
                        switch (CurrentGame.ROM.NametableMirroring)
                        {
                            case 0:  // Horizontal mirroring
                                int i = index - 0x2000;
                                if (i > 0x7FF)
                                    return NametableB[i % 0x400];
                                else
                                    return NametableA[i % 0x400];
                            case 1:  // Vertical mirroring
                                int i2 = (index - 0x2000) % 0x800;
                                if (i2 > 0x3FF)
                                    return NametableB[i2 - 0x400];
                                else
                                    return NametableA[i2];
                        }
                    }
                    foreach (MemoryAccessHandler<ReadHandler> handler in CurrentGame.ROM.Mapper.PPUReadHandlers)
                    {
                        if (index >= handler.StartAddress && index <= handler.EndAddress)
                            return handler.Handler((ushort)index);
                    }
                    return _memory[index];
                }
                set
                {
                    if (index >= 0x2000 && index <= 0x2FFF)
                    {
                        switch (CurrentGame.ROM.NametableMirroring)
                        {
                            case 0:  // Horizontal mirroring
                                int i = index - 0x2000;
                                if (i > 0x7FF)
                                    NametableB[i % 0x400] = value;
                                else
                                    NametableA[i % 0x400] = value;
                                break;
                            case 1:  // Vertical mirroring
                                int i2 = (index - 0x2000) % 0x800;
                                if (i2 > 0x3FF)
                                    NametableB[i2 - 0x400] = value;
                                else
                                    NametableA[i2] = value;
                                break;
                        }
                    }
                    foreach (MemoryAccessHandler<WriteHandler> handler in CurrentGame.ROM.Mapper.PPUWriteHandlers)
                    {
                        if (index >= handler.StartAddress && index <= handler.EndAddress)
                        {
                            handler.Handler((ushort)index, value);
                            return;
                        }
                    }
                    if (index >= 0x3F00 && index <= 0x3FFF)
                    {
                        switch (index)
                        {
                            case 0x3F10: index = 0x3F00; break;
                            case 0x3F14: index = 0x3F04; break;
                            case 0x3F18: index = 0x3F08; break;
                            case 0x3F1C: index = 0x3F0C; break;
                        }
                        _memory[0x3F00 + (index % 0x20)] = value;
                    }
                    else
                        _memory[index] = value;
                }
            }

            public override byte ReadByte(int addr) => base.ReadByte(addr);
            public override ushort ReadWord(int addr) => base.ReadWord(addr);
            public PPUMemory() : base(0x4000) { }
        }

        public static void Initialize()
        {
            using (FileStream fs = new FileStream("nes_palette.bin", FileMode.Open, FileAccess.Read))
                for (int i = 0; MasterPalette.Length > i; i++)
                    MasterPalette[i] = 0xFF000000u | (uint)fs.ReadByte() | (uint)(fs.ReadByte() << 8) | (uint)(fs.ReadByte() << 16);
        }

        private static ushort _getBGPaletteAddress(int palette) => (ushort)(0x3F01 + (palette * 4));
        private static ushort _getSpritePaletteAddress(int palette) => (ushort)(0x3F11 + (palette * 4));

        private static void _XIncrement()
        {
            if ((_v & 0x001F) == 31) // If coarse X == 31
            {
                _v &= 0xFFE0; // Coarse X = 0
                _v ^= 0x0400; // Switch horizontal nametable
            }
            else
                _v++;
        }

        private static void _YIncrement()
        {
            if ((_v & 0x7000) != 0x7000) // If fine Y < 7
                _v += 0x1000; // Increment fine Y
            else
            {
                _v &= 0xFFF; // Fine Y = 0
                int y = (_v & 0x03E0) >> 5; // Let y = coarse Y
                if (y == 29)
                {
                    y = 0; // Coarse Y = 0
                    _v ^= 0x0800; // Switch vertical nametable
                }
                else if (y == 31)
                    y = 0; // Coarse Y = 0, nametable not switched
                else
                    y++; // Increment coarse Y
                _v = (ushort)((_v & ~0x03E0) | (y << 5)); // Put coarse Y back into v
            }
        }

        public static byte HandleRegisterRead(ushort address)
        {
            int portNumber = address % 8;
            if (portNumber == 0x0002) // Is port PPUSTATUS?
            {
                byte s = (byte)(((VBlanking ? 1 : 0) << 7) | // VBlank flag
                                ((SpriteZeroHit ? 1 : 0) << 6) | // Sprite 0 flag
                                ((SpriteOverflow ? 1 : 0) << 5)); // Sprite overflow flag
                AddressLatch = 0x0000;
                VBlanking = false;
                _w = 0;
                return s;
            }
            else if (portNumber == 0x0004) // Is port OAMDATA?
            {
                byte b = OAM[OAMAddress];
                OAMAddress++;
                return b;
            }
            else if (portNumber == 0x0007) // Is port PPUDATA?
            {
                byte b = Memory[_busAddr];
                if (_busAddr < 0x3F00)
                {
                    byte a = _readBuffer;
                    _readBuffer = b;
                    b = a;
                }
                else
                    _readBuffer = b;

                if (!IsRendering)
                    _busAddr += (ushort)AutoIncrementMode;
                else
                {
                    _XIncrement();
                    _YIncrement();
                }
                return b;
            }
            return 0x00;
        }

        public static byte Grayscalify(byte color) => (byte)(color & 0x30);

        public static void HandleRegisterWrite(ushort address, byte value)
        {
            int portNumber = address % 8;
            if (portNumber == 0x0006 || portNumber == 0x0005) // Is port PPUADDR or PPUSCROLL?
            {
                if (_w == 1)
                {
                    _w = 0;
                    if (portNumber == 0x0006)
                    {
                        _t = (ushort)((_t & 0xFF00) | value);
                        _busAddr = _t;
                        _v = _t;
                    }
                    else if (portNumber == 0x0005)
                    {
                        _t = (ushort)((_t & 0x8FFF) | (value & 0x7) << 12);
                        _t = (ushort)((_t & 0xFC1F) | (value & 0xF8) << 2);
                    }
                }
                else if (_w == 0)
                {
                    _w = 1;
                    if (portNumber == 0x0006)
                    {
                        _t &= 0xFF;
                        _t |= (ushort)((value & 0x3F) << 8);
                    }
                    else if (portNumber == 0x0005)
                    {
                        _t &= 0xFFE0;
                        _t |= (ushort)(value >> 3);
                        _x = (byte)(value & 0x07);
                    }
                }
            }
            else if (portNumber == 0x0000 && CPU.TotalCycles >= 10000) // Is port PPUCTRL and 30000 CPU cycles have passed?
            {
                CauseNMI = (value >> 7) == 1;
                SpriteHeight = ((value >> 5) & 1) == 1;
                BGPatternAddress = ((value >> 4) & 1) == 0 ? (ushort)0x0000 : (ushort)0x1000;
                SpritePatternAddress = ((value >> 3) & 1) == 0 ? (ushort)0x0000 : (ushort)0x1000;
                AutoIncrementMode = ((value >> 2) & 1) == 0 ? AutoIncrement.Add1 : AutoIncrement.Add32;
                Nametable = (byte)((value >> 0) & 3);
                _t &= 0x73FF;
                _t |= (ushort)(Nametable << 10);
            }
            else if (portNumber == 0x0003) // Is port OAMADDR?
                OAMAddress = value;
            else if (portNumber == 0x0004) // Is port OAMDATA?
            {
                if (!IsRendering)
                {
                    OAM[OAMAddress] = value;
                    OAMAddress++;
                }
            }
            else if (portNumber == 0x0001) // Is port PPUMASK?
            {
                Grayscale = ((value >> 0) & 1) == 1;
                RenderBGInFirstTile = ((value >> 1) & 1) == 1;
                RenderSpritesInFirstTile = ((value >> 2) & 1) == 1;
                RenderBG = ((value >> 3) & 1) == 1;
                RenderSprites = ((value >> 4) & 1) == 1;
                EmphasizeRed = ((value >> 5) & 1) == 1;
                EmphasizeGreen = ((value >> 6) & 1) == 1;
                EmphasizeBlue = ((value >> 7) & 1) == 1;
                if (CurrentGame.OutputMode == OutputMode.PAL)
                {
                    bool red = EmphasizeRed;
                    EmphasizeRed = EmphasizeGreen;
                    EmphasizeGreen = red;
                }
            }
            else if (portNumber == 0x0007) // Is port PPUDATA?
            {
                Memory[_busAddr] = value;
                _busAddr += (ushort)AutoIncrementMode;
            }
        }

        public static string GetHexMemory(ushort bs, int length)
        {
            string s = "";
            for (int i = 0; length > i; i++)
            {
                s += $"{Memory[bs + i]:X2} ";
                if ((i + 1) % 16 == 0)
                    s += "\r\n";
            }
            return s;
        }

        private static void _putPixel(int x, int y, byte pixel)
        {
            TextureData[x + (y * 256)] = MasterPalette[Grayscale ? Grayscalify(pixel) : pixel];
        }

        private static void _loadLowTile()
        {
            _tileLow = Memory.ReadByte((ushort)(BGPatternAddress + (_ntByte * 16) + FineY));
        }

        private static void _loadHighTile()
        {
            _tileHigh = Memory.ReadByte((ushort)(BGPatternAddress + (_ntByte * 16) + 8 + FineY));
        }

        private static byte _renderFinalPixel(byte bg, byte sprite, Sprite spr)
        {
            byte b = (byte)(bg & 3);
            byte s = (byte)(sprite & 3);
            if (!(spr.X < 8 && (RenderBGInFirstTile || RenderSpritesInFirstTile)) &&
                spr.X != 0xFF &&
                b > 0 && s > 0 &&
                !SpriteZeroHit &&
                RenderingEnabled)
                SpriteZeroHit = true;
            if (b == 0 && s == 0)
                return BGColor;
            else if (b == 0 && s > 0)
                return Memory[0x3F10 + sprite];
            else if (s == 0 && b > 0)
                return Memory[0x3F00 + bg];
            else if (s > 0 && b > 0)
                return Memory[0x3F00 + (spr.Priority == 0 ? (sprite + 0x10) : bg)];
            return 0;
        }

        private static byte _renderBGPixel()
        {
            if (!RenderBG)
                return 0;
            return (byte)((_tileShift >> ( 32 + (7 - FineX) * 4) ) & 15);
        }

        private static byte _renderSpritePixel()
        {
            if (!RenderSprites)
                return 0;
            byte spritePixel = 0;
            byte newSpritePixel = 0;
            for (int i = 0; 8 > i; i++)
            {
                Sprite s = _secondaryOAM[i];
                if ((CurrentCycle - 1) < (s.X + 8) && (CurrentCycle - 1) >= s.X && s.Y != 0xFF)
                {
                    int height = SpriteHeight ? 16 : 8;
                    int yPixel = CurrentScanline - s.Y;
                    int xPixel = (CurrentCycle - 1) - s.X;
                    int patternAddr = SpritePatternAddress;
                    int tileNum = s.TileNumber;
                    if (height == 16)
                    {
                        patternAddr = ((tileNum & 1) == 1) ? 0x1000 : 0x0000;
                        tileNum &= 0xFE;
                    }
                    if (s.FlipX)
                        xPixel = 7 - xPixel;
                    if (s.FlipY)
                        yPixel = (height - 1) - yPixel;
                    patternAddr += tileNum * 16;
                    newSpritePixel = (byte)( ( (( Memory.ReadByte(patternAddr + yPixel + 0) >> (7 - xPixel) ) & 1 ) << 0) |
                                          ( (( Memory.ReadByte(patternAddr + yPixel + 8) >> (7 - xPixel) ) & 1 ) << 1) );
                    newSpritePixel |= (byte)(s.Palette << 2);
                    if ((newSpritePixel & 3) != 0)
                    {
                        spritePixel = newSpritePixel;
                        _lastSprite = s;
                    }
                 }
            }
            return spritePixel;
        }

        public static void Tick() // This is the part everyone came for.
        {
            if (CurrentScanline == 240) { } // Do nothing, post-render scanline.
            else if (CurrentScanline > 240 && CurrentScanline < 261)
            {
                if (CurrentScanline == 241 && CurrentCycle == 1)
                {
                    VBlanking = true;
                    if (CauseNMI)
                        NMILow = true; // Cause VBlank interrupt in the CPU.
                }
            }
            else
            {
                if (CurrentScanline > -1 && RenderingEnabled && (CurrentCycle > 0 && CurrentCycle < 257))
                {
                    byte bgPixel = _renderBGPixel();
                    byte spritePixel = _renderSpritePixel();
                    byte finalPixel = _renderFinalPixel(bgPixel, spritePixel, _lastSprite);

                    if (CurrentScanline > -1 && RenderingEnabled && (CurrentCycle > 0 && CurrentCycle < 257))
                        _putPixel(CurrentCycle - 1, CurrentScanline, finalPixel); // Render BG pixel
                }

                if (CurrentScanline == -1)
                {
                    if (CurrentCycle == 1)
                    { 
                        VBlanking = false;
                        SpriteOverflow = false;
                        SpriteZeroHit = false;
                    }
                    else if (CurrentCycle >= 280 && CurrentCycle <= 304) // Copy vertical bits from _t to _v
                    {
                        _v &= 0x841F;
                        _v |= (ushort)(_t & 0x7BE0);
                    }
                    else if (CurrentCycle == 340)
                       NewFrame = true;
                }

                if ((CurrentCycle > 0 && CurrentCycle < 257) || (CurrentCycle >= 321 && CurrentCycle <= 336)) // Reload
                {
                    _tileShift <<= 4;
                    switch (CurrentCycle % 8)
                    {
                        case 1: _loadNTByte(); break;
                        case 3: _loadATByte(); break;
                        case 5: _loadLowTile(); break;
                        case 7: _loadHighTile(); break;
                        case 0:
                            if (CurrentCycle == 256)
                                _YIncrement();
                            else
                                _XIncrement();
                            _loadShiftRegisters();
                            break;
                    }
                }
                if (CurrentCycle == 257) // Copy horizontal bits from t to v
                {
                    _v &= 0xFBE0;
                    _v |= (ushort)(_t & 0x041F);
                    _loadSecondaryOAM();
                }
            }

            CurrentCycle++;

            if (CurrentCycle == 341)
            {
                CurrentCycle = 0;
                if (CurrentScanline == 260)
                    CurrentScanline = -1;
                else
                    CurrentScanline++;
            }
        }

        private static void _loadSecondaryOAM()
        {
            int index = 0;
            for (int i = 0; 8 > i; i++)
                _secondaryOAM[i] = new Sprite { X = 0xFF, Y = 0xFF, Attributes = 0, TileNumber = 0 };
            for (int i = 0; 64 > i; i++)
            {
                Sprite s = Sprite.FromOAM(i);
                int height = SpriteHeight ? 16 : 8;
                if (CurrentScanline + 1 < (s.Y + height) && CurrentScanline + 1 >= s.Y)
                {
                    if (index < 8)
                        _secondaryOAM[index++] = s;
                    else
                        SpriteOverflow = true;
                }
            }
        }

        private static void _loadShiftRegisters()
        {
            for (int x = 0; x < 8; x++)
            {
                uint pixel = (uint)(((_tileHigh & 0x80) >> 6) | ((_tileLow & 0x80) >> 7));
                _tileShift |= ((pixel | (uint)(_atByte << 2)) << ((7 - x) * 4));
                _tileLow <<= 1;
                _tileHigh <<= 1;
            }
        }

        private static void _loadATByte()
        {
            int addr = 0x23C0 | (_v & 0x0C00) | ((_v >> 4) & 0x38) | ((_v >> 2) & 0x07); // Attribute table byte address
            _atByte = (byte)( (Memory[addr] >> ( (CoarseX & 2) | ( (CoarseY & 2) << 1 ) )) & 3 );
        }

        private static void _loadNTByte()
        {
            _ntByte = Memory[(ushort)(0x2000 | (_v & 0xFFF))];
        }
    }
}
