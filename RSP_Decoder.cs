using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N64_RSP_DISASSEMBLER
{
    class RSP_DECODER
    {
        // General-Purpose Registers
        private enum GP_REGISTER
        {
            r0, // Constant 0
            at, // Used for psuedo-instructions
            v0, v1, // Function returns
            a0, a1, a2, a3, // Function arguments
            t0, t1, t2, t3, t4, t5, t6, t7, // Temporary
            s0, s1, s2, s3, s4, s5, s6, s7, // Saved
            t8, t9, // More temporary
            k0, k1, // Reserved for kernal
            gp, // Global area pointer
            sp, // Stack pointer
            s8, // One more saved pointer
            ra // Return address
        }

        private enum CP0_REGISTER
        {
            DMA_CACHE,
            DMA_DRAM,
            DMA_READ_LENGTH,
            DMA_WRITE_LENGTH,
            SP_STATUS,
            DMA_FULL,
            DMA_BUSY,
            SP_RESERVED,
            CMD_START,
            CMD_END,
            CMD_CURRENT,
            CMD_STATUS,
            CMD_CLOCK,
            CMD_BUSY,
            CMD_PIPE_BUSY,
            CMD_TMEM_BUSY
        }

        private enum RSP_OPCODE
        {
            SPECIAL = 0x00, // 000000
            REGIMM = 0x01,  // 000001
            J = 0x02,       // 000010
            JAL = 0x03,     // 000011
            BEQ = 0x04,     // 000100
            BNE = 0x05,     // 000101
            BLEZ = 0x06,    // 000110
            BGTZ = 0x07,    // 000111
            ADDI = 0x08,    // 001000
            ADDIU = 0x09,   // 001001
            SLTI = 0x0A,    // 001010
            SLTIU = 0x0B,   // 001011
            ANDI = 0x0C,    // 001100
            ORI = 0x0D,     // 001101
            XORI = 0x0E,    // 001110
            LUI = 0x0F,     // 001111
            COP0 = 0x10,    // 010000
            COP2 = 0x12,    // 010010
            LB = 0x20,      // 100000
            LH = 0x21,      // 100001
            LW = 0x23,      // 100011
            LBU = 0x24,     // 100100
            LHU = 0x25,     // 100101
            LWU = 0x27,     // 100111
            SB = 0x28,      // 101000
            SH = 0x29,      // 101001
            SW = 0x2B,      // 101011
            LWC2 = 0x32,    // 110010
            SWC2 = 0x3A,    // 111010
        }
        
        private enum RSP_VECTOR_OPCODE
        {
            VMULF = 0x00, // Vector (Frac) Multiply
            VMULU = 0x01, // Vector (Unsigned Frac) Multiply
            VRNDP = 0x02, // Vector DCT Round (+)
            VMULQ = 0x03, // Vector (Integer) Multiply
            VMUDL = 0x04, // Vector low multiply
            VMUDM = 0x05, // Vector mid-m multiply
            VMUDN = 0x06, // Vector mid-n multiply
            VMUDH = 0x07, // Vector high multiply
            VMACF = 0x08, // Vector (Frac) Multiply Accumulate
            VMACU = 0x09, // Vector (Unsigned Frac) Multiply Accumulate
            VRNDN = 0x0A, // Vector DCT Round (-)
            VMACQ = 0x0B, // Vector (Integer) Multiply Accumulate
            VMADL = 0x0C, // Vector low multiply accumulate
            VMADM = 0x0D, // Vector mid-m multiply accumulate
            VMADN = 0x0E, // Vector mid-n multiply accumulate
            VMADH = 0x0F, // Vector high multiply accumulate
            VADD = 0x10,  // Vector add
            VSUB = 0x11,  // Vector subtract
            VABS = 0x13,  // Vector absolute value
            VADDC = 0x14, // Vector add with carry
            VSUBC = 0x15, // Vector subtract with carry
            VSAR = 0x1D,  // Vector accumulator read (and write)
            VLT = 0x20,   // Vector select (Less than)
            VEQ = 0x21,   // Vector select (Equal)
            VNE = 0x22,   // Vector select (Not equal)
            VGE = 0x23,   // Vector select (Greater than or equal)
            VCL = 0x24,   // Vector select clip (Test low)
            VCH = 0x25,   // Vector select clip (Test high)
            VCR = 0x26,   // Vector select crimp (Test low)
            VMRG = 0x27,  // Vector select merge
            VAND = 0x28,  // Vector AND
            VNAND = 0x29, // Vector NAND
            VOR = 0x2A,   // Vector OR
            VNOR = 0x2B,  // Vector NOR
            VXOR = 0x2C,  // Vector XOR
            VNXOR = 0x2D, // Vector NXOR
            VRCP = 0x30,  // Vector element scalar reciprocal (Single precision)
            VRCPL = 0x31, // Vector element scalar reciprocal (Double precision, Low)
            VRCPH = 0x32, // Vector element scalar reciprocal (Double precision, High)
            VMOV = 0x33,  // Vector element scalar move
            VRSQ = 0x34,  // Vector element scalar SQRT reciprocal
            VRSQL = 0x35, // Vector element scalar SQRT reciprocal (Double precision, Low)
            VRSQH = 0x36, // Vector element scalar SQRT reciprocal (Double precision, High)
            VNOP = 0x37,  // Vector null instruction
        }

        private enum RSP_LOAD_STORE_COMMAND
        {
            b = 0x00, // (BYTE)        00000
            s = 0x01, // (HALFWORD)    00001
            l = 0x02, // (WORD)        00010
            d = 0x03, // (DOUBLEWORD)  00011
            q = 0x04, // (QUADWORD)    00100
            r = 0x05, // (REST)        00101
            p = 0x06, // (PACKED)      00110
            u = 0x07, // (UNPACKED)    00111
            h = 0x08, // (HALF)        01000
            f = 0x09, // (FOURTH)      01001
            w = 0x0A, // (WRAP)        01010
            t = 0x0B, // (TRANSPOSE)   01011
        }

        private static bool usingRegNames = true;

        private static string getRPRegName(byte reg)
        {
            if (usingRegNames)
                return ((GP_REGISTER)reg).ToString();
            else
                return "r" + reg.ToString();
        }

        private static string getGRPRegName(GP_REGISTER reg)
        {
            if (usingRegNames)
                return reg.ToString();
            else
                return "r" + ((byte)reg).ToString();
        }

        private static string getCP0RegName(CP0_REGISTER reg)
        {
            //if (usingRegNames)
                return reg.ToString();
            //else
                //return "c" + ((byte)reg).ToString();
        }

        private static string decodeLoadStoreOperation(uint operation, string LorS)
        {
            RSP_LOAD_STORE_COMMAND ls_subop = 
                (RSP_LOAD_STORE_COMMAND)((operation >> 11) & 0x1F);
            string str = LorS + ls_subop.ToString() + "v";
            byte base_ = (byte)((operation >> 21) & 0x1F);
            byte dest = (byte)((operation >> 16) & 0x1F);
            byte del = (byte)((operation >> 7) & 0xF);
            ushort offset = (ushort)((operation & 0x3F) * 8);

            str += " $v" + dest + "[" + del + "], 0x" + offset.ToString("X4") + "(" + getRPRegName(base_) + ")";

            return str;
        }

        private static string decodeVectorElement(byte v, byte e)
        {
            if ((e & 0x8) == 8)
                return v + "[" + (e & 0x7) + "]";
            else if ((e & 0xC) == 4)
                return v + "[" + (e & 0x3) + "h]";
            else if ((e & 0xE) == 2)
                return v + "[" + (e & 0x1) + "q]";
            else
                return v.ToString();
        }

        private static string decodeVectorElementScalarOperation(RSP_VECTOR_OPCODE opcode, uint operation)
        {
            byte e = (byte)((operation >> 21) & 0xF);
            byte vt = (byte)((operation >> 16) & 0x1F);
            byte de = (byte)((operation >> 11) & 0x1F);
            byte vd = (byte)((operation >> 6) & 0x1F);
            
            // return opcode.ToString().ToLower() + " $v" + vd + "[" + de + "], $v" + vt + "[" + e + "]";
            return opcode.ToString().ToLower() + " $v" + decodeVectorElement(vd, de) 
                + ", $v" + decodeVectorElement(vt, e);
        }

        private static string decodeVectorOperation(uint operation)
        {
            RSP_VECTOR_OPCODE opcode = (RSP_VECTOR_OPCODE)(operation & 0x3F);

            switch(opcode)
            {
                case RSP_VECTOR_OPCODE.VRCP:
                case RSP_VECTOR_OPCODE.VRCPL:
                case RSP_VECTOR_OPCODE.VRCPH:
                case RSP_VECTOR_OPCODE.VMOV:
                case RSP_VECTOR_OPCODE.VRSQ:
                case RSP_VECTOR_OPCODE.VRSQL:
                case RSP_VECTOR_OPCODE.VRSQH:
                    return decodeVectorElementScalarOperation(opcode, operation);
                case RSP_VECTOR_OPCODE.VNOP:
                    return "vnop";
            }

            byte e = (byte)((operation >> 21) & 0xF);
            byte vt = (byte)((operation >> 16) & 0x1F);
            byte vs = (byte)((operation >> 11) & 0x1F);
            byte vd = (byte)((operation >> 6) & 0x1F);

            string element = "";
            /*
            | Vector         | $v1     | 0 0 0 0 | vector operand
            | Scalar Quarter | $v1[xq] | 0 0 1 x | 1 of 2 elements for 4 2-element quarters of vector
            | Scalar Half    | $v1[xh] | 0 1 x x | 1 of 4 elements for 2 4-element halves of vector
            | Scalar Whole   | $v1[x]  | 1 x x x | 1 of 8 elements for whole vector

            Look for the "Using Scalar Elements of a Vector Register" section in the 
            Ultra64 RSP programming manual for more information.
            */
            if (e != 0)
            {
                if ((e & 0xE) == 2)
                    element = "[" + (e & 0x1) + "q]";
                else if ((e & 0xC) == 4)
                    element = "[" + (e & 0x3) + "h]";
                else if ((e & 0x8) == 8)
                    element = "[" + (e & 0x7) + "]";
            }

            return opcode.ToString().ToLower() + " $v" + vd + ", $v" + vs + ", $v" + vt + element;
        }

        private static string decodeMoveControlToFromCoprocessorOperation(string opcode, uint operation)
        {
            GP_REGISTER rt = (GP_REGISTER)((operation >> 16) & 0x1F);
            byte rd = (byte)((operation >> 11) & 0x1F);
            
            return opcode + " " + getGRPRegName(rt) + ", $v" + rd;
        }

        private static string decodeMoveToFromCoprocessorOperation(string opcode, uint operation)
        {
            GP_REGISTER rt = (GP_REGISTER)((operation >> 16) & 0x1F);
            byte rd = (byte)((operation >> 11) & 0x1F);
            byte e = (byte)((operation >> 7) & 0xF);
            return opcode + " " + getGRPRegName(rt) + ", $v" + rd + "[" + e + "]";
        }

        private static string decodeCOP2Operation(uint operation)
        {
            if ((operation & 0x7FF) != 0)
                return decodeVectorOperation(operation);
            
            byte subop = (byte)((operation >> 21) & 0x1F);
            switch (subop)
            {
                case 0x00: // MFC2 operation
                    return decodeMoveToFromCoprocessorOperation("mfc2", operation);
                case 0x02: // CFC2 operation
                    return decodeMoveControlToFromCoprocessorOperation("cfc2", operation);
                case 0x04: // MTC2 operation
                    return decodeMoveToFromCoprocessorOperation("mtc2", operation);
                case 0x06: // CTC2 operation
                    return decodeMoveControlToFromCoprocessorOperation("ctc2", operation);
                default:
                    return "Unimplemented (COP2 opcode: " + Convert.ToString(subop, 2) + "b)"; ;
            }
        }

        private static string decodeNormalMIPSLoadStore(string opcode, uint operation)
        {
            GP_REGISTER dest = (GP_REGISTER)((operation >> 16) & 0x1F);
            GP_REGISTER base_ = (GP_REGISTER)((operation >> 21) & 0x1F);
            short imm = (short)(operation & 0xFFFF);
            
            if (imm < 0)
                opcode += " " + getGRPRegName(dest) + ", -0x" + (-imm).ToString("X4") + "(" + getGRPRegName(base_) + ")";
            else
                opcode += " " + getGRPRegName(dest) + ", 0x" + imm.ToString("X4") + "(" + getGRPRegName(base_) + ")";
            return opcode;
        }

        private static string decodeCOP0Operation(uint operation)
        {
            string str = "Unimplemented (COP0 operation)";
            byte mt = (byte)((operation >> 21) & 0x1F);
            GP_REGISTER rt = (GP_REGISTER)((operation >> 16) & 0x1F);
            CP0_REGISTER rd = (CP0_REGISTER)((operation >> 11) & 0x1F);

            switch (mt)
            {
                case 0x00: // MFC0
                    str = "mfc0 " + getGRPRegName(rt) + ", " + getCP0RegName(rd);
                    break;
                case 0x04: // MTC0
                    str = "mtc0 " + getGRPRegName(rt) + ", " + getCP0RegName(rd);
                    break;
            }

            return str;
        }

        private static string decodeOneRegisterWithImmediateOperation(string opcode, uint operation)
        {
            GP_REGISTER dest = (GP_REGISTER)((operation >> 16) & 0x1F);
            return opcode + " " + getGRPRegName(dest) + ", 0x" + (operation & 0xFFFF).ToString("X4");
        }

        private static string decodeTwoRegistersWithImmediateOperation(string opcode, uint operation)
        {
            GP_REGISTER dest = (GP_REGISTER)((operation >> 16) & 0x1F);
            GP_REGISTER src = (GP_REGISTER)((operation >> 21) & 0x1F);
            
            short imm = (short)(operation & 0xFFFF);
            if (imm < 0)
                return opcode + " " + getGRPRegName(dest) + ", " + getGRPRegName(src) + ", -0x" + (-imm).ToString("X4");
            else
                return opcode + " " + getGRPRegName(dest) + ", " + getGRPRegName(src) + ", 0x" + imm.ToString("X4");
        }

        private static string decodeThreeRegisterOperation(string opcode, uint operation, bool swapRT_RS)
        {
            GP_REGISTER dest = (GP_REGISTER)((operation >> 11) & 0x1F);
            GP_REGISTER src1 = (GP_REGISTER)((operation >> 21) & 0x1F);
            GP_REGISTER src2 = (GP_REGISTER)((operation >> 16) & 0x1F);
            if(!swapRT_RS)
                return opcode + " " + getGRPRegName(dest) + ", " + getGRPRegName(src1) + ", " + getGRPRegName(src2);
            else
                return opcode + " " + getGRPRegName(dest) + ", " + getGRPRegName(src2) + ", " + getGRPRegName(src1);
        }

        private static string decodeBranchOperation(string opcode, uint operation, uint address)
        {
            GP_REGISTER src = (GP_REGISTER)((operation >> 21) & 0x1F);
            short imm = (short)((operation & 0xFFFF) << 2);
            uint current_offset = (uint)((address + 4) + imm);
            return opcode + " " + getGRPRegName(src) + ", 0x" + (current_offset).ToString("X8");
        }

        private static string decodeBranchEqualsOperation(string opcode, uint operation, uint address)
        {
            GP_REGISTER src1 = (GP_REGISTER)((operation >> 21) & 0x1F);
            GP_REGISTER src2 = (GP_REGISTER)((operation >> 16) & 0x1F);
            short imm = (short)((operation & 0xFFFF) << 2);
            //Console.WriteLine("("+address.ToString("X8")+")immediate = " + imm);
            uint current_offset = (uint)((address + 4) + imm);
            return opcode + " " + getGRPRegName(src1) + ", " + getGRPRegName(src2) + ", 0x" + current_offset.ToString("X8");
        }

        private static string decodeSpecialShiftOperation(string opcode, uint operation)
        {
            GP_REGISTER dest = (GP_REGISTER)((operation >> 11) & 0x1F);
            GP_REGISTER src = (GP_REGISTER)((operation >> 16) & 0x1F);
            int imm = (int)((operation >> 6) & 0x1F);
            return opcode + " " + getGRPRegName(dest) + ", " + src.ToString() + ", " + imm;
        }
        
        public static string decodeOPERATION(uint operation, uint address, bool useRegNames)
        {
            usingRegNames = useRegNames;

            if (operation == 0x00000000)
                return "nop";

            RSP_OPCODE opcode = (RSP_OPCODE)((operation >> 26) & 0x3F);
            string str = "Unimplemented (opcode: " + Convert.ToString((int)opcode, 2) + "b)";
            switch (opcode)
            {
                case RSP_OPCODE.J:
                case RSP_OPCODE.JAL:
                    str = opcode.ToString().ToLower() + " 0x0" + ((operation & 0x03FFFFFF) << 2).ToString("X7");
                    break;
                case RSP_OPCODE.BEQ:
                case RSP_OPCODE.BNE:
                    str = decodeBranchEqualsOperation(opcode.ToString().ToLower(), operation, address);
                    break;
                case RSP_OPCODE.BLEZ:
                case RSP_OPCODE.BGTZ:
                    str = decodeBranchOperation(opcode.ToString().ToLower(), operation, address);
                    break;
                case RSP_OPCODE.ADDI:
                case RSP_OPCODE.ADDIU:
                case RSP_OPCODE.SLTI:
                case RSP_OPCODE.SLTIU:
                case RSP_OPCODE.ANDI:
                case RSP_OPCODE.ORI:
                case RSP_OPCODE.XORI:
                    str = decodeTwoRegistersWithImmediateOperation(opcode.ToString().ToLower(), operation);
                    break;
                case RSP_OPCODE.LUI:
                    str = decodeOneRegisterWithImmediateOperation(opcode.ToString().ToLower(), operation);
                    break;
                case RSP_OPCODE.COP0:
                    str = decodeCOP0Operation(operation);
                    break;
                case RSP_OPCODE.COP2:
                    {
                        //str = "Vector operation!";
                        str = decodeCOP2Operation(operation);
                    }
                    break;
                case RSP_OPCODE.LB:
                case RSP_OPCODE.LH:
                case RSP_OPCODE.LW:
                case RSP_OPCODE.LBU:
                case RSP_OPCODE.LHU:
                case RSP_OPCODE.LWU:
                case RSP_OPCODE.SB:
                case RSP_OPCODE.SH:
                case RSP_OPCODE.SW:
                    str = decodeNormalMIPSLoadStore(opcode.ToString().ToLower(), operation);
                    break;
                case RSP_OPCODE.LWC2:
                    {
                        str = decodeLoadStoreOperation(operation, "l");
                    }
                    break;
                case RSP_OPCODE.SWC2:
                    {
                        str = decodeLoadStoreOperation(operation, "s");
                    }
                    break;
                case RSP_OPCODE.REGIMM:
                    {
                        byte subop = (byte)((operation >> 16) & 0x1F);
                        switch (subop)
                        {
                            case 0x00: // BLTZ operation
                                str = decodeBranchOperation("bltz", operation, address);
                                break;
                            case 0x01: // BGEZ operation
                                str = decodeBranchOperation("bgez", operation, address);
                                break;
                            case 0x10: // BLTZAL operation
                                str = decodeBranchOperation("bltzal", operation, address);
                                break;
                            case 0x11: // BGEZAL operation
                                str = decodeBranchOperation("bgezal", operation, address);
                                break;
                        }
                    }
                    break;
                case RSP_OPCODE.SPECIAL:
                    {
                        byte subop = (byte)(operation & 0x3F);
                        switch (subop)
                        {
                            case 0x00: // SLL operation
                                str = decodeSpecialShiftOperation("sll", operation);
                                break;
                            case 0x02: // SRL operation
                                str = decodeSpecialShiftOperation("srl", operation);
                                break;
                            case 0x03: // SRA operation
                                str = decodeSpecialShiftOperation("sra", operation);
                                break;
                            case 0x04: // SLLV operation
                                str = decodeThreeRegisterOperation("sllv", operation, true);
                                break;
                            case 0x06: // SRLV operation
                                str = decodeThreeRegisterOperation("srlv", operation, true);
                                break;
                            case 0x07: // SRAV operation
                                str = decodeThreeRegisterOperation("srav", operation, true);
                                break;
                            case 0x08: // JR operation
                                str = "jr " + getGRPRegName(((GP_REGISTER)((operation >> 21) & 0x1F)));
                                break;
                            case 0x09: // JALR operation
                                {
                                    GP_REGISTER return_reg = (GP_REGISTER)((operation >> 11) & 0x1F);
                                    if(return_reg == GP_REGISTER.ra)
                                        str = "jalr " + getGRPRegName(((GP_REGISTER)((operation >> 21) & 0x1F)));
                                    else
                                        str = "jalr " + getGRPRegName(return_reg) + ", " + getGRPRegName(((GP_REGISTER)((operation >> 21) & 0x1F)));
                                }
                                break;
                            case 0x0D: // BREAK operation
                                str = "break " + ((operation >> 6) & 0xFFFFF);
                                break;
                            case 0x20: // ADD operation
                                str = decodeThreeRegisterOperation("add", operation, false);
                                break;
                            case 0x21: // ADDU operation
                                str = decodeThreeRegisterOperation("addu", operation, false);
                                break;
                            case 0x22: // SUB operation
                                str = decodeThreeRegisterOperation("sub", operation, false);
                                break;
                            case 0x23: // SUBU operation
                                str = decodeThreeRegisterOperation("subu", operation, false);
                                break;
                            case 0x24: // AND operation
                                str = decodeThreeRegisterOperation("and", operation, false);
                                break;
                            case 0x25: // OR operation
                                str = decodeThreeRegisterOperation("or", operation, false);
                                break;
                            case 0x26: // XOR operation
                                str = decodeThreeRegisterOperation("xor", operation, false);
                                break;
                            case 0x27: // NOR operation
                                str = decodeThreeRegisterOperation("nor", operation, false);
                                break;
                            case 0x2A: // SLT operation
                                str = decodeThreeRegisterOperation("slt", operation, false);
                                break;
                            case 0x2B: // SLTU operation
                                str = decodeThreeRegisterOperation("sltu", operation, false);
                                break;
                            default:
                                str = "Unimplemented (special: " + Convert.ToString(subop, 2) + "b)";
                                break;
                        }
                    }
                    break;
            }

            return str;
        }
    }
}
