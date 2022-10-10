using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 清楚指定玩家仓库密码
    /// </summary>
    [Command("ClearHumanPassword", "清楚指定玩家仓库密码", "人物名称", 10)]
    public class ClearHumanPasswordCommand : Command
    {
        [ExecuteCommand]
        public void ClearHumanPassword(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
            {
                PlayObject.SysMsg("清除玩家的仓库密码!!!", MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                return;
            }
            m_PlayObject.m_boPasswordLocked = false;
            m_PlayObject.m_boUnLockStoragePwd = false;
            m_PlayObject.m_sStoragePwd = "";
            m_PlayObject.SysMsg("你的保护密码已被清除!!!", MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg($"{sHumanName}的保护密码已被清除!!!", MsgColor.Green, MsgType.Hint);
        }
    }
}