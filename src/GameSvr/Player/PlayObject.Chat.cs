using GameSvr.GameCommand;
using System.Globalization;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Player
{
    public partial class PlayObject
    {
        /// <summary>
        /// 玩家私聊
        /// </summary>
        /// <param name="whostr"></param>
        /// <param name="saystr"></param>
        protected virtual void Whisper(string whostr, string saystr)
        {
            var svidx = 0;
            var PlayObject = M2Share.WorldEngine.GetPlayObject(whostr);
            if (PlayObject != null)
            {
                if (!PlayObject.m_boReadyRun)
                {
                    SysMsg(whostr + M2Share.g_sCanotSendmsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (!PlayObject.HearWhisper || PlayObject.IsBlockWhisper(ChrName))
                {
                    SysMsg(whostr + M2Share.g_sUserDenyWhisperMsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (!OffLineFlag && PlayObject.OffLineFlag)
                {
                    if (PlayObject.MSOffLineLeaveword != "")
                    {
                        PlayObject.Whisper(ChrName, PlayObject.MSOffLineLeaveword);
                    }
                    else
                    {
                        PlayObject.Whisper(ChrName, M2Share.Config.ServerName + '[' + M2Share.Config.ServerIPaddr + "]提示您");
                    }
                    return;
                }
                if (Permission > 0)
                {
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btGMWhisperMsgBColor, 0, ChrName + "=> " + saystr);
                    if (m_GetWhisperHuman != null && !m_GetWhisperHuman.Ghost)
                    {
                        m_GetWhisperHuman.SendMsg(m_GetWhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btGMWhisperMsgBColor, 0, ChrName + "=>" + PlayObject.ChrName + ' ' + saystr);
                    }
                    if (PlayObject.m_GetWhisperHuman != null && !PlayObject.m_GetWhisperHuman.Ghost)
                    {
                        PlayObject.m_GetWhisperHuman.SendMsg(PlayObject.m_GetWhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btGMWhisperMsgBColor, 0, ChrName + "=>" + PlayObject.ChrName + ' ' + saystr);
                    }
                }
                else
                {
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_WHISPER, 0, M2Share.Config.btWhisperMsgFColor, M2Share.Config.btWhisperMsgBColor, 0, ChrName + "=> " + saystr);
                    if (m_GetWhisperHuman != null && !m_GetWhisperHuman.Ghost)
                    {
                        m_GetWhisperHuman.SendMsg(m_GetWhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.Config.btWhisperMsgFColor, M2Share.Config.btWhisperMsgBColor, 0, ChrName + "=>" + PlayObject.ChrName + ' ' + saystr);
                    }
                    if (PlayObject.m_GetWhisperHuman != null && !PlayObject.m_GetWhisperHuman.Ghost)
                    {
                        PlayObject.m_GetWhisperHuman.SendMsg(PlayObject.m_GetWhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.Config.btWhisperMsgFColor, M2Share.Config.btWhisperMsgBColor, 0, ChrName + "=>" + PlayObject.ChrName + ' ' + saystr);
                    }
                }
            }
            else
            {
                if (M2Share.WorldEngine.FindOtherServerUser(whostr, ref svidx))
                {
                    M2Share.WorldEngine.SendServerGroupMsg(Grobal2.ISM_WHISPER, svidx, whostr + '/' + ChrName + "=> " + saystr);
                }
                else
                {
                    SysMsg(whostr + M2Share.g_sUserNotOnLine, MsgColor.Red, MsgType.Hint);
                }
            }
        }

        public void WhisperRe(string SayStr, byte MsgType)
        {
            var sendwho = string.Empty;
            HUtil32.GetValidStr3(SayStr, ref sendwho, new string[] { "[", " ", "=", ">" });
            if (HearWhisper && !IsBlockWhisper(sendwho))
            {
                switch (MsgType)
                {
                    case 0:
                        SendMsg(this, Grobal2.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btGMWhisperMsgBColor, 0, SayStr);
                        break;
                    case 1:
                        SendMsg(this, Grobal2.RM_WHISPER, 0, M2Share.Config.btWhisperMsgFColor, M2Share.Config.btWhisperMsgBColor, 0, SayStr);
                        break;
                    case 2:
                        SendMsg(this, Grobal2.RM_WHISPER, 0, M2Share.Config.PurpleMsgFColor, M2Share.Config.PurpleMsgBColor, 0, SayStr);
                        break;
                }
            }
        }

        /// <summary>
        /// 处理玩家说话
        /// </summary>
        /// <param name="sData"></param>
        protected override void ProcessSayMsg(string sData)
        {
            bool boDisableSayMsg;
            var sParam1 = string.Empty;
            const string sExceptionMsg = "[Exception] TPlayObject.ProcessSayMsg Msg = {0}";
            try
            {
                if (sData.Length > M2Share.Config.SayMsgMaxLen)
                {
                    sData = sData[..M2Share.Config.SayMsgMaxLen]; // 3 * 1000
                }
                if ((HUtil32.GetTickCount() - m_dwSayMsgTick) < M2Share.Config.SayMsgTime)
                {
                    m_nSayMsgCount++;// 2
                    if (m_nSayMsgCount >= M2Share.Config.SayMsgCount)
                    {
                        m_boDisableSayMsg = true;
                        m_dwDisableSayMsgTick = HUtil32.GetTickCount() + M2Share.Config.DisableSayMsgTime;// 60 * 1000
                        SysMsg(Format(M2Share.g_sDisableSayMsg, M2Share.Config.DisableSayMsgTime / (60 * 1000)), MsgColor.Red, MsgType.Hint);
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
                if (M2Share.DenySayMsgList.ContainsKey(this.ChrName))
                {
                    boDisableSayMsg = true;
                }
                if (!(boDisableSayMsg || Envir.Flag.boNOCHAT))
                {
                    M2Share.Log.LogInformation('[' + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "] " + ChrName + ": " + sData);
                    m_sOldSayMsg = sData;
                    if (sData.StartsWith("@@加速处理"))
                    {
                        M2Share.g_FunctionNPC.GotoLable(this, "@加速处理", false);
                        return;
                    }
                    string sText;
                    switch (sData[0])
                    {
                        case '/':
                        {
                            sText = sData.AsSpan()[1..].ToString();
                            sText = HUtil32.GetValidStr3(sText, ref sParam1, new[] { " " });
                            if (!m_boFilterSendMsg)
                            {
                                Whisper(sParam1, sText);
                            }
                            return;
                        }
                        case '!':
                            {
                                if (sData.Length >= 1)
                                {
                                    switch (sData[1])
                                    {
                                        case '!'://发送组队消息
                                            sText = sData.AsSpan()[2..].ToString();
                                            SendGroupText(ChrName + ": " + sText);
                                            M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_208, M2Share.ServerIndex, ChrName + "/:" + sText);
                                            return;
                                        case '~' when MyGuild != null://发送行会消息
                                            sText = sData.AsSpan()[2..].ToString();
                                            MyGuild.SendGuildMsg(ChrName + ": " + sText);
                                            M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_208, M2Share.ServerIndex, MyGuild.sGuildName + '/' + ChrName + '/' + sText);
                                            return;
                                    }
                                }
                                if (!Envir.Flag.boQUIZ)
                                {
                                    if ((HUtil32.GetTickCount() - ShoutMsgTick) > 10 * 1000)
                                    {
                                        if (Abil.Level <= M2Share.Config.CanShoutMsgLevel)
                                        {
                                            SysMsg(Format(M2Share.g_sYouNeedLevelMsg, M2Share.Config.CanShoutMsgLevel + 1), MsgColor.Red, MsgType.Hint);
                                            return;
                                        }
                                        ShoutMsgTick = HUtil32.GetTickCount();
                                        sText = sData.AsSpan()[1..].ToString();
                                        var sCryCryMsg = "(!)" + ChrName + ": " + sText;
                                        if (m_boFilterSendMsg)
                                        {
                                            SendMsg(null, Grobal2.RM_CRY, 0, 0, 0xFFFF, 0, sCryCryMsg);
                                        }
                                        else
                                        {
                                            M2Share.WorldEngine.CryCry(Grobal2.RM_CRY, Envir, CurrX, CurrY, 50, M2Share.Config.CryMsgFColor, M2Share.Config.CryMsgBColor, sCryCryMsg);
                                        }
                                        return;
                                    }
                                    SysMsg(Format(M2Share.g_sYouCanSendCyCyLaterMsg, new[] { 10 - (HUtil32.GetTickCount() - ShoutMsgTick) / 1000 }), MsgColor.Red, MsgType.Hint);
                                    return;
                                }
                                SysMsg(M2Share.g_sThisMapDisableSendCyCyMsg, MsgColor.Red, MsgType.Hint);
                                return;
                            }
                    }
                    if (m_boFilterSendMsg)
                    {
                        SendMsg(this, Grobal2.RM_HEAR, 0, M2Share.Config.btHearMsgFColor, M2Share.Config.btHearMsgBColor, 0, ChrName + ':' + sData);// 如果禁止发信息，则只向自己发信息
                    }
                    else
                    {
                        base.ProcessSayMsg(sData);
                    }
                    return;
                }
                SysMsg(M2Share.g_sYouIsDisableSendMsg, MsgColor.Red, MsgType.Hint);
            }
            catch (Exception e)
            {
                M2Share.Log.LogError(Format(sExceptionMsg, sData));
                M2Share.Log.LogError(e.StackTrace);
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
            PlayObject PlayObject;
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
                        SysMsg(M2Share.g_sReSetPasswordMsg, MsgColor.Green, MsgType.Hint);
                        SendMsg(this, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                    }
                    else
                    {
                        SysMsg(M2Share.g_sPasswordOverLongMsg, MsgColor.Red, MsgType.Hint);// '输入的密码长度不正确!!!，密码长度必须在 4 - 7 的范围内，请重新设置密码。'
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
                        SysMsg(M2Share.g_sReSetPasswordOKMsg, MsgColor.Blue, MsgType.Hint);
                    }
                    else
                    {
                        m_sTempPwd = "";
                        SysMsg(M2Share.g_sReSetPasswordNotMatchMsg, MsgColor.Red, MsgType.Hint);
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
                            if (M2Share.Config.LockDealAction)
                            {
                                m_boCanDeal = true;
                            }
                            if (M2Share.Config.LockDropAction)
                            {
                                m_boCanDrop = true;
                            }
                            if (M2Share.Config.LockWalkAction)
                            {
                                m_boCanWalk = true;
                            }
                            if (M2Share.Config.LockRunAction)
                            {
                                m_boCanRun = true;
                            }
                            if (M2Share.Config.LockHitAction)
                            {
                                m_boCanHit = true;
                            }
                            if (M2Share.Config.LockSpellAction)
                            {
                                m_boCanSpell = true;
                            }
                            if (M2Share.Config.LockSendMsgAction)
                            {
                                m_boCanSendMsg = true;
                            }
                            if (M2Share.Config.LockUserItemAction)
                            {
                                m_boCanUseItem = true;
                            }
                            if (M2Share.Config.LockInObModeAction)
                            {
                                ObMode = false;
                                AdminMode = false;
                            }
                            m_boLockLogoned = true;
                            SysMsg(M2Share.g_sPasswordUnLockOKMsg, MsgColor.Blue, MsgType.Hint);
                        }
                        if (m_boUnLockStoragePwd)
                        {
                            if (M2Share.Config.LockGetBackItemAction)
                            {
                                m_boCanGetBackItem = true;
                            }
                            SysMsg(M2Share.g_sStorageUnLockOKMsg, MsgColor.Blue, MsgType.Hint);
                        }
                    }
                    else
                    {
                        m_btPwdFailCount++;
                        SysMsg(M2Share.g_sUnLockPasswordFailMsg, MsgColor.Red, MsgType.Hint);
                        if (m_btPwdFailCount > 3)
                        {
                            SysMsg(M2Share.g_sStoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
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
                        SysMsg(M2Share.g_sSetPasswordMsg, MsgColor.Green, MsgType.Hint);
                        m_boSetStoragePwd = true;
                    }
                    else
                    {
                        m_btPwdFailCount++;
                        SysMsg(M2Share.g_sOldPasswordIncorrectMsg, MsgColor.Red, MsgType.Hint);
                        if (m_btPwdFailCount > 3)
                        {
                            SysMsg(M2Share.g_sStoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
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
                sC = sData.AsSpan()[1..].ToString();
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
                if (CommandMgr.GetInstance().ExecCmd(sData, this))
                {
                    return;
                }
                if (Permission >= 2 && sData.Length > 2)
                {
                    if (Permission >= 6 && sData[2] == M2Share.g_GMRedMsgCmd)
                    {
                        if (HUtil32.GetTickCount() - m_dwSayMsgTick > 2000)
                        {
                            m_dwSayMsgTick = HUtil32.GetTickCount();
                            sData = sData.AsSpan()[2..].ToString();
                            if (sData.Length > M2Share.Config.SayRedMsgMaxLen)
                            {
                                sData = sData[..M2Share.Config.SayRedMsgMaxLen];
                            }
                            if (M2Share.Config.ShutRedMsgShowGMName)
                            {
                                sC = ChrName + ": " + sData;
                            }
                            else
                            {
                                sC = sData;
                            }
                            M2Share.WorldEngine.SendBroadCastMsg(sC, MsgType.GameManger);
                        }
                        return;
                    }
                }
                SysMsg($"@{sCMD}此命令不正确，或没有足够的权限!!!", MsgColor.Red, MsgType.Hint);
            }
            catch (Exception e)
            {
                M2Share.Log.LogError(Format(sExceptionMsg, sData));
                M2Share.Log.LogError(e.Message);
            }
        }
    }
}