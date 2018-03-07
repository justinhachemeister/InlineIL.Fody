﻿using System;
using System.Linq;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using Mono.Collections.Generic;

namespace InlineIL.Fody
{
    internal class MethodWeaver
    {
        private readonly ModuleDefinition _module;
        private readonly MethodDefinition _method;
        private readonly ILProcessor _il;

        private Collection<Instruction> Instructions => _method.Body.Instructions;

        public MethodWeaver(ModuleDefinition module, MethodDefinition method)
        {
            _module = module;
            _method = method;
            _il = _method.Body.GetILProcessor();
        }

        public static bool NeedsProcessing(MethodDefinition method)
        {
            return method.HasBody
                   && method.Body
                            .Instructions
                            .Where(i => i.OpCode == OpCodes.Call)
                            .Select(i => i.Operand)
                            .OfType<MethodReference>()
                            .Any(m => IsIlType(m.DeclaringType));
        }

        public void Process()
        {
            try
            {
                ProcessImpl();
            }
            catch (WeavingException ex)
            {
                throw new WeavingException($"Error processing method {_method.FullName}: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unexpected error occured while processing method {_method.FullName}: {ex.Message}", ex);
            }
        }

        private static bool IsIlType(TypeReference type)
            => type.Name == KnownNames.Short.IlType && type.FullName == KnownNames.Full.IlType;

        private void ProcessImpl()
        {
            _method.Body.SimplifyMacros();

            var instruction = Instructions.FirstOrDefault();

            while (instruction != null)
            {
                var nextInstruction = instruction.Next;

                if (instruction.OpCode == OpCodes.Call && instruction.Operand is MethodReference calledMethod && IsIlType(calledMethod.DeclaringType))
                {
                    switch (calledMethod.Name)
                    {
                        case KnownNames.Short.OpMethod:
                            ProcessOpMethod(instruction);
                            break;

                        case KnownNames.Short.PushMethod:
                            ProcessPushMethod(instruction);
                            break;

                        case KnownNames.Short.UnreachableMethod:
                            ProcessUnreachableMethod(instruction);
                            break;

                        case KnownNames.Short.ReturnMethod:
                            ProcessReturnMethod(instruction);
                            break;

                        default:
                            throw new WeavingException($"Unsupported method: {calledMethod.FullName}");
                    }
                }

                instruction = nextInstruction;
            }

            _method.Body.OptimizeMacros();
        }

        private void ProcessOpMethod(Instruction instruction)
        {
            var methodParams = ((MethodReference)instruction.Operand).Parameters;

            var args = instruction.GetArgumentPushInstructions();
            var opCode = ConsumeArgOpCode(args[0]);

            if (methodParams.Count == 1)
            {
                _il.Replace(instruction, InstructionHelper.Create(opCode));
                return;
            }

            var operandType = methodParams[1].ParameterType;
            if (operandType.IsPrimitive || operandType.FullName == _module.TypeSystem.String.FullName)
            {
                var operandValue = ConsumeArgConst(args[1]);
                _il.Replace(instruction, InstructionHelper.CreateConst(_il, opCode, operandValue));
                return;
            }

            if (operandType.FullName == KnownNames.Full.TypeRefType)
            {
                var typeRef = ConsumeArgTypeRef(args[1]);
                _il.Replace(instruction, InstructionHelper.Create(opCode, typeRef));
                return;
            }

            if (operandType.FullName == KnownNames.Full.MethodRefType)
            {
                var methodRef = ConsumeArgMethodRef(args[1]);
                _il.Replace(instruction, InstructionHelper.Create(opCode, methodRef));
                return;
            }

            throw new InvalidOperationException("Unsupported IL.Op overload");
        }

        private void ProcessPushMethod(Instruction instruction)
        {
            _il.Remove(instruction);
        }

        private void ProcessUnreachableMethod(Instruction instruction)
        {
            var throwInstruction = instruction.NextSkipNops();
            if (throwInstruction.OpCode != OpCodes.Throw)
                throw new WeavingException("The Unreachable method should be used along with the throw keyword: throw IL.Unreachable();");

            _il.Remove(instruction);
            _il.RemoveNopsAround(throwInstruction);
            _il.Remove(throwInstruction);
        }

        private void ProcessReturnMethod(Instruction instruction)
        {
            var nextInstruction = instruction.NextSkipNops();

            switch (nextInstruction.OpCode.Code)
            {
                case Code.Ret:
                    break;

                case Code.Stloc: // Debug builds
                {
                    var localIndex = ((VariableReference)nextInstruction.Operand).Index;
                    var branchInstruction = nextInstruction.NextSkipNops();
                    if (branchInstruction?.OpCode == OpCodes.Br && branchInstruction.Operand is Instruction branchTarget)
                    {
                        if (branchTarget.OpCode == OpCodes.Nop)
                            branchTarget = branchTarget.NextSkipNops();

                        if (branchTarget.OpCode == OpCodes.Ldloc && ((VariableReference)branchTarget.Operand).Index == localIndex)
                            break;
                    }

                    goto default;
                }

                default:
                    throw new WeavingException("The Return method should be used along the return keyword: return IL.Return<T>();");
            }

            _il.Remove(instruction);
        }

        private OpCode ConsumeArgOpCode(Instruction instruction)
        {
            _il.Remove(instruction);
            return OpCodeMap.FromLdsfld(instruction);
        }

        private string ConsumeArgString(Instruction instruction)
        {
            if (instruction.OpCode != OpCodes.Ldstr)
                throw new WeavingException($"Unexpected instruction, expected a constant string, but was {instruction}");

            _il.Remove(instruction);
            return (string)instruction.Operand;
        }

        private object ConsumeArgConst(Instruction instruction)
        {
            switch (instruction.OpCode.OperandType)
            {
                case OperandType.InlineI:
                case OperandType.InlineI8:
                case OperandType.InlineR:
                case OperandType.ShortInlineI:
                case OperandType.ShortInlineR:
                case OperandType.InlineString:
                    _il.Remove(instruction);
                    return instruction.Operand;
            }

            switch (instruction.OpCode.Code)
            {
                case Code.Conv_I1:
                case Code.Conv_I2:
                case Code.Conv_I4:
                case Code.Conv_I8:
                case Code.Conv_U1:
                case Code.Conv_U2:
                case Code.Conv_U4:
                case Code.Conv_U8:
                case Code.Conv_R4:
                case Code.Conv_R8:
                    var value = ConsumeArgConst(instruction.PrevSkipNops());
                    _il.Remove(instruction);
                    return value;
            }

            throw new WeavingException($"Unexpected instruction, expected a constant, but was {instruction}");
        }

        private TypeReference ConsumeArgTypeRef(Instruction instruction)
        {
            if (instruction.OpCode.FlowControl != FlowControl.Call || !(instruction.Operand is MethodReference method))
                throw new WeavingException($"Unexpected instruction, expected a call, but was {instruction}");

            switch (method.FullName)
            {
                case "System.Type System.Type::GetTypeFromHandle(System.RuntimeTypeHandle)":
                {
                    var ldToken = instruction.GetArgumentPushInstructions().Single();
                    if (ldToken.OpCode != OpCodes.Ldtoken)
                        throw new WeavingException($"Unexpected instruction, expected ldtoken, but was {ldToken}");

                    _il.Remove(ldToken);
                    _il.Remove(instruction);
                    return (TypeReference)ldToken.Operand;
                }

                case "InlineIL.TypeRef InlineIL.TypeRef::op_Implicit(System.Type)":
                case "System.Void InlineIL.TypeRef::.ctor(System.Type)":
                {
                    var innerTypeRef = ConsumeArgTypeRef(instruction.GetArgumentPushInstructions().Single());
                    _il.Remove(instruction);
                    return innerTypeRef;
                }

                case "System.Void InlineIL.TypeRef::.ctor(System.String,System.String)":
                {
                    var args = instruction.GetArgumentPushInstructions();
                    var assemblyName = ConsumeArgString(args[0]);
                    var typeName = ConsumeArgString(args[1]);

                    var assembly = assemblyName == _module.Assembly.Name.Name
                        ? _module.Assembly
                        : _module.AssemblyResolver.Resolve(new AssemblyNameReference(assemblyName, null));

                    if (assembly == null)
                        throw new WeavingException($"Could not resolve assembly {assemblyName}");

                    var typeReference = assembly.Modules
                                                .Select(module =>
                                                {
                                                    var parsedType = module.GetType(typeName, true);

                                                    // parsedType is not null when the type doesn't exist in the current version of Cecil
                                                    return parsedType != null ? module.GetType(parsedType.FullName) : null;
                                                })
                                                .FirstOrDefault(t => t != null);

                    if (typeReference == null)
                        throw new WeavingException($"Could not find type {typeName} in assembly {assemblyName}");

                    _il.Remove(instruction);
                    return _module.ImportReference(typeReference);
                }

                case "InlineIL.TypeRef InlineIL.TypeRef::ToPointer()":
                {
                    var innerTypeRef = ConsumeArgTypeRef(instruction.GetArgumentPushInstructions().Single());
                    _il.Remove(instruction);
                    return innerTypeRef.MakePointerType();
                }

                case "InlineIL.TypeRef InlineIL.TypeRef::ToReference()":
                {
                    var innerTypeRef = ConsumeArgTypeRef(instruction.GetArgumentPushInstructions().Single());
                    _il.Remove(instruction);
                    return innerTypeRef.MakeByReferenceType();
                }

                case "InlineIL.TypeRef InlineIL.TypeRef::ToArray()":
                {
                    var innerTypeRef = ConsumeArgTypeRef(instruction.GetArgumentPushInstructions().Single());
                    _il.Remove(instruction);
                    return innerTypeRef.MakeArrayType();
                }
            }

            throw new WeavingException($"Invalid operand, expected a type reference at {instruction}");
        }

        private MethodReference ConsumeArgMethodRef(Instruction instruction)
        {
            if (instruction.OpCode.FlowControl != FlowControl.Call || !(instruction.Operand is MethodReference method))
                throw new WeavingException($"Unexpected instruction, expected a call, but was {instruction}");

            switch (method.FullName)
            {
                case "System.Void InlineIL.MethodRef::.ctor(InlineIL.TypeRef,System.String)":
                {
                    var args = instruction.GetArgumentPushInstructions();
                    var typeDef = ConsumeArgTypeRef(args[0]).ResolveRequiredType();
                    var methodName = ConsumeArgString(args[1]);

                    var methods = typeDef.Methods.Where(m => m.Name == methodName).ToList();
                    switch (methods.Count)
                    {
                        case 0:
                            throw new WeavingException($"Method {methodName} not found in type {typeDef.FullName}");

                        case 1:
                            _il.Remove(instruction);
                            return _module.ImportReference(methods.Single());

                        default:
                            throw new WeavingException($"Ambiguous method {methodName} in type {typeDef.FullName}");
                    }
                }

                case "System.Void InlineIL.MethodRef::.ctor(InlineIL.TypeRef,System.String,InlineIL.TypeRef[])":
                {
                    var args = instruction.GetArgumentPushInstructions();
                    var typeDef = ConsumeArgTypeRef(args[0]).ResolveRequiredType();
                    var methodName = ConsumeArgString(args[1]);
                    var paramTypes = ConsumeArgArray(args[2], ConsumeArgTypeRef);

                    var methods = typeDef.Methods
                                         .Where(m => m.Name == methodName
                                                     && m.Parameters.Count == paramTypes.Length
                                                     && m.Parameters.Select(p => p.ParameterType.FullName).SequenceEqual(paramTypes.Select(p => p.FullName)))
                                         .ToList();

                    switch (methods.Count)
                    {
                        case 0:
                            throw new WeavingException($"Method {methodName}({string.Join(", ", paramTypes.Select(p => p.FullName))}) not found in type {typeDef.FullName}");

                        case 1:
                            _il.Remove(instruction);
                            return _module.ImportReference(methods.Single());

                        default:
                            // This should never happen
                            throw new WeavingException($"Ambiguous method {methodName}({string.Join(", ", paramTypes.Select(p => p.FullName))}) in type {typeDef.FullName}");
                    }
                }
            }

            throw new WeavingException($"Invalid operand, expected a type reference at {instruction}");
        }

        private T[] ConsumeArgArray<T>(Instruction instruction, Func<Instruction, T> consumeItem)
        {
            if (instruction.OpCode == OpCodes.Call)
            {
                if (!(instruction.Operand is MethodReference method) || method.GetElementMethod().FullName != "!!0[] System.Array::Empty()")
                    throw new WeavingException($"Unexpected instruction, expected newarr or call to Array.Empty, but was {instruction}");

                _il.Remove(instruction);
                return Array.Empty<T>();
            }

            if (instruction.OpCode != OpCodes.Newarr)
                throw new WeavingException($"Unexpected instruction, expected newarr or call to Array.Empty, but was {instruction}");

            var newarrInstruction = instruction;

            var countInstruction = newarrInstruction.PrevSkipNops();
            if (countInstruction.OpCode != OpCodes.Ldc_I4)
                throw new WeavingException($"Unexpected instruction, expected ldc.i4, but was {countInstruction}");

            var count = (int)countInstruction.Operand;
            var args = new T[count];

            var currentDupInstruction = newarrInstruction.NextSkipNops();

            for (var index = 0; index < count; ++index)
            {
                var dupInstruction = currentDupInstruction;
                if (dupInstruction.OpCode != OpCodes.Dup)
                    throw new WeavingException($"Unexpected instruction, expected dup, but was {dupInstruction}");

                var indexInstruction = dupInstruction.NextSkipNops();
                if (indexInstruction.OpCode != OpCodes.Ldc_I4)
                    throw new WeavingException($"Unexpected instruction, expected ldc.i4, but was {indexInstruction}");

                if ((int)indexInstruction.Operand != index)
                    throw new WeavingException($"Unexpected instruction, expected ldc.i4 with value of {index}, but was {indexInstruction}");

                var stelemInstruction = dupInstruction.GetValueConsumingInstruction();
                if (!stelemInstruction.OpCode.IsStelem())
                    throw new WeavingException($"Unexpected instruction, expected stelem, but was {stelemInstruction}");

                args[index] = consumeItem(stelemInstruction.PrevSkipNops());

                currentDupInstruction = stelemInstruction.NextSkipNops();

                _il.Remove(dupInstruction);
                _il.Remove(indexInstruction);
                _il.Remove(stelemInstruction);
            }

            _il.Remove(countInstruction);
            _il.Remove(newarrInstruction);

            return args;
        }
    }
}
