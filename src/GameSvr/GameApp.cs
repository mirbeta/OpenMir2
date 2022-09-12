using GameSvr.Actor;
using GameSvr.Castle;
using GameSvr.Command;
using GameSvr.DataStores;
using GameSvr.Event;
using GameSvr.Event.Events;
using GameSvr.GateWay;
using GameSvr.Guild;
using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.Maps;
using GameSvr.Notices;
using GameSvr.Robots;
using GameSvr.Script;
using GameSvr.Services;
using GameSvr.Snaps;
using GameSvr.UsrSystem;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Concurrent;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr
{
    public class GameApp : ServerBase
    {
        public GameApp(ILogger<ServerBase> logger) : base(logger)
        {
            M2Share.LocalDb = new LocalDB();
            M2Share.CommonDb = new CommonDB();
            M2Share.g_nSockCountMax = 0;
            M2Share.g_nHumCountMax = 0;
            M2Share.dwUsrRotCountMin = 0;
            M2Share.dwUsrRotCountMax = 0;
            M2Share.g_nProcessHumanLoopTime = 0;
            M2Share.g_dwHumLimit = 30;
            M2Share.g_dwMonLimit = 30;
            M2Share.g_dwZenLimit = 5;
            M2Share.g_dwNpcLimit = 5;
            M2Share.g_dwSocLimit = 10;
            M2Share.nDecLimit = 20;
            M2Share.Config.nLoadDBErrorCount = 0;
            M2Share.Config.nLoadDBCount = 0;
            M2Share.Config.nSaveDBCount = 0;
            M2Share.Config.nDBQueryID = 0;
            M2Share.Config.ItemNumber = 0;
            M2Share.Config.ItemNumberEx = int.MaxValue / 2;
            M2Share.StartReady = false;
            M2Share.boFilterWord = true;
            M2Share.Config.WinLotteryCount = 0;
            M2Share.Config.NoWinLotteryCount = 0;
            M2Share.Config.WinLotteryLevel1 = 0;
            M2Share.Config.WinLotteryLevel2 = 0;
            M2Share.Config.WinLotteryLevel3 = 0;
            M2Share.Config.WinLotteryLevel4 = 0;
            M2Share.Config.WinLotteryLevel5 = 0;
            M2Share.Config.WinLotteryLevel6 = 0;
            M2Share.DataServer = new DBService();
            M2Share.ActorMgr = new ActorMgr();
            M2Share.ScriptSystem = new ScriptSystem();
            M2Share.GateMgr = new GameGateMgr();
            M2Share.FindPath = new FindPath();
            M2Share.CommandMgr = new CommandManager();
            M2Share.CellObjectSystem = new CellObjectMgr();
            M2Share.ItemLogQueue = new();
            M2Share.LogonCostLogList = new ArrayList();
            M2Share.MapMgr = new MapManager();
            M2Share.ItemUnit = new ItemUnit();
            M2Share.MagicMgr = new MagicManager();
            M2Share.NoticeMgr = new NoticeManager();
            M2Share.GuildMgr = new GuildManager();
            M2Share.EventMgr = new EventManager();
            M2Share.CastleMgr = new CastleManager();
            M2Share.FrontEngine = new TFrontEngine();
            M2Share.UserEngine = new UserEngine();
            M2Share.RobotMgr = new RobotManage();
            M2Share.MakeItemList = new Dictionary<string, IList<MakeItem>>(StringComparer.OrdinalIgnoreCase);
            M2Share.StartPointList = new List<StartPoint>();
            M2Share.ServerTableList = new TRouteInfo[20];
            M2Share.DenySayMsgList = new ConcurrentDictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            M2Share.MiniMapList = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            M2Share.g_UnbindList = new Dictionary<int, string>();
            M2Share.LineNoticeList = new List<string>();
            M2Share.QuestDiaryList = new List<IList<TQDDinfo>>();
            M2Share.AbuseTextList = new StringList();
            M2Share.g_MonSayMsgList = new Dictionary<string, IList<TMonSayMsg>>(StringComparer.OrdinalIgnoreCase);
            M2Share.g_DisableMakeItemList = new List<string>();
            M2Share.g_EnableMakeItemList = new List<string>();
            M2Share.g_DisableSellOffList = new List<string>();
            M2Share.g_DisableMoveMapList = new List<string>();
            M2Share.g_DisableSendMsgList = new List<string>();
            M2Share.g_MonDropLimitLIst = new ConcurrentDictionary<string, TMonDrop>(StringComparer.OrdinalIgnoreCase);
            M2Share.g_DisableTakeOffList = new List<string>();
            M2Share.g_UnMasterList = new List<string>();
            M2Share.g_UnForceMasterList = new List<string>();
            M2Share.g_GameLogItemNameList = new List<string>();
            M2Share.g_DenyIPAddrList = new List<string>();
            M2Share.g_DenyChrNameList = new List<string>();
            M2Share.g_DenyAccountList = new List<string>();
            M2Share.g_NoClearMonLIst = new List<string>();
            M2Share.g_NoHptoexpMonLIst = new List<string>();
            M2Share.g_ItemBindIPaddr = new List<TItemBind>();
            M2Share.g_ItemBindAccount = new List<TItemBind>();
            M2Share.g_ItemBindCharName = new List<TItemBind>();
            M2Share.ProcessMsgCriticalSection = new object();
            M2Share.ProcessHumanCriticalSection = new object();
            M2Share.Config.UserIDSection = new object();
            M2Share.UserDBSection = new object();
            M2Share.g_DynamicVarList = new Dictionary<string, TDynamicVar>(StringComparer.OrdinalIgnoreCase);
            M2Share.sSellOffItemList = new List<DealOffInfo>();
            M2Share.dwRunDBTimeMax = HUtil32.GetTickCount();
        }

        public void Initialize()
        {
            M2Share.LoadConfig();
            LoadServerTable();
            M2Share.CommandMgr.RegisterCommand();
            M2Share.LoadGameLogItemNameList();
            M2Share.LoadDenyIPAddrList();
            M2Share.LoadDenyAccountList();
            M2Share.LoadDenyChrNameList();
            M2Share.LoadNoClearMonList();
            _logger.LogInformation("正在加载物品数据库...");
            var nCode = M2Share.CommonDb.LoadItemsDB();
            if (nCode < 0)
            {
                _logger.LogInformation("物品数据库加载失败!!!" + "Code: " + nCode);
                return;
            }
            _logger.LogInformation($"物品数据库加载成功({M2Share.UserEngine.StdItemList.Count})...");
            _logger.LogInformation("正在加载数据图文件...");
            nCode = Maps.Maps.LoadMinMap();
            if (nCode < 0)
            {
                _logger.LogInformation("小地图数据加载失败!!!" + "Code: " + nCode);
                return;
            }
            _logger.LogInformation("小地图数据加载成功...");
            _logger.LogInformation("正在加载地图数据...");
            nCode = Maps.Maps.LoadMapInfo();
            if (nCode < 0)
            {
                _logger.LogInformation("地图数据加载失败!!!" + "Code: " + nCode);
                return;
            }
            _logger.LogInformation($"地图数据加载成功({M2Share.MapMgr.Maps.Count})...");
            _logger.LogInformation("正在加载怪物数据库...");
            nCode = M2Share.CommonDb.LoadMonsterDB();
            if (nCode < 0)
            {
                _logger.LogInformation("加载怪物数据库失败!!!" + "Code: " + nCode);
                return;
            }
            _logger.LogInformation($"加载怪物数据库成功({M2Share.UserEngine.MonsterList.Count})...");
            _logger.LogInformation("正在加载技能数据库...");
            nCode = M2Share.CommonDb.LoadMagicDB();
            if (nCode < 0)
            {
                _logger.LogInformation("加载技能数据库失败!!!" + "Code: " + nCode);
                return;
            }
            _logger.LogInformation($"加载技能数据库成功({M2Share.UserEngine.MagicList.Count})...");
            _logger.LogInformation("正在加载怪物刷新配置信息...");
            nCode = M2Share.LocalDb.LoadMonGen();
            if (nCode < 0)
            {
                _logger.LogInformation("加载怪物刷新配置信息失败!!!" + "Code: " + nCode);
                return;
            }
            _logger.LogInformation($"加载怪物刷新配置信息成功({M2Share.UserEngine.MonGenList.Count})...");
            _logger.LogInformation("正加载怪物说话配置信息...");
            M2Share.LoadMonSayMsg();
            _logger.LogInformation($"加载怪物说话配置信息成功({M2Share.g_MonSayMsgList.Count})...");
            M2Share.LoadDisableTakeOffList();
            M2Share.LoadMonDropLimitList();
            M2Share.LoadDisableMakeItem();
            M2Share.LoadEnableMakeItem();
            M2Share.LoadAllowSellOffItem();
            M2Share.LoadDisableMoveMap();
            M2Share.ItemUnit.LoadCustomItemName();
            M2Share.LoadDisableSendMsgList();
            M2Share.LoadItemBindIPaddr();
            M2Share.LoadItemBindAccount();
            M2Share.LoadItemBindCharName();
            M2Share.LoadUnMasterList();
            M2Share.LoadUnForceMasterList();
            _logger.LogInformation("正在加载捆装物品信息...");
            nCode = M2Share.LocalDb.LoadUnbindList();
            if (nCode < 0)
            {
                _logger.LogInformation("加载捆装物品信息失败!!!" + "Code: " + nCode);
                return;
            }
            _logger.LogInformation("加载捆装物品信息成功...");
            _logger.LogInformation("加载物品寄售系统...");
            M2Share.CommonDb.LoadSellOffItemList();
            _logger.LogInformation("正在加载任务地图信息...");
            nCode = M2Share.LocalDb.LoadMapQuest();
            if (nCode < 0)
            {
                _logger.LogInformation("加载任务地图信息失败!!!");
                return;
            }
            _logger.LogInformation("加载任务地图信息成功...");
            _logger.LogInformation("正在加载任务说明信息...");
            nCode = M2Share.LocalDb.LoadQuestDiary();
            if (nCode < 0)
            {
                _logger.LogInformation("加载任务说明信息失败!!!");
                return;
            }
            _logger.LogInformation("加载任务说明信息成功...");
            if (LoadAbuseInformation(".\\!abuse.txt"))
            {
                _logger.LogInformation("加载文字过滤信息成功...");
            }
            _logger.LogInformation("正在加载公告提示信息...");
            if (!M2Share.LoadLineNotice(Path.Combine(M2Share.BasePath, M2Share.Config.NoticeDir, "LineNotice.txt")))
            {
                _logger.LogInformation("加载公告提示信息失败!!!");
            }
            _logger.LogInformation("加载公告提示信息成功...");
            M2Share.LocalDb.LoadAdminList();
            _logger.LogInformation("管理员列表加载成功...");
            M2Share.GuildMgr.LoadGuildInfo();
            _logger.LogInformation("行会列表加载成功...");
            M2Share.CastleMgr.LoadCastleList();
            _logger.LogInformation("城堡列表加载成功...");
            M2Share.CastleMgr.Initialize();
            _logger.LogInformation("城堡城初始完成...");
            if (M2Share.ServerIndex == 0)
            {
                SnapsmService.Instance.StartSnapsServer();
                _logger.LogInformation("当前服务器运行主节点模式...");
            }
            else
            {
                SnapsmClient.Instance.ConnectMsgServer();
                _logger.LogInformation($"当前运行从节点模式...[{M2Share.Config.MsgSrvAddr}:{M2Share.Config.MsgSrvPort}]");
            }
        }

        public void StartEngine(CancellationToken stoppingToken)
        {
            try
            {
                IdSrvClient.Instance.Initialize();
                _logger.LogInformation("登录服务器连接初始化完成...");
                M2Share.MapMgr.LoadMapDoor();
                _logger.LogInformation("地图环境加载成功...");
                MakeStoneMines();
                _logger.LogInformation("矿物数据初始成功...");
                M2Share.LocalDb.LoadMerchant();
                _logger.LogInformation("交易NPC列表加载成功...");
                if (!M2Share.Config.boVentureServer)
                {
                    M2Share.LocalDb.LoadGuardList();
                    _logger.LogInformation("守卫列表加载成功...");
                }
                M2Share.LocalDb.LoadNpcs();
                _logger.LogInformation("管理NPC列表加载成功...");
                M2Share.LocalDb.LoadMakeItem();
                _logger.LogInformation("炼制物品信息加载成功...");
                M2Share.LocalDb.LoadStartPoint();
                _logger.LogInformation("回城点配置加载成功...");
                _logger.LogInformation("正在初始安全区光圈...");
                M2Share.MapMgr.MakeSafePkZone();
                _logger.LogInformation("安全区光圈初始化成功...");
                M2Share.FrontEngine.Start(stoppingToken);
                _logger.LogInformation("人物数据引擎启动成功...");
                M2Share.UserEngine.Initialize();
                _logger.LogInformation("游戏处理引擎初始化成功...");
                _logger.LogInformation(M2Share.g_sVersion);
                _logger.LogInformation(M2Share.g_sUpDateTime);
                M2Share.StartReady = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Source);
            }
        }

        private void MakeStoneMines()
        {
            var mineMapList = M2Share.MapMgr.GetMineMaps();
            for (var i = 0; i < mineMapList.Count; i++)
            {
                var Envir = mineMapList[i];
                for (var nW = 0; nW < Envir.Width; nW++)
                {
                    for (var nH = 0; nH < Envir.Height; nH++)
                    {
                        var mine = new StoneMineEvent(Envir, nW, nH, Grobal2.ET_MINE);
                        if (!mine.AddToMap)
                        {
                            mine.Dispose();
                        }
                    }
                }
            }
        }

        private void LoadServerTable()
        {
            var nRouteIdx = 0;
            var sLineText = string.Empty;
            var sIdx = string.Empty;
            var sSelGateIPaddr = string.Empty;
            var sGameGateIPaddr = string.Empty;
            var sGameGate = string.Empty;
            var sGameGatePort = string.Empty;
            var sMapName = string.Empty;
            var sMapInfo = string.Empty;
            var sServerIndex = string.Empty;
            var sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.BaseDir, "!servertable.txt");
            if (File.Exists(sFileName))
            {
                var LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i];
                    if (sLineText != "" && sLineText[0] != ';')
                    {
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sIdx, new[] { " ", "\09" });
                        sGameGate = HUtil32.GetValidStr3(sLineText, ref sSelGateIPaddr, new[] { " ", "\09" });
                        if (sIdx == "" || sGameGate == "" || sSelGateIPaddr == "")
                        {
                            continue;
                        }
                        if (M2Share.ServerTableList[nRouteIdx] == null)
                        {
                            M2Share.ServerTableList[nRouteIdx] = new TRouteInfo();
                        }
                        M2Share.ServerTableList[nRouteIdx].nGateCount = 0;
                        M2Share.ServerTableList[nRouteIdx].nServerIdx = HUtil32.Str_ToInt(sIdx, 0);
                        M2Share.ServerTableList[nRouteIdx].sSelGateIP = sSelGateIPaddr.Trim();
                        var nGateIdx = 0;
                        while (sGameGate != "")
                        {
                            sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGateIPaddr, new[] { " ", "\09" });
                            sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGatePort, new[] { " ", "\09" });
                            M2Share.ServerTableList[nRouteIdx].sGameGateIP[nGateIdx] = sGameGateIPaddr.Trim();
                            M2Share.ServerTableList[nRouteIdx].nGameGatePort[nGateIdx] = HUtil32.Str_ToInt(sGameGatePort, 0);
                            nGateIdx++;
                        }
                        M2Share.ServerTableList[nRouteIdx].nGateCount = nGateIdx;
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
        /// <param name="FileName"></param>
        /// <returns></returns>
        private bool LoadAbuseInformation(string FileName)
        {
            int lineCount = 0;
            var result = false;
            if (File.Exists(FileName))
            {
                M2Share.AbuseTextList.Clear();
                M2Share.AbuseTextList.LoadFromFile(FileName);
                while (true)
                {
                    if (M2Share.AbuseTextList.Count <= lineCount)
                    {
                        break;
                    }
                    var sText = M2Share.AbuseTextList[lineCount].Trim();
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
