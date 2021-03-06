﻿using System;
using System.Collections.Generic;
using System.Linq;
using Fody;
using InlineIL.Fody.Extensions;
using InlineIL.Fody.Model;
using JetBrains.Annotations;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace InlineIL.Fody.Processing
{
    internal class WeaverILProcessor
    {
        private readonly ILProcessor _il;
        private readonly HashSet<Instruction> _referencedInstructions;

        [CanBeNull]
        public MethodLocals Locals { get; private set; }

        public WeaverILProcessor(MethodDefinition method)
        {
            _il = method.Body.GetILProcessor();
            _referencedInstructions = GetAllReferencedInstructions();
        }

        public void Remove(Instruction instruction)
        {
            var newRef = instruction.Next ?? instruction.Previous;
            _il.Remove(instruction);
            UpdateReferences(instruction, newRef);
        }

        public void Replace(Instruction oldInstruction, Instruction newInstruction)
        {
            _il.Replace(oldInstruction, newInstruction);
            UpdateReferences(oldInstruction, newInstruction);
        }

        public void DeclareLocals(IEnumerable<LocalVarBuilder> locals)
        {
            if (Locals != null)
                throw new WeavingException("Local variables have already been declared for this method");

            Locals = new MethodLocals(_il.Body.Method, locals);
        }

        public HashSet<Instruction> GetAllReferencedInstructions()
        {
            var refs = new HashSet<Instruction>();

            foreach (var handler in _il.Body.ExceptionHandlers)
                refs.UnionWith(handler.GetInstructions());

            foreach (var instruction in _il.Body.Instructions)
            {
                switch (instruction.Operand)
                {
                    case Instruction target:
                        refs.Add(target);
                        break;

                    case Instruction[] targets:
                        refs.UnionWith(targets.Where(t => t != null));
                        break;
                }
            }

            return refs;
        }

        private void UpdateReferences(Instruction oldInstruction, Instruction newInstruction)
        {
            if (!_referencedInstructions.Contains(oldInstruction))
                return;

            foreach (var handler in _il.Body.ExceptionHandlers)
            {
                if (handler.TryStart == oldInstruction)
                    handler.TryStart = newInstruction;

                if (handler.TryEnd == oldInstruction)
                    handler.TryEnd = newInstruction;

                if (handler.FilterStart == oldInstruction)
                    handler.FilterStart = newInstruction;

                if (handler.HandlerStart == oldInstruction)
                    handler.HandlerStart = newInstruction;

                if (handler.HandlerEnd == oldInstruction)
                    handler.HandlerEnd = newInstruction;
            }

            foreach (var instruction in _il.Body.Instructions)
            {
                switch (instruction.Operand)
                {
                    case Instruction target when target == oldInstruction:
                        instruction.Operand = newInstruction;
                        break;

                    case Instruction[] targets:
                        for (var i = 0; i < targets.Length; ++i)
                        {
                            if (targets[i] == oldInstruction)
                                targets[i] = newInstruction;
                        }

                        break;
                }
            }

            _referencedInstructions.Remove(oldInstruction);
            _referencedInstructions.Add(newInstruction);
        }

        public void RemoveNopsAround(Instruction instruction)
        {
            RemoveNopsBefore(instruction);
            RemoveNopsAfter(instruction);
        }

        private void RemoveNopsBefore(Instruction instruction)
        {
            var currentInstruction = instruction?.Previous;
            while (currentInstruction != null && currentInstruction.OpCode == OpCodes.Nop)
            {
                var prev = currentInstruction.Previous;
                Remove(currentInstruction);
                currentInstruction = prev;
            }
        }

        public void RemoveNopsAfter(Instruction instruction)
        {
            var currentInstruction = instruction?.Next;
            while (currentInstruction != null && currentInstruction.OpCode == OpCodes.Nop)
            {
                var next = currentInstruction.Next;
                Remove(currentInstruction);
                currentInstruction = next;
            }
        }

        public Instruction Create(OpCode opCode)
        {
            try
            {
                var instruction = _il.Create(opCode);
                MethodLocals.MapMacroInstruction(Locals, instruction);
                return instruction;
            }
            catch (ArgumentException)
            {
                throw ExceptionInvalidOperand(opCode);
            }
        }

        public Instruction Create(OpCode opCode, TypeReference typeRef)
        {
            try
            {
                return _il.Create(opCode, typeRef);
            }
            catch (ArgumentException)
            {
                throw ExceptionInvalidOperand(opCode);
            }
        }

        public Instruction Create(OpCode opCode, MethodReference methodRef)
        {
            try
            {
                return _il.Create(opCode, methodRef);
            }
            catch (ArgumentException)
            {
                throw ExceptionInvalidOperand(opCode);
            }
        }

        public Instruction Create(OpCode opCode, FieldReference fieldRef)
        {
            try
            {
                return _il.Create(opCode, fieldRef);
            }
            catch (ArgumentException)
            {
                throw ExceptionInvalidOperand(opCode);
            }
        }

        public Instruction Create(OpCode opCode, Instruction instruction)
        {
            try
            {
                var result = _il.Create(opCode, instruction);
                _referencedInstructions.Add(instruction);
                return result;
            }
            catch (ArgumentException)
            {
                throw ExceptionInvalidOperand(opCode);
            }
        }

        public Instruction Create(OpCode opCode, Instruction[] instructions)
        {
            try
            {
                var result = _il.Create(opCode, instructions);
                _referencedInstructions.UnionWith(instructions.Where(i => i != null));
                return result;
            }
            catch (ArgumentException)
            {
                throw ExceptionInvalidOperand(opCode);
            }
        }

        public Instruction Create(OpCode opCode, VariableDefinition variableDef)
        {
            try
            {
                return _il.Create(opCode, variableDef);
            }
            catch (ArgumentException)
            {
                throw ExceptionInvalidOperand(opCode);
            }
        }

        public Instruction Create(OpCode opCode, CallSite callSite)
        {
            try
            {
                return _il.Create(opCode, callSite);
            }
            catch (ArgumentException)
            {
                throw ExceptionInvalidOperand(opCode);
            }
        }

        public Instruction CreateConst(OpCode opCode, object operand)
        {
            try
            {
                switch (opCode.OperandType)
                {
                    case OperandType.InlineI:
                        operand = Convert.ToInt32(operand);
                        break;

                    case OperandType.InlineI8:
                        operand = Convert.ToInt64(operand);
                        break;

                    case OperandType.InlineR:
                        operand = Convert.ToDouble(operand);
                        break;

                    case OperandType.InlineVar:
                    case OperandType.InlineArg:
                        // It's an uint16 but Cecil expects int32
                        operand = Convert.ToInt32(operand);
                        break;

                    case OperandType.ShortInlineI:
                        operand = opCode == OpCodes.Ldc_I4_S
                            ? Convert.ToSByte(operand)
                            : (object)Convert.ToByte(operand);
                        break;

                    case OperandType.ShortInlineR:
                        operand = Convert.ToSingle(operand);
                        break;

                    case OperandType.ShortInlineVar:
                    case OperandType.ShortInlineArg:
                        operand = Convert.ToByte(operand);
                        break;
                }

                switch (operand)
                {
                    case int value:
                    {
                        if (MethodLocals.MapIndexInstruction(Locals, ref opCode, value, out var localVar))
                            return Create(opCode, localVar);

                        return _il.Create(opCode, value);
                    }

                    case byte value:
                    {
                        if (MethodLocals.MapIndexInstruction(Locals, ref opCode, value, out var localVar))
                            return Create(opCode, localVar);

                        return _il.Create(opCode, value);
                    }

                    case sbyte value:
                        return _il.Create(opCode, value);
                    case long value:
                        return _il.Create(opCode, value);
                    case double value:
                        return _il.Create(opCode, value);
                    case short value:
                        return _il.Create(opCode, value);
                    case float value:
                        return _il.Create(opCode, value);
                    case string value:
                        return _il.Create(opCode, value);

                    default:
                        throw new WeavingException($"Unexpected operand for instruction {opCode}: {operand}");
                }
            }
            catch (ArgumentException)
            {
                throw ExceptionInvalidOperand(opCode);
            }
        }

        private static WeavingException ExceptionInvalidOperand(OpCode opCode)
        {
            switch (opCode.OperandType)
            {
                case OperandType.InlineNone:
                    return new WeavingException($"Opcode {opCode} does not expect an operand");

                case OperandType.InlineBrTarget:
                case OperandType.ShortInlineBrTarget:
                    return ExpectedOperand("label name");

                case OperandType.InlineField:
                    return ExpectedOperand(KnownNames.Short.FieldRefType);

                case OperandType.InlineI:
                case OperandType.ShortInlineI:
                case OperandType.InlineArg:
                case OperandType.ShortInlineArg:
                    return ExpectedOperand(nameof(Int32));

                case OperandType.InlineI8:
                    return ExpectedOperand(nameof(Int64));

                case OperandType.InlineMethod:
                    return ExpectedOperand(KnownNames.Short.MethodRefType);

                case OperandType.InlineR:
                case OperandType.ShortInlineR:
                    return ExpectedOperand(nameof(Double));

                case OperandType.InlineSig:
                    return ExpectedOperand(KnownNames.Short.StandAloneMethodSigType);

                case OperandType.InlineString:
                    return ExpectedOperand(nameof(String));

                case OperandType.InlineSwitch:
                    return ExpectedOperand("array of label names");

                case OperandType.InlineType:
                    return ExpectedOperand($"{KnownNames.Short.TypeRefType} or {nameof(Type)}");

                case OperandType.InlineVar:
                case OperandType.ShortInlineVar:
                    return ExpectedOperand("local variable name or index");

                default:
                    return ExpectedOperand(opCode.OperandType.ToString());
            }

            WeavingException ExpectedOperand(string expected)
                => new WeavingException($"Opcode {opCode} expects an operand of type {expected}");
        }
    }
}
