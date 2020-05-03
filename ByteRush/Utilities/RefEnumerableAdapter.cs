using ByteRush.Utilities.Interface;
using System.Collections.Generic;

namespace ByteRush.Utilities
{
    public sealed class RefEnumerableAdapter<T> : IRefEnumerable<T>
    {
        private readonly IEnumerable<T> _inner;

        private RefEnumerableAdapter(IEnumerable<T> inner) => _inner = inner;

        public static RefEnumerableAdapter<T> New(IEnumerable<T> inner) => new RefEnumerableAdapter<T>(inner);

        public IRefEnumerator<T> GetEnumerator() => RefEnumeratorAdapter<T>.New(_inner.GetEnumerator());
    }
}
