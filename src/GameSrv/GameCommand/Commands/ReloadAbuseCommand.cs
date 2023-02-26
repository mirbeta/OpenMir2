using GameSrv.Player;

namespace GameSrv.GameCommand.Commands {
    [Command("ReloadAbuse", "无用", 10)]
    public class ReloadAbuseCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {

        }
    }
}