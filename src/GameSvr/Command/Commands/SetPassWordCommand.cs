using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 设置密码
    /// </summary>
    [GameCommand("SetPassWord", "设置登录密码", "", 0)]
    public class SetPassWordCommand : BaseCommond
    {
        [DefaultCommand]
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
    [GameCommand("UnPassWord", "解除登录密码", "", 0)]
    public class UnPasswWordCommand : BaseCommond
    {
        [DefaultCommand]
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
    [GameCommand("ChgpassWordCommand", "修改登录密码", "", 0)]
    public class ChgpassWordCommand : BaseCommond
    {
        [DefaultCommand]
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
    [GameCommand("UnlockStorage", "解除仓库密码", "", 0)]
    public class UnlockStorageCommand : BaseCommond
    {
        [DefaultCommand]
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
    [GameCommand("UnLock", "解除密码", "", 0)]
    public class UnLockCommand : BaseCommond
    {
        [DefaultCommand]
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
    [GameCommand("Lock", "锁定密码", "", 0)]
    public class LockCommand : BaseCommond
    {
        [DefaultCommand]
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
    [GameCommand("SetFlag", "SetFlag", "", 4)]
    public class SetFlagCommand : BaseCommond
    {
        [DefaultCommand]
        public void SetFlag(string[] @params, PlayObject playObject)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            var PlayObject = M2Share.WorldEngine.GetPlayObject(@params[0]);
            if (PlayObject != null)
            {
                var nFlag = HUtil32.Str_ToInt(@params[1], 0);
                var nValue = HUtil32.Str_ToInt(@params[2], 0);
                PlayObject.SetQuestFlagStatus(nFlag, nValue);
                if (PlayObject.GetQuestFalgStatus(nFlag) == 1)
                {
                    playObject.SysMsg(PlayObject.CharName + ": [" + nFlag + "] = ON", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    playObject.SysMsg(PlayObject.CharName + ": [" + nFlag + "] = OFF", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                playObject.SysMsg('@' + M2Share.GameCommand.SetFlag.CommandName + " 人物名称 标志号 数字(0 - 1)", MsgColor.Red, MsgType.Hint);
            }
            return;
        }
    }

    /// <summary>
    /// SetOpen
    /// </summary>
    [GameCommand("SetOpen", "SetOpen", "", 4)]
    public class SetOpenCommand : BaseCommond
    {
        [DefaultCommand]
        public void SetOpen(string[] @params, PlayObject playObject)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            var PlayObject = M2Share.WorldEngine.GetPlayObject(@params[0]);
            if (PlayObject != null)
            {
                var nFlag = HUtil32.Str_ToInt(@params[1], 0);
                var nValue = HUtil32.Str_ToInt(@params[2], 0);
                PlayObject.SetQuestUnitOpenStatus(nFlag, nValue);
                if (PlayObject.GetQuestUnitOpenStatus(nFlag) == 1)
                {
                    playObject.SysMsg(PlayObject.CharName + ": [" + nFlag + "] = ON", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    playObject.SysMsg(PlayObject.CharName + ": [" + nFlag + "] = OFF", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                playObject.SysMsg('@' + M2Share.GameCommand.SetOpen.CommandName + " 人物名称 标志号 数字(0 - 1)", MsgColor.Red, MsgType.Hint);
            }
            return;
        }
    }

    /// <summary>
    /// SetUnit
    /// </summary>
    [GameCommand("SetUnit", "SetUnit", "", 4)]
    public class SetUnitCommand : BaseCommond
    {
        [DefaultCommand]
        public void SetUnit(string[] @params, PlayObject playObject)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            var PlayObject = M2Share.WorldEngine.GetPlayObject(@params[0]);
            if (PlayObject != null)
            {
                var nFlag = HUtil32.Str_ToInt(@params[1], 0);
                var nValue = HUtil32.Str_ToInt(@params[2], 0);
                PlayObject.SetQuestUnitStatus(nFlag, nValue);
                if (PlayObject.GetQuestUnitStatus(nFlag) == 1)
                {
                    playObject.SysMsg(PlayObject.CharName + ": [" + nFlag + "] = ON", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    playObject.SysMsg(PlayObject.CharName + ": [" + nFlag + "] = OFF", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                playObject.SysMsg('@' + M2Share.GameCommand.SetUnit.CommandName + " 人物名称 标志号 数字(0 - 1)", MsgColor.Red, MsgType.Hint);
            }
            return;
        }
    }
}