using GameSvr.Player;

namespace GameSvr.GameCommand.Commands {
    [Command("ReloadAbuse", "无用", 10)]
    public class ReloadAbuseCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {

        }
    }
}