using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    [Command("SbkDoorControl", "", 10)]
    public class SbkDoorControlCommand : Command
    {
        [ExecuteCommand]
        public static void SbkDoorControl(PlayObject PlayObject)
        {
        }
    }
}