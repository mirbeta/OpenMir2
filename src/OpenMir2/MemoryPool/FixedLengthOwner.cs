using System;
using System.Buffers;

namespace OpenMir2.MemoryPool
{
    public sealed partial class FixedLengthOwner<T> : IMemoryOwner<T>
    {
        private T[] _array;
        private readonly int _size;
        private readonly Memory<T> memory;

        public FixedLengthOwner(int size)
        {
            _array = ArrayPool<T>.Shared.Rent(size);
            _size = size;
            memory = new Memory<T>(_array, 0, size);
        }

        public T[] Buffer
        {
            get
            {
                return _array;
            }
        }

        public Memory<T> Memory
        {
            get
            {
                return memory;
                //var array = _array;
                //if (array == null)
                //{
                //    throw new ObjectDisposedException(nameof(FixedLengthOwner<T>));
                //}

                //return new Memory<T>(array, 0, _size);
            }
        }

        public IMemoryOwner<T> GetUnderlyingBuffer(bool own = false) => new UnderlyingOwner(this, own);

        public void Dispose()
        {
            T[] array = _array;
            if (array == null)
            {
                return;
            }

            _array = null;
            ArrayPool<T>.Shared.Return(array);
        }
    }
}