namespace OpenMir2.NativeList.Interfaces.Entities
{
    /// <summary>
    /// Wraps the native structure. Extends the <see cref="INativeHandle"/> interface.
    /// </summary>
    public interface INativeStructure : INativeHandle
    {
        /// <summary>
        /// Shows the real size of the wrapped structure, which can be less than your expectation.
        /// </summary>
        /// <remarks>
        /// Structure size is in bytes.
        /// </remarks>
        int Size { get; }
    }
}