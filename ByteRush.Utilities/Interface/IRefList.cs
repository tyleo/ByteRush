namespace ByteRush.Utilities.Interface
{
    public interface IRefList<T> : IRefReadOnlyList<T>
    {
        void Add(in T value);

        new ref T this[int i] { get; }
    }
}
