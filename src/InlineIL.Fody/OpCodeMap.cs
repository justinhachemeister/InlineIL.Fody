using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fody;
using Mono.Cecil.Cil;

namespace InlineIL.Fody
{
    internal static class OpCodeMap
    {
        private static readonly Dictionary<short, OpCode> _byValue;

        static OpCodeMap()
        {
            _byValue = typeof(OpCodes)
                       .GetFields(BindingFlags.Public | BindingFlags.Static)
                       .Where(field => field.IsInitOnly && field.FieldType == typeof(OpCode))
                       .Select(field => (OpCode)field.GetValue(null))
                       .ToDictionary(field => field.Value);
        }

        public static OpCode GetByValue(int value)
        {
            if (value < short.MinValue || value > short.MaxValue || !_byValue.TryGetValue((short)value, out var result))
                throw new WeavingException($"Unsupported opcode with value {value}");

            return result;
        }
    }
}
