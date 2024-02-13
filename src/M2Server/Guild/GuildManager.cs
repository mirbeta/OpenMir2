using OpenMir2;
using OpenMir2.Common;
using SystemModule;
using SystemModule.Castles;
using SystemModule.SubSystem;

namespace M2Server.Guild
{
    public class GuildManager : IGuildSystem
    {

        private readonly IList<IGuild> GuildList;

        public GuildManager()
        {
            GuildList = new List<IGuild>();
        }

        public bool AddGuild(string sGuildName, string sChief)
        {
            bool result = false;
            if (M2Share.CheckGuildName(sGuildName) && FindGuild(sGuildName) == null)
            {
                GuildInfo guild = new GuildInfo(sGuildName);
                guild.SetGuildInfo(sChief);
                GuildList.Add(guild);
                SaveGuildList();
                result = true;
            }
            LogService.Debug($"创建行会: {sGuildName} 掌门人: {sChief} 结果: {result}");
            return result;
        }

        public bool DelGuild(string sGuildName)
        {
            bool result = false;
            for (int i = 0; i < GuildList.Count; i++)
            {
                IGuild guild = GuildList[i];
                if (string.Compare(guild.GuildName, sGuildName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (guild.RankList.Count > 1)
                    {
                        break;
                    }
                    guild.BackupGuildFile();
                    GuildList.RemoveAt(i);
                    SaveGuildList();
                    result = true;
                    break;
                }
            }
            LogService.Debug($"删除行会: {sGuildName} 结果: {result}");
            return result;
        }

        public void ClearGuildInf()
        {
            for (int i = 0; i < GuildList.Count; i++)
            {
                GuildList[i] = null;
            }
            GuildList.Clear();
        }

        public IGuild FindGuild(string sGuildName)
        {
            for (int i = 0; i < GuildList.Count; i++)
            {
                if (GuildList[i].GuildName == sGuildName)
                {
                    return GuildList[i];
                }
            }
            return null;
        }

        public void LoadGuildInfo()
        {
            GuildList.Clear();
            if (File.Exists(SystemShare.Config.GuildFile))
            {
                using StringList loadList = new StringList();
                loadList.LoadFromFile(SystemShare.Config.GuildFile);
                for (int i = 0; i < loadList.Count; i++)
                {
                    string sGuildName = loadList[i].Trim();
                    if (!string.IsNullOrEmpty(sGuildName))
                    {
                        GuildInfo guild = new GuildInfo(sGuildName);
                        GuildList.Add(guild);
                    }
                }
                for (int i = GuildList.Count - 1; i >= 0; i--)
                {
                    IGuild guild = GuildList[i];
                    if (!guild.LoadGuild())
                    {
                        LogService.Warn(guild.GuildName + " 读取出错!!!");
                        GuildList.RemoveAt(i);
                        SaveGuildList();
                    }
                }
                LogService.Info($"已读取 [{GuildList.Count}] 个行会信息...");
            }
            else
            {
                LogService.Error("行会信息文件未找到!!!");
            }
        }

        public IGuild MemberOfGuild(string sName)
        {
            for (int i = 0; i < GuildList.Count; i++)
            {
                if (GuildList[i].IsMember(sName))
                {
                    return GuildList[i];
                }
            }
            return null;
        }

        private void SaveGuildList()
        {
            if (M2Share.ServerIndex != 0)
            {
                return;
            }
            StringList saveList = new StringList();
            for (int i = 0; i < GuildList.Count; i++)
            {
                saveList.Add(GuildList[i].GuildName);
            }
            try
            {
                saveList.SaveToFile(SystemShare.Config.GuildFile);
            }
            catch
            {
                LogService.Error("行会信息保存失败!!!");
            }
        }

        public void Run()
        {
            for (int i = 0; i < GuildList.Count; i++)
            {
                IGuild guild = GuildList[i];
                bool boChanged = false;
                for (int j = guild.GuildWarList.Count - 1; j >= 0; j--)
                {
                    SystemModule.Data.WarGuild warGuild = guild.GuildWarList[j];
                    if ((HUtil32.GetTickCount() - warGuild.WarTick) > warGuild.WarTime) //行会战时间到
                    {
                        guild.EndGuildWar(warGuild.Guild);
                        guild.GuildWarList.RemoveAt(j);
                        boChanged = true;
                    }
                }
                if (boChanged)
                {
                    guild.UpdateGuildFile();
                }
                guild.CheckSaveGuildFile();
            }
        }
    }
}