﻿using ByteRush.Utilities;

namespace ByteRush.CodeGen
{
    public sealed class IntInserter
    {
        private readonly ArrayList<byte> _bytes;
        private readonly int _index;

        public IntInserter(ArrayList<byte> bytes, int index)
        {
            _bytes = bytes;
            _index = index;
        }

        public void Write(int value) => ByteUtil.WriteI32(_bytes.Inner, _index, value);
    }
}
