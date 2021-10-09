using GameSvr.CommandSystem;

namespace GameSvr
{
    [GameCommand("GuildWar", "", 10)]
    public class GuildWarCommand : BaseCommond
    {
        [DefaultCommand]
        public void GuildWar(string[] @Parsms, TPlayObject PlayObject)
        {
            if (PlayObject.m_btPermission < 6)
            {
                return;
            }
        }
    }
}