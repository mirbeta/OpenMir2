using System;
using System.Collections;
using System.Collections.Generic;

namespace M2Server
{
    public partial class TPlayObject
    {
        private void SendNotice()
        {
            var LoadList = new List<string>();
            M2Share.NoticeManager.GetNoticeMsg("Notice", LoadList);
            var sNoticeMsg = string.Empty;
            if (LoadList.Count > 0)
            {
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sNoticeMsg = sNoticeMsg + LoadList[i] + "\x20\x1B";
                }
            }
            LoadList = null;
            SendDefMessage(grobal2.SM_SENDNOTICE, 2000, 0, 0, 0, sNoticeMsg.Replace("/r/n/r/n ", ""));
        }

        public void RunNotice()
        {
            TProcessMessage Msg = null;
            const string sExceptionMsg = "[Exception] TPlayObject::RunNotice";
            if (m_boEmergencyClose || m_boKickFlag || m_boSoftClose)
            {
                if (m_boKickFlag)
                {
                    SendDefMessage(grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0, "");
                }
                MakeGhost();
            }
            else
            {
                try
                {
                    if (!m_boSendNotice)
                    {
                        SendNotice();
                        m_boSendNotice = true;
                        m_dwWaitLoginNoticeOKTick = HUtil32.GetTickCount();
                    }
                    else
                    {
                        if (HUtil32.GetTickCount() - m_dwWaitLoginNoticeOKTick > 10 * 1000)
                        {
                            m_boEmergencyClose = true;
                        }
                        while (GetMessage(ref Msg))
                        {
                            if (Msg.wIdent == grobal2.CM_LOGINNOTICEOK)
                            {
                                m_boLoginNoticeOK = true;
                                m_dwClientTick = (short)Msg.nParam1;
                                SysMsg(m_dwClientTick.ToString(), TMsgColor.c_Red, TMsgType.t_Notice);
                            }
                        }
                    }
                }
                catch
                {
                    M2Share.ErrorMessage(sExceptionMsg);
                }
            }
        }

        private void SendLogon()
        {
            var MessageBodyWL = new TMessageBodyWL();
            m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_LOGON, ObjectId, m_nCurrX, m_nCurrY, HUtil32.MakeWord(m_btDirection, m_nLight));
            MessageBodyWL.lParam1 = GetFeatureToLong();
            MessageBodyWL.lParam2 = m_nCharStatus;
            if (m_boAllowGroup)
            {
                MessageBodyWL.lTag1 = HUtil32.MakeLong(HUtil32.MakeWord(1, 0), GetFeatureEx());
            }
            else
            {
                MessageBodyWL.lTag1 = 0;
            }
            MessageBodyWL.lTag2 = 0;
            SendSocket(m_DefMsg, EDcode.EncodeBuffer(MessageBodyWL));
            var nRecog = GetFeatureToLong();
            SendDefMessage(grobal2.SM_FEATURECHANGED, ObjectId, HUtil32.LoWord(nRecog), HUtil32.HiWord(nRecog), GetFeatureEx(), "");
            SendDefMessage(grobal2.SM_ATTACKMODE, m_btAttatckMode, 0, 0, 0, "");
        }

        public void UserLogon()
        {
            TUserItem UserItem;
            var sIPaddr = "127.0.0.1";
            const string sExceptionMsg = "[Exception] TPlayObject::UserLogon";
            const string sCheckIPaddrFail = "登录IP地址不匹配！！！";
            try
            {
                if (M2Share.g_Config.boTestServer)
                {
                    if (m_Abil.Level < M2Share.g_Config.nTestLevel)
                    {
                        m_Abil.Level = (ushort)M2Share.g_Config.nTestLevel;
                    }
                    if (m_nGold < M2Share.g_Config.nTestGold)
                    {
                        m_nGold = M2Share.g_Config.nTestGold;
                    }
                }
                if (M2Share.g_Config.boTestServer || M2Share.g_Config.boServiceMode)
                {
                    m_nPayMent = 3;
                }
                m_dwMapMoveTick = HUtil32.GetTickCount();
                m_dLogonTime = DateTime.Now;
                m_dwLogonTick = HUtil32.GetTickCount();
                Initialize();
                SendMsg(this, grobal2.RM_LOGON, 0, 0, 0, 0, "");
                if (m_Abil.Level <= 7)
                {
                    if (GetRangeHumanCount() >= 80)
                    {
                        MapRandomMove(m_PEnvir.sMapName, 0);
                    }
                }
                if (m_boDieInFight3Zone)
                {
                    MapRandomMove(m_PEnvir.sMapName, 0);
                }
                if (M2Share.UserEngine.GetHumPermission(m_sCharName, ref sIPaddr, ref m_btPermission))
                {
                    if (M2Share.g_Config.PermissionSystem)
                    {
                        if (!M2Share.CompareIPaddr(m_sIPaddr, sIPaddr))
                        {
                            SysMsg(sCheckIPaddrFail, TMsgColor.c_Red, TMsgType.t_Hint);
                            m_boEmergencyClose = true;
                        }
                    }
                }
                GetStartPoint();
                for (var i = m_MagicList.Count - 1; i >= 0; i--)
                {
                    sub_4C713C(m_MagicList[i]);
                }
                if (m_boNewHuman)
                {
                    UserItem = new TUserItem();
                    if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sCandle, ref UserItem))
                    {
                        m_ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                    UserItem = new TUserItem();
                    if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sBasicDrug, ref UserItem))
                    {
                        m_ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                    UserItem = new TUserItem();
                    if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sWoodenSword, ref UserItem))
                    {
                        m_ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                    UserItem = new TUserItem();
                    var sItem = m_btGender == ObjBase.gMan
                        ? M2Share.g_Config.sClothsMan
                        : M2Share.g_Config.sClothsWoman;
                    if (M2Share.UserEngine.CopyToUserItemFromName(sItem, ref UserItem))
                    {
                        m_ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                }
                // 检查背包中的物品是否合法
                for (var i = m_ItemList.Count - 1; i >= 0; i--)
                {
                    UserItem = m_ItemList[i];
                    if (!string.IsNullOrEmpty(M2Share.UserEngine.GetStdItemName(UserItem.wIndex))) continue;
                    Dispose(m_ItemList[i]);
                    m_ItemList.RemoveAt(i);
                }
                // 检查人物身上的物品是否符合使用规则
                if (M2Share.g_Config.boCheckUserItemPlace)
                {
                    for (var i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
                    {
                        if (m_UseItems[i] == null || m_UseItems[i].wIndex <= 0) continue;
                        var StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[i].wIndex);
                        if (StdItem != null)
                        {
                            if (!M2Share.CheckUserItems(i, StdItem))
                            {
                                UserItem = m_UseItems[i];
                                if (!AddItemToBag(UserItem))
                                {
                                    m_ItemList.Insert(0, UserItem);
                                }
                                m_UseItems[i].wIndex = 0;
                            }
                        }
                        else
                        {
                            m_UseItems[i].wIndex = 0;
                        }
                    }
                }
                // 检查背包中是否有复制品
                for (var i = m_ItemList.Count - 1; i >= 0; i--)
                {
                    UserItem = m_ItemList[i];
                    var sItemName = M2Share.UserEngine.GetStdItemName(UserItem.wIndex);
                    for (var j = i - 1; j >= 0; j--)
                    {
                        var UserItem1 = m_ItemList[j];
                        if (M2Share.UserEngine.GetStdItemName(UserItem1.wIndex) == sItemName && UserItem.MakeIndex == UserItem1.MakeIndex)
                        {
                            m_ItemList.RemoveAt(j);
                            break;
                        }
                    }
                }
                for (var i = m_dwStatusArrTick.GetLowerBound(0); i <= m_dwStatusArrTick.GetUpperBound(0); i++)
                {
                    if (m_wStatusTimeArr[i] > 0)
                    {
                        m_dwStatusArrTick[i] = HUtil32.GetTickCount();
                    }
                }
                m_nCharStatus = GetCharStatus();
                RecalcLevelAbilitys();
                RecalcAbilitys();
                if (btB2 == 0)
                {
                    m_nPkPoint = 0;
                    btB2++;
                }
                if (m_nGold > M2Share.g_Config.nHumanMaxGold * 2 && M2Share.g_Config.nHumanMaxGold > 0)
                {
                    m_nGold = M2Share.g_Config.nHumanMaxGold * 2;
                }
                if (!bo6AB)
                {
                    if (m_nSoftVersionDate < M2Share.g_Config.nSoftVersionDate)
                    {
                        SysMsg(M2Share.sClientSoftVersionError, TMsgColor.c_Red, TMsgType.t_Hint);
                        SysMsg(M2Share.sDownLoadNewClientSoft, TMsgColor.c_Red, TMsgType.t_Hint);
                        SysMsg(M2Share.sForceDisConnect, TMsgColor.c_Red, TMsgType.t_Hint);
                        m_boEmergencyClose = true;
                        return;
                    }
                    if (m_nSoftVersionDateEx == 0 && M2Share.g_Config.boOldClientShowHiLevel)
                    {
                        SysMsg(M2Share.sClientSoftVersionTooOld, TMsgColor.c_Blue, TMsgType.t_Hint);
                        SysMsg(M2Share.sDownLoadAndUseNewClient, TMsgColor.c_Red, TMsgType.t_Hint);
                        if (!M2Share.g_Config.boCanOldClientLogon)
                        {
                            SysMsg(M2Share.sClientSoftVersionError, TMsgColor.c_Red, TMsgType.t_Hint);
                            SysMsg(M2Share.sDownLoadNewClientSoft, TMsgColor.c_Red, TMsgType.t_Hint);
                            SysMsg(M2Share.sForceDisConnect, TMsgColor.c_Red, TMsgType.t_Hint);
                            m_boEmergencyClose = true;
                            return;
                        }
                    }
                    switch (m_btAttatckMode)
                    {
                        case M2Share.HAM_ALL:// [攻击模式: 全体攻击]
                            SysMsg(M2Share.sAttackModeOfAll, TMsgColor.c_Green, TMsgType.t_Hint);
                            break;
                        case M2Share.HAM_PEACE:// [攻击模式: 和平攻击]
                            SysMsg(M2Share.sAttackModeOfPeaceful, TMsgColor.c_Green, TMsgType.t_Hint);
                            break;
                        case M2Share.HAM_DEAR:// [攻击模式: 和平攻击]
                            SysMsg(M2Share.sAttackModeOfDear, TMsgColor.c_Green, TMsgType.t_Hint);
                            break;
                        case M2Share.HAM_MASTER:// [攻击模式: 和平攻击]
                            SysMsg(M2Share.sAttackModeOfMaster, TMsgColor.c_Green, TMsgType.t_Hint);
                            break;
                        case M2Share.HAM_GROUP:// [攻击模式: 编组攻击]
                            SysMsg(M2Share.sAttackModeOfGroup, TMsgColor.c_Green, TMsgType.t_Hint);
                            break;
                        case M2Share.HAM_GUILD:// [攻击模式: 行会攻击]
                            SysMsg(M2Share.sAttackModeOfGuild, TMsgColor.c_Green, TMsgType.t_Hint);
                            break;
                        case M2Share.HAM_PKATTACK:// [攻击模式: 红名攻击]
                            SysMsg(M2Share.sAttackModeOfRedWhite, TMsgColor.c_Green, TMsgType.t_Hint);
                            break;
                    }
                    SysMsg(M2Share.sStartChangeAttackModeHelp, TMsgColor.c_Green, TMsgType.t_Hint);// 使用组合快捷键 CTRL-H 更改攻击...
                    if (M2Share.g_Config.boTestServer)
                    {
                        SysMsg(M2Share.sStartNoticeMsg, TMsgColor.c_Green, TMsgType.t_Hint);// 欢迎进入本服务器进行游戏...
                    }
                    if (M2Share.UserEngine.PlayObjectCount > M2Share.g_Config.nTestUserLimit)
                    {
                        if (m_btPermission < 2)
                        {
                            SysMsg(M2Share.sOnlineUserFull, TMsgColor.c_Red, TMsgType.t_Hint);
                            SysMsg(M2Share.sForceDisConnect, TMsgColor.c_Red, TMsgType.t_Hint);
                            m_boEmergencyClose = true;
                        }
                    }
                }
                m_btBright = (byte)M2Share.g_nGameTime;
                m_Abil.MaxExp = GetLevelExp(m_Abil.Level);// 登录重新取得升级所需经验值
                SendMsg(this, grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                SendMsg(this, grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
                SendMsg(this, grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                SendMsg(this, grobal2.RM_DAYCHANGING, 0, 0, 0, 0, "");
                SendMsg(this, grobal2.RM_SENDUSEITEMS, 0, 0, 0, 0, "");
                SendMsg(this, grobal2.RM_SENDMYMAGIC, 0, 0, 0, 0, "");
                // FeatureChanged(); //增加，广播人物骑马信息
                m_MyGuild = M2Share.GuildManager.MemberOfGuild(m_sCharName);
                if (m_MyGuild != null)
                {
                    m_sGuildRankName = m_MyGuild.GetRankName(this, ref m_nGuildRankNo);
                    for (var i = m_MyGuild.GuildWarList.Count - 1; i >= 0; i--)
                    {
                        SysMsg(m_MyGuild.GuildWarList[i] + " 正在与本行会进行行会战.", TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                }
                RefShowName();
                if (m_nPayMent == 1)
                {
                    if (!bo6AB)
                    {
                        SysMsg(M2Share.sYouNowIsTryPlayMode, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    m_nGoldMax = M2Share.g_Config.nHumanTryModeMaxGold;
                    if (m_Abil.Level > M2Share.g_Config.nTryModeLevel)
                    {
                        SysMsg("测试状态可以使用到第 " + M2Share.g_Config.nTryModeLevel, TMsgColor.c_Red, TMsgType.t_Hint);
                        SysMsg("链接中断，请到以下地址获得收费相关信息。(http://www.mir2.com)", TMsgColor.c_Red, TMsgType.t_Hint);
                        m_boEmergencyClose = true;
                    }
                }
                if (m_nPayMent == 3 && !bo6AB)
                {
                    SysMsg(M2Share.g_sNowIsFreePlayMode, TMsgColor.c_Green, TMsgType.t_Hint);
                }
                if (M2Share.g_Config.boVentureServer)
                {
                    SysMsg("当前服务器运行于冒险模式.", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                if (m_MagicErgumSkill != null && !m_boUseThrusting)
                {
                    m_boUseThrusting = true;
                    SendSocket("+LNG");
                }
                if (m_PEnvir.Flag.boNORECONNECT)
                {
                    MapRandomMove(m_PEnvir.Flag.sNoReConnectMap, 0);
                }
                if (CheckDenyLogon())// 如果人物在禁止登录列表里则直接掉线而不执行下面内容
                {
                    return;
                }
                if (M2Share.g_ManageNPC != null)
                {
                    M2Share.g_ManageNPC.GotoLable(this, "@Login", false);
                }
                m_boFixedHideMode = false;
                if (!string.IsNullOrEmpty(m_sDearName))
                {
                    CheckMarry();
                }
                CheckMaster();
                m_boFilterSendMsg = M2Share.GetDisableSendMsgList(m_sCharName);
                // 密码保护系统
                if (M2Share.g_Config.boPasswordLockSystem)
                {
                    if (m_boPasswordLocked)
                    {
                        m_boCanGetBackItem = !M2Share.g_Config.boLockGetBackItemAction;
                    }
                    if (M2Share.g_Config.boLockHumanLogin && m_boLockLogon && m_boPasswordLocked)
                    {
                        m_boCanDeal = !M2Share.g_Config.boLockDealAction;
                        m_boCanDrop = !M2Share.g_Config.boLockDropAction;
                        m_boCanUseItem = !M2Share.g_Config.boLockUserItemAction;
                        m_boCanWalk = !M2Share.g_Config.boLockWalkAction;
                        m_boCanRun = !M2Share.g_Config.boLockRunAction;
                        m_boCanHit = !M2Share.g_Config.boLockHitAction;
                        m_boCanSpell = !M2Share.g_Config.boLockSpellAction;
                        m_boCanSendMsg = !M2Share.g_Config.boLockSendMsgAction;
                        m_boObMode = M2Share.g_Config.boLockInObModeAction;
                        m_boAdminMode = M2Share.g_Config.boLockInObModeAction;
                        SysMsg(M2Share.g_sActionIsLockedMsg + " 开锁命令: @" + M2Share.g_GameCommand.LOCKLOGON.sCmd, TMsgColor.c_Red, TMsgType.t_Hint);
                        SendMsg(M2Share.g_ManageNPC, grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sActionIsLockedMsg + "\\ \\" + "密码命令: @" + M2Share.g_GameCommand.PASSWORDLOCK.sCmd);
                    }
                    if (!m_boPasswordLocked)
                    {
                        SysMsg(format(M2Share.g_sPasswordNotSetMsg, M2Share.g_GameCommand.PASSWORDLOCK.sCmd), TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    if (!m_boLockLogon && m_boPasswordLocked)
                    {
                        SysMsg(format(M2Share.g_sNotPasswordProtectMode, M2Share.g_GameCommand.LOCKLOGON.sCmd), TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    SysMsg(M2Share.g_sActionIsLockedMsg + " 开锁命令: @" + M2Share.g_GameCommand.UNLOCK.sCmd, TMsgColor.c_Red, TMsgType.t_Hint);
                    SendMsg(M2Share.g_ManageNPC, grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sActionIsLockedMsg + "\\ \\" + "开锁命令: @" + M2Share.g_GameCommand.UNLOCK.sCmd + '\\' + "加锁命令: @" + M2Share.g_GameCommand.__LOCK.sCmd + '\\' + "设置密码命令: @" + M2Share.g_GameCommand.SETPASSWORD.sCmd + '\\' + "修改密码命令: @" + M2Share.g_GameCommand.CHGPASSWORD.sCmd);
                }
                // 重置泡点方面计时
                m_dwIncGamePointTick = HUtil32.GetTickCount();
                m_dwIncGameGoldTick = HUtil32.GetTickCount();
                m_dwAutoGetExpTick = HUtil32.GetTickCount();
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message);
            }
            // ReadAllBook();
        }

        public override void Run()
        {
            int tObjCount;
            int nInteger;
            TProcessMessage ProcessMsg = null;
            const string sPayMentExpire = "您的帐户充值时间已到期！！！";
            const string sDisConnectMsg = "游戏被强行中断！！！";
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
                    SysMsg(sPayMentExpire, TMsgColor.c_Red, TMsgType.t_Hint);
                    SysMsg(sDisConnectMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    m_boEmergencyClose = true;
                    m_boExpire = false;
                }
                if (m_boFireHitSkill && HUtil32.GetTickCount() - m_dwLatestFireHitTick > 20 * 1000)
                {
                    m_boFireHitSkill = false;
                    SysMsg(M2Share.sSpiritsGone, TMsgColor.c_Red, TMsgType.t_Hint);
                    SendSocket("+UFIR");
                }
                if (m_boTwinHitSkill && HUtil32.GetTickCount() - m_dwLatestTwinHitTick > 60 * 1000)
                {
                    m_boTwinHitSkill = false;
                    SendSocket("+UTWN");
                }
                if (m_boTimeRecall && HUtil32.GetTickCount() > m_dwTimeRecallTick)
                {
                    m_boTimeRecall = false;
                    SpaceMove(m_sMoveMap, m_nMoveX, m_nMoveY, 0);
                }
                // 增加挂机
                if (m_boOffLineFlag && HUtil32.GetTickCount() > m_dwKickOffLineTick)
                {
                    m_boOffLineFlag = false;
                    m_boSoftClose = true;
                }
                if (m_boDelayCall && HUtil32.GetTickCount() - m_dwDelayCallTick > m_nDelayCall)
                {
                    m_boDelayCall = false;
                    var normNpc = M2Share.UserEngine.FindMerchant<TNormNpc>(m_DelayCallNPC);
                    if (normNpc == null)
                    {
                        normNpc = M2Share.UserEngine.FindNPC(m_DelayCallNPC);
                    }
                    if (normNpc != null)
                    {
                        normNpc.GotoLable(this, m_sDelayCallLabel, false);
                    }
                }
                if (HUtil32.GetTickCount() - m_dwCheckDupObjTick > 3000)
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
                    if ((tObjCount >= 3 && HUtil32.GetTickCount() - m_dwDupObjTick > 3000 || tObjCount == 2
                        && HUtil32.GetTickCount() - m_dwDupObjTick > 10000) && HUtil32.GetTickCount() - m_dwDupObjTick < 20000)
                    {
                        CharPushed((byte)M2Share.RandomNumber.Random(8), 1);
                    }
                }
                var castle = M2Share.CastleManager.InCastleWarArea(this);
                if (castle != null && castle.m_boUnderWar)
                {
                    ChangePKStatus(true);
                }
                if (HUtil32.GetTickCount() - dwTick578 > 1000)
                {
                    dwTick578 = HUtil32.GetTickCount();
                    var wHour = DateTime.Now.Hour;
                    var wMin = DateTime.Now.Minute;
                    var wSec = DateTime.Now.Second;
                    var wMSec = DateTime.Now.Millisecond;
                    if (M2Share.g_Config.boDiscountForNightTime && (wHour == M2Share.g_Config.nHalfFeeStart || wHour == M2Share.g_Config.nHalfFeeEnd))
                    {
                        if (wMin == 0 && wSec <= 30 && HUtil32.GetTickCount() - m_dwLogonTick > 60000)
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
                                        M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_211, M2Share.nServerIndex, m_MyGuild.sGuildName);
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
                if (HUtil32.GetTickCount() - dwTick57C > 500)
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
                while (HUtil32.GetTickCount() - m_dwGetMsgTick < M2Share.g_Config.dwHumanGetMsgTime && GetMessage(ref ProcessMsg))
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
                        SendDefMessage(grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0, "");
                    }
                    if (!m_boReconnection && m_boSoftClose)
                    {
                        m_MyGuild = M2Share.GuildManager.MemberOfGuild(m_sCharName);
                        if (m_MyGuild != null)
                        {
                            m_MyGuild.SendGuildMsg(m_sCharName + " 已经退出游戏.");
                            M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_208, M2Share.nServerIndex, m_MyGuild.sGuildName + '/' + "" + '/' + m_sCharName + " has exited the game.");
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
            if (m_boDecGameGold && HUtil32.GetTickCount() - m_dwDecGameGoldTick > m_dwDecGameGoldTime)
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
                    M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, grobal2.LOG_GAMEGOLD, m_sMapName, m_nCurrX, m_nCurrY, m_sCharName, M2Share.g_Config.sGameGoldName, nInteger, '-', "Auto"));
                }
            }
            if (m_boIncGameGold && HUtil32.GetTickCount() - m_dwIncGameGoldTick > m_dwIncGameGoldTime)
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
                    M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, grobal2.LOG_GAMEGOLD, m_sMapName, m_nCurrX, m_nCurrY, m_sCharName, M2Share.g_Config.sGameGoldName, nInteger, '-', "Auto"));
                }
            }
            if (!m_boDecGameGold && m_PEnvir.Flag.boDECGAMEGOLD)
            {
                if (HUtil32.GetTickCount() - m_dwDecGameGoldTick > m_PEnvir.Flag.nDECGAMEGOLDTIME * 1000)
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
                        M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, grobal2.LOG_GAMEGOLD, m_sMapName, m_nCurrX, m_nCurrY, m_sCharName, M2Share.g_Config.sGameGoldName, nInteger, '-', "Map"));
                    }
                }
            }
            if (!m_boIncGameGold && m_PEnvir.Flag.boINCGAMEGOLD)
            {
                if (HUtil32.GetTickCount() - m_dwIncGameGoldTick > m_PEnvir.Flag.nINCGAMEGOLDTIME * 1000)
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
                        M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, grobal2.LOG_GAMEGOLD, m_sMapName, m_nCurrX, m_nCurrY, m_sCharName, M2Share.g_Config.sGameGoldName, nInteger, '+', "Map"));
                    }
                }
            }
            if (tObjCount != m_nGameGold)
            {
                SendUpdateMsg(this, grobal2.RM_GOLDCHANGED, 0, 0, 0, 0, "");
            }
            if (m_PEnvir.Flag.boINCGAMEPOINT)
            {
                if (HUtil32.GetTickCount() - m_dwIncGamePointTick > m_PEnvir.Flag.nINCGAMEPOINTTIME * 1000)
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
                        M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, grobal2.LOG_GAMEPOINT, m_sMapName, m_nCurrX, m_nCurrY, m_sCharName, M2Share.g_Config.sGamePointName, nInteger, '+', "Map"));
                    }
                }
            }
            if (m_PEnvir.Flag.boDECHP && HUtil32.GetTickCount() - m_dwDecHPTick > m_PEnvir.Flag.nDECHPTIME * 1000)
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
            if (m_PEnvir.Flag.boINCHP && HUtil32.GetTickCount() - m_dwIncHPTick > m_PEnvir.Flag.nINCHPTIME * 1000)
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
                if (HUtil32.GetTickCount() - m_dwDecHungerPointTick > 1000)
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
            if (HUtil32.GetTickCount() - m_dwRateTick > 1000)
            {
                m_dwRateTick = HUtil32.GetTickCount();
                if (m_dwKillMonExpRateTime > 0)
                {
                    m_dwKillMonExpRateTime -= 1;
                    if (m_dwKillMonExpRateTime == 0)
                    {
                        m_nKillMonExpRate = 100;
                        SysMsg("经验倍数恢复正常...", TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                }
                if (m_dwPowerRateTime > 0)
                {
                    m_dwPowerRateTime -= 1;
                    if (m_dwPowerRateTime == 0)
                    {
                        m_nPowerRate = 100;
                        SysMsg("攻击力倍数恢复正常...", TMsgColor.c_Red, TMsgType.t_Hint);
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
            if (M2Share.g_Config.boReNewChangeColor && m_btReLevel > 0 && HUtil32.GetTickCount() - m_dwReColorTick > M2Share.g_Config.dwReNewNameColorTime)
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
                if (HUtil32.GetTickCount() - m_dwClearObjTick > 10000)
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
            if (m_nAutoGetExpPoint > 0 && (m_AutoGetExpEnvir == null || m_AutoGetExpEnvir == m_PEnvir) && HUtil32.GetTickCount() - m_dwAutoGetExpTick > m_nAutoGetExpTime)
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
            string s1C;
            TMessageBodyWL MessageBodyWL = null;
            TOAbility OAbility = null;
            var dwDelayTime = 0;
            int nMsgCount;
            var result = true;
            TBaseObject BaseObject = null;
            if (ProcessMsg.BaseObject > 0)
            {
                BaseObject = M2Share.ObjectSystem.Get(ProcessMsg.BaseObject);
            }
            switch (ProcessMsg.wIdent)
            {
                case grobal2.CM_QUERYUSERNAME:
                    ClientQueryUserName(ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3);
                    break;
                case grobal2.CM_QUERYBAGITEMS: //僵尸攻击：不断刷新包裹发送大量数据，导致网络阻塞
                    if (HUtil32.GetTickCount() - m_dwQueryBagItemsTick > 30 * 1000)
                    {
                        m_dwQueryBagItemsTick = HUtil32.GetTickCount();
                        ClientQueryBagItems();
                    }
                    else
                    {
                        SysMsg(M2Share.g_sQUERYBAGITEMS, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case grobal2.CM_QUERYUSERSTATE:
                    ClientQueryUserState(ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3);
                    break;
                case grobal2.CM_QUERYUSERSET:
                    ClientQueryUserSet(ProcessMsg);
                    break;
                case grobal2.CM_DROPITEM:
                    if (ClientDropItem(ProcessMsg.sMsg, ProcessMsg.nParam1))
                    {
                        SendDefMessage(grobal2.SM_DROPITEM_SUCCESS, ProcessMsg.nParam1, 0, 0, 0, ProcessMsg.sMsg);
                    }
                    else
                    {
                        SendDefMessage(grobal2.SM_DROPITEM_FAIL, ProcessMsg.nParam1, 0, 0, 0, ProcessMsg.sMsg);
                    }
                    break;
                case grobal2.CM_PICKUP:
                    if (m_nCurrX == ProcessMsg.nParam2 && m_nCurrY == ProcessMsg.nParam3)
                    {
                        ClientPickUpItem();
                    }
                    break;
                case grobal2.CM_OPENDOOR:
                    ClientOpenDoor(ProcessMsg.nParam2, ProcessMsg.nParam3);
                    break;
                case grobal2.CM_TAKEONITEM:
                    ClientTakeOnItems((byte)ProcessMsg.nParam2, ProcessMsg.nParam1, ProcessMsg.sMsg);
                    break;
                case grobal2.CM_TAKEOFFITEM:
                    ClientTakeOffItems((byte)ProcessMsg.nParam2, ProcessMsg.nParam1, ProcessMsg.sMsg);
                    break;
                case grobal2.CM_EAT:
                    ClientUseItems(ProcessMsg.nParam1, ProcessMsg.sMsg);
                    break;
                case grobal2.CM_BUTCH:
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
                                        SysMsg(M2Share.g_sKickClientUserMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.MainOutMessage(format(M2Share.g_sBunOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime < M2Share.g_Config.dwDropOverSpeed)
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), TMsgColor.c_Red, TMsgType.t_Hint);
                                    }
                                    SendSocket(grobal2.sSTATUS_GOOD + HUtil32.GetTickCount());
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
                case grobal2.CM_MAGICKEYCHANGE:
                    ClientChangeMagicKey(ProcessMsg.nParam1, ProcessMsg.nParam2);
                    break;
                case grobal2.CM_SOFTCLOSE:
                    if (!m_boOffLineFlag)
                    {
                        m_boReconnection = true;
                        m_boSoftClose = true;
                    }
                    break;
                case grobal2.CM_CLICKNPC:
                    ClientClickNPC(ProcessMsg.nParam1);
                    break;
                case grobal2.CM_MERCHANTDLGSELECT:
                    ClientMerchantDlgSelect(ProcessMsg.nParam1, ProcessMsg.sMsg);
                    break;
                case grobal2.CM_MERCHANTQUERYSELLPRICE:
                    ClientMerchantQuerySellPrice(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.sMsg);
                    break;
                case grobal2.CM_USERSELLITEM:
                    ClientUserSellItem(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.sMsg);
                    break;
                case grobal2.CM_USERBUYITEM:
                    ClientUserBuyItem(ProcessMsg.wIdent, ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), 0, ProcessMsg.sMsg);
                    break;
                case grobal2.CM_USERGETDETAILITEM:
                    ClientUserBuyItem(ProcessMsg.wIdent, ProcessMsg.nParam1, 0, ProcessMsg.nParam2, ProcessMsg.sMsg);
                    break;
                case grobal2.CM_DROPGOLD:
                    if (ProcessMsg.nParam1 > 0)
                    {
                        ClientDropGold(ProcessMsg.nParam1);
                    }
                    break;
                case grobal2.CM_1017:
                    SendDefMessage(1, 0, 0, 0, 0, "");
                    break;
                case grobal2.CM_GROUPMODE:
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
                        SendDefMessage(grobal2.SM_GROUPMODECHANGED, 0, 1, 0, 0, "");
                    }
                    else
                    {
                        SendDefMessage(grobal2.SM_GROUPMODECHANGED, 0, 0, 0, 0, "");
                    }
                    break;
                case grobal2.CM_CREATEGROUP:
                    ClientCreateGroup(ProcessMsg.sMsg.Trim());
                    break;
                case grobal2.CM_ADDGROUPMEMBER:
                    ClientAddGroupMember(ProcessMsg.sMsg.Trim());
                    break;
                case grobal2.CM_DELGROUPMEMBER:
                    ClientDelGroupMember(ProcessMsg.sMsg.Trim());
                    break;
                case grobal2.CM_USERREPAIRITEM:
                    ClientRepairItem(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.sMsg);
                    break;
                case grobal2.CM_MERCHANTQUERYREPAIRCOST:
                    ClientQueryRepairCost(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.sMsg);
                    break;
                case grobal2.CM_DEALTRY:
                    ClientDealTry(ProcessMsg.sMsg.Trim());
                    break;
                case grobal2.CM_DEALADDITEM:
                    ClientAddDealItem(ProcessMsg.nParam1, ProcessMsg.sMsg);
                    break;
                case grobal2.CM_DEALDELITEM:
                    ClientDelDealItem(ProcessMsg.nParam1, ProcessMsg.sMsg);
                    break;
                case grobal2.CM_DEALCANCEL:
                    ClientCancelDeal();
                    break;
                case grobal2.CM_DEALCHGGOLD:
                    ClientChangeDealGold(ProcessMsg.nParam1);
                    break;
                case grobal2.CM_DEALEND:
                    ClientDealEnd();
                    break;
                case grobal2.CM_USERSTORAGEITEM:
                    ClientStorageItem(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.sMsg);
                    break;
                case grobal2.CM_USERTAKEBACKSTORAGEITEM:
                    ClientTakeBackStorageItem(ProcessMsg.nParam1, HUtil32.MakeLong(ProcessMsg.nParam2, ProcessMsg.nParam3), ProcessMsg.sMsg);
                    break;
                case grobal2.CM_WANTMINIMAP:
                    ClientGetMinMap();
                    break;
                case grobal2.CM_USERMAKEDRUGITEM:
                    ClientMakeDrugItem(ProcessMsg.nParam1, ProcessMsg.sMsg);
                    break;
                case grobal2.CM_OPENGUILDDLG:
                    ClientOpenGuildDlg();
                    break;
                case grobal2.CM_GUILDHOME:
                    ClientGuildHome();
                    break;
                case grobal2.CM_GUILDMEMBERLIST:
                    ClientGuildMemberList();
                    break;
                case grobal2.CM_GUILDADDMEMBER:
                    ClientGuildAddMember(ProcessMsg.sMsg);
                    break;
                case grobal2.CM_GUILDDELMEMBER:
                    ClientGuildDelMember(ProcessMsg.sMsg);
                    break;
                case grobal2.CM_GUILDUPDATENOTICE:
                    ClientGuildUpdateNotice(ProcessMsg.sMsg);
                    break;
                case grobal2.CM_GUILDUPDATERANKINFO:
                    ClientGuildUpdateRankInfo(ProcessMsg.sMsg);
                    break;
                case grobal2.CM_1042:
                    M2Share.MainOutMessage("[非法数据] " + m_sCharName);
                    break;
                case grobal2.CM_ADJUST_BONUS:
                    ClientAdjustBonus(ProcessMsg.nParam1, ProcessMsg.sMsg);
                    break;
                case grobal2.CM_GUILDALLY:
                    ClientGuildAlly();
                    break;
                case grobal2.CM_GUILDBREAKALLY:
                    ClientGuildBreakAlly(ProcessMsg.sMsg);
                    break;
#if CHECKNEWMSG
                case CM_1046:
                    M2Share.MainOutMessage(format("%s/%d/%d/%d/%d/%d/%s", new string[] {this.m_sCharName, ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, EDcode.DecodeString(ProcessMsg.sMsg)}));
                    break;
                case CM_1056:
                    M2Share.MainOutMessage(format("%s/%d/%d/%d/%d/%d/%s", new string[] {this.m_sCharName, ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, EDcode.DecodeString(ProcessMsg.sMsg)}));
                    break;
#endif
                case grobal2.CM_TURN:
                    if (ClientChangeDir((short)ProcessMsg.wIdent, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam, ref dwDelayTime))
                    {
                        m_dwActionTick = HUtil32.GetTickCount();
                        SendSocket(grobal2.sSTATUS_GOOD + HUtil32.GetTickCount());
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());
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
                                        SysMsg(M2Share.g_sKickClientUserMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.MainOutMessage(format(M2Share.g_sBunOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime < M2Share.g_Config.dwDropOverSpeed)
                                {
                                    SendSocket(grobal2.sSTATUS_GOOD + HUtil32.GetTickCount());
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), TMsgColor.c_Red, TMsgType.t_Hint);
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
                case grobal2.CM_WALK:
                    if (ClientWalkXY((short)ProcessMsg.wIdent, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.boLateDelivery, ref dwDelayTime))
                    {
                        m_dwActionTick = HUtil32.GetTickCount();
                        SendSocket(grobal2.sSTATUS_GOOD + HUtil32.GetTickCount());
                        n5F8++;
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());
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
                                        SysMsg(M2Share.g_sKickClientUserMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.MainOutMessage(format(M2Share.g_sWalkOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                // 如果超速则发送攻击失败信息
                                SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());
                                if (m_boTestSpeedMode)
                                {
                                    SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), TMsgColor.c_Red, TMsgType.t_Hint);
                                }
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.g_Config.dwDropOverSpeed && M2Share.g_Config.btSpeedControlMode == 1 && m_boFilterAction)
                                {
                                    SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), TMsgColor.c_Red, TMsgType.t_Hint);
                                    }
                                }
                                else
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("操作延迟 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), TMsgColor.c_Red, TMsgType.t_Hint);
                                    }
                                    SendDelayMsg(this, (short)ProcessMsg.wIdent, (short)ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case grobal2.CM_HORSERUN:
                    if (ClientHorseRunXY((short)ProcessMsg.wIdent, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.boLateDelivery, ref dwDelayTime))
                    {
                        m_dwActionTick = HUtil32.GetTickCount();
                        SendSocket(grobal2.sSTATUS_GOOD + HUtil32.GetTickCount());
                        n5F8++;
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());
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
                                        SysMsg(M2Share.g_sKickClientUserMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.MainOutMessage(format(M2Share.g_sRunOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());// 如果超速则发送攻击失败信息
                                if (m_boTestSpeedMode)
                                {
                                    SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), TMsgColor.c_Red, TMsgType.t_Hint);
                                }
                            }
                            else
                            {
                                if (m_boTestSpeedMode)
                                {
                                    SysMsg(format("操作延迟 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), TMsgColor.c_Red, TMsgType.t_Hint);
                                }
                                SendDelayMsg(this, (short)ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                result = false;
                            }
                        }
                    }
                    break;
                case grobal2.CM_RUN:
                    if (ClientRunXY((short)ProcessMsg.wIdent, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ref dwDelayTime))
                    {
                        m_dwActionTick = HUtil32.GetTickCount();
                        SendSocket(grobal2.sSTATUS_GOOD + HUtil32.GetTickCount());
                        n5F8++;
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());
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
                                        SysMsg(M2Share.g_sKickClientUserMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.MainOutMessage(format(M2Share.g_sRunOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.g_Config.dwDropOverSpeed && M2Share.g_Config.btSpeedControlMode == 1 && m_boFilterAction)
                                {
                                    SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), TMsgColor.c_Red, TMsgType.t_Hint);
                                    }
                                }
                                else
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("操作延迟 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), TMsgColor.c_Red, TMsgType.t_Hint);
                                    }
                                    SendDelayMsg(this, (short)ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, grobal2.CM_RUN, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case grobal2.CM_HIT:
                case grobal2.CM_HEAVYHIT:
                case grobal2.CM_BIGHIT:
                case grobal2.CM_POWERHIT:
                case grobal2.CM_LONGHIT:
                case grobal2.CM_WIDEHIT:
                case grobal2.CM_CRSHIT:
                case grobal2.CM_TWINHIT:
                case grobal2.CM_FIREHIT:
                    if (ClientHitXY((short)ProcessMsg.wIdent, ProcessMsg.nParam1, ProcessMsg.nParam2, (byte)ProcessMsg.wParam, ProcessMsg.boLateDelivery, ref dwDelayTime))
                    {
                        m_dwActionTick = HUtil32.GetTickCount();
                        SendSocket(grobal2.sSTATUS_GOOD + HUtil32.GetTickCount());
                        n5F8++;
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());
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
                                        // '请勿使用非法软件！！！'
                                        SysMsg(M2Share.g_sKickClientUserMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.MainOutMessage(format(M2Share.g_sHitOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.g_Config.dwDropOverSpeed && M2Share.g_Config.btSpeedControlMode == 1 && m_boFilterAction)
                                {
                                    SendSocket(grobal2.sSTATUS_GOOD + HUtil32.GetTickCount());
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), TMsgColor.c_Red, TMsgType.t_Hint);
                                    }
                                }
                                else
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg("操作延迟 Ident: " + ProcessMsg.wIdent + " Time: " + dwDelayTime, TMsgColor.c_Red, TMsgType.t_Hint);
                                    }
                                    SendDelayMsg(this, (short)ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case grobal2.CM_SITDOWN:
                    if (ClientSitDownHit(ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam, ref dwDelayTime))
                    {
                        m_dwActionTick = HUtil32.GetTickCount();
                        SendSocket(grobal2.sSTATUS_GOOD + HUtil32.GetTickCount());
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());
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
                                        SysMsg(M2Share.g_sKickClientUserMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.MainOutMessage(format(M2Share.g_sBunOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime < M2Share.g_Config.dwDropOverSpeed)
                                {
                                    SendSocket(grobal2.sSTATUS_GOOD + HUtil32.GetTickCount());
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), TMsgColor.c_Red, TMsgType.t_Hint);
                                    }
                                }
                                else
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("操作延迟 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), TMsgColor.c_Red, TMsgType.t_Hint);
                                    }
                                    SendDelayMsg(this, (short)ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case grobal2.CM_SPELL:
                    if (ClientSpellXY((short)ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, M2Share.ObjectSystem.Get(ProcessMsg.nParam3), ProcessMsg.boLateDelivery, ref dwDelayTime))
                    {
                        m_dwActionTick = HUtil32.GetTickCount();
                        SendSocket(grobal2.sSTATUS_GOOD + HUtil32.GetTickCount());
                        n5F8++;
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());
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
                                        SysMsg(M2Share.g_sKickClientUserMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                                        m_boEmergencyClose = true;
                                    }
                                    if (M2Share.g_Config.boViewHackMessage)
                                    {
                                        M2Share.MainOutMessage(format(M2Share.g_sSpellOverSpeed, m_sCharName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.g_Config.dwDropOverSpeed && M2Share.g_Config.btSpeedControlMode == 1 && m_boFilterAction)
                                {
                                    SendSocket(grobal2.sSTATUS_FAIL + HUtil32.GetTickCount());
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("速度异常 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), TMsgColor.c_Red, TMsgType.t_Hint);
                                    }
                                }
                                else
                                {
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg(format("操作延迟 Ident: {0} Time: {1}", ProcessMsg.wIdent, dwDelayTime), TMsgColor.c_Red, TMsgType.t_Hint);
                                    }
                                    SendDelayMsg(this, (short)ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case grobal2.CM_SAY:
                    if (!string.IsNullOrEmpty(ProcessMsg.sMsg))
                    {
                        ProcessUserLineMsg(ProcessMsg.sMsg);
                    }
                    break;
                case grobal2.CM_PASSWORD:
                    ProcessClientPassword(ProcessMsg);
                    break;
                case grobal2.RM_WALK:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_WALK, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                        CharDesc = new TCharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.m_nCharStatus;
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    }
                    break;
                case grobal2.RM_HORSERUN:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_HORSERUN, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                        CharDesc = new TCharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.m_nCharStatus;
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    }
                    break;
                case grobal2.RM_RUN:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_RUN, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                        CharDesc = new TCharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.m_nCharStatus;
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    }
                    break;
                case grobal2.RM_HIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_HIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case grobal2.RM_HEAVYHIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_HEAVYHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg, ProcessMsg.sMsg);
                    }
                    break;
                case grobal2.RM_BIGHIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_BIGHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case grobal2.RM_SPELL:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SPELL, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg, ProcessMsg.nParam3.ToString());
                    }
                    break;
                case grobal2.RM_SPELL2:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_POWERHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case grobal2.RM_MOVEFAIL:
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_MOVEFAIL, ObjectId, m_nCurrX, m_nCurrY, m_btDirection);
                    CharDesc = new TCharDesc();
                    CharDesc.Feature = BaseObject.GetFeatureToLong();
                    CharDesc.Status = BaseObject.m_nCharStatus;
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    break;
                case grobal2.RM_LONGHIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_LONGHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case grobal2.RM_WIDEHIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_WIDEHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case grobal2.RM_FIREHIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_FIREHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case grobal2.RM_CRSHIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_CRSHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case grobal2.RM_41:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_41, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case grobal2.RM_TWINHIT:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_TWINHIT, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case grobal2.RM_43:
                    if (ProcessMsg.BaseObject != this.ObjectId)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_43, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case grobal2.RM_TURN:
                case grobal2.RM_PUSH:
                case grobal2.RM_RUSH:
                case grobal2.RM_RUSHKUNG:
                    if (ProcessMsg.BaseObject != this.ObjectId || ProcessMsg.wIdent == grobal2.RM_PUSH || ProcessMsg.wIdent == grobal2.RM_RUSH || ProcessMsg.wIdent == grobal2.RM_RUSHKUNG)
                    {
                        switch (ProcessMsg.wIdent)
                        {
                            case grobal2.RM_PUSH:
                                m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_BACKSTEP, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                                break;
                            case grobal2.RM_RUSH:
                                m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_RUSH, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                                break;
                            case grobal2.RM_RUSHKUNG:
                                m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_RUSHKUNG, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                                break;
                            default:
                                m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_TURN, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                                break;
                        }
                        CharDesc = new TCharDesc();
                        CharDesc.Feature = BaseObject.GetFeature(BaseObject);
                        CharDesc.Status = BaseObject.m_nCharStatus;
                        s1C = EDcode.EncodeBuffer(CharDesc);
                        nObjCount = GetCharColor(BaseObject);
                        if (ProcessMsg.sMsg != "")
                        {
                            s1C = s1C + EDcode.EncodeString(ProcessMsg.sMsg + '/' + nObjCount);
                        }
                        SendSocket(m_DefMsg, s1C);
                        if (ProcessMsg.wIdent == grobal2.RM_TURN)
                        {
                            nObjCount = BaseObject.GetFeatureToLong();
                            SendDefMessage(grobal2.SM_FEATURECHANGED, ProcessMsg.BaseObject, HUtil32.LoWord(nObjCount), HUtil32.HiWord(nObjCount), BaseObject.GetFeatureEx(), "");
                        }
                    }
                    break;
                case grobal2.RM_STRUCK:
                case grobal2.RM_STRUCK_MAG:
                    if (ProcessMsg.wParam > 0)
                    {
                        if (ProcessMsg.BaseObject == ObjectId)
                        {
                            if (M2Share.ObjectSystem.Get(ProcessMsg.nParam3) != null)
                            {
                                if (M2Share.ObjectSystem.Get(ProcessMsg.nParam3).m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                {
                                    SetPKFlag(M2Share.ObjectSystem.Get(ProcessMsg.nParam3));
                                }
                                SetLastHiter(M2Share.ObjectSystem.Get(ProcessMsg.nParam3));
                            }
                            if (PKLevel() >= 2)
                            {
                                m_dw5D4 = HUtil32.GetTickCount();
                            }
                            if (M2Share.CastleManager.IsCastleMember(this) != null && M2Share.ObjectSystem.Get(ProcessMsg.nParam3) != null)
                            {
                                M2Share.ObjectSystem.Get(ProcessMsg.nParam3).bo2B0 = true;
                                M2Share.ObjectSystem.Get(ProcessMsg.nParam3).m_dw2B4Tick = HUtil32.GetTickCount();
                            }
                            m_nHealthTick = 0;
                            m_nSpellTick = 0;
                            m_nPerHealth -= 1;
                            m_nPerSpell -= 1;
                            m_dwStruckTick = HUtil32.GetTickCount();
                        }
                        if (ProcessMsg.BaseObject != 0)
                        {
                            if (ProcessMsg.BaseObject == ObjectId && M2Share.g_Config.boDisableSelfStruck || BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT && M2Share.g_Config.boDisableStruck)
                            {
                                BaseObject.SendRefMsg(grobal2.RM_HEALTHSPELLCHANGED, 0, 0, 0, 0, "");
                            }
                            else
                            {
                                m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_STRUCK, ProcessMsg.BaseObject, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MaxHP, ProcessMsg.wParam);
                                MessageBodyWL = new TMessageBodyWL();
                                MessageBodyWL.lParam1 = BaseObject.GetFeature(this);
                                MessageBodyWL.lParam2 = BaseObject.m_nCharStatus;
                                MessageBodyWL.lTag1 = ProcessMsg.nParam3;
                                if (ProcessMsg.wIdent == grobal2.RM_STRUCK_MAG)
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
                case grobal2.RM_DEATH:
                    if (ProcessMsg.nParam3 == 1)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_NOWDEATH, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
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
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_DEATH, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                    }
                    CharDesc = new TCharDesc();
                    CharDesc.Feature = BaseObject.GetFeature(this);
                    CharDesc.Status = BaseObject.m_nCharStatus;
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    break;
                case grobal2.RM_DISAPPEAR:
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_DISAPPEAR, ProcessMsg.BaseObject, 0, 0, 0);
                    SendSocket(m_DefMsg);
                    break;
                case grobal2.RM_SKELETON:
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SKELETON, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                    CharDesc = new TCharDesc();
                    CharDesc.Feature = BaseObject.GetFeature(this);
                    CharDesc.Status = BaseObject.m_nCharStatus;
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    break;
                case grobal2.RM_USERNAME:
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_USERNAME, ProcessMsg.BaseObject, GetCharColor(BaseObject), 0, 0);
                    SendSocket(m_DefMsg, EDcode.EncodeString(ProcessMsg.sMsg));
                    break;
                case grobal2.RM_WINEXP:
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_WINEXP, m_Abil.Exp, HUtil32.LoWord(ProcessMsg.nParam1), HUtil32.HiWord(ProcessMsg.nParam1), 0);
                    SendSocket(m_DefMsg);
                    break;
                case grobal2.RM_LEVELUP:
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_LEVELUP, m_Abil.Exp, m_Abil.Level, 0, 0);
                    SendSocket(m_DefMsg);
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_ABILITY, m_nGold, HUtil32.MakeWord(m_btJob, 99), HUtil32.LoWord(m_nGameGold), HUtil32.HiWord(m_nGameGold));
                    if (m_nSoftVersionDateEx == 0 && m_dwClientTick == 0)
                    {
                        GetOldAbil(ref OAbility);
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(OAbility));
                    }
                    else
                    {
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(m_WAbil));
                    }
                    SendDefMessage(grobal2.SM_SUBABILITY, HUtil32.MakeLong(HUtil32.MakeWord(m_nAntiMagic, 0), 0), HUtil32.MakeWord(m_btHitPoint, m_btSpeedPoint), HUtil32.MakeWord(m_btAntiPoison, m_nPoisonRecover), HUtil32.MakeWord(m_nHealthRecover, m_nSpellRecover), "");
                    break;
                case grobal2.RM_CHANGENAMECOLOR:
                    SendDefMessage(grobal2.SM_CHANGENAMECOLOR, ProcessMsg.BaseObject, GetCharColor(BaseObject), 0, 0, "");
                    break;
                case grobal2.RM_LOGON:
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_NEWMAP, ObjectId, m_nCurrX, m_nCurrY, DayBright());
                    SendSocket(m_DefMsg, EDcode.EncodeString(m_sMapFileName));
                    SendMsg(this, grobal2.RM_CHANGELIGHT, 0, 0, 0, 0, "");
                    SendLogon();
                    SendServerConfig();
                    ClientQueryUserName(ObjectId, m_nCurrX, m_nCurrY);
                    RefUserState();
                    SendMapDescription();
                    SendGoldInfo(true);
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_VERSION_FAIL, M2Share.g_Config.nClientFile1_CRC, HUtil32.LoWord(M2Share.g_Config.nClientFile2_CRC), HUtil32.HiWord(M2Share.g_Config.nClientFile2_CRC), 0);
                    SendSocket(m_DefMsg, "<<<<<<");//EDcode.EncodeBuffer(HUtil32.GetBytes(M2Share.g_Config.nClientFile3_CRC), sizeof(int))
                    break;
                case grobal2.RM_HEAR:
                case grobal2.RM_WHISPER:
                case grobal2.RM_CRY:
                case grobal2.RM_SYSMESSAGE:
                case grobal2.RM_GROUPMESSAGE:
                case grobal2.RM_SYSMESSAGE2:
                case grobal2.RM_GUILDMESSAGE:
                case grobal2.RM_SYSMESSAGE3:
                case grobal2.RM_MERCHANTSAY:
                    switch (ProcessMsg.wIdent)
                    {
                        case grobal2.RM_HEAR:
                            m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_HEAR, ProcessMsg.BaseObject, HUtil32.MakeWord(ProcessMsg.nParam1, ProcessMsg.nParam2), 0, 1);
                            break;
                        case grobal2.RM_WHISPER:
                            m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_WHISPER, ProcessMsg.BaseObject, HUtil32.MakeWord(ProcessMsg.nParam1, ProcessMsg.nParam2), 0, 1);
                            break;
                        case grobal2.RM_CRY:
                            m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_HEAR, ProcessMsg.BaseObject, HUtil32.MakeWord(ProcessMsg.nParam1, ProcessMsg.nParam2), 0, 1);
                            break;
                        case grobal2.RM_SYSMESSAGE:
                            m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SYSMESSAGE, ProcessMsg.BaseObject, HUtil32.MakeWord(ProcessMsg.nParam1, ProcessMsg.nParam2), 0, 1);
                            break;
                        case grobal2.RM_GROUPMESSAGE:
                            m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SYSMESSAGE, ProcessMsg.BaseObject, HUtil32.MakeWord(ProcessMsg.nParam1, ProcessMsg.nParam2), 0, 1);
                            break;
                        case grobal2.RM_GUILDMESSAGE:
                            m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_GUILDMESSAGE, ProcessMsg.BaseObject, HUtil32.MakeWord(ProcessMsg.nParam1, ProcessMsg.nParam2), 0, 1);
                            break;
                        case grobal2.RM_MERCHANTSAY:
                            m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_MERCHANTSAY, ProcessMsg.BaseObject, HUtil32.MakeWord(ProcessMsg.nParam1, ProcessMsg.nParam2), 0, 1);
                            break;
                    }
                    SendSocket(m_DefMsg, EDcode.EncodeString(ProcessMsg.sMsg));
                    break;
                case grobal2.RM_ABILITY:
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_ABILITY, m_nGold, HUtil32.MakeWord(m_btJob, 99), HUtil32.LoWord(m_nGameGold), HUtil32.HiWord(m_nGameGold));
                    if (m_nSoftVersionDateEx == 0 && m_dwClientTick == 0)
                    {
                        GetOldAbil(ref OAbility);
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(OAbility));
                        if (M2Share.g_Config.boOldClientShowHiLevel && m_Abil.Level > 255)
                        {
                            // '由于您使用的客户端版本太老了，无法正确显示人物信息！！！'
                            SysMsg(M2Share.g_sClientVersionTooOld, TMsgColor.c_Red, TMsgType.t_Hint);
                            SysMsg("Level: " + m_Abil.Level, TMsgColor.c_Green, TMsgType.t_Hint);
                            SysMsg("HP: " + m_WAbil.HP + '-' + m_WAbil.MaxHP, TMsgColor.c_Blue, TMsgType.t_Hint);
                            SysMsg("MP: " + m_WAbil.MP + '-' + m_WAbil.MaxMP, TMsgColor.c_Red, TMsgType.t_Hint);
                            SysMsg("AC: " + HUtil32.LoWord(m_WAbil.AC) + '-' + HUtil32.HiWord(m_WAbil.AC), TMsgColor.c_Green, TMsgType.t_Hint);
                            SysMsg("MAC: " + HUtil32.LoWord(m_WAbil.MAC) + '-' + HUtil32.HiWord(m_WAbil.MAC), TMsgColor.c_Blue, TMsgType.t_Hint);
                            SysMsg("DC: " + HUtil32.LoWord(m_WAbil.DC) + '-' + HUtil32.HiWord(m_WAbil.DC), TMsgColor.c_Red, TMsgType.t_Hint);
                            SysMsg("MC: " + HUtil32.LoWord(m_WAbil.MC) + '-' + HUtil32.HiWord(m_WAbil.MC), TMsgColor.c_Green, TMsgType.t_Hint);
                            SysMsg("SC: " + HUtil32.LoWord(m_WAbil.SC) + '-' + HUtil32.HiWord(m_WAbil.SC), TMsgColor.c_Blue, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(m_WAbil));
                    }
                    break;
                case grobal2.RM_HEALTHSPELLCHANGED:
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_HEALTHSPELLCHANGED, ProcessMsg.BaseObject, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MP, BaseObject.m_WAbil.MaxHP);
                    SendSocket(m_DefMsg);
                    break;
                case grobal2.RM_DAYCHANGING:
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_DAYCHANGING, 0, m_btBright, DayBright(), 0);
                    SendSocket(m_DefMsg);
                    break;
                case grobal2.RM_ITEMSHOW:
                    SendDefMessage(grobal2.SM_ITEMSHOW, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.wParam, ProcessMsg.sMsg);
                    break;
                case grobal2.RM_ITEMHIDE:
                    SendDefMessage(grobal2.SM_ITEMHIDE, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, 0, "");
                    break;
                case grobal2.RM_DOOROPEN:
                    SendDefMessage(grobal2.SM_OPENDOOR_OK, 0, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, "");
                    break;
                case grobal2.RM_DOORCLOSE:
                    SendDefMessage(grobal2.SM_CLOSEDOOR, 0, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, "");
                    break;
                case grobal2.RM_SENDUSEITEMS:
                    SendUseitems();
                    break;
                case grobal2.RM_WEIGHTCHANGED:
                    SendDefMessage(grobal2.SM_WEIGHTCHANGED, m_WAbil.Weight, m_WAbil.WearWeight, m_WAbil.HandWeight, 0, "");
                    break;
                case grobal2.RM_FEATURECHANGED:
                    SendDefMessage(grobal2.SM_FEATURECHANGED, ProcessMsg.BaseObject, HUtil32.LoWord(ProcessMsg.nParam1), HUtil32.HiWord(ProcessMsg.nParam1), ProcessMsg.wParam, "");
                    break;
                case grobal2.RM_CLEAROBJECTS:
                    SendDefMessage(grobal2.SM_CLEAROBJECTS, 0, 0, 0, 0, "");
                    break;
                case grobal2.RM_CHANGEMAP:
                    SendDefMessage(grobal2.SM_CHANGEMAP, ObjectId, m_nCurrX, m_nCurrY, DayBright(), ProcessMsg.sMsg);
                    RefUserState();
                    SendMapDescription();
                    SendServerConfig();
                    break;
                case grobal2.RM_BUTCH:
                    if (ProcessMsg.BaseObject != 0)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_BUTCH, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg);
                    }
                    break;
                case grobal2.RM_MAGICFIRE:
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_MAGICFIRE, ProcessMsg.BaseObject, HUtil32.LoWord(ProcessMsg.nParam2), HUtil32.HiWord(ProcessMsg.nParam2), ProcessMsg.nParam1);
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
                case grobal2.RM_MAGICFIREFAIL:
                    SendDefMessage(grobal2.SM_MAGICFIRE_FAIL, ProcessMsg.BaseObject, 0, 0, 0, "");
                    break;
                case grobal2.RM_SENDMYMAGIC:
                    SendUseMagic();
                    break;
                case grobal2.RM_MAGIC_LVEXP:
                    SendDefMessage(grobal2.SM_MAGIC_LVEXP, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.LoWord(ProcessMsg.nParam3), HUtil32.HiWord(ProcessMsg.nParam3), "");
                    break;
                case grobal2.RM_DURACHANGE:
                    SendDefMessage(grobal2.SM_DURACHANGE, ProcessMsg.nParam1, ProcessMsg.wParam, HUtil32.LoWord(ProcessMsg.nParam2), HUtil32.HiWord(ProcessMsg.nParam2), "");
                    break;
                case grobal2.RM_MERCHANTDLGCLOSE:
                    SendDefMessage(grobal2.SM_MERCHANTDLGCLOSE, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case grobal2.RM_SENDGOODSLIST:
                    SendDefMessage(grobal2.SM_SENDGOODSLIST, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, 0, ProcessMsg.sMsg);
                    break;
                case grobal2.RM_SENDUSERSELL:
                    SendDefMessage(grobal2.SM_SENDUSERSELL, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, 0, ProcessMsg.sMsg);
                    break;
                case grobal2.RM_SENDBUYPRICE:
                    SendDefMessage(grobal2.SM_SENDBUYPRICE, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case grobal2.RM_USERSELLITEM_OK:
                    SendDefMessage(grobal2.SM_USERSELLITEM_OK, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case grobal2.RM_USERSELLITEM_FAIL:
                    SendDefMessage(grobal2.SM_USERSELLITEM_FAIL, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case grobal2.RM_BUYITEM_SUCCESS:
                    SendDefMessage(grobal2.SM_BUYITEM_SUCCESS, ProcessMsg.nParam1, HUtil32.LoWord(ProcessMsg.nParam2), HUtil32.HiWord(ProcessMsg.nParam2), 0, "");
                    break;
                case grobal2.RM_BUYITEM_FAIL:
                    SendDefMessage(grobal2.SM_BUYITEM_FAIL, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case grobal2.RM_SENDDETAILGOODSLIST:
                    SendDefMessage(grobal2.SM_SENDDETAILGOODSLIST, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, 0, ProcessMsg.sMsg);
                    break;
                case grobal2.RM_GOLDCHANGED:
                    SendDefMessage(grobal2.SM_GOLDCHANGED, m_nGold, HUtil32.LoWord(m_nGameGold), HUtil32.HiWord(m_nGameGold), 0, "");
                    break;
                case grobal2.RM_GAMEGOLDCHANGED:
                    SendGoldInfo(false);
                    break;
                case grobal2.RM_CHANGELIGHT:
                    SendDefMessage(grobal2.SM_CHANGELIGHT, ProcessMsg.BaseObject, (short)BaseObject.m_nLight, (short)M2Share.g_Config.nClientKey, 0, "");
                    break;
                case grobal2.RM_LAMPCHANGEDURA:
                    SendDefMessage(grobal2.SM_LAMPCHANGEDURA, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case grobal2.RM_CHARSTATUSCHANGED:
                    SendDefMessage(grobal2.SM_CHARSTATUSCHANGED, ProcessMsg.BaseObject, HUtil32.LoWord(ProcessMsg.nParam1), HUtil32.HiWord(ProcessMsg.nParam1), ProcessMsg.wParam, "");
                    break;
                case grobal2.RM_GROUPCANCEL:
                    SendDefMessage(grobal2.SM_GROUPCANCEL, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case grobal2.RM_SENDUSERREPAIR:
                case grobal2.RM_SENDUSERSREPAIR:
                    SendDefMessage(grobal2.SM_SENDUSERREPAIR, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, 0, "");
                    break;
                case grobal2.RM_USERREPAIRITEM_OK:
                    SendDefMessage(grobal2.SM_USERREPAIRITEM_OK, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, 0, "");
                    break;
                case grobal2.RM_SENDREPAIRCOST:
                    SendDefMessage(grobal2.SM_SENDREPAIRCOST, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case grobal2.RM_USERREPAIRITEM_FAIL:
                    SendDefMessage(grobal2.SM_USERREPAIRITEM_FAIL, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case grobal2.RM_USERSTORAGEITEM:
                    SendDefMessage(grobal2.SM_SENDUSERSTORAGEITEM, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, 0, "");
                    break;
                case grobal2.RM_USERGETBACKITEM:
                    SendSaveItemList(ProcessMsg.nParam1);
                    break;
                case grobal2.RM_SENDDELITEMLIST:
                    var delItemList = (IList<int>)M2Share.ObjectSystem.GetOhter(ProcessMsg.nParam1);
                    SendDelItemList(delItemList);
                    break;
                case grobal2.RM_USERMAKEDRUGITEMLIST:
                    SendDefMessage(grobal2.SM_SENDUSERMAKEDRUGITEMLIST, ProcessMsg.nParam1, ProcessMsg.nParam2, 0, 0, ProcessMsg.sMsg);
                    break;
                case grobal2.RM_MAKEDRUG_SUCCESS:
                    SendDefMessage(grobal2.SM_MAKEDRUG_SUCCESS, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case grobal2.RM_MAKEDRUG_FAIL:
                    SendDefMessage(grobal2.SM_MAKEDRUG_FAIL, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case grobal2.RM_ALIVE:
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_ALIVE, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                    CharDesc = new TCharDesc();
                    CharDesc.Feature = BaseObject.GetFeature(this);
                    CharDesc.Status = BaseObject.m_nCharStatus;
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    break;
                case grobal2.RM_DIGUP:
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_DIGUP, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                    MessageBodyWL = new TMessageBodyWL();
                    MessageBodyWL.lParam1 = BaseObject.GetFeature(this);
                    MessageBodyWL.lParam2 = BaseObject.m_nCharStatus;
                    MessageBodyWL.lTag1 = ProcessMsg.nParam3;
                    MessageBodyWL.lTag1 = 0;
                    s1C = EDcode.EncodeBuffer(MessageBodyWL);
                    SendSocket(m_DefMsg, s1C);
                    break;
                case grobal2.RM_DIGDOWN:
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_DIGDOWN, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, 0);
                    SendSocket(m_DefMsg);
                    break;
                case grobal2.RM_FLYAXE:
                    if (M2Share.ObjectSystem.Get(ProcessMsg.nParam3) != null)
                    {
                        var MessageBodyW = new TMessageBodyW();
                        MessageBodyW.Param1 = (ushort)M2Share.ObjectSystem.Get(ProcessMsg.nParam3).m_nCurrX;
                        MessageBodyW.Param2 = (ushort)M2Share.ObjectSystem.Get(ProcessMsg.nParam3).m_nCurrY;
                        MessageBodyW.Tag1 = HUtil32.LoWord(ProcessMsg.nParam3);
                        MessageBodyW.Tag2 = HUtil32.HiWord(ProcessMsg.nParam3);
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_FLYAXE, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.wParam);
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(MessageBodyW));
                    }
                    break;
                case grobal2.RM_LIGHTING:
                    if (M2Share.ObjectSystem.Get(ProcessMsg.nParam3) != null)
                    {
                        MessageBodyWL = new TMessageBodyWL();
                        MessageBodyWL.lParam1 = M2Share.ObjectSystem.Get(ProcessMsg.nParam3).m_nCurrX;
                        MessageBodyWL.lParam2 = M2Share.ObjectSystem.Get(ProcessMsg.nParam3).m_nCurrY;
                        MessageBodyWL.lTag1 = ProcessMsg.nParam3;
                        MessageBodyWL.lTag2 = ProcessMsg.wParam;
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_LIGHTING, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, BaseObject.m_btDirection);
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(MessageBodyWL));
                    }
                    break;
                case grobal2.RM_10205:
                    SendDefMessage(grobal2.SM_716, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, "");
                    break;
                case grobal2.RM_CHANGEGUILDNAME:
                    SendChangeGuildName();
                    break;
                case grobal2.RM_SUBABILITY:
                    SendDefMessage(grobal2.SM_SUBABILITY, HUtil32.MakeLong(HUtil32.MakeWord(m_nAntiMagic, 0), 0), HUtil32.MakeWord(m_btHitPoint, m_btSpeedPoint), HUtil32.MakeWord(m_btAntiPoison, m_nPoisonRecover), HUtil32.MakeWord(m_nHealthRecover, m_nSpellRecover), "");
                    break;
                case grobal2.RM_BUILDGUILD_OK:
                    SendDefMessage(grobal2.SM_BUILDGUILD_OK, 0, 0, 0, 0, "");
                    break;
                case grobal2.RM_BUILDGUILD_FAIL:
                    SendDefMessage(grobal2.SM_BUILDGUILD_FAIL, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case grobal2.RM_DONATE_OK:
                    SendDefMessage(grobal2.SM_DONATE_OK, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case grobal2.RM_DONATE_FAIL:
                    SendDefMessage(grobal2.SM_DONATE_FAIL, ProcessMsg.nParam1, 0, 0, 0, "");
                    break;
                case grobal2.RM_MYSTATUS:
                    SendDefMessage(grobal2.SM_MYSTATUS, 0, (short)GetMyStatus(), 0, 0, "");
                    break;
                case grobal2.RM_MENU_OK:
                    SendDefMessage(grobal2.SM_MENU_OK, ProcessMsg.nParam1, 0, 0, 0, ProcessMsg.sMsg);
                    break;
                case grobal2.RM_SPACEMOVE_FIRE:
                case grobal2.RM_SPACEMOVE_FIRE2:
                    if (ProcessMsg.wIdent == grobal2.RM_SPACEMOVE_FIRE)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SPACEMOVE_HIDE, ProcessMsg.BaseObject, 0, 0, 0);
                    }
                    else
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SPACEMOVE_HIDE2, ProcessMsg.BaseObject, 0, 0, 0);
                    }
                    SendSocket(m_DefMsg);
                    break;
                case grobal2.RM_SPACEMOVE_SHOW:
                case grobal2.RM_SPACEMOVE_SHOW2:
                    if (ProcessMsg.wIdent == grobal2.RM_SPACEMOVE_SHOW)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SPACEMOVE_SHOW, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                    }
                    else
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SPACEMOVE_SHOW2, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, HUtil32.MakeWord(ProcessMsg.wParam, BaseObject.m_nLight));
                    }
                    CharDesc = new TCharDesc();
                    CharDesc.Feature = BaseObject.GetFeature(this);
                    CharDesc.Status = BaseObject.m_nCharStatus;
                    s1C = EDcode.EncodeBuffer(CharDesc);
                    nObjCount = GetCharColor(BaseObject);
                    if (ProcessMsg.sMsg != "")
                    {
                        s1C = s1C + EDcode.EncodeString(ProcessMsg.sMsg + '/' + nObjCount);
                    }
                    SendSocket(m_DefMsg, s1C);
                    break;
                case grobal2.RM_RECONNECTION:
                    m_boReconnection = true;
                    SendDefMessage(grobal2.SM_RECONNECT, 0, 0, 0, 0, ProcessMsg.sMsg);
                    break;
                case grobal2.RM_HIDEEVENT:
                    SendDefMessage(grobal2.SM_HIDEEVENT, ProcessMsg.nParam1, ProcessMsg.wParam, ProcessMsg.nParam2, ProcessMsg.nParam3, "");
                    break;
                case grobal2.RM_SHOWEVENT:
                    var ShortMessage = new TShortMessage();
                    ShortMessage.Ident = HUtil32.HiWord(ProcessMsg.nParam2);
                    ShortMessage.wMsg = 0;
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SHOWEVENT, ProcessMsg.nParam1, ProcessMsg.wParam, ProcessMsg.nParam2, ProcessMsg.nParam3);
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(ShortMessage));
                    break;
                case grobal2.RM_ADJUST_BONUS:
                    SendAdjustBonus();
                    break;
                case grobal2.RM_10401:
                    Console.WriteLine("todo ChangeServerMakeSlave...");
                    //ChangeServerMakeSlave(((TSlaveInfo)(ProcessMsg.nParam1)));
                    //Dispose(((TSlaveInfo)(ProcessMsg.nParam1)));
                    break;
                case grobal2.RM_OPENHEALTH:
                    SendDefMessage(grobal2.SM_OPENHEALTH, ProcessMsg.BaseObject, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MaxHP, 0, "");
                    break;
                case grobal2.RM_CLOSEHEALTH:
                    SendDefMessage(grobal2.SM_CLOSEHEALTH, ProcessMsg.BaseObject, 0, 0, 0, "");
                    break;
                case grobal2.RM_BREAKWEAPON:
                    SendDefMessage(grobal2.SM_BREAKWEAPON, ProcessMsg.BaseObject, 0, 0, 0, "");
                    break;
                case grobal2.RM_10414:
                    SendDefMessage(grobal2.SM_INSTANCEHEALGUAGE, ProcessMsg.BaseObject, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MaxHP, 0, "");
                    break;
                case grobal2.RM_CHANGEFACE:
                    if (ProcessMsg.nParam1 != 0 && ProcessMsg.nParam2 != 0)
                    {
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_CHANGEFACE, ProcessMsg.nParam1, HUtil32.LoWord(ProcessMsg.nParam2), HUtil32.HiWord(ProcessMsg.nParam2), 0);
                        CharDesc = new TCharDesc();
                        CharDesc.Feature = M2Share.ObjectSystem.Get(ProcessMsg.nParam2).GetFeature(this);
                        CharDesc.Status = M2Share.ObjectSystem.Get(ProcessMsg.nParam2).m_nCharStatus;
                        SendSocket(m_DefMsg, EDcode.EncodeBuffer(CharDesc));
                    }
                    break;
                case grobal2.RM_PASSWORD:
                    SendDefMessage(grobal2.SM_PASSWORD, 0, 0, 0, 0, "");
                    break;
                case grobal2.RM_PLAYDICE:
                    MessageBodyWL = new TMessageBodyWL();
                    MessageBodyWL.lParam1 = ProcessMsg.nParam1;
                    MessageBodyWL.lParam2 = ProcessMsg.nParam2;
                    MessageBodyWL.lTag1 = ProcessMsg.nParam3;
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_PLAYDICE, ProcessMsg.BaseObject, ProcessMsg.wParam, 0, 0);
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(MessageBodyWL) + EDcode.EncodeString(ProcessMsg.sMsg));
                    break;
                case grobal2.RM_PASSWORDSTATUS:
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_PASSWORDSTATUS, ProcessMsg.BaseObject, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3);
                    SendSocket(m_DefMsg, ProcessMsg.sMsg);
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
                m_wStatusTimeArr[grobal2.STATE_TRANSPARENT] = 0;
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

        public override void DropUseItems(TBaseObject BaseObject)
        {
            const string sExceptionMsg = "[Exception] TPlayObject::DropUseItems";
            IList<int> delList = null;
            try
            {
                if (m_boAngryRing || m_boNoDropUseItem)
                {
                    return;
                }
                TItem StdItem;
                for (var i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
                {
                    StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[i].wIndex);
                    if (StdItem != null)
                    {
                        if ((StdItem.Reserved & 8) != 0)
                        {
                            if (delList == null)
                            {
                                delList = new List<int>();
                            }
                            delList.Add(this.m_UseItems[i].MakeIndex);
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog("16" + "\t" + m_sMapName + "\t" + m_nCurrX + "\t" + m_nCurrY + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + m_UseItems[i].MakeIndex + "\t" + HUtil32.BoolToIntStr(m_btRaceServer == grobal2.RC_PLAYOBJECT) + "\t" + '0');
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
                    if (M2Share.InDisableTakeOffList(m_UseItems[i].wIndex))
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
                                if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                {
                                    if (delList == null)
                                    {
                                        delList = new List<int>();
                                    }
                                    delList.Add(this.m_UseItems[i].MakeIndex);
                                }
                                m_UseItems[i].wIndex = 0;
                            }
                        }
                    }
                }
                if (delList != null)
                {
                    var ObjectId = HUtil32.Sequence();
                    M2Share.ObjectSystem.AddOhter(ObjectId, delList);
                    this.SendMsg(this, grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

        /// <summary>
        /// 使用祝福油
        /// </summary>
        /// <returns></returns>
        private bool WeaptonMakeLuck()
        {
            var result = false;
            if (m_UseItems[grobal2.U_WEAPON] == null && m_UseItems[grobal2.U_WEAPON].wIndex <= 0)
            {
                return result;
            }
            var nRand = 0;
            var StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[grobal2.U_WEAPON].wIndex);
            if (StdItem != null)
            {
                nRand = Math.Abs(StdItem.DC2 - StdItem.DC) / 5;
            }
            if (M2Share.RandomNumber.Random(M2Share.g_Config.nWeaponMakeUnLuckRate) == 1)
            {
                MakeWeaponUnlock();
            }
            else
            {
                var boMakeLuck = false;
                if (m_UseItems[grobal2.U_WEAPON].btValue[4] > 0)
                {
                    m_UseItems[grobal2.U_WEAPON].btValue[4] -= 1;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, TMsgColor.c_Green, TMsgType.t_Hint);
                    boMakeLuck = true;
                }
                else if (m_UseItems[grobal2.U_WEAPON].btValue[3] < M2Share.g_Config.nWeaponMakeLuckPoint1)
                {
                    m_UseItems[grobal2.U_WEAPON].btValue[3]++;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, TMsgColor.c_Green, TMsgType.t_Hint);
                    boMakeLuck = true;
                }
                else if (m_UseItems[grobal2.U_WEAPON].btValue[3] < M2Share.g_Config.nWeaponMakeLuckPoint2 && M2Share.RandomNumber.Random(nRand + M2Share.g_Config.nWeaponMakeLuckPoint2Rate) == 1)
                {
                    m_UseItems[grobal2.U_WEAPON].btValue[3]++;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, TMsgColor.c_Green, TMsgType.t_Hint);
                    boMakeLuck = true;
                }
                else if (m_UseItems[grobal2.U_WEAPON].btValue[3] < M2Share.g_Config.nWeaponMakeLuckPoint3 && M2Share.RandomNumber.Random(nRand * M2Share.g_Config.nWeaponMakeLuckPoint3Rate) == 1)
                {
                    m_UseItems[grobal2.U_WEAPON].btValue[3]++;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, TMsgColor.c_Green, TMsgType.t_Hint);
                    boMakeLuck = true;
                }
                if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                {
                    RecalcAbilitys();
                    SendMsg(this, grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                    SendMsg(this, grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
                }
                if (!boMakeLuck)
                {
                    SysMsg(M2Share.g_sWeaptonNotMakeLuck, TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            result = true;
            return result;
        }

        /// <summary>
        /// 修复武器
        /// </summary>
        /// <returns></returns>
        private bool RepairWeapon()
        {
            if (m_UseItems[grobal2.U_WEAPON] == null)
            {
                return false;
            }
            var UserItem = m_UseItems[grobal2.U_WEAPON];
            if (UserItem.wIndex <= 0 || UserItem.DuraMax <= UserItem.Dura)
            {
                return false;
            }
            UserItem.DuraMax -= (ushort)((UserItem.DuraMax - UserItem.Dura) / M2Share.g_Config.nRepairItemDecDura);
            var nDura = HUtil32._MIN(5000, UserItem.DuraMax - UserItem.Dura);
            if (nDura <= 0) return false;
            UserItem.Dura += (ushort)nDura;
            SendMsg(this, grobal2.RM_DURACHANGE, 1, UserItem.Dura, UserItem.DuraMax, 0, "");// '武器修复成功...'
            SysMsg(M2Share.g_sWeaponRepairSuccess, TMsgColor.c_Green, TMsgType.t_Hint);
            return true;
        }

        /// <summary>
        /// 超级修复武器
        /// </summary>
        /// <returns></returns>
        private bool SuperRepairWeapon()
        {
            if (m_UseItems[grobal2.U_WEAPON] == null && m_UseItems[grobal2.U_WEAPON].wIndex <= 0)
            {
                return false;
            }
            m_UseItems[grobal2.U_WEAPON].Dura = m_UseItems[grobal2.U_WEAPON].DuraMax;
            SendMsg(this, grobal2.RM_DURACHANGE, 1, m_UseItems[grobal2.U_WEAPON].Dura,
                m_UseItems[grobal2.U_WEAPON].DuraMax, 0, "");
            SysMsg(M2Share.g_sWeaponRepairSuccess, TMsgColor.c_Green, TMsgType.t_Hint); 
            return true;
        }

        private void WinLottery()
        {
            var nGold = 0;
            var nWinLevel = 0;
            var nRate = M2Share.RandomNumber.Random(M2Share.g_Config.nWinLotteryRate);
            if (nRate >= M2Share.g_Config.nWinLottery6Min && nRate <= M2Share.g_Config.nWinLottery6Max)
            {
                if (M2Share.g_Config.nWinLotteryCount < M2Share.g_Config.nNoWinLotteryCount)
                {
                    nGold = M2Share.g_Config.nWinLottery6Gold;
                    nWinLevel = 6;
                    M2Share.g_Config.nWinLotteryLevel6++;
                }
            }
            else if (nRate >= M2Share.g_Config.nWinLottery5Min && nRate <= M2Share.g_Config.nWinLottery5Max)
            {
                if (M2Share.g_Config.nWinLotteryCount < M2Share.g_Config.nNoWinLotteryCount)
                {
                    nGold = M2Share.g_Config.nWinLottery5Gold;
                    nWinLevel = 5;
                    M2Share.g_Config.nWinLotteryLevel5++;
                }
            }
            else if (nRate >= M2Share.g_Config.nWinLottery4Min && nRate <= M2Share.g_Config.nWinLottery4Max)
            {
                if (M2Share.g_Config.nWinLotteryCount < M2Share.g_Config.nNoWinLotteryCount)
                {
                    nGold = M2Share.g_Config.nWinLottery4Gold;
                    nWinLevel = 4;
                    M2Share.g_Config.nWinLotteryLevel4++;
                }
            }
            else if (nRate >= M2Share.g_Config.nWinLottery3Min && nRate <= M2Share.g_Config.nWinLottery3Max)
            {
                if (M2Share.g_Config.nWinLotteryCount < M2Share.g_Config.nNoWinLotteryCount)
                {
                    nGold = M2Share.g_Config.nWinLottery3Gold;
                    nWinLevel = 3;
                    M2Share.g_Config.nWinLotteryLevel3++;
                }
            }
            else if (nRate >= M2Share.g_Config.nWinLottery2Min && nRate <= M2Share.g_Config.nWinLottery2Max)
            {
                if (M2Share.g_Config.nWinLotteryCount < M2Share.g_Config.nNoWinLotteryCount)
                {
                    nGold = M2Share.g_Config.nWinLottery2Gold;
                    nWinLevel = 2;
                    M2Share.g_Config.nWinLotteryLevel2++;
                }
            }
            else if (new ArrayList(new object[] { M2Share.g_Config.nWinLottery1Min + M2Share.g_Config.nWinLottery1Max }).Contains(nRate))
            {
                if (M2Share.g_Config.nWinLotteryCount < M2Share.g_Config.nNoWinLotteryCount)
                {
                    nGold = M2Share.g_Config.nWinLottery1Gold;
                    nWinLevel = 1;
                    M2Share.g_Config.nWinLotteryLevel1++;
                }
            }
            if (nGold > 0)
            {
                switch (nWinLevel)
                {
                    case 1:
                        SysMsg(M2Share.g_sWinLottery1Msg, TMsgColor.c_Green, TMsgType.t_Hint);
                        break;
                    case 2:
                        SysMsg(M2Share.g_sWinLottery2Msg, TMsgColor.c_Green, TMsgType.t_Hint);
                        break;
                    case 3:
                        SysMsg(M2Share.g_sWinLottery3Msg, TMsgColor.c_Green, TMsgType.t_Hint);
                        break;
                    case 4:
                        SysMsg(M2Share.g_sWinLottery4Msg, TMsgColor.c_Green, TMsgType.t_Hint);
                        break;
                    case 5:
                        SysMsg(M2Share.g_sWinLottery5Msg, TMsgColor.c_Green, TMsgType.t_Hint);
                        break;
                    case 6:
                        SysMsg(M2Share.g_sWinLottery6Msg, TMsgColor.c_Green, TMsgType.t_Hint);
                        break;
                }
                if (IncGold(nGold))
                {
                    GoldChanged();
                }
                else
                {
                    DropGoldDown(nGold, true, null, null);
                }
            }
            else
            {
                M2Share.g_Config.nNoWinLotteryCount += 500;
                SysMsg(M2Share.g_sNotWinLotteryMsg, TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public override void RecalcAbilitys()
        {
            base.RecalcAbilitys();
            RecalcAdjusBonus();
        }

        public override void UpdateVisibleGay(TBaseObject BaseObject)
        {
            var boIsVisible = false;
            TVisibleBaseObject VisibleBaseObject;
            if (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT || BaseObject.m_Master != null)
            {
                m_boIsVisibleActive = true;
            }
            // 如果是人物或宝宝则置TRUE
            for (var i = 0; i < m_VisibleActors.Count; i++)
            {
                VisibleBaseObject = m_VisibleActors[i];
                if (VisibleBaseObject.BaseObject == BaseObject)
                {
                    VisibleBaseObject.nVisibleFlag = 1;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            VisibleBaseObject = new TVisibleBaseObject
            {
                nVisibleFlag = 2,
                BaseObject = BaseObject
            };
            m_VisibleActors.Add(VisibleBaseObject);
            if (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                SendWhisperMsg(BaseObject as TPlayObject);
            }
        }

        public override void SearchViewRange()
        {
            TMapCellinfo MapCellInfo = null;
            TBaseObject BaseObject= null;
            TEvent MapEvent= null;
            for (var i = m_VisibleItems.Count - 1; i >= 0; i--)
            {
                m_VisibleItems[i].nVisibleFlag = 0;
            }
            for (var i = m_VisibleEvents.Count - 1; i >= 0; i--)
            {
                m_VisibleEvents[i].nVisibleFlag = 0;
            }
            for (var i = m_VisibleActors.Count - 1; i >= 0; i--)
            {
                m_VisibleActors[i].nVisibleFlag = 0;
            }
            var nStartX = m_nCurrX - m_nViewRange;
            var nEndX = m_nCurrX + m_nViewRange;
            var nStartY = m_nCurrY - m_nViewRange;
            var nEndY = m_nCurrY + m_nViewRange;
            try
            {
                for (var n18 = nStartX; n18 <= nEndX; n18++)
                {
                    for (var n1C = nStartY; n1C <= nEndY; n1C++)
                    {
                        if (m_PEnvir.GetMapCellInfo(n18, n1C, ref MapCellInfo) && MapCellInfo.ObjList != null)
                        {
                            var nIdx = 0;
                            while (true)
                            {
                                if (MapCellInfo.ObjList.Count <= nIdx)
                                {
                                    break;
                                }
                                var OSObject = MapCellInfo.ObjList[nIdx];
                                if (OSObject != null)
                                {
                                    if (OSObject.btType == grobal2.OS_MOVINGOBJECT)
                                    {
                                        if (HUtil32.GetTickCount() - OSObject.dwAddTime >= 60 * 1000)
                                        {
                                            Dispose(OSObject);
                                            MapCellInfo.ObjList.RemoveAt(nIdx);
                                            if (MapCellInfo.ObjList.Count > 0)
                                            {
                                                continue;
                                            }
                                            MapCellInfo.ObjList = null;
                                            break;
                                        }
                                        BaseObject = OSObject.CellObj as TBaseObject;
                                        if (BaseObject != null)
                                        {
                                            if (!BaseObject.m_boGhost && !BaseObject.m_boFixedHideMode && !BaseObject.m_boObMode)
                                            {
                                                if (m_btRaceServer < grobal2.RC_ANIMAL || m_Master != null || m_boCrazyMode || m_boNastyMode || m_boWantRefMsg || BaseObject.m_Master != null && Math.Abs(BaseObject.m_nCurrX - m_nCurrX) <= 3 && Math.Abs(BaseObject.m_nCurrY - m_nCurrY) <= 3 || BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                                {
                                                    UpdateVisibleGay(BaseObject);
                                                }
                                            }
                                        }
                                    }
                                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                    {
                                        if (OSObject.btType == grobal2.OS_ITEMOBJECT)
                                        {
                                            if (HUtil32.GetTickCount() - OSObject.dwAddTime > M2Share.g_Config.dwClearDropOnFloorItemTime)// 60 * 60 * 1000
                                            {
                                                Dispose((TMapItem)OSObject.CellObj);
                                                Dispose(OSObject);
                                                MapCellInfo.ObjList.RemoveAt(nIdx);
                                                if (MapCellInfo.ObjList.Count > 0)
                                                {
                                                    continue;
                                                }
                                                MapCellInfo.ObjList = null;
                                                break;
                                            }
                                            var MapItem = (TMapItem)OSObject.CellObj;
                                            UpdateVisibleItem(n18, n1C, MapItem);
                                            if (MapItem.OfBaseObject != null || MapItem.DropBaseObject != null)
                                            {
                                                // 2 * 60 * 1000
                                                if (HUtil32.GetTickCount() - MapItem.dwCanPickUpTick > M2Share.g_Config.dwFloorItemCanPickUpTime)
                                                {
                                                    MapItem.OfBaseObject = null;
                                                    MapItem.DropBaseObject = null;
                                                }
                                                else
                                                {
                                                    if (MapItem.OfBaseObject as TBaseObject != null)
                                                    {
                                                        if ((MapItem.OfBaseObject as TBaseObject).m_boGhost)
                                                        {
                                                            MapItem.OfBaseObject = null;
                                                        }
                                                    }
                                                    if (MapItem.DropBaseObject as TBaseObject != null)
                                                    {
                                                        if ((MapItem.DropBaseObject as TBaseObject).m_boGhost)
                                                        {
                                                            MapItem.DropBaseObject = null;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (OSObject.btType == grobal2.OS_EVENTOBJECT)
                                        {
                                            MapEvent = (TEvent)OSObject.CellObj;
                                            if (MapEvent.m_boVisible)
                                            {
                                                UpdateVisibleEvent(n18, n1C, MapEvent);
                                            }
                                        }
                                    }
                                }
                                nIdx++;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(e.StackTrace);
                KickException();
            }

            try
            {
                var n18 = 0;
                while (true)
                {
                    if (m_VisibleActors.Count <= n18)
                    {
                        break;
                    }
                    var VisibleBaseObject = m_VisibleActors[n18];
                    if (VisibleBaseObject.nVisibleFlag == 0)
                    {
                        if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                        {
                            BaseObject = VisibleBaseObject.BaseObject;
                            if (!BaseObject.m_boFixedHideMode && !BaseObject.m_boGhost)
                            {
                                // 01/21 修改防止人物退出时发送重复的消息占用带宽，人物进入隐身模式时人物不消失问题
                                SendMsg(BaseObject, grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                            }
                        }
                        m_VisibleActors.RemoveAt(n18);
                        Dispose(VisibleBaseObject);
                        continue;
                    }
                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT && VisibleBaseObject.nVisibleFlag == 2)
                    {
                        BaseObject = VisibleBaseObject.BaseObject;
                        if (BaseObject != this)
                        {
                            if (BaseObject.m_boDeath)
                            {
                                if (BaseObject.m_boSkeleton)
                                {
                                    SendMsg(BaseObject, grobal2.RM_SKELETON, BaseObject.m_btDirection, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 0, "");
                                }
                                else
                                {
                                    SendMsg(BaseObject, grobal2.RM_DEATH, BaseObject.m_btDirection, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 0, "");
                                }
                            }
                            else
                            {
                                SendMsg(BaseObject, grobal2.RM_TURN, BaseObject.m_btDirection, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 0, BaseObject.GetShowName());
                            }
                        }
                    }
                    n18++;
                }
            }
            catch (Exception ex)
            {
                M2Share.ErrorMessage(ex.StackTrace);
                KickException();
            }
            try
            {
                var I = 0;
                while (true)
                {
                    if (m_VisibleItems.Count <= I)
                    {
                        break;
                    }
                    var VisibleMapItem = m_VisibleItems[I];
                    if (VisibleMapItem.nVisibleFlag == 0)
                    {
                        SendMsg(this, grobal2.RM_ITEMHIDE, 0, VisibleMapItem.MapItem.Id, VisibleMapItem.nX, VisibleMapItem.nY, "");
                        m_VisibleItems.RemoveAt(I);
                        Dispose(VisibleMapItem);
                        continue;
                    }
                    if (VisibleMapItem.nVisibleFlag == 2)
                    {
                        SendMsg(this, grobal2.RM_ITEMSHOW, VisibleMapItem.wLooks, VisibleMapItem.MapItem.Id, VisibleMapItem.nX, VisibleMapItem.nY, VisibleMapItem.sName);
                    }
                    I++;
                }
                I = 0;
                while (true)
                {
                    if (m_VisibleEvents.Count <= I)
                    {
                        break;
                    }
                    MapEvent = m_VisibleEvents[I];
                    if (MapEvent.nVisibleFlag == 0)
                    {
                        SendMsg(this, grobal2.RM_HIDEEVENT, 0, MapEvent.Id, MapEvent.m_nX, MapEvent.m_nY, "");
                        m_VisibleEvents.RemoveAt(I);
                        continue;
                    }
                    if (MapEvent.nVisibleFlag == 2)
                    {
                        SendMsg(this, grobal2.RM_SHOWEVENT, (short)MapEvent.m_nEventType, MapEvent.Id, HUtil32.MakeLong(MapEvent.m_nX, MapEvent.m_nEventParam), MapEvent.m_nY, "");
                    }
                    I++;
                }
            }
            catch
            {
                M2Share.MainOutMessage(m_sCharName + ',' + m_sMapName + ',' + m_nCurrX + ',' + m_nCurrY + ',' + " SearchViewRange 3");
                KickException();
            }
        }

        public override string GetShowName()
        {
            var result = string.Empty;
            string sShowName;
            var sCharName = string.Empty;
            var sGuildName = string.Empty;
            var sDearName = string.Empty;
            var sMasterName = string.Empty;
            const string sExceptionMsg = "[Exception] TPlayObject::GetShowName";
            try
            {
                if (m_MyGuild != null)
                {
                    var Castle = M2Share.CastleManager.IsCastleMember(this);
                    if (Castle != null)
                    {
                        sGuildName = M2Share.g_sCastleGuildName.Replace("%castlename", Castle.m_sName);
                        sGuildName = sGuildName.Replace("%guildname", m_MyGuild.sGuildName);
                        sGuildName = sGuildName.Replace("%rankname", m_sGuildRankName);
                    }
                    else
                    {
                        Castle = M2Share.CastleManager.InCastleWarArea(this);// 01/25 多城堡
                        if (M2Share.g_Config.boShowGuildName || Castle != null && Castle.m_boUnderWar || m_boInFreePKArea)
                        {
                            sGuildName = M2Share.g_sNoCastleGuildName.Replace("%guildname", m_MyGuild.sGuildName);
                            sGuildName = sGuildName.Replace("%rankname", m_sGuildRankName);
                        }
                    }
                }
                if (!M2Share.g_Config.boShowRankLevelName)
                {
                    if (m_btReLevel > 0)
                    {
                        switch (m_btJob)
                        {
                            case M2Share.jWarr:
                                sCharName = M2Share.g_sWarrReNewName.Replace("%chrname", m_sCharName);
                                break;
                            case M2Share.jWizard:
                                sCharName = M2Share.g_sWizardReNewName.Replace("%chrname", m_sCharName);
                                break;
                            case M2Share.jTaos:
                                sCharName = M2Share.g_sTaosReNewName.Replace("%chrname", m_sCharName);
                                break;
                        }
                    }
                    else
                    {
                        sCharName = m_sCharName;
                    }
                }
                else
                {
                    sCharName = format(m_sRankLevelName, m_sCharName);
                }
                if (!string.IsNullOrEmpty(m_sMasterName))
                {
                    if (m_boMaster)
                    {
                        sMasterName = format(M2Share.g_sMasterName, m_sMasterName);
                    }
                    else
                    {
                        sMasterName = format(M2Share.g_sNoMasterName, m_sMasterName);
                    }
                }
                if (!string.IsNullOrEmpty(m_sDearName))
                {
                    if (m_btGender == ObjBase.gMan)
                    {
                        sDearName = format(M2Share.g_sManDearName, m_sDearName);
                    }
                    else
                    {
                        sDearName = format(M2Share.g_sWoManDearName, m_sDearName);
                    }
                }
                sShowName = M2Share.g_sHumanShowName.Replace("%chrname", sCharName);
                sShowName = sShowName.Replace("%guildname", sGuildName);
                sShowName = sShowName.Replace("%dearname", sDearName);
                sShowName = sShowName.Replace("%mastername", sMasterName);
                result = sShowName;
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message);
            }
            return result;
        }

        public override void MakeGhost()
        {
            string sSayMsg;
            TPlayObject Human;
            const string sExceptionMsg = "[Exception] TPlayObject::MakeGhost";
            try
            {
                if (M2Share.g_HighLevelHuman == this)
                {
                    M2Share.g_HighLevelHuman = null;
                }
                if (M2Share.g_HighPKPointHuman == this)
                {
                    M2Share.g_HighPKPointHuman = null;
                }
                if (M2Share.g_HighDCHuman == this)
                {
                    M2Share.g_HighDCHuman = null;
                }
                if (M2Share.g_HighMCHuman == this)
                {
                    M2Share.g_HighMCHuman = null;
                }
                if (M2Share.g_HighSCHuman == this)
                {
                    M2Share.g_HighSCHuman = null;
                }
                if (M2Share.g_HighOnlineHuman == this)
                {
                    M2Share.g_HighOnlineHuman = null;
                }
                // 人物下线后通知配偶，并把对方的相关记录清空
                if (m_DearHuman != null)
                {
                    if (m_btGender == ObjBase.gMan)
                    {
                        sSayMsg = M2Share.g_sManLongOutDearOnlineMsg.Replace("%d", m_sDearName);
                        sSayMsg = sSayMsg.Replace("%s", m_sCharName);
                        sSayMsg = sSayMsg.Replace("%m", m_PEnvir.sMapDesc);
                        sSayMsg = sSayMsg.Replace("%x", m_nCurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", m_nCurrY.ToString());
                        m_DearHuman.SysMsg(sSayMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    else
                    {
                        sSayMsg = M2Share.g_sWoManLongOutDearOnlineMsg.Replace("%d", m_sDearName);
                        sSayMsg = sSayMsg.Replace("%s", m_sCharName);
                        sSayMsg = sSayMsg.Replace("%m", m_PEnvir.sMapDesc);
                        sSayMsg = sSayMsg.Replace("%x", m_nCurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", m_nCurrY.ToString());
                        m_DearHuman.SysMsg(sSayMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    m_DearHuman.m_DearHuman = null;
                    m_DearHuman = null;
                }
                if (m_MasterHuman != null || m_MasterList.Count > 0)
                {
                    if (m_boMaster)
                    {
                        for (var i = m_MasterList.Count - 1; i >= 0; i--)
                        {
                            Human = m_MasterList[i];
                            sSayMsg = M2Share.g_sMasterLongOutMasterListOnlineMsg.Replace("%s", m_sCharName);
                            sSayMsg = sSayMsg.Replace("%m", m_PEnvir.sMapDesc);
                            sSayMsg = sSayMsg.Replace("%x", m_nCurrX.ToString());
                            sSayMsg = sSayMsg.Replace("%y", m_nCurrY.ToString());
                            Human.SysMsg(sSayMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                            Human.m_MasterHuman = null;
                        }
                    }
                    else
                    {
                        if (m_MasterHuman == null)
                        {
                            return;
                        }
                        sSayMsg = M2Share.g_sMasterListLongOutMasterOnlineMsg.Replace("%d", m_sMasterName);
                        sSayMsg = sSayMsg.Replace("%s", m_sCharName);
                        sSayMsg = sSayMsg.Replace("%m", m_PEnvir.sMapDesc);
                        sSayMsg = sSayMsg.Replace("%x", m_nCurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", m_nCurrY.ToString());
                        m_MasterHuman.SysMsg(sSayMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        // 如果为大徒弟则将对方的记录清空
                        if (m_MasterHuman.m_sMasterName == m_sCharName)
                        {
                            m_MasterHuman.m_MasterHuman = null;
                        }
                        for (var i = 0; i < m_MasterHuman.m_MasterList.Count; i++)
                        {
                            if (m_MasterHuman.m_MasterList[i] == this)
                            {
                                m_MasterHuman.m_MasterList.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message);
            }
            base.MakeGhost();
        }

        public override void ScatterBagItems(TBaseObject ItemOfCreat)
        {
            const int DropWide = 2;
            TUserItem pu;
            const string sExceptionMsg = "[Exception] TPlayObject::ScatterBagItems";
            ArrayList DelList = null;
            if (m_boAngryRing || m_boNoDropItem || m_PEnvir.Flag.boNODROPITEM)
            {
                return;// 不死戒指
            }
            var boDropall = M2Share.g_Config.boDieRedScatterBagAll && PKLevel() >= 2;
            try
            {
                for (var i = m_ItemList.Count - 1; i >= 0; i--)
                {
                    if (boDropall || M2Share.RandomNumber.Random(M2Share.g_Config.nDieScatterBagRate) == 0)
                    {
                        if (DropItemDown(m_ItemList[i], DropWide, true, ItemOfCreat, this))
                        {
                            pu = m_ItemList[i];
                            if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                            {
                                if (DelList == null)
                                {
                                    DelList = new ArrayList();
                                }
                                //DelList.Add(M2Share.UserEngine.GetStdItemName(pu.wIndex), ((pu.MakeIndex) as Object));
                            }
                            Dispose(m_ItemList[i]);
                            m_ItemList.RemoveAt(i);
                        }
                    }
                }
                if (DelList != null)
                {
                    var ObjectId = HUtil32.Sequence();
                    M2Share.ObjectSystem.AddOhter(ObjectId, DelList);
                    this.SendMsg(this, grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }
    }
}
