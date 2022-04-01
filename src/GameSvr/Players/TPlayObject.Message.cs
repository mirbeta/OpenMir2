using System;
using System.Collections.Generic;
using SystemModule;

namespace GameSvr
{
    public partial class TPlayObject
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
                if (m_boDealing)
                {
                    if (GetPoseCreate() != m_DealCreat || m_DealCreat == this || m_DealCreat == null)
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
                if (m_boFireHitSkill && (HUtil32.GetTickCount() - m_dwLatestFireHitTick) > 20 * 1000)
                {
                    m_boFireHitSkill = false;
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
                if (m_boOffLineFlag && HUtil32.GetTickCount() > m_dwKickOffLineTick)
                {
                    m_boOffLineFlag = false;
                    m_boSoftClose = true;
                }
                if (m_boDelayCall && (HUtil32.GetTickCount() - m_dwDelayCallTick) > m_nDelayCall)
                {
                    m_boDelayCall = false;
                    NormNpc normNpc = (Merchant)M2Share.UserEngine.FindMerchant(m_DelayCallNPC);
                    if (normNpc == null)
                    {
                        normNpc = (NormNpc)M2Share.UserEngine.FindNPC(m_DelayCallNPC);
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
                    tObjCount = m_PEnvir.GetXYObjCount(m_nCurrX, m_nCurrY);
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
                        CharPushed((byte)M2Share.RandomNumber.Random(8), 1);
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
                    if (m_MyGuild != null)
                    {
                        if (m_MyGuild.GuildWarList.Count > 0)
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
                        if (m_PEnvir == castle.m_MapPalace && m_MyGuild != null)
                        {
                            if (!castle.IsMember(this))
                            {
                                if (castle.IsAttackGuild(m_MyGuild))
                                {
                                    if (castle.CanGetCastle(m_MyGuild))
                                    {
                                        castle.GetCastle(m_MyGuild);
                                        M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_211, M2Share.nServerIndex, m_MyGuild.sGuildName);
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
                    if (m_boNameColorChanged)
                    {
                        m_boNameColorChanged = false;
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
                M2Share.MainOutMessage(sExceptionMsg1);
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
                        m_sMapName = m_sSwitchMapName;
                        m_nCurrX = m_nSwitchMapX;
                        m_nCurrY = m_nSwitchMapY;
                    }
                    MakeGhost();
                    if (m_boKickFlag)
                    {
                        SendDefMessage(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0, "");
                    }
                    if (!m_boReconnection && m_boSoftClose)
                    {
                        m_MyGuild = M2Share.GuildManager.MemberOfGuild(m_sCharName);
                        if (m_MyGuild != null)
                        {
                            m_MyGuild.SendGuildMsg(m_sCharName + " 已经退出游戏.");
                            M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_208, M2Share.nServerIndex, m_MyGuild.sGuildName + '/' + "" + '/' + m_sCharName + " has exited the game.");
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
                M2Share.ErrorMessage(format(sExceptionMsg2, m_sCharName, ProcessMsg.wIdent, ProcessMsg.BaseObject, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.sMsg));
                M2Share.ErrorMessage(e.Message);
            }
            var boTakeItem = false;
            // 检查身上的装备有没不符合
            for (var i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
            {
                if (m_UseItems[i] != null && m_UseItems[i].wIndex > 0)
                {
                    var StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[i].wIndex);
                    if (StdItem != null)
                    {
                        if (!CheckItemsNeed(StdItem))
                        {
                            // m_ItemList.Add((UserItem));
                            var UserItem = m_UseItems[i];
                            if (AddItemToBag(UserItem))
                            {
                                SendAddItem(UserItem);
                                WeightChanged();
                                boTakeItem = true;
                            }
                            else
                            {
                                if (DropItemDown(m_UseItems[i], 1, false, null, this))
                                {
                                    boTakeItem = true;
                                }
                            }
                            if (boTakeItem)
                            {
                                SendDelItems(m_UseItems[i]);
                                m_UseItems[i].wIndex = 0;
                                RecalcAbilitys();
                            }
                        }
                    }
                    else
                    {
                        m_UseItems[i].wIndex = 0;
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
                    M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, Grobal2.LOG_GAMEGOLD, m_sMapName, m_nCurrX, m_nCurrY, m_sCharName, M2Share.g_Config.sGameGoldName, nInteger, '-', "Auto"));
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
                    M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, Grobal2.LOG_GAMEGOLD, m_sMapName, m_nCurrX, m_nCurrY, m_sCharName, M2Share.g_Config.sGameGoldName, nInteger, '-', "Auto"));
                }
            }
            if (!m_boDecGameGold && m_PEnvir.Flag.boDECGAMEGOLD)
            {
                if ((HUtil32.GetTickCount() - m_dwDecGameGoldTick) > m_PEnvir.Flag.nDECGAMEGOLDTIME * 1000)
                {
                    m_dwDecGameGoldTick = HUtil32.GetTickCount();
                    if (m_nGameGold >= m_PEnvir.Flag.nDECGAMEGOLD)
                    {
                        m_nGameGold -= m_PEnvir.Flag.nDECGAMEGOLD;
                        nInteger = m_PEnvir.Flag.nDECGAMEGOLD;
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
                        M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, Grobal2.LOG_GAMEGOLD, m_sMapName, m_nCurrX, m_nCurrY, m_sCharName, M2Share.g_Config.sGameGoldName, nInteger, '-', "Map"));
                    }
                }
            }
            if (!m_boIncGameGold && m_PEnvir.Flag.boINCGAMEGOLD)
            {
                if ((HUtil32.GetTickCount() - m_dwIncGameGoldTick) > (m_PEnvir.Flag.nINCGAMEGOLDTIME * 1000))
                {
                    m_dwIncGameGoldTick = HUtil32.GetTickCount();
                    if (m_nGameGold + m_PEnvir.Flag.nINCGAMEGOLD <= 2000000)
                    {
                        m_nGameGold += m_PEnvir.Flag.nINCGAMEGOLD;
                        nInteger = m_PEnvir.Flag.nINCGAMEGOLD;
                    }
                    else
                    {
                        nInteger = 2000000 - m_nGameGold;
                        m_nGameGold = 2000000;
                    }
                    if (M2Share.g_boGameLogGameGold)
                    {
                        M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, Grobal2.LOG_GAMEGOLD, m_sMapName, m_nCurrX, m_nCurrY, m_sCharName, M2Share.g_Config.sGameGoldName, nInteger, '+', "Map"));
                    }
                }
            }
            if (tObjCount != m_nGameGold)
            {
                SendUpdateMsg(this, Grobal2.RM_GOLDCHANGED, 0, 0, 0, 0, "");
            }
            if (m_PEnvir.Flag.boINCGAMEPOINT)
            {
                if ((HUtil32.GetTickCount() - m_dwIncGamePointTick) > (m_PEnvir.Flag.nINCGAMEPOINTTIME * 1000))
                {
                    m_dwIncGamePointTick = HUtil32.GetTickCount();
                    if (m_nGamePoint + m_PEnvir.Flag.nINCGAMEPOINT <= 2000000)
                    {
                        m_nGamePoint += m_PEnvir.Flag.nINCGAMEPOINT;
                        nInteger = m_PEnvir.Flag.nINCGAMEPOINT;
                    }
                    else
                    {
                        m_nGamePoint = 2000000;
                        nInteger = 2000000 - m_nGamePoint;
                    }
                    if (M2Share.g_boGameLogGamePoint)
                    {
                        M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, Grobal2.LOG_GAMEPOINT, m_sMapName, m_nCurrX, m_nCurrY, m_sCharName, M2Share.g_Config.sGamePointName, nInteger, '+', "Map"));
                    }
                }
            }
            if (m_PEnvir.Flag.boDECHP && (HUtil32.GetTickCount() - m_dwDecHPTick) > (m_PEnvir.Flag.nDECHPTIME * 1000))
            {
                m_dwDecHPTick = HUtil32.GetTickCount();
                if (m_WAbil.HP > m_PEnvir.Flag.nDECHPPOINT)
                {
                    m_WAbil.HP -= (ushort)m_PEnvir.Flag.nDECHPPOINT;
                }
                else
                {
                    m_WAbil.HP = 0;
                }
                HealthSpellChanged();
            }
            if (m_PEnvir.Flag.boINCHP && (HUtil32.GetTickCount() - m_dwIncHPTick) > (m_PEnvir.Flag.nINCHPTIME * 1000))
            {
                m_dwIncHPTick = HUtil32.GetTickCount();
                if (m_WAbil.HP + m_PEnvir.Flag.nDECHPPOINT < m_WAbil.MaxHP)
                {
                    m_WAbil.HP += (ushort)m_PEnvir.Flag.nDECHPPOINT;
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
                if ((HUtil32.GetTickCount() - m_dwDecHungerPointTick) > 1000)
                {
                    m_dwDecHungerPointTick = HUtil32.GetTickCount();
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
                if (M2Share.g_HighLevelHuman == this && (m_boDeath || m_boGhost))
                {
                    M2Share.g_HighLevelHuman = null;
                }
                if (M2Share.g_HighPKPointHuman == this && (m_boDeath || m_boGhost))
                {
                    M2Share.g_HighPKPointHuman = null;
                }
                if (M2Share.g_HighDCHuman == this && (m_boDeath || m_boGhost))
                {
                    M2Share.g_HighDCHuman = null;
                }
                if (M2Share.g_HighMCHuman == this && (m_boDeath || m_boGhost))
                {
                    M2Share.g_HighMCHuman = null;
                }
                if (M2Share.g_HighSCHuman == this && (m_boDeath || m_boGhost))
                {
                    M2Share.g_HighSCHuman = null;
                }
                if (M2Share.g_HighOnlineHuman == this && (m_boDeath || m_boGhost))
                {
                    M2Share.g_HighOnlineHuman = null;
                }
                if (m_btPermission < 6)
                {
                    if (M2Share.g_HighLevelHuman == null || (M2Share.g_HighLevelHuman as TPlayObject).m_boGhost)
                    {
                        M2Share.g_HighLevelHuman = this;
                    }
                    else
                    {
                        if (m_Abil.Level > (M2Share.g_HighLevelHuman as TPlayObject).m_Abil.Level)
                        {
                            M2Share.g_HighLevelHuman = this;
                        }
                    }
                    // 最高PK
                    if (M2Share.g_HighPKPointHuman == null || (M2Share.g_HighPKPointHuman as TPlayObject).m_boGhost)
                    {
                        if (m_nPkPoint > 0)
                        {
                            M2Share.g_HighPKPointHuman = this;
                        }
                    }
                    else
                    {
                        if (m_nPkPoint > (M2Share.g_HighPKPointHuman as TPlayObject).m_nPkPoint)
                        {
                            M2Share.g_HighPKPointHuman = this;
                        }
                    }
                    // 最高攻击力
                    if (M2Share.g_HighDCHuman == null || (M2Share.g_HighDCHuman as TPlayObject).m_boGhost)
                    {
                        M2Share.g_HighDCHuman = this;
                    }
                    else
                    {
                        if (HUtil32.HiWord(m_WAbil.DC) > HUtil32.HiWord((M2Share.g_HighDCHuman as TPlayObject).m_WAbil.DC))
                        {
                            M2Share.g_HighDCHuman = this;
                        }
                    }
                    // 最高魔法
                    if (M2Share.g_HighMCHuman == null || (M2Share.g_HighMCHuman as TPlayObject).m_boGhost)
                    {
                        M2Share.g_HighMCHuman = this;
                    }
                    else
                    {
                        if (HUtil32.HiWord(m_WAbil.MC) > HUtil32.HiWord((M2Share.g_HighMCHuman as TPlayObject).m_WAbil.MC))
                        {
                            M2Share.g_HighMCHuman = this;
                        }
                    }
                    // 最高道术
                    if (M2Share.g_HighSCHuman == null || (M2Share.g_HighSCHuman as TPlayObject).m_boGhost)
                    {
                        M2Share.g_HighSCHuman = this;
                    }
                    else
                    {
                        if (HUtil32.HiWord(m_WAbil.SC) > HUtil32.HiWord((M2Share.g_HighSCHuman as TPlayObject).m_WAbil.SC))
                        {
                            M2Share.g_HighSCHuman = this;
                        }
                    }
                    // 最长在线时间
                    if (M2Share.g_HighOnlineHuman == null || (M2Share.g_HighOnlineHuman as TPlayObject).m_boGhost)
                    {
                        M2Share.g_HighOnlineHuman = this;
                    }
                    else
                    {
                        if (m_dwLogonTick < (M2Share.g_HighOnlineHuman as TPlayObject).m_dwLogonTick)
                        {
                            M2Share.g_HighOnlineHuman = this;
                        }
                    }
                }
            }
            catch (Exception)
            {
                M2Share.MainOutMessage(sExceptionMsg3);
            }
            if (M2Share.g_Config.boReNewChangeColor && m_btReLevel > 0 && (HUtil32.GetTickCount() - m_dwReColorTick) > M2Share.g_Config.dwReNewNameColorTime)
            {
                m_dwReColorTick = HUtil32.GetTickCount();
                m_btReColorIdx++;
                if (m_btReColorIdx > M2Share.g_Config.ReNewNameColor.GetUpperBound(0))
                {
                    m_btReColorIdx = 0;
                }
                m_btNameColor = M2Share.g_Config.ReNewNameColor[m_btReColorIdx];
                RefNameColor();
            }
            // 检测侦听私聊对像
            if (m_GetWhisperHuman != null)
            {
                if (m_GetWhisperHuman.m_boDeath || m_GetWhisperHuman.m_boGhost)
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
                    if (m_DearHuman != null && (m_DearHuman.m_boDeath || m_DearHuman.m_boGhost))
                    {
                        m_DearHuman = null;
                    }
                    if (m_boMaster)
                    {
                        for (var i = m_MasterList.Count - 1; i >= 0; i--)
                        {
                            var PlayObject = m_MasterList[i];
                            if (PlayObject.m_boDeath || PlayObject.m_boGhost)
                            {
                                m_MasterList.RemoveAt(i);
                            }
                        }
                    }
                    else
                    {
                        if (m_MasterHuman != null && (m_MasterHuman.m_boDeath || m_MasterHuman.m_boGhost))
                        {
                            m_MasterHuman = null;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg4);
                M2Share.ErrorMessage(e.Message);
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
            if (m_nAutoGetExpPoint > 0 && (m_AutoGetExpEnvir == null || m_AutoGetExpEnvir == m_PEnvir) && (HUtil32.GetTickCount() - m_dwAutoGetExpTick) > m_nAutoGetExpTime)
            {
                m_dwAutoGetExpTick = HUtil32.GetTickCount();
                if (!m_boAutoGetExpInSafeZone || m_boAutoGetExpInSafeZone && InSafeZone())
                {
                    GetExp(m_nAutoGetExpPoint);
                }
            }
            base.Run();
        }

        public override bool Operate(TProcessMessage ProcessMsg)
        {
            TCharDesc CharDesc;
            int nObjCount;
            string sendMsg;
            TMessageBodyWL MessageBodyWL = null;
            TOAbility OAbility = null;
            var dwDelayTime = 0;
            int nMsgCount;
            var result = true;
            TBaseObject BaseObject = null;
            if (ProcessMsg.BaseObject > 0)
            {
                BaseObject = M2Share.ObjectManager.Get(ProcessMsg.BaseObject);
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
                    ClientQueryUserState(ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3);
                    break;
                case Grobal2.CM_QUERYUSERSET:
                    ClientQueryUserSet(ProcessMsg);
                    break;
                case Grobal2.CM_DROPITEM:
                    if (ClientDropItem(ProcessMsg.sMsg, ProcessMsg.nParam1))
                    {
                        SendDefMessage(Grobal2.SM_DROPITEM_SUCCESS, ProcessMsg.nParam1, 0, 0, 0, ProcessMsg.sMsg);
                    }
                    else
                    {
                        SendDefMessage(Grobal2.SM_DROPITEM_FAIL, ProcessMsg.nParam1, 0, 0, 0, ProcessMsg.sMsg);
                    }
                    break;
                case Grobal2.CM_PICKUP:
                    if (m_nCurrX == ProcessMsg.nParam2 && m_nCurrY == ProcessMsg.nParam3)
                    {
                        ClientPickUpItem();
                    }
                    break;
                case Grobal2.CM_OPENDOOR:
                    ClientOpenDoor(ProcessMsg.nParam2, ProcessMsg.nParam3);
                    break;
                case Grobal2.CM_TAKEONITEM:
                    ClientTakeOnItems((byte)ProcessMsg.nParam2, ProcessMsg.nParam1, ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_TAKEOFFITEM:
                    ClientTakeOffItems((byte)ProcessMsg.nParam2, ProcessMsg.nParam1, ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_EAT:
                    ClientUseItems(ProcessMsg.nParam1, ProcessMsg.sMsg);
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
                                        M2Share.MainOutMessage(format(M2Share.g_sBunOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
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
                    if (!m_boOffLineFlag)
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
                    ClientMerchantDlgSelect(ProcessMsg.nParam1, ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_MERCHANTQUERYSELLPRICE:
                    ClientMerchantQuerySellPrice(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_USERSELLITEM:
                    ClientUserSellItem(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_USERBUYITEM:
                    ClientUserBuyItem(ProcessMsg.wIdent, ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), 0, ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_USERGETDETAILITEM:
                    ClientUserBuyItem(ProcessMsg.wIdent, ProcessMsg.nParam1, 0, ProcessMsg.nParam2, ProcessMsg.sMsg);
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
                        m_boAllowGroup = true;
                    }
                    if (m_boAllowGroup)
                    {
                        SendDefMessage(Grobal2.SM_GROUPMODECHANGED, 0, 1, 0, 0, "");
                    }
                    else
                    {
                        SendDefMessage(Grobal2.SM_GROUPMODECHANGED, 0, 0, 0, 0, "");
                    }
                    break;
                case Grobal2.CM_CREATEGROUP:
                    ClientCreateGroup(ProcessMsg.sMsg.Trim());
                    break;
                case Grobal2.CM_ADDGROUPMEMBER:
                    ClientAddGroupMember(ProcessMsg.sMsg.Trim());
                    break;
                case Grobal2.CM_DELGROUPMEMBER:
                    ClientDelGroupMember(ProcessMsg.sMsg.Trim());
                    break;
                case Grobal2.CM_USERREPAIRITEM:
                    ClientRepairItem(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_MERCHANTQUERYREPAIRCOST:
                    ClientQueryRepairCost(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_DEALTRY:
                    ClientDealTry(ProcessMsg.sMsg.Trim());
                    break;
                case Grobal2.CM_DEALADDITEM:
                    ClientAddDealItem(ProcessMsg.nParam1, ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_DEALDELITEM:
                    ClientDelDealItem(ProcessMsg.nParam1, ProcessMsg.sMsg);
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
                    ClientStorageItem(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_USERTAKEBACKSTORAGEITEM:
                    ClientTakeBackStorageItem(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_WANTMINIMAP:
                    ClientGetMinMap();
                    break;
                case Grobal2.CM_USERMAKEDRUGITEM:
                    ClientMakeDrugItem(ProcessMsg.nParam1, ProcessMsg.sMsg);
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
                    ClientGuildAddMember(ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_GUILDDELMEMBER:
                    ClientGuildDelMember(ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_GUILDUPDATENOTICE:
                    ClientGuildUpdateNotice(ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_GUILDUPDATERANKINFO:
                    ClientGuildUpdateRankInfo(ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_1042:
                    M2Share.MainOutMessage("[非法数据] " + m_sCharName);
                    break;
                case Grobal2.CM_ADJUST_BONUS:
                    ClientAdjustBonus(ProcessMsg.nParam1, ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_GUILDALLY:
                    ClientGuildAlly();
                    break;
                case Grobal2.CM_GUILDBREAKALLY:
                    ClientGuildBreakAlly(ProcessMsg.sMsg);
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
                                        M2Share.MainOutMessage(format(M2Share.g_sBunOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
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
                    if (ClientWalkXY(ProcessMsg.wIdent, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.boLateDelivery, ref dwDelayTime))
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
                                        M2Share.MainOutMessage(format(M2Share.g_sWalkOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
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
                    if (ClientHorseRunXY((short)ProcessMsg.wIdent, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.boLateDelivery, ref dwDelayTime))
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
                                        M2Share.MainOutMessage(format(M2Share.g_sRunOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
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
                                        M2Share.MainOutMessage(format(M2Share.g_sRunOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
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
                    if (ClientHitXY(ProcessMsg.wIdent, ProcessMsg.nParam1, ProcessMsg.nParam2, (byte)ProcessMsg.wParam, ProcessMsg.boLateDelivery, ref dwDelayTime))
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
                                        M2Share.MainOutMessage(format(M2Share.g_sHitOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
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
                                        M2Share.MainOutMessage(format(M2Share.g_sBunOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
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
                    if (ClientSpellXY((short)ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, M2Share.ObjectManager.Get(ProcessMsg.nParam3), ProcessMsg.boLateDelivery, ref dwDelayTime))
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
                                        M2Share.MainOutMessage(format(M2Share.g_sSpellOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
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
                    if (!string.IsNullOrEmpty(ProcessMsg.sMsg))
                    {
                        ProcessUserLineMsg(ProcessMsg.sMsg);
                    }
                    break;
                case Grobal2.CM_PASSWORD:
                    ProcessClientPassword(ProcessMsg);
                    break;
                case Grobal2.RM_WALK:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_WALK, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                        CharDesc = new TCharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.m_nCharStatus;
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    }
                    break;
                case Grobal2.RM_HORSERUN:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_HORSERUN, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                        CharDesc = new TCharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.m_nCharStatus;
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    }
                    break;
                case Grobal2.RM_RUN:
                    if (ProcessMsg.BaseObject != this.ObjectId && BaseObject != null)
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_RUN, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                        CharDesc = new TCharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.m_nCharStatus;
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
                        SendSocket(m_DefMsg, ProcessMsg.sMsg);
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
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_MOVEFAIL, ObjectId, m_nCurrX, m_nCurrY, m_btDirection);
                    CharDesc = new TCharDesc();
                    CharDesc.Feature = BaseObject.GetFeatureToLong();
                    CharDesc.Status = BaseObject.m_nCharStatus;
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
                                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_BACKSTEP, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                                break;
                            case Grobal2.RM_RUSH:
                                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_RUSH, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                                break;
                            case Grobal2.RM_RUSHKUNG:
                                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_RUSHKUNG, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                                break;
                            default:
                                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_TURN, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                                break;
                        }
                        CharDesc = new TCharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.m_nCharStatus;
                        sendMsg = EDcode.EncodeBuffer(CharDesc);
                        nObjCount = GetCharColor(BaseObject);
                        if (!string.IsNullOrEmpty(ProcessMsg.sMsg))
                        {
                            sendMsg = sendMsg + EDcode.EncodeString($"{ProcessMsg.sMsg}/{nObjCount}");
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
                            if (M2Share.ObjectManager.Get(ProcessMsg.nParam3) != null)
                            {
                                if (M2Share.ObjectManager.Get(ProcessMsg.nParam3).m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                {
                                    SetPKFlag(M2Share.ObjectManager.Get(ProcessMsg.nParam3));
                                }
                                SetLastHiter(M2Share.ObjectManager.Get(ProcessMsg.nParam3));
                            }
                            if (M2Share.CastleManager.IsCastleMember(this) != null && M2Share.ObjectManager.Get(ProcessMsg.nParam3) != null)
                            {
                                M2Share.ObjectManager.Get(ProcessMsg.nParam3).bo2B0 = true;
                                M2Share.ObjectManager.Get(ProcessMsg.nParam3).m_dw2B4Tick = HUtil32.GetTickCount();
                            }
                            m_nHealthTick = 0;
                            m_nSpellTick = 0;
                            m_nPerHealth -= 1;
                            m_nPerSpell -= 1;
                            m_dwStruckTick = HUtil32.GetTickCount();
                        }
                        if (ProcessMsg.BaseObject != 0)
                        {
                            if (ProcessMsg.BaseObject == ObjectId && M2Share.g_Config.boDisableSelfStruck || BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT && M2Share.g_Config.boDisableStruck)
                            {
                                BaseObject.SendRefMsg(Grobal2.RM_HEALTHSPELLCHANGED, 0, 0, 0, 0, "");
                            }
                            else
                            {
                                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_STRUCK, ProcessMsg.BaseObject, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MaxHP, ProcessMsg.wParam);
                                MessageBodyWL = new TMessageBodyWL();
                                MessageBodyWL.lParam1 = BaseObject.GetFeature(this);
                                MessageBodyWL.lParam2 = (int)BaseObject.m_nCharStatus;
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
                    CharDesc.Status = BaseObject.m_nCharStatus;
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
                    CharDesc.Status = BaseObject.m_nCharStatus;
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    break;
                case Grobal2.RM_USERNAME:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_USERNAME, ProcessMsg.BaseObject, GetCharColor(BaseObject), 0, 0);
                    SendSocket(m_DefMsg, EDcode.EncodeString(ProcessMsg.sMsg));
                    break;
                case Grobal2.RM_WINEXP:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_WINEXP, m_Abil.Exp, HUtil32.LoWord(ProcessMsg.nParam1), HUtil32.HiWord(ProcessMsg.nParam1), 0);
                    SendSocket(m_DefMsg);
                    break;
                case Grobal2.RM_LEVELUP:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_LEVELUP, m_Abil.Exp, m_Abil.Level, 0, 0);
                    SendSocket(m_DefMsg);
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ABILITY, m_nGold, HUtil32.MakeWord(m_btJob, 99), HUtil32.LoWord(m_nGameGold), HUtil32.HiWord(m_nGameGold));
                    if (m_nSoftVersionDateEx == 0 && m_dwClientTick == 0)
                    {
                        GetOldAbil(ref OAbility);
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(OAbility));
                    }
                    else
                    {
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(m_WAbil));
                    }
                    SendDefMessage(Grobal2.SM_SUBABILITY, HUtil32.MakeLong(HUtil32.MakeWord(m_nAntiMagic, 0), 0), HUtil32.MakeWord(m_btHitPoint, m_btSpeedPoint), HUtil32.MakeWord(m_btAntiPoison, m_nPoisonRecover), HUtil32.MakeWord(m_nHealthRecover, m_nSpellRecover), "");
                    break;
                case Grobal2.RM_CHANGENAMECOLOR:
                    SendDefMessage(Grobal2.SM_CHANGENAMECOLOR, ProcessMsg.BaseObject, GetCharColor(BaseObject), 0, 0, "");
                    break;
                case Grobal2.RM_LOGON:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEWMAP, ObjectId, m_nCurrX, m_nCurrY, DayBright());
                    SendSocket(m_DefMsg, EDcode.EncodeString(m_sMapFileName));
                    SendMsg(this, Grobal2.RM_CHANGELIGHT, 0, 0, 0, 0, "");
                    SendLogon();
                    SendServerConfig();
                    ClientQueryUserName(ObjectId, m_nCurrX, m_nCurrY);
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
                    SendSocket(m_DefMsg, EDcode.EncodeString(ProcessMsg.sMsg));
                    break;
                case Grobal2.RM_ABILITY:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ABILITY, m_nGold, HUtil32.MakeWord(m_btJob, 99), HUtil32.LoWord(m_nGameGold), HUtil32.HiWord(m_nGameGold));
                    if (m_nSoftVersionDateEx == 0 && m_dwClientTick == 0)
                    {
                        GetOldAbil(ref OAbility);
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(OAbility));
                        if (M2Share.g_Config.boOldClientShowHiLevel && m_Abil.Level > 255)
                        {
                            SysMsg(M2Share.g_sClientVersionTooOld, MsgColor.Red, MsgType.Hint);
                            SysMsg("Level: " + m_Abil.Level, MsgColor.Green, MsgType.Hint);
                            SysMsg("HP: " + m_WAbil.HP + '-' + m_WAbil.MaxHP, MsgColor.Blue, MsgType.Hint);
                            SysMsg("MP: " + m_WAbil.MP + '-' + m_WAbil.MaxMP, MsgColor.Red, MsgType.Hint);
                            SysMsg("AC: " + HUtil32.LoWord(m_WAbil.AC) + '-' + HUtil32.HiWord(m_WAbil.AC), MsgColor.Green, MsgType.Hint);
                            SysMsg("MAC: " + HUtil32.LoWord(m_WAbil.MAC) + '-' + HUtil32.HiWord(m_WAbil.MAC), MsgColor.Blue, MsgType.Hint);
                            SysMsg("DC: " + HUtil32.LoWord(m_WAbil.DC) + '-' + HUtil32.HiWord(m_WAbil.DC), MsgColor.Red, MsgType.Hint);
                            SysMsg("MC: " + HUtil32.LoWord(m_WAbil.MC) + '-' + HUtil32.HiWord(m_WAbil.MC), MsgColor.Green, MsgType.Hint);
                            SysMsg("SC: " + HUtil32.LoWord(m_WAbil.SC) + '-' + HUtil32.HiWord(m_WAbil.SC), MsgColor.Blue, MsgType.Hint);
                        }
                    }
                    else
                    {
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(m_WAbil));
                    }
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
                    SendDefMessage(Grobal2.SM_ITEMSHOW, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam, ProcessMsg.sMsg);
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
                    SendDefMessage(Grobal2.SM_CHANGEMAP, ObjectId, m_nCurrX, m_nCurrY, DayBright(), ProcessMsg.sMsg);
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
                    SendDefMessage(Grobal2.SM_SENDGOODSLIST, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, 0, ProcessMsg.sMsg);
                    break;
                case Grobal2.RM_SENDUSERSELL:
                    SendDefMessage(Grobal2.SM_SENDUSERSELL, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, 0, ProcessMsg.sMsg);
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
                    SendDefMessage(Grobal2.SM_SENDDETAILGOODSLIST, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, 0, ProcessMsg.sMsg);
                    break;
                case Grobal2.RM_GOLDCHANGED:
                    SendDefMessage(Grobal2.SM_GOLDCHANGED, m_nGold, HUtil32.LoWord(m_nGameGold), HUtil32.HiWord(m_nGameGold), 0, "");
                    break;
                case Grobal2.RM_GAMEGOLDCHANGED:
                    SendGoldInfo(false);
                    break;
                case Grobal2.RM_CHANGELIGHT:
                    SendDefMessage(Grobal2.SM_CHANGELIGHT, ProcessMsg.BaseObject, (short)BaseObject.m_nLight, (short)M2Share.g_Config.nClientKey, 0, "");
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
                    var delItemList = (IList<TDeleteItem>)M2Share.ObjectManager.GetOhter(ProcessMsg.nParam1);
                    SendDelItemList(delItemList);
                    break;
                case Grobal2.RM_USERMAKEDRUGITEMLIST:
                    SendDefMessage(Grobal2.SM_SENDUSERMAKEDRUGITEMLIST, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, 0, ProcessMsg.sMsg);
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
                    CharDesc.Status = BaseObject.m_nCharStatus;
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    break;
                case Grobal2.RM_DIGUP:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DIGUP, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                    MessageBodyWL = new TMessageBodyWL();
                    MessageBodyWL.lParam1 = BaseObject.GetFeature(this);
                    MessageBodyWL.lParam2 = (int)BaseObject.m_nCharStatus;
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
                    if (M2Share.ObjectManager.Get(ProcessMsg.nParam3) != null)
                    {
                        var MessageBodyW = new TMessageBodyW();
                        MessageBodyW.Param1 = (ushort)M2Share.ObjectManager.Get(ProcessMsg.nParam3).m_nCurrX;
                        MessageBodyW.Param2 = (ushort)M2Share.ObjectManager.Get(ProcessMsg.nParam3).m_nCurrY;
                        MessageBodyW.Tag1 = HUtil32.LoWord(ProcessMsg.nParam3);
                        MessageBodyW.Tag2 = HUtil32.HiWord(ProcessMsg.nParam3);
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_FLYAXE, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(MessageBodyW));
                    }
                    break;
                case Grobal2.RM_LIGHTING:
                    if (M2Share.ObjectManager.Get(ProcessMsg.nParam3) != null)
                    {
                        MessageBodyWL = new TMessageBodyWL();
                        MessageBodyWL.lParam1 = M2Share.ObjectManager.Get(ProcessMsg.nParam3).m_nCurrX;
                        MessageBodyWL.lParam2 = M2Share.ObjectManager.Get(ProcessMsg.nParam3).m_nCurrY;
                        MessageBodyWL.lTag1 = ProcessMsg.nParam3;
                        MessageBodyWL.lTag2 = ProcessMsg.wParam;
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_LIGHTING, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, BaseObject.m_btDirection);
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
                    SendDefMessage(Grobal2.SM_SUBABILITY, HUtil32.MakeLong(HUtil32.MakeWord(m_nAntiMagic, 0), 0), HUtil32.MakeWord(m_btHitPoint, m_btSpeedPoint), HUtil32.MakeWord(m_btAntiPoison, m_nPoisonRecover), HUtil32.MakeWord(m_nHealthRecover, m_nSpellRecover), "");
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
                    SendDefMessage(Grobal2.SM_MENU_OK, ProcessMsg.nParam1, 0, 0, 0, ProcessMsg.sMsg);
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
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SPACEMOVE_SHOW, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                    }
                    else
                    {
                        m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SPACEMOVE_SHOW2, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                    }
                    CharDesc = new TCharDesc();
                    CharDesc.Feature = BaseObject.GetFeature(this);
                    CharDesc.Status = BaseObject.m_nCharStatus;
                    sendMsg = EDcode.EncodeBuffer(CharDesc);
                    nObjCount = GetCharColor(BaseObject);
                    if (ProcessMsg.sMsg != "")
                    {
                        sendMsg = sendMsg + EDcode.EncodeString(ProcessMsg.sMsg + '/' + nObjCount);
                    }
                    SendSocket(m_DefMsg, sendMsg);
                    break;
                case Grobal2.RM_RECONNECTION:
                    m_boReconnection = true;
                    SendDefMessage(Grobal2.SM_RECONNECT, 0, 0, 0, 0, ProcessMsg.sMsg);
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
                    ChangeServerMakeSlave((TSlaveInfo)M2Share.ObjectManager.GetOhter(ProcessMsg.nParam1));
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
                        CharDesc.Feature = M2Share.ObjectManager.Get(ProcessMsg.nParam2).GetFeature(this);
                        CharDesc.Status = M2Share.ObjectManager.Get(ProcessMsg.nParam2).m_nCharStatus;
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
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(MessageBodyWL) + EDcode.EncodeString(ProcessMsg.sMsg));
                    break;
                case Grobal2.RM_PASSWORDSTATUS:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_PASSWORDSTATUS, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3);
                    SendSocket(m_DefMsg, ProcessMsg.sMsg);
                    break;
                // ---------------------------元宝寄售系统---------------------------------------
                case Grobal2.RM_SENDDEALOFFFORM:// 打开出售物品窗口
                    SendDefMessage(Grobal2.SM_SENDDEALOFFFORM, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, 0, ProcessMsg.sMsg);
                    break;
                case Grobal2.RM_QUERYYBSELL:// 查询正在出售的物品
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_QUERYYBSELL, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.sMsg);
                    break;
                case Grobal2.RM_QUERYYBDEAL:// 查询可以的购买物品
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_QUERYYBDEAL, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_SELLOFFADDITEM:// 客户端往出售物品窗口里加物品 
                    ClientAddSellOffItem(ProcessMsg.nParam1, ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_SELLOFFDELITEM:// 客户端删除出售物品窗里的物品 
                    ClientDelSellOffItem(ProcessMsg.nParam1, ProcessMsg.sMsg);
                    break;
                case Grobal2.CM_SELLOFFCANCEL:// 客户端取消元宝寄售 
                    ClientCancelSellOff();
                    break;
                case Grobal2.CM_SELLOFFEND:// 客户端元宝寄售结束 
                    ClientSellOffEnd(ProcessMsg.sMsg, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3);
                    break;
                case Grobal2.CM_CANCELSELLOFFITEMING:// 取消正在寄售的物品(出售人)
                    ClientCancelSellOffIng();
                    break;
                case Grobal2.CM_SELLOFFBUYCANCEL:// 取消寄售 物品购买(购买人)
                    ClientBuyCancelSellOff(ProcessMsg.sMsg);// 出售人
                    break;
                case Grobal2.CM_SELLOFFBUY:// 确定购买寄售物品
                    ClientBuySellOffItme(ProcessMsg.sMsg);// 出售人
                    break;
                case Grobal2.RM_SELLOFFCANCEL:// 元宝寄售取消出售
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SellOffCANCEL, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.sMsg);
                    break;
                case Grobal2.RM_SELLOFFADDITEM_OK:// 客户端往出售物品窗口里加物品 成功
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELLOFFADDITEM_OK, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.sMsg);
                    break;
                case Grobal2.RM_SellOffADDITEM_FAIL:// 客户端往出售物品窗口里加物品 失败
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SellOffADDITEM_FAIL, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.sMsg);
                    break;
                case Grobal2.RM_SELLOFFDELITEM_OK:// 客户端删除出售物品窗里的物品 成功
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELLOFFDELITEM_OK, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.sMsg);
                    break;
                case Grobal2.RM_SELLOFFDELITEM_FAIL:// 客户端删除出售物品窗里的物品 失败
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELLOFFDELITEM_FAIL, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.sMsg);
                    break;
                case Grobal2.RM_SELLOFFEND_OK:// 客户端元宝寄售结束 成功
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELLOFFEND_OK, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.sMsg);
                    break;
                case Grobal2.RM_SELLOFFEND_FAIL:// 客户端元宝寄售结束 失败
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELLOFFEND_FAIL, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.sMsg);
                    break;
                case Grobal2.RM_SELLOFFBUY_OK:// 购买成功
                    this.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELLOFFBUY_OK, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam);
                    SendSocket(this.m_DefMsg, ProcessMsg.sMsg);
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
            if (m_boTransparent && m_boHideMode)
            {
                m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 0;
            }
            if (m_GroupOwner != null)
            {
                m_GroupOwner.DelMember(this);
            }
            if (m_MyGuild != null)
            {
                m_MyGuild.DelHumanObj(this);
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
                if (m_boAngryRing || m_boNoDropUseItem)
                {
                    return;
                }
                GoodItem StdItem;
                for (var i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
                {
                    if (m_UseItems[i] == null)
                    {
                        continue;
                    }
                    StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[i].wIndex);
                    if (StdItem != null)
                    {
                        if ((StdItem.Reserved & 8) != 0)
                        {
                            if (delList == null)
                            {
                                delList = new List<TDeleteItem>();
                            }
                            delList.Add(new TDeleteItem() { MakeIndex = this.m_UseItems[i].MakeIndex });
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog("16" + "\t" + m_sMapName + "\t" + m_nCurrX + "\t" + m_nCurrY + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + m_UseItems[i].MakeIndex + "\t" + HUtil32.BoolToIntStr(m_btRaceServer == Grobal2.RC_PLAYOBJECT) + "\t" + '0');
                            }
                            m_UseItems[i].wIndex = 0;
                        }
                    }
                }
                var nRate = PKLevel() > 2 ? M2Share.g_Config.nDieRedDropUseItemRate : M2Share.g_Config.nDieDropUseItemRate;
                for (var i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
                {
                    if (M2Share.RandomNumber.Random(nRate) != 0)
                    {
                        continue;
                    }
                    if (m_UseItems[i] != null && M2Share.InDisableTakeOffList(m_UseItems[i].wIndex))
                    {
                        continue;
                    }
                    // 检查是否在禁止取下列表,如果在列表中则不掉此物品
                    if (DropItemDown(m_UseItems[i], 2, true, BaseObject, this))
                    {
                        StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[i].wIndex);
                        if (StdItem != null)
                        {
                            if ((StdItem.Reserved & 10) == 0)
                            {
                                if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                {
                                    if (delList == null)
                                    {
                                        delList = new List<TDeleteItem>();
                                    }
                                    delList.Add(new TDeleteItem()
                                    {
                                        sItemName = M2Share.UserEngine.GetStdItemName(m_UseItems[i].wIndex),
                                        MakeIndex = this.m_UseItems[i].MakeIndex
                                    });
                                }
                                m_UseItems[i].wIndex = 0;
                            }
                        }
                    }
                }
                if (delList != null)
                {
                    var ObjectId = HUtil32.Sequence();
                    M2Share.ObjectManager.AddOhter(ObjectId, delList);
                    this.SendMsg(this, Grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

    }
}
