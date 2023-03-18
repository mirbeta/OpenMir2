using SystemModule.Common;

namespace GameSrv.Guild
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
            guild.BuildPoint = ReadWriteInteger("Guild", "BuildPoint", guild.BuildPoint);
            guild.Aurae = ReadWriteInteger("Guild", "Aurae", guild.Aurae);
            guild.Stability = ReadWriteInteger("Guild", "Stability", guild.Stability);
            guild.Flourishing = ReadWriteInteger("Guild", "Flourishing", guild.Flourishing);
            guild.ChiefItemCount = ReadWriteInteger("Guild", "ChiefItemCount", guild.ChiefItemCount);
        }

        public void SaveGuildConfig(GuildInfo guild)
        {
            WriteString("Guild", "GuildName", guild.GuildName);
            WriteInteger("Guild", "BuildPoint", guild.BuildPoint);
            WriteInteger("Guild", "Aurae", guild.Aurae);
            WriteInteger("Guild", "Stability", guild.Stability);
            WriteInteger("Guild", "Flourishing", guild.Flourishing);
            WriteInteger("Guild", "ChiefItemCount", guild.ChiefItemCount);
        }
    }
}