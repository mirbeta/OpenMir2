using M2Server.Player;
using M2Server;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands
{
    /// <summary>
    /// 调整当前玩家属下状态
    /// </summary>
    [Command("Rest", "调整当前玩家属下状态")]
    public class ChangeSalveStatusCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(PlayObject playObject)
        {
            if (playObject.SlaveList == null)
            {
                return;
            }
            playObject.SlaveRelax = !playObject.SlaveRelax;
            if (playObject.SlaveList.Count > 0)
            {
                if (playObject.SlaveRelax)
                {
                    playObject.SysMsg(Settings.PetRest, MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    playObject.SysMsg(Settings.PetAttack, MsgColor.Green, MsgType.Hint);
                }
            }
        }
    }
}