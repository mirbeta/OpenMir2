using SystemModule;
using SystemModule.Enums;

namespace M2Server.Player
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
            int svidx = 0;
            IPlayerActor PlayObject = M2Share.WorldEngine.GetPlayObject(whostr);
            if (PlayObject != null)
            {
                if (!PlayObject.BoReadyRun)
                {
                    SysMsg(whostr + Settings.CanotSendmsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (!PlayObject.HearWhisper || PlayObject.IsBlockWhisper(ChrName))
                {
                    SysMsg(whostr + Settings.UserDenyWhisperMsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (!OffLineFlag && PlayObject.OffLineFlag)
                {
                    if (!string.IsNullOrEmpty(PlayObject.OffLineLeaveWord))
                    {
                        PlayObject.Whisper(ChrName, PlayObject.OffLineLeaveWord);
                    }
                    else
                    {
                        PlayObject.Whisper(ChrName, SystemShare.Config.ServerName + '[' + SystemShare.Config.ServerIPaddr + "]提示您");
                    }
                    return;
                }
                if (Permission > 0)
                {
                    PlayObject.SendMsg(PlayObject, Messages.RM_WHISPER, 0, SystemShare.Config.btGMWhisperMsgFColor, SystemShare.Config.btGMWhisperMsgBColor, 0, ChrName + "=> " + saystr);
                    if (WhisperHuman != null && !WhisperHuman.Ghost)
                    {
                        WhisperHuman.SendMsg(WhisperHuman, Messages.RM_WHISPER, 0, SystemShare.Config.btGMWhisperMsgFColor, SystemShare.Config.btGMWhisperMsgBColor, 0, ChrName + "=>" + PlayObject.ChrName + ' ' + saystr);
                    }
                    if (PlayObject.WhisperHuman != null && !PlayObject.WhisperHuman.Ghost)
                    {
                        PlayObject.WhisperHuman.SendMsg(PlayObject.WhisperHuman, Messages.RM_WHISPER, 0, SystemShare.Config.btGMWhisperMsgFColor, SystemShare.Config.btGMWhisperMsgBColor, 0, ChrName + "=>" + PlayObject.ChrName + ' ' + saystr);
                    }
                }
                else
                {
                    PlayObject.SendMsg(PlayObject, Messages.RM_WHISPER, 0, SystemShare.Config.btWhisperMsgFColor, SystemShare.Config.btWhisperMsgBColor, 0, ChrName + "=> " + saystr);
                    if (WhisperHuman != null && !WhisperHuman.Ghost)
                    {
                        WhisperHuman.SendMsg(WhisperHuman, Messages.RM_WHISPER, 0, SystemShare.Config.btWhisperMsgFColor, SystemShare.Config.btWhisperMsgBColor, 0, ChrName + "=>" + PlayObject.ChrName + ' ' + saystr);
                    }
                    if (PlayObject.WhisperHuman != null && !PlayObject.WhisperHuman.Ghost)
                    {
                        PlayObject.WhisperHuman.SendMsg(PlayObject.WhisperHuman, Messages.RM_WHISPER, 0, SystemShare.Config.btWhisperMsgFColor, SystemShare.Config.btWhisperMsgBColor, 0, ChrName + "=>" + PlayObject.ChrName + ' ' + saystr);
                    }
                }
            }
            else
            {
                if (M2Share.WorldEngine.FindOtherServerUser(whostr, ref svidx))
                {
                    M2Share.WorldEngine.SendServerGroupMsg(Messages.ISM_WHISPER, svidx, whostr + '/' + ChrName + "=> " + saystr);
                }
                else
                {
                    SysMsg(whostr + Settings.UserNotOnLine, MsgColor.Red, MsgType.Hint);
                }
            }
        }

        public void WhisperRe(string SayStr, byte MsgType)
        {
            string sendwho = string.Empty;
            HUtil32.GetValidStr3(SayStr, ref sendwho, new[] { '[', ' ', '=', '>' });
            if (HearWhisper && !IsBlockWhisper(sendwho))
            {
                switch (MsgType)
                {
                    case 0:
                        SendMsg(Messages.RM_WHISPER, 0, SystemShare.Config.btGMWhisperMsgFColor, SystemShare.Config.btGMWhisperMsgBColor, 0, SayStr);
                        break;
                    case 1:
                        SendMsg(Messages.RM_WHISPER, 0, SystemShare.Config.btWhisperMsgFColor, SystemShare.Config.btWhisperMsgBColor, 0, SayStr);
                        break;
                    case 2:
                        SendMsg(Messages.RM_WHISPER, 0, SystemShare.Config.PurpleMsgFColor, SystemShare.Config.PurpleMsgBColor, 0, SayStr);
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
            string sParam1 = string.Empty;
            const string sExceptionMsg = "[Exception] PlayObject.ProcessSayMsg Msg = {0}";
            try
            {
                if (sData.Length > SystemShare.Config.SayMsgMaxLen)
                {
                    sData = sData[..SystemShare.Config.SayMsgMaxLen]; // 3 * 1000
                }
                if ((HUtil32.GetTickCount() - SayMsgTick) < SystemShare.Config.SayMsgTime)
                {
                    SayMsgCount++;
                    if (SayMsgCount >= SystemShare.Config.SayMsgCount)
                    {
                        DisableSayMsg = true;
                        DisableSayMsgTick = HUtil32.GetTickCount() + SystemShare.Config.DisableSayMsgTime;// 60 * 1000
                        SysMsg(Format(Settings.DisableSayMsg, SystemShare.Config.DisableSayMsgTime / (60 * 1000)), MsgColor.Red, MsgType.Hint);
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
                    //OldSayMsg = sData;
                    if (sData.StartsWith("@@加速处理"))
                    {
                        //ModuleShare.FunctionNPC.GotoLable(this, "@加速处理", false);
                        return;
                    }
                    string sText;
                    switch (sData[0])
                    {
                        case '/':
                            {
                                sText = sData.AsSpan()[1..].ToString();
                                sText = HUtil32.GetValidStr3(sText, ref sParam1, ' ');
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
                                            M2Share.WorldEngine.SendServerGroupMsg(Messages.SS_208, M2Share.ServerIndex, ChrName + "/:" + sText);
                                            return;
                                        case '~' when MyGuild != null://发送行会消息
                                            sText = sData.AsSpan()[2..].ToString();
                                            MyGuild.SendGuildMsg(ChrName + ": " + sText);
                                            M2Share.WorldEngine.SendServerGroupMsg(Messages.SS_208, M2Share.ServerIndex, MyGuild.GuildName + '/' + ChrName + '/' + sText);
                                            return;
                                    }
                                }
                                if (!Envir.Flag.boQUIZ)
                                {
                                    if ((HUtil32.GetTickCount() - ShoutMsgTick) > 10 * 1000)
                                    {
                                        if (Abil.Level <= SystemShare.Config.CanShoutMsgLevel)
                                        {
                                            SysMsg(Format(Settings.YouNeedLevelMsg, SystemShare.Config.CanShoutMsgLevel + 1), MsgColor.Red, MsgType.Hint);
                                            return;
                                        }
                                        ShoutMsgTick = HUtil32.GetTickCount();
                                        sText = sData.AsSpan()[1..].ToString();
                                        string sCryCryMsg = "(!)" + ChrName + ": " + sText;
                                        if (FilterSendMsg)
                                        {
                                            SendMsg(null, Messages.RM_CRY, 0, 0, 0xFFFF, 0, sCryCryMsg);
                                        }
                                        else
                                        {
                                            M2Share.WorldEngine.CryCry(Messages.RM_CRY, Envir, CurrX, CurrY, 50, SystemShare.Config.CryMsgFColor, SystemShare.Config.CryMsgBColor, sCryCryMsg);
                                        }
                                        return;
                                    }
                                    SysMsg(Format(Settings.YouCanSendCyCyLaterMsg, new[] { 10 - (HUtil32.GetTickCount() - ShoutMsgTick) / 1000 }), MsgColor.Red, MsgType.Hint);
                                    return;
                                }
                                SysMsg(Settings.ThisMapDisableSendCyCyMsg, MsgColor.Red, MsgType.Hint);
                                return;
                            }
                    }
                    if (FilterSendMsg)
                    {
                        SendMsg(Messages.RM_HEAR, 0, SystemShare.Config.btHearMsgFColor, SystemShare.Config.btHearMsgBColor, 0, ChrName + ':' + sData);// 如果禁止发信息，则只向自己发信息
                    }
                    else
                    {
                        base.ProcessSayMsg(sData);
                    }
                    return;
                }
                SysMsg(Settings.YouIsDisableSendMsg, MsgColor.Red, MsgType.Hint);
            }
            catch (Exception e)
            {
                M2Share.Logger.Error(Format(sExceptionMsg, sData));
                M2Share.Logger.Error(e.StackTrace);
            }
        }

        public void ProcessUserLineMsg(string sData)
        {
            string sC;
            string sCMD = string.Empty;
            string sParam1 = string.Empty;
            string sParam2 = string.Empty;
            string sParam3 = string.Empty;
            string sParam4 = string.Empty;
            string sParam5 = string.Empty;
            string sParam6 = string.Empty;
            string sParam7 = string.Empty;
            int nLen;
            const string sExceptionMsg = "[Exception] PlayObject::ProcessUserLineMsg Msg = {0}";
            try
            {
                nLen = sData.Length;
                if (nLen <= 0)
                {
                    return;
                }
                if (IsSetStoragePwd)
                {
                    IsSetStoragePwd = false;
                    if (nLen > 3 && nLen < 8)
                    {
                        MSTempPwd = sData;
                        IsReConfigPwd = true;
                        SysMsg(Settings.ReSetPasswordMsg, MsgColor.Green, MsgType.Hint);
                        SendMsg(Messages.RM_PASSWORD, 0, 0, 0, 0);
                    }
                    else
                    {
                        SysMsg(Settings.PasswordOverLongMsg, MsgColor.Red, MsgType.Hint);// '输入的密码长度不正确!!!，密码长度必须在 4 - 7 的范围内，请重新设置密码。'
                    }
                    return;
                }
                if (IsReConfigPwd)
                {
                    IsReConfigPwd = false;
                    if (string.Compare(MSTempPwd, sData, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        StoragePwd = sData;
                        IsPasswordLocked = true;
                        IsCanGetBackItem = false;
                        SysMsg(Settings.ReSetPasswordOKMsg, MsgColor.Blue, MsgType.Hint);
                    }
                    else
                    {
                        SysMsg(Settings.ReSetPasswordNotMatchMsg, MsgColor.Red, MsgType.Hint);
                    }
                    return;
                }
                if (IsUnLockPwd || IsUnLockStoragePwd)
                {
                    if (string.Compare(StoragePwd, sData, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        IsPasswordLocked = false;
                        if (IsUnLockPwd)
                        {
                            if (SystemShare.Config.LockDealAction)
                            {
                                IsCanDeal = true;
                            }
                            if (SystemShare.Config.LockDropAction)
                            {
                                IsCanDrop = true;
                            }
                            if (SystemShare.Config.LockWalkAction)
                            {
                                IsCanWalk = true;
                            }
                            if (SystemShare.Config.LockRunAction)
                            {
                                IsCanRun = true;
                            }
                            if (SystemShare.Config.LockHitAction)
                            {
                                IsCanHit = true;
                            }
                            if (SystemShare.Config.LockSpellAction)
                            {
                                IsCanSpell = true;
                            }
                            if (SystemShare.Config.LockSendMsgAction)
                            {
                                IsCanSendMsg = true;
                            }
                            if (SystemShare.Config.LockUserItemAction)
                            {
                                BoCanUseItem = true;
                            }
                            if (SystemShare.Config.LockInObModeAction)
                            {
                                ObMode = false;
                                AdminMode = false;
                            }
                            IsLockLogoned = true;
                            SysMsg(Settings.PasswordUnLockOKMsg, MsgColor.Blue, MsgType.Hint);
                        }
                        if (IsUnLockStoragePwd)
                        {
                            if (SystemShare.Config.LockGetBackItemAction)
                            {
                                IsCanGetBackItem = true;
                            }
                            SysMsg(Settings.StorageUnLockOKMsg, MsgColor.Blue, MsgType.Hint);
                        }
                    }
                    else
                    {
                        PwdFailCount++;
                        SysMsg(Settings.UnLockPasswordFailMsg, MsgColor.Red, MsgType.Hint);
                        if (PwdFailCount > 3)
                        {
                            SysMsg(Settings.StoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                        }
                    }
                    IsUnLockPwd = false;
                    IsUnLockStoragePwd = false;
                    return;
                }
                if (IsCheckOldPwd)
                {
                    IsCheckOldPwd = false;
                    if (StoragePwd == sData)
                    {
                        SendMsg(Messages.RM_PASSWORD, 0, 0, 0, 0);
                        SysMsg(Settings.SetPasswordMsg, MsgColor.Green, MsgType.Hint);
                        IsSetStoragePwd = true;
                    }
                    else
                    {
                        PwdFailCount++;
                        SysMsg(Settings.OldPasswordIncorrectMsg, MsgColor.Red, MsgType.Hint);
                        if (PwdFailCount > 3)
                        {
                            SysMsg(Settings.StoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                            IsPasswordLocked = true;
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
                if (!string.IsNullOrEmpty(sC))
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam1, new[] { ' ', ':', ',', '\t' });
                }
                if (!string.IsNullOrEmpty(sC))
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam2, new[] { ' ', ':', ',', '\t' });
                }
                if (!string.IsNullOrEmpty(sC))
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam3, new[] { ' ', ':', ',', '\t' });
                }
                if (!string.IsNullOrEmpty(sC))
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam4, new[] { ' ', ':', ',', '\t' });
                }
                if (!string.IsNullOrEmpty(sC))
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam5, new[] { ' ', ':', ',', '\t' });
                }
                if (!string.IsNullOrEmpty(sC))
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam6, new[] { ' ', ':', ',', '\t' });
                }
                if (!string.IsNullOrEmpty(sC))
                {
                    sC = HUtil32.GetValidStr3(sC, ref sParam7, new[] { ' ', ':', ',', '\t' });
                }
                //if (CommandMgr.Execute(this, sData))
                //{
                //    return;
                //}
                if (Permission >= 2 && sData.Length > 2)
                {
                    if (Permission >= 6 && sData[2] == M2Share.GMRedMsgCmd)
                    {
                        if (HUtil32.GetTickCount() - SayMsgTick > 2000)
                        {
                            SayMsgTick = HUtil32.GetTickCount();
                            sData = sData.AsSpan()[2..].ToString();
                            if (sData.Length > SystemShare.Config.SayRedMsgMaxLen)
                            {
                                sData = sData[..SystemShare.Config.SayRedMsgMaxLen];
                            }
                            if (SystemShare.Config.ShutRedMsgShowGMName)
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
                M2Share.Logger.Error(Format(sExceptionMsg, sData));
                M2Share.Logger.Error(e.Message);
            }
        }
    }
}