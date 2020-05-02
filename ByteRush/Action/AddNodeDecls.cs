using ByteRush.Graph;
using ByteRush.Util;

namespace ByteRush.Action
{
    public sealed class AddNodeDecls : IAction
    {
        public ActionKind Kind => ActionKind.AddNodeDecls;

        public ArrayList<NodeDecl> NodeDecls { get; }

        private AddNodeDecls(ArrayList<NodeDecl> nodeDecls) => NodeDecls = nodeDecls;

        public static AddNodeDecls New(params NodeDecl[] nodeDecls) =>
            new AddNodeDecls(ArrayList<NodeDecl>.FromArray(nodeDecls));
    }
}
