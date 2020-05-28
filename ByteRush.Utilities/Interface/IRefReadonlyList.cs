using System;
using System.Collections.Generic;
using System.Text;

namespace ByteRush.Utilities.Interface
{
    public interface IRefReadOnlyList<T> : IRefCollection<T>
    {
        ref readonly T this[int i] { get; }
    }
}
