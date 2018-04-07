﻿using System;
using System.Linq;
using InlineIL.Fody;
using InlineIL.Tests.Support;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Xunit;

namespace InlineIL.Tests
{
    using OpCodes = Mono.Cecil.Cil.OpCodes;

    public class CecilExtensionsTests : IDisposable
    {
        private readonly AssemblyDefinition _assembly;
        private readonly ILProcessor _il;
        private readonly MethodReference _methodPop3Push0;
        private readonly MethodReference _methodPop2Push1;

        public CecilExtensionsTests()
        {
            _assembly = AssemblyDefinition.CreateAssembly(new AssemblyNameDefinition("Test", new Version()), "Test", ModuleKind.Dll);

            var method = new MethodDefinition("Test", MethodAttributes.Public | MethodAttributes.Static, _assembly.MainModule.TypeSystem.Void);
            _il = method.Body.GetILProcessor();

            _methodPop3Push0 = new MethodReference("Test", _assembly.MainModule.TypeSystem.Void);
            _methodPop3Push0.Parameters.Add(new ParameterDefinition(_assembly.MainModule.TypeSystem.Int32));
            _methodPop3Push0.Parameters.Add(new ParameterDefinition(_assembly.MainModule.TypeSystem.Int32));
            _methodPop3Push0.Parameters.Add(new ParameterDefinition(_assembly.MainModule.TypeSystem.Int32));

            _methodPop2Push1 = new MethodReference("Test", _assembly.MainModule.TypeSystem.Int32);
            _methodPop2Push1.Parameters.Add(new ParameterDefinition(_assembly.MainModule.TypeSystem.Int32));
            _methodPop2Push1.Parameters.Add(new ParameterDefinition(_assembly.MainModule.TypeSystem.Int32));
        }

        public void Dispose()
        {
            _assembly.Dispose();
        }

        [Fact]
        public void should_find_call_arguments()
        {
            _il.Emit(OpCodes.Ldc_I4_0);
            var p0 = _il.Body.Instructions.Last();

            _il.Emit(OpCodes.Nop);
            _il.Emit(OpCodes.Ldc_I4_1);
            var p1 = _il.Body.Instructions.Last();

            _il.Emit(OpCodes.Ldc_I4_8);
            _il.Emit(OpCodes.Nop);
            _il.Emit(OpCodes.Nop);
            _il.Emit(OpCodes.Ldc_I4_8);
            _il.Emit(OpCodes.Add);
            var p2 = _il.Body.Instructions.Last();

            _il.Emit(OpCodes.Call, _methodPop3Push0);
            var callInstruction = _il.Body.Instructions.Last();

            var result = callInstruction.GetArgumentPushInstructions();
            result.ShouldEqual(new[] { p0, p1, p2 });
        }

        [Fact]
        public void should_handle_dup_and_nop()
        {
            _il.Emit(OpCodes.Nop);
            _il.Emit(OpCodes.Ldc_I4_0);
            var p0 = _il.Body.Instructions.Last();

            _il.Emit(OpCodes.Nop);
            _il.Emit(OpCodes.Dup);
            var p1 = _il.Body.Instructions.Last();

            _il.Emit(OpCodes.Nop);
            _il.Emit(OpCodes.Ldc_I4_8);
            _il.Emit(OpCodes.Nop);
            _il.Emit(OpCodes.Nop);
            _il.Emit(OpCodes.Ldc_I4_8);
            _il.Emit(OpCodes.Nop);
            _il.Emit(OpCodes.Add);
            var p2 = _il.Body.Instructions.Last();

            _il.Emit(OpCodes.Call, _methodPop3Push0);
            var callInstruction = _il.Body.Instructions.Last();

            var result = callInstruction.GetArgumentPushInstructions();
            result.ShouldEqual(new[] { p0, p1, p2 });
        }

        [Fact]
        public void should_skip_over_method_calls()
        {
            _il.Emit(OpCodes.Ldc_I4_0);
            var p0 = _il.Body.Instructions.Last();

            _il.Emit(OpCodes.Ldc_I4_8);
            _il.Emit(OpCodes.Ldc_I4_8);
            _il.Emit(OpCodes.Ldc_I4_8);
            _il.Emit(OpCodes.Add);
            _il.Emit(OpCodes.Ldc_I4_8);
            _il.Emit(OpCodes.Ldc_I4_8);
            _il.Emit(OpCodes.Add);
            _il.Emit(OpCodes.Mul);
            _il.Emit(OpCodes.Call, _methodPop2Push1);
            var p1 = _il.Body.Instructions.Last();

            _il.Emit(OpCodes.Ldc_I4_8);
            _il.Emit(OpCodes.Ldc_I4_8);
            _il.Emit(OpCodes.Add);
            var p2 = _il.Body.Instructions.Last();

            _il.Emit(OpCodes.Call, _methodPop3Push0);
            var callInstruction = _il.Body.Instructions.Last();

            var result = callInstruction.GetArgumentPushInstructions();
            result.ShouldEqual(new[] { p0, p1, p2 });
        }
    }
}
