using System;
using System.Text;

namespace OpenMir2.Extensions
{
    public static class SpanExtension
    {
        public static string ToString(this ReadOnlySpan<byte> span, Encoding encoding)
        {
            return encoding.GetString(span);
        }
    }
}