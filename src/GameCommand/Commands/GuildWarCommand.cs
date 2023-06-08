using M2Server.Player;

namespace M2Server.GameCommand.Commands {
    [Command("GuildWar", "", 10)]
    public class GuildWarCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (playObject.Permission < 6) {
                return;
            }
        }
    }
}