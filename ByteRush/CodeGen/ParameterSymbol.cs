namespace ByteRush.CodeGen
{
    public sealed class ParameterSymbol<T> : ISymbol<T>
    {
        public int Index { get; }

        public SymbolKind Kind => SymbolKind.Parameter;

        private ParameterSymbol(int index) => Index = index;

        public static ParameterSymbol<T> New(int index) => new ParameterSymbol<T>(index);

        public ISymbol<U> Mark<U>() => ParameterSymbol<U>.New(Index);

        public void Release() { }
    }
}
