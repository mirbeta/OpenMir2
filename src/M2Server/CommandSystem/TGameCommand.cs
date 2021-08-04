using SystemModule;

namespace M2Server
{
    public class TGameCommand
    {
        public TGameCmd DATA;
        public TGameCmd PRVMSG;
        public TGameCmd ALLOWMSG;
        public TGameCmd LETSHOUT;
        public TGameCmd LETTRADE;
        public TGameCmd LETGUILD;
        public TGameCmd ENDGUILD;
        public TGameCmd BANGUILDCHAT;
        public TGameCmd AUTHALLY;
        public TGameCmd AUTH;
        public TGameCmd AUTHCANCEL;
        public TGameCmd DIARY;
        public TGameCmd USERMOVE;
        public TGameCmd SEARCHING;
        public TGameCmd ALLOWGROUPCALL;
        public TGameCmd GROUPRECALLL;
        public TGameCmd ALLOWGUILDRECALL;
        public TGameCmd GUILDRECALLL;
        public TGameCmd UNLOCKSTORAGE;
        public TGameCmd UNLOCK;
        public TGameCmd __LOCK;
        public TGameCmd PASSWORDLOCK;
        public TGameCmd SETPASSWORD;
        public TGameCmd CHGPASSWORD;
        public TGameCmd CLRPASSWORD;
        public TGameCmd UNPASSWORD;
        public TGameCmd MEMBERFUNCTION;
        public TGameCmd MEMBERFUNCTIONEX;
        public TGameCmd DEAR;
        public TGameCmd ALLOWDEARRCALL;
        public TGameCmd DEARRECALL;
        public TGameCmd MASTER;
        public TGameCmd ALLOWMASTERRECALL;
        public TGameCmd MASTERECALL;
        public TGameCmd ATTACKMODE;
        public TGameCmd REST;
        public TGameCmd TAKEONHORSE;
        public TGameCmd TAKEOFHORSE;
        public TGameCmd HUMANLOCAL;
        public TGameCmd MOVE;
        public TGameCmd POSITIONMOVE;
        public TGameCmd INFO;
        public TGameCmd MOBLEVEL;
        public TGameCmd MOBCOUNT;
        public TGameCmd HUMANCOUNT;
        public TGameCmd MAP;
        public TGameCmd KICK;
        public TGameCmd TING;
        public TGameCmd SUPERTING;
        public TGameCmd MAPMOVE;
        public TGameCmd SHUTUP;
        public TGameCmd RELEASESHUTUP;
        public TGameCmd SHUTUPLIST;
        public TGameCmd GAMEMASTER;
        public TGameCmd OBSERVER;
        public TGameCmd SUEPRMAN;
        public TGameCmd LEVEL;
        public TGameCmd SABUKWALLGOLD;
        public TGameCmd RECALL;
        public TGameCmd REGOTO;
        public TGameCmd SHOWFLAG;
        public TGameCmd SHOWOPEN;
        public TGameCmd SHOWUNIT;
        public TGameCmd ATTACK;
        public TGameCmd MOB;
        public TGameCmd MOBNPC;
        public TGameCmd DELNPC;
        public TGameCmd NPCSCRIPT;
        public TGameCmd RECALLMOB;
        public TGameCmd LUCKYPOINT;
        public TGameCmd LOTTERYTICKET;
        public TGameCmd RELOADGUILD;
        public TGameCmd RELOADLINENOTICE;
        public TGameCmd RELOADABUSE;
        public TGameCmd BACKSTEP;
        public TGameCmd BALL;
        public TGameCmd FREEPENALTY;
        public TGameCmd PKPOINT;
        public TGameCmd INCPKPOINT;
        public TGameCmd CHANGELUCK;
        public TGameCmd HUNGER;
        public TGameCmd HAIR;
        public TGameCmd TRAINING;
        public TGameCmd DELETESKILL;
        public TGameCmd CHANGEJOB;
        public TGameCmd CHANGEGENDER;
        public TGameCmd NAMECOLOR;
        public TGameCmd MISSION;
        public TGameCmd MOBPLACE;
        public TGameCmd TRANSPARECY;
        public TGameCmd DELETEITEM;
        public TGameCmd LEVEL0;
        public TGameCmd CLEARMISSION;
        public TGameCmd SETFLAG;
        public TGameCmd SETOPEN;
        public TGameCmd SETUNIT;
        public TGameCmd RECONNECTION;
        public TGameCmd DISABLEFILTER;
        public TGameCmd CHGUSERFULL;
        public TGameCmd CHGZENFASTSTEP;
        public TGameCmd CONTESTPOINT;
        public TGameCmd STARTCONTEST;
        public TGameCmd ENDCONTEST;
        public TGameCmd ANNOUNCEMENT;
        public TGameCmd OXQUIZROOM;
        public TGameCmd GSA;
        public TGameCmd CHANGEITEMNAME;
        public TGameCmd DISABLESENDMSG;
        public TGameCmd ENABLESENDMSG;
        public TGameCmd DISABLESENDMSGLIST;
        public TGameCmd KILL;
        public TGameCmd MAKE;
        public TGameCmd SMAKE;
        public TGameCmd BONUSPOINT;
        public TGameCmd DELBONUSPOINT;
        public TGameCmd RESTBONUSPOINT;
        public TGameCmd FIREBURN;
        public TGameCmd TESTFIRE;
        public TGameCmd TESTSTATUS;
        public TGameCmd DELGOLD;
        public TGameCmd ADDGOLD;
        public TGameCmd DELGAMEGOLD;
        public TGameCmd ADDGAMEGOLD;
        public TGameCmd GAMEGOLD;
        public TGameCmd GAMEPOINT;
        public TGameCmd CREDITPOINT;
        public TGameCmd TESTGOLDCHANGE;
        public TGameCmd REFINEWEAPON;
        public TGameCmd RELOADADMIN;
        public TGameCmd RELOADNPC;
        public TGameCmd RELOADMANAGE;
        public TGameCmd RELOADROBOTMANAGE;
        public TGameCmd RELOADROBOT;
        public TGameCmd RELOADMONITEMS;
        public TGameCmd RELOADDIARY;
        public TGameCmd RELOADITEMDB;
        public TGameCmd RELOADMAGICDB;
        public TGameCmd RELOADMONSTERDB;
        public TGameCmd RELOADMINMAP;
        public TGameCmd REALIVE;
        public TGameCmd ADJUESTLEVEL;
        public TGameCmd ADJUESTEXP;
        public TGameCmd ADDGUILD;
        public TGameCmd DELGUILD;
        public TGameCmd CHANGESABUKLORD;
        public TGameCmd FORCEDWALLCONQUESTWAR;
        public TGameCmd ADDTOITEMEVENT;
        public TGameCmd ADDTOITEMEVENTASPIECES;
        public TGameCmd ITEMEVENTLIST;
        public TGameCmd STARTINGGIFTNO;
        public TGameCmd DELETEALLITEMEVENT;
        public TGameCmd STARTITEMEVENT;
        public TGameCmd ITEMEVENTTERM;
        public TGameCmd ADJUESTTESTLEVEL;
        public TGameCmd TRAININGSKILL;
        public TGameCmd OPDELETESKILL;
        public TGameCmd CHANGEWEAPONDURA;
        public TGameCmd RELOADGUILDALL;
        public TGameCmd WHO;
        public TGameCmd TOTAL;
        public TGameCmd TESTGA;
        public TGameCmd MAPINFO;
        public TGameCmd SBKDOOR;
        public TGameCmd CHANGEDEARNAME;
        public TGameCmd CHANGEMASTERNAME;
        public TGameCmd STARTQUEST;
        public TGameCmd SETPERMISSION;
        public TGameCmd CLEARMON;
        public TGameCmd RENEWLEVEL;
        public TGameCmd DENYIPLOGON;
        public TGameCmd DENYACCOUNTLOGON;
        public TGameCmd DENYCHARNAMELOGON;
        public TGameCmd DELDENYIPLOGON;
        public TGameCmd DELDENYACCOUNTLOGON;
        public TGameCmd DELDENYCHARNAMELOGON;
        public TGameCmd SHOWDENYIPLOGON;
        public TGameCmd SHOWDENYACCOUNTLOGON;
        public TGameCmd SHOWDENYCHARNAMELOGON;
        public TGameCmd VIEWWHISPER;
        public TGameCmd SPIRIT;
        public TGameCmd SPIRITSTOP;
        public TGameCmd SETMAPMODE;
        public TGameCmd SHOWMAPMODE;
        public TGameCmd TESTSERVERCONFIG;
        public TGameCmd SERVERSTATUS;
        public TGameCmd TESTGETBAGITEM;
        public TGameCmd CLEARBAG;
        public TGameCmd SHOWUSEITEMINFO;
        public TGameCmd BINDUSEITEM;
        public TGameCmd MOBFIREBURN;
        public TGameCmd TESTSPEEDMODE;
        public TGameCmd LOCKLOGON;

        public TGameCommand()
        {
    //        {
    //            { "Date", 0, 10} , 
    //{ "PrvMsg", 0, 10} , 
    //{ "AllowMsg", 0, 10} , 
    //{ "LetShout", 0, 10} , 
    //{ "LetTrade", 0, 10} , 
    //{ "LetGuild", 0, 10} , 
    //{ "EndGuild", 0, 10} , 
    //{ "BanGuildChat", 0, 10} , 
    //{ "AuthAlly", 0, 10} , 
    //{ "联盟", 0, 10} , 
    //{ "取消联盟", 0, 10} , 
    //{ "Diary", 0, 10} , 
    //{ "Move", 0, 10} , 
    //{ "Searching", 0, 10} , 
    //{ "AllowGroupRecall", 0, 10} , 
    //{ "GroupRecall", 0, 10} , 
    //{ "AllowGuildRecall", 0, 10} , 
    //{ "GuildRecall", 0, 10} , 
    //{ "UnLockStorage", 0, 10} , 
    //{ "UnLock", 0, 10} , 
    //{ "Lock", 0, 10} , 
    //{ "PasswordLock", 0, 10} , 
    //{ "SetPassword", 0, 10} , 
    //{ "ChgPassword", 0, 10} , 
    //{ "ClrPassword", 10, 10} , 
    //{ "UnPassword", 0, 10} , 
    //{ "MemberFunc", 0, 10} , 
    //{ "MemberFuncEx", 0, 10} , 
    //{ "Dear", 0, 10} , 
    //{ "AllowDearRecall", 0, 10} , 
    //{ "DearRecall", 0, 10} , 
    //{ "Master", 0, 10} , 
    //{ "AllowMasterRecall", 0, 10} , 
    //{ "MasterRecall", 0, 10} , 
    //{ "AttackMode", 0, 10} , 
    //{ "Rest", 0, 10} , 
    //{ "OnHorse", 0, 10} , 
    //{ "OffHorse", 0, 10} , 
    //{ "HumanLocal", 3, 10} , 
    //{ "Move", 3, 6} , 
    //{ "PositionMove", 3, 6} , 
    //{ "Info", 3, 10} , 
    //{ "MobLevel", 3, 10} , 
    //{ "MobCount", 3, 10} , 
    //{ "HumanCount", 3, 10} , 
    //{ "Map", 3, 10} , 
    //{ "Kick", 10, 10} , 
    //{ "Ting", 10, 10} , 
    //{ "SuperTing", 10, 10} , 
    //{ "MapMove", 10, 10} , 
    //{ "Shutup", 10, 10} , 
    //{ "ReleaseShutup", 10, 10} , 
    //{ "ShutupList", 10, 10} , 
    //{ "GameMaster", 10, 10} , 
    //{ "Observer", 10, 10} , 
    //{ "Superman", 10, 10} , 
    //{ "Level", 10, 10} , 
    //{ "SabukWallGold", 10, 10} , 
    //{ "Recall", 10, 10} , 
    //{ "ReGoto", 10, 10} , 
    //{ "showflag", 10, 10} , 
    //{ "showopen", 10, 10} , 
    //{ "showunit", 10, 10} , 
    //{ "Attack", 10, 10} , 
    //{ "Mob", 10, 10} , 
    //{ "MobNpc", 10, 10} , 
    //{ "DelNpc", 10, 10} , 
    //{ "NpcScript", 10, 10} , 
    //{ "RecallMob", 10, 10} , 
    //{ "LuckyPoint", 10, 10} , 
    //{ "LotteryTicket", 10, 10} , 
    //{ "ReloadGuild", 10, 10} , 
    //{ "ReloadLineNotice", 10, 10} , 
    //{ "ReloadAbuse", 10, 10} , 
    //{ "Backstep", 10, 10} , 
    //{ "Ball", 10, 10} , 
    //{ "FreePK", 10, 10} , 
    //{ "PKpoint", 10, 10} , 
    //{ "IncPkPoint", 10, 10} , 
    //{ "ChangeLuck", 10, 10} , 
    //{ "Hunger", 10, 10} , 
    //{ "hair", 10, 10} , 
    //{ "Training", 10, 10} , 
    //{ "DeleteSkill", 10, 10} , 
    //{ "ChangeJob", 10, 10} , 
    //{ "ChangeGender", 10, 10} , 
    //{ "NameColor", 10, 10} , 
    //{ "Mission", 10, 10} , 
    //{ "MobPlace", 10, 10} , 
    //{ "Transparency", 10, 10} , 
    //{ "DeleteItem", 10, 10} , 
    //{ "Level0", 10, 10} , 
    //{ "ClearMission", 10, 10} , 
    //{ "setflag", 10, 10} , 
    //{ "setopen", 10, 10} , 
    //{ "setunit", 10, 10} , 
    //{ "Reconnection", 10, 10} , 
    //{ "DisableFilter", 10, 10} , 
    //{ "CHGUSERFULL", 10, 10} , 
    //{ "CHGZENFASTSTEP", 10, 10} , 
    //{ "ContestPoint", 10, 10} , 
    //{ "StartContest", 10, 10} , 
    //{ "EndContest", 10, 10} , 
    //{ "Announcement", 10, 10} , 
    //{ "OXQuizRoom", 10, 10} , 
    //{ "gsa", 10, 10} , 
    //{ "ChangeItemName", 10, 10} , 
    //{ "DisableSendMsg", 10, 10} , 
    //{ "EnableSendMsg", 10, 10} , 
    //{ "DisableSendMsgList", 10, 10} , 
    //{ "Kill", 10, 10} , 
    //{ "make", 10, 10} , 
    //{ "Supermake", 10, 10} , 
    //{ "BonusPoint", 10, 10} , 
    //{ "DelBonusPoint", 10, 10} , 
    //{ "RestBonusPoint", 10, 10} , 
    //{ "FireBurn", 10, 10} , 
    //{ "TestFire", 10, 10} , 
    //{ "TestStatus", 10, 10} , 
    //{ "DelGold", 10, 10} , 
    //{ "AddGold", 10, 10} , 
    //{ "DelGamePoint", 10, 10} , 
    //{ "AddGamePoint", 10, 10} , 
    //{ "GameGold", 10, 10} , 
    //{ "GamePoint", 10, 10} , 
    //{ "CreditPoint", 10, 10} , 
    //{ "Test_GOLD_Change", 10, 10} , 
    //{ "RefineWeapon", 10, 10} , 
    //{ "ReloadAdmin", 10, 10} , 
    //{ "ReloadNpc", 10, 10} , 
    //{ "ReloadManage", 10, 10} , 
    //{ "ReloadRobotManage", 10, 10} , 
    //{ "ReloadRobot", 10, 10} , 
    //{ "ReloadMonItems", 10, 10} , 
    //{ "ReloadDiary", 10, 10} , 
    //{ "ReloadItemDB", 10, 10} , 
    //{ "ReloadMagicDB", 10, 10} , 
    //{ "ReloadMonsterDB", 10, 10} , 
    //{ "ReLoadMinMap", 10, 10} , 
    //{ "ReAlive", 10, 10} , 
    //{ "AdjustLevel", 10, 10} , 
    //{ "AdjustExp", 10, 10} , 
    //{ "AddGuild", 10, 10} , 
    //{ "DelGuild", 10, 10} , 
    //{ "ChangeSabukLord", 10, 10} , 
    //{ "ForcedWallconquestWar", 10, 10} , 
    //{ "AddToItemEvent", 10, 10} , 
    //{ "AddToItemEventAsPieces", 10, 10} , 
    //{ "ItemEventList", 10, 10} , 
    //{ "StartingGiftNo", 10, 10} , 
    //{ "DeleteAllItemEvent", 10, 10} , 
    //{ "StartItemEvent", 10, 10} , 
    //{ "ItemEventTerm", 10, 10} , 
    //{ "AdjustTestLevel", 10, 10} , 
    //{ "TrainingSkill", 10, 10} , 
    //{ "OPDeleteSkill", 10, 10} , 
    //{ "ChangeWeaponDura", 10, 10} , 
    //{ "ReloadGuildAll", 10, 10} , 
    //{ "Who ", 3, 10} , 
    //{ "Total ", 5, 10} , 
    //{ "Testga", 10, 10} , 
    //{ "MapInfo", 10, 10} , 
    //{ "SbkDoor", 10, 10} , 
    //{ "DearName", 10, 10} , 
    //{ "MasterName", 10, 10} , 
    //{ "StartQuest", 10, 10} , 
    //{ "SetPermission", 10, 10} , 
    //{ "ClearMon", 10, 10} , 
    //{ "ReNewLevel", 10, 10} , 
    //{ "DenyIPLogon", 10, 10} , 
    //{ "DenyAccountLogon", 10, 10} , 
    //{ "DenyCharNameLogon", 10, 10} , 
    //{ "DelDenyIPLogon", 10, 10} , 
    //{ "DelDenyAccountLogon", 10, 10} , 
    //{ "DelDenyCharNameLogon", 10, 10} , 
    //{ "ShowDenyIPLogon", 10, 10} , 
    //{ "ShowDenyAccountLogon", 10, 10} , 
    //{ "ShowDenyCharNameLogon", 10, 10} , 
    //{ "ViewWhisper", 10, 10} , 
    //{ "祈祷生效", 10, 10} , 
    //{ "停止叛变", 10, 10} , 
    //{ "SetMapMode", 10, 10} , 
    //{ "ShowMapMode", 10, 10} , 
    //{ "TestServerConfig", 10, 10} , 
    //{ "ServerStatus", 10, 10} , 
    //{ "TestGetBagItem", 10, 10} , 
    //{ "ClearBag", 10, 10} , 
    //{ "ShowUseItemInfo", 10, 10} , 
    //{ "BindUseItem", 10, 10} , 
    //{ "MobFireBurn", 10, 10} , 
    //{ "TestSpeedMode", 10, 10} , 
    //{ "LockLogin", 0, 0}
    //        }
        }
    }
}