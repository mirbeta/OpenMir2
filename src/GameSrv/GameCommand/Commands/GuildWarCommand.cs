using GameSrv.Player;

namespace GameSrv.GameCommand.Commands {
    [Command("GuildWar", "", 10)]
    public class GuildWarCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            if (PlayObject.Permission < 6) {
                return;
            }
        }
    }
}