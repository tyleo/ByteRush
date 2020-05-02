using ByteRush.Action;
using ByteRush.Util;

namespace ByteRush.Graph
{
    public struct State
    {
        private readonly ArrayList<NodeDecl> _nodeDecls;
        private readonly ArrayList<NodeDef> _nodeDefs;

        public ref NodeDecl GetNodeDecl(int i) => ref _nodeDecls[i];

        private State(
            ArrayList<NodeDecl> nodeDecls,
            ArrayList<NodeDef> nodeDefs
        )
        {
            this._nodeDecls = nodeDecls;
            this._nodeDefs = nodeDefs;
        }

        public static State New() => new State(
            ArrayList<NodeDecl>.New(),
            ArrayList<NodeDef>.New()
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
