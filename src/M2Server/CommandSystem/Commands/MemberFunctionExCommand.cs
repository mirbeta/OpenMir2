using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    [GameCommand("MemberFunctionEx", "", 10)]
    public class MemberFunctionExCommand : BaseCommond
    {
        [DefaultCommand]
        public void MemberFunctionEx(string[] @Params, TPlayObject PlayObject)
        {
            var sParam = @Params.Length > 0 ? @Params[0] : "";
            if ((sParam != "") && (sParam[0] == '?'))
            {
                PlayObject.SysMsg("打开会员功能窗口.", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(PlayObject, "@Member", false);
            }
        }
    }
}