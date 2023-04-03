using System;
using System.Collections;
using System.Collections.Generic;
using SystemModule.NativeList.Abstracts;
using SystemModule.NativeList.Interfaces.Entities;

namespace SystemModule.NativeList.Utils
{
    /// <summary>
    /// This is like a array but in the native heap.
    /// </summary>
    /// <remarks>
    /// This guy can take zero length just to not make exclusions for the zero length cases (but this is pointless actually)
    /// <br/>But I'm not sure that the system allocator on another system can allocate zero length piece of memory (Windows can)
    /// </remarks>
    public unsafe class NativeBuffer<TItem> : OwnedStructureBase, IEnumerable<TItem> where TItem : unmanaged
    {
        /// <summary>
        /// Just to have an ability to LINQ this thing.
        /// </summary>
        private class NativeBufferEnumerator : IEnumerator<TItem>
        {
            private NativeBuffer<TItem> _buffer;
            private int _index;

            public NativeBufferEnumerator(NativeBuffer<TItem> owner)
            {
                _buffer = owner;

                Reset();
            }

            public TItem Current => _buffer[_index];

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                _index++;

                return _index < _buffer.Length;
            }

            public void Reset()
            {
                _index = -1;
            }

            public void Dispose()
            {
                Reset();
                _buffer = null;
            }
        }

        /// <summary>
        /// Initialize an empty buffer with desired length.
        /// </summary>
        /// <remarks>
        /// This is stupid remark but the LENGTH is not a SIZE.
        /// </remarks>
        /// <exception cref="ArgumentException">If length is less then zero.</exception>
        public NativeBuffer(int length)
        {
            Initialize(length * sizeof(TItem));

            Length = length;
        }

        /// <summary>
        /// Initialize a buffer from the input pointer with desired length.
        /// </summary>
        /// <remarks>
        /// This is stupid remark but the LENGTH is not a SIZE.
        /// <br/> This is a pretty UNSAFE constructor. Think twice before using it.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If pointer is null.</exception>
        /// <exception cref="ArgumentException">If length is less then zero.</exception>
        public NativeBuffer(IntPtr pointer, int length, bool makeCopy = true)
        {
            Initialize(pointer, length * sizeof(TItem), makeCopy);

            Length = length;
        }

        /// <summary>
        /// Creates a new buffer based on the input native structure with zero offset and input length.
        /// </summary>
        /// <remarks>
        /// This is stupid remark but the LENGTH is not a SIZE.
        /// </remarks>
        /// <exception cref="ArgumentException">If length is less then zero.</exception>
        /// <exception cref="ArgumentNullException">If the input structure is null.</exception>
        /// <exception cref="ObjectDisposedException">If the input structure is disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If a new buffer is bigger than the basic structure.</exception>
        public NativeBuffer(INativeStructure basic, int length, bool makeCopy = true) : this(basic, 0, length, makeCopy)
        { }

        /// <summary>
        /// Creates a new buffer based on the input native structure with desired offset and input length.
        /// </summary>
        /// <remarks>
        /// This is stupid remark but the LENGTH is not a SIZE, but offset in bytes.
        /// </remarks>
        /// <exception cref="ArgumentException">If length is less then zero.</exception>
        /// <exception cref="ArgumentNullException">If the input structure is null.</exception>
        /// <exception cref="ObjectDisposedException">If the input structure is disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If a new buffer is bigger than the basic structure.</exception>
        public NativeBuffer(INativeStructure basic, int offset, int length, bool makeCopy = true)
        {
            Initialize(basic, offset, length * sizeof(TItem), makeCopy);

            Length = length;
        }

        /// <summary>
        /// Gets structure by index.
        /// </summary>
        /// <remarks>
        /// Still have no idea how it works, but it works.
        /// </remarks>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="ArgumentOutOfRangeException">If you tries to get something outside of structure.</exception>
        public ref TItem this[int index]
        {
            get
            {
                ThrowIfDisposed();

                if (index < 0 || index >= Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return ref *(((TItem*)UnsafeHandle.ToPointer()) + index);
            }
        }

        /// <summary>
        /// Returns number of elements stored in buffer.
        /// </summary>
        /// <remarks>
        /// Do not confuse with size.
        /// </remarks>
        public int Length { get; private set; }

        /// <inheritdoc/>
        /// <exception cref="ObjectDisposedException"/>
        public IEnumerator<TItem> GetEnumerator()
        {
            ThrowIfDisposed();

            return new NativeBufferEnumerator(this);
        }

        /// <inheritdoc/>
        /// <exception cref="ObjectDisposedException"/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected override void InternalDispose(bool manual)
        {
            Length = 0;

            base.InternalDispose(manual);
        }
    }
}