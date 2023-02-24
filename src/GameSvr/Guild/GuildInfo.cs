using GameSvr.Actor;
using GameSvr.Player;
using System.Collections;
using SystemModule.Common;
using SystemModule.Data;

namespace GameSvr.Guild
{
    /// <summary>
    /// 公会系统
    /// </summary>
    public class GuildInfo
    {
        public int Count => GetMemberCount();

        public bool IsFull => GetMemgerIsFull();

        public int BuildPoint
        {
            get => buildPoint;
            set => SetBuildPoint(value);
        }

        public int Aurae
        {
            get => aurae;
            set => SetAuraePoint(value);
        }

        public int Stability
        {
            get => stability;
            set => SetStabilityPoint(value);
        }

        public int Flourishing
        {
            get => flourishing;
            set => SetFlourishPoint(value);
        }

        public int ChiefItemCount
        {
            get => chiefItemCount;
            set => SetChiefItemCount(value);
        }

        /// <summary>
        /// 行会名称
        /// </summary>
        public string GuildName;
        /// <summary>
        /// 行会公告
        /// </summary>
        public ArrayList NoticeList;
        public IList<WarGuild> GuildWarList;
        public IList<GuildInfo> GuildAllList;
        /// <summary>
        /// 职位列表
        /// </summary>
        public IList<GuildRank> MRankList;
        public int ContestPoint;
        public bool BoTeamFight;
        public ArrayList TeamFightDeadList;
        /// <summary>
        /// 是否允许行会联盟
        /// </summary>
        public bool EnableAuthAlly;
        public int DwSaveTick;
        public bool BoChanged;
        /// <summary>
        /// 行会变量
        /// </summary>
        public Dictionary<string, DynamicVar> DynamicVarList;
        /// <summary>
        /// 建筑度
        /// </summary>
        private int buildPoint;
        /// <summary>
        /// 人气度
        /// </summary>        
        private int aurae;
        /// <summary>
        /// 安定度
        /// </summary>        
        private int stability;
        /// <summary>
        /// 繁荣度
        /// </summary>        
        private int flourishing;
        private int chiefItemCount;
        private readonly GuildConf _guildConf;

        private void ClearRank()
        {
            for (var i = 0; i < MRankList.Count; i++)
            {
                MRankList[i] = null;
            }
            MRankList.Clear();
        }

        public GuildInfo(string sName)
        {
            GuildName = sName;
            NoticeList = new ArrayList();
            GuildWarList = new List<WarGuild>();
            GuildAllList = new List<GuildInfo>();
            MRankList = new List<GuildRank>();
            TeamFightDeadList = new ArrayList();
            DwSaveTick = 0;
            BoChanged = false;
            ContestPoint = 0;
            BoTeamFight = false;
            EnableAuthAlly = false;
            buildPoint = 0;
            aurae = 0;
            stability = 0;
            flourishing = 0;
            chiefItemCount = 0;
            DynamicVarList = new Dictionary<string, DynamicVar>(StringComparer.OrdinalIgnoreCase);
            var sFileName = Path.Combine(M2Share.Config.GuildDir, string.Concat(sName + ".ini"));
            _guildConf = new GuildConf(sName, sFileName);
        }

        public bool DelAllyGuild(GuildInfo guild)
        {
            var result = false;
            GuildInfo allyGuild;
            for (var i = 0; i < GuildAllList.Count; i++)
            {
                allyGuild = GuildAllList[i];
                if (allyGuild == guild)
                {
                    GuildAllList.RemoveAt(i);
                    result = true;
                    break;
                }
            }
            SaveGuildInfoFile();
            return result;
        }

        public bool IsAllyGuild(GuildInfo guild)
        {
            var result = false;
            GuildInfo allyGuild;
            for (var i = 0; i < GuildAllList.Count; i++)
            {
                allyGuild = GuildAllList[i];
                if (allyGuild == guild)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public bool IsMember(string sName)
        {
            var result = false;
            for (var i = 0; i < MRankList.Count; i++)
            {
                GuildRank guildRank = MRankList[i];
                for (var j = 0; j < guildRank.MemberList.Count; j++)
                {
                    if (string.IsNullOrEmpty(guildRank.MemberList[j].MemberName))
                    {
                        continue;
                    }
                    if (guildRank.MemberList[j].MemberName == sName)
                    {
                        result = true;
                        return result;
                    }
                }
            }
            return result;
        }

        public bool IsWarGuild(GuildInfo guild)
        {
            var result = false;
            for (var i = 0; i < GuildWarList.Count; i++)
            {
                if (GuildWarList[i].Guild == guild)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public bool LoadGuild()
        {
            var sFileName = GuildName + ".txt";
            var result = LoadGuildFile(sFileName);
            LoadGuildConfig();
            return result;
        }

        private void LoadGuildConfig()
        {
            _guildConf.LoadConfig(this);
        }

        public bool LoadGuildFile(string sGuildFileName)
        {
            StringList loadList;
            var s1C = string.Empty;
            var s20 = string.Empty;
            int n28;
            WarGuild guildWar;
            GuildInfo guild;
            GuildRank guildRank = null;
            var sFileName = Path.Combine(M2Share.Config.GuildDir, sGuildFileName);
            if (!File.Exists(sFileName))
            {
                return false;
            }
            ClearRank();
            NoticeList.Clear();
            for (var i = 0; i < GuildWarList.Count; i++)
            {
                GuildWarList[i] = default(WarGuild);
            }
            GuildWarList.Clear();
            GuildAllList.Clear();
            n28 = 0;
            short n2C = 0;
            var s24 = "";
            loadList = new StringList();
            loadList.LoadFromFile(sFileName);
            for (var i = 0; i < loadList.Count; i++)
            {
                var s18 = loadList[i];
                if (s18 == "" || s18[0] == ';')
                {
                    continue;
                }
                if (s18[0] != '+')
                {
                    if (s18 == M2Share.Config.GuildNotice)
                    {
                        n28 = 1;
                    }
                    if (s18 == M2Share.Config.GuildWar)
                    {
                        n28 = 2;
                    }
                    if (s18 == M2Share.Config.GuildAll)
                    {
                        n28 = 3;
                    }
                    if (s18 == M2Share.Config.GuildMember)
                    {
                        n28 = 4;
                    }
                    if (s18[0] == '#')
                    {
                        s18 = s18.AsSpan()[1..].ToString();
                        s18 = HUtil32.GetValidStr3(s18, ref s1C, new[] { ' ', ',' });
                        n2C = HUtil32.StrToInt16(s1C, 0);
                        s24 = s18.Trim();
                        guildRank = null;
                    }
                    continue;
                }
                s18 = s18.AsSpan()[1..].ToString();
                switch (n28)
                {
                    case 1:
                        NoticeList.Add(s18);
                        break;
                    case 2:
                        while (!string.IsNullOrEmpty(s18))
                        {
                            s18 = HUtil32.GetValidStr3(s18, ref s1C, new[] { ' ', ',' });
                            if (s1C == "")
                            {
                                break;
                            }
                            guildWar = new WarGuild
                            {
                                Guild = M2Share.GuildMgr.FindGuild(s1C)
                            };
                            if (guildWar.Guild != null)
                            {
                                guildWar.dwWarTick = HUtil32.GetTickCount();
                                guildWar.dwWarTime = HUtil32.StrToInt(s20.Trim(), 0);
                                GuildWarList.Add(guildWar);
                            }
                            else
                            {
                            }
                        }
                        break;
                    case 3:
                        while (!string.IsNullOrEmpty(s18))
                        {
                            s18 = HUtil32.GetValidStr3(s18, ref s1C, new[] { ' ', ',' });
                            s18 = HUtil32.GetValidStr3(s18, ref s20, new[] { ' ', ',' });
                            if (s1C == "")
                            {
                                break;
                            }
                            guild = M2Share.GuildMgr.FindGuild(s1C);
                            if (guild != null)
                            {
                                GuildAllList.Add(guild);
                            }
                        }
                        break;
                    case 4:
                        if (n2C > 0 && !string.IsNullOrEmpty(s24))
                        {
                            if (s24.Length > 30)
                            {
                                s24 = s24[..M2Share.Config.GuildRankNameLen];//限制职倍的长度
                            }
                            if (guildRank == null)
                            {
                                guildRank = new GuildRank
                                {
                                    RankNo = n2C,
                                    RankName = s24,
                                    MemberList = new List<GuildMember>()
                                };
                                MRankList.Add(guildRank);
                            }
                            while (!string.IsNullOrEmpty(s18))
                            {
                                s18 = HUtil32.GetValidStr3(s18, ref s1C, new[] { ' ', ',' });
                                if (string.IsNullOrEmpty(s1C))
                                {
                                    break;
                                }
                                var playObject = M2Share.WorldEngine.GetPlayObject(s1C);
                                guildRank.MemberList.Add(new GuildMember()
                                {
                                    MemberName = s1C,
                                    PlayObject = playObject
                                });
                            }
                        }
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// 刷新封号名称
        /// </summary>
        public void RefMemberName()
        {
            GuildRank guildRank;
            BaseObject baseObject;
            for (var i = 0; i < MRankList.Count; i++)
            {
                guildRank = MRankList[i];
                for (var j = 0; j < guildRank.MemberList.Count; j++)
                {
                    baseObject = guildRank.MemberList[j].PlayObject;
                    if (baseObject != null)
                    {
                        baseObject.RefShowName();
                    }
                }
            }
        }

        public void SaveGuildInfoFile()
        {
            if (M2Share.ServerIndex == 0)
            {
                SaveGuildFile(Path.Combine(M2Share.Config.GuildDir, string.Concat(GuildName, ".txt")));
                SaveGuildConfig();
            }
            else
            {
                SaveGuildFile(Path.Combine(M2Share.Config.GuildDir, GuildName, ".", M2Share.ServerIndex.ToString()));
            }
        }

        private void SaveGuildConfig()
        {
            _guildConf.SaveGuildConfig(this);
        }

        private void SaveGuildFile(string sFileName)
        {
            var saveList = new StringList();
            saveList.Add(M2Share.Config.GuildNotice);
            for (var i = 0; i < NoticeList.Count; i++)
            {
                saveList.Add("+" + NoticeList[i]);
            }
            saveList.Add(" ");
            saveList.Add(M2Share.Config.GuildWar);
            for (var i = 0; i < GuildWarList.Count; i++)
            {
                var warGuild = GuildWarList[i];
                long n14 = warGuild.dwWarTime - (HUtil32.GetTickCount() - warGuild.dwWarTick);
                if (n14 <= 0)
                {
                    continue;
                }
                saveList.Add("+" + GuildWarList[i].Guild.GuildName + ' ' + n14);
            }
            saveList.Add(" ");
            saveList.Add(M2Share.Config.GuildAll);
            for (var i = 0; i < GuildAllList.Count; i++)
            {
                saveList.Add("+" + GuildAllList[i]);
            }
            saveList.Add(" ");
            saveList.Add(M2Share.Config.GuildMember);
            for (var i = 0; i < MRankList.Count; i++)
            {
                var guildRank = MRankList[i];
                saveList.Add("#" + guildRank.RankNo + ' ' + guildRank.RankName);
                for (var j = 0; j < guildRank.MemberList.Count; j++)
                {
                    saveList.Add("+" + guildRank.MemberList[j].MemberName);
                }
            }
            try
            {
                saveList.SaveToFile(sFileName);
            }
            catch
            {
                M2Share.Logger.Error("保存行会信息失败!!! " + sFileName);
            }
        }

        public void SendGuildMsg(string sMsg)
        {
            try
            {
                if (M2Share.Config.ShowPreFixMsg)
                {
                    sMsg = M2Share.Config.GuildMsgPreFix + sMsg;
                }
                for (var i = 0; i < MRankList.Count; i++)
                {
                    var guildRank = MRankList[i];
                    for (var j = 0; j < guildRank.MemberList.Count; j++)
                    {
                        var guildMember = guildRank.MemberList[j].PlayObject;
                        if (guildMember == null)
                        {
                            continue;
                        }
                        if (guildMember.BanGuildChat)
                        {
                            guildMember.SendMsg(guildMember, Messages.RM_GUILDMESSAGE, 0, M2Share.Config.GuildMsgFColor, M2Share.Config.GuildMsgBColor, 0, sMsg);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.Logger.Error("[Exceptiion] TGuild.SendGuildMsg GuildName = " + GuildName + " Msg = " + sMsg);
                M2Share.Logger.Error(e.Message);
            }
        }

        public bool SetGuildInfo(string sChief)
        {
            GuildRank guildRank;
            if (MRankList.Count == 0)
            {
                guildRank = new GuildRank
                {
                    RankNo = 1,
                    RankName = M2Share.Config.GuildChief,
                    MemberList = new List<GuildMember>()
                };
                var playObject = M2Share.WorldEngine.GetPlayObject(sChief);
                guildRank.MemberList.Add(new GuildMember() { MemberName = sChief, PlayObject = playObject });
                MRankList.Add(guildRank);
                SaveGuildInfoFile();
            }
            return true;
        }

        public string GetRankName(PlayObject playObject, ref short nRankNo)
        {
            var result = string.Empty;
            for (var i = 0; i < MRankList.Count; i++)
            {
                var guildRank = MRankList[i];
                for (var j = 0; j < guildRank.MemberList.Count; j++)
                {
                    if (string.Compare(guildRank.MemberList[j].MemberName, playObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        //guildRank.MemberList[j].PlayObject = PlayObject;
                        nRankNo = guildRank.RankNo;
                        result = guildRank.RankName;
                        // PlayObject.RefShowName();
                        playObject.SendMsg(playObject, Messages.RM_CHANGEGUILDNAME, 0, 0, 0, 0, "");
                        return result;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取行会掌门人
        /// </summary>
        /// <returns></returns>
        public string GetChiefName()
        {
            if (MRankList.Count <= 0)
            {
                return string.Empty;
            }
            var guildRank = MRankList[0];
            if (guildRank.MemberList.Count <= 0)
            {
                return string.Empty;
            }
            return guildRank.MemberList[0].MemberName;
        }

        public void CheckSaveGuildFile()
        {
            if (BoChanged && (HUtil32.GetTickCount() - DwSaveTick) > 30 * 1000)
            {
                BoChanged = false;
                SaveGuildInfoFile();
            }
        }

        public void DelHumanObj(PlayObject playObject)
        {
            CheckSaveGuildFile();
            for (var i = 0; i < MRankList.Count; i++)
            {
                var guildRank = MRankList[i];
                for (var j = 0; j < guildRank.MemberList.Count; j++)
                {
                    if (string.Compare(guildRank.MemberList[j].MemberName, playObject.ChrName, StringComparison.OrdinalIgnoreCase) == 1)
                    {
                        guildRank.MemberList[j] = default(GuildMember);
                        return;
                    }
                }
            }
        }

        public void TeamFightWhoDead(string sName)
        {
            if (!BoTeamFight)
            {
                return;
            }
            //for (var I = 0; I < TeamFightDeadList.Count; I++)
            //{
            //    if (TeamFightDeadList[I] == sName)
            //    {
            //        n10 = ((int)TeamFightDeadList.Values[I]);
            //        TeamFightDeadList.Values[I] = ((MakeLong(LoWord(n10) + 1, HiWord(n10))) as Object);
            //    }
            //}
        }

        public void TeamFightWhoWinPoint(string sName, int nPoint)
        {
            if (!BoTeamFight)
            {
                return;
            }
            ContestPoint += nPoint;
            //for (var i = 0; i < TeamFightDeadList.Count; i ++ )
            //{
            //    if (TeamFightDeadList[i] == sName)
            //    {
            //        n14 = TeamFightDeadList[i];
            //        TeamFightDeadList[i] = ((MakeLong(LoWord(n14), HiWord(n14) + nPoint)) as Object);
            //    }
            //}
        }

        public void UpdateGuildFile()
        {
            BoChanged = true;
            DwSaveTick = HUtil32.GetTickCount();
            SaveGuildInfoFile();
        }

        public void BackupGuildFile()
        {
            PlayObject playObject;
            GuildRank guildRank;
            if (M2Share.ServerIndex == 0)
            {
                SaveGuildFile(Path.Combine(M2Share.Config.GuildDir, GuildName, '.' + HUtil32.GetTickCount() + ".bak"));
            }
            for (var i = 0; i < MRankList.Count; i++)
            {
                guildRank = MRankList[i];
                for (var j = 0; j < guildRank.MemberList.Count; j++)
                {
                    playObject = guildRank.MemberList[j].PlayObject;
                    if (playObject != null)
                    {
                        playObject.MyGuild = null;
                        playObject.RefRankInfo(0, "");
                        playObject.RefShowName();
                    }
                }
                guildRank.MemberList = null;
            }
            MRankList.Clear();
            NoticeList.Clear();
            for (var i = 0; i < GuildWarList.Count; i++)
            {
                GuildWarList[i] = default(WarGuild);
            }
            GuildWarList.Clear();
            GuildAllList.Clear();
            SaveGuildInfoFile();
        }

        public void AddMember(PlayObject playObject)
        {
            GuildRank guildRank;
            GuildRank guildRank18 = null;
            for (var i = 0; i < MRankList.Count; i++)
            {
                guildRank = MRankList[i];
                if (guildRank.RankNo == 99)
                {
                    guildRank18 = guildRank;
                    break;
                }
            }
            if (guildRank18 == null)
            {
                guildRank18 = new GuildRank
                {
                    RankNo = 99,
                    RankName = M2Share.Config.GuildMemberRank,
                    MemberList = new List<GuildMember>()
                };
                MRankList.Add(guildRank18);
            }
            guildRank18.MemberList.Add(new GuildMember()
            {
                PlayObject = playObject,
                MemberName = playObject.ChrName
            });
            UpdateGuildFile();
        }

        public bool DelMember(string sHumName)
        {
            var result = false;
            for (var i = 0; i < MRankList.Count; i++)
            {
                var guildRank = MRankList[i];
                for (var j = 0; j < guildRank.MemberList.Count; j++)
                {
                    if (guildRank.MemberList[j].MemberName == sHumName)
                    {
                        guildRank.MemberList.RemoveAt(j);
                        result = true;
                        break;
                    }
                }
                if (result)
                {
                    break;
                }
            }
            if (result)
            {
                UpdateGuildFile();
            }
            return result;
        }

        public bool CancelGuld(string sHumName)
        {
            if (MRankList.Count != 1)
            {
                return false;
            }
            var guildRank = MRankList[0];
            if (guildRank.MemberList.Count != 1)
            {
                return false;
            }
            if (string.Compare(guildRank.MemberList[0].MemberName, sHumName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                BackupGuildFile();
                return true;
            }
            return false;
        }

        private static void ClearRankList(ref IList<GuildRank> rankList)
        {
            for (var i = 0; i < rankList.Count; i++)
            {
                var guildRank = rankList[i];
                guildRank.MemberList = null;
            }
        }

        public int UpdateRank(string sRankData)
        {
            GuildRank newGuildRank;
            var sRankInfo = string.Empty;
            var sRankNo = string.Empty;
            var sRankName = string.Empty;
            var sMemberName = string.Empty;
            int n28;
            int n2C;
            int n30;
            bool boCheckChange;
            IList<GuildRank> guildRankList = new List<GuildRank>();
            GuildRank guildRank = null;
            while (true)
            {
                if (string.IsNullOrEmpty(sRankData))
                {
                    break;
                }
                sRankData = HUtil32.GetValidStr3(sRankData, ref sRankInfo, '\r');
                sRankInfo = sRankInfo.Trim();
                if (string.IsNullOrEmpty(sRankInfo))
                {
                    continue;
                }
                if (sRankInfo[0] == '#')// 取得职称的名称
                {
                    sRankInfo = sRankInfo.AsSpan()[1..].ToString();
                    sRankInfo = HUtil32.GetValidStr3(sRankInfo, ref sRankNo, new[] { ' ', '<' });
                    sRankInfo = HUtil32.GetValidStr3(sRankInfo, ref sRankName, new[] { '<', '>' });
                    if (sRankName.Length > 30)
                    {
                        sRankName = sRankName[..30]; // 限制只为名字的长度
                    }
                    if (guildRank != null)
                    {
                        guildRankList.Add(guildRank);
                    }
                    guildRank = new GuildRank
                    {
                        RankNo = HUtil32.StrToInt16(sRankNo, 99),
                        RankName = sRankName.Trim(),
                        MemberList = new List<GuildMember>()
                    };
                    continue;
                }
                if (guildRank == null)
                {
                    continue;
                }
                var count = 0;
                while (true)
                {
                    // 将成员名称加入职称表里
                    if (string.IsNullOrEmpty(sRankInfo))
                    {
                        break;
                    }
                    sRankInfo = HUtil32.GetValidStr3(sRankInfo, ref sMemberName, new[] { ' ', ',' });
                    if (!string.IsNullOrEmpty(sMemberName))
                    {
                        guildRank.MemberList.Add(new GuildMember()
                        {
                            PlayObject = M2Share.WorldEngine.GetPlayObject(sMemberName),
                            MemberName = sMemberName
                        });
                    }
                    count++;
                    if (count >= 10)
                    {
                        break;
                    }
                }
            }
            if (guildRank != null)
            {
                guildRankList.Add(guildRank);
            }
            int result;
            // 校验成员列表是否有改变，如果未修改则退出
            if (MRankList.Count == guildRankList.Count)
            {
                boCheckChange = true;
                for (var i = 0; i < MRankList.Count; i++)
                {
                    guildRank = MRankList[i];
                    newGuildRank = guildRankList[i];
                    if (guildRank.RankNo == newGuildRank.RankNo && guildRank.RankName == newGuildRank.RankName && guildRank.MemberList.Count == newGuildRank.MemberList.Count)
                    {
                        for (var j = 0; j < guildRank.MemberList.Count; j++)
                        {
                            if (guildRank.MemberList[j] != newGuildRank.MemberList[j])
                            {
                                boCheckChange = false; // 如果有改变则将其置为FALSE
                                break;
                            }
                        }
                    }
                    else
                    {
                        boCheckChange = false;
                        break;
                    }
                }
                if (boCheckChange)
                {
                    result = -1;
                    ClearRankList(ref guildRankList);
                    return result;
                }
            }
            // 检查行会掌门职业是否为空
            result = -2;
            if (guildRankList.Count > 0)
            {
                guildRank = guildRankList[0];
                if (guildRank.RankNo == 1)
                {
                    if (!string.IsNullOrEmpty(guildRank.RankName))
                    {
                        result = 0;
                    }
                    else
                    {
                        result = -3;
                    }
                }
            }
            // 检查行会掌门人是否在线(？？？)
            if (result == 0)
            {
                guildRank = guildRankList[0];
                if (guildRank.MemberList.Count <= 2)
                {
                    n28 = guildRank.MemberList.Count;
                    for (var i = 0; i < guildRank.MemberList.Count; i++)
                    {
                        if (M2Share.WorldEngine.GetPlayObject(guildRank.MemberList[i].MemberName) == null)
                        {
                            n28 -= 1;
                            break;
                        }
                    }
                    if (n28 <= 0)
                    {
                        result = -5;
                    }
                }
                else
                {
                    result = -4;
                }
            }
            if (result == 0)
            {
                n2C = 0;
                n30 = 0;
                for (var i = 0; i < MRankList.Count; i++)
                {
                    guildRank = MRankList[i];
                    boCheckChange = true;
                    for (var j = 0; j < guildRank.MemberList.Count; j++)
                    {
                        boCheckChange = false;
                        sMemberName = guildRank.MemberList[j].MemberName;
                        n2C++;
                        for (var k = 0; k < guildRankList.Count; k++)
                        {
                            // 搜索新列表
                            newGuildRank = guildRankList[k];
                            for (n28 = 0; n28 < newGuildRank.MemberList.Count; n28++)
                            {
                                if (newGuildRank.MemberList[n28].MemberName == sMemberName)
                                {
                                    boCheckChange = true;
                                    break;
                                }
                            }
                            if (boCheckChange)
                            {
                                break;
                            }
                        }
                        if (!boCheckChange)// 原列表中的人物名称是否在新的列表中
                        {
                            result = -6;
                            break;
                        }
                    }
                    if (!boCheckChange)
                    {
                        break;
                    }
                }
                for (var i = 0; i < guildRankList.Count; i++)
                {
                    guildRank = guildRankList[i];
                    boCheckChange = true;
                    for (var j = 0; j < guildRank.MemberList.Count; j++)
                    {
                        boCheckChange = false;
                        sMemberName = guildRank.MemberList[j].MemberName;
                        n30++;
                        for (var k = 0; k < guildRankList.Count; k++)
                        {
                            newGuildRank = guildRankList[k];
                            for (n28 = 0; n28 < newGuildRank.MemberList.Count; n28++)
                            {
                                if (newGuildRank.MemberList[n28].MemberName == sMemberName)
                                {
                                    boCheckChange = true;
                                    break;
                                }
                            }
                            if (boCheckChange)
                            {
                                break;
                            }
                        }
                        if (!boCheckChange)
                        {
                            result = -6;
                            break;
                        }
                    }
                    if (!boCheckChange)
                    {
                        break;
                    }
                }
                if (result == 0 && n2C != n30)
                {
                    result = -6;
                }
            }
            if (result == 0)
            {
                // 检查职位号是否重复及非法
                for (var i = 0; i < guildRankList.Count; i++)
                {
                    n28 = guildRankList[i].RankNo;
                    for (var k = i + 1; k < guildRankList.Count; k++)
                    {
                        if (guildRankList[k].RankNo == n28 || n28 <= 0 || n28 > 99)
                        {
                            result = -7;
                            break;
                        }
                    }
                    if (result != 0)
                    {
                        break;
                    }
                }
            }
            if (result == 0)
            {
                ClearRankList(ref MRankList);
                MRankList = guildRankList;// 更新在线人物职位表
                for (var i = 0; i < MRankList.Count; i++)
                {
                    guildRank = MRankList[i];
                    for (var j = 0; j < guildRank.MemberList.Count; j++)
                    {
                        var memberObject = M2Share.WorldEngine.GetPlayObject(guildRank.MemberList[j].MemberName);
                        if (memberObject != null)
                        {
                            //GuildRank.MemberList[j].PlayObject = PlayObject;
                            memberObject.RefRankInfo(guildRank.RankNo, guildRank.RankName);
                            memberObject.RefShowName();
                        }
                    }
                }
                UpdateGuildFile();
            }
            else
            {
                ClearRankList(ref guildRankList);
            }
            return result;
        }

        public bool IsNotWarGuild(GuildInfo guild)
        {
            for (var i = 0; i < GuildWarList.Count; i++)
            {
                if (GuildWarList[i].Guild == guild)
                {
                    return false;
                }
            }
            return true;
        }

        public void AllyGuild(GuildInfo guild)
        {
            for (var i = 0; i < GuildAllList.Count; i++)
            {
                if (GuildAllList[i] == guild)
                {
                    return;
                }
            }
            GuildAllList.Add(guild);
            SaveGuildInfoFile();
        }

        public WarGuild AddWarGuild(GuildInfo guild)
        {
            WarGuild result = default;
            WarGuild warGuild = default;
            if (guild != null)
            {
                if (!IsAllyGuild(guild))
                {
                    for (var i = 0; i < GuildWarList.Count; i++)
                    {
                        if (GuildWarList[i].Guild == guild)
                        {
                            warGuild = GuildWarList[i];
                            warGuild.dwWarTick = HUtil32.GetTickCount();
                            warGuild.dwWarTime = M2Share.Config.GuildWarTime;// 10800000
                            SendGuildMsg("***" + guild.GuildName + "行会战争将持续三个小时。");
                            break;
                        }
                    }
                    if (warGuild.dwWarTime > 0)
                    {
                        warGuild = new WarGuild
                        {
                            Guild = guild,
                            dwWarTick = HUtil32.GetTickCount(),
                            dwWarTime = M2Share.Config.GuildWarTime// 10800000
                        };
                        GuildWarList.Add(warGuild);
                        SendGuildMsg("***" + guild.GuildName + "行会战争开始(三个小时)");
                    }
                    result = warGuild;
                }
            }
            RefMemberName();
            UpdateGuildFile();
            return result;
        }

        public void EndGuildWar(GuildInfo guild)
        {
            SendGuildMsg("***" + guild.GuildName + "行会战争结束");
        }

        private int GetMemberCount()
        {
            return MRankList.Sum(t => t.MemberList.Count);
        }

        private bool GetMemgerIsFull()
        {
            return Count >= M2Share.Config.GuildMemberMaxLimit;
        }

        public void StartTeamFight()
        {
            ContestPoint = 0;
            BoTeamFight = true;
            TeamFightDeadList.Clear();
        }

        public void EndTeamFight()
        {
            BoTeamFight = false;
        }

        public void AddTeamFightMember(string sHumanName)
        {
            TeamFightDeadList.Add(sHumanName);
        }

        private void SetAuraePoint(int nPoint)
        {
            aurae = nPoint;
            BoChanged = true;
        }

        private void SetBuildPoint(int nPoint)
        {
            buildPoint = nPoint;
            BoChanged = true;
        }

        private void SetFlourishPoint(int nPoint)
        {
            flourishing = nPoint;
            BoChanged = true;
        }

        private void SetStabilityPoint(int nPoint)
        {
            stability = nPoint;
            BoChanged = true;
        }

        private void SetChiefItemCount(int nPoint)
        {
            chiefItemCount = nPoint;
            BoChanged = true;
        }

    }
}