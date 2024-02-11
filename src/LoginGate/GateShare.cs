using System;
using System.Collections.Generic;
using SystemModule;
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
        /// 网关游戏服务器之间检测超时时间长度
        /// </summary>
        public const int CheckServerTimeOutTime = 10 * 1000;
        /// <summary>
        /// 心跳响应超时时间
        /// </summary>
        public const int KeepAliveTickTimeOut = 30 * 1000;
        /// <summary>
        /// 禁止连接IP列表
        /// </summary>
        public static StringList BlockIPList = null;
        /// <summary>
        /// 临时禁止连接IP列表
        /// </summary>
        public static IList<string> TempBlockIPList = null;
        public static int nMaxConnOfIPaddr = 50;
        public static IServiceProvider ServiceProvider = null;

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

        private bool IsBlockIP(string sIPaddr)
        {
            bool result = false;
            string sBlockIPaddr;
            for (var i = 0; i < TempBlockIPList.Count; i++)
            {
                sBlockIPaddr = TempBlockIPList[i];
                if (string.Compare(sIPaddr, sBlockIPaddr, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = true;
                    break;
                }
            }
            for (var i = 0; i < BlockIPList.Count; i++)
            {
                sBlockIPaddr = BlockIPList[i];
                if (HUtil32.CompareLStr(sIPaddr, sBlockIPaddr))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool IsConnLimited(string sIPaddr)
        {
            bool result = false;
            int nCount = 0;
            //for (var i = 0; i < ServerSocket.Socket.ActiveConnections; i++)
            //{
            //    if ((sIPaddr).CompareTo((ServerSocket.Connections[i].RemoteAddress)) == 0)
            //    {
            //        nCount++;
            //    }
            //}
            if (nCount > nMaxConnOfIPaddr)
            {
                result = true;
            }
            return result;
        }
    }
}