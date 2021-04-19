using mSystemModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace M2Server
{
    public class TGuild
    {
        public int Count { get { return GetMemberCount(); } }
        public bool IsFull { get { return GetMemgerIsFull(); } }
        public int nBuildPoint
        {
            get { return m_nBuildPoint; }
            set { SetBuildPoint(value); }
        }
        public int nAurae
        {
            get { return m_nAurae; }
            set { SetAuraePoint(value); }
        }
        public int nStability
        {
            get { return m_nStability; }
            set { SetStabilityPoint(value); }
        }
        public int nFlourishing
        {
            get { return m_nFlourishing; }
            set { SetFlourishPoint(value); }
        }
        public int nChiefItemCount
        {
            get { return m_nChiefItemCount; }
            set { SetChiefItemCount(value); }
        }
        public string sGuildName = string.Empty;
        public ArrayList NoticeList = null;
        public IList<TWarGuild> GuildWarList = null;
        public IList<TGuild> GuildAllList = null;
        public IList<TGuildRank> m_RankList = null;
        // 0x14 职位列表
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
                    if (GuildRank.MemberList[j].m_sCharName == sName)
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
            bool result;
            m_nBuildPoint = m_Config.ReadInteger("Guild", "BuildPoint", m_nBuildPoint);
            m_nAurae = m_Config.ReadInteger("Guild", "Aurae", m_nAurae);
            m_nStability = m_Config.ReadInteger("Guild", "Stability", m_nStability);
            m_nFlourishing = m_Config.ReadInteger("Guild", "Flourishing", m_nFlourishing);
            m_nChiefItemCount = m_Config.ReadInteger("Guild", "ChiefItemCount", m_nChiefItemCount);
            result = true;
            return result;
        }

        public bool LoadGuildFile(string sGuildFileName)
        {
            bool result;
            int I;
            StringList LoadList;
            var s18 = string.Empty;
            var s1C = string.Empty;
            var s20 = string.Empty;
            var s24 = string.Empty;
            string sFileName;
            int n28;
            int n2C;
            TWarGuild GuildWar;
            TGuildRank GuildRank;
            TGuild Guild;
            result = false;
            GuildRank = null;
            sFileName = M2Share.g_Config.sGuildDir + sGuildFileName;
            if (!File.Exists(sFileName))
            {
                return result;
            }
            ClearRank();
            NoticeList.Clear();
            for (I = 0; I < GuildWarList.Count; I++)
            {
                //Dispose(((GuildWarList.Values[I]) as TWarGuild));
            }
            // for
            GuildWarList.Clear();
            GuildAllList.Clear();
            n28 = 0;
            n2C = 0;
            s24 = "";
            LoadList = new StringList();
            LoadList.LoadFromFile(sFileName);
            for (I = 0; I < LoadList.Count; I++)
            {
                s18 = LoadList[I];
                if ((s18 == "") || (s18[0] == ';'))
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
                s18 = s18.Substring(2 - 1, s18.Length - 1);
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
                        if ((n2C > 0) && (s24 != ""))
                        {
                            if (s24.Length > 30)
                            {
                                // Jacky 限制职倍的长度
                                // 30
                                s24 = s24.Substring(1 - 1, M2Share.g_Config.nGuildRankNameLen);
                            }
                            if (GuildRank == null)
                            {
                                GuildRank = new TGuildRank
                                {
                                    nRankNo = n2C,
                                    sRankName = s24,
                                    MemberList = new List<TPlayObject>()
                                };
                                m_RankList.Add(GuildRank);
                            }
                            while (s18 != "")
                            {
                                s18 = HUtil32.GetValidStr3(s18, ref s1C, new char[] { ' ', ',' });
                                if (s1C == "")
                                {
                                    break;
                                }
                                var play = M2Share.UserEngine.GetPlayObject(s1C);
                                GuildRank.MemberList.Add(play);
                            }
                        }
                        break;
                }
            }
            //LoadList.Free;
            result = true;
            return result;
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
                    BaseObject = GuildRank.MemberList[j];
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
            ArrayList SaveList;
            SaveList = new ArrayList
            {
                M2Share.g_Config.sGuildNotice
            };
            //for (I = 0; I < NoticeList.Count; I ++ )
            //{
            //    SaveList.Add('+' + NoticeList[I]);
            //}
            //SaveList.Add(' ');
            //SaveList.Add(M2Share.g_Config.sGuildWar);
            //for (I = 0; I < GuildWarList.Count; I ++ )
            //{
            //    WarGuild = ((GuildWarList.Values[I]) as TWarGuild);
            //    n14 = WarGuild.dwWarTime - (HUtil32.GetTickCount() - WarGuild.dwWarTick);
            //    if (n14 <= 0)
            //    {
            //        continue;
            //    }
            //    SaveList.Add('+' + GuildWarList[I] + ' ' + (n14).ToString());
            //}
            //SaveList.Add(' ');
            //SaveList.Add(M2Share.g_Config.sGuildAll);
            //for (I = 0; I < GuildAllList.Count; I ++ )
            //{
            //    SaveList.Add('+' + GuildAllList[I]);
            //}
            //SaveList.Add(' ');
            //SaveList.Add(M2Share.g_Config.sGuildMember);
            //for (I = 0; I < m_RankList.Count; I ++ )
            //{
            //    GuildRank = m_RankList[I];
            //    SaveList.Add('#' + (GuildRank.nRankNo).ToString() + ' ' + GuildRank.sRankName);
            //    for (II = 0; II < GuildRank.MemberList.Count; II ++ )
            //    {
            //        SaveList.Add('+' + GuildRank.MemberList[II]);
            //    }
            //}
            //try
            //{
            //    SaveList.SaveToFile(sFileName);
            //}
            //catch
            //{
            //    M2Share.MainOutMessage("保存行会信息失败！！！ " + sFileName);
            //}
            //SaveList.Free;
        }

        public void SendGuildMsg(string sMsg)
        {
            int I;
            int II;
            TGuildRank GuildRank;
            TBaseObject BaseObject;
            int nCheckCode;
            nCheckCode = 0;
            try
            {
                if (M2Share.g_Config.boShowPreFixMsg)
                {
                    sMsg = M2Share.g_Config.sGuildMsgPreFix + sMsg;
                }
                // if RankList = nil then exit;
                nCheckCode = 1;
                for (I = 0; I < m_RankList.Count; I++)
                {
                    GuildRank = m_RankList[I];
                    nCheckCode = 2;
                    // if GuildRank.MemberList = nil then Continue;
                    for (II = 0; II < GuildRank.MemberList.Count; II++)
                    {
                        nCheckCode = 3;
                        BaseObject = GuildRank.MemberList[II];
                        if (BaseObject == null)
                        {
                            continue;
                        }
                        nCheckCode = 4;
                        if (BaseObject.m_boBanGuildChat)
                        {
                            nCheckCode = 5;
                            BaseObject.SendMsg(BaseObject, grobal2.RM_GUILDMESSAGE, 0, M2Share.g_Config.btGuildMsgFColor, M2Share.g_Config.btGuildMsgBColor, 0, sMsg);
                            nCheckCode = 6;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.MainOutMessage("[Exceptiion] TGuild.SendGuildMsg CheckCode: " + nCheckCode + " GuildName = " + sGuildName + " Msg = " + sMsg);
                M2Share.MainOutMessage(e.Message, MessageType.Error);
            }
        }

        // 行会领取装备数量
        public bool SetGuildInfo(string sChief)
        {
            bool result;
            TGuildRank GuildRank;
            if (m_RankList.Count == 0)
            {
                GuildRank = new TGuildRank
                {
                    nRankNo = 1,
                    sRankName = M2Share.g_Config.sGuildChief,
                    MemberList = new List<TPlayObject>()
                };
                var playObject = M2Share.UserEngine.GetPlayObject(sChief);
                GuildRank.MemberList.Add(playObject);
                m_RankList.Add(GuildRank);
                SaveGuildInfoFile();
            }
            result = true;
            return result;
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
                    if (GuildRank.MemberList[j].m_sCharName == PlayObject.m_sCharName)
                    {
                        GuildRank.MemberList[j] = PlayObject;
                        nRankNo = GuildRank.nRankNo;
                        result = GuildRank.sRankName;
                        // PlayObject.RefShowName();
                        PlayObject.SendMsg(PlayObject, grobal2.RM_CHANGEGUILDNAME, 0, 0, 0, 0, "");
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
            result = GuildRank.MemberList[0].m_sCharName;
            return result;
        }

        public void CheckSaveGuildFile()
        {
            if (boChanged && ((HUtil32.GetTickCount() - dwSaveTick) > 30 * 1000))
            {
                boChanged = false;
                SaveGuildInfoFile();
            }
        }

        public void DelHumanObj(TPlayObject PlayObject)
        {
            CheckSaveGuildFile();
            //for (var I = 0; I < m_RankList.Count; I ++ )
            //{
            //    GuildRank = m_RankList[I];
            //    for (var II = 0; II < GuildRank.MemberList.Count; II ++ )
            //    {
            //        if (((TPlayObject)(GuildRank.MemberList.Values[II])) == PlayObject)
            //        {
            //            GuildRank.MemberList.Values[II] = null;
            //            return;
            //        }
            //    }
            //}
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
                SaveGuildFile(M2Share.g_Config.sGuildDir + sGuildName + '.' + HUtil32.GetTickCount() + ".bak");
            }
            for (var i = 0; i < m_RankList.Count; i++)
            {
                GuildRank = m_RankList[i];
                for (var j = 0; j < GuildRank.MemberList.Count; j++)
                {
                    PlayObject = GuildRank.MemberList[j];
                    if (PlayObject != null)
                    {
                        PlayObject.m_MyGuild = null;
                        PlayObject.RefRankInfo(0, "");
                        PlayObject.RefShowName();// 10/31
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
                    MemberList = new List<TPlayObject>()
                };
                m_RankList.Add(GuildRank18);
            }
            GuildRank18.MemberList.Add(PlayObject);
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
                    if (GuildRank.MemberList[j].m_sCharName == sHumName)
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
            if (GuildRank.MemberList[0].m_sCharName == sHumName)
            {
                BackupGuildFile();
                result = true;
            }
            return result;
        }

        public void UpdateRank_ClearRankList(ref IList<TGuildRank> RankList)
        {
            TGuildRank GuildRank;
            for (var i = 0; i < RankList.Count; i++)
            {
                GuildRank = RankList[i];
                GuildRank.MemberList = null;
                GuildRank = null;
            }
            //RankList.Free;
        }

        public int UpdateRank(string sRankData)
        {
            int result;
            int I;
            int II;
            int III;
            IList<TGuildRank> GuildRankList;
            TGuildRank GuildRank;
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
            result = -1;
            GuildRankList = new List<TGuildRank>();
            GuildRank = null;
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
                if (sRankInfo[0] == '#')
                {
                    // 取得职称的名称
                    sRankInfo = sRankInfo.Substring(1, sRankInfo.Length - 1);
                    sRankInfo = HUtil32.GetValidStr3(sRankInfo, ref sRankNo, new char[] { ' ', '<' });
                    sRankInfo = HUtil32.GetValidStr3(sRankInfo, ref sRankName, new char[] { '<', '>' });
                    if (sRankName.Length > 30)
                    {
                        // Jacky 限制职倍的长度
                        sRankName = sRankName.Substring(0, 30);
                    }
                    if (GuildRank != null)
                    {
                        GuildRankList.Add(GuildRank);
                    }
                    GuildRank = new TGuildRank
                    {
                        nRankNo = HUtil32.Str_ToInt(sRankNo, 99),
                        sRankName = sRankName.Trim(),
                        MemberList = new List<TPlayObject>()
                    };
                    continue;
                }
                if (GuildRank == null)
                {
                    continue;
                }
                I = 0;
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
                        //GuildRank.MemberList.Add(sMemberName);
                    }
                    I++;
                    if (I >= 10)
                    {
                        break;
                    }
                }
            }
            if (GuildRank != null)
            {
                GuildRankList.Add(GuildRank);
            }
            // 0049931F  校验成员列表是否有改变，如果未修改则退出
            if (m_RankList.Count == GuildRankList.Count)
            {
                boCheckChange = true;
                for (I = 0; I < m_RankList.Count; I++)
                {
                    GuildRank = m_RankList[I];
                    NewGuildRank = GuildRankList[I];
                    if ((GuildRank.nRankNo == NewGuildRank.nRankNo) && (GuildRank.sRankName == NewGuildRank.sRankName) && (GuildRank.MemberList.Count == NewGuildRank.MemberList.Count))
                    {
                        for (II = 0; II < GuildRank.MemberList.Count; II++)
                        {
                            if (GuildRank.MemberList[II] != NewGuildRank.MemberList[II])
                            {
                                boCheckChange = false;
                                // 如果有改变则将其置为FALSE
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
            // 0049943D 检查行会掌门职业是否为空
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
                    for (I = 0; I < GuildRank.MemberList.Count; I++)
                    {
                        if (M2Share.UserEngine.GetPlayObject(GuildRank.MemberList[I].m_sCharName) == null)
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
                for (I = 0; I < m_RankList.Count; I++)
                {
                    GuildRank = m_RankList[I];
                    boCheckChange = true;
                    for (II = 0; II < GuildRank.MemberList.Count; II++)
                    {
                        boCheckChange = false;
                        sMemberName = GuildRank.MemberList[II].m_sCharName;
                        n2C++;
                        for (III = 0; III < GuildRankList.Count; III++)
                        {
                            // 搜索新列表
                            NewGuildRank = GuildRankList[III];
                            for (n28 = 0; n28 < NewGuildRank.MemberList.Count; n28++)
                            {
                                if (NewGuildRank.MemberList[n28].m_sCharName == sMemberName)
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
                            // 原列表中的人物名称是否在新的列表中
                            result = -6;
                            break;
                        }
                    }
                    if (!boCheckChange)
                    {
                        break;
                    }
                }
                for (I = 0; I < GuildRankList.Count; I++)
                {
                    GuildRank = GuildRankList[I];
                    boCheckChange = true;
                    for (II = 0; II < GuildRank.MemberList.Count; II++)
                    {
                        boCheckChange = false;
                        sMemberName = GuildRank.MemberList[II].m_sCharName;
                        n30++;
                        for (III = 0; III < GuildRankList.Count; III++)
                        {
                            NewGuildRank = GuildRankList[III];
                            for (n28 = 0; n28 < NewGuildRank.MemberList.Count; n28++)
                            {
                                if (NewGuildRank.MemberList[n28].m_sCharName == sMemberName)
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
                if ((result == 0) && (n2C != n30))
                {
                    result = -6;
                }
            }
            if (result == 0)
            {
                // 检查职位号是否重复及非法
                for (I = 0; I < GuildRankList.Count; I++)
                {
                    n28 = GuildRankList[I].nRankNo;
                    for (III = I + 1; III < GuildRankList.Count; III++)
                    {
                        if ((GuildRankList[III].nRankNo == n28) || (n28 <= 0) || (n28 > 99))
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
                for (I = 0; I < m_RankList.Count; I++)
                {
                    GuildRank = m_RankList[I];
                    for (III = 0; III < GuildRank.MemberList.Count; III++)
                    {
                        PlayObject = M2Share.UserEngine.GetPlayObject(GuildRank.MemberList[III].m_sCharName);
                        if (PlayObject != null)
                        {
                            GuildRank.MemberList[III] = PlayObject;
                            PlayObject.RefRankInfo(GuildRank.nRankNo, GuildRank.sRankName);
                            PlayObject.RefShowName();// 10/31
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
            var result = false;
            for (var i = 0; i < GuildWarList.Count; i++)
            {
                if (GuildWarList[i].Guild == Guild)
                {
                    return result;
                }
            }
            result = true;
            return result;
        }

        public bool AllyGuild(TGuild Guild)
        {
            var result = false;
            for (var I = 0; I < GuildAllList.Count; I++)
            {
                if (GuildAllList[I] == Guild)
                {
                    return result;
                }
            }
            GuildAllList.Add(Guild);
            SaveGuildInfoFile();
            result = true;
            return result;
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
                            WarGuild.dwWarTick = HUtil32.GetTickCount();// 10800000
                            WarGuild.dwWarTime = M2Share.g_Config.dwGuildWarTime;
                            SendGuildMsg("***" + Guild.sGuildName + "行会战争将持续三个小时。");
                            break;
                        }
                    }
                    if (WarGuild == null)
                    {
                        WarGuild = new TWarGuild
                        {
                            Guild = Guild,
                            dwWarTick = HUtil32.GetTickCount(),// 10800000
                            dwWarTime = M2Share.g_Config.dwGuildWarTime
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
            var result = false;
            if (Count >= M2Share.g_Config.nGuildMemberMaxLimit)
            {
                result = true;
            }
            return result;
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