using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    [Command("MemberFunction", "", help: "打开会员功能窗口", 0)]
    public class MemberFunctionCommand : Command
    {
        [ExecuteCommand]
        public static void MemberFunction(PlayObject PlayObject)
        {
            if (M2Share.g_ManageNPC != null)
            {
                PlayObject.ScriptGotoCount = 0;
                M2Share.g_ManageNPC.GotoLable(PlayObject, "@Member", false);
            }
        }
    }
}