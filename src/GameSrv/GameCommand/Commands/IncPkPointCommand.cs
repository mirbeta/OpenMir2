using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家PK值
    /// </summary>
    [Command("IncPkPoint", "调整指定玩家PK值", CommandHelp.GameCommandIncPkPointHelpMsg, 10)]
    public class IncPkPointCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var nPoint = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;
            if (string.IsNullOrEmpty(sHumanName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = GameShare.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            mPlayObject.PkPoint += nPoint;
            mPlayObject.RefNameColor();
            if (nPoint > 0) {
                playObject.SysMsg(string.Format(CommandHelp.GameCommandIncPkPointAddPointMsg, sHumanName, nPoint), MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(string.Format(CommandHelp.GameCommandIncPkPointDecPointMsg, sHumanName, -nPoint), MsgColor.Green, MsgType.Hint);
            }
        }
    }
}