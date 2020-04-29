using System;
using System.Linq;

namespace ByteRush.Util
{
    public sealed class ArrayList<T>
    {
        private const int GROWTH_FACTOR = 2;

        public T[] Inner { get; private set; }

        public int Length { get; private set; }

        private ArrayList(T[] values, int size)
        {
            Inner = values;
            Length = size;
        }

        public ArrayList<T> Add(T value)
        {
            EnsureOverhead(1);
            Inner[Length] = value;
            Length++;
            return this;
        }

        public static ArrayList<T> New() => new ArrayList<T>(new T[0], 0);

        public ArrayList<T> Grow(int growthAmount)
        {
            EnsureOverhead(growthAmount);
            Length += growthAmount;
            return this;
        }

        public ArrayList<T> EnsureOverhead(int overhead)
        {
            if (Length + overhead > Inner.Length)
            {
                var newValues = new T[(Length + overhead) * GROWTH_FACTOR];
                Array.Copy(Inner, newValues, Length);
                Inner = newValues;
            }
            return this;
        }

        public T[] ToArray() => Inner.Take(Length).ToArray();
    }
}
