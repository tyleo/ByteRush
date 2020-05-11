using ByteRush.Utilities.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ByteRush.Utilities
{
    public sealed class ArrayList<T> : IRefList<T>, IEnumerable<T>
    {
        private const int GROWTH_FACTOR = 2;

        public T[] Inner { get; private set; }

        public int Count { get; private set; }

        ref readonly T IRefReadOnlyList<T>.this[int i] => ref Inner[i];

        public ref T this[int i] => ref Inner[i];

        private ArrayList(T[] values, int size)
        {
            Inner = values;
            Count = size;
        }

        public void Add(in T value)
        {
            EnsureOverhead(1);
            Inner[Count] = value;
            Count++;
        }

        public static ArrayList<T> New() => new ArrayList<T>(new T[0], 0);

        public static ArrayList<T> FromArray(T[] array) => new ArrayList<T>(array, array.Length);

        public void Grow(int growthAmount)
        {
            EnsureOverhead(growthAmount);
            Count += growthAmount;
        }

        public void EnsureOverhead(int overhead)
        {
            if (Count + overhead > Inner.Length)
            {
                var newValues = new T[(Count + overhead) * GROWTH_FACTOR];
                Array.Copy(Inner, newValues, Count);
                Inner = newValues;
            }
        }

        public T[] ToArray() => Inner.Take(Count).ToArray();

        public IRefEnumerator<T> GetEnumerator() => ArrayListEnumerator<T>.New(this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Inner.Take(Count).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Inner.Take(Count).GetEnumerator();
    }

    public static class ArrayList
    {
        public static ArrayList<T> New<T>() => ArrayList<T>.New();

        public static ArrayList<T> FromArray<T>(T[] array) => ArrayList<T>.FromArray(array);
    }
}
