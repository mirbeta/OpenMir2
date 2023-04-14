using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 查看指定玩家信息
    /// </summary>
    [Command("HumanInfo", "查看指定玩家信息", CommandHelp.GameCommandHumanLocalHelpMsg, 10)]
    public class HumanInfoCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?') {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            playObject.SysMsg(playObject.GetBaseObjectInfo(), MsgColor.Green, MsgType.Hint);
        }
    }
}