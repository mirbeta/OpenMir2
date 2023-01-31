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
            get => m_nBuildPoint;
            set => SetBuildPoint(value);
        }

        public int Aurae
        {
            get => m_nAurae;
            set => SetAuraePoint(value);
        }

        public int Stability
        {
            get => m_nStability;
            set => SetStabilityPoint(value);
        }

        public int Flourishing
        {
            get => m_nFlourishing;
            set => SetFlourishPoint(value);
        }

        public int ChiefItemCount
        {
            get => m_nChiefItemCount;
            set => SetChiefItemCount(value);
        }

        /// <summary>
        /// 行会名称
        /// </summary>
        public string sGuildName;
        /// <summary>
        /// 行会公告
        /// </summary>
        public ArrayList NoticeList;
        public IList<WarGuild> GuildWarList;
        public IList<GuildInfo> GuildAllList;
        /// <summary>
        /// 职位列表
        /// </summary>
        public IList<GuildRank> m_RankList;
        public int nContestPoint;
        public bool boTeamFight;
        public ArrayList TeamFightDeadList;
        public bool m_boEnableAuthAlly;
        public int dwSaveTick;
        public bool boChanged;
        public Dictionary<string, DynamicVar> m_DynamicVarList;
        /// <summary>
        /// 建筑度
        /// </summary>
        public int m_nBuildPoint;
        /// <summary>
        /// 人气度
        /// </summary>        
        public int m_nAurae;
        /// <summary>
        /// 安定度
        /// </summary>        
        public int m_nStability;
        /// <summary>
        /// 繁荣度
        /// </summary>        
        public int m_nFlourishing;
        public int m_nChiefItemCount;
        private readonly GuildConf _guildConf;

        private void ClearRank()
        {
            for (int i = 0; i < m_RankList.Count; i++)
            {
                m_RankList[i] = null;
            }
            m_RankList.Clear();
        }

        public GuildInfo(string sName)
        {
            sGuildName = sName;
            NoticeList = new ArrayList();
            GuildWarList = new List<WarGuild>();
            GuildAllList = new List<GuildInfo>();
            m_RankList = new List<GuildRank>();
            TeamFightDeadList = new ArrayList();
            dwSaveTick = 0;
            boChanged = false;
            nContestPoint = 0;
            boTeamFight = false;
            m_boEnableAuthAlly = false;
            m_nBuildPoint = 0;
            m_nAurae = 0;
            m_nStability = 0;
            m_nFlourishing = 0;
            m_nChiefItemCount = 0;
            m_DynamicVarList = new Dictionary<string, DynamicVar>(StringComparer.OrdinalIgnoreCase);
            string sFileName = Path.Combine(M2Share.Config.GuildDir, string.Concat(sName + ".ini"));
            _guildConf = new GuildConf(sName, sFileName);
        }

        public bool DelAllyGuild(GuildInfo Guild)
        {
            bool result = false;
            GuildInfo AllyGuild;
            for (int i = 0; i < GuildAllList.Count; i++)
            {
                AllyGuild = GuildAllList[i];
                if (AllyGuild == Guild)
                {
                    GuildAllList.RemoveAt(i);
                    result = true;
                    break;
                }
            }
            SaveGuildInfoFile();
            return result;
        }

        public bool IsAllyGuild(GuildInfo Guild)
        {
            bool result = false;
            GuildInfo AllyGuild;
            for (int i = 0; i < GuildAllList.Count; i++)
            {
                AllyGuild = GuildAllList[i];
                if (AllyGuild == Guild)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public bool IsMember(string sName)
        {
            bool result = false;
            GuildRank GuildRank;
            for (int i = 0; i < m_RankList.Count; i++)
            {
                GuildRank = m_RankList[i];
                for (int j = 0; j < GuildRank.MemberList.Count; j++)
                {
                    if (GuildRank.MemberList[j] == null)
                    {
                        continue;
                    }
                    if (GuildRank.MemberList[j].sMemberName == sName)
                    {
                        result = true;
                        return result;
                    }
                }
            }
            return result;
        }

        public bool IsWarGuild(GuildInfo Guild)
        {
            bool result = false;
            for (int i = 0; i < GuildWarList.Count; i++)
            {
                if (GuildWarList[i].Guild == Guild)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public bool LoadGuild()
        {
            string sFileName = sGuildName + ".txt";
            bool result = LoadGuildFile(sFileName);
            LoadGuildConfig();
            return result;
        }

        private void LoadGuildConfig()
        {
            _guildConf.LoadConfig(this);
        }

        public bool LoadGuildFile(string sGuildFileName)
        {
            StringList LoadList;
            string s1C = string.Empty;
            string s20 = string.Empty;
            int n28;
            WarGuild GuildWar;
            GuildInfo Guild;
            GuildRank GuildRank = null;
            string sFileName = Path.Combine(M2Share.Config.GuildDir, sGuildFileName);
            if (!File.Exists(sFileName))
            {
                return false;
            }
            ClearRank();
            NoticeList.Clear();
            for (int i = 0; i < GuildWarList.Count; i++)
            {
                GuildWarList[i] = null;
            }
            GuildWarList.Clear();
            GuildAllList.Clear();
            n28 = 0;
            short n2C = 0;
            string s24 = "";
            LoadList = new StringList();
            LoadList.LoadFromFile(sFileName);
            for (int i = 0; i < LoadList.Count; i++)
            {
                string s18 = LoadList[i];
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
                        n2C = (short)HUtil32.StrToInt(s1C, 0);
                        s24 = s18.Trim();
                        GuildRank = null;
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
                        while (s18 != "")
                        {
                            s18 = HUtil32.GetValidStr3(s18, ref s1C, new[] { ' ', ',' });
                            if (s1C == "")
                            {
                                break;
                            }
                            GuildWar = new WarGuild
                            {
                                Guild = M2Share.GuildMgr.FindGuild(s1C)
                            };
                            if (GuildWar.Guild != null)
                            {
                                GuildWar.dwWarTick = HUtil32.GetTickCount();
                                GuildWar.dwWarTime = HUtil32.StrToInt(s20.Trim(), 0);
                                GuildWarList.Add(GuildWar);
                            }
                            else
                            {
                            }
                        }
                        break;
                    case 3:
                        while (s18 != "")
                        {
                            s18 = HUtil32.GetValidStr3(s18, ref s1C, new[] { ' ', ',' });
                            s18 = HUtil32.GetValidStr3(s18, ref s20, new[] { ' ', ',' });
                            if (s1C == "")
                            {
                                break;
                            }
                            Guild = M2Share.GuildMgr.FindGuild(s1C);
                            if (Guild != null)
                            {
                                GuildAllList.Add(Guild);
                            }
                        }
                        break;
                    case 4:
                        if (n2C > 0 && s24 != "")
                        {
                            if (s24.Length > 30)
                            {
                                s24 = s24[..M2Share.Config.GuildRankNameLen];//限制职倍的长度
                            }
                            if (GuildRank == null)
                            {
                                GuildRank = new GuildRank
                                {
                                    nRankNo = n2C,
                                    sRankName = s24,
                                    MemberList = new List<GuildMember>()
                                };
                                m_RankList.Add(GuildRank);
                            }
                            while (s18 != "")
                            {
                                s18 = HUtil32.GetValidStr3(s18, ref s1C, new[] { ' ', ',' });
                                if (string.IsNullOrEmpty(s1C))
                                {
                                    break;
                                }
                                PlayObject playObject = M2Share.WorldEngine.GetPlayObject(s1C);
                                GuildRank.MemberList.Add(new GuildMember()
                                {
                                    sMemberName = s1C,
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
            GuildRank GuildRank;
            BaseObject BaseObject;
            for (int i = 0; i < m_RankList.Count; i++)
            {
                GuildRank = m_RankList[i];
                for (int j = 0; j < GuildRank.MemberList.Count; j++)
                {
                    BaseObject = GuildRank.MemberList[j].PlayObject;
                    if (BaseObject != null)
                    {
                        BaseObject.RefShowName();
                    }
                }
            }
        }

        public void SaveGuildInfoFile()
        {
            if (M2Share.ServerIndex == 0)
            {
                SaveGuildFile(Path.Combine(M2Share.Config.GuildDir, string.Concat(sGuildName, ".txt")));
                SaveGuildConfig();
            }
            else
            {
                SaveGuildFile(Path.Combine(M2Share.Config.GuildDir, sGuildName, ".", M2Share.ServerIndex.ToString()));
            }
        }

        private void SaveGuildConfig()
        {
            _guildConf.SaveGuildConfig(this);
        }

        private void SaveGuildFile(string sFileName)
        {
            StringList SaveList = new StringList();
            SaveList.Add(M2Share.Config.GuildNotice);
            for (int i = 0; i < NoticeList.Count; i++)
            {
                SaveList.Add("+" + NoticeList[i]);
            }
            SaveList.Add(" ");
            SaveList.Add(M2Share.Config.GuildWar);
            for (int i = 0; i < GuildWarList.Count; i++)
            {
                WarGuild WarGuild = GuildWarList[i];
                long n14 = WarGuild.dwWarTime - (HUtil32.GetTickCount() - WarGuild.dwWarTick);
                if (n14 <= 0)
                {
                    continue;
                }
                SaveList.Add("+" + GuildWarList[i].Guild.sGuildName + ' ' + n14);
            }
            SaveList.Add(" ");
            SaveList.Add(M2Share.Config.GuildAll);
            for (int i = 0; i < GuildAllList.Count; i++)
            {
                SaveList.Add("+" + GuildAllList[i]);
            }
            SaveList.Add(" ");
            SaveList.Add(M2Share.Config.GuildMember);
            for (int i = 0; i < m_RankList.Count; i++)
            {
                GuildRank GuildRank = m_RankList[i];
                SaveList.Add("#" + GuildRank.nRankNo + ' ' + GuildRank.sRankName);
                for (int j = 0; j < GuildRank.MemberList.Count; j++)
                {
                    SaveList.Add("+" + GuildRank.MemberList[j].sMemberName);
                }
            }
            try
            {
                SaveList.SaveToFile(sFileName);
            }
            catch
            {
                M2Share.Log.Error("保存行会信息失败!!! " + sFileName);
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
                for (int i = 0; i < m_RankList.Count; i++)
                {
                    GuildRank guildRank = m_RankList[i];
                    for (int j = 0; j < guildRank.MemberList.Count; j++)
                    {
                        PlayObject guildMember = guildRank.MemberList[j].PlayObject;
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
                M2Share.Log.Error("[Exceptiion] TGuild.SendGuildMsg GuildName = " + sGuildName + " Msg = " + sMsg);
                M2Share.Log.Error(e.Message);
            }
        }

        public bool SetGuildInfo(string sChief)
        {
            GuildRank GuildRank;
            if (m_RankList.Count == 0)
            {
                GuildRank = new GuildRank
                {
                    nRankNo = 1,
                    sRankName = M2Share.Config.GuildChief,
                    MemberList = new List<GuildMember>()
                };
                PlayObject playObject = M2Share.WorldEngine.GetPlayObject(sChief);
                GuildRank.MemberList.Add(new GuildMember() { sMemberName = sChief, PlayObject = playObject });
                m_RankList.Add(GuildRank);
                SaveGuildInfoFile();
            }
            return true;
        }

        public string GetRankName(PlayObject PlayObject, ref short nRankNo)
        {
            string result = string.Empty;
            GuildRank GuildRank;
            for (int i = 0; i < m_RankList.Count; i++)
            {
                GuildRank = m_RankList[i];
                for (int j = 0; j < GuildRank.MemberList.Count; j++)
                {
                    if (GuildRank.MemberList[j].sMemberName == PlayObject.ChrName)
                    {
                        GuildRank.MemberList[j].PlayObject = PlayObject;
                        nRankNo = GuildRank.nRankNo;
                        result = GuildRank.sRankName;
                        // PlayObject.RefShowName();
                        PlayObject.SendMsg(PlayObject, Messages.RM_CHANGEGUILDNAME, 0, 0, 0, 0, "");
                        return result;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取会长名称
        /// </summary>
        /// <returns></returns>
        public string GetChiefName()
        {
            GuildRank GuildRank;
            if (m_RankList.Count <= 0)
            {
                return string.Empty;
            }
            GuildRank = m_RankList[0];
            if (GuildRank.MemberList.Count <= 0)
            {
                return string.Empty;
            }
            return GuildRank.MemberList[0].sMemberName;
        }

        public void CheckSaveGuildFile()
        {
            if (boChanged && (HUtil32.GetTickCount() - dwSaveTick) > 30 * 1000)
            {
                boChanged = false;
                SaveGuildInfoFile();
            }
        }

        public void DelHumanObj(PlayObject PlayObject)
        {
            CheckSaveGuildFile();
            for (int i = 0; i < m_RankList.Count; i++)
            {
                GuildRank guildRank = m_RankList[i];
                for (int j = 0; j < guildRank.MemberList.Count; j++)
                {
                    if (string.Compare(guildRank.MemberList[j].sMemberName, PlayObject.ChrName, StringComparison.OrdinalIgnoreCase) == 1)
                    {
                        guildRank.MemberList[j] = null;
                        return;
                    }
                }
            }
        }

        public void TeamFightWhoDead(string sName)
        {
            if (!boTeamFight)
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
            if (!boTeamFight)
            {
                return;
            }
            nContestPoint += nPoint;
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
            boChanged = true;
            dwSaveTick = HUtil32.GetTickCount();
            SaveGuildInfoFile();
        }

        public void BackupGuildFile()
        {
            PlayObject PlayObject;
            GuildRank GuildRank;
            if (M2Share.ServerIndex == 0)
            {
                SaveGuildFile(Path.Combine(M2Share.Config.GuildDir, sGuildName, '.' + HUtil32.GetTickCount() + ".bak"));
            }
            for (int i = 0; i < m_RankList.Count; i++)
            {
                GuildRank = m_RankList[i];
                for (int j = 0; j < GuildRank.MemberList.Count; j++)
                {
                    PlayObject = GuildRank.MemberList[j].PlayObject;
                    if (PlayObject != null)
                    {
                        PlayObject.MyGuild = null;
                        PlayObject.RefRankInfo(0, "");
                        PlayObject.RefShowName();
                    }
                }
                GuildRank.MemberList = null;
            }
            m_RankList.Clear();
            NoticeList.Clear();
            for (int i = 0; i < GuildWarList.Count; i++)
            {
                GuildWarList[i] = null;
            }
            GuildWarList.Clear();
            GuildAllList.Clear();
            SaveGuildInfoFile();
        }

        public void AddMember(PlayObject PlayObject)
        {
            GuildRank GuildRank;
            GuildRank GuildRank18 = null;
            for (int i = 0; i < m_RankList.Count; i++)
            {
                GuildRank = m_RankList[i];
                if (GuildRank.nRankNo == 99)
                {
                    GuildRank18 = GuildRank;
                    break;
                }
            }
            if (GuildRank18 == null)
            {
                GuildRank18 = new GuildRank
                {
                    nRankNo = 99,
                    sRankName = M2Share.Config.GuildMemberRank,
                    MemberList = new List<GuildMember>()
                };
                m_RankList.Add(GuildRank18);
            }
            GuildRank18.MemberList.Add(new GuildMember()
            {
                PlayObject = PlayObject,
                sMemberName = PlayObject.ChrName
            });
            UpdateGuildFile();
        }

        public bool DelMember(string sHumName)
        {
            bool result = false;
            GuildRank GuildRank;
            for (int i = 0; i < m_RankList.Count; i++)
            {
                GuildRank = m_RankList[i];
                for (int j = 0; j < GuildRank.MemberList.Count; j++)
                {
                    if (GuildRank.MemberList[j].sMemberName == sHumName)
                    {
                        GuildRank.MemberList.RemoveAt(j);
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
            GuildRank GuildRank;
            if (m_RankList.Count != 1)
            {
                return false;
            }
            GuildRank = m_RankList[0];
            if (GuildRank.MemberList.Count != 1)
            {
                return false;
            }
            if (GuildRank.MemberList[0].sMemberName == sHumName)
            {
                BackupGuildFile();
                return true;
            }
            return false;
        }

        private static void UpdateRank_ClearRankList(ref IList<GuildRank> RankList)
        {
            GuildRank GuildRank;
            for (int i = 0; i < RankList.Count; i++)
            {
                GuildRank = RankList[i];
                GuildRank.MemberList = null;
            }
        }

        public int UpdateRank(string sRankData)
        {
            GuildRank NewGuildRank;
            string sRankInfo = string.Empty;
            string sRankNo = string.Empty;
            string sRankName = string.Empty;
            string sMemberName = string.Empty;
            int n28;
            int n2C;
            int n30;
            bool boCheckChange;
            PlayObject PlayObject;
            IList<GuildRank> GuildRankList = new List<GuildRank>();
            GuildRank GuildRank = null;
            while (true)
            {
                if (sRankData == "")
                {
                    break;
                }
                sRankData = HUtil32.GetValidStr3(sRankData, ref sRankInfo, '\r');
                sRankInfo = sRankInfo.Trim();
                if (sRankInfo == "")
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
                        sRankName = sRankName[..30]; // Jacky 限制职倍的长度
                    }
                    if (GuildRank != null)
                    {
                        GuildRankList.Add(GuildRank);
                    }
                    GuildRank = new GuildRank
                    {
                        nRankNo = (short)HUtil32.StrToInt(sRankNo, 99),
                        sRankName = sRankName.Trim(),
                        MemberList = new List<GuildMember>()
                    };
                    continue;
                }
                if (GuildRank == null)
                {
                    continue;
                }
                int count = 0;
                while (true)
                {
                    // 将成员名称加入职称表里
                    if (sRankInfo == "")
                    {
                        break;
                    }
                    sRankInfo = HUtil32.GetValidStr3(sRankInfo, ref sMemberName, new[] { ' ', ',' });
                    if (sMemberName != "")
                    {
                        GuildRank.MemberList.Add(new GuildMember()
                        {
                            PlayObject = M2Share.WorldEngine.GetPlayObject(sMemberName),
                            sMemberName = sMemberName
                        });
                    }
                    count++;
                    if (count >= 10)
                    {
                        break;
                    }
                }
            }
            if (GuildRank != null)
            {
                GuildRankList.Add(GuildRank);
            }
            int result;
            // 校验成员列表是否有改变，如果未修改则退出
            if (m_RankList.Count == GuildRankList.Count)
            {
                boCheckChange = true;
                for (int i = 0; i < m_RankList.Count; i++)
                {
                    GuildRank = m_RankList[i];
                    NewGuildRank = GuildRankList[i];
                    if (GuildRank.nRankNo == NewGuildRank.nRankNo && GuildRank.sRankName == NewGuildRank.sRankName && GuildRank.MemberList.Count == NewGuildRank.MemberList.Count)
                    {
                        for (int j = 0; j < GuildRank.MemberList.Count; j++)
                        {
                            if (GuildRank.MemberList[j] != NewGuildRank.MemberList[j])
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
                    UpdateRank_ClearRankList(ref GuildRankList);
                    return result;
                }
            }
            // 检查行会掌门职业是否为空
            result = -2;
            if (GuildRankList.Count > 0)
            {
                GuildRank = GuildRankList[0];
                if (GuildRank.nRankNo == 1)
                {
                    if (GuildRank.sRankName != "")
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
                GuildRank = GuildRankList[0];
                if (GuildRank.MemberList.Count <= 2)
                {
                    n28 = GuildRank.MemberList.Count;
                    for (int i = 0; i < GuildRank.MemberList.Count; i++)
                    {
                        if (M2Share.WorldEngine.GetPlayObject(GuildRank.MemberList[i].sMemberName) == null)
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
                for (int i = 0; i < m_RankList.Count; i++)
                {
                    GuildRank = m_RankList[i];
                    boCheckChange = true;
                    for (int j = 0; j < GuildRank.MemberList.Count; j++)
                    {
                        boCheckChange = false;
                        sMemberName = GuildRank.MemberList[j].sMemberName;
                        n2C++;
                        for (int k = 0; k < GuildRankList.Count; k++)
                        {
                            // 搜索新列表
                            NewGuildRank = GuildRankList[k];
                            for (n28 = 0; n28 < NewGuildRank.MemberList.Count; n28++)
                            {
                                if (NewGuildRank.MemberList[n28].sMemberName == sMemberName)
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
                for (int i = 0; i < GuildRankList.Count; i++)
                {
                    GuildRank = GuildRankList[i];
                    boCheckChange = true;
                    for (int j = 0; j < GuildRank.MemberList.Count; j++)
                    {
                        boCheckChange = false;
                        sMemberName = GuildRank.MemberList[j].sMemberName;
                        n30++;
                        for (int k = 0; k < GuildRankList.Count; k++)
                        {
                            NewGuildRank = GuildRankList[k];
                            for (n28 = 0; n28 < NewGuildRank.MemberList.Count; n28++)
                            {
                                if (NewGuildRank.MemberList[n28].sMemberName == sMemberName)
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
                for (int i = 0; i < GuildRankList.Count; i++)
                {
                    n28 = GuildRankList[i].nRankNo;
                    for (int k = i + 1; k < GuildRankList.Count; k++)
                    {
                        if (GuildRankList[k].nRankNo == n28 || n28 <= 0 || n28 > 99)
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
                UpdateRank_ClearRankList(ref m_RankList);
                m_RankList = GuildRankList;// 更新在线人物职位表
                for (int i = 0; i < m_RankList.Count; i++)
                {
                    GuildRank = m_RankList[i];
                    for (int j = 0; j < GuildRank.MemberList.Count; j++)
                    {
                        PlayObject = M2Share.WorldEngine.GetPlayObject(GuildRank.MemberList[j].sMemberName);
                        if (PlayObject != null)
                        {
                            GuildRank.MemberList[j].PlayObject = PlayObject;
                            PlayObject.RefRankInfo(GuildRank.nRankNo, GuildRank.sRankName);
                            PlayObject.RefShowName();
                        }
                    }
                }
                UpdateGuildFile();
            }
            else
            {
                UpdateRank_ClearRankList(ref GuildRankList);
            }
            return result;
        }

        public bool IsNotWarGuild(GuildInfo Guild)
        {
            for (int i = 0; i < GuildWarList.Count; i++)
            {
                if (GuildWarList[i].Guild == Guild)
                {
                    return false;
                }
            }
            return true;
        }

        public void AllyGuild(GuildInfo Guild)
        {
            for (int i = 0; i < GuildAllList.Count; i++)
            {
                if (GuildAllList[i] == Guild)
                {
                    return;
                }
            }
            GuildAllList.Add(Guild);
            SaveGuildInfoFile();
        }

        public WarGuild AddWarGuild(GuildInfo Guild)
        {
            WarGuild result = null;
            WarGuild WarGuild;
            if (Guild != null)
            {
                if (!IsAllyGuild(Guild))
                {
                    WarGuild = null;
                    for (int i = 0; i < GuildWarList.Count; i++)
                    {
                        if (GuildWarList[i].Guild == Guild)
                        {
                            WarGuild = GuildWarList[i];
                            WarGuild.dwWarTick = HUtil32.GetTickCount();
                            WarGuild.dwWarTime = M2Share.Config.GuildWarTime;// 10800000
                            SendGuildMsg("***" + Guild.sGuildName + "行会战争将持续三个小时。");
                            break;
                        }
                    }
                    if (WarGuild == null)
                    {
                        WarGuild = new WarGuild
                        {
                            Guild = Guild,
                            dwWarTick = HUtil32.GetTickCount(),
                            dwWarTime = M2Share.Config.GuildWarTime// 10800000
                        };
                        GuildWarList.Add(WarGuild);
                        SendGuildMsg("***" + Guild.sGuildName + "行会战争开始(三个小时)");
                    }
                    result = WarGuild;
                }
            }
            RefMemberName();
            UpdateGuildFile();
            return result;
        }

        public void EndGuildWar(GuildInfo Guild)
        {
            SendGuildMsg("***" + Guild.sGuildName + "行会战争结束");
        }

        private int GetMemberCount()
        {
            return m_RankList.Sum(t => t.MemberList.Count);
        }

        private bool GetMemgerIsFull()
        {
            return Count >= M2Share.Config.GuildMemberMaxLimit;
        }

        public void StartTeamFight()
        {
            nContestPoint = 0;
            boTeamFight = true;
            TeamFightDeadList.Clear();
        }

        public void EndTeamFight()
        {
            boTeamFight = false;
        }

        public void AddTeamFightMember(string sHumanName)
        {
            TeamFightDeadList.Add(sHumanName);
        }

        private void SetAuraePoint(int nPoint)
        {
            m_nAurae = nPoint;
            boChanged = true;
        }

        private void SetBuildPoint(int nPoint)
        {
            m_nBuildPoint = nPoint;
            boChanged = true;
        }

        private void SetFlourishPoint(int nPoint)
        {
            m_nFlourishing = nPoint;
            boChanged = true;
        }

        private void SetStabilityPoint(int nPoint)
        {
            m_nStability = nPoint;
            boChanged = true;
        }

        private void SetChiefItemCount(int nPoint)
        {
            m_nChiefItemCount = nPoint;
            boChanged = true;
        }

    }
}