using GameSvr.GameCommand;
using SystemModule.Data;
using SystemModule.Enums;

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
                if (!PlayObject.BoReadyRun)
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
                    if (PlayObject.MsOffLineLeaveword != "")
                    {
                        PlayObject.Whisper(ChrName, PlayObject.MsOffLineLeaveword);
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
                    if (WhisperHuman != null && !WhisperHuman.Ghost)
                    {
                        WhisperHuman.SendMsg(WhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btGMWhisperMsgBColor, 0, ChrName + "=>" + PlayObject.ChrName + ' ' + saystr);
                    }
                    if (PlayObject.WhisperHuman != null && !PlayObject.WhisperHuman.Ghost)
                    {
                        PlayObject.WhisperHuman.SendMsg(PlayObject.WhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btGMWhisperMsgBColor, 0, ChrName + "=>" + PlayObject.ChrName + ' ' + saystr);
                    }
                }
                else
                {
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_WHISPER, 0, M2Share.Config.btWhisperMsgFColor, M2Share.Config.btWhisperMsgBColor, 0, ChrName + "=> " + saystr);
                    if (WhisperHuman != null && !WhisperHuman.Ghost)
                    {
                        WhisperHuman.SendMsg(WhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.Config.btWhisperMsgFColor, M2Share.Config.btWhisperMsgBColor, 0, ChrName + "=>" + PlayObject.ChrName + ' ' + saystr);
                    }
                    if (PlayObject.WhisperHuman != null && !PlayObject.WhisperHuman.Ghost)
                    {
                        PlayObject.WhisperHuman.SendMsg(PlayObject.WhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.Config.btWhisperMsgFColor, M2Share.Config.btWhisperMsgBColor, 0, ChrName + "=>" + PlayObject.ChrName + ' ' + saystr);
                    }
                }
            }
            else
            {
                if (M2Share.WorldEngine.FindOtherServerUser(whostr, ref svidx))
                {
                    World.WorldServer.SendServerGroupMsg(Grobal2.ISM_WHISPER, svidx, whostr + '/' + ChrName + "=> " + saystr);
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
            HUtil32.GetValidStr3(SayStr, ref sendwho, new[] { '[', ' ', '=', '>' });
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
                if ((HUtil32.GetTickCount() - SayMsgTick) < M2Share.Config.SayMsgTime)
                {
                    SayMsgCount++;
                    if (SayMsgCount >= M2Share.Config.SayMsgCount)
                    {
                        DisableSayMsg = true;
                        DisableSayMsgTick = HUtil32.GetTickCount() + M2Share.Config.DisableSayMsgTime;// 60 * 1000
                        SysMsg(Format(M2Share.g_sDisableSayMsg, M2Share.Config.DisableSayMsgTime / (60 * 1000)), MsgColor.Red, MsgType.Hint);
                    }
                }
                else
                {
                    SayMsgTick = HUtil32.GetTickCount();
                    SayMsgCount = 0;
                }
                if (HUtil32.GetTickCount() >= DisableSayMsgTick)
                {
                    DisableSayMsg = false;
                }
                boDisableSayMsg = DisableSayMsg;
                if (M2Share.DenySayMsgList.ContainsKey(this.ChrName))
                {
                    boDisableSayMsg = true;
                }
                if (!(boDisableSayMsg || Envir.Flag.boNOCHAT))
                {
                    //M2Share.Log.Info('[' + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "] " + ChrName + ": " + sData);
                    OldSayMsg = sData;
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
                                sText = HUtil32.GetValidStr3(sText, ref sParam1, new[] { ' ' });
                                if (!FilterSendMsg)
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
                                            World.WorldServer.SendServerGroupMsg(Grobal2.SS_208, M2Share.ServerIndex, ChrName + "/:" + sText);
                                            return;
                                        case '~' when MyGuild != null://发送行会消息
                                            sText = sData.AsSpan()[2..].ToString();
                                            MyGuild.SendGuildMsg(ChrName + ": " + sText);
                                            World.WorldServer.SendServerGroupMsg(Grobal2.SS_208, M2Share.ServerIndex, MyGuild.sGuildName + '/' + ChrName + '/' + sText);
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
                                        if (FilterSendMsg)
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
                    if (FilterSendMsg)
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
                M2Share.Log.Error(Format(sExceptionMsg, sData));
                M2Share.Log.Error(e.StackTrace);
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
            int nLen;
            const string sExceptionMsg = "[Exception] TPlayObject::ProcessUserLineMsg Msg = {0}";
            try
            {
                nLen = sData.Length;
                if (nLen <= 0)
                {
                    return;
                }
                if (MBoSetStoragePwd)
                {
                    MBoSetStoragePwd = false;
                    if (nLen > 3 && nLen < 8)
                    {
                        MSTempPwd = sData;
                        MBoReConfigPwd = true;
                        SysMsg(M2Share.g_sReSetPasswordMsg, MsgColor.Green, MsgType.Hint);
                        SendMsg(this, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                    }
                    else
                    {
                        SysMsg(M2Share.g_sPasswordOverLongMsg, MsgColor.Red, MsgType.Hint);// '输入的密码长度不正确!!!，密码长度必须在 4 - 7 的范围内，请重新设置密码。'
                    }
                    return;
                }
                if (MBoReConfigPwd)
                {
                    MBoReConfigPwd = false;
                    if (string.Compare(MSTempPwd, sData, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        StoragePwd = sData;
                        MBoPasswordLocked = true;
                        BoCanGetBackItem = false;
                        MSTempPwd = "";
                        SysMsg(M2Share.g_sReSetPasswordOKMsg, MsgColor.Blue, MsgType.Hint);
                    }
                    else
                    {
                        MSTempPwd = "";
                        SysMsg(M2Share.g_sReSetPasswordNotMatchMsg, MsgColor.Red, MsgType.Hint);
                    }
                    return;
                }
                if (MBoUnLockPwd || MBoUnLockStoragePwd)
                {
                    if (string.Compare(StoragePwd, sData, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        MBoPasswordLocked = false;
                        if (MBoUnLockPwd)
                        {
                            if (M2Share.Config.LockDealAction)
                            {
                                BoCanDeal = true;
                            }
                            if (M2Share.Config.LockDropAction)
                            {
                                BoCanDrop = true;
                            }
                            if (M2Share.Config.LockWalkAction)
                            {
                                BoCanWalk = true;
                            }
                            if (M2Share.Config.LockRunAction)
                            {
                                BoCanRun = true;
                            }
                            if (M2Share.Config.LockHitAction)
                            {
                                BoCanHit = true;
                            }
                            if (M2Share.Config.LockSpellAction)
                            {
                                BoCanSpell = true;
                            }
                            if (M2Share.Config.LockSendMsgAction)
                            {
                                BoCanSendMsg = true;
                            }
                            if (M2Share.Config.LockUserItemAction)
                            {
                                MBoCanUseItem = true;
                            }
                            if (M2Share.Config.LockInObModeAction)
                            {
                                ObMode = false;
                                AdminMode = false;
                            }
                            BoLockLogoned = true;
                            SysMsg(M2Share.g_sPasswordUnLockOKMsg, MsgColor.Blue, MsgType.Hint);
                        }
                        if (MBoUnLockStoragePwd)
                        {
                            if (M2Share.Config.LockGetBackItemAction)
                            {
                                BoCanGetBackItem = true;
                            }
                            SysMsg(M2Share.g_sStorageUnLockOKMsg, MsgColor.Blue, MsgType.Hint);
                        }
                    }
                    else
                    {
                        MBtPwdFailCount++;
                        SysMsg(M2Share.g_sUnLockPasswordFailMsg, MsgColor.Red, MsgType.Hint);
                        if (MBtPwdFailCount > 3)
                        {
                            SysMsg(M2Share.g_sStoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                        }
                    }
                    MBoUnLockPwd = false;
                    MBoUnLockStoragePwd = false;
                    return;
                }
                if (MBoCheckOldPwd)
                {
                    MBoCheckOldPwd = false;
                    if (StoragePwd == sData)
                    {
                        SendMsg(this, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                        SysMsg(M2Share.g_sSetPasswordMsg, MsgColor.Green, MsgType.Hint);
                        MBoSetStoragePwd = true;
                    }
                    else
                    {
                        MBtPwdFailCount++;
                        SysMsg(M2Share.g_sOldPasswordIncorrectMsg, MsgColor.Red, MsgType.Hint);
                        if (MBtPwdFailCount > 3)
                        {
                            SysMsg(M2Share.g_sStoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                            MBoPasswordLocked = true;
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
                sC = HUtil32.GetValidStr3(sC, ref sCMD, new[] { ' ', ':', ',', '\t' });
                if (sC != "")
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam1, new[] { ' ', ':', ',', '\t' });
                }
                if (sC != "")
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam2, new[] { ' ', ':', ',', '\t' });
                }
                if (sC != "")
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam3, new[] { ' ', ':', ',', '\t' });
                }
                if (sC != "")
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam4, new[] { ' ', ':', ',', '\t' });
                }
                if (sC != "")
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam5, new[] { ' ', ':', ',', '\t' });
                }
                if (sC != "")
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam6, new[] { ' ', ':', ',', '\t' });
                }
                if (sC != "")
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam7, new[] { ' ', ':', ',', '\t' });
                }
                if (CommandMgr.GetInstance().ExecCmd(sData, this))
                {
                    return;
                }
                if (Permission >= 2 && sData.Length > 2)
                {
                    if (Permission >= 6 && sData[2] == M2Share.g_GMRedMsgCmd)
                    {
                        if (HUtil32.GetTickCount() - SayMsgTick > 2000)
                        {
                            SayMsgTick = HUtil32.GetTickCount();
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
                M2Share.Log.Error(Format(sExceptionMsg, sData));
                M2Share.Log.Error(e.Message);
            }
        }
    }
}