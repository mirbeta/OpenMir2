using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    [GameCommand("MemberFunctionEx", "", help: "打开会员功能窗口", 0)]
    public class MemberFunctionExCommand : BaseCommond
    {
        [DefaultCommand]
        public void MemberFunctionEx(TPlayObject PlayObject)
        {
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(PlayObject, "@Member", false);
            }
        }
    }
}