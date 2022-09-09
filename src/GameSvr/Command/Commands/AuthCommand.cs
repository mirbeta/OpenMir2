using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 此命令允许或禁止公会联盟
    /// </summary>
    [GameCommand("Auth", "", 0)]
    public class AuthCommand : BaseCommond
    {
        [DefaultCommand]
        public void Auth(PlayObject PlayObject)
        {
            if (PlayObject.IsGuildMaster())
            {
                PlayObject.ClientGuildAlly();
            }
        }
    }
}