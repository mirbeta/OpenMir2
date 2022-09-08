using SystemModule.Common;

namespace GameSvr.Conf
{
    public class GameCmdConf : IniFile
    {
        public GameCmdConf(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            int nLoadInteger;
            string LoadString = ReadString("Command", "Date", "");
            if (LoadString == "")
            {
                WriteString("Command", "Date", M2Share.g_GameCommand.DATA.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DATA.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Date", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Date", M2Share.g_GameCommand.DATA.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DATA.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "PrvMsg", "");
            if (LoadString == "")
            {
                WriteString("Command", "PrvMsg", M2Share.g_GameCommand.PRVMSG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.PRVMSG.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "PrvMsg", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "PrvMsg", M2Share.g_GameCommand.PRVMSG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.PRVMSG.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AllowMsg", "");
            if (LoadString == "")
            {
                WriteString("Command", "AllowMsg", M2Share.g_GameCommand.ALLOWMSG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWMSG.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AllowMsg", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AllowMsg", M2Share.g_GameCommand.ALLOWMSG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWMSG.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "LetShout", "");
            if (LoadString == "")
            {
                WriteString("Command", "LetShout", M2Share.g_GameCommand.LETSHOUT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LETSHOUT.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "LetTrade", "");
            if (LoadString == "")
            {
                WriteString("Command", "LetTrade", M2Share.g_GameCommand.LETTRADE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LETTRADE.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "LetGuild", "");
            if (LoadString == "")
            {
                WriteString("Command", "LetGuild", M2Share.g_GameCommand.LETGUILD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LETGUILD.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "EndGuild", "");
            if (LoadString == "")
            {
                WriteString("Command", "EndGuild", M2Share.g_GameCommand.ENDGUILD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ENDGUILD.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "BanGuildChat", "");
            if (LoadString == "")
            {
                WriteString("Command", "BanGuildChat", M2Share.g_GameCommand.BANGUILDCHAT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.BANGUILDCHAT.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "AuthAlly", "");
            if (LoadString == "")
            {
                WriteString("Command", "AuthAlly", M2Share.g_GameCommand.AUTHALLY.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.AUTHALLY.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "Auth", "");
            if (LoadString == "")
            {
                WriteString("Command", "Auth", M2Share.g_GameCommand.AUTH.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.AUTH.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "AuthCancel", "");
            if (LoadString == "")
            {
                WriteString("Command", "AuthCancel", M2Share.g_GameCommand.AUTHCANCEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.AUTHCANCEL.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "ViewDiary", "");
            if (LoadString == "")
            {
                WriteString("Command", "ViewDiary", M2Share.g_GameCommand.DIARY.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DIARY.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "UserMove", "");
            if (LoadString == "")
            {
                WriteString("Command", "UserMove", M2Share.g_GameCommand.USERMOVE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.USERMOVE.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "Searching", "");
            if (LoadString == "")
            {
                WriteString("Command", "Searching", M2Share.g_GameCommand.SEARCHING.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SEARCHING.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "AllowGroupCall", "");
            if (LoadString == "")
            {
                WriteString("Command", "AllowGroupCall", M2Share.g_GameCommand.ALLOWGROUPCALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWGROUPCALL.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "GroupCall", "");
            if (LoadString == "")
            {
                WriteString("Command", "GroupCall", M2Share.g_GameCommand.GROUPRECALLL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GROUPRECALLL.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "AllowGuildReCall", "");
            if (LoadString == "")
            {
                WriteString("Command", "AllowGuildReCall", M2Share.g_GameCommand.ALLOWGUILDRECALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWGUILDRECALL.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "GuildReCall", "");
            if (LoadString == "")
            {
                WriteString("Command", "GuildReCall", M2Share.g_GameCommand.GUILDRECALLL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GUILDRECALLL.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "StorageUnLock", "");
            if (LoadString == "")
            {
                WriteString("Command", "StorageUnLock", M2Share.g_GameCommand.UNLOCKSTORAGE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.UNLOCKSTORAGE.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "PasswordUnLock", "");
            if (LoadString == "")
            {
                WriteString("Command", "PasswordUnLock", M2Share.g_GameCommand.UNLOCK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.UNLOCK.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "StorageLock", "");
            if (LoadString == "")
            {
                WriteString("Command", "StorageLock", M2Share.g_GameCommand.__LOCK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.__LOCK.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "StorageSetPassword", "");
            if (LoadString == "")
            {
                WriteString("Command", "StorageSetPassword", M2Share.g_GameCommand.SETPASSWORD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETPASSWORD.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "PasswordLock", "");
            if (LoadString == "")
            {
                WriteString("Command", "PasswordLock", M2Share.g_GameCommand.PASSWORDLOCK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.PASSWORDLOCK.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "StorageChgPassword", "");
            if (LoadString == "")
            {
                WriteString("Command", "StorageChgPassword", M2Share.g_GameCommand.CHGPASSWORD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHGPASSWORD.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "StorageClearPassword", "");
            if (LoadString == "")
            {
                WriteString("Command", "StorageClearPassword", M2Share.g_GameCommand.CLRPASSWORD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CLRPASSWORD.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "StorageClearPassword", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "StorageClearPassword", M2Share.g_GameCommand.CLRPASSWORD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CLRPASSWORD.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "StorageUserClearPassword", "");
            if (LoadString == "")
            {
                WriteString("Command", "StorageUserClearPassword", M2Share.g_GameCommand.UNPASSWORD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.UNPASSWORD.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "MemberFunc", "");
            if (LoadString == "")
            {
                WriteString("Command", "MemberFunc", M2Share.g_GameCommand.MEMBERFUNCTION.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MEMBERFUNCTION.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "MemberFuncEx", "");
            if (LoadString == "")
            {
                WriteString("Command", "MemberFuncEx", M2Share.g_GameCommand.MEMBERFUNCTIONEX.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MEMBERFUNCTIONEX.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "Dear", "");
            if (LoadString == "")
            {
                WriteString("Command", "Dear", M2Share.g_GameCommand.DEAR.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DEAR.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "Master", "");
            if (LoadString == "")
            {
                WriteString("Command", "Master", M2Share.g_GameCommand.MASTER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MASTER.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "DearRecall", "");
            if (LoadString == "")
            {
                WriteString("Command", "DearRecall", M2Share.g_GameCommand.DEARRECALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DEARRECALL.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "MasterRecall", "");
            if (LoadString == "")
            {
                WriteString("Command", "MasterRecall", M2Share.g_GameCommand.MASTERECALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MASTERECALL.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "AllowDearRecall", "");
            if (LoadString == "")
            {
                WriteString("Command", "AllowDearRecall", M2Share.g_GameCommand.ALLOWDEARRCALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWDEARRCALL.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "AllowMasterRecall", "");
            if (LoadString == "")
            {
                WriteString("Command", "AllowMasterRecall", M2Share.g_GameCommand.ALLOWMASTERRECALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWMASTERRECALL.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "AttackMode", "");
            if (LoadString == "")
            {
                WriteString("Command", "AttackMode", M2Share.g_GameCommand.ATTACKMODE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ATTACKMODE.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "Rest", "");
            if (LoadString == "")
            {
                WriteString("Command", "Rest", M2Share.g_GameCommand.REST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.REST.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "TakeOnHorse", "");
            if (LoadString == "")
            {
                WriteString("Command", "TakeOnHorse", M2Share.g_GameCommand.TAKEONHORSE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TAKEONHORSE.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "TakeOffHorse", "");
            if (LoadString == "")
            {
                WriteString("Command", "TakeOffHorse", M2Share.g_GameCommand.TAKEOFHORSE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TAKEOFHORSE.sCmd = LoadString;
            }
            LoadString = ReadString("Command", "HumanLocal", "");
            if (LoadString == "")
            {
                WriteString("Command", "HumanLocal", M2Share.g_GameCommand.HUMANLOCAL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.HUMANLOCAL.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "HumanLocal", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "HumanLocal", M2Share.g_GameCommand.HUMANLOCAL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.HUMANLOCAL.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Move", "");
            if (LoadString == "")
            {
                WriteString("Command", "Move", M2Share.g_GameCommand.MOVE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOVE.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "MoveMin", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MoveMin", M2Share.g_GameCommand.MOVE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOVE.nPerMissionMin = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Permission", "MoveMax", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MoveMax", M2Share.g_GameCommand.MOVE.nPerMissionMax);
            }
            else
            {
                M2Share.g_GameCommand.MOVE.nPerMissionMax = nLoadInteger;
            }
            LoadString = ReadString("Command", "PositionMove", "");
            if (LoadString == "")
            {
                WriteString("Command", "PositionMove", M2Share.g_GameCommand.POSITIONMOVE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.POSITIONMOVE.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "PositionMoveMin", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "PositionMoveMin", M2Share.g_GameCommand.POSITIONMOVE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.POSITIONMOVE.nPerMissionMin = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Permission", "PositionMoveMax", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "PositionMoveMax", M2Share.g_GameCommand.POSITIONMOVE.nPerMissionMax);
            }
            else
            {
                M2Share.g_GameCommand.POSITIONMOVE.nPerMissionMax = nLoadInteger;
            }
            LoadString = ReadString("Command", "Info", "");
            if (LoadString == "")
            {
                WriteString("Command", "Info", M2Share.g_GameCommand.INFO.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.INFO.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Info", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Info", M2Share.g_GameCommand.INFO.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.INFO.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "MobLevel", "");
            if (LoadString == "")
            {
                WriteString("Command", "MobLevel", M2Share.g_GameCommand.MOBLEVEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOBLEVEL.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "MobLevel", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MobLevel", M2Share.g_GameCommand.MOBLEVEL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOBLEVEL.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "MobCount", "");
            if (LoadString == "")
            {
                WriteString("Command", "MobCount", M2Share.g_GameCommand.MOBCOUNT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOBCOUNT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "MobCount", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MobCount", M2Share.g_GameCommand.MOBCOUNT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOBCOUNT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "HumanCount", "");
            if (LoadString == "")
            {
                WriteString("Command", "HumanCount", M2Share.g_GameCommand.HUMANCOUNT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.HUMANCOUNT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "HumanCount", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "HumanCount", M2Share.g_GameCommand.HUMANCOUNT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.HUMANCOUNT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Map", "");
            if (LoadString == "")
            {
                WriteString("Command", "Map", M2Share.g_GameCommand.MAP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MAP.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Map", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Map", M2Share.g_GameCommand.MAP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MAP.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Kick", "");
            if (LoadString == "")
            {
                WriteString("Command", "Kick", M2Share.g_GameCommand.KICK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.KICK.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Kick", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Kick", M2Share.g_GameCommand.KICK.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.KICK.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Ting", "");
            if (LoadString == "")
            {
                WriteString("Command", "Ting", M2Share.g_GameCommand.TING.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TING.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Ting", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Ting", M2Share.g_GameCommand.TING.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TING.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SuperTing", "");
            if (LoadString == "")
            {
                WriteString("Command", "SuperTing", M2Share.g_GameCommand.SUPERTING.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SUPERTING.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SuperTing", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SuperTing", M2Share.g_GameCommand.SUPERTING.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SUPERTING.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "MapMove", "");
            if (LoadString == "")
            {
                WriteString("Command", "MapMove", M2Share.g_GameCommand.MAPMOVE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MAPMOVE.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "MapMove", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MapMove", M2Share.g_GameCommand.MAPMOVE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MAPMOVE.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Shutup", "");
            if (LoadString == "")
            {
                WriteString("Command", "Shutup", M2Share.g_GameCommand.SHUTUP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHUTUP.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Shutup", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Shutup", M2Share.g_GameCommand.SHUTUP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHUTUP.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReleaseShutup", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReleaseShutup", M2Share.g_GameCommand.RELEASESHUTUP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELEASESHUTUP.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReleaseShutup", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReleaseShutup", M2Share.g_GameCommand.RELEASESHUTUP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELEASESHUTUP.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ShutupList", "");
            if (LoadString == "")
            {
                WriteString("Command", "ShutupList", M2Share.g_GameCommand.SHUTUPLIST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHUTUPLIST.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ShutupList", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ShutupList", M2Share.g_GameCommand.SHUTUPLIST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHUTUPLIST.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "GameMaster", "");
            if (LoadString == "")
            {
                WriteString("Command", "GameMaster", M2Share.g_GameCommand.GAMEMASTER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GAMEMASTER.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "GameMaster", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "GameMaster", M2Share.g_GameCommand.GAMEMASTER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.GAMEMASTER.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ObServer", "");
            if (LoadString == "")
            {
                WriteString("Command", "ObServer", M2Share.g_GameCommand.OBSERVER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.OBSERVER.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ObServer", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ObServer", M2Share.g_GameCommand.OBSERVER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.OBSERVER.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SuperMan", "");
            if (LoadString == "")
            {
                WriteString("Command", "SuperMan", M2Share.g_GameCommand.SUEPRMAN.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SUEPRMAN.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SuperMan", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SuperMan", M2Share.g_GameCommand.SUEPRMAN.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SUEPRMAN.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Level", "");
            if (LoadString == "")
            {
                WriteString("Command", "Level", M2Share.g_GameCommand.LEVEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LEVEL.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Level", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Level", M2Share.g_GameCommand.LEVEL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.LEVEL.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SabukWallGold", "");
            if (LoadString == "")
            {
                WriteString("Command", "SabukWallGold", M2Share.g_GameCommand.SABUKWALLGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SABUKWALLGOLD.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SabukWallGold", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SabukWallGold", M2Share.g_GameCommand.SABUKWALLGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SABUKWALLGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Recall", "");
            if (LoadString == "")
            {
                WriteString("Command", "Recall", M2Share.g_GameCommand.RECALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RECALL.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Recall", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Recall", M2Share.g_GameCommand.RECALL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RECALL.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReGoto", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReGoto", M2Share.g_GameCommand.REGOTO.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.REGOTO.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReGoto", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReGoto", M2Share.g_GameCommand.REGOTO.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.REGOTO.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Flag", "");
            if (LoadString == "")
            {
                WriteString("Command", "Flag", M2Share.g_GameCommand.SHOWFLAG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWFLAG.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Flag", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Flag", M2Share.g_GameCommand.SHOWFLAG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWFLAG.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ShowOpen", "");
            if (LoadString == "")
            {
                WriteString("Command", "ShowOpen", M2Share.g_GameCommand.SHOWOPEN.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWOPEN.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ShowOpen", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ShowOpen", M2Share.g_GameCommand.SHOWOPEN.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWOPEN.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ShowUnit", "");
            if (LoadString == "")
            {
                WriteString("Command", "ShowUnit", M2Share.g_GameCommand.SHOWUNIT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWUNIT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ShowUnit", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ShowUnit", M2Share.g_GameCommand.SHOWUNIT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWUNIT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Attack", "");
            if (LoadString == "")
            {
                WriteString("Command", "Attack", M2Share.g_GameCommand.ATTACK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ATTACK.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Attack", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Attack", M2Share.g_GameCommand.ATTACK.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ATTACK.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Mob", "");
            if (LoadString == "")
            {
                WriteString("Command", "Mob", M2Share.g_GameCommand.MOB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOB.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Mob", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Mob", M2Share.g_GameCommand.MOB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOB.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "MobNpc", "");
            if (LoadString == "")
            {
                WriteString("Command", "MobNpc", M2Share.g_GameCommand.MOBNPC.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOBNPC.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "MobNpc", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MobNpc", M2Share.g_GameCommand.MOBNPC.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOBNPC.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelNpc", "");
            if (LoadString == "")
            {
                WriteString("Command", "DelNpc", M2Share.g_GameCommand.DELNPC.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELNPC.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelNpc", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelNpc", M2Share.g_GameCommand.DELNPC.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELNPC.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "NpcScript", "");
            if (LoadString == "")
            {
                WriteString("Command", "NpcScript", M2Share.g_GameCommand.NPCSCRIPT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.NPCSCRIPT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "NpcScript", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "NpcScript", M2Share.g_GameCommand.NPCSCRIPT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.NPCSCRIPT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "RecallMob", "");
            if (LoadString == "")
            {
                WriteString("Command", "RecallMob", M2Share.g_GameCommand.RECALLMOB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RECALLMOB.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "RecallMob", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "RecallMob", M2Share.g_GameCommand.RECALLMOB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RECALLMOB.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "LuckPoint", "");
            if (LoadString == "")
            {
                WriteString("Command", "LuckPoint", M2Share.g_GameCommand.LUCKYPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LUCKYPOINT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "LuckPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "LuckPoint", M2Share.g_GameCommand.LUCKYPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.LUCKYPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "LotteryTicket", "");
            if (LoadString == "")
            {
                WriteString("Command", "LotteryTicket", M2Share.g_GameCommand.LOTTERYTICKET.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LOTTERYTICKET.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "LotteryTicket", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "LotteryTicket", M2Share.g_GameCommand.LOTTERYTICKET.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.LOTTERYTICKET.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadGuild", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReloadGuild", M2Share.g_GameCommand.RELOADGUILD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADGUILD.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadGuild", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadGuild", M2Share.g_GameCommand.RELOADGUILD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADGUILD.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadLineNotice", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReloadLineNotice", M2Share.g_GameCommand.RELOADLINENOTICE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADLINENOTICE.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadLineNotice", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadLineNotice", M2Share.g_GameCommand.RELOADLINENOTICE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADLINENOTICE.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadAbuse", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReloadAbuse", M2Share.g_GameCommand.RELOADABUSE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADABUSE.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadAbuse", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadAbuse", M2Share.g_GameCommand.RELOADABUSE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADABUSE.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "BackStep", "");
            if (LoadString == "")
            {
                WriteString("Command", "BackStep", M2Share.g_GameCommand.BACKSTEP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.BACKSTEP.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "BackStep", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "BackStep", M2Share.g_GameCommand.BACKSTEP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.BACKSTEP.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Ball", "");
            if (LoadString == "")
            {
                WriteString("Command", "Ball", M2Share.g_GameCommand.BALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.BALL.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Ball", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Ball", M2Share.g_GameCommand.BALL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.BALL.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "FreePenalty", "");
            if (LoadString == "")
            {
                WriteString("Command", "FreePenalty", M2Share.g_GameCommand.FREEPENALTY.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.FREEPENALTY.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "FreePenalty", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "FreePenalty", M2Share.g_GameCommand.FREEPENALTY.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.FREEPENALTY.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "PkPoint", "");
            if (LoadString == "")
            {
                WriteString("Command", "PkPoint", M2Share.g_GameCommand.PKPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.PKPOINT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "PkPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "PkPoint", M2Share.g_GameCommand.PKPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.PKPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "IncPkPoint", "");
            if (LoadString == "")
            {
                WriteString("Command", "IncPkPoint", M2Share.g_GameCommand.INCPKPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.INCPKPOINT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "IncPkPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "IncPkPoint", M2Share.g_GameCommand.INCPKPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.INCPKPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeLuck", "");
            if (LoadString == "")
            {
                WriteString("Command", "ChangeLuck", M2Share.g_GameCommand.CHANGELUCK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGELUCK.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeLuck", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeLuck", M2Share.g_GameCommand.CHANGELUCK.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGELUCK.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Hunger", "");
            if (LoadString == "")
            {
                WriteString("Command", "Hunger", M2Share.g_GameCommand.HUNGER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.HUNGER.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Hunger", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Hunger", M2Share.g_GameCommand.HUNGER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.HUNGER.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Hair", "");
            if (LoadString == "")
            {
                WriteString("Command", "Hair", M2Share.g_GameCommand.HAIR.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.HAIR.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Hair", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Hair", M2Share.g_GameCommand.HAIR.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.HAIR.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Training", "");
            if (LoadString == "")
            {
                WriteString("Command", "Training", M2Share.g_GameCommand.TRAINING.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TRAINING.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Training", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Training", M2Share.g_GameCommand.TRAINING.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TRAINING.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DeleteSkill", "");
            if (LoadString == "")
            {
                WriteString("Command", "DeleteSkill", M2Share.g_GameCommand.DELETESKILL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELETESKILL.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DeleteSkill", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DeleteSkill", M2Share.g_GameCommand.DELETESKILL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELETESKILL.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeJob", "");
            if (LoadString == "")
            {
                WriteString("Command", "ChangeJob", M2Share.g_GameCommand.CHANGEJOB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEJOB.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeJob", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeJob", M2Share.g_GameCommand.CHANGEJOB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEJOB.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeGender", "");
            if (LoadString == "")
            {
                WriteString("Command", "ChangeGender", M2Share.g_GameCommand.CHANGEGENDER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEGENDER.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeGender", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeGender", M2Share.g_GameCommand.CHANGEGENDER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEGENDER.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "NameColor", "");
            if (LoadString == "")
            {
                WriteString("Command", "NameColor", M2Share.g_GameCommand.NAMECOLOR.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.NAMECOLOR.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "NameColor", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "NameColor", M2Share.g_GameCommand.NAMECOLOR.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.NAMECOLOR.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Mission", "");
            if (LoadString == "")
            {
                WriteString("Command", "Mission", M2Share.g_GameCommand.MISSION.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MISSION.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Mission", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Mission", M2Share.g_GameCommand.MISSION.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MISSION.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "MobPlace", "");
            if (LoadString == "")
            {
                WriteString("Command", "MobPlace", M2Share.g_GameCommand.MOBPLACE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOBPLACE.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "MobPlace", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MobPlace", M2Share.g_GameCommand.MOBPLACE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOBPLACE.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Transparecy", "");
            if (LoadString == "")
            {
                WriteString("Command", "Transparecy", M2Share.g_GameCommand.TRANSPARECY.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TRANSPARECY.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Transparecy", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Transparecy", M2Share.g_GameCommand.TRANSPARECY.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TRANSPARECY.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DeleteItem", "");
            if (LoadString == "")
            {
                WriteString("Command", "DeleteItem", M2Share.g_GameCommand.DELETEITEM.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELETEITEM.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DeleteItem", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DeleteItem", M2Share.g_GameCommand.DELETEITEM.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELETEITEM.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Level0", "");
            if (LoadString == "")
            {
                WriteString("Command", "Level0", M2Share.g_GameCommand.LEVEL0.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LEVEL0.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Level0", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Level0", M2Share.g_GameCommand.LEVEL0.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.LEVEL0.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ClearMission", "");
            if (LoadString == "")
            {
                WriteString("Command", "ClearMission", M2Share.g_GameCommand.CLEARMISSION.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CLEARMISSION.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ClearMission", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ClearMission", M2Share.g_GameCommand.CLEARMISSION.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CLEARMISSION.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SetFlag", "");
            if (LoadString == "")
            {
                WriteString("Command", "SetFlag", M2Share.g_GameCommand.SETFLAG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETFLAG.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SetFlag", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SetFlag", M2Share.g_GameCommand.SETFLAG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SETFLAG.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SetOpen", "");
            if (LoadString == "")
            {
                WriteString("Command", "SetOpen", M2Share.g_GameCommand.SETOPEN.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETOPEN.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SetOpen", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SetOpen", M2Share.g_GameCommand.SETOPEN.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SETOPEN.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SetUnit", "");
            if (LoadString == "")
            {
                WriteString("Command", "SetUnit", M2Share.g_GameCommand.SETUNIT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETUNIT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SetUnit", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SetUnit", M2Share.g_GameCommand.SETUNIT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SETUNIT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReConnection", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReConnection", M2Share.g_GameCommand.RECONNECTION.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RECONNECTION.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReConnection", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReConnection", M2Share.g_GameCommand.RECONNECTION.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RECONNECTION.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DisableFilter", "");
            if (LoadString == "")
            {
                WriteString("Command", "DisableFilter", M2Share.g_GameCommand.DISABLEFILTER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DISABLEFILTER.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DisableFilter", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DisableFilter", M2Share.g_GameCommand.DISABLEFILTER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DISABLEFILTER.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeUserFull", "");
            if (LoadString == "")
            {
                WriteString("Command", "ChangeUserFull", M2Share.g_GameCommand.CHGUSERFULL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHGUSERFULL.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeUserFull", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeUserFull", M2Share.g_GameCommand.CHGUSERFULL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHGUSERFULL.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeZenFastStep", "");
            if (LoadString == "")
            {
                WriteString("Command", "ChangeZenFastStep", M2Share.g_GameCommand.CHGZENFASTSTEP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHGZENFASTSTEP.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeZenFastStep", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeZenFastStep", M2Share.g_GameCommand.CHGZENFASTSTEP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHGZENFASTSTEP.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ContestPoint", "");
            if (LoadString == "")
            {
                WriteString("Command", "ContestPoint", M2Share.g_GameCommand.CONTESTPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CONTESTPOINT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ContestPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ContestPoint", M2Share.g_GameCommand.CONTESTPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CONTESTPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "StartContest", "");
            if (LoadString == "")
            {
                WriteString("Command", "StartContest", M2Share.g_GameCommand.STARTCONTEST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.STARTCONTEST.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "StartContest", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "StartContest", M2Share.g_GameCommand.STARTCONTEST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.STARTCONTEST.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "EndContest", "");
            if (LoadString == "")
            {
                WriteString("Command", "EndContest", M2Share.g_GameCommand.ENDCONTEST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ENDCONTEST.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "EndContest", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "EndContest", M2Share.g_GameCommand.ENDCONTEST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ENDCONTEST.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Announcement", "");
            if (LoadString == "")
            {
                WriteString("Command", "Announcement", M2Share.g_GameCommand.ANNOUNCEMENT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ANNOUNCEMENT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Announcement", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Announcement", M2Share.g_GameCommand.ANNOUNCEMENT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ANNOUNCEMENT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "OXQuizRoom", "");
            if (LoadString == "")
            {
                WriteString("Command", "OXQuizRoom", M2Share.g_GameCommand.OXQUIZROOM.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.OXQUIZROOM.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "OXQuizRoom", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "OXQuizRoom", M2Share.g_GameCommand.OXQUIZROOM.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.OXQUIZROOM.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Gsa", "");
            if (LoadString == "")
            {
                WriteString("Command", "Gsa", M2Share.g_GameCommand.GSA.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GSA.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Gsa", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Gsa", M2Share.g_GameCommand.GSA.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.GSA.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeItemName", "");
            if (LoadString == "")
            {
                WriteString("Command", "ChangeItemName", M2Share.g_GameCommand.CHANGEITEMNAME.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEITEMNAME.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeItemName", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeItemName", M2Share.g_GameCommand.CHANGEITEMNAME.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEITEMNAME.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DisableSendMsg", "");
            if (LoadString == "")
            {
                WriteString("Command", "DisableSendMsg", M2Share.g_GameCommand.DISABLESENDMSG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DISABLESENDMSG.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DisableSendMsg", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DisableSendMsg", M2Share.g_GameCommand.DISABLESENDMSG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DISABLESENDMSG.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "EnableSendMsg", "");
            if (LoadString == "")
            {
                WriteString("Command", "EnableSendMsg", M2Share.g_GameCommand.ENABLESENDMSG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ENABLESENDMSG.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "EnableSendMsg", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "EnableSendMsg", M2Share.g_GameCommand.ENABLESENDMSG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ENABLESENDMSG.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DisableSendMsgList", "");
            if (LoadString == "")
            {
                WriteString("Command", "DisableSendMsgList", M2Share.g_GameCommand.DISABLESENDMSGLIST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DISABLESENDMSGLIST.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DisableSendMsgList", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DisableSendMsgList", M2Share.g_GameCommand.DISABLESENDMSGLIST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DISABLESENDMSGLIST.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Kill", "");
            if (LoadString == "")
            {
                WriteString("Command", "Kill", M2Share.g_GameCommand.KILL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.KILL.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Kill", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Kill", M2Share.g_GameCommand.KILL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.KILL.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Make", "");
            if (LoadString == "")
            {
                WriteString("Command", "Make", M2Share.g_GameCommand.MAKE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MAKE.sCmd = LoadString;
                M2Share.CommandMgr.RegisterCommand("Make", LoadString);
            }
            nLoadInteger = ReadInteger("Permission", "MakeMin", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MakeMin", M2Share.g_GameCommand.MAKE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MAKE.nPerMissionMin = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Permission", "MakeMax", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MakeMax", M2Share.g_GameCommand.MAKE.nPerMissionMax);
            }
            else
            {
                M2Share.g_GameCommand.MAKE.nPerMissionMax = nLoadInteger;
            }
            LoadString = ReadString("Command", "SuperMake", "");
            if (LoadString == "")
            {
                WriteString("Command", "SuperMake", M2Share.g_GameCommand.SMAKE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SMAKE.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SuperMake", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SuperMake", M2Share.g_GameCommand.SMAKE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SMAKE.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "BonusPoint", "");
            if (LoadString == "")
            {
                WriteString("Command", "BonusPoint", M2Share.g_GameCommand.BONUSPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.BONUSPOINT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "BonusPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "BonusPoint", M2Share.g_GameCommand.BONUSPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.BONUSPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelBonuPoint", "");
            if (LoadString == "")
            {
                WriteString("Command", "DelBonuPoint", M2Share.g_GameCommand.DELBONUSPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELBONUSPOINT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelBonuPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelBonuPoint", M2Share.g_GameCommand.DELBONUSPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELBONUSPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "RestBonuPoint", "");
            if (LoadString == "")
            {
                WriteString("Command", "RestBonuPoint", M2Share.g_GameCommand.RESTBONUSPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RESTBONUSPOINT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "RestBonuPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "RestBonuPoint", M2Share.g_GameCommand.RESTBONUSPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RESTBONUSPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "FireBurn", "");
            if (LoadString == "")
            {
                WriteString("Command", "FireBurn", M2Share.g_GameCommand.FIREBURN.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.FIREBURN.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "FireBurn", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "FireBurn", M2Share.g_GameCommand.FIREBURN.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.FIREBURN.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "TestStatus", "");
            if (LoadString == "")
            {
                WriteString("Command", "TestStatus", M2Share.g_GameCommand.TESTSTATUS.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TESTSTATUS.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "TestStatus", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "TestStatus", M2Share.g_GameCommand.TESTSTATUS.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TESTSTATUS.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelGold", "");
            if (LoadString == "")
            {
                WriteString("Command", "DelGold", M2Share.g_GameCommand.DELGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELGOLD.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelGold", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelGold", M2Share.g_GameCommand.DELGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AddGold", "");
            if (LoadString == "")
            {
                WriteString("Command", "AddGold", M2Share.g_GameCommand.ADDGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADDGOLD.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AddGold", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AddGold", M2Share.g_GameCommand.ADDGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADDGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelGameGold", "");
            if (LoadString == "")
            {
                WriteString("Command", "DelGameGold", M2Share.g_GameCommand.DELGAMEGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELGAMEGOLD.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelGameGold", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelGameGold", M2Share.g_GameCommand.DELGAMEGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELGAMEGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AddGamePoint", "");
            if (LoadString == "")
            {
                WriteString("Command", "AddGamePoint", M2Share.g_GameCommand.ADDGAMEGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADDGAMEGOLD.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AddGameGold", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AddGameGold", M2Share.g_GameCommand.ADDGAMEGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADDGAMEGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "GameGold", "");
            if (LoadString == "")
            {
                WriteString("Command", "GameGold", M2Share.g_GameCommand.GAMEGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GAMEGOLD.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "GameGold", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "GameGold", M2Share.g_GameCommand.GAMEGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.GAMEGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "GamePoint", "");
            if (LoadString == "")
            {
                WriteString("Command", "GamePoint", M2Share.g_GameCommand.GAMEPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GAMEPOINT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "GamePoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "GamePoint", M2Share.g_GameCommand.GAMEPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.GAMEPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "CreditPoint", "");
            if (LoadString == "")
            {
                WriteString("Command", "CreditPoint", M2Share.g_GameCommand.CREDITPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CREDITPOINT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "CreditPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "CreditPoint", M2Share.g_GameCommand.CREDITPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CREDITPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "TestGoldChange", "");
            if (LoadString == "")
            {
                WriteString("Command", "TestGoldChange", M2Share.g_GameCommand.TESTGOLDCHANGE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TESTGOLDCHANGE.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "TestGoldChange", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "TestGoldChange", M2Share.g_GameCommand.TESTGOLDCHANGE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TESTGOLDCHANGE.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "RefineWeapon", "");
            if (LoadString == "")
            {
                WriteString("Command", "RefineWeapon", M2Share.g_GameCommand.REFINEWEAPON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.REFINEWEAPON.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "RefineWeapon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "RefineWeapon", M2Share.g_GameCommand.REFINEWEAPON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.REFINEWEAPON.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadAdmin", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReloadAdmin", M2Share.g_GameCommand.RELOADADMIN.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADADMIN.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadAdmin", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadAdmin", M2Share.g_GameCommand.RELOADADMIN.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADADMIN.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadNpc", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReloadNpc", M2Share.g_GameCommand.RELOADNPC.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADNPC.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadNpc", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadNpc", M2Share.g_GameCommand.RELOADNPC.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADNPC.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadManage", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReloadManage", M2Share.g_GameCommand.RELOADMANAGE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMANAGE.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadManage", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadManage", M2Share.g_GameCommand.RELOADMANAGE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMANAGE.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadRobotManage", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReloadRobotManage", M2Share.g_GameCommand.RELOADROBOTMANAGE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADROBOTMANAGE.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadRobotManage", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadRobotManage", M2Share.g_GameCommand.RELOADROBOTMANAGE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADROBOTMANAGE.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadRobot", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReloadRobot", M2Share.g_GameCommand.RELOADROBOT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADROBOT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadRobot", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadRobot", M2Share.g_GameCommand.RELOADROBOT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADROBOT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadMonitems", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReloadMonitems", M2Share.g_GameCommand.RELOADMONITEMS.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMONITEMS.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadMonitems", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadMonitems", M2Share.g_GameCommand.RELOADMONITEMS.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMONITEMS.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadDiary", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReloadDiary", M2Share.g_GameCommand.RELOADDIARY.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADDIARY.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadDiary", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadDiary", M2Share.g_GameCommand.RELOADDIARY.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADDIARY.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadItemDB", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReloadItemDB", M2Share.g_GameCommand.RELOADITEMDB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADITEMDB.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadItemDB", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadItemDB", M2Share.g_GameCommand.RELOADITEMDB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADITEMDB.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadMagicDB", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReloadMagicDB", M2Share.g_GameCommand.RELOADMAGICDB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMAGICDB.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadMagicDB", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadMagicDB", M2Share.g_GameCommand.RELOADMAGICDB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMAGICDB.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadMonsterDB", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReloadMonsterDB", M2Share.g_GameCommand.RELOADMONSTERDB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMONSTERDB.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadMonsterDB", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadMonsterDB", M2Share.g_GameCommand.RELOADMONSTERDB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMONSTERDB.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReAlive", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReAlive", M2Share.g_GameCommand.REALIVE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.REALIVE.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReAlive", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReAlive", M2Share.g_GameCommand.REALIVE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.REALIVE.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AdjuestTLevel", "");
            if (LoadString == "")
            {
                WriteString("Command", "AdjuestTLevel", M2Share.g_GameCommand.ADJUESTLEVEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTLEVEL.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AdjuestTLevel", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AdjuestTLevel", M2Share.g_GameCommand.ADJUESTLEVEL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTLEVEL.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AdjuestExp", "");
            if (LoadString == "")
            {
                WriteString("Command", "AdjuestExp", M2Share.g_GameCommand.ADJUESTEXP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTEXP.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AdjuestExp", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AdjuestExp", M2Share.g_GameCommand.ADJUESTEXP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTEXP.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AddGuild", "");
            if (LoadString == "")
            {
                WriteString("Command", "AddGuild", M2Share.g_GameCommand.ADDGUILD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADDGUILD.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AddGuild", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AddGuild", M2Share.g_GameCommand.ADDGUILD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADDGUILD.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelGuild", "");
            if (LoadString == "")
            {
                WriteString("Command", "DelGuild", M2Share.g_GameCommand.DELGUILD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELGUILD.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelGuild", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelGuild", M2Share.g_GameCommand.DELGUILD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELGUILD.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeSabukLord", "");
            if (LoadString == "")
            {
                WriteString("Command", "ChangeSabukLord", M2Share.g_GameCommand.CHANGESABUKLORD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGESABUKLORD.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeSabukLord", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeSabukLord", M2Share.g_GameCommand.CHANGESABUKLORD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGESABUKLORD.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ForcedWallConQuestWar", "");
            if (LoadString == "")
            {
                WriteString("Command", "ForcedWallConQuestWar", M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ForcedWallConQuestWar", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ForcedWallConQuestWar", M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AddToItemEvent", "");
            if (LoadString == "")
            {
                WriteString("Command", "AddToItemEvent", M2Share.g_GameCommand.ADDTOITEMEVENT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADDTOITEMEVENT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AddToItemEvent", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AddToItemEvent", M2Share.g_GameCommand.ADDTOITEMEVENT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADDTOITEMEVENT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AddToItemEventAspieces", "");
            if (LoadString == "")
            {
                WriteString("Command", "AddToItemEventAspieces", M2Share.g_GameCommand.ADDTOITEMEVENTASPIECES.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADDTOITEMEVENTASPIECES.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AddToItemEventAspieces", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AddToItemEventAspieces", M2Share.g_GameCommand.ADDTOITEMEVENTASPIECES.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADDTOITEMEVENTASPIECES.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ItemEventList", "");
            if (LoadString == "")
            {
                WriteString("Command", "ItemEventList", M2Share.g_GameCommand.ITEMEVENTLIST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ITEMEVENTLIST.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ItemEventList", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ItemEventList", M2Share.g_GameCommand.ITEMEVENTLIST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ITEMEVENTLIST.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "StartIngGiftNO", "");
            if (LoadString == "")
            {
                WriteString("Command", "StartIngGiftNO", M2Share.g_GameCommand.STARTINGGIFTNO.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.STARTINGGIFTNO.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "StartIngGiftNO", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "StartIngGiftNO", M2Share.g_GameCommand.STARTINGGIFTNO.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.STARTINGGIFTNO.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DeleteAllItemEvent", "");
            if (LoadString == "")
            {
                WriteString("Command", "DeleteAllItemEvent", M2Share.g_GameCommand.DELETEALLITEMEVENT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELETEALLITEMEVENT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DeleteAllItemEvent", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DeleteAllItemEvent", M2Share.g_GameCommand.DELETEALLITEMEVENT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELETEALLITEMEVENT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "StartItemEvent", "");
            if (LoadString == "")
            {
                WriteString("Command", "StartItemEvent", M2Share.g_GameCommand.STARTITEMEVENT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.STARTITEMEVENT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "StartItemEvent", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "StartItemEvent", M2Share.g_GameCommand.STARTITEMEVENT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.STARTITEMEVENT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ItemEventTerm", "");
            if (LoadString == "")
            {
                WriteString("Command", "ItemEventTerm", M2Share.g_GameCommand.ITEMEVENTTERM.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ITEMEVENTTERM.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ItemEventTerm", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ItemEventTerm", M2Share.g_GameCommand.ITEMEVENTTERM.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ITEMEVENTTERM.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AdjuestTestLevel", "");
            if (LoadString == "")
            {
                WriteString("Command", "AdjuestTestLevel", M2Share.g_GameCommand.ADJUESTTESTLEVEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTTESTLEVEL.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AdjuestTestLevel", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AdjuestTestLevel", M2Share.g_GameCommand.ADJUESTTESTLEVEL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTTESTLEVEL.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "OpTraining", "");
            if (LoadString == "")
            {
                WriteString("Command", "OpTraining", M2Share.g_GameCommand.TRAININGSKILL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TRAININGSKILL.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "OpTraining", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "OpTraining", M2Share.g_GameCommand.TRAININGSKILL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TRAININGSKILL.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "OpDeleteSkill", "");
            if (LoadString == "")
            {
                WriteString("Command", "OpDeleteSkill", M2Share.g_GameCommand.OPDELETESKILL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.OPDELETESKILL.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "OpDeleteSkill", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "OpDeleteSkill", M2Share.g_GameCommand.OPDELETESKILL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.OPDELETESKILL.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeWeaponDura", "");
            if (LoadString == "")
            {
                WriteString("Command", "ChangeWeaponDura", M2Share.g_GameCommand.CHANGEWEAPONDURA.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEWEAPONDURA.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeWeaponDura", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeWeaponDura", M2Share.g_GameCommand.CHANGEWEAPONDURA.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEWEAPONDURA.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadGuildAll", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReloadGuildAll", M2Share.g_GameCommand.RELOADGUILDALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADGUILDALL.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadGuildAll", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadGuildAll", M2Share.g_GameCommand.RELOADGUILDALL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADGUILDALL.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Who", "");
            if (LoadString == "")
            {
                WriteString("Command", "Who", M2Share.g_GameCommand.WHO.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.WHO.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Who", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Who", M2Share.g_GameCommand.WHO.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.WHO.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Total", "");
            if (LoadString == "")
            {
                WriteString("Command", "Total", M2Share.g_GameCommand.TOTAL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TOTAL.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Total", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Total", M2Share.g_GameCommand.TOTAL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TOTAL.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "TestGa", "");
            if (LoadString == "")
            {
                WriteString("Command", "TestGa", M2Share.g_GameCommand.TESTGA.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TESTGA.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "TestGa", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "TestGa", M2Share.g_GameCommand.TESTGA.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TESTGA.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "MapInfo", "");
            if (LoadString == "")
            {
                WriteString("Command", "MapInfo", M2Share.g_GameCommand.MAPINFO.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MAPINFO.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "MapInfo", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MapInfo", M2Share.g_GameCommand.MAPINFO.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MAPINFO.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SbkDoor", "");
            if (LoadString == "")
            {
                WriteString("Command", "SbkDoor", M2Share.g_GameCommand.SBKDOOR.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SBKDOOR.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SbkDoor", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SbkDoor", M2Share.g_GameCommand.SBKDOOR.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SBKDOOR.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeDearName", "");
            if (LoadString == "")
            {
                WriteString("Command", "ChangeDearName", M2Share.g_GameCommand.CHANGEDEARNAME.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEDEARNAME.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeDearName", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeDearName", M2Share.g_GameCommand.CHANGEDEARNAME.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEDEARNAME.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeMasterName", "");
            if (LoadString == "")
            {
                WriteString("Command", "ChangeMasterrName", M2Share.g_GameCommand.CHANGEMASTERNAME.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEMASTERNAME.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeMasterName", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeMasterName", M2Share.g_GameCommand.CHANGEMASTERNAME.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEMASTERNAME.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "StartQuest", "");
            if (LoadString == "")
            {
                WriteString("Command", "StartQuest", M2Share.g_GameCommand.STARTQUEST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.STARTQUEST.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "StartQuest", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "StartQuest", M2Share.g_GameCommand.STARTQUEST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.STARTQUEST.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SetPermission", "");
            if (LoadString == "")
            {
                WriteString("Command", "SetPermission", M2Share.g_GameCommand.SETPERMISSION.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETPERMISSION.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SetPermission", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SetPermission", M2Share.g_GameCommand.SETPERMISSION.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SETPERMISSION.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ClearMon", "");
            if (LoadString == "")
            {
                WriteString("Command", "ClearMon", M2Share.g_GameCommand.CLEARMON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CLEARMON.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ClearMon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ClearMon", M2Share.g_GameCommand.CLEARMON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CLEARMON.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReNewLevel", "");
            if (LoadString == "")
            {
                WriteString("Command", "ReNewLevel", M2Share.g_GameCommand.RENEWLEVEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RENEWLEVEL.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReNewLevel", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReNewLevel", M2Share.g_GameCommand.RENEWLEVEL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RENEWLEVEL.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DenyIPaddrLogon", "");
            if (LoadString == "")
            {
                WriteString("Command", "DenyIPaddrLogon", M2Share.g_GameCommand.DENYIPLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DENYIPLOGON.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DenyIPaddrLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DenyIPaddrLogon", M2Share.g_GameCommand.DENYIPLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DENYIPLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DenyAccountLogon", "");
            if (LoadString == "")
            {
                WriteString("Command", "DenyAccountLogon", M2Share.g_GameCommand.DENYACCOUNTLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DENYACCOUNTLOGON.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DenyAccountLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DenyAccountLogon", M2Share.g_GameCommand.DENYACCOUNTLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DENYACCOUNTLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DenyCharNameLogon", "");
            if (LoadString == "")
            {
                WriteString("Command", "DenyCharNameLogon", M2Share.g_GameCommand.DENYCHARNAMELOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DENYCHARNAMELOGON.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DenyCharNameLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DenyCharNameLogon", M2Share.g_GameCommand.DENYCHARNAMELOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DENYCHARNAMELOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelDenyIPLogon", "");
            if (LoadString == "")
            {
                WriteString("Command", "DelDenyIPLogon", M2Share.g_GameCommand.DELDENYIPLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYIPLOGON.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelDenyIPLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelDenyIPLogon", M2Share.g_GameCommand.DELDENYIPLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYIPLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelDenyAccountLogon", "");
            if (LoadString == "")
            {
                WriteString("Command", "DelDenyAccountLogon", M2Share.g_GameCommand.DELDENYACCOUNTLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYACCOUNTLOGON.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelDenyAccountLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelDenyAccountLogon", M2Share.g_GameCommand.DELDENYACCOUNTLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYACCOUNTLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelDenyCharNameLogon", "");
            if (LoadString == "")
            {
                WriteString("Command", "DelDenyCharNameLogon", M2Share.g_GameCommand.DELDENYCHARNAMELOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYCHARNAMELOGON.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelDenyCharNameLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelDenyCharNameLogon", M2Share.g_GameCommand.DELDENYCHARNAMELOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYCHARNAMELOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ShowDenyIPLogon", "");
            if (LoadString == "")
            {
                WriteString("Command", "ShowDenyIPLogon", M2Share.g_GameCommand.SHOWDENYIPLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYIPLOGON.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ShowDenyIPLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ShowDenyIPLogon", M2Share.g_GameCommand.SHOWDENYIPLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYIPLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ShowDenyAccountLogon", "");
            if (LoadString == "")
            {
                WriteString("Command", "ShowDenyAccountLogon", M2Share.g_GameCommand.SHOWDENYACCOUNTLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYACCOUNTLOGON.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ShowDenyAccountLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ShowDenyAccountLogon", M2Share.g_GameCommand.SHOWDENYACCOUNTLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYACCOUNTLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ShowDenyCharNameLogon", "");
            if (LoadString == "")
            {
                WriteString("Command", "ShowDenyCharNameLogon", M2Share.g_GameCommand.SHOWDENYCHARNAMELOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYCHARNAMELOGON.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ShowDenyCharNameLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ShowDenyCharNameLogon", M2Share.g_GameCommand.SHOWDENYCHARNAMELOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYCHARNAMELOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ViewWhisper", "");
            if (LoadString == "")
            {
                WriteString("Command", "ViewWhisper", M2Share.g_GameCommand.VIEWWHISPER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.VIEWWHISPER.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ViewWhisper", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ViewWhisper", M2Share.g_GameCommand.VIEWWHISPER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.VIEWWHISPER.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SpiritStart", "");
            if (LoadString == "")
            {
                WriteString("Command", "SpiritStart", M2Share.g_GameCommand.SPIRIT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SPIRIT.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SpiritStart", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SpiritStart", M2Share.g_GameCommand.SPIRIT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SPIRIT.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SpiritStop", "");
            if (LoadString == "")
            {
                WriteString("Command", "SpiritStop", M2Share.g_GameCommand.SPIRITSTOP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SPIRITSTOP.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SpiritStop", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SpiritStop", M2Share.g_GameCommand.SPIRITSTOP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SPIRITSTOP.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SetMapMode", "");
            if (LoadString == "")
            {
                WriteString("Command", "SetMapMode", M2Share.g_GameCommand.SETMAPMODE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETMAPMODE.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SetMapMode", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SetMapMode", M2Share.g_GameCommand.SETMAPMODE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SETMAPMODE.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ShoweMapMode", "");
            if (LoadString == "")
            {
                WriteString("Command", "ShoweMapMode", M2Share.g_GameCommand.SHOWMAPMODE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWMAPMODE.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ShoweMapMode", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ShoweMapMode", M2Share.g_GameCommand.SHOWMAPMODE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWMAPMODE.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ClearBag", "");
            if (LoadString == "")
            {
                WriteString("Command", "ClearBag", M2Share.g_GameCommand.CLEARBAG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CLEARBAG.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ClearBag", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ClearBag", M2Share.g_GameCommand.CLEARBAG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CLEARBAG.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "LockLogin", "");
            if (LoadString == "")
            {
                WriteString("Command", "LockLogin", M2Share.g_GameCommand.LOCKLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LOCKLOGON.sCmd = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "LockLogin", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "LockLogin", M2Share.g_GameCommand.LOCKLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.LOCKLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "GMRedMsgCmd", "");
            if (LoadString == "")
            {
                WriteString("Command", "GMRedMsgCmd", M2Share.g_GMRedMsgCmd);
            }
            else
            {
                M2Share.g_GMRedMsgCmd = LoadString[0];
            }
            nLoadInteger = ReadInteger("Permission", "GMRedMsgCmd", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "GMRedMsgCmd", M2Share.g_nGMREDMSGCMD);
            }
            else
            {
                M2Share.g_nGMREDMSGCMD = nLoadInteger;
            }
        }
    }
}