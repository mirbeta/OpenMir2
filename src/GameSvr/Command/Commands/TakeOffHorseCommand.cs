using GameSvr.CommandSystem;

namespace GameSvr
{
    [GameCommand("TakeOffHorse", desc: "下马命令，在骑马状态输入此命令下马。", 10)]
    public class TakeOffHorseCommand : BaseCommond
    {
        [DefaultCommand]
        public void TakeOffHorse(TPlayObject PlayObject)
        {
            if (!PlayObject.m_boOnHorse)
            {
                return;
            }
            PlayObject.m_boOnHorse = false;
            PlayObject.FeatureChanged();
        }
    }
}