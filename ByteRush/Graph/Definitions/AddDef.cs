﻿using ByteRush.CodeGen;
using ByteRush.Utilities;

namespace ByteRush.Graph.Definitions
{
    public sealed class AddDef : OpDef
    {
        public override string Name => "Add";

        private AddDef() : base(
            Util.NewArray(PortDecl.New("lhs", TypeKind.I32), PortDecl.New("lhs", TypeKind.I32)),
            Util.NewArray(PortDecl.New("sum", TypeKind.I32))
        ) { }

        public static AddDef New() => new AddDef();

        public override void GenerateCode(
            NodeId nodeId,
            in Node node,
            CodeOnlyState state
        )
        {
            var lhsSym = state.GenerateDataBack<MI32>(in node, PortId.New(0));
            var rhsSym = state.GenerateDataBack<MI32>(in node, PortId.New(1));

            lhsSym.Release();
            rhsSym.Release();

            var (_, lhs, rhs, @return) = state.OpWriter.AddI32();

            var returnPortEdges = node.GetOutput(PortId.New(0)).EdgeCount;
            var returnSym = state.RetainAnonomyous<MI32>(returnPortEdges);

            state.QueueSymbolAddressWrite(lhsSym, lhs);
            state.QueueSymbolAddressWrite(rhsSym, rhs);
            state.QueueSymbolAddressWrite(returnSym, @return);

            state.SetOutputSymbol(OutputPortKey.New(nodeId, PortId.New(0)), returnSym);
        }
    }
}
