using System;
using System.Collections.Generic;
using System.IO;
using SystemModule;
using SystemModule.Common;

namespace GameSvr
{
    public class TUserCastle
    {
        public TObjUnit[] m_Archer = new TObjUnit[12];
        /// <summary>
        /// 攻城行会列表
        /// </summary>
        private IList<TGuild> m_AttackGuildList;
        /// <summary>
        /// 攻城列表
        /// </summary>
        private IList<TAttackerInfo> m_AttackWarList;
        /// <summary>
        /// 是否显示攻城战役结束消息
        /// </summary>
        public bool m_boShowOverMsg;
        /// <summary>
        /// 是否开始攻城
        /// </summary>
        public bool m_boStartWar;
        public bool m_boUnderWar;
        public TObjUnit m_CenterWall;
        public DateTime m_ChangeDate;
        /// <summary>
        /// 城门状态
        /// </summary>
        public TDoorStatus m_DoorStatus;
        /// <summary>
        /// 是否已显示攻城结束信息
        /// </summary>
        private int m_dwSaveTick;
        public int m_dwStartCastleWarTick;
        public IList<string> m_EnvirList;
        public TObjUnit[] m_Guard = new TObjUnit[4];
        public DateTime m_IncomeToday;
        public TObjUnit m_LeftWall;
        public TObjUnit m_MainDoor;
        /// <summary>
        /// 城堡所在地图
        /// </summary>
        public Envirnoment m_MapCastle;
        /// <summary>
        /// 皇宫所在地图
        /// </summary>
        public Envirnoment m_MapPalace;
        private Envirnoment m_MapSecret;
        /// <summary>
        /// 所属行会名称
        /// </summary>
        public TGuild m_MasterGuild;
        /// <summary>
        /// 行会回城点X
        /// </summary>
        public int m_nHomeX;
        /// <summary>
        /// 行会回城点Y
        /// </summary>
        public int m_nHomeY;
        public int m_nPalaceDoorX;
        /// <summary>
        /// 科技等级
        /// </summary>
        public int m_nPalaceDoorY;
        private int m_nPower;
        private int m_nTechLevel;
        /// <summary>
        /// 今日收入
        /// </summary>
        public int m_nTodayIncome;
        /// <summary>
        /// 收入多少金币
        /// </summary>
        public int m_nTotalGold;
        public int m_nWarRangeX;
        public int m_nWarRangeY;
        /// <summary>
        /// 皇宫右城墙
        /// </summary>
        public TObjUnit m_RightWall;
        /// <summary>
        /// 皇宫门状态
        /// </summary>
        public string m_sConfigDir = string.Empty;
        /// <summary>
        /// 行会回城点地图
        /// </summary>
        public string m_sHomeMap = string.Empty;
        /// <summary>
        /// 城堡所在地图名
        /// </summary>
        public string m_sMapName = string.Empty;
        /// <summary>
        /// 城堡名称
        /// </summary>
        public string m_sName = string.Empty;
        public string m_sOwnGuild = string.Empty;
        /// <summary>
        /// 皇宫所在地图
        /// </summary>
        public string m_sPalaceMap = string.Empty;
        public string m_sSecretMap = string.Empty;
        public DateTime m_WarDate;
        private readonly CastleConfManager castleConf;

        public TUserCastle(string sCastleDir)
        {
            m_MasterGuild = null;
            m_sHomeMap = M2Share.g_Config.sCastleHomeMap; // '3'
            m_nHomeX = M2Share.g_Config.nCastleHomeX; // 644
            m_nHomeY = M2Share.g_Config.nCastleHomeY; // 290
            m_sName = M2Share.g_Config.sCastleName; // '沙巴克'
            m_sConfigDir = sCastleDir;
            m_sPalaceMap = "0150";
            m_sSecretMap = "D701";
            m_MapCastle = null;
            m_DoorStatus = null;
            m_boStartWar = false;
            m_boUnderWar = false;
            m_boShowOverMsg = false;
            m_AttackWarList = new List<TAttackerInfo>();
            m_AttackGuildList = new List<TGuild>();
            m_dwSaveTick = 0;
            m_nWarRangeX = M2Share.g_Config.nCastleWarRangeX;
            m_nWarRangeY = M2Share.g_Config.nCastleWarRangeY;
            m_EnvirList = new List<string>();
            var filePath = Path.Combine(M2Share.g_Config.sCastleDir, m_sConfigDir);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var sConfigFile = "SabukW.txt";
            castleConf = new CastleConfManager(Path.Combine(filePath, sConfigFile));
        }

        public int nTechLevel
        {
            get { return m_nTechLevel; }
            set { SetTechLevel(value); }
        }

        public int nPower
        {
            get { return m_nPower; }
            set { SetPower(value); }
        }

        public void Initialize()
        {
            TObjUnit ObjUnit;
            TDoorInfo Door;
            LoadConfig();
            LoadAttackSabukWall();
            if (M2Share.g_MapManager.GetMapOfServerIndex(m_sMapName) == M2Share.nServerIndex)
            {
                m_MapPalace = M2Share.g_MapManager.FindMap(m_sPalaceMap);
                if (m_MapPalace == null) M2Share.MainOutMessage($"皇宫地图{m_sPalaceMap}没找到!!!");
                m_MapSecret = M2Share.g_MapManager.FindMap(m_sSecretMap);
                if (m_MapSecret == null) M2Share.MainOutMessage($"密道地图{m_sSecretMap}没找到!!!");
                m_MapCastle = M2Share.g_MapManager.FindMap(m_sMapName);
                if (m_MapCastle != null)
                {
                    m_MainDoor.BaseObject = M2Share.UserEngine.RegenMonsterByName(m_sMapName, m_MainDoor.nX, m_MainDoor.nY, m_MainDoor.sName);
                    if (m_MainDoor.BaseObject != null)
                    {
                        m_MainDoor.BaseObject.m_WAbil.HP = m_MainDoor.nHP;
                        m_MainDoor.BaseObject.m_Castle = this;
                        if (m_MainDoor.nStatus)
                        {
                            ((TCastleDoor)m_MainDoor.BaseObject).Open();
                        }
                    }
                    else
                    {
                        M2Share.ErrorMessage("[Error] UserCastle.Initialize MainDoor.UnitObj = nil");
                    }
                    m_LeftWall.BaseObject = M2Share.UserEngine.RegenMonsterByName(m_sMapName, m_LeftWall.nX, m_LeftWall.nY, m_LeftWall.sName);
                    if (m_LeftWall.BaseObject != null)
                    {
                        m_LeftWall.BaseObject.m_WAbil.HP = m_LeftWall.nHP;
                        m_LeftWall.BaseObject.m_Castle = this;
                    }
                    else
                    {
                        M2Share.ErrorMessage("[错误信息] 城堡初始化城门失败，检查怪物数据库里有没城门的设置: " + m_MainDoor.sName);
                    }
                    m_CenterWall.BaseObject = M2Share.UserEngine.RegenMonsterByName(m_sMapName, m_CenterWall.nX, m_CenterWall.nY, m_CenterWall.sName);
                    if (m_CenterWall.BaseObject != null)
                    {
                        m_CenterWall.BaseObject.m_WAbil.HP = m_CenterWall.nHP;
                        m_CenterWall.BaseObject.m_Castle = this;
                    }
                    else
                    {
                        M2Share.ErrorMessage("[错误信息] 城堡初始化左城墙失败，检查怪物数据库里有没左城墙的设置: " + m_LeftWall.sName);
                    }
                    m_RightWall.BaseObject = M2Share.UserEngine.RegenMonsterByName(m_sMapName, m_RightWall.nX, m_RightWall.nY, m_RightWall.sName);
                    if (m_RightWall.BaseObject != null)
                    {
                        m_RightWall.BaseObject.m_WAbil.HP = m_RightWall.nHP;
                        m_RightWall.BaseObject.m_Castle = this;
                    }
                    else
                    {
                        M2Share.ErrorMessage("[错误信息] 城堡初始化中城墙失败，检查怪物数据库里有没中城墙的设置: " + m_CenterWall.sName);
                    }
                    for (var i = m_Archer.GetLowerBound(0); i <= m_Archer.GetUpperBound(0); i++)
                    {
                        ObjUnit = m_Archer[i];
                        if (ObjUnit.nHP <= 0) continue;
                        ObjUnit.BaseObject = M2Share.UserEngine.RegenMonsterByName(m_sMapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
                        if (ObjUnit.BaseObject != null)
                        {
                            ObjUnit.BaseObject.m_WAbil.HP = m_Archer[i].nHP;
                            ObjUnit.BaseObject.m_Castle = this;
                            ((TGuardUnit)ObjUnit.BaseObject).m_nX550 = ObjUnit.nX;
                            ((TGuardUnit)ObjUnit.BaseObject).m_nY554 = ObjUnit.nY;
                            ((TGuardUnit)ObjUnit.BaseObject).m_nDirection = 3;
                        }
                        else
                        {
                            M2Share.ErrorMessage("[错误信息] 城堡初始化弓箭手失败，检查怪物数据库里有没弓箭手的设置: " + ObjUnit.sName);
                        }
                    }

                    for (var i = m_Guard.GetLowerBound(0); i <= m_Guard.GetUpperBound(0); i++)
                    {
                        ObjUnit = m_Guard[i];
                        if (ObjUnit.nHP <= 0) continue;
                        ObjUnit.BaseObject = M2Share.UserEngine.RegenMonsterByName(m_sMapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
                        if (ObjUnit.BaseObject != null)
                            ObjUnit.BaseObject.m_WAbil.HP = m_Guard[i].nHP;
                        else
                            M2Share.ErrorMessage("[错误信息] 城堡初始化守卫失败(检查怪物数据库里有没守卫怪物)");
                    }
                    for (var i = 0; i < m_MapCastle.m_DoorList.Count; i++)
                    {
                        Door = m_MapCastle.m_DoorList[i];
                        if (Math.Abs(Door.nX - m_nPalaceDoorX) <= 3 && Math.Abs(Door.nY - m_nPalaceDoorY) <= 3)
                        {
                            m_DoorStatus = Door.Status;
                        }
                    }
                }
                else
                {
                    M2Share.ErrorMessage($"[错误信息] 城堡所在地图不存在(检查地图配置文件里是否有地图{m_sMapName}的设置)");
                }
            }
        }

        private void LoadConfig()
        {
            castleConf.LoadConfig(this);
            m_MasterGuild = M2Share.GuildManager.FindGuild(m_sOwnGuild);
        }

        private void SaveConfigFile()
        {
            castleConf.SaveConfig(this);
        }

        private void LoadAttackSabukWall()
        {
            var sabukwallPath = Path.Combine(M2Share.g_Config.sCastleDir, m_sConfigDir);
            var guildName = string.Empty;
            if (!Directory.Exists(sabukwallPath))
                Directory.CreateDirectory(sabukwallPath);
            var sConfigFile = "AttackSabukWall.txt";
            var sFileName = Path.Combine(sabukwallPath, sConfigFile);
            if (!File.Exists(sFileName)) return;
            var loadList = new StringList();
            loadList.LoadFromFile(sFileName);
            for (var i = 0; i < m_AttackWarList.Count; i++)
            {
                m_AttackWarList[i] = null;
            }
            m_AttackWarList.Clear();
            for (var i = 0; i < loadList.Count; i++)
            {
                var sData = loadList[i];
                var s20 = HUtil32.GetValidStr3(sData, ref guildName, new[] { " ", "\t" });
                var guild = M2Share.GuildManager.FindGuild(guildName);
                if (guild == null) continue;
                var attackerInfo = new TAttackerInfo();
                HUtil32.ArrestStringEx(s20, '\"', '\"', ref s20);
                var time = DateTime.Now;
                if (DateTime.TryParse(s20, out time))
                {
                    attackerInfo.AttackDate = time;
                }
                else
                {
                    attackerInfo.AttackDate = DateTime.Now;
                }
                attackerInfo.sGuildName = guildName;
                attackerInfo.Guild = guild;
                m_AttackWarList.Add(attackerInfo);
            }
        }

        private void SaveAttackSabukWall()
        {
            var sabukwallPath = Path.Combine(M2Share.g_Config.sCastleDir, m_sConfigDir);
            if (!Directory.Exists(sabukwallPath))
                Directory.CreateDirectory(sabukwallPath);
            var sConfigFile = "AttackSabukWall.txt";
            var sFileName = Path.Combine(sabukwallPath, sConfigFile);
            var loadLis = new StringList();
            for (var i = 0; i < m_AttackWarList.Count; i++)
            {
                var attackerInfo = m_AttackWarList[i];
                loadLis.Add(attackerInfo.sGuildName + "       \"" + attackerInfo.AttackDate + '\"');
            }
            loadLis.SaveToFile(sFileName);
            loadLis = null;
        }

        public void Run()
        {
            string s20;
            const string sWarStartMsg = "[{0} 攻城战已经开始]";
            const string sWarStopTimeMsg = "[{0} 攻城战离结束还有{1}分钟]";
            const string sExceptionMsg = "[Exception] TUserCastle::Run";
            try
            {
                if (M2Share.nServerIndex != M2Share.g_MapManager.GetMapOfServerIndex(m_sMapName)) return;
                var Year = DateTime.Now.Year;
                var Month = DateTime.Now.Month;
                var Day = DateTime.Now.Day;
                var wYear = m_IncomeToday.Year;
                var wMonth = m_IncomeToday.Month;
                var wDay = m_IncomeToday.Day;
                if (Year != wYear || Month != wMonth || Day != wDay)
                {
                    m_nTodayIncome = 0;
                    m_IncomeToday = DateTime.Now;
                    m_boStartWar = false;
                }
                if (!m_boStartWar && !m_boUnderWar)
                {
                    var hour = DateTime.Now.Hour;
                    if (hour == M2Share.g_Config.nStartCastlewarTime) // 20
                    {
                        m_boStartWar = true;
                        m_AttackGuildList.Clear();
                        for (var i = m_AttackWarList.Count - 1; i >= 0; i--)
                        {
                            var attackerInfo = m_AttackWarList[i];
                            wYear = attackerInfo.AttackDate.Year;
                            wMonth = attackerInfo.AttackDate.Month;
                            wDay = attackerInfo.AttackDate.Day;
                            if (Year == wYear && Month == wMonth && Day == wDay)
                            {
                                m_boUnderWar = true;
                                m_boShowOverMsg = false;
                                m_WarDate = DateTime.Now;
                                m_dwStartCastleWarTick = HUtil32.GetTickCount();
                                m_AttackGuildList.Add(attackerInfo.Guild);
                                attackerInfo = null;
                                m_AttackWarList.RemoveAt(i);
                            }
                        }
                        if (m_boUnderWar)
                        {
                            m_AttackGuildList.Add(m_MasterGuild);
                            StartWallconquestWar();
                            SaveAttackSabukWall();
                            M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_212, M2Share.nServerIndex, "");
                            s20 = string.Format(sWarStartMsg, m_sName);
                            M2Share.UserEngine.SendBroadCastMsgExt(s20, TMsgType.t_System);
                            M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_204, M2Share.nServerIndex, s20);
                            M2Share.MainOutMessage(s20);
                            MainDoorControl(true);
                        }
                    }
                }
                for (var i = m_Guard.GetLowerBound(0); i <= m_Guard.GetUpperBound(0); i++)
                {
                    if (m_Guard[i].BaseObject != null && m_Guard[i].BaseObject.m_boGhost)
                    {
                        m_Guard[i].BaseObject = null;
                    }
                }
                for (var i = m_Archer.GetLowerBound(0); i <= m_Archer.GetUpperBound(0); i++)
                {
                    if (m_Archer[i].BaseObject != null && m_Archer[i].BaseObject.m_boGhost)
                    {
                        m_Archer[i].BaseObject = null;
                    }
                }
                if (m_boUnderWar)
                {
                    if (m_LeftWall.BaseObject != null) m_LeftWall.BaseObject.m_boStoneMode = false;
                    if (m_CenterWall.BaseObject != null) m_CenterWall.BaseObject.m_boStoneMode = false;
                    if (m_RightWall.BaseObject != null) m_RightWall.BaseObject.m_boStoneMode = false;
                    if (!m_boShowOverMsg)
                    {
                        if ((HUtil32.GetTickCount() - m_dwStartCastleWarTick) > (M2Share.g_Config.dwCastleWarTime - M2Share.g_Config.dwShowCastleWarEndMsgTime)) // 3 * 60 * 60 * 1000 - 10 * 60 * 1000
                        {
                            m_boShowOverMsg = true;
                            s20 = string.Format(sWarStopTimeMsg,  m_sName, M2Share.g_Config.dwShowCastleWarEndMsgTime / (60 * 1000));
                            M2Share.UserEngine.SendBroadCastMsgExt(s20, TMsgType.t_System);
                            M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_204, M2Share.nServerIndex, s20);
                            M2Share.MainOutMessage(s20);
                        }
                    }
                    if ((HUtil32.GetTickCount() - m_dwStartCastleWarTick) > M2Share.g_Config.dwCastleWarTime)
                    {
                        StopWallconquestWar();
                    }
                }
                else
                {
                    if (m_LeftWall.BaseObject != null) m_LeftWall.BaseObject.m_boStoneMode = true;
                    if (m_CenterWall.BaseObject != null) m_CenterWall.BaseObject.m_boStoneMode = true;
                    if (m_RightWall.BaseObject != null) m_RightWall.BaseObject.m_boStoneMode = true;
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

        public void Save()
        {
            SaveConfigFile();
            SaveAttackSabukWall();
        }

        public bool InCastleWarArea(Envirnoment envir, int nX, int nY)
        {
            if (envir == null)
            {
                return false;
            }
            if (envir == m_MapCastle && Math.Abs(m_nHomeX - nX) < m_nWarRangeX &&
                Math.Abs(m_nHomeY - nY) < m_nWarRangeY) return true;
            if (envir == m_MapPalace || envir == m_MapSecret) return true;
            for (var i = 0; i < m_EnvirList.Count; i++) // 增加取得城堡所有地图列表
                if (m_EnvirList[i] == envir.sMapName)
                    return true;
            return false;
        }

        public bool IsMember(TBaseObject cert)
        {
            return IsMasterGuild(cert.m_MyGuild);
        }

        // 检查是否为攻城方行会的联盟行会
        public bool IsAttackAllyGuild(TGuild Guild)
        {
            TGuild AttackGuild;
            var result = false;
            for (var i = 0; i < m_AttackGuildList.Count; i++)
            {
                AttackGuild = m_AttackGuildList[i];
                if (AttackGuild != m_MasterGuild && AttackGuild.IsAllyGuild(Guild))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        // 检查是否为攻城方行会
        public bool IsAttackGuild(TGuild Guild)
        {
            TGuild AttackGuild;
            var result = false;
            for (var i = 0; i < m_AttackGuildList.Count; i++)
            {
                AttackGuild = m_AttackGuildList[i];
                if (AttackGuild != m_MasterGuild && AttackGuild == Guild)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public bool CanGetCastle(TGuild guild)
        {
            var result = false;
            if ((HUtil32.GetTickCount() - m_dwStartCastleWarTick) <= M2Share.g_Config.dwGetCastleTime)
            {
                return result;
            }
            var List14 = new List<TBaseObject>();
            M2Share.UserEngine.GetMapRageHuman(m_MapPalace, 0, 0, 1000, List14);
            result = true;
            for (var i = 0; i < List14.Count; i++)
            {
                var playObject = (TPlayObject)List14[i];
                if (!playObject.m_boDeath && playObject.m_MyGuild != guild)
                {
                    result = false;
                    break;
                }
            }
            List14 = null;
            return result;
        }

        public void GetCastle(TGuild Guild)
        {
            const string sGetCastleMsg = "[{0} 已被 {1} 占领]";
            var oldGuild = m_MasterGuild;
            m_MasterGuild = Guild;
            m_sOwnGuild = Guild.sGuildName;
            m_ChangeDate = DateTime.Now;
            SaveConfigFile();
            if (oldGuild != null) oldGuild.RefMemberName();
            if (m_MasterGuild != null) m_MasterGuild.RefMemberName();
            var s10 = string.Format(sGetCastleMsg,  m_sName, m_sOwnGuild);
            M2Share.UserEngine.SendBroadCastMsgExt(s10, TMsgType.t_System);
            M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_204, M2Share.nServerIndex, s10);
            M2Share.MainOutMessage(s10);
        }

        public void StartWallconquestWar()
        {
            TPlayObject PlayObject;
            var ListC = new List<TBaseObject>();
            M2Share.UserEngine.GetMapRageHuman(m_MapPalace, m_nHomeX, m_nHomeY, 100, ListC);
            for (var i = 0; i < ListC.Count; i++)
            {
                PlayObject = (TPlayObject)ListC[i];
                PlayObject.RefShowName();
            }
        }

        /// <summary>
        /// 停止沙巴克攻城战役
        /// </summary>
        public void StopWallconquestWar()
        {
            TPlayObject PlayObject;
            const string sWallWarStop = "[{0} 攻城战已经结束]";
            m_boUnderWar = false;
            m_AttackGuildList.Clear();
            IList<TPlayObject> ListC = new List<TPlayObject>();
            M2Share.UserEngine.GetMapOfRangeHumanCount(m_MapCastle, m_nHomeX, m_nHomeY, 100);
            for (var i = 0; i < ListC.Count; i++)
            {
                PlayObject = ListC[i];
                PlayObject.ChangePKStatus(false);
                if (PlayObject.m_MyGuild != m_MasterGuild)
                {
                    PlayObject.MapRandomMove(PlayObject.m_sHomeMap, 0);
                }
            }
            var s14 = string.Format(sWallWarStop, m_sName);
            M2Share.UserEngine.SendBroadCastMsgExt(s14, TMsgType.t_System);
            M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_204, M2Share.nServerIndex, s14);
            M2Share.MainOutMessage(s14);
        }

        public int InPalaceGuildCount()
        {
            return m_AttackGuildList.Count;
        }

        public bool IsDefenseAllyGuild(TGuild guild)
        {
            var result = false;
            if (!m_boUnderWar) return result;
            if (m_MasterGuild != null) // 如果未开始攻城，则无效
                result = m_MasterGuild.IsAllyGuild(guild);
            return result;
        }

        // 检查是否为守城方行会
        public bool IsDefenseGuild(TGuild guild)
        {
            var result = false;
            if (!m_boUnderWar) return result;
            if (guild == m_MasterGuild) // 如果未开始攻城，则无效
                result = true;
            return result;
        }

        public bool IsMasterGuild(TGuild guild)
        {
            return m_MasterGuild != null && m_MasterGuild == guild;
        }

        public short GetHomeX()
        {
            return (short)(m_nHomeX - 4 + M2Share.RandomNumber.Random(9));
        }

        public short GetHomeY()
        {
            return (short)(m_nHomeY - 4 + M2Share.RandomNumber.Random(9));
        }

        public string GetMapName()
        {
            return m_sMapName;
        }

        public bool CheckInPalace(int nX, int nY, TBaseObject cert)
        {
            TObjUnit ObjUnit;
            var result = IsMasterGuild(cert.m_MyGuild);
            if (result) return result;
            ObjUnit = m_LeftWall;
            if (ObjUnit.BaseObject != null && ObjUnit.BaseObject.m_boDeath && ObjUnit.BaseObject.m_nCurrX == nX &&
                ObjUnit.BaseObject.m_nCurrY == nY) result = true;
            ObjUnit = m_CenterWall;
            if (ObjUnit.BaseObject != null && ObjUnit.BaseObject.m_boDeath && ObjUnit.BaseObject.m_nCurrX == nX &&
                ObjUnit.BaseObject.m_nCurrY == nY) result = true;
            ObjUnit = m_RightWall;
            if (ObjUnit.BaseObject != null && ObjUnit.BaseObject.m_boDeath && ObjUnit.BaseObject.m_nCurrX == nX &&
                ObjUnit.BaseObject.m_nCurrY == nY) result = true;
            return result;
        }

        public string GetWarDate()
        {
            const string sMsg = "{0}年{1}月{2}日";
            var result = string.Empty;
            if (m_AttackWarList.Count <= 0) return result;
            var AttackerInfo = m_AttackWarList[0];
            var Year = AttackerInfo.AttackDate.Year;
            var Month = AttackerInfo.AttackDate.Month;
            var Day = AttackerInfo.AttackDate.Day;
            return string.Format(sMsg, Year, Month, Day);
        }

        public string GetAttackWarList()
        {
            TAttackerInfo AttackerInfo;
            var result = string.Empty;
            short wYear = 0;
            short wMonth = 0;
            short wDay = 0;
            var n10 = 0;
            for (var i = 0; i < m_AttackWarList.Count; i++)
            {
                AttackerInfo = m_AttackWarList[i];
                var Year = AttackerInfo.AttackDate.Year;
                var Month = AttackerInfo.AttackDate.Month;
                var Day = AttackerInfo.AttackDate.Day;
                if (Year != wYear || Month != wMonth || Day != wDay)
                {
                    wYear = (short)Year;
                    wMonth = (short)Month;
                    wDay = (short)Day;
                    if (result != "") result = result + '\\';
                    result = result + wYear + '年' + wMonth + '月' + wDay + "日\\";
                    n10 = 0;
                }
                if (n10 > 40)
                {
                    result = result + '\\';
                    n10 = 0;
                }
                var s20 = '\"' + AttackerInfo.sGuildName + '\"';
                n10 += s20.Length;
                result = result + s20;
            }
            return result;
        }

        /// <summary>
        /// 增加每日收入
        /// </summary>
        /// <param name="nGold"></param>
        public void IncRateGold(int nGold)
        {
            var nInGold = HUtil32.Round(nGold * (M2Share.g_Config.nCastleTaxRate / 100));
            if (m_nTodayIncome + nInGold <= M2Share.g_Config.nCastleOneDayGold)
            {
                m_nTodayIncome += nInGold;
            }
            else
            {
                if (m_nTodayIncome >= M2Share.g_Config.nCastleOneDayGold)
                {
                    nInGold = 0;
                }
                else
                {
                    nInGold = M2Share.g_Config.nCastleOneDayGold - m_nTodayIncome;
                    m_nTodayIncome = M2Share.g_Config.nCastleOneDayGold;
                }
            }
            if (nInGold > 0)
            {
                if (m_nTotalGold + nInGold < M2Share.g_Config.nCastleGoldMax)
                    m_nTotalGold += nInGold;
                else
                    m_nTotalGold = M2Share.g_Config.nCastleGoldMax;
            }
            if ((HUtil32.GetTickCount() - m_dwSaveTick) > 10 * 60 * 1000)
            {
                m_dwSaveTick = HUtil32.GetTickCount();
                if (M2Share.g_boGameLogGold)
                    M2Share.AddGameDataLog("23" + "\t" + '0' + "\t" + '0' + "\t" + '0' + "\t" + "autosave" + "\t" +
                                           Grobal2.sSTRING_GOLDNAME + "\t" + m_nTotalGold + "\t" + '1' + "\t" + '0');
            }
        }

        /// <summary>
        /// 提取每日收入
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="nGold"></param>
        /// <returns></returns>
        public int WithDrawalGolds(TPlayObject PlayObject, int nGold)
        {
            var result = -1;
            if (nGold <= 0)
            {
                result = -4;
                return result;
            }
            if (m_MasterGuild == PlayObject.m_MyGuild && PlayObject.m_nGuildRankNo == 1)
            {
                if (nGold <= m_nTotalGold)
                {
                    if (PlayObject.m_nGold + nGold <= PlayObject.m_nGoldMax)
                    {
                        m_nTotalGold -= nGold;
                        PlayObject.IncGold(nGold);
                        if (M2Share.g_boGameLogGold)
                            M2Share.AddGameDataLog("22" + "\t" + PlayObject.m_sMapName + "\t" + PlayObject.m_nCurrX +
                                                   "\t" + PlayObject.m_nCurrY + "\t" + PlayObject.m_sCharName + "\t" +
                                                   Grobal2.sSTRING_GOLDNAME + "\t" + nGold + "\t" + '1' + "\t" + '0');
                        PlayObject.GoldChanged();
                        result = 1;
                    }
                    else
                    {
                        result = -3;
                    }
                }
                else
                {
                    result = -2;
                }
            }
            return result;
        }

        public int ReceiptGolds(TPlayObject PlayObject, int nGold)
        {
            var result = -1;
            if (nGold <= 0)
            {
                result = -4;
                return result;
            }
            if (m_MasterGuild == PlayObject.m_MyGuild && PlayObject.m_nGuildRankNo == 1 && nGold > 0)
            {
                if (nGold <= PlayObject.m_nGold)
                {
                    if (m_nTotalGold + nGold <= M2Share.g_Config.nCastleGoldMax)
                    {
                        PlayObject.m_nGold -= nGold;
                        m_nTotalGold += nGold;
                        if (M2Share.g_boGameLogGold)
                            M2Share.AddGameDataLog("23" + "\t" + PlayObject.m_sMapName + "\t" + PlayObject.m_nCurrX +
                                                   "\t" + PlayObject.m_nCurrY + "\t" + PlayObject.m_sCharName + "\t" +
                                                   Grobal2.sSTRING_GOLDNAME + "\t" + nGold + "\t" + '1' + "\t" + '0');
                        PlayObject.GoldChanged();
                        result = 1;
                    }
                    else
                    {
                        result = -3;
                    }
                }
                else
                {
                    result = -2;
                }
            }
            return result;
        }

        /// <summary>
        /// 城门控制
        /// </summary>
        /// <param name="boClose"></param>
        public void MainDoorControl(bool boClose)
        {
            if (m_MainDoor.BaseObject != null && !m_MainDoor.BaseObject.m_boGhost)
            {
                if (boClose)
                {
                    if (((TCastleDoor)m_MainDoor.BaseObject).m_boOpened)
                    {
                        ((TCastleDoor)m_MainDoor.BaseObject).Close();
                    }
                }
                else
                {
                    if (!((TCastleDoor)m_MainDoor.BaseObject).m_boOpened)
                    {
                        ((TCastleDoor)m_MainDoor.BaseObject).Open();
                    }
                }
            }
        }

        /// <summary>
        /// 修复城门
        /// </summary>
        /// <returns></returns>
        public bool RepairDoor()
        {
            var result = false;
            var CastleDoor = m_MainDoor;
            if (CastleDoor.BaseObject == null || m_boUnderWar || CastleDoor.BaseObject.m_WAbil.HP >= CastleDoor.BaseObject.m_WAbil.MaxHP)
            {
                return result;
            }
            if (!CastleDoor.BaseObject.m_boDeath)
            {
                if ((HUtil32.GetTickCount() - CastleDoor.BaseObject.m_dwStruckTick) > 60 * 1000)
                {
                    CastleDoor.BaseObject.m_WAbil.HP = CastleDoor.BaseObject.m_WAbil.MaxHP;
                    ((TCastleDoor)CastleDoor.BaseObject).RefStatus();
                    result = true;
                }
            }
            else
            {
                if ((HUtil32.GetTickCount() - CastleDoor.BaseObject.m_dwStruckTick) > 60 * 1000)
                {
                    CastleDoor.BaseObject.m_WAbil.HP = CastleDoor.BaseObject.m_WAbil.MaxHP;
                    CastleDoor.BaseObject.m_boDeath = false;
                    ((TCastleDoor)CastleDoor.BaseObject).m_boOpened = false;
                    ((TCastleDoor)CastleDoor.BaseObject).RefStatus();
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 修复城墙
        /// </summary>
        /// <param name="nWallIndex"></param>
        /// <returns></returns>
        public bool RepairWall(int nWallIndex)
        {
            var result = false;
            TBaseObject Wall = null;
            switch (nWallIndex)
            {
                case 1:
                    Wall = m_LeftWall.BaseObject;
                    break;
                case 2:
                    Wall = m_CenterWall.BaseObject;
                    break;
                case 3:
                    Wall = m_RightWall.BaseObject;
                    break;
            }
            if (Wall == null || m_boUnderWar || Wall.m_WAbil.HP >= Wall.m_WAbil.MaxHP)
            {
                return result;
            }
            if (!Wall.m_boDeath)
            {
                if ((HUtil32.GetTickCount() - Wall.m_dwStruckTick) > 60 * 1000)
                {
                    Wall.m_WAbil.HP = Wall.m_WAbil.MaxHP;
                    ((TWallStructure)Wall).RefStatus();
                    result = true;
                }
            }
            else
            {
                if ((HUtil32.GetTickCount() - Wall.m_dwStruckTick) > 60 * 1000)
                {
                    Wall.m_WAbil.HP = Wall.m_WAbil.MaxHP;
                    Wall.m_boDeath = false;
                    ((TWallStructure)Wall).RefStatus();
                    result = true;
                }
            }
            return result;
        }

        public bool AddAttackerInfo(TGuild Guild)
        {
            var result = false;
            if (InAttackerList(Guild)) return result;
            var AttackerInfo = new TAttackerInfo();
            AttackerInfo.AttackDate = M2Share.AddDateTimeOfDay(DateTime.Now, M2Share.g_Config.nStartCastleWarDays);
            AttackerInfo.sGuildName = Guild.sGuildName;
            AttackerInfo.Guild = Guild;
            m_AttackWarList.Add(AttackerInfo);
            SaveAttackSabukWall();
            M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_212, M2Share.nServerIndex, "");
            result = true;
            return result;
        }

        private bool InAttackerList(TGuild Guild)
        {
            var result = false;
            for (var i = 0; i < m_AttackWarList.Count; i++)
            {
                if (m_AttackWarList[i].Guild == Guild)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private void SetPower(int nPower)
        {
            m_nPower = nPower;
        }

        private void SetTechLevel(int nLevel)
        {
            m_nTechLevel = nLevel;
        }
    }
}