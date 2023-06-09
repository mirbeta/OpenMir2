namespace SystemModule
{
    public interface IGuildSystem
    {
        bool AddGuild(string sGuildName, string sChief);

        void ClearGuildInf();

        bool DelGuild(string sGuildName);

        IGuild FindGuild(string sGuildName);

        void LoadGuildInfo();

        GuildInfo MemberOfGuild(string sName);

        void Run();
    }
}
