using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("Allowguildrecall", "", "", 0)]
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
