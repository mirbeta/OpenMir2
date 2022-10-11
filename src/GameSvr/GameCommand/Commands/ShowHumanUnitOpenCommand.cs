using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// ShowOpen
    /// </summary>
    [Command("ShowOpen", "", "", 10)]
    public class ShowHumanUnitOpenCommand : Command
    {
        [ExecuteCommand]
        public void ShowOpen(string[] @params, PlayObject playObject)
        {

        }
    }
}