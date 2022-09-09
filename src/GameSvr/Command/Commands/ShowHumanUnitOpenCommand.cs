using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// ShowOpen
    /// </summary>
    [GameCommand("ShowOpen", "", "", 10)]
    public class ShowHumanUnitOpenCommand : BaseCommond
    {
        [DefaultCommand]
        public void ShowOpen(string[] @params, PlayObject playObject)
        {

        }
    }
}