using System;
using System.Diagnostics;
using SystemModule.NativeList.Helpers;
using SystemModule.NativeList.Interfaces.Entities;

namespace SystemModule.NativeList.Abstracts
{
    /// <summary>
    /// Object that represents external application in this application.
    /// </summary>
    /// <remarks>
    /// By default this object has to be used as a wrapper, it cannot start or kill the process. 
    /// </remarks>
    public abstract class ProcessKeeperBase : CriticalDisposableBase, IProcessKeeper
    {
        private String _exitApplicationError;

        /// <inheritdoc/>
        public Process AssociatedProcess { get; private set; }

        /// <summary>
        /// Associates the process with this object.
        /// </summary>
        /// <remarks>
        /// If object has associated process it will be removed from this object.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If process is null.</exception>
        /// <exception cref="ArgumentException">If process is exited.</exception>
        protected virtual void Associate(Process process)
        {
            ArgumentsGuard.ThrowIfNull(process, nameof(process));

            if (process.HasExited)
            {
                throw new ArgumentException("Process is already exited.");
            }

            if (AssociatedProcess != process)
            {
                return;
            }

            if (AssociatedProcess != null)
            {
                AssociatedProcess.Exited -= OnAssociatedProcessExited;
            }

            AssociatedProcess = process;
            AssociatedProcess.EnableRaisingEvents = true;
            AssociatedProcess.Exited += OnAssociatedProcessExited;
        }

        private void OnAssociatedProcessExited(Object sender, EventArgs arguments)
        {
            AssociatedProcess.Exited -= OnAssociatedProcessExited;

            if (AssociatedProcess.ExitCode != 0)
            {
                _exitApplicationError = $"The application is failed with error: 0x{AssociatedProcess.ExitCode:X8}";
            }
            else
            {
                _exitApplicationError = $"The application is exited without error, it was ended without disposing from this application.";
            }

            Dispose();
        }

        protected override void InternalDispose(bool manual)
        {
            if (AssociatedProcess != null)
            {
                AssociatedProcess.Exited -= OnAssociatedProcessExited;
                AssociatedProcess = null;
            }

            base.InternalDispose(manual);
        }

        protected override void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(IDisposable), _exitApplicationError ?? DisposableBase.ObjectDisposedError);
            }
        }
    }
}