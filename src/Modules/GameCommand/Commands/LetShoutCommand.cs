using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    [Command("Letshout", "", "")]
    public class LetShoutCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            playObject.BanShout = !playObject.BanShout;
            if (playObject.BanShout) {
                playObject.SysMsg(CommandHelp.EnableShoutMsg, MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(CommandHelp.DisableShoutMsg, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}
