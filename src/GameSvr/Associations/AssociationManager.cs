using SystemModule;
using SystemModule.Common;

namespace GameSvr
{
    public class AssociationManager
    {
        private readonly IList<Association> GuildList = null;

        public bool AddGuild(string sGuildName, string sChief)
        {
            Association Guild;
            var result = false;
            if (M2Share.CheckGuildName(sGuildName) && FindGuild(sGuildName) == null)
            {
                Guild = new Association(sGuildName);
                Guild.SetGuildInfo(sChief);
                GuildList.Add(Guild);
                SaveGuildList();
                result = true;
            }
            return result;
        }

        public bool DelGuild(string sGuildName)
        {
            Association Guild;
            var result = false;
            for (var i = 0; i < GuildList.Count; i++)
            {
                Guild = GuildList[i];
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

        public AssociationManager()
        {
            GuildList = new List<Association>();
        }

        public Association FindGuild(string sGuildName)
        {
            Association result = null;
            for (var i = 0; i < GuildList.Count; i++)
            {
                if (GuildList[i].sGuildName == sGuildName)
                {
                    result = GuildList[i];
                    break;
                }
            }
            return result;
        }

        public void LoadGuildInfo()
        {
            StringList LoadList;
            Association Guild;
            string sGuildName;
            if (File.Exists(M2Share.g_Config.sGuildFile))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(M2Share.g_Config.sGuildFile);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sGuildName = LoadList[i].Trim();
                    if (sGuildName != "")
                    {
                        Guild = new Association(sGuildName);
                        GuildList.Add(Guild);
                    }
                }
                for (var i = GuildList.Count - 1; i >= 0; i--)
                {
                    Guild = GuildList[i];
                    if (!Guild.LoadGuild())
                    {
                        M2Share.ErrorMessage(Guild.sGuildName + " 读取出错!!!");
                        GuildList.RemoveAt(i);
                        SaveGuildList();
                    }
                }
                M2Share.MainOutMessage($"已读取 [{GuildList.Count}] 个行会信息...", messageColor: ConsoleColor.Green);
            }
            else
            {
                M2Share.ErrorMessage("行会信息文件未找到!!!");
            }
        }

        public Association MemberOfGuild(string sName)
        {
            Association result = null;
            for (var i = 0; i < GuildList.Count; i++)
            {
                if (GuildList[i].IsMember(sName))
                {
                    result = GuildList[i];
                    break;
                }
            }
            return result;
        }

        private void SaveGuildList()
        {
            StringList SaveList;
            if (M2Share.nServerIndex != 0)
            {
                return;
            }
            SaveList = new StringList();
            for (var i = 0; i < GuildList.Count; i++)
            {
                SaveList.Add(GuildList[i].sGuildName);
            }
            try
            {
                SaveList.SaveToFile(M2Share.g_Config.sGuildFile);
            }
            catch
            {
                M2Share.MainOutMessage("行会信息保存失败!!!");
            }
            SaveList = null;
        }

        public void Run()
        {
            Association Guild;
            bool boChanged;
            TWarGuild WarGuild;
            for (var i = 0; i < GuildList.Count; i++)
            {
                Guild = GuildList[i];
                boChanged = false;
                for (var j = Guild.GuildWarList.Count - 1; j >= 0; j--)
                {
                    WarGuild = Guild.GuildWarList[j];
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