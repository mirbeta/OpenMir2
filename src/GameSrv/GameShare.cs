using EventLogSystem;
using GameSrv.DataSource;
using GameSrv.Maps;
using GameSrv.Network;
using GameSrv.Robots;
using GameSrv.Services;
using GameSrv.Word.Threads;
using M2Server;
using MarketSystem;
using PlanesSystem;
using ScriptSystem;
using System.Text.RegularExpressions;
using SystemModule.Data;

namespace GameSrv
{
    public static class GameShare
    {
        /// <summary>
        /// 启动路径
        /// </summary>
        public static readonly string BasePath;
        /// <summary>
        /// 服务器编号
        /// </summary>
        public static byte ServerIndex = 0;
        /// <summary>
        /// 服务器启动时间
        /// </summary>
        public static long StartTime;
        public static readonly WordStatistics Statistics;
        public static readonly LocalDb LocalDb;
        public static readonly CommonDB CommonDb;
        public static readonly RandomNumber RandomNumber;
        public static MapQuestManager QuestManager;
        public static DBService DataServer = null;
        public static IMarketService MarketService = null;
        public static IChatService ChatChannel = null;
        public static ThreadSocketMgr SocketMgr = null;
        public static GameEventSource EventSource;
        public static ScriptEngine ScriptEngine = null;
        public static RobotManage RobotMgr = null;
        public static IPlanesService PlanesService;
        public static NetworkMonitor NetworkMonitor;
        public static SystemProcessor SystemProcess;
        public static UserProcessor UserProcessor;
        public static RobotProcessor RobotProcessor;
        public static MerchantProcessor MerchantProcessor;
        public static GeneratorProcessor GeneratorProcessor;
        public static EventProcessor EventProcessor;
        public static StorageProcessor StorageProcessor;
        public static TimedRobotProcessor TimedRobotProcessor;
        public static bool StartReady = false;
        public static int SendOnlineTick = 0;
        public static readonly HashSet<byte> IsAccessoryMap = new HashSet<byte> { 19, 20, 21, 22, 23, 24, 26 };

        static GameShare()
        {
            BasePath = AppContext.BaseDirectory;
            RandomNumber = RandomNumber.GetInstance();
            ScriptEngine = new ScriptEngine();
            Statistics = new WordStatistics();
            LocalDb = new LocalDb();
            CommonDb = new CommonDB();
            NetworkMonitor = new NetworkMonitor();
            SystemProcess = new SystemProcessor();
            UserProcessor = new UserProcessor();
            RobotProcessor = new RobotProcessor();
            MerchantProcessor = new MerchantProcessor();
            GeneratorProcessor = new GeneratorProcessor();
            EventProcessor = new EventProcessor();
            StorageProcessor = new StorageProcessor();
            TimedRobotProcessor = new TimedRobotProcessor();
            PlanesService=new PlanesService();
            StartTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 随机获取其他服务器
        /// </summary>
        /// <returns></returns>
        public static bool GetMultiServerAddrPort(byte serverIndex, ref string sIPaddr, ref int nPort)
        {
            var result = false;
            for (var i = 0; i < M2Share.ServerTableList.Length; i++)
            {
                var routeInfo = M2Share.ServerTableList[i];
                if (routeInfo == null)
                {
                    continue;
                }
                if (routeInfo.GateCount <= 0)
                {
                    continue;
                }
                if (routeInfo.ServerIdx == serverIndex)
                {
                    sIPaddr = GetRandpmRoute(routeInfo, ref nPort);
                    result = true;
                    break;
                }
            }
            return result;
        }

        private static readonly Regex ScriptRegex = new Regex("(?<=(<))[.\\s\\S]*?(?=(>))", RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.RightToLeft);

        public static MatchCollection MatchScriptLabel(string script)
        {
            return ScriptRegex.Matches(script);
        }

        private static string GetRandpmRoute(TRouteInfo routeInfo, ref int gatePort)
        {
            var random = RandomNumber.Random(routeInfo.GateCount);
            gatePort = routeInfo.GameGatePort[random];
            return routeInfo.GameGateIP[random];
        }

        public static int GetExVersionNO(int nVersionDate, ref int nOldVerstionDate)
        {
            var result = 0;
            if (nVersionDate > 10000000)
            {
                while (nVersionDate > 10000000)
                {
                    nVersionDate -= 10000;
                    result += 100000000;
                }
            }
            nOldVerstionDate = nVersionDate;
            return result;
        }

        public static void LoadConfig()
        {
            SystemShare.ServerConf.LoadConfig();
            SystemShare.StringConf.LoadString();
            SystemShare.ExpConf.LoadConfig();
            SystemShare.GlobalConf.LoadConfig();
            SystemShare.GameSetting.LoadConfig();
        }

        public static string GetEnvirFilePath(string filePath)
        {
            if (filePath.StartsWith(".."))
            {
                return Path.Combine(BasePath, SystemShare.Config.EnvirDir, filePath[3..]);
            }
            return Path.Combine(BasePath, SystemShare.Config.EnvirDir, filePath);
        }

        public static string GetEnvirFilePath(string dirPath, string filePath)
        {
            if (filePath.StartsWith(".."))
            {
                return Path.Combine(BasePath, SystemShare.Config.EnvirDir, filePath[3..]);
            }
            return Path.Combine(BasePath, SystemShare.Config.EnvirDir, dirPath, filePath);
        }

        public static string GetNoticeFilePath(string filePath)
        {
            if (filePath.StartsWith(".."))
            {
                return Path.Combine(BasePath, SystemShare.Config.EnvirDir, filePath[3..]);
            }
            return Path.Combine(BasePath, SystemShare.Config.NoticeDir, filePath);
        }
    }
}