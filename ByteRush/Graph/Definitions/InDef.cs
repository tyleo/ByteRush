using ByteRush.CodeGen;
using ByteRush.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ByteRush.Graph.Definitions
{
    public sealed class InDef : INodeDecl
    {
        private static FullName StaticFullName { get; } = FullName.FromLibEnd("System", "In");
        public static NodeDeclId Id => StaticFullName.NodeDeclId();
        public FullName FullName => StaticFullName;

        public IReadOnlyList<PortDecl> GetInputs(FunctionDef function) => new List<PortDecl>();

        public IReadOnlyList<PortDecl> GetOutputs(FunctionDef function) => function.GetInputs(function);

        public object DefaultMeta() => null;

        public void GenerateCode(
            NodeId nodeId,
            in Node node,
            CodeGenState state
        )
        {
            var outputs = GetOutputs(state.Function);
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
