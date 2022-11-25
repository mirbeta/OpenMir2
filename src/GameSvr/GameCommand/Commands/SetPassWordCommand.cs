using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 设置密码
    /// </summary>
    [Command("SetPassWord", "设置登录密码", "", 0)]
    public class SetPassWordCommand : Command
    {
        [ExecuteCommand]
        public void SetPassWord(string[] @params, PlayObject playObject)
        {
            if (!M2Share.Config.PasswordLockSystem)
            {
                playObject.SysMsg(M2Share.g_sNoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.m_sStoragePwd == "")
            {
                playObject.SendMsg(playObject, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                playObject.m_boSetStoragePwd = true;
                playObject.SysMsg(M2Share.g_sSetPasswordMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(M2Share.g_sAlreadySetPasswordMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// 解除密码
    /// </summary>
    [Command("UnPassWord", "解除登录密码", "", 0)]
    public class UnPasswWordCommand : Command
    {
        [ExecuteCommand]
        public void UnPassWord(string[] @params, PlayObject playObject)
        {
            if (!M2Share.Config.PasswordLockSystem)
            {
                playObject.SysMsg(M2Share.g_sNoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!playObject.m_boPasswordLocked)
            {
                playObject.m_sStoragePwd = "";
                playObject.SysMsg(M2Share.g_sOldPasswordIsClearMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(M2Share.g_sPleaseUnLockPasswordMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// 修改登录密码
    /// </summary>
    [Command("ChgpassWordCommand", "修改登录密码", "", 0)]
    public class ChgpassWordCommand : Command
    {
        [ExecuteCommand]
        public void ChgpassWord(string[] @params, PlayObject playObject)
        {
            if (!M2Share.Config.PasswordLockSystem)
            {
                playObject.SysMsg(M2Share.g_sNoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.m_btPwdFailCount > 3)
            {
                playObject.SysMsg(M2Share.g_sStoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                playObject.m_boPasswordLocked = true;
                return;
            }
            if (playObject.m_sStoragePwd != "")
            {
                playObject.SendMsg(playObject, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                playObject.m_boCheckOldPwd = true;
                playObject.SysMsg(M2Share.g_sPleaseInputOldPasswordMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(M2Share.g_sNoPasswordSetMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// 解除仓库密码
    /// </summary>
    [Command("UnlockStorage", "解除仓库密码", "", 0)]
    public class UnlockStorageCommand : Command
    {
        [ExecuteCommand]
        public void UnlockStorage(string[] @params, PlayObject playObject)
        {
            if (!M2Share.Config.PasswordLockSystem)
            {
                playObject.SysMsg(M2Share.g_sNoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.m_btPwdFailCount > M2Share.Config.PasswordErrorCountLock)
            {
                playObject.SysMsg(M2Share.g_sStoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                playObject.m_boPasswordLocked = true;
                return;
            }
            if (playObject.m_sStoragePwd != "")
            {
                if (!playObject.m_boUnLockStoragePwd)
                {
                    playObject.SendMsg(playObject, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                    playObject.SysMsg(M2Share.g_sPleaseInputUnLockPasswordMsg, MsgColor.Green, MsgType.Hint);
                    playObject.m_boUnLockStoragePwd = true;
                }
                else
                {
                    playObject.SysMsg(M2Share.g_sStorageAlreadyUnLockMsg, MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                playObject.SysMsg(M2Share.g_sStorageNoPasswordMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// 解除密码
    /// </summary>
    [Command("UnLock", "解除密码", "", 0)]
    public class UnLockCommand : Command
    {
        [ExecuteCommand]
        public void UnLock(string[] @params, PlayObject playObject)
        {
            if (!M2Share.Config.PasswordLockSystem)
            {
                playObject.SysMsg(M2Share.g_sNoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.m_btPwdFailCount > M2Share.Config.PasswordErrorCountLock)
            {
                playObject.SysMsg(M2Share.g_sStoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                playObject.m_boPasswordLocked = true;
                return;
            }
            if (playObject.m_sStoragePwd != "")
            {
                if (!playObject.m_boUnLockPwd)
                {
                    playObject.SendMsg(playObject, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                    playObject.SysMsg(M2Share.g_sPleaseInputUnLockPasswordMsg, MsgColor.Green, MsgType.Hint);
                    playObject.m_boUnLockPwd = true;
                }
                else
                {
                    playObject.SysMsg(M2Share.g_sStorageAlreadyUnLockMsg, MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                playObject.SysMsg(M2Share.g_sStorageNoPasswordMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// 锁定密码
    /// </summary>
    [Command("Lock", "锁定密码", "", 0)]
    public class LockCommand : Command
    {
        [ExecuteCommand]
        public void Lock(string[] @params, PlayObject playObject)
        {
            if (!M2Share.Config.PasswordLockSystem)
            {
                playObject.SysMsg(M2Share.g_sNoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!playObject.m_boPasswordLocked)
            {
                if (playObject.m_sStoragePwd != "")
                {
                    playObject.m_boPasswordLocked = true;
                    playObject.m_boCanGetBackItem = false;
                    playObject.SysMsg(M2Share.g_sLockStorageSuccessMsg, MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    playObject.SysMsg(M2Share.g_sStorageNoPasswordMsg, MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                playObject.SysMsg(M2Share.g_sStorageAlreadyLockMsg, MsgColor.Red, MsgType.Hint);
            }
            return;
        }
    }

    /// <summary>
    /// SetFlag
    /// </summary>
    [Command("SetFlag", "SetFlag", "", 4)]
    public class SetFlagCommand : Command
    {
        [ExecuteCommand]
        public void SetFlag(string[] @params, PlayObject playObject)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            var PlayObject = M2Share.WorldEngine.GetPlayObject(@params[0]);
            if (PlayObject != null)
            {
                var nFlag = HUtil32.StrToInt(@params[1], 0);
                var nValue = HUtil32.StrToInt(@params[2], 0);
                PlayObject.SetQuestFlagStatus(nFlag, nValue);
                if (PlayObject.GetQuestFalgStatus(nFlag) == 1)
                {
                    playObject.SysMsg(PlayObject.ChrName + ": [" + nFlag + "] = ON", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    playObject.SysMsg(PlayObject.ChrName + ": [" + nFlag + "] = OFF", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                playObject.SysMsg('@' + this.GameCommand.Name + " 人物名称 标志号 数字(0 - 1)", MsgColor.Red, MsgType.Hint);
            }
            return;
        }
    }

    /// <summary>
    /// SetOpen
    /// </summary>
    [Command("SetOpen", "SetOpen", "", 4)]
    public class SetOpenCommand : Command
    {
        [ExecuteCommand]
        public void SetOpen(string[] @params, PlayObject playObject)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            var PlayObject = M2Share.WorldEngine.GetPlayObject(@params[0]);
            if (PlayObject != null)
            {
                var nFlag = HUtil32.StrToInt(@params[1], 0);
                var nValue = HUtil32.StrToInt(@params[2], 0);
                PlayObject.SetQuestUnitOpenStatus(nFlag, nValue);
                if (PlayObject.GetQuestUnitOpenStatus(nFlag) == 1)
                {
                    playObject.SysMsg(PlayObject.ChrName + ": [" + nFlag + "] = ON", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    playObject.SysMsg(PlayObject.ChrName + ": [" + nFlag + "] = OFF", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                playObject.SysMsg('@' + this.GameCommand.Name + " 人物名称 标志号 数字(0 - 1)", MsgColor.Red, MsgType.Hint);
            }
            return;
        }
    }

    /// <summary>
    /// SetUnit
    /// </summary>
    [Command("SetUnit", "SetUnit", "", 4)]
    public class SetUnitCommand : Command
    {
        [ExecuteCommand]
        public void SetUnit(string[] @params, PlayObject playObject)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            var PlayObject = M2Share.WorldEngine.GetPlayObject(@params[0]);
            if (PlayObject != null)
            {
                var nFlag = HUtil32.StrToInt(@params[1], 0);
                var nValue = HUtil32.StrToInt(@params[2], 0);
                PlayObject.SetQuestUnitStatus(nFlag, nValue);
                if (PlayObject.GetQuestUnitStatus(nFlag) == 1)
                {
                    playObject.SysMsg(PlayObject.ChrName + ": [" + nFlag + "] = ON", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    playObject.SysMsg(PlayObject.ChrName + ": [" + nFlag + "] = OFF", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                playObject.SysMsg('@' + this.GameCommand.Name + " 人物名称 标志号 数字(0 - 1)", MsgColor.Red, MsgType.Hint);
            }
            return;
        }
    }
}