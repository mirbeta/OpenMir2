using M2Server.Player;

namespace M2Server.GameCommand.Commands {
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