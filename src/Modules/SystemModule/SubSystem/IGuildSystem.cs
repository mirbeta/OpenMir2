using SystemModule.Castles;

namespace SystemModule.SubSystem
{
    public interface IGuildSystem
    {
        bool AddGuild(string sGuildName, string sChief);

        void ClearGuildInf();

        bool DelGuild(string sGuildName);

        IGuild FindGuild(string sGuildName);

        void LoadGuildInfo();

        IGuild MemberOfGuild(string sName);

        void Run();
    }
}
