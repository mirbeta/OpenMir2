using OpenMir2.NativeList.Helpers;
using OpenMir2.NativeList.Interfaces.Entities;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OpenMir2.NativeList.Abstracts
{
    /// <summary>
    /// Basic class for object which hold structure in unmanaged memory.
    /// </summary>
    public abstract unsafe class NativeStructureBase : DisposableBase, INativeStructure
    {
        public const String AlreadyHasAllocatedError = "Memory is already allocated";
        public const String AllocateReturnedUnexpectedValueError = nameof(Allocate) + " - method returns an unexpected result.";

        /// <inheritdoc/>
        public int Size { get; private set; }

        /// <inheritdoc/>
        public bool IsHandleOwner { get; private set; }

        /// <inheritdoc/>
        public IntPtr UnsafeHandle { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfInitialized()
        {
            if (UnsafeHandle != IntPtr.Zero)
            {
                throw new Exception(AlreadyHasAllocatedError);
            }
        }

        /// <summary>
        /// Allocates a new piece of memory by given size.
        /// </summary>
        /// <remarks>
        /// <see cref="IsHandleOwner"/> will be true.
        /// </remarks>
        /// <exception cref="Exception">In case when you try to allocate memory twice.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If input size is less of equal zero.</exception>
        /// <exception cref="Exception">If <see cref="Allocate(int, out int)"/> returns an unexpected value.</exception>
        /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
        protected virtual void Initialize(int size)
        {
            ThrowIfDisposed();
            ThrowIfInitialized();

            ArgumentsGuard.ThrowIfLessOrEqualZero(size);

            UnsafeHandle = Allocate(size);

            if (UnsafeHandle == nint.Zero)
            {
                throw new Exception(AllocateReturnedUnexpectedValueError);
            }

            Size = size;
            IsHandleOwner = true;
        }

        /// <summary>
        /// Copies or Sets memory pointer.
        /// </summary>
        /// <remarks>
        /// This method is quite UNSAFE. Thinks twice before using it.
        /// </remarks>
        /// <exception cref="Exception">In case when you try to set pointer twice.</exception>
        /// <exception cref="ArgumentNullException">If input pointer is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If input size is less of equal zero.</exception>
        /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
        /// <exception cref="Exception">If <see cref="Allocate(int, out int)"/> returns an unexpected value.</exception>
        protected virtual void Initialize(IntPtr pointer, int size, bool makeCopy = true)
        {
            ThrowIfDisposed();
            ThrowIfInitialized();

            NativeGuard.ThrowIfNull(pointer);
            ArgumentsGuard.ThrowIfLessOrEqualZero(size);

            if (makeCopy)
            {
                UnsafeHandle = pointer;
                IsHandleOwner = false;

                GC.SuppressFinalize(this);
            }
            else
            {
                UnsafeHandle = Allocate(size);

                if (UnsafeHandle == IntPtr.Zero)
                {
                    throw new Exception(AllocateReturnedUnexpectedValueError);
                }

                Span<byte> inputSpan = new Span<byte>(pointer.ToPointer(), size);
                Span<byte> destinationSpan = new Span<byte>(UnsafeHandle.ToPointer(), size);

                inputSpan.CopyTo(destinationSpan);

                IsHandleOwner = true;
            }

            Size = size;
        }

        /// <summary>
        /// Allocates new piece of memory and frees the previous one.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">If input size is less of equal zero.</exception>
        /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
        /// <exception cref="Exception">If <see cref="Allocate(int, out int)"/> returns an unexpected value.</exception>
        /// <exception cref="Exception">You try reinitialize the piece of memory without owner rights.</exception>
        protected virtual void ReInitialize(int newSize)
        {
            ThrowIfDisposed();

            if (!IsHandleOwner)
            {
                throw new Exception("This object is not an owner of memory handle.");
            }

            ArgumentsGuard.ThrowIfLessOrEqualZero(newSize);

            nint newHandle = Reallocate(UnsafeHandle, newSize);

            if (newHandle == IntPtr.Zero)
            {
                throw new Exception(AllocateReturnedUnexpectedValueError);
            }

            UnsafeHandle = newHandle;
            Size = newSize;
        }

        /// <summary>
        /// Has to allocate new piece of memory for this object.
        /// </summary>
        /// <param name="size">Needed size.</param>
        /// <returns>Pointer to the allocated memory</returns>
        protected virtual IntPtr Allocate(int size)
        {
            return new IntPtr(NativeMemory.AllocZeroed((nuint)size));
        }

        /// <summary>
        /// Reallocating existing piece of memory to desired size.
        /// </summary>
        /// <remarks>
        /// Algorithm guarantees that the pointer is valid. 
        /// </remarks>
        /// <param name="size">New size.</param>
        /// <returns>Pointer to the allocated memory</returns>
        protected virtual IntPtr Reallocate(IntPtr pointer, int size)
        {
            return new IntPtr(NativeMemory.Realloc(pointer.ToPointer(), (nuint)size));
        }

        /// <summary>
        /// Frees allocated piece of memory.
        /// </summary>
        /// <remarks>
        /// Algorithm guarantees that the pointer is valid. 
        /// </remarks>
        protected virtual void Free(IntPtr pointer)
        {
            NativeMemory.Free(pointer.ToPointer());
        }

        protected override void InternalDispose(bool manual)
        {
            if (IsHandleOwner)
            {
                Free(UnsafeHandle);
            }

            UnsafeHandle = IntPtr.Zero;
            Size = 0;

            base.InternalDispose(manual);
        }
    }
}