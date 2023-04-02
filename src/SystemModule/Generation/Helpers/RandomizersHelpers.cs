using System;
using SystemModule.Generation.Interfaces.Entities;
using SystemModule.NativeList.Helpers;

namespace SystemModule.Generation.Helpers;

public static class RandomizersHelpers
{
    /// <summary>
    /// Returns random integer number is limited by the input maxValue parameter.
    /// </summary>
    /// <exception cref="ArgumentNullException">If the randomizer is null.</exception>
    public static int NextInteger(this IRandomizer randomizer, int maxValue)
    {
        ArgumentsGuard.ThrowIfNull(randomizer);

        return (int)(randomizer.NextNormalized() * maxValue);
    }

    /// <summary>
    /// Returns random integer number is in range based on input parameters.
    /// </summary>
    /// <exception cref="ArgumentNullException">If the randomizer is null.</exception>
    /// <exception cref="ArgumentException">If min value is more or equal to max value.</exception>
    public static int NextInteger(this IRandomizer randomizer, int minValue, int maxValue)
    {
        ArgumentsGuard.ThrowIfNull(randomizer);

        if (minValue >= maxValue)
            throw new ArgumentException("Min value cannot be more or equal to max value.", nameof(minValue));

        int range = maxValue - minValue;

        return (int)(randomizer.NextNormalized() * range + minValue);
    }
}