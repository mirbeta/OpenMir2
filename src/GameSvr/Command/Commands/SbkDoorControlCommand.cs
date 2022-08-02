using GameSvr.CommandSystem;

namespace GameSvr
{
    [GameCommand("SbkDoorControl", "", 10)]
    public class SbkDoorControlCommand : BaseCommond
    {
        [DefaultCommand]
        public void SbkDoorControl(TPlayObject PlayObject)
        {
        }
    }
}