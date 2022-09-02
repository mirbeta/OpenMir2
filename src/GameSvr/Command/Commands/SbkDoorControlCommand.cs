using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    [GameCommand("SbkDoorControl", "", 10)]
    public class SbkDoorControlCommand : BaseCommond
    {
        [DefaultCommand]
        public void SbkDoorControl(PlayObject PlayObject)
        {
        }
    }
}