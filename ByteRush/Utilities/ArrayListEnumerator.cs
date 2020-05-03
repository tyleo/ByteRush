using ByteRush.Utilities.Interface;

namespace ByteRush.Utilities
{
    public sealed class ArrayListEnumerator<T> : IRefEnumerator<T>
    {
        private readonly ArrayList<T> _inner;
        private int _index = -1;

        public ref T Current => ref _inner[_index];

        private ArrayListEnumerator(ArrayList<T> inner) => _inner = inner;

        public static ArrayListEnumerator<T> New(ArrayList<T> inner) => new ArrayListEnumerator<T>(inner);

        public bool MoveNext()
        {
            _index++;
            return _index < _inner.Count;
        }
    }
}
