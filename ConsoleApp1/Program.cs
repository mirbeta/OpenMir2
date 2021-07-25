using System;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var sb = "(@s_repair)";
                sb+="%100";
                sb+="+30";
                sb+="+25";
                sb+="[@main]";
                sb+="你想做什么？\\";
                sb+="<查询声望点数/@talkwith>\\";
                sb+="<解除师徒关系/@unmaster>\\";
                sb+="<随机领取荣誉勋章/@talkwith2>\\";
                sb+="<指定领取荣誉勋章/@talkwith3>\\";
                sb+="<我要拜师/@teacher>\\";
                sb+="<修理勋章/@s_repair>\\";
                sb+="[@talkwith]";

            var matchCollection = Regex.Matches(sb, "<?@(\\w+?>)", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(3));
            foreach (Match match in matchCollection)
            {
                Console.WriteLine(match.Value.Remove(match.Value.Length - 1));
            }
        }
    }
}