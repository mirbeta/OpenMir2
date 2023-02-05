using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// ShowOpen
    /// </summary>
    [Command("ShowOpen", "", "", 10)]
    public class ShowHumanUnitOpenCommand : GameCommand
    {
        [ExecuteCommand]
        public static void ShowOpen(string[] @params, PlayObject playObject)
        {

        }
    }
}