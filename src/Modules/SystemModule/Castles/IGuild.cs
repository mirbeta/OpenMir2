using System.Collections;
using OpenMir2.Data;
using SystemModule.Actors;
using SystemModule.Data;

namespace SystemModule.Castles
{
    public interface IGuild
    {
        string GuildName { get; set; }
        int Aurae { get; set; }
        int BuildPoint { get; set; }
        int ChiefItemCount { get; set; }
        int ContestPoint { get; set; }
        int Count { get; }
        int Flourishing { get; set; }
        bool IsFull { get; }
        int Stability { get; set; }
        bool EnableAuthAlly { get; set; }
        Dictionary<string, DynamicVar> DynamicVarList { get; set; }
        ArrayList NoticeList { get; set; }

        IList<WarGuild> GuildWarList { get; set; }

        IList<IGuild> GuildAllList { get; set; }

        IList<GuildRank> RankList { get; set; }

        void AddMember(IPlayerActor playObject);
        void AddTeamFightMember(string sHumanName);
        WarGuild AddWarGuild(IGuild guild);
        void AllyGuild(IGuild guild);
        void BackupGuildFile();
        bool CancelGuld(string sHumName);
        void CheckSaveGuildFile();
        bool DelAllyGuild(IGuild guild);
        void DelHumanObj(IPlayerActor playObject);
        bool DelMember(string sHumName);
        void EndGuildWar(IGuild guild);
        void EndTeamFight();
        string GetChiefName();
        string GetRankName(IPlayerActor playObject, ref short nRankNo);
        bool IsAllyGuild(IGuild guild);
        bool IsMember(string sName);
        bool IsNotWarGuild(IGuild guild);
        bool IsWarGuild(IGuild guild);
        bool LoadGuild();
        bool LoadGuildFile(string sGuildFileName);
        void RefMemberName();
        void SaveGuildInfoFile();
        void SendGuildMsg(string sMsg);
        bool SetGuildInfo(string sChief);
        void StartTeamFight();
        void TeamFightWhoDead(string sName);
        void TeamFightWhoWinPoint(string sName, int nPoint);
        void UpdateGuildFile();
        int UpdateRank(string sRankData);
    }
}