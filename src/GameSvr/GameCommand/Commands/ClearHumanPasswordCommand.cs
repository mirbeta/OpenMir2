using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 清楚指定玩家仓库密码
    /// </summary>
    [Command("ClearHumanPassword", "清楚指定玩家仓库密码", "人物名称", 10)]
    public class ClearHumanPasswordCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
            {
                PlayObject.SysMsg("清除玩家的仓库密码!!!", MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                return;
            }
            m_PlayObject.IsPasswordLocked = false;
            m_PlayObject.IsUnLockStoragePwd = false;
            m_PlayObject.StoragePwd = "";
            m_PlayObject.SysMsg("你的保护密码已被清除!!!", MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg($"{sHumanName}的保护密码已被清除!!!", MsgColor.Green, MsgType.Hint);
        }
    }
}