using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("ReloadRobotManage", "重新加载机器人管理列表", 10)]
    public class ReloadRobotManageCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReloadRobotManage(PlayObject PlayObject)
        {
            if (M2Share.g_RobotNPC != null)
            {
                M2Share.g_RobotNPC.ClearScript();
                M2Share.g_RobotNPC.LoadNPCScript();
                PlayObject.SysMsg("重新加载机器人专用脚本完成...", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg("重新加载机器人专用脚本失败...", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}