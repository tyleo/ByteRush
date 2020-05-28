using ByteRush.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ByteRush.Utilities
{
    public sealed class Indexer
    {
        private int nextIndex = 0;
        private readonly HashSet<int> freeIndices = new HashSet<int>();

        public int GetIndex()
        {
            if (freeIndices.Any())
            {
                var result = freeIndices.First();
                freeIndices.Remove(result);
                return result;
            }
            return nextIndex++;
        }

        public void FreeIndex(int index) => freeIndices.Add(index);

        public bool IsActive(int index) => index < nextIndex && !freeIndices.Contains(index);

        private Indexer() { }

        public static Indexer New() => new Indexer();
    }
}
