using SystemModule.Common;

namespace GameSvr.Configs
{
    public class StringConfig
    {
        public IniFile StringConf;

        public StringConfig()
        {
            StringConf = new IniFile(M2Share.sStringFileName);
        }

        public void LoadString()
        {
            string LoadString;
            LoadString = StringConf.ReadString("String", "ClientSoftVersionError", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ClientSoftVersionError", M2Share.sClientSoftVersionError);
            }
            else
            {
                M2Share.sClientSoftVersionError = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DownLoadNewClientSoft", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DownLoadNewClientSoft", M2Share.sDownLoadNewClientSoft);
            }
            else
            {
                M2Share.sDownLoadNewClientSoft = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ForceDisConnect", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ForceDisConnect", M2Share.sForceDisConnect);
            }
            else
            {
                M2Share.sForceDisConnect = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ClientSoftVersionTooOld", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ClientSoftVersionTooOld", M2Share.sClientSoftVersionTooOld);
            }
            else
            {
                M2Share.sClientSoftVersionTooOld = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DownLoadAndUseNewClient", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DownLoadAndUseNewClient", M2Share.sDownLoadAndUseNewClient);
            }
            else
            {
                M2Share.sDownLoadAndUseNewClient = LoadString;
            }
            LoadString = StringConf.ReadString("String", "OnlineUserFull", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "OnlineUserFull", M2Share.sOnlineUserFull);
            }
            else
            {
                M2Share.sOnlineUserFull = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YouNowIsTryPlayMode", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YouNowIsTryPlayMode", M2Share.sYouNowIsTryPlayMode);
            }
            else
            {
                M2Share.sYouNowIsTryPlayMode = LoadString;
            }
            LoadString = StringConf.ReadString("String", "NowIsFreePlayMode", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "NowIsFreePlayMode", M2Share.g_sNowIsFreePlayMode);
            }
            else
            {
                M2Share.g_sNowIsFreePlayMode = LoadString;
            }
            LoadString = StringConf.ReadString("String", "AttackModeOfAll", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "AttackModeOfAll", M2Share.sAttackModeOfAll);
            }
            else
            {
                M2Share.sAttackModeOfAll = LoadString;
            }
            LoadString = StringConf.ReadString("String", "AttackModeOfPeaceful", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "AttackModeOfPeaceful", M2Share.sAttackModeOfPeaceful);
            }
            else
            {
                M2Share.sAttackModeOfPeaceful = LoadString;
            }
            LoadString = StringConf.ReadString("String", "AttackModeOfGroup", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "AttackModeOfGroup", M2Share.sAttackModeOfGroup);
            }
            else
            {
                M2Share.sAttackModeOfGroup = LoadString;
            }
            LoadString = StringConf.ReadString("String", "AttackModeOfGuild", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "AttackModeOfGuild", M2Share.sAttackModeOfGuild);
            }
            else
            {
                M2Share.sAttackModeOfGuild = LoadString;
            }
            LoadString = StringConf.ReadString("String", "AttackModeOfRedWhite", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "AttackModeOfRedWhite", M2Share.sAttackModeOfRedWhite);
            }
            else
            {
                M2Share.sAttackModeOfRedWhite = LoadString;
            }
            LoadString = StringConf.ReadString("String", "StartChangeAttackModeHelp", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "StartChangeAttackModeHelp", M2Share.sStartChangeAttackModeHelp);
            }
            else
            {
                M2Share.sStartChangeAttackModeHelp = LoadString;
            }
            LoadString = StringConf.ReadString("String", "StartNoticeMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "StartNoticeMsg", M2Share.sStartNoticeMsg);
            }
            else
            {
                M2Share.sStartNoticeMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ThrustingOn", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ThrustingOn", M2Share.sThrustingOn);
            }
            else
            {
                M2Share.sThrustingOn = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ThrustingOff", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ThrustingOff", M2Share.sThrustingOff);
            }
            else
            {
                M2Share.sThrustingOff = LoadString;
            }
            LoadString = StringConf.ReadString("String", "HalfMoonOn", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "HalfMoonOn", M2Share.sHalfMoonOn);
            }
            else
            {
                M2Share.sHalfMoonOn = LoadString;
            }
            LoadString = StringConf.ReadString("String", "HalfMoonOff", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "HalfMoonOff", M2Share.sHalfMoonOff);
            }
            else
            {
                M2Share.sHalfMoonOff = LoadString;
            }
            M2Share.sCrsHitOn = LoadConfigString("String", "CrsHitOn", M2Share.sCrsHitOn);
            M2Share.sCrsHitOff = LoadConfigString("String", "CrsHitOff", M2Share.sCrsHitOff);
            M2Share.sTwinHitOn = LoadConfigString("String", "TwinHitOn", M2Share.sTwinHitOn);
            M2Share.sTwinHitOff = LoadConfigString("String", "TwinHitOff", M2Share.sTwinHitOff);
            LoadString = StringConf.ReadString("String", "FireSpiritsSummoned", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "FireSpiritsSummoned", M2Share.sFireSpiritsSummoned);
            }
            else
            {
                M2Share.sFireSpiritsSummoned = LoadString;
            }
            LoadString = StringConf.ReadString("String", "FireSpiritsFail", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "FireSpiritsFail", M2Share.sFireSpiritsFail);
            }
            else
            {
                M2Share.sFireSpiritsFail = LoadString;
            }
            LoadString = StringConf.ReadString("String", "SpiritsGone", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "SpiritsGone", M2Share.sSpiritsGone);
            }
            else
            {
                M2Share.sSpiritsGone = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MateDoTooweak", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MateDoTooweak", M2Share.sMateDoTooweak);
            }
            else
            {
                M2Share.sMateDoTooweak = LoadString;
            }
            LoadString = StringConf.ReadString("String", "TheWeaponBroke", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "TheWeaponBroke", M2Share.g_sTheWeaponBroke);
            }
            else
            {
                M2Share.g_sTheWeaponBroke = LoadString;
            }
            LoadString = StringConf.ReadString("String", "TheWeaponRefineSuccessfull", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "TheWeaponRefineSuccessfull", M2Share.sTheWeaponRefineSuccessfull);
            }
            else
            {
                M2Share.sTheWeaponRefineSuccessfull = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YouPoisoned", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YouPoisoned", M2Share.sYouPoisoned);
            }
            else
            {
                M2Share.sYouPoisoned = LoadString;
            }
            LoadString = StringConf.ReadString("String", "PetRest", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "PetRest", M2Share.sPetRest);
            }
            else
            {
                M2Share.sPetRest = LoadString;
            }
            LoadString = StringConf.ReadString("String", "PetAttack", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "PetAttack", M2Share.sPetAttack);
            }
            else
            {
                M2Share.sPetAttack = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WearNotOfWoMan", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WearNotOfWoMan", M2Share.sWearNotOfWoMan);
            }
            else
            {
                M2Share.sWearNotOfWoMan = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WearNotOfMan", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WearNotOfMan", M2Share.sWearNotOfMan);
            }
            else
            {
                M2Share.sWearNotOfMan = LoadString;
            }
            LoadString = StringConf.ReadString("String", "HandWeightNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "HandWeightNot", M2Share.sHandWeightNot);
            }
            else
            {
                M2Share.sHandWeightNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WearWeightNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WearWeightNot", M2Share.sWearWeightNot);
            }
            else
            {
                M2Share.sWearWeightNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ItemIsNotThisAccount", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ItemIsNotThisAccount", M2Share.g_sItemIsNotThisAccount);
            }
            else
            {
                M2Share.g_sItemIsNotThisAccount = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ItemIsNotThisIPaddr", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ItemIsNotThisIPaddr", M2Share.g_sItemIsNotThisIPaddr);
            }
            else
            {
                M2Share.g_sItemIsNotThisIPaddr = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ItemIsNotThisCharName", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ItemIsNotThisCharName", M2Share.g_sItemIsNotThisCharName);
            }
            else
            {
                M2Share.g_sItemIsNotThisCharName = LoadString;
            }
            LoadString = StringConf.ReadString("String", "LevelNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "LevelNot", M2Share.g_sLevelNot);
            }
            else
            {
                M2Share.g_sLevelNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "JobOrLevelNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "JobOrLevelNot", M2Share.g_sJobOrLevelNot);
            }
            else
            {
                M2Share.g_sJobOrLevelNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "JobOrDCNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "JobOrDCNot", M2Share.g_sJobOrDCNot);
            }
            else
            {
                M2Share.g_sJobOrDCNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "JobOrMCNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "JobOrMCNot", M2Share.g_sJobOrMCNot);
            }
            else
            {
                M2Share.g_sJobOrMCNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "JobOrSCNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "JobOrSCNot", M2Share.g_sJobOrSCNot);
            }
            else
            {
                M2Share.g_sJobOrSCNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DCNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DCNot", M2Share.g_sDCNot);
            }
            else
            {
                M2Share.g_sDCNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MCNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MCNot", M2Share.g_sMCNot);
            }
            else
            {
                M2Share.g_sMCNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "SCNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "SCNot", M2Share.g_sSCNot);
            }
            else
            {
                M2Share.g_sSCNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CreditPointNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CreditPointNot", M2Share.g_sCreditPointNot);
            }
            else
            {
                M2Share.g_sCreditPointNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ReNewLevelNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ReNewLevelNot", M2Share.g_sReNewLevelNot);
            }
            else
            {
                M2Share.g_sReNewLevelNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "GuildNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "GuildNot", M2Share.g_sGuildNot);
            }
            else
            {
                M2Share.g_sGuildNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "GuildMasterNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "GuildMasterNot", M2Share.g_sGuildMasterNot);
            }
            else
            {
                M2Share.g_sGuildMasterNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "SabukHumanNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "SabukHumanNot", M2Share.g_sSabukHumanNot);
            }
            else
            {
                M2Share.g_sSabukHumanNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "SabukMasterManNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "SabukMasterManNot", M2Share.g_sSabukMasterManNot);
            }
            else
            {
                M2Share.g_sSabukMasterManNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MemberNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MemberNot", M2Share.g_sMemberNot);
            }
            else
            {
                M2Share.g_sMemberNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MemberTypeNot", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MemberTypeNot", M2Share.g_sMemberTypeNot);
            }
            else
            {
                M2Share.g_sMemberTypeNot = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CanottWearIt", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CanottWearIt", M2Share.g_sCanottWearIt);
            }
            else
            {
                M2Share.g_sCanottWearIt = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CanotUseDrugOnThisMap", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CanotUseDrugOnThisMap", M2Share.sCanotUseDrugOnThisMap);
            }
            else
            {
                M2Share.sCanotUseDrugOnThisMap = LoadString;
            }
            LoadString = StringConf.ReadString("String", "GameMasterMode", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "GameMasterMode", M2Share.sGameMasterMode);
            }
            else
            {
                M2Share.sGameMasterMode = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ReleaseGameMasterMode", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ReleaseGameMasterMode", M2Share.sReleaseGameMasterMode);
            }
            else
            {
                M2Share.sReleaseGameMasterMode = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ObserverMode", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ObserverMode", M2Share.sObserverMode);
            }
            else
            {
                M2Share.sObserverMode = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ReleaseObserverMode", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ReleaseObserverMode", M2Share.g_sReleaseObserverMode);
            }
            else
            {
                M2Share.g_sReleaseObserverMode = LoadString;
            }
            LoadString = StringConf.ReadString("String", "SupermanMode", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "SupermanMode", M2Share.sSupermanMode);
            }
            else
            {
                M2Share.sSupermanMode = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ReleaseSupermanMode", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ReleaseSupermanMode", M2Share.sReleaseSupermanMode);
            }
            else
            {
                M2Share.sReleaseSupermanMode = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YouFoundNothing", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YouFoundNothing", M2Share.sYouFoundNothing);
            }
            else
            {
                M2Share.sYouFoundNothing = LoadString;
            }
            LoadString = StringConf.ReadString("String", "LineNoticePreFix", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "LineNoticePreFix", M2Share.g_Config.sLineNoticePreFix);
            }
            else
            {
                M2Share.g_Config.sLineNoticePreFix = LoadString;
            }
            LoadString = StringConf.ReadString("String", "SysMsgPreFix", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "SysMsgPreFix", M2Share.g_Config.sSysMsgPreFix);
            }
            else
            {
                M2Share.g_Config.sSysMsgPreFix = LoadString;
            }
            LoadString = StringConf.ReadString("String", "GuildMsgPreFix", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "GuildMsgPreFix", M2Share.g_Config.sGuildMsgPreFix);
            }
            else
            {
                M2Share.g_Config.sGuildMsgPreFix = LoadString;
            }
            LoadString = StringConf.ReadString("String", "GroupMsgPreFix", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "GroupMsgPreFix", M2Share.g_Config.sGroupMsgPreFix);
            }
            else
            {
                M2Share.g_Config.sGroupMsgPreFix = LoadString;
            }
            LoadString = StringConf.ReadString("String", "HintMsgPreFix", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "HintMsgPreFix", M2Share.g_Config.sHintMsgPreFix);
            }
            else
            {
                M2Share.g_Config.sHintMsgPreFix = LoadString;
            }
            LoadString = StringConf.ReadString("String", "GMRedMsgpreFix", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "GMRedMsgpreFix", M2Share.g_Config.sGMRedMsgpreFix);
            }
            else
            {
                M2Share.g_Config.sGMRedMsgpreFix = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MonSayMsgpreFix", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MonSayMsgpreFix", M2Share.g_Config.sMonSayMsgpreFix);
            }
            else
            {
                M2Share.g_Config.sMonSayMsgpreFix = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CustMsgpreFix", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CustMsgpreFix", M2Share.g_Config.sCustMsgpreFix);
            }
            else
            {
                M2Share.g_Config.sCustMsgpreFix = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CastleMsgpreFix", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CastleMsgpreFix", M2Share.g_Config.sCastleMsgpreFix);
            }
            else
            {
                M2Share.g_Config.sCastleMsgpreFix = LoadString;
            }
            LoadString = StringConf.ReadString("String", "NoPasswordLockSystemMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "NoPasswordLockSystemMsg", M2Share.g_sNoPasswordLockSystemMsg);
            }
            else
            {
                M2Share.g_sNoPasswordLockSystemMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "AlreadySetPassword", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "AlreadySetPassword", M2Share.g_sAlreadySetPasswordMsg);
            }
            else
            {
                M2Share.g_sAlreadySetPasswordMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ReSetPassword", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ReSetPassword", M2Share.g_sReSetPasswordMsg);
            }
            else
            {
                M2Share.g_sReSetPasswordMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "PasswordOverLong", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "PasswordOverLong", M2Share.g_sPasswordOverLongMsg);
            }
            else
            {
                M2Share.g_sPasswordOverLongMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ReSetPasswordOK", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ReSetPasswordOK", M2Share.g_sReSetPasswordOKMsg);
            }
            else
            {
                M2Share.g_sReSetPasswordOKMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ReSetPasswordNotMatch", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ReSetPasswordNotMatch", M2Share.g_sReSetPasswordNotMatchMsg);
            }
            else
            {
                M2Share.g_sReSetPasswordNotMatchMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "PleaseInputUnLockPassword", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "PleaseInputUnLockPassword", M2Share.g_sPleaseInputUnLockPasswordMsg);
            }
            else
            {
                M2Share.g_sPleaseInputUnLockPasswordMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "StorageUnLockOK", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "StorageUnLockOK", M2Share.g_sStorageUnLockOKMsg);
            }
            else
            {
                M2Share.g_sStorageUnLockOKMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "StorageAlreadyUnLock", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "StorageAlreadyUnLock", M2Share.g_sStorageAlreadyUnLockMsg);
            }
            else
            {
                M2Share.g_sStorageAlreadyUnLockMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "StorageNoPassword", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "StorageNoPassword", M2Share.g_sStorageNoPasswordMsg);
            }
            else
            {
                M2Share.g_sStorageNoPasswordMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "UnLockPasswordFail", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "UnLockPasswordFail", M2Share.g_sUnLockPasswordFailMsg);
            }
            else
            {
                M2Share.g_sUnLockPasswordFailMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "LockStorageSuccess", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "LockStorageSuccess", M2Share.g_sLockStorageSuccessMsg);
            }
            else
            {
                M2Share.g_sLockStorageSuccessMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "StoragePasswordClearMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "StoragePasswordClearMsg", M2Share.g_sStoragePasswordClearMsg);
            }
            else
            {
                M2Share.g_sStoragePasswordClearMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "PleaseUnloadStoragePasswordMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "PleaseUnloadStoragePasswordMsg", M2Share.g_sPleaseUnloadStoragePasswordMsg);
            }
            else
            {
                M2Share.g_sPleaseUnloadStoragePasswordMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "StorageAlreadyLock", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "StorageAlreadyLock", M2Share.g_sStorageAlreadyLockMsg);
            }
            else
            {
                M2Share.g_sStorageAlreadyLockMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "StoragePasswordLocked", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "StoragePasswordLocked", M2Share.g_sStoragePasswordLockedMsg);
            }
            else
            {
                M2Share.g_sStoragePasswordLockedMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "StorageSetPassword", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "StorageSetPassword", M2Share.g_sSetPasswordMsg);
            }
            else
            {
                M2Share.g_sSetPasswordMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "PleaseInputOldPassword", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "PleaseInputOldPassword", M2Share.g_sPleaseInputOldPasswordMsg);
            }
            else
            {
                M2Share.g_sPleaseInputOldPasswordMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "PasswordIsClearMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "PasswordIsClearMsg", M2Share.g_sOldPasswordIsClearMsg);
            }
            else
            {
                M2Share.g_sOldPasswordIsClearMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "NoPasswordSet", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "NoPasswordSet", M2Share.g_sNoPasswordSetMsg);
            }
            else
            {
                M2Share.g_sNoPasswordSetMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "OldPasswordIncorrect", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "OldPasswordIncorrect", M2Share.g_sOldPasswordIncorrectMsg);
            }
            else
            {
                M2Share.g_sOldPasswordIncorrectMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "StorageIsLocked", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "StorageIsLocked", M2Share.g_sStorageIsLockedMsg);
            }
            else
            {
                M2Share.g_sStorageIsLockedMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "PleaseTryDealLaterMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "PleaseTryDealLaterMsg", M2Share.g_sPleaseTryDealLaterMsg);
            }
            else
            {
                M2Share.g_sPleaseTryDealLaterMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DealItemsDenyGetBackMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DealItemsDenyGetBackMsg", M2Share.g_sDealItemsDenyGetBackMsg);
            }
            else
            {
                M2Share.g_sDealItemsDenyGetBackMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DisableDealItemsMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DisableDealItemsMsg", M2Share.g_sDisableDealItemsMsg);
            }
            else
            {
                M2Share.g_sDisableDealItemsMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CanotTryDealMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CanotTryDealMsg", M2Share.g_sCanotTryDealMsg);
            }
            else
            {
                M2Share.g_sCanotTryDealMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DealActionCancelMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DealActionCancelMsg", M2Share.g_sDealActionCancelMsg);
            }
            else
            {
                M2Share.g_sDealActionCancelMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "PoseDisableDealMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "PoseDisableDealMsg", M2Share.g_sPoseDisableDealMsg);
            }
            else
            {
                M2Share.g_sPoseDisableDealMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DealSuccessMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DealSuccessMsg", M2Share.g_sDealSuccessMsg);
            }
            else
            {
                M2Share.g_sDealSuccessMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DealOKTooFast", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DealOKTooFast", M2Share.g_sDealOKTooFast);
            }
            else
            {
                M2Share.g_sDealOKTooFast = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourBagSizeTooSmall", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourBagSizeTooSmall", M2Share.g_sYourBagSizeTooSmall);
            }
            else
            {
                M2Share.g_sYourBagSizeTooSmall = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DealHumanBagSizeTooSmall", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DealHumanBagSizeTooSmall", M2Share.g_sDealHumanBagSizeTooSmall);
            }
            else
            {
                M2Share.g_sDealHumanBagSizeTooSmall = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourGoldLargeThenLimit", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourGoldLargeThenLimit", M2Share.g_sYourGoldLargeThenLimit);
            }
            else
            {
                M2Share.g_sYourGoldLargeThenLimit = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DealHumanGoldLargeThenLimit", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DealHumanGoldLargeThenLimit", M2Share.g_sDealHumanGoldLargeThenLimit);
            }
            else
            {
                M2Share.g_sDealHumanGoldLargeThenLimit = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YouDealOKMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YouDealOKMsg", M2Share.g_sYouDealOKMsg);
            }
            else
            {
                M2Share.g_sYouDealOKMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "PoseDealOKMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "PoseDealOKMsg", M2Share.g_sPoseDealOKMsg);
            }
            else
            {
                M2Share.g_sPoseDealOKMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "KickClientUserMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "KickClientUserMsg", M2Share.g_sKickClientUserMsg);
            }
            else
            {
                M2Share.g_sKickClientUserMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ActionIsLockedMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ActionIsLockedMsg", M2Share.g_sActionIsLockedMsg);
            }
            else
            {
                M2Share.g_sActionIsLockedMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "PasswordNotSetMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "PasswordNotSetMsg", M2Share.g_sPasswordNotSetMsg);
            }
            else
            {
                M2Share.g_sPasswordNotSetMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "NotPasswordProtectMode", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "NotPasswordProtectMode", M2Share.g_sNotPasswordProtectMode);
            }
            else
            {
                M2Share.g_sNotPasswordProtectMode = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CanotDropGoldMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CanotDropGoldMsg", M2Share.g_sCanotDropGoldMsg);
            }
            else
            {
                M2Share.g_sCanotDropGoldMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CanotDropInSafeZoneMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CanotDropInSafeZoneMsg", M2Share.g_sCanotDropInSafeZoneMsg);
            }
            else
            {
                M2Share.g_sCanotDropInSafeZoneMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CanotDropItemMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CanotDropItemMsg", M2Share.g_sCanotDropItemMsg);
            }
            else
            {
                M2Share.g_sCanotDropItemMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CanotDropItemMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CanotDropItemMsg", M2Share.g_sCanotDropItemMsg);
            }
            else
            {
                M2Share.g_sCanotDropItemMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CanotUseItemMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CanotUseItemMsg", M2Share.g_sCanotUseItemMsg);
            }
            else
            {
                M2Share.g_sCanotUseItemMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "StartMarryManMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "StartMarryManMsg", M2Share.g_sStartMarryManMsg);
            }
            else
            {
                M2Share.g_sStartMarryManMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "StartMarryWoManMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "StartMarryWoManMsg", M2Share.g_sStartMarryWoManMsg);
            }
            else
            {
                M2Share.g_sStartMarryWoManMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "StartMarryManAskQuestionMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "StartMarryManAskQuestionMsg", M2Share.g_sStartMarryManAskQuestionMsg);
            }
            else
            {
                M2Share.g_sStartMarryManAskQuestionMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "StartMarryWoManAskQuestionMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "StartMarryWoManAskQuestionMsg", M2Share.g_sStartMarryWoManAskQuestionMsg);
            }
            else
            {
                M2Share.g_sStartMarryWoManAskQuestionMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MarryManAnswerQuestionMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MarryManAnswerQuestionMsg", M2Share.g_sMarryManAnswerQuestionMsg);
            }
            else
            {
                M2Share.g_sMarryManAnswerQuestionMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MarryManAskQuestionMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MarryManAskQuestionMsg", M2Share.g_sMarryManAskQuestionMsg);
            }
            else
            {
                M2Share.g_sMarryManAskQuestionMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MarryWoManAnswerQuestionMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MarryWoManAnswerQuestionMsg", M2Share.g_sMarryWoManAnswerQuestionMsg);
            }
            else
            {
                M2Share.g_sMarryWoManAnswerQuestionMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MarryWoManGetMarryMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MarryWoManGetMarryMsg", M2Share.g_sMarryWoManGetMarryMsg);
            }
            else
            {
                M2Share.g_sMarryWoManGetMarryMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MarryWoManDenyMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MarryWoManDenyMsg", M2Share.g_sMarryWoManDenyMsg);
            }
            else
            {
                M2Share.g_sMarryWoManDenyMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MarryWoManCancelMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MarryWoManCancelMsg", M2Share.g_sMarryWoManCancelMsg);
            }
            else
            {
                M2Share.g_sMarryWoManCancelMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ForceUnMarryManLoginMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ForceUnMarryManLoginMsg", M2Share.g_sfUnMarryManLoginMsg);
            }
            else
            {
                M2Share.g_sfUnMarryManLoginMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ForceUnMarryWoManLoginMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ForceUnMarryWoManLoginMsg", M2Share.g_sfUnMarryWoManLoginMsg);
            }
            else
            {
                M2Share.g_sfUnMarryWoManLoginMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ManLoginDearOnlineSelfMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ManLoginDearOnlineSelfMsg", M2Share.g_sManLoginDearOnlineSelfMsg);
            }
            else
            {
                M2Share.g_sManLoginDearOnlineSelfMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ManLoginDearOnlineDearMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ManLoginDearOnlineDearMsg", M2Share.g_sManLoginDearOnlineDearMsg);
            }
            else
            {
                M2Share.g_sManLoginDearOnlineDearMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WoManLoginDearOnlineSelfMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WoManLoginDearOnlineSelfMsg", M2Share.g_sWoManLoginDearOnlineSelfMsg);
            }
            else
            {
                M2Share.g_sWoManLoginDearOnlineSelfMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WoManLoginDearOnlineDearMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WoManLoginDearOnlineDearMsg", M2Share.g_sWoManLoginDearOnlineDearMsg);
            }
            else
            {
                M2Share.g_sWoManLoginDearOnlineDearMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ManLoginDearNotOnlineMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ManLoginDearNotOnlineMsg", M2Share.g_sManLoginDearNotOnlineMsg);
            }
            else
            {
                M2Share.g_sManLoginDearNotOnlineMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WoManLoginDearNotOnlineMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WoManLoginDearNotOnlineMsg", M2Share.g_sWoManLoginDearNotOnlineMsg);
            }
            else
            {
                M2Share.g_sWoManLoginDearNotOnlineMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ManLongOutDearOnlineMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ManLongOutDearOnlineMsg", M2Share.g_sManLongOutDearOnlineMsg);
            }
            else
            {
                M2Share.g_sManLongOutDearOnlineMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WoManLongOutDearOnlineMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WoManLongOutDearOnlineMsg", M2Share.g_sWoManLongOutDearOnlineMsg);
            }
            else
            {
                M2Share.g_sWoManLongOutDearOnlineMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YouAreNotMarryedMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YouAreNotMarryedMsg", M2Share.g_sYouAreNotMarryedMsg);
            }
            else
            {
                M2Share.g_sYouAreNotMarryedMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourWifeNotOnlineMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourWifeNotOnlineMsg", M2Share.g_sYourWifeNotOnlineMsg);
            }
            else
            {
                M2Share.g_sYourWifeNotOnlineMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourHusbandNotOnlineMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourHusbandNotOnlineMsg", M2Share.g_sYourHusbandNotOnlineMsg);
            }
            else
            {
                M2Share.g_sYourHusbandNotOnlineMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourWifeNowLocateMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourWifeNowLocateMsg", M2Share.g_sYourWifeNowLocateMsg);
            }
            else
            {
                M2Share.g_sYourWifeNowLocateMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourHusbandSearchLocateMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourHusbandSearchLocateMsg", M2Share.g_sYourHusbandSearchLocateMsg);
            }
            else
            {
                M2Share.g_sYourHusbandSearchLocateMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourHusbandNowLocateMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourHusbandNowLocateMsg", M2Share.g_sYourHusbandNowLocateMsg);
            }
            else
            {
                M2Share.g_sYourHusbandNowLocateMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourWifeSearchLocateMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourWifeSearchLocateMsg", M2Share.g_sYourWifeSearchLocateMsg);
            }
            else
            {
                M2Share.g_sYourWifeSearchLocateMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "FUnMasterLoginMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "FUnMasterLoginMsg", M2Share.g_sfUnMasterLoginMsg);
            }
            else
            {
                M2Share.g_sfUnMasterLoginMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "UnMasterListLoginMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "UnMasterListLoginMsg", M2Share.g_sfUnMasterListLoginMsg);
            }
            else
            {
                M2Share.g_sfUnMasterListLoginMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MasterListOnlineSelfMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MasterListOnlineSelfMsg", M2Share.g_sMasterListOnlineSelfMsg);
            }
            else
            {
                M2Share.g_sMasterListOnlineSelfMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MasterListOnlineMasterMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MasterListOnlineMasterMsg", M2Share.g_sMasterListOnlineMasterMsg);
            }
            else
            {
                M2Share.g_sMasterListOnlineMasterMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MasterOnlineSelfMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MasterOnlineSelfMsg", M2Share.g_sMasterOnlineSelfMsg);
            }
            else
            {
                M2Share.g_sMasterOnlineSelfMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MasterOnlineMasterListMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MasterOnlineMasterListMsg", M2Share.g_sMasterOnlineMasterListMsg);
            }
            else
            {
                M2Share.g_sMasterOnlineMasterListMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MasterLongOutMasterListOnlineMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MasterLongOutMasterListOnlineMsg", M2Share.g_sMasterLongOutMasterListOnlineMsg);
            }
            else
            {
                M2Share.g_sMasterLongOutMasterListOnlineMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MasterListLongOutMasterOnlineMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MasterListLongOutMasterOnlineMsg", M2Share.g_sMasterListLongOutMasterOnlineMsg);
            }
            else
            {
                M2Share.g_sMasterListLongOutMasterOnlineMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MasterListNotOnlineMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MasterListNotOnlineMsg", M2Share.g_sMasterListNotOnlineMsg);
            }
            else
            {
                M2Share.g_sMasterListNotOnlineMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MasterNotOnlineMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MasterNotOnlineMsg", M2Share.g_sMasterNotOnlineMsg);
            }
            else
            {
                M2Share.g_sMasterNotOnlineMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YouAreNotMasterMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YouAreNotMasterMsg", M2Share.g_sYouAreNotMasterMsg);
            }
            else
            {
                M2Share.g_sYouAreNotMasterMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourMasterNotOnlineMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourMasterNotOnlineMsg", M2Share.g_sYourMasterNotOnlineMsg);
            }
            else
            {
                M2Share.g_sYourMasterNotOnlineMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourMasterListNotOnlineMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourMasterListNotOnlineMsg", M2Share.g_sYourMasterListNotOnlineMsg);
            }
            else
            {
                M2Share.g_sYourMasterListNotOnlineMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourMasterNowLocateMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourMasterNowLocateMsg", M2Share.g_sYourMasterNowLocateMsg);
            }
            else
            {
                M2Share.g_sYourMasterNowLocateMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourMasterListSearchLocateMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourMasterListSearchLocateMsg", M2Share.g_sYourMasterListSearchLocateMsg);
            }
            else
            {
                M2Share.g_sYourMasterListSearchLocateMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourMasterListNowLocateMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourMasterListNowLocateMsg", M2Share.g_sYourMasterListNowLocateMsg);
            }
            else
            {
                M2Share.g_sYourMasterListNowLocateMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourMasterSearchLocateMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourMasterSearchLocateMsg", M2Share.g_sYourMasterSearchLocateMsg);
            }
            else
            {
                M2Share.g_sYourMasterSearchLocateMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourMasterListUnMasterOKMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourMasterListUnMasterOKMsg", M2Share.g_sYourMasterListUnMasterOKMsg);
            }
            else
            {
                M2Share.g_sYourMasterListUnMasterOKMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YouAreUnMasterOKMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YouAreUnMasterOKMsg", M2Share.g_sYouAreUnMasterOKMsg);
            }
            else
            {
                M2Share.g_sYouAreUnMasterOKMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "UnMasterLoginMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "UnMasterLoginMsg", M2Share.g_sUnMasterLoginMsg);
            }
            else
            {
                M2Share.g_sUnMasterLoginMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "NPCSayUnMasterOKMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "NPCSayUnMasterOKMsg", M2Share.g_sNPCSayUnMasterOKMsg);
            }
            else
            {
                M2Share.g_sNPCSayUnMasterOKMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "NPCSayForceUnMasterMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "NPCSayForceUnMasterMsg", M2Share.g_sNPCSayForceUnMasterMsg);
            }
            else
            {
                M2Share.g_sNPCSayForceUnMasterMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MyInfo", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MyInfo", M2Share.g_sMyInfo);
            }
            else
            {
                M2Share.g_sMyInfo = LoadString;
            }
            LoadString = StringConf.ReadString("String", "OpenedDealMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "OpenedDealMsg", M2Share.g_sOpenedDealMsg);
            }
            else
            {
                M2Share.g_sOpenedDealMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "SendCustMsgCanNotUseNowMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "SendCustMsgCanNotUseNowMsg", M2Share.g_sSendCustMsgCanNotUseNowMsg);
            }
            else
            {
                M2Share.g_sSendCustMsgCanNotUseNowMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "SubkMasterMsgCanNotUseNowMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "SubkMasterMsgCanNotUseNowMsg", M2Share.g_sSubkMasterMsgCanNotUseNowMsg);
            }
            else
            {
                M2Share.g_sSubkMasterMsgCanNotUseNowMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "SendOnlineCountMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "SendOnlineCountMsg", M2Share.g_sSendOnlineCountMsg);
            }
            else
            {
                M2Share.g_sSendOnlineCountMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WeaponRepairSuccess", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WeaponRepairSuccess", M2Share.g_sWeaponRepairSuccess);
            }
            else
            {
                M2Share.g_sWeaponRepairSuccess = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DefenceUpTime", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DefenceUpTime", M2Share.g_sDefenceUpTime);
            }
            else
            {
                M2Share.g_sDefenceUpTime = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MagDefenceUpTime", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MagDefenceUpTime", M2Share.g_sMagDefenceUpTime);
            }
            else
            {
                M2Share.g_sMagDefenceUpTime = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WinLottery1Msg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WinLottery1Msg", M2Share.g_sWinLottery1Msg);
            }
            else
            {
                M2Share.g_sWinLottery1Msg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WinLottery2Msg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WinLottery2Msg", M2Share.g_sWinLottery2Msg);
            }
            else
            {
                M2Share.g_sWinLottery2Msg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WinLottery3Msg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WinLottery3Msg", M2Share.g_sWinLottery3Msg);
            }
            else
            {
                M2Share.g_sWinLottery3Msg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WinLottery4Msg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WinLottery4Msg", M2Share.g_sWinLottery4Msg);
            }
            else
            {
                M2Share.g_sWinLottery4Msg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WinLottery5Msg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WinLottery5Msg", M2Share.g_sWinLottery5Msg);
            }
            else
            {
                M2Share.g_sWinLottery5Msg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WinLottery6Msg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WinLottery6Msg", M2Share.g_sWinLottery6Msg);
            }
            else
            {
                M2Share.g_sWinLottery6Msg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "NotWinLotteryMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "NotWinLotteryMsg", M2Share.g_sNotWinLotteryMsg);
            }
            else
            {
                M2Share.g_sNotWinLotteryMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WeaptonMakeLuck", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WeaptonMakeLuck", M2Share.g_sWeaptonMakeLuck);
            }
            else
            {
                M2Share.g_sWeaptonMakeLuck = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WeaptonNotMakeLuck", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WeaptonNotMakeLuck", M2Share.g_sWeaptonNotMakeLuck);
            }
            else
            {
                M2Share.g_sWeaptonNotMakeLuck = LoadString;
            }
            LoadString = StringConf.ReadString("String", "TheWeaponIsCursed", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "TheWeaponIsCursed", M2Share.g_sTheWeaponIsCursed);
            }
            else
            {
                M2Share.g_sTheWeaponIsCursed = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CanotTakeOffItem", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CanotTakeOffItem", M2Share.g_sCanotTakeOffItem);
            }
            else
            {
                M2Share.g_sCanotTakeOffItem = LoadString;
            }
            LoadString = StringConf.ReadString("String", "JoinGroupMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "JoinGroupMsg", M2Share.g_sJoinGroup);
            }
            else
            {
                M2Share.g_sJoinGroup = LoadString;
            }
            LoadString = StringConf.ReadString("String", "TryModeCanotUseStorage", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "TryModeCanotUseStorage", M2Share.g_sTryModeCanotUseStorage);
            }
            else
            {
                M2Share.g_sTryModeCanotUseStorage = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CanotGetItemsMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CanotGetItemsMsg", M2Share.g_sCanotGetItems);
            }
            else
            {
                M2Share.g_sCanotGetItems = LoadString;
            }
            LoadString = StringConf.ReadString("String", "EnableDearRecall", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "EnableDearRecall", M2Share.g_sEnableDearRecall);
            }
            else
            {
                M2Share.g_sEnableDearRecall = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DisableDearRecall", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DisableDearRecall", M2Share.g_sDisableDearRecall);
            }
            else
            {
                M2Share.g_sDisableDearRecall = LoadString;
            }
            LoadString = StringConf.ReadString("String", "EnableMasterRecall", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "EnableMasterRecall", M2Share.g_sEnableMasterRecall);
            }
            else
            {
                M2Share.g_sEnableMasterRecall = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DisableMasterRecall", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DisableMasterRecall", M2Share.g_sDisableMasterRecall);
            }
            else
            {
                M2Share.g_sDisableMasterRecall = LoadString;
            }
            LoadString = StringConf.ReadString("String", "NowCurrDateTime", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "NowCurrDateTime", M2Share.g_sNowCurrDateTime);
            }
            else
            {
                M2Share.g_sNowCurrDateTime = LoadString;
            }
            LoadString = StringConf.ReadString("String", "EnableHearWhisper", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "EnableHearWhisper", M2Share.g_sEnableHearWhisper);
            }
            else
            {
                M2Share.g_sEnableHearWhisper = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DisableHearWhisper", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DisableHearWhisper", M2Share.g_sDisableHearWhisper);
            }
            else
            {
                M2Share.g_sDisableHearWhisper = LoadString;
            }
            LoadString = StringConf.ReadString("String", "EnableShoutMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "EnableShoutMsg", M2Share.g_sEnableShoutMsg);
            }
            else
            {
                M2Share.g_sEnableShoutMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DisableShoutMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DisableShoutMsg", M2Share.g_sDisableShoutMsg);
            }
            else
            {
                M2Share.g_sDisableShoutMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "EnableDealMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "EnableDealMsg", M2Share.g_sEnableDealMsg);
            }
            else
            {
                M2Share.g_sEnableDealMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DisableDealMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DisableDealMsg", M2Share.g_sDisableDealMsg);
            }
            else
            {
                M2Share.g_sDisableDealMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "EnableGuildChat", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "EnableGuildChat", M2Share.g_sEnableGuildChat);
            }
            else
            {
                M2Share.g_sEnableGuildChat = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DisableGuildChat", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DisableGuildChat", M2Share.g_sDisableGuildChat);
            }
            else
            {
                M2Share.g_sDisableGuildChat = LoadString;
            }
            LoadString = StringConf.ReadString("String", "EnableJoinGuild", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "EnableJoinGuild", M2Share.g_sEnableJoinGuild);
            }
            else
            {
                M2Share.g_sEnableJoinGuild = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DisableJoinGuild", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DisableJoinGuild", M2Share.g_sDisableJoinGuild);
            }
            else
            {
                M2Share.g_sDisableJoinGuild = LoadString;
            }
            LoadString = StringConf.ReadString("String", "EnableAuthAllyGuild", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "EnableAuthAllyGuild", M2Share.g_sEnableAuthAllyGuild);
            }
            else
            {
                M2Share.g_sEnableAuthAllyGuild = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DisableAuthAllyGuild", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DisableAuthAllyGuild", M2Share.g_sDisableAuthAllyGuild);
            }
            else
            {
                M2Share.g_sDisableAuthAllyGuild = LoadString;
            }
            LoadString = StringConf.ReadString("String", "EnableGroupRecall", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "EnableGroupRecall", M2Share.g_sEnableGroupRecall);
            }
            else
            {
                M2Share.g_sEnableGroupRecall = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DisableGroupRecall", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DisableGroupRecall", M2Share.g_sDisableGroupRecall);
            }
            else
            {
                M2Share.g_sDisableGroupRecall = LoadString;
            }
            LoadString = StringConf.ReadString("String", "EnableGuildRecall", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "EnableGuildRecall", M2Share.g_sEnableGuildRecall);
            }
            else
            {
                M2Share.g_sEnableGuildRecall = LoadString;
            }
            LoadString = StringConf.ReadString("String", "DisableGuildRecall", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "DisableGuildRecall", M2Share.g_sDisableGuildRecall);
            }
            else
            {
                M2Share.g_sDisableGuildRecall = LoadString;
            }
            LoadString = StringConf.ReadString("String", "PleaseInputPassword", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "PleaseInputPassword", M2Share.g_sPleaseInputPassword);
            }
            else
            {
                M2Share.g_sPleaseInputPassword = LoadString;
            }
            LoadString = StringConf.ReadString("String", "TheMapDisableMove", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "TheMapDisableMove", M2Share.g_sTheMapDisableMove);
            }
            else
            {
                M2Share.g_sTheMapDisableMove = LoadString;
            }
            LoadString = StringConf.ReadString("String", "TheMapNotFound", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "TheMapNotFound", M2Share.g_sTheMapNotFound);
            }
            else
            {
                M2Share.g_sTheMapNotFound = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourIPaddrDenyLogon", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourIPaddrDenyLogon", M2Share.g_sYourIPaddrDenyLogon);
            }
            else
            {
                M2Share.g_sYourIPaddrDenyLogon = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourAccountDenyLogon", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourAccountDenyLogon", M2Share.g_sYourAccountDenyLogon);
            }
            else
            {
                M2Share.g_sYourAccountDenyLogon = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YourCharNameDenyLogon", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YourCharNameDenyLogon", M2Share.g_sYourCharNameDenyLogon);
            }
            else
            {
                M2Share.g_sYourCharNameDenyLogon = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CanotPickUpItem", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CanotPickUpItem", M2Share.g_sCanotPickUpItem);
            }
            else
            {
                M2Share.g_sCanotPickUpItem = LoadString;
            }
            LoadString = StringConf.ReadString("String", "sQUERYBAGITEMS", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "sQUERYBAGITEMS", M2Share.g_sQUERYBAGITEMS);
            }
            else
            {
                M2Share.g_sQUERYBAGITEMS = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CanotSendmsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CanotSendmsg", M2Share.g_sCanotSendmsg);
            }
            else
            {
                M2Share.g_sCanotSendmsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "UserDenyWhisperMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "UserDenyWhisperMsg", M2Share.g_sUserDenyWhisperMsg);
            }
            else
            {
                M2Share.g_sUserDenyWhisperMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "UserNotOnLine", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "UserNotOnLine", M2Share.g_sUserNotOnLine);
            }
            else
            {
                M2Share.g_sUserNotOnLine = LoadString;
            }
            LoadString = StringConf.ReadString("String", "RevivalRecoverMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "RevivalRecoverMsg", M2Share.g_sRevivalRecoverMsg);
            }
            else
            {
                M2Share.g_sRevivalRecoverMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ClientVersionTooOld", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ClientVersionTooOld", M2Share.g_sClientVersionTooOld);
            }
            else
            {
                M2Share.g_sClientVersionTooOld = LoadString;
            }
            LoadString = StringConf.ReadString("String", "CastleGuildName", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "CastleGuildName", M2Share.g_sCastleGuildName);
            }
            else
            {
                M2Share.g_sCastleGuildName = LoadString;
            }
            LoadString = StringConf.ReadString("String", "NoCastleGuildName", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "NoCastleGuildName", M2Share.g_sNoCastleGuildName);
            }
            else
            {
                M2Share.g_sNoCastleGuildName = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WarrReNewName", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WarrReNewName", M2Share.g_sWarrReNewName);
            }
            else
            {
                M2Share.g_sWarrReNewName = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WizardReNewName", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WizardReNewName", M2Share.g_sWizardReNewName);
            }
            else
            {
                M2Share.g_sWizardReNewName = LoadString;
            }
            LoadString = StringConf.ReadString("String", "TaosReNewName", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "TaosReNewName", M2Share.g_sTaosReNewName);
            }
            else
            {
                M2Share.g_sTaosReNewName = LoadString;
            }
            LoadString = StringConf.ReadString("String", "RankLevelName", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "RankLevelName", M2Share.g_sRankLevelName);
            }
            else
            {
                M2Share.g_sRankLevelName = LoadString.Replace("%s", "{0}");
            }
            LoadString = StringConf.ReadString("String", "ManDearName", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ManDearName", M2Share.g_sManDearName);
            }
            else
            {
                M2Share.g_sManDearName = LoadString;
            }
            LoadString = StringConf.ReadString("String", "WoManDearName", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "WoManDearName", M2Share.g_sWoManDearName);
            }
            else
            {
                M2Share.g_sWoManDearName = LoadString;
            }
            LoadString = StringConf.ReadString("String", "MasterName", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "MasterName", M2Share.g_sMasterName);
            }
            else
            {
                M2Share.g_sMasterName = LoadString;
            }
            
            LoadString = StringConf.ReadString("String", "NoMasterName", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "NoMasterName", M2Share.g_sNoMasterName);
            }
            else
            {
                M2Share.g_sNoMasterName = LoadString;
            }
            LoadString = StringConf.ReadString("String", "HumanShowName", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "HumanShowName", M2Share.g_sHumanShowName);
            }
            else
            {
                M2Share.g_sHumanShowName = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ChangePermissionMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ChangePermissionMsg", M2Share.g_sChangePermissionMsg);
            }
            else
            {
                M2Share.g_sChangePermissionMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ChangeKillMonExpRateMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ChangeKillMonExpRateMsg", M2Share.g_sChangeKillMonExpRateMsg);
            }
            else
            {
                M2Share.g_sChangeKillMonExpRateMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ChangePowerRateMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ChangePowerRateMsg", M2Share.g_sChangePowerRateMsg);
            }
            else
            {
                M2Share.g_sChangePowerRateMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ChangeMemberLevelMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ChangeMemberLevelMsg", M2Share.g_sChangeMemberLevelMsg);
            }
            else
            {
                M2Share.g_sChangeMemberLevelMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ChangeMemberTypeMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ChangeMemberTypeMsg", M2Share.g_sChangeMemberTypeMsg);
            }
            else
            {
                M2Share.g_sChangeMemberTypeMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ScriptChangeHumanHPMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ScriptChangeHumanHPMsg", M2Share.g_sScriptChangeHumanHPMsg);
            }
            else
            {
                M2Share.g_sScriptChangeHumanHPMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ScriptChangeHumanMPMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ScriptChangeHumanMPMsg", M2Share.g_sScriptChangeHumanMPMsg);
            }
            else
            {
                M2Share.g_sScriptChangeHumanMPMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YouCanotDisableSayMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YouCanotDisableSayMsg", M2Share.g_sDisableSayMsg);
            }
            else
            {
                M2Share.g_sDisableSayMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "OnlineCountMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "OnlineCountMsg", M2Share.g_sOnlineCountMsg);
            }
            else
            {
                M2Share.g_sOnlineCountMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "TotalOnlineCountMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "TotalOnlineCountMsg", M2Share.g_sTotalOnlineCountMsg);
            }
            else
            {
                M2Share.g_sTotalOnlineCountMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YouNeedLevelSendMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YouNeedLevelSendMsg", M2Share.g_sYouNeedLevelMsg);
            }
            else
            {
                M2Share.g_sYouNeedLevelMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "ThisMapDisableSendCyCyMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "ThisMapDisableSendCyCyMsg", M2Share.g_sThisMapDisableSendCyCyMsg);
            }
            else
            {
                M2Share.g_sThisMapDisableSendCyCyMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YouCanSendCyCyLaterMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YouCanSendCyCyLaterMsg", M2Share.g_sYouCanSendCyCyLaterMsg);
            }
            else
            {
                M2Share.g_sYouCanSendCyCyLaterMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YouIsDisableSendMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YouIsDisableSendMsg", M2Share.g_sYouIsDisableSendMsg);
            }
            else
            {
                M2Share.g_sYouIsDisableSendMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YouMurderedMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YouMurderedMsg", M2Share.g_sYouMurderedMsg);
            }
            else
            {
                M2Share.g_sYouMurderedMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YouKilledByMsg", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YouKilledByMsg", M2Share.g_sYouKilledByMsg);
            }
            else
            {
                M2Share.g_sYouKilledByMsg = LoadString;
            }
            LoadString = StringConf.ReadString("String", "YouProtectedByLawOfDefense", "");
            if (LoadString == "")
            {
                StringConf.WriteString("String", "YouProtectedByLawOfDefense", M2Share.g_sYouProtectedByLawOfDefense);
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
            sString = StringConf.ReadString(sSection, sIdent, "");
            if (sString == "")
            {
                StringConf.WriteString(sSection, sIdent, sDefault);
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
