using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 此命令用于允许或禁止编组传送功能
    /// </summary>
    [GameCommand("AllowGroupReCall", "此命令用于允许或禁止编组传送功能", 10)]
    public class AllowGroupReCallCommand : BaseCommond
    {
        [DefaultCommand]
        public void AllowGroupReCall(string[] @Params, TPlayObject PlayObject)
        {
            var sParam = @Params.Length > 0 ? @Params[0] : "";
            if (sParam != "" && sParam[0] == '?')
            {
                PlayObject.SysMsg("此命令用于允许或禁止编组传送功能。", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.m_boAllowGroupReCall = !PlayObject.m_boAllowGroupReCall;
            if (PlayObject.m_boAllowGroupReCall) 
            {
                // '[允许天地合一]'
                PlayObject.SysMsg(M2Share.g_sEnableGroupRecall, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else // '[禁止天地合一]'
            {
                PlayObject.SysMsg(M2Share.g_sDisableGroupRecall, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }
    }
}