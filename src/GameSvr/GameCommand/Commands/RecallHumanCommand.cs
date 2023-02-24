using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 将指定人物召唤到身边(支持权限分配)
    /// </summary>
    [Command("RecallHuman", "将指定人物召唤到身边(支持权限分配)", CommandHelp.GameCommandPrvMsgHelpMsg, 10)]
    public class RecallHumanCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?') {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject.RecallHuman(sHumanName);
        }
    }
}