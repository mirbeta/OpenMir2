using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    [Command("Allowguildrecall", "", "")]
    public class AllowGuildRecallCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            playObject.AllowGuildReCall = !playObject.AllowGuildReCall;
            if (playObject.AllowGuildReCall) {
                playObject.SysMsg(CommandHelp.EnableGuildRecall, MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(CommandHelp.DisableGuildRecall, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}
