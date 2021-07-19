using System.IO;
using mSystemModule;

namespace M2Server.Configs
{
    public class ServerConfig
    {
        public readonly IniFile Config;
        public readonly IniFile ExpConf;
        public readonly IniFile StringConf;

        public ServerConfig()
        {
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix && System.Environment.OSVersion.Version.Major>=11)
            {
                StringConf = new IniFile(Path.Combine("/Volumes/Data/MirServer/Mir200", M2Share.sStringFileName));
                Config = new IniFile(Path.Combine("/Volumes/Data/MirServer/Mir200", M2Share.sConfigFileName));
                ExpConf = new IniFile(Path.Combine("/Volumes/Data/MirServer/Mir200", M2Share.sExpConfigFileName));
            }
            else if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                StringConf = new IniFile(Path.Combine("/opt/MirServer/Mir200", M2Share.sStringFileName));
                Config = new IniFile(Path.Combine("/opt/MirServer/Mir200", M2Share.sConfigFileName));
                ExpConf = new IniFile(Path.Combine("/opt/MirServer/Mir200", M2Share.sExpConfigFileName));
            }
            else
            {
                StringConf = new IniFile(M2Share.sStringFileName);
                Config = new IniFile(M2Share.sConfigFileName);
                ExpConf = new IniFile(M2Share.sExpConfigFileName);
            }
            Config.Load();
            StringConf.Load();
            ExpConf.Load();
        }

        public void LoadConfig()
        {
            var nLoadInteger = 0;
            var sLoadString = string.Empty;
            if (StringConf.ReadString("Guild", "GuildNotice", "") == "")
                StringConf.WriteString("Guild", "GuildNotice", M2Share.g_Config.sGuildNotice);
            M2Share.g_Config.sGuildNotice =
                StringConf.ReadString("Guild", "GuildNotice", M2Share.g_Config.sGuildNotice);
            if (StringConf.ReadString("Guild", "GuildWar", "") == "")
                StringConf.WriteString("Guild", "GuildWar", M2Share.g_Config.sGuildWar);
            M2Share.g_Config.sGuildWar = StringConf.ReadString("Guild", "GuildWar", M2Share.g_Config.sGuildWar);
            if (StringConf.ReadString("Guild", "GuildAll", "") == "")
                StringConf.WriteString("Guild", "GuildAll", M2Share.g_Config.sGuildAll);
            M2Share.g_Config.sGuildAll = StringConf.ReadString("Guild", "GuildAll", M2Share.g_Config.sGuildAll);
            if (StringConf.ReadString("Guild", "GuildMember", "") == "")
                StringConf.WriteString("Guild", "GuildMember", M2Share.g_Config.sGuildMember);
            M2Share.g_Config.sGuildMember =
                StringConf.ReadString("Guild", "GuildMember", M2Share.g_Config.sGuildMember);
            if (StringConf.ReadString("Guild", "GuildMemberRank", "") == "")
                StringConf.WriteString("Guild", "GuildMemberRank", M2Share.g_Config.sGuildMemberRank);
            M2Share.g_Config.sGuildMemberRank =
                StringConf.ReadString("Guild", "GuildMemberRank", M2Share.g_Config.sGuildMemberRank);
            if (StringConf.ReadString("Guild", "GuildChief", "") == "")
                StringConf.WriteString("Guild", "GuildChief", M2Share.g_Config.sGuildChief);
            M2Share.g_Config.sGuildChief = StringConf.ReadString("Guild", "GuildChief", M2Share.g_Config.sGuildChief);
            // 服务器设置
            if (Config.ReadInteger("Server", "ServerIndex", -1) < 0)
                Config.WriteInteger("Server", "ServerIndex", M2Share.nServerIndex);
            M2Share.nServerIndex = Config.ReadInteger("Server", "ServerIndex", M2Share.nServerIndex);
            if (Config.ReadString("Server", "ServerName", "") == "")
                Config.WriteString("Server", "ServerName", M2Share.g_Config.sServerName);
            M2Share.g_Config.sServerName = Config.ReadString("Server", "ServerName", M2Share.g_Config.sServerName);
            if (StringConf.ReadString("Server", "ServerIP", "") == "")
                StringConf.WriteString("Server", "ServerIP", M2Share.g_Config.sServerIPaddr);
            M2Share.g_Config.sServerIPaddr =
                StringConf.ReadString("Server", "ServerIP", M2Share.g_Config.sServerIPaddr);
            if (StringConf.ReadString("Server", "WebSite", "") == "")
                StringConf.WriteString("Server", "WebSite", M2Share.g_Config.sWebSite);
            M2Share.g_Config.sWebSite = StringConf.ReadString("Server", "WebSite", M2Share.g_Config.sWebSite);
            if (StringConf.ReadString("Server", "BbsSite", "") == "")
                StringConf.WriteString("Server", "BbsSite", M2Share.g_Config.sBbsSite);
            M2Share.g_Config.sBbsSite = StringConf.ReadString("Server", "BbsSite", M2Share.g_Config.sBbsSite);
            if (StringConf.ReadString("Server", "ClientDownload", "") == "")
                StringConf.WriteString("Server", "ClientDownload", M2Share.g_Config.sClientDownload);
            M2Share.g_Config.sClientDownload =
                StringConf.ReadString("Server", "ClientDownload", M2Share.g_Config.sClientDownload);
            if (StringConf.ReadString("Server", "QQ", "") == "")
                StringConf.WriteString("Server", "QQ", M2Share.g_Config.sQQ);
            M2Share.g_Config.sQQ = StringConf.ReadString("Server", "QQ", M2Share.g_Config.sQQ);
            if (StringConf.ReadString("Server", "Phone", "") == "")
                StringConf.WriteString("Server", "Phone", M2Share.g_Config.sPhone);
            M2Share.g_Config.sPhone = StringConf.ReadString("Server", "Phone", M2Share.g_Config.sPhone);
            if (StringConf.ReadString("Server", "BankAccount0", "") == "")
                StringConf.WriteString("Server", "BankAccount0", M2Share.g_Config.sBankAccount0);
            M2Share.g_Config.sBankAccount0 =
                StringConf.ReadString("Server", "BankAccount0", M2Share.g_Config.sBankAccount0);
            if (StringConf.ReadString("Server", "BankAccount1", "") == "")
                StringConf.WriteString("Server", "BankAccount1", M2Share.g_Config.sBankAccount1);
            M2Share.g_Config.sBankAccount1 =
                StringConf.ReadString("Server", "BankAccount1", M2Share.g_Config.sBankAccount1);
            if (StringConf.ReadString("Server", "BankAccount2", "") == "")
                StringConf.WriteString("Server", "BankAccount2", M2Share.g_Config.sBankAccount2);
            M2Share.g_Config.sBankAccount2 =
                StringConf.ReadString("Server", "BankAccount2", M2Share.g_Config.sBankAccount2);
            if (StringConf.ReadString("Server", "BankAccount3", "") == "")
                StringConf.WriteString("Server", "BankAccount3", M2Share.g_Config.sBankAccount3);
            M2Share.g_Config.sBankAccount3 =
                StringConf.ReadString("Server", "BankAccount3", M2Share.g_Config.sBankAccount3);
            if (StringConf.ReadString("Server", "BankAccount4", "") == "")
                StringConf.WriteString("Server", "BankAccount4", M2Share.g_Config.sBankAccount4);
            M2Share.g_Config.sBankAccount4 =
                StringConf.ReadString("Server", "BankAccount4", M2Share.g_Config.sBankAccount4);
            if (StringConf.ReadString("Server", "BankAccount5", "") == "")
                StringConf.WriteString("Server", "BankAccount5", M2Share.g_Config.sBankAccount5);
            M2Share.g_Config.sBankAccount5 =
                StringConf.ReadString("Server", "BankAccount5", M2Share.g_Config.sBankAccount5);
            if (StringConf.ReadString("Server", "BankAccount6", "") == "")
                StringConf.WriteString("Server", "BankAccount6", M2Share.g_Config.sBankAccount6);
            M2Share.g_Config.sBankAccount6 =
                StringConf.ReadString("Server", "BankAccount6", M2Share.g_Config.sBankAccount6);
            if (StringConf.ReadString("Server", "BankAccount7", "") == "")
                StringConf.WriteString("Server", "BankAccount7", M2Share.g_Config.sBankAccount7);
            M2Share.g_Config.sBankAccount7 =
                StringConf.ReadString("Server", "BankAccount7", M2Share.g_Config.sBankAccount7);
            if (StringConf.ReadString("Server", "BankAccount8", "") == "")
                StringConf.WriteString("Server", "BankAccount8", M2Share.g_Config.sBankAccount8);
            M2Share.g_Config.sBankAccount8 =
                StringConf.ReadString("Server", "BankAccount8", M2Share.g_Config.sBankAccount8);
            if (StringConf.ReadString("Server", "BankAccount9", "") == "")
                StringConf.WriteString("Server", "BankAccount9", M2Share.g_Config.sBankAccount9);
            M2Share.g_Config.sBankAccount9 =
                StringConf.ReadString("Server", "BankAccount9", M2Share.g_Config.sBankAccount9);
            if (Config.ReadInteger("Server", "ServerNumber", -1) < 0)
                Config.WriteInteger("Server", "ServerNumber", M2Share.g_Config.nServerNumber);
            M2Share.g_Config.nServerNumber =
                Config.ReadInteger("Server", "ServerNumber", M2Share.g_Config.nServerNumber);
            if (Config.ReadString("Server", "VentureServer", "") == "")
                Config.WriteString("Server", "VentureServer", HUtil32.BoolToStr(M2Share.g_Config.boVentureServer));
            M2Share.g_Config.boVentureServer = Config.ReadString("Server", "VentureServer", "FALSE").ToLower()
                                                   .CompareTo("TRUE".ToLower()) == 0;
            if (Config.ReadString("Server", "TestServer", "") == "")
                Config.WriteString("Server", "TestServer", HUtil32.BoolToStr(M2Share.g_Config.boTestServer));
            M2Share.g_Config.boTestServer =
                Config.ReadString("Server", "TestServer", "FALSE").ToLower().CompareTo("TRUE".ToLower()) == 0;
            if (Config.ReadInteger("Server", "TestLevel", -1) < 0)
                Config.WriteInteger("Server", "TestLevel", M2Share.g_Config.nTestLevel);
            M2Share.g_Config.nTestLevel = Config.ReadInteger("Server", "TestLevel", M2Share.g_Config.nTestLevel);
            if (Config.ReadInteger("Server", "TestGold", -1) < 0)
                Config.WriteInteger("Server", "TestGold", M2Share.g_Config.nTestGold);
            M2Share.g_Config.nTestGold = Config.ReadInteger("Server", "TestGold", M2Share.g_Config.nTestGold);
            if (Config.ReadInteger("Server", "TestServerUserLimit", -1) < 0)
                Config.WriteInteger("Server", "TestServerUserLimit", M2Share.g_Config.nTestUserLimit);
            M2Share.g_Config.nTestUserLimit =
                Config.ReadInteger("Server", "TestServerUserLimit", M2Share.g_Config.nTestUserLimit);
            if (Config.ReadString("Server", "ServiceMode", "") == "")
                Config.WriteString("Server", "ServiceMode", HUtil32.BoolToStr(M2Share.g_Config.boServiceMode));
            M2Share.g_Config.boServiceMode =
                Config.ReadString("Server", "ServiceMode", "FALSE").ToLower().CompareTo("TRUE".ToLower()) == 0;
            if (Config.ReadString("Server", "NonPKServer", "") == "")
                Config.WriteString("Server", "NonPKServer", HUtil32.BoolToStr(M2Share.g_Config.boNonPKServer));
            M2Share.g_Config.boNonPKServer =
                Config.ReadString("Server", "NonPKServer", "FALSE").ToLower().CompareTo("TRUE".ToLower()) == 0;
            if (Config.ReadString("Server", "ViewHackMessage", "") == "")
                Config.WriteString("Server", "ViewHackMessage", HUtil32.BoolToStr(M2Share.g_Config.boViewHackMessage));
            M2Share.g_Config.boViewHackMessage = Config.ReadString("Server", "ViewHackMessage", "FALSE").ToLower()
                                                     .CompareTo("TRUE".ToLower()) == 0;
            if (Config.ReadString("Server", "ViewAdmissionFailure", "") == "")
                Config.WriteString("Server", "ViewAdmissionFailure",
                    HUtil32.BoolToStr(M2Share.g_Config.boViewAdmissionFailure));
            M2Share.g_Config.boViewAdmissionFailure = Config.ReadString("Server", "ViewAdmissionFailure", "FALSE")
                                                          .ToLower().CompareTo("TRUE".ToLower()) == 0;
            if (Config.ReadString("Server", "GateAddr", "") == "")
                Config.WriteString("Server", "GateAddr", M2Share.g_Config.sGateAddr);
            M2Share.g_Config.sGateAddr = Config.ReadString("Server", "GateAddr", M2Share.g_Config.sGateAddr);
            if (Config.ReadInteger("Server", "GatePort", -1) < 0)
                Config.WriteInteger("Server", "GatePort", M2Share.g_Config.nGatePort);
            M2Share.g_Config.nGatePort = Config.ReadInteger("Server", "GatePort", M2Share.g_Config.nGatePort);
            if (Config.ReadString("Server", "DBAddr", "") == "")
                Config.WriteString("Server", "DBAddr", M2Share.g_Config.sDBAddr);
            M2Share.g_Config.sDBAddr = Config.ReadString("Server", "DBAddr", M2Share.g_Config.sDBAddr);
            if (Config.ReadInteger("Server", "DBPort", -1) < 0)
                Config.WriteInteger("Server", "DBPort", M2Share.g_Config.nDBPort);
            M2Share.g_Config.nDBPort = Config.ReadInteger("Server", "DBPort", M2Share.g_Config.nDBPort);
            if (Config.ReadString("Server", "IDSAddr", "") == "")
                Config.WriteString("Server", "IDSAddr", M2Share.g_Config.sIDSAddr);
            M2Share.g_Config.sIDSAddr = Config.ReadString("Server", "IDSAddr", M2Share.g_Config.sIDSAddr);
            if (Config.ReadInteger("Server", "IDSPort", -1) < 0)
                Config.WriteInteger("Server", "IDSPort", M2Share.g_Config.nIDSPort);
            M2Share.g_Config.nIDSPort = Config.ReadInteger("Server", "IDSPort", M2Share.g_Config.nIDSPort);
            if (Config.ReadString("Server", "MsgSrvAddr", "") == "")
                Config.WriteString("Server", "MsgSrvAddr", M2Share.g_Config.sMsgSrvAddr);
            M2Share.g_Config.sMsgSrvAddr = Config.ReadString("Server", "MsgSrvAddr", M2Share.g_Config.sMsgSrvAddr);
            if (Config.ReadInteger("Server", "MsgSrvPort", -1) < 0)
                Config.WriteInteger("Server", "MsgSrvPort", M2Share.g_Config.nMsgSrvPort);
            M2Share.g_Config.nMsgSrvPort = Config.ReadInteger("Server", "MsgSrvPort", M2Share.g_Config.nMsgSrvPort);
            if (Config.ReadString("Server", "LogServerAddr", "") == "")
                Config.WriteString("Server", "LogServerAddr", M2Share.g_Config.sLogServerAddr);
            M2Share.g_Config.sLogServerAddr =
                Config.ReadString("Server", "LogServerAddr", M2Share.g_Config.sLogServerAddr);
            if (Config.ReadInteger("Server", "LogServerPort", -1) < 0)
                Config.WriteInteger("Server", "LogServerPort", M2Share.g_Config.nLogServerPort);
            M2Share.g_Config.nLogServerPort =
                Config.ReadInteger("Server", "LogServerPort", M2Share.g_Config.nLogServerPort);
            if (Config.ReadString("Server", "DiscountForNightTime", "") == "")
                Config.WriteString("Server", "DiscountForNightTime",
                    HUtil32.BoolToStr(M2Share.g_Config.boDiscountForNightTime));
            M2Share.g_Config.boDiscountForNightTime = Config.ReadString("Server", "DiscountForNightTime", "FALSE")
                                                          .ToLower().CompareTo("TRUE".ToLower()) == 0;
            if (Config.ReadInteger("Server", "HalfFeeStart", -1) < 0)
                Config.WriteInteger("Server", "HalfFeeStart", M2Share.g_Config.nHalfFeeStart);
            M2Share.g_Config.nHalfFeeStart =
                Config.ReadInteger("Server", "HalfFeeStart", M2Share.g_Config.nHalfFeeStart);
            if (Config.ReadInteger("Server", "HalfFeeEnd", -1) < 0)
                Config.WriteInteger("Server", "HalfFeeEnd", M2Share.g_Config.nHalfFeeEnd);
            M2Share.g_Config.nHalfFeeEnd = Config.ReadInteger("Server", "HalfFeeEnd", M2Share.g_Config.nHalfFeeEnd);
            if (Config.ReadInteger("Server", "HumLimit", -1) < 0)
                Config.WriteInteger("Server", "HumLimit", M2Share.g_dwHumLimit);
            M2Share.g_dwHumLimit = Config.ReadInteger("Server", "HumLimit", M2Share.g_dwHumLimit);
            if (Config.ReadInteger("Server", "MonLimit", -1) < 0)
                Config.WriteInteger("Server", "MonLimit", M2Share.g_dwMonLimit);
            M2Share.g_dwMonLimit = Config.ReadInteger("Server", "MonLimit", M2Share.g_dwMonLimit);
            if (Config.ReadInteger("Server", "ZenLimit", -1) < 0)
                Config.WriteInteger("Server", "ZenLimit", M2Share.g_dwZenLimit);
            M2Share.g_dwZenLimit = Config.ReadInteger("Server", "ZenLimit", M2Share.g_dwZenLimit);
            if (Config.ReadInteger("Server", "NpcLimit", -1) < 0)
                Config.WriteInteger("Server", "NpcLimit", M2Share.g_dwNpcLimit);
            M2Share.g_dwNpcLimit = Config.ReadInteger("Server", "NpcLimit", M2Share.g_dwNpcLimit);
            if (Config.ReadInteger("Server", "SocLimit", -1) < 0)
                Config.WriteInteger("Server", "SocLimit", M2Share.g_dwSocLimit);
            M2Share.g_dwSocLimit = Config.ReadInteger("Server", "SocLimit", M2Share.g_dwSocLimit);
            if (Config.ReadInteger("Server", "DecLimit", -1) < 0)
                Config.WriteInteger("Server", "DecLimit", M2Share.nDecLimit);
            M2Share.nDecLimit = Config.ReadInteger("Server", "DecLimit", M2Share.nDecLimit);
            if (Config.ReadInteger("Server", "SendBlock", -1) < 0)
                Config.WriteInteger("Server", "SendBlock", M2Share.g_Config.nSendBlock);
            M2Share.g_Config.nSendBlock = Config.ReadInteger("Server", "SendBlock", M2Share.g_Config.nSendBlock);
            if (Config.ReadInteger("Server", "CheckBlock", -1) < 0)
                Config.WriteInteger("Server", "CheckBlock", M2Share.g_Config.nCheckBlock);
            M2Share.g_Config.nCheckBlock = Config.ReadInteger("Server", "CheckBlock", M2Share.g_Config.nCheckBlock);
            if (Config.ReadInteger("Server", "SocCheckTimeOut", -1) < 0)
                Config.WriteInteger("Server", "SocCheckTimeOut", M2Share.g_dwSocCheckTimeOut);
            M2Share.g_dwSocCheckTimeOut = Config.ReadInteger("Server", "SocCheckTimeOut", M2Share.g_dwSocCheckTimeOut);
            if (Config.ReadInteger("Server", "AvailableBlock", -1) < 0)
                Config.WriteInteger("Server", "AvailableBlock", M2Share.g_Config.nAvailableBlock);
            M2Share.g_Config.nAvailableBlock =
                Config.ReadInteger("Server", "AvailableBlock", M2Share.g_Config.nAvailableBlock);
            if (Config.ReadInteger("Server", "GateLoad", -1) < 0)
                Config.WriteInteger("Server", "GateLoad", M2Share.g_Config.nGateLoad);
            M2Share.g_Config.nGateLoad = Config.ReadInteger("Server", "GateLoad", M2Share.g_Config.nGateLoad);
            if (Config.ReadInteger("Server", "UserFull", -1) < 0)
                Config.WriteInteger("Server", "UserFull", M2Share.g_Config.nUserFull);
            M2Share.g_Config.nUserFull = Config.ReadInteger("Server", "UserFull", M2Share.g_Config.nUserFull);
            if (Config.ReadInteger("Server", "ZenFastStep", -1) < 0)
                Config.WriteInteger("Server", "ZenFastStep", M2Share.g_Config.nZenFastStep);
            M2Share.g_Config.nZenFastStep = Config.ReadInteger("Server", "ZenFastStep", M2Share.g_Config.nZenFastStep);
            if (Config.ReadInteger("Server", "ProcessMonstersTime", -1) < 0)
                Config.WriteInteger("Server", "ProcessMonstersTime", M2Share.g_Config.dwProcessMonstersTime);
            M2Share.g_Config.dwProcessMonstersTime = Config.ReadInteger("Server", "ProcessMonstersTime",
                M2Share.g_Config.dwProcessMonstersTime);
            if (Config.ReadInteger("Server", "RegenMonstersTime", -1) < 0)
                Config.WriteInteger("Server", "RegenMonstersTime", M2Share.g_Config.dwRegenMonstersTime);
            M2Share.g_Config.dwRegenMonstersTime =
                Config.ReadInteger("Server", "RegenMonstersTime", M2Share.g_Config.dwRegenMonstersTime);
            if (Config.ReadInteger("Server", "HumanGetMsgTimeLimit", -1) < 0)
                Config.WriteInteger("Server", "HumanGetMsgTimeLimit", M2Share.g_Config.dwHumanGetMsgTime);
            M2Share.g_Config.dwHumanGetMsgTime =
                Config.ReadInteger("Server", "HumanGetMsgTimeLimit", M2Share.g_Config.dwHumanGetMsgTime);

            // ============================================================================
            // 目录设置
            if (Config.ReadString("Share", "BaseDir", "") == "")
                Config.WriteString("Share", "BaseDir", M2Share.g_Config.sBaseDir);
            M2Share.g_Config.sBaseDir = Config.ReadString("Share", "BaseDir", M2Share.g_Config.sBaseDir);
            if (Config.ReadString("Share", "GuildDir", "") == "")
                Config.WriteString("Share", "GuildDir", M2Share.g_Config.sGuildDir);
            M2Share.g_Config.sGuildDir = Config.ReadString("Share", "GuildDir", M2Share.g_Config.sGuildDir);
            if (Config.ReadString("Share", "GuildFile", "") == "")
                Config.WriteString("Share", "GuildFile", M2Share.g_Config.sGuildFile);
            M2Share.g_Config.sGuildFile = Config.ReadString("Share", "GuildFile", M2Share.g_Config.sGuildFile);
            M2Share.g_Config.sGuildFile = Path.Combine(M2Share.g_Config.sBaseDir, M2Share.g_Config.sGuildFile);
            if (Config.ReadString("Share", "VentureDir", "") == "")
                Config.WriteString("Share", "VentureDir", M2Share.g_Config.sVentureDir);
            M2Share.g_Config.sVentureDir = Config.ReadString("Share", "VentureDir", M2Share.g_Config.sVentureDir);
            if (Config.ReadString("Share", "ConLogDir", "") == "")
                Config.WriteString("Share", "ConLogDir", M2Share.g_Config.sConLogDir);
            M2Share.g_Config.sConLogDir = Config.ReadString("Share", "ConLogDir", M2Share.g_Config.sConLogDir);
            if (Config.ReadString("Share", "CastleDir", "") == "")
                Config.WriteString("Share", "CastleDir", M2Share.g_Config.sCastleDir);
            M2Share.g_Config.sCastleDir = Config.ReadString("Share", "CastleDir", M2Share.g_Config.sCastleDir);
            if (Config.ReadString("Share", "CastleFile", "") == "")
                Config.WriteString("Share", "CastleFile", M2Share.g_Config.sCastleDir + "List.txt");
            M2Share.g_Config.sCastleFile = Config.ReadString("Share", "CastleFile", M2Share.g_Config.sCastleFile);
            M2Share.g_Config.sCastleFile = Path.Combine(M2Share.g_Config.sBaseDir, M2Share.g_Config.sCastleFile);

            if (Config.ReadString("Share", "EnvirDir", "") == "")
            {
                Config.WriteString("Share", "EnvirDir", M2Share.g_Config.sEnvirDir);
            }
            M2Share.g_Config.sEnvirDir = Config.ReadString("Share", "EnvirDir", M2Share.g_Config.sEnvirDir);
            M2Share.g_Config.sEnvirDir = Path.Combine(M2Share.g_Config.sBaseDir, M2Share.g_Config.sEnvirDir);
            if (Config.ReadString("Share", "MapDir", "") == "")
                Config.WriteString("Share", "MapDir", M2Share.g_Config.sMapDir);
            M2Share.g_Config.sMapDir = Config.ReadString("Share", "MapDir", M2Share.g_Config.sMapDir);
            M2Share.g_Config.sMapDir = Path.Combine(M2Share.g_Config.sBaseDir, M2Share.g_Config.sMapDir);
            if (Config.ReadString("Share", "NoticeDir", "") == "")
                Config.WriteString("Share", "NoticeDir", M2Share.g_Config.sNoticeDir);
            M2Share.g_Config.sNoticeDir = Config.ReadString("Share", "NoticeDir", M2Share.g_Config.sNoticeDir);
            M2Share.g_Config.sNoticeDir = Path.Combine(M2Share.g_Config.sBaseDir, M2Share.g_Config.sNoticeDir);
            sLoadString = Config.ReadString("Share", "LogDir", "");
            if (sLoadString == "")
                Config.WriteString("Share", "LogDir", M2Share.g_Config.sLogDir);
            else
                M2Share.g_Config.sLogDir = sLoadString;
            if (Config.ReadString("Share", "PlugDir", "") == "")
                Config.WriteString("Share", "PlugDir", M2Share.g_Config.sPlugDir);
            M2Share.g_Config.sPlugDir = Config.ReadString("Share", "PlugDir", M2Share.g_Config.sPlugDir);
            // ============================================================================
            // 名称设置
            if (Config.ReadString("Names", "HealSkill", "") == "")
                Config.WriteString("Names", "HealSkill", M2Share.g_Config.sHealSkill);
            M2Share.g_Config.sHealSkill = Config.ReadString("Names", "HealSkill", M2Share.g_Config.sHealSkill);
            if (Config.ReadString("Names", "FireBallSkill", "") == "")
                Config.WriteString("Names", "FireBallSkill", M2Share.g_Config.sFireBallSkill);
            M2Share.g_Config.sFireBallSkill =
                Config.ReadString("Names", "FireBallSkill", M2Share.g_Config.sFireBallSkill);
            if (Config.ReadString("Names", "ClothsMan", "") == "")
                Config.WriteString("Names", "ClothsMan", M2Share.g_Config.sClothsMan);
            M2Share.g_Config.sClothsMan = Config.ReadString("Names", "ClothsMan", M2Share.g_Config.sClothsMan);
            if (Config.ReadString("Names", "ClothsWoman", "") == "")
                Config.WriteString("Names", "ClothsWoman", M2Share.g_Config.sClothsWoman);
            M2Share.g_Config.sClothsWoman = Config.ReadString("Names", "ClothsWoman", M2Share.g_Config.sClothsWoman);
            if (Config.ReadString("Names", "WoodenSword", "") == "")
                Config.WriteString("Names", "WoodenSword", M2Share.g_Config.sWoodenSword);
            M2Share.g_Config.sWoodenSword = Config.ReadString("Names", "WoodenSword", M2Share.g_Config.sWoodenSword);
            if (Config.ReadString("Names", "Candle", "") == "")
                Config.WriteString("Names", "Candle", M2Share.g_Config.sCandle);
            M2Share.g_Config.sCandle = Config.ReadString("Names", "Candle", M2Share.g_Config.sCandle);
            if (Config.ReadString("Names", "BasicDrug", "") == "")
                Config.WriteString("Names", "BasicDrug", M2Share.g_Config.sBasicDrug);
            M2Share.g_Config.sBasicDrug = Config.ReadString("Names", "BasicDrug", M2Share.g_Config.sBasicDrug);
            if (Config.ReadString("Names", "GoldStone", "") == "")
                Config.WriteString("Names", "GoldStone", M2Share.g_Config.sGoldStone);
            M2Share.g_Config.sGoldStone = Config.ReadString("Names", "GoldStone", M2Share.g_Config.sGoldStone);
            if (Config.ReadString("Names", "SilverStone", "") == "")
                Config.WriteString("Names", "SilverStone", M2Share.g_Config.sSilverStone);
            M2Share.g_Config.sSilverStone = Config.ReadString("Names", "SilverStone", M2Share.g_Config.sSilverStone);
            if (Config.ReadString("Names", "SteelStone", "") == "")
                Config.WriteString("Names", "SteelStone", M2Share.g_Config.sSteelStone);
            M2Share.g_Config.sSteelStone = Config.ReadString("Names", "SteelStone", M2Share.g_Config.sSteelStone);
            if (Config.ReadString("Names", "CopperStone", "") == "")
                Config.WriteString("Names", "CopperStone", M2Share.g_Config.sCopperStone);
            M2Share.g_Config.sCopperStone = Config.ReadString("Names", "CopperStone", M2Share.g_Config.sCopperStone);
            if (Config.ReadString("Names", "BlackStone", "") == "")
                Config.WriteString("Names", "BlackStone", M2Share.g_Config.sBlackStone);
            M2Share.g_Config.sBlackStone = Config.ReadString("Names", "BlackStone", M2Share.g_Config.sBlackStone);
            if (Config.ReadString("Names", "Gem1Stone", "") == "")
                Config.WriteString("Names", "Gem1Stone", M2Share.g_Config.sGemStone1);
            M2Share.g_Config.sGemStone1 = Config.ReadString("Names", "Gem1Stone", M2Share.g_Config.sGemStone1);
            if (Config.ReadString("Names", "Gem2Stone", "") == "")
                Config.WriteString("Names", "Gem2Stone", M2Share.g_Config.sGemStone2);
            M2Share.g_Config.sGemStone2 = Config.ReadString("Names", "Gem2Stone", M2Share.g_Config.sGemStone2);
            if (Config.ReadString("Names", "Gem3Stone", "") == "")
                Config.WriteString("Names", "Gem3Stone", M2Share.g_Config.sGemStone3);
            M2Share.g_Config.sGemStone3 = Config.ReadString("Names", "Gem3Stone", M2Share.g_Config.sGemStone3);
            if (Config.ReadString("Names", "Gem4Stone", "") == "")
                Config.WriteString("Names", "Gem4Stone", M2Share.g_Config.sGemStone4);
            M2Share.g_Config.sGemStone4 = Config.ReadString("Names", "Gem4Stone", M2Share.g_Config.sGemStone4);
            if (Config.ReadString("Names", "Zuma1", "") == "")
                Config.WriteString("Names", "Zuma1", M2Share.g_Config.sZuma[0]);
            M2Share.g_Config.sZuma[0] = Config.ReadString("Names", "Zuma1", M2Share.g_Config.sZuma[0]);
            if (Config.ReadString("Names", "Zuma2", "") == "")
                Config.WriteString("Names", "Zuma2", M2Share.g_Config.sZuma[1]);
            M2Share.g_Config.sZuma[1] = Config.ReadString("Names", "Zuma2", M2Share.g_Config.sZuma[1]);
            if (Config.ReadString("Names", "Zuma3", "") == "")
                Config.WriteString("Names", "Zuma3", M2Share.g_Config.sZuma[2]);
            M2Share.g_Config.sZuma[2] = Config.ReadString("Names", "Zuma3", M2Share.g_Config.sZuma[2]);
            if (Config.ReadString("Names", "Zuma4", "") == "")
                Config.WriteString("Names", "Zuma4", M2Share.g_Config.sZuma[3]);
            M2Share.g_Config.sZuma[3] = Config.ReadString("Names", "Zuma4", M2Share.g_Config.sZuma[3]);
            if (Config.ReadString("Names", "Bee", "") == "") Config.WriteString("Names", "Bee", M2Share.g_Config.sBee);
            M2Share.g_Config.sBee = Config.ReadString("Names", "Bee", M2Share.g_Config.sBee);
            if (Config.ReadString("Names", "Spider", "") == "")
                Config.WriteString("Names", "Spider", M2Share.g_Config.sSpider);
            M2Share.g_Config.sSpider = Config.ReadString("Names", "Spider", M2Share.g_Config.sSpider);
            if (Config.ReadString("Names", "WomaHorn", "") == "")
                Config.WriteString("Names", "WomaHorn", M2Share.g_Config.sWomaHorn);
            M2Share.g_Config.sWomaHorn = Config.ReadString("Names", "WomaHorn", M2Share.g_Config.sWomaHorn);
            if (Config.ReadString("Names", "ZumaPiece", "") == "")
                Config.WriteString("Names", "ZumaPiece", M2Share.g_Config.sZumaPiece);
            M2Share.g_Config.sZumaPiece = Config.ReadString("Names", "ZumaPiece", M2Share.g_Config.sZumaPiece);
            if (Config.ReadString("Names", "Skeleton", "") == "")
                Config.WriteString("Names", "Skeleton", M2Share.g_Config.sSkeleton);
            M2Share.g_Config.sSkeleton = Config.ReadString("Names", "Skeleton", M2Share.g_Config.sSkeleton);
            if (Config.ReadString("Names", "Dragon", "") == "")
                Config.WriteString("Names", "Dragon", M2Share.g_Config.sDragon);
            M2Share.g_Config.sDragon = Config.ReadString("Names", "Dragon", M2Share.g_Config.sDragon);
            if (Config.ReadString("Names", "Dragon1", "") == "")
                Config.WriteString("Names", "Dragon1", M2Share.g_Config.sDragon1);
            M2Share.g_Config.sDragon1 = Config.ReadString("Names", "Dragon1", M2Share.g_Config.sDragon1);
            if (Config.ReadString("Names", "Angel", "") == "")
                Config.WriteString("Names", "Angel", M2Share.g_Config.sAngel);
            M2Share.g_Config.sAngel = Config.ReadString("Names", "Angel", M2Share.g_Config.sAngel);
            sLoadString = Config.ReadString("Names", "GameGold", "");
            if (sLoadString == "")
                Config.WriteString("Share", "GameGold", M2Share.g_Config.sGameGoldName);
            else
                M2Share.g_Config.sGameGoldName = sLoadString;
            sLoadString = Config.ReadString("Names", "GamePoint", "");
            if (sLoadString == "")
                Config.WriteString("Share", "GamePoint", M2Share.g_Config.sGamePointName);
            else
                M2Share.g_Config.sGamePointName = sLoadString;
            sLoadString = Config.ReadString("Names", "PayMentPointName", "");
            if (sLoadString == "")
                Config.WriteString("Share", "PayMentPointName", M2Share.g_Config.sPayMentPointName);
            else
                M2Share.g_Config.sPayMentPointName = sLoadString;
            if (M2Share.g_Config.nAppIconCrc != 1242102148) M2Share.g_Config.boCheckFail = true;
            // ============================================================================
            // 游戏设置
            if (Config.ReadInteger("Setup", "ItemNumber", -1) < 0)
                Config.WriteInteger("Setup", "ItemNumber", M2Share.g_Config.nItemNumber);
            M2Share.g_Config.nItemNumber = Config.ReadInteger("Setup", "ItemNumber", M2Share.g_Config.nItemNumber);
            M2Share.g_Config.nItemNumber = M2Share.g_Config.nItemNumber + M2Share.RandomNumber.Random(10000);
            if (Config.ReadInteger("Setup", "ItemNumberEx", -1) < 0)
                Config.WriteInteger("Setup", "ItemNumberEx", M2Share.g_Config.nItemNumberEx);
            M2Share.g_Config.nItemNumberEx =
                Config.ReadInteger("Setup", "ItemNumberEx", M2Share.g_Config.nItemNumberEx);
            if (Config.ReadString("Setup", "ClientFile1", "") == "")
                Config.WriteString("Setup", "ClientFile1", M2Share.g_Config.sClientFile1);
            M2Share.g_Config.sClientFile1 = Config.ReadString("Setup", "ClientFile1", M2Share.g_Config.sClientFile1);
            if (Config.ReadString("Setup", "ClientFile2", "") == "")
                Config.WriteString("Setup", "ClientFile2", M2Share.g_Config.sClientFile2);
            M2Share.g_Config.sClientFile2 = Config.ReadString("Setup", "ClientFile2", M2Share.g_Config.sClientFile2);
            if (Config.ReadString("Setup", "ClientFile3", "") == "")
                Config.WriteString("Setup", "ClientFile3", M2Share.g_Config.sClientFile3);
            M2Share.g_Config.sClientFile3 = Config.ReadString("Setup", "ClientFile3", M2Share.g_Config.sClientFile3);
            if (Config.ReadInteger("Setup", "MonUpLvNeedKillBase", -1) < 0)
                Config.WriteInteger("Setup", "MonUpLvNeedKillBase", M2Share.g_Config.nMonUpLvNeedKillBase);
            M2Share.g_Config.nMonUpLvNeedKillBase =
                Config.ReadInteger("Setup", "MonUpLvNeedKillBase", M2Share.g_Config.nMonUpLvNeedKillBase);
            if (Config.ReadInteger("Setup", "MonUpLvRate", -1) < 0)
                Config.WriteInteger("Setup", "MonUpLvRate", M2Share.g_Config.nMonUpLvRate);
            M2Share.g_Config.nMonUpLvRate = Config.ReadInteger("Setup", "MonUpLvRate", M2Share.g_Config.nMonUpLvRate);
            for (var I = M2Share.g_Config.MonUpLvNeedKillCount.GetLowerBound(0);
                I <= M2Share.g_Config.MonUpLvNeedKillCount.GetUpperBound(0);
                I++)
            {
                if (Config.ReadInteger("Setup", "MonUpLvNeedKillCount" + I, -1) < 0)
                    Config.WriteInteger("Setup", "MonUpLvNeedKillCount" + I, M2Share.g_Config.MonUpLvNeedKillCount[I]);
                M2Share.g_Config.MonUpLvNeedKillCount[I] = Config.ReadInteger("Setup", "MonUpLvNeedKillCount" + I,
                    M2Share.g_Config.MonUpLvNeedKillCount[I]);
            }

            for (var I = M2Share.g_Config.SlaveColor.GetLowerBound(0);
                I <= M2Share.g_Config.SlaveColor.GetUpperBound(0);
                I++)
            {
                if (Config.ReadInteger("Setup", "SlaveColor" + I, -1) < 0)
                    Config.WriteInteger("Setup", "SlaveColor" + I, M2Share.g_Config.SlaveColor[I]);
                M2Share.g_Config.SlaveColor[I] =
                    Config.ReadInteger<byte>("Setup", "SlaveColor" + I, M2Share.g_Config.SlaveColor[I]);
            }

            if (Config.ReadString("Setup", "HomeMap", "") == "")
                Config.WriteString("Setup", "HomeMap", M2Share.g_Config.sHomeMap);
            M2Share.g_Config.sHomeMap = Config.ReadString("Setup", "HomeMap", M2Share.g_Config.sHomeMap);
            if (Config.ReadInteger("Setup", "HomeX", -1) < 0)
                Config.WriteInteger("Setup", "HomeX", M2Share.g_Config.nHomeX);
            M2Share.g_Config.nHomeX = Config.ReadInteger<short>("Setup", "HomeX", M2Share.g_Config.nHomeX);
            if (Config.ReadInteger("Setup", "HomeY", -1) < 0)
                Config.WriteInteger("Setup", "HomeY", M2Share.g_Config.nHomeY);
            M2Share.g_Config.nHomeY = Config.ReadInteger<short>("Setup", "HomeY", M2Share.g_Config.nHomeY);
            if (Config.ReadString("Setup", "RedHomeMap", "") == "")
                Config.WriteString("Setup", "RedHomeMap", M2Share.g_Config.sRedHomeMap);
            M2Share.g_Config.sRedHomeMap = Config.ReadString("Setup", "RedHomeMap", M2Share.g_Config.sRedHomeMap);
            if (Config.ReadInteger("Setup", "RedHomeX", -1) < 0)
                Config.WriteInteger("Setup", "RedHomeX", M2Share.g_Config.nRedHomeX);
            M2Share.g_Config.nRedHomeX = Config.ReadInteger<short>("Setup", "RedHomeX", M2Share.g_Config.nRedHomeX);
            if (Config.ReadInteger("Setup", "RedHomeY", -1) < 0)
                Config.WriteInteger("Setup", "RedHomeY", M2Share.g_Config.nRedHomeY);
            M2Share.g_Config.nRedHomeY = Config.ReadInteger<short>("Setup", "RedHomeY", M2Share.g_Config.nRedHomeY);
            if (Config.ReadString("Setup", "RedDieHomeMap", "") == "")
                Config.WriteString("Setup", "RedDieHomeMap", M2Share.g_Config.sRedDieHomeMap);
            M2Share.g_Config.sRedDieHomeMap =
                Config.ReadString("Setup", "RedDieHomeMap", M2Share.g_Config.sRedDieHomeMap);
            if (Config.ReadInteger("Setup", "RedDieHomeX", -1) < 0)
                Config.WriteInteger("Setup", "RedDieHomeX", M2Share.g_Config.nRedDieHomeX);
            M2Share.g_Config.nRedDieHomeX = Config.ReadInteger<short>("Setup", "RedDieHomeX", M2Share.g_Config.nRedDieHomeX);
            if (Config.ReadInteger("Setup", "RedDieHomeY", -1) < 0)
                Config.WriteInteger("Setup", "RedDieHomeY", M2Share.g_Config.nRedDieHomeY);
            M2Share.g_Config.nRedDieHomeY = Config.ReadInteger<short>("Setup", "RedDieHomeY", M2Share.g_Config.nRedDieHomeY);
            if (Config.ReadInteger("Setup", "JobHomePointSystem", -1) < 0)
                Config.WriteBool("Setup", "JobHomePointSystem", M2Share.g_Config.boJobHomePoint);
            M2Share.g_Config.boJobHomePoint =
                Config.ReadBool("Setup", "JobHomePointSystem", M2Share.g_Config.boJobHomePoint);
            if (Config.ReadString("Setup", "WarriorHomeMap", "") == "")
                Config.WriteString("Setup", "WarriorHomeMap", M2Share.g_Config.sWarriorHomeMap);
            M2Share.g_Config.sWarriorHomeMap =
                Config.ReadString("Setup", "WarriorHomeMap", M2Share.g_Config.sWarriorHomeMap);
            if (Config.ReadInteger("Setup", "WarriorHomeX", -1) < 0)
                Config.WriteInteger("Setup", "WarriorHomeX", M2Share.g_Config.nWarriorHomeX);
            M2Share.g_Config.nWarriorHomeX =
                Config.ReadInteger<short>("Setup", "WarriorHomeX", M2Share.g_Config.nWarriorHomeX);
            if (Config.ReadInteger("Setup", "WarriorHomeY", -1) < 0)
                Config.WriteInteger("Setup", "WarriorHomeY", M2Share.g_Config.nWarriorHomeY);
            M2Share.g_Config.nWarriorHomeY =
                Config.ReadInteger<short>("Setup", "WarriorHomeY", M2Share.g_Config.nWarriorHomeY);
            if (Config.ReadString("Setup", "WizardHomeMap", "") == "")
                Config.WriteString("Setup", "WizardHomeMap", M2Share.g_Config.sWizardHomeMap);
            M2Share.g_Config.sWizardHomeMap =
                Config.ReadString("Setup", "WizardHomeMap", M2Share.g_Config.sWizardHomeMap);
            if (Config.ReadInteger("Setup", "WizardHomeX", -1) < 0)
                Config.WriteInteger("Setup", "WizardHomeX", M2Share.g_Config.nWizardHomeX);
            M2Share.g_Config.nWizardHomeX = Config.ReadInteger<short>("Setup", "WizardHomeX", M2Share.g_Config.nWizardHomeX);
            if (Config.ReadInteger("Setup", "WizardHomeY", -1) < 0)
                Config.WriteInteger("Setup", "WizardHomeY", M2Share.g_Config.nWizardHomeY);
            M2Share.g_Config.nWizardHomeY = Config.ReadInteger<short>("Setup", "WizardHomeY", M2Share.g_Config.nWizardHomeY);
            if (Config.ReadString("Setup", "TaoistHomeMap", "") == "")
                Config.WriteString("Setup", "TaoistHomeMap", M2Share.g_Config.sTaoistHomeMap);
            M2Share.g_Config.sTaoistHomeMap =
                Config.ReadString("Setup", "TaoistHomeMap", M2Share.g_Config.sTaoistHomeMap);
            if (Config.ReadInteger("Setup", "TaoistHomeX", -1) < 0)
                Config.WriteInteger("Setup", "TaoistHomeX", M2Share.g_Config.nTaoistHomeX);
            M2Share.g_Config.nTaoistHomeX = Config.ReadInteger<short>("Setup", "TaoistHomeX", M2Share.g_Config.nTaoistHomeX);
            if (Config.ReadInteger("Setup", "TaoistHomeY", -1) < 0)
                Config.WriteInteger("Setup", "TaoistHomeY", M2Share.g_Config.nTaoistHomeY);
            M2Share.g_Config.nTaoistHomeY = Config.ReadInteger<short>("Setup", "TaoistHomeY", M2Share.g_Config.nTaoistHomeY);
            nLoadInteger = Config.ReadInteger("Setup", "HealthFillTime", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "HealthFillTime", M2Share.g_Config.nHealthFillTime);
            else
                M2Share.g_Config.nHealthFillTime = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "SpellFillTime", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "SpellFillTime", M2Share.g_Config.nSpellFillTime);
            else
                M2Share.g_Config.nSpellFillTime = nLoadInteger;
            if (Config.ReadInteger("Setup", "DecPkPointTime", -1) < 0)
                Config.WriteInteger("Setup", "DecPkPointTime", M2Share.g_Config.dwDecPkPointTime);
            M2Share.g_Config.dwDecPkPointTime =
                Config.ReadInteger("Setup", "DecPkPointTime", M2Share.g_Config.dwDecPkPointTime);
            if (Config.ReadInteger("Setup", "DecPkPointCount", -1) < 0)
                Config.WriteInteger("Setup", "DecPkPointCount", M2Share.g_Config.nDecPkPointCount);
            M2Share.g_Config.nDecPkPointCount =
                Config.ReadInteger("Setup", "DecPkPointCount", M2Share.g_Config.nDecPkPointCount);
            if (Config.ReadInteger("Setup", "PKFlagTime", -1) < 0)
                Config.WriteInteger("Setup", "PKFlagTime", M2Share.g_Config.dwPKFlagTime);
            M2Share.g_Config.dwPKFlagTime = Config.ReadInteger("Setup", "PKFlagTime", M2Share.g_Config.dwPKFlagTime);
            if (Config.ReadInteger("Setup", "KillHumanAddPKPoint", -1) < 0)
                Config.WriteInteger("Setup", "KillHumanAddPKPoint", M2Share.g_Config.nKillHumanAddPKPoint);
            M2Share.g_Config.nKillHumanAddPKPoint =
                Config.ReadInteger("Setup", "KillHumanAddPKPoint", M2Share.g_Config.nKillHumanAddPKPoint);
            if (Config.ReadInteger("Setup", "KillHumanDecLuckPoint", -1) < 0)
                Config.WriteInteger("Setup", "KillHumanDecLuckPoint", M2Share.g_Config.nKillHumanDecLuckPoint);
            M2Share.g_Config.nKillHumanDecLuckPoint = Config.ReadInteger("Setup", "KillHumanDecLuckPoint",
                M2Share.g_Config.nKillHumanDecLuckPoint);
            if (Config.ReadInteger("Setup", "DecLightItemDrugTime", -1) < 0)
                Config.WriteInteger("Setup", "DecLightItemDrugTime", M2Share.g_Config.dwDecLightItemDrugTime);
            M2Share.g_Config.dwDecLightItemDrugTime = Config.ReadInteger("Setup", "DecLightItemDrugTime",
                M2Share.g_Config.dwDecLightItemDrugTime);
            if (Config.ReadInteger("Setup", "SafeZoneSize", -1) < 0)
                Config.WriteInteger("Setup", "SafeZoneSize", M2Share.g_Config.nSafeZoneSize);
            M2Share.g_Config.nSafeZoneSize =
                Config.ReadInteger("Setup", "SafeZoneSize", M2Share.g_Config.nSafeZoneSize);
            if (Config.ReadInteger("Setup", "StartPointSize", -1) < 0)
                Config.WriteInteger("Setup", "StartPointSize", M2Share.g_Config.nStartPointSize);
            M2Share.g_Config.nStartPointSize =
                Config.ReadInteger("Setup", "StartPointSize", M2Share.g_Config.nStartPointSize);
            for (var I = M2Share.g_Config.ReNewNameColor.GetLowerBound(0);
                I <= M2Share.g_Config.ReNewNameColor.GetUpperBound(0);
                I++)
            {
                if (Config.ReadInteger("Setup", "ReNewNameColor" + I, -1) < 0)
                    Config.WriteInteger("Setup", "ReNewNameColor" + I, M2Share.g_Config.ReNewNameColor[I]);
                M2Share.g_Config.ReNewNameColor[I] =
                    Config.ReadInteger<byte>("Setup", "ReNewNameColor" + I, M2Share.g_Config.ReNewNameColor[I]);
            }

            if (Config.ReadInteger("Setup", "ReNewNameColorTime", -1) < 0)
                Config.WriteInteger("Setup", "ReNewNameColorTime", M2Share.g_Config.dwReNewNameColorTime);
            M2Share.g_Config.dwReNewNameColorTime =
                Config.ReadInteger("Setup", "ReNewNameColorTime", M2Share.g_Config.dwReNewNameColorTime);
            if (Config.ReadInteger("Setup", "ReNewChangeColor", -1) < 0)
                Config.WriteBool("Setup", "ReNewChangeColor", M2Share.g_Config.boReNewChangeColor);
            M2Share.g_Config.boReNewChangeColor =
                Config.ReadBool("Setup", "ReNewChangeColor", M2Share.g_Config.boReNewChangeColor);
            if (Config.ReadInteger("Setup", "ReNewLevelClearExp", -1) < 0)
                Config.WriteBool("Setup", "ReNewLevelClearExp", M2Share.g_Config.boReNewLevelClearExp);
            M2Share.g_Config.boReNewLevelClearExp =
                Config.ReadBool("Setup", "ReNewLevelClearExp", M2Share.g_Config.boReNewLevelClearExp);
            if (Config.ReadInteger("Setup", "BonusAbilofWarrDC", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWarrDC", M2Share.g_Config.BonusAbilofWarr.DC);
            M2Share.g_Config.BonusAbilofWarr.DC =
                Config.ReadInteger<short>("Setup", "BonusAbilofWarrDC", M2Share.g_Config.BonusAbilofWarr.DC);
            if (Config.ReadInteger("Setup", "BonusAbilofWarrMC", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWarrMC", M2Share.g_Config.BonusAbilofWarr.MC);
            M2Share.g_Config.BonusAbilofWarr.MC =
                Config.ReadInteger<short>("Setup", "BonusAbilofWarrMC", M2Share.g_Config.BonusAbilofWarr.MC);
            if (Config.ReadInteger("Setup", "BonusAbilofWarrSC", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWarrSC", M2Share.g_Config.BonusAbilofWarr.SC);
            M2Share.g_Config.BonusAbilofWarr.SC =
                Config.ReadInteger<short>("Setup", "BonusAbilofWarrSC", M2Share.g_Config.BonusAbilofWarr.SC);
            if (Config.ReadInteger("Setup", "BonusAbilofWarrAC", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWarrAC", M2Share.g_Config.BonusAbilofWarr.AC);
            M2Share.g_Config.BonusAbilofWarr.AC =
                Config.ReadInteger<short>("Setup", "BonusAbilofWarrAC", M2Share.g_Config.BonusAbilofWarr.AC);
            if (Config.ReadInteger("Setup", "BonusAbilofWarrMAC", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWarrMAC", M2Share.g_Config.BonusAbilofWarr.MAC);
            M2Share.g_Config.BonusAbilofWarr.MAC =
                Config.ReadInteger<short>("Setup", "BonusAbilofWarrMAC", M2Share.g_Config.BonusAbilofWarr.MAC);
            if (Config.ReadInteger("Setup", "BonusAbilofWarrHP", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWarrHP", M2Share.g_Config.BonusAbilofWarr.HP);
            M2Share.g_Config.BonusAbilofWarr.HP =
                Config.ReadInteger<short>("Setup", "BonusAbilofWarrHP", M2Share.g_Config.BonusAbilofWarr.HP);
            if (Config.ReadInteger("Setup", "BonusAbilofWarrMP", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWarrMP", M2Share.g_Config.BonusAbilofWarr.MP);
            M2Share.g_Config.BonusAbilofWarr.MP =
                Config.ReadInteger<short>("Setup", "BonusAbilofWarrMP", M2Share.g_Config.BonusAbilofWarr.MP);
            if (Config.ReadInteger("Setup", "BonusAbilofWarrHit", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWarrHit", M2Share.g_Config.BonusAbilofWarr.Hit);
            M2Share.g_Config.BonusAbilofWarr.Hit =
                Config.ReadInteger<byte>("Setup", "BonusAbilofWarrHit", M2Share.g_Config.BonusAbilofWarr.Hit);
            if (Config.ReadInteger("Setup", "BonusAbilofWarrSpeed", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWarrSpeed", M2Share.g_Config.BonusAbilofWarr.Speed);
            M2Share.g_Config.BonusAbilofWarr.Speed = Config.ReadInteger("Setup", "BonusAbilofWarrSpeed",
                M2Share.g_Config.BonusAbilofWarr.Speed);
            if (Config.ReadInteger("Setup", "BonusAbilofWarrX2", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWarrX2", M2Share.g_Config.BonusAbilofWarr.X2);
            M2Share.g_Config.BonusAbilofWarr.X2 =
                Config.ReadInteger<byte>("Setup", "BonusAbilofWarrX2", M2Share.g_Config.BonusAbilofWarr.X2);
            if (Config.ReadInteger("Setup", "BonusAbilofWizardDC", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWizardDC", M2Share.g_Config.BonusAbilofWizard.DC);
            M2Share.g_Config.BonusAbilofWizard.DC =
                Config.ReadInteger<short>("Setup", "BonusAbilofWizardDC", M2Share.g_Config.BonusAbilofWizard.DC);
            if (Config.ReadInteger("Setup", "BonusAbilofWizardMC", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWizardMC", M2Share.g_Config.BonusAbilofWizard.MC);
            M2Share.g_Config.BonusAbilofWizard.MC =
                Config.ReadInteger<short>("Setup", "BonusAbilofWizardMC", M2Share.g_Config.BonusAbilofWizard.MC);
            if (Config.ReadInteger("Setup", "BonusAbilofWizardSC", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWizardSC", M2Share.g_Config.BonusAbilofWizard.SC);
            M2Share.g_Config.BonusAbilofWizard.SC =
                Config.ReadInteger<short>("Setup", "BonusAbilofWizardSC", M2Share.g_Config.BonusAbilofWizard.SC);
            if (Config.ReadInteger("Setup", "BonusAbilofWizardAC", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWizardAC", M2Share.g_Config.BonusAbilofWizard.AC);
            M2Share.g_Config.BonusAbilofWizard.AC =
                Config.ReadInteger<short>("Setup", "BonusAbilofWizardAC", M2Share.g_Config.BonusAbilofWizard.AC);
            if (Config.ReadInteger("Setup", "BonusAbilofWizardMAC", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWizardMAC", M2Share.g_Config.BonusAbilofWizard.MAC);
            M2Share.g_Config.BonusAbilofWizard.MAC = Config.ReadInteger<short>("Setup", "BonusAbilofWizardMAC",
                M2Share.g_Config.BonusAbilofWizard.MAC);
            if (Config.ReadInteger("Setup", "BonusAbilofWizardHP", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWizardHP", M2Share.g_Config.BonusAbilofWizard.HP);
            M2Share.g_Config.BonusAbilofWizard.HP =
                Config.ReadInteger<short>("Setup", "BonusAbilofWizardHP", M2Share.g_Config.BonusAbilofWizard.HP);
            if (Config.ReadInteger("Setup", "BonusAbilofWizardMP", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWizardMP", M2Share.g_Config.BonusAbilofWizard.MP);
            M2Share.g_Config.BonusAbilofWizard.MP =
                Config.ReadInteger<short>("Setup", "BonusAbilofWizardMP", M2Share.g_Config.BonusAbilofWizard.MP);
            if (Config.ReadInteger("Setup", "BonusAbilofWizardHit", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWizardHit", M2Share.g_Config.BonusAbilofWizard.Hit);
            M2Share.g_Config.BonusAbilofWizard.Hit = Config.ReadInteger<byte>("Setup", "BonusAbilofWizardHit",
                M2Share.g_Config.BonusAbilofWizard.Hit);
            if (Config.ReadInteger("Setup", "BonusAbilofWizardSpeed", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWizardSpeed", M2Share.g_Config.BonusAbilofWizard.Speed);
            M2Share.g_Config.BonusAbilofWizard.Speed = Config.ReadInteger("Setup", "BonusAbilofWizardSpeed",
                M2Share.g_Config.BonusAbilofWizard.Speed);
            if (Config.ReadInteger("Setup", "BonusAbilofWizardX2", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofWizardX2", M2Share.g_Config.BonusAbilofWizard.X2);
            M2Share.g_Config.BonusAbilofWizard.X2 =
                Config.ReadInteger<byte>("Setup", "BonusAbilofWizardX2", M2Share.g_Config.BonusAbilofWizard.X2);
            if (Config.ReadInteger("Setup", "BonusAbilofTaosDC", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofTaosDC", M2Share.g_Config.BonusAbilofTaos.DC);
            M2Share.g_Config.BonusAbilofTaos.DC =
                Config.ReadInteger<byte>("Setup", "BonusAbilofTaosDC", M2Share.g_Config.BonusAbilofTaos.DC);
            if (Config.ReadInteger("Setup", "BonusAbilofTaosMC", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofTaosMC", M2Share.g_Config.BonusAbilofTaos.MC);
            M2Share.g_Config.BonusAbilofTaos.MC =
                Config.ReadInteger<byte>("Setup", "BonusAbilofTaosMC", M2Share.g_Config.BonusAbilofTaos.MC);
            if (Config.ReadInteger("Setup", "BonusAbilofTaosSC", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofTaosSC", M2Share.g_Config.BonusAbilofTaos.SC);
            M2Share.g_Config.BonusAbilofTaos.SC =
                Config.ReadInteger<byte>("Setup", "BonusAbilofTaosSC", M2Share.g_Config.BonusAbilofTaos.SC);
            if (Config.ReadInteger("Setup", "BonusAbilofTaosAC", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofTaosAC", M2Share.g_Config.BonusAbilofTaos.AC);
            M2Share.g_Config.BonusAbilofTaos.AC =
                Config.ReadInteger<byte>("Setup", "BonusAbilofTaosAC", M2Share.g_Config.BonusAbilofTaos.AC);
            if (Config.ReadInteger("Setup", "BonusAbilofTaosMAC", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofTaosMAC", M2Share.g_Config.BonusAbilofTaos.MAC);
            M2Share.g_Config.BonusAbilofTaos.MAC =
                Config.ReadInteger<short>("Setup", "BonusAbilofTaosMAC", M2Share.g_Config.BonusAbilofTaos.MAC);
            if (Config.ReadInteger("Setup", "BonusAbilofTaosHP", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofTaosHP", M2Share.g_Config.BonusAbilofTaos.HP);
            M2Share.g_Config.BonusAbilofTaos.HP =
                Config.ReadInteger<short>("Setup", "BonusAbilofTaosHP", M2Share.g_Config.BonusAbilofTaos.HP);
            if (Config.ReadInteger("Setup", "BonusAbilofTaosMP", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofTaosMP", M2Share.g_Config.BonusAbilofTaos.MP);
            M2Share.g_Config.BonusAbilofTaos.MP =
                Config.ReadInteger<short>("Setup", "BonusAbilofTaosMP", M2Share.g_Config.BonusAbilofTaos.MP);
            if (Config.ReadInteger("Setup", "BonusAbilofTaosHit", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofTaosHit", M2Share.g_Config.BonusAbilofTaos.Hit);
            M2Share.g_Config.BonusAbilofTaos.Hit =
                Config.ReadInteger<byte>("Setup", "BonusAbilofTaosHit", M2Share.g_Config.BonusAbilofTaos.Hit);
            if (Config.ReadInteger("Setup", "BonusAbilofTaosSpeed", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofTaosSpeed", M2Share.g_Config.BonusAbilofTaos.Speed);
            M2Share.g_Config.BonusAbilofTaos.Speed = Config.ReadInteger("Setup", "BonusAbilofTaosSpeed",
                M2Share.g_Config.BonusAbilofTaos.Speed);
            if (Config.ReadInteger("Setup", "BonusAbilofTaosX2", -1) < 0)
                Config.WriteInteger("Setup", "BonusAbilofTaosX2", M2Share.g_Config.BonusAbilofTaos.X2);
            M2Share.g_Config.BonusAbilofTaos.X2 = Config.ReadInteger<byte>("Setup", "BonusAbilofTaosX2", M2Share.g_Config.BonusAbilofTaos.X2);
            if (Config.ReadInteger("Setup", "NakedAbilofWarrDC", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWarrDC", M2Share.g_Config.NakedAbilofWarr.DC);
            M2Share.g_Config.NakedAbilofWarr.DC =
                Config.ReadInteger<short>("Setup", "NakedAbilofWarrDC", M2Share.g_Config.NakedAbilofWarr.DC);
            if (Config.ReadInteger("Setup", "NakedAbilofWarrMC", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWarrMC", M2Share.g_Config.NakedAbilofWarr.MC);
            M2Share.g_Config.NakedAbilofWarr.MC =
                Config.ReadInteger<short>("Setup", "NakedAbilofWarrMC", M2Share.g_Config.NakedAbilofWarr.MC);
            if (Config.ReadInteger("Setup", "NakedAbilofWarrSC", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWarrSC", M2Share.g_Config.NakedAbilofWarr.SC);
            M2Share.g_Config.NakedAbilofWarr.SC =
                Config.ReadInteger<short>("Setup", "NakedAbilofWarrSC", M2Share.g_Config.NakedAbilofWarr.SC);
            if (Config.ReadInteger("Setup", "NakedAbilofWarrAC", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWarrAC", M2Share.g_Config.NakedAbilofWarr.AC);
            M2Share.g_Config.NakedAbilofWarr.AC =
                Config.ReadInteger<short>("Setup", "NakedAbilofWarrAC", M2Share.g_Config.NakedAbilofWarr.AC);
            if (Config.ReadInteger("Setup", "NakedAbilofWarrMAC", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWarrMAC", M2Share.g_Config.NakedAbilofWarr.MAC);
            M2Share.g_Config.NakedAbilofWarr.MAC =
                Config.ReadInteger<short>("Setup", "NakedAbilofWarrMAC", M2Share.g_Config.NakedAbilofWarr.MAC);
            if (Config.ReadInteger("Setup", "NakedAbilofWarrHP", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWarrHP", M2Share.g_Config.NakedAbilofWarr.HP);
            M2Share.g_Config.NakedAbilofWarr.HP =
                Config.ReadInteger<short>("Setup", "NakedAbilofWarrHP", M2Share.g_Config.NakedAbilofWarr.HP);
            if (Config.ReadInteger("Setup", "NakedAbilofWarrMP", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWarrMP", M2Share.g_Config.NakedAbilofWarr.MP);
            M2Share.g_Config.NakedAbilofWarr.MP =
                Config.ReadInteger<short>("Setup", "NakedAbilofWarrMP", M2Share.g_Config.NakedAbilofWarr.MP);
            if (Config.ReadInteger("Setup", "NakedAbilofWarrHit", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWarrHit", M2Share.g_Config.NakedAbilofWarr.Hit);
            M2Share.g_Config.NakedAbilofWarr.Hit =
                Config.ReadInteger<byte>("Setup", "NakedAbilofWarrHit", M2Share.g_Config.NakedAbilofWarr.Hit);
            if (Config.ReadInteger("Setup", "NakedAbilofWarrSpeed", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWarrSpeed", M2Share.g_Config.NakedAbilofWarr.Speed);
            M2Share.g_Config.NakedAbilofWarr.Speed = Config.ReadInteger("Setup", "NakedAbilofWarrSpeed",
                M2Share.g_Config.NakedAbilofWarr.Speed);
            if (Config.ReadInteger("Setup", "NakedAbilofWarrX2", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWarrX2", M2Share.g_Config.NakedAbilofWarr.X2);
            M2Share.g_Config.NakedAbilofWarr.X2 =
                Config.ReadInteger<byte>("Setup", "NakedAbilofWarrX2", M2Share.g_Config.NakedAbilofWarr.X2);
            if (Config.ReadInteger("Setup", "NakedAbilofWizardDC", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWizardDC", M2Share.g_Config.NakedAbilofWizard.DC);
            M2Share.g_Config.NakedAbilofWizard.DC =
                Config.ReadInteger<short>("Setup", "NakedAbilofWizardDC", M2Share.g_Config.NakedAbilofWizard.DC);
            if (Config.ReadInteger("Setup", "NakedAbilofWizardMC", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWizardMC", M2Share.g_Config.NakedAbilofWizard.MC);
            M2Share.g_Config.NakedAbilofWizard.MC =
                Config.ReadInteger<short>("Setup", "NakedAbilofWizardMC", M2Share.g_Config.NakedAbilofWizard.MC);
            if (Config.ReadInteger("Setup", "NakedAbilofWizardSC", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWizardSC", M2Share.g_Config.NakedAbilofWizard.SC);
            M2Share.g_Config.NakedAbilofWizard.SC =
                Config.ReadInteger<short>("Setup", "NakedAbilofWizardSC", M2Share.g_Config.NakedAbilofWizard.SC);
            if (Config.ReadInteger("Setup", "NakedAbilofWizardAC", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWizardAC", M2Share.g_Config.NakedAbilofWizard.AC);
            M2Share.g_Config.NakedAbilofWizard.AC =
                Config.ReadInteger<short>("Setup", "NakedAbilofWizardAC", M2Share.g_Config.NakedAbilofWizard.AC);
            if (Config.ReadInteger("Setup", "NakedAbilofWizardMAC", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWizardMAC", M2Share.g_Config.NakedAbilofWizard.MAC);
            M2Share.g_Config.NakedAbilofWizard.MAC = Config.ReadInteger<short>("Setup", "NakedAbilofWizardMAC", M2Share.g_Config.NakedAbilofWizard.MAC);
            if (Config.ReadInteger("Setup", "NakedAbilofWizardHP", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWizardHP", M2Share.g_Config.NakedAbilofWizard.HP);
            M2Share.g_Config.NakedAbilofWizard.HP =
                Config.ReadInteger<short>("Setup", "NakedAbilofWizardHP", M2Share.g_Config.NakedAbilofWizard.HP);
            if (Config.ReadInteger("Setup", "NakedAbilofWizardMP", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWizardMP", M2Share.g_Config.NakedAbilofWizard.MP);
            M2Share.g_Config.NakedAbilofWizard.MP =
                Config.ReadInteger<short>("Setup", "NakedAbilofWizardMP", M2Share.g_Config.NakedAbilofWizard.MP);
            if (Config.ReadInteger("Setup", "NakedAbilofWizardHit", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWizardHit", M2Share.g_Config.NakedAbilofWizard.Hit);
            M2Share.g_Config.NakedAbilofWizard.Hit = Config.ReadInteger<byte>("Setup", "NakedAbilofWizardHit",
                M2Share.g_Config.NakedAbilofWizard.Hit);
            if (Config.ReadInteger("Setup", "NakedAbilofWizardSpeed", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWizardSpeed", M2Share.g_Config.NakedAbilofWizard.Speed);
            M2Share.g_Config.NakedAbilofWizard.Speed = Config.ReadInteger("Setup", "NakedAbilofWizardSpeed",
                M2Share.g_Config.NakedAbilofWizard.Speed);
            if (Config.ReadInteger("Setup", "NakedAbilofWizardX2", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofWizardX2", M2Share.g_Config.NakedAbilofWizard.X2);
            M2Share.g_Config.NakedAbilofWizard.X2 =
                Config.ReadInteger<byte>("Setup", "NakedAbilofWizardX2", M2Share.g_Config.NakedAbilofWizard.X2);
            if (Config.ReadInteger("Setup", "NakedAbilofTaosDC", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofTaosDC", M2Share.g_Config.NakedAbilofTaos.DC);
            M2Share.g_Config.NakedAbilofTaos.DC =
                Config.ReadInteger<short>("Setup", "NakedAbilofTaosDC", M2Share.g_Config.NakedAbilofTaos.DC);
            if (Config.ReadInteger("Setup", "NakedAbilofTaosMC", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofTaosMC", M2Share.g_Config.NakedAbilofTaos.MC);
            M2Share.g_Config.NakedAbilofTaos.MC =
                Config.ReadInteger<short>("Setup", "NakedAbilofTaosMC", M2Share.g_Config.NakedAbilofTaos.MC);
            if (Config.ReadInteger("Setup", "NakedAbilofTaosSC", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofTaosSC", M2Share.g_Config.NakedAbilofTaos.SC);
            M2Share.g_Config.NakedAbilofTaos.SC = Config.ReadInteger<short>("Setup", "NakedAbilofTaosSC", M2Share.g_Config.NakedAbilofTaos.SC);
            if (Config.ReadInteger("Setup", "NakedAbilofTaosAC", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofTaosAC", M2Share.g_Config.NakedAbilofTaos.AC);
            M2Share.g_Config.NakedAbilofTaos.AC =
                Config.ReadInteger<short>("Setup", "NakedAbilofTaosAC", M2Share.g_Config.NakedAbilofTaos.AC);
            if (Config.ReadInteger("Setup", "NakedAbilofTaosMAC", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofTaosMAC", M2Share.g_Config.NakedAbilofTaos.MAC);
            M2Share.g_Config.NakedAbilofTaos.MAC =
                Config.ReadInteger<short>("Setup", "NakedAbilofTaosMAC", M2Share.g_Config.NakedAbilofTaos.MAC);
            if (Config.ReadInteger("Setup", "NakedAbilofTaosHP", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofTaosHP", M2Share.g_Config.NakedAbilofTaos.HP);
            M2Share.g_Config.NakedAbilofTaos.HP =
                Config.ReadInteger<short>("Setup", "NakedAbilofTaosHP", M2Share.g_Config.NakedAbilofTaos.HP);
            if (Config.ReadInteger("Setup", "NakedAbilofTaosMP", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofTaosMP", M2Share.g_Config.NakedAbilofTaos.MP);
            M2Share.g_Config.NakedAbilofTaos.MP =
                Config.ReadInteger<short>("Setup", "NakedAbilofTaosMP", M2Share.g_Config.NakedAbilofTaos.MP);
            if (Config.ReadInteger("Setup", "NakedAbilofTaosHit", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofTaosHit", M2Share.g_Config.NakedAbilofTaos.Hit);
            M2Share.g_Config.NakedAbilofTaos.Hit =
                Config.ReadInteger<byte>("Setup", "NakedAbilofTaosHit", M2Share.g_Config.NakedAbilofTaos.Hit);
            if (Config.ReadInteger("Setup", "NakedAbilofTaosSpeed", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofTaosSpeed", M2Share.g_Config.NakedAbilofTaos.Speed);
            M2Share.g_Config.NakedAbilofTaos.Speed = Config.ReadInteger("Setup", "NakedAbilofTaosSpeed",
                M2Share.g_Config.NakedAbilofTaos.Speed);
            if (Config.ReadInteger("Setup", "NakedAbilofTaosX2", -1) < 0)
                Config.WriteInteger("Setup", "NakedAbilofTaosX2", M2Share.g_Config.NakedAbilofTaos.X2);
            M2Share.g_Config.NakedAbilofTaos.X2 =
                Config.ReadInteger<byte>("Setup", "NakedAbilofTaosX2", M2Share.g_Config.NakedAbilofTaos.X2);
            if (Config.ReadInteger("Setup", "GroupMembersMax", -1) < 0)
                Config.WriteInteger("Setup", "GroupMembersMax", M2Share.g_Config.nGroupMembersMax);
            M2Share.g_Config.nGroupMembersMax =
                Config.ReadInteger("Setup", "GroupMembersMax", M2Share.g_Config.nGroupMembersMax);
            if (Config.ReadInteger("Setup", "WarrAttackMon", -1) < 0)
                Config.WriteInteger("Setup", "WarrAttackMon", M2Share.g_Config.nWarrMon);
            M2Share.g_Config.nWarrMon = Config.ReadInteger("Setup", "WarrAttackMon", M2Share.g_Config.nWarrMon);
            if (Config.ReadInteger("Setup", "WizardAttackMon", -1) < 0)
                Config.WriteInteger("Setup", "WizardAttackMon", M2Share.g_Config.nWizardMon);
            M2Share.g_Config.nWizardMon = Config.ReadInteger("Setup", "WizardAttackMon", M2Share.g_Config.nWizardMon);
            if (Config.ReadInteger("Setup", "TaosAttackMon", -1) < 0)
                Config.WriteInteger("Setup", "TaosAttackMon", M2Share.g_Config.nTaosMon);
            M2Share.g_Config.nTaosMon = Config.ReadInteger("Setup", "TaosAttackMon", M2Share.g_Config.nTaosMon);
            if (Config.ReadInteger("Setup", "MonAttackHum", -1) < 0)
                Config.WriteInteger("Setup", "MonAttackHum", M2Share.g_Config.nMonHum);
            M2Share.g_Config.nMonHum = Config.ReadInteger("Setup", "MonAttackHum", M2Share.g_Config.nMonHum);
            if (Config.ReadInteger("Setup", "UPgradeWeaponGetBackTime", -1) < 0)
                Config.WriteInteger("Setup", "UPgradeWeaponGetBackTime", M2Share.g_Config.dwUPgradeWeaponGetBackTime);
            M2Share.g_Config.dwUPgradeWeaponGetBackTime = Config.ReadInteger("Setup", "UPgradeWeaponGetBackTime",
                M2Share.g_Config.dwUPgradeWeaponGetBackTime);
            if (Config.ReadInteger("Setup", "ClearExpireUpgradeWeaponDays", -1) < 0)
                Config.WriteInteger("Setup", "ClearExpireUpgradeWeaponDays",
                    M2Share.g_Config.nClearExpireUpgradeWeaponDays);
            M2Share.g_Config.nClearExpireUpgradeWeaponDays = Config.ReadInteger("Setup", "ClearExpireUpgradeWeaponDays",
                M2Share.g_Config.nClearExpireUpgradeWeaponDays);
            if (Config.ReadInteger("Setup", "UpgradeWeaponPrice", -1) < 0)
                Config.WriteInteger("Setup", "UpgradeWeaponPrice", M2Share.g_Config.nUpgradeWeaponPrice);
            M2Share.g_Config.nUpgradeWeaponPrice =
                Config.ReadInteger("Setup", "UpgradeWeaponPrice", M2Share.g_Config.nUpgradeWeaponPrice);
            if (Config.ReadInteger("Setup", "UpgradeWeaponMaxPoint", -1) < 0)
                Config.WriteInteger("Setup", "UpgradeWeaponMaxPoint", M2Share.g_Config.nUpgradeWeaponMaxPoint);
            M2Share.g_Config.nUpgradeWeaponMaxPoint = Config.ReadInteger("Setup", "UpgradeWeaponMaxPoint",
                M2Share.g_Config.nUpgradeWeaponMaxPoint);
            if (Config.ReadInteger("Setup", "UpgradeWeaponDCRate", -1) < 0)
                Config.WriteInteger("Setup", "UpgradeWeaponDCRate", M2Share.g_Config.nUpgradeWeaponDCRate);
            M2Share.g_Config.nUpgradeWeaponDCRate =
                Config.ReadInteger("Setup", "UpgradeWeaponDCRate", M2Share.g_Config.nUpgradeWeaponDCRate);
            if (Config.ReadInteger("Setup", "UpgradeWeaponDCTwoPointRate", -1) < 0)
                Config.WriteInteger("Setup", "UpgradeWeaponDCTwoPointRate",
                    M2Share.g_Config.nUpgradeWeaponDCTwoPointRate);
            M2Share.g_Config.nUpgradeWeaponDCTwoPointRate = Config.ReadInteger("Setup", "UpgradeWeaponDCTwoPointRate",
                M2Share.g_Config.nUpgradeWeaponDCTwoPointRate);
            if (Config.ReadInteger("Setup", "UpgradeWeaponDCThreePointRate", -1) < 0)
                Config.WriteInteger("Setup", "UpgradeWeaponDCThreePointRate",
                    M2Share.g_Config.nUpgradeWeaponDCThreePointRate);
            M2Share.g_Config.nUpgradeWeaponDCThreePointRate = Config.ReadInteger("Setup",
                "UpgradeWeaponDCThreePointRate", M2Share.g_Config.nUpgradeWeaponDCThreePointRate);
            if (Config.ReadInteger("Setup", "UpgradeWeaponMCRate", -1) < 0)
                Config.WriteInteger("Setup", "UpgradeWeaponMCRate", M2Share.g_Config.nUpgradeWeaponMCRate);
            M2Share.g_Config.nUpgradeWeaponMCRate =
                Config.ReadInteger("Setup", "UpgradeWeaponMCRate", M2Share.g_Config.nUpgradeWeaponMCRate);
            if (Config.ReadInteger("Setup", "UpgradeWeaponMCTwoPointRate", -1) < 0)
                Config.WriteInteger("Setup", "UpgradeWeaponMCTwoPointRate",
                    M2Share.g_Config.nUpgradeWeaponMCTwoPointRate);
            M2Share.g_Config.nUpgradeWeaponMCTwoPointRate = Config.ReadInteger("Setup", "UpgradeWeaponMCTwoPointRate",
                M2Share.g_Config.nUpgradeWeaponMCTwoPointRate);
            if (Config.ReadInteger("Setup", "UpgradeWeaponMCThreePointRate", -1) < 0)
                Config.WriteInteger("Setup", "UpgradeWeaponMCThreePointRate",
                    M2Share.g_Config.nUpgradeWeaponMCThreePointRate);
            M2Share.g_Config.nUpgradeWeaponMCThreePointRate = Config.ReadInteger("Setup",
                "UpgradeWeaponMCThreePointRate", M2Share.g_Config.nUpgradeWeaponMCThreePointRate);
            if (Config.ReadInteger("Setup", "UpgradeWeaponSCRate", -1) < 0)
                Config.WriteInteger("Setup", "UpgradeWeaponSCRate", M2Share.g_Config.nUpgradeWeaponSCRate);
            M2Share.g_Config.nUpgradeWeaponSCRate =
                Config.ReadInteger("Setup", "UpgradeWeaponSCRate", M2Share.g_Config.nUpgradeWeaponSCRate);
            if (Config.ReadInteger("Setup", "UpgradeWeaponSCTwoPointRate", -1) < 0)
                Config.WriteInteger("Setup", "UpgradeWeaponSCTwoPointRate",
                    M2Share.g_Config.nUpgradeWeaponSCTwoPointRate);
            M2Share.g_Config.nUpgradeWeaponSCTwoPointRate = Config.ReadInteger("Setup", "UpgradeWeaponSCTwoPointRate",
                M2Share.g_Config.nUpgradeWeaponSCTwoPointRate);
            if (Config.ReadInteger("Setup", "UpgradeWeaponSCThreePointRate", -1) < 0)
                Config.WriteInteger("Setup", "UpgradeWeaponSCThreePointRate",
                    M2Share.g_Config.nUpgradeWeaponSCThreePointRate);
            M2Share.g_Config.nUpgradeWeaponSCThreePointRate = Config.ReadInteger("Setup",
                "UpgradeWeaponSCThreePointRate", M2Share.g_Config.nUpgradeWeaponSCThreePointRate);
            if (Config.ReadInteger("Setup", "BuildGuild", -1) < 0)
                Config.WriteInteger("Setup", "BuildGuild", M2Share.g_Config.nBuildGuildPrice);
            M2Share.g_Config.nBuildGuildPrice =
                Config.ReadInteger("Setup", "BuildGuild", M2Share.g_Config.nBuildGuildPrice);
            if (Config.ReadInteger("Setup", "MakeDurg", -1) < 0)
                Config.WriteInteger("Setup", "MakeDurg", M2Share.g_Config.nMakeDurgPrice);
            M2Share.g_Config.nMakeDurgPrice = Config.ReadInteger("Setup", "MakeDurg", M2Share.g_Config.nMakeDurgPrice);
            if (Config.ReadInteger("Setup", "GuildWarFee", -1) < 0)
                Config.WriteInteger("Setup", "GuildWarFee", M2Share.g_Config.nGuildWarPrice);
            M2Share.g_Config.nGuildWarPrice =
                Config.ReadInteger("Setup", "GuildWarFee", M2Share.g_Config.nGuildWarPrice);
            if (Config.ReadInteger("Setup", "HireGuard", -1) < 0)
                Config.WriteInteger("Setup", "HireGuard", M2Share.g_Config.nHireGuardPrice);
            M2Share.g_Config.nHireGuardPrice =
                Config.ReadInteger("Setup", "HireGuard", M2Share.g_Config.nHireGuardPrice);
            if (Config.ReadInteger("Setup", "HireArcher", -1) < 0)
                Config.WriteInteger("Setup", "HireArcher", M2Share.g_Config.nHireArcherPrice);
            M2Share.g_Config.nHireArcherPrice =
                Config.ReadInteger("Setup", "HireArcher", M2Share.g_Config.nHireArcherPrice);
            if (Config.ReadInteger("Setup", "RepairDoor", -1) < 0)
                Config.WriteInteger("Setup", "RepairDoor", M2Share.g_Config.nRepairDoorPrice);
            M2Share.g_Config.nRepairDoorPrice =
                Config.ReadInteger("Setup", "RepairDoor", M2Share.g_Config.nRepairDoorPrice);
            if (Config.ReadInteger("Setup", "RepairWall", -1) < 0)
                Config.WriteInteger("Setup", "RepairWall", M2Share.g_Config.nRepairWallPrice);
            M2Share.g_Config.nRepairWallPrice =
                Config.ReadInteger("Setup", "RepairWall", M2Share.g_Config.nRepairWallPrice);
            if (Config.ReadInteger("Setup", "CastleMemberPriceRate", -1) < 0)
                Config.WriteInteger("Setup", "CastleMemberPriceRate", M2Share.g_Config.nCastleMemberPriceRate);
            M2Share.g_Config.nCastleMemberPriceRate = Config.ReadInteger("Setup", "CastleMemberPriceRate",
                M2Share.g_Config.nCastleMemberPriceRate);
            if (Config.ReadInteger("Setup", "CastleGoldMax", -1) < 0)
                Config.WriteInteger("Setup", "CastleGoldMax", M2Share.g_Config.nCastleGoldMax);
            M2Share.g_Config.nCastleGoldMax =
                Config.ReadInteger("Setup", "CastleGoldMax", M2Share.g_Config.nCastleGoldMax);
            if (Config.ReadInteger("Setup", "CastleOneDayGold", -1) < 0)
                Config.WriteInteger("Setup", "CastleOneDayGold", M2Share.g_Config.nCastleOneDayGold);
            M2Share.g_Config.nCastleOneDayGold =
                Config.ReadInteger("Setup", "CastleOneDayGold", M2Share.g_Config.nCastleOneDayGold);
            if (Config.ReadString("Setup", "CastleName", "") == "")
                Config.WriteString("Setup", "CastleName", M2Share.g_Config.sCastleName);
            M2Share.g_Config.sCastleName = Config.ReadString("Setup", "CastleName", M2Share.g_Config.sCastleName);
            if (Config.ReadString("Setup", "CastleHomeMap", "") == "")
                Config.WriteString("Setup", "CastleHomeMap", M2Share.g_Config.sCastleHomeMap);
            M2Share.g_Config.sCastleHomeMap =
                Config.ReadString("Setup", "CastleHomeMap", M2Share.g_Config.sCastleHomeMap);
            if (Config.ReadInteger("Setup", "CastleHomeX", -1) < 0)
                Config.WriteInteger("Setup", "CastleHomeX", M2Share.g_Config.nCastleHomeX);
            M2Share.g_Config.nCastleHomeX = Config.ReadInteger("Setup", "CastleHomeX", M2Share.g_Config.nCastleHomeX);
            if (Config.ReadInteger("Setup", "CastleHomeY", -1) < 0)
                Config.WriteInteger("Setup", "CastleHomeY", M2Share.g_Config.nCastleHomeY);
            M2Share.g_Config.nCastleHomeY = Config.ReadInteger("Setup", "CastleHomeY", M2Share.g_Config.nCastleHomeY);
            if (Config.ReadInteger("Setup", "CastleWarRangeX", -1) < 0)
                Config.WriteInteger("Setup", "CastleWarRangeX", M2Share.g_Config.nCastleWarRangeX);
            M2Share.g_Config.nCastleWarRangeX =
                Config.ReadInteger("Setup", "CastleWarRangeX", M2Share.g_Config.nCastleWarRangeX);
            if (Config.ReadInteger("Setup", "CastleWarRangeY", -1) < 0)
                Config.WriteInteger("Setup", "CastleWarRangeY", M2Share.g_Config.nCastleWarRangeY);
            M2Share.g_Config.nCastleWarRangeY =
                Config.ReadInteger("Setup", "CastleWarRangeY", M2Share.g_Config.nCastleWarRangeY);
            if (Config.ReadInteger("Setup", "CastleTaxRate", -1) < 0)
                Config.WriteInteger("Setup", "CastleTaxRate", M2Share.g_Config.nCastleTaxRate);
            M2Share.g_Config.nCastleTaxRate =
                Config.ReadInteger("Setup", "CastleTaxRate", M2Share.g_Config.nCastleTaxRate);
            if (Config.ReadInteger("Setup", "CastleGetAllNpcTax", -1) < 0)
                Config.WriteBool("Setup", "CastleGetAllNpcTax", M2Share.g_Config.boGetAllNpcTax);
            M2Share.g_Config.boGetAllNpcTax =
                Config.ReadBool("Setup", "CastleGetAllNpcTax", M2Share.g_Config.boGetAllNpcTax);
            nLoadInteger = Config.ReadInteger("Setup", "GenMonRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "GenMonRate", M2Share.g_Config.nMonGenRate);
            else
                M2Share.g_Config.nMonGenRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "ProcessMonRandRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "ProcessMonRandRate", M2Share.g_Config.nProcessMonRandRate);
            else
                M2Share.g_Config.nProcessMonRandRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "ProcessMonLimitCount", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "ProcessMonLimitCount", M2Share.g_Config.nProcessMonLimitCount);
            else
                M2Share.g_Config.nProcessMonLimitCount = nLoadInteger;
            if (Config.ReadInteger("Setup", "HumanMaxGold", -1) < 0)
                Config.WriteInteger("Setup", "HumanMaxGold", M2Share.g_Config.nHumanMaxGold);
            M2Share.g_Config.nHumanMaxGold =
                Config.ReadInteger("Setup", "HumanMaxGold", M2Share.g_Config.nHumanMaxGold);
            if (Config.ReadInteger("Setup", "HumanTryModeMaxGold", -1) < 0)
                Config.WriteInteger("Setup", "HumanTryModeMaxGold", M2Share.g_Config.nHumanTryModeMaxGold);
            M2Share.g_Config.nHumanTryModeMaxGold =
                Config.ReadInteger("Setup", "HumanTryModeMaxGold", M2Share.g_Config.nHumanTryModeMaxGold);
            if (Config.ReadInteger("Setup", "TryModeLevel", -1) < 0)
                Config.WriteInteger("Setup", "TryModeLevel", M2Share.g_Config.nTryModeLevel);
            M2Share.g_Config.nTryModeLevel =
                Config.ReadInteger("Setup", "TryModeLevel", M2Share.g_Config.nTryModeLevel);
            if (Config.ReadInteger("Setup", "TryModeUseStorage", -1) < 0)
                Config.WriteBool("Setup", "TryModeUseStorage", M2Share.g_Config.boTryModeUseStorage);
            M2Share.g_Config.boTryModeUseStorage =
                Config.ReadBool("Setup", "TryModeUseStorage", M2Share.g_Config.boTryModeUseStorage);
            if (Config.ReadInteger("Setup", "ShutRedMsgShowGMName", -1) < 0)
                Config.WriteBool("Setup", "ShutRedMsgShowGMName", M2Share.g_Config.boShutRedMsgShowGMName);
            M2Share.g_Config.boShutRedMsgShowGMName = Config.ReadBool("Setup", "ShutRedMsgShowGMName",
                M2Share.g_Config.boShutRedMsgShowGMName);
            if (Config.ReadInteger("Setup", "ShowMakeItemMsg", -1) < 0)
                Config.WriteBool("Setup", "ShowMakeItemMsg", M2Share.g_Config.boShowMakeItemMsg);
            M2Share.g_Config.boShowMakeItemMsg =
                Config.ReadBool("Setup", "ShowMakeItemMsg", M2Share.g_Config.boShowMakeItemMsg);
            if (Config.ReadInteger("Setup", "ShowGuildName", -1) < 0)
                Config.WriteBool("Setup", "ShowGuildName", M2Share.g_Config.boShowGuildName);
            M2Share.g_Config.boShowGuildName =
                Config.ReadBool("Setup", "ShowGuildName", M2Share.g_Config.boShowGuildName);
            if (Config.ReadInteger("Setup", "ShowRankLevelName", -1) < 0)
                Config.WriteBool("Setup", "ShowRankLevelName", M2Share.g_Config.boShowRankLevelName);
            M2Share.g_Config.boShowRankLevelName =
                Config.ReadBool("Setup", "ShowRankLevelName", M2Share.g_Config.boShowRankLevelName);
            if (Config.ReadInteger("Setup", "MonSayMsg", -1) < 0)
                Config.WriteBool("Setup", "MonSayMsg", M2Share.g_Config.boMonSayMsg);
            M2Share.g_Config.boMonSayMsg = Config.ReadBool("Setup", "MonSayMsg", M2Share.g_Config.boMonSayMsg);
            if (Config.ReadInteger("Setup", "SayMsgMaxLen", -1) < 0)
                Config.WriteInteger("Setup", "SayMsgMaxLen", M2Share.g_Config.nSayMsgMaxLen);
            M2Share.g_Config.nSayMsgMaxLen =
                Config.ReadInteger("Setup", "SayMsgMaxLen", M2Share.g_Config.nSayMsgMaxLen);
            if (Config.ReadInteger("Setup", "SayMsgTime", -1) < 0)
                Config.WriteInteger("Setup", "SayMsgTime", M2Share.g_Config.dwSayMsgTime);
            M2Share.g_Config.dwSayMsgTime = Config.ReadInteger("Setup", "SayMsgTime", M2Share.g_Config.dwSayMsgTime);
            if (Config.ReadInteger("Setup", "SayMsgCount", -1) < 0)
                Config.WriteInteger("Setup", "SayMsgCount", M2Share.g_Config.nSayMsgCount);
            M2Share.g_Config.nSayMsgCount = Config.ReadInteger("Setup", "SayMsgCount", M2Share.g_Config.nSayMsgCount);
            if (Config.ReadInteger("Setup", "DisableSayMsgTime", -1) < 0)
                Config.WriteInteger("Setup", "DisableSayMsgTime", M2Share.g_Config.dwDisableSayMsgTime);
            M2Share.g_Config.dwDisableSayMsgTime =
                Config.ReadInteger("Setup", "DisableSayMsgTime", M2Share.g_Config.dwDisableSayMsgTime);
            if (Config.ReadInteger("Setup", "SayRedMsgMaxLen", -1) < 0)
                Config.WriteInteger("Setup", "SayRedMsgMaxLen", M2Share.g_Config.nSayRedMsgMaxLen);
            M2Share.g_Config.nSayRedMsgMaxLen =
                Config.ReadInteger("Setup", "SayRedMsgMaxLen", M2Share.g_Config.nSayRedMsgMaxLen);
            if (Config.ReadInteger("Setup", "CanShoutMsgLevel", -1) < 0)
                Config.WriteInteger("Setup", "CanShoutMsgLevel", M2Share.g_Config.nCanShoutMsgLevel);
            M2Share.g_Config.nCanShoutMsgLevel =
                Config.ReadInteger("Setup", "CanShoutMsgLevel", M2Share.g_Config.nCanShoutMsgLevel);
            if (Config.ReadInteger("Setup", "StartPermission", -1) < 0)
                Config.WriteInteger("Setup", "StartPermission", M2Share.g_Config.nStartPermission);
            M2Share.g_Config.nStartPermission =
                Config.ReadInteger("Setup", "StartPermission", M2Share.g_Config.nStartPermission);
            if (Config.ReadInteger("Setup", "SendRefMsgRange", -1) < 0)
                Config.WriteInteger("Setup", "SendRefMsgRange", M2Share.g_Config.nSendRefMsgRange);
            M2Share.g_Config.nSendRefMsgRange =
                Config.ReadInteger("Setup", "SendRefMsgRange", M2Share.g_Config.nSendRefMsgRange);
            if (Config.ReadInteger("Setup", "DecLampDura", -1) < 0)
                Config.WriteBool("Setup", "DecLampDura", M2Share.g_Config.boDecLampDura);
            M2Share.g_Config.boDecLampDura = Config.ReadBool("Setup", "DecLampDura", M2Share.g_Config.boDecLampDura);
            if (Config.ReadInteger("Setup", "HungerSystem", -1) < 0)
                Config.WriteBool("Setup", "HungerSystem", M2Share.g_Config.boHungerSystem);
            M2Share.g_Config.boHungerSystem = Config.ReadBool("Setup", "HungerSystem", M2Share.g_Config.boHungerSystem);
            if (Config.ReadInteger("Setup", "HungerDecHP", -1) < 0)
                Config.WriteBool("Setup", "HungerDecHP", M2Share.g_Config.boHungerDecHP);
            M2Share.g_Config.boHungerDecHP = Config.ReadBool("Setup", "HungerDecHP", M2Share.g_Config.boHungerDecHP);
            if (Config.ReadInteger("Setup", "HungerDecPower", -1) < 0)
                Config.WriteBool("Setup", "HungerDecPower", M2Share.g_Config.boHungerDecPower);
            M2Share.g_Config.boHungerDecPower =
                Config.ReadBool("Setup", "HungerDecPower", M2Share.g_Config.boHungerDecPower);
            if (Config.ReadInteger("Setup", "DiableHumanRun", -1) < 0)
                Config.WriteBool("Setup", "DiableHumanRun", M2Share.g_Config.boDiableHumanRun);
            M2Share.g_Config.boDiableHumanRun =
                Config.ReadBool("Setup", "DiableHumanRun", M2Share.g_Config.boDiableHumanRun);
            if (Config.ReadInteger("Setup", "RunHuman", -1) < 0)
                Config.WriteBool("Setup", "RunHuman", M2Share.g_Config.boRunHuman);
            M2Share.g_Config.boRunHuman = Config.ReadBool("Setup", "RunHuman", M2Share.g_Config.boRunHuman);
            if (Config.ReadInteger("Setup", "RunMon", -1) < 0)
                Config.WriteBool("Setup", "RunMon", M2Share.g_Config.boRunMon);
            M2Share.g_Config.boRunMon = Config.ReadBool("Setup", "RunMon", M2Share.g_Config.boRunMon);
            if (Config.ReadInteger("Setup", "RunNpc", -1) < 0)
                Config.WriteBool("Setup", "RunNpc", M2Share.g_Config.boRunNpc);
            M2Share.g_Config.boRunNpc = Config.ReadBool("Setup", "RunNpc", M2Share.g_Config.boRunNpc);
            nLoadInteger = Config.ReadInteger("Setup", "RunGuard", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "RunGuard", M2Share.g_Config.boRunGuard);
            else
                M2Share.g_Config.boRunGuard = nLoadInteger == 1;
            if (Config.ReadInteger("Setup", "WarDisableHumanRun", -1) < 0)
                Config.WriteBool("Setup", "WarDisableHumanRun", M2Share.g_Config.boWarDisHumRun);
            M2Share.g_Config.boWarDisHumRun =
                Config.ReadBool("Setup", "WarDisableHumanRun", M2Share.g_Config.boWarDisHumRun);
            if (Config.ReadInteger("Setup", "GMRunAll", -1) < 0)
                Config.WriteBool("Setup", "GMRunAll", M2Share.g_Config.boGMRunAll);
            M2Share.g_Config.boGMRunAll = Config.ReadBool("Setup", "GMRunAll", M2Share.g_Config.boGMRunAll);
            if (Config.ReadInteger("Setup", "SkeletonCount", -1) < 0)
                Config.WriteInteger("Setup", "SkeletonCount", M2Share.g_Config.nSkeletonCount);
            M2Share.g_Config.nSkeletonCount =
                Config.ReadInteger("Setup", "SkeletonCount", M2Share.g_Config.nSkeletonCount);
            for (var I = M2Share.g_Config.SkeletonArray.GetLowerBound(0);
                I <= M2Share.g_Config.SkeletonArray.GetUpperBound(0);
                I++)
            {
                if (Config.ReadInteger("Setup", "SkeletonHumLevel" + I, -1) < 0)
                    Config.WriteInteger("Setup", "SkeletonHumLevel" + I, M2Share.g_Config.SkeletonArray[I].nHumLevel);
                M2Share.g_Config.SkeletonArray[I].nHumLevel = Config.ReadInteger("Setup", "SkeletonHumLevel" + I,
                    M2Share.g_Config.SkeletonArray[I].nHumLevel);
                if (Config.ReadString("Names", "Skeleton" + I, "") == "")
                    Config.WriteString("Names", "Skeleton" + I, M2Share.g_Config.SkeletonArray[I].sMonName);
                M2Share.g_Config.SkeletonArray[I].sMonName = Config.ReadString("Names", "Skeleton" + I,
                    M2Share.g_Config.SkeletonArray[I].sMonName);
                if (Config.ReadInteger("Setup", "SkeletonCount" + I, -1) < 0)
                    Config.WriteInteger("Setup", "SkeletonCount" + I, M2Share.g_Config.SkeletonArray[I].nCount);
                M2Share.g_Config.SkeletonArray[I].nCount = Config.ReadInteger("Setup", "SkeletonCount" + I,
                    M2Share.g_Config.SkeletonArray[I].nCount);
                if (Config.ReadInteger("Setup", "SkeletonLevel" + I, -1) < 0)
                    Config.WriteInteger("Setup", "SkeletonLevel" + I, M2Share.g_Config.SkeletonArray[I].nLevel);
                M2Share.g_Config.SkeletonArray[I].nLevel = Config.ReadInteger("Setup", "SkeletonLevel" + I,
                    M2Share.g_Config.SkeletonArray[I].nLevel);
            }

            if (Config.ReadInteger("Setup", "DragonCount", -1) < 0)
                Config.WriteInteger("Setup", "DragonCount", M2Share.g_Config.nDragonCount);
            M2Share.g_Config.nDragonCount = Config.ReadInteger("Setup", "DragonCount", M2Share.g_Config.nDragonCount);
            for (var I = M2Share.g_Config.DragonArray.GetLowerBound(0);
                I <= M2Share.g_Config.DragonArray.GetUpperBound(0);
                I++)
            {
                if (Config.ReadInteger("Setup", "DragonHumLevel" + I, -1) < 0)
                    Config.WriteInteger("Setup", "DragonHumLevel" + I, M2Share.g_Config.DragonArray[I].nHumLevel);
                M2Share.g_Config.DragonArray[I].nHumLevel = Config.ReadInteger("Setup", "DragonHumLevel" + I,
                    M2Share.g_Config.DragonArray[I].nHumLevel);
                if (Config.ReadString("Names", "Dragon" + I, "") == "")
                    Config.WriteString("Names", "Dragon" + I, M2Share.g_Config.DragonArray[I].sMonName);
                M2Share.g_Config.DragonArray[I].sMonName =
                    Config.ReadString("Names", "Dragon" + I, M2Share.g_Config.DragonArray[I].sMonName);
                if (Config.ReadInteger("Setup", "DragonCount" + I, -1) < 0)
                    Config.WriteInteger("Setup", "DragonCount" + I, M2Share.g_Config.DragonArray[I].nCount);
                M2Share.g_Config.DragonArray[I].nCount = Config.ReadInteger("Setup", "DragonCount" + I,
                    M2Share.g_Config.DragonArray[I].nCount);
                if (Config.ReadInteger("Setup", "DragonLevel" + I, -1) < 0)
                    Config.WriteInteger("Setup", "DragonLevel" + I, M2Share.g_Config.DragonArray[I].nLevel);
                M2Share.g_Config.DragonArray[I].nLevel = Config.ReadInteger("Setup", "DragonLevel" + I,
                    M2Share.g_Config.DragonArray[I].nLevel);
            }

            if (Config.ReadInteger("Setup", "TryDealTime", -1) < 0)
                Config.WriteInteger("Setup", "TryDealTime", M2Share.g_Config.dwTryDealTime);
            M2Share.g_Config.dwTryDealTime = Config.ReadInteger("Setup", "TryDealTime", M2Share.g_Config.dwTryDealTime);
            if (Config.ReadInteger("Setup", "DealOKTime", -1) < 0)
                Config.WriteInteger("Setup", "DealOKTime", M2Share.g_Config.dwDealOKTime);
            M2Share.g_Config.dwDealOKTime = Config.ReadInteger("Setup", "DealOKTime", M2Share.g_Config.dwDealOKTime);
            if (Config.ReadInteger("Setup", "CanNotGetBackDeal", -1) < 0)
                Config.WriteBool("Setup", "CanNotGetBackDeal", M2Share.g_Config.boCanNotGetBackDeal);
            M2Share.g_Config.boCanNotGetBackDeal =
                Config.ReadBool("Setup", "CanNotGetBackDeal", M2Share.g_Config.boCanNotGetBackDeal);
            if (Config.ReadInteger("Setup", "DisableDeal", -1) < 0)
                Config.WriteBool("Setup", "DisableDeal", M2Share.g_Config.boDisableDeal);
            M2Share.g_Config.boDisableDeal = Config.ReadBool("Setup", "DisableDeal", M2Share.g_Config.boDisableDeal);
            if (Config.ReadInteger("Setup", "MasterOKLevel", -1) < 0)
                Config.WriteInteger("Setup", "MasterOKLevel", M2Share.g_Config.nMasterOKLevel);
            M2Share.g_Config.nMasterOKLevel =
                Config.ReadInteger("Setup", "MasterOKLevel", M2Share.g_Config.nMasterOKLevel);
            if (Config.ReadInteger("Setup", "MasterOKCreditPoint", -1) < 0)
                Config.WriteInteger("Setup", "MasterOKCreditPoint", M2Share.g_Config.nMasterOKCreditPoint);
            M2Share.g_Config.nMasterOKCreditPoint =
                Config.ReadInteger("Setup", "MasterOKCreditPoint", M2Share.g_Config.nMasterOKCreditPoint);
            if (Config.ReadInteger("Setup", "MasterOKBonusPoint", -1) < 0)
                Config.WriteInteger("Setup", "MasterOKBonusPoint", M2Share.g_Config.nMasterOKBonusPoint);
            M2Share.g_Config.nMasterOKBonusPoint =
                Config.ReadInteger("Setup", "MasterOKBonusPoint", M2Share.g_Config.nMasterOKBonusPoint);
            if (Config.ReadInteger("Setup", "PKProtect", -1) < 0)
                Config.WriteBool("Setup", "PKProtect", M2Share.g_Config.boPKLevelProtect);
            M2Share.g_Config.boPKLevelProtect =
                Config.ReadBool("Setup", "PKProtect", M2Share.g_Config.boPKLevelProtect);
            if (Config.ReadInteger("Setup", "PKProtectLevel", -1) < 0)
                Config.WriteInteger("Setup", "PKProtectLevel", M2Share.g_Config.nPKProtectLevel);
            M2Share.g_Config.nPKProtectLevel =
                Config.ReadInteger("Setup", "PKProtectLevel", M2Share.g_Config.nPKProtectLevel);
            if (Config.ReadInteger("Setup", "RedPKProtectLevel", -1) < 0)
                Config.WriteInteger("Setup", "RedPKProtectLevel", M2Share.g_Config.nRedPKProtectLevel);
            M2Share.g_Config.nRedPKProtectLevel =
                Config.ReadInteger("Setup", "RedPKProtectLevel", M2Share.g_Config.nRedPKProtectLevel);
            if (Config.ReadInteger("Setup", "ItemPowerRate", -1) < 0)
                Config.WriteInteger("Setup", "ItemPowerRate", M2Share.g_Config.nItemPowerRate);
            M2Share.g_Config.nItemPowerRate =
                Config.ReadInteger("Setup", "ItemPowerRate", M2Share.g_Config.nItemPowerRate);
            if (Config.ReadInteger("Setup", "ItemExpRate", -1) < 0)
                Config.WriteInteger("Setup", "ItemExpRate", M2Share.g_Config.nItemExpRate);
            M2Share.g_Config.nItemExpRate = Config.ReadInteger("Setup", "ItemExpRate", M2Share.g_Config.nItemExpRate);
            if (Config.ReadInteger("Setup", "ScriptGotoCountLimit", -1) < 0)
                Config.WriteInteger("Setup", "ScriptGotoCountLimit", M2Share.g_Config.nScriptGotoCountLimit);
            M2Share.g_Config.nScriptGotoCountLimit = Config.ReadInteger("Setup", "ScriptGotoCountLimit",
                M2Share.g_Config.nScriptGotoCountLimit);
            if (Config.ReadInteger("Setup", "HearMsgFColor", -1) < 0)
                Config.WriteInteger("Setup", "HearMsgFColor", M2Share.g_Config.btHearMsgFColor);
            M2Share.g_Config.btHearMsgFColor =
                Config.ReadInteger<byte>("Setup", "HearMsgFColor", M2Share.g_Config.btHearMsgFColor);
            if (Config.ReadInteger("Setup", "HearMsgBColor", -1) < 0)
                Config.WriteInteger("Setup", "HearMsgBColor", M2Share.g_Config.btHearMsgBColor);
            M2Share.g_Config.btHearMsgBColor =
                Config.ReadInteger<byte>("Setup", "HearMsgBColor", M2Share.g_Config.btHearMsgBColor);
            if (Config.ReadInteger("Setup", "WhisperMsgFColor", -1) < 0)
                Config.WriteInteger("Setup", "WhisperMsgFColor", M2Share.g_Config.btWhisperMsgFColor);
            M2Share.g_Config.btWhisperMsgFColor =
                Config.ReadInteger<byte>("Setup", "WhisperMsgFColor", M2Share.g_Config.btWhisperMsgFColor);
            if (Config.ReadInteger("Setup", "WhisperMsgBColor", -1) < 0)
                Config.WriteInteger("Setup", "WhisperMsgBColor", M2Share.g_Config.btWhisperMsgBColor);
            M2Share.g_Config.btWhisperMsgBColor =
                Config.ReadInteger<byte>("Setup", "WhisperMsgBColor", M2Share.g_Config.btWhisperMsgBColor);
            if (Config.ReadInteger("Setup", "GMWhisperMsgFColor", -1) < 0)
                Config.WriteInteger("Setup", "GMWhisperMsgFColor", M2Share.g_Config.btGMWhisperMsgFColor);
            M2Share.g_Config.btGMWhisperMsgFColor =
                Config.ReadInteger<byte>("Setup", "GMWhisperMsgFColor", M2Share.g_Config.btGMWhisperMsgFColor);
            if (Config.ReadInteger("Setup", "GMWhisperMsgBColor", -1) < 0)
                Config.WriteInteger("Setup", "GMWhisperMsgBColor", M2Share.g_Config.btGMWhisperMsgBColor);
            M2Share.g_Config.btGMWhisperMsgBColor =
                Config.ReadInteger<byte>("Setup", "GMWhisperMsgBColor", M2Share.g_Config.btGMWhisperMsgBColor);
            if (Config.ReadInteger("Setup", "CryMsgFColor", -1) < 0)
                Config.WriteInteger("Setup", "CryMsgFColor", M2Share.g_Config.btCryMsgFColor);
            M2Share.g_Config.btCryMsgFColor =
                Config.ReadInteger<byte>("Setup", "CryMsgFColor", M2Share.g_Config.btCryMsgFColor);
            if (Config.ReadInteger("Setup", "CryMsgBColor", -1) < 0)
                Config.WriteInteger("Setup", "CryMsgBColor", M2Share.g_Config.btCryMsgBColor);
            M2Share.g_Config.btCryMsgBColor =
                Config.ReadInteger<byte>("Setup", "CryMsgBColor", M2Share.g_Config.btCryMsgBColor);
            if (Config.ReadInteger("Setup", "GreenMsgFColor", -1) < 0)
                Config.WriteInteger("Setup", "GreenMsgFColor", M2Share.g_Config.btGreenMsgFColor);
            M2Share.g_Config.btGreenMsgFColor =
                Config.ReadInteger<byte>("Setup", "GreenMsgFColor", M2Share.g_Config.btGreenMsgFColor);
            if (Config.ReadInteger("Setup", "GreenMsgBColor", -1) < 0)
                Config.WriteInteger("Setup", "GreenMsgBColor", M2Share.g_Config.btGreenMsgBColor);
            M2Share.g_Config.btGreenMsgBColor =
                Config.ReadInteger<byte>("Setup", "GreenMsgBColor", M2Share.g_Config.btGreenMsgBColor);
            if (Config.ReadInteger("Setup", "BlueMsgFColor", -1) < 0)
                Config.WriteInteger("Setup", "BlueMsgFColor", M2Share.g_Config.btBlueMsgFColor);
            M2Share.g_Config.btBlueMsgFColor =
                Config.ReadInteger<byte>("Setup", "BlueMsgFColor", M2Share.g_Config.btBlueMsgFColor);
            if (Config.ReadInteger("Setup", "BlueMsgBColor", -1) < 0)
                Config.WriteInteger("Setup", "BlueMsgBColor", M2Share.g_Config.btBlueMsgBColor);
            M2Share.g_Config.btBlueMsgBColor =
                Config.ReadInteger<byte>("Setup", "BlueMsgBColor", M2Share.g_Config.btBlueMsgBColor);
            if (Config.ReadInteger("Setup", "RedMsgFColor", -1) < 0)
                Config.WriteInteger("Setup", "RedMsgFColor", M2Share.g_Config.btRedMsgFColor);
            M2Share.g_Config.btRedMsgFColor =
                Config.ReadInteger<byte>("Setup", "RedMsgFColor", M2Share.g_Config.btRedMsgFColor);
            if (Config.ReadInteger("Setup", "RedMsgBColor", -1) < 0)
                Config.WriteInteger("Setup", "RedMsgBColor", M2Share.g_Config.btRedMsgBColor);
            M2Share.g_Config.btRedMsgBColor = Config.ReadInteger<byte>("Setup", "RedMsgBColor", M2Share.g_Config.btRedMsgBColor);
            if (Config.ReadInteger("Setup", "GuildMsgFColor", -1) < 0)
                Config.WriteInteger("Setup", "GuildMsgFColor", M2Share.g_Config.btGuildMsgFColor);
            M2Share.g_Config.btGuildMsgFColor =
                Config.ReadInteger<byte>("Setup", "GuildMsgFColor", M2Share.g_Config.btGuildMsgFColor);
            if (Config.ReadInteger("Setup", "GuildMsgBColor", -1) < 0)
                Config.WriteInteger("Setup", "GuildMsgBColor", M2Share.g_Config.btGuildMsgBColor);
            M2Share.g_Config.btGuildMsgBColor =
                Config.ReadInteger<byte>("Setup", "GuildMsgBColor", M2Share.g_Config.btGuildMsgBColor);
            if (Config.ReadInteger("Setup", "GroupMsgFColor", -1) < 0)
                Config.WriteInteger("Setup", "GroupMsgFColor", M2Share.g_Config.btGroupMsgFColor);
            M2Share.g_Config.btGroupMsgFColor =
                Config.ReadInteger<byte>("Setup", "GroupMsgFColor", M2Share.g_Config.btGroupMsgFColor);
            if (Config.ReadInteger("Setup", "GroupMsgBColor", -1) < 0)
                Config.WriteInteger("Setup", "GroupMsgBColor", M2Share.g_Config.btGroupMsgBColor);
            M2Share.g_Config.btGroupMsgBColor =
                Config.ReadInteger<byte>("Setup", "GroupMsgBColor", M2Share.g_Config.btGroupMsgBColor);
            if (Config.ReadInteger("Setup", "CustMsgFColor", -1) < 0)
                Config.WriteInteger("Setup", "CustMsgFColor", M2Share.g_Config.btCustMsgFColor);
            M2Share.g_Config.btCustMsgFColor =
                Config.ReadInteger<byte>("Setup", "CustMsgFColor", M2Share.g_Config.btCustMsgFColor);
            if (Config.ReadInteger("Setup", "CustMsgBColor", -1) < 0)
                Config.WriteInteger("Setup", "CustMsgBColor", M2Share.g_Config.btCustMsgBColor);
            M2Share.g_Config.btCustMsgBColor =
                Config.ReadInteger<byte>("Setup", "CustMsgBColor", M2Share.g_Config.btCustMsgBColor);
            if (Config.ReadInteger("Setup", "MonRandomAddValue", -1) < 0)
                Config.WriteInteger("Setup", "MonRandomAddValue", M2Share.g_Config.nMonRandomAddValue);
            M2Share.g_Config.nMonRandomAddValue =
                Config.ReadInteger("Setup", "MonRandomAddValue", M2Share.g_Config.nMonRandomAddValue);
            if (Config.ReadInteger("Setup", "MakeRandomAddValue", -1) < 0)
                Config.WriteInteger("Setup", "MakeRandomAddValue", M2Share.g_Config.nMakeRandomAddValue);
            M2Share.g_Config.nMakeRandomAddValue =
                Config.ReadInteger("Setup", "MakeRandomAddValue", M2Share.g_Config.nMakeRandomAddValue);
            if (Config.ReadInteger("Setup", "WeaponDCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "WeaponDCAddValueMaxLimit", M2Share.g_Config.nWeaponDCAddValueMaxLimit);
            M2Share.g_Config.nWeaponDCAddValueMaxLimit = Config.ReadInteger("Setup", "WeaponDCAddValueMaxLimit",
                M2Share.g_Config.nWeaponDCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "WeaponDCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "WeaponDCAddValueRate", M2Share.g_Config.nWeaponDCAddValueRate);
            M2Share.g_Config.nWeaponDCAddValueRate = Config.ReadInteger("Setup", "WeaponDCAddValueRate",
                M2Share.g_Config.nWeaponDCAddValueRate);
            if (Config.ReadInteger("Setup", "WeaponMCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "WeaponMCAddValueMaxLimit", M2Share.g_Config.nWeaponMCAddValueMaxLimit);
            M2Share.g_Config.nWeaponMCAddValueMaxLimit = Config.ReadInteger("Setup", "WeaponMCAddValueMaxLimit",
                M2Share.g_Config.nWeaponMCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "WeaponMCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "WeaponMCAddValueRate", M2Share.g_Config.nWeaponMCAddValueRate);
            M2Share.g_Config.nWeaponMCAddValueRate = Config.ReadInteger("Setup", "WeaponMCAddValueRate",
                M2Share.g_Config.nWeaponMCAddValueRate);
            if (Config.ReadInteger("Setup", "WeaponSCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "WeaponSCAddValueMaxLimit", M2Share.g_Config.nWeaponSCAddValueMaxLimit);
            M2Share.g_Config.nWeaponSCAddValueMaxLimit = Config.ReadInteger("Setup", "WeaponSCAddValueMaxLimit",
                M2Share.g_Config.nWeaponSCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "WeaponSCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "WeaponSCAddValueRate", M2Share.g_Config.nWeaponSCAddValueRate);
            M2Share.g_Config.nWeaponSCAddValueRate = Config.ReadInteger("Setup", "WeaponSCAddValueRate",
                M2Share.g_Config.nWeaponSCAddValueRate);
            if (Config.ReadInteger("Setup", "DressDCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "DressDCAddValueMaxLimit", M2Share.g_Config.nDressDCAddValueMaxLimit);
            M2Share.g_Config.nDressDCAddValueMaxLimit = Config.ReadInteger("Setup", "DressDCAddValueMaxLimit",
                M2Share.g_Config.nDressDCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "DressDCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "DressDCAddValueRate", M2Share.g_Config.nDressDCAddValueRate);
            M2Share.g_Config.nDressDCAddValueRate =
                Config.ReadInteger("Setup", "DressDCAddValueRate", M2Share.g_Config.nDressDCAddValueRate);
            if (Config.ReadInteger("Setup", "DressDCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "DressDCAddRate", M2Share.g_Config.nDressDCAddRate);
            M2Share.g_Config.nDressDCAddRate =
                Config.ReadInteger("Setup", "DressDCAddRate", M2Share.g_Config.nDressDCAddRate);
            if (Config.ReadInteger("Setup", "DressMCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "DressMCAddValueMaxLimit", M2Share.g_Config.nDressMCAddValueMaxLimit);
            M2Share.g_Config.nDressMCAddValueMaxLimit = Config.ReadInteger("Setup", "DressMCAddValueMaxLimit",
                M2Share.g_Config.nDressMCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "DressMCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "DressMCAddValueRate", M2Share.g_Config.nDressMCAddValueRate);
            M2Share.g_Config.nDressMCAddValueRate =
                Config.ReadInteger("Setup", "DressMCAddValueRate", M2Share.g_Config.nDressMCAddValueRate);
            if (Config.ReadInteger("Setup", "DressMCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "DressMCAddRate", M2Share.g_Config.nDressMCAddRate);
            M2Share.g_Config.nDressMCAddRate =
                Config.ReadInteger("Setup", "DressMCAddRate", M2Share.g_Config.nDressMCAddRate);
            if (Config.ReadInteger("Setup", "DressSCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "DressSCAddValueMaxLimit", M2Share.g_Config.nDressSCAddValueMaxLimit);
            M2Share.g_Config.nDressSCAddValueMaxLimit = Config.ReadInteger("Setup", "DressSCAddValueMaxLimit",
                M2Share.g_Config.nDressSCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "DressSCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "DressSCAddValueRate", M2Share.g_Config.nDressSCAddValueRate);
            M2Share.g_Config.nDressSCAddValueRate =
                Config.ReadInteger("Setup", "DressSCAddValueRate", M2Share.g_Config.nDressSCAddValueRate);
            if (Config.ReadInteger("Setup", "DressSCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "DressSCAddRate", M2Share.g_Config.nDressSCAddRate);
            M2Share.g_Config.nDressSCAddRate =
                Config.ReadInteger("Setup", "DressSCAddRate", M2Share.g_Config.nDressSCAddRate);
            if (Config.ReadInteger("Setup", "NeckLace19DCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace19DCAddValueMaxLimit",
                    M2Share.g_Config.nNeckLace19DCAddValueMaxLimit);
            M2Share.g_Config.nNeckLace19DCAddValueMaxLimit = Config.ReadInteger("Setup", "NeckLace19DCAddValueMaxLimit",
                M2Share.g_Config.nNeckLace19DCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "NeckLace19DCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace19DCAddValueRate", M2Share.g_Config.nNeckLace19DCAddValueRate);
            M2Share.g_Config.nNeckLace19DCAddValueRate = Config.ReadInteger("Setup", "NeckLace19DCAddValueRate",
                M2Share.g_Config.nNeckLace19DCAddValueRate);
            if (Config.ReadInteger("Setup", "NeckLace19DCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace19DCAddRate", M2Share.g_Config.nNeckLace19DCAddRate);
            M2Share.g_Config.nNeckLace19DCAddRate =
                Config.ReadInteger("Setup", "NeckLace19DCAddRate", M2Share.g_Config.nNeckLace19DCAddRate);
            if (Config.ReadInteger("Setup", "NeckLace19MCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace19MCAddValueMaxLimit",
                    M2Share.g_Config.nNeckLace19MCAddValueMaxLimit);
            M2Share.g_Config.nNeckLace19MCAddValueMaxLimit = Config.ReadInteger("Setup", "NeckLace19MCAddValueMaxLimit",
                M2Share.g_Config.nNeckLace19MCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "NeckLace19MCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace19MCAddValueRate", M2Share.g_Config.nNeckLace19MCAddValueRate);
            M2Share.g_Config.nNeckLace19MCAddValueRate = Config.ReadInteger("Setup", "NeckLace19MCAddValueRate",
                M2Share.g_Config.nNeckLace19MCAddValueRate);
            if (Config.ReadInteger("Setup", "NeckLace19MCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace19MCAddRate", M2Share.g_Config.nNeckLace19MCAddRate);
            M2Share.g_Config.nNeckLace19MCAddRate =
                Config.ReadInteger("Setup", "NeckLace19MCAddRate", M2Share.g_Config.nNeckLace19MCAddRate);
            if (Config.ReadInteger("Setup", "NeckLace19SCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace19SCAddValueMaxLimit",
                    M2Share.g_Config.nNeckLace19SCAddValueMaxLimit);
            M2Share.g_Config.nNeckLace19SCAddValueMaxLimit = Config.ReadInteger("Setup", "NeckLace19SCAddValueMaxLimit",
                M2Share.g_Config.nNeckLace19SCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "NeckLace19SCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace19SCAddValueRate", M2Share.g_Config.nNeckLace19SCAddValueRate);
            M2Share.g_Config.nNeckLace19SCAddValueRate = Config.ReadInteger("Setup", "NeckLace19SCAddValueRate",
                M2Share.g_Config.nNeckLace19SCAddValueRate);
            if (Config.ReadInteger("Setup", "NeckLace19SCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace19SCAddRate", M2Share.g_Config.nNeckLace19SCAddRate);
            M2Share.g_Config.nNeckLace19SCAddRate =
                Config.ReadInteger("Setup", "NeckLace19SCAddRate", M2Share.g_Config.nNeckLace19SCAddRate);
            if (Config.ReadInteger("Setup", "NeckLace202124DCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace202124DCAddValueMaxLimit",
                    M2Share.g_Config.nNeckLace202124DCAddValueMaxLimit);
            M2Share.g_Config.nNeckLace202124DCAddValueMaxLimit = Config.ReadInteger("Setup",
                "NeckLace202124DCAddValueMaxLimit", M2Share.g_Config.nNeckLace202124DCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "NeckLace202124DCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace202124DCAddValueRate",
                    M2Share.g_Config.nNeckLace202124DCAddValueRate);
            M2Share.g_Config.nNeckLace202124DCAddValueRate = Config.ReadInteger("Setup", "NeckLace202124DCAddValueRate",
                M2Share.g_Config.nNeckLace202124DCAddValueRate);
            if (Config.ReadInteger("Setup", "NeckLace202124DCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace202124DCAddRate", M2Share.g_Config.nNeckLace202124DCAddRate);
            M2Share.g_Config.nNeckLace202124DCAddRate = Config.ReadInteger("Setup", "NeckLace202124DCAddRate",
                M2Share.g_Config.nNeckLace202124DCAddRate);
            if (Config.ReadInteger("Setup", "NeckLace202124MCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace202124MCAddValueMaxLimit",
                    M2Share.g_Config.nNeckLace202124MCAddValueMaxLimit);
            M2Share.g_Config.nNeckLace202124MCAddValueMaxLimit = Config.ReadInteger("Setup",
                "NeckLace202124MCAddValueMaxLimit", M2Share.g_Config.nNeckLace202124MCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "NeckLace202124MCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace202124MCAddValueRate",
                    M2Share.g_Config.nNeckLace202124MCAddValueRate);
            M2Share.g_Config.nNeckLace202124MCAddValueRate = Config.ReadInteger("Setup", "NeckLace202124MCAddValueRate",
                M2Share.g_Config.nNeckLace202124MCAddValueRate);
            if (Config.ReadInteger("Setup", "NeckLace202124MCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace202124MCAddRate", M2Share.g_Config.nNeckLace202124MCAddRate);
            M2Share.g_Config.nNeckLace202124MCAddRate = Config.ReadInteger("Setup", "NeckLace202124MCAddRate",
                M2Share.g_Config.nNeckLace202124MCAddRate);
            if (Config.ReadInteger("Setup", "NeckLace202124SCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace202124SCAddValueMaxLimit",
                    M2Share.g_Config.nNeckLace202124SCAddValueMaxLimit);
            M2Share.g_Config.nNeckLace202124SCAddValueMaxLimit = Config.ReadInteger("Setup",
                "NeckLace202124SCAddValueMaxLimit", M2Share.g_Config.nNeckLace202124SCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "NeckLace202124SCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace202124SCAddValueRate",
                    M2Share.g_Config.nNeckLace202124SCAddValueRate);
            M2Share.g_Config.nNeckLace202124SCAddValueRate = Config.ReadInteger("Setup", "NeckLace202124SCAddValueRate",
                M2Share.g_Config.nNeckLace202124SCAddValueRate);
            if (Config.ReadInteger("Setup", "NeckLace202124SCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "NeckLace202124SCAddRate", M2Share.g_Config.nNeckLace202124SCAddRate);
            M2Share.g_Config.nNeckLace202124SCAddRate = Config.ReadInteger("Setup", "NeckLace202124SCAddRate",
                M2Share.g_Config.nNeckLace202124SCAddRate);
            if (Config.ReadInteger("Setup", "ArmRing26DCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "ArmRing26DCAddValueMaxLimit",
                    M2Share.g_Config.nArmRing26DCAddValueMaxLimit);
            M2Share.g_Config.nArmRing26DCAddValueMaxLimit = Config.ReadInteger("Setup", "ArmRing26DCAddValueMaxLimit",
                M2Share.g_Config.nArmRing26DCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "ArmRing26DCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "ArmRing26DCAddValueRate", M2Share.g_Config.nArmRing26DCAddValueRate);
            M2Share.g_Config.nArmRing26DCAddValueRate = Config.ReadInteger("Setup", "ArmRing26DCAddValueRate",
                M2Share.g_Config.nArmRing26DCAddValueRate);
            if (Config.ReadInteger("Setup", "ArmRing26DCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "ArmRing26DCAddRate", M2Share.g_Config.nArmRing26DCAddRate);
            M2Share.g_Config.nArmRing26DCAddRate =
                Config.ReadInteger("Setup", "ArmRing26DCAddRate", M2Share.g_Config.nArmRing26DCAddRate);
            if (Config.ReadInteger("Setup", "ArmRing26MCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "ArmRing26MCAddValueMaxLimit",
                    M2Share.g_Config.nArmRing26MCAddValueMaxLimit);
            M2Share.g_Config.nArmRing26MCAddValueMaxLimit = Config.ReadInteger("Setup", "ArmRing26MCAddValueMaxLimit",
                M2Share.g_Config.nArmRing26MCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "ArmRing26MCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "ArmRing26MCAddValueRate", M2Share.g_Config.nArmRing26MCAddValueRate);
            M2Share.g_Config.nArmRing26MCAddValueRate = Config.ReadInteger("Setup", "ArmRing26MCAddValueRate",
                M2Share.g_Config.nArmRing26MCAddValueRate);
            if (Config.ReadInteger("Setup", "ArmRing26MCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "ArmRing26MCAddRate", M2Share.g_Config.nArmRing26MCAddRate);
            M2Share.g_Config.nArmRing26MCAddRate =
                Config.ReadInteger("Setup", "ArmRing26MCAddRate", M2Share.g_Config.nArmRing26MCAddRate);
            if (Config.ReadInteger("Setup", "ArmRing26SCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "ArmRing26SCAddValueMaxLimit",
                    M2Share.g_Config.nArmRing26SCAddValueMaxLimit);
            M2Share.g_Config.nArmRing26SCAddValueMaxLimit = Config.ReadInteger("Setup", "ArmRing26SCAddValueMaxLimit",
                M2Share.g_Config.nArmRing26SCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "ArmRing26SCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "ArmRing26SCAddValueRate", M2Share.g_Config.nArmRing26SCAddValueRate);
            M2Share.g_Config.nArmRing26SCAddValueRate = Config.ReadInteger("Setup", "ArmRing26SCAddValueRate",
                M2Share.g_Config.nArmRing26SCAddValueRate);
            if (Config.ReadInteger("Setup", "ArmRing26SCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "ArmRing26SCAddRate", M2Share.g_Config.nArmRing26SCAddRate);
            M2Share.g_Config.nArmRing26SCAddRate =
                Config.ReadInteger("Setup", "ArmRing26SCAddRate", M2Share.g_Config.nArmRing26SCAddRate);
            if (Config.ReadInteger("Setup", "Ring22DCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "Ring22DCAddValueMaxLimit", M2Share.g_Config.nRing22DCAddValueMaxLimit);
            M2Share.g_Config.nRing22DCAddValueMaxLimit = Config.ReadInteger("Setup", "Ring22DCAddValueMaxLimit",
                M2Share.g_Config.nRing22DCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "Ring22DCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "Ring22DCAddValueRate", M2Share.g_Config.nRing22DCAddValueRate);
            M2Share.g_Config.nRing22DCAddValueRate = Config.ReadInteger("Setup", "Ring22DCAddValueRate",
                M2Share.g_Config.nRing22DCAddValueRate);
            if (Config.ReadInteger("Setup", "Ring22DCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "Ring22DCAddRate", M2Share.g_Config.nRing22DCAddRate);
            M2Share.g_Config.nRing22DCAddRate =
                Config.ReadInteger("Setup", "Ring22DCAddRate", M2Share.g_Config.nRing22DCAddRate);
            if (Config.ReadInteger("Setup", "Ring22MCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "Ring22MCAddValueMaxLimit", M2Share.g_Config.nRing22MCAddValueMaxLimit);
            M2Share.g_Config.nRing22MCAddValueMaxLimit = Config.ReadInteger("Setup", "Ring22MCAddValueMaxLimit",
                M2Share.g_Config.nRing22MCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "Ring22MCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "Ring22MCAddValueRate", M2Share.g_Config.nRing22MCAddValueRate);
            M2Share.g_Config.nRing22MCAddValueRate = Config.ReadInteger("Setup", "Ring22MCAddValueRate",
                M2Share.g_Config.nRing22MCAddValueRate);
            if (Config.ReadInteger("Setup", "Ring22MCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "Ring22MCAddRate", M2Share.g_Config.nRing22MCAddRate);
            M2Share.g_Config.nRing22MCAddRate =
                Config.ReadInteger("Setup", "Ring22MCAddRate", M2Share.g_Config.nRing22MCAddRate);
            if (Config.ReadInteger("Setup", "Ring22SCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "Ring22SCAddValueMaxLimit", M2Share.g_Config.nRing22SCAddValueMaxLimit);
            M2Share.g_Config.nRing22SCAddValueMaxLimit = Config.ReadInteger("Setup", "Ring22SCAddValueMaxLimit",
                M2Share.g_Config.nRing22SCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "Ring22SCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "Ring22SCAddValueRate", M2Share.g_Config.nRing22SCAddValueRate);
            M2Share.g_Config.nRing22SCAddValueRate = Config.ReadInteger("Setup", "Ring22SCAddValueRate",
                M2Share.g_Config.nRing22SCAddValueRate);
            if (Config.ReadInteger("Setup", "Ring22SCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "Ring22SCAddRate", M2Share.g_Config.nRing22SCAddRate);
            M2Share.g_Config.nRing22SCAddRate =
                Config.ReadInteger("Setup", "Ring22SCAddRate", M2Share.g_Config.nRing22SCAddRate);
            if (Config.ReadInteger("Setup", "Ring23DCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "Ring23DCAddValueMaxLimit", M2Share.g_Config.nRing23DCAddValueMaxLimit);
            M2Share.g_Config.nRing23DCAddValueMaxLimit = Config.ReadInteger("Setup", "Ring23DCAddValueMaxLimit",
                M2Share.g_Config.nRing23DCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "Ring23DCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "Ring23DCAddValueRate", M2Share.g_Config.nRing23DCAddValueRate);
            M2Share.g_Config.nRing23DCAddValueRate = Config.ReadInteger("Setup", "Ring23DCAddValueRate",
                M2Share.g_Config.nRing23DCAddValueRate);
            if (Config.ReadInteger("Setup", "Ring23DCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "Ring23DCAddRate", M2Share.g_Config.nRing23DCAddRate);
            M2Share.g_Config.nRing23DCAddRate =
                Config.ReadInteger("Setup", "Ring23DCAddRate", M2Share.g_Config.nRing23DCAddRate);
            if (Config.ReadInteger("Setup", "Ring23MCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "Ring23MCAddValueMaxLimit", M2Share.g_Config.nRing23MCAddValueMaxLimit);
            M2Share.g_Config.nRing23MCAddValueMaxLimit = Config.ReadInteger("Setup", "Ring23MCAddValueMaxLimit",
                M2Share.g_Config.nRing23MCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "Ring23MCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "Ring23MCAddValueRate", M2Share.g_Config.nRing23MCAddValueRate);
            M2Share.g_Config.nRing23MCAddValueRate = Config.ReadInteger("Setup", "Ring23MCAddValueRate",
                M2Share.g_Config.nRing23MCAddValueRate);
            if (Config.ReadInteger("Setup", "Ring23MCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "Ring23MCAddRate", M2Share.g_Config.nRing23MCAddRate);
            M2Share.g_Config.nRing23MCAddRate =
                Config.ReadInteger("Setup", "Ring23MCAddRate", M2Share.g_Config.nRing23MCAddRate);
            if (Config.ReadInteger("Setup", "Ring23SCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "Ring23SCAddValueMaxLimit", M2Share.g_Config.nRing23SCAddValueMaxLimit);
            M2Share.g_Config.nRing23SCAddValueMaxLimit = Config.ReadInteger("Setup", "Ring23SCAddValueMaxLimit",
                M2Share.g_Config.nRing23SCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "Ring23SCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "Ring23SCAddValueRate", M2Share.g_Config.nRing23SCAddValueRate);
            M2Share.g_Config.nRing23SCAddValueRate = Config.ReadInteger("Setup", "Ring23SCAddValueRate",
                M2Share.g_Config.nRing23SCAddValueRate);
            if (Config.ReadInteger("Setup", "Ring23SCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "Ring23SCAddRate", M2Share.g_Config.nRing23SCAddRate);
            M2Share.g_Config.nRing23SCAddRate =
                Config.ReadInteger("Setup", "Ring23SCAddRate", M2Share.g_Config.nRing23SCAddRate);
            if (Config.ReadInteger("Setup", "HelMetDCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "HelMetDCAddValueMaxLimit", M2Share.g_Config.nHelMetDCAddValueMaxLimit);
            M2Share.g_Config.nHelMetDCAddValueMaxLimit = Config.ReadInteger("Setup", "HelMetDCAddValueMaxLimit",
                M2Share.g_Config.nHelMetDCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "HelMetDCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "HelMetDCAddValueRate", M2Share.g_Config.nHelMetDCAddValueRate);
            M2Share.g_Config.nHelMetDCAddValueRate = Config.ReadInteger("Setup", "HelMetDCAddValueRate",
                M2Share.g_Config.nHelMetDCAddValueRate);
            if (Config.ReadInteger("Setup", "HelMetDCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "HelMetDCAddRate", M2Share.g_Config.nHelMetDCAddRate);
            M2Share.g_Config.nHelMetDCAddRate =
                Config.ReadInteger("Setup", "HelMetDCAddRate", M2Share.g_Config.nHelMetDCAddRate);
            if (Config.ReadInteger("Setup", "HelMetMCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "HelMetMCAddValueMaxLimit", M2Share.g_Config.nHelMetMCAddValueMaxLimit);
            M2Share.g_Config.nHelMetMCAddValueMaxLimit = Config.ReadInteger("Setup", "HelMetMCAddValueMaxLimit",
                M2Share.g_Config.nHelMetMCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "HelMetMCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "HelMetMCAddValueRate", M2Share.g_Config.nHelMetMCAddValueRate);
            M2Share.g_Config.nHelMetMCAddValueRate = Config.ReadInteger("Setup", "HelMetMCAddValueRate",
                M2Share.g_Config.nHelMetMCAddValueRate);
            if (Config.ReadInteger("Setup", "HelMetMCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "HelMetMCAddRate", M2Share.g_Config.nHelMetMCAddRate);
            M2Share.g_Config.nHelMetMCAddRate =
                Config.ReadInteger("Setup", "HelMetMCAddRate", M2Share.g_Config.nHelMetMCAddRate);
            if (Config.ReadInteger("Setup", "HelMetSCAddValueMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "HelMetSCAddValueMaxLimit", M2Share.g_Config.nHelMetSCAddValueMaxLimit);
            M2Share.g_Config.nHelMetSCAddValueMaxLimit = Config.ReadInteger("Setup", "HelMetSCAddValueMaxLimit",
                M2Share.g_Config.nHelMetSCAddValueMaxLimit);
            if (Config.ReadInteger("Setup", "HelMetSCAddValueRate", -1) < 0)
                Config.WriteInteger("Setup", "HelMetSCAddValueRate", M2Share.g_Config.nHelMetSCAddValueRate);
            M2Share.g_Config.nHelMetSCAddValueRate = Config.ReadInteger("Setup", "HelMetSCAddValueRate",
                M2Share.g_Config.nHelMetSCAddValueRate);
            if (Config.ReadInteger("Setup", "HelMetSCAddRate", -1) < 0)
                Config.WriteInteger("Setup", "HelMetSCAddRate", M2Share.g_Config.nHelMetSCAddRate);
            M2Share.g_Config.nHelMetSCAddRate =
                Config.ReadInteger("Setup", "HelMetSCAddRate", M2Share.g_Config.nHelMetSCAddRate);
            nLoadInteger = Config.ReadInteger("Setup", "UnknowHelMetACAddRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowHelMetACAddRate", M2Share.g_Config.nUnknowHelMetACAddRate);
            else
                M2Share.g_Config.nUnknowHelMetACAddRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowHelMetACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowHelMetACAddValueMaxLimit",
                    M2Share.g_Config.nUnknowHelMetACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowHelMetACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowHelMetMACAddRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowHelMetMACAddRate", M2Share.g_Config.nUnknowHelMetMACAddRate);
            else
                M2Share.g_Config.nUnknowHelMetMACAddRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowHelMetMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowHelMetMACAddValueMaxLimit",
                    M2Share.g_Config.nUnknowHelMetMACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowHelMetMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowHelMetDCAddRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowHelMetDCAddRate", M2Share.g_Config.nUnknowHelMetDCAddRate);
            else
                M2Share.g_Config.nUnknowHelMetDCAddRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowHelMetDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowHelMetDCAddValueMaxLimit",
                    M2Share.g_Config.nUnknowHelMetDCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowHelMetDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowHelMetMCAddRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowHelMetMCAddRate", M2Share.g_Config.nUnknowHelMetMCAddRate);
            else
                M2Share.g_Config.nUnknowHelMetMCAddRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowHelMetMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowHelMetMCAddValueMaxLimit",
                    M2Share.g_Config.nUnknowHelMetMCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowHelMetMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowHelMetSCAddRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowHelMetSCAddRate", M2Share.g_Config.nUnknowHelMetSCAddRate);
            else
                M2Share.g_Config.nUnknowHelMetSCAddRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowHelMetSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowHelMetSCAddValueMaxLimit",
                    M2Share.g_Config.nUnknowHelMetSCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowHelMetSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowNecklaceACAddRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowNecklaceACAddRate", M2Share.g_Config.nUnknowNecklaceACAddRate);
            else
                M2Share.g_Config.nUnknowNecklaceACAddRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowNecklaceACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowNecklaceACAddValueMaxLimit",
                    M2Share.g_Config.nUnknowNecklaceACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowNecklaceACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowNecklaceMACAddRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowNecklaceMACAddRate", M2Share.g_Config.nUnknowNecklaceMACAddRate);
            else
                M2Share.g_Config.nUnknowNecklaceMACAddRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowNecklaceMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowNecklaceMACAddValueMaxLimit",
                    M2Share.g_Config.nUnknowNecklaceMACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowNecklaceMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowNecklaceDCAddRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowNecklaceDCAddRate", M2Share.g_Config.nUnknowNecklaceDCAddRate);
            else
                M2Share.g_Config.nUnknowNecklaceDCAddRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowNecklaceDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowNecklaceDCAddValueMaxLimit",
                    M2Share.g_Config.nUnknowNecklaceDCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowNecklaceDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowNecklaceMCAddRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowNecklaceMCAddRate", M2Share.g_Config.nUnknowNecklaceMCAddRate);
            else
                M2Share.g_Config.nUnknowNecklaceMCAddRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowNecklaceMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowNecklaceMCAddValueMaxLimit",
                    M2Share.g_Config.nUnknowNecklaceMCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowNecklaceMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowNecklaceSCAddRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowNecklaceSCAddRate", M2Share.g_Config.nUnknowNecklaceSCAddRate);
            else
                M2Share.g_Config.nUnknowNecklaceSCAddRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowNecklaceSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowNecklaceSCAddValueMaxLimit",
                    M2Share.g_Config.nUnknowNecklaceSCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowNecklaceSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowRingACAddRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowRingACAddRate", M2Share.g_Config.nUnknowRingACAddRate);
            else
                M2Share.g_Config.nUnknowRingACAddRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowRingACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowRingACAddValueMaxLimit",
                    M2Share.g_Config.nUnknowRingACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowRingACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowRingMACAddRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowRingMACAddRate", M2Share.g_Config.nUnknowRingMACAddRate);
            else
                M2Share.g_Config.nUnknowRingMACAddRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowRingMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowRingMACAddValueMaxLimit",
                    M2Share.g_Config.nUnknowRingMACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowRingMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowRingDCAddRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowRingDCAddRate", M2Share.g_Config.nUnknowRingDCAddRate);
            else
                M2Share.g_Config.nUnknowRingDCAddRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowRingDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowRingDCAddValueMaxLimit",
                    M2Share.g_Config.nUnknowRingDCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowRingDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowRingMCAddRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowRingMCAddRate", M2Share.g_Config.nUnknowRingMCAddRate);
            else
                M2Share.g_Config.nUnknowRingMCAddRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowRingMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowRingMCAddValueMaxLimit",
                    M2Share.g_Config.nUnknowRingMCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowRingMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowRingSCAddRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowRingSCAddRate", M2Share.g_Config.nUnknowRingSCAddRate);
            else
                M2Share.g_Config.nUnknowRingSCAddRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "UnknowRingSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UnknowRingSCAddValueMaxLimit",
                    M2Share.g_Config.nUnknowRingSCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowRingSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "MonOneDropGoldCount", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "MonOneDropGoldCount", M2Share.g_Config.nMonOneDropGoldCount);
            else
                M2Share.g_Config.nMonOneDropGoldCount = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "SendCurTickCount", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "SendCurTickCount", M2Share.g_Config.boSendCurTickCount);
            else
                M2Share.g_Config.boSendCurTickCount = nLoadInteger == 1;
            if (Config.ReadInteger("Setup", "MakeMineHitRate", -1) < 0)
                Config.WriteInteger("Setup", "MakeMineHitRate", M2Share.g_Config.nMakeMineHitRate);
            M2Share.g_Config.nMakeMineHitRate =
                Config.ReadInteger("Setup", "MakeMineHitRate", M2Share.g_Config.nMakeMineHitRate);
            if (Config.ReadInteger("Setup", "MakeMineRate", -1) < 0)
                Config.WriteInteger("Setup", "MakeMineRate", M2Share.g_Config.nMakeMineRate);
            M2Share.g_Config.nMakeMineRate =
                Config.ReadInteger("Setup", "MakeMineRate", M2Share.g_Config.nMakeMineRate);
            if (Config.ReadInteger("Setup", "StoneTypeRate", -1) < 0)
                Config.WriteInteger("Setup", "StoneTypeRate", M2Share.g_Config.nStoneTypeRate);
            M2Share.g_Config.nStoneTypeRate =
                Config.ReadInteger("Setup", "StoneTypeRate", M2Share.g_Config.nStoneTypeRate);
            if (Config.ReadInteger("Setup", "StoneTypeRateMin", -1) < 0)
                Config.WriteInteger("Setup", "StoneTypeRateMin", M2Share.g_Config.nStoneTypeRateMin);
            M2Share.g_Config.nStoneTypeRateMin =
                Config.ReadInteger("Setup", "StoneTypeRateMin", M2Share.g_Config.nStoneTypeRateMin);
            if (Config.ReadInteger("Setup", "GoldStoneMin", -1) < 0)
                Config.WriteInteger("Setup", "GoldStoneMin", M2Share.g_Config.nGoldStoneMin);
            M2Share.g_Config.nGoldStoneMin =
                Config.ReadInteger("Setup", "GoldStoneMin", M2Share.g_Config.nGoldStoneMin);
            if (Config.ReadInteger("Setup", "GoldStoneMax", -1) < 0)
                Config.WriteInteger("Setup", "GoldStoneMax", M2Share.g_Config.nGoldStoneMax);
            M2Share.g_Config.nGoldStoneMax =
                Config.ReadInteger("Setup", "GoldStoneMax", M2Share.g_Config.nGoldStoneMax);
            if (Config.ReadInteger("Setup", "SilverStoneMin", -1) < 0)
                Config.WriteInteger("Setup", "SilverStoneMin", M2Share.g_Config.nSilverStoneMin);
            M2Share.g_Config.nSilverStoneMin =
                Config.ReadInteger("Setup", "SilverStoneMin", M2Share.g_Config.nSilverStoneMin);
            if (Config.ReadInteger("Setup", "SilverStoneMax", -1) < 0)
                Config.WriteInteger("Setup", "SilverStoneMax", M2Share.g_Config.nSilverStoneMax);
            M2Share.g_Config.nSilverStoneMax =
                Config.ReadInteger("Setup", "SilverStoneMax", M2Share.g_Config.nSilverStoneMax);
            if (Config.ReadInteger("Setup", "SteelStoneMin", -1) < 0)
                Config.WriteInteger("Setup", "SteelStoneMin", M2Share.g_Config.nSteelStoneMin);
            M2Share.g_Config.nSteelStoneMin =
                Config.ReadInteger("Setup", "SteelStoneMin", M2Share.g_Config.nSteelStoneMin);
            if (Config.ReadInteger("Setup", "SteelStoneMax", -1) < 0)
                Config.WriteInteger("Setup", "SteelStoneMax", M2Share.g_Config.nSteelStoneMax);
            M2Share.g_Config.nSteelStoneMax =
                Config.ReadInteger("Setup", "SteelStoneMax", M2Share.g_Config.nSteelStoneMax);
            if (Config.ReadInteger("Setup", "BlackStoneMin", -1) < 0)
                Config.WriteInteger("Setup", "BlackStoneMin", M2Share.g_Config.nBlackStoneMin);
            M2Share.g_Config.nBlackStoneMin =
                Config.ReadInteger("Setup", "BlackStoneMin", M2Share.g_Config.nBlackStoneMin);
            if (Config.ReadInteger("Setup", "BlackStoneMax", -1) < 0)
                Config.WriteInteger("Setup", "BlackStoneMax", M2Share.g_Config.nBlackStoneMax);
            M2Share.g_Config.nBlackStoneMax =
                Config.ReadInteger("Setup", "BlackStoneMax", M2Share.g_Config.nBlackStoneMax);
            if (Config.ReadInteger("Setup", "StoneMinDura", -1) < 0)
                Config.WriteInteger("Setup", "StoneMinDura", M2Share.g_Config.nStoneMinDura);
            M2Share.g_Config.nStoneMinDura =
                Config.ReadInteger("Setup", "StoneMinDura", M2Share.g_Config.nStoneMinDura);
            if (Config.ReadInteger("Setup", "StoneGeneralDuraRate", -1) < 0)
                Config.WriteInteger("Setup", "StoneGeneralDuraRate", M2Share.g_Config.nStoneGeneralDuraRate);
            M2Share.g_Config.nStoneGeneralDuraRate = Config.ReadInteger("Setup", "StoneGeneralDuraRate",
                M2Share.g_Config.nStoneGeneralDuraRate);
            if (Config.ReadInteger("Setup", "StoneAddDuraRate", -1) < 0)
                Config.WriteInteger("Setup", "StoneAddDuraRate", M2Share.g_Config.nStoneAddDuraRate);
            M2Share.g_Config.nStoneAddDuraRate =
                Config.ReadInteger("Setup", "StoneAddDuraRate", M2Share.g_Config.nStoneAddDuraRate);
            if (Config.ReadInteger("Setup", "StoneAddDuraMax", -1) < 0)
                Config.WriteInteger("Setup", "StoneAddDuraMax", M2Share.g_Config.nStoneAddDuraMax);
            M2Share.g_Config.nStoneAddDuraMax =
                Config.ReadInteger("Setup", "StoneAddDuraMax", M2Share.g_Config.nStoneAddDuraMax);
            if (Config.ReadInteger("Setup", "WinLottery1Min", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery1Min", M2Share.g_Config.nWinLottery1Min);
            M2Share.g_Config.nWinLottery1Min =
                Config.ReadInteger("Setup", "WinLottery1Min", M2Share.g_Config.nWinLottery1Min);
            if (Config.ReadInteger("Setup", "WinLottery1Max", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery1Max", M2Share.g_Config.nWinLottery1Max);
            M2Share.g_Config.nWinLottery1Max =
                Config.ReadInteger("Setup", "WinLottery1Max", M2Share.g_Config.nWinLottery1Max);
            if (Config.ReadInteger("Setup", "WinLottery2Min", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery2Min", M2Share.g_Config.nWinLottery2Min);
            M2Share.g_Config.nWinLottery2Min =
                Config.ReadInteger("Setup", "WinLottery2Min", M2Share.g_Config.nWinLottery2Min);
            if (Config.ReadInteger("Setup", "WinLottery2Max", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery2Max", M2Share.g_Config.nWinLottery2Max);
            M2Share.g_Config.nWinLottery2Max =
                Config.ReadInteger("Setup", "WinLottery2Max", M2Share.g_Config.nWinLottery2Max);
            if (Config.ReadInteger("Setup", "WinLottery3Min", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery3Min", M2Share.g_Config.nWinLottery3Min);
            M2Share.g_Config.nWinLottery3Min =
                Config.ReadInteger("Setup", "WinLottery3Min", M2Share.g_Config.nWinLottery3Min);
            if (Config.ReadInteger("Setup", "WinLottery3Max", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery3Max", M2Share.g_Config.nWinLottery3Max);
            M2Share.g_Config.nWinLottery3Max =
                Config.ReadInteger("Setup", "WinLottery3Max", M2Share.g_Config.nWinLottery3Max);
            if (Config.ReadInteger("Setup", "WinLottery4Min", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery4Min", M2Share.g_Config.nWinLottery4Min);
            M2Share.g_Config.nWinLottery4Min =
                Config.ReadInteger("Setup", "WinLottery4Min", M2Share.g_Config.nWinLottery4Min);
            if (Config.ReadInteger("Setup", "WinLottery4Max", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery4Max", M2Share.g_Config.nWinLottery4Max);
            M2Share.g_Config.nWinLottery4Max =
                Config.ReadInteger("Setup", "WinLottery4Max", M2Share.g_Config.nWinLottery4Max);
            if (Config.ReadInteger("Setup", "WinLottery5Min", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery5Min", M2Share.g_Config.nWinLottery5Min);
            M2Share.g_Config.nWinLottery5Min =
                Config.ReadInteger("Setup", "WinLottery5Min", M2Share.g_Config.nWinLottery5Min);
            if (Config.ReadInteger("Setup", "WinLottery5Max", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery5Max", M2Share.g_Config.nWinLottery5Max);
            M2Share.g_Config.nWinLottery5Max =
                Config.ReadInteger("Setup", "WinLottery5Max", M2Share.g_Config.nWinLottery5Max);
            if (Config.ReadInteger("Setup", "WinLottery6Min", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery6Min", M2Share.g_Config.nWinLottery6Min);
            M2Share.g_Config.nWinLottery6Min =
                Config.ReadInteger("Setup", "WinLottery6Min", M2Share.g_Config.nWinLottery6Min);
            if (Config.ReadInteger("Setup", "WinLottery6Max", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery6Max", M2Share.g_Config.nWinLottery6Max);
            M2Share.g_Config.nWinLottery6Max =
                Config.ReadInteger("Setup", "WinLottery6Max", M2Share.g_Config.nWinLottery6Max);
            if (Config.ReadInteger("Setup", "WinLotteryRate", -1) < 0)
                Config.WriteInteger("Setup", "WinLotteryRate", M2Share.g_Config.nWinLotteryRate);
            M2Share.g_Config.nWinLotteryRate =
                Config.ReadInteger("Setup", "WinLotteryRate", M2Share.g_Config.nWinLotteryRate);
            if (Config.ReadInteger("Setup", "WinLottery1Gold", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery1Gold", M2Share.g_Config.nWinLottery1Gold);
            M2Share.g_Config.nWinLottery1Gold =
                Config.ReadInteger("Setup", "WinLottery1Gold", M2Share.g_Config.nWinLottery1Gold);
            if (Config.ReadInteger("Setup", "WinLottery2Gold", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery2Gold", M2Share.g_Config.nWinLottery2Gold);
            M2Share.g_Config.nWinLottery2Gold =
                Config.ReadInteger("Setup", "WinLottery2Gold", M2Share.g_Config.nWinLottery2Gold);
            if (Config.ReadInteger("Setup", "WinLottery3Gold", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery3Gold", M2Share.g_Config.nWinLottery3Gold);
            M2Share.g_Config.nWinLottery3Gold =
                Config.ReadInteger("Setup", "WinLottery3Gold", M2Share.g_Config.nWinLottery3Gold);
            if (Config.ReadInteger("Setup", "WinLottery4Gold", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery4Gold", M2Share.g_Config.nWinLottery4Gold);
            M2Share.g_Config.nWinLottery4Gold =
                Config.ReadInteger("Setup", "WinLottery4Gold", M2Share.g_Config.nWinLottery4Gold);
            if (Config.ReadInteger("Setup", "WinLottery5Gold", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery5Gold", M2Share.g_Config.nWinLottery5Gold);
            M2Share.g_Config.nWinLottery5Gold =
                Config.ReadInteger("Setup", "WinLottery5Gold", M2Share.g_Config.nWinLottery5Gold);
            if (Config.ReadInteger("Setup", "WinLottery6Gold", -1) < 0)
                Config.WriteInteger("Setup", "WinLottery6Gold", M2Share.g_Config.nWinLottery6Gold);
            M2Share.g_Config.nWinLottery6Gold =
                Config.ReadInteger("Setup", "WinLottery6Gold", M2Share.g_Config.nWinLottery6Gold);
            if (Config.ReadInteger("Setup", "GuildRecallTime", -1) < 0)
                Config.WriteInteger("Setup", "GuildRecallTime", M2Share.g_Config.nGuildRecallTime);
            M2Share.g_Config.nGuildRecallTime =
                Config.ReadInteger("Setup", "GuildRecallTime", M2Share.g_Config.nGuildRecallTime);
            if (Config.ReadInteger("Setup", "GroupRecallTime", -1) < 0)
                Config.WriteInteger("Setup", "GroupRecallTime", M2Share.g_Config.nGroupRecallTime);
            M2Share.g_Config.nGroupRecallTime =
                Config.ReadInteger("Setup", "GroupRecallTime", M2Share.g_Config.nGroupRecallTime);
            if (Config.ReadInteger("Setup", "ControlDropItem", -1) < 0)
                Config.WriteBool("Setup", "ControlDropItem", M2Share.g_Config.boControlDropItem);
            M2Share.g_Config.boControlDropItem =
                Config.ReadBool("Setup", "ControlDropItem", M2Share.g_Config.boControlDropItem);
            if (Config.ReadInteger("Setup", "InSafeDisableDrop", -1) < 0)
                Config.WriteBool("Setup", "InSafeDisableDrop", M2Share.g_Config.boInSafeDisableDrop);
            M2Share.g_Config.boInSafeDisableDrop =
                Config.ReadBool("Setup", "InSafeDisableDrop", M2Share.g_Config.boInSafeDisableDrop);
            if (Config.ReadInteger("Setup", "CanDropGold", -1) < 0)
                Config.WriteInteger("Setup", "CanDropGold", M2Share.g_Config.nCanDropGold);
            M2Share.g_Config.nCanDropGold = Config.ReadInteger("Setup", "CanDropGold", M2Share.g_Config.nCanDropGold);
            if (Config.ReadInteger("Setup", "CanDropPrice", -1) < 0)
                Config.WriteInteger("Setup", "CanDropPrice", M2Share.g_Config.nCanDropPrice);
            M2Share.g_Config.nCanDropPrice =
                Config.ReadInteger("Setup", "CanDropPrice", M2Share.g_Config.nCanDropPrice);
            nLoadInteger = Config.ReadInteger("Setup", "SendCustemMsg", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "SendCustemMsg", M2Share.g_Config.boSendCustemMsg);
            else
                M2Share.g_Config.boSendCustemMsg = nLoadInteger == 1;
            nLoadInteger = Config.ReadInteger("Setup", "SubkMasterSendMsg", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "SubkMasterSendMsg", M2Share.g_Config.boSubkMasterSendMsg);
            else
                M2Share.g_Config.boSubkMasterSendMsg = nLoadInteger == 1;
            if (Config.ReadInteger("Setup", "SuperRepairPriceRate", -1) < 0)
                Config.WriteInteger("Setup", "SuperRepairPriceRate", M2Share.g_Config.nSuperRepairPriceRate);
            M2Share.g_Config.nSuperRepairPriceRate = Config.ReadInteger("Setup", "SuperRepairPriceRate",
                M2Share.g_Config.nSuperRepairPriceRate);
            if (Config.ReadInteger("Setup", "RepairItemDecDura", -1) < 0)
                Config.WriteInteger("Setup", "RepairItemDecDura", M2Share.g_Config.nRepairItemDecDura);
            M2Share.g_Config.nRepairItemDecDura =
                Config.ReadInteger("Setup", "RepairItemDecDura", M2Share.g_Config.nRepairItemDecDura);
            if (Config.ReadInteger("Setup", "DieScatterBag", -1) < 0)
                Config.WriteBool("Setup", "DieScatterBag", M2Share.g_Config.boDieScatterBag);
            M2Share.g_Config.boDieScatterBag =
                Config.ReadBool("Setup", "DieScatterBag", M2Share.g_Config.boDieScatterBag);
            if (Config.ReadInteger("Setup", "DieScatterBagRate", -1) < 0)
                Config.WriteInteger("Setup", "DieScatterBagRate", M2Share.g_Config.nDieScatterBagRate);
            M2Share.g_Config.nDieScatterBagRate =
                Config.ReadInteger("Setup", "DieScatterBagRate", M2Share.g_Config.nDieScatterBagRate);
            if (Config.ReadInteger("Setup", "DieRedScatterBagAll", -1) < 0)
                Config.WriteBool("Setup", "DieRedScatterBagAll", M2Share.g_Config.boDieRedScatterBagAll);
            M2Share.g_Config.boDieRedScatterBagAll = Config.ReadBool("Setup", "DieRedScatterBagAll",
                M2Share.g_Config.boDieRedScatterBagAll);
            if (Config.ReadInteger("Setup", "DieDropUseItemRate", -1) < 0)
                Config.WriteInteger("Setup", "DieDropUseItemRate", M2Share.g_Config.nDieDropUseItemRate);
            M2Share.g_Config.nDieDropUseItemRate =
                Config.ReadInteger("Setup", "DieDropUseItemRate", M2Share.g_Config.nDieDropUseItemRate);
            if (Config.ReadInteger("Setup", "DieRedDropUseItemRate", -1) < 0)
                Config.WriteInteger("Setup", "DieRedDropUseItemRate", M2Share.g_Config.nDieRedDropUseItemRate);
            M2Share.g_Config.nDieRedDropUseItemRate = Config.ReadInteger("Setup", "DieRedDropUseItemRate",
                M2Share.g_Config.nDieRedDropUseItemRate);
            if (Config.ReadInteger("Setup", "DieDropGold", -1) < 0)
                Config.WriteBool("Setup", "DieDropGold", M2Share.g_Config.boDieDropGold);
            M2Share.g_Config.boDieDropGold = Config.ReadBool("Setup", "DieDropGold", M2Share.g_Config.boDieDropGold);
            if (Config.ReadInteger("Setup", "KillByHumanDropUseItem", -1) < 0)
                Config.WriteBool("Setup", "KillByHumanDropUseItem", M2Share.g_Config.boKillByHumanDropUseItem);
            M2Share.g_Config.boKillByHumanDropUseItem = Config.ReadBool("Setup", "KillByHumanDropUseItem",
                M2Share.g_Config.boKillByHumanDropUseItem);
            if (Config.ReadInteger("Setup", "KillByMonstDropUseItem", -1) < 0)
                Config.WriteBool("Setup", "KillByMonstDropUseItem", M2Share.g_Config.boKillByMonstDropUseItem);
            M2Share.g_Config.boKillByMonstDropUseItem = Config.ReadBool("Setup", "KillByMonstDropUseItem",
                M2Share.g_Config.boKillByMonstDropUseItem);
            if (Config.ReadInteger("Setup", "KickExpireHuman", -1) < 0)
                Config.WriteBool("Setup", "KickExpireHuman", M2Share.g_Config.boKickExpireHuman);
            M2Share.g_Config.boKickExpireHuman =
                Config.ReadBool("Setup", "KickExpireHuman", M2Share.g_Config.boKickExpireHuman);
            if (Config.ReadInteger("Setup", "GuildRankNameLen", -1) < 0)
                Config.WriteInteger("Setup", "GuildRankNameLen", M2Share.g_Config.nGuildRankNameLen);
            M2Share.g_Config.nGuildRankNameLen =
                Config.ReadInteger("Setup", "GuildRankNameLen", M2Share.g_Config.nGuildRankNameLen);
            if (Config.ReadInteger("Setup", "GuildNameLen", -1) < 0)
                Config.WriteInteger("Setup", "GuildNameLen", M2Share.g_Config.nGuildNameLen);
            M2Share.g_Config.nGuildNameLen =
                Config.ReadInteger("Setup", "GuildNameLen", M2Share.g_Config.nGuildNameLen);
            if (Config.ReadInteger("Setup", "GuildMemberMaxLimit", -1) < 0)
                Config.WriteInteger("Setup", "GuildMemberMaxLimit", M2Share.g_Config.nGuildMemberMaxLimit);
            M2Share.g_Config.nGuildMemberMaxLimit =
                Config.ReadInteger("Setup", "GuildMemberMaxLimit", M2Share.g_Config.nGuildMemberMaxLimit);
            if (Config.ReadInteger("Setup", "AttackPosionRate", -1) < 0)
                Config.WriteInteger("Setup", "AttackPosionRate", M2Share.g_Config.nAttackPosionRate);
            M2Share.g_Config.nAttackPosionRate =
                Config.ReadInteger("Setup", "AttackPosionRate", M2Share.g_Config.nAttackPosionRate);
            if (Config.ReadInteger("Setup", "AttackPosionTime", -1) < 0)
                Config.WriteInteger("Setup", "AttackPosionTime", M2Share.g_Config.nAttackPosionTime);
            M2Share.g_Config.nAttackPosionTime =
                Config.ReadInteger("Setup", "AttackPosionTime", M2Share.g_Config.nAttackPosionTime);
            if (Config.ReadInteger("Setup", "RevivalTime", -1) < 0)
                Config.WriteInteger("Setup", "RevivalTime", M2Share.g_Config.dwRevivalTime);
            M2Share.g_Config.dwRevivalTime = Config.ReadInteger("Setup", "RevivalTime", M2Share.g_Config.dwRevivalTime);
            nLoadInteger = Config.ReadInteger("Setup", "UserMoveCanDupObj", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "UserMoveCanDupObj", M2Share.g_Config.boUserMoveCanDupObj);
            else
                M2Share.g_Config.boUserMoveCanDupObj = nLoadInteger == 1;
            nLoadInteger = Config.ReadInteger("Setup", "UserMoveCanOnItem", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "UserMoveCanOnItem", M2Share.g_Config.boUserMoveCanOnItem);
            else
                M2Share.g_Config.boUserMoveCanOnItem = nLoadInteger == 1;
            nLoadInteger = Config.ReadInteger("Setup", "UserMoveTime", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "UserMoveTime", M2Share.g_Config.dwUserMoveTime);
            else
                M2Share.g_Config.dwUserMoveTime = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "PKDieLostExpRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "PKDieLostExpRate", M2Share.g_Config.dwPKDieLostExpRate);
            else
                M2Share.g_Config.dwPKDieLostExpRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "PKDieLostLevelRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "PKDieLostLevelRate", M2Share.g_Config.nPKDieLostLevelRate);
            else
                M2Share.g_Config.nPKDieLostLevelRate = nLoadInteger;
            if (Config.ReadInteger("Setup", "PKFlagNameColor", -1) < 0)
                Config.WriteInteger("Setup", "PKFlagNameColor", M2Share.g_Config.btPKFlagNameColor);
            M2Share.g_Config.btPKFlagNameColor =
                Config.ReadInteger<byte>("Setup", "PKFlagNameColor", M2Share.g_Config.btPKFlagNameColor);
            if (Config.ReadInteger("Setup", "AllyAndGuildNameColor", -1) < 0)
                Config.WriteInteger("Setup", "AllyAndGuildNameColor", M2Share.g_Config.btAllyAndGuildNameColor);
            M2Share.g_Config.btAllyAndGuildNameColor = Config.ReadInteger<byte>("Setup", "AllyAndGuildNameColor",
                M2Share.g_Config.btAllyAndGuildNameColor);
            if (Config.ReadInteger("Setup", "WarGuildNameColor", -1) < 0)
                Config.WriteInteger("Setup", "WarGuildNameColor", M2Share.g_Config.btWarGuildNameColor);
            M2Share.g_Config.btWarGuildNameColor =
                Config.ReadInteger<byte>("Setup", "WarGuildNameColor", M2Share.g_Config.btWarGuildNameColor);
            if (Config.ReadInteger("Setup", "InFreePKAreaNameColor", -1) < 0)
                Config.WriteInteger("Setup", "InFreePKAreaNameColor", M2Share.g_Config.btInFreePKAreaNameColor);
            M2Share.g_Config.btInFreePKAreaNameColor = Config.ReadInteger<byte>("Setup", "InFreePKAreaNameColor",
                M2Share.g_Config.btInFreePKAreaNameColor);
            if (Config.ReadInteger("Setup", "PKLevel1NameColor", -1) < 0)
                Config.WriteInteger("Setup", "PKLevel1NameColor", M2Share.g_Config.btPKLevel1NameColor);
            M2Share.g_Config.btPKLevel1NameColor =
                Config.ReadInteger<byte>("Setup", "PKLevel1NameColor", M2Share.g_Config.btPKLevel1NameColor);
            if (Config.ReadInteger("Setup", "PKLevel2NameColor", -1) < 0)
                Config.WriteInteger("Setup", "PKLevel2NameColor", M2Share.g_Config.btPKLevel2NameColor);
            M2Share.g_Config.btPKLevel2NameColor =
                Config.ReadInteger<byte>("Setup", "PKLevel2NameColor", M2Share.g_Config.btPKLevel2NameColor);
            if (Config.ReadInteger("Setup", "SpiritMutiny", -1) < 0)
                Config.WriteBool("Setup", "SpiritMutiny", M2Share.g_Config.boSpiritMutiny);
            M2Share.g_Config.boSpiritMutiny = Config.ReadBool("Setup", "SpiritMutiny", M2Share.g_Config.boSpiritMutiny);
            if (Config.ReadInteger("Setup", "SpiritMutinyTime", -1) < 0)
                Config.WriteInteger("Setup", "SpiritMutinyTime", M2Share.g_Config.dwSpiritMutinyTime);
            M2Share.g_Config.dwSpiritMutinyTime =
                Config.ReadInteger("Setup", "SpiritMutinyTime", M2Share.g_Config.dwSpiritMutinyTime);
            if (Config.ReadInteger("Setup", "SpiritPowerRate", -1) < 0)
                Config.WriteInteger("Setup", "SpiritPowerRate", M2Share.g_Config.nSpiritPowerRate);
            M2Share.g_Config.nSpiritPowerRate =
                Config.ReadInteger("Setup", "SpiritPowerRate", M2Share.g_Config.nSpiritPowerRate);
            if (Config.ReadInteger("Setup", "MasterDieMutiny", -1) < 0)
                Config.WriteBool("Setup", "MasterDieMutiny", M2Share.g_Config.boMasterDieMutiny);
            M2Share.g_Config.boMasterDieMutiny =
                Config.ReadBool("Setup", "MasterDieMutiny", M2Share.g_Config.boMasterDieMutiny);
            if (Config.ReadInteger("Setup", "MasterDieMutinyRate", -1) < 0)
                Config.WriteInteger("Setup", "MasterDieMutinyRate", M2Share.g_Config.nMasterDieMutinyRate);
            M2Share.g_Config.nMasterDieMutinyRate =
                Config.ReadInteger("Setup", "MasterDieMutinyRate", M2Share.g_Config.nMasterDieMutinyRate);
            if (Config.ReadInteger("Setup", "MasterDieMutinyPower", -1) < 0)
                Config.WriteInteger("Setup", "MasterDieMutinyPower", M2Share.g_Config.nMasterDieMutinyPower);
            M2Share.g_Config.nMasterDieMutinyPower = Config.ReadInteger("Setup", "MasterDieMutinyPower",
                M2Share.g_Config.nMasterDieMutinyPower);
            if (Config.ReadInteger("Setup", "MasterDieMutinyPower", -1) < 0)
                Config.WriteInteger("Setup", "MasterDieMutinyPower", M2Share.g_Config.nMasterDieMutinySpeed);
            M2Share.g_Config.nMasterDieMutinySpeed = Config.ReadInteger("Setup", "MasterDieMutinyPower",
                M2Share.g_Config.nMasterDieMutinySpeed);
            nLoadInteger = Config.ReadInteger("Setup", "BBMonAutoChangeColor", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "BBMonAutoChangeColor", M2Share.g_Config.boBBMonAutoChangeColor);
            else
                M2Share.g_Config.boBBMonAutoChangeColor = nLoadInteger == 1;
            nLoadInteger = Config.ReadInteger("Setup", "BBMonAutoChangeColorTime", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "BBMonAutoChangeColorTime", M2Share.g_Config.dwBBMonAutoChangeColorTime);
            else
                M2Share.g_Config.dwBBMonAutoChangeColorTime = nLoadInteger;
            if (Config.ReadInteger("Setup", "OldClientShowHiLevel", -1) < 0)
                Config.WriteBool("Setup", "OldClientShowHiLevel", M2Share.g_Config.boOldClientShowHiLevel);
            M2Share.g_Config.boOldClientShowHiLevel = Config.ReadBool("Setup", "OldClientShowHiLevel",
                M2Share.g_Config.boOldClientShowHiLevel);
            if (Config.ReadInteger("Setup", "ShowScriptActionMsg", -1) < 0)
                Config.WriteBool("Setup", "ShowScriptActionMsg", M2Share.g_Config.boShowScriptActionMsg);
            M2Share.g_Config.boShowScriptActionMsg = Config.ReadBool("Setup", "ShowScriptActionMsg",
                M2Share.g_Config.boShowScriptActionMsg);
            if (Config.ReadInteger("Setup", "RunSocketDieLoopLimit", -1) < 0)
                Config.WriteInteger("Setup", "RunSocketDieLoopLimit", M2Share.g_Config.nRunSocketDieLoopLimit);
            M2Share.g_Config.nRunSocketDieLoopLimit = Config.ReadInteger("Setup", "RunSocketDieLoopLimit",
                M2Share.g_Config.nRunSocketDieLoopLimit);
            if (Config.ReadInteger("Setup", "ThreadRun", -1) < 0)
                Config.WriteBool("Setup", "ThreadRun", M2Share.g_Config.boThreadRun);
            M2Share.g_Config.boThreadRun = Config.ReadBool("Setup", "ThreadRun", M2Share.g_Config.boThreadRun);
            if (Config.ReadInteger("Setup", "DeathColorEffect", -1) < 0)
                Config.WriteInteger("Setup", "DeathColorEffect", M2Share.g_Config.ClientConf.btDieColor);
            M2Share.g_Config.ClientConf.btDieColor =
                Config.ReadInteger<byte>("Setup", "DeathColorEffect", M2Share.g_Config.ClientConf.btDieColor);
            if (Config.ReadInteger("Setup", "ParalyCanRun", -1) < 0)
                Config.WriteBool("Setup", "ParalyCanRun", M2Share.g_Config.ClientConf.boParalyCanRun);
            M2Share.g_Config.ClientConf.boParalyCanRun =
                Config.ReadBool("Setup", "ParalyCanRun", M2Share.g_Config.ClientConf.boParalyCanRun);
            if (Config.ReadInteger("Setup", "ParalyCanWalk", -1) < 0)
                Config.WriteBool("Setup", "ParalyCanWalk", M2Share.g_Config.ClientConf.boParalyCanWalk);
            M2Share.g_Config.ClientConf.boParalyCanWalk = Config.ReadBool("Setup", "ParalyCanWalk",
                M2Share.g_Config.ClientConf.boParalyCanWalk);
            if (Config.ReadInteger("Setup", "ParalyCanHit", -1) < 0)
                Config.WriteBool("Setup", "ParalyCanHit", M2Share.g_Config.ClientConf.boParalyCanHit);
            M2Share.g_Config.ClientConf.boParalyCanHit =
                Config.ReadBool("Setup", "ParalyCanHit", M2Share.g_Config.ClientConf.boParalyCanHit);
            if (Config.ReadInteger("Setup", "ParalyCanSpell", -1) < 0)
                Config.WriteBool("Setup", "ParalyCanSpell", M2Share.g_Config.ClientConf.boParalyCanSpell);
            M2Share.g_Config.ClientConf.boParalyCanSpell = Config.ReadBool("Setup", "ParalyCanSpell",
                M2Share.g_Config.ClientConf.boParalyCanSpell);
            if (Config.ReadInteger("Setup", "ShowExceptionMsg", -1) < 0)
                Config.WriteBool("Setup", "ShowExceptionMsg", M2Share.g_Config.boShowExceptionMsg);
            M2Share.g_Config.boShowExceptionMsg =
                Config.ReadBool("Setup", "ShowExceptionMsg", M2Share.g_Config.boShowExceptionMsg);
            if (Config.ReadInteger("Setup", "ShowPreFixMsg", -1) < 0)
                Config.WriteBool("Setup", "ShowPreFixMsg", M2Share.g_Config.boShowPreFixMsg);
            M2Share.g_Config.boShowPreFixMsg =
                Config.ReadBool("Setup", "ShowPreFixMsg", M2Share.g_Config.boShowPreFixMsg);
            if (Config.ReadInteger("Setup", "MagTurnUndeadLevel", -1) < 0)
                Config.WriteInteger("Setup", "MagTurnUndeadLevel", M2Share.g_Config.nMagTurnUndeadLevel);
            M2Share.g_Config.nMagTurnUndeadLevel =
                Config.ReadInteger("Setup", "MagTurnUndeadLevel", M2Share.g_Config.nMagTurnUndeadLevel);
            nLoadInteger = Config.ReadInteger("Setup", "MagTammingLevel", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "MagTammingLevel", M2Share.g_Config.nMagTammingLevel);
            else
                M2Share.g_Config.nMagTammingLevel = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "MagTammingTargetLevel", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "MagTammingTargetLevel", M2Share.g_Config.nMagTammingTargetLevel);
            else
                M2Share.g_Config.nMagTammingTargetLevel = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "MagTammingTargetHPRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "MagTammingTargetHPRate", M2Share.g_Config.nMagTammingHPRate);
            else
                M2Share.g_Config.nMagTammingHPRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "MagTammingCount", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "MagTammingCount", M2Share.g_Config.nMagTammingCount);
            else
                M2Share.g_Config.nMagTammingCount = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "MabMabeHitRandRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "MabMabeHitRandRate", M2Share.g_Config.nMabMabeHitRandRate);
            else
                M2Share.g_Config.nMabMabeHitRandRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "MabMabeHitMinLvLimit", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "MabMabeHitMinLvLimit", M2Share.g_Config.nMabMabeHitMinLvLimit);
            else
                M2Share.g_Config.nMabMabeHitMinLvLimit = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "MabMabeHitSucessRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "MabMabeHitSucessRate", M2Share.g_Config.nMabMabeHitSucessRate);
            else
                M2Share.g_Config.nMabMabeHitSucessRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "MabMabeHitMabeTimeRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "MabMabeHitMabeTimeRate", M2Share.g_Config.nMabMabeHitMabeTimeRate);
            else
                M2Share.g_Config.nMabMabeHitMabeTimeRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "MagicAttackRage", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "MagicAttackRage", M2Share.g_Config.nMagicAttackRage);
            else
                M2Share.g_Config.nMagicAttackRage = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "DropItemRage", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "DropItemRage", M2Share.g_Config.nDropItemRage);
            else
                M2Share.g_Config.nDropItemRage = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "AmyOunsulPoint", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "AmyOunsulPoint", M2Share.g_Config.nAmyOunsulPoint);
            else
                M2Share.g_Config.nAmyOunsulPoint = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "DisableInSafeZoneFireCross", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "DisableInSafeZoneFireCross", M2Share.g_Config.boDisableInSafeZoneFireCross);
            else
                M2Share.g_Config.boDisableInSafeZoneFireCross = nLoadInteger == 1;
            nLoadInteger = Config.ReadInteger("Setup", "GroupMbAttackPlayObject", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "GroupMbAttackPlayObject", M2Share.g_Config.boGroupMbAttackPlayObject);
            else
                M2Share.g_Config.boGroupMbAttackPlayObject = nLoadInteger == 1;
            nLoadInteger = Config.ReadInteger("Setup", "PosionDecHealthTime", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "PosionDecHealthTime", M2Share.g_Config.dwPosionDecHealthTime);
            else
                M2Share.g_Config.dwPosionDecHealthTime = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "PosionDamagarmor", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "PosionDamagarmor", M2Share.g_Config.nPosionDamagarmor);
            else
                M2Share.g_Config.nPosionDamagarmor = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "LimitSwordLong", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "LimitSwordLong", M2Share.g_Config.boLimitSwordLong);
            else
                M2Share.g_Config.boLimitSwordLong = !(nLoadInteger == 0);
            nLoadInteger = Config.ReadInteger("Setup", "SwordLongPowerRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "SwordLongPowerRate", M2Share.g_Config.nSwordLongPowerRate);
            else
                M2Share.g_Config.nSwordLongPowerRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "FireBoomRage", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "FireBoomRage", M2Share.g_Config.nFireBoomRage);
            else
                M2Share.g_Config.nFireBoomRage = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "SnowWindRange", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "SnowWindRange", M2Share.g_Config.nSnowWindRange);
            else
                M2Share.g_Config.nSnowWindRange = nLoadInteger;
            if (Config.ReadInteger("Setup", "ElecBlizzardRange", -1) < 0)
                Config.WriteInteger("Setup", "ElecBlizzardRange", M2Share.g_Config.nElecBlizzardRange);
            M2Share.g_Config.nElecBlizzardRange =
                Config.ReadInteger("Setup", "ElecBlizzardRange", M2Share.g_Config.nElecBlizzardRange);
            if (Config.ReadInteger("Setup", "HumanLevelDiffer", -1) < 0)
                Config.WriteInteger("Setup", "HumanLevelDiffer", M2Share.g_Config.nHumanLevelDiffer);
            M2Share.g_Config.nHumanLevelDiffer =
                Config.ReadInteger("Setup", "HumanLevelDiffer", M2Share.g_Config.nHumanLevelDiffer);
            if (Config.ReadInteger("Setup", "KillHumanWinLevel", -1) < 0)
                Config.WriteBool("Setup", "KillHumanWinLevel", M2Share.g_Config.boKillHumanWinLevel);
            M2Share.g_Config.boKillHumanWinLevel =
                Config.ReadBool("Setup", "KillHumanWinLevel", M2Share.g_Config.boKillHumanWinLevel);
            if (Config.ReadInteger("Setup", "KilledLostLevel", -1) < 0)
                Config.WriteBool("Setup", "KilledLostLevel", M2Share.g_Config.boKilledLostLevel);
            M2Share.g_Config.boKilledLostLevel =
                Config.ReadBool("Setup", "KilledLostLevel", M2Share.g_Config.boKilledLostLevel);
            if (Config.ReadInteger("Setup", "KillHumanWinLevelPoint", -1) < 0)
                Config.WriteInteger("Setup", "KillHumanWinLevelPoint", M2Share.g_Config.nKillHumanWinLevel);
            M2Share.g_Config.nKillHumanWinLevel = Config.ReadInteger("Setup", "KillHumanWinLevelPoint",
                M2Share.g_Config.nKillHumanWinLevel);
            if (Config.ReadInteger("Setup", "KilledLostLevelPoint", -1) < 0)
                Config.WriteInteger("Setup", "KilledLostLevelPoint", M2Share.g_Config.nKilledLostLevel);
            M2Share.g_Config.nKilledLostLevel =
                Config.ReadInteger("Setup", "KilledLostLevelPoint", M2Share.g_Config.nKilledLostLevel);
            if (Config.ReadInteger("Setup", "KillHumanWinExp", -1) < 0)
                Config.WriteBool("Setup", "KillHumanWinExp", M2Share.g_Config.boKillHumanWinExp);
            M2Share.g_Config.boKillHumanWinExp =
                Config.ReadBool("Setup", "KillHumanWinExp", M2Share.g_Config.boKillHumanWinExp);
            if (Config.ReadInteger("Setup", "KilledLostExp", -1) < 0)
                Config.WriteBool("Setup", "KilledLostExp", M2Share.g_Config.boKilledLostExp);
            M2Share.g_Config.boKilledLostExp =
                Config.ReadBool("Setup", "KilledLostExp", M2Share.g_Config.boKilledLostExp);
            if (Config.ReadInteger("Setup", "KillHumanWinExpPoint", -1) < 0)
                Config.WriteInteger("Setup", "KillHumanWinExpPoint", M2Share.g_Config.nKillHumanWinExp);
            M2Share.g_Config.nKillHumanWinExp =
                Config.ReadInteger("Setup", "KillHumanWinExpPoint", M2Share.g_Config.nKillHumanWinExp);
            if (Config.ReadInteger("Setup", "KillHumanLostExpPoint", -1) < 0)
                Config.WriteInteger("Setup", "KillHumanLostExpPoint", M2Share.g_Config.nKillHumanLostExp);
            M2Share.g_Config.nKillHumanLostExp =
                Config.ReadInteger("Setup", "KillHumanLostExpPoint", M2Share.g_Config.nKillHumanLostExp);
            if (Config.ReadInteger("Setup", "MonsterPowerRate", -1) < 0)
                Config.WriteInteger("Setup", "MonsterPowerRate", M2Share.g_Config.nMonsterPowerRate);
            M2Share.g_Config.nMonsterPowerRate =
                Config.ReadInteger("Setup", "MonsterPowerRate", M2Share.g_Config.nMonsterPowerRate);
            if (Config.ReadInteger("Setup", "ItemsPowerRate", -1) < 0)
                Config.WriteInteger("Setup", "ItemsPowerRate", M2Share.g_Config.nItemsPowerRate);
            M2Share.g_Config.nItemsPowerRate =
                Config.ReadInteger("Setup", "ItemsPowerRate", M2Share.g_Config.nItemsPowerRate);
            if (Config.ReadInteger("Setup", "ItemsACPowerRate", -1) < 0)
                Config.WriteInteger("Setup", "ItemsACPowerRate", M2Share.g_Config.nItemsACPowerRate);
            M2Share.g_Config.nItemsACPowerRate =
                Config.ReadInteger("Setup", "ItemsACPowerRate", M2Share.g_Config.nItemsACPowerRate);
            if (Config.ReadInteger("Setup", "SendOnlineCount", -1) < 0)
                Config.WriteBool("Setup", "SendOnlineCount", M2Share.g_Config.boSendOnlineCount);
            M2Share.g_Config.boSendOnlineCount =
                Config.ReadBool("Setup", "SendOnlineCount", M2Share.g_Config.boSendOnlineCount);
            if (Config.ReadInteger("Setup", "SendOnlineCountRate", -1) < 0)
                Config.WriteInteger("Setup", "SendOnlineCountRate", M2Share.g_Config.nSendOnlineCountRate);
            M2Share.g_Config.nSendOnlineCountRate =
                Config.ReadInteger("Setup", "SendOnlineCountRate", M2Share.g_Config.nSendOnlineCountRate);
            if (Config.ReadInteger("Setup", "SendOnlineTime", -1) < 0)
                Config.WriteInteger("Setup", "SendOnlineTime", M2Share.g_Config.dwSendOnlineTime);
            M2Share.g_Config.dwSendOnlineTime =
                Config.ReadInteger("Setup", "SendOnlineTime", M2Share.g_Config.dwSendOnlineTime);
            if (Config.ReadInteger("Setup", "SaveHumanRcdTime", -1) < 0)
                Config.WriteInteger("Setup", "SaveHumanRcdTime", M2Share.g_Config.dwSaveHumanRcdTime);
            M2Share.g_Config.dwSaveHumanRcdTime =
                Config.ReadInteger("Setup", "SaveHumanRcdTime", M2Share.g_Config.dwSaveHumanRcdTime);
            if (Config.ReadInteger("Setup", "HumanFreeDelayTime", -1) < 0)
                Config.WriteInteger("Setup", "HumanFreeDelayTime", M2Share.g_Config.dwHumanFreeDelayTime);
            if (Config.ReadInteger("Setup", "MakeGhostTime", -1) < 0)
                Config.WriteInteger("Setup", "MakeGhostTime", M2Share.g_Config.dwMakeGhostTime);
            M2Share.g_Config.dwMakeGhostTime =
                Config.ReadInteger("Setup", "MakeGhostTime", M2Share.g_Config.dwMakeGhostTime);
            if (Config.ReadInteger("Setup", "ClearDropOnFloorItemTime", -1) < 0)
                Config.WriteInteger("Setup", "ClearDropOnFloorItemTime", M2Share.g_Config.dwClearDropOnFloorItemTime);
            M2Share.g_Config.dwClearDropOnFloorItemTime = Config.ReadInteger("Setup", "ClearDropOnFloorItemTime",
                M2Share.g_Config.dwClearDropOnFloorItemTime);
            if (Config.ReadInteger("Setup", "FloorItemCanPickUpTime", -1) < 0)
                Config.WriteInteger("Setup", "FloorItemCanPickUpTime", M2Share.g_Config.dwFloorItemCanPickUpTime);
            M2Share.g_Config.dwFloorItemCanPickUpTime = Config.ReadInteger("Setup", "FloorItemCanPickUpTime",
                M2Share.g_Config.dwFloorItemCanPickUpTime);
            if (Config.ReadInteger("Setup", "PasswordLockSystem", -1) < 0)
                Config.WriteBool("Setup", "PasswordLockSystem", M2Share.g_Config.boPasswordLockSystem);
            M2Share.g_Config.boPasswordLockSystem =
                Config.ReadBool("Setup", "PasswordLockSystem", M2Share.g_Config.boPasswordLockSystem);
            if (Config.ReadInteger("Setup", "PasswordLockDealAction", -1) < 0)
                Config.WriteBool("Setup", "PasswordLockDealAction", M2Share.g_Config.boLockDealAction);
            M2Share.g_Config.boLockDealAction =
                Config.ReadBool("Setup", "PasswordLockDealAction", M2Share.g_Config.boLockDealAction);
            if (Config.ReadInteger("Setup", "PasswordLockDropAction", -1) < 0)
                Config.WriteBool("Setup", "PasswordLockDropAction", M2Share.g_Config.boLockDropAction);
            M2Share.g_Config.boLockDropAction =
                Config.ReadBool("Setup", "PasswordLockDropAction", M2Share.g_Config.boLockDropAction);
            if (Config.ReadInteger("Setup", "PasswordLockGetBackItemAction", -1) < 0)
                Config.WriteBool("Setup", "PasswordLockGetBackItemAction", M2Share.g_Config.boLockGetBackItemAction);
            M2Share.g_Config.boLockGetBackItemAction = Config.ReadBool("Setup", "PasswordLockGetBackItemAction",
                M2Share.g_Config.boLockGetBackItemAction);
            if (Config.ReadInteger("Setup", "PasswordLockHumanLogin", -1) < 0)
                Config.WriteBool("Setup", "PasswordLockHumanLogin", M2Share.g_Config.boLockHumanLogin);
            M2Share.g_Config.boLockHumanLogin =
                Config.ReadBool("Setup", "PasswordLockHumanLogin", M2Share.g_Config.boLockHumanLogin);
            if (Config.ReadInteger("Setup", "PasswordLockWalkAction", -1) < 0)
                Config.WriteBool("Setup", "PasswordLockWalkAction", M2Share.g_Config.boLockWalkAction);
            M2Share.g_Config.boLockWalkAction =
                Config.ReadBool("Setup", "PasswordLockWalkAction", M2Share.g_Config.boLockWalkAction);
            if (Config.ReadInteger("Setup", "PasswordLockRunAction", -1) < 0)
                Config.WriteBool("Setup", "PasswordLockRunAction", M2Share.g_Config.boLockRunAction);
            M2Share.g_Config.boLockRunAction =
                Config.ReadBool("Setup", "PasswordLockRunAction", M2Share.g_Config.boLockRunAction);
            if (Config.ReadInteger("Setup", "PasswordLockHitAction", -1) < 0)
                Config.WriteBool("Setup", "PasswordLockHitAction", M2Share.g_Config.boLockHitAction);
            M2Share.g_Config.boLockHitAction =
                Config.ReadBool("Setup", "PasswordLockHitAction", M2Share.g_Config.boLockHitAction);
            if (Config.ReadInteger("Setup", "PasswordLockSpellAction", -1) < 0)
                Config.WriteBool("Setup", "PasswordLockSpellAction", M2Share.g_Config.boLockSpellAction);
            M2Share.g_Config.boLockSpellAction =
                Config.ReadBool("Setup", "PasswordLockSpellAction", M2Share.g_Config.boLockSpellAction);
            if (Config.ReadInteger("Setup", "PasswordLockSendMsgAction", -1) < 0)
                Config.WriteBool("Setup", "PasswordLockSendMsgAction", M2Share.g_Config.boLockSendMsgAction);
            M2Share.g_Config.boLockSendMsgAction = Config.ReadBool("Setup", "PasswordLockSendMsgAction",
                M2Share.g_Config.boLockSendMsgAction);
            if (Config.ReadInteger("Setup", "PasswordLockUserItemAction", -1) < 0)
                Config.WriteBool("Setup", "PasswordLockUserItemAction", M2Share.g_Config.boLockUserItemAction);
            M2Share.g_Config.boLockUserItemAction = Config.ReadBool("Setup", "PasswordLockUserItemAction",
                M2Share.g_Config.boLockUserItemAction);
            if (Config.ReadInteger("Setup", "PasswordLockInObModeAction", -1) < 0)
                Config.WriteBool("Setup", "PasswordLockInObModeAction", M2Share.g_Config.boLockInObModeAction);
            M2Share.g_Config.boLockInObModeAction = Config.ReadBool("Setup", "PasswordLockInObModeAction",
                M2Share.g_Config.boLockInObModeAction);
            if (Config.ReadInteger("Setup", "PasswordErrorKick", -1) < 0)
                Config.WriteBool("Setup", "PasswordErrorKick", M2Share.g_Config.boPasswordErrorKick);
            M2Share.g_Config.boPasswordErrorKick =
                Config.ReadBool("Setup", "PasswordErrorKick", M2Share.g_Config.boPasswordErrorKick);
            if (Config.ReadInteger("Setup", "PasswordErrorCountLock", -1) < 0)
                Config.WriteInteger("Setup", "PasswordErrorCountLock", M2Share.g_Config.nPasswordErrorCountLock);
            M2Share.g_Config.nPasswordErrorCountLock = Config.ReadInteger("Setup", "PasswordErrorCountLock",
                M2Share.g_Config.nPasswordErrorCountLock);
            if (Config.ReadInteger("Setup", "SoftVersionDate", -1) < 0)
                Config.WriteInteger("Setup", "SoftVersionDate", M2Share.g_Config.nSoftVersionDate);
            M2Share.g_Config.nSoftVersionDate =
                Config.ReadInteger("Setup", "SoftVersionDate", M2Share.g_Config.nSoftVersionDate);
            nLoadInteger = Config.ReadInteger("Setup", "CanOldClientLogon", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "CanOldClientLogon", M2Share.g_Config.boCanOldClientLogon);
            else
                M2Share.g_Config.boCanOldClientLogon = nLoadInteger == 1;
            if (Config.ReadInteger("Setup", "ConsoleShowUserCountTime", -1) < 0)
                Config.WriteInteger("Setup", "ConsoleShowUserCountTime", M2Share.g_Config.dwConsoleShowUserCountTime);
            M2Share.g_Config.dwConsoleShowUserCountTime = Config.ReadInteger("Setup", "ConsoleShowUserCountTime",
                M2Share.g_Config.dwConsoleShowUserCountTime);
            if (Config.ReadInteger("Setup", "ShowLineNoticeTime", -1) < 0)
                Config.WriteInteger("Setup", "ShowLineNoticeTime", M2Share.g_Config.dwShowLineNoticeTime);
            M2Share.g_Config.dwShowLineNoticeTime =
                Config.ReadInteger("Setup", "ShowLineNoticeTime", M2Share.g_Config.dwShowLineNoticeTime);
            if (Config.ReadInteger("Setup", "LineNoticeColor", -1) < 0)
                Config.WriteInteger("Setup", "LineNoticeColor", M2Share.g_Config.nLineNoticeColor);
            M2Share.g_Config.nLineNoticeColor =
                Config.ReadInteger("Setup", "LineNoticeColor", M2Share.g_Config.nLineNoticeColor);
            if (Config.ReadInteger("Setup", "ItemSpeedTime", -1) < 0)
                Config.WriteInteger("Setup", "ItemSpeedTime", M2Share.g_Config.ClientConf.btItemSpeed);
            M2Share.g_Config.ClientConf.btItemSpeed =
                Config.ReadInteger<byte>("Setup", "ItemSpeedTime", M2Share.g_Config.ClientConf.btItemSpeed);
            if (Config.ReadInteger("Setup", "MaxHitMsgCount", -1) < 0)
                Config.WriteInteger("Setup", "MaxHitMsgCount", M2Share.g_Config.nMaxHitMsgCount);
            M2Share.g_Config.nMaxHitMsgCount =
                Config.ReadInteger("Setup", "MaxHitMsgCount", M2Share.g_Config.nMaxHitMsgCount);
            if (Config.ReadInteger("Setup", "MaxSpellMsgCount", -1) < 0)
                Config.WriteInteger("Setup", "MaxSpellMsgCount", M2Share.g_Config.nMaxSpellMsgCount);
            M2Share.g_Config.nMaxSpellMsgCount =
                Config.ReadInteger("Setup", "MaxSpellMsgCount", M2Share.g_Config.nMaxSpellMsgCount);
            if (Config.ReadInteger("Setup", "MaxRunMsgCount", -1) < 0)
                Config.WriteInteger("Setup", "MaxRunMsgCount", M2Share.g_Config.nMaxRunMsgCount);
            M2Share.g_Config.nMaxRunMsgCount =
                Config.ReadInteger("Setup", "MaxRunMsgCount", M2Share.g_Config.nMaxRunMsgCount);
            if (Config.ReadInteger("Setup", "MaxWalkMsgCount", -1) < 0)
                Config.WriteInteger("Setup", "MaxWalkMsgCount", M2Share.g_Config.nMaxWalkMsgCount);
            M2Share.g_Config.nMaxWalkMsgCount =
                Config.ReadInteger("Setup", "MaxWalkMsgCount", M2Share.g_Config.nMaxWalkMsgCount);
            if (Config.ReadInteger("Setup", "MaxTurnMsgCount", -1) < 0)
                Config.WriteInteger("Setup", "MaxTurnMsgCount", M2Share.g_Config.nMaxTurnMsgCount);
            M2Share.g_Config.nMaxTurnMsgCount =
                Config.ReadInteger("Setup", "MaxTurnMsgCount", M2Share.g_Config.nMaxTurnMsgCount);
            if (Config.ReadInteger("Setup", "MaxSitDonwMsgCount", -1) < 0)
                Config.WriteInteger("Setup", "MaxSitDonwMsgCount", M2Share.g_Config.nMaxSitDonwMsgCount);
            M2Share.g_Config.nMaxSitDonwMsgCount =
                Config.ReadInteger("Setup", "MaxSitDonwMsgCount", M2Share.g_Config.nMaxSitDonwMsgCount);
            if (Config.ReadInteger("Setup", "MaxDigUpMsgCount", -1) < 0)
                Config.WriteInteger("Setup", "MaxDigUpMsgCount", M2Share.g_Config.nMaxDigUpMsgCount);
            M2Share.g_Config.nMaxDigUpMsgCount =
                Config.ReadInteger("Setup", "MaxDigUpMsgCount", M2Share.g_Config.nMaxDigUpMsgCount);
            if (Config.ReadInteger("Setup", "SpellSendUpdateMsg", -1) < 0)
                Config.WriteBool("Setup", "SpellSendUpdateMsg", M2Share.g_Config.boSpellSendUpdateMsg);
            M2Share.g_Config.boSpellSendUpdateMsg =
                Config.ReadBool("Setup", "SpellSendUpdateMsg", M2Share.g_Config.boSpellSendUpdateMsg);
            if (Config.ReadInteger("Setup", "ActionSendActionMsg", -1) < 0)
                Config.WriteBool("Setup", "ActionSendActionMsg", M2Share.g_Config.boActionSendActionMsg);
            M2Share.g_Config.boActionSendActionMsg = Config.ReadBool("Setup", "ActionSendActionMsg",
                M2Share.g_Config.boActionSendActionMsg);
            if (Config.ReadInteger("Setup", "OverSpeedKickCount", -1) < 0)
                Config.WriteInteger("Setup", "OverSpeedKickCount", M2Share.g_Config.nOverSpeedKickCount);
            M2Share.g_Config.nOverSpeedKickCount =
                Config.ReadInteger("Setup", "OverSpeedKickCount", M2Share.g_Config.nOverSpeedKickCount);
            if (Config.ReadInteger("Setup", "DropOverSpeed", -1) < 0)
                Config.WriteInteger("Setup", "DropOverSpeed", M2Share.g_Config.dwDropOverSpeed);
            M2Share.g_Config.dwDropOverSpeed =
                Config.ReadInteger("Setup", "DropOverSpeed", M2Share.g_Config.dwDropOverSpeed);
            if (Config.ReadInteger("Setup", "KickOverSpeed", -1) < 0)
                Config.WriteBool("Setup", "KickOverSpeed", M2Share.g_Config.boKickOverSpeed);
            M2Share.g_Config.boKickOverSpeed =
                Config.ReadBool("Setup", "KickOverSpeed", M2Share.g_Config.boKickOverSpeed);
            nLoadInteger = Config.ReadInteger("Setup", "SpeedControlMode", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "SpeedControlMode", M2Share.g_Config.btSpeedControlMode);
            else
                M2Share.g_Config.btSpeedControlMode = nLoadInteger;
            if (Config.ReadInteger("Setup", "HitIntervalTime", -1) < 0)
                Config.WriteInteger("Setup", "HitIntervalTime", M2Share.g_Config.dwHitIntervalTime);
            M2Share.g_Config.dwHitIntervalTime =
                Config.ReadInteger("Setup", "HitIntervalTime", M2Share.g_Config.dwHitIntervalTime);
            if (Config.ReadInteger("Setup", "MagicHitIntervalTime", -1) < 0)
                Config.WriteInteger("Setup", "MagicHitIntervalTime", M2Share.g_Config.dwMagicHitIntervalTime);
            M2Share.g_Config.dwMagicHitIntervalTime = Config.ReadInteger("Setup", "MagicHitIntervalTime",
                M2Share.g_Config.dwMagicHitIntervalTime);
            if (Config.ReadInteger("Setup", "RunIntervalTime", -1) < 0)
                Config.WriteInteger("Setup", "RunIntervalTime", M2Share.g_Config.dwRunIntervalTime);
            M2Share.g_Config.dwRunIntervalTime =
                Config.ReadInteger("Setup", "RunIntervalTime", M2Share.g_Config.dwRunIntervalTime);
            if (Config.ReadInteger("Setup", "WalkIntervalTime", -1) < 0)
                Config.WriteInteger("Setup", "WalkIntervalTime", M2Share.g_Config.dwWalkIntervalTime);
            M2Share.g_Config.dwWalkIntervalTime =
                Config.ReadInteger("Setup", "WalkIntervalTime", M2Share.g_Config.dwWalkIntervalTime);
            if (Config.ReadInteger("Setup", "TurnIntervalTime", -1) < 0)
                Config.WriteInteger("Setup", "TurnIntervalTime", M2Share.g_Config.dwTurnIntervalTime);
            M2Share.g_Config.dwTurnIntervalTime =
                Config.ReadInteger("Setup", "TurnIntervalTime", M2Share.g_Config.dwTurnIntervalTime);
            nLoadInteger = Config.ReadInteger("Setup", "ControlActionInterval", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "ControlActionInterval", M2Share.g_Config.boControlActionInterval);
            else
                M2Share.g_Config.boControlActionInterval = nLoadInteger == 1;
            nLoadInteger = Config.ReadInteger("Setup", "ControlWalkHit", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "ControlWalkHit", M2Share.g_Config.boControlWalkHit);
            else
                M2Share.g_Config.boControlWalkHit = nLoadInteger == 1;
            nLoadInteger = Config.ReadInteger("Setup", "ControlRunLongHit", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "ControlRunLongHit", M2Share.g_Config.boControlRunLongHit);
            else
                M2Share.g_Config.boControlRunLongHit = nLoadInteger == 1;
            nLoadInteger = Config.ReadInteger("Setup", "ControlRunHit", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "ControlRunHit", M2Share.g_Config.boControlRunHit);
            else
                M2Share.g_Config.boControlRunHit = nLoadInteger == 1;
            nLoadInteger = Config.ReadInteger("Setup", "ControlRunMagic", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "ControlRunMagic", M2Share.g_Config.boControlRunMagic);
            else
                M2Share.g_Config.boControlRunMagic = nLoadInteger == 1;
            nLoadInteger = Config.ReadInteger("Setup", "ActionIntervalTime", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "ActionIntervalTime", M2Share.g_Config.dwActionIntervalTime);
            else
                M2Share.g_Config.dwActionIntervalTime = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "RunLongHitIntervalTime", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "RunLongHitIntervalTime", M2Share.g_Config.dwRunLongHitIntervalTime);
            else
                M2Share.g_Config.dwRunLongHitIntervalTime = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "RunHitIntervalTime", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "RunHitIntervalTime", M2Share.g_Config.dwRunHitIntervalTime);
            else
                M2Share.g_Config.dwRunHitIntervalTime = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "WalkHitIntervalTime", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "WalkHitIntervalTime", M2Share.g_Config.dwWalkHitIntervalTime);
            else
                M2Share.g_Config.dwWalkHitIntervalTime = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "RunMagicIntervalTime", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "RunMagicIntervalTime", M2Share.g_Config.dwRunMagicIntervalTime);
            else
                M2Share.g_Config.dwRunMagicIntervalTime = nLoadInteger;
            if (Config.ReadInteger("Setup", "DisableStruck", -1) < 0)
                Config.WriteBool("Setup", "DisableStruck", M2Share.g_Config.boDisableStruck);
            M2Share.g_Config.boDisableStruck =
                Config.ReadBool("Setup", "DisableStruck", M2Share.g_Config.boDisableStruck);
            if (Config.ReadInteger("Setup", "DisableSelfStruck", -1) < 0)
                Config.WriteBool("Setup", "DisableSelfStruck", M2Share.g_Config.boDisableSelfStruck);
            M2Share.g_Config.boDisableSelfStruck =
                Config.ReadBool("Setup", "DisableSelfStruck", M2Share.g_Config.boDisableSelfStruck);
            if (Config.ReadInteger("Setup", "StruckTime", -1) < 0)
                Config.WriteInteger("Setup", "StruckTime", M2Share.g_Config.dwStruckTime);
            M2Share.g_Config.dwStruckTime = Config.ReadInteger("Setup", "StruckTime", M2Share.g_Config.dwStruckTime);
            nLoadInteger = Config.ReadInteger("Setup", "AddUserItemNewValue", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "AddUserItemNewValue", M2Share.g_Config.boAddUserItemNewValue);
            else
                M2Share.g_Config.boAddUserItemNewValue = nLoadInteger == 1;
            nLoadInteger = ExpConf.ReadInteger("Exp", "LimitExpLevel", -1);
            if (nLoadInteger < 0)
                ExpConf.WriteInteger("Exp", "LimitExpLevel", M2Share.g_Config.nLimitExpLevel);
            else
                M2Share.g_Config.nLimitExpLevel = nLoadInteger;
            nLoadInteger = ExpConf.ReadInteger("Exp", "LimitExpValue", -1);
            if (nLoadInteger < 0)
                ExpConf.WriteInteger("Exp", "LimitExpValue", M2Share.g_Config.nLimitExpValue);
            else
                M2Share.g_Config.nLimitExpValue = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "TestSpeedMode", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "TestSpeedMode", M2Share.g_Config.boTestSpeedMode);
            else
                M2Share.g_Config.boTestSpeedMode = nLoadInteger == 1;
            // 气血石开始
            if (Config.ReadInteger("Setup", "HPStoneStartRate", -1) < 0)
                Config.WriteInteger("Setup", "HPStoneStartRate", M2Share.g_Config.HPStoneStartRate);
            M2Share.g_Config.HPStoneStartRate =
                Config.ReadInteger<byte>("Setup", "HPStoneStartRate", M2Share.g_Config.HPStoneStartRate);
            if (Config.ReadInteger("Setup", "MPStoneStartRate", -1) < 0)
                Config.WriteInteger("Setup", "MPStoneStartRate", M2Share.g_Config.MPStoneStartRate);
            M2Share.g_Config.MPStoneStartRate =
                Config.ReadInteger<byte>("Setup", "MPStoneStartRate", M2Share.g_Config.MPStoneStartRate);
            if (Config.ReadInteger("Setup", "HPStoneIntervalTime", -1) < 0)
                Config.WriteInteger("Setup", "HPStoneIntervalTime", M2Share.g_Config.HPStoneIntervalTime);
            M2Share.g_Config.HPStoneIntervalTime =
                Config.ReadInteger("Setup", "HPStoneIntervalTime", M2Share.g_Config.HPStoneIntervalTime);
            if (Config.ReadInteger("Setup", "MPStoneIntervalTime", -1) < 0)
                Config.WriteInteger("Setup", "MPStoneIntervalTime", M2Share.g_Config.MPStoneIntervalTime);
            M2Share.g_Config.MPStoneIntervalTime =
                Config.ReadInteger("Setup", "MPStoneIntervalTime", M2Share.g_Config.MPStoneIntervalTime);
            if (Config.ReadInteger("Setup", "HPStoneAddRate", -1) < 0)
                Config.WriteInteger("Setup", "HPStoneAddRate", M2Share.g_Config.HPStoneAddRate);
            M2Share.g_Config.HPStoneAddRate =
                Config.ReadInteger<byte>("Setup", "HPStoneAddRate", M2Share.g_Config.HPStoneAddRate);
            if (Config.ReadInteger("Setup", "MPStoneAddRate", -1) < 0)
                Config.WriteInteger("Setup", "MPStoneAddRate", M2Share.g_Config.MPStoneAddRate);
            M2Share.g_Config.MPStoneAddRate =
                Config.ReadInteger<byte>("Setup", "MPStoneAddRate", M2Share.g_Config.MPStoneAddRate);
            if (Config.ReadInteger("Setup", "HPStoneDecDura", -1) < 0)
                Config.WriteInteger("Setup", "HPStoneDecDura", M2Share.g_Config.HPStoneDecDura);
            M2Share.g_Config.HPStoneDecDura =
                Config.ReadInteger("Setup", "HPStoneDecDura", M2Share.g_Config.HPStoneDecDura);
            if (Config.ReadInteger("Setup", "MPStoneDecDura", -1) < 0)
                Config.WriteInteger("Setup", "MPStoneDecDura", M2Share.g_Config.MPStoneDecDura);
            M2Share.g_Config.MPStoneDecDura =
                Config.ReadInteger("Setup", "MPStoneDecDura", M2Share.g_Config.MPStoneDecDura);
            // 气血石结束
            nLoadInteger = Config.ReadInteger("Setup", "WeaponMakeUnLuckRate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "WeaponMakeUnLuckRate", M2Share.g_Config.nWeaponMakeUnLuckRate);
            else
                M2Share.g_Config.nWeaponMakeUnLuckRate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "WeaponMakeLuckPoint1", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "WeaponMakeLuckPoint1", M2Share.g_Config.nWeaponMakeLuckPoint1);
            else
                M2Share.g_Config.nWeaponMakeLuckPoint1 = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "WeaponMakeLuckPoint2", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "WeaponMakeLuckPoint2", M2Share.g_Config.nWeaponMakeLuckPoint2);
            else
                M2Share.g_Config.nWeaponMakeLuckPoint2 = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "WeaponMakeLuckPoint3", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "WeaponMakeLuckPoint3", M2Share.g_Config.nWeaponMakeLuckPoint3);
            else
                M2Share.g_Config.nWeaponMakeLuckPoint3 = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "WeaponMakeLuckPoint2Rate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "WeaponMakeLuckPoint2Rate", M2Share.g_Config.nWeaponMakeLuckPoint2Rate);
            else
                M2Share.g_Config.nWeaponMakeLuckPoint2Rate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "WeaponMakeLuckPoint3Rate", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "WeaponMakeLuckPoint3Rate", M2Share.g_Config.nWeaponMakeLuckPoint3Rate);
            else
                M2Share.g_Config.nWeaponMakeLuckPoint3Rate = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "CheckUserItemPlace", -1);
            if (nLoadInteger < 0)
                Config.WriteBool("Setup", "CheckUserItemPlace", M2Share.g_Config.boCheckUserItemPlace);
            else
                M2Share.g_Config.boCheckUserItemPlace = nLoadInteger == 1;
            nLoadInteger = Config.ReadInteger("Setup", "LevelValueOfTaosHP", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "LevelValueOfTaosHP", M2Share.g_Config.nLevelValueOfTaosHP);
            else
                M2Share.g_Config.nLevelValueOfTaosHP = nLoadInteger;
            var nLoadFloatRate = Config.ReadInteger<double>("Setup", "LevelValueOfTaosHPRate", 0);
            if (nLoadFloatRate == 0)
                Config.WriteInteger("Setup", "LevelValueOfTaosHPRate", M2Share.g_Config.nLevelValueOfTaosHPRate);
            else
                M2Share.g_Config.nLevelValueOfTaosHPRate = nLoadFloatRate;
            nLoadInteger = Config.ReadInteger("Setup", "LevelValueOfTaosMP", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "LevelValueOfTaosMP", M2Share.g_Config.nLevelValueOfTaosMP);
            else
                M2Share.g_Config.nLevelValueOfTaosMP = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "LevelValueOfWizardHP", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "LevelValueOfWizardHP", M2Share.g_Config.nLevelValueOfWizardHP);
            else
                M2Share.g_Config.nLevelValueOfWizardHP = nLoadInteger;
            nLoadFloatRate = Config.ReadInteger<double>("Setup", "LevelValueOfWizardHPRate", 0);
            if (nLoadFloatRate == 0)
                Config.WriteInteger("Setup", "LevelValueOfWizardHPRate", M2Share.g_Config.nLevelValueOfWizardHPRate);
            else
                M2Share.g_Config.nLevelValueOfWizardHPRate = nLoadFloatRate;
            nLoadInteger = Config.ReadInteger("Setup", "LevelValueOfWarrHP", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "LevelValueOfWarrHP", M2Share.g_Config.nLevelValueOfWarrHP);
            else
                M2Share.g_Config.nLevelValueOfWarrHP = nLoadInteger;
            nLoadFloatRate = Config.ReadInteger<double>("Setup", "LevelValueOfWarrHPRate", 0);
            if (nLoadFloatRate == 0)
                Config.WriteInteger("Setup", "LevelValueOfWarrHPRate", M2Share.g_Config.nLevelValueOfWarrHPRate);
            else
                M2Share.g_Config.nLevelValueOfWarrHPRate = nLoadFloatRate;
            nLoadInteger = Config.ReadInteger("Setup", "ProcessMonsterInterval", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "ProcessMonsterInterval", M2Share.g_Config.nProcessMonsterInterval);
            else
                M2Share.g_Config.nProcessMonsterInterval = nLoadInteger;
            if (Config.ReadInteger("Setup", "StartCastleWarDays", -1) < 0)
                Config.WriteInteger("Setup", "StartCastleWarDays", M2Share.g_Config.nStartCastleWarDays);
            M2Share.g_Config.nStartCastleWarDays =
                Config.ReadInteger("Setup", "StartCastleWarDays", M2Share.g_Config.nStartCastleWarDays);
            if (Config.ReadInteger("Setup", "StartCastlewarTime", -1) < 0)
                Config.WriteInteger("Setup", "StartCastlewarTime", M2Share.g_Config.nStartCastlewarTime);
            M2Share.g_Config.nStartCastlewarTime =
                Config.ReadInteger("Setup", "StartCastlewarTime", M2Share.g_Config.nStartCastlewarTime);
            if (Config.ReadInteger("Setup", "ShowCastleWarEndMsgTime", -1) < 0)
                Config.WriteInteger("Setup", "ShowCastleWarEndMsgTime", M2Share.g_Config.dwShowCastleWarEndMsgTime);
            M2Share.g_Config.dwShowCastleWarEndMsgTime = Config.ReadInteger("Setup", "ShowCastleWarEndMsgTime",
                M2Share.g_Config.dwShowCastleWarEndMsgTime);
            if (Config.ReadInteger("Server", "ClickNPCTime", -1) < 0)
                Config.WriteInteger("Server", "ClickNPCTime", M2Share.g_Config.dwclickNpcTime);
            M2Share.g_Config.dwclickNpcTime =
                Config.ReadInteger("Server", "ClickNPCTime", M2Share.g_Config.dwclickNpcTime);
            if (Config.ReadInteger("Setup", "CastleWarTime", -1) < 0)
                Config.WriteInteger("Setup", "CastleWarTime", M2Share.g_Config.dwCastleWarTime);
            M2Share.g_Config.dwCastleWarTime =
                Config.ReadInteger("Setup", "CastleWarTime", M2Share.g_Config.dwCastleWarTime);
            nLoadInteger = Config.ReadInteger("Setup", "GetCastleTime", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "GetCastleTime", M2Share.g_Config.dwGetCastleTime);
            else
                M2Share.g_Config.dwGetCastleTime = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "GuildWarTime", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "GuildWarTime", M2Share.g_Config.dwGuildWarTime);
            else
                M2Share.g_Config.dwGuildWarTime = nLoadInteger;
            for (var i = M2Share.g_Config.GlobalVal.GetLowerBound(0); i <= M2Share.g_Config.GlobalVal.GetUpperBound(0); i++)
            {
                nLoadInteger = Config.ReadInteger("Setup", "GlobalVal" + i, -1);
                if (nLoadInteger < 0)
                    Config.WriteInteger("Setup", "GlobalVal" + i, M2Share.g_Config.GlobalVal[i]);
                else
                    M2Share.g_Config.GlobalVal[i] = nLoadInteger;
            }
            nLoadInteger = Config.ReadInteger("Setup", "WinLotteryCount", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "WinLotteryCount", M2Share.g_Config.nWinLotteryCount);
            else
                M2Share.g_Config.nWinLotteryCount = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "NoWinLotteryCount", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "NoWinLotteryCount", M2Share.g_Config.nNoWinLotteryCount);
            else
                M2Share.g_Config.nNoWinLotteryCount = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "WinLotteryLevel1", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "WinLotteryLevel1", M2Share.g_Config.nWinLotteryLevel1);
            else
                M2Share.g_Config.nWinLotteryLevel1 = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "WinLotteryLevel2", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "WinLotteryLevel2", M2Share.g_Config.nWinLotteryLevel2);
            else
                M2Share.g_Config.nWinLotteryLevel2 = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "WinLotteryLevel3", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "WinLotteryLevel3", M2Share.g_Config.nWinLotteryLevel3);
            else
                M2Share.g_Config.nWinLotteryLevel3 = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "WinLotteryLevel4", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "WinLotteryLevel4", M2Share.g_Config.nWinLotteryLevel4);
            else
                M2Share.g_Config.nWinLotteryLevel4 = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "WinLotteryLevel5", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "WinLotteryLevel5", M2Share.g_Config.nWinLotteryLevel5);
            else
                M2Share.g_Config.nWinLotteryLevel5 = nLoadInteger;
            nLoadInteger = Config.ReadInteger("Setup", "WinLotteryLevel6", -1);
            if (nLoadInteger < 0)
                Config.WriteInteger("Setup", "WinLotteryLevel6", M2Share.g_Config.nWinLotteryLevel6);
            else
                M2Share.g_Config.nWinLotteryLevel6 = nLoadInteger;
        }

        public void SaveVariable()
        {
            Config.WriteInteger("Setup", "ItemNumber", M2Share.g_Config.nItemNumber);
            Config.WriteInteger("Setup", "ItemNumberEx", M2Share.g_Config.nItemNumberEx);
            for (var i = M2Share.g_Config.GlobalVal.GetLowerBound(0); i <= M2Share.g_Config.GlobalVal.GetUpperBound(0); i++)
            {
                Config.WriteInteger("Setup", "GlobalVal" + i, M2Share.g_Config.GlobalVal[i]);
            }
            Config.WriteInteger("Setup", "WinLotteryCount", M2Share.g_Config.nWinLotteryCount);
            Config.WriteInteger("Setup", "NoWinLotteryCount", M2Share.g_Config.nNoWinLotteryCount);
            Config.WriteInteger("Setup", "WinLotteryLevel1", M2Share.g_Config.nWinLotteryLevel1);
            Config.WriteInteger("Setup", "WinLotteryLevel2", M2Share.g_Config.nWinLotteryLevel2);
            Config.WriteInteger("Setup", "WinLotteryLevel3", M2Share.g_Config.nWinLotteryLevel3);
            Config.WriteInteger("Setup", "WinLotteryLevel4", M2Share.g_Config.nWinLotteryLevel4);
            Config.WriteInteger("Setup", "WinLotteryLevel5", M2Share.g_Config.nWinLotteryLevel5);
            Config.WriteInteger("Setup", "WinLotteryLevel6", M2Share.g_Config.nWinLotteryLevel6);
        }
    }
}