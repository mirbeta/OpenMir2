using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("ReloadRobotManage", "重新加载机器人管理列表", 10)]
    public class ReloadRobotManageCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject)
        {
            if (M2Share.RobotNPC != null)
            {
                M2Share.RobotNPC.ClearScript();
                M2Share.RobotNPC.LoadNPCScript();
                PlayObject.SysMsg("重新加载机器人专用脚本完成...", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg("重新加载机器人专用脚本失败...", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}