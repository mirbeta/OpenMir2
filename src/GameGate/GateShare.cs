using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using SystemModule;
using SystemModule.Common;

namespace GameGate
{
    public class GateShare
    {
        public static object CS_MainLog = null;
        public static object CS_FilterMsg = null;
        public static IList<string> MainLogMsgList = null;
        public static int nShowLogLevel = 0;
        public static string GateClass = "GameGate";
        public static string GateAddr = "*";
        public static int GatePort = 7200;
        /// <summary>
        /// 显示B 或 KB
        /// </summary>
        public static bool boShowBite = true;
        /// <summary>
        ///  网关游戏服务器之间检测超时时间长度
        /// </summary>
        public static long dwCheckServerTimeOutTime = 3 * 60 * 1000;
        public static IList<string> AbuseList = null;
        public static string sReplaceWord = "*";
        public static long dwSendHoldTick = 0;
        public static long dwCheckRecviceTick = 0;
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
        public static ConcurrentDictionary<string, byte> g_ChatCmdFilterList;
        public static Dictionary<string, ClientSession> PunishList;
        public static HWIDFilter _HwidFilter;

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

        public static void LoadAbuseFile()
        {
            AddMainLogMsg("正在加载文字过滤配置信息...", 4);
            var sFileName = ".\\WordFilter.txt";
            if (File.Exists(sFileName))
            {
                try
                {
                    HUtil32.EnterCriticalSection(CS_FilterMsg);
                    //AbuseList.LoadFromFile(sFileName);
                }
                finally
                {
                    HUtil32.LeaveCriticalSection(CS_FilterMsg);
                }
            }
            AddMainLogMsg("文字过滤信息加载完成...", 4);
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
            CS_FilterMsg = new object();
            MainLogMsgList = new List<string>();
            AbuseList = new List<string>();
            BlockIPList = new StringList();
            TempBlockIPList = new List<string>();
            ServerGateList = new List<ClientThread>();
            PunishList = new Dictionary<string, ClientSession>();
            g_ChatCmdFilterList = new ConcurrentDictionary<string, byte>(StringComparer.OrdinalIgnoreCase);
        }
    }

    public class THardwareHeader : Packets
    {
        public uint dwMagicCode;
        public byte[] xMd5Digest;

        public THardwareHeader(byte[] buffer)
        {
            var binaryReader = new BinaryReader(new MemoryStream(buffer));
            dwMagicCode = binaryReader.ReadUInt32();
            xMd5Digest = binaryReader.ReadBytes(16);
        }
    }
}