using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 设置密码
    /// </summary>
    [Command("SetPassWord", "设置登录密码", "", 0)]
    public class SetPassWordCommand : Command
    {
        [ExecuteCommand]
        public static void SetPassWord(string[] @params, PlayObject playObject)
        {
            if (!M2Share.Config.PasswordLockSystem)
            {
                playObject.SysMsg(Settings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.StoragePwd == "")
            {
                playObject.SendMsg(playObject, Messages.RM_PASSWORD, 0, 0, 0, 0, "");
                playObject.MBoSetStoragePwd = true;
                playObject.SysMsg(Settings.SetPasswordMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(Settings.AlreadySetPasswordMsg, MsgColor.Red, MsgType.Hint);
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
        public static void UnPassWord(string[] @params, PlayObject playObject)
        {
            if (!M2Share.Config.PasswordLockSystem)
            {
                playObject.SysMsg(Settings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!playObject.MBoPasswordLocked)
            {
                playObject.StoragePwd = "";
                playObject.SysMsg(Settings.OldPasswordIsClearMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(Settings.PleaseUnLockPasswordMsg, MsgColor.Red, MsgType.Hint);
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
        public static void ChgpassWord(string[] @params, PlayObject playObject)
        {
            if (!M2Share.Config.PasswordLockSystem)
            {
                playObject.SysMsg(Settings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.MBtPwdFailCount > 3)
            {
                playObject.SysMsg(Settings.StoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                playObject.MBoPasswordLocked = true;
                return;
            }
            if (playObject.StoragePwd != "")
            {
                playObject.SendMsg(playObject, Messages.RM_PASSWORD, 0, 0, 0, 0, "");
                playObject.MBoCheckOldPwd = true;
                playObject.SysMsg(Settings.PleaseInputOldPasswordMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(Settings.NoPasswordSetMsg, MsgColor.Red, MsgType.Hint);
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
        public static void UnlockStorage(string[] @params, PlayObject playObject)
        {
            if (!M2Share.Config.PasswordLockSystem)
            {
                playObject.SysMsg(Settings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.MBtPwdFailCount > M2Share.Config.PasswordErrorCountLock)
            {
                playObject.SysMsg(Settings.StoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                playObject.MBoPasswordLocked = true;
                return;
            }
            if (playObject.StoragePwd != "")
            {
                if (!playObject.MBoUnLockStoragePwd)
                {
                    playObject.SendMsg(playObject, Messages.RM_PASSWORD, 0, 0, 0, 0, "");
                    playObject.SysMsg(Settings.PleaseInputUnLockPasswordMsg, MsgColor.Green, MsgType.Hint);
                    playObject.MBoUnLockStoragePwd = true;
                }
                else
                {
                    playObject.SysMsg(Settings.StorageAlreadyUnLockMsg, MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                playObject.SysMsg(Settings.StorageNoPasswordMsg, MsgColor.Red, MsgType.Hint);
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
        public static void UnLock(string[] @params, PlayObject playObject)
        {
            if (!M2Share.Config.PasswordLockSystem)
            {
                playObject.SysMsg(Settings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.MBtPwdFailCount > M2Share.Config.PasswordErrorCountLock)
            {
                playObject.SysMsg(Settings.StoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                playObject.MBoPasswordLocked = true;
                return;
            }
            if (playObject.StoragePwd != "")
            {
                if (!playObject.MBoUnLockPwd)
                {
                    playObject.SendMsg(playObject, Messages.RM_PASSWORD, 0, 0, 0, 0, "");
                    playObject.SysMsg(Settings.PleaseInputUnLockPasswordMsg, MsgColor.Green, MsgType.Hint);
                    playObject.MBoUnLockPwd = true;
                }
                else
                {
                    playObject.SysMsg(Settings.StorageAlreadyUnLockMsg, MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                playObject.SysMsg(Settings.StorageNoPasswordMsg, MsgColor.Red, MsgType.Hint);
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
        public static void Lock(string[] @params, PlayObject playObject)
        {
            if (!M2Share.Config.PasswordLockSystem)
            {
                playObject.SysMsg(Settings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!playObject.MBoPasswordLocked)
            {
                if (playObject.StoragePwd != "")
                {
                    playObject.MBoPasswordLocked = true;
                    playObject.BoCanGetBackItem = false;
                    playObject.SysMsg(Settings.LockStorageSuccessMsg, MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    playObject.SysMsg(Settings.StorageNoPasswordMsg, MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                playObject.SysMsg(Settings.StorageAlreadyLockMsg, MsgColor.Red, MsgType.Hint);
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
            PlayObject PlayObject = M2Share.WorldEngine.GetPlayObject(@params[0]);
            if (PlayObject != null)
            {
                int nFlag = HUtil32.StrToInt(@params[1], 0);
                int nValue = HUtil32.StrToInt(@params[2], 0);
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
            PlayObject PlayObject = M2Share.WorldEngine.GetPlayObject(@params[0]);
            if (PlayObject != null)
            {
                int nFlag = HUtil32.StrToInt(@params[1], 0);
                int nValue = HUtil32.StrToInt(@params[2], 0);
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
            PlayObject PlayObject = M2Share.WorldEngine.GetPlayObject(@params[0]);
            if (PlayObject != null)
            {
                int nFlag = HUtil32.StrToInt(@params[1], 0);
                int nValue = HUtil32.StrToInt(@params[2], 0);
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