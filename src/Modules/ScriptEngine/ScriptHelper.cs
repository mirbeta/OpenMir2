using System.Text.RegularExpressions;

namespace ScriptEngine
{
    public partial class ScriptHelper
    {
        [GeneratedRegex("#CALL", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.RightToLeft, "zh-CN")]
        private static partial Regex RegexCallCount();

        public static int GetScriptCallCount(string sText)
        {
            return RegexCallCount().Count(sText);
        }
    }
}