using System.Collections.Generic;

namespace ByteRush.Graph
{
    public interface INodeDecl
    {
        FullName FullName { get; }

        IReadOnlyList<PortDecl> GetInputs(FunctionDef function);

        IReadOnlyList<PortDecl> GetOutputs(FunctionDef function);

        object DefaultMeta();

        void GenerateCode(
            NodeId nodeId,
            in Node node,
            CodeGen.CodeGenState state
        );
    }
}
