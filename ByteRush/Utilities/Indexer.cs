using ByteRush.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ByteRush.Utilities
{
    public sealed class Indexer
    {
        private int nextIndex = 0;
        private readonly Stack<int> freeIndices = new Stack<int>();

        public int GetIndex()
        {
            if (freeIndices.Any()) return freeIndices.Pop();
            return nextIndex++;
        }

        public void FreeIndex(int index) => freeIndices.Push(index);
    }
}
