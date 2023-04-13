using GameSrv.Player;

namespace GameSrv.GameCommand.Commands {
    [Command("SbkDoorControl", "", 10)]
    public class SbkDoorControlCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
        }
    }
}