using NLog;
using SystemModule;
using SystemModule.Common;

namespace M2Server.Guild
{
    public class GuildManager
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IList<IGuild> _guildList;

        public GuildManager()
        {
            _guildList = new List<IGuild>();
        }

        public bool AddGuild(string sGuildName, string sChief)
        {
            var result = false;
            if (M2Share.CheckGuildName(sGuildName) && FindGuild(sGuildName) == null)
            {
                var guild = new GuildInfo(sGuildName);
                guild.SetGuildInfo(sChief);
                _guildList.Add(guild);
                SaveGuildList();
                result = true;
            }
            _logger.Debug($"创建行会: {sGuildName} 掌门人: {sChief} 结果: {result}");
            return result;
        }

        public bool DelGuild(string sGuildName)
        {
            var result = false;
            for (var i = 0; i < _guildList.Count; i++)
            {
                var guild = _guildList[i];
                if (string.Compare(guild.GuildName, sGuildName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    //if (guild.RankList.Count > 1)
                    //{
                    //    break;
                    //}
                    guild.BackupGuildFile();
                    _guildList.RemoveAt(i);
                    SaveGuildList();
                    result = true;
                    break;
                }
            }
            _logger.Debug($"删除行会: {sGuildName} 结果: {result}");
            return result;
        }

        public void ClearGuildInf()
        {
            for (var i = 0; i < _guildList.Count; i++)
            {
                _guildList[i] = null;
            }
            _guildList.Clear();
        }

        public IGuild FindGuild(string sGuildName)
        {
            for (var i = 0; i < _guildList.Count; i++)
            {
                if (_guildList[i].GuildName == sGuildName)
                {
                    return _guildList[i];
                }
            }
            return null;
        }

        public void LoadGuildInfo()
        {
            _guildList.Clear();
            if (File.Exists(SystemShare.Config.GuildFile))
            {
                using var loadList = new StringList();
                loadList.LoadFromFile(SystemShare.Config.GuildFile);
                for (var i = 0; i < loadList.Count; i++)
                {
                    var sGuildName = loadList[i].Trim();
                    if (!string.IsNullOrEmpty(sGuildName))
                    {
                        var guild = new GuildInfo(sGuildName);
                        _guildList.Add(guild);
                    }
                }
                for (var i = _guildList.Count - 1; i >= 0; i--)
                {
                    var guild = _guildList[i];
                    if (!guild.LoadGuild())
                    {
                        _logger.Warn(guild.GuildName + " 读取出错!!!");
                        _guildList.RemoveAt(i);
                        SaveGuildList();
                    }
                }
                _logger.Info($"已读取 [{_guildList.Count}] 个行会信息...");
            }
            else
            {
                _logger.Error("行会信息文件未找到!!!");
            }
        }

        public GuildInfo MemberOfGuild(string sName)
        {
            for (var i = 0; i < _guildList.Count; i++)
            {
                //if (_guildList[i].IsMember(sName))
                //{
                //    return _guildList[i];
                //}
            }
            return null;
        }

        private void SaveGuildList()
        {
            if (M2Share.ServerIndex != 0)
            {
                return;
            }
            var saveList = new StringList();
            for (var i = 0; i < _guildList.Count; i++)
            {
                saveList.Add(_guildList[i].GuildName);
            }
            try
            {
                saveList.SaveToFile(SystemShare.Config.GuildFile);
            }
            catch
            {
                _logger.Error("行会信息保存失败!!!");
            }
        }

        public void Run()
        {
            for (var i = 0; i < _guildList.Count; i++)
            {
                var guild = _guildList[i];
                var boChanged = false;
                //for (var j = guild.GuildWarList.Count - 1; j >= 0; j--)
                //{
                //    var warGuild = guild.GuildWarList[j];
                //    if ((HUtil32.GetTickCount() - warGuild.dwWarTick) > warGuild.dwWarTime) //行会战时间到
                //    {
                //        guild.EndGuildWar(warGuild.Guild);
                //        guild.GuildWarList.RemoveAt(j);
                //        boChanged = true;
                //    }
                //}
                if (boChanged)
                {
                    guild.UpdateGuildFile();
                }
                guild.CheckSaveGuildFile();
            }
        }
    }
}