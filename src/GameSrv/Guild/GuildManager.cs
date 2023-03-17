using NLog;
using SystemModule.Common;

namespace GameSrv.Guild {
    public class GuildManager {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IList<GuildInfo> GuildList;

        public bool AddGuild(string sGuildName, string sChief) {
            bool result = false;
            if (M2Share.CheckGuildName(sGuildName) && FindGuild(sGuildName) == null) {
                GuildInfo Guild = new GuildInfo(sGuildName);
                Guild.SetGuildInfo(sChief);
                GuildList.Add(Guild);
                SaveGuildList();
                result = true;
            }
            return result;
        }

        public bool DelGuild(string sGuildName) {
            bool result = false;
            for (int i = 0; i < GuildList.Count; i++) {
                GuildInfo Guild = GuildList[i];
                if (string.Compare(Guild.GuildName, sGuildName, StringComparison.OrdinalIgnoreCase) == 0) {
                    if (Guild.RankList.Count > 1) {
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

        public void ClearGuildInf() {
            for (int i = 0; i < GuildList.Count; i++) {
                GuildList[i] = null;
            }
            GuildList.Clear();
        }

        public GuildManager() {
            GuildList = new List<GuildInfo>();
        }

        public GuildInfo FindGuild(string sGuildName) {
            for (int i = 0; i < GuildList.Count; i++) {
                if (GuildList[i].GuildName == sGuildName) {
                    return GuildList[i];
                }
            }
            return null;
        }

        public void LoadGuildInfo() {
            GuildList.Clear();
            GuildInfo Guild;
            if (File.Exists(M2Share.Config.GuildFile)) {
                using StringList LoadList = new StringList();
                LoadList.LoadFromFile(M2Share.Config.GuildFile);
                for (int i = 0; i < LoadList.Count; i++) {
                    string sGuildName = LoadList[i].Trim();
                    if (!string.IsNullOrEmpty(sGuildName)) {
                        Guild = new GuildInfo(sGuildName);
                        GuildList.Add(Guild);
                    }
                }
                for (int i = GuildList.Count - 1; i >= 0; i--) {
                    Guild = GuildList[i];
                    if (!Guild.LoadGuild()) {
                        _logger.Warn(Guild.GuildName + " 读取出错!!!");
                        GuildList.RemoveAt(i);
                        SaveGuildList();
                    }
                }
                _logger.Info($"已读取 [{GuildList.Count}] 个行会信息...");
            }
            else {
                _logger.Error("行会信息文件未找到!!!");
            }
        }

        public GuildInfo MemberOfGuild(string sName) {
            for (int i = 0; i < GuildList.Count; i++) {
                if (GuildList[i].IsMember(sName)) {
                    return GuildList[i];
                }
            }
            return null;
        }

        private void SaveGuildList() {
            if (M2Share.ServerIndex != 0) {
                return;
            }
            StringList SaveList = new StringList();
            for (int i = 0; i < GuildList.Count; i++) {
                SaveList.Add(GuildList[i].GuildName);
            }
            try {
                SaveList.SaveToFile(M2Share.Config.GuildFile);
            }
            catch {
                _logger.Error("行会信息保存失败!!!");
            }
        }

        public void Run() {
            for (int i = 0; i < GuildList.Count; i++) {
                GuildInfo Guild = GuildList[i];
                bool boChanged = false;
                for (int j = Guild.GuildWarList.Count - 1; j >= 0; j--) {
                    WarGuild WarGuild = Guild.GuildWarList[j];
                    if ((HUtil32.GetTickCount() - WarGuild.dwWarTick) > WarGuild.dwWarTime) {
                        Guild.EndGuildWar(WarGuild.Guild);
                        Guild.GuildWarList.RemoveAt(j);
                        boChanged = true;
                    }
                }
                if (boChanged) {
                    Guild.UpdateGuildFile();
                }
                Guild.CheckSaveGuildFile();
            }
        }
    }
}