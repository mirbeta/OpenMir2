using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 自定义命令
    /// </summary>
    [Command("UserCmd", "自定义命令", 10)]
    public class UserCmdCommand : Command
    {
        [ExecuteCommand]
        public static void UserCmd(string[] @Params, PlayObject PlayObject)
        {
            // string sLable = @Params.Length > 0 ? @Params[0] : "";
            // byte Flag = 0;
            // if (PlayObject.m_nUserCmdNPC == M2Share.g_FunctionNPC)
            // {
            //     M2Share.g_FunctionNPC.GotoLable(PlayObject, sLable, false);
            //     Flag = 8;
            // }
            // if (Flag != 8)
            // {
            //     M2Share.g_FunctionNPC.GotoLable(PlayObject, sLable, false);// 执行默认脚本  修复不能执行自定义脚本问题
            // }
            // PlayObject.m_nUserCmdNPC = null;
        }
    }
}