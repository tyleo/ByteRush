using ByteRush.CodeGen;
using ByteRush.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ByteRush.Graph.Definitions
{
    public sealed class InDef : INodeDecl
    {
        public string Name => "In";

        public IReadOnlyList<PortDecl> GetInputs(NodeDef nodeDef) => new List<PortDecl>();

        public IReadOnlyList<PortDecl> GetOutputs(NodeDef nodeDef) => nodeDef.GetInputs(nodeDef);

        public void GenerateCode(
            NodeId nodeId,
            in Node node,
            CodeGen.State state
        )
        {
            var outputs = GetOutputs(state.NodeDef);
            foreach (var (output, portId) in outputs.Enumerate())
            {
                if (output.Type == TypeKind.Exec) continue;
                state.AddSymbol(
                    OutputPortKey.New(nodeId, PortId.New(portId)),
                    VariableSymbol.New(output.Name)
                );
            }

            // Impure Function
            if (outputs.Any() && outputs.First().Type == TypeKind.Exec)
            {
                state.GenerateExecForwards(in node, PortId.New(0));
            }
        }
    }
}
