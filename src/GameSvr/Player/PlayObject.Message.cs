using GameSvr.Actor;
using GameSvr.GameCommand;
using GameSvr.Items;
using GameSvr.Npc;
using GameSvr.Services;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Player
{
    public partial class PlayObject
    {
        public override void Run()
        {
            int tObjCount;
            int nInteger;
            ProcessMessage ProcessMsg = null;
            const string sPayMentExpire = "您的帐户充值时间已到期!!!";
            const string sDisConnectMsg = "游戏被强行中断!!!";
            const string sExceptionMsg1 = "[Exception] TPlayObject::Run -> Operate 1";
            const string sExceptionMsg2 = "[Exception] TPlayObject::Run -> Operate 2 # {0} Ident:{1} Sender:{2} wP:{3} nP1:{4} nP2:{5} np3:{6} Msg:{7}";
            const string sExceptionMsg3 = "[Exception] TPlayObject::Run -> GetHighHuman";
            const string sExceptionMsg4 = "[Exception] TPlayObject::Run -> ClearObj";
            try
            {
                if (Dealing)
                {
                    if (GetPoseCreate() != DealCreat || DealCreat == this || DealCreat == null)
                    {
                        DealCancel();
                    }
                }
                if (HUtil32.GetTickCount() - AccountExpiredTick > QueryExpireTick)//一分钟查询一次账号游戏到期时间
                {
                    ExpireTime = ExpireTime - 60;//游戏时间减去一分钟
                    IdSrvClient.Instance.SendUserPlayTime(UserID, ExpireTime);
                    AccountExpiredTick = HUtil32.GetTickCount();
                    CheckExpiredTime();
                }
                if (AccountExpired)
                {
                    SysMsg(sPayMentExpire, MsgColor.Red, MsgType.Hint);
                    SysMsg(sDisConnectMsg, MsgColor.Red, MsgType.Hint);
                    m_boEmergencyClose = true;
                    AccountExpired = false;
                }
                if (FireHitSkill && (HUtil32.GetTickCount() - LatestFireHitTick) > 20 * 1000)
                {
                    FireHitSkill = false;
                    SysMsg(M2Share.sSpiritsGone, MsgColor.Red, MsgType.Hint);
                    SendSocket("+UFIR");
                }
                if (TwinHitSkill && (HUtil32.GetTickCount() - LatestTwinHitTick) > 60 * 1000)
                {
                    TwinHitSkill = false;
                    SendSocket("+UTWN");
                }
                if (m_boTimeRecall && HUtil32.GetTickCount() > m_dwTimeRecallTick) //执行 TimeRecall回到原地
                {
                    m_boTimeRecall = false;
                    SpaceMove(m_sMoveMap, m_nMoveX, m_nMoveY, 0);
                }
                for (var i = 0; i < 20; i++) //个人定时器
                {
                    if (AutoTimerStatus[i] > 500)
                    {
                        if ((HUtil32.GetTickCount() - AutoTimerTick[i]) > AutoTimerStatus[i])
                        {
                            if (M2Share.g_ManageNPC != null)
                            {
                                AutoTimerTick[i] = HUtil32.GetTickCount();
                                m_nScriptGotoCount = 0;
                                M2Share.g_ManageNPC.GotoLable(this, "@OnTimer" + i, false);
                            }
                        }
                    }
                }
                if (m_boTimeGoto && (HUtil32.GetTickCount() > m_dwTimeGotoTick)) //Delaygoto延时跳转
                {
                    m_boTimeGoto = false;
                    ((Merchant)m_TimeGotoNPC)?.GotoLable(this, m_sTimeGotoLable, false);
                }
                // 增加挂机
                if (OffLineFlag && HUtil32.GetTickCount() > KickOffLineTick)
                {
                    OffLineFlag = false;
                    m_boSoftClose = true;
                }
                if (m_boDelayCall && (HUtil32.GetTickCount() - m_dwDelayCallTick) > m_nDelayCall)
                {
                    m_boDelayCall = false;
                    NormNpc normNpc = (Merchant)M2Share.WorldEngine.FindMerchant(m_DelayCallNPC);
                    if (normNpc == null)
                    {
                        normNpc = (NormNpc)M2Share.WorldEngine.FindNpc(m_DelayCallNPC);
                    }
                    if (normNpc != null)
                    {
                        normNpc.GotoLable(this, m_sDelayCallLabel, false);
                    }
                }
                if ((HUtil32.GetTickCount() - m_dwCheckDupObjTick) > 3000)
                {
                    m_dwCheckDupObjTick = HUtil32.GetTickCount();
                    GetStartPoint();
                    tObjCount = Envir.GetXyObjCount(CurrX, CurrY);
                    if (tObjCount >= 2)
                    {
                        if (!BoDuplication)
                        {
                            BoDuplication = true;
                            DupStartTick = HUtil32.GetTickCount();
                        }
                    }
                    else
                    {
                        BoDuplication = false;
                    }
                    if ((tObjCount >= 3 && ((HUtil32.GetTickCount() - DupStartTick) > 3000) || tObjCount == 2
                        && ((HUtil32.GetTickCount() - DupStartTick) > 10000)) && ((HUtil32.GetTickCount() - DupStartTick) < 20000))
                    {
                        CharPushed(M2Share.RandomNumber.RandomByte(8), 1);
                    }
                }
                var castle = M2Share.CastleMgr.InCastleWarArea(this);
                if (castle != null && castle.UnderWar)
                {
                    ChangePkStatus(true);
                }
                if ((HUtil32.GetTickCount() - dwTick578) > 1000)
                {
                    dwTick578 = HUtil32.GetTickCount();
                    var wHour = DateTime.Now.Hour;
                    var wMin = DateTime.Now.Minute;
                    var wSec = DateTime.Now.Second;
                    var wMSec = DateTime.Now.Millisecond;
                    if (M2Share.Config.DiscountForNightTime && (wHour == M2Share.Config.HalfFeeStart || wHour == M2Share.Config.HalfFeeEnd))
                    {
                        if (wMin == 0 && wSec <= 30 && (HUtil32.GetTickCount() - m_dwLogonTick) > 60000)
                        {
                            LogonTimcCost();
                            m_dwLogonTick = HUtil32.GetTickCount();
                            m_dLogonTime = DateTime.Now;
                        }
                    }
                    if (MyGuild != null)
                    {
                        if (MyGuild.GuildWarList.Count > 0)
                        {
                            var boInSafeArea = InSafeArea();
                            if (boInSafeArea != m_boInSafeArea)
                            {
                                m_boInSafeArea = boInSafeArea;
                                RefNameColor();
                            }
                        }
                    }
                    if (castle != null && castle.UnderWar)
                    {
                        if (Envir == castle.PalaceEnvir && MyGuild != null)
                        {
                            if (!castle.IsMember(this))
                            {
                                if (castle.IsAttackGuild(MyGuild))
                                {
                                    if (castle.CanGetCastle(MyGuild))
                                    {
                                        castle.GetCastle(MyGuild);
                                        M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_211, M2Share.ServerIndex, MyGuild.sGuildName);
                                        if (castle.InPalaceGuildCount() <= 1)
                                        {
                                            castle.StopWallconquestWar();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ChangePkStatus(false);
                    }
                    if (NameColorChanged)
                    {
                        NameColorChanged = false;
                        RefUserState();
                        RefShowName();
                    }
                }
                if ((HUtil32.GetTickCount() - dwTick57C) > 500)
                {
                    dwTick57C = HUtil32.GetTickCount();
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg1);
            }
            try
            {
                m_dwGetMsgTick = HUtil32.GetTickCount();
                while (((HUtil32.GetTickCount() - m_dwGetMsgTick) < M2Share.Config.HumanGetMsgTime) && GetMessage(ref ProcessMsg))
                {
                    if (!Operate(ProcessMsg))
                    {
                        break;
                    }
                }
                if (m_boEmergencyClose || m_boKickFlag || m_boSoftClose)
                {
                    if (m_boSwitchData)
                    {
                        MapName = m_sSwitchMapName;
                        CurrX = m_nSwitchMapX;
                        CurrY = m_nSwitchMapY;
                    }
                    MakeGhost();
                    if (m_boKickFlag)
                    {
                        SendDefMessage(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0, "");
                    }
                    if (!m_boReconnection && m_boSoftClose)
                    {
                        MyGuild = M2Share.GuildMgr.MemberOfGuild(ChrName);
                        if (MyGuild != null)
                        {
                            MyGuild.SendGuildMsg(ChrName + " 已经退出游戏.");
                            M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_208, M2Share.ServerIndex, MyGuild.sGuildName + '/' + "" + '/' + ChrName + " has exited the game.");
                        }
                        IdSrvClient.Instance.SendHumanLogOutMsg(UserID, m_nSessionID);
                    }
                }
            }
            catch (Exception e)
            {
                if (ProcessMsg.wIdent == 0)
                {
                    MakeGhost(); //用于处理 人物异常退出，但人物还在游戏中问题
                }
                M2Share.Log.Error(Format(sExceptionMsg2, ChrName, ProcessMsg.wIdent, ProcessMsg.BaseObject, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.Msg));
                M2Share.Log.Error(e.Message);
            }
            var boTakeItem = false;
            // 检查身上的装备有没不符合
            for (var i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] != null && UseItems[i].Index > 0)
                {
                    var StdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                    if (StdItem != null)
                    {
                        if (!CheckItemsNeed(StdItem))
                        {
                            // m_ItemList.Add((UserItem));
                            var UserItem = UseItems[i];
                            if (AddItemToBag(UserItem))
                            {
                                SendAddItem(UserItem);
                                WeightChanged();
                                boTakeItem = true;
                            }
                            else
                            {
                                if (DropItemDown(UseItems[i], 1, false, null, this))
                                {
                                    boTakeItem = true;
                                }
                            }
                            if (boTakeItem)
                            {
                                SendDelItems(UseItems[i]);
                                UseItems[i].Index = 0;
                                RecalcAbilitys();
                            }
                        }
                    }
                    else
                    {
                        UseItems[i].Index = 0;
                    }
                }
            }
            tObjCount = m_nGameGold;
            if (m_boDecGameGold && (HUtil32.GetTickCount() - m_dwDecGameGoldTick) > m_dwDecGameGoldTime)
            {
                m_dwDecGameGoldTick = HUtil32.GetTickCount();
                if (m_nGameGold >= m_nDecGameGold)
                {
                    m_nGameGold -= m_nDecGameGold;
                    nInteger = m_nDecGameGold;
                }
                else
                {
                    nInteger = m_nGameGold;
                    m_nGameGold = 0;
                    m_boDecGameGold = false;
                    MoveToHome();
                }
                if (M2Share.GameLogGameGold)
                {
                    M2Share.EventSource.AddEventLog(Grobal2.LOG_GAMEGOLD, Format(CommandHelp.GameLogMsg1, MapName, CurrX, CurrY, ChrName, M2Share.Config.GameGoldName, nInteger, '-', "Auto"));
                }
            }
            if (m_boIncGameGold && (HUtil32.GetTickCount() - m_dwIncGameGoldTick) > m_dwIncGameGoldTime)
            {
                m_dwIncGameGoldTick = HUtil32.GetTickCount();
                if (m_nGameGold + m_nIncGameGold < 2000000)
                {
                    m_nGameGold += m_nIncGameGold;
                    nInteger = m_nIncGameGold;
                }
                else
                {
                    m_nGameGold = 2000000;
                    nInteger = 2000000 - m_nGameGold;
                    m_boIncGameGold = false;
                }
                if (M2Share.GameLogGameGold)
                {
                    M2Share.EventSource.AddEventLog(Grobal2.LOG_GAMEGOLD, Format(CommandHelp.GameLogMsg1, MapName, CurrX, CurrY, ChrName, M2Share.Config.GameGoldName, nInteger, '-', "Auto"));
                }
            }
            if (!m_boDecGameGold && Envir.Flag.boDECGAMEGOLD)
            {
                if ((HUtil32.GetTickCount() - m_dwDecGameGoldTick) > Envir.Flag.nDECGAMEGOLDTIME * 1000)
                {
                    m_dwDecGameGoldTick = HUtil32.GetTickCount();
                    if (m_nGameGold >= Envir.Flag.nDECGAMEGOLD)
                    {
                        m_nGameGold -= Envir.Flag.nDECGAMEGOLD;
                        nInteger = Envir.Flag.nDECGAMEGOLD;
                    }
                    else
                    {
                        nInteger = m_nGameGold;
                        m_nGameGold = 0;
                        m_boDecGameGold = false;
                        MoveToHome();
                    }
                    if (M2Share.GameLogGameGold)
                    {
                        M2Share.EventSource.AddEventLog(Grobal2.LOG_GAMEGOLD, Format(CommandHelp.GameLogMsg1, MapName, CurrX, CurrY, ChrName, M2Share.Config.GameGoldName, nInteger, '-', "Map"));
                    }
                }
            }
            if (!m_boIncGameGold && Envir.Flag.boINCGAMEGOLD)
            {
                if ((HUtil32.GetTickCount() - m_dwIncGameGoldTick) > (Envir.Flag.nINCGAMEGOLDTIME * 1000))
                {
                    m_dwIncGameGoldTick = HUtil32.GetTickCount();
                    if (m_nGameGold + Envir.Flag.nINCGAMEGOLD <= 2000000)
                    {
                        m_nGameGold += Envir.Flag.nINCGAMEGOLD;
                        nInteger = Envir.Flag.nINCGAMEGOLD;
                    }
                    else
                    {
                        nInteger = 2000000 - m_nGameGold;
                        m_nGameGold = 2000000;
                    }
                    if (M2Share.GameLogGameGold)
                    {
                        M2Share.EventSource.AddEventLog(Grobal2.LOG_GAMEGOLD, Format(CommandHelp.GameLogMsg1, MapName, CurrX, CurrY, ChrName, M2Share.Config.GameGoldName, nInteger, '+', "Map"));
                    }
                }
            }
            if (tObjCount != m_nGameGold)
            {
                SendUpdateMsg(this, Grobal2.RM_GOLDCHANGED, 0, 0, 0, 0, "");
            }
            if (Envir.Flag.boINCGAMEPOINT)
            {
                if ((HUtil32.GetTickCount() - m_dwIncGamePointTick) > (Envir.Flag.nINCGAMEPOINTTIME * 1000))
                {
                    m_dwIncGamePointTick = HUtil32.GetTickCount();
                    if (m_nGamePoint + Envir.Flag.nINCGAMEPOINT <= 2000000)
                    {
                        m_nGamePoint += Envir.Flag.nINCGAMEPOINT;
                        nInteger = Envir.Flag.nINCGAMEPOINT;
                    }
                    else
                    {
                        m_nGamePoint = 2000000;
                        nInteger = 2000000 - m_nGamePoint;
                    }
                    if (M2Share.GameLogGamePoint)
                    {
                        M2Share.EventSource.AddEventLog(Grobal2.LOG_GAMEPOINT, Format(CommandHelp.GameLogMsg1, MapName, CurrX, CurrY, ChrName, M2Share.Config.GamePointName, nInteger, '+', "Map"));
                    }
                }
            }
            if (Envir.Flag.boDECHP && (HUtil32.GetTickCount() - m_dwDecHPTick) > (Envir.Flag.nDECHPTIME * 1000))
            {
                m_dwDecHPTick = HUtil32.GetTickCount();
                if (WAbil.HP > Envir.Flag.nDECHPPOINT)
                {
                    WAbil.HP -= (ushort)Envir.Flag.nDECHPPOINT;
                }
                else
                {
                    WAbil.HP = 0;
                }
                HealthSpellChanged();
            }
            if (Envir.Flag.boINCHP && (HUtil32.GetTickCount() - m_dwIncHPTick) > (Envir.Flag.nINCHPTIME * 1000))
            {
                m_dwIncHPTick = HUtil32.GetTickCount();
                if (WAbil.HP + Envir.Flag.nDECHPPOINT < WAbil.MaxHP)
                {
                    WAbil.HP += (ushort)Envir.Flag.nDECHPPOINT;
                }
                else
                {
                    WAbil.HP = WAbil.MaxHP;
                }
                HealthSpellChanged();
            }
            // 降饥饿点
            if (M2Share.Config.HungerSystem)
            {
                if ((HUtil32.GetTickCount() - DecHungerPointTick) > 1000)
                {
                    DecHungerPointTick = HUtil32.GetTickCount();
                    if (HungerStatus > 0)
                    {
                        tObjCount = GetMyStatus();
                        HungerStatus -= 1;
                        if (tObjCount != GetMyStatus())
                        {
                            RefMyStatus();
                        }
                    }
                    else
                    {
                        if (M2Share.Config.HungerDecHP)
                        {
                            // 减少涨HP，MP
                            HealthTick -= 60;
                            SpellTick -= 10;
                            SpellTick = HUtil32._MAX(0, SpellTick);
                            PerHealth -= 1;
                            PerSpell -= 1;
                            if (WAbil.HP > WAbil.HP / 100)
                            {
                                WAbil.HP -= (ushort)HUtil32._MAX(1, WAbil.HP / 100);
                            }
                            else
                            {
                                if (WAbil.HP <= 2)
                                {
                                    WAbil.HP = 0;
                                }
                            }
                            HealthSpellChanged();
                        }
                    }
                }
            }
            if ((HUtil32.GetTickCount() - m_dwRateTick) > 1000)
            {
                m_dwRateTick = HUtil32.GetTickCount();
                if (m_dwKillMonExpRateTime > 0)
                {
                    m_dwKillMonExpRateTime -= 1;
                    if (m_dwKillMonExpRateTime == 0)
                    {
                        m_nKillMonExpRate = 100;
                        SysMsg("经验倍数恢复正常...", MsgColor.Red, MsgType.Hint);
                    }
                }
                if (m_dwPowerRateTime > 0)
                {
                    m_dwPowerRateTime -= 1;
                    if (m_dwPowerRateTime == 0)
                    {
                        m_nPowerRate = 100;
                        SysMsg("攻击力倍数恢复正常...", MsgColor.Red, MsgType.Hint);
                    }
                }
            }
            try
            {
                // 取得在线最高等级、PK、攻击力、魔法、道术 的人物
                if (M2Share.g_HighLevelHuman == this && (Death || Ghost))
                {
                    M2Share.g_HighLevelHuman = null;
                }
                if (M2Share.g_HighPKPointHuman == this && (Death || Ghost))
                {
                    M2Share.g_HighPKPointHuman = null;
                }
                if (M2Share.g_HighDCHuman == this && (Death || Ghost))
                {
                    M2Share.g_HighDCHuman = null;
                }
                if (M2Share.g_HighMCHuman == this && (Death || Ghost))
                {
                    M2Share.g_HighMCHuman = null;
                }
                if (M2Share.g_HighSCHuman == this && (Death || Ghost))
                {
                    M2Share.g_HighSCHuman = null;
                }
                if (M2Share.g_HighOnlineHuman == this && (Death || Ghost))
                {
                    M2Share.g_HighOnlineHuman = null;
                }
                if (Permission < 6)
                {
                    if (M2Share.g_HighLevelHuman == null || (M2Share.g_HighLevelHuman as PlayObject).Ghost)
                    {
                        M2Share.g_HighLevelHuman = this;
                    }
                    else
                    {
                        if (Abil.Level > (M2Share.g_HighLevelHuman as PlayObject).Abil.Level)
                        {
                            M2Share.g_HighLevelHuman = this;
                        }
                    }
                    // 最高PK
                    if (M2Share.g_HighPKPointHuman == null || (M2Share.g_HighPKPointHuman as PlayObject).Ghost)
                    {
                        if (PkPoint > 0)
                        {
                            M2Share.g_HighPKPointHuman = this;
                        }
                    }
                    else
                    {
                        if (PkPoint > (M2Share.g_HighPKPointHuman as PlayObject).PkPoint)
                        {
                            M2Share.g_HighPKPointHuman = this;
                        }
                    }
                    // 最高攻击力
                    if (M2Share.g_HighDCHuman == null || (M2Share.g_HighDCHuman as PlayObject).Ghost)
                    {
                        M2Share.g_HighDCHuman = this;
                    }
                    else
                    {
                        if (HUtil32.HiWord(WAbil.DC) > HUtil32.HiWord((M2Share.g_HighDCHuman as PlayObject).WAbil.DC))
                        {
                            M2Share.g_HighDCHuman = this;
                        }
                    }
                    // 最高魔法
                    if (M2Share.g_HighMCHuman == null || (M2Share.g_HighMCHuman as PlayObject).Ghost)
                    {
                        M2Share.g_HighMCHuman = this;
                    }
                    else
                    {
                        if (HUtil32.HiWord(WAbil.MC) > HUtil32.HiWord((M2Share.g_HighMCHuman as PlayObject).WAbil.MC))
                        {
                            M2Share.g_HighMCHuman = this;
                        }
                    }
                    // 最高道术
                    if (M2Share.g_HighSCHuman == null || (M2Share.g_HighSCHuman as PlayObject).Ghost)
                    {
                        M2Share.g_HighSCHuman = this;
                    }
                    else
                    {
                        if (HUtil32.HiWord(WAbil.SC) > HUtil32.HiWord((M2Share.g_HighSCHuman as PlayObject).WAbil.SC))
                        {
                            M2Share.g_HighSCHuman = this;
                        }
                    }
                    // 最长在线时间
                    if (M2Share.g_HighOnlineHuman == null || (M2Share.g_HighOnlineHuman as PlayObject).Ghost)
                    {
                        M2Share.g_HighOnlineHuman = this;
                    }
                    else
                    {
                        if (m_dwLogonTick < (M2Share.g_HighOnlineHuman as PlayObject).m_dwLogonTick)
                        {
                            M2Share.g_HighOnlineHuman = this;
                        }
                    }
                }
            }
            catch (Exception)
            {
                M2Share.Log.Error(sExceptionMsg3);
            }
            if (M2Share.Config.ReNewChangeColor && m_btReLevel > 0 && (HUtil32.GetTickCount() - m_dwReColorTick) > M2Share.Config.ReNewNameColorTime)
            {
                m_dwReColorTick = HUtil32.GetTickCount();
                m_btReColorIdx++;
                if (m_btReColorIdx >= M2Share.Config.ReNewNameColor.Length)
                {
                    m_btReColorIdx = 0;
                }
                NameColor = M2Share.Config.ReNewNameColor[m_btReColorIdx];
                RefNameColor();
            }
            // 检测侦听私聊对像
            if (m_GetWhisperHuman != null)
            {
                if (m_GetWhisperHuman.Death || m_GetWhisperHuman.Ghost)
                {
                    m_GetWhisperHuman = null;
                }
            }
            ProcessSpiritSuite();
            try
            {
                if ((HUtil32.GetTickCount() - m_dwClearObjTick) > 10000)
                {
                    m_dwClearObjTick = HUtil32.GetTickCount();
                    if (m_DearHuman != null && (m_DearHuman.Death || m_DearHuman.Ghost))
                    {
                        m_DearHuman = null;
                    }
                    if (m_boMaster)
                    {
                        for (var i = m_MasterList.Count - 1; i >= 0; i--)
                        {
                            var PlayObject = m_MasterList[i];
                            if (PlayObject.Death || PlayObject.Ghost)
                            {
                                m_MasterList.RemoveAt(i);
                            }
                        }
                    }
                    else
                    {
                        if (m_MasterHuman != null && (m_MasterHuman.Death || m_MasterHuman.Ghost))
                        {
                            m_MasterHuman = null;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg4);
                M2Share.Log.Error(e.Message);
            }
            if (m_nAutoGetExpPoint > 0 && (m_AutoGetExpEnvir == null || m_AutoGetExpEnvir == Envir) && (HUtil32.GetTickCount() - m_dwAutoGetExpTick) > m_nAutoGetExpTime)
            {
                m_dwAutoGetExpTick = HUtil32.GetTickCount();
                if (!m_boAutoGetExpInSafeZone || m_boAutoGetExpInSafeZone && InSafeZone())
                {
                    GetExp(m_nAutoGetExpPoint);
                }
            }
            base.Run();
        }

        protected override bool Operate(ProcessMessage ProcessMsg)
        {
            CharDesc CharDesc;
            int nObjCount;
            string sendMsg;
            var dwDelayTime = 0;
            int nMsgCount;
            var result = true;
            BaseObject BaseObject = null;
            if (ProcessMsg.BaseObject > 0)
            {
                BaseObject = M2Share.ActorMgr.Get(ProcessMsg.BaseObject);
            }
            MessageBodyWL MessageBodyWL;
            switch (ProcessMsg.wIdent)
            {
                case Grobal2.CM_QUERYUSERNAME:
                    ClientQueryUserName(ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3);
                    break;
                case Grobal2.CM_QUERYBAGITEMS: //僵尸攻击：不断刷新包裹发送大量数据，导致网络阻塞
                    if ((HUtil32.GetTickCount() - m_dwQueryBagItemsTick) > 30 * 1000)
                    {
                        m_dwQueryBagItemsTick = HUtil32.GetTickCount();
                        ClientQueryBagItems();
                    }
                    else
                    {
                        SysMsg(M2Share.g_sQUERYBAGITEMS, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case Grobal2.CM_QUERYUSERSTATE:
                    ClientQueryUserInformation(ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3);
                    break;
                case Grobal2.CM_QUERYUSERSET:
                    ClientQueryUserSet(ProcessMsg);
                    break;
                case Grobal2.CM_DROPITEM:
                    if (ClientDropItem(ProcessMsg.Msg, ProcessMsg.nParam1))
                    {
                        SendDefMessage(Grobal2.SM_DROPITEM_SUCCESS, ProcessMsg.nParam1, 0, 0, 0, ProcessMsg.Msg);
                    }
                    else
                    {
                        SendDefMessage(Grobal2.SM_DROPITEM_FAIL, ProcessMsg.nParam1, 0, 0, 0, ProcessMsg.Msg);
                    }
                    break;
                case Grobal2.CM_PICKUP:
                    if (CurrX == ProcessMsg.nParam2 && CurrY == ProcessMsg.nParam3)
                    {
                        ClientPickUpItem();
                    }
                    break;
                case Grobal2.CM_OPENDOOR:
                    ClientOpenDoor(ProcessMsg.nParam2, ProcessMsg.nParam3);
                    break;
                case Grobal2.CM_TAKEONITEM:
                    ClientTakeOnItems((byte)ProcessMsg.nParam2, ProcessMsg.nParam1, ProcessMsg.Msg);
                    break;
                case Grobal2.CM_TAKEOFFITEM:
                    ClientTakeOffItems((byte)ProcessMsg.nParam2, ProcessMsg.nParam1, ProcessMsg.Msg);
                    break;
                case Grobal2.CM_EAT:
                    ClientUseItems(ProcessMsg.nParam1, ProcessMsg.Msg);
                    break;
                case Grobal2.CM_BUTCH:
                    if (!ClientGetButchItem(ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, (byte)ProcessMsg.wParam, ref dwDelayTime))
                    {
                        if (dwDelayTime != 0)
                        {
                            nMsgCount = GetDigUpMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxDigUpMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Log.Warn(Format(CommandHelp.BunOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime < M2Share.Config.DropOverSpeed)
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(Format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                    SendSocket(M2Share.GetGoodTick);
                                }
                                else
                                {
                                    SendDelayMsg(this, ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Grobal2.CM_MAGICKEYCHANGE:
                    ClientChangeMagicKey(ProcessMsg.nParam1, (char)ProcessMsg.nParam2);
                    break;
                case Grobal2.CM_SOFTCLOSE:
                    if (!OffLineFlag)
                    {
                        m_boReconnection = true;
                        m_boSoftClose = true;
                        if (ProcessMsg.wParam == 1)
                        {
                            m_boEmergencyClose = true;
                        }
                    }
                    break;
                case Grobal2.CM_CLICKNPC:
                    ClientClickNpc(ProcessMsg.nParam1);
                    break;
                case Grobal2.CM_MERCHANTDLGSELECT:
                    ClientMerchantDlgSelect(ProcessMsg.nParam1, ProcessMsg.Msg);
                    break;
                case Grobal2.CM_MERCHANTQUERYSELLPRICE:
                    ClientMerchantQuerySellPrice(ProcessMsg.nParam1, HUtil32.MakeLong((short)ProcessMsg.nParam2, (short)ProcessMsg.nParam3), ProcessMsg.Msg);
                    break;
                case Grobal2.CM_USERSELLITEM:
                    ClientUserSellItem(ProcessMsg.nParam1, HUtil32.MakeLong((ushort)ProcessMsg.nParam2, (ushort)ProcessMsg.nParam3), ProcessMsg.Msg);
                    break;
                case Grobal2.CM_USERBUYITEM:
                    ClientUserBuyItem(ProcessMsg.wIdent, ProcessMsg.nParam1, HUtil32.MakeLong((ushort)ProcessMsg.nParam2, (ushort)ProcessMsg.nParam3), 0, ProcessMsg.Msg);
                    break;
                case Grobal2.CM_USERGETDETAILITEM:
                    ClientUserBuyItem(ProcessMsg.wIdent, ProcessMsg.nParam1, 0, ProcessMsg.nParam2, ProcessMsg.Msg);
                    break;
                case Grobal2.CM_DROPGOLD:
                    if (ProcessMsg.nParam1 > 0)
                    {
                        ClientDropGold(ProcessMsg.nParam1);
                    }
                    break;
                case Grobal2.CM_TEST:
                    SendDefMessage(1, 0, 0, 0, 0, "");
                    break;
                case Grobal2.CM_GROUPMODE:
                    if (ProcessMsg.nParam2 == 0)
                    {
                        ClientGroupClose();
                    }
                    else
                    {
                        AllowGroup = true;
                    }
                    if (AllowGroup)
                    {
                        SendDefMessage(Grobal2.SM_GROUPMODECHANGED, 0, 1, 0, 0, "");
                    }
                    else
                    {
                        SendDefMessage(Grobal2.SM_GROUPMODECHANGED, 0, 0, 0, 0, "");
                    }
                    break;
                case Grobal2.CM_CREATEGROUP:
                    ClientCreateGroup(ProcessMsg.Msg.Trim());
                    break;
                case Grobal2.CM_ADDGROUPMEMBER:
                    ClientAddGroupMember(ProcessMsg.Msg.Trim());
                    break;
                case Grobal2.CM_DELGROUPMEMBER:
                    ClientDelGroupMember(ProcessMsg.Msg.Trim());
                    break;
                case Grobal2.CM_USERREPAIRITEM:
                    ClientRepairItem(ProcessMsg.nParam1, HUtil32.MakeLong((ushort)ProcessMsg.nParam2,(ushort) ProcessMsg.nParam3), ProcessMsg.Msg);
                    break;
                case Grobal2.CM_MERCHANTQUERYREPAIRCOST:
                    ClientQueryRepairCost(ProcessMsg.nParam1, HUtil32.MakeLong((ushort)ProcessMsg.nParam2, (ushort)ProcessMsg.nParam3), ProcessMsg.Msg);
                    break;
                case Grobal2.CM_DEALTRY:
                    ClientDealTry(ProcessMsg.Msg.Trim());
                    break;
                case Grobal2.CM_DEALADDITEM:
                    ClientAddDealItem(ProcessMsg.nParam1, ProcessMsg.Msg);
                    break;
                case Grobal2.CM_DEALDELITEM:
                    ClientDelDealItem(ProcessMsg.nParam1, ProcessMsg.Msg);
                    break;
                case Grobal2.CM_DEALCANCEL:
                    ClientCancelDeal();
                    break;
                case Grobal2.CM_DEALCHGGOLD:
                    ClientChangeDealGold(ProcessMsg.nParam1);
                    break;
                case Grobal2.CM_DEALEND:
                    ClientDealEnd();
                    break;
                case Grobal2.CM_USERSTORAGEITEM:
                    ClientStorageItem(ProcessMsg.nParam1, HUtil32.MakeLong((ushort)ProcessMsg.nParam2, (ushort)ProcessMsg.nParam3), ProcessMsg.Msg);
                    break;
                case Grobal2.CM_USERTAKEBACKSTORAGEITEM:
                    ClientTakeBackStorageItem(ProcessMsg.nParam1, HUtil32.MakeLong((ushort)ProcessMsg.nParam2, (ushort)ProcessMsg.nParam3), ProcessMsg.Msg);
                    break;
                case Grobal2.CM_WANTMINIMAP:
                    ClientGetMinMap();
                    break;
                case Grobal2.CM_USERMAKEDRUGITEM:
                    ClientMakeDrugItem(ProcessMsg.nParam1, ProcessMsg.Msg);
                    break;
                case Grobal2.CM_OPENGUILDDLG:
                    ClientOpenGuildDlg();
                    break;
                case Grobal2.CM_GUILDHOME:
                    ClientGuildHome();
                    break;
                case Grobal2.CM_GUILDMEMBERLIST:
                    ClientGuildMemberList();
                    break;
                case Grobal2.CM_GUILDADDMEMBER:
                    ClientGuildAddMember(ProcessMsg.Msg);
                    break;
                case Grobal2.CM_GUILDDELMEMBER:
                    ClientGuildDelMember(ProcessMsg.Msg);
                    break;
                case Grobal2.CM_GUILDUPDATENOTICE:
                    ClientGuildUpdateNotice(ProcessMsg.Msg);
                    break;
                case Grobal2.CM_GUILDUPDATERANKINFO:
                    ClientGuildUpdateRankInfo(ProcessMsg.Msg);
                    break;
                case Grobal2.CM_1042:
                    M2Share.Log.Warn("[非法数据] " + ChrName);
                    break;
                case Grobal2.CM_ADJUST_BONUS:
                    ClientAdjustBonus(ProcessMsg.nParam1, ProcessMsg.Msg);
                    break;
                case Grobal2.CM_GUILDALLY:
                    ClientGuildAlly();
                    break;
                case Grobal2.CM_GUILDBREAKALLY:
                    ClientGuildBreakAlly(ProcessMsg.Msg);
                    break;
                case Grobal2.CM_TURN:
                    if (ClientChangeDir((short)ProcessMsg.wIdent, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam, ref dwDelayTime))
                    {
                        m_dwActionTick = HUtil32.GetTickCount();
                        SendSocket(M2Share.GetGoodTick);
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");
                        }
                        else
                        {
                            nMsgCount = GetTurnMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxTurnMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Log.Warn(Format(CommandHelp.BunOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime < M2Share.Config.DropOverSpeed)
                                {
                                    SendSocket(M2Share.GetGoodTick);
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(Format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    SendDelayMsg(this, ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Grobal2.CM_WALK:
                    if (ClientWalkXY(ProcessMsg.wIdent, (short)ProcessMsg.nParam1, (short)ProcessMsg.nParam2, ProcessMsg.LateDelivery, ref dwDelayTime))
                    {
                        m_dwActionTick = HUtil32.GetTickCount();
                        SendSocket(M2Share.GetGoodTick);
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");
                        }
                        else
                        {
                            nMsgCount = GetWalkMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxWalkMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Log.Warn(Format(CommandHelp.WalkOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                                if (m_boTestSpeedMode)
                                {
                                    SysMsg(Format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                }
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.Config.DropOverSpeed && M2Share.Config.SpeedControlMode == 1 && m_boFilterAction)
                                {
                                    SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(Format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(Format("操作延迟 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                    SendDelayMsg(this, ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Grobal2.CM_HORSERUN:
                    if (ClientHorseRunXY(ProcessMsg.wIdent, (short)ProcessMsg.nParam1, (short)ProcessMsg.nParam2, ProcessMsg.LateDelivery, ref dwDelayTime))
                    {
                        m_dwActionTick = HUtil32.GetTickCount();
                        SendSocket(M2Share.GetGoodTick);
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");
                        }
                        else
                        {
                            nMsgCount = GetRunMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxRunMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Log.Warn(Format(CommandHelp.RunOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, ""); // 如果超速则发送攻击失败信息
                                if (m_boTestSpeedMode)
                                {
                                    SysMsg(Format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                }
                            }
                            else
                            {
                                if (m_boTestSpeedMode)
                                {
                                    SysMsg(Format("操作延迟 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                }
                                SendDelayMsg(this, ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                result = false;
                            }
                        }
                    }
                    break;
                case Grobal2.CM_RUN:
                    if (ClientRunXY(ProcessMsg.wIdent, (short)ProcessMsg.nParam1, (short)ProcessMsg.nParam2, ProcessMsg.nParam3, ref dwDelayTime))
                    {
                        m_dwActionTick = HUtil32.GetTickCount();
                        SendSocket(M2Share.GetGoodTick);
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");
                        }
                        else
                        {
                            nMsgCount = GetRunMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxRunMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Log.Warn(Format(CommandHelp.RunOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, ""); // 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.Config.DropOverSpeed && M2Share.Config.SpeedControlMode == 1 && m_boFilterAction)
                                {
                                    SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(Format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(Format("操作延迟 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                    SendDelayMsg(this, ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, Grobal2.CM_RUN, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Grobal2.CM_HIT:
                case Grobal2.CM_HEAVYHIT:
                case Grobal2.CM_BIGHIT:
                case Grobal2.CM_POWERHIT:
                case Grobal2.CM_LONGHIT:
                case Grobal2.CM_WIDEHIT:
                case Grobal2.CM_CRSHIT:
                case Grobal2.CM_TWINHIT:
                case Grobal2.CM_FIREHIT:
                    if (ClientHitXY(ProcessMsg.wIdent, ProcessMsg.nParam1, ProcessMsg.nParam2, (byte)ProcessMsg.wParam, ProcessMsg.LateDelivery, ref dwDelayTime))
                    {
                        m_dwActionTick = HUtil32.GetTickCount();
                        SendSocket(M2Share.GetGoodTick);
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");
                        }
                        else
                        {
                            nMsgCount = GetHitMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxHitMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Log.Warn(Format(CommandHelp.HitOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.Config.DropOverSpeed && M2Share.Config.SpeedControlMode == 1 && m_boFilterAction)
                                {
                                    SendSocket(M2Share.GetGoodTick);
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(Format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg("操作延迟 Ident: " + ProcessMsg.wIdent + " Time: " + dwDelayTime, MsgColor.Red, MsgType.Hint);
                                    }
                                    SendDelayMsg(this, ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Grobal2.CM_SITDOWN:
                    if (ClientSitDownHit(ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam, ref dwDelayTime))
                    {
                        m_dwActionTick = HUtil32.GetTickCount();
                        SendSocket(M2Share.GetGoodTick);
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");
                        }
                        else
                        {
                            nMsgCount = GetSiteDownMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxSitDonwMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Log.Warn(Format(CommandHelp.BunOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime < M2Share.Config.DropOverSpeed)
                                {
                                    SendSocket(M2Share.GetGoodTick);
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(Format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(Format("操作延迟 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                    SendDelayMsg(this, ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Grobal2.CM_SPELL:
                    if (ClientSpellXY(ProcessMsg.wIdent, ProcessMsg.wParam, (short)ProcessMsg.nParam1, (short)ProcessMsg.nParam2, M2Share.ActorMgr.Get(ProcessMsg.nParam3), ProcessMsg.LateDelivery, ref dwDelayTime))
                    {
                        m_dwActionTick = HUtil32.GetTickCount();
                        SendSocket(M2Share.GetGoodTick);
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");
                        }
                        else
                        {
                            nMsgCount = GetSpellMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxSpellMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Log.Warn(Format(CommandHelp.SpellOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.Config.DropOverSpeed && M2Share.Config.SpeedControlMode == 1 && m_boFilterAction)
                                {
                                    SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(Format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(Format("操作延迟 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                    SendDelayMsg(this, ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Grobal2.CM_SAY:
                    ProcessUserLineMsg(ProcessMsg.Msg);
                    break;
                case Grobal2.CM_PASSWORD:
                    ProcessClientPassword(ProcessMsg);
                    break;
                case Grobal2.CM_QUERYVAL:
                    ProcessQueryValue(ProcessMsg.nParam1, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_WALK:
                    if (ProcessMsg.BaseObject != this.ActorId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_WALK, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord((ushort)ProcessMsg.wParam, (ushort)BaseObject.Light));
                        CharDesc = new CharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.CharStatus;
                        SendSocket(m_DefMsg, EDCode.EncodeBuffer(CharDesc));
                    }
                    break;
                case Grobal2.RM_HORSERUN:
                    if (ProcessMsg.BaseObject != this.ActorId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_HORSERUN, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord((ushort)ProcessMsg.wParam, (ushort)BaseObject.Light));
                        CharDesc = new CharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.CharStatus;
                        SendSocket(m_DefMsg, EDCode.EncodeBuffer(CharDesc));
                    }
                    break;
                case Grobal2.RM_RUN:
                    if (ProcessMsg.BaseObject != this.ActorId && BaseObject != null)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_RUN, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord((ushort)ProcessMsg.wParam, (ushort)BaseObject.Light));
                        CharDesc = new CharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.CharStatus;
                        SendSocket(m_DefMsg, EDCode.EncodeBuffer(CharDesc));
                    }
                    break;
                case Grobal2.RM_HIT:
                    if (ProcessMsg.BaseObject != this.ActorId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_HIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_HEAVYHIT:
                    if (ProcessMsg.BaseObject != this.ActorId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_HEAVYHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg, ProcessMsg.Msg);
                    }
                    break;
                case Grobal2.RM_BIGHIT:
                    if (ProcessMsg.BaseObject != this.ActorId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_BIGHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_SPELL:
                    if (ProcessMsg.BaseObject != this.ActorId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SPELL, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg, ProcessMsg.nParam3.ToString());
                    }
                    break;
                case Grobal2.RM_SPELL2:
                    if (ProcessMsg.BaseObject != this.ActorId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_POWERHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_MOVEFAIL:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_MOVEFAIL, ActorId, CurrX, CurrY, Direction);
                    CharDesc = new CharDesc();
                    CharDesc.Feature = BaseObject.GetFeatureToLong();
                    CharDesc.Status = BaseObject.CharStatus;
                    SendSocket(m_DefMsg, EDCode.EncodeBuffer(CharDesc));
                    break;
                case Grobal2.RM_LONGHIT:
                    if (ProcessMsg.BaseObject != this.ActorId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_LONGHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_WIDEHIT:
                    if (ProcessMsg.BaseObject != this.ActorId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_WIDEHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_FIREHIT:
                    if (ProcessMsg.BaseObject != this.ActorId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_FIREHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_CRSHIT:
                    if (ProcessMsg.BaseObject != this.ActorId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CRSHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_41:
                    if (ProcessMsg.BaseObject != this.ActorId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_41, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_TWINHIT:
                    if (ProcessMsg.BaseObject != this.ActorId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_TWINHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_43:
                    if (ProcessMsg.BaseObject != this.ActorId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_43, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_TURN:
                case Grobal2.RM_PUSH:
                case Grobal2.RM_RUSH:
                case Grobal2.RM_RUSHKUNG:
                    if (ProcessMsg.BaseObject != this.ActorId || ProcessMsg.wIdent == Grobal2.RM_PUSH || ProcessMsg.wIdent == Grobal2.RM_RUSH || ProcessMsg.wIdent == Grobal2.RM_RUSHKUNG)
                    {
                        switch (ProcessMsg.wIdent)
                        {
                            case Grobal2.RM_PUSH:
                                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_BACKSTEP, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord((ushort)ProcessMsg.wParam, (ushort)BaseObject.Light));
                                break;
                            case Grobal2.RM_RUSH:
                                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_RUSH, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord((ushort)ProcessMsg.wParam, (ushort)BaseObject.Light));
                                break;
                            case Grobal2.RM_RUSHKUNG:
                                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_RUSHKUNG, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord((ushort)ProcessMsg.wParam, (ushort)BaseObject.Light));
                                break;
                            default:
                                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_TURN, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord((ushort)ProcessMsg.wParam, (ushort)BaseObject.Light));
                                break;
                        }
                        CharDesc = new CharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.CharStatus;
                        sendMsg = EDCode.EncodeBuffer(CharDesc);
                        nObjCount = GetChrColor(BaseObject);
                        if (!string.IsNullOrEmpty(ProcessMsg.Msg))
                        {
                            sendMsg = sendMsg + EDCode.EncodeString($"{ProcessMsg.Msg}/{nObjCount}");
                        }
                        SendSocket(m_DefMsg, sendMsg);
                        if (ProcessMsg.wIdent == Grobal2.RM_TURN)
                        {
                            nObjCount = BaseObject.GetFeatureToLong();
                            SendDefMessage(Grobal2.SM_FEATURECHANGED, ProcessMsg.BaseObject, HUtil32.LoWord(nObjCount), HUtil32.HiWord(nObjCount), BaseObject.GetFeatureEx(), "");
                        }
                    }
                    break;
                case Grobal2.RM_STRUCK:
                case Grobal2.RM_STRUCK_MAG:
                    if (ProcessMsg.wParam > 0)
                    {
                        if (ProcessMsg.BaseObject == ActorId)
                        {
                            if (M2Share.ActorMgr.Get(ProcessMsg.nParam3) != null)
                            {
                                if (M2Share.ActorMgr.Get(ProcessMsg.nParam3).Race == ActorRace.Play)
                                {
                                    SetPkFlag(M2Share.ActorMgr.Get(ProcessMsg.nParam3));
                                }
                                SetLastHiter(M2Share.ActorMgr.Get(ProcessMsg.nParam3));
                            }
                            if (M2Share.CastleMgr.IsCastleMember(this) != null && M2Share.ActorMgr.Get(ProcessMsg.nParam3) != null)
                            {
                                M2Share.ActorMgr.Get(ProcessMsg.nParam3).BoCrimeforCastle = true;
                                M2Share.ActorMgr.Get(ProcessMsg.nParam3).CrimeforCastleTime = HUtil32.GetTickCount();
                            }
                            HealthTick = 0;
                            SpellTick = 0;
                            PerHealth -= 1;
                            PerSpell -= 1;
                            StruckTick = HUtil32.GetTickCount();
                        }
                        if (ProcessMsg.BaseObject != 0)
                        {
                            if (ProcessMsg.BaseObject == ActorId && M2Share.Config.DisableSelfStruck || BaseObject.Race == ActorRace.Play && M2Share.Config.DisableStruck)
                            {
                                BaseObject.SendRefMsg(Grobal2.RM_HEALTHSPELLCHANGED, 0, 0, 0, 0, "");
                            }
                            else
                            {
                                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_STRUCK, ProcessMsg.BaseObject, BaseObject.WAbil.HP, BaseObject.WAbil.MaxHP, ProcessMsg.wParam);
                                MessageBodyWL = new MessageBodyWL();
                                MessageBodyWL.Param1 = BaseObject.GetFeature(this);
                                MessageBodyWL.Param2 = BaseObject.CharStatus;
                                MessageBodyWL.Tag1 = ProcessMsg.nParam3;
                                if (ProcessMsg.wIdent == Grobal2.RM_STRUCK_MAG)
                                {
                                    MessageBodyWL.Tag2 = 1;
                                }
                                else
                                {
                                    MessageBodyWL.Tag2 = 0;
                                }
                                SendSocket(m_DefMsg, EDCode.EncodeBuffer(MessageBodyWL));
                            }
                        }
                    }
                    break;
                case Grobal2.RM_DEATH:
                    if (ProcessMsg.nParam3 == 1)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_NOWDEATH, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        if (ProcessMsg.BaseObject == ActorId)
                        {
                            if (M2Share.g_FunctionNPC != null)
                            {
                                M2Share.g_FunctionNPC.GotoLable(this, "@OnDeath", false);
                            }
                        }
                    }
                    else
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DEATH, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                    }
                    CharDesc = new CharDesc();
                    CharDesc.Feature = BaseObject.GetFeature(this);
                    CharDesc.Status = BaseObject.CharStatus;
                    SendSocket(m_DefMsg, EDCode.EncodeBuffer(CharDesc));
                    break;
                case Grobal2.RM_DISAPPEAR:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DISAPPEAR, ProcessMsg.BaseObject, 0, 0, 0);
                    SendSocket(m_DefMsg);
                    break;
                case Grobal2.RM_SKELETON:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SKELETON, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                    CharDesc = new CharDesc();
                    CharDesc.Feature = BaseObject.GetFeature(this);
                    CharDesc.Status = BaseObject.CharStatus;
                    SendSocket(m_DefMsg, EDCode.EncodeBuffer(CharDesc));
                    break;
                case Grobal2.RM_USERNAME:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_USERNAME, ProcessMsg.BaseObject, GetChrColor(BaseObject), 0, 0);
                    SendSocket(m_DefMsg, EDCode.EncodeString(ProcessMsg.Msg));
                    break;
                case Grobal2.RM_WINEXP:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_WINEXP, Abil.Exp, HUtil32.LoWord(ProcessMsg.nParam1), HUtil32.HiWord(ProcessMsg.nParam1), 0);
                    SendSocket(m_DefMsg);
                    break;
                case Grobal2.RM_LEVELUP:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_LEVELUP, Abil.Exp, Abil.Level, 0, 0);
                    SendSocket(m_DefMsg);
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ABILITY, Gold, HUtil32.MakeWord((byte)Job, 99), HUtil32.LoWord(m_nGameGold), HUtil32.HiWord(m_nGameGold));
                    SendSocket(m_DefMsg, EDCode.EncodeBuffer(WAbil));
                    SendDefMessage(Grobal2.SM_SUBABILITY, HUtil32.MakeLong(HUtil32.MakeWord(AntiMagic, 0), 0), HUtil32.MakeWord(HitPoint, SpeedPoint), HUtil32.MakeWord(AntiPoison, PoisonRecover), HUtil32.MakeWord(HealthRecover, SpellRecover), "");
                    break;
                case Grobal2.RM_CHANGENAMECOLOR:
                    SendDefMessage(Grobal2.SM_CHANGENAMECOLOR, ProcessMsg.BaseObject, GetChrColor(BaseObject), 0, 0, "");
                    break;
                case Grobal2.RM_LOGON:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEWMAP, ActorId, CurrX, CurrY, DayBright());
                    SendSocket(m_DefMsg, EDCode.EncodeString(MapFileName));
                    SendMsg(this, Grobal2.RM_CHANGELIGHT, 0, 0, 0, 0, "");
                    SendLogon();
                    SendServerConfig();
                    ClientQueryUserName(ActorId, CurrX, CurrY);
                    RefUserState();
                    SendMapDescription();
                    SendGoldInfo(true);
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_VERSION_FAIL, M2Share.Config.nClientFile1_CRC, HUtil32.LoWord(M2Share.Config.nClientFile2_CRC), HUtil32.HiWord(M2Share.Config.nClientFile2_CRC), 0);
                    SendSocket(m_DefMsg, "<<<<<<");
                    break;
                case Grobal2.RM_HEAR:
                case Grobal2.RM_WHISPER:
                case Grobal2.RM_CRY:
                case Grobal2.RM_SYSMESSAGE:
                case Grobal2.RM_GROUPMESSAGE:
                case Grobal2.RM_SYSMESSAGE2:
                case Grobal2.RM_GUILDMESSAGE:
                case Grobal2.RM_SYSMESSAGE3:
                case Grobal2.RM_MOVEMESSAGE:
                case Grobal2.RM_MERCHANTSAY:
                    switch (ProcessMsg.wIdent)
                    {
                        case Grobal2.RM_HEAR:
                            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_HEAR, ProcessMsg.BaseObject, HUtil32.MakeWord((ushort)ProcessMsg.nParam1, (ushort)ProcessMsg.nParam2), 0, 1);
                            break;
                        case Grobal2.RM_WHISPER:
                            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_WHISPER, ProcessMsg.BaseObject, HUtil32.MakeWord((ushort)ProcessMsg.nParam1, (ushort)ProcessMsg.nParam2), 0, 1);
                            break;
                        case Grobal2.RM_CRY:
                            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_HEAR, ProcessMsg.BaseObject, HUtil32.MakeWord((ushort)ProcessMsg.nParam1, (ushort)ProcessMsg.nParam2), 0, 1);
                            break;
                        case Grobal2.RM_SYSMESSAGE:
                            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SYSMESSAGE, ProcessMsg.BaseObject, HUtil32.MakeWord((ushort)ProcessMsg.nParam1, (ushort)ProcessMsg.nParam2), 0, 1);
                            break;
                        case Grobal2.RM_GROUPMESSAGE:
                            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SYSMESSAGE, ProcessMsg.BaseObject, HUtil32.MakeWord((ushort)ProcessMsg.nParam1, (ushort)ProcessMsg.nParam2), 0, 1);
                            break;
                        case Grobal2.RM_GUILDMESSAGE:
                            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_GUILDMESSAGE, ProcessMsg.BaseObject, HUtil32.MakeWord((ushort)ProcessMsg.nParam1, (ushort)ProcessMsg.nParam2), 0, 1);
                            break;
                        case Grobal2.RM_MERCHANTSAY:
                            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_MERCHANTSAY, ProcessMsg.BaseObject, HUtil32.MakeWord((ushort)ProcessMsg.nParam1, (ushort)ProcessMsg.nParam2), 0, 1);
                            break;
                        case Grobal2.RM_MOVEMESSAGE:
                            this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_MOVEMESSAGE, ProcessMsg.BaseObject, HUtil32.MakeWord((ushort)ProcessMsg.nParam1, (ushort)ProcessMsg.nParam2), ProcessMsg.nParam3, ProcessMsg.wParam);
                            break;
                    }
                    SendSocket(m_DefMsg, EDCode.EncodeString(ProcessMsg.Msg));
                    break;
                case Grobal2.RM_ABILITY:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ABILITY, Gold, HUtil32.MakeWord((byte)Job, 99), HUtil32.LoWord(m_nGameGold), HUtil32.HiWord(m_nGameGold));
                    SendSocket(m_DefMsg, EDCode.EncodeBuffer(WAbil));
                    break;
                case Grobal2.RM_HEALTHSPELLCHANGED:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_HEALTHSPELLCHANGED, ProcessMsg.BaseObject, BaseObject.WAbil.HP, BaseObject.WAbil.MP, BaseObject.WAbil.MaxHP);
                    SendSocket(m_DefMsg);
                    break;
                case Grobal2.RM_DAYCHANGING:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DAYCHANGING, 0, m_btBright, DayBright(), 0);
                    SendSocket(m_DefMsg);
                    break;
                case Grobal2.RM_ITEMSHOW:
                    SendDefMessage(Grobal2.SM_ITEMSHOW, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_ITEMHIDE:
                    SendDefMessage(Grobal2.SM_ITEMHIDE, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, 0, "");
                    break;
                case Grobal2.RM_DOOROPEN:
                    SendDefMessage(Grobal2.SM_OPENDOOR_OK, 0, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, "");
                    break;
                case Grobal2.RM_DOORCLOSE:
                    SendDefMessage(Grobal2.SM_CLOSEDOOR, 0, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, "");
                    break;
                case Grobal2.RM_SENDUSEITEMS:
                    SendUseItems();
                    break;
                case Grobal2.RM_WEIGHTCHANGED:
                    SendDefMessage(Grobal2.SM_WEIGHTCHANGED, WAbil.Weight, WAbil.WearWeight, WAbil.HandWeight, (((WAbil.Weight + WAbil.WearWeight + WAbil.HandWeight) ^ 0x3A5F) ^ 0x1F35) ^ 0xaa21, "");
                    break;
                case Grobal2.RM_FEATURECHANGED:
                    SendDefMessage(Grobal2.SM_FEATURECHANGED, ProcessMsg.BaseObject, HUtil32.LoWord(ProcessMsg.nParam1), HUtil32.HiWord(ProcessMsg.nParam1), ProcessMsg.wParam, "");
                    break;
                case Grobal2.RM_CLEAROBJECTS:
                    SendDefMessage(Grobal2.SM_CLEAROBJECTS, 0, 0, 0, 0, "");
                    break;
                case Grobal2.RM_CHANGEMAP:
                    SendDefMessage(Grobal2.SM_CHANGEMAP, ActorId, CurrX, CurrY, DayBright(), ProcessMsg.Msg);
                    RefUserState();
                    SendMapDescription();
                    SendServerConfig();
                    break;
                case Grobal2.RM_BUTCH:
                    if (ProcessMsg.BaseObject != 0)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_BUTCH, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_MAGICFIRE:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_MAGICFIRE, ProcessMsg.BaseObject, HUtil32.LoWord(ProcessMsg.nParam2), HUtil32.HiWord(ProcessMsg.nParam2), ProcessMsg.nParam1);
                    var by = BitConverter.GetBytes(ProcessMsg.nParam3);
                    var sSendStr = EDCode.EncodeBuffer(by, by.Length);
                    SendSocket(m_DefMsg, sSendStr);
                    break;
                case Grobal2.RM_MAGICFIREFAIL:
                    SendDefMessage(Grobal2.SM_MAGICFIRE_FAIL, ProcessMsg.BaseObject, 0, 0, 0, "");
                    break;
                case Grobal2.RM_SENDMYMAGIC:
                    SendUseMagic();
                    break;
                case Grobal2.RM_MAGIC_LVEXP:
                    SendDefMessage(Grobal2.SM_MAGIC_LVEXP, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.LoWord(ProcessMsg.nParam3), HUtil32.HiWord(ProcessMsg.nParam3), "");
                    break;
                case Grobal2.RM_DURACHANGE:
                    SendDefMessage(Grobal2.SM_DURACHANGE, ProcessMsg.nParam1, ProcessMsg.wParam, HUtil32.LoWord(ProcessMsg.nParam2), HUtil32.HiWord(ProcessMsg.nParam2), "");
                    break;
                case Grobal2.RM_MERCHANTDLGCLOSE:
                    SendDefMessage(Grobal2.SM_MERCHANTDLGCLOSE, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case Grobal2.RM_SENDGOODSLIST:
                    SendDefMessage(Grobal2.SM_SENDGOODSLIST, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, 0, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_SENDUSERSELL:
                    SendDefMessage(Grobal2.SM_SENDUSERSELL, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, 0, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_SENDBUYPRICE:
                    SendDefMessage(Grobal2.SM_SENDBUYPRICE, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case Grobal2.RM_USERSELLITEM_OK:
                    SendDefMessage(Grobal2.SM_USERSELLITEM_OK, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case Grobal2.RM_USERSELLITEM_FAIL:
                    SendDefMessage(Grobal2.SM_USERSELLITEM_FAIL, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case Grobal2.RM_BUYITEM_SUCCESS:
                    SendDefMessage(Grobal2.SM_BUYITEM_SUCCESS, ProcessMsg.nParam1, HUtil32.LoWord(ProcessMsg.nParam2), HUtil32.HiWord(ProcessMsg.nParam2), 0, "");
                    break;
                case Grobal2.RM_BUYITEM_FAIL:
                    SendDefMessage(Grobal2.SM_BUYITEM_FAIL, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case Grobal2.RM_SENDDETAILGOODSLIST:
                    SendDefMessage(Grobal2.SM_SENDDETAILGOODSLIST, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, 0, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_GOLDCHANGED:
                    SendDefMessage(Grobal2.SM_GOLDCHANGED, Gold, HUtil32.LoWord(m_nGameGold), HUtil32.HiWord(m_nGameGold), 0, "");
                    break;
                case Grobal2.RM_GAMEGOLDCHANGED:
                    SendGoldInfo(false);
                    break;
                case Grobal2.RM_CHANGELIGHT:
                    SendDefMessage(Grobal2.SM_CHANGELIGHT, ProcessMsg.BaseObject, (short)BaseObject.Light, (short)M2Share.Config.nClientKey, 0, "");
                    break;
                case Grobal2.RM_LAMPCHANGEDURA:
                    SendDefMessage(Grobal2.SM_LAMPCHANGEDURA, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case Grobal2.RM_CHARSTATUSCHANGED:
                    SendDefMessage(Grobal2.SM_CHARSTATUSCHANGED, ProcessMsg.BaseObject, HUtil32.LoWord(ProcessMsg.nParam1), HUtil32.HiWord(ProcessMsg.nParam1), ProcessMsg.wParam, "");
                    break;
                case Grobal2.RM_GROUPCANCEL:
                    SendDefMessage(Grobal2.SM_GROUPCANCEL, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case Grobal2.RM_SENDUSERREPAIR:
                case Grobal2.RM_SENDUSERSREPAIR:
                    SendDefMessage(Grobal2.SM_SENDUSERREPAIR, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, 0, "");
                    break;
                case Grobal2.RM_USERREPAIRITEM_OK:
                    SendDefMessage(Grobal2.SM_USERREPAIRITEM_OK, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, 0, "");
                    break;
                case Grobal2.RM_SENDREPAIRCOST:
                    SendDefMessage(Grobal2.SM_SENDREPAIRCOST, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case Grobal2.RM_USERREPAIRITEM_FAIL:
                    SendDefMessage(Grobal2.SM_USERREPAIRITEM_FAIL, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case Grobal2.RM_USERSTORAGEITEM:
                    SendDefMessage(Grobal2.SM_SENDUSERSTORAGEITEM, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, 0, "");
                    break;
                case Grobal2.RM_USERGETBACKITEM:
                    SendSaveItemList(ProcessMsg.nParam1);
                    break;
                case Grobal2.RM_SENDDELITEMLIST:
                    var delItemList = (IList<DeleteItem>)M2Share.ActorMgr.GetOhter(ProcessMsg.nParam1);
                    SendDelItemList(delItemList);
                    break;
                case Grobal2.RM_USERMAKEDRUGITEMLIST:
                    SendDefMessage(Grobal2.SM_SENDUSERMAKEDRUGITEMLIST, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, 0, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_MAKEDRUG_SUCCESS:
                    SendDefMessage(Grobal2.SM_MAKEDRUG_SUCCESS, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case Grobal2.RM_MAKEDRUG_FAIL:
                    SendDefMessage(Grobal2.SM_MAKEDRUG_FAIL, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case Grobal2.RM_ALIVE:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ALIVE, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                    CharDesc = new CharDesc();
                    CharDesc.Feature = BaseObject.GetFeature(this);
                    CharDesc.Status = BaseObject.CharStatus;
                    SendSocket(m_DefMsg, EDCode.EncodeBuffer(CharDesc));
                    break;
                case Grobal2.RM_DIGUP:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DIGUP, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord((ushort)ProcessMsg.wParam, (ushort)BaseObject.Light));
                    MessageBodyWL = new MessageBodyWL();
                    MessageBodyWL.Param1 = BaseObject.GetFeature(this);
                    MessageBodyWL.Param2 = BaseObject.CharStatus;
                    MessageBodyWL.Tag1 = ProcessMsg.nParam3;
                    MessageBodyWL.Tag1 = 0;
                    sendMsg = EDCode.EncodeBuffer(MessageBodyWL);
                    SendSocket(m_DefMsg, sendMsg);
                    break;
                case Grobal2.RM_DIGDOWN:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DIGDOWN, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, 0);
                    SendSocket(m_DefMsg);
                    break;
                case Grobal2.RM_FLYAXE:
                    if (M2Share.ActorMgr.Get(ProcessMsg.nParam3) != null)
                    {
                        var MessageBodyW = new MessageBodyW();
                        MessageBodyW.Param1 = (ushort)M2Share.ActorMgr.Get(ProcessMsg.nParam3).CurrX;
                        MessageBodyW.Param2 = (ushort)M2Share.ActorMgr.Get(ProcessMsg.nParam3).CurrY;
                        MessageBodyW.Tag1 = HUtil32.LoWord(ProcessMsg.nParam3);
                        MessageBodyW.Tag2 = HUtil32.HiWord(ProcessMsg.nParam3);
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_FLYAXE, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg, EDCode.EncodeBuffer(MessageBodyW));
                    }
                    break;
                case Grobal2.RM_LIGHTING:
                    if (M2Share.ActorMgr.Get(ProcessMsg.nParam3) != null)
                    {
                        MessageBodyWL = new MessageBodyWL();
                        MessageBodyWL.Param1 = M2Share.ActorMgr.Get(ProcessMsg.nParam3).CurrX;
                        MessageBodyWL.Param2 = M2Share.ActorMgr.Get(ProcessMsg.nParam3).CurrY;
                        MessageBodyWL.Tag1 = ProcessMsg.nParam3;
                        MessageBodyWL.Tag2 = ProcessMsg.wParam;
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_LIGHTING, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, BaseObject.Direction);
                        SendSocket(m_DefMsg, EDCode.EncodeBuffer(MessageBodyWL));
                    }
                    break;
                case Grobal2.RM_10205:
                    SendDefMessage(Grobal2.SM_716, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "");
                    break;
                case Grobal2.RM_CHANGEGUILDNAME:
                    SendChangeGuildName();
                    break;
                case Grobal2.RM_SUBABILITY:
                    SendDefMessage(Grobal2.SM_SUBABILITY, HUtil32.MakeLong(HUtil32.MakeWord(AntiMagic, 0), 0), HUtil32.MakeWord(HitPoint, SpeedPoint), HUtil32.MakeWord(AntiPoison, PoisonRecover), HUtil32.MakeWord(HealthRecover, SpellRecover), "");
                    break;
                case Grobal2.RM_BUILDGUILD_OK:
                    SendDefMessage(Grobal2.SM_BUILDGUILD_OK, 0, 0, 0, 0, "");
                    break;
                case Grobal2.RM_BUILDGUILD_FAIL:
                    SendDefMessage(Grobal2.SM_BUILDGUILD_FAIL, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case Grobal2.RM_DONATE_OK:
                    SendDefMessage(Grobal2.SM_DONATE_OK, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case Grobal2.RM_DONATE_FAIL:
                    SendDefMessage(Grobal2.SM_DONATE_FAIL, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case Grobal2.RM_MYSTATUS:
                    SendDefMessage(Grobal2.SM_MYSTATUS, 0, (short)GetMyStatus(), 0, 0, "");
                    break;
                case Grobal2.RM_MENU_OK:
                    SendDefMessage(Grobal2.SM_MENU_OK, ProcessMsg.nParam1, 0, 0, 0, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_SPACEMOVE_FIRE:
                case Grobal2.RM_SPACEMOVE_FIRE2:
                    if (ProcessMsg.wIdent == Grobal2.RM_SPACEMOVE_FIRE)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SPACEMOVE_HIDE, ProcessMsg.BaseObject, 0, 0, 0);
                    }
                    else
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SPACEMOVE_HIDE2, ProcessMsg.BaseObject, 0, 0, 0);
                    }
                    SendSocket(m_DefMsg);
                    break;
                case Grobal2.RM_SPACEMOVE_SHOW:
                case Grobal2.RM_SPACEMOVE_SHOW2:
                    if (ProcessMsg.wIdent == Grobal2.RM_SPACEMOVE_SHOW)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SPACEMOVE_SHOW, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord((ushort)ProcessMsg.wParam, (ushort)BaseObject.Light));
                    }
                    else
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SPACEMOVE_SHOW2, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord((ushort)ProcessMsg.wParam, (ushort)BaseObject.Light));
                    }
                    CharDesc = new CharDesc();
                    CharDesc.Feature = BaseObject.GetFeature(this);
                    CharDesc.Status = BaseObject.CharStatus;
                    sendMsg = EDCode.EncodeBuffer(CharDesc);
                    nObjCount = GetChrColor(BaseObject);
                    if (ProcessMsg.Msg != "")
                    {
                        sendMsg = sendMsg + EDCode.EncodeString(ProcessMsg.Msg + '/' + nObjCount);
                    }
                    SendSocket(m_DefMsg, sendMsg);
                    break;
                case Grobal2.RM_RECONNECTION:
                    m_boReconnection = true;
                    SendDefMessage(Grobal2.SM_RECONNECT, 0, 0, 0, 0, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_HIDEEVENT:
                    SendDefMessage(Grobal2.SM_HIDEEVENT, ProcessMsg.nParam1, ProcessMsg.wParam, ProcessMsg.nParam2, ProcessMsg.nParam3, "");
                    break;
                case Grobal2.RM_SHOWEVENT:
                    var ShortMessage = new ShortMessage();
                    ShortMessage.Ident = HUtil32.HiWord(ProcessMsg.nParam2);
                    ShortMessage.wMsg = 0;
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SHOWEVENT, ProcessMsg.nParam1, ProcessMsg.wParam, ProcessMsg.nParam2, ProcessMsg.nParam3);
                    SendSocket(m_DefMsg, EDCode.EncodeBuffer(ShortMessage));
                    break;
                case Grobal2.RM_ADJUST_BONUS:
                    SendAdjustBonus();
                    break;
                case Grobal2.RM_10401:
                    ChangeServerMakeSlave((SlaveInfo)M2Share.ActorMgr.GetOhter(ProcessMsg.nParam1));
                    break;
                case Grobal2.RM_OPENHEALTH:
                    SendDefMessage(Grobal2.SM_OPENHEALTH, ProcessMsg.BaseObject, BaseObject.WAbil.HP, BaseObject.WAbil.MaxHP, 0, "");
                    break;
                case Grobal2.RM_CLOSEHEALTH:
                    SendDefMessage(Grobal2.SM_CLOSEHEALTH, ProcessMsg.BaseObject, 0, 0, 0, "");
                    break;
                case Grobal2.RM_BREAKWEAPON:
                    SendDefMessage(Grobal2.SM_BREAKWEAPON, ProcessMsg.BaseObject, 0, 0, 0, "");
                    break;
                case Grobal2.RM_10414:
                    SendDefMessage(Grobal2.SM_INSTANCEHEALGUAGE, ProcessMsg.BaseObject, BaseObject.WAbil.HP, BaseObject.WAbil.MaxHP, 0, "");
                    break;
                case Grobal2.RM_CHANGEFACE:
                    if (ProcessMsg.nParam1 != 0 && ProcessMsg.nParam2 != 0)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CHANGEFACE, ProcessMsg.nParam1, HUtil32.LoWord(ProcessMsg.nParam2), HUtil32.HiWord(ProcessMsg.nParam2), 0);
                        CharDesc = new CharDesc();
                        CharDesc.Feature = M2Share.ActorMgr.Get(ProcessMsg.nParam2).GetFeature(this);
                        CharDesc.Status = M2Share.ActorMgr.Get(ProcessMsg.nParam2).CharStatus;
                        SendSocket(m_DefMsg, EDCode.EncodeBuffer(CharDesc));
                    }
                    break;
                case Grobal2.RM_PASSWORD:
                    SendDefMessage(Grobal2.SM_PASSWORD, 0, 0, 0, 0, "");
                    break;
                case Grobal2.RM_PLAYDICE:
                    MessageBodyWL = new MessageBodyWL();
                    MessageBodyWL.Param1 = ProcessMsg.nParam1;
                    MessageBodyWL.Param2 = ProcessMsg.nParam2;
                    MessageBodyWL.Tag1 = ProcessMsg.nParam3;
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_PLAYDICE, ProcessMsg.BaseObject, ProcessMsg.wParam, 0, 0);
                    SendSocket(m_DefMsg, EDCode.EncodeBuffer(MessageBodyWL) + EDCode.EncodeString(ProcessMsg.Msg));
                    break;
                case Grobal2.RM_PASSWORDSTATUS:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_PASSWORDSTATUS, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3);
                    SendSocket(m_DefMsg, ProcessMsg.Msg);
                    break;
                // ---------------------------元宝寄售系统---------------------------------------
                case Grobal2.RM_SENDDEALOFFFORM:// 打开出售物品窗口
                    SendDefMessage(Grobal2.SM_SENDDEALOFFFORM, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, 0, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_QUERYYBSELL:// 查询正在出售的物品
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_QUERYYBSELL, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_QUERYYBDEAL:// 查询可以的购买物品
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_QUERYYBDEAL, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.Msg);
                    break;
                case Grobal2.CM_SELLOFFADDITEM:// 客户端往出售物品窗口里加物品 
                    ClientAddSellOffItem(ProcessMsg.nParam1, ProcessMsg.Msg);
                    break;
                case Grobal2.CM_SELLOFFDELITEM:// 客户端删除出售物品窗里的物品 
                    ClientDelSellOffItem(ProcessMsg.nParam1, ProcessMsg.Msg);
                    break;
                case Grobal2.CM_SELLOFFCANCEL:// 客户端取消元宝寄售 
                    ClientCancelSellOff();
                    break;
                case Grobal2.CM_SELLOFFEND:// 客户端元宝寄售结束 
                    ClientSellOffEnd(ProcessMsg.Msg, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3);
                    break;
                case Grobal2.CM_CANCELSELLOFFITEMING:// 取消正在寄售的物品(出售人)
                    ClientCancelSellOffIng();
                    break;
                case Grobal2.CM_SELLOFFBUYCANCEL:// 取消寄售 物品购买(购买人)
                    ClientBuyCancelSellOff(ProcessMsg.Msg);// 出售人
                    break;
                case Grobal2.CM_SELLOFFBUY:// 确定购买寄售物品
                    ClientBuySellOffItme(ProcessMsg.Msg);// 出售人
                    break;
                case Grobal2.RM_SELLOFFCANCEL:// 元宝寄售取消出售
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SellOffCANCEL, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_SELLOFFADDITEM_OK:// 客户端往出售物品窗口里加物品 成功
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELLOFFADDITEM_OK, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_SellOffADDITEM_FAIL:// 客户端往出售物品窗口里加物品 失败
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SellOffADDITEM_FAIL, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_SELLOFFDELITEM_OK:// 客户端删除出售物品窗里的物品 成功
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELLOFFDELITEM_OK, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_SELLOFFDELITEM_FAIL:// 客户端删除出售物品窗里的物品 失败
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELLOFFDELITEM_FAIL, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_SELLOFFEND_OK:// 客户端元宝寄售结束 成功
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELLOFFEND_OK, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_SELLOFFEND_FAIL:// 客户端元宝寄售结束 失败
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELLOFFEND_FAIL, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.Msg);
                    break;
                case Grobal2.RM_SELLOFFBUY_OK:// 购买成功
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELLOFFBUY_OK, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.Msg);
                    break;
                default:
                    result = base.Operate(ProcessMsg);
                    break;
            }
            return result;
        }

        public override void Disappear()
        {
            if (m_boReadyRun)
            {
                DisappearA();
            }
            if (Transparent && HideMode)
            {
                StatusArr[StatuStateConst.STATE_TRANSPARENT] = 0;
            }
            if (GroupOwner != null)
            {
                GroupOwner.DelMember(this);
            }
            if (MyGuild != null)
            {
                MyGuild.DelHumanObj(this);
            }
            LogonTimcCost();
            base.Disappear();
        }

        protected override void DropUseItems(BaseObject BaseObject)
        {
            const string sExceptionMsg = "[Exception] TPlayObject::DropUseItems";
            IList<DeleteItem> delList = new List<DeleteItem>();
            try
            {
                if (AngryRing || NoDropUseItem)
                {
                    return;
                }
                StdItem StdItem;
                for (var i = 0; i < UseItems.Length; i++)
                {
                    if (UseItems[i] == null)
                    {
                        continue;
                    }
                    StdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                    if (StdItem != null)
                    {
                        if ((StdItem.ItemDesc & 8) != 0)
                        {
                            delList.Add(new DeleteItem() { MakeIndex = this.UseItems[i].MakeIndex });
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.EventSource.AddEventLog(16, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + StdItem.Name + "\t" + UseItems[i].MakeIndex + "\t" + HUtil32.BoolToIntStr(Race == ActorRace.Play) + "\t" + '0');
                            }
                            UseItems[i].Index = 0;
                        }
                    }
                }
                var nRate = PvpLevel() > 2 ? M2Share.Config.DieRedDropUseItemRate : M2Share.Config.DieDropUseItemRate;
                for (var i = 0; i < UseItems.Length; i++)
                {
                    if (M2Share.RandomNumber.Random(nRate) != 0)
                    {
                        continue;
                    }
                    if (UseItems[i] != null && M2Share.InDisableTakeOffList(UseItems[i].Index))
                    {
                        continue;
                    }
                    // 检查是否在禁止取下列表,如果在列表中则不掉此物品
                    if (DropItemDown(UseItems[i], 2, true, BaseObject, this))
                    {
                        StdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                        if (StdItem != null)
                        {
                            if ((StdItem.ItemDesc & 10) == 0)
                            {
                                if (Race == ActorRace.Play)
                                {
                                    delList.Add(new DeleteItem()
                                    {
                                        ItemName = M2Share.WorldEngine.GetStdItemName(UseItems[i].Index),
                                        MakeIndex = this.UseItems[i].MakeIndex
                                    });
                                }
                                UseItems[i].Index = 0;
                            }
                        }
                    }
                }
                if (delList != null)
                {
                    var ObjectId = HUtil32.Sequence();
                    M2Share.ActorMgr.AddOhter(ObjectId, delList);
                    this.SendMsg(this, Grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
                }
            }
            catch(Exception ex)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(ex.StackTrace);
            }
        }

    }
}
