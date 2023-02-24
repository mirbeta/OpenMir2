using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 设置密码
    /// </summary>
    [Command("SetPassWord", "设置登录密码", "", 0)]
    public class SetPassWordCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (!M2Share.Config.PasswordLockSystem) {
                playObject.SysMsg(Settings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.StoragePwd == "") {
                playObject.SendMsg(playObject, Messages.RM_PASSWORD, 0, 0, 0, 0, "");
                playObject.IsSetStoragePwd = true;
                playObject.SysMsg(Settings.SetPasswordMsg, MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(Settings.AlreadySetPasswordMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// 解除密码
    /// </summary>
    [Command("UnPassWord", "解除登录密码", "", 0)]
    public class UnPasswWordCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (!M2Share.Config.PasswordLockSystem) {
                playObject.SysMsg(Settings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!playObject.IsPasswordLocked) {
                playObject.StoragePwd = "";
                playObject.SysMsg(Settings.OldPasswordIsClearMsg, MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(Settings.PleaseUnLockPasswordMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// 修改登录密码
    /// </summary>
    [Command("ChgpassWordCommand", "修改登录密码", "", 0)]
    public class ChgpassWordCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (!M2Share.Config.PasswordLockSystem) {
                playObject.SysMsg(Settings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.PwdFailCount > 3) {
                playObject.SysMsg(Settings.StoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                playObject.IsPasswordLocked = true;
                return;
            }
            if (!string.IsNullOrEmpty(playObject.StoragePwd)) {
                playObject.SendMsg(playObject, Messages.RM_PASSWORD, 0, 0, 0, 0, "");
                playObject.IsCheckOldPwd = true;
                playObject.SysMsg(Settings.PleaseInputOldPasswordMsg, MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(Settings.NoPasswordSetMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// 解除仓库密码
    /// </summary>
    [Command("UnlockStorage", "解除仓库密码", "", 0)]
    public class UnlockStorageCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (!M2Share.Config.PasswordLockSystem) {
                playObject.SysMsg(Settings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.PwdFailCount > M2Share.Config.PasswordErrorCountLock) {
                playObject.SysMsg(Settings.StoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                playObject.IsPasswordLocked = true;
                return;
            }
            if (!string.IsNullOrEmpty(playObject.StoragePwd)) {
                if (!playObject.IsUnLockStoragePwd) {
                    playObject.SendMsg(playObject, Messages.RM_PASSWORD, 0, 0, 0, 0, "");
                    playObject.SysMsg(Settings.PleaseInputUnLockPasswordMsg, MsgColor.Green, MsgType.Hint);
                    playObject.IsUnLockStoragePwd = true;
                }
                else {
                    playObject.SysMsg(Settings.StorageAlreadyUnLockMsg, MsgColor.Red, MsgType.Hint);
                }
            }
            else {
                playObject.SysMsg(Settings.StorageNoPasswordMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// 解除密码
    /// </summary>
    [Command("UnLock", "解除密码", "", 0)]
    public class UnLockCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (!M2Share.Config.PasswordLockSystem) {
                playObject.SysMsg(Settings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.PwdFailCount > M2Share.Config.PasswordErrorCountLock) {
                playObject.SysMsg(Settings.StoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                playObject.IsPasswordLocked = true;
                return;
            }
            if (!string.IsNullOrEmpty(playObject.StoragePwd)) {
                if (!playObject.IsUnLockPwd) {
                    playObject.SendMsg(playObject, Messages.RM_PASSWORD, 0, 0, 0, 0, "");
                    playObject.SysMsg(Settings.PleaseInputUnLockPasswordMsg, MsgColor.Green, MsgType.Hint);
                    playObject.IsUnLockPwd = true;
                }
                else {
                    playObject.SysMsg(Settings.StorageAlreadyUnLockMsg, MsgColor.Red, MsgType.Hint);
                }
            }
            else {
                playObject.SysMsg(Settings.StorageNoPasswordMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }

    /// <summary>
    /// 锁定密码
    /// </summary>
    [Command("Lock", "锁定密码", "", 0)]
    public class LockCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (!M2Share.Config.PasswordLockSystem) {
                playObject.SysMsg(Settings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!playObject.IsPasswordLocked) {
                if (!string.IsNullOrEmpty(playObject.StoragePwd)) {
                    playObject.IsPasswordLocked = true;
                    playObject.IsCanGetBackItem = false;
                    playObject.SysMsg(Settings.LockStorageSuccessMsg, MsgColor.Green, MsgType.Hint);
                }
                else {
                    playObject.SysMsg(Settings.StorageNoPasswordMsg, MsgColor.Green, MsgType.Hint);
                }
            }
            else {
                playObject.SysMsg(Settings.StorageAlreadyLockMsg, MsgColor.Red, MsgType.Hint);
            }
            return;
        }
    }

    /// <summary>
    /// SetFlag
    /// </summary>
    [Command("SetFlag", "SetFlag", "", 4)]
    public class SetFlagCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null || @params.Length <= 0) {
                return;
            }
            PlayObject PlayObject = M2Share.WorldEngine.GetPlayObject(@params[0]);
            if (PlayObject != null) {
                int nFlag = HUtil32.StrToInt(@params[1], 0);
                int nValue = HUtil32.StrToInt(@params[2], 0);
                PlayObject.SetQuestFlagStatus(nFlag, nValue);
                if (PlayObject.GetQuestFalgStatus(nFlag) == 1) {
                    playObject.SysMsg(PlayObject.ChrName + ": [" + nFlag + "] = ON", MsgColor.Green, MsgType.Hint);
                }
                else {
                    playObject.SysMsg(PlayObject.ChrName + ": [" + nFlag + "] = OFF", MsgColor.Green, MsgType.Hint);
                }
            }
            else {
                playObject.SysMsg('@' + this.Command.Name + " 人物名称 标志号 数字(0 - 1)", MsgColor.Red, MsgType.Hint);
            }
            return;
        }
    }

    /// <summary>
    /// SetOpen
    /// </summary>
    [Command("SetOpen", "SetOpen", "", 4)]
    public class SetOpenCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null || @params.Length <= 0) {
                return;
            }
            PlayObject PlayObject = M2Share.WorldEngine.GetPlayObject(@params[0]);
            if (PlayObject != null) {
                int nFlag = HUtil32.StrToInt(@params[1], 0);
                int nValue = HUtil32.StrToInt(@params[2], 0);
                PlayObject.SetQuestUnitOpenStatus(nFlag, nValue);
                if (PlayObject.GetQuestUnitOpenStatus(nFlag) == 1) {
                    playObject.SysMsg(PlayObject.ChrName + ": [" + nFlag + "] = ON", MsgColor.Green, MsgType.Hint);
                }
                else {
                    playObject.SysMsg(PlayObject.ChrName + ": [" + nFlag + "] = OFF", MsgColor.Green, MsgType.Hint);
                }
            }
            else {
                playObject.SysMsg('@' + this.Command.Name + " 人物名称 标志号 数字(0 - 1)", MsgColor.Red, MsgType.Hint);
            }
            return;
        }
    }

    /// <summary>
    /// SetUnit
    /// </summary>
    [Command("SetUnit", "SetUnit", "", 4)]
    public class SetUnitCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null || @params.Length <= 0) {
                return;
            }
            PlayObject PlayObject = M2Share.WorldEngine.GetPlayObject(@params[0]);
            if (PlayObject != null) {
                int nFlag = HUtil32.StrToInt(@params[1], 0);
                int nValue = HUtil32.StrToInt(@params[2], 0);
                PlayObject.SetQuestUnitStatus(nFlag, nValue);
                if (PlayObject.GetQuestUnitStatus(nFlag) == 1) {
                    playObject.SysMsg(PlayObject.ChrName + ": [" + nFlag + "] = ON", MsgColor.Green, MsgType.Hint);
                }
                else {
                    playObject.SysMsg(PlayObject.ChrName + ": [" + nFlag + "] = OFF", MsgColor.Green, MsgType.Hint);
                }
            }
            else {
                playObject.SysMsg('@' + this.Command.Name + " 人物名称 标志号 数字(0 - 1)", MsgColor.Red, MsgType.Hint);
            }
            return;
        }
    }
}