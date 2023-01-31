using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 此命令允许公会取消联盟
    /// </summary>
    [Command("AuthCancel", "", 0)]
    public class AuthCancelCommand : Command
    {
        [ExecuteCommand]
        public static void AuthCancel(string[] @params, PlayObject PlayObject)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            if (PlayObject.IsGuildMaster())
            {
                PlayObject.ClientGuildBreakAlly(@params[0]);
            }
        }
    }
}