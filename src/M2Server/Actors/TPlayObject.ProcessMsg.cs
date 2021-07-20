using System;
using System.Globalization;

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
                        SysMsg(format(M2Share.g_sDisableSayMsg, new [] { M2Share.g_Config.dwDisableSayMsgTime / (60 * 1000) }), TMsgColor.c_Red, TMsgType.t_Hint);
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
                var value = 0;
                if (M2Share.g_DenySayMsgList.TryGetValue(this.m_sCharName.GetHashCode(), out value))
                {
                    boDisableSayMsg = true;
                }
                if (!(boDisableSayMsg || m_PEnvir.Flag.boNOCHAT))
                {
                    M2Share.g_ChatLoggingList.Add('[' + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "] " + m_sCharName + ": " + sData);
                    m_sOldSayMsg = sData;
                    if (sData[1] == '/')
                    {
                        sC = sData.Substring(2 - 1, sData.Length - 1);
                        if (string.Compare(sC.Trim(), M2Share.g_GameCommand.WHO.sCmd.Trim(), StringComparison.Ordinal) == 0)
                        {
                            if (m_btPermission < M2Share.g_GameCommand.WHO.nPerMissionMin)
                            {
                                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                                return;
                            }
                            HearMsg(format(M2Share.g_sOnlineCountMsg, M2Share.UserEngine.PlayObjectCount));
                            return;
                        }
                        if (string.Compare(sC.Trim(), M2Share.g_GameCommand.TOTAL.sCmd.Trim(), StringComparison.Ordinal) == 0)
                        {
                            if (m_btPermission < M2Share.g_GameCommand.TOTAL.nPerMissionMin)
                            {
                                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                                return;
                            }
                            HearMsg(format(M2Share.g_sTotalOnlineCountMsg, M2Share.g_nTotalHumCount));
                            return;
                        }
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
                            if (sData[1] == '!')
                            {
                                sC = sData.Substring(3 - 1, sData.Length - 2);
                                SendGroupText(m_sCharName + ": " + sC);
                                return;
                            }
                            if (sData[1] == '~')
                            {
                                if (m_MyGuild != null)
                                {
                                    sC = sData.Substring(2, sData.Length - 2);
                                    m_MyGuild.SendGuildMsg(m_sCharName + ": " + sC);
                                    M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_208, M2Share.nServerIndex, m_MyGuild.sGuildName + '/' + m_sCharName + '/' + sC);
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
                                    SendMsg(null, grobal2.RM_CRY, 0, 0, 0xFFFF, 0, sCryCryMsg);
                                }
                                else
                                {
                                    M2Share.UserEngine.CryCry(grobal2.RM_CRY, m_PEnvir, m_nCurrX, m_nCurrY, 50, M2Share.g_Config.btCryMsgFColor, M2Share.g_Config.btCryMsgBColor, sCryCryMsg);
                                }
                                return;
                            }
                            SysMsg(format(M2Share.g_sYouCanSendCyCyLaterMsg, new [] { 10 - (HUtil32.GetTickCount() - m_dwShoutMsgTick) / 1000 }), TMsgColor.c_Red, TMsgType.t_Hint);
                            return;
                        }
                        SysMsg(M2Share.g_sThisMapDisableSendCyCyMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                    if (m_boFilterSendMsg)
                    {
                        SendMsg(this, grobal2.RM_HEAR, 0, M2Share.g_Config.btHearMsgFColor, M2Share.g_Config.btHearMsgBColor, 0, m_sCharName + ':' + sData);// 如果禁止发信息，则只向自己发信息
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
                        SendMsg(this, grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
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
                    if (string.Compare(m_sTempPwd, sData, StringComparison.Ordinal) == 0)
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
                    if (string.Compare(m_sStoragePwd, sData, StringComparison.Ordinal) == 0)
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
                        SendMsg(this, grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.PASSWORDLOCK.sCmd, StringComparison.Ordinal) == 0)
                {
                    if (!M2Share.g_Config.boPasswordLockSystem)
                    {
                        SysMsg(M2Share.g_sNoPasswordLockSystemMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                    if (m_sStoragePwd == "")
                    {
                        SendMsg(this, grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
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
                        SendMsg(this, grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.SETPASSWORD.sCmd, StringComparison.Ordinal) == 0)
                {
                    if (!M2Share.g_Config.boPasswordLockSystem)
                    {
                        SysMsg(M2Share.g_sNoPasswordLockSystemMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                    if (m_sStoragePwd == "")
                    {
                        SendMsg(this, grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                        m_boSetStoragePwd = true;
                        SysMsg(M2Share.g_sSetPasswordMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    else
                    {
                        SysMsg(M2Share.g_sAlreadySetPasswordMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.UNPASSWORD.sCmd, StringComparison.Ordinal) == 0)
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.CHGPASSWORD.sCmd, StringComparison.Ordinal) == 0)
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
                        SendMsg(this, grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                        m_boCheckOldPwd = true;
                        SysMsg(M2Share.g_sPleaseInputOldPasswordMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                    else
                    {
                        SysMsg(M2Share.g_sNoPasswordSetMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.UNLOCKSTORAGE.sCmd, StringComparison.Ordinal) == 0)
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
                            SendMsg(this, grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.UNLOCK.sCmd, StringComparison.Ordinal) == 0)
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
                            SendMsg(this, grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.__LOCK.sCmd, StringComparison.Ordinal) == 0)
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.MEMBERFUNCTION.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdMemberFunction(M2Share.g_GameCommand.MEMBERFUNCTION.sCmd, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.MEMBERFUNCTIONEX.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdMemberFunctionEx(M2Share.g_GameCommand.MEMBERFUNCTIONEX.sCmd, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DEAR.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdSearchDear(M2Share.g_GameCommand.DEAR.sCmd, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.MASTER.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdSearchMaster(M2Share.g_GameCommand.MASTER.sCmd, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.MASTERECALL.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdMasterRecall(M2Share.g_GameCommand.MASTERECALL.sCmd, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DEARRECALL.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdDearRecall(M2Share.g_GameCommand.DEARRECALL.sCmd, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.ALLOWDEARRCALL.sCmd, StringComparison.Ordinal) == 0)
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.ALLOWMASTERRECALL.sCmd, StringComparison.Ordinal) == 0)
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.DATA.sCmd, StringComparison.Ordinal) == 0)
                {
                    // '当前日期时间: '
                    SysMsg(M2Share.g_sNowCurrDateTime + DateTime.Now.ToString("dddddd,dddd,hh:mm:nn"), TMsgColor.c_Blue, TMsgType.t_Hint);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.PRVMSG.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdPrvMsg(M2Share.g_GameCommand.PRVMSG.sCmd, M2Share.g_GameCommand.PRVMSG.nPerMissionMin, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.ALLOWMSG.sCmd, StringComparison.Ordinal) == 0)
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.LETSHOUT.sCmd, StringComparison.Ordinal) == 0)
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.LETTRADE.sCmd, StringComparison.Ordinal) == 0)
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.BANGUILDCHAT.sCmd, StringComparison.Ordinal) == 0)
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.LETGUILD.sCmd, StringComparison.Ordinal) == 0)
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.ENDGUILD.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdEndGuild();
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.AUTHALLY.sCmd, StringComparison.Ordinal) == 0)
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.ALLOWGROUPCALL.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdAllowGroupReCall(sCMD, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.GROUPRECALLL.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdGroupRecall(M2Share.g_GameCommand.GROUPRECALLL.sCmd);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.ALLOWGUILDRECALL.sCmd, StringComparison.Ordinal) == 0)
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.GUILDRECALLL.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdGuildRecall(M2Share.g_GameCommand.GUILDRECALLL.sCmd, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.AUTH.sCmd, StringComparison.Ordinal) == 0)
                {
                    if (IsGuildMaster())
                    {
                        ClientGuildAlly();
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.AUTHCANCEL.sCmd, StringComparison.Ordinal) == 0)
                {
                    if (IsGuildMaster())
                    {
                        ClientGuildBreakAlly(sParam1);
                    }
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DIARY.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdViewDiary(M2Share.g_GameCommand.DIARY.sCmd, HUtil32.Str_ToInt(sParam1, 0));
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.ATTACKMODE.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdChangeAttackMode(HUtil32.Str_ToInt(sParam1, -1), sParam1, sParam2, sParam3, sParam4, sParam5, sParam6, sParam7);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.REST.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdChangeSalveStatus();
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.TAKEONHORSE.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdTakeOnHorse(sCMD, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.TAKEOFHORSE.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdTakeOffHorse(sCMD, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.TESTGA.sCmd, StringComparison.Ordinal) == 0)
                {
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.MAPINFO.sCmd, StringComparison.Ordinal) == 0)
                {
                    ShowMapInfo(sParam1, sParam2, sParam3);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.CLEARBAG.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdClearBagItem(M2Share.g_GameCommand.CLEARBAG, sParam1);
                    return;
                }
                //if ((sCMD).CompareTo((M2Share.g_GameCommand.SHOWUSEITEMINFO.sCmd), StringComparison.Ordinal) == 0)
                //{
                //    CmdShowUseItemInfo(M2Share.g_GameCommand.SHOWUSEITEMINFO, sParam1);
                //    return;
                //}
                //if ((sCMD).CompareTo((M2Share.g_GameCommand.BINDUSEITEM.sCmd), StringComparison.Ordinal) == 0)
                //{
                //    CmdBindUseItem(M2Share.g_GameCommand.BINDUSEITEM, sParam1, sParam2, sParam3);
                //    return;
                //}
                if (string.Compare(sCMD, M2Share.g_GameCommand.SBKDOOR.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdSbkDoorControl(M2Share.g_GameCommand.SBKDOOR.sCmd, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.USERMOVE.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdUserMoveXY(M2Share.g_GameCommand.USERMOVE.sCmd, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SEARCHING.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdSearchHuman(M2Share.g_GameCommand.SEARCHING.sCmd, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.LOCKLOGON.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdLockLogin(M2Share.g_GameCommand.LOCKLOGON);
                    return;
                }
                if (m_btPermission >= 2 && sData.Length > 2)
                {
                    // if sData[2] = '!' then begin
                    if (m_btPermission >= 6 && sData[2] == M2Share.g_GMRedMsgCmd)
                    {
                        if (HUtil32.GetTickCount() - m_dwSayMsgTick > 2000)
                        {
                            m_dwSayMsgTick = HUtil32.GetTickCount();
                            sData = sData.Substring(3 - 1, sData.Length - 2);
                            if (sData.Length > M2Share.g_Config.nSayRedMsgMaxLen)
                            {
                                sData = sData.Substring(1 - 1, M2Share.g_Config.nSayRedMsgMaxLen);
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
                if (string.Compare(sCMD, M2Share.g_GameCommand.HUMANLOCAL.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdHumanLocal(M2Share.g_GameCommand.HUMANLOCAL, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.MOVE.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdMapMove(M2Share.g_GameCommand.MOVE, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.POSITIONMOVE.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdPositionMove(M2Share.g_GameCommand.POSITIONMOVE, sParam1, sParam2, sParam3);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.INFO.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdHumanInfo(M2Share.g_GameCommand.INFO, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.MOBLEVEL.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdMobLevel(M2Share.g_GameCommand.MOBLEVEL, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.MOBCOUNT.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdMobCount(M2Share.g_GameCommand.MOBCOUNT, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.HUMANCOUNT.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdHumanCount(M2Share.g_GameCommand.HUMANCOUNT, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.KICK.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdKickHuman(M2Share.g_GameCommand.KICK, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.TING.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdTing(M2Share.g_GameCommand.TING, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SUPERTING.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdSuperTing(M2Share.g_GameCommand.SUPERTING, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.MAPMOVE.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdMapMoveHuman(M2Share.g_GameCommand.MAPMOVE, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SHUTUP.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdShutup(M2Share.g_GameCommand.SHUTUP, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.MAP.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdShowMapInfo(M2Share.g_GameCommand.MAP, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.RELEASESHUTUP.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdShutupRelease(M2Share.g_GameCommand.RELEASESHUTUP, sParam1, true);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SHUTUPLIST.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdShutupList(M2Share.g_GameCommand.SHUTUPLIST, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.GAMEMASTER.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdChangeAdminMode(M2Share.g_GameCommand.GAMEMASTER.sCmd, M2Share.g_GameCommand.GAMEMASTER.nPerMissionMin, sParam1, !m_boAdminMode);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.OBSERVER.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdChangeObMode(M2Share.g_GameCommand.OBSERVER.sCmd, M2Share.g_GameCommand.OBSERVER.nPerMissionMin, sParam1, !m_boObMode);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SUEPRMAN.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdChangeSuperManMode(M2Share.g_GameCommand.OBSERVER.sCmd, M2Share.g_GameCommand.OBSERVER.nPerMissionMin, sParam1, !m_boSuperMan);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.LEVEL.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdChangeLevel(M2Share.g_GameCommand.LEVEL, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SABUKWALLGOLD.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdShowSbkGold(M2Share.g_GameCommand.SABUKWALLGOLD, sParam1, sParam2, sParam3);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.RECALL.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdRecallHuman(M2Share.g_GameCommand.RECALL, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.REGOTO.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdReGotoHuman(M2Share.g_GameCommand.REGOTO, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SHOWFLAG.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdShowHumanFlag(M2Share.g_GameCommand.SHOWFLAG.sCmd, M2Share.g_GameCommand.SHOWFLAG.nPerMissionMin, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SHOWOPEN.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdShowHumanUnitOpen(M2Share.g_GameCommand.SHOWOPEN.sCmd, M2Share.g_GameCommand.SHOWOPEN.nPerMissionMin, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SHOWUNIT.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdShowHumanUnit(M2Share.g_GameCommand.SHOWUNIT.sCmd, M2Share.g_GameCommand.SHOWUNIT.nPerMissionMin, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.ATTACK.sCmd, StringComparison.Ordinal) == 0)
                {
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.MOB.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdMob(M2Share.g_GameCommand.MOB, sParam1, HUtil32.Str_ToInt(sParam2, 0), HUtil32.Str_ToInt(sParam3, 0), HUtil32.Str_ToInt(sParam4, -1));
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.MOBNPC.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdMobNpc(M2Share.g_GameCommand.MOBNPC.sCmd, M2Share.g_GameCommand.MOBNPC.nPerMissionMin, sParam1, sParam2, sParam3, sParam4);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.NPCSCRIPT.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdNpcScript(M2Share.g_GameCommand.NPCSCRIPT.sCmd, M2Share.g_GameCommand.NPCSCRIPT.nPerMissionMin, sParam1, sParam2, sParam3);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DELNPC.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdDelNpc(M2Share.g_GameCommand.DELNPC.sCmd, M2Share.g_GameCommand.DELNPC.nPerMissionMin, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.RECALLMOB.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdRecallMob(M2Share.g_GameCommand.RECALLMOB, sParam1, HUtil32.Str_ToInt(sParam2, 0), HUtil32.Str_ToInt(sParam3, 0), HUtil32.Str_ToInt(sParam4, 0), HUtil32.Str_ToInt(sParam5, 0));
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.LUCKYPOINT.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdLuckPoint(M2Share.g_GameCommand.LUCKYPOINT.sCmd, M2Share.g_GameCommand.LUCKYPOINT.nPerMissionMin, sParam1, sParam2, sParam3);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.LOTTERYTICKET.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdLotteryTicket(M2Share.g_GameCommand.LOTTERYTICKET.sCmd, M2Share.g_GameCommand.LOTTERYTICKET.nPerMissionMin, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADGUILD.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdReloadGuild(M2Share.g_GameCommand.RELOADGUILD.sCmd, M2Share.g_GameCommand.RELOADGUILD.nPerMissionMin, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADLINENOTICE.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdReloadLineNotice(M2Share.g_GameCommand.RELOADLINENOTICE.sCmd, M2Share.g_GameCommand.RELOADLINENOTICE.nPerMissionMin, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADABUSE.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdReloadAbuse(M2Share.g_GameCommand.RELOADABUSE.sCmd, M2Share.g_GameCommand.RELOADABUSE.nPerMissionMin, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.FREEPENALTY.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdFreePenalty(M2Share.g_GameCommand.FREEPENALTY, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.PKPOINT.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdPKpoint(M2Share.g_GameCommand.PKPOINT, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.INCPKPOINT.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdIncPkPoint(M2Share.g_GameCommand.INCPKPOINT, sParam1, HUtil32.Str_ToInt(sParam2, 0));
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.VIEWWHISPER.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdViewWhisper(M2Share.g_GameCommand.VIEWWHISPER, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.REALIVE.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdReAlive(M2Share.g_GameCommand.REALIVE, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.KILL.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdKill(M2Share.g_GameCommand.KILL, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SMAKE.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdSmakeItem(M2Share.g_GameCommand.SMAKE, HUtil32.Str_ToInt(sParam1, 0), HUtil32.Str_ToInt(sParam2, 0), HUtil32.Str_ToInt(sParam3, 0));
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.CHANGEJOB.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdChangeJob(M2Share.g_GameCommand.CHANGEJOB, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.CHANGEGENDER.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdChangeGender(M2Share.g_GameCommand.CHANGEGENDER, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.HAIR.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdHair(M2Share.g_GameCommand.HAIR, sParam1, HUtil32.Str_ToInt(sParam2, 0));
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.BONUSPOINT.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdBonuPoint(M2Share.g_GameCommand.BONUSPOINT, sParam1, HUtil32.Str_ToInt(sParam2, 0));
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DELBONUSPOINT.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdDelBonuPoint(M2Share.g_GameCommand.DELBONUSPOINT, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.RESTBONUSPOINT.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdRestBonuPoint(M2Share.g_GameCommand.RESTBONUSPOINT, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SETPERMISSION.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdSetPermission(M2Share.g_GameCommand.SETPERMISSION, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.RENEWLEVEL.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdReNewLevel(M2Share.g_GameCommand.RENEWLEVEL, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DELGOLD.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdDelGold(M2Share.g_GameCommand.DELGOLD, sParam1, HUtil32.Str_ToInt(sParam2, 0));
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.ADDGOLD.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdAddGold(M2Share.g_GameCommand.ADDGOLD, sParam1, HUtil32.Str_ToInt(sParam2, 0));
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.GAMEGOLD.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdGameGold(M2Share.g_GameCommand.GAMEGOLD, sParam1, sParam2, HUtil32.Str_ToInt(sParam3, 0));
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.GAMEPOINT.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdGamePoint(M2Share.g_GameCommand.GAMEPOINT, sParam1, sParam2, HUtil32.Str_ToInt(sParam3, 0));
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.CREDITPOINT.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdCreditPoint(M2Share.g_GameCommand.CREDITPOINT, sParam1, sParam2, HUtil32.Str_ToInt(sParam3, 0));
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.TRAINING.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdTrainingSkill(M2Share.g_GameCommand.TRAINING, sParam1, sParam2, HUtil32.Str_ToInt(sParam3, 0));
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DELETEITEM.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdDeleteItem(M2Share.g_GameCommand.DELETEITEM, sParam1, sParam2, HUtil32.Str_ToInt(sParam3, 1));
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DELETESKILL.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdDelSkill(M2Share.g_GameCommand.DELETESKILL, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.CLEARMISSION.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdClearMission(M2Share.g_GameCommand.CLEARMISSION, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.STARTQUEST.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdStartQuest(M2Share.g_GameCommand.STARTQUEST, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DENYIPLOGON.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdDenyIPaddrLogon(M2Share.g_GameCommand.DENYIPLOGON, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.CHANGEDEARNAME.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdChangeDearName(M2Share.g_GameCommand.CHANGEDEARNAME, sParam1, sParam2);
                    return;
                }
                //if ((sCMD).CompareTo((M2Share.g_GameCommand.CHANGEMASTERNAME.sCmd), StringComparison.Ordinal) == 0)
                //{
                //    CmdChangeMasterName(M2Share.g_GameCommand.CHANGEMASTERNAME, sParam1, sParam2, sParam3);
                //    return;
                //}
                if (string.Compare(sCMD, M2Share.g_GameCommand.CLEARMON.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdClearMapMonster(M2Share.g_GameCommand.CLEARMON, sParam1, sParam2, sParam3);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DENYACCOUNTLOGON.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdDenyAccountLogon(M2Share.g_GameCommand.DENYACCOUNTLOGON, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DENYCHARNAMELOGON.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdDenyCharNameLogon(M2Share.g_GameCommand.DENYCHARNAMELOGON, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DELDENYIPLOGON.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdDelDenyIPaddrLogon(M2Share.g_GameCommand.DELDENYIPLOGON, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DELDENYACCOUNTLOGON.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdDelDenyAccountLogon(M2Share.g_GameCommand.DELDENYACCOUNTLOGON, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DELDENYCHARNAMELOGON.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdDelDenyCharNameLogon(M2Share.g_GameCommand.DELDENYCHARNAMELOGON, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SHOWDENYIPLOGON.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdShowDenyIPaddrLogon(M2Share.g_GameCommand.SHOWDENYIPLOGON, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SHOWDENYACCOUNTLOGON.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdShowDenyAccountLogon(M2Share.g_GameCommand.SHOWDENYACCOUNTLOGON, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SHOWDENYCHARNAMELOGON.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdShowDenyCharNameLogon(M2Share.g_GameCommand.SHOWDENYCHARNAMELOGON, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.MISSION.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdMission(M2Share.g_GameCommand.MISSION, sParam1, sParam2);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.MOBPLACE.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdMobPlace(M2Share.g_GameCommand.MOBPLACE, sParam1, sParam2, sParam3, sParam4);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SETMAPMODE.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdSetMapMode(M2Share.g_GameCommand.SETMAPMODE.sCmd, sParam1, sParam2, sParam3, sParam4);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.SHOWMAPMODE.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdShowMapMode(M2Share.g_GameCommand.SHOWMAPMODE.sCmd, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.CLRPASSWORD.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdClearHumanPassword(M2Share.g_GameCommand.CLRPASSWORD.sCmd, M2Share.g_GameCommand.CLRPASSWORD.nPerMissionMin, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.CONTESTPOINT.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdContestPoint(M2Share.g_GameCommand.CONTESTPOINT, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.STARTCONTEST.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdStartContest(M2Share.g_GameCommand.STARTCONTEST, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.ENDCONTEST.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdEndContest(M2Share.g_GameCommand.ENDCONTEST, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.ANNOUNCEMENT.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdAnnouncement(M2Share.g_GameCommand.ANNOUNCEMENT, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DISABLESENDMSG.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdDisableSendMsg(M2Share.g_GameCommand.DISABLESENDMSG, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.ENABLESENDMSG.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdEnableSendMsg(M2Share.g_GameCommand.ENABLESENDMSG, sParam1);
                    return;
                }
                if (string.Compare(sCMD, M2Share.g_GameCommand.DISABLESENDMSGLIST.sCmd, StringComparison.Ordinal) == 0)
                {
                    CmdDisableSendMsgList(M2Share.g_GameCommand.DISABLESENDMSGLIST);
                    return;
                }
                if (m_btPermission > 4)
                {
                    if (string.Compare(sCMD, M2Share.g_GameCommand.BACKSTEP.sCmd, StringComparison.Ordinal) == 0)
                    {
                        CmdBackStep(sCMD, HUtil32.Str_ToInt(sParam1, 0), HUtil32.Str_ToInt(sParam2, 1));
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.BALL.sCmd, StringComparison.Ordinal) == 0)
                    {
                        // 精神波
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.CHANGELUCK.sCmd, StringComparison.Ordinal) == 0)
                    {
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.HUNGER.sCmd, StringComparison.Ordinal) == 0)
                    {
                        CmdHunger(M2Share.g_GameCommand.HUNGER.sCmd, sParam1, HUtil32.Str_ToInt(sParam2, 0));
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.NAMECOLOR.sCmd, StringComparison.Ordinal) == 0)
                    {
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.TRANSPARECY.sCmd, StringComparison.Ordinal) == 0)
                    {
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.LEVEL0.sCmd, StringComparison.Ordinal) == 0)
                    {
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.SETFLAG.sCmd, StringComparison.Ordinal) == 0)
                    {
                        PlayObject = M2Share.UserEngine.GetPlayObject(sParam1);
                        if (PlayObject != null)
                        {
                            nFlag = HUtil32.Str_ToInt(sParam2, 0);
                            nValue = HUtil32.Str_ToInt(sParam3, 0);
                            PlayObject.SetQuestFlagStatus(nFlag, nValue);
                            if (PlayObject.GetQuestFalgStatus(nFlag) == 1)
                            {
                                SysMsg(PlayObject.m_sCharName + ": [" + nFlag.ToString() + "] = ON", TMsgColor.c_Green, TMsgType.t_Hint);
                            }
                            else
                            {
                                SysMsg(PlayObject.m_sCharName + ": [" + nFlag.ToString() + "] = OFF", TMsgColor.c_Green, TMsgType.t_Hint);
                            }
                        }
                        else
                        {
                            SysMsg('@' + M2Share.g_GameCommand.SETFLAG.sCmd + " 人物名称 标志号 数字(0 - 1)", TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.SETOPEN.sCmd, StringComparison.Ordinal) == 0)
                    {
                        PlayObject = M2Share.UserEngine.GetPlayObject(sParam1);
                        if (PlayObject != null)
                        {
                            nFlag = HUtil32.Str_ToInt(sParam2, 0);
                            nValue = HUtil32.Str_ToInt(sParam3, 0);
                            PlayObject.SetQuestUnitOpenStatus(nFlag, nValue);
                            if (PlayObject.GetQuestUnitOpenStatus(nFlag) == 1)
                            {
                                SysMsg(PlayObject.m_sCharName + ": [" + nFlag.ToString() + "] = ON", TMsgColor.c_Green, TMsgType.t_Hint);
                            }
                            else
                            {
                                SysMsg(PlayObject.m_sCharName + ": [" + nFlag.ToString() + "] = OFF", TMsgColor.c_Green, TMsgType.t_Hint);
                            }
                        }
                        else
                        {
                            SysMsg('@' + M2Share.g_GameCommand.SETOPEN.sCmd + " 人物名称 标志号 数字(0 - 1)", TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.SETUNIT.sCmd, StringComparison.Ordinal) == 0)
                    {
                        PlayObject = M2Share.UserEngine.GetPlayObject(sParam1);
                        if (PlayObject != null)
                        {
                            nFlag = HUtil32.Str_ToInt(sParam2, 0);
                            nValue = HUtil32.Str_ToInt(sParam3, 0);
                            PlayObject.SetQuestUnitStatus(nFlag, nValue);
                            if (PlayObject.GetQuestUnitStatus(nFlag) == 1)
                            {
                                SysMsg(PlayObject.m_sCharName + ": [" + nFlag.ToString() + "] = ON", TMsgColor.c_Green, TMsgType.t_Hint);
                            }
                            else
                            {
                                SysMsg(PlayObject.m_sCharName + ": [" + nFlag.ToString() + "] = OFF", TMsgColor.c_Green, TMsgType.t_Hint);
                            }
                        }
                        else
                        {
                            SysMsg('@' + M2Share.g_GameCommand.SETUNIT.sCmd + " 人物名称 标志号 数字(0 - 1)", TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.RECONNECTION.sCmd, StringComparison.Ordinal) == 0)
                    {
                        CmdReconnection(sCMD, sParam1, sParam2);
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.DISABLEFILTER.sCmd, StringComparison.Ordinal) == 0)
                    {
                        CmdDisableFilter(sCMD, sParam1);
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.CHGUSERFULL.sCmd, StringComparison.Ordinal) == 0)
                    {
                        CmdChangeUserFull(sCMD, sParam1);
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.CHGZENFASTSTEP.sCmd, StringComparison.Ordinal) == 0)
                    {
                        CmdChangeZenFastStep(sCMD, sParam1);
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.OXQUIZROOM.sCmd, StringComparison.Ordinal) == 0)
                    {
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.GSA.sCmd, StringComparison.Ordinal) == 0)
                    {
                        return;
                    }
                    if (string.Compare(sCMD, M2Share.g_GameCommand.CHANGEITEMNAME.sCmd, StringComparison.Ordinal) == 0)
                    {
                        CmdChangeItemName(M2Share.g_GameCommand.CHANGEITEMNAME.sCmd, sParam1, sParam2, sParam3);
                        return;
                    }
                    if (m_btPermission >= 5 || M2Share.g_Config.boTestServer)
                    {
                        if (string.Compare(sCMD, M2Share.g_GameCommand.FIREBURN.sCmd, StringComparison.Ordinal) == 0)
                        {
                            //CmdFireBurn(HUtil32.Str_ToInt(sParam1, 0), HUtil32.Str_ToInt(sParam2, 0), HUtil32.Str_ToInt(sParam3, 0));
                            return;
                        }
                        //if ((sCMD).CompareTo((M2Share.g_GameCommand.TESTFIRE.sCmd), StringComparison.Ordinal) == 0)
                        //{
                        //    //CmdTestFire(sCMD, HUtil32.Str_ToInt(sParam1, 0), HUtil32.Str_ToInt(sParam2, 0), HUtil32.Str_ToInt(sParam3, 0), HUtil32.Str_ToInt(sParam4, 0));
                        //    return;
                        //}
                        if (string.Compare(sCMD, M2Share.g_GameCommand.TESTSTATUS.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdTestStatus(sCMD, HUtil32.Str_ToInt(sParam1, -1), HUtil32.Str_ToInt(sParam2, 0));
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.DELGAMEGOLD.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdDelGameGold(M2Share.g_GameCommand.DELGAMEGOLD.sCmd, sParam1, HUtil32.Str_ToInt(sParam2, 0));
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ADDGAMEGOLD.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdAddGameGold(M2Share.g_GameCommand.ADDGAMEGOLD.sCmd, sParam1, HUtil32.Str_ToInt(sParam2, 0));
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.TESTGOLDCHANGE.sCmd, StringComparison.Ordinal) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADADMIN.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdReLoadAdmin(M2Share.g_GameCommand.RELOADADMIN.sCmd);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADNPC.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdReloadNpc(sParam1);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADMANAGE.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdReloadManage(M2Share.g_GameCommand.RELOADMANAGE, sParam1);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADROBOTMANAGE.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdReloadRobotManage();
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADROBOT.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdReloadRobot();
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADMONITEMS.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdReloadMonItems();
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADDIARY.sCmd, StringComparison.Ordinal) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADITEMDB.sCmd, StringComparison.Ordinal) == 0)
                        {
                            LocalDB.FrmDB.LoadItemsDB();
                            SysMsg("物品数据库重新加载完成。", TMsgColor.c_Green, TMsgType.t_Hint);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADMAGICDB.sCmd, StringComparison.Ordinal) == 0)
                        {
                            // FrmDB.LoadMagicDB();
                            // SysMsg('魔法数据库重新加载完成。',c_Green,t_Hint);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADMONSTERDB.sCmd, StringComparison.Ordinal) == 0)
                        {
                            LocalDB.FrmDB.LoadMonsterDB();
                            SysMsg("怪物数据库重新加载完成。", TMsgColor.c_Green, TMsgType.t_Hint);
                            return;
                        }
                        //if ((sCMD).CompareTo((M2Share.g_GameCommand.RELOADMINMAP.sCmd), StringComparison.Ordinal) == 0)
                        //{
                        //    // FrmDB.LoadMinMap();
                        //    // g_MapManager.ReSetMinMap();
                        //    SysMsg("小地图配置重新加载完成。", TMsgColor.c_Green, TMsgType.t_Hint);
                        //    return;
                        //}
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ADJUESTLEVEL.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdAdjuestLevel(M2Share.g_GameCommand.ADJUESTLEVEL, sParam1, HUtil32.Str_ToInt(sParam2, 1));
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ADJUESTEXP.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdAdjuestExp(M2Share.g_GameCommand.ADJUESTEXP, sParam1, sParam2);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ADDGUILD.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdAddGuild(M2Share.g_GameCommand.ADDGUILD, sParam1, sParam2);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.DELGUILD.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdDelGuild(M2Share.g_GameCommand.DELGUILD, sParam1);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.CHANGESABUKLORD.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdChangeSabukLord(M2Share.g_GameCommand.CHANGESABUKLORD, sParam1, sParam2, true);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdForcedWallconquestWar(M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR, sParam1);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ADDTOITEMEVENT.sCmd, StringComparison.Ordinal) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ADDTOITEMEVENTASPIECES.sCmd, StringComparison.Ordinal) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ITEMEVENTLIST.sCmd, StringComparison.Ordinal) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.STARTINGGIFTNO.sCmd, StringComparison.Ordinal) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.DELETEALLITEMEVENT.sCmd, StringComparison.Ordinal) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.STARTITEMEVENT.sCmd, StringComparison.Ordinal) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ITEMEVENTTERM.sCmd, StringComparison.Ordinal) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.ADJUESTTESTLEVEL.sCmd, StringComparison.Ordinal) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.OPDELETESKILL.sCmd, StringComparison.Ordinal) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.CHANGEWEAPONDURA.sCmd, StringComparison.Ordinal) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.RELOADGUILDALL.sCmd, StringComparison.Ordinal) == 0)
                        {
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.SPIRIT.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdSpirtStart(M2Share.g_GameCommand.SPIRIT.sCmd, sParam1);
                            return;
                        }
                        if (string.Compare(sCMD, M2Share.g_GameCommand.SPIRITSTOP.sCmd, StringComparison.Ordinal) == 0)
                        {
                            CmdSpirtStop(M2Share.g_GameCommand.SPIRITSTOP.sCmd, sParam1);
                            return;
                        }
                        //else if ((sCMD).CompareTo((M2Share.g_GameCommand.TESTSERVERCONFIG.sCmd), StringComparison.Ordinal) == 0)
                        //{
                        //    SendServerConfig();
                        //    return;
                        //}
                        //else if ((sCMD).CompareTo((M2Share.g_GameCommand.SERVERSTATUS.sCmd), StringComparison.Ordinal) == 0)
                        //{
                        //    SendServerStatus();
                        //    return;
                        //}
                        //else if ((sCMD).CompareTo((M2Share.g_GameCommand.TESTGETBAGITEM.sCmd), StringComparison.Ordinal) == 0)
                        //{
                        //    CmdTestGetBagItems(M2Share.g_GameCommand.TESTGETBAGITEM, sParam1);
                        //    return;
                        //}
                        //else if ((sCMD).CompareTo((M2Share.g_GameCommand.MOBFIREBURN.sCmd), StringComparison.Ordinal) == 0)
                        //{
                        //    CmdMobFireBurn(M2Share.g_GameCommand.MOBFIREBURN, sParam1, sParam2, sParam3, sParam4, sParam5, sParam6);
                        //    return;
                        //}
                        //else if ((sCMD).CompareTo((M2Share.g_GameCommand.TESTSPEEDMODE.sCmd), StringComparison.Ordinal) == 0)
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