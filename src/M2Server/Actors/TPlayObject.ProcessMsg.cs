using System;
using System.Globalization;
using SystemModule;

namespace M2Server
{
    public partial class TPlayObject
    {
        public override void ProcessSayMsg(string sData)
        {
            bool boDisableSayMsg;
            var sC = string.Empty;
            var sCryCryMsg = string.Empty;
            var sParam1 = string.Empty;
            const string sExceptionMsg = "[Exception] TPlayObject.ProcessSayMsg Msg = {0}";
            try
            {
                if (sData.Length > M2Share.g_Config.nSayMsgMaxLen)
                {
                    sData = sData.Substring(0, M2Share.g_Config.nSayMsgMaxLen); // 3 * 1000
                }
                if (HUtil32.GetTickCount() - m_dwSayMsgTick < M2Share.g_Config.dwSayMsgTime)
                {
                    m_nSayMsgCount++;// 2
                    if (m_nSayMsgCount >= M2Share.g_Config.nSayMsgCount)
                    {
                        m_boDisableSayMsg = true;
                        m_dwDisableSayMsgTick = HUtil32.GetTickCount() + M2Share.g_Config.dwDisableSayMsgTime;// 60 * 1000
                        SysMsg(format(M2Share.g_sDisableSayMsg, new[] { M2Share.g_Config.dwDisableSayMsgTime / (60 * 1000) }), TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                }
                else
                {
                    m_dwSayMsgTick = HUtil32.GetTickCount();
                    m_nSayMsgCount = 0;
                }
                if (HUtil32.GetTickCount() >= m_dwDisableSayMsgTick)
                {
                    m_boDisableSayMsg = false;
                }
                boDisableSayMsg = m_boDisableSayMsg;
                if (M2Share.g_DenySayMsgList.ContainsKey(this.m_sCharName))
                {
                    boDisableSayMsg = true;
                }
                if (!(boDisableSayMsg || m_PEnvir.Flag.boNOCHAT))
                {
                    M2Share.g_ChatLoggingList.Add('[' + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "] " + m_sCharName + ": " + sData);
                    m_sOldSayMsg = sData;
                    if (sData[0] == '/')
                    {
                        sC = sData.Substring(1, sData.Length - 1);
                        //if (string.Compare(sC.Trim(), M2Share.g_GameCommand.WHO.sCmd.Trim(), StringComparison.OrdinalIgnoreCase) == 0)
                        //{
                        //    if (m_btPermission < M2Share.g_GameCommand.WHO.nPerMissionMin)
                        //    {
                        //        SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                        //        return;
                        //    }
                        //    HearMsg(format(M2Share.g_sOnlineCountMsg, M2Share.UserEngine.PlayObjectCount));
                        //    return;
                        //}
                        //if (string.Compare(sC.Trim(), M2Share.g_GameCommand.TOTAL.sCmd.Trim(), StringComparison.OrdinalIgnoreCase) == 0) //统计在线人数
                        //{
                        //    if (m_btPermission < M2Share.g_GameCommand.TOTAL.nPerMissionMin)
                        //    {
                        //        SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                        //        return;
                        //    }
                        //    HearMsg(format(M2Share.g_sTotalOnlineCountMsg, M2Share.g_nTotalHumCount));
                        //    return;
                        //}
                        sC = HUtil32.GetValidStr3(sC, ref sParam1, new[] { " " });
                        if (!m_boFilterSendMsg)
                        {
                            Whisper(sParam1, sC);
                        }
                        return;
                    }
                    if (sData[0] == '!')
                    {
                        if (sData.Length >= 1)
                        {
                            if (sData[1] == '!') //发送组队消息
                            {
                                sC = sData.Substring(3 - 1, sData.Length - 2);
                                SendGroupText(m_sCharName + ": " + sC);
                                M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_208, M2Share.nServerIndex, m_sCharName + "/:" + sC);
                                return;
                            }
                            if (sData[1] == '~') //发送行会消息
                            {
                                if (m_MyGuild != null)
                                {
                                    sC = sData.Substring(2, sData.Length - 2);
                                    m_MyGuild.SendGuildMsg(m_sCharName + ": " + sC);
                                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_208, M2Share.nServerIndex, m_MyGuild.sGuildName + '/' + m_sCharName + '/' + sC);
                                }
                                return;
                            }
                        }
                        if (!m_PEnvir.Flag.boQUIZ)
                        {
                            if (HUtil32.GetTickCount() - m_dwShoutMsgTick > 10 * 1000)
                            {
                                if (m_Abil.Level <= M2Share.g_Config.nCanShoutMsgLevel)
                                {
                                    SysMsg(format(M2Share.g_sYouNeedLevelMsg, M2Share.g_Config.nCanShoutMsgLevel + 1), TMsgColor.c_Red, TMsgType.t_Hint);
                                    return;
                                }
                                m_dwShoutMsgTick = HUtil32.GetTickCount();
                                sC = sData.Substring(1, sData.Length - 1);
                                sCryCryMsg = "(!)" + m_sCharName + ": " + sC;
                                if (m_boFilterSendMsg)
                                {
                                    SendMsg(null, Grobal2.RM_CRY, 0, 0, 0xFFFF, 0, sCryCryMsg);
                                }
                                else
                                {
                                    M2Share.UserEngine.CryCry(Grobal2.RM_CRY, m_PEnvir, m_nCurrX, m_nCurrY, 50, M2Share.g_Config.btCryMsgFColor, M2Share.g_Config.btCryMsgBColor, sCryCryMsg);
                                }
                                return;
                            }
                            SysMsg(format(M2Share.g_sYouCanSendCyCyLaterMsg, new[] { 10 - (HUtil32.GetTickCount() - m_dwShoutMsgTick) / 1000 }), TMsgColor.c_Red, TMsgType.t_Hint);
                            return;
                        }
                        SysMsg(M2Share.g_sThisMapDisableSendCyCyMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                    if (m_boFilterSendMsg)
                    {
                        SendMsg(this, Grobal2.RM_HEAR, 0, M2Share.g_Config.btHearMsgFColor, M2Share.g_Config.btHearMsgBColor, 0, m_sCharName + ':' + sData);// 如果禁止发信息，则只向自己发信息
                    }
                    else
                    {
                        base.ProcessSayMsg(sData);
                    }
                    return;
                }
                SysMsg(M2Share.g_sYouIsDisableSendMsg, TMsgColor.c_Red, TMsgType.t_Hint);
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(format(sExceptionMsg, sData));
                M2Share.ErrorMessage(e.StackTrace);
            }
        }

        internal void ProcessUserLineMsg(string sData)
        {
            string sC;
            var sCMD = string.Empty;
            var sParam1 = string.Empty;
            var sParam2 = string.Empty;
            var sParam3 = string.Empty;
            var sParam4 = string.Empty;
            var sParam5 = string.Empty;
            var sParam6 = string.Empty;
            var sParam7 = string.Empty;
            TPlayObject PlayObject;
            int nFlag;
            int nValue;
            int nLen;
            const string sExceptionMsg = "[Exception] TPlayObject::ProcessUserLineMsg Msg = {0}";
            try
            {
                nLen = sData.Length;
                if (nLen <= 0)
                {
                    return;
                }
                if (m_boSetStoragePwd)
                {
                    m_boSetStoragePwd = false;
                    if (nLen > 3 && nLen < 8)
                    {
                        m_sTempPwd = sData;
                        m_boReConfigPwd = true;
                        SysMsg(M2Share.g_sReSetPasswordMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                        SendMsg(this, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                    }
                    else
                    {
                        SysMsg(M2Share.g_sPasswordOverLongMsg, TMsgColor.c_Red, TMsgType.t_Hint);// '输入的密码长度不正确！！！，密码长度必须在 4 - 7 的范围内，请重新设置密码。'
                    }
                    return;
                }
                if (m_boReConfigPwd)
                {
                    m_boReConfigPwd = false;
                    if (string.Compare(m_sTempPwd, sData, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        m_sStoragePwd = sData;
                        m_boPasswordLocked = true;
                        m_boCanGetBackItem = false;
                        m_sTempPwd = "";
                        SysMsg(M2Share.g_sReSetPasswordOKMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
                    }
                    else
                    {
                        m_sTempPwd = "";
                        SysMsg(M2Share.g_sReSetPasswordNotMatchMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    return;
                }
                if (m_boUnLockPwd || m_boUnLockStoragePwd)
                {
                    if (string.Compare(m_sStoragePwd, sData, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        m_boPasswordLocked = false;
                        if (m_boUnLockPwd)
                        {
                            if (M2Share.g_Config.boLockDealAction)
                            {
                                m_boCanDeal = true;
                            }
                            if (M2Share.g_Config.boLockDropAction)
                            {
                                m_boCanDrop = true;
                            }
                            if (M2Share.g_Config.boLockWalkAction)
                            {
                                m_boCanWalk = true;
                            }
                            if (M2Share.g_Config.boLockRunAction)
                            {
                                m_boCanRun = true;
                            }
                            if (M2Share.g_Config.boLockHitAction)
                            {
                                m_boCanHit = true;
                            }
                            if (M2Share.g_Config.boLockSpellAction)
                            {
                                m_boCanSpell = true;
                            }
                            if (M2Share.g_Config.boLockSendMsgAction)
                            {
                                m_boCanSendMsg = true;
                            }
                            if (M2Share.g_Config.boLockUserItemAction)
                            {
                                m_boCanUseItem = true;
                            }
                            if (M2Share.g_Config.boLockInObModeAction)
                            {
                                m_boObMode = false;
                                m_boAdminMode = false;
                            }
                            m_boLockLogoned = true;
                            SysMsg(M2Share.g_sPasswordUnLockOKMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
                        }
                        if (m_boUnLockStoragePwd)
                        {
                            if (M2Share.g_Config.boLockGetBackItemAction)
                            {
                                m_boCanGetBackItem = true;
                            }
                            SysMsg(M2Share.g_sStorageUnLockOKMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        m_btPwdFailCount++;
                        SysMsg(M2Share.g_sUnLockPasswordFailMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        if (m_btPwdFailCount > 3)
                        {
                            SysMsg(M2Share.g_sStoragePasswordLockedMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                    m_boUnLockPwd = false;
                    m_boUnLockStoragePwd = false;
                    return;
                }
                if (m_boCheckOldPwd)
                {
                    m_boCheckOldPwd = false;
                    if (m_sStoragePwd == sData)
                    {
                        SendMsg(this, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                        SysMsg(M2Share.g_sSetPasswordMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                        m_boSetStoragePwd = true;
                    }
                    else
                    {
                        m_btPwdFailCount++;
                        SysMsg(M2Share.g_sOldPasswordIncorrectMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        if (m_btPwdFailCount > 3)
                        {
                            SysMsg(M2Share.g_sStoragePasswordLockedMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                            m_boPasswordLocked = true;
                        }
                    }
                    return;
                }
                if (!sData.StartsWith("@"))
                {
                    ProcessSayMsg(sData);
                    return;
                }
                sC = sData.Substring(1, sData.Length - 1);
                sC = HUtil32.GetValidStr3(sC, ref sCMD, new[] { " ", ":", ",", "\t" });
                if (sC != "")
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam1, new[] { " ", ":", ",", "\t" });
                }
                if (sC != "")
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam2, new[] { " ", ":", ",", "\t" });
                }
                if (sC != "")
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam3, new[] { " ", ":", ",", "\t" });
                }
                if (sC != "")
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam4, new[] { " ", ":", ",", "\t" });
                }
                if (sC != "")
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam5, new[] { " ", ":", ",", "\t" });
                }
                if (sC != "")
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam6, new[] { " ", ":", ",", "\t" });
                }
                if (sC != "")
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam7, new[] { " ", ":", ",", "\t" });
                }
                // 新密码命令
                if (string.Compare(sCMD, M2Share.g_GameCommand.PASSWORDLOCK.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (!M2Share.g_Config.boPasswordLockSystem)
                    {
                        SysMsg(M2Share.g_sNoPasswordLockSystemMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                    if (m_sStoragePwd == "")
                    {
                        SendMsg(this, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                        m_boSetStoragePwd = true;
                        SysMsg(M2Share.g_sSetPasswordMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                        return;
                    }
                    if (m_btPwdFailCount > 3)
                    {
                        SysMsg(M2Share.g_sStoragePasswordLockedMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        m_boPasswordLocked = true;
                        return;
                    }
                    if (m_sStoragePwd != "")
                    {
                        SendMsg(this, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                        m_boCheckOldPwd = true;
                        SysMsg(M2Share.g_sPleaseInputOldPasswordMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                        return;
                    }
                    return;
                }
                if (M2Share.CommandSystem.ExecCmd(sData, this))
                {
                    return;
                }
                // 新密码命令
                if (string.Compare(sCMD, M2Share.g_GameCommand.SETPASSWORD.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (!M2Share.g_Config.boPasswordLockSystem)
                    {
                        SysMsg(M2Share.g_sNoPasswordLockSystemMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                    if (m_sStoragePwd == "")
                    {
                        SendMsg(this, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                        m_boSetStoragePwd = true;
                        SysMsg(M2Share.g_sSetPasswordMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    else
                    {
                        SysMsg(M2Share.g_sAlreadySetPasswordMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.UNPASSWORD.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (!M2Share.g_Config.boPasswordLockSystem)
                    {
                        SysMsg(M2Share.g_sNoPasswordLockSystemMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                    if (!m_boPasswordLocked)
                    {
                        m_sStoragePwd = "";
                        SysMsg(M2Share.g_sOldPasswordIsClearMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    else
                    {
                        SysMsg(M2Share.g_sPleaseUnLockPasswordMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.CHGPASSWORD.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (!M2Share.g_Config.boPasswordLockSystem)
                    {
                        SysMsg(M2Share.g_sNoPasswordLockSystemMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                    if (m_btPwdFailCount > 3)
                    {
                        SysMsg(M2Share.g_sStoragePasswordLockedMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        m_boPasswordLocked = true;
                        return;
                    }
                    if (m_sStoragePwd != "")
                    {
                        SendMsg(this, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                        m_boCheckOldPwd = true;
                        SysMsg(M2Share.g_sPleaseInputOldPasswordMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    else
                    {
                        SysMsg(M2Share.g_sNoPasswordSetMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.UNLOCKSTORAGE.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (!M2Share.g_Config.boPasswordLockSystem)
                    {
                        SysMsg(M2Share.g_sNoPasswordLockSystemMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                    if (m_btPwdFailCount > M2Share.g_Config.nPasswordErrorCountLock)
                    {
                        SysMsg(M2Share.g_sStoragePasswordLockedMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        m_boPasswordLocked = true;
                        return;
                    }
                    if (m_sStoragePwd != "")
                    {
                        if (!m_boUnLockStoragePwd)
                        {
                            SendMsg(this, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                            SysMsg(M2Share.g_sPleaseInputUnLockPasswordMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                            m_boUnLockStoragePwd = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sStorageAlreadyUnLockMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        SysMsg(M2Share.g_sStorageNoPasswordMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.UNLOCK.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (!M2Share.g_Config.boPasswordLockSystem)
                    {
                        SysMsg(M2Share.g_sNoPasswordLockSystemMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                    if (m_btPwdFailCount > M2Share.g_Config.nPasswordErrorCountLock)
                    {
                        SysMsg(M2Share.g_sStoragePasswordLockedMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        m_boPasswordLocked = true;
                        return;
                    }
                    if (m_sStoragePwd != "")
                    {
                        if (!m_boUnLockPwd)
                        {
                            SendMsg(this, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                            SysMsg(M2Share.g_sPleaseInputUnLockPasswordMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                            m_boUnLockPwd = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sStorageAlreadyUnLockMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        SysMsg(M2Share.g_sStorageNoPasswordMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.__LOCK.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (!M2Share.g_Config.boPasswordLockSystem)
                    {
                        SysMsg(M2Share.g_sNoPasswordLockSystemMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                    if (!m_boPasswordLocked)
                    {
                        if (m_sStoragePwd != "")
                        {
                            m_boPasswordLocked = true;
                            m_boCanGetBackItem = false;
                            SysMsg(M2Share.g_sLockStorageSuccessMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                        else
                        {
                            SysMsg(M2Share.g_sStorageNoPasswordMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        SysMsg(M2Share.g_sStorageAlreadyLockMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DEAR.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    CmdSearchDear(M2Share.g_GameCommand.DEAR.sCmd, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.MASTER.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    CmdSearchMaster(M2Share.g_GameCommand.MASTER.sCmd, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.ALLOWDEARRCALL.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_boCanDearRecall = !m_boCanDearRecall;
                    if (m_boCanDearRecall)
                    {
                        // '允许夫妻传送！！！'
                        SysMsg(M2Share.g_sEnableDearRecall, TMsgColor.c_Blue, TMsgType.t_Hint);
                    }
                    else
                    {
                        // '禁止夫妻传送！！！'
                        SysMsg(M2Share.g_sDisableDearRecall, TMsgColor.c_Blue, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.ALLOWMASTERRECALL.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_boCanMasterRecall = !m_boCanMasterRecall;
                    if (m_boCanMasterRecall)
                    {
                        // '允许师徒传送！！！'
                        SysMsg(M2Share.g_sEnableMasterRecall, TMsgColor.c_Blue, TMsgType.t_Hint);
                    }
                    else
                    {
                        // '禁止师徒传送！！！'
                        SysMsg(M2Share.g_sDisableMasterRecall, TMsgColor.c_Blue, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DATA.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // '当前日期时间: '
                    SysMsg(M2Share.g_sNowCurrDateTime + DateTime.Now.ToString("dddddd,dddd,hh:mm:nn"), TMsgColor.c_Blue, TMsgType.t_Hint);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.PRVMSG.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    CmdPrvMsg(M2Share.g_GameCommand.PRVMSG.sCmd, M2Share.g_GameCommand.PRVMSG.nPerMissionMin, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.ALLOWMSG.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_boHearWhisper = !m_boHearWhisper;
                    if (m_boHearWhisper)
                    {
                        // '[允许私聊]'
                        SysMsg(M2Share.g_sEnableHearWhisper, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    else
                    {
                        // '[禁止私聊]'
                        SysMsg(M2Share.g_sDisableHearWhisper, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.LETSHOUT.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_boBanShout = !m_boBanShout;
                    if (m_boBanShout)
                    {
                        // '[允许群聊]'
                        SysMsg(M2Share.g_sEnableShoutMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    else
                    {
                        // '[禁止群聊]'
                        SysMsg(M2Share.g_sDisableShoutMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.LETTRADE.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_boAllowDeal = !m_boAllowDeal;
                    if (m_boAllowDeal)
                    {
                        // '[允许交易]'
                        SysMsg(M2Share.g_sEnableDealMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    else
                    {
                        // '[禁止交易]'
                        SysMsg(M2Share.g_sDisableDealMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.BANGUILDCHAT.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_boBanGuildChat = !m_boBanGuildChat;
                    if (m_boBanGuildChat)
                    {
                        // '[允许行会聊天]'
                        SysMsg(M2Share.g_sEnableGuildChat, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    else
                    {
                        // '[禁止行会聊天]'
                        SysMsg(M2Share.g_sDisableGuildChat, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.LETGUILD.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_boAllowGuild = !m_boAllowGuild;
                    if (m_boAllowGuild)
                    {
                        // '[允许加入行会]'
                        SysMsg(M2Share.g_sEnableJoinGuild, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    else
                    {
                        // '[禁止加入行会]'
                        SysMsg(M2Share.g_sDisableJoinGuild, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.ENDGUILD.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    CmdEndGuild();
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.AUTHALLY.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (IsGuildMaster())
                    {
                        m_MyGuild.m_boEnableAuthAlly = !m_MyGuild.m_boEnableAuthAlly;
                        if (m_MyGuild.m_boEnableAuthAlly)
                        {
                            // '[允许行会联盟]'
                            SysMsg(M2Share.g_sEnableAuthAllyGuild, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                        else
                        {
                            // '[禁止行会联盟]'
                            SysMsg(M2Share.g_sDisableAuthAllyGuild, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.ALLOWGUILDRECALL.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_boAllowGuildReCall = !m_boAllowGuildReCall;
                    if (m_boAllowGuildReCall)
                    {
                        // '[允许行会合一]'
                        SysMsg(M2Share.g_sEnableGuildRecall, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    else
                    {
                        // '[禁止行会合一]'
                        SysMsg(M2Share.g_sDisableGuildRecall, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.AUTH.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (IsGuildMaster())
                    {
                        ClientGuildAlly();
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.AUTHCANCEL.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (IsGuildMaster())
                    {
                        ClientGuildBreakAlly(sParam1);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.TAKEONHORSE.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    CmdTakeOnHorse(sCMD, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.TAKEOFHORSE.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    CmdTakeOffHorse(sCMD, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.MAPINFO.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    ShowMapInfo(sParam1, sParam2, sParam3);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.CLEARBAG.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    CmdClearBagItem(M2Share.g_GameCommand.CLEARBAG, sParam1);
                    return;
                }
                //if ((sCMD).CompareTo((M2Share.g_GameCommand.SHOWUSEITEMINFO.sCmd), StringComparison.OrdinalIgnoreCase) == 0)
                //{
                //    CmdShowUseItemInfo(M2Share.g_GameCommand.SHOWUSEITEMINFO, sParam1);
                //    return;
                //}
                //if ((sCMD).CompareTo((M2Share.g_GameCommand.BINDUSEITEM.sCmd), StringComparison.OrdinalIgnoreCase) == 0)
                //{
                //    CmdBindUseItem(M2Share.g_GameCommand.BINDUSEITEM, sParam1, sParam2, sParam3);
                //    return;
                //}
                if (string.Compare(sCMD, M2Share.g_GameCommand.SEARCHING.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    CmdSearchHuman(M2Share.g_GameCommand.SEARCHING.sCmd, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.LOCKLOGON.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    CmdLockLogin(M2Share.g_GameCommand.LOCKLOGON);
                    return;
                }
                if (m_btPermission >= 2 && sData.Length > 2)
                {
                    if (m_btPermission >= 6 && sData[2] == M2Share.g_GMRedMsgCmd)
                    {
                        if (HUtil32.GetTickCount() - m_dwSayMsgTick > 2000)
                        {
                            m_dwSayMsgTick = HUtil32.GetTickCount();
                            sData = sData.Substring(3 - 1, sData.Length - 2);
                            if (sData.Length > M2Share.g_Config.nSayRedMsgMaxLen)
                            {
                                sData = sData.Substring(0, M2Share.g_Config.nSayRedMsgMaxLen);
                            }
                            if (M2Share.g_Config.boShutRedMsgShowGMName)
                            {
                                sC = m_sCharName + ": " + sData;
                            }
                            else
                            {
                                sC = sData;
                            }
                            M2Share.UserEngine.SendBroadCastMsg(sC, TMsgType.t_GM);
                        }
                        return;
                    }
                }
                if (m_btPermission > 4)
                {
                    if (string.Compare(sCMD, M2Share.g_GameCommand.BACKSTEP.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        CmdBackStep(sCMD, HUtil32.Str_ToInt(sParam1, 0), HUtil32.Str_ToInt(sParam2, 1));
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.BALL.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        // 精神波
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.CHANGELUCK.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.HUNGER.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        CmdHunger(M2Share.g_GameCommand.HUNGER.sCmd, sParam1, HUtil32.Str_ToInt(sParam2, 0));
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.NAMECOLOR.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.TRANSPARECY.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.LEVEL0.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.SETFLAG.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        PlayObject = M2Share.UserEngine.GetPlayObject(sParam1);
                        if (PlayObject != null)
                        {
                            nFlag = HUtil32.Str_ToInt(sParam2, 0);
                            nValue = HUtil32.Str_ToInt(sParam3, 0);
                            PlayObject.SetQuestFlagStatus(nFlag, nValue);
                            if (PlayObject.GetQuestFalgStatus(nFlag) == 1)
                            {
                                SysMsg(PlayObject.m_sCharName + ": [" + nFlag + "] = ON", TMsgColor.c_Green, TMsgType.t_Hint);
                            }
                            else
                            {
                                SysMsg(PlayObject.m_sCharName + ": [" + nFlag + "] = OFF", TMsgColor.c_Green, TMsgType.t_Hint);
                            }
                        }
                        else
                        {
                            SysMsg('@' + M2Share.g_GameCommand.SETFLAG.sCmd + " 人物名称 标志号 数字(0 - 1)", TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.SETOPEN.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        PlayObject = M2Share.UserEngine.GetPlayObject(sParam1);
                        if (PlayObject != null)
                        {
                            nFlag = HUtil32.Str_ToInt(sParam2, 0);
                            nValue = HUtil32.Str_ToInt(sParam3, 0);
                            PlayObject.SetQuestUnitOpenStatus(nFlag, nValue);
                            if (PlayObject.GetQuestUnitOpenStatus(nFlag) == 1)
                            {
                                SysMsg(PlayObject.m_sCharName + ": [" + nFlag + "] = ON", TMsgColor.c_Green, TMsgType.t_Hint);
                            }
                            else
                            {
                                SysMsg(PlayObject.m_sCharName + ": [" + nFlag + "] = OFF", TMsgColor.c_Green, TMsgType.t_Hint);
                            }
                        }
                        else
                        {
                            SysMsg('@' + M2Share.g_GameCommand.SETOPEN.sCmd + " 人物名称 标志号 数字(0 - 1)", TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.SETUNIT.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        PlayObject = M2Share.UserEngine.GetPlayObject(sParam1);
                        if (PlayObject != null)
                        {
                            nFlag = HUtil32.Str_ToInt(sParam2, 0);
                            nValue = HUtil32.Str_ToInt(sParam3, 0);
                            PlayObject.SetQuestUnitStatus(nFlag, nValue);
                            if (PlayObject.GetQuestUnitStatus(nFlag) == 1)
                            {
                                SysMsg(PlayObject.m_sCharName + ": [" + nFlag + "] = ON", TMsgColor.c_Green, TMsgType.t_Hint);
                            }
                            else
                            {
                                SysMsg(PlayObject.m_sCharName + ": [" + nFlag + "] = OFF", TMsgColor.c_Green, TMsgType.t_Hint);
                            }
                        }
                        else
                        {
                            SysMsg('@' + M2Share.g_GameCommand.SETUNIT.sCmd + " 人物名称 标志号 数字(0 - 1)", TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.DISABLEFILTER.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        CmdDisableFilter(sCMD, sParam1);
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.OXQUIZROOM.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.GSA.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.CHANGEITEMNAME.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        CmdChangeItemName(M2Share.g_GameCommand.CHANGEITEMNAME.sCmd, sParam1, sParam2, sParam3);
                        return;
                    }
                    if (m_btPermission >= 5 || M2Share.g_Config.boTestServer)
                    {
                        if (string.Compare(sCMD, M2Share.g_GameCommand.FIREBURN.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            //CmdFireBurn(HUtil32.Str_ToInt(sParam1, 0), HUtil32.Str_ToInt(sParam2, 0), HUtil32.Str_ToInt(sParam3, 0));
                            return;
                        }
                        //if ((sCMD).CompareTo((M2Share.g_GameCommand.TESTFIRE.sCmd), StringComparison.OrdinalIgnoreCase) == 0)
                        //{
                        //    //CmdTestFire(sCMD, HUtil32.Str_ToInt(sParam1, 0), HUtil32.Str_ToInt(sParam2, 0), HUtil32.Str_ToInt(sParam3, 0), HUtil32.Str_ToInt(sParam4, 0));
                        //    return;
                        //}
                        if (string.Compare(sCMD, M2Share.g_GameCommand.TESTSTATUS.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            CmdTestStatus(sCMD, HUtil32.Str_ToInt(sParam1, -1), HUtil32.Str_ToInt(sParam2, 0));
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.DELGAMEGOLD.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            CmdDelGameGold(M2Share.g_GameCommand.DELGAMEGOLD.sCmd, sParam1, HUtil32.Str_ToInt(sParam2, 0));
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ADDGAMEGOLD.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            CmdAddGameGold(M2Share.g_GameCommand.ADDGAMEGOLD.sCmd, sParam1, HUtil32.Str_ToInt(sParam2, 0));
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.TESTGOLDCHANGE.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADADMIN.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            CmdReLoadAdmin(M2Share.g_GameCommand.RELOADADMIN.sCmd);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADNPC.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            CmdReloadNpc(sParam1);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADMANAGE.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            CmdReloadManage(M2Share.g_GameCommand.RELOADMANAGE, sParam1);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADROBOTMANAGE.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            CmdReloadRobotManage();
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADROBOT.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            CmdReloadRobot();
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADMONITEMS.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            CmdReloadMonItems();
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADDIARY.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADITEMDB.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            M2Share.LocalDB.LoadItemsDB();
                            SysMsg("物品数据库重新加载完成。", TMsgColor.c_Green, TMsgType.t_Hint);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADMAGICDB.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            // FrmDB.LoadMagicDB();
                            // SysMsg('魔法数据库重新加载完成。',c_Green,t_Hint);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADMONSTERDB.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            M2Share.LocalDB.LoadMonsterDB();
                            SysMsg("怪物数据库重新加载完成。", TMsgColor.c_Green, TMsgType.t_Hint);
                            return;
                        }
                        //if ((sCMD).CompareTo((M2Share.g_GameCommand.RELOADMINMAP.sCmd), StringComparison.OrdinalIgnoreCase) == 0)
                        //{
                        //    // FrmDB.LoadMinMap();
                        //    // g_MapManager.ReSetMinMap();
                        //    SysMsg("小地图配置重新加载完成。", TMsgColor.c_Green, TMsgType.t_Hint);
                        //    return;
                        //}
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ADDGUILD.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            CmdAddGuild(M2Share.g_GameCommand.ADDGUILD, sParam1, sParam2);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.DELGUILD.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            CmdDelGuild(M2Share.g_GameCommand.DELGUILD, sParam1);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.CHANGESABUKLORD.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            CmdChangeSabukLord(M2Share.g_GameCommand.CHANGESABUKLORD, sParam1, sParam2, true);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            CmdForcedWallconquestWar(M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR, sParam1);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ADDTOITEMEVENT.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ADDTOITEMEVENTASPIECES.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ITEMEVENTLIST.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.STARTINGGIFTNO.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.DELETEALLITEMEVENT.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.STARTITEMEVENT.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ITEMEVENTTERM.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ADJUESTTESTLEVEL.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.OPDELETESKILL.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.CHANGEWEAPONDURA.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADGUILDALL.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.SPIRIT.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            CmdSpirtStart(M2Share.g_GameCommand.SPIRIT.sCmd, sParam1);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.SPIRITSTOP.sCmd, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            CmdSpirtStop(M2Share.g_GameCommand.SPIRITSTOP.sCmd, sParam1);
                            return;
                        }
                        //else if ((sCMD).CompareTo((M2Share.g_GameCommand.TESTSERVERCONFIG.sCmd), StringComparison.OrdinalIgnoreCase) == 0)
                        //{
                        //    SendServerConfig();
                        //    return;
                        //}
                        //else if ((sCMD).CompareTo((M2Share.g_GameCommand.SERVERSTATUS.sCmd), StringComparison.OrdinalIgnoreCase) == 0)
                        //{
                        //    SendServerStatus();
                        //    return;
                        //}
                        //else if ((sCMD).CompareTo((M2Share.g_GameCommand.TESTGETBAGITEM.sCmd), StringComparison.OrdinalIgnoreCase) == 0)
                        //{
                        //    CmdTestGetBagItems(M2Share.g_GameCommand.TESTGETBAGITEM, sParam1);
                        //    return;
                        //}
                        //else if ((sCMD).CompareTo((M2Share.g_GameCommand.MOBFIREBURN.sCmd), StringComparison.OrdinalIgnoreCase) == 0)
                        //{
                        //    CmdMobFireBurn(M2Share.g_GameCommand.MOBFIREBURN, sParam1, sParam2, sParam3, sParam4, sParam5, sParam6);
                        //    return;
                        //}
                        //else if ((sCMD).CompareTo((M2Share.g_GameCommand.TESTSPEEDMODE.sCmd), StringComparison.OrdinalIgnoreCase) == 0)
                        //{
                        //    CmdTestSpeedMode(M2Share.g_GameCommand.TESTSPEEDMODE);
                        //    return;
                        //}
                    }
                }
                SysMsg('@' + sCMD + " 此命令不正确，或没有足够的权限！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(format(sExceptionMsg, sData));
                M2Share.ErrorMessage(e.Message);
            }
        }
    }
}