using SystemModule;
using SystemModule.Enums;

namespace CommandSystem
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
                    PlayerActor.SysMsg(Settings.PetRest, MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    PlayerActor.SysMsg(Settings.PetAttack, MsgColor.Green, MsgType.Hint);
                }
            }
        }
    }
}