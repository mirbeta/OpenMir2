using System.IO;
using SystemModule.Common;

namespace GameSvr.Configs
{
    public class StringConfig
    {
        private readonly IniFile _conf;

        public StringConfig()
        {
            _conf = new IniFile(Path.Combine(M2Share.sConfigPath, M2Share.sStringFileName));
        }

        public void LoadString()
        {
            string LoadString;
            if (_conf.ReadString("Server", "ServerIP", "") == "")
            {
                _conf.WriteString("Server", "ServerIP", M2Share.g_Config.sServerIPaddr);
            }
            M2Share.g_Config.sServerIPaddr = _conf.ReadString("Server", "ServerIP", M2Share.g_Config.sServerIPaddr);
            if (_conf.ReadString("Server", "WebSite", "") == "")
            {
                _conf.WriteString("Server", "WebSite", M2Share.g_Config.sWebSite);
            }
            M2Share.g_Config.sWebSite = _conf.ReadString("Server", "WebSite", M2Share.g_Config.sWebSite);
            if (_conf.ReadString("Server", "BbsSite", "") == "")
            {
                _conf.WriteString("Server", "BbsSite", M2Share.g_Config.sBbsSite);
            }
            M2Share.g_Config.sBbsSite = _conf.ReadString("Server", "BbsSite", M2Share.g_Config.sBbsSite);
            if (_conf.ReadString("Server", "ClientDownload", "") == "")
            {
                _conf.WriteString("Server", "ClientDownload", M2Share.g_Config.sClientDownload);
            }
            M2Share.g_Config.sClientDownload = _conf.ReadString("Server", "ClientDownload", M2Share.g_Config.sClientDownload);
            if (_conf.ReadString("Server", "QQ", "") == "")
            {
                _conf.WriteString("Server", "QQ", M2Share.g_Config.sQQ);
            }
            M2Share.g_Config.sQQ = _conf.ReadString("Server", "QQ", M2Share.g_Config.sQQ);
            if (_conf.ReadString("Server", "Phone", "") == "")
            {
                _conf.WriteString("Server", "Phone", M2Share.g_Config.sPhone);
            }
            M2Share.g_Config.sPhone = _conf.ReadString("Server", "Phone", M2Share.g_Config.sPhone);
            if (_conf.ReadString("Server", "BankAccount0", "") == "")
            {
                _conf.WriteString("Server", "BankAccount0", M2Share.g_Config.sBankAccount0);
            }
            M2Share.g_Config.sBankAccount0 = _conf.ReadString("Server", "BankAccount0", M2Share.g_Config.sBankAccount0);
            if (_conf.ReadString("Server", "BankAccount1", "") == "")
            {
                _conf.WriteString("Server", "BankAccount1", M2Share.g_Config.sBankAccount1);
            }
            M2Share.g_Config.sBankAccount1 = _conf.ReadString("Server", "BankAccount1", M2Share.g_Config.sBankAccount1);
            if (_conf.ReadString("Server", "BankAccount2", "") == "")
            {
                _conf.WriteString("Server", "BankAccount2", M2Share.g_Config.sBankAccount2);
            }
            M2Share.g_Config.sBankAccount2 = _conf.ReadString("Server", "BankAccount2", M2Share.g_Config.sBankAccount2);
            if (_conf.ReadString("Server", "BankAccount3", "") == "")
            {
                _conf.WriteString("Server", "BankAccount3", M2Share.g_Config.sBankAccount3);
            }
            M2Share.g_Config.sBankAccount3 = _conf.ReadString("Server", "BankAccount3", M2Share.g_Config.sBankAccount3);
            if (_conf.ReadString("Server", "BankAccount4", "") == "")
            {
                _conf.WriteString("Server", "BankAccount4", M2Share.g_Config.sBankAccount4);
            }
            M2Share.g_Config.sBankAccount4 = _conf.ReadString("Server", "BankAccount4", M2Share.g_Config.sBankAccount4);
            if (_conf.ReadString("Server", "BankAccount5", "") == "")
            {
                _conf.WriteString("Server", "BankAccount5", M2Share.g_Config.sBankAccount5);
            }
            M2Share.g_Config.sBankAccount5 = _conf.ReadString("Server", "BankAccount5", M2Share.g_Config.sBankAccount5);
            if (_conf.ReadString("Server", "BankAccount6", "") == "")
            {
                _conf.WriteString("Server", "BankAccount6", M2Share.g_Config.sBankAccount6);
            }
            M2Share.g_Config.sBankAccount6 = _conf.ReadString("Server", "BankAccount6", M2Share.g_Config.sBankAccount6);
            if (_conf.ReadString("Server", "BankAccount7", "") == "")
            {
                _conf.WriteString("Server", "BankAccount7", M2Share.g_Config.sBankAccount7);
            }
            M2Share.g_Config.sBankAccount7 = _conf.ReadString("Server", "BankAccount7", M2Share.g_Config.sBankAccount7);
            if (_conf.ReadString("Server", "BankAccount8", "") == "")
            {
                _conf.WriteString("Server", "BankAccount8", M2Share.g_Config.sBankAccount8);
            }
            M2Share.g_Config.sBankAccount8 = _conf.ReadString("Server", "BankAccount8", M2Share.g_Config.sBankAccount8);
            if (_conf.ReadString("Server", "BankAccount9", "") == "")
            {
                _conf.WriteString("Server", "BankAccount9", M2Share.g_Config.sBankAccount9);
            }
            M2Share.g_Config.sBankAccount9 = _conf.ReadString("Server", "BankAccount9", M2Share.g_Config.sBankAccount9);
            if (_conf.ReadString("Guild", "GuildNotice", "") == "")
            {
                _conf.WriteString("Guild", "GuildNotice", M2Share.g_Config.sGuildNotice);
            }
            M2Share.g_Config.sGuildNotice = _conf.ReadString("Guild", "GuildNotice", M2Share.g_Config.sGuildNotice);
            if (_conf.ReadString("Guild", "GuildWar", "") == "")
            {
                _conf.WriteString("Guild", "GuildWar", M2Share.g_Config.sGuildWar);
            }
            M2Share.g_Config.sGuildWar = _conf.ReadString("Guild", "GuildWar", M2Share.g_Config.sGuildWar);
            if (_conf.ReadString("Guild", "GuildAll", "") == "")
            {
                _conf.WriteString("Guild", "GuildAll", M2Share.g_Config.sGuildAll);
            }
            M2Share.g_Config.sGuildAll = _conf.ReadString("Guild", "GuildAll", M2Share.g_Config.sGuildAll);
            if (_conf.ReadString("Guild", "GuildMember", "") == "")
            {
                _conf.WriteString("Guild", "GuildMember", M2Share.g_Config.sGuildMember);
            }
            M2Share.g_Config.sGuildMember =_conf.ReadString("Guild", "GuildMember", M2Share.g_Config.sGuildMember);
            if (_conf.ReadString("Guild", "GuildMemberRank", "") == "")
            {
                _conf.WriteString("Guild", "GuildMemberRank", M2Share.g_Config.sGuildMemberRank);
            }
            M2Share.g_Config.sGuildMemberRank =_conf.ReadString("Guild", "GuildMemberRank", M2Share.g_Config.sGuildMemberRank);
            if (_conf.ReadString("Guild", "GuildChief", "") == "")
            {
                _conf.WriteString("Guild", "GuildChief", M2Share.g_Config.sGuildChief);
            }
            M2Share.g_Config.sGuildChief = _conf.ReadString("Guild", "GuildChief", M2Share.g_Config.sGuildChief);
            LoadString = _conf.ReadString("String", "ClientSoftVersionError", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ClientSoftVersionError", M2Share.sClientSoftVersionError);
            }
            else
            {
                M2Share.sClientSoftVersionError = LoadString;
            }
            LoadString = _conf.ReadString("String", "DownLoadNewClientSoft", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DownLoadNewClientSoft", M2Share.sDownLoadNewClientSoft);
            }
            else
            {
                M2Share.sDownLoadNewClientSoft = LoadString;
            }
            LoadString = _conf.ReadString("String", "ForceDisConnect", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ForceDisConnect", M2Share.sForceDisConnect);
            }
            else
            {
                M2Share.sForceDisConnect = LoadString;
            }
            LoadString = _conf.ReadString("String", "ClientSoftVersionTooOld", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ClientSoftVersionTooOld", M2Share.sClientSoftVersionTooOld);
            }
            else
            {
                M2Share.sClientSoftVersionTooOld = LoadString;
            }
            LoadString = _conf.ReadString("String", "DownLoadAndUseNewClient", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DownLoadAndUseNewClient", M2Share.sDownLoadAndUseNewClient);
            }
            else
            {
                M2Share.sDownLoadAndUseNewClient = LoadString;
            }
            LoadString = _conf.ReadString("String", "OnlineUserFull", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "OnlineUserFull", M2Share.sOnlineUserFull);
            }
            else
            {
                M2Share.sOnlineUserFull = LoadString;
            }
            LoadString = _conf.ReadString("String", "YouNowIsTryPlayMode", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YouNowIsTryPlayMode", M2Share.sYouNowIsTryPlayMode);
            }
            else
            {
                M2Share.sYouNowIsTryPlayMode = LoadString;
            }
            LoadString = _conf.ReadString("String", "NowIsFreePlayMode", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "NowIsFreePlayMode", M2Share.g_sNowIsFreePlayMode);
            }
            else
            {
                M2Share.g_sNowIsFreePlayMode = LoadString;
            }
            LoadString = _conf.ReadString("String", "AttackModeOfAll", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "AttackModeOfAll", M2Share.sAttackModeOfAll);
            }
            else
            {
                M2Share.sAttackModeOfAll = LoadString;
            }
            LoadString = _conf.ReadString("String", "AttackModeOfPeaceful", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "AttackModeOfPeaceful", M2Share.sAttackModeOfPeaceful);
            }
            else
            {
                M2Share.sAttackModeOfPeaceful = LoadString;
            }
            LoadString = _conf.ReadString("String", "AttackModeOfGroup", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "AttackModeOfGroup", M2Share.sAttackModeOfGroup);
            }
            else
            {
                M2Share.sAttackModeOfGroup = LoadString;
            }
            LoadString = _conf.ReadString("String", "AttackModeOfGuild", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "AttackModeOfGuild", M2Share.sAttackModeOfGuild);
            }
            else
            {
                M2Share.sAttackModeOfGuild = LoadString;
            }
            LoadString = _conf.ReadString("String", "AttackModeOfRedWhite", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "AttackModeOfRedWhite", M2Share.sAttackModeOfRedWhite);
            }
            else
            {
                M2Share.sAttackModeOfRedWhite = LoadString;
            }
            LoadString = _conf.ReadString("String", "StartChangeAttackModeHelp", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "StartChangeAttackModeHelp", M2Share.sStartChangeAttackModeHelp);
            }
            else
            {
                M2Share.sStartChangeAttackModeHelp = LoadString;
            }
            LoadString = _conf.ReadString("String", "StartNoticeMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "StartNoticeMsg", M2Share.sStartNoticeMsg);
            }
            else
            {
                M2Share.sStartNoticeMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ThrustingOn", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ThrustingOn", M2Share.sThrustingOn);
            }
            else
            {
                M2Share.sThrustingOn = LoadString;
            }
            LoadString = _conf.ReadString("String", "ThrustingOff", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ThrustingOff", M2Share.sThrustingOff);
            }
            else
            {
                M2Share.sThrustingOff = LoadString;
            }
            LoadString = _conf.ReadString("String", "HalfMoonOn", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "HalfMoonOn", M2Share.sHalfMoonOn);
            }
            else
            {
                M2Share.sHalfMoonOn = LoadString;
            }
            LoadString = _conf.ReadString("String", "HalfMoonOff", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "HalfMoonOff", M2Share.sHalfMoonOff);
            }
            else
            {
                M2Share.sHalfMoonOff = LoadString;
            }
            M2Share.sCrsHitOn = LoadConfigString("String", "CrsHitOn", M2Share.sCrsHitOn);
            M2Share.sCrsHitOff = LoadConfigString("String", "CrsHitOff", M2Share.sCrsHitOff);
            M2Share.sTwinHitOn = LoadConfigString("String", "TwinHitOn", M2Share.sTwinHitOn);
            M2Share.sTwinHitOff = LoadConfigString("String", "TwinHitOff", M2Share.sTwinHitOff);
            LoadString = _conf.ReadString("String", "FireSpiritsSummoned", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "FireSpiritsSummoned", M2Share.sFireSpiritsSummoned);
            }
            else
            {
                M2Share.sFireSpiritsSummoned = LoadString;
            }
            LoadString = _conf.ReadString("String", "FireSpiritsFail", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "FireSpiritsFail", M2Share.sFireSpiritsFail);
            }
            else
            {
                M2Share.sFireSpiritsFail = LoadString;
            }
            LoadString = _conf.ReadString("String", "SpiritsGone", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "SpiritsGone", M2Share.sSpiritsGone);
            }
            else
            {
                M2Share.sSpiritsGone = LoadString;
            }
            LoadString = _conf.ReadString("String", "MateDoTooweak", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MateDoTooweak", M2Share.sMateDoTooweak);
            }
            else
            {
                M2Share.sMateDoTooweak = LoadString;
            }
            LoadString = _conf.ReadString("String", "TheWeaponBroke", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "TheWeaponBroke", M2Share.g_sTheWeaponBroke);
            }
            else
            {
                M2Share.g_sTheWeaponBroke = LoadString;
            }
            LoadString = _conf.ReadString("String", "TheWeaponRefineSuccessfull", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "TheWeaponRefineSuccessfull", M2Share.sTheWeaponRefineSuccessfull);
            }
            else
            {
                M2Share.sTheWeaponRefineSuccessfull = LoadString;
            }
            LoadString = _conf.ReadString("String", "YouPoisoned", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YouPoisoned", M2Share.sYouPoisoned);
            }
            else
            {
                M2Share.sYouPoisoned = LoadString;
            }
            LoadString = _conf.ReadString("String", "PetRest", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "PetRest", M2Share.sPetRest);
            }
            else
            {
                M2Share.sPetRest = LoadString;
            }
            LoadString = _conf.ReadString("String", "PetAttack", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "PetAttack", M2Share.sPetAttack);
            }
            else
            {
                M2Share.sPetAttack = LoadString;
            }
            LoadString = _conf.ReadString("String", "WearNotOfWoMan", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WearNotOfWoMan", M2Share.sWearNotOfWoMan);
            }
            else
            {
                M2Share.sWearNotOfWoMan = LoadString;
            }
            LoadString = _conf.ReadString("String", "WearNotOfMan", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WearNotOfMan", M2Share.sWearNotOfMan);
            }
            else
            {
                M2Share.sWearNotOfMan = LoadString;
            }
            LoadString = _conf.ReadString("String", "HandWeightNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "HandWeightNot", M2Share.sHandWeightNot);
            }
            else
            {
                M2Share.sHandWeightNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "WearWeightNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WearWeightNot", M2Share.sWearWeightNot);
            }
            else
            {
                M2Share.sWearWeightNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "ItemIsNotThisAccount", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ItemIsNotThisAccount", M2Share.g_sItemIsNotThisAccount);
            }
            else
            {
                M2Share.g_sItemIsNotThisAccount = LoadString;
            }
            LoadString = _conf.ReadString("String", "ItemIsNotThisIPaddr", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ItemIsNotThisIPaddr", M2Share.g_sItemIsNotThisIPaddr);
            }
            else
            {
                M2Share.g_sItemIsNotThisIPaddr = LoadString;
            }
            LoadString = _conf.ReadString("String", "ItemIsNotThisCharName", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ItemIsNotThisCharName", M2Share.g_sItemIsNotThisCharName);
            }
            else
            {
                M2Share.g_sItemIsNotThisCharName = LoadString;
            }
            LoadString = _conf.ReadString("String", "LevelNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "LevelNot", M2Share.g_sLevelNot);
            }
            else
            {
                M2Share.g_sLevelNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "JobOrLevelNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "JobOrLevelNot", M2Share.g_sJobOrLevelNot);
            }
            else
            {
                M2Share.g_sJobOrLevelNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "JobOrDCNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "JobOrDCNot", M2Share.g_sJobOrDCNot);
            }
            else
            {
                M2Share.g_sJobOrDCNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "JobOrMCNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "JobOrMCNot", M2Share.g_sJobOrMCNot);
            }
            else
            {
                M2Share.g_sJobOrMCNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "JobOrSCNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "JobOrSCNot", M2Share.g_sJobOrSCNot);
            }
            else
            {
                M2Share.g_sJobOrSCNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "DCNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DCNot", M2Share.g_sDCNot);
            }
            else
            {
                M2Share.g_sDCNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "MCNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MCNot", M2Share.g_sMCNot);
            }
            else
            {
                M2Share.g_sMCNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "SCNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "SCNot", M2Share.g_sSCNot);
            }
            else
            {
                M2Share.g_sSCNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "CreditPointNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CreditPointNot", M2Share.g_sCreditPointNot);
            }
            else
            {
                M2Share.g_sCreditPointNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "ReNewLevelNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ReNewLevelNot", M2Share.g_sReNewLevelNot);
            }
            else
            {
                M2Share.g_sReNewLevelNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "GuildNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "GuildNot", M2Share.g_sGuildNot);
            }
            else
            {
                M2Share.g_sGuildNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "GuildMasterNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "GuildMasterNot", M2Share.g_sGuildMasterNot);
            }
            else
            {
                M2Share.g_sGuildMasterNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "SabukHumanNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "SabukHumanNot", M2Share.g_sSabukHumanNot);
            }
            else
            {
                M2Share.g_sSabukHumanNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "SabukMasterManNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "SabukMasterManNot", M2Share.g_sSabukMasterManNot);
            }
            else
            {
                M2Share.g_sSabukMasterManNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "MemberNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MemberNot", M2Share.g_sMemberNot);
            }
            else
            {
                M2Share.g_sMemberNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "MemberTypeNot", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MemberTypeNot", M2Share.g_sMemberTypeNot);
            }
            else
            {
                M2Share.g_sMemberTypeNot = LoadString;
            }
            LoadString = _conf.ReadString("String", "CanottWearIt", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CanottWearIt", M2Share.g_sCanottWearIt);
            }
            else
            {
                M2Share.g_sCanottWearIt = LoadString;
            }
            LoadString = _conf.ReadString("String", "CanotUseDrugOnThisMap", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CanotUseDrugOnThisMap", M2Share.sCanotUseDrugOnThisMap);
            }
            else
            {
                M2Share.sCanotUseDrugOnThisMap = LoadString;
            }
            LoadString = _conf.ReadString("String", "GameMasterMode", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "GameMasterMode", M2Share.sGameMasterMode);
            }
            else
            {
                M2Share.sGameMasterMode = LoadString;
            }
            LoadString = _conf.ReadString("String", "ReleaseGameMasterMode", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ReleaseGameMasterMode", M2Share.sReleaseGameMasterMode);
            }
            else
            {
                M2Share.sReleaseGameMasterMode = LoadString;
            }
            LoadString = _conf.ReadString("String", "ObserverMode", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ObserverMode", M2Share.sObserverMode);
            }
            else
            {
                M2Share.sObserverMode = LoadString;
            }
            LoadString = _conf.ReadString("String", "ReleaseObserverMode", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ReleaseObserverMode", M2Share.g_sReleaseObserverMode);
            }
            else
            {
                M2Share.g_sReleaseObserverMode = LoadString;
            }
            LoadString = _conf.ReadString("String", "SupermanMode", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "SupermanMode", M2Share.sSupermanMode);
            }
            else
            {
                M2Share.sSupermanMode = LoadString;
            }
            LoadString = _conf.ReadString("String", "ReleaseSupermanMode", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ReleaseSupermanMode", M2Share.sReleaseSupermanMode);
            }
            else
            {
                M2Share.sReleaseSupermanMode = LoadString;
            }
            LoadString = _conf.ReadString("String", "YouFoundNothing", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YouFoundNothing", M2Share.sYouFoundNothing);
            }
            else
            {
                M2Share.sYouFoundNothing = LoadString;
            }
            LoadString = _conf.ReadString("String", "LineNoticePreFix", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "LineNoticePreFix", M2Share.g_Config.sLineNoticePreFix);
            }
            else
            {
                M2Share.g_Config.sLineNoticePreFix = LoadString;
            }
            LoadString = _conf.ReadString("String", "SysMsgPreFix", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "SysMsgPreFix", M2Share.g_Config.sSysMsgPreFix);
            }
            else
            {
                M2Share.g_Config.sSysMsgPreFix = LoadString;
            }
            LoadString = _conf.ReadString("String", "GuildMsgPreFix", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "GuildMsgPreFix", M2Share.g_Config.sGuildMsgPreFix);
            }
            else
            {
                M2Share.g_Config.sGuildMsgPreFix = LoadString;
            }
            LoadString = _conf.ReadString("String", "GroupMsgPreFix", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "GroupMsgPreFix", M2Share.g_Config.sGroupMsgPreFix);
            }
            else
            {
                M2Share.g_Config.sGroupMsgPreFix = LoadString;
            }
            LoadString = _conf.ReadString("String", "HintMsgPreFix", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "HintMsgPreFix", M2Share.g_Config.sHintMsgPreFix);
            }
            else
            {
                M2Share.g_Config.sHintMsgPreFix = LoadString;
            }
            LoadString = _conf.ReadString("String", "GMRedMsgpreFix", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "GMRedMsgpreFix", M2Share.g_Config.sGMRedMsgpreFix);
            }
            else
            {
                M2Share.g_Config.sGMRedMsgpreFix = LoadString;
            }
            LoadString = _conf.ReadString("String", "MonSayMsgpreFix", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MonSayMsgpreFix", M2Share.g_Config.sMonSayMsgpreFix);
            }
            else
            {
                M2Share.g_Config.sMonSayMsgpreFix = LoadString;
            }
            LoadString = _conf.ReadString("String", "CustMsgpreFix", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CustMsgpreFix", M2Share.g_Config.sCustMsgpreFix);
            }
            else
            {
                M2Share.g_Config.sCustMsgpreFix = LoadString;
            }
            LoadString = _conf.ReadString("String", "CastleMsgpreFix", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CastleMsgpreFix", M2Share.g_Config.sCastleMsgpreFix);
            }
            else
            {
                M2Share.g_Config.sCastleMsgpreFix = LoadString;
            }
            LoadString = _conf.ReadString("String", "NoPasswordLockSystemMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "NoPasswordLockSystemMsg", M2Share.g_sNoPasswordLockSystemMsg);
            }
            else
            {
                M2Share.g_sNoPasswordLockSystemMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "AlreadySetPassword", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "AlreadySetPassword", M2Share.g_sAlreadySetPasswordMsg);
            }
            else
            {
                M2Share.g_sAlreadySetPasswordMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ReSetPassword", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ReSetPassword", M2Share.g_sReSetPasswordMsg);
            }
            else
            {
                M2Share.g_sReSetPasswordMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "PasswordOverLong", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "PasswordOverLong", M2Share.g_sPasswordOverLongMsg);
            }
            else
            {
                M2Share.g_sPasswordOverLongMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ReSetPasswordOK", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ReSetPasswordOK", M2Share.g_sReSetPasswordOKMsg);
            }
            else
            {
                M2Share.g_sReSetPasswordOKMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ReSetPasswordNotMatch", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ReSetPasswordNotMatch", M2Share.g_sReSetPasswordNotMatchMsg);
            }
            else
            {
                M2Share.g_sReSetPasswordNotMatchMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "PleaseInputUnLockPassword", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "PleaseInputUnLockPassword", M2Share.g_sPleaseInputUnLockPasswordMsg);
            }
            else
            {
                M2Share.g_sPleaseInputUnLockPasswordMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "StorageUnLockOK", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "StorageUnLockOK", M2Share.g_sStorageUnLockOKMsg);
            }
            else
            {
                M2Share.g_sStorageUnLockOKMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "StorageAlreadyUnLock", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "StorageAlreadyUnLock", M2Share.g_sStorageAlreadyUnLockMsg);
            }
            else
            {
                M2Share.g_sStorageAlreadyUnLockMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "StorageNoPassword", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "StorageNoPassword", M2Share.g_sStorageNoPasswordMsg);
            }
            else
            {
                M2Share.g_sStorageNoPasswordMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "UnLockPasswordFail", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "UnLockPasswordFail", M2Share.g_sUnLockPasswordFailMsg);
            }
            else
            {
                M2Share.g_sUnLockPasswordFailMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "LockStorageSuccess", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "LockStorageSuccess", M2Share.g_sLockStorageSuccessMsg);
            }
            else
            {
                M2Share.g_sLockStorageSuccessMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "StoragePasswordClearMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "StoragePasswordClearMsg", M2Share.g_sStoragePasswordClearMsg);
            }
            else
            {
                M2Share.g_sStoragePasswordClearMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "PleaseUnloadStoragePasswordMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "PleaseUnloadStoragePasswordMsg", M2Share.g_sPleaseUnloadStoragePasswordMsg);
            }
            else
            {
                M2Share.g_sPleaseUnloadStoragePasswordMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "StorageAlreadyLock", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "StorageAlreadyLock", M2Share.g_sStorageAlreadyLockMsg);
            }
            else
            {
                M2Share.g_sStorageAlreadyLockMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "StoragePasswordLocked", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "StoragePasswordLocked", M2Share.g_sStoragePasswordLockedMsg);
            }
            else
            {
                M2Share.g_sStoragePasswordLockedMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "StorageSetPassword", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "StorageSetPassword", M2Share.g_sSetPasswordMsg);
            }
            else
            {
                M2Share.g_sSetPasswordMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "PleaseInputOldPassword", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "PleaseInputOldPassword", M2Share.g_sPleaseInputOldPasswordMsg);
            }
            else
            {
                M2Share.g_sPleaseInputOldPasswordMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "PasswordIsClearMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "PasswordIsClearMsg", M2Share.g_sOldPasswordIsClearMsg);
            }
            else
            {
                M2Share.g_sOldPasswordIsClearMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "NoPasswordSet", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "NoPasswordSet", M2Share.g_sNoPasswordSetMsg);
            }
            else
            {
                M2Share.g_sNoPasswordSetMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "OldPasswordIncorrect", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "OldPasswordIncorrect", M2Share.g_sOldPasswordIncorrectMsg);
            }
            else
            {
                M2Share.g_sOldPasswordIncorrectMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "StorageIsLocked", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "StorageIsLocked", M2Share.g_sStorageIsLockedMsg);
            }
            else
            {
                M2Share.g_sStorageIsLockedMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "PleaseTryDealLaterMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "PleaseTryDealLaterMsg", M2Share.g_sPleaseTryDealLaterMsg);
            }
            else
            {
                M2Share.g_sPleaseTryDealLaterMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "DealItemsDenyGetBackMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DealItemsDenyGetBackMsg", M2Share.g_sDealItemsDenyGetBackMsg);
            }
            else
            {
                M2Share.g_sDealItemsDenyGetBackMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "DisableDealItemsMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DisableDealItemsMsg", M2Share.g_sDisableDealItemsMsg);
            }
            else
            {
                M2Share.g_sDisableDealItemsMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "CanotTryDealMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CanotTryDealMsg", M2Share.g_sCanotTryDealMsg);
            }
            else
            {
                M2Share.g_sCanotTryDealMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "DealActionCancelMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DealActionCancelMsg", M2Share.g_sDealActionCancelMsg);
            }
            else
            {
                M2Share.g_sDealActionCancelMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "PoseDisableDealMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "PoseDisableDealMsg", M2Share.g_sPoseDisableDealMsg);
            }
            else
            {
                M2Share.g_sPoseDisableDealMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "DealSuccessMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DealSuccessMsg", M2Share.g_sDealSuccessMsg);
            }
            else
            {
                M2Share.g_sDealSuccessMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "DealOKTooFast", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DealOKTooFast", M2Share.g_sDealOKTooFast);
            }
            else
            {
                M2Share.g_sDealOKTooFast = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourBagSizeTooSmall", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourBagSizeTooSmall", M2Share.g_sYourBagSizeTooSmall);
            }
            else
            {
                M2Share.g_sYourBagSizeTooSmall = LoadString;
            }
            LoadString = _conf.ReadString("String", "DealHumanBagSizeTooSmall", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DealHumanBagSizeTooSmall", M2Share.g_sDealHumanBagSizeTooSmall);
            }
            else
            {
                M2Share.g_sDealHumanBagSizeTooSmall = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourGoldLargeThenLimit", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourGoldLargeThenLimit", M2Share.g_sYourGoldLargeThenLimit);
            }
            else
            {
                M2Share.g_sYourGoldLargeThenLimit = LoadString;
            }
            LoadString = _conf.ReadString("String", "DealHumanGoldLargeThenLimit", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DealHumanGoldLargeThenLimit", M2Share.g_sDealHumanGoldLargeThenLimit);
            }
            else
            {
                M2Share.g_sDealHumanGoldLargeThenLimit = LoadString;
            }
            LoadString = _conf.ReadString("String", "YouDealOKMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YouDealOKMsg", M2Share.g_sYouDealOKMsg);
            }
            else
            {
                M2Share.g_sYouDealOKMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "PoseDealOKMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "PoseDealOKMsg", M2Share.g_sPoseDealOKMsg);
            }
            else
            {
                M2Share.g_sPoseDealOKMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "KickClientUserMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "KickClientUserMsg", M2Share.g_sKickClientUserMsg);
            }
            else
            {
                M2Share.g_sKickClientUserMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ActionIsLockedMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ActionIsLockedMsg", M2Share.g_sActionIsLockedMsg);
            }
            else
            {
                M2Share.g_sActionIsLockedMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "PasswordNotSetMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "PasswordNotSetMsg", M2Share.g_sPasswordNotSetMsg);
            }
            else
            {
                M2Share.g_sPasswordNotSetMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "NotPasswordProtectMode", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "NotPasswordProtectMode", M2Share.g_sNotPasswordProtectMode);
            }
            else
            {
                M2Share.g_sNotPasswordProtectMode = LoadString;
            }
            LoadString = _conf.ReadString("String", "CanotDropGoldMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CanotDropGoldMsg", M2Share.g_sCanotDropGoldMsg);
            }
            else
            {
                M2Share.g_sCanotDropGoldMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "CanotDropInSafeZoneMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CanotDropInSafeZoneMsg", M2Share.g_sCanotDropInSafeZoneMsg);
            }
            else
            {
                M2Share.g_sCanotDropInSafeZoneMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "CanotDropItemMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CanotDropItemMsg", M2Share.g_sCanotDropItemMsg);
            }
            else
            {
                M2Share.g_sCanotDropItemMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "CanotDropItemMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CanotDropItemMsg", M2Share.g_sCanotDropItemMsg);
            }
            else
            {
                M2Share.g_sCanotDropItemMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "CanotUseItemMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CanotUseItemMsg", M2Share.g_sCanotUseItemMsg);
            }
            else
            {
                M2Share.g_sCanotUseItemMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "StartMarryManMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "StartMarryManMsg", M2Share.g_sStartMarryManMsg);
            }
            else
            {
                M2Share.g_sStartMarryManMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "StartMarryWoManMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "StartMarryWoManMsg", M2Share.g_sStartMarryWoManMsg);
            }
            else
            {
                M2Share.g_sStartMarryWoManMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "StartMarryManAskQuestionMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "StartMarryManAskQuestionMsg", M2Share.g_sStartMarryManAskQuestionMsg);
            }
            else
            {
                M2Share.g_sStartMarryManAskQuestionMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "StartMarryWoManAskQuestionMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "StartMarryWoManAskQuestionMsg", M2Share.g_sStartMarryWoManAskQuestionMsg);
            }
            else
            {
                M2Share.g_sStartMarryWoManAskQuestionMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "MarryManAnswerQuestionMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MarryManAnswerQuestionMsg", M2Share.g_sMarryManAnswerQuestionMsg);
            }
            else
            {
                M2Share.g_sMarryManAnswerQuestionMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "MarryManAskQuestionMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MarryManAskQuestionMsg", M2Share.g_sMarryManAskQuestionMsg);
            }
            else
            {
                M2Share.g_sMarryManAskQuestionMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "MarryWoManAnswerQuestionMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MarryWoManAnswerQuestionMsg", M2Share.g_sMarryWoManAnswerQuestionMsg);
            }
            else
            {
                M2Share.g_sMarryWoManAnswerQuestionMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "MarryWoManGetMarryMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MarryWoManGetMarryMsg", M2Share.g_sMarryWoManGetMarryMsg);
            }
            else
            {
                M2Share.g_sMarryWoManGetMarryMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "MarryWoManDenyMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MarryWoManDenyMsg", M2Share.g_sMarryWoManDenyMsg);
            }
            else
            {
                M2Share.g_sMarryWoManDenyMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "MarryWoManCancelMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MarryWoManCancelMsg", M2Share.g_sMarryWoManCancelMsg);
            }
            else
            {
                M2Share.g_sMarryWoManCancelMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ForceUnMarryManLoginMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ForceUnMarryManLoginMsg", M2Share.g_sfUnMarryManLoginMsg);
            }
            else
            {
                M2Share.g_sfUnMarryManLoginMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ForceUnMarryWoManLoginMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ForceUnMarryWoManLoginMsg", M2Share.g_sfUnMarryWoManLoginMsg);
            }
            else
            {
                M2Share.g_sfUnMarryWoManLoginMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ManLoginDearOnlineSelfMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ManLoginDearOnlineSelfMsg", M2Share.g_sManLoginDearOnlineSelfMsg);
            }
            else
            {
                M2Share.g_sManLoginDearOnlineSelfMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ManLoginDearOnlineDearMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ManLoginDearOnlineDearMsg", M2Share.g_sManLoginDearOnlineDearMsg);
            }
            else
            {
                M2Share.g_sManLoginDearOnlineDearMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "WoManLoginDearOnlineSelfMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WoManLoginDearOnlineSelfMsg", M2Share.g_sWoManLoginDearOnlineSelfMsg);
            }
            else
            {
                M2Share.g_sWoManLoginDearOnlineSelfMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "WoManLoginDearOnlineDearMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WoManLoginDearOnlineDearMsg", M2Share.g_sWoManLoginDearOnlineDearMsg);
            }
            else
            {
                M2Share.g_sWoManLoginDearOnlineDearMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ManLoginDearNotOnlineMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ManLoginDearNotOnlineMsg", M2Share.g_sManLoginDearNotOnlineMsg);
            }
            else
            {
                M2Share.g_sManLoginDearNotOnlineMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "WoManLoginDearNotOnlineMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WoManLoginDearNotOnlineMsg", M2Share.g_sWoManLoginDearNotOnlineMsg);
            }
            else
            {
                M2Share.g_sWoManLoginDearNotOnlineMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ManLongOutDearOnlineMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ManLongOutDearOnlineMsg", M2Share.g_sManLongOutDearOnlineMsg);
            }
            else
            {
                M2Share.g_sManLongOutDearOnlineMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "WoManLongOutDearOnlineMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WoManLongOutDearOnlineMsg", M2Share.g_sWoManLongOutDearOnlineMsg);
            }
            else
            {
                M2Share.g_sWoManLongOutDearOnlineMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YouAreNotMarryedMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YouAreNotMarryedMsg", M2Share.g_sYouAreNotMarryedMsg);
            }
            else
            {
                M2Share.g_sYouAreNotMarryedMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourWifeNotOnlineMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourWifeNotOnlineMsg", M2Share.g_sYourWifeNotOnlineMsg);
            }
            else
            {
                M2Share.g_sYourWifeNotOnlineMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourHusbandNotOnlineMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourHusbandNotOnlineMsg", M2Share.g_sYourHusbandNotOnlineMsg);
            }
            else
            {
                M2Share.g_sYourHusbandNotOnlineMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourWifeNowLocateMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourWifeNowLocateMsg", M2Share.g_sYourWifeNowLocateMsg);
            }
            else
            {
                M2Share.g_sYourWifeNowLocateMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourHusbandSearchLocateMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourHusbandSearchLocateMsg", M2Share.g_sYourHusbandSearchLocateMsg);
            }
            else
            {
                M2Share.g_sYourHusbandSearchLocateMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourHusbandNowLocateMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourHusbandNowLocateMsg", M2Share.g_sYourHusbandNowLocateMsg);
            }
            else
            {
                M2Share.g_sYourHusbandNowLocateMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourWifeSearchLocateMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourWifeSearchLocateMsg", M2Share.g_sYourWifeSearchLocateMsg);
            }
            else
            {
                M2Share.g_sYourWifeSearchLocateMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "FUnMasterLoginMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "FUnMasterLoginMsg", M2Share.g_sfUnMasterLoginMsg);
            }
            else
            {
                M2Share.g_sfUnMasterLoginMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "UnMasterListLoginMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "UnMasterListLoginMsg", M2Share.g_sfUnMasterListLoginMsg);
            }
            else
            {
                M2Share.g_sfUnMasterListLoginMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "MasterListOnlineSelfMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MasterListOnlineSelfMsg", M2Share.g_sMasterListOnlineSelfMsg);
            }
            else
            {
                M2Share.g_sMasterListOnlineSelfMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "MasterListOnlineMasterMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MasterListOnlineMasterMsg", M2Share.g_sMasterListOnlineMasterMsg);
            }
            else
            {
                M2Share.g_sMasterListOnlineMasterMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "MasterOnlineSelfMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MasterOnlineSelfMsg", M2Share.g_sMasterOnlineSelfMsg);
            }
            else
            {
                M2Share.g_sMasterOnlineSelfMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "MasterOnlineMasterListMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MasterOnlineMasterListMsg", M2Share.g_sMasterOnlineMasterListMsg);
            }
            else
            {
                M2Share.g_sMasterOnlineMasterListMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "MasterLongOutMasterListOnlineMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MasterLongOutMasterListOnlineMsg", M2Share.g_sMasterLongOutMasterListOnlineMsg);
            }
            else
            {
                M2Share.g_sMasterLongOutMasterListOnlineMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "MasterListLongOutMasterOnlineMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MasterListLongOutMasterOnlineMsg", M2Share.g_sMasterListLongOutMasterOnlineMsg);
            }
            else
            {
                M2Share.g_sMasterListLongOutMasterOnlineMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "MasterListNotOnlineMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MasterListNotOnlineMsg", M2Share.g_sMasterListNotOnlineMsg);
            }
            else
            {
                M2Share.g_sMasterListNotOnlineMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "MasterNotOnlineMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MasterNotOnlineMsg", M2Share.g_sMasterNotOnlineMsg);
            }
            else
            {
                M2Share.g_sMasterNotOnlineMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YouAreNotMasterMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YouAreNotMasterMsg", M2Share.g_sYouAreNotMasterMsg);
            }
            else
            {
                M2Share.g_sYouAreNotMasterMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourMasterNotOnlineMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourMasterNotOnlineMsg", M2Share.g_sYourMasterNotOnlineMsg);
            }
            else
            {
                M2Share.g_sYourMasterNotOnlineMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourMasterListNotOnlineMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourMasterListNotOnlineMsg", M2Share.g_sYourMasterListNotOnlineMsg);
            }
            else
            {
                M2Share.g_sYourMasterListNotOnlineMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourMasterNowLocateMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourMasterNowLocateMsg", M2Share.g_sYourMasterNowLocateMsg);
            }
            else
            {
                M2Share.g_sYourMasterNowLocateMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourMasterListSearchLocateMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourMasterListSearchLocateMsg", M2Share.g_sYourMasterListSearchLocateMsg);
            }
            else
            {
                M2Share.g_sYourMasterListSearchLocateMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourMasterListNowLocateMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourMasterListNowLocateMsg", M2Share.g_sYourMasterListNowLocateMsg);
            }
            else
            {
                M2Share.g_sYourMasterListNowLocateMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourMasterSearchLocateMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourMasterSearchLocateMsg", M2Share.g_sYourMasterSearchLocateMsg);
            }
            else
            {
                M2Share.g_sYourMasterSearchLocateMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourMasterListUnMasterOKMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourMasterListUnMasterOKMsg", M2Share.g_sYourMasterListUnMasterOKMsg);
            }
            else
            {
                M2Share.g_sYourMasterListUnMasterOKMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YouAreUnMasterOKMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YouAreUnMasterOKMsg", M2Share.g_sYouAreUnMasterOKMsg);
            }
            else
            {
                M2Share.g_sYouAreUnMasterOKMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "UnMasterLoginMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "UnMasterLoginMsg", M2Share.g_sUnMasterLoginMsg);
            }
            else
            {
                M2Share.g_sUnMasterLoginMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "NPCSayUnMasterOKMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "NPCSayUnMasterOKMsg", M2Share.g_sNPCSayUnMasterOKMsg);
            }
            else
            {
                M2Share.g_sNPCSayUnMasterOKMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "NPCSayForceUnMasterMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "NPCSayForceUnMasterMsg", M2Share.g_sNPCSayForceUnMasterMsg);
            }
            else
            {
                M2Share.g_sNPCSayForceUnMasterMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "MyInfo", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MyInfo", M2Share.g_sMyInfo);
            }
            else
            {
                M2Share.g_sMyInfo = LoadString;
            }
            LoadString = _conf.ReadString("String", "OpenedDealMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "OpenedDealMsg", M2Share.g_sOpenedDealMsg);
            }
            else
            {
                M2Share.g_sOpenedDealMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "SendCustMsgCanNotUseNowMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "SendCustMsgCanNotUseNowMsg", M2Share.g_sSendCustMsgCanNotUseNowMsg);
            }
            else
            {
                M2Share.g_sSendCustMsgCanNotUseNowMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "SubkMasterMsgCanNotUseNowMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "SubkMasterMsgCanNotUseNowMsg", M2Share.g_sSubkMasterMsgCanNotUseNowMsg);
            }
            else
            {
                M2Share.g_sSubkMasterMsgCanNotUseNowMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "SendOnlineCountMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "SendOnlineCountMsg", M2Share.g_sSendOnlineCountMsg);
            }
            else
            {
                M2Share.g_sSendOnlineCountMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "WeaponRepairSuccess", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WeaponRepairSuccess", M2Share.g_sWeaponRepairSuccess);
            }
            else
            {
                M2Share.g_sWeaponRepairSuccess = LoadString;
            }
            LoadString = _conf.ReadString("String", "DefenceUpTime", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DefenceUpTime", M2Share.g_sDefenceUpTime);
            }
            else
            {
                M2Share.g_sDefenceUpTime = LoadString;
            }
            LoadString = _conf.ReadString("String", "MagDefenceUpTime", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MagDefenceUpTime", M2Share.g_sMagDefenceUpTime);
            }
            else
            {
                M2Share.g_sMagDefenceUpTime = LoadString;
            }
            LoadString = _conf.ReadString("String", "WinLottery1Msg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WinLottery1Msg", M2Share.g_sWinLottery1Msg);
            }
            else
            {
                M2Share.g_sWinLottery1Msg = LoadString;
            }
            LoadString = _conf.ReadString("String", "WinLottery2Msg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WinLottery2Msg", M2Share.g_sWinLottery2Msg);
            }
            else
            {
                M2Share.g_sWinLottery2Msg = LoadString;
            }
            LoadString = _conf.ReadString("String", "WinLottery3Msg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WinLottery3Msg", M2Share.g_sWinLottery3Msg);
            }
            else
            {
                M2Share.g_sWinLottery3Msg = LoadString;
            }
            LoadString = _conf.ReadString("String", "WinLottery4Msg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WinLottery4Msg", M2Share.g_sWinLottery4Msg);
            }
            else
            {
                M2Share.g_sWinLottery4Msg = LoadString;
            }
            LoadString = _conf.ReadString("String", "WinLottery5Msg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WinLottery5Msg", M2Share.g_sWinLottery5Msg);
            }
            else
            {
                M2Share.g_sWinLottery5Msg = LoadString;
            }
            LoadString = _conf.ReadString("String", "WinLottery6Msg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WinLottery6Msg", M2Share.g_sWinLottery6Msg);
            }
            else
            {
                M2Share.g_sWinLottery6Msg = LoadString;
            }
            LoadString = _conf.ReadString("String", "NotWinLotteryMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "NotWinLotteryMsg", M2Share.g_sNotWinLotteryMsg);
            }
            else
            {
                M2Share.g_sNotWinLotteryMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "WeaptonMakeLuck", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WeaptonMakeLuck", M2Share.g_sWeaptonMakeLuck);
            }
            else
            {
                M2Share.g_sWeaptonMakeLuck = LoadString;
            }
            LoadString = _conf.ReadString("String", "WeaptonNotMakeLuck", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WeaptonNotMakeLuck", M2Share.g_sWeaptonNotMakeLuck);
            }
            else
            {
                M2Share.g_sWeaptonNotMakeLuck = LoadString;
            }
            LoadString = _conf.ReadString("String", "TheWeaponIsCursed", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "TheWeaponIsCursed", M2Share.g_sTheWeaponIsCursed);
            }
            else
            {
                M2Share.g_sTheWeaponIsCursed = LoadString;
            }
            LoadString = _conf.ReadString("String", "CanotTakeOffItem", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CanotTakeOffItem", M2Share.g_sCanotTakeOffItem);
            }
            else
            {
                M2Share.g_sCanotTakeOffItem = LoadString;
            }
            LoadString = _conf.ReadString("String", "JoinGroupMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "JoinGroupMsg", M2Share.g_sJoinGroup);
            }
            else
            {
                M2Share.g_sJoinGroup = LoadString;
            }
            LoadString = _conf.ReadString("String", "TryModeCanotUseStorage", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "TryModeCanotUseStorage", M2Share.g_sTryModeCanotUseStorage);
            }
            else
            {
                M2Share.g_sTryModeCanotUseStorage = LoadString;
            }
            LoadString = _conf.ReadString("String", "CanotGetItemsMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CanotGetItemsMsg", M2Share.g_sCanotGetItems);
            }
            else
            {
                M2Share.g_sCanotGetItems = LoadString;
            }
            LoadString = _conf.ReadString("String", "EnableDearRecall", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "EnableDearRecall", M2Share.g_sEnableDearRecall);
            }
            else
            {
                M2Share.g_sEnableDearRecall = LoadString;
            }
            LoadString = _conf.ReadString("String", "DisableDearRecall", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DisableDearRecall", M2Share.g_sDisableDearRecall);
            }
            else
            {
                M2Share.g_sDisableDearRecall = LoadString;
            }
            LoadString = _conf.ReadString("String", "EnableMasterRecall", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "EnableMasterRecall", M2Share.g_sEnableMasterRecall);
            }
            else
            {
                M2Share.g_sEnableMasterRecall = LoadString;
            }
            LoadString = _conf.ReadString("String", "DisableMasterRecall", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DisableMasterRecall", M2Share.g_sDisableMasterRecall);
            }
            else
            {
                M2Share.g_sDisableMasterRecall = LoadString;
            }
            LoadString = _conf.ReadString("String", "NowCurrDateTime", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "NowCurrDateTime", M2Share.g_sNowCurrDateTime);
            }
            else
            {
                M2Share.g_sNowCurrDateTime = LoadString;
            }
            LoadString = _conf.ReadString("String", "EnableHearWhisper", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "EnableHearWhisper", M2Share.g_sEnableHearWhisper);
            }
            else
            {
                M2Share.g_sEnableHearWhisper = LoadString;
            }
            LoadString = _conf.ReadString("String", "DisableHearWhisper", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DisableHearWhisper", M2Share.g_sDisableHearWhisper);
            }
            else
            {
                M2Share.g_sDisableHearWhisper = LoadString;
            }
            LoadString = _conf.ReadString("String", "EnableShoutMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "EnableShoutMsg", M2Share.g_sEnableShoutMsg);
            }
            else
            {
                M2Share.g_sEnableShoutMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "DisableShoutMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DisableShoutMsg", M2Share.g_sDisableShoutMsg);
            }
            else
            {
                M2Share.g_sDisableShoutMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "EnableDealMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "EnableDealMsg", M2Share.g_sEnableDealMsg);
            }
            else
            {
                M2Share.g_sEnableDealMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "DisableDealMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DisableDealMsg", M2Share.g_sDisableDealMsg);
            }
            else
            {
                M2Share.g_sDisableDealMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "EnableGuildChat", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "EnableGuildChat", M2Share.g_sEnableGuildChat);
            }
            else
            {
                M2Share.g_sEnableGuildChat = LoadString;
            }
            LoadString = _conf.ReadString("String", "DisableGuildChat", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DisableGuildChat", M2Share.g_sDisableGuildChat);
            }
            else
            {
                M2Share.g_sDisableGuildChat = LoadString;
            }
            LoadString = _conf.ReadString("String", "EnableJoinGuild", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "EnableJoinGuild", M2Share.g_sEnableJoinGuild);
            }
            else
            {
                M2Share.g_sEnableJoinGuild = LoadString;
            }
            LoadString = _conf.ReadString("String", "DisableJoinGuild", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DisableJoinGuild", M2Share.g_sDisableJoinGuild);
            }
            else
            {
                M2Share.g_sDisableJoinGuild = LoadString;
            }
            LoadString = _conf.ReadString("String", "EnableAuthAllyGuild", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "EnableAuthAllyGuild", M2Share.g_sEnableAuthAllyGuild);
            }
            else
            {
                M2Share.g_sEnableAuthAllyGuild = LoadString;
            }
            LoadString = _conf.ReadString("String", "DisableAuthAllyGuild", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DisableAuthAllyGuild", M2Share.g_sDisableAuthAllyGuild);
            }
            else
            {
                M2Share.g_sDisableAuthAllyGuild = LoadString;
            }
            LoadString = _conf.ReadString("String", "EnableGroupRecall", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "EnableGroupRecall", M2Share.g_sEnableGroupRecall);
            }
            else
            {
                M2Share.g_sEnableGroupRecall = LoadString;
            }
            LoadString = _conf.ReadString("String", "DisableGroupRecall", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DisableGroupRecall", M2Share.g_sDisableGroupRecall);
            }
            else
            {
                M2Share.g_sDisableGroupRecall = LoadString;
            }
            LoadString = _conf.ReadString("String", "EnableGuildRecall", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "EnableGuildRecall", M2Share.g_sEnableGuildRecall);
            }
            else
            {
                M2Share.g_sEnableGuildRecall = LoadString;
            }
            LoadString = _conf.ReadString("String", "DisableGuildRecall", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "DisableGuildRecall", M2Share.g_sDisableGuildRecall);
            }
            else
            {
                M2Share.g_sDisableGuildRecall = LoadString;
            }
            LoadString = _conf.ReadString("String", "PleaseInputPassword", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "PleaseInputPassword", M2Share.g_sPleaseInputPassword);
            }
            else
            {
                M2Share.g_sPleaseInputPassword = LoadString;
            }
            LoadString = _conf.ReadString("String", "TheMapDisableMove", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "TheMapDisableMove", M2Share.g_sTheMapDisableMove);
            }
            else
            {
                M2Share.g_sTheMapDisableMove = LoadString;
            }
            LoadString = _conf.ReadString("String", "TheMapNotFound", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "TheMapNotFound", M2Share.g_sTheMapNotFound);
            }
            else
            {
                M2Share.g_sTheMapNotFound = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourIPaddrDenyLogon", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourIPaddrDenyLogon", M2Share.g_sYourIPaddrDenyLogon);
            }
            else
            {
                M2Share.g_sYourIPaddrDenyLogon = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourAccountDenyLogon", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourAccountDenyLogon", M2Share.g_sYourAccountDenyLogon);
            }
            else
            {
                M2Share.g_sYourAccountDenyLogon = LoadString;
            }
            LoadString = _conf.ReadString("String", "YourCharNameDenyLogon", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YourCharNameDenyLogon", M2Share.g_sYourCharNameDenyLogon);
            }
            else
            {
                M2Share.g_sYourCharNameDenyLogon = LoadString;
            }
            LoadString = _conf.ReadString("String", "CanotPickUpItem", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CanotPickUpItem", M2Share.g_sCanotPickUpItem);
            }
            else
            {
                M2Share.g_sCanotPickUpItem = LoadString;
            }
            LoadString = _conf.ReadString("String", "sQUERYBAGITEMS", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "sQUERYBAGITEMS", M2Share.g_sQUERYBAGITEMS);
            }
            else
            {
                M2Share.g_sQUERYBAGITEMS = LoadString;
            }
            LoadString = _conf.ReadString("String", "CanotSendmsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CanotSendmsg", M2Share.g_sCanotSendmsg);
            }
            else
            {
                M2Share.g_sCanotSendmsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "UserDenyWhisperMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "UserDenyWhisperMsg", M2Share.g_sUserDenyWhisperMsg);
            }
            else
            {
                M2Share.g_sUserDenyWhisperMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "UserNotOnLine", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "UserNotOnLine", M2Share.g_sUserNotOnLine);
            }
            else
            {
                M2Share.g_sUserNotOnLine = LoadString;
            }
            LoadString = _conf.ReadString("String", "RevivalRecoverMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "RevivalRecoverMsg", M2Share.g_sRevivalRecoverMsg);
            }
            else
            {
                M2Share.g_sRevivalRecoverMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ClientVersionTooOld", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ClientVersionTooOld", M2Share.g_sClientVersionTooOld);
            }
            else
            {
                M2Share.g_sClientVersionTooOld = LoadString;
            }
            LoadString = _conf.ReadString("String", "CastleGuildName", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "CastleGuildName", M2Share.g_sCastleGuildName);
            }
            else
            {
                M2Share.g_sCastleGuildName = LoadString;
            }
            LoadString = _conf.ReadString("String", "NoCastleGuildName", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "NoCastleGuildName", M2Share.g_sNoCastleGuildName);
            }
            else
            {
                M2Share.g_sNoCastleGuildName = LoadString;
            }
            LoadString = _conf.ReadString("String", "WarrReNewName", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WarrReNewName", M2Share.g_sWarrReNewName);
            }
            else
            {
                M2Share.g_sWarrReNewName = LoadString;
            }
            LoadString = _conf.ReadString("String", "WizardReNewName", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WizardReNewName", M2Share.g_sWizardReNewName);
            }
            else
            {
                M2Share.g_sWizardReNewName = LoadString;
            }
            LoadString = _conf.ReadString("String", "TaosReNewName", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "TaosReNewName", M2Share.g_sTaosReNewName);
            }
            else
            {
                M2Share.g_sTaosReNewName = LoadString;
            }
            LoadString = _conf.ReadString("String", "RankLevelName", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "RankLevelName", M2Share.g_sRankLevelName);
            }
            else
            {
                M2Share.g_sRankLevelName = LoadString.Replace("%s", "{0}");
            }
            LoadString = _conf.ReadString("String", "ManDearName", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ManDearName", M2Share.g_sManDearName);
            }
            else
            {
                M2Share.g_sManDearName = LoadString;
            }
            LoadString = _conf.ReadString("String", "WoManDearName", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "WoManDearName", M2Share.g_sWoManDearName);
            }
            else
            {
                M2Share.g_sWoManDearName = LoadString;
            }
            LoadString = _conf.ReadString("String", "MasterName", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "MasterName", M2Share.g_sMasterName);
            }
            else
            {
                M2Share.g_sMasterName = LoadString;
            }
            
            LoadString = _conf.ReadString("String", "NoMasterName", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "NoMasterName", M2Share.g_sNoMasterName);
            }
            else
            {
                M2Share.g_sNoMasterName = LoadString;
            }
            LoadString = _conf.ReadString("String", "HumanShowName", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "HumanShowName", M2Share.g_sHumanShowName);
            }
            else
            {
                M2Share.g_sHumanShowName = LoadString;
            }
            LoadString = _conf.ReadString("String", "ChangePermissionMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ChangePermissionMsg", M2Share.g_sChangePermissionMsg);
            }
            else
            {
                M2Share.g_sChangePermissionMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ChangeKillMonExpRateMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ChangeKillMonExpRateMsg", M2Share.g_sChangeKillMonExpRateMsg);
            }
            else
            {
                M2Share.g_sChangeKillMonExpRateMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ChangePowerRateMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ChangePowerRateMsg", M2Share.g_sChangePowerRateMsg);
            }
            else
            {
                M2Share.g_sChangePowerRateMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ChangeMemberLevelMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ChangeMemberLevelMsg", M2Share.g_sChangeMemberLevelMsg);
            }
            else
            {
                M2Share.g_sChangeMemberLevelMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ChangeMemberTypeMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ChangeMemberTypeMsg", M2Share.g_sChangeMemberTypeMsg);
            }
            else
            {
                M2Share.g_sChangeMemberTypeMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ScriptChangeHumanHPMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ScriptChangeHumanHPMsg", M2Share.g_sScriptChangeHumanHPMsg);
            }
            else
            {
                M2Share.g_sScriptChangeHumanHPMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ScriptChangeHumanMPMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ScriptChangeHumanMPMsg", M2Share.g_sScriptChangeHumanMPMsg);
            }
            else
            {
                M2Share.g_sScriptChangeHumanMPMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YouCanotDisableSayMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YouCanotDisableSayMsg", M2Share.g_sDisableSayMsg);
            }
            else
            {
                M2Share.g_sDisableSayMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "OnlineCountMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "OnlineCountMsg", M2Share.g_sOnlineCountMsg);
            }
            else
            {
                M2Share.g_sOnlineCountMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "TotalOnlineCountMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "TotalOnlineCountMsg", M2Share.g_sTotalOnlineCountMsg);
            }
            else
            {
                M2Share.g_sTotalOnlineCountMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YouNeedLevelSendMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YouNeedLevelSendMsg", M2Share.g_sYouNeedLevelMsg);
            }
            else
            {
                M2Share.g_sYouNeedLevelMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "ThisMapDisableSendCyCyMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "ThisMapDisableSendCyCyMsg", M2Share.g_sThisMapDisableSendCyCyMsg);
            }
            else
            {
                M2Share.g_sThisMapDisableSendCyCyMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YouCanSendCyCyLaterMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YouCanSendCyCyLaterMsg", M2Share.g_sYouCanSendCyCyLaterMsg);
            }
            else
            {
                M2Share.g_sYouCanSendCyCyLaterMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YouIsDisableSendMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YouIsDisableSendMsg", M2Share.g_sYouIsDisableSendMsg);
            }
            else
            {
                M2Share.g_sYouIsDisableSendMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YouMurderedMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YouMurderedMsg", M2Share.g_sYouMurderedMsg);
            }
            else
            {
                M2Share.g_sYouMurderedMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YouKilledByMsg", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YouKilledByMsg", M2Share.g_sYouKilledByMsg);
            }
            else
            {
                M2Share.g_sYouKilledByMsg = LoadString;
            }
            LoadString = _conf.ReadString("String", "YouProtectedByLawOfDefense", "");
            if (LoadString == "")
            {
                _conf.WriteString("String", "YouProtectedByLawOfDefense", M2Share.g_sYouProtectedByLawOfDefense);
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
            sString = _conf.ReadString(sSection, sIdent, "");
            if (sString == "")
            {
                _conf.WriteString(sSection, sIdent, sDefault);
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
