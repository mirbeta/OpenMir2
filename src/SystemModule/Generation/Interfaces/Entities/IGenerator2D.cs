namespace SystemModule.Generation.Interfaces.Entities;

/// <summary>
/// General interface for all two dimensions generators.
/// </summary>
/// <typeparam name="TItem">Type of generated item.</typeparam>
public interface IGenerator2D<TItem>
{
    /// <summary>
    /// Generates new set of data.
    /// </summary>
    TItem[,] GenerateNext(int width, int height);
}