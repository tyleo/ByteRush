namespace ByteRush.Utilities.Interface
{
    public interface IRefCollection<T> : IRefEnumerable<T>
    {
        int Count { get; }
    }
}
