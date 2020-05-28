using ByteRush.Graph;
using System.Collections.Generic;

namespace ByteRush.Action
{
    public sealed class AddNodeDecls : IAction
    {
        public ActionKind Kind => ActionKind.AddNodeDecls;

        public IEnumerable<(NodeDeclId Id, INodeDecl NodeDecl)> NodeDecls { get; }

        private AddNodeDecls((NodeDeclId Id, INodeDecl NodeDecl)[] nodeDecls) => NodeDecls = nodeDecls;

        public static AddNodeDecls New(params (NodeDeclId Id, INodeDecl NodeDecl)[] nodeDecls) => new AddNodeDecls(nodeDecls);
    }
}
