using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ByteRush.Utilities.Extensions
{
    public static class GenericExt
    {
        public static IEnumerable<T> AsOneItemEnumerable<T>(this T self)
        {
            yield return self;
        }
    }
}
