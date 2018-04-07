﻿using System.Collections.Generic;

namespace InlineIL.Fody
{
    internal static class KnownNames
    {
        public static class Short
        {
            public const string IlType = "IL";
            public const string OpCodesType = "OpCodes";
            public const string TypeRefType = "TypeRef";
            public const string MethodRefType = "MethodRef";
            public const string FieldRefType = "FieldRef";
            public const string LabelRefType = "LabelRef";
            public const string LocalVarType = "LocalVar";
            public const string LocalRefType = "LocalRef";
            public const string StandAloneMethodSigType = "StandAloneMethodSig";

            public const string EmitMethod = "Emit";
            public const string PushMethod = "Push";
            public const string UnreachableMethod = "Unreachable";
            public const string ReturnMethod = "Return";
            public const string MarkLabelMethod = "MarkLabel";
            public const string DeclareLocalsMethod = "DeclareLocals";
        }

        public static class Full
        {
            private const string _nsPrefix = "InlineIL.";

            public const string IlType = _nsPrefix + Short.IlType;
            public const string OpCodesType = _nsPrefix + Short.OpCodesType;
            public const string TypeRefType = _nsPrefix + Short.TypeRefType;
            public const string MethodRefType = _nsPrefix + Short.MethodRefType;
            public const string FieldRefType = _nsPrefix + Short.FieldRefType;
            public const string LabelRefType = _nsPrefix + Short.LabelRefType;
            public const string LocalVarType = _nsPrefix + Short.LocalVarType;
            public const string LocalRefType = _nsPrefix + Short.LocalRefType;
            public const string StandAloneMethodSigType = _nsPrefix + Short.StandAloneMethodSigType;

            public static readonly HashSet<string> AllTypes = new HashSet<string>
            {
                IlType,
                OpCodesType,
                TypeRefType,
                MethodRefType,
                FieldRefType,
                LabelRefType,
                LocalVarType,
                LocalRefType,
                StandAloneMethodSigType
            };
        }
    }
}
