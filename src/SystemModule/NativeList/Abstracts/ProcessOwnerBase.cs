using System;
using System.Diagnostics;

namespace SystemModule.NativeList.Abstracts
{
    /// <summary>
    /// Improved version of <see cref="ProcessKeeperBase"/>
    /// Will kill process while disposing.
    /// </summary>
    public abstract class ProcessOwnerBase : ProcessKeeperBase
    {
        protected override void InternalDispose(bool manual)
        {
            if(AssociatedProcess != null && !AssociatedProcess.HasExited)
            {
                try
                {
                    AssociatedProcess.EnableRaisingEvents = false;

                    KillProcess();
                }
                catch (Exception exception)
                {
                    Trace.WriteLine(exception.Message);
                    Trace.WriteLine(exception.StackTrace);
                }
            }

            base.InternalDispose(manual);
        }

        protected virtual void KillProcess()
        {
            AssociatedProcess.Kill();
        }
    }
}
