using GameSvr.Player;

namespace GameSvr.Guild
{
    public class GuildRank
    {
        public int nRankNo;
        public string sRankName;
        public IList<GuildMember> MemberList;
    }

    public class GuildMember
    {
        public string sMemberName;
        public PlayObject PlayObject;
    }

    public class WarGuild
    {
        public GuildInfo Guild;
        public int dwWarTick;
        public int dwWarTime;
    }
}

