﻿using System.Diagnostics.CodeAnalysis;
using InlineIL;
using static InlineIL.IL.Emit;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class LabelTestCases
{
    public void NullLabelName()
    {
        Br(null);
    }

    public void NullLabel()
    {
        IL.MarkLabel(null);
    }

    public void UndefinedLabel()
    {
        Br("foo");
    }

    public void RedefinedLabel()
    {
        IL.MarkLabel("foo");
        IL.MarkLabel("foo");
    }
}
