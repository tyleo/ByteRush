namespace ByteRush.Utilities.Interface
{
    public interface IInEquatable<T>
    {
        bool Equals(in T other);
    }
}
