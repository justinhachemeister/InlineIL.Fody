using System.Diagnostics.CodeAnalysis;

namespace InlineIL
{
    /// <summary>
    /// IL OpCodes for use with <see cref="IL.Emit(OpCodes)"/>
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum OpCodes
    {
        /// <summary>
        /// nop
        /// </summary>
        Nop = 0,

        /// <summary>
        /// break
        /// </summary>
        Break = 1,

        /// <summary>
        /// ldarg.0
        /// </summary>
        Ldarg_0 = 2,

        /// <summary>
        /// ldarg.1
        /// </summary>
        Ldarg_1 = 3,

        /// <summary>
        /// ldarg.2
        /// </summary>
        Ldarg_2 = 4,

        /// <summary>
        /// ldarg.3
        /// </summary>
        Ldarg_3 = 5,

        /// <summary>
        /// ldloc.0
        /// </summary>
        Ldloc_0 = 6,

        /// <summary>
        /// ldloc.1
        /// </summary>
        Ldloc_1 = 7,

        /// <summary>
        /// ldloc.2
        /// </summary>
        Ldloc_2 = 8,

        /// <summary>
        /// ldloc.3
        /// </summary>
        Ldloc_3 = 9,

        /// <summary>
        /// stloc.0
        /// </summary>
        Stloc_0 = 10,

        /// <summary>
        /// stloc.1
        /// </summary>
        Stloc_1 = 11,

        /// <summary>
        /// stloc.2
        /// </summary>
        Stloc_2 = 12,

        /// <summary>
        /// stloc.3
        /// </summary>
        Stloc_3 = 13,

        /// <summary>
        /// ldarg.s
        /// </summary>
        Ldarg_S = 14,

        /// <summary>
        /// ldarga.s
        /// </summary>
        Ldarga_S = 15,

        /// <summary>
        /// starg.s
        /// </summary>
        Starg_S = 16,

        /// <summary>
        /// ldloc.s
        /// </summary>
        Ldloc_S = 17,

        /// <summary>
        /// ldloca.s
        /// </summary>
        Ldloca_S = 18,

        /// <summary>
        /// stloc.s
        /// </summary>
        Stloc_S = 19,

        /// <summary>
        /// ldnull
        /// </summary>
        Ldnull = 20,

        /// <summary>
        /// ldc.i4.m1
        /// </summary>
        Ldc_I4_M1 = 21,

        /// <summary>
        /// ldc.i4.0
        /// </summary>
        Ldc_I4_0 = 22,

        /// <summary>
        /// ldc.i4.1
        /// </summary>
        Ldc_I4_1 = 23,

        /// <summary>
        /// ldc.i4.2
        /// </summary>
        Ldc_I4_2 = 24,

        /// <summary>
        /// ldc.i4.3
        /// </summary>
        Ldc_I4_3 = 25,

        /// <summary>
        /// ldc.i4.4
        /// </summary>
        Ldc_I4_4 = 26,

        /// <summary>
        /// ldc.i4.5
        /// </summary>
        Ldc_I4_5 = 27,

        /// <summary>
        /// ldc.i4.6
        /// </summary>
        Ldc_I4_6 = 28,

        /// <summary>
        /// ldc.i4.7
        /// </summary>
        Ldc_I4_7 = 29,

        /// <summary>
        /// ldc.i4.8
        /// </summary>
        Ldc_I4_8 = 30,

        /// <summary>
        /// ldc.i4.s
        /// </summary>
        Ldc_I4_S = 31,

        /// <summary>
        /// ldc.i4
        /// </summary>
        Ldc_I4 = 32,

        /// <summary>
        /// ldc.i8
        /// </summary>
        Ldc_I8 = 33,

        /// <summary>
        /// ldc.r4
        /// </summary>
        Ldc_R4 = 34,

        /// <summary>
        /// ldc.r8
        /// </summary>
        Ldc_R8 = 35,

        /// <summary>
        /// dup
        /// </summary>
        Dup = 37,

        /// <summary>
        /// pop
        /// </summary>
        Pop = 38,

        /// <summary>
        /// jmp
        /// </summary>
        Jmp = 39,

        /// <summary>
        /// call
        /// </summary>
        Call = 40,

        /// <summary>
        /// calli
        /// </summary>
        Calli = 41,

        /// <summary>
        /// ret
        /// </summary>
        Ret = 42,

        /// <summary>
        /// br.s
        /// </summary>
        Br_S = 43,

        /// <summary>
        /// brfalse.s
        /// </summary>
        Brfalse_S = 44,

        /// <summary>
        /// brtrue.s
        /// </summary>
        Brtrue_S = 45,

        /// <summary>
        /// beq.s
        /// </summary>
        Beq_S = 46,

        /// <summary>
        /// bge.s
        /// </summary>
        Bge_S = 47,

        /// <summary>
        /// bgt.s
        /// </summary>
        Bgt_S = 48,

        /// <summary>
        /// ble.s
        /// </summary>
        Ble_S = 49,

        /// <summary>
        /// blt.s
        /// </summary>
        Blt_S = 50,

        /// <summary>
        /// bne.un.s
        /// </summary>
        Bne_Un_S = 51,

        /// <summary>
        /// bge.un.s
        /// </summary>
        Bge_Un_S = 52,

        /// <summary>
        /// bgt.un.s
        /// </summary>
        Bgt_Un_S = 53,

        /// <summary>
        /// ble.un.s
        /// </summary>
        Ble_Un_S = 54,

        /// <summary>
        /// blt.un.s
        /// </summary>
        Blt_Un_S = 55,

        /// <summary>
        /// br
        /// </summary>
        Br = 56,

        /// <summary>
        /// brfalse
        /// </summary>
        Brfalse = 57,

        /// <summary>
        /// brtrue
        /// </summary>
        Brtrue = 58,

        /// <summary>
        /// beq
        /// </summary>
        Beq = 59,

        /// <summary>
        /// bge
        /// </summary>
        Bge = 60,

        /// <summary>
        /// bgt
        /// </summary>
        Bgt = 61,

        /// <summary>
        /// ble
        /// </summary>
        Ble = 62,

        /// <summary>
        /// blt
        /// </summary>
        Blt = 63,

        /// <summary>
        /// bne.un
        /// </summary>
        Bne_Un = 64,

        /// <summary>
        /// bge.un
        /// </summary>
        Bge_Un = 65,

        /// <summary>
        /// bgt.un
        /// </summary>
        Bgt_Un = 66,

        /// <summary>
        /// ble.un
        /// </summary>
        Ble_Un = 67,

        /// <summary>
        /// blt.un
        /// </summary>
        Blt_Un = 68,

        /// <summary>
        /// switch
        /// </summary>
        Switch = 69,

        /// <summary>
        /// ldind.i1
        /// </summary>
        Ldind_I1 = 70,

        /// <summary>
        /// ldind.u1
        /// </summary>
        Ldind_U1 = 71,

        /// <summary>
        /// ldind.i2
        /// </summary>
        Ldind_I2 = 72,

        /// <summary>
        /// ldind.u2
        /// </summary>
        Ldind_U2 = 73,

        /// <summary>
        /// ldind.i4
        /// </summary>
        Ldind_I4 = 74,

        /// <summary>
        /// ldind.u4
        /// </summary>
        Ldind_U4 = 75,

        /// <summary>
        /// ldind.i8
        /// </summary>
        Ldind_I8 = 76,

        /// <summary>
        /// ldind.i
        /// </summary>
        Ldind_I = 77,

        /// <summary>
        /// ldind.r4
        /// </summary>
        Ldind_R4 = 78,

        /// <summary>
        /// ldind.r8
        /// </summary>
        Ldind_R8 = 79,

        /// <summary>
        /// ldind.ref
        /// </summary>
        Ldind_Ref = 80,

        /// <summary>
        /// stind.ref
        /// </summary>
        Stind_Ref = 81,

        /// <summary>
        /// stind.i1
        /// </summary>
        Stind_I1 = 82,

        /// <summary>
        /// stind.i2
        /// </summary>
        Stind_I2 = 83,

        /// <summary>
        /// stind.i4
        /// </summary>
        Stind_I4 = 84,

        /// <summary>
        /// stind.i8
        /// </summary>
        Stind_I8 = 85,

        /// <summary>
        /// stind.r4
        /// </summary>
        Stind_R4 = 86,

        /// <summary>
        /// stind.r8
        /// </summary>
        Stind_R8 = 87,

        /// <summary>
        /// add
        /// </summary>
        Add = 88,

        /// <summary>
        /// sub
        /// </summary>
        Sub = 89,

        /// <summary>
        /// mul
        /// </summary>
        Mul = 90,

        /// <summary>
        /// div
        /// </summary>
        Div = 91,

        /// <summary>
        /// div.un
        /// </summary>
        Div_Un = 92,

        /// <summary>
        /// rem
        /// </summary>
        Rem = 93,

        /// <summary>
        /// rem.un
        /// </summary>
        Rem_Un = 94,

        /// <summary>
        /// and
        /// </summary>
        And = 95,

        /// <summary>
        /// or
        /// </summary>
        Or = 96,

        /// <summary>
        /// xor
        /// </summary>
        Xor = 97,

        /// <summary>
        /// shl
        /// </summary>
        Shl = 98,

        /// <summary>
        /// shr
        /// </summary>
        Shr = 99,

        /// <summary>
        /// shr.un
        /// </summary>
        Shr_Un = 100,

        /// <summary>
        /// neg
        /// </summary>
        Neg = 101,

        /// <summary>
        /// not
        /// </summary>
        Not = 102,

        /// <summary>
        /// conv.i1
        /// </summary>
        Conv_I1 = 103,

        /// <summary>
        /// conv.i2
        /// </summary>
        Conv_I2 = 104,

        /// <summary>
        /// conv.i4
        /// </summary>
        Conv_I4 = 105,

        /// <summary>
        /// conv.i8
        /// </summary>
        Conv_I8 = 106,

        /// <summary>
        /// conv.r4
        /// </summary>
        Conv_R4 = 107,

        /// <summary>
        /// conv.r8
        /// </summary>
        Conv_R8 = 108,

        /// <summary>
        /// conv.u4
        /// </summary>
        Conv_U4 = 109,

        /// <summary>
        /// conv.u8
        /// </summary>
        Conv_U8 = 110,

        /// <summary>
        /// callvirt
        /// </summary>
        Callvirt = 111,

        /// <summary>
        /// cpobj
        /// </summary>
        Cpobj = 112,

        /// <summary>
        /// ldobj
        /// </summary>
        Ldobj = 113,

        /// <summary>
        /// ldstr
        /// </summary>
        Ldstr = 114,

        /// <summary>
        /// newobj
        /// </summary>
        Newobj = 115,

        /// <summary>
        /// castclass
        /// </summary>
        Castclass = 116,

        /// <summary>
        /// isinst
        /// </summary>
        Isinst = 117,

        /// <summary>
        /// conv.r.un
        /// </summary>
        Conv_R_Un = 118,

        /// <summary>
        /// unbox
        /// </summary>
        Unbox = 121,

        /// <summary>
        /// throw
        /// </summary>
        Throw = 122,

        /// <summary>
        /// ldfld
        /// </summary>
        Ldfld = 123,

        /// <summary>
        /// ldflda
        /// </summary>
        Ldflda = 124,

        /// <summary>
        /// stfld
        /// </summary>
        Stfld = 125,

        /// <summary>
        /// ldsfld
        /// </summary>
        Ldsfld = 126,

        /// <summary>
        /// ldsflda
        /// </summary>
        Ldsflda = 127,

        /// <summary>
        /// stsfld
        /// </summary>
        Stsfld = 128,

        /// <summary>
        /// stobj
        /// </summary>
        Stobj = 129,

        /// <summary>
        /// conv.ovf.i1.un
        /// </summary>
        Conv_Ovf_I1_Un = 130,

        /// <summary>
        /// conv.ovf.i2.un
        /// </summary>
        Conv_Ovf_I2_Un = 131,

        /// <summary>
        /// conv.ovf.i4.un
        /// </summary>
        Conv_Ovf_I4_Un = 132,

        /// <summary>
        /// conv.ovf.i8.un
        /// </summary>
        Conv_Ovf_I8_Un = 133,

        /// <summary>
        /// conv.ovf.u1.un
        /// </summary>
        Conv_Ovf_U1_Un = 134,

        /// <summary>
        /// conv.ovf.u2.un
        /// </summary>
        Conv_Ovf_U2_Un = 135,

        /// <summary>
        /// conv.ovf.u4.un
        /// </summary>
        Conv_Ovf_U4_Un = 136,

        /// <summary>
        /// conv.ovf.u8.un
        /// </summary>
        Conv_Ovf_U8_Un = 137,

        /// <summary>
        /// conv.ovf.i.un
        /// </summary>
        Conv_Ovf_I_Un = 138,

        /// <summary>
        /// conv.ovf.u.un
        /// </summary>
        Conv_Ovf_U_Un = 139,

        /// <summary>
        /// box
        /// </summary>
        Box = 140,

        /// <summary>
        /// newarr
        /// </summary>
        Newarr = 141,

        /// <summary>
        /// ldlen
        /// </summary>
        Ldlen = 142,

        /// <summary>
        /// ldelema
        /// </summary>
        Ldelema = 143,

        /// <summary>
        /// ldelem.i1
        /// </summary>
        Ldelem_I1 = 144,

        /// <summary>
        /// ldelem.u1
        /// </summary>
        Ldelem_U1 = 145,

        /// <summary>
        /// ldelem.i2
        /// </summary>
        Ldelem_I2 = 146,

        /// <summary>
        /// ldelem.u2
        /// </summary>
        Ldelem_U2 = 147,

        /// <summary>
        /// ldelem.i4
        /// </summary>
        Ldelem_I4 = 148,

        /// <summary>
        /// ldelem.u4
        /// </summary>
        Ldelem_U4 = 149,

        /// <summary>
        /// ldelem.i8
        /// </summary>
        Ldelem_I8 = 150,

        /// <summary>
        /// ldelem.i
        /// </summary>
        Ldelem_I = 151,

        /// <summary>
        /// ldelem.r4
        /// </summary>
        Ldelem_R4 = 152,

        /// <summary>
        /// ldelem.r8
        /// </summary>
        Ldelem_R8 = 153,

        /// <summary>
        /// ldelem.ref
        /// </summary>
        Ldelem_Ref = 154,

        /// <summary>
        /// stelem.i
        /// </summary>
        Stelem_I = 155,

        /// <summary>
        /// stelem.i1
        /// </summary>
        Stelem_I1 = 156,

        /// <summary>
        /// stelem.i2
        /// </summary>
        Stelem_I2 = 157,

        /// <summary>
        /// stelem.i4
        /// </summary>
        Stelem_I4 = 158,

        /// <summary>
        /// stelem.i8
        /// </summary>
        Stelem_I8 = 159,

        /// <summary>
        /// stelem.r4
        /// </summary>
        Stelem_R4 = 160,

        /// <summary>
        /// stelem.r8
        /// </summary>
        Stelem_R8 = 161,

        /// <summary>
        /// stelem.ref
        /// </summary>
        Stelem_Ref = 162,

        /// <summary>
        /// ldelem.any
        /// </summary>
        Ldelem_Any = 163,

        /// <summary>
        /// stelem.any
        /// </summary>
        Stelem_Any = 164,

        /// <summary>
        /// unbox.any
        /// </summary>
        Unbox_Any = 165,

        /// <summary>
        /// conv.ovf.i1
        /// </summary>
        Conv_Ovf_I1 = 179,

        /// <summary>
        /// conv.ovf.u1
        /// </summary>
        Conv_Ovf_U1 = 180,

        /// <summary>
        /// conv.ovf.i2
        /// </summary>
        Conv_Ovf_I2 = 181,

        /// <summary>
        /// conv.ovf.u2
        /// </summary>
        Conv_Ovf_U2 = 182,

        /// <summary>
        /// conv.ovf.i4
        /// </summary>
        Conv_Ovf_I4 = 183,

        /// <summary>
        /// conv.ovf.u4
        /// </summary>
        Conv_Ovf_U4 = 184,

        /// <summary>
        /// conv.ovf.i8
        /// </summary>
        Conv_Ovf_I8 = 185,

        /// <summary>
        /// conv.ovf.u8
        /// </summary>
        Conv_Ovf_U8 = 186,

        /// <summary>
        /// refanyval
        /// </summary>
        Refanyval = 194,

        /// <summary>
        /// ckfinite
        /// </summary>
        Ckfinite = 195,

        /// <summary>
        /// mkrefany
        /// </summary>
        Mkrefany = 198,

        /// <summary>
        /// ldtoken
        /// </summary>
        Ldtoken = 208,

        /// <summary>
        /// conv.u2
        /// </summary>
        Conv_U2 = 209,

        /// <summary>
        /// conv.u1
        /// </summary>
        Conv_U1 = 210,

        /// <summary>
        /// conv.i
        /// </summary>
        Conv_I = 211,

        /// <summary>
        /// conv.ovf.i
        /// </summary>
        Conv_Ovf_I = 212,

        /// <summary>
        /// conv.ovf.u
        /// </summary>
        Conv_Ovf_U = 213,

        /// <summary>
        /// add.ovf
        /// </summary>
        Add_Ovf = 214,

        /// <summary>
        /// add.ovf.un
        /// </summary>
        Add_Ovf_Un = 215,

        /// <summary>
        /// mul.ovf
        /// </summary>
        Mul_Ovf = 216,

        /// <summary>
        /// mul.ovf.un
        /// </summary>
        Mul_Ovf_Un = 217,

        /// <summary>
        /// sub.ovf
        /// </summary>
        Sub_Ovf = 218,

        /// <summary>
        /// sub.ovf.un
        /// </summary>
        Sub_Ovf_Un = 219,

        /// <summary>
        /// endfinally
        /// </summary>
        Endfinally = 220,

        /// <summary>
        /// leave
        /// </summary>
        Leave = 221,

        /// <summary>
        /// leave.s
        /// </summary>
        Leave_S = 222,

        /// <summary>
        /// stind.i
        /// </summary>
        Stind_I = 223,

        /// <summary>
        /// conv.u
        /// </summary>
        Conv_U = 224,

        /// <summary>
        /// arglist
        /// </summary>
        Arglist = -512,

        /// <summary>
        /// ceq
        /// </summary>
        Ceq = -511,

        /// <summary>
        /// cgt
        /// </summary>
        Cgt = -510,

        /// <summary>
        /// cgt.un
        /// </summary>
        Cgt_Un = -509,

        /// <summary>
        /// clt
        /// </summary>
        Clt = -508,

        /// <summary>
        /// clt.un
        /// </summary>
        Clt_Un = -507,

        /// <summary>
        /// ldftn
        /// </summary>
        Ldftn = -506,

        /// <summary>
        /// ldvirtftn
        /// </summary>
        Ldvirtftn = -505,

        /// <summary>
        /// ldarg
        /// </summary>
        Ldarg = -503,

        /// <summary>
        /// ldarga
        /// </summary>
        Ldarga = -502,

        /// <summary>
        /// starg
        /// </summary>
        Starg = -501,

        /// <summary>
        /// ldloc
        /// </summary>
        Ldloc = -500,

        /// <summary>
        /// ldloca
        /// </summary>
        Ldloca = -499,

        /// <summary>
        /// stloc
        /// </summary>
        Stloc = -498,

        /// <summary>
        /// localloc
        /// </summary>
        Localloc = -497,

        /// <summary>
        /// endfilter
        /// </summary>
        Endfilter = -495,

        /// <summary>
        /// unaligned.
        /// </summary>
        Unaligned = -494,

        /// <summary>
        /// volatile.
        /// </summary>
        Volatile = -493,

        /// <summary>
        /// tail.
        /// </summary>
        Tail = -492,

        /// <summary>
        /// initobj
        /// </summary>
        Initobj = -491,

        /// <summary>
        /// constrained.
        /// </summary>
        Constrained = -490,

        /// <summary>
        /// cpblk
        /// </summary>
        Cpblk = -489,

        /// <summary>
        /// initblk
        /// </summary>
        Initblk = -488,

        /// <summary>
        /// no.
        /// </summary>
        No = -487,

        /// <summary>
        /// rethrow
        /// </summary>
        Rethrow = -486,

        /// <summary>
        /// sizeof
        /// </summary>
        Sizeof = -484,

        /// <summary>
        /// refanytype
        /// </summary>
        Refanytype = -483,

        /// <summary>
        /// readonly.
        /// </summary>
        Readonly = -482,

    }
}
