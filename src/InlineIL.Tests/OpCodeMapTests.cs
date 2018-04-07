﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InlineIL.Fody;
using InlineIL.Tests.Support;
using Mono.Cecil.Cil;
using Xunit;
using Xunit.Abstractions;

namespace InlineIL.Tests
{
    using OpCodes = Mono.Cecil.Cil.OpCodes;

    public class OpCodeMapTests
    {
        private readonly ITestOutputHelper _output;

        public OpCodeMapTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void PrintMap()
        {
            var cecilCodes = typeof(OpCodes)
                             .GetFields(BindingFlags.Public | BindingFlags.Static)
                             .Where(field => field.IsInitOnly && field.FieldType == typeof(OpCode))
                             .Select(field => (OpCode)field.GetValue(null))
                             .ToDictionary(i => i.Value);

            var reflectionEmitCodes = typeof(System.Reflection.Emit.OpCodes)
                                      .GetFields(BindingFlags.Public | BindingFlags.Static)
                                      .Where(field => field.IsInitOnly && field.FieldType == typeof(System.Reflection.Emit.OpCode))
                                      .Select(field => (System.Reflection.Emit.OpCode)field.GetValue(null))
                                      .ToDictionary(i => i.Value);

            var values = new HashSet<short>();
            values.UnionWith(cecilCodes.Keys);
            values.UnionWith(reflectionEmitCodes.Keys);

            foreach (var value in values.OrderBy(i => unchecked((ushort)i)))
            {
                var reflectionEmitCode = reflectionEmitCodes.TryGetValue(value, out var reflectionEmitOpCode) ? reflectionEmitOpCode.Name : "???";
                var cecilCode = cecilCodes.TryGetValue(value, out var cecilOpCode) ? cecilOpCode.Name : "???";

                _output.WriteLine($"{value:X4}: {reflectionEmitCode,-15} {cecilCode,-15}");
            }
        }

        [Fact]
        public void should_map_opcodes()
        {
            OpCodeMap.GetByValue(OpCodes.Nop.Value).ShouldEqual(OpCodes.Nop);
            OpCodeMap.GetByValue(OpCodes.Dup.Value).ShouldEqual(OpCodes.Dup);
            OpCodeMap.GetByValue(OpCodes.Leave.Value).ShouldEqual(OpCodes.Leave);
            OpCodeMap.GetByValue(OpCodes.Sizeof.Value).ShouldEqual(OpCodes.Sizeof);
        }
    }
}
