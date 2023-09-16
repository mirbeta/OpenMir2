using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 设置密码
    /// </summary>
    [Command("SetPassWord", "设置登录密码", "")]
    public class SetPassWordCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (!SystemShare.Config.PasswordLockSystem)
            {
                PlayerActor.SysMsg(MessageSettings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (string.IsNullOrEmpty(PlayerActor.StoragePwd))
            {
                PlayerActor.SendMsg(Messages.RM_PASSWORD, 0, 0, 0, 0);
                PlayerActor.IsSetStoragePwd = true;
                PlayerActor.SysMsg(MessageSettings.SetPasswordMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg(MessageSettings.AlreadySetPasswordMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// 解除密码
    /// </summary>
    [Command("UnPassWord", "解除登录密码", "")]
    public class UnPasswWordCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (!SystemShare.Config.PasswordLockSystem)
            {
                PlayerActor.SysMsg(MessageSettings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!PlayerActor.IsPasswordLocked)
            {
                PlayerActor.StoragePwd = "";
                PlayerActor.SysMsg(MessageSettings.OldPasswordIsClearMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg(MessageSettings.PleaseUnLockPasswordMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// 修改登录密码
    /// </summary>
    [Command("ChgpassWordCommand", "修改登录密码", "")]
    public class ChgpassWordCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (!SystemShare.Config.PasswordLockSystem)
            {
                PlayerActor.SysMsg(MessageSettings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayerActor.PwdFailCount > 3)
            {
                PlayerActor.SysMsg(MessageSettings.StoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                PlayerActor.IsPasswordLocked = true;
                return;
            }
            if (!string.IsNullOrEmpty(PlayerActor.StoragePwd))
            {
                PlayerActor.SendMsg(Messages.RM_PASSWORD, 0, 0, 0, 0);
                PlayerActor.IsCheckOldPwd = true;
                PlayerActor.SysMsg(MessageSettings.PleaseInputOldPasswordMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg(MessageSettings.NoPasswordSetMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// 解除仓库密码
    /// </summary>
    [Command("UnlockStorage", "解除仓库密码", "")]
    public class UnlockStorageCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (!SystemShare.Config.PasswordLockSystem)
            {
                PlayerActor.SysMsg(MessageSettings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayerActor.PwdFailCount > SystemShare.Config.PasswordErrorCountLock)
            {
                PlayerActor.SysMsg(MessageSettings.StoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                PlayerActor.IsPasswordLocked = true;
                return;
            }
            if (!string.IsNullOrEmpty(PlayerActor.StoragePwd))
            {
                if (!PlayerActor.IsUnLockStoragePwd)
                {
                    PlayerActor.SendMsg(Messages.RM_PASSWORD, 0, 0, 0, 0);
                    PlayerActor.SysMsg(MessageSettings.PleaseInputUnLockPasswordMsg, MsgColor.Green, MsgType.Hint);
                    PlayerActor.IsUnLockStoragePwd = true;
                }
                else
                {
                    PlayerActor.SysMsg(MessageSettings.StorageAlreadyUnLockMsg, MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayerActor.SysMsg(MessageSettings.StorageNoPasswordMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// 解除密码
    /// </summary>
    [Command("UnLock", "解除密码", "")]
    public class UnLockCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (!SystemShare.Config.PasswordLockSystem)
            {
                PlayerActor.SysMsg(MessageSettings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayerActor.PwdFailCount > SystemShare.Config.PasswordErrorCountLock)
            {
                PlayerActor.SysMsg(MessageSettings.StoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                PlayerActor.IsPasswordLocked = true;
                return;
            }
            if (!string.IsNullOrEmpty(PlayerActor.StoragePwd))
            {
                if (!PlayerActor.IsUnLockPwd)
                {
                    PlayerActor.SendMsg(Messages.RM_PASSWORD, 0, 0, 0, 0);
                    PlayerActor.SysMsg(MessageSettings.PleaseInputUnLockPasswordMsg, MsgColor.Green, MsgType.Hint);
                    PlayerActor.IsUnLockPwd = true;
                }
                else
                {
                    PlayerActor.SysMsg(MessageSettings.StorageAlreadyUnLockMsg, MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayerActor.SysMsg(MessageSettings.StorageNoPasswordMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// 锁定密码
    /// </summary>
    [Command("Lock", "锁定密码", "")]
    public class LockCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (!SystemShare.Config.PasswordLockSystem)
            {
                PlayerActor.SysMsg(MessageSettings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!PlayerActor.IsPasswordLocked)
            {
                if (!string.IsNullOrEmpty(PlayerActor.StoragePwd))
                {
                    PlayerActor.IsPasswordLocked = true;
                    PlayerActor.IsCanGetBackItem = false;
                    PlayerActor.SysMsg(MessageSettings.LockStorageSuccessMsg, MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    PlayerActor.SysMsg(MessageSettings.StorageNoPasswordMsg, MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                PlayerActor.SysMsg(MessageSettings.StorageAlreadyLockMsg, MsgColor.Red, MsgType.Hint);
            }
            return;
        }
    }

    /// <summary>
    /// SetFlag
    /// </summary>
    [Command("SetFlag", "SetFlag", "", 4)]
    public class SetFlagCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            var targetObject = SystemShare.WorldEngine.GetPlayObject(@params[0]);
            if (targetObject != null)
            {
                var nFlag = HUtil32.StrToInt(@params[1], 0);
                var nValue = HUtil32.StrToInt(@params[2], 0);
                PlayerActor.SetQuestFlagStatus(nFlag, nValue);
                if (PlayerActor.GetQuestFalgStatus(nFlag) == 1)
                {
                    targetObject.SysMsg(PlayerActor.ChrName + ": [" + nFlag + "] = ON", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    targetObject.SysMsg(PlayerActor.ChrName + ": [" + nFlag + "] = OFF", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                targetObject.SysMsg('@' + this.Command.Name + " 人物名称 标志号 数字(0 - 1)", MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// SetOpen
    /// </summary>
    [Command("SetOpen", "SetOpen", "", 4)]
    public class SetOpenCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            var targetObject = SystemShare.WorldEngine.GetPlayObject(@params[0]);
            if (targetObject != null)
            {
                var nFlag = HUtil32.StrToInt(@params[1], 0);
                var nValue = HUtil32.StrToInt(@params[2], 0);
                PlayerActor.SetQuestUnitOpenStatus(nFlag, nValue);
                if (PlayerActor.GetQuestUnitOpenStatus(nFlag) == 1)
                {
                    targetObject.SysMsg(PlayerActor.ChrName + ": [" + nFlag + "] = ON", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    targetObject.SysMsg(PlayerActor.ChrName + ": [" + nFlag + "] = OFF", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                targetObject.SysMsg('@' + this.Command.Name + " 人物名称 标志号 数字(0 - 1)", MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// SetUnit
    /// </summary>
    [Command("SetUnit", "SetUnit", "", 4)]
    public class SetUnitCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            var targetObject = SystemShare.WorldEngine.GetPlayObject(@params[0]);
            if (targetObject != null)
            {
                var nFlag = HUtil32.StrToInt(@params[1], 0);
                var nValue = HUtil32.StrToInt(@params[2], 0);
                PlayerActor.SetQuestUnitStatus(nFlag, nValue);
                if (PlayerActor.GetQuestUnitStatus(nFlag) == 1)
                {
                    targetObject.SysMsg(PlayerActor.ChrName + ": [" + nFlag + "] = ON", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    targetObject.SysMsg(PlayerActor.ChrName + ": [" + nFlag + "] = OFF", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                targetObject.SysMsg('@' + this.Command.Name + " 人物名称 标志号 数字(0 - 1)", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}