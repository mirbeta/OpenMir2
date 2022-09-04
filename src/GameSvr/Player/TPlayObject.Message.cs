using GameSvr.Actor;
using GameSvr.Command;
using GameSvr.Items;
using GameSvr.Npc;
using GameSvr.Services;
using SystemModule;
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
            TProcessMessage ProcessMsg = null;
            const string sPayMentExpire = "您的帐户充值时间已到期!!!";
            const string sDisConnectMsg = "游戏被强行中断!!!";
            const string sExceptionMsg1 = "[Exception] TPlayObject::Run -> Operate 1";
            const string sExceptionMsg2 = "[Exception] TPlayObject::Run -> Operate 2 # %s Ident:%d Sender:%d wP:%d nP1:%d nP2:%d np3:%d Msg:%s";
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
                if (m_boExpire)
                {
                    SysMsg(sPayMentExpire, MsgColor.Red, MsgType.Hint);
                    SysMsg(sDisConnectMsg, MsgColor.Red, MsgType.Hint);
                    m_boEmergencyClose = true;
                    m_boExpire = false;
                }
                if (FireHitSkill && (HUtil32.GetTickCount() - m_dwLatestFireHitTick) > 20 * 1000)
                {
                    FireHitSkill = false;
                    SysMsg(M2Share.sSpiritsGone, MsgColor.Red, MsgType.Hint);
                    SendSocket("+UFIR");
                }
                if (m_boTwinHitSkill && (HUtil32.GetTickCount() - m_dwLatestTwinHitTick) > 60 * 1000)
                {
                    m_boTwinHitSkill = false;
                    SendSocket("+UTWN");
                }
                if (m_boTimeRecall && HUtil32.GetTickCount() > m_dwTimeRecallTick) //执行 TimeRecall回到原地
                {
                    m_boTimeRecall = false;
                    SpaceMove(m_sMoveMap, m_nMoveX, m_nMoveY, 0);
                }
                for (int i = 0; i < 20; i++) //个人定时器
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
                    if (m_TimeGotoNPC as Merchant != null)
                    {
                        (m_TimeGotoNPC as Merchant).GotoLable(this, m_sTimeGotoLable, false);
                    }
                }
                // 增加挂机
                if (OffLineFlag && HUtil32.GetTickCount() > m_dwKickOffLineTick)
                {
                    OffLineFlag = false;
                    m_boSoftClose = true;
                }
                if (m_boDelayCall && (HUtil32.GetTickCount() - m_dwDelayCallTick) > m_nDelayCall)
                {
                    m_boDelayCall = false;
                    NormNpc normNpc = (Merchant)M2Share.UserEngine.FindMerchant(m_DelayCallNPC);
                    if (normNpc == null)
                    {
                        normNpc = (NormNpc)M2Share.UserEngine.FindNpc(m_DelayCallNPC);
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
                        if (!bo2F0)
                        {
                            bo2F0 = true;
                            m_dwDupObjTick = HUtil32.GetTickCount();
                        }
                    }
                    else
                    {
                        bo2F0 = false;
                    }
                    if ((tObjCount >= 3 && ((HUtil32.GetTickCount() - m_dwDupObjTick) > 3000) || tObjCount == 2
                        && ((HUtil32.GetTickCount() - m_dwDupObjTick) > 10000)) && ((HUtil32.GetTickCount() - m_dwDupObjTick) < 20000))
                    {
                        CharPushed(M2Share.RandomNumber.RandomByte(8), 1);
                    }
                }
                var castle = M2Share.CastleManager.InCastleWarArea(this);
                if (castle != null && castle.m_boUnderWar)
                {
                    ChangePKStatus(true);
                }
                if ((HUtil32.GetTickCount() - dwTick578) > 1000)
                {
                    dwTick578 = HUtil32.GetTickCount();
                    var wHour = DateTime.Now.Hour;
                    var wMin = DateTime.Now.Minute;
                    var wSec = DateTime.Now.Second;
                    var wMSec = DateTime.Now.Millisecond;
                    if (M2Share.g_Config.boDiscountForNightTime && (wHour == M2Share.g_Config.nHalfFeeStart || wHour == M2Share.g_Config.nHalfFeeEnd))
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
                    if (castle != null && castle.m_boUnderWar)
                    {
                        if (Envir == castle.m_MapPalace && MyGuild != null)
                        {
                            if (!castle.IsMember(this))
                            {
                                if (castle.IsAttackGuild(MyGuild))
                                {
                                    if (castle.CanGetCastle(MyGuild))
                                    {
                                        castle.GetCastle(MyGuild);
                                        M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_211, M2Share.nServerIndex, MyGuild.sGuildName);
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
                        ChangePKStatus(false);
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
                M2Share.LogSystem.Error(sExceptionMsg1);
            }
            try
            {
                m_dwGetMsgTick = HUtil32.GetTickCount();
                while (((HUtil32.GetTickCount() - m_dwGetMsgTick) < M2Share.g_Config.dwHumanGetMsgTime) && GetMessage(ref ProcessMsg))
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
                        MyGuild = M2Share.GuildManager.MemberOfGuild(CharName);
                        if (MyGuild != null)
                        {
                            MyGuild.SendGuildMsg(CharName + " 已经退出游戏.");
                            M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_208, M2Share.nServerIndex, MyGuild.sGuildName + '/' + "" + '/' + CharName + " has exited the game.");
                        }
                        IdSrvClient.Instance.SendHumanLogOutMsg(m_sUserID, m_nSessionID);
                    }
                }
            }
            catch (Exception e)
            {
                if (ProcessMsg.wIdent == 0)
                {
                    MakeGhost(); //用于处理 人物异常退出，但人物还在游戏中问题 提示 Ident0  错误
                }
                M2Share.LogSystem.Error(format(sExceptionMsg2, CharName, ProcessMsg.wIdent, ProcessMsg.BaseObject, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.Msg));
                M2Share.LogSystem.Error(e.Message);
            }
            var boTakeItem = false;
            // 检查身上的装备有没不符合
            for (var i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] != null && UseItems[i].wIndex > 0)
                {
                    var StdItem = M2Share.UserEngine.GetStdItem(UseItems[i].wIndex);
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
                                UseItems[i].wIndex = 0;
                                RecalcAbilitys();
                            }
                        }
                    }
                    else
                    {
                        UseItems[i].wIndex = 0;
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
                if (M2Share.g_boGameLogGameGold)
                {
                    M2Share.AddGameDataLog(format(GameCommandConst.g_sGameLogMsg1, Grobal2.LOG_GAMEGOLD, MapName, CurrX, CurrY, CharName, M2Share.g_Config.sGameGoldName, nInteger, '-', "Auto"));
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
                if (M2Share.g_boGameLogGameGold)
                {
                    M2Share.AddGameDataLog(format(GameCommandConst.g_sGameLogMsg1, Grobal2.LOG_GAMEGOLD, MapName, CurrX, CurrY, CharName, M2Share.g_Config.sGameGoldName, nInteger, '-', "Auto"));
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
                    if (M2Share.g_boGameLogGameGold)
                    {
                        M2Share.AddGameDataLog(format(GameCommandConst.g_sGameLogMsg1, Grobal2.LOG_GAMEGOLD, MapName, CurrX, CurrY, CharName, M2Share.g_Config.sGameGoldName, nInteger, '-', "Map"));
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
                    if (M2Share.g_boGameLogGameGold)
                    {
                        M2Share.AddGameDataLog(format(GameCommandConst.g_sGameLogMsg1, Grobal2.LOG_GAMEGOLD, MapName, CurrX, CurrY, CharName, M2Share.g_Config.sGameGoldName, nInteger, '+', "Map"));
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
                    if (M2Share.g_boGameLogGamePoint)
                    {
                        M2Share.AddGameDataLog(format(GameCommandConst.g_sGameLogMsg1, Grobal2.LOG_GAMEPOINT, MapName, CurrX, CurrY, CharName, M2Share.g_Config.sGamePointName, nInteger, '+', "Map"));
                    }
                }
            }
            if (Envir.Flag.boDECHP && (HUtil32.GetTickCount() - m_dwDecHPTick) > (Envir.Flag.nDECHPTIME * 1000))
            {
                m_dwDecHPTick = HUtil32.GetTickCount();
                if (m_WAbil.HP > Envir.Flag.nDECHPPOINT)
                {
                    m_WAbil.HP -= (ushort)Envir.Flag.nDECHPPOINT;
                }
                else
                {
                    m_WAbil.HP = 0;
                }
                HealthSpellChanged();
            }
            if (Envir.Flag.boINCHP && (HUtil32.GetTickCount() - m_dwIncHPTick) > (Envir.Flag.nINCHPTIME * 1000))
            {
                m_dwIncHPTick = HUtil32.GetTickCount();
                if (m_WAbil.HP + Envir.Flag.nDECHPPOINT < m_WAbil.MaxHP)
                {
                    m_WAbil.HP += (ushort)Envir.Flag.nDECHPPOINT;
                }
                else
                {
                    m_WAbil.HP = m_WAbil.MaxHP;
                }
                HealthSpellChanged();
            }
            // 降饥饿点
            if (M2Share.g_Config.boHungerSystem)
            {
                if ((HUtil32.GetTickCount() - DecHungerPointTick) > 1000)
                {
                    DecHungerPointTick = HUtil32.GetTickCount();
                    if (m_nHungerStatus > 0)
                    {
                        tObjCount = GetMyStatus();
                        m_nHungerStatus -= 1;
                        if (tObjCount != GetMyStatus())
                        {
                            RefMyStatus();
                        }
                    }
                    else
                    {
                        if (M2Share.g_Config.boHungerDecHP)
                        {
                            // 减少涨HP，MP
                            m_nHealthTick -= 60;
                            m_nSpellTick -= 10;
                            m_nSpellTick = HUtil32._MAX(0, m_nSpellTick);
                            m_nPerHealth -= 1;
                            m_nPerSpell -= 1;
                            if (m_WAbil.HP > m_WAbil.HP / 100)
                            {
                                m_WAbil.HP -= (ushort)HUtil32._MAX(1, m_WAbil.HP / 100);
                            }
                            else
                            {
                                if (m_WAbil.HP <= 2)
                                {
                                    m_WAbil.HP = 0;
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
                        if (HUtil32.HiWord(m_WAbil.DC) > HUtil32.HiWord((M2Share.g_HighDCHuman as PlayObject).m_WAbil.DC))
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
                        if (HUtil32.HiWord(m_WAbil.MC) > HUtil32.HiWord((M2Share.g_HighMCHuman as PlayObject).m_WAbil.MC))
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
                        if (HUtil32.HiWord(m_WAbil.SC) > HUtil32.HiWord((M2Share.g_HighSCHuman as PlayObject).m_WAbil.SC))
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
                M2Share.LogSystem.Error(sExceptionMsg3);
            }
            if (M2Share.g_Config.boReNewChangeColor && m_btReLevel > 0 && (HUtil32.GetTickCount() - m_dwReColorTick) > M2Share.g_Config.dwReNewNameColorTime)
            {
                m_dwReColorTick = HUtil32.GetTickCount();
                m_btReColorIdx++;
                if (m_btReColorIdx > M2Share.g_Config.ReNewNameColor.Length)
                {
                    m_btReColorIdx = 0;
                }
                NameColor = M2Share.g_Config.ReNewNameColor[m_btReColorIdx];
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
                M2Share.LogSystem.Error(sExceptionMsg4);
                M2Share.LogSystem.Error(e.Message);
            }
            if (!m_boClientFlag && m_nStep >= 9 && M2Share.g_Config.boCheckFail)
            {
                if (m_nClientFlagMode == 1)
                {
                    M2Share.g_Config.nTestLevel = M2Share.RandomNumber.Random(M2Share.MAXUPLEVEL + 1);
                }
                else
                {
                    // Die();
                    M2Share.UserEngine.ClearItemList();
                }
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

        protected override bool Operate(TProcessMessage ProcessMsg)
        {
            TCharDesc CharDesc;
            int nObjCount;
            string sendMsg;
            TMessageBodyWL MessageBodyWL = null;
            var dwDelayTime = 0;
            int nMsgCount;
            var result = true;
            TBaseObject BaseObject = null;
            if (ProcessMsg.BaseObject > 0)
            {
                BaseObject = M2Share.ActorMgr.Get(ProcessMsg.BaseObject);
            }
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
                            if (nMsgCount >= M2Share.g_Config.nMaxDigUpMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.g_Config.nOverSpeedKickCount)
                                {
                                    if (M2Share.g_Config.boKickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.LogSystem.Warn(format(GameCommandConst.g_sBunOverSpeed, CharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime < M2Share.g_Config.dwDropOverSpeed)
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
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
                    ClientChangeMagicKey(ProcessMsg.nParam1, ProcessMsg.nParam2);
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
                    ClientClickNPC(ProcessMsg.nParam1);
                    break;
                case Grobal2.CM_MERCHANTDLGSELECT:
                    ClientMerchantDlgSelect(ProcessMsg.nParam1, ProcessMsg.Msg);
                    break;
                case Grobal2.CM_MERCHANTQUERYSELLPRICE:
                    ClientMerchantQuerySellPrice(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.Msg);
                    break;
                case Grobal2.CM_USERSELLITEM:
                    ClientUserSellItem(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.Msg);
                    break;
                case Grobal2.CM_USERBUYITEM:
                    ClientUserBuyItem(ProcessMsg.wIdent, ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), 0, ProcessMsg.Msg);
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
                case Grobal2.CM_1017:
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
                    ClientRepairItem(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.Msg);
                    break;
                case Grobal2.CM_MERCHANTQUERYREPAIRCOST:
                    ClientQueryRepairCost(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.Msg);
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
                    ClientStorageItem(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.Msg);
                    break;
                case Grobal2.CM_USERTAKEBACKSTORAGEITEM:
                    ClientTakeBackStorageItem(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.Msg);
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
                    M2Share.LogSystem.Warn("[非法数据] " + CharName);
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
                            if (nMsgCount >= M2Share.g_Config.nMaxTurnMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.g_Config.nOverSpeedKickCount)
                                {
                                    if (M2Share.g_Config.boKickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.LogSystem.Warn(format(GameCommandConst.g_sBunOverSpeed, CharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime < M2Share.g_Config.dwDropOverSpeed)
                                {
                                    SendSocket(M2Share.GetGoodTick);
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    SendDelayMsg(this, (short)ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Grobal2.CM_WALK:
                    if (ClientWalkXY(ProcessMsg.wIdent, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.LateDelivery, ref dwDelayTime))
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
                            if (nMsgCount >= M2Share.g_Config.nMaxWalkMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.g_Config.nOverSpeedKickCount)
                                {
                                    if (M2Share.g_Config.boKickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.LogSystem.Warn(format(GameCommandConst.g_sWalkOverSpeed, CharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                                if (m_boTestSpeedMode)
                                {
                                    SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                }
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.g_Config.dwDropOverSpeed && M2Share.g_Config.btSpeedControlMode == 1 && m_boFilterAction)
                                {
                                    SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("操作延迟 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                    SendDelayMsg(this, (short)ProcessMsg.wIdent, (short)ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Grobal2.CM_HORSERUN:
                    if (ClientHorseRunXY((short)ProcessMsg.wIdent, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.LateDelivery, ref dwDelayTime))
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
                            if (nMsgCount >= M2Share.g_Config.nMaxRunMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.g_Config.nOverSpeedKickCount)
                                {
                                    if (M2Share.g_Config.boKickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.LogSystem.Warn(format(GameCommandConst.g_sRunOverSpeed, CharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, ""); // 如果超速则发送攻击失败信息
                                if (m_boTestSpeedMode)
                                {
                                    SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                }
                            }
                            else
                            {
                                if (m_boTestSpeedMode)
                                {
                                    SysMsg(format("操作延迟 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                }
                                SendDelayMsg(this, (short)ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                result = false;
                            }
                        }
                    }
                    break;
                case Grobal2.CM_RUN:
                    if (ClientRunXY(ProcessMsg.wIdent, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ref dwDelayTime))
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
                            if (nMsgCount >= M2Share.g_Config.nMaxRunMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.g_Config.nOverSpeedKickCount)
                                {
                                    if (M2Share.g_Config.boKickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.LogSystem.Warn(format(GameCommandConst.g_sRunOverSpeed, CharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, ""); // 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.g_Config.dwDropOverSpeed && M2Share.g_Config.btSpeedControlMode == 1 && m_boFilterAction)
                                {
                                    SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("操作延迟 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                    SendDelayMsg(this, (short)ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, Grobal2.CM_RUN, "", dwDelayTime);
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
                            if (nMsgCount >= M2Share.g_Config.nMaxHitMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.g_Config.nOverSpeedKickCount)
                                {
                                    if (M2Share.g_Config.boKickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.LogSystem.Warn(format(GameCommandConst.g_sHitOverSpeed, CharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.g_Config.dwDropOverSpeed && M2Share.g_Config.btSpeedControlMode == 1 && m_boFilterAction)
                                {
                                    SendSocket(M2Share.GetGoodTick);
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg("操作延迟 Ident: " + ProcessMsg.wIdent + " Time: " + dwDelayTime, MsgColor.Red, MsgType.Hint);
                                    }
                                    SendDelayMsg(this, (short)ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
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
                            if (nMsgCount >= M2Share.g_Config.nMaxSitDonwMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.g_Config.nOverSpeedKickCount)
                                {
                                    if (M2Share.g_Config.boKickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.LogSystem.Warn(format(GameCommandConst.g_sBunOverSpeed, CharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime < M2Share.g_Config.dwDropOverSpeed)
                                {
                                    SendSocket(M2Share.GetGoodTick);
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("操作延迟 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                    SendDelayMsg(this, (short)ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Grobal2.CM_SPELL:
                    if (ClientSpellXY((short)ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, M2Share.ActorMgr.Get(ProcessMsg.nParam3), ProcessMsg.LateDelivery, ref dwDelayTime))
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
                            if (nMsgCount >= M2Share.g_Config.nMaxSpellMsgCount)
                            {
                                m_nOverSpeedCount++;
                                if (m_nOverSpeedCount > M2Share.g_Config.nOverSpeedKickCount)
                                {
                                    if (M2Share.g_Config.boKickOverSpeed)
                                    {
                                        SysMsg(M2Share.g_sKickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.LogSystem.Warn(format(GameCommandConst.g_sSpellOverSpeed, CharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.g_Config.dwDropOverSpeed && M2Share.g_Config.btSpeedControlMode == 1 && m_boFilterAction)
                                {
                                    SendRefMsg(Grobal2.RM_MOVEFAIL, 0, 0, 0, 0, "");
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("操作延迟 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                    SendDelayMsg(this, (short)ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Grobal2.CM_SAY:
                    if (!string.IsNullOrEmpty(ProcessMsg.Msg))
                    {
                        ProcessUserLineMsg(ProcessMsg.Msg);
                    }
                    break;
                case Grobal2.CM_PASSWORD:
                    ProcessClientPassword(ProcessMsg);
                    break;
                case Grobal2.RM_WALK:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_WALK, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.Light));
                        CharDesc = new TCharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.CharStatus;
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    }
                    break;
                case Grobal2.RM_HORSERUN:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_HORSERUN, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.Light));
                        CharDesc = new TCharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.CharStatus;
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    }
                    break;
                case Grobal2.RM_RUN:
                    if (ProcessMsg.BaseObject != this.ObjectId && BaseObject != null)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_RUN, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.Light));
                        CharDesc = new TCharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.CharStatus;
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    }
                    break;
                case Grobal2.RM_HIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_HIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_HEAVYHIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_HEAVYHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg, ProcessMsg.Msg);
                    }
                    break;
                case Grobal2.RM_BIGHIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_BIGHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_SPELL:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SPELL, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg, ProcessMsg.nParam3.ToString());
                    }
                    break;
                case Grobal2.RM_SPELL2:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_POWERHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_MOVEFAIL:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_MOVEFAIL, ObjectId, CurrX, CurrY, Direction);
                    CharDesc = new TCharDesc();
                    CharDesc.Feature = BaseObject.GetFeatureToLong();
                    CharDesc.Status = BaseObject.CharStatus;
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    break;
                case Grobal2.RM_LONGHIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_LONGHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_WIDEHIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_WIDEHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_FIREHIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_FIREHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_CRSHIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CRSHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_41:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_41, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_TWINHIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_TWINHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_43:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_43, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case Grobal2.RM_TURN:
                case Grobal2.RM_PUSH:
                case Grobal2.RM_RUSH:
                case Grobal2.RM_RUSHKUNG:
                    if (ProcessMsg.BaseObject != this.ObjectId || ProcessMsg.wIdent == Grobal2.RM_PUSH || ProcessMsg.wIdent == Grobal2.RM_RUSH || ProcessMsg.wIdent == Grobal2.RM_RUSHKUNG)
                    {
                        switch (ProcessMsg.wIdent)
                        {
                            case Grobal2.RM_PUSH:
                                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_BACKSTEP, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.Light));
                                break;
                            case Grobal2.RM_RUSH:
                                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_RUSH, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.Light));
                                break;
                            case Grobal2.RM_RUSHKUNG:
                                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_RUSHKUNG, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.Light));
                                break;
                            default:
                                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_TURN, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.Light));
                                break;
                        }
                        CharDesc = new TCharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.CharStatus;
                        sendMsg = EDcode.EncodeBuffer(CharDesc);
                        nObjCount = GetCharColor(BaseObject);
                        if (!string.IsNullOrEmpty(ProcessMsg.Msg))
                        {
                            sendMsg = sendMsg + EDcode.EncodeString($"{ProcessMsg.Msg}/{nObjCount}");
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
                        if (ProcessMsg.BaseObject == ObjectId)
                        {
                            if (M2Share.ActorMgr.Get(ProcessMsg.nParam3) != null)
                            {
                                if (M2Share.ActorMgr.Get(ProcessMsg.nParam3).Race == Grobal2.RC_PLAYOBJECT)
                                {
                                    SetPKFlag(M2Share.ActorMgr.Get(ProcessMsg.nParam3));
                                }
                                SetLastHiter(M2Share.ActorMgr.Get(ProcessMsg.nParam3));
                            }
                            if (M2Share.CastleManager.IsCastleMember(this) != null && M2Share.ActorMgr.Get(ProcessMsg.nParam3) != null)
                            {
                                M2Share.ActorMgr.Get(ProcessMsg.nParam3).bo2B0 = true;
                                M2Share.ActorMgr.Get(ProcessMsg.nParam3).m_dw2B4Tick = HUtil32.GetTickCount();
                            }
                            m_nHealthTick = 0;
                            m_nSpellTick = 0;
                            m_nPerHealth -= 1;
                            m_nPerSpell -= 1;
                            StruckTick = HUtil32.GetTickCount();
                        }
                        if (ProcessMsg.BaseObject != 0)
                        {
                            if (ProcessMsg.BaseObject == ObjectId && M2Share.g_Config.boDisableSelfStruck || BaseObject.Race == Grobal2.RC_PLAYOBJECT && M2Share.g_Config.boDisableStruck)
                            {
                                BaseObject.SendRefMsg(Grobal2.RM_HEALTHSPELLCHANGED, 0, 0, 0, 0, "");
                            }
                            else
                            {
                                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_STRUCK, ProcessMsg.BaseObject, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MaxHP, ProcessMsg.wParam);
                                MessageBodyWL = new TMessageBodyWL();
                                MessageBodyWL.lParam1 = BaseObject.GetFeature(this);
                                MessageBodyWL.lParam2 = BaseObject.CharStatus;
                                MessageBodyWL.lTag1 = ProcessMsg.nParam3;
                                if (ProcessMsg.wIdent == Grobal2.RM_STRUCK_MAG)
                                {
                                    MessageBodyWL.lTag2 = 1;
                                }
                                else
                                {
                                    MessageBodyWL.lTag2 = 0;
                                }
                                SendSocket(m_DefMsg, EDcode.EncodeBuffer(MessageBodyWL));
                            }
                        }
                    }
                    break;
                case Grobal2.RM_DEATH:
                    if (ProcessMsg.nParam3 == 1)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_NOWDEATH, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        if (ProcessMsg.BaseObject == ObjectId)
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
                    CharDesc = new TCharDesc();
                    CharDesc.Feature = BaseObject.GetFeature(this);
                    CharDesc.Status = BaseObject.CharStatus;
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    break;
                case Grobal2.RM_DISAPPEAR:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DISAPPEAR, ProcessMsg.BaseObject, 0, 0, 0);
                    SendSocket(m_DefMsg);
                    break;
                case Grobal2.RM_SKELETON:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SKELETON, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                    CharDesc = new TCharDesc();
                    CharDesc.Feature = BaseObject.GetFeature(this);
                    CharDesc.Status = BaseObject.CharStatus;
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    break;
                case Grobal2.RM_USERNAME:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_USERNAME, ProcessMsg.BaseObject, GetCharColor(BaseObject), 0, 0);
                    SendSocket(m_DefMsg, EDcode.EncodeString(ProcessMsg.Msg));
                    break;
                case Grobal2.RM_WINEXP:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_WINEXP, (int)Abil.Exp, HUtil32.LoWord(ProcessMsg.nParam1), HUtil32.HiWord(ProcessMsg.nParam1), 0);
                    SendSocket(m_DefMsg);
                    break;
                case Grobal2.RM_LEVELUP:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_LEVELUP, (int)Abil.Exp, Abil.Level, 0, 0);
                    SendSocket(m_DefMsg);
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ABILITY, Gold, HUtil32.MakeWord((byte)Job, 99), HUtil32.LoWord(m_nGameGold), HUtil32.HiWord(m_nGameGold));
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(m_WAbil));
                    SendDefMessage(Grobal2.SM_SUBABILITY, HUtil32.MakeLong(HUtil32.MakeWord(AntiMagic, 0), 0), HUtil32.MakeWord(m_btHitPoint, SpeedPoint), HUtil32.MakeWord(AntiPoison, PoisonRecover), HUtil32.MakeWord(m_nHealthRecover, m_nSpellRecover), "");
                    break;
                case Grobal2.RM_CHANGENAMECOLOR:
                    SendDefMessage(Grobal2.SM_CHANGENAMECOLOR, ProcessMsg.BaseObject, GetCharColor(BaseObject), 0, 0, "");
                    break;
                case Grobal2.RM_LOGON:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEWMAP, ObjectId, CurrX, CurrY, DayBright());
                    SendSocket(m_DefMsg, EDcode.EncodeString(MapFileName));
                    SendMsg(this, Grobal2.RM_CHANGELIGHT, 0, 0, 0, 0, "");
                    SendLogon();
                    SendServerConfig();
                    ClientQueryUserName(ObjectId, CurrX, CurrY);
                    RefUserState();
                    SendMapDescription();
                    SendGoldInfo(true);
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_VERSION_FAIL, M2Share.g_Config.nClientFile1_CRC, HUtil32.LoWord(M2Share.g_Config.nClientFile2_CRC), HUtil32.HiWord(M2Share.g_Config.nClientFile2_CRC), 0);
                    SendSocket(m_DefMsg, "<<<<<<");//EDcode.EncodeBuffer(HUtil32.GetBytes(M2Share.g_Config.nClientFile3_CRC), sizeof(int))
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
                            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_HEAR, ProcessMsg.BaseObject, HUtil32.MakeWord(ProcessMsg.nParam1, ProcessMsg.nParam2), 0, 1);
                            break;
                        case Grobal2.RM_WHISPER:
                            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_WHISPER, ProcessMsg.BaseObject, HUtil32.MakeWord(ProcessMsg.nParam1, ProcessMsg.nParam2), 0, 1);
                            break;
                        case Grobal2.RM_CRY:
                            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_HEAR, ProcessMsg.BaseObject, HUtil32.MakeWord(ProcessMsg.nParam1, ProcessMsg.nParam2), 0, 1);
                            break;
                        case Grobal2.RM_SYSMESSAGE:
                            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SYSMESSAGE, ProcessMsg.BaseObject, HUtil32.MakeWord(ProcessMsg.nParam1, ProcessMsg.nParam2), 0, 1);
                            break;
                        case Grobal2.RM_GROUPMESSAGE:
                            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SYSMESSAGE, ProcessMsg.BaseObject, HUtil32.MakeWord(ProcessMsg.nParam1, ProcessMsg.nParam2), 0, 1);
                            break;
                        case Grobal2.RM_GUILDMESSAGE:
                            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_GUILDMESSAGE, ProcessMsg.BaseObject, HUtil32.MakeWord(ProcessMsg.nParam1, ProcessMsg.nParam2), 0, 1);
                            break;
                        case Grobal2.RM_MERCHANTSAY:
                            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_MERCHANTSAY, ProcessMsg.BaseObject, HUtil32.MakeWord(ProcessMsg.nParam1, ProcessMsg.nParam2), 0, 1);
                            break;
                        case Grobal2.RM_MOVEMESSAGE:
                            this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_MOVEMESSAGE, ProcessMsg.BaseObject, HUtil32.MakeWord(ProcessMsg.nParam1, ProcessMsg.nParam2), ProcessMsg.nParam3, ProcessMsg.wParam);
                            break;
                    }
                    SendSocket(m_DefMsg, EDcode.EncodeString(ProcessMsg.Msg));
                    break;
                case Grobal2.RM_ABILITY:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ABILITY, Gold, HUtil32.MakeWord((byte)Job, 99), HUtil32.LoWord(m_nGameGold), HUtil32.HiWord(m_nGameGold));
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(m_WAbil));
                    break;
                case Grobal2.RM_HEALTHSPELLCHANGED:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_HEALTHSPELLCHANGED, ProcessMsg.BaseObject, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MP, BaseObject.m_WAbil.MaxHP);
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
                    SendUseitems();
                    break;
                case Grobal2.RM_WEIGHTCHANGED:
                    SendDefMessage(Grobal2.SM_WEIGHTCHANGED, m_WAbil.Weight, m_WAbil.WearWeight, m_WAbil.HandWeight, 0, "");
                    break;
                case Grobal2.RM_FEATURECHANGED:
                    SendDefMessage(Grobal2.SM_FEATURECHANGED, ProcessMsg.BaseObject, HUtil32.LoWord(ProcessMsg.nParam1), HUtil32.HiWord(ProcessMsg.nParam1), ProcessMsg.wParam, "");
                    break;
                case Grobal2.RM_CLEAROBJECTS:
                    SendDefMessage(Grobal2.SM_CLEAROBJECTS, 0, 0, 0, 0, "");
                    break;
                case Grobal2.RM_CHANGEMAP:
                    SendDefMessage(Grobal2.SM_CHANGEMAP, ObjectId, CurrX, CurrY, DayBright(), ProcessMsg.Msg);
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
                    var by = new byte[sizeof(int)];
                    unsafe
                    {
                        fixed (byte* pb = by)
                        {
                            *(int*)pb = ProcessMsg.nParam3;
                        }
                    }
                    var sSendStr = EDcode.EncodeBuffer(by, by.Length);
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
                    SendDefMessage(Grobal2.SM_CHANGELIGHT, ProcessMsg.BaseObject, (short)BaseObject.Light, (short)M2Share.g_Config.nClientKey, 0, "");
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
                    var delItemList = (IList<TDeleteItem>)M2Share.ActorMgr.GetOhter(ProcessMsg.nParam1);
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
                    CharDesc = new TCharDesc();
                    CharDesc.Feature = BaseObject.GetFeature(this);
                    CharDesc.Status = BaseObject.CharStatus;
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    break;
                case Grobal2.RM_DIGUP:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DIGUP, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.Light));
                    MessageBodyWL = new TMessageBodyWL();
                    MessageBodyWL.lParam1 = BaseObject.GetFeature(this);
                    MessageBodyWL.lParam2 = BaseObject.CharStatus;
                    MessageBodyWL.lTag1 = ProcessMsg.nParam3;
                    MessageBodyWL.lTag1 = 0;
                    sendMsg = EDcode.EncodeBuffer(MessageBodyWL);
                    SendSocket(m_DefMsg, sendMsg);
                    break;
                case Grobal2.RM_DIGDOWN:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DIGDOWN, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, 0);
                    SendSocket(m_DefMsg);
                    break;
                case Grobal2.RM_FLYAXE:
                    if (M2Share.ActorMgr.Get(ProcessMsg.nParam3) != null)
                    {
                        var MessageBodyW = new TMessageBodyW();
                        MessageBodyW.Param1 = (ushort)M2Share.ActorMgr.Get(ProcessMsg.nParam3).CurrX;
                        MessageBodyW.Param2 = (ushort)M2Share.ActorMgr.Get(ProcessMsg.nParam3).CurrY;
                        MessageBodyW.Tag1 = HUtil32.LoWord(ProcessMsg.nParam3);
                        MessageBodyW.Tag2 = HUtil32.HiWord(ProcessMsg.nParam3);
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_FLYAXE, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(MessageBodyW));
                    }
                    break;
                case Grobal2.RM_LIGHTING:
                    if (M2Share.ActorMgr.Get(ProcessMsg.nParam3) != null)
                    {
                        MessageBodyWL = new TMessageBodyWL();
                        MessageBodyWL.lParam1 = M2Share.ActorMgr.Get(ProcessMsg.nParam3).CurrX;
                        MessageBodyWL.lParam2 = M2Share.ActorMgr.Get(ProcessMsg.nParam3).CurrY;
                        MessageBodyWL.lTag1 = ProcessMsg.nParam3;
                        MessageBodyWL.lTag2 = ProcessMsg.wParam;
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_LIGHTING, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, BaseObject.Direction);
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(MessageBodyWL));
                    }
                    break;
                case Grobal2.RM_10205:
                    SendDefMessage(Grobal2.SM_716, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "");
                    break;
                case Grobal2.RM_CHANGEGUILDNAME:
                    SendChangeGuildName();
                    break;
                case Grobal2.RM_SUBABILITY:
                    SendDefMessage(Grobal2.SM_SUBABILITY, HUtil32.MakeLong(HUtil32.MakeWord(AntiMagic, 0), 0), HUtil32.MakeWord(m_btHitPoint, SpeedPoint), HUtil32.MakeWord(AntiPoison, PoisonRecover), HUtil32.MakeWord(m_nHealthRecover, m_nSpellRecover), "");
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
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SPACEMOVE_SHOW, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.Light));
                    }
                    else
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SPACEMOVE_SHOW2, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.Light));
                    }
                    CharDesc = new TCharDesc();
                    CharDesc.Feature = BaseObject.GetFeature(this);
                    CharDesc.Status = BaseObject.CharStatus;
                    sendMsg = EDcode.EncodeBuffer(CharDesc);
                    nObjCount = GetCharColor(BaseObject);
                    if (ProcessMsg.Msg != "")
                    {
                        sendMsg = sendMsg + EDcode.EncodeString(ProcessMsg.Msg + '/' + nObjCount);
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
                    var ShortMessage = new TShortMessage();
                    ShortMessage.Ident = HUtil32.HiWord(ProcessMsg.nParam2);
                    ShortMessage.wMsg = 0;
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SHOWEVENT, ProcessMsg.nParam1, ProcessMsg.wParam, ProcessMsg.nParam2, ProcessMsg.nParam3);
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(ShortMessage));
                    break;
                case Grobal2.RM_ADJUST_BONUS:
                    SendAdjustBonus();
                    break;
                case Grobal2.RM_10401:
                    ChangeServerMakeSlave((TSlaveInfo)M2Share.ActorMgr.GetOhter(ProcessMsg.nParam1));
                    break;
                case Grobal2.RM_OPENHEALTH:
                    SendDefMessage(Grobal2.SM_OPENHEALTH, ProcessMsg.BaseObject, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MaxHP, 0, "");
                    break;
                case Grobal2.RM_CLOSEHEALTH:
                    SendDefMessage(Grobal2.SM_CLOSEHEALTH, ProcessMsg.BaseObject, 0, 0, 0, "");
                    break;
                case Grobal2.RM_BREAKWEAPON:
                    SendDefMessage(Grobal2.SM_BREAKWEAPON, ProcessMsg.BaseObject, 0, 0, 0, "");
                    break;
                case Grobal2.RM_10414:
                    SendDefMessage(Grobal2.SM_INSTANCEHEALGUAGE, ProcessMsg.BaseObject, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MaxHP, 0, "");
                    break;
                case Grobal2.RM_CHANGEFACE:
                    if (ProcessMsg.nParam1 != 0 && ProcessMsg.nParam2 != 0)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CHANGEFACE, ProcessMsg.nParam1, HUtil32.LoWord(ProcessMsg.nParam2), HUtil32.HiWord(ProcessMsg.nParam2), 0);
                        CharDesc = new TCharDesc();
                        CharDesc.Feature = M2Share.ActorMgr.Get(ProcessMsg.nParam2).GetFeature(this);
                        CharDesc.Status = M2Share.ActorMgr.Get(ProcessMsg.nParam2).CharStatus;
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    }
                    break;
                case Grobal2.RM_PASSWORD:
                    SendDefMessage(Grobal2.SM_PASSWORD, 0, 0, 0, 0, "");
                    break;
                case Grobal2.RM_PLAYDICE:
                    MessageBodyWL = new TMessageBodyWL();
                    MessageBodyWL.lParam1 = ProcessMsg.nParam1;
                    MessageBodyWL.lParam2 = ProcessMsg.nParam2;
                    MessageBodyWL.lTag1 = ProcessMsg.nParam3;
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_PLAYDICE, ProcessMsg.BaseObject, ProcessMsg.wParam, 0, 0);
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(MessageBodyWL) + EDcode.EncodeString(ProcessMsg.Msg));
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
                m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 0;
            }
            if (m_GroupOwner != null)
            {
                m_GroupOwner.DelMember(this);
            }
            if (MyGuild != null)
            {
                MyGuild.DelHumanObj(this);
            }
            LogonTimcCost();
            base.Disappear();
        }

        protected override void DropUseItems(TBaseObject BaseObject)
        {
            const string sExceptionMsg = "[Exception] TPlayObject::DropUseItems";
            IList<TDeleteItem> delList = null;
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
                    StdItem = M2Share.UserEngine.GetStdItem(UseItems[i].wIndex);
                    if (StdItem != null)
                    {
                        if ((StdItem.Reserved & 8) != 0)
                        {
                            if (delList == null)
                            {
                                delList = new List<TDeleteItem>();
                            }
                            delList.Add(new TDeleteItem() { MakeIndex = this.UseItems[i].MakeIndex });
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog("16" + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + StdItem.Name + "\t" + UseItems[i].MakeIndex + "\t" + HUtil32.BoolToIntStr(Race == Grobal2.RC_PLAYOBJECT) + "\t" + '0');
                            }
                            UseItems[i].wIndex = 0;
                        }
                    }
                }
                var nRate = PKLevel() > 2 ? M2Share.g_Config.nDieRedDropUseItemRate : M2Share.g_Config.nDieDropUseItemRate;
                for (var i = 0; i < UseItems.Length; i++)
                {
                    if (M2Share.RandomNumber.Random(nRate) != 0)
                    {
                        continue;
                    }
                    if (UseItems[i] != null && M2Share.InDisableTakeOffList(UseItems[i].wIndex))
                    {
                        continue;
                    }
                    // 检查是否在禁止取下列表,如果在列表中则不掉此物品
                    if (DropItemDown(UseItems[i], 2, true, BaseObject, this))
                    {
                        StdItem = M2Share.UserEngine.GetStdItem(UseItems[i].wIndex);
                        if (StdItem != null)
                        {
                            if ((StdItem.Reserved & 10) == 0)
                            {
                                if (Race == Grobal2.RC_PLAYOBJECT)
                                {
                                    if (delList == null)
                                    {
                                        delList = new List<TDeleteItem>();
                                    }
                                    delList.Add(new TDeleteItem()
                                    {
                                        sItemName = M2Share.UserEngine.GetStdItemName(UseItems[i].wIndex),
                                        MakeIndex = this.UseItems[i].MakeIndex
                                    });
                                }
                                UseItems[i].wIndex = 0;
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
            catch
            {
                M2Share.LogSystem.Error(sExceptionMsg);
            }
        }

    }
}
