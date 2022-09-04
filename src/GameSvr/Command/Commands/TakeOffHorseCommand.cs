using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    [GameCommand("TakeOffHorse", desc: "下马命令，在骑马状态输入此命令下马。", 10)]
    public class TakeOffHorseCommand : BaseCommond
    {
        [DefaultCommand]
        public void TakeOffHorse(PlayObject PlayObject)
        {
            if (!PlayObject.OnHorse)
            {
                return;
            }
            PlayObject.OnHorse = false;
            PlayObject.FeatureChanged();
        }
    }
}