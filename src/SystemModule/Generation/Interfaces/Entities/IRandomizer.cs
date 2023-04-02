namespace SystemModule.Generation.Interfaces.Entities;

/// <summary>
/// Representation of randomizer in form of interface, 
/// to have an ability to flexible change it in different entities.
/// </summary>
public interface IRandomizer
{
    /// <summary>
    /// Seed with which <see cref="IRandomizer"/> instance was created.
    /// </summary>
    long Seed { get; }

    /// <summary>
    /// Returns new integer value.
    /// </summary>
    int NextInteger();

    /// <summary>
    /// Returns new double value in range from 0.0 to 1.0
    /// </summary>
    double NextNormalized();
}