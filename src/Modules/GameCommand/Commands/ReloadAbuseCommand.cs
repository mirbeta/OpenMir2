using M2Server.Player;

namespace M2Server.GameCommand.Commands {
    [Command("ReloadAbuse", "无用", 10)]
    public class ReloadAbuseCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {

        }
    }
}