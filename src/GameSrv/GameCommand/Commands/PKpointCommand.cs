using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 查看指定玩家PK值
    /// </summary>
    [Command("PKpoint", "查看指定玩家PK值", CommandHelp.GameCommandPKPointHelpMsg, 10)]
    public class PKpointCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (!string.IsNullOrEmpty(sHumanName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            playObject.SysMsg(string.Format(CommandHelp.GameCommandPKPointMsg, sHumanName, mPlayObject.PkPoint), MsgColor.Green, MsgType.Hint);
        }
    }
}