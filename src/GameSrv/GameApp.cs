using GameSrv.Robots;
using M2Server.Castle;
using M2Server.DataSource;
using M2Server.Event;
using M2Server.Guild;
using M2Server.Items;
using M2Server.Maps;
using M2Server.Network;
using M2Server.Notices;
using M2Server.Planes;
using M2Server.Services;
using M2Server.World;
using M2Server.World.Managers;
using NLog;
using ScriptModule;
using System.Collections;
using System.Collections.Concurrent;
using SystemModule.Common;
using SystemModule.Data;

namespace GameSrv
{
    public class GameApp : ServerBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public GameApp()
        {
            GameShare.HumLimit = 30;
            GameShare.MonLimit = 30;
            GameShare.ZenLimit = 5;
            GameShare.NpcLimit = 5;
            GameShare.SocLimit = 10;
            GameShare.DecLimit = 20;
            GameShare.Config.nLoadDBErrorCount = 0;
            GameShare.Config.nLoadDBCount = 0;
            GameShare.Config.nSaveDBCount = 0;
            GameShare.Config.nDBQueryID = 0;
            GameShare.Config.ItemNumber = 0;
            GameShare.Config.ItemNumberEx = int.MaxValue / 2;
            GameShare.StartReady = false;
            GameShare.FilterWord = true;
            GameShare.Config.WinLotteryCount = 0;
            GameShare.Config.NoWinLotteryCount = 0;
            GameShare.Config.WinLotteryLevel1 = 0;
            GameShare.Config.WinLotteryLevel2 = 0;
            GameShare.Config.WinLotteryLevel3 = 0;
            GameShare.Config.WinLotteryLevel4 = 0;
            GameShare.Config.WinLotteryLevel5 = 0;
            GameShare.Config.WinLotteryLevel6 = 0;
            GameShare.LogonCostLogList = new ArrayList();
            GameShare.MakeItemList = new Dictionary<string, IList<MakeItem>>(StringComparer.OrdinalIgnoreCase);
            GameShare.StartPointList = new List<StartPoint>();
            GameShare.ServerTableList = new TRouteInfo[20];
            GameShare.DenySayMsgList = new ConcurrentDictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            GameShare.MiniMapList = new ConcurrentDictionary<string, short>(StringComparer.OrdinalIgnoreCase);
            GameShare.UnbindList = new Dictionary<int, string>();
            GameShare.LineNoticeList = new List<string>();
            GameShare.QuestDiaryList = new List<IList<TQDDinfo>>();
            GameShare.AbuseTextList = new StringList();
            GameShare.MonSayMsgList = new Dictionary<string, IList<MonsterSayMsg>>(StringComparer.OrdinalIgnoreCase);
            GameShare.DisableMakeItemList = new List<string>();
            GameShare.EnableMakeItemList = new List<string>();
            GameShare.DisableSellOffList = new List<string>();
            GameShare.DisableMoveMapList = new StringList();
            GameShare.DisableSendMsgList = new List<string>();
            GameShare.MonDropLimitLIst = new ConcurrentDictionary<string, MonsterLimitDrop>(StringComparer.OrdinalIgnoreCase);
            GameShare.DisableTakeOffList = new Dictionary<int, string>();
            GameShare.UnMasterList = new List<string>();
            GameShare.UnForceMasterList = new List<string>();
            GameShare.GameLogItemNameList = new List<string>();
            GameShare.DenyIPAddrList = new List<string>();
            GameShare.DenyChrNameList = new List<string>();
            GameShare.DenyAccountList = new List<string>();
            GameShare.NoClearMonLIst = new List<string>();
            GameShare.NoHptoexpMonLIst = new List<string>();
            GameShare.ItemBindIPaddr = new List<ItemBind>();
            GameShare.ItemBindAccount = new List<ItemBind>();
            GameShare.ItemBindChrName = new List<ItemBind>();
            GameShare.ProcessMsgCriticalSection = new object();
            GameShare.ProcessHumanCriticalSection = new object();
            GameShare.UserDBCriticalSection = new object();
            GameShare.DynamicVarList = new Dictionary<string, DynamicVar>(StringComparer.OrdinalIgnoreCase);
            GameShare.SellOffItemList = new List<DealOffInfo>();
        }

        public void Initialize(CancellationToken stoppingToken)
        {
            _logger.Info("读取游戏引擎数据配置文件...");
            GameShare.GeneratorProcessor.Initialize(stoppingToken);
            GameShare.DataServer = new DBService();
            GameShare.MarketService = new MarketService();
            GameShare.ChatChannel = new ChatChannelService();
            GameShare.SocketMgr = new ThreadSocketMgr();
            GameShare.EventSource = new GameEventSource();
            GameShare.MapMgr = new MapManager();
            GameShare.CustomItemMgr = new CustomItem();
            GameShare.NoticeMgr = new NoticeManager();
            GameShare.GuildMgr = new GuildManager();
            GameShare.MarketManager = new MarketManager();
            GameShare.EventMgr = new EventManager();
            GameShare.CastleMgr = new CastleManager();
            GameShare.FrontEngine = new FrontEngine();
            GameShare.WorldEngine = new WorldServer();
            GameShare.RobotMgr = new RobotManage();
            GameShare.LoadConfig();
            LoadServerTable();
            _logger.Info("初始化游戏引擎数据配置文件完成...");
            //CommandMgr.RegisterCommand();
            GameShare.LoadGameLogItemNameList();
            GameShare.LoadDenyIPAddrList();
            GameShare.LoadDenyAccountList();
            GameShare.LoadDenyChrNameList();
            GameShare.LoadNoClearMonList();
            _logger.Info("正在加载物品数据库...");
            var nCode = GameShare.CommonDb.LoadItemsDB();
            if (nCode < 0)
            {
                _logger.Info($"物品数据库加载失败!!! Code: {nCode}");
                return;
            }
            _logger.Info($"物品数据库加载成功...[{ItemSystem.StdItemList.Count}]");
            nCode = Map.LoadMinMap();
            if (nCode < 0)
            {
                _logger.Info($"小地图数据加载失败!!! Code: {nCode}");
                return;
            }
            nCode = Map.LoadMapInfo();
            if (nCode < 0)
            {
                _logger.Info($"地图数据加载失败!!! Code: {nCode}");
                return;
            }
            _logger.Info("正在加载怪物数据库...");
            nCode = GameShare.CommonDb.LoadMonsterDB();
            if (nCode < 0)
            {
                _logger.Info($"加载怪物数据库失败!!! Code: {nCode}");
                return;
            }
            _logger.Info($"加载怪物数据库成功...[{GameShare.WorldEngine.MonsterList.Count}]");
            _logger.Info("正在加载技能数据库...");
            nCode = GameShare.CommonDb.LoadMagicDB();
            if (nCode < 0)
            {
                _logger.Info($"加载技能数据库失败!!! Code: {nCode}");
                return;
            }
            _logger.Info($"加载技能数据库成功...[{GameShare.WorldEngine.MagicList.Count}]");
            _logger.Info("正在加载怪物刷新配置信息...");
            nCode = GameShare.LocalDb.LoadMonGen(out var mongenCount);
            if (nCode < 0)
            {
                _logger.Info($"加载怪物刷新配置信息失败!!! Code: {nCode}");
                return;
            }
            _logger.Info($"加载怪物刷新配置信息成功...[{mongenCount}]");
            _logger.Info("初始化怪物处理线程...");
            GameShare.WorldEngine.InitializeMonster();
            _logger.Info("初始化怪物处理完成...");
            _logger.Info("正加载怪物说话配置信息...");
            GameShare.LoadMonSayMsg();
            _logger.Info($"加载怪物说话配置信息成功...[{GameShare.MonSayMsgList.Count}]");
            GameShare.LoadDisableTakeOffList();
            GameShare.LoadMonDropLimitList();
            GameShare.LoadDisableMakeItem();
            GameShare.LoadEnableMakeItem();
            GameShare.LoadAllowSellOffItem();
            GameShare.LoadDisableMoveMap();
            GameShare.CustomItemMgr.LoadCustomItemName();
            GameShare.LoadDisableSendMsgList();
            GameShare.LoadItemBindIPaddr();
            GameShare.LoadItemBindAccount();
            GameShare.LoadItemBindChrName();
            GameShare.LoadUnMasterList();
            GameShare.LoadUnForceMasterList();
            _logger.Info("正在加载捆装物品信息...");
            nCode = GameShare.LocalDb.LoadUnbindList();
            if (nCode < 0)
            {
                _logger.Info($"加载捆装物品信息失败!!! Code: {nCode}");
                return;
            }
            _logger.Info("加载捆装物品信息成功...");
            _logger.Info("加载物品寄售系统...");
            GameShare.CommonDb.LoadSellOffItemList();
            _logger.Info("正在加载任务地图信息...");
            nCode = GameShare.LocalDb.LoadMapQuest();
            if (nCode < 0)
            {
                _logger.Info("加载任务地图信息失败!!!");
                return;
            }
            _logger.Info("加载任务地图信息成功...");
            _logger.Info("正在加载任务说明信息...");
            nCode = GameShare.LocalDb.LoadQuestDiary();
            if (nCode < 0)
            {
                _logger.Info("加载任务说明信息失败!!!");
                return;
            }
            _logger.Info("加载任务说明信息成功...");
            if (LoadAbuseInformation(".\\!abuse.txt"))
            {
                _logger.Info("加载文字过滤信息成功...");
            }
            _logger.Info("正在加载公告提示信息...");
            if (!GameShare.LoadLineNotice(GameShare.GetNoticeFilePath("LineNotice.txt")))
            {
                _logger.Info("加载公告提示信息失败!!!");
            }
            _logger.Info("加载公告提示信息成功...");
            LocalDb.LoadAdminList();
            _logger.Info("管理员列表加载成功...");
            GameShare.SocketMgr.Initialize();
            _logger.Info("正在初始化网络引擎...");
        }

        public void StartServer(CancellationToken stoppingToken)
        {
            try
            {
                GameShare.MapMgr.LoadMapDoor();
                GameShare.LocalDb.LoadMerchant();
                _logger.Info("交易NPC列表加载成功...");
                GameShare.LocalDb.LoadNpcs();
                _logger.Info("管理NPC列表加载成功...");
                GameShare.LocalDb.LoadMakeItem();
                _logger.Info("炼制物品信息加载成功...");
                GameShare.LocalDb.LoadStartPoint();
                _logger.Info("回城点配置加载成功...");
                _logger.Info("正在初始安全区光圈...");
                GameShare.MapMgr.MakeSafePkZone();
                _logger.Info("安全区光圈初始化成功...");
                GameShare.WorldEngine.InitializationMonsterThread();
                if (!GameShare.Config.VentureServer)
                {
                    LocalDb.LoadGuardList();
                    _logger.Info("守卫列表加载成功...");
                }
                _logger.Info("游戏处理引擎初始化成功...");
                if (GameShare.ServerIndex == 0)
                {
                    PlanesServer.Instance.StartPlanesServer();
                    _logger.Debug("主机运行模式...");
                }
                else
                {
                    PlanesClient.Instance.ConnectPlanesServer();
                    _logger.Info($"节点运行模式...主机端口:[{GameShare.Config.MasterSrvAddr}:{GameShare.Config.MasterSrvPort}]");
                }
                IdSrvClient.Instance.Initialize();
                GameShare.GuildMgr.LoadGuildInfo();
                GameShare.CastleMgr.LoadCastleList();
                GameShare.CastleMgr.Initialize();
                GameShare.WorldEngine.Initialize();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace);
            }
        }

        private static void LoadServerTable()
        {
            int nRouteIdx = 0;
            string sIdx = string.Empty;
            string sSelGateIPaddr = string.Empty;
            string sGameGateIPaddr = string.Empty;
            string sGameGatePort = string.Empty;
            string sFileName = Path.Combine(GameShare.BasePath, "!servertable.txt");
            if (File.Exists(sFileName))
            {
                StringList loadList = new StringList();
                loadList.LoadFromFile(sFileName);
                for (int i = 0; i < loadList.Count; i++)
                {
                    string sLineText = loadList[i];
                    if (!string.IsNullOrEmpty(sLineText) && sLineText[0] != ';')
                    {
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sIdx, new[] { " ", "\09" });
                        string sGameGate = HUtil32.GetValidStr3(sLineText, ref sSelGateIPaddr, new[] { " ", "\09" });
                        if (string.IsNullOrEmpty(sIdx) || string.IsNullOrEmpty(sGameGate) || string.IsNullOrEmpty(sSelGateIPaddr))
                        {
                            continue;
                        }
                        if (GameShare.ServerTableList[nRouteIdx] == null)
                        {
                            GameShare.ServerTableList[nRouteIdx] = new TRouteInfo();
                        }
                        GameShare.ServerTableList[nRouteIdx].GateCount = 0;
                        GameShare.ServerTableList[nRouteIdx].ServerIdx = HUtil32.StrToInt(sIdx, 0);
                        GameShare.ServerTableList[nRouteIdx].SelGateIP = sSelGateIPaddr.Trim();
                        int nGateIdx = 0;
                        while (!string.IsNullOrEmpty(sGameGate))
                        {
                            sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGateIPaddr, new[] { " ", "\09" });
                            sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGatePort, new[] { " ", "\09" });
                            GameShare.ServerTableList[nRouteIdx].GameGateIP[nGateIdx] = sGameGateIPaddr.Trim();
                            GameShare.ServerTableList[nRouteIdx].GameGatePort[nGateIdx] = HUtil32.StrToInt(sGameGatePort, 0);
                            nGateIdx++;
                        }
                        GameShare.ServerTableList[nRouteIdx].GateCount = nGateIdx;
                        nRouteIdx++;
                        if (nRouteIdx > GameShare.ServerTableList.Length)
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 读取文字过滤配置
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static bool LoadAbuseInformation(string fileName)
        {
            int lineCount = 0;
            bool result = false;
            if (File.Exists(fileName))
            {
                GameShare.AbuseTextList.Clear();
                GameShare.AbuseTextList.LoadFromFile(fileName);
                while (true)
                {
                    if (GameShare.AbuseTextList.Count <= lineCount)
                    {
                        break;
                    }
                    string sText = GameShare.AbuseTextList[lineCount].Trim();
                    if (string.IsNullOrEmpty(sText))
                    {
                        GameShare.AbuseTextList.RemoveAt(lineCount);
                        continue;
                    }
                    lineCount++;
                }
                result = true;
            }
            return result;
        }
    }
}