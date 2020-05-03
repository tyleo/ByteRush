using ByteRush.Graph;
using ByteRush.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ByteRush.CodeGen
{
    public struct InstructionStreamInserter
    {
        private readonly int _address;
    }

    public interface IStackAddressInserter
    {
        
    }

    public sealed class ConstantAddressInserter : IStackAddressInserter
    {
        private readonly InstructionStreamInserter _writeLocation;
    }

    public sealed class VariableAddressInserter : IStackAddressInserter
    {
        private readonly InstructionStreamInserter _writeLocation;
    }

    public interface ISymbol { }

    public sealed class ConstantSymbol : ISymbol
    {
        private readonly (TypeKind, Variant) inner;
    }

    public sealed class VariableSymbol : ISymbol
    {
        private readonly string _inner;

        private VariableSymbol(string inner) => _inner = inner;

        public static VariableSymbol New(string inner) => new VariableSymbol(inner);
    }

    public sealed class AnonymousSymbol : ISymbol
    {
        private readonly int inner;
    }

    public sealed class AddressRequests
    {
        // Seen constant addresses and the locations they need to be inserted.
        Dictionary<ISymbol, ArrayList<IStackAddressInserter>> stackAddressRequests;
    }
}
