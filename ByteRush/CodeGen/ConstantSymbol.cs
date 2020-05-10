using ByteRush.Graph;
using ByteRush.Utilities;

namespace ByteRush.CodeGen
{
    public sealed class ConstantSymbol<T> : ISymbol<T>
    {
        public (TypeKind, Value) TypedValue { get; }

        public SymbolKind Kind => SymbolKind.Constant;

        private ConstantSymbol((TypeKind, Value) typedValue) => TypedValue = typedValue;

        public static ConstantSymbol<T> New((TypeKind, Value) typedValue) => new ConstantSymbol<T>(typedValue);

        public ISymbol<U> Mark<U>() => ConstantSymbol<U>.New(TypedValue);

        public void Release() { }
    }
}
