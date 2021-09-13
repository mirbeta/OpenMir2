using SystemModule.Common;

namespace GameSvr.Configs
{
    public class GameCmdConfig
    {
        public IniFile CommandConf = null;

        public GameCmdConfig()
        {
            CommandConf = new IniFile(M2Share.sCommandFileName);
        }

        public void LoadConfig()
        {
            string LoadString;
            int nLoadInteger;
            LoadString = CommandConf.ReadString("Command", "Date", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Date", M2Share.g_GameCommand.DATA.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DATA.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Date", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Date", M2Share.g_GameCommand.DATA.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DATA.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "PrvMsg", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "PrvMsg", M2Share.g_GameCommand.PRVMSG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.PRVMSG.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "PrvMsg", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "PrvMsg", M2Share.g_GameCommand.PRVMSG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.PRVMSG.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "AllowMsg", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AllowMsg", M2Share.g_GameCommand.ALLOWMSG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWMSG.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "AllowMsg", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "AllowMsg", M2Share.g_GameCommand.ALLOWMSG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWMSG.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "LetShout", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "LetShout", M2Share.g_GameCommand.LETSHOUT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LETSHOUT.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "LetTrade", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "LetTrade", M2Share.g_GameCommand.LETTRADE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LETTRADE.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "LetGuild", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "LetGuild", M2Share.g_GameCommand.LETGUILD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LETGUILD.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "EndGuild", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "EndGuild", M2Share.g_GameCommand.ENDGUILD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ENDGUILD.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "BanGuildChat", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "BanGuildChat", M2Share.g_GameCommand.BANGUILDCHAT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.BANGUILDCHAT.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "AuthAlly", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AuthAlly", M2Share.g_GameCommand.AUTHALLY.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.AUTHALLY.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "Auth", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Auth", M2Share.g_GameCommand.AUTH.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.AUTH.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "AuthCancel", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AuthCancel", M2Share.g_GameCommand.AUTHCANCEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.AUTHCANCEL.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "ViewDiary", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ViewDiary", M2Share.g_GameCommand.DIARY.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DIARY.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "UserMove", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "UserMove", M2Share.g_GameCommand.USERMOVE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.USERMOVE.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "Searching", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Searching", M2Share.g_GameCommand.SEARCHING.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SEARCHING.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "AllowGroupCall", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AllowGroupCall", M2Share.g_GameCommand.ALLOWGROUPCALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWGROUPCALL.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "GroupCall", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "GroupCall", M2Share.g_GameCommand.GROUPRECALLL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GROUPRECALLL.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "AllowGuildReCall", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AllowGuildReCall", M2Share.g_GameCommand.ALLOWGUILDRECALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWGUILDRECALL.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "GuildReCall", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "GuildReCall", M2Share.g_GameCommand.GUILDRECALLL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GUILDRECALLL.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "StorageUnLock", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "StorageUnLock", M2Share.g_GameCommand.UNLOCKSTORAGE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.UNLOCKSTORAGE.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "PasswordUnLock", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "PasswordUnLock", M2Share.g_GameCommand.UNLOCK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.UNLOCK.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "StorageLock", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "StorageLock", M2Share.g_GameCommand.__LOCK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.__LOCK.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "StorageSetPassword", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "StorageSetPassword", M2Share.g_GameCommand.SETPASSWORD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETPASSWORD.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "PasswordLock", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "PasswordLock", M2Share.g_GameCommand.PASSWORDLOCK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.PASSWORDLOCK.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "StorageChgPassword", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "StorageChgPassword", M2Share.g_GameCommand.CHGPASSWORD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHGPASSWORD.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "StorageClearPassword", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "StorageClearPassword", M2Share.g_GameCommand.CLRPASSWORD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CLRPASSWORD.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "StorageClearPassword", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "StorageClearPassword", M2Share.g_GameCommand.CLRPASSWORD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CLRPASSWORD.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "StorageUserClearPassword", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "StorageUserClearPassword", M2Share.g_GameCommand.UNPASSWORD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.UNPASSWORD.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "MemberFunc", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "MemberFunc", M2Share.g_GameCommand.MEMBERFUNCTION.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MEMBERFUNCTION.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "MemberFuncEx", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "MemberFuncEx", M2Share.g_GameCommand.MEMBERFUNCTIONEX.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MEMBERFUNCTIONEX.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "Dear", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Dear", M2Share.g_GameCommand.DEAR.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DEAR.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "Master", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Master", M2Share.g_GameCommand.MASTER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MASTER.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "DearRecall", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DearRecall", M2Share.g_GameCommand.DEARRECALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DEARRECALL.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "MasterRecall", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "MasterRecall", M2Share.g_GameCommand.MASTERECALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MASTERECALL.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "AllowDearRecall", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AllowDearRecall", M2Share.g_GameCommand.ALLOWDEARRCALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWDEARRCALL.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "AllowMasterRecall", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AllowMasterRecall", M2Share.g_GameCommand.ALLOWMASTERRECALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWMASTERRECALL.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "AttackMode", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AttackMode", M2Share.g_GameCommand.ATTACKMODE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ATTACKMODE.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "Rest", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Rest", M2Share.g_GameCommand.REST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.REST.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "TakeOnHorse", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "TakeOnHorse", M2Share.g_GameCommand.TAKEONHORSE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TAKEONHORSE.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "TakeOffHorse", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "TakeOffHorse", M2Share.g_GameCommand.TAKEOFHORSE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TAKEOFHORSE.sCmd = LoadString;
            }
            LoadString = CommandConf.ReadString("Command", "HumanLocal", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "HumanLocal", M2Share.g_GameCommand.HUMANLOCAL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.HUMANLOCAL.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "HumanLocal", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "HumanLocal", M2Share.g_GameCommand.HUMANLOCAL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.HUMANLOCAL.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Move", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Move", M2Share.g_GameCommand.MOVE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOVE.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "MoveMin", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "MoveMin", M2Share.g_GameCommand.MOVE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOVE.nPerMissionMin = nLoadInteger;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "MoveMax", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "MoveMax", M2Share.g_GameCommand.MOVE.nPerMissionMax);
            }
            else
            {
                M2Share.g_GameCommand.MOVE.nPerMissionMax = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "PositionMove", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "PositionMove", M2Share.g_GameCommand.POSITIONMOVE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.POSITIONMOVE.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "PositionMoveMin", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "PositionMoveMin", M2Share.g_GameCommand.POSITIONMOVE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.POSITIONMOVE.nPerMissionMin = nLoadInteger;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "PositionMoveMax", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "PositionMoveMax", M2Share.g_GameCommand.POSITIONMOVE.nPerMissionMax);
            }
            else
            {
                M2Share.g_GameCommand.POSITIONMOVE.nPerMissionMax = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Info", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Info", M2Share.g_GameCommand.INFO.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.INFO.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Info", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Info", M2Share.g_GameCommand.INFO.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.INFO.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "MobLevel", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "MobLevel", M2Share.g_GameCommand.MOBLEVEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOBLEVEL.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "MobLevel", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "MobLevel", M2Share.g_GameCommand.MOBLEVEL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOBLEVEL.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "MobCount", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "MobCount", M2Share.g_GameCommand.MOBCOUNT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOBCOUNT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "MobCount", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "MobCount", M2Share.g_GameCommand.MOBCOUNT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOBCOUNT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "HumanCount", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "HumanCount", M2Share.g_GameCommand.HUMANCOUNT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.HUMANCOUNT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "HumanCount", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "HumanCount", M2Share.g_GameCommand.HUMANCOUNT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.HUMANCOUNT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Map", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Map", M2Share.g_GameCommand.MAP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MAP.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Map", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Map", M2Share.g_GameCommand.MAP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MAP.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Kick", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Kick", M2Share.g_GameCommand.KICK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.KICK.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Kick", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Kick", M2Share.g_GameCommand.KICK.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.KICK.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Ting", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Ting", M2Share.g_GameCommand.TING.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TING.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Ting", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Ting", M2Share.g_GameCommand.TING.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TING.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "SuperTing", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "SuperTing", M2Share.g_GameCommand.SUPERTING.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SUPERTING.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "SuperTing", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "SuperTing", M2Share.g_GameCommand.SUPERTING.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SUPERTING.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "MapMove", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "MapMove", M2Share.g_GameCommand.MAPMOVE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MAPMOVE.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "MapMove", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "MapMove", M2Share.g_GameCommand.MAPMOVE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MAPMOVE.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Shutup", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Shutup", M2Share.g_GameCommand.SHUTUP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHUTUP.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Shutup", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Shutup", M2Share.g_GameCommand.SHUTUP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHUTUP.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReleaseShutup", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReleaseShutup", M2Share.g_GameCommand.RELEASESHUTUP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELEASESHUTUP.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReleaseShutup", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReleaseShutup", M2Share.g_GameCommand.RELEASESHUTUP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELEASESHUTUP.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ShutupList", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ShutupList", M2Share.g_GameCommand.SHUTUPLIST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHUTUPLIST.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ShutupList", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ShutupList", M2Share.g_GameCommand.SHUTUPLIST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHUTUPLIST.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "GameMaster", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "GameMaster", M2Share.g_GameCommand.GAMEMASTER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GAMEMASTER.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "GameMaster", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "GameMaster", M2Share.g_GameCommand.GAMEMASTER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.GAMEMASTER.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ObServer", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ObServer", M2Share.g_GameCommand.OBSERVER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.OBSERVER.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ObServer", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ObServer", M2Share.g_GameCommand.OBSERVER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.OBSERVER.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "SuperMan", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "SuperMan", M2Share.g_GameCommand.SUEPRMAN.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SUEPRMAN.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "SuperMan", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "SuperMan", M2Share.g_GameCommand.SUEPRMAN.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SUEPRMAN.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Level", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Level", M2Share.g_GameCommand.LEVEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LEVEL.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Level", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Level", M2Share.g_GameCommand.LEVEL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.LEVEL.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "SabukWallGold", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "SabukWallGold", M2Share.g_GameCommand.SABUKWALLGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SABUKWALLGOLD.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "SabukWallGold", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "SabukWallGold", M2Share.g_GameCommand.SABUKWALLGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SABUKWALLGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Recall", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Recall", M2Share.g_GameCommand.RECALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RECALL.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Recall", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Recall", M2Share.g_GameCommand.RECALL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RECALL.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReGoto", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReGoto", M2Share.g_GameCommand.REGOTO.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.REGOTO.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReGoto", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReGoto", M2Share.g_GameCommand.REGOTO.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.REGOTO.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Flag", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Flag", M2Share.g_GameCommand.SHOWFLAG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWFLAG.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Flag", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Flag", M2Share.g_GameCommand.SHOWFLAG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWFLAG.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ShowOpen", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ShowOpen", M2Share.g_GameCommand.SHOWOPEN.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWOPEN.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ShowOpen", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ShowOpen", M2Share.g_GameCommand.SHOWOPEN.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWOPEN.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ShowUnit", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ShowUnit", M2Share.g_GameCommand.SHOWUNIT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWUNIT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ShowUnit", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ShowUnit", M2Share.g_GameCommand.SHOWUNIT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWUNIT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Attack", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Attack", M2Share.g_GameCommand.ATTACK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ATTACK.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Attack", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Attack", M2Share.g_GameCommand.ATTACK.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ATTACK.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Mob", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Mob", M2Share.g_GameCommand.MOB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOB.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Mob", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Mob", M2Share.g_GameCommand.MOB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOB.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "MobNpc", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "MobNpc", M2Share.g_GameCommand.MOBNPC.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOBNPC.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "MobNpc", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "MobNpc", M2Share.g_GameCommand.MOBNPC.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOBNPC.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DelNpc", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DelNpc", M2Share.g_GameCommand.DELNPC.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELNPC.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DelNpc", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DelNpc", M2Share.g_GameCommand.DELNPC.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELNPC.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "NpcScript", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "NpcScript", M2Share.g_GameCommand.NPCSCRIPT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.NPCSCRIPT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "NpcScript", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "NpcScript", M2Share.g_GameCommand.NPCSCRIPT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.NPCSCRIPT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "RecallMob", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "RecallMob", M2Share.g_GameCommand.RECALLMOB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RECALLMOB.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "RecallMob", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "RecallMob", M2Share.g_GameCommand.RECALLMOB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RECALLMOB.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "LuckPoint", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "LuckPoint", M2Share.g_GameCommand.LUCKYPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LUCKYPOINT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "LuckPoint", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "LuckPoint", M2Share.g_GameCommand.LUCKYPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.LUCKYPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "LotteryTicket", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "LotteryTicket", M2Share.g_GameCommand.LOTTERYTICKET.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LOTTERYTICKET.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "LotteryTicket", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "LotteryTicket", M2Share.g_GameCommand.LOTTERYTICKET.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.LOTTERYTICKET.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReloadGuild", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReloadGuild", M2Share.g_GameCommand.RELOADGUILD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADGUILD.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReloadGuild", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReloadGuild", M2Share.g_GameCommand.RELOADGUILD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADGUILD.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReloadLineNotice", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReloadLineNotice", M2Share.g_GameCommand.RELOADLINENOTICE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADLINENOTICE.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReloadLineNotice", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReloadLineNotice", M2Share.g_GameCommand.RELOADLINENOTICE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADLINENOTICE.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReloadAbuse", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReloadAbuse", M2Share.g_GameCommand.RELOADABUSE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADABUSE.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReloadAbuse", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReloadAbuse", M2Share.g_GameCommand.RELOADABUSE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADABUSE.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "BackStep", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "BackStep", M2Share.g_GameCommand.BACKSTEP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.BACKSTEP.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "BackStep", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "BackStep", M2Share.g_GameCommand.BACKSTEP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.BACKSTEP.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Ball", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Ball", M2Share.g_GameCommand.BALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.BALL.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Ball", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Ball", M2Share.g_GameCommand.BALL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.BALL.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "FreePenalty", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "FreePenalty", M2Share.g_GameCommand.FREEPENALTY.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.FREEPENALTY.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "FreePenalty", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "FreePenalty", M2Share.g_GameCommand.FREEPENALTY.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.FREEPENALTY.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "PkPoint", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "PkPoint", M2Share.g_GameCommand.PKPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.PKPOINT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "PkPoint", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "PkPoint", M2Share.g_GameCommand.PKPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.PKPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "IncPkPoint", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "IncPkPoint", M2Share.g_GameCommand.INCPKPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.INCPKPOINT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "IncPkPoint", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "IncPkPoint", M2Share.g_GameCommand.INCPKPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.INCPKPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ChangeLuck", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ChangeLuck", M2Share.g_GameCommand.CHANGELUCK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGELUCK.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ChangeLuck", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ChangeLuck", M2Share.g_GameCommand.CHANGELUCK.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGELUCK.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Hunger", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Hunger", M2Share.g_GameCommand.HUNGER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.HUNGER.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Hunger", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Hunger", M2Share.g_GameCommand.HUNGER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.HUNGER.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Hair", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Hair", M2Share.g_GameCommand.HAIR.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.HAIR.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Hair", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Hair", M2Share.g_GameCommand.HAIR.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.HAIR.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Training", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Training", M2Share.g_GameCommand.TRAINING.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TRAINING.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Training", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Training", M2Share.g_GameCommand.TRAINING.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TRAINING.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DeleteSkill", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DeleteSkill", M2Share.g_GameCommand.DELETESKILL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELETESKILL.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DeleteSkill", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DeleteSkill", M2Share.g_GameCommand.DELETESKILL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELETESKILL.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ChangeJob", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ChangeJob", M2Share.g_GameCommand.CHANGEJOB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEJOB.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ChangeJob", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ChangeJob", M2Share.g_GameCommand.CHANGEJOB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEJOB.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ChangeGender", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ChangeGender", M2Share.g_GameCommand.CHANGEGENDER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEGENDER.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ChangeGender", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ChangeGender", M2Share.g_GameCommand.CHANGEGENDER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEGENDER.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "NameColor", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "NameColor", M2Share.g_GameCommand.NAMECOLOR.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.NAMECOLOR.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "NameColor", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "NameColor", M2Share.g_GameCommand.NAMECOLOR.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.NAMECOLOR.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Mission", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Mission", M2Share.g_GameCommand.MISSION.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MISSION.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Mission", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Mission", M2Share.g_GameCommand.MISSION.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MISSION.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "MobPlace", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "MobPlace", M2Share.g_GameCommand.MOBPLACE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOBPLACE.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "MobPlace", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "MobPlace", M2Share.g_GameCommand.MOBPLACE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOBPLACE.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Transparecy", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Transparecy", M2Share.g_GameCommand.TRANSPARECY.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TRANSPARECY.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Transparecy", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Transparecy", M2Share.g_GameCommand.TRANSPARECY.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TRANSPARECY.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DeleteItem", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DeleteItem", M2Share.g_GameCommand.DELETEITEM.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELETEITEM.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DeleteItem", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DeleteItem", M2Share.g_GameCommand.DELETEITEM.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELETEITEM.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Level0", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Level0", M2Share.g_GameCommand.LEVEL0.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LEVEL0.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Level0", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Level0", M2Share.g_GameCommand.LEVEL0.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.LEVEL0.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ClearMission", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ClearMission", M2Share.g_GameCommand.CLEARMISSION.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CLEARMISSION.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ClearMission", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ClearMission", M2Share.g_GameCommand.CLEARMISSION.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CLEARMISSION.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "SetFlag", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "SetFlag", M2Share.g_GameCommand.SETFLAG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETFLAG.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "SetFlag", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "SetFlag", M2Share.g_GameCommand.SETFLAG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SETFLAG.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "SetOpen", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "SetOpen", M2Share.g_GameCommand.SETOPEN.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETOPEN.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "SetOpen", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "SetOpen", M2Share.g_GameCommand.SETOPEN.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SETOPEN.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "SetUnit", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "SetUnit", M2Share.g_GameCommand.SETUNIT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETUNIT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "SetUnit", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "SetUnit", M2Share.g_GameCommand.SETUNIT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SETUNIT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReConnection", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReConnection", M2Share.g_GameCommand.RECONNECTION.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RECONNECTION.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReConnection", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReConnection", M2Share.g_GameCommand.RECONNECTION.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RECONNECTION.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DisableFilter", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DisableFilter", M2Share.g_GameCommand.DISABLEFILTER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DISABLEFILTER.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DisableFilter", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DisableFilter", M2Share.g_GameCommand.DISABLEFILTER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DISABLEFILTER.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ChangeUserFull", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ChangeUserFull", M2Share.g_GameCommand.CHGUSERFULL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHGUSERFULL.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ChangeUserFull", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ChangeUserFull", M2Share.g_GameCommand.CHGUSERFULL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHGUSERFULL.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ChangeZenFastStep", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ChangeZenFastStep", M2Share.g_GameCommand.CHGZENFASTSTEP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHGZENFASTSTEP.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ChangeZenFastStep", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ChangeZenFastStep", M2Share.g_GameCommand.CHGZENFASTSTEP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHGZENFASTSTEP.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ContestPoint", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ContestPoint", M2Share.g_GameCommand.CONTESTPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CONTESTPOINT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ContestPoint", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ContestPoint", M2Share.g_GameCommand.CONTESTPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CONTESTPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "StartContest", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "StartContest", M2Share.g_GameCommand.STARTCONTEST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.STARTCONTEST.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "StartContest", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "StartContest", M2Share.g_GameCommand.STARTCONTEST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.STARTCONTEST.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "EndContest", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "EndContest", M2Share.g_GameCommand.ENDCONTEST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ENDCONTEST.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "EndContest", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "EndContest", M2Share.g_GameCommand.ENDCONTEST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ENDCONTEST.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Announcement", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Announcement", M2Share.g_GameCommand.ANNOUNCEMENT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ANNOUNCEMENT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Announcement", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Announcement", M2Share.g_GameCommand.ANNOUNCEMENT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ANNOUNCEMENT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "OXQuizRoom", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "OXQuizRoom", M2Share.g_GameCommand.OXQUIZROOM.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.OXQUIZROOM.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "OXQuizRoom", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "OXQuizRoom", M2Share.g_GameCommand.OXQUIZROOM.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.OXQUIZROOM.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Gsa", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Gsa", M2Share.g_GameCommand.GSA.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GSA.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Gsa", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Gsa", M2Share.g_GameCommand.GSA.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.GSA.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ChangeItemName", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ChangeItemName", M2Share.g_GameCommand.CHANGEITEMNAME.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEITEMNAME.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ChangeItemName", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ChangeItemName", M2Share.g_GameCommand.CHANGEITEMNAME.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEITEMNAME.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DisableSendMsg", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DisableSendMsg", M2Share.g_GameCommand.DISABLESENDMSG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DISABLESENDMSG.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DisableSendMsg", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DisableSendMsg", M2Share.g_GameCommand.DISABLESENDMSG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DISABLESENDMSG.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "EnableSendMsg", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "EnableSendMsg", M2Share.g_GameCommand.ENABLESENDMSG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ENABLESENDMSG.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "EnableSendMsg", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "EnableSendMsg", M2Share.g_GameCommand.ENABLESENDMSG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ENABLESENDMSG.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DisableSendMsgList", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DisableSendMsgList", M2Share.g_GameCommand.DISABLESENDMSGLIST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DISABLESENDMSGLIST.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DisableSendMsgList", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DisableSendMsgList", M2Share.g_GameCommand.DISABLESENDMSGLIST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DISABLESENDMSGLIST.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Kill", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Kill", M2Share.g_GameCommand.KILL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.KILL.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Kill", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Kill", M2Share.g_GameCommand.KILL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.KILL.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Make", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Make", M2Share.g_GameCommand.MAKE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MAKE.sCmd = LoadString;
                M2Share.CommandSystem.RegisterCommand("Make", LoadString);
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "MakeMin", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "MakeMin", M2Share.g_GameCommand.MAKE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MAKE.nPerMissionMin = nLoadInteger;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "MakeMax", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "MakeMax", M2Share.g_GameCommand.MAKE.nPerMissionMax);
            }
            else
            {
                M2Share.g_GameCommand.MAKE.nPerMissionMax = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "SuperMake", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "SuperMake", M2Share.g_GameCommand.SMAKE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SMAKE.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "SuperMake", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "SuperMake", M2Share.g_GameCommand.SMAKE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SMAKE.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "BonusPoint", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "BonusPoint", M2Share.g_GameCommand.BONUSPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.BONUSPOINT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "BonusPoint", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "BonusPoint", M2Share.g_GameCommand.BONUSPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.BONUSPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DelBonuPoint", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DelBonuPoint", M2Share.g_GameCommand.DELBONUSPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELBONUSPOINT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DelBonuPoint", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DelBonuPoint", M2Share.g_GameCommand.DELBONUSPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELBONUSPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "RestBonuPoint", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "RestBonuPoint", M2Share.g_GameCommand.RESTBONUSPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RESTBONUSPOINT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "RestBonuPoint", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "RestBonuPoint", M2Share.g_GameCommand.RESTBONUSPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RESTBONUSPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "FireBurn", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "FireBurn", M2Share.g_GameCommand.FIREBURN.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.FIREBURN.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "FireBurn", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "FireBurn", M2Share.g_GameCommand.FIREBURN.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.FIREBURN.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "TestStatus", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "TestStatus", M2Share.g_GameCommand.TESTSTATUS.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TESTSTATUS.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "TestStatus", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "TestStatus", M2Share.g_GameCommand.TESTSTATUS.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TESTSTATUS.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DelGold", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DelGold", M2Share.g_GameCommand.DELGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELGOLD.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DelGold", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DelGold", M2Share.g_GameCommand.DELGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "AddGold", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AddGold", M2Share.g_GameCommand.ADDGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADDGOLD.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "AddGold", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "AddGold", M2Share.g_GameCommand.ADDGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADDGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DelGameGold", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DelGameGold", M2Share.g_GameCommand.DELGAMEGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELGAMEGOLD.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DelGameGold", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DelGameGold", M2Share.g_GameCommand.DELGAMEGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELGAMEGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "AddGamePoint", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AddGamePoint", M2Share.g_GameCommand.ADDGAMEGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADDGAMEGOLD.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "AddGameGold", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "AddGameGold", M2Share.g_GameCommand.ADDGAMEGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADDGAMEGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "GameGold", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "GameGold", M2Share.g_GameCommand.GAMEGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GAMEGOLD.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "GameGold", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "GameGold", M2Share.g_GameCommand.GAMEGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.GAMEGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "GamePoint", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "GamePoint", M2Share.g_GameCommand.GAMEPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GAMEPOINT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "GamePoint", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "GamePoint", M2Share.g_GameCommand.GAMEPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.GAMEPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "CreditPoint", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "CreditPoint", M2Share.g_GameCommand.CREDITPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CREDITPOINT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "CreditPoint", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "CreditPoint", M2Share.g_GameCommand.CREDITPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CREDITPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "TestGoldChange", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "TestGoldChange", M2Share.g_GameCommand.TESTGOLDCHANGE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TESTGOLDCHANGE.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "TestGoldChange", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "TestGoldChange", M2Share.g_GameCommand.TESTGOLDCHANGE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TESTGOLDCHANGE.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "RefineWeapon", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "RefineWeapon", M2Share.g_GameCommand.REFINEWEAPON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.REFINEWEAPON.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "RefineWeapon", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "RefineWeapon", M2Share.g_GameCommand.REFINEWEAPON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.REFINEWEAPON.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReloadAdmin", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReloadAdmin", M2Share.g_GameCommand.RELOADADMIN.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADADMIN.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReloadAdmin", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReloadAdmin", M2Share.g_GameCommand.RELOADADMIN.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADADMIN.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReloadNpc", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReloadNpc", M2Share.g_GameCommand.RELOADNPC.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADNPC.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReloadNpc", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReloadNpc", M2Share.g_GameCommand.RELOADNPC.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADNPC.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReloadManage", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReloadManage", M2Share.g_GameCommand.RELOADMANAGE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMANAGE.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReloadManage", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReloadManage", M2Share.g_GameCommand.RELOADMANAGE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMANAGE.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReloadRobotManage", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReloadRobotManage", M2Share.g_GameCommand.RELOADROBOTMANAGE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADROBOTMANAGE.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReloadRobotManage", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReloadRobotManage", M2Share.g_GameCommand.RELOADROBOTMANAGE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADROBOTMANAGE.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReloadRobot", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReloadRobot", M2Share.g_GameCommand.RELOADROBOT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADROBOT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReloadRobot", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReloadRobot", M2Share.g_GameCommand.RELOADROBOT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADROBOT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReloadMonitems", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReloadMonitems", M2Share.g_GameCommand.RELOADMONITEMS.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMONITEMS.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReloadMonitems", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReloadMonitems", M2Share.g_GameCommand.RELOADMONITEMS.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMONITEMS.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReloadDiary", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReloadDiary", M2Share.g_GameCommand.RELOADDIARY.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADDIARY.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReloadDiary", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReloadDiary", M2Share.g_GameCommand.RELOADDIARY.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADDIARY.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReloadItemDB", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReloadItemDB", M2Share.g_GameCommand.RELOADITEMDB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADITEMDB.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReloadItemDB", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReloadItemDB", M2Share.g_GameCommand.RELOADITEMDB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADITEMDB.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReloadMagicDB", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReloadMagicDB", M2Share.g_GameCommand.RELOADMAGICDB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMAGICDB.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReloadMagicDB", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReloadMagicDB", M2Share.g_GameCommand.RELOADMAGICDB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMAGICDB.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReloadMonsterDB", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReloadMonsterDB", M2Share.g_GameCommand.RELOADMONSTERDB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMONSTERDB.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReloadMonsterDB", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReloadMonsterDB", M2Share.g_GameCommand.RELOADMONSTERDB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMONSTERDB.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReAlive", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReAlive", M2Share.g_GameCommand.REALIVE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.REALIVE.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReAlive", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReAlive", M2Share.g_GameCommand.REALIVE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.REALIVE.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "AdjuestTLevel", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AdjuestTLevel", M2Share.g_GameCommand.ADJUESTLEVEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTLEVEL.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "AdjuestTLevel", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "AdjuestTLevel", M2Share.g_GameCommand.ADJUESTLEVEL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTLEVEL.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "AdjuestExp", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AdjuestExp", M2Share.g_GameCommand.ADJUESTEXP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTEXP.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "AdjuestExp", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "AdjuestExp", M2Share.g_GameCommand.ADJUESTEXP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTEXP.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "AddGuild", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AddGuild", M2Share.g_GameCommand.ADDGUILD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADDGUILD.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "AddGuild", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "AddGuild", M2Share.g_GameCommand.ADDGUILD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADDGUILD.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DelGuild", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DelGuild", M2Share.g_GameCommand.DELGUILD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELGUILD.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DelGuild", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DelGuild", M2Share.g_GameCommand.DELGUILD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELGUILD.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ChangeSabukLord", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ChangeSabukLord", M2Share.g_GameCommand.CHANGESABUKLORD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGESABUKLORD.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ChangeSabukLord", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ChangeSabukLord", M2Share.g_GameCommand.CHANGESABUKLORD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGESABUKLORD.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ForcedWallConQuestWar", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ForcedWallConQuestWar", M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ForcedWallConQuestWar", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ForcedWallConQuestWar", M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "AddToItemEvent", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AddToItemEvent", M2Share.g_GameCommand.ADDTOITEMEVENT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADDTOITEMEVENT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "AddToItemEvent", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "AddToItemEvent", M2Share.g_GameCommand.ADDTOITEMEVENT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADDTOITEMEVENT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "AddToItemEventAspieces", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AddToItemEventAspieces", M2Share.g_GameCommand.ADDTOITEMEVENTASPIECES.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADDTOITEMEVENTASPIECES.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "AddToItemEventAspieces", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "AddToItemEventAspieces", M2Share.g_GameCommand.ADDTOITEMEVENTASPIECES.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADDTOITEMEVENTASPIECES.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ItemEventList", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ItemEventList", M2Share.g_GameCommand.ITEMEVENTLIST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ITEMEVENTLIST.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ItemEventList", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ItemEventList", M2Share.g_GameCommand.ITEMEVENTLIST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ITEMEVENTLIST.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "StartIngGiftNO", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "StartIngGiftNO", M2Share.g_GameCommand.STARTINGGIFTNO.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.STARTINGGIFTNO.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "StartIngGiftNO", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "StartIngGiftNO", M2Share.g_GameCommand.STARTINGGIFTNO.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.STARTINGGIFTNO.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DeleteAllItemEvent", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DeleteAllItemEvent", M2Share.g_GameCommand.DELETEALLITEMEVENT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELETEALLITEMEVENT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DeleteAllItemEvent", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DeleteAllItemEvent", M2Share.g_GameCommand.DELETEALLITEMEVENT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELETEALLITEMEVENT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "StartItemEvent", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "StartItemEvent", M2Share.g_GameCommand.STARTITEMEVENT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.STARTITEMEVENT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "StartItemEvent", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "StartItemEvent", M2Share.g_GameCommand.STARTITEMEVENT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.STARTITEMEVENT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ItemEventTerm", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ItemEventTerm", M2Share.g_GameCommand.ITEMEVENTTERM.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ITEMEVENTTERM.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ItemEventTerm", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ItemEventTerm", M2Share.g_GameCommand.ITEMEVENTTERM.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ITEMEVENTTERM.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "AdjuestTestLevel", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "AdjuestTestLevel", M2Share.g_GameCommand.ADJUESTTESTLEVEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTTESTLEVEL.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "AdjuestTestLevel", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "AdjuestTestLevel", M2Share.g_GameCommand.ADJUESTTESTLEVEL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTTESTLEVEL.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "OpTraining", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "OpTraining", M2Share.g_GameCommand.TRAININGSKILL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TRAININGSKILL.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "OpTraining", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "OpTraining", M2Share.g_GameCommand.TRAININGSKILL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TRAININGSKILL.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "OpDeleteSkill", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "OpDeleteSkill", M2Share.g_GameCommand.OPDELETESKILL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.OPDELETESKILL.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "OpDeleteSkill", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "OpDeleteSkill", M2Share.g_GameCommand.OPDELETESKILL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.OPDELETESKILL.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ChangeWeaponDura", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ChangeWeaponDura", M2Share.g_GameCommand.CHANGEWEAPONDURA.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEWEAPONDURA.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ChangeWeaponDura", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ChangeWeaponDura", M2Share.g_GameCommand.CHANGEWEAPONDURA.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEWEAPONDURA.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReloadGuildAll", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReloadGuildAll", M2Share.g_GameCommand.RELOADGUILDALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADGUILDALL.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReloadGuildAll", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReloadGuildAll", M2Share.g_GameCommand.RELOADGUILDALL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADGUILDALL.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Who", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Who", M2Share.g_GameCommand.WHO.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.WHO.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Who", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Who", M2Share.g_GameCommand.WHO.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.WHO.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "Total", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "Total", M2Share.g_GameCommand.TOTAL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TOTAL.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "Total", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "Total", M2Share.g_GameCommand.TOTAL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TOTAL.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "TestGa", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "TestGa", M2Share.g_GameCommand.TESTGA.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TESTGA.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "TestGa", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "TestGa", M2Share.g_GameCommand.TESTGA.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TESTGA.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "MapInfo", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "MapInfo", M2Share.g_GameCommand.MAPINFO.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MAPINFO.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "MapInfo", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "MapInfo", M2Share.g_GameCommand.MAPINFO.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MAPINFO.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "SbkDoor", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "SbkDoor", M2Share.g_GameCommand.SBKDOOR.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SBKDOOR.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "SbkDoor", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "SbkDoor", M2Share.g_GameCommand.SBKDOOR.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SBKDOOR.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ChangeDearName", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ChangeDearName", M2Share.g_GameCommand.CHANGEDEARNAME.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEDEARNAME.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ChangeDearName", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ChangeDearName", M2Share.g_GameCommand.CHANGEDEARNAME.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEDEARNAME.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ChangeMasterName", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ChangeMasterrName", M2Share.g_GameCommand.CHANGEMASTERNAME.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEMASTERNAME.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ChangeMasterName", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ChangeMasterName", M2Share.g_GameCommand.CHANGEMASTERNAME.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEMASTERNAME.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "StartQuest", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "StartQuest", M2Share.g_GameCommand.STARTQUEST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.STARTQUEST.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "StartQuest", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "StartQuest", M2Share.g_GameCommand.STARTQUEST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.STARTQUEST.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "SetPermission", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "SetPermission", M2Share.g_GameCommand.SETPERMISSION.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETPERMISSION.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "SetPermission", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "SetPermission", M2Share.g_GameCommand.SETPERMISSION.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SETPERMISSION.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ClearMon", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ClearMon", M2Share.g_GameCommand.CLEARMON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CLEARMON.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ClearMon", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ClearMon", M2Share.g_GameCommand.CLEARMON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CLEARMON.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ReNewLevel", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ReNewLevel", M2Share.g_GameCommand.RENEWLEVEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RENEWLEVEL.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ReNewLevel", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ReNewLevel", M2Share.g_GameCommand.RENEWLEVEL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RENEWLEVEL.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DenyIPaddrLogon", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DenyIPaddrLogon", M2Share.g_GameCommand.DENYIPLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DENYIPLOGON.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DenyIPaddrLogon", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DenyIPaddrLogon", M2Share.g_GameCommand.DENYIPLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DENYIPLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DenyAccountLogon", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DenyAccountLogon", M2Share.g_GameCommand.DENYACCOUNTLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DENYACCOUNTLOGON.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DenyAccountLogon", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DenyAccountLogon", M2Share.g_GameCommand.DENYACCOUNTLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DENYACCOUNTLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DenyCharNameLogon", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DenyCharNameLogon", M2Share.g_GameCommand.DENYCHARNAMELOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DENYCHARNAMELOGON.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DenyCharNameLogon", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DenyCharNameLogon", M2Share.g_GameCommand.DENYCHARNAMELOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DENYCHARNAMELOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DelDenyIPLogon", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DelDenyIPLogon", M2Share.g_GameCommand.DELDENYIPLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYIPLOGON.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DelDenyIPLogon", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DelDenyIPLogon", M2Share.g_GameCommand.DELDENYIPLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYIPLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DelDenyAccountLogon", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DelDenyAccountLogon", M2Share.g_GameCommand.DELDENYACCOUNTLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYACCOUNTLOGON.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DelDenyAccountLogon", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DelDenyAccountLogon", M2Share.g_GameCommand.DELDENYACCOUNTLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYACCOUNTLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "DelDenyCharNameLogon", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "DelDenyCharNameLogon", M2Share.g_GameCommand.DELDENYCHARNAMELOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYCHARNAMELOGON.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "DelDenyCharNameLogon", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "DelDenyCharNameLogon", M2Share.g_GameCommand.DELDENYCHARNAMELOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYCHARNAMELOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ShowDenyIPLogon", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ShowDenyIPLogon", M2Share.g_GameCommand.SHOWDENYIPLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYIPLOGON.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ShowDenyIPLogon", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ShowDenyIPLogon", M2Share.g_GameCommand.SHOWDENYIPLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYIPLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ShowDenyAccountLogon", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ShowDenyAccountLogon", M2Share.g_GameCommand.SHOWDENYACCOUNTLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYACCOUNTLOGON.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ShowDenyAccountLogon", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ShowDenyAccountLogon", M2Share.g_GameCommand.SHOWDENYACCOUNTLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYACCOUNTLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ShowDenyCharNameLogon", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ShowDenyCharNameLogon", M2Share.g_GameCommand.SHOWDENYCHARNAMELOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYCHARNAMELOGON.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ShowDenyCharNameLogon", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ShowDenyCharNameLogon", M2Share.g_GameCommand.SHOWDENYCHARNAMELOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYCHARNAMELOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ViewWhisper", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ViewWhisper", M2Share.g_GameCommand.VIEWWHISPER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.VIEWWHISPER.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ViewWhisper", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ViewWhisper", M2Share.g_GameCommand.VIEWWHISPER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.VIEWWHISPER.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "SpiritStart", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "SpiritStart", M2Share.g_GameCommand.SPIRIT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SPIRIT.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "SpiritStart", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "SpiritStart", M2Share.g_GameCommand.SPIRIT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SPIRIT.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "SpiritStop", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "SpiritStop", M2Share.g_GameCommand.SPIRITSTOP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SPIRITSTOP.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "SpiritStop", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "SpiritStop", M2Share.g_GameCommand.SPIRITSTOP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SPIRITSTOP.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "SetMapMode", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "SetMapMode", M2Share.g_GameCommand.SETMAPMODE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETMAPMODE.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "SetMapMode", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "SetMapMode", M2Share.g_GameCommand.SETMAPMODE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SETMAPMODE.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ShoweMapMode", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ShoweMapMode", M2Share.g_GameCommand.SHOWMAPMODE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWMAPMODE.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ShoweMapMode", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ShoweMapMode", M2Share.g_GameCommand.SHOWMAPMODE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWMAPMODE.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "ClearBag", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "ClearBag", M2Share.g_GameCommand.CLEARBAG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CLEARBAG.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "ClearBag", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "ClearBag", M2Share.g_GameCommand.CLEARBAG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CLEARBAG.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "LockLogin", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "LockLogin", M2Share.g_GameCommand.LOCKLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LOCKLOGON.sCmd = LoadString;
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "LockLogin", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "LockLogin", M2Share.g_GameCommand.LOCKLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.LOCKLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = CommandConf.ReadString("Command", "GMRedMsgCmd", "");
            if (LoadString == "")
            {
                CommandConf.WriteString("Command", "GMRedMsgCmd", M2Share.g_GMRedMsgCmd);
            }
            else
            {
                M2Share.g_GMRedMsgCmd = LoadString[0];
            }
            nLoadInteger = CommandConf.ReadInteger("Permission", "GMRedMsgCmd", -1);
            if (nLoadInteger < 0)
            {
                CommandConf.WriteInteger("Permission", "GMRedMsgCmd", M2Share.g_nGMREDMSGCMD);
            }
            else
            {
                M2Share.g_nGMREDMSGCMD = nLoadInteger;
            }
        }
    }
}
