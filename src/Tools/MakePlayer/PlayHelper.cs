using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakePlayer
{
    public static class PlayHelper
    {
        public static readonly IList<string> SayMsgList = new List<string>(100);

        public static void LoadSayListFile()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "SayMessage.txt");
            if (File.Exists(filePath))
            {
                string line;
                var sr = new StreamReader(filePath, System.Text.Encoding.ASCII);
                while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                {
                    SayMsgList.Add(line);
                }
            }
            else
            {
                Console.WriteLine("自动发言列表文件不存在.");
            }
        }
    }
}
