using System;
using System.Text;

namespace SystemModule.Extensions
{
    public static class SpanExtension
    {
        public static string ToString(this ReadOnlySpan<byte> span, Encoding encoding)
        {
            return encoding.GetString(span);
        }
    }
}