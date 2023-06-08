using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    [Command("Banguildchat", "", "")]
    public class BanGuildChatCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            playObject.BanGuildChat = !playObject.BanGuildChat;
            if (playObject.BanGuildChat) {
                playObject.SysMsg(CommandHelp.EnableGuildChat, MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(CommandHelp.DisableGuildChat, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}