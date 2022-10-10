using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    [Command("SbkDoorControl", "", 10)]
    public class SbkDoorControlCommand : Command
    {
        [ExecuteCommand]
        public void SbkDoorControl(PlayObject PlayObject)
        {
        }
    }
}