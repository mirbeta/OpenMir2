using SystemModule.NativeList.Abstracts;
using SystemModule.NativeList.Enums;

namespace SystemModule.NativeList.Utils;

public class IntComparer : ComparerBase<int>
{
    public override Equality Compare(int first, int second)
    {
        if (first == second)
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