using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SystemModule;
using SystemModule.Common;

namespace GameSvr
{
    /// <summary>
    /// 行会系统
    /// </summary>
    public class TGuild
    {
        public int Count => GetMemberCount();
        
        public bool IsFull => GetMemgerIsFull();

        public int BuildPoint
        {
            get => m_nBuildPoint;
            set => SetBuildPoint(value);
        }
        
        public int nAurae
        {
            get => m_nAurae;
            set => SetAuraePoint(value);
        }
        
        public int nStability
        {
            get => m_nStability;
            set => SetStabilityPoint(value);
        }
        
        public int nFlourishing
        {
            get => m_nFlourishing;
            set => SetFlourishPoint(value);
        }
        
        public int nChiefItemCount
        {
            get => m_nChiefItemCount;
            set => SetChiefItemCount(value);
        }
        
        public string sGuildName;
        public ArrayList NoticeList = null;
        public IList<TWarGuild> GuildWarList = null;
        public IList<TGuild> GuildAllList = null;
        /// <summary>
        /// 职位列表
        /// </summary>
        public IList<TGuildRank> m_RankList = null;
        public int nContestPoint = 0;
        public bool boTeamFight = false;
        public ArrayList TeamFightDeadList = null;
        public bool m_boEnableAuthAlly = false;
        public int dwSaveTick = 0;
        public bool boChanged = false;
        public IList<TDynamicVar> m_DynamicVarList = null;
        private readonly IniFile m_Config = null;
        /// <summary>
        /// 建筑度
        /// </summary>
        private int m_nBuildPoint = 0;
        /// <summary>
        /// 人气度
        /// </summary>        
        private int m_nAurae = 0;
        /// <summary>
        /// 安定度
        /// </summary>        
        private int m_nStability = 0;
        /// <summary>
        /// 繁荣度
        /// </summary>        
        private int m_nFlourishing = 0;
        private int m_nChiefItemCount = 0;

        private void ClearRank()
        {
            TGuildRank GuildRank;
            for (var i = 0; i < m_RankList.Count; i++)
            {
                GuildRank = m_RankList[i];
                GuildRank = null;
            }
            m_RankList.Clear();
        }

        public TGuild(string sName)
        {
            sGuildName = sName;
            NoticeList = new ArrayList();
            GuildWarList = new List<TWarGuild>();
            GuildAllList = new List<TGuild>();
            m_RankList = new List<TGuildRank>();
            TeamFightDeadList = new ArrayList();
            dwSaveTick = 0;
            boChanged = false;
            nContestPoint = 0;
            boTeamFight = false;
            m_boEnableAuthAlly = false;
            var sFileName = M2Share.g_Config.sGuildDir + sName + ".ini";
            m_Config = new IniFile(sFileName);
            if (!File.Exists(sFileName))
            {
                m_Config.WriteString("Guild", "GuildName", sName);
            }
            m_nBuildPoint = 0;
            m_nAurae = 0;
            m_nStability = 0;
            m_nFlourishing = 0;
            m_nChiefItemCount = 0;
            m_DynamicVarList = new List<TDynamicVar>();
        }

        public bool DelAllyGuild(TGuild Guild)
        {
            var result = false;
            TGuild AllyGuild;
            for (var i = 0; i < GuildAllList.Count; i++)
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

        public bool IsAllyGuild(TGuild Guild)
        {
            var result = false;
            TGuild AllyGuild;
            for (var i = 0; i < GuildAllList.Count; i++)
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
            var result = false;
            TGuildRank GuildRank;
            for (var i = 0; i < m_RankList.Count; i++)
            {
                GuildRank = m_RankList[i];
                for (var j = 0; j < GuildRank.MemberList.Count; j++)
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

        public bool IsWarGuild(TGuild Guild)
        {
            var result = false;
            for (var i = 0; i < GuildWarList.Count; i++)
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
            var sFileName = sGuildName + ".txt";
            var result = LoadGuildFile(sFileName);
            LoadGuildConfig(sGuildName + ".ini");
            return result;
        }

        public bool LoadGuildConfig(string sGuildFileName)
        {
            m_nBuildPoint = m_Config.ReadInteger("Guild", "BuildPoint", m_nBuildPoint);
            m_nAurae = m_Config.ReadInteger("Guild", "Aurae", m_nAurae);
            m_nStability = m_Config.ReadInteger("Guild", "Stability", m_nStability);
            m_nFlourishing = m_Config.ReadInteger("Guild", "Flourishing", m_nFlourishing);
            m_nChiefItemCount = m_Config.ReadInteger("Guild", "ChiefItemCount", m_nChiefItemCount);
            return true;
        }

        public bool LoadGuildFile(string sGuildFileName)
        {
            StringList LoadList;
            var s18 = string.Empty;
            var s1C = string.Empty;
            var s20 = string.Empty;
            var s24 = string.Empty;
            int n28;
            int n2C;
            TWarGuild GuildWar;
            TGuild Guild;
            TGuildRank GuildRank = null;
            var sFileName = Path.Combine(M2Share.g_Config.sGuildDir, sGuildFileName);
            if (!File.Exists(sFileName))
            {
                return false;
            }
            ClearRank();
            NoticeList.Clear();
            for (var i = 0; i < GuildWarList.Count; i++)
            {
                GuildWarList[i] = null;
            }
            GuildWarList.Clear();
            GuildAllList.Clear();
            n28 = 0;
            n2C = 0;
            s24 = "";
            LoadList = new StringList();
            LoadList.LoadFromFile(sFileName);
            for (var i = 0; i < LoadList.Count; i++)
            {
                s18 = LoadList[i];
                if (s18 == "" || s18[0] == ';')
                {
                    continue;
                }
                if (s18[0] != '+')
                {
                    if (s18 == M2Share.g_Config.sGuildNotice)
                    {
                        n28 = 1;
                    }
                    if (s18 == M2Share.g_Config.sGuildWar)
                    {
                        n28 = 2;
                    }
                    if (s18 == M2Share.g_Config.sGuildAll)
                    {
                        n28 = 3;
                    }
                    if (s18 == M2Share.g_Config.sGuildMember)
                    {
                        n28 = 4;
                    }
                    if (s18[0] == '#')
                    {
                        s18 = s18.Substring(2 - 1, s18.Length - 1);
                        s18 = HUtil32.GetValidStr3(s18, ref s1C, new char[] { ' ', ',' });
                        n2C = HUtil32.Str_ToInt(s1C, 0);
                        s24 = s18.Trim();
                        GuildRank = null;
                    }
                    continue;
                }
                s18 = s18.Substring(1, s18.Length - 1);
                switch (n28)
                {
                    case 1:
                        NoticeList.Add(s18);
                        break;
                    case 2:
                        while (s18 != "")
                        {
                            s18 = HUtil32.GetValidStr3(s18, ref s1C, new char[] { ' ', ',' });
                            if (s1C == "")
                            {
                                break;
                            }
                            GuildWar = new TWarGuild
                            {
                                Guild = M2Share.GuildManager.FindGuild(s1C)
                            };
                            if (GuildWar.Guild != null)
                            {
                                GuildWar.dwWarTick = HUtil32.GetTickCount();
                                GuildWar.dwWarTime = HUtil32.Str_ToInt(s20.Trim(), 0);
                                GuildWarList.Add(GuildWar);
                            }
                            else
                            {
                                GuildWar = null;
                            }
                        }
                        break;
                    case 3:
                        while (s18 != "")
                        {
                            s18 = HUtil32.GetValidStr3(s18, ref s1C, new char[] { ' ', ',' });
                            s18 = HUtil32.GetValidStr3(s18, ref s20, new char[] { ' ', ',' });
                            if (s1C == "")
                            {
                                break;
                            }
                            Guild = M2Share.GuildManager.FindGuild(s1C);
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
                                s24 = s24.Substring(0, M2Share.g_Config.nGuildRankNameLen);//限制职倍的长度
                            }
                            if (GuildRank == null)
                            {
                                GuildRank = new TGuildRank
                                {
                                    nRankNo = n2C,
                                    sRankName = s24,
                                    MemberList = new List<TGuildMember>()
                                };
                                m_RankList.Add(GuildRank);
                            }
                            while (s18 != "")
                            {
                                s18 = HUtil32.GetValidStr3(s18, ref s1C, new char[] { ' ', ',' });
                                if (string.IsNullOrEmpty(s1C))
                                {
                                    break;
                                }
                                var playObject = M2Share.UserEngine.GetPlayObject(s1C);
                                GuildRank.MemberList.Add(new TGuildMember()
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

        public void RefMemberName()
        {
            TGuildRank GuildRank;
            TBaseObject BaseObject;
            for (var i = 0; i < m_RankList.Count; i++)
            {
                GuildRank = m_RankList[i];
                for (var j = 0; j < GuildRank.MemberList.Count; j++)
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
            if (M2Share.nServerIndex == 0)
            {
                SaveGuildFile(M2Share.g_Config.sGuildDir + sGuildName + ".txt");
                SaveGuildConfig(M2Share.g_Config.sGuildDir + sGuildName + ".ini");
            }
            else
            {
                SaveGuildFile(M2Share.g_Config.sGuildDir + sGuildName + '.' + M2Share.nServerIndex);
            }
        }

        private void SaveGuildConfig(string sFileName)
        {
            m_Config.WriteString("Guild", "GuildName", sGuildName);
            m_Config.WriteInteger("Guild", "BuildPoint", m_nBuildPoint);
            m_Config.WriteInteger("Guild", "Aurae", m_nAurae);
            m_Config.WriteInteger("Guild", "Stability", m_nStability);
            m_Config.WriteInteger("Guild", "Flourishing", m_nFlourishing);
            m_Config.WriteInteger("Guild", "ChiefItemCount", m_nChiefItemCount);
        }

        private void SaveGuildFile(string sFileName)
        {
            StringList SaveList = new StringList();
            SaveList.Add(M2Share.g_Config.sGuildNotice);
            TWarGuild WarGuild = null;
            long n14 = 0;
            TGuildRank GuildRank = null;
            for (var i = 0; i < NoticeList.Count; i++)
            {
                SaveList.Add("+" + NoticeList[i]);
            }
            SaveList.Add(" ");
            SaveList.Add(M2Share.g_Config.sGuildWar);
            for (var i = 0; i < GuildWarList.Count; i++)
            {
                WarGuild = GuildWarList[i];
                n14 = WarGuild.dwWarTime - (HUtil32.GetTickCount() - WarGuild.dwWarTick);
                if (n14 <= 0)
                {
                    continue;
                }
                SaveList.Add("+" + GuildWarList[i].Guild.sGuildName + ' ' + n14);
            }
            SaveList.Add(" ");
            SaveList.Add(M2Share.g_Config.sGuildAll);
            for (var i = 0; i < GuildAllList.Count; i++)
            {
                SaveList.Add("+" + GuildAllList[i]);
            }
            SaveList.Add(" ");
            SaveList.Add(M2Share.g_Config.sGuildMember);
            for (var i = 0; i < m_RankList.Count; i++)
            {
                GuildRank = m_RankList[i];
                SaveList.Add("#" + GuildRank.nRankNo + ' ' + GuildRank.sRankName);
                for (var j = 0; j < GuildRank.MemberList.Count; j++)
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
                M2Share.MainOutMessage("保存行会信息失败！！！ " + sFileName);
            }
        }

        public void SendGuildMsg(string sMsg)
        {
            TGuildRank GuildRank;
            TBaseObject BaseObject;
            try
            {
                if (M2Share.g_Config.boShowPreFixMsg)
                {
                    sMsg = M2Share.g_Config.sGuildMsgPreFix + sMsg;
                }
                for (var i = 0; i < m_RankList.Count; i++)
                {
                    GuildRank = m_RankList[i];
                    for (var j = 0; j < GuildRank.MemberList.Count; j++)
                    {
                        BaseObject = GuildRank.MemberList[j].PlayObject;
                        if (BaseObject == null)
                        {
                            continue;
                        }
                        if (BaseObject.m_boBanGuildChat)
                        {
                            BaseObject.SendMsg(BaseObject, Grobal2.RM_GUILDMESSAGE, 0, M2Share.g_Config.btGuildMsgFColor, M2Share.g_Config.btGuildMsgBColor, 0, sMsg);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage("[Exceptiion] TGuild.SendGuildMsg GuildName = " + sGuildName + " Msg = " + sMsg);
                M2Share.ErrorMessage(e.Message);
            }
        }

        public bool SetGuildInfo(string sChief)
        {
            TGuildRank GuildRank;
            if (m_RankList.Count == 0)
            {
                GuildRank = new TGuildRank
                {
                    nRankNo = 1,
                    sRankName = M2Share.g_Config.sGuildChief,
                    MemberList = new List<TGuildMember>()
                };
                var playObject = M2Share.UserEngine.GetPlayObject(sChief);
                GuildRank.MemberList.Add(new TGuildMember() { sMemberName = sChief, PlayObject = playObject });
                m_RankList.Add(GuildRank);
                SaveGuildInfoFile();
            }
            return true;
        }

        public string GetRankName(TPlayObject PlayObject, ref int nRankNo)
        {
            var result = string.Empty;
            TGuildRank GuildRank;
            for (var i = 0; i < m_RankList.Count; i++)
            {
                GuildRank = m_RankList[i];
                for (var j = 0; j < GuildRank.MemberList.Count; j++)
                {
                    if (GuildRank.MemberList[j].sMemberName == PlayObject.m_sCharName)
                    {
                        GuildRank.MemberList[j].PlayObject = PlayObject;
                        nRankNo = GuildRank.nRankNo;
                        result = GuildRank.sRankName;
                        // PlayObject.RefShowName();
                        PlayObject.SendMsg(PlayObject, Grobal2.RM_CHANGEGUILDNAME, 0, 0, 0, 0, "");
                        return result;
                    }
                }
            }
            return result;
        }

        public string GetChiefName()
        {
            var result = string.Empty;
            TGuildRank GuildRank;
            if (m_RankList.Count <= 0)
            {
                return result;
            }
            GuildRank = m_RankList[0];
            if (GuildRank.MemberList.Count <= 0)
            {
                return result;
            }
            result = GuildRank.MemberList[0].sMemberName;
            return result;
        }

        public void CheckSaveGuildFile()
        {
            if (boChanged && (HUtil32.GetTickCount() - dwSaveTick) > 30 * 1000)
            {
                boChanged = false;
                SaveGuildInfoFile();
            }
        }

        public void DelHumanObj(TPlayObject PlayObject)
        {
            CheckSaveGuildFile();
            for (var i = 0; i < m_RankList.Count; i ++ )
            {
                var guildRank = m_RankList[i];
                for (var j = 0; j < guildRank.MemberList.Count; j ++ )
                {
                    if (string.Compare(guildRank.MemberList[j].sMemberName, PlayObject.m_sCharName, StringComparison.OrdinalIgnoreCase) == 1)
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
            TPlayObject PlayObject;
            TGuildRank GuildRank;
            if (M2Share.nServerIndex == 0)
            {
                SaveGuildFile(Path.Combine(M2Share.g_Config.sGuildDir, sGuildName, '.' + HUtil32.GetTickCount() + ".bak"));
            }
            for (var i = 0; i < m_RankList.Count; i++)
            {
                GuildRank = m_RankList[i];
                for (var j = 0; j < GuildRank.MemberList.Count; j++)
                {
                    PlayObject = GuildRank.MemberList[j].PlayObject;
                    if (PlayObject != null)
                    {
                        PlayObject.m_MyGuild = null;
                        PlayObject.RefRankInfo(0, "");
                        PlayObject.RefShowName();
                    }
                }
                GuildRank.MemberList = null;
                GuildRank = null;
            }
            m_RankList.Clear();
            NoticeList.Clear();
            for (var i = 0; i < GuildWarList.Count; i++)
            {
                GuildWarList[i] = null;
            }
            GuildWarList.Clear();
            GuildAllList.Clear();
            SaveGuildInfoFile();
        }

        public bool AddMember(TPlayObject PlayObject)
        {
            bool result;
            TGuildRank GuildRank;
            TGuildRank GuildRank18 = null;
            for (var i = 0; i < m_RankList.Count; i++)
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
                GuildRank18 = new TGuildRank
                {
                    nRankNo = 99,
                    sRankName = M2Share.g_Config.sGuildMemberRank,
                    MemberList = new List<TGuildMember>()
                };
                m_RankList.Add(GuildRank18);
            }
            GuildRank18.MemberList.Add(new TGuildMember()
            {
                PlayObject = PlayObject,
                sMemberName = PlayObject.m_sCharName
            });
            UpdateGuildFile();
            result = true;
            return result;
        }

        public bool DelMember(string sHumName)
        {
            var result = false;
            TGuildRank GuildRank;
            for (var i = 0; i < m_RankList.Count; i++)
            {
                GuildRank = m_RankList[i];
                for (var j = 0; j < GuildRank.MemberList.Count; j++)
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
            var result = false;
            TGuildRank GuildRank;
            if (m_RankList.Count != 1)
            {
                return result;
            }
            GuildRank = m_RankList[0];
            if (GuildRank.MemberList.Count != 1)
            {
                return result;
            }
            if (GuildRank.MemberList[0].sMemberName == sHumName)
            {
                BackupGuildFile();
                result = true;
            }
            return result;
        }

        private void UpdateRank_ClearRankList(ref IList<TGuildRank> RankList)
        {
            TGuildRank GuildRank;
            for (var i = 0; i < RankList.Count; i++)
            {
                GuildRank = RankList[i];
                GuildRank.MemberList = null;
                GuildRank = null;
            }
        }

        public int UpdateRank(string sRankData)
        {
            int result= -1;
            TGuildRank NewGuildRank;
            var sRankInfo = string.Empty;
            var sRankNo = string.Empty;
            var sRankName = string.Empty;
            var sMemberName = string.Empty;
            int n28;
            int n2C;
            int n30;
            bool boCheckChange;
            TPlayObject PlayObject;
            IList<TGuildRank> GuildRankList = new List<TGuildRank>();
            TGuildRank GuildRank = null;
            while (true)
            {
                if (sRankData == "")
                {
                    break;
                }
                sRankData = HUtil32.GetValidStr3(sRankData, ref sRankInfo, new char[] { '\r' });
                sRankInfo = sRankInfo.Trim();
                if (sRankInfo == "")
                {
                    continue;
                }
                if (sRankInfo[0] == '#')// 取得职称的名称
                {
                    sRankInfo = sRankInfo.Substring(1, sRankInfo.Length - 1);
                    sRankInfo = HUtil32.GetValidStr3(sRankInfo, ref sRankNo, new char[] { ' ', '<' });
                    sRankInfo = HUtil32.GetValidStr3(sRankInfo, ref sRankName, new char[] { '<', '>' });
                    if (sRankName.Length > 30)
                    {
                        sRankName = sRankName.Substring(0, 30); // Jacky 限制职倍的长度
                    }
                    if (GuildRank != null)
                    {
                        GuildRankList.Add(GuildRank);
                    }
                    GuildRank = new TGuildRank
                    {
                        nRankNo = HUtil32.Str_ToInt(sRankNo, 99),
                        sRankName = sRankName.Trim(),
                        MemberList = new List<TGuildMember>()
                    };
                    continue;
                }
                if (GuildRank == null)
                {
                    continue;
                }
                var count = 0;
                while (true)
                {
                    // 将成员名称加入职称表里
                    if (sRankInfo == "")
                    {
                        break;
                    }
                    sRankInfo = HUtil32.GetValidStr3(sRankInfo, ref sMemberName, new char[] { ' ', ',' });
                    if (sMemberName != "")
                    {
                        GuildRank.MemberList.Add(new TGuildMember()
                        {
                            PlayObject = M2Share.UserEngine.GetPlayObject(sMemberName),
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
            // 校验成员列表是否有改变，如果未修改则退出
            if (m_RankList.Count == GuildRankList.Count)
            {
                boCheckChange = true;
                for (var i = 0; i < m_RankList.Count; i++)
                {
                    GuildRank = m_RankList[i];
                    NewGuildRank = GuildRankList[i];
                    if (GuildRank.nRankNo == NewGuildRank.nRankNo && GuildRank.sRankName == NewGuildRank.sRankName && GuildRank.MemberList.Count == NewGuildRank.MemberList.Count)
                    {
                        for (var j = 0; j < GuildRank.MemberList.Count; j++)
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
                    for (var i = 0; i < GuildRank.MemberList.Count; i++)
                    {
                        if (M2Share.UserEngine.GetPlayObject(GuildRank.MemberList[i].sMemberName) == null)
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
                for (var i = 0; i < m_RankList.Count; i++)
                {
                    GuildRank = m_RankList[i];
                    boCheckChange = true;
                    for (var j = 0; j < GuildRank.MemberList.Count; j++)
                    {
                        boCheckChange = false;
                        sMemberName = GuildRank.MemberList[j].sMemberName;
                        n2C++;
                        for (var k = 0; k < GuildRankList.Count; k++)
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
                for (var i = 0; i < GuildRankList.Count; i++)
                {
                    GuildRank = GuildRankList[i];
                    boCheckChange = true;
                    for (var j = 0; j < GuildRank.MemberList.Count; j++)
                    {
                        boCheckChange = false;
                        sMemberName = GuildRank.MemberList[j].sMemberName;
                        n30++;
                        for (var k = 0; k < GuildRankList.Count; k++)
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
                for (var i = 0; i < GuildRankList.Count; i++)
                {
                    n28 = GuildRankList[i].nRankNo;
                    for (var k = i + 1; k < GuildRankList.Count; k++)
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
                for (var i = 0; i < m_RankList.Count; i++)
                {
                    GuildRank = m_RankList[i];
                    for (var j = 0; j < GuildRank.MemberList.Count; j++)
                    {
                        PlayObject = M2Share.UserEngine.GetPlayObject(GuildRank.MemberList[j].sMemberName);
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

        public bool IsNotWarGuild(TGuild Guild)
        {
            for (var i = 0; i < GuildWarList.Count; i++)
            {
                if (GuildWarList[i].Guild == Guild)
                {
                    return false;
                }
            }
            return true;
        }

        public bool AllyGuild(TGuild Guild)
        {
            for (var i = 0; i < GuildAllList.Count; i++)
            {
                if (GuildAllList[i] == Guild)
                {
                    return false;
                }
            }
            GuildAllList.Add(Guild);
            SaveGuildInfoFile();
            return true;
        }

        public TWarGuild AddWarGuild(TGuild Guild)
        {
            TWarGuild result = null;
            TWarGuild WarGuild;
            if (Guild != null)
            {
                if (!IsAllyGuild(Guild))
                {
                    WarGuild = null;
                    for (var i = 0; i < GuildWarList.Count; i++)
                    {
                        if (GuildWarList[i].Guild == Guild)
                        {
                            WarGuild = GuildWarList[i];
                            WarGuild.dwWarTick = HUtil32.GetTickCount();
                            WarGuild.dwWarTime = M2Share.g_Config.dwGuildWarTime;// 10800000
                            SendGuildMsg("***" + Guild.sGuildName + "行会战争将持续三个小时。");
                            break;
                        }
                    }
                    if (WarGuild == null)
                    {
                        WarGuild = new TWarGuild
                        {
                            Guild = Guild,
                            dwWarTick = HUtil32.GetTickCount(),
                            dwWarTime = M2Share.g_Config.dwGuildWarTime// 10800000
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

        public void sub_499B4C(TGuild Guild)
        {
            SendGuildMsg("***" + Guild.sGuildName + "行会战争结束");
        }

        private int GetMemberCount()
        {
            var result = 0;
            TGuildRank GuildRank;
            for (var i = 0; i < m_RankList.Count; i++)
            {
                GuildRank = m_RankList[i];
                result += GuildRank.MemberList.Count;
            }
            return result;
        }

        private bool GetMemgerIsFull()
        {
            return Count >= M2Share.g_Config.nGuildMemberMaxLimit;
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