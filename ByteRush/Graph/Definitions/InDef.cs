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

        public object DefaultMeta() => null;

        public void GenerateCode(
            NodeId nodeId,
            in Node node,
            CodeGenState state
        )
        {
            var outputs = GetOutputs(state.NodeDef);
            int index = 0;
            foreach (var (output, portId) in outputs.Enumerate())
            {
                if (output.Type == TypeKind.Exec) continue;
                state.SetOutputSymbol(
                    OutputPortKey.New(nodeId, OutputPortId.New(portId)),
                    ParameterSymbol<MValue>.New(index)
                );
                index++;
            }

            // Impure Function
            if (outputs.Any() && outputs.First().Type == TypeKind.Exec)
            {
                state.GenerateExecForwards(in node, OutputPortId.New(0));
            }
        }
    }
}
