using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("ReloadManage", "重新加载脚本", 10)]
    public class ReloadManageCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (SystemShare.ManageNPC != null)
            {
                SystemShare.ManageNPC.ClearScript();
                SystemShare.ManageNPC.LoadNPCScript();
                PlayerActor.SysMsg("重新加载登录脚本完成...", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg("重新加载登录脚本失败...", MsgColor.Green, MsgType.Hint);
            }
            if (SystemShare.FunctionNPC != null)
            {
                SystemShare.FunctionNPC.ClearScript();
                SystemShare.FunctionNPC.LoadNPCScript();
                PlayerActor.SysMsg("重新加载功能脚本完成...", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg("重新加载功能脚本失败...", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}