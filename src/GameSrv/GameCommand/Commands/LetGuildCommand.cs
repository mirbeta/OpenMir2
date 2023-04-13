using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("Letguild", "加入公会", "")]
    public class LetGuildCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            playObject.AllowGuild = !playObject.AllowGuild;
            if (playObject.AllowGuild) {
                playObject.SysMsg(CommandHelp.EnableJoinGuild, MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(CommandHelp.DisableJoinGuild, MsgColor.Green, MsgType.Hint);
            }
            return;
        }
    }
}