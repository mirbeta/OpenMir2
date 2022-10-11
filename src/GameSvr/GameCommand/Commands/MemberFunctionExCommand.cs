using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    [Command("MemberFunctionEx", "", help: "打开会员功能窗口", 0)]
    public class MemberFunctionExCommand : Command
    {
        [ExecuteCommand]
        public void MemberFunctionEx(PlayObject PlayObject)
        {
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(PlayObject, "@Member", false);
            }
        }
    }
}