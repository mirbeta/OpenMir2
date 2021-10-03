using System.IO;
using SystemModule.Common;

namespace GameSvr.Configs
{
    public class GameCmdConfig
    {
        private readonly IniFile _conf = null;

        public GameCmdConfig()
        {
            _conf = new IniFile(Path.Combine(M2Share.sConfigPath, M2Share.sCommandFileName));
        }

        public void LoadConfig()
        {
            int nLoadInteger;
            string LoadString = _conf.ReadString("Command", "Date", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Date", M2Share.g_GameCommand.DATA.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DATA.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Date", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Date", M2Share.g_GameCommand.DATA.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DATA.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "PrvMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "PrvMsg", M2Share.g_GameCommand.PRVMSG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.PRVMSG.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "PrvMsg", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "PrvMsg", M2Share.g_GameCommand.PRVMSG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.PRVMSG.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "AllowMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AllowMsg", M2Share.g_GameCommand.ALLOWMSG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWMSG.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "AllowMsg", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "AllowMsg", M2Share.g_GameCommand.ALLOWMSG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWMSG.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "LetShout", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "LetShout", M2Share.g_GameCommand.LETSHOUT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LETSHOUT.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "LetTrade", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "LetTrade", M2Share.g_GameCommand.LETTRADE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LETTRADE.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "LetGuild", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "LetGuild", M2Share.g_GameCommand.LETGUILD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LETGUILD.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "EndGuild", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "EndGuild", M2Share.g_GameCommand.ENDGUILD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ENDGUILD.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "BanGuildChat", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "BanGuildChat", M2Share.g_GameCommand.BANGUILDCHAT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.BANGUILDCHAT.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "AuthAlly", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AuthAlly", M2Share.g_GameCommand.AUTHALLY.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.AUTHALLY.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "Auth", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Auth", M2Share.g_GameCommand.AUTH.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.AUTH.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "AuthCancel", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AuthCancel", M2Share.g_GameCommand.AUTHCANCEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.AUTHCANCEL.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "ViewDiary", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ViewDiary", M2Share.g_GameCommand.DIARY.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DIARY.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "UserMove", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "UserMove", M2Share.g_GameCommand.USERMOVE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.USERMOVE.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "Searching", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Searching", M2Share.g_GameCommand.SEARCHING.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SEARCHING.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "AllowGroupCall", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AllowGroupCall", M2Share.g_GameCommand.ALLOWGROUPCALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWGROUPCALL.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "GroupCall", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "GroupCall", M2Share.g_GameCommand.GROUPRECALLL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GROUPRECALLL.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "AllowGuildReCall", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AllowGuildReCall", M2Share.g_GameCommand.ALLOWGUILDRECALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWGUILDRECALL.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "GuildReCall", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "GuildReCall", M2Share.g_GameCommand.GUILDRECALLL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GUILDRECALLL.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "StorageUnLock", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "StorageUnLock", M2Share.g_GameCommand.UNLOCKSTORAGE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.UNLOCKSTORAGE.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "PasswordUnLock", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "PasswordUnLock", M2Share.g_GameCommand.UNLOCK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.UNLOCK.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "StorageLock", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "StorageLock", M2Share.g_GameCommand.__LOCK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.__LOCK.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "StorageSetPassword", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "StorageSetPassword", M2Share.g_GameCommand.SETPASSWORD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETPASSWORD.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "PasswordLock", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "PasswordLock", M2Share.g_GameCommand.PASSWORDLOCK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.PASSWORDLOCK.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "StorageChgPassword", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "StorageChgPassword", M2Share.g_GameCommand.CHGPASSWORD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHGPASSWORD.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "StorageClearPassword", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "StorageClearPassword", M2Share.g_GameCommand.CLRPASSWORD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CLRPASSWORD.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "StorageClearPassword", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "StorageClearPassword", M2Share.g_GameCommand.CLRPASSWORD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CLRPASSWORD.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "StorageUserClearPassword", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "StorageUserClearPassword", M2Share.g_GameCommand.UNPASSWORD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.UNPASSWORD.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "MemberFunc", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "MemberFunc", M2Share.g_GameCommand.MEMBERFUNCTION.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MEMBERFUNCTION.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "MemberFuncEx", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "MemberFuncEx", M2Share.g_GameCommand.MEMBERFUNCTIONEX.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MEMBERFUNCTIONEX.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "Dear", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Dear", M2Share.g_GameCommand.DEAR.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DEAR.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "Master", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Master", M2Share.g_GameCommand.MASTER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MASTER.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "DearRecall", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DearRecall", M2Share.g_GameCommand.DEARRECALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DEARRECALL.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "MasterRecall", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "MasterRecall", M2Share.g_GameCommand.MASTERECALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MASTERECALL.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "AllowDearRecall", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AllowDearRecall", M2Share.g_GameCommand.ALLOWDEARRCALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWDEARRCALL.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "AllowMasterRecall", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AllowMasterRecall", M2Share.g_GameCommand.ALLOWMASTERRECALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ALLOWMASTERRECALL.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "AttackMode", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AttackMode", M2Share.g_GameCommand.ATTACKMODE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ATTACKMODE.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "Rest", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Rest", M2Share.g_GameCommand.REST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.REST.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "TakeOnHorse", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "TakeOnHorse", M2Share.g_GameCommand.TAKEONHORSE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TAKEONHORSE.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "TakeOffHorse", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "TakeOffHorse", M2Share.g_GameCommand.TAKEOFHORSE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TAKEOFHORSE.sCmd = LoadString;
            }
            LoadString = _conf.ReadString("Command", "HumanLocal", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "HumanLocal", M2Share.g_GameCommand.HUMANLOCAL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.HUMANLOCAL.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "HumanLocal", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "HumanLocal", M2Share.g_GameCommand.HUMANLOCAL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.HUMANLOCAL.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Move", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Move", M2Share.g_GameCommand.MOVE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOVE.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "MoveMin", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "MoveMin", M2Share.g_GameCommand.MOVE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOVE.nPerMissionMin = nLoadInteger;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "MoveMax", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "MoveMax", M2Share.g_GameCommand.MOVE.nPerMissionMax);
            }
            else
            {
                M2Share.g_GameCommand.MOVE.nPerMissionMax = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "PositionMove", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "PositionMove", M2Share.g_GameCommand.POSITIONMOVE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.POSITIONMOVE.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "PositionMoveMin", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "PositionMoveMin", M2Share.g_GameCommand.POSITIONMOVE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.POSITIONMOVE.nPerMissionMin = nLoadInteger;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "PositionMoveMax", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "PositionMoveMax", M2Share.g_GameCommand.POSITIONMOVE.nPerMissionMax);
            }
            else
            {
                M2Share.g_GameCommand.POSITIONMOVE.nPerMissionMax = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Info", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Info", M2Share.g_GameCommand.INFO.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.INFO.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Info", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Info", M2Share.g_GameCommand.INFO.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.INFO.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "MobLevel", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "MobLevel", M2Share.g_GameCommand.MOBLEVEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOBLEVEL.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "MobLevel", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "MobLevel", M2Share.g_GameCommand.MOBLEVEL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOBLEVEL.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "MobCount", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "MobCount", M2Share.g_GameCommand.MOBCOUNT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOBCOUNT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "MobCount", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "MobCount", M2Share.g_GameCommand.MOBCOUNT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOBCOUNT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "HumanCount", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "HumanCount", M2Share.g_GameCommand.HUMANCOUNT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.HUMANCOUNT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "HumanCount", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "HumanCount", M2Share.g_GameCommand.HUMANCOUNT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.HUMANCOUNT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Map", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Map", M2Share.g_GameCommand.MAP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MAP.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Map", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Map", M2Share.g_GameCommand.MAP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MAP.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Kick", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Kick", M2Share.g_GameCommand.KICK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.KICK.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Kick", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Kick", M2Share.g_GameCommand.KICK.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.KICK.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Ting", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Ting", M2Share.g_GameCommand.TING.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TING.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Ting", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Ting", M2Share.g_GameCommand.TING.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TING.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "SuperTing", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "SuperTing", M2Share.g_GameCommand.SUPERTING.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SUPERTING.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "SuperTing", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "SuperTing", M2Share.g_GameCommand.SUPERTING.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SUPERTING.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "MapMove", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "MapMove", M2Share.g_GameCommand.MAPMOVE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MAPMOVE.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "MapMove", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "MapMove", M2Share.g_GameCommand.MAPMOVE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MAPMOVE.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Shutup", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Shutup", M2Share.g_GameCommand.SHUTUP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHUTUP.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Shutup", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Shutup", M2Share.g_GameCommand.SHUTUP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHUTUP.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReleaseShutup", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReleaseShutup", M2Share.g_GameCommand.RELEASESHUTUP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELEASESHUTUP.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReleaseShutup", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReleaseShutup", M2Share.g_GameCommand.RELEASESHUTUP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELEASESHUTUP.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ShutupList", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ShutupList", M2Share.g_GameCommand.SHUTUPLIST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHUTUPLIST.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ShutupList", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ShutupList", M2Share.g_GameCommand.SHUTUPLIST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHUTUPLIST.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "GameMaster", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "GameMaster", M2Share.g_GameCommand.GAMEMASTER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GAMEMASTER.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "GameMaster", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "GameMaster", M2Share.g_GameCommand.GAMEMASTER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.GAMEMASTER.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ObServer", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ObServer", M2Share.g_GameCommand.OBSERVER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.OBSERVER.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ObServer", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ObServer", M2Share.g_GameCommand.OBSERVER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.OBSERVER.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "SuperMan", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "SuperMan", M2Share.g_GameCommand.SUEPRMAN.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SUEPRMAN.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "SuperMan", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "SuperMan", M2Share.g_GameCommand.SUEPRMAN.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SUEPRMAN.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Level", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Level", M2Share.g_GameCommand.LEVEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LEVEL.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Level", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Level", M2Share.g_GameCommand.LEVEL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.LEVEL.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "SabukWallGold", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "SabukWallGold", M2Share.g_GameCommand.SABUKWALLGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SABUKWALLGOLD.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "SabukWallGold", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "SabukWallGold", M2Share.g_GameCommand.SABUKWALLGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SABUKWALLGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Recall", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Recall", M2Share.g_GameCommand.RECALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RECALL.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Recall", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Recall", M2Share.g_GameCommand.RECALL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RECALL.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReGoto", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReGoto", M2Share.g_GameCommand.REGOTO.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.REGOTO.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReGoto", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReGoto", M2Share.g_GameCommand.REGOTO.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.REGOTO.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Flag", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Flag", M2Share.g_GameCommand.SHOWFLAG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWFLAG.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Flag", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Flag", M2Share.g_GameCommand.SHOWFLAG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWFLAG.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ShowOpen", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ShowOpen", M2Share.g_GameCommand.SHOWOPEN.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWOPEN.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ShowOpen", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ShowOpen", M2Share.g_GameCommand.SHOWOPEN.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWOPEN.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ShowUnit", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ShowUnit", M2Share.g_GameCommand.SHOWUNIT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWUNIT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ShowUnit", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ShowUnit", M2Share.g_GameCommand.SHOWUNIT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWUNIT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Attack", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Attack", M2Share.g_GameCommand.ATTACK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ATTACK.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Attack", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Attack", M2Share.g_GameCommand.ATTACK.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ATTACK.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Mob", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Mob", M2Share.g_GameCommand.MOB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOB.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Mob", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Mob", M2Share.g_GameCommand.MOB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOB.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "MobNpc", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "MobNpc", M2Share.g_GameCommand.MOBNPC.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOBNPC.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "MobNpc", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "MobNpc", M2Share.g_GameCommand.MOBNPC.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOBNPC.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DelNpc", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DelNpc", M2Share.g_GameCommand.DELNPC.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELNPC.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DelNpc", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DelNpc", M2Share.g_GameCommand.DELNPC.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELNPC.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "NpcScript", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "NpcScript", M2Share.g_GameCommand.NPCSCRIPT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.NPCSCRIPT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "NpcScript", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "NpcScript", M2Share.g_GameCommand.NPCSCRIPT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.NPCSCRIPT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "RecallMob", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "RecallMob", M2Share.g_GameCommand.RECALLMOB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RECALLMOB.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "RecallMob", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "RecallMob", M2Share.g_GameCommand.RECALLMOB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RECALLMOB.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "LuckPoint", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "LuckPoint", M2Share.g_GameCommand.LUCKYPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LUCKYPOINT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "LuckPoint", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "LuckPoint", M2Share.g_GameCommand.LUCKYPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.LUCKYPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "LotteryTicket", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "LotteryTicket", M2Share.g_GameCommand.LOTTERYTICKET.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LOTTERYTICKET.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "LotteryTicket", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "LotteryTicket", M2Share.g_GameCommand.LOTTERYTICKET.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.LOTTERYTICKET.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReloadGuild", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReloadGuild", M2Share.g_GameCommand.RELOADGUILD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADGUILD.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReloadGuild", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReloadGuild", M2Share.g_GameCommand.RELOADGUILD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADGUILD.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReloadLineNotice", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReloadLineNotice", M2Share.g_GameCommand.RELOADLINENOTICE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADLINENOTICE.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReloadLineNotice", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReloadLineNotice", M2Share.g_GameCommand.RELOADLINENOTICE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADLINENOTICE.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReloadAbuse", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReloadAbuse", M2Share.g_GameCommand.RELOADABUSE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADABUSE.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReloadAbuse", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReloadAbuse", M2Share.g_GameCommand.RELOADABUSE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADABUSE.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "BackStep", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "BackStep", M2Share.g_GameCommand.BACKSTEP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.BACKSTEP.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "BackStep", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "BackStep", M2Share.g_GameCommand.BACKSTEP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.BACKSTEP.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Ball", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Ball", M2Share.g_GameCommand.BALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.BALL.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Ball", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Ball", M2Share.g_GameCommand.BALL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.BALL.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "FreePenalty", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "FreePenalty", M2Share.g_GameCommand.FREEPENALTY.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.FREEPENALTY.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "FreePenalty", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "FreePenalty", M2Share.g_GameCommand.FREEPENALTY.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.FREEPENALTY.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "PkPoint", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "PkPoint", M2Share.g_GameCommand.PKPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.PKPOINT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "PkPoint", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "PkPoint", M2Share.g_GameCommand.PKPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.PKPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "IncPkPoint", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "IncPkPoint", M2Share.g_GameCommand.INCPKPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.INCPKPOINT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "IncPkPoint", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "IncPkPoint", M2Share.g_GameCommand.INCPKPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.INCPKPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ChangeLuck", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ChangeLuck", M2Share.g_GameCommand.CHANGELUCK.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGELUCK.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ChangeLuck", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ChangeLuck", M2Share.g_GameCommand.CHANGELUCK.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGELUCK.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Hunger", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Hunger", M2Share.g_GameCommand.HUNGER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.HUNGER.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Hunger", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Hunger", M2Share.g_GameCommand.HUNGER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.HUNGER.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Hair", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Hair", M2Share.g_GameCommand.HAIR.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.HAIR.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Hair", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Hair", M2Share.g_GameCommand.HAIR.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.HAIR.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Training", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Training", M2Share.g_GameCommand.TRAINING.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TRAINING.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Training", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Training", M2Share.g_GameCommand.TRAINING.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TRAINING.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DeleteSkill", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DeleteSkill", M2Share.g_GameCommand.DELETESKILL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELETESKILL.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DeleteSkill", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DeleteSkill", M2Share.g_GameCommand.DELETESKILL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELETESKILL.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ChangeJob", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ChangeJob", M2Share.g_GameCommand.CHANGEJOB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEJOB.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ChangeJob", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ChangeJob", M2Share.g_GameCommand.CHANGEJOB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEJOB.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ChangeGender", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ChangeGender", M2Share.g_GameCommand.CHANGEGENDER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEGENDER.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ChangeGender", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ChangeGender", M2Share.g_GameCommand.CHANGEGENDER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEGENDER.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "NameColor", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "NameColor", M2Share.g_GameCommand.NAMECOLOR.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.NAMECOLOR.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "NameColor", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "NameColor", M2Share.g_GameCommand.NAMECOLOR.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.NAMECOLOR.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Mission", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Mission", M2Share.g_GameCommand.MISSION.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MISSION.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Mission", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Mission", M2Share.g_GameCommand.MISSION.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MISSION.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "MobPlace", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "MobPlace", M2Share.g_GameCommand.MOBPLACE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MOBPLACE.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "MobPlace", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "MobPlace", M2Share.g_GameCommand.MOBPLACE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MOBPLACE.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Transparecy", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Transparecy", M2Share.g_GameCommand.TRANSPARECY.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TRANSPARECY.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Transparecy", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Transparecy", M2Share.g_GameCommand.TRANSPARECY.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TRANSPARECY.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DeleteItem", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DeleteItem", M2Share.g_GameCommand.DELETEITEM.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELETEITEM.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DeleteItem", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DeleteItem", M2Share.g_GameCommand.DELETEITEM.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELETEITEM.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Level0", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Level0", M2Share.g_GameCommand.LEVEL0.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LEVEL0.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Level0", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Level0", M2Share.g_GameCommand.LEVEL0.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.LEVEL0.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ClearMission", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ClearMission", M2Share.g_GameCommand.CLEARMISSION.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CLEARMISSION.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ClearMission", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ClearMission", M2Share.g_GameCommand.CLEARMISSION.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CLEARMISSION.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "SetFlag", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "SetFlag", M2Share.g_GameCommand.SETFLAG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETFLAG.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "SetFlag", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "SetFlag", M2Share.g_GameCommand.SETFLAG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SETFLAG.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "SetOpen", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "SetOpen", M2Share.g_GameCommand.SETOPEN.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETOPEN.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "SetOpen", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "SetOpen", M2Share.g_GameCommand.SETOPEN.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SETOPEN.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "SetUnit", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "SetUnit", M2Share.g_GameCommand.SETUNIT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETUNIT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "SetUnit", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "SetUnit", M2Share.g_GameCommand.SETUNIT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SETUNIT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReConnection", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReConnection", M2Share.g_GameCommand.RECONNECTION.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RECONNECTION.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReConnection", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReConnection", M2Share.g_GameCommand.RECONNECTION.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RECONNECTION.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DisableFilter", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DisableFilter", M2Share.g_GameCommand.DISABLEFILTER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DISABLEFILTER.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DisableFilter", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DisableFilter", M2Share.g_GameCommand.DISABLEFILTER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DISABLEFILTER.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ChangeUserFull", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ChangeUserFull", M2Share.g_GameCommand.CHGUSERFULL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHGUSERFULL.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ChangeUserFull", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ChangeUserFull", M2Share.g_GameCommand.CHGUSERFULL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHGUSERFULL.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ChangeZenFastStep", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ChangeZenFastStep", M2Share.g_GameCommand.CHGZENFASTSTEP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHGZENFASTSTEP.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ChangeZenFastStep", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ChangeZenFastStep", M2Share.g_GameCommand.CHGZENFASTSTEP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHGZENFASTSTEP.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ContestPoint", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ContestPoint", M2Share.g_GameCommand.CONTESTPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CONTESTPOINT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ContestPoint", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ContestPoint", M2Share.g_GameCommand.CONTESTPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CONTESTPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "StartContest", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "StartContest", M2Share.g_GameCommand.STARTCONTEST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.STARTCONTEST.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "StartContest", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "StartContest", M2Share.g_GameCommand.STARTCONTEST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.STARTCONTEST.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "EndContest", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "EndContest", M2Share.g_GameCommand.ENDCONTEST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ENDCONTEST.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "EndContest", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "EndContest", M2Share.g_GameCommand.ENDCONTEST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ENDCONTEST.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Announcement", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Announcement", M2Share.g_GameCommand.ANNOUNCEMENT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ANNOUNCEMENT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Announcement", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Announcement", M2Share.g_GameCommand.ANNOUNCEMENT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ANNOUNCEMENT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "OXQuizRoom", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "OXQuizRoom", M2Share.g_GameCommand.OXQUIZROOM.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.OXQUIZROOM.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "OXQuizRoom", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "OXQuizRoom", M2Share.g_GameCommand.OXQUIZROOM.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.OXQUIZROOM.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Gsa", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Gsa", M2Share.g_GameCommand.GSA.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GSA.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Gsa", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Gsa", M2Share.g_GameCommand.GSA.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.GSA.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ChangeItemName", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ChangeItemName", M2Share.g_GameCommand.CHANGEITEMNAME.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEITEMNAME.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ChangeItemName", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ChangeItemName", M2Share.g_GameCommand.CHANGEITEMNAME.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEITEMNAME.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DisableSendMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DisableSendMsg", M2Share.g_GameCommand.DISABLESENDMSG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DISABLESENDMSG.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DisableSendMsg", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DisableSendMsg", M2Share.g_GameCommand.DISABLESENDMSG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DISABLESENDMSG.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "EnableSendMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "EnableSendMsg", M2Share.g_GameCommand.ENABLESENDMSG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ENABLESENDMSG.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "EnableSendMsg", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "EnableSendMsg", M2Share.g_GameCommand.ENABLESENDMSG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ENABLESENDMSG.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DisableSendMsgList", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DisableSendMsgList", M2Share.g_GameCommand.DISABLESENDMSGLIST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DISABLESENDMSGLIST.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DisableSendMsgList", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DisableSendMsgList", M2Share.g_GameCommand.DISABLESENDMSGLIST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DISABLESENDMSGLIST.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Kill", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Kill", M2Share.g_GameCommand.KILL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.KILL.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Kill", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Kill", M2Share.g_GameCommand.KILL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.KILL.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Make", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Make", M2Share.g_GameCommand.MAKE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MAKE.sCmd = LoadString;
                M2Share.CommandSystem.RegisterCommand("Make", LoadString);
            }
            nLoadInteger = _conf.ReadInteger("Permission", "MakeMin", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "MakeMin", M2Share.g_GameCommand.MAKE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MAKE.nPerMissionMin = nLoadInteger;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "MakeMax", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "MakeMax", M2Share.g_GameCommand.MAKE.nPerMissionMax);
            }
            else
            {
                M2Share.g_GameCommand.MAKE.nPerMissionMax = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "SuperMake", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "SuperMake", M2Share.g_GameCommand.SMAKE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SMAKE.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "SuperMake", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "SuperMake", M2Share.g_GameCommand.SMAKE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SMAKE.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "BonusPoint", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "BonusPoint", M2Share.g_GameCommand.BONUSPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.BONUSPOINT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "BonusPoint", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "BonusPoint", M2Share.g_GameCommand.BONUSPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.BONUSPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DelBonuPoint", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DelBonuPoint", M2Share.g_GameCommand.DELBONUSPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELBONUSPOINT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DelBonuPoint", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DelBonuPoint", M2Share.g_GameCommand.DELBONUSPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELBONUSPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "RestBonuPoint", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "RestBonuPoint", M2Share.g_GameCommand.RESTBONUSPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RESTBONUSPOINT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "RestBonuPoint", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "RestBonuPoint", M2Share.g_GameCommand.RESTBONUSPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RESTBONUSPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "FireBurn", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "FireBurn", M2Share.g_GameCommand.FIREBURN.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.FIREBURN.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "FireBurn", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "FireBurn", M2Share.g_GameCommand.FIREBURN.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.FIREBURN.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "TestStatus", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "TestStatus", M2Share.g_GameCommand.TESTSTATUS.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TESTSTATUS.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "TestStatus", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "TestStatus", M2Share.g_GameCommand.TESTSTATUS.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TESTSTATUS.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DelGold", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DelGold", M2Share.g_GameCommand.DELGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELGOLD.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DelGold", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DelGold", M2Share.g_GameCommand.DELGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "AddGold", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AddGold", M2Share.g_GameCommand.ADDGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADDGOLD.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "AddGold", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "AddGold", M2Share.g_GameCommand.ADDGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADDGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DelGameGold", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DelGameGold", M2Share.g_GameCommand.DELGAMEGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELGAMEGOLD.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DelGameGold", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DelGameGold", M2Share.g_GameCommand.DELGAMEGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELGAMEGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "AddGamePoint", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AddGamePoint", M2Share.g_GameCommand.ADDGAMEGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADDGAMEGOLD.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "AddGameGold", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "AddGameGold", M2Share.g_GameCommand.ADDGAMEGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADDGAMEGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "GameGold", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "GameGold", M2Share.g_GameCommand.GAMEGOLD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GAMEGOLD.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "GameGold", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "GameGold", M2Share.g_GameCommand.GAMEGOLD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.GAMEGOLD.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "GamePoint", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "GamePoint", M2Share.g_GameCommand.GAMEPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.GAMEPOINT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "GamePoint", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "GamePoint", M2Share.g_GameCommand.GAMEPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.GAMEPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "CreditPoint", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "CreditPoint", M2Share.g_GameCommand.CREDITPOINT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CREDITPOINT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "CreditPoint", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "CreditPoint", M2Share.g_GameCommand.CREDITPOINT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CREDITPOINT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "TestGoldChange", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "TestGoldChange", M2Share.g_GameCommand.TESTGOLDCHANGE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TESTGOLDCHANGE.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "TestGoldChange", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "TestGoldChange", M2Share.g_GameCommand.TESTGOLDCHANGE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TESTGOLDCHANGE.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "RefineWeapon", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "RefineWeapon", M2Share.g_GameCommand.REFINEWEAPON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.REFINEWEAPON.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "RefineWeapon", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "RefineWeapon", M2Share.g_GameCommand.REFINEWEAPON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.REFINEWEAPON.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReloadAdmin", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReloadAdmin", M2Share.g_GameCommand.RELOADADMIN.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADADMIN.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReloadAdmin", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReloadAdmin", M2Share.g_GameCommand.RELOADADMIN.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADADMIN.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReloadNpc", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReloadNpc", M2Share.g_GameCommand.RELOADNPC.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADNPC.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReloadNpc", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReloadNpc", M2Share.g_GameCommand.RELOADNPC.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADNPC.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReloadManage", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReloadManage", M2Share.g_GameCommand.RELOADMANAGE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMANAGE.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReloadManage", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReloadManage", M2Share.g_GameCommand.RELOADMANAGE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMANAGE.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReloadRobotManage", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReloadRobotManage", M2Share.g_GameCommand.RELOADROBOTMANAGE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADROBOTMANAGE.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReloadRobotManage", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReloadRobotManage", M2Share.g_GameCommand.RELOADROBOTMANAGE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADROBOTMANAGE.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReloadRobot", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReloadRobot", M2Share.g_GameCommand.RELOADROBOT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADROBOT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReloadRobot", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReloadRobot", M2Share.g_GameCommand.RELOADROBOT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADROBOT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReloadMonitems", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReloadMonitems", M2Share.g_GameCommand.RELOADMONITEMS.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMONITEMS.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReloadMonitems", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReloadMonitems", M2Share.g_GameCommand.RELOADMONITEMS.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMONITEMS.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReloadDiary", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReloadDiary", M2Share.g_GameCommand.RELOADDIARY.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADDIARY.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReloadDiary", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReloadDiary", M2Share.g_GameCommand.RELOADDIARY.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADDIARY.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReloadItemDB", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReloadItemDB", M2Share.g_GameCommand.RELOADITEMDB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADITEMDB.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReloadItemDB", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReloadItemDB", M2Share.g_GameCommand.RELOADITEMDB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADITEMDB.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReloadMagicDB", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReloadMagicDB", M2Share.g_GameCommand.RELOADMAGICDB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMAGICDB.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReloadMagicDB", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReloadMagicDB", M2Share.g_GameCommand.RELOADMAGICDB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMAGICDB.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReloadMonsterDB", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReloadMonsterDB", M2Share.g_GameCommand.RELOADMONSTERDB.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMONSTERDB.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReloadMonsterDB", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReloadMonsterDB", M2Share.g_GameCommand.RELOADMONSTERDB.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADMONSTERDB.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReAlive", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReAlive", M2Share.g_GameCommand.REALIVE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.REALIVE.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReAlive", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReAlive", M2Share.g_GameCommand.REALIVE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.REALIVE.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "AdjuestTLevel", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AdjuestTLevel", M2Share.g_GameCommand.ADJUESTLEVEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTLEVEL.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "AdjuestTLevel", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "AdjuestTLevel", M2Share.g_GameCommand.ADJUESTLEVEL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTLEVEL.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "AdjuestExp", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AdjuestExp", M2Share.g_GameCommand.ADJUESTEXP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTEXP.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "AdjuestExp", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "AdjuestExp", M2Share.g_GameCommand.ADJUESTEXP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTEXP.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "AddGuild", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AddGuild", M2Share.g_GameCommand.ADDGUILD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADDGUILD.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "AddGuild", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "AddGuild", M2Share.g_GameCommand.ADDGUILD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADDGUILD.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DelGuild", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DelGuild", M2Share.g_GameCommand.DELGUILD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELGUILD.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DelGuild", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DelGuild", M2Share.g_GameCommand.DELGUILD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELGUILD.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ChangeSabukLord", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ChangeSabukLord", M2Share.g_GameCommand.CHANGESABUKLORD.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGESABUKLORD.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ChangeSabukLord", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ChangeSabukLord", M2Share.g_GameCommand.CHANGESABUKLORD.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGESABUKLORD.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ForcedWallConQuestWar", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ForcedWallConQuestWar", M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ForcedWallConQuestWar", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ForcedWallConQuestWar", M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.FORCEDWALLCONQUESTWAR.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "AddToItemEvent", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AddToItemEvent", M2Share.g_GameCommand.ADDTOITEMEVENT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADDTOITEMEVENT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "AddToItemEvent", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "AddToItemEvent", M2Share.g_GameCommand.ADDTOITEMEVENT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADDTOITEMEVENT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "AddToItemEventAspieces", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AddToItemEventAspieces", M2Share.g_GameCommand.ADDTOITEMEVENTASPIECES.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADDTOITEMEVENTASPIECES.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "AddToItemEventAspieces", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "AddToItemEventAspieces", M2Share.g_GameCommand.ADDTOITEMEVENTASPIECES.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADDTOITEMEVENTASPIECES.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ItemEventList", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ItemEventList", M2Share.g_GameCommand.ITEMEVENTLIST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ITEMEVENTLIST.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ItemEventList", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ItemEventList", M2Share.g_GameCommand.ITEMEVENTLIST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ITEMEVENTLIST.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "StartIngGiftNO", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "StartIngGiftNO", M2Share.g_GameCommand.STARTINGGIFTNO.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.STARTINGGIFTNO.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "StartIngGiftNO", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "StartIngGiftNO", M2Share.g_GameCommand.STARTINGGIFTNO.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.STARTINGGIFTNO.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DeleteAllItemEvent", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DeleteAllItemEvent", M2Share.g_GameCommand.DELETEALLITEMEVENT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELETEALLITEMEVENT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DeleteAllItemEvent", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DeleteAllItemEvent", M2Share.g_GameCommand.DELETEALLITEMEVENT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELETEALLITEMEVENT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "StartItemEvent", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "StartItemEvent", M2Share.g_GameCommand.STARTITEMEVENT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.STARTITEMEVENT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "StartItemEvent", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "StartItemEvent", M2Share.g_GameCommand.STARTITEMEVENT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.STARTITEMEVENT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ItemEventTerm", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ItemEventTerm", M2Share.g_GameCommand.ITEMEVENTTERM.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ITEMEVENTTERM.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ItemEventTerm", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ItemEventTerm", M2Share.g_GameCommand.ITEMEVENTTERM.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ITEMEVENTTERM.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "AdjuestTestLevel", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "AdjuestTestLevel", M2Share.g_GameCommand.ADJUESTTESTLEVEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTTESTLEVEL.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "AdjuestTestLevel", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "AdjuestTestLevel", M2Share.g_GameCommand.ADJUESTTESTLEVEL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.ADJUESTTESTLEVEL.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "OpTraining", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "OpTraining", M2Share.g_GameCommand.TRAININGSKILL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TRAININGSKILL.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "OpTraining", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "OpTraining", M2Share.g_GameCommand.TRAININGSKILL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TRAININGSKILL.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "OpDeleteSkill", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "OpDeleteSkill", M2Share.g_GameCommand.OPDELETESKILL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.OPDELETESKILL.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "OpDeleteSkill", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "OpDeleteSkill", M2Share.g_GameCommand.OPDELETESKILL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.OPDELETESKILL.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ChangeWeaponDura", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ChangeWeaponDura", M2Share.g_GameCommand.CHANGEWEAPONDURA.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEWEAPONDURA.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ChangeWeaponDura", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ChangeWeaponDura", M2Share.g_GameCommand.CHANGEWEAPONDURA.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEWEAPONDURA.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReloadGuildAll", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReloadGuildAll", M2Share.g_GameCommand.RELOADGUILDALL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RELOADGUILDALL.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReloadGuildAll", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReloadGuildAll", M2Share.g_GameCommand.RELOADGUILDALL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RELOADGUILDALL.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Who", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Who", M2Share.g_GameCommand.WHO.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.WHO.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Who", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Who", M2Share.g_GameCommand.WHO.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.WHO.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "Total", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "Total", M2Share.g_GameCommand.TOTAL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TOTAL.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "Total", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "Total", M2Share.g_GameCommand.TOTAL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TOTAL.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "TestGa", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "TestGa", M2Share.g_GameCommand.TESTGA.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.TESTGA.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "TestGa", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "TestGa", M2Share.g_GameCommand.TESTGA.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.TESTGA.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "MapInfo", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "MapInfo", M2Share.g_GameCommand.MAPINFO.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.MAPINFO.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "MapInfo", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "MapInfo", M2Share.g_GameCommand.MAPINFO.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.MAPINFO.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "SbkDoor", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "SbkDoor", M2Share.g_GameCommand.SBKDOOR.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SBKDOOR.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "SbkDoor", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "SbkDoor", M2Share.g_GameCommand.SBKDOOR.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SBKDOOR.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ChangeDearName", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ChangeDearName", M2Share.g_GameCommand.CHANGEDEARNAME.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEDEARNAME.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ChangeDearName", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ChangeDearName", M2Share.g_GameCommand.CHANGEDEARNAME.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEDEARNAME.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ChangeMasterName", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ChangeMasterrName", M2Share.g_GameCommand.CHANGEMASTERNAME.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEMASTERNAME.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ChangeMasterName", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ChangeMasterName", M2Share.g_GameCommand.CHANGEMASTERNAME.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CHANGEMASTERNAME.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "StartQuest", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "StartQuest", M2Share.g_GameCommand.STARTQUEST.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.STARTQUEST.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "StartQuest", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "StartQuest", M2Share.g_GameCommand.STARTQUEST.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.STARTQUEST.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "SetPermission", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "SetPermission", M2Share.g_GameCommand.SETPERMISSION.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETPERMISSION.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "SetPermission", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "SetPermission", M2Share.g_GameCommand.SETPERMISSION.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SETPERMISSION.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ClearMon", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ClearMon", M2Share.g_GameCommand.CLEARMON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CLEARMON.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ClearMon", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ClearMon", M2Share.g_GameCommand.CLEARMON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CLEARMON.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ReNewLevel", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ReNewLevel", M2Share.g_GameCommand.RENEWLEVEL.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.RENEWLEVEL.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ReNewLevel", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ReNewLevel", M2Share.g_GameCommand.RENEWLEVEL.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.RENEWLEVEL.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DenyIPaddrLogon", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DenyIPaddrLogon", M2Share.g_GameCommand.DENYIPLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DENYIPLOGON.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DenyIPaddrLogon", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DenyIPaddrLogon", M2Share.g_GameCommand.DENYIPLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DENYIPLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DenyAccountLogon", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DenyAccountLogon", M2Share.g_GameCommand.DENYACCOUNTLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DENYACCOUNTLOGON.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DenyAccountLogon", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DenyAccountLogon", M2Share.g_GameCommand.DENYACCOUNTLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DENYACCOUNTLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DenyCharNameLogon", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DenyCharNameLogon", M2Share.g_GameCommand.DENYCHARNAMELOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DENYCHARNAMELOGON.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DenyCharNameLogon", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DenyCharNameLogon", M2Share.g_GameCommand.DENYCHARNAMELOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DENYCHARNAMELOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DelDenyIPLogon", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DelDenyIPLogon", M2Share.g_GameCommand.DELDENYIPLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYIPLOGON.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DelDenyIPLogon", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DelDenyIPLogon", M2Share.g_GameCommand.DELDENYIPLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYIPLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DelDenyAccountLogon", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DelDenyAccountLogon", M2Share.g_GameCommand.DELDENYACCOUNTLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYACCOUNTLOGON.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DelDenyAccountLogon", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DelDenyAccountLogon", M2Share.g_GameCommand.DELDENYACCOUNTLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYACCOUNTLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "DelDenyCharNameLogon", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "DelDenyCharNameLogon", M2Share.g_GameCommand.DELDENYCHARNAMELOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYCHARNAMELOGON.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "DelDenyCharNameLogon", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "DelDenyCharNameLogon", M2Share.g_GameCommand.DELDENYCHARNAMELOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.DELDENYCHARNAMELOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ShowDenyIPLogon", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ShowDenyIPLogon", M2Share.g_GameCommand.SHOWDENYIPLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYIPLOGON.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ShowDenyIPLogon", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ShowDenyIPLogon", M2Share.g_GameCommand.SHOWDENYIPLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYIPLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ShowDenyAccountLogon", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ShowDenyAccountLogon", M2Share.g_GameCommand.SHOWDENYACCOUNTLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYACCOUNTLOGON.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ShowDenyAccountLogon", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ShowDenyAccountLogon", M2Share.g_GameCommand.SHOWDENYACCOUNTLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYACCOUNTLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ShowDenyCharNameLogon", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ShowDenyCharNameLogon", M2Share.g_GameCommand.SHOWDENYCHARNAMELOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYCHARNAMELOGON.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ShowDenyCharNameLogon", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ShowDenyCharNameLogon", M2Share.g_GameCommand.SHOWDENYCHARNAMELOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWDENYCHARNAMELOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ViewWhisper", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ViewWhisper", M2Share.g_GameCommand.VIEWWHISPER.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.VIEWWHISPER.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ViewWhisper", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ViewWhisper", M2Share.g_GameCommand.VIEWWHISPER.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.VIEWWHISPER.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "SpiritStart", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "SpiritStart", M2Share.g_GameCommand.SPIRIT.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SPIRIT.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "SpiritStart", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "SpiritStart", M2Share.g_GameCommand.SPIRIT.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SPIRIT.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "SpiritStop", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "SpiritStop", M2Share.g_GameCommand.SPIRITSTOP.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SPIRITSTOP.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "SpiritStop", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "SpiritStop", M2Share.g_GameCommand.SPIRITSTOP.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SPIRITSTOP.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "SetMapMode", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "SetMapMode", M2Share.g_GameCommand.SETMAPMODE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SETMAPMODE.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "SetMapMode", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "SetMapMode", M2Share.g_GameCommand.SETMAPMODE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SETMAPMODE.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ShoweMapMode", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ShoweMapMode", M2Share.g_GameCommand.SHOWMAPMODE.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.SHOWMAPMODE.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ShoweMapMode", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ShoweMapMode", M2Share.g_GameCommand.SHOWMAPMODE.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.SHOWMAPMODE.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "ClearBag", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "ClearBag", M2Share.g_GameCommand.CLEARBAG.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.CLEARBAG.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "ClearBag", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "ClearBag", M2Share.g_GameCommand.CLEARBAG.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.CLEARBAG.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "LockLogin", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "LockLogin", M2Share.g_GameCommand.LOCKLOGON.sCmd);
            }
            else
            {
                M2Share.g_GameCommand.LOCKLOGON.sCmd = LoadString;
            }
            nLoadInteger = _conf.ReadInteger("Permission", "LockLogin", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "LockLogin", M2Share.g_GameCommand.LOCKLOGON.nPerMissionMin);
            }
            else
            {
                M2Share.g_GameCommand.LOCKLOGON.nPerMissionMin = nLoadInteger;
            }
            LoadString = _conf.ReadString("Command", "GMRedMsgCmd", "");
            if (LoadString == "")
            {
                _conf.WriteString("Command", "GMRedMsgCmd", M2Share.g_GMRedMsgCmd);
            }
            else
            {
                M2Share.g_GMRedMsgCmd = LoadString[0];
            }
            nLoadInteger = _conf.ReadInteger("Permission", "GMRedMsgCmd", -1);
            if (nLoadInteger < 0)
            {
                _conf.WriteInteger("Permission", "GMRedMsgCmd", M2Share.g_nGMREDMSGCMD);
            }
            else
            {
                M2Share.g_nGMREDMSGCMD = nLoadInteger;
            }
        }
    }
}
