using ByteRush.Graph;
using ByteRush.Utilities;
using System.Collections.Generic;

namespace ByteRush.Action
{
    public sealed class AddNode : IAction
    {
        public ActionKind Kind => ActionKind.AddNode;

        public NodeDeclId NodeDefId { get; }
        public NodeDeclId NodeDeclId { get; }

        private AddNode(NodeDeclId nodeDeclId, NodeDeclId nodeDefId)
        {
            NodeDeclId = nodeDeclId;
            NodeDefId = nodeDefId;
        }

        public static AddNode New(NodeDeclId nodeDeclId, NodeDeclId nodeDefId) =>
            new AddNode(nodeDeclId, nodeDefId);
    }
}
