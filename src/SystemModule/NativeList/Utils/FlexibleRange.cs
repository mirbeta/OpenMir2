using System;
using System.Text;
using SystemModule.NativeList.Abstracts;
using SystemModule.NativeList.Enums;
using SystemModule.NativeList.Helpers;

namespace SystemModule.NativeList.Utils;

/// <summary>
/// A range of values with ability to set any type that you want.
/// </summary>
public struct FlexibleRange<TItem>
{
    public FlexibleRange(TItem left, bool isLeftStrictly, TItem right, bool isRightStrictly, ComparerBase<TItem> comparer)
    {
        ArgumentsGuard.ThrowIfNull(comparer);

        Left = left;
        Right = right;

        IsLeftStrictly = isLeftStrictly;
        IsRightStrictly = isRightStrictly;

        Comparer = comparer;

        if (Comparer.Compare(Left, Right) == Equality.Equal)
            throw new Exception("Cannot create range when left and right sides are equal.");
    }

    /// <summary>
    /// Minimal value of the range.
    /// </summary>
    public TItem Left { get; private set; }

    /// <summary>
    /// This means that the <see cref="Left"/> value is included in the range.
    /// </summary>
    public bool IsLeftStrictly { get; private set; }

    /// <summary>
    /// Maximum value of the range.
    /// </summary>
    public TItem Right { get; private set; }

    /// <summary>
    /// This means that the <see cref="Right"/> value is included in the range.
    /// </summary>
    public bool IsRightStrictly { get; private set; }

    /// <summary>
    /// Special instance to make a comparing process possible 
    /// </summary>
    public ComparerBase<TItem> Comparer { get; private set; }

    /// <summary>
    /// Returns true if the input value is in range.
    /// </summary>
    /// <remarks>
    /// Contains a lot of calls to the <see cref="Comparer"/>. Make sure that the <see cref="Comparer"/> can handle it quickly.
    /// </remarks>
    public bool IsInRange(TItem item)
    {
        Equality sidesEquality = Comparer.Compare(Left, Right);
        Equality leftEquality = Comparer.Compare(item, Left);
        Equality rightEquality = Comparer.Compare(item, Right);

        bool rightResult;
        bool leftResult;

        if (sidesEquality == Equality.Less)
        {
            leftResult = leftEquality == Equality.Greater;
            rightResult = rightEquality == Equality.Less;
        }
        else
        {
            leftResult = leftEquality == Equality.Less;
            rightResult = rightEquality == Equality.Greater;
        }

        if (IsLeftStrictly)
            leftResult = leftResult || leftEquality == Equality.Equal;

        if (IsRightStrictly)
            rightResult = rightResult || rightEquality == Equality.Equal;

        return leftResult && rightResult;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        if (IsLeftStrictly)
            builder.Append('[');
        else
            builder.Append('(');

        builder.Append(Left);
        builder.Append("..");
        builder.Append(Right);

        if (IsRightStrictly)
            builder.Append(']');
        else
            builder.Append(')');

        return builder.ToString();
    }
}