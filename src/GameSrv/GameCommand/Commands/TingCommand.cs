using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 将指定人物随机传送(支持权限分配)
    /// </summary>
    [Command("Ting", "将指定人物随机传送(支持权限分配)", CommandHelp.GameCommandTingHelpMsg, 10)]
    public class TingCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?') {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = GameShare.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject != null) {
                mPlayObject.MapRandomMove(mPlayObject.HomeMap, 0);
            }
            else {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}