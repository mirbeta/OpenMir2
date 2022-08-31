using SystemModule;
using SystemModule.Common;

namespace GameSvr.Castle
{
    public class CastleConfManager : IniFile
    {
        public CastleConfManager(string fileName) : base(fileName)
        {

        }

        public void LoadConfig(TUserCastle userCastle)
        {
            var mapSplit = new[] { ',' };
            userCastle.m_sName = ReadString("Setup", "CastleName", userCastle.m_sName);
            userCastle.m_sOwnGuild = ReadString("Setup", "OwnGuild", "");
            userCastle.m_ChangeDate = ReadDateTime("Setup", "ChangeDate", DateTime.Now);
            userCastle.m_WarDate = ReadDateTime("Setup", "WarDate", DateTime.Now);
            userCastle.m_IncomeToday = ReadDateTime("Setup", "IncomeToday", DateTime.Now);
            userCastle.m_nTotalGold = ReadInteger("Setup", "TotalGold", 0);
            userCastle.m_nTodayIncome = ReadInteger("Setup", "TodayIncome", 0);
            var sMapList = ReadString("Defense", "CastleMapList", "");
            var sMap = string.Empty;
            if (!string.IsNullOrEmpty(sMapList))
            {
                while (!string.IsNullOrEmpty(sMapList))
                {
                    sMapList = HUtil32.GetValidStr3(sMapList, ref sMap, mapSplit);
                    if (sMap == "") break;
                    userCastle.m_EnvirList.Add(sMap);
                }
            }
            for (var i = 0; i < userCastle.m_EnvirList.Count; i++)
            {
                userCastle.m_EnvirList[i] = M2Share.MapManager.FindMap(userCastle.m_EnvirList[i]).SMapName;
            }
            userCastle.m_sMapName = ReadString("Defense", "CastleMap", "3");
            userCastle.m_sHomeMap = ReadString("Defense", "CastleHomeMap", userCastle.m_sHomeMap);
            userCastle.m_nHomeX = ReadInteger("Defense", "CastleHomeX", userCastle.m_nHomeX);
            userCastle.m_nHomeY = ReadInteger("Defense", "CastleHomeY", userCastle.m_nHomeY);
            userCastle.m_nWarRangeX = ReadInteger("Defense", "CastleWarRangeX", userCastle.m_nWarRangeX);
            userCastle.m_nWarRangeY = ReadInteger("Defense", "CastleWarRangeY", userCastle.m_nWarRangeY);
            userCastle.m_sPalaceMap = ReadString("Defense", "CastlePlaceMap", userCastle.m_sPalaceMap);
            userCastle.m_sSecretMap = ReadString("Defense", "CastleSecretMap", userCastle.m_sSecretMap);
            userCastle.m_nPalaceDoorX = ReadInteger("Defense", "CastlePalaceDoorX", 631);
            userCastle.m_nPalaceDoorY = ReadInteger("Defense", "CastlePalaceDoorY", 274);
            userCastle.m_MainDoor = new TObjUnit();
            userCastle.m_MainDoor.nX = Read<short>("Defense", "MainDoorX", (short)672);
            userCastle.m_MainDoor.nY = Read<short>("Defense", "MainDoorY", (short)330);
            userCastle.m_MainDoor.sName = ReadString("Defense", "MainDoorName", "MainDoor");
            userCastle.m_MainDoor.nStatus = ReadBool("Defense", "MainDoorOpen", true);
            userCastle.m_MainDoor.nHP = Read<ushort>("Defense", "MainDoorHP", (short)2000);
            userCastle.m_MainDoor.BaseObject = null;
            userCastle.m_LeftWall = new TObjUnit();
            userCastle.m_LeftWall.nX = Read<short>("Defense", "LeftWallX", (short)624);
            userCastle.m_LeftWall.nY = Read<short>("Defense", "LeftWallY", (short)278);
            userCastle.m_LeftWall.sName = ReadString("Defense", "LeftWallName", "LeftWall");
            userCastle.m_LeftWall.nHP = Read<ushort>("Defense", "LeftWallHP", (short)2000);
            userCastle.m_LeftWall.BaseObject = null;
            userCastle.m_CenterWall = new TObjUnit();
            userCastle.m_CenterWall.nX = Read<short>("Defense", "CenterWallX", (short)627);
            userCastle.m_CenterWall.nY = Read<short>("Defense", "CenterWallY", (short)278);
            userCastle.m_CenterWall.sName = ReadString("Defense", "CenterWallName", "CenterWall");
            userCastle.m_CenterWall.nHP = Read<ushort>("Defense", "CenterWallHP", (short)2000);
            userCastle.m_CenterWall.BaseObject = null;
            userCastle.m_RightWall = new TObjUnit();
            userCastle.m_RightWall.nX = Read<short>("Defense", "RightWallX", (short)634);
            userCastle.m_RightWall.nY = Read<short>("Defense", "RightWallY", (short)271);
            userCastle.m_RightWall.sName = ReadString("Defense", "RightWallName", "RightWall");
            userCastle.m_RightWall.nHP = Read<ushort>("Defense", "RightWallHP", (short)2000);
            userCastle.m_RightWall.BaseObject = null;
            for (var i = 0; i < userCastle.Archer.Length; i++)
            {
                var objUnit = new TObjUnit();
                objUnit.nX = Read<short>("Defense", "Archer_" + i + 1 + "_X", (short)0);
                objUnit.nY = Read<short>("Defense", "Archer_" + i + 1 + "_Y", (short)0);
                objUnit.sName = ReadString("Defense", "Archer_" + i + 1 + "_Name", "弓箭手");
                objUnit.nHP = Read<ushort>("Defense", "Archer_" + i + 1 + "_HP", (short)2000);
                objUnit.BaseObject = null;
                userCastle.Archer[i] = objUnit;
            }

            for (var i = 0; i < userCastle.m_Guard.Length; i++)
            {
                var objUnit = new TObjUnit();
                objUnit.nX = Read<short>("Defense", "Guard_" + i + 1 + "_X", (short)0);
                objUnit.nY = Read<short>("Defense", "Guard_" + i + 1 + "_Y", (short)0);
                objUnit.sName = ReadString("Defense", "Guard_" + i + 1 + "_Name", "守卫");
                objUnit.nHP = Read<ushort>("Defense", "Guard_" + i + 1 + "_HP", (short)2000);
                objUnit.BaseObject = null;
                userCastle.m_Guard[i] = objUnit;
            }
        }

        public void SaveConfig(TUserCastle userCastle)
        {
            var filePath = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sCastleDir, userCastle.m_sConfigDir);
            var sMapList = string.Empty;
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            if (M2Share.MapManager.GetMapOfServerIndex(userCastle.m_sMapName) != M2Share.nServerIndex) return;
            if (!string.IsNullOrEmpty(userCastle.m_sName))
            {
                WriteString("Setup", "CastleName", userCastle.m_sName);
            }
            if (!string.IsNullOrEmpty(userCastle.m_sOwnGuild))
            {
                WriteString("Setup", "OwnGuild", userCastle.m_sOwnGuild);
            }
            WriteDateTime("Setup", "ChangeDate", userCastle.m_ChangeDate);
            WriteDateTime("Setup", "WarDate", userCastle.m_WarDate);
            WriteDateTime("Setup", "IncomeToday", userCastle.m_IncomeToday);
            if (userCastle.m_nTotalGold != 0) WriteInteger("Setup", "TotalGold", userCastle.m_nTotalGold);
            if (userCastle.m_nTodayIncome != 0) WriteInteger("Setup", "TodayIncome", userCastle.m_nTodayIncome);
            for (var i = 0; i < userCastle.m_EnvirList.Count; i++) sMapList = sMapList + userCastle.m_EnvirList[i] + ',';
            if (!string.IsNullOrEmpty(sMapList)) WriteString("Defense", "CastleMapList", sMapList);
            if (userCastle.m_sMapName != "") WriteString("Defense", "CastleMap", userCastle.m_sMapName);
            if (userCastle.m_sHomeMap != "") WriteString("Defense", "CastleHomeMap", userCastle.m_sHomeMap);
            if (userCastle.m_nHomeX != 0) WriteInteger("Defense", "CastleHomeX", userCastle.m_nHomeX);
            if (userCastle.m_nHomeY != 0) WriteInteger("Defense", "CastleHomeY", userCastle.m_nHomeY);
            if (userCastle.m_nWarRangeX != 0) WriteInteger("Defense", "CastleWarRangeX", userCastle.m_nWarRangeX);
            if (userCastle.m_nWarRangeY != 0) WriteInteger("Defense", "CastleWarRangeY", userCastle.m_nWarRangeY);
            if (userCastle.m_sPalaceMap != "") WriteString("Defense", "CastlePlaceMap", userCastle.m_sPalaceMap);
            if (userCastle.m_sSecretMap != "") WriteString("Defense", "CastleSecretMap", userCastle.m_sSecretMap);
            if (userCastle.m_nPalaceDoorX != 0) WriteInteger("Defense", "CastlePalaceDoorX", userCastle.m_nPalaceDoorX);
            if (userCastle.m_nPalaceDoorY != 0) WriteInteger("Defense", "CastlePalaceDoorY", userCastle.m_nPalaceDoorY);
            if (userCastle.m_MainDoor.nX != 0) WriteInteger("Defense", "MainDoorX", userCastle.m_MainDoor.nX);
            if (userCastle.m_MainDoor.nY != 0) WriteInteger("Defense", "MainDoorY", userCastle.m_MainDoor.nY);
            if (userCastle.m_MainDoor.sName != "") WriteString("Defense", "MainDoorName", userCastle.m_MainDoor.sName);
            if (userCastle.m_MainDoor.BaseObject != null)
            {
                WriteBool("Defense", "MainDoorOpen", userCastle.m_MainDoor.nStatus);
                WriteInteger("Defense", "MainDoorHP", userCastle.m_MainDoor.BaseObject.m_WAbil.HP);
            }
            if (userCastle.m_LeftWall.nX != 0) WriteInteger("Defense", "LeftWallX", userCastle.m_LeftWall.nX);
            if (userCastle.m_LeftWall.nY != 0) WriteInteger("Defense", "LeftWallY", userCastle.m_LeftWall.nY);
            if (userCastle.m_LeftWall.sName != "") WriteString("Defense", "LeftWallName", userCastle.m_LeftWall.sName);
            if (userCastle.m_LeftWall.BaseObject != null)
                WriteInteger("Defense", "LeftWallHP", userCastle.m_LeftWall.BaseObject.m_WAbil.HP);
            if (userCastle.m_CenterWall.nX != 0) WriteInteger("Defense", "CenterWallX", userCastle.m_CenterWall.nX);
            if (userCastle.m_CenterWall.nY != 0) WriteInteger("Defense", "CenterWallY", userCastle.m_CenterWall.nY);
            if (userCastle.m_CenterWall.sName != "") WriteString("Defense", "CenterWallName", userCastle.m_CenterWall.sName);
            if (userCastle.m_CenterWall.BaseObject != null)
                WriteInteger("Defense", "CenterWallHP", userCastle.m_CenterWall.BaseObject.m_WAbil.HP);
            if (userCastle.m_RightWall.nX != 0) WriteInteger("Defense", "RightWallX", userCastle.m_RightWall.nX);
            if (userCastle.m_RightWall.nY != 0) WriteInteger("Defense", "RightWallY", userCastle.m_RightWall.nY);
            if (userCastle.m_RightWall.sName != "") WriteString("Defense", "RightWallName", userCastle.m_RightWall.sName);
            if (userCastle.m_RightWall.BaseObject != null)
                WriteInteger("Defense", "RightWallHP", userCastle.m_RightWall.BaseObject.m_WAbil.HP);
            TObjUnit objUnit;
            for (var i = 0; i < userCastle.Archer.Length; i++)
            {
                objUnit = userCastle.Archer[i];
                if (objUnit.nX != 0) WriteInteger("Defense", "Archer_" + (i + 1) + "_X", objUnit.nX);
                if (objUnit.nY != 0) WriteInteger("Defense", "Archer_" + (i + 1) + "_Y", objUnit.nY);
                if (objUnit.sName != "")
                    WriteString("Defense", "Archer_" + (i + 1) + "_Name", objUnit.sName);
                if (objUnit.BaseObject != null)
                    WriteInteger("Defense", "Archer_" + (i + 1) + "_HP", objUnit.BaseObject.m_WAbil.HP);
                else
                    WriteInteger("Defense", "Archer_" + (i + 1) + "_HP", 0);
            }

            for (var i = 0; i < userCastle.m_Guard.Length; i++)
            {
                objUnit = userCastle.m_Guard[i];
                if (objUnit.nX != 0) WriteInteger("Defense", "Guard_" + (i + 1) + "_X", objUnit.nX);
                if (objUnit.nY != 0) WriteInteger("Defense", "Guard_" + (i + 1) + "_Y", objUnit.nY);
                if (objUnit.sName != "") WriteString("Defense", "Guard_" + (i + 1) + "_Name", objUnit.sName);
                if (objUnit.BaseObject != null)
                    WriteInteger("Defense", "Guard_" + (i + 1) + "_HP", objUnit.BaseObject.m_WAbil.HP);
                else
                    WriteInteger("Defense", "Guard_" + (i + 1) + "_HP", 0);
            }
        }
    }
}