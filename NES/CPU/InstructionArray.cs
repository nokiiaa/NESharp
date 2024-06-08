using NESharp.NES.Instructions;

namespace NESharp.NES
{
    public static partial class CPU
    {
        public static Instruction[] Instructions = new Instruction[]
        {
                new Instruction(1, 7, BRK.Execute),     // $00
                new Instruction(2, 6, ORA.IndirectX),   // $01
                null,                                   // $02
                new Instruction(2, 8, SLO.IndirectX),   // $03
                new Instruction(2, 4, NOP.Execute),     // $04
                new Instruction(2, 3, ORA.ZeroPage),    // $05
                new Instruction(2, 5, ASL.ZeroPage),    // $06
                new Instruction(2, 5, SLO.ZeroPage),    // $07
                new Instruction(1, 3, PHP.Execute),     // $08
                new Instruction(2, 2, ORA.Immediate),   // $09
                new Instruction(1, 2, ASL.Accumulator), // $0A
                new Instruction(2, 2, ANC.Immediate),   // $0B
                new Instruction(3, 4, IGN.Absolute),    // $0C
                new Instruction(3, 4, ORA.Absolute),    // $0D
                new Instruction(3, 6, ASL.Absolute),    // $0E
                new Instruction(3, 6, SLO.Absolute),    // $0F
                new Instruction(2, 2, BPL.ZeroPage),    // $10
                new Instruction(2, 5, ORA.IndirectY),   // $11
                null,                                   // $12
                new Instruction(2, 8, SLO.IndirectY),   // $13
                new Instruction(2, 4, NOP.Execute),     // $14
                new Instruction(2, 4, ORA.ZeroPageX),   // $15
                new Instruction(2, 6, ASL.ZeroPageX),   // $16
                new Instruction(2, 6, SLO.ZeroPageX),   // $17
                new Instruction(1, 2, CLC.Execute),     // $18
                new Instruction(3, 4, ORA.AbsoluteY),   // $19
                new Instruction(1, 2, NOP.Execute),     // $1A
                new Instruction(3, 7, SLO.AbsoluteY),   // $1B
                new Instruction(3, 4, IGN.AbsoluteX),   // $1C
                new Instruction(3, 4, ORA.AbsoluteX),   // $1D
                new Instruction(3, 7, ASL.AbsoluteX),   // $1E
                new Instruction(3, 7, SLO.AbsoluteX),   // $1F
                new Instruction(3, 6, JSR.Absolute),    // $20
                new Instruction(2, 6, AND.IndirectX),   // $21
                null,                                   // $22
                new Instruction(2, 8, RLA.IndirectX),   // $23
                new Instruction(2, 3, BIT.ZeroPage),    // $24
                new Instruction(2, 3, AND.ZeroPage),    // $25
                new Instruction(2, 5, ROL.ZeroPage),    // $26
                new Instruction(2, 5, RLA.ZeroPage),    // $27
                new Instruction(1, 4, PLP.Execute),     // $28
                new Instruction(2, 2, AND.Immediate),   // $29
                new Instruction(1, 2, ROL.Accumulator), // $2A
                new Instruction(2, 2, ANC.Immediate),   // $2B
                new Instruction(3, 4, BIT.Absolute),    // $2C
                new Instruction(3, 4, AND.Absolute),    // $2D
                new Instruction(3, 6, ROL.Absolute),    // $2E
                new Instruction(3, 6, RLA.Absolute),    // $2F
                new Instruction(2, 2, BMI.ZeroPage),    // $30 
                new Instruction(2, 5, AND.IndirectY),   // $31
                null,                                   // $32
                new Instruction(2, 8, RLA.IndirectY),   // $33
                new Instruction(2, 4, NOP.Execute),     // $34
                new Instruction(2, 4, AND.ZeroPageX),   // $35
                new Instruction(2, 6, ROL.ZeroPageX),   // $36
                new Instruction(2, 6, RLA.ZeroPageX),   // $37
                new Instruction(1, 2, SEC.Execute),     // $38
                new Instruction(3, 4, AND.AbsoluteY),   // $39
                new Instruction(1, 2, NOP.Execute),     // $3A
                new Instruction(3, 7, RLA.AbsoluteY),   // $3B
                new Instruction(3, 4, IGN.AbsoluteX),   // $3C
                new Instruction(3, 4, AND.AbsoluteX),   // $3D
                new Instruction(3, 7, ROL.AbsoluteX),   // $3E
                new Instruction(3, 7, RLA.AbsoluteX),   // $3F
                new Instruction(1, 6, RTI.Execute),     // $40
                new Instruction(2, 6, EOR.IndirectX),   // $41
                null,                                   // $42
                new Instruction(2, 8, SRE.IndirectX),   // $43
                new Instruction(2, 3, NOP.Execute),     // $44
                new Instruction(2, 3, EOR.ZeroPage),    // $45
                new Instruction(2, 5, LSR.ZeroPage),    // $46
                new Instruction(2, 5, SRE.ZeroPage),    // $47
                new Instruction(1, 3, PHA.Execute),     // $48
                new Instruction(2, 2, EOR.Immediate),   // $49
                new Instruction(1, 2, LSR.Accumulator), // $4A
                new Instruction(2, 2, ALR.Immediate),   // $4B
                new Instruction(3, 3, JMP.Absolute),    // $4C
                new Instruction(3, 4, EOR.Absolute),    // $4D
                new Instruction(3, 6, LSR.Absolute),    // $4E
                new Instruction(3, 6, SRE.Absolute),    // $4F
                new Instruction(2, 2, BVC.ZeroPage),    // $50
                new Instruction(2, 5, EOR.IndirectY),   // $51
                null,                                   // $52
                new Instruction(2, 8, SRE.IndirectY),   // $53
                new Instruction(2, 4, NOP.Execute),     // $54
                new Instruction(2, 4, EOR.ZeroPageX),   // $55
                new Instruction(2, 6, LSR.ZeroPageX),   // $56
                new Instruction(2, 6, SRE.ZeroPageX),   // $57
                new Instruction(1, 2, CLI.Execute),     // $58
                new Instruction(3, 4, EOR.AbsoluteY),   // $59
                new Instruction(1, 2, NOP.Execute),     // $5A
                new Instruction(3, 7, SRE.AbsoluteY),   // $5B
                new Instruction(3, 4, IGN.AbsoluteX),   // $5C
                new Instruction(3, 4, EOR.AbsoluteX),   // $5D
                new Instruction(3, 7, LSR.AbsoluteX),   // $5E
                new Instruction(3, 7, SRE.AbsoluteX),   // $5F
                new Instruction(1, 6, RTS.Execute),     // $60
                new Instruction(2, 6, ADC.IndirectX),   // $61
                null,                                   // $62
                new Instruction(2, 8, RRA.IndirectX),   // $63
                new Instruction(2, 3, NOP.Execute),     // $64
                new Instruction(2, 3, ADC.ZeroPage),    // $65
                new Instruction(2, 5, ROR.ZeroPage),    // $66
                new Instruction(2, 5, RRA.ZeroPage),    // $67
                new Instruction(1, 4, PLA.Execute),     // $68
                new Instruction(2, 2, ADC.Immediate),   // $69
                new Instruction(1, 2, ROR.Accumulator), // $6A
                new Instruction(2, 2, ARR.Immediate),   // $6B
                new Instruction(3, 5, JMP.Indirect),    // $6C
                new Instruction(3, 4, ADC.Absolute),    // $6D
                new Instruction(3, 7, ROR.Absolute),    // $6E
                new Instruction(3, 6, RRA.Absolute),    // $6F
                new Instruction(2, 2, BVS.ZeroPage),    // $70
                new Instruction(2, 5, ADC.IndirectY),   // $71
                null,                                   // $72 
                new Instruction(2, 8, RRA.IndirectY),   // $73
                new Instruction(2, 4, NOP.Execute),     // $74
                new Instruction(2, 4, ADC.ZeroPageX),   // $75
                new Instruction(2, 6, ROR.ZeroPageX),   // $76
                new Instruction(2, 6, RRA.ZeroPageX),   // $77
                new Instruction(1, 2, SEI.Execute),     // $78
                new Instruction(3, 4, ADC.AbsoluteY),   // $79
                new Instruction(1, 2, NOP.Execute),     // $7A
                new Instruction(3, 7, RRA.AbsoluteY),   // $7B
                new Instruction(3, 4, IGN.AbsoluteX),   // $7C
                new Instruction(3, 4, ADC.AbsoluteX),   // $7D
                new Instruction(3, 6, ROR.AbsoluteX),   // $7E
                new Instruction(3, 7, RRA.AbsoluteX),   // $7F
                new Instruction(2, 2, SKB.Immediate),   // $80
                new Instruction(2, 6, STA.IndirectX),   // $81
                new Instruction(2, 2, SKB.Immediate),   // $82
                new Instruction(2, 6, SAX.IndirectX),   // $83
                new Instruction(2, 3, STY.ZeroPage),    // $84
                new Instruction(2, 3, STA.ZeroPage),    // $85
                new Instruction(2, 3, STX.ZeroPage),    // $86
                new Instruction(2, 3, SAX.ZeroPage),    // $87
                new Instruction(1, 2, DEY.Execute),     // $88
                new Instruction(2, 2, SKB.Immediate),   // $89
                new Instruction(1, 2, TXA.Execute),     // $8A
                null,                                   // $8B   
                new Instruction(3, 4, STY.Absolute),    // $8C
                new Instruction(3, 4, STA.Absolute),    // $8D
                new Instruction(3, 4, STX.Absolute),    // $8E
                new Instruction(3, 4, SAX.Absolute),    // $8F
                new Instruction(2, 2, BCC.ZeroPage),    // $90
                new Instruction(2, 6, STA.IndirectY),   // $91
                null,                                   // $92
                null,                                   // $93
                new Instruction(2, 4, STY.ZeroPageX),   // $94
                new Instruction(2, 4, STA.ZeroPageX),   // $95
                new Instruction(2, 4, STX.ZeroPageY),   // $96
                new Instruction(2, 4, SAX.ZeroPageY),   // $97
                new Instruction(1, 2, TYA.Execute),     // $98
                new Instruction(3, 5, STA.AbsoluteY),   // $99
                new Instruction(1, 2, TXS.Execute),     // $9A
                null,                                   // $9B
                null,                                   // $9C
                new Instruction(3, 5, STA.AbsoluteX),   // $9D
                null,                                   // $9E
                null,                                   // $9F
                new Instruction(2, 2, LDY.Immediate),   // $A0
                new Instruction(2, 6, LDA.IndirectX),   // $A1
                new Instruction(2, 2, LDX.Immediate),   // $A2
                new Instruction(2, 6, LAX.IndirectX),   // $A3
                new Instruction(2, 3, LDY.ZeroPage),    // $A4
                new Instruction(2, 3, LDA.ZeroPage),    // $A5
                new Instruction(2, 3, LDX.ZeroPage),    // $A6
                new Instruction(2, 3, LAX.ZeroPage),    // $A7
                new Instruction(1, 2, TAY.Execute),     // $A8
                new Instruction(2, 2, LDA.Immediate),   // $A9
                new Instruction(1, 2, TAX.Execute),     // $AA 
                null,                                   // $AB 
                new Instruction(3, 4, LDY.Absolute),    // $AC
                new Instruction(3, 4, LDA.Absolute),    // $AD
                new Instruction(3, 4, LDX.Absolute),    // $AE
                new Instruction(3, 4, LAX.Absolute),    // $AF
                new Instruction(2, 2, BCS.ZeroPage),    // $B0
                new Instruction(2, 5, LDA.IndirectY),   // $B1
                null,                                   // $B2
                new Instruction(2, 5, LAX.IndirectY),   // $B3
                new Instruction(2, 4, LDY.ZeroPageX),   // $B4
                new Instruction(2, 4, LDA.ZeroPageX),   // $B5
                new Instruction(2, 4, LDX.ZeroPageY),   // $B6
                new Instruction(2, 4, LAX.ZeroPageY),   // $B7
                new Instruction(1, 2, CLV.Execute),     // $B8
                new Instruction(3, 4, LDA.AbsoluteY),   // $B9
                new Instruction(1, 2, TSX.Execute),     // $BA
                null,                                   // $BB
                new Instruction(3, 4, LDY.AbsoluteX),   // $BC
                new Instruction(3, 4, LDA.AbsoluteX),   // $BD
                new Instruction(3, 4, LDX.AbsoluteY),   // $BE
                new Instruction(3, 4, LAX.AbsoluteY),   // $BF
                new Instruction(2, 2, CPY.Immediate),   // $C0
                new Instruction(2, 6, CMP.IndirectX),   // $C1
                new Instruction(2, 2, SKB.Immediate),   // $C2
                new Instruction(2, 8, DCP.IndirectX),   // $C3
                new Instruction(2, 3, CPY.ZeroPage),    // $C4
                new Instruction(2, 3, CMP.ZeroPage),    // $C5
                new Instruction(2, 5, DEC.ZeroPage),    // $C6
                new Instruction(2, 5, DCP.ZeroPage),    // $C7
                new Instruction(1, 2, INY.Execute),     // $C8
                new Instruction(2, 2, CMP.Immediate),   // $C9
                new Instruction(1, 2, DEX.Execute),     // $CA
                new Instruction(2, 2, AXS.Immediate),   // $CB
                new Instruction(3, 4, CPY.Absolute),    // $CC
                new Instruction(3, 4, CMP.Absolute),    // $CD
                new Instruction(3, 3, DEC.Absolute),    // $CE
                new Instruction(3, 6, DCP.Absolute),    // $CF
                new Instruction(2, 2, BNE.ZeroPage),    // $D0
                new Instruction(2, 5, CMP.IndirectY),   // $D1
                null,                                   // $D2
                new Instruction(2, 8, DCP.IndirectY),   // $D3
                new Instruction(2, 4, NOP.Execute),     // $D4
                new Instruction(2, 4, CMP.ZeroPageX),   // $D5
                new Instruction(2, 6, DEC.ZeroPageX),   // $D6
                new Instruction(2, 6, DCP.ZeroPageX),   // $D7
                new Instruction(1, 2, CLD.Execute),     // $D8
                new Instruction(3, 4, CMP.AbsoluteY),   // $D9
                new Instruction(1, 2, NOP.Execute),     // $DA
                new Instruction(3, 7, DCP.AbsoluteY),   // $DB
                new Instruction(3, 4, IGN.AbsoluteX),   // $DC
                new Instruction(3, 4, CMP.AbsoluteX),   // $DD
                new Instruction(3, 7, DEC.AbsoluteX),   // $DE
                new Instruction(2, 7, DCP.ZeroPageX),   // $DF
                new Instruction(2, 2, CPX.Immediate),   // $E0
                new Instruction(2, 6, SBC.IndirectX),   // $E1
                new Instruction(2, 2, SKB.Immediate),   // $E2
                new Instruction(2, 8, ISC.IndirectX),   // $E3
                new Instruction(2, 3, CPX.ZeroPage),    // $E4
                new Instruction(2, 3, SBC.ZeroPage),    // $E5
                new Instruction(2, 5, INC.ZeroPage),    // $E6
                new Instruction(2, 5, ISC.ZeroPage),    // $E7
                new Instruction(1, 2, INX.Execute),     // $E8
                new Instruction(2, 2, SBC.Immediate),   // $E9
                new Instruction(1, 2, NOP.Execute),     // $EA
                new Instruction(2, 2, SBC.Immediate),   // $EB
                new Instruction(3, 4, CPX.Absolute),    // $EC
                new Instruction(3, 4, SBC.Absolute),    // $ED
                new Instruction(3, 6, INC.Absolute),    // $EE 
                new Instruction(3, 6, ISC.Absolute),    // $EF
                new Instruction(2, 2, BEQ.ZeroPage),    // $F0
                new Instruction(2, 5, SBC.IndirectY),   // $F1
                null,                                   // $F2
                new Instruction(2, 8, ISC.IndirectY),   // $F3
                new Instruction(2, 4, NOP.Execute),     // $F4
                new Instruction(2, 4, SBC.ZeroPageX),   // $F5
                new Instruction(2, 6, INC.ZeroPageX),   // $F6
                new Instruction(2, 6, ISC.ZeroPageX),   // $F7
                new Instruction(1, 2, SED.Execute),     // $F8
                new Instruction(3, 4, SBC.AbsoluteY),   // $F9
                new Instruction(1, 2, NOP.Execute),     // $FA
                new Instruction(3, 7, ISC.AbsoluteY),   // $FB
                new Instruction(3, 4, IGN.AbsoluteX),   // $FC
                new Instruction(3, 4, SBC.AbsoluteX),   // $FD
                new Instruction(3, 7, INC.AbsoluteX),   // $FE
                new Instruction(3, 7, ISC.AbsoluteX),   // $FF
        };
    }
}
