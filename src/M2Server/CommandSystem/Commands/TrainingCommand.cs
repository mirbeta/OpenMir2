using M2Server.CommandSystem;

namespace M2Server
{
    [GameCommand("Training", "", 10)]
    public class TrainingCommand : BaseCommond
    {
        [DefaultCommand]
        public void Training(string[] @Params, TPlayObject PlayObject)
        {
            if (PlayObject.m_btPermission < 6)
            {
                return;
            }
        }
    }
}