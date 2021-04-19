using System.Collections.Generic;

namespace M2Server
{
    public class TGuildRank
    {
        public int nRankNo;
        public string sRankName;
        public IList<TPlayObject> MemberList;
    }

    public class TWarGuild
    {
        public TGuild Guild;
        public int dwWarTick;
        public int dwWarTime;
    }
}

