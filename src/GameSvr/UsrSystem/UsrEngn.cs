using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using SystemModule;
using SystemModule.Packages;

namespace GameSvr
{
    public partial class UserEngine
    {
        private int dwProcessMapDoorTick;
        public int dwProcessMerchantTimeMax;
        public int dwProcessMerchantTimeMin;
        private int dwProcessMissionsTime;
        public int dwProcessNpcTimeMax;
        public int dwProcessNpcTimeMin;
        private int dwRegenMonstersTick;
        private int dwSendOnlineHumTime;
        private int dwShowOnlineTick;
        public IList<TAdminInfo> m_AdminList;
        private IList<TGoldChangeInfo> m_ChangeHumanDBGoldList;
        private IList<TSwitchDataInfo> m_ChangeServerList;
        private int m_dwProcessLoadPlayTick;
        private ArrayList m_ListOfGateIdx;
        private ArrayList m_ListOfSocket;
        /// <summary>
        /// 从DB读取人物数据
        /// </summary>
        private IList<TUserOpenInfo> m_LoadPlayList;
        private object m_LoadPlaySection;
        public IList<TMagicEvent> m_MagicEventList;
        public IList<TMagic> m_MagicList;
        public IList<TMerchant> m_MerchantList;
        private ArrayList m_MonFreeList;
        public IList<TMonGenInfo> m_MonGenList;
        private int m_nCurrMonGen;
        private IList<TPlayObject> m_NewHumanList;
        /// <summary>
        /// 当前怪物列表刷新位置索引
        /// </summary>
        private int m_nMonGenCertListPosition;

        private int m_nMonGenListPosition;
        private int m_nProcHumIDx;
        private IList<TPlayObject> m_PlayObjectFreeList;
        private Dictionary<string,ServerGruopInfo> m_OtherUserNameList;
        private IList<TPlayObject> m_PlayObjectList;
        private IList<TPlayObject> m_AiPlayObjectList;
        public IList<TMonInfo> MonsterList;
        private int nMerchantPosition;
        /// <summary>
        /// 怪物总数
        /// </summary>
        private int nMonsterCount;
        /// <summary>
        /// 处理怪物数，用于统计处理怪物个数
        /// </summary>
        private int nMonsterProcessCount = 0;
        /// <summary>
        /// 处理怪物总数位置，用于计算怪物总数
        /// </summary>
        private int nMonsterProcessPostion;

        private int nNpcPosition;
        /// <summary>
        /// 处理人物开始索引（每次处理人物数限制）
        /// </summary>
        private int nProcessHumanLoopTime;

        private ArrayList OldMagicList;
        public IList<TNormNpc> QuestNPCList;
        public IList<GameItem> StdItemList;
        public long m_dwAILogonTick;//处理假人间隔
        public IList<TAILogon> m_UserLogonList;//假人列表
        private readonly Thread _userEngineThread;
        private readonly Thread _processTheread;
        private readonly Thread _processAiThread;

        public UserEngine()
        {
            m_LoadPlaySection = new object();
            m_LoadPlayList = new List<TUserOpenInfo>();
            m_PlayObjectList = new List<TPlayObject>();
            m_PlayObjectFreeList = new List<TPlayObject>();
            m_ChangeHumanDBGoldList = new List<TGoldChangeInfo>();
            dwShowOnlineTick = HUtil32.GetTickCount();
            dwSendOnlineHumTime = HUtil32.GetTickCount();
            dwProcessMapDoorTick = HUtil32.GetTickCount();
            dwProcessMissionsTime = HUtil32.GetTickCount();
            dwRegenMonstersTick = HUtil32.GetTickCount();
            m_dwProcessLoadPlayTick = HUtil32.GetTickCount();
            m_nCurrMonGen = 0;
            m_nMonGenListPosition = 0;
            m_nMonGenCertListPosition = 0;
            m_nProcHumIDx = 0;
            nProcessHumanLoopTime = 0;
            nMerchantPosition = 0;
            nNpcPosition = 0;
            StdItemList = new List<GameItem>();
            MonsterList = new List<TMonInfo>();
            m_MonGenList = new List<TMonGenInfo>();
            m_MonFreeList = new ArrayList();
            m_MagicList = new List<TMagic>();
            m_AdminList = new List<TAdminInfo>();
            m_MerchantList = new List<TMerchant>();
            QuestNPCList = new List<TNormNpc>();
            m_ChangeServerList = new List<TSwitchDataInfo>();
            m_MagicEventList = new List<TMagicEvent>();
            dwProcessMerchantTimeMin = 0;
            dwProcessMerchantTimeMax = 0;
            dwProcessNpcTimeMin = 0;
            dwProcessNpcTimeMax = 0;
            m_NewHumanList = new List<TPlayObject>();
            m_ListOfGateIdx = new ArrayList();
            m_ListOfSocket = new ArrayList();
            OldMagicList = new ArrayList();
            m_OtherUserNameList = new Dictionary<string, ServerGruopInfo>();
            m_UserLogonList = new List<TAILogon>();
            _userEngineThread = new Thread(PrcocessData) { IsBackground = true };
            _processTheread = new Thread(ProcessPlayObjectData) { IsBackground = true };
            _processAiThread = new Thread(ProcessAiPlayObjectData) { IsBackground = true };
            m_AiPlayObjectList = new List<TPlayObject>();
        }

        public int MonsterCount { get { return nMonsterCount; } }
        public int OnlinePlayObject { get { return GetOnlineHumCount(); } }
        public int PlayObjectCount { get { return GetUserCount(); } }
        public int LoadPlayCount { get { return GetLoadPlayCount(); } }

        public IEnumerable<TPlayObject> PlayObjects => m_PlayObjectList;

        public void Start()
        {
            _userEngineThread.Start();
            _processTheread.Start();
            _processAiThread.Start();
        }

        public void Stop()
        {
            _userEngineThread.Interrupt();
            _processTheread.Interrupt();
            _processAiThread.Interrupt();
        }

        public void Initialize()
        {
            TMonGenInfo MonGen;
            M2Share.MainOutMessage("正在初始化NPC脚本...");
            MerchantInitialize();
            NpCinitialize();
            M2Share.MainOutMessage("初始化NPC脚本完成...");
            for (var i = 0; i < m_MonGenList.Count; i++)
            {
                MonGen = m_MonGenList[i];
                if (MonGen != null) MonGen.nRace = GetMonRace(MonGen.sMonName);
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
            TMerchant Merchant;
            for (var i = m_MerchantList.Count - 1; i >= 0; i--)
            {
                Merchant = m_MerchantList[i];
                Merchant.m_PEnvir = M2Share.g_MapManager.FindMap(Merchant.m_sMapName);
                if (Merchant.m_PEnvir != null)
                {
                    Merchant.Initialize();
                    if (Merchant.m_boAddtoMapSuccess && !Merchant.m_boIsHide)
                    {
                        M2Share.MainOutMessage("Merchant Initalize fail..." + Merchant.m_sCharName + ' ' +
                                               Merchant.m_sMapName + '(' +
                                               Merchant.m_nCurrX + ':' + Merchant.m_nCurrY + ')');
                        m_MerchantList.RemoveAt(i);
                    }
                    else
                    {
                        Merchant.LoadMerchantScript();
                        Merchant.LoadNPCData();
                    }
                }
                else
                {
                    M2Share.MainOutMessage(Merchant.m_sCharName + " - Merchant Initalize fail... (m.PEnvir=nil)");
                    m_MerchantList.RemoveAt(i);
                }
            }
        }

        private void NpCinitialize()
        {
            TNormNpc NormNpc;
            for (var i = QuestNPCList.Count - 1; i >= 0; i--)
            {
                NormNpc = QuestNPCList[i];
                NormNpc.m_PEnvir = M2Share.g_MapManager.FindMap(NormNpc.m_sMapName);
                if (NormNpc.m_PEnvir != null)
                {
                    NormNpc.Initialize();
                    if (NormNpc.m_boAddtoMapSuccess && !NormNpc.m_boIsHide)
                    {
                        M2Share.MainOutMessage(NormNpc.m_sCharName + " Npc Initalize fail... ");
                        QuestNPCList.RemoveAt(i);
                    }
                    else
                    {
                        NormNpc.LoadNPCScript();
                    }
                }
                else
                {
                    M2Share.MainOutMessage(NormNpc.m_sCharName + " Npc Initalize fail... (npc.PEnvir=nil) ");
                    QuestNPCList.RemoveAt(i);
                }
            }
        }

        private int GetLoadPlayCount()
        {
            return m_LoadPlayList.Count;
        }

        private int GetOnlineHumCount()
        {
            return m_PlayObjectList.Count + m_AiPlayObjectList.Count;
        }

        private int GetUserCount()
        {
            return m_PlayObjectList.Count + m_AiPlayObjectList.Count;
        }

        private bool ProcessHumans_IsLogined(string sChrName)
        {
            var result = false;
            if (M2Share.FrontEngine.InSaveRcdList(sChrName))
            {
                result = true;
            }
            else
            {
                for (var i = 0; i < m_PlayObjectList.Count; i++)
                {
                    if (string.Compare(m_PlayObjectList[i].m_sCharName, sChrName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        private TPlayObject ProcessHumans_MakeNewHuman(TUserOpenInfo UserOpenInfo)
        {
            TPlayObject result = null;
            TPlayObject PlayObject = null;
            TAbility Abil = null;
            TEnvirnoment Envir = null;
            int nC;
            TSwitchDataInfo SwitchDataInfo = null;
            TUserCastle Castle = null;
            const string sExceptionMsg = "[Exception] TUserEngine::MakeNewHuman";
            const string sChangeServerFail1 = "chg-server-fail-1 [{0}] -> [{1}] [{2}]";
            const string sChangeServerFail2 = "chg-server-fail-2 [{0}] -> [{1}] [{2}]";
            const string sChangeServerFail3 = "chg-server-fail-3 [{0}] -> [{1}] [{2}]";
            const string sChangeServerFail4 = "chg-server-fail-4 [{0}] -> [{1}] [{2}]";
            const string sErrorEnvirIsNil = "[Error] PlayObject.PEnvir = nil";
        ReGetMap:
            try
            {
                PlayObject = new TPlayObject();
                if (!M2Share.g_Config.boVentureServer)
                {
                    UserOpenInfo.sChrName = string.Empty;
                    UserOpenInfo.LoadUser.nSessionID = 0;
                    SwitchDataInfo = GetSwitchData(UserOpenInfo.sChrName, UserOpenInfo.LoadUser.nSessionID);
                }
                else
                {
                    SwitchDataInfo = null;
                }
                //SwitchDataInfo = null;
                if (SwitchDataInfo == null)
                {
                    GetHumData(PlayObject, ref UserOpenInfo.HumanRcd);
                    PlayObject.m_btRaceServer = Grobal2.RC_PLAYOBJECT;
                    if (string.IsNullOrEmpty(PlayObject.m_sHomeMap))
                    {
                        PlayObject.m_sHomeMap = GetHomeInfo(PlayObject.m_btJob, ref PlayObject.m_nHomeX, ref PlayObject.m_nHomeY);
                        PlayObject.m_sMapName = PlayObject.m_sHomeMap;
                        PlayObject.m_nCurrX = GetRandHomeX(PlayObject);
                        PlayObject.m_nCurrY = GetRandHomeY(PlayObject);
                        if (PlayObject.m_Abil.Level == 0)
                        {
                            Abil = PlayObject.m_Abil;
                            Abil.Level = 1;
                            Abil.AC = 0;
                            Abil.MAC = 0;
                            Abil.DC = HUtil32.MakeLong(1, 2);
                            Abil.MC = HUtil32.MakeLong(1, 2);
                            Abil.SC = HUtil32.MakeLong(1, 2);
                            Abil.MP = 15;
                            Abil.HP = 15;
                            Abil.MaxHP = 15;
                            Abil.MaxMP = 15;
                            Abil.Exp = 0;
                            Abil.MaxExp = 100;
                            Abil.Weight = 0;
                            Abil.MaxWeight = 30;
                            PlayObject.m_boNewHuman = true;
                        }
                    }
                    Envir = M2Share.g_MapManager.GetMapInfo(M2Share.nServerIndex, PlayObject.m_sMapName);
                    if (Envir != null)
                    {
                        PlayObject.m_sMapFileName = Envir.m_sMapFileName;
                        if (Envir.Flag.boFight3Zone) // 是否在行会战争地图死亡
                        {
                            if (PlayObject.m_Abil.HP <= 0 && PlayObject.m_nFightZoneDieCount < 3)
                            {
                                PlayObject.m_Abil.HP = PlayObject.m_Abil.MaxHP;
                                PlayObject.m_Abil.MP = PlayObject.m_Abil.MaxMP;
                                PlayObject.m_boDieInFight3Zone = true;
                            }
                            else
                            {
                                PlayObject.m_nFightZoneDieCount = 0;
                            }
                        }
                    }
                    PlayObject.m_MyGuild = M2Share.GuildManager.MemberOfGuild(PlayObject.m_sCharName);
                    Castle = M2Share.CastleManager.InCastleWarArea(Envir, PlayObject.m_nCurrX, PlayObject.m_nCurrY);
                    if (Envir != null && Castle != null && (Castle.m_MapPalace == Envir || Castle.m_boUnderWar))
                    {
                        Castle = M2Share.CastleManager.IsCastleMember(PlayObject);
                        if (Castle == null)
                        {
                            PlayObject.m_sMapName = PlayObject.m_sHomeMap;
                            PlayObject.m_nCurrX = (short)(PlayObject.m_nHomeX - 2 + M2Share.RandomNumber.Random(5));
                            PlayObject.m_nCurrY = (short)(PlayObject.m_nHomeY - 2 + M2Share.RandomNumber.Random(5));
                        }
                        else
                        {
                            if (Castle.m_MapPalace == Envir)
                            {
                                PlayObject.m_sMapName = Castle.GetMapName();
                                PlayObject.m_nCurrX = Castle.GetHomeX();
                                PlayObject.m_nCurrY = Castle.GetHomeY();
                            }
                        }
                    }
                    if (PlayObject.nC4 <= 1 && PlayObject.m_Abil.Level >= 1) PlayObject.nC4 = 2;
                    if (M2Share.g_MapManager.FindMap(PlayObject.m_sMapName) == null) PlayObject.m_Abil.HP = 0;
                    if (PlayObject.m_Abil.HP <= 0)
                    {
                        PlayObject.ClearStatusTime();
                        if (PlayObject.PKLevel() < 2)
                        {
                            Castle = M2Share.CastleManager.IsCastleMember(PlayObject);
                            if (Castle != null && Castle.m_boUnderWar)
                            {
                                PlayObject.m_sMapName = Castle.m_sHomeMap;
                                PlayObject.m_nCurrX = Castle.GetHomeX();
                                PlayObject.m_nCurrY = Castle.GetHomeY();
                            }
                            else
                            {
                                PlayObject.m_sMapName = PlayObject.m_sHomeMap;
                                PlayObject.m_nCurrX = (short)(PlayObject.m_nHomeX - 2 + M2Share.RandomNumber.Random(5));
                                PlayObject.m_nCurrY = (short)(PlayObject.m_nHomeY - 2 + M2Share.RandomNumber.Random(5));
                            }
                        }
                        else
                        {
                            PlayObject.m_sMapName = M2Share.g_Config.sRedDieHomeMap;// '3'
                            PlayObject.m_nCurrX = (short)(M2Share.RandomNumber.Random(13) + M2Share.g_Config.nRedDieHomeX);// 839
                            PlayObject.m_nCurrY = (short)(M2Share.RandomNumber.Random(13) + M2Share.g_Config.nRedDieHomeY);// 668
                        }
                        PlayObject.m_Abil.HP = 14;
                    }
                    PlayObject.AbilCopyToWAbil();
                    Envir = M2Share.g_MapManager.GetMapInfo(M2Share.nServerIndex, PlayObject.m_sMapName);//切换其他服务器
                    if (Envir == null)
                    {
                        PlayObject.m_nSessionID = UserOpenInfo.LoadUser.nSessionID;
                        PlayObject.m_nSocket = UserOpenInfo.LoadUser.nSocket;
                        PlayObject.m_nGateIdx = UserOpenInfo.LoadUser.nGateIdx;
                        PlayObject.m_nGSocketIdx = UserOpenInfo.LoadUser.nGSocketIdx;
                        PlayObject.m_WAbil = PlayObject.m_Abil;
                        PlayObject.m_nServerIndex = M2Share.g_MapManager.GetMapOfServerIndex(PlayObject.m_sMapName);
                        if (PlayObject.m_Abil.HP != 14)
                        {
                            M2Share.MainOutMessage(string.Format(sChangeServerFail1, new object[] { M2Share.nServerIndex, PlayObject.m_nServerIndex, PlayObject.m_sMapName }));
                        }
                        SendSwitchData(PlayObject, PlayObject.m_nServerIndex);
                        SendChangeServer(PlayObject, (byte)PlayObject.m_nServerIndex);
                        PlayObject = null;
                        return result;
                    }
                    PlayObject.m_sMapFileName = Envir.m_sMapFileName;
                    nC = 0;
                    while (true)
                    {
                        if (Envir.CanWalk(PlayObject.m_nCurrX, PlayObject.m_nCurrY, true)) break;
                        PlayObject.m_nCurrX = (short)(PlayObject.m_nCurrX - 3 + M2Share.RandomNumber.Random(6));
                        PlayObject.m_nCurrY = (short)(PlayObject.m_nCurrY - 3 + M2Share.RandomNumber.Random(6));
                        nC++;
                        if (nC >= 5) break;
                    }
                    if (!Envir.CanWalk(PlayObject.m_nCurrX, PlayObject.m_nCurrY, true))
                    {
                        M2Share.MainOutMessage(string.Format(sChangeServerFail2,
                            new object[] { M2Share.nServerIndex, PlayObject.m_nServerIndex, PlayObject.m_sMapName }));
                        PlayObject.m_sMapName = M2Share.g_Config.sHomeMap;
                        Envir = M2Share.g_MapManager.FindMap(M2Share.g_Config.sHomeMap);
                        PlayObject.m_nCurrX = M2Share.g_Config.nHomeX;
                        PlayObject.m_nCurrY = M2Share.g_Config.nHomeY;
                    }
                    PlayObject.m_PEnvir = Envir;
                    if (PlayObject.m_PEnvir == null)
                    {
                        M2Share.MainOutMessage(sErrorEnvirIsNil);
                        goto ReGetMap;
                    }
                    else
                        PlayObject.m_boReadyRun = false;
                    PlayObject.m_sMapFileName = Envir.m_sMapFileName;
                }
                else
                {
                    GetHumData(PlayObject, ref UserOpenInfo.HumanRcd);
                    PlayObject.m_sMapName = SwitchDataInfo.sMap;
                    PlayObject.m_nCurrX = SwitchDataInfo.wX;
                    PlayObject.m_nCurrY = SwitchDataInfo.wY;
                    PlayObject.m_Abil = SwitchDataInfo.Abil;
                    PlayObject.m_WAbil = SwitchDataInfo.Abil;
                    LoadSwitchData(SwitchDataInfo, ref PlayObject);
                    DelSwitchData(SwitchDataInfo);
                    Envir = M2Share.g_MapManager.GetMapInfo(M2Share.nServerIndex, PlayObject.m_sMapName);
                    if (Envir != null)
                    {
                        M2Share.MainOutMessage(string.Format(sChangeServerFail3,
                            new object[] { M2Share.nServerIndex, PlayObject.m_nServerIndex, PlayObject.m_sMapName }));
                        PlayObject.m_sMapName = M2Share.g_Config.sHomeMap;
                        Envir = M2Share.g_MapManager.FindMap(M2Share.g_Config.sHomeMap);
                        PlayObject.m_nCurrX = M2Share.g_Config.nHomeX;
                        PlayObject.m_nCurrY = M2Share.g_Config.nHomeY;
                    }
                    else
                    {
                        if (!Envir.CanWalk(PlayObject.m_nCurrX, PlayObject.m_nCurrY, true))
                        {
                            M2Share.MainOutMessage(string.Format(sChangeServerFail4,
                                new object[] { M2Share.nServerIndex, PlayObject.m_nServerIndex, PlayObject.m_sMapName }));
                            PlayObject.m_sMapName = M2Share.g_Config.sHomeMap;
                            Envir = M2Share.g_MapManager.FindMap(M2Share.g_Config.sHomeMap);
                            PlayObject.m_nCurrX = M2Share.g_Config.nHomeX;
                            PlayObject.m_nCurrY = M2Share.g_Config.nHomeY;
                        }

                        PlayObject.AbilCopyToWAbil();
                        PlayObject.m_PEnvir = Envir;
                        if (PlayObject.m_PEnvir == null)
                        {
                            M2Share.MainOutMessage(sErrorEnvirIsNil);
                            goto ReGetMap;
                        }
                        else
                        {
                            PlayObject.m_boReadyRun = false;
                            PlayObject.m_boLoginNoticeOK = true;
                            PlayObject.bo6AB = true;
                        }
                    }
                }
                PlayObject.m_sUserID = UserOpenInfo.LoadUser.sAccount;
                PlayObject.m_sIPaddr = UserOpenInfo.LoadUser.sIPaddr;
                PlayObject.m_sIPLocal = M2Share.GetIPLocal(PlayObject.m_sIPaddr);
                PlayObject.m_nSocket = UserOpenInfo.LoadUser.nSocket;
                PlayObject.m_nGSocketIdx = UserOpenInfo.LoadUser.nGSocketIdx;
                PlayObject.m_nGateIdx = UserOpenInfo.LoadUser.nGateIdx;
                PlayObject.m_nSessionID = UserOpenInfo.LoadUser.nSessionID;
                PlayObject.m_nPayMent = UserOpenInfo.LoadUser.nPayMent;
                PlayObject.m_nPayMode = UserOpenInfo.LoadUser.nPayMode;
                PlayObject.m_dwLoadTick = UserOpenInfo.LoadUser.dwNewUserTick;
                PlayObject.m_nSoftVersionDateEx = M2Share.GetExVersionNO(UserOpenInfo.LoadUser.nSoftVersionDate,
                    ref PlayObject.m_nSoftVersionDate);
                result = PlayObject;
            }
            catch (Exception ex)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(ex.StackTrace);
            }

            return result;
        }

        private void ProcessHumans()
        {
            const string sExceptionMsg1 = "[Exception] TUserEngine::ProcessHumans -> Ready, Save, Load...";
            const string sExceptionMsg2 = "[Exception] TUserEngine::ProcessHumans ClosePlayer.Delete - Free";
            const string sExceptionMsg3 = "[Exception] TUserEngine::ProcessHumans ClosePlayer.Delete";
            var dwCheckTime = HUtil32.GetTickCount();
            TPlayObject PlayObject;
            if ((HUtil32.GetTickCount() - m_dwProcessLoadPlayTick) > 200)
            {
                m_dwProcessLoadPlayTick = HUtil32.GetTickCount();
                try
                {
                    HUtil32.EnterCriticalSection(m_LoadPlaySection);
                    try
                    {
                        for (var i = 0; i < m_LoadPlayList.Count; i++)
                        {
                            TUserOpenInfo UserOpenInfo;
                            if (!M2Share.FrontEngine.IsFull() && !ProcessHumans_IsLogined(m_LoadPlayList[i].sChrName))
                            {
                                UserOpenInfo = m_LoadPlayList[i];
                                PlayObject = ProcessHumans_MakeNewHuman(UserOpenInfo);
                                if (PlayObject != null)
                                {
                                    if (PlayObject.m_boAI)
                                    {
                                        m_AiPlayObjectList.Add(PlayObject);
                                    }
                                    else
                                    {
                                        m_PlayObjectList.Add(PlayObject);
                                    }
                                    m_NewHumanList.Add(PlayObject);
                                    SendServerGroupMsg(Grobal2.ISM_USERLOGON, M2Share.nServerIndex, PlayObject.m_sCharName);
                                }
                            }
                            else
                            {
                                KickOnlineUser(m_LoadPlayList[i].sChrName);
                                UserOpenInfo = m_LoadPlayList[i];
                                m_ListOfGateIdx.Add(UserOpenInfo.LoadUser.nGateIdx);
                                m_ListOfSocket.Add(UserOpenInfo.LoadUser.nSocket);
                            }
                            m_LoadPlayList[i] = null;
                        }
                        m_LoadPlayList.Clear();
                        for (var i = 0; i < m_ChangeHumanDBGoldList.Count; i++)
                        {
                            var GoldChangeInfo = m_ChangeHumanDBGoldList[i];
                            PlayObject = GetPlayObject(GoldChangeInfo.sGameMasterName);
                            if (PlayObject != null)
                                PlayObject.GoldChange(GoldChangeInfo.sGetGoldUser, GoldChangeInfo.nGold);
                            GoldChangeInfo = null;
                        }
                        m_ChangeHumanDBGoldList.Clear();
                    }
                    finally
                    {
                        HUtil32.LeaveCriticalSection(m_LoadPlaySection);
                    }
                    for (var i = 0; i < m_NewHumanList.Count; i++)
                    {
                        PlayObject = m_NewHumanList[i];
                        M2Share.RunSocket.SetGateUserList(PlayObject.m_nGateIdx, PlayObject.m_nSocket, PlayObject);
                    }
                    m_NewHumanList.Clear();
                    for (var i = 0; i < m_ListOfGateIdx.Count; i++)
                    {
                        M2Share.RunSocket.CloseUser((int)m_ListOfGateIdx[i], (int)m_ListOfSocket[i]);
                    }
                    m_ListOfGateIdx.Clear();
                    m_ListOfSocket.Clear();
                }
                catch (Exception e)
                {
                    M2Share.ErrorMessage(sExceptionMsg1);
                    M2Share.ErrorMessage(e.Message);
                }
            }

            //人工智障开始登陆
            if (m_UserLogonList.Count > 0)
            {
                if (HUtil32.GetTickCount() - m_dwAILogonTick > 1000)
                {
                    m_dwAILogonTick = HUtil32.GetTickCount();
                    if (m_UserLogonList.Count > 0)
                    {
                        var AI = m_UserLogonList[0];
                        RegenAIObject(AI);
                        m_UserLogonList.RemoveAt(0);
                    }
                }
            }

            try
            {
                for (var i = 0; i < m_PlayObjectFreeList.Count; i++)
                {
                    PlayObject = m_PlayObjectFreeList[i];
                    if ((HUtil32.GetTickCount() - PlayObject.m_dwGhostTick) > M2Share.g_Config.dwHumanFreeDelayTime)// 5 * 60 * 1000
                    {
                        m_PlayObjectFreeList[i] = null;
                        m_PlayObjectFreeList.RemoveAt(i);
                        break;
                    }
                    if (PlayObject.m_boSwitchData && PlayObject.m_boRcdSaved)
                    {
                        if (SendSwitchData(PlayObject, PlayObject.m_nServerIndex) || PlayObject.m_nWriteChgDataErrCount > 20)
                        {
                            PlayObject.m_boSwitchData = false;
                            PlayObject.m_boSwitchDataOK = true;
                            PlayObject.m_boSwitchDataSended = true;
                            PlayObject.m_dwChgDataWritedTick = HUtil32.GetTickCount();
                        }
                        else
                        {
                            PlayObject.m_nWriteChgDataErrCount++;
                        }
                    }
                    if (PlayObject.m_boSwitchDataSended && HUtil32.GetTickCount() - PlayObject.m_dwChgDataWritedTick > 100)
                    {
                        PlayObject.m_boSwitchDataSended = false;
                        SendChangeServer(PlayObject, (byte)PlayObject.m_nServerIndex);
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage(sExceptionMsg3);
            }
            
            nProcessHumanLoopTime++;
            M2Share.g_nProcessHumanLoopTime = nProcessHumanLoopTime;
            if (m_nProcHumIDx == 0)
            {
                nProcessHumanLoopTime = 0;
                M2Share.g_nProcessHumanLoopTime = nProcessHumanLoopTime;
                var dwUsrRotTime = HUtil32.GetTickCount() - M2Share.g_dwUsrRotCountTick;
                M2Share.dwUsrRotCountMin = dwUsrRotTime;
                M2Share.g_dwUsrRotCountTick = HUtil32.GetTickCount();
                if (M2Share.dwUsrRotCountMax < dwUsrRotTime) M2Share.dwUsrRotCountMax = dwUsrRotTime;
            }
            M2Share.g_nHumCountMin = HUtil32.GetTickCount() - dwCheckTime;
            if (M2Share.g_nHumCountMax < M2Share.g_nHumCountMin) M2Share.g_nHumCountMax = M2Share.g_nHumCountMin;
        }

        private void ProcessAiPlayObjectData()
        {
            const string sExceptionMsg8 = "[Exception] TUserEngine::ProcessHumans";
            try
            {
                while (M2Share.boStartReady)
                {
                    var dwCurTick = HUtil32.GetTickCount();
                    var nIdx = m_nProcHumIDx;
                    var boCheckTimeLimit = false;
                    var dwCheckTime = HUtil32.GetTickCount();
                    while (true)
                    {
                        if (m_AiPlayObjectList.Count <= nIdx) break;
                        var PlayObject = m_AiPlayObjectList[nIdx];
                        if (dwCurTick - PlayObject.m_dwRunTick > PlayObject.m_nRunTime)
                        {
                            PlayObject.m_dwRunTick = dwCurTick;
                            if (!PlayObject.m_boGhost)
                            {
                                if (!PlayObject.m_boLoginNoticeOK)
                                {
                                    PlayObject.RunNotice();
                                }
                                else
                                {
                                    if (!PlayObject.m_boReadyRun)
                                    {
                                        PlayObject.m_boReadyRun = true;
                                        PlayObject.UserLogon();
                                    }
                                    else
                                    {
                                        if ((HUtil32.GetTickCount() - PlayObject.m_dwSearchTick) > PlayObject.m_dwSearchTime)
                                        {
                                            PlayObject.m_dwSearchTick = HUtil32.GetTickCount();
                                            PlayObject.SearchViewRange();
                                            PlayObject.GameTimeChanged();
                                        }
                                        // if (HUtil32.GetTickCount() - PlayObject.m_dwShowLineNoticeTick > M2Share.g_Config.dwShowLineNoticeTime)
                                        // {
                                        //     PlayObject.m_dwShowLineNoticeTick = HUtil32.GetTickCount();
                                        //     if (M2Share.LineNoticeList.Count > PlayObject.m_nShowLineNoticeIdx)
                                        //     {
                                        //         var lineNoticeMsg = M2Share.g_ManageNPC.GetLineVariableText(PlayObject, M2Share.LineNoticeList[PlayObject.m_nShowLineNoticeIdx]);
                                        //         switch (lineNoticeMsg[0])
                                        //         {
                                        //             case 'R':
                                        //                 PlayObject.SysMsg(lineNoticeMsg.Substring(1, lineNoticeMsg.Length - 1), TMsgColor.c_Red, TMsgType.t_Notice);
                                        //                 break;
                                        //             case 'G':
                                        //                 PlayObject.SysMsg(lineNoticeMsg.Substring(1, lineNoticeMsg.Length - 1), TMsgColor.c_Green, TMsgType.t_Notice);
                                        //                 break;
                                        //             case 'B':
                                        //                 PlayObject.SysMsg(lineNoticeMsg.Substring(1, lineNoticeMsg.Length - 1), TMsgColor.c_Blue, TMsgType.t_Notice);
                                        //                 break;
                                        //             default:
                                        //                 PlayObject.SysMsg(lineNoticeMsg, (TMsgColor)M2Share.g_Config.nLineNoticeColor, TMsgType.t_Notice);
                                        //                 break;
                                        //         }
                                        //     }
                                        //     PlayObject.m_nShowLineNoticeIdx++;
                                        //     if (M2Share.LineNoticeList.Count <= PlayObject.m_nShowLineNoticeIdx)
                                        //     {
                                        //         PlayObject.m_nShowLineNoticeIdx = 0;
                                        //     }
                                        // }
                                        PlayObject.Run();
                                        // if (!M2Share.FrontEngine.IsFull() && HUtil32.GetTickCount() - PlayObject.m_dwSaveRcdTick > M2Share.g_Config.dwSaveHumanRcdTime)
                                        // {
                                        //     PlayObject.m_dwSaveRcdTick = HUtil32.GetTickCount();
                                        //     PlayObject.DealCancelA();
                                        //     SaveHumanRcd(PlayObject);
                                        // }
                                    }
                                }
                            }
                            else
                            {
                                m_AiPlayObjectList.Remove(PlayObject);
                                PlayObject.Disappear();
                                AddToHumanFreeList(PlayObject);
                                PlayObject.DealCancelA();
                                SaveHumanRcd(PlayObject);
                                M2Share.RunSocket.CloseUser(PlayObject.m_nGateIdx, PlayObject.m_nSocket);
                                SendServerGroupMsg(Grobal2.SS_202, M2Share.nServerIndex, PlayObject.m_sCharName);
                                continue;
                            }
                        }
                        nIdx++;
                        if ((HUtil32.GetTickCount() - dwCheckTime) > M2Share.g_dwHumLimit)
                        {
                            boCheckTimeLimit = true;
                            m_nProcHumIDx = nIdx;
                            break;
                        }
                    }
                    if (!boCheckTimeLimit) m_nProcHumIDx = 0;

                    Thread.Sleep(30);
                }
            }
            catch(Exception ex)
            {
                M2Share.MainOutMessage(sExceptionMsg8);
                M2Share.MainOutMessage(ex.StackTrace);
            }
        }

        private void ProcessPlayObjectData()
        {
            const string sExceptionMsg8 = "[Exception] TUserEngine::ProcessHumans";
            try
            {
                while (M2Share.boStartReady)
                {
                    var dwCurTick = HUtil32.GetTickCount();
                    var nIdx = m_nProcHumIDx;
                    var boCheckTimeLimit = false;
                    var dwCheckTime = HUtil32.GetTickCount();
                    while (true)
                    {
                        if (m_PlayObjectList.Count <= nIdx) break;
                        var PlayObject = m_PlayObjectList[nIdx];
                        if ((dwCurTick - PlayObject.m_dwRunTick) > PlayObject.m_nRunTime)
                        {
                            PlayObject.m_dwRunTick = dwCurTick;
                            if (!PlayObject.m_boGhost)
                            {
                                if (!PlayObject.m_boLoginNoticeOK)
                                {
                                    PlayObject.RunNotice();
                                }
                                else
                                {
                                    if (!PlayObject.m_boReadyRun)
                                    {
                                        PlayObject.m_boReadyRun = true;
                                        PlayObject.UserLogon();
                                    }
                                    else
                                    {
                                        if ((HUtil32.GetTickCount() - PlayObject.m_dwSearchTick) > PlayObject.m_dwSearchTime)
                                        {
                                            PlayObject.m_dwSearchTick = HUtil32.GetTickCount();
                                            PlayObject.SearchViewRange();//搜索对像
                                            PlayObject.GameTimeChanged();//游戏时间改变
                                        }
                                        if ((HUtil32.GetTickCount() - PlayObject.m_dwShowLineNoticeTick) > M2Share.g_Config.dwShowLineNoticeTime)
                                        {
                                            PlayObject.m_dwShowLineNoticeTick = HUtil32.GetTickCount();
                                            if (M2Share.LineNoticeList.Count > PlayObject.m_nShowLineNoticeIdx)
                                            {
                                                var lineNoticeMsg = M2Share.g_ManageNPC.GetLineVariableText(PlayObject, M2Share.LineNoticeList[PlayObject.m_nShowLineNoticeIdx]);
                                                switch (lineNoticeMsg[0])
                                                {
                                                    case 'R':
                                                        PlayObject.SysMsg(lineNoticeMsg.Substring(1, lineNoticeMsg.Length - 1), TMsgColor.c_Red, TMsgType.t_Notice);
                                                        break;
                                                    case 'G':
                                                        PlayObject.SysMsg(lineNoticeMsg.Substring(1, lineNoticeMsg.Length - 1), TMsgColor.c_Green, TMsgType.t_Notice);
                                                        break;
                                                    case 'B':
                                                        PlayObject.SysMsg(lineNoticeMsg.Substring(1, lineNoticeMsg.Length - 1), TMsgColor.c_Blue, TMsgType.t_Notice);
                                                        break;
                                                    default:
                                                        PlayObject.SysMsg(lineNoticeMsg, (TMsgColor)M2Share.g_Config.nLineNoticeColor, TMsgType.t_Notice);
                                                        break;
                                                }
                                            }
                                            PlayObject.m_nShowLineNoticeIdx++;
                                            if (M2Share.LineNoticeList.Count <= PlayObject.m_nShowLineNoticeIdx)
                                            {
                                                PlayObject.m_nShowLineNoticeIdx = 0;
                                            }
                                        }
                                        PlayObject.Run();
                                        if (!M2Share.FrontEngine.IsFull() && (HUtil32.GetTickCount() - PlayObject.m_dwSaveRcdTick) > M2Share.g_Config.dwSaveHumanRcdTime)
                                        {
                                            PlayObject.m_dwSaveRcdTick = HUtil32.GetTickCount();
                                            PlayObject.DealCancelA();
                                            SaveHumanRcd(PlayObject);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                
                                Console.WriteLine("我踢人了");
                                
                                m_PlayObjectList.Remove(PlayObject);
                                PlayObject.Disappear();
                                AddToHumanFreeList(PlayObject);
                                PlayObject.DealCancelA();
                                SaveHumanRcd(PlayObject);
                                M2Share.RunSocket.CloseUser(PlayObject.m_nGateIdx, PlayObject.m_nSocket);
                                SendServerGroupMsg(Grobal2.SS_202, M2Share.nServerIndex, PlayObject.m_sCharName);
                                continue;
                            }
                        }
                        nIdx++;
                        if ((HUtil32.GetTickCount() - dwCheckTime) > M2Share.g_dwHumLimit)
                        {
                            boCheckTimeLimit = true;
                            m_nProcHumIDx = nIdx;
                            break;
                        }
                    }
                    if (!boCheckTimeLimit) m_nProcHumIDx = 0;

                    Thread.Sleep(1);
                }
            }
            catch(Exception ex)
            {
                M2Share.MainOutMessage(sExceptionMsg8);
                M2Share.MainOutMessage(ex.StackTrace);
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
                for (var i = nMerchantPosition; i < m_MerchantList.Count; i++)
                {
                    var merchantNpc = m_MerchantList[i];
                    if (!merchantNpc.m_boGhost)
                    {
                        if (dwCurrTick - merchantNpc.m_dwRunTick > merchantNpc.m_nRunTime)
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
                            m_MerchantList.RemoveAt(i);
                            break;
                        }
                    }
                    if ((HUtil32.GetTickCount() - dwRunTick) > M2Share.g_dwNpcLimit)
                    {
                        nMerchantPosition = i;
                        boProcessLimit = true;
                        break;
                    }
                }
                if (!boProcessLimit) nMerchantPosition = 0;
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
            dwProcessMerchantTimeMin = HUtil32.GetTickCount() - dwRunTick;
            if (dwProcessMerchantTimeMin > dwProcessMerchantTimeMax)
                dwProcessMerchantTimeMax = dwProcessMerchantTimeMin;
            if (dwProcessNpcTimeMin > dwProcessNpcTimeMax) dwProcessNpcTimeMax = dwProcessNpcTimeMin;
        }

        private void ProcessMissions()
        {
            
        }

        private int ProcessMonsters_GetZenTime(int dwTime)
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
            TAnimalObject Monster = null;
            try
            {
                var boProcessLimit = false;
                var dwCurrentTick = HUtil32.GetTickCount();
                TMonGenInfo MonGen = null;
                // 刷新怪物开始
                if ((HUtil32.GetTickCount() - dwRegenMonstersTick) > M2Share.g_Config.dwRegenMonstersTime)
                {
                    dwRegenMonstersTick = HUtil32.GetTickCount();
                    if (m_nCurrMonGen < m_MonGenList.Count) MonGen = m_MonGenList[m_nCurrMonGen];
                    if (m_nCurrMonGen < m_MonGenList.Count - 1)
                        m_nCurrMonGen++;
                    else
                        m_nCurrMonGen = 0;
                    if (MonGen != null && !string.IsNullOrEmpty(MonGen.sMonName) && !M2Share.g_Config.boVentureServer)
                    {
                        var nTemp = HUtil32.GetTickCount() - MonGen.dwStartTick;
                        if (MonGen.dwStartTick == 0 || nTemp > ProcessMonsters_GetZenTime(MonGen.dwZenTime))
                        {
                            var nGenCount = GetGenMonCount(MonGen); //取已刷出来的怪数量
                            var boRegened = true;
                            var nGenModCount = MonGen.nCount;
                            var map = M2Share.g_MapManager.FindMap(MonGen.sMapName);
                            if (map == null || map.Flag.boNOHUMNOMON && map.HumCount <= 0)
                                boCanCreate = false;
                            else
                                boCanCreate = true;
                            if (nGenModCount > nGenCount && boCanCreate)// 增加 控制刷怪数量比例
                            {
                                boRegened = RegenMonsters(MonGen, nGenModCount - nGenCount);
                            }
                            if (boRegened)
                            {
                                MonGen.dwStartTick = HUtil32.GetTickCount();
                            }
                        }
                    }
                }
                // 刷新怪物结束
                var dwMonProcTick = HUtil32.GetTickCount();
                nMonsterProcessCount = 0;
                var currentMongen = 0;
                var monGenTotal = m_MonGenList.Count;
                for (currentMongen = m_nMonGenListPosition; currentMongen < monGenTotal; currentMongen++)
                {
                    MonGen = m_MonGenList[currentMongen];
                    int nProcessPosition;
                    if (m_nMonGenCertListPosition < MonGen.CertCount) //TODO 耗CPU MonGen.CertList.Count, 修改为计数
                        nProcessPosition = m_nMonGenCertListPosition;
                    else
                        nProcessPosition = 0;
                    m_nMonGenCertListPosition = 0;
                    while (true)
                    {
                        if (nProcessPosition >= MonGen.CertCount) //MonGen.CertList.Count 修改为计数
                            break;
                        Monster = (TAnimalObject)MonGen.CertList[nProcessPosition];
                        if (Monster != null)
                        {
                            if (!Monster.m_boGhost)
                            {
                                if ((dwCurrentTick - Monster.m_dwRunTick) > Monster.m_nRunTime)
                                {
                                    Monster.m_dwRunTick = dwRunTick;
                                    if ((dwCurrentTick - Monster.m_dwSearchTick) > Monster.m_dwSearchTime)
                                    {
                                        Monster.m_dwSearchTick = HUtil32.GetTickCount();
                                        Monster.SearchViewRange();
                                    }
                                    if (!Monster.m_boIsVisibleActive && Monster.m_nProcessRunCount < M2Share.g_Config.nProcessMonsterInterval)
                                    {
                                        Monster.m_nProcessRunCount++;
                                    }
                                    else
                                    {
                                        Monster.m_nProcessRunCount = 0;
                                        Monster.Run();
                                    }
                                    nMonsterProcessCount++;
                                }
                                nMonsterProcessPostion++;
                            }
                            else
                            {
                                if ((HUtil32.GetTickCount() - Monster.m_dwGhostTick) > 5 * 60 * 1000)
                                {
                                    MonGen.CertList.RemoveAt(nProcessPosition);
                                    MonGen.CertCount--;
                                    Monster = null;
                                    continue;
                                }
                            }
                        }
                        nProcessPosition++;
                        if ((HUtil32.GetTickCount() - dwMonProcTick) > M2Share.g_dwMonLimit)
                        {
                            boProcessLimit = true;
                            m_nMonGenCertListPosition = nProcessPosition;
                            break;
                        }
                    }
                    if (boProcessLimit) break;
                }
                if (monGenTotal <= currentMongen)
                {
                    m_nMonGenListPosition = 0;
                    nMonsterCount = nMonsterProcessPostion;
                    nMonsterProcessPostion = 0;
                }
                if (!boProcessLimit)
                    m_nMonGenListPosition = 0;
                else
                    m_nMonGenListPosition = currentMongen;
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(e.StackTrace);
            }
        }

        /// <summary>
        /// 获取刷怪数量
        /// </summary>
        /// <param name="MonGen"></param>
        /// <returns></returns>
        private int GetGenMonCount(TMonGenInfo MonGen)
        {
            var nCount = 0;
            TBaseObject BaseObject;
            for (var i = 0; i < MonGen.CertList.Count; i++)
            {
                BaseObject = MonGen.CertList[i];
                if (!BaseObject.m_boDeath && !BaseObject.m_boGhost) nCount++;
            }
            return nCount;
        }

        private void ProcessNpcs()
        {
            TNormNpc NPC;
            var dwRunTick = HUtil32.GetTickCount();
            var boProcessLimit = false;
            try
            {
                var dwCurrTick = HUtil32.GetTickCount();
                for (var i = nNpcPosition; i < QuestNPCList.Count; i++)
                {
                    NPC = QuestNPCList[i];
                    if (!NPC.m_boGhost)
                    {
                        if ((dwCurrTick - NPC.m_dwRunTick) > NPC.m_nRunTime)
                        {
                            if ((HUtil32.GetTickCount() - NPC.m_dwSearchTick) > NPC.m_dwSearchTime)
                            {
                                NPC.m_dwSearchTick = HUtil32.GetTickCount();
                                NPC.SearchViewRange();
                            }
                            if ((dwCurrTick - NPC.m_dwRunTick) > NPC.m_nRunTime)
                            {
                                NPC.m_dwRunTick = dwCurrTick;
                                NPC.Run();
                            }
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - NPC.m_dwGhostTick) > 60 * 1000)
                        {
                            QuestNPCList.RemoveAt(i);
                            break;
                        }
                    }
                    if ((HUtil32.GetTickCount() - dwRunTick) > M2Share.g_dwNpcLimit)
                    {
                        nNpcPosition = i;
                        boProcessLimit = true;
                        break;
                    }
                }
                if (!boProcessLimit) nNpcPosition = 0;
            }
            catch
            {
                M2Share.MainOutMessage("[Exceptioin] TUserEngine.ProcessNpcs");
            }
            dwProcessNpcTimeMin = HUtil32.GetTickCount() - dwRunTick;
            if (dwProcessNpcTimeMin > dwProcessNpcTimeMax) dwProcessNpcTimeMax = dwProcessNpcTimeMin;
        }

        public TBaseObject RegenMonsterByName(string sMap, short nX, short nY, string sMonName)
        {
            TBaseObject result;
            var nRace = GetMonRace(sMonName);
            var BaseObject = AddBaseObject(sMap, nX, nY, nRace, sMonName);
            if (BaseObject != null)
            {
                var n18 = m_MonGenList.Count - 1;
                if (n18 < 0) n18 = 0;
                if (m_MonGenList.Count > n18)
                {
                    var MonGen = m_MonGenList[n18];
                    MonGen.CertList.Add(BaseObject);
                    MonGen.CertCount++;
                }
                BaseObject.m_PEnvir.AddObject(BaseObject);
                BaseObject.m_boAddToMaped = true;
            }
            result = BaseObject;
            return result;
        }

        public void Run()
        {
            const string sExceptionMsg = "[Exception] TUserEngine::Run";
            try
            {
                if ((HUtil32.GetTickCount() - dwShowOnlineTick) > M2Share.g_Config.dwConsoleShowUserCountTime)
                {
                    dwShowOnlineTick = HUtil32.GetTickCount();
                    M2Share.NoticeManager.LoadingNotice();
                    M2Share.MainOutMessage("在线数: " + PlayObjectCount);
                    M2Share.CastleManager.Save();
                }
                if ((HUtil32.GetTickCount() - dwSendOnlineHumTime) > 10000)
                {
                    dwSendOnlineHumTime = HUtil32.GetTickCount();
                    IdSrvClient.Instance.SendOnlineHumCountMsg(OnlinePlayObject);
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message);
            }
        }

        public GameItem GetStdItem(int nItemIdx)
        {
            GameItem result = null;
            nItemIdx -= 1;
            if (nItemIdx >= 0 && StdItemList.Count > nItemIdx)
            {
                result = StdItemList[nItemIdx];
                if (result.Name == "") result = null;
            }
            return result;
        }

        public GameItem GetStdItem(string sItemName)
        {
            GameItem result = null;
            GameItem StdItem = null;
            if (string.IsNullOrEmpty(sItemName)) return result;
            for (var i = 0; i < StdItemList.Count; i++)
            {
                StdItem = StdItemList[i];
                if (string.Compare(StdItem.Name, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = StdItem;
                    break;
                }
            }
            return result;
        }

        public int GetStdItemWeight(int nItemIdx)
        {
            int result;
            nItemIdx -= 1;
            if (nItemIdx >= 0 && StdItemList.Count > nItemIdx)
                result = StdItemList[nItemIdx].Weight;
            else
                result = 0;
            return result;
        }

        public string GetStdItemName(int nItemIdx)
        {
            var result = "";
            nItemIdx -= 1;
            if (nItemIdx >= 0 && StdItemList.Count > nItemIdx)
                result = StdItemList[nItemIdx].Name;
            else
                result = "";
            return result;
        }

        public bool FindOtherServerUser(string sName, ref int nServerIndex)
        {
            if (m_OtherUserNameList.TryGetValue(sName, out var groupServer))
            {
                nServerIndex = groupServer.nServerIdx;
                Console.WriteLine($"玩家在[{nServerIndex}]服务器上.");
                return true;
            }
            return false;
        }

        public void CryCry(short wIdent, TEnvirnoment pMap, int nX, int nY, int nWide, byte btFColor, byte btBColor, string sMsg)
        {
            TPlayObject PlayObject;
            for (var i = 0; i < m_PlayObjectList.Count; i++)
            {
                PlayObject = m_PlayObjectList[i];
                if (!PlayObject.m_boGhost && PlayObject.m_PEnvir == pMap && PlayObject.m_boBanShout &&
                    Math.Abs(PlayObject.m_nCurrX - nX) < nWide && Math.Abs(PlayObject.m_nCurrY - nY) < nWide)
                    PlayObject.SendMsg(null, wIdent, 0, btFColor, btBColor, 0, sMsg);
            }
        }

        /// <summary>
        /// 计算怪物掉落物品
        /// 即创建怪物对象的时候已经算好要掉落的物品和属性
        /// </summary>
        /// <returns></returns>
        private void MonGetRandomItems(TBaseObject mon)
        {
            IList<TMonItem> ItemList = null;
            var iname = string.Empty;
            for (var i = 0; i < MonsterList.Count; i++)
            {
                var Monster = MonsterList[i];
                if (string.Compare(Monster.sName, mon.m_sCharName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    ItemList = Monster.ItemList;
                    break;
                }
            }
            if (ItemList != null)
            {
                for (var i = 0; i < ItemList.Count; i++)
                {
                    var MonItem = ItemList[i];
                    if (M2Share.RandomNumber.Random(MonItem.MaxPoint) <= MonItem.SelPoint)
                    {
                        if (string.Compare(MonItem.ItemName, Grobal2.sSTRING_GOLDNAME, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            mon.m_nGold = mon.m_nGold + MonItem.Count / 2 + M2Share.RandomNumber.Random(MonItem.Count);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(iname)) iname = MonItem.ItemName;
                            TUserItem UserItem = null;
                            if (CopyToUserItemFromName(iname, ref UserItem))
                            {
                                UserItem.Dura = (ushort)HUtil32.Round(UserItem.DuraMax / 100 * (20 + M2Share.RandomNumber.Random(80)));
                                var StdItem = GetStdItem(UserItem.wIndex);
                                if (StdItem == null) continue;
                                if (M2Share.RandomNumber.Random(M2Share.g_Config.nMonRandomAddValue) == 0)
                                {
                                    StdItem.RandomUpgradeItem(UserItem);
                                }
                                if (new ArrayList(new[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem.StdMode))
                                {
                                    if (StdItem.Shape == 130 || StdItem.Shape == 131 || StdItem.Shape == 132)
                                    {
                                        StdItem.RandomUpgradeUnknownItem(UserItem);
                                    }
                                }
                                mon.m_ItemList.Add(UserItem);
                            }
                        }
                    }
                }
            }
        }

        public bool CopyToUserItemFromName(string sItemName, ref TUserItem Item)
        {
            if (string.IsNullOrEmpty(sItemName)) return false;
            for (var i = 0; i < StdItemList.Count; i++)
            {
                var StdItem = StdItemList[i];
                if (!StdItem.Name.Equals(sItemName, StringComparison.OrdinalIgnoreCase)) continue;
                if (Item == null) Item = new TUserItem();
                Item.wIndex = (ushort)(i + 1);
                Item.MakeIndex = M2Share.GetItemNumber();
                Item.Dura = StdItem.DuraMax;
                Item.DuraMax = StdItem.DuraMax;
                return true;
            }
            return false;
        }

        public void ProcessUserMessage(TPlayObject PlayObject, TDefaultMessage DefMsg, string Buff)
        {
            var sMsg = string.Empty;
            if (PlayObject.m_boOffLineFlag) return;
            if (!string.IsNullOrEmpty(Buff)) sMsg = Buff;
            switch (DefMsg.Ident)
            {
                case Grobal2.CM_SPELL:
                    if (M2Share.g_Config.boSpellSendUpdateMsg) // 使用UpdateMsg 可以防止消息队列里有多个操作
                    {
                        PlayObject.SendUpdateMsg(PlayObject, DefMsg.Ident, DefMsg.Tag, HUtil32.LoWord(DefMsg.Recog),
                            HUtil32.HiWord(DefMsg.Recog), HUtil32.MakeLong(DefMsg.Param, DefMsg.Series), "");
                    }
                    else
                    {
                        PlayObject.SendMsg(PlayObject, DefMsg.Ident, DefMsg.Tag, HUtil32.LoWord(DefMsg.Recog),
                            HUtil32.HiWord(DefMsg.Recog), HUtil32.MakeLong(DefMsg.Param, DefMsg.Series), "");
                    }
                    break;
                case Grobal2.CM_QUERYUSERNAME:
                    PlayObject.SendMsg(PlayObject, DefMsg.Ident, 0, DefMsg.Recog, DefMsg.Param, DefMsg.Tag, "");
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
                    PlayObject.SendMsg(PlayObject, DefMsg.Ident, DefMsg.Series, DefMsg.Recog, DefMsg.Param, DefMsg.Tag,
                        sMsg);
                    break;
                case Grobal2.CM_PASSWORD:
                case Grobal2.CM_CHGPASSWORD:
                case Grobal2.CM_SETPASSWORD:
                    PlayObject.SendMsg(PlayObject, DefMsg.Ident, DefMsg.Param, DefMsg.Recog, DefMsg.Series, DefMsg.Tag,
                        sMsg);
                    break;
                case Grobal2.CM_ADJUST_BONUS:
                    PlayObject.SendMsg(PlayObject, DefMsg.Ident, DefMsg.Series, DefMsg.Recog, DefMsg.Param, DefMsg.Tag,
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
                        PlayObject.SendActionMsg(PlayObject, DefMsg.Ident, DefMsg.Tag, HUtil32.LoWord(DefMsg.Recog),
                            HUtil32.HiWord(DefMsg.Recog), 0, "");
                    }
                    else
                    {
                        PlayObject.SendMsg(PlayObject, DefMsg.Ident, DefMsg.Tag, HUtil32.LoWord(DefMsg.Recog),
                            HUtil32.HiWord(DefMsg.Recog), 0, "");
                    }
                    break;
                case Grobal2.CM_SAY:
                    PlayObject.SendMsg(PlayObject, Grobal2.CM_SAY, 0, 0, 0, 0, sMsg);
                    break;
                default:
                    PlayObject.SendMsg(PlayObject, DefMsg.Ident, DefMsg.Series, DefMsg.Recog, DefMsg.Param, DefMsg.Tag,
                        sMsg);
                    break;
            }
            if (!PlayObject.m_boReadyRun) return;
            switch (DefMsg.Ident)
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
                    PlayObject.m_dwRunTick -= 100;
                    break;
            }
        }

        public void SendServerGroupMsg(int nCode, int nServerIdx, string sMsg)
        {
            if (M2Share.nServerIndex == 0)
            {
                InterServerMsg.Instance.SendServerSocket(nCode + "/" + nServerIdx + "/" + sMsg);
            }
            else
            {
                InterMsgClient.Instance.SendSocket(nCode + "/" + nServerIdx + "/" + sMsg);
            }
        }

        public void GetISMChangeServerReceive(string flName)
        {
            TPlayObject hum;
            for (var i = 0; i < m_PlayObjectFreeList.Count; i++)
            {
                hum = m_PlayObjectFreeList[i];
                if (hum.m_sSwitchDataTempFile == flName)
                {
                    hum.m_boSwitchDataOK = true;
                    break;
                }
            }
        }

        public void OtherServerUserLogon(int sNum, string uname)
        {
            var Name = string.Empty;
            var apmode = HUtil32.GetValidStr3(uname, ref Name, ":");
            m_OtherUserNameList.Remove(Name.ToLower());
            m_OtherUserNameList.Add(Name.ToLower(), new ServerGruopInfo()
            {
                nServerIdx = sNum,
                sCharName = uname
            });
        }

        public void OtherServerUserLogout(int sNum, string uname)
        {
            var Name = string.Empty;
            var apmode = HUtil32.GetValidStr3(uname, ref Name, ":");
            m_OtherUserNameList.Remove(Name.ToLower());
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
            TBaseObject Cert = null;
            int n1C;
            int n20;
            int n24;
            object p28;
            var map = M2Share.g_MapManager.FindMap(sMapName);
            if (map == null) return result;
            switch (nMonRace)
            {
                case M2Share.SUPREGUARD:
                    Cert = new TSuperGuard();
                    break;
                case M2Share.PETSUPREGUARD:
                    Cert = new TPetSuperGuard();
                    break;
                case M2Share.ARCHER_POLICE:
                    Cert = new TArcherPolice();
                    break;
                case M2Share.ANIMAL_CHICKEN:
                    Cert = new TMonster
                    {
                        m_boAnimal = true,
                        m_nMeatQuality = (ushort)(M2Share.RandomNumber.Random(3500) + 3000),
                        m_nBodyLeathery = 50
                    };
                    break;
                case M2Share.ANIMAL_DEER:
                    if (M2Share.RandomNumber.Random(30) == 0)
                        Cert = new TChickenDeer
                        {
                            m_boAnimal = true,
                            m_nMeatQuality = (ushort)(M2Share.RandomNumber.Random(20000) + 10000),
                            m_nBodyLeathery = 150
                        };
                    else
                        Cert = new TMonster
                        {
                            m_boAnimal = true,
                            m_nMeatQuality = (ushort)(M2Share.RandomNumber.Random(8000) + 8000),
                            m_nBodyLeathery = 150
                        };
                    break;
                case M2Share.ANIMAL_WOLF:
                    Cert = new TATMonster
                    {
                        m_boAnimal = true,
                        m_nMeatQuality = (ushort)(M2Share.RandomNumber.Random(8000) + 8000),
                        m_nBodyLeathery = 150
                    };
                    break;
                case M2Share.TRAINER:
                    Cert = new TTrainer();
                    break;
                case M2Share.MONSTER_OMA:
                    Cert = new TMonster();
                    break;
                case M2Share.MONSTER_OMAKNIGHT:
                    Cert = new TATMonster();
                    break;
                case M2Share.MONSTER_SPITSPIDER:
                    Cert = new TSpitSpider();
                    break;
                case 83:
                    Cert = new TSlowATMonster();
                    break;
                case 84:
                    Cert = new TScorpion();
                    break;
                case M2Share.MONSTER_STICK:
                    Cert = new TStickMonster();
                    break;
                case 86:
                    Cert = new TATMonster();
                    break;
                case M2Share.MONSTER_DUALAXE:
                    Cert = new TDualAxeMonster();
                    break;
                case 88:
                    Cert = new TATMonster();
                    break;
                case 89:
                    Cert = new TATMonster();
                    break;
                case 90:
                    Cert = new TGasAttackMonster();
                    break;
                case 91:
                    Cert = new TMagCowMonster();
                    break;
                case 92:
                    Cert = new TCowKingMonster();
                    break;
                case M2Share.MONSTER_THONEDARK:
                    Cert = new TThornDarkMonster();
                    break;
                case M2Share.MONSTER_LIGHTZOMBI:
                    Cert = new TLightingZombi();
                    break;
                case M2Share.MONSTER_DIGOUTZOMBI:
                    Cert = new TDigOutZombi();
                    if (M2Share.RandomNumber.Random(2) == 0) Cert.bo2BA = true;
                    break;
                case M2Share.MONSTER_ZILKINZOMBI:
                    Cert = new TZilKinZombi();
                    if (M2Share.RandomNumber.Random(4) == 0) Cert.bo2BA = true;
                    break;
                case 97:
                    Cert = new TCowMonster();
                    if (M2Share.RandomNumber.Random(2) == 0) Cert.bo2BA = true;
                    break;
                case M2Share.MONSTER_WHITESKELETON:
                    Cert = new TWhiteSkeleton();
                    break;
                case M2Share.MONSTER_SCULTURE:
                    Cert = new TScultureMonster
                    {
                        bo2BA = true
                    };
                    break;
                case M2Share.MONSTER_SCULTUREKING:
                    Cert = new TScultureKingMonster();
                    break;
                case M2Share.MONSTER_BEEQUEEN:
                    Cert = new TBeeQueen();
                    break;
                case 104:
                    Cert = new TArcherMonster();
                    break;
                case 105:
                    Cert = new TGasMothMonster();
                    break;
                case 106: // 楔蛾
                    Cert = new TGasDungMonster();
                    break;
                case 107:
                    Cert = new TCentipedeKingMonster();
                    break;
                case 110:
                    Cert = new TCastleDoor();
                    break;
                case 111:
                    Cert = new TWallStructure();
                    break;
                case M2Share.MONSTER_ARCHERGUARD:
                    Cert = new TArcherGuard();
                    break;
                case M2Share.MONSTER_ELFMONSTER:
                    Cert = new TElfMonster();
                    break;
                case M2Share.MONSTER_ELFWARRIOR:
                    Cert = new TElfWarriorMonster();
                    break;
                case 115:
                    Cert = new TBigHeartMonster();
                    break;
                case 116:
                    Cert = new TSpiderHouseMonster();
                    break;
                case 117:
                    Cert = new TExplosionSpider();
                    break;
                case 118:
                    Cert = new THighRiskSpider();
                    break;
                case 119:
                    Cert = new TBigPoisionSpider();
                    break;
                case 120:
                    Cert = new TSoccerBall();
                    break;
                case 130:
                    Cert = new TDoubleCriticalMonster();
                    break;
                case 131:
                    Cert = new TRonObject();
                    break;
                case 132:
                    Cert = new TSandMobObject();
                    break;
                case 133:
                    Cert = new TMagicMonObject();
                    break;
                case 134:
                    Cert = new TBoneKingMonster();
                    break;
                case 200:
                    Cert = new TElectronicScolpionMon();
                    break;
                case 201:
                    Cert = new TClone();
                    break;
                case 203:
                    Cert = new TTeleMonster();
                    break;
                case 206:
                    Cert = new TKhazard();
                    break;
                case 208:
                    Cert = new TGreenMonster();
                    break;
                case 209:
                    Cert = new TRedMonster();
                    break;
                case 210:
                    Cert = new TFrostTiger();
                    break;
                case 214:
                    Cert = new TFireMonster();
                    break;
                case 215:
                    Cert = new TFireballMonster();
                    break;
            }

            if (Cert != null)
            {
                MonInitialize(Cert, sMonName);
                Cert.m_PEnvir = map;
                Cert.m_sMapName = sMapName;
                Cert.m_nCurrX = nX;
                Cert.m_nCurrY = nY;
                Cert.m_btDirection = (byte)M2Share.RandomNumber.Random(8);
                Cert.m_sCharName = sMonName;
                Cert.m_WAbil = Cert.m_Abil;
                if (M2Share.RandomNumber.Random(100) < Cert.m_btCoolEye) Cert.m_boCoolEye = true;
                MonGetRandomItems(Cert);
                Cert.Initialize();
                if (Cert.m_boAddtoMapSuccess)
                {
                    p28 = null;
                    if (Cert.m_PEnvir.wWidth < 50)
                        n20 = 2;
                    else
                        n20 = 3;
                    if (Cert.m_PEnvir.wHeight < 250)
                    {
                        if (Cert.m_PEnvir.wHeight < 30)
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
                        if (!Cert.m_PEnvir.CanWalk(Cert.m_nCurrX, Cert.m_nCurrY, false))
                        {
                            if (Cert.m_PEnvir.wWidth - n24 - 1 > Cert.m_nCurrX)
                            {
                                Cert.m_nCurrX += (short)n20;
                            }
                            else
                            {
                                Cert.m_nCurrX = (short)(M2Share.RandomNumber.Random(Cert.m_PEnvir.wWidth / 2) + n24);
                                if (Cert.m_PEnvir.wHeight - n24 - 1 > Cert.m_nCurrY)
                                    Cert.m_nCurrY += (short)n20;
                                else
                                    Cert.m_nCurrY =
                                        (short)(M2Share.RandomNumber.Random(Cert.m_PEnvir.wHeight / 2) + n24);
                            }
                        }
                        else
                        {
                            p28 = Cert.m_PEnvir.AddToMap(Cert.m_nCurrX, Cert.m_nCurrY, Grobal2.OS_MOVINGOBJECT, Cert);
                            break;
                        }

                        n1C++;
                        if (n1C >= 31) break;
                    }

                    if (p28 == null)
                        //Cert.Free;
                        Cert = null;
                }
            }

            result = Cert;
            return result;
        }

        /// <summary>
        /// 创建怪物对象
        /// 在指定时间内创建完对象，则返加TRUE，如果超过指定时间则返回FALSE
        /// </summary>
        /// <returns></returns>
        private bool RegenMonsters(TMonGenInfo MonGen, int nCount)
        {
            TBaseObject Cert;
            const string sExceptionMsg = "[Exception] TUserEngine::RegenMonsters";
            var result = true;
            var dwStartTick = HUtil32.GetTickCount();
            try
            {
                if (MonGen.nRace > 0)
                {
                    short nX;
                    short nY;
                    if (M2Share.RandomNumber.Random(100) < MonGen.nMissionGenRate)
                    {
                        nX = (short)(MonGen.nX - MonGen.nRange + M2Share.RandomNumber.Random(MonGen.nRange * 2 + 1));
                        nY = (short)(MonGen.nY - MonGen.nRange + M2Share.RandomNumber.Random(MonGen.nRange * 2 + 1));
                        for (var i = 0; i < nCount; i++)
                        {
                            Cert = AddBaseObject(MonGen.sMapName, (short)(nX - 10 + M2Share.RandomNumber.Random(20)),
                                (short)(nY - 10 + M2Share.RandomNumber.Random(20)), MonGen.nRace, MonGen.sMonName);
                            if (Cert != null)
                            {
                                MonGen.CertCount++;
                                MonGen.CertList.Add(Cert);
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
                            nX = (short)(MonGen.nX - MonGen.nRange + M2Share.RandomNumber.Random(MonGen.nRange * 2 + 1));
                            nY = (short)(MonGen.nY - MonGen.nRange + M2Share.RandomNumber.Random(MonGen.nRange * 2 + 1));
                            Cert = AddBaseObject(MonGen.sMapName, nX, nY, MonGen.nRace, MonGen.sMonName);
                            if (Cert != null)
                            {
                                MonGen.CertCount++;
                                MonGen.CertList.Add(Cert);
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
                M2Share.ErrorMessage(sExceptionMsg);
            }
            return result;
        }

        public TPlayObject GetPlayObject(string sName)
        {
            TPlayObject result = null;
            TPlayObject PlayObject = null;
            for (var i = 0; i < m_PlayObjectList.Count; i++)
            {
                if (string.Compare(m_PlayObjectList[i].m_sCharName, sName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    PlayObject = m_PlayObjectList[i];
                    if (!PlayObject.m_boGhost)
                    {
                        if (!(PlayObject.m_boPasswordLocked && PlayObject.m_boObMode && PlayObject.m_boAdminMode))
                        {
                            result = PlayObject;
                        }
                    }
                    break;
                }
            }
            return result;
        }

        public void KickPlayObjectEx(string sName)
        {
            TPlayObject PlayObject;
            HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
            try
            {
                for (var i = 0; i < m_PlayObjectList.Count; i++)
                {
                    if (string.Compare(m_PlayObjectList[i].m_sCharName, sName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        PlayObject = m_PlayObjectList[i];
                        PlayObject.m_boEmergencyClose = true;
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessHumanCriticalSection);
            }
        }

        public TPlayObject GetPlayObjectEx(string sName)
        {
            TPlayObject result = null;
            HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
            try
            {
                for (var i = 0; i < m_PlayObjectList.Count; i++)
                {                    
                    if (string.Compare(m_PlayObjectList[i].m_sCharName, sName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = m_PlayObjectList[i];
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
            var normNpc = M2Share.ObjectSystem.Get(merchantId);
            TNormNpc npcObject = null;
            var npcType = normNpc.GetType();
            if (npcType == typeof(TMerchant))
            {
                npcObject = (TMerchant)Convert.ChangeType(normNpc, typeof(TMerchant));
            }
            if (npcType == typeof(TGuildOfficial))
            {
                npcObject = (TGuildOfficial) Convert.ChangeType(normNpc, typeof(TGuildOfficial));
            }
            if (npcType == typeof(TNormNpc))
            {
                npcObject = (TNormNpc)Convert.ChangeType(normNpc, typeof(TNormNpc));
            }
            if (npcType == typeof(TCastleOfficial))
            {
                npcObject = (TCastleOfficial)Convert.ChangeType(normNpc, typeof(TCastleOfficial));
            }
            return npcObject;
        }

        public object FindNPC(int npcId)
        {
            return M2Share.ObjectSystem.Get(npcId);;
        }

        /// <summary>
        /// 获取指定地图范围对象数
        /// </summary>
        /// <returns></returns>
        public int GetMapOfRangeHumanCount(TEnvirnoment Envir, int nX, int nY, int nRange)
        {
            var result = 0;
            TPlayObject PlayObject;
            for (var i = 0; i < m_PlayObjectList.Count; i++)
            {
                PlayObject = m_PlayObjectList[i];
                if (!PlayObject.m_boGhost && PlayObject.m_PEnvir == Envir)
                {
                    if (Math.Abs(PlayObject.m_nCurrX - nX) < nRange && Math.Abs(PlayObject.m_nCurrY - nY) < nRange)
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
            TAdminInfo AdminInfo;
            btPermission = (byte)M2Share.g_Config.nStartPermission;
            for (var i = 0; i < m_AdminList.Count; i++)
            {
                AdminInfo = m_AdminList[i];
                if (string.Compare(AdminInfo.sChrName, sUserName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    btPermission = (byte)AdminInfo.nLv;
                    sIPaddr = AdminInfo.sIPaddr;
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void AddUserOpenInfo(TUserOpenInfo UserOpenInfo)
        {
            HUtil32.EnterCriticalSection(m_LoadPlaySection);
            try
            {
                m_LoadPlayList.Add(UserOpenInfo);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_LoadPlaySection);
            }
        }

        private void KickOnlineUser(string sChrName)
        {
            TPlayObject PlayObject;
            for (var i = 0; i < m_PlayObjectList.Count; i++)
            {
                PlayObject = m_PlayObjectList[i];
                if (string.Compare(PlayObject.m_sCharName, sChrName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    PlayObject.m_boKickFlag = true;
                    break;
                }
            }
        }

        private void SendChangeServer(TPlayObject PlayObject, byte nServerIndex)
        {
            var sIPaddr = string.Empty;
            var nPort = 0;
            const string sMsg = "{0}/{1}";
            if (M2Share.GetMultiServerAddrPort(nServerIndex, ref sIPaddr, ref nPort))
            {
                PlayObject.m_boReconnection = true;
                PlayObject.SendDefMessage(Grobal2.SM_RECONNECT, 0, 0, 0, 0, string.Format(sMsg, sIPaddr, nPort));
            }
        }

        public void SaveHumanRcd(TPlayObject PlayObject)
        {
            if (PlayObject.m_boAI) //AI玩家不需要保存数据
            {
                return;
            }
            var SaveRcd = new TSaveRcd
            {
                sAccount = PlayObject.m_sUserID,
                sChrName = PlayObject.m_sCharName,
                nSessionID = PlayObject.m_nSessionID,
                PlayObject = PlayObject,
                HumanRcd = new THumDataInfo()
            };
            PlayObject.MakeSaveRcd(ref SaveRcd.HumanRcd);
            M2Share.FrontEngine.AddToSaveRcdList(SaveRcd);
        }

        private void AddToHumanFreeList(TPlayObject PlayObject)
        {
            PlayObject.m_dwGhostTick = HUtil32.GetTickCount();
            m_PlayObjectFreeList.Add(PlayObject);
        }

        private void GetHumData(TPlayObject PlayObject, ref THumDataInfo HumanRcd)
        {
            THumInfoData HumData;
            TUserItem[] HumItems;
            TUserItem[] BagItems;
            TMagicRcd[] HumMagic;
            TMagic MagicInfo;
            TUserMagic UserMagic;
            TUserItem[] StorageItems;
            TUserItem UserItem;
            HumData = HumanRcd.Data;
            PlayObject.m_sCharName = HumData.sCharName;
            PlayObject.m_sMapName = HumData.sCurMap;
            PlayObject.m_nCurrX = HumData.wCurX;
            PlayObject.m_nCurrY = HumData.wCurY;
            PlayObject.m_btDirection = HumData.btDir;
            PlayObject.m_btHair = HumData.btHair;
            PlayObject.m_btGender = HumData.btSex;
            PlayObject.m_btJob = HumData.btJob;
            PlayObject.m_nGold = HumData.nGold;
            PlayObject.m_Abil.Level = HumData.Abil.Level;
            PlayObject.m_Abil.HP = HumData.Abil.HP;
            PlayObject.m_Abil.MP = HumData.Abil.MP;
            PlayObject.m_Abil.MaxHP = HumData.Abil.MaxHP;
            PlayObject.m_Abil.MaxMP = HumData.Abil.MaxMP;
            PlayObject.m_Abil.Exp = HumData.Abil.Exp;
            PlayObject.m_Abil.MaxExp = HumData.Abil.MaxExp;
            PlayObject.m_Abil.Weight = HumData.Abil.Weight;
            PlayObject.m_Abil.MaxWeight = HumData.Abil.MaxWeight;
            PlayObject.m_Abil.WearWeight = HumData.Abil.WearWeight;
            PlayObject.m_Abil.MaxWearWeight = HumData.Abil.MaxWearWeight;
            PlayObject.m_Abil.HandWeight = HumData.Abil.HandWeight;
            PlayObject.m_Abil.MaxHandWeight = HumData.Abil.MaxHandWeight;
            PlayObject.m_wStatusTimeArr = HumData.wStatusTimeArr;
            PlayObject.m_sHomeMap = HumData.sHomeMap;
            PlayObject.m_nHomeX = HumData.wHomeX;
            PlayObject.m_nHomeY = HumData.wHomeY;
            PlayObject.m_BonusAbil = HumData.BonusAbil;
            PlayObject.m_nBonusPoint = HumData.nBonusPoint;
            PlayObject.m_btCreditPoint = HumData.btCreditPoint;
            PlayObject.m_btReLevel = HumData.btReLevel;
            PlayObject.m_sMasterName = HumData.sMasterName;
            PlayObject.m_boMaster = HumData.boMaster;
            PlayObject.m_sDearName = HumData.sDearName;
            PlayObject.m_sStoragePwd = HumData.sStoragePwd;
            if (PlayObject.m_sStoragePwd != "")
            {
                PlayObject.m_boPasswordLocked = true;
            }
            PlayObject.m_nGameGold = HumData.nGameGold;
            PlayObject.m_nGamePoint = HumData.nGamePoint;
            PlayObject.m_nPayMentPoint = HumData.nPayMentPoint;
            PlayObject.m_nPkPoint = HumData.nPKPoint;
            if (HumData.btAllowGroup > 0)
            {
                PlayObject.m_boAllowGroup = true;
            }
            else
            {
                PlayObject.m_boAllowGroup = false;
            }
            PlayObject.btB2 = HumData.btF9;
            PlayObject.m_btAttatckMode = HumData.btAttatckMode;
            PlayObject.m_nIncHealth = HumData.btIncHealth;
            PlayObject.m_nIncSpell = HumData.btIncSpell;
            PlayObject.m_nIncHealing = HumData.btIncHealing;
            PlayObject.m_nFightZoneDieCount = HumData.btFightZoneDieCount;
            PlayObject.m_sUserID = HumData.sAccount;
            PlayObject.nC4 = HumData.btEE;
            PlayObject.m_boLockLogon = HumData.boLockLogon;
            PlayObject.m_wContribution = HumData.wContribution;
            PlayObject.btC8 = HumData.btEF;
            PlayObject.m_nHungerStatus = HumData.nHungerStatus;
            PlayObject.m_boAllowGuildReCall = HumData.boAllowGuildReCall;
            PlayObject.m_wGroupRcallTime = HumData.wGroupRcallTime;
            PlayObject.m_dBodyLuck = HumData.dBodyLuck;
            PlayObject.m_boAllowGroupReCall = HumData.boAllowGroupReCall;
            PlayObject.m_QuestUnitOpen = HumData.QuestUnitOpen;
            PlayObject.m_QuestUnit = HumData.QuestUnit;
            PlayObject.m_QuestFlag = HumData.QuestFlag;
            HumItems = HumanRcd.Data.HumItems;
            PlayObject.m_UseItems[Grobal2.U_DRESS] = HumItems[Grobal2.U_DRESS];
            PlayObject.m_UseItems[Grobal2.U_WEAPON] = HumItems[Grobal2.U_WEAPON];
            PlayObject.m_UseItems[Grobal2.U_RIGHTHAND] = HumItems[Grobal2.U_RIGHTHAND];
            PlayObject.m_UseItems[Grobal2.U_NECKLACE] = HumItems[Grobal2.U_HELMET];
            PlayObject.m_UseItems[Grobal2.U_HELMET] = HumItems[Grobal2.U_NECKLACE];
            PlayObject.m_UseItems[Grobal2.U_ARMRINGL] = HumItems[Grobal2.U_ARMRINGL];
            PlayObject.m_UseItems[Grobal2.U_ARMRINGR] = HumItems[Grobal2.U_ARMRINGR];
            PlayObject.m_UseItems[Grobal2.U_RINGL] = HumItems[Grobal2.U_RINGL];
            PlayObject.m_UseItems[Grobal2.U_RINGR] = HumItems[Grobal2.U_RINGR];
            PlayObject.m_UseItems[Grobal2.U_BUJUK] = HumItems[Grobal2.U_BUJUK];
            PlayObject.m_UseItems[Grobal2.U_BELT] = HumItems[Grobal2.U_BELT];
            PlayObject.m_UseItems[Grobal2.U_BOOTS] = HumItems[Grobal2.U_BOOTS];
            PlayObject.m_UseItems[Grobal2.U_CHARM] = HumItems[Grobal2.U_CHARM];
            BagItems = HumanRcd.Data.BagItems;
            if (BagItems != null)
            {
                for (var i = BagItems.GetLowerBound(0); i <= BagItems.GetUpperBound(0); i++)
                {
                    if (BagItems[i] == null)
                    {
                        continue;
                    }
                    if (BagItems[i].wIndex > 0)
                    {
                        UserItem = BagItems[i];
                        PlayObject.m_ItemList.Add(UserItem);
                    }
                }
            }
            HumMagic = HumanRcd.Data.Magic;
            if (HumMagic != null)
            {
                for (var i = HumMagic.GetLowerBound(0); i <= HumMagic.GetUpperBound(0); i++)
                {
                    if (HumMagic[i] == null)
                    {
                        continue;
                    }
                    MagicInfo = M2Share.UserEngine.FindMagic(HumMagic[i].wMagIdx);
                    if (MagicInfo != null)
                    {
                        UserMagic = new TUserMagic();
                        UserMagic.MagicInfo = MagicInfo;
                        UserMagic.wMagIdx = HumMagic[i].wMagIdx;
                        UserMagic.btLevel = HumMagic[i].btLevel;
                        UserMagic.btKey = HumMagic[i].btKey;
                        UserMagic.nTranPoint = HumMagic[i].nTranPoint;
                        PlayObject.m_MagicList.Add(UserMagic);
                    }
                }
            }
            StorageItems = HumanRcd.Data.StorageItems;
            if (StorageItems != null)
            {
                for (var i = StorageItems.GetLowerBound(0); i <= StorageItems.GetUpperBound(0); i++)
                {
                    if (StorageItems[i] == null)
                    {
                        continue;
                    }
                    if (StorageItems[i].wIndex > 0)
                    {
                        UserItem = StorageItems[i];
                        PlayObject.m_StorageItemList.Add(UserItem);
                    }
                }
            }
        }

        private string GetHomeInfo(int nJob, ref short nX, ref short nY)
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

        private short GetRandHomeX(TPlayObject PlayObject)
        {
            return (short)(M2Share.RandomNumber.Random(3) + (PlayObject.m_nHomeX - 2));
        }

        private short GetRandHomeY(TPlayObject PlayObject)
        {
            return (short)(M2Share.RandomNumber.Random(3) + (PlayObject.m_nHomeY - 2));
        }

        public TMagic FindMagic(int nMagIdx)
        {
            TMagic result = null;
            TMagic Magic = null;
            for (var i = 0; i < m_MagicList.Count; i++)
            {
                Magic = m_MagicList[i];
                if (Magic.wMagicID == nMagIdx)
                {
                    result = Magic;
                    break;
                }
            }
            return result;
        }

        private void MonInitialize(TBaseObject BaseObject, string sMonName)
        {
            TMonInfo Monster;
            for (var i = 0; i < MonsterList.Count; i++)
            {
                Monster = MonsterList[i];
                if(string.Compare(Monster.sName,sMonName,StringComparison.OrdinalIgnoreCase)==0)
                {
                    BaseObject.m_btRaceServer = Monster.btRace;
                    BaseObject.m_btRaceImg = Monster.btRaceImg;
                    BaseObject.m_wAppr = Monster.wAppr;
                    BaseObject.m_Abil.Level = Monster.wLevel;
                    BaseObject.m_btLifeAttrib = Monster.btLifeAttrib;
                    BaseObject.m_btCoolEye = (byte)Monster.wCoolEye;
                    BaseObject.m_dwFightExp = Monster.dwExp;
                    BaseObject.m_Abil.HP = Monster.wHP;
                    BaseObject.m_Abil.MaxHP = Monster.wHP;
                    BaseObject.m_btMonsterWeapon = HUtil32.LoByte(Monster.wMP);
                    BaseObject.m_Abil.MP = 0;
                    BaseObject.m_Abil.MaxMP = Monster.wMP;
                    BaseObject.m_Abil.AC = HUtil32.MakeLong(Monster.wAC, Monster.wAC);
                    BaseObject.m_Abil.MAC = HUtil32.MakeLong(Monster.wMAC, Monster.wMAC);
                    BaseObject.m_Abil.DC = HUtil32.MakeLong(Monster.wDC, Monster.wMaxDC);
                    BaseObject.m_Abil.MC = HUtil32.MakeLong(Monster.wMC, Monster.wMC);
                    BaseObject.m_Abil.SC = HUtil32.MakeLong(Monster.wSC, Monster.wSC);
                    BaseObject.m_btSpeedPoint = (byte)Monster.wSpeed;
                    BaseObject.m_btHitPoint = (byte)Monster.wHitPoint;
                    BaseObject.m_nWalkSpeed = Monster.wWalkSpeed;
                    BaseObject.m_nWalkStep = Monster.wWalkStep;
                    BaseObject.m_dwWalkWait = Monster.wWalkWait;
                    BaseObject.m_nNextHitTime = Monster.wAttackSpeed;
                    BaseObject.m_boNastyMode = Monster.boAggro;
                    BaseObject.m_boNoTame = Monster.boTame;
                    break;
                }
            }
        }

        public bool OpenDoor(TEnvirnoment Envir, int nX, int nY)
        {
            var result = false;
            var door = Envir.GetDoor(nX, nY);
            if (door != null && !door.Status.boOpened && !door.Status.bo01)
            {
                door.Status.boOpened = true;
                door.Status.dwOpenTick = HUtil32.GetTickCount();
                SendDoorStatus(Envir, nX, nY, Grobal2.RM_DOOROPEN, 0, nX, nY, 0, "");
                result = true;
            }
            return result;
        }

        private bool CloseDoor(TEnvirnoment Envir, TDoorInfo Door)
        {
            var result = false;
            if (Door != null && Door.Status.boOpened)
            {
                Door.Status.boOpened = false;
                SendDoorStatus(Envir, Door.nX, Door.nY, Grobal2.RM_DOORCLOSE, 0, Door.nX, Door.nY, 0, "");
                result = true;
            }
            return result;
        }

        private void SendDoorStatus(TEnvirnoment Envir, int nX, int nY, short wIdent, short wX, int nDoorX, int nDoorY,
            int nA, string sStr)
        {
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            int n1C = nX - 12;
            int n24 = nX + 12;
            int n20 = nY - 12;
            int n28 = nY + 12;
            for (var n10 = n1C; n10 <= n24; n10++)
            {
                for (var n14 = n20; n14 <= n28; n14++)
                {
                    if (Envir.GetMapCellInfo(n10, n14, ref MapCellInfo) && MapCellInfo.ObjList != null)
                    {
                        for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
                        {
                            OSObject = MapCellInfo.ObjList[i];
                            if (OSObject != null && OSObject.btType == Grobal2.OS_MOVINGOBJECT)
                            {
                                BaseObject = (TBaseObject) OSObject.CellObj;
                                if (BaseObject != null && !BaseObject.m_boGhost && BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                {
                                    BaseObject.SendMsg(BaseObject, wIdent, wX, nDoorX, nDoorY, nA, sStr);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ProcessMapDoor()
        {
            TEnvirnoment Envir;
            TDoorInfo Door;
            var dorrList = M2Share.g_MapManager.GetDoorMapList();
            for (var i = 0; i < dorrList.Count; i++)
            {
                Envir = dorrList[i];
                for (var j = 0; j < Envir.m_DoorList.Count; j++)
                {
                    Door = Envir.m_DoorList[j];
                    if (Door.Status.boOpened)
                    {
                        if ((HUtil32.GetTickCount() - Door.Status.dwOpenTick) > 5 * 1000)
                        {
                            CloseDoor(Envir, Door);
                        }
                    }
                }
            }
        }

        private void ProcessEvents()
        {
            int count;
            TMagicEvent MagicEvent;
            TBaseObject BaseObject;
            for (var i = m_MagicEventList.Count - 1; i >= 0; i--)
            {
                MagicEvent = m_MagicEventList[i];
                if (MagicEvent != null)
                {
                    for (var j = MagicEvent.BaseObjectList.Count - 1; j >= 0; j--)
                    {
                        BaseObject = MagicEvent.BaseObjectList[j];
                        if (BaseObject.m_boDeath || BaseObject.m_boGhost || !BaseObject.m_boHolySeize)
                            MagicEvent.BaseObjectList.RemoveAt(j);
                    }
                    if (MagicEvent.BaseObjectList.Count <= 0 || (HUtil32.GetTickCount() - MagicEvent.dwStartTick) > MagicEvent.dwTime ||
                        (HUtil32.GetTickCount() - MagicEvent.dwStartTick) > 180000)
                    {
                        count = 0;
                        while (true)
                        {
                            if (MagicEvent.Events[count] != null) MagicEvent.Events[count].Close();
                            count++;
                            if (count >= 8) break;
                        }
                        MagicEvent = null;
                        m_MagicEventList.RemoveAt(i);
                    }
                }
            }
        }

        private void Process4AECFC()
        {
        }

        public TMagic FindMagic(string sMagicName)
        {
            TMagic result = null;
            TMagic Magic = null;
            for (var i = 0; i < m_MagicList.Count; i++)
            {
                Magic = m_MagicList[i];
                if (Magic.sMagicName.Equals(sMagicName, StringComparison.OrdinalIgnoreCase))
                {
                    result = Magic;
                    break;
                }
            }
            return result;
        }

        public int GetMapRangeMonster(TEnvirnoment Envir, int nX, int nY, int nRange, IList<TBaseObject> List)
        {
            var result = 0;
            if (Envir == null) return result;
            for (var i = 0; i < m_MonGenList.Count; i++)
            {
                var MonGen = m_MonGenList[i];
                if (MonGen == null) continue;
                if (MonGen.Envir != null && MonGen.Envir != Envir) continue;
                for (var j = 0; j < MonGen.CertList.Count; j++)
                {
                    var BaseObject = MonGen.CertList[j];
                    if (!BaseObject.m_boDeath && !BaseObject.m_boGhost && BaseObject.m_PEnvir == Envir &&
                        Math.Abs(BaseObject.m_nCurrX - nX) <= nRange && Math.Abs(BaseObject.m_nCurrY - nY) <= nRange)
                    {
                        if (List != null) List.Add(BaseObject);
                        result++;
                    }
                }
            }
            return result;
        }

        public void AddMerchant(TMerchant Merchant)
        {
            M2Share.UserEngine.m_MerchantList.Add(Merchant);
        }

        public int GetMerchantList(TEnvirnoment Envir, int nX, int nY, int nRange, IList<TBaseObject> TmpList)
        {
            TMerchant Merchant;
            for (var i = 0; i < m_MerchantList.Count; i++)
            {
                Merchant = m_MerchantList[i];
                if (Merchant.m_PEnvir == Envir && Math.Abs(Merchant.m_nCurrX - nX) <= nRange &&
                    Math.Abs(Merchant.m_nCurrY - nY) <= nRange) TmpList.Add(Merchant);
            }
            return TmpList.Count;
        }

        public int GetNpcList(TEnvirnoment Envir, int nX, int nY, int nRange, IList<TBaseObject> TmpList)
        {
            TNormNpc Npc;
            for (var i = 0; i < QuestNPCList.Count; i++)
            {
                Npc = QuestNPCList[i];
                if (Npc.m_PEnvir == Envir && Math.Abs(Npc.m_nCurrX - nX) <= nRange &&
                    Math.Abs(Npc.m_nCurrY - nY) <= nRange) TmpList.Add(Npc);
            }
            return TmpList.Count;
        }

        public void ReloadMerchantList()
        {
            TMerchant Merchant;
            for (var i = 0; i < m_MerchantList.Count; i++)
            {
                Merchant = m_MerchantList[i];
                if (!Merchant.m_boGhost)
                {
                    Merchant.ClearScript();
                    Merchant.LoadNPCScript();
                }
            }
        }

        public void ReloadNpcList()
        {
            TNormNpc Npc;
            for (var i = 0; i < QuestNPCList.Count; i++)
            {
                Npc = QuestNPCList[i];
                Npc.ClearScript();
                Npc.LoadNPCScript();
            }
        }

        public int GetMapMonster(TEnvirnoment Envir, IList<TBaseObject> List)
        {
            TMonGenInfo MonGen;
            TBaseObject BaseObject;
            var result = 0;
            if (Envir == null) return result;
            for (var i = 0; i < m_MonGenList.Count; i++)
            {
                MonGen = m_MonGenList[i];
                if (MonGen == null) continue;
                for (var j = 0; j < MonGen.CertList.Count; j++)
                {
                    BaseObject = MonGen.CertList[j];
                    if (!BaseObject.m_boDeath && !BaseObject.m_boGhost && BaseObject.m_PEnvir == Envir)
                    {
                        if (List != null) 
                            List.Add(BaseObject);
                        result++;
                    }
                }
            }
            return result;
        }

        public void HumanExpire(string sAccount)
        {
            TPlayObject PlayObject;
            if (!M2Share.g_Config.boKickExpireHuman) return;
            for (var i = 0; i < m_PlayObjectList.Count; i++)
            {
                PlayObject = m_PlayObjectList[i];
                if (string.Compare(PlayObject.m_sUserID, sAccount, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    PlayObject.m_boExpire = true;
                    break;
                }
            }
        }

        public int GetMapHuman(string sMapName)
        {
            TPlayObject PlayObject;
            var result = 0;
            var Envir = M2Share.g_MapManager.FindMap(sMapName);
            if (Envir == null) return result;
            for (var i = 0; i < m_PlayObjectList.Count; i++)
            {
                PlayObject = m_PlayObjectList[i];
                if (!PlayObject.m_boDeath && !PlayObject.m_boGhost && PlayObject.m_PEnvir == Envir) result++;
            }
            return result;
        }

        public int GetMapRageHuman(TEnvirnoment Envir, int nRageX, int nRageY, int nRage, IList<TBaseObject> List)
        {
            var result = 0;
            TPlayObject PlayObject;
            for (var i = 0; i < m_PlayObjectList.Count; i++)
            {
                PlayObject = m_PlayObjectList[i];
                if (!PlayObject.m_boDeath && !PlayObject.m_boGhost && PlayObject.m_PEnvir == Envir &&
                    Math.Abs(PlayObject.m_nCurrX - nRageX) <= nRage && Math.Abs(PlayObject.m_nCurrY - nRageY) <= nRage)
                {
                    List.Add(PlayObject);
                    result++;
                }
            }
            return result;
        }

        public int GetStdItemIdx(string sItemName)
        {
            GameItem StdItem;
            var result = -1;
            if (string.IsNullOrEmpty(sItemName)) return result;
            for (var i = 0; i < StdItemList.Count; i++)
            {
                StdItem = StdItemList[i];
                if (StdItem.Name.Equals(sItemName, StringComparison.OrdinalIgnoreCase))
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
       /// <param name="sMsg"></param>
       /// <param name="MsgType"></param>
        public void SendBroadCastMsgExt(string sMsg, TMsgType MsgType)
        {
            TPlayObject PlayObject;
            for (var i = 0; i < m_PlayObjectList.Count; i++)
            {
                PlayObject = m_PlayObjectList[i];
                if (!PlayObject.m_boGhost) 
                    PlayObject.SysMsg(sMsg, TMsgColor.c_Red, MsgType);
            }
        }

        public void SendBroadCastMsg(string sMsg, TMsgType MsgType)
        {
            TPlayObject PlayObject;
            for (var i = 0; i < m_PlayObjectList.Count; i++)
            {
                PlayObject = m_PlayObjectList[i];
                if (!PlayObject.m_boGhost)
                {
                    PlayObject.SysMsg(sMsg, TMsgColor.c_Red, MsgType);
                }
            }
        }

        public void sub_4AE514(TGoldChangeInfo GoldChangeInfo)
        {
            var GoldChange = GoldChangeInfo;
            HUtil32.EnterCriticalSection(m_LoadPlaySection);
            m_ChangeHumanDBGoldList.Add(GoldChange);
        }

        public void ClearMonSayMsg()
        {
            TMonGenInfo MonGen;
            TBaseObject MonBaseObject;
            for (var i = 0; i < m_MonGenList.Count; i++)
            {
                MonGen = m_MonGenList[i];
                for (var j = 0; j < MonGen.CertList.Count; j++)
                {
                    MonBaseObject = MonGen.CertList[j];
                    MonBaseObject.m_SayMsgList = null;
                }
            }
        }

        private void PrcocessData()
        {
            while (M2Share.boStartReady)
            {
                ProcessHumans();
                ProcessMonsters();
                ProcessMerchants();
                ProcessNpcs();
                if ((HUtil32.GetTickCount() - dwProcessMissionsTime) > 1000)
                {
                    dwProcessMissionsTime = HUtil32.GetTickCount();
                    ProcessMissions();
                    Process4AECFC();
                    ProcessEvents();
                }
                if ((HUtil32.GetTickCount() - dwProcessMapDoorTick) > 500)
                {
                    dwProcessMapDoorTick = HUtil32.GetTickCount();
                    ProcessMapDoor();
                }
                Thread.Sleep(20);
            }
        }

        public string GetHomeInfo(ref short nX,ref short nY)
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

        public void AddAILogon(TAILogon AI)
        {
            m_UserLogonList.Add(AI);
        }

        private bool RegenAIObject(TAILogon AI)
        {
            var PlayObject = AddAIPlayObject(AI);
            if (PlayObject != null)
            {
                PlayObject.m_sHomeMap = GetHomeInfo(ref PlayObject.m_nHomeX, ref PlayObject.m_nHomeY);
                PlayObject.m_sUserID = "假人";
                PlayObject.Start(TPathType.t_Dynamic);
                m_AiPlayObjectList.Add(PlayObject);
                return true;
            }
            return false;
        }

        private TAIPlayObject AddAIPlayObject(TAILogon AI)
        {
            int n1C;
            int n20;
            int n24;
            object p28;
            TAIPlayObject result = null;
            var Map = M2Share.g_MapManager.FindMap(AI.sMapName);
            if (Map == null)
            {
                return result;
            }
            TAIPlayObject Cert = new TAIPlayObject();
            if (Cert != null)
            {
                Cert.m_PEnvir = Map;
                Cert.m_sMapName = AI.sMapName;
                Cert.m_nCurrX = AI.nX;
                Cert.m_nCurrY = AI.nY;
                Cert.m_btDirection = (byte)(new System.Random(8)).Next();
                Cert.m_sCharName = AI.sCharName;
                Cert.m_WAbil = Cert.m_Abil;
                if ((new System.Random(100)).Next() < Cert.m_btCoolEye)
                {
                    Cert.m_boCoolEye = true;
                }
                //Cert.m_sIPaddr = GetIPAddr;// Mac问题
                //Cert.m_sIPLocal = GetIPLocal(Cert.m_sIPaddr);
                Cert.m_sConfigFileName = AI.sConfigFileName;
                Cert.m_sHeroConfigFileName = AI.sHeroConfigFileName;
                Cert.m_sFilePath = AI.sFilePath;
                Cert.m_sConfigListFileName = AI.sConfigListFileName;
                Cert.m_sHeroConfigListFileName = AI.sHeroConfigListFileName;
                // 英雄配置列表目录
                Cert.Initialize();
                Cert.RecalcLevelAbilitys();
                Cert.RecalcAbilitys();
                Cert.m_WAbil.HP = Cert.m_WAbil.MaxHP;
                Cert.m_WAbil.MP = Cert.m_WAbil.MaxMP;
                if (Cert.m_boAddtoMapSuccess)
                {
                    p28 = null;
                    if (Cert.m_PEnvir.wWidth < 50)
                    {
                        n20 = 2;
                    }
                    else
                    {
                        n20 = 3;
                    }
                    if ((Cert.m_PEnvir.wHeight < 250))
                    {
                        if ((Cert.m_PEnvir.wHeight < 30))
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
                        if (!Cert.m_PEnvir.CanWalk(Cert.m_nCurrX, Cert.m_nCurrY, false))
                        {
                            if ((Cert.m_PEnvir.wWidth - n24 - 1) > Cert.m_nCurrX)
                            {
                                Cert.m_nCurrX += (short)n20;
                            }
                            else
                            {
                                Cert.m_nCurrX = (byte)((new System.Random(Cert.m_PEnvir.wWidth / 2)).Next() + n24);
                                if (Cert.m_PEnvir.wHeight - n24 - 1 > Cert.m_nCurrY)
                                {
                                    Cert.m_nCurrY += (short)n20;
                                }
                                else
                                {
                                    Cert.m_nCurrY = (byte)((new System.Random(Cert.m_PEnvir.wHeight / 2)).Next() + n24);
                                }
                            }
                        }
                        else
                        {
                            p28 = Cert.m_PEnvir.AddToMap(Cert.m_nCurrX, Cert.m_nCurrY, Grobal2.OS_MOVINGOBJECT, Cert);
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
                        Cert = null;
                    }
                }
            }
            result = Cert;
            return result;
        }

        public void SendQuestMsg(string sQuestName)
        {
            TPlayObject PlayObject;
            for (var i = 0; i < m_PlayObjectList.Count; i++)
            {
                PlayObject = m_PlayObjectList[i];
                if (!PlayObject.m_boDeath && !PlayObject.m_boGhost)
                    M2Share.g_ManageNPC.GotoLable(PlayObject, sQuestName, false);
            }
        }

        public void ClearItemList()
        {
            StdItemList.Reverse();
            ClearMerchantData();
        }

        public void SwitchMagicList()
        {
            if (m_MagicList.Count > 0)
            {
                OldMagicList.Add(m_MagicList);
                m_MagicList = new List<TMagic>();
            }
        }

        private void ClearMerchantData()
        {
            TMerchant Merchant;
            for (var i = 0; i < m_MerchantList.Count; i++)
            {
                Merchant = m_MerchantList[i];
                Merchant.ClearData();
            }
        }

        public void GuildMemberReGetRankName(TGuild guild)
        {
            var nRankNo = 0;
            for (int i = 0; i < m_PlayObjectList.Count; i++)
            {
                if (m_PlayObjectList[i].m_MyGuild == guild)
                {
                    guild.GetRankName(m_PlayObjectList[i], ref nRankNo);
                }
            }
        }
    }
}