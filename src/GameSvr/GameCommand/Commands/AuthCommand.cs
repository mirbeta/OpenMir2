using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 此命令允许或禁止公会联盟
    /// </summary>
    [Command("Auth", "", 0)]
    public class AuthCommand : Command
    {
        [ExecuteCommand]
        public void Auth(PlayObject PlayObject)
        {
            if (PlayObject.IsGuildMaster())
            {
                PlayObject.ClientGuildAlly();
            }
        }
    }
}