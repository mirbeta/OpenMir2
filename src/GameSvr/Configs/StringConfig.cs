using SystemModule.Common;

namespace GameSvr.Configs
{
    public class StringConfig : IniFile
    {
        public StringConfig(string fileName) : base(fileName)
        {

        }

        public void LoadString()
        {
            Load();
            string LoadString;
            if (ReadString("Server", "ServerIP", "") == "")
            {
                WriteString("Server", "ServerIP", M2Share.g_Config.sServerIPaddr);
            }
            M2Share.g_Config.sServerIPaddr = ReadString("Server", "ServerIP", M2Share.g_Config.sServerIPaddr);
            if (ReadString("Server", "WebSite", "") == "")
            {
                WriteString("Server", "WebSite", M2Share.g_Config.sWebSite);
            }
            M2Share.g_Config.sWebSite = ReadString("Server", "WebSite", M2Share.g_Config.sWebSite);
            if (ReadString("Server", "BbsSite", "") == "")
            {
                WriteString("Server", "BbsSite", M2Share.g_Config.sBbsSite);
            }
            M2Share.g_Config.sBbsSite = ReadString("Server", "BbsSite", M2Share.g_Config.sBbsSite);
            if (ReadString("Server", "ClientDownload", "") == "")
            {
                WriteString("Server", "ClientDownload", M2Share.g_Config.sClientDownload);
            }
            M2Share.g_Config.sClientDownload = ReadString("Server", "ClientDownload", M2Share.g_Config.sClientDownload);
            if (ReadString("Server", "QQ", "") == "")
            {
                WriteString("Server", "QQ", M2Share.g_Config.sQQ);
            }
            M2Share.g_Config.sQQ = ReadString("Server", "QQ", M2Share.g_Config.sQQ);
            if (ReadString("Server", "Phone", "") == "")
            {
                WriteString("Server", "Phone", M2Share.g_Config.sPhone);
            }
            M2Share.g_Config.sPhone = ReadString("Server", "Phone", M2Share.g_Config.sPhone);
            if (ReadString("Server", "BankAccount0", "") == "")
            {
                WriteString("Server", "BankAccount0", M2Share.g_Config.sBankAccount0);
            }
            M2Share.g_Config.sBankAccount0 = ReadString("Server", "BankAccount0", M2Share.g_Config.sBankAccount0);
            if (ReadString("Server", "BankAccount1", "") == "")
            {
                WriteString("Server", "BankAccount1", M2Share.g_Config.sBankAccount1);
            }
            M2Share.g_Config.sBankAccount1 = ReadString("Server", "BankAccount1", M2Share.g_Config.sBankAccount1);
            if (ReadString("Server", "BankAccount2", "") == "")
            {
                WriteString("Server", "BankAccount2", M2Share.g_Config.sBankAccount2);
            }
            M2Share.g_Config.sBankAccount2 = ReadString("Server", "BankAccount2", M2Share.g_Config.sBankAccount2);
            if (ReadString("Server", "BankAccount3", "") == "")
            {
                WriteString("Server", "BankAccount3", M2Share.g_Config.sBankAccount3);
            }
            M2Share.g_Config.sBankAccount3 = ReadString("Server", "BankAccount3", M2Share.g_Config.sBankAccount3);
            if (ReadString("Server", "BankAccount4", "") == "")
            {
                WriteString("Server", "BankAccount4", M2Share.g_Config.sBankAccount4);
            }
            M2Share.g_Config.sBankAccount4 = ReadString("Server", "BankAccount4", M2Share.g_Config.sBankAccount4);
            if (ReadString("Server", "BankAccount5", "") == "")
            {
                WriteString("Server", "BankAccount5", M2Share.g_Config.sBankAccount5);
            }
            M2Share.g_Config.sBankAccount5 = ReadString("Server", "BankAccount5", M2Share.g_Config.sBankAccount5);
            if (ReadString("Server", "BankAccount6", "") == "")
            {
                WriteString("Server", "BankAccount6", M2Share.g_Config.sBankAccount6);
            }
            M2Share.g_Config.sBankAccount6 = ReadString("Server", "BankAccount6", M2Share.g_Config.sBankAccount6);
            if (ReadString("Server", "BankAccount7", "") == "")
            {
                WriteString("Server", "BankAccount7", M2Share.g_Config.sBankAccount7);
            }
            M2Share.g_Config.sBankAccount7 = ReadString("Server", "BankAccount7", M2Share.g_Config.sBankAccount7);
            if (ReadString("Server", "BankAccount8", "") == "")
            {
                WriteString("Server", "BankAccount8", M2Share.g_Config.sBankAccount8);
            }
            M2Share.g_Config.sBankAccount8 = ReadString("Server", "BankAccount8", M2Share.g_Config.sBankAccount8);
            if (ReadString("Server", "BankAccount9", "") == "")
            {
                WriteString("Server", "BankAccount9", M2Share.g_Config.sBankAccount9);
            }
            M2Share.g_Config.sBankAccount9 = ReadString("Server", "BankAccount9", M2Share.g_Config.sBankAccount9);
            if (ReadString("Guild", "GuildNotice", "") == "")
            {
                WriteString("Guild", "GuildNotice", M2Share.g_Config.sGuildNotice);
            }
            M2Share.g_Config.sGuildNotice = ReadString("Guild", "GuildNotice", M2Share.g_Config.sGuildNotice);
            if (ReadString("Guild", "GuildWar", "") == "")
            {
                WriteString("Guild", "GuildWar", M2Share.g_Config.sGuildWar);
            }
            M2Share.g_Config.sGuildWar = ReadString("Guild", "GuildWar", M2Share.g_Config.sGuildWar);
            if (ReadString("Guild", "GuildAll", "") == "")
            {
                WriteString("Guild", "GuildAll", M2Share.g_Config.sGuildAll);
            }
            M2Share.g_Config.sGuildAll = ReadString("Guild", "GuildAll", M2Share.g_Config.sGuildAll);
            if (ReadString("Guild", "GuildMember", "") == "")
            {
                WriteString("Guild", "GuildMember", M2Share.g_Config.sGuildMember);
            }
            M2Share.g_Config.sGuildMember = ReadString("Guild", "GuildMember", M2Share.g_Config.sGuildMember);
            if (ReadString("Guild", "GuildMemberRank", "") == "")
            {
                WriteString("Guild", "GuildMemberRank", M2Share.g_Config.sGuildMemberRank);
            }
            M2Share.g_Config.sGuildMemberRank = ReadString("Guild", "GuildMemberRank", M2Share.g_Config.sGuildMemberRank);
            if (ReadString("Guild", "GuildChief", "") == "")
            {
                WriteString("Guild", "GuildChief", M2Share.g_Config.sGuildChief);
            }
            M2Share.g_Config.sGuildChief = ReadString("Guild", "GuildChief", M2Share.g_Config.sGuildChief);
            LoadString = ReadString("String", "ClientSoftVersionError", "");
            if (LoadString == "")
            {
                WriteString("String", "ClientSoftVersionError", M2Share.sClientSoftVersionError);
            }
            else
            {
                M2Share.sClientSoftVersionError = LoadString;
            }
            LoadString = ReadString("String", "DownLoadNewClientSoft", "");
            if (LoadString == "")
            {
                WriteString("String", "DownLoadNewClientSoft", M2Share.sDownLoadNewClientSoft);
            }
            else
            {
                M2Share.sDownLoadNewClientSoft = LoadString;
            }
            LoadString = ReadString("String", "ForceDisConnect", "");
            if (LoadString == "")
            {
                WriteString("String", "ForceDisConnect", M2Share.sForceDisConnect);
            }
            else
            {
                M2Share.sForceDisConnect = LoadString;
            }
            LoadString = ReadString("String", "ClientSoftVersionTooOld", "");
            if (LoadString == "")
            {
                WriteString("String", "ClientSoftVersionTooOld", M2Share.sClientSoftVersionTooOld);
            }
            else
            {
                M2Share.sClientSoftVersionTooOld = LoadString;
            }
            LoadString = ReadString("String", "DownLoadAndUseNewClient", "");
            if (LoadString == "")
            {
                WriteString("String", "DownLoadAndUseNewClient", M2Share.sDownLoadAndUseNewClient);
            }
            else
            {
                M2Share.sDownLoadAndUseNewClient = LoadString;
            }
            LoadString = ReadString("String", "OnlineUserFull", "");
            if (LoadString == "")
            {
                WriteString("String", "OnlineUserFull", M2Share.sOnlineUserFull);
            }
            else
            {
                M2Share.sOnlineUserFull = LoadString;
            }
            LoadString = ReadString("String", "YouNowIsTryPlayMode", "");
            if (LoadString == "")
            {
                WriteString("String", "YouNowIsTryPlayMode", M2Share.sYouNowIsTryPlayMode);
            }
            else
            {
                M2Share.sYouNowIsTryPlayMode = LoadString;
            }
            LoadString = ReadString("String", "NowIsFreePlayMode", "");
            if (LoadString == "")
            {
                WriteString("String", "NowIsFreePlayMode", M2Share.g_sNowIsFreePlayMode);
            }
            else
            {
                M2Share.g_sNowIsFreePlayMode = LoadString;
            }
            LoadString = ReadString("String", "AttackModeOfAll", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfAll", M2Share.sAttackModeOfAll);
            }
            else
            {
                M2Share.sAttackModeOfAll = LoadString;
            }
            LoadString = ReadString("String", "AttackModeOfPeaceful", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfPeaceful", M2Share.sAttackModeOfPeaceful);
            }
            else
            {
                M2Share.sAttackModeOfPeaceful = LoadString;
            }
            LoadString = ReadString("String", "AttackModeOfGroup", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfGroup", M2Share.sAttackModeOfGroup);
            }
            else
            {
                M2Share.sAttackModeOfGroup = LoadString;
            }
            LoadString = ReadString("String", "AttackModeOfGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfGuild", M2Share.sAttackModeOfGuild);
            }
            else
            {
                M2Share.sAttackModeOfGuild = LoadString;
            }
            LoadString = ReadString("String", "AttackModeOfRedWhite", "");
            if (LoadString == "")
            {
                WriteString("String", "AttackModeOfRedWhite", M2Share.sAttackModeOfRedWhite);
            }
            else
            {
                M2Share.sAttackModeOfRedWhite = LoadString;
            }
            LoadString = ReadString("String", "StartChangeAttackModeHelp", "");
            if (LoadString == "")
            {
                WriteString("String", "StartChangeAttackModeHelp", M2Share.sStartChangeAttackModeHelp);
            }
            else
            {
                M2Share.sStartChangeAttackModeHelp = LoadString;
            }
            LoadString = ReadString("String", "StartNoticeMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartNoticeMsg", M2Share.sStartNoticeMsg);
            }
            else
            {
                M2Share.sStartNoticeMsg = LoadString;
            }
            LoadString = ReadString("String", "ThrustingOn", "");
            if (LoadString == "")
            {
                WriteString("String", "ThrustingOn", M2Share.sThrustingOn);
            }
            else
            {
                M2Share.sThrustingOn = LoadString;
            }
            LoadString = ReadString("String", "ThrustingOff", "");
            if (LoadString == "")
            {
                WriteString("String", "ThrustingOff", M2Share.sThrustingOff);
            }
            else
            {
                M2Share.sThrustingOff = LoadString;
            }
            LoadString = ReadString("String", "HalfMoonOn", "");
            if (LoadString == "")
            {
                WriteString("String", "HalfMoonOn", M2Share.sHalfMoonOn);
            }
            else
            {
                M2Share.sHalfMoonOn = LoadString;
            }
            LoadString = ReadString("String", "HalfMoonOff", "");
            if (LoadString == "")
            {
                WriteString("String", "HalfMoonOff", M2Share.sHalfMoonOff);
            }
            else
            {
                M2Share.sHalfMoonOff = LoadString;
            }
            M2Share.sCrsHitOn = LoadConfigString("String", "CrsHitOn", M2Share.sCrsHitOn);
            M2Share.sCrsHitOff = LoadConfigString("String", "CrsHitOff", M2Share.sCrsHitOff);
            M2Share.sTwinHitOn = LoadConfigString("String", "TwinHitOn", M2Share.sTwinHitOn);
            M2Share.sTwinHitOff = LoadConfigString("String", "TwinHitOff", M2Share.sTwinHitOff);
            LoadString = ReadString("String", "FireSpiritsSummoned", "");
            if (LoadString == "")
            {
                WriteString("String", "FireSpiritsSummoned", M2Share.sFireSpiritsSummoned);
            }
            else
            {
                M2Share.sFireSpiritsSummoned = LoadString;
            }
            LoadString = ReadString("String", "FireSpiritsFail", "");
            if (LoadString == "")
            {
                WriteString("String", "FireSpiritsFail", M2Share.sFireSpiritsFail);
            }
            else
            {
                M2Share.sFireSpiritsFail = LoadString;
            }
            LoadString = ReadString("String", "SpiritsGone", "");
            if (LoadString == "")
            {
                WriteString("String", "SpiritsGone", M2Share.sSpiritsGone);
            }
            else
            {
                M2Share.sSpiritsGone = LoadString;
            }
            LoadString = ReadString("String", "MateDoTooweak", "");
            if (LoadString == "")
            {
                WriteString("String", "MateDoTooweak", M2Share.sMateDoTooweak);
            }
            else
            {
                M2Share.sMateDoTooweak = LoadString;
            }
            LoadString = ReadString("String", "TheWeaponBroke", "");
            if (LoadString == "")
            {
                WriteString("String", "TheWeaponBroke", M2Share.g_sTheWeaponBroke);
            }
            else
            {
                M2Share.g_sTheWeaponBroke = LoadString;
            }
            LoadString = ReadString("String", "TheWeaponRefineSuccessfull", "");
            if (LoadString == "")
            {
                WriteString("String", "TheWeaponRefineSuccessfull", M2Share.sTheWeaponRefineSuccessfull);
            }
            else
            {
                M2Share.sTheWeaponRefineSuccessfull = LoadString;
            }
            LoadString = ReadString("String", "YouPoisoned", "");
            if (LoadString == "")
            {
                WriteString("String", "YouPoisoned", M2Share.sYouPoisoned);
            }
            else
            {
                M2Share.sYouPoisoned = LoadString;
            }
            LoadString = ReadString("String", "PetRest", "");
            if (LoadString == "")
            {
                WriteString("String", "PetRest", M2Share.sPetRest);
            }
            else
            {
                M2Share.sPetRest = LoadString;
            }
            LoadString = ReadString("String", "PetAttack", "");
            if (LoadString == "")
            {
                WriteString("String", "PetAttack", M2Share.sPetAttack);
            }
            else
            {
                M2Share.sPetAttack = LoadString;
            }
            LoadString = ReadString("String", "WearNotOfWoMan", "");
            if (LoadString == "")
            {
                WriteString("String", "WearNotOfWoMan", M2Share.sWearNotOfWoMan);
            }
            else
            {
                M2Share.sWearNotOfWoMan = LoadString;
            }
            LoadString = ReadString("String", "WearNotOfMan", "");
            if (LoadString == "")
            {
                WriteString("String", "WearNotOfMan", M2Share.sWearNotOfMan);
            }
            else
            {
                M2Share.sWearNotOfMan = LoadString;
            }
            LoadString = ReadString("String", "HandWeightNot", "");
            if (LoadString == "")
            {
                WriteString("String", "HandWeightNot", M2Share.sHandWeightNot);
            }
            else
            {
                M2Share.sHandWeightNot = LoadString;
            }
            LoadString = ReadString("String", "WearWeightNot", "");
            if (LoadString == "")
            {
                WriteString("String", "WearWeightNot", M2Share.sWearWeightNot);
            }
            else
            {
                M2Share.sWearWeightNot = LoadString;
            }
            LoadString = ReadString("String", "ItemIsNotThisAccount", "");
            if (LoadString == "")
            {
                WriteString("String", "ItemIsNotThisAccount", M2Share.g_sItemIsNotThisAccount);
            }
            else
            {
                M2Share.g_sItemIsNotThisAccount = LoadString;
            }
            LoadString = ReadString("String", "ItemIsNotThisIPaddr", "");
            if (LoadString == "")
            {
                WriteString("String", "ItemIsNotThisIPaddr", M2Share.g_sItemIsNotThisIPaddr);
            }
            else
            {
                M2Share.g_sItemIsNotThisIPaddr = LoadString;
            }
            LoadString = ReadString("String", "ItemIsNotThisCharName", "");
            if (LoadString == "")
            {
                WriteString("String", "ItemIsNotThisCharName", M2Share.g_sItemIsNotThisCharName);
            }
            else
            {
                M2Share.g_sItemIsNotThisCharName = LoadString;
            }
            LoadString = ReadString("String", "LevelNot", "");
            if (LoadString == "")
            {
                WriteString("String", "LevelNot", M2Share.g_sLevelNot);
            }
            else
            {
                M2Share.g_sLevelNot = LoadString;
            }
            LoadString = ReadString("String", "JobOrLevelNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrLevelNot", M2Share.g_sJobOrLevelNot);
            }
            else
            {
                M2Share.g_sJobOrLevelNot = LoadString;
            }
            LoadString = ReadString("String", "JobOrDCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrDCNot", M2Share.g_sJobOrDCNot);
            }
            else
            {
                M2Share.g_sJobOrDCNot = LoadString;
            }
            LoadString = ReadString("String", "JobOrMCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrMCNot", M2Share.g_sJobOrMCNot);
            }
            else
            {
                M2Share.g_sJobOrMCNot = LoadString;
            }
            LoadString = ReadString("String", "JobOrSCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "JobOrSCNot", M2Share.g_sJobOrSCNot);
            }
            else
            {
                M2Share.g_sJobOrSCNot = LoadString;
            }
            LoadString = ReadString("String", "DCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "DCNot", M2Share.g_sDCNot);
            }
            else
            {
                M2Share.g_sDCNot = LoadString;
            }
            LoadString = ReadString("String", "MCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "MCNot", M2Share.g_sMCNot);
            }
            else
            {
                M2Share.g_sMCNot = LoadString;
            }
            LoadString = ReadString("String", "SCNot", "");
            if (LoadString == "")
            {
                WriteString("String", "SCNot", M2Share.g_sSCNot);
            }
            else
            {
                M2Share.g_sSCNot = LoadString;
            }
            LoadString = ReadString("String", "CreditPointNot", "");
            if (LoadString == "")
            {
                WriteString("String", "CreditPointNot", M2Share.g_sCreditPointNot);
            }
            else
            {
                M2Share.g_sCreditPointNot = LoadString;
            }
            LoadString = ReadString("String", "ReNewLevelNot", "");
            if (LoadString == "")
            {
                WriteString("String", "ReNewLevelNot", M2Share.g_sReNewLevelNot);
            }
            else
            {
                M2Share.g_sReNewLevelNot = LoadString;
            }
            LoadString = ReadString("String", "GuildNot", "");
            if (LoadString == "")
            {
                WriteString("String", "GuildNot", M2Share.g_sGuildNot);
            }
            else
            {
                M2Share.g_sGuildNot = LoadString;
            }
            LoadString = ReadString("String", "GuildMasterNot", "");
            if (LoadString == "")
            {
                WriteString("String", "GuildMasterNot", M2Share.g_sGuildMasterNot);
            }
            else
            {
                M2Share.g_sGuildMasterNot = LoadString;
            }
            LoadString = ReadString("String", "SabukHumanNot", "");
            if (LoadString == "")
            {
                WriteString("String", "SabukHumanNot", M2Share.g_sSabukHumanNot);
            }
            else
            {
                M2Share.g_sSabukHumanNot = LoadString;
            }
            LoadString = ReadString("String", "SabukMasterManNot", "");
            if (LoadString == "")
            {
                WriteString("String", "SabukMasterManNot", M2Share.g_sSabukMasterManNot);
            }
            else
            {
                M2Share.g_sSabukMasterManNot = LoadString;
            }
            LoadString = ReadString("String", "MemberNot", "");
            if (LoadString == "")
            {
                WriteString("String", "MemberNot", M2Share.g_sMemberNot);
            }
            else
            {
                M2Share.g_sMemberNot = LoadString;
            }
            LoadString = ReadString("String", "MemberTypeNot", "");
            if (LoadString == "")
            {
                WriteString("String", "MemberTypeNot", M2Share.g_sMemberTypeNot);
            }
            else
            {
                M2Share.g_sMemberTypeNot = LoadString;
            }
            LoadString = ReadString("String", "CanottWearIt", "");
            if (LoadString == "")
            {
                WriteString("String", "CanottWearIt", M2Share.g_sCanottWearIt);
            }
            else
            {
                M2Share.g_sCanottWearIt = LoadString;
            }
            LoadString = ReadString("String", "CanotUseDrugOnThisMap", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotUseDrugOnThisMap", M2Share.sCanotUseDrugOnThisMap);
            }
            else
            {
                M2Share.sCanotUseDrugOnThisMap = LoadString;
            }
            LoadString = ReadString("String", "GameMasterMode", "");
            if (LoadString == "")
            {
                WriteString("String", "GameMasterMode", M2Share.sGameMasterMode);
            }
            else
            {
                M2Share.sGameMasterMode = LoadString;
            }
            LoadString = ReadString("String", "ReleaseGameMasterMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ReleaseGameMasterMode", M2Share.sReleaseGameMasterMode);
            }
            else
            {
                M2Share.sReleaseGameMasterMode = LoadString;
            }
            LoadString = ReadString("String", "ObserverMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ObserverMode", M2Share.sObserverMode);
            }
            else
            {
                M2Share.sObserverMode = LoadString;
            }
            LoadString = ReadString("String", "ReleaseObserverMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ReleaseObserverMode", M2Share.g_sReleaseObserverMode);
            }
            else
            {
                M2Share.g_sReleaseObserverMode = LoadString;
            }
            LoadString = ReadString("String", "SupermanMode", "");
            if (LoadString == "")
            {
                WriteString("String", "SupermanMode", M2Share.sSupermanMode);
            }
            else
            {
                M2Share.sSupermanMode = LoadString;
            }
            LoadString = ReadString("String", "ReleaseSupermanMode", "");
            if (LoadString == "")
            {
                WriteString("String", "ReleaseSupermanMode", M2Share.sReleaseSupermanMode);
            }
            else
            {
                M2Share.sReleaseSupermanMode = LoadString;
            }
            LoadString = ReadString("String", "YouFoundNothing", "");
            if (LoadString == "")
            {
                WriteString("String", "YouFoundNothing", M2Share.sYouFoundNothing);
            }
            else
            {
                M2Share.sYouFoundNothing = LoadString;
            }
            LoadString = ReadString("String", "LineNoticePreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "LineNoticePreFix", M2Share.g_Config.sLineNoticePreFix);
            }
            else
            {
                M2Share.g_Config.sLineNoticePreFix = LoadString;
            }
            LoadString = ReadString("String", "SysMsgPreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "SysMsgPreFix", M2Share.g_Config.sSysMsgPreFix);
            }
            else
            {
                M2Share.g_Config.sSysMsgPreFix = LoadString;
            }
            LoadString = ReadString("String", "GuildMsgPreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "GuildMsgPreFix", M2Share.g_Config.sGuildMsgPreFix);
            }
            else
            {
                M2Share.g_Config.sGuildMsgPreFix = LoadString;
            }
            LoadString = ReadString("String", "GroupMsgPreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "GroupMsgPreFix", M2Share.g_Config.sGroupMsgPreFix);
            }
            else
            {
                M2Share.g_Config.sGroupMsgPreFix = LoadString;
            }
            LoadString = ReadString("String", "HintMsgPreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "HintMsgPreFix", M2Share.g_Config.sHintMsgPreFix);
            }
            else
            {
                M2Share.g_Config.sHintMsgPreFix = LoadString;
            }
            LoadString = ReadString("String", "GMRedMsgpreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "GMRedMsgpreFix", M2Share.g_Config.sGMRedMsgpreFix);
            }
            else
            {
                M2Share.g_Config.sGMRedMsgpreFix = LoadString;
            }
            LoadString = ReadString("String", "MonSayMsgpreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "MonSayMsgpreFix", M2Share.g_Config.sMonSayMsgpreFix);
            }
            else
            {
                M2Share.g_Config.sMonSayMsgpreFix = LoadString;
            }
            LoadString = ReadString("String", "CustMsgpreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "CustMsgpreFix", M2Share.g_Config.sCustMsgpreFix);
            }
            else
            {
                M2Share.g_Config.sCustMsgpreFix = LoadString;
            }
            LoadString = ReadString("String", "CastleMsgpreFix", "");
            if (LoadString == "")
            {
                WriteString("String", "CastleMsgpreFix", M2Share.g_Config.sCastleMsgpreFix);
            }
            else
            {
                M2Share.g_Config.sCastleMsgpreFix = LoadString;
            }
            LoadString = ReadString("String", "NoPasswordLockSystemMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "NoPasswordLockSystemMsg", M2Share.g_sNoPasswordLockSystemMsg);
            }
            else
            {
                M2Share.g_sNoPasswordLockSystemMsg = LoadString;
            }
            LoadString = ReadString("String", "AlreadySetPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "AlreadySetPassword", M2Share.g_sAlreadySetPasswordMsg);
            }
            else
            {
                M2Share.g_sAlreadySetPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "ReSetPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "ReSetPassword", M2Share.g_sReSetPasswordMsg);
            }
            else
            {
                M2Share.g_sReSetPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "PasswordOverLong", "");
            if (LoadString == "")
            {
                WriteString("String", "PasswordOverLong", M2Share.g_sPasswordOverLongMsg);
            }
            else
            {
                M2Share.g_sPasswordOverLongMsg = LoadString;
            }
            LoadString = ReadString("String", "ReSetPasswordOK", "");
            if (LoadString == "")
            {
                WriteString("String", "ReSetPasswordOK", M2Share.g_sReSetPasswordOKMsg);
            }
            else
            {
                M2Share.g_sReSetPasswordOKMsg = LoadString;
            }
            LoadString = ReadString("String", "ReSetPasswordNotMatch", "");
            if (LoadString == "")
            {
                WriteString("String", "ReSetPasswordNotMatch", M2Share.g_sReSetPasswordNotMatchMsg);
            }
            else
            {
                M2Share.g_sReSetPasswordNotMatchMsg = LoadString;
            }
            LoadString = ReadString("String", "PleaseInputUnLockPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseInputUnLockPassword", M2Share.g_sPleaseInputUnLockPasswordMsg);
            }
            else
            {
                M2Share.g_sPleaseInputUnLockPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageUnLockOK", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageUnLockOK", M2Share.g_sStorageUnLockOKMsg);
            }
            else
            {
                M2Share.g_sStorageUnLockOKMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageAlreadyUnLock", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageAlreadyUnLock", M2Share.g_sStorageAlreadyUnLockMsg);
            }
            else
            {
                M2Share.g_sStorageAlreadyUnLockMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageNoPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageNoPassword", M2Share.g_sStorageNoPasswordMsg);
            }
            else
            {
                M2Share.g_sStorageNoPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "UnLockPasswordFail", "");
            if (LoadString == "")
            {
                WriteString("String", "UnLockPasswordFail", M2Share.g_sUnLockPasswordFailMsg);
            }
            else
            {
                M2Share.g_sUnLockPasswordFailMsg = LoadString;
            }
            LoadString = ReadString("String", "LockStorageSuccess", "");
            if (LoadString == "")
            {
                WriteString("String", "LockStorageSuccess", M2Share.g_sLockStorageSuccessMsg);
            }
            else
            {
                M2Share.g_sLockStorageSuccessMsg = LoadString;
            }
            LoadString = ReadString("String", "StoragePasswordClearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StoragePasswordClearMsg", M2Share.g_sStoragePasswordClearMsg);
            }
            else
            {
                M2Share.g_sStoragePasswordClearMsg = LoadString;
            }
            LoadString = ReadString("String", "PleaseUnloadStoragePasswordMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseUnloadStoragePasswordMsg", M2Share.g_sPleaseUnloadStoragePasswordMsg);
            }
            else
            {
                M2Share.g_sPleaseUnloadStoragePasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageAlreadyLock", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageAlreadyLock", M2Share.g_sStorageAlreadyLockMsg);
            }
            else
            {
                M2Share.g_sStorageAlreadyLockMsg = LoadString;
            }
            LoadString = ReadString("String", "StoragePasswordLocked", "");
            if (LoadString == "")
            {
                WriteString("String", "StoragePasswordLocked", M2Share.g_sStoragePasswordLockedMsg);
            }
            else
            {
                M2Share.g_sStoragePasswordLockedMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageSetPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageSetPassword", M2Share.g_sSetPasswordMsg);
            }
            else
            {
                M2Share.g_sSetPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "PleaseInputOldPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseInputOldPassword", M2Share.g_sPleaseInputOldPasswordMsg);
            }
            else
            {
                M2Share.g_sPleaseInputOldPasswordMsg = LoadString;
            }
            LoadString = ReadString("String", "PasswordIsClearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PasswordIsClearMsg", M2Share.g_sOldPasswordIsClearMsg);
            }
            else
            {
                M2Share.g_sOldPasswordIsClearMsg = LoadString;
            }
            LoadString = ReadString("String", "NoPasswordSet", "");
            if (LoadString == "")
            {
                WriteString("String", "NoPasswordSet", M2Share.g_sNoPasswordSetMsg);
            }
            else
            {
                M2Share.g_sNoPasswordSetMsg = LoadString;
            }
            LoadString = ReadString("String", "OldPasswordIncorrect", "");
            if (LoadString == "")
            {
                WriteString("String", "OldPasswordIncorrect", M2Share.g_sOldPasswordIncorrectMsg);
            }
            else
            {
                M2Share.g_sOldPasswordIncorrectMsg = LoadString;
            }
            LoadString = ReadString("String", "StorageIsLocked", "");
            if (LoadString == "")
            {
                WriteString("String", "StorageIsLocked", M2Share.g_sStorageIsLockedMsg);
            }
            else
            {
                M2Share.g_sStorageIsLockedMsg = LoadString;
            }
            LoadString = ReadString("String", "PleaseTryDealLaterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseTryDealLaterMsg", M2Share.g_sPleaseTryDealLaterMsg);
            }
            else
            {
                M2Share.g_sPleaseTryDealLaterMsg = LoadString;
            }
            LoadString = ReadString("String", "DealItemsDenyGetBackMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DealItemsDenyGetBackMsg", M2Share.g_sDealItemsDenyGetBackMsg);
            }
            else
            {
                M2Share.g_sDealItemsDenyGetBackMsg = LoadString;
            }
            LoadString = ReadString("String", "DisableDealItemsMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableDealItemsMsg", M2Share.g_sDisableDealItemsMsg);
            }
            else
            {
                M2Share.g_sDisableDealItemsMsg = LoadString;
            }
            LoadString = ReadString("String", "CanotTryDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotTryDealMsg", M2Share.g_sCanotTryDealMsg);
            }
            else
            {
                M2Share.g_sCanotTryDealMsg = LoadString;
            }
            LoadString = ReadString("String", "DealActionCancelMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DealActionCancelMsg", M2Share.g_sDealActionCancelMsg);
            }
            else
            {
                M2Share.g_sDealActionCancelMsg = LoadString;
            }
            LoadString = ReadString("String", "PoseDisableDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PoseDisableDealMsg", M2Share.g_sPoseDisableDealMsg);
            }
            else
            {
                M2Share.g_sPoseDisableDealMsg = LoadString;
            }
            LoadString = ReadString("String", "DealSuccessMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DealSuccessMsg", M2Share.g_sDealSuccessMsg);
            }
            else
            {
                M2Share.g_sDealSuccessMsg = LoadString;
            }
            LoadString = ReadString("String", "DealOKTooFast", "");
            if (LoadString == "")
            {
                WriteString("String", "DealOKTooFast", M2Share.g_sDealOKTooFast);
            }
            else
            {
                M2Share.g_sDealOKTooFast = LoadString;
            }
            LoadString = ReadString("String", "YourBagSizeTooSmall", "");
            if (LoadString == "")
            {
                WriteString("String", "YourBagSizeTooSmall", M2Share.g_sYourBagSizeTooSmall);
            }
            else
            {
                M2Share.g_sYourBagSizeTooSmall = LoadString;
            }
            LoadString = ReadString("String", "DealHumanBagSizeTooSmall", "");
            if (LoadString == "")
            {
                WriteString("String", "DealHumanBagSizeTooSmall", M2Share.g_sDealHumanBagSizeTooSmall);
            }
            else
            {
                M2Share.g_sDealHumanBagSizeTooSmall = LoadString;
            }
            LoadString = ReadString("String", "YourGoldLargeThenLimit", "");
            if (LoadString == "")
            {
                WriteString("String", "YourGoldLargeThenLimit", M2Share.g_sYourGoldLargeThenLimit);
            }
            else
            {
                M2Share.g_sYourGoldLargeThenLimit = LoadString;
            }
            LoadString = ReadString("String", "DealHumanGoldLargeThenLimit", "");
            if (LoadString == "")
            {
                WriteString("String", "DealHumanGoldLargeThenLimit", M2Share.g_sDealHumanGoldLargeThenLimit);
            }
            else
            {
                M2Share.g_sDealHumanGoldLargeThenLimit = LoadString;
            }
            LoadString = ReadString("String", "YouDealOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouDealOKMsg", M2Share.g_sYouDealOKMsg);
            }
            else
            {
                M2Share.g_sYouDealOKMsg = LoadString;
            }
            LoadString = ReadString("String", "PoseDealOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PoseDealOKMsg", M2Share.g_sPoseDealOKMsg);
            }
            else
            {
                M2Share.g_sPoseDealOKMsg = LoadString;
            }
            LoadString = ReadString("String", "KickClientUserMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "KickClientUserMsg", M2Share.g_sKickClientUserMsg);
            }
            else
            {
                M2Share.g_sKickClientUserMsg = LoadString;
            }
            LoadString = ReadString("String", "ActionIsLockedMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ActionIsLockedMsg", M2Share.g_sActionIsLockedMsg);
            }
            else
            {
                M2Share.g_sActionIsLockedMsg = LoadString;
            }
            LoadString = ReadString("String", "PasswordNotSetMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "PasswordNotSetMsg", M2Share.g_sPasswordNotSetMsg);
            }
            else
            {
                M2Share.g_sPasswordNotSetMsg = LoadString;
            }
            LoadString = ReadString("String", "NotPasswordProtectMode", "");
            if (LoadString == "")
            {
                WriteString("String", "NotPasswordProtectMode", M2Share.g_sNotPasswordProtectMode);
            }
            else
            {
                M2Share.g_sNotPasswordProtectMode = LoadString;
            }
            LoadString = ReadString("String", "CanotDropGoldMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropGoldMsg", M2Share.g_sCanotDropGoldMsg);
            }
            else
            {
                M2Share.g_sCanotDropGoldMsg = LoadString;
            }
            LoadString = ReadString("String", "CanotDropInSafeZoneMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropInSafeZoneMsg", M2Share.g_sCanotDropInSafeZoneMsg);
            }
            else
            {
                M2Share.g_sCanotDropInSafeZoneMsg = LoadString;
            }
            LoadString = ReadString("String", "CanotDropItemMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropItemMsg", M2Share.g_sCanotDropItemMsg);
            }
            else
            {
                M2Share.g_sCanotDropItemMsg = LoadString;
            }
            LoadString = ReadString("String", "CanotDropItemMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotDropItemMsg", M2Share.g_sCanotDropItemMsg);
            }
            else
            {
                M2Share.g_sCanotDropItemMsg = LoadString;
            }
            LoadString = ReadString("String", "CanotUseItemMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotUseItemMsg", M2Share.g_sCanotUseItemMsg);
            }
            else
            {
                M2Share.g_sCanotUseItemMsg = LoadString;
            }
            LoadString = ReadString("String", "StartMarryManMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryManMsg", M2Share.g_sStartMarryManMsg);
            }
            else
            {
                M2Share.g_sStartMarryManMsg = LoadString;
            }
            LoadString = ReadString("String", "StartMarryWoManMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryWoManMsg", M2Share.g_sStartMarryWoManMsg);
            }
            else
            {
                M2Share.g_sStartMarryWoManMsg = LoadString;
            }
            LoadString = ReadString("String", "StartMarryManAskQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryManAskQuestionMsg", M2Share.g_sStartMarryManAskQuestionMsg);
            }
            else
            {
                M2Share.g_sStartMarryManAskQuestionMsg = LoadString;
            }
            LoadString = ReadString("String", "StartMarryWoManAskQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "StartMarryWoManAskQuestionMsg", M2Share.g_sStartMarryWoManAskQuestionMsg);
            }
            else
            {
                M2Share.g_sStartMarryWoManAskQuestionMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryManAnswerQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryManAnswerQuestionMsg", M2Share.g_sMarryManAnswerQuestionMsg);
            }
            else
            {
                M2Share.g_sMarryManAnswerQuestionMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryManAskQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryManAskQuestionMsg", M2Share.g_sMarryManAskQuestionMsg);
            }
            else
            {
                M2Share.g_sMarryManAskQuestionMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryWoManAnswerQuestionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManAnswerQuestionMsg", M2Share.g_sMarryWoManAnswerQuestionMsg);
            }
            else
            {
                M2Share.g_sMarryWoManAnswerQuestionMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryWoManGetMarryMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManGetMarryMsg", M2Share.g_sMarryWoManGetMarryMsg);
            }
            else
            {
                M2Share.g_sMarryWoManGetMarryMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryWoManDenyMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManDenyMsg", M2Share.g_sMarryWoManDenyMsg);
            }
            else
            {
                M2Share.g_sMarryWoManDenyMsg = LoadString;
            }
            LoadString = ReadString("String", "MarryWoManCancelMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MarryWoManCancelMsg", M2Share.g_sMarryWoManCancelMsg);
            }
            else
            {
                M2Share.g_sMarryWoManCancelMsg = LoadString;
            }
            LoadString = ReadString("String", "ForceUnMarryManLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ForceUnMarryManLoginMsg", M2Share.g_sfUnMarryManLoginMsg);
            }
            else
            {
                M2Share.g_sfUnMarryManLoginMsg = LoadString;
            }
            LoadString = ReadString("String", "ForceUnMarryWoManLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ForceUnMarryWoManLoginMsg", M2Share.g_sfUnMarryWoManLoginMsg);
            }
            else
            {
                M2Share.g_sfUnMarryWoManLoginMsg = LoadString;
            }
            LoadString = ReadString("String", "ManLoginDearOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLoginDearOnlineSelfMsg", M2Share.g_sManLoginDearOnlineSelfMsg);
            }
            else
            {
                M2Share.g_sManLoginDearOnlineSelfMsg = LoadString;
            }
            LoadString = ReadString("String", "ManLoginDearOnlineDearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLoginDearOnlineDearMsg", M2Share.g_sManLoginDearOnlineDearMsg);
            }
            else
            {
                M2Share.g_sManLoginDearOnlineDearMsg = LoadString;
            }
            LoadString = ReadString("String", "WoManLoginDearOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLoginDearOnlineSelfMsg", M2Share.g_sWoManLoginDearOnlineSelfMsg);
            }
            else
            {
                M2Share.g_sWoManLoginDearOnlineSelfMsg = LoadString;
            }
            LoadString = ReadString("String", "WoManLoginDearOnlineDearMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLoginDearOnlineDearMsg", M2Share.g_sWoManLoginDearOnlineDearMsg);
            }
            else
            {
                M2Share.g_sWoManLoginDearOnlineDearMsg = LoadString;
            }
            LoadString = ReadString("String", "ManLoginDearNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLoginDearNotOnlineMsg", M2Share.g_sManLoginDearNotOnlineMsg);
            }
            else
            {
                M2Share.g_sManLoginDearNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "WoManLoginDearNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLoginDearNotOnlineMsg", M2Share.g_sWoManLoginDearNotOnlineMsg);
            }
            else
            {
                M2Share.g_sWoManLoginDearNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "ManLongOutDearOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ManLongOutDearOnlineMsg", M2Share.g_sManLongOutDearOnlineMsg);
            }
            else
            {
                M2Share.g_sManLongOutDearOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "WoManLongOutDearOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManLongOutDearOnlineMsg", M2Share.g_sWoManLongOutDearOnlineMsg);
            }
            else
            {
                M2Share.g_sWoManLongOutDearOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YouAreNotMarryedMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouAreNotMarryedMsg", M2Share.g_sYouAreNotMarryedMsg);
            }
            else
            {
                M2Share.g_sYouAreNotMarryedMsg = LoadString;
            }
            LoadString = ReadString("String", "YourWifeNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourWifeNotOnlineMsg", M2Share.g_sYourWifeNotOnlineMsg);
            }
            else
            {
                M2Share.g_sYourWifeNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YourHusbandNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourHusbandNotOnlineMsg", M2Share.g_sYourHusbandNotOnlineMsg);
            }
            else
            {
                M2Share.g_sYourHusbandNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YourWifeNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourWifeNowLocateMsg", M2Share.g_sYourWifeNowLocateMsg);
            }
            else
            {
                M2Share.g_sYourWifeNowLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourHusbandSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourHusbandSearchLocateMsg", M2Share.g_sYourHusbandSearchLocateMsg);
            }
            else
            {
                M2Share.g_sYourHusbandSearchLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourHusbandNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourHusbandNowLocateMsg", M2Share.g_sYourHusbandNowLocateMsg);
            }
            else
            {
                M2Share.g_sYourHusbandNowLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourWifeSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourWifeSearchLocateMsg", M2Share.g_sYourWifeSearchLocateMsg);
            }
            else
            {
                M2Share.g_sYourWifeSearchLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "FUnMasterLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "FUnMasterLoginMsg", M2Share.g_sfUnMasterLoginMsg);
            }
            else
            {
                M2Share.g_sfUnMasterLoginMsg = LoadString;
            }
            LoadString = ReadString("String", "UnMasterListLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "UnMasterListLoginMsg", M2Share.g_sfUnMasterListLoginMsg);
            }
            else
            {
                M2Share.g_sfUnMasterListLoginMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterListOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListOnlineSelfMsg", M2Share.g_sMasterListOnlineSelfMsg);
            }
            else
            {
                M2Share.g_sMasterListOnlineSelfMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterListOnlineMasterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListOnlineMasterMsg", M2Share.g_sMasterListOnlineMasterMsg);
            }
            else
            {
                M2Share.g_sMasterListOnlineMasterMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterOnlineSelfMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterOnlineSelfMsg", M2Share.g_sMasterOnlineSelfMsg);
            }
            else
            {
                M2Share.g_sMasterOnlineSelfMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterOnlineMasterListMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterOnlineMasterListMsg", M2Share.g_sMasterOnlineMasterListMsg);
            }
            else
            {
                M2Share.g_sMasterOnlineMasterListMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterLongOutMasterListOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterLongOutMasterListOnlineMsg", M2Share.g_sMasterLongOutMasterListOnlineMsg);
            }
            else
            {
                M2Share.g_sMasterLongOutMasterListOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterListLongOutMasterOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListLongOutMasterOnlineMsg", M2Share.g_sMasterListLongOutMasterOnlineMsg);
            }
            else
            {
                M2Share.g_sMasterListLongOutMasterOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterListNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterListNotOnlineMsg", M2Share.g_sMasterListNotOnlineMsg);
            }
            else
            {
                M2Share.g_sMasterListNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "MasterNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterNotOnlineMsg", M2Share.g_sMasterNotOnlineMsg);
            }
            else
            {
                M2Share.g_sMasterNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YouAreNotMasterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouAreNotMasterMsg", M2Share.g_sYouAreNotMasterMsg);
            }
            else
            {
                M2Share.g_sYouAreNotMasterMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterNotOnlineMsg", M2Share.g_sYourMasterNotOnlineMsg);
            }
            else
            {
                M2Share.g_sYourMasterNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterListNotOnlineMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListNotOnlineMsg", M2Share.g_sYourMasterListNotOnlineMsg);
            }
            else
            {
                M2Share.g_sYourMasterListNotOnlineMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterNowLocateMsg", M2Share.g_sYourMasterNowLocateMsg);
            }
            else
            {
                M2Share.g_sYourMasterNowLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterListSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListSearchLocateMsg", M2Share.g_sYourMasterListSearchLocateMsg);
            }
            else
            {
                M2Share.g_sYourMasterListSearchLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterListNowLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListNowLocateMsg", M2Share.g_sYourMasterListNowLocateMsg);
            }
            else
            {
                M2Share.g_sYourMasterListNowLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterSearchLocateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterSearchLocateMsg", M2Share.g_sYourMasterSearchLocateMsg);
            }
            else
            {
                M2Share.g_sYourMasterSearchLocateMsg = LoadString;
            }
            LoadString = ReadString("String", "YourMasterListUnMasterOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YourMasterListUnMasterOKMsg", M2Share.g_sYourMasterListUnMasterOKMsg);
            }
            else
            {
                M2Share.g_sYourMasterListUnMasterOKMsg = LoadString;
            }
            LoadString = ReadString("String", "YouAreUnMasterOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouAreUnMasterOKMsg", M2Share.g_sYouAreUnMasterOKMsg);
            }
            else
            {
                M2Share.g_sYouAreUnMasterOKMsg = LoadString;
            }
            LoadString = ReadString("String", "UnMasterLoginMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "UnMasterLoginMsg", M2Share.g_sUnMasterLoginMsg);
            }
            else
            {
                M2Share.g_sUnMasterLoginMsg = LoadString;
            }
            LoadString = ReadString("String", "NPCSayUnMasterOKMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "NPCSayUnMasterOKMsg", M2Share.g_sNPCSayUnMasterOKMsg);
            }
            else
            {
                M2Share.g_sNPCSayUnMasterOKMsg = LoadString;
            }
            LoadString = ReadString("String", "NPCSayForceUnMasterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "NPCSayForceUnMasterMsg", M2Share.g_sNPCSayForceUnMasterMsg);
            }
            else
            {
                M2Share.g_sNPCSayForceUnMasterMsg = LoadString;
            }
            LoadString = ReadString("String", "MyInfo", "");
            if (LoadString == "")
            {
                WriteString("String", "MyInfo", M2Share.g_sMyInfo);
            }
            else
            {
                M2Share.g_sMyInfo = LoadString;
            }
            LoadString = ReadString("String", "OpenedDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "OpenedDealMsg", M2Share.g_sOpenedDealMsg);
            }
            else
            {
                M2Share.g_sOpenedDealMsg = LoadString;
            }
            LoadString = ReadString("String", "SendCustMsgCanNotUseNowMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "SendCustMsgCanNotUseNowMsg", M2Share.g_sSendCustMsgCanNotUseNowMsg);
            }
            else
            {
                M2Share.g_sSendCustMsgCanNotUseNowMsg = LoadString;
            }
            LoadString = ReadString("String", "SubkMasterMsgCanNotUseNowMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "SubkMasterMsgCanNotUseNowMsg", M2Share.g_sSubkMasterMsgCanNotUseNowMsg);
            }
            else
            {
                M2Share.g_sSubkMasterMsgCanNotUseNowMsg = LoadString;
            }
            LoadString = ReadString("String", "SendOnlineCountMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "SendOnlineCountMsg", M2Share.g_sSendOnlineCountMsg);
            }
            else
            {
                M2Share.g_sSendOnlineCountMsg = LoadString;
            }
            LoadString = ReadString("String", "WeaponRepairSuccess", "");
            if (LoadString == "")
            {
                WriteString("String", "WeaponRepairSuccess", M2Share.g_sWeaponRepairSuccess);
            }
            else
            {
                M2Share.g_sWeaponRepairSuccess = LoadString;
            }
            LoadString = ReadString("String", "DefenceUpTime", "");
            if (LoadString == "")
            {
                WriteString("String", "DefenceUpTime", M2Share.g_sDefenceUpTime);
            }
            else
            {
                M2Share.g_sDefenceUpTime = LoadString;
            }
            LoadString = ReadString("String", "MagDefenceUpTime", "");
            if (LoadString == "")
            {
                WriteString("String", "MagDefenceUpTime", M2Share.g_sMagDefenceUpTime);
            }
            else
            {
                M2Share.g_sMagDefenceUpTime = LoadString;
            }
            LoadString = ReadString("String", "WinLottery1Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery1Msg", M2Share.g_sWinLottery1Msg);
            }
            else
            {
                M2Share.g_sWinLottery1Msg = LoadString;
            }
            LoadString = ReadString("String", "WinLottery2Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery2Msg", M2Share.g_sWinLottery2Msg);
            }
            else
            {
                M2Share.g_sWinLottery2Msg = LoadString;
            }
            LoadString = ReadString("String", "WinLottery3Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery3Msg", M2Share.g_sWinLottery3Msg);
            }
            else
            {
                M2Share.g_sWinLottery3Msg = LoadString;
            }
            LoadString = ReadString("String", "WinLottery4Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery4Msg", M2Share.g_sWinLottery4Msg);
            }
            else
            {
                M2Share.g_sWinLottery4Msg = LoadString;
            }
            LoadString = ReadString("String", "WinLottery5Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery5Msg", M2Share.g_sWinLottery5Msg);
            }
            else
            {
                M2Share.g_sWinLottery5Msg = LoadString;
            }
            LoadString = ReadString("String", "WinLottery6Msg", "");
            if (LoadString == "")
            {
                WriteString("String", "WinLottery6Msg", M2Share.g_sWinLottery6Msg);
            }
            else
            {
                M2Share.g_sWinLottery6Msg = LoadString;
            }
            LoadString = ReadString("String", "NotWinLotteryMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "NotWinLotteryMsg", M2Share.g_sNotWinLotteryMsg);
            }
            else
            {
                M2Share.g_sNotWinLotteryMsg = LoadString;
            }
            LoadString = ReadString("String", "WeaptonMakeLuck", "");
            if (LoadString == "")
            {
                WriteString("String", "WeaptonMakeLuck", M2Share.g_sWeaptonMakeLuck);
            }
            else
            {
                M2Share.g_sWeaptonMakeLuck = LoadString;
            }
            LoadString = ReadString("String", "WeaptonNotMakeLuck", "");
            if (LoadString == "")
            {
                WriteString("String", "WeaptonNotMakeLuck", M2Share.g_sWeaptonNotMakeLuck);
            }
            else
            {
                M2Share.g_sWeaptonNotMakeLuck = LoadString;
            }
            LoadString = ReadString("String", "TheWeaponIsCursed", "");
            if (LoadString == "")
            {
                WriteString("String", "TheWeaponIsCursed", M2Share.g_sTheWeaponIsCursed);
            }
            else
            {
                M2Share.g_sTheWeaponIsCursed = LoadString;
            }
            LoadString = ReadString("String", "CanotTakeOffItem", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotTakeOffItem", M2Share.g_sCanotTakeOffItem);
            }
            else
            {
                M2Share.g_sCanotTakeOffItem = LoadString;
            }
            LoadString = ReadString("String", "JoinGroupMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "JoinGroupMsg", M2Share.g_sJoinGroup);
            }
            else
            {
                M2Share.g_sJoinGroup = LoadString;
            }
            LoadString = ReadString("String", "TryModeCanotUseStorage", "");
            if (LoadString == "")
            {
                WriteString("String", "TryModeCanotUseStorage", M2Share.g_sTryModeCanotUseStorage);
            }
            else
            {
                M2Share.g_sTryModeCanotUseStorage = LoadString;
            }
            LoadString = ReadString("String", "CanotGetItemsMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotGetItemsMsg", M2Share.g_sCanotGetItems);
            }
            else
            {
                M2Share.g_sCanotGetItems = LoadString;
            }
            LoadString = ReadString("String", "EnableDearRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableDearRecall", M2Share.g_sEnableDearRecall);
            }
            else
            {
                M2Share.g_sEnableDearRecall = LoadString;
            }
            LoadString = ReadString("String", "DisableDearRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableDearRecall", M2Share.g_sDisableDearRecall);
            }
            else
            {
                M2Share.g_sDisableDearRecall = LoadString;
            }
            LoadString = ReadString("String", "EnableMasterRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableMasterRecall", M2Share.g_sEnableMasterRecall);
            }
            else
            {
                M2Share.g_sEnableMasterRecall = LoadString;
            }
            LoadString = ReadString("String", "DisableMasterRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableMasterRecall", M2Share.g_sDisableMasterRecall);
            }
            else
            {
                M2Share.g_sDisableMasterRecall = LoadString;
            }
            LoadString = ReadString("String", "NowCurrDateTime", "");
            if (LoadString == "")
            {
                WriteString("String", "NowCurrDateTime", M2Share.g_sNowCurrDateTime);
            }
            else
            {
                M2Share.g_sNowCurrDateTime = LoadString;
            }
            LoadString = ReadString("String", "EnableHearWhisper", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableHearWhisper", M2Share.g_sEnableHearWhisper);
            }
            else
            {
                M2Share.g_sEnableHearWhisper = LoadString;
            }
            LoadString = ReadString("String", "DisableHearWhisper", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableHearWhisper", M2Share.g_sDisableHearWhisper);
            }
            else
            {
                M2Share.g_sDisableHearWhisper = LoadString;
            }
            LoadString = ReadString("String", "EnableShoutMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableShoutMsg", M2Share.g_sEnableShoutMsg);
            }
            else
            {
                M2Share.g_sEnableShoutMsg = LoadString;
            }
            LoadString = ReadString("String", "DisableShoutMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableShoutMsg", M2Share.g_sDisableShoutMsg);
            }
            else
            {
                M2Share.g_sDisableShoutMsg = LoadString;
            }
            LoadString = ReadString("String", "EnableDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableDealMsg", M2Share.g_sEnableDealMsg);
            }
            else
            {
                M2Share.g_sEnableDealMsg = LoadString;
            }
            LoadString = ReadString("String", "DisableDealMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableDealMsg", M2Share.g_sDisableDealMsg);
            }
            else
            {
                M2Share.g_sDisableDealMsg = LoadString;
            }
            LoadString = ReadString("String", "EnableGuildChat", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableGuildChat", M2Share.g_sEnableGuildChat);
            }
            else
            {
                M2Share.g_sEnableGuildChat = LoadString;
            }
            LoadString = ReadString("String", "DisableGuildChat", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableGuildChat", M2Share.g_sDisableGuildChat);
            }
            else
            {
                M2Share.g_sDisableGuildChat = LoadString;
            }
            LoadString = ReadString("String", "EnableJoinGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableJoinGuild", M2Share.g_sEnableJoinGuild);
            }
            else
            {
                M2Share.g_sEnableJoinGuild = LoadString;
            }
            LoadString = ReadString("String", "DisableJoinGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableJoinGuild", M2Share.g_sDisableJoinGuild);
            }
            else
            {
                M2Share.g_sDisableJoinGuild = LoadString;
            }
            LoadString = ReadString("String", "EnableAuthAllyGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableAuthAllyGuild", M2Share.g_sEnableAuthAllyGuild);
            }
            else
            {
                M2Share.g_sEnableAuthAllyGuild = LoadString;
            }
            LoadString = ReadString("String", "DisableAuthAllyGuild", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableAuthAllyGuild", M2Share.g_sDisableAuthAllyGuild);
            }
            else
            {
                M2Share.g_sDisableAuthAllyGuild = LoadString;
            }
            LoadString = ReadString("String", "EnableGroupRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableGroupRecall", M2Share.g_sEnableGroupRecall);
            }
            else
            {
                M2Share.g_sEnableGroupRecall = LoadString;
            }
            LoadString = ReadString("String", "DisableGroupRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableGroupRecall", M2Share.g_sDisableGroupRecall);
            }
            else
            {
                M2Share.g_sDisableGroupRecall = LoadString;
            }
            LoadString = ReadString("String", "EnableGuildRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "EnableGuildRecall", M2Share.g_sEnableGuildRecall);
            }
            else
            {
                M2Share.g_sEnableGuildRecall = LoadString;
            }
            LoadString = ReadString("String", "DisableGuildRecall", "");
            if (LoadString == "")
            {
                WriteString("String", "DisableGuildRecall", M2Share.g_sDisableGuildRecall);
            }
            else
            {
                M2Share.g_sDisableGuildRecall = LoadString;
            }
            LoadString = ReadString("String", "PleaseInputPassword", "");
            if (LoadString == "")
            {
                WriteString("String", "PleaseInputPassword", M2Share.g_sPleaseInputPassword);
            }
            else
            {
                M2Share.g_sPleaseInputPassword = LoadString;
            }
            LoadString = ReadString("String", "TheMapDisableMove", "");
            if (LoadString == "")
            {
                WriteString("String", "TheMapDisableMove", M2Share.g_sTheMapDisableMove);
            }
            else
            {
                M2Share.g_sTheMapDisableMove = LoadString;
            }
            LoadString = ReadString("String", "TheMapNotFound", "");
            if (LoadString == "")
            {
                WriteString("String", "TheMapNotFound", M2Share.g_sTheMapNotFound);
            }
            else
            {
                M2Share.g_sTheMapNotFound = LoadString;
            }
            LoadString = ReadString("String", "YourIPaddrDenyLogon", "");
            if (LoadString == "")
            {
                WriteString("String", "YourIPaddrDenyLogon", M2Share.g_sYourIPaddrDenyLogon);
            }
            else
            {
                M2Share.g_sYourIPaddrDenyLogon = LoadString;
            }
            LoadString = ReadString("String", "YourAccountDenyLogon", "");
            if (LoadString == "")
            {
                WriteString("String", "YourAccountDenyLogon", M2Share.g_sYourAccountDenyLogon);
            }
            else
            {
                M2Share.g_sYourAccountDenyLogon = LoadString;
            }
            LoadString = ReadString("String", "YourCharNameDenyLogon", "");
            if (LoadString == "")
            {
                WriteString("String", "YourCharNameDenyLogon", M2Share.g_sYourCharNameDenyLogon);
            }
            else
            {
                M2Share.g_sYourCharNameDenyLogon = LoadString;
            }
            LoadString = ReadString("String", "CanotPickUpItem", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotPickUpItem", M2Share.g_sCanotPickUpItem);
            }
            else
            {
                M2Share.g_sCanotPickUpItem = LoadString;
            }
            LoadString = ReadString("String", "sQUERYBAGITEMS", "");
            if (LoadString == "")
            {
                WriteString("String", "sQUERYBAGITEMS", M2Share.g_sQUERYBAGITEMS);
            }
            else
            {
                M2Share.g_sQUERYBAGITEMS = LoadString;
            }
            LoadString = ReadString("String", "CanotSendmsg", "");
            if (LoadString == "")
            {
                WriteString("String", "CanotSendmsg", M2Share.g_sCanotSendmsg);
            }
            else
            {
                M2Share.g_sCanotSendmsg = LoadString;
            }
            LoadString = ReadString("String", "UserDenyWhisperMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "UserDenyWhisperMsg", M2Share.g_sUserDenyWhisperMsg);
            }
            else
            {
                M2Share.g_sUserDenyWhisperMsg = LoadString;
            }
            LoadString = ReadString("String", "UserNotOnLine", "");
            if (LoadString == "")
            {
                WriteString("String", "UserNotOnLine", M2Share.g_sUserNotOnLine);
            }
            else
            {
                M2Share.g_sUserNotOnLine = LoadString;
            }
            LoadString = ReadString("String", "RevivalRecoverMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "RevivalRecoverMsg", M2Share.g_sRevivalRecoverMsg);
            }
            else
            {
                M2Share.g_sRevivalRecoverMsg = LoadString;
            }
            LoadString = ReadString("String", "ClientVersionTooOld", "");
            if (LoadString == "")
            {
                WriteString("String", "ClientVersionTooOld", M2Share.g_sClientVersionTooOld);
            }
            else
            {
                M2Share.g_sClientVersionTooOld = LoadString;
            }
            LoadString = ReadString("String", "CastleGuildName", "");
            if (LoadString == "")
            {
                WriteString("String", "CastleGuildName", M2Share.g_sCastleGuildName);
            }
            else
            {
                M2Share.g_sCastleGuildName = LoadString;
            }
            LoadString = ReadString("String", "NoCastleGuildName", "");
            if (LoadString == "")
            {
                WriteString("String", "NoCastleGuildName", M2Share.g_sNoCastleGuildName);
            }
            else
            {
                M2Share.g_sNoCastleGuildName = LoadString;
            }
            LoadString = ReadString("String", "WarrReNewName", "");
            if (LoadString == "")
            {
                WriteString("String", "WarrReNewName", M2Share.g_sWarrReNewName);
            }
            else
            {
                M2Share.g_sWarrReNewName = LoadString;
            }
            LoadString = ReadString("String", "WizardReNewName", "");
            if (LoadString == "")
            {
                WriteString("String", "WizardReNewName", M2Share.g_sWizardReNewName);
            }
            else
            {
                M2Share.g_sWizardReNewName = LoadString;
            }
            LoadString = ReadString("String", "TaosReNewName", "");
            if (LoadString == "")
            {
                WriteString("String", "TaosReNewName", M2Share.g_sTaosReNewName);
            }
            else
            {
                M2Share.g_sTaosReNewName = LoadString;
            }
            LoadString = ReadString("String", "RankLevelName", "");
            if (LoadString == "")
            {
                WriteString("String", "RankLevelName", M2Share.g_sRankLevelName);
            }
            else
            {
                M2Share.g_sRankLevelName = LoadString.Replace("%s", "{0}");
            }
            LoadString = ReadString("String", "ManDearName", "");
            if (LoadString == "")
            {
                WriteString("String", "ManDearName", M2Share.g_sManDearName);
            }
            else
            {
                M2Share.g_sManDearName = LoadString;
            }
            LoadString = ReadString("String", "WoManDearName", "");
            if (LoadString == "")
            {
                WriteString("String", "WoManDearName", M2Share.g_sWoManDearName);
            }
            else
            {
                M2Share.g_sWoManDearName = LoadString;
            }
            LoadString = ReadString("String", "MasterName", "");
            if (LoadString == "")
            {
                WriteString("String", "MasterName", M2Share.g_sMasterName);
            }
            else
            {
                M2Share.g_sMasterName = LoadString;
            }

            LoadString = ReadString("String", "NoMasterName", "");
            if (LoadString == "")
            {
                WriteString("String", "NoMasterName", M2Share.g_sNoMasterName);
            }
            else
            {
                M2Share.g_sNoMasterName = LoadString;
            }
            LoadString = ReadString("String", "HumanShowName", "");
            if (LoadString == "")
            {
                WriteString("String", "HumanShowName", M2Share.g_sHumanShowName);
            }
            else
            {
                M2Share.g_sHumanShowName = LoadString;
            }
            LoadString = ReadString("String", "ChangePermissionMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangePermissionMsg", M2Share.g_sChangePermissionMsg);
            }
            else
            {
                M2Share.g_sChangePermissionMsg = LoadString;
            }
            LoadString = ReadString("String", "ChangeKillMonExpRateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangeKillMonExpRateMsg", M2Share.g_sChangeKillMonExpRateMsg);
            }
            else
            {
                M2Share.g_sChangeKillMonExpRateMsg = LoadString;
            }
            LoadString = ReadString("String", "ChangePowerRateMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangePowerRateMsg", M2Share.g_sChangePowerRateMsg);
            }
            else
            {
                M2Share.g_sChangePowerRateMsg = LoadString;
            }
            LoadString = ReadString("String", "ChangeMemberLevelMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangeMemberLevelMsg", M2Share.g_sChangeMemberLevelMsg);
            }
            else
            {
                M2Share.g_sChangeMemberLevelMsg = LoadString;
            }
            LoadString = ReadString("String", "ChangeMemberTypeMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ChangeMemberTypeMsg", M2Share.g_sChangeMemberTypeMsg);
            }
            else
            {
                M2Share.g_sChangeMemberTypeMsg = LoadString;
            }
            LoadString = ReadString("String", "ScriptChangeHumanHPMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ScriptChangeHumanHPMsg", M2Share.g_sScriptChangeHumanHPMsg);
            }
            else
            {
                M2Share.g_sScriptChangeHumanHPMsg = LoadString;
            }
            LoadString = ReadString("String", "ScriptChangeHumanMPMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ScriptChangeHumanMPMsg", M2Share.g_sScriptChangeHumanMPMsg);
            }
            else
            {
                M2Share.g_sScriptChangeHumanMPMsg = LoadString;
            }
            LoadString = ReadString("String", "YouCanotDisableSayMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouCanotDisableSayMsg", M2Share.g_sDisableSayMsg);
            }
            else
            {
                M2Share.g_sDisableSayMsg = LoadString;
            }
            LoadString = ReadString("String", "OnlineCountMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "OnlineCountMsg", M2Share.g_sOnlineCountMsg);
            }
            else
            {
                M2Share.g_sOnlineCountMsg = LoadString;
            }
            LoadString = ReadString("String", "TotalOnlineCountMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "TotalOnlineCountMsg", M2Share.g_sTotalOnlineCountMsg);
            }
            else
            {
                M2Share.g_sTotalOnlineCountMsg = LoadString;
            }
            LoadString = ReadString("String", "YouNeedLevelSendMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouNeedLevelSendMsg", M2Share.g_sYouNeedLevelMsg);
            }
            else
            {
                M2Share.g_sYouNeedLevelMsg = LoadString;
            }
            LoadString = ReadString("String", "ThisMapDisableSendCyCyMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "ThisMapDisableSendCyCyMsg", M2Share.g_sThisMapDisableSendCyCyMsg);
            }
            else
            {
                M2Share.g_sThisMapDisableSendCyCyMsg = LoadString;
            }
            LoadString = ReadString("String", "YouCanSendCyCyLaterMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouCanSendCyCyLaterMsg", M2Share.g_sYouCanSendCyCyLaterMsg);
            }
            else
            {
                M2Share.g_sYouCanSendCyCyLaterMsg = LoadString;
            }
            LoadString = ReadString("String", "YouIsDisableSendMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouIsDisableSendMsg", M2Share.g_sYouIsDisableSendMsg);
            }
            else
            {
                M2Share.g_sYouIsDisableSendMsg = LoadString;
            }
            LoadString = ReadString("String", "YouMurderedMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouMurderedMsg", M2Share.g_sYouMurderedMsg);
            }
            else
            {
                M2Share.g_sYouMurderedMsg = LoadString;
            }
            LoadString = ReadString("String", "YouKilledByMsg", "");
            if (LoadString == "")
            {
                WriteString("String", "YouKilledByMsg", M2Share.g_sYouKilledByMsg);
            }
            else
            {
                M2Share.g_sYouKilledByMsg = LoadString;
            }
            LoadString = ReadString("String", "YouProtectedByLawOfDefense", "");
            if (LoadString == "")
            {
                WriteString("String", "YouProtectedByLawOfDefense", M2Share.g_sYouProtectedByLawOfDefense);
            }
            else
            {
                M2Share.g_sYouProtectedByLawOfDefense = LoadString;
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