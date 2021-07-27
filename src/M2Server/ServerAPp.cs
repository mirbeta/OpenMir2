using M2Server.CommandSystem;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SystemModule;
using SystemModule.Common;

namespace M2Server
{
    public class ServerApp : ServerBase
    {
        public ServerApp()
        {
            M2Share.MainOutMessage("正在读取配置信息...");
            InitializeServer();
            M2Share.MainOutMessage("读取配置信息完成...");
        }

        public void StartServer(CancellationToken token)
        {
            int nCode;
            M2Share.LocalDB = new LocalDB();
            if (!M2Share.LocalDB.CheckDataBase())
            {
                return;
            }
            try
            {
                M2Share.LoadGameLogItemNameList();
                M2Share.LoadDenyIPAddrList();
                M2Share.LoadDenyAccountList();
                M2Share.LoadDenyChrNameList();
                M2Share.LoadNoClearMonList();
                M2Share.MainOutMessage("正在加载物品数据库...");
                nCode = M2Share.LocalDB.LoadItemsDB();
                if (nCode < 0)
                {
                    M2Share.MainOutMessage("物品数据库加载失败！！！" + "Code: " + nCode);
                    return;
                }
                M2Share.MainOutMessage($"物品数据库加载成功({M2Share.UserEngine.StdItemList.Count})...");
                M2Share.MainOutMessage("正在加载数据图文件...");
                nCode = Maps.LoadMinMap();
                if (nCode < 0)
                {
                    M2Share.MainOutMessage("小地图数据加载失败！！！" + "Code: " + nCode);
                    return;
                }
                M2Share.MainOutMessage("小地图数据加载成功...");
                M2Share.MainOutMessage("正在加载地图数据...");
                nCode = Maps.LoadMapInfo();
                if (nCode < 0)
                {
                    M2Share.MainOutMessage("地图数据加载失败！！！" + "Code: " + nCode);
                    return;
                }
                M2Share.MainOutMessage($"地图数据加载成功({M2Share.g_MapManager.Maps.Count})...");
                M2Share.MainOutMessage("正在加载怪物数据库...");
                nCode = M2Share.LocalDB.LoadMonsterDB();
                if (nCode < 0)
                {
                    M2Share.MainOutMessage("加载怪物数据库失败！！！" + "Code: " + nCode);
                    return;
                }
                M2Share.MainOutMessage($"加载怪物数据库成功({M2Share.UserEngine.MonsterList.Count})...");
                M2Share.MainOutMessage("正在加载技能数据库...");
                nCode = M2Share.LocalDB.LoadMagicDB();
                if (nCode < 0)
                {
                    M2Share.MainOutMessage("加载技能数据库失败！！！" + "Code: " + nCode);
                    return;
                }
                M2Share.MainOutMessage($"加载技能数据库成功({M2Share.UserEngine.m_MagicList.Count})...");
                M2Share.MainOutMessage("正在加载怪物刷新配置信息...");
                nCode = M2Share.LocalDB.LoadMonGen();
                if (nCode < 0)
                {
                    M2Share.MainOutMessage("加载怪物刷新配置信息失败！！！" + "Code: " + nCode);
                    return;
                }
                M2Share.MainOutMessage($"加载怪物刷新配置信息成功({M2Share.UserEngine.m_MonGenList.Count})...");
                M2Share.MainOutMessage("正加载怪物说话配置信息...");
                M2Share.LoadMonSayMsg();
                M2Share.MainOutMessage($"加载怪物说话配置信息成功({M2Share.g_MonSayMsgList.Count})...");
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
                M2Share.MainOutMessage("正在加载捆装物品信息...");
                nCode = M2Share.LocalDB.LoadUnbindList();
                if (nCode < 0)
                {
                    M2Share.MainOutMessage("加载捆装物品信息失败！！！" + "Code: " + nCode);
                    return;
                }
                M2Share.MainOutMessage("加载捆装物品信息成功...");
                M2Share.MainOutMessage("正在加载任务地图信息...");
                nCode = M2Share.LocalDB.LoadMapQuest();
                if (nCode < 0)
                {
                    M2Share.MainOutMessage("加载任务地图信息失败！！！");
                    return;
                }
                M2Share.MainOutMessage("加载任务地图信息成功...");
                M2Share.MainOutMessage("正在加载任务说明信息...");
                nCode = M2Share.LocalDB.LoadQuestDiary();
                if (nCode < 0)
                {
                    M2Share.MainOutMessage("加载任务说明信息失败！！！");
                    return;
                }
                M2Share.MainOutMessage("加载任务说明信息成功...");
                if (LoadAbuseInformation(".\\!abuse.txt"))
                {
                    M2Share.MainOutMessage("加载文字过滤信息成功...");
                }
                M2Share.MainOutMessage("正在加载公告提示信息...");
                if (!M2Share.LoadLineNotice(Path.Combine(M2Share.g_Config.sNoticeDir, "LineNotice.txt")))
                {
                    M2Share.MainOutMessage("加载公告提示信息失败！！！");
                }
                M2Share.MainOutMessage("加载公告提示信息成功...");
                M2Share.LocalDB.LoadAdminList();
                M2Share.MainOutMessage("管理员列表加载成功...");
                M2Share.GuildManager.LoadGuildInfo();
                M2Share.MainOutMessage("行会列表加载成功...");
                M2Share.CastleManager.LoadCastleList();
                M2Share.MainOutMessage("城堡列表加载成功...");
                M2Share.CastleManager.Initialize();
                M2Share.MainOutMessage("城堡城初始完成...");
                if (M2Share.nServerIndex == 0)
                {
                    InterServerMsg.Instance.StartMsgServer();
                    M2Share.MainOutMessage("当前服务器运行主节点模式...");
                }
                else
                {
                    InterMsgClient.Instance.ConnectMsgServer();
                    M2Share.MainOutMessage($"当前运行从节点模式...[{M2Share.g_Config.sMsgSrvAddr}:{M2Share.g_Config.nMsgSrvPort}]");
                }
                StartEngine();
                M2Share.g_dwUsrRotCountTick = HUtil32.GetTickCount();
                base.Start();
            }
            catch (Exception e)
            {
                M2Share.MainOutMessage(e.StackTrace);
            }
        }

        private void StartEngine()
        {
            try
            {
                IdSrvClient.Instance.Initialize();
                M2Share.MainOutMessage("登录服务器连接初始化完成...");
                M2Share.g_MapManager.LoadMapDoor();
                M2Share.MainOutMessage("地图环境加载成功...");
                MakeStoneMines();
                M2Share.MainOutMessage("矿物数据初始成功...");
                M2Share.LocalDB.LoadMerchant();
                M2Share.MainOutMessage("交易NPC列表加载成功...");
                if (!M2Share.g_Config.boVentureServer)
                {
                    M2Share.LocalDB.LoadGuardList();
                    M2Share.MainOutMessage("守卫列表加载成功...");
                }
                M2Share.LocalDB.LoadNpcs();
                M2Share.MainOutMessage("管理NPC列表加载成功...");
                M2Share.LocalDB.LoadMakeItem();
                M2Share.MainOutMessage("炼制物品信息加载成功...");
                M2Share.LocalDB.LoadStartPoint();
                M2Share.MainOutMessage("回城点配置加载成功...");
                M2Share.MainOutMessage("正在初始安全区光圈...");
                M2Share.g_MapManager.MakeSafePkZone();
                M2Share.MainOutMessage("安全区光圈初始化成功...");
                M2Share.FrontEngine.Start();
                M2Share.MainOutMessage("人物数据引擎启动成功...");
                M2Share.UserEngine.Initialize();
                M2Share.MainOutMessage("游戏处理引擎初始化成功...");
                M2Share.MainOutMessage(M2Share.g_sVersion);
                M2Share.MainOutMessage(M2Share.g_sUpDateTime);
                M2Share.boStartReady = true;

                //制造一个人工智障
                M2Share.UserEngine.AddAILogon(new TAILogon()
                {
                    sCharName = "我是人工智障",
                    sMapName = "0",
                    sConfigFileName = "",
                    sHeroConfigFileName = "",
                    sFilePath = M2Share.g_Config.sEnvirDir,
                    sConfigListFileName = M2Share.g_Config.sAIConfigListFileName,
                    sHeroConfigListFileName = M2Share.g_Config.sHeroAIConfigListFileName,
                    nX = 285,
                    nY = 608
                });
            }
            catch (Exception ex)
            {
                M2Share.ErrorMessage(ex.Message);
            }
        }

        private void MakeStoneMines()
        {
            TEnvirnoment Envir;
            var mineMapList = M2Share.g_MapManager.GetMineMaps();
            for (var i = 0; i < mineMapList.Count; i++)
            {
                Envir = mineMapList[i];
                for (var nW = 0; nW < Envir.wWidth; nW++)
                {
                    for (var nH = 0; nH < Envir.wHeight; nH++)
                    {
                        new TStoneMineEvent(Envir, nW, nH, grobal2.ET_MINE);
                    }
                }
            }
        }

        private void InitializeServer()
        {
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
            M2Share.g_Config.sDBSocketRecvText = "";
            M2Share.g_Config.boDBSocketWorking = false;
            M2Share.g_Config.nLoadDBErrorCount = 0;
            M2Share.g_Config.nLoadDBCount = 0;
            M2Share.g_Config.nSaveDBCount = 0;
            M2Share.g_Config.nDBQueryID = 0;
            M2Share.g_Config.nItemNumber = 0;
            M2Share.g_Config.nItemNumberEx = int.MaxValue / 2;
            M2Share.boStartReady = false;
            M2Share.boFilterWord = true;
            M2Share.g_Config.nWinLotteryCount = 0;
            M2Share.g_Config.nNoWinLotteryCount = 0;
            M2Share.g_Config.nWinLotteryLevel1 = 0;
            M2Share.g_Config.nWinLotteryLevel2 = 0;
            M2Share.g_Config.nWinLotteryLevel3 = 0;
            M2Share.g_Config.nWinLotteryLevel4 = 0;
            M2Share.g_Config.nWinLotteryLevel5 = 0;
            M2Share.g_Config.nWinLotteryLevel6 = 0;
            M2Share.g_Config.GlobalVal=new int[20];
            M2Share.g_Config.GlobaDyMval = new short[100];
            M2Share.LoadConfig();
            M2Share.GroupServer = new GroupServer();
            M2Share.DataServer = new DataServer();
            M2Share.ObjectSystem = new ObjectSystem();
            M2Share.ScriptSystem = new ScriptSystem();
            M2Share.g_FindPath = new TFindPath();
            M2Share.CommandSystem = new CommandManager();
            M2Share.RunSocket = new TRunSocket();
            M2Share.LogStringList = new ArrayList();
            M2Share.LogonCostLogList = new ArrayList();
            M2Share.g_MapManager = new TMapManager();
            M2Share.ItemUnit = new ItemUnit();
            M2Share.MagicManager = new MagicManager();
            M2Share.NoticeManager = new TNoticeManager();
            M2Share.GuildManager = new GuildManager();
            M2Share.EventManager = new EventManager();
            M2Share.CastleManager = new CastleManager();
            M2Share.FrontEngine = new TFrontEngine();
            M2Share.UserEngine = new UserEngine();
            M2Share.RobotManage = new RobotManage();
            M2Share.g_MakeItemList = new Dictionary<string, IList<TMakeItem>>();
            M2Share.StartPointList = new List<TStartPoint>();
            M2Share.ServerTableList = new TRouteInfo[20];
            M2Share.g_DenySayMsgList = new ConcurrentDictionary<string, long>();
            M2Share.MiniMapList = new Dictionary<string, int>();
            M2Share.g_UnbindList = new Dictionary<int, string>();
            M2Share.LineNoticeList = new StringList();
            M2Share.QuestDiaryList = new List<TQDDinfo>();
            M2Share.AbuseTextList = new ArrayList();
            M2Share.g_MonSayMsgList = new Dictionary<string, IList<TMonSayMsg>>();
            M2Share.g_ChatLoggingList = new List<string>();
            M2Share.g_DisableMakeItemList = new List<string>();
            M2Share.g_EnableMakeItemList = new List<string>();
            M2Share.g_DisableSellOffList = new List<string>();
            M2Share.g_DisableMoveMapList = new List<string>();
            M2Share.g_DisableSendMsgList = new List<string>();
            M2Share.g_MonDropLimitLIst = new Dictionary<string, TMonDrop>();
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
            M2Share.LogMsgCriticalSection = new object();
            M2Share.ProcessMsgCriticalSection = new object();
            M2Share.ProcessHumanCriticalSection = new object();
            M2Share.g_Config.UserIDSection = new object();
            M2Share.UserDBSection = new object();
            M2Share.g_DynamicVarList = new List<TDynamicVar>();
            LoadServerTable();
            M2Share.dwRunDBTimeMax = HUtil32.GetTickCount();
            M2Share.CommandSystem.RegisterCommand();
        }

        private void LoadServerTable()
        {
            StringList LoadList;
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
            if (File.Exists(".\\!servertable.txt"))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(".\\!servertable.txt");
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i];
                    if (sLineText != "" && sLineText[0] != ';')
                    {
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sIdx, new string[] { " ", "\09" });
                        sGameGate = HUtil32.GetValidStr3(sLineText, ref sSelGateIPaddr, new string[] { " ", "\09" });
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
                            sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGateIPaddr, new string[] { " ", "\09" });
                            sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGatePort, new string[] { " ", "\09" });
                            M2Share.ServerTableList[nRouteIdx].sGameGateIP[nGateIdx] = sGameGateIPaddr.Trim();
                            M2Share.ServerTableList[nRouteIdx].nGameGatePort[nGateIdx] = HUtil32.Str_ToInt(sGameGatePort, 0);
                            nGateIdx++;
                        }
                        M2Share.ServerTableList[nRouteIdx].nGateCount = nGateIdx;
                        nRouteIdx++;
                        if (nRouteIdx > M2Share.ServerTableList.GetUpperBound(0))
                        {
                            break;
                        }
                    }
                }
            }
        }

        private bool LoadAbuseInformation(string FileName)
        {
            bool result;
            int i;
            result = false;
            if (File.Exists(FileName))
            {
                M2Share.AbuseTextList.Clear();
                //M2Share.AbuseTextList.LoadFromFile(FileName);
                i = 0;
                while (true)
                {
                    if (M2Share.AbuseTextList.Count <= i)
                    {
                        break;
                    }
                    //sText = M2Share.AbuseTextList[i].Trim();
                    //if (sText == "")
                    //{
                    //    M2Share.AbuseTextList.Remove(i);
                    //    continue;
                    //}
                    i++;
                }
                result = true;
            }
            return result;
        }

        private void StatisticTime()
        {
            var sc = new System.Text.StringBuilder();
            //sc.AppendLine($"({M2Share.UserEngine.MonsterCount}) {M2Share.UserEngine.OnlinePlayObject} / {M2Share.UserEngine.PlayObjectCount}  [{M2Share.UserEngine.LoadPlayCount}/{M2Share.UserEngine.m_PlayObjectFreeList.Count}]");
            //sc.AppendLine("Run:{0}/{1} Soc:{2}/{3} Usr:{4}/{5}",M2Share.nRunTimeMin, M2Share.nRunTimeMax, M2Share.g_nSockCountMin, M2Share.g_nSockCountMax,M2Share. g_nUsrTimeMin, M2Share.g_nUsrTimeMax);
            sc.AppendLine(
                $"Hum:{M2Share.g_nHumCountMin}/{M2Share.g_nHumCountMax} UsrRot:{M2Share.dwUsrRotCountMin}/{M2Share.dwUsrRotCountMax} Merch:{M2Share.UserEngine.dwProcessMerchantTimeMin}/{M2Share.UserEngine.dwProcessMerchantTimeMax} Npc:{M2Share.UserEngine.dwProcessNpcTimeMin}/{M2Share.UserEngine.dwProcessNpcTimeMax} ({M2Share.g_nProcessHumanLoopTime})");
            //sc.AppendLine("MonG:{0}/{1}/{2} MonP:{3}/{4}/{5} ObjRun:{6}/{7}", M2Share.g_nMonGenTime, M2Share.g_nMonGenTimeMin, M2Share.g_nMonGenTimeMax, M2Share.g_nMonProcTime, M2Share.g_nMonProcTimeMin, M2Share.g_nMonProcTimeMax, M2Share.g_nBaseObjTimeMin, M2Share.g_nBaseObjTimeMax);
        }
    }
}
