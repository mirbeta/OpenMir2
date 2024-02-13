using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using OpenMir2.NativeList.Interfaces.Shared;

namespace OpenMir2.NativeList.Abstracts
{
    /// <summary>
    /// Basic class which includes implementation of <see cref="IDisposable"/> and <see cref="IDisposeIndication"/>
    /// </summary>
    public abstract class DisposableBase : IDisposable, IDisposeIndication
    {
        public const String ObjectDisposedError = "This object was disposed.";

        ~DisposableBase()
        {
            SafeDisposed(false);
        }

        /// <inheritdoc/>
        public virtual bool IsDisposed { get; private set; }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            SafeDisposed(true);

            IsDisposed = true;
        }

        /// <summary>
        /// Special private method that guaranties that dispose will not throw an exception in finalizer.
        /// </summary>
        /// <remarks>
        /// Do not change the method signature.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SafeDisposed(bool manual)
        {
            try
            {
                InternalDispose(manual);
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
                Trace.WriteLine(exception.StackTrace);
            }
        }

        /// <summary>
        /// Override this method to define disposing logic.
        /// </summary>
        protected virtual void InternalDispose(bool manual)
        { }

        /// <summary>
        /// Helper method that throws <see cref="ObjectDisposedException"/> if this object is disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        protected virtual void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(IDisposable), ObjectDisposedError);
            }
        }
    }
}