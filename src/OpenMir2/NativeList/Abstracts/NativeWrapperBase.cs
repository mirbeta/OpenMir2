using System;
using System.Runtime.CompilerServices;
using OpenMir2.NativeList.Helpers;
using OpenMir2.NativeList.Interfaces.Entities;

namespace OpenMir2.NativeList.Abstracts
{
    /// <summary>
    /// Special wrapper to allocate structure in native memory.
    /// </summary>
    public abstract unsafe class NativeWrapperBase<TStructure> : OwnedStructureBase where TStructure : unmanaged
    {
        private const String SizesAreNotCorrectError = "Input size has to be more or equal to wrapped structure.";

        /// <summary>
        /// Returns ref to the structure internally without any checks.
        /// </summary>
        protected ref TStructure InternalValue
        {
            get
            {
                return ref *(TStructure*)UnsafeHandle.ToPointer();
            }
        }

        /// <summary>
        /// Initializes object with default structure size.
        /// </summary>
        protected virtual void Initialize()
        {
            Initialize(sizeof(TStructure));
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">If input size is less then structure size.</exception>
        protected override void Initialize(int size)
        {
            ArgumentsGuard.ThrowIfLessZero(size);
            ThrowIfSizeIsLessThanStructure(size);

            base.Initialize(size);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">If input size is less then structure size.</exception>
        protected override void Initialize(IntPtr pointer, int size, bool makeCopy = true)
        {
            ArgumentsGuard.ThrowIfLessZero(size);
            ThrowIfSizeIsLessThanStructure(size);

            base.Initialize(pointer, size, makeCopy);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">If input size is less then structure size.</exception>
        protected override void Initialize(INativeStructure basic, int offset, int size, bool makeCopy = true)
        {
            ArgumentsGuard.ThrowIfLessZero(size);
            ThrowIfSizeIsLessThanStructure(size);

            base.Initialize(basic, offset, size, makeCopy);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">If input size is less then structure size.</exception>
        protected override void ReInitialize(int newSize)
        {
            ArgumentsGuard.ThrowIfLessZero(newSize);
            ThrowIfSizeIsLessThanStructure(newSize);

            base.ReInitialize(newSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfSizeIsLessThanStructure(int value, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value < sizeof(TStructure))
            {
                throw new ArgumentOutOfRangeException(name, SizesAreNotCorrectError);
            }
        }
    }
}