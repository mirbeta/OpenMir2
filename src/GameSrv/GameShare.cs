using GameSrv.DB;
using GameSrv.Maps;
using GameSrv.Module;
using GameSrv.Services;
using GameSrv.Word.Threads;
using PlanesSystem;

namespace GameSrv
{
    public static class GameShare
    {
        /// <summary>
        /// 服务器启动时间
        /// </summary>
        public static readonly long StartTime;
        public static readonly WordStatistics Statistics;
        public static readonly LocalDb LocalDb;
        public static readonly IDataSource DataSource;
        public static readonly IPlanesService PlanesService;
        public static readonly NetworkMonitor NetworkMonitor;
        public static readonly SystemProcessor SystemProcess;
        public static readonly UserProcessor UserProcessor;
        public static readonly MerchantProcessor MerchantProcessor;
        public static readonly GeneratorProcessor GeneratorProcessor;
        public static readonly ActorBuffProcessor ActorBuffProcessor;
        public static readonly EventProcessor EventProcessor;
        public static readonly CharacterDataProcessor CharacterDataProcessor;
        public static readonly TimedRobotProcessor TimedRobotProcessor;
        public static readonly MapQuestManager QuestManager;
        public static readonly DataQueryServer DataServer;
        public static IList<ModuleInfo> Modules { get; set; } = new List<ModuleInfo>();

        static GameShare()
        {
            Statistics = new WordStatistics();
            LocalDb = new LocalDb();
            DataSource = new MySqlDB();
            NetworkMonitor = new NetworkMonitor();
            SystemProcess = new SystemProcessor();
            UserProcessor = new UserProcessor();
            MerchantProcessor = new MerchantProcessor();
            GeneratorProcessor = new GeneratorProcessor();
            EventProcessor = new EventProcessor();
            CharacterDataProcessor = new CharacterDataProcessor();
            TimedRobotProcessor = new TimedRobotProcessor();
            ActorBuffProcessor = new ActorBuffProcessor();
            PlanesService = new PlanesService();
            DataServer = new DataQueryServer();
            QuestManager = new MapQuestManager();
            StartTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 随机获取其他服务器
        /// </summary>
        /// <returns></returns>
        public static bool GetMultiServerAddrPort(byte serverIndex, ref string sIPaddr, ref int nPort)
        {
            bool result = false;
            for (int i = 0; i < M2Share.ServerTableList.Length; i++)
            {
                TRouteInfo routeInfo = M2Share.ServerTableList[i];
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

        private static string GetRandpmRoute(TRouteInfo routeInfo, ref int gatePort)
        {
            int random = RandomNumber.GetInstance().Random(routeInfo.GateCount);
            gatePort = routeInfo.GameGatePort[random];
            return routeInfo.GameGateIP[random];
        }

        public static void LoadConfig()
        {
            SystemShare.ServerConf.LoadConfig();
            SystemShare.StringConf.LoadString();
            SystemShare.ExpConf.LoadConfig();
            SystemShare.GlobalConf.LoadConfig();
            SystemShare.GameSetting.LoadConfig();
        }
    }
}