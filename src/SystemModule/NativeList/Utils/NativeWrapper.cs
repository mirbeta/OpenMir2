using System;
using SystemModule.NativeList.Abstracts;
using SystemModule.NativeList.Interfaces.Entities;

namespace SystemModule.NativeList.Utils;

/// <summary>
/// Simple wrapper to manage a native structure 
/// or create it in the managed environment.
/// </summary>
public unsafe class NativeWrapper<TStructure> : NativeWrapperBase<TStructure> where TStructure : unmanaged
{
    /// <summary>
    /// Allocates a zeroed piece of memory just in size of the structure.
    /// </summary>
    public NativeWrapper() : this(sizeof(TStructure))
    { }

    /// <summary>
    /// Allocates a zeroed piece of memory in the given size.
    /// </summary>
    /// <remarks>
    /// Size must be equal or more then size of the structure.
    /// </remarks>
    /// <exception cref="ArgumentException">If size is wrong.</exception>
    public NativeWrapper(int size)
    {
        Initialize(size);
    }

    /// <summary>
    /// Sets or makes a copy of the structure by given pointer.
    /// </summary>
    /// <remarks>
    /// <br/> This is a pretty UNSAFE constructor. Think twice before using it.
    /// </remarks>
    /// <exception cref="ArgumentNullException">If pointer is null.</exception>
    public NativeWrapper(IntPtr pointer, bool makeCopy = true) : this(pointer, sizeof(TStructure), makeCopy)
    { }

    /// <summary>
    /// Sets or makes a copy of the structure by given pointer.
    /// </summary>
    /// <remarks>
    /// Size must be equal or more then size of the structure.
    /// <br/> This is a pretty UNSAFE constructor. Think twice before using it.
    /// </remarks>
    /// <exception cref="ArgumentNullException">If pointer is null.</exception>
    /// <exception cref="ArgumentException">If size is wrong.</exception>
    public NativeWrapper(IntPtr pointer, int size, bool makeCopy = true)
    {
        Initialize(pointer, size, makeCopy);
    }

    /// <summary>
    /// Creates a new wrapper based on the input native structure with zero offset and default size.
    /// </summary>
    /// <exception cref="ArgumentException">If size is wrong.</exception>
    /// <exception cref="ArgumentNullException">If the input structure is null.</exception>
    /// <exception cref="ObjectDisposedException">If the input structure is disposed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If new structure is outside of the basic structure.</exception>
    public NativeWrapper(INativeStructure basic, bool makeCopy = true) : this(basic, 0, sizeof(TStructure), makeCopy)
    { }

    /// <summary>
    /// Creates a new wrapper based on the input native structure with zero offset and desired size.
    /// </summary>
    /// <exception cref="ArgumentException">If size is wrong.</exception>
    /// <exception cref="ArgumentNullException">If the input structure is null.</exception>
    /// <exception cref="ObjectDisposedException">If the input structure is disposed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If new structure is outside of the basic structure.</exception>
    public NativeWrapper(INativeStructure basic, int size, bool makeCopy = true) : this(basic, 0, size, makeCopy)
    { }

    /// <summary>
    /// Creates a new wrapper based on the input native structure with desired offset and default size.
    /// </summary>
    /// <exception cref="ArgumentException">If size is wrong.</exception>
    /// <exception cref="ArgumentNullException">If the input structure is null.</exception>
    /// <exception cref="ObjectDisposedException">If the input structure is disposed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If new structure is outside of the basic structure.</exception>
    public NativeWrapper(INativeStructure basic, int offset, int size, bool makeCopy = true)
    {
        Initialize(basic, offset, size, makeCopy);
    }

    /// <summary>
    /// Dereferenced pointer passed by ref.
    /// </summary>
    /// <remarks>
    /// I don't really know how it works, 
    /// but it works without creation a copy of a structure on the stack.
    /// </remarks>
    /// <exception cref="ObjectDisposedException"/>
    public ref TStructure Value
    {
        get
        {
            ThrowIfDisposed();

            return ref InternalValue;
        }
    }
}