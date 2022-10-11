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

        [CustomCommand(typeof(WhoCommand))]
        public readonly GameCmd Who;
        [CustomCommand(typeof(TotalCommand))]
        public readonly GameCmd Total;
        [CustomCommand((typeof(ReloadGuildCommand)))]
        public readonly GameCmd ReloadGuild;
        [CustomCommand(typeof(SearchMasterCommand))]
        public readonly GameCmd Master;
        [CustomCommand(typeof(SearchDearCommand))]
        public readonly GameCmd Dear;
        [CustomCommand(typeof(NpcScriptCommand))]
        public readonly GameCmd NpcScript;
        [CustomCommand(typeof(GroupRecallCommand))]
        public readonly GameCmd GroupRecalll;
        [CustomCommand(typeof(GuildRecallCommand))]
        public readonly GameCmd GuildRecalll;
        [CustomCommand(typeof(SearchHumanCommand))]
        public readonly GameCmd Searching;
        [CustomCommand(typeof(RecallMobCommand))]
        public readonly GameCmd RecallMob;
        [CustomCommand(typeof(ChangeSalveStatusCommand))]
        public readonly GameCmd Rest;
        [CustomCommand(typeof(EndGuildCommand))]
        public readonly GameCmd Endguild;
        [CustomCommand(typeof(AuthallyCommand))]
        public readonly GameCmd Authally;
        [CustomCommand(typeof(LetGuildCommand))]
        public readonly GameCmd Letguild;
        [CustomCommand(typeof(BanGuildChatCommand))]
        public readonly GameCmd BanGuildChat;
        [CustomCommand(typeof(LetTradeCommand))]
        public readonly GameCmd LetTrade;
        [CustomCommand(typeof(LetShoutCommand))]
        public readonly GameCmd Letshout;
        [CustomCommand(typeof(AllowMsgCommand))]
        public readonly GameCmd AllowMsg;
        [CustomCommand(typeof(ShowHumanUnitOpenCommand))]
        public readonly GameCmd ShowOpen;
        [CustomCommand(typeof(ShowUnitCommand))]
        public readonly GameCmd ShowUnit;
        [CustomCommand(typeof(AllowGuildRecallCommand))]
        public readonly GameCmd AllowGuildRecall;
        [CustomCommand(typeof(DelBonuPointCommand))]
        public readonly GameCmd DelBonusPoint;
        [CustomCommand(typeof(BonuPointCommand))]
        public readonly GameCmd BonusPoint;
        [CustomCommand(typeof(ShowSbkGoldCommand))]
        public readonly GameCmd SabukwallGold;
        [CustomCommand(typeof(MapInfoCommand))]
        public readonly GameCmd Info;
        [CustomCommand(typeof(SetPassWordCommand))]
        public readonly GameCmd SetPassword;
        [CustomCommand(typeof(ChgpassWordCommand))]
        public readonly GameCmd ChgPassword;
        [CustomCommand(typeof(ClearHumanPasswordCommand))]
        public readonly GameCmd ClrPassword;
        [CustomCommand(typeof(UnPasswWordCommand))]
        public readonly GameCmd UnPassword;
        [CustomCommand(typeof(UnlockStorageCommand))]
        public readonly GameCmd UnlockStorage;
        [CustomCommand(typeof(UnLockCommand))]
        public readonly GameCmd Unlock;
        [CustomCommand(typeof(LockCommand))]
        public readonly GameCmd Lock;
        [CustomCommand(typeof(SetFlagCommand))]
        public readonly GameCmd SetFlag;
        [CustomCommand(typeof(SetOpenCommand))]
        public readonly GameCmd SetOpen;
        [CustomCommand(typeof(SetUnitCommand))]
        public readonly GameCmd SetUnit;
        [CustomCommand(typeof(PasswordLockCommand))]
        public readonly GameCmd PasswordLock;
        [CustomCommand(typeof(AuthCancelCommand))]
        public readonly GameCmd AuthCancel;
        [CustomCommand(typeof(AuthCommand))]
        public readonly GameCmd Auth;
        [CustomCommand(typeof(DataCommand))]
        public readonly GameCmd Data;
        [CustomCommand(typeof(PrvMsgCommand))]
        public readonly GameCmd Prvmsg;
        [CustomCommand(typeof(UserMoveXYCommand))]
        public readonly GameCmd UserMove;
        [CustomCommand(typeof(AllowGroupReCallCommand))]
        public readonly GameCmd AllowGroupCall;
        [CustomCommand(typeof(MemberFunctionCommand))]
        public readonly GameCmd MemberFunction;
        [CustomCommand(typeof(MemberFunctionExCommand))]
        public readonly GameCmd MemberFunctioneX;
        [CustomCommand(typeof(AllowDearRecallCommand))]
        public readonly GameCmd AllowDearRcall;
        [CustomCommand(typeof(DearRecallCommond))]
        public readonly GameCmd DearRecall;
        [CustomCommand(typeof(AllowMasterRecallCommand))]
        public readonly GameCmd AllowMasterRecall;
        [CustomCommand(typeof(MasterRecallCommand))]
        public readonly GameCmd MasteRecall;
        [CustomCommand(typeof(ChangeAttackModeCommand))]
        public readonly GameCmd AttackMode;
        [CustomCommand(typeof(TakeOnHorseCommand))]
        public readonly GameCmd TakeonHorse;
        [CustomCommand(typeof(TakeOffHorseCommand))]
        public readonly GameCmd TakeofHorse;
        [CustomCommand(typeof(HumanLocalCommand))]
        public readonly GameCmd HumanLocal;
        [CustomCommand(typeof(PositionMoveCommand))]
        public readonly GameCmd Move;
        [CustomCommand(typeof(PositionMoveCommand))]
        public readonly GameCmd PositionMove;
        [CustomCommand(typeof(MobLevelCommand))]
        public readonly GameCmd MobLevel;
        [CustomCommand(typeof(MobCountCommand))]
        public readonly GameCmd MobCount;
        [CustomCommand(typeof(HumanCountCommand))]
        public readonly GameCmd HumanCount;
        [CustomCommand(typeof(ShowMapInfoCommand))]
        public readonly GameCmd Map;
        [CustomCommand(typeof(KickHumanCommand))]
        public readonly GameCmd Kick;
        [CustomCommand(typeof(TingCommand))]
        public readonly GameCmd Ting;
        [CustomCommand(typeof(SuperTingCommand))]
        public readonly GameCmd Superting;
        [CustomCommand(typeof(MapMoveCommand))]
        public readonly GameCmd MapMove;
        [CustomCommand(typeof(ShutupCommand))]
        public readonly GameCmd ShutUp;
        [CustomCommand(typeof(ShutupReleaseCommand))]
        public readonly GameCmd ReleaseShutup;
        [CustomCommand(typeof(ShutupListCommand))]
        public readonly GameCmd ShutupList;
        [CustomCommand(typeof(ChangeAdminModeCommand))]
        public readonly GameCmd GameMaster;
        [CustomCommand(typeof(ChangeObModeCommand))]
        public readonly GameCmd ObServer;
        [CustomCommand(typeof(ChangeSuperManModeCommand))]
        public readonly GameCmd SueprMan;
        [CustomCommand(typeof(ChangeLevelCommand))]
        public readonly GameCmd Level;
        [CustomCommand(typeof(ShowHumanFlagCommand))]
        public readonly GameCmd Showflag;
        [CustomCommand(typeof(MobCommand))]
        public readonly GameCmd Mob;
        [CustomCommand(typeof(MobNpcCommand))]
        public readonly GameCmd MobNpc;
        [CustomCommand(typeof(DelNpcCommand))]
        public readonly GameCmd DeleteNpc;
        [CustomCommand(typeof(LuckPointCommand))]
        public readonly GameCmd LuckyPoint;
        [CustomCommand(typeof(LotteryTicketCommandL))]
        public readonly GameCmd LotteryTicket;
        [CustomCommand(typeof(ReloadLineNoticeCommand))]
        public readonly GameCmd ReloadLineNotice;
        [CustomCommand(typeof(ReloadAbuseCommand))]
        public readonly GameCmd ReloadAbuse;
        [CustomCommand(typeof(BackStepCommand))]
        public readonly GameCmd BackStep;
        [CustomCommand(typeof(FreePenaltyCommand))]
        public readonly GameCmd FreePenalty;
        [CustomCommand(typeof(PKpointCommand))]
        public readonly GameCmd PkPoint;
        [CustomCommand(typeof(IncPkPointCommand))]
        public readonly GameCmd Incpkpoint;
        [CustomCommand(typeof(HungerCommand))]
        public readonly GameCmd Hunger;
        [CustomCommand(typeof(HairCommand))]
        public readonly GameCmd Hair;
        [CustomCommand(typeof(TrainingCommand))]
        public readonly GameCmd Training;
        [CustomCommand(typeof(DelSkillCommand))]
        public readonly GameCmd DeleteSkill;
        [CustomCommand(typeof(ChangeJobCommand))]
        public readonly GameCmd ChangeJob;
        [CustomCommand(typeof(ChangeGenderCommand))]
        public readonly GameCmd ChangeGender;
        [CustomCommand(typeof(MissionCommand))]
        public readonly GameCmd Mission;
        [CustomCommand(typeof(MobPlaceCommand))]
        public readonly GameCmd MobPlace;
        [CustomCommand(typeof(DeleteItemCommand))]
        public readonly GameCmd DeleteItem;
        [CustomCommand(typeof(ClearMissionCommand))]
        public readonly GameCmd ClearMission;
        [CustomCommand(typeof(ReconnectionCommand))]
        public readonly GameCmd Reconnection;
        [CustomCommand(typeof(DisableFilterCommand))]
        public readonly GameCmd DisableFilter;
        [CustomCommand(typeof(ChangeUserFullCommand))]
        public readonly GameCmd ChguserFull;
        [CustomCommand(typeof(ChangeZenFastStepCommand))]
        public readonly GameCmd ChgZenFastStep;
        [CustomCommand(typeof(ContestPointCommand))]
        public readonly GameCmd ContestPoint;
        [CustomCommand(typeof(StartContestCommand))]
        public readonly GameCmd StartContest;
        [CustomCommand(typeof(EndContestCommand))]
        public readonly GameCmd EndContest;
        [CustomCommand(typeof(AnnouncementCommand))]
        public readonly GameCmd Announcement;
        [CustomCommand(typeof(ChangeItemNameCommand))]
        public readonly GameCmd ChangeItemName;
        [CustomCommand(typeof(DisableSendMsgCommand))]
        public readonly GameCmd DisableSendMsg;
        [CustomCommand(typeof(EnableSendMsgCommand))]
        public readonly GameCmd EnableSendMsg;
        [CustomCommand(typeof(DisableSendMsgListCommand))]
        public readonly GameCmd DisableSendMsgList;
        [CustomCommand(typeof(KillCommand))]
        public readonly GameCmd Kill;
        [CustomCommand(typeof(MakeItemCommond))]
        public readonly GameCmd Make;
        [CustomCommand(typeof(SmakeItemCommand))]
        public readonly GameCmd Smake;
        [CustomCommand(typeof(FireBurnCommand))]
        public readonly GameCmd FireBurn;
        [CustomCommand(typeof(TestFireCommand))]
        public readonly GameCmd TestFire;
        [CustomCommand(typeof(TestStatusCommand))]
        public readonly GameCmd TestStatus;
        [CustomCommand(typeof(DelGoldCommand))]
        public readonly GameCmd DelGold;
        [CustomCommand(typeof(AddGoldCommand))]
        public readonly GameCmd AddGold;
        [CustomCommand(typeof(DelGameGoldCommand))]
        public readonly GameCmd DelGameGold;
        [CustomCommand(typeof(AddGameGoldCommand))]
        public readonly GameCmd AddGameGold;
        [CustomCommand(typeof(GameGoldCommand))]
        public readonly GameCmd GameGold;
        [CustomCommand(typeof(GamePointCommand))]
        public readonly GameCmd GamePoint;
        [CustomCommand(typeof(CreditPointCommand))]
        public readonly GameCmd CreditPoint;
        [CustomCommand(typeof(RefineWeaponCommand))]
        public readonly GameCmd RefineWeapon;
        [CustomCommand(typeof(ReLoadAdminCommand))]
        public readonly GameCmd ReloadAdmin;
        [CustomCommand(typeof(ReloadNpcCommand))]
        public readonly GameCmd ReloadNpc;
        [CustomCommand(typeof(ReloadManageCommand))]
        public readonly GameCmd ReloadManage;
        [CustomCommand(typeof(ReloadManageCommand))]
        public readonly GameCmd ReloadRobotManage;
        [CustomCommand(typeof(ReloadRobotCommand))]
        public readonly GameCmd ReloadRobot;
        [CustomCommand(typeof(ReloadMonItemsCommand))]
        public readonly GameCmd ReloadMonItems;
        [CustomCommand(typeof(ReloadMagicDBCommand))]
        public readonly GameCmd ReloadMagicDb;
        [CustomCommand(typeof(ReAliveCommand))]
        public readonly GameCmd ReaLive;
        [CustomCommand(typeof(AdjuestLevelCommand))]
        public readonly GameCmd AdjuestLevel;
        [CustomCommand(typeof(AdjuestExpCommand))]
        public readonly GameCmd AdjuestExp;
        [CustomCommand(typeof(AddGuildCommand))]
        public readonly GameCmd AddGuild;
        [CustomCommand(typeof(DelGuildCommand))]
        public readonly GameCmd DelGuild;
        [CustomCommand(typeof(ChangeSabukLordCommand))]
        public readonly GameCmd ChangeSabukLord;
        [CustomCommand(typeof(ForcedWallconquestWarCommand))]
        public readonly GameCmd ForcedWallConQuestWar;
        [CustomCommand(typeof(TrainingSkillCommand))]
        public readonly GameCmd TrainingSkill;
        [CustomCommand(typeof(ReloadAllGuildCommand))]
        public readonly GameCmd ReloadGuildAll;
        [CustomCommand(typeof(ShowMapInfoCommand))]
        public readonly GameCmd MapInfo;
        [CustomCommand(typeof(SbkDoorControlCommand))]
        public readonly GameCmd SbkDoor;
        [CustomCommand(typeof(ChangeDearNameCommand))]
        public readonly GameCmd ChangeDearName;
        [CustomCommand(typeof(ChangeMasterNameCommand))]
        public readonly GameCmd ChangeMasterName;
        [CustomCommand(typeof(StartQuestCommand))]
        public readonly GameCmd StartQuest;
        [CustomCommand(typeof(SetPermissionCommand))]
        public readonly GameCmd SetperMission;
        [CustomCommand(typeof(ClearMapMonsterCommand))]
        public readonly GameCmd ClearMon;
        [CustomCommand(typeof(ReNewLevelCommand))]
        public readonly GameCmd RenewLevel;
        [CustomCommand(typeof(DenyIPaddrLogonCommand))]
        public readonly GameCmd DenyipLogon;
        [CustomCommand(typeof(DenyAccountLogonCommand))]
        public readonly GameCmd DenyAccountLogon;
        [CustomCommand(typeof(DenyChrNameLogonCommand))]
        public readonly GameCmd DenyChrNameLogon;
        [CustomCommand(typeof(DelDenyIPaddrLogonCommand))]
        public readonly GameCmd DelDenyIpLogon;
        [CustomCommand(typeof(DelDenyAccountLogonCommand))]
        public readonly GameCmd DelDenyAccountLogon;
        [CustomCommand(typeof(DelDenyChrNameLogonCommand))]
        public readonly GameCmd DelDenyChrNameLogon;
        [CustomCommand(typeof(ShowDenyIPaddrLogonCommand))]
        public readonly GameCmd ShowDenyIpLogon;
        [CustomCommand(typeof(ShowDenyAccountLogonCommand))]
        public readonly GameCmd ShowDenyAccountLogon;
        [CustomCommand(typeof(ShowDenyChrNameLogonCommand))]
        public readonly GameCmd ShowDenyChrNameLogon;
        [CustomCommand(typeof(ViewWhisperCommand))]
        public readonly GameCmd ViewWhisper;
        [CustomCommand(typeof(SpirtStartCommand))]
        public readonly GameCmd Spirit;
        [CustomCommand(typeof(SpirtStopCommand))]
        public readonly GameCmd SpiritStop;
        [CustomCommand(typeof(SetMapModeCommamd))]
        public readonly GameCmd SetMapMode;
        [CustomCommand(typeof(ShowMapModeCommand))]
        public readonly GameCmd ShowMapMode;
        [CustomCommand(typeof(ClearBagItemCommand))]
        public readonly GameCmd ClearBag;
        [CustomCommand(typeof(LockLoginCommand))]
        public readonly GameCmd LockLogon;

        public GameCommands()
        {
            Data = new GameCmd();
            Prvmsg = new GameCmd();
            AllowMsg = new GameCmd();
            Letshout = new GameCmd();
            LetTrade = new GameCmd();
            Letguild = new GameCmd();
            Endguild = new GameCmd();
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
            SabukwallGold = new GameCmd();
            Recall = new GameCmd();
            Regoto = new GameCmd();
            Showflag = new GameCmd();
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
            Incpkpoint = new GameCmd();
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
        public int PerMissionMin { get; set; }
        /// <summary>
        /// 最大权限
        /// </summary>
        public int PerMissionMax { get; set; }
    }
}