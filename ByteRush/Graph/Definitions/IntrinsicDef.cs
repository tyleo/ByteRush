using ByteRush.CodeGen;
using ByteRush.Interpreter;
using ByteRush.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ByteRush.Graph.Definitions
{
    public sealed class IntrinsicDef : INodeDecl
    {
        public FullName FullName { get; }

        private readonly Intrinsic _op;

        private readonly IReadOnlyList<PortDecl> _inputs;
        public IReadOnlyList<PortDecl> GetInputs(FunctionDef function) => _inputs;

        private readonly IReadOnlyList<PortDecl> _outputs;
        public IReadOnlyList<PortDecl> GetOutputs(FunctionDef function) => _outputs;

        public object DefaultMeta() => null;

        private bool InputsArePure => !_inputs.Any() || _inputs.First().Type == TypeKind.Exec;
        private bool OutputsArePure => !_outputs.Any() || _outputs.First().Type == TypeKind.Exec;
        private bool IsPure => InputsArePure && OutputsArePure;

        private IntrinsicDef(
            FullName fullName,
            Intrinsic op,
            IEnumerable<(string Name, TypeKind Type)> inputs,
            IEnumerable<(string Name, TypeKind Type)> outputs
        )
        {
            FullName = fullName;
            _op = op;

            _inputs = inputs.Select(i => PortDecl.New(i.Name, i.Type)).ToArray();
            _outputs = outputs.Select(i => PortDecl.New(i.Name, i.Type)).ToArray();
        }

        public static IntrinsicDef New(
            FullName fullName,
            Intrinsic op,
            IEnumerable<(string Name, TypeKind Type)> inputs,
            IEnumerable<(string Name, TypeKind Type)> outputs
        ) => new IntrinsicDef(fullName, op, inputs, outputs);

        public void GenerateCode(NodeId nodeId, in Node node, CodeGenState state)
        {
            var inputs = GetInputs(state.Function);
            var outputs = GetOutputs(state.Function);

            var inputsNotExec = inputs.Where(i => i.Type != TypeKind.Exec);
            var outputsNotExec = outputs.Where(i => i.Type != TypeKind.Exec);

            var inputSymbols = new (TypeKind Type, ISymbol<MUnknown> Symbol)[inputsNotExec.Count()];
            foreach (var (input, idx) in inputsNotExec.Enumerate())
            {
                inputSymbols[idx] = (input.Type, state.GenerateDataBack<MUnknown>(in node, InputPortId.New(idx)));
            }
            foreach (var typedSymbol in inputSymbols)
            {
                typedSymbol.Symbol.Release();
            }

            var (_0, intrinsic, numParams, @params, numReturns, returns) =
                state.OpWriter.CallIntrinsic(inputs.Count.Byte(), outputs.Count.Byte());

            var outputSymbols = new (TypeKind Type, ISymbol<MUnknown> Symbol)[outputsNotExec.Count()];
            foreach (var (output, idx) in outputsNotExec.Enumerate())
            {
                var portId = OutputPortId.New(idx);
                var edgeUses = node.GetOutput(portId).EdgeCount;
                outputSymbols[idx] = (output.Type, state.RetainAnonomyous<MUnknown>(edgeUses));
                state.SetOutputSymbol(OutputPortKey.New(nodeId, portId), outputSymbols[idx].Symbol);
            }

            var id = state.Shared.GetIntrinsicId(node.DeclId, _op);

            state.OpWriter.WriteIntrinsic(id, intrinsic);
            state.OpWriter.WriteU8(inputs.Count.Byte(), numParams);
            state.OpWriter.WriteU8(outputs.Count.Byte(), numReturns);

            foreach (var (inputSymbol, inputParam) in inputSymbols.ValueZip(@params))
            {
                switch (inputSymbol.Type)
                {
                    case TypeKind.Bool:
                        state.QueueSymbolAddressWrite(inputSymbol.Symbol.Mark<MBool>(), inputParam.Mark<MStackAddress<MBool>>());
                        break;
                    case TypeKind.F32:
                        state.QueueSymbolAddressWrite(inputSymbol.Symbol.Mark<MF32>(), inputParam.Mark<MStackAddress<MF32>>());
                        break;
                    case TypeKind.I32:
                        state.QueueSymbolAddressWrite(inputSymbol.Symbol.Mark<MI32>(), inputParam.Mark<MStackAddress<MI32>>());
                        break;
                    case TypeKind.U8:
                        state.QueueSymbolAddressWrite(inputSymbol.Symbol.Mark<MU8>(), inputParam.Mark<MStackAddress<MU8>>());
                        break;
                }
            }

            foreach (var (outputSymbol, outputParam) in outputSymbols.ValueZip(returns))
            {
                switch (outputSymbol.Type)
                {
                    case TypeKind.Bool:
                        state.QueueSymbolAddressWrite(outputSymbol.Symbol.Mark<MBool>(), outputParam.Mark<MStackAddress<MBool>>());
                        break;
                    case TypeKind.F32:
                        state.QueueSymbolAddressWrite(outputSymbol.Symbol.Mark<MF32>(), outputParam.Mark<MStackAddress<MF32>>());
                        break;
                    case TypeKind.I32:
                        state.QueueSymbolAddressWrite(outputSymbol.Symbol.Mark<MI32>(), outputParam.Mark<MStackAddress<MI32>>());
                        break;
                    case TypeKind.U8:
                        state.QueueSymbolAddressWrite(outputSymbol.Symbol.Mark<MU8>(), outputParam.Mark<MStackAddress<MU8>>());
                        break;
                }
            }

            var (Value, Index) = outputs.Enumerate().FirstOrDefault(i => i.Value.Type == TypeKind.Exec);
            if (Value != null)
            {
                state.GenerateExecForwards(in node, OutputPortId.New(Index));
            }
        }
    }
}
