using System;
using System.Collections.Generic;
using System.Reflection;
using static NESharp.NESharpMain;
using static NESharp.NES.CPU.Registers;
using Out = System.Console;
using SFML.Window;

namespace NESharp.NES
{

    public static partial class CPU
    {
        public const ushort NMIVector   = 0xFFFA;
        public const ushort ResetVector = 0xFFFC;
        public const ushort IRQVector   = 0xFFFE;

        public class CPUMemory : BaseMemory
        {
            public override byte this [int ind]
            {
                get
                {
                    int index = ind % 0x10000;
                    if (index >= 0x2000 && index <= 0x3FFF) // Check if writing to PPU port
                        return PPU.HandleRegisterRead((ushort)index);
                    if (index <= 0x1FFF) // Check if reading from internal RAM
                        return _memory[index % 0x800];
                    else if (index == 0x4016) // Controller port 
                        return CurrentGame.Controller.GetData();
                    else
                    {
                        foreach (MemoryAccessHandler<ReadHandler> handler in CurrentGame.ROM.Mapper.CPUReadHandlers)
                            if (index >= handler.StartAddress && index <= handler.EndAddress)
                                return handler.Handler((ushort)index);
                    }
                    return _memory[index];
                }
                set
                {
                    uint index = (uint)ind % 0x10000;
                    if (index >= 0x2000 && index <= 0x3FFF) // Check if writing to PPU port
                    {
                        PPU.HandleRegisterWrite((ushort)index, value);
                        return;
                    }
                    else if (index <= 0x1FFF) // Check if writing to internal memory
                    {
                        _memory[index % 0x800] = value;
                        return;
                    }
                    else if (index == 0x4016) // Controller port 
                    {
                        CurrentGame.Controller.StrobeBit = (value & 1) == 1;
                        return;
                    }
                    else if (index == 0x4014) // PPU OAM DMA
                    {
                        _dma = true;
                        for (int i = 0; 256 - PPU.OAMAddress > i; i++)
                            PPU.OAM[PPU.OAMAddress + i] = Memory[(value << 8) + i];
                    }
                    else
                    {
                        foreach (MemoryAccessHandler<WriteHandler> handler in CurrentGame.ROM.Mapper.CPUWriteHandlers)
                        {
                            if (index >= handler.StartAddress && index <= handler.EndAddress)
                            {
                                handler.Handler((ushort)index, value);
                                return;
                            }
                        }
                    }
                    _memory[index] = value;
                }
            }

            public override byte ReadByte(int addr) => base.ReadByte(addr);
            public override ushort ReadWord(int addr)
            {
                if ((addr & 0xFF) == 0xFF)
                {
                    byte lo = Memory[(ushort)addr];
                    byte hi = Memory[(ushort)addr & 0xFF00];
                    return (ushort)(lo | (hi << 8));
                }
                else
                    return ReadWordInternal(addr);
            }
            internal ushort ReadWordInternal(int addr) => base.ReadWord(addr);
        }

        public static bool PCModified { get; internal set; }

        public static CPUMemory Memory = new CPUMemory();

        public static List<ushort> Breakpoints { get; private set; } = new List<ushort>();

        public static class Registers
        {
            public static byte A; // Accumulator.
            public static byte X; // Index register X.
            public static byte Y; // Index register Y.
            public static ushort PC = 0xC000; // Program counter.
            public static byte S = 0xFD; // Stack pointer.
                                         //                               NV-BDIZC
            public static byte P = 0x04; // Status register: by default 0b00000100 (0x34).
            public enum Flag
            {
                Carry,
                Zero,
                InterruptDisable,
                Decimal,
                Unused,
                Unused2,
                Overflow,
                Negative
            }
            public static void ToggleFlag(Flag f) => P ^= (byte)(1 << (byte)f);
            public static void SetFlag(Flag f)    => P |= (byte)(1 << (byte)f);
            public static void ClearFlag(Flag f)  => P &= (byte)~(1 << (byte)f);
            public static bool IsFlagSet(Flag f)  => ((P >> (byte)f) & 1) == 1;
        }
        

        private static bool _dma;
        public static long TotalCycles { get; set; }

        public static void TriggerNMI()
        {
            PushToStack16(PC);
            PushToStack((byte)(P | (1 << 4)));
            PC = Memory.ReadWord(NMIVector);
        }

        public static void Reset()
        {
            TotalCycles = 0;
            S -= 3;
            SetFlag(Flag.InterruptDisable);
            PC = Memory.ReadWordInternal(ResetVector);
        }

        public static bool DoPagesDiffer(ushort a, ushort b) => (a >> 8) != (b >> 8); // Check if the 256-byte pages of two 16-bit addresses are different.

        public static void Step()
        {
            long oldCycles = TotalCycles;
            ushort oldPC = PC;
            Instruction ins = Instructions[Memory[PC]]; // Find instruction using given opcode.
            if (Breakpoints.Contains(oldPC))
            {
                Out.WriteLine($"*** BREAKPOINT @ ${PC:X4}, PRESS ANY KEY ***");
                Out.WriteLine(GetStatus());
                Out.ReadKey(true);
            }
            switch (ins.Length - 1)
            {
                case 0: // This is an instruction with no arguments, 1 byte  long.
                    ins.Code(null);
                    break;
                case 1: // This is an instruction with one argument, 2 bytes long.
                    ins.Code(Memory[PC + 1]);
                    break;
                case 2: // This is an instruction with one argument, 3 bytes long.
                    ins.Code(Memory.ReadWordInternal(PC + 1));
                    break;
            }
            if (!PCModified) // Check if the instruction didn't manipulate the PC (i. e. JMP).
                PC += ins.Length;
            PCModified = false;
            if (_dma)
            {
                _dma = false;
                if (TotalCycles % 2 == 1)
                    ins.Cycles++;
                ins.Cycles += 513; // Add DMA cycles
            }
            TotalCycles += ins.Cycles;
            if (PPU.CauseNMI && PPU.NMILow)
            {
                PPU.NMILow = false;
                TriggerNMI();
            }
        }

        public static void Initialize()
        {
            PC = Memory.ReadWord(ResetVector);
        }
        public static void CheckAndSetNegative(byte b)
        {
            if (b >> 7 == 1) // Check if sign bit is set.
                SetFlag(Flag.Negative);
            else
                ClearFlag(Flag.Negative);
        }
        public static void CheckAndSetZero(byte b)
        {
            if (b == 0)
                SetFlag(Flag.Zero);
            else
                ClearFlag(Flag.Zero);
        }
        public static string GetStatus()
        {
            return 
                $"   NV-BDIZC\r\nP=%{Convert.ToString(P, 2).PadLeft(8, '0')}\r\nS=${S:X2}\r\nPC=${PC:X2}\r\nY=${Y:X2}\r\nX=${X:X2}\r\nA=${A:X2}\r\nTotal Cycles={TotalCycles}";
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
        public static void Compare(byte b, byte b2)
        {
            byte cmp = (byte)(b - b2);
            CheckAndSetZero(cmp);
            CheckAndSetNegative(cmp);
            if (b >= b2)
                SetFlag(Flag.Carry);
            else
                ClearFlag(Flag.Carry);
        }
        public static string Disassemble(ushort address, ushort length)
        {
            string disasm = "";
            ushort pc = address;
            while (pc < address + length)
            {
                try
                {
                    Instruction ins = Instructions[Memory[pc]];
                    disasm += $"${pc.ToString("X4")}: ";
                    disasm += GetHexMemory(address, length);
                    if (ins.Length == 1)
                        disasm += "\t\t";
                    else if (ins.Length == 2)
                        disasm += "\t";
                    disasm += $"{ins.Code.GetMethodInfo().DeclaringType.Name} ";
                    if (ins.Length > 1)
                    {
                        switch (ins.Code.GetMethodInfo().Name)
                        {
                            case "Immediate":
                                disasm += $"#${(ins.Length == 2 ? Memory[pc + 1].ToString("X2") : Memory.ReadWordInternal(pc + 1).ToString("X4"))}";
                                break;
                            case "ZeroPage":
                            case "Relative":
                                disasm += $"${Memory[pc + 1].ToString("X2")}";
                                break;
                            case "ZeroPageX":
                                disasm += $"${Memory[pc + 1].ToString("X2")},X";
                                break;
                            case "ZeroPageY":
                                disasm += $"${Memory[pc + 1].ToString("X2")},Y";
                                break;
                            case "Absolute":
                                disasm += $"${Memory.ReadWordInternal(pc + 1).ToString("X4")}";
                                break;
                            case "AbsoluteX":
                                disasm += $"${Memory.ReadWordInternal(pc + 1).ToString("X4")},X";
                                break;
                            case "AbsoluteY":
                                disasm += $"${Memory.ReadWordInternal(pc + 1).ToString("X4")},Y";
                                break;
                            case "Indirect":
                                disasm += $"(${Memory.ReadWordInternal(pc + 1).ToString("X4")})";
                                break;
                            case "IndirectX":
                                disasm += $"(${Memory[pc + 1].ToString("X2")},X)";
                                break;
                            case "IndirectY":
                                disasm += $"(${Memory[pc + 1].ToString("X2")}),Y";
                                break;
                        }
                    }
                    else if (ins.Code.GetMethodInfo().Name == "Accumulator") disasm += "A";
                    disasm += Environment.NewLine;
                    pc += ins.Length;
                }
                catch { break; }
            }
            return disasm;
        }

        public static void PushToStack(byte b) => Memory[0x100 | S--] = b;

        public static void PushToStack16(ushort s)
        {
            PushToStack((byte)(s >>   8));
            PushToStack((byte)(s & 0xFF));
        }

        public static byte PopFromStack() => Memory[0x100 | ++S];

        public static ushort PopFromStack16() => (ushort)(PopFromStack() | (PopFromStack() << 8));

        public static void BranchTo(byte b)
        {
            PCModified = true;
            ushort oldPC = PC;
            if (b >> 7 == 0)
                PC += (ushort)(b + 2);
            else
                PC -= (ushort)(0xFF - b - 1);
            TotalCycles += (DoPagesDiffer(oldPC, PC) ? 2 : 1); // Add 1 cycle if branch is on the same page, otherwise 2.
        }
    }
}
