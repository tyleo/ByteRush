namespace ByteRush.Util.Interface
{
    public interface IRefEnumerator<T>
    {
        ref T Current { get; }
        bool MoveNext();
    }
}
