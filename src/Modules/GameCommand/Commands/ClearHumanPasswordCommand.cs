using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 清楚指定玩家仓库密码
    /// </summary>
    [Command("ClearHumanPassword", "清楚指定玩家仓库密码", "人物名称", 10)]
    public class ClearHumanPasswordCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?') {
                playObject.SysMsg("清除玩家的仓库密码!!!", MsgColor.Red, MsgType.Hint);
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                return;
            }
            mPlayObject.IsPasswordLocked = false;
            mPlayObject.IsUnLockStoragePwd = false;
            mPlayObject.StoragePwd = "";
            mPlayObject.SysMsg("你的保护密码已被清除!!!", MsgColor.Green, MsgType.Hint);
            playObject.SysMsg($"{sHumanName}的保护密码已被清除!!!", MsgColor.Green, MsgType.Hint);
        }
    }
}