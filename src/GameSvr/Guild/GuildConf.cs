using SystemModule.Common;

namespace GameSvr.Guild
{
    public class GuildConf : ConfigFile
    {
        public GuildConf(string guidName, string fileName) : base(fileName)
        {
            if (!File.Exists(fileName))
            {
                WriteString("Guild", "GuildName", guidName);
            }
            Load();
        }

        public void LoadConfig(GuildInfo guild)
        {
            guild.m_nBuildPoint = ReadWriteInteger("Guild", "BuildPoint", guild.m_nBuildPoint);
            guild.m_nAurae = ReadWriteInteger("Guild", "Aurae", guild.m_nAurae);
            guild.m_nStability = ReadWriteInteger("Guild", "Stability", guild.m_nStability);
            guild.m_nFlourishing = ReadWriteInteger("Guild", "Flourishing", guild.m_nFlourishing);
            guild.m_nChiefItemCount = ReadWriteInteger("Guild", "ChiefItemCount", guild.m_nChiefItemCount);
        }

        public void SaveGuildConfig(GuildInfo guild)
        {
            WriteString("Guild", "GuildName", guild.sGuildName);
            WriteInteger("Guild", "BuildPoint", guild.m_nBuildPoint);
            WriteInteger("Guild", "Aurae", guild.m_nAurae);
            WriteInteger("Guild", "Stability", guild.m_nStability);
            WriteInteger("Guild", "Flourishing", guild.m_nFlourishing);
            WriteInteger("Guild", "ChiefItemCount", guild.m_nChiefItemCount);
        }

    }
}