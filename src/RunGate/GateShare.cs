using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SystemModule;
using SystemModule.Common;

namespace RunGate
{
    public class GateShare
    {
        public static object CS_MainLog = null;
        public static object CS_FilterMsg = null;
        public static ArrayList MainLogMsgList = null;
        public static int nShowLogLevel = 3;
        public static string GateClass = "GameGate";
        public static string GateName = "游戏网关";
        public static string TitleName = "SKY引擎";
        public static string ServerAddr = "127.0.0.1";
        public static int ServerPort = 5000;
        public static string GateAddr = "0.0.0.0";
        public static int GatePort = 7200;
        public static bool boStarted = false;
        public static bool boClose = false;
        /// <summary>
        /// 显示B 或 KB
        /// </summary>
        public static bool boShowBite = true;
        public static bool boServiceStart = false;
        /// <summary>
        /// 网关是否就绪
        /// </summary>
        public static bool boGateReady = false;
        /// <summary>
        ///  网关 <->游戏服务器之间检测是否失败（超时）
        /// </summary>
        public static bool boCheckServerFail = false;
        /// <summary>
        ///  网关游戏服务器之间检测超时时间长度
        /// </summary>
        public static long dwCheckServerTimeOutTime = 3 * 60 * 1000;
        public static IList<string> AbuseList = null;
        public static TSessionInfo[] SessionArray = new TSessionInfo[GATEMAXSESSION - 1 + 1];
        /// <summary>
        /// 连接会话数
        /// </summary>
        public static int SessionCount = 0;
        /// <summary>
        /// 是否显示SOCKET接收的信息
        /// </summary>
        public static bool boShowSckData = false;
        public static string sReplaceWord = "*";
        public static IList<TSendUserData> ReviceMsgList = null;
        public static IList<TSendUserData> SendMsgList = null;
        public static int nCurrConnCount = 0;
        public static bool boSendHoldTimeOut = false;
        public static long dwSendHoldTick = 0;
        public static int n45AA80 = 0;
        public static int n45AA84 = 0;
        public static long dwCheckRecviceTick = 0;
        public static long dwCheckRecviceMin = 0;
        public static long dwCheckRecviceMax = 0;
        public static long dwCheckServerTick = 0;
        public static long dwCheckServerTimeMin = 0;
        public static long dwCheckServerTimeMax = 0;
        public static byte[] SocketBuffer = null;
        public static int nBuffLen = 0;
        public static ArrayList List_45AA58 = null;
        public static bool boDecodeMsgLock = false;
        public static long dwProcessReviceMsgTimeLimit = 0;
        public static long dwProcessSendMsgTimeLimit = 0;
        /// <summary>
        /// 禁止连接IP列表
        /// </summary>
        public static List<string> BlockIPList = null;
        /// <summary>
        /// 临时禁止连接IP列表
        /// </summary>
        public static List<string> TempBlockIPList = null;
        public static int nMaxConnOfIPaddr = 50;
        public static int nMaxClientPacketSize = 7000;
        public static int nNomClientPacketSize = 150;
        public static long dwClientCheckTimeOut = 50;
        public static int nMaxOverNomSizeCount = 2;
        public static int nMaxClientMsgCount = 15;
        public static TBlockIPMethod BlockMethod = TBlockIPMethod.mDisconnect;
        public static bool bokickOverPacketSize = true;
        /// <summary>
        /// 发送给客户端数据包大小限制
        /// </summary>
        public static int nClientSendBlockSize = 1000;
        /// <summary>
        /// 客户端连接会话超时(指定时间内未有数据传输)
        /// </summary>
        public static long dwClientTimeOutTime = 5000;
        public static IniFile Conf = null;
        public static string sConfigFileName = ".\\Config.ini";
        /// <summary>
        /// 发言字符长度
        /// </summary>
        public static int nSayMsgMaxLen = 70;
        /// <summary>
        /// 发言间隔时间
        /// </summary>
        public static long dwSayMsgTime = 1000;
        /// <summary>
        /// 攻击间隔时间
        /// </summary>
        public static long dwHitTime = 300;
        public static long dwSessionTimeOutTime = 60 * 60 * 1000;
        public const int GATEMAXSESSION = 1000;
        public const int MSGMAXLENGTH = 20000;
        public const int SENDCHECKSIZE = 512;
        public const int SENDCHECKSIZEMAX = 2048;

        public static void AddMainLogMsg(string Msg, int nLevel)
        {
            string tMsg;
            try
            {
                HUtil32.EnterCriticalSection(CS_MainLog);
                if (nLevel <= nShowLogLevel)
                {
                    tMsg = "[" + DateTime.Now.ToString() + "] " + Msg;
                    MainLogMsgList.Add(tMsg);
                }
            }
            finally
            {
              HUtil32.LeaveCriticalSection(CS_MainLog);
            }
        }

        public static void LoadAbuseFile()
        {
            string sFileName;
            AddMainLogMsg("正在加载文字过滤配置信息...", 4);
            sFileName = ".\\WordFilter.txt";
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
            string sFileName;
            AddMainLogMsg("正在加载IP过滤配置信息...", 4);
            sFileName = ".\\BlockIPList.txt";
            if (File.Exists(sFileName))
            {
                //BlockIPList.LoadFromFile(sFileName);
            }
            AddMainLogMsg("IP过滤配置信息加载完成...", 4);
        }

        public static void Initialization()
        {
            Conf = new IniFile(sConfigFileName);
            Conf.Load();
            nShowLogLevel = Conf.ReadInteger(GateClass, "ShowLogLevel", nShowLogLevel);
            CS_MainLog = new object();
            CS_FilterMsg = new object();
            MainLogMsgList = new ArrayList();
            AbuseList = new List<string>();
            ReviceMsgList = new List<TSendUserData>();
            SendMsgList = new List<TSendUserData>();
            List_45AA58 = new ArrayList();
            boShowSckData = false;
            BlockIPList = new List<string>();
            TempBlockIPList = new List<string>();
        }
    } 
}