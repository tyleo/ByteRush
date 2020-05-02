namespace ByteRush.Util.Interface
{
    public interface IRefEnumerable<T>
    {
        IRefEnumerator<T> GetEnumerator();
    }
}
