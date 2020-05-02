using ByteRush.Util.Interface;
using System;
using System.Linq;

namespace ByteRush.Util
{
    public sealed class ArrayList<T> : IRefList<T>
    {
        private const int GROWTH_FACTOR = 2;

        public T[] Inner { get; private set; }

        public int Length { get; private set; }

        private ArrayList(T[] values, int size)
        {
            Inner = values;
            Length = size;
        }

        public void Add(in T value)
        {
            EnsureOverhead(1);
            Inner[Length] = value;
            Length++;
        }

        public static ArrayList<T> New() => new ArrayList<T>(new T[0], 0);

        public static ArrayList<T> FromArray(T[] array) => new ArrayList<T>(array, array.Length);

        public void Grow(int growthAmount)
        {
            EnsureOverhead(growthAmount);
            Length += growthAmount;
        }

        public void EnsureOverhead(int overhead)
        {
            if (Length + overhead > Inner.Length)
            {
                var newValues = new T[(Length + overhead) * GROWTH_FACTOR];
                Array.Copy(Inner, newValues, Length);
                Inner = newValues;
            }
        }

        public T[] ToArray() => Inner.Take(Length).ToArray();

        public ref T this[int i] => ref Inner[i];
    }
}
