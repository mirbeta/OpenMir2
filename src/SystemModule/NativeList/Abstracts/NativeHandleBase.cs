using System;
using System.Runtime.CompilerServices;
using SystemModule.NativeList.Helpers;
using SystemModule.NativeList.Interfaces.Entities;

namespace SystemModule.NativeList.Abstracts;

/// <summary>
/// Special disposable object to handle native structures in unmanaged memory.
/// </summary>
public abstract class NativeHandleBase : DisposableBase, INativeHandle
{
    private const String AlreadyHasHandleError = "A handle is already initialized.";

    /// <inheritdoc/>
    public bool IsHandleOwner { get; private set; }

    /// <inheritdoc/>
    public IntPtr UnsafeHandle { get; private set; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ThrowIfInitialized()
    {
        if (UnsafeHandle != IntPtr.Zero)
            throw new Exception(AlreadyHasHandleError);
    }

    /// <summary>
    /// Sets handle internally.
    /// Can be invoked only once.
    /// Sets <see cref="IsHandleOwner"/> in false state.
    /// </summary>
    /// <exception cref="Exception">If was initialized before.</exception>
    /// <exception cref="ObjectDisposedException"/>
    protected void SetHandle(IntPtr handle)
    {
        ThrowIfDisposed();
        ThrowIfInitialized();

        if (UnsafeHandle != IntPtr.Zero)
            throw new Exception(AlreadyHasHandleError);

        NativeGuard.ThrowIfNull(handle);

        UnsafeHandle = handle;

        IsHandleOwner = false;
    }

    /// <summary>
    /// Creates handle internally.
    /// Can be invoked only once.
    /// Sets <see cref="IsHandleOwner"/> in true state.
    /// </summary>
    /// <exception cref="Exception">If was initialized before.</exception>
    /// <exception cref="Exception">If <see cref="CreateHandleInternal"/> returns <see cref="IntPtr.Zero"/></exception>
    /// <exception cref="ObjectDisposedException"/>
    protected void CreateHandle()
    {
        ThrowIfDisposed();
        ThrowIfInitialized();

        nint handle = CreateHandleInternal();

        if (handle == IntPtr.Zero)
            throw new Exception($"{nameof(CreateHandleInternal)} returns an unexpected result.");

        UnsafeHandle = handle;

        IsHandleOwner = true;
    }

    /// <summary>
    /// Has to create a new handle. 
    /// </summary>
    protected abstract IntPtr CreateHandleInternal();

    /// <summary>
    /// Has to frees created handle.
    /// Algorithm guarantees that the handle is not <see cref="IntPtr.Zero"/>
    /// </summary>
    protected abstract void FreeHandleInternal(IntPtr handle);

    protected override void InternalDispose(bool manual)
    {
        if (UnsafeHandle != IntPtr.Zero || !IsHandleOwner)
            FreeHandleInternal(UnsafeHandle);

        UnsafeHandle = IntPtr.Zero;

        base.InternalDispose(manual);
    }
}