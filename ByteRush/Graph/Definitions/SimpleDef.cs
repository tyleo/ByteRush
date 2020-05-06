using System.Collections.Generic;

namespace ByteRush.Graph.Definitions
{
    public abstract class SimpleDef : INodeDecl
    {
        public abstract string Name { get; }

        private readonly PortDecl[] _inputs;
        public IReadOnlyList<PortDecl> GetInputs(NodeDef nodeDef) => _inputs;

        private readonly PortDecl[] _outputs;
        public IReadOnlyList<PortDecl> GetOutputs(NodeDef nodeDef) => _outputs;

        public virtual object DefaultMeta() => null;

        protected SimpleDef(PortDecl[] inputs, PortDecl[] outputs)
        {
            _inputs = inputs;
            _outputs = outputs;
        }

        public abstract void GenerateCode(NodeId nodeId, in Node node, CodeGen.CodeOnlyState state);
    }

    public abstract class SimpleDef<T> : SimpleDef
    {
        public sealed override object DefaultMeta() => DefaultMetaTyped();

        protected abstract T DefaultMetaTyped();

        protected SimpleDef(PortDecl[] inputs, PortDecl[] outputs) : base(inputs, outputs) { }
    }
}
