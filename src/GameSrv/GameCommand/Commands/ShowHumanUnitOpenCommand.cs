using GameSrv.Player;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// ShowOpen
    /// </summary>
    [Command("ShowOpen", "", "", 10)]
    public class ShowHumanUnitOpenCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {

        }
    }
}