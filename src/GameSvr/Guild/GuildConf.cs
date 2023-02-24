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
            guild.MNBuildPoint = ReadWriteInteger("Guild", "BuildPoint", guild.MNBuildPoint);
            guild.MNAurae = ReadWriteInteger("Guild", "Aurae", guild.MNAurae);
            guild.MNStability = ReadWriteInteger("Guild", "Stability", guild.MNStability);
            guild.MNFlourishing = ReadWriteInteger("Guild", "Flourishing", guild.MNFlourishing);
            guild.MNChiefItemCount = ReadWriteInteger("Guild", "ChiefItemCount", guild.MNChiefItemCount);
        }

        public void SaveGuildConfig(GuildInfo guild)
        {
            WriteString("Guild", "GuildName", guild.SGuildName);
            WriteInteger("Guild", "BuildPoint", guild.MNBuildPoint);
            WriteInteger("Guild", "Aurae", guild.MNAurae);
            WriteInteger("Guild", "Stability", guild.MNStability);
            WriteInteger("Guild", "Flourishing", guild.MNFlourishing);
            WriteInteger("Guild", "ChiefItemCount", guild.MNChiefItemCount);
        }

    }
}