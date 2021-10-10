using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    [GameCommand("MemberFunction", "",help:"打开会员功能窗口", 10)]
    internal class MemberFunctionCommand : BaseCommond
    {
        [DefaultCommand]
        public void MemberFunction(string[] @Params, TPlayObject PlayObject)
        {
            if (M2Share.g_ManageNPC != null)
            {
                PlayObject.m_nScriptGotoCount = 0;
                M2Share.g_ManageNPC.GotoLable(PlayObject, "@Member", false);
            }
        }
    }
}