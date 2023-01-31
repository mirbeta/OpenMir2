using GameSvr.GameCommand;
using SystemModule.Common;

namespace GameSvr.Conf
{
    public class GameCmdConf : IniFile
    {
        public GameCmdConf(string fileName) : base(fileName)
        {
            Load();
        }

        /// <summary>
        /// 读取自定义命令配置
        /// </summary>
        public void LoadConfig()
        {
            var gameCommands = CommandMgr.GameCommands;
            int nLoadInteger;
            string LoadString = ReadString("Command", "Date", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Date", gameCommands.Data.CmdName);
            }
            else
            {
                gameCommands.Data.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Date", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Date", gameCommands.Data.PerMissionMin);
            }
            else
            {
                gameCommands.Data.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "PrvMsg", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "PrvMsg", gameCommands.Prvmsg.CmdName);
            }
            else
            {
                gameCommands.Prvmsg.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "PrvMsg", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "PrvMsg", gameCommands.Prvmsg.PerMissionMin);
            }
            else
            {
                gameCommands.Prvmsg.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AllowMsg", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AllowMsg", gameCommands.AllowMsg.CmdName);
            }
            else
            {
                gameCommands.AllowMsg.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AllowMsg", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AllowMsg", gameCommands.AllowMsg.PerMissionMin);
            }
            else
            {
                gameCommands.AllowMsg.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "LetShout", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "LetShout", gameCommands.Letshout.CmdName);
            }
            else
            {
                gameCommands.Letshout.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "LetTrade", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "LetTrade", gameCommands.LetTrade.CmdName);
            }
            else
            {
                gameCommands.LetTrade.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "LetGuild", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "LetGuild", gameCommands.Letguild.CmdName);
            }
            else
            {
                gameCommands.Letguild.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "EndGuild", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "EndGuild", gameCommands.Endguild.CmdName);
            }
            else
            {
                gameCommands.Endguild.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "BanGuildChat", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "BanGuildChat", gameCommands.BanGuildChat.CmdName);
            }
            else
            {
                gameCommands.BanGuildChat.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "AuthAlly", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AuthAlly", gameCommands.Authally.CmdName);
            }
            else
            {
                gameCommands.Authally.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "Auth", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Auth", gameCommands.Auth.CmdName);
            }
            else
            {
                gameCommands.Auth.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "AuthCancel", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AuthCancel", gameCommands.AuthCancel.CmdName);
            }
            else
            {
                gameCommands.AuthCancel.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "ViewDiary", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ViewDiary", gameCommands.Diary.CmdName);
            }
            else
            {
                gameCommands.Diary.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "UserMove", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "UserMove", gameCommands.UserMove.CmdName);
            }
            else
            {
                gameCommands.UserMove.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "Searching", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Searching", gameCommands.Searching.CmdName);
            }
            else
            {
                gameCommands.Searching.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "AllowGroupCall", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AllowGroupCall", gameCommands.AllowGroupCall.CmdName);
            }
            else
            {
                gameCommands.AllowGroupCall.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "GroupCall", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "GroupCall", gameCommands.GroupRecalll.CmdName);
            }
            else
            {
                gameCommands.GroupRecalll.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "AllowGuildReCall", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AllowGuildReCall", gameCommands.AllowGuildRecall.CmdName);
            }
            else
            {
                gameCommands.AllowGuildRecall.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "GuildReCall", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "GuildReCall", gameCommands.GuildRecalll.CmdName);
            }
            else
            {
                gameCommands.GuildRecalll.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "StorageUnLock", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "StorageUnLock", gameCommands.UnlockStorage.CmdName);
            }
            else
            {
                gameCommands.UnlockStorage.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "PasswordUnLock", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "PasswordUnLock", gameCommands.Unlock.CmdName);
            }
            else
            {
                gameCommands.Unlock.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "StorageLock", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "StorageLock", gameCommands.Lock.CmdName);
            }
            else
            {
                gameCommands.Lock.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "StorageSetPassword", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "StorageSetPassword", gameCommands.SetPassword.CmdName);
            }
            else
            {
                gameCommands.SetPassword.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "PasswordLock", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "PasswordLock", gameCommands.PasswordLock.CmdName);
            }
            else
            {
                gameCommands.PasswordLock.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "StorageChgPassword", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "StorageChgPassword", gameCommands.ChgPassword.CmdName);
            }
            else
            {
                gameCommands.ChgPassword.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "StorageClearPassword", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "StorageClearPassword", gameCommands.ClrPassword.CmdName);
            }
            else
            {
                gameCommands.ClrPassword.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "StorageClearPassword", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "StorageClearPassword", gameCommands.ClrPassword.PerMissionMin);
            }
            else
            {
                gameCommands.ClrPassword.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "StorageUserClearPassword", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "StorageUserClearPassword", gameCommands.UnPassword.CmdName);
            }
            else
            {
                gameCommands.UnPassword.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "MemberFunc", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "MemberFunc", gameCommands.MemberFunction.CmdName);
            }
            else
            {
                gameCommands.MemberFunction.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "MemberFuncEx", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "MemberFuncEx", gameCommands.MemberFunctioneX.CmdName);
            }
            else
            {
                gameCommands.MemberFunctioneX.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "Dear", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Dear", gameCommands.Dear.CmdName);
            }
            else
            {
                gameCommands.Dear.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "Master", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Master", gameCommands.Master.CmdName);
            }
            else
            {
                gameCommands.Master.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "DearRecall", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DearRecall", gameCommands.DearRecall.CmdName);
            }
            else
            {
                gameCommands.DearRecall.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "MasterRecall", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "MasterRecall", gameCommands.MasteRecall.CmdName);
            }
            else
            {
                gameCommands.MasteRecall.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "AllowDearRecall", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AllowDearRecall", gameCommands.AllowDearRcall.CmdName);
            }
            else
            {
                gameCommands.AllowDearRcall.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "AllowMasterRecall", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AllowMasterRecall", gameCommands.AllowMasterRecall.CmdName);
            }
            else
            {
                gameCommands.AllowMasterRecall.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "AttackMode", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AttackMode", gameCommands.AttackMode.CmdName);
            }
            else
            {
                gameCommands.AttackMode.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "Rest", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Rest", gameCommands.Rest.CmdName);
            }
            else
            {
                gameCommands.Rest.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "TakeOnHorse", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "TakeOnHorse", gameCommands.TakeonHorse.CmdName);
            }
            else
            {
                gameCommands.TakeonHorse.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "TakeOffHorse", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "TakeOffHorse", gameCommands.TakeofHorse.CmdName);
            }
            else
            {
                gameCommands.TakeofHorse.CmdName = LoadString;
            }
            LoadString = ReadString("Command", "HumanLocal", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "HumanLocal", gameCommands.HumanLocal.CmdName);
            }
            else
            {
                gameCommands.HumanLocal.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "HumanLocal", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "HumanLocal", gameCommands.HumanLocal.PerMissionMin);
            }
            else
            {
                gameCommands.HumanLocal.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Move", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Move", gameCommands.Move.CmdName);
            }
            else
            {
                gameCommands.Move.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "MoveMin", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MoveMin", gameCommands.Move.PerMissionMin);
            }
            else
            {
                gameCommands.Move.PerMissionMin = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Permission", "MoveMax", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MoveMax", gameCommands.Move.PerMissionMax);
            }
            else
            {
                gameCommands.Move.PerMissionMax = nLoadInteger;
            }
            LoadString = ReadString("Command", "PositionMove", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "PositionMove", gameCommands.PositionMove.CmdName);
            }
            else
            {
                gameCommands.PositionMove.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "PositionMoveMin", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "PositionMoveMin", gameCommands.PositionMove.PerMissionMin);
            }
            else
            {
                gameCommands.PositionMove.PerMissionMin = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Permission", "PositionMoveMax", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "PositionMoveMax", gameCommands.PositionMove.PerMissionMax);
            }
            else
            {
                gameCommands.PositionMove.PerMissionMax = nLoadInteger;
            }
            LoadString = ReadString("Command", "Info", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Info", gameCommands.Info.CmdName);
            }
            else
            {
                gameCommands.Info.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Info", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Info", gameCommands.Info.PerMissionMin);
            }
            else
            {
                gameCommands.Info.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "MobLevel", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "MobLevel", gameCommands.MobLevel.CmdName);
            }
            else
            {
                gameCommands.MobLevel.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "MobLevel", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MobLevel", gameCommands.MobLevel.PerMissionMin);
            }
            else
            {
                gameCommands.MobLevel.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "MobCount", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "MobCount", gameCommands.MobCount.CmdName);
            }
            else
            {
                gameCommands.MobCount.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "MobCount", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MobCount", gameCommands.MobCount.PerMissionMin);
            }
            else
            {
                gameCommands.MobCount.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "HumanCount", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "HumanCount", gameCommands.HumanCount.CmdName);
            }
            else
            {
                gameCommands.HumanCount.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "HumanCount", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "HumanCount", gameCommands.HumanCount.PerMissionMin);
            }
            else
            {
                gameCommands.HumanCount.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Map", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Map", gameCommands.Map.CmdName);
            }
            else
            {
                gameCommands.Map.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Map", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Map", gameCommands.Map.PerMissionMin);
            }
            else
            {
                gameCommands.Map.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Kick", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Kick", gameCommands.Kick.CmdName);
            }
            else
            {
                gameCommands.Kick.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Kick", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Kick", gameCommands.Kick.PerMissionMin);
            }
            else
            {
                gameCommands.Kick.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Ting", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Ting", gameCommands.Ting.CmdName);
            }
            else
            {
                gameCommands.Ting.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Ting", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Ting", gameCommands.Ting.PerMissionMin);
            }
            else
            {
                gameCommands.Ting.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SuperTing", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "SuperTing", gameCommands.Superting.CmdName);
            }
            else
            {
                gameCommands.Superting.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SuperTing", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SuperTing", gameCommands.Superting.PerMissionMin);
            }
            else
            {
                gameCommands.Superting.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "MapMove", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "MapMove", gameCommands.MapMove.CmdName);
            }
            else
            {
                gameCommands.MapMove.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "MapMove", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MapMove", gameCommands.MapMove.PerMissionMin);
            }
            else
            {
                gameCommands.MapMove.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Shutup", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Shutup", gameCommands.ShutUp.CmdName);
            }
            else
            {
                gameCommands.ShutUp.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Shutup", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Shutup", gameCommands.ShutUp.PerMissionMin);
            }
            else
            {
                gameCommands.ShutUp.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReleaseShutup", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReleaseShutup", gameCommands.ReleaseShutup.CmdName);
            }
            else
            {
                gameCommands.ReleaseShutup.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReleaseShutup", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReleaseShutup", gameCommands.ReleaseShutup.PerMissionMin);
            }
            else
            {
                gameCommands.ReleaseShutup.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ShutupList", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ShutupList", gameCommands.ShutupList.CmdName);
            }
            else
            {
                gameCommands.ShutupList.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ShutupList", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ShutupList", gameCommands.ShutupList.PerMissionMin);
            }
            else
            {
                gameCommands.ShutupList.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "GameMaster", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "GameMaster", gameCommands.GameMaster.CmdName);
            }
            else
            {
                gameCommands.GameMaster.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "GameMaster", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "GameMaster", gameCommands.GameMaster.PerMissionMin);
            }
            else
            {
                gameCommands.GameMaster.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ObServer", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ObServer", gameCommands.ObServer.CmdName);
            }
            else
            {
                gameCommands.ObServer.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ObServer", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ObServer", gameCommands.ObServer.PerMissionMin);
            }
            else
            {
                gameCommands.ObServer.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SuperMan", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "SuperMan", gameCommands.SueprMan.CmdName);
            }
            else
            {
                gameCommands.SueprMan.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SuperMan", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SuperMan", gameCommands.SueprMan.PerMissionMin);
            }
            else
            {
                gameCommands.SueprMan.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Level", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Level", gameCommands.Level.CmdName);
            }
            else
            {
                gameCommands.Level.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Level", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Level", gameCommands.Level.PerMissionMin);
            }
            else
            {
                gameCommands.Level.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SabukWallGold", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "SabukWallGold", gameCommands.SabukwallGold.CmdName);
            }
            else
            {
                gameCommands.SabukwallGold.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SabukWallGold", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SabukWallGold", gameCommands.SabukwallGold.PerMissionMin);
            }
            else
            {
                gameCommands.SabukwallGold.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Recall", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Recall", gameCommands.Recall.CmdName);
            }
            else
            {
                gameCommands.Recall.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Recall", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Recall", gameCommands.Recall.PerMissionMin);
            }
            else
            {
                gameCommands.Recall.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReGoto", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReGoto", gameCommands.Regoto.CmdName);
            }
            else
            {
                gameCommands.Regoto.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReGoto", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReGoto", gameCommands.Regoto.PerMissionMin);
            }
            else
            {
                gameCommands.Regoto.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Flag", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Flag", gameCommands.Showflag.CmdName);
            }
            else
            {
                gameCommands.Showflag.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Flag", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Flag", gameCommands.Showflag.PerMissionMin);
            }
            else
            {
                gameCommands.Showflag.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ShowOpen", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ShowOpen", gameCommands.ShowOpen.CmdName);
            }
            else
            {
                gameCommands.ShowOpen.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ShowOpen", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ShowOpen", gameCommands.ShowOpen.PerMissionMin);
            }
            else
            {
                gameCommands.ShowOpen.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ShowUnit", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ShowUnit", gameCommands.ShowUnit.CmdName);
            }
            else
            {
                gameCommands.ShowUnit.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ShowUnit", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ShowUnit", gameCommands.ShowUnit.PerMissionMin);
            }
            else
            {
                gameCommands.ShowUnit.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Attack", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Attack", gameCommands.Attack.CmdName);
            }
            else
            {
                gameCommands.Attack.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Attack", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Attack", gameCommands.Attack.PerMissionMin);
            }
            else
            {
                gameCommands.Attack.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Mob", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Mob", gameCommands.Mob.CmdName);
            }
            else
            {
                gameCommands.Mob.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Mob", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Mob", gameCommands.Mob.PerMissionMin);
            }
            else
            {
                gameCommands.Mob.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "MobNpc", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "MobNpc", gameCommands.MobNpc.CmdName);
            }
            else
            {
                gameCommands.MobNpc.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "MobNpc", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MobNpc", gameCommands.MobNpc.PerMissionMin);
            }
            else
            {
                gameCommands.MobNpc.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelNpc", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DelNpc", gameCommands.DeleteNpc.CmdName);
            }
            else
            {
                gameCommands.DeleteNpc.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelNpc", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelNpc", gameCommands.DeleteNpc.PerMissionMin);
            }
            else
            {
                gameCommands.DeleteNpc.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "NpcScript", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "NpcScript", gameCommands.NpcScript.CmdName);
            }
            else
            {
                gameCommands.NpcScript.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "NpcScript", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "NpcScript", gameCommands.NpcScript.PerMissionMin);
            }
            else
            {
                gameCommands.NpcScript.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "RecallMob", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "RecallMob", gameCommands.RecallMob.CmdName);
            }
            else
            {
                gameCommands.RecallMob.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "RecallMob", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "RecallMob", gameCommands.RecallMob.PerMissionMin);
            }
            else
            {
                gameCommands.RecallMob.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "LuckPoint", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "LuckPoint", gameCommands.LuckyPoint.CmdName);
            }
            else
            {
                gameCommands.LuckyPoint.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "LuckPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "LuckPoint", gameCommands.LuckyPoint.PerMissionMin);
            }
            else
            {
                gameCommands.LuckyPoint.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "LotteryTicket", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "LotteryTicket", gameCommands.LotteryTicket.CmdName);
            }
            else
            {
                gameCommands.LotteryTicket.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "LotteryTicket", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "LotteryTicket", gameCommands.LotteryTicket.PerMissionMin);
            }
            else
            {
                gameCommands.LotteryTicket.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadGuild", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReloadGuild", gameCommands.ReloadGuild.CmdName);
            }
            else
            {
                gameCommands.ReloadGuild.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadGuild", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadGuild", gameCommands.ReloadGuild.PerMissionMin);
            }
            else
            {
                gameCommands.ReloadGuild.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadLineNotice", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReloadLineNotice", gameCommands.ReloadLineNotice.CmdName);
            }
            else
            {
                gameCommands.ReloadLineNotice.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadLineNotice", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadLineNotice", gameCommands.ReloadLineNotice.PerMissionMin);
            }
            else
            {
                gameCommands.ReloadLineNotice.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadAbuse", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReloadAbuse", gameCommands.ReloadAbuse.CmdName);
            }
            else
            {
                gameCommands.ReloadAbuse.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadAbuse", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadAbuse", gameCommands.ReloadAbuse.PerMissionMin);
            }
            else
            {
                gameCommands.ReloadAbuse.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "BackStep", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "BackStep", gameCommands.BackStep.CmdName);
            }
            else
            {
                gameCommands.BackStep.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "BackStep", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "BackStep", gameCommands.BackStep.PerMissionMin);
            }
            else
            {
                gameCommands.BackStep.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Ball", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Ball", gameCommands.Ball.CmdName);
            }
            else
            {
                gameCommands.Ball.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Ball", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Ball", gameCommands.Ball.PerMissionMin);
            }
            else
            {
                gameCommands.Ball.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "FreePenalty", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "FreePenalty", gameCommands.FreePenalty.CmdName);
            }
            else
            {
                gameCommands.FreePenalty.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "FreePenalty", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "FreePenalty", gameCommands.FreePenalty.PerMissionMin);
            }
            else
            {
                gameCommands.FreePenalty.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "PkPoint", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "PkPoint", gameCommands.PkPoint.CmdName);
            }
            else
            {
                gameCommands.PkPoint.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "PkPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "PkPoint", gameCommands.PkPoint.PerMissionMin);
            }
            else
            {
                gameCommands.PkPoint.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "IncPkPoint", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "IncPkPoint", gameCommands.Incpkpoint.CmdName);
            }
            else
            {
                gameCommands.Incpkpoint.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "IncPkPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "IncPkPoint", gameCommands.Incpkpoint.PerMissionMin);
            }
            else
            {
                gameCommands.Incpkpoint.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeLuck", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ChangeLuck", gameCommands.ChangeLuck.CmdName);
            }
            else
            {
                gameCommands.ChangeLuck.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeLuck", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeLuck", gameCommands.ChangeLuck.PerMissionMin);
            }
            else
            {
                gameCommands.ChangeLuck.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Hunger", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Hunger", gameCommands.Hunger.CmdName);
            }
            else
            {
                gameCommands.Hunger.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Hunger", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Hunger", gameCommands.Hunger.PerMissionMin);
            }
            else
            {
                gameCommands.Hunger.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Hair", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Hair", gameCommands.Hair.CmdName);
            }
            else
            {
                gameCommands.Hair.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Hair", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Hair", gameCommands.Hair.PerMissionMin);
            }
            else
            {
                gameCommands.Hair.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Training", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Training", gameCommands.Training.CmdName);
            }
            else
            {
                gameCommands.Training.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Training", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Training", gameCommands.Training.PerMissionMin);
            }
            else
            {
                gameCommands.Training.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DeleteSkill", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DeleteSkill", gameCommands.DeleteSkill.CmdName);
            }
            else
            {
                gameCommands.DeleteSkill.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DeleteSkill", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DeleteSkill", gameCommands.DeleteSkill.PerMissionMin);
            }
            else
            {
                gameCommands.DeleteSkill.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeJob", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ChangeJob", gameCommands.ChangeJob.CmdName);
            }
            else
            {
                gameCommands.ChangeJob.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeJob", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeJob", gameCommands.ChangeJob.PerMissionMin);
            }
            else
            {
                gameCommands.ChangeJob.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeGender", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ChangeGender", gameCommands.ChangeGender.CmdName);
            }
            else
            {
                gameCommands.ChangeGender.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeGender", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeGender", gameCommands.ChangeGender.PerMissionMin);
            }
            else
            {
                gameCommands.ChangeGender.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "NameColor", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "NameColor", gameCommands.Namecolor.CmdName);
            }
            else
            {
                gameCommands.Namecolor.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "NameColor", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "NameColor", gameCommands.Namecolor.PerMissionMin);
            }
            else
            {
                gameCommands.Namecolor.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Mission", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Mission", gameCommands.Mission.CmdName);
            }
            else
            {
                gameCommands.Mission.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Mission", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Mission", gameCommands.Mission.PerMissionMin);
            }
            else
            {
                gameCommands.Mission.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "MobPlace", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "MobPlace", gameCommands.MobPlace.CmdName);
            }
            else
            {
                gameCommands.MobPlace.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "MobPlace", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MobPlace", gameCommands.MobPlace.PerMissionMin);
            }
            else
            {
                gameCommands.MobPlace.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Transparecy", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Transparecy", gameCommands.Transparecy.CmdName);
            }
            else
            {
                gameCommands.Transparecy.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Transparecy", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Transparecy", gameCommands.Transparecy.PerMissionMin);
            }
            else
            {
                gameCommands.Transparecy.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DeleteItem", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DeleteItem", gameCommands.DeleteItem.CmdName);
            }
            else
            {
                gameCommands.DeleteItem.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DeleteItem", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DeleteItem", gameCommands.DeleteItem.PerMissionMin);
            }
            else
            {
                gameCommands.DeleteItem.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Level0", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Level0", gameCommands.Level.CmdName);
            }
            else
            {
                gameCommands.Level.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Level0", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Level0", gameCommands.Level.PerMissionMin);
            }
            else
            {
                gameCommands.Level.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ClearMission", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ClearMission", gameCommands.ClearMission.CmdName);
            }
            else
            {
                gameCommands.ClearMission.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ClearMission", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ClearMission", gameCommands.ClearMission.PerMissionMin);
            }
            else
            {
                gameCommands.ClearMission.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SetFlag", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "SetFlag", gameCommands.SetFlag.CmdName);
            }
            else
            {
                gameCommands.SetFlag.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SetFlag", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SetFlag", gameCommands.SetFlag.PerMissionMin);
            }
            else
            {
                gameCommands.SetFlag.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SetOpen", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "SetOpen", gameCommands.SetOpen.CmdName);
            }
            else
            {
                gameCommands.SetOpen.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SetOpen", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SetOpen", gameCommands.SetOpen.PerMissionMin);
            }
            else
            {
                gameCommands.SetOpen.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SetUnit", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "SetUnit", gameCommands.SetUnit.CmdName);
            }
            else
            {
                gameCommands.SetUnit.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SetUnit", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SetUnit", gameCommands.SetUnit.PerMissionMin);
            }
            else
            {
                gameCommands.SetUnit.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReConnection", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReConnection", gameCommands.Reconnection.CmdName);
            }
            else
            {
                gameCommands.Reconnection.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReConnection", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReConnection", gameCommands.Reconnection.PerMissionMin);
            }
            else
            {
                gameCommands.Reconnection.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DisableFilter", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DisableFilter", gameCommands.DisableFilter.CmdName);
            }
            else
            {
                gameCommands.DisableFilter.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DisableFilter", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DisableFilter", gameCommands.DisableFilter.PerMissionMin);
            }
            else
            {
                gameCommands.DisableFilter.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeUserFull", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ChangeUserFull", gameCommands.ChguserFull.CmdName);
            }
            else
            {
                gameCommands.ChguserFull.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeUserFull", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeUserFull", gameCommands.ChguserFull.PerMissionMin);
            }
            else
            {
                gameCommands.ChguserFull.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeZenFastStep", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ChangeZenFastStep", gameCommands.ChgZenFastStep.CmdName);
            }
            else
            {
                gameCommands.ChgZenFastStep.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeZenFastStep", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeZenFastStep", gameCommands.ChgZenFastStep.PerMissionMin);
            }
            else
            {
                gameCommands.ChgZenFastStep.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ContestPoint", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ContestPoint", gameCommands.ContestPoint.CmdName);
            }
            else
            {
                gameCommands.ContestPoint.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ContestPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ContestPoint", gameCommands.ContestPoint.PerMissionMin);
            }
            else
            {
                gameCommands.ContestPoint.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "StartContest", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "StartContest", gameCommands.StartContest.CmdName);
            }
            else
            {
                gameCommands.StartContest.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "StartContest", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "StartContest", gameCommands.StartContest.PerMissionMin);
            }
            else
            {
                gameCommands.StartContest.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "EndContest", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "EndContest", gameCommands.EndContest.CmdName);
            }
            else
            {
                gameCommands.EndContest.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "EndContest", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "EndContest", gameCommands.EndContest.PerMissionMin);
            }
            else
            {
                gameCommands.EndContest.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Announcement", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Announcement", gameCommands.Announcement.CmdName);
            }
            else
            {
                gameCommands.Announcement.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Announcement", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Announcement", gameCommands.Announcement.PerMissionMin);
            }
            else
            {
                gameCommands.Announcement.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "OXQuizRoom", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "OXQuizRoom", gameCommands.Oxquizroom.CmdName);
            }
            else
            {
                gameCommands.Oxquizroom.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "OXQuizRoom", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "OXQuizRoom", gameCommands.Oxquizroom.PerMissionMin);
            }
            else
            {
                gameCommands.Oxquizroom.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Gsa", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Gsa", gameCommands.Gsa.CmdName);
            }
            else
            {
                gameCommands.Gsa.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Gsa", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Gsa", gameCommands.Gsa.PerMissionMin);
            }
            else
            {
                gameCommands.Gsa.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeItemName", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ChangeItemName", gameCommands.ChangeItemName.CmdName);
            }
            else
            {
                gameCommands.ChangeItemName.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeItemName", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeItemName", gameCommands.ChangeItemName.PerMissionMin);
            }
            else
            {
                gameCommands.ChangeItemName.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DisableSendMsg", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DisableSendMsg", gameCommands.DisableSendMsg.CmdName);
            }
            else
            {
                gameCommands.DisableSendMsg.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DisableSendMsg", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DisableSendMsg", gameCommands.DisableSendMsg.PerMissionMin);
            }
            else
            {
                gameCommands.DisableSendMsg.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "EnableSendMsg", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "EnableSendMsg", gameCommands.EnableSendMsg.CmdName);
            }
            else
            {
                gameCommands.EnableSendMsg.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "EnableSendMsg", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "EnableSendMsg", gameCommands.EnableSendMsg.PerMissionMin);
            }
            else
            {
                gameCommands.EnableSendMsg.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DisableSendMsgList", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DisableSendMsgList", gameCommands.DisableSendMsgList.CmdName);
            }
            else
            {
                gameCommands.DisableSendMsgList.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DisableSendMsgList", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DisableSendMsgList", gameCommands.DisableSendMsgList.PerMissionMin);
            }
            else
            {
                gameCommands.DisableSendMsgList.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Kill", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Kill", gameCommands.Kill.CmdName);
            }
            else
            {
                gameCommands.Kill.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Kill", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Kill", gameCommands.Kill.PerMissionMin);
            }
            else
            {
                gameCommands.Kill.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Make", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Make", gameCommands.Make.CmdName);
            }
            else
            {
                gameCommands.Make.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "MakeMin", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MakeMin", gameCommands.Make.PerMissionMin);
            }
            else
            {
                gameCommands.Make.PerMissionMin = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Permission", "MakeMax", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MakeMax", gameCommands.Make.PerMissionMax);
            }
            else
            {
                gameCommands.Make.PerMissionMax = nLoadInteger;
            }
            LoadString = ReadString("Command", "SuperMake", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "SuperMake", gameCommands.Smake.CmdName);
            }
            else
            {
                gameCommands.Smake.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SuperMake", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SuperMake", gameCommands.Smake.PerMissionMin);
            }
            else
            {
                gameCommands.Smake.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "BonusPoint", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "BonusPoint", gameCommands.BonusPoint.CmdName);
            }
            else
            {
                gameCommands.BonusPoint.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "BonusPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "BonusPoint", gameCommands.BonusPoint.PerMissionMin);
            }
            else
            {
                gameCommands.BonusPoint.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelBonuPoint", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DelBonuPoint", gameCommands.DelBonusPoint.CmdName);
            }
            else
            {
                gameCommands.DelBonusPoint.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelBonuPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelBonuPoint", gameCommands.DelBonusPoint.PerMissionMin);
            }
            else
            {
                gameCommands.DelBonusPoint.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "RestBonuPoint", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "RestBonuPoint", gameCommands.Restbonuspoint.CmdName);
            }
            else
            {
                gameCommands.Restbonuspoint.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "RestBonuPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "RestBonuPoint", gameCommands.Restbonuspoint.PerMissionMin);
            }
            else
            {
                gameCommands.Restbonuspoint.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "FireBurn", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "FireBurn", gameCommands.FireBurn.CmdName);
            }
            else
            {
                gameCommands.FireBurn.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "FireBurn", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "FireBurn", gameCommands.FireBurn.PerMissionMin);
            }
            else
            {
                gameCommands.FireBurn.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "TestStatus", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "TestStatus", gameCommands.TestStatus.CmdName);
            }
            else
            {
                gameCommands.TestStatus.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "TestStatus", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "TestStatus", gameCommands.TestStatus.PerMissionMin);
            }
            else
            {
                gameCommands.TestStatus.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelGold", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DelGold", gameCommands.DelGold.CmdName);
            }
            else
            {
                gameCommands.DelGold.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelGold", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelGold", gameCommands.DelGold.PerMissionMin);
            }
            else
            {
                gameCommands.DelGold.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AddGold", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AddGold", gameCommands.AddGold.CmdName);
            }
            else
            {
                gameCommands.AddGold.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AddGold", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AddGold", gameCommands.AddGold.PerMissionMin);
            }
            else
            {
                gameCommands.AddGold.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelGameGold", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DelGameGold", gameCommands.DelGameGold.CmdName);
            }
            else
            {
                gameCommands.DelGameGold.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelGameGold", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelGameGold", gameCommands.DelGameGold.PerMissionMin);
            }
            else
            {
                gameCommands.DelGameGold.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AddGamePoint", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AddGamePoint", gameCommands.AddGameGold.CmdName);
            }
            else
            {
                gameCommands.AddGameGold.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AddGameGold", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AddGameGold", gameCommands.AddGameGold.PerMissionMin);
            }
            else
            {
                gameCommands.AddGameGold.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "GameGold", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "GameGold", gameCommands.GameGold.CmdName);
            }
            else
            {
                gameCommands.GameGold.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "GameGold", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "GameGold", gameCommands.GameGold.PerMissionMin);
            }
            else
            {
                gameCommands.GameGold.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "GamePoint", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "GamePoint", gameCommands.GamePoint.CmdName);
            }
            else
            {
                gameCommands.GamePoint.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "GamePoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "GamePoint", gameCommands.GamePoint.PerMissionMin);
            }
            else
            {
                gameCommands.GamePoint.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "CreditPoint", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "CreditPoint", gameCommands.CreditPoint.CmdName);
            }
            else
            {
                gameCommands.CreditPoint.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "CreditPoint", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "CreditPoint", gameCommands.CreditPoint.PerMissionMin);
            }
            else
            {
                gameCommands.CreditPoint.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "TestGoldChange", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "TestGoldChange", gameCommands.Testgoldchange.CmdName);
            }
            else
            {
                gameCommands.Testgoldchange.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "TestGoldChange", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "TestGoldChange", gameCommands.Testgoldchange.PerMissionMin);
            }
            else
            {
                gameCommands.Testgoldchange.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "RefineWeapon", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "RefineWeapon", gameCommands.RefineWeapon.CmdName);
            }
            else
            {
                gameCommands.RefineWeapon.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "RefineWeapon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "RefineWeapon", gameCommands.RefineWeapon.PerMissionMin);
            }
            else
            {
                gameCommands.RefineWeapon.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadAdmin", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReloadAdmin", gameCommands.ReloadAdmin.CmdName);
            }
            else
            {
                gameCommands.ReloadAdmin.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadAdmin", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadAdmin", gameCommands.ReloadAdmin.PerMissionMin);
            }
            else
            {
                gameCommands.ReloadAdmin.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadNpc", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReloadNpc", gameCommands.ReloadNpc.CmdName);
            }
            else
            {
                gameCommands.ReloadNpc.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadNpc", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadNpc", gameCommands.ReloadNpc.PerMissionMin);
            }
            else
            {
                gameCommands.ReloadNpc.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadManage", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReloadManage", gameCommands.ReloadManage.CmdName);
            }
            else
            {
                gameCommands.ReloadManage.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadManage", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadManage", gameCommands.ReloadManage.PerMissionMin);
            }
            else
            {
                gameCommands.ReloadManage.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadRobotManage", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReloadRobotManage", gameCommands.ReloadRobotManage.CmdName);
            }
            else
            {
                gameCommands.ReloadRobotManage.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadRobotManage", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadRobotManage", gameCommands.ReloadRobotManage.PerMissionMin);
            }
            else
            {
                gameCommands.ReloadRobotManage.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadRobot", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReloadRobot", gameCommands.ReloadRobot.CmdName);
            }
            else
            {
                gameCommands.ReloadRobot.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadRobot", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadRobot", gameCommands.ReloadRobot.PerMissionMin);
            }
            else
            {
                gameCommands.ReloadRobot.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadMonitems", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReloadMonitems", gameCommands.ReloadMonItems.CmdName);
            }
            else
            {
                gameCommands.ReloadMonItems.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadMonitems", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadMonitems", gameCommands.ReloadMonItems.PerMissionMin);
            }
            else
            {
                gameCommands.ReloadMonItems.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadDiary", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReloadDiary", gameCommands.Reloaddiary.CmdName);
            }
            else
            {
                gameCommands.Reloaddiary.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadDiary", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadDiary", gameCommands.Reloaddiary.PerMissionMin);
            }
            else
            {
                gameCommands.Reloaddiary.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadItemDB", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReloadItemDB", gameCommands.ReloadItemDB.CmdName);
            }
            else
            {
                gameCommands.ReloadItemDB.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadItemDB", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadItemDB", gameCommands.ReloadItemDB.PerMissionMin);
            }
            else
            {
                gameCommands.ReloadItemDB.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadMagicDB", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReloadMagicDB", gameCommands.ReloadMagicDb.CmdName);
            }
            else
            {
                gameCommands.ReloadMagicDb.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadMagicDB", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadMagicDB", gameCommands.ReloadMagicDb.PerMissionMin);
            }
            else
            {
                gameCommands.ReloadMagicDb.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadMonsterDB", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReloadMonsterDB", gameCommands.Reloadmonsterdb.CmdName);
            }
            else
            {
                gameCommands.Reloadmonsterdb.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadMonsterDB", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadMonsterDB", gameCommands.Reloadmonsterdb.PerMissionMin);
            }
            else
            {
                gameCommands.Reloadmonsterdb.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReAlive", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReAlive", gameCommands.ReaLive.CmdName);
            }
            else
            {
                gameCommands.ReaLive.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReAlive", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReAlive", gameCommands.ReaLive.PerMissionMin);
            }
            else
            {
                gameCommands.ReaLive.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AdjuestTLevel", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AdjuestTLevel", gameCommands.AdjuestLevel.CmdName);
            }
            else
            {
                gameCommands.AdjuestLevel.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AdjuestTLevel", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AdjuestTLevel", gameCommands.AdjuestLevel.PerMissionMin);
            }
            else
            {
                gameCommands.AdjuestLevel.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AdjuestExp", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AdjuestExp", gameCommands.AdjuestExp.CmdName);
            }
            else
            {
                gameCommands.AdjuestExp.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AdjuestExp", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AdjuestExp", gameCommands.AdjuestExp.PerMissionMin);
            }
            else
            {
                gameCommands.AdjuestExp.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AddGuild", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AddGuild", gameCommands.AddGuild.CmdName);
            }
            else
            {
                gameCommands.AddGuild.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AddGuild", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AddGuild", gameCommands.AddGuild.PerMissionMin);
            }
            else
            {
                gameCommands.AddGuild.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelGuild", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DelGuild", gameCommands.DelGuild.CmdName);
            }
            else
            {
                gameCommands.DelGuild.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelGuild", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelGuild", gameCommands.DelGuild.PerMissionMin);
            }
            else
            {
                gameCommands.DelGuild.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeSabukLord", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ChangeSabukLord", gameCommands.ChangeSabukLord.CmdName);
            }
            else
            {
                gameCommands.ChangeSabukLord.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeSabukLord", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeSabukLord", gameCommands.ChangeSabukLord.PerMissionMin);
            }
            else
            {
                gameCommands.ChangeSabukLord.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ForcedWallConQuestWar", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ForcedWallConQuestWar", gameCommands.ForcedWallConQuestWar.CmdName);
            }
            else
            {
                gameCommands.ForcedWallConQuestWar.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ForcedWallConQuestWar", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ForcedWallConQuestWar", gameCommands.ForcedWallConQuestWar.PerMissionMin);
            }
            else
            {
                gameCommands.ForcedWallConQuestWar.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AddToItemEvent", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AddToItemEvent", gameCommands.Addtoitemevent.CmdName);
            }
            else
            {
                gameCommands.Addtoitemevent.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AddToItemEvent", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AddToItemEvent", gameCommands.Addtoitemevent.PerMissionMin);
            }
            else
            {
                gameCommands.Addtoitemevent.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AddToItemEventAspieces", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AddToItemEventAspieces", gameCommands.Addtoitemeventaspieces.CmdName);
            }
            else
            {
                gameCommands.Addtoitemeventaspieces.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AddToItemEventAspieces", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AddToItemEventAspieces", gameCommands.Addtoitemeventaspieces.PerMissionMin);
            }
            else
            {
                gameCommands.Addtoitemeventaspieces.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ItemEventList", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ItemEventList", gameCommands.Itemeventlist.CmdName);
            }
            else
            {
                gameCommands.Itemeventlist.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ItemEventList", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ItemEventList", gameCommands.Itemeventlist.PerMissionMin);
            }
            else
            {
                gameCommands.Itemeventlist.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "StartIngGiftNO", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "StartIngGiftNO", gameCommands.Startinggiftno.CmdName);
            }
            else
            {
                gameCommands.Startinggiftno.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "StartIngGiftNO", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "StartIngGiftNO", gameCommands.Startinggiftno.PerMissionMin);
            }
            else
            {
                gameCommands.Startinggiftno.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DeleteAllItemEvent", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DeleteAllItemEvent", gameCommands.Deleteallitemevent.CmdName);
            }
            else
            {
                gameCommands.Deleteallitemevent.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DeleteAllItemEvent", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DeleteAllItemEvent", gameCommands.Deleteallitemevent.PerMissionMin);
            }
            else
            {
                gameCommands.Deleteallitemevent.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "StartItemEvent", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "StartItemEvent", gameCommands.Startitemevent.CmdName);
            }
            else
            {
                gameCommands.Startitemevent.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "StartItemEvent", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "StartItemEvent", gameCommands.Startitemevent.PerMissionMin);
            }
            else
            {
                gameCommands.Startitemevent.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ItemEventTerm", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ItemEventTerm", gameCommands.Itemeventterm.CmdName);
            }
            else
            {
                gameCommands.Itemeventterm.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ItemEventTerm", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ItemEventTerm", gameCommands.Itemeventterm.PerMissionMin);
            }
            else
            {
                gameCommands.Itemeventterm.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "AdjuestTestLevel", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "AdjuestTestLevel", gameCommands.Adjuesttestlevel.CmdName);
            }
            else
            {
                gameCommands.Adjuesttestlevel.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "AdjuestTestLevel", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "AdjuestTestLevel", gameCommands.Adjuesttestlevel.PerMissionMin);
            }
            else
            {
                gameCommands.Adjuesttestlevel.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "OpTraining", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "OpTraining", gameCommands.TrainingSkill.CmdName);
            }
            else
            {
                gameCommands.TrainingSkill.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "OpTraining", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "OpTraining", gameCommands.TrainingSkill.PerMissionMin);
            }
            else
            {
                gameCommands.TrainingSkill.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "OpDeleteSkill", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "OpDeleteSkill", gameCommands.Opdeleteskill.CmdName);
            }
            else
            {
                gameCommands.Opdeleteskill.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "OpDeleteSkill", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "OpDeleteSkill", gameCommands.Opdeleteskill.PerMissionMin);
            }
            else
            {
                gameCommands.Opdeleteskill.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeWeaponDura", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ChangeWeaponDura", gameCommands.Changeweapondura.CmdName);
            }
            else
            {
                gameCommands.Changeweapondura.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeWeaponDura", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeWeaponDura", gameCommands.Changeweapondura.PerMissionMin);
            }
            else
            {
                gameCommands.Changeweapondura.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReloadGuildAll", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReloadGuildAll", gameCommands.ReloadGuildAll.CmdName);
            }
            else
            {
                gameCommands.ReloadGuildAll.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReloadGuildAll", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReloadGuildAll", gameCommands.ReloadGuildAll.PerMissionMin);
            }
            else
            {
                gameCommands.ReloadGuildAll.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Who", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Who", gameCommands.Who.CmdName);
            }
            else
            {
                gameCommands.Who.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Who", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Who", gameCommands.Who.PerMissionMin);
            }
            else
            {
                gameCommands.Who.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "Total", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "Total", gameCommands.Total.CmdName);
            }
            else
            {
                gameCommands.Total.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "Total", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "Total", gameCommands.Total.PerMissionMin);
            }
            else
            {
                gameCommands.Total.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "TestGa", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "TestGa", gameCommands.Testga.CmdName);
            }
            else
            {
                gameCommands.Testga.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "TestGa", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "TestGa", gameCommands.Testga.PerMissionMin);
            }
            else
            {
                gameCommands.Testga.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "MapInfo", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "MapInfo", gameCommands.MapInfo.CmdName);
            }
            else
            {
                gameCommands.MapInfo.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "MapInfo", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "MapInfo", gameCommands.MapInfo.PerMissionMin);
            }
            else
            {
                gameCommands.MapInfo.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SbkDoor", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "SbkDoor", gameCommands.SbkDoor.CmdName);
            }
            else
            {
                gameCommands.SbkDoor.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SbkDoor", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SbkDoor", gameCommands.SbkDoor.PerMissionMin);
            }
            else
            {
                gameCommands.SbkDoor.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeDearName", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ChangeDearName", gameCommands.ChangeDearName.CmdName);
            }
            else
            {
                gameCommands.ChangeDearName.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeDearName", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeDearName", gameCommands.ChangeDearName.PerMissionMin);
            }
            else
            {
                gameCommands.ChangeDearName.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ChangeMasterName", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ChangeMasterrName", gameCommands.ChangeMasterName.CmdName);
            }
            else
            {
                gameCommands.ChangeMasterName.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ChangeMasterName", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ChangeMasterName", gameCommands.ChangeMasterName.PerMissionMin);
            }
            else
            {
                gameCommands.ChangeMasterName.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "StartQuest", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "StartQuest", gameCommands.StartQuest.CmdName);
            }
            else
            {
                gameCommands.StartQuest.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "StartQuest", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "StartQuest", gameCommands.StartQuest.PerMissionMin);
            }
            else
            {
                gameCommands.StartQuest.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SetPermission", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "SetPermission", gameCommands.SetperMission.CmdName);
            }
            else
            {
                gameCommands.SetperMission.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SetPermission", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SetPermission", gameCommands.SetperMission.PerMissionMin);
            }
            else
            {
                gameCommands.SetperMission.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ClearMon", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ClearMon", gameCommands.ClearMon.CmdName);
            }
            else
            {
                gameCommands.ClearMon.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ClearMon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ClearMon", gameCommands.ClearMon.PerMissionMin);
            }
            else
            {
                gameCommands.ClearMon.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ReNewLevel", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ReNewLevel", gameCommands.RenewLevel.CmdName);
            }
            else
            {
                gameCommands.RenewLevel.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ReNewLevel", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ReNewLevel", gameCommands.RenewLevel.PerMissionMin);
            }
            else
            {
                gameCommands.RenewLevel.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DenyIPaddrLogon", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DenyIPaddrLogon", gameCommands.DenyipLogon.CmdName);
            }
            else
            {
                gameCommands.DenyipLogon.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DenyIPaddrLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DenyIPaddrLogon", gameCommands.DenyipLogon.PerMissionMin);
            }
            else
            {
                gameCommands.DenyipLogon.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DenyAccountLogon", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DenyAccountLogon", gameCommands.DenyAccountLogon.CmdName);
            }
            else
            {
                gameCommands.DenyAccountLogon.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DenyAccountLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DenyAccountLogon", gameCommands.DenyAccountLogon.PerMissionMin);
            }
            else
            {
                gameCommands.DenyAccountLogon.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DenyChrNameLogon", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DenyChrNameLogon", gameCommands.DenyChrNameLogon.CmdName);
            }
            else
            {
                gameCommands.DenyChrNameLogon.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DenyChrNameLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DenyChrNameLogon", gameCommands.DenyChrNameLogon.PerMissionMin);
            }
            else
            {
                gameCommands.DenyChrNameLogon.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelDenyIPLogon", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DelDenyIPLogon", gameCommands.DelDenyIpLogon.CmdName);
            }
            else
            {
                gameCommands.DelDenyIpLogon.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelDenyIPLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelDenyIPLogon", gameCommands.DelDenyIpLogon.PerMissionMin);
            }
            else
            {
                gameCommands.DelDenyIpLogon.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelDenyAccountLogon", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DelDenyAccountLogon", gameCommands.DelDenyAccountLogon.CmdName);
            }
            else
            {
                gameCommands.DelDenyAccountLogon.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelDenyAccountLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelDenyAccountLogon", gameCommands.DelDenyAccountLogon.PerMissionMin);
            }
            else
            {
                gameCommands.DelDenyAccountLogon.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "DelDenyChrNameLogon", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "DelDenyChrNameLogon", gameCommands.DelDenyChrNameLogon.CmdName);
            }
            else
            {
                gameCommands.DelDenyChrNameLogon.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "DelDenyChrNameLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "DelDenyChrNameLogon", gameCommands.DelDenyChrNameLogon.PerMissionMin);
            }
            else
            {
                gameCommands.DelDenyChrNameLogon.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ShowDenyIPLogon", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ShowDenyIPLogon", gameCommands.ShowDenyIpLogon.CmdName);
            }
            else
            {
                gameCommands.ShowDenyIpLogon.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ShowDenyIPLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ShowDenyIPLogon", gameCommands.ShowDenyIpLogon.PerMissionMin);
            }
            else
            {
                gameCommands.ShowDenyIpLogon.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ShowDenyAccountLogon", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ShowDenyAccountLogon", gameCommands.ShowDenyAccountLogon.CmdName);
            }
            else
            {
                gameCommands.ShowDenyAccountLogon.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ShowDenyAccountLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ShowDenyAccountLogon", gameCommands.ShowDenyAccountLogon.PerMissionMin);
            }
            else
            {
                gameCommands.ShowDenyAccountLogon.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ShowDenyChrNameLogon", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ShowDenyChrNameLogon", gameCommands.ShowDenyChrNameLogon.CmdName);
            }
            else
            {
                gameCommands.ShowDenyChrNameLogon.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ShowDenyChrNameLogon", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ShowDenyChrNameLogon", gameCommands.ShowDenyChrNameLogon.PerMissionMin);
            }
            else
            {
                gameCommands.ShowDenyChrNameLogon.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ViewWhisper", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ViewWhisper", gameCommands.ViewWhisper.CmdName);
            }
            else
            {
                gameCommands.ViewWhisper.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ViewWhisper", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ViewWhisper", gameCommands.ViewWhisper.PerMissionMin);
            }
            else
            {
                gameCommands.ViewWhisper.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SpiritStart", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "SpiritStart", gameCommands.Spirit.CmdName);
            }
            else
            {
                gameCommands.Spirit.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SpiritStart", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SpiritStart", gameCommands.Spirit.PerMissionMin);
            }
            else
            {
                gameCommands.Spirit.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SpiritStop", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "SpiritStop", gameCommands.SpiritStop.CmdName);
            }
            else
            {
                gameCommands.SpiritStop.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SpiritStop", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SpiritStop", gameCommands.SpiritStop.PerMissionMin);
            }
            else
            {
                gameCommands.SpiritStop.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "SetMapMode", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "SetMapMode", gameCommands.SetMapMode.CmdName);
            }
            else
            {
                gameCommands.SetMapMode.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "SetMapMode", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "SetMapMode", gameCommands.SetMapMode.PerMissionMin);
            }
            else
            {
                gameCommands.SetMapMode.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ShoweMapMode", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ShoweMapMode", gameCommands.ShowMapMode.CmdName);
            }
            else
            {
                gameCommands.ShowMapMode.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ShoweMapMode", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ShoweMapMode", gameCommands.ShowMapMode.PerMissionMin);
            }
            else
            {
                gameCommands.ShowMapMode.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "ClearBag", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "ClearBag", gameCommands.ClearBag.CmdName);
            }
            else
            {
                gameCommands.ClearBag.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "ClearBag", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "ClearBag", gameCommands.ClearBag.PerMissionMin);
            }
            else
            {
                gameCommands.ClearBag.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "LockLogin", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "LockLogin", gameCommands.LockLogon.CmdName);
            }
            else
            {
                gameCommands.LockLogon.CmdName = LoadString;
            }
            nLoadInteger = ReadInteger("Permission", "LockLogin", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "LockLogin", gameCommands.LockLogon.PerMissionMin);
            }
            else
            {
                gameCommands.LockLogon.PerMissionMin = nLoadInteger;
            }
            LoadString = ReadString("Command", "GMRedMsgCmd", "");
            if (string.IsNullOrEmpty(LoadString))
            {
                WriteString("Command", "GMRedMsgCmd", M2Share.GMRedMsgCmd);
            }
            else
            {
                M2Share.GMRedMsgCmd = LoadString[0];
            }
            nLoadInteger = ReadInteger("Permission", "GMRedMsgCmd", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Permission", "GMRedMsgCmd", M2Share.GMREDMSGCMD);
            }
            else
            {
                M2Share.GMREDMSGCMD = nLoadInteger;
            }
        }
    }
}