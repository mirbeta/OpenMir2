using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 密码锁定
    /// </summary>
    [Command("PasswordLock", "锁定登录", "", 0)]
    public class PasswordLockCommand : Command
    {
        [ExecuteCommand]
        public static void PasswordLock(string[] @params, PlayObject playObject)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            if (!M2Share.Config.PasswordLockSystem)
            {
                playObject.SysMsg(Settings.g_sNoPasswordLockSystemMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.StoragePwd == "")
            {
                playObject.SendMsg(playObject, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                playObject.MBoSetStoragePwd = true;
                playObject.SysMsg(Settings.g_sSetPasswordMsg, MsgColor.Green, MsgType.Hint);
                return;
            }
            if (playObject.MBtPwdFailCount > 3)
            {
                playObject.SysMsg(Settings.g_sStoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                playObject.MBoPasswordLocked = true;
                return;
            }
            if (!string.IsNullOrEmpty(playObject.StoragePwd))
            {
                playObject.SendMsg(playObject, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                playObject.MBoCheckOldPwd = true;
                playObject.SysMsg(Settings.g_sPleaseInputOldPasswordMsg, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}