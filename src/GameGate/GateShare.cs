using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Channels;
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
        public static bool boServiceStart = false;
        /// <summary>
        ///  网关游戏服务器之间检测超时时间长度
        /// </summary>
        public static long dwCheckServerTimeOutTime = 3 * 60 * 1000;
        public static IList<string> AbuseList = null;
        /// <summary>
        /// 是否显示SOCKET接收的信息
        /// </summary>
        public static bool boShowSckData = true;
        public static string sReplaceWord = "*";
        /// <summary>
        /// 转发封包（数据引擎-》网关）
        /// </summary>
        public static Channel<ForwardMessage> ForwardMsgList = null;
        public static int nCurrConnCount = 0;
        public static bool boSendHoldTimeOut = false;
        public static long dwSendHoldTick = 0;
        public static long dwCheckRecviceTick = 0;
        public static long dwCheckRecviceMin = 0;
        public static long dwCheckRecviceMax = 0;
        public static long dwCheckServerTick = 0;
        public static long dwCheckServerTimeMin = 0;
        public static long dwCheckServerTimeMax = 0;
        /// <summary>
        /// 累计接受数据大小
        /// </summary>
        public static int NReviceMsgSize;
        public static bool boDecodeMsgLock = false;
        public static long dwProcessReviceMsgTimeLimit = 0;
        public static long dwProcessSendMsgTimeLimit = 0;
        /// <summary>
        /// 禁止连接IP列表
        /// </summary>
        public static StringList BlockIPList = null;
        /// <summary>
        /// 临时禁止连接IP列表
        /// </summary>
        public static IList<string> TempBlockIPList = null;
        public static int nMaxConnOfIPaddr = 50;
        public static int nMaxClientPacketSize = 7000;
        public static int nNomClientPacketSize = 150;
        public static int dwClientCheckTimeOut = 50;
        public static int nMaxOverNomSizeCount = 2;
        public static int nMaxClientMsgCount = 15;
        public static bool bokickOverPacketSize = true;
        /// <summary>
        /// 发送给客户端数据包大小限制
        /// </summary>
        public static int nClientSendBlockSize = 1000;
        /// <summary>
        /// 客户端连接会话超时(指定时间内未有数据传输)
        /// </summary>
        public static long dwClientTimeOutTime = 5000;
        /// <summary>
        /// 会话超时时间
        /// </summary>
        public static long dwSessionTimeOutTime = 15 * 24 * 60 * 60 * 1000;
        public static IList<ClientThread> ServerGateList;
        public static Dictionary<string, ClientSession> PunishList;

        public static void AddMainLogMsg(string Msg, int nLevel)
        {
            string tMsg;
            try
            {
                HUtil32.EnterCriticalSection(CS_MainLog);
                tMsg = "[" + DateTime.Now + "] " + Msg;
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
            boShowSckData = false;
            BlockIPList = new StringList();
            TempBlockIPList = new List<string>();
            ServerGateList = new List<ClientThread>();
            PunishList = new Dictionary<string, ClientSession>();
            ForwardMsgList = Channel.CreateUnbounded<ForwardMessage>();
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