using System.Collections.Generic;

namespace GameSvr
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
        public TGuild Guild;
        public int dwWarTick;
        public int dwWarTime;
    }
}

