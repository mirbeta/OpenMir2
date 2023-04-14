using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 将指定人物召唤到身边(支持权限分配)
    /// </summary>
    [Command("RecallHuman", "将指定人物召唤到身边(支持权限分配)", CommandHelp.GameCommandPrvMsgHelpMsg, 10)]
    public class RecallHumanCommand : GameCommand {
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
            playObject.RecallHuman(sHumanName);
        }
    }
}