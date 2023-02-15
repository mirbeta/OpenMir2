using System.Collections.Generic;

namespace SystemModule.NativeList.Enums
{
    /// <summary>
    /// Enum for comparer method to avoid non human friendly result of integer number.
    /// </summary>
    /// <remarks>
    /// Compatible with <see cref=" IComparer{T}"/> result.
    /// </remarks>
    public enum Equality
    {
        Less = -1,
        Equal = 0,
        Greater = 1
    }
}
