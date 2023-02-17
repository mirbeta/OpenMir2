using GameSvr.Castle;
using GameSvr.DataSource;
using GameSvr.Event;
using GameSvr.GameCommand;
using GameSvr.GameGate;
using GameSvr.Guild;
using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.Maps;
using GameSvr.Notices;
using GameSvr.Planes;
using GameSvr.Robots;
using GameSvr.Script;
using GameSvr.Services;
using GameSvr.World;
using NLog;
using System.Collections;
using System.Collections.Concurrent;
using SystemModule.Common;
using SystemModule.Data;

namespace GameSvr
{
    public class GameApp : ServerBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public GameApp()
        {
            M2Share.SockCountMax = 0;
            M2Share.HumCountMax = 0;
            M2Share.UsrRotCountMin = 0;
            M2Share.UsrRotCountMax = 0;
            M2Share.ProcessHumanLoopTime = 0;
            M2Share.HumLimit = 30;
            M2Share.MonLimit = 30;
            M2Share.ZenLimit = 5;
            M2Share.NpcLimit = 5;
            M2Share.SocLimit = 10;
            M2Share.DecLimit = 20;
            M2Share.Config.nLoadDBErrorCount = 0;
            M2Share.Config.nLoadDBCount = 0;
            M2Share.Config.nSaveDBCount = 0;
            M2Share.Config.nDBQueryID = 0;
            M2Share.Config.ItemNumber = 0;
            M2Share.Config.ItemNumberEx = int.MaxValue / 2;
            M2Share.StartReady = false;
            M2Share.FilterWord = true;
            M2Share.Config.WinLotteryCount = 0;
            M2Share.Config.NoWinLotteryCount = 0;
            M2Share.Config.WinLotteryLevel1 = 0;
            M2Share.Config.WinLotteryLevel2 = 0;
            M2Share.Config.WinLotteryLevel3 = 0;
            M2Share.Config.WinLotteryLevel4 = 0;
            M2Share.Config.WinLotteryLevel5 = 0;
            M2Share.Config.WinLotteryLevel6 = 0;
            M2Share.DataServer = new DBService();
            M2Share.ScriptSystem = new ScriptSystem();
            M2Share.GateMgr = new GameGateMgr();
            M2Share.EventSource = new GameEventSource();
            M2Share.LogonCostLogList = new ArrayList();
            M2Share.MapMgr = new MapManager();
            M2Share.CustomItemMgr = new CustomItem();
            M2Share.MagicMgr = new MagicManager();
            M2Share.NoticeMgr = new NoticeManager();
            M2Share.GuildMgr = new GuildManager();
            M2Share.EventMgr = new EventManager();
            M2Share.CastleMgr = new CastleManager();
            M2Share.FrontEngine = new TFrontEngine();
            M2Share.WorldEngine = new WorldServer();
            M2Share.RobotMgr = new RobotManage();
            M2Share.MakeItemList = new Dictionary<string, IList<MakeItem>>(StringComparer.OrdinalIgnoreCase);
            M2Share.StartPointList = new List<StartPoint>();
            M2Share.ServerTableList = new TRouteInfo[20];
            M2Share.DenySayMsgList = new ConcurrentDictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            M2Share.MiniMapList = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            M2Share.UnbindList = new Dictionary<int, string>();
            M2Share.LineNoticeList = new List<string>();
            M2Share.QuestDiaryList = new List<IList<TQDDinfo>>();
            M2Share.AbuseTextList = new StringList();
            M2Share.MonSayMsgList = new Dictionary<string, IList<MonsterSayMsg>>(StringComparer.OrdinalIgnoreCase);
            M2Share.DisableMakeItemList = new List<string>();
            M2Share.EnableMakeItemList = new List<string>();
            M2Share.DisableSellOffList = new List<string>();
            M2Share.DisableMoveMapList = new StringList();
            M2Share.DisableSendMsgList = new List<string>();
            M2Share.MonDropLimitLIst = new ConcurrentDictionary<string, MonsterLimitDrop>(StringComparer.OrdinalIgnoreCase);
            M2Share.DisableTakeOffList = new Dictionary<int, string>();
            M2Share.UnMasterList = new List<string>();
            M2Share.UnForceMasterList = new List<string>();
            M2Share.GameLogItemNameList = new List<string>();
            M2Share.DenyIPAddrList = new List<string>();
            M2Share.DenyChrNameList = new List<string>();
            M2Share.DenyAccountList = new List<string>();
            M2Share.NoClearMonLIst = new List<string>();
            M2Share.NoHptoexpMonLIst = new List<string>();
            M2Share.ItemBindIPaddr = new List<ItemBind>();
            M2Share.ItemBindAccount = new List<ItemBind>();
            M2Share.ItemBindChrName = new List<ItemBind>();
            M2Share.ProcessMsgCriticalSection = new object();
            M2Share.ProcessHumanCriticalSection = new object();
            M2Share.Config.UserIDSection = new object();
            M2Share.UserDBCriticalSection = new object();
            M2Share.DynamicVarList = new Dictionary<string, DynamicVar>(StringComparer.OrdinalIgnoreCase);
            M2Share.SellOffItemList = new List<DealOffInfo>();
        }

        public void Initialize()
        {
            _logger.Info("读取游戏引擎数据配置文件...");
            M2Share.LoadConfig();
            LoadServerTable();
            _logger.Info("读取游戏引擎数据配置文件完成...");
            _logger.Info("读取游戏命令配置...");
            CommandMgr.RegisterCommand();
            M2Share.LoadGameLogItemNameList();
            M2Share.LoadDenyIPAddrList();
            M2Share.LoadDenyAccountList();
            M2Share.LoadDenyChrNameList();
            M2Share.LoadNoClearMonList();
            _logger.Info("正在加载物品数据库...");
            int nCode = M2Share.CommonDb.LoadItemsDB();
            if (nCode < 0)
            {
                _logger.Info("物品数据库加载失败!!!" + "Code: " + nCode);
                return;
            }
            _logger.Info($"物品数据库加载成功...[{M2Share.WorldEngine.StdItemList.Count}]");
            nCode = Map.LoadMinMap();
            if (nCode < 0)
            {
                _logger.Info("小地图数据加载失败!!!" + "Code: " + nCode);
                return;
            }
            nCode = Map.LoadMapInfo();
            if (nCode < 0)
            {
                _logger.Info("地图数据加载失败!!!" + "Code: " + nCode);
                return;
            }
            _logger.Info("正在加载怪物数据库...");
            nCode = M2Share.CommonDb.LoadMonsterDB();
            if (nCode < 0)
            {
                _logger.Info("加载怪物数据库失败!!!" + "Code: " + nCode);
                return;
            }
            _logger.Info($"加载怪物数据库成功...[{M2Share.WorldEngine.MonsterList.Count}]");
            _logger.Info("正在加载技能数据库...");
            nCode = M2Share.CommonDb.LoadMagicDB();
            if (nCode < 0)
            {
                _logger.Info("加载技能数据库失败!!!" + "Code: " + nCode);
                return;
            }
            _logger.Info($"加载技能数据库成功...[{M2Share.WorldEngine.MagicList.Count}]");
            _logger.Info("正在加载怪物刷新配置信息...");
            nCode = M2Share.LocalDb.LoadMonGen(out int mongenCount);
            if (nCode < 0)
            {
                _logger.Info("加载怪物刷新配置信息失败!!!" + "Code: " + nCode);
                return;
            }
            _logger.Info($"加载怪物刷新配置信息成功...[{mongenCount}]");
            _logger.Info("初始化怪物处理线程...");
            M2Share.WorldEngine.InitializeMonster();
            _logger.Info("初始化怪物处理完成...");
            _logger.Info("正加载怪物说话配置信息...");
            M2Share.LoadMonSayMsg();
            _logger.Info($"加载怪物说话配置信息成功...[{M2Share.MonSayMsgList.Count}]");
            M2Share.LoadDisableTakeOffList();
            M2Share.LoadMonDropLimitList();
            M2Share.LoadDisableMakeItem();
            M2Share.LoadEnableMakeItem();
            M2Share.LoadAllowSellOffItem();
            M2Share.LoadDisableMoveMap();
            M2Share.CustomItemMgr.LoadCustomItemName();
            M2Share.LoadDisableSendMsgList();
            M2Share.LoadItemBindIPaddr();
            M2Share.LoadItemBindAccount();
            M2Share.LoadItemBindChrName();
            M2Share.LoadUnMasterList();
            M2Share.LoadUnForceMasterList();
            _logger.Info("正在加载捆装物品信息...");
            nCode = M2Share.LocalDb.LoadUnbindList();
            if (nCode < 0)
            {
                _logger.Info("加载捆装物品信息失败!!!" + "Code: " + nCode);
                return;
            }
            _logger.Info("加载捆装物品信息成功...");
            _logger.Info("加载物品寄售系统...");
            M2Share.CommonDb.LoadSellOffItemList();
            _logger.Info("正在加载任务地图信息...");
            nCode = M2Share.LocalDb.LoadMapQuest();
            if (nCode < 0)
            {
                _logger.Info("加载任务地图信息失败!!!");
                return;
            }
            _logger.Info("加载任务地图信息成功...");
            _logger.Info("正在加载任务说明信息...");
            nCode = M2Share.LocalDb.LoadQuestDiary();
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
            if (!M2Share.LoadLineNotice(M2Share.GetNoticeFilePath("LineNotice.txt")))
            {
                _logger.Info("加载公告提示信息失败!!!");
            }
            _logger.Info("加载公告提示信息成功...");
            LocalDB.LoadAdminList();
            _logger.Info("管理员列表加载成功...");
        }

        public void StartServer(CancellationToken stoppingToken)
        {
            try
            {
                M2Share.MapMgr.LoadMapDoor();
                Map.StartMakeStoneThread();
                M2Share.LocalDb.LoadMerchant();
                _logger.Info("交易NPC列表加载成功...");
                M2Share.LocalDb.LoadNpcs();
                _logger.Info("管理NPC列表加载成功...");
                M2Share.LocalDb.LoadMakeItem();
                _logger.Info("炼制物品信息加载成功...");
                M2Share.LocalDb.LoadStartPoint();
                _logger.Info("回城点配置加载成功...");
                _logger.Info("正在初始安全区光圈...");
                M2Share.MapMgr.MakeSafePkZone();
                _logger.Info("安全区光圈初始化成功...");
                M2Share.FrontEngine.Start(stoppingToken);
                M2Share.WorldEngine.InitializationMonsterThread();
                if (!M2Share.Config.VentureServer)
                {
                    LocalDB.LoadGuardList();
                    _logger.Info("守卫列表加载成功...");
                }
                _logger.Info("游戏处理引擎初始化成功...");
                if (M2Share.ServerIndex == 0)
                {
                    PlanesServer.Instance.StartSnapsServer();
                    _logger.Debug("当前服务运行主节点模式...");
                }
                else
                {
                    PlanesClient.Instance.ConnectMsgServer();
                    _logger.Info($"当前运行从节点模式...[{M2Share.Config.MsgSrvAddr}:{M2Share.Config.MsgSrvPort}]");
                }
                IdSrvClient.Instance.Initialize();
                M2Share.GuildMgr.LoadGuildInfo();
                M2Share.CastleMgr.LoadCastleList();
                M2Share.CastleMgr.Initialize();
                M2Share.WorldEngine.Initialize();
                M2Share.StartReady = true;
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
            string sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.BaseDir, "!servertable.txt");
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
                        if (M2Share.ServerTableList[nRouteIdx] == null)
                        {
                            M2Share.ServerTableList[nRouteIdx] = new TRouteInfo();
                        }
                        M2Share.ServerTableList[nRouteIdx].GateCount = 0;
                        M2Share.ServerTableList[nRouteIdx].ServerIdx = HUtil32.StrToInt(sIdx, 0);
                        M2Share.ServerTableList[nRouteIdx].SelGateIP = sSelGateIPaddr.Trim();
                        int nGateIdx = 0;
                        while (!string.IsNullOrEmpty(sGameGate))
                        {
                            sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGateIPaddr, new[] { " ", "\09" });
                            sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGatePort, new[] { " ", "\09" });
                            M2Share.ServerTableList[nRouteIdx].GameGateIP[nGateIdx] = sGameGateIPaddr.Trim();
                            M2Share.ServerTableList[nRouteIdx].GameGatePort[nGateIdx] = HUtil32.StrToInt(sGameGatePort, 0);
                            nGateIdx++;
                        }
                        M2Share.ServerTableList[nRouteIdx].GateCount = nGateIdx;
                        nRouteIdx++;
                        if (nRouteIdx > M2Share.ServerTableList.Length)
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
                M2Share.AbuseTextList.Clear();
                M2Share.AbuseTextList.LoadFromFile(fileName);
                while (true)
                {
                    if (M2Share.AbuseTextList.Count <= lineCount)
                    {
                        break;
                    }
                    string sText = M2Share.AbuseTextList[lineCount].Trim();
                    if (string.IsNullOrEmpty(sText))
                    {
                        M2Share.AbuseTextList.RemoveAt(lineCount);
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
