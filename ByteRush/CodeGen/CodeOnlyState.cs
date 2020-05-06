using ByteRush.Graph;
using ByteRush.Utilities;
using ByteRush.Utilities.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ByteRush.CodeGen
{
    public sealed class CodeOnlyState
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


        private bool GenerateCode(NodeId nodeId)
        {
            _generatedNodes.Add(nodeId);
            NodeDef.GetNode(nodeId).GenerateCode(nodeId, this);
            return true;
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

        private CodeOnlyState(NodeDef nodeDef, State graphState)
        {
            NodeDef = nodeDef;
            GraphState = graphState;
        }

        public static CodeOnlyState New(NodeDef nodeDef, State graphState) =>
            new CodeOnlyState(nodeDef, graphState);
    }
}
