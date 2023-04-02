using System;
using System.Runtime.CompilerServices;
using SystemModule.Generation.Interfaces.Entities;

namespace SystemModule.Generation.Entities;

/// <summary>
/// Randomizer that was created under influence of OpenJDK Randomizer.
/// </summary>
public class OpenJDKRandomizer : IRandomizer
{
    private static readonly long _multiplier = 0x5DEECE66DL;
    private static readonly long _addend = 0xBL;
    private static readonly long _mask = (1L << 48) - 1;

    private static long _seedUniquifier = 8682522807148012L;

    private readonly long _initialSeed;
    private long _currentSeed;

    public OpenJDKRandomizer() : this(SeedUniquifier() * DateTime.Now.Ticks * 100)
    { }

    public OpenJDKRandomizer(long seed)
    {
        _initialSeed = seed;
        _currentSeed = seed;
    }

    /// <inheritdoc/>
    public long Seed => _initialSeed;

    /// <inheritdoc/>
    public int NextInteger()
    {
        return InternalNextInteger(32);
    }

    /// <inheritdoc/>
    public double NextNormalized()
    {
        return (((long)(InternalNextInteger(26)) << 27) + InternalNextInteger(27)) / (double)(1L << 53);
    }

    /// <summary>
    /// Returns next random integer number in range that based on input bits.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int InternalNextInteger(int bits)
    {
        long nextseed;

        do
        {
            nextseed = (_currentSeed * _multiplier + _addend) & _mask;
        } while (!CompareAndSet(ref _currentSeed, nextseed));

        return (int)(nextseed >> (48 - bits));
    }

    /// <summary>
    /// Returns true if new value and current value are different.
    /// </summary>
    private static bool CompareAndSet(ref long currentValue, long newValue)
    {
        if (currentValue == newValue)
            return false;

        currentValue = newValue;

        return true;
    }

    /// <summary>
    /// Helps to make seed more unique.
    /// </summary>
    private static long SeedUniquifier()
    {
        for (; ; )
        {
            long next = _seedUniquifier * 181783497276652981L;

            if (CompareAndSet(ref _seedUniquifier, next))
                return next;
        }
    }
}