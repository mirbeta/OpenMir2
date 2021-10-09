using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    [GameCommand("MemberFunction", "", 10)]
    internal class MemberFunctionCommand : BaseCommond
    {
        [DefaultCommand]
        public void MemberFunction(string[] @Params, TPlayObject PlayObject)
        {
            var sParam = @Params.Length > 0 ? @Params[0] : "";

            if (sParam != "" && sParam[1] == '?')
            {
                PlayObject.SysMsg("打开会员功能窗口.", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.g_ManageNPC != null)
            {
                PlayObject.m_nScriptGotoCount = 0;
                M2Share.g_ManageNPC.GotoLable(PlayObject, "@Member", false);
            }
        }
    }
}