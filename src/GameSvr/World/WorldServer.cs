using GameSvr.Actor;
using GameSvr.Event.Events;
using GameSvr.Guild;
using GameSvr.Maps;
using GameSvr.Monster;
using GameSvr.Npc;
using GameSvr.Planes;
using GameSvr.Player;
using GameSvr.RobotPlay;
using GameSvr.Services;
using NLog;
using System.Collections;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;
using SystemModule.Packet.ServerPackets;

namespace GameSvr.World
{
    public partial class WorldServer
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private int ProcessMapDoorTick;
        private int ProcessMerchantTimeMax;
        private int ProcessMerchantTimeMin;
        private int ProcessMissionsTime;
        private int ProcessNpcTimeMax;
        private int ProcessNpcTimeMin;
        private int SendOnlineHumTime;
        private int ShowOnlineTick;
        private int ProcessLoadPlayTick;
        private int ProcHumIdx;
        private int ProcBotHubIdx;
        /// <summary>
        /// 交易NPC处理位置
        /// </summary>
        private int _merchantPosition;
        /// <summary>
        /// NPC处理位置
        /// </summary>
        private int NpcPosition;
        /// <summary>
        /// 处理人物开始索引（每次处理人物数限制）
        /// </summary>
        private int ProcessHumanLoopTime;
        /// <summary>
        /// 处理假人间隔
        /// </summary>
        public long RobotLogonTick;
        public readonly IList<TAdminInfo> AdminList;
        private readonly IList<TGoldChangeInfo> _mChangeHumanDbGoldList;
        private readonly IList<SwitchDataInfo> _mChangeServerList;
        private readonly IList<int> ListOfGateIdx;
        private readonly IList<int> ListOfSocket;
        /// <summary>
        /// 从DB读取人物数据
        /// </summary>
        protected readonly IList<UserOpenInfo> LoadPlayList;
        protected readonly object LoadPlaySection;
        public readonly IList<MagicEvent> MagicEventList;
        public IList<MagicInfo> MagicList;
        public readonly IList<Merchant> MerchantList;
        protected readonly IList<PlayObject> NewHumanList;
        protected readonly IList<PlayObject> PlayObjectFreeList;
        protected readonly Dictionary<string, ServerGruopInfo> OtherUserNameList;
        protected readonly IList<PlayObject> PlayObjectList;
        protected readonly IList<PlayObject> BotPlayObjectList;
        private readonly ArrayList _oldMagicList;
        public readonly IList<NormNpc> QuestNpcList;
        /// <summary>
        /// 物品列表
        /// </summary>
        public readonly IList<Items.StdItem> StdItemList;
        /// <summary>
        /// 怪物列表
        /// </summary>
        internal readonly Dictionary<string, MonsterInfo> MonsterList;
        /// <summary>
        /// 假人列表
        /// </summary>
        private readonly IList<RoBotLogon> RobotLogonList;

        public WorldServer()
        {
            LoadPlaySection = new object();
            LoadPlayList = new List<UserOpenInfo>();
            PlayObjectList = new List<PlayObject>();
            PlayObjectFreeList = new List<PlayObject>();
            _mChangeHumanDbGoldList = new List<TGoldChangeInfo>();
            ShowOnlineTick = HUtil32.GetTickCount();
            SendOnlineHumTime = HUtil32.GetTickCount();
            ProcessMapDoorTick = HUtil32.GetTickCount();
            ProcessMissionsTime = HUtil32.GetTickCount();
            ProcessLoadPlayTick = HUtil32.GetTickCount();
            ProcHumIdx = 0;
            ProcBotHubIdx = 0;
            ProcessHumanLoopTime = 0;
            _merchantPosition = 0;
            NpcPosition = 0;
            StdItemList = new List<Items.StdItem>();
            MonsterList = new Dictionary<string, MonsterInfo>(StringComparer.OrdinalIgnoreCase);
            MonGenList = new List<MonGenInfo>();
            MonGenInfoThreadMap = new Dictionary<int, IList<MonGenInfo>>();
            MonGenCountInfo = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            MonsterThreadMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            MagicList = new List<MagicInfo>();
            AdminList = new List<TAdminInfo>();
            MerchantList = new List<Merchant>();
            QuestNpcList = new List<NormNpc>();
            _mChangeServerList = new List<SwitchDataInfo>();
            MagicEventList = new List<MagicEvent>();
            ProcessMerchantTimeMin = 0;
            ProcessMerchantTimeMax = 0;
            ProcessNpcTimeMin = 0;
            ProcessNpcTimeMax = 0;
            NewHumanList = new List<PlayObject>();
            ListOfGateIdx = new List<int>();
            ListOfSocket = new List<int>();
            _oldMagicList = new ArrayList();
            OtherUserNameList = new Dictionary<string, ServerGruopInfo>(StringComparer.OrdinalIgnoreCase);
            RobotLogonList = new List<RoBotLogon>();
            BotPlayObjectList = new List<PlayObject>();
        }

        public int OnlinePlayObject => GetOnlineHumCount();
        public int PlayObjectCount => GetUserCount();
        public int LoadPlayCount => GetLoadPlayCount();

        public IEnumerable<PlayObject> PlayObjects => PlayObjectList;

        public void Execute()
        {
            PrcocessData();
            ProcessRobotPlayData();
        }

        public void Initialize()
        {
            _logger.Info("正在初始化NPC脚本...");
            MerchantInitialize();
            NpCinitialize();
            _logger.Info("初始化NPC脚本完成...");
        }

        private void PrcocessData()
        {
            try
            {
                for (var i = 0; i < MobThreads.Length; i++)
                {
                    var mobThread = MobThreads[i];
                    if (mobThread == null)
                    {
                        continue;
                    }
                    if (!mobThread.Stop) continue;
                    mobThread.EndTime = HUtil32.GetTickCount() + 10;
                    mobThread.Stop = false;
                }
                lock (_locker)
                {
                    Monitor.PulseAll(_locker);
                }

                ProcessHumans();
                //ProcessMonsters(MobThreads[0]);
                ProcessMerchants();
                ProcessNpcs();
                if ((HUtil32.GetTickCount() - ProcessMissionsTime) > 1000)
                {
                    ProcessMissionsTime = HUtil32.GetTickCount();
                    ProcessMissions();
                    ProcessEvents();
                }
                if ((HUtil32.GetTickCount() - ProcessMapDoorTick) > 500)
                {
                    ProcessMapDoorTick = HUtil32.GetTickCount();
                    ProcessMapDoor();
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.StackTrace);
            }
        }

        private int GetMonRace(string sMonName)
        {
            var result = -1;
            if (MonsterList.ContainsKey(sMonName))
            {
                return MonsterList[sMonName].Race;
            }
            return result;
        }

        private int GetMonsterThreadId(string sMonName)
        {
            if (MonsterThreadMap.TryGetValue(sMonName, out var threadId))
            {
                return threadId;
            }
            return -1;
        }

        private void MerchantInitialize()
        {
            for (var i = MerchantList.Count - 1; i >= 0; i--)
            {
                var merchant = MerchantList[i];
                merchant.Envir = M2Share.MapMgr.FindMap(merchant.MapName);
                if (merchant.Envir != null)
                {
                    merchant.OnEnvirnomentChanged();
                    merchant.Initialize();
                    if (merchant.AddtoMapSuccess && !merchant.m_boIsHide)
                    {
                        _logger.Warn("Merchant Initalize fail..." + merchant.ChrName + ' ' + merchant.MapName + '(' + merchant.CurrX + ':' + merchant.CurrY + ')');
                        MerchantList.RemoveAt(i);
                    }
                    else
                    {
                        merchant.LoadMerchantScript();
                        merchant.LoadNPCData();
                    }
                }
                else
                {
                    _logger.Error(merchant.ChrName + " - Merchant Initalize fail... (m.PEnvir=nil)");
                    MerchantList.RemoveAt(i);
                }
            }
        }

        private void NpCinitialize()
        {
            for (var i = QuestNpcList.Count - 1; i >= 0; i--)
            {
                var normNpc = QuestNpcList[i];
                normNpc.Envir = M2Share.MapMgr.FindMap(normNpc.MapName);
                if (normNpc.Envir != null)
                {
                    normNpc.OnEnvirnomentChanged();
                    normNpc.Initialize();
                    if (normNpc.AddtoMapSuccess && !normNpc.m_boIsHide)
                    {
                        _logger.Warn(normNpc.ChrName + " Npc Initalize fail... ");
                        QuestNpcList.RemoveAt(i);
                    }
                    else
                    {
                        normNpc.LoadNPCScript();
                    }
                }
                else
                {
                    _logger.Error(normNpc.ChrName + " Npc Initalize fail... (npc.PEnvir=nil) ");
                    QuestNpcList.RemoveAt(i);
                }
            }
        }

        private int GetLoadPlayCount()
        {
            return LoadPlayList.Count;
        }

        private int GetOnlineHumCount()
        {
            return PlayObjectList.Count + BotPlayObjectList.Count;
        }

        private int GetUserCount()
        {
            return PlayObjectList.Count + BotPlayObjectList.Count;
        }

        private bool ProcessHumansIsLogined(string sChrName)
        {
            var result = false;
            if (M2Share.FrontEngine.InSaveRcdList(sChrName))
            {
                result = true;
            }
            else
            {
                for (var i = 0; i < PlayObjectList.Count; i++)
                {
                    if (string.Compare(PlayObjectList[i].ChrName, sChrName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        private PlayObject ProcessHumans_MakeNewHuman(UserOpenInfo userOpenInfo)
        {
            PlayObject result = null;
            PlayObject playObject = null;
            const string sExceptionMsg = "[Exception] TUserEngine::MakeNewHuman";
            const string sChangeServerFail1 = "chg-server-fail-1 [{0}] -> [{1}] [{2}]";
            const string sChangeServerFail2 = "chg-server-fail-2 [{0}] -> [{1}] [{2}]";
            const string sChangeServerFail3 = "chg-server-fail-3 [{0}] -> [{1}] [{2}]";
            const string sChangeServerFail4 = "chg-server-fail-4 [{0}] -> [{1}] [{2}]";
            const string sErrorEnvirIsNil = "[Error] PlayObject.PEnvir = nil";
        ReGetMap:
            try
            {
                playObject = new PlayObject();
                SwitchDataInfo switchDataInfo;
                if (!M2Share.Config.VentureServer)
                {
                    userOpenInfo.sChrName = string.Empty;
                    userOpenInfo.LoadUser.nSessionID = 0;
                    switchDataInfo = GetSwitchData(userOpenInfo.sChrName, userOpenInfo.LoadUser.nSessionID);
                }
                else
                {
                    switchDataInfo = null;
                }
                if (switchDataInfo == null)
                {
                    GetHumData(playObject, ref userOpenInfo.HumanRcd);
                    playObject.Race = ActorRace.Play;
                    if (string.IsNullOrEmpty(playObject.HomeMap))
                    {
                        playObject.HomeMap = GetHomeInfo(playObject.Job, ref playObject.HomeX, ref playObject.HomeY);
                        playObject.MapName = playObject.HomeMap;
                        playObject.CurrX = GetRandHomeX(playObject);
                        playObject.CurrY = GetRandHomeY(playObject);
                        if (playObject.Abil.Level == 0)
                        {
                            var abil = playObject.Abil;
                            abil.Level = 1;
                            abil.AC = 0;
                            abil.MAC = 0;
                            abil.DC = (ushort)HUtil32.MakeLong(1, 2);
                            abil.MC = (ushort)HUtil32.MakeLong(1, 2);
                            abil.SC = (ushort)HUtil32.MakeLong(1, 2);
                            abil.MP = 15;
                            abil.HP = 15;
                            abil.MaxHP = 15;
                            abil.MaxMP = 15;
                            abil.Exp = 0;
                            abil.MaxExp = 100;
                            abil.Weight = 0;
                            abil.MaxWeight = 30;
                            playObject.m_boNewHuman = true;
                        }
                    }
                    Envirnoment envir = M2Share.MapMgr.GetMapInfo(M2Share.ServerIndex, playObject.MapName);
                    if (envir != null)
                    {
                        playObject.MapFileName = envir.MapFileName;
                        if (envir.Flag.boFight3Zone) // 是否在行会战争地图死亡
                        {
                            if (playObject.Abil.HP <= 0 && playObject.FightZoneDieCount < 3)
                            {
                                playObject.Abil.HP = playObject.Abil.MaxHP;
                                playObject.Abil.MP = playObject.Abil.MaxMP;
                                playObject.m_boDieInFight3Zone = true;
                            }
                            else
                            {
                                playObject.FightZoneDieCount = 0;
                            }
                        }
                    }
                    playObject.MyGuild = M2Share.GuildMgr.MemberOfGuild(playObject.ChrName);
                    var userCastle = M2Share.CastleMgr.InCastleWarArea(envir, playObject.CurrX, playObject.CurrY);
                    if (envir != null && userCastle != null && (userCastle.PalaceEnvir == envir || userCastle.UnderWar))
                    {
                        userCastle = M2Share.CastleMgr.IsCastleMember(playObject);
                        if (userCastle == null)
                        {
                            playObject.MapName = playObject.HomeMap;
                            playObject.CurrX = (short)(playObject.HomeX - 2 + M2Share.RandomNumber.Random(5));
                            playObject.CurrY = (short)(playObject.HomeY - 2 + M2Share.RandomNumber.Random(5));
                        }
                        else
                        {
                            if (userCastle.PalaceEnvir == envir)
                            {
                                playObject.MapName = userCastle.GetMapName();
                                playObject.CurrX = userCastle.GetHomeX();
                                playObject.CurrY = userCastle.GetHomeY();
                            }
                        }
                    }
                    if (M2Share.MapMgr.FindMap(playObject.MapName) == null) playObject.Abil.HP = 0;
                    if (playObject.Abil.HP <= 0)
                    {
                        playObject.ClearStatusTime();
                        if (playObject.PvpLevel() < 2)
                        {
                            userCastle = M2Share.CastleMgr.IsCastleMember(playObject);
                            if (userCastle != null && userCastle.UnderWar)
                            {
                                playObject.MapName = userCastle.HomeMap;
                                playObject.CurrX = userCastle.GetHomeX();
                                playObject.CurrY = userCastle.GetHomeY();
                            }
                            else
                            {
                                playObject.MapName = playObject.HomeMap;
                                playObject.CurrX = (short)(playObject.HomeX - 2 + M2Share.RandomNumber.Random(5));
                                playObject.CurrY = (short)(playObject.HomeY - 2 + M2Share.RandomNumber.Random(5));
                            }
                        }
                        else
                        {
                            playObject.MapName = M2Share.Config.RedDieHomeMap;// '3'
                            playObject.CurrX = (short)(M2Share.RandomNumber.Random(13) + M2Share.Config.RedDieHomeX);// 839
                            playObject.CurrY = (short)(M2Share.RandomNumber.Random(13) + M2Share.Config.RedDieHomeY);// 668
                        }
                        playObject.Abil.HP = 14;
                    }
                    playObject.AbilCopyToWAbil();
                    envir = M2Share.MapMgr.GetMapInfo(M2Share.ServerIndex, playObject.MapName);//切换其他服务器
                    if (envir == null)
                    {
                        playObject.m_nSessionID = userOpenInfo.LoadUser.nSessionID;
                        playObject.m_nSocket = userOpenInfo.LoadUser.nSocket;
                        playObject.m_nGateIdx = userOpenInfo.LoadUser.nGateIdx;
                        playObject.m_nGSocketIdx = userOpenInfo.LoadUser.nGSocketIdx;
                        playObject.WAbil = playObject.Abil;
                        playObject.m_nServerIndex = M2Share.MapMgr.GetMapOfServerIndex(playObject.MapName);
                        if (playObject.Abil.HP != 14)
                        {
                            _logger.Warn(string.Format(sChangeServerFail1, new object[] { M2Share.ServerIndex, playObject.m_nServerIndex, playObject.MapName }));
                        }
                        SendSwitchData(playObject, playObject.m_nServerIndex);
                        SendChangeServer(playObject, (byte)playObject.m_nServerIndex);
                        playObject = null;
                        return result;
                    }
                    playObject.MapFileName = envir.MapFileName;
                    var nC = 0;
                    while (true)
                    {
                        if (envir.CanWalk(playObject.CurrX, playObject.CurrY, true)) break;
                        playObject.CurrX = (short)(playObject.CurrX - 3 + M2Share.RandomNumber.Random(6));
                        playObject.CurrY = (short)(playObject.CurrY - 3 + M2Share.RandomNumber.Random(6));
                        nC++;
                        if (nC >= 5) break;
                    }
                    if (!envir.CanWalk(playObject.CurrX, playObject.CurrY, true))
                    {
                        _logger.Warn(string.Format(sChangeServerFail2,
                            new object[] { M2Share.ServerIndex, playObject.m_nServerIndex, playObject.MapName }));
                        playObject.MapName = M2Share.Config.HomeMap;
                        envir = M2Share.MapMgr.FindMap(M2Share.Config.HomeMap);
                        playObject.CurrX = M2Share.Config.HomeX;
                        playObject.CurrY = M2Share.Config.HomeY;
                    }
                    playObject.Envir = envir;
                    playObject.OnEnvirnomentChanged();
                    if (playObject.Envir == null)
                    {
                        _logger.Error(sErrorEnvirIsNil);
                        goto ReGetMap;
                    }
                    else
                        playObject.m_boReadyRun = false;
                    playObject.MapFileName = envir.MapFileName;
                }
                else
                {
                    GetHumData(playObject, ref userOpenInfo.HumanRcd);
                    playObject.MapName = switchDataInfo.sMap;
                    playObject.CurrX = switchDataInfo.wX;
                    playObject.CurrY = switchDataInfo.wY;
                    playObject.Abil = switchDataInfo.Abil;
                    playObject.Abil = switchDataInfo.Abil;
                    LoadSwitchData(switchDataInfo, ref playObject);
                    DelSwitchData(switchDataInfo);
                    Envirnoment envir = M2Share.MapMgr.GetMapInfo(M2Share.ServerIndex, playObject.MapName);
                    if (envir != null)
                    {
                        _logger.Warn(string.Format(sChangeServerFail3, new object[] { M2Share.ServerIndex, playObject.m_nServerIndex, playObject.MapName }));
                        playObject.MapName = M2Share.Config.HomeMap;
                        envir = M2Share.MapMgr.FindMap(M2Share.Config.HomeMap);
                        playObject.CurrX = M2Share.Config.HomeX;
                        playObject.CurrY = M2Share.Config.HomeY;
                    }
                    else
                    {
                        if (!envir.CanWalk(playObject.CurrX, playObject.CurrY, true))
                        {
                            _logger.Warn(string.Format(sChangeServerFail4, new object[] { M2Share.ServerIndex, playObject.m_nServerIndex, playObject.MapName }));
                            playObject.MapName = M2Share.Config.HomeMap;
                            envir = M2Share.MapMgr.FindMap(M2Share.Config.HomeMap);
                            playObject.CurrX = M2Share.Config.HomeX;
                            playObject.CurrY = M2Share.Config.HomeY;
                        }
                        playObject.AbilCopyToWAbil();
                        playObject.Envir = envir;
                        playObject.OnEnvirnomentChanged();
                        if (playObject.Envir == null)
                        {
                            _logger.Error(sErrorEnvirIsNil);
                            goto ReGetMap;
                        }
                        else
                        {
                            playObject.m_boReadyRun = false;
                            playObject.m_boLoginNoticeOK = true;
                            playObject.bo6AB = true;
                        }
                    }
                }
                playObject.m_sUserID = userOpenInfo.LoadUser.sAccount;
                playObject.m_sIPaddr = userOpenInfo.LoadUser.sIPaddr;
                playObject.m_sIPLocal = M2Share.GetIPLocal(playObject.m_sIPaddr);
                playObject.m_nSocket = userOpenInfo.LoadUser.nSocket;
                playObject.m_nGSocketIdx = userOpenInfo.LoadUser.nGSocketIdx;
                playObject.m_nGateIdx = userOpenInfo.LoadUser.nGateIdx;
                playObject.m_nSessionID = userOpenInfo.LoadUser.nSessionID;
                playObject.m_nPayMent = userOpenInfo.LoadUser.nPayMent;
                playObject.m_nPayMode = userOpenInfo.LoadUser.nPayMode;
                playObject.m_dwLoadTick = userOpenInfo.LoadUser.dwNewUserTick;
                //PlayObject.m_nSoftVersionDateEx = M2Share.GetExVersionNO(UserOpenInfo.LoadUser.nSoftVersionDate, ref PlayObject.m_nSoftVersionDate);
                playObject.m_nSoftVersionDate = userOpenInfo.LoadUser.nSoftVersionDate;
                playObject.m_nSoftVersionDateEx = userOpenInfo.LoadUser.nSoftVersionDate;//M2Share.GetExVersionNO(UserOpenInfo.LoadUser.nSoftVersionDate, ref PlayObject.m_nSoftVersionDate);
                result = playObject;
            }
            catch (Exception ex)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(ex.StackTrace);
            }
            return result;
        }

        private void ProcessHumans()
        {
            const string sExceptionMsg1 = "[Exception] TUserEngine::ProcessHumans -> Ready, Save, Load...";
            const string sExceptionMsg3 = "[Exception] TUserEngine::ProcessHumans ClosePlayer.Delete";
            var dwCheckTime = HUtil32.GetTickCount();
            PlayObject playObject;
            if ((HUtil32.GetTickCount() - ProcessLoadPlayTick) > 200)
            {
                ProcessLoadPlayTick = HUtil32.GetTickCount();
                try
                {
                    HUtil32.EnterCriticalSection(LoadPlaySection);
                    try
                    {
                        for (var i = 0; i < LoadPlayList.Count; i++)
                        {
                            UserOpenInfo userOpenInfo;
                            if (!M2Share.FrontEngine.IsFull() && !ProcessHumansIsLogined(LoadPlayList[i].sChrName))
                            {
                                userOpenInfo = LoadPlayList[i];
                                playObject = ProcessHumans_MakeNewHuman(userOpenInfo);
                                if (playObject != null)
                                {
                                    if (playObject.IsRobot)
                                    {
                                        BotPlayObjectList.Add(playObject);
                                    }
                                    else
                                    {
                                        PlayObjectList.Add(playObject);
                                    }
                                    NewHumanList.Add(playObject);
                                    SendServerGroupMsg(Grobal2.ISM_USERLOGON, M2Share.ServerIndex, playObject.ChrName);
                                }
                            }
                            else
                            {
                                KickOnlineUser(LoadPlayList[i].sChrName);
                                userOpenInfo = LoadPlayList[i];
                                ListOfGateIdx.Add(userOpenInfo.LoadUser.nGateIdx);
                                ListOfSocket.Add(userOpenInfo.LoadUser.nSocket);
                            }
                            LoadPlayList[i] = null;
                        }
                        LoadPlayList.Clear();
                        for (var i = 0; i < _mChangeHumanDbGoldList.Count; i++)
                        {
                            var goldChangeInfo = _mChangeHumanDbGoldList[i];
                            playObject = GetPlayObject(goldChangeInfo.sGameMasterName);
                            if (playObject != null)
                                playObject.GoldChange(goldChangeInfo.sGetGoldUser, goldChangeInfo.nGold);
                            goldChangeInfo = null;
                        }
                        _mChangeHumanDbGoldList.Clear();
                    }
                    finally
                    {
                        HUtil32.LeaveCriticalSection(LoadPlaySection);
                    }
                    for (var i = 0; i < NewHumanList.Count; i++)
                    {
                        playObject = NewHumanList[i];
                        M2Share.GateMgr.SetGateUserList(playObject.m_nGateIdx, playObject.m_nSocket, playObject);
                    }
                    NewHumanList.Clear();
                    for (var i = 0; i < ListOfGateIdx.Count; i++)
                    {
                        M2Share.GateMgr.CloseUser(ListOfGateIdx[i], ListOfSocket[i]);
                    }
                    ListOfGateIdx.Clear();
                    ListOfSocket.Clear();
                }
                catch (Exception e)
                {
                    _logger.Error(sExceptionMsg1);
                    _logger.Error(e.Message);
                }
            }

            //人工智障开始登陆
            if (RobotLogonList.Count > 0)
            {
                if (HUtil32.GetTickCount() - RobotLogonTick > 1000)
                {
                    RobotLogonTick = HUtil32.GetTickCount();
                    if (RobotLogonList.Count > 0)
                    {
                        var roBot = RobotLogonList[0];
                        RegenAiObject(roBot);
                        RobotLogonList.RemoveAt(0);
                    }
                }
            }

            try
            {
                for (var i = 0; i < PlayObjectFreeList.Count; i++)
                {
                    playObject = PlayObjectFreeList[i];
                    if ((HUtil32.GetTickCount() - playObject.GhostTick) > M2Share.Config.HumanFreeDelayTime)// 5 * 60 * 1000
                    {
                        PlayObjectFreeList[i] = null;
                        PlayObjectFreeList.RemoveAt(i);
                        break;
                    }
                    if (playObject.m_boSwitchData && playObject.m_boRcdSaved)
                    {
                        if (SendSwitchData(playObject, playObject.m_nServerIndex) || playObject.m_nWriteChgDataErrCount > 20)
                        {
                            playObject.m_boSwitchData = false;
                            playObject.m_boSwitchDataOK = true;
                            playObject.m_boSwitchDataSended = true;
                            playObject.m_dwChgDataWritedTick = HUtil32.GetTickCount();
                        }
                        else
                        {
                            playObject.m_nWriteChgDataErrCount++;
                        }
                    }
                    if (playObject.m_boSwitchDataSended && HUtil32.GetTickCount() - playObject.m_dwChgDataWritedTick > 100)
                    {
                        playObject.m_boSwitchDataSended = false;
                        SendChangeServer(playObject, (byte)playObject.m_nServerIndex);
                    }
                }
            }
            catch
            {
                _logger.Error(sExceptionMsg3);
            }
            ProcessPlayObjectData();
            ProcessHumanLoopTime++;
            M2Share.g_nProcessHumanLoopTime = ProcessHumanLoopTime;
            if (ProcHumIdx == 0)
            {
                ProcessHumanLoopTime = 0;
                M2Share.g_nProcessHumanLoopTime = ProcessHumanLoopTime;
                var dwUsrRotTime = HUtil32.GetTickCount() - M2Share.g_dwUsrRotCountTick;
                M2Share.dwUsrRotCountMin = dwUsrRotTime;
                M2Share.g_dwUsrRotCountTick = HUtil32.GetTickCount();
                if (M2Share.dwUsrRotCountMax < dwUsrRotTime) M2Share.dwUsrRotCountMax = dwUsrRotTime;
            }
            M2Share.g_nHumCountMin = HUtil32.GetTickCount() - dwCheckTime;
            if (M2Share.g_nHumCountMax < M2Share.g_nHumCountMin) M2Share.g_nHumCountMax = M2Share.g_nHumCountMin;
        }

        private void ProcessRobotPlayData()
        {
            const string sExceptionMsg = "[Exception] TUserEngine::ProcessRobotPlayData";
            try
            {
                var dwCurTick = HUtil32.GetTickCount();
                var nIdx = ProcBotHubIdx;
                var boCheckTimeLimit = false;
                var dwCheckTime = HUtil32.GetTickCount();
                while (true)
                {
                    if (BotPlayObjectList.Count <= nIdx) break;
                    var playObject = BotPlayObjectList[nIdx];
                    if (dwCurTick - playObject.RunTick > playObject.RunTime)
                    {
                        playObject.RunTick = dwCurTick;
                        if (!playObject.Ghost)
                        {
                            if (!playObject.m_boLoginNoticeOK)
                            {
                                playObject.RunNotice();
                            }
                            else
                            {
                                if (!playObject.m_boReadyRun)
                                {
                                    playObject.m_boReadyRun = true;
                                    playObject.UserLogon();
                                }
                                else
                                {
                                    if ((HUtil32.GetTickCount() - playObject.SearchTick) > playObject.SearchTime)
                                    {
                                        playObject.SearchTick = HUtil32.GetTickCount();
                                        playObject.SearchViewRange();
                                        playObject.GameTimeChanged();
                                    }
                                    playObject.Run();
                                }
                            }
                        }
                        else
                        {
                            BotPlayObjectList.Remove(playObject);
                            playObject.Disappear();
                            AddToHumanFreeList(playObject);
                            playObject.DealCancelA();
                            SaveHumanRcd(playObject);
                            M2Share.GateMgr.CloseUser(playObject.m_nGateIdx, playObject.m_nSocket);
                            SendServerGroupMsg(Grobal2.SS_202, M2Share.ServerIndex, playObject.ChrName);
                            continue;
                        }
                    }
                    nIdx++;
                    if ((HUtil32.GetTickCount() - dwCheckTime) > M2Share.HumLimit)
                    {
                        boCheckTimeLimit = true;
                        ProcBotHubIdx = nIdx;
                        break;
                    }
                }
                if (!boCheckTimeLimit) ProcBotHubIdx = 0;
            }
            catch (Exception ex)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(ex.StackTrace);
            }
        }

        private void ProcessPlayObjectData()
        {
            try
            {
                var dwCurTick = HUtil32.GetTickCount();
                var nIdx = ProcHumIdx;
                var boCheckTimeLimit = false;
                var dwCheckTime = HUtil32.GetTickCount();
                while (true)
                {
                    if (PlayObjectList.Count <= nIdx) break;
                    var playObject = PlayObjectList[nIdx];
                    if (playObject == null)
                    {
                        continue;
                    }
                    if ((dwCurTick - playObject.RunTick) > playObject.RunTime)
                    {
                        playObject.RunTick = dwCurTick;
                        if (!playObject.Ghost)
                        {
                            if (!playObject.m_boLoginNoticeOK)
                            {
                                playObject.RunNotice();
                            }
                            else
                            {
                                if (!playObject.m_boReadyRun)
                                {
                                    playObject.m_boReadyRun = true;
                                    playObject.UserLogon();
                                }
                                else
                                {
                                    if ((HUtil32.GetTickCount() - playObject.SearchTick) > playObject.SearchTime)
                                    {
                                        playObject.SearchTick = HUtil32.GetTickCount();
                                        playObject.SearchViewRange();//搜索对像
                                        playObject.GameTimeChanged();//游戏时间改变
                                    }
                                    if ((HUtil32.GetTickCount() - playObject.m_dwShowLineNoticeTick) > M2Share.Config.ShowLineNoticeTime)
                                    {
                                        playObject.m_dwShowLineNoticeTick = HUtil32.GetTickCount();
                                        if (M2Share.LineNoticeList.Count > playObject.m_nShowLineNoticeIdx)
                                        {
                                            var lineNoticeMsg = M2Share.g_ManageNPC.GetLineVariableText(playObject, M2Share.LineNoticeList[playObject.m_nShowLineNoticeIdx]);
                                            switch (lineNoticeMsg[0])
                                            {
                                                case 'R':
                                                    playObject.SysMsg(lineNoticeMsg.Substring(1, lineNoticeMsg.Length - 1), MsgColor.Red, MsgType.Notice);
                                                    break;
                                                case 'G':
                                                    playObject.SysMsg(lineNoticeMsg.Substring(1, lineNoticeMsg.Length - 1), MsgColor.Green, MsgType.Notice);
                                                    break;
                                                case 'B':
                                                    playObject.SysMsg(lineNoticeMsg.Substring(1, lineNoticeMsg.Length - 1), MsgColor.Blue, MsgType.Notice);
                                                    break;
                                                default:
                                                    playObject.SysMsg(lineNoticeMsg, (MsgColor)M2Share.Config.LineNoticeColor, MsgType.Notice);
                                                    break;
                                            }
                                        }
                                        playObject.m_nShowLineNoticeIdx++;
                                        if (M2Share.LineNoticeList.Count <= playObject.m_nShowLineNoticeIdx)
                                        {
                                            playObject.m_nShowLineNoticeIdx = 0;
                                        }
                                    }
                                    playObject.Run();
                                    if (!M2Share.FrontEngine.IsFull() && (HUtil32.GetTickCount() - playObject.m_dwSaveRcdTick) > M2Share.Config.SaveHumanRcdTime)
                                    {
                                        playObject.m_dwSaveRcdTick = HUtil32.GetTickCount();
                                        playObject.DealCancelA();
                                        SaveHumanRcd(playObject);
                                    }
                                }
                            }
                        }
                        else
                        {
                            PlayObjectList.Remove(playObject);
                            playObject.Disappear();
                            AddToHumanFreeList(playObject);
                            playObject.DealCancelA();
                            SaveHumanRcd(playObject);
                            M2Share.GateMgr.CloseUser(playObject.m_nGateIdx, playObject.m_nSocket);
                            SendServerGroupMsg(Grobal2.ISM_USERLOGOUT, M2Share.ServerIndex, playObject.ChrName);
                            continue;
                        }
                    }
                    nIdx++;
                    if ((HUtil32.GetTickCount() - dwCheckTime) > M2Share.HumLimit)
                    {
                        boCheckTimeLimit = true;
                        ProcHumIdx = nIdx;
                        break;
                    }
                }
                if (!boCheckTimeLimit) ProcHumIdx = 0;
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] TUserEngine::ProcessHumans");
                _logger.Error(ex.StackTrace);
            }
        }

        private void ProcessMerchants()
        {
            var boProcessLimit = false;
            const string sExceptionMsg = "[Exception] TUserEngine::ProcessMerchants";
            var dwRunTick = HUtil32.GetTickCount();
            try
            {
                var dwCurrTick = HUtil32.GetTickCount();
                for (var i = _merchantPosition; i < MerchantList.Count; i++)
                {
                    var merchantNpc = MerchantList[i];
                    if (!merchantNpc.Ghost)
                    {
                        if ((dwCurrTick - merchantNpc.RunTick) > merchantNpc.RunTime)
                        {
                            //if ((HUtil32.GetTickCount() - merchantNpc.SearchTick) > merchantNpc.SearchTime)
                            //{
                            //    merchantNpc.SearchTick = HUtil32.GetTickCount();
                            //   // merchantNpc.SearchViewRange();
                            //}
                            if ((HUtil32.GetTickCount() - merchantNpc.RunTick) > merchantNpc.RunTime)
                            {
                                merchantNpc.RunTick = dwCurrTick;
                                merchantNpc.Run();
                            }
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - merchantNpc.GhostTick) > 60 * 1000)
                        {
                            merchantNpc = null;
                            MerchantList.RemoveAt(i);
                            break;
                        }
                    }
                    if ((HUtil32.GetTickCount() - dwRunTick) > M2Share.NpcLimit)
                    {
                        _merchantPosition = i;
                        boProcessLimit = true;
                        break;
                    }
                }
                if (!boProcessLimit)
                {
                    _merchantPosition = 0;
                }
            }
            catch
            {
                _logger.Error(sExceptionMsg);
            }
            ProcessMerchantTimeMin = HUtil32.GetTickCount() - dwRunTick;
            if (ProcessMerchantTimeMin > ProcessMerchantTimeMax)
            {
                ProcessMerchantTimeMax = ProcessMerchantTimeMin;
            }
            if (ProcessNpcTimeMin > ProcessNpcTimeMax)
            {
                ProcessNpcTimeMax = ProcessNpcTimeMin;
            }
        }

        private void ProcessMissions()
        {

        }

        private void ProcessNpcs()
        {
            var dwRunTick = HUtil32.GetTickCount();
            var boProcessLimit = false;
            try
            {
                var dwCurrTick = HUtil32.GetTickCount();
                for (var i = NpcPosition; i < QuestNpcList.Count; i++)
                {
                    NormNpc npc = QuestNpcList[i];
                    if (!npc.Ghost)
                    {
                        if ((dwCurrTick - npc.RunTick) > npc.RunTime)
                        {
                            //if ((HUtil32.GetTickCount() - npc.SearchTick) > npc.SearchTime)
                            //{
                            //    npc.SearchTick = HUtil32.GetTickCount();
                            //    npc.SearchViewRange();
                            //}
                            if ((dwCurrTick - npc.RunTick) > npc.RunTime)
                            {
                                npc.RunTick = dwCurrTick;
                                npc.Run();
                            }
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - npc.GhostTick) > 60 * 1000)
                        {
                            QuestNpcList.RemoveAt(i);
                            break;
                        }
                    }
                    if ((HUtil32.GetTickCount() - dwRunTick) > M2Share.NpcLimit)
                    {
                        NpcPosition = i;
                        boProcessLimit = true;
                        break;
                    }
                }
                if (!boProcessLimit) NpcPosition = 0;
            }
            catch
            {
                _logger.Error("[Exceptioin] TUserEngine.ProcessNpcs");
            }
            ProcessNpcTimeMin = HUtil32.GetTickCount() - dwRunTick;
            if (ProcessNpcTimeMin > ProcessNpcTimeMax) ProcessNpcTimeMax = ProcessNpcTimeMin;
        }

        public void Run()
        {
            const string sExceptionMsg = "[Exception] TUserEngine::Run";
            try
            {
                if ((HUtil32.GetTickCount() - ShowOnlineTick) > M2Share.Config.ConsoleShowUserCountTime)
                {
                    ShowOnlineTick = HUtil32.GetTickCount();
                    M2Share.NoticeMgr.LoadingNotice();
                    _logger.Info("在线数: " + PlayObjectCount);
                    M2Share.CastleMgr.Save();
                }
                if ((HUtil32.GetTickCount() - SendOnlineHumTime) > 10000)
                {
                    SendOnlineHumTime = HUtil32.GetTickCount();
                    IdSrvClient.Instance.SendOnlineHumCountMsg(OnlinePlayObject);
                }
            }
            catch (Exception e)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(e.Message);
            }
        }

        public Items.StdItem GetStdItem(ushort nItemIdx)
        {
            Items.StdItem result = null;
            nItemIdx -= 1;
            if (nItemIdx >= 0 && StdItemList.Count > nItemIdx)
            {
                result = StdItemList[nItemIdx];
                if (result.Name == "") result = null;
            }
            return result;
        }

        public Items.StdItem GetStdItem(string sItemName)
        {
            Items.StdItem result = null;
            if (string.IsNullOrEmpty(sItemName)) return result;
            for (var i = 0; i < StdItemList.Count; i++)
            {
                Items.StdItem stdItem = StdItemList[i];
                if (string.Compare(stdItem.Name, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = stdItem;
                    break;
                }
            }
            return result;
        }

        public int GetStdItemWeight(int nItemIdx)
        {
            int result = 0;
            nItemIdx -= 1;
            if (nItemIdx >= 0 && StdItemList.Count > nItemIdx)
            {
                result = StdItemList[nItemIdx].Weight;
            }
            return result;
        }

        public string GetStdItemName(int nItemIdx)
        {
            var result = "";
            nItemIdx -= 1;
            if (nItemIdx >= 0 && StdItemList.Count > nItemIdx)
            {
                result = StdItemList[nItemIdx].Name;
            }
            return result;
        }

        public bool FindOtherServerUser(string sName, ref int nServerIndex)
        {
            if (OtherUserNameList.TryGetValue(sName, out var groupServer))
            {
                nServerIndex = groupServer.nServerIdx;
                M2Share.Log.Info($"玩家在[{nServerIndex}]服务器上.");
                return true;
            }
            return false;
        }

        public void CryCry(short wIdent, Envirnoment pMap, int nX, int nY, int nWide, byte btFColor, byte btBColor, string sMsg)
        {
            PlayObject playObject;
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                playObject = PlayObjectList[i];
                if (!playObject.Ghost && playObject.Envir == pMap && playObject.BanShout &&
                    Math.Abs(playObject.CurrX - nX) < nWide && Math.Abs(playObject.CurrY - nY) < nWide)
                    playObject.SendMsg(null, wIdent, 0, btFColor, btBColor, 0, sMsg);
            }
        }

        public bool CopyToUserItemFromName(string sItemName, ref UserItem item)
        {
            if (string.IsNullOrEmpty(sItemName)) return false;
            for (var i = 0; i < StdItemList.Count; i++)
            {
                var stdItem = StdItemList[i];
                if (!stdItem.Name.Equals(sItemName, StringComparison.OrdinalIgnoreCase)) continue;
                if (item == null) item = new UserItem();
                item.Index = (ushort)(i + 1);
                item.MakeIndex = M2Share.GetItemNumber();
                item.Dura = stdItem.DuraMax;
                item.DuraMax = stdItem.DuraMax;
                return true;
            }
            return false;
        }

        public void ProcessUserMessage(PlayObject playObject, ClientPacket defMsg, string buff)
        {
            var sMsg = string.Empty;
            if (playObject.OffLineFlag) return;
            if (!string.IsNullOrEmpty(buff)) sMsg = buff;
            switch (defMsg.Ident)
            {
                case Grobal2.CM_SPELL:
                    if (M2Share.Config.SpellSendUpdateMsg) // 使用UpdateMsg 可以防止消息队列里有多个操作
                    {
                        playObject.SendUpdateMsg(playObject, defMsg.Ident, defMsg.Tag, HUtil32.LoWord(defMsg.Recog), HUtil32.HiWord(defMsg.Recog), HUtil32.MakeLong(defMsg.Param, defMsg.Series), "");
                    }
                    else
                    {
                        playObject.SendMsg(playObject, defMsg.Ident, defMsg.Tag, HUtil32.LoWord(defMsg.Recog), HUtil32.HiWord(defMsg.Recog), HUtil32.MakeLong(defMsg.Param, defMsg.Series), "");
                    }
                    break;
                case Grobal2.CM_QUERYUSERNAME:
                    playObject.SendMsg(playObject, defMsg.Ident, 0, defMsg.Recog, defMsg.Param, defMsg.Tag, "");
                    break;
                case Grobal2.CM_DROPITEM:
                case Grobal2.CM_TAKEONITEM:
                case Grobal2.CM_TAKEOFFITEM:
                case Grobal2.CM_1005:
                case Grobal2.CM_MERCHANTDLGSELECT:
                case Grobal2.CM_MERCHANTQUERYSELLPRICE:
                case Grobal2.CM_USERSELLITEM:
                case Grobal2.CM_USERBUYITEM:
                case Grobal2.CM_USERGETDETAILITEM:
                case Grobal2.CM_CREATEGROUP:
                case Grobal2.CM_ADDGROUPMEMBER:
                case Grobal2.CM_DELGROUPMEMBER:
                case Grobal2.CM_USERREPAIRITEM:
                case Grobal2.CM_MERCHANTQUERYREPAIRCOST:
                case Grobal2.CM_DEALTRY:
                case Grobal2.CM_DEALADDITEM:
                case Grobal2.CM_DEALDELITEM:
                case Grobal2.CM_USERSTORAGEITEM:
                case Grobal2.CM_USERTAKEBACKSTORAGEITEM:
                case Grobal2.CM_USERMAKEDRUGITEM:
                case Grobal2.CM_GUILDADDMEMBER:
                case Grobal2.CM_GUILDDELMEMBER:
                case Grobal2.CM_GUILDUPDATENOTICE:
                case Grobal2.CM_GUILDUPDATERANKINFO:
                    playObject.SendMsg(playObject, defMsg.Ident, defMsg.Series, defMsg.Recog, defMsg.Param, defMsg.Tag,
                        sMsg);
                    break;
                case Grobal2.CM_PASSWORD:
                case Grobal2.CM_CHGPASSWORD:
                case Grobal2.CM_SETPASSWORD:
                    playObject.SendMsg(playObject, defMsg.Ident, defMsg.Param, defMsg.Recog, defMsg.Series, defMsg.Tag,
                        sMsg);
                    break;
                case Grobal2.CM_ADJUST_BONUS:
                    playObject.SendMsg(playObject, defMsg.Ident, defMsg.Series, defMsg.Recog, defMsg.Param, defMsg.Tag,
                        sMsg);
                    break;
                case Grobal2.CM_HORSERUN:
                case Grobal2.CM_TURN:
                case Grobal2.CM_WALK:
                case Grobal2.CM_SITDOWN:
                case Grobal2.CM_RUN:
                case Grobal2.CM_HIT:
                case Grobal2.CM_HEAVYHIT:
                case Grobal2.CM_BIGHIT:
                case Grobal2.CM_POWERHIT:
                case Grobal2.CM_LONGHIT:
                case Grobal2.CM_CRSHIT:
                case Grobal2.CM_TWINHIT:
                case Grobal2.CM_WIDEHIT:
                case Grobal2.CM_FIREHIT:
                    if (M2Share.Config.ActionSendActionMsg) // 使用UpdateMsg 可以防止消息队列里有多个操作
                    {
                        playObject.SendActionMsg(playObject, defMsg.Ident, defMsg.Tag, HUtil32.LoWord(defMsg.Recog),
                            HUtil32.HiWord(defMsg.Recog), 0, "");
                    }
                    else
                    {
                        playObject.SendMsg(playObject, defMsg.Ident, defMsg.Tag, HUtil32.LoWord(defMsg.Recog),
                            HUtil32.HiWord(defMsg.Recog), 0, "");
                    }
                    break;
                case Grobal2.CM_SAY:
                    playObject.SendMsg(playObject, Grobal2.CM_SAY, 0, 0, 0, 0, sMsg);
                    break;
                default:
                    playObject.SendMsg(playObject, defMsg.Ident, defMsg.Series, defMsg.Recog, defMsg.Param, defMsg.Tag,
                        sMsg);
                    break;
            }
            if (!playObject.m_boReadyRun) return;
            switch (defMsg.Ident)
            {
                case Grobal2.CM_TURN:
                case Grobal2.CM_WALK:
                case Grobal2.CM_SITDOWN:
                case Grobal2.CM_RUN:
                case Grobal2.CM_HIT:
                case Grobal2.CM_HEAVYHIT:
                case Grobal2.CM_BIGHIT:
                case Grobal2.CM_POWERHIT:
                case Grobal2.CM_LONGHIT:
                case Grobal2.CM_WIDEHIT:
                case Grobal2.CM_FIREHIT:
                case Grobal2.CM_CRSHIT:
                case Grobal2.CM_TWINHIT:
                    playObject.RunTick -= 100;
                    break;
            }
        }

        public void SendServerGroupMsg(int nCode, int nServerIdx, string sMsg)
        {
            if (M2Share.ServerIndex == 0)
            {
                PlanesServer.Instance.SendServerSocket(nCode + "/" + nServerIdx + "/" + sMsg);
            }
            else
            {
                PlanesClient.Instance.SendSocket(nCode + "/" + nServerIdx + "/" + sMsg);
            }
        }

        public void GetIsmChangeServerReceive(string flName)
        {
            for (var i = 0; i < PlayObjectFreeList.Count; i++)
            {
                PlayObject hum = PlayObjectFreeList[i];
                if (hum.m_sSwitchDataTempFile == flName)
                {
                    hum.m_boSwitchDataOK = true;
                    break;
                }
            }
        }

        public void OtherServerUserLogon(int sNum, string uname)
        {
            var name = string.Empty;
            var apmode = HUtil32.GetValidStr3(uname, ref name, ":");
            OtherUserNameList.Remove(name);
            OtherUserNameList.Add(name, new ServerGruopInfo()
            {
                nServerIdx = sNum,
                sChrName = uname
            });
        }

        public void OtherServerUserLogout(int sNum, string uname)
        {
            var name = string.Empty;
            var apmode = HUtil32.GetValidStr3(uname, ref name, ":");
            OtherUserNameList.Remove(name);
            // for (var i = m_OtherUserNameList.Count - 1; i >= 0; i--)
            // {
            //     if (string.Compare(m_OtherUserNameList[i].sChrName, Name, StringComparison.OrdinalIgnoreCase) == 0 && m_OtherUserNameList[i].nServerIdx == sNum)
            //     {
            //         m_OtherUserNameList.RemoveAt(i);
            //         break;
            //     }
            // }
        }

        public PlayObject GetPlayObject(string sName)
        {
            PlayObject result = null;
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                if (string.Compare(PlayObjectList[i].ChrName, sName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    PlayObject playObject = PlayObjectList[i];
                    if (!playObject.Ghost)
                    {
                        if (!(playObject.m_boPasswordLocked && playObject.ObMode && playObject.AdminMode))
                        {
                            result = playObject;
                        }
                    }
                    break;
                }
            }
            return result;
        }

        public void KickPlayObjectEx(string sName)
        {
            PlayObject playObject;
            HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
            try
            {
                for (var i = 0; i < PlayObjectList.Count; i++)
                {
                    if (string.Compare(PlayObjectList[i].ChrName, sName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        playObject = PlayObjectList[i];
                        playObject.m_boEmergencyClose = true;
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessHumanCriticalSection);
            }
        }

        public PlayObject GetPlayObjectEx(string sName)
        {
            PlayObject result = null;
            HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
            try
            {
                for (var i = 0; i < PlayObjectList.Count; i++)
                {
                    if (string.Compare(PlayObjectList[i].ChrName, sName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = PlayObjectList[i];
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessHumanCriticalSection);
            }
            return result;
        }

        public object FindMerchant(int merchantId)
        {
            var normNpc = M2Share.ActorMgr.Get(merchantId);
            NormNpc npcObject = null;
            var npcType = normNpc.GetType();
            if (npcType == typeof(Merchant))
            {
                npcObject = (Merchant)Convert.ChangeType(normNpc, typeof(Merchant));
            }
            if (npcType == typeof(GuildOfficial))
            {
                npcObject = (GuildOfficial)Convert.ChangeType(normNpc, typeof(GuildOfficial));
            }
            if (npcType == typeof(NormNpc))
            {
                npcObject = (NormNpc)Convert.ChangeType(normNpc, typeof(NormNpc));
            }
            if (npcType == typeof(CastleOfficial))
            {
                npcObject = (CastleOfficial)Convert.ChangeType(normNpc, typeof(CastleOfficial));
            }
            return npcObject;
        }

        public object FindNpc(int npcId)
        {
            return M2Share.ActorMgr.Get(npcId); ;
        }

        /// <summary>
        /// 获取指定地图范围对象数
        /// </summary>
        /// <returns></returns>
        public int GetMapOfRangeHumanCount(Envirnoment envir, int nX, int nY, int nRange)
        {
            var result = 0;
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                var playObject = PlayObjectList[i];
                if (!playObject.Ghost && playObject.Envir == envir)
                {
                    if (Math.Abs(playObject.CurrX - nX) < nRange && Math.Abs(playObject.CurrY - nY) < nRange)
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public bool GetHumPermission(string sUserName, ref string sIPaddr, ref byte btPermission)
        {
            var result = false;
            btPermission = (byte)M2Share.Config.StartPermission;
            for (var i = 0; i < AdminList.Count; i++)
            {
                TAdminInfo adminInfo = AdminList[i];
                if (string.Compare(adminInfo.sChrName, sUserName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    btPermission = (byte)adminInfo.nLv;
                    sIPaddr = adminInfo.sIPaddr;
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void AddUserOpenInfo(UserOpenInfo userOpenInfo)
        {
            HUtil32.EnterCriticalSection(LoadPlaySection);
            try
            {
                LoadPlayList.Add(userOpenInfo);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(LoadPlaySection);
            }
        }

        private void KickOnlineUser(string sChrName)
        {
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                var playObject = PlayObjectList[i];
                if (string.Compare(playObject.ChrName, sChrName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    playObject.m_boKickFlag = true;
                    break;
                }
            }
        }

        private void SendChangeServer(PlayObject playObject, byte nServerIndex)
        {
            var sIPaddr = string.Empty;
            var nPort = 0;
            const string sMsg = "{0}/{1}";
            if (M2Share.GetMultiServerAddrPort(nServerIndex, ref sIPaddr, ref nPort))
            {
                playObject.m_boReconnection = true;
                playObject.SendDefMessage(Grobal2.SM_RECONNECT, 0, 0, 0, 0, string.Format(sMsg, sIPaddr, nPort));
            }
        }

        public void SaveHumanRcd(PlayObject playObject)
        {
            if (playObject.IsRobot) //Bot玩家不保存数据
            {
                return;
            }
            var saveRcd = new TSaveRcd
            {
                sAccount = playObject.m_sUserID,
                sChrName = playObject.ChrName,
                nSessionID = playObject.m_nSessionID,
                PlayObject = playObject,
                HumanRcd = new HumDataInfo()
            };
            MakeSaveRcd(playObject, ref saveRcd.HumanRcd);
            M2Share.FrontEngine.AddToSaveRcdList(saveRcd);
        }

        private void AddToHumanFreeList(PlayObject playObject)
        {
            playObject.GhostTick = HUtil32.GetTickCount();
            PlayObjectFreeList.Add(playObject);
        }

        private void GetHumData(PlayObject playObject, ref HumDataInfo humanRcd)
        {
            HumInfoData humData;
            UserItem[] humItems;
            UserItem[] bagItems;
            MagicRcd[] humMagic;
            MagicInfo magicInfo;
            UserMagic userMagic;
            UserItem[] storageItems;
            UserItem userItem;
            humData = humanRcd.Data;
            playObject.ChrName = humData.ChrName;
            playObject.MapName = humData.CurMap;
            playObject.CurrX = humData.CurX;
            playObject.CurrY = humData.CurY;
            playObject.Direction = humData.Dir;
            playObject.Hair = humData.Hair;
            playObject.Gender = Enum.Parse<PlayGender>(humData.Sex.ToString());
            playObject.Job = (PlayJob)humData.Job;
            playObject.Gold = humData.Gold;
            playObject.Abil.Level = humData.Abil.Level;
            playObject.Abil.HP = humData.Abil.HP;
            playObject.Abil.MP = humData.Abil.MP;
            playObject.Abil.MaxHP = humData.Abil.MaxHP;
            playObject.Abil.MaxMP = humData.Abil.MaxMP;
            playObject.Abil.Exp = humData.Abil.Exp;
            playObject.Abil.MaxExp = humData.Abil.MaxExp;
            playObject.Abil.Weight = humData.Abil.Weight;
            playObject.Abil.MaxWeight = humData.Abil.MaxWeight;
            playObject.Abil.WearWeight = humData.Abil.WearWeight;
            playObject.Abil.MaxWearWeight = humData.Abil.MaxWearWeight;
            playObject.Abil.HandWeight = humData.Abil.HandWeight;
            playObject.Abil.MaxHandWeight = humData.Abil.MaxHandWeight;
            playObject.StatusArr = humData.StatusTimeArr;
            playObject.HomeMap = humData.HomeMap;
            playObject.HomeX = humData.HomeX;
            playObject.HomeY = humData.HomeY;
            playObject.BonusAbil = humData.BonusAbil;
            playObject.BonusPoint = humData.BonusPoint;
            playObject.m_btCreditPoint = humData.CreditPoint;
            playObject.m_btReLevel = humData.ReLevel;
            playObject.m_sMasterName = humData.MasterName;
            playObject.m_boMaster = humData.boMaster;
            playObject.m_sDearName = humData.DearName;
            playObject.m_sStoragePwd = humData.StoragePwd;
            if (playObject.m_sStoragePwd != "")
            {
                playObject.m_boPasswordLocked = true;
            }
            playObject.m_nGameGold = humData.GameGold;
            playObject.m_nGamePoint = humData.GamePoint;
            playObject.m_nPayMentPoint = humData.PayMentPoint;
            playObject.PkPoint = humData.PKPoint;
            if (humData.AllowGroup > 0)
            {
                playObject.AllowGroup = true;
            }
            else
            {
                playObject.AllowGroup = false;
            }
            playObject.BtB2 = humData.btF9;
            playObject.AttatckMode = (AttackMode)humData.AttatckMode;
            playObject.IncHealth = humData.IncHealth;
            playObject.IncSpell = humData.IncSpell;
            playObject.IncHealing = humData.IncHealing;
            playObject.FightZoneDieCount = humData.FightZoneDieCount;
            playObject.m_sUserID = humData.Account;
            playObject.m_boLockLogon = humData.LockLogon;
            playObject.m_wContribution = humData.Contribution;
            playObject.HungerStatus = humData.HungerStatus;
            playObject.AllowGuildReCall = humData.AllowGuildReCall;
            playObject.GroupRcallTime = humData.GroupRcallTime;
            playObject.BodyLuck = humData.BodyLuck;
            playObject.AllowGroupReCall = humData.AllowGroupReCall;
            playObject.QuestUnitOpen = humData.QuestUnitOpen;
            playObject.QuestUnit = humData.QuestUnit;
            playObject.QuestFlag = humData.QuestFlag;
            humItems = humanRcd.Data.HumItems;
            playObject.UseItems[Grobal2.U_DRESS] = humItems[Grobal2.U_DRESS];
            playObject.UseItems[Grobal2.U_WEAPON] = humItems[Grobal2.U_WEAPON];
            playObject.UseItems[Grobal2.U_RIGHTHAND] = humItems[Grobal2.U_RIGHTHAND];
            playObject.UseItems[Grobal2.U_NECKLACE] = humItems[Grobal2.U_HELMET];
            playObject.UseItems[Grobal2.U_HELMET] = humItems[Grobal2.U_NECKLACE];
            playObject.UseItems[Grobal2.U_ARMRINGL] = humItems[Grobal2.U_ARMRINGL];
            playObject.UseItems[Grobal2.U_ARMRINGR] = humItems[Grobal2.U_ARMRINGR];
            playObject.UseItems[Grobal2.U_RINGL] = humItems[Grobal2.U_RINGL];
            playObject.UseItems[Grobal2.U_RINGR] = humItems[Grobal2.U_RINGR];
            playObject.UseItems[Grobal2.U_BUJUK] = humItems[Grobal2.U_BUJUK];
            playObject.UseItems[Grobal2.U_BELT] = humItems[Grobal2.U_BELT];
            playObject.UseItems[Grobal2.U_BOOTS] = humItems[Grobal2.U_BOOTS];
            playObject.UseItems[Grobal2.U_CHARM] = humItems[Grobal2.U_CHARM];
            bagItems = humanRcd.Data.BagItems;
            if (bagItems != null)
            {
                for (var i = 0; i < bagItems.Length; i++)
                {
                    if (bagItems[i] == null)
                    {
                        continue;
                    }
                    if (bagItems[i].Index > 0)
                    {
                        userItem = bagItems[i];
                        playObject.ItemList.Add(userItem);
                    }
                }
            }
            humMagic = humanRcd.Data.Magic;
            if (humMagic != null)
            {
                for (var i = 0; i < humMagic.Length; i++)
                {
                    if (humMagic[i] == null)
                    {
                        continue;
                    }
                    magicInfo = FindMagic(humMagic[i].MagIdx);
                    if (magicInfo != null)
                    {
                        userMagic = new UserMagic();
                        userMagic.Magic = magicInfo;
                        userMagic.MagIdx = humMagic[i].MagIdx;
                        userMagic.Level = humMagic[i].Level;
                        userMagic.Key = humMagic[i].MagicKey;
                        userMagic.TranPoint = humMagic[i].TranPoint;
                        playObject.MagicList.Add(userMagic);
                    }
                }
            }
            storageItems = humanRcd.Data.StorageItems;
            if (storageItems != null)
            {
                for (var i = 0; i < storageItems.Length; i++)
                {
                    if (storageItems[i] == null)
                    {
                        continue;
                    }
                    if (storageItems[i].Index > 0)
                    {
                        userItem = storageItems[i];
                        playObject.StorageItemList.Add(userItem);
                    }
                }
            }
        }

        private void MakeSaveRcd(PlayObject playObject, ref HumDataInfo humanRcd)
        {
            humanRcd.Data.ServerIndex = M2Share.ServerIndex;
            humanRcd.Data.ChrName = playObject.ChrName;
            humanRcd.Data.CurMap = playObject.MapName;
            humanRcd.Data.CurX = playObject.CurrX;
            humanRcd.Data.CurY = playObject.CurrY;
            humanRcd.Data.Dir = playObject.Direction;
            humanRcd.Data.Hair = playObject.Hair;
            humanRcd.Data.Sex = (byte)playObject.Gender;
            humanRcd.Data.Job = (byte)playObject.Job;
            humanRcd.Data.Gold = playObject.Gold;
            humanRcd.Data.Abil.Level = playObject.Abil.Level;
            humanRcd.Data.Abil.HP = playObject.WAbil.HP;
            humanRcd.Data.Abil.MP = playObject.WAbil.MP;
            humanRcd.Data.Abil.MaxHP = playObject.WAbil.MaxHP;
            humanRcd.Data.Abil.MaxMP = playObject.WAbil.MaxMP;
            humanRcd.Data.Abil.Exp = playObject.Abil.Exp;
            humanRcd.Data.Abil.MaxExp = playObject.Abil.MaxExp;
            humanRcd.Data.Abil.Weight = playObject.WAbil.Weight;
            humanRcd.Data.Abil.MaxWeight = playObject.WAbil.MaxWeight;
            humanRcd.Data.Abil.WearWeight = playObject.WAbil.WearWeight;
            humanRcd.Data.Abil.MaxWearWeight = playObject.WAbil.MaxWearWeight;
            humanRcd.Data.Abil.HandWeight = playObject.WAbil.HandWeight;
            humanRcd.Data.Abil.MaxHandWeight = playObject.WAbil.MaxHandWeight;
            humanRcd.Data.Abil.HP = playObject.WAbil.HP;
            humanRcd.Data.Abil.MP = playObject.WAbil.MP;
            humanRcd.Data.StatusTimeArr = playObject.StatusArr;
            humanRcd.Data.HomeMap = playObject.HomeMap;
            humanRcd.Data.HomeX = playObject.HomeX;
            humanRcd.Data.HomeY = playObject.HomeY;
            humanRcd.Data.PKPoint = playObject.PkPoint;
            humanRcd.Data.BonusAbil = playObject.BonusAbil;
            humanRcd.Data.BonusPoint = playObject.BonusPoint;
            humanRcd.Data.StoragePwd = playObject.m_sStoragePwd;
            humanRcd.Data.CreditPoint = playObject.m_btCreditPoint;
            humanRcd.Data.ReLevel = playObject.m_btReLevel;
            humanRcd.Data.MasterName = playObject.m_sMasterName;
            humanRcd.Data.boMaster = playObject.m_boMaster;
            humanRcd.Data.DearName = playObject.m_sDearName;
            humanRcd.Data.GameGold = playObject.m_nGameGold;
            humanRcd.Data.GamePoint = playObject.m_nGamePoint;
            humanRcd.Data.AllowGroup = playObject.AllowGroup ? (byte)1 : (byte)0;
            humanRcd.Data.btF9 = playObject.BtB2;
            humanRcd.Data.AttatckMode = (byte)playObject.AttatckMode;
            humanRcd.Data.IncHealth = (byte)playObject.IncHealth;
            humanRcd.Data.IncSpell = (byte)playObject.IncSpell;
            humanRcd.Data.IncHealing = (byte)playObject.IncHealing;
            humanRcd.Data.FightZoneDieCount = (byte)playObject.FightZoneDieCount;
            humanRcd.Data.Account = playObject.m_sUserID;
            humanRcd.Data.LockLogon = playObject.m_boLockLogon;
            humanRcd.Data.Contribution = playObject.m_wContribution;
            humanRcd.Data.HungerStatus = playObject.HungerStatus;
            humanRcd.Data.AllowGuildReCall = playObject.AllowGuildReCall;
            humanRcd.Data.GroupRcallTime = playObject.GroupRcallTime;
            humanRcd.Data.BodyLuck = playObject.BodyLuck;
            humanRcd.Data.AllowGroupReCall = playObject.AllowGroupReCall;
            humanRcd.Data.QuestUnitOpen = playObject.QuestUnitOpen;
            humanRcd.Data.QuestUnit = playObject.QuestUnit;
            humanRcd.Data.QuestFlag = playObject.QuestFlag;
            var HumItems = humanRcd.Data.HumItems;
            if (HumItems == null)
            {
                HumItems = new UserItem[13];
            }
            HumItems[Grobal2.U_DRESS] = playObject.UseItems[Grobal2.U_DRESS] == null ? HUtil32.DelfautItem : playObject.UseItems[Grobal2.U_DRESS];
            HumItems[Grobal2.U_WEAPON] = playObject.UseItems[Grobal2.U_WEAPON] == null ? HUtil32.DelfautItem : playObject.UseItems[Grobal2.U_WEAPON];
            HumItems[Grobal2.U_RIGHTHAND] = playObject.UseItems[Grobal2.U_RIGHTHAND] == null ? HUtil32.DelfautItem : playObject.UseItems[Grobal2.U_RIGHTHAND];
            HumItems[Grobal2.U_HELMET] = playObject.UseItems[Grobal2.U_NECKLACE] == null ? HUtil32.DelfautItem : playObject.UseItems[Grobal2.U_NECKLACE];
            HumItems[Grobal2.U_NECKLACE] = playObject.UseItems[Grobal2.U_HELMET] == null ? HUtil32.DelfautItem : playObject.UseItems[Grobal2.U_HELMET];
            HumItems[Grobal2.U_ARMRINGL] = playObject.UseItems[Grobal2.U_ARMRINGL] == null ? HUtil32.DelfautItem : playObject.UseItems[Grobal2.U_ARMRINGL];
            HumItems[Grobal2.U_ARMRINGR] = playObject.UseItems[Grobal2.U_ARMRINGR] == null ? HUtil32.DelfautItem : playObject.UseItems[Grobal2.U_ARMRINGR];
            HumItems[Grobal2.U_RINGL] = playObject.UseItems[Grobal2.U_RINGL] == null ? HUtil32.DelfautItem : playObject.UseItems[Grobal2.U_RINGL];
            HumItems[Grobal2.U_RINGR] = playObject.UseItems[Grobal2.U_RINGR] == null ? HUtil32.DelfautItem : playObject.UseItems[Grobal2.U_RINGR];
            HumItems[Grobal2.U_BUJUK] = playObject.UseItems[Grobal2.U_BUJUK] == null ? HUtil32.DelfautItem : playObject.UseItems[Grobal2.U_BUJUK];
            HumItems[Grobal2.U_BELT] = playObject.UseItems[Grobal2.U_BELT] == null ? HUtil32.DelfautItem : playObject.UseItems[Grobal2.U_BELT];
            HumItems[Grobal2.U_BOOTS] = playObject.UseItems[Grobal2.U_BOOTS] == null ? HUtil32.DelfautItem : playObject.UseItems[Grobal2.U_BOOTS];
            HumItems[Grobal2.U_CHARM] = playObject.UseItems[Grobal2.U_CHARM] == null ? HUtil32.DelfautItem : playObject.UseItems[Grobal2.U_CHARM];
            var BagItems = humanRcd.Data.BagItems;
            if (BagItems == null)
            {
                BagItems = new UserItem[46];
            }
            for (var i = 0; i < playObject.ItemList.Count; i++)
            {
                if (i <= 46)
                {
                    BagItems[i] = playObject.ItemList[i];
                }
            }
            for (var i = 0; i < BagItems.Length; i++)
            {
                if (BagItems[i] == null)
                {
                    BagItems[i] = HUtil32.DelfautItem;
                }
            }
            var HumMagic = humanRcd.Data.Magic;
            if (HumMagic == null)
            {
                HumMagic = new MagicRcd[Grobal2.MaxMagicCount];
            }
            for (var i = 0; i < playObject.MagicList.Count; i++)
            {
                if (i >= Grobal2.MaxMagicCount)
                {
                    break;
                }
                var userMagic = playObject.MagicList[i];
                if (HumMagic[i] == null)
                {
                    HumMagic[i] = new MagicRcd();
                }
                HumMagic[i].MagIdx = userMagic.MagIdx;
                HumMagic[i].Level = userMagic.Level;
                HumMagic[i].MagicKey = userMagic.Key;
                HumMagic[i].TranPoint = userMagic.TranPoint;
            }
            for (var i = 0; i < HumMagic.Length; i++)
            {
                if (HumMagic[i] == null)
                {
                    HumMagic[i] = HUtil32.DetailtMagicRcd;
                }
            }
            var StorageItems = humanRcd.Data.StorageItems;
            if (StorageItems == null)
            {
                StorageItems = new UserItem[50];
            }
            for (var i = 0; i < playObject.StorageItemList.Count; i++)
            {
                if (i >= StorageItems.Length)
                {
                    break;
                }
                StorageItems[i] = playObject.StorageItemList[i];
            }
            for (var i = 0; i < StorageItems.Length; i++)
            {
                if (StorageItems[i] == null)
                {
                    StorageItems[i] = HUtil32.DelfautItem;
                }
            }
        }

        private string GetHomeInfo(PlayJob nJob, ref short nX, ref short nY)
        {
            string result;
            int I;
            if (M2Share.StartPointList.Count > 0)
            {
                if (M2Share.StartPointList.Count > M2Share.Config.StartPointSize)
                    I = M2Share.RandomNumber.Random(M2Share.Config.StartPointSize);
                else
                    I = 0;
                result = M2Share.GetStartPointInfo(I, ref nX, ref nY);
            }
            else
            {
                result = M2Share.Config.HomeMap;
                nX = M2Share.Config.HomeX;
                nX = M2Share.Config.HomeY;
            }
            return result;
        }

        private short GetRandHomeX(PlayObject playObject)
        {
            return (short)(M2Share.RandomNumber.Random(3) + (playObject.HomeX - 2));
        }

        private short GetRandHomeY(PlayObject playObject)
        {
            return (short)(M2Share.RandomNumber.Random(3) + (playObject.HomeY - 2));
        }

        public MagicInfo FindMagic(int nMagIdx)
        {
            MagicInfo result = null;
            for (var i = 0; i < MagicList.Count; i++)
            {
                MagicInfo magic = MagicList[i];
                if (magic.MagicId == nMagIdx)
                {
                    result = magic;
                    break;
                }
            }
            return result;
        }

        public void OpenDoor(Envirnoment envir, int nX, int nY)
        {
            var door = envir.GetDoor(nX, nY);
            if (door != null && !door.Status.Opened && !door.Status.bo01)
            {
                door.Status.Opened = true;
                door.Status.OpenTick = HUtil32.GetTickCount();
                SendDoorStatus(envir, nX, nY, Grobal2.RM_DOOROPEN, 0, nX, nY);
            }
        }

        private void CloseDoor(Envirnoment envir, DoorInfo door)
        {
            if (door == null || !door.Status.Opened)
                return;
            door.Status.Opened = false;
            SendDoorStatus(envir, door.nX, door.nY, Grobal2.RM_DOORCLOSE, 0, door.nX, door.nY);
        }
 
        private void SendDoorStatus(Envirnoment envir, int nX, int nY, short wIdent, short wX, int nDoorX, int nDoorY)
        {
            int n1C = nX - 12;
            int n24 = nX + 12;
            int n20 = nY - 12;
            int n28 = nY + 12;
            for (var n10 = n1C; n10 <= n24; n10++)
            {
                for (var n14 = n20; n14 <= n28; n14++)
                {
                    var cellSuccess = false;
                    var cellInfo = envir.GetCellInfo(n10, n14, ref cellSuccess);
                    if (cellSuccess && cellInfo.IsAvailable)
                    {
                        for (var i = 0; i < cellInfo.Count; i++)
                        {
                            var cellObject = cellInfo.ObjList[i];
                            if (cellObject != null && cellObject.CellType == CellType.Monster || cellObject.CellType == CellType.Play)
                            {
                                var baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId); ;
                                if (baseObject != null && !baseObject.Ghost && baseObject.Race == ActorRace.Play)
                                {
                                    baseObject.SendMsg(baseObject, wIdent, wX, nDoorX, nDoorY, 0, "");
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ProcessMapDoor()
        {
            var doorList = M2Share.MapMgr.GetDoorMapList();
            for (var i = 0; i < doorList.Count; i++)
            {
                var envir = doorList[i];
                for (var j = 0; j < envir.DoorList.Count; j++)
                {
                    DoorInfo door = envir.DoorList[j];
                    if (door.Status.Opened)
                    {
                        if ((HUtil32.GetTickCount() - door.Status.OpenTick) > 5 * 1000)
                        {
                            CloseDoor(envir, door);
                        }
                    }
                }
            }
        }

        private void ProcessEvents()
        {
            for (var i = MagicEventList.Count - 1; i >= 0; i--)
            {
                MagicEvent magicEvent = MagicEventList[i];
                if (magicEvent != null)
                {
                    for (var j = magicEvent.BaseObjectList.Count - 1; j >= 0; j--)
                    {
                        BaseObject baseObject = magicEvent.BaseObjectList[j];
                        if (baseObject.Death || baseObject.Ghost || !baseObject.HolySeize)
                        {
                            magicEvent.BaseObjectList.RemoveAt(j);
                        }
                    }
                    if (magicEvent.BaseObjectList.Count <= 0 || (HUtil32.GetTickCount() - magicEvent.dwStartTick) > magicEvent.dwTime ||
                        (HUtil32.GetTickCount() - magicEvent.dwStartTick) > 180000)
                    {
                        var count = 0;
                        while (true)
                        {
                            if (magicEvent.Events[count] != null) magicEvent.Events[count].Close();
                            count++;
                            if (count >= 8) break;
                        }

                        MagicEventList.RemoveAt(i);
                    }
                }
            }
        }

        public MagicInfo FindMagic(string sMagicName)
        {
            for (var i = 0; i < MagicList.Count; i++)
            {
                MagicInfo magic = MagicList[i];
                if (magic.MagicName.Equals(sMagicName, StringComparison.OrdinalIgnoreCase))
                {
                    return magic;
                }
            }
            return null;
        }

        public int GetMapRangeMonster(Envirnoment envir, int nX, int nY, int nRange, IList<BaseObject> list)
        {
            var result = 0;
            if (envir == null) return result;
            for (var i = 0; i < M2Share.Config.ProcessMonsterMultiThreadLimit; i++)
            {
                for (var j = 0; j < MonGenInfoThreadMap[i].Count; j++)
                {
                    var monGen = MonGenInfoThreadMap[i][j];
                    if (monGen == null) continue;
                    if (monGen.Envir != null && monGen.Envir != envir) continue;
                    for (var k = 0; k < monGen.CertList.Count; k++)
                    {
                        var baseObject = monGen.CertList[k];
                        if (!baseObject.Death && !baseObject.Ghost && baseObject.Envir == envir &&
                            Math.Abs(baseObject.CurrX - nX) <= nRange && Math.Abs(baseObject.CurrY - nY) <= nRange)
                        {
                            if (list != null) list.Add(baseObject);
                            result++;
                        }
                    }
                }
            }
            return result;
        }

        public void AddMerchant(Merchant merchant)
        {
            MerchantList.Add(merchant);
        }

        public int GetMerchantList(Envirnoment envir, int nX, int nY, int nRange, IList<BaseObject> tmpList)
        {
            for (var i = 0; i < MerchantList.Count; i++)
            {
                var merchant = MerchantList[i];
                if (merchant.Envir == envir && Math.Abs(merchant.CurrX - nX) <= nRange &&
                    Math.Abs(merchant.CurrY - nY) <= nRange) tmpList.Add(merchant);
            }
            return tmpList.Count;
        }

        public int GetNpcList(Envirnoment envir, int nX, int nY, int nRange, IList<BaseObject> tmpList)
        {
            for (var i = 0; i < QuestNpcList.Count; i++)
            {
                var npc = QuestNpcList[i];
                if (npc.Envir == envir && Math.Abs(npc.CurrX - nX) <= nRange &&
                    Math.Abs(npc.CurrY - nY) <= nRange) tmpList.Add(npc);
            }
            return tmpList.Count;
        }

        public void ReloadMerchantList()
        {
            for (var i = 0; i < MerchantList.Count; i++)
            {
                Merchant merchant = MerchantList[i];
                if (!merchant.Ghost)
                {
                    merchant.ClearScript();
                    merchant.LoadNPCScript();
                }
            }
        }

        public void ReloadNpcList()
        {
            for (var i = 0; i < QuestNpcList.Count; i++)
            {
                NormNpc npc = QuestNpcList[i];
                npc.ClearScript();
                npc.LoadNPCScript();
            }
        }

        public int GetMapMonster(Envirnoment envir, IList<BaseObject> list)
        {
            if (list == null)
            {
                list = new List<BaseObject>();
            }
            var result = 0;
            if (envir == null) return result;
            for (var i = 0; i < MonGenInfoThreadMap.Count; i++)
            {
                if (MonGenInfoThreadMap.TryGetValue(i, out var mongenList))
                {
                    for (var j = 0; j < mongenList.Count; j++)
                    {
                        MonGenInfo monGen = mongenList[j];
                        if (monGen == null) continue;
                        for (var k = 0; k < monGen.CertList.Count; k++)
                        {
                            BaseObject baseObject = monGen.CertList[k];
                            if (!baseObject.Death && !baseObject.Ghost && baseObject.Envir == envir)
                            {
                                list.Add(baseObject);
                                result++;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public void HumanExpire(string sAccount)
        {
            if (!M2Share.Config.KickExpireHuman) return;
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                PlayObject playObject = PlayObjectList[i];
                if (string.Compare(playObject.m_sUserID, sAccount, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    playObject.m_boExpire = true;
                    break;
                }
            }
        }

        public int GetMapHuman(string sMapName)
        {
            var result = 0;
            var envir = M2Share.MapMgr.FindMap(sMapName);
            if (envir == null) return result;
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                PlayObject playObject = PlayObjectList[i];
                if (!playObject.Death && !playObject.Ghost && playObject.Envir == envir) result++;
            }
            return result;
        }

        public int GetMapRageHuman(Envirnoment envir, int nRageX, int nRageY, int nRage, IList<BaseObject> list)
        {
            var result = 0;
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                PlayObject playObject = PlayObjectList[i];
                if (!playObject.Death && !playObject.Ghost && playObject.Envir == envir &&
                    Math.Abs(playObject.CurrX - nRageX) <= nRage && Math.Abs(playObject.CurrY - nRageY) <= nRage)
                {
                    list.Add(playObject);
                    result++;
                }
            }
            return result;
        }

        public ushort GetStdItemIdx(string sItemName)
        {
            ushort result = 0;
            if (string.IsNullOrEmpty(sItemName)) return result;
            for (var i = 0; i < StdItemList.Count; i++)
            {
                var stdItem = StdItemList[i];
                if (stdItem.Name.Equals(sItemName, StringComparison.OrdinalIgnoreCase))
                {
                    result = (ushort)(i + 1);
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 向每个人物发送消息
        /// </summary>
        public void SendBroadCastMsgExt(string sMsg, MsgType msgType)
        {
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                PlayObject playObject = PlayObjectList[i];
                if (!playObject.Ghost)
                    playObject.SysMsg(sMsg, MsgColor.Red, msgType);
            }
        }

        public void SendBroadCastMsg(string sMsg, MsgType msgType)
        {
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                PlayObject playObject = PlayObjectList[i];
                if (!playObject.Ghost)
                {
                    playObject.SysMsg(sMsg, MsgColor.Red, msgType);
                }
            }
        }

        public void sub_4AE514(TGoldChangeInfo goldChangeInfo)
        {
            var goldChange = goldChangeInfo;
            HUtil32.EnterCriticalSection(LoadPlaySection);
            _mChangeHumanDbGoldList.Add(goldChange);
        }

        public void ClearMonSayMsg()
        {
            for (var i = 0; i < M2Share.Config.ProcessMonsterMultiThreadLimit; i++)
            {
                for (var j = 0; j < MonGenInfoThreadMap[i].Count; j++)
                {
                    MonGenInfo monGen = MonGenInfoThreadMap[i][j];
                    for (var k = 0; k < monGen.CertList.Count; k++)
                    {
                        BaseObject monBaseObject = monGen.CertList[k];
                        monBaseObject.SayMsgList = null;
                    }
                }
            }
        }

        public string GetHomeInfo(ref short nX, ref short nY)
        {
            string result;
            if (M2Share.StartPointList.Count > 0)
            {
                int I;
                if (M2Share.StartPointList.Count > M2Share.Config.StartPointSize)
                    I = M2Share.RandomNumber.Random(M2Share.Config.StartPointSize);
                else
                    I = 0;
                result = M2Share.GetStartPointInfo(I, ref nX, ref nY);
            }
            else
            {
                result = M2Share.Config.HomeMap;
                nX = M2Share.Config.HomeX;
                nX = M2Share.Config.HomeY;
            }
            return result;
        }

        public void StartAi()
        {
           
        }

        public void AddAiLogon(RoBotLogon ai)
        {
            RobotLogonList.Add(ai);
        }

        private bool RegenAiObject(RoBotLogon ai)
        {
            var playObject = AddAiPlayObject(ai);
            if (playObject != null)
            {
                playObject.HomeMap = GetHomeInfo(ref playObject.HomeX, ref playObject.HomeY);
                playObject.m_sUserID = "假人" + ai.sChrName;
                playObject.Start(TPathType.t_Dynamic);
                BotPlayObjectList.Add(playObject);
                return true;
            }
            return false;
        }

        private RobotPlayObject AddAiPlayObject(RoBotLogon ai)
        {
            int n1C;
            int n20;
            int n24;
            object p28;
            var envirnoment = M2Share.MapMgr.FindMap(ai.sMapName);
            if (envirnoment == null)
            {
                return null;
            }
            RobotPlayObject cert = new RobotPlayObject();
            cert.Envir = envirnoment;
            cert.MapName = ai.sMapName;
            cert.CurrX = ai.nX;
            cert.CurrY = ai.nY;
            cert.Direction = (byte)M2Share.RandomNumber.Random(8);
            cert.ChrName = ai.sChrName;
            cert.WAbil = cert.Abil;
            if (M2Share.RandomNumber.Random(100) < cert.CoolEyeCode)
            {
                cert.CoolEye = true;
            }
            //Cert.m_sIPaddr = GetIPAddr;// Mac问题
            //Cert.m_sIPLocal = GetIPLocal(Cert.m_sIPaddr);
            cert.m_sConfigFileName = ai.sConfigFileName;
            cert.m_sHeroConfigFileName = ai.sHeroConfigFileName;
            cert.m_sFilePath = ai.sFilePath;
            cert.m_sConfigListFileName = ai.sConfigListFileName;
            cert.m_sHeroConfigListFileName = ai.sHeroConfigListFileName;// 英雄配置列表目录
            cert.Initialize();
            cert.RecalcLevelAbilitys();
            cert.RecalcAbilitys();
            cert.Abil.HP = cert.Abil.MaxHP;
            cert.Abil.MP = cert.Abil.MaxMP;
            if (cert.AddtoMapSuccess)
            {
                p28 = null;
                if (cert.Envir.Width < 50)
                {
                    n20 = 2;
                }
                else
                {
                    n20 = 3;
                }
                if (cert.Envir.Height < 250)
                {
                    if (cert.Envir.Height < 30)
                    {
                        n24 = 2;
                    }
                    else
                    {
                        n24 = 20;
                    }
                }
                else
                {
                    n24 = 50;
                }
                n1C = 0;
                while (true)
                {
                    if (!cert.Envir.CanWalk(cert.CurrX, cert.CurrY, false))
                    {
                        if ((cert.Envir.Width - n24 - 1) > cert.CurrX)
                        {
                            cert.CurrX += (short)n20;
                        }
                        else
                        {
                            cert.CurrX = (byte)(M2Share.RandomNumber.Random(cert.Envir.Width / 2) + n24);
                            if (cert.Envir.Height - n24 - 1 > cert.CurrY)
                            {
                                cert.CurrY += (short)n20;
                            }
                            else
                            {
                                cert.CurrY = (byte)(M2Share.RandomNumber.Random(cert.Envir.Height / 2) + n24);
                            }
                        }
                    }
                    else
                    {
                        p28 = cert.Envir.AddToMap(cert.CurrX, cert.CurrY, cert.MapCell, cert);
                        break;
                    }
                    n1C++;
                    if (n1C >= 31)
                    {
                        break;
                    }
                }
                if (p28 == null)
                {
                    cert = null;
                }
            }
            return cert;
        }

        public void SendQuestMsg(string sQuestName)
        {
            PlayObject playObject;
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                playObject = PlayObjectList[i];
                if (!playObject.Death && !playObject.Ghost)
                    M2Share.g_ManageNPC.GotoLable(playObject, sQuestName, false);
            }
        }

        public void ClearItemList()
        {
            StdItemList.Reverse();
            ClearMerchantData();
        }

        public void SwitchMagicList()
        {
            if (MagicList.Count > 0)
            {
                _oldMagicList.Add(MagicList);
                MagicList = null;
                MagicList = new List<MagicInfo>();
            }
        }

        private void ClearMerchantData()
        {
            for (var i = 0; i < MerchantList.Count; i++)
            {
                MerchantList[i].ClearData();
            }
        }

        public void GuildMemberReGetRankName(GuildInfo guild)
        {
            var nRankNo = 0;
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                if (PlayObjectList[i].MyGuild == guild)
                {
                    guild.GetRankName(PlayObjectList[i], ref nRankNo);
                }
            }
        }
    }
}