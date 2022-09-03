using GameSvr.Actor;
using GameSvr.Event.Events;
using GameSvr.Guild;
using GameSvr.Items;
using GameSvr.Maps;
using GameSvr.Monster;
using GameSvr.Monster.Monsters;
using GameSvr.Npc;
using GameSvr.Player;
using GameSvr.RobotPlay;
using GameSvr.Services;
using GameSvr.Snaps;
using NLog;
using System.Collections;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.UsrSystem
{
    public partial class UserEngine
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private int _dwProcessMapDoorTick;
        public int DwProcessMerchantTimeMax;
        public int DwProcessMerchantTimeMin;
        private int _dwProcessMissionsTime;
        public int DwProcessNpcTimeMax;
        public int DwProcessNpcTimeMin;
        private int _dwRegenMonstersTick;
        private int _dwSendOnlineHumTime;
        private int _dwShowOnlineTick;
        public readonly IList<TAdminInfo> MAdminList;
        private readonly IList<TGoldChangeInfo> _mChangeHumanDbGoldList;
        private readonly IList<TSwitchDataInfo> _mChangeServerList;
        private int _mDwProcessLoadPlayTick;
        private readonly IList<int> _mListOfGateIdx;
        private readonly IList<int> _mListOfSocket;
        /// <summary>
        /// 从DB读取人物数据
        /// </summary>
        private readonly IList<TUserOpenInfo> _LoadPlayList;
        private readonly object _LoadPlaySection;
        public readonly IList<MagicEvent> MagicEventList;
        public IList<TMagic> MagicList;
        public readonly IList<Merchant> MerchantList;
        public readonly IList<MonGenInfo> MonGenList;
        private int _currMonGenIdx;
        private readonly IList<PlayObject> NewHumanList;
        /// <summary>
        /// 当前怪物列表刷新位置索引
        /// </summary>
        private int _monGenCertListPosition;
        private int _monGenListPosition;
        private int _procHumIdx;
        private int _procBotHubIdx;
        private readonly IList<PlayObject> _PlayObjectFreeList;
        private readonly Dictionary<string, ServerGruopInfo> _OtherUserNameList;
        private readonly IList<PlayObject> _PlayObjectList;
        private readonly IList<PlayObject> _BotPlayObjectList;
        public readonly IList<TMonInfo> MonsterList;
        /// <summary>
        /// 交易NPC处理位置
        /// </summary>
        private int _merchantPosition;
        /// <summary>
        /// 怪物总数
        /// </summary>
        private int _monsterCount;
        /// <summary>
        /// 处理怪物数，用于统计处理怪物个数
        /// </summary>
        private int MonsterProcessCount = 0;
        /// <summary>
        /// 处理怪物总数位置，用于计算怪物总数
        /// </summary>
        private int MonsterProcessPostion;
        /// <summary>
        /// NPC处理位置
        /// </summary>
        private int NpcPosition;
        /// <summary>
        /// 处理人物开始索引（每次处理人物数限制）
        /// </summary>
        private int _nProcessHumanLoopTime;
        private readonly ArrayList _oldMagicList;
        public readonly IList<NormNpc> QuestNpcList;
        public readonly IList<StdItem> StdItemList;
        public long MDwAiLogonTick;//处理假人间隔
        private readonly IList<RoBotLogon> _mUserLogonList;//假人列表
        private Timer _userEngineThread;
        private Thread _processAiThread;

        public UserEngine()
        {
            _LoadPlaySection = new object();
            _LoadPlayList = new List<TUserOpenInfo>();
            _PlayObjectList = new List<PlayObject>();
            _PlayObjectFreeList = new List<PlayObject>();
            _mChangeHumanDbGoldList = new List<TGoldChangeInfo>();
            _dwShowOnlineTick = HUtil32.GetTickCount();
            _dwSendOnlineHumTime = HUtil32.GetTickCount();
            _dwProcessMapDoorTick = HUtil32.GetTickCount();
            _dwProcessMissionsTime = HUtil32.GetTickCount();
            _dwRegenMonstersTick = HUtil32.GetTickCount();
            _mDwProcessLoadPlayTick = HUtil32.GetTickCount();
            _currMonGenIdx = 0;
            _monGenListPosition = 0;
            _monGenCertListPosition = 0;
            _procHumIdx = 0;
            _procBotHubIdx = 0;
            _nProcessHumanLoopTime = 0;
            _merchantPosition = 0;
            NpcPosition = 0;
            StdItemList = new List<StdItem>();
            MonsterList = new List<TMonInfo>();
            MonGenList = new List<MonGenInfo>();
            MagicList = new List<TMagic>();
            MAdminList = new List<TAdminInfo>();
            MerchantList = new List<Merchant>();
            QuestNpcList = new List<NormNpc>();
            _mChangeServerList = new List<TSwitchDataInfo>();
            MagicEventList = new List<MagicEvent>();
            DwProcessMerchantTimeMin = 0;
            DwProcessMerchantTimeMax = 0;
            DwProcessNpcTimeMin = 0;
            DwProcessNpcTimeMax = 0;
            NewHumanList = new List<PlayObject>();
            _mListOfGateIdx = new List<int>();
            _mListOfSocket = new List<int>();
            _oldMagicList = new ArrayList();
            _OtherUserNameList = new Dictionary<string, ServerGruopInfo>(StringComparer.OrdinalIgnoreCase);
            _mUserLogonList = new List<RoBotLogon>();
            _BotPlayObjectList = new List<PlayObject>();
        }

        public int MonsterCount => _monsterCount;
        public int OnlinePlayObject => GetOnlineHumCount();
        public int PlayObjectCount => GetUserCount();
        public int LoadPlayCount => GetLoadPlayCount();

        public IEnumerable<PlayObject> PlayObjects => _PlayObjectList;

        public void Start()
        {
            _userEngineThread = new Timer(PrcocessData, null, 1000, 20);
            _processAiThread = new Thread(ProcessAiPlayObjectData) { IsBackground = true };
        }

        public void Stop()
        {
            _userEngineThread.Dispose();
            _processAiThread.Interrupt();
        }

        public void Initialize()
        {
            _logger.Info("正在初始化NPC脚本...");
            MerchantInitialize();
            NpCinitialize();
            _logger.Info("初始化NPC脚本完成...");
            for (var i = 0; i < MonGenList.Count; i++)
            {
                if (MonGenList[i] != null)
                    MonGenList[i].nRace = GetMonRace(MonGenList[i].sMonName);
            }
        }

        private int GetMonRace(string sMonName)
        {
            var result = -1;
            for (var i = 0; i < MonsterList.Count; i++)
            {
                var sName = MonsterList[i].sName;
                if (!sName.Equals(sMonName, StringComparison.OrdinalIgnoreCase)) continue;
                result = MonsterList[i].btRace;
                break;
            }
            return result;
        }

        private void MerchantInitialize()
        {
            Merchant merchant;
            for (var i = MerchantList.Count - 1; i >= 0; i--)
            {
                merchant = MerchantList[i];
                merchant.m_PEnvir = M2Share.MapManager.FindMap(merchant.m_sMapName);
                if (merchant.m_PEnvir != null)
                {
                    merchant.OnEnvirnomentChanged();
                    merchant.Initialize();
                    if (merchant.m_boAddtoMapSuccess && !merchant.m_boIsHide)
                    {
                        _logger.Warn("Merchant Initalize fail..." + merchant.m_sCharName + ' ' +
                                     merchant.m_sMapName + '(' +
                                     merchant.m_nCurrX + ':' + merchant.m_nCurrY + ')');
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
                    _logger.Error(merchant.m_sCharName + " - Merchant Initalize fail... (m.PEnvir=nil)");
                    MerchantList.RemoveAt(i);
                }
            }
        }

        private void NpCinitialize()
        {
            NormNpc normNpc;
            for (var i = QuestNpcList.Count - 1; i >= 0; i--)
            {
                normNpc = QuestNpcList[i];
                normNpc.m_PEnvir = M2Share.MapManager.FindMap(normNpc.m_sMapName);
                if (normNpc.m_PEnvir != null)
                {
                    normNpc.OnEnvirnomentChanged();
                    normNpc.Initialize();
                    if (normNpc.m_boAddtoMapSuccess && !normNpc.m_boIsHide)
                    {
                        _logger.Warn(normNpc.m_sCharName + " Npc Initalize fail... ");
                        QuestNpcList.RemoveAt(i);
                    }
                    else
                    {
                        normNpc.LoadNPCScript();
                    }
                }
                else
                {
                    _logger.Error(normNpc.m_sCharName + " Npc Initalize fail... (npc.PEnvir=nil) ");
                    QuestNpcList.RemoveAt(i);
                }
            }
        }

        private int GetLoadPlayCount()
        {
            return _LoadPlayList.Count;
        }

        private int GetOnlineHumCount()
        {
            return _PlayObjectList.Count + _BotPlayObjectList.Count;
        }

        private int GetUserCount()
        {
            return _PlayObjectList.Count + _BotPlayObjectList.Count;
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
                for (var i = 0; i < _PlayObjectList.Count; i++)
                {
                    if (string.Compare(_PlayObjectList[i].m_sCharName, sChrName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        private PlayObject ProcessHumans_MakeNewHuman(TUserOpenInfo userOpenInfo)
        {
            PlayObject result = null;
            PlayObject playObject = null;
            TSwitchDataInfo switchDataInfo = null;
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
                if (!M2Share.g_Config.boVentureServer)
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
                    playObject.m_btRaceServer = Grobal2.RC_PLAYOBJECT;
                    if (string.IsNullOrEmpty(playObject.m_sHomeMap))
                    {
                        playObject.m_sHomeMap = GetHomeInfo(playObject.m_btJob, ref playObject.m_nHomeX, ref playObject.m_nHomeY);
                        playObject.m_sMapName = playObject.m_sHomeMap;
                        playObject.m_nCurrX = GetRandHomeX(playObject);
                        playObject.m_nCurrY = GetRandHomeY(playObject);
                        if (playObject.m_Abil.Level == 0)
                        {
                            var abil = playObject.m_Abil;
                            abil.Level = 1;
                            abil.AC = 0;
                            abil.MAC = 0;
                            abil.DC = HUtil32.MakeLong(1, 2);
                            abil.MC = HUtil32.MakeLong(1, 2);
                            abil.SC = HUtil32.MakeLong(1, 2);
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
                    Envirnoment envir = M2Share.MapManager.GetMapInfo(M2Share.nServerIndex, playObject.m_sMapName);
                    if (envir != null)
                    {
                        playObject.m_sMapFileName = envir.MapFileName;
                        if (envir.Flag.boFight3Zone) // 是否在行会战争地图死亡
                        {
                            if (playObject.m_Abil.HP <= 0 && playObject.m_nFightZoneDieCount < 3)
                            {
                                playObject.m_Abil.HP = playObject.m_Abil.MaxHP;
                                playObject.m_Abil.MP = playObject.m_Abil.MaxMP;
                                playObject.m_boDieInFight3Zone = true;
                            }
                            else
                            {
                                playObject.m_nFightZoneDieCount = 0;
                            }
                        }
                    }
                    playObject.m_MyGuild = M2Share.GuildManager.MemberOfGuild(playObject.m_sCharName);
                    var userCastle = M2Share.CastleManager.InCastleWarArea(envir, playObject.m_nCurrX, playObject.m_nCurrY);
                    if (envir != null && userCastle != null && (userCastle.m_MapPalace == envir || userCastle.m_boUnderWar))
                    {
                        userCastle = M2Share.CastleManager.IsCastleMember(playObject);
                        if (userCastle == null)
                        {
                            playObject.m_sMapName = playObject.m_sHomeMap;
                            playObject.m_nCurrX = (short)(playObject.m_nHomeX - 2 + M2Share.RandomNumber.Random(5));
                            playObject.m_nCurrY = (short)(playObject.m_nHomeY - 2 + M2Share.RandomNumber.Random(5));
                        }
                        else
                        {
                            if (userCastle.m_MapPalace == envir)
                            {
                                playObject.m_sMapName = userCastle.GetMapName();
                                playObject.m_nCurrX = userCastle.GetHomeX();
                                playObject.m_nCurrY = userCastle.GetHomeY();
                            }
                        }
                    }
                    if (playObject.nC4 <= 1 && playObject.m_Abil.Level >= 1) playObject.nC4 = 2;
                    if (M2Share.MapManager.FindMap(playObject.m_sMapName) == null) playObject.m_Abil.HP = 0;
                    if (playObject.m_Abil.HP <= 0)
                    {
                        playObject.ClearStatusTime();
                        if (playObject.PKLevel() < 2)
                        {
                            userCastle = M2Share.CastleManager.IsCastleMember(playObject);
                            if (userCastle != null && userCastle.m_boUnderWar)
                            {
                                playObject.m_sMapName = userCastle.m_sHomeMap;
                                playObject.m_nCurrX = userCastle.GetHomeX();
                                playObject.m_nCurrY = userCastle.GetHomeY();
                            }
                            else
                            {
                                playObject.m_sMapName = playObject.m_sHomeMap;
                                playObject.m_nCurrX = (short)(playObject.m_nHomeX - 2 + M2Share.RandomNumber.Random(5));
                                playObject.m_nCurrY = (short)(playObject.m_nHomeY - 2 + M2Share.RandomNumber.Random(5));
                            }
                        }
                        else
                        {
                            playObject.m_sMapName = M2Share.g_Config.sRedDieHomeMap;// '3'
                            playObject.m_nCurrX = (short)(M2Share.RandomNumber.Random(13) + M2Share.g_Config.nRedDieHomeX);// 839
                            playObject.m_nCurrY = (short)(M2Share.RandomNumber.Random(13) + M2Share.g_Config.nRedDieHomeY);// 668
                        }
                        playObject.m_Abil.HP = 14;
                    }
                    playObject.AbilCopyToWAbil();
                    envir = M2Share.MapManager.GetMapInfo(M2Share.nServerIndex, playObject.m_sMapName);//切换其他服务器
                    if (envir == null)
                    {
                        playObject.m_nSessionID = userOpenInfo.LoadUser.nSessionID;
                        playObject.m_nSocket = userOpenInfo.LoadUser.nSocket;
                        playObject.m_nGateIdx = userOpenInfo.LoadUser.nGateIdx;
                        playObject.m_nGSocketIdx = userOpenInfo.LoadUser.nGSocketIdx;
                        playObject.m_WAbil = playObject.m_Abil;
                        playObject.m_nServerIndex = M2Share.MapManager.GetMapOfServerIndex(playObject.m_sMapName);
                        if (playObject.m_Abil.HP != 14)
                        {
                            _logger.Warn(string.Format(sChangeServerFail1, new object[] { M2Share.nServerIndex, playObject.m_nServerIndex, playObject.m_sMapName }));
                        }
                        SendSwitchData(playObject, playObject.m_nServerIndex);
                        SendChangeServer(playObject, (byte)playObject.m_nServerIndex);
                        playObject = null;
                        return result;
                    }
                    playObject.m_sMapFileName = envir.MapFileName;
                    var nC = 0;
                    while (true)
                    {
                        if (envir.CanWalk(playObject.m_nCurrX, playObject.m_nCurrY, true)) break;
                        playObject.m_nCurrX = (short)(playObject.m_nCurrX - 3 + M2Share.RandomNumber.Random(6));
                        playObject.m_nCurrY = (short)(playObject.m_nCurrY - 3 + M2Share.RandomNumber.Random(6));
                        nC++;
                        if (nC >= 5) break;
                    }
                    if (!envir.CanWalk(playObject.m_nCurrX, playObject.m_nCurrY, true))
                    {
                        _logger.Warn(string.Format(sChangeServerFail2,
                            new object[] { M2Share.nServerIndex, playObject.m_nServerIndex, playObject.m_sMapName }));
                        playObject.m_sMapName = M2Share.g_Config.sHomeMap;
                        envir = M2Share.MapManager.FindMap(M2Share.g_Config.sHomeMap);
                        playObject.m_nCurrX = M2Share.g_Config.nHomeX;
                        playObject.m_nCurrY = M2Share.g_Config.nHomeY;
                    }
                    playObject.m_PEnvir = envir;
                    playObject.OnEnvirnomentChanged();
                    if (playObject.m_PEnvir == null)
                    {
                        _logger.Error(sErrorEnvirIsNil);
                        goto ReGetMap;
                    }
                    else
                        playObject.m_boReadyRun = false;
                    playObject.m_sMapFileName = envir.MapFileName;
                }
                else
                {
                    GetHumData(playObject, ref userOpenInfo.HumanRcd);
                    playObject.m_sMapName = switchDataInfo.sMap;
                    playObject.m_nCurrX = switchDataInfo.wX;
                    playObject.m_nCurrY = switchDataInfo.wY;
                    playObject.m_Abil = switchDataInfo.Abil;
                    playObject.m_WAbil = switchDataInfo.Abil;
                    LoadSwitchData(switchDataInfo, ref playObject);
                    DelSwitchData(switchDataInfo);
                    Envirnoment envir = M2Share.MapManager.GetMapInfo(M2Share.nServerIndex, playObject.m_sMapName);
                    if (envir != null)
                    {
                        _logger.Warn(string.Format(sChangeServerFail3,
                            new object[] { M2Share.nServerIndex, playObject.m_nServerIndex, playObject.m_sMapName }));
                        playObject.m_sMapName = M2Share.g_Config.sHomeMap;
                        envir = M2Share.MapManager.FindMap(M2Share.g_Config.sHomeMap);
                        playObject.m_nCurrX = M2Share.g_Config.nHomeX;
                        playObject.m_nCurrY = M2Share.g_Config.nHomeY;
                    }
                    else
                    {
                        if (!envir.CanWalk(playObject.m_nCurrX, playObject.m_nCurrY, true))
                        {
                            _logger.Warn(string.Format(sChangeServerFail4,
                                new object[] { M2Share.nServerIndex, playObject.m_nServerIndex, playObject.m_sMapName }));
                            playObject.m_sMapName = M2Share.g_Config.sHomeMap;
                            envir = M2Share.MapManager.FindMap(M2Share.g_Config.sHomeMap);
                            playObject.m_nCurrX = M2Share.g_Config.nHomeX;
                            playObject.m_nCurrY = M2Share.g_Config.nHomeY;
                        }
                        playObject.AbilCopyToWAbil();
                        playObject.m_PEnvir = envir;
                        playObject.OnEnvirnomentChanged();
                        if (playObject.m_PEnvir == null)
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
            if ((HUtil32.GetTickCount() - _mDwProcessLoadPlayTick) > 200)
            {
                _mDwProcessLoadPlayTick = HUtil32.GetTickCount();
                try
                {
                    HUtil32.EnterCriticalSection(_LoadPlaySection);
                    try
                    {
                        for (var i = 0; i < _LoadPlayList.Count; i++)
                        {
                            TUserOpenInfo userOpenInfo;
                            if (!M2Share.FrontEngine.IsFull() && !ProcessHumansIsLogined(_LoadPlayList[i].sChrName))
                            {
                                userOpenInfo = _LoadPlayList[i];
                                playObject = ProcessHumans_MakeNewHuman(userOpenInfo);
                                if (playObject != null)
                                {
                                    if (playObject.m_boAI)
                                    {
                                        _BotPlayObjectList.Add(playObject);
                                    }
                                    else
                                    {
                                        _PlayObjectList.Add(playObject);
                                    }
                                    NewHumanList.Add(playObject);
                                    SendServerGroupMsg(Grobal2.ISM_USERLOGON, M2Share.nServerIndex, playObject.m_sCharName);
                                }
                            }
                            else
                            {
                                KickOnlineUser(_LoadPlayList[i].sChrName);
                                userOpenInfo = _LoadPlayList[i];
                                _mListOfGateIdx.Add(userOpenInfo.LoadUser.nGateIdx);
                                _mListOfSocket.Add(userOpenInfo.LoadUser.nSocket);
                            }
                            _LoadPlayList[i] = null;
                        }
                        _LoadPlayList.Clear();
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
                        HUtil32.LeaveCriticalSection(_LoadPlaySection);
                    }
                    for (var i = 0; i < NewHumanList.Count; i++)
                    {
                        playObject = NewHumanList[i];
                        M2Share.GateManager.SetGateUserList(playObject.m_nGateIdx, playObject.m_nSocket, playObject);
                    }
                    NewHumanList.Clear();
                    for (var i = 0; i < _mListOfGateIdx.Count; i++)
                    {
                        M2Share.GateManager.CloseUser(_mListOfGateIdx[i], _mListOfSocket[i]);
                    }
                    _mListOfGateIdx.Clear();
                    _mListOfSocket.Clear();
                }
                catch (Exception e)
                {
                    _logger.Error(sExceptionMsg1);
                    _logger.Error(e.Message);
                }
            }

            //人工智障开始登陆
            if (_mUserLogonList.Count > 0)
            {
                if (HUtil32.GetTickCount() - MDwAiLogonTick > 1000)
                {
                    MDwAiLogonTick = HUtil32.GetTickCount();
                    if (_mUserLogonList.Count > 0)
                    {
                        var roBot = _mUserLogonList[0];
                        RegenAiObject(roBot);
                        _mUserLogonList.RemoveAt(0);
                    }
                }
            }

            try
            {
                for (var i = 0; i < _PlayObjectFreeList.Count; i++)
                {
                    playObject = _PlayObjectFreeList[i];
                    if ((HUtil32.GetTickCount() - playObject.m_dwGhostTick) > M2Share.g_Config.dwHumanFreeDelayTime)// 5 * 60 * 1000
                    {
                        _PlayObjectFreeList[i] = null;
                        _PlayObjectFreeList.RemoveAt(i);
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
            _nProcessHumanLoopTime++;
            M2Share.g_nProcessHumanLoopTime = _nProcessHumanLoopTime;
            if (_procHumIdx == 0)
            {
                _nProcessHumanLoopTime = 0;
                M2Share.g_nProcessHumanLoopTime = _nProcessHumanLoopTime;
                var dwUsrRotTime = HUtil32.GetTickCount() - M2Share.g_dwUsrRotCountTick;
                M2Share.dwUsrRotCountMin = dwUsrRotTime;
                M2Share.g_dwUsrRotCountTick = HUtil32.GetTickCount();
                if (M2Share.dwUsrRotCountMax < dwUsrRotTime) M2Share.dwUsrRotCountMax = dwUsrRotTime;
            }
            M2Share.g_nHumCountMin = HUtil32.GetTickCount() - dwCheckTime;
            if (M2Share.g_nHumCountMax < M2Share.g_nHumCountMin) M2Share.g_nHumCountMax = M2Share.g_nHumCountMin;
        }

        private void ProcessAiPlayObjectData(object obj)
        {
            const string sExceptionMsg8 = "[Exception] TUserEngine::ProcessHumans";
            try
            {
                while (M2Share.boStartReady)
                {
                    var dwCurTick = HUtil32.GetTickCount();
                    var nIdx = _procBotHubIdx;
                    var boCheckTimeLimit = false;
                    var dwCheckTime = HUtil32.GetTickCount();
                    while (true)
                    {
                        if (_BotPlayObjectList.Count <= nIdx) break;
                        var playObject = _BotPlayObjectList[nIdx];
                        if (dwCurTick - playObject.m_dwRunTick > playObject.m_nRunTime)
                        {
                            playObject.m_dwRunTick = dwCurTick;
                            if (!playObject.m_boGhost)
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
                                        if ((HUtil32.GetTickCount() - playObject.m_dwSearchTick) > playObject.m_dwSearchTime)
                                        {
                                            playObject.m_dwSearchTick = HUtil32.GetTickCount();
                                            playObject.SearchViewRange();
                                            playObject.GameTimeChanged();
                                        }
                                        playObject.Run();
                                    }
                                }
                            }
                            else
                            {
                                _BotPlayObjectList.Remove(playObject);
                                playObject.Disappear();
                                AddToHumanFreeList(playObject);
                                playObject.DealCancelA();
                                SaveHumanRcd(playObject);
                                M2Share.GateManager.CloseUser(playObject.m_nGateIdx, playObject.m_nSocket);
                                SendServerGroupMsg(Grobal2.SS_202, M2Share.nServerIndex, playObject.m_sCharName);
                                continue;
                            }
                        }
                        nIdx++;
                        if ((HUtil32.GetTickCount() - dwCheckTime) > M2Share.g_dwHumLimit)
                        {
                            boCheckTimeLimit = true;
                            _procBotHubIdx = nIdx;
                            break;
                        }
                    }
                    if (!boCheckTimeLimit) _procBotHubIdx = 0;

                    Thread.Sleep(30);
                }
            }
            catch (Exception ex)
            {
               _logger.Error(sExceptionMsg8);
               _logger.Error(ex.StackTrace);
            }
        }

        private void ProcessPlayObjectData()
        {
            try
            {
                var dwCurTick = HUtil32.GetTickCount();
                var nIdx = _procHumIdx;
                var boCheckTimeLimit = false;
                var dwCheckTime = HUtil32.GetTickCount();
                while (true)
                {
                    if (_PlayObjectList.Count <= nIdx) break;
                    var playObject = _PlayObjectList[nIdx];
                    if (playObject == null)
                    {
                        continue;
                    }
                    if ((dwCurTick - playObject.m_dwRunTick) > playObject.m_nRunTime)
                    {
                        playObject.m_dwRunTick = dwCurTick;
                        if (!playObject.m_boGhost)
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
                                    if ((HUtil32.GetTickCount() - playObject.m_dwSearchTick) > playObject.m_dwSearchTime)
                                    {
                                        playObject.m_dwSearchTick = HUtil32.GetTickCount();
                                        playObject.SearchViewRange();//搜索对像
                                        playObject.GameTimeChanged();//游戏时间改变
                                    }
                                    if ((HUtil32.GetTickCount() - playObject.m_dwShowLineNoticeTick) > M2Share.g_Config.dwShowLineNoticeTime)
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
                                                    playObject.SysMsg(lineNoticeMsg, (MsgColor)M2Share.g_Config.nLineNoticeColor, MsgType.Notice);
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
                                    if (!M2Share.FrontEngine.IsFull() && (HUtil32.GetTickCount() - playObject.m_dwSaveRcdTick) > M2Share.g_Config.dwSaveHumanRcdTime)
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
                            _PlayObjectList.Remove(playObject);
                            playObject.Disappear();
                            AddToHumanFreeList(playObject);
                            playObject.DealCancelA();
                            SaveHumanRcd(playObject);
                            M2Share.GateManager.CloseUser(playObject.m_nGateIdx, playObject.m_nSocket);
                            SendServerGroupMsg(Grobal2.ISM_USERLOGOUT, M2Share.nServerIndex, playObject.m_sCharName);
                            continue;
                        }
                    }
                    nIdx++;
                    if ((HUtil32.GetTickCount() - dwCheckTime) > M2Share.g_dwHumLimit)
                    {
                        boCheckTimeLimit = true;
                        _procHumIdx = nIdx;
                        break;
                    }
                }
                if (!boCheckTimeLimit) _procHumIdx = 0;
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
                    if (!merchantNpc.m_boGhost)
                    {
                        if ((dwCurrTick - merchantNpc.m_dwRunTick) > merchantNpc.m_nRunTime)
                        {
                            if ((HUtil32.GetTickCount() - merchantNpc.m_dwSearchTick) > merchantNpc.m_dwSearchTime)
                            {
                                merchantNpc.m_dwSearchTick = HUtil32.GetTickCount();
                                merchantNpc.SearchViewRange();
                            }
                            if ((dwCurrTick - merchantNpc.m_dwRunTick) > merchantNpc.m_nRunTime)
                            {
                                merchantNpc.m_dwRunTick = dwCurrTick;
                                merchantNpc.Run();
                            }
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - merchantNpc.m_dwGhostTick) > 60 * 1000)
                        {
                            merchantNpc = null;
                            MerchantList.RemoveAt(i);
                            break;
                        }
                    }
                    if ((HUtil32.GetTickCount() - dwRunTick) > M2Share.g_dwNpcLimit)
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
            DwProcessMerchantTimeMin = HUtil32.GetTickCount() - dwRunTick;
            if (DwProcessMerchantTimeMin > DwProcessMerchantTimeMax)
            {
                DwProcessMerchantTimeMax = DwProcessMerchantTimeMin;
            }
            if (DwProcessNpcTimeMin > DwProcessNpcTimeMax)
            {
                DwProcessNpcTimeMax = DwProcessNpcTimeMin;
            }
        }

        private void ProcessMissions()
        {

        }

        public int ProcessMonsters_GetZenTime(int dwTime)
        {
            int result;
            if (dwTime < 30 * 60 * 1000)
            {
                var d10 = (PlayObjectCount - M2Share.g_Config.nUserFull) / HUtil32._MAX(1, M2Share.g_Config.nZenFastStep);
                if (d10 > 0)
                {
                    if (d10 > 6) d10 = 6;
                    result = (int)(dwTime - Math.Round(dwTime / 10 * (double)d10));
                }
                else
                {
                    result = dwTime;
                }
            }
            else
            {
                result = dwTime;
            }
            return result;
        }

        private void ProcessMonsters()
        {
            bool boCanCreate;
            var dwRunTick = HUtil32.GetTickCount();
            AnimalObject monster = null;
            try
            {
                var boProcessLimit = false;
                var dwCurrentTick = HUtil32.GetTickCount();
                MonGenInfo monGen = null;
                // 刷新怪物开始
                if ((HUtil32.GetTickCount() - _dwRegenMonstersTick) > M2Share.g_Config.dwRegenMonstersTime)
                {
                    _dwRegenMonstersTick = HUtil32.GetTickCount();
                    if (_currMonGenIdx < MonGenList.Count)
                    {
                        monGen = MonGenList[_currMonGenIdx];
                    }
                    else if (MonGenList.Count > 0)
                    {
                        monGen = MonGenList[0];
                    }
                    if (_currMonGenIdx < MonGenList.Count - 1)
                    {
                        _currMonGenIdx++;
                    }
                    else
                    {
                        _currMonGenIdx = 0;
                    }
                    if (monGen != null && !string.IsNullOrEmpty(monGen.sMonName) && !M2Share.g_Config.boVentureServer)
                    {
                        var nTemp = HUtil32.GetTickCount() - monGen.dwStartTick;
                        if (monGen.dwStartTick == 0 || nTemp > ProcessMonsters_GetZenTime(monGen.dwZenTime))
                        {
                            var nGenCount = monGen.nActiveCount; //取已刷出来的怪数量
                            var boRegened = true;
                            var nGenModCount = (monGen.nCount / M2Share.g_Config.nMonGenRate) * 10;
                            var map = M2Share.MapManager.FindMap(monGen.sMapName);
                            if (map == null || map.Flag.boNOHUMNOMON && map.HumCount <= 0)
                                boCanCreate = false;
                            else
                                boCanCreate = true;
                            if (nGenModCount > nGenCount && boCanCreate)// 增加 控制刷怪数量比例
                            {
                                boRegened = RegenMonsters(monGen, nGenModCount - nGenCount);
                            }
                            if (boRegened)
                            {
                                monGen.dwStartTick = HUtil32.GetTickCount();
                            }
                        }
                    }
                }
                // 刷新怪物结束
                var dwMonProcTick = HUtil32.GetTickCount();

                MonsterProcessCount = 0;
                var i = 0;
                for (i = _monGenListPosition; i < MonGenList.Count; i++)
                {
                    monGen = MonGenList[i];
                    int nProcessPosition;
                    if (_monGenCertListPosition < monGen.CertList.Count)
                        nProcessPosition = _monGenCertListPosition;
                    else
                        nProcessPosition = 0;
                    _monGenCertListPosition = 0;
                    while (true)
                    {
                        if (nProcessPosition >= monGen.CertList.Count)
                        {
                            break;
                        }
                        monster = (AnimalObject)monGen.CertList[nProcessPosition];
                        if (monster != null)
                        {
                            if (!monster.m_boGhost)
                            {
                                if ((dwCurrentTick - monster.m_dwRunTick) > monster.m_nRunTime)
                                {
                                    monster.m_dwRunTick = dwRunTick;
                                    if (monster.m_boDeath && monster.m_boCanReAlive && monster.m_boInvisible && (monster.m_pMonGen != null))
                                    {
                                        if ((HUtil32.GetTickCount() - monster.m_dwReAliveTick) > M2Share.UserEngine.ProcessMonsters_GetZenTime(monster.m_pMonGen.dwZenTime))
                                        {
                                            if (monster.ReAliveEx(monster.m_pMonGen))
                                            {
                                                monster.m_dwReAliveTick = HUtil32.GetTickCount();
                                            }
                                        }
                                    }
                                    if (!monster.m_boIsVisibleActive && (monster.m_nProcessRunCount < M2Share.g_Config.nProcessMonsterInterval))
                                    {
                                        monster.m_nProcessRunCount++;
                                    }
                                    else
                                    {
                                        if ((dwCurrentTick - monster.m_dwSearchTick) > monster.m_dwSearchTime)
                                        {
                                            monster.m_dwSearchTick = HUtil32.GetTickCount();
                                            if (!monster.m_boDeath)
                                            {
                                                monster.SearchViewRange();
                                            }
                                            else
                                            {
                                                monster.SearchViewRangeDeath();
                                            }
                                        }
                                        monster.m_nProcessRunCount = 0;
                                        monster.Run();
                                    }
                                }
                                MonsterProcessPostion++;
                            }
                            else
                            {
                                if ((HUtil32.GetTickCount() - monster.m_dwGhostTick) > 5 * 60 * 1000)
                                {
                                    monGen.CertList.RemoveAt(nProcessPosition);
                                    monGen.CertCount--;
                                    monster = null;
                                    continue;
                                }
                            }
                        }
                        nProcessPosition++;
                        if ((HUtil32.GetTickCount() - dwMonProcTick) > M2Share.g_dwMonLimit)
                        {
                            boProcessLimit = true;
                            _monGenCertListPosition = nProcessPosition;
                            break;
                        }
                    }
                    if (boProcessLimit) break;
                }
                if (MonGenList.Count <= i)
                {
                    _monGenListPosition = 0;
                    _monsterCount = MonsterProcessPostion;
                    MonsterProcessPostion = 0;
                }
                if (!boProcessLimit)
                    _monGenListPosition = 0;
                else
                    _monGenListPosition = i;
            }
            catch (Exception e)
            {
                _logger.Error(e.StackTrace);
            }
        }

        /// <summary>
        /// 获取刷怪数量
        /// </summary>
        /// <param name="monGen"></param>
        /// <returns></returns>
        private int GetGenMonCount(MonGenInfo monGen)
        {
            var nCount = 0;
            TBaseObject baseObject;
            for (var i = 0; i < monGen.CertList.Count; i++)
            {
                baseObject = monGen.CertList[i];
                if (!baseObject.m_boDeath && !baseObject.m_boGhost)
                {
                    nCount++;
                }
            }
            return nCount;
        }

        private void ProcessNpcs()
        {
            NormNpc npc;
            var dwRunTick = HUtil32.GetTickCount();
            var boProcessLimit = false;
            try
            {
                var dwCurrTick = HUtil32.GetTickCount();
                for (var i = NpcPosition; i < QuestNpcList.Count; i++)
                {
                    npc = QuestNpcList[i];
                    if (!npc.m_boGhost)
                    {
                        if ((dwCurrTick - npc.m_dwRunTick) > npc.m_nRunTime)
                        {
                            if ((HUtil32.GetTickCount() - npc.m_dwSearchTick) > npc.m_dwSearchTime)
                            {
                                npc.m_dwSearchTick = HUtil32.GetTickCount();
                                npc.SearchViewRange();
                            }
                            if ((dwCurrTick - npc.m_dwRunTick) > npc.m_nRunTime)
                            {
                                npc.m_dwRunTick = dwCurrTick;
                                npc.Run();
                            }
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - npc.m_dwGhostTick) > 60 * 1000)
                        {
                            QuestNpcList.RemoveAt(i);
                            break;
                        }
                    }
                    if ((HUtil32.GetTickCount() - dwRunTick) > M2Share.g_dwNpcLimit)
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
            DwProcessNpcTimeMin = HUtil32.GetTickCount() - dwRunTick;
            if (DwProcessNpcTimeMin > DwProcessNpcTimeMax) DwProcessNpcTimeMax = DwProcessNpcTimeMin;
        }

        public TBaseObject RegenMonsterByName(string sMap, short nX, short nY, string sMonName)
        {
            TBaseObject result;
            var nRace = GetMonRace(sMonName);
            var baseObject = AddBaseObject(sMap, nX, nY, nRace, sMonName);
            if (baseObject != null)
            {
                var n18 = MonGenList.Count - 1;
                if (n18 < 0) n18 = 0;
                if (MonGenList.Count > n18)
                {
                    var monGen = MonGenList[n18];
                    monGen.CertList.Add(baseObject);
                    monGen.CertCount++;
                }
                baseObject.m_PEnvir.AddObject(baseObject);
                baseObject.m_boAddToMaped = true;
            }
            result = baseObject;
            return result;
        }

        public void Run()
        {
            const string sExceptionMsg = "[Exception] TUserEngine::Run";
            try
            {
                if ((HUtil32.GetTickCount() - _dwShowOnlineTick) > M2Share.g_Config.dwConsoleShowUserCountTime)
                {
                    _dwShowOnlineTick = HUtil32.GetTickCount();
                    M2Share.NoticeManager.LoadingNotice();
                    _logger.Info("在线数: " + PlayObjectCount);
                    M2Share.CastleManager.Save();
                }
                if ((HUtil32.GetTickCount() - _dwSendOnlineHumTime) > 10000)
                {
                    _dwSendOnlineHumTime = HUtil32.GetTickCount();
                    IdSrvClient.Instance.SendOnlineHumCountMsg(OnlinePlayObject);
                }
            }
            catch (Exception e)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(e.Message);
            }
        }

        public StdItem GetStdItem(int nItemIdx)
        {
            StdItem result = null;
            nItemIdx -= 1;
            if (nItemIdx >= 0 && StdItemList.Count > nItemIdx)
            {
                result = StdItemList[nItemIdx];
                if (result.Name == "") result = null;
            }
            return result;
        }

        public StdItem GetStdItem(string sItemName)
        {
            StdItem result = null;
            StdItem stdItem = null;
            if (string.IsNullOrEmpty(sItemName)) return result;
            for (var i = 0; i < StdItemList.Count; i++)
            {
                stdItem = StdItemList[i];
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
            if (_OtherUserNameList.TryGetValue(sName, out var groupServer))
            {
                nServerIndex = groupServer.nServerIdx;
                Console.WriteLine($"玩家在[{nServerIndex}]服务器上.");
                return true;
            }
            return false;
        }

        public void CryCry(short wIdent, Envirnoment pMap, int nX, int nY, int nWide, byte btFColor, byte btBColor, string sMsg)
        {
            PlayObject playObject;
            for (var i = 0; i < _PlayObjectList.Count; i++)
            {
                playObject = _PlayObjectList[i];
                if (!playObject.m_boGhost && playObject.m_PEnvir == pMap && playObject.m_boBanShout &&
                    Math.Abs(playObject.m_nCurrX - nX) < nWide && Math.Abs(playObject.m_nCurrY - nY) < nWide)
                    playObject.SendMsg(null, wIdent, 0, btFColor, btBColor, 0, sMsg);
            }
        }

        /// <summary>
        /// 计算怪物掉落物品
        /// 即创建怪物对象的时候已经算好要掉落的物品和属性
        /// </summary>
        /// <returns></returns>
        private void MonGetRandomItems(TBaseObject mon)
        {
            IList<TMonItem> itemList = null;
            var iname = string.Empty;
            for (var i = 0; i < MonsterList.Count; i++)
            {
                var monster = MonsterList[i];
                if (string.Compare(monster.sName, mon.m_sCharName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    itemList = monster.ItemList;
                    break;
                }
            }
            if (itemList != null)
            {
                for (var i = 0; i < itemList.Count; i++)
                {
                    var monItem = itemList[i];
                    if (M2Share.RandomNumber.Random(monItem.MaxPoint) <= monItem.SelPoint)
                    {
                        if (string.Compare(monItem.ItemName, Grobal2.sSTRING_GOLDNAME, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            mon.m_nGold = mon.m_nGold + monItem.Count / 2 + M2Share.RandomNumber.Random(monItem.Count);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(iname)) iname = monItem.ItemName;
                            TUserItem userItem = null;
                            if (CopyToUserItemFromName(iname, ref userItem))
                            {
                                userItem.Dura = (ushort)HUtil32.Round(userItem.DuraMax / 100 * (20 + M2Share.RandomNumber.Random(80)));
                                var stdItem = GetStdItem(userItem.wIndex);
                                if (stdItem == null) continue;
                                if (M2Share.RandomNumber.Random(M2Share.g_Config.nMonRandomAddValue) == 0)
                                {
                                    stdItem.RandomUpgradeItem(userItem);
                                }
                                if (new ArrayList(new byte[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(stdItem.StdMode))
                                {
                                    if (stdItem.Shape == 130 || stdItem.Shape == 131 || stdItem.Shape == 132)
                                    {
                                        stdItem.RandomUpgradeUnknownItem(userItem);
                                    }
                                }
                                mon.m_ItemList.Add(userItem);
                            }
                        }
                    }
                }
            }
        }

        public bool CopyToUserItemFromName(string sItemName, ref TUserItem item)
        {
            if (string.IsNullOrEmpty(sItemName)) return false;
            for (var i = 0; i < StdItemList.Count; i++)
            {
                var stdItem = StdItemList[i];
                if (!stdItem.Name.Equals(sItemName, StringComparison.OrdinalIgnoreCase)) continue;
                if (item == null) item = new TUserItem();
                item.wIndex = (ushort)(i + 1);
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
            if (playObject.m_boOffLineFlag) return;
            if (!string.IsNullOrEmpty(buff)) sMsg = buff;
            switch (defMsg.Ident)
            {
                case Grobal2.CM_SPELL:
                    if (M2Share.g_Config.boSpellSendUpdateMsg) // 使用UpdateMsg 可以防止消息队列里有多个操作
                    {
                        playObject.SendUpdateMsg(playObject, defMsg.Ident, defMsg.Tag, HUtil32.LoWord(defMsg.Recog),
                            HUtil32.HiWord(defMsg.Recog), HUtil32.MakeLong(defMsg.Param, defMsg.Series), "");
                    }
                    else
                    {
                        playObject.SendMsg(playObject, defMsg.Ident, defMsg.Tag, HUtil32.LoWord(defMsg.Recog),
                            HUtil32.HiWord(defMsg.Recog), HUtil32.MakeLong(defMsg.Param, defMsg.Series), "");
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
                    if (M2Share.g_Config.boActionSendActionMsg) // 使用UpdateMsg 可以防止消息队列里有多个操作
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
                    playObject.m_dwRunTick -= 100;
                    break;
            }
        }

        public void SendServerGroupMsg(int nCode, int nServerIdx, string sMsg)
        {
            if (M2Share.nServerIndex == 0)
            {
                SnapsmService.Instance.SendServerSocket(nCode + "/" + nServerIdx + "/" + sMsg);
            }
            else
            {
                SnapsmClient.Instance.SendSocket(nCode + "/" + nServerIdx + "/" + sMsg);
            }
        }

        public void GetIsmChangeServerReceive(string flName)
        {
            PlayObject hum;
            for (var i = 0; i < _PlayObjectFreeList.Count; i++)
            {
                hum = _PlayObjectFreeList[i];
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
            _OtherUserNameList.Remove(name);
            _OtherUserNameList.Add(name, new ServerGruopInfo()
            {
                nServerIdx = sNum,
                sCharName = uname
            });
        }

        public void OtherServerUserLogout(int sNum, string uname)
        {
            var name = string.Empty;
            var apmode = HUtil32.GetValidStr3(uname, ref name, ":");
            _OtherUserNameList.Remove(name);
            // for (var i = m_OtherUserNameList.Count - 1; i >= 0; i--)
            // {
            //     if (string.Compare(m_OtherUserNameList[i].sCharName, Name, StringComparison.OrdinalIgnoreCase) == 0 && m_OtherUserNameList[i].nServerIdx == sNum)
            //     {
            //         m_OtherUserNameList.RemoveAt(i);
            //         break;
            //     }
            // }
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <returns></returns>
        private TBaseObject AddBaseObject(string sMapName, short nX, short nY, int nMonRace, string sMonName)
        {
            TBaseObject result = null;
            TBaseObject cert = null;
            int n1C;
            int n20;
            int n24;
            object p28;
            var map = M2Share.MapManager.FindMap(sMapName);
            if (map == null) return result;
            switch (nMonRace)
            {
                case MonsterConst.SUPREGUARD:
                    cert = new SuperGuard();
                    break;
                case MonsterConst.PETSUPREGUARD:
                    cert = new PetSuperGuard();
                    break;
                case MonsterConst.ARCHER_POLICE:
                    cert = new ArcherPolice();
                    break;
                case MonsterConst.ANIMAL_CHICKEN:
                    cert = new MonsterObject
                    {
                        m_boAnimal = true,
                        m_nMeatQuality = (ushort)(M2Share.RandomNumber.Random(3500) + 3000),
                        m_nBodyLeathery = 50
                    };
                    break;
                case MonsterConst.ANIMAL_DEER:
                    if (M2Share.RandomNumber.Random(30) == 0)
                        cert = new ChickenDeer
                        {
                            m_boAnimal = true,
                            m_nMeatQuality = (ushort)(M2Share.RandomNumber.Random(20000) + 10000),
                            m_nBodyLeathery = 150
                        };
                    else
                        cert = new MonsterObject()
                        {
                            m_boAnimal = true,
                            m_nMeatQuality = (ushort)(M2Share.RandomNumber.Random(8000) + 8000),
                            m_nBodyLeathery = 150
                        };
                    break;
                case MonsterConst.ANIMAL_WOLF:
                    cert = new AtMonster
                    {
                        m_boAnimal = true,
                        m_nMeatQuality = (ushort)(M2Share.RandomNumber.Random(8000) + 8000),
                        m_nBodyLeathery = 150
                    };
                    break;
                case MonsterConst.TRAINER:
                    cert = new Trainer();
                    break;
                case MonsterConst.MONSTER_OMA:
                    cert = new MonsterObject();
                    break;
                case MonsterConst.MONSTER_OMAKNIGHT:
                    cert = new AtMonster();
                    break;
                case MonsterConst.MONSTER_SPITSPIDER:
                    cert = new SpitSpider();
                    break;
                case 83:
                    cert = new SlowAtMonster();
                    break;
                case 84:
                    cert = new Scorpion();
                    break;
                case MonsterConst.MONSTER_STICK:
                    cert = new StickMonster();
                    break;
                case 86:
                    cert = new AtMonster();
                    break;
                case MonsterConst.MONSTER_DUALAXE:
                    cert = new DualAxeMonster();
                    break;
                case 88:
                    cert = new AtMonster();
                    break;
                case 89:
                    cert = new AtMonster();
                    break;
                case 90:
                    cert = new GasAttackMonster();
                    break;
                case 91:
                    cert = new MagCowMonster();
                    break;
                case 92:
                    cert = new CowKingMonster();
                    break;
                case MonsterConst.MONSTER_THONEDARK:
                    cert = new ThornDarkMonster();
                    break;
                case MonsterConst.MONSTER_LIGHTZOMBI:
                    cert = new LightingZombi();
                    break;
                case MonsterConst.MONSTER_DIGOUTZOMBI:
                    cert = new DigOutZombi();
                    if (M2Share.RandomNumber.Random(2) == 0) cert.bo2BA = true;
                    break;
                case MonsterConst.MONSTER_ZILKINZOMBI:
                    cert = new ZilKinZombi();
                    if (M2Share.RandomNumber.Random(4) == 0) cert.bo2BA = true;
                    break;
                case 97:
                    cert = new TCowMonster();
                    if (M2Share.RandomNumber.Random(2) == 0) cert.bo2BA = true;
                    break;
                case MonsterConst.MONSTER_WHITESKELETON:
                    cert = new WhiteSkeleton();
                    break;
                case MonsterConst.MONSTER_SCULTURE:
                    cert = new ScultureMonster
                    {
                        bo2BA = true
                    };
                    break;
                case MonsterConst.MONSTER_SCULTUREKING:
                    cert = new ScultureKingMonster();
                    break;
                case MonsterConst.MONSTER_BEEQUEEN:
                    cert = new BeeQueen();
                    break;
                case 104:
                    cert = new ArcherMonster();
                    break;
                case 105:
                    cert = new GasMothMonster();
                    break;
                case 106: // 楔蛾
                    cert = new GasDungMonster();
                    break;
                case 107:
                    cert = new CentipedeKingMonster();
                    break;
                case 110:
                    cert = new CastleDoor();
                    break;
                case 111:
                    cert = new WallStructure();
                    break;
                case MonsterConst.MONSTER_ARCHERGUARD:
                    cert = new ArcherGuard();
                    break;
                case MonsterConst.MONSTER_ELFMONSTER:
                    cert = new ElfMonster();
                    break;
                case MonsterConst.MONSTER_ELFWARRIOR:
                    cert = new ElfWarriorMonster();
                    break;
                case 115:
                    cert = new BigHeartMonster();
                    break;
                case 116:
                    cert = new SpiderHouseMonster();
                    break;
                case 117:
                    cert = new ExplosionSpider();
                    break;
                case 118:
                    cert = new HighRiskSpider();
                    break;
                case 119:
                    cert = new BigPoisionSpider();
                    break;
                case 120:
                    cert = new SoccerBall();
                    break;
                case 130:
                    cert = new DoubleCriticalMonster();
                    break;
                case 131:
                    cert = new RonObject();
                    break;
                case 132:
                    cert = new SandMobObject();
                    break;
                case 133:
                    cert = new MagicMonObject();
                    break;
                case 134:
                    cert = new BoneKingMonster();
                    break;
                case 200:
                    cert = new ElectronicScolpionMon();
                    break;
                case 201:
                    cert = new CloneMonster();
                    break;
                case 203:
                    cert = new TeleMonster();
                    break;
                case 206:
                    cert = new Khazard();
                    break;
                case 208:
                    cert = new GreenMonster();
                    break;
                case 209:
                    cert = new RedMonster();
                    break;
                case 210:
                    cert = new FrostTiger();
                    break;
                case 214:
                    cert = new FireMonster();
                    break;
                case 215:
                    cert = new FireballMonster();
                    break;
            }

            if (cert != null)
            {
                MonInitialize(cert, sMonName);
                cert.m_PEnvir = map;
                cert.m_sMapName = sMapName;
                cert.m_nCurrX = nX;
                cert.m_nCurrY = nY;
                cert.Direction = M2Share.RandomNumber.RandomByte(8);
                cert.m_sCharName = sMonName;
                cert.m_WAbil = cert.m_Abil;
                cert.OnEnvirnomentChanged();
                if (M2Share.RandomNumber.Random(100) < cert.m_btCoolEye) cert.m_boCoolEye = true;
                MonGetRandomItems(cert);
                cert.Initialize();
                if (cert.m_boAddtoMapSuccess)
                {
                    p28 = null;
                    if (cert.m_PEnvir.Width < 50)
                        n20 = 2;
                    else
                        n20 = 3;
                    if (cert.m_PEnvir.Height < 250)
                    {
                        if (cert.m_PEnvir.Height < 30)
                            n24 = 2;
                        else
                            n24 = 20;
                    }
                    else
                    {
                        n24 = 50;
                    }

                    n1C = 0;
                    while (true)
                    {
                        if (!cert.m_PEnvir.CanWalk(cert.m_nCurrX, cert.m_nCurrY, false))
                        {
                            if (cert.m_PEnvir.Width - n24 - 1 > cert.m_nCurrX)
                            {
                                cert.m_nCurrX += (short)n20;
                            }
                            else
                            {
                                cert.m_nCurrX = (short)(M2Share.RandomNumber.Random(cert.m_PEnvir.Width / 2) + n24);
                                if (cert.m_PEnvir.Height - n24 - 1 > cert.m_nCurrY)
                                    cert.m_nCurrY += (short)n20;
                                else
                                    cert.m_nCurrY =
                                        (short)(M2Share.RandomNumber.Random(cert.m_PEnvir.Height / 2) + n24);
                            }
                        }
                        else
                        {
                            p28 = cert.m_PEnvir.AddToMap(cert.m_nCurrX, cert.m_nCurrY, CellType.MovingObject, cert);
                            break;
                        }

                        n1C++;
                        if (n1C >= 31) break;
                    }

                    if (p28 == null)
                        //Cert.Free;
                        cert = null;
                }
            }

            result = cert;
            return result;
        }

        /// <summary>
        /// 创建怪物对象
        /// 在指定时间内创建完对象，则返加TRUE，如果超过指定时间则返回FALSE
        /// </summary>
        /// <returns></returns>
        private bool RegenMonsters(MonGenInfo monGen, int nCount)
        {
            TBaseObject cert;
            const string sExceptionMsg = "[Exception] TUserEngine::RegenMonsters";
            var result = true;
            var dwStartTick = HUtil32.GetTickCount();
            try
            {
                if (monGen.nRace > 0)
                {
                    short nX;
                    short nY;
                    if (M2Share.RandomNumber.Random(100) < monGen.nMissionGenRate)
                    {
                        nX = (short)(monGen.nX - monGen.nRange + M2Share.RandomNumber.Random(monGen.nRange * 2 + 1));
                        nY = (short)(monGen.nY - monGen.nRange + M2Share.RandomNumber.Random(monGen.nRange * 2 + 1));
                        for (var i = 0; i < nCount; i++)
                        {
                            cert = AddBaseObject(monGen.sMapName, (short)(nX - 10 + M2Share.RandomNumber.Random(20)),
                                (short)(nY - 10 + M2Share.RandomNumber.Random(20)), monGen.nRace, monGen.sMonName);
                            if (cert != null)
                            {
                                cert.m_boCanReAlive = true;
                                cert.m_dwReAliveTick = HUtil32.GetTickCount();
                                cert.m_pMonGen = monGen;
                                monGen.nActiveCount++;
                                monGen.CertList.Add(cert);
                            }
                            if ((HUtil32.GetTickCount() - dwStartTick) > M2Share.g_dwZenLimit)
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (var i = 0; i < nCount; i++)
                        {
                            nX = (short)(monGen.nX - monGen.nRange + M2Share.RandomNumber.Random(monGen.nRange * 2 + 1));
                            nY = (short)(monGen.nY - monGen.nRange + M2Share.RandomNumber.Random(monGen.nRange * 2 + 1));
                            cert = AddBaseObject(monGen.sMapName, nX, nY, monGen.nRace, monGen.sMonName);
                            if (cert != null)
                            {
                                cert.m_boCanReAlive = true;
                                cert.m_dwReAliveTick = HUtil32.GetTickCount();
                                cert.m_pMonGen = monGen;
                                monGen.nActiveCount++;
                                monGen.CertList.Add(cert);
                            }
                            if (HUtil32.GetTickCount() - dwStartTick > M2Share.g_dwZenLimit)
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                _logger.Error(sExceptionMsg);
            }
            return result;
        }

        public PlayObject GetPlayObject(string sName)
        {
            PlayObject result = null;
            for (var i = 0; i < _PlayObjectList.Count; i++)
            {
                if (string.Compare(_PlayObjectList[i].m_sCharName, sName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    PlayObject playObject = _PlayObjectList[i];
                    if (!playObject.m_boGhost)
                    {
                        if (!(playObject.m_boPasswordLocked && playObject.m_boObMode && playObject.m_boAdminMode))
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
                for (var i = 0; i < _PlayObjectList.Count; i++)
                {
                    if (string.Compare(_PlayObjectList[i].m_sCharName, sName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        playObject = _PlayObjectList[i];
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
                for (var i = 0; i < _PlayObjectList.Count; i++)
                {
                    if (string.Compare(_PlayObjectList[i].m_sCharName, sName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = _PlayObjectList[i];
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
            if (npcType == typeof(TGuildOfficial))
            {
                npcObject = (TGuildOfficial)Convert.ChangeType(normNpc, typeof(TGuildOfficial));
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
            PlayObject playObject;
            for (var i = 0; i < _PlayObjectList.Count; i++)
            {
                playObject = _PlayObjectList[i];
                if (!playObject.m_boGhost && playObject.m_PEnvir == envir)
                {
                    if (Math.Abs(playObject.m_nCurrX - nX) < nRange && Math.Abs(playObject.m_nCurrY - nY) < nRange)
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
            TAdminInfo adminInfo;
            btPermission = (byte)M2Share.g_Config.nStartPermission;
            for (var i = 0; i < MAdminList.Count; i++)
            {
                adminInfo = MAdminList[i];
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

        public void AddUserOpenInfo(TUserOpenInfo userOpenInfo)
        {
            HUtil32.EnterCriticalSection(_LoadPlaySection);
            try
            {
                _LoadPlayList.Add(userOpenInfo);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(_LoadPlaySection);
            }
        }

        private void KickOnlineUser(string sChrName)
        {
            PlayObject playObject;
            for (var i = 0; i < _PlayObjectList.Count; i++)
            {
                playObject = _PlayObjectList[i];
                if (string.Compare(playObject.m_sCharName, sChrName, StringComparison.OrdinalIgnoreCase) == 0)
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
            if (playObject.m_boAI) //AI玩家不需要保存数据
            {
                return;
            }
            var saveRcd = new TSaveRcd
            {
                sAccount = playObject.m_sUserID,
                sChrName = playObject.m_sCharName,
                nSessionID = playObject.m_nSessionID,
                PlayObject = playObject,
                HumanRcd = new THumDataInfo()
            };
            saveRcd.HumanRcd.Data.Initialization();
            playObject.MakeSaveRcd(ref saveRcd.HumanRcd);
            M2Share.FrontEngine.AddToSaveRcdList(saveRcd);
        }

        private void AddToHumanFreeList(PlayObject playObject)
        {
            playObject.m_dwGhostTick = HUtil32.GetTickCount();
            _PlayObjectFreeList.Add(playObject);
        }

        private void GetHumData(PlayObject playObject, ref THumDataInfo humanRcd)
        {
            THumInfoData humData;
            TUserItem[] humItems;
            TUserItem[] bagItems;
            TMagicRcd[] humMagic;
            TMagic magicInfo;
            TUserMagic userMagic;
            TUserItem[] storageItems;
            TUserItem userItem;
            humData = humanRcd.Data;
            playObject.m_sCharName = humData.sCharName;
            playObject.m_sMapName = humData.sCurMap;
            playObject.m_nCurrX = humData.wCurX;
            playObject.m_nCurrY = humData.wCurY;
            playObject.Direction = humData.btDir;
            playObject.m_btHair = humData.btHair;
            playObject.Gender = Enum.Parse<PlayGender>(humData.btSex.ToString());
            playObject.m_btJob = (PlayJob)humData.btJob;
            playObject.m_nGold = humData.nGold;
            playObject.m_Abil.Level = humData.Abil.Level;
            playObject.m_Abil.HP = humData.Abil.HP;
            playObject.m_Abil.MP = humData.Abil.MP;
            playObject.m_Abil.MaxHP = humData.Abil.MaxHP;
            playObject.m_Abil.MaxMP = humData.Abil.MaxMP;
            playObject.m_Abil.Exp = humData.Abil.Exp;
            playObject.m_Abil.MaxExp = humData.Abil.MaxExp;
            playObject.m_Abil.Weight = humData.Abil.Weight;
            playObject.m_Abil.MaxWeight = humData.Abil.MaxWeight;
            playObject.m_Abil.WearWeight = humData.Abil.WearWeight;
            playObject.m_Abil.MaxWearWeight = humData.Abil.MaxWearWeight;
            playObject.m_Abil.HandWeight = humData.Abil.HandWeight;
            playObject.m_Abil.MaxHandWeight = humData.Abil.MaxHandWeight;
            playObject.m_wStatusTimeArr = humData.wStatusTimeArr;
            playObject.m_sHomeMap = humData.sHomeMap;
            playObject.m_nHomeX = humData.wHomeX;
            playObject.m_nHomeY = humData.wHomeY;
            playObject.m_BonusAbil = humData.BonusAbil;
            playObject.m_nBonusPoint = humData.nBonusPoint;
            playObject.m_btCreditPoint = humData.btCreditPoint;
            playObject.m_btReLevel = humData.btReLevel;
            playObject.m_sMasterName = humData.sMasterName;
            playObject.m_boMaster = humData.boMaster;
            playObject.m_sDearName = humData.sDearName;
            playObject.m_sStoragePwd = humData.sStoragePwd;
            if (playObject.m_sStoragePwd != "")
            {
                playObject.m_boPasswordLocked = true;
            }
            playObject.m_nGameGold = humData.nGameGold;
            playObject.m_nGamePoint = humData.nGamePoint;
            playObject.m_nPayMentPoint = humData.nPayMentPoint;
            playObject.m_nPkPoint = humData.nPKPoint;
            if (humData.btAllowGroup > 0)
            {
                playObject.m_boAllowGroup = true;
            }
            else
            {
                playObject.m_boAllowGroup = false;
            }
            playObject.btB2 = humData.btF9;
            playObject.m_btAttatckMode = (AttackMode)humData.btAttatckMode;
            playObject.m_nIncHealth = humData.btIncHealth;
            playObject.m_nIncSpell = humData.btIncSpell;
            playObject.m_nIncHealing = humData.btIncHealing;
            playObject.m_nFightZoneDieCount = humData.btFightZoneDieCount;
            playObject.m_sUserID = humData.sAccount;
            playObject.nC4 = humData.btEE;
            playObject.m_boLockLogon = humData.boLockLogon;
            playObject.m_wContribution = humData.wContribution;
            playObject.btC8 = humData.btEF;
            playObject.m_nHungerStatus = humData.nHungerStatus;
            playObject.m_boAllowGuildReCall = humData.boAllowGuildReCall;
            playObject.m_wGroupRcallTime = humData.wGroupRcallTime;
            playObject.m_dBodyLuck = humData.dBodyLuck;
            playObject.m_boAllowGroupReCall = humData.boAllowGroupReCall;
            playObject.m_QuestUnitOpen = humData.QuestUnitOpen;
            playObject.m_QuestUnit = humData.QuestUnit;
            playObject.m_QuestFlag = humData.QuestFlag;
            humItems = humanRcd.Data.HumItems;
            playObject.m_UseItems[Grobal2.U_DRESS] = humItems[Grobal2.U_DRESS];
            playObject.m_UseItems[Grobal2.U_WEAPON] = humItems[Grobal2.U_WEAPON];
            playObject.m_UseItems[Grobal2.U_RIGHTHAND] = humItems[Grobal2.U_RIGHTHAND];
            playObject.m_UseItems[Grobal2.U_NECKLACE] = humItems[Grobal2.U_HELMET];
            playObject.m_UseItems[Grobal2.U_HELMET] = humItems[Grobal2.U_NECKLACE];
            playObject.m_UseItems[Grobal2.U_ARMRINGL] = humItems[Grobal2.U_ARMRINGL];
            playObject.m_UseItems[Grobal2.U_ARMRINGR] = humItems[Grobal2.U_ARMRINGR];
            playObject.m_UseItems[Grobal2.U_RINGL] = humItems[Grobal2.U_RINGL];
            playObject.m_UseItems[Grobal2.U_RINGR] = humItems[Grobal2.U_RINGR];
            playObject.m_UseItems[Grobal2.U_BUJUK] = humItems[Grobal2.U_BUJUK];
            playObject.m_UseItems[Grobal2.U_BELT] = humItems[Grobal2.U_BELT];
            playObject.m_UseItems[Grobal2.U_BOOTS] = humItems[Grobal2.U_BOOTS];
            playObject.m_UseItems[Grobal2.U_CHARM] = humItems[Grobal2.U_CHARM];
            bagItems = humanRcd.Data.BagItems;
            if (bagItems != null)
            {
                for (var i = 0; i < bagItems.Length; i++)
                {
                    if (bagItems[i] == null)
                    {
                        continue;
                    }
                    if (bagItems[i].wIndex > 0)
                    {
                        userItem = bagItems[i];
                        playObject.m_ItemList.Add(userItem);
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
                    magicInfo = M2Share.UserEngine.FindMagic(humMagic[i].wMagIdx);
                    if (magicInfo != null)
                    {
                        userMagic = new TUserMagic();
                        userMagic.MagicInfo = magicInfo;
                        userMagic.wMagIdx = humMagic[i].wMagIdx;
                        userMagic.btLevel = humMagic[i].btLevel;
                        userMagic.btKey = humMagic[i].btKey;
                        userMagic.nTranPoint = humMagic[i].nTranPoint;
                        playObject.m_MagicList.Add(userMagic);
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
                    if (storageItems[i].wIndex > 0)
                    {
                        userItem = storageItems[i];
                        playObject.m_StorageItemList.Add(userItem);
                    }
                }
            }
        }

        private string GetHomeInfo(PlayJob nJob, ref short nX, ref short nY)
        {
            string result;
            int I;
            if (M2Share.StartPointList.Count > 0)
            {
                if (M2Share.StartPointList.Count > M2Share.g_Config.nStartPointSize)
                    I = M2Share.RandomNumber.Random(M2Share.g_Config.nStartPointSize);
                else
                    I = 0;
                result = M2Share.GetStartPointInfo(I, ref nX, ref nY);
            }
            else
            {
                result = M2Share.g_Config.sHomeMap;
                nX = M2Share.g_Config.nHomeX;
                nX = M2Share.g_Config.nHomeY;
            }
            return result;
        }

        private short GetRandHomeX(PlayObject playObject)
        {
            return (short)(M2Share.RandomNumber.Random(3) + (playObject.m_nHomeX - 2));
        }

        private short GetRandHomeY(PlayObject playObject)
        {
            return (short)(M2Share.RandomNumber.Random(3) + (playObject.m_nHomeY - 2));
        }

        public TMagic FindMagic(int nMagIdx)
        {
            TMagic result = null;
            TMagic magic = null;
            for (var i = 0; i < MagicList.Count; i++)
            {
                magic = MagicList[i];
                if (magic.wMagicID == nMagIdx)
                {
                    result = magic;
                    break;
                }
            }
            return result;
        }

        private void MonInitialize(TBaseObject baseObject, string sMonName)
        {
            TMonInfo monster;
            for (var i = 0; i < MonsterList.Count; i++)
            {
                monster = MonsterList[i];
                if (string.Compare(monster.sName, sMonName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    baseObject.m_btRaceServer = monster.btRace;
                    baseObject.m_btRaceImg = monster.btRaceImg;
                    baseObject.m_wAppr = monster.wAppr;
                    baseObject.m_Abil.Level = (byte)monster.wLevel;
                    baseObject.m_btLifeAttrib = monster.btLifeAttrib;
                    baseObject.m_btCoolEye = (byte)monster.wCoolEye;
                    baseObject.m_dwFightExp = monster.dwExp;
                    baseObject.m_Abil.HP = monster.wHP;
                    baseObject.m_Abil.MaxHP = monster.wHP;
                    baseObject.m_btMonsterWeapon = HUtil32.LoByte(monster.wMP);
                    baseObject.m_Abil.MP = 0;
                    baseObject.m_Abil.MaxMP = monster.wMP;
                    baseObject.m_Abil.AC = HUtil32.MakeLong(monster.wAC, monster.wAC);
                    baseObject.m_Abil.MAC = HUtil32.MakeLong(monster.wMAC, monster.wMAC);
                    baseObject.m_Abil.DC = HUtil32.MakeLong(monster.wDC, monster.wMaxDC);
                    baseObject.m_Abil.MC = HUtil32.MakeLong(monster.wMC, monster.wMC);
                    baseObject.m_Abil.SC = HUtil32.MakeLong(monster.wSC, monster.wSC);
                    baseObject.m_btSpeedPoint = (byte)monster.wSpeed;
                    baseObject.m_btHitPoint = (byte)monster.wHitPoint;
                    baseObject.m_nWalkSpeed = monster.wWalkSpeed;
                    baseObject.m_nWalkStep = monster.wWalkStep;
                    baseObject.m_dwWalkWait = monster.wWalkWait;
                    baseObject.m_nNextHitTime = monster.wAttackSpeed;
                    baseObject.m_boNastyMode = monster.boAggro;
                    baseObject.m_boNoTame = monster.boTame;
                    break;
                }
            }
        }

        public bool OpenDoor(Envirnoment envir, int nX, int nY)
        {
            var result = false;
            var door = envir.GetDoor(nX, nY);
            if (door != null && !door.Status.boOpened && !door.Status.bo01)
            {
                door.Status.boOpened = true;
                door.Status.dwOpenTick = HUtil32.GetTickCount();
                SendDoorStatus(envir, nX, nY, Grobal2.RM_DOOROPEN, 0, nX, nY, 0, "");
                result = true;
            }
            return result;
        }

        private bool CloseDoor(Envirnoment envir, TDoorInfo door)
        {
            var result = false;
            if (door != null && door.Status.boOpened)
            {
                door.Status.boOpened = false;
                SendDoorStatus(envir, door.nX, door.nY, Grobal2.RM_DOORCLOSE, 0, door.nX, door.nY, 0, "");
                result = true;
            }
            return result;
        }

        private void SendDoorStatus(Envirnoment envir, int nX, int nY, short wIdent, short wX, int nDoorX, int nDoorY,
            int nA, string sStr)
        {
            int n1C = nX - 12;
            int n24 = nX + 12;
            int n20 = nY - 12;
            int n28 = nY + 12;
            for (var n10 = n1C; n10 <= n24; n10++)
            {
                for (var n14 = n20; n14 <= n28; n14++)
                {
                    var cellsuccess = false;
                    var cellInfo = envir.GetCellInfo(n10, n14, ref cellsuccess);
                    if (cellsuccess && cellInfo.ObjList != null)
                    {
                        for (var i = 0; i < cellInfo.Count; i++)
                        {
                            var osObject = cellInfo.ObjList[i];
                            if (osObject != null && osObject.CellType == CellType.MovingObject)
                            {
                                var baseObject = M2Share.ActorMgr.Get(osObject.CellObjId);;
                                if (baseObject != null && !baseObject.m_boGhost && baseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                {
                                    baseObject.SendMsg(baseObject, wIdent, wX, nDoorX, nDoorY, nA, sStr);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ProcessMapDoor()
        {
            TDoorInfo door;
            var dorrList = M2Share.MapManager.GetDoorMapList();
            for (var i = 0; i < dorrList.Count; i++)
            {
                var envir = dorrList[i];
                for (var j = 0; j < envir.DoorList.Count; j++)
                {
                    door = envir.DoorList[j];
                    if (door.Status.boOpened)
                    {
                        if ((HUtil32.GetTickCount() - door.Status.dwOpenTick) > 5 * 1000)
                        {
                            CloseDoor(envir, door);
                        }
                    }
                }
            }
        }

        private void ProcessEvents()
        {
            int count;
            MagicEvent magicEvent;
            TBaseObject baseObject;
            for (var i = MagicEventList.Count - 1; i >= 0; i--)
            {
                magicEvent = MagicEventList[i];
                if (magicEvent != null)
                {
                    for (var j = magicEvent.BaseObjectList.Count - 1; j >= 0; j--)
                    {
                        baseObject = magicEvent.BaseObjectList[j];
                        if (baseObject.m_boDeath || baseObject.m_boGhost || !baseObject.m_boHolySeize)
                            magicEvent.BaseObjectList.RemoveAt(j);
                    }
                    if (magicEvent.BaseObjectList.Count <= 0 || (HUtil32.GetTickCount() - magicEvent.dwStartTick) > magicEvent.dwTime ||
                        (HUtil32.GetTickCount() - magicEvent.dwStartTick) > 180000)
                    {
                        count = 0;
                        while (true)
                        {
                            if (magicEvent.Events[count] != null) magicEvent.Events[count].Close();
                            count++;
                            if (count >= 8) break;
                        }
                        magicEvent = null;
                        MagicEventList.RemoveAt(i);
                    }
                }
            }
        }

        public TMagic FindMagic(string sMagicName)
        {
            TMagic result = null;
            TMagic magic = null;
            for (var i = 0; i < MagicList.Count; i++)
            {
                magic = MagicList[i];
                if (magic.sMagicName.Equals(sMagicName, StringComparison.OrdinalIgnoreCase))
                {
                    result = magic;
                    break;
                }
            }
            return result;
        }

        public int GetMapRangeMonster(Envirnoment envir, int nX, int nY, int nRange, IList<TBaseObject> list)
        {
            var result = 0;
            if (envir == null) return result;
            for (var i = 0; i < MonGenList.Count; i++)
            {
                var monGen = MonGenList[i];
                if (monGen == null) continue;
                if (monGen.Envir != null && monGen.Envir != envir) continue;
                for (var j = 0; j < monGen.CertList.Count; j++)
                {
                    var baseObject = monGen.CertList[j];
                    if (!baseObject.m_boDeath && !baseObject.m_boGhost && baseObject.m_PEnvir == envir &&
                        Math.Abs(baseObject.m_nCurrX - nX) <= nRange && Math.Abs(baseObject.m_nCurrY - nY) <= nRange)
                    {
                        if (list != null) list.Add(baseObject);
                        result++;
                    }
                }
            }
            return result;
        }

        public void AddMerchant(Merchant merchant)
        {
            M2Share.UserEngine.MerchantList.Add(merchant);
        }

        public int GetMerchantList(Envirnoment envir, int nX, int nY, int nRange, IList<TBaseObject> tmpList)
        {
            Merchant merchant;
            for (var i = 0; i < MerchantList.Count; i++)
            {
                merchant = MerchantList[i];
                if (merchant.m_PEnvir == envir && Math.Abs(merchant.m_nCurrX - nX) <= nRange &&
                    Math.Abs(merchant.m_nCurrY - nY) <= nRange) tmpList.Add(merchant);
            }
            return tmpList.Count;
        }

        public int GetNpcList(Envirnoment envir, int nX, int nY, int nRange, IList<TBaseObject> tmpList)
        {
            NormNpc npc;
            for (var i = 0; i < QuestNpcList.Count; i++)
            {
                npc = QuestNpcList[i];
                if (npc.m_PEnvir == envir && Math.Abs(npc.m_nCurrX - nX) <= nRange &&
                    Math.Abs(npc.m_nCurrY - nY) <= nRange) tmpList.Add(npc);
            }
            return tmpList.Count;
        }

        public void ReloadMerchantList()
        {
            Merchant merchant;
            for (var i = 0; i < MerchantList.Count; i++)
            {
                merchant = MerchantList[i];
                if (!merchant.m_boGhost)
                {
                    merchant.ClearScript();
                    merchant.LoadNPCScript();
                }
            }
        }

        public void ReloadNpcList()
        {
            NormNpc npc;
            for (var i = 0; i < QuestNpcList.Count; i++)
            {
                npc = QuestNpcList[i];
                npc.ClearScript();
                npc.LoadNPCScript();
            }
        }

        public int GetMapMonster(Envirnoment envir, IList<TBaseObject> list)
        {
            MonGenInfo monGen;
            TBaseObject baseObject;
            var result = 0;
            if (envir == null) return result;
            for (var i = 0; i < MonGenList.Count; i++)
            {
                monGen = MonGenList[i];
                if (monGen == null) continue;
                for (var j = 0; j < monGen.CertList.Count; j++)
                {
                    baseObject = monGen.CertList[j];
                    if (!baseObject.m_boDeath && !baseObject.m_boGhost && baseObject.m_PEnvir == envir)
                    {
                        if (list != null)
                            list.Add(baseObject);
                        result++;
                    }
                }
            }
            return result;
        }

        public void HumanExpire(string sAccount)
        {
            PlayObject playObject;
            if (!M2Share.g_Config.boKickExpireHuman) return;
            for (var i = 0; i < _PlayObjectList.Count; i++)
            {
                playObject = _PlayObjectList[i];
                if (string.Compare(playObject.m_sUserID, sAccount, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    playObject.m_boExpire = true;
                    break;
                }
            }
        }

        public int GetMapHuman(string sMapName)
        {
            PlayObject playObject;
            var result = 0;
            var envir = M2Share.MapManager.FindMap(sMapName);
            if (envir == null) return result;
            for (var i = 0; i < _PlayObjectList.Count; i++)
            {
                playObject = _PlayObjectList[i];
                if (!playObject.m_boDeath && !playObject.m_boGhost && playObject.m_PEnvir == envir) result++;
            }
            return result;
        }

        public int GetMapRageHuman(Envirnoment envir, int nRageX, int nRageY, int nRage, IList<TBaseObject> list)
        {
            var result = 0;
            PlayObject playObject;
            for (var i = 0; i < _PlayObjectList.Count; i++)
            {
                playObject = _PlayObjectList[i];
                if (!playObject.m_boDeath && !playObject.m_boGhost && playObject.m_PEnvir == envir &&
                    Math.Abs(playObject.m_nCurrX - nRageX) <= nRage && Math.Abs(playObject.m_nCurrY - nRageY) <= nRage)
                {
                    list.Add(playObject);
                    result++;
                }
            }
            return result;
        }

        public int GetStdItemIdx(string sItemName)
        {
            StdItem stdItem;
            var result = -1;
            if (string.IsNullOrEmpty(sItemName)) return result;
            for (var i = 0; i < StdItemList.Count; i++)
            {
                stdItem = StdItemList[i];
                if (stdItem.Name.Equals(sItemName, StringComparison.OrdinalIgnoreCase))
                {
                    result = i + 1;
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
            PlayObject playObject;
            for (var i = 0; i < _PlayObjectList.Count; i++)
            {
                playObject = _PlayObjectList[i];
                if (!playObject.m_boGhost)
                    playObject.SysMsg(sMsg, MsgColor.Red, msgType);
            }
        }

        public void SendBroadCastMsg(string sMsg, MsgType msgType)
        {
            PlayObject playObject;
            for (var i = 0; i < _PlayObjectList.Count; i++)
            {
                playObject = _PlayObjectList[i];
                if (!playObject.m_boGhost)
                {
                    playObject.SysMsg(sMsg, MsgColor.Red, msgType);
                }
            }
        }

        public void sub_4AE514(TGoldChangeInfo goldChangeInfo)
        {
            var goldChange = goldChangeInfo;
            HUtil32.EnterCriticalSection(_LoadPlaySection);
            _mChangeHumanDbGoldList.Add(goldChange);
        }

        public void ClearMonSayMsg()
        {
            MonGenInfo monGen;
            TBaseObject monBaseObject;
            for (var i = 0; i < MonGenList.Count; i++)
            {
                monGen = MonGenList[i];
                for (var j = 0; j < monGen.CertList.Count; j++)
                {
                    monBaseObject = monGen.CertList[j];
                    monBaseObject.m_SayMsgList = null;
                }
            }
        }

        private void PrcocessData(object state)
        {
            try
            {
                ProcessHumans();
                ProcessMonsters();
                ProcessMerchants();
                ProcessNpcs();
                if ((HUtil32.GetTickCount() - _dwProcessMissionsTime) > 1000)
                {
                    _dwProcessMissionsTime = HUtil32.GetTickCount();
                    ProcessMissions();
                    ProcessEvents();
                }
                if ((HUtil32.GetTickCount() - _dwProcessMapDoorTick) > 500)
                {
                    _dwProcessMapDoorTick = HUtil32.GetTickCount();
                    ProcessMapDoor();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetHomeInfo(ref short nX, ref short nY)
        {
            string result;
            if (M2Share.StartPointList.Count > 0)
            {
                int I;
                if (M2Share.StartPointList.Count > M2Share.g_Config.nStartPointSize)
                    I = M2Share.RandomNumber.Random(M2Share.g_Config.nStartPointSize);
                else
                    I = 0;
                result = M2Share.GetStartPointInfo(I, ref nX, ref nY);
            }
            else
            {
                result = M2Share.g_Config.sHomeMap;
                nX = M2Share.g_Config.nHomeX;
                nX = M2Share.g_Config.nHomeY;
            }
            return result;
        }

        public void StartAi()
        {
            if (_processAiThread.ThreadState != ThreadState.Running)
            {
                _processAiThread.Start();
            }
        }

        public void AddAiLogon(RoBotLogon ai)
        {
            _mUserLogonList.Add(ai);
        }

        private bool RegenAiObject(RoBotLogon ai)
        {
            var playObject = AddAiPlayObject(ai);
            if (playObject != null)
            {
                playObject.m_sHomeMap = GetHomeInfo(ref playObject.m_nHomeX, ref playObject.m_nHomeY);
                playObject.m_sUserID = "假人" + ai.sCharName;
                playObject.Start(TPathType.t_Dynamic);
                _BotPlayObjectList.Add(playObject);
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
            var envirnoment = M2Share.MapManager.FindMap(ai.sMapName);
            if (envirnoment == null)
            {
                return null;
            }
            RobotPlayObject cert = new RobotPlayObject();
            cert.m_PEnvir = envirnoment;
            cert.m_sMapName = ai.sMapName;
            cert.m_nCurrX = ai.nX;
            cert.m_nCurrY = ai.nY;
            cert.Direction = (byte)M2Share.RandomNumber.Random(8);
            cert.m_sCharName = ai.sCharName;
            cert.m_WAbil = cert.m_Abil;
            if (M2Share.RandomNumber.Random(100) < cert.m_btCoolEye)
            {
                cert.m_boCoolEye = true;
            }
            //Cert.m_sIPaddr = GetIPAddr;// Mac问题
            //Cert.m_sIPLocal = GetIPLocal(Cert.m_sIPaddr);
            cert.m_sConfigFileName = ai.sConfigFileName;
            cert.m_sHeroConfigFileName = ai.sHeroConfigFileName;
            cert.m_sFilePath = ai.sFilePath;
            cert.m_sConfigListFileName = ai.sConfigListFileName;
            cert.m_sHeroConfigListFileName = ai.sHeroConfigListFileName;
            // 英雄配置列表目录
            cert.Initialize();
            cert.RecalcLevelAbilitys();
            cert.RecalcAbilitys();
            cert.m_WAbil.HP = cert.m_WAbil.MaxHP;
            cert.m_WAbil.MP = cert.m_WAbil.MaxMP;
            if (cert.m_boAddtoMapSuccess)
            {
                p28 = null;
                if (cert.m_PEnvir.Width < 50)
                {
                    n20 = 2;
                }
                else
                {
                    n20 = 3;
                }
                if ((cert.m_PEnvir.Height < 250))
                {
                    if ((cert.m_PEnvir.Height < 30))
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
                    if (!cert.m_PEnvir.CanWalk(cert.m_nCurrX, cert.m_nCurrY, false))
                    {
                        if ((cert.m_PEnvir.Width - n24 - 1) > cert.m_nCurrX)
                        {
                            cert.m_nCurrX += (short)n20;
                        }
                        else
                        {
                            cert.m_nCurrX = (byte)((M2Share.RandomNumber.Random(cert.m_PEnvir.Width / 2)) + n24);
                            if (cert.m_PEnvir.Height - n24 - 1 > cert.m_nCurrY)
                            {
                                cert.m_nCurrY += (short)n20;
                            }
                            else
                            {
                                cert.m_nCurrY = (byte)(M2Share.RandomNumber.Random(cert.m_PEnvir.Height / 2) + n24);
                            }
                        }
                    }
                    else
                    {
                        p28 = cert.m_PEnvir.AddToMap(cert.m_nCurrX, cert.m_nCurrY, CellType.MovingObject, cert);
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
            for (var i = 0; i < _PlayObjectList.Count; i++)
            {
                playObject = _PlayObjectList[i];
                if (!playObject.m_boDeath && !playObject.m_boGhost)
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
                MagicList = new List<TMagic>();
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
            for (int i = 0; i < _PlayObjectList.Count; i++)
            {
                if (_PlayObjectList[i].m_MyGuild == guild)
                {
                    guild.GetRankName(_PlayObjectList[i], ref nRankNo);
                }
            }
        }
    }
}