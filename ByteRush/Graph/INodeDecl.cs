﻿using System.Collections.Generic;

namespace ByteRush.Graph
{
    public interface INodeDecl
    {
        FullName FullName { get; }

        IReadOnlyList<PortDecl> GetInputs(NodeDef nodeDef);

        IReadOnlyList<PortDecl> GetOutputs(NodeDef nodeDef);

        object DefaultMeta();

        void GenerateCode(
            NodeId nodeId,
            in Node node,
            CodeGen.CodeGenState state
        );
    }
}
