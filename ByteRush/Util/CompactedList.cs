using System.Collections.Generic;

namespace ByteRush.Util
{
    public sealed class CompactedList<T>
    {
        private readonly ArrayList<T> _inner = ArrayList<T>.New();
        private readonly Indexer _indexer = new Indexer();

        public int Add(T item)
        {
            var index = _indexer.GetIndex();
            if (_inner.Length >= index)
            {
                _inner.Add(item);
            }
            else
            {
                _inner[index] = item;
            }
            return index;
        }

        public void Remove(int index)
        {
            _inner[index] = default;
            _indexer.FreeIndex(index);
        }

        public ref T this[int i] => ref _inner[i];
    }
}
