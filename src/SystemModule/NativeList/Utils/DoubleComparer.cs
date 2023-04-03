using System;
using SystemModule.NativeList.Abstracts;
using SystemModule.NativeList.Enums;

namespace SystemModule.NativeList.Utils;

public class DoubleComparer : ComparerBase<double>
{
    public DoubleComparer(double approximationValue)
    {
        ApproximationValue = approximationValue;
    }

    public double ApproximationValue { get; private set; }

    public override Equality Compare(double first, double second)
    {
        if (Math.Abs(first - second) < ApproximationValue)
        {
            return Equality.Equal;
        }
        else if (first > second)
        {
            return Equality.Greater;
        }
        else
        {
            return Equality.Less;
        }
    }
}