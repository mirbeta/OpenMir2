using M2Server.Player;

namespace M2Server.GameCommand.Commands {
    [Command("SbkDoorControl", "", 10)]
    public class SbkDoorControlCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
        }
    }
}