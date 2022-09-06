using NLog;
using SystemModule;
using SystemModule.Common;

namespace GameSvr.Guild
{
    public class GuildManager
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IList<GuildInfo> GuildList;

        public bool AddGuild(string sGuildName, string sChief)
        {
            var result = false;
            if (M2Share.CheckGuildName(sGuildName) && FindGuild(sGuildName) == null)
            {
                var Guild = new GuildInfo(sGuildName);
                Guild.SetGuildInfo(sChief);
                GuildList.Add(Guild);
                SaveGuildList();
                result = true;
            }
            return result;
        }

        public bool DelGuild(string sGuildName)
        {
            var result = false;
            for (var i = 0; i < GuildList.Count; i++)
            {
                var Guild = GuildList[i];
                if (string.Compare(Guild.sGuildName, sGuildName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (Guild.m_RankList.Count > 1)
                    {
                        break;
                    }
                    Guild.BackupGuildFile();
                    GuildList.RemoveAt(i);
                    SaveGuildList();
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void ClearGuildInf()
        {
            for (var i = 0; i < GuildList.Count; i++)
            {
                GuildList[i] = null;
            }
            GuildList.Clear();
        }

        public GuildManager()
        {
            GuildList = new List<GuildInfo>();
        }

        public GuildInfo FindGuild(string sGuildName)
        {
            for (var i = 0; i < GuildList.Count; i++)
            {
                if (GuildList[i].sGuildName == sGuildName)
                {
                    return GuildList[i];
                }
            }
            return null;
        }

        public void LoadGuildInfo()
        {
            GuildInfo Guild;
            if (File.Exists(M2Share.Config.GuildFile))
            {
                var LoadList = new StringList();
                LoadList.LoadFromFile(M2Share.Config.GuildFile);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    var sGuildName = LoadList[i].Trim();
                    if (sGuildName != "")
                    {
                        Guild = new GuildInfo(sGuildName);
                        GuildList.Add(Guild);
                    }
                }
                for (var i = GuildList.Count - 1; i >= 0; i--)
                {
                    Guild = GuildList[i];
                    if (!Guild.LoadGuild())
                    {
                        _logger.Warn(Guild.sGuildName + " 读取出错!!!");
                        GuildList.RemoveAt(i);
                        SaveGuildList();
                    }
                }
                _logger.Info($"已读取 [{GuildList.Count}] 个行会信息...");
            }
            else
            {
                _logger.Error("行会信息文件未找到!!!");
            }
        }

        public GuildInfo MemberOfGuild(string sName)
        {
            for (var i = 0; i < GuildList.Count; i++)
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
            var SaveList = new StringList();
            for (var i = 0; i < GuildList.Count; i++)
            {
                SaveList.Add(GuildList[i].sGuildName);
            }
            try
            {
                SaveList.SaveToFile(M2Share.Config.GuildFile);
            }
            catch
            {
                _logger.Error("行会信息保存失败!!!");
            }
            SaveList = null;
        }

        public void Run()
        {
            for (var i = 0; i < GuildList.Count; i++)
            {
                var Guild = GuildList[i];
                var boChanged = false;
                for (var j = Guild.GuildWarList.Count - 1; j >= 0; j--)
                {
                    var WarGuild = Guild.GuildWarList[j];
                    if ((HUtil32.GetTickCount() - WarGuild.dwWarTick) > WarGuild.dwWarTime)
                    {
                        Guild.EndGuildWar(WarGuild.Guild);
                        Guild.GuildWarList.RemoveAt(j);
                        WarGuild = null;
                        boChanged = true;
                    }
                }
                if (boChanged)
                {
                    Guild.UpdateGuildFile();
                }
                Guild.CheckSaveGuildFile();
            }
        }
    }
}