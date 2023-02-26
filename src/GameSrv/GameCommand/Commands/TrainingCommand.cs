using GameSrv.Player;

namespace GameSrv.GameCommand.Commands {
    [Command("Training", "", 10)]
    public class TrainingCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            if (PlayObject.Permission < 6) {
                return;
            }
        }
    }
}