namespace ByteRush.Utilities.Interface
{
    public interface IRefEnumerator<T>
    {
        ref T Current { get; }
        bool MoveNext();
    }
}
