using System;
using SystemModule.NativeList.Interfaces.Entities;
using SystemModule.NativeList.Utils;

namespace SystemModule.NativeList.Helpers
{
    public static unsafe class NativeExtensions
    {
        /// <summary>
        /// Creates a new span from the native structure.
        /// </summary>
        /// <remarks>
        /// Be carefully, the result span is not owning the pointer.
        /// </remarks>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ObjectDisposedException"/>
        public static Span<byte> ToSpan(this INativeStructure item)
        {
            ArgumentsGuard.ThrowIfNull(item);
            ArgumentsGuard.ThrowIfDisposed(item);

            return new Span<byte>(item.UnsafeHandle.ToPointer(), item.Size);
        }

        /// <summary>
        /// Creates a new span from the native buffer.
        /// </summary>
        /// <remarks>
        /// Items of span will be the same as is in <see cref="NativeBuffer{TItem}"/>
        /// Be carefully, the result span is not owning the pointer.
        /// </remarks>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ObjectDisposedException"/>
        public static Span<TItem> ToSpan<TItem>(this NativeBuffer<TItem> item) where TItem : unmanaged
        {
            ArgumentsGuard.ThrowIfNull(item);
            ArgumentsGuard.ThrowIfDisposed(item);

            return new Span<TItem>(item.UnsafeHandle.ToPointer(), item.Length);
        }

        /// <summary>
        /// Creates a new span of unmanaged items from the native structure.
        /// </summary>
        /// <remarks>
        /// The length of the new span will be just to keep max possible number of items.
        /// Be carefully, the result span is not owning the pointer.
        /// </remarks>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ObjectDisposedException"/>
        public static Span<TItem> ToSpanOf<TItem>(this INativeStructure item) where TItem : unmanaged
        {
            ArgumentsGuard.ThrowIfNull(item);
            ArgumentsGuard.ThrowIfDisposed(item);

            return new Span<TItem>(item.UnsafeHandle.ToPointer(), item.Size / sizeof(TItem));
        }

        /// <summary>
        /// Returns <see cref="Span{T}"/> structure with desired length (or with computed length).
        /// </summary>
        /// <remarks>
        /// This is stupid remark but the LENGTH is not a SIZE.
        /// <br/> This is a pretty UNSAFE method, because <see cref="Span{T}"/> can stay live after buffer disposing. 
        /// Think twice before using it.
        /// </remarks>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="ArgumentOutOfRangeException">If the length is outside of buffer.</exception>
        public static Span<TItem> ToSpanOf<TItem>(this INativeStructure item, int length) where TItem : unmanaged
        {
            ArgumentsGuard.ThrowIfNull(item);
            ArgumentsGuard.ThrowIfDisposed(item);
            ArgumentsGuard.ThrowIfLessZero(length);

            if (length * sizeof(TItem) > item.Size)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "The desired length is more than the buffer size.");
            }

            return new Span<TItem>(item.UnsafeHandle.ToPointer(), length);
        }

        /// <summary>
        /// Creates an instance of <see cref="NativeBuffer{TItem}"/> and fill it by data from input structure.
        /// </summary>
        /// <remarks>
        /// The length of the new <see cref="NativeBuffer{TItem}"/> will be just to keep max possible number of items.
        /// </remarks>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ObjectDisposedException"/>
        public static NativeBuffer<TItem> ToNativeBuffer<TItem>(this INativeStructure item, bool makeCopy = true) where TItem : unmanaged
        {
            ArgumentsGuard.ThrowIfNull(item);
            ArgumentsGuard.ThrowIfDisposed(item);

            return new NativeBuffer<TItem>(item, item.Size / sizeof(TItem), makeCopy);
        }

        /// <summary>
        /// Creates an instance of <see cref="NativeBuffer{TItem}"/> and fill it by data from input structure.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="ArgumentOutOfRangeException">If the length is outside of buffer.</exception>
        public static NativeBuffer<TItem> ToNativeBuffer<TItem>(this INativeStructure item, int length, bool makeCopy = true) where TItem : unmanaged
        {
            ArgumentsGuard.ThrowIfNull(item);
            ArgumentsGuard.ThrowIfDisposed(item);

            if (length * sizeof(TItem) > item.Size)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "The desired length is more than the buffer size.");
            }

            return new NativeBuffer<TItem>(item, length, makeCopy);
        }
    }
}