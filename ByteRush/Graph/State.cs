using ByteRush.Action;
using ByteRush.Utilities;

namespace ByteRush.Graph
{
    public sealed class State
    {
        private readonly ArrayList<INodeDecl> _nodeDecls;

        public INodeDecl GetNodeDecl(NodeDeclId i) => _nodeDecls[i.Int];

        private State(
            ArrayList<INodeDecl> nodeDecls
        )
        {
            this._nodeDecls = nodeDecls;
        }

        public static State New() => new State(
            ArrayList<INodeDecl>.New()
        );

        public void Reduce(IAction action)
        {
            switch (action.Kind)
            {
                case ActionKind.AddNodeDecls:
                    {
                        var addNodeDecls = (AddNodeDecls)action;
                        //addNodeDecls.NodeDecls;
                    }
                    break;
            }
        }
    }
}
