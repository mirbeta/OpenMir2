using SystemModule;
using SystemModule.Common;

namespace GameSvr.Configs
{
    public class ServerConfig : IniFile
    {
        public ServerConfig(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            var nLoadInteger = 0;
            var sLoadString = string.Empty;
            //数据库设置
            if (ReadString("DataBase", "ConnctionString", "") == "")
            {
                WriteString("DataBase", "ConnctionString", M2Share.g_Config.sConnctionString);
            }
            M2Share.g_Config.sConnctionString = ReadString("DataBase", "ConnctionString", M2Share.g_Config.sConnctionString);
            //数据库类型设置
            if (ReadString("DataBase", "DbType", "") == "")
            {
                WriteString("DataBase", "DbType", M2Share.g_Config.sDBType);
            }
            M2Share.g_Config.sDBType = ReadString("DataBase", "DbType", M2Share.g_Config.sDBType);
            // 服务器设置
            if (ReadInteger("Server", "ServerIndex", -1) < 0)
            {
                WriteInteger("Server", "ServerIndex", M2Share.nServerIndex);
            }
            M2Share.nServerIndex = ReadInteger("Server", "ServerIndex", M2Share.nServerIndex);
            if (ReadString("Server", "ServerName", "") == "")
            {
                WriteString("Server", "ServerName", M2Share.g_Config.sServerName);
            }
            M2Share.g_Config.sServerName = ReadString("Server", "ServerName", M2Share.g_Config.sServerName);
            if (ReadInteger("Server", "ServerNumber", -1) < 0)
                WriteInteger("Server", "ServerNumber", M2Share.g_Config.nServerNumber);
            M2Share.g_Config.nServerNumber = ReadInteger("Server", "ServerNumber", M2Share.g_Config.nServerNumber);
            if (ReadString("Server", "VentureServer", "") == "")
                WriteString("Server", "VentureServer", HUtil32.BoolToStr(M2Share.g_Config.boVentureServer));
            M2Share.g_Config.boVentureServer = String.Compare(ReadString("Server", "VentureServer", "FALSE"), "TRUE", StringComparison.Ordinal) == 0;
            if (ReadString("Server", "TestServer", "") == "")
                WriteString("Server", "TestServer", HUtil32.BoolToStr(M2Share.g_Config.boTestServer));
            M2Share.g_Config.boTestServer = String.Compare(ReadString("Server", "TestServer", "FALSE"), "TRUE", StringComparison.Ordinal) == 0;
            if (ReadInteger("Server", "TestLevel", -1) < 0)
                WriteInteger("Server", "TestLevel", M2Share.g_Config.nTestLevel);
            M2Share.g_Config.nTestLevel = ReadInteger("Server", "TestLevel", M2Share.g_Config.nTestLevel);
            if (ReadInteger("Server", "TestGold", -1) < 0)
                WriteInteger("Server", "TestGold", M2Share.g_Config.nTestGold);
            M2Share.g_Config.nTestGold = ReadInteger("Server", "TestGold", M2Share.g_Config.nTestGold);
            if (ReadInteger("Server", "TestServerUserLimit", -1) < 0)
                WriteInteger("Server", "TestServerUserLimit", M2Share.g_Config.nTestUserLimit);
            M2Share.g_Config.nTestUserLimit = ReadInteger("Server", "TestServerUserLimit", M2Share.g_Config.nTestUserLimit);
            if (ReadString("Server", "ServiceMode", "") == "")
                WriteString("Server", "ServiceMode", HUtil32.BoolToStr(M2Share.g_Config.boServiceMode));
            M2Share.g_Config.boServiceMode = ReadString("Server", "ServiceMode", "FALSE").CompareTo("TRUE") == 0;
            if (ReadString("Server", "NonPKServer", "") == "")
                WriteString("Server", "NonPKServer", HUtil32.BoolToStr(M2Share.g_Config.boNonPKServer));
            M2Share.g_Config.boNonPKServer = ReadString("Server", "NonPKServer", "FALSE").CompareTo("TRUE") == 0;
            if (ReadString("Server", "ViewHackMessage", "") == "")
                WriteString("Server", "ViewHackMessage", HUtil32.BoolToStr(M2Share.g_Config.boViewHackMessage));
            M2Share.g_Config.boViewHackMessage = ReadString("Server", "ViewHackMessage", "FALSE").CompareTo("TRUE") == 0;
            if (ReadString("Server", "ViewAdmissionFailure", "") == "")
            {
                WriteString("Server", "ViewAdmissionFailure", HUtil32.BoolToStr(M2Share.g_Config.boViewAdmissionFailure));
            }
            M2Share.g_Config.boViewAdmissionFailure = ReadString("Server", "ViewAdmissionFailure", "FALSE").CompareTo("TRUE") == 0;
            if (ReadString("Server", "GateAddr", "") == "")
            {
                WriteString("Server", "GateAddr", M2Share.g_Config.sGateAddr);
            }
            M2Share.g_Config.sGateAddr = ReadString("Server", "GateAddr", M2Share.g_Config.sGateAddr);
            if (ReadInteger("Server", "GatePort", -1) < 0)
            {
                WriteInteger("Server", "GatePort", M2Share.g_Config.nGatePort);
            }
            M2Share.g_Config.nGatePort = ReadInteger("Server", "GatePort", M2Share.g_Config.nGatePort);
            if (ReadString("Server", "DBAddr", "") == "")
                WriteString("Server", "DBAddr", M2Share.g_Config.sDBAddr);
            M2Share.g_Config.sDBAddr = ReadString("Server", "DBAddr", M2Share.g_Config.sDBAddr);
            if (ReadInteger("Server", "DBPort", -1) < 0)
                WriteInteger("Server", "DBPort", M2Share.g_Config.nDBPort);
            M2Share.g_Config.nDBPort = ReadInteger("Server", "DBPort", M2Share.g_Config.nDBPort);
            if (ReadString("Server", "IDSAddr", "") == "")
                WriteString("Server", "IDSAddr", M2Share.g_Config.sIDSAddr);
            M2Share.g_Config.sIDSAddr = ReadString("Server", "IDSAddr", M2Share.g_Config.sIDSAddr);
            if (ReadInteger("Server", "IDSPort", -1) < 0)
                WriteInteger("Server", "IDSPort", M2Share.g_Config.nIDSPort);
            M2Share.g_Config.nIDSPort = ReadInteger("Server", "IDSPort", M2Share.g_Config.nIDSPort);
            if (ReadString("Server", "MsgSrvAddr", "") == "")
                WriteString("Server", "MsgSrvAddr", M2Share.g_Config.sMsgSrvAddr);
            M2Share.g_Config.sMsgSrvAddr = ReadString("Server", "MsgSrvAddr", M2Share.g_Config.sMsgSrvAddr);
            if (ReadInteger("Server", "MsgSrvPort", -1) < 0)
                WriteInteger("Server", "MsgSrvPort", M2Share.g_Config.nMsgSrvPort);
            M2Share.g_Config.nMsgSrvPort = ReadInteger("Server", "MsgSrvPort", M2Share.g_Config.nMsgSrvPort);
            if (ReadString("Server", "LogServerAddr", "") == "")
                WriteString("Server", "LogServerAddr", M2Share.g_Config.sLogServerAddr);
            M2Share.g_Config.sLogServerAddr = ReadString("Server", "LogServerAddr", M2Share.g_Config.sLogServerAddr);
            if (ReadInteger("Server", "LogServerPort", -1) < 0)
                WriteInteger("Server", "LogServerPort", M2Share.g_Config.nLogServerPort);
            M2Share.g_Config.nLogServerPort = ReadInteger("Server", "LogServerPort", M2Share.g_Config.nLogServerPort);
            if (ReadString("Server", "DiscountForNightTime", "") == "")
                WriteString("Server", "DiscountForNightTime", HUtil32.BoolToStr(M2Share.g_Config.boDiscountForNightTime));
            M2Share.g_Config.boDiscountForNightTime = ReadString("Server", "DiscountForNightTime", "FALSE").CompareTo("TRUE".ToLower()) == 0;
            if (ReadInteger("Server", "HalfFeeStart", -1) < 0)
                WriteInteger("Server", "HalfFeeStart", M2Share.g_Config.nHalfFeeStart);
            M2Share.g_Config.nHalfFeeStart = ReadInteger("Server", "HalfFeeStart", M2Share.g_Config.nHalfFeeStart);
            if (ReadInteger("Server", "HalfFeeEnd", -1) < 0)
                WriteInteger("Server", "HalfFeeEnd", M2Share.g_Config.nHalfFeeEnd);
            M2Share.g_Config.nHalfFeeEnd = ReadInteger("Server", "HalfFeeEnd", M2Share.g_Config.nHalfFeeEnd);
            if (ReadInteger("Server", "HumLimit", -1) < 0)
                WriteInteger("Server", "HumLimit", M2Share.g_dwHumLimit);
            M2Share.g_dwHumLimit = ReadInteger("Server", "HumLimit", M2Share.g_dwHumLimit);
            if (ReadInteger("Server", "MonLimit", -1) < 0)
                WriteInteger("Server", "MonLimit", M2Share.g_dwMonLimit);
            M2Share.g_dwMonLimit = ReadInteger("Server", "MonLimit", M2Share.g_dwMonLimit);
            if (ReadInteger("Server", "ZenLimit", -1) < 0)
                WriteInteger("Server", "ZenLimit", M2Share.g_dwZenLimit);
            M2Share.g_dwZenLimit = ReadInteger("Server", "ZenLimit", M2Share.g_dwZenLimit);
            if (ReadInteger("Server", "NpcLimit", -1) < 0)
                WriteInteger("Server", "NpcLimit", M2Share.g_dwNpcLimit);
            M2Share.g_dwNpcLimit = ReadInteger("Server", "NpcLimit", M2Share.g_dwNpcLimit);
            if (ReadInteger("Server", "SocLimit", -1) < 0)
                WriteInteger("Server", "SocLimit", M2Share.g_dwSocLimit);
            M2Share.g_dwSocLimit = ReadInteger("Server", "SocLimit", M2Share.g_dwSocLimit);
            if (ReadInteger("Server", "DecLimit", -1) < 0)
                WriteInteger("Server", "DecLimit", M2Share.nDecLimit);
            M2Share.nDecLimit = ReadInteger("Server", "DecLimit", M2Share.nDecLimit);
            if (ReadInteger("Server", "SendBlock", -1) < 0)
                WriteInteger("Server", "SendBlock", M2Share.g_Config.nSendBlock);
            M2Share.g_Config.nSendBlock = ReadInteger("Server", "SendBlock", M2Share.g_Config.nSendBlock);
            if (ReadInteger("Server", "CheckBlock", -1) < 0)
                WriteInteger("Server", "CheckBlock", M2Share.g_Config.nCheckBlock);
            M2Share.g_Config.nCheckBlock = ReadInteger("Server", "CheckBlock", M2Share.g_Config.nCheckBlock);
            if (ReadInteger("Server", "SocCheckTimeOut", -1) < 0)
                WriteInteger("Server", "SocCheckTimeOut", M2Share.g_dwSocCheckTimeOut);
            M2Share.g_dwSocCheckTimeOut = ReadInteger("Server", "SocCheckTimeOut", M2Share.g_dwSocCheckTimeOut);
            if (ReadInteger("Server", "AvailableBlock", -1) < 0)
                WriteInteger("Server", "AvailableBlock", M2Share.g_Config.nAvailableBlock);
            M2Share.g_Config.nAvailableBlock = ReadInteger("Server", "AvailableBlock", M2Share.g_Config.nAvailableBlock);
            if (ReadInteger("Server", "GateLoad", -1) < 0)
                WriteInteger("Server", "GateLoad", M2Share.g_Config.nGateLoad);
            M2Share.g_Config.nGateLoad = ReadInteger("Server", "GateLoad", M2Share.g_Config.nGateLoad);
            if (ReadInteger("Server", "UserFull", -1) < 0)
                WriteInteger("Server", "UserFull", M2Share.g_Config.nUserFull);
            M2Share.g_Config.nUserFull = ReadInteger("Server", "UserFull", M2Share.g_Config.nUserFull);
            if (ReadInteger("Server", "ZenFastStep", -1) < 0)
                WriteInteger("Server", "ZenFastStep", M2Share.g_Config.nZenFastStep);
            M2Share.g_Config.nZenFastStep = ReadInteger("Server", "ZenFastStep", M2Share.g_Config.nZenFastStep);
            if (ReadInteger("Server", "ProcessMonstersTime", -1) < 0)
                WriteInteger("Server", "ProcessMonstersTime", M2Share.g_Config.dwProcessMonstersTime);
            M2Share.g_Config.dwProcessMonstersTime = ReadInteger("Server", "ProcessMonstersTime", M2Share.g_Config.dwProcessMonstersTime);
            if (ReadInteger("Server", "RegenMonstersTime", -1) < 0)
                WriteInteger("Server", "RegenMonstersTime", M2Share.g_Config.dwRegenMonstersTime);
            M2Share.g_Config.dwRegenMonstersTime = ReadInteger("Server", "RegenMonstersTime", M2Share.g_Config.dwRegenMonstersTime);
            if (ReadInteger("Server", "HumanGetMsgTimeLimit", -1) < 0)
                WriteInteger("Server", "HumanGetMsgTimeLimit", M2Share.g_Config.dwHumanGetMsgTime);
            M2Share.g_Config.dwHumanGetMsgTime = ReadInteger("Server", "HumanGetMsgTimeLimit", M2Share.g_Config.dwHumanGetMsgTime);
            if (ReadString("Share", "BaseDir", "") == "")
                WriteString("Share", "BaseDir", M2Share.g_Config.sBaseDir);
            M2Share.g_Config.sBaseDir = ReadString("Share", "BaseDir", M2Share.g_Config.sBaseDir);
            if (ReadString("Share", "GuildDir", "") == "")
                WriteString("Share", "GuildDir", M2Share.g_Config.sGuildDir);
            M2Share.g_Config.sGuildDir = ReadString("Share", "GuildDir", M2Share.g_Config.sGuildDir);
            if (ReadString("Share", "GuildFile", "") == "")
                WriteString("Share", "GuildFile", M2Share.g_Config.sGuildFile);
            M2Share.g_Config.sGuildFile = ReadString("Share", "GuildFile", M2Share.g_Config.sGuildFile);
            if (ReadString("Share", "VentureDir", "") == "")
                WriteString("Share", "VentureDir", M2Share.g_Config.sVentureDir);
            M2Share.g_Config.sVentureDir = ReadString("Share", "VentureDir", M2Share.g_Config.sVentureDir);
            if (ReadString("Share", "ConLogDir", "") == "")
                WriteString("Share", "ConLogDir", M2Share.g_Config.sConLogDir);
            M2Share.g_Config.sConLogDir = ReadString("Share", "ConLogDir", M2Share.g_Config.sConLogDir);
            if (ReadString("Share", "CastleDir", "") == "")
                WriteString("Share", "CastleDir", M2Share.g_Config.sCastleDir);
            M2Share.g_Config.sCastleDir = ReadString("Share", "CastleDir", M2Share.g_Config.sCastleDir);
            if (ReadString("Share", "CastleFile", "") == "")
                WriteString("Share", "CastleFile", M2Share.g_Config.sCastleDir + "List.txt");
            M2Share.g_Config.sCastleFile = ReadString("Share", "CastleFile", M2Share.g_Config.sCastleFile);
            if (ReadString("Share", "EnvirDir", "") == "")
            {
                WriteString("Share", "EnvirDir", M2Share.g_Config.sEnvirDir);
            }
            M2Share.g_Config.sEnvirDir = ReadString("Share", "EnvirDir", M2Share.g_Config.sEnvirDir);
            if (ReadString("Share", "MapDir", "") == "")
                WriteString("Share", "MapDir", M2Share.g_Config.sMapDir);
            M2Share.g_Config.sMapDir = ReadString("Share", "MapDir", M2Share.g_Config.sMapDir);
            if (ReadString("Share", "NoticeDir", "") == "")
            {
                WriteString("Share", "NoticeDir", M2Share.g_Config.sNoticeDir);
            }
            M2Share.g_Config.sNoticeDir = ReadString("Share", "NoticeDir", M2Share.g_Config.sNoticeDir);
            sLoadString = ReadString("Share", "LogDir", "");
            if (sLoadString == "")
            {
                WriteString("Share", "LogDir", M2Share.g_Config.sLogDir);
            }
            else
            {
                M2Share.g_Config.sLogDir = sLoadString;
            }
            // ============================================================================
            // 名称设置
            if (ReadString("Names", "HealSkill", "") == "")
                WriteString("Names", "HealSkill", M2Share.g_Config.sHealSkill);
            M2Share.g_Config.sHealSkill = ReadString("Names", "HealSkill", M2Share.g_Config.sHealSkill);
            if (ReadString("Names", "FireBallSkill", "") == "")
                WriteString("Names", "FireBallSkill", M2Share.g_Config.sFireBallSkill);
            M2Share.g_Config.sFireBallSkill = ReadString("Names", "FireBallSkill", M2Share.g_Config.sFireBallSkill);
            if (ReadString("Names", "ClothsMan", "") == "")
                WriteString("Names", "ClothsMan", M2Share.g_Config.sClothsMan);
            M2Share.g_Config.sClothsMan = ReadString("Names", "ClothsMan", M2Share.g_Config.sClothsMan);
            if (ReadString("Names", "ClothsWoman", "") == "")
                WriteString("Names", "ClothsWoman", M2Share.g_Config.sClothsWoman);
            M2Share.g_Config.sClothsWoman = ReadString("Names", "ClothsWoman", M2Share.g_Config.sClothsWoman);
            if (ReadString("Names", "WoodenSword", "") == "")
                WriteString("Names", "WoodenSword", M2Share.g_Config.sWoodenSword);
            M2Share.g_Config.sWoodenSword = ReadString("Names", "WoodenSword", M2Share.g_Config.sWoodenSword);
            if (ReadString("Names", "Candle", "") == "")
                WriteString("Names", "Candle", M2Share.g_Config.sCandle);
            M2Share.g_Config.sCandle = ReadString("Names", "Candle", M2Share.g_Config.sCandle);
            if (ReadString("Names", "BasicDrug", "") == "")
                WriteString("Names", "BasicDrug", M2Share.g_Config.sBasicDrug);
            M2Share.g_Config.sBasicDrug = ReadString("Names", "BasicDrug", M2Share.g_Config.sBasicDrug);
            if (ReadString("Names", "GoldStone", "") == "")
                WriteString("Names", "GoldStone", M2Share.g_Config.sGoldStone);
            M2Share.g_Config.sGoldStone = ReadString("Names", "GoldStone", M2Share.g_Config.sGoldStone);
            if (ReadString("Names", "SilverStone", "") == "")
                WriteString("Names", "SilverStone", M2Share.g_Config.sSilverStone);
            M2Share.g_Config.sSilverStone = ReadString("Names", "SilverStone", M2Share.g_Config.sSilverStone);
            if (ReadString("Names", "SteelStone", "") == "")
                WriteString("Names", "SteelStone", M2Share.g_Config.sSteelStone);
            M2Share.g_Config.sSteelStone = ReadString("Names", "SteelStone", M2Share.g_Config.sSteelStone);
            if (ReadString("Names", "CopperStone", "") == "")
                WriteString("Names", "CopperStone", M2Share.g_Config.sCopperStone);
            M2Share.g_Config.sCopperStone = ReadString("Names", "CopperStone", M2Share.g_Config.sCopperStone);
            if (ReadString("Names", "BlackStone", "") == "")
                WriteString("Names", "BlackStone", M2Share.g_Config.sBlackStone);
            M2Share.g_Config.sBlackStone = ReadString("Names", "BlackStone", M2Share.g_Config.sBlackStone);
            if (ReadString("Names", "Gem1Stone", "") == "")
                WriteString("Names", "Gem1Stone", M2Share.g_Config.sGemStone1);
            M2Share.g_Config.sGemStone1 = ReadString("Names", "Gem1Stone", M2Share.g_Config.sGemStone1);
            if (ReadString("Names", "Gem2Stone", "") == "")
                WriteString("Names", "Gem2Stone", M2Share.g_Config.sGemStone2);
            M2Share.g_Config.sGemStone2 = ReadString("Names", "Gem2Stone", M2Share.g_Config.sGemStone2);
            if (ReadString("Names", "Gem3Stone", "") == "")
                WriteString("Names", "Gem3Stone", M2Share.g_Config.sGemStone3);
            M2Share.g_Config.sGemStone3 = ReadString("Names", "Gem3Stone", M2Share.g_Config.sGemStone3);
            if (ReadString("Names", "Gem4Stone", "") == "")
                WriteString("Names", "Gem4Stone", M2Share.g_Config.sGemStone4);
            M2Share.g_Config.sGemStone4 = ReadString("Names", "Gem4Stone", M2Share.g_Config.sGemStone4);
            if (ReadString("Names", "Zuma1", "") == "")
                WriteString("Names", "Zuma1", M2Share.g_Config.sZuma[0]);
            M2Share.g_Config.sZuma[0] = ReadString("Names", "Zuma1", M2Share.g_Config.sZuma[0]);
            if (ReadString("Names", "Zuma2", "") == "")
                WriteString("Names", "Zuma2", M2Share.g_Config.sZuma[1]);
            M2Share.g_Config.sZuma[1] = ReadString("Names", "Zuma2", M2Share.g_Config.sZuma[1]);
            if (ReadString("Names", "Zuma3", "") == "")
                WriteString("Names", "Zuma3", M2Share.g_Config.sZuma[2]);
            M2Share.g_Config.sZuma[2] = ReadString("Names", "Zuma3", M2Share.g_Config.sZuma[2]);
            if (ReadString("Names", "Zuma4", "") == "")
                WriteString("Names", "Zuma4", M2Share.g_Config.sZuma[3]);
            M2Share.g_Config.sZuma[3] = ReadString("Names", "Zuma4", M2Share.g_Config.sZuma[3]);
            if (ReadString("Names", "Bee", "") == "")
                WriteString("Names", "Bee", M2Share.g_Config.sBee);
            M2Share.g_Config.sBee = ReadString("Names", "Bee", M2Share.g_Config.sBee);
            if (ReadString("Names", "Spider", "") == "")
                WriteString("Names", "Spider", M2Share.g_Config.sSpider);
            M2Share.g_Config.sSpider = ReadString("Names", "Spider", M2Share.g_Config.sSpider);
            if (ReadString("Names", "WomaHorn", "") == "")
                WriteString("Names", "WomaHorn", M2Share.g_Config.sWomaHorn);
            M2Share.g_Config.sWomaHorn = ReadString("Names", "WomaHorn", M2Share.g_Config.sWomaHorn);
            if (ReadString("Names", "ZumaPiece", "") == "")
                WriteString("Names", "ZumaPiece", M2Share.g_Config.sZumaPiece);
            M2Share.g_Config.sZumaPiece = ReadString("Names", "ZumaPiece", M2Share.g_Config.sZumaPiece);
            if (ReadString("Names", "Skeleton", "") == "")
                WriteString("Names", "Skeleton", M2Share.g_Config.sSkeleton);
            M2Share.g_Config.sSkeleton = ReadString("Names", "Skeleton", M2Share.g_Config.sSkeleton);
            if (ReadString("Names", "Dragon", "") == "")
                WriteString("Names", "Dragon", M2Share.g_Config.sDragon);
            M2Share.g_Config.sDragon = ReadString("Names", "Dragon", M2Share.g_Config.sDragon);
            if (ReadString("Names", "Dragon1", "") == "")
                WriteString("Names", "Dragon1", M2Share.g_Config.sDragon1);
            M2Share.g_Config.sDragon1 = ReadString("Names", "Dragon1", M2Share.g_Config.sDragon1);
            if (ReadString("Names", "Angel", "") == "")
                WriteString("Names", "Angel", M2Share.g_Config.sAngel);
            M2Share.g_Config.sAngel = ReadString("Names", "Angel", M2Share.g_Config.sAngel);
            sLoadString = ReadString("Names", "GameGold", "");
            if (sLoadString == "")
                WriteString("Share", "GameGold", M2Share.g_Config.sGameGoldName);
            else
                M2Share.g_Config.sGameGoldName = sLoadString;
            sLoadString = ReadString("Names", "GamePoint", "");
            if (sLoadString == "")
                WriteString("Share", "GamePoint", M2Share.g_Config.sGamePointName);
            else
                M2Share.g_Config.sGamePointName = sLoadString;
            sLoadString = ReadString("Names", "PayMentPointName", "");
            if (sLoadString == "")
                WriteString("Share", "PayMentPointName", M2Share.g_Config.sPayMentPointName);
            else
                M2Share.g_Config.sPayMentPointName = sLoadString;
            if (M2Share.g_Config.nAppIconCrc != 1242102148) M2Share.g_Config.boCheckFail = true;
            // ============================================================================
            // 游戏设置
            if (ReadInteger("Setup", "ItemNumber", -1) < 0)
                WriteInteger("Setup", "ItemNumber", M2Share.g_Config.nItemNumber);
            M2Share.g_Config.nItemNumber = ReadInteger("Setup", "ItemNumber", M2Share.g_Config.nItemNumber);
            M2Share.g_Config.nItemNumber = M2Share.g_Config.nItemNumber + M2Share.RandomNumber.Random(10000);
            if (ReadInteger("Setup", "ItemNumberEx", -1) < 0)
                WriteInteger("Setup", "ItemNumberEx", M2Share.g_Config.nItemNumberEx);
            M2Share.g_Config.nItemNumberEx = ReadInteger("Setup", "ItemNumberEx", M2Share.g_Config.nItemNumberEx);
            if (ReadString("Setup", "ClientFile1", "") == "")
                WriteString("Setup", "ClientFile1", M2Share.g_Config.sClientFile1);
            M2Share.g_Config.sClientFile1 = ReadString("Setup", "ClientFile1", M2Share.g_Config.sClientFile1);
            if (ReadString("Setup", "ClientFile2", "") == "")
                WriteString("Setup", "ClientFile2", M2Share.g_Config.sClientFile2);
            M2Share.g_Config.sClientFile2 = ReadString("Setup", "ClientFile2", M2Share.g_Config.sClientFile2);
            if (ReadString("Setup", "ClientFile3", "") == "")
                WriteString("Setup", "ClientFile3", M2Share.g_Config.sClientFile3);
            M2Share.g_Config.sClientFile3 = ReadString("Setup", "ClientFile3", M2Share.g_Config.sClientFile3);
            if (ReadInteger("Setup", "MonUpLvNeedKillBase", -1) < 0)
                WriteInteger("Setup", "MonUpLvNeedKillBase", M2Share.g_Config.nMonUpLvNeedKillBase);
            M2Share.g_Config.nMonUpLvNeedKillBase = ReadInteger("Setup", "MonUpLvNeedKillBase", M2Share.g_Config.nMonUpLvNeedKillBase);
            if (ReadInteger("Setup", "MonUpLvRate", -1) < 0)
            {
                WriteInteger("Setup", "MonUpLvRate", M2Share.g_Config.nMonUpLvRate);
            }
            M2Share.g_Config.nMonUpLvRate = ReadInteger("Setup", "MonUpLvRate", M2Share.g_Config.nMonUpLvRate);
            for (var i = 0; i < M2Share.g_Config.MonUpLvNeedKillCount.Length; i++)
            {
                if (ReadInteger("Setup", "MonUpLvNeedKillCount" + i, -1) < 0)
                {
                    WriteInteger("Setup", "MonUpLvNeedKillCount" + i, M2Share.g_Config.MonUpLvNeedKillCount[i]);
                }
                M2Share.g_Config.MonUpLvNeedKillCount[i] = ReadInteger("Setup", "MonUpLvNeedKillCount" + i, M2Share.g_Config.MonUpLvNeedKillCount[i]);
            }
            for (var i = 0; i < M2Share.g_Config.SlaveColor.Length; i++)
            {
                if (ReadInteger("Setup", "SlaveColor" + i, -1) < 0)
                {
                    WriteInteger("Setup", "SlaveColor" + i, M2Share.g_Config.SlaveColor[i]);
                }
                M2Share.g_Config.SlaveColor[i] = Read<byte>("Setup", "SlaveColor" + i, M2Share.g_Config.SlaveColor[i]);
            }
            if (ReadString("Setup", "HomeMap", "") == "")
            {
                WriteString("Setup", "HomeMap", M2Share.g_Config.sHomeMap);
            }
            M2Share.g_Config.sHomeMap = ReadString("Setup", "HomeMap", M2Share.g_Config.sHomeMap);
            if (ReadInteger("Setup", "HomeX", -1) < 0)
            {
                WriteInteger("Setup", "HomeX", M2Share.g_Config.nHomeX);
            }
            M2Share.g_Config.nHomeX = Read<short>("Setup", "HomeX", M2Share.g_Config.nHomeX);
            if (ReadInteger("Setup", "HomeY", -1) < 0)
            {
                WriteInteger("Setup", "HomeY", M2Share.g_Config.nHomeY);
            }
            M2Share.g_Config.nHomeY = Read<short>("Setup", "HomeY", M2Share.g_Config.nHomeY);
            if (ReadString("Setup", "RedHomeMap", "") == "")
            {
                WriteString("Setup", "RedHomeMap", M2Share.g_Config.sRedHomeMap);
            }
            M2Share.g_Config.sRedHomeMap = ReadString("Setup", "RedHomeMap", M2Share.g_Config.sRedHomeMap);
            if (ReadInteger("Setup", "RedHomeX", -1) < 0)
            {
                WriteInteger("Setup", "RedHomeX", M2Share.g_Config.nRedHomeX);
            }
            M2Share.g_Config.nRedHomeX = Read<short>("Setup", "RedHomeX", M2Share.g_Config.nRedHomeX);
            if (ReadInteger("Setup", "RedHomeY", -1) < 0)
                WriteInteger("Setup", "RedHomeY", M2Share.g_Config.nRedHomeY);
            M2Share.g_Config.nRedHomeY = Read<short>("Setup", "RedHomeY", M2Share.g_Config.nRedHomeY);
            if (ReadString("Setup", "RedDieHomeMap", "") == "")
                WriteString("Setup", "RedDieHomeMap", M2Share.g_Config.sRedDieHomeMap);
            M2Share.g_Config.sRedDieHomeMap = ReadString("Setup", "RedDieHomeMap", M2Share.g_Config.sRedDieHomeMap);
            if (ReadInteger("Setup", "RedDieHomeX", -1) < 0)
                WriteInteger("Setup", "RedDieHomeX", M2Share.g_Config.nRedDieHomeX);
            M2Share.g_Config.nRedDieHomeX = Read<short>("Setup", "RedDieHomeX", M2Share.g_Config.nRedDieHomeX);
            if (ReadInteger("Setup", "RedDieHomeY", -1) < 0)
                WriteInteger("Setup", "RedDieHomeY", M2Share.g_Config.nRedDieHomeY);
            M2Share.g_Config.nRedDieHomeY = Read<short>("Setup", "RedDieHomeY", M2Share.g_Config.nRedDieHomeY);
            if (ReadInteger("Setup", "JobHomePointSystem", -1) < 0)
                WriteBool("Setup", "JobHomePointSystem", M2Share.g_Config.boJobHomePoint);
            M2Share.g_Config.boJobHomePoint = ReadBool("Setup", "JobHomePointSystem", M2Share.g_Config.boJobHomePoint);
            if (ReadString("Setup", "WarriorHomeMap", "") == "")
                WriteString("Setup", "WarriorHomeMap", M2Share.g_Config.sWarriorHomeMap);
            M2Share.g_Config.sWarriorHomeMap = ReadString("Setup", "WarriorHomeMap", M2Share.g_Config.sWarriorHomeMap);
            if (ReadInteger("Setup", "WarriorHomeX", -1) < 0)
                WriteInteger("Setup", "WarriorHomeX", M2Share.g_Config.nWarriorHomeX);
            M2Share.g_Config.nWarriorHomeX = Read<short>("Setup", "WarriorHomeX", M2Share.g_Config.nWarriorHomeX);
            if (ReadInteger("Setup", "WarriorHomeY", -1) < 0)
                WriteInteger("Setup", "WarriorHomeY", M2Share.g_Config.nWarriorHomeY);
            M2Share.g_Config.nWarriorHomeY = Read<short>("Setup", "WarriorHomeY", M2Share.g_Config.nWarriorHomeY);
            if (ReadString("Setup", "WizardHomeMap", "") == "")
                WriteString("Setup", "WizardHomeMap", M2Share.g_Config.sWizardHomeMap);
            M2Share.g_Config.sWizardHomeMap = ReadString("Setup", "WizardHomeMap", M2Share.g_Config.sWizardHomeMap);
            if (ReadInteger("Setup", "WizardHomeX", -1) < 0)
                WriteInteger("Setup", "WizardHomeX", M2Share.g_Config.nWizardHomeX);
            M2Share.g_Config.nWizardHomeX = Read<short>("Setup", "WizardHomeX", M2Share.g_Config.nWizardHomeX);
            if (ReadInteger("Setup", "WizardHomeY", -1) < 0)
                WriteInteger("Setup", "WizardHomeY", M2Share.g_Config.nWizardHomeY);
            M2Share.g_Config.nWizardHomeY = Read<short>("Setup", "WizardHomeY", M2Share.g_Config.nWizardHomeY);
            if (ReadString("Setup", "TaoistHomeMap", "") == "")
                WriteString("Setup", "TaoistHomeMap", M2Share.g_Config.sTaoistHomeMap);
            M2Share.g_Config.sTaoistHomeMap = ReadString("Setup", "TaoistHomeMap", M2Share.g_Config.sTaoistHomeMap);
            if (ReadInteger("Setup", "TaoistHomeX", -1) < 0)
                WriteInteger("Setup", "TaoistHomeX", M2Share.g_Config.nTaoistHomeX);
            M2Share.g_Config.nTaoistHomeX = Read<short>("Setup", "TaoistHomeX", M2Share.g_Config.nTaoistHomeX);
            if (ReadInteger("Setup", "TaoistHomeY", -1) < 0)
                WriteInteger("Setup", "TaoistHomeY", M2Share.g_Config.nTaoistHomeY);
            M2Share.g_Config.nTaoistHomeY = Read<short>("Setup", "TaoistHomeY", M2Share.g_Config.nTaoistHomeY);
            nLoadInteger = ReadInteger("Setup", "HealthFillTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "HealthFillTime", M2Share.g_Config.nHealthFillTime);
            else
                M2Share.g_Config.nHealthFillTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "SpellFillTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "SpellFillTime", M2Share.g_Config.nSpellFillTime);
            else
                M2Share.g_Config.nSpellFillTime = nLoadInteger;
            if (ReadInteger("Setup", "DecPkPointTime", -1) < 0)
                WriteInteger("Setup", "DecPkPointTime", M2Share.g_Config.dwDecPkPointTime);
            M2Share.g_Config.dwDecPkPointTime = ReadInteger("Setup", "DecPkPointTime", M2Share.g_Config.dwDecPkPointTime);
            if (ReadInteger("Setup", "DecPkPointCount", -1) < 0)
                WriteInteger("Setup", "DecPkPointCount", M2Share.g_Config.nDecPkPointCount);
            M2Share.g_Config.nDecPkPointCount = ReadInteger("Setup", "DecPkPointCount", M2Share.g_Config.nDecPkPointCount);
            if (ReadInteger("Setup", "PKFlagTime", -1) < 0)
                WriteInteger("Setup", "PKFlagTime", M2Share.g_Config.dwPKFlagTime);
            M2Share.g_Config.dwPKFlagTime = ReadInteger("Setup", "PKFlagTime", M2Share.g_Config.dwPKFlagTime);
            if (ReadInteger("Setup", "KillHumanAddPKPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanAddPKPoint", M2Share.g_Config.nKillHumanAddPKPoint);
            M2Share.g_Config.nKillHumanAddPKPoint = ReadInteger("Setup", "KillHumanAddPKPoint", M2Share.g_Config.nKillHumanAddPKPoint);
            if (ReadInteger("Setup", "KillHumanDecLuckPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanDecLuckPoint", M2Share.g_Config.nKillHumanDecLuckPoint);
            M2Share.g_Config.nKillHumanDecLuckPoint = ReadInteger("Setup", "KillHumanDecLuckPoint", M2Share.g_Config.nKillHumanDecLuckPoint);
            if (ReadInteger("Setup", "DecLightItemDrugTime", -1) < 0)
                WriteInteger("Setup", "DecLightItemDrugTime", M2Share.g_Config.dwDecLightItemDrugTime);
            M2Share.g_Config.dwDecLightItemDrugTime = ReadInteger("Setup", "DecLightItemDrugTime", M2Share.g_Config.dwDecLightItemDrugTime);
            if (ReadInteger("Setup", "SafeZoneSize", -1) < 0)
                WriteInteger("Setup", "SafeZoneSize", M2Share.g_Config.nSafeZoneSize);
            M2Share.g_Config.nSafeZoneSize =
                ReadInteger("Setup", "SafeZoneSize", M2Share.g_Config.nSafeZoneSize);
            if (ReadInteger("Setup", "StartPointSize", -1) < 0)
                WriteInteger("Setup", "StartPointSize", M2Share.g_Config.nStartPointSize);
            M2Share.g_Config.nStartPointSize =
                ReadInteger("Setup", "StartPointSize", M2Share.g_Config.nStartPointSize);
            for (var i = 0; i < M2Share.g_Config.ReNewNameColor.Length; i++)
            {
                if (ReadInteger("Setup", "ReNewNameColor" + i, -1) < 0)
                {
                    WriteInteger("Setup", "ReNewNameColor" + i, M2Share.g_Config.ReNewNameColor[i]);
                }
                M2Share.g_Config.ReNewNameColor[i] = Read<byte>("Setup", "ReNewNameColor" + i, M2Share.g_Config.ReNewNameColor[i]);
            }
            if (ReadInteger("Setup", "ReNewNameColorTime", -1) < 0)
                WriteInteger("Setup", "ReNewNameColorTime", M2Share.g_Config.dwReNewNameColorTime);
            M2Share.g_Config.dwReNewNameColorTime = ReadInteger("Setup", "ReNewNameColorTime", M2Share.g_Config.dwReNewNameColorTime);
            if (ReadInteger("Setup", "ReNewChangeColor", -1) < 0)
                WriteBool("Setup", "ReNewChangeColor", M2Share.g_Config.boReNewChangeColor);
            M2Share.g_Config.boReNewChangeColor = ReadBool("Setup", "ReNewChangeColor", M2Share.g_Config.boReNewChangeColor);
            if (ReadInteger("Setup", "ReNewLevelClearExp", -1) < 0)
                WriteBool("Setup", "ReNewLevelClearExp", M2Share.g_Config.boReNewLevelClearExp);
            M2Share.g_Config.boReNewLevelClearExp = ReadBool("Setup", "ReNewLevelClearExp", M2Share.g_Config.boReNewLevelClearExp);
            if (ReadInteger("Setup", "BonusAbilofWarrDC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrDC", M2Share.g_Config.BonusAbilofWarr.DC);
            M2Share.g_Config.BonusAbilofWarr.DC = Read<ushort>("Setup", "BonusAbilofWarrDC", M2Share.g_Config.BonusAbilofWarr.DC);
            if (ReadInteger("Setup", "BonusAbilofWarrMC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrMC", M2Share.g_Config.BonusAbilofWarr.MC);
            M2Share.g_Config.BonusAbilofWarr.MC = Read<ushort>("Setup", "BonusAbilofWarrMC", M2Share.g_Config.BonusAbilofWarr.MC);
            if (ReadInteger("Setup", "BonusAbilofWarrSC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrSC", M2Share.g_Config.BonusAbilofWarr.SC);
            M2Share.g_Config.BonusAbilofWarr.SC = Read<ushort>("Setup", "BonusAbilofWarrSC", M2Share.g_Config.BonusAbilofWarr.SC);
            if (ReadInteger("Setup", "BonusAbilofWarrAC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrAC", M2Share.g_Config.BonusAbilofWarr.AC);
            M2Share.g_Config.BonusAbilofWarr.AC = Read<ushort>("Setup", "BonusAbilofWarrAC", M2Share.g_Config.BonusAbilofWarr.AC);
            if (ReadInteger("Setup", "BonusAbilofWarrMAC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrMAC", M2Share.g_Config.BonusAbilofWarr.MAC);
            M2Share.g_Config.BonusAbilofWarr.MAC = Read<ushort>("Setup", "BonusAbilofWarrMAC", M2Share.g_Config.BonusAbilofWarr.MAC);
            if (ReadInteger("Setup", "BonusAbilofWarrHP", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrHP", M2Share.g_Config.BonusAbilofWarr.HP);
            M2Share.g_Config.BonusAbilofWarr.HP = Read<ushort>("Setup", "BonusAbilofWarrHP", M2Share.g_Config.BonusAbilofWarr.HP);
            if (ReadInteger("Setup", "BonusAbilofWarrMP", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrMP", M2Share.g_Config.BonusAbilofWarr.MP);
            M2Share.g_Config.BonusAbilofWarr.MP = Read<ushort>("Setup", "BonusAbilofWarrMP", M2Share.g_Config.BonusAbilofWarr.MP);
            if (ReadInteger("Setup", "BonusAbilofWarrHit", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrHit", M2Share.g_Config.BonusAbilofWarr.Hit);
            M2Share.g_Config.BonusAbilofWarr.Hit = Read<byte>("Setup", "BonusAbilofWarrHit", M2Share.g_Config.BonusAbilofWarr.Hit);
            if (ReadInteger("Setup", "BonusAbilofWarrSpeed", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrSpeed", M2Share.g_Config.BonusAbilofWarr.Speed);
            M2Share.g_Config.BonusAbilofWarr.Speed = ReadInteger("Setup", "BonusAbilofWarrSpeed", M2Share.g_Config.BonusAbilofWarr.Speed);
            if (ReadInteger("Setup", "BonusAbilofWarrX2", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrX2", M2Share.g_Config.BonusAbilofWarr.X2);
            M2Share.g_Config.BonusAbilofWarr.X2 = Read<byte>("Setup", "BonusAbilofWarrX2", M2Share.g_Config.BonusAbilofWarr.X2);
            if (ReadInteger("Setup", "BonusAbilofWizardDC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardDC", M2Share.g_Config.BonusAbilofWizard.DC);
            M2Share.g_Config.BonusAbilofWizard.DC = Read<ushort>("Setup", "BonusAbilofWizardDC", M2Share.g_Config.BonusAbilofWizard.DC);
            if (ReadInteger("Setup", "BonusAbilofWizardMC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardMC", M2Share.g_Config.BonusAbilofWizard.MC);
            M2Share.g_Config.BonusAbilofWizard.MC = Read<ushort>("Setup", "BonusAbilofWizardMC", M2Share.g_Config.BonusAbilofWizard.MC);
            if (ReadInteger("Setup", "BonusAbilofWizardSC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardSC", M2Share.g_Config.BonusAbilofWizard.SC);
            M2Share.g_Config.BonusAbilofWizard.SC = Read<ushort>("Setup", "BonusAbilofWizardSC", M2Share.g_Config.BonusAbilofWizard.SC);
            if (ReadInteger("Setup", "BonusAbilofWizardAC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardAC", M2Share.g_Config.BonusAbilofWizard.AC);
            M2Share.g_Config.BonusAbilofWizard.AC = Read<ushort>("Setup", "BonusAbilofWizardAC", M2Share.g_Config.BonusAbilofWizard.AC);
            if (ReadInteger("Setup", "BonusAbilofWizardMAC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardMAC", M2Share.g_Config.BonusAbilofWizard.MAC);
            M2Share.g_Config.BonusAbilofWizard.MAC = Read<ushort>("Setup", "BonusAbilofWizardMAC", M2Share.g_Config.BonusAbilofWizard.MAC);
            if (ReadInteger("Setup", "BonusAbilofWizardHP", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardHP", M2Share.g_Config.BonusAbilofWizard.HP);
            M2Share.g_Config.BonusAbilofWizard.HP = Read<ushort>("Setup", "BonusAbilofWizardHP", M2Share.g_Config.BonusAbilofWizard.HP);
            if (ReadInteger("Setup", "BonusAbilofWizardMP", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardMP", M2Share.g_Config.BonusAbilofWizard.MP);
            M2Share.g_Config.BonusAbilofWizard.MP = Read<ushort>("Setup", "BonusAbilofWizardMP", M2Share.g_Config.BonusAbilofWizard.MP);
            if (ReadInteger("Setup", "BonusAbilofWizardHit", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardHit", M2Share.g_Config.BonusAbilofWizard.Hit);
            M2Share.g_Config.BonusAbilofWizard.Hit = Read<byte>("Setup", "BonusAbilofWizardHit", M2Share.g_Config.BonusAbilofWizard.Hit);
            if (ReadInteger("Setup", "BonusAbilofWizardSpeed", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardSpeed", M2Share.g_Config.BonusAbilofWizard.Speed);
            M2Share.g_Config.BonusAbilofWizard.Speed = ReadInteger("Setup", "BonusAbilofWizardSpeed", M2Share.g_Config.BonusAbilofWizard.Speed);
            if (ReadInteger("Setup", "BonusAbilofWizardX2", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardX2", M2Share.g_Config.BonusAbilofWizard.X2);
            M2Share.g_Config.BonusAbilofWizard.X2 = Read<byte>("Setup", "BonusAbilofWizardX2", M2Share.g_Config.BonusAbilofWizard.X2);
            if (ReadInteger("Setup", "BonusAbilofTaosDC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosDC", M2Share.g_Config.BonusAbilofTaos.DC);
            M2Share.g_Config.BonusAbilofTaos.DC = Read<byte>("Setup", "BonusAbilofTaosDC", M2Share.g_Config.BonusAbilofTaos.DC);
            if (ReadInteger("Setup", "BonusAbilofTaosMC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosMC", M2Share.g_Config.BonusAbilofTaos.MC);
            M2Share.g_Config.BonusAbilofTaos.MC = Read<byte>("Setup", "BonusAbilofTaosMC", M2Share.g_Config.BonusAbilofTaos.MC);
            if (ReadInteger("Setup", "BonusAbilofTaosSC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosSC", M2Share.g_Config.BonusAbilofTaos.SC);
            M2Share.g_Config.BonusAbilofTaos.SC = Read<byte>("Setup", "BonusAbilofTaosSC", M2Share.g_Config.BonusAbilofTaos.SC);
            if (ReadInteger("Setup", "BonusAbilofTaosAC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosAC", M2Share.g_Config.BonusAbilofTaos.AC);
            M2Share.g_Config.BonusAbilofTaos.AC = Read<byte>("Setup", "BonusAbilofTaosAC", M2Share.g_Config.BonusAbilofTaos.AC);
            if (ReadInteger("Setup", "BonusAbilofTaosMAC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosMAC", M2Share.g_Config.BonusAbilofTaos.MAC);
            M2Share.g_Config.BonusAbilofTaos.MAC = Read<ushort>("Setup", "BonusAbilofTaosMAC", M2Share.g_Config.BonusAbilofTaos.MAC);
            if (ReadInteger("Setup", "BonusAbilofTaosHP", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosHP", M2Share.g_Config.BonusAbilofTaos.HP);
            M2Share.g_Config.BonusAbilofTaos.HP = Read<ushort>("Setup", "BonusAbilofTaosHP", M2Share.g_Config.BonusAbilofTaos.HP);
            if (ReadInteger("Setup", "BonusAbilofTaosMP", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosMP", M2Share.g_Config.BonusAbilofTaos.MP);
            M2Share.g_Config.BonusAbilofTaos.MP = Read<ushort>("Setup", "BonusAbilofTaosMP", M2Share.g_Config.BonusAbilofTaos.MP);
            if (ReadInteger("Setup", "BonusAbilofTaosHit", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosHit", M2Share.g_Config.BonusAbilofTaos.Hit);
            M2Share.g_Config.BonusAbilofTaos.Hit = Read<byte>("Setup", "BonusAbilofTaosHit", M2Share.g_Config.BonusAbilofTaos.Hit);
            if (ReadInteger("Setup", "BonusAbilofTaosSpeed", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosSpeed", M2Share.g_Config.BonusAbilofTaos.Speed);
            M2Share.g_Config.BonusAbilofTaos.Speed = ReadInteger("Setup", "BonusAbilofTaosSpeed", M2Share.g_Config.BonusAbilofTaos.Speed);
            if (ReadInteger("Setup", "BonusAbilofTaosX2", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosX2", M2Share.g_Config.BonusAbilofTaos.X2);
            M2Share.g_Config.BonusAbilofTaos.X2 = Read<byte>("Setup", "BonusAbilofTaosX2", M2Share.g_Config.BonusAbilofTaos.X2);
            if (ReadInteger("Setup", "NakedAbilofWarrDC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrDC", M2Share.g_Config.NakedAbilofWarr.DC);
            M2Share.g_Config.NakedAbilofWarr.DC = Read<ushort>("Setup", "NakedAbilofWarrDC", M2Share.g_Config.NakedAbilofWarr.DC);
            if (ReadInteger("Setup", "NakedAbilofWarrMC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrMC", M2Share.g_Config.NakedAbilofWarr.MC);
            M2Share.g_Config.NakedAbilofWarr.MC = Read<ushort>("Setup", "NakedAbilofWarrMC", M2Share.g_Config.NakedAbilofWarr.MC);
            if (ReadInteger("Setup", "NakedAbilofWarrSC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrSC", M2Share.g_Config.NakedAbilofWarr.SC);
            M2Share.g_Config.NakedAbilofWarr.SC = Read<ushort>("Setup", "NakedAbilofWarrSC", M2Share.g_Config.NakedAbilofWarr.SC);
            if (ReadInteger("Setup", "NakedAbilofWarrAC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrAC", M2Share.g_Config.NakedAbilofWarr.AC);
            M2Share.g_Config.NakedAbilofWarr.AC = Read<ushort>("Setup", "NakedAbilofWarrAC", M2Share.g_Config.NakedAbilofWarr.AC);
            if (ReadInteger("Setup", "NakedAbilofWarrMAC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrMAC", M2Share.g_Config.NakedAbilofWarr.MAC);
            M2Share.g_Config.NakedAbilofWarr.MAC = Read<ushort>("Setup", "NakedAbilofWarrMAC", M2Share.g_Config.NakedAbilofWarr.MAC);
            if (ReadInteger("Setup", "NakedAbilofWarrHP", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrHP", M2Share.g_Config.NakedAbilofWarr.HP);
            M2Share.g_Config.NakedAbilofWarr.HP = Read<ushort>("Setup", "NakedAbilofWarrHP", M2Share.g_Config.NakedAbilofWarr.HP);
            if (ReadInteger("Setup", "NakedAbilofWarrMP", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrMP", M2Share.g_Config.NakedAbilofWarr.MP);
            M2Share.g_Config.NakedAbilofWarr.MP = Read<ushort>("Setup", "NakedAbilofWarrMP", M2Share.g_Config.NakedAbilofWarr.MP);
            if (ReadInteger("Setup", "NakedAbilofWarrHit", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrHit", M2Share.g_Config.NakedAbilofWarr.Hit);
            M2Share.g_Config.NakedAbilofWarr.Hit = Read<byte>("Setup", "NakedAbilofWarrHit", M2Share.g_Config.NakedAbilofWarr.Hit);
            if (ReadInteger("Setup", "NakedAbilofWarrSpeed", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrSpeed", M2Share.g_Config.NakedAbilofWarr.Speed);
            M2Share.g_Config.NakedAbilofWarr.Speed = ReadInteger("Setup", "NakedAbilofWarrSpeed", M2Share.g_Config.NakedAbilofWarr.Speed);
            if (ReadInteger("Setup", "NakedAbilofWarrX2", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrX2", M2Share.g_Config.NakedAbilofWarr.X2);
            M2Share.g_Config.NakedAbilofWarr.X2 = Read<byte>("Setup", "NakedAbilofWarrX2", M2Share.g_Config.NakedAbilofWarr.X2);
            if (ReadInteger("Setup", "NakedAbilofWizardDC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardDC", M2Share.g_Config.NakedAbilofWizard.DC);
            M2Share.g_Config.NakedAbilofWizard.DC = Read<ushort>("Setup", "NakedAbilofWizardDC", M2Share.g_Config.NakedAbilofWizard.DC);
            if (ReadInteger("Setup", "NakedAbilofWizardMC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardMC", M2Share.g_Config.NakedAbilofWizard.MC);
            M2Share.g_Config.NakedAbilofWizard.MC = Read<ushort>("Setup", "NakedAbilofWizardMC", M2Share.g_Config.NakedAbilofWizard.MC);
            if (ReadInteger("Setup", "NakedAbilofWizardSC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardSC", M2Share.g_Config.NakedAbilofWizard.SC);
            M2Share.g_Config.NakedAbilofWizard.SC = Read<ushort>("Setup", "NakedAbilofWizardSC", M2Share.g_Config.NakedAbilofWizard.SC);
            if (ReadInteger("Setup", "NakedAbilofWizardAC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardAC", M2Share.g_Config.NakedAbilofWizard.AC);
            M2Share.g_Config.NakedAbilofWizard.AC = Read<ushort>("Setup", "NakedAbilofWizardAC", M2Share.g_Config.NakedAbilofWizard.AC);
            if (ReadInteger("Setup", "NakedAbilofWizardMAC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardMAC", M2Share.g_Config.NakedAbilofWizard.MAC);
            M2Share.g_Config.NakedAbilofWizard.MAC = Read<ushort>("Setup", "NakedAbilofWizardMAC", M2Share.g_Config.NakedAbilofWizard.MAC);
            if (ReadInteger("Setup", "NakedAbilofWizardHP", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardHP", M2Share.g_Config.NakedAbilofWizard.HP);
            M2Share.g_Config.NakedAbilofWizard.HP = Read<ushort>("Setup", "NakedAbilofWizardHP", M2Share.g_Config.NakedAbilofWizard.HP);
            if (ReadInteger("Setup", "NakedAbilofWizardMP", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardMP", M2Share.g_Config.NakedAbilofWizard.MP);
            M2Share.g_Config.NakedAbilofWizard.MP = Read<ushort>("Setup", "NakedAbilofWizardMP", M2Share.g_Config.NakedAbilofWizard.MP);
            if (ReadInteger("Setup", "NakedAbilofWizardHit", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardHit", M2Share.g_Config.NakedAbilofWizard.Hit);
            M2Share.g_Config.NakedAbilofWizard.Hit = Read<byte>("Setup", "NakedAbilofWizardHit", M2Share.g_Config.NakedAbilofWizard.Hit);
            if (ReadInteger("Setup", "NakedAbilofWizardSpeed", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardSpeed", M2Share.g_Config.NakedAbilofWizard.Speed);
            M2Share.g_Config.NakedAbilofWizard.Speed = ReadInteger("Setup", "NakedAbilofWizardSpeed", M2Share.g_Config.NakedAbilofWizard.Speed);
            if (ReadInteger("Setup", "NakedAbilofWizardX2", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardX2", M2Share.g_Config.NakedAbilofWizard.X2);
            M2Share.g_Config.NakedAbilofWizard.X2 = Read<byte>("Setup", "NakedAbilofWizardX2", M2Share.g_Config.NakedAbilofWizard.X2);
            if (ReadInteger("Setup", "NakedAbilofTaosDC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosDC", M2Share.g_Config.NakedAbilofTaos.DC);
            M2Share.g_Config.NakedAbilofTaos.DC = Read<ushort>("Setup", "NakedAbilofTaosDC", M2Share.g_Config.NakedAbilofTaos.DC);
            if (ReadInteger("Setup", "NakedAbilofTaosMC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosMC", M2Share.g_Config.NakedAbilofTaos.MC);
            M2Share.g_Config.NakedAbilofTaos.MC = Read<ushort>("Setup", "NakedAbilofTaosMC", M2Share.g_Config.NakedAbilofTaos.MC);
            if (ReadInteger("Setup", "NakedAbilofTaosSC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosSC", M2Share.g_Config.NakedAbilofTaos.SC);
            M2Share.g_Config.NakedAbilofTaos.SC = Read<ushort>("Setup", "NakedAbilofTaosSC", M2Share.g_Config.NakedAbilofTaos.SC);
            if (ReadInteger("Setup", "NakedAbilofTaosAC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosAC", M2Share.g_Config.NakedAbilofTaos.AC);
            M2Share.g_Config.NakedAbilofTaos.AC = Read<ushort>("Setup", "NakedAbilofTaosAC", M2Share.g_Config.NakedAbilofTaos.AC);
            if (ReadInteger("Setup", "NakedAbilofTaosMAC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosMAC", M2Share.g_Config.NakedAbilofTaos.MAC);
            M2Share.g_Config.NakedAbilofTaos.MAC = Read<ushort>("Setup", "NakedAbilofTaosMAC", M2Share.g_Config.NakedAbilofTaos.MAC);
            if (ReadInteger("Setup", "NakedAbilofTaosHP", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosHP", M2Share.g_Config.NakedAbilofTaos.HP);
            M2Share.g_Config.NakedAbilofTaos.HP = Read<ushort>("Setup", "NakedAbilofTaosHP", M2Share.g_Config.NakedAbilofTaos.HP);
            if (ReadInteger("Setup", "NakedAbilofTaosMP", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosMP", M2Share.g_Config.NakedAbilofTaos.MP);
            M2Share.g_Config.NakedAbilofTaos.MP = Read<ushort>("Setup", "NakedAbilofTaosMP", M2Share.g_Config.NakedAbilofTaos.MP);
            if (ReadInteger("Setup", "NakedAbilofTaosHit", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosHit", M2Share.g_Config.NakedAbilofTaos.Hit);
            M2Share.g_Config.NakedAbilofTaos.Hit = Read<byte>("Setup", "NakedAbilofTaosHit", M2Share.g_Config.NakedAbilofTaos.Hit);
            if (ReadInteger("Setup", "NakedAbilofTaosSpeed", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosSpeed", M2Share.g_Config.NakedAbilofTaos.Speed);
            M2Share.g_Config.NakedAbilofTaos.Speed = ReadInteger("Setup", "NakedAbilofTaosSpeed", M2Share.g_Config.NakedAbilofTaos.Speed);
            if (ReadInteger("Setup", "NakedAbilofTaosX2", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosX2", M2Share.g_Config.NakedAbilofTaos.X2);
            M2Share.g_Config.NakedAbilofTaos.X2 = Read<byte>("Setup", "NakedAbilofTaosX2", M2Share.g_Config.NakedAbilofTaos.X2);
            if (ReadInteger("Setup", "GroupMembersMax", -1) < 0)
                WriteInteger("Setup", "GroupMembersMax", M2Share.g_Config.nGroupMembersMax);
            M2Share.g_Config.nGroupMembersMax = ReadInteger("Setup", "GroupMembersMax", M2Share.g_Config.nGroupMembersMax);
            if (ReadInteger("Setup", "WarrAttackMon", -1) < 0)
                WriteInteger("Setup", "WarrAttackMon", M2Share.g_Config.nWarrMon);
            M2Share.g_Config.nWarrMon = ReadInteger("Setup", "WarrAttackMon", M2Share.g_Config.nWarrMon);
            if (ReadInteger("Setup", "WizardAttackMon", -1) < 0)
                WriteInteger("Setup", "WizardAttackMon", M2Share.g_Config.nWizardMon);
            M2Share.g_Config.nWizardMon = ReadInteger("Setup", "WizardAttackMon", M2Share.g_Config.nWizardMon);
            if (ReadInteger("Setup", "TaosAttackMon", -1) < 0)
                WriteInteger("Setup", "TaosAttackMon", M2Share.g_Config.nTaosMon);
            M2Share.g_Config.nTaosMon = ReadInteger("Setup", "TaosAttackMon", M2Share.g_Config.nTaosMon);
            if (ReadInteger("Setup", "MonAttackHum", -1) < 0)
                WriteInteger("Setup", "MonAttackHum", M2Share.g_Config.nMonHum);
            M2Share.g_Config.nMonHum = ReadInteger("Setup", "MonAttackHum", M2Share.g_Config.nMonHum);
            if (ReadInteger("Setup", "UPgradeWeaponGetBackTime", -1) < 0)
            {
                WriteInteger("Setup", "UPgradeWeaponGetBackTime", M2Share.g_Config.dwUPgradeWeaponGetBackTime);
            }
            M2Share.g_Config.dwUPgradeWeaponGetBackTime = ReadInteger("Setup", "UPgradeWeaponGetBackTime", M2Share.g_Config.dwUPgradeWeaponGetBackTime);
            if (ReadInteger("Setup", "ClearExpireUpgradeWeaponDays", -1) < 0)
            {
                WriteInteger("Setup", "ClearExpireUpgradeWeaponDays", M2Share.g_Config.nClearExpireUpgradeWeaponDays);
            }
            M2Share.g_Config.nClearExpireUpgradeWeaponDays = ReadInteger("Setup", "ClearExpireUpgradeWeaponDays", M2Share.g_Config.nClearExpireUpgradeWeaponDays);
            if (ReadInteger("Setup", "UpgradeWeaponPrice", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponPrice", M2Share.g_Config.nUpgradeWeaponPrice);
            M2Share.g_Config.nUpgradeWeaponPrice = ReadInteger("Setup", "UpgradeWeaponPrice", M2Share.g_Config.nUpgradeWeaponPrice);
            if (ReadInteger("Setup", "UpgradeWeaponMaxPoint", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponMaxPoint", M2Share.g_Config.nUpgradeWeaponMaxPoint);
            M2Share.g_Config.nUpgradeWeaponMaxPoint = ReadInteger("Setup", "UpgradeWeaponMaxPoint", M2Share.g_Config.nUpgradeWeaponMaxPoint);
            if (ReadInteger("Setup", "UpgradeWeaponDCRate", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponDCRate", M2Share.g_Config.nUpgradeWeaponDCRate);
            M2Share.g_Config.nUpgradeWeaponDCRate = ReadInteger("Setup", "UpgradeWeaponDCRate", M2Share.g_Config.nUpgradeWeaponDCRate);
            if (ReadInteger("Setup", "UpgradeWeaponDCTwoPointRate", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponDCTwoPointRate", M2Share.g_Config.nUpgradeWeaponDCTwoPointRate);
            M2Share.g_Config.nUpgradeWeaponDCTwoPointRate = ReadInteger("Setup", "UpgradeWeaponDCTwoPointRate", M2Share.g_Config.nUpgradeWeaponDCTwoPointRate);
            if (ReadInteger("Setup", "UpgradeWeaponDCThreePointRate", -1) < 0)
            {
                WriteInteger("Setup", "UpgradeWeaponDCThreePointRate", M2Share.g_Config.nUpgradeWeaponDCThreePointRate);
            }
            M2Share.g_Config.nUpgradeWeaponDCThreePointRate = ReadInteger("Setup", "UpgradeWeaponDCThreePointRate", M2Share.g_Config.nUpgradeWeaponDCThreePointRate);
            if (ReadInteger("Setup", "UpgradeWeaponMCRate", -1) < 0)
            {
                WriteInteger("Setup", "UpgradeWeaponMCRate", M2Share.g_Config.nUpgradeWeaponMCRate);
            }
            M2Share.g_Config.nUpgradeWeaponMCRate = ReadInteger("Setup", "UpgradeWeaponMCRate", M2Share.g_Config.nUpgradeWeaponMCRate);
            if (ReadInteger("Setup", "UpgradeWeaponMCTwoPointRate", -1) < 0)
            {
                WriteInteger("Setup", "UpgradeWeaponMCTwoPointRate", M2Share.g_Config.nUpgradeWeaponMCTwoPointRate);
            }
            M2Share.g_Config.nUpgradeWeaponMCTwoPointRate = ReadInteger("Setup", "UpgradeWeaponMCTwoPointRate", M2Share.g_Config.nUpgradeWeaponMCTwoPointRate);
            if (ReadInteger("Setup", "UpgradeWeaponMCThreePointRate", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponMCThreePointRate", M2Share.g_Config.nUpgradeWeaponMCThreePointRate);
            M2Share.g_Config.nUpgradeWeaponMCThreePointRate = ReadInteger("Setup", "UpgradeWeaponMCThreePointRate", M2Share.g_Config.nUpgradeWeaponMCThreePointRate);
            if (ReadInteger("Setup", "UpgradeWeaponSCRate", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponSCRate", M2Share.g_Config.nUpgradeWeaponSCRate);
            M2Share.g_Config.nUpgradeWeaponSCRate =
                ReadInteger("Setup", "UpgradeWeaponSCRate", M2Share.g_Config.nUpgradeWeaponSCRate);
            if (ReadInteger("Setup", "UpgradeWeaponSCTwoPointRate", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponSCTwoPointRate", M2Share.g_Config.nUpgradeWeaponSCTwoPointRate);
            M2Share.g_Config.nUpgradeWeaponSCTwoPointRate = ReadInteger("Setup", "UpgradeWeaponSCTwoPointRate", M2Share.g_Config.nUpgradeWeaponSCTwoPointRate);
            if (ReadInteger("Setup", "UpgradeWeaponSCThreePointRate", -1) < 0)
            {
                WriteInteger("Setup", "UpgradeWeaponSCThreePointRate", M2Share.g_Config.nUpgradeWeaponSCThreePointRate);
            }
            M2Share.g_Config.nUpgradeWeaponSCThreePointRate = ReadInteger("Setup", "UpgradeWeaponSCThreePointRate", M2Share.g_Config.nUpgradeWeaponSCThreePointRate);
            if (ReadInteger("Setup", "BuildGuild", -1) < 0)
                WriteInteger("Setup", "BuildGuild", M2Share.g_Config.nBuildGuildPrice);
            M2Share.g_Config.nBuildGuildPrice = ReadInteger("Setup", "BuildGuild", M2Share.g_Config.nBuildGuildPrice);
            if (ReadInteger("Setup", "MakeDurg", -1) < 0)
                WriteInteger("Setup", "MakeDurg", M2Share.g_Config.nMakeDurgPrice);
            M2Share.g_Config.nMakeDurgPrice = ReadInteger("Setup", "MakeDurg", M2Share.g_Config.nMakeDurgPrice);
            if (ReadInteger("Setup", "GuildWarFee", -1) < 0)
                WriteInteger("Setup", "GuildWarFee", M2Share.g_Config.nGuildWarPrice);
            M2Share.g_Config.nGuildWarPrice = ReadInteger("Setup", "GuildWarFee", M2Share.g_Config.nGuildWarPrice);
            if (ReadInteger("Setup", "HireGuard", -1) < 0)
                WriteInteger("Setup", "HireGuard", M2Share.g_Config.nHireGuardPrice);
            M2Share.g_Config.nHireGuardPrice = ReadInteger("Setup", "HireGuard", M2Share.g_Config.nHireGuardPrice);
            if (ReadInteger("Setup", "HireArcher", -1) < 0)
                WriteInteger("Setup", "HireArcher", M2Share.g_Config.nHireArcherPrice);
            M2Share.g_Config.nHireArcherPrice = ReadInteger("Setup", "HireArcher", M2Share.g_Config.nHireArcherPrice);
            if (ReadInteger("Setup", "RepairDoor", -1) < 0)
                WriteInteger("Setup", "RepairDoor", M2Share.g_Config.nRepairDoorPrice);
            M2Share.g_Config.nRepairDoorPrice = ReadInteger("Setup", "RepairDoor", M2Share.g_Config.nRepairDoorPrice);
            if (ReadInteger("Setup", "RepairWall", -1) < 0)
                WriteInteger("Setup", "RepairWall", M2Share.g_Config.nRepairWallPrice);
            M2Share.g_Config.nRepairWallPrice = ReadInteger("Setup", "RepairWall", M2Share.g_Config.nRepairWallPrice);
            if (ReadInteger("Setup", "CastleMemberPriceRate", -1) < 0)
                WriteInteger("Setup", "CastleMemberPriceRate", M2Share.g_Config.nCastleMemberPriceRate);
            M2Share.g_Config.nCastleMemberPriceRate = ReadInteger("Setup", "CastleMemberPriceRate", M2Share.g_Config.nCastleMemberPriceRate);
            if (ReadInteger("Setup", "CastleGoldMax", -1) < 0)
                WriteInteger("Setup", "CastleGoldMax", M2Share.g_Config.nCastleGoldMax);
            M2Share.g_Config.nCastleGoldMax = ReadInteger("Setup", "CastleGoldMax", M2Share.g_Config.nCastleGoldMax);
            if (ReadInteger("Setup", "CastleOneDayGold", -1) < 0)
                WriteInteger("Setup", "CastleOneDayGold", M2Share.g_Config.nCastleOneDayGold);
            M2Share.g_Config.nCastleOneDayGold = ReadInteger("Setup", "CastleOneDayGold", M2Share.g_Config.nCastleOneDayGold);
            if (ReadString("Setup", "CastleName", "") == "")
                WriteString("Setup", "CastleName", M2Share.g_Config.sCastleName);
            M2Share.g_Config.sCastleName = ReadString("Setup", "CastleName", M2Share.g_Config.sCastleName);
            if (ReadString("Setup", "CastleHomeMap", "") == "")
                WriteString("Setup", "CastleHomeMap", M2Share.g_Config.sCastleHomeMap);
            M2Share.g_Config.sCastleHomeMap = ReadString("Setup", "CastleHomeMap", M2Share.g_Config.sCastleHomeMap);
            if (ReadInteger("Setup", "CastleHomeX", -1) < 0)
                WriteInteger("Setup", "CastleHomeX", M2Share.g_Config.nCastleHomeX);
            M2Share.g_Config.nCastleHomeX = ReadInteger("Setup", "CastleHomeX", M2Share.g_Config.nCastleHomeX);
            if (ReadInteger("Setup", "CastleHomeY", -1) < 0)
                WriteInteger("Setup", "CastleHomeY", M2Share.g_Config.nCastleHomeY);
            M2Share.g_Config.nCastleHomeY = ReadInteger("Setup", "CastleHomeY", M2Share.g_Config.nCastleHomeY);
            if (ReadInteger("Setup", "CastleWarRangeX", -1) < 0)
                WriteInteger("Setup", "CastleWarRangeX", M2Share.g_Config.nCastleWarRangeX);
            M2Share.g_Config.nCastleWarRangeX = ReadInteger("Setup", "CastleWarRangeX", M2Share.g_Config.nCastleWarRangeX);
            if (ReadInteger("Setup", "CastleWarRangeY", -1) < 0)
                WriteInteger("Setup", "CastleWarRangeY", M2Share.g_Config.nCastleWarRangeY);
            M2Share.g_Config.nCastleWarRangeY = ReadInteger("Setup", "CastleWarRangeY", M2Share.g_Config.nCastleWarRangeY);
            if (ReadInteger("Setup", "CastleTaxRate", -1) < 0)
                WriteInteger("Setup", "CastleTaxRate", M2Share.g_Config.nCastleTaxRate);
            M2Share.g_Config.nCastleTaxRate = ReadInteger("Setup", "CastleTaxRate", M2Share.g_Config.nCastleTaxRate);
            if (ReadInteger("Setup", "CastleGetAllNpcTax", -1) < 0)
                WriteBool("Setup", "CastleGetAllNpcTax", M2Share.g_Config.boGetAllNpcTax);
            M2Share.g_Config.boGetAllNpcTax = ReadBool("Setup", "CastleGetAllNpcTax", M2Share.g_Config.boGetAllNpcTax);
            nLoadInteger = ReadInteger("Setup", "GenMonRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "GenMonRate", M2Share.g_Config.nMonGenRate);
            else
                M2Share.g_Config.nMonGenRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "ProcessMonRandRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "ProcessMonRandRate", M2Share.g_Config.nProcessMonRandRate);
            else
                M2Share.g_Config.nProcessMonRandRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "ProcessMonLimitCount", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "ProcessMonLimitCount", M2Share.g_Config.nProcessMonLimitCount);
            else
                M2Share.g_Config.nProcessMonLimitCount = nLoadInteger;
            if (ReadInteger("Setup", "HumanMaxGold", -1) < 0)
                WriteInteger("Setup", "HumanMaxGold", M2Share.g_Config.nHumanMaxGold);
            M2Share.g_Config.nHumanMaxGold = ReadInteger("Setup", "HumanMaxGold", M2Share.g_Config.nHumanMaxGold);
            if (ReadInteger("Setup", "HumanTryModeMaxGold", -1) < 0)
                WriteInteger("Setup", "HumanTryModeMaxGold", M2Share.g_Config.nHumanTryModeMaxGold);
            M2Share.g_Config.nHumanTryModeMaxGold = ReadInteger("Setup", "HumanTryModeMaxGold", M2Share.g_Config.nHumanTryModeMaxGold);
            if (ReadInteger("Setup", "TryModeLevel", -1) < 0)
                WriteInteger("Setup", "TryModeLevel", M2Share.g_Config.nTryModeLevel);
            M2Share.g_Config.nTryModeLevel = ReadInteger("Setup", "TryModeLevel", M2Share.g_Config.nTryModeLevel);
            if (ReadInteger("Setup", "TryModeUseStorage", -1) < 0)
                WriteBool("Setup", "TryModeUseStorage", M2Share.g_Config.boTryModeUseStorage);
            M2Share.g_Config.boTryModeUseStorage = ReadBool("Setup", "TryModeUseStorage", M2Share.g_Config.boTryModeUseStorage);
            if (ReadInteger("Setup", "ShutRedMsgShowGMName", -1) < 0)
                WriteBool("Setup", "ShutRedMsgShowGMName", M2Share.g_Config.boShutRedMsgShowGMName);
            M2Share.g_Config.boShutRedMsgShowGMName = ReadBool("Setup", "ShutRedMsgShowGMName", M2Share.g_Config.boShutRedMsgShowGMName);
            if (ReadInteger("Setup", "ShowMakeItemMsg", -1) < 0)
                WriteBool("Setup", "ShowMakeItemMsg", M2Share.g_Config.boShowMakeItemMsg);
            M2Share.g_Config.boShowMakeItemMsg = ReadBool("Setup", "ShowMakeItemMsg", M2Share.g_Config.boShowMakeItemMsg);
            if (ReadInteger("Setup", "ShowGuildName", -1) < 0)
                WriteBool("Setup", "ShowGuildName", M2Share.g_Config.boShowGuildName);
            M2Share.g_Config.boShowGuildName = ReadBool("Setup", "ShowGuildName", M2Share.g_Config.boShowGuildName);
            if (ReadInteger("Setup", "ShowRankLevelName", -1) < 0)
                WriteBool("Setup", "ShowRankLevelName", M2Share.g_Config.boShowRankLevelName);
            M2Share.g_Config.boShowRankLevelName = ReadBool("Setup", "ShowRankLevelName", M2Share.g_Config.boShowRankLevelName);
            if (ReadInteger("Setup", "MonSayMsg", -1) < 0)
                WriteBool("Setup", "MonSayMsg", M2Share.g_Config.boMonSayMsg);
            M2Share.g_Config.boMonSayMsg = ReadBool("Setup", "MonSayMsg", M2Share.g_Config.boMonSayMsg);
            if (ReadInteger("Setup", "SayMsgMaxLen", -1) < 0)
                WriteInteger("Setup", "SayMsgMaxLen", M2Share.g_Config.nSayMsgMaxLen);
            M2Share.g_Config.nSayMsgMaxLen = ReadInteger("Setup", "SayMsgMaxLen", M2Share.g_Config.nSayMsgMaxLen);
            if (ReadInteger("Setup", "SayMsgTime", -1) < 0)
                WriteInteger("Setup", "SayMsgTime", M2Share.g_Config.dwSayMsgTime);
            M2Share.g_Config.dwSayMsgTime = ReadInteger("Setup", "SayMsgTime", M2Share.g_Config.dwSayMsgTime);
            if (ReadInteger("Setup", "SayMsgCount", -1) < 0)
                WriteInteger("Setup", "SayMsgCount", M2Share.g_Config.nSayMsgCount);
            M2Share.g_Config.nSayMsgCount = ReadInteger("Setup", "SayMsgCount", M2Share.g_Config.nSayMsgCount);
            if (ReadInteger("Setup", "DisableSayMsgTime", -1) < 0)
                WriteInteger("Setup", "DisableSayMsgTime", M2Share.g_Config.dwDisableSayMsgTime);
            M2Share.g_Config.dwDisableSayMsgTime = ReadInteger("Setup", "DisableSayMsgTime", M2Share.g_Config.dwDisableSayMsgTime);
            if (ReadInteger("Setup", "SayRedMsgMaxLen", -1) < 0)
                WriteInteger("Setup", "SayRedMsgMaxLen", M2Share.g_Config.nSayRedMsgMaxLen);
            M2Share.g_Config.nSayRedMsgMaxLen = ReadInteger("Setup", "SayRedMsgMaxLen", M2Share.g_Config.nSayRedMsgMaxLen);
            if (ReadInteger("Setup", "CanShoutMsgLevel", -1) < 0)
                WriteInteger("Setup", "CanShoutMsgLevel", M2Share.g_Config.nCanShoutMsgLevel);
            M2Share.g_Config.nCanShoutMsgLevel = ReadInteger("Setup", "CanShoutMsgLevel", M2Share.g_Config.nCanShoutMsgLevel);
            if (ReadInteger("Setup", "StartPermission", -1) < 0)
                WriteInteger("Setup", "StartPermission", M2Share.g_Config.nStartPermission);
            M2Share.g_Config.nStartPermission = ReadInteger("Setup", "StartPermission", M2Share.g_Config.nStartPermission);
            if (ReadInteger("Setup", "SendRefMsgRange", -1) < 0)
                WriteInteger("Setup", "SendRefMsgRange", M2Share.g_Config.nSendRefMsgRange);
            M2Share.g_Config.nSendRefMsgRange = (byte)ReadInteger("Setup", "SendRefMsgRange", M2Share.g_Config.nSendRefMsgRange);
            if (ReadInteger("Setup", "DecLampDura", -1) < 0)
                WriteBool("Setup", "DecLampDura", M2Share.g_Config.boDecLampDura);
            M2Share.g_Config.boDecLampDura = ReadBool("Setup", "DecLampDura", M2Share.g_Config.boDecLampDura);
            if (ReadInteger("Setup", "HungerSystem", -1) < 0)
                WriteBool("Setup", "HungerSystem", M2Share.g_Config.boHungerSystem);
            M2Share.g_Config.boHungerSystem = ReadBool("Setup", "HungerSystem", M2Share.g_Config.boHungerSystem);
            if (ReadInteger("Setup", "HungerDecHP", -1) < 0)
                WriteBool("Setup", "HungerDecHP", M2Share.g_Config.boHungerDecHP);
            M2Share.g_Config.boHungerDecHP = ReadBool("Setup", "HungerDecHP", M2Share.g_Config.boHungerDecHP);
            if (ReadInteger("Setup", "HungerDecPower", -1) < 0)
                WriteBool("Setup", "HungerDecPower", M2Share.g_Config.boHungerDecPower);
            M2Share.g_Config.boHungerDecPower = ReadBool("Setup", "HungerDecPower", M2Share.g_Config.boHungerDecPower);
            if (ReadInteger("Setup", "DiableHumanRun", -1) < 0)
                WriteBool("Setup", "DiableHumanRun", M2Share.g_Config.boDiableHumanRun);
            M2Share.g_Config.boDiableHumanRun = ReadBool("Setup", "DiableHumanRun", M2Share.g_Config.boDiableHumanRun);
            if (ReadInteger("Setup", "RunHuman", -1) < 0)
                WriteBool("Setup", "RunHuman", M2Share.g_Config.boRunHuman);
            M2Share.g_Config.boRunHuman = ReadBool("Setup", "RunHuman", M2Share.g_Config.boRunHuman);
            if (ReadInteger("Setup", "RunMon", -1) < 0)
                WriteBool("Setup", "RunMon", M2Share.g_Config.boRunMon);
            M2Share.g_Config.boRunMon = ReadBool("Setup", "RunMon", M2Share.g_Config.boRunMon);
            if (ReadInteger("Setup", "RunNpc", -1) < 0)
                WriteBool("Setup", "RunNpc", M2Share.g_Config.boRunNpc);
            M2Share.g_Config.boRunNpc = ReadBool("Setup", "RunNpc", M2Share.g_Config.boRunNpc);
            nLoadInteger = ReadInteger("Setup", "RunGuard", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "RunGuard", M2Share.g_Config.boRunGuard);
            else
                M2Share.g_Config.boRunGuard = nLoadInteger == 1;
            if (ReadInteger("Setup", "WarDisableHumanRun", -1) < 0)
                WriteBool("Setup", "WarDisableHumanRun", M2Share.g_Config.boWarDisHumRun);
            M2Share.g_Config.boWarDisHumRun = ReadBool("Setup", "WarDisableHumanRun", M2Share.g_Config.boWarDisHumRun);
            if (ReadInteger("Setup", "GMRunAll", -1) < 0)
            {
                WriteBool("Setup", "GMRunAll", M2Share.g_Config.boGMRunAll);
            }
            M2Share.g_Config.boGMRunAll = ReadBool("Setup", "GMRunAll", M2Share.g_Config.boGMRunAll);
            if (ReadInteger("Setup", "SkeletonCount", -1) < 0)
            {
                WriteInteger("Setup", "SkeletonCount", M2Share.g_Config.nSkeletonCount);
            }
            M2Share.g_Config.nSkeletonCount = ReadInteger("Setup", "SkeletonCount", M2Share.g_Config.nSkeletonCount);
            for (var i = 0; i < M2Share.g_Config.SkeletonArray.Length; i++)
            {
                if (ReadInteger("Setup", "SkeletonHumLevel" + i, -1) < 0)
                {
                    WriteInteger("Setup", "SkeletonHumLevel" + i, M2Share.g_Config.SkeletonArray[i].nHumLevel);
                }
                M2Share.g_Config.SkeletonArray[i].nHumLevel = ReadInteger("Setup", "SkeletonHumLevel" + i, M2Share.g_Config.SkeletonArray[i].nHumLevel);
                if (ReadString("Names", "Skeleton" + i, "") == "")
                {
                    WriteString("Names", "Skeleton" + i, M2Share.g_Config.SkeletonArray[i].sMonName);
                }
                M2Share.g_Config.SkeletonArray[i].sMonName = ReadString("Names", "Skeleton" + i, M2Share.g_Config.SkeletonArray[i].sMonName);
                if (ReadInteger("Setup", "SkeletonCount" + i, -1) < 0)
                {
                    WriteInteger("Setup", "SkeletonCount" + i, M2Share.g_Config.SkeletonArray[i].nCount);
                }
                M2Share.g_Config.SkeletonArray[i].nCount = ReadInteger("Setup", "SkeletonCount" + i, M2Share.g_Config.SkeletonArray[i].nCount);
                if (ReadInteger("Setup", "SkeletonLevel" + i, -1) < 0)
                {
                    WriteInteger("Setup", "SkeletonLevel" + i, M2Share.g_Config.SkeletonArray[i].nLevel);
                }
                M2Share.g_Config.SkeletonArray[i].nLevel = ReadInteger("Setup", "SkeletonLevel" + i, M2Share.g_Config.SkeletonArray[i].nLevel);
            }
            if (ReadInteger("Setup", "DragonCount", -1) < 0)
            {
                WriteInteger("Setup", "DragonCount", M2Share.g_Config.nDragonCount);
            }
            M2Share.g_Config.nDragonCount = ReadInteger("Setup", "DragonCount", M2Share.g_Config.nDragonCount);
            for (var i = 0; i < M2Share.g_Config.DragonArray.Length; i++)
            {
                if (ReadInteger("Setup", "DragonHumLevel" + i, -1) < 0)
                {
                    WriteInteger("Setup", "DragonHumLevel" + i, M2Share.g_Config.DragonArray[i].nHumLevel);
                }
                M2Share.g_Config.DragonArray[i].nHumLevel = ReadInteger("Setup", "DragonHumLevel" + i, M2Share.g_Config.DragonArray[i].nHumLevel);
                if (ReadString("Names", "Dragon" + i, "") == "")
                {
                    WriteString("Names", "Dragon" + i, M2Share.g_Config.DragonArray[i].sMonName);
                }
                M2Share.g_Config.DragonArray[i].sMonName = ReadString("Names", "Dragon" + i, M2Share.g_Config.DragonArray[i].sMonName);
                if (ReadInteger("Setup", "DragonCount" + i, -1) < 0)
                {
                    WriteInteger("Setup", "DragonCount" + i, M2Share.g_Config.DragonArray[i].nCount);
                }
                M2Share.g_Config.DragonArray[i].nCount = ReadInteger("Setup", "DragonCount" + i, M2Share.g_Config.DragonArray[i].nCount);
                if (ReadInteger("Setup", "DragonLevel" + i, -1) < 0)
                {
                    WriteInteger("Setup", "DragonLevel" + i, M2Share.g_Config.DragonArray[i].nLevel);
                }
                M2Share.g_Config.DragonArray[i].nLevel = ReadInteger("Setup", "DragonLevel" + i, M2Share.g_Config.DragonArray[i].nLevel);
            }
            if (ReadInteger("Setup", "TryDealTime", -1) < 0)
                WriteInteger("Setup", "TryDealTime", M2Share.g_Config.dwTryDealTime);
            M2Share.g_Config.dwTryDealTime = ReadInteger("Setup", "TryDealTime", M2Share.g_Config.dwTryDealTime);
            if (ReadInteger("Setup", "DealOKTime", -1) < 0)
                WriteInteger("Setup", "DealOKTime", M2Share.g_Config.dwDealOKTime);
            M2Share.g_Config.dwDealOKTime = ReadInteger("Setup", "DealOKTime", M2Share.g_Config.dwDealOKTime);
            if (ReadInteger("Setup", "CanNotGetBackDeal", -1) < 0)
                WriteBool("Setup", "CanNotGetBackDeal", M2Share.g_Config.boCanNotGetBackDeal);
            M2Share.g_Config.boCanNotGetBackDeal = ReadBool("Setup", "CanNotGetBackDeal", M2Share.g_Config.boCanNotGetBackDeal);
            if (ReadInteger("Setup", "DisableDeal", -1) < 0)
                WriteBool("Setup", "DisableDeal", M2Share.g_Config.boDisableDeal);
            M2Share.g_Config.boDisableDeal = ReadBool("Setup", "DisableDeal", M2Share.g_Config.boDisableDeal);
            if (ReadInteger("Setup", "MasterOKLevel", -1) < 0)
                WriteInteger("Setup", "MasterOKLevel", M2Share.g_Config.nMasterOKLevel);
            M2Share.g_Config.nMasterOKLevel = ReadInteger("Setup", "MasterOKLevel", M2Share.g_Config.nMasterOKLevel);
            if (ReadInteger("Setup", "MasterOKCreditPoint", -1) < 0)
                WriteInteger("Setup", "MasterOKCreditPoint", M2Share.g_Config.nMasterOKCreditPoint);
            M2Share.g_Config.nMasterOKCreditPoint = ReadInteger("Setup", "MasterOKCreditPoint", M2Share.g_Config.nMasterOKCreditPoint);
            if (ReadInteger("Setup", "MasterOKBonusPoint", -1) < 0)
                WriteInteger("Setup", "MasterOKBonusPoint", M2Share.g_Config.nMasterOKBonusPoint);
            M2Share.g_Config.nMasterOKBonusPoint = ReadInteger("Setup", "MasterOKBonusPoint", M2Share.g_Config.nMasterOKBonusPoint);
            if (ReadInteger("Setup", "PKProtect", -1) < 0)
                WriteBool("Setup", "PKProtect", M2Share.g_Config.boPKLevelProtect);
            M2Share.g_Config.boPKLevelProtect = ReadBool("Setup", "PKProtect", M2Share.g_Config.boPKLevelProtect);
            if (ReadInteger("Setup", "PKProtectLevel", -1) < 0)
                WriteInteger("Setup", "PKProtectLevel", M2Share.g_Config.nPKProtectLevel);
            M2Share.g_Config.nPKProtectLevel = ReadInteger("Setup", "PKProtectLevel", M2Share.g_Config.nPKProtectLevel);
            if (ReadInteger("Setup", "RedPKProtectLevel", -1) < 0)
                WriteInteger("Setup", "RedPKProtectLevel", M2Share.g_Config.nRedPKProtectLevel);
            M2Share.g_Config.nRedPKProtectLevel = ReadInteger("Setup", "RedPKProtectLevel", M2Share.g_Config.nRedPKProtectLevel);
            if (ReadInteger("Setup", "ItemPowerRate", -1) < 0)
                WriteInteger("Setup", "ItemPowerRate", M2Share.g_Config.nItemPowerRate);
            M2Share.g_Config.nItemPowerRate = ReadInteger("Setup", "ItemPowerRate", M2Share.g_Config.nItemPowerRate);
            if (ReadInteger("Setup", "ItemExpRate", -1) < 0)
                WriteInteger("Setup", "ItemExpRate", M2Share.g_Config.nItemExpRate);
            M2Share.g_Config.nItemExpRate = ReadInteger("Setup", "ItemExpRate", M2Share.g_Config.nItemExpRate);
            if (ReadInteger("Setup", "ScriptGotoCountLimit", -1) < 0)
                WriteInteger("Setup", "ScriptGotoCountLimit", M2Share.g_Config.nScriptGotoCountLimit);
            M2Share.g_Config.nScriptGotoCountLimit = ReadInteger("Setup", "ScriptGotoCountLimit", M2Share.g_Config.nScriptGotoCountLimit);
            if (ReadInteger("Setup", "HearMsgFColor", -1) < 0)
                WriteInteger("Setup", "HearMsgFColor", M2Share.g_Config.btHearMsgFColor);
            M2Share.g_Config.btHearMsgFColor = Read<byte>("Setup", "HearMsgFColor", M2Share.g_Config.btHearMsgFColor);
            if (ReadInteger("Setup", "HearMsgBColor", -1) < 0)
                WriteInteger("Setup", "HearMsgBColor", M2Share.g_Config.btHearMsgBColor);
            M2Share.g_Config.btHearMsgBColor = Read<byte>("Setup", "HearMsgBColor", M2Share.g_Config.btHearMsgBColor);
            if (ReadInteger("Setup", "WhisperMsgFColor", -1) < 0)
                WriteInteger("Setup", "WhisperMsgFColor", M2Share.g_Config.btWhisperMsgFColor);
            M2Share.g_Config.btWhisperMsgFColor = Read<byte>("Setup", "WhisperMsgFColor", M2Share.g_Config.btWhisperMsgFColor);
            if (ReadInteger("Setup", "WhisperMsgBColor", -1) < 0)
                WriteInteger("Setup", "WhisperMsgBColor", M2Share.g_Config.btWhisperMsgBColor);
            M2Share.g_Config.btWhisperMsgBColor = Read<byte>("Setup", "WhisperMsgBColor", M2Share.g_Config.btWhisperMsgBColor);
            if (ReadInteger("Setup", "GMWhisperMsgFColor", -1) < 0)
                WriteInteger("Setup", "GMWhisperMsgFColor", M2Share.g_Config.btGMWhisperMsgFColor);
            M2Share.g_Config.btGMWhisperMsgFColor = Read<byte>("Setup", "GMWhisperMsgFColor", M2Share.g_Config.btGMWhisperMsgFColor);
            if (ReadInteger("Setup", "GMWhisperMsgBColor", -1) < 0)
                WriteInteger("Setup", "GMWhisperMsgBColor", M2Share.g_Config.btGMWhisperMsgBColor);
            M2Share.g_Config.btGMWhisperMsgBColor = Read<byte>("Setup", "GMWhisperMsgBColor", M2Share.g_Config.btGMWhisperMsgBColor);
            if (ReadInteger("Setup", "CryMsgFColor", -1) < 0)
                WriteInteger("Setup", "CryMsgFColor", M2Share.g_Config.btCryMsgFColor);
            M2Share.g_Config.btCryMsgFColor = Read<byte>("Setup", "CryMsgFColor", M2Share.g_Config.btCryMsgFColor);
            if (ReadInteger("Setup", "CryMsgBColor", -1) < 0)
                WriteInteger("Setup", "CryMsgBColor", M2Share.g_Config.btCryMsgBColor);
            M2Share.g_Config.btCryMsgBColor = Read<byte>("Setup", "CryMsgBColor", M2Share.g_Config.btCryMsgBColor);
            if (ReadInteger("Setup", "GreenMsgFColor", -1) < 0)
                WriteInteger("Setup", "GreenMsgFColor", M2Share.g_Config.btGreenMsgFColor);
            M2Share.g_Config.btGreenMsgFColor = Read<byte>("Setup", "GreenMsgFColor", M2Share.g_Config.btGreenMsgFColor);
            if (ReadInteger("Setup", "GreenMsgBColor", -1) < 0)
                WriteInteger("Setup", "GreenMsgBColor", M2Share.g_Config.btGreenMsgBColor);
            M2Share.g_Config.btGreenMsgBColor = Read<byte>("Setup", "GreenMsgBColor", M2Share.g_Config.btGreenMsgBColor);
            if (ReadInteger("Setup", "BlueMsgFColor", -1) < 0)
                WriteInteger("Setup", "BlueMsgFColor", M2Share.g_Config.btBlueMsgFColor);
            M2Share.g_Config.btBlueMsgFColor = Read<byte>("Setup", "BlueMsgFColor", M2Share.g_Config.btBlueMsgFColor);
            if (ReadInteger("Setup", "BlueMsgBColor", -1) < 0)
                WriteInteger("Setup", "BlueMsgBColor", M2Share.g_Config.btBlueMsgBColor);
            M2Share.g_Config.btBlueMsgBColor = Read<byte>("Setup", "BlueMsgBColor", M2Share.g_Config.btBlueMsgBColor);
            if (ReadInteger("Setup", "RedMsgFColor", -1) < 0)
                WriteInteger("Setup", "RedMsgFColor", M2Share.g_Config.btRedMsgFColor);
            M2Share.g_Config.btRedMsgFColor = Read<byte>("Setup", "RedMsgFColor", M2Share.g_Config.btRedMsgFColor);
            if (ReadInteger("Setup", "RedMsgBColor", -1) < 0)
                WriteInteger("Setup", "RedMsgBColor", M2Share.g_Config.btRedMsgBColor);
            M2Share.g_Config.btRedMsgBColor = Read<byte>("Setup", "RedMsgBColor", M2Share.g_Config.btRedMsgBColor);
            if (ReadInteger("Setup", "GuildMsgFColor", -1) < 0)
                WriteInteger("Setup", "GuildMsgFColor", M2Share.g_Config.btGuildMsgFColor);
            M2Share.g_Config.btGuildMsgFColor = Read<byte>("Setup", "GuildMsgFColor", M2Share.g_Config.btGuildMsgFColor);
            if (ReadInteger("Setup", "GuildMsgBColor", -1) < 0)
                WriteInteger("Setup", "GuildMsgBColor", M2Share.g_Config.btGuildMsgBColor);
            M2Share.g_Config.btGuildMsgBColor = Read<byte>("Setup", "GuildMsgBColor", M2Share.g_Config.btGuildMsgBColor);
            if (ReadInteger("Setup", "GroupMsgFColor", -1) < 0)
                WriteInteger("Setup", "GroupMsgFColor", M2Share.g_Config.btGroupMsgFColor);
            M2Share.g_Config.btGroupMsgFColor = Read<byte>("Setup", "GroupMsgFColor", M2Share.g_Config.btGroupMsgFColor);
            if (ReadInteger("Setup", "GroupMsgBColor", -1) < 0)
                WriteInteger("Setup", "GroupMsgBColor", M2Share.g_Config.btGroupMsgBColor);
            M2Share.g_Config.btGroupMsgBColor = Read<byte>("Setup", "GroupMsgBColor", M2Share.g_Config.btGroupMsgBColor);
            if (ReadInteger("Setup", "CustMsgFColor", -1) < 0)
                WriteInteger("Setup", "CustMsgFColor", M2Share.g_Config.btCustMsgFColor);
            M2Share.g_Config.btCustMsgFColor = Read<byte>("Setup", "CustMsgFColor", M2Share.g_Config.btCustMsgFColor);
            if (ReadInteger("Setup", "CustMsgBColor", -1) < 0)
                WriteInteger("Setup", "CustMsgBColor", M2Share.g_Config.btCustMsgBColor);
            M2Share.g_Config.btCustMsgBColor = Read<byte>("Setup", "CustMsgBColor", M2Share.g_Config.btCustMsgBColor);
            if (ReadInteger("Setup", "MonRandomAddValue", -1) < 0)
                WriteInteger("Setup", "MonRandomAddValue", M2Share.g_Config.nMonRandomAddValue);
            M2Share.g_Config.nMonRandomAddValue = ReadInteger("Setup", "MonRandomAddValue", M2Share.g_Config.nMonRandomAddValue);
            if (ReadInteger("Setup", "MakeRandomAddValue", -1) < 0)
                WriteInteger("Setup", "MakeRandomAddValue", M2Share.g_Config.nMakeRandomAddValue);
            M2Share.g_Config.nMakeRandomAddValue = ReadInteger("Setup", "MakeRandomAddValue", M2Share.g_Config.nMakeRandomAddValue);
            if (ReadInteger("Setup", "WeaponDCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "WeaponDCAddValueMaxLimit", M2Share.g_Config.nWeaponDCAddValueMaxLimit);
            M2Share.g_Config.nWeaponDCAddValueMaxLimit = ReadInteger("Setup", "WeaponDCAddValueMaxLimit", M2Share.g_Config.nWeaponDCAddValueMaxLimit);
            if (ReadInteger("Setup", "WeaponDCAddValueRate", -1) < 0)
                WriteInteger("Setup", "WeaponDCAddValueRate", M2Share.g_Config.nWeaponDCAddValueRate);
            M2Share.g_Config.nWeaponDCAddValueRate = ReadInteger("Setup", "WeaponDCAddValueRate", M2Share.g_Config.nWeaponDCAddValueRate);
            if (ReadInteger("Setup", "WeaponMCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "WeaponMCAddValueMaxLimit", M2Share.g_Config.nWeaponMCAddValueMaxLimit);
            M2Share.g_Config.nWeaponMCAddValueMaxLimit = ReadInteger("Setup", "WeaponMCAddValueMaxLimit", M2Share.g_Config.nWeaponMCAddValueMaxLimit);
            if (ReadInteger("Setup", "WeaponMCAddValueRate", -1) < 0)
                WriteInteger("Setup", "WeaponMCAddValueRate", M2Share.g_Config.nWeaponMCAddValueRate);
            M2Share.g_Config.nWeaponMCAddValueRate = ReadInteger("Setup", "WeaponMCAddValueRate", M2Share.g_Config.nWeaponMCAddValueRate);
            if (ReadInteger("Setup", "WeaponSCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "WeaponSCAddValueMaxLimit", M2Share.g_Config.nWeaponSCAddValueMaxLimit);
            M2Share.g_Config.nWeaponSCAddValueMaxLimit = ReadInteger("Setup", "WeaponSCAddValueMaxLimit", M2Share.g_Config.nWeaponSCAddValueMaxLimit);
            if (ReadInteger("Setup", "WeaponSCAddValueRate", -1) < 0)
                WriteInteger("Setup", "WeaponSCAddValueRate", M2Share.g_Config.nWeaponSCAddValueRate);
            M2Share.g_Config.nWeaponSCAddValueRate = ReadInteger("Setup", "WeaponSCAddValueRate", M2Share.g_Config.nWeaponSCAddValueRate);
            if (ReadInteger("Setup", "DressDCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "DressDCAddValueMaxLimit", M2Share.g_Config.nDressDCAddValueMaxLimit);
            M2Share.g_Config.nDressDCAddValueMaxLimit = ReadInteger("Setup", "DressDCAddValueMaxLimit", M2Share.g_Config.nDressDCAddValueMaxLimit);
            if (ReadInteger("Setup", "DressDCAddValueRate", -1) < 0)
                WriteInteger("Setup", "DressDCAddValueRate", M2Share.g_Config.nDressDCAddValueRate);
            M2Share.g_Config.nDressDCAddValueRate = ReadInteger("Setup", "DressDCAddValueRate", M2Share.g_Config.nDressDCAddValueRate);
            if (ReadInteger("Setup", "DressDCAddRate", -1) < 0)
                WriteInteger("Setup", "DressDCAddRate", M2Share.g_Config.nDressDCAddRate);
            M2Share.g_Config.nDressDCAddRate = ReadInteger("Setup", "DressDCAddRate", M2Share.g_Config.nDressDCAddRate);
            if (ReadInteger("Setup", "DressMCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "DressMCAddValueMaxLimit", M2Share.g_Config.nDressMCAddValueMaxLimit);
            M2Share.g_Config.nDressMCAddValueMaxLimit = ReadInteger("Setup", "DressMCAddValueMaxLimit", M2Share.g_Config.nDressMCAddValueMaxLimit);
            if (ReadInteger("Setup", "DressMCAddValueRate", -1) < 0)
                WriteInteger("Setup", "DressMCAddValueRate", M2Share.g_Config.nDressMCAddValueRate);
            M2Share.g_Config.nDressMCAddValueRate = ReadInteger("Setup", "DressMCAddValueRate", M2Share.g_Config.nDressMCAddValueRate);
            if (ReadInteger("Setup", "DressMCAddRate", -1) < 0)
                WriteInteger("Setup", "DressMCAddRate", M2Share.g_Config.nDressMCAddRate);
            M2Share.g_Config.nDressMCAddRate = ReadInteger("Setup", "DressMCAddRate", M2Share.g_Config.nDressMCAddRate);
            if (ReadInteger("Setup", "DressSCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "DressSCAddValueMaxLimit", M2Share.g_Config.nDressSCAddValueMaxLimit);
            M2Share.g_Config.nDressSCAddValueMaxLimit = ReadInteger("Setup", "DressSCAddValueMaxLimit", M2Share.g_Config.nDressSCAddValueMaxLimit);
            if (ReadInteger("Setup", "DressSCAddValueRate", -1) < 0)
                WriteInteger("Setup", "DressSCAddValueRate", M2Share.g_Config.nDressSCAddValueRate);
            M2Share.g_Config.nDressSCAddValueRate = ReadInteger("Setup", "DressSCAddValueRate", M2Share.g_Config.nDressSCAddValueRate);
            if (ReadInteger("Setup", "DressSCAddRate", -1) < 0)
                WriteInteger("Setup", "DressSCAddRate", M2Share.g_Config.nDressSCAddRate);
            M2Share.g_Config.nDressSCAddRate = ReadInteger("Setup", "DressSCAddRate", M2Share.g_Config.nDressSCAddRate);
            if (ReadInteger("Setup", "NeckLace19DCAddValueMaxLimit", -1) < 0)
            {
                WriteInteger("Setup", "NeckLace19DCAddValueMaxLimit", M2Share.g_Config.nNeckLace19DCAddValueMaxLimit);
            }
            M2Share.g_Config.nNeckLace19DCAddValueMaxLimit = ReadInteger("Setup", "NeckLace19DCAddValueMaxLimit", M2Share.g_Config.nNeckLace19DCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace19DCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19DCAddValueRate", M2Share.g_Config.nNeckLace19DCAddValueRate);
            M2Share.g_Config.nNeckLace19DCAddValueRate = ReadInteger("Setup", "NeckLace19DCAddValueRate", M2Share.g_Config.nNeckLace19DCAddValueRate);
            if (ReadInteger("Setup", "NeckLace19DCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19DCAddRate", M2Share.g_Config.nNeckLace19DCAddRate);
            M2Share.g_Config.nNeckLace19DCAddRate = ReadInteger("Setup", "NeckLace19DCAddRate", M2Share.g_Config.nNeckLace19DCAddRate);
            if (ReadInteger("Setup", "NeckLace19MCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "NeckLace19MCAddValueMaxLimit", M2Share.g_Config.nNeckLace19MCAddValueMaxLimit);
            M2Share.g_Config.nNeckLace19MCAddValueMaxLimit = ReadInteger("Setup", "NeckLace19MCAddValueMaxLimit", M2Share.g_Config.nNeckLace19MCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace19MCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19MCAddValueRate", M2Share.g_Config.nNeckLace19MCAddValueRate);
            M2Share.g_Config.nNeckLace19MCAddValueRate = ReadInteger("Setup", "NeckLace19MCAddValueRate", M2Share.g_Config.nNeckLace19MCAddValueRate);
            if (ReadInteger("Setup", "NeckLace19MCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19MCAddRate", M2Share.g_Config.nNeckLace19MCAddRate);
            M2Share.g_Config.nNeckLace19MCAddRate = ReadInteger("Setup", "NeckLace19MCAddRate", M2Share.g_Config.nNeckLace19MCAddRate);
            if (ReadInteger("Setup", "NeckLace19SCAddValueMaxLimit", -1) < 0)
            {
                WriteInteger("Setup", "NeckLace19SCAddValueMaxLimit", M2Share.g_Config.nNeckLace19SCAddValueMaxLimit);
            }
            M2Share.g_Config.nNeckLace19SCAddValueMaxLimit = ReadInteger("Setup", "NeckLace19SCAddValueMaxLimit", M2Share.g_Config.nNeckLace19SCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace19SCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19SCAddValueRate", M2Share.g_Config.nNeckLace19SCAddValueRate);
            M2Share.g_Config.nNeckLace19SCAddValueRate = ReadInteger("Setup", "NeckLace19SCAddValueRate", M2Share.g_Config.nNeckLace19SCAddValueRate);
            if (ReadInteger("Setup", "NeckLace19SCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19SCAddRate", M2Share.g_Config.nNeckLace19SCAddRate);
            M2Share.g_Config.nNeckLace19SCAddRate = ReadInteger("Setup", "NeckLace19SCAddRate", M2Share.g_Config.nNeckLace19SCAddRate);
            if (ReadInteger("Setup", "NeckLace202124DCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "NeckLace202124DCAddValueMaxLimit", M2Share.g_Config.nNeckLace202124DCAddValueMaxLimit);
            M2Share.g_Config.nNeckLace202124DCAddValueMaxLimit = ReadInteger("Setup", "NeckLace202124DCAddValueMaxLimit", M2Share.g_Config.nNeckLace202124DCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace202124DCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124DCAddValueRate", M2Share.g_Config.nNeckLace202124DCAddValueRate);
            M2Share.g_Config.nNeckLace202124DCAddValueRate = ReadInteger("Setup", "NeckLace202124DCAddValueRate", M2Share.g_Config.nNeckLace202124DCAddValueRate);
            if (ReadInteger("Setup", "NeckLace202124DCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124DCAddRate", M2Share.g_Config.nNeckLace202124DCAddRate);
            M2Share.g_Config.nNeckLace202124DCAddRate = ReadInteger("Setup", "NeckLace202124DCAddRate", M2Share.g_Config.nNeckLace202124DCAddRate);
            if (ReadInteger("Setup", "NeckLace202124MCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "NeckLace202124MCAddValueMaxLimit", M2Share.g_Config.nNeckLace202124MCAddValueMaxLimit);
            M2Share.g_Config.nNeckLace202124MCAddValueMaxLimit = ReadInteger("Setup", "NeckLace202124MCAddValueMaxLimit", M2Share.g_Config.nNeckLace202124MCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace202124MCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124MCAddValueRate", M2Share.g_Config.nNeckLace202124MCAddValueRate);
            M2Share.g_Config.nNeckLace202124MCAddValueRate = ReadInteger("Setup", "NeckLace202124MCAddValueRate", M2Share.g_Config.nNeckLace202124MCAddValueRate);
            if (ReadInteger("Setup", "NeckLace202124MCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124MCAddRate", M2Share.g_Config.nNeckLace202124MCAddRate);
            M2Share.g_Config.nNeckLace202124MCAddRate = ReadInteger("Setup", "NeckLace202124MCAddRate", M2Share.g_Config.nNeckLace202124MCAddRate);
            if (ReadInteger("Setup", "NeckLace202124SCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "NeckLace202124SCAddValueMaxLimit", M2Share.g_Config.nNeckLace202124SCAddValueMaxLimit);
            M2Share.g_Config.nNeckLace202124SCAddValueMaxLimit = ReadInteger("Setup", "NeckLace202124SCAddValueMaxLimit", M2Share.g_Config.nNeckLace202124SCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace202124SCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124SCAddValueRate", M2Share.g_Config.nNeckLace202124SCAddValueRate);
            M2Share.g_Config.nNeckLace202124SCAddValueRate = ReadInteger("Setup", "NeckLace202124SCAddValueRate", M2Share.g_Config.nNeckLace202124SCAddValueRate);
            if (ReadInteger("Setup", "NeckLace202124SCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124SCAddRate", M2Share.g_Config.nNeckLace202124SCAddRate);
            M2Share.g_Config.nNeckLace202124SCAddRate = ReadInteger("Setup", "NeckLace202124SCAddRate", M2Share.g_Config.nNeckLace202124SCAddRate);
            if (ReadInteger("Setup", "ArmRing26DCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "ArmRing26DCAddValueMaxLimit", M2Share.g_Config.nArmRing26DCAddValueMaxLimit);
            M2Share.g_Config.nArmRing26DCAddValueMaxLimit = ReadInteger("Setup", "ArmRing26DCAddValueMaxLimit", M2Share.g_Config.nArmRing26DCAddValueMaxLimit);
            if (ReadInteger("Setup", "ArmRing26DCAddValueRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26DCAddValueRate", M2Share.g_Config.nArmRing26DCAddValueRate);
            M2Share.g_Config.nArmRing26DCAddValueRate = ReadInteger("Setup", "ArmRing26DCAddValueRate", M2Share.g_Config.nArmRing26DCAddValueRate);
            if (ReadInteger("Setup", "ArmRing26DCAddRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26DCAddRate", M2Share.g_Config.nArmRing26DCAddRate);
            M2Share.g_Config.nArmRing26DCAddRate = ReadInteger("Setup", "ArmRing26DCAddRate", M2Share.g_Config.nArmRing26DCAddRate);
            if (ReadInteger("Setup", "ArmRing26MCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "ArmRing26MCAddValueMaxLimit", M2Share.g_Config.nArmRing26MCAddValueMaxLimit);
            M2Share.g_Config.nArmRing26MCAddValueMaxLimit = ReadInteger("Setup", "ArmRing26MCAddValueMaxLimit", M2Share.g_Config.nArmRing26MCAddValueMaxLimit);
            if (ReadInteger("Setup", "ArmRing26MCAddValueRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26MCAddValueRate", M2Share.g_Config.nArmRing26MCAddValueRate);
            M2Share.g_Config.nArmRing26MCAddValueRate = ReadInteger("Setup", "ArmRing26MCAddValueRate", M2Share.g_Config.nArmRing26MCAddValueRate);
            if (ReadInteger("Setup", "ArmRing26MCAddRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26MCAddRate", M2Share.g_Config.nArmRing26MCAddRate);
            M2Share.g_Config.nArmRing26MCAddRate = ReadInteger("Setup", "ArmRing26MCAddRate", M2Share.g_Config.nArmRing26MCAddRate);
            if (ReadInteger("Setup", "ArmRing26SCAddValueMaxLimit", -1) < 0)
            {
                WriteInteger("Setup", "ArmRing26SCAddValueMaxLimit", M2Share.g_Config.nArmRing26SCAddValueMaxLimit);
            }
            M2Share.g_Config.nArmRing26SCAddValueMaxLimit = ReadInteger("Setup", "ArmRing26SCAddValueMaxLimit", M2Share.g_Config.nArmRing26SCAddValueMaxLimit);
            if (ReadInteger("Setup", "ArmRing26SCAddValueRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26SCAddValueRate", M2Share.g_Config.nArmRing26SCAddValueRate);
            M2Share.g_Config.nArmRing26SCAddValueRate = ReadInteger("Setup", "ArmRing26SCAddValueRate", M2Share.g_Config.nArmRing26SCAddValueRate);
            if (ReadInteger("Setup", "ArmRing26SCAddRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26SCAddRate", M2Share.g_Config.nArmRing26SCAddRate);
            M2Share.g_Config.nArmRing26SCAddRate = ReadInteger("Setup", "ArmRing26SCAddRate", M2Share.g_Config.nArmRing26SCAddRate);
            if (ReadInteger("Setup", "Ring22DCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring22DCAddValueMaxLimit", M2Share.g_Config.nRing22DCAddValueMaxLimit);
            M2Share.g_Config.nRing22DCAddValueMaxLimit = ReadInteger("Setup", "Ring22DCAddValueMaxLimit", M2Share.g_Config.nRing22DCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring22DCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring22DCAddValueRate", M2Share.g_Config.nRing22DCAddValueRate);
            M2Share.g_Config.nRing22DCAddValueRate = ReadInteger("Setup", "Ring22DCAddValueRate", M2Share.g_Config.nRing22DCAddValueRate);
            if (ReadInteger("Setup", "Ring22DCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring22DCAddRate", M2Share.g_Config.nRing22DCAddRate);
            M2Share.g_Config.nRing22DCAddRate = ReadInteger("Setup", "Ring22DCAddRate", M2Share.g_Config.nRing22DCAddRate);
            if (ReadInteger("Setup", "Ring22MCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring22MCAddValueMaxLimit", M2Share.g_Config.nRing22MCAddValueMaxLimit);
            M2Share.g_Config.nRing22MCAddValueMaxLimit = ReadInteger("Setup", "Ring22MCAddValueMaxLimit", M2Share.g_Config.nRing22MCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring22MCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring22MCAddValueRate", M2Share.g_Config.nRing22MCAddValueRate);
            M2Share.g_Config.nRing22MCAddValueRate = ReadInteger("Setup", "Ring22MCAddValueRate", M2Share.g_Config.nRing22MCAddValueRate);
            if (ReadInteger("Setup", "Ring22MCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring22MCAddRate", M2Share.g_Config.nRing22MCAddRate);
            M2Share.g_Config.nRing22MCAddRate = ReadInteger("Setup", "Ring22MCAddRate", M2Share.g_Config.nRing22MCAddRate);
            if (ReadInteger("Setup", "Ring22SCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring22SCAddValueMaxLimit", M2Share.g_Config.nRing22SCAddValueMaxLimit);
            M2Share.g_Config.nRing22SCAddValueMaxLimit = ReadInteger("Setup", "Ring22SCAddValueMaxLimit", M2Share.g_Config.nRing22SCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring22SCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring22SCAddValueRate", M2Share.g_Config.nRing22SCAddValueRate);
            M2Share.g_Config.nRing22SCAddValueRate = ReadInteger("Setup", "Ring22SCAddValueRate", M2Share.g_Config.nRing22SCAddValueRate);
            if (ReadInteger("Setup", "Ring22SCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring22SCAddRate", M2Share.g_Config.nRing22SCAddRate);
            M2Share.g_Config.nRing22SCAddRate = ReadInteger("Setup", "Ring22SCAddRate", M2Share.g_Config.nRing22SCAddRate);
            if (ReadInteger("Setup", "Ring23DCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring23DCAddValueMaxLimit", M2Share.g_Config.nRing23DCAddValueMaxLimit);
            M2Share.g_Config.nRing23DCAddValueMaxLimit = ReadInteger("Setup", "Ring23DCAddValueMaxLimit", M2Share.g_Config.nRing23DCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring23DCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring23DCAddValueRate", M2Share.g_Config.nRing23DCAddValueRate);
            M2Share.g_Config.nRing23DCAddValueRate = ReadInteger("Setup", "Ring23DCAddValueRate", M2Share.g_Config.nRing23DCAddValueRate);
            if (ReadInteger("Setup", "Ring23DCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring23DCAddRate", M2Share.g_Config.nRing23DCAddRate);
            M2Share.g_Config.nRing23DCAddRate = ReadInteger("Setup", "Ring23DCAddRate", M2Share.g_Config.nRing23DCAddRate);
            if (ReadInteger("Setup", "Ring23MCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring23MCAddValueMaxLimit", M2Share.g_Config.nRing23MCAddValueMaxLimit);
            M2Share.g_Config.nRing23MCAddValueMaxLimit = ReadInteger("Setup", "Ring23MCAddValueMaxLimit", M2Share.g_Config.nRing23MCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring23MCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring23MCAddValueRate", M2Share.g_Config.nRing23MCAddValueRate);
            M2Share.g_Config.nRing23MCAddValueRate = ReadInteger("Setup", "Ring23MCAddValueRate", M2Share.g_Config.nRing23MCAddValueRate);
            if (ReadInteger("Setup", "Ring23MCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring23MCAddRate", M2Share.g_Config.nRing23MCAddRate);
            M2Share.g_Config.nRing23MCAddRate = ReadInteger("Setup", "Ring23MCAddRate", M2Share.g_Config.nRing23MCAddRate);
            if (ReadInteger("Setup", "Ring23SCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring23SCAddValueMaxLimit", M2Share.g_Config.nRing23SCAddValueMaxLimit);
            M2Share.g_Config.nRing23SCAddValueMaxLimit = ReadInteger("Setup", "Ring23SCAddValueMaxLimit", M2Share.g_Config.nRing23SCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring23SCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring23SCAddValueRate", M2Share.g_Config.nRing23SCAddValueRate);
            M2Share.g_Config.nRing23SCAddValueRate = ReadInteger("Setup", "Ring23SCAddValueRate", M2Share.g_Config.nRing23SCAddValueRate);
            if (ReadInteger("Setup", "Ring23SCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring23SCAddRate", M2Share.g_Config.nRing23SCAddRate);
            M2Share.g_Config.nRing23SCAddRate = ReadInteger("Setup", "Ring23SCAddRate", M2Share.g_Config.nRing23SCAddRate);
            if (ReadInteger("Setup", "HelMetDCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "HelMetDCAddValueMaxLimit", M2Share.g_Config.nHelMetDCAddValueMaxLimit);
            M2Share.g_Config.nHelMetDCAddValueMaxLimit = ReadInteger("Setup", "HelMetDCAddValueMaxLimit", M2Share.g_Config.nHelMetDCAddValueMaxLimit);
            if (ReadInteger("Setup", "HelMetDCAddValueRate", -1) < 0)
                WriteInteger("Setup", "HelMetDCAddValueRate", M2Share.g_Config.nHelMetDCAddValueRate);
            M2Share.g_Config.nHelMetDCAddValueRate = ReadInteger("Setup", "HelMetDCAddValueRate", M2Share.g_Config.nHelMetDCAddValueRate);
            if (ReadInteger("Setup", "HelMetDCAddRate", -1) < 0)
                WriteInteger("Setup", "HelMetDCAddRate", M2Share.g_Config.nHelMetDCAddRate);
            M2Share.g_Config.nHelMetDCAddRate = ReadInteger("Setup", "HelMetDCAddRate", M2Share.g_Config.nHelMetDCAddRate);
            if (ReadInteger("Setup", "HelMetMCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "HelMetMCAddValueMaxLimit", M2Share.g_Config.nHelMetMCAddValueMaxLimit);
            M2Share.g_Config.nHelMetMCAddValueMaxLimit = ReadInteger("Setup", "HelMetMCAddValueMaxLimit", M2Share.g_Config.nHelMetMCAddValueMaxLimit);
            if (ReadInteger("Setup", "HelMetMCAddValueRate", -1) < 0)
                WriteInteger("Setup", "HelMetMCAddValueRate", M2Share.g_Config.nHelMetMCAddValueRate);
            M2Share.g_Config.nHelMetMCAddValueRate = ReadInteger("Setup", "HelMetMCAddValueRate", M2Share.g_Config.nHelMetMCAddValueRate);
            if (ReadInteger("Setup", "HelMetMCAddRate", -1) < 0)
                WriteInteger("Setup", "HelMetMCAddRate", M2Share.g_Config.nHelMetMCAddRate);
            M2Share.g_Config.nHelMetMCAddRate = ReadInteger("Setup", "HelMetMCAddRate", M2Share.g_Config.nHelMetMCAddRate);
            if (ReadInteger("Setup", "HelMetSCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "HelMetSCAddValueMaxLimit", M2Share.g_Config.nHelMetSCAddValueMaxLimit);
            M2Share.g_Config.nHelMetSCAddValueMaxLimit = ReadInteger("Setup", "HelMetSCAddValueMaxLimit", M2Share.g_Config.nHelMetSCAddValueMaxLimit);
            if (ReadInteger("Setup", "HelMetSCAddValueRate", -1) < 0)
                WriteInteger("Setup", "HelMetSCAddValueRate", M2Share.g_Config.nHelMetSCAddValueRate);
            M2Share.g_Config.nHelMetSCAddValueRate = ReadInteger("Setup", "HelMetSCAddValueRate", M2Share.g_Config.nHelMetSCAddValueRate);
            if (ReadInteger("Setup", "HelMetSCAddRate", -1) < 0)
                WriteInteger("Setup", "HelMetSCAddRate", M2Share.g_Config.nHelMetSCAddRate);
            M2Share.g_Config.nHelMetSCAddRate = ReadInteger("Setup", "HelMetSCAddRate", M2Share.g_Config.nHelMetSCAddRate);
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetACAddRate", M2Share.g_Config.nUnknowHelMetACAddRate);
            else
                M2Share.g_Config.nUnknowHelMetACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetACAddValueMaxLimit", M2Share.g_Config.nUnknowHelMetACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowHelMetACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetMACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetMACAddRate", M2Share.g_Config.nUnknowHelMetMACAddRate);
            else
                M2Share.g_Config.nUnknowHelMetMACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetMACAddValueMaxLimit", M2Share.g_Config.nUnknowHelMetMACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowHelMetMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetDCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetDCAddRate", M2Share.g_Config.nUnknowHelMetDCAddRate);
            else
                M2Share.g_Config.nUnknowHelMetDCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetDCAddValueMaxLimit", M2Share.g_Config.nUnknowHelMetDCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowHelMetDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetMCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetMCAddRate", M2Share.g_Config.nUnknowHelMetMCAddRate);
            else
                M2Share.g_Config.nUnknowHelMetMCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetMCAddValueMaxLimit", M2Share.g_Config.nUnknowHelMetMCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowHelMetMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetSCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetSCAddRate", M2Share.g_Config.nUnknowHelMetSCAddRate);
            else
                M2Share.g_Config.nUnknowHelMetSCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetSCAddValueMaxLimit", M2Share.g_Config.nUnknowHelMetSCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowHelMetSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceACAddRate", M2Share.g_Config.nUnknowNecklaceACAddRate);
            else
                M2Share.g_Config.nUnknowNecklaceACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceACAddValueMaxLimit", M2Share.g_Config.nUnknowNecklaceACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowNecklaceACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceMACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMACAddRate", M2Share.g_Config.nUnknowNecklaceMACAddRate);
            else
                M2Share.g_Config.nUnknowNecklaceMACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMACAddValueMaxLimit", M2Share.g_Config.nUnknowNecklaceMACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowNecklaceMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceDCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceDCAddRate", M2Share.g_Config.nUnknowNecklaceDCAddRate);
            else
                M2Share.g_Config.nUnknowNecklaceDCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceDCAddValueMaxLimit", M2Share.g_Config.nUnknowNecklaceDCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowNecklaceDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceMCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMCAddRate", M2Share.g_Config.nUnknowNecklaceMCAddRate);
            else
                M2Share.g_Config.nUnknowNecklaceMCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMCAddValueMaxLimit", M2Share.g_Config.nUnknowNecklaceMCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowNecklaceMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceSCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceSCAddRate", M2Share.g_Config.nUnknowNecklaceSCAddRate);
            else
                M2Share.g_Config.nUnknowNecklaceSCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceSCAddValueMaxLimit", M2Share.g_Config.nUnknowNecklaceSCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowNecklaceSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingACAddRate", M2Share.g_Config.nUnknowRingACAddRate);
            else
                M2Share.g_Config.nUnknowRingACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingACAddValueMaxLimit", M2Share.g_Config.nUnknowRingACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowRingACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingMACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMACAddRate", M2Share.g_Config.nUnknowRingMACAddRate);
            else
                M2Share.g_Config.nUnknowRingMACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMACAddValueMaxLimit", M2Share.g_Config.nUnknowRingMACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowRingMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingDCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingDCAddRate", M2Share.g_Config.nUnknowRingDCAddRate);
            else
                M2Share.g_Config.nUnknowRingDCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingDCAddValueMaxLimit", M2Share.g_Config.nUnknowRingDCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowRingDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingMCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMCAddRate", M2Share.g_Config.nUnknowRingMCAddRate);
            else
                M2Share.g_Config.nUnknowRingMCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMCAddValueMaxLimit", M2Share.g_Config.nUnknowRingMCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowRingMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingSCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingSCAddRate", M2Share.g_Config.nUnknowRingSCAddRate);
            else
                M2Share.g_Config.nUnknowRingSCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingSCAddValueMaxLimit", M2Share.g_Config.nUnknowRingSCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowRingSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MonOneDropGoldCount", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MonOneDropGoldCount", M2Share.g_Config.nMonOneDropGoldCount);
            else
                M2Share.g_Config.nMonOneDropGoldCount = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "SendCurTickCount", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "SendCurTickCount", M2Share.g_Config.boSendCurTickCount);
            else
                M2Share.g_Config.boSendCurTickCount = nLoadInteger == 1;
            if (ReadInteger("Setup", "MakeMineHitRate", -1) < 0)
                WriteInteger("Setup", "MakeMineHitRate", M2Share.g_Config.nMakeMineHitRate);
            M2Share.g_Config.nMakeMineHitRate = ReadInteger("Setup", "MakeMineHitRate", M2Share.g_Config.nMakeMineHitRate);
            if (ReadInteger("Setup", "MakeMineRate", -1) < 0)
                WriteInteger("Setup", "MakeMineRate", M2Share.g_Config.nMakeMineRate);
            M2Share.g_Config.nMakeMineRate = ReadInteger("Setup", "MakeMineRate", M2Share.g_Config.nMakeMineRate);
            if (ReadInteger("Setup", "StoneTypeRate", -1) < 0)
                WriteInteger("Setup", "StoneTypeRate", M2Share.g_Config.nStoneTypeRate);
            M2Share.g_Config.nStoneTypeRate = ReadInteger("Setup", "StoneTypeRate", M2Share.g_Config.nStoneTypeRate);
            if (ReadInteger("Setup", "StoneTypeRateMin", -1) < 0)
                WriteInteger("Setup", "StoneTypeRateMin", M2Share.g_Config.nStoneTypeRateMin);
            M2Share.g_Config.nStoneTypeRateMin = ReadInteger("Setup", "StoneTypeRateMin", M2Share.g_Config.nStoneTypeRateMin);
            if (ReadInteger("Setup", "GoldStoneMin", -1) < 0)
                WriteInteger("Setup", "GoldStoneMin", M2Share.g_Config.nGoldStoneMin);
            M2Share.g_Config.nGoldStoneMin = ReadInteger("Setup", "GoldStoneMin", M2Share.g_Config.nGoldStoneMin);
            if (ReadInteger("Setup", "GoldStoneMax", -1) < 0)
                WriteInteger("Setup", "GoldStoneMax", M2Share.g_Config.nGoldStoneMax);
            M2Share.g_Config.nGoldStoneMax = ReadInteger("Setup", "GoldStoneMax", M2Share.g_Config.nGoldStoneMax);
            if (ReadInteger("Setup", "SilverStoneMin", -1) < 0)
                WriteInteger("Setup", "SilverStoneMin", M2Share.g_Config.nSilverStoneMin);
            M2Share.g_Config.nSilverStoneMin = ReadInteger("Setup", "SilverStoneMin", M2Share.g_Config.nSilverStoneMin);
            if (ReadInteger("Setup", "SilverStoneMax", -1) < 0)
                WriteInteger("Setup", "SilverStoneMax", M2Share.g_Config.nSilverStoneMax);
            M2Share.g_Config.nSilverStoneMax = ReadInteger("Setup", "SilverStoneMax", M2Share.g_Config.nSilverStoneMax);
            if (ReadInteger("Setup", "SteelStoneMin", -1) < 0)
                WriteInteger("Setup", "SteelStoneMin", M2Share.g_Config.nSteelStoneMin);
            M2Share.g_Config.nSteelStoneMin = ReadInteger("Setup", "SteelStoneMin", M2Share.g_Config.nSteelStoneMin);
            if (ReadInteger("Setup", "SteelStoneMax", -1) < 0)
                WriteInteger("Setup", "SteelStoneMax", M2Share.g_Config.nSteelStoneMax);
            M2Share.g_Config.nSteelStoneMax = ReadInteger("Setup", "SteelStoneMax", M2Share.g_Config.nSteelStoneMax);
            if (ReadInteger("Setup", "BlackStoneMin", -1) < 0)
                WriteInteger("Setup", "BlackStoneMin", M2Share.g_Config.nBlackStoneMin);
            M2Share.g_Config.nBlackStoneMin = ReadInteger("Setup", "BlackStoneMin", M2Share.g_Config.nBlackStoneMin);
            if (ReadInteger("Setup", "BlackStoneMax", -1) < 0)
                WriteInteger("Setup", "BlackStoneMax", M2Share.g_Config.nBlackStoneMax);
            M2Share.g_Config.nBlackStoneMax = ReadInteger("Setup", "BlackStoneMax", M2Share.g_Config.nBlackStoneMax);
            if (ReadInteger("Setup", "StoneMinDura", -1) < 0)
                WriteInteger("Setup", "StoneMinDura", M2Share.g_Config.nStoneMinDura);
            M2Share.g_Config.nStoneMinDura = ReadInteger("Setup", "StoneMinDura", M2Share.g_Config.nStoneMinDura);
            if (ReadInteger("Setup", "StoneGeneralDuraRate", -1) < 0)
                WriteInteger("Setup", "StoneGeneralDuraRate", M2Share.g_Config.nStoneGeneralDuraRate);
            M2Share.g_Config.nStoneGeneralDuraRate = ReadInteger("Setup", "StoneGeneralDuraRate", M2Share.g_Config.nStoneGeneralDuraRate);
            if (ReadInteger("Setup", "StoneAddDuraRate", -1) < 0)
                WriteInteger("Setup", "StoneAddDuraRate", M2Share.g_Config.nStoneAddDuraRate);
            M2Share.g_Config.nStoneAddDuraRate = ReadInteger("Setup", "StoneAddDuraRate", M2Share.g_Config.nStoneAddDuraRate);
            if (ReadInteger("Setup", "StoneAddDuraMax", -1) < 0)
                WriteInteger("Setup", "StoneAddDuraMax", M2Share.g_Config.nStoneAddDuraMax);
            M2Share.g_Config.nStoneAddDuraMax = ReadInteger("Setup", "StoneAddDuraMax", M2Share.g_Config.nStoneAddDuraMax);
            if (ReadInteger("Setup", "WinLottery1Min", -1) < 0)
                WriteInteger("Setup", "WinLottery1Min", M2Share.g_Config.nWinLottery1Min);
            M2Share.g_Config.nWinLottery1Min = ReadInteger("Setup", "WinLottery1Min", M2Share.g_Config.nWinLottery1Min);
            if (ReadInteger("Setup", "WinLottery1Max", -1) < 0)
                WriteInteger("Setup", "WinLottery1Max", M2Share.g_Config.nWinLottery1Max);
            M2Share.g_Config.nWinLottery1Max = ReadInteger("Setup", "WinLottery1Max", M2Share.g_Config.nWinLottery1Max);
            if (ReadInteger("Setup", "WinLottery2Min", -1) < 0)
                WriteInteger("Setup", "WinLottery2Min", M2Share.g_Config.nWinLottery2Min);
            M2Share.g_Config.nWinLottery2Min = ReadInteger("Setup", "WinLottery2Min", M2Share.g_Config.nWinLottery2Min);
            if (ReadInteger("Setup", "WinLottery2Max", -1) < 0)
                WriteInteger("Setup", "WinLottery2Max", M2Share.g_Config.nWinLottery2Max);
            M2Share.g_Config.nWinLottery2Max = ReadInteger("Setup", "WinLottery2Max", M2Share.g_Config.nWinLottery2Max);
            if (ReadInteger("Setup", "WinLottery3Min", -1) < 0)
                WriteInteger("Setup", "WinLottery3Min", M2Share.g_Config.nWinLottery3Min);
            M2Share.g_Config.nWinLottery3Min = ReadInteger("Setup", "WinLottery3Min", M2Share.g_Config.nWinLottery3Min);
            if (ReadInteger("Setup", "WinLottery3Max", -1) < 0)
                WriteInteger("Setup", "WinLottery3Max", M2Share.g_Config.nWinLottery3Max);
            M2Share.g_Config.nWinLottery3Max = ReadInteger("Setup", "WinLottery3Max", M2Share.g_Config.nWinLottery3Max);
            if (ReadInteger("Setup", "WinLottery4Min", -1) < 0)
                WriteInteger("Setup", "WinLottery4Min", M2Share.g_Config.nWinLottery4Min);
            M2Share.g_Config.nWinLottery4Min = ReadInteger("Setup", "WinLottery4Min", M2Share.g_Config.nWinLottery4Min);
            if (ReadInteger("Setup", "WinLottery4Max", -1) < 0)
                WriteInteger("Setup", "WinLottery4Max", M2Share.g_Config.nWinLottery4Max);
            M2Share.g_Config.nWinLottery4Max = ReadInteger("Setup", "WinLottery4Max", M2Share.g_Config.nWinLottery4Max);
            if (ReadInteger("Setup", "WinLottery5Min", -1) < 0)
                WriteInteger("Setup", "WinLottery5Min", M2Share.g_Config.nWinLottery5Min);
            M2Share.g_Config.nWinLottery5Min = ReadInteger("Setup", "WinLottery5Min", M2Share.g_Config.nWinLottery5Min);
            if (ReadInteger("Setup", "WinLottery5Max", -1) < 0)
                WriteInteger("Setup", "WinLottery5Max", M2Share.g_Config.nWinLottery5Max);
            M2Share.g_Config.nWinLottery5Max = ReadInteger("Setup", "WinLottery5Max", M2Share.g_Config.nWinLottery5Max);
            if (ReadInteger("Setup", "WinLottery6Min", -1) < 0)
                WriteInteger("Setup", "WinLottery6Min", M2Share.g_Config.nWinLottery6Min);
            M2Share.g_Config.nWinLottery6Min = ReadInteger("Setup", "WinLottery6Min", M2Share.g_Config.nWinLottery6Min);
            if (ReadInteger("Setup", "WinLottery6Max", -1) < 0)
                WriteInteger("Setup", "WinLottery6Max", M2Share.g_Config.nWinLottery6Max);
            M2Share.g_Config.nWinLottery6Max = ReadInteger("Setup", "WinLottery6Max", M2Share.g_Config.nWinLottery6Max);
            if (ReadInteger("Setup", "WinLotteryRate", -1) < 0)
                WriteInteger("Setup", "WinLotteryRate", M2Share.g_Config.nWinLotteryRate);
            M2Share.g_Config.nWinLotteryRate = ReadInteger("Setup", "WinLotteryRate", M2Share.g_Config.nWinLotteryRate);
            if (ReadInteger("Setup", "WinLottery1Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery1Gold", M2Share.g_Config.nWinLottery1Gold);
            M2Share.g_Config.nWinLottery1Gold = ReadInteger("Setup", "WinLottery1Gold", M2Share.g_Config.nWinLottery1Gold);
            if (ReadInteger("Setup", "WinLottery2Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery2Gold", M2Share.g_Config.nWinLottery2Gold);
            M2Share.g_Config.nWinLottery2Gold = ReadInteger("Setup", "WinLottery2Gold", M2Share.g_Config.nWinLottery2Gold);
            if (ReadInteger("Setup", "WinLottery3Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery3Gold", M2Share.g_Config.nWinLottery3Gold);
            M2Share.g_Config.nWinLottery3Gold = ReadInteger("Setup", "WinLottery3Gold", M2Share.g_Config.nWinLottery3Gold);
            if (ReadInteger("Setup", "WinLottery4Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery4Gold", M2Share.g_Config.nWinLottery4Gold);
            M2Share.g_Config.nWinLottery4Gold = ReadInteger("Setup", "WinLottery4Gold", M2Share.g_Config.nWinLottery4Gold);
            if (ReadInteger("Setup", "WinLottery5Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery5Gold", M2Share.g_Config.nWinLottery5Gold);
            M2Share.g_Config.nWinLottery5Gold = ReadInteger("Setup", "WinLottery5Gold", M2Share.g_Config.nWinLottery5Gold);
            if (ReadInteger("Setup", "WinLottery6Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery6Gold", M2Share.g_Config.nWinLottery6Gold);
            M2Share.g_Config.nWinLottery6Gold = ReadInteger("Setup", "WinLottery6Gold", M2Share.g_Config.nWinLottery6Gold);
            if (ReadInteger("Setup", "GuildRecallTime", -1) < 0)
                WriteInteger("Setup", "GuildRecallTime", M2Share.g_Config.nGuildRecallTime);
            M2Share.g_Config.nGuildRecallTime = ReadInteger("Setup", "GuildRecallTime", M2Share.g_Config.nGuildRecallTime);
            if (ReadInteger("Setup", "GroupRecallTime", -1) < 0)
                WriteInteger("Setup", "GroupRecallTime", M2Share.g_Config.nGroupRecallTime);
            M2Share.g_Config.nGroupRecallTime = ReadInteger("Setup", "GroupRecallTime", M2Share.g_Config.nGroupRecallTime);
            if (ReadInteger("Setup", "ControlDropItem", -1) < 0)
                WriteBool("Setup", "ControlDropItem", M2Share.g_Config.boControlDropItem);
            M2Share.g_Config.boControlDropItem = ReadBool("Setup", "ControlDropItem", M2Share.g_Config.boControlDropItem);
            if (ReadInteger("Setup", "InSafeDisableDrop", -1) < 0)
                WriteBool("Setup", "InSafeDisableDrop", M2Share.g_Config.boInSafeDisableDrop);
            M2Share.g_Config.boInSafeDisableDrop = ReadBool("Setup", "InSafeDisableDrop", M2Share.g_Config.boInSafeDisableDrop);
            if (ReadInteger("Setup", "CanDropGold", -1) < 0)
                WriteInteger("Setup", "CanDropGold", M2Share.g_Config.nCanDropGold);
            M2Share.g_Config.nCanDropGold = ReadInteger("Setup", "CanDropGold", M2Share.g_Config.nCanDropGold);
            if (ReadInteger("Setup", "CanDropPrice", -1) < 0)
                WriteInteger("Setup", "CanDropPrice", M2Share.g_Config.nCanDropPrice);
            M2Share.g_Config.nCanDropPrice = ReadInteger("Setup", "CanDropPrice", M2Share.g_Config.nCanDropPrice);
            nLoadInteger = ReadInteger("Setup", "SendCustemMsg", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "SendCustemMsg", M2Share.g_Config.boSendCustemMsg);
            else
                M2Share.g_Config.boSendCustemMsg = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "SubkMasterSendMsg", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "SubkMasterSendMsg", M2Share.g_Config.boSubkMasterSendMsg);
            else
                M2Share.g_Config.boSubkMasterSendMsg = nLoadInteger == 1;
            if (ReadInteger("Setup", "SuperRepairPriceRate", -1) < 0)
                WriteInteger("Setup", "SuperRepairPriceRate", M2Share.g_Config.nSuperRepairPriceRate);
            M2Share.g_Config.nSuperRepairPriceRate = ReadInteger("Setup", "SuperRepairPriceRate", M2Share.g_Config.nSuperRepairPriceRate);
            if (ReadInteger("Setup", "RepairItemDecDura", -1) < 0)
                WriteInteger("Setup", "RepairItemDecDura", M2Share.g_Config.nRepairItemDecDura);
            M2Share.g_Config.nRepairItemDecDura = ReadInteger("Setup", "RepairItemDecDura", M2Share.g_Config.nRepairItemDecDura);
            if (ReadInteger("Setup", "DieScatterBag", -1) < 0)
                WriteBool("Setup", "DieScatterBag", M2Share.g_Config.boDieScatterBag);
            M2Share.g_Config.boDieScatterBag = ReadBool("Setup", "DieScatterBag", M2Share.g_Config.boDieScatterBag);
            if (ReadInteger("Setup", "DieScatterBagRate", -1) < 0)
                WriteInteger("Setup", "DieScatterBagRate", M2Share.g_Config.nDieScatterBagRate);
            M2Share.g_Config.nDieScatterBagRate = ReadInteger("Setup", "DieScatterBagRate", M2Share.g_Config.nDieScatterBagRate);
            if (ReadInteger("Setup", "DieRedScatterBagAll", -1) < 0)
                WriteBool("Setup", "DieRedScatterBagAll", M2Share.g_Config.boDieRedScatterBagAll);
            M2Share.g_Config.boDieRedScatterBagAll = ReadBool("Setup", "DieRedScatterBagAll", M2Share.g_Config.boDieRedScatterBagAll);
            if (ReadInteger("Setup", "DieDropUseItemRate", -1) < 0)
                WriteInteger("Setup", "DieDropUseItemRate", M2Share.g_Config.nDieDropUseItemRate);
            M2Share.g_Config.nDieDropUseItemRate = ReadInteger("Setup", "DieDropUseItemRate", M2Share.g_Config.nDieDropUseItemRate);
            if (ReadInteger("Setup", "DieRedDropUseItemRate", -1) < 0)
                WriteInteger("Setup", "DieRedDropUseItemRate", M2Share.g_Config.nDieRedDropUseItemRate);
            M2Share.g_Config.nDieRedDropUseItemRate = ReadInteger("Setup", "DieRedDropUseItemRate", M2Share.g_Config.nDieRedDropUseItemRate);
            if (ReadInteger("Setup", "DieDropGold", -1) < 0)
                WriteBool("Setup", "DieDropGold", M2Share.g_Config.boDieDropGold);
            M2Share.g_Config.boDieDropGold = ReadBool("Setup", "DieDropGold", M2Share.g_Config.boDieDropGold);
            if (ReadInteger("Setup", "KillByHumanDropUseItem", -1) < 0)
                WriteBool("Setup", "KillByHumanDropUseItem", M2Share.g_Config.boKillByHumanDropUseItem);
            M2Share.g_Config.boKillByHumanDropUseItem = ReadBool("Setup", "KillByHumanDropUseItem", M2Share.g_Config.boKillByHumanDropUseItem);
            if (ReadInteger("Setup", "KillByMonstDropUseItem", -1) < 0)
                WriteBool("Setup", "KillByMonstDropUseItem", M2Share.g_Config.boKillByMonstDropUseItem);
            M2Share.g_Config.boKillByMonstDropUseItem = ReadBool("Setup", "KillByMonstDropUseItem", M2Share.g_Config.boKillByMonstDropUseItem);
            if (ReadInteger("Setup", "KickExpireHuman", -1) < 0)
                WriteBool("Setup", "KickExpireHuman", M2Share.g_Config.boKickExpireHuman);
            M2Share.g_Config.boKickExpireHuman = ReadBool("Setup", "KickExpireHuman", M2Share.g_Config.boKickExpireHuman);
            if (ReadInteger("Setup", "GuildRankNameLen", -1) < 0)
                WriteInteger("Setup", "GuildRankNameLen", M2Share.g_Config.nGuildRankNameLen);
            M2Share.g_Config.nGuildRankNameLen = ReadInteger("Setup", "GuildRankNameLen", M2Share.g_Config.nGuildRankNameLen);
            if (ReadInteger("Setup", "GuildNameLen", -1) < 0)
                WriteInteger("Setup", "GuildNameLen", M2Share.g_Config.nGuildNameLen);
            M2Share.g_Config.nGuildNameLen = ReadInteger("Setup", "GuildNameLen", M2Share.g_Config.nGuildNameLen);
            if (ReadInteger("Setup", "GuildMemberMaxLimit", -1) < 0)
                WriteInteger("Setup", "GuildMemberMaxLimit", M2Share.g_Config.nGuildMemberMaxLimit);
            M2Share.g_Config.nGuildMemberMaxLimit = ReadInteger("Setup", "GuildMemberMaxLimit", M2Share.g_Config.nGuildMemberMaxLimit);
            if (ReadInteger("Setup", "AttackPosionRate", -1) < 0)
                WriteInteger("Setup", "AttackPosionRate", M2Share.g_Config.nAttackPosionRate);
            M2Share.g_Config.nAttackPosionRate = ReadInteger("Setup", "AttackPosionRate", M2Share.g_Config.nAttackPosionRate);
            if (ReadInteger("Setup", "AttackPosionTime", -1) < 0)
                WriteInteger("Setup", "AttackPosionTime", M2Share.g_Config.nAttackPosionTime);
            M2Share.g_Config.nAttackPosionTime = ReadInteger("Setup", "AttackPosionTime", M2Share.g_Config.nAttackPosionTime);
            if (ReadInteger("Setup", "RevivalTime", -1) < 0)
                WriteInteger("Setup", "RevivalTime", M2Share.g_Config.dwRevivalTime);
            M2Share.g_Config.dwRevivalTime = ReadInteger("Setup", "RevivalTime", M2Share.g_Config.dwRevivalTime);
            nLoadInteger = ReadInteger("Setup", "UserMoveCanDupObj", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "UserMoveCanDupObj", M2Share.g_Config.boUserMoveCanDupObj);
            else
                M2Share.g_Config.boUserMoveCanDupObj = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "UserMoveCanOnItem", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "UserMoveCanOnItem", M2Share.g_Config.boUserMoveCanOnItem);
            else
                M2Share.g_Config.boUserMoveCanOnItem = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "UserMoveTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UserMoveTime", M2Share.g_Config.dwUserMoveTime);
            else
                M2Share.g_Config.dwUserMoveTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "PKDieLostExpRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "PKDieLostExpRate", M2Share.g_Config.dwPKDieLostExpRate);
            else
                M2Share.g_Config.dwPKDieLostExpRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "PKDieLostLevelRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "PKDieLostLevelRate", M2Share.g_Config.nPKDieLostLevelRate);
            else
                M2Share.g_Config.nPKDieLostLevelRate = nLoadInteger;
            if (ReadInteger("Setup", "PKFlagNameColor", -1) < 0)
                WriteInteger("Setup", "PKFlagNameColor", M2Share.g_Config.btPKFlagNameColor);
            M2Share.g_Config.btPKFlagNameColor = Read<byte>("Setup", "PKFlagNameColor", M2Share.g_Config.btPKFlagNameColor);
            if (ReadInteger("Setup", "AllyAndGuildNameColor", -1) < 0)
                WriteInteger("Setup", "AllyAndGuildNameColor", M2Share.g_Config.btAllyAndGuildNameColor);
            M2Share.g_Config.btAllyAndGuildNameColor = Read<byte>("Setup", "AllyAndGuildNameColor", M2Share.g_Config.btAllyAndGuildNameColor);
            if (ReadInteger("Setup", "WarGuildNameColor", -1) < 0)
                WriteInteger("Setup", "WarGuildNameColor", M2Share.g_Config.btWarGuildNameColor);
            M2Share.g_Config.btWarGuildNameColor = Read<byte>("Setup", "WarGuildNameColor", M2Share.g_Config.btWarGuildNameColor);
            if (ReadInteger("Setup", "InFreePKAreaNameColor", -1) < 0)
                WriteInteger("Setup", "InFreePKAreaNameColor", M2Share.g_Config.btInFreePKAreaNameColor);
            M2Share.g_Config.btInFreePKAreaNameColor = Read<byte>("Setup", "InFreePKAreaNameColor", M2Share.g_Config.btInFreePKAreaNameColor);
            if (ReadInteger("Setup", "PKLevel1NameColor", -1) < 0)
                WriteInteger("Setup", "PKLevel1NameColor", M2Share.g_Config.btPKLevel1NameColor);
            M2Share.g_Config.btPKLevel1NameColor = Read<byte>("Setup", "PKLevel1NameColor", M2Share.g_Config.btPKLevel1NameColor);
            if (ReadInteger("Setup", "PKLevel2NameColor", -1) < 0)
                WriteInteger("Setup", "PKLevel2NameColor", M2Share.g_Config.btPKLevel2NameColor);
            M2Share.g_Config.btPKLevel2NameColor = Read<byte>("Setup", "PKLevel2NameColor", M2Share.g_Config.btPKLevel2NameColor);
            if (ReadInteger("Setup", "SpiritMutiny", -1) < 0)
                WriteBool("Setup", "SpiritMutiny", M2Share.g_Config.boSpiritMutiny);
            M2Share.g_Config.boSpiritMutiny = ReadBool("Setup", "SpiritMutiny", M2Share.g_Config.boSpiritMutiny);
            if (ReadInteger("Setup", "SpiritMutinyTime", -1) < 0)
                WriteInteger("Setup", "SpiritMutinyTime", M2Share.g_Config.dwSpiritMutinyTime);
            M2Share.g_Config.dwSpiritMutinyTime = ReadInteger("Setup", "SpiritMutinyTime", M2Share.g_Config.dwSpiritMutinyTime);
            if (ReadInteger("Setup", "SpiritPowerRate", -1) < 0)
                WriteInteger("Setup", "SpiritPowerRate", M2Share.g_Config.nSpiritPowerRate);
            M2Share.g_Config.nSpiritPowerRate = ReadInteger("Setup", "SpiritPowerRate", M2Share.g_Config.nSpiritPowerRate);
            if (ReadInteger("Setup", "MasterDieMutiny", -1) < 0)
                WriteBool("Setup", "MasterDieMutiny", M2Share.g_Config.boMasterDieMutiny);
            M2Share.g_Config.boMasterDieMutiny = ReadBool("Setup", "MasterDieMutiny", M2Share.g_Config.boMasterDieMutiny);
            if (ReadInteger("Setup", "MasterDieMutinyRate", -1) < 0)
                WriteInteger("Setup", "MasterDieMutinyRate", M2Share.g_Config.nMasterDieMutinyRate);
            M2Share.g_Config.nMasterDieMutinyRate = ReadInteger("Setup", "MasterDieMutinyRate", M2Share.g_Config.nMasterDieMutinyRate);
            if (ReadInteger("Setup", "MasterDieMutinyPower", -1) < 0)
                WriteInteger("Setup", "MasterDieMutinyPower", M2Share.g_Config.nMasterDieMutinyPower);
            M2Share.g_Config.nMasterDieMutinyPower = ReadInteger("Setup", "MasterDieMutinyPower", M2Share.g_Config.nMasterDieMutinyPower);
            if (ReadInteger("Setup", "MasterDieMutinyPower", -1) < 0)
                WriteInteger("Setup", "MasterDieMutinyPower", M2Share.g_Config.nMasterDieMutinySpeed);
            M2Share.g_Config.nMasterDieMutinySpeed = ReadInteger("Setup", "MasterDieMutinyPower", M2Share.g_Config.nMasterDieMutinySpeed);
            nLoadInteger = ReadInteger("Setup", "BBMonAutoChangeColor", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "BBMonAutoChangeColor", M2Share.g_Config.boBBMonAutoChangeColor);
            else
                M2Share.g_Config.boBBMonAutoChangeColor = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "BBMonAutoChangeColorTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "BBMonAutoChangeColorTime", M2Share.g_Config.dwBBMonAutoChangeColorTime);
            else
                M2Share.g_Config.dwBBMonAutoChangeColorTime = nLoadInteger;
            if (ReadInteger("Setup", "OldClientShowHiLevel", -1) < 0)
                WriteBool("Setup", "OldClientShowHiLevel", M2Share.g_Config.boOldClientShowHiLevel);
            M2Share.g_Config.boOldClientShowHiLevel = ReadBool("Setup", "OldClientShowHiLevel", M2Share.g_Config.boOldClientShowHiLevel);
            if (ReadInteger("Setup", "ShowScriptActionMsg", -1) < 0)
                WriteBool("Setup", "ShowScriptActionMsg", M2Share.g_Config.boShowScriptActionMsg);
            M2Share.g_Config.boShowScriptActionMsg = ReadBool("Setup", "ShowScriptActionMsg", M2Share.g_Config.boShowScriptActionMsg);
            if (ReadInteger("Setup", "RunSocketDieLoopLimit", -1) < 0)
                WriteInteger("Setup", "RunSocketDieLoopLimit", M2Share.g_Config.nRunSocketDieLoopLimit);
            M2Share.g_Config.nRunSocketDieLoopLimit = ReadInteger("Setup", "RunSocketDieLoopLimit", M2Share.g_Config.nRunSocketDieLoopLimit);
            if (ReadInteger("Setup", "ThreadRun", -1) < 0)
                WriteBool("Setup", "ThreadRun", M2Share.g_Config.boThreadRun);
            M2Share.g_Config.boThreadRun = ReadBool("Setup", "ThreadRun", M2Share.g_Config.boThreadRun);
            if (ReadInteger("Setup", "DeathColorEffect", -1) < 0)
                WriteInteger("Setup", "DeathColorEffect", M2Share.g_Config.ClientConf.btDieColor);
            M2Share.g_Config.ClientConf.btDieColor = Read<byte>("Setup", "DeathColorEffect", M2Share.g_Config.ClientConf.btDieColor);
            if (ReadInteger("Setup", "ParalyCanRun", -1) < 0)
                WriteBool("Setup", "ParalyCanRun", M2Share.g_Config.ClientConf.boParalyCanRun);
            M2Share.g_Config.ClientConf.boParalyCanRun = ReadBool("Setup", "ParalyCanRun", M2Share.g_Config.ClientConf.boParalyCanRun);
            if (ReadInteger("Setup", "ParalyCanWalk", -1) < 0)
                WriteBool("Setup", "ParalyCanWalk", M2Share.g_Config.ClientConf.boParalyCanWalk);
            M2Share.g_Config.ClientConf.boParalyCanWalk = ReadBool("Setup", "ParalyCanWalk", M2Share.g_Config.ClientConf.boParalyCanWalk);
            if (ReadInteger("Setup", "ParalyCanHit", -1) < 0)
                WriteBool("Setup", "ParalyCanHit", M2Share.g_Config.ClientConf.boParalyCanHit);
            M2Share.g_Config.ClientConf.boParalyCanHit = ReadBool("Setup", "ParalyCanHit", M2Share.g_Config.ClientConf.boParalyCanHit);
            if (ReadInteger("Setup", "ParalyCanSpell", -1) < 0)
                WriteBool("Setup", "ParalyCanSpell", M2Share.g_Config.ClientConf.boParalyCanSpell);
            M2Share.g_Config.ClientConf.boParalyCanSpell = ReadBool("Setup", "ParalyCanSpell", M2Share.g_Config.ClientConf.boParalyCanSpell);
            if (ReadInteger("Setup", "ShowExceptionMsg", -1) < 0)
                WriteBool("Setup", "ShowExceptionMsg", M2Share.g_Config.boShowExceptionMsg);
            M2Share.g_Config.boShowExceptionMsg = ReadBool("Setup", "ShowExceptionMsg", M2Share.g_Config.boShowExceptionMsg);
            if (ReadInteger("Setup", "ShowPreFixMsg", -1) < 0)
                WriteBool("Setup", "ShowPreFixMsg", M2Share.g_Config.boShowPreFixMsg);
            M2Share.g_Config.boShowPreFixMsg = ReadBool("Setup", "ShowPreFixMsg", M2Share.g_Config.boShowPreFixMsg);
            if (ReadInteger("Setup", "MagTurnUndeadLevel", -1) < 0)
                WriteInteger("Setup", "MagTurnUndeadLevel", M2Share.g_Config.nMagTurnUndeadLevel);
            M2Share.g_Config.nMagTurnUndeadLevel = ReadInteger("Setup", "MagTurnUndeadLevel", M2Share.g_Config.nMagTurnUndeadLevel);
            nLoadInteger = ReadInteger("Setup", "MagTammingLevel", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingLevel", M2Share.g_Config.nMagTammingLevel);
            else
                M2Share.g_Config.nMagTammingLevel = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MagTammingTargetLevel", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingTargetLevel", M2Share.g_Config.nMagTammingTargetLevel);
            else
                M2Share.g_Config.nMagTammingTargetLevel = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MagTammingTargetHPRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingTargetHPRate", M2Share.g_Config.nMagTammingHPRate);
            else
                M2Share.g_Config.nMagTammingHPRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MagTammingCount", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingCount", M2Share.g_Config.nMagTammingCount);
            else
                M2Share.g_Config.nMagTammingCount = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MabMabeHitRandRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitRandRate", M2Share.g_Config.nMabMabeHitRandRate);
            else
                M2Share.g_Config.nMabMabeHitRandRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MabMabeHitMinLvLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitMinLvLimit", M2Share.g_Config.nMabMabeHitMinLvLimit);
            else
                M2Share.g_Config.nMabMabeHitMinLvLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MabMabeHitSucessRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitSucessRate", M2Share.g_Config.nMabMabeHitSucessRate);
            else
                M2Share.g_Config.nMabMabeHitSucessRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MabMabeHitMabeTimeRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitMabeTimeRate", M2Share.g_Config.nMabMabeHitMabeTimeRate);
            else
                M2Share.g_Config.nMabMabeHitMabeTimeRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MagicAttackRage", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagicAttackRage", M2Share.g_Config.nMagicAttackRage);
            else
                M2Share.g_Config.nMagicAttackRage = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "DropItemRage", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "DropItemRage", M2Share.g_Config.nDropItemRage);
            else
                M2Share.g_Config.nDropItemRage = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "AmyOunsulPoint", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "AmyOunsulPoint", M2Share.g_Config.nAmyOunsulPoint);
            else
                M2Share.g_Config.nAmyOunsulPoint = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "DisableInSafeZoneFireCross", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "DisableInSafeZoneFireCross", M2Share.g_Config.boDisableInSafeZoneFireCross);
            else
                M2Share.g_Config.boDisableInSafeZoneFireCross = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "GroupMbAttackPlayObject", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "GroupMbAttackPlayObject", M2Share.g_Config.boGroupMbAttackPlayObject);
            else
                M2Share.g_Config.boGroupMbAttackPlayObject = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "PosionDecHealthTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "PosionDecHealthTime", M2Share.g_Config.dwPosionDecHealthTime);
            else
                M2Share.g_Config.dwPosionDecHealthTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "PosionDamagarmor", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "PosionDamagarmor", M2Share.g_Config.nPosionDamagarmor);
            else
                M2Share.g_Config.nPosionDamagarmor = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "LimitSwordLong", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "LimitSwordLong", M2Share.g_Config.boLimitSwordLong);
            else
                M2Share.g_Config.boLimitSwordLong = !(nLoadInteger == 0);
            nLoadInteger = ReadInteger("Setup", "SwordLongPowerRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "SwordLongPowerRate", M2Share.g_Config.nSwordLongPowerRate);
            else
                M2Share.g_Config.nSwordLongPowerRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "FireBoomRage", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "FireBoomRage", M2Share.g_Config.nFireBoomRage);
            else
                M2Share.g_Config.nFireBoomRage = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "SnowWindRange", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "SnowWindRange", M2Share.g_Config.nSnowWindRange);
            else
                M2Share.g_Config.nSnowWindRange = nLoadInteger;
            if (ReadInteger("Setup", "ElecBlizzardRange", -1) < 0)
                WriteInteger("Setup", "ElecBlizzardRange", M2Share.g_Config.nElecBlizzardRange);
            M2Share.g_Config.nElecBlizzardRange = ReadInteger("Setup", "ElecBlizzardRange", M2Share.g_Config.nElecBlizzardRange);
            if (ReadInteger("Setup", "HumanLevelDiffer", -1) < 0)
                WriteInteger("Setup", "HumanLevelDiffer", M2Share.g_Config.nHumanLevelDiffer);
            M2Share.g_Config.nHumanLevelDiffer = ReadInteger("Setup", "HumanLevelDiffer", M2Share.g_Config.nHumanLevelDiffer);
            if (ReadInteger("Setup", "KillHumanWinLevel", -1) < 0)
                WriteBool("Setup", "KillHumanWinLevel", M2Share.g_Config.boKillHumanWinLevel);
            M2Share.g_Config.boKillHumanWinLevel = ReadBool("Setup", "KillHumanWinLevel", M2Share.g_Config.boKillHumanWinLevel);
            if (ReadInteger("Setup", "KilledLostLevel", -1) < 0)
                WriteBool("Setup", "KilledLostLevel", M2Share.g_Config.boKilledLostLevel);
            M2Share.g_Config.boKilledLostLevel = ReadBool("Setup", "KilledLostLevel", M2Share.g_Config.boKilledLostLevel);
            if (ReadInteger("Setup", "KillHumanWinLevelPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanWinLevelPoint", M2Share.g_Config.nKillHumanWinLevel);
            M2Share.g_Config.nKillHumanWinLevel = ReadInteger("Setup", "KillHumanWinLevelPoint", M2Share.g_Config.nKillHumanWinLevel);
            if (ReadInteger("Setup", "KilledLostLevelPoint", -1) < 0)
                WriteInteger("Setup", "KilledLostLevelPoint", M2Share.g_Config.nKilledLostLevel);
            M2Share.g_Config.nKilledLostLevel = ReadInteger("Setup", "KilledLostLevelPoint", M2Share.g_Config.nKilledLostLevel);
            if (ReadInteger("Setup", "KillHumanWinExp", -1) < 0)
                WriteBool("Setup", "KillHumanWinExp", M2Share.g_Config.boKillHumanWinExp);
            M2Share.g_Config.boKillHumanWinExp = ReadBool("Setup", "KillHumanWinExp", M2Share.g_Config.boKillHumanWinExp);
            if (ReadInteger("Setup", "KilledLostExp", -1) < 0)
                WriteBool("Setup", "KilledLostExp", M2Share.g_Config.boKilledLostExp);
            M2Share.g_Config.boKilledLostExp = ReadBool("Setup", "KilledLostExp", M2Share.g_Config.boKilledLostExp);
            if (ReadInteger("Setup", "KillHumanWinExpPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanWinExpPoint", M2Share.g_Config.nKillHumanWinExp);
            M2Share.g_Config.nKillHumanWinExp = ReadInteger("Setup", "KillHumanWinExpPoint", M2Share.g_Config.nKillHumanWinExp);
            if (ReadInteger("Setup", "KillHumanLostExpPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanLostExpPoint", M2Share.g_Config.nKillHumanLostExp);
            M2Share.g_Config.nKillHumanLostExp = ReadInteger("Setup", "KillHumanLostExpPoint", M2Share.g_Config.nKillHumanLostExp);
            if (ReadInteger("Setup", "MonsterPowerRate", -1) < 0)
                WriteInteger("Setup", "MonsterPowerRate", M2Share.g_Config.nMonsterPowerRate);
            M2Share.g_Config.nMonsterPowerRate = ReadInteger("Setup", "MonsterPowerRate", M2Share.g_Config.nMonsterPowerRate);
            if (ReadInteger("Setup", "ItemsPowerRate", -1) < 0)
                WriteInteger("Setup", "ItemsPowerRate", M2Share.g_Config.nItemsPowerRate);
            M2Share.g_Config.nItemsPowerRate = ReadInteger("Setup", "ItemsPowerRate", M2Share.g_Config.nItemsPowerRate);
            if (ReadInteger("Setup", "ItemsACPowerRate", -1) < 0)
                WriteInteger("Setup", "ItemsACPowerRate", M2Share.g_Config.nItemsACPowerRate);
            M2Share.g_Config.nItemsACPowerRate = ReadInteger("Setup", "ItemsACPowerRate", M2Share.g_Config.nItemsACPowerRate);
            if (ReadInteger("Setup", "SendOnlineCount", -1) < 0)
                WriteBool("Setup", "SendOnlineCount", M2Share.g_Config.boSendOnlineCount);
            M2Share.g_Config.boSendOnlineCount = ReadBool("Setup", "SendOnlineCount", M2Share.g_Config.boSendOnlineCount);
            if (ReadInteger("Setup", "SendOnlineCountRate", -1) < 0)
                WriteInteger("Setup", "SendOnlineCountRate", M2Share.g_Config.nSendOnlineCountRate);
            M2Share.g_Config.nSendOnlineCountRate = ReadInteger("Setup", "SendOnlineCountRate", M2Share.g_Config.nSendOnlineCountRate);
            if (ReadInteger("Setup", "SendOnlineTime", -1) < 0)
                WriteInteger("Setup", "SendOnlineTime", M2Share.g_Config.dwSendOnlineTime);
            M2Share.g_Config.dwSendOnlineTime = ReadInteger("Setup", "SendOnlineTime", M2Share.g_Config.dwSendOnlineTime);
            if (ReadInteger("Setup", "SaveHumanRcdTime", -1) < 0)
                WriteInteger("Setup", "SaveHumanRcdTime", M2Share.g_Config.dwSaveHumanRcdTime);
            M2Share.g_Config.dwSaveHumanRcdTime = ReadInteger("Setup", "SaveHumanRcdTime", M2Share.g_Config.dwSaveHumanRcdTime);
            if (ReadInteger("Setup", "HumanFreeDelayTime", -1) < 0)
                WriteInteger("Setup", "HumanFreeDelayTime", M2Share.g_Config.dwHumanFreeDelayTime);
            if (ReadInteger("Setup", "MakeGhostTime", -1) < 0)
                WriteInteger("Setup", "MakeGhostTime", M2Share.g_Config.dwMakeGhostTime);
            M2Share.g_Config.dwMakeGhostTime = ReadInteger("Setup", "MakeGhostTime", M2Share.g_Config.dwMakeGhostTime);
            if (ReadInteger("Setup", "ClearDropOnFloorItemTime", -1) < 0)
                WriteInteger("Setup", "ClearDropOnFloorItemTime", M2Share.g_Config.dwClearDropOnFloorItemTime);
            M2Share.g_Config.dwClearDropOnFloorItemTime = ReadInteger("Setup", "ClearDropOnFloorItemTime", M2Share.g_Config.dwClearDropOnFloorItemTime);
            if (ReadInteger("Setup", "FloorItemCanPickUpTime", -1) < 0)
                WriteInteger("Setup", "FloorItemCanPickUpTime", M2Share.g_Config.dwFloorItemCanPickUpTime);
            M2Share.g_Config.dwFloorItemCanPickUpTime = ReadInteger("Setup", "FloorItemCanPickUpTime", M2Share.g_Config.dwFloorItemCanPickUpTime);
            if (ReadInteger("Setup", "PasswordLockSystem", -1) < 0)
                WriteBool("Setup", "PasswordLockSystem", M2Share.g_Config.boPasswordLockSystem);
            M2Share.g_Config.boPasswordLockSystem = ReadBool("Setup", "PasswordLockSystem", M2Share.g_Config.boPasswordLockSystem);
            if (ReadInteger("Setup", "PasswordLockDealAction", -1) < 0)
                WriteBool("Setup", "PasswordLockDealAction", M2Share.g_Config.boLockDealAction);
            M2Share.g_Config.boLockDealAction = ReadBool("Setup", "PasswordLockDealAction", M2Share.g_Config.boLockDealAction);
            if (ReadInteger("Setup", "PasswordLockDropAction", -1) < 0)
                WriteBool("Setup", "PasswordLockDropAction", M2Share.g_Config.boLockDropAction);
            M2Share.g_Config.boLockDropAction = ReadBool("Setup", "PasswordLockDropAction", M2Share.g_Config.boLockDropAction);
            if (ReadInteger("Setup", "PasswordLockGetBackItemAction", -1) < 0)
                WriteBool("Setup", "PasswordLockGetBackItemAction", M2Share.g_Config.boLockGetBackItemAction);
            M2Share.g_Config.boLockGetBackItemAction = ReadBool("Setup", "PasswordLockGetBackItemAction", M2Share.g_Config.boLockGetBackItemAction);
            if (ReadInteger("Setup", "PasswordLockHumanLogin", -1) < 0)
                WriteBool("Setup", "PasswordLockHumanLogin", M2Share.g_Config.boLockHumanLogin);
            M2Share.g_Config.boLockHumanLogin = ReadBool("Setup", "PasswordLockHumanLogin", M2Share.g_Config.boLockHumanLogin);
            if (ReadInteger("Setup", "PasswordLockWalkAction", -1) < 0)
                WriteBool("Setup", "PasswordLockWalkAction", M2Share.g_Config.boLockWalkAction);
            M2Share.g_Config.boLockWalkAction = ReadBool("Setup", "PasswordLockWalkAction", M2Share.g_Config.boLockWalkAction);
            if (ReadInteger("Setup", "PasswordLockRunAction", -1) < 0)
                WriteBool("Setup", "PasswordLockRunAction", M2Share.g_Config.boLockRunAction);
            M2Share.g_Config.boLockRunAction = ReadBool("Setup", "PasswordLockRunAction", M2Share.g_Config.boLockRunAction);
            if (ReadInteger("Setup", "PasswordLockHitAction", -1) < 0)
                WriteBool("Setup", "PasswordLockHitAction", M2Share.g_Config.boLockHitAction);
            M2Share.g_Config.boLockHitAction = ReadBool("Setup", "PasswordLockHitAction", M2Share.g_Config.boLockHitAction);
            if (ReadInteger("Setup", "PasswordLockSpellAction", -1) < 0)
                WriteBool("Setup", "PasswordLockSpellAction", M2Share.g_Config.boLockSpellAction);
            M2Share.g_Config.boLockSpellAction = ReadBool("Setup", "PasswordLockSpellAction", M2Share.g_Config.boLockSpellAction);
            if (ReadInteger("Setup", "PasswordLockSendMsgAction", -1) < 0)
                WriteBool("Setup", "PasswordLockSendMsgAction", M2Share.g_Config.boLockSendMsgAction);
            M2Share.g_Config.boLockSendMsgAction = ReadBool("Setup", "PasswordLockSendMsgAction", M2Share.g_Config.boLockSendMsgAction);
            if (ReadInteger("Setup", "PasswordLockUserItemAction", -1) < 0)
                WriteBool("Setup", "PasswordLockUserItemAction", M2Share.g_Config.boLockUserItemAction);
            M2Share.g_Config.boLockUserItemAction = ReadBool("Setup", "PasswordLockUserItemAction", M2Share.g_Config.boLockUserItemAction);
            if (ReadInteger("Setup", "PasswordLockInObModeAction", -1) < 0)
                WriteBool("Setup", "PasswordLockInObModeAction", M2Share.g_Config.boLockInObModeAction);
            M2Share.g_Config.boLockInObModeAction = ReadBool("Setup", "PasswordLockInObModeAction", M2Share.g_Config.boLockInObModeAction);
            if (ReadInteger("Setup", "PasswordErrorKick", -1) < 0)
                WriteBool("Setup", "PasswordErrorKick", M2Share.g_Config.boPasswordErrorKick);
            M2Share.g_Config.boPasswordErrorKick = ReadBool("Setup", "PasswordErrorKick", M2Share.g_Config.boPasswordErrorKick);
            if (ReadInteger("Setup", "PasswordErrorCountLock", -1) < 0)
                WriteInteger("Setup", "PasswordErrorCountLock", M2Share.g_Config.nPasswordErrorCountLock);
            M2Share.g_Config.nPasswordErrorCountLock = ReadInteger("Setup", "PasswordErrorCountLock", M2Share.g_Config.nPasswordErrorCountLock);
            if (ReadInteger("Setup", "SoftVersionDate", -1) < 0)
                WriteInteger("Setup", "SoftVersionDate", M2Share.g_Config.nSoftVersionDate);
            M2Share.g_Config.nSoftVersionDate = ReadInteger("Setup", "SoftVersionDate", M2Share.g_Config.nSoftVersionDate);
            nLoadInteger = ReadInteger("Setup", "CanOldClientLogon", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "CanOldClientLogon", M2Share.g_Config.boCanOldClientLogon);
            else
                M2Share.g_Config.boCanOldClientLogon = nLoadInteger == 1;
            if (ReadInteger("Setup", "ConsoleShowUserCountTime", -1) < 0)
                WriteInteger("Setup", "ConsoleShowUserCountTime", M2Share.g_Config.dwConsoleShowUserCountTime);
            M2Share.g_Config.dwConsoleShowUserCountTime = ReadInteger("Setup", "ConsoleShowUserCountTime", M2Share.g_Config.dwConsoleShowUserCountTime);
            if (ReadInteger("Setup", "ShowLineNoticeTime", -1) < 0)
                WriteInteger("Setup", "ShowLineNoticeTime", M2Share.g_Config.dwShowLineNoticeTime);
            M2Share.g_Config.dwShowLineNoticeTime = ReadInteger("Setup", "ShowLineNoticeTime", M2Share.g_Config.dwShowLineNoticeTime);
            if (ReadInteger("Setup", "LineNoticeColor", -1) < 0)
                WriteInteger("Setup", "LineNoticeColor", M2Share.g_Config.nLineNoticeColor);
            M2Share.g_Config.nLineNoticeColor = ReadInteger("Setup", "LineNoticeColor", M2Share.g_Config.nLineNoticeColor);
            if (ReadInteger("Setup", "ItemSpeedTime", -1) < 0)
                WriteInteger("Setup", "ItemSpeedTime", M2Share.g_Config.ClientConf.btItemSpeed);
            M2Share.g_Config.ClientConf.btItemSpeed = Read<byte>("Setup", "ItemSpeedTime", M2Share.g_Config.ClientConf.btItemSpeed);
            if (ReadInteger("Setup", "MaxHitMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxHitMsgCount", M2Share.g_Config.nMaxHitMsgCount);
            M2Share.g_Config.nMaxHitMsgCount = ReadInteger("Setup", "MaxHitMsgCount", M2Share.g_Config.nMaxHitMsgCount);
            if (ReadInteger("Setup", "MaxSpellMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxSpellMsgCount", M2Share.g_Config.nMaxSpellMsgCount);
            M2Share.g_Config.nMaxSpellMsgCount = ReadInteger("Setup", "MaxSpellMsgCount", M2Share.g_Config.nMaxSpellMsgCount);
            if (ReadInteger("Setup", "MaxRunMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxRunMsgCount", M2Share.g_Config.nMaxRunMsgCount);
            M2Share.g_Config.nMaxRunMsgCount = ReadInteger("Setup", "MaxRunMsgCount", M2Share.g_Config.nMaxRunMsgCount);
            if (ReadInteger("Setup", "MaxWalkMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxWalkMsgCount", M2Share.g_Config.nMaxWalkMsgCount);
            M2Share.g_Config.nMaxWalkMsgCount = ReadInteger("Setup", "MaxWalkMsgCount", M2Share.g_Config.nMaxWalkMsgCount);
            if (ReadInteger("Setup", "MaxTurnMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxTurnMsgCount", M2Share.g_Config.nMaxTurnMsgCount);
            M2Share.g_Config.nMaxTurnMsgCount = ReadInteger("Setup", "MaxTurnMsgCount", M2Share.g_Config.nMaxTurnMsgCount);
            if (ReadInteger("Setup", "MaxSitDonwMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxSitDonwMsgCount", M2Share.g_Config.nMaxSitDonwMsgCount);
            M2Share.g_Config.nMaxSitDonwMsgCount = ReadInteger("Setup", "MaxSitDonwMsgCount", M2Share.g_Config.nMaxSitDonwMsgCount);
            if (ReadInteger("Setup", "MaxDigUpMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxDigUpMsgCount", M2Share.g_Config.nMaxDigUpMsgCount);
            M2Share.g_Config.nMaxDigUpMsgCount = ReadInteger("Setup", "MaxDigUpMsgCount", M2Share.g_Config.nMaxDigUpMsgCount);
            if (ReadInteger("Setup", "SpellSendUpdateMsg", -1) < 0)
                WriteBool("Setup", "SpellSendUpdateMsg", M2Share.g_Config.boSpellSendUpdateMsg);
            M2Share.g_Config.boSpellSendUpdateMsg = ReadBool("Setup", "SpellSendUpdateMsg", M2Share.g_Config.boSpellSendUpdateMsg);
            if (ReadInteger("Setup", "ActionSendActionMsg", -1) < 0)
                WriteBool("Setup", "ActionSendActionMsg", M2Share.g_Config.boActionSendActionMsg);
            M2Share.g_Config.boActionSendActionMsg = ReadBool("Setup", "ActionSendActionMsg", M2Share.g_Config.boActionSendActionMsg);
            if (ReadInteger("Setup", "OverSpeedKickCount", -1) < 0)
                WriteInteger("Setup", "OverSpeedKickCount", M2Share.g_Config.nOverSpeedKickCount);
            M2Share.g_Config.nOverSpeedKickCount = ReadInteger("Setup", "OverSpeedKickCount", M2Share.g_Config.nOverSpeedKickCount);
            if (ReadInteger("Setup", "DropOverSpeed", -1) < 0)
                WriteInteger("Setup", "DropOverSpeed", M2Share.g_Config.dwDropOverSpeed);
            M2Share.g_Config.dwDropOverSpeed = ReadInteger("Setup", "DropOverSpeed", M2Share.g_Config.dwDropOverSpeed);
            if (ReadInteger("Setup", "KickOverSpeed", -1) < 0)
                WriteBool("Setup", "KickOverSpeed", M2Share.g_Config.boKickOverSpeed);
            M2Share.g_Config.boKickOverSpeed = ReadBool("Setup", "KickOverSpeed", M2Share.g_Config.boKickOverSpeed);
            nLoadInteger = ReadInteger("Setup", "SpeedControlMode", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "SpeedControlMode", M2Share.g_Config.btSpeedControlMode);
            else
                M2Share.g_Config.btSpeedControlMode = nLoadInteger;
            if (ReadInteger("Setup", "HitIntervalTime", -1) < 0)
                WriteInteger("Setup", "HitIntervalTime", M2Share.g_Config.dwHitIntervalTime);
            M2Share.g_Config.dwHitIntervalTime =
                ReadInteger("Setup", "HitIntervalTime", M2Share.g_Config.dwHitIntervalTime);
            if (ReadInteger("Setup", "MagicHitIntervalTime", -1) < 0)
                WriteInteger("Setup", "MagicHitIntervalTime", M2Share.g_Config.dwMagicHitIntervalTime);
            M2Share.g_Config.dwMagicHitIntervalTime = ReadInteger("Setup", "MagicHitIntervalTime", M2Share.g_Config.dwMagicHitIntervalTime);
            if (ReadInteger("Setup", "RunIntervalTime", -1) < 0)
                WriteInteger("Setup", "RunIntervalTime", M2Share.g_Config.dwRunIntervalTime);
            M2Share.g_Config.dwRunIntervalTime = ReadInteger("Setup", "RunIntervalTime", M2Share.g_Config.dwRunIntervalTime);
            if (ReadInteger("Setup", "WalkIntervalTime", -1) < 0)
                WriteInteger("Setup", "WalkIntervalTime", M2Share.g_Config.dwWalkIntervalTime);
            M2Share.g_Config.dwWalkIntervalTime = ReadInteger("Setup", "WalkIntervalTime", M2Share.g_Config.dwWalkIntervalTime);
            if (ReadInteger("Setup", "TurnIntervalTime", -1) < 0)
                WriteInteger("Setup", "TurnIntervalTime", M2Share.g_Config.dwTurnIntervalTime);
            M2Share.g_Config.dwTurnIntervalTime = ReadInteger("Setup", "TurnIntervalTime", M2Share.g_Config.dwTurnIntervalTime);
            nLoadInteger = ReadInteger("Setup", "ControlActionInterval", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "ControlActionInterval", M2Share.g_Config.boControlActionInterval);
            else
                M2Share.g_Config.boControlActionInterval = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "ControlWalkHit", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "ControlWalkHit", M2Share.g_Config.boControlWalkHit);
            else
                M2Share.g_Config.boControlWalkHit = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "ControlRunLongHit", -1);
            if (nLoadInteger < 0)
            {
                WriteBool("Setup", "ControlRunLongHit", M2Share.g_Config.boControlRunLongHit);
            }
            else
            {
                M2Share.g_Config.boControlRunLongHit = nLoadInteger == 1;
            }
            nLoadInteger = ReadInteger("Setup", "ControlRunHit", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "ControlRunHit", M2Share.g_Config.boControlRunHit);
            else
                M2Share.g_Config.boControlRunHit = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "ControlRunMagic", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "ControlRunMagic", M2Share.g_Config.boControlRunMagic);
            else
                M2Share.g_Config.boControlRunMagic = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "ActionIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "ActionIntervalTime", M2Share.g_Config.dwActionIntervalTime);
            else
                M2Share.g_Config.dwActionIntervalTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "RunLongHitIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "RunLongHitIntervalTime", M2Share.g_Config.dwRunLongHitIntervalTime);
            else
                M2Share.g_Config.dwRunLongHitIntervalTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "RunHitIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "RunHitIntervalTime", M2Share.g_Config.dwRunHitIntervalTime);
            else
                M2Share.g_Config.dwRunHitIntervalTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "WalkHitIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "WalkHitIntervalTime", M2Share.g_Config.dwWalkHitIntervalTime);
            else
                M2Share.g_Config.dwWalkHitIntervalTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "RunMagicIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "RunMagicIntervalTime", M2Share.g_Config.dwRunMagicIntervalTime);
            else
                M2Share.g_Config.dwRunMagicIntervalTime = nLoadInteger;
            if (ReadInteger("Setup", "DisableStruck", -1) < 0)
                WriteBool("Setup", "DisableStruck", M2Share.g_Config.boDisableStruck);
            M2Share.g_Config.boDisableStruck =
                ReadBool("Setup", "DisableStruck", M2Share.g_Config.boDisableStruck);
            if (ReadInteger("Setup", "DisableSelfStruck", -1) < 0)
                WriteBool("Setup", "DisableSelfStruck", M2Share.g_Config.boDisableSelfStruck);
            M2Share.g_Config.boDisableSelfStruck =
                ReadBool("Setup", "DisableSelfStruck", M2Share.g_Config.boDisableSelfStruck);
            if (ReadInteger("Setup", "StruckTime", -1) < 0)
                WriteInteger("Setup", "StruckTime", M2Share.g_Config.dwStruckTime);
            M2Share.g_Config.dwStruckTime = ReadInteger("Setup", "StruckTime", M2Share.g_Config.dwStruckTime);
            nLoadInteger = ReadInteger("Setup", "AddUserItemNewValue", -1);
            if (nLoadInteger < 0)
            {
                WriteBool("Setup", "AddUserItemNewValue", M2Share.g_Config.boAddUserItemNewValue);
            }
            else
            {
                M2Share.g_Config.boAddUserItemNewValue = nLoadInteger == 1;
            }
            nLoadInteger = ReadInteger("Setup", "TestSpeedMode", -1);
            if (nLoadInteger < 0)
            {
                WriteBool("Setup", "TestSpeedMode", M2Share.g_Config.boTestSpeedMode);
            }
            else
            {
                M2Share.g_Config.boTestSpeedMode = nLoadInteger == 1;
            }
            // 气血石开始
            if (ReadInteger("Setup", "HPStoneStartRate", -1) < 0)
            {
                WriteInteger("Setup", "HPStoneStartRate", M2Share.g_Config.HPStoneStartRate);
            }
            M2Share.g_Config.HPStoneStartRate = Read<byte>("Setup", "HPStoneStartRate", M2Share.g_Config.HPStoneStartRate);
            if (ReadInteger("Setup", "MPStoneStartRate", -1) < 0)
            {
                WriteInteger("Setup", "MPStoneStartRate", M2Share.g_Config.MPStoneStartRate);
            }
            M2Share.g_Config.MPStoneStartRate = Read<byte>("Setup", "MPStoneStartRate", M2Share.g_Config.MPStoneStartRate);
            if (ReadInteger("Setup", "HPStoneIntervalTime", -1) < 0)
            {
                WriteInteger("Setup", "HPStoneIntervalTime", M2Share.g_Config.HPStoneIntervalTime);
            }
            M2Share.g_Config.HPStoneIntervalTime = ReadInteger("Setup", "HPStoneIntervalTime", M2Share.g_Config.HPStoneIntervalTime);
            if (ReadInteger("Setup", "MPStoneIntervalTime", -1) < 0)
            {
                WriteInteger("Setup", "MPStoneIntervalTime", M2Share.g_Config.MPStoneIntervalTime);
            }
            M2Share.g_Config.MPStoneIntervalTime = ReadInteger("Setup", "MPStoneIntervalTime", M2Share.g_Config.MPStoneIntervalTime);
            if (ReadInteger("Setup", "HPStoneAddRate", -1) < 0)
            {
                WriteInteger("Setup", "HPStoneAddRate", M2Share.g_Config.HPStoneAddRate);
            }
            M2Share.g_Config.HPStoneAddRate = Read<byte>("Setup", "HPStoneAddRate", M2Share.g_Config.HPStoneAddRate);
            if (ReadInteger("Setup", "MPStoneAddRate", -1) < 0)
            {
                WriteInteger("Setup", "MPStoneAddRate", M2Share.g_Config.MPStoneAddRate);
            }
            M2Share.g_Config.MPStoneAddRate = Read<byte>("Setup", "MPStoneAddRate", M2Share.g_Config.MPStoneAddRate);
            if (ReadInteger("Setup", "HPStoneDecDura", -1) < 0)
            {
                WriteInteger("Setup", "HPStoneDecDura", M2Share.g_Config.HPStoneDecDura);
            }
            M2Share.g_Config.HPStoneDecDura = ReadInteger("Setup", "HPStoneDecDura", M2Share.g_Config.HPStoneDecDura);
            if (ReadInteger("Setup", "MPStoneDecDura", -1) < 0)
            {
                WriteInteger("Setup", "MPStoneDecDura", M2Share.g_Config.MPStoneDecDura);
            }
            M2Share.g_Config.MPStoneDecDura = ReadInteger("Setup", "MPStoneDecDura", M2Share.g_Config.MPStoneDecDura);

            // 气血石结束
            nLoadInteger = ReadInteger("Setup", "WeaponMakeUnLuckRate", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeUnLuckRate", M2Share.g_Config.nWeaponMakeUnLuckRate);
            }
            else
            {
                M2Share.g_Config.nWeaponMakeUnLuckRate = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WeaponMakeLuckPoint1", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint1", M2Share.g_Config.nWeaponMakeLuckPoint1);
            }
            else
            {
                M2Share.g_Config.nWeaponMakeLuckPoint1 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WeaponMakeLuckPoint2", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint2", M2Share.g_Config.nWeaponMakeLuckPoint2);
            }
            else
            {
                M2Share.g_Config.nWeaponMakeLuckPoint2 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WeaponMakeLuckPoint3", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint3", M2Share.g_Config.nWeaponMakeLuckPoint3);
            }
            else
            {
                M2Share.g_Config.nWeaponMakeLuckPoint3 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WeaponMakeLuckPoint2Rate", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint2Rate", M2Share.g_Config.nWeaponMakeLuckPoint2Rate);
            }
            else
            {
                M2Share.g_Config.nWeaponMakeLuckPoint2Rate = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WeaponMakeLuckPoint3Rate", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint3Rate", M2Share.g_Config.nWeaponMakeLuckPoint3Rate);
            }
            else
            {
                M2Share.g_Config.nWeaponMakeLuckPoint3Rate = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "CheckUserItemPlace", -1);
            if (nLoadInteger < 0)
            {
                WriteBool("Setup", "CheckUserItemPlace", M2Share.g_Config.boCheckUserItemPlace);
            }
            else
            {
                M2Share.g_Config.boCheckUserItemPlace = nLoadInteger == 1;
            }
            nLoadInteger = ReadInteger("Setup", "LevelValueOfTaosHP", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "LevelValueOfTaosHP", M2Share.g_Config.nLevelValueOfTaosHP);
            }
            else
            {
                M2Share.g_Config.nLevelValueOfTaosHP = nLoadInteger;
            }
            var nLoadFloatRate = Read<double>("Setup", "LevelValueOfTaosHPRate", 0);
            if (nLoadFloatRate == 0)
            {
                WriteInteger("Setup", "LevelValueOfTaosHPRate", M2Share.g_Config.nLevelValueOfTaosHPRate);
            }
            else
            {
                M2Share.g_Config.nLevelValueOfTaosHPRate = nLoadFloatRate;
            }
            nLoadInteger = ReadInteger("Setup", "LevelValueOfTaosMP", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "LevelValueOfTaosMP", M2Share.g_Config.nLevelValueOfTaosMP);
            }
            else
            {
                M2Share.g_Config.nLevelValueOfTaosMP = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "LevelValueOfWizardHP", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "LevelValueOfWizardHP", M2Share.g_Config.nLevelValueOfWizardHP);
            }
            else
            {
                M2Share.g_Config.nLevelValueOfWizardHP = nLoadInteger;
            }
            nLoadFloatRate = Read<double>("Setup", "LevelValueOfWizardHPRate", 0);
            if (nLoadFloatRate == 0)
            {
                WriteInteger("Setup", "LevelValueOfWizardHPRate", M2Share.g_Config.nLevelValueOfWizardHPRate);
            }
            else
            {
                M2Share.g_Config.nLevelValueOfWizardHPRate = nLoadFloatRate;
            }
            nLoadInteger = ReadInteger("Setup", "LevelValueOfWarrHP", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "LevelValueOfWarrHP", M2Share.g_Config.nLevelValueOfWarrHP);
            }
            else
            {
                M2Share.g_Config.nLevelValueOfWarrHP = nLoadInteger;
            }
            nLoadFloatRate = Read<double>("Setup", "LevelValueOfWarrHPRate", 0);
            if (nLoadFloatRate == 0)
            {
                WriteInteger("Setup", "LevelValueOfWarrHPRate", M2Share.g_Config.nLevelValueOfWarrHPRate);
            }
            else
            {
                M2Share.g_Config.nLevelValueOfWarrHPRate = nLoadFloatRate;
            }
            nLoadInteger = ReadInteger("Setup", "ProcessMonsterInterval", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "ProcessMonsterInterval", M2Share.g_Config.nProcessMonsterInterval);
            }
            else
            {
                M2Share.g_Config.nProcessMonsterInterval = nLoadInteger;
            }
            if (ReadInteger("Setup", "StartCastleWarDays", -1) < 0)
            {
                WriteInteger("Setup", "StartCastleWarDays", M2Share.g_Config.nStartCastleWarDays);
            }
            M2Share.g_Config.nStartCastleWarDays = ReadInteger("Setup", "StartCastleWarDays", M2Share.g_Config.nStartCastleWarDays);
            if (ReadInteger("Setup", "StartCastlewarTime", -1) < 0)
            {
                WriteInteger("Setup", "StartCastlewarTime", M2Share.g_Config.nStartCastlewarTime);
            }
            M2Share.g_Config.nStartCastlewarTime = ReadInteger("Setup", "StartCastlewarTime", M2Share.g_Config.nStartCastlewarTime);
            if (ReadInteger("Setup", "ShowCastleWarEndMsgTime", -1) < 0)
            {
                WriteInteger("Setup", "ShowCastleWarEndMsgTime", M2Share.g_Config.dwShowCastleWarEndMsgTime);
            }
            M2Share.g_Config.dwShowCastleWarEndMsgTime = ReadInteger("Setup", "ShowCastleWarEndMsgTime", M2Share.g_Config.dwShowCastleWarEndMsgTime);
            if (ReadInteger("Server", "ClickNPCTime", -1) < 0)
            {
                WriteInteger("Server", "ClickNPCTime", M2Share.g_Config.dwClickNpcTime);
            }
            M2Share.g_Config.dwClickNpcTime = ReadInteger("Server", "ClickNPCTime", M2Share.g_Config.dwClickNpcTime);
            if (ReadInteger("Setup", "CastleWarTime", -1) < 0)
            {
                WriteInteger("Setup", "CastleWarTime", M2Share.g_Config.dwCastleWarTime);
            }
            M2Share.g_Config.dwCastleWarTime = ReadInteger("Setup", "CastleWarTime", M2Share.g_Config.dwCastleWarTime);
            nLoadInteger = ReadInteger("Setup", "GetCastleTime", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "GetCastleTime", M2Share.g_Config.dwGetCastleTime);
            }
            else
            {
                M2Share.g_Config.dwGetCastleTime = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "GuildWarTime", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "GuildWarTime", M2Share.g_Config.dwGuildWarTime);
            }
            else
            {
                M2Share.g_Config.dwGuildWarTime = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryCount", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryCount", M2Share.g_Config.nWinLotteryCount);
            }
            else
            {
                M2Share.g_Config.nWinLotteryCount = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "NoWinLotteryCount", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "NoWinLotteryCount", M2Share.g_Config.nNoWinLotteryCount);
            }
            else
            {
                M2Share.g_Config.nNoWinLotteryCount = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel1", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel1", M2Share.g_Config.nWinLotteryLevel1);
            }
            else
            {
                M2Share.g_Config.nWinLotteryLevel1 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel2", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel2", M2Share.g_Config.nWinLotteryLevel2);
            }
            else
            {
                M2Share.g_Config.nWinLotteryLevel2 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel3", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel3", M2Share.g_Config.nWinLotteryLevel3);
            }
            else
            {
                M2Share.g_Config.nWinLotteryLevel3 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel4", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel4", M2Share.g_Config.nWinLotteryLevel4);
            }
            else
            {
                M2Share.g_Config.nWinLotteryLevel4 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel5", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel5", M2Share.g_Config.nWinLotteryLevel5);
            }
            else
            {
                M2Share.g_Config.nWinLotteryLevel5 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel6", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel6", M2Share.g_Config.nWinLotteryLevel6);
            }
            else
            {
                M2Share.g_Config.nWinLotteryLevel6 = nLoadInteger;
            }
        }

        /// <summary>
        /// 保存游戏变量和彩票中奖数据
        /// </summary>
        public void SaveVariable()
        {
            WriteInteger("Setup", "ItemNumber", M2Share.g_Config.nItemNumber);
            WriteInteger("Setup", "ItemNumberEx", M2Share.g_Config.nItemNumberEx);
            WriteInteger("Setup", "WinLotteryCount", M2Share.g_Config.nWinLotteryCount);
            WriteInteger("Setup", "NoWinLotteryCount", M2Share.g_Config.nNoWinLotteryCount);
            WriteInteger("Setup", "WinLotteryLevel1", M2Share.g_Config.nWinLotteryLevel1);
            WriteInteger("Setup", "WinLotteryLevel2", M2Share.g_Config.nWinLotteryLevel2);
            WriteInteger("Setup", "WinLotteryLevel3", M2Share.g_Config.nWinLotteryLevel3);
            WriteInteger("Setup", "WinLotteryLevel4", M2Share.g_Config.nWinLotteryLevel4);
            WriteInteger("Setup", "WinLotteryLevel5", M2Share.g_Config.nWinLotteryLevel5);
            WriteInteger("Setup", "WinLotteryLevel6", M2Share.g_Config.nWinLotteryLevel6);
        }

    }
}