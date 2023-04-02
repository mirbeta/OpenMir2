using SystemModule.NativeList.Utils;

namespace SystemModule.NativeList.Helpers;

/// <summary>
/// Helper class to make easier creation of <see cref="FlexibleRange{TItem}"/>
/// </summary>
public static class FlexibleRanges
{
    public static readonly IntComparer IntComparerInstance;
    public static readonly DoubleComparer DoubleComparerInstance;

    static FlexibleRanges()
    {
        IntComparerInstance = new IntComparer();
        DoubleComparerInstance = new DoubleComparer(ArgumentsGuard.ApproximationValue);
    }

    public static FlexibleRange<int> Create<TItem>(TItem[] array)
    {
        ArgumentsGuard.ThrowIfNull(array);

        return Create(0, true, array.Length, false);
    }

    public static FlexibleRange<int> Create(int left, bool isLeftStrictly, int right, bool isRightStrictly)
    {
        return new FlexibleRange<int>
        (
            left,
            isLeftStrictly,
            right,
            isRightStrictly,
            IntComparerInstance
        );
    }

    public static FlexibleRange<double> Create(double left, bool isLeftStrictly, double right, bool isRightStrictly)
    {
        return new FlexibleRange<double>
        (
            left,
            isLeftStrictly,
            right,
            isRightStrictly,
            DoubleComparerInstance
        );
    }
}