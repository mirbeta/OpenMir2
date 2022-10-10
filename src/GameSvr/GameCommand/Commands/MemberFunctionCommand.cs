using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    [Command("MemberFunction", "", help: "打开会员功能窗口", 0)]
    public class MemberFunctionCommand : Command
    {
        [ExecuteCommand]
        public void MemberFunction(PlayObject PlayObject)
        {
            if (M2Share.g_ManageNPC != null)
            {
                PlayObject.m_nScriptGotoCount = 0;
                M2Share.g_ManageNPC.GotoLable(PlayObject, "@Member", false);
            }
        }
    }
}