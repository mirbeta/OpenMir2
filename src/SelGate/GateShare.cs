using System;
using SelGate.Services;
using System.Collections.Generic;
using SystemModule;
using SystemModule.Common;

namespace SelGate
{
    public class GateShare
    {
        public static string GateAddr = "*";
        /// <summary>
        /// 角色网关端口
        /// </summary>
        public const int GatePort = 7100;
        /// <summary>
        ///  网关游戏服务器之间检测超时时间长度
        /// </summary>
        public const int CheckServerTimeOutTime = 10 * 1000;
        public static int CheckServerTick = 0;
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
        public const int SessionTimeOutTime = 15 * 24 * 60 * 60 * 1000;
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
        
        private bool IsBlockIP(string sIPaddr)
        {
            bool result = false;
            string sBlockIPaddr;
            for (var i = 0; i < GateShare.TempBlockIPList.Count; i++)
            {
                sBlockIPaddr = GateShare.TempBlockIPList[i];
                if (string.Compare(sIPaddr, sBlockIPaddr, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = true;
                    break;
                }
            }
            for (var i = 0; i < GateShare.BlockIPList.Count; i++)
            {
                sBlockIPaddr = GateShare.BlockIPList[i];
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
            if (nCount > GateShare.nMaxConnOfIPaddr)
            {
                result = true;
            }
            return result;
        }
    }
}