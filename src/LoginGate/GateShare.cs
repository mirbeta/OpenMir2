using System.Collections.Generic;
using SystemModule.Common;

namespace LoginGate
{
    public class GateShare
    {
        /// <summary>
        /// 最大用户数
        /// </summary>
        public const int MaxSession = 10000;
        /// <summary>
        ///  网关游戏服务器之间检测超时时间长度
        /// </summary>
        public const int CheckServerTimeOutTime = 3 * 60 * 1000;
        /// <summary>
        /// 禁止连接IP列表
        /// </summary>
        public static StringList BlockIPList = null;
        /// <summary>
        /// 临时禁止连接IP列表
        /// </summary>
        public static IList<string> TempBlockIPList = null;
        public static int nMaxConnOfIPaddr = 50;

        public static void LoadBlockIPFile()
        {
            //AddMainLogMsg("正在加载IP过滤配置信息...", 4);
            //var sFileName = ".\\BlockIPList.txt";
            //if (File.Exists(sFileName))
            //{
            //    BlockIPList.LoadFromFile(sFileName);
            //}
            //AddMainLogMsg("IP过滤配置信息加载完成...", 4);
        }

        public static void Initialization()
        {
            BlockIPList = new StringList();
            TempBlockIPList = new List<string>();
        }
    }
}