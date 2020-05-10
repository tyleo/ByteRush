namespace ByteRush.CodeGen
{
    public interface ISymbol<T>
    {
        SymbolKind Kind { get; }
        ISymbol<U> Mark<U>();
        void Release();
    }
}
