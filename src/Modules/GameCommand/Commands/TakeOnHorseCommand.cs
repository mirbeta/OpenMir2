using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    [Command("TakeOnHorse", "", 10)]
    public class TakeOnHorseCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (PlayerActor.OnHorse)
            {
                return;
            }
            if (PlayerActor.HorseType == 0)
            {
                PlayerActor.SysMsg("骑马必须先戴上马牌!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayerActor.OnHorse = true;
            PlayerActor.FeatureChanged();
            if (PlayerActor.OnHorse)
            {
                SystemShare.FunctionNPC.GotoLable(PlayerActor, "@OnHorse", false);
            }
        }
    }
}