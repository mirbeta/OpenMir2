using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace SystemModule.MemoryPool;

public sealed partial class FixedLengthOwner<T>
{
    private class UnderlyingOwner : IMemoryOwner<T>
    {
        private readonly FixedLengthOwner<T> _buffer;
        private readonly bool _own;

        public UnderlyingOwner([NotNull] FixedLengthOwner<T> buffer, bool own)
        {
            _buffer = buffer;
            _own = own;
        }

        public void Dispose()
        {
            if (_own)
                _buffer.Dispose();
        }

        public Memory<T> Memory
        {
            get
            {
                T[] array = _buffer._array;
                if (array == null)
                {
                    throw new ObjectDisposedException(nameof(FixedLengthOwner<T>));
                }

                return new Memory<T>(array, 0, array.Length);
            }
        }
    }
}