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
            var LoadString = string.Empty;

            M2Share.Config.ServerIPaddr = ReadWriteString("Server", "ServerIP", M2Share.Config.ServerIPaddr);
            M2Share.Config.sWebSite = ReadWriteString("Server", "WebSite", M2Share.Config.sWebSite);
            M2Share.Config.sBbsSite = ReadWriteString("Server", "BbsSite", M2Share.Config.sBbsSite);
            M2Share.Config.sClientDownload = ReadWriteString("Server", "ClientDownload", M2Share.Config.sClientDownload);
            M2Share.Config.sQQ = ReadWriteString("Server", "QQ", M2Share.Config.sQQ);
            M2Share.Config.sPhone = ReadWriteString("Server", "Phone", M2Share.Config.sPhone);
            M2Share.Config.sBankAccount0 = ReadWriteString("Server", "BankAccount0", M2Share.Config.sBankAccount0);
            M2Share.Config.sBankAccount1 = ReadWriteString("Server", "BankAccount1", M2Share.Config.sBankAccount1);
            M2Share.Config.sBankAccount2 = ReadWriteString("Server", "BankAccount2", M2Share.Config.sBankAccount2);
            M2Share.Config.sBankAccount3 = ReadWriteString("Server", "BankAccount3", M2Share.Config.sBankAccount3);
            M2Share.Config.sBankAccount4 = ReadWriteString("Server", "BankAccount4", M2Share.Config.sBankAccount4);
            M2Share.Config.sBankAccount5 = ReadWriteString("Server", "BankAccount5", M2Share.Config.sBankAccount5);
            M2Share.Config.sBankAccount6 = ReadWriteString("Server", "BankAccount6", M2Share.Config.sBankAccount6);
            M2Share.Config.sBankAccount7 = ReadWriteString("Server", "BankAccount7", M2Share.Config.sBankAccount7);
            M2Share.Config.sBankAccount8 = ReadWriteString("Server", "BankAccount8", M2Share.Config.sBankAccount8);
            M2Share.Config.sBankAccount9 = ReadWriteString("Server", "BankAccount9", M2Share.Config.sBankAccount9);
            M2Share.Config.GuildNotice = ReadWriteString("Guild", "GuildNotice", M2Share.Config.GuildNotice);
            M2Share.Config.GuildWar = ReadWriteString("Guild", "GuildWar", M2Share.Config.GuildWar);
            M2Share.Config.GuildAll = ReadWriteString("Guild", "GuildAll", M2Share.Config.GuildAll);
            M2Share.Config.GuildMember = ReadWriteString("Guild", "GuildMember", M2Share.Config.GuildMember);
            M2Share.Config.GuildMemberRank = ReadWriteString("Guild", "GuildMemberRank", M2Share.Config.GuildMemberRank);
            M2Share.Config.GuildChief = ReadWriteString("Guild", "GuildChief", M2Share.Config.GuildChief);
            M2Share.Config.LineNoticePreFix = ReadWriteString("String", "LineNoticePreFix", M2Share.Config.LineNoticePreFix);
            M2Share.Config.SysMsgPreFix = ReadWriteString("String", "SysMsgPreFix", M2Share.Config.SysMsgPreFix);
            M2Share.Config.GuildMsgPreFix = ReadWriteString("String", "GuildMsgPreFix", M2Share.Config.GuildMsgPreFix);
            M2Share.Config.GroupMsgPreFix = ReadWriteString("String", "GroupMsgPreFix", M2Share.Config.GroupMsgPreFix);
            M2Share.Config.HintMsgPreFix = ReadWriteString("String", "HintMsgPreFix", M2Share.Config.HintMsgPreFix);
            M2Share.Config.GameManagerRedMsgPreFix = ReadWriteString("String", "GMRedMsgpreFix", M2Share.Config.GameManagerRedMsgPreFix);
            M2Share.Config.MonSayMsgPreFix = ReadWriteString("String", "MonSayMsgpreFix", M2Share.Config.MonSayMsgPreFix);
            M2Share.Config.CustMsgPreFix = ReadWriteString("String", "CustMsgpreFix", M2Share.Config.CustMsgPreFix);
            M2Share.Config.CastleMsgPreFix = ReadWriteString("String", "CastleMsgpreFix", M2Share.Config.CastleMsgPreFix);
            
            Settings.ClientSoftVersionError = ReadWriteString("String", "ClientSoftVersionError", Settings.ClientSoftVersionError);
            Settings.DownLoadNewClientSoft = ReadWriteString("String", "DownLoadNewClientSoft", Settings.DownLoadNewClientSoft);    
            Settings.ForceDisConnect = ReadWriteString("String", "ForceDisConnect", Settings.ForceDisConnect);
            Settings.ClientSoftVersionTooOld = ReadWriteString("String", "ClientSoftVersionTooOld", Settings.ClientSoftVersionTooOld);
            Settings.DownLoadAndUseNewClient = ReadWriteString("String", "DownLoadAndUseNewClient", Settings.DownLoadAndUseNewClient);
            Settings.OnlineUserFull = ReadWriteString("String", "OnlineUserFull", Settings.OnlineUserFull);
            Settings.YouNowIsTryPlayMode = ReadWriteString("String", "YouNowIsTryPlayMode", Settings.YouNowIsTryPlayMode);
            Settings.NowIsFreePlayMode = ReadWriteString("String", "NowIsFreePlayMode", Settings.NowIsFreePlayMode);
            Settings.AttackModeOfAll = ReadWriteString("String", "AttackModeOfAll",  Settings.AttackModeOfAll);
            Settings.AttackModeOfPeaceful = ReadWriteString("String", "AttackModeOfPeaceful", Settings.AttackModeOfPeaceful);
            Settings.AttackModeOfGroup = ReadWriteString("String", "AttackModeOfGroup", Settings.AttackModeOfGroup);
            Settings.AttackModeOfGuild = ReadWriteString("String", "AttackModeOfGuild", Settings.AttackModeOfGuild);
            Settings.AttackModeOfRedWhite = ReadWriteString("String", "AttackModeOfRedWhite", Settings.AttackModeOfRedWhite);
            Settings.AttackModeOfMaster = ReadWriteString("String", "AttackModeOfMaster", Settings.AttackModeOfMaster);
            Settings.StartChangeAttackModeHelp = ReadWriteString("String", "StartChangeAttackModeHelp", Settings.StartChangeAttackModeHelp);
            Settings.StartNoticeMsg = ReadWriteString("String", "StartNoticeMsg", Settings.StartNoticeMsg);
            Settings.ThrustingOn = ReadWriteString("String", "ThrustingOn", Settings.ThrustingOn);
            Settings.ThrustingOff = ReadWriteString("String", "ThrustingOff", Settings.ThrustingOff);
            Settings.HalfMoonOn = ReadWriteString("String", "HalfMoonOn", Settings.HalfMoonOn);
            Settings.HalfMoonOff = ReadWriteString("String", "HalfMoonOff", Settings.HalfMoonOff);
            Settings.CrsHitOn = ReadWriteString("String", "CrsHitOn", Settings.CrsHitOn);
            Settings.CrsHitOff = ReadWriteString("String", "CrsHitOff", Settings.CrsHitOff);
            Settings.TwinHitOn = LoadConfigString("String", "TwinHitOn", Settings.TwinHitOn);
            Settings.TwinHitOff = LoadConfigString("String", "TwinHitOff", Settings.TwinHitOff);
            Settings.FireSpiritsSummoned = ReadWriteString("String", "FireSpiritsSummoned", Settings.FireSpiritsSummoned);
            Settings.FireSpiritsFail = ReadWriteString("String", "FireSpiritsFail", Settings.FireSpiritsFail);
            Settings.SpiritsGone = ReadWriteString("String", "SpiritsGone", Settings.SpiritsGone);
            Settings.MateDoTooweak = ReadWriteString("String", "MateDoTooweak", Settings.MateDoTooweak);
            Settings.TheWeaponBroke = ReadWriteString("String", "TheWeaponBroke", Settings.TheWeaponBroke);
            Settings.TheWeaponRefineSuccessfull = ReadWriteString("String", "TheWeaponRefineSuccessfull", Settings.TheWeaponRefineSuccessfull);
            Settings.YouPoisoned = ReadWriteString("String", "YouPoisoned", Settings.YouPoisoned);
            Settings.PetRest = ReadWriteString("String", "PetRest", Settings.PetRest);
            Settings.PetAttack = ReadWriteString("String", "PetAttack", Settings.PetAttack);
            Settings.WearNotOfWoMan = ReadWriteString("String", "WearNotOfWoMan", Settings.WearNotOfWoMan);
            Settings.WearNotOfMan = ReadWriteString("String", "WearNotOfMan", Settings.WearNotOfMan);
            Settings.HandWeightNot = ReadWriteString("String", "HandWeightNot", Settings.HandWeightNot);
            Settings.WearWeightNot = ReadWriteString("String", "WearWeightNot", Settings.WearWeightNot);
            Settings.ItemIsNotThisAccount = ReadWriteString("String", "ItemIsNotThisAccount", Settings.ItemIsNotThisAccount);
            Settings.ItemIsNotThisIPaddr = ReadWriteString("String", "ItemIsNotThisIPaddr", Settings.ItemIsNotThisIPaddr);
            Settings.ItemIsNotThisChrName = ReadWriteString("String", "ItemIsNotThisChrName", Settings.ItemIsNotThisChrName);
            Settings.LevelNot = ReadWriteString("String", "LevelNot", Settings.LevelNot);
            Settings.JobOrLevelNot = ReadWriteString("String", "JobOrLevelNot", Settings.JobOrLevelNot);
            Settings.JobOrDCNot = ReadWriteString("String", "JobOrDCNot", Settings.JobOrDCNot);
            Settings.JobOrMCNot = ReadWriteString("String", "JobOrMCNot", Settings.JobOrMCNot);
            Settings.JobOrSCNot = ReadWriteString("String", "JobOrSCNot",  Settings.JobOrSCNot);
            Settings.DCNot = ReadWriteString("String", "DCNot", Settings.DCNot);
            Settings.MCNot = ReadWriteString("String", "MCNot", Settings.MCNot);
            Settings.SCNot = ReadWriteString("String", "SCNot", Settings.SCNot);
            Settings.CreditPointNot = ReadWriteString("String", "CreditPointNot", Settings.CreditPointNot);
            Settings.ReNewLevelNot = ReadWriteString("String", "ReNewLevelNot", Settings.ReNewLevelNot);
            Settings.GuildNot = ReadWriteString("String", "GuildNot", Settings.GuildNot);
            Settings.GuildMasterNot = ReadWriteString("String", "GuildMasterNot", Settings.GuildMasterNot);
            Settings.SabukHumanNot = ReadWriteString("String", "SabukHumanNot", Settings.SabukHumanNot);
            Settings.SabukMasterManNot = ReadWriteString("String", "SabukMasterManNot", Settings.SabukMasterManNot);
            Settings.MemberNot = ReadWriteString("String", "MemberNot", Settings.MemberNot);
            Settings.MemberTypeNot = ReadWriteString("String", "MemberTypeNot", Settings.MemberTypeNot);
            Settings.CanottWearIt = ReadWriteString("String", "CanottWearIt", Settings.CanottWearIt);
            Settings.CanotUseDrugOnThisMap = ReadWriteString("String", "CanotUseDrugOnThisMap", Settings.CanotUseDrugOnThisMap);
            Settings.GameMasterMode = ReadWriteString("String", "GameMasterMode", Settings.GameMasterMode);
            Settings.ReleaseGameMasterMode = ReadWriteString("String", "ReleaseGameMasterMode", Settings.ReleaseGameMasterMode);
            Settings.ObserverMode = ReadWriteString("String", "ObserverMode", Settings.ObserverMode);
            Settings.ReleaseObserverMode = ReadWriteString("String", "ReleaseObserverMode", Settings.ReleaseObserverMode);
            Settings.SupermanMode = ReadWriteString("String", "SupermanMode", Settings.SupermanMode);
            Settings.ReleaseSupermanMode = ReadWriteString("String", "ReleaseSupermanMode", Settings.ReleaseSupermanMode);
            Settings.YouFoundNothing = ReadWriteString("String", "YouFoundNothing", Settings.YouFoundNothing);
            Settings.NoPasswordLockSystemMsg = ReadWriteString("String", "NoPasswordLockSystemMsg", Settings.NoPasswordLockSystemMsg);
            Settings.AlreadySetPasswordMsg = ReadWriteString("String", "AlreadySetPassword", Settings.AlreadySetPasswordMsg);
            Settings.ReSetPasswordMsg = ReadWriteString("String", "ReSetPassword", Settings.ReSetPasswordMsg);
            Settings.PasswordOverLongMsg = ReadWriteString("String", "PasswordOverLong", Settings.PasswordOverLongMsg);
            Settings.ReSetPasswordOKMsg = ReadWriteString("String", "ReSetPasswordOK", Settings.ReSetPasswordOKMsg);
            Settings.ReSetPasswordNotMatchMsg = ReadWriteString("String", "ReSetPasswordNotMatch", Settings.ReSetPasswordNotMatchMsg);
            Settings.PleaseInputUnLockPasswordMsg = ReadWriteString("String", "PleaseInputUnLockPassword", Settings.PleaseInputUnLockPasswordMsg);
            Settings.StorageUnLockOKMsg = ReadWriteString("String", "StorageUnLockOK", Settings.StorageUnLockOKMsg);
            Settings.StorageAlreadyUnLockMsg = ReadWriteString("String", "StorageAlreadyUnLock", Settings.StorageAlreadyUnLockMsg);
            Settings.StorageNoPasswordMsg = ReadWriteString("String", "StorageNoPassword", Settings.StorageNoPasswordMsg);
            Settings.UnLockPasswordFailMsg = ReadWriteString("String", "UnLockPasswordFail", Settings.UnLockPasswordFailMsg);
            Settings.LockStorageSuccessMsg = ReadWriteString("String", "LockStorageSuccess", Settings.LockStorageSuccessMsg);
            Settings.StoragePasswordClearMsg = ReadWriteString("String", "StoragePasswordClear", Settings.StoragePasswordClearMsg);
            Settings.PleaseUnloadStoragePasswordMsg = ReadWriteString("String", "PleaseUnloadStoragePassword", Settings.PleaseUnloadStoragePasswordMsg);
            Settings.StorageAlreadyLockMsg = ReadWriteString("String", "StorageAlreadyLock", Settings.StorageAlreadyLockMsg);
            Settings.StoragePasswordLockedMsg = ReadWriteString("String", "StoragePasswordLocked", Settings.StoragePasswordLockedMsg);
            Settings.SetPasswordMsg = ReadWriteString("String", "StorageSetPassword", Settings.SetPasswordMsg);
            Settings.PleaseInputOldPasswordMsg = ReadWriteString("String", "PleaseInputOldPassword", Settings.PleaseInputOldPasswordMsg);
            Settings.OldPasswordIsClearMsg = ReadWriteString("String", "PasswordIsClearMsg", Settings.OldPasswordIsClearMsg);
            Settings.NoPasswordSetMsg = ReadWriteString("String", "NoPasswordSet", Settings.NoPasswordSetMsg);
            Settings.OldPasswordIncorrectMsg = ReadWriteString("String", "OldPasswordIncorrect", Settings.OldPasswordIncorrectMsg);
            Settings.StorageIsLockedMsg = ReadWriteString("String", "StorageIsLocked", Settings.StorageIsLockedMsg);
            Settings.PleaseTryDealLaterMsg = ReadWriteString("String", "PleaseTryDealLaterMsg", Settings.PleaseTryDealLaterMsg);
            Settings.DealItemsDenyGetBackMsg = ReadWriteString("String", "DealItemsDenyGetBackMsg", Settings.DealItemsDenyGetBackMsg);
            Settings.DisableDealItemsMsg = ReadWriteString("String", "DisableDealItemsMsg", Settings.DisableDealItemsMsg);
            Settings.CanotTryDealMsg = ReadWriteString("String", "CanotTryDealMsg", Settings.CanotTryDealMsg);
            Settings.DealActionCancelMsg = ReadWriteString("String", "DealActionCancelMsg", Settings.DealActionCancelMsg);
            Settings.PoseDisableDealMsg = ReadWriteString("String", "PoseDisableDealMsg", Settings.PoseDisableDealMsg);
            Settings.DealSuccessMsg = ReadWriteString("String", "DealSuccessMsg", Settings.DealSuccessMsg);
            Settings.DealOKTooFast = ReadWriteString("String", "DealOKTooFast", Settings.DealOKTooFast);
            Settings.YourBagSizeTooSmall = ReadWriteString("String", "YourBagSizeTooSmall", Settings.YourBagSizeTooSmall);
            Settings.DealHumanBagSizeTooSmall = ReadWriteString("String", "DealHumanBagSizeTooSmall", Settings.DealHumanBagSizeTooSmall);
            Settings.YourGoldLargeThenLimit = ReadWriteString("String", "YourGoldLargeThenLimit", Settings.YourGoldLargeThenLimit);
            Settings.DealHumanGoldLargeThenLimit = ReadWriteString("String", "DealHumanGoldLargeThenLimit", Settings.DealHumanGoldLargeThenLimit);
            Settings.YouDealOKMsg = ReadWriteString("String", "YouDealOKMsg", Settings.YouDealOKMsg);
            Settings.PoseDealOKMsg = ReadWriteString("String", "PoseDealOKMsg", Settings.PoseDealOKMsg);
            Settings.KickClientUserMsg = ReadWriteString("String", "KickClientUserMsg", Settings.KickClientUserMsg);
            Settings.ActionIsLockedMsg = ReadWriteString("String", "ActionIsLockedMsg", Settings.ActionIsLockedMsg);
            Settings.PasswordNotSetMsg = ReadWriteString("String", "PasswordNotSetMsg", Settings.PasswordNotSetMsg);
            Settings.NotPasswordProtectMode = ReadWriteString("String", "NotPasswordProtectMode", Settings.NotPasswordProtectMode);
            Settings.CanotDropGoldMsg = ReadWriteString("String", "CanotDropGoldMsg", Settings.CanotDropGoldMsg);
            Settings.CanotDropInSafeZoneMsg = ReadWriteString("String", "CanotDropInSafeZoneMsg", Settings.CanotDropInSafeZoneMsg);
            Settings.CanotDropItemMsg = ReadWriteString("String", "CanotDropItemMsg", Settings.CanotDropItemMsg);
            Settings.CanotUseItemMsg = ReadWriteString("String", "CanotUseItemMsg", Settings.CanotUseItemMsg);
            Settings.StartMarryManMsg = ReadWriteString("String", "StartMarryManMsg", Settings.StartMarryManMsg);
            Settings.StartMarryWoManMsg = ReadWriteString("String", "StartMarryWoManMsg", Settings.StartMarryWoManMsg);
            Settings.StartMarryManAskQuestionMsg = ReadWriteString("String", "StartMarryManAskQuestionMsg", Settings.StartMarryManAskQuestionMsg);
            Settings.StartMarryWoManAskQuestionMsg = ReadWriteString("String", "StartMarryWoManAskQuestionMsg", Settings.StartMarryWoManAskQuestionMsg);
            Settings.MarryManAnswerQuestionMsg = ReadWriteString("String", "MarryManAnswerQuestionMsg", Settings.MarryManAnswerQuestionMsg);
            Settings.MarryManAskQuestionMsg = ReadWriteString("String", "MarryManAskQuestionMsg", Settings.MarryManAskQuestionMsg);
            Settings.MarryWoManAnswerQuestionMsg = ReadWriteString("String", "MarryWoManAnswerQuestionMsg", Settings.MarryWoManAnswerQuestionMsg);
            Settings.MarryWoManGetMarryMsg = ReadWriteString("String", "MarryWoManGetMarryMsg", Settings.MarryWoManGetMarryMsg);
            Settings.MarryWoManDenyMsg = ReadWriteString("String", "MarryWoManDenyMsg", Settings.MarryWoManDenyMsg);
            Settings.MarryWoManCancelMsg = ReadWriteString("String", "MarryWoManCancelMsg", Settings.MarryWoManCancelMsg);
            Settings.fUnMarryManLoginMsg = ReadWriteString("String", "ForceUnMarryManLoginMsg", Settings.fUnMarryManLoginMsg);
            Settings.fUnMarryWoManLoginMsg = ReadWriteString("String", "ForceUnMarryWoManLoginMsg", Settings.fUnMarryWoManLoginMsg);
            Settings.ManLoginDearOnlineSelfMsg = ReadWriteString("String", "ManLoginDearOnlineSelfMsg", Settings.ManLoginDearOnlineSelfMsg);
            Settings.ManLoginDearOnlineDearMsg = ReadWriteString("String", "ManLoginDearOnlineDearMsg", Settings.ManLoginDearOnlineDearMsg);
            Settings.WoManLoginDearOnlineSelfMsg = ReadWriteString("String", "WoManLoginDearOnlineSelfMsg", Settings.WoManLoginDearOnlineSelfMsg);
            Settings.WoManLoginDearOnlineDearMsg = ReadWriteString("String", "WoManLoginDearOnlineDearMsg", Settings.WoManLoginDearOnlineDearMsg);
            Settings.ManLoginDearNotOnlineMsg = ReadWriteString("String", "ManLoginDearNotOnlineMsg", Settings.ManLoginDearNotOnlineMsg);
            Settings.WoManLoginDearNotOnlineMsg = ReadWriteString("String", "WoManLoginDearNotOnlineMsg", Settings.WoManLoginDearNotOnlineMsg);
            Settings.ManLongOutDearOnlineMsg = ReadWriteString("String", "ManLongOutDearOnlineMsg", Settings.ManLongOutDearOnlineMsg);
            Settings.WoManLongOutDearOnlineMsg = ReadWriteString("String", "WoManLongOutDearOnlineMsg", Settings.WoManLongOutDearOnlineMsg);
            Settings.YouAreNotMarryedMsg = ReadWriteString("String", "YouAreNotMarryedMsg", Settings.YouAreNotMarryedMsg);
            Settings.YourWifeNotOnlineMsg = ReadWriteString("String", "YourWifeNotOnlineMsg", Settings.YourWifeNotOnlineMsg);
            Settings.YourHusbandNotOnlineMsg = ReadWriteString("String", "YourHusbandNotOnlineMsg", Settings.YourHusbandNotOnlineMsg);
            Settings.YourWifeNowLocateMsg = ReadWriteString("String", "YourWifeNowLocateMsg", Settings.YourWifeNowLocateMsg);
            Settings.YourHusbandSearchLocateMsg = ReadWriteString("String", "YourHusbandSearchLocateMsg", Settings.YourHusbandSearchLocateMsg);
            Settings.YourHusbandNowLocateMsg = ReadWriteString("String", "YourHusbandNowLocateMsg", Settings.YourHusbandNowLocateMsg);
            Settings.YourWifeSearchLocateMsg = ReadWriteString("String", "YourWifeSearchLocateMsg", Settings.YourWifeSearchLocateMsg);
            Settings.fUnMasterLoginMsg = ReadWriteString("String", "FUnMasterLoginMsg", Settings.fUnMasterLoginMsg);
            Settings.fUnMasterListLoginMsg = ReadWriteString("String", "UnMasterListLoginMsg", Settings.fUnMasterListLoginMsg);
            Settings.MasterListOnlineSelfMsg = ReadWriteString("String", "MasterListOnlineSelfMsg", Settings.MasterListOnlineSelfMsg);
            Settings.MasterListOnlineMasterMsg = ReadWriteString("String", "MasterListOnlineMasterMsg", Settings.MasterListOnlineMasterMsg);
            Settings.MasterOnlineSelfMsg = ReadWriteString("String", "MasterOnlineSelfMsg", Settings.MasterOnlineSelfMsg);
            Settings.MasterOnlineMasterListMsg = ReadWriteString("String", "MasterOnlineMasterListMsg", Settings.MasterOnlineMasterListMsg);
            Settings.MasterLongOutMasterListOnlineMsg = ReadWriteString("String", "MasterLongOutMasterListOnlineMsg", Settings.MasterLongOutMasterListOnlineMsg);
            Settings.MasterListLongOutMasterOnlineMsg = ReadWriteString("String", "MasterListLongOutMasterOnlineMsg", Settings.MasterListLongOutMasterOnlineMsg);
            Settings.MasterListNotOnlineMsg = ReadWriteString("String", "MasterListNotOnlineMsg", Settings.MasterListNotOnlineMsg);
            Settings.MasterNotOnlineMsg = ReadWriteString("String", "MasterNotOnlineMsg", Settings.MasterNotOnlineMsg);
            Settings.YouAreNotMasterMsg = ReadWriteString("String", "YouAreNotMasterMsg", Settings.YouAreNotMasterMsg);
            Settings.YourMasterNotOnlineMsg = ReadWriteString("String", "YourMasterNotOnlineMsg", Settings.YourMasterNotOnlineMsg);
            Settings.YourMasterListNotOnlineMsg = ReadWriteString("String", "YourMasterListNotOnlineMsg", Settings.YourMasterListNotOnlineMsg);
            Settings.YourMasterNowLocateMsg = ReadWriteString("String", "YourMasterNowLocateMsg", Settings.YourMasterNowLocateMsg);
            Settings.YourMasterListSearchLocateMsg = ReadWriteString("String", "YourMasterListSearchLocateMsg", Settings.YourMasterListSearchLocateMsg);
            Settings.YourMasterListNowLocateMsg = ReadWriteString("String", "YourMasterListNowLocateMsg", Settings.YourMasterListNowLocateMsg);
            Settings.YourMasterSearchLocateMsg = ReadWriteString("String", "YourMasterSearchLocateMsg", Settings.YourMasterSearchLocateMsg);
            Settings.YourMasterListUnMasterOKMsg = ReadWriteString("String", "YourMasterListUnMasterOKMsg",  Settings.YourMasterListUnMasterOKMsg);
            Settings.YouAreUnMasterOKMsg = ReadWriteString("String", "YouAreUnMasterOKMsg", Settings.YouAreUnMasterOKMsg);
            Settings.UnMasterLoginMsg = ReadWriteString("String", "UnMasterLoginMsg", Settings.UnMasterLoginMsg);
            Settings.NPCSayUnMasterOKMsg = ReadWriteString("String", "NPCSayUnMasterOKMsg", Settings.NPCSayUnMasterOKMsg);
            Settings.NPCSayForceUnMasterMsg = ReadWriteString("String", "NPCSayForceUnMasterMsg", Settings.NPCSayForceUnMasterMsg);
            
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