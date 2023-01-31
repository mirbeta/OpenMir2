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
                WriteString("String", "ClientSoftVersionError", Settings.sClientSoftVersionError);
            }
            else
            {
                Settings.sClientSoftVersionError = LoadString;
            }
            LoadString = ReadString("String", "DownLoadNewClientSoft", "");
            if (LoadString == "")
            {
                WriteString("String", "DownLoadNewClientSoft", Settings.sDownLoadNewClientSoft);
            }
            else
            {
                Settings.sDownLoadNewClientSoft = LoadString;
            }
            LoadString = ReadString("String", "ForceDisConnect", "");
            if (LoadString == "")
            {
                WriteString("String", "ForceDisConnect", Settings.sForceDisConnect);
            }
            else
            {
                Settings.sForceDisConnect = LoadString;
            }
            LoadString = ReadString("String", "ClientSoftVersionTooOld", "");
            if (LoadString == "")
            {
                WriteString("String", "ClientSoftVersionTooOld", Settings.sClientSoftVersionTooOld);
            }
            else
            {
                Settings.sClientSoftVersionTooOld = LoadString;
            }
            LoadString = ReadString("String", "DownLoadAndUseNewClient", "");
            if (LoadString == "")
            {
                WriteString("String", "DownLoadAndUseNewClient", Settings.sDownLoadAndUseNewClient);
            }
            else
            {
                Settings.sDownLoadAndUseNewClient = LoadString;
            }
            LoadString = ReadString("String", "OnlineUserFull", "");
            if (LoadString == "")
            {
                WriteString("String", "OnlineUserFull", Settings.sOnlineUserFull);
            }
            else
            {
                Settings.sOnlineUserFull = LoadString;
            }
            LoadString = ReadString("String", "YouNowIsTryPlayMode", "");
            if (LoadString == "")
            {
                WriteString("String", "YouNowIsTryPlayMode", Settings.sYouNowIsTryPlayMode);
            }
            else
            {
                Settings.sYouNowIsTryPlayMode = LoadString;
            }
            LoadString = ReadString("String", "NowIsFreePlayMode", "");
            if (LoadString == "")
            {
                WriteString("String", "NowIsFreePlayMode", Settings.g_sNowIsFreePlayMode);
            }
            else
            {
                Settings.g_sNowIsFreePlayMode = LoadString;
            }
            LoadString = ReadString("String", "AttackModeOfAll", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfAll", Settings.sAttackModeOfAll);
            }
            else
            {
                Settings.sAttackModeOfAll = LoadString;
            }
            LoadString = ReadString("String", "AttackModeOfPeaceful", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfPeaceful", Settings.sAttackModeOfPeaceful);
            }
            else
            {
                Settings.sAttackModeOfPeaceful = LoadString;
            }
            LoadString = ReadString("String", "AttackModeOfGroup", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfGroup", Settings.sAttackModeOfGroup);
            }
            else
            {
                Settings.sAttackModeOfGroup = LoadString;
            }
            LoadString = ReadString("String", "AttackModeOfGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfGuild", Settings.sAttackModeOfGuild);
            }
            else
            {
                Settings.sAttackModeOfGuild = LoadString;
            }
            LoadString = ReadString("String", "AttackModeOfRedWhite", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfRedWhite", Settings.sAttackModeOfRedWhite);
            }
            else
            {
                Settings.sAttackModeOfRedWhite = LoadString;
            }
            LoadString = ReadString("String", "StartChangeAttackModeHelp", "");
            if (LoadString == "")
            {
                WriteString("String", "StartChangeAttackModeHelp", Settings.sStartChangeAttackModeHelp);
            }
            else
            {
                Settings.sStartChangeAttackModeHelp = LoadString;
            }
            LoadString = ReadString("String", "StartNoticeMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartNoticeMsg", Settings.sStartNoticeMsg);
            }
            else
            {
                Settings.sStartNoticeMsg = LoadString;
            }
            LoadString = ReadString("String", "ThrustingOn", "");
            if (LoadString == "")
            {
                WriteString("String", "ThrustingOn", Settings.sThrustingOn);
            }
            else
            {
                Settings.sThrustingOn = LoadString;
            }
            LoadString = ReadString("String", "ThrustingOff", "");
            if (LoadString == "")
            {
                WriteString("String", "ThrustingOff", Settings.sThrustingOff);
            }
            else
            {
                Settings.sThrustingOff = LoadString;
            }
            LoadString = ReadString("String", "HalfMoonOn", "");
            if (LoadString == "")
            {
                WriteString("String", "HalfMoonOn", Settings.sHalfMoonOn);
            }
            else
            {
                Settings.sHalfMoonOn = LoadString;
            }
            LoadString = ReadString("String", "HalfMoonOff", "");
            if (LoadString == "")
            {
                WriteString("String", "HalfMoonOff", Settings.sHalfMoonOff);
            }
            else
            {
                Settings.sHalfMoonOff = LoadString;
            }
            Settings.sCrsHitOn = LoadConfigString("String", "CrsHitOn", Settings.sCrsHitOn);
            Settings.sCrsHitOff = LoadConfigString("String", "CrsHitOff", Settings.sCrsHitOff);
            Settings.sTwinHitOn = LoadConfigString("String", "TwinHitOn", Settings.sTwinHitOn);
            Settings.sTwinHitOff = LoadConfigString("String", "TwinHitOff", Settings.sTwinHitOff);
            LoadString = ReadString("String", "FireSpiritsSummoned", "");
            if (LoadString == "")
            {
                WriteString("String", "FireSpiritsSummoned", Settings.sFireSpiritsSummoned);
            }
            else
            {
                Settings.sFireSpiritsSummoned = LoadString;
            }
            LoadString = ReadString("String", "FireSpiritsFail", "");
            if (LoadString == "")
            {
                WriteString("String", "FireSpiritsFail", Settings.sFireSpiritsFail);
            }
            else
            {
                Settings.sFireSpiritsFail = LoadString;
            }
            LoadString = ReadString("String", "SpiritsGone", "");
            if (LoadString == "")
            {
                WriteString("String", "SpiritsGone", Settings.sSpiritsGone);
            }
            else
            {
                Settings.sSpiritsGone = LoadString;
            }
            LoadString = ReadString("String", "MateDoTooweak", "");
            if (LoadString == "")
            {
                WriteString("String", "MateDoTooweak", Settings.sMateDoTooweak);
            }
            else
            {
                Settings.sMateDoTooweak = LoadString;
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
                WriteString("String", "YouPoisoned", Settings.sYouPoisoned);
            }
            else
            {
                Settings.sYouPoisoned = LoadString;
            }
            LoadString = ReadString("String", "PetRest", "");
            if (LoadString == "")
            {
                WriteString("String", "PetRest", Settings.sPetRest);
            }
            else
            {
                Settings.sPetRest = LoadString;
            }
            LoadString = ReadString("String", "PetAttack", "");
            if (LoadString == "")
            {
                WriteString("String", "PetAttack", Settings.sPetAttack);
            }
            else
            {
                Settings.sPetAttack = LoadString;
            }
            LoadString = ReadString("String", "WearNotOfWoMan", "");
            if (LoadString == "")
            {
                WriteString("String", "WearNotOfWoMan", Settings.sWearNotOfWoMan);
            }
            else
            {
                Settings.sWearNotOfWoMan = LoadString;
            }
            LoadString = ReadString("String", "WearNotOfMan", "");
            if (LoadString == "")
            {
                WriteString("String", "WearNotOfMan", Settings.sWearNotOfMan);
            }
            else
            {
                Settings.sWearNotOfMan = LoadString;
            }
            LoadString = ReadString("String", "HandWeightNot", "");
            if (LoadString == "")
            {
                WriteString("String", "HandWeightNot", Settings.sHandWeightNot);
            }
            else
            {
                Settings.sHandWeightNot = LoadString;
            }
            LoadString = ReadString("String", "WearWeightNot", "");
            if (LoadString == "")
            {
                WriteString("String", "WearWeightNot", Settings.sWearWeightNot);
            }
            else
            {
                Settings.sWearWeightNot = LoadString;
            }
            LoadString = ReadString("String", "ItemIsNotThisAccount", "");
            if (LoadString == "")
            {
                WriteString("String", "ItemIsNotThisAccount", Settings.g_sItemIsNotThisAccount);
            }
            else
            {
                Settings.g_sItemIsNotThisAccount = LoadString;
            }
            LoadString = ReadString("String", "ItemIsNotThisIPaddr", "");
            if (LoadString == "")
            {
                WriteString("String", "ItemIsNotThisIPaddr", Settings.g_sItemIsNotThisIPaddr);
            }
            else
            {
                Settings.g_sItemIsNotThisIPaddr = LoadString;
            }
            LoadString = ReadString("String", "ItemIsNotThisChrName", "");
            if (LoadString == "")
            {
                WriteString("String", "ItemIsNotThisChrName", Settings.g_sItemIsNotThisChrName);
            }
            else
            {
                Settings.g_sItemIsNotThisChrName = LoadString;
            }
            LoadString = ReadString("String", "LevelNot", "");
            if (LoadString == "")
            {
                WriteString("String", "LevelNot", Settings.g_sLevelNot);
            }
            else
            {
                Settings.g_sLevelNot = LoadString;
            }
            LoadString = ReadString("String", "JobOrLevelNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrLevelNot", Settings.g_sJobOrLevelNot);
            }
            else
            {
                Settings.g_sJobOrLevelNot = LoadString;
            }
            LoadString = ReadString("String", "JobOrDCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrDCNot", Settings.g_sJobOrDCNot);
            }
            else
            {
                Settings.g_sJobOrDCNot = LoadString;
            }
            LoadString = ReadString("String", "JobOrMCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrMCNot", Settings.g_sJobOrMCNot);
            }
            else
            {
                Settings.g_sJobOrMCNot = LoadString;
            }
            LoadString = ReadString("String", "JobOrSCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrSCNot", Settings.g_sJobOrSCNot);
            }
            else
            {
                Settings.g_sJobOrSCNot = LoadString;
            }
            LoadString = ReadString("String", "DCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "DCNot", Settings.g_sDCNot);
            }
            else
            {
                Settings.g_sDCNot = LoadString;
            }
            LoadString = ReadString("String", "MCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "MCNot", Settings.g_sMCNot);
            }
            else
            {
                Settings.g_sMCNot = LoadString;
            }
            LoadString = ReadString("String", "SCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "SCNot", Settings.g_sSCNot);
            }
            else
            {
                Settings.g_sSCNot = LoadString;
            }
            LoadString = ReadString("String", "CreditPointNot", "");
            if (LoadString == "")
            {
                WriteString("String", "CreditPointNot", Settings.g_sCreditPointNot);
            }
            else
            {
                Settings.g_sCreditPointNot = LoadString;
            }
            LoadString = ReadString("String", "ReNewLevelNot", "");
            if (LoadString == "")
            {
                WriteString("String", "ReNewLevelNot", Settings.g_sReNewLevelNot);
            }
            else
            {
                Settings.g_sReNewLevelNot = LoadString;
            }
            LoadString = ReadString("String", "GuildNot", "");
            if (LoadString == "")
            {
                WriteString("String", "GuildNot", Settings.g_sGuildNot);
            }
            else
            {
                Settings.g_sGuildNot = LoadString;
            }
            LoadString = ReadString("String", "GuildMasterNot", "");
            if (LoadString == "")
            {
                WriteString("String", "GuildMasterNot", Settings.g_sGuildMasterNot);
            }
            else
            {
                Settings.g_sGuildMasterNot = LoadString;
            }
            LoadString = ReadString("String", "SabukHumanNot", "");
            if (LoadString == "")
            {
                WriteString("String", "SabukHumanNot", Settings.g_sSabukHumanNot);
            }
            else
            {
                Settings.g_sSabukHumanNot = LoadString;
            }
            LoadString = ReadString("String", "SabukMasterManNot", "");
            if (LoadString == "")
            {
                WriteString("String", "SabukMasterManNot", Settings.g_sSabukMasterManNot);
            }
            else
            {
                Settings.g_sSabukMasterManNot = LoadString;
            }
            LoadString = ReadString("String", "MemberNot", "");
            if (LoadString == "")
            {
                WriteString("String", "MemberNot", Settings.g_sMemberNot);
            }
            else
            {
                Settings.g_sMemberNot = LoadString;
            }
            LoadString = ReadString("String", "MemberTypeNot", "");
            if (LoadString == "")
            {
                WriteString("String", "MemberTypeNot", Settings.g_sMemberTypeNot);
            }
            else
            {
                Settings.g_sMemberTypeNot = LoadString;
            }
            LoadString = ReadString("String", "CanottWearIt", "");
            if (LoadString == "")
            {
                WriteString("String", "CanottWearIt", Settings.g_sCanottWearIt);
            }
            else
            {
                Settings.g_sCanottWearIt = LoadString;
            }
            LoadString = ReadString("String", "CanotUseDrugOnThisMap", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotUseDrugOnThisMap", Settings.sCanotUseDrugOnThisMap);
            }
            else
            {
                Settings.sCanotUseDrugOnThisMap = LoadString;
            }
            LoadString = ReadString("String", "GameMasterMode", "");
            if (LoadString == "")
            {
                WriteString("String", "GameMasterMode", Settings.sGameMasterMode);
            }
            else
            {
                Settings.sGameMasterMode = LoadString;
            }
            LoadString = ReadString("String", "ReleaseGameMasterMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ReleaseGameMasterMode", Settings.sReleaseGameMasterMode);
            }
            else
            {
                Settings.sReleaseGameMasterMode = LoadString;
            }
            LoadString = ReadString("String", "ObserverMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ObserverMode", Settings.sObserverMode);
            }
            else
            {
                Settings.sObserverMode = LoadString;
            }
            LoadString = ReadString("String", "ReleaseObserverMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ReleaseObserverMode", Settings.g_sReleaseObserverMode);
            }
            else
            {
                Settings.g_sReleaseObserverMode = LoadString;
            }
            LoadString = ReadString("String", "SupermanMode", "");
            if (LoadString == "")
            {
                WriteString("String", "SupermanMode", Settings.sSupermanMode);
            }
            else
            {
                Settings.sSupermanMode = LoadString;
            }
            LoadString = ReadString("String", "ReleaseSupermanMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ReleaseSupermanMode", Settings.sReleaseSupermanMode);
            }
            else
            {
                Settings.sReleaseSupermanMode = LoadString;
            }
            LoadString = ReadString("String", "YouFoundNothing", "");
            if (LoadString == "")
            {
                WriteString("String", "YouFoundNothing", Settings.sYouFoundNothing);
            }
            else
            {
                Settings.sYouFoundNothing = LoadString;
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
                WriteString("String", "NoPasswordLockSystemMsg", Settings.g_sNoPasswordLockSystemMsg);
            }
            else
            {
                Settings.g_sNoPasswordLockSystemMsg = LoadString;
            }
            LoadString = ReadString("String", "AlreadySetPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "AlreadySetPassword", Settings.g_sAlreadySetPasswordMsg);
            }
            else
            {
                Settings.g_sAlreadySetPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "ReSetPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "ReSetPassword", Settings.g_sReSetPasswordMsg);
            }
            else
            {
                Settings.g_sReSetPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "PasswordOverLong", "");
            if (LoadString == "")
            {
                WriteString("String", "PasswordOverLong", Settings.g_sPasswordOverLongMsg);
            }
            else
            {
                Settings.g_sPasswordOverLongMsg = LoadString;
            }
            LoadString = ReadString("String", "ReSetPasswordOK", "");
            if (LoadString == "")
            {
                WriteString("String", "ReSetPasswordOK", Settings.g_sReSetPasswordOKMsg);
            }
            else
            {
                Settings.g_sReSetPasswordOKMsg = LoadString;
            }
            LoadString = ReadString("String", "ReSetPasswordNotMatch", "");
            if (LoadString == "")
            {
                WriteString("String", "ReSetPasswordNotMatch", Settings.g_sReSetPasswordNotMatchMsg);
            }
            else
            {
                Settings.g_sReSetPasswordNotMatchMsg = LoadString;
            }
            LoadString = ReadString("String", "PleaseInputUnLockPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseInputUnLockPassword", Settings.g_sPleaseInputUnLockPasswordMsg);
            }
            else
            {
                Settings.g_sPleaseInputUnLockPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageUnLockOK", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageUnLockOK", Settings.g_sStorageUnLockOKMsg);
            }
            else
            {
                Settings.g_sStorageUnLockOKMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageAlreadyUnLock", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageAlreadyUnLock", Settings.g_sStorageAlreadyUnLockMsg);
            }
            else
            {
                Settings.g_sStorageAlreadyUnLockMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageNoPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageNoPassword", Settings.g_sStorageNoPasswordMsg);
            }
            else
            {
                Settings.g_sStorageNoPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "UnLockPasswordFail", "");
            if (LoadString == "")
            {
                WriteString("String", "UnLockPasswordFail", Settings.g_sUnLockPasswordFailMsg);
            }
            else
            {
                Settings.g_sUnLockPasswordFailMsg = LoadString;
            }
            LoadString = ReadString("String", "LockStorageSuccess", "");
            if (LoadString == "")
            {
                WriteString("String", "LockStorageSuccess", Settings.g_sLockStorageSuccessMsg);
            }
            else
            {
                Settings.g_sLockStorageSuccessMsg = LoadString;
            }
            LoadString = ReadString("String", "StoragePasswordClearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StoragePasswordClearMsg", Settings.g_sStoragePasswordClearMsg);
            }
            else
            {
                Settings.g_sStoragePasswordClearMsg = LoadString;
            }
            LoadString = ReadString("String", "PleaseUnloadStoragePasswordMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseUnloadStoragePasswordMsg", Settings.g_sPleaseUnloadStoragePasswordMsg);
            }
            else
            {
                Settings.g_sPleaseUnloadStoragePasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageAlreadyLock", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageAlreadyLock", Settings.g_sStorageAlreadyLockMsg);
            }
            else
            {
                Settings.g_sStorageAlreadyLockMsg = LoadString;
            }
            LoadString = ReadString("String", "StoragePasswordLocked", "");
            if (LoadString == "")
            {
                WriteString("String", "StoragePasswordLocked", Settings.g_sStoragePasswordLockedMsg);
            }
            else
            {
                Settings.g_sStoragePasswordLockedMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageSetPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageSetPassword", Settings.g_sSetPasswordMsg);
            }
            else
            {
                Settings.g_sSetPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "PleaseInputOldPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseInputOldPassword", Settings.g_sPleaseInputOldPasswordMsg);
            }
            else
            {
                Settings.g_sPleaseInputOldPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "PasswordIsClearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PasswordIsClearMsg", Settings.g_sOldPasswordIsClearMsg);
            }
            else
            {
                Settings.g_sOldPasswordIsClearMsg = LoadString;
            }
            LoadString = ReadString("String", "NoPasswordSet", "");
            if (LoadString == "")
            {
                WriteString("String", "NoPasswordSet", Settings.g_sNoPasswordSetMsg);
            }
            else
            {
                Settings.g_sNoPasswordSetMsg = LoadString;
            }
            LoadString = ReadString("String", "OldPasswordIncorrect", "");
            if (LoadString == "")
            {
                WriteString("String", "OldPasswordIncorrect", Settings.g_sOldPasswordIncorrectMsg);
            }
            else
            {
                Settings.g_sOldPasswordIncorrectMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageIsLocked", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageIsLocked", Settings.g_sStorageIsLockedMsg);
            }
            else
            {
                Settings.g_sStorageIsLockedMsg = LoadString;
            }
            LoadString = ReadString("String", "PleaseTryDealLaterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseTryDealLaterMsg", Settings.g_sPleaseTryDealLaterMsg);
            }
            else
            {
                Settings.g_sPleaseTryDealLaterMsg = LoadString;
            }
            LoadString = ReadString("String", "DealItemsDenyGetBackMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DealItemsDenyGetBackMsg", Settings.g_sDealItemsDenyGetBackMsg);
            }
            else
            {
                Settings.g_sDealItemsDenyGetBackMsg = LoadString;
            }
            LoadString = ReadString("String", "DisableDealItemsMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableDealItemsMsg", Settings.g_sDisableDealItemsMsg);
            }
            else
            {
                Settings.g_sDisableDealItemsMsg = LoadString;
            }
            LoadString = ReadString("String", "CanotTryDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotTryDealMsg", Settings.g_sCanotTryDealMsg);
            }
            else
            {
                Settings.g_sCanotTryDealMsg = LoadString;
            }
            LoadString = ReadString("String", "DealActionCancelMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DealActionCancelMsg", Settings.g_sDealActionCancelMsg);
            }
            else
            {
                Settings.g_sDealActionCancelMsg = LoadString;
            }
            LoadString = ReadString("String", "PoseDisableDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PoseDisableDealMsg", Settings.g_sPoseDisableDealMsg);
            }
            else
            {
                Settings.g_sPoseDisableDealMsg = LoadString;
            }
            LoadString = ReadString("String", "DealSuccessMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DealSuccessMsg", Settings.g_sDealSuccessMsg);
            }
            else
            {
                Settings.g_sDealSuccessMsg = LoadString;
            }
            LoadString = ReadString("String", "DealOKTooFast", "");
            if (LoadString == "")
            {
                WriteString("String", "DealOKTooFast", Settings.g_sDealOKTooFast);
            }
            else
            {
                Settings.g_sDealOKTooFast = LoadString;
            }
            LoadString = ReadString("String", "YourBagSizeTooSmall", "");
            if (LoadString == "")
            {
                WriteString("String", "YourBagSizeTooSmall", Settings.g_sYourBagSizeTooSmall);
            }
            else
            {
                Settings.g_sYourBagSizeTooSmall = LoadString;
            }
            LoadString = ReadString("String", "DealHumanBagSizeTooSmall", "");
            if (LoadString == "")
            {
                WriteString("String", "DealHumanBagSizeTooSmall", Settings.g_sDealHumanBagSizeTooSmall);
            }
            else
            {
                Settings.g_sDealHumanBagSizeTooSmall = LoadString;
            }
            LoadString = ReadString("String", "YourGoldLargeThenLimit", "");
            if (LoadString == "")
            {
                WriteString("String", "YourGoldLargeThenLimit", Settings.g_sYourGoldLargeThenLimit);
            }
            else
            {
                Settings.g_sYourGoldLargeThenLimit = LoadString;
            }
            LoadString = ReadString("String", "DealHumanGoldLargeThenLimit", "");
            if (LoadString == "")
            {
                WriteString("String", "DealHumanGoldLargeThenLimit", Settings.g_sDealHumanGoldLargeThenLimit);
            }
            else
            {
                Settings.g_sDealHumanGoldLargeThenLimit = LoadString;
            }
            LoadString = ReadString("String", "YouDealOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouDealOKMsg", Settings.g_sYouDealOKMsg);
            }
            else
            {
                Settings.g_sYouDealOKMsg = LoadString;
            }
            LoadString = ReadString("String", "PoseDealOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PoseDealOKMsg", Settings.g_sPoseDealOKMsg);
            }
            else
            {
                Settings.g_sPoseDealOKMsg = LoadString;
            }
            LoadString = ReadString("String", "KickClientUserMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "KickClientUserMsg", Settings.g_sKickClientUserMsg);
            }
            else
            {
                Settings.g_sKickClientUserMsg = LoadString;
            }
            LoadString = ReadString("String", "ActionIsLockedMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ActionIsLockedMsg", Settings.g_sActionIsLockedMsg);
            }
            else
            {
                Settings.g_sActionIsLockedMsg = LoadString;
            }
            LoadString = ReadString("String", "PasswordNotSetMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PasswordNotSetMsg", Settings.g_sPasswordNotSetMsg);
            }
            else
            {
                Settings.g_sPasswordNotSetMsg = LoadString;
            }
            LoadString = ReadString("String", "NotPasswordProtectMode", "");
            if (LoadString == "")
            {
                WriteString("String", "NotPasswordProtectMode", Settings.g_sNotPasswordProtectMode);
            }
            else
            {
                Settings.g_sNotPasswordProtectMode = LoadString;
            }
            LoadString = ReadString("String", "CanotDropGoldMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropGoldMsg", Settings.g_sCanotDropGoldMsg);
            }
            else
            {
                Settings.g_sCanotDropGoldMsg = LoadString;
            }
            LoadString = ReadString("String", "CanotDropInSafeZoneMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropInSafeZoneMsg", Settings.g_sCanotDropInSafeZoneMsg);
            }
            else
            {
                Settings.g_sCanotDropInSafeZoneMsg = LoadString;
            }
            LoadString = ReadString("String", "CanotDropItemMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropItemMsg", Settings.g_sCanotDropItemMsg);
            }
            else
            {
                Settings.g_sCanotDropItemMsg = LoadString;
            }
            LoadString = ReadString("String", "CanotDropItemMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropItemMsg", Settings.g_sCanotDropItemMsg);
            }
            else
            {
                Settings.g_sCanotDropItemMsg = LoadString;
            }
            LoadString = ReadString("String", "CanotUseItemMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotUseItemMsg", Settings.g_sCanotUseItemMsg);
            }
            else
            {
                Settings.g_sCanotUseItemMsg = LoadString;
            }
            LoadString = ReadString("String", "StartMarryManMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryManMsg", Settings.g_sStartMarryManMsg);
            }
            else
            {
                Settings.g_sStartMarryManMsg = LoadString;
            }
            LoadString = ReadString("String", "StartMarryWoManMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryWoManMsg", Settings.g_sStartMarryWoManMsg);
            }
            else
            {
                Settings.g_sStartMarryWoManMsg = LoadString;
            }
            LoadString = ReadString("String", "StartMarryManAskQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryManAskQuestionMsg", Settings.g_sStartMarryManAskQuestionMsg);
            }
            else
            {
                Settings.g_sStartMarryManAskQuestionMsg = LoadString;
            }
            LoadString = ReadString("String", "StartMarryWoManAskQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryWoManAskQuestionMsg", Settings.g_sStartMarryWoManAskQuestionMsg);
            }
            else
            {
                Settings.g_sStartMarryWoManAskQuestionMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryManAnswerQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryManAnswerQuestionMsg", Settings.g_sMarryManAnswerQuestionMsg);
            }
            else
            {
                Settings.g_sMarryManAnswerQuestionMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryManAskQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryManAskQuestionMsg", Settings.g_sMarryManAskQuestionMsg);
            }
            else
            {
                Settings.g_sMarryManAskQuestionMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryWoManAnswerQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManAnswerQuestionMsg", Settings.g_sMarryWoManAnswerQuestionMsg);
            }
            else
            {
                Settings.g_sMarryWoManAnswerQuestionMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryWoManGetMarryMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManGetMarryMsg", Settings.g_sMarryWoManGetMarryMsg);
            }
            else
            {
                Settings.g_sMarryWoManGetMarryMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryWoManDenyMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManDenyMsg", Settings.g_sMarryWoManDenyMsg);
            }
            else
            {
                Settings.g_sMarryWoManDenyMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryWoManCancelMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManCancelMsg", Settings.g_sMarryWoManCancelMsg);
            }
            else
            {
                Settings.g_sMarryWoManCancelMsg = LoadString;
            }
            LoadString = ReadString("String", "ForceUnMarryManLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ForceUnMarryManLoginMsg", Settings.g_sfUnMarryManLoginMsg);
            }
            else
            {
                Settings.g_sfUnMarryManLoginMsg = LoadString;
            }
            LoadString = ReadString("String", "ForceUnMarryWoManLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ForceUnMarryWoManLoginMsg", Settings.g_sfUnMarryWoManLoginMsg);
            }
            else
            {
                Settings.g_sfUnMarryWoManLoginMsg = LoadString;
            }
            LoadString = ReadString("String", "ManLoginDearOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLoginDearOnlineSelfMsg", Settings.g_sManLoginDearOnlineSelfMsg);
            }
            else
            {
                Settings.g_sManLoginDearOnlineSelfMsg = LoadString;
            }
            LoadString = ReadString("String", "ManLoginDearOnlineDearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLoginDearOnlineDearMsg", Settings.g_sManLoginDearOnlineDearMsg);
            }
            else
            {
                Settings.g_sManLoginDearOnlineDearMsg = LoadString;
            }
            LoadString = ReadString("String", "WoManLoginDearOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLoginDearOnlineSelfMsg", Settings.g_sWoManLoginDearOnlineSelfMsg);
            }
            else
            {
                Settings.g_sWoManLoginDearOnlineSelfMsg = LoadString;
            }
            LoadString = ReadString("String", "WoManLoginDearOnlineDearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLoginDearOnlineDearMsg", Settings.g_sWoManLoginDearOnlineDearMsg);
            }
            else
            {
                Settings.g_sWoManLoginDearOnlineDearMsg = LoadString;
            }
            LoadString = ReadString("String", "ManLoginDearNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLoginDearNotOnlineMsg", Settings.g_sManLoginDearNotOnlineMsg);
            }
            else
            {
                Settings.g_sManLoginDearNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "WoManLoginDearNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLoginDearNotOnlineMsg", Settings.g_sWoManLoginDearNotOnlineMsg);
            }
            else
            {
                Settings.g_sWoManLoginDearNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "ManLongOutDearOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLongOutDearOnlineMsg", Settings.g_sManLongOutDearOnlineMsg);
            }
            else
            {
                Settings.g_sManLongOutDearOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "WoManLongOutDearOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLongOutDearOnlineMsg", Settings.g_sWoManLongOutDearOnlineMsg);
            }
            else
            {
                Settings.g_sWoManLongOutDearOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YouAreNotMarryedMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouAreNotMarryedMsg", Settings.g_sYouAreNotMarryedMsg);
            }
            else
            {
                Settings.g_sYouAreNotMarryedMsg = LoadString;
            }
            LoadString = ReadString("String", "YourWifeNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourWifeNotOnlineMsg", Settings.g_sYourWifeNotOnlineMsg);
            }
            else
            {
                Settings.g_sYourWifeNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YourHusbandNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourHusbandNotOnlineMsg", Settings.g_sYourHusbandNotOnlineMsg);
            }
            else
            {
                Settings.g_sYourHusbandNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YourWifeNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourWifeNowLocateMsg", Settings.g_sYourWifeNowLocateMsg);
            }
            else
            {
                Settings.g_sYourWifeNowLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourHusbandSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourHusbandSearchLocateMsg", Settings.g_sYourHusbandSearchLocateMsg);
            }
            else
            {
                Settings.g_sYourHusbandSearchLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourHusbandNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourHusbandNowLocateMsg", Settings.g_sYourHusbandNowLocateMsg);
            }
            else
            {
                Settings.g_sYourHusbandNowLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourWifeSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourWifeSearchLocateMsg", Settings.g_sYourWifeSearchLocateMsg);
            }
            else
            {
                Settings.g_sYourWifeSearchLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "FUnMasterLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "FUnMasterLoginMsg", Settings.g_sfUnMasterLoginMsg);
            }
            else
            {
                Settings.g_sfUnMasterLoginMsg = LoadString;
            }
            LoadString = ReadString("String", "UnMasterListLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "UnMasterListLoginMsg", Settings.g_sfUnMasterListLoginMsg);
            }
            else
            {
                Settings.g_sfUnMasterListLoginMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterListOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListOnlineSelfMsg", Settings.g_sMasterListOnlineSelfMsg);
            }
            else
            {
                Settings.g_sMasterListOnlineSelfMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterListOnlineMasterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListOnlineMasterMsg", Settings.g_sMasterListOnlineMasterMsg);
            }
            else
            {
                Settings.g_sMasterListOnlineMasterMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterOnlineSelfMsg", Settings.g_sMasterOnlineSelfMsg);
            }
            else
            {
                Settings.g_sMasterOnlineSelfMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterOnlineMasterListMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterOnlineMasterListMsg", Settings.g_sMasterOnlineMasterListMsg);
            }
            else
            {
                Settings.g_sMasterOnlineMasterListMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterLongOutMasterListOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterLongOutMasterListOnlineMsg", Settings.g_sMasterLongOutMasterListOnlineMsg);
            }
            else
            {
                Settings.g_sMasterLongOutMasterListOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterListLongOutMasterOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListLongOutMasterOnlineMsg", Settings.g_sMasterListLongOutMasterOnlineMsg);
            }
            else
            {
                Settings.g_sMasterListLongOutMasterOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterListNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListNotOnlineMsg", Settings.g_sMasterListNotOnlineMsg);
            }
            else
            {
                Settings.g_sMasterListNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterNotOnlineMsg", Settings.g_sMasterNotOnlineMsg);
            }
            else
            {
                Settings.g_sMasterNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YouAreNotMasterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouAreNotMasterMsg", Settings.g_sYouAreNotMasterMsg);
            }
            else
            {
                Settings.g_sYouAreNotMasterMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterNotOnlineMsg", Settings.g_sYourMasterNotOnlineMsg);
            }
            else
            {
                Settings.g_sYourMasterNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterListNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListNotOnlineMsg", Settings.g_sYourMasterListNotOnlineMsg);
            }
            else
            {
                Settings.g_sYourMasterListNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterNowLocateMsg", Settings.g_sYourMasterNowLocateMsg);
            }
            else
            {
                Settings.g_sYourMasterNowLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterListSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListSearchLocateMsg", Settings.g_sYourMasterListSearchLocateMsg);
            }
            else
            {
                Settings.g_sYourMasterListSearchLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterListNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListNowLocateMsg", Settings.g_sYourMasterListNowLocateMsg);
            }
            else
            {
                Settings.g_sYourMasterListNowLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterSearchLocateMsg", Settings.g_sYourMasterSearchLocateMsg);
            }
            else
            {
                Settings.g_sYourMasterSearchLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterListUnMasterOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListUnMasterOKMsg", Settings.g_sYourMasterListUnMasterOKMsg);
            }
            else
            {
                Settings.g_sYourMasterListUnMasterOKMsg = LoadString;
            }
            LoadString = ReadString("String", "YouAreUnMasterOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouAreUnMasterOKMsg", Settings.g_sYouAreUnMasterOKMsg);
            }
            else
            {
                Settings.g_sYouAreUnMasterOKMsg = LoadString;
            }
            LoadString = ReadString("String", "UnMasterLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "UnMasterLoginMsg", Settings.g_sUnMasterLoginMsg);
            }
            else
            {
                Settings.g_sUnMasterLoginMsg = LoadString;
            }
            LoadString = ReadString("String", "NPCSayUnMasterOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "NPCSayUnMasterOKMsg", Settings.g_sNPCSayUnMasterOKMsg);
            }
            else
            {
                Settings.g_sNPCSayUnMasterOKMsg = LoadString;
            }
            LoadString = ReadString("String", "NPCSayForceUnMasterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "NPCSayForceUnMasterMsg", Settings.g_sNPCSayForceUnMasterMsg);
            }
            else
            {
                Settings.g_sNPCSayForceUnMasterMsg = LoadString;
            }
            LoadString = ReadString("String", "MyInfo", "");
            if (LoadString == "")
            {
                WriteString("String", "MyInfo", Settings.g_sMyInfo);
            }
            else
            {
                Settings.g_sMyInfo = LoadString;
            }
            LoadString = ReadString("String", "OpenedDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "OpenedDealMsg", Settings.g_sOpenedDealMsg);
            }
            else
            {
                Settings.g_sOpenedDealMsg = LoadString;
            }
            LoadString = ReadString("String", "SendCustMsgCanNotUseNowMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "SendCustMsgCanNotUseNowMsg", Settings.g_sSendCustMsgCanNotUseNowMsg);
            }
            else
            {
                Settings.g_sSendCustMsgCanNotUseNowMsg = LoadString;
            }
            LoadString = ReadString("String", "SubkMasterMsgCanNotUseNowMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "SubkMasterMsgCanNotUseNowMsg", Settings.g_sSubkMasterMsgCanNotUseNowMsg);
            }
            else
            {
                Settings.g_sSubkMasterMsgCanNotUseNowMsg = LoadString;
            }
            LoadString = ReadString("String", "SendOnlineCountMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "SendOnlineCountMsg", Settings.g_sSendOnlineCountMsg);
            }
            else
            {
                Settings.g_sSendOnlineCountMsg = LoadString;
            }
            LoadString = ReadString("String", "WeaponRepairSuccess", "");
            if (LoadString == "")
            {
                WriteString("String", "WeaponRepairSuccess", Settings.g_sWeaponRepairSuccess);
            }
            else
            {
                Settings.g_sWeaponRepairSuccess = LoadString;
            }
            LoadString = ReadString("String", "DefenceUpTime", "");
            if (LoadString == "")
            {
                WriteString("String", "DefenceUpTime", Settings.g_sDefenceUpTime);
            }
            else
            {
                Settings.g_sDefenceUpTime = LoadString;
            }
            LoadString = ReadString("String", "MagDefenceUpTime", "");
            if (LoadString == "")
            {
                WriteString("String", "MagDefenceUpTime", Settings.g_sMagDefenceUpTime);
            }
            else
            {
                Settings.g_sMagDefenceUpTime = LoadString;
            }
            LoadString = ReadString("String", "WinLottery1Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery1Msg", Settings.g_sWinLottery1Msg);
            }
            else
            {
                Settings.g_sWinLottery1Msg = LoadString;
            }
            LoadString = ReadString("String", "WinLottery2Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery2Msg", Settings.g_sWinLottery2Msg);
            }
            else
            {
                Settings.g_sWinLottery2Msg = LoadString;
            }
            LoadString = ReadString("String", "WinLottery3Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery3Msg", Settings.g_sWinLottery3Msg);
            }
            else
            {
                Settings.g_sWinLottery3Msg = LoadString;
            }
            LoadString = ReadString("String", "WinLottery4Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery4Msg", Settings.g_sWinLottery4Msg);
            }
            else
            {
                Settings.g_sWinLottery4Msg = LoadString;
            }
            LoadString = ReadString("String", "WinLottery5Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery5Msg", Settings.g_sWinLottery5Msg);
            }
            else
            {
                Settings.g_sWinLottery5Msg = LoadString;
            }
            LoadString = ReadString("String", "WinLottery6Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery6Msg", Settings.g_sWinLottery6Msg);
            }
            else
            {
                Settings.g_sWinLottery6Msg = LoadString;
            }
            LoadString = ReadString("String", "NotWinLotteryMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "NotWinLotteryMsg", Settings.g_sNotWinLotteryMsg);
            }
            else
            {
                Settings.g_sNotWinLotteryMsg = LoadString;
            }
            LoadString = ReadString("String", "WeaptonMakeLuck", "");
            if (LoadString == "")
            {
                WriteString("String", "WeaptonMakeLuck", Settings.g_sWeaptonMakeLuck);
            }
            else
            {
                Settings.g_sWeaptonMakeLuck = LoadString;
            }
            LoadString = ReadString("String", "WeaptonNotMakeLuck", "");
            if (LoadString == "")
            {
                WriteString("String", "WeaptonNotMakeLuck", Settings.g_sWeaptonNotMakeLuck);
            }
            else
            {
                Settings.g_sWeaptonNotMakeLuck = LoadString;
            }
            LoadString = ReadString("String", "TheWeaponIsCursed", "");
            if (LoadString == "")
            {
                WriteString("String", "TheWeaponIsCursed", Settings.g_sTheWeaponIsCursed);
            }
            else
            {
                Settings.g_sTheWeaponIsCursed = LoadString;
            }
            LoadString = ReadString("String", "CanotTakeOffItem", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotTakeOffItem", Settings.g_sCanotTakeOffItem);
            }
            else
            {
                Settings.g_sCanotTakeOffItem = LoadString;
            }
            LoadString = ReadString("String", "JoinGroupMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "JoinGroupMsg", Settings.g_sJoinGroup);
            }
            else
            {
                Settings.g_sJoinGroup = LoadString;
            }
            LoadString = ReadString("String", "TryModeCanotUseStorage", "");
            if (LoadString == "")
            {
                WriteString("String", "TryModeCanotUseStorage", Settings.g_sTryModeCanotUseStorage);
            }
            else
            {
                Settings.g_sTryModeCanotUseStorage = LoadString;
            }
            LoadString = ReadString("String", "CanotGetItemsMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotGetItemsMsg", Settings.g_sCanotGetItems);
            }
            else
            {
                Settings.g_sCanotGetItems = LoadString;
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
                WriteString("String", "YourIPaddrDenyLogon", Settings.g_sYourIPaddrDenyLogon);
            }
            else
            {
                Settings.g_sYourIPaddrDenyLogon = LoadString;
            }
            LoadString = ReadString("String", "YourAccountDenyLogon", "");
            if (LoadString == "")
            {
                WriteString("String", "YourAccountDenyLogon", Settings.g_sYourAccountDenyLogon);
            }
            else
            {
                Settings.g_sYourAccountDenyLogon = LoadString;
            }
            LoadString = ReadString("String", "YourChrNameDenyLogon", "");
            if (LoadString == "")
            {
                WriteString("String", "YourChrNameDenyLogon", Settings.g_sYourChrNameDenyLogon);
            }
            else
            {
                Settings.g_sYourChrNameDenyLogon = LoadString;
            }
            LoadString = ReadString("String", "CanotPickUpItem", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotPickUpItem", Settings.g_sCanotPickUpItem);
            }
            else
            {
                Settings.g_sCanotPickUpItem = LoadString;
            }
            LoadString = ReadString("String", "sQUERYBAGITEMS", "");
            if (LoadString == "")
            {
                WriteString("String", "sQUERYBAGITEMS", Settings.g_sQUERYBAGITEMS);
            }
            else
            {
                Settings.g_sQUERYBAGITEMS = LoadString;
            }
            LoadString = ReadString("String", "CanotSendmsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotSendmsg", Settings.g_sCanotSendmsg);
            }
            else
            {
                Settings.g_sCanotSendmsg = LoadString;
            }
            LoadString = ReadString("String", "UserDenyWhisperMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "UserDenyWhisperMsg", Settings.g_sUserDenyWhisperMsg);
            }
            else
            {
                Settings.g_sUserDenyWhisperMsg = LoadString;
            }
            LoadString = ReadString("String", "UserNotOnLine", "");
            if (LoadString == "")
            {
                WriteString("String", "UserNotOnLine", Settings.g_sUserNotOnLine);
            }
            else
            {
                Settings.g_sUserNotOnLine = LoadString;
            }
            LoadString = ReadString("String", "RevivalRecoverMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "RevivalRecoverMsg", Settings.g_sRevivalRecoverMsg);
            }
            else
            {
                Settings.g_sRevivalRecoverMsg = LoadString;
            }
            LoadString = ReadString("String", "ClientVersionTooOld", "");
            if (LoadString == "")
            {
                WriteString("String", "ClientVersionTooOld", Settings.g_sClientVersionTooOld);
            }
            else
            {
                Settings.g_sClientVersionTooOld = LoadString;
            }
            LoadString = ReadString("String", "CastleGuildName", "");
            if (LoadString == "")
            {
                WriteString("String", "CastleGuildName", Settings.g_sCastleGuildName);
            }
            else
            {
                Settings.g_sCastleGuildName = LoadString;
            }
            LoadString = ReadString("String", "NoCastleGuildName", "");
            if (LoadString == "")
            {
                WriteString("String", "NoCastleGuildName", Settings.g_sNoCastleGuildName);
            }
            else
            {
                Settings.g_sNoCastleGuildName = LoadString;
            }
            LoadString = ReadString("String", "WarrReNewName", "");
            if (LoadString == "")
            {
                WriteString("String", "WarrReNewName", Settings.g_sWarrReNewName);
            }
            else
            {
                Settings.g_sWarrReNewName = LoadString;
            }
            LoadString = ReadString("String", "WizardReNewName", "");
            if (LoadString == "")
            {
                WriteString("String", "WizardReNewName", Settings.g_sWizardReNewName);
            }
            else
            {
                Settings.g_sWizardReNewName = LoadString;
            }
            LoadString = ReadString("String", "TaosReNewName", "");
            if (LoadString == "")
            {
                WriteString("String", "TaosReNewName", Settings.g_sTaosReNewName);
            }
            else
            {
                Settings.g_sTaosReNewName = LoadString;
            }
            LoadString = ReadString("String", "RankLevelName", "");
            if (LoadString == "")
            {
                WriteString("String", "RankLevelName", Settings.g_sRankLevelName);
            }
            else
            {
                Settings.g_sRankLevelName = LoadString.Replace("%s", "{0}");
            }
            LoadString = ReadString("String", "ManDearName", "");
            if (LoadString == "")
            {
                WriteString("String", "ManDearName", Settings.g_sManDearName);
            }
            else
            {
                Settings.g_sManDearName = LoadString;
            }
            LoadString = ReadString("String", "WoManDearName", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManDearName", Settings.g_sWoManDearName);
            }
            else
            {
                Settings.g_sWoManDearName = LoadString;
            }
            LoadString = ReadString("String", "MasterName", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterName", Settings.g_sMasterName);
            }
            else
            {
                Settings.g_sMasterName = LoadString;
            }

            LoadString = ReadString("String", "NoMasterName", "");
            if (LoadString == "")
            {
                WriteString("String", "NoMasterName", Settings.g_sNoMasterName);
            }
            else
            {
                Settings.g_sNoMasterName = LoadString;
            }
            LoadString = ReadString("String", "HumanShowName", "");
            if (LoadString == "")
            {
                WriteString("String", "HumanShowName", Settings.g_sHumanShowName);
            }
            else
            {
                Settings.g_sHumanShowName = LoadString;
            }
            LoadString = ReadString("String", "ChangePermissionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangePermissionMsg", Settings.g_sChangePermissionMsg);
            }
            else
            {
                Settings.g_sChangePermissionMsg = LoadString;
            }
            LoadString = ReadString("String", "ChangeKillMonExpRateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangeKillMonExpRateMsg", Settings.g_sChangeKillMonExpRateMsg);
            }
            else
            {
                Settings.g_sChangeKillMonExpRateMsg = LoadString;
            }
            LoadString = ReadString("String", "ChangePowerRateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangePowerRateMsg", Settings.g_sChangePowerRateMsg);
            }
            else
            {
                Settings.g_sChangePowerRateMsg = LoadString;
            }
            LoadString = ReadString("String", "ChangeMemberLevelMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangeMemberLevelMsg", Settings.g_sChangeMemberLevelMsg);
            }
            else
            {
                Settings.g_sChangeMemberLevelMsg = LoadString;
            }
            LoadString = ReadString("String", "ChangeMemberTypeMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangeMemberTypeMsg", Settings.g_sChangeMemberTypeMsg);
            }
            else
            {
                Settings.g_sChangeMemberTypeMsg = LoadString;
            }
            LoadString = ReadString("String", "ScriptChangeHumanHPMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ScriptChangeHumanHPMsg", Settings.g_sScriptChangeHumanHPMsg);
            }
            else
            {
                Settings.g_sScriptChangeHumanHPMsg = LoadString;
            }
            LoadString = ReadString("String", "ScriptChangeHumanMPMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ScriptChangeHumanMPMsg", Settings.g_sScriptChangeHumanMPMsg);
            }
            else
            {
                Settings.g_sScriptChangeHumanMPMsg = LoadString;
            }
            LoadString = ReadString("String", "YouCanotDisableSayMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouCanotDisableSayMsg", Settings.g_sDisableSayMsg);
            }
            else
            {
                Settings.g_sDisableSayMsg = LoadString;
            }
            LoadString = ReadString("String", "OnlineCountMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "OnlineCountMsg", Settings.g_sOnlineCountMsg);
            }
            else
            {
                Settings.g_sOnlineCountMsg = LoadString;
            }
            LoadString = ReadString("String", "TotalOnlineCountMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "TotalOnlineCountMsg", Settings.g_sTotalOnlineCountMsg);
            }
            else
            {
                Settings.g_sTotalOnlineCountMsg = LoadString;
            }
            LoadString = ReadString("String", "YouNeedLevelSendMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouNeedLevelSendMsg", Settings.g_sYouNeedLevelMsg);
            }
            else
            {
                Settings.g_sYouNeedLevelMsg = LoadString;
            }
            LoadString = ReadString("String", "ThisMapDisableSendCyCyMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ThisMapDisableSendCyCyMsg", Settings.g_sThisMapDisableSendCyCyMsg);
            }
            else
            {
                Settings.g_sThisMapDisableSendCyCyMsg = LoadString;
            }
            LoadString = ReadString("String", "YouCanSendCyCyLaterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouCanSendCyCyLaterMsg", Settings.g_sYouCanSendCyCyLaterMsg);
            }
            else
            {
                Settings.g_sYouCanSendCyCyLaterMsg = LoadString;
            }
            LoadString = ReadString("String", "YouIsDisableSendMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouIsDisableSendMsg", Settings.g_sYouIsDisableSendMsg);
            }
            else
            {
                Settings.g_sYouIsDisableSendMsg = LoadString;
            }
            LoadString = ReadString("String", "YouMurderedMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouMurderedMsg", Settings.g_sYouMurderedMsg);
            }
            else
            {
                Settings.g_sYouMurderedMsg = LoadString;
            }
            LoadString = ReadString("String", "YouKilledByMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouKilledByMsg", Settings.g_sYouKilledByMsg);
            }
            else
            {
                Settings.g_sYouKilledByMsg = LoadString;
            }
            LoadString = ReadString("String", "YouprotectedByLawOfDefense", "");
            if (LoadString == "")
            {
                WriteString("String", "YouprotectedByLawOfDefense", Settings.g_sYouprotectedByLawOfDefense);
            }
            else
            {
                Settings.g_sYouprotectedByLawOfDefense = LoadString;
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