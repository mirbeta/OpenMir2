using SystemModule.Common;

namespace GameSvr.Castle
{
    public class CastleConfMgr : ConfigFile
    {
        public CastleConfMgr(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig(UserCastle userCastle)
        {
            userCastle.sName = ReadWriteString("Setup", "CastleName", userCastle.sName);
            userCastle.OwnGuild = ReadWriteString("Setup", "OwnGuild", "");
            userCastle.ChangeDate = ReadWriteDate("Setup", "ChangeDate", DateTime.Now);
            userCastle.WarDate = ReadWriteDate("Setup", "WarDate", DateTime.Now);
            userCastle.IncomeToday = ReadWriteDate("Setup", "IncomeToday", DateTime.Now);
            userCastle.TotalGold = ReadWriteInteger("Setup", "TotalGold", 0);
            userCastle.TodayIncome = ReadWriteInteger("Setup", "TodayIncome", 0);
            string sMapList = ReadWriteString("Defense", "CastleMapList", "");
            string sMap = string.Empty;
            if (!string.IsNullOrEmpty(sMapList))
            {
                while (!string.IsNullOrEmpty(sMapList))
                {
                    sMapList = HUtil32.GetValidStr3(sMapList, ref sMap, ',');
                    if (string.IsNullOrEmpty(sMap)) break;
                    userCastle.EnvirList.Add(sMap);
                }
            }
            userCastle.MapName = ReadWriteString("Defense", "CastleMap", "3");
            userCastle.HomeMap = ReadWriteString("Defense", "CastleHomeMap", userCastle.HomeMap);
            userCastle.HomeX = ReadWriteInteger("Defense", "CastleHomeX", userCastle.HomeX);
            userCastle.HomeY = ReadWriteInteger("Defense", "CastleHomeY", userCastle.HomeY);
            userCastle.WarRangeX = ReadWriteInteger("Defense", "CastleWarRangeX", userCastle.WarRangeX);
            userCastle.WarRangeY = ReadWriteInteger("Defense", "CastleWarRangeY", userCastle.WarRangeY);
            userCastle.PalaceMap = ReadWriteString("Defense", "CastlePlaceMap", userCastle.PalaceMap);
            userCastle.SecretMap = ReadWriteString("Defense", "CastleSecretMap", userCastle.SecretMap);
            userCastle.PalaceDoorX = ReadWriteInteger("Defense", "CastlePalaceDoorX", 631);
            userCastle.PalaceDoorY = ReadWriteInteger("Defense", "CastlePalaceDoorY", 274);
            userCastle.MainDoor = new ArcherUnit();
            userCastle.MainDoor.nX = ReadWriteInt16("Defense", "MainDoorX", 672);
            userCastle.MainDoor.nY = ReadWriteInt16("Defense", "MainDoorY", 330);
            userCastle.MainDoor.sName = ReadWriteString("Defense", "MainDoorName", "MainDoor");
            userCastle.MainDoor.nStatus = ReadWriteBool("Defense", "MainDoorOpen", true);
            userCastle.MainDoor.nHP = ReadWriteUInt16("Defense", "MainDoorHP", 2000);
            userCastle.MainDoor.BaseObject = null;
            userCastle.LeftWall = new ArcherUnit();
            userCastle.LeftWall.nX = ReadWriteInt16("Defense", "LeftWallX", 624);
            userCastle.LeftWall.nY = ReadWriteInt16("Defense", "LeftWallY", 278);
            userCastle.LeftWall.sName = ReadWriteString("Defense", "LeftWallName", "LeftWall");
            userCastle.LeftWall.nHP = ReadWriteUInt16("Defense", "LeftWallHP", 2000);
            userCastle.LeftWall.BaseObject = null;
            userCastle.CenterWall = new ArcherUnit();
            userCastle.CenterWall.nX = ReadWriteInt16("Defense", "CenterWallX", 627);
            userCastle.CenterWall.nY = ReadWriteInt16("Defense", "CenterWallY", 278);
            userCastle.CenterWall.sName = ReadWriteString("Defense", "CenterWallName", "CenterWall");
            userCastle.CenterWall.nHP = ReadWriteUInt16("Defense", "CenterWallHP", 2000);
            userCastle.CenterWall.BaseObject = null;
            userCastle.RightWall = new ArcherUnit();
            userCastle.RightWall.nX = ReadWriteInt16("Defense", "RightWallX", 634);
            userCastle.RightWall.nY = ReadWriteInt16("Defense", "RightWallY", 271);
            userCastle.RightWall.sName = ReadWriteString("Defense", "RightWallName", "RightWall");
            userCastle.RightWall.nHP = ReadWriteUInt16("Defense", "RightWallHP", 2000);
            userCastle.RightWall.BaseObject = null;
            for (int i = 0; i < userCastle.Archers.Length; i++)
            {
                ArcherUnit objUnit = new ArcherUnit();
                objUnit.nX = ReadWriteInt16("Defense", $"Archer_{i + 1}_X", 0);
                objUnit.nY = ReadWriteInt16("Defense", $"Archer_{i + 1}_Y", 0);
                objUnit.sName = ReadWriteString("Defense", $"Archer_{i + 1}_Name", "弓箭手");
                objUnit.nHP = ReadWriteUInt16("Defense", $"Archer_{i + 1}_HP", 2000);
                objUnit.BaseObject = null;
                userCastle.Archers[i] = objUnit;
            }
            for (int i = 0; i < userCastle.Guards.Length; i++)
            {
                ArcherUnit objUnit = new ArcherUnit();
                objUnit.nX = ReadWriteInt16("Defense", $"Guard_{i + 1}_X", 0);
                objUnit.nY = ReadWriteInt16("Defense", $"Guard_{i + 1}_Y", 0);
                objUnit.sName = ReadWriteString("Defense", $"Guard_{i + 1}_Name", "守卫");
                objUnit.nHP = ReadWriteUInt16("Defense", $"Guard_{i + 1}_HP", 2000);
                objUnit.BaseObject = null;
                userCastle.Guards[i] = objUnit;
            }
        }

        public void SaveConfig(UserCastle userCastle)
        {
            string filePath = Path.Combine(M2Share.BasePath, M2Share.Config.CastleDir, userCastle.ConfigDir);
            string sMapList = string.Empty;
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
            WriteDateTime("Setup", "WarDate", userCastle.WarDate);
            WriteDateTime("Setup", "IncomeToday", userCastle.IncomeToday);
            if (userCastle.TotalGold != 0)
                WriteInteger("Setup", "TotalGold", userCastle.TotalGold);
            if (userCastle.TodayIncome != 0)
                WriteInteger("Setup", "TodayIncome", userCastle.TodayIncome);
            for (int i = 0; i < userCastle.EnvirList.Count; i++)
            {
                sMapList = sMapList + userCastle.EnvirList[i] + ',';
            }
            if (!string.IsNullOrEmpty(sMapList))
                WriteString("Defense", "CastleMapList", sMapList);
            if (!string.IsNullOrEmpty(userCastle.MapName))
                WriteString("Defense", "CastleMap", userCastle.MapName);
            if (!string.IsNullOrEmpty(userCastle.HomeMap))
                WriteString("Defense", "CastleHomeMap", userCastle.HomeMap);
            if (userCastle.HomeX != 0)
                WriteInteger("Defense", "CastleHomeX", userCastle.HomeX);
            if (userCastle.HomeY != 0)
                WriteInteger("Defense", "CastleHomeY", userCastle.HomeY);
            if (userCastle.WarRangeX != 0)
                WriteInteger("Defense", "CastleWarRangeX", userCastle.WarRangeX);
            if (userCastle.WarRangeY != 0)
                WriteInteger("Defense", "CastleWarRangeY", userCastle.WarRangeY);
            if (!string.IsNullOrEmpty(userCastle.PalaceMap))
                WriteString("Defense", "CastlePlaceMap", userCastle.PalaceMap);
            if (!string.IsNullOrEmpty(userCastle.SecretMap))
                WriteString("Defense", "CastleSecretMap", userCastle.SecretMap);
            if (userCastle.PalaceDoorX != 0)
                WriteInteger("Defense", "CastlePalaceDoorX", userCastle.PalaceDoorX);
            if (userCastle.PalaceDoorY != 0)
                WriteInteger("Defense", "CastlePalaceDoorY", userCastle.PalaceDoorY);
            if (userCastle.MainDoor.nX != 0)
                WriteInteger("Defense", "MainDoorX", userCastle.MainDoor.nX);
            if (userCastle.MainDoor.nY != 0)
                WriteInteger("Defense", "MainDoorY", userCastle.MainDoor.nY);
            if (!string.IsNullOrEmpty(userCastle.MainDoor.sName))
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
            if (!string.IsNullOrEmpty(userCastle.LeftWall.sName))
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
            if (!string.IsNullOrEmpty(userCastle.CenterWall.sName))
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
            for (int i = 0; i < userCastle.Archers.Length; i++)
            {
                ArcherUnit objUnit = userCastle.Archers[i];
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
            for (int i = 0; i < userCastle.Guards.Length; i++)
            {
                ArcherUnit objUnit = userCastle.Guards[i];
                if (objUnit.nX != 0) WriteInteger("Defense", $"Guard_{i + 1}_X", objUnit.nX);
                if (objUnit.nY != 0) WriteInteger("Defense", $"Guard_{i + 1}_Y", objUnit.nY);
                if (!string.IsNullOrEmpty(objUnit.sName))
                {
                    WriteString("Defense", $"Guard_{i + 1}_Name", objUnit.sName);
                }
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