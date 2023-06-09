using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 密码锁定
    /// </summary>
    [Command("PasswordLock", "锁定登录", "")]
    public class PasswordLockCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            if (!ModuleShare.Config.PasswordLockSystem)
            {
                PlayerActor.SysMsg(Settings.NoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (string.IsNullOrEmpty(PlayerActor.StoragePwd))
            {
                PlayerActor.SendMsg(Messages.RM_PASSWORD, 0, 0, 0, 0);
                PlayerActor.IsSetStoragePwd = true;
                PlayerActor.SysMsg(Settings.SetPasswordMsg, MsgColor.Green, MsgType.Hint);
                return;
            }
            if (PlayerActor.PwdFailCount > 3)
            {
                PlayerActor.SysMsg(Settings.StoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                PlayerActor.IsPasswordLocked = true;
                return;
            }
            if (!string.IsNullOrEmpty(PlayerActor.StoragePwd))
            {
                PlayerActor.SendMsg(Messages.RM_PASSWORD, 0, 0, 0, 0);
                PlayerActor.IsCheckOldPwd = true;
                PlayerActor.SysMsg(Settings.PleaseInputOldPasswordMsg, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}