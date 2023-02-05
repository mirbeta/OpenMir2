using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    [Command("SbkDoorControl", "", 10)]
    public class SbkDoorControlCommand : GameCommand
    {
        [ExecuteCommand]
        public static void SbkDoorControl(PlayObject PlayObject)
        {
        }
    }
}