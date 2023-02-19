namespace SystemModule.Generation.Interfaces.Entities
{
    /// <summary>
    /// General interface for all three dimensions generators.
    /// </summary>
    /// <typeparam name="TItem">Type of generated item.</typeparam>
    public interface IGenerator3D<TItem>
    {
        /// <summary>
        /// Generates new set of data.
        /// </summary>
        TItem[,,] GenerateNext(int width, int height, int depth);
    }
}
