using System;
using System.Collections.Generic;
using System.IO;
using LoginGate.Services;
using SystemModule;
using SystemModule.Common;

namespace LoginGate
{
    public class GateShare
    {
        public static object CS_MainLog = null;
        public static IList<string> MainLogMsgList = null;
        public static string GateAddr = "*";
        /// <summary>
        /// 登录网关端口
        /// </summary>
        public static int GatePort = 7000;
        /// <summary>
        /// 显示B 或 KB
        /// </summary>
        public static bool boShowBite = true;
        /// <summary>
        ///  网关游戏服务器之间检测超时时间长度
        /// </summary>
        public static long dwCheckServerTimeOutTime = 3 * 60 * 1000;
        public static long dwCheckServerTick = 0;
        public static long dwCheckServerTimeMin = 0;
        public static long dwCheckServerTimeMax = 0;
        public static bool boDecodeMsgLock = false;
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
        public static long dwSessionTimeOutTime = 15 * 24 * 60 * 60 * 1000;
        public static IList<ClientThread> ServerGateList;

        public static void AddMainLogMsg(string Msg, int nLevel)
        {
            try
            {
                HUtil32.EnterCriticalSection(CS_MainLog);
                var tMsg = "[" + DateTime.Now + "] " + Msg;
                MainLogMsgList.Add(tMsg);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(CS_MainLog);
            }
        }

        public static void LoadBlockIPFile()
        {
            AddMainLogMsg("正在加载IP过滤配置信息...", 4);
            var sFileName = ".\\BlockIPList.txt";
            if (File.Exists(sFileName))
            {
                BlockIPList.LoadFromFile(sFileName);
            }
            AddMainLogMsg("IP过滤配置信息加载完成...", 4);
        }

        public static void Initialization()
        {
            CS_MainLog = new object();
            MainLogMsgList = new List<string>();
            BlockIPList = new StringList();
            TempBlockIPList = new List<string>();
            ServerGateList = new List<ClientThread>();
        }
    }
}