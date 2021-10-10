using GameSvr.CommandSystem;

namespace GameSvr
{
    [GameCommand("Training", "", 10)]
    public class TrainingCommand : BaseCommond
    {
        [DefaultCommand]
        public void Training(TPlayObject PlayObject)
        {
            if (PlayObject.m_btPermission < 6)
            {
                return;
            }
        }
    }
}