using SelGate.Services;
using System.Collections.Generic;
using SystemModule.Common;

namespace SelGate
{
    public class GateShare
    {
        public static string GateAddr = "*";
        /// <summary>
        /// 角色网关端口
        /// </summary>
        public static int GatePort = 7100;
        /// <summary>
        ///  网关游戏服务器之间检测超时时间长度
        /// </summary>
        public static int dwCheckServerTimeOutTime = 3 * 60 * 1000;
        public static int dwCheckServerTick = 0;
        /// <summary>
        /// 禁止连接IP列表
        /// </summary>
        public static StringList BlockIPList = null;
        /// <summary>
        /// 临时禁止连接IP列表
        /// </summary>
        public static IList<string> TempBlockIPList = null;
        public static int nMaxConnOfIPaddr = 50;
        /// <summary>
        /// 会话超时时间
        /// </summary>
        public static int dwSessionTimeOutTime = 15 * 24 * 60 * 60 * 1000;
        public static IList<ClientThread> ServerGateList;

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
            ServerGateList = new List<ClientThread>();
        }
    }
}