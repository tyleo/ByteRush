using ByteRush.Utilities;
using System;

namespace ByteRush.Graph.Definitions
{
    public static class Def
    {
        public static readonly NodeDeclId ADD_ID =          new Guid("ca49a1c6-995e-4bd0-aa4e-b4c0f338adfd").NodeDeclId();
        public static readonly NodeDeclId FOR_ID =          new Guid("25a4d1a3-2e55-43fe-bc72-dc5007565e33").NodeDeclId();
        public static readonly NodeDeclId IF_ID =           new Guid("423c8fe0-2864-4acc-b7fe-4d12f21c9140").NodeDeclId();
        public static readonly NodeDeclId IN_ID =           new Guid("fb66f685-2cd2-4e26-9854-494bb2dc6a92").NodeDeclId();
        public static readonly NodeDeclId LESS_THAN_ID =    new Guid("b6848e7a-071c-4eec-a555-e739a0cdfba7").NodeDeclId();
        public static readonly NodeDeclId SET_ID =          new Guid("47278bdb-d571-4f35-ac2f-56455490c0b2").NodeDeclId();
        public static readonly NodeDeclId GET_ID =          new Guid("632137c3-e185-48f7-967d-f119865dca69").NodeDeclId();

        public static (NodeDeclId, INodeDecl)[] ALL => Util.NewArray<(NodeDeclId, INodeDecl)>(
            (ADD_ID, AddDef.New()),
            (FOR_ID, ForDef.New()),
            (IF_ID, IfDef.New()),
            (IN_ID, InDef.New()),
            (LESS_THAN_ID, LessThanDef.New()),
            (SET_ID, SetDef.New()),
            (GET_ID, GetDef.New())
        );
    }
}
