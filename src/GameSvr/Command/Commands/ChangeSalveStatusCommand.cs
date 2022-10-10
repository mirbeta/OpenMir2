using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整当前玩家属下状态
    /// </summary>
    [Command("Rest", "调整当前玩家属下状态", 0)]
    public class ChangeSalveStatusCommand : Commond
    {
        [ExecuteCommand]
        public void ChangeSalveStatus(PlayObject PlayObject)
        {
            PlayObject.SlaveRelax = !PlayObject.SlaveRelax;
            if (PlayObject.SlaveList.Count > 0)
            {
                if (PlayObject.SlaveRelax)
                {
                    PlayObject.SysMsg(M2Share.sPetRest, MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    PlayObject.SysMsg(M2Share.sPetAttack, MsgColor.Green, MsgType.Hint);
                }
            }
        }
    }
}