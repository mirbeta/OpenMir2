using System;
using System.Buffers;

namespace GameGate
{
    public class UserMemoryPool<T> : MemoryPool<T>
    {
        public override int MaxBufferSize => Int32.MaxValue;

        public static new UserMemoryPool<T>.Impl Shared { get; } = new UserMemoryPool<T>.Impl();

        protected override void Dispose(bool disposing) { }

        public override IMemoryOwner<T> Rent(int minBufferSize) => RentCore(minBufferSize);

        private Rental RentCore(int minBufferSize) => new Rental(minBufferSize);

        public sealed class Impl : UserMemoryPool<T>
        {
            public new Rental Rent(int minBufferSize) => RentCore(minBufferSize);
        }

        // Struct implements the interface so it can be boxed if necessary.
        public struct Rental : IMemoryOwner<T>
        {
            private T[] _array;

            public Rental(int minBufferSize)
            {
                _array = ArrayPool<T>.Shared.Rent(minBufferSize);
            }

            public Memory<T> Memory
            {
                get
                {
                    if (_array == null)
                        throw new ObjectDisposedException("");

                    return new Memory<T>(_array);
                }
            }

            public void Dispose()
            {
                if (_array != null)
                {
                    ArrayPool<T>.Shared.Return(_array);
                    _array = null;
                }
            }
        }
    }
}
