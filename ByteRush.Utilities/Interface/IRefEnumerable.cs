namespace ByteRush.Utilities.Interface
{
    public interface IRefEnumerable<T>
    {
        IRefEnumerator<T> GetEnumerator();
    }
}
