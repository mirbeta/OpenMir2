using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 清除指定玩家的任务标志。
    /// </summary>
    [Command("ClearMission", "清除指定玩家的任务标志", "人物名称", 10)]
    public class ClearMissionCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (sHumanName[0] == '?') {
                playObject.SysMsg("此命令用于清除人物的任务标志。", MsgColor.Blue, MsgType.Hint);
                return;
            }
            var mPlayObject = GameShare.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                playObject.SysMsg($"{sHumanName}不在线，或在其它服务器上!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            playObject.SysMsg($"{sHumanName}的任务标志已经全部清零。", MsgColor.Green, MsgType.Hint);
        }
    }
}