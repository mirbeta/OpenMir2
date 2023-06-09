using System.Text.RegularExpressions;

namespace ScriptSystem{
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