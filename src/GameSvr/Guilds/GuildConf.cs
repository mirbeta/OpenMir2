using System.IO;
using SystemModule.Common;

namespace GameSvr
{
    public class GuildConf : IniFile
    {
        public GuildConf(string guidName, string fileName) : base(fileName)
        {
            if (!File.Exists(fileName))
            {
                WriteString("Guild", "GuildName", guidName);
            }
            Load();
        }

        public void LoadConfig(TGuild guild)
        {
            guild.m_nBuildPoint = ReadInteger("Guild", "BuildPoint", guild.m_nBuildPoint);
            guild.m_nAurae = ReadInteger("Guild", "Aurae", guild.m_nAurae);
            guild.m_nStability = ReadInteger("Guild", "Stability", guild.m_nStability);
            guild.m_nFlourishing = ReadInteger("Guild", "Flourishing", guild.m_nFlourishing);
            guild.m_nChiefItemCount = ReadInteger("Guild", "ChiefItemCount", guild.m_nChiefItemCount);
        }
        
        public void SaveGuildConfig(TGuild guild)
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