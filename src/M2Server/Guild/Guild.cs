using SystemModule;

namespace M2Server.Guild
{
    public class GuildRank
    {
        public short RankNo;
        public string RankName;
        public IList<GuildMember> MemberList;
    }

    public record struct GuildMember
    {
        public string MemberName;
        public IPlayerActor PlayObject;
    }

    public record struct WarGuild
    {
        public IGuild Guild;
        public int dwWarTick;
        public int dwWarTime;
    }
}

