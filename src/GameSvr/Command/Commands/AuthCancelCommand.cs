using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 此命令允许公会取消联盟
    /// </summary>
    [Command("AuthCancel", "", 0)]
    public class AuthCancelCommand : Commond
    {
        [ExecuteCommand]
        public void AuthCancel(string[] @params, PlayObject PlayObject)
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