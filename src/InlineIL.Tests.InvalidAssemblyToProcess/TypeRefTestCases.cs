﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using InlineIL;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class TypeRefTestCases
{
    public void LoadNullType()
    {
        IL.Emit(OpCodes.Ldtoken, new TypeRef(null));
    }

    public void LoadNullTypeRef()
    {
        IL.Emit(OpCodes.Ldtoken, (TypeRef)null);
    }

    public void InvalidAssembly()
    {
        IL.Emit(OpCodes.Ldtoken, new TypeRef("AssemblyThatDonestExist", "TypeThatDoesntExist"));
    }

    public void InvalidType()
    {
        IL.Emit(OpCodes.Ldtoken, new TypeRef("System", "TypeThatDoesntExist"));
    }

    public void InvalidArrayRank()
    {
        IL.Emit(OpCodes.Ldtoken, typeof(int).MakeArrayType(-1));
    }

    public void UnusedInstance()
    {
        GC.KeepAlive(new TypeRef(typeof(int)));
    }

    public void NotAGenericType()
    {
        IL.Emit(OpCodes.Ldtoken, typeof(string).MakeGenericType(typeof(int)));
    }

    public void NoGenericTypeArgs()
    {
        IL.Emit(OpCodes.Ldtoken, typeof(Dictionary<,>).MakeGenericType());
    }

    public void InvalidGenericArgsCount()
    {
        IL.Emit(OpCodes.Ldtoken, typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(double), typeof(string)));
    }
}
