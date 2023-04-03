namespace SystemModule.NativeList.Interfaces.Shared
{
    /// <summary>
    /// Special interface to mark that the object is already disposed.
    /// </summary>
    /// <note>
    /// Have no idea why in actually "System" library we have no such interface.
    /// </note>
    public interface IDisposeIndication
    {
        /// <summary>
        /// Show that the object is disposed or not.
        /// Some functionality may work after disposing but this is all on author mind.
        /// In most cases I do not recommend to touch disposed objects.
        /// </summary>
        bool IsDisposed { get; }
    }
}