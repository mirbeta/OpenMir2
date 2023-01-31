using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 调整当前玩家属下状态
    /// </summary>
    [Command("Rest", "调整当前玩家属下状态", 0)]
    public class ChangeSalveStatusCommand : Command
    {
        [ExecuteCommand]
        public static void ChangeSalveStatus(PlayObject PlayObject)
        {
            PlayObject.SlaveRelax = !PlayObject.SlaveRelax;
            if (PlayObject.SlaveList.Count > 0)
            {
                if (PlayObject.SlaveRelax)
                {
                    PlayObject.SysMsg(Settings.sPetRest, MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    PlayObject.SysMsg(Settings.sPetAttack, MsgColor.Green, MsgType.Hint);
                }
            }
        }
    }
}