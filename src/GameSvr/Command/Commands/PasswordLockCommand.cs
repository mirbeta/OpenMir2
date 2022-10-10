using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 密码锁定
    /// </summary>
    [Command("PasswordLock", "锁定登录", "", 0)]
    public class PasswordLockCommand : Commond
    {
        [ExecuteCommand]
        public void PasswordLock(string[] @params, PlayObject playObject)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
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
                return;
            }
            if (playObject.m_btPwdFailCount > 3)
            {
                playObject.SysMsg(M2Share.g_sStoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                playObject.m_boPasswordLocked = true;
                return;
            }
            if (!string.IsNullOrEmpty(playObject.m_sStoragePwd))
            {
                playObject.SendMsg(playObject, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                playObject.m_boCheckOldPwd = true;
                playObject.SysMsg(M2Share.g_sPleaseInputOldPasswordMsg, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}