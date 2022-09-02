using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    [GameCommand("Training", "", 10)]
    public class TrainingCommand : BaseCommond
    {
        [DefaultCommand]
        public void Training(PlayObject PlayObject)
        {
            if (PlayObject.m_btPermission < 6)
            {
                return;
            }
        }
    }
}