using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    [GameCommand("ReloadManage", "重新加载脚本", 10)]
    public class ReloadManageCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReloadManage(string[] @params, TPlayObject PlayObject)
        {
            var sParam = @params.Length > 0 ? @params[0] : "";
            if (sParam != "" && sParam[1] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam == "")
            {
                if (M2Share.g_ManageNPC != null)
                {
                    M2Share.g_ManageNPC.ClearScript();
                    M2Share.g_ManageNPC.LoadNPCScript();
                    PlayObject.SysMsg("重新加载登录脚本完成...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    PlayObject.SysMsg("重新加载登录脚本失败...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            else
            {
                if (M2Share.g_FunctionNPC != null)
                {
                    M2Share.g_FunctionNPC.ClearScript();
                    M2Share.g_FunctionNPC.LoadNPCScript();
                    PlayObject.SysMsg("重新加载功能脚本完成...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    PlayObject.SysMsg("重新加载功能脚本失败...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
        }
    }
}