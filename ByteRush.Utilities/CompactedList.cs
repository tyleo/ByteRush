﻿using ByteRush.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ByteRush.Utilities
{
    public sealed class CompactedList<T>
    {
        private readonly ArrayList<T> _inner = ArrayList<T>.New();
        private readonly Indexer _indexer = Indexer.New();

        private CompactedList() { }
        public static CompactedList<T> New() => new CompactedList<T>();

        public int Add(T item)
        {
            var index = _indexer.GetIndex();
            if (_inner.Count >= index)
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

        public IEnumerable<T> Items() => _inner
            .Enumerate()
            .Where(i => _indexer.IsActive(i.Index))
            .Select(i => i.Value);
    }
}
