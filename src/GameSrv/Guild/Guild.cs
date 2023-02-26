using GameSrv.Player;

namespace GameSrv.Guild {
    public class GuildRank {
        public short RankNo;
        public string RankName;
        public IList<GuildMember> MemberList;
    }

    public record struct GuildMember {
        public string MemberName;
        public PlayObject PlayObject;
    }

    public record struct WarGuild {
        public GuildInfo Guild;
        public int dwWarTick;
        public int dwWarTime;
    }
}

