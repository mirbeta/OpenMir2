using System;
using SystemModule.NativeList.Helpers;
using SystemModule.NativeList.Interfaces.Entities;

namespace SystemModule.NativeList.Abstracts;

/// <summary>
/// Native structure that has additional initializer which allows to initialize it by existed <see cref="INativeStructure"/> 
/// </summary>
public abstract class OwnedStructureBase : NativeStructureBase
{
    private INativeStructure _basicStructure;

    /// <inheritdoc/>
    /// <remarks>
    /// If structure is created from other <see cref="INativeStructure"/> without coping 
    /// then <see cref="IsDisposed"/> will be connected to the basic <see cref="INativeStructure"/>
    /// </remarks>
    public override bool IsDisposed
    {
        get
        {
            if (_basicStructure == null)
                return base.IsDisposed;
            else
                return base.IsDisposed || _basicStructure.IsDisposed;
        }
    }

    /// <summary>
    /// Sets/Copies a piece of memory based on the input native structure with desired offset and size.
    /// </summary>
    /// <exception cref="ArgumentException">If size is wrong.</exception>
    /// <exception cref="ArgumentNullException">If the input structure is null.</exception>
    /// <exception cref="ObjectDisposedException">If the input structure is disposed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If new structure is outside of the basic structure.</exception>
    protected virtual void Initialize(INativeStructure basic, int offset, int size, bool makeCopy = true)
    {
        ArgumentsGuard.ThrowIfNull(basic);
        ArgumentsGuard.ThrowIfDisposed(basic);
        ArgumentsGuard.ThrowIfLessZero(size);

        nint targetPointer = basic.UnsafeHandle + offset;

        NativeGuard.ThrowIfPointerIsOutOfRange(basic.UnsafeHandle, basic.Size, targetPointer, size, "computed pointer");

        Initialize(targetPointer, size, makeCopy);

        if (!makeCopy)
            _basicStructure = basic;
    }

    protected override void InternalDispose(bool manual)
    {
        _basicStructure = null;

        base.InternalDispose(manual);
    }
}