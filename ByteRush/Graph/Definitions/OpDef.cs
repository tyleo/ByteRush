using System.Collections.Generic;

namespace ByteRush.Graph.Definitions
{
    public abstract class OpDef : INodeDecl
    {
        public abstract string Name { get; }

        private readonly PortDecl[] _inputs;
        public IReadOnlyList<PortDecl> GetInputs(NodeDef nodeDef) => _inputs;

        private readonly PortDecl[] _outputs;
        public IReadOnlyList<PortDecl> GetOutputs(NodeDef nodeDef) => _outputs;

        protected OpDef(PortDecl[] inputs, PortDecl[] outputs)
        {
            _inputs = inputs;
            _outputs = outputs;
        }

        public abstract void GenerateCode(NodeId nodeId, in Node node, CodeGen.State state);
    }
}
