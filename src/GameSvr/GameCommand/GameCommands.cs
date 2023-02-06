using GameSvr.GameCommand.Commands;

namespace GameSvr.GameCommand
{
    public class GameCommands
    {
        public readonly GameCmd Testserverconfig;
        public readonly GameCmd Serverstatus;
        public readonly GameCmd Testgetbagitem;
        public readonly GameCmd Showuseiteminfo;
        public readonly GameCmd Binduseitem;
        public readonly GameCmd Mobfireburn;
        public readonly GameCmd Testspeedmode;
        public readonly GameCmd Reloadminmap;
        public readonly GameCmd Attack;
        public readonly GameCmd Testgoldchange;
        public readonly GameCmd Diary;
        public readonly GameCmd Namecolor;
        public readonly GameCmd Ball;
        public readonly GameCmd ChangeLuck;
        public readonly GameCmd Transparecy;
        public readonly GameCmd Addtoitemevent;
        public readonly GameCmd Addtoitemeventaspieces;
        public readonly GameCmd Itemeventlist;
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

        [CommandHandle(typeof(WhoCommand))]
        public readonly GameCmd Who;
        [CommandHandle(typeof(TotalCommand))]
        public readonly GameCmd Total;
        [CommandHandle((typeof(ReloadGuildCommand)))]
        public readonly GameCmd ReloadGuild;
        [CommandHandle(typeof(SearchMasterCommand))]
        public readonly GameCmd Master;
        [CommandHandle(typeof(SearchDearCommand))]
        public readonly GameCmd Dear;
        [CommandHandle(typeof(NpcScriptCommand))]
        public readonly GameCmd NpcScript;
        [CommandHandle(typeof(GroupRecallCommand))]
        public readonly GameCmd GroupRecalll;
        [CommandHandle(typeof(GuildRecallCommand))]
        public readonly GameCmd GuildRecalll;
        [CommandHandle(typeof(SearchHumanCommand))]
        public readonly GameCmd Searching;
        [CommandHandle(typeof(RecallMobCommand))]
        public readonly GameCmd RecallMob;
        [CommandHandle(typeof(ChangeSalveStatusCommand))]
        public readonly GameCmd Rest;
        [CommandHandle(typeof(EndGuildCommand))]
        public readonly GameCmd EndGuild;
        [CommandHandle(typeof(AuthallyCommand))]
        public readonly GameCmd Authally;
        [CommandHandle(typeof(LetGuildCommand))]
        public readonly GameCmd LetGuild;
        [CommandHandle(typeof(BanGuildChatCommand))]
        public readonly GameCmd BanGuildChat;
        [CommandHandle(typeof(LetTradeCommand))]
        public readonly GameCmd LetTrade;
        [CommandHandle(typeof(LetShoutCommand))]
        public readonly GameCmd Letshout;
        [CommandHandle(typeof(AllowMsgCommand))]
        public readonly GameCmd AllowMsg;
        [CommandHandle(typeof(ShowHumanUnitOpenCommand))]
        public readonly GameCmd ShowOpen;
        [CommandHandle(typeof(ShowUnitCommand))]
        public readonly GameCmd ShowUnit;
        [CommandHandle(typeof(AllowGuildRecallCommand))]
        public readonly GameCmd AllowGuildRecall;
        [CommandHandle(typeof(DelBonuPointCommand))]
        public readonly GameCmd DelBonusPoint;
        [CommandHandle(typeof(BonuPointCommand))]
        public readonly GameCmd BonusPoint;
        [CommandHandle(typeof(ShowSbkGoldCommand))]
        public readonly GameCmd SabukWallGold;
        [CommandHandle(typeof(MapInfoCommand))]
        public readonly GameCmd Info;
        [CommandHandle(typeof(SetPassWordCommand))]
        public readonly GameCmd SetPassword;
        [CommandHandle(typeof(ChgpassWordCommand))]
        public readonly GameCmd ChgPassword;
        [CommandHandle(typeof(ClearHumanPasswordCommand))]
        public readonly GameCmd ClrPassword;
        [CommandHandle(typeof(UnPasswWordCommand))]
        public readonly GameCmd UnPassword;
        [CommandHandle(typeof(UnlockStorageCommand))]
        public readonly GameCmd UnlockStorage;
        [CommandHandle(typeof(UnLockCommand))]
        public readonly GameCmd Unlock;
        [CommandHandle(typeof(LockCommand))]
        public readonly GameCmd Lock;
        [CommandHandle(typeof(SetFlagCommand))]
        public readonly GameCmd SetFlag;
        [CommandHandle(typeof(SetOpenCommand))]
        public readonly GameCmd SetOpen;
        [CommandHandle(typeof(SetUnitCommand))]
        public readonly GameCmd SetUnit;
        [CommandHandle(typeof(PasswordLockCommand))]
        public readonly GameCmd PasswordLock;
        [CommandHandle(typeof(AuthCancelCommand))]
        public readonly GameCmd AuthCancel;
        [CommandHandle(typeof(AuthCommand))]
        public readonly GameCmd Auth;
        [CommandHandle(typeof(DataCommand))]
        public readonly GameCmd Data;
        [CommandHandle(typeof(PrvMsgCommand))]
        public readonly GameCmd PrvMsg;
        [CommandHandle(typeof(UserMoveXYCommand))]
        public readonly GameCmd UserMove;
        [CommandHandle(typeof(AllowGroupReCallCommand))]
        public readonly GameCmd AllowGroupCall;
        [CommandHandle(typeof(MemberFunctionCommand))]
        public readonly GameCmd MemberFunction;
        [CommandHandle(typeof(MemberFunctionExCommand))]
        public readonly GameCmd MemberFunctioneX;
        [CommandHandle(typeof(AllowDearRecallCommand))]
        public readonly GameCmd AllowDearRcall;
        [CommandHandle(typeof(DearRecallCommond))]
        public readonly GameCmd DearRecall;
        [CommandHandle(typeof(AllowMasterRecallCommand))]
        public readonly GameCmd AllowMasterRecall;
        [CommandHandle(typeof(MasterRecallCommand))]
        public readonly GameCmd MasteRecall;
        [CommandHandle(typeof(ChangeAttackModeCommand))]
        public readonly GameCmd AttackMode;
        [CommandHandle(typeof(TakeOnHorseCommand))]
        public readonly GameCmd TakeonHorse;
        [CommandHandle(typeof(TakeOffHorseCommand))]
        public readonly GameCmd TakeofHorse;
        [CommandHandle(typeof(HumanLocalCommand))]
        public readonly GameCmd HumanLocal;
        [CommandHandle(typeof(PositionMoveCommand))]
        public readonly GameCmd Move;
        [CommandHandle(typeof(PositionMoveCommand))]
        public readonly GameCmd PositionMove;
        [CommandHandle(typeof(MobLevelCommand))]
        public readonly GameCmd MobLevel;
        [CommandHandle(typeof(MobCountCommand))]
        public readonly GameCmd MobCount;
        [CommandHandle(typeof(HumanCountCommand))]
        public readonly GameCmd HumanCount;
        [CommandHandle(typeof(ShowMapInfoCommand))]
        public readonly GameCmd Map;
        [CommandHandle(typeof(KickHumanCommand))]
        public readonly GameCmd Kick;
        [CommandHandle(typeof(TingCommand))]
        public readonly GameCmd Ting;
        [CommandHandle(typeof(SuperTingCommand))]
        public readonly GameCmd Superting;
        [CommandHandle(typeof(MapMoveCommand))]
        public readonly GameCmd MapMove;
        [CommandHandle(typeof(ShutupCommand))]
        public readonly GameCmd ShutUp;
        [CommandHandle(typeof(ShutupReleaseCommand))]
        public readonly GameCmd ReleaseShutup;
        [CommandHandle(typeof(ShutupListCommand))]
        public readonly GameCmd ShutupList;
        [CommandHandle(typeof(ChangeAdminModeCommand))]
        public readonly GameCmd GameMaster;
        [CommandHandle(typeof(ChangeObModeCommand))]
        public readonly GameCmd ObServer;
        [CommandHandle(typeof(ChangeSuperManModeCommand))]
        public readonly GameCmd SueprMan;
        [CommandHandle(typeof(ChangeLevelCommand))]
        public readonly GameCmd Level;
        [CommandHandle(typeof(ShowHumanFlagCommand))]
        public readonly GameCmd ShowFlag;
        [CommandHandle(typeof(MobCommand))]
        public readonly GameCmd Mob;
        [CommandHandle(typeof(MobNpcCommand))]
        public readonly GameCmd MobNpc;
        [CommandHandle(typeof(DelNpcCommand))]
        public readonly GameCmd DeleteNpc;
        [CommandHandle(typeof(LuckPointCommand))]
        public readonly GameCmd LuckyPoint;
        [CommandHandle(typeof(LotteryTicketCommandL))]
        public readonly GameCmd LotteryTicket;
        [CommandHandle(typeof(ReloadLineNoticeCommand))]
        public readonly GameCmd ReloadLineNotice;
        [CommandHandle(typeof(ReloadAbuseCommand))]
        public readonly GameCmd ReloadAbuse;
        [CommandHandle(typeof(BackStepCommand))]
        public readonly GameCmd BackStep;
        [CommandHandle(typeof(FreePenaltyCommand))]
        public readonly GameCmd FreePenalty;
        [CommandHandle(typeof(PKpointCommand))]
        public readonly GameCmd PkPoint;
        [CommandHandle(typeof(IncPkPointCommand))]
        public readonly GameCmd IncpkPoint;
        [CommandHandle(typeof(HungerCommand))]
        public readonly GameCmd Hunger;
        [CommandHandle(typeof(HairCommand))]
        public readonly GameCmd Hair;
        [CommandHandle(typeof(TrainingCommand))]
        public readonly GameCmd Training;
        [CommandHandle(typeof(DelSkillCommand))]
        public readonly GameCmd DeleteSkill;
        [CommandHandle(typeof(ChangeJobCommand))]
        public readonly GameCmd ChangeJob;
        [CommandHandle(typeof(ChangeGenderCommand))]
        public readonly GameCmd ChangeGender;
        [CommandHandle(typeof(MissionCommand))]
        public readonly GameCmd Mission;
        [CommandHandle(typeof(MobPlaceCommand))]
        public readonly GameCmd MobPlace;
        [CommandHandle(typeof(DeleteItemCommand))]
        public readonly GameCmd DeleteItem;
        [CommandHandle(typeof(ClearMissionCommand))]
        public readonly GameCmd ClearMission;
        [CommandHandle(typeof(ReconnectionCommand))]
        public readonly GameCmd Reconnection;
        [CommandHandle(typeof(DisableFilterCommand))]
        public readonly GameCmd DisableFilter;
        [CommandHandle(typeof(ChangeUserFullCommand))]
        public readonly GameCmd ChguserFull;
        [CommandHandle(typeof(ChangeZenFastStepCommand))]
        public readonly GameCmd ChgZenFastStep;
        [CommandHandle(typeof(ContestPointCommand))]
        public readonly GameCmd ContestPoint;
        [CommandHandle(typeof(StartContestCommand))]
        public readonly GameCmd StartContest;
        [CommandHandle(typeof(EndContestCommand))]
        public readonly GameCmd EndContest;
        [CommandHandle(typeof(AnnouncementCommand))]
        public readonly GameCmd Announcement;
        [CommandHandle(typeof(ChangeItemNameCommand))]
        public readonly GameCmd ChangeItemName;
        [CommandHandle(typeof(DisableSendMsgCommand))]
        public readonly GameCmd DisableSendMsg;
        [CommandHandle(typeof(EnableSendMsgCommand))]
        public readonly GameCmd EnableSendMsg;
        [CommandHandle(typeof(DisableSendMsgListCommand))]
        public readonly GameCmd DisableSendMsgList;
        [CommandHandle(typeof(KillCommand))]
        public readonly GameCmd Kill;
        [CommandHandle(typeof(MakeItemCommond))]
        public readonly GameCmd Make;
        [CommandHandle(typeof(SmakeItemCommand))]
        public readonly GameCmd Smake;
        [CommandHandle(typeof(FireBurnCommand))]
        public readonly GameCmd FireBurn;
        [CommandHandle(typeof(TestFireCommand))]
        public readonly GameCmd TestFire;
        [CommandHandle(typeof(TestStatusCommand))]
        public readonly GameCmd TestStatus;
        [CommandHandle(typeof(DelGoldCommand))]
        public readonly GameCmd DelGold;
        [CommandHandle(typeof(AddGoldCommand))]
        public readonly GameCmd AddGold;
        [CommandHandle(typeof(DelGameGoldCommand))]
        public readonly GameCmd DelGameGold;
        [CommandHandle(typeof(AddGameGoldCommand))]
        public readonly GameCmd AddGameGold;
        [CommandHandle(typeof(GameGoldCommand))]
        public readonly GameCmd GameGold;
        [CommandHandle(typeof(GamePointCommand))]
        public readonly GameCmd GamePoint;
        [CommandHandle(typeof(CreditPointCommand))]
        public readonly GameCmd CreditPoint;
        [CommandHandle(typeof(RefineWeaponCommand))]
        public readonly GameCmd RefineWeapon;
        [CommandHandle(typeof(ReLoadAdminCommand))]
        public readonly GameCmd ReloadAdmin;
        [CommandHandle(typeof(ReloadNpcCommand))]
        public readonly GameCmd ReloadNpc;
        [CommandHandle(typeof(ReloadManageCommand))]
        public readonly GameCmd ReloadManage;
        [CommandHandle(typeof(ReloadManageCommand))]
        public readonly GameCmd ReloadRobotManage;
        [CommandHandle(typeof(ReloadRobotCommand))]
        public readonly GameCmd ReloadRobot;
        [CommandHandle(typeof(ReloadMonItemsCommand))]
        public readonly GameCmd ReloadMonItems;
        [CommandHandle(typeof(ReloadMagicDBCommand))]
        public readonly GameCmd ReloadMagicDb;
        [CommandHandle(typeof(ReAliveCommand))]
        public readonly GameCmd ReaLive;
        [CommandHandle(typeof(AdjuestLevelCommand))]
        public readonly GameCmd AdjuestLevel;
        [CommandHandle(typeof(AdjuestExpCommand))]
        public readonly GameCmd AdjuestExp;
        [CommandHandle(typeof(AddGuildCommand))]
        public readonly GameCmd AddGuild;
        [CommandHandle(typeof(DelGuildCommand))]
        public readonly GameCmd DelGuild;
        [CommandHandle(typeof(ChangeSabukLordCommand))]
        public readonly GameCmd ChangeSabukLord;
        [CommandHandle(typeof(ForcedWallconquestWarCommand))]
        public readonly GameCmd ForcedWallConQuestWar;
        [CommandHandle(typeof(TrainingSkillCommand))]
        public readonly GameCmd TrainingSkill;
        [CommandHandle(typeof(ReloadAllGuildCommand))]
        public readonly GameCmd ReloadGuildAll;
        [CommandHandle(typeof(ShowMapInfoCommand))]
        public readonly GameCmd MapInfo;
        [CommandHandle(typeof(SbkDoorControlCommand))]
        public readonly GameCmd SbkDoor;
        [CommandHandle(typeof(ChangeDearNameCommand))]
        public readonly GameCmd ChangeDearName;
        [CommandHandle(typeof(ChangeMasterNameCommand))]
        public readonly GameCmd ChangeMasterName;
        [CommandHandle(typeof(StartQuestCommand))]
        public readonly GameCmd StartQuest;
        [CommandHandle(typeof(SetPermissionCommand))]
        public readonly GameCmd SetperMission;
        [CommandHandle(typeof(ClearMapMonsterCommand))]
        public readonly GameCmd ClearMon;
        [CommandHandle(typeof(ReNewLevelCommand))]
        public readonly GameCmd RenewLevel;
        [CommandHandle(typeof(DenyIPaddrLogonCommand))]
        public readonly GameCmd DenyipLogon;
        [CommandHandle(typeof(DenyAccountLogonCommand))]
        public readonly GameCmd DenyAccountLogon;
        [CommandHandle(typeof(DenyChrNameLogonCommand))]
        public readonly GameCmd DenyChrNameLogon;
        [CommandHandle(typeof(DelDenyIPaddrLogonCommand))]
        public readonly GameCmd DelDenyIpLogon;
        [CommandHandle(typeof(DelDenyAccountLogonCommand))]
        public readonly GameCmd DelDenyAccountLogon;
        [CommandHandle(typeof(DelDenyChrNameLogonCommand))]
        public readonly GameCmd DelDenyChrNameLogon;
        [CommandHandle(typeof(ShowDenyIPaddrLogonCommand))]
        public readonly GameCmd ShowDenyIpLogon;
        [CommandHandle(typeof(ShowDenyAccountLogonCommand))]
        public readonly GameCmd ShowDenyAccountLogon;
        [CommandHandle(typeof(ShowDenyChrNameLogonCommand))]
        public readonly GameCmd ShowDenyChrNameLogon;
        [CommandHandle(typeof(ViewWhisperCommand))]
        public readonly GameCmd ViewWhisper;
        [CommandHandle(typeof(SpirtStartCommand))]
        public readonly GameCmd Spirit;
        [CommandHandle(typeof(SpirtStopCommand))]
        public readonly GameCmd SpiritStop;
        [CommandHandle(typeof(SetMapModeCommamd))]
        public readonly GameCmd SetMapMode;
        [CommandHandle(typeof(ShowMapModeCommand))]
        public readonly GameCmd ShowMapMode;
        [CommandHandle(typeof(ClearBagItemCommand))]
        public readonly GameCmd ClearBag;
        [CommandHandle(typeof(LockLoginCommand))]
        public readonly GameCmd LockLogon;

        public GameCommands()
        {
            Data = new GameCmd();
            PrvMsg = new GameCmd();
            AllowMsg = new GameCmd();
            Letshout = new GameCmd();
            LetTrade = new GameCmd();
            LetGuild = new GameCmd();
            EndGuild = new GameCmd();
            BanGuildChat = new GameCmd();
            Authally = new GameCmd();
            Auth = new GameCmd();
            AuthCancel = new GameCmd();
            Diary = new GameCmd();
            UserMove = new GameCmd();
            Searching = new GameCmd();
            AllowGroupCall = new GameCmd();
            GroupRecalll = new GameCmd();
            AllowGuildRecall = new GameCmd();
            GuildRecalll = new GameCmd();
            UnlockStorage = new GameCmd();
            Unlock = new GameCmd();
            Lock = new GameCmd();
            PasswordLock = new GameCmd();
            SetPassword = new GameCmd();
            ChgPassword = new GameCmd();
            ClrPassword = new GameCmd();
            UnPassword = new GameCmd();
            MemberFunction = new GameCmd();
            MemberFunctioneX = new GameCmd();
            Dear = new GameCmd();
            AllowDearRcall = new GameCmd();
            DearRecall = new GameCmd();
            Master = new GameCmd();
            AllowMasterRecall = new GameCmd();
            MasteRecall = new GameCmd();
            AttackMode = new GameCmd();
            Rest = new GameCmd();
            TakeonHorse = new GameCmd();
            TakeofHorse = new GameCmd();
            HumanLocal = new GameCmd();
            Move = new GameCmd();
            PositionMove = new GameCmd();
            Info = new GameCmd();
            MobLevel = new GameCmd();
            MobCount = new GameCmd();
            HumanCount = new GameCmd();
            Map = new GameCmd();
            Kick = new GameCmd();
            Ting = new GameCmd();
            Superting = new GameCmd();
            MapMove = new GameCmd();
            ShutUp = new GameCmd();
            ReleaseShutup = new GameCmd();
            ShutupList = new GameCmd();
            GameMaster = new GameCmd();
            ObServer = new GameCmd();
            SueprMan = new GameCmd();
            Level = new GameCmd();
            SabukWallGold = new GameCmd();
            Recall = new GameCmd();
            Regoto = new GameCmd();
            ShowFlag = new GameCmd();
            ShowOpen = new GameCmd();
            ShowUnit = new GameCmd();
            Attack = new GameCmd();
            Mob = new GameCmd();
            MobNpc = new GameCmd();
            DeleteNpc = new GameCmd();
            NpcScript = new GameCmd();
            RecallMob = new GameCmd();
            LuckyPoint = new GameCmd();
            LotteryTicket = new GameCmd();
            ReloadGuild = new GameCmd();
            ReloadLineNotice = new GameCmd();
            ReloadAbuse = new GameCmd();
            BackStep = new GameCmd();
            Ball = new GameCmd();
            FreePenalty = new GameCmd();
            PkPoint = new GameCmd();
            IncpkPoint = new GameCmd();
            ChangeLuck = new GameCmd();
            Hunger = new GameCmd();
            Hair = new GameCmd();
            Training = new GameCmd();
            DeleteSkill = new GameCmd();
            ChangeJob = new GameCmd();
            ChangeGender = new GameCmd();
            Namecolor = new GameCmd();
            Mission = new GameCmd();
            MobPlace = new GameCmd();
            Transparecy = new GameCmd();
            DeleteItem = new GameCmd();
            Level = new GameCmd();
            ClearMission = new GameCmd();
            SetFlag = new GameCmd();
            SetOpen = new GameCmd();
            SetUnit = new GameCmd();
            Reconnection = new GameCmd();
            DisableFilter = new GameCmd();
            ChguserFull = new GameCmd();
            ChgZenFastStep = new GameCmd();
            ContestPoint = new GameCmd();
            StartContest = new GameCmd();
            EndContest = new GameCmd();
            Announcement = new GameCmd();
            Oxquizroom = new GameCmd();
            Gsa = new GameCmd();
            ChangeItemName = new GameCmd();
            DisableSendMsg = new GameCmd();
            EnableSendMsg = new GameCmd();
            DisableSendMsgList = new GameCmd();
            Kill = new GameCmd();
            Make = new GameCmd();
            Smake = new GameCmd();
            BonusPoint = new GameCmd();
            DelBonusPoint = new GameCmd();
            Restbonuspoint = new GameCmd();
            FireBurn = new GameCmd();
            TestFire = new GameCmd();
            TestStatus = new GameCmd();
            DelGold = new GameCmd();
            AddGold = new GameCmd();
            DelGameGold = new GameCmd();
            AddGameGold = new GameCmd();
            GameGold = new GameCmd();
            GamePoint = new GameCmd();
            CreditPoint = new GameCmd();
            Testgoldchange = new GameCmd();
            RefineWeapon = new GameCmd();
            ReloadAdmin = new GameCmd();
            ReloadNpc = new GameCmd();
            ReloadManage = new GameCmd();
            ReloadRobotManage = new GameCmd();
            ReloadRobot = new GameCmd();
            ReloadMonItems = new GameCmd();
            Reloaddiary = new GameCmd();
            ReloadItemDB = new GameCmd();
            ReloadMagicDb = new GameCmd();
            Reloadmonsterdb = new GameCmd();
            Reloadminmap = new GameCmd();
            ReaLive = new GameCmd();
            AdjuestLevel = new GameCmd();
            AdjuestExp = new GameCmd();
            AddGuild = new GameCmd();
            DelGuild = new GameCmd();
            ChangeSabukLord = new GameCmd();
            ForcedWallConQuestWar = new GameCmd();
            Addtoitemevent = new GameCmd();
            Addtoitemeventaspieces = new GameCmd();
            Itemeventlist = new GameCmd();
            Startinggiftno = new GameCmd();
            Deleteallitemevent = new GameCmd();
            Startitemevent = new GameCmd();
            Itemeventterm = new GameCmd();
            Adjuesttestlevel = new GameCmd();
            TrainingSkill = new GameCmd();
            Opdeleteskill = new GameCmd();
            Changeweapondura = new GameCmd();
            ReloadGuildAll = new GameCmd();
            Who = new GameCmd();
            Total = new GameCmd();
            Testga = new GameCmd();
            MapInfo = new GameCmd();
            SbkDoor = new GameCmd();
            ChangeDearName = new GameCmd();
            ChangeMasterName = new GameCmd();
            StartQuest = new GameCmd();
            SetperMission = new GameCmd();
            ClearMon = new GameCmd();
            RenewLevel = new GameCmd();
            DenyipLogon = new GameCmd();
            DenyAccountLogon = new GameCmd();
            DenyChrNameLogon = new GameCmd();
            DelDenyIpLogon = new GameCmd();
            DelDenyAccountLogon = new GameCmd();
            DelDenyChrNameLogon = new GameCmd();
            ShowDenyIpLogon = new GameCmd();
            ShowDenyAccountLogon = new GameCmd();
            ShowDenyChrNameLogon = new GameCmd();
            ViewWhisper = new GameCmd();
            Spirit = new GameCmd();
            SpiritStop = new GameCmd();
            SetMapMode = new GameCmd();
            ShowMapMode = new GameCmd();
            Testserverconfig = new GameCmd();
            Serverstatus = new GameCmd();
            Testgetbagitem = new GameCmd();
            ClearBag = new GameCmd();
            Showuseiteminfo = new GameCmd();
            Binduseitem = new GameCmd();
            Mobfireburn = new GameCmd();
            Testspeedmode = new GameCmd();
            LockLogon = new GameCmd();
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
    }
}