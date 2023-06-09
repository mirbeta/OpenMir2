using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("ReloadRobotManage", "重新加载机器人管理列表", 10)]
    public class ReloadRobotManageCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (ModuleShare.RobotNPC != null)
            {
                ModuleShare.RobotNPC.ClearScript();
                ModuleShare.RobotNPC.LoadNPCScript();
                PlayerActor.SysMsg("重新加载机器人专用脚本完成...", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg("重新加载机器人专用脚本失败...", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}