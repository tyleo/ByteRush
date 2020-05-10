using ByteRush.Utilities;

using static ByteRush.Utilities.DebugUtil;

namespace ByteRush.CodeGen
{
    public sealed class AnonymousSymbol<T> : ISymbol<T>
    {
        public int Id { get; }
        private readonly Box<int> _uses;
        private System.Action _release;

        public SymbolKind Kind => SymbolKind.Anonymous;

        private AnonymousSymbol(
            int id,
            Box<int> uses,
            System.Action release
        )
        {
            Id = id;
            _uses = uses;
            _release = release;
        }

        public static AnonymousSymbol<T> New(
            int id,
            int uses,
            System.Action release
        ) => new AnonymousSymbol<T>(id, Box.New(uses), release);

        public ISymbol<U> Mark<U>() => new AnonymousSymbol<U>(Id, _uses, _release);

        public void Release()
        {
            _uses._value--;
            if (_uses._value <= 0)
            {
                _release();
                _release = () => Fail("Releasing anonomyous symbol multiple times!");
            }
        }
    }
}
