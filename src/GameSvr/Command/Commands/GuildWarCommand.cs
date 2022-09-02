using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    [GameCommand("GuildWar", "", 10)]
    public class GuildWarCommand : BaseCommond
    {
        [DefaultCommand]
        public void GuildWar(PlayObject PlayObject)
        {
            if (PlayObject.m_btPermission < 6)
            {
                return;
            }
        }
    }
}