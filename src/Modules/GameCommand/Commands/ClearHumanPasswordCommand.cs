using SystemModule;
using SystemModule.Enums;

namespace CommandSystem
{
    /// <summary>
    /// 清楚指定玩家仓库密码
    /// </summary>
    [Command("ClearHumanPassword", "清楚指定玩家仓库密码", "人物名称", 10)]
    public class ClearHumanPasswordCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?') {
                PlayerActor.SysMsg("清除玩家的仓库密码!!!", MsgColor.Red, MsgType.Hint);
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = ModuleShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null) {
                return;
            }
            mIPlayerActor.IsPasswordLocked = false;
            mIPlayerActor.IsUnLockStoragePwd = false;
            mIPlayerActor.StoragePwd = "";
            mIPlayerActor.SysMsg("你的保护密码已被清除!!!", MsgColor.Green, MsgType.Hint);
            PlayerActor.SysMsg($"{sHumanName}的保护密码已被清除!!!", MsgColor.Green, MsgType.Hint);
        }
    }
}