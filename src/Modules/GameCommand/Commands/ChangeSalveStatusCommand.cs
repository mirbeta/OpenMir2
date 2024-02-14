using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整当前玩家属下状态
    /// </summary>
    [Command("Rest", "调整当前玩家属下状态")]
    public class ChangeSalveStatusCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (PlayerActor.SlaveList == null)
            {
                return;
            }
            PlayerActor.SlaveRelax = !PlayerActor.SlaveRelax;
            if (PlayerActor.SlaveList.Count > 0)
            {
                if (PlayerActor.SlaveRelax)
                {
                    PlayerActor.SysMsg(MessageSettings.PetRest, MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    PlayerActor.SysMsg(MessageSettings.PetAttack, MsgColor.Green, MsgType.Hint);
                }
            }
        }
    }
}