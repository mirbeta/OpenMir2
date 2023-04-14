using GameSrv.Player;

namespace GameSrv.GameCommand.Commands {
    [Command("Training", "", 10)]
    public class TrainingCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (playObject.Permission < 6) {
                return;
            }
        }
    }
}