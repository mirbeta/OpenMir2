using OpenMir2.NativeList.Abstracts;
using OpenMir2.NativeList.Enums;

namespace OpenMir2.NativeList.Utils
{
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
}