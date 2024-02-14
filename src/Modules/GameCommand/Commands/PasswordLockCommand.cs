using OpenMir2;
using SystemModule;
using SystemModule.Actors;
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
        }
    }
}