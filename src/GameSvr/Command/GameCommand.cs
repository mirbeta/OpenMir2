using GameSvr.Command.Commands;

namespace GameSvr.Command
{
    public class GameCommand
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
        public readonly GameCmd ReloadGuild;
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
        public readonly GameCmd Who;
        public readonly GameCmd Total;
        public readonly GameCmd Testga;
        public readonly GameCmd Opdeleteskill;
        public readonly GameCmd Changeweapondura;
        public readonly GameCmd Reloadmonsterdb;
        public readonly GameCmd Reloaddiary;
        public readonly GameCmd Reloaditemdb;
        public readonly GameCmd Restbonuspoint;
        public readonly GameCmd Oxquizroom;
        public readonly GameCmd Gsa;
        public readonly GameCmd Recall;
        public readonly GameCmd Regoto;

        [ConvertToBinary(typeof(SearchMasterCommand))]
        public readonly GameCmd Master;
        [ConvertToBinary(typeof(SearchDearCommand))]
        public readonly GameCmd Dear;
        [ConvertToBinary(typeof(NpcScriptCommand))]
        public readonly GameCmd NpcScript;
        [ConvertToBinary(typeof(GroupRecallCommand))]
        public readonly GameCmd GroupRecalll;
        [ConvertToBinary(typeof(GuildRecallCommand))]
        public readonly GameCmd GuildRecalll;
        [ConvertToBinary(typeof(SearchHumanCommand))]
        public readonly GameCmd Searching;
        [ConvertToBinary(typeof(RecallMobCommand))]
        public readonly GameCmd RecallMob;
        [ConvertToBinary(typeof(ChangeSalveStatusCommand))]
        public readonly GameCmd Rest;
        [ConvertToBinary(typeof(EndGuildCommand))]
        public readonly GameCmd Endguild;
        [ConvertToBinary(typeof(AuthallyCommand))]
        public readonly GameCmd Authally;
        [ConvertToBinary(typeof(LetGuildCommand))]
        public readonly GameCmd Letguild;
        [ConvertToBinary(typeof(BanGuildChatCommand))]
        public readonly GameCmd BanGuildChat;
        [ConvertToBinary(typeof(LetTradeCommand))]
        public readonly GameCmd LetTrade;
        [ConvertToBinary(typeof(LetShoutCommand))]
        public readonly GameCmd Letshout;
        [ConvertToBinary(typeof(AllowMsgCommand))]
        public readonly GameCmd AllowMsg;
        [ConvertToBinary(typeof(ShowHumanUnitOpenCommand))]
        public readonly GameCmd ShowOpen;
        [ConvertToBinary(typeof(ShowUnitCommand))]
        public readonly GameCmd ShowUnit;
        [ConvertToBinary(typeof(AllowGuildRecallCommand))]
        public readonly GameCmd AllowGuildRecall;
        [ConvertToBinary(typeof(DelBonuPointCommand))]
        public readonly GameCmd DelBonusPoint;
        [ConvertToBinary(typeof(BonuPointCommand))]
        public readonly GameCmd BonusPoint;
        [ConvertToBinary(typeof(ShowSbkGoldCommand))]
        public readonly GameCmd SabukwallGold;
        [ConvertToBinary(typeof(MapInfoCommand))]
        public readonly GameCmd Info;
        [ConvertToBinary(typeof(SetPassWordCommand))]
        public readonly GameCmd SetPassword;
        [ConvertToBinary(typeof(ChgpassWordCommand))]
        public readonly GameCmd ChgPassword;
        [ConvertToBinary(typeof(ClearHumanPasswordCommand))]
        public readonly GameCmd ClrPassword;
        [ConvertToBinary(typeof(UnPasswWordCommand))]
        public readonly GameCmd UnPassword;
        [ConvertToBinary(typeof(UnlockStorageCommand))]
        public readonly GameCmd UnlockStorage;
        [ConvertToBinary(typeof(UnLockCommand))]
        public readonly GameCmd Unlock;
        [ConvertToBinary(typeof(LockCommand))]
        public readonly GameCmd Lock;
        [ConvertToBinary(typeof(SetFlagCommand))]
        public readonly GameCmd SetFlag;
        [ConvertToBinary(typeof(SetOpenCommand))]
        public readonly GameCmd SetOpen;
        [ConvertToBinary(typeof(SetUnitCommand))]
        public readonly GameCmd SetUnit;
        [ConvertToBinary(typeof(PasswordLockCommand))]
        public readonly GameCmd PasswordLock;
        [ConvertToBinary(typeof(AuthCancelCommand))]
        public readonly GameCmd AuthCancel;
        [ConvertToBinary(typeof(AuthCommand))]
        public readonly GameCmd Auth;
        [ConvertToBinary(typeof(DataCommand))]
        public readonly GameCmd Data;
        [ConvertToBinary(typeof(PrvMsgCommand))]
        public readonly GameCmd Prvmsg;
        [ConvertToBinary(typeof(UserMoveXYCommand))]
        public readonly GameCmd UserMove;
        [ConvertToBinary(typeof(AllowGroupReCallCommand))]
        public readonly GameCmd AllowGroupCall;
        [ConvertToBinary(typeof(MemberFunctionCommand))]
        public readonly GameCmd MemberFunction;
        [ConvertToBinary(typeof(MemberFunctionExCommand))]
        public readonly GameCmd MemberFunctioneX;
        [ConvertToBinary(typeof(AllowDearRecallCommand))]
        public readonly GameCmd AllowDearRcall;
        [ConvertToBinary(typeof(DearRecallCommond))]
        public readonly GameCmd DearRecall;
        [ConvertToBinary(typeof(AllowMasterRecallCommand))]
        public readonly GameCmd AllowMasterRecall;
        [ConvertToBinary(typeof(MasterRecallCommand))]
        public readonly GameCmd MasteRecall;
        [ConvertToBinary(typeof(ChangeAttackModeCommand))]
        public readonly GameCmd AttackMode;
        [ConvertToBinary(typeof(TakeOnHorseCommand))]
        public readonly GameCmd TakeonHorse;
        [ConvertToBinary(typeof(TakeOffHorseCommand))]
        public readonly GameCmd TakeofHorse;
        [ConvertToBinary(typeof(HumanLocalCommand))]
        public readonly GameCmd HumanLocal;
        [ConvertToBinary(typeof(PositionMoveCommand))]
        public readonly GameCmd Move;
        [ConvertToBinary(typeof(PositionMoveCommand))]
        public readonly GameCmd PositionMove;
        [ConvertToBinary(typeof(MobLevelCommand))]
        public readonly GameCmd MobLevel;
        [ConvertToBinary(typeof(MobCountCommand))]
        public readonly GameCmd MobCount;
        [ConvertToBinary(typeof(HumanCountCommand))]
        public readonly GameCmd HumanCount;
        [ConvertToBinary(typeof(ShowMapInfoCommand))]
        public readonly GameCmd Map;
        [ConvertToBinary(typeof(KickHumanCommand))]
        public readonly GameCmd Kick;
        [ConvertToBinary(typeof(TingCommand))]
        public readonly GameCmd Ting;
        [ConvertToBinary(typeof(SuperTingCommand))]
        public readonly GameCmd Superting;
        [ConvertToBinary(typeof(MapMoveCommand))]
        public readonly GameCmd MapMove;
        [ConvertToBinary(typeof(ShutupCommand))]
        public readonly GameCmd ShutUp;
        [ConvertToBinary(typeof(ShutupReleaseCommand))]
        public readonly GameCmd ReleaseShutup;
        [ConvertToBinary(typeof(ShutupListCommand))]
        public readonly GameCmd ShutupList;
        [ConvertToBinary(typeof(ChangeAdminModeCommand))]
        public readonly GameCmd GameMaster;
        [ConvertToBinary(typeof(ChangeObModeCommand))]
        public readonly GameCmd ObServer;
        [ConvertToBinary(typeof(ChangeSuperManModeCommand))]
        public readonly GameCmd SueprMan;
        [ConvertToBinary(typeof(ChangeLevelCommand))]
        public readonly GameCmd Level;
        [ConvertToBinary(typeof(ShowHumanFlagCommand))]
        public readonly GameCmd Showflag;
        [ConvertToBinary(typeof(MobCommand))]
        public readonly GameCmd Mob;
        [ConvertToBinary(typeof(MobNpcCommand))]
        public readonly GameCmd MobNpc;
        [ConvertToBinary(typeof(DelNpcCommand))]
        public readonly GameCmd DeleteNpc;
        [ConvertToBinary(typeof(LuckPointCommand))]
        public readonly GameCmd LuckyPoint;
        [ConvertToBinary(typeof(LotteryTicketCommandL))]
        public readonly GameCmd LotteryTicket;
        [ConvertToBinary(typeof(ReloadLineNoticeCommand))]
        public readonly GameCmd ReloadLineNotice;
        [ConvertToBinary(typeof(ReloadAbuseCommand))]
        public readonly GameCmd ReloadAbuse;
        [ConvertToBinary(typeof(BackStepCommand))]
        public readonly GameCmd BackStep;
        [ConvertToBinary(typeof(FreePenaltyCommand))]
        public readonly GameCmd FreePenalty;
        [ConvertToBinary(typeof(PKpointCommand))]
        public readonly GameCmd PkPoint;
        [ConvertToBinary(typeof(IncPkPointCommand))]
        public readonly GameCmd Incpkpoint;
        [ConvertToBinary(typeof(HungerCommand))]
        public readonly GameCmd Hunger;
        [ConvertToBinary(typeof(HairCommand))]
        public readonly GameCmd Hair;
        [ConvertToBinary(typeof(TrainingCommand))]
        public readonly GameCmd Training;
        [ConvertToBinary(typeof(DelSkillCommand))]
        public readonly GameCmd DeleteSkill;
        [ConvertToBinary(typeof(ChangeJobCommand))]
        public readonly GameCmd ChangeJob;
        [ConvertToBinary(typeof(ChangeGenderCommand))]
        public readonly GameCmd ChangeGender;
        [ConvertToBinary(typeof(MissionCommand))]
        public readonly GameCmd Mission;
        [ConvertToBinary(typeof(MobPlaceCommand))]
        public readonly GameCmd MobPlace;
        [ConvertToBinary(typeof(DeleteItemCommand))]
        public readonly GameCmd DeleteItem;
        [ConvertToBinary(typeof(ClearMissionCommand))]
        public readonly GameCmd ClearMission;
        [ConvertToBinary(typeof(ReconnectionCommand))]
        public readonly GameCmd Reconnection;
        [ConvertToBinary(typeof(DisableFilterCommand))]
        public readonly GameCmd DisableFilter;
        [ConvertToBinary(typeof(ChangeUserFullCommand))]
        public readonly GameCmd ChguserFull;
        [ConvertToBinary(typeof(ChangeZenFastStepCommand))]
        public readonly GameCmd ChgZenFastStep;
        [ConvertToBinary(typeof(ContestPointCommand))]
        public readonly GameCmd ContestPoint;
        [ConvertToBinary(typeof(StartContestCommand))]
        public readonly GameCmd StartContest;
        [ConvertToBinary(typeof(EndContestCommand))]
        public readonly GameCmd EndContest;
        [ConvertToBinary(typeof(AnnouncementCommand))]
        public readonly GameCmd Announcement;
        [ConvertToBinary(typeof(ChangeItemNameCommand))]
        public readonly GameCmd ChangeItemName;
        [ConvertToBinary(typeof(DisableSendMsgCommand))]
        public readonly GameCmd DisableSendMsg;
        [ConvertToBinary(typeof(EnableSendMsgCommand))]
        public readonly GameCmd EnableSendMsg;
        [ConvertToBinary(typeof(DisableSendMsgListCommand))]
        public readonly GameCmd DisableSendMsgList;
        [ConvertToBinary(typeof(KillCommand))]
        public readonly GameCmd Kill;
        [ConvertToBinary(typeof(MakeItemCommond))]
        public readonly GameCmd Make;
        [ConvertToBinary(typeof(SmakeItemCommand))]
        public readonly GameCmd Smake;
        [ConvertToBinary(typeof(FireBurnCommand))]
        public readonly GameCmd FireBurn;
        [ConvertToBinary(typeof(TestFireCommand))]
        public readonly GameCmd TestFire;
        [ConvertToBinary(typeof(TestStatusCommand))]
        public readonly GameCmd TestStatus;
        [ConvertToBinary(typeof(DelGoldCommand))]
        public readonly GameCmd DelGold;
        [ConvertToBinary(typeof(AddGoldCommand))]
        public readonly GameCmd AddGold;
        [ConvertToBinary(typeof(DelGameGoldCommand))]
        public readonly GameCmd DelGameGold;
        [ConvertToBinary(typeof(AddGameGoldCommand))]
        public readonly GameCmd AddGameGold;
        [ConvertToBinary(typeof(GameGoldCommand))]
        public readonly GameCmd GameGold;
        [ConvertToBinary(typeof(GamePointCommand))]
        public readonly GameCmd GamePoint;
        [ConvertToBinary(typeof(CreditPointCommand))]
        public readonly GameCmd CreditPoint;
        [ConvertToBinary(typeof(RefineWeaponCommand))]
        public readonly GameCmd RefineWeapon;
        [ConvertToBinary(typeof(ReLoadAdminCommand))]
        public readonly GameCmd ReloadAdmin;
        [ConvertToBinary(typeof(ReloadNpcCommand))]
        public readonly GameCmd ReloadNpc;
        [ConvertToBinary(typeof(ReloadManageCommand))]
        public readonly GameCmd ReloadManage;
        [ConvertToBinary(typeof(ReloadManageCommand))]
        public readonly GameCmd ReloadRobotManage;
        [ConvertToBinary(typeof(ReloadRobotCommand))]
        public readonly GameCmd ReloadRobot;
        [ConvertToBinary(typeof(ReloadMonItemsCommand))]
        public readonly GameCmd ReloadMonItems;
        [ConvertToBinary(typeof(ReloadMagicDBCommand))]
        public readonly GameCmd ReloadMagicDb;
        [ConvertToBinary(typeof(ReAliveCommand))]
        public readonly GameCmd ReaLive;
        [ConvertToBinary(typeof(AdjuestLevelCommand))]
        public readonly GameCmd AdjuestLevel;
        [ConvertToBinary(typeof(AdjuestExpCommand))]
        public readonly GameCmd AdjuestExp;
        [ConvertToBinary(typeof(AddGuildCommand))]
        public readonly GameCmd AddGuild;
        [ConvertToBinary(typeof(DelGuildCommand))]
        public readonly GameCmd DelGuild;
        [ConvertToBinary(typeof(ChangeSabukLordCommand))]
        public readonly GameCmd ChangeSabukLord;
        [ConvertToBinary(typeof(ForcedWallconquestWarCommand))]
        public readonly GameCmd ForcedWallConQuestWar;
        [ConvertToBinary(typeof(TrainingSkillCommand))]
        public readonly GameCmd TrainingSkill;
        [ConvertToBinary(typeof(ReloadGuildCommand))]
        public readonly GameCmd ReloadGuildAll;
        [ConvertToBinary(typeof(ShowMapInfoCommand))]
        public readonly GameCmd MapInfo;
        [ConvertToBinary(typeof(SbkDoorControlCommand))]
        public readonly GameCmd SbkDoor;
        [ConvertToBinary(typeof(ChangeDearNameCommand))]
        public readonly GameCmd ChangeDearName;
        [ConvertToBinary(typeof(ChangeMasterNameCommand))]
        public readonly GameCmd ChangeMasterName;
        [ConvertToBinary(typeof(StartQuestCommand))]
        public readonly GameCmd StartQuest;
        [ConvertToBinary(typeof(SetPermissionCommand))]
        public readonly GameCmd SetperMission;
        [ConvertToBinary(typeof(ClearMapMonsterCommand))]
        public readonly GameCmd ClearMon;
        [ConvertToBinary(typeof(ReNewLevelCommand))]
        public readonly GameCmd RenewLevel;
        [ConvertToBinary(typeof(DenyIPaddrLogonCommand))]
        public readonly GameCmd DenyipLogon;
        [ConvertToBinary(typeof(DenyAccountLogonCommand))]
        public readonly GameCmd DenyAccountLogon;
        [ConvertToBinary(typeof(DenyChrNameLogonCommand))]
        public readonly GameCmd DenyChrNameLogon;
        [ConvertToBinary(typeof(DelDenyIPaddrLogonCommand))]
        public readonly GameCmd DelDenyIpLogon;
        [ConvertToBinary(typeof(DelDenyAccountLogonCommand))]
        public readonly GameCmd DelDenyAccountLogon;
        [ConvertToBinary(typeof(DelDenyChrNameLogonCommand))]
        public readonly GameCmd DelDenyChrNameLogon;
        [ConvertToBinary(typeof(ShowDenyIPaddrLogonCommand))]
        public readonly GameCmd ShowDenyIpLogon;
        [ConvertToBinary(typeof(ShowDenyAccountLogonCommand))]
        public readonly GameCmd ShowDenyAccountLogon;
        [ConvertToBinary(typeof(ShowDenyChrNameLogonCommand))]
        public readonly GameCmd ShowDenyChrNameLogon;
        [ConvertToBinary(typeof(ViewWhisperCommand))]
        public readonly GameCmd ViewWhisper;
        [ConvertToBinary(typeof(SpirtStartCommand))]
        public readonly GameCmd Spirit;
        [ConvertToBinary(typeof(SpirtStopCommand))]
        public readonly GameCmd SpiritStop;
        [ConvertToBinary(typeof(SetMapModeCommamd))]
        public readonly GameCmd SetMapMode;
        [ConvertToBinary(typeof(ShowMapModeCommand))]
        public readonly GameCmd ShowMapMode;
        [ConvertToBinary(typeof(ClearBagItemCommand))]
        public readonly GameCmd ClearBag;
        [ConvertToBinary(typeof(LockLoginCommand))]
        public readonly GameCmd LockLogon;

        public GameCommand()
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
            Reloaditemdb = new GameCmd();
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
}