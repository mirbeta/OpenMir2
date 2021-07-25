using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using mSystemModule;
using SystemModule;

namespace M2Server
{
    public class TUserCastle
    {
        public TObjUnit[] m_Archer = new TObjUnit[12];
        public ArrayList m_AttackGuildList;
        public IList<TAttackerInfo> m_AttackWarList;
        public bool m_boShowOverMsg;
        /// <summary>
        /// 是否开始攻城
        /// </summary>
        private bool m_boStartWar;
        // 
        /// <summary>
        /// 
        /// </summary>
        public bool m_boStatus = false;
        public bool m_boUnderWar;
        private TObjUnit m_CenterWall;
        public DateTime m_ChangeDate;
        /// <summary>
        /// 城门状态
        /// </summary>
        public TDoorStatus m_DoorStatus;
        /// <summary>
        /// 是否已显示攻城结束信息
        /// </summary>
        public int m_dwSaveTick;
        public int m_dwStartCastleWarTick;
        public IList<string> m_EnvirList;
        public TObjUnit[] m_Guard = new TObjUnit[4];
        public DateTime m_IncomeToday;
        public TObjUnit m_LeftWall;
        public TObjUnit m_MainDoor;
        /// <summary>
        /// 城堡所在地图
        /// </summary>
        public TEnvirnoment m_MapCastle;
        /// 皇宫所在地图 
        public TEnvirnoment m_MapPalace;
        public TEnvirnoment m_MapSecret;
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
        public int m_nPower;
        public int m_nTechLevel;
        public int m_nTodayIncome;
        public int m_nTotalGold;
        public int m_nWarRangeX;
        public int m_nWarRangeY;
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
            m_AttackGuildList = new ArrayList();
            m_dwSaveTick = 0;
            m_nWarRangeX = M2Share.g_Config.nCastleWarRangeX;
            m_nWarRangeY = M2Share.g_Config.nCastleWarRangeY;
            m_EnvirList = new List<string>();
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
                if (m_MapPalace == null) M2Share.MainOutMessage(string.Format("皇宫地图{0}没找到！！！", m_sPalaceMap));
                m_MapSecret = M2Share.g_MapManager.FindMap(m_sSecretMap);
                if (m_MapSecret == null) M2Share.MainOutMessage(string.Format("密道地图{0}没找到！！！", m_sSecretMap));
                m_MapCastle = M2Share.g_MapManager.FindMap(m_sMapName);
                if (m_MapCastle != null)
                {
                    m_MainDoor.BaseObject =
                        M2Share.UserEngine.RegenMonsterByName(m_sMapName, m_MainDoor.nX, m_MainDoor.nY,
                            m_MainDoor.sName);
                    if (m_MainDoor.BaseObject != null)
                    {
                        m_MainDoor.BaseObject.m_WAbil.HP = m_MainDoor.nHP;
                        m_MainDoor.BaseObject.m_Castle = this;
                        if (m_MainDoor.nStatus) ((TCastleDoor) m_MainDoor.BaseObject).Open();
                    }
                    else
                    {
                        M2Share.ErrorMessage("[Error] UserCastle.Initialize MainDoor.UnitObj = nil");
                    }
                    m_LeftWall.BaseObject =
                        M2Share.UserEngine.RegenMonsterByName(m_sMapName, m_LeftWall.nX, m_LeftWall.nY,
                            m_LeftWall.sName);
                    if (m_LeftWall.BaseObject != null)
                    {
                        m_LeftWall.BaseObject.m_WAbil.HP = m_LeftWall.nHP;
                        m_LeftWall.BaseObject.m_Castle = this;
                    }
                    else
                    {
                        M2Share.ErrorMessage("[错误信息] 城堡初始化城门失败，检查怪物数据库里有没城门的设置: " + m_MainDoor.sName);
                    }

                    m_CenterWall.BaseObject = M2Share.UserEngine.RegenMonsterByName(m_sMapName, m_CenterWall.nX,
                        m_CenterWall.nY, m_CenterWall.sName);
                    if (m_CenterWall.BaseObject != null)
                    {
                        m_CenterWall.BaseObject.m_WAbil.HP = m_CenterWall.nHP;
                        m_CenterWall.BaseObject.m_Castle = this;
                    }
                    else
                    {
                        M2Share.ErrorMessage("[错误信息] 城堡初始化左城墙失败，检查怪物数据库里有没左城墙的设置: " + m_LeftWall.sName);
                    }

                    m_RightWall.BaseObject = M2Share.UserEngine.RegenMonsterByName(m_sMapName, m_RightWall.nX,
                        m_RightWall.nY, m_RightWall.sName);
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
                        ObjUnit.BaseObject =
                            M2Share.UserEngine.RegenMonsterByName(m_sMapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
                        if (ObjUnit.BaseObject != null)
                        {
                            ObjUnit.BaseObject.m_WAbil.HP = m_Archer[i].nHP;
                            ObjUnit.BaseObject.m_Castle = this;
                            ((TGuardUnit) ObjUnit.BaseObject).m_nX550 = ObjUnit.nX;
                            ((TGuardUnit) ObjUnit.BaseObject).m_nY554 = ObjUnit.nY;
                            ((TGuardUnit) ObjUnit.BaseObject).m_nDirection = 3;
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
                        ObjUnit.BaseObject =
                            M2Share.UserEngine.RegenMonsterByName(m_sMapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
                        if (ObjUnit.BaseObject != null)
                            ObjUnit.BaseObject.m_WAbil.HP = m_Guard[i].nHP;
                        else
                            M2Share.ErrorMessage("[错误信息] 城堡初始化守卫失败(检查怪物数据库里有没守卫怪物)");
                    }
                    for (var i = 0; i < m_MapCastle.m_DoorList.Count; i++)
                    {
                        Door = m_MapCastle.m_DoorList[i];
                        if (Math.Abs(Door.nX - m_nPalaceDoorX) <= 3 && Math.Abs(Door.nY - m_nPalaceDoorY) <= 3)
                            m_DoorStatus = Door.Status;
                    }
                }
                else
                {
                    M2Share.ErrorMessage(string.Format("[错误信息] 城堡所在地图不存在(检查地图配置文件里是否有地图{0}的设置)", m_sMapName));
                }
            }
        }

        private void LoadConfig()
        {
            var mapSplit = new[] {','};
            TObjUnit objUnit;
            var sMap = string.Empty;
            if (!Directory.Exists(M2Share.g_Config.sCastleDir + m_sConfigDir))
                Directory.CreateDirectory(M2Share.g_Config.sCastleDir + m_sConfigDir);
            var sConfigFile = "SabukW.txt";
            var sFileName = M2Share.g_Config.sCastleDir + m_sConfigDir + '\\' + sConfigFile;
            var CastleConf = new IniFile(sFileName);
            m_sName = CastleConf.ReadString("Setup", "CastleName", m_sName);
            m_sOwnGuild = CastleConf.ReadString("Setup", "OwnGuild", "");
            m_ChangeDate = CastleConf.ReadDateTime("Setup", "ChangeDate", DateTime.Now);
            m_WarDate = CastleConf.ReadDateTime("Setup", "WarDate", DateTime.Now);
            m_IncomeToday = CastleConf.ReadDateTime("Setup", "IncomeToday", DateTime.Now);
            m_nTotalGold = CastleConf.ReadInteger("Setup", "TotalGold", 0);
            m_nTodayIncome = CastleConf.ReadInteger("Setup", "TodayIncome", 0);
            var sMapList = CastleConf.ReadString("Defense", "CastleMapList", "");
            if (!string.IsNullOrEmpty(sMapList))
                while (!string.IsNullOrEmpty(sMapList))
                {
                    sMapList = HUtil32.GetValidStr3(sMapList, ref sMap, mapSplit);
                    if (sMap == "") break;
                    m_EnvirList.Add(sMap);
                }

            for (var i = 0; i < m_EnvirList.Count; i++)
                m_EnvirList[i] = M2Share.g_MapManager.FindMap(m_EnvirList[i]).sMapName;
            m_sMapName = CastleConf.ReadString("Defense", "CastleMap", "3");
            m_sHomeMap = CastleConf.ReadString("Defense", "CastleHomeMap", m_sHomeMap);
            m_nHomeX = CastleConf.ReadInteger("Defense", "CastleHomeX", m_nHomeX);
            m_nHomeY = CastleConf.ReadInteger("Defense", "CastleHomeY", m_nHomeY);
            m_nWarRangeX = CastleConf.ReadInteger("Defense", "CastleWarRangeX", m_nWarRangeX);
            m_nWarRangeY = CastleConf.ReadInteger("Defense", "CastleWarRangeY", m_nWarRangeY);
            m_sPalaceMap = CastleConf.ReadString("Defense", "CastlePlaceMap", m_sPalaceMap);
            m_sSecretMap = CastleConf.ReadString("Defense", "CastleSecretMap", m_sSecretMap);
            m_nPalaceDoorX = CastleConf.ReadInteger("Defense", "CastlePalaceDoorX", 631);
            m_nPalaceDoorY = CastleConf.ReadInteger("Defense", "CastlePalaceDoorY", 274);
            m_MainDoor = new TObjUnit();
            m_MainDoor.nX = CastleConf.ReadInteger<short>("Defense", "MainDoorX", (short) 672);
            m_MainDoor.nY = CastleConf.ReadInteger<short>("Defense", "MainDoorY", (short) 330);
            m_MainDoor.sName = CastleConf.ReadString("Defense", "MainDoorName", "MainDoor");
            m_MainDoor.nStatus = CastleConf.ReadBool("Defense", "MainDoorOpen", true);
            m_MainDoor.nHP = CastleConf.ReadInteger<ushort>("Defense", "MainDoorHP", (short) 2000);
            m_MainDoor.BaseObject = null;
            m_LeftWall = new TObjUnit();
            m_LeftWall.nX = CastleConf.ReadInteger<short>("Defense", "LeftWallX", (short) 624);
            m_LeftWall.nY = CastleConf.ReadInteger<short>("Defense", "LeftWallY", (short) 278);
            m_LeftWall.sName = CastleConf.ReadString("Defense", "LeftWallName", "LeftWall");
            m_LeftWall.nHP = CastleConf.ReadInteger<ushort>("Defense", "LeftWallHP", (short) 2000);
            m_LeftWall.BaseObject = null;
            m_CenterWall = new TObjUnit();
            m_CenterWall.nX = CastleConf.ReadInteger<short>("Defense", "CenterWallX", (short) 627);
            m_CenterWall.nY = CastleConf.ReadInteger<short>("Defense", "CenterWallY", (short) 278);
            m_CenterWall.sName = CastleConf.ReadString("Defense", "CenterWallName", "CenterWall");
            m_CenterWall.nHP = CastleConf.ReadInteger<ushort>("Defense", "CenterWallHP", (short) 2000);
            m_CenterWall.BaseObject = null;
            m_RightWall = new TObjUnit();
            m_RightWall.nX = CastleConf.ReadInteger<short>("Defense", "RightWallX", (short) 634);
            m_RightWall.nY = CastleConf.ReadInteger<short>("Defense", "RightWallY", (short) 271);
            m_RightWall.sName = CastleConf.ReadString("Defense", "RightWallName", "RightWall");
            m_RightWall.nHP = CastleConf.ReadInteger<ushort>("Defense", "RightWallHP", (short) 2000);
            m_RightWall.BaseObject = null;
            for (var i = m_Archer.GetLowerBound(0); i <= m_Archer.GetUpperBound(0); i++)
            {
                objUnit = new TObjUnit();
                objUnit.nX = CastleConf.ReadInteger<short>("Defense", "Archer_" + i + 1 + "_X", (short) 0);
                objUnit.nY = CastleConf.ReadInteger<short>("Defense", "Archer_" + i + 1 + "_Y", (short) 0);
                objUnit.sName = CastleConf.ReadString("Defense", "Archer_" + i + 1 + "_Name", "弓箭手");
                objUnit.nHP = CastleConf.ReadInteger<ushort>("Defense", "Archer_" + i + 1 + "_HP", (short) 2000);
                objUnit.BaseObject = null;
                m_Archer[i] = objUnit;
            }

            for (var i = m_Guard.GetLowerBound(0); i <= m_Guard.GetUpperBound(0); i++)
            {
                objUnit = new TObjUnit();
                objUnit.nX = CastleConf.ReadInteger<short>("Defense", "Guard_" + i + 1 + "_X", (short) 0);
                objUnit.nY = CastleConf.ReadInteger<short>("Defense", "Guard_" + i + 1 + "_Y", (short) 0);
                objUnit.sName = CastleConf.ReadString("Defense", "Guard_" + i + 1 + "_Name", "守卫");
                objUnit.nHP = CastleConf.ReadInteger<ushort>("Defense", "Guard_" + i + 1 + "_HP", (short) 2000);
                objUnit.BaseObject = null;
                m_Guard[i] = objUnit;
            }

            CastleConf = null;
            m_MasterGuild = M2Share.GuildManager.FindGuild(m_sOwnGuild);
        }

        private void SaveConfigFile()
        {
            var sMapList = string.Empty;
            if (!Directory.Exists(M2Share.g_Config.sCastleDir + m_sConfigDir))
                Directory.CreateDirectory(M2Share.g_Config.sCastleDir + m_sConfigDir);
            if (M2Share.g_MapManager.GetMapOfServerIndex(m_sMapName) != M2Share.nServerIndex) return;
            var sConfigFile = "SabukW.txt";
            var sFileName = M2Share.g_Config.sCastleDir + m_sConfigDir + '\\' + sConfigFile;
            var castleConf = new IniFile(sFileName);
            if (!string.IsNullOrEmpty(m_sName)) castleConf.WriteString("Setup", "CastleName", m_sName);
            if (!string.IsNullOrEmpty(m_sOwnGuild)) castleConf.WriteString("Setup", "OwnGuild", m_sOwnGuild);
            castleConf.WriteDateTime("Setup", "ChangeDate", m_ChangeDate);
            castleConf.WriteDateTime("Setup", "WarDate", m_WarDate);
            castleConf.WriteDateTime("Setup", "IncomeToday", m_IncomeToday);
            if (m_nTotalGold != 0) castleConf.WriteInteger("Setup", "TotalGold", m_nTotalGold);
            if (m_nTodayIncome != 0) castleConf.WriteInteger("Setup", "TodayIncome", m_nTodayIncome);
            for (var i = 0; i < m_EnvirList.Count; i++) sMapList = sMapList + m_EnvirList[i] + ',';
            if (!string.IsNullOrEmpty(sMapList)) castleConf.WriteString("Defense", "CastleMapList", sMapList);
            if (m_sMapName != "") castleConf.WriteString("Defense", "CastleMap", m_sMapName);
            if (m_sHomeMap != "") castleConf.WriteString("Defense", "CastleHomeMap", m_sHomeMap);
            if (m_nHomeX != 0) castleConf.WriteInteger("Defense", "CastleHomeX", m_nHomeX);
            if (m_nHomeY != 0) castleConf.WriteInteger("Defense", "CastleHomeY", m_nHomeY);
            if (m_nWarRangeX != 0) castleConf.WriteInteger("Defense", "CastleWarRangeX", m_nWarRangeX);
            if (m_nWarRangeY != 0) castleConf.WriteInteger("Defense", "CastleWarRangeY", m_nWarRangeY);
            if (m_sPalaceMap != "") castleConf.WriteString("Defense", "CastlePlaceMap", m_sPalaceMap);
            if (m_sSecretMap != "") castleConf.WriteString("Defense", "CastleSecretMap", m_sSecretMap);
            if (m_nPalaceDoorX != 0) castleConf.WriteInteger("Defense", "CastlePalaceDoorX", m_nPalaceDoorX);
            if (m_nPalaceDoorY != 0) castleConf.WriteInteger("Defense", "CastlePalaceDoorY", m_nPalaceDoorY);
            if (m_MainDoor.nX != 0) castleConf.WriteInteger("Defense", "MainDoorX", m_MainDoor.nX);
            if (m_MainDoor.nY != 0) castleConf.WriteInteger("Defense", "MainDoorY", m_MainDoor.nY);
            if (m_MainDoor.sName != "") castleConf.WriteString("Defense", "MainDoorName", m_MainDoor.sName);
            if (m_MainDoor.BaseObject != null)
            {
                castleConf.WriteBool("Defense", "MainDoorOpen", m_MainDoor.nStatus);
                castleConf.WriteInteger("Defense", "MainDoorHP", m_MainDoor.BaseObject.m_WAbil.HP);
            }
            if (m_LeftWall.nX != 0) castleConf.WriteInteger("Defense", "LeftWallX", m_LeftWall.nX);
            if (m_LeftWall.nY != 0) castleConf.WriteInteger("Defense", "LeftWallY", m_LeftWall.nY);
            if (m_LeftWall.sName != "") castleConf.WriteString("Defense", "LeftWallName", m_LeftWall.sName);
            if (m_LeftWall.BaseObject != null)
                castleConf.WriteInteger("Defense", "LeftWallHP", m_LeftWall.BaseObject.m_WAbil.HP);
            if (m_CenterWall.nX != 0) castleConf.WriteInteger("Defense", "CenterWallX", m_CenterWall.nX);
            if (m_CenterWall.nY != 0) castleConf.WriteInteger("Defense", "CenterWallY", m_CenterWall.nY);
            if (m_CenterWall.sName != "") castleConf.WriteString("Defense", "CenterWallName", m_CenterWall.sName);
            if (m_CenterWall.BaseObject != null)
                castleConf.WriteInteger("Defense", "CenterWallHP", m_CenterWall.BaseObject.m_WAbil.HP);
            if (m_RightWall.nX != 0) castleConf.WriteInteger("Defense", "RightWallX", m_RightWall.nX);
            if (m_RightWall.nY != 0) castleConf.WriteInteger("Defense", "RightWallY", m_RightWall.nY);
            if (m_RightWall.sName != "") castleConf.WriteString("Defense", "RightWallName", m_RightWall.sName);
            if (m_RightWall.BaseObject != null)
                castleConf.WriteInteger("Defense", "RightWallHP", m_RightWall.BaseObject.m_WAbil.HP);
            TObjUnit objUnit;
            for (var i = m_Archer.GetLowerBound(0); i <= m_Archer.GetUpperBound(0); i++)
            {
                objUnit = m_Archer[i];
                if (objUnit.nX != 0) castleConf.WriteInteger("Defense", "Archer_" + (i + 1) + "_X", objUnit.nX);
                if (objUnit.nY != 0) castleConf.WriteInteger("Defense", "Archer_" + (i + 1) + "_Y", objUnit.nY);
                if (objUnit.sName != "")
                    castleConf.WriteString("Defense", "Archer_" + (i + 1) + "_Name", objUnit.sName);
                if (objUnit.BaseObject != null)
                    castleConf.WriteInteger("Defense", "Archer_" + (i + 1) + "_HP", objUnit.BaseObject.m_WAbil.HP);
                else
                    castleConf.WriteInteger("Defense", "Archer_" + (i + 1) + "_HP", 0);
            }

            for (var i = m_Guard.GetLowerBound(0); i <= m_Guard.GetUpperBound(0); i++)
            {
                objUnit = m_Guard[i];
                if (objUnit.nX != 0) castleConf.WriteInteger("Defense", "Guard_" + (i + 1) + "_X", objUnit.nX);
                if (objUnit.nY != 0) castleConf.WriteInteger("Defense", "Guard_" + (i + 1) + "_Y", objUnit.nY);
                if (objUnit.sName != "") castleConf.WriteString("Defense", "Guard_" + (i + 1) + "_Name", objUnit.sName);
                if (objUnit.BaseObject != null)
                    castleConf.WriteInteger("Defense", "Guard_" + (i + 1) + "_HP", objUnit.BaseObject.m_WAbil.HP);
                else
                    castleConf.WriteInteger("Defense", "Guard_" + (i + 1) + "_HP", 0);
            }
            castleConf = null;
        }

        private void LoadAttackSabukWall()
        {
            var guildName = string.Empty;
            if (!Directory.Exists(M2Share.g_Config.sCastleDir + m_sConfigDir))
                Directory.CreateDirectory(M2Share.g_Config.sCastleDir + m_sConfigDir);
            var sConfigFile = "AttackSabukWall.txt";
            var sFileName = M2Share.g_Config.sCastleDir + m_sConfigDir + '\\' + sConfigFile;
            if (!File.Exists(sFileName)) return;
            var loadList = new StringList();
            loadList.LoadFromFile(sFileName);
            for (var i = 0; i < m_AttackWarList.Count; i++) m_AttackWarList[i] = null;
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
                attackerInfo.AttackDate = DateTime.TryParse(s20, out time) ? time : DateTime.Now;
                attackerInfo.AttackDate = Convert.ToDateTime(s20);
                attackerInfo.sGuildName = guildName;
                attackerInfo.Guild = guild;
                m_AttackWarList.Add(attackerInfo);
            }
            loadList = null;
        }

        private void SaveAttackSabukWall()
        {
            if (!Directory.Exists(M2Share.g_Config.sCastleDir + m_sConfigDir))
                Directory.CreateDirectory(M2Share.g_Config.sCastleDir + m_sConfigDir);
            var sConfigFile = "AttackSabukWall.txt";
            var sFileName = M2Share.g_Config.sCastleDir + m_sConfigDir + '\\' + sConfigFile;
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
                            M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_212, M2Share.nServerIndex, "");
                            s20 = string.Format(sWarStartMsg, new[] {m_sName});
                            M2Share.UserEngine.SendBroadCastMsgExt(s20, TMsgType.t_System);
                            M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_204, M2Share.nServerIndex, s20);
                            M2Share.MainOutMessage(s20);
                            MainDoorControl(true);
                        }
                    }
                }

                for (var i = m_Guard.GetLowerBound(0); i <= m_Guard.GetUpperBound(0); i++)
                    if (m_Guard[i].BaseObject != null && m_Guard[i].BaseObject.m_boGhost)
                        m_Guard[i].BaseObject = null;
                for (var i = m_Archer.GetLowerBound(0); i <= m_Archer.GetUpperBound(0); i++)
                    if (m_Archer[i].BaseObject != null && m_Archer[i].BaseObject.m_boGhost)
                        m_Archer[i].BaseObject = null;
                if (m_boUnderWar)
                {
                    if (m_LeftWall.BaseObject != null) m_LeftWall.BaseObject.m_boStoneMode = false;
                    if (m_CenterWall.BaseObject != null) m_CenterWall.BaseObject.m_boStoneMode = false;
                    if (m_RightWall.BaseObject != null) m_RightWall.BaseObject.m_boStoneMode = false;
                    if (!m_boShowOverMsg)
                        if (HUtil32.GetTickCount() - m_dwStartCastleWarTick > M2Share.g_Config.dwCastleWarTime -
                            M2Share.g_Config.dwShowCastleWarEndMsgTime) // 3 * 60 * 60 * 1000 - 10 * 60 * 1000
                        {
                            m_boShowOverMsg = true;
                            s20 = string.Format(sWarStopTimeMsg,
                                new object[] {m_sName, M2Share.g_Config.dwShowCastleWarEndMsgTime / (60 * 1000)});
                            M2Share.UserEngine.SendBroadCastMsgExt(s20, TMsgType.t_System);
                            M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_204, M2Share.nServerIndex, s20);
                            M2Share.MainOutMessage(s20);
                        }

                    if (HUtil32.GetTickCount() - m_dwStartCastleWarTick > M2Share.g_Config.dwCastleWarTime)
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

        public bool InCastleWarArea(TEnvirnoment envir, int nX, int nY)
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
            ;
        }

        // 检查是否为攻城方行会的联盟行会
        public bool IsAttackAllyGuild(TGuild Guild)
        {
            TGuild AttackGuild;
            var result = false;
            for (var i = 0; i < m_AttackGuildList.Count; i++)
            {
                AttackGuild = (TGuild) m_AttackGuildList[i];
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
                AttackGuild = (TGuild) m_AttackGuildList[i];
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
            if (HUtil32.GetTickCount() - m_dwStartCastleWarTick <= M2Share.g_Config.dwGetCastleTime) return result;
            var List14 = new ArrayList();
            M2Share.UserEngine.GetMapRageHuman(m_MapPalace, 0, 0, 1000, List14);
            result = true;
            for (var i = 0; i < List14.Count; i++)
            {
                var playObject = (TPlayObject) List14[i];
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
            var s10 = string.Format(sGetCastleMsg, new object[] {m_sName, m_sOwnGuild});
            M2Share.UserEngine.SendBroadCastMsgExt(s10, TMsgType.t_System);
            M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_204, M2Share.nServerIndex, s10);
            M2Share.MainOutMessage(s10);
        }

        public void StartWallconquestWar()
        {
            TPlayObject PlayObject;
            var ListC = new ArrayList();
            M2Share.UserEngine.GetMapRageHuman(m_MapPalace, m_nHomeX, m_nHomeY, 100, ListC);
            for (var i = 0; i < ListC.Count; i++)
            {
                PlayObject = (TPlayObject) ListC[i];
                PlayObject.RefShowName();
            }

            //ListC.Free;
        }

        public void StopWallconquestWar()
        {
            ArrayList ListC;
            TPlayObject PlayObject;
            string s14;
            const string sWallWarStop = "[{0} 攻城战已经结束]";
            m_boUnderWar = false;
            m_AttackGuildList.Clear();
            ListC = new ArrayList();
            M2Share.UserEngine.GetMapOfRangeHumanCount(m_MapCastle, m_nHomeX, m_nHomeY, 100);
            for (var i = 0; i < ListC.Count; i++)
            {
                PlayObject = (TPlayObject) ListC[i];
                PlayObject.ChangePKStatus(false);
                if (PlayObject.m_MyGuild != m_MasterGuild) PlayObject.MapRandomMove(PlayObject.m_sHomeMap, 0);
            }
            //ListC.Free;
            s14 = string.Format(sWallWarStop, new[] {m_sName});
            M2Share.UserEngine.SendBroadCastMsgExt(s14, TMsgType.t_System);
            M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_204, M2Share.nServerIndex, s14);
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
            return (short) (m_nHomeX - 4 + M2Share.RandomNumber.Random(9));
        }

        public short GetHomeY()
        {
            return (short) (m_nHomeY - 4 + M2Share.RandomNumber.Random(9));
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
            TAttackerInfo AttackerInfo;
            const string sMsg = "{0}年{1}月{2}日";
            var result = string.Empty;
            if (m_AttackWarList.Count <= 0) return result;
            AttackerInfo = m_AttackWarList[0];
            var Year = AttackerInfo.AttackDate.Year;
            var Month = AttackerInfo.AttackDate.Month;
            var Day = AttackerInfo.AttackDate.Day;
            result = string.Format(sMsg, new[] {Year, Month, Day});
            return result;
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
                    wYear = (short) Year;
                    wMonth = (short) Month;
                    wDay = (short) Day;
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

            if (HUtil32.GetTickCount() - m_dwSaveTick > 10 * 60 * 1000)
            {
                m_dwSaveTick = HUtil32.GetTickCount();
                if (M2Share.g_boGameLogGold)
                    M2Share.AddGameDataLog("23" + "\t" + '0' + "\t" + '0' + "\t" + '0' + "\t" + "autosave" + "\t" +
                                           grobal2.sSTRING_GOLDNAME + "\t" + m_nTotalGold + "\t" + '1' + "\t" + '0');
            }
        }

        public int WithDrawalGolds(TPlayObject PlayObject, int nGold)
        {
            var result = -1;
            if (nGold <= 0)
            {
                result = -4;
                return result;
            }
            if (m_MasterGuild == PlayObject.m_MyGuild && PlayObject.m_nGuildRankNo == 1 && nGold > 0)
            {
                if (nGold > 0 && nGold <= m_nTotalGold)
                {
                    if (PlayObject.m_nGold + nGold <= PlayObject.m_nGoldMax)
                    {
                        m_nTotalGold -= nGold;
                        PlayObject.IncGold(nGold);
                        if (M2Share.g_boGameLogGold)
                            M2Share.AddGameDataLog("22" + "\t" + PlayObject.m_sMapName + "\t" + PlayObject.m_nCurrX +
                                                   "\t" + PlayObject.m_nCurrY + "\t" + PlayObject.m_sCharName + "\t" +
                                                   grobal2.sSTRING_GOLDNAME + "\t" + nGold + "\t" + '1' + "\t" + '0');
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
                                                   grobal2.sSTRING_GOLDNAME + "\t" + nGold + "\t" + '1' + "\t" + '0');
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

        public void MainDoorControl(bool boClose)
        {
            if (m_MainDoor.BaseObject != null && !m_MainDoor.BaseObject.m_boGhost)
            {
                if (boClose)
                {
                    if (((TCastleDoor) m_MainDoor.BaseObject).m_boOpened) ((TCastleDoor) m_MainDoor.BaseObject).Close();
                }
                else
                {
                    if (!((TCastleDoor) m_MainDoor.BaseObject).m_boOpened) ((TCastleDoor) m_MainDoor.BaseObject).Open();
                }
            }
        }

        public bool RepairDoor()
        {
            var result = false;
            var CastleDoor = m_MainDoor;
            if (CastleDoor.BaseObject == null || m_boUnderWar ||
                CastleDoor.BaseObject.m_WAbil.HP >= CastleDoor.BaseObject.m_WAbil.MaxHP) return result;
            if (!CastleDoor.BaseObject.m_boDeath)
            {
                if (HUtil32.GetTickCount() - CastleDoor.BaseObject.m_dwStruckTick > 60 * 1000)
                {
                    CastleDoor.BaseObject.m_WAbil.HP = CastleDoor.BaseObject.m_WAbil.MaxHP;
                    ((TCastleDoor) CastleDoor.BaseObject).RefStatus();
                    result = true;
                }
            }
            else
            {
                if (HUtil32.GetTickCount() - CastleDoor.BaseObject.m_dwStruckTick > 60 * 1000)
                {
                    CastleDoor.BaseObject.m_WAbil.HP = CastleDoor.BaseObject.m_WAbil.MaxHP;
                    CastleDoor.BaseObject.m_boDeath = false;
                    ((TCastleDoor) CastleDoor.BaseObject).m_boOpened = false;
                    ((TCastleDoor) CastleDoor.BaseObject).RefStatus();
                    result = true;
                }
            }

            return result;
        }

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

            if (Wall == null || m_boUnderWar || Wall.m_WAbil.HP >= Wall.m_WAbil.MaxHP) return result;
            if (!Wall.m_boDeath)
            {
                if (HUtil32.GetTickCount() - Wall.m_dwStruckTick > 60 * 1000)
                {
                    Wall.m_WAbil.HP = Wall.m_WAbil.MaxHP;
                    ((TWallStructure) Wall).RefStatus();
                    result = true;
                }
            }
            else
            {
                if (HUtil32.GetTickCount() - Wall.m_dwStruckTick > 60 * 1000)
                {
                    Wall.m_WAbil.HP = Wall.m_WAbil.MaxHP;
                    Wall.m_boDeath = false;
                    ((TWallStructure) Wall).RefStatus();
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
            M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_212, M2Share.nServerIndex, "");
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