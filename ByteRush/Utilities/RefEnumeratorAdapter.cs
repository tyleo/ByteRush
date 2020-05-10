using ByteRush.Utilities.Interface;
using System.Collections.Generic;

namespace ByteRush.Utilities
{
    public sealed class RefEnumeratorAdapter<T> : IRefEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;
        private T _current;

        private RefEnumeratorAdapter(IEnumerator<T> inner) => _inner = inner;

        public ref T Current => ref _current;

        public static RefEnumeratorAdapter<T> New(IEnumerator<T> inner) => new RefEnumeratorAdapter<T>(inner);

        public bool MoveNext()
        {
            if (_inner.MoveNext())
            {
                _current = _inner.Current;
                return true;
            }
            return false;
        }
    }
}
