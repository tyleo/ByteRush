using ByteRush.Graph;
using System.Collections.Generic;

namespace ByteRush.Action
{
    public sealed class AddNodeDecls : IAction
    {
        public ActionKind Kind => ActionKind.AddNodeDecls;

        public IEnumerable<INodeDecl> NodeDecls { get; }

        private AddNodeDecls(INodeDecl[] nodeDecls) => NodeDecls = nodeDecls;

        public static AddNodeDecls New(params INodeDecl[] nodeDecls) => new AddNodeDecls(nodeDecls);
    }
}
