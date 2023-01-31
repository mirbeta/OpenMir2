using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("ReloadManage", "重新加载脚本", 10)]
    public class ReloadManageCommand : Command
    {
        [ExecuteCommand]
        public static void ReloadManage(PlayObject PlayObject)
        {
            if (M2Share.ManageNPC != null)
            {
                M2Share.ManageNPC.ClearScript();
                M2Share.ManageNPC.LoadNPCScript();
                PlayObject.SysMsg("重新加载登录脚本完成...", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg("重新加载登录脚本失败...", MsgColor.Green, MsgType.Hint);
            }
            if (M2Share.FunctionNPC != null)
            {
                M2Share.FunctionNPC.ClearScript();
                M2Share.FunctionNPC.LoadNPCScript();
                PlayObject.SysMsg("重新加载功能脚本完成...", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg("重新加载功能脚本失败...", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}