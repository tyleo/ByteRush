using ByteRush.Graph;
using ByteRush.Utilities;
using System.Collections.Generic;

namespace ByteRush.Action
{
    public sealed class AddNode : IAction
    {
        public ActionKind Kind => ActionKind.AddNode;

        public NodeDeclId FunctionId { get; }
        public NodeDeclId NodeDeclId { get; }

        private AddNode(NodeDeclId nodeDeclId, NodeDeclId functionId)
        {
            NodeDeclId = nodeDeclId;
            FunctionId = functionId;
        }

        public static AddNode New(NodeDeclId nodeDeclId, NodeDeclId functionId) =>
            new AddNode(nodeDeclId, functionId);
    }
}
