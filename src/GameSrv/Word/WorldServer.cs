using GameSrv.Services;
using M2Server;
using M2Server.Actor;
using M2Server.Event.Events;
using M2Server.Player;
using NLog;
using PlanesSystem;
using System.Collections;
using SystemModule;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;
using SystemModule.Packets.ServerPackets;
using GuildInfo = M2Server.Guild.GuildInfo;

namespace GameSrv.Word
{
    public partial class WorldServer : IWorldEngine
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private int ProcessMapDoorTick { get; set; }
        private int ProcessMerchantTimeMax { get; set; }
        private int ProcessMerchantTimeMin { get; set; }
        private int ProcessMissionsTime { get; set; }
        private int ProcessNpcTimeMax { get; set; }
        private int ProcessNpcTimeMin { get; set; }
        private int ProcessLoadPlayTick { get; set; }
        private int ProcHumIdx { get; set; }
        /// <summary>
        /// 交易NPC处理位置
        /// </summary>
        private int MerchantPosition { get; set; }
        /// <summary>
        /// NPC处理位置
        /// </summary>
        private int NpcPosition { get; set; }
        /// <summary>
        /// 处理人物开始索引（每次处理人物数限制）
        /// </summary>
        private int ProcessHumanLoopTime { get; set; }
        public IList<AdminInfo> AdminList { get; set; }
        private readonly IList<GoldChangeInfo> ChangeHumanDbGoldList;
        private readonly IList<SwitchDataInfo> ChangeServerList;
        private readonly IList<int> ListOfGateIdx;
        private readonly IList<int> ListOfSocket;
        /// <summary>
        /// 从DB读取人物数据
        /// </summary>
        protected readonly IList<UserOpenInfo> LoadPlayList;
        protected readonly object LoadPlaySection;
        public readonly IList<MagicEvent> MagicEventList;
        public IList<MagicInfo> MagicList;
        public IList<IMerchant> MerchantList { get; set; }
        protected readonly IList<IPlayerActor> NewHumanList;
        protected readonly IList<IPlayerActor> PlayObjectFreeList;
        protected readonly Dictionary<string, ServerGruopInfo> OtherUserNameList;
        protected readonly IList<IPlayerActor> PlayObjectList;
        protected readonly List<int> LoadPlayerQueue = new List<int>();
        private readonly ArrayList OldMagicList;
        private readonly IList<INormNpc> QuestNpcList;
        /// <summary>
        /// 怪物列表
        /// </summary>
        internal readonly Dictionary<string, MonsterInfo> MonsterList;

        public WorldServer()
        {
            LoadPlaySection = new object();
            LoadPlayList = new List<UserOpenInfo>();
            PlayObjectList = new List<IPlayerActor>();
            PlayObjectFreeList = new List<IPlayerActor>();
            ChangeHumanDbGoldList = new List<GoldChangeInfo>();
            ProcessMapDoorTick = HUtil32.GetTickCount();
            ProcessMissionsTime = HUtil32.GetTickCount();
            ProcessLoadPlayTick = HUtil32.GetTickCount();
            ProcHumIdx = 0;
            ProcessHumanLoopTime = 0;
            MerchantPosition = 0;
            NpcPosition = 0;
            MonsterList = new Dictionary<string, MonsterInfo>(StringComparer.OrdinalIgnoreCase);
            MonGenList = new List<MonGenInfo>();
            MonGenInfoThreadMap = new Dictionary<int, IList<MonGenInfo>>();
            MonsterThreadMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            MagicList = new List<MagicInfo>();
            AdminList = new List<AdminInfo>();
            MerchantList = new List<IMerchant>();
            QuestNpcList = new List<INormNpc>();
            ChangeServerList = new List<SwitchDataInfo>();
            MagicEventList = new List<MagicEvent>();
            ProcessMerchantTimeMin = 0;
            ProcessMerchantTimeMax = 0;
            ProcessNpcTimeMin = 0;
            ProcessNpcTimeMax = 0;
            NewHumanList = new List<IPlayerActor>();
            ListOfGateIdx = new List<int>();
            ListOfSocket = new List<int>();
            OldMagicList = new ArrayList();
            OtherUserNameList = new Dictionary<string, ServerGruopInfo>(StringComparer.OrdinalIgnoreCase);
        }

        public int OnlinePlayObject => GetOnlineHumCount();
        public int PlayObjectCount => GetUserCount();
        public int OfflinePlayCount => 0;
        public int LoadPlayCount => GetLoadPlayCount();
        public IEnumerable<IPlayerActor> PlayObjects { get { return PlayObjectList; } }
        public int MonsterCount { get { return MonsterList.Count; } }
        public int MagicCount { get { return MagicList.Count; } }
        public int MonGenCount => MonGenList.Sum(x => x.Count);

        public void Initialize()
        {
            _logger.Info("正在初始化NPC脚本...");
            MerchantInitialize();
            NpCinitialize();
            _logger.Info("初始化NPC脚本完成...");
        }

        public void AddMonsterList(MonsterInfo monsterInfo)
        {
            MonsterList.Add(monsterInfo.Name, monsterInfo);
        }

        public void AddMagicList(MagicInfo magicInfo)
        {
            MagicList.Add(magicInfo);
        }

        public void Run()
        {
            try
            {
                //for (var i = 0; i < MobThreads.Length; i++)
                //{
                //    var mobThread = MobThreads[i];
                //    if (mobThread == null)
                //    {
                //        continue;
                //    }
                //    if (!mobThread.Stop) continue;
                //    mobThread.Stop = false;
                //}
                //lock (_locker)
                //{
                //    Monitor.PulseAll(_locker);
                //}
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
            if (MonsterList.TryGetValue(sMonName, out var value))
            {
                return value.Race;
            }
            return -1;
        }

        private int GetMonsterThreadId(string sMonName)
        {
            if (MonsterThreadMap.TryGetValue(sMonName, out var threadId))
            {
                return threadId;
            }
            return -1;
        }

        private int GetLoadPlayCount()
        {
            return LoadPlayList.Count;
        }

        internal int GetOnlineHumCount()
        {
            return PlayObjectList.Count;
        }

        internal int GetUserCount()
        {
            return PlayObjectList.Count;
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

        private IPlayerActor ProcessHumansMakeNewHuman(UserOpenInfo userOpenInfo)
        {
            IPlayerActor result = null;
            IPlayerActor playObject = null;
            const string sExceptionMsg = "[Exception] WorldServer::MakeNewHuman";
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
                if (!SystemShare.Config.VentureServer)
                {
                    userOpenInfo.ChrName = string.Empty;
                    userOpenInfo.LoadUser.SessionID = 0;
                    switchDataInfo = GetSwitchData(userOpenInfo.ChrName, userOpenInfo.LoadUser.SessionID);
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
                        short homeX = 0;
                        short homeY = 0;
                        playObject.HomeMap = GetHomeInfo(playObject.Job, ref homeX, ref homeY);
                        playObject.HomeX = homeX;
                        playObject.HomeY = homeY;
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
                            playObject.IsNewHuman = true;
                        }
                    }
                    var envir = SystemShare.MapMgr.GetMapInfo(M2Share.ServerIndex, playObject.MapName);
                    if (envir != null)
                    {
                        playObject.MapFileName = envir.MapFileName;
                        if (envir.Flag.Fight3Zone) // 是否在行会战争地图死亡
                        {
                            if (playObject.Abil.HP <= 0 && playObject.FightZoneDieCount < 3)
                            {
                                playObject.Abil.HP = playObject.Abil.MaxHP;
                                playObject.Abil.MP = playObject.Abil.MaxMP;
                                playObject.DieInFight3Zone = true;
                            }
                            else
                            {
                                playObject.FightZoneDieCount = 0;
                            }
                        }
                    }
                    playObject.MyGuild = SystemShare.GuildMgr.MemberOfGuild(playObject.ChrName);
                    var userCastle = SystemShare.CastleMgr.InCastleWarArea(envir, playObject.CurrX, playObject.CurrY);
                    if (envir != null && userCastle != null && (userCastle.PalaceEnvir == envir || userCastle.UnderWar))
                    {
                        userCastle = SystemShare.CastleMgr.IsCastleMember(playObject);
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
                    if (SystemShare.MapMgr.FindMap(playObject.MapName) == null) playObject.Abil.HP = 0;
                    if (playObject.Abil.HP <= 0)
                    {
                        playObject.ClearStatusTime();
                        if (playObject.PvpLevel() < 2)
                        {
                            userCastle = SystemShare.CastleMgr.IsCastleMember(playObject);
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
                            playObject.MapName = SystemShare.Config.RedDieHomeMap;// '3'
                            playObject.CurrX = (short)(M2Share.RandomNumber.Random(13) + SystemShare.Config.RedDieHomeX);// 839
                            playObject.CurrY = (short)(M2Share.RandomNumber.Random(13) + SystemShare.Config.RedDieHomeY);// 668
                        }
                        playObject.Abil.HP = 14;
                    }
                    playObject.AbilCopyToWAbil();
                    envir = SystemShare.MapMgr.GetMapInfo(M2Share.ServerIndex, playObject.MapName);//切换其他服务器
                    if (envir == null)
                    {
                        playObject.SessionId = userOpenInfo.LoadUser.SessionID;
                        playObject.SocketId = userOpenInfo.LoadUser.SocketId;
                        playObject.GateIdx = userOpenInfo.LoadUser.GateIdx;
                        playObject.SocketIdx = userOpenInfo.LoadUser.GSocketIdx;
                        playObject.WAbil = playObject.Abil;
                        playObject.ServerIndex = (byte)SystemShare.MapMgr.GetMapOfServerIndex(playObject.MapName);
                        if (playObject.Abil.HP != 14)
                        {
                            _logger.Warn(string.Format(sChangeServerFail1, M2Share.ServerIndex, playObject.ServerIndex, playObject.MapName));
                        }
                        SendSwitchData(playObject, playObject.ServerIndex);
                        SendChangeServer(playObject, playObject.ServerIndex);
                        //playObject.SetSocket();
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
                        _logger.Warn(string.Format(sChangeServerFail2, M2Share.ServerIndex, playObject.ServerIndex, playObject.MapName));
                        playObject.MapName = SystemShare.Config.HomeMap;
                        envir = SystemShare.MapMgr.FindMap(SystemShare.Config.HomeMap);
                        playObject.CurrX = SystemShare.Config.HomeX;
                        playObject.CurrY = SystemShare.Config.HomeY;
                    }
                    playObject.Envir = envir;
                    playObject.OnEnvirnomentChanged();
                    if (playObject.Envir == null)
                    {
                        _logger.Error(sErrorEnvirIsNil);
                        goto ReGetMap;
                    }
                    else
                        playObject.BoReadyRun = false;
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
                    var envir = SystemShare.MapMgr.GetMapInfo(M2Share.ServerIndex, playObject.MapName);
                    if (envir != null)
                    {
                        _logger.Warn(string.Format(sChangeServerFail3, M2Share.ServerIndex, playObject.ServerIndex, playObject.MapName));
                        playObject.MapName = SystemShare.Config.HomeMap;
                        envir = SystemShare.MapMgr.FindMap(SystemShare.Config.HomeMap);
                        playObject.CurrX = SystemShare.Config.HomeX;
                        playObject.CurrY = SystemShare.Config.HomeY;
                    }
                    else
                    {
                        if (!envir.CanWalk(playObject.CurrX, playObject.CurrY, true))
                        {
                            _logger.Warn(string.Format(sChangeServerFail4, M2Share.ServerIndex, playObject.ServerIndex, playObject.MapName));
                            playObject.MapName = SystemShare.Config.HomeMap;
                            envir = SystemShare.MapMgr.FindMap(SystemShare.Config.HomeMap);
                            playObject.CurrX = SystemShare.Config.HomeX;
                            playObject.CurrY = SystemShare.Config.HomeY;
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
                            playObject.BoReadyRun = false;
                            playObject.LoginNoticeOk = true;
                            playObject.TryPlayMode = true;
                        }
                    }
                }
                playObject.UserAccount = userOpenInfo.LoadUser.Account;
                playObject.LoginIpAddr = userOpenInfo.LoadUser.sIPaddr;
                playObject.LoginIpLocal = M2Share.GetIPLocal(playObject.LoginIpAddr);
                playObject.SocketId = userOpenInfo.LoadUser.SocketId;
                playObject.SocketIdx = userOpenInfo.LoadUser.GSocketIdx;
                playObject.GateIdx = userOpenInfo.LoadUser.GateIdx;
                playObject.SessionId = userOpenInfo.LoadUser.SessionID;
                playObject.PayMent = (byte)userOpenInfo.LoadUser.PayMent;
                playObject.PayMode = (byte)userOpenInfo.LoadUser.PayMode;
                playObject.ExpireTime = userOpenInfo.LoadUser.PlayTime;
                playObject.ExpireCount = (int)Math.Round(TimeSpan.FromSeconds(playObject.ExpireTime).TotalMinutes, 1);
                playObject.LoadTick = userOpenInfo.LoadUser.NewUserTick;
                //PlayObject.m_nSoftVersionDateEx = M2Share.GetExVersionNO(UserOpenInfo.LoadUser.nSoftVersionDate, ref PlayObject.m_nSoftVersionDate);
                playObject.SoftVersionDate = userOpenInfo.LoadUser.SoftVersionDate;
                playObject.SoftVersionDateEx = userOpenInfo.LoadUser.SoftVersionDate;//M2Share.GetExVersionNO(UserOpenInfo.LoadUser.nSoftVersionDate, ref PlayObject.m_nSoftVersionDate);
                //playObject.SetSocket();
                result = playObject;
            }
            catch (Exception ex)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(ex.StackTrace);
            }
            return result;
        }

        public void ProcessHumans()
        {
            const string sExceptionMsg1 = "[Exception] WorldServer::ProcessHumans -> Ready, Save, Load...";
            const string sExceptionMsg3 = "[Exception] WorldServer::ProcessHumans ClosePlayer.Delete";
            //var dwCheckTime = HUtil32.GetTickCount();
            IPlayerActor playObject;
            if ((HUtil32.GetTickCount() - ProcessLoadPlayTick) > 200)
            {
                ProcessLoadPlayTick = HUtil32.GetTickCount();
                try
                {
                    HUtil32.EnterCriticalSection(LoadPlaySection);
                    try
                    {
                        //没有进入游戏前 不删除和清空列表
                        for (var i = 0; i < LoadPlayList.Count; i++)
                        {
                            var userOpenInfo = LoadPlayList[i];
                            if (userOpenInfo == null)
                            {
                                continue;
                            }
                            if (!M2Share.FrontEngine.IsFull() && !ProcessHumansIsLogined(userOpenInfo.ChrName))
                            {
                                if (userOpenInfo.FailCount >= 50) //超过错误查询次数
                                {
                                    _logger.Warn($"获取玩家数据[{userOpenInfo.ChrName}]失败.");
                                    LoadPlayerQueue.Add(i);
                                    M2Share.NetChannel.SendOutConnectMsg(userOpenInfo.LoadUser.GateIdx, userOpenInfo.LoadUser.SocketId, userOpenInfo.LoadUser.GSocketIdx);
                                    continue;
                                }
                                if (!PlayerDataService.GetPlayData(userOpenInfo.QueryId, ref userOpenInfo.HumanRcd))
                                {
                                    userOpenInfo.FailCount++;
                                    continue;
                                }
                                LoadPlayerQueue.Add(i);
                                playObject = ProcessHumansMakeNewHuman(userOpenInfo);
                                if (playObject != null)
                                {
                                    if (playObject.IsRobot)
                                    {
                                        //BotPlayObjectList.Add((IRobotPlayer)playObject);
                                    }
                                    else
                                    {
                                        PlayObjectList.Add(playObject);
                                    }
                                    NewHumanList.Add(playObject);
                                    SendServerGroupMsg(Messages.ISM_USERLOGON, M2Share.ServerIndex, playObject.ChrName);
                                }
                            }
                            else
                            {
                                KickOnlineUser(userOpenInfo.ChrName);
                                ListOfGateIdx.Add(userOpenInfo.LoadUser.GateIdx);
                                ListOfSocket.Add(userOpenInfo.LoadUser.SocketId);
                            }
                            LoadPlayList[i] = null;
                        }
                        for (var i = 0; i < LoadPlayerQueue.Count; i++)
                        {
                            LoadPlayList.RemoveAt(i);
                        }
                        LoadPlayerQueue.Clear();
                        //LoadPlayList.Clear();
                        for (var i = 0; i < ChangeHumanDbGoldList.Count; i++)
                        {
                            var goldChangeInfo = ChangeHumanDbGoldList[i];
                            playObject = GetPlayObject(goldChangeInfo.sGameMasterName);
                            if (playObject != null)
                            {
                                //  playObject.GoldChange(goldChangeInfo.sGetGoldUser, goldChangeInfo.nGold);
                            }
                        }
                        ChangeHumanDbGoldList.Clear();
                    }
                    finally
                    {
                        HUtil32.LeaveCriticalSection(LoadPlaySection);
                    }
                    for (var i = 0; i < NewHumanList.Count; i++)
                    {
                        playObject = NewHumanList[i];
                        M2Share.NetChannel.SetGateUserList(playObject.GateIdx, playObject.SocketId, playObject);
                    }
                    NewHumanList.Clear();
                    for (var i = 0; i < ListOfGateIdx.Count; i++)
                    {
                        M2Share.NetChannel.CloseUser(ListOfGateIdx[i], ListOfSocket[i]);
                    }
                    ListOfGateIdx.Clear();
                    ListOfSocket.Clear();
                }
                catch (Exception e)
                {
                    _logger.Error(sExceptionMsg1);
                    _logger.Error(e.StackTrace);
                }
            }
            try
            {
                for (var i = 0; i < PlayObjectFreeList.Count; i++)
                {
                    playObject = PlayObjectFreeList[i];
                    if ((HUtil32.GetTickCount() - playObject.GhostTick) > SystemShare.Config.HumanFreeDelayTime)// 5 * 60 * 1000
                    {
                        PlayObjectFreeList[i] = null;
                        PlayObjectFreeList.RemoveAt(i);
                        break;
                    }
                    if (playObject.SwitchData && playObject.RcdSaved)
                    {
                        if (SendSwitchData(playObject, playObject.ServerIndex) || playObject.WriteChgDataErrCount > 20)
                        {
                            playObject.SwitchData = false;
                            playObject.SwitchDataOk = true;
                            playObject.SwitchDataSended = true;
                            playObject.ChgDataWritedTick = HUtil32.GetTickCount();
                        }
                        else
                        {
                            playObject.WriteChgDataErrCount++;
                        }
                    }
                    if (playObject.SwitchDataSended && HUtil32.GetTickCount() - playObject.ChgDataWritedTick > 100)
                    {
                        playObject.SwitchDataSended = false;
                        SendChangeServer(playObject, playObject.ServerIndex);
                    }
                }
            }
            catch
            {
                _logger.Error(sExceptionMsg3);
            }
            ProcessPlayObjectData();
            ProcessHumanLoopTime++;
            if (ProcHumIdx == 0)
            {
                ProcessHumanLoopTime = 0;
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
                            if (!playObject.LoginNoticeOk)
                            {
                                playObject.RunNotice();
                            }
                            else
                            {
                                if (!playObject.BoReadyRun)
                                {
                                    playObject.BoReadyRun = true;
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
                                    if ((HUtil32.GetTickCount() - playObject.ShowLineNoticeTick) > SystemShare.Config.ShowLineNoticeTime)
                                    {
                                        playObject.ShowLineNoticeTick = HUtil32.GetTickCount();
                                        if (M2Share.LineNoticeList.Count > playObject.ShowLineNoticeIdx)
                                        {
                                            var lineNoticeMsg = SystemShare.ManageNPC.GetLineVariableText(playObject, M2Share.LineNoticeList[playObject.ShowLineNoticeIdx]);
                                            switch (lineNoticeMsg[0])
                                            {
                                                case 'R':
                                                    playObject.SysMsg(lineNoticeMsg.AsSpan()[1..].ToString(), MsgColor.Red, MsgType.Notice);
                                                    break;
                                                case 'G':
                                                    playObject.SysMsg(lineNoticeMsg.AsSpan()[1..].ToString(), MsgColor.Green, MsgType.Notice);
                                                    break;
                                                case 'B':
                                                    playObject.SysMsg(lineNoticeMsg.AsSpan()[1..].ToString(), MsgColor.Blue, MsgType.Notice);
                                                    break;
                                                default:
                                                    playObject.SysMsg(lineNoticeMsg, (MsgColor)SystemShare.Config.LineNoticeColor, MsgType.Notice);
                                                    break;
                                            }
                                        }
                                        playObject.ShowLineNoticeIdx++;
                                        if (M2Share.LineNoticeList.Count <= playObject.ShowLineNoticeIdx)
                                        {
                                            playObject.ShowLineNoticeIdx = 0;
                                        }
                                    }
                                    playObject.Run();
                                    if (!M2Share.FrontEngine.IsFull() && (HUtil32.GetTickCount() - playObject.SaveRcdTick) > SystemShare.Config.SaveHumanRcdTime)
                                    {
                                        playObject.SaveRcdTick = HUtil32.GetTickCount();
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
                            M2Share.NetChannel.CloseUser(playObject.GateIdx, playObject.SocketId);
                            SendServerGroupMsg(Messages.ISM_USERLOGOUT, M2Share.ServerIndex, playObject.ChrName);
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
                _logger.Error("[Exception] WorldServer::ProcessHumans");
                _logger.Error(ex.StackTrace);
            }
        }

        private static void ProcessMissions()
        {

        }

        public bool FindOtherServerUser(string sName, ref int nServerIndex)
        {
            if (OtherUserNameList.TryGetValue(sName, out var groupServer))
            {
                nServerIndex = groupServer.nServerIdx;
                _logger.Info($"玩家在[{nServerIndex}]服务器上.");
                return true;
            }
            return false;
        }

        public void CryCry(short wIdent, IEnvirnoment pMap, int nX, int nY, int nWide, byte btFColor, byte btBColor, string sMsg)
        {
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                var playObject = PlayObjectList[i];
                if (!playObject.Ghost && playObject.Envir == pMap && playObject.BanShout &&
                    Math.Abs(playObject.CurrX - nX) < nWide && Math.Abs(playObject.CurrY - nY) < nWide)
                    playObject.SendMsg(null, wIdent, 0, btFColor, btBColor, 0, sMsg);
            }
        }

        public void ProcessUserMessage(IPlayerActor playObject, CommandMessage defMsg, string buff)
        {
            if (playObject.OffLineFlag) return;
            var sMsg = string.Empty;
            if (!string.IsNullOrEmpty(buff)) sMsg = buff;
            switch (defMsg.Ident)
            {
                case Messages.CM_SPELL:
                    if (SystemShare.Config.SpellSendUpdateMsg) // 使用UpdateMsg 可以防止消息队列里有多个操作
                    {
                        playObject.SendUpdateMsg(defMsg.Ident, defMsg.Tag, HUtil32.LoWord(defMsg.Recog), HUtil32.HiWord(defMsg.Recog), HUtil32.MakeLong(defMsg.Param, defMsg.Series), "");
                    }
                    else
                    {
                        playObject.SendMsg(playObject, defMsg.Ident, defMsg.Tag, HUtil32.LoWord(defMsg.Recog), HUtil32.HiWord(defMsg.Recog), HUtil32.MakeLong(defMsg.Param, defMsg.Series));
                    }
                    break;
                case Messages.CM_QUERYUSERNAME:
                    playObject.SendMsg(playObject, defMsg.Ident, 0, defMsg.Recog, defMsg.Param, defMsg.Tag);
                    break;
                case Messages.CM_DROPITEM:
                case Messages.CM_TAKEONITEM:
                case Messages.CM_TAKEOFFITEM:
                case Messages.CM_1005:
                case Messages.CM_MERCHANTDLGSELECT:
                case Messages.CM_MERCHANTQUERYSELLPRICE:
                case Messages.CM_USERSELLITEM:
                case Messages.CM_USERBUYITEM:
                case Messages.CM_USERGETDETAILITEM:
                case Messages.CM_CREATEGROUP:
                case Messages.CM_ADDGROUPMEMBER:
                case Messages.CM_DELGROUPMEMBER:
                case Messages.CM_USERREPAIRITEM:
                case Messages.CM_MERCHANTQUERYREPAIRCOST:
                case Messages.CM_DEALTRY:
                case Messages.CM_DEALADDITEM:
                case Messages.CM_DEALDELITEM:
                case Messages.CM_USERSTORAGEITEM:
                case Messages.CM_USERTAKEBACKSTORAGEITEM:
                case Messages.CM_USERMAKEDRUGITEM:
                case Messages.CM_GUILDADDMEMBER:
                case Messages.CM_GUILDDELMEMBER:
                case Messages.CM_GUILDUPDATENOTICE:
                case Messages.CM_GUILDUPDATERANKINFO:
                    playObject.SendMsg(playObject, defMsg.Ident, defMsg.Series, defMsg.Recog, defMsg.Param, defMsg.Tag, sMsg);
                    break;
                case Messages.CM_PASSWORD:
                case Messages.CM_CHGPASSWORD:
                case Messages.CM_SETPASSWORD:
                    playObject.SendMsg(playObject, defMsg.Ident, defMsg.Param, defMsg.Recog, defMsg.Series, defMsg.Tag, sMsg);
                    break;
                case Messages.CM_ADJUST_BONUS:
                    playObject.SendMsg(playObject, defMsg.Ident, defMsg.Series, defMsg.Recog, defMsg.Param, defMsg.Tag, sMsg);
                    break;
                case Messages.CM_HORSERUN:
                case Messages.CM_TURN:
                case Messages.CM_WALK:
                case Messages.CM_SITDOWN:
                case Messages.CM_RUN:
                case Messages.CM_HIT:
                case Messages.CM_HEAVYHIT:
                case Messages.CM_BIGHIT:
                case Messages.CM_POWERHIT:
                case Messages.CM_LONGHIT:
                case Messages.CM_CRSHIT:
                case Messages.CM_TWINHIT:
                case Messages.CM_WIDEHIT:
                case Messages.CM_FIREHIT:
                    if (SystemShare.Config.ActionSendActionMsg) // 使用UpdateMsg 可以防止消息队列里有多个操作
                    {
                        playObject.SendActionMsg(defMsg.Ident, defMsg.Tag, HUtil32.LoWord(defMsg.Recog), HUtil32.HiWord(defMsg.Recog), 0, "");
                    }
                    else
                    {
                        playObject.SendMsg(playObject, defMsg.Ident, defMsg.Tag, HUtil32.LoWord(defMsg.Recog), HUtil32.HiWord(defMsg.Recog), 0);
                    }
                    break;
                case Messages.CM_SAY:
                    playObject.SendMsg(playObject, Messages.CM_SAY, 0, 0, 0, 0, sMsg);
                    break;
                default:
                    playObject.SendMsg(playObject, defMsg.Ident, defMsg.Series, defMsg.Recog, defMsg.Param, defMsg.Tag, sMsg);
                    break;
            }
            if (!playObject.BoReadyRun) return;
            switch (defMsg.Ident)
            {
                case Messages.CM_TURN:
                case Messages.CM_WALK:
                case Messages.CM_SITDOWN:
                case Messages.CM_RUN:
                case Messages.CM_HIT:
                case Messages.CM_HEAVYHIT:
                case Messages.CM_BIGHIT:
                case Messages.CM_POWERHIT:
                case Messages.CM_LONGHIT:
                case Messages.CM_WIDEHIT:
                case Messages.CM_FIREHIT:
                case Messages.CM_CRSHIT:
                case Messages.CM_TWINHIT:
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
                var hum = PlayObjectFreeList[i];
                if (hum.SwitchDataTempFile == flName)
                {
                    hum.SwitchDataOk = true;
                    break;
                }
            }
        }

        public void OtherServerUserLogon(int sNum, string uname)
        {
            var name = string.Empty;
            var apmode = HUtil32.GetValidStr3(uname, ref name, ':');
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
            var apmode = HUtil32.GetValidStr3(uname, ref name, ':');
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

        public IPlayerActor GetPlayObject(string sName)
        {
            IPlayerActor result = null;
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                if (string.Compare(PlayObjectList[i].ChrName, sName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    var playObject = PlayObjectList[i];
                    if (!playObject.Ghost)
                    {
                        if (!(playObject.IsPasswordLocked && playObject.ObMode && playObject.AdminMode))
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
            HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
            try
            {
                for (var i = 0; i < PlayObjectList.Count; i++)
                {
                    if (string.Compare(PlayObjectList[i].ChrName, sName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        PlayObjectList[i].BoEmergencyClose = true;
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessHumanCriticalSection);
            }
        }

        public IPlayerActor GetPlayObjectEx(string sName)
        {
            IPlayerActor result = null;
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

        /// <summary>
        /// 获取指定地图范围对象数
        /// </summary>
        /// <returns></returns>
        public int GetMapOfRangeHumanCount(IEnvirnoment envir, int nX, int nY, int nRange)
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
            btPermission = SystemShare.Config.StartPermission;
            for (var i = 0; i < AdminList.Count; i++)
            {
                var adminInfo = AdminList[i];
                if (string.Compare(adminInfo.ChrName, sUserName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    btPermission = adminInfo.Level;
                    sIPaddr = adminInfo.IPaddr;
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
                    playObject.BoKickFlag = true;
                    break;
                }
            }
        }

        private static void SendChangeServer(IPlayerActor playObject, byte nServerIndex)
        {
            var sIPaddr = string.Empty;
            var nPort = 0;
            const string sMsg = "{0}/{1}";
            if (M2Share.GetMultiServerAddrPort(nServerIndex, ref sIPaddr, ref nPort))
            {
                playObject.BoReconnection = true;
                playObject.SendDefMessage(Messages.SM_RECONNECT, 0, 0, 0, 0, string.Format(sMsg, sIPaddr, nPort));
            }
        }

        public static void SaveHumanRcd(IPlayerActor playObject)
        {
            if (playObject.IsRobot) //Bot玩家不保存数据
            {
                return;
            }
            var saveRcd = new SavePlayerRcd
            {
                Account = playObject.UserAccount,
                ChrName = playObject.ChrName,
                SessionID = playObject.SessionId,
                PlayObject = playObject
            };
            saveRcd.HumanRcd = MakeSaveRcd(playObject);
            M2Share.FrontEngine.AddToSaveRcdList(saveRcd);
        }

        private void AddToHumanFreeList(IPlayerActor playObject)
        {
            playObject.GhostTick = HUtil32.GetTickCount();
            PlayObjectFreeList.Add(playObject);
        }

        private void GetHumData(IPlayerActor playObject, ref PlayerDataInfo humanRcd)
        {
            var humData = humanRcd.Data;
            playObject.UserAccount = humData.Account;
            playObject.ChrName = humData.ChrName;
            playObject.MapName = humData.CurMap;
            playObject.CurrX = humData.CurX;
            playObject.CurrY = humData.CurY;
            playObject.Dir = humData.Dir;
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
            playObject.StatusTimeArr = humData.StatusTimeArr;
            playObject.HomeMap = humData.HomeMap;
            playObject.HomeX = humData.HomeX;
            playObject.HomeY = humData.HomeY;
            playObject.BonusAbil = humData.BonusAbil;
            playObject.BonusPoint = humData.BonusPoint;
            playObject.CreditPoint = humData.CreditPoint;
            playObject.ReLevel = humData.ReLevel;
            playObject.MasterName = humData.MasterName;
            playObject.IsMaster = humData.IsMaster;
            playObject.DearName = humData.DearName;
            playObject.StoragePwd = humData.StoragePwd;
            if (!string.IsNullOrEmpty(playObject.StoragePwd))
            {
                playObject.IsPasswordLocked = true;
            }
            playObject.GameGold = humData.GameGold;
            playObject.GamePoint = humData.GamePoint;
            playObject.PayMentPoint = humData.PayMentPoint;
            playObject.PkPoint = humData.PKPoint;
            playObject.AllowGroup = humData.AllowGroup > 0;
            playObject.BtB2 = humData.btF9;
            playObject.AttatckMode = (AttackMode)humData.AttatckMode;
            playObject.IncHealth = humData.IncHealth;
            playObject.IncSpell = humData.IncSpell;
            playObject.IncHealing = humData.IncHealing;
            playObject.FightZoneDieCount = humData.FightZoneDieCount;
            playObject.IsLockLogon = humData.LockLogon;
            playObject.Contribution = humData.Contribution;
            playObject.HungerStatus = humData.HungerStatus;
            playObject.AllowGuildReCall = humData.AllowGuildReCall;
            playObject.GroupRcallTime = humData.GroupRcallTime;
            playObject.BodyLuck = humData.BodyLuck;
            playObject.AllowGroupReCall = humData.AllowGroupReCall;
            playObject.QuestUnitOpen = humData.QuestUnitOpen;
            playObject.QuestUnit = humData.QuestUnit;
            playObject.QuestFlag = humData.QuestFlag;
            ServerUserItem[] humItems = humanRcd.Data.HumItems;
            playObject.UseItems[ItemLocation.Dress] = humItems[ItemLocation.Dress].ToClientItem();
            playObject.UseItems[ItemLocation.Weapon] = humItems[ItemLocation.Weapon].ToClientItem();
            playObject.UseItems[ItemLocation.RighThand] = humItems[ItemLocation.RighThand].ToClientItem();
            playObject.UseItems[ItemLocation.Necklace] = humItems[ItemLocation.Helmet].ToClientItem();
            playObject.UseItems[ItemLocation.Helmet] = humItems[ItemLocation.Necklace].ToClientItem();
            playObject.UseItems[ItemLocation.ArmRingl] = humItems[ItemLocation.ArmRingl].ToClientItem();
            playObject.UseItems[ItemLocation.ArmRingr] = humItems[ItemLocation.ArmRingr].ToClientItem();
            playObject.UseItems[ItemLocation.Ringl] = humItems[ItemLocation.Ringl].ToClientItem();
            playObject.UseItems[ItemLocation.Ringr] = humItems[ItemLocation.Ringr].ToClientItem();
            playObject.UseItems[ItemLocation.Bujuk] = humItems[ItemLocation.Bujuk].ToClientItem();
            playObject.UseItems[ItemLocation.Belt] = humItems[ItemLocation.Belt].ToClientItem();
            playObject.UseItems[ItemLocation.Boots] = humItems[ItemLocation.Boots].ToClientItem();
            playObject.UseItems[ItemLocation.Charm] = humItems[ItemLocation.Charm].ToClientItem();
            ServerUserItem[] bagItems = humanRcd.Data.BagItems;
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
                        playObject.ItemList.Add(bagItems[i].ToClientItem());
                    }
                }
            }
            MagicRcd[] humMagic = humanRcd.Data.Magic;
            if (humMagic != null)
            {
                for (var i = 0; i < humMagic.Length; i++)
                {
                    if (humMagic[i] == null)
                    {
                        continue;
                    }
                    var magicInfo = FindMagic(humMagic[i].MagIdx);
                    if (magicInfo != null)
                    {
                        var userMagic = new UserMagic();
                        userMagic.Magic = magicInfo;
                        userMagic.MagIdx = humMagic[i].MagIdx;
                        userMagic.Level = humMagic[i].Level;
                        userMagic.Key = humMagic[i].MagicKey;
                        userMagic.TranPoint = humMagic[i].TranPoint;
                        playObject.MagicList.Add(userMagic);
                    }
                }
            }
            ServerUserItem[] storageItems = humanRcd.Data.StorageItems;
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
                        playObject.StorageItemList.Add(storageItems[i].ToClientItem());
                    }
                }
            }
        }

        private static PlayerDataInfo MakeSaveRcd(IPlayerActor playObject)
        {
            var humanRcd = new PlayerDataInfo();
            var playerInfo = humanRcd.Data;
            var playAbil = humanRcd.Data.Abil;
            playerInfo.ServerIndex = M2Share.ServerIndex;
            playerInfo.ChrName = playObject.ChrName;
            playerInfo.CurMap = playObject.MapName;
            playerInfo.CurX = playObject.CurrX;
            playerInfo.CurY = playObject.CurrY;
            playerInfo.Dir = playObject.Dir;
            playerInfo.Hair = playObject.Hair;
            playerInfo.Sex = (byte)playObject.Gender;
            playerInfo.Job = (byte)playObject.Job;
            playerInfo.Gold = playObject.Gold;
            playAbil.Level = playObject.Abil.Level;
            playAbil.HP = playObject.WAbil.HP;
            playAbil.MP = playObject.WAbil.MP;
            playAbil.MaxHP = playObject.WAbil.MaxHP;
            playAbil.MaxMP = playObject.WAbil.MaxMP;
            playAbil.Exp = playObject.Abil.Exp;
            playAbil.MaxExp = playObject.Abil.MaxExp;
            playAbil.Weight = playObject.WAbil.Weight;
            playAbil.MaxWeight = playObject.WAbil.MaxWeight;
            playAbil.WearWeight = playObject.WAbil.WearWeight;
            playAbil.MaxWearWeight = playObject.WAbil.MaxWearWeight;
            playAbil.HandWeight = playObject.WAbil.HandWeight;
            playAbil.MaxHandWeight = playObject.WAbil.MaxHandWeight;
            playAbil.HP = playObject.WAbil.HP;
            playAbil.MP = playObject.WAbil.MP;
            playerInfo.StatusTimeArr = playObject.StatusTimeArr;
            playerInfo.HomeMap = playObject.HomeMap;
            playerInfo.HomeX = playObject.HomeX;
            playerInfo.HomeY = playObject.HomeY;
            playerInfo.PKPoint = playObject.PkPoint;
            playerInfo.BonusAbil = playObject.BonusAbil;
            playerInfo.BonusPoint = playObject.BonusPoint;
            playerInfo.StoragePwd = playObject.StoragePwd;
            playerInfo.CreditPoint = playObject.CreditPoint;
            playerInfo.ReLevel = playObject.ReLevel;
            playerInfo.MasterName = playObject.MasterName;
            playerInfo.IsMaster = playObject.IsMaster;
            playerInfo.DearName = playObject.DearName;
            playerInfo.GameGold = playObject.GameGold;
            playerInfo.GamePoint = playObject.GamePoint;
            playerInfo.AllowGroup = playObject.AllowGroup ? (byte)1 : (byte)0;
            playerInfo.btF9 = playObject.BtB2;
            playerInfo.AttatckMode = (byte)playObject.AttatckMode;
            playerInfo.IncHealth = (byte)playObject.IncHealth;
            playerInfo.IncSpell = (byte)playObject.IncSpell;
            playerInfo.IncHealing = (byte)playObject.IncHealing;
            playerInfo.FightZoneDieCount = (byte)playObject.FightZoneDieCount;
            playerInfo.Account = playObject.UserAccount;
            playerInfo.LockLogon = playObject.IsLockLogon;
            playerInfo.Contribution = playObject.Contribution;
            playerInfo.HungerStatus = playObject.HungerStatus;
            playerInfo.AllowGuildReCall = playObject.AllowGuildReCall;
            playerInfo.GroupRcallTime = playObject.GroupRcallTime;
            playerInfo.BodyLuck = playObject.BodyLuck;
            playerInfo.AllowGroupReCall = playObject.AllowGroupReCall;
            playerInfo.QuestUnitOpen = playObject.QuestUnitOpen;
            playerInfo.QuestUnit = playObject.QuestUnit;
            playerInfo.QuestFlag = playObject.QuestFlag;
            ServerUserItem[] HumItems = humanRcd.Data.HumItems;
            if (HumItems == null)
            {
                HumItems = new ServerUserItem[13];
            }
            HumItems[ItemLocation.Dress] = playObject.UseItems[ItemLocation.Dress] == null ? HUtil32.DelfautItem.ToServerItem() : playObject.UseItems[ItemLocation.Dress].ToServerItem();
            HumItems[ItemLocation.Weapon] = playObject.UseItems[ItemLocation.Weapon] == null ? HUtil32.DelfautItem.ToServerItem() : playObject.UseItems[ItemLocation.Weapon].ToServerItem();
            HumItems[ItemLocation.RighThand] = playObject.UseItems[ItemLocation.RighThand] == null ? HUtil32.DelfautItem.ToServerItem() : playObject.UseItems[ItemLocation.RighThand].ToServerItem();
            HumItems[ItemLocation.Helmet] = playObject.UseItems[ItemLocation.Necklace] == null ? HUtil32.DelfautItem.ToServerItem() : playObject.UseItems[ItemLocation.Necklace].ToServerItem();
            HumItems[ItemLocation.Necklace] = playObject.UseItems[ItemLocation.Helmet] == null ? HUtil32.DelfautItem.ToServerItem() : playObject.UseItems[ItemLocation.Helmet].ToServerItem();
            HumItems[ItemLocation.ArmRingl] = playObject.UseItems[ItemLocation.ArmRingl] == null ? HUtil32.DelfautItem.ToServerItem() : playObject.UseItems[ItemLocation.ArmRingl].ToServerItem();
            HumItems[ItemLocation.ArmRingr] = playObject.UseItems[ItemLocation.ArmRingr] == null ? HUtil32.DelfautItem.ToServerItem() : playObject.UseItems[ItemLocation.ArmRingr].ToServerItem();
            HumItems[ItemLocation.Ringl] = playObject.UseItems[ItemLocation.Ringl] == null ? HUtil32.DelfautItem.ToServerItem() : playObject.UseItems[ItemLocation.Ringl].ToServerItem();
            HumItems[ItemLocation.Ringr] = playObject.UseItems[ItemLocation.Ringr] == null ? HUtil32.DelfautItem.ToServerItem() : playObject.UseItems[ItemLocation.Ringr].ToServerItem();
            HumItems[ItemLocation.Bujuk] = playObject.UseItems[ItemLocation.Bujuk] == null ? HUtil32.DelfautItem.ToServerItem() : playObject.UseItems[ItemLocation.Bujuk].ToServerItem();
            HumItems[ItemLocation.Belt] = playObject.UseItems[ItemLocation.Belt] == null ? HUtil32.DelfautItem.ToServerItem() : playObject.UseItems[ItemLocation.Belt].ToServerItem();
            HumItems[ItemLocation.Boots] = playObject.UseItems[ItemLocation.Boots] == null ? HUtil32.DelfautItem.ToServerItem() : playObject.UseItems[ItemLocation.Boots].ToServerItem();
            HumItems[ItemLocation.Charm] = playObject.UseItems[ItemLocation.Charm] == null ? HUtil32.DelfautItem.ToServerItem() : playObject.UseItems[ItemLocation.Charm].ToServerItem();
            ServerUserItem[] BagItems = humanRcd.Data.BagItems;
            if (BagItems == null)
            {
                BagItems = new ServerUserItem[Grobal2.MaxBagItem];
            }
            for (var i = 0; i < playObject.ItemList.Count; i++)
            {
                if (i < Grobal2.MaxBagItem)
                {
                    BagItems[i] = playObject.ItemList[i].ToServerItem();
                }
            }
            for (var i = 0; i < BagItems.Length; i++)
            {
                if (BagItems[i] == null)
                {
                    BagItems[i] = HUtil32.DelfautItem.ToServerItem();
                }
            }
            MagicRcd[] HumMagic = humanRcd.Data.Magic;
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
            ServerUserItem[] StorageItems = humanRcd.Data.StorageItems;
            if (StorageItems == null)
            {
                StorageItems = new ServerUserItem[50];
            }
            for (var i = 0; i < playObject.StorageItemList.Count; i++)
            {
                if (i >= StorageItems.Length)
                {
                    break;
                }
                StorageItems[i] = playObject.StorageItemList[i].ToServerItem();
            }
            for (var i = 0; i < StorageItems.Length; i++)
            {
                if (StorageItems[i] == null)
                {
                    StorageItems[i] = HUtil32.DelfautItem.ToServerItem();
                }
            }
            humanRcd.Data = playerInfo;
            humanRcd.Data.Abil = playAbil;
            return humanRcd;
        }

        private static string GetHomeInfo(PlayJob nJob, ref short nX, ref short nY)
        {
            string result;
            int I;
            if (M2Share.StartPointList.Count > 0)
            {
                if (M2Share.StartPointList.Count > SystemShare.Config.StartPointSize)
                    I = M2Share.RandomNumber.Random(SystemShare.Config.StartPointSize);
                else
                    I = 0;
                result = M2Share.GetStartPointInfo(I, ref nX, ref nY);
            }
            else
            {
                result = SystemShare.Config.HomeMap;
                nX = SystemShare.Config.HomeX;
                nX = SystemShare.Config.HomeY;
            }
            return result;
        }

        private static short GetRandHomeX(IPlayerActor playObject)
        {
            return (short)(M2Share.RandomNumber.Random(3) + (playObject.HomeX - 2));
        }

        private static short GetRandHomeY(IPlayerActor playObject)
        {
            return (short)(M2Share.RandomNumber.Random(3) + (playObject.HomeY - 2));
        }

        public INormNpc FindNpc(int npcId)
        {
            return SystemShare.ActorMgr.Get<INormNpc>(npcId);
        }

        public IMerchant FindMerchant(int npcId)
        {
            return SystemShare.ActorMgr.Get<IMerchant>(npcId);
        }

        public MagicInfo FindMagic(int nMagIdx)
        {
            MagicInfo result = null;
            for (var i = 0; i < MagicList.Count; i++)
            {
                var magic = MagicList[i];
                if (magic.MagicId == nMagIdx)
                {
                    result = magic;
                    break;
                }
            }
            return result;
        }

        public static void OpenDoor(IEnvirnoment envir, int nX, int nY)
        {
            MapDoor door = default;
            if (envir.GetDoor(nX, nY, ref door) && !door.Status.Opened)
            {
                door.Status.Opened = true;
                door.Status.OpenTick = HUtil32.GetTickCount();
                SendDoorStatus(envir, nX, nY, Messages.RM_DOOROPEN, 0, nX, nY);
            }
        }

        private static void CloseDoor(IEnvirnoment envir, MapDoor door)
        {
            if (!door.Status.Opened)
                return;
            door.Status.Opened = false;
            SendDoorStatus(envir, door.nX, door.nY, Messages.RM_DOORCLOSE, 0, door.nX, door.nY);
        }

        private static void SendDoorStatus(IEnvirnoment envir, int nX, int nY, short wIdent, short wX, int nDoorX, int nDoorY)
        {
            var n1C = nX - 12;
            var n24 = nX + 12;
            var n20 = nY - 12;
            var n28 = nY + 12;
            for (var n10 = n1C; n10 <= n24; n10++)
            {
                for (var n14 = n20; n14 <= n28; n14++)
                {
                    ref var cellInfo = ref envir.GetCellInfo(n10, n14, out var cellSuccess);
                    if (cellSuccess && cellInfo.IsAvailable)
                    {
                        for (var i = 0; i < cellInfo.ObjList.Count; i++)
                        {
                            var cellObject = cellInfo.ObjList[i];
                            if (cellObject.CellObjId > 0 && cellObject.ActorObject)
                            {
                                var baseObject = SystemShare.ActorMgr.Get(cellObject.CellObjId);
                                if (baseObject != null && !baseObject.Ghost && baseObject.Race == ActorRace.Play)
                                {
                                    baseObject.SendMsg(baseObject, wIdent, wX, nDoorX, nDoorY, 0);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void ProcessMapDoor()
        {
            IList<IEnvirnoment> doorList = SystemShare.MapMgr.GetDoorMapList();
            for (var i = 0; i < doorList.Count; i++)
            {
                var envir = doorList[i];
                for (var j = 0; j < envir.DoorList.Count; j++)
                {
                    var door = envir.DoorList[j];
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
                var magicEvent = MagicEventList[i];
                if (magicEvent != null)
                {
                    for (var j = magicEvent.ObjectList.Count - 1; j >= 0; j--)
                    {
                        var baseObject = magicEvent.ObjectList[j];
                        if (baseObject.Race >= ActorRace.Animal && !((AnimalObject)baseObject).HolySeize)
                        {
                            magicEvent.ObjectList.RemoveAt(j);
                        }
                        else if (baseObject.Death || baseObject.Ghost)
                        {
                            magicEvent.ObjectList.RemoveAt(j);
                        }
                    }
                    if (magicEvent.ObjectList.Count <= 0 || (HUtil32.GetTickCount() - magicEvent.StartTick) > magicEvent.Time || (HUtil32.GetTickCount() - magicEvent.StartTick) > 180000)
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
                var magic = MagicList[i];
                if (magic.MagicName.Equals(sMagicName, StringComparison.OrdinalIgnoreCase))
                {
                    return magic;
                }
            }
            return null;
        }

        public void AddMerchant(IMerchant merchant)
        {
            if (merchant != null)
            {
                MerchantList.Add(merchant);
            }
        }

        public int GetMapRangeMonster(Envirnoment envir, int nX, int nY, int nRange, IList<IActor> list)
        {
            var result = 0;
            if (envir == null) return result;
            for (var i = 0; i < SystemShare.Config.ProcessMonsterMultiThreadLimit; i++)
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
                            list.Add(baseObject);
                            result++;
                        }
                    }
                }
            }
            return result;
        }

        public int GetMapMonster(IEnvirnoment envir, IList<IActor> list)
        {
            if (list == null)
            {
                list = new List<IActor>();
            }
            var result = 0;
            if (envir == null) return result;
            for (var i = 0; i < MonGenInfoThreadMap.Count; i++)
            {
                if (MonGenInfoThreadMap.TryGetValue(i, out IList<MonGenInfo> mongenList))
                {
                    for (var j = 0; j < mongenList.Count; j++)
                    {
                        var monGen = mongenList[j];
                        if (monGen == null) continue;
                        for (var k = 0; k < monGen.CertList.Count; k++)
                        {
                            var monObject = monGen.CertList[k];
                            if (!monObject.Death && !monObject.Ghost && monObject.Envir == envir)
                            {
                                list.Add(monObject);
                                result++;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public int GetMapHuman(string sMapName)
        {
            var result = 0;
            var envir = SystemShare.MapMgr.FindMap(sMapName);
            if (envir == null) return result;
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                var playObject = PlayObjectList[i];
                if (!playObject.Death && !playObject.Ghost && playObject.Envir == envir) result++;
            }
            return result;
        }

        public void GetMapRageHuman(IEnvirnoment envir, int nRageX, int nRageY, int nRage, ref IList<IActor> list, bool botPlay = false)
        {
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                var playObject = PlayObjectList[i];
                if (!playObject.Death && !playObject.Ghost && playObject.Envir == envir &&
                    Math.Abs(playObject.CurrX - nRageX) <= nRage && Math.Abs(playObject.CurrY - nRageY) <= nRage)
                {
                    list.Add(playObject);
                }
            }
            if (botPlay)
            {
                //for (var i = 0; i < BotPlayObjectList.Count; i++)
                //{
                //    var botPlayer = BotPlayObjectList[i];
                //    if (!botPlayer.Death && !botPlayer.Ghost && botPlayer.Envir == envir &&
                //        Math.Abs(botPlayer.CurrX - nRageX) <= nRage && Math.Abs(botPlayer.CurrY - nRageY) <= nRage)
                //    {
                //        list.Add(botPlayer);
                //    }
                //}
            }
        }

        /// <summary>
        /// 向每个人物发送消息
        /// </summary>
        public void SendBroadCastMsgExt(string sMsg, MsgType msgType)
        {
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                var playObject = PlayObjectList[i];
                if (!playObject.Ghost)
                    playObject.SysMsg(sMsg, MsgColor.Red, msgType);
            }
        }

        public void SendBroadCastMsg(string sMsg, MsgType msgType)
        {
            if (SystemShare.Config.ShowPreFixMsg)
            {
                switch (msgType)
                {
                    case MsgType.Mon:
                        sMsg = SystemShare.Config.MonSayMsgPreFix + sMsg;
                        break;
                    case MsgType.Hint:
                        sMsg = SystemShare.Config.HintMsgPreFix + sMsg;
                        break;
                    case MsgType.GameManger:
                        sMsg = SystemShare.Config.GameManagerRedMsgPreFix + sMsg;
                        break;
                    case MsgType.System:
                        sMsg = SystemShare.Config.SysMsgPreFix + sMsg;
                        break;
                    case MsgType.Cust:
                        sMsg = SystemShare.Config.CustMsgPreFix + sMsg;
                        break;
                    case MsgType.Castle:
                        sMsg = SystemShare.Config.CastleMsgPreFix + sMsg;
                        break;
                }
            }
            if (SystemShare.Config.EnableChatServer)
            {
                //M2Share.ChatChannel.SendPubChannelMessage(sMsg);
            }
            else
            {
                for (var i = 0; i < PlayObjectList.Count; i++)
                {
                    var playObject = PlayObjectList[i];
                    if (!playObject.Ghost)
                    {
                        playObject.SysMsg(sMsg, MsgColor.Red, msgType);
                    }
                }
            }
        }

        public void sub4AE514(GoldChangeInfo goldChangeInfo)
        {
            var goldChange = goldChangeInfo;
            HUtil32.EnterCriticalSection(LoadPlaySection);
            ChangeHumanDbGoldList.Add(goldChange);
        }

        public void ClearMonSayMsg()
        {
            for (var i = 0; i < SystemShare.Config.ProcessMonsterMultiThreadLimit; i++)
            {
                for (var j = 0; j < MonGenInfoThreadMap[i].Count; j++)
                {
                    var monGen = MonGenInfoThreadMap[i][j];
                    for (var k = 0; k < monGen.CertList.Count; k++)
                    {
                        var monBaseObject = monGen.CertList[k];
                        //monBaseObject.SayMsgList = null;
                    }
                }
            }
        }

        public static string GetHomeInfo(ref short nX, ref short nY)
        {
            string result;
            if (M2Share.StartPointList.Count > 0)
            {
                int I;
                if (M2Share.StartPointList.Count > SystemShare.Config.StartPointSize)
                    I = M2Share.RandomNumber.Random(SystemShare.Config.StartPointSize);
                else
                    I = 0;
                result = M2Share.GetStartPointInfo(I, ref nX, ref nY);
            }
            else
            {
                result = SystemShare.Config.HomeMap;
                nX = SystemShare.Config.HomeX;
                nX = SystemShare.Config.HomeY;
            }
            return result;
        }

        public void SendQuestMsg(string sQuestName)
        {
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                IPlayerActor playObject = PlayObjectList[i];
                if (!playObject.Death && !playObject.Ghost)
                {
                    playObject.NpcGotoLable(SystemShare.ManageNPC, sQuestName, false);
                }
            }
        }

        public void ClearItemList()
        {
            ClearMerchantData();
        }

        public void ClearMonsterList()
        {
            MonsterList.Clear();
        }

        public void SwitchMagicList()
        {
            if (MagicList.Count > 0)
            {
                OldMagicList.Add(MagicList);
                MagicList = null;
                MagicList = new List<MagicInfo>();
            }
        }

        public void GuildMemberReGetRankName(GuildInfo guild)
        {
            short nRankNo = 0;
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                if (PlayObjectList[i].MyGuild == guild)
                {
                    guild.GetRankName(PlayObjectList[i], ref nRankNo);
                }
            }
        }

        public int GetPlayExpireTime(string account)
        {
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                if (string.Compare(PlayObjectList[i].UserAccount, account, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return PlayObjectList[i].QueryExpireTick;
                }
            }
            return 0;
        }

        public void SetPlayExpireTime(string account, int playTime)
        {
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                if (string.Compare(PlayObjectList[i].UserAccount, account, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    PlayObjectList[i].QueryExpireTick = playTime;
                    break;
                }
            }
        }

        public void AccountExpired(string account)
        {
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                if (string.Compare(PlayObjectList[i].UserAccount, account, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    PlayObjectList[i].AccountExpired = true;
                    break;
                }
            }
        }

        public void TimeAccountExpired(string account)
        {
            for (var i = 0; i < PlayObjectList.Count; i++)
            {
                if (string.Compare(PlayObjectList[i].UserAccount, account, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    //  PlayObjectList[i].SetExpiredTime(5);
                    break;
                }
            }
        }

        public void AddMonGenList(MonGenInfo monGenInfo)
        {
            MonGenList.Add(monGenInfo);
        }

        public bool CheckMonGenInfoThreadMap(int threadId)
        {
            return MonGenInfoThreadMap.ContainsKey(threadId);
        }

        public void AddMonGenInfoThreadMap(int threadId, MonGenInfo monGenInfo)
        {
            if (MonGenInfoThreadMap.ContainsKey(threadId))
            {
                MonGenInfoThreadMap[threadId].Add(monGenInfo);
            }
        }

        public void CreateMonGenInfoThreadMap(int threadId, IList<MonGenInfo> monGenInfo)
        {
            MonGenInfoThreadMap.Add(threadId, monGenInfo);
        }

        public void AddQuestNpc(INormNpc normNpc)
        {
            QuestNpcList.Add(normNpc);
        }
    }
}