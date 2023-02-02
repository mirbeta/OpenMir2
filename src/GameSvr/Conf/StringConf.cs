using GameSvr.GameCommand;
using SystemModule.Common;

namespace GameSvr.Conf
{
    public class StringConf : ConfigFile
    {
        public StringConf(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadString()
        {
            string LoadString;
            if (ReadWriteString("Server", "ServerIP", "") == "")
            {
                WriteString("Server", "ServerIP", M2Share.Config.ServerIPaddr);
            }
            M2Share.Config.ServerIPaddr = ReadWriteString("Server", "ServerIP", M2Share.Config.ServerIPaddr);
            if (ReadWriteString("Server", "WebSite", "") == "")
            {
                WriteString("Server", "WebSite", M2Share.Config.sWebSite);
            }
            M2Share.Config.sWebSite = ReadWriteString("Server", "WebSite", M2Share.Config.sWebSite);
            if (ReadWriteString("Server", "BbsSite", "") == "")
            {
                WriteString("Server", "BbsSite", M2Share.Config.sBbsSite);
            }
            M2Share.Config.sBbsSite = ReadWriteString("Server", "BbsSite", M2Share.Config.sBbsSite);
            if (ReadWriteString("Server", "ClientDownload", "") == "")
            {
                WriteString("Server", "ClientDownload", M2Share.Config.sClientDownload);
            }
            M2Share.Config.sClientDownload = ReadWriteString("Server", "ClientDownload", M2Share.Config.sClientDownload);
            if (ReadWriteString("Server", "QQ", "") == "")
            {
                WriteString("Server", "QQ", M2Share.Config.sQQ);
            }
            M2Share.Config.sQQ = ReadWriteString("Server", "QQ", M2Share.Config.sQQ);
            if (ReadWriteString("Server", "Phone", "") == "")
            {
                WriteString("Server", "Phone", M2Share.Config.sPhone);
            }
            M2Share.Config.sPhone = ReadWriteString("Server", "Phone", M2Share.Config.sPhone);
            if (ReadWriteString("Server", "BankAccount0", "") == "")
            {
                WriteString("Server", "BankAccount0", M2Share.Config.sBankAccount0);
            }
            M2Share.Config.sBankAccount0 = ReadWriteString("Server", "BankAccount0", M2Share.Config.sBankAccount0);
            if (ReadWriteString("Server", "BankAccount1", "") == "")
            {
                WriteString("Server", "BankAccount1", M2Share.Config.sBankAccount1);
            }
            M2Share.Config.sBankAccount1 = ReadWriteString("Server", "BankAccount1", M2Share.Config.sBankAccount1);
            if (ReadWriteString("Server", "BankAccount2", "") == "")
            {
                WriteString("Server", "BankAccount2", M2Share.Config.sBankAccount2);
            }
            M2Share.Config.sBankAccount2 = ReadWriteString("Server", "BankAccount2", M2Share.Config.sBankAccount2);
            if (ReadWriteString("Server", "BankAccount3", "") == "")
            {
                WriteString("Server", "BankAccount3", M2Share.Config.sBankAccount3);
            }
            M2Share.Config.sBankAccount3 = ReadWriteString("Server", "BankAccount3", M2Share.Config.sBankAccount3);
            if (ReadWriteString("Server", "BankAccount4", "") == "")
            {
                WriteString("Server", "BankAccount4", M2Share.Config.sBankAccount4);
            }
            M2Share.Config.sBankAccount4 = ReadWriteString("Server", "BankAccount4", M2Share.Config.sBankAccount4);
            M2Share.Config.sBankAccount5 = ReadWriteString("Server", "BankAccount5", M2Share.Config.sBankAccount5);
            if (ReadWriteString("Server", "BankAccount6", "") == "")
            {
                WriteString("Server", "BankAccount6", M2Share.Config.sBankAccount6);
            }
            M2Share.Config.sBankAccount6 = ReadWriteString("Server", "BankAccount6", M2Share.Config.sBankAccount6);
            if (ReadWriteString("Server", "BankAccount7", "") == "")
            {
                WriteString("Server", "BankAccount7", M2Share.Config.sBankAccount7);
            }
            M2Share.Config.sBankAccount7 = ReadWriteString("Server", "BankAccount7", M2Share.Config.sBankAccount7);
            if (ReadWriteString("Server", "BankAccount8", "") == "")
            {
                WriteString("Server", "BankAccount8", M2Share.Config.sBankAccount8);
            }
            M2Share.Config.sBankAccount8 = ReadWriteString("Server", "BankAccount8", M2Share.Config.sBankAccount8);
            if (ReadWriteString("Server", "BankAccount9", "") == "")
            {
                WriteString("Server", "BankAccount9", M2Share.Config.sBankAccount9);
            }
            M2Share.Config.sBankAccount9 = ReadWriteString("Server", "BankAccount9", M2Share.Config.sBankAccount9);
            if (ReadWriteString("Guild", "GuildNotice", "") == "")
            {
                WriteString("Guild", "GuildNotice", M2Share.Config.GuildNotice);
            }
            M2Share.Config.GuildNotice = ReadWriteString("Guild", "GuildNotice", M2Share.Config.GuildNotice);
            if (ReadWriteString("Guild", "GuildWar", "") == "")
            {
                WriteString("Guild", "GuildWar", M2Share.Config.GuildWar);
            }
            M2Share.Config.GuildWar = ReadWriteString("Guild", "GuildWar", M2Share.Config.GuildWar);
            if (ReadWriteString("Guild", "GuildAll", "") == "")
            {
                WriteString("Guild", "GuildAll", M2Share.Config.GuildAll);
            }
            M2Share.Config.GuildAll = ReadWriteString("Guild", "GuildAll", M2Share.Config.GuildAll);
            if (ReadWriteString("Guild", "GuildMember", "") == "")
            {
                WriteString("Guild", "GuildMember", M2Share.Config.GuildMember);
            }
            M2Share.Config.GuildMember = ReadWriteString("Guild", "GuildMember", M2Share.Config.GuildMember);
            if (ReadWriteString("Guild", "GuildMemberRank", "") == "")
            {
                WriteString("Guild", "GuildMemberRank", M2Share.Config.GuildMemberRank);
            }
            M2Share.Config.GuildMemberRank = ReadWriteString("Guild", "GuildMemberRank", M2Share.Config.GuildMemberRank);
            if (ReadWriteString("Guild", "GuildChief", "") == "")
            {
                WriteString("Guild", "GuildChief", M2Share.Config.GuildChief);
            }
            M2Share.Config.GuildChief = ReadWriteString("Guild", "GuildChief", M2Share.Config.GuildChief);
            LoadString = ReadWriteString("String", "ClientSoftVersionError", "");
            if (LoadString == "")
            {
                WriteString("String", "ClientSoftVersionError", Settings.ClientSoftVersionError);
            }
            else
            {
                Settings.ClientSoftVersionError = LoadString;
            }
            LoadString = ReadWriteString("String", "DownLoadNewClientSoft", "");
            if (LoadString == "")
            {
                WriteString("String", "DownLoadNewClientSoft", Settings.DownLoadNewClientSoft);
            }
            else
            {
                Settings.DownLoadNewClientSoft = LoadString;
            }
            LoadString = ReadWriteString("String", "ForceDisConnect", "");
            if (LoadString == "")
            {
                WriteString("String", "ForceDisConnect", Settings.ForceDisConnect);
            }
            else
            {
                Settings.ForceDisConnect = LoadString;
            }
            LoadString = ReadWriteString("String", "ClientSoftVersionTooOld", "");
            if (LoadString == "")
            {
                WriteString("String", "ClientSoftVersionTooOld", Settings.ClientSoftVersionTooOld);
            }
            else
            {
                Settings.ClientSoftVersionTooOld = LoadString;
            }
            LoadString = ReadWriteString("String", "DownLoadAndUseNewClient", "");
            if (LoadString == "")
            {
                WriteString("String", "DownLoadAndUseNewClient", Settings.DownLoadAndUseNewClient);
            }
            else
            {
                Settings.DownLoadAndUseNewClient = LoadString;
            }
            LoadString = ReadWriteString("String", "OnlineUserFull", "");
            if (LoadString == "")
            {
                WriteString("String", "OnlineUserFull", Settings.OnlineUserFull);
            }
            else
            {
                Settings.OnlineUserFull = LoadString;
            }
            LoadString = ReadWriteString("String", "YouNowIsTryPlayMode", "");
            if (LoadString == "")
            {
                WriteString("String", "YouNowIsTryPlayMode", Settings.YouNowIsTryPlayMode);
            }
            else
            {
                Settings.YouNowIsTryPlayMode = LoadString;
            }
            LoadString = ReadWriteString("String", "NowIsFreePlayMode", "");
            if (LoadString == "")
            {
                WriteString("String", "NowIsFreePlayMode", Settings.NowIsFreePlayMode);
            }
            else
            {
                Settings.NowIsFreePlayMode = LoadString;
            }
            LoadString = ReadWriteString("String", "AttackModeOfAll", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfAll", Settings.AttackModeOfAll);
            }
            else
            {
                Settings.AttackModeOfAll = LoadString;
            }
            LoadString = ReadWriteString("String", "AttackModeOfPeaceful", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfPeaceful", Settings.AttackModeOfPeaceful);
            }
            else
            {
                Settings.AttackModeOfPeaceful = LoadString;
            }
            LoadString = ReadWriteString("String", "AttackModeOfGroup", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfGroup", Settings.AttackModeOfGroup);
            }
            else
            {
                Settings.AttackModeOfGroup = LoadString;
            }
            LoadString = ReadWriteString("String", "AttackModeOfGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfGuild", Settings.AttackModeOfGuild);
            }
            else
            {
                Settings.AttackModeOfGuild = LoadString;
            }
            LoadString = ReadWriteString("String", "AttackModeOfRedWhite", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfRedWhite", Settings.AttackModeOfRedWhite);
            }
            else
            {
                Settings.AttackModeOfRedWhite = LoadString;
            }
            LoadString = ReadWriteString("String", "StartChangeAttackModeHelp", "");
            if (LoadString == "")
            {
                WriteString("String", "StartChangeAttackModeHelp", Settings.StartChangeAttackModeHelp);
            }
            else
            {
                Settings.StartChangeAttackModeHelp = LoadString;
            }
            LoadString = ReadWriteString("String", "StartNoticeMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartNoticeMsg", Settings.StartNoticeMsg);
            }
            else
            {
                Settings.StartNoticeMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ThrustingOn", "");
            if (LoadString == "")
            {
                WriteString("String", "ThrustingOn", Settings.ThrustingOn);
            }
            else
            {
                Settings.ThrustingOn = LoadString;
            }
            LoadString = ReadWriteString("String", "ThrustingOff", "");
            if (LoadString == "")
            {
                WriteString("String", "ThrustingOff", Settings.ThrustingOff);
            }
            else
            {
                Settings.ThrustingOff = LoadString;
            }
            LoadString = ReadWriteString("String", "HalfMoonOn", "");
            if (LoadString == "")
            {
                WriteString("String", "HalfMoonOn", Settings.HalfMoonOn);
            }
            else
            {
                Settings.HalfMoonOn = LoadString;
            }
            LoadString = ReadWriteString("String", "HalfMoonOff", "");
            if (LoadString == "")
            {
                WriteString("String", "HalfMoonOff", Settings.HalfMoonOff);
            }
            else
            {
                Settings.HalfMoonOff = LoadString;
            }
            Settings.CrsHitOn = LoadConfigString("String", "CrsHitOn", Settings.CrsHitOn);
            Settings.CrsHitOff = LoadConfigString("String", "CrsHitOff", Settings.CrsHitOff);
            Settings.TwinHitOn = LoadConfigString("String", "TwinHitOn", Settings.TwinHitOn);
            Settings.TwinHitOff = LoadConfigString("String", "TwinHitOff", Settings.TwinHitOff);
            LoadString = ReadWriteString("String", "FireSpiritsSummoned", "");
            if (LoadString == "")
            {
                WriteString("String", "FireSpiritsSummoned", Settings.FireSpiritsSummoned);
            }
            else
            {
                Settings.FireSpiritsSummoned = LoadString;
            }
            LoadString = ReadWriteString("String", "FireSpiritsFail", "");
            if (LoadString == "")
            {
                WriteString("String", "FireSpiritsFail", Settings.FireSpiritsFail);
            }
            else
            {
                Settings.FireSpiritsFail = LoadString;
            }
            LoadString = ReadWriteString("String", "SpiritsGone", "");
            if (LoadString == "")
            {
                WriteString("String", "SpiritsGone", Settings.SpiritsGone);
            }
            else
            {
                Settings.SpiritsGone = LoadString;
            }
            LoadString = ReadWriteString("String", "MateDoTooweak", "");
            if (LoadString == "")
            {
                WriteString("String", "MateDoTooweak", Settings.MateDoTooweak);
            }
            else
            {
                Settings.MateDoTooweak = LoadString;
            }
            LoadString = ReadWriteString("String", "TheWeaponBroke", "");
            if (LoadString == "")
            {
                WriteString("String", "TheWeaponBroke", Settings.TheWeaponBroke);
            }
            else
            {
                Settings.TheWeaponBroke = LoadString;
            }
            LoadString = ReadWriteString("String", "TheWeaponRefineSuccessfull", "");
            if (LoadString == "")
            {
                WriteString("String", "TheWeaponRefineSuccessfull", Settings.TheWeaponRefineSuccessfull);
            }
            else
            {
                Settings.TheWeaponRefineSuccessfull = LoadString;
            }
            LoadString = ReadWriteString("String", "YouPoisoned", "");
            if (LoadString == "")
            {
                WriteString("String", "YouPoisoned", Settings.YouPoisoned);
            }
            else
            {
                Settings.YouPoisoned = LoadString;
            }
            LoadString = ReadWriteString("String", "PetRest", "");
            if (LoadString == "")
            {
                WriteString("String", "PetRest", Settings.PetRest);
            }
            else
            {
                Settings.PetRest = LoadString;
            }
            LoadString = ReadWriteString("String", "PetAttack", "");
            if (LoadString == "")
            {
                WriteString("String", "PetAttack", Settings.PetAttack);
            }
            else
            {
                Settings.PetAttack = LoadString;
            }
            LoadString = ReadWriteString("String", "WearNotOfWoMan", "");
            if (LoadString == "")
            {
                WriteString("String", "WearNotOfWoMan", Settings.WearNotOfWoMan);
            }
            else
            {
                Settings.WearNotOfWoMan = LoadString;
            }
            LoadString = ReadWriteString("String", "WearNotOfMan", "");
            if (LoadString == "")
            {
                WriteString("String", "WearNotOfMan", Settings.WearNotOfMan);
            }
            else
            {
                Settings.WearNotOfMan = LoadString;
            }
            LoadString = ReadWriteString("String", "HandWeightNot", "");
            if (LoadString == "")
            {
                WriteString("String", "HandWeightNot", Settings.HandWeightNot);
            }
            else
            {
                Settings.HandWeightNot = LoadString;
            }
            LoadString = ReadWriteString("String", "WearWeightNot", "");
            if (LoadString == "")
            {
                WriteString("String", "WearWeightNot", Settings.WearWeightNot);
            }
            else
            {
                Settings.WearWeightNot = LoadString;
            }
            LoadString = ReadWriteString("String", "ItemIsNotThisAccount", "");
            if (LoadString == "")
            {
                WriteString("String", "ItemIsNotThisAccount", Settings.ItemIsNotThisAccount);
            }
            else
            {
                Settings.ItemIsNotThisAccount = LoadString;
            }
            LoadString = ReadWriteString("String", "ItemIsNotThisIPaddr", "");
            if (LoadString == "")
            {
                WriteString("String", "ItemIsNotThisIPaddr", Settings.ItemIsNotThisIPaddr);
            }
            else
            {
                Settings.ItemIsNotThisIPaddr = LoadString;
            }
            LoadString = ReadWriteString("String", "ItemIsNotThisChrName", "");
            if (LoadString == "")
            {
                WriteString("String", "ItemIsNotThisChrName", Settings.ItemIsNotThisChrName);
            }
            else
            {
                Settings.ItemIsNotThisChrName = LoadString;
            }
            LoadString = ReadWriteString("String", "LevelNot", "");
            if (LoadString == "")
            {
                WriteString("String", "LevelNot", Settings.LevelNot);
            }
            else
            {
                Settings.LevelNot = LoadString;
            }
            LoadString = ReadWriteString("String", "JobOrLevelNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrLevelNot", Settings.JobOrLevelNot);
            }
            else
            {
                Settings.JobOrLevelNot = LoadString;
            }
            LoadString = ReadWriteString("String", "JobOrDCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrDCNot", Settings.JobOrDCNot);
            }
            else
            {
                Settings.JobOrDCNot = LoadString;
            }
            LoadString = ReadWriteString("String", "JobOrMCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrMCNot", Settings.JobOrMCNot);
            }
            else
            {
                Settings.JobOrMCNot = LoadString;
            }
            LoadString = ReadWriteString("String", "JobOrSCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrSCNot", Settings.JobOrSCNot);
            }
            else
            {
                Settings.JobOrSCNot = LoadString;
            }
            LoadString = ReadWriteString("String", "DCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "DCNot", Settings.DCNot);
            }
            else
            {
                Settings.DCNot = LoadString;
            }
            LoadString = ReadWriteString("String", "MCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "MCNot", Settings.MCNot);
            }
            else
            {
                Settings.MCNot = LoadString;
            }
            LoadString = ReadWriteString("String", "SCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "SCNot", Settings.SCNot);
            }
            else
            {
                Settings.SCNot = LoadString;
            }
            LoadString = ReadWriteString("String", "CreditPointNot", "");
            if (LoadString == "")
            {
                WriteString("String", "CreditPointNot", Settings.CreditPointNot);
            }
            else
            {
                Settings.CreditPointNot = LoadString;
            }
            LoadString = ReadWriteString("String", "ReNewLevelNot", "");
            if (LoadString == "")
            {
                WriteString("String", "ReNewLevelNot", Settings.ReNewLevelNot);
            }
            else
            {
                Settings.ReNewLevelNot = LoadString;
            }
            LoadString = ReadWriteString("String", "GuildNot", "");
            if (LoadString == "")
            {
                WriteString("String", "GuildNot", Settings.GuildNot);
            }
            else
            {
                Settings.GuildNot = LoadString;
            }
            LoadString = ReadWriteString("String", "GuildMasterNot", "");
            if (LoadString == "")
            {
                WriteString("String", "GuildMasterNot", Settings.GuildMasterNot);
            }
            else
            {
                Settings.GuildMasterNot = LoadString;
            }
            LoadString = ReadWriteString("String", "SabukHumanNot", "");
            if (LoadString == "")
            {
                WriteString("String", "SabukHumanNot", Settings.SabukHumanNot);
            }
            else
            {
                Settings.SabukHumanNot = LoadString;
            }
            LoadString = ReadWriteString("String", "SabukMasterManNot", "");
            if (LoadString == "")
            {
                WriteString("String", "SabukMasterManNot", Settings.SabukMasterManNot);
            }
            else
            {
                Settings.SabukMasterManNot = LoadString;
            }
            LoadString = ReadWriteString("String", "MemberNot", "");
            if (LoadString == "")
            {
                WriteString("String", "MemberNot", Settings.MemberNot);
            }
            else
            {
                Settings.MemberNot = LoadString;
            }
            LoadString = ReadWriteString("String", "MemberTypeNot", "");
            if (LoadString == "")
            {
                WriteString("String", "MemberTypeNot", Settings.MemberTypeNot);
            }
            else
            {
                Settings.MemberTypeNot = LoadString;
            }
            LoadString = ReadWriteString("String", "CanottWearIt", "");
            if (LoadString == "")
            {
                WriteString("String", "CanottWearIt", Settings.CanottWearIt);
            }
            else
            {
                Settings.CanottWearIt = LoadString;
            }
            LoadString = ReadWriteString("String", "CanotUseDrugOnThisMap", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotUseDrugOnThisMap", Settings.CanotUseDrugOnThisMap);
            }
            else
            {
                Settings.CanotUseDrugOnThisMap = LoadString;
            }
            LoadString = ReadWriteString("String", "GameMasterMode", "");
            if (LoadString == "")
            {
                WriteString("String", "GameMasterMode", Settings.GameMasterMode);
            }
            else
            {
                Settings.GameMasterMode = LoadString;
            }
            LoadString = ReadWriteString("String", "ReleaseGameMasterMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ReleaseGameMasterMode", Settings.ReleaseGameMasterMode);
            }
            else
            {
                Settings.ReleaseGameMasterMode = LoadString;
            }
            LoadString = ReadWriteString("String", "ObserverMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ObserverMode", Settings.ObserverMode);
            }
            else
            {
                Settings.ObserverMode = LoadString;
            }
            LoadString = ReadWriteString("String", "ReleaseObserverMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ReleaseObserverMode", Settings.ReleaseObserverMode);
            }
            else
            {
                Settings.ReleaseObserverMode = LoadString;
            }
            LoadString = ReadWriteString("String", "SupermanMode", "");
            if (LoadString == "")
            {
                WriteString("String", "SupermanMode", Settings.SupermanMode);
            }
            else
            {
                Settings.SupermanMode = LoadString;
            }
            LoadString = ReadWriteString("String", "ReleaseSupermanMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ReleaseSupermanMode", Settings.ReleaseSupermanMode);
            }
            else
            {
                Settings.ReleaseSupermanMode = LoadString;
            }
            LoadString = ReadWriteString("String", "YouFoundNothing", "");
            if (LoadString == "")
            {
                WriteString("String", "YouFoundNothing", Settings.YouFoundNothing);
            }
            else
            {
                Settings.YouFoundNothing = LoadString;
            }
            LoadString = ReadWriteString("String", "LineNoticePreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "LineNoticePreFix", M2Share.Config.LineNoticePreFix);
            }
            else
            {
                M2Share.Config.LineNoticePreFix = LoadString;
            }
            LoadString = ReadWriteString("String", "SysMsgPreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "SysMsgPreFix", M2Share.Config.SysMsgPreFix);
            }
            else
            {
                M2Share.Config.SysMsgPreFix = LoadString;
            }
            LoadString = ReadWriteString("String", "GuildMsgPreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "GuildMsgPreFix", M2Share.Config.GuildMsgPreFix);
            }
            else
            {
                M2Share.Config.GuildMsgPreFix = LoadString;
            }
            LoadString = ReadWriteString("String", "GroupMsgPreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "GroupMsgPreFix", M2Share.Config.GroupMsgPreFix);
            }
            else
            {
                M2Share.Config.GroupMsgPreFix = LoadString;
            }
            LoadString = ReadWriteString("String", "HintMsgPreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "HintMsgPreFix", M2Share.Config.HintMsgPreFix);
            }
            else
            {
                M2Share.Config.HintMsgPreFix = LoadString;
            }
            LoadString = ReadWriteString("String", "GMRedMsgpreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "GMRedMsgpreFix", M2Share.Config.GameManagerRedMsgPreFix);
            }
            else
            {
                M2Share.Config.GameManagerRedMsgPreFix = LoadString;
            }
            LoadString = ReadWriteString("String", "MonSayMsgpreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "MonSayMsgpreFix", M2Share.Config.MonSayMsgPreFix);
            }
            else
            {
                M2Share.Config.MonSayMsgPreFix = LoadString;
            }
            LoadString = ReadWriteString("String", "CustMsgpreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "CustMsgpreFix", M2Share.Config.CustMsgPreFix);
            }
            else
            {
                M2Share.Config.CustMsgPreFix = LoadString;
            }
            LoadString = ReadWriteString("String", "CastleMsgpreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "CastleMsgpreFix", M2Share.Config.CastleMsgPreFix);
            }
            else
            {
                M2Share.Config.CastleMsgPreFix = LoadString;
            }
            LoadString = ReadWriteString("String", "NoPasswordLockSystemMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "NoPasswordLockSystemMsg", Settings.NoPasswordLockSystemMsg);
            }
            else
            {
                Settings.NoPasswordLockSystemMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "AlreadySetPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "AlreadySetPassword", Settings.AlreadySetPasswordMsg);
            }
            else
            {
                Settings.AlreadySetPasswordMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ReSetPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "ReSetPassword", Settings.ReSetPasswordMsg);
            }
            else
            {
                Settings.ReSetPasswordMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "PasswordOverLong", "");
            if (LoadString == "")
            {
                WriteString("String", "PasswordOverLong", Settings.PasswordOverLongMsg);
            }
            else
            {
                Settings.PasswordOverLongMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ReSetPasswordOK", "");
            if (LoadString == "")
            {
                WriteString("String", "ReSetPasswordOK", Settings.ReSetPasswordOKMsg);
            }
            else
            {
                Settings.ReSetPasswordOKMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ReSetPasswordNotMatch", "");
            if (LoadString == "")
            {
                WriteString("String", "ReSetPasswordNotMatch", Settings.ReSetPasswordNotMatchMsg);
            }
            else
            {
                Settings.ReSetPasswordNotMatchMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "PleaseInputUnLockPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseInputUnLockPassword", Settings.PleaseInputUnLockPasswordMsg);
            }
            else
            {
                Settings.PleaseInputUnLockPasswordMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "StorageUnLockOK", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageUnLockOK", Settings.StorageUnLockOKMsg);
            }
            else
            {
                Settings.StorageUnLockOKMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "StorageAlreadyUnLock", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageAlreadyUnLock", Settings.StorageAlreadyUnLockMsg);
            }
            else
            {
                Settings.StorageAlreadyUnLockMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "StorageNoPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageNoPassword", Settings.StorageNoPasswordMsg);
            }
            else
            {
                Settings.StorageNoPasswordMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "UnLockPasswordFail", "");
            if (LoadString == "")
            {
                WriteString("String", "UnLockPasswordFail", Settings.UnLockPasswordFailMsg);
            }
            else
            {
                Settings.UnLockPasswordFailMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "LockStorageSuccess", "");
            if (LoadString == "")
            {
                WriteString("String", "LockStorageSuccess", Settings.LockStorageSuccessMsg);
            }
            else
            {
                Settings.LockStorageSuccessMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "StoragePasswordClearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StoragePasswordClearMsg", Settings.StoragePasswordClearMsg);
            }
            else
            {
                Settings.StoragePasswordClearMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "PleaseUnloadStoragePasswordMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseUnloadStoragePasswordMsg", Settings.PleaseUnloadStoragePasswordMsg);
            }
            else
            {
                Settings.PleaseUnloadStoragePasswordMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "StorageAlreadyLock", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageAlreadyLock", Settings.StorageAlreadyLockMsg);
            }
            else
            {
                Settings.StorageAlreadyLockMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "StoragePasswordLocked", "");
            if (LoadString == "")
            {
                WriteString("String", "StoragePasswordLocked", Settings.StoragePasswordLockedMsg);
            }
            else
            {
                Settings.StoragePasswordLockedMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "StorageSetPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageSetPassword", Settings.SetPasswordMsg);
            }
            else
            {
                Settings.SetPasswordMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "PleaseInputOldPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseInputOldPassword", Settings.PleaseInputOldPasswordMsg);
            }
            else
            {
                Settings.PleaseInputOldPasswordMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "PasswordIsClearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PasswordIsClearMsg", Settings.OldPasswordIsClearMsg);
            }
            else
            {
                Settings.OldPasswordIsClearMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "NoPasswordSet", "");
            if (LoadString == "")
            {
                WriteString("String", "NoPasswordSet", Settings.NoPasswordSetMsg);
            }
            else
            {
                Settings.NoPasswordSetMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "OldPasswordIncorrect", "");
            if (LoadString == "")
            {
                WriteString("String", "OldPasswordIncorrect", Settings.OldPasswordIncorrectMsg);
            }
            else
            {
                Settings.OldPasswordIncorrectMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "StorageIsLocked", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageIsLocked", Settings.StorageIsLockedMsg);
            }
            else
            {
                Settings.StorageIsLockedMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "PleaseTryDealLaterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseTryDealLaterMsg", Settings.PleaseTryDealLaterMsg);
            }
            else
            {
                Settings.PleaseTryDealLaterMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "DealItemsDenyGetBackMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DealItemsDenyGetBackMsg", Settings.DealItemsDenyGetBackMsg);
            }
            else
            {
                Settings.DealItemsDenyGetBackMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "DisableDealItemsMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableDealItemsMsg", Settings.DisableDealItemsMsg);
            }
            else
            {
                Settings.DisableDealItemsMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "CanotTryDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotTryDealMsg", Settings.CanotTryDealMsg);
            }
            else
            {
                Settings.CanotTryDealMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "DealActionCancelMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DealActionCancelMsg", Settings.DealActionCancelMsg);
            }
            else
            {
                Settings.DealActionCancelMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "PoseDisableDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PoseDisableDealMsg", Settings.PoseDisableDealMsg);
            }
            else
            {
                Settings.PoseDisableDealMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "DealSuccessMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DealSuccessMsg", Settings.DealSuccessMsg);
            }
            else
            {
                Settings.DealSuccessMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "DealOKTooFast", "");
            if (LoadString == "")
            {
                WriteString("String", "DealOKTooFast", Settings.DealOKTooFast);
            }
            else
            {
                Settings.DealOKTooFast = LoadString;
            }
            LoadString = ReadWriteString("String", "YourBagSizeTooSmall", "");
            if (LoadString == "")
            {
                WriteString("String", "YourBagSizeTooSmall", Settings.YourBagSizeTooSmall);
            }
            else
            {
                Settings.YourBagSizeTooSmall = LoadString;
            }
            LoadString = ReadWriteString("String", "DealHumanBagSizeTooSmall", "");
            if (LoadString == "")
            {
                WriteString("String", "DealHumanBagSizeTooSmall", Settings.DealHumanBagSizeTooSmall);
            }
            else
            {
                Settings.DealHumanBagSizeTooSmall = LoadString;
            }
            LoadString = ReadWriteString("String", "YourGoldLargeThenLimit", "");
            if (LoadString == "")
            {
                WriteString("String", "YourGoldLargeThenLimit", Settings.YourGoldLargeThenLimit);
            }
            else
            {
                Settings.YourGoldLargeThenLimit = LoadString;
            }
            LoadString = ReadWriteString("String", "DealHumanGoldLargeThenLimit", "");
            if (LoadString == "")
            {
                WriteString("String", "DealHumanGoldLargeThenLimit", Settings.DealHumanGoldLargeThenLimit);
            }
            else
            {
                Settings.DealHumanGoldLargeThenLimit = LoadString;
            }
            LoadString = ReadWriteString("String", "YouDealOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouDealOKMsg", Settings.YouDealOKMsg);
            }
            else
            {
                Settings.YouDealOKMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "PoseDealOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PoseDealOKMsg", Settings.PoseDealOKMsg);
            }
            else
            {
                Settings.PoseDealOKMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "KickClientUserMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "KickClientUserMsg", Settings.KickClientUserMsg);
            }
            else
            {
                Settings.KickClientUserMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ActionIsLockedMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ActionIsLockedMsg", Settings.ActionIsLockedMsg);
            }
            else
            {
                Settings.ActionIsLockedMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "PasswordNotSetMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PasswordNotSetMsg", Settings.PasswordNotSetMsg);
            }
            else
            {
                Settings.PasswordNotSetMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "NotPasswordProtectMode", "");
            if (LoadString == "")
            {
                WriteString("String", "NotPasswordProtectMode", Settings.NotPasswordProtectMode);
            }
            else
            {
                Settings.NotPasswordProtectMode = LoadString;
            }
            LoadString = ReadWriteString("String", "CanotDropGoldMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropGoldMsg", Settings.CanotDropGoldMsg);
            }
            else
            {
                Settings.CanotDropGoldMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "CanotDropInSafeZoneMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropInSafeZoneMsg", Settings.CanotDropInSafeZoneMsg);
            }
            else
            {
                Settings.CanotDropInSafeZoneMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "CanotDropItemMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropItemMsg", Settings.CanotDropItemMsg);
            }
            else
            {
                Settings.CanotDropItemMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "CanotDropItemMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropItemMsg", Settings.CanotDropItemMsg);
            }
            else
            {
                Settings.CanotDropItemMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "CanotUseItemMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotUseItemMsg", Settings.CanotUseItemMsg);
            }
            else
            {
                Settings.CanotUseItemMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "StartMarryManMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryManMsg", Settings.StartMarryManMsg);
            }
            else
            {
                Settings.StartMarryManMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "StartMarryWoManMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryWoManMsg", Settings.StartMarryWoManMsg);
            }
            else
            {
                Settings.StartMarryWoManMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "StartMarryManAskQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryManAskQuestionMsg", Settings.StartMarryManAskQuestionMsg);
            }
            else
            {
                Settings.StartMarryManAskQuestionMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "StartMarryWoManAskQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryWoManAskQuestionMsg", Settings.StartMarryWoManAskQuestionMsg);
            }
            else
            {
                Settings.StartMarryWoManAskQuestionMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "MarryManAnswerQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryManAnswerQuestionMsg", Settings.MarryManAnswerQuestionMsg);
            }
            else
            {
                Settings.MarryManAnswerQuestionMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "MarryManAskQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryManAskQuestionMsg", Settings.MarryManAskQuestionMsg);
            }
            else
            {
                Settings.MarryManAskQuestionMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "MarryWoManAnswerQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManAnswerQuestionMsg", Settings.MarryWoManAnswerQuestionMsg);
            }
            else
            {
                Settings.MarryWoManAnswerQuestionMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "MarryWoManGetMarryMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManGetMarryMsg", Settings.MarryWoManGetMarryMsg);
            }
            else
            {
                Settings.MarryWoManGetMarryMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "MarryWoManDenyMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManDenyMsg", Settings.MarryWoManDenyMsg);
            }
            else
            {
                Settings.MarryWoManDenyMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "MarryWoManCancelMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManCancelMsg", Settings.MarryWoManCancelMsg);
            }
            else
            {
                Settings.MarryWoManCancelMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ForceUnMarryManLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ForceUnMarryManLoginMsg", Settings.fUnMarryManLoginMsg);
            }
            else
            {
                Settings.fUnMarryManLoginMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ForceUnMarryWoManLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ForceUnMarryWoManLoginMsg", Settings.fUnMarryWoManLoginMsg);
            }
            else
            {
                Settings.fUnMarryWoManLoginMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ManLoginDearOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLoginDearOnlineSelfMsg", Settings.ManLoginDearOnlineSelfMsg);
            }
            else
            {
                Settings.ManLoginDearOnlineSelfMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ManLoginDearOnlineDearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLoginDearOnlineDearMsg", Settings.ManLoginDearOnlineDearMsg);
            }
            else
            {
                Settings.ManLoginDearOnlineDearMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "WoManLoginDearOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLoginDearOnlineSelfMsg", Settings.WoManLoginDearOnlineSelfMsg);
            }
            else
            {
                Settings.WoManLoginDearOnlineSelfMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "WoManLoginDearOnlineDearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLoginDearOnlineDearMsg", Settings.WoManLoginDearOnlineDearMsg);
            }
            else
            {
                Settings.WoManLoginDearOnlineDearMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ManLoginDearNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLoginDearNotOnlineMsg", Settings.ManLoginDearNotOnlineMsg);
            }
            else
            {
                Settings.ManLoginDearNotOnlineMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "WoManLoginDearNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLoginDearNotOnlineMsg", Settings.WoManLoginDearNotOnlineMsg);
            }
            else
            {
                Settings.WoManLoginDearNotOnlineMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ManLongOutDearOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLongOutDearOnlineMsg", Settings.ManLongOutDearOnlineMsg);
            }
            else
            {
                Settings.ManLongOutDearOnlineMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "WoManLongOutDearOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLongOutDearOnlineMsg", Settings.WoManLongOutDearOnlineMsg);
            }
            else
            {
                Settings.WoManLongOutDearOnlineMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YouAreNotMarryedMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouAreNotMarryedMsg", Settings.YouAreNotMarryedMsg);
            }
            else
            {
                Settings.YouAreNotMarryedMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YourWifeNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourWifeNotOnlineMsg", Settings.YourWifeNotOnlineMsg);
            }
            else
            {
                Settings.YourWifeNotOnlineMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YourHusbandNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourHusbandNotOnlineMsg", Settings.YourHusbandNotOnlineMsg);
            }
            else
            {
                Settings.YourHusbandNotOnlineMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YourWifeNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourWifeNowLocateMsg", Settings.YourWifeNowLocateMsg);
            }
            else
            {
                Settings.YourWifeNowLocateMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YourHusbandSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourHusbandSearchLocateMsg", Settings.YourHusbandSearchLocateMsg);
            }
            else
            {
                Settings.YourHusbandSearchLocateMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YourHusbandNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourHusbandNowLocateMsg", Settings.YourHusbandNowLocateMsg);
            }
            else
            {
                Settings.YourHusbandNowLocateMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YourWifeSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourWifeSearchLocateMsg", Settings.YourWifeSearchLocateMsg);
            }
            else
            {
                Settings.YourWifeSearchLocateMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "FUnMasterLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "FUnMasterLoginMsg", Settings.fUnMasterLoginMsg);
            }
            else
            {
                Settings.fUnMasterLoginMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "UnMasterListLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "UnMasterListLoginMsg", Settings.fUnMasterListLoginMsg);
            }
            else
            {
                Settings.fUnMasterListLoginMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "MasterListOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListOnlineSelfMsg", Settings.MasterListOnlineSelfMsg);
            }
            else
            {
                Settings.MasterListOnlineSelfMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "MasterListOnlineMasterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListOnlineMasterMsg", Settings.MasterListOnlineMasterMsg);
            }
            else
            {
                Settings.MasterListOnlineMasterMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "MasterOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterOnlineSelfMsg", Settings.MasterOnlineSelfMsg);
            }
            else
            {
                Settings.MasterOnlineSelfMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "MasterOnlineMasterListMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterOnlineMasterListMsg", Settings.MasterOnlineMasterListMsg);
            }
            else
            {
                Settings.MasterOnlineMasterListMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "MasterLongOutMasterListOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterLongOutMasterListOnlineMsg", Settings.MasterLongOutMasterListOnlineMsg);
            }
            else
            {
                Settings.MasterLongOutMasterListOnlineMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "MasterListLongOutMasterOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListLongOutMasterOnlineMsg", Settings.MasterListLongOutMasterOnlineMsg);
            }
            else
            {
                Settings.MasterListLongOutMasterOnlineMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "MasterListNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListNotOnlineMsg", Settings.MasterListNotOnlineMsg);
            }
            else
            {
                Settings.MasterListNotOnlineMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "MasterNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterNotOnlineMsg", Settings.MasterNotOnlineMsg);
            }
            else
            {
                Settings.MasterNotOnlineMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YouAreNotMasterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouAreNotMasterMsg", Settings.YouAreNotMasterMsg);
            }
            else
            {
                Settings.YouAreNotMasterMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YourMasterNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterNotOnlineMsg", Settings.YourMasterNotOnlineMsg);
            }
            else
            {
                Settings.YourMasterNotOnlineMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YourMasterListNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListNotOnlineMsg", Settings.YourMasterListNotOnlineMsg);
            }
            else
            {
                Settings.YourMasterListNotOnlineMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YourMasterNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterNowLocateMsg", Settings.YourMasterNowLocateMsg);
            }
            else
            {
                Settings.YourMasterNowLocateMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YourMasterListSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListSearchLocateMsg", Settings.YourMasterListSearchLocateMsg);
            }
            else
            {
                Settings.YourMasterListSearchLocateMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YourMasterListNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListNowLocateMsg", Settings.YourMasterListNowLocateMsg);
            }
            else
            {
                Settings.YourMasterListNowLocateMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YourMasterSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterSearchLocateMsg", Settings.YourMasterSearchLocateMsg);
            }
            else
            {
                Settings.YourMasterSearchLocateMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YourMasterListUnMasterOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListUnMasterOKMsg", Settings.YourMasterListUnMasterOKMsg);
            }
            else
            {
                Settings.YourMasterListUnMasterOKMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YouAreUnMasterOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouAreUnMasterOKMsg", Settings.YouAreUnMasterOKMsg);
            }
            else
            {
                Settings.YouAreUnMasterOKMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "UnMasterLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "UnMasterLoginMsg", Settings.UnMasterLoginMsg);
            }
            else
            {
                Settings.UnMasterLoginMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "NPCSayUnMasterOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "NPCSayUnMasterOKMsg", Settings.NPCSayUnMasterOKMsg);
            }
            else
            {
                Settings.NPCSayUnMasterOKMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "NPCSayForceUnMasterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "NPCSayForceUnMasterMsg", Settings.NPCSayForceUnMasterMsg);
            }
            else
            {
                Settings.NPCSayForceUnMasterMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "MyInfo", "");
            if (LoadString == "")
            {
                WriteString("String", "MyInfo", Settings.MyInfo);
            }
            else
            {
                Settings.MyInfo = LoadString;
            }
            LoadString = ReadWriteString("String", "OpenedDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "OpenedDealMsg", Settings.OpenedDealMsg);
            }
            else
            {
                Settings.OpenedDealMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "SendCustMsgCanNotUseNowMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "SendCustMsgCanNotUseNowMsg", Settings.SendCustMsgCanNotUseNowMsg);
            }
            else
            {
                Settings.SendCustMsgCanNotUseNowMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "SubkMasterMsgCanNotUseNowMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "SubkMasterMsgCanNotUseNowMsg", Settings.SubkMasterMsgCanNotUseNowMsg);
            }
            else
            {
                Settings.SubkMasterMsgCanNotUseNowMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "SendOnlineCountMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "SendOnlineCountMsg", Settings.SendOnlineCountMsg);
            }
            else
            {
                Settings.SendOnlineCountMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "WeaponRepairSuccess", "");
            if (LoadString == "")
            {
                WriteString("String", "WeaponRepairSuccess", Settings.WeaponRepairSuccess);
            }
            else
            {
                Settings.WeaponRepairSuccess = LoadString;
            }
            LoadString = ReadWriteString("String", "DefenceUpTime", "");
            if (LoadString == "")
            {
                WriteString("String", "DefenceUpTime", Settings.DefenceUpTime);
            }
            else
            {
                Settings.DefenceUpTime = LoadString;
            }
            LoadString = ReadWriteString("String", "MagDefenceUpTime", "");
            if (LoadString == "")
            {
                WriteString("String", "MagDefenceUpTime", Settings.MagDefenceUpTime);
            }
            else
            {
                Settings.MagDefenceUpTime = LoadString;
            }
            LoadString = ReadWriteString("String", "WinLottery1Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery1Msg", Settings.WinLottery1Msg);
            }
            else
            {
                Settings.WinLottery1Msg = LoadString;
            }
            LoadString = ReadWriteString("String", "WinLottery2Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery2Msg", Settings.WinLottery2Msg);
            }
            else
            {
                Settings.WinLottery2Msg = LoadString;
            }
            LoadString = ReadWriteString("String", "WinLottery3Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery3Msg", Settings.WinLottery3Msg);
            }
            else
            {
                Settings.WinLottery3Msg = LoadString;
            }
            LoadString = ReadWriteString("String", "WinLottery4Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery4Msg", Settings.WinLottery4Msg);
            }
            else
            {
                Settings.WinLottery4Msg = LoadString;
            }
            LoadString = ReadWriteString("String", "WinLottery5Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery5Msg", Settings.WinLottery5Msg);
            }
            else
            {
                Settings.WinLottery5Msg = LoadString;
            }
            LoadString = ReadWriteString("String", "WinLottery6Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery6Msg", Settings.WinLottery6Msg);
            }
            else
            {
                Settings.WinLottery6Msg = LoadString;
            }
            LoadString = ReadWriteString("String", "NotWinLotteryMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "NotWinLotteryMsg", Settings.NotWinLotteryMsg);
            }
            else
            {
                Settings.NotWinLotteryMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "WeaptonMakeLuck", "");
            if (LoadString == "")
            {
                WriteString("String", "WeaptonMakeLuck", Settings.WeaptonMakeLuck);
            }
            else
            {
                Settings.WeaptonMakeLuck = LoadString;
            }
            LoadString = ReadWriteString("String", "WeaptonNotMakeLuck", "");
            if (LoadString == "")
            {
                WriteString("String", "WeaptonNotMakeLuck", Settings.WeaptonNotMakeLuck);
            }
            else
            {
                Settings.WeaptonNotMakeLuck = LoadString;
            }
            LoadString = ReadWriteString("String", "TheWeaponIsCursed", "");
            if (LoadString == "")
            {
                WriteString("String", "TheWeaponIsCursed", Settings.TheWeaponIsCursed);
            }
            else
            {
                Settings.TheWeaponIsCursed = LoadString;
            }
            LoadString = ReadWriteString("String", "CanotTakeOffItem", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotTakeOffItem", Settings.CanotTakeOffItem);
            }
            else
            {
                Settings.CanotTakeOffItem = LoadString;
            }
            LoadString = ReadWriteString("String", "JoinGroupMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "JoinGroupMsg", Settings.JoinGroup);
            }
            else
            {
                Settings.JoinGroup = LoadString;
            }
            LoadString = ReadWriteString("String", "TryModeCanotUseStorage", "");
            if (LoadString == "")
            {
                WriteString("String", "TryModeCanotUseStorage", Settings.TryModeCanotUseStorage);
            }
            else
            {
                Settings.TryModeCanotUseStorage = LoadString;
            }
            LoadString = ReadWriteString("String", "CanotGetItemsMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotGetItemsMsg", Settings.CanotGetItems);
            }
            else
            {
                Settings.CanotGetItems = LoadString;
            }
            LoadString = ReadWriteString("String", "EnableDearRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableDearRecall", CommandHelp.EnableDearRecall);
            }
            else
            {
                CommandHelp.EnableDearRecall = LoadString;
            }
            LoadString = ReadWriteString("String", "DisableDearRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableDearRecall", CommandHelp.DisableDearRecall);
            }
            else
            {
                CommandHelp.DisableDearRecall = LoadString;
            }
            LoadString = ReadWriteString("String", "EnableMasterRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableMasterRecall", CommandHelp.EnableMasterRecall);
            }
            else
            {
                CommandHelp.EnableMasterRecall = LoadString;
            }
            LoadString = ReadWriteString("String", "DisableMasterRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableMasterRecall", CommandHelp.DisableMasterRecall);
            }
            else
            {
                CommandHelp.DisableMasterRecall = LoadString;
            }
            LoadString = ReadWriteString("String", "NowCurrDateTime", "");
            if (LoadString == "")
            {
                WriteString("String", "NowCurrDateTime", CommandHelp.NowCurrDateTime);
            }
            else
            {
                CommandHelp.NowCurrDateTime = LoadString;
            }
            LoadString = ReadWriteString("String", "EnableHearWhisper", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableHearWhisper", CommandHelp.EnableHearWhisper);
            }
            else
            {
                CommandHelp.EnableHearWhisper = LoadString;
            }
            LoadString = ReadWriteString("String", "DisableHearWhisper", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableHearWhisper", CommandHelp.DisableHearWhisper);
            }
            else
            {
                CommandHelp.DisableHearWhisper = LoadString;
            }
            LoadString = ReadWriteString("String", "EnableShoutMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableShoutMsg", CommandHelp.EnableShoutMsg);
            }
            else
            {
                CommandHelp.EnableShoutMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "DisableShoutMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableShoutMsg", CommandHelp.DisableShoutMsg);
            }
            else
            {
                CommandHelp.DisableShoutMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "EnableDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableDealMsg", CommandHelp.EnableDealMsg);
            }
            else
            {
                CommandHelp.EnableDealMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "DisableDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableDealMsg", CommandHelp.DisableDealMsg);
            }
            else
            {
                CommandHelp.DisableDealMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "EnableGuildChat", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableGuildChat", CommandHelp.EnableGuildChat);
            }
            else
            {
                CommandHelp.EnableGuildChat = LoadString;
            }
            LoadString = ReadWriteString("String", "DisableGuildChat", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableGuildChat", CommandHelp.DisableGuildChat);
            }
            else
            {
                CommandHelp.DisableGuildChat = LoadString;
            }
            LoadString = ReadWriteString("String", "EnableJoinGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableJoinGuild", CommandHelp.EnableJoinGuild);
            }
            else
            {
                CommandHelp.EnableJoinGuild = LoadString;
            }
            LoadString = ReadWriteString("String", "DisableJoinGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableJoinGuild", CommandHelp.DisableJoinGuild);
            }
            else
            {
                CommandHelp.DisableJoinGuild = LoadString;
            }
            LoadString = ReadWriteString("String", "EnableAuthAllyGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableAuthAllyGuild", CommandHelp.EnableAuthAllyGuild);
            }
            else
            {
                CommandHelp.EnableAuthAllyGuild = LoadString;
            }
            LoadString = ReadWriteString("String", "DisableAuthAllyGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableAuthAllyGuild", CommandHelp.DisableAuthAllyGuild);
            }
            else
            {
                CommandHelp.DisableAuthAllyGuild = LoadString;
            }
            LoadString = ReadWriteString("String", "EnableGroupRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableGroupRecall", CommandHelp.EnableGroupRecall);
            }
            else
            {
                CommandHelp.EnableGroupRecall = LoadString;
            }
            LoadString = ReadWriteString("String", "DisableGroupRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableGroupRecall", CommandHelp.DisableGroupRecall);
            }
            else
            {
                CommandHelp.DisableGroupRecall = LoadString;
            }
            LoadString = ReadWriteString("String", "EnableGuildRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableGuildRecall", CommandHelp.EnableGuildRecall);
            }
            else
            {
                CommandHelp.EnableGuildRecall = LoadString;
            }
            LoadString = ReadWriteString("String", "DisableGuildRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableGuildRecall", CommandHelp.DisableGuildRecall);
            }
            else
            {
                CommandHelp.DisableGuildRecall = LoadString;
            }
            LoadString = ReadWriteString("String", "PleaseInputPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseInputPassword", CommandHelp.PleaseInputPassword);
            }
            else
            {
                CommandHelp.PleaseInputPassword = LoadString;
            }
            LoadString = ReadWriteString("String", "TheMapDisableMove", "");
            if (LoadString == "")
            {
                WriteString("String", "TheMapDisableMove", CommandHelp.TheMapDisableMove);
            }
            else
            {
                CommandHelp.TheMapDisableMove = LoadString;
            }
            LoadString = ReadWriteString("String", "TheMapNotFound", "");
            if (LoadString == "")
            {
                WriteString("String", "TheMapNotFound", CommandHelp.TheMapNotFound);
            }
            else
            {
                CommandHelp.TheMapNotFound = LoadString;
            }
            LoadString = ReadWriteString("String", "YourIPaddrDenyLogon", "");
            if (LoadString == "")
            {
                WriteString("String", "YourIPaddrDenyLogon", Settings.YourIPaddrDenyLogon);
            }
            else
            {
                Settings.YourIPaddrDenyLogon = LoadString;
            }
            LoadString = ReadWriteString("String", "YourAccountDenyLogon", "");
            if (LoadString == "")
            {
                WriteString("String", "YourAccountDenyLogon", Settings.YourAccountDenyLogon);
            }
            else
            {
                Settings.YourAccountDenyLogon = LoadString;
            }
            LoadString = ReadWriteString("String", "YourChrNameDenyLogon", "");
            if (LoadString == "")
            {
                WriteString("String", "YourChrNameDenyLogon", Settings.YourChrNameDenyLogon);
            }
            else
            {
                Settings.YourChrNameDenyLogon = LoadString;
            }
            LoadString = ReadWriteString("String", "CanotPickUpItem", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotPickUpItem", Settings.CanotPickUpItem);
            }
            else
            {
                Settings.CanotPickUpItem = LoadString;
            }
            LoadString = ReadWriteString("String", "sQUERYBAGITEMS", "");
            if (LoadString == "")
            {
                WriteString("String", "sQUERYBAGITEMS", Settings.QUERYBAGITEMS);
            }
            else
            {
                Settings.QUERYBAGITEMS = LoadString;
            }
            LoadString = ReadWriteString("String", "CanotSendmsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotSendmsg", Settings.CanotSendmsg);
            }
            else
            {
                Settings.CanotSendmsg = LoadString;
            }
            LoadString = ReadWriteString("String", "UserDenyWhisperMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "UserDenyWhisperMsg", Settings.UserDenyWhisperMsg);
            }
            else
            {
                Settings.UserDenyWhisperMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "UserNotOnLine", "");
            if (LoadString == "")
            {
                WriteString("String", "UserNotOnLine", Settings.UserNotOnLine);
            }
            else
            {
                Settings.UserNotOnLine = LoadString;
            }
            LoadString = ReadWriteString("String", "RevivalRecoverMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "RevivalRecoverMsg", Settings.RevivalRecoverMsg);
            }
            else
            {
                Settings.RevivalRecoverMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ClientVersionTooOld", "");
            if (LoadString == "")
            {
                WriteString("String", "ClientVersionTooOld", Settings.ClientVersionTooOld);
            }
            else
            {
                Settings.ClientVersionTooOld = LoadString;
            }
            LoadString = ReadWriteString("String", "CastleGuildName", "");
            if (LoadString == "")
            {
                WriteString("String", "CastleGuildName", Settings.CastleGuildName);
            }
            else
            {
                Settings.CastleGuildName = LoadString;
            }
            LoadString = ReadWriteString("String", "NoCastleGuildName", "");
            if (LoadString == "")
            {
                WriteString("String", "NoCastleGuildName", Settings.NoCastleGuildName);
            }
            else
            {
                Settings.NoCastleGuildName = LoadString;
            }
            LoadString = ReadWriteString("String", "WarrReNewName", "");
            if (LoadString == "")
            {
                WriteString("String", "WarrReNewName", Settings.WarrReNewName);
            }
            else
            {
                Settings.WarrReNewName = LoadString;
            }
            LoadString = ReadWriteString("String", "WizardReNewName", "");
            if (LoadString == "")
            {
                WriteString("String", "WizardReNewName", Settings.WizardReNewName);
            }
            else
            {
                Settings.WizardReNewName = LoadString;
            }
            LoadString = ReadWriteString("String", "TaosReNewName", "");
            if (LoadString == "")
            {
                WriteString("String", "TaosReNewName", Settings.TaosReNewName);
            }
            else
            {
                Settings.TaosReNewName = LoadString;
            }
            LoadString = ReadWriteString("String", "RankLevelName", "");
            if (LoadString == "")
            {
                WriteString("String", "RankLevelName", Settings.RankLevelName);
            }
            else
            {
                Settings.RankLevelName = LoadString.Replace("%s", "{0}");
            }
            LoadString = ReadWriteString("String", "ManDearName", "");
            if (LoadString == "")
            {
                WriteString("String", "ManDearName", Settings.ManDearName);
            }
            else
            {
                Settings.ManDearName = LoadString;
            }
            LoadString = ReadWriteString("String", "WoManDearName", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManDearName", Settings.WoManDearName);
            }
            else
            {
                Settings.WoManDearName = LoadString;
            }
            LoadString = ReadWriteString("String", "MasterName", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterName", Settings.MasterName);
            }
            else
            {
                Settings.MasterName = LoadString;
            }

            LoadString = ReadWriteString("String", "NoMasterName", "");
            if (LoadString == "")
            {
                WriteString("String", "NoMasterName", Settings.NoMasterName);
            }
            else
            {
                Settings.NoMasterName = LoadString;
            }
            LoadString = ReadWriteString("String", "HumanShowName", "");
            if (LoadString == "")
            {
                WriteString("String", "HumanShowName", Settings.HumanShowName);
            }
            else
            {
                Settings.HumanShowName = LoadString;
            }
            LoadString = ReadWriteString("String", "ChangePermissionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangePermissionMsg", Settings.ChangePermissionMsg);
            }
            else
            {
                Settings.ChangePermissionMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ChangeKillMonExpRateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangeKillMonExpRateMsg", Settings.ChangeKillMonExpRateMsg);
            }
            else
            {
                Settings.ChangeKillMonExpRateMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ChangePowerRateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangePowerRateMsg", Settings.ChangePowerRateMsg);
            }
            else
            {
                Settings.ChangePowerRateMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ChangeMemberLevelMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangeMemberLevelMsg", Settings.ChangeMemberLevelMsg);
            }
            else
            {
                Settings.ChangeMemberLevelMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ChangeMemberTypeMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangeMemberTypeMsg", Settings.ChangeMemberTypeMsg);
            }
            else
            {
                Settings.ChangeMemberTypeMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ScriptChangeHumanHPMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ScriptChangeHumanHPMsg", Settings.ScriptChangeHumanHPMsg);
            }
            else
            {
                Settings.ScriptChangeHumanHPMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ScriptChangeHumanMPMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ScriptChangeHumanMPMsg", Settings.ScriptChangeHumanMPMsg);
            }
            else
            {
                Settings.ScriptChangeHumanMPMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YouCanotDisableSayMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouCanotDisableSayMsg", Settings.DisableSayMsg);
            }
            else
            {
                Settings.DisableSayMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "OnlineCountMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "OnlineCountMsg", Settings.OnlineCountMsg);
            }
            else
            {
                Settings.OnlineCountMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "TotalOnlineCountMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "TotalOnlineCountMsg", Settings.TotalOnlineCountMsg);
            }
            else
            {
                Settings.TotalOnlineCountMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YouNeedLevelSendMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouNeedLevelSendMsg", Settings.YouNeedLevelMsg);
            }
            else
            {
                Settings.YouNeedLevelMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "ThisMapDisableSendCyCyMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ThisMapDisableSendCyCyMsg", Settings.ThisMapDisableSendCyCyMsg);
            }
            else
            {
                Settings.ThisMapDisableSendCyCyMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YouCanSendCyCyLaterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouCanSendCyCyLaterMsg", Settings.YouCanSendCyCyLaterMsg);
            }
            else
            {
                Settings.YouCanSendCyCyLaterMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YouIsDisableSendMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouIsDisableSendMsg", Settings.YouIsDisableSendMsg);
            }
            else
            {
                Settings.YouIsDisableSendMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YouMurderedMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouMurderedMsg", Settings.YouMurderedMsg);
            }
            else
            {
                Settings.YouMurderedMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YouKilledByMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouKilledByMsg", Settings.YouKilledByMsg);
            }
            else
            {
                Settings.YouKilledByMsg = LoadString;
            }
            LoadString = ReadWriteString("String", "YouprotectedByLawOfDefense", "");
            if (LoadString == "")
            {
                WriteString("String", "YouprotectedByLawOfDefense", Settings.YouprotectedByLawOfDefense);
            }
            else
            {
                Settings.YouprotectedByLawOfDefense = LoadString;
            }
        }

        public string LoadConfigString(string sSection, string sIdent, string sDefault)
        {
            string result;
            string sString;
            sString = ReadWriteString(sSection, sIdent, "");
            if (sString == "")
            {
                WriteString(sSection, sIdent, sDefault);
                result = sDefault;
            }
            else
            {
                result = sString;
            }
            return result;
        }

    }
}