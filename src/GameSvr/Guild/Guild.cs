using GameSvr.Player;

namespace GameSvr.Guild
{
    public class TGuildRank
    {
        public int nRankNo;
        public string sRankName;
        public IList<TGuildMember> MemberList;
    }

    public class TGuildMember
    {
        public string sMemberName;
        public TPlayObject PlayObject;
    }

    public class TWarGuild
    {
        public GuildInfo Guild;
        public int dwWarTick;
        public int dwWarTime;
    }
}

