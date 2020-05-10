namespace ByteRush.CodeGen
{
    public sealed class VariableSymbol<T> : ISymbol<T>
    {
        public string Name { get; }

        public SymbolKind Kind => SymbolKind.Variable;

        private VariableSymbol(string name) => Name = name;

        public static VariableSymbol<T> New(string name) => new VariableSymbol<T>(name);

        public ISymbol<U> Mark<U>() => VariableSymbol<U>.New(Name);

        public void Release() { }
    }
}
