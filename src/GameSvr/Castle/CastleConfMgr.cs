using SystemModule;
using SystemModule.Common;

namespace GameSvr.Castle
{
    public class CastleConfMgr : IniFile
    {
        public CastleConfMgr(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig(UserCastle userCastle)
        {
            userCastle.sName = ReadString("Setup", "CastleName", userCastle.sName);
            userCastle.OwnGuild = ReadString("Setup", "OwnGuild", "");
            userCastle.ChangeDate = ReadDateTime("Setup", "ChangeDate", DateTime.Now);
            userCastle.m_WarDate = ReadDateTime("Setup", "WarDate", DateTime.Now);
            userCastle.IncomeToday = ReadDateTime("Setup", "IncomeToday", DateTime.Now);
            userCastle.TotalGold = ReadInteger("Setup", "TotalGold", 0);
            userCastle.TodayIncome = ReadInteger("Setup", "TodayIncome", 0);
            var sMapList = ReadString("Defense", "CastleMapList", "");
            var sMap = string.Empty;
            if (!string.IsNullOrEmpty(sMapList))
            {
                while (!string.IsNullOrEmpty(sMapList))
                {
                    sMapList = HUtil32.GetValidStr3(sMapList, ref sMap, new[] { ',' });
                    if (string.IsNullOrEmpty(sMap)) break;
                    userCastle.EnvirList.Add(sMap);
                }
            }
            userCastle.MapName = ReadString("Defense", "CastleMap", "3");
            userCastle.HomeMap = ReadString("Defense", "CastleHomeMap", userCastle.HomeMap);
            userCastle.HomeX = ReadInteger("Defense", "CastleHomeX", userCastle.HomeX);
            userCastle.HomeY = ReadInteger("Defense", "CastleHomeY", userCastle.HomeY);
            userCastle.WarRangeX = ReadInteger("Defense", "CastleWarRangeX", userCastle.WarRangeX);
            userCastle.WarRangeY = ReadInteger("Defense", "CastleWarRangeY", userCastle.WarRangeY);
            userCastle.PalaceMap = ReadString("Defense", "CastlePlaceMap", userCastle.PalaceMap);
            userCastle.SecretMap = ReadString("Defense", "CastleSecretMap", userCastle.SecretMap);
            userCastle.PalaceDoorX = ReadInteger("Defense", "CastlePalaceDoorX", 631);
            userCastle.PalaceDoorY = ReadInteger("Defense", "CastlePalaceDoorY", 274);
            userCastle.MainDoor = new ArcherUnit();
            userCastle.MainDoor.nX = Read<short>("Defense", "MainDoorX", (short)672);
            userCastle.MainDoor.nY = Read<short>("Defense", "MainDoorY", (short)330);
            userCastle.MainDoor.sName = ReadString("Defense", "MainDoorName", "MainDoor");
            userCastle.MainDoor.nStatus = ReadBool("Defense", "MainDoorOpen", true);
            userCastle.MainDoor.nHP = Read<ushort>("Defense", "MainDoorHP", (short)2000);
            userCastle.MainDoor.BaseObject = null;
            userCastle.LeftWall = new ArcherUnit();
            userCastle.LeftWall.nX = Read<short>("Defense", "LeftWallX", (short)624);
            userCastle.LeftWall.nY = Read<short>("Defense", "LeftWallY", (short)278);
            userCastle.LeftWall.sName = ReadString("Defense", "LeftWallName", "LeftWall");
            userCastle.LeftWall.nHP = Read<ushort>("Defense", "LeftWallHP", (short)2000);
            userCastle.LeftWall.BaseObject = null;
            userCastle.CenterWall = new ArcherUnit();
            userCastle.CenterWall.nX = Read<short>("Defense", "CenterWallX", (short)627);
            userCastle.CenterWall.nY = Read<short>("Defense", "CenterWallY", (short)278);
            userCastle.CenterWall.sName = ReadString("Defense", "CenterWallName", "CenterWall");
            userCastle.CenterWall.nHP = Read<ushort>("Defense", "CenterWallHP", (short)2000);
            userCastle.CenterWall.BaseObject = null;
            userCastle.RightWall = new ArcherUnit();
            userCastle.RightWall.nX = Read<short>("Defense", "RightWallX", (short)634);
            userCastle.RightWall.nY = Read<short>("Defense", "RightWallY", (short)271);
            userCastle.RightWall.sName = ReadString("Defense", "RightWallName", "RightWall");
            userCastle.RightWall.nHP = Read<ushort>("Defense", "RightWallHP", (short)2000);
            userCastle.RightWall.BaseObject = null;
            for (var i = 0; i < userCastle.Archers.Length; i++)
            {
                var objUnit = new ArcherUnit();
                objUnit.nX = Read<short>("Defense", $"Archer_{i + 1}_X", (short)0);
                objUnit.nY = Read<short>("Defense", $"Archer_{i + 1}_Y", (short)0);
                objUnit.sName = ReadString("Defense", $"Archer_{i + 1}_Name", "弓箭手");
                objUnit.nHP = Read<ushort>("Defense", $"Archer_{i + 1}_HP", (short)2000);
                objUnit.BaseObject = null;
                userCastle.Archers[i] = objUnit;
            }
            for (var i = 0; i < userCastle.Guards.Length; i++)
            {
                var objUnit = new ArcherUnit();
                objUnit.nX = Read<short>("Defense", $"Guard_{i + 1}_X", (short)0);
                objUnit.nY = Read<short>("Defense", $"Guard_{i + 1}_Y", (short)0);
                objUnit.sName = ReadString("Defense", $"Guard_{i + 1}_Name", "守卫");
                objUnit.nHP = Read<ushort>("Defense", $"Guard_{i + 1}_HP", (short)2000);
                objUnit.BaseObject = null;
                userCastle.Guards[i] = objUnit;
            }
        }

        public void SaveConfig(UserCastle userCastle)
        {
            var filePath = Path.Combine(M2Share.BasePath, M2Share.Config.CastleDir, userCastle.ConfigDir);
            var sMapList = string.Empty;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            if (M2Share.MapMgr.GetMapOfServerIndex(userCastle.MapName) != M2Share.ServerIndex) return;
            if (!string.IsNullOrEmpty(userCastle.sName))
            {
                WriteString("Setup", "CastleName", userCastle.sName);
            }
            if (!string.IsNullOrEmpty(userCastle.OwnGuild))
            {
                WriteString("Setup", "OwnGuild", userCastle.OwnGuild);
            }
            WriteDateTime("Setup", "ChangeDate", userCastle.ChangeDate);
            WriteDateTime("Setup", "WarDate", userCastle.m_WarDate);
            WriteDateTime("Setup", "IncomeToday", userCastle.IncomeToday);
            if (userCastle.TotalGold != 0)
                WriteInteger("Setup", "TotalGold", userCastle.TotalGold);
            if (userCastle.TodayIncome != 0)
                WriteInteger("Setup", "TodayIncome", userCastle.TodayIncome);
            for (var i = 0; i < userCastle.EnvirList.Count; i++)
            {
                sMapList = sMapList + userCastle.EnvirList[i] + ',';
            }
            if (!string.IsNullOrEmpty(sMapList))
                WriteString("Defense", "CastleMapList", sMapList);
            if (userCastle.MapName != "")
                WriteString("Defense", "CastleMap", userCastle.MapName);
            if (userCastle.HomeMap != "")
                WriteString("Defense", "CastleHomeMap", userCastle.HomeMap);
            if (userCastle.HomeX != 0)
                WriteInteger("Defense", "CastleHomeX", userCastle.HomeX);
            if (userCastle.HomeY != 0)
                WriteInteger("Defense", "CastleHomeY", userCastle.HomeY);
            if (userCastle.WarRangeX != 0)
                WriteInteger("Defense", "CastleWarRangeX", userCastle.WarRangeX);
            if (userCastle.WarRangeY != 0)
                WriteInteger("Defense", "CastleWarRangeY", userCastle.WarRangeY);
            if (userCastle.PalaceMap != "")
                WriteString("Defense", "CastlePlaceMap", userCastle.PalaceMap);
            if (userCastle.SecretMap != "")
                WriteString("Defense", "CastleSecretMap", userCastle.SecretMap);
            if (userCastle.PalaceDoorX != 0)
                WriteInteger("Defense", "CastlePalaceDoorX", userCastle.PalaceDoorX);
            if (userCastle.PalaceDoorY != 0)
                WriteInteger("Defense", "CastlePalaceDoorY", userCastle.PalaceDoorY);
            if (userCastle.MainDoor.nX != 0)
                WriteInteger("Defense", "MainDoorX", userCastle.MainDoor.nX);
            if (userCastle.MainDoor.nY != 0)
                WriteInteger("Defense", "MainDoorY", userCastle.MainDoor.nY);
            if (userCastle.MainDoor.sName != "")
                WriteString("Defense", "MainDoorName", userCastle.MainDoor.sName);
            if (userCastle.MainDoor.BaseObject != null)
            {
                WriteBool("Defense", "MainDoorOpen", userCastle.MainDoor.nStatus);
                WriteInteger("Defense", "MainDoorHP", userCastle.MainDoor.BaseObject.WAbil.HP);
            }
            if (userCastle.LeftWall.nX != 0)
            {
                WriteInteger("Defense", "LeftWallX", userCastle.LeftWall.nX);
            }
            if (userCastle.LeftWall.nY != 0)
            {
                WriteInteger("Defense", "LeftWallY", userCastle.LeftWall.nY);
            }
            if (userCastle.LeftWall.sName != "")
            {
                WriteString("Defense", "LeftWallName", userCastle.LeftWall.sName);
            }
            if (userCastle.LeftWall.BaseObject != null)
            {
                WriteInteger("Defense", "LeftWallHP", userCastle.LeftWall.BaseObject.WAbil.HP);
            }
            if (userCastle.CenterWall.nX != 0)
            {
                WriteInteger("Defense", "CenterWallX", userCastle.CenterWall.nX);
            }
            if (userCastle.CenterWall.nY != 0)
            {
                WriteInteger("Defense", "CenterWallY", userCastle.CenterWall.nY);
            }
            if (userCastle.CenterWall.sName != "")
            {
                WriteString("Defense", "CenterWallName", userCastle.CenterWall.sName);
            }
            if (userCastle.CenterWall.BaseObject != null)
            {
                WriteInteger("Defense", "CenterWallHP", userCastle.CenterWall.BaseObject.WAbil.HP);
            }
            if (userCastle.RightWall.nX != 0)
            {
                WriteInteger("Defense", "RightWallX", userCastle.RightWall.nX);
            }
            if (userCastle.RightWall.nY != 0)
            {
                WriteInteger("Defense", "RightWallY", userCastle.RightWall.nY);
            }
            if (!string.IsNullOrEmpty(userCastle.RightWall.sName))
            {
                WriteString("Defense", "RightWallName", userCastle.RightWall.sName);
            }
            if (userCastle.RightWall.BaseObject != null)
            {
                WriteInteger("Defense", "RightWallHP", userCastle.RightWall.BaseObject.WAbil.HP);
            }
            for (var i = 0; i < userCastle.Archers.Length; i++)
            {
                var objUnit = userCastle.Archers[i];
                if (objUnit.nX != 0) WriteInteger("Defense", $"Archer_{i + 1}_X", objUnit.nX);
                if (objUnit.nY != 0) WriteInteger("Defense", $"Archer_{i + 1}_Y", objUnit.nY);
                if (!string.IsNullOrEmpty(objUnit.sName))
                {
                    WriteString("Defense", $"Archer_{i + 1}_Name", objUnit.sName);
                }
                if (objUnit.BaseObject != null)
                {
                    WriteInteger("Defense", $"Archer_{i + 1}_HP", objUnit.BaseObject.WAbil.HP);
                }
                else
                {
                    WriteInteger("Defense", $"Archer_{i + 1}_HP", 0);
                }
            }
            for (var i = 0; i < userCastle.Guards.Length; i++)
            {
                var objUnit = userCastle.Guards[i];
                if (objUnit.nX != 0) WriteInteger("Defense", $"Guard_{i + 1}_X", objUnit.nX);
                if (objUnit.nY != 0) WriteInteger("Defense", $"Guard_{i + 1}_Y", objUnit.nY);
                if (objUnit.sName != "") WriteString("Defense", $"Guard_{i + 1}_Name", objUnit.sName);
                if (objUnit.BaseObject != null)
                {
                    WriteInteger("Defense", $"Guard_{i + 1}_HP", objUnit.BaseObject.WAbil.HP);
                }
                else
                {
                    WriteInteger("Defense", $"Guard_{i + 1}_HP", 0);
                }
            }
            Save();
        }
    }
}