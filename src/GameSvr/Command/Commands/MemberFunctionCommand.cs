using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    [GameCommand("MemberFunction", "", help: "打开会员功能窗口", 0)]
    internal class MemberFunctionCommand : BaseCommond
    {
        [DefaultCommand]
        public void MemberFunction(TPlayObject PlayObject)
        {
            if (M2Share.g_ManageNPC != null)
            {
                PlayObject.m_nScriptGotoCount = 0;
                M2Share.g_ManageNPC.GotoLable(PlayObject, "@Member", false);
            }
        }
    }
}