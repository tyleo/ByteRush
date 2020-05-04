using ByteRush.Graph;
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
        public Graph.State GraphState { get; }
        public CodeOnlyOpCodeWriter OpWriter { get; }

        private int _currentAnonomyous;

        //private readonly IList<(OpCodeOnlyAddress, ISymbol)> _allSymbols;
        private readonly IDictionary<InputPortKey, int> _execBranchTerminationPoints;
        private readonly IDictionary<OutputPortKey, ISymbol> _portToSymbol;
        private readonly ISet<NodeId> _generatedNodes;
        private readonly ISet<NodeId> _generatedExpressions;

        public void SetOutputSymbol(OutputPortKey key, ISymbol symbol)
        {
            _portToSymbol.Add(key, symbol);
        }
        public ISymbol GetSymbol(OutputPortKey key) => _portToSymbol[key];

        public ISymbol RetainAnonomyous() => AnonymousSymbol.New(_currentAnonomyous++, () => _currentAnonomyous--);

        private bool GenerateCode(NodeId nodeId)
        {
            if (_generatedNodes.Contains(nodeId)) return false;
            _generatedNodes.Add(nodeId);
            NodeDef.GetNode(nodeId).GenerateCode(nodeId, this);
            return true;
        }

        public ISymbol GenerateDataBack(
            in Node currentNode,
            PortId inputPortId
        )
        {
            // TODO: This needs to account for constants
            ref readonly var output = ref NodeDef.GetEdge(currentNode.GetInput(inputPortId).GetEdge(EdgeId.New(0)))._output;
            if (GenerateCode(output.Node)) _generatedExpressions.Add(output.Node);
            return GetSymbol(OutputPortKey.New(output.Node, output.Port));
        }

        public void GenerateExecForwards(
            in Node currentNode,
            PortId outputPortId
        )
        {
            ref readonly var output = ref currentNode.GetOutput(outputPortId);
            if (output.EdgeCount == 0)
            {
                return;
            }

            void BeginImpureNode()
            {
                _generatedNodes.RemoveRange(_generatedExpressions);
                _generatedExpressions.Clear();
            }

            ref readonly var input = ref NodeDef.GetEdge(output.GetEdge(EdgeId.New(0)))._input;
            var nodeId = input.Node;
            ref readonly var node = ref NodeDef.GetNode(input.Node);
            ref readonly var port = ref node.GetInput(input.Port);
            if (port.EdgeCount == 1)
            {
                BeginImpureNode();
                GenerateCode(nodeId);
                return;
            }

            var portKey = InputPortKey.New(input.Node, input.Port);
            var remainingJoins = _execBranchTerminationPoints.GetOrAdd(portKey, port.EdgeCount);
            remainingJoins--;
            _execBranchTerminationPoints[portKey] = remainingJoins;
            if (remainingJoins == 0)
            {
                BeginImpureNode();
                GenerateCode(nodeId);
                return;
            }
        }

        private CodeOnlyState()
        {
            _currentAnonomyous = 0;
        }

        public static CodeOnlyState New() => new CodeOnlyState();
    }
}
