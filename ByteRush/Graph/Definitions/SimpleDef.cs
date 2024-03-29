﻿using System.Collections.Generic;

namespace ByteRush.Graph.Definitions
{
    public abstract class SimpleDef : INodeDecl
    {
        public abstract FullName FullName { get; }

        private readonly PortDecl[] _inputs;
        public IReadOnlyList<PortDecl> GetInputs(FunctionDef function) => _inputs;

        private readonly PortDecl[] _outputs;
        public IReadOnlyList<PortDecl> GetOutputs(FunctionDef function) => _outputs;

        public virtual object DefaultMeta() => new object();

        protected SimpleDef(PortDecl[] inputs, PortDecl[] outputs)
        {
            _inputs = inputs;
            _outputs = outputs;
        }

        public abstract void GenerateCode(NodeId nodeId, in Node node, CodeGen.CodeGenState state);
    }

    public abstract class SimpleDef<T> : SimpleDef
    {
        public sealed override object DefaultMeta() => DefaultMetaTyped();

        protected abstract T DefaultMetaTyped();

        protected SimpleDef(PortDecl[] inputs, PortDecl[] outputs) : base(inputs, outputs) { }
    }
}
