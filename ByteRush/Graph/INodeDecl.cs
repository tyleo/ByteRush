using System.Collections.Generic;

namespace ByteRush.Graph
{
    public interface INodeDecl
    {
        string Name { get; }

        IReadOnlyList<PortDecl> GetInputs(NodeDef nodeDef);

        IReadOnlyList<PortDecl> GetOutputs(NodeDef nodeDef);

        void GenerateCode(
            NodeId nodeId,
            in Node node,
            CodeGen.State state
        );
    }
}
