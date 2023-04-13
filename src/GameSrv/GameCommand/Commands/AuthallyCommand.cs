using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("Authally", "", "")]
    internal class AuthallyCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (playObject.IsGuildMaster()) {
                playObject.MyGuild.EnableAuthAlly = !playObject.MyGuild.EnableAuthAlly;
                if (playObject.MyGuild.EnableAuthAlly) {
                    playObject.SysMsg(CommandHelp.EnableAuthAllyGuild, MsgColor.Green, MsgType.Hint);
                }
                else {
                    playObject.SysMsg(CommandHelp.DisableAuthAllyGuild, MsgColor.Green, MsgType.Hint);
                }
            }
            return;
        }
    }
}