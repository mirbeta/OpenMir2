using CommandModule.Commands;

namespace CommandModule
{
    public class GameCommands
    {
        public readonly GameCmd Testserverconfig;
        public readonly GameCmd Serverstatus;
        public readonly GameCmd Testgetbagitem;
        public readonly GameCmd Mobfireburn;
        public readonly GameCmd Testspeedmode;
        public readonly GameCmd Reloadminmap;
        public readonly GameCmd Attack;
        public readonly GameCmd Testgoldchange;
        public readonly GameCmd Diary;
        public readonly GameCmd NameColor;
        public readonly GameCmd Ball;
        public readonly GameCmd ChangeLuck;
        public readonly GameCmd Transparecy;
        public readonly GameCmd Addtoitemevent;
        public readonly GameCmd Addtoitemeventaspieces;
        public readonly GameCmd ItemEventList;
        public readonly GameCmd Startinggiftno;
        public readonly GameCmd Deleteallitemevent;
        public readonly GameCmd Startitemevent;
        public readonly GameCmd Itemeventterm;
        public readonly GameCmd Adjuesttestlevel;
        public readonly GameCmd Testga;
        public readonly GameCmd Opdeleteskill;
        public readonly GameCmd Changeweapondura;
        public readonly GameCmd Reloadmonsterdb;
        public readonly GameCmd Reloaddiary;
        public readonly GameCmd ReloadItemDB;
        public readonly GameCmd Restbonuspoint;
        public readonly GameCmd Oxquizroom;
        public readonly GameCmd Gsa;
        public readonly GameCmd Recall;
        public readonly GameCmd Regoto;

        [RegisterCommand(typeof(ShowUseItemInfoCommand))]
        public readonly GameCmd ShowUseItemInfo;
        [RegisterCommand(typeof(BindUseItemCommand))]
        public readonly GameCmd BindUseItem;
        [RegisterCommand(typeof(AdjustExpCommand))]
        public readonly GameCmd AdjustExp;
        [RegisterCommand(typeof(AdjuestExpCommand))]
        public readonly GameCmd AdjuestExp;
        [RegisterCommand(typeof(WhoCommand))]
        public readonly GameCmd Who;
        [RegisterCommand(typeof(TotalCommand))]
        public readonly GameCmd Total;
        [RegisterCommand((typeof(ReloadGuildCommand)))]
        public readonly GameCmd ReloadGuild;
        [RegisterCommand(typeof(SearchMasterCommand))]
        public readonly GameCmd Master;
        [RegisterCommand(typeof(SearchDearCommand))]
        public readonly GameCmd Dear;
        [RegisterCommand(typeof(NpcScriptCommand))]
        public readonly GameCmd NpcScript;
        [RegisterCommand(typeof(GroupRecallCommand))]
        public readonly GameCmd GroupRecalll;
        [RegisterCommand(typeof(GuildRecallCommand))]
        public readonly GameCmd GuildRecalll;
        [RegisterCommand(typeof(SearchHumanCommand))]
        public readonly GameCmd Searching;
        [RegisterCommand(typeof(RecallMobCommand))]
        public readonly GameCmd RecallMob;
        [RegisterCommand(typeof(ChangeSalveStatusCommand))]
        public readonly GameCmd Rest;
        [RegisterCommand(typeof(EndGuildCommand))]
        public readonly GameCmd EndGuild;
        [RegisterCommand(typeof(AuthallyCommand))]
        public readonly GameCmd Authally;
        [RegisterCommand(typeof(LetGuildCommand))]
        public readonly GameCmd LetGuild;
        [RegisterCommand(typeof(BanGuildChatCommand))]
        public readonly GameCmd BanGuildChat;
        [RegisterCommand(typeof(LetTradeCommand))]
        public readonly GameCmd LetTrade;
        [RegisterCommand(typeof(LetShoutCommand))]
        public readonly GameCmd Letshout;
        [RegisterCommand(typeof(AllowMsgCommand))]
        public readonly GameCmd AllowMsg;
        [RegisterCommand(typeof(ShowHumanUnitOpenCommand))]
        public readonly GameCmd ShowOpen;
        [RegisterCommand(typeof(ShowUnitCommand))]
        public readonly GameCmd ShowUnit;
        [RegisterCommand(typeof(AllowGuildRecallCommand))]
        public readonly GameCmd AllowGuildRecall;
        [RegisterCommand(typeof(DelBonuPointCommand))]
        public readonly GameCmd DelBonusPoint;
        [RegisterCommand(typeof(BonuPointCommand))]
        public readonly GameCmd BonusPoint;
        [RegisterCommand(typeof(ShowSbkGoldCommand))]
        public readonly GameCmd SabukWallGold;
        [RegisterCommand(typeof(HumanInfoCommand))]
        public readonly GameCmd Info;
        [RegisterCommand(typeof(SetPassWordCommand))]
        public readonly GameCmd SetPassword;
        [RegisterCommand(typeof(ChgpassWordCommand))]
        public readonly GameCmd ChgPassword;
        [RegisterCommand(typeof(ClearHumanPasswordCommand))]
        public readonly GameCmd ClrPassword;
        [RegisterCommand(typeof(UnPasswWordCommand))]
        public readonly GameCmd UnPassword;
        [RegisterCommand(typeof(UnlockStorageCommand))]
        public readonly GameCmd UnlockStorage;
        [RegisterCommand(typeof(UnLockCommand))]
        public readonly GameCmd Unlock;
        [RegisterCommand(typeof(LockCommand))]
        public readonly GameCmd Lock;
        [RegisterCommand(typeof(SetFlagCommand))]
        public readonly GameCmd SetFlag;
        [RegisterCommand(typeof(SetOpenCommand))]
        public readonly GameCmd SetOpen;
        [RegisterCommand(typeof(SetUnitCommand))]
        public readonly GameCmd SetUnit;
        [RegisterCommand(typeof(PasswordLockCommand))]
        public readonly GameCmd PasswordLock;
        [RegisterCommand(typeof(AuthCancelCommand))]
        public readonly GameCmd AuthCancel;
        [RegisterCommand(typeof(AuthCommand))]
        public readonly GameCmd Auth;
        [RegisterCommand(typeof(DataCommand))]
        public readonly GameCmd Data;
        [RegisterCommand(typeof(PrvMsgCommand))]
        public readonly GameCmd PrvMsg;
        [RegisterCommand(typeof(UserMoveXyCommand))]
        public readonly GameCmd UserMove;
        [RegisterCommand(typeof(AllowGroupReCallCommand))]
        public readonly GameCmd AllowGroupCall;
        [RegisterCommand(typeof(MemberFunctionCommand))]
        public readonly GameCmd MemberFunction;
        [RegisterCommand(typeof(MemberFunctionExCommand))]
        public readonly GameCmd MemberFunctioneX;
        [RegisterCommand(typeof(AllowDearRecallCommand))]
        public readonly GameCmd AllowDearRcall;
        [RegisterCommand(typeof(DearRecallCommond))]
        public readonly GameCmd DearRecall;
        [RegisterCommand(typeof(AllowMasterRecallCommand))]
        public readonly GameCmd AllowMasterRecall;
        [RegisterCommand(typeof(MasterRecallCommand))]
        public readonly GameCmd MasteRecall;
        [RegisterCommand(typeof(ChangeAttackModeCommand))]
        public readonly GameCmd AttackMode;
        [RegisterCommand(typeof(TakeOnHorseCommand))]
        public readonly GameCmd TakeonHorse;
        [RegisterCommand(typeof(TakeOffHorseCommand))]
        public readonly GameCmd TakeofHorse;
        [RegisterCommand(typeof(HumanLocalCommand))]
        public readonly GameCmd HumanLocal;
        [RegisterCommand(typeof(MapMoveCommand))]
        public readonly GameCmd Move;
        [RegisterCommand(typeof(PositionMoveCommand))]
        public readonly GameCmd PositionMove;
        [RegisterCommand(typeof(MobLevelCommand))]
        public readonly GameCmd MobLevel;
        [RegisterCommand(typeof(MobCountCommand))]
        public readonly GameCmd MobCount;
        [RegisterCommand(typeof(HumanCountCommand))]
        public readonly GameCmd HumanCount;
        [RegisterCommand(typeof(MapInfoCommand))]
        public readonly GameCmd Map;
        [RegisterCommand(typeof(KickHumanCommand))]
        public readonly GameCmd Kick;
        [RegisterCommand(typeof(TingCommand))]
        public readonly GameCmd Ting;
        [RegisterCommand(typeof(SuperTingCommand))]
        public readonly GameCmd Superting;
        [RegisterCommand(typeof(MapMoveHumanCommand))]
        public readonly GameCmd MapMove;
        [RegisterCommand(typeof(ShutupCommand))]
        public readonly GameCmd ShutUp;
        [RegisterCommand(typeof(ShutupReleaseCommand))]
        public readonly GameCmd ReleaseShutup;
        [RegisterCommand(typeof(ShutupListCommand))]
        public readonly GameCmd ShutupList;
        [RegisterCommand(typeof(ChangeAdminModeCommand))]
        public readonly GameCmd GameMaster;
        [RegisterCommand(typeof(ChangeObModeCommand))]
        public readonly GameCmd ObServer;
        [RegisterCommand(typeof(ChangeSuperManModeCommand))]
        public readonly GameCmd SueprMan;
        [RegisterCommand(typeof(ChangeLevelCommand))]
        public readonly GameCmd Level;
        [RegisterCommand(typeof(ShowHumanFlagCommand))]
        public readonly GameCmd ShowFlag;
        [RegisterCommand(typeof(MobCommand))]
        public readonly GameCmd Mob;
        [RegisterCommand(typeof(MobNpcCommand))]
        public readonly GameCmd MobNpc;
        [RegisterCommand(typeof(DelNpcCommand))]
        public readonly GameCmd DeleteNpc;
        [RegisterCommand(typeof(LuckPointCommand))]
        public readonly GameCmd LuckyPoint;
        [RegisterCommand(typeof(LotteryTicketCommandL))]
        public readonly GameCmd LotteryTicket;
        [RegisterCommand(typeof(ReloadLineNoticeCommand))]
        public readonly GameCmd ReloadLineNotice;
        [RegisterCommand(typeof(ReloadAbuseCommand))]
        public readonly GameCmd ReloadAbuse;
        [RegisterCommand(typeof(BackStepCommand))]
        public readonly GameCmd BackStep;
        [RegisterCommand(typeof(FreePenaltyCommand))]
        public readonly GameCmd FreePenalty;
        [RegisterCommand(typeof(PKpointCommand))]
        public readonly GameCmd PkPoint;
        [RegisterCommand(typeof(IncPkPointCommand))]
        public readonly GameCmd IncpkPoint;
        [RegisterCommand(typeof(HungerCommand))]
        public readonly GameCmd Hunger;
        [RegisterCommand(typeof(HairCommand))]
        public readonly GameCmd Hair;
        [RegisterCommand(typeof(TrainingCommand))]
        public readonly GameCmd Training;
        [RegisterCommand(typeof(DelSkillCommand))]
        public readonly GameCmd DeleteSkill;
        [RegisterCommand(typeof(ChangeJobCommand))]
        public readonly GameCmd ChangeJob;
        [RegisterCommand(typeof(ChangeGenderCommand))]
        public readonly GameCmd ChangeGender;
        [RegisterCommand(typeof(MissionCommand))]
        public readonly GameCmd Mission;
        [RegisterCommand(typeof(MobPlaceCommand))]
        public readonly GameCmd MobPlace;
        [RegisterCommand(typeof(DeleteItemCommand))]
        public readonly GameCmd DeleteItem;
        [RegisterCommand(typeof(ClearMissionCommand))]
        public readonly GameCmd ClearMission;
        [RegisterCommand(typeof(ReconnectionCommand))]
        public readonly GameCmd Reconnection;
        [RegisterCommand(typeof(DisableFilterCommand))]
        public readonly GameCmd DisableFilter;
        [RegisterCommand(typeof(ChangeUserFullCommand))]
        public readonly GameCmd ChguserFull;
        [RegisterCommand(typeof(ChangeZenFastStepCommand))]
        public readonly GameCmd ChgZenFastStep;
        [RegisterCommand(typeof(ContestPointCommand))]
        public readonly GameCmd ContestPoint;
        [RegisterCommand(typeof(StartContestCommand))]
        public readonly GameCmd StartContest;
        [RegisterCommand(typeof(EndContestCommand))]
        public readonly GameCmd EndContest;
        [RegisterCommand(typeof(AnnouncementCommand))]
        public readonly GameCmd Announcement;
        [RegisterCommand(typeof(ChangeItemNameCommand))]
        public readonly GameCmd ChangeItemName;
        [RegisterCommand(typeof(DisableSendMsgCommand))]
        public readonly GameCmd DisableSendMsg;
        [RegisterCommand(typeof(EnableSendMsgCommand))]
        public readonly GameCmd EnableSendMsg;
        [RegisterCommand(typeof(DisableSendMsgListCommand))]
        public readonly GameCmd DisableSendMsgList;
        [RegisterCommand(typeof(KillCommand))]
        public readonly GameCmd Kill;
        [RegisterCommand(typeof(MakeItemCommond))]
        public readonly GameCmd Make;
        [RegisterCommand(typeof(SmakeItemCommand))]
        public readonly GameCmd Smake;
        [RegisterCommand(typeof(FireBurnCommand))]
        public readonly GameCmd FireBurn;
        [RegisterCommand(typeof(TestFireCommand))]
        public readonly GameCmd TestFire;
        [RegisterCommand(typeof(TestStatusCommand))]
        public readonly GameCmd TestStatus;
        [RegisterCommand(typeof(DelGoldCommand))]
        public readonly GameCmd DelGold;
        [RegisterCommand(typeof(AddGoldCommand))]
        public readonly GameCmd AddGold;
        [RegisterCommand(typeof(DelGameGoldCommand))]
        public readonly GameCmd DelGameGold;
        [RegisterCommand(typeof(AddGameGoldCommand))]
        public readonly GameCmd AddGameGold;
        [RegisterCommand(typeof(GameGoldCommand))]
        public readonly GameCmd GameGold;
        [RegisterCommand(typeof(GamePointCommand))]
        public readonly GameCmd GamePoint;
        [RegisterCommand(typeof(CreditPointCommand))]
        public readonly GameCmd CreditPoint;
        [RegisterCommand(typeof(RefineWeaponCommand))]
        public readonly GameCmd RefineWeapon;
        [RegisterCommand(typeof(ReLoadAdminCommand))]
        public readonly GameCmd ReloadAdmin;
        [RegisterCommand(typeof(ReloadNpcCommand))]
        public readonly GameCmd ReloadNpc;
        [RegisterCommand(typeof(ReloadManageCommand))]
        public readonly GameCmd ReloadManage;
        [RegisterCommand(typeof(ReloadRobotManageCommand))]
        public readonly GameCmd ReloadRobotManage;
        [RegisterCommand(typeof(ReloadRobotCommand))]
        public readonly GameCmd ReloadRobot;
        [RegisterCommand(typeof(ReloadMonItemsCommand))]
        public readonly GameCmd ReloadMonItems;
        [RegisterCommand(typeof(ReloadMagicDbCommand))]
        public readonly GameCmd ReloadMagicDb;
        [RegisterCommand(typeof(ReAliveCommand))]
        public readonly GameCmd ReaLive;
        [RegisterCommand(typeof(AdjuestLevelCommand))]
        public readonly GameCmd AdjuestLevel;
        [RegisterCommand(typeof(AddGuildCommand))]
        public readonly GameCmd AddGuild;
        [RegisterCommand(typeof(DelGuildCommand))]
        public readonly GameCmd DelGuild;
        [RegisterCommand(typeof(ChangeSabukLordCommand))]
        public readonly GameCmd ChangeSabukLord;
        [RegisterCommand(typeof(ForcedWallconquestWarCommand))]
        public readonly GameCmd ForcedWallConQuestWar;
        [RegisterCommand(typeof(TrainingSkillCommand))]
        public readonly GameCmd TrainingSkill;
        [RegisterCommand(typeof(ReloadAllGuildCommand))]
        public readonly GameCmd ReloadGuildAll;
        [RegisterCommand(typeof(ShowMapInfoCommand))]
        public readonly GameCmd MapInfo;
        [RegisterCommand(typeof(SbkDoorControlCommand))]
        public readonly GameCmd SbkDoor;
        [RegisterCommand(typeof(ChangeDearNameCommand))]
        public readonly GameCmd ChangeDearName;
        [RegisterCommand(typeof(ChangeMasterNameCommand))]
        public readonly GameCmd ChangeMasterName;
        [RegisterCommand(typeof(StartQuestCommand))]
        public readonly GameCmd StartQuest;
        [RegisterCommand(typeof(SetPermissionCommand))]
        public readonly GameCmd SetperMission;
        [RegisterCommand(typeof(ClearMapMonsterCommand))]
        public readonly GameCmd ClearMon;
        [RegisterCommand(typeof(ReNewLevelCommand))]
        public readonly GameCmd RenewLevel;
        [RegisterCommand(typeof(DenyIPaddrLogonCommand))]
        public readonly GameCmd DenyipLogon;
        [RegisterCommand(typeof(DenyAccountLogonCommand))]
        public readonly GameCmd DenyAccountLogon;
        [RegisterCommand(typeof(DenyChrNameLogonCommand))]
        public readonly GameCmd DenyChrNameLogon;
        [RegisterCommand(typeof(DelDenyIPaddrLogonCommand))]
        public readonly GameCmd DelDenyIpLogon;
        [RegisterCommand(typeof(DelDenyAccountLogonCommand))]
        public readonly GameCmd DelDenyAccountLogon;
        [RegisterCommand(typeof(DelDenyChrNameLogonCommand))]
        public readonly GameCmd DelDenyChrNameLogon;
        [RegisterCommand(typeof(ShowDenyIPaddrLogonCommand))]
        public readonly GameCmd ShowDenyIpLogon;
        [RegisterCommand(typeof(ShowDenyAccountLogonCommand))]
        public readonly GameCmd ShowDenyAccountLogon;
        [RegisterCommand(typeof(ShowDenyChrNameLogonCommand))]
        public readonly GameCmd ShowDenyChrNameLogon;
        [RegisterCommand(typeof(ViewWhisperCommand))]
        public readonly GameCmd ViewWhisper;
        [RegisterCommand(typeof(SpirtStartCommand))]
        public readonly GameCmd Spirit;
        [RegisterCommand(typeof(SpirtStopCommand))]
        public readonly GameCmd SpiritStop;
        [RegisterCommand(typeof(SetMapModeCommamd))]
        public readonly GameCmd SetMapMode;
        [RegisterCommand(typeof(ShowMapModeCommand))]
        public readonly GameCmd ShowMapMode;
        [RegisterCommand(typeof(ClearBagItemCommand))]
        public readonly GameCmd ClearBag;
        [RegisterCommand(typeof(LockLoginCommand))]
        public readonly GameCmd LockLogon;

        public GameCommands()
        {
            Data = new GameCmd { CmdName = "Data", PerMissionMin = 10, PerMissionMax = 10 };
            PrvMsg = new GameCmd { CmdName = "PrvMsg", PerMissionMin = 10, PerMissionMax = 10 };
            AllowMsg = new GameCmd { CmdName = "AllowMsg", PerMissionMin = 10, PerMissionMax = 10 };
            Letshout = new GameCmd { CmdName = "Letshout", PerMissionMin = 10, PerMissionMax = 10 };
            LetTrade = new GameCmd { CmdName = "LetTrade", PerMissionMin = 10, PerMissionMax = 10 };
            LetGuild = new GameCmd { CmdName = "LetGuild", PerMissionMin = 10, PerMissionMax = 10 };
            EndGuild = new GameCmd { CmdName = "EndGuild", PerMissionMin = 10, PerMissionMax = 10 };
            BanGuildChat = new GameCmd { CmdName = "BanGuildChat", PerMissionMin = 10, PerMissionMax = 10 };
            Authally = new GameCmd { CmdName = "Authally", PerMissionMin = 10, PerMissionMax = 10 };
            Auth = new GameCmd { CmdName = "Auth", PerMissionMin = 10, PerMissionMax = 10 };
            AuthCancel = new GameCmd { CmdName = "AuthCancel", PerMissionMin = 10, PerMissionMax = 10 };
            Diary = new GameCmd { CmdName = "Diary", PerMissionMin = 10, PerMissionMax = 10 };
            UserMove = new GameCmd { CmdName = "UserMove", PerMissionMin = 10, PerMissionMax = 10 };
            Searching = new GameCmd { CmdName = "Searching", PerMissionMin = 10, PerMissionMax = 10 };
            AllowGroupCall = new GameCmd { CmdName = "AllowGroupCall", PerMissionMin = 10, PerMissionMax = 10 };
            GroupRecalll = new GameCmd { CmdName = "GroupRecalll", PerMissionMin = 10, PerMissionMax = 10 };
            AllowGuildRecall = new GameCmd { CmdName = "AllowGuildRecall", PerMissionMin = 10, PerMissionMax = 10 };
            GuildRecalll = new GameCmd { CmdName = "GuildRecalll", PerMissionMin = 10, PerMissionMax = 10 };
            UnlockStorage = new GameCmd { CmdName = "UnlockStorage", PerMissionMin = 10, PerMissionMax = 10 };
            Unlock = new GameCmd { CmdName = "Unlock", PerMissionMin = 10, PerMissionMax = 10 };
            Lock = new GameCmd { CmdName = "Lock", PerMissionMin = 10, PerMissionMax = 10 };
            PasswordLock = new GameCmd { CmdName = "PasswordLock", PerMissionMin = 10, PerMissionMax = 10 };
            SetPassword = new GameCmd { CmdName = "SetPassword", PerMissionMin = 10, PerMissionMax = 10 };
            ChgPassword = new GameCmd { CmdName = "ChgPassword", PerMissionMin = 10, PerMissionMax = 10 };
            ClrPassword = new GameCmd { CmdName = "ClrPassword", PerMissionMin = 10, PerMissionMax = 10 };
            UnPassword = new GameCmd { CmdName = "UnPassword", PerMissionMin = 10, PerMissionMax = 10 };
            MemberFunction = new GameCmd { CmdName = "MemberFunction", PerMissionMin = 10, PerMissionMax = 10 };
            MemberFunctioneX = new GameCmd { CmdName = "MemberFunctioneX", PerMissionMin = 10, PerMissionMax = 10 };
            Dear = new GameCmd { CmdName = "Dear", PerMissionMin = 10, PerMissionMax = 10 };
            AllowDearRcall = new GameCmd { CmdName = "AllowDearRcall", PerMissionMin = 10, PerMissionMax = 10 };
            DearRecall = new GameCmd { CmdName = "DearRecall", PerMissionMin = 10, PerMissionMax = 10 };
            Master = new GameCmd { CmdName = "Master", PerMissionMin = 10, PerMissionMax = 10 };
            AllowMasterRecall = new GameCmd { CmdName = "AllowMasterRecall", PerMissionMin = 10, PerMissionMax = 10 };
            MasteRecall = new GameCmd { CmdName = "MasteRecall", PerMissionMin = 10, PerMissionMax = 10 };
            AttackMode = new GameCmd { CmdName = "AttackMode", PerMissionMin = 10, PerMissionMax = 10 };
            Rest = new GameCmd { CmdName = "Rest", PerMissionMin = 10, PerMissionMax = 10 };
            TakeonHorse = new GameCmd { CmdName = "TakeonHorse", PerMissionMin = 10, PerMissionMax = 10 };
            TakeofHorse = new GameCmd { CmdName = "TakeofHorse", PerMissionMin = 10, PerMissionMax = 10 };
            HumanLocal = new GameCmd { CmdName = "HumanLocal", PerMissionMin = 10, PerMissionMax = 10 };
            Move = new GameCmd { CmdName = "Move", PerMissionMin = 10, PerMissionMax = 10 };
            PositionMove = new GameCmd { CmdName = "PositionMove", PerMissionMin = 10, PerMissionMax = 10 };
            Info = new GameCmd { CmdName = "Info", PerMissionMin = 10, PerMissionMax = 10 };
            MobLevel = new GameCmd { CmdName = "MobLevel", PerMissionMin = 10, PerMissionMax = 10 };
            MobCount = new GameCmd { CmdName = "MobCount", PerMissionMin = 10, PerMissionMax = 10 };
            HumanCount = new GameCmd { CmdName = "HumanCount", PerMissionMin = 10, PerMissionMax = 10 };
            Map = new GameCmd { CmdName = "Map", PerMissionMin = 10, PerMissionMax = 10 };
            Kick = new GameCmd { CmdName = "Kick", PerMissionMin = 10, PerMissionMax = 10 };
            Ting = new GameCmd { CmdName = "Ting", PerMissionMin = 10, PerMissionMax = 10 };
            Superting = new GameCmd { CmdName = "Superting", PerMissionMin = 10, PerMissionMax = 10 };
            MapMove = new GameCmd { CmdName = "MapMove", PerMissionMin = 10, PerMissionMax = 10 };
            ShutUp = new GameCmd { CmdName = "ShutUp", PerMissionMin = 10, PerMissionMax = 10 };
            ReleaseShutup = new GameCmd { CmdName = "ReleaseShutup", PerMissionMin = 10, PerMissionMax = 10 };
            ShutupList = new GameCmd { CmdName = "ShutupList", PerMissionMin = 10, PerMissionMax = 10 };
            GameMaster = new GameCmd { CmdName = "GameMaster", PerMissionMin = 10, PerMissionMax = 10 };
            ObServer = new GameCmd { CmdName = "ObServer", PerMissionMin = 10, PerMissionMax = 10 };
            SueprMan = new GameCmd { CmdName = "SueprMan", PerMissionMin = 10, PerMissionMax = 10 };
            Level = new GameCmd { CmdName = "Level", PerMissionMin = 10, PerMissionMax = 10 };
            SabukWallGold = new GameCmd { CmdName = "SabukWallGold", PerMissionMin = 10, PerMissionMax = 10 };
            Recall = new GameCmd { CmdName = "Recall", PerMissionMin = 10, PerMissionMax = 10 };
            Regoto = new GameCmd { CmdName = "Regoto", PerMissionMin = 10, PerMissionMax = 10 };
            ShowFlag = new GameCmd { CmdName = "ShowFlag", PerMissionMin = 10, PerMissionMax = 10 };
            ShowOpen = new GameCmd { CmdName = "ShowOpen", PerMissionMin = 10, PerMissionMax = 10 };
            ShowUnit = new GameCmd { CmdName = "ShowUnit", PerMissionMin = 10, PerMissionMax = 10 };
            Attack = new GameCmd { CmdName = "Attack", PerMissionMin = 10, PerMissionMax = 10 };
            Mob = new GameCmd { CmdName = "Mob", PerMissionMin = 10, PerMissionMax = 10 };
            MobNpc = new GameCmd { CmdName = "MobNpc", PerMissionMin = 10, PerMissionMax = 10 };
            DeleteNpc = new GameCmd { CmdName = "DeleteNpc", PerMissionMin = 10, PerMissionMax = 10 };
            NpcScript = new GameCmd { CmdName = "NpcScript", PerMissionMin = 10, PerMissionMax = 10 };
            RecallMob = new GameCmd { CmdName = "RecallMob", PerMissionMin = 10, PerMissionMax = 10 };
            LuckyPoint = new GameCmd { CmdName = "LuckyPoint", PerMissionMin = 10, PerMissionMax = 10 };
            LotteryTicket = new GameCmd { CmdName = "Namecolor", PerMissionMin = 10, PerMissionMax = 10 };
            ReloadGuild = new GameCmd { CmdName = "ReloadGuild", PerMissionMin = 10, PerMissionMax = 10 };
            ReloadLineNotice = new GameCmd { CmdName = "ReloadLineNotice", PerMissionMin = 10, PerMissionMax = 10 };
            ReloadAbuse = new GameCmd { CmdName = "ReloadAbuse", PerMissionMin = 10, PerMissionMax = 10 };
            BackStep = new GameCmd { CmdName = "BackStep", PerMissionMin = 10, PerMissionMax = 10 };
            Ball = new GameCmd { CmdName = "Ball", PerMissionMin = 10, PerMissionMax = 10 };
            FreePenalty = new GameCmd { CmdName = "FreePenalty", PerMissionMin = 10, PerMissionMax = 10 };
            PkPoint = new GameCmd { CmdName = "PkPoint", PerMissionMin = 10, PerMissionMax = 10 };
            IncpkPoint = new GameCmd { CmdName = "IncpkPoint", PerMissionMin = 10, PerMissionMax = 10 };
            ChangeLuck = new GameCmd { CmdName = "ChangeLuck", PerMissionMin = 10, PerMissionMax = 10 };
            Hunger = new GameCmd { CmdName = "Hunger", PerMissionMin = 10, PerMissionMax = 10 };
            Hair = new GameCmd { CmdName = "Hair", PerMissionMin = 10, PerMissionMax = 10 };
            Training = new GameCmd { CmdName = "Training", PerMissionMin = 10, PerMissionMax = 10 };
            DeleteSkill = new GameCmd { CmdName = "DeleteSkill", PerMissionMin = 10, PerMissionMax = 10 };
            ChangeJob = new GameCmd { CmdName = "ChangeJob", PerMissionMin = 10, PerMissionMax = 10 };
            ChangeGender = new GameCmd { CmdName = "ChangeGender", PerMissionMin = 10, PerMissionMax = 10 };
            NameColor = new GameCmd { CmdName = "Namecolor", PerMissionMin = 10, PerMissionMax = 10 };
            Mission = new GameCmd { CmdName = "Mission", PerMissionMin = 10, PerMissionMax = 10 };
            MobPlace = new GameCmd { CmdName = "MobPlace", PerMissionMin = 10, PerMissionMax = 10 };
            Transparecy = new GameCmd { CmdName = "Transparecy", PerMissionMin = 10, PerMissionMax = 10 };
            DeleteItem = new GameCmd { CmdName = "DeleteItem", PerMissionMin = 10, PerMissionMax = 10 };
            Level = new GameCmd { CmdName = "Level", PerMissionMin = 10, PerMissionMax = 10 };
            ClearMission = new GameCmd { CmdName = "ClearMission", PerMissionMin = 10, PerMissionMax = 10 };
            SetFlag = new GameCmd { CmdName = "SetFlag", PerMissionMin = 10, PerMissionMax = 10 };
            SetOpen = new GameCmd { CmdName = "SetOpen", PerMissionMin = 10, PerMissionMax = 10 };
            SetUnit = new GameCmd { CmdName = "SetUnit", PerMissionMin = 10, PerMissionMax = 10 };
            Reconnection = new GameCmd { CmdName = "Reconnection", PerMissionMin = 10, PerMissionMax = 10 };
            DisableFilter = new GameCmd { CmdName = "DisableFilter", PerMissionMin = 10, PerMissionMax = 10 };
            ChguserFull = new GameCmd { CmdName = "KilChguserFulll", PerMissionMin = 10, PerMissionMax = 10 };
            ChgZenFastStep = new GameCmd { CmdName = "ChgZenFastStep", PerMissionMin = 10, PerMissionMax = 10 };
            ContestPoint = new GameCmd { CmdName = "ContestPoint", PerMissionMin = 10, PerMissionMax = 10 };
            StartContest = new GameCmd { CmdName = "StartContest", PerMissionMin = 10, PerMissionMax = 10 };
            EndContest = new GameCmd { CmdName = "EndContest", PerMissionMin = 10, PerMissionMax = 10 };
            Announcement = new GameCmd { CmdName = "Announcement", PerMissionMin = 10, PerMissionMax = 10 };
            Oxquizroom = new GameCmd { CmdName = "Oxquizroom", PerMissionMin = 10, PerMissionMax = 10 };
            Gsa = new GameCmd { CmdName = "Gsa", PerMissionMin = 10, PerMissionMax = 10 };
            ChangeItemName = new GameCmd { CmdName = "ChangeItemName", PerMissionMin = 10, PerMissionMax = 10 };
            DisableSendMsg = new GameCmd { CmdName = "DisableSendMsg", PerMissionMin = 10, PerMissionMax = 10 };
            EnableSendMsg = new GameCmd { CmdName = "EnableSendMsg", PerMissionMin = 10, PerMissionMax = 10 };
            DisableSendMsgList = new GameCmd { CmdName = "DisableSendMsgList", PerMissionMin = 10, PerMissionMax = 10 };
            Kill = new GameCmd { CmdName = "Kill", PerMissionMin = 10, PerMissionMax = 10 };
            Make = new GameCmd { CmdName = "Make", PerMissionMin = 10, PerMissionMax = 10 };
            Smake = new GameCmd { CmdName = "Smake", PerMissionMin = 10, PerMissionMax = 10 };
            BonusPoint = new GameCmd { CmdName = "BonusPoint", PerMissionMin = 10, PerMissionMax = 10 };
            DelBonusPoint = new GameCmd { CmdName = "DelBonusPoint", PerMissionMin = 10, PerMissionMax = 10 };
            Restbonuspoint = new GameCmd { CmdName = "Restbonuspoint", PerMissionMin = 10, PerMissionMax = 10 };
            FireBurn = new GameCmd { CmdName = "FireBurn", PerMissionMin = 10, PerMissionMax = 10 };
            TestFire = new GameCmd { CmdName = "TestFire", PerMissionMin = 10, PerMissionMax = 10 };
            TestStatus = new GameCmd { CmdName = "TestStatus", PerMissionMin = 10, PerMissionMax = 10 };
            DelGold = new GameCmd { CmdName = "DelGold", PerMissionMin = 10, PerMissionMax = 10 };
            AddGold = new GameCmd { CmdName = "AddGold", PerMissionMin = 10, PerMissionMax = 10 };
            DelGameGold = new GameCmd { CmdName = "DelGameGold", PerMissionMin = 10, PerMissionMax = 10 };
            AddGameGold = new GameCmd { CmdName = "AddGameGold", PerMissionMin = 10, PerMissionMax = 10 };
            GameGold = new GameCmd { CmdName = "GameGold", PerMissionMin = 10, PerMissionMax = 10 };
            GamePoint = new GameCmd { CmdName = "GamePoint", PerMissionMin = 10, PerMissionMax = 10 };
            CreditPoint = new GameCmd { CmdName = "CreditPoint", PerMissionMin = 10, PerMissionMax = 10 };
            Testgoldchange = new GameCmd { CmdName = "Testgoldchange", PerMissionMin = 10, PerMissionMax = 10 };
            RefineWeapon = new GameCmd { CmdName = "RefineWeapon", PerMissionMin = 10, PerMissionMax = 10 };
            ReloadAdmin = new GameCmd { CmdName = "ReloadAdmin", PerMissionMin = 10, PerMissionMax = 10 };
            ReloadNpc = new GameCmd { CmdName = "ReloadNpc", PerMissionMin = 10, PerMissionMax = 10 };
            ReloadManage = new GameCmd { CmdName = "ReloadManage", PerMissionMin = 10, PerMissionMax = 10 };
            ReloadRobotManage = new GameCmd { CmdName = "ReloadRobotManage", PerMissionMin = 10, PerMissionMax = 10 };
            ReloadRobot = new GameCmd { CmdName = "ReloadRobot", PerMissionMin = 10, PerMissionMax = 10 };
            ReloadMonItems = new GameCmd { CmdName = "ReloadMonItems", PerMissionMin = 10, PerMissionMax = 10 };
            Reloaddiary = new GameCmd { CmdName = "Reloaddiary", PerMissionMin = 10, PerMissionMax = 10 };
            ReloadItemDB = new GameCmd { CmdName = "ReloadItemDB", PerMissionMin = 10, PerMissionMax = 10 };
            ReloadMagicDb = new GameCmd { CmdName = "ReloadMagicDb", PerMissionMin = 10, PerMissionMax = 10 };
            Reloadmonsterdb = new GameCmd { CmdName = "Reloadmonsterdb", PerMissionMin = 10, PerMissionMax = 10 };
            Reloadminmap = new GameCmd { CmdName = "Reloadminmap", PerMissionMin = 10, PerMissionMax = 10 };
            ReaLive = new GameCmd { CmdName = "ReaLive", PerMissionMin = 10, PerMissionMax = 10 };
            AdjuestLevel = new GameCmd { CmdName = "AdjuestLevel", PerMissionMin = 10, PerMissionMax = 10 };
            AdjuestExp = new GameCmd { CmdName = "AdjuestExp", PerMissionMin = 10, PerMissionMax = 10 };
            AdjustExp = new GameCmd { CmdName = "AdjustExp", PerMissionMin = 10, PerMissionMax = 10 };
            AddGuild = new GameCmd { CmdName = "AddGuild", PerMissionMin = 10, PerMissionMax = 10 };
            DelGuild = new GameCmd { CmdName = "DelGuild", PerMissionMin = 10, PerMissionMax = 10 };
            ChangeSabukLord = new GameCmd { CmdName = "ChangeSabukLord", PerMissionMin = 10, PerMissionMax = 10 };
            ForcedWallConQuestWar = new GameCmd { CmdName = "ForcedWallConQuestWar", PerMissionMin = 10, PerMissionMax = 10 };
            Addtoitemevent = new GameCmd { CmdName = "Addtoitemevent", PerMissionMin = 10, PerMissionMax = 10 };
            Addtoitemeventaspieces = new GameCmd { CmdName = "Addtoitemeventaspieces", PerMissionMin = 10, PerMissionMax = 10 };
            ItemEventList = new GameCmd { CmdName = "Itemeventlist", PerMissionMin = 10, PerMissionMax = 10 };
            Startinggiftno = new GameCmd { CmdName = "Startinggiftno", PerMissionMin = 10, PerMissionMax = 10 };
            Deleteallitemevent = new GameCmd { CmdName = "Deleteallitemevent", PerMissionMin = 10, PerMissionMax = 10 };
            Startitemevent = new GameCmd { CmdName = "Startitemevent", PerMissionMin = 10, PerMissionMax = 10 };
            Itemeventterm = new GameCmd { CmdName = "Itemeventterm", PerMissionMin = 10, PerMissionMax = 10 };
            Adjuesttestlevel = new GameCmd { CmdName = "Adjuesttestlevel", PerMissionMin = 10, PerMissionMax = 10 };
            TrainingSkill = new GameCmd { CmdName = "TrainingSkill", PerMissionMin = 10, PerMissionMax = 10 };
            Opdeleteskill = new GameCmd { CmdName = "Opdeleteskill", PerMissionMin = 10, PerMissionMax = 10 };
            Changeweapondura = new GameCmd { CmdName = "Changeweapondura", PerMissionMin = 10, PerMissionMax = 10 };
            ReloadGuildAll = new GameCmd { CmdName = "ReloadGuildAll", PerMissionMin = 10, PerMissionMax = 10 };
            Who = new GameCmd { CmdName = "Who", PerMissionMin = 10, PerMissionMax = 10 };
            Total = new GameCmd { CmdName = "Total", PerMissionMin = 10, PerMissionMax = 10 };
            Testga = new GameCmd { CmdName = "Testga", PerMissionMin = 10, PerMissionMax = 10 };
            MapInfo = new GameCmd { CmdName = "MapInfo", PerMissionMin = 10, PerMissionMax = 10 };
            SbkDoor = new GameCmd { CmdName = "SbkDoor", PerMissionMin = 10, PerMissionMax = 10 };
            ChangeDearName = new GameCmd { CmdName = "DearName", PerMissionMin = 10, PerMissionMax = 10 };
            ChangeMasterName = new GameCmd { CmdName = "MasterName", PerMissionMax = 10, PerMissionMin = 10 };
            StartQuest = new GameCmd { CmdName = "StartQuest", PerMissionMax = 10, PerMissionMin = 10 };
            SetperMission = new GameCmd { CmdName = "SetperMission", PerMissionMax = 10, PerMissionMin = 10 };
            ClearMon = new GameCmd { CmdName = "ClearMon", PerMissionMax = 10, PerMissionMin = 10 };
            RenewLevel = new GameCmd { CmdName = "ReNewLevel", PerMissionMin = 10, PerMissionMax = 10 };
            DenyipLogon = new GameCmd { CmdName = "DenyipLogon", PerMissionMax = 10, PerMissionMin = 10 };
            DenyAccountLogon = new GameCmd { CmdName = "DenyAccountLogon", PerMissionMax = 10, PerMissionMin = 10 };
            DenyChrNameLogon = new GameCmd { CmdName = "DenyChrNameLogon", PerMissionMin = 10, PerMissionMax = 10 };
            DelDenyIpLogon = new GameCmd { CmdName = "DelDenyIpLogon", PerMissionMax = 10, PerMissionMin = 10 };
            DelDenyAccountLogon = new GameCmd { CmdName = "DelDenyAccountLogon", PerMissionMax = 10, PerMissionMin = 10 };
            DelDenyChrNameLogon = new GameCmd { CmdName = "DelDenyChrNameLogon", PerMissionMax = 10, PerMissionMin = 10 };
            ShowDenyIpLogon = new GameCmd { CmdName = "ShowDenyIpLogon", PerMissionMax = 10, PerMissionMin = 10 };
            ShowDenyAccountLogon = new GameCmd { CmdName = "ShowDenyAccountLogon", PerMissionMax = 10, PerMissionMin = 10 };
            ShowDenyChrNameLogon = new GameCmd { CmdName = "ShowDenyCharNameLogon", PerMissionMax = 10, PerMissionMin = 10 };
            ViewWhisper = new GameCmd { CmdName = "ViewWhisper", PerMissionMax = 10, PerMissionMin = 10 };
            Spirit = new GameCmd { CmdName = "Spirit", PerMissionMax = 10, PerMissionMin = 10 };
            SpiritStop = new GameCmd { CmdName = "SpiritStop", PerMissionMax = 10, PerMissionMin = 10 };
            SetMapMode = new GameCmd { CmdName = "SetMapMode", PerMissionMax = 10, PerMissionMin = 10 };
            ShowMapMode = new GameCmd { CmdName = "ShowMapMode", PerMissionMax = 10, PerMissionMin = 10 };
            Testserverconfig = new GameCmd { CmdName = "Testserverconfig", PerMissionMax = 10, PerMissionMin = 10 };
            Serverstatus = new GameCmd { CmdName = "Serverstatus", PerMissionMax = 10, PerMissionMin = 10 };
            Testgetbagitem = new GameCmd { CmdName = "Testgetbagitem", PerMissionMax = 10, PerMissionMin = 10 };
            ClearBag = new GameCmd { CmdName = "ClearBag", PerMissionMax = 10, PerMissionMin = 10 };
            ShowUseItemInfo = new GameCmd { CmdName = "Showuseiteminfo", PerMissionMax = 10, PerMissionMin = 10 };
            BindUseItem = new GameCmd { CmdName = "Binduseitem", PerMissionMax = 10, PerMissionMin = 10 };
            Mobfireburn = new GameCmd { CmdName = "Mobfireburn", PerMissionMax = 10, PerMissionMin = 10 };
            Testspeedmode = new GameCmd { CmdName = "Testspeedmode", PerMissionMax = 10, PerMissionMin = 10 };
            LockLogon = new GameCmd { CmdName = "LockLogon", PerMissionMax = 10, PerMissionMin = 10 };
        }
    }

    public class GameCmd
    {
        /// <summary>
        /// 命令名称
        /// </summary>
        public string CmdName { get; set; }
        /// <summary>
        /// 最小权限
        /// </summary>
        public byte PerMissionMin { get; set; }
        /// <summary>
        /// 最大权限
        /// </summary>
        public byte PerMissionMax { get; set; }

        public GameCmd()
        {

        }

        public GameCmd(string cmdName, byte perMissionMin, byte perMissionMax)
        {
            CmdName = cmdName;
            PerMissionMin = perMissionMin;
            PerMissionMax = perMissionMax;
        }
    }
}