using System;
using System.Buffers;

namespace SystemModule.MemoryPool
{
    public sealed class FixedLengthMemoryPool<T> : MemoryPool<T>
    {
        public static new FixedLengthMemoryPool<T> Shared { get; } = new FixedLengthMemoryPool<T>();

        protected override void Dispose(bool disposing) { }

        public override IMemoryOwner<T> Rent(int bufferSize = -1)
        {
            if (bufferSize == -1)
            {
                bufferSize = 4096;
            }
            else if ((uint)bufferSize > MaxBufferSize)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            return new FixedLengthOwner<T>(bufferSize);
        }

        public override int MaxBufferSize { get; } = int.MaxValue;
    }
}