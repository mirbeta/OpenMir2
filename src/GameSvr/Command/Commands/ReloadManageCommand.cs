using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    [GameCommand("ReloadManage", "重新加载脚本", 10)]
    public class ReloadManageCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReloadManage(TPlayObject PlayObject)
        {
            if (M2Share.g_ManageNPC != null)
            {
                M2Share.g_ManageNPC.ClearScript();
                M2Share.g_ManageNPC.LoadNPCScript();
                PlayObject.SysMsg("重新加载登录脚本完成...", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg("重新加载登录脚本失败...", MsgColor.Green, MsgType.Hint);
            }
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.ClearScript();
                M2Share.g_FunctionNPC.LoadNPCScript();
                PlayObject.SysMsg("重新加载功能脚本完成...", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg("重新加载功能脚本失败...", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}