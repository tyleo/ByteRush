using ByteRush.Util.Interface;
using System.Collections.Generic;

namespace ByteRush.Util
{
    public sealed class RefEnumeratorAdapter<T> : IRefEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;
        private T _current;

        private RefEnumeratorAdapter(IEnumerator<T> inner) => _inner = inner;

        public ref T Current
        {
            get
            {
                _current = _inner.Current;
                return ref _current;
            }
        }

        public static RefEnumeratorAdapter<T> New(IEnumerator<T> inner) => new RefEnumeratorAdapter<T>(inner);

        public bool MoveNext() => _inner.MoveNext();
    }
}
