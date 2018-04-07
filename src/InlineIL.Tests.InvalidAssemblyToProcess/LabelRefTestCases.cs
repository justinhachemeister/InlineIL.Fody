﻿using System;
using System.Diagnostics.CodeAnalysis;
using InlineIL;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class LabelRefTestCases
{
    public void NullLabelName()
    {
        IL.Emit(OpCodes.Br, new LabelRef(null));
    }

    public void NullLabelRef()
    {
        IL.Emit(OpCodes.Br, (LabelRef)null);
    }

    public void NullLabel()
    {
        IL.MarkLabel(null);
    }

    public void UndefinedLabel()
    {
        IL.Emit(OpCodes.Br, new LabelRef("foo"));
    }

    public void RedefinedLabel()
    {
        IL.MarkLabel("foo");
        IL.MarkLabel("foo");
    }

    public void UnusedInstance()
    {
        GC.KeepAlive(new LabelRef("foo"));
    }
}
