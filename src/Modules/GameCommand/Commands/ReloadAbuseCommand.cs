using SystemModule;

namespace CommandSystem {
    [Command("ReloadAbuse", "无用", 10)]
    public class ReloadAbuseCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor) {

        }
    }
}