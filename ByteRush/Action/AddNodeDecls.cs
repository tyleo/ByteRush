using ByteRush.Graph;
using ByteRush.Utilities;

namespace ByteRush.Action
{
    public sealed class AddNodeDecls : IAction
    {
        public ActionKind Kind => ActionKind.AddNodeDecls;

        public ArrayList<INodeDecl> NodeDecls { get; }

        private AddNodeDecls(ArrayList<INodeDecl> nodeDecls) => NodeDecls = nodeDecls;

        public static AddNodeDecls New(params INodeDecl[] nodeDecls) =>
            new AddNodeDecls(ArrayList<INodeDecl>.FromArray(nodeDecls));
    }
}
