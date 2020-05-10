using ByteRush.Graph;
using ByteRush.Utilities;
using ByteRush.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ByteRush.CodeGen
{
    public sealed class CodeGenState
    {
        public NodeDef NodeDef { get; }
        public State GraphState { get; }
        public CodeOnlyOpCodeWriter OpWriter { get; } = CodeOnlyOpCodeWriter.New();


        private readonly IList<PendingOpCodeOnlyStackAddressWrite<MBool>> _bools = new List<PendingOpCodeOnlyStackAddressWrite<MBool>>();
        private readonly IList<PendingOpCodeOnlyStackAddressWrite<MF32>> _f32s = new List<PendingOpCodeOnlyStackAddressWrite<MF32>>();
        private readonly IList<PendingOpCodeOnlyStackAddressWrite<MI32>> _i32s = new List<PendingOpCodeOnlyStackAddressWrite<MI32>>();
        private readonly IList<PendingOpCodeOnlyStackAddressWrite<MU8>> _u8s = new List<PendingOpCodeOnlyStackAddressWrite<MU8>>();


        private readonly IDictionary<OutputPortKey, ISymbol<MUnknown>> _currentOutputSymbolValues = new Dictionary<OutputPortKey, ISymbol<MUnknown>>();
        private readonly ISet<NodeId> _generatedNodes = new HashSet<NodeId>();
        private readonly IList<NodeId> _generatedExpressions = new List<NodeId>();


        private readonly IList<PendingOpCodeOnlyOpCodeAddressWrite> _opCodeLocations = new List<PendingOpCodeOnlyOpCodeAddressWrite>();


        private readonly IDictionary<InputPortKey, int> _execBranchMergePoints = new Dictionary<InputPortKey, int>();


        private Indexer _anonomyousGenerator = new Indexer();


        public void QueueSymbolAddressWrite(ISymbol<MBool> symbol, OpCodeOnlyAddress<MStackAddress<MBool>> writeLocation) =>
            _bools.Add(PendingOpCodeOnlyStackAddressWrite<MBool>.New(symbol, writeLocation));

        public void QueueSymbolAddressWrite(ISymbol<MF32> symbol, OpCodeOnlyAddress<MStackAddress<MF32>> writeLocation) =>
            _f32s.Add(PendingOpCodeOnlyStackAddressWrite<MF32>.New(symbol, writeLocation));

        public void QueueSymbolAddressWrite(ISymbol<MI32> symbol, OpCodeOnlyAddress<MStackAddress<MI32>> writeLocation) =>
            _i32s.Add(PendingOpCodeOnlyStackAddressWrite<MI32>.New(symbol, writeLocation));

        public void QueueSymbolAddressWrite(ISymbol<MU8> symbol, OpCodeOnlyAddress<MStackAddress<MU8>> writeLocation) =>
            _u8s.Add(PendingOpCodeOnlyStackAddressWrite<MU8>.New(symbol, writeLocation));


        public void SetOutputSymbol<T>(OutputPortKey key, ISymbol<T> symbol) => _currentOutputSymbolValues.AddOrUpdate(key, symbol.Mark<MUnknown>());

        private ISymbol<T> GetOutputSymbol<T>(OutputPortKey key) => _currentOutputSymbolValues[key].Mark<T>();


        public ISymbol<T> RetainAnonomyous<T>(int uses)
        {
            var index = _anonomyousGenerator.GetIndex();
            void Free() => _anonomyousGenerator.FreeIndex(index);
            var result = AnonymousSymbol<T>.New(index, uses, Free);
            if (uses == 0) Free();
            return result;
        }


        public void QueueOpCodeAddressWrite(OpCodeOnlyAddress<MOpCode> address, OpCodeOnlyAddress<MFinalOpCodeAddress<MOpCode>> writeLocation) =>
            _opCodeLocations.Add(PendingOpCodeOnlyOpCodeAddressWrite.New(address, writeLocation));


        public void GenerateCode(NodeId nodeId)
        {
            _generatedNodes.Add(nodeId);
            NodeDef.GetNode(nodeId).GenerateCode(nodeId, this);
        }

        public ISymbol<T> GenerateDataBack<T>(
            in Node currentNode,
            InputPortId inputPortId
        )
        {
            ref readonly var inputPort = ref currentNode.GetInput(inputPortId);
            if (inputPort.EdgeCount == 0)
            {
                // If the input port has no edges then the data is a constant.
                return ConstantSymbol<T>.New((inputPort.Type, currentNode.GetDefaultValue(inputPortId)));
            }

            ref readonly var outputPort = ref NodeDef.GetEdge(inputPort.GetEdge(EdgeId.New(0)))._from;

            if (!_generatedNodes.Contains(outputPort.Node))
            {
                // If generated nodes does not contain the backwards node, we haven't evaluated it
                // as a part of the current expression. We also know the node must be part of an
                // expression. We will generate code for it and then add it to the
                // _generatedExpressions list.

                GenerateCode(outputPort.Node);
                _generatedExpressions.Add(outputPort.Node);
            }

            // Code for the node has already been generated and its output is available. Just
            // return it.
            return GetOutputSymbol<T>(OutputPortKey.New(outputPort.Node, outputPort.Port));
        }

        public System.Action GenerateExecForwards(
            in Node currentNode,
            OutputPortId outputPortId
        )
        {
            ref readonly var outputPort = ref currentNode.GetOutput(outputPortId);
            if (outputPort.EdgeCount == 0)
            {
                // Nodes will call GenerateExecForwards on their exec ports. If they don't have any
                // connections on an exec port we are done generating code for this chain of nodes.
                return ActionExt.Empty();
            }

            void BeginImpureNode()
            {
                // When we start a new impure node we end the current expression. If we evaluate
                // the same expression again it may have a different value. We clear the nodes in
                // the generated expression so they can be regenerated.

                // Any node with an exec port is impure.
                _generatedNodes.RemoveRange(_generatedExpressions);
                _generatedExpressions.Clear();
            }

            ref readonly var inputEdge = ref NodeDef.GetEdge(outputPort.GetEdge(EdgeId.New(0)))._to;
            var nextNodeId = inputEdge.Node;
            ref readonly var nextNode = ref NodeDef.GetNode(inputEdge.Node);
            ref readonly var nextPort = ref nextNode.GetInput(inputEdge.Port);
            if (nextPort.EdgeCount == 1)
            {
                // If the next port has only one edge we can start generating code for it. It is
                // part of as simple sequence.
                BeginImpureNode();
                GenerateCode(nextNodeId);
                return ActionExt.Empty();
            }

            var portKey = InputPortKey.New(inputEdge.Node, inputEdge.Port);
            var remainingJoins = _execBranchMergePoints.GetOrAdd(portKey, nextPort.EdgeCount);
            remainingJoins--;
            _execBranchMergePoints[portKey] = remainingJoins;

            if (remainingJoins == 0)
            {
                // If we have visited this node multiple times we can consider it "merged". We can
                // continue evaluating any chain which is fully merged.
                return () =>
                {
                    BeginImpureNode();
                    GenerateCode(nextNodeId);
                };
            }

            return ActionExt.Empty();
        }

        private static (
            IEnumerable<IGrouping<int, PendingOpCodeOnlyStackAddressWrite<M, ParameterSymbol<M>>>> Parameters,
            IEnumerable<IGrouping<string, PendingOpCodeOnlyStackAddressWrite<M, VariableSymbol<M>>>> Variables,
            IEnumerable<IGrouping<int, PendingOpCodeOnlyStackAddressWrite<M, AnonymousSymbol<M>>>> Anonomyous,
            IEnumerable<IGrouping<(TypeKind, Value), PendingOpCodeOnlyStackAddressWrite<M, ConstantSymbol<M>>>> Constants
        ) Bucket<M>(IEnumerable<PendingOpCodeOnlyStackAddressWrite<M>> self)
        {
            var parameters = new List<PendingOpCodeOnlyStackAddressWrite<M, ParameterSymbol<M>>>();
            var variables = new List<PendingOpCodeOnlyStackAddressWrite<M, VariableSymbol<M>>>();
            var anonomyous = new List<PendingOpCodeOnlyStackAddressWrite<M, AnonymousSymbol<M>>>();
            var constants = new List<PendingOpCodeOnlyStackAddressWrite<M, ConstantSymbol<M>>>();

            foreach (var write in self)
            {
                switch (write.Symbol.Kind)
                {
                    case SymbolKind.Parameter:
                        parameters.Add(PendingOpCodeOnlyStackAddressWrite.New((ParameterSymbol<M>)write.Symbol, write.WriteLocation));
                        break;

                    case SymbolKind.Variable:
                        variables.Add(PendingOpCodeOnlyStackAddressWrite.New((VariableSymbol<M>)write.Symbol, write.WriteLocation));
                        break;

                    case SymbolKind.Anonymous:
                        anonomyous.Add(PendingOpCodeOnlyStackAddressWrite.New((AnonymousSymbol<M>)write.Symbol, write.WriteLocation));
                        break;

                    case SymbolKind.Constant:
                        constants.Add(PendingOpCodeOnlyStackAddressWrite.New((ConstantSymbol<M>)write.Symbol, write.WriteLocation));
                        break;
                }
            }

            return (
                parameters.GroupBy(i => i.Symbol.Index),
                variables.GroupBy(i => i.Symbol.Name),
                anonomyous.GroupBy(i => i.Symbol.Id),
                constants.GroupBy(i => i.Symbol.TypedValue)
            );
        }

        private static T[] Bundle<T, TBool, TF32, TI32, TU8>(
            IEnumerable<IGrouping<T, TBool>> boolGroups,
            IEnumerable<IGrouping<T, TF32>> f32Groups,
            IEnumerable<IGrouping<T, TI32>> i32Groups,
            IEnumerable<IGrouping<T, TU8>> u8Groups
        ) => IEnumerableExt.Concat(
            boolGroups.Select(i => i.Key),
            f32Groups.Select(i => i.Key),
            i32Groups.Select(i => i.Key),
            u8Groups.Select(i => i.Key)
        ).ToHashSet().ToArray();

        private static void WriteStackAddresses<T, M, S>(
            FinalOpCodeWriter finalOpCodeWriter,
            IReadOnlyDictionary<T, StackAddress<MValue>> dict,
            IEnumerable<IGrouping<T, PendingOpCodeOnlyStackAddressWrite<M, S>>> grouping
        )
            where S : ISymbol<M>
        {
            foreach (var value in grouping)
            {
                var address = dict[value.Key];
                foreach (var writer in value)
                {
                    finalOpCodeWriter.WriteAddress(address.Mark<M>(), writer.WriteLocation);
                }
            }
        }

        private static void WriteOpCodeAddresess(FinalOpCodeWriter finalOpCodeWriter, IEnumerable<PendingOpCodeOnlyOpCodeAddressWrite> addressWrites)
        {
            foreach (var addressWrite in addressWrites)
            {
                finalOpCodeWriter.WriteAddress(addressWrite.Address, addressWrite.WriteLocation);
            }
        }

        public byte[] Finish()
        {
            var (boolParams, boolVars, boolAnons, boolConsts) = Bucket(_bools);
            var (f32Params, f32Vars, f32Anons, f32Consts) = Bucket(_f32s);
            var (i32Params, i32Vars, i32Anons, i32Consts) = Bucket(_i32s);
            var (u8Params, u8Vars, u8Anons, u8Consts) = Bucket(_u8s);

            var paramBundle = Bundle(boolParams, f32Params, i32Params, u8Params);
            var varBundle = Bundle(boolVars, f32Vars, i32Vars, u8Vars);
            var anonBundle = Bundle(boolAnons, f32Anons, i32Anons, u8Anons);
            var constBundle = Bundle(boolConsts, f32Consts, i32Consts, u8Consts);

            var preambleWriter = PreambleOpCodeWriter.New();
            var (varStackAddresses, anonStackAddresses, constStackAddresses) =
                preambleWriter.EnterFunction(varBundle.Length, anonBundle.Length, constBundle.Select(i => i.Item2).ToArray());

            var varAddressDict = varBundle.Zip(varStackAddresses).ToDictionary();
            var anonAddressDict = anonBundle.Zip(anonStackAddresses).ToDictionary();
            var constAddressDict = constBundle.Zip(constStackAddresses).ToDictionary();

            var finalOpCodeWriter = OpWriter.AddPremable(preambleWriter);

            WriteStackAddresses(finalOpCodeWriter, varAddressDict, boolVars);
            WriteStackAddresses(finalOpCodeWriter, varAddressDict, f32Vars);
            WriteStackAddresses(finalOpCodeWriter, varAddressDict, i32Vars);
            WriteStackAddresses(finalOpCodeWriter, varAddressDict, u8Vars);

            WriteStackAddresses(finalOpCodeWriter, anonAddressDict, boolAnons);
            WriteStackAddresses(finalOpCodeWriter, anonAddressDict, f32Anons);
            WriteStackAddresses(finalOpCodeWriter, anonAddressDict, i32Anons);
            WriteStackAddresses(finalOpCodeWriter, anonAddressDict, u8Anons);

            WriteStackAddresses(finalOpCodeWriter, constAddressDict, boolConsts);
            WriteStackAddresses(finalOpCodeWriter, constAddressDict, f32Consts);
            WriteStackAddresses(finalOpCodeWriter, constAddressDict, i32Consts);
            WriteStackAddresses(finalOpCodeWriter, constAddressDict, u8Consts);

            WriteOpCodeAddresess(finalOpCodeWriter, _opCodeLocations);

            return finalOpCodeWriter.GetOpCode();
        }

        private CodeGenState(NodeDef nodeDef, State graphState)
        {
            NodeDef = nodeDef;
            GraphState = graphState;
        }

        public static CodeGenState New(NodeDef nodeDef, State graphState) =>
            new CodeGenState(nodeDef, graphState);

        public static byte[] GenerateCodeForNode(NodeDef nodeDef, State graphState, NodeId node)
        {
            var self = New(nodeDef, graphState);
            self.GenerateCode(node);
            return self.Finish();
        }
    }
}
