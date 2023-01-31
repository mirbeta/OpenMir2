using GameSvr.GameCommand;
using SystemModule.Common;

namespace GameSvr.Conf
{
    public class StringConf : IniFile
    {
        public StringConf(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadString()
        {
            string LoadString;
            if (ReadString("Server", "ServerIP", "") == "")
            {
                WriteString("Server", "ServerIP", M2Share.Config.ServerIPaddr);
            }
            M2Share.Config.ServerIPaddr = ReadString("Server", "ServerIP", M2Share.Config.ServerIPaddr);
            if (ReadString("Server", "WebSite", "") == "")
            {
                WriteString("Server", "WebSite", M2Share.Config.sWebSite);
            }
            M2Share.Config.sWebSite = ReadString("Server", "WebSite", M2Share.Config.sWebSite);
            if (ReadString("Server", "BbsSite", "") == "")
            {
                WriteString("Server", "BbsSite", M2Share.Config.sBbsSite);
            }
            M2Share.Config.sBbsSite = ReadString("Server", "BbsSite", M2Share.Config.sBbsSite);
            if (ReadString("Server", "ClientDownload", "") == "")
            {
                WriteString("Server", "ClientDownload", M2Share.Config.sClientDownload);
            }
            M2Share.Config.sClientDownload = ReadString("Server", "ClientDownload", M2Share.Config.sClientDownload);
            if (ReadString("Server", "QQ", "") == "")
            {
                WriteString("Server", "QQ", M2Share.Config.sQQ);
            }
            M2Share.Config.sQQ = ReadString("Server", "QQ", M2Share.Config.sQQ);
            if (ReadString("Server", "Phone", "") == "")
            {
                WriteString("Server", "Phone", M2Share.Config.sPhone);
            }
            M2Share.Config.sPhone = ReadString("Server", "Phone", M2Share.Config.sPhone);
            if (ReadString("Server", "BankAccount0", "") == "")
            {
                WriteString("Server", "BankAccount0", M2Share.Config.sBankAccount0);
            }
            M2Share.Config.sBankAccount0 = ReadString("Server", "BankAccount0", M2Share.Config.sBankAccount0);
            if (ReadString("Server", "BankAccount1", "") == "")
            {
                WriteString("Server", "BankAccount1", M2Share.Config.sBankAccount1);
            }
            M2Share.Config.sBankAccount1 = ReadString("Server", "BankAccount1", M2Share.Config.sBankAccount1);
            if (ReadString("Server", "BankAccount2", "") == "")
            {
                WriteString("Server", "BankAccount2", M2Share.Config.sBankAccount2);
            }
            M2Share.Config.sBankAccount2 = ReadString("Server", "BankAccount2", M2Share.Config.sBankAccount2);
            if (ReadString("Server", "BankAccount3", "") == "")
            {
                WriteString("Server", "BankAccount3", M2Share.Config.sBankAccount3);
            }
            M2Share.Config.sBankAccount3 = ReadString("Server", "BankAccount3", M2Share.Config.sBankAccount3);
            if (ReadString("Server", "BankAccount4", "") == "")
            {
                WriteString("Server", "BankAccount4", M2Share.Config.sBankAccount4);
            }
            M2Share.Config.sBankAccount4 = ReadString("Server", "BankAccount4", M2Share.Config.sBankAccount4);
            if (ReadString("Server", "BankAccount5", "") == "")
            {
                WriteString("Server", "BankAccount5", M2Share.Config.sBankAccount5);
            }
            M2Share.Config.sBankAccount5 = ReadString("Server", "BankAccount5", M2Share.Config.sBankAccount5);
            if (ReadString("Server", "BankAccount6", "") == "")
            {
                WriteString("Server", "BankAccount6", M2Share.Config.sBankAccount6);
            }
            M2Share.Config.sBankAccount6 = ReadString("Server", "BankAccount6", M2Share.Config.sBankAccount6);
            if (ReadString("Server", "BankAccount7", "") == "")
            {
                WriteString("Server", "BankAccount7", M2Share.Config.sBankAccount7);
            }
            M2Share.Config.sBankAccount7 = ReadString("Server", "BankAccount7", M2Share.Config.sBankAccount7);
            if (ReadString("Server", "BankAccount8", "") == "")
            {
                WriteString("Server", "BankAccount8", M2Share.Config.sBankAccount8);
            }
            M2Share.Config.sBankAccount8 = ReadString("Server", "BankAccount8", M2Share.Config.sBankAccount8);
            if (ReadString("Server", "BankAccount9", "") == "")
            {
                WriteString("Server", "BankAccount9", M2Share.Config.sBankAccount9);
            }
            M2Share.Config.sBankAccount9 = ReadString("Server", "BankAccount9", M2Share.Config.sBankAccount9);
            if (ReadString("Guild", "GuildNotice", "") == "")
            {
                WriteString("Guild", "GuildNotice", M2Share.Config.GuildNotice);
            }
            M2Share.Config.GuildNotice = ReadString("Guild", "GuildNotice", M2Share.Config.GuildNotice);
            if (ReadString("Guild", "GuildWar", "") == "")
            {
                WriteString("Guild", "GuildWar", M2Share.Config.GuildWar);
            }
            M2Share.Config.GuildWar = ReadString("Guild", "GuildWar", M2Share.Config.GuildWar);
            if (ReadString("Guild", "GuildAll", "") == "")
            {
                WriteString("Guild", "GuildAll", M2Share.Config.GuildAll);
            }
            M2Share.Config.GuildAll = ReadString("Guild", "GuildAll", M2Share.Config.GuildAll);
            if (ReadString("Guild", "GuildMember", "") == "")
            {
                WriteString("Guild", "GuildMember", M2Share.Config.GuildMember);
            }
            M2Share.Config.GuildMember = ReadString("Guild", "GuildMember", M2Share.Config.GuildMember);
            if (ReadString("Guild", "GuildMemberRank", "") == "")
            {
                WriteString("Guild", "GuildMemberRank", M2Share.Config.GuildMemberRank);
            }
            M2Share.Config.GuildMemberRank = ReadString("Guild", "GuildMemberRank", M2Share.Config.GuildMemberRank);
            if (ReadString("Guild", "GuildChief", "") == "")
            {
                WriteString("Guild", "GuildChief", M2Share.Config.GuildChief);
            }
            M2Share.Config.GuildChief = ReadString("Guild", "GuildChief", M2Share.Config.GuildChief);
            LoadString = ReadString("String", "ClientSoftVersionError", "");
            if (LoadString == "")
            {
                WriteString("String", "ClientSoftVersionError", Settings.ClientSoftVersionError);
            }
            else
            {
                Settings.ClientSoftVersionError = LoadString;
            }
            LoadString = ReadString("String", "DownLoadNewClientSoft", "");
            if (LoadString == "")
            {
                WriteString("String", "DownLoadNewClientSoft", Settings.DownLoadNewClientSoft);
            }
            else
            {
                Settings.DownLoadNewClientSoft = LoadString;
            }
            LoadString = ReadString("String", "ForceDisConnect", "");
            if (LoadString == "")
            {
                WriteString("String", "ForceDisConnect", Settings.ForceDisConnect);
            }
            else
            {
                Settings.ForceDisConnect = LoadString;
            }
            LoadString = ReadString("String", "ClientSoftVersionTooOld", "");
            if (LoadString == "")
            {
                WriteString("String", "ClientSoftVersionTooOld", Settings.ClientSoftVersionTooOld);
            }
            else
            {
                Settings.ClientSoftVersionTooOld = LoadString;
            }
            LoadString = ReadString("String", "DownLoadAndUseNewClient", "");
            if (LoadString == "")
            {
                WriteString("String", "DownLoadAndUseNewClient", Settings.DownLoadAndUseNewClient);
            }
            else
            {
                Settings.DownLoadAndUseNewClient = LoadString;
            }
            LoadString = ReadString("String", "OnlineUserFull", "");
            if (LoadString == "")
            {
                WriteString("String", "OnlineUserFull", Settings.OnlineUserFull);
            }
            else
            {
                Settings.OnlineUserFull = LoadString;
            }
            LoadString = ReadString("String", "YouNowIsTryPlayMode", "");
            if (LoadString == "")
            {
                WriteString("String", "YouNowIsTryPlayMode", Settings.YouNowIsTryPlayMode);
            }
            else
            {
                Settings.YouNowIsTryPlayMode = LoadString;
            }
            LoadString = ReadString("String", "NowIsFreePlayMode", "");
            if (LoadString == "")
            {
                WriteString("String", "NowIsFreePlayMode", Settings.NowIsFreePlayMode);
            }
            else
            {
                Settings.NowIsFreePlayMode = LoadString;
            }
            LoadString = ReadString("String", "AttackModeOfAll", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfAll", Settings.AttackModeOfAll);
            }
            else
            {
                Settings.AttackModeOfAll = LoadString;
            }
            LoadString = ReadString("String", "AttackModeOfPeaceful", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfPeaceful", Settings.AttackModeOfPeaceful);
            }
            else
            {
                Settings.AttackModeOfPeaceful = LoadString;
            }
            LoadString = ReadString("String", "AttackModeOfGroup", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfGroup", Settings.AttackModeOfGroup);
            }
            else
            {
                Settings.AttackModeOfGroup = LoadString;
            }
            LoadString = ReadString("String", "AttackModeOfGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfGuild", Settings.AttackModeOfGuild);
            }
            else
            {
                Settings.AttackModeOfGuild = LoadString;
            }
            LoadString = ReadString("String", "AttackModeOfRedWhite", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfRedWhite", Settings.AttackModeOfRedWhite);
            }
            else
            {
                Settings.AttackModeOfRedWhite = LoadString;
            }
            LoadString = ReadString("String", "StartChangeAttackModeHelp", "");
            if (LoadString == "")
            {
                WriteString("String", "StartChangeAttackModeHelp", Settings.StartChangeAttackModeHelp);
            }
            else
            {
                Settings.StartChangeAttackModeHelp = LoadString;
            }
            LoadString = ReadString("String", "StartNoticeMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartNoticeMsg", Settings.StartNoticeMsg);
            }
            else
            {
                Settings.StartNoticeMsg = LoadString;
            }
            LoadString = ReadString("String", "ThrustingOn", "");
            if (LoadString == "")
            {
                WriteString("String", "ThrustingOn", Settings.ThrustingOn);
            }
            else
            {
                Settings.ThrustingOn = LoadString;
            }
            LoadString = ReadString("String", "ThrustingOff", "");
            if (LoadString == "")
            {
                WriteString("String", "ThrustingOff", Settings.ThrustingOff);
            }
            else
            {
                Settings.ThrustingOff = LoadString;
            }
            LoadString = ReadString("String", "HalfMoonOn", "");
            if (LoadString == "")
            {
                WriteString("String", "HalfMoonOn", Settings.HalfMoonOn);
            }
            else
            {
                Settings.HalfMoonOn = LoadString;
            }
            LoadString = ReadString("String", "HalfMoonOff", "");
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
            LoadString = ReadString("String", "FireSpiritsSummoned", "");
            if (LoadString == "")
            {
                WriteString("String", "FireSpiritsSummoned", Settings.FireSpiritsSummoned);
            }
            else
            {
                Settings.FireSpiritsSummoned = LoadString;
            }
            LoadString = ReadString("String", "FireSpiritsFail", "");
            if (LoadString == "")
            {
                WriteString("String", "FireSpiritsFail", Settings.FireSpiritsFail);
            }
            else
            {
                Settings.FireSpiritsFail = LoadString;
            }
            LoadString = ReadString("String", "SpiritsGone", "");
            if (LoadString == "")
            {
                WriteString("String", "SpiritsGone", Settings.SpiritsGone);
            }
            else
            {
                Settings.SpiritsGone = LoadString;
            }
            LoadString = ReadString("String", "MateDoTooweak", "");
            if (LoadString == "")
            {
                WriteString("String", "MateDoTooweak", Settings.MateDoTooweak);
            }
            else
            {
                Settings.MateDoTooweak = LoadString;
            }
            LoadString = ReadString("String", "TheWeaponBroke", "");
            if (LoadString == "")
            {
                WriteString("String", "TheWeaponBroke", Settings.TheWeaponBroke);
            }
            else
            {
                Settings.TheWeaponBroke = LoadString;
            }
            LoadString = ReadString("String", "TheWeaponRefineSuccessfull", "");
            if (LoadString == "")
            {
                WriteString("String", "TheWeaponRefineSuccessfull", Settings.TheWeaponRefineSuccessfull);
            }
            else
            {
                Settings.TheWeaponRefineSuccessfull = LoadString;
            }
            LoadString = ReadString("String", "YouPoisoned", "");
            if (LoadString == "")
            {
                WriteString("String", "YouPoisoned", Settings.YouPoisoned);
            }
            else
            {
                Settings.YouPoisoned = LoadString;
            }
            LoadString = ReadString("String", "PetRest", "");
            if (LoadString == "")
            {
                WriteString("String", "PetRest", Settings.PetRest);
            }
            else
            {
                Settings.PetRest = LoadString;
            }
            LoadString = ReadString("String", "PetAttack", "");
            if (LoadString == "")
            {
                WriteString("String", "PetAttack", Settings.PetAttack);
            }
            else
            {
                Settings.PetAttack = LoadString;
            }
            LoadString = ReadString("String", "WearNotOfWoMan", "");
            if (LoadString == "")
            {
                WriteString("String", "WearNotOfWoMan", Settings.WearNotOfWoMan);
            }
            else
            {
                Settings.WearNotOfWoMan = LoadString;
            }
            LoadString = ReadString("String", "WearNotOfMan", "");
            if (LoadString == "")
            {
                WriteString("String", "WearNotOfMan", Settings.WearNotOfMan);
            }
            else
            {
                Settings.WearNotOfMan = LoadString;
            }
            LoadString = ReadString("String", "HandWeightNot", "");
            if (LoadString == "")
            {
                WriteString("String", "HandWeightNot", Settings.HandWeightNot);
            }
            else
            {
                Settings.HandWeightNot = LoadString;
            }
            LoadString = ReadString("String", "WearWeightNot", "");
            if (LoadString == "")
            {
                WriteString("String", "WearWeightNot", Settings.WearWeightNot);
            }
            else
            {
                Settings.WearWeightNot = LoadString;
            }
            LoadString = ReadString("String", "ItemIsNotThisAccount", "");
            if (LoadString == "")
            {
                WriteString("String", "ItemIsNotThisAccount", Settings.ItemIsNotThisAccount);
            }
            else
            {
                Settings.ItemIsNotThisAccount = LoadString;
            }
            LoadString = ReadString("String", "ItemIsNotThisIPaddr", "");
            if (LoadString == "")
            {
                WriteString("String", "ItemIsNotThisIPaddr", Settings.ItemIsNotThisIPaddr);
            }
            else
            {
                Settings.ItemIsNotThisIPaddr = LoadString;
            }
            LoadString = ReadString("String", "ItemIsNotThisChrName", "");
            if (LoadString == "")
            {
                WriteString("String", "ItemIsNotThisChrName", Settings.ItemIsNotThisChrName);
            }
            else
            {
                Settings.ItemIsNotThisChrName = LoadString;
            }
            LoadString = ReadString("String", "LevelNot", "");
            if (LoadString == "")
            {
                WriteString("String", "LevelNot", Settings.LevelNot);
            }
            else
            {
                Settings.LevelNot = LoadString;
            }
            LoadString = ReadString("String", "JobOrLevelNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrLevelNot", Settings.JobOrLevelNot);
            }
            else
            {
                Settings.JobOrLevelNot = LoadString;
            }
            LoadString = ReadString("String", "JobOrDCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrDCNot", Settings.JobOrDCNot);
            }
            else
            {
                Settings.JobOrDCNot = LoadString;
            }
            LoadString = ReadString("String", "JobOrMCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrMCNot", Settings.JobOrMCNot);
            }
            else
            {
                Settings.JobOrMCNot = LoadString;
            }
            LoadString = ReadString("String", "JobOrSCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrSCNot", Settings.JobOrSCNot);
            }
            else
            {
                Settings.JobOrSCNot = LoadString;
            }
            LoadString = ReadString("String", "DCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "DCNot", Settings.DCNot);
            }
            else
            {
                Settings.DCNot = LoadString;
            }
            LoadString = ReadString("String", "MCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "MCNot", Settings.MCNot);
            }
            else
            {
                Settings.MCNot = LoadString;
            }
            LoadString = ReadString("String", "SCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "SCNot", Settings.SCNot);
            }
            else
            {
                Settings.SCNot = LoadString;
            }
            LoadString = ReadString("String", "CreditPointNot", "");
            if (LoadString == "")
            {
                WriteString("String", "CreditPointNot", Settings.CreditPointNot);
            }
            else
            {
                Settings.CreditPointNot = LoadString;
            }
            LoadString = ReadString("String", "ReNewLevelNot", "");
            if (LoadString == "")
            {
                WriteString("String", "ReNewLevelNot", Settings.ReNewLevelNot);
            }
            else
            {
                Settings.ReNewLevelNot = LoadString;
            }
            LoadString = ReadString("String", "GuildNot", "");
            if (LoadString == "")
            {
                WriteString("String", "GuildNot", Settings.GuildNot);
            }
            else
            {
                Settings.GuildNot = LoadString;
            }
            LoadString = ReadString("String", "GuildMasterNot", "");
            if (LoadString == "")
            {
                WriteString("String", "GuildMasterNot", Settings.GuildMasterNot);
            }
            else
            {
                Settings.GuildMasterNot = LoadString;
            }
            LoadString = ReadString("String", "SabukHumanNot", "");
            if (LoadString == "")
            {
                WriteString("String", "SabukHumanNot", Settings.SabukHumanNot);
            }
            else
            {
                Settings.SabukHumanNot = LoadString;
            }
            LoadString = ReadString("String", "SabukMasterManNot", "");
            if (LoadString == "")
            {
                WriteString("String", "SabukMasterManNot", Settings.SabukMasterManNot);
            }
            else
            {
                Settings.SabukMasterManNot = LoadString;
            }
            LoadString = ReadString("String", "MemberNot", "");
            if (LoadString == "")
            {
                WriteString("String", "MemberNot", Settings.MemberNot);
            }
            else
            {
                Settings.MemberNot = LoadString;
            }
            LoadString = ReadString("String", "MemberTypeNot", "");
            if (LoadString == "")
            {
                WriteString("String", "MemberTypeNot", Settings.MemberTypeNot);
            }
            else
            {
                Settings.MemberTypeNot = LoadString;
            }
            LoadString = ReadString("String", "CanottWearIt", "");
            if (LoadString == "")
            {
                WriteString("String", "CanottWearIt", Settings.CanottWearIt);
            }
            else
            {
                Settings.CanottWearIt = LoadString;
            }
            LoadString = ReadString("String", "CanotUseDrugOnThisMap", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotUseDrugOnThisMap", Settings.CanotUseDrugOnThisMap);
            }
            else
            {
                Settings.CanotUseDrugOnThisMap = LoadString;
            }
            LoadString = ReadString("String", "GameMasterMode", "");
            if (LoadString == "")
            {
                WriteString("String", "GameMasterMode", Settings.GameMasterMode);
            }
            else
            {
                Settings.GameMasterMode = LoadString;
            }
            LoadString = ReadString("String", "ReleaseGameMasterMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ReleaseGameMasterMode", Settings.ReleaseGameMasterMode);
            }
            else
            {
                Settings.ReleaseGameMasterMode = LoadString;
            }
            LoadString = ReadString("String", "ObserverMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ObserverMode", Settings.ObserverMode);
            }
            else
            {
                Settings.ObserverMode = LoadString;
            }
            LoadString = ReadString("String", "ReleaseObserverMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ReleaseObserverMode", Settings.ReleaseObserverMode);
            }
            else
            {
                Settings.ReleaseObserverMode = LoadString;
            }
            LoadString = ReadString("String", "SupermanMode", "");
            if (LoadString == "")
            {
                WriteString("String", "SupermanMode", Settings.SupermanMode);
            }
            else
            {
                Settings.SupermanMode = LoadString;
            }
            LoadString = ReadString("String", "ReleaseSupermanMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ReleaseSupermanMode", Settings.ReleaseSupermanMode);
            }
            else
            {
                Settings.ReleaseSupermanMode = LoadString;
            }
            LoadString = ReadString("String", "YouFoundNothing", "");
            if (LoadString == "")
            {
                WriteString("String", "YouFoundNothing", Settings.YouFoundNothing);
            }
            else
            {
                Settings.YouFoundNothing = LoadString;
            }
            LoadString = ReadString("String", "LineNoticePreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "LineNoticePreFix", M2Share.Config.LineNoticePreFix);
            }
            else
            {
                M2Share.Config.LineNoticePreFix = LoadString;
            }
            LoadString = ReadString("String", "SysMsgPreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "SysMsgPreFix", M2Share.Config.SysMsgPreFix);
            }
            else
            {
                M2Share.Config.SysMsgPreFix = LoadString;
            }
            LoadString = ReadString("String", "GuildMsgPreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "GuildMsgPreFix", M2Share.Config.GuildMsgPreFix);
            }
            else
            {
                M2Share.Config.GuildMsgPreFix = LoadString;
            }
            LoadString = ReadString("String", "GroupMsgPreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "GroupMsgPreFix", M2Share.Config.GroupMsgPreFix);
            }
            else
            {
                M2Share.Config.GroupMsgPreFix = LoadString;
            }
            LoadString = ReadString("String", "HintMsgPreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "HintMsgPreFix", M2Share.Config.HintMsgPreFix);
            }
            else
            {
                M2Share.Config.HintMsgPreFix = LoadString;
            }
            LoadString = ReadString("String", "GMRedMsgpreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "GMRedMsgpreFix", M2Share.Config.GameManagerRedMsgPreFix);
            }
            else
            {
                M2Share.Config.GameManagerRedMsgPreFix = LoadString;
            }
            LoadString = ReadString("String", "MonSayMsgpreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "MonSayMsgpreFix", M2Share.Config.MonSayMsgPreFix);
            }
            else
            {
                M2Share.Config.MonSayMsgPreFix = LoadString;
            }
            LoadString = ReadString("String", "CustMsgpreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "CustMsgpreFix", M2Share.Config.CustMsgPreFix);
            }
            else
            {
                M2Share.Config.CustMsgPreFix = LoadString;
            }
            LoadString = ReadString("String", "CastleMsgpreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "CastleMsgpreFix", M2Share.Config.CastleMsgPreFix);
            }
            else
            {
                M2Share.Config.CastleMsgPreFix = LoadString;
            }
            LoadString = ReadString("String", "NoPasswordLockSystemMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "NoPasswordLockSystemMsg", Settings.NoPasswordLockSystemMsg);
            }
            else
            {
                Settings.NoPasswordLockSystemMsg = LoadString;
            }
            LoadString = ReadString("String", "AlreadySetPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "AlreadySetPassword", Settings.AlreadySetPasswordMsg);
            }
            else
            {
                Settings.AlreadySetPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "ReSetPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "ReSetPassword", Settings.ReSetPasswordMsg);
            }
            else
            {
                Settings.ReSetPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "PasswordOverLong", "");
            if (LoadString == "")
            {
                WriteString("String", "PasswordOverLong", Settings.PasswordOverLongMsg);
            }
            else
            {
                Settings.PasswordOverLongMsg = LoadString;
            }
            LoadString = ReadString("String", "ReSetPasswordOK", "");
            if (LoadString == "")
            {
                WriteString("String", "ReSetPasswordOK", Settings.ReSetPasswordOKMsg);
            }
            else
            {
                Settings.ReSetPasswordOKMsg = LoadString;
            }
            LoadString = ReadString("String", "ReSetPasswordNotMatch", "");
            if (LoadString == "")
            {
                WriteString("String", "ReSetPasswordNotMatch", Settings.ReSetPasswordNotMatchMsg);
            }
            else
            {
                Settings.ReSetPasswordNotMatchMsg = LoadString;
            }
            LoadString = ReadString("String", "PleaseInputUnLockPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseInputUnLockPassword", Settings.PleaseInputUnLockPasswordMsg);
            }
            else
            {
                Settings.PleaseInputUnLockPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageUnLockOK", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageUnLockOK", Settings.StorageUnLockOKMsg);
            }
            else
            {
                Settings.StorageUnLockOKMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageAlreadyUnLock", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageAlreadyUnLock", Settings.StorageAlreadyUnLockMsg);
            }
            else
            {
                Settings.StorageAlreadyUnLockMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageNoPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageNoPassword", Settings.StorageNoPasswordMsg);
            }
            else
            {
                Settings.StorageNoPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "UnLockPasswordFail", "");
            if (LoadString == "")
            {
                WriteString("String", "UnLockPasswordFail", Settings.UnLockPasswordFailMsg);
            }
            else
            {
                Settings.UnLockPasswordFailMsg = LoadString;
            }
            LoadString = ReadString("String", "LockStorageSuccess", "");
            if (LoadString == "")
            {
                WriteString("String", "LockStorageSuccess", Settings.LockStorageSuccessMsg);
            }
            else
            {
                Settings.LockStorageSuccessMsg = LoadString;
            }
            LoadString = ReadString("String", "StoragePasswordClearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StoragePasswordClearMsg", Settings.StoragePasswordClearMsg);
            }
            else
            {
                Settings.StoragePasswordClearMsg = LoadString;
            }
            LoadString = ReadString("String", "PleaseUnloadStoragePasswordMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseUnloadStoragePasswordMsg", Settings.PleaseUnloadStoragePasswordMsg);
            }
            else
            {
                Settings.PleaseUnloadStoragePasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageAlreadyLock", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageAlreadyLock", Settings.StorageAlreadyLockMsg);
            }
            else
            {
                Settings.StorageAlreadyLockMsg = LoadString;
            }
            LoadString = ReadString("String", "StoragePasswordLocked", "");
            if (LoadString == "")
            {
                WriteString("String", "StoragePasswordLocked", Settings.StoragePasswordLockedMsg);
            }
            else
            {
                Settings.StoragePasswordLockedMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageSetPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageSetPassword", Settings.SetPasswordMsg);
            }
            else
            {
                Settings.SetPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "PleaseInputOldPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseInputOldPassword", Settings.PleaseInputOldPasswordMsg);
            }
            else
            {
                Settings.PleaseInputOldPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "PasswordIsClearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PasswordIsClearMsg", Settings.OldPasswordIsClearMsg);
            }
            else
            {
                Settings.OldPasswordIsClearMsg = LoadString;
            }
            LoadString = ReadString("String", "NoPasswordSet", "");
            if (LoadString == "")
            {
                WriteString("String", "NoPasswordSet", Settings.NoPasswordSetMsg);
            }
            else
            {
                Settings.NoPasswordSetMsg = LoadString;
            }
            LoadString = ReadString("String", "OldPasswordIncorrect", "");
            if (LoadString == "")
            {
                WriteString("String", "OldPasswordIncorrect", Settings.OldPasswordIncorrectMsg);
            }
            else
            {
                Settings.OldPasswordIncorrectMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageIsLocked", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageIsLocked", Settings.StorageIsLockedMsg);
            }
            else
            {
                Settings.StorageIsLockedMsg = LoadString;
            }
            LoadString = ReadString("String", "PleaseTryDealLaterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseTryDealLaterMsg", Settings.PleaseTryDealLaterMsg);
            }
            else
            {
                Settings.PleaseTryDealLaterMsg = LoadString;
            }
            LoadString = ReadString("String", "DealItemsDenyGetBackMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DealItemsDenyGetBackMsg", Settings.DealItemsDenyGetBackMsg);
            }
            else
            {
                Settings.DealItemsDenyGetBackMsg = LoadString;
            }
            LoadString = ReadString("String", "DisableDealItemsMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableDealItemsMsg", Settings.DisableDealItemsMsg);
            }
            else
            {
                Settings.DisableDealItemsMsg = LoadString;
            }
            LoadString = ReadString("String", "CanotTryDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotTryDealMsg", Settings.CanotTryDealMsg);
            }
            else
            {
                Settings.CanotTryDealMsg = LoadString;
            }
            LoadString = ReadString("String", "DealActionCancelMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DealActionCancelMsg", Settings.DealActionCancelMsg);
            }
            else
            {
                Settings.DealActionCancelMsg = LoadString;
            }
            LoadString = ReadString("String", "PoseDisableDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PoseDisableDealMsg", Settings.PoseDisableDealMsg);
            }
            else
            {
                Settings.PoseDisableDealMsg = LoadString;
            }
            LoadString = ReadString("String", "DealSuccessMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DealSuccessMsg", Settings.DealSuccessMsg);
            }
            else
            {
                Settings.DealSuccessMsg = LoadString;
            }
            LoadString = ReadString("String", "DealOKTooFast", "");
            if (LoadString == "")
            {
                WriteString("String", "DealOKTooFast", Settings.DealOKTooFast);
            }
            else
            {
                Settings.DealOKTooFast = LoadString;
            }
            LoadString = ReadString("String", "YourBagSizeTooSmall", "");
            if (LoadString == "")
            {
                WriteString("String", "YourBagSizeTooSmall", Settings.YourBagSizeTooSmall);
            }
            else
            {
                Settings.YourBagSizeTooSmall = LoadString;
            }
            LoadString = ReadString("String", "DealHumanBagSizeTooSmall", "");
            if (LoadString == "")
            {
                WriteString("String", "DealHumanBagSizeTooSmall", Settings.DealHumanBagSizeTooSmall);
            }
            else
            {
                Settings.DealHumanBagSizeTooSmall = LoadString;
            }
            LoadString = ReadString("String", "YourGoldLargeThenLimit", "");
            if (LoadString == "")
            {
                WriteString("String", "YourGoldLargeThenLimit", Settings.YourGoldLargeThenLimit);
            }
            else
            {
                Settings.YourGoldLargeThenLimit = LoadString;
            }
            LoadString = ReadString("String", "DealHumanGoldLargeThenLimit", "");
            if (LoadString == "")
            {
                WriteString("String", "DealHumanGoldLargeThenLimit", Settings.DealHumanGoldLargeThenLimit);
            }
            else
            {
                Settings.DealHumanGoldLargeThenLimit = LoadString;
            }
            LoadString = ReadString("String", "YouDealOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouDealOKMsg", Settings.YouDealOKMsg);
            }
            else
            {
                Settings.YouDealOKMsg = LoadString;
            }
            LoadString = ReadString("String", "PoseDealOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PoseDealOKMsg", Settings.PoseDealOKMsg);
            }
            else
            {
                Settings.PoseDealOKMsg = LoadString;
            }
            LoadString = ReadString("String", "KickClientUserMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "KickClientUserMsg", Settings.KickClientUserMsg);
            }
            else
            {
                Settings.KickClientUserMsg = LoadString;
            }
            LoadString = ReadString("String", "ActionIsLockedMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ActionIsLockedMsg", Settings.ActionIsLockedMsg);
            }
            else
            {
                Settings.ActionIsLockedMsg = LoadString;
            }
            LoadString = ReadString("String", "PasswordNotSetMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PasswordNotSetMsg", Settings.PasswordNotSetMsg);
            }
            else
            {
                Settings.PasswordNotSetMsg = LoadString;
            }
            LoadString = ReadString("String", "NotPasswordProtectMode", "");
            if (LoadString == "")
            {
                WriteString("String", "NotPasswordProtectMode", Settings.NotPasswordProtectMode);
            }
            else
            {
                Settings.NotPasswordProtectMode = LoadString;
            }
            LoadString = ReadString("String", "CanotDropGoldMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropGoldMsg", Settings.CanotDropGoldMsg);
            }
            else
            {
                Settings.CanotDropGoldMsg = LoadString;
            }
            LoadString = ReadString("String", "CanotDropInSafeZoneMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropInSafeZoneMsg", Settings.CanotDropInSafeZoneMsg);
            }
            else
            {
                Settings.CanotDropInSafeZoneMsg = LoadString;
            }
            LoadString = ReadString("String", "CanotDropItemMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropItemMsg", Settings.CanotDropItemMsg);
            }
            else
            {
                Settings.CanotDropItemMsg = LoadString;
            }
            LoadString = ReadString("String", "CanotDropItemMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropItemMsg", Settings.CanotDropItemMsg);
            }
            else
            {
                Settings.CanotDropItemMsg = LoadString;
            }
            LoadString = ReadString("String", "CanotUseItemMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotUseItemMsg", Settings.CanotUseItemMsg);
            }
            else
            {
                Settings.CanotUseItemMsg = LoadString;
            }
            LoadString = ReadString("String", "StartMarryManMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryManMsg", Settings.StartMarryManMsg);
            }
            else
            {
                Settings.StartMarryManMsg = LoadString;
            }
            LoadString = ReadString("String", "StartMarryWoManMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryWoManMsg", Settings.StartMarryWoManMsg);
            }
            else
            {
                Settings.StartMarryWoManMsg = LoadString;
            }
            LoadString = ReadString("String", "StartMarryManAskQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryManAskQuestionMsg", Settings.StartMarryManAskQuestionMsg);
            }
            else
            {
                Settings.StartMarryManAskQuestionMsg = LoadString;
            }
            LoadString = ReadString("String", "StartMarryWoManAskQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryWoManAskQuestionMsg", Settings.StartMarryWoManAskQuestionMsg);
            }
            else
            {
                Settings.StartMarryWoManAskQuestionMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryManAnswerQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryManAnswerQuestionMsg", Settings.MarryManAnswerQuestionMsg);
            }
            else
            {
                Settings.MarryManAnswerQuestionMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryManAskQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryManAskQuestionMsg", Settings.MarryManAskQuestionMsg);
            }
            else
            {
                Settings.MarryManAskQuestionMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryWoManAnswerQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManAnswerQuestionMsg", Settings.MarryWoManAnswerQuestionMsg);
            }
            else
            {
                Settings.MarryWoManAnswerQuestionMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryWoManGetMarryMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManGetMarryMsg", Settings.MarryWoManGetMarryMsg);
            }
            else
            {
                Settings.MarryWoManGetMarryMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryWoManDenyMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManDenyMsg", Settings.MarryWoManDenyMsg);
            }
            else
            {
                Settings.MarryWoManDenyMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryWoManCancelMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManCancelMsg", Settings.MarryWoManCancelMsg);
            }
            else
            {
                Settings.MarryWoManCancelMsg = LoadString;
            }
            LoadString = ReadString("String", "ForceUnMarryManLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ForceUnMarryManLoginMsg", Settings.fUnMarryManLoginMsg);
            }
            else
            {
                Settings.fUnMarryManLoginMsg = LoadString;
            }
            LoadString = ReadString("String", "ForceUnMarryWoManLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ForceUnMarryWoManLoginMsg", Settings.fUnMarryWoManLoginMsg);
            }
            else
            {
                Settings.fUnMarryWoManLoginMsg = LoadString;
            }
            LoadString = ReadString("String", "ManLoginDearOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLoginDearOnlineSelfMsg", Settings.ManLoginDearOnlineSelfMsg);
            }
            else
            {
                Settings.ManLoginDearOnlineSelfMsg = LoadString;
            }
            LoadString = ReadString("String", "ManLoginDearOnlineDearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLoginDearOnlineDearMsg", Settings.ManLoginDearOnlineDearMsg);
            }
            else
            {
                Settings.ManLoginDearOnlineDearMsg = LoadString;
            }
            LoadString = ReadString("String", "WoManLoginDearOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLoginDearOnlineSelfMsg", Settings.WoManLoginDearOnlineSelfMsg);
            }
            else
            {
                Settings.WoManLoginDearOnlineSelfMsg = LoadString;
            }
            LoadString = ReadString("String", "WoManLoginDearOnlineDearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLoginDearOnlineDearMsg", Settings.WoManLoginDearOnlineDearMsg);
            }
            else
            {
                Settings.WoManLoginDearOnlineDearMsg = LoadString;
            }
            LoadString = ReadString("String", "ManLoginDearNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLoginDearNotOnlineMsg", Settings.ManLoginDearNotOnlineMsg);
            }
            else
            {
                Settings.ManLoginDearNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "WoManLoginDearNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLoginDearNotOnlineMsg", Settings.WoManLoginDearNotOnlineMsg);
            }
            else
            {
                Settings.WoManLoginDearNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "ManLongOutDearOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLongOutDearOnlineMsg", Settings.ManLongOutDearOnlineMsg);
            }
            else
            {
                Settings.ManLongOutDearOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "WoManLongOutDearOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLongOutDearOnlineMsg", Settings.WoManLongOutDearOnlineMsg);
            }
            else
            {
                Settings.WoManLongOutDearOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YouAreNotMarryedMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouAreNotMarryedMsg", Settings.YouAreNotMarryedMsg);
            }
            else
            {
                Settings.YouAreNotMarryedMsg = LoadString;
            }
            LoadString = ReadString("String", "YourWifeNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourWifeNotOnlineMsg", Settings.YourWifeNotOnlineMsg);
            }
            else
            {
                Settings.YourWifeNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YourHusbandNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourHusbandNotOnlineMsg", Settings.YourHusbandNotOnlineMsg);
            }
            else
            {
                Settings.YourHusbandNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YourWifeNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourWifeNowLocateMsg", Settings.YourWifeNowLocateMsg);
            }
            else
            {
                Settings.YourWifeNowLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourHusbandSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourHusbandSearchLocateMsg", Settings.YourHusbandSearchLocateMsg);
            }
            else
            {
                Settings.YourHusbandSearchLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourHusbandNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourHusbandNowLocateMsg", Settings.YourHusbandNowLocateMsg);
            }
            else
            {
                Settings.YourHusbandNowLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourWifeSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourWifeSearchLocateMsg", Settings.YourWifeSearchLocateMsg);
            }
            else
            {
                Settings.YourWifeSearchLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "FUnMasterLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "FUnMasterLoginMsg", Settings.fUnMasterLoginMsg);
            }
            else
            {
                Settings.fUnMasterLoginMsg = LoadString;
            }
            LoadString = ReadString("String", "UnMasterListLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "UnMasterListLoginMsg", Settings.fUnMasterListLoginMsg);
            }
            else
            {
                Settings.fUnMasterListLoginMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterListOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListOnlineSelfMsg", Settings.MasterListOnlineSelfMsg);
            }
            else
            {
                Settings.MasterListOnlineSelfMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterListOnlineMasterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListOnlineMasterMsg", Settings.MasterListOnlineMasterMsg);
            }
            else
            {
                Settings.MasterListOnlineMasterMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterOnlineSelfMsg", Settings.MasterOnlineSelfMsg);
            }
            else
            {
                Settings.MasterOnlineSelfMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterOnlineMasterListMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterOnlineMasterListMsg", Settings.MasterOnlineMasterListMsg);
            }
            else
            {
                Settings.MasterOnlineMasterListMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterLongOutMasterListOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterLongOutMasterListOnlineMsg", Settings.MasterLongOutMasterListOnlineMsg);
            }
            else
            {
                Settings.MasterLongOutMasterListOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterListLongOutMasterOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListLongOutMasterOnlineMsg", Settings.MasterListLongOutMasterOnlineMsg);
            }
            else
            {
                Settings.MasterListLongOutMasterOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterListNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListNotOnlineMsg", Settings.MasterListNotOnlineMsg);
            }
            else
            {
                Settings.MasterListNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterNotOnlineMsg", Settings.MasterNotOnlineMsg);
            }
            else
            {
                Settings.MasterNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YouAreNotMasterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouAreNotMasterMsg", Settings.YouAreNotMasterMsg);
            }
            else
            {
                Settings.YouAreNotMasterMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterNotOnlineMsg", Settings.YourMasterNotOnlineMsg);
            }
            else
            {
                Settings.YourMasterNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterListNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListNotOnlineMsg", Settings.YourMasterListNotOnlineMsg);
            }
            else
            {
                Settings.YourMasterListNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterNowLocateMsg", Settings.YourMasterNowLocateMsg);
            }
            else
            {
                Settings.YourMasterNowLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterListSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListSearchLocateMsg", Settings.YourMasterListSearchLocateMsg);
            }
            else
            {
                Settings.YourMasterListSearchLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterListNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListNowLocateMsg", Settings.YourMasterListNowLocateMsg);
            }
            else
            {
                Settings.YourMasterListNowLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterSearchLocateMsg", Settings.YourMasterSearchLocateMsg);
            }
            else
            {
                Settings.YourMasterSearchLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterListUnMasterOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListUnMasterOKMsg", Settings.YourMasterListUnMasterOKMsg);
            }
            else
            {
                Settings.YourMasterListUnMasterOKMsg = LoadString;
            }
            LoadString = ReadString("String", "YouAreUnMasterOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouAreUnMasterOKMsg", Settings.YouAreUnMasterOKMsg);
            }
            else
            {
                Settings.YouAreUnMasterOKMsg = LoadString;
            }
            LoadString = ReadString("String", "UnMasterLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "UnMasterLoginMsg", Settings.UnMasterLoginMsg);
            }
            else
            {
                Settings.UnMasterLoginMsg = LoadString;
            }
            LoadString = ReadString("String", "NPCSayUnMasterOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "NPCSayUnMasterOKMsg", Settings.NPCSayUnMasterOKMsg);
            }
            else
            {
                Settings.NPCSayUnMasterOKMsg = LoadString;
            }
            LoadString = ReadString("String", "NPCSayForceUnMasterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "NPCSayForceUnMasterMsg", Settings.NPCSayForceUnMasterMsg);
            }
            else
            {
                Settings.NPCSayForceUnMasterMsg = LoadString;
            }
            LoadString = ReadString("String", "MyInfo", "");
            if (LoadString == "")
            {
                WriteString("String", "MyInfo", Settings.MyInfo);
            }
            else
            {
                Settings.MyInfo = LoadString;
            }
            LoadString = ReadString("String", "OpenedDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "OpenedDealMsg", Settings.OpenedDealMsg);
            }
            else
            {
                Settings.OpenedDealMsg = LoadString;
            }
            LoadString = ReadString("String", "SendCustMsgCanNotUseNowMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "SendCustMsgCanNotUseNowMsg", Settings.SendCustMsgCanNotUseNowMsg);
            }
            else
            {
                Settings.SendCustMsgCanNotUseNowMsg = LoadString;
            }
            LoadString = ReadString("String", "SubkMasterMsgCanNotUseNowMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "SubkMasterMsgCanNotUseNowMsg", Settings.SubkMasterMsgCanNotUseNowMsg);
            }
            else
            {
                Settings.SubkMasterMsgCanNotUseNowMsg = LoadString;
            }
            LoadString = ReadString("String", "SendOnlineCountMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "SendOnlineCountMsg", Settings.SendOnlineCountMsg);
            }
            else
            {
                Settings.SendOnlineCountMsg = LoadString;
            }
            LoadString = ReadString("String", "WeaponRepairSuccess", "");
            if (LoadString == "")
            {
                WriteString("String", "WeaponRepairSuccess", Settings.WeaponRepairSuccess);
            }
            else
            {
                Settings.WeaponRepairSuccess = LoadString;
            }
            LoadString = ReadString("String", "DefenceUpTime", "");
            if (LoadString == "")
            {
                WriteString("String", "DefenceUpTime", Settings.DefenceUpTime);
            }
            else
            {
                Settings.DefenceUpTime = LoadString;
            }
            LoadString = ReadString("String", "MagDefenceUpTime", "");
            if (LoadString == "")
            {
                WriteString("String", "MagDefenceUpTime", Settings.MagDefenceUpTime);
            }
            else
            {
                Settings.MagDefenceUpTime = LoadString;
            }
            LoadString = ReadString("String", "WinLottery1Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery1Msg", Settings.WinLottery1Msg);
            }
            else
            {
                Settings.WinLottery1Msg = LoadString;
            }
            LoadString = ReadString("String", "WinLottery2Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery2Msg", Settings.WinLottery2Msg);
            }
            else
            {
                Settings.WinLottery2Msg = LoadString;
            }
            LoadString = ReadString("String", "WinLottery3Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery3Msg", Settings.WinLottery3Msg);
            }
            else
            {
                Settings.WinLottery3Msg = LoadString;
            }
            LoadString = ReadString("String", "WinLottery4Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery4Msg", Settings.WinLottery4Msg);
            }
            else
            {
                Settings.WinLottery4Msg = LoadString;
            }
            LoadString = ReadString("String", "WinLottery5Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery5Msg", Settings.WinLottery5Msg);
            }
            else
            {
                Settings.WinLottery5Msg = LoadString;
            }
            LoadString = ReadString("String", "WinLottery6Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery6Msg", Settings.WinLottery6Msg);
            }
            else
            {
                Settings.WinLottery6Msg = LoadString;
            }
            LoadString = ReadString("String", "NotWinLotteryMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "NotWinLotteryMsg", Settings.NotWinLotteryMsg);
            }
            else
            {
                Settings.NotWinLotteryMsg = LoadString;
            }
            LoadString = ReadString("String", "WeaptonMakeLuck", "");
            if (LoadString == "")
            {
                WriteString("String", "WeaptonMakeLuck", Settings.WeaptonMakeLuck);
            }
            else
            {
                Settings.WeaptonMakeLuck = LoadString;
            }
            LoadString = ReadString("String", "WeaptonNotMakeLuck", "");
            if (LoadString == "")
            {
                WriteString("String", "WeaptonNotMakeLuck", Settings.WeaptonNotMakeLuck);
            }
            else
            {
                Settings.WeaptonNotMakeLuck = LoadString;
            }
            LoadString = ReadString("String", "TheWeaponIsCursed", "");
            if (LoadString == "")
            {
                WriteString("String", "TheWeaponIsCursed", Settings.TheWeaponIsCursed);
            }
            else
            {
                Settings.TheWeaponIsCursed = LoadString;
            }
            LoadString = ReadString("String", "CanotTakeOffItem", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotTakeOffItem", Settings.CanotTakeOffItem);
            }
            else
            {
                Settings.CanotTakeOffItem = LoadString;
            }
            LoadString = ReadString("String", "JoinGroupMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "JoinGroupMsg", Settings.JoinGroup);
            }
            else
            {
                Settings.JoinGroup = LoadString;
            }
            LoadString = ReadString("String", "TryModeCanotUseStorage", "");
            if (LoadString == "")
            {
                WriteString("String", "TryModeCanotUseStorage", Settings.TryModeCanotUseStorage);
            }
            else
            {
                Settings.TryModeCanotUseStorage = LoadString;
            }
            LoadString = ReadString("String", "CanotGetItemsMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotGetItemsMsg", Settings.CanotGetItems);
            }
            else
            {
                Settings.CanotGetItems = LoadString;
            }
            LoadString = ReadString("String", "EnableDearRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableDearRecall", CommandHelp.EnableDearRecall);
            }
            else
            {
                CommandHelp.EnableDearRecall = LoadString;
            }
            LoadString = ReadString("String", "DisableDearRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableDearRecall", CommandHelp.DisableDearRecall);
            }
            else
            {
                CommandHelp.DisableDearRecall = LoadString;
            }
            LoadString = ReadString("String", "EnableMasterRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableMasterRecall", CommandHelp.EnableMasterRecall);
            }
            else
            {
                CommandHelp.EnableMasterRecall = LoadString;
            }
            LoadString = ReadString("String", "DisableMasterRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableMasterRecall", CommandHelp.DisableMasterRecall);
            }
            else
            {
                CommandHelp.DisableMasterRecall = LoadString;
            }
            LoadString = ReadString("String", "NowCurrDateTime", "");
            if (LoadString == "")
            {
                WriteString("String", "NowCurrDateTime", CommandHelp.NowCurrDateTime);
            }
            else
            {
                CommandHelp.NowCurrDateTime = LoadString;
            }
            LoadString = ReadString("String", "EnableHearWhisper", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableHearWhisper", CommandHelp.EnableHearWhisper);
            }
            else
            {
                CommandHelp.EnableHearWhisper = LoadString;
            }
            LoadString = ReadString("String", "DisableHearWhisper", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableHearWhisper", CommandHelp.DisableHearWhisper);
            }
            else
            {
                CommandHelp.DisableHearWhisper = LoadString;
            }
            LoadString = ReadString("String", "EnableShoutMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableShoutMsg", CommandHelp.EnableShoutMsg);
            }
            else
            {
                CommandHelp.EnableShoutMsg = LoadString;
            }
            LoadString = ReadString("String", "DisableShoutMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableShoutMsg", CommandHelp.DisableShoutMsg);
            }
            else
            {
                CommandHelp.DisableShoutMsg = LoadString;
            }
            LoadString = ReadString("String", "EnableDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableDealMsg", CommandHelp.EnableDealMsg);
            }
            else
            {
                CommandHelp.EnableDealMsg = LoadString;
            }
            LoadString = ReadString("String", "DisableDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableDealMsg", CommandHelp.DisableDealMsg);
            }
            else
            {
                CommandHelp.DisableDealMsg = LoadString;
            }
            LoadString = ReadString("String", "EnableGuildChat", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableGuildChat", CommandHelp.EnableGuildChat);
            }
            else
            {
                CommandHelp.EnableGuildChat = LoadString;
            }
            LoadString = ReadString("String", "DisableGuildChat", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableGuildChat", CommandHelp.DisableGuildChat);
            }
            else
            {
                CommandHelp.DisableGuildChat = LoadString;
            }
            LoadString = ReadString("String", "EnableJoinGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableJoinGuild", CommandHelp.EnableJoinGuild);
            }
            else
            {
                CommandHelp.EnableJoinGuild = LoadString;
            }
            LoadString = ReadString("String", "DisableJoinGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableJoinGuild", CommandHelp.DisableJoinGuild);
            }
            else
            {
                CommandHelp.DisableJoinGuild = LoadString;
            }
            LoadString = ReadString("String", "EnableAuthAllyGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableAuthAllyGuild", CommandHelp.EnableAuthAllyGuild);
            }
            else
            {
                CommandHelp.EnableAuthAllyGuild = LoadString;
            }
            LoadString = ReadString("String", "DisableAuthAllyGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableAuthAllyGuild", CommandHelp.DisableAuthAllyGuild);
            }
            else
            {
                CommandHelp.DisableAuthAllyGuild = LoadString;
            }
            LoadString = ReadString("String", "EnableGroupRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableGroupRecall", CommandHelp.EnableGroupRecall);
            }
            else
            {
                CommandHelp.EnableGroupRecall = LoadString;
            }
            LoadString = ReadString("String", "DisableGroupRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableGroupRecall", CommandHelp.DisableGroupRecall);
            }
            else
            {
                CommandHelp.DisableGroupRecall = LoadString;
            }
            LoadString = ReadString("String", "EnableGuildRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableGuildRecall", CommandHelp.EnableGuildRecall);
            }
            else
            {
                CommandHelp.EnableGuildRecall = LoadString;
            }
            LoadString = ReadString("String", "DisableGuildRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableGuildRecall", CommandHelp.DisableGuildRecall);
            }
            else
            {
                CommandHelp.DisableGuildRecall = LoadString;
            }
            LoadString = ReadString("String", "PleaseInputPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseInputPassword", CommandHelp.PleaseInputPassword);
            }
            else
            {
                CommandHelp.PleaseInputPassword = LoadString;
            }
            LoadString = ReadString("String", "TheMapDisableMove", "");
            if (LoadString == "")
            {
                WriteString("String", "TheMapDisableMove", CommandHelp.TheMapDisableMove);
            }
            else
            {
                CommandHelp.TheMapDisableMove = LoadString;
            }
            LoadString = ReadString("String", "TheMapNotFound", "");
            if (LoadString == "")
            {
                WriteString("String", "TheMapNotFound", CommandHelp.TheMapNotFound);
            }
            else
            {
                CommandHelp.TheMapNotFound = LoadString;
            }
            LoadString = ReadString("String", "YourIPaddrDenyLogon", "");
            if (LoadString == "")
            {
                WriteString("String", "YourIPaddrDenyLogon", Settings.YourIPaddrDenyLogon);
            }
            else
            {
                Settings.YourIPaddrDenyLogon = LoadString;
            }
            LoadString = ReadString("String", "YourAccountDenyLogon", "");
            if (LoadString == "")
            {
                WriteString("String", "YourAccountDenyLogon", Settings.YourAccountDenyLogon);
            }
            else
            {
                Settings.YourAccountDenyLogon = LoadString;
            }
            LoadString = ReadString("String", "YourChrNameDenyLogon", "");
            if (LoadString == "")
            {
                WriteString("String", "YourChrNameDenyLogon", Settings.YourChrNameDenyLogon);
            }
            else
            {
                Settings.YourChrNameDenyLogon = LoadString;
            }
            LoadString = ReadString("String", "CanotPickUpItem", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotPickUpItem", Settings.CanotPickUpItem);
            }
            else
            {
                Settings.CanotPickUpItem = LoadString;
            }
            LoadString = ReadString("String", "sQUERYBAGITEMS", "");
            if (LoadString == "")
            {
                WriteString("String", "sQUERYBAGITEMS", Settings.QUERYBAGITEMS);
            }
            else
            {
                Settings.QUERYBAGITEMS = LoadString;
            }
            LoadString = ReadString("String", "CanotSendmsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotSendmsg", Settings.CanotSendmsg);
            }
            else
            {
                Settings.CanotSendmsg = LoadString;
            }
            LoadString = ReadString("String", "UserDenyWhisperMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "UserDenyWhisperMsg", Settings.UserDenyWhisperMsg);
            }
            else
            {
                Settings.UserDenyWhisperMsg = LoadString;
            }
            LoadString = ReadString("String", "UserNotOnLine", "");
            if (LoadString == "")
            {
                WriteString("String", "UserNotOnLine", Settings.UserNotOnLine);
            }
            else
            {
                Settings.UserNotOnLine = LoadString;
            }
            LoadString = ReadString("String", "RevivalRecoverMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "RevivalRecoverMsg", Settings.RevivalRecoverMsg);
            }
            else
            {
                Settings.RevivalRecoverMsg = LoadString;
            }
            LoadString = ReadString("String", "ClientVersionTooOld", "");
            if (LoadString == "")
            {
                WriteString("String", "ClientVersionTooOld", Settings.ClientVersionTooOld);
            }
            else
            {
                Settings.ClientVersionTooOld = LoadString;
            }
            LoadString = ReadString("String", "CastleGuildName", "");
            if (LoadString == "")
            {
                WriteString("String", "CastleGuildName", Settings.CastleGuildName);
            }
            else
            {
                Settings.CastleGuildName = LoadString;
            }
            LoadString = ReadString("String", "NoCastleGuildName", "");
            if (LoadString == "")
            {
                WriteString("String", "NoCastleGuildName", Settings.NoCastleGuildName);
            }
            else
            {
                Settings.NoCastleGuildName = LoadString;
            }
            LoadString = ReadString("String", "WarrReNewName", "");
            if (LoadString == "")
            {
                WriteString("String", "WarrReNewName", Settings.WarrReNewName);
            }
            else
            {
                Settings.WarrReNewName = LoadString;
            }
            LoadString = ReadString("String", "WizardReNewName", "");
            if (LoadString == "")
            {
                WriteString("String", "WizardReNewName", Settings.WizardReNewName);
            }
            else
            {
                Settings.WizardReNewName = LoadString;
            }
            LoadString = ReadString("String", "TaosReNewName", "");
            if (LoadString == "")
            {
                WriteString("String", "TaosReNewName", Settings.TaosReNewName);
            }
            else
            {
                Settings.TaosReNewName = LoadString;
            }
            LoadString = ReadString("String", "RankLevelName", "");
            if (LoadString == "")
            {
                WriteString("String", "RankLevelName", Settings.RankLevelName);
            }
            else
            {
                Settings.RankLevelName = LoadString.Replace("%s", "{0}");
            }
            LoadString = ReadString("String", "ManDearName", "");
            if (LoadString == "")
            {
                WriteString("String", "ManDearName", Settings.ManDearName);
            }
            else
            {
                Settings.ManDearName = LoadString;
            }
            LoadString = ReadString("String", "WoManDearName", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManDearName", Settings.WoManDearName);
            }
            else
            {
                Settings.WoManDearName = LoadString;
            }
            LoadString = ReadString("String", "MasterName", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterName", Settings.MasterName);
            }
            else
            {
                Settings.MasterName = LoadString;
            }

            LoadString = ReadString("String", "NoMasterName", "");
            if (LoadString == "")
            {
                WriteString("String", "NoMasterName", Settings.NoMasterName);
            }
            else
            {
                Settings.NoMasterName = LoadString;
            }
            LoadString = ReadString("String", "HumanShowName", "");
            if (LoadString == "")
            {
                WriteString("String", "HumanShowName", Settings.HumanShowName);
            }
            else
            {
                Settings.HumanShowName = LoadString;
            }
            LoadString = ReadString("String", "ChangePermissionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangePermissionMsg", Settings.ChangePermissionMsg);
            }
            else
            {
                Settings.ChangePermissionMsg = LoadString;
            }
            LoadString = ReadString("String", "ChangeKillMonExpRateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangeKillMonExpRateMsg", Settings.ChangeKillMonExpRateMsg);
            }
            else
            {
                Settings.ChangeKillMonExpRateMsg = LoadString;
            }
            LoadString = ReadString("String", "ChangePowerRateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangePowerRateMsg", Settings.ChangePowerRateMsg);
            }
            else
            {
                Settings.ChangePowerRateMsg = LoadString;
            }
            LoadString = ReadString("String", "ChangeMemberLevelMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangeMemberLevelMsg", Settings.ChangeMemberLevelMsg);
            }
            else
            {
                Settings.ChangeMemberLevelMsg = LoadString;
            }
            LoadString = ReadString("String", "ChangeMemberTypeMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangeMemberTypeMsg", Settings.ChangeMemberTypeMsg);
            }
            else
            {
                Settings.ChangeMemberTypeMsg = LoadString;
            }
            LoadString = ReadString("String", "ScriptChangeHumanHPMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ScriptChangeHumanHPMsg", Settings.ScriptChangeHumanHPMsg);
            }
            else
            {
                Settings.ScriptChangeHumanHPMsg = LoadString;
            }
            LoadString = ReadString("String", "ScriptChangeHumanMPMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ScriptChangeHumanMPMsg", Settings.ScriptChangeHumanMPMsg);
            }
            else
            {
                Settings.ScriptChangeHumanMPMsg = LoadString;
            }
            LoadString = ReadString("String", "YouCanotDisableSayMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouCanotDisableSayMsg", Settings.DisableSayMsg);
            }
            else
            {
                Settings.DisableSayMsg = LoadString;
            }
            LoadString = ReadString("String", "OnlineCountMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "OnlineCountMsg", Settings.OnlineCountMsg);
            }
            else
            {
                Settings.OnlineCountMsg = LoadString;
            }
            LoadString = ReadString("String", "TotalOnlineCountMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "TotalOnlineCountMsg", Settings.TotalOnlineCountMsg);
            }
            else
            {
                Settings.TotalOnlineCountMsg = LoadString;
            }
            LoadString = ReadString("String", "YouNeedLevelSendMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouNeedLevelSendMsg", Settings.YouNeedLevelMsg);
            }
            else
            {
                Settings.YouNeedLevelMsg = LoadString;
            }
            LoadString = ReadString("String", "ThisMapDisableSendCyCyMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ThisMapDisableSendCyCyMsg", Settings.ThisMapDisableSendCyCyMsg);
            }
            else
            {
                Settings.ThisMapDisableSendCyCyMsg = LoadString;
            }
            LoadString = ReadString("String", "YouCanSendCyCyLaterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouCanSendCyCyLaterMsg", Settings.YouCanSendCyCyLaterMsg);
            }
            else
            {
                Settings.YouCanSendCyCyLaterMsg = LoadString;
            }
            LoadString = ReadString("String", "YouIsDisableSendMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouIsDisableSendMsg", Settings.YouIsDisableSendMsg);
            }
            else
            {
                Settings.YouIsDisableSendMsg = LoadString;
            }
            LoadString = ReadString("String", "YouMurderedMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouMurderedMsg", Settings.YouMurderedMsg);
            }
            else
            {
                Settings.YouMurderedMsg = LoadString;
            }
            LoadString = ReadString("String", "YouKilledByMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouKilledByMsg", Settings.YouKilledByMsg);
            }
            else
            {
                Settings.YouKilledByMsg = LoadString;
            }
            LoadString = ReadString("String", "YouprotectedByLawOfDefense", "");
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
            sString = ReadString(sSection, sIdent, "");
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