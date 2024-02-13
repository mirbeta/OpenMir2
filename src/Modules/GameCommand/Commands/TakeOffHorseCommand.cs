using SystemModule.Actors;

namespace CommandModule.Commands
{
    [Command("TakeOffHorse", desc: "下马命令，在骑马状态输入此命令下马。", 10)]
    public class TakeOffHorseCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (!PlayerActor.OnHorse)
            {
                return;
            }
            PlayerActor.OnHorse = false;
            PlayerActor.FeatureChanged();
        }
    }
}