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
                WriteString("DataBase", "ConnctionString", M2Share.Config.sConnctionString);
            }
            M2Share.Config.sConnctionString = ReadString("DataBase", "ConnctionString", M2Share.Config.sConnctionString);
            //数据库类型设置
            if (ReadString("DataBase", "DbType", "") == "")
            {
                WriteString("DataBase", "DbType", M2Share.Config.sDBType);
            }
            M2Share.Config.sDBType = ReadString("DataBase", "DbType", M2Share.Config.sDBType);
            // 服务器设置
            if (ReadInteger("Server", "ServerIndex", -1) < 0)
            {
                WriteInteger("Server", "ServerIndex", M2Share.ServerIndex);
            }
            M2Share.ServerIndex = ReadInteger("Server", "ServerIndex", M2Share.ServerIndex);
            if (ReadString("Server", "ServerName", "") == "")
            {
                WriteString("Server", "ServerName", M2Share.Config.sServerName);
            }
            M2Share.Config.sServerName = ReadString("Server", "ServerName", M2Share.Config.sServerName);
            if (ReadInteger("Server", "ServerNumber", -1) < 0)
                WriteInteger("Server", "ServerNumber", M2Share.Config.nServerNumber);
            M2Share.Config.nServerNumber = ReadInteger("Server", "ServerNumber", M2Share.Config.nServerNumber);
            if (ReadString("Server", "VentureServer", "") == "")
                WriteString("Server", "VentureServer", HUtil32.BoolToStr(M2Share.Config.boVentureServer));
            M2Share.Config.boVentureServer = String.Compare(ReadString("Server", "VentureServer", "FALSE"), "TRUE", StringComparison.Ordinal) == 0;
            if (ReadString("Server", "TestServer", "") == "")
                WriteString("Server", "TestServer", HUtil32.BoolToStr(M2Share.Config.boTestServer));
            M2Share.Config.boTestServer = String.Compare(ReadString("Server", "TestServer", "FALSE"), "TRUE", StringComparison.Ordinal) == 0;
            if (ReadInteger("Server", "TestLevel", -1) < 0)
                WriteInteger("Server", "TestLevel", M2Share.Config.nTestLevel);
            M2Share.Config.nTestLevel = ReadInteger("Server", "TestLevel", M2Share.Config.nTestLevel);
            if (ReadInteger("Server", "TestGold", -1) < 0)
                WriteInteger("Server", "TestGold", M2Share.Config.nTestGold);
            M2Share.Config.nTestGold = ReadInteger("Server", "TestGold", M2Share.Config.nTestGold);
            if (ReadInteger("Server", "TestServerUserLimit", -1) < 0)
                WriteInteger("Server", "TestServerUserLimit", M2Share.Config.nTestUserLimit);
            M2Share.Config.nTestUserLimit = ReadInteger("Server", "TestServerUserLimit", M2Share.Config.nTestUserLimit);
            if (ReadString("Server", "ServiceMode", "") == "")
                WriteString("Server", "ServiceMode", HUtil32.BoolToStr(M2Share.Config.boServiceMode));
            M2Share.Config.boServiceMode = ReadString("Server", "ServiceMode", "FALSE").CompareTo("TRUE") == 0;
            if (ReadString("Server", "NonPKServer", "") == "")
                WriteString("Server", "NonPKServer", HUtil32.BoolToStr(M2Share.Config.boNonPKServer));
            M2Share.Config.boNonPKServer = ReadString("Server", "NonPKServer", "FALSE").CompareTo("TRUE") == 0;
            if (ReadString("Server", "ViewHackMessage", "") == "")
                WriteString("Server", "ViewHackMessage", HUtil32.BoolToStr(M2Share.Config.boViewHackMessage));
            M2Share.Config.boViewHackMessage = ReadString("Server", "ViewHackMessage", "FALSE").CompareTo("TRUE") == 0;
            if (ReadString("Server", "ViewAdmissionFailure", "") == "")
            {
                WriteString("Server", "ViewAdmissionFailure", HUtil32.BoolToStr(M2Share.Config.boViewAdmissionFailure));
            }
            M2Share.Config.boViewAdmissionFailure = ReadString("Server", "ViewAdmissionFailure", "FALSE").CompareTo("TRUE") == 0;
            if (ReadString("Server", "GateAddr", "") == "")
            {
                WriteString("Server", "GateAddr", M2Share.Config.sGateAddr);
            }
            M2Share.Config.sGateAddr = ReadString("Server", "GateAddr", M2Share.Config.sGateAddr);
            if (ReadInteger("Server", "GatePort", -1) < 0)
            {
                WriteInteger("Server", "GatePort", M2Share.Config.nGatePort);
            }
            M2Share.Config.nGatePort = ReadInteger("Server", "GatePort", M2Share.Config.nGatePort);
            if (ReadString("Server", "DBAddr", "") == "")
                WriteString("Server", "DBAddr", M2Share.Config.sDBAddr);
            M2Share.Config.sDBAddr = ReadString("Server", "DBAddr", M2Share.Config.sDBAddr);
            if (ReadInteger("Server", "DBPort", -1) < 0)
                WriteInteger("Server", "DBPort", M2Share.Config.nDBPort);
            M2Share.Config.nDBPort = ReadInteger("Server", "DBPort", M2Share.Config.nDBPort);
            if (ReadString("Server", "IDSAddr", "") == "")
                WriteString("Server", "IDSAddr", M2Share.Config.sIDSAddr);
            M2Share.Config.sIDSAddr = ReadString("Server", "IDSAddr", M2Share.Config.sIDSAddr);
            if (ReadInteger("Server", "IDSPort", -1) < 0)
                WriteInteger("Server", "IDSPort", M2Share.Config.nIDSPort);
            M2Share.Config.nIDSPort = ReadInteger("Server", "IDSPort", M2Share.Config.nIDSPort);
            if (ReadString("Server", "MsgSrvAddr", "") == "")
                WriteString("Server", "MsgSrvAddr", M2Share.Config.sMsgSrvAddr);
            M2Share.Config.sMsgSrvAddr = ReadString("Server", "MsgSrvAddr", M2Share.Config.sMsgSrvAddr);
            if (ReadInteger("Server", "MsgSrvPort", -1) < 0)
                WriteInteger("Server", "MsgSrvPort", M2Share.Config.nMsgSrvPort);
            M2Share.Config.nMsgSrvPort = ReadInteger("Server", "MsgSrvPort", M2Share.Config.nMsgSrvPort);
            if (ReadString("Server", "LogServerAddr", "") == "")
                WriteString("Server", "LogServerAddr", M2Share.Config.sLogServerAddr);
            M2Share.Config.sLogServerAddr = ReadString("Server", "LogServerAddr", M2Share.Config.sLogServerAddr);
            if (ReadInteger("Server", "LogServerPort", -1) < 0)
                WriteInteger("Server", "LogServerPort", M2Share.Config.nLogServerPort);
            M2Share.Config.nLogServerPort = ReadInteger("Server", "LogServerPort", M2Share.Config.nLogServerPort);
            if (ReadString("Server", "DiscountForNightTime", "") == "")
                WriteString("Server", "DiscountForNightTime", HUtil32.BoolToStr(M2Share.Config.boDiscountForNightTime));
            M2Share.Config.boDiscountForNightTime = ReadString("Server", "DiscountForNightTime", "FALSE").CompareTo("TRUE".ToLower()) == 0;
            if (ReadInteger("Server", "HalfFeeStart", -1) < 0)
                WriteInteger("Server", "HalfFeeStart", M2Share.Config.nHalfFeeStart);
            M2Share.Config.nHalfFeeStart = ReadInteger("Server", "HalfFeeStart", M2Share.Config.nHalfFeeStart);
            if (ReadInteger("Server", "HalfFeeEnd", -1) < 0)
                WriteInteger("Server", "HalfFeeEnd", M2Share.Config.nHalfFeeEnd);
            M2Share.Config.nHalfFeeEnd = ReadInteger("Server", "HalfFeeEnd", M2Share.Config.nHalfFeeEnd);
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
                WriteInteger("Server", "SendBlock", M2Share.Config.nSendBlock);
            M2Share.Config.nSendBlock = ReadInteger("Server", "SendBlock", M2Share.Config.nSendBlock);
            if (ReadInteger("Server", "CheckBlock", -1) < 0)
                WriteInteger("Server", "CheckBlock", M2Share.Config.nCheckBlock);
            M2Share.Config.nCheckBlock = ReadInteger("Server", "CheckBlock", M2Share.Config.nCheckBlock);
            if (ReadInteger("Server", "SocCheckTimeOut", -1) < 0)
                WriteInteger("Server", "SocCheckTimeOut", M2Share.g_dwSocCheckTimeOut);
            M2Share.g_dwSocCheckTimeOut = ReadInteger("Server", "SocCheckTimeOut", M2Share.g_dwSocCheckTimeOut);
            if (ReadInteger("Server", "AvailableBlock", -1) < 0)
                WriteInteger("Server", "AvailableBlock", M2Share.Config.nAvailableBlock);
            M2Share.Config.nAvailableBlock = ReadInteger("Server", "AvailableBlock", M2Share.Config.nAvailableBlock);
            if (ReadInteger("Server", "GateLoad", -1) < 0)
                WriteInteger("Server", "GateLoad", M2Share.Config.nGateLoad);
            M2Share.Config.nGateLoad = ReadInteger("Server", "GateLoad", M2Share.Config.nGateLoad);
            if (ReadInteger("Server", "UserFull", -1) < 0)
                WriteInteger("Server", "UserFull", M2Share.Config.nUserFull);
            M2Share.Config.nUserFull = ReadInteger("Server", "UserFull", M2Share.Config.nUserFull);
            if (ReadInteger("Server", "ZenFastStep", -1) < 0)
                WriteInteger("Server", "ZenFastStep", M2Share.Config.nZenFastStep);
            M2Share.Config.nZenFastStep = ReadInteger("Server", "ZenFastStep", M2Share.Config.nZenFastStep);
            if (ReadInteger("Server", "ProcessMonstersTime", -1) < 0)
                WriteInteger("Server", "ProcessMonstersTime", M2Share.Config.dwProcessMonstersTime);
            M2Share.Config.dwProcessMonstersTime = ReadInteger("Server", "ProcessMonstersTime", M2Share.Config.dwProcessMonstersTime);
            if (ReadInteger("Server", "RegenMonstersTime", -1) < 0)
                WriteInteger("Server", "RegenMonstersTime", M2Share.Config.dwRegenMonstersTime);
            M2Share.Config.dwRegenMonstersTime = ReadInteger("Server", "RegenMonstersTime", M2Share.Config.dwRegenMonstersTime);
            if (ReadInteger("Server", "HumanGetMsgTimeLimit", -1) < 0)
                WriteInteger("Server", "HumanGetMsgTimeLimit", M2Share.Config.dwHumanGetMsgTime);
            M2Share.Config.dwHumanGetMsgTime = ReadInteger("Server", "HumanGetMsgTimeLimit", M2Share.Config.dwHumanGetMsgTime);
            if (ReadString("Share", "BaseDir", "") == "")
                WriteString("Share", "BaseDir", M2Share.Config.sBaseDir);
            M2Share.Config.sBaseDir = ReadString("Share", "BaseDir", M2Share.Config.sBaseDir);
            if (ReadString("Share", "GuildDir", "") == "")
                WriteString("Share", "GuildDir", M2Share.Config.sGuildDir);
            M2Share.Config.sGuildDir = ReadString("Share", "GuildDir", M2Share.Config.sGuildDir);
            if (ReadString("Share", "GuildFile", "") == "")
                WriteString("Share", "GuildFile", M2Share.Config.sGuildFile);
            M2Share.Config.sGuildFile = ReadString("Share", "GuildFile", M2Share.Config.sGuildFile);
            if (ReadString("Share", "VentureDir", "") == "")
                WriteString("Share", "VentureDir", M2Share.Config.sVentureDir);
            M2Share.Config.sVentureDir = ReadString("Share", "VentureDir", M2Share.Config.sVentureDir);
            if (ReadString("Share", "ConLogDir", "") == "")
                WriteString("Share", "ConLogDir", M2Share.Config.sConLogDir);
            M2Share.Config.sConLogDir = ReadString("Share", "ConLogDir", M2Share.Config.sConLogDir);
            if (ReadString("Share", "CastleDir", "") == "")
                WriteString("Share", "CastleDir", M2Share.Config.sCastleDir);
            M2Share.Config.sCastleDir = ReadString("Share", "CastleDir", M2Share.Config.sCastleDir);
            if (ReadString("Share", "CastleFile", "") == "")
                WriteString("Share", "CastleFile", M2Share.Config.sCastleDir + "List.txt");
            M2Share.Config.sCastleFile = ReadString("Share", "CastleFile", M2Share.Config.sCastleFile);
            if (ReadString("Share", "EnvirDir", "") == "")
            {
                WriteString("Share", "EnvirDir", M2Share.Config.sEnvirDir);
            }
            M2Share.Config.sEnvirDir = ReadString("Share", "EnvirDir", M2Share.Config.sEnvirDir);
            if (ReadString("Share", "MapDir", "") == "")
                WriteString("Share", "MapDir", M2Share.Config.sMapDir);
            M2Share.Config.sMapDir = ReadString("Share", "MapDir", M2Share.Config.sMapDir);
            if (ReadString("Share", "NoticeDir", "") == "")
            {
                WriteString("Share", "NoticeDir", M2Share.Config.sNoticeDir);
            }
            M2Share.Config.sNoticeDir = ReadString("Share", "NoticeDir", M2Share.Config.sNoticeDir);
            sLoadString = ReadString("Share", "LogDir", "");
            if (sLoadString == "")
            {
                WriteString("Share", "LogDir", M2Share.Config.sLogDir);
            }
            else
            {
                M2Share.Config.sLogDir = sLoadString;
            }
            // ============================================================================
            // 名称设置
            if (ReadString("Names", "HealSkill", "") == "")
                WriteString("Names", "HealSkill", M2Share.Config.sHealSkill);
            M2Share.Config.sHealSkill = ReadString("Names", "HealSkill", M2Share.Config.sHealSkill);
            if (ReadString("Names", "FireBallSkill", "") == "")
                WriteString("Names", "FireBallSkill", M2Share.Config.sFireBallSkill);
            M2Share.Config.sFireBallSkill = ReadString("Names", "FireBallSkill", M2Share.Config.sFireBallSkill);
            if (ReadString("Names", "ClothsMan", "") == "")
                WriteString("Names", "ClothsMan", M2Share.Config.sClothsMan);
            M2Share.Config.sClothsMan = ReadString("Names", "ClothsMan", M2Share.Config.sClothsMan);
            if (ReadString("Names", "ClothsWoman", "") == "")
                WriteString("Names", "ClothsWoman", M2Share.Config.sClothsWoman);
            M2Share.Config.sClothsWoman = ReadString("Names", "ClothsWoman", M2Share.Config.sClothsWoman);
            if (ReadString("Names", "WoodenSword", "") == "")
                WriteString("Names", "WoodenSword", M2Share.Config.sWoodenSword);
            M2Share.Config.sWoodenSword = ReadString("Names", "WoodenSword", M2Share.Config.sWoodenSword);
            if (ReadString("Names", "Candle", "") == "")
                WriteString("Names", "Candle", M2Share.Config.sCandle);
            M2Share.Config.sCandle = ReadString("Names", "Candle", M2Share.Config.sCandle);
            if (ReadString("Names", "BasicDrug", "") == "")
                WriteString("Names", "BasicDrug", M2Share.Config.sBasicDrug);
            M2Share.Config.sBasicDrug = ReadString("Names", "BasicDrug", M2Share.Config.sBasicDrug);
            if (ReadString("Names", "GoldStone", "") == "")
                WriteString("Names", "GoldStone", M2Share.Config.sGoldStone);
            M2Share.Config.sGoldStone = ReadString("Names", "GoldStone", M2Share.Config.sGoldStone);
            if (ReadString("Names", "SilverStone", "") == "")
                WriteString("Names", "SilverStone", M2Share.Config.sSilverStone);
            M2Share.Config.sSilverStone = ReadString("Names", "SilverStone", M2Share.Config.sSilverStone);
            if (ReadString("Names", "SteelStone", "") == "")
                WriteString("Names", "SteelStone", M2Share.Config.sSteelStone);
            M2Share.Config.sSteelStone = ReadString("Names", "SteelStone", M2Share.Config.sSteelStone);
            if (ReadString("Names", "CopperStone", "") == "")
                WriteString("Names", "CopperStone", M2Share.Config.sCopperStone);
            M2Share.Config.sCopperStone = ReadString("Names", "CopperStone", M2Share.Config.sCopperStone);
            if (ReadString("Names", "BlackStone", "") == "")
                WriteString("Names", "BlackStone", M2Share.Config.sBlackStone);
            M2Share.Config.sBlackStone = ReadString("Names", "BlackStone", M2Share.Config.sBlackStone);
            if (ReadString("Names", "Gem1Stone", "") == "")
                WriteString("Names", "Gem1Stone", M2Share.Config.sGemStone1);
            M2Share.Config.sGemStone1 = ReadString("Names", "Gem1Stone", M2Share.Config.sGemStone1);
            if (ReadString("Names", "Gem2Stone", "") == "")
                WriteString("Names", "Gem2Stone", M2Share.Config.sGemStone2);
            M2Share.Config.sGemStone2 = ReadString("Names", "Gem2Stone", M2Share.Config.sGemStone2);
            if (ReadString("Names", "Gem3Stone", "") == "")
                WriteString("Names", "Gem3Stone", M2Share.Config.sGemStone3);
            M2Share.Config.sGemStone3 = ReadString("Names", "Gem3Stone", M2Share.Config.sGemStone3);
            if (ReadString("Names", "Gem4Stone", "") == "")
                WriteString("Names", "Gem4Stone", M2Share.Config.sGemStone4);
            M2Share.Config.sGemStone4 = ReadString("Names", "Gem4Stone", M2Share.Config.sGemStone4);
            if (ReadString("Names", "Zuma1", "") == "")
                WriteString("Names", "Zuma1", M2Share.Config.sZuma[0]);
            M2Share.Config.sZuma[0] = ReadString("Names", "Zuma1", M2Share.Config.sZuma[0]);
            if (ReadString("Names", "Zuma2", "") == "")
                WriteString("Names", "Zuma2", M2Share.Config.sZuma[1]);
            M2Share.Config.sZuma[1] = ReadString("Names", "Zuma2", M2Share.Config.sZuma[1]);
            if (ReadString("Names", "Zuma3", "") == "")
                WriteString("Names", "Zuma3", M2Share.Config.sZuma[2]);
            M2Share.Config.sZuma[2] = ReadString("Names", "Zuma3", M2Share.Config.sZuma[2]);
            if (ReadString("Names", "Zuma4", "") == "")
                WriteString("Names", "Zuma4", M2Share.Config.sZuma[3]);
            M2Share.Config.sZuma[3] = ReadString("Names", "Zuma4", M2Share.Config.sZuma[3]);
            if (ReadString("Names", "Bee", "") == "")
                WriteString("Names", "Bee", M2Share.Config.sBee);
            M2Share.Config.sBee = ReadString("Names", "Bee", M2Share.Config.sBee);
            if (ReadString("Names", "Spider", "") == "")
                WriteString("Names", "Spider", M2Share.Config.sSpider);
            M2Share.Config.sSpider = ReadString("Names", "Spider", M2Share.Config.sSpider);
            if (ReadString("Names", "WomaHorn", "") == "")
                WriteString("Names", "WomaHorn", M2Share.Config.sWomaHorn);
            M2Share.Config.sWomaHorn = ReadString("Names", "WomaHorn", M2Share.Config.sWomaHorn);
            if (ReadString("Names", "ZumaPiece", "") == "")
                WriteString("Names", "ZumaPiece", M2Share.Config.sZumaPiece);
            M2Share.Config.sZumaPiece = ReadString("Names", "ZumaPiece", M2Share.Config.sZumaPiece);
            if (ReadString("Names", "Skeleton", "") == "")
                WriteString("Names", "Skeleton", M2Share.Config.sSkeleton);
            M2Share.Config.sSkeleton = ReadString("Names", "Skeleton", M2Share.Config.sSkeleton);
            if (ReadString("Names", "Dragon", "") == "")
                WriteString("Names", "Dragon", M2Share.Config.sDragon);
            M2Share.Config.sDragon = ReadString("Names", "Dragon", M2Share.Config.sDragon);
            if (ReadString("Names", "Dragon1", "") == "")
                WriteString("Names", "Dragon1", M2Share.Config.sDragon1);
            M2Share.Config.sDragon1 = ReadString("Names", "Dragon1", M2Share.Config.sDragon1);
            if (ReadString("Names", "Angel", "") == "")
                WriteString("Names", "Angel", M2Share.Config.sAngel);
            M2Share.Config.sAngel = ReadString("Names", "Angel", M2Share.Config.sAngel);
            sLoadString = ReadString("Names", "GameGold", "");
            if (sLoadString == "")
                WriteString("Share", "GameGold", M2Share.Config.sGameGoldName);
            else
                M2Share.Config.sGameGoldName = sLoadString;
            sLoadString = ReadString("Names", "GamePoint", "");
            if (sLoadString == "")
                WriteString("Share", "GamePoint", M2Share.Config.sGamePointName);
            else
                M2Share.Config.sGamePointName = sLoadString;
            sLoadString = ReadString("Names", "PayMentPointName", "");
            if (sLoadString == "")
                WriteString("Share", "PayMentPointName", M2Share.Config.sPayMentPointName);
            else
                M2Share.Config.sPayMentPointName = sLoadString;
            if (M2Share.Config.nAppIconCrc != 1242102148) M2Share.Config.boCheckFail = true;
            // ============================================================================
            // 游戏设置
            if (ReadInteger("Setup", "ItemNumber", -1) < 0)
                WriteInteger("Setup", "ItemNumber", M2Share.Config.nItemNumber);
            M2Share.Config.nItemNumber = ReadInteger("Setup", "ItemNumber", M2Share.Config.nItemNumber);
            M2Share.Config.nItemNumber = M2Share.Config.nItemNumber + M2Share.RandomNumber.Random(10000);
            if (ReadInteger("Setup", "ItemNumberEx", -1) < 0)
                WriteInteger("Setup", "ItemNumberEx", M2Share.Config.nItemNumberEx);
            M2Share.Config.nItemNumberEx = ReadInteger("Setup", "ItemNumberEx", M2Share.Config.nItemNumberEx);
            if (ReadString("Setup", "ClientFile1", "") == "")
                WriteString("Setup", "ClientFile1", M2Share.Config.sClientFile1);
            M2Share.Config.sClientFile1 = ReadString("Setup", "ClientFile1", M2Share.Config.sClientFile1);
            if (ReadString("Setup", "ClientFile2", "") == "")
                WriteString("Setup", "ClientFile2", M2Share.Config.sClientFile2);
            M2Share.Config.sClientFile2 = ReadString("Setup", "ClientFile2", M2Share.Config.sClientFile2);
            if (ReadString("Setup", "ClientFile3", "") == "")
                WriteString("Setup", "ClientFile3", M2Share.Config.sClientFile3);
            M2Share.Config.sClientFile3 = ReadString("Setup", "ClientFile3", M2Share.Config.sClientFile3);
            if (ReadInteger("Setup", "MonUpLvNeedKillBase", -1) < 0)
                WriteInteger("Setup", "MonUpLvNeedKillBase", M2Share.Config.nMonUpLvNeedKillBase);
            M2Share.Config.nMonUpLvNeedKillBase = ReadInteger("Setup", "MonUpLvNeedKillBase", M2Share.Config.nMonUpLvNeedKillBase);
            if (ReadInteger("Setup", "MonUpLvRate", -1) < 0)
            {
                WriteInteger("Setup", "MonUpLvRate", M2Share.Config.nMonUpLvRate);
            }
            M2Share.Config.nMonUpLvRate = ReadInteger("Setup", "MonUpLvRate", M2Share.Config.nMonUpLvRate);
            for (var i = 0; i < M2Share.Config.MonUpLvNeedKillCount.Length; i++)
            {
                if (ReadInteger("Setup", "MonUpLvNeedKillCount" + i, -1) < 0)
                {
                    WriteInteger("Setup", "MonUpLvNeedKillCount" + i, M2Share.Config.MonUpLvNeedKillCount[i]);
                }
                M2Share.Config.MonUpLvNeedKillCount[i] = ReadInteger("Setup", "MonUpLvNeedKillCount" + i, M2Share.Config.MonUpLvNeedKillCount[i]);
            }
            for (var i = 0; i < M2Share.Config.SlaveColor.Length; i++)
            {
                if (ReadInteger("Setup", "SlaveColor" + i, -1) < 0)
                {
                    WriteInteger("Setup", "SlaveColor" + i, M2Share.Config.SlaveColor[i]);
                }
                M2Share.Config.SlaveColor[i] = Read<byte>("Setup", "SlaveColor" + i, M2Share.Config.SlaveColor[i]);
            }
            if (ReadString("Setup", "HomeMap", "") == "")
            {
                WriteString("Setup", "HomeMap", M2Share.Config.sHomeMap);
            }
            M2Share.Config.sHomeMap = ReadString("Setup", "HomeMap", M2Share.Config.sHomeMap);
            if (ReadInteger("Setup", "HomeX", -1) < 0)
            {
                WriteInteger("Setup", "HomeX", M2Share.Config.nHomeX);
            }
            M2Share.Config.nHomeX = Read<short>("Setup", "HomeX", M2Share.Config.nHomeX);
            if (ReadInteger("Setup", "HomeY", -1) < 0)
            {
                WriteInteger("Setup", "HomeY", M2Share.Config.nHomeY);
            }
            M2Share.Config.nHomeY = Read<short>("Setup", "HomeY", M2Share.Config.nHomeY);
            if (ReadString("Setup", "RedHomeMap", "") == "")
            {
                WriteString("Setup", "RedHomeMap", M2Share.Config.sRedHomeMap);
            }
            M2Share.Config.sRedHomeMap = ReadString("Setup", "RedHomeMap", M2Share.Config.sRedHomeMap);
            if (ReadInteger("Setup", "RedHomeX", -1) < 0)
            {
                WriteInteger("Setup", "RedHomeX", M2Share.Config.nRedHomeX);
            }
            M2Share.Config.nRedHomeX = Read<short>("Setup", "RedHomeX", M2Share.Config.nRedHomeX);
            if (ReadInteger("Setup", "RedHomeY", -1) < 0)
                WriteInteger("Setup", "RedHomeY", M2Share.Config.nRedHomeY);
            M2Share.Config.nRedHomeY = Read<short>("Setup", "RedHomeY", M2Share.Config.nRedHomeY);
            if (ReadString("Setup", "RedDieHomeMap", "") == "")
                WriteString("Setup", "RedDieHomeMap", M2Share.Config.sRedDieHomeMap);
            M2Share.Config.sRedDieHomeMap = ReadString("Setup", "RedDieHomeMap", M2Share.Config.sRedDieHomeMap);
            if (ReadInteger("Setup", "RedDieHomeX", -1) < 0)
                WriteInteger("Setup", "RedDieHomeX", M2Share.Config.nRedDieHomeX);
            M2Share.Config.nRedDieHomeX = Read<short>("Setup", "RedDieHomeX", M2Share.Config.nRedDieHomeX);
            if (ReadInteger("Setup", "RedDieHomeY", -1) < 0)
                WriteInteger("Setup", "RedDieHomeY", M2Share.Config.nRedDieHomeY);
            M2Share.Config.nRedDieHomeY = Read<short>("Setup", "RedDieHomeY", M2Share.Config.nRedDieHomeY);
            if (ReadInteger("Setup", "JobHomePointSystem", -1) < 0)
                WriteBool("Setup", "JobHomePointSystem", M2Share.Config.boJobHomePoint);
            M2Share.Config.boJobHomePoint = ReadBool("Setup", "JobHomePointSystem", M2Share.Config.boJobHomePoint);
            if (ReadString("Setup", "WarriorHomeMap", "") == "")
                WriteString("Setup", "WarriorHomeMap", M2Share.Config.sWarriorHomeMap);
            M2Share.Config.sWarriorHomeMap = ReadString("Setup", "WarriorHomeMap", M2Share.Config.sWarriorHomeMap);
            if (ReadInteger("Setup", "WarriorHomeX", -1) < 0)
                WriteInteger("Setup", "WarriorHomeX", M2Share.Config.nWarriorHomeX);
            M2Share.Config.nWarriorHomeX = Read<short>("Setup", "WarriorHomeX", M2Share.Config.nWarriorHomeX);
            if (ReadInteger("Setup", "WarriorHomeY", -1) < 0)
                WriteInteger("Setup", "WarriorHomeY", M2Share.Config.nWarriorHomeY);
            M2Share.Config.nWarriorHomeY = Read<short>("Setup", "WarriorHomeY", M2Share.Config.nWarriorHomeY);
            if (ReadString("Setup", "WizardHomeMap", "") == "")
                WriteString("Setup", "WizardHomeMap", M2Share.Config.sWizardHomeMap);
            M2Share.Config.sWizardHomeMap = ReadString("Setup", "WizardHomeMap", M2Share.Config.sWizardHomeMap);
            if (ReadInteger("Setup", "WizardHomeX", -1) < 0)
                WriteInteger("Setup", "WizardHomeX", M2Share.Config.nWizardHomeX);
            M2Share.Config.nWizardHomeX = Read<short>("Setup", "WizardHomeX", M2Share.Config.nWizardHomeX);
            if (ReadInteger("Setup", "WizardHomeY", -1) < 0)
                WriteInteger("Setup", "WizardHomeY", M2Share.Config.nWizardHomeY);
            M2Share.Config.nWizardHomeY = Read<short>("Setup", "WizardHomeY", M2Share.Config.nWizardHomeY);
            if (ReadString("Setup", "TaoistHomeMap", "") == "")
                WriteString("Setup", "TaoistHomeMap", M2Share.Config.sTaoistHomeMap);
            M2Share.Config.sTaoistHomeMap = ReadString("Setup", "TaoistHomeMap", M2Share.Config.sTaoistHomeMap);
            if (ReadInteger("Setup", "TaoistHomeX", -1) < 0)
                WriteInteger("Setup", "TaoistHomeX", M2Share.Config.nTaoistHomeX);
            M2Share.Config.nTaoistHomeX = Read<short>("Setup", "TaoistHomeX", M2Share.Config.nTaoistHomeX);
            if (ReadInteger("Setup", "TaoistHomeY", -1) < 0)
                WriteInteger("Setup", "TaoistHomeY", M2Share.Config.nTaoistHomeY);
            M2Share.Config.nTaoistHomeY = Read<short>("Setup", "TaoistHomeY", M2Share.Config.nTaoistHomeY);
            nLoadInteger = ReadInteger("Setup", "HealthFillTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "HealthFillTime", M2Share.Config.nHealthFillTime);
            else
                M2Share.Config.nHealthFillTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "SpellFillTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "SpellFillTime", M2Share.Config.nSpellFillTime);
            else
                M2Share.Config.nSpellFillTime = nLoadInteger;
            if (ReadInteger("Setup", "DecPkPointTime", -1) < 0)
                WriteInteger("Setup", "DecPkPointTime", M2Share.Config.dwDecPkPointTime);
            M2Share.Config.dwDecPkPointTime = ReadInteger("Setup", "DecPkPointTime", M2Share.Config.dwDecPkPointTime);
            if (ReadInteger("Setup", "DecPkPointCount", -1) < 0)
                WriteInteger("Setup", "DecPkPointCount", M2Share.Config.nDecPkPointCount);
            M2Share.Config.nDecPkPointCount = ReadInteger("Setup", "DecPkPointCount", M2Share.Config.nDecPkPointCount);
            if (ReadInteger("Setup", "PKFlagTime", -1) < 0)
                WriteInteger("Setup", "PKFlagTime", M2Share.Config.dwPKFlagTime);
            M2Share.Config.dwPKFlagTime = ReadInteger("Setup", "PKFlagTime", M2Share.Config.dwPKFlagTime);
            if (ReadInteger("Setup", "KillHumanAddPKPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanAddPKPoint", M2Share.Config.nKillHumanAddPKPoint);
            M2Share.Config.nKillHumanAddPKPoint = ReadInteger("Setup", "KillHumanAddPKPoint", M2Share.Config.nKillHumanAddPKPoint);
            if (ReadInteger("Setup", "KillHumanDecLuckPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanDecLuckPoint", M2Share.Config.nKillHumanDecLuckPoint);
            M2Share.Config.nKillHumanDecLuckPoint = ReadInteger("Setup", "KillHumanDecLuckPoint", M2Share.Config.nKillHumanDecLuckPoint);
            if (ReadInteger("Setup", "DecLightItemDrugTime", -1) < 0)
                WriteInteger("Setup", "DecLightItemDrugTime", M2Share.Config.dwDecLightItemDrugTime);
            M2Share.Config.dwDecLightItemDrugTime = ReadInteger("Setup", "DecLightItemDrugTime", M2Share.Config.dwDecLightItemDrugTime);
            if (ReadInteger("Setup", "SafeZoneSize", -1) < 0)
                WriteInteger("Setup", "SafeZoneSize", M2Share.Config.nSafeZoneSize);
            M2Share.Config.nSafeZoneSize =
                ReadInteger("Setup", "SafeZoneSize", M2Share.Config.nSafeZoneSize);
            if (ReadInteger("Setup", "StartPointSize", -1) < 0)
                WriteInteger("Setup", "StartPointSize", M2Share.Config.nStartPointSize);
            M2Share.Config.nStartPointSize =
                ReadInteger("Setup", "StartPointSize", M2Share.Config.nStartPointSize);
            for (var i = 0; i < M2Share.Config.ReNewNameColor.Length; i++)
            {
                if (ReadInteger("Setup", "ReNewNameColor" + i, -1) < 0)
                {
                    WriteInteger("Setup", "ReNewNameColor" + i, M2Share.Config.ReNewNameColor[i]);
                }
                M2Share.Config.ReNewNameColor[i] = Read<byte>("Setup", "ReNewNameColor" + i, M2Share.Config.ReNewNameColor[i]);
            }
            if (ReadInteger("Setup", "ReNewNameColorTime", -1) < 0)
                WriteInteger("Setup", "ReNewNameColorTime", M2Share.Config.dwReNewNameColorTime);
            M2Share.Config.dwReNewNameColorTime = ReadInteger("Setup", "ReNewNameColorTime", M2Share.Config.dwReNewNameColorTime);
            if (ReadInteger("Setup", "ReNewChangeColor", -1) < 0)
                WriteBool("Setup", "ReNewChangeColor", M2Share.Config.boReNewChangeColor);
            M2Share.Config.boReNewChangeColor = ReadBool("Setup", "ReNewChangeColor", M2Share.Config.boReNewChangeColor);
            if (ReadInteger("Setup", "ReNewLevelClearExp", -1) < 0)
                WriteBool("Setup", "ReNewLevelClearExp", M2Share.Config.boReNewLevelClearExp);
            M2Share.Config.boReNewLevelClearExp = ReadBool("Setup", "ReNewLevelClearExp", M2Share.Config.boReNewLevelClearExp);
            if (ReadInteger("Setup", "BonusAbilofWarrDC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrDC", M2Share.Config.BonusAbilofWarr.DC);
            M2Share.Config.BonusAbilofWarr.DC = Read<ushort>("Setup", "BonusAbilofWarrDC", M2Share.Config.BonusAbilofWarr.DC);
            if (ReadInteger("Setup", "BonusAbilofWarrMC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrMC", M2Share.Config.BonusAbilofWarr.MC);
            M2Share.Config.BonusAbilofWarr.MC = Read<ushort>("Setup", "BonusAbilofWarrMC", M2Share.Config.BonusAbilofWarr.MC);
            if (ReadInteger("Setup", "BonusAbilofWarrSC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrSC", M2Share.Config.BonusAbilofWarr.SC);
            M2Share.Config.BonusAbilofWarr.SC = Read<ushort>("Setup", "BonusAbilofWarrSC", M2Share.Config.BonusAbilofWarr.SC);
            if (ReadInteger("Setup", "BonusAbilofWarrAC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrAC", M2Share.Config.BonusAbilofWarr.AC);
            M2Share.Config.BonusAbilofWarr.AC = Read<ushort>("Setup", "BonusAbilofWarrAC", M2Share.Config.BonusAbilofWarr.AC);
            if (ReadInteger("Setup", "BonusAbilofWarrMAC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrMAC", M2Share.Config.BonusAbilofWarr.MAC);
            M2Share.Config.BonusAbilofWarr.MAC = Read<ushort>("Setup", "BonusAbilofWarrMAC", M2Share.Config.BonusAbilofWarr.MAC);
            if (ReadInteger("Setup", "BonusAbilofWarrHP", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrHP", M2Share.Config.BonusAbilofWarr.HP);
            M2Share.Config.BonusAbilofWarr.HP = Read<ushort>("Setup", "BonusAbilofWarrHP", M2Share.Config.BonusAbilofWarr.HP);
            if (ReadInteger("Setup", "BonusAbilofWarrMP", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrMP", M2Share.Config.BonusAbilofWarr.MP);
            M2Share.Config.BonusAbilofWarr.MP = Read<ushort>("Setup", "BonusAbilofWarrMP", M2Share.Config.BonusAbilofWarr.MP);
            if (ReadInteger("Setup", "BonusAbilofWarrHit", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrHit", M2Share.Config.BonusAbilofWarr.Hit);
            M2Share.Config.BonusAbilofWarr.Hit = Read<byte>("Setup", "BonusAbilofWarrHit", M2Share.Config.BonusAbilofWarr.Hit);
            if (ReadInteger("Setup", "BonusAbilofWarrSpeed", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrSpeed", M2Share.Config.BonusAbilofWarr.Speed);
            M2Share.Config.BonusAbilofWarr.Speed = ReadInteger("Setup", "BonusAbilofWarrSpeed", M2Share.Config.BonusAbilofWarr.Speed);
            if (ReadInteger("Setup", "BonusAbilofWarrX2", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWarrX2", M2Share.Config.BonusAbilofWarr.X2);
            M2Share.Config.BonusAbilofWarr.X2 = Read<byte>("Setup", "BonusAbilofWarrX2", M2Share.Config.BonusAbilofWarr.X2);
            if (ReadInteger("Setup", "BonusAbilofWizardDC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardDC", M2Share.Config.BonusAbilofWizard.DC);
            M2Share.Config.BonusAbilofWizard.DC = Read<ushort>("Setup", "BonusAbilofWizardDC", M2Share.Config.BonusAbilofWizard.DC);
            if (ReadInteger("Setup", "BonusAbilofWizardMC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardMC", M2Share.Config.BonusAbilofWizard.MC);
            M2Share.Config.BonusAbilofWizard.MC = Read<ushort>("Setup", "BonusAbilofWizardMC", M2Share.Config.BonusAbilofWizard.MC);
            if (ReadInteger("Setup", "BonusAbilofWizardSC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardSC", M2Share.Config.BonusAbilofWizard.SC);
            M2Share.Config.BonusAbilofWizard.SC = Read<ushort>("Setup", "BonusAbilofWizardSC", M2Share.Config.BonusAbilofWizard.SC);
            if (ReadInteger("Setup", "BonusAbilofWizardAC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardAC", M2Share.Config.BonusAbilofWizard.AC);
            M2Share.Config.BonusAbilofWizard.AC = Read<ushort>("Setup", "BonusAbilofWizardAC", M2Share.Config.BonusAbilofWizard.AC);
            if (ReadInteger("Setup", "BonusAbilofWizardMAC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardMAC", M2Share.Config.BonusAbilofWizard.MAC);
            M2Share.Config.BonusAbilofWizard.MAC = Read<ushort>("Setup", "BonusAbilofWizardMAC", M2Share.Config.BonusAbilofWizard.MAC);
            if (ReadInteger("Setup", "BonusAbilofWizardHP", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardHP", M2Share.Config.BonusAbilofWizard.HP);
            M2Share.Config.BonusAbilofWizard.HP = Read<ushort>("Setup", "BonusAbilofWizardHP", M2Share.Config.BonusAbilofWizard.HP);
            if (ReadInteger("Setup", "BonusAbilofWizardMP", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardMP", M2Share.Config.BonusAbilofWizard.MP);
            M2Share.Config.BonusAbilofWizard.MP = Read<ushort>("Setup", "BonusAbilofWizardMP", M2Share.Config.BonusAbilofWizard.MP);
            if (ReadInteger("Setup", "BonusAbilofWizardHit", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardHit", M2Share.Config.BonusAbilofWizard.Hit);
            M2Share.Config.BonusAbilofWizard.Hit = Read<byte>("Setup", "BonusAbilofWizardHit", M2Share.Config.BonusAbilofWizard.Hit);
            if (ReadInteger("Setup", "BonusAbilofWizardSpeed", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardSpeed", M2Share.Config.BonusAbilofWizard.Speed);
            M2Share.Config.BonusAbilofWizard.Speed = ReadInteger("Setup", "BonusAbilofWizardSpeed", M2Share.Config.BonusAbilofWizard.Speed);
            if (ReadInteger("Setup", "BonusAbilofWizardX2", -1) < 0)
                WriteInteger("Setup", "BonusAbilofWizardX2", M2Share.Config.BonusAbilofWizard.X2);
            M2Share.Config.BonusAbilofWizard.X2 = Read<byte>("Setup", "BonusAbilofWizardX2", M2Share.Config.BonusAbilofWizard.X2);
            if (ReadInteger("Setup", "BonusAbilofTaosDC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosDC", M2Share.Config.BonusAbilofTaos.DC);
            M2Share.Config.BonusAbilofTaos.DC = Read<byte>("Setup", "BonusAbilofTaosDC", M2Share.Config.BonusAbilofTaos.DC);
            if (ReadInteger("Setup", "BonusAbilofTaosMC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosMC", M2Share.Config.BonusAbilofTaos.MC);
            M2Share.Config.BonusAbilofTaos.MC = Read<byte>("Setup", "BonusAbilofTaosMC", M2Share.Config.BonusAbilofTaos.MC);
            if (ReadInteger("Setup", "BonusAbilofTaosSC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosSC", M2Share.Config.BonusAbilofTaos.SC);
            M2Share.Config.BonusAbilofTaos.SC = Read<byte>("Setup", "BonusAbilofTaosSC", M2Share.Config.BonusAbilofTaos.SC);
            if (ReadInteger("Setup", "BonusAbilofTaosAC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosAC", M2Share.Config.BonusAbilofTaos.AC);
            M2Share.Config.BonusAbilofTaos.AC = Read<byte>("Setup", "BonusAbilofTaosAC", M2Share.Config.BonusAbilofTaos.AC);
            if (ReadInteger("Setup", "BonusAbilofTaosMAC", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosMAC", M2Share.Config.BonusAbilofTaos.MAC);
            M2Share.Config.BonusAbilofTaos.MAC = Read<ushort>("Setup", "BonusAbilofTaosMAC", M2Share.Config.BonusAbilofTaos.MAC);
            if (ReadInteger("Setup", "BonusAbilofTaosHP", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosHP", M2Share.Config.BonusAbilofTaos.HP);
            M2Share.Config.BonusAbilofTaos.HP = Read<ushort>("Setup", "BonusAbilofTaosHP", M2Share.Config.BonusAbilofTaos.HP);
            if (ReadInteger("Setup", "BonusAbilofTaosMP", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosMP", M2Share.Config.BonusAbilofTaos.MP);
            M2Share.Config.BonusAbilofTaos.MP = Read<ushort>("Setup", "BonusAbilofTaosMP", M2Share.Config.BonusAbilofTaos.MP);
            if (ReadInteger("Setup", "BonusAbilofTaosHit", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosHit", M2Share.Config.BonusAbilofTaos.Hit);
            M2Share.Config.BonusAbilofTaos.Hit = Read<byte>("Setup", "BonusAbilofTaosHit", M2Share.Config.BonusAbilofTaos.Hit);
            if (ReadInteger("Setup", "BonusAbilofTaosSpeed", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosSpeed", M2Share.Config.BonusAbilofTaos.Speed);
            M2Share.Config.BonusAbilofTaos.Speed = ReadInteger("Setup", "BonusAbilofTaosSpeed", M2Share.Config.BonusAbilofTaos.Speed);
            if (ReadInteger("Setup", "BonusAbilofTaosX2", -1) < 0)
                WriteInteger("Setup", "BonusAbilofTaosX2", M2Share.Config.BonusAbilofTaos.X2);
            M2Share.Config.BonusAbilofTaos.X2 = Read<byte>("Setup", "BonusAbilofTaosX2", M2Share.Config.BonusAbilofTaos.X2);
            if (ReadInteger("Setup", "NakedAbilofWarrDC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrDC", M2Share.Config.NakedAbilofWarr.DC);
            M2Share.Config.NakedAbilofWarr.DC = Read<ushort>("Setup", "NakedAbilofWarrDC", M2Share.Config.NakedAbilofWarr.DC);
            if (ReadInteger("Setup", "NakedAbilofWarrMC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrMC", M2Share.Config.NakedAbilofWarr.MC);
            M2Share.Config.NakedAbilofWarr.MC = Read<ushort>("Setup", "NakedAbilofWarrMC", M2Share.Config.NakedAbilofWarr.MC);
            if (ReadInteger("Setup", "NakedAbilofWarrSC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrSC", M2Share.Config.NakedAbilofWarr.SC);
            M2Share.Config.NakedAbilofWarr.SC = Read<ushort>("Setup", "NakedAbilofWarrSC", M2Share.Config.NakedAbilofWarr.SC);
            if (ReadInteger("Setup", "NakedAbilofWarrAC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrAC", M2Share.Config.NakedAbilofWarr.AC);
            M2Share.Config.NakedAbilofWarr.AC = Read<ushort>("Setup", "NakedAbilofWarrAC", M2Share.Config.NakedAbilofWarr.AC);
            if (ReadInteger("Setup", "NakedAbilofWarrMAC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrMAC", M2Share.Config.NakedAbilofWarr.MAC);
            M2Share.Config.NakedAbilofWarr.MAC = Read<ushort>("Setup", "NakedAbilofWarrMAC", M2Share.Config.NakedAbilofWarr.MAC);
            if (ReadInteger("Setup", "NakedAbilofWarrHP", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrHP", M2Share.Config.NakedAbilofWarr.HP);
            M2Share.Config.NakedAbilofWarr.HP = Read<ushort>("Setup", "NakedAbilofWarrHP", M2Share.Config.NakedAbilofWarr.HP);
            if (ReadInteger("Setup", "NakedAbilofWarrMP", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrMP", M2Share.Config.NakedAbilofWarr.MP);
            M2Share.Config.NakedAbilofWarr.MP = Read<ushort>("Setup", "NakedAbilofWarrMP", M2Share.Config.NakedAbilofWarr.MP);
            if (ReadInteger("Setup", "NakedAbilofWarrHit", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrHit", M2Share.Config.NakedAbilofWarr.Hit);
            M2Share.Config.NakedAbilofWarr.Hit = Read<byte>("Setup", "NakedAbilofWarrHit", M2Share.Config.NakedAbilofWarr.Hit);
            if (ReadInteger("Setup", "NakedAbilofWarrSpeed", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrSpeed", M2Share.Config.NakedAbilofWarr.Speed);
            M2Share.Config.NakedAbilofWarr.Speed = ReadInteger("Setup", "NakedAbilofWarrSpeed", M2Share.Config.NakedAbilofWarr.Speed);
            if (ReadInteger("Setup", "NakedAbilofWarrX2", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWarrX2", M2Share.Config.NakedAbilofWarr.X2);
            M2Share.Config.NakedAbilofWarr.X2 = Read<byte>("Setup", "NakedAbilofWarrX2", M2Share.Config.NakedAbilofWarr.X2);
            if (ReadInteger("Setup", "NakedAbilofWizardDC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardDC", M2Share.Config.NakedAbilofWizard.DC);
            M2Share.Config.NakedAbilofWizard.DC = Read<ushort>("Setup", "NakedAbilofWizardDC", M2Share.Config.NakedAbilofWizard.DC);
            if (ReadInteger("Setup", "NakedAbilofWizardMC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardMC", M2Share.Config.NakedAbilofWizard.MC);
            M2Share.Config.NakedAbilofWizard.MC = Read<ushort>("Setup", "NakedAbilofWizardMC", M2Share.Config.NakedAbilofWizard.MC);
            if (ReadInteger("Setup", "NakedAbilofWizardSC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardSC", M2Share.Config.NakedAbilofWizard.SC);
            M2Share.Config.NakedAbilofWizard.SC = Read<ushort>("Setup", "NakedAbilofWizardSC", M2Share.Config.NakedAbilofWizard.SC);
            if (ReadInteger("Setup", "NakedAbilofWizardAC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardAC", M2Share.Config.NakedAbilofWizard.AC);
            M2Share.Config.NakedAbilofWizard.AC = Read<ushort>("Setup", "NakedAbilofWizardAC", M2Share.Config.NakedAbilofWizard.AC);
            if (ReadInteger("Setup", "NakedAbilofWizardMAC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardMAC", M2Share.Config.NakedAbilofWizard.MAC);
            M2Share.Config.NakedAbilofWizard.MAC = Read<ushort>("Setup", "NakedAbilofWizardMAC", M2Share.Config.NakedAbilofWizard.MAC);
            if (ReadInteger("Setup", "NakedAbilofWizardHP", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardHP", M2Share.Config.NakedAbilofWizard.HP);
            M2Share.Config.NakedAbilofWizard.HP = Read<ushort>("Setup", "NakedAbilofWizardHP", M2Share.Config.NakedAbilofWizard.HP);
            if (ReadInteger("Setup", "NakedAbilofWizardMP", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardMP", M2Share.Config.NakedAbilofWizard.MP);
            M2Share.Config.NakedAbilofWizard.MP = Read<ushort>("Setup", "NakedAbilofWizardMP", M2Share.Config.NakedAbilofWizard.MP);
            if (ReadInteger("Setup", "NakedAbilofWizardHit", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardHit", M2Share.Config.NakedAbilofWizard.Hit);
            M2Share.Config.NakedAbilofWizard.Hit = Read<byte>("Setup", "NakedAbilofWizardHit", M2Share.Config.NakedAbilofWizard.Hit);
            if (ReadInteger("Setup", "NakedAbilofWizardSpeed", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardSpeed", M2Share.Config.NakedAbilofWizard.Speed);
            M2Share.Config.NakedAbilofWizard.Speed = ReadInteger("Setup", "NakedAbilofWizardSpeed", M2Share.Config.NakedAbilofWizard.Speed);
            if (ReadInteger("Setup", "NakedAbilofWizardX2", -1) < 0)
                WriteInteger("Setup", "NakedAbilofWizardX2", M2Share.Config.NakedAbilofWizard.X2);
            M2Share.Config.NakedAbilofWizard.X2 = Read<byte>("Setup", "NakedAbilofWizardX2", M2Share.Config.NakedAbilofWizard.X2);
            if (ReadInteger("Setup", "NakedAbilofTaosDC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosDC", M2Share.Config.NakedAbilofTaos.DC);
            M2Share.Config.NakedAbilofTaos.DC = Read<ushort>("Setup", "NakedAbilofTaosDC", M2Share.Config.NakedAbilofTaos.DC);
            if (ReadInteger("Setup", "NakedAbilofTaosMC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosMC", M2Share.Config.NakedAbilofTaos.MC);
            M2Share.Config.NakedAbilofTaos.MC = Read<ushort>("Setup", "NakedAbilofTaosMC", M2Share.Config.NakedAbilofTaos.MC);
            if (ReadInteger("Setup", "NakedAbilofTaosSC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosSC", M2Share.Config.NakedAbilofTaos.SC);
            M2Share.Config.NakedAbilofTaos.SC = Read<ushort>("Setup", "NakedAbilofTaosSC", M2Share.Config.NakedAbilofTaos.SC);
            if (ReadInteger("Setup", "NakedAbilofTaosAC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosAC", M2Share.Config.NakedAbilofTaos.AC);
            M2Share.Config.NakedAbilofTaos.AC = Read<ushort>("Setup", "NakedAbilofTaosAC", M2Share.Config.NakedAbilofTaos.AC);
            if (ReadInteger("Setup", "NakedAbilofTaosMAC", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosMAC", M2Share.Config.NakedAbilofTaos.MAC);
            M2Share.Config.NakedAbilofTaos.MAC = Read<ushort>("Setup", "NakedAbilofTaosMAC", M2Share.Config.NakedAbilofTaos.MAC);
            if (ReadInteger("Setup", "NakedAbilofTaosHP", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosHP", M2Share.Config.NakedAbilofTaos.HP);
            M2Share.Config.NakedAbilofTaos.HP = Read<ushort>("Setup", "NakedAbilofTaosHP", M2Share.Config.NakedAbilofTaos.HP);
            if (ReadInteger("Setup", "NakedAbilofTaosMP", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosMP", M2Share.Config.NakedAbilofTaos.MP);
            M2Share.Config.NakedAbilofTaos.MP = Read<ushort>("Setup", "NakedAbilofTaosMP", M2Share.Config.NakedAbilofTaos.MP);
            if (ReadInteger("Setup", "NakedAbilofTaosHit", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosHit", M2Share.Config.NakedAbilofTaos.Hit);
            M2Share.Config.NakedAbilofTaos.Hit = Read<byte>("Setup", "NakedAbilofTaosHit", M2Share.Config.NakedAbilofTaos.Hit);
            if (ReadInteger("Setup", "NakedAbilofTaosSpeed", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosSpeed", M2Share.Config.NakedAbilofTaos.Speed);
            M2Share.Config.NakedAbilofTaos.Speed = ReadInteger("Setup", "NakedAbilofTaosSpeed", M2Share.Config.NakedAbilofTaos.Speed);
            if (ReadInteger("Setup", "NakedAbilofTaosX2", -1) < 0)
                WriteInteger("Setup", "NakedAbilofTaosX2", M2Share.Config.NakedAbilofTaos.X2);
            M2Share.Config.NakedAbilofTaos.X2 = Read<byte>("Setup", "NakedAbilofTaosX2", M2Share.Config.NakedAbilofTaos.X2);
            if (ReadInteger("Setup", "GroupMembersMax", -1) < 0)
                WriteInteger("Setup", "GroupMembersMax", M2Share.Config.nGroupMembersMax);
            M2Share.Config.nGroupMembersMax = ReadInteger("Setup", "GroupMembersMax", M2Share.Config.nGroupMembersMax);
            if (ReadInteger("Setup", "WarrAttackMon", -1) < 0)
                WriteInteger("Setup", "WarrAttackMon", M2Share.Config.nWarrMon);
            M2Share.Config.nWarrMon = ReadInteger("Setup", "WarrAttackMon", M2Share.Config.nWarrMon);
            if (ReadInteger("Setup", "WizardAttackMon", -1) < 0)
                WriteInteger("Setup", "WizardAttackMon", M2Share.Config.nWizardMon);
            M2Share.Config.nWizardMon = ReadInteger("Setup", "WizardAttackMon", M2Share.Config.nWizardMon);
            if (ReadInteger("Setup", "TaosAttackMon", -1) < 0)
                WriteInteger("Setup", "TaosAttackMon", M2Share.Config.nTaosMon);
            M2Share.Config.nTaosMon = ReadInteger("Setup", "TaosAttackMon", M2Share.Config.nTaosMon);
            if (ReadInteger("Setup", "MonAttackHum", -1) < 0)
                WriteInteger("Setup", "MonAttackHum", M2Share.Config.nMonHum);
            M2Share.Config.nMonHum = ReadInteger("Setup", "MonAttackHum", M2Share.Config.nMonHum);
            if (ReadInteger("Setup", "UPgradeWeaponGetBackTime", -1) < 0)
            {
                WriteInteger("Setup", "UPgradeWeaponGetBackTime", M2Share.Config.dwUPgradeWeaponGetBackTime);
            }
            M2Share.Config.dwUPgradeWeaponGetBackTime = ReadInteger("Setup", "UPgradeWeaponGetBackTime", M2Share.Config.dwUPgradeWeaponGetBackTime);
            if (ReadInteger("Setup", "ClearExpireUpgradeWeaponDays", -1) < 0)
            {
                WriteInteger("Setup", "ClearExpireUpgradeWeaponDays", M2Share.Config.nClearExpireUpgradeWeaponDays);
            }
            M2Share.Config.nClearExpireUpgradeWeaponDays = ReadInteger("Setup", "ClearExpireUpgradeWeaponDays", M2Share.Config.nClearExpireUpgradeWeaponDays);
            if (ReadInteger("Setup", "UpgradeWeaponPrice", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponPrice", M2Share.Config.nUpgradeWeaponPrice);
            M2Share.Config.nUpgradeWeaponPrice = ReadInteger("Setup", "UpgradeWeaponPrice", M2Share.Config.nUpgradeWeaponPrice);
            if (ReadInteger("Setup", "UpgradeWeaponMaxPoint", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponMaxPoint", M2Share.Config.nUpgradeWeaponMaxPoint);
            M2Share.Config.nUpgradeWeaponMaxPoint = ReadInteger("Setup", "UpgradeWeaponMaxPoint", M2Share.Config.nUpgradeWeaponMaxPoint);
            if (ReadInteger("Setup", "UpgradeWeaponDCRate", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponDCRate", M2Share.Config.nUpgradeWeaponDCRate);
            M2Share.Config.nUpgradeWeaponDCRate = ReadInteger("Setup", "UpgradeWeaponDCRate", M2Share.Config.nUpgradeWeaponDCRate);
            if (ReadInteger("Setup", "UpgradeWeaponDCTwoPointRate", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponDCTwoPointRate", M2Share.Config.nUpgradeWeaponDCTwoPointRate);
            M2Share.Config.nUpgradeWeaponDCTwoPointRate = ReadInteger("Setup", "UpgradeWeaponDCTwoPointRate", M2Share.Config.nUpgradeWeaponDCTwoPointRate);
            if (ReadInteger("Setup", "UpgradeWeaponDCThreePointRate", -1) < 0)
            {
                WriteInteger("Setup", "UpgradeWeaponDCThreePointRate", M2Share.Config.nUpgradeWeaponDCThreePointRate);
            }
            M2Share.Config.nUpgradeWeaponDCThreePointRate = ReadInteger("Setup", "UpgradeWeaponDCThreePointRate", M2Share.Config.nUpgradeWeaponDCThreePointRate);
            if (ReadInteger("Setup", "UpgradeWeaponMCRate", -1) < 0)
            {
                WriteInteger("Setup", "UpgradeWeaponMCRate", M2Share.Config.nUpgradeWeaponMCRate);
            }
            M2Share.Config.nUpgradeWeaponMCRate = ReadInteger("Setup", "UpgradeWeaponMCRate", M2Share.Config.nUpgradeWeaponMCRate);
            if (ReadInteger("Setup", "UpgradeWeaponMCTwoPointRate", -1) < 0)
            {
                WriteInteger("Setup", "UpgradeWeaponMCTwoPointRate", M2Share.Config.nUpgradeWeaponMCTwoPointRate);
            }
            M2Share.Config.nUpgradeWeaponMCTwoPointRate = ReadInteger("Setup", "UpgradeWeaponMCTwoPointRate", M2Share.Config.nUpgradeWeaponMCTwoPointRate);
            if (ReadInteger("Setup", "UpgradeWeaponMCThreePointRate", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponMCThreePointRate", M2Share.Config.nUpgradeWeaponMCThreePointRate);
            M2Share.Config.nUpgradeWeaponMCThreePointRate = ReadInteger("Setup", "UpgradeWeaponMCThreePointRate", M2Share.Config.nUpgradeWeaponMCThreePointRate);
            if (ReadInteger("Setup", "UpgradeWeaponSCRate", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponSCRate", M2Share.Config.nUpgradeWeaponSCRate);
            M2Share.Config.nUpgradeWeaponSCRate =
                ReadInteger("Setup", "UpgradeWeaponSCRate", M2Share.Config.nUpgradeWeaponSCRate);
            if (ReadInteger("Setup", "UpgradeWeaponSCTwoPointRate", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponSCTwoPointRate", M2Share.Config.nUpgradeWeaponSCTwoPointRate);
            M2Share.Config.nUpgradeWeaponSCTwoPointRate = ReadInteger("Setup", "UpgradeWeaponSCTwoPointRate", M2Share.Config.nUpgradeWeaponSCTwoPointRate);
            if (ReadInteger("Setup", "UpgradeWeaponSCThreePointRate", -1) < 0)
            {
                WriteInteger("Setup", "UpgradeWeaponSCThreePointRate", M2Share.Config.nUpgradeWeaponSCThreePointRate);
            }
            M2Share.Config.nUpgradeWeaponSCThreePointRate = ReadInteger("Setup", "UpgradeWeaponSCThreePointRate", M2Share.Config.nUpgradeWeaponSCThreePointRate);
            if (ReadInteger("Setup", "BuildGuild", -1) < 0)
                WriteInteger("Setup", "BuildGuild", M2Share.Config.nBuildGuildPrice);
            M2Share.Config.nBuildGuildPrice = ReadInteger("Setup", "BuildGuild", M2Share.Config.nBuildGuildPrice);
            if (ReadInteger("Setup", "MakeDurg", -1) < 0)
                WriteInteger("Setup", "MakeDurg", M2Share.Config.nMakeDurgPrice);
            M2Share.Config.nMakeDurgPrice = ReadInteger("Setup", "MakeDurg", M2Share.Config.nMakeDurgPrice);
            if (ReadInteger("Setup", "GuildWarFee", -1) < 0)
                WriteInteger("Setup", "GuildWarFee", M2Share.Config.nGuildWarPrice);
            M2Share.Config.nGuildWarPrice = ReadInteger("Setup", "GuildWarFee", M2Share.Config.nGuildWarPrice);
            if (ReadInteger("Setup", "HireGuard", -1) < 0)
                WriteInteger("Setup", "HireGuard", M2Share.Config.nHireGuardPrice);
            M2Share.Config.nHireGuardPrice = ReadInteger("Setup", "HireGuard", M2Share.Config.nHireGuardPrice);
            if (ReadInteger("Setup", "HireArcher", -1) < 0)
                WriteInteger("Setup", "HireArcher", M2Share.Config.nHireArcherPrice);
            M2Share.Config.nHireArcherPrice = ReadInteger("Setup", "HireArcher", M2Share.Config.nHireArcherPrice);
            if (ReadInteger("Setup", "RepairDoor", -1) < 0)
                WriteInteger("Setup", "RepairDoor", M2Share.Config.nRepairDoorPrice);
            M2Share.Config.nRepairDoorPrice = ReadInteger("Setup", "RepairDoor", M2Share.Config.nRepairDoorPrice);
            if (ReadInteger("Setup", "RepairWall", -1) < 0)
                WriteInteger("Setup", "RepairWall", M2Share.Config.nRepairWallPrice);
            M2Share.Config.nRepairWallPrice = ReadInteger("Setup", "RepairWall", M2Share.Config.nRepairWallPrice);
            if (ReadInteger("Setup", "CastleMemberPriceRate", -1) < 0)
                WriteInteger("Setup", "CastleMemberPriceRate", M2Share.Config.nCastleMemberPriceRate);
            M2Share.Config.nCastleMemberPriceRate = ReadInteger("Setup", "CastleMemberPriceRate", M2Share.Config.nCastleMemberPriceRate);
            if (ReadInteger("Setup", "CastleGoldMax", -1) < 0)
                WriteInteger("Setup", "CastleGoldMax", M2Share.Config.nCastleGoldMax);
            M2Share.Config.nCastleGoldMax = ReadInteger("Setup", "CastleGoldMax", M2Share.Config.nCastleGoldMax);
            if (ReadInteger("Setup", "CastleOneDayGold", -1) < 0)
                WriteInteger("Setup", "CastleOneDayGold", M2Share.Config.nCastleOneDayGold);
            M2Share.Config.nCastleOneDayGold = ReadInteger("Setup", "CastleOneDayGold", M2Share.Config.nCastleOneDayGold);
            if (ReadString("Setup", "CastleName", "") == "")
                WriteString("Setup", "CastleName", M2Share.Config.sCastleName);
            M2Share.Config.sCastleName = ReadString("Setup", "CastleName", M2Share.Config.sCastleName);
            if (ReadString("Setup", "CastleHomeMap", "") == "")
                WriteString("Setup", "CastleHomeMap", M2Share.Config.sCastleHomeMap);
            M2Share.Config.sCastleHomeMap = ReadString("Setup", "CastleHomeMap", M2Share.Config.sCastleHomeMap);
            if (ReadInteger("Setup", "CastleHomeX", -1) < 0)
                WriteInteger("Setup", "CastleHomeX", M2Share.Config.nCastleHomeX);
            M2Share.Config.nCastleHomeX = ReadInteger("Setup", "CastleHomeX", M2Share.Config.nCastleHomeX);
            if (ReadInteger("Setup", "CastleHomeY", -1) < 0)
                WriteInteger("Setup", "CastleHomeY", M2Share.Config.nCastleHomeY);
            M2Share.Config.nCastleHomeY = ReadInteger("Setup", "CastleHomeY", M2Share.Config.nCastleHomeY);
            if (ReadInteger("Setup", "CastleWarRangeX", -1) < 0)
                WriteInteger("Setup", "CastleWarRangeX", M2Share.Config.nCastleWarRangeX);
            M2Share.Config.nCastleWarRangeX = ReadInteger("Setup", "CastleWarRangeX", M2Share.Config.nCastleWarRangeX);
            if (ReadInteger("Setup", "CastleWarRangeY", -1) < 0)
                WriteInteger("Setup", "CastleWarRangeY", M2Share.Config.nCastleWarRangeY);
            M2Share.Config.nCastleWarRangeY = ReadInteger("Setup", "CastleWarRangeY", M2Share.Config.nCastleWarRangeY);
            if (ReadInteger("Setup", "CastleTaxRate", -1) < 0)
                WriteInteger("Setup", "CastleTaxRate", M2Share.Config.nCastleTaxRate);
            M2Share.Config.nCastleTaxRate = ReadInteger("Setup", "CastleTaxRate", M2Share.Config.nCastleTaxRate);
            if (ReadInteger("Setup", "CastleGetAllNpcTax", -1) < 0)
                WriteBool("Setup", "CastleGetAllNpcTax", M2Share.Config.boGetAllNpcTax);
            M2Share.Config.boGetAllNpcTax = ReadBool("Setup", "CastleGetAllNpcTax", M2Share.Config.boGetAllNpcTax);
            nLoadInteger = ReadInteger("Setup", "GenMonRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "GenMonRate", M2Share.Config.nMonGenRate);
            else
                M2Share.Config.nMonGenRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "ProcessMonRandRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "ProcessMonRandRate", M2Share.Config.nProcessMonRandRate);
            else
                M2Share.Config.nProcessMonRandRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "ProcessMonLimitCount", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "ProcessMonLimitCount", M2Share.Config.nProcessMonLimitCount);
            else
                M2Share.Config.nProcessMonLimitCount = nLoadInteger;
            if (ReadInteger("Setup", "HumanMaxGold", -1) < 0)
                WriteInteger("Setup", "HumanMaxGold", M2Share.Config.nHumanMaxGold);
            M2Share.Config.nHumanMaxGold = ReadInteger("Setup", "HumanMaxGold", M2Share.Config.nHumanMaxGold);
            if (ReadInteger("Setup", "HumanTryModeMaxGold", -1) < 0)
                WriteInteger("Setup", "HumanTryModeMaxGold", M2Share.Config.nHumanTryModeMaxGold);
            M2Share.Config.nHumanTryModeMaxGold = ReadInteger("Setup", "HumanTryModeMaxGold", M2Share.Config.nHumanTryModeMaxGold);
            if (ReadInteger("Setup", "TryModeLevel", -1) < 0)
                WriteInteger("Setup", "TryModeLevel", M2Share.Config.nTryModeLevel);
            M2Share.Config.nTryModeLevel = ReadInteger("Setup", "TryModeLevel", M2Share.Config.nTryModeLevel);
            if (ReadInteger("Setup", "TryModeUseStorage", -1) < 0)
                WriteBool("Setup", "TryModeUseStorage", M2Share.Config.boTryModeUseStorage);
            M2Share.Config.boTryModeUseStorage = ReadBool("Setup", "TryModeUseStorage", M2Share.Config.boTryModeUseStorage);
            if (ReadInteger("Setup", "ShutRedMsgShowGMName", -1) < 0)
                WriteBool("Setup", "ShutRedMsgShowGMName", M2Share.Config.boShutRedMsgShowGMName);
            M2Share.Config.boShutRedMsgShowGMName = ReadBool("Setup", "ShutRedMsgShowGMName", M2Share.Config.boShutRedMsgShowGMName);
            if (ReadInteger("Setup", "ShowMakeItemMsg", -1) < 0)
                WriteBool("Setup", "ShowMakeItemMsg", M2Share.Config.boShowMakeItemMsg);
            M2Share.Config.boShowMakeItemMsg = ReadBool("Setup", "ShowMakeItemMsg", M2Share.Config.boShowMakeItemMsg);
            if (ReadInteger("Setup", "ShowGuildName", -1) < 0)
                WriteBool("Setup", "ShowGuildName", M2Share.Config.boShowGuildName);
            M2Share.Config.boShowGuildName = ReadBool("Setup", "ShowGuildName", M2Share.Config.boShowGuildName);
            if (ReadInteger("Setup", "ShowRankLevelName", -1) < 0)
                WriteBool("Setup", "ShowRankLevelName", M2Share.Config.boShowRankLevelName);
            M2Share.Config.boShowRankLevelName = ReadBool("Setup", "ShowRankLevelName", M2Share.Config.boShowRankLevelName);
            if (ReadInteger("Setup", "MonSayMsg", -1) < 0)
                WriteBool("Setup", "MonSayMsg", M2Share.Config.boMonSayMsg);
            M2Share.Config.boMonSayMsg = ReadBool("Setup", "MonSayMsg", M2Share.Config.boMonSayMsg);
            if (ReadInteger("Setup", "SayMsgMaxLen", -1) < 0)
                WriteInteger("Setup", "SayMsgMaxLen", M2Share.Config.nSayMsgMaxLen);
            M2Share.Config.nSayMsgMaxLen = ReadInteger("Setup", "SayMsgMaxLen", M2Share.Config.nSayMsgMaxLen);
            if (ReadInteger("Setup", "SayMsgTime", -1) < 0)
                WriteInteger("Setup", "SayMsgTime", M2Share.Config.dwSayMsgTime);
            M2Share.Config.dwSayMsgTime = ReadInteger("Setup", "SayMsgTime", M2Share.Config.dwSayMsgTime);
            if (ReadInteger("Setup", "SayMsgCount", -1) < 0)
                WriteInteger("Setup", "SayMsgCount", M2Share.Config.nSayMsgCount);
            M2Share.Config.nSayMsgCount = ReadInteger("Setup", "SayMsgCount", M2Share.Config.nSayMsgCount);
            if (ReadInteger("Setup", "DisableSayMsgTime", -1) < 0)
                WriteInteger("Setup", "DisableSayMsgTime", M2Share.Config.dwDisableSayMsgTime);
            M2Share.Config.dwDisableSayMsgTime = ReadInteger("Setup", "DisableSayMsgTime", M2Share.Config.dwDisableSayMsgTime);
            if (ReadInteger("Setup", "SayRedMsgMaxLen", -1) < 0)
                WriteInteger("Setup", "SayRedMsgMaxLen", M2Share.Config.nSayRedMsgMaxLen);
            M2Share.Config.nSayRedMsgMaxLen = ReadInteger("Setup", "SayRedMsgMaxLen", M2Share.Config.nSayRedMsgMaxLen);
            if (ReadInteger("Setup", "CanShoutMsgLevel", -1) < 0)
                WriteInteger("Setup", "CanShoutMsgLevel", M2Share.Config.nCanShoutMsgLevel);
            M2Share.Config.nCanShoutMsgLevel = ReadInteger("Setup", "CanShoutMsgLevel", M2Share.Config.nCanShoutMsgLevel);
            if (ReadInteger("Setup", "StartPermission", -1) < 0)
                WriteInteger("Setup", "StartPermission", M2Share.Config.nStartPermission);
            M2Share.Config.nStartPermission = ReadInteger("Setup", "StartPermission", M2Share.Config.nStartPermission);
            if (ReadInteger("Setup", "SendRefMsgRange", -1) < 0)
                WriteInteger("Setup", "SendRefMsgRange", M2Share.Config.nSendRefMsgRange);
            M2Share.Config.nSendRefMsgRange = (byte)ReadInteger("Setup", "SendRefMsgRange", M2Share.Config.nSendRefMsgRange);
            if (ReadInteger("Setup", "DecLampDura", -1) < 0)
                WriteBool("Setup", "DecLampDura", M2Share.Config.boDecLampDura);
            M2Share.Config.boDecLampDura = ReadBool("Setup", "DecLampDura", M2Share.Config.boDecLampDura);
            if (ReadInteger("Setup", "HungerSystem", -1) < 0)
                WriteBool("Setup", "HungerSystem", M2Share.Config.boHungerSystem);
            M2Share.Config.boHungerSystem = ReadBool("Setup", "HungerSystem", M2Share.Config.boHungerSystem);
            if (ReadInteger("Setup", "HungerDecHP", -1) < 0)
                WriteBool("Setup", "HungerDecHP", M2Share.Config.boHungerDecHP);
            M2Share.Config.boHungerDecHP = ReadBool("Setup", "HungerDecHP", M2Share.Config.boHungerDecHP);
            if (ReadInteger("Setup", "HungerDecPower", -1) < 0)
                WriteBool("Setup", "HungerDecPower", M2Share.Config.boHungerDecPower);
            M2Share.Config.boHungerDecPower = ReadBool("Setup", "HungerDecPower", M2Share.Config.boHungerDecPower);
            if (ReadInteger("Setup", "DiableHumanRun", -1) < 0)
                WriteBool("Setup", "DiableHumanRun", M2Share.Config.boDiableHumanRun);
            M2Share.Config.boDiableHumanRun = ReadBool("Setup", "DiableHumanRun", M2Share.Config.boDiableHumanRun);
            if (ReadInteger("Setup", "RunHuman", -1) < 0)
                WriteBool("Setup", "RunHuman", M2Share.Config.boRunHuman);
            M2Share.Config.boRunHuman = ReadBool("Setup", "RunHuman", M2Share.Config.boRunHuman);
            if (ReadInteger("Setup", "RunMon", -1) < 0)
                WriteBool("Setup", "RunMon", M2Share.Config.boRunMon);
            M2Share.Config.boRunMon = ReadBool("Setup", "RunMon", M2Share.Config.boRunMon);
            if (ReadInteger("Setup", "RunNpc", -1) < 0)
                WriteBool("Setup", "RunNpc", M2Share.Config.boRunNpc);
            M2Share.Config.boRunNpc = ReadBool("Setup", "RunNpc", M2Share.Config.boRunNpc);
            nLoadInteger = ReadInteger("Setup", "RunGuard", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "RunGuard", M2Share.Config.boRunGuard);
            else
                M2Share.Config.boRunGuard = nLoadInteger == 1;
            if (ReadInteger("Setup", "WarDisableHumanRun", -1) < 0)
                WriteBool("Setup", "WarDisableHumanRun", M2Share.Config.boWarDisHumRun);
            M2Share.Config.boWarDisHumRun = ReadBool("Setup", "WarDisableHumanRun", M2Share.Config.boWarDisHumRun);
            if (ReadInteger("Setup", "GMRunAll", -1) < 0)
            {
                WriteBool("Setup", "GMRunAll", M2Share.Config.boGMRunAll);
            }
            M2Share.Config.boGMRunAll = ReadBool("Setup", "GMRunAll", M2Share.Config.boGMRunAll);
            if (ReadInteger("Setup", "SkeletonCount", -1) < 0)
            {
                WriteInteger("Setup", "SkeletonCount", M2Share.Config.nSkeletonCount);
            }
            M2Share.Config.nSkeletonCount = ReadInteger("Setup", "SkeletonCount", M2Share.Config.nSkeletonCount);
            for (var i = 0; i < M2Share.Config.SkeletonArray.Length; i++)
            {
                if (ReadInteger("Setup", "SkeletonHumLevel" + i, -1) < 0)
                {
                    WriteInteger("Setup", "SkeletonHumLevel" + i, M2Share.Config.SkeletonArray[i].nHumLevel);
                }
                M2Share.Config.SkeletonArray[i].nHumLevel = ReadInteger("Setup", "SkeletonHumLevel" + i, M2Share.Config.SkeletonArray[i].nHumLevel);
                if (ReadString("Names", "Skeleton" + i, "") == "")
                {
                    WriteString("Names", "Skeleton" + i, M2Share.Config.SkeletonArray[i].sMonName);
                }
                M2Share.Config.SkeletonArray[i].sMonName = ReadString("Names", "Skeleton" + i, M2Share.Config.SkeletonArray[i].sMonName);
                if (ReadInteger("Setup", "SkeletonCount" + i, -1) < 0)
                {
                    WriteInteger("Setup", "SkeletonCount" + i, M2Share.Config.SkeletonArray[i].nCount);
                }
                M2Share.Config.SkeletonArray[i].nCount = ReadInteger("Setup", "SkeletonCount" + i, M2Share.Config.SkeletonArray[i].nCount);
                if (ReadInteger("Setup", "SkeletonLevel" + i, -1) < 0)
                {
                    WriteInteger("Setup", "SkeletonLevel" + i, M2Share.Config.SkeletonArray[i].nLevel);
                }
                M2Share.Config.SkeletonArray[i].nLevel = ReadInteger("Setup", "SkeletonLevel" + i, M2Share.Config.SkeletonArray[i].nLevel);
            }
            if (ReadInteger("Setup", "DragonCount", -1) < 0)
            {
                WriteInteger("Setup", "DragonCount", M2Share.Config.nDragonCount);
            }
            M2Share.Config.nDragonCount = ReadInteger("Setup", "DragonCount", M2Share.Config.nDragonCount);
            for (var i = 0; i < M2Share.Config.DragonArray.Length; i++)
            {
                if (ReadInteger("Setup", "DragonHumLevel" + i, -1) < 0)
                {
                    WriteInteger("Setup", "DragonHumLevel" + i, M2Share.Config.DragonArray[i].nHumLevel);
                }
                M2Share.Config.DragonArray[i].nHumLevel = ReadInteger("Setup", "DragonHumLevel" + i, M2Share.Config.DragonArray[i].nHumLevel);
                if (ReadString("Names", "Dragon" + i, "") == "")
                {
                    WriteString("Names", "Dragon" + i, M2Share.Config.DragonArray[i].sMonName);
                }
                M2Share.Config.DragonArray[i].sMonName = ReadString("Names", "Dragon" + i, M2Share.Config.DragonArray[i].sMonName);
                if (ReadInteger("Setup", "DragonCount" + i, -1) < 0)
                {
                    WriteInteger("Setup", "DragonCount" + i, M2Share.Config.DragonArray[i].nCount);
                }
                M2Share.Config.DragonArray[i].nCount = ReadInteger("Setup", "DragonCount" + i, M2Share.Config.DragonArray[i].nCount);
                if (ReadInteger("Setup", "DragonLevel" + i, -1) < 0)
                {
                    WriteInteger("Setup", "DragonLevel" + i, M2Share.Config.DragonArray[i].nLevel);
                }
                M2Share.Config.DragonArray[i].nLevel = ReadInteger("Setup", "DragonLevel" + i, M2Share.Config.DragonArray[i].nLevel);
            }
            if (ReadInteger("Setup", "TryDealTime", -1) < 0)
                WriteInteger("Setup", "TryDealTime", M2Share.Config.dwTryDealTime);
            M2Share.Config.dwTryDealTime = ReadInteger("Setup", "TryDealTime", M2Share.Config.dwTryDealTime);
            if (ReadInteger("Setup", "DealOKTime", -1) < 0)
                WriteInteger("Setup", "DealOKTime", M2Share.Config.dwDealOKTime);
            M2Share.Config.dwDealOKTime = ReadInteger("Setup", "DealOKTime", M2Share.Config.dwDealOKTime);
            if (ReadInteger("Setup", "CanNotGetBackDeal", -1) < 0)
                WriteBool("Setup", "CanNotGetBackDeal", M2Share.Config.boCanNotGetBackDeal);
            M2Share.Config.boCanNotGetBackDeal = ReadBool("Setup", "CanNotGetBackDeal", M2Share.Config.boCanNotGetBackDeal);
            if (ReadInteger("Setup", "DisableDeal", -1) < 0)
                WriteBool("Setup", "DisableDeal", M2Share.Config.boDisableDeal);
            M2Share.Config.boDisableDeal = ReadBool("Setup", "DisableDeal", M2Share.Config.boDisableDeal);
            if (ReadInteger("Setup", "MasterOKLevel", -1) < 0)
                WriteInteger("Setup", "MasterOKLevel", M2Share.Config.nMasterOKLevel);
            M2Share.Config.nMasterOKLevel = ReadInteger("Setup", "MasterOKLevel", M2Share.Config.nMasterOKLevel);
            if (ReadInteger("Setup", "MasterOKCreditPoint", -1) < 0)
                WriteInteger("Setup", "MasterOKCreditPoint", M2Share.Config.nMasterOKCreditPoint);
            M2Share.Config.nMasterOKCreditPoint = ReadInteger("Setup", "MasterOKCreditPoint", M2Share.Config.nMasterOKCreditPoint);
            if (ReadInteger("Setup", "MasterOKBonusPoint", -1) < 0)
                WriteInteger("Setup", "MasterOKBonusPoint", M2Share.Config.nMasterOKBonusPoint);
            M2Share.Config.nMasterOKBonusPoint = ReadInteger("Setup", "MasterOKBonusPoint", M2Share.Config.nMasterOKBonusPoint);
            if (ReadInteger("Setup", "PKProtect", -1) < 0)
                WriteBool("Setup", "PKProtect", M2Share.Config.boPKLevelProtect);
            M2Share.Config.boPKLevelProtect = ReadBool("Setup", "PKProtect", M2Share.Config.boPKLevelProtect);
            if (ReadInteger("Setup", "PKProtectLevel", -1) < 0)
                WriteInteger("Setup", "PKProtectLevel", M2Share.Config.nPKProtectLevel);
            M2Share.Config.nPKProtectLevel = ReadInteger("Setup", "PKProtectLevel", M2Share.Config.nPKProtectLevel);
            if (ReadInteger("Setup", "RedPKProtectLevel", -1) < 0)
                WriteInteger("Setup", "RedPKProtectLevel", M2Share.Config.nRedPKProtectLevel);
            M2Share.Config.nRedPKProtectLevel = ReadInteger("Setup", "RedPKProtectLevel", M2Share.Config.nRedPKProtectLevel);
            if (ReadInteger("Setup", "ItemPowerRate", -1) < 0)
                WriteInteger("Setup", "ItemPowerRate", M2Share.Config.nItemPowerRate);
            M2Share.Config.nItemPowerRate = ReadInteger("Setup", "ItemPowerRate", M2Share.Config.nItemPowerRate);
            if (ReadInteger("Setup", "ItemExpRate", -1) < 0)
                WriteInteger("Setup", "ItemExpRate", M2Share.Config.nItemExpRate);
            M2Share.Config.nItemExpRate = ReadInteger("Setup", "ItemExpRate", M2Share.Config.nItemExpRate);
            if (ReadInteger("Setup", "ScriptGotoCountLimit", -1) < 0)
                WriteInteger("Setup", "ScriptGotoCountLimit", M2Share.Config.nScriptGotoCountLimit);
            M2Share.Config.nScriptGotoCountLimit = ReadInteger("Setup", "ScriptGotoCountLimit", M2Share.Config.nScriptGotoCountLimit);
            if (ReadInteger("Setup", "HearMsgFColor", -1) < 0)
                WriteInteger("Setup", "HearMsgFColor", M2Share.Config.btHearMsgFColor);
            M2Share.Config.btHearMsgFColor = Read<byte>("Setup", "HearMsgFColor", M2Share.Config.btHearMsgFColor);
            if (ReadInteger("Setup", "HearMsgBColor", -1) < 0)
                WriteInteger("Setup", "HearMsgBColor", M2Share.Config.btHearMsgBColor);
            M2Share.Config.btHearMsgBColor = Read<byte>("Setup", "HearMsgBColor", M2Share.Config.btHearMsgBColor);
            if (ReadInteger("Setup", "WhisperMsgFColor", -1) < 0)
                WriteInteger("Setup", "WhisperMsgFColor", M2Share.Config.btWhisperMsgFColor);
            M2Share.Config.btWhisperMsgFColor = Read<byte>("Setup", "WhisperMsgFColor", M2Share.Config.btWhisperMsgFColor);
            if (ReadInteger("Setup", "WhisperMsgBColor", -1) < 0)
                WriteInteger("Setup", "WhisperMsgBColor", M2Share.Config.btWhisperMsgBColor);
            M2Share.Config.btWhisperMsgBColor = Read<byte>("Setup", "WhisperMsgBColor", M2Share.Config.btWhisperMsgBColor);
            if (ReadInteger("Setup", "GMWhisperMsgFColor", -1) < 0)
                WriteInteger("Setup", "GMWhisperMsgFColor", M2Share.Config.btGMWhisperMsgFColor);
            M2Share.Config.btGMWhisperMsgFColor = Read<byte>("Setup", "GMWhisperMsgFColor", M2Share.Config.btGMWhisperMsgFColor);
            if (ReadInteger("Setup", "GMWhisperMsgBColor", -1) < 0)
                WriteInteger("Setup", "GMWhisperMsgBColor", M2Share.Config.btGMWhisperMsgBColor);
            M2Share.Config.btGMWhisperMsgBColor = Read<byte>("Setup", "GMWhisperMsgBColor", M2Share.Config.btGMWhisperMsgBColor);
            if (ReadInteger("Setup", "CryMsgFColor", -1) < 0)
                WriteInteger("Setup", "CryMsgFColor", M2Share.Config.btCryMsgFColor);
            M2Share.Config.btCryMsgFColor = Read<byte>("Setup", "CryMsgFColor", M2Share.Config.btCryMsgFColor);
            if (ReadInteger("Setup", "CryMsgBColor", -1) < 0)
                WriteInteger("Setup", "CryMsgBColor", M2Share.Config.btCryMsgBColor);
            M2Share.Config.btCryMsgBColor = Read<byte>("Setup", "CryMsgBColor", M2Share.Config.btCryMsgBColor);
            if (ReadInteger("Setup", "GreenMsgFColor", -1) < 0)
                WriteInteger("Setup", "GreenMsgFColor", M2Share.Config.btGreenMsgFColor);
            M2Share.Config.btGreenMsgFColor = Read<byte>("Setup", "GreenMsgFColor", M2Share.Config.btGreenMsgFColor);
            if (ReadInteger("Setup", "GreenMsgBColor", -1) < 0)
                WriteInteger("Setup", "GreenMsgBColor", M2Share.Config.btGreenMsgBColor);
            M2Share.Config.btGreenMsgBColor = Read<byte>("Setup", "GreenMsgBColor", M2Share.Config.btGreenMsgBColor);
            if (ReadInteger("Setup", "BlueMsgFColor", -1) < 0)
                WriteInteger("Setup", "BlueMsgFColor", M2Share.Config.btBlueMsgFColor);
            M2Share.Config.btBlueMsgFColor = Read<byte>("Setup", "BlueMsgFColor", M2Share.Config.btBlueMsgFColor);
            if (ReadInteger("Setup", "BlueMsgBColor", -1) < 0)
                WriteInteger("Setup", "BlueMsgBColor", M2Share.Config.btBlueMsgBColor);
            M2Share.Config.btBlueMsgBColor = Read<byte>("Setup", "BlueMsgBColor", M2Share.Config.btBlueMsgBColor);
            if (ReadInteger("Setup", "RedMsgFColor", -1) < 0)
                WriteInteger("Setup", "RedMsgFColor", M2Share.Config.btRedMsgFColor);
            M2Share.Config.btRedMsgFColor = Read<byte>("Setup", "RedMsgFColor", M2Share.Config.btRedMsgFColor);
            if (ReadInteger("Setup", "RedMsgBColor", -1) < 0)
                WriteInteger("Setup", "RedMsgBColor", M2Share.Config.btRedMsgBColor);
            M2Share.Config.btRedMsgBColor = Read<byte>("Setup", "RedMsgBColor", M2Share.Config.btRedMsgBColor);
            if (ReadInteger("Setup", "GuildMsgFColor", -1) < 0)
                WriteInteger("Setup", "GuildMsgFColor", M2Share.Config.btGuildMsgFColor);
            M2Share.Config.btGuildMsgFColor = Read<byte>("Setup", "GuildMsgFColor", M2Share.Config.btGuildMsgFColor);
            if (ReadInteger("Setup", "GuildMsgBColor", -1) < 0)
                WriteInteger("Setup", "GuildMsgBColor", M2Share.Config.btGuildMsgBColor);
            M2Share.Config.btGuildMsgBColor = Read<byte>("Setup", "GuildMsgBColor", M2Share.Config.btGuildMsgBColor);
            if (ReadInteger("Setup", "GroupMsgFColor", -1) < 0)
                WriteInteger("Setup", "GroupMsgFColor", M2Share.Config.btGroupMsgFColor);
            M2Share.Config.btGroupMsgFColor = Read<byte>("Setup", "GroupMsgFColor", M2Share.Config.btGroupMsgFColor);
            if (ReadInteger("Setup", "GroupMsgBColor", -1) < 0)
                WriteInteger("Setup", "GroupMsgBColor", M2Share.Config.btGroupMsgBColor);
            M2Share.Config.btGroupMsgBColor = Read<byte>("Setup", "GroupMsgBColor", M2Share.Config.btGroupMsgBColor);
            if (ReadInteger("Setup", "CustMsgFColor", -1) < 0)
                WriteInteger("Setup", "CustMsgFColor", M2Share.Config.btCustMsgFColor);
            M2Share.Config.btCustMsgFColor = Read<byte>("Setup", "CustMsgFColor", M2Share.Config.btCustMsgFColor);
            if (ReadInteger("Setup", "CustMsgBColor", -1) < 0)
                WriteInteger("Setup", "CustMsgBColor", M2Share.Config.btCustMsgBColor);
            M2Share.Config.btCustMsgBColor = Read<byte>("Setup", "CustMsgBColor", M2Share.Config.btCustMsgBColor);
            if (ReadInteger("Setup", "MonRandomAddValue", -1) < 0)
                WriteInteger("Setup", "MonRandomAddValue", M2Share.Config.nMonRandomAddValue);
            M2Share.Config.nMonRandomAddValue = ReadInteger("Setup", "MonRandomAddValue", M2Share.Config.nMonRandomAddValue);
            if (ReadInteger("Setup", "MakeRandomAddValue", -1) < 0)
                WriteInteger("Setup", "MakeRandomAddValue", M2Share.Config.nMakeRandomAddValue);
            M2Share.Config.nMakeRandomAddValue = ReadInteger("Setup", "MakeRandomAddValue", M2Share.Config.nMakeRandomAddValue);
            if (ReadInteger("Setup", "WeaponDCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "WeaponDCAddValueMaxLimit", M2Share.Config.nWeaponDCAddValueMaxLimit);
            M2Share.Config.nWeaponDCAddValueMaxLimit = ReadInteger("Setup", "WeaponDCAddValueMaxLimit", M2Share.Config.nWeaponDCAddValueMaxLimit);
            if (ReadInteger("Setup", "WeaponDCAddValueRate", -1) < 0)
                WriteInteger("Setup", "WeaponDCAddValueRate", M2Share.Config.nWeaponDCAddValueRate);
            M2Share.Config.nWeaponDCAddValueRate = ReadInteger("Setup", "WeaponDCAddValueRate", M2Share.Config.nWeaponDCAddValueRate);
            if (ReadInteger("Setup", "WeaponMCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "WeaponMCAddValueMaxLimit", M2Share.Config.nWeaponMCAddValueMaxLimit);
            M2Share.Config.nWeaponMCAddValueMaxLimit = ReadInteger("Setup", "WeaponMCAddValueMaxLimit", M2Share.Config.nWeaponMCAddValueMaxLimit);
            if (ReadInteger("Setup", "WeaponMCAddValueRate", -1) < 0)
                WriteInteger("Setup", "WeaponMCAddValueRate", M2Share.Config.nWeaponMCAddValueRate);
            M2Share.Config.nWeaponMCAddValueRate = ReadInteger("Setup", "WeaponMCAddValueRate", M2Share.Config.nWeaponMCAddValueRate);
            if (ReadInteger("Setup", "WeaponSCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "WeaponSCAddValueMaxLimit", M2Share.Config.nWeaponSCAddValueMaxLimit);
            M2Share.Config.nWeaponSCAddValueMaxLimit = ReadInteger("Setup", "WeaponSCAddValueMaxLimit", M2Share.Config.nWeaponSCAddValueMaxLimit);
            if (ReadInteger("Setup", "WeaponSCAddValueRate", -1) < 0)
                WriteInteger("Setup", "WeaponSCAddValueRate", M2Share.Config.nWeaponSCAddValueRate);
            M2Share.Config.nWeaponSCAddValueRate = ReadInteger("Setup", "WeaponSCAddValueRate", M2Share.Config.nWeaponSCAddValueRate);
            if (ReadInteger("Setup", "DressDCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "DressDCAddValueMaxLimit", M2Share.Config.nDressDCAddValueMaxLimit);
            M2Share.Config.nDressDCAddValueMaxLimit = ReadInteger("Setup", "DressDCAddValueMaxLimit", M2Share.Config.nDressDCAddValueMaxLimit);
            if (ReadInteger("Setup", "DressDCAddValueRate", -1) < 0)
                WriteInteger("Setup", "DressDCAddValueRate", M2Share.Config.nDressDCAddValueRate);
            M2Share.Config.nDressDCAddValueRate = ReadInteger("Setup", "DressDCAddValueRate", M2Share.Config.nDressDCAddValueRate);
            if (ReadInteger("Setup", "DressDCAddRate", -1) < 0)
                WriteInteger("Setup", "DressDCAddRate", M2Share.Config.nDressDCAddRate);
            M2Share.Config.nDressDCAddRate = ReadInteger("Setup", "DressDCAddRate", M2Share.Config.nDressDCAddRate);
            if (ReadInteger("Setup", "DressMCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "DressMCAddValueMaxLimit", M2Share.Config.nDressMCAddValueMaxLimit);
            M2Share.Config.nDressMCAddValueMaxLimit = ReadInteger("Setup", "DressMCAddValueMaxLimit", M2Share.Config.nDressMCAddValueMaxLimit);
            if (ReadInteger("Setup", "DressMCAddValueRate", -1) < 0)
                WriteInteger("Setup", "DressMCAddValueRate", M2Share.Config.nDressMCAddValueRate);
            M2Share.Config.nDressMCAddValueRate = ReadInteger("Setup", "DressMCAddValueRate", M2Share.Config.nDressMCAddValueRate);
            if (ReadInteger("Setup", "DressMCAddRate", -1) < 0)
                WriteInteger("Setup", "DressMCAddRate", M2Share.Config.nDressMCAddRate);
            M2Share.Config.nDressMCAddRate = ReadInteger("Setup", "DressMCAddRate", M2Share.Config.nDressMCAddRate);
            if (ReadInteger("Setup", "DressSCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "DressSCAddValueMaxLimit", M2Share.Config.nDressSCAddValueMaxLimit);
            M2Share.Config.nDressSCAddValueMaxLimit = ReadInteger("Setup", "DressSCAddValueMaxLimit", M2Share.Config.nDressSCAddValueMaxLimit);
            if (ReadInteger("Setup", "DressSCAddValueRate", -1) < 0)
                WriteInteger("Setup", "DressSCAddValueRate", M2Share.Config.nDressSCAddValueRate);
            M2Share.Config.nDressSCAddValueRate = ReadInteger("Setup", "DressSCAddValueRate", M2Share.Config.nDressSCAddValueRate);
            if (ReadInteger("Setup", "DressSCAddRate", -1) < 0)
                WriteInteger("Setup", "DressSCAddRate", M2Share.Config.nDressSCAddRate);
            M2Share.Config.nDressSCAddRate = ReadInteger("Setup", "DressSCAddRate", M2Share.Config.nDressSCAddRate);
            if (ReadInteger("Setup", "NeckLace19DCAddValueMaxLimit", -1) < 0)
            {
                WriteInteger("Setup", "NeckLace19DCAddValueMaxLimit", M2Share.Config.nNeckLace19DCAddValueMaxLimit);
            }
            M2Share.Config.nNeckLace19DCAddValueMaxLimit = ReadInteger("Setup", "NeckLace19DCAddValueMaxLimit", M2Share.Config.nNeckLace19DCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace19DCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19DCAddValueRate", M2Share.Config.nNeckLace19DCAddValueRate);
            M2Share.Config.nNeckLace19DCAddValueRate = ReadInteger("Setup", "NeckLace19DCAddValueRate", M2Share.Config.nNeckLace19DCAddValueRate);
            if (ReadInteger("Setup", "NeckLace19DCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19DCAddRate", M2Share.Config.nNeckLace19DCAddRate);
            M2Share.Config.nNeckLace19DCAddRate = ReadInteger("Setup", "NeckLace19DCAddRate", M2Share.Config.nNeckLace19DCAddRate);
            if (ReadInteger("Setup", "NeckLace19MCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "NeckLace19MCAddValueMaxLimit", M2Share.Config.nNeckLace19MCAddValueMaxLimit);
            M2Share.Config.nNeckLace19MCAddValueMaxLimit = ReadInteger("Setup", "NeckLace19MCAddValueMaxLimit", M2Share.Config.nNeckLace19MCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace19MCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19MCAddValueRate", M2Share.Config.nNeckLace19MCAddValueRate);
            M2Share.Config.nNeckLace19MCAddValueRate = ReadInteger("Setup", "NeckLace19MCAddValueRate", M2Share.Config.nNeckLace19MCAddValueRate);
            if (ReadInteger("Setup", "NeckLace19MCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19MCAddRate", M2Share.Config.nNeckLace19MCAddRate);
            M2Share.Config.nNeckLace19MCAddRate = ReadInteger("Setup", "NeckLace19MCAddRate", M2Share.Config.nNeckLace19MCAddRate);
            if (ReadInteger("Setup", "NeckLace19SCAddValueMaxLimit", -1) < 0)
            {
                WriteInteger("Setup", "NeckLace19SCAddValueMaxLimit", M2Share.Config.nNeckLace19SCAddValueMaxLimit);
            }
            M2Share.Config.nNeckLace19SCAddValueMaxLimit = ReadInteger("Setup", "NeckLace19SCAddValueMaxLimit", M2Share.Config.nNeckLace19SCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace19SCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19SCAddValueRate", M2Share.Config.nNeckLace19SCAddValueRate);
            M2Share.Config.nNeckLace19SCAddValueRate = ReadInteger("Setup", "NeckLace19SCAddValueRate", M2Share.Config.nNeckLace19SCAddValueRate);
            if (ReadInteger("Setup", "NeckLace19SCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19SCAddRate", M2Share.Config.nNeckLace19SCAddRate);
            M2Share.Config.nNeckLace19SCAddRate = ReadInteger("Setup", "NeckLace19SCAddRate", M2Share.Config.nNeckLace19SCAddRate);
            if (ReadInteger("Setup", "NeckLace202124DCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "NeckLace202124DCAddValueMaxLimit", M2Share.Config.nNeckLace202124DCAddValueMaxLimit);
            M2Share.Config.nNeckLace202124DCAddValueMaxLimit = ReadInteger("Setup", "NeckLace202124DCAddValueMaxLimit", M2Share.Config.nNeckLace202124DCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace202124DCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124DCAddValueRate", M2Share.Config.nNeckLace202124DCAddValueRate);
            M2Share.Config.nNeckLace202124DCAddValueRate = ReadInteger("Setup", "NeckLace202124DCAddValueRate", M2Share.Config.nNeckLace202124DCAddValueRate);
            if (ReadInteger("Setup", "NeckLace202124DCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124DCAddRate", M2Share.Config.nNeckLace202124DCAddRate);
            M2Share.Config.nNeckLace202124DCAddRate = ReadInteger("Setup", "NeckLace202124DCAddRate", M2Share.Config.nNeckLace202124DCAddRate);
            if (ReadInteger("Setup", "NeckLace202124MCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "NeckLace202124MCAddValueMaxLimit", M2Share.Config.nNeckLace202124MCAddValueMaxLimit);
            M2Share.Config.nNeckLace202124MCAddValueMaxLimit = ReadInteger("Setup", "NeckLace202124MCAddValueMaxLimit", M2Share.Config.nNeckLace202124MCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace202124MCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124MCAddValueRate", M2Share.Config.nNeckLace202124MCAddValueRate);
            M2Share.Config.nNeckLace202124MCAddValueRate = ReadInteger("Setup", "NeckLace202124MCAddValueRate", M2Share.Config.nNeckLace202124MCAddValueRate);
            if (ReadInteger("Setup", "NeckLace202124MCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124MCAddRate", M2Share.Config.nNeckLace202124MCAddRate);
            M2Share.Config.nNeckLace202124MCAddRate = ReadInteger("Setup", "NeckLace202124MCAddRate", M2Share.Config.nNeckLace202124MCAddRate);
            if (ReadInteger("Setup", "NeckLace202124SCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "NeckLace202124SCAddValueMaxLimit", M2Share.Config.nNeckLace202124SCAddValueMaxLimit);
            M2Share.Config.nNeckLace202124SCAddValueMaxLimit = ReadInteger("Setup", "NeckLace202124SCAddValueMaxLimit", M2Share.Config.nNeckLace202124SCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace202124SCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124SCAddValueRate", M2Share.Config.nNeckLace202124SCAddValueRate);
            M2Share.Config.nNeckLace202124SCAddValueRate = ReadInteger("Setup", "NeckLace202124SCAddValueRate", M2Share.Config.nNeckLace202124SCAddValueRate);
            if (ReadInteger("Setup", "NeckLace202124SCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124SCAddRate", M2Share.Config.nNeckLace202124SCAddRate);
            M2Share.Config.nNeckLace202124SCAddRate = ReadInteger("Setup", "NeckLace202124SCAddRate", M2Share.Config.nNeckLace202124SCAddRate);
            if (ReadInteger("Setup", "ArmRing26DCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "ArmRing26DCAddValueMaxLimit", M2Share.Config.nArmRing26DCAddValueMaxLimit);
            M2Share.Config.nArmRing26DCAddValueMaxLimit = ReadInteger("Setup", "ArmRing26DCAddValueMaxLimit", M2Share.Config.nArmRing26DCAddValueMaxLimit);
            if (ReadInteger("Setup", "ArmRing26DCAddValueRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26DCAddValueRate", M2Share.Config.nArmRing26DCAddValueRate);
            M2Share.Config.nArmRing26DCAddValueRate = ReadInteger("Setup", "ArmRing26DCAddValueRate", M2Share.Config.nArmRing26DCAddValueRate);
            if (ReadInteger("Setup", "ArmRing26DCAddRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26DCAddRate", M2Share.Config.nArmRing26DCAddRate);
            M2Share.Config.nArmRing26DCAddRate = ReadInteger("Setup", "ArmRing26DCAddRate", M2Share.Config.nArmRing26DCAddRate);
            if (ReadInteger("Setup", "ArmRing26MCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "ArmRing26MCAddValueMaxLimit", M2Share.Config.nArmRing26MCAddValueMaxLimit);
            M2Share.Config.nArmRing26MCAddValueMaxLimit = ReadInteger("Setup", "ArmRing26MCAddValueMaxLimit", M2Share.Config.nArmRing26MCAddValueMaxLimit);
            if (ReadInteger("Setup", "ArmRing26MCAddValueRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26MCAddValueRate", M2Share.Config.nArmRing26MCAddValueRate);
            M2Share.Config.nArmRing26MCAddValueRate = ReadInteger("Setup", "ArmRing26MCAddValueRate", M2Share.Config.nArmRing26MCAddValueRate);
            if (ReadInteger("Setup", "ArmRing26MCAddRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26MCAddRate", M2Share.Config.nArmRing26MCAddRate);
            M2Share.Config.nArmRing26MCAddRate = ReadInteger("Setup", "ArmRing26MCAddRate", M2Share.Config.nArmRing26MCAddRate);
            if (ReadInteger("Setup", "ArmRing26SCAddValueMaxLimit", -1) < 0)
            {
                WriteInteger("Setup", "ArmRing26SCAddValueMaxLimit", M2Share.Config.nArmRing26SCAddValueMaxLimit);
            }
            M2Share.Config.nArmRing26SCAddValueMaxLimit = ReadInteger("Setup", "ArmRing26SCAddValueMaxLimit", M2Share.Config.nArmRing26SCAddValueMaxLimit);
            if (ReadInteger("Setup", "ArmRing26SCAddValueRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26SCAddValueRate", M2Share.Config.nArmRing26SCAddValueRate);
            M2Share.Config.nArmRing26SCAddValueRate = ReadInteger("Setup", "ArmRing26SCAddValueRate", M2Share.Config.nArmRing26SCAddValueRate);
            if (ReadInteger("Setup", "ArmRing26SCAddRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26SCAddRate", M2Share.Config.nArmRing26SCAddRate);
            M2Share.Config.nArmRing26SCAddRate = ReadInteger("Setup", "ArmRing26SCAddRate", M2Share.Config.nArmRing26SCAddRate);
            if (ReadInteger("Setup", "Ring22DCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring22DCAddValueMaxLimit", M2Share.Config.nRing22DCAddValueMaxLimit);
            M2Share.Config.nRing22DCAddValueMaxLimit = ReadInteger("Setup", "Ring22DCAddValueMaxLimit", M2Share.Config.nRing22DCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring22DCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring22DCAddValueRate", M2Share.Config.nRing22DCAddValueRate);
            M2Share.Config.nRing22DCAddValueRate = ReadInteger("Setup", "Ring22DCAddValueRate", M2Share.Config.nRing22DCAddValueRate);
            if (ReadInteger("Setup", "Ring22DCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring22DCAddRate", M2Share.Config.nRing22DCAddRate);
            M2Share.Config.nRing22DCAddRate = ReadInteger("Setup", "Ring22DCAddRate", M2Share.Config.nRing22DCAddRate);
            if (ReadInteger("Setup", "Ring22MCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring22MCAddValueMaxLimit", M2Share.Config.nRing22MCAddValueMaxLimit);
            M2Share.Config.nRing22MCAddValueMaxLimit = ReadInteger("Setup", "Ring22MCAddValueMaxLimit", M2Share.Config.nRing22MCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring22MCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring22MCAddValueRate", M2Share.Config.nRing22MCAddValueRate);
            M2Share.Config.nRing22MCAddValueRate = ReadInteger("Setup", "Ring22MCAddValueRate", M2Share.Config.nRing22MCAddValueRate);
            if (ReadInteger("Setup", "Ring22MCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring22MCAddRate", M2Share.Config.nRing22MCAddRate);
            M2Share.Config.nRing22MCAddRate = ReadInteger("Setup", "Ring22MCAddRate", M2Share.Config.nRing22MCAddRate);
            if (ReadInteger("Setup", "Ring22SCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring22SCAddValueMaxLimit", M2Share.Config.nRing22SCAddValueMaxLimit);
            M2Share.Config.nRing22SCAddValueMaxLimit = ReadInteger("Setup", "Ring22SCAddValueMaxLimit", M2Share.Config.nRing22SCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring22SCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring22SCAddValueRate", M2Share.Config.nRing22SCAddValueRate);
            M2Share.Config.nRing22SCAddValueRate = ReadInteger("Setup", "Ring22SCAddValueRate", M2Share.Config.nRing22SCAddValueRate);
            if (ReadInteger("Setup", "Ring22SCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring22SCAddRate", M2Share.Config.nRing22SCAddRate);
            M2Share.Config.nRing22SCAddRate = ReadInteger("Setup", "Ring22SCAddRate", M2Share.Config.nRing22SCAddRate);
            if (ReadInteger("Setup", "Ring23DCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring23DCAddValueMaxLimit", M2Share.Config.nRing23DCAddValueMaxLimit);
            M2Share.Config.nRing23DCAddValueMaxLimit = ReadInteger("Setup", "Ring23DCAddValueMaxLimit", M2Share.Config.nRing23DCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring23DCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring23DCAddValueRate", M2Share.Config.nRing23DCAddValueRate);
            M2Share.Config.nRing23DCAddValueRate = ReadInteger("Setup", "Ring23DCAddValueRate", M2Share.Config.nRing23DCAddValueRate);
            if (ReadInteger("Setup", "Ring23DCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring23DCAddRate", M2Share.Config.nRing23DCAddRate);
            M2Share.Config.nRing23DCAddRate = ReadInteger("Setup", "Ring23DCAddRate", M2Share.Config.nRing23DCAddRate);
            if (ReadInteger("Setup", "Ring23MCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring23MCAddValueMaxLimit", M2Share.Config.nRing23MCAddValueMaxLimit);
            M2Share.Config.nRing23MCAddValueMaxLimit = ReadInteger("Setup", "Ring23MCAddValueMaxLimit", M2Share.Config.nRing23MCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring23MCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring23MCAddValueRate", M2Share.Config.nRing23MCAddValueRate);
            M2Share.Config.nRing23MCAddValueRate = ReadInteger("Setup", "Ring23MCAddValueRate", M2Share.Config.nRing23MCAddValueRate);
            if (ReadInteger("Setup", "Ring23MCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring23MCAddRate", M2Share.Config.nRing23MCAddRate);
            M2Share.Config.nRing23MCAddRate = ReadInteger("Setup", "Ring23MCAddRate", M2Share.Config.nRing23MCAddRate);
            if (ReadInteger("Setup", "Ring23SCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring23SCAddValueMaxLimit", M2Share.Config.nRing23SCAddValueMaxLimit);
            M2Share.Config.nRing23SCAddValueMaxLimit = ReadInteger("Setup", "Ring23SCAddValueMaxLimit", M2Share.Config.nRing23SCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring23SCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring23SCAddValueRate", M2Share.Config.nRing23SCAddValueRate);
            M2Share.Config.nRing23SCAddValueRate = ReadInteger("Setup", "Ring23SCAddValueRate", M2Share.Config.nRing23SCAddValueRate);
            if (ReadInteger("Setup", "Ring23SCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring23SCAddRate", M2Share.Config.nRing23SCAddRate);
            M2Share.Config.nRing23SCAddRate = ReadInteger("Setup", "Ring23SCAddRate", M2Share.Config.nRing23SCAddRate);
            if (ReadInteger("Setup", "HelMetDCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "HelMetDCAddValueMaxLimit", M2Share.Config.nHelMetDCAddValueMaxLimit);
            M2Share.Config.nHelMetDCAddValueMaxLimit = ReadInteger("Setup", "HelMetDCAddValueMaxLimit", M2Share.Config.nHelMetDCAddValueMaxLimit);
            if (ReadInteger("Setup", "HelMetDCAddValueRate", -1) < 0)
                WriteInteger("Setup", "HelMetDCAddValueRate", M2Share.Config.nHelMetDCAddValueRate);
            M2Share.Config.nHelMetDCAddValueRate = ReadInteger("Setup", "HelMetDCAddValueRate", M2Share.Config.nHelMetDCAddValueRate);
            if (ReadInteger("Setup", "HelMetDCAddRate", -1) < 0)
                WriteInteger("Setup", "HelMetDCAddRate", M2Share.Config.nHelMetDCAddRate);
            M2Share.Config.nHelMetDCAddRate = ReadInteger("Setup", "HelMetDCAddRate", M2Share.Config.nHelMetDCAddRate);
            if (ReadInteger("Setup", "HelMetMCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "HelMetMCAddValueMaxLimit", M2Share.Config.nHelMetMCAddValueMaxLimit);
            M2Share.Config.nHelMetMCAddValueMaxLimit = ReadInteger("Setup", "HelMetMCAddValueMaxLimit", M2Share.Config.nHelMetMCAddValueMaxLimit);
            if (ReadInteger("Setup", "HelMetMCAddValueRate", -1) < 0)
                WriteInteger("Setup", "HelMetMCAddValueRate", M2Share.Config.nHelMetMCAddValueRate);
            M2Share.Config.nHelMetMCAddValueRate = ReadInteger("Setup", "HelMetMCAddValueRate", M2Share.Config.nHelMetMCAddValueRate);
            if (ReadInteger("Setup", "HelMetMCAddRate", -1) < 0)
                WriteInteger("Setup", "HelMetMCAddRate", M2Share.Config.nHelMetMCAddRate);
            M2Share.Config.nHelMetMCAddRate = ReadInteger("Setup", "HelMetMCAddRate", M2Share.Config.nHelMetMCAddRate);
            if (ReadInteger("Setup", "HelMetSCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "HelMetSCAddValueMaxLimit", M2Share.Config.nHelMetSCAddValueMaxLimit);
            M2Share.Config.nHelMetSCAddValueMaxLimit = ReadInteger("Setup", "HelMetSCAddValueMaxLimit", M2Share.Config.nHelMetSCAddValueMaxLimit);
            if (ReadInteger("Setup", "HelMetSCAddValueRate", -1) < 0)
                WriteInteger("Setup", "HelMetSCAddValueRate", M2Share.Config.nHelMetSCAddValueRate);
            M2Share.Config.nHelMetSCAddValueRate = ReadInteger("Setup", "HelMetSCAddValueRate", M2Share.Config.nHelMetSCAddValueRate);
            if (ReadInteger("Setup", "HelMetSCAddRate", -1) < 0)
                WriteInteger("Setup", "HelMetSCAddRate", M2Share.Config.nHelMetSCAddRate);
            M2Share.Config.nHelMetSCAddRate = ReadInteger("Setup", "HelMetSCAddRate", M2Share.Config.nHelMetSCAddRate);
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetACAddRate", M2Share.Config.nUnknowHelMetACAddRate);
            else
                M2Share.Config.nUnknowHelMetACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetACAddValueMaxLimit", M2Share.Config.nUnknowHelMetACAddValueMaxLimit);
            else
                M2Share.Config.nUnknowHelMetACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetMACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetMACAddRate", M2Share.Config.nUnknowHelMetMACAddRate);
            else
                M2Share.Config.nUnknowHelMetMACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetMACAddValueMaxLimit", M2Share.Config.nUnknowHelMetMACAddValueMaxLimit);
            else
                M2Share.Config.nUnknowHelMetMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetDCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetDCAddRate", M2Share.Config.nUnknowHelMetDCAddRate);
            else
                M2Share.Config.nUnknowHelMetDCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetDCAddValueMaxLimit", M2Share.Config.nUnknowHelMetDCAddValueMaxLimit);
            else
                M2Share.Config.nUnknowHelMetDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetMCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetMCAddRate", M2Share.Config.nUnknowHelMetMCAddRate);
            else
                M2Share.Config.nUnknowHelMetMCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetMCAddValueMaxLimit", M2Share.Config.nUnknowHelMetMCAddValueMaxLimit);
            else
                M2Share.Config.nUnknowHelMetMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetSCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetSCAddRate", M2Share.Config.nUnknowHelMetSCAddRate);
            else
                M2Share.Config.nUnknowHelMetSCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetSCAddValueMaxLimit", M2Share.Config.nUnknowHelMetSCAddValueMaxLimit);
            else
                M2Share.Config.nUnknowHelMetSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceACAddRate", M2Share.Config.nUnknowNecklaceACAddRate);
            else
                M2Share.Config.nUnknowNecklaceACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceACAddValueMaxLimit", M2Share.Config.nUnknowNecklaceACAddValueMaxLimit);
            else
                M2Share.Config.nUnknowNecklaceACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceMACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMACAddRate", M2Share.Config.nUnknowNecklaceMACAddRate);
            else
                M2Share.Config.nUnknowNecklaceMACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMACAddValueMaxLimit", M2Share.Config.nUnknowNecklaceMACAddValueMaxLimit);
            else
                M2Share.Config.nUnknowNecklaceMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceDCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceDCAddRate", M2Share.Config.nUnknowNecklaceDCAddRate);
            else
                M2Share.Config.nUnknowNecklaceDCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceDCAddValueMaxLimit", M2Share.Config.nUnknowNecklaceDCAddValueMaxLimit);
            else
                M2Share.Config.nUnknowNecklaceDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceMCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMCAddRate", M2Share.Config.nUnknowNecklaceMCAddRate);
            else
                M2Share.Config.nUnknowNecklaceMCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMCAddValueMaxLimit", M2Share.Config.nUnknowNecklaceMCAddValueMaxLimit);
            else
                M2Share.Config.nUnknowNecklaceMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceSCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceSCAddRate", M2Share.Config.nUnknowNecklaceSCAddRate);
            else
                M2Share.Config.nUnknowNecklaceSCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceSCAddValueMaxLimit", M2Share.Config.nUnknowNecklaceSCAddValueMaxLimit);
            else
                M2Share.Config.nUnknowNecklaceSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingACAddRate", M2Share.Config.nUnknowRingACAddRate);
            else
                M2Share.Config.nUnknowRingACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingACAddValueMaxLimit", M2Share.Config.nUnknowRingACAddValueMaxLimit);
            else
                M2Share.Config.nUnknowRingACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingMACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMACAddRate", M2Share.Config.nUnknowRingMACAddRate);
            else
                M2Share.Config.nUnknowRingMACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMACAddValueMaxLimit", M2Share.Config.nUnknowRingMACAddValueMaxLimit);
            else
                M2Share.Config.nUnknowRingMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingDCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingDCAddRate", M2Share.Config.nUnknowRingDCAddRate);
            else
                M2Share.Config.nUnknowRingDCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingDCAddValueMaxLimit", M2Share.Config.nUnknowRingDCAddValueMaxLimit);
            else
                M2Share.Config.nUnknowRingDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingMCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMCAddRate", M2Share.Config.nUnknowRingMCAddRate);
            else
                M2Share.Config.nUnknowRingMCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMCAddValueMaxLimit", M2Share.Config.nUnknowRingMCAddValueMaxLimit);
            else
                M2Share.Config.nUnknowRingMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingSCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingSCAddRate", M2Share.Config.nUnknowRingSCAddRate);
            else
                M2Share.Config.nUnknowRingSCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingSCAddValueMaxLimit", M2Share.Config.nUnknowRingSCAddValueMaxLimit);
            else
                M2Share.Config.nUnknowRingSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MonOneDropGoldCount", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MonOneDropGoldCount", M2Share.Config.nMonOneDropGoldCount);
            else
                M2Share.Config.nMonOneDropGoldCount = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "SendCurTickCount", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "SendCurTickCount", M2Share.Config.boSendCurTickCount);
            else
                M2Share.Config.boSendCurTickCount = nLoadInteger == 1;
            if (ReadInteger("Setup", "MakeMineHitRate", -1) < 0)
                WriteInteger("Setup", "MakeMineHitRate", M2Share.Config.nMakeMineHitRate);
            M2Share.Config.nMakeMineHitRate = ReadInteger("Setup", "MakeMineHitRate", M2Share.Config.nMakeMineHitRate);
            if (ReadInteger("Setup", "MakeMineRate", -1) < 0)
                WriteInteger("Setup", "MakeMineRate", M2Share.Config.nMakeMineRate);
            M2Share.Config.nMakeMineRate = ReadInteger("Setup", "MakeMineRate", M2Share.Config.nMakeMineRate);
            if (ReadInteger("Setup", "StoneTypeRate", -1) < 0)
                WriteInteger("Setup", "StoneTypeRate", M2Share.Config.nStoneTypeRate);
            M2Share.Config.nStoneTypeRate = ReadInteger("Setup", "StoneTypeRate", M2Share.Config.nStoneTypeRate);
            if (ReadInteger("Setup", "StoneTypeRateMin", -1) < 0)
                WriteInteger("Setup", "StoneTypeRateMin", M2Share.Config.nStoneTypeRateMin);
            M2Share.Config.nStoneTypeRateMin = ReadInteger("Setup", "StoneTypeRateMin", M2Share.Config.nStoneTypeRateMin);
            if (ReadInteger("Setup", "GoldStoneMin", -1) < 0)
                WriteInteger("Setup", "GoldStoneMin", M2Share.Config.nGoldStoneMin);
            M2Share.Config.nGoldStoneMin = ReadInteger("Setup", "GoldStoneMin", M2Share.Config.nGoldStoneMin);
            if (ReadInteger("Setup", "GoldStoneMax", -1) < 0)
                WriteInteger("Setup", "GoldStoneMax", M2Share.Config.nGoldStoneMax);
            M2Share.Config.nGoldStoneMax = ReadInteger("Setup", "GoldStoneMax", M2Share.Config.nGoldStoneMax);
            if (ReadInteger("Setup", "SilverStoneMin", -1) < 0)
                WriteInteger("Setup", "SilverStoneMin", M2Share.Config.nSilverStoneMin);
            M2Share.Config.nSilverStoneMin = ReadInteger("Setup", "SilverStoneMin", M2Share.Config.nSilverStoneMin);
            if (ReadInteger("Setup", "SilverStoneMax", -1) < 0)
                WriteInteger("Setup", "SilverStoneMax", M2Share.Config.nSilverStoneMax);
            M2Share.Config.nSilverStoneMax = ReadInteger("Setup", "SilverStoneMax", M2Share.Config.nSilverStoneMax);
            if (ReadInteger("Setup", "SteelStoneMin", -1) < 0)
                WriteInteger("Setup", "SteelStoneMin", M2Share.Config.nSteelStoneMin);
            M2Share.Config.nSteelStoneMin = ReadInteger("Setup", "SteelStoneMin", M2Share.Config.nSteelStoneMin);
            if (ReadInteger("Setup", "SteelStoneMax", -1) < 0)
                WriteInteger("Setup", "SteelStoneMax", M2Share.Config.nSteelStoneMax);
            M2Share.Config.nSteelStoneMax = ReadInteger("Setup", "SteelStoneMax", M2Share.Config.nSteelStoneMax);
            if (ReadInteger("Setup", "BlackStoneMin", -1) < 0)
                WriteInteger("Setup", "BlackStoneMin", M2Share.Config.nBlackStoneMin);
            M2Share.Config.nBlackStoneMin = ReadInteger("Setup", "BlackStoneMin", M2Share.Config.nBlackStoneMin);
            if (ReadInteger("Setup", "BlackStoneMax", -1) < 0)
                WriteInteger("Setup", "BlackStoneMax", M2Share.Config.nBlackStoneMax);
            M2Share.Config.nBlackStoneMax = ReadInteger("Setup", "BlackStoneMax", M2Share.Config.nBlackStoneMax);
            if (ReadInteger("Setup", "StoneMinDura", -1) < 0)
                WriteInteger("Setup", "StoneMinDura", M2Share.Config.nStoneMinDura);
            M2Share.Config.nStoneMinDura = ReadInteger("Setup", "StoneMinDura", M2Share.Config.nStoneMinDura);
            if (ReadInteger("Setup", "StoneGeneralDuraRate", -1) < 0)
                WriteInteger("Setup", "StoneGeneralDuraRate", M2Share.Config.nStoneGeneralDuraRate);
            M2Share.Config.nStoneGeneralDuraRate = ReadInteger("Setup", "StoneGeneralDuraRate", M2Share.Config.nStoneGeneralDuraRate);
            if (ReadInteger("Setup", "StoneAddDuraRate", -1) < 0)
                WriteInteger("Setup", "StoneAddDuraRate", M2Share.Config.nStoneAddDuraRate);
            M2Share.Config.nStoneAddDuraRate = ReadInteger("Setup", "StoneAddDuraRate", M2Share.Config.nStoneAddDuraRate);
            if (ReadInteger("Setup", "StoneAddDuraMax", -1) < 0)
                WriteInteger("Setup", "StoneAddDuraMax", M2Share.Config.nStoneAddDuraMax);
            M2Share.Config.nStoneAddDuraMax = ReadInteger("Setup", "StoneAddDuraMax", M2Share.Config.nStoneAddDuraMax);
            if (ReadInteger("Setup", "WinLottery1Min", -1) < 0)
                WriteInteger("Setup", "WinLottery1Min", M2Share.Config.nWinLottery1Min);
            M2Share.Config.nWinLottery1Min = ReadInteger("Setup", "WinLottery1Min", M2Share.Config.nWinLottery1Min);
            if (ReadInteger("Setup", "WinLottery1Max", -1) < 0)
                WriteInteger("Setup", "WinLottery1Max", M2Share.Config.nWinLottery1Max);
            M2Share.Config.nWinLottery1Max = ReadInteger("Setup", "WinLottery1Max", M2Share.Config.nWinLottery1Max);
            if (ReadInteger("Setup", "WinLottery2Min", -1) < 0)
                WriteInteger("Setup", "WinLottery2Min", M2Share.Config.nWinLottery2Min);
            M2Share.Config.nWinLottery2Min = ReadInteger("Setup", "WinLottery2Min", M2Share.Config.nWinLottery2Min);
            if (ReadInteger("Setup", "WinLottery2Max", -1) < 0)
                WriteInteger("Setup", "WinLottery2Max", M2Share.Config.nWinLottery2Max);
            M2Share.Config.nWinLottery2Max = ReadInteger("Setup", "WinLottery2Max", M2Share.Config.nWinLottery2Max);
            if (ReadInteger("Setup", "WinLottery3Min", -1) < 0)
                WriteInteger("Setup", "WinLottery3Min", M2Share.Config.nWinLottery3Min);
            M2Share.Config.nWinLottery3Min = ReadInteger("Setup", "WinLottery3Min", M2Share.Config.nWinLottery3Min);
            if (ReadInteger("Setup", "WinLottery3Max", -1) < 0)
                WriteInteger("Setup", "WinLottery3Max", M2Share.Config.nWinLottery3Max);
            M2Share.Config.nWinLottery3Max = ReadInteger("Setup", "WinLottery3Max", M2Share.Config.nWinLottery3Max);
            if (ReadInteger("Setup", "WinLottery4Min", -1) < 0)
                WriteInteger("Setup", "WinLottery4Min", M2Share.Config.nWinLottery4Min);
            M2Share.Config.nWinLottery4Min = ReadInteger("Setup", "WinLottery4Min", M2Share.Config.nWinLottery4Min);
            if (ReadInteger("Setup", "WinLottery4Max", -1) < 0)
                WriteInteger("Setup", "WinLottery4Max", M2Share.Config.nWinLottery4Max);
            M2Share.Config.nWinLottery4Max = ReadInteger("Setup", "WinLottery4Max", M2Share.Config.nWinLottery4Max);
            if (ReadInteger("Setup", "WinLottery5Min", -1) < 0)
                WriteInteger("Setup", "WinLottery5Min", M2Share.Config.nWinLottery5Min);
            M2Share.Config.nWinLottery5Min = ReadInteger("Setup", "WinLottery5Min", M2Share.Config.nWinLottery5Min);
            if (ReadInteger("Setup", "WinLottery5Max", -1) < 0)
                WriteInteger("Setup", "WinLottery5Max", M2Share.Config.nWinLottery5Max);
            M2Share.Config.nWinLottery5Max = ReadInteger("Setup", "WinLottery5Max", M2Share.Config.nWinLottery5Max);
            if (ReadInteger("Setup", "WinLottery6Min", -1) < 0)
                WriteInteger("Setup", "WinLottery6Min", M2Share.Config.nWinLottery6Min);
            M2Share.Config.nWinLottery6Min = ReadInteger("Setup", "WinLottery6Min", M2Share.Config.nWinLottery6Min);
            if (ReadInteger("Setup", "WinLottery6Max", -1) < 0)
                WriteInteger("Setup", "WinLottery6Max", M2Share.Config.nWinLottery6Max);
            M2Share.Config.nWinLottery6Max = ReadInteger("Setup", "WinLottery6Max", M2Share.Config.nWinLottery6Max);
            if (ReadInteger("Setup", "WinLotteryRate", -1) < 0)
                WriteInteger("Setup", "WinLotteryRate", M2Share.Config.nWinLotteryRate);
            M2Share.Config.nWinLotteryRate = ReadInteger("Setup", "WinLotteryRate", M2Share.Config.nWinLotteryRate);
            if (ReadInteger("Setup", "WinLottery1Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery1Gold", M2Share.Config.nWinLottery1Gold);
            M2Share.Config.nWinLottery1Gold = ReadInteger("Setup", "WinLottery1Gold", M2Share.Config.nWinLottery1Gold);
            if (ReadInteger("Setup", "WinLottery2Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery2Gold", M2Share.Config.nWinLottery2Gold);
            M2Share.Config.nWinLottery2Gold = ReadInteger("Setup", "WinLottery2Gold", M2Share.Config.nWinLottery2Gold);
            if (ReadInteger("Setup", "WinLottery3Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery3Gold", M2Share.Config.nWinLottery3Gold);
            M2Share.Config.nWinLottery3Gold = ReadInteger("Setup", "WinLottery3Gold", M2Share.Config.nWinLottery3Gold);
            if (ReadInteger("Setup", "WinLottery4Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery4Gold", M2Share.Config.nWinLottery4Gold);
            M2Share.Config.nWinLottery4Gold = ReadInteger("Setup", "WinLottery4Gold", M2Share.Config.nWinLottery4Gold);
            if (ReadInteger("Setup", "WinLottery5Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery5Gold", M2Share.Config.nWinLottery5Gold);
            M2Share.Config.nWinLottery5Gold = ReadInteger("Setup", "WinLottery5Gold", M2Share.Config.nWinLottery5Gold);
            if (ReadInteger("Setup", "WinLottery6Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery6Gold", M2Share.Config.nWinLottery6Gold);
            M2Share.Config.nWinLottery6Gold = ReadInteger("Setup", "WinLottery6Gold", M2Share.Config.nWinLottery6Gold);
            if (ReadInteger("Setup", "GuildRecallTime", -1) < 0)
                WriteInteger("Setup", "GuildRecallTime", M2Share.Config.nGuildRecallTime);
            M2Share.Config.nGuildRecallTime = ReadInteger("Setup", "GuildRecallTime", M2Share.Config.nGuildRecallTime);
            if (ReadInteger("Setup", "GroupRecallTime", -1) < 0)
                WriteInteger("Setup", "GroupRecallTime", M2Share.Config.nGroupRecallTime);
            M2Share.Config.nGroupRecallTime = ReadInteger("Setup", "GroupRecallTime", M2Share.Config.nGroupRecallTime);
            if (ReadInteger("Setup", "ControlDropItem", -1) < 0)
                WriteBool("Setup", "ControlDropItem", M2Share.Config.boControlDropItem);
            M2Share.Config.boControlDropItem = ReadBool("Setup", "ControlDropItem", M2Share.Config.boControlDropItem);
            if (ReadInteger("Setup", "InSafeDisableDrop", -1) < 0)
                WriteBool("Setup", "InSafeDisableDrop", M2Share.Config.boInSafeDisableDrop);
            M2Share.Config.boInSafeDisableDrop = ReadBool("Setup", "InSafeDisableDrop", M2Share.Config.boInSafeDisableDrop);
            if (ReadInteger("Setup", "CanDropGold", -1) < 0)
                WriteInteger("Setup", "CanDropGold", M2Share.Config.nCanDropGold);
            M2Share.Config.nCanDropGold = ReadInteger("Setup", "CanDropGold", M2Share.Config.nCanDropGold);
            if (ReadInteger("Setup", "CanDropPrice", -1) < 0)
                WriteInteger("Setup", "CanDropPrice", M2Share.Config.nCanDropPrice);
            M2Share.Config.nCanDropPrice = ReadInteger("Setup", "CanDropPrice", M2Share.Config.nCanDropPrice);
            nLoadInteger = ReadInteger("Setup", "SendCustemMsg", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "SendCustemMsg", M2Share.Config.boSendCustemMsg);
            else
                M2Share.Config.boSendCustemMsg = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "SubkMasterSendMsg", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "SubkMasterSendMsg", M2Share.Config.boSubkMasterSendMsg);
            else
                M2Share.Config.boSubkMasterSendMsg = nLoadInteger == 1;
            if (ReadInteger("Setup", "SuperRepairPriceRate", -1) < 0)
                WriteInteger("Setup", "SuperRepairPriceRate", M2Share.Config.nSuperRepairPriceRate);
            M2Share.Config.nSuperRepairPriceRate = ReadInteger("Setup", "SuperRepairPriceRate", M2Share.Config.nSuperRepairPriceRate);
            if (ReadInteger("Setup", "RepairItemDecDura", -1) < 0)
                WriteInteger("Setup", "RepairItemDecDura", M2Share.Config.nRepairItemDecDura);
            M2Share.Config.nRepairItemDecDura = ReadInteger("Setup", "RepairItemDecDura", M2Share.Config.nRepairItemDecDura);
            if (ReadInteger("Setup", "DieScatterBag", -1) < 0)
                WriteBool("Setup", "DieScatterBag", M2Share.Config.boDieScatterBag);
            M2Share.Config.boDieScatterBag = ReadBool("Setup", "DieScatterBag", M2Share.Config.boDieScatterBag);
            if (ReadInteger("Setup", "DieScatterBagRate", -1) < 0)
                WriteInteger("Setup", "DieScatterBagRate", M2Share.Config.nDieScatterBagRate);
            M2Share.Config.nDieScatterBagRate = ReadInteger("Setup", "DieScatterBagRate", M2Share.Config.nDieScatterBagRate);
            if (ReadInteger("Setup", "DieRedScatterBagAll", -1) < 0)
                WriteBool("Setup", "DieRedScatterBagAll", M2Share.Config.boDieRedScatterBagAll);
            M2Share.Config.boDieRedScatterBagAll = ReadBool("Setup", "DieRedScatterBagAll", M2Share.Config.boDieRedScatterBagAll);
            if (ReadInteger("Setup", "DieDropUseItemRate", -1) < 0)
                WriteInteger("Setup", "DieDropUseItemRate", M2Share.Config.nDieDropUseItemRate);
            M2Share.Config.nDieDropUseItemRate = ReadInteger("Setup", "DieDropUseItemRate", M2Share.Config.nDieDropUseItemRate);
            if (ReadInteger("Setup", "DieRedDropUseItemRate", -1) < 0)
                WriteInteger("Setup", "DieRedDropUseItemRate", M2Share.Config.nDieRedDropUseItemRate);
            M2Share.Config.nDieRedDropUseItemRate = ReadInteger("Setup", "DieRedDropUseItemRate", M2Share.Config.nDieRedDropUseItemRate);
            if (ReadInteger("Setup", "DieDropGold", -1) < 0)
                WriteBool("Setup", "DieDropGold", M2Share.Config.boDieDropGold);
            M2Share.Config.boDieDropGold = ReadBool("Setup", "DieDropGold", M2Share.Config.boDieDropGold);
            if (ReadInteger("Setup", "KillByHumanDropUseItem", -1) < 0)
                WriteBool("Setup", "KillByHumanDropUseItem", M2Share.Config.boKillByHumanDropUseItem);
            M2Share.Config.boKillByHumanDropUseItem = ReadBool("Setup", "KillByHumanDropUseItem", M2Share.Config.boKillByHumanDropUseItem);
            if (ReadInteger("Setup", "KillByMonstDropUseItem", -1) < 0)
                WriteBool("Setup", "KillByMonstDropUseItem", M2Share.Config.boKillByMonstDropUseItem);
            M2Share.Config.boKillByMonstDropUseItem = ReadBool("Setup", "KillByMonstDropUseItem", M2Share.Config.boKillByMonstDropUseItem);
            if (ReadInteger("Setup", "KickExpireHuman", -1) < 0)
                WriteBool("Setup", "KickExpireHuman", M2Share.Config.boKickExpireHuman);
            M2Share.Config.boKickExpireHuman = ReadBool("Setup", "KickExpireHuman", M2Share.Config.boKickExpireHuman);
            if (ReadInteger("Setup", "GuildRankNameLen", -1) < 0)
                WriteInteger("Setup", "GuildRankNameLen", M2Share.Config.nGuildRankNameLen);
            M2Share.Config.nGuildRankNameLen = ReadInteger("Setup", "GuildRankNameLen", M2Share.Config.nGuildRankNameLen);
            if (ReadInteger("Setup", "GuildNameLen", -1) < 0)
                WriteInteger("Setup", "GuildNameLen", M2Share.Config.nGuildNameLen);
            M2Share.Config.nGuildNameLen = ReadInteger("Setup", "GuildNameLen", M2Share.Config.nGuildNameLen);
            if (ReadInteger("Setup", "GuildMemberMaxLimit", -1) < 0)
                WriteInteger("Setup", "GuildMemberMaxLimit", M2Share.Config.nGuildMemberMaxLimit);
            M2Share.Config.nGuildMemberMaxLimit = ReadInteger("Setup", "GuildMemberMaxLimit", M2Share.Config.nGuildMemberMaxLimit);
            if (ReadInteger("Setup", "AttackPosionRate", -1) < 0)
                WriteInteger("Setup", "AttackPosionRate", M2Share.Config.AttackPosionRate);
            M2Share.Config.AttackPosionRate = ReadInteger("Setup", "AttackPosionRate", M2Share.Config.AttackPosionRate);
            if (ReadInteger("Setup", "AttackPosionTime", -1) < 0)
                WriteInteger("Setup", "AttackPosionTime", M2Share.Config.AttackPosionTime);
            M2Share.Config.AttackPosionTime = (ushort)ReadInteger("Setup", "AttackPosionTime", M2Share.Config.AttackPosionTime);
            if (ReadInteger("Setup", "RevivalTime", -1) < 0)
                WriteInteger("Setup", "RevivalTime", M2Share.Config.dwRevivalTime);
            M2Share.Config.dwRevivalTime = ReadInteger("Setup", "RevivalTime", M2Share.Config.dwRevivalTime);
            nLoadInteger = ReadInteger("Setup", "UserMoveCanDupObj", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "UserMoveCanDupObj", M2Share.Config.boUserMoveCanDupObj);
            else
                M2Share.Config.boUserMoveCanDupObj = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "UserMoveCanOnItem", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "UserMoveCanOnItem", M2Share.Config.boUserMoveCanOnItem);
            else
                M2Share.Config.boUserMoveCanOnItem = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "UserMoveTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UserMoveTime", M2Share.Config.dwUserMoveTime);
            else
                M2Share.Config.dwUserMoveTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "PKDieLostExpRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "PKDieLostExpRate", M2Share.Config.dwPKDieLostExpRate);
            else
                M2Share.Config.dwPKDieLostExpRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "PKDieLostLevelRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "PKDieLostLevelRate", M2Share.Config.nPKDieLostLevelRate);
            else
                M2Share.Config.nPKDieLostLevelRate = nLoadInteger;
            if (ReadInteger("Setup", "PKFlagNameColor", -1) < 0)
                WriteInteger("Setup", "PKFlagNameColor", M2Share.Config.btPKFlagNameColor);
            M2Share.Config.btPKFlagNameColor = Read<byte>("Setup", "PKFlagNameColor", M2Share.Config.btPKFlagNameColor);
            if (ReadInteger("Setup", "AllyAndGuildNameColor", -1) < 0)
                WriteInteger("Setup", "AllyAndGuildNameColor", M2Share.Config.btAllyAndGuildNameColor);
            M2Share.Config.btAllyAndGuildNameColor = Read<byte>("Setup", "AllyAndGuildNameColor", M2Share.Config.btAllyAndGuildNameColor);
            if (ReadInteger("Setup", "WarGuildNameColor", -1) < 0)
                WriteInteger("Setup", "WarGuildNameColor", M2Share.Config.btWarGuildNameColor);
            M2Share.Config.btWarGuildNameColor = Read<byte>("Setup", "WarGuildNameColor", M2Share.Config.btWarGuildNameColor);
            if (ReadInteger("Setup", "InFreePKAreaNameColor", -1) < 0)
                WriteInteger("Setup", "InFreePKAreaNameColor", M2Share.Config.btInFreePKAreaNameColor);
            M2Share.Config.btInFreePKAreaNameColor = Read<byte>("Setup", "InFreePKAreaNameColor", M2Share.Config.btInFreePKAreaNameColor);
            if (ReadInteger("Setup", "PKLevel1NameColor", -1) < 0)
                WriteInteger("Setup", "PKLevel1NameColor", M2Share.Config.btPKLevel1NameColor);
            M2Share.Config.btPKLevel1NameColor = Read<byte>("Setup", "PKLevel1NameColor", M2Share.Config.btPKLevel1NameColor);
            if (ReadInteger("Setup", "PKLevel2NameColor", -1) < 0)
                WriteInteger("Setup", "PKLevel2NameColor", M2Share.Config.btPKLevel2NameColor);
            M2Share.Config.btPKLevel2NameColor = Read<byte>("Setup", "PKLevel2NameColor", M2Share.Config.btPKLevel2NameColor);
            if (ReadInteger("Setup", "SpiritMutiny", -1) < 0)
                WriteBool("Setup", "SpiritMutiny", M2Share.Config.boSpiritMutiny);
            M2Share.Config.boSpiritMutiny = ReadBool("Setup", "SpiritMutiny", M2Share.Config.boSpiritMutiny);
            if (ReadInteger("Setup", "SpiritMutinyTime", -1) < 0)
                WriteInteger("Setup", "SpiritMutinyTime", M2Share.Config.dwSpiritMutinyTime);
            M2Share.Config.dwSpiritMutinyTime = ReadInteger("Setup", "SpiritMutinyTime", M2Share.Config.dwSpiritMutinyTime);
            if (ReadInteger("Setup", "SpiritPowerRate", -1) < 0)
                WriteInteger("Setup", "SpiritPowerRate", M2Share.Config.nSpiritPowerRate);
            M2Share.Config.nSpiritPowerRate = ReadInteger("Setup", "SpiritPowerRate", M2Share.Config.nSpiritPowerRate);
            if (ReadInteger("Setup", "MasterDieMutiny", -1) < 0)
                WriteBool("Setup", "MasterDieMutiny", M2Share.Config.boMasterDieMutiny);
            M2Share.Config.boMasterDieMutiny = ReadBool("Setup", "MasterDieMutiny", M2Share.Config.boMasterDieMutiny);
            if (ReadInteger("Setup", "MasterDieMutinyRate", -1) < 0)
                WriteInteger("Setup", "MasterDieMutinyRate", M2Share.Config.nMasterDieMutinyRate);
            M2Share.Config.nMasterDieMutinyRate = ReadInteger("Setup", "MasterDieMutinyRate", M2Share.Config.nMasterDieMutinyRate);
            if (ReadInteger("Setup", "MasterDieMutinyPower", -1) < 0)
                WriteInteger("Setup", "MasterDieMutinyPower", M2Share.Config.nMasterDieMutinyPower);
            M2Share.Config.nMasterDieMutinyPower = ReadInteger("Setup", "MasterDieMutinyPower", M2Share.Config.nMasterDieMutinyPower);
            if (ReadInteger("Setup", "MasterDieMutinyPower", -1) < 0)
                WriteInteger("Setup", "MasterDieMutinyPower", M2Share.Config.nMasterDieMutinySpeed);
            M2Share.Config.nMasterDieMutinySpeed = ReadInteger("Setup", "MasterDieMutinyPower", M2Share.Config.nMasterDieMutinySpeed);
            nLoadInteger = ReadInteger("Setup", "BBMonAutoChangeColor", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "BBMonAutoChangeColor", M2Share.Config.boBBMonAutoChangeColor);
            else
                M2Share.Config.boBBMonAutoChangeColor = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "BBMonAutoChangeColorTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "BBMonAutoChangeColorTime", M2Share.Config.dwBBMonAutoChangeColorTime);
            else
                M2Share.Config.dwBBMonAutoChangeColorTime = nLoadInteger;
            if (ReadInteger("Setup", "OldClientShowHiLevel", -1) < 0)
                WriteBool("Setup", "OldClientShowHiLevel", M2Share.Config.boOldClientShowHiLevel);
            M2Share.Config.boOldClientShowHiLevel = ReadBool("Setup", "OldClientShowHiLevel", M2Share.Config.boOldClientShowHiLevel);
            if (ReadInteger("Setup", "ShowScriptActionMsg", -1) < 0)
                WriteBool("Setup", "ShowScriptActionMsg", M2Share.Config.boShowScriptActionMsg);
            M2Share.Config.boShowScriptActionMsg = ReadBool("Setup", "ShowScriptActionMsg", M2Share.Config.boShowScriptActionMsg);
            if (ReadInteger("Setup", "RunSocketDieLoopLimit", -1) < 0)
                WriteInteger("Setup", "RunSocketDieLoopLimit", M2Share.Config.nRunSocketDieLoopLimit);
            M2Share.Config.nRunSocketDieLoopLimit = ReadInteger("Setup", "RunSocketDieLoopLimit", M2Share.Config.nRunSocketDieLoopLimit);
            if (ReadInteger("Setup", "ThreadRun", -1) < 0)
                WriteBool("Setup", "ThreadRun", M2Share.Config.boThreadRun);
            M2Share.Config.boThreadRun = ReadBool("Setup", "ThreadRun", M2Share.Config.boThreadRun);
            if (ReadInteger("Setup", "DeathColorEffect", -1) < 0)
                WriteInteger("Setup", "DeathColorEffect", M2Share.Config.ClientConf.btDieColor);
            M2Share.Config.ClientConf.btDieColor = Read<byte>("Setup", "DeathColorEffect", M2Share.Config.ClientConf.btDieColor);
            if (ReadInteger("Setup", "ParalyCanRun", -1) < 0)
                WriteBool("Setup", "ParalyCanRun", M2Share.Config.ClientConf.boParalyCanRun);
            M2Share.Config.ClientConf.boParalyCanRun = ReadBool("Setup", "ParalyCanRun", M2Share.Config.ClientConf.boParalyCanRun);
            if (ReadInteger("Setup", "ParalyCanWalk", -1) < 0)
                WriteBool("Setup", "ParalyCanWalk", M2Share.Config.ClientConf.boParalyCanWalk);
            M2Share.Config.ClientConf.boParalyCanWalk = ReadBool("Setup", "ParalyCanWalk", M2Share.Config.ClientConf.boParalyCanWalk);
            if (ReadInteger("Setup", "ParalyCanHit", -1) < 0)
                WriteBool("Setup", "ParalyCanHit", M2Share.Config.ClientConf.boParalyCanHit);
            M2Share.Config.ClientConf.boParalyCanHit = ReadBool("Setup", "ParalyCanHit", M2Share.Config.ClientConf.boParalyCanHit);
            if (ReadInteger("Setup", "ParalyCanSpell", -1) < 0)
                WriteBool("Setup", "ParalyCanSpell", M2Share.Config.ClientConf.boParalyCanSpell);
            M2Share.Config.ClientConf.boParalyCanSpell = ReadBool("Setup", "ParalyCanSpell", M2Share.Config.ClientConf.boParalyCanSpell);
            if (ReadInteger("Setup", "ShowExceptionMsg", -1) < 0)
                WriteBool("Setup", "ShowExceptionMsg", M2Share.Config.boShowExceptionMsg);
            M2Share.Config.boShowExceptionMsg = ReadBool("Setup", "ShowExceptionMsg", M2Share.Config.boShowExceptionMsg);
            if (ReadInteger("Setup", "ShowPreFixMsg", -1) < 0)
                WriteBool("Setup", "ShowPreFixMsg", M2Share.Config.boShowPreFixMsg);
            M2Share.Config.boShowPreFixMsg = ReadBool("Setup", "ShowPreFixMsg", M2Share.Config.boShowPreFixMsg);
            if (ReadInteger("Setup", "MagTurnUndeadLevel", -1) < 0)
                WriteInteger("Setup", "MagTurnUndeadLevel", M2Share.Config.nMagTurnUndeadLevel);
            M2Share.Config.nMagTurnUndeadLevel = ReadInteger("Setup", "MagTurnUndeadLevel", M2Share.Config.nMagTurnUndeadLevel);
            nLoadInteger = ReadInteger("Setup", "MagTammingLevel", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingLevel", M2Share.Config.nMagTammingLevel);
            else
                M2Share.Config.nMagTammingLevel = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MagTammingTargetLevel", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingTargetLevel", M2Share.Config.nMagTammingTargetLevel);
            else
                M2Share.Config.nMagTammingTargetLevel = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MagTammingTargetHPRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingTargetHPRate", M2Share.Config.nMagTammingHPRate);
            else
                M2Share.Config.nMagTammingHPRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MagTammingCount", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingCount", M2Share.Config.nMagTammingCount);
            else
                M2Share.Config.nMagTammingCount = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MabMabeHitRandRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitRandRate", M2Share.Config.nMabMabeHitRandRate);
            else
                M2Share.Config.nMabMabeHitRandRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MabMabeHitMinLvLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitMinLvLimit", M2Share.Config.nMabMabeHitMinLvLimit);
            else
                M2Share.Config.nMabMabeHitMinLvLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MabMabeHitSucessRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitSucessRate", M2Share.Config.nMabMabeHitSucessRate);
            else
                M2Share.Config.nMabMabeHitSucessRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MabMabeHitMabeTimeRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitMabeTimeRate", M2Share.Config.nMabMabeHitMabeTimeRate);
            else
                M2Share.Config.nMabMabeHitMabeTimeRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MagicAttackRage", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagicAttackRage", M2Share.Config.nMagicAttackRage);
            else
                M2Share.Config.nMagicAttackRage = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "DropItemRage", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "DropItemRage", M2Share.Config.nDropItemRage);
            else
                M2Share.Config.nDropItemRage = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "AmyOunsulPoint", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "AmyOunsulPoint", M2Share.Config.nAmyOunsulPoint);
            else
                M2Share.Config.nAmyOunsulPoint = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "DisableInSafeZoneFireCross", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "DisableInSafeZoneFireCross", M2Share.Config.boDisableInSafeZoneFireCross);
            else
                M2Share.Config.boDisableInSafeZoneFireCross = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "GroupMbAttackPlayObject", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "GroupMbAttackPlayObject", M2Share.Config.boGroupMbAttackPlayObject);
            else
                M2Share.Config.boGroupMbAttackPlayObject = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "PosionDecHealthTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "PosionDecHealthTime", M2Share.Config.dwPosionDecHealthTime);
            else
                M2Share.Config.dwPosionDecHealthTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "PosionDamagarmor", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "PosionDamagarmor", M2Share.Config.nPosionDamagarmor);
            else
                M2Share.Config.nPosionDamagarmor = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "LimitSwordLong", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "LimitSwordLong", M2Share.Config.boLimitSwordLong);
            else
                M2Share.Config.boLimitSwordLong = !(nLoadInteger == 0);
            nLoadInteger = ReadInteger("Setup", "SwordLongPowerRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "SwordLongPowerRate", M2Share.Config.nSwordLongPowerRate);
            else
                M2Share.Config.nSwordLongPowerRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "FireBoomRage", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "FireBoomRage", M2Share.Config.nFireBoomRage);
            else
                M2Share.Config.nFireBoomRage = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "SnowWindRange", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "SnowWindRange", M2Share.Config.nSnowWindRange);
            else
                M2Share.Config.nSnowWindRange = nLoadInteger;
            if (ReadInteger("Setup", "ElecBlizzardRange", -1) < 0)
                WriteInteger("Setup", "ElecBlizzardRange", M2Share.Config.nElecBlizzardRange);
            M2Share.Config.nElecBlizzardRange = ReadInteger("Setup", "ElecBlizzardRange", M2Share.Config.nElecBlizzardRange);
            if (ReadInteger("Setup", "HumanLevelDiffer", -1) < 0)
                WriteInteger("Setup", "HumanLevelDiffer", M2Share.Config.nHumanLevelDiffer);
            M2Share.Config.nHumanLevelDiffer = ReadInteger("Setup", "HumanLevelDiffer", M2Share.Config.nHumanLevelDiffer);
            if (ReadInteger("Setup", "KillHumanWinLevel", -1) < 0)
                WriteBool("Setup", "KillHumanWinLevel", M2Share.Config.boKillHumanWinLevel);
            M2Share.Config.boKillHumanWinLevel = ReadBool("Setup", "KillHumanWinLevel", M2Share.Config.boKillHumanWinLevel);
            if (ReadInteger("Setup", "KilledLostLevel", -1) < 0)
                WriteBool("Setup", "KilledLostLevel", M2Share.Config.boKilledLostLevel);
            M2Share.Config.boKilledLostLevel = ReadBool("Setup", "KilledLostLevel", M2Share.Config.boKilledLostLevel);
            if (ReadInteger("Setup", "KillHumanWinLevelPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanWinLevelPoint", M2Share.Config.nKillHumanWinLevel);
            M2Share.Config.nKillHumanWinLevel = ReadInteger("Setup", "KillHumanWinLevelPoint", M2Share.Config.nKillHumanWinLevel);
            if (ReadInteger("Setup", "KilledLostLevelPoint", -1) < 0)
                WriteInteger("Setup", "KilledLostLevelPoint", M2Share.Config.nKilledLostLevel);
            M2Share.Config.nKilledLostLevel = ReadInteger("Setup", "KilledLostLevelPoint", M2Share.Config.nKilledLostLevel);
            if (ReadInteger("Setup", "KillHumanWinExp", -1) < 0)
                WriteBool("Setup", "KillHumanWinExp", M2Share.Config.boKillHumanWinExp);
            M2Share.Config.boKillHumanWinExp = ReadBool("Setup", "KillHumanWinExp", M2Share.Config.boKillHumanWinExp);
            if (ReadInteger("Setup", "KilledLostExp", -1) < 0)
                WriteBool("Setup", "KilledLostExp", M2Share.Config.boKilledLostExp);
            M2Share.Config.boKilledLostExp = ReadBool("Setup", "KilledLostExp", M2Share.Config.boKilledLostExp);
            if (ReadInteger("Setup", "KillHumanWinExpPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanWinExpPoint", M2Share.Config.nKillHumanWinExp);
            M2Share.Config.nKillHumanWinExp = ReadInteger("Setup", "KillHumanWinExpPoint", M2Share.Config.nKillHumanWinExp);
            if (ReadInteger("Setup", "KillHumanLostExpPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanLostExpPoint", M2Share.Config.nKillHumanLostExp);
            M2Share.Config.nKillHumanLostExp = ReadInteger("Setup", "KillHumanLostExpPoint", M2Share.Config.nKillHumanLostExp);
            if (ReadInteger("Setup", "MonsterPowerRate", -1) < 0)
                WriteInteger("Setup", "MonsterPowerRate", M2Share.Config.nMonsterPowerRate);
            M2Share.Config.nMonsterPowerRate = ReadInteger("Setup", "MonsterPowerRate", M2Share.Config.nMonsterPowerRate);
            if (ReadInteger("Setup", "ItemsPowerRate", -1) < 0)
                WriteInteger("Setup", "ItemsPowerRate", M2Share.Config.nItemsPowerRate);
            M2Share.Config.nItemsPowerRate = ReadInteger("Setup", "ItemsPowerRate", M2Share.Config.nItemsPowerRate);
            if (ReadInteger("Setup", "ItemsACPowerRate", -1) < 0)
                WriteInteger("Setup", "ItemsACPowerRate", M2Share.Config.nItemsACPowerRate);
            M2Share.Config.nItemsACPowerRate = ReadInteger("Setup", "ItemsACPowerRate", M2Share.Config.nItemsACPowerRate);
            if (ReadInteger("Setup", "SendOnlineCount", -1) < 0)
                WriteBool("Setup", "SendOnlineCount", M2Share.Config.boSendOnlineCount);
            M2Share.Config.boSendOnlineCount = ReadBool("Setup", "SendOnlineCount", M2Share.Config.boSendOnlineCount);
            if (ReadInteger("Setup", "SendOnlineCountRate", -1) < 0)
                WriteInteger("Setup", "SendOnlineCountRate", M2Share.Config.nSendOnlineCountRate);
            M2Share.Config.nSendOnlineCountRate = ReadInteger("Setup", "SendOnlineCountRate", M2Share.Config.nSendOnlineCountRate);
            if (ReadInteger("Setup", "SendOnlineTime", -1) < 0)
                WriteInteger("Setup", "SendOnlineTime", M2Share.Config.dwSendOnlineTime);
            M2Share.Config.dwSendOnlineTime = ReadInteger("Setup", "SendOnlineTime", M2Share.Config.dwSendOnlineTime);
            if (ReadInteger("Setup", "SaveHumanRcdTime", -1) < 0)
                WriteInteger("Setup", "SaveHumanRcdTime", M2Share.Config.dwSaveHumanRcdTime);
            M2Share.Config.dwSaveHumanRcdTime = ReadInteger("Setup", "SaveHumanRcdTime", M2Share.Config.dwSaveHumanRcdTime);
            if (ReadInteger("Setup", "HumanFreeDelayTime", -1) < 0)
                WriteInteger("Setup", "HumanFreeDelayTime", M2Share.Config.dwHumanFreeDelayTime);
            if (ReadInteger("Setup", "MakeGhostTime", -1) < 0)
                WriteInteger("Setup", "MakeGhostTime", M2Share.Config.dwMakeGhostTime);
            M2Share.Config.dwMakeGhostTime = ReadInteger("Setup", "MakeGhostTime", M2Share.Config.dwMakeGhostTime);
            if (ReadInteger("Setup", "ClearDropOnFloorItemTime", -1) < 0)
                WriteInteger("Setup", "ClearDropOnFloorItemTime", M2Share.Config.dwClearDropOnFloorItemTime);
            M2Share.Config.dwClearDropOnFloorItemTime = ReadInteger("Setup", "ClearDropOnFloorItemTime", M2Share.Config.dwClearDropOnFloorItemTime);
            if (ReadInteger("Setup", "FloorItemCanPickUpTime", -1) < 0)
                WriteInteger("Setup", "FloorItemCanPickUpTime", M2Share.Config.dwFloorItemCanPickUpTime);
            M2Share.Config.dwFloorItemCanPickUpTime = ReadInteger("Setup", "FloorItemCanPickUpTime", M2Share.Config.dwFloorItemCanPickUpTime);
            if (ReadInteger("Setup", "PasswordLockSystem", -1) < 0)
                WriteBool("Setup", "PasswordLockSystem", M2Share.Config.boPasswordLockSystem);
            M2Share.Config.boPasswordLockSystem = ReadBool("Setup", "PasswordLockSystem", M2Share.Config.boPasswordLockSystem);
            if (ReadInteger("Setup", "PasswordLockDealAction", -1) < 0)
                WriteBool("Setup", "PasswordLockDealAction", M2Share.Config.boLockDealAction);
            M2Share.Config.boLockDealAction = ReadBool("Setup", "PasswordLockDealAction", M2Share.Config.boLockDealAction);
            if (ReadInteger("Setup", "PasswordLockDropAction", -1) < 0)
                WriteBool("Setup", "PasswordLockDropAction", M2Share.Config.boLockDropAction);
            M2Share.Config.boLockDropAction = ReadBool("Setup", "PasswordLockDropAction", M2Share.Config.boLockDropAction);
            if (ReadInteger("Setup", "PasswordLockGetBackItemAction", -1) < 0)
                WriteBool("Setup", "PasswordLockGetBackItemAction", M2Share.Config.boLockGetBackItemAction);
            M2Share.Config.boLockGetBackItemAction = ReadBool("Setup", "PasswordLockGetBackItemAction", M2Share.Config.boLockGetBackItemAction);
            if (ReadInteger("Setup", "PasswordLockHumanLogin", -1) < 0)
                WriteBool("Setup", "PasswordLockHumanLogin", M2Share.Config.boLockHumanLogin);
            M2Share.Config.boLockHumanLogin = ReadBool("Setup", "PasswordLockHumanLogin", M2Share.Config.boLockHumanLogin);
            if (ReadInteger("Setup", "PasswordLockWalkAction", -1) < 0)
                WriteBool("Setup", "PasswordLockWalkAction", M2Share.Config.boLockWalkAction);
            M2Share.Config.boLockWalkAction = ReadBool("Setup", "PasswordLockWalkAction", M2Share.Config.boLockWalkAction);
            if (ReadInteger("Setup", "PasswordLockRunAction", -1) < 0)
                WriteBool("Setup", "PasswordLockRunAction", M2Share.Config.boLockRunAction);
            M2Share.Config.boLockRunAction = ReadBool("Setup", "PasswordLockRunAction", M2Share.Config.boLockRunAction);
            if (ReadInteger("Setup", "PasswordLockHitAction", -1) < 0)
                WriteBool("Setup", "PasswordLockHitAction", M2Share.Config.boLockHitAction);
            M2Share.Config.boLockHitAction = ReadBool("Setup", "PasswordLockHitAction", M2Share.Config.boLockHitAction);
            if (ReadInteger("Setup", "PasswordLockSpellAction", -1) < 0)
                WriteBool("Setup", "PasswordLockSpellAction", M2Share.Config.boLockSpellAction);
            M2Share.Config.boLockSpellAction = ReadBool("Setup", "PasswordLockSpellAction", M2Share.Config.boLockSpellAction);
            if (ReadInteger("Setup", "PasswordLockSendMsgAction", -1) < 0)
                WriteBool("Setup", "PasswordLockSendMsgAction", M2Share.Config.boLockSendMsgAction);
            M2Share.Config.boLockSendMsgAction = ReadBool("Setup", "PasswordLockSendMsgAction", M2Share.Config.boLockSendMsgAction);
            if (ReadInteger("Setup", "PasswordLockUserItemAction", -1) < 0)
                WriteBool("Setup", "PasswordLockUserItemAction", M2Share.Config.boLockUserItemAction);
            M2Share.Config.boLockUserItemAction = ReadBool("Setup", "PasswordLockUserItemAction", M2Share.Config.boLockUserItemAction);
            if (ReadInteger("Setup", "PasswordLockInObModeAction", -1) < 0)
                WriteBool("Setup", "PasswordLockInObModeAction", M2Share.Config.boLockInObModeAction);
            M2Share.Config.boLockInObModeAction = ReadBool("Setup", "PasswordLockInObModeAction", M2Share.Config.boLockInObModeAction);
            if (ReadInteger("Setup", "PasswordErrorKick", -1) < 0)
                WriteBool("Setup", "PasswordErrorKick", M2Share.Config.boPasswordErrorKick);
            M2Share.Config.boPasswordErrorKick = ReadBool("Setup", "PasswordErrorKick", M2Share.Config.boPasswordErrorKick);
            if (ReadInteger("Setup", "PasswordErrorCountLock", -1) < 0)
                WriteInteger("Setup", "PasswordErrorCountLock", M2Share.Config.nPasswordErrorCountLock);
            M2Share.Config.nPasswordErrorCountLock = ReadInteger("Setup", "PasswordErrorCountLock", M2Share.Config.nPasswordErrorCountLock);
            if (ReadInteger("Setup", "SoftVersionDate", -1) < 0)
                WriteInteger("Setup", "SoftVersionDate", M2Share.Config.nSoftVersionDate);
            M2Share.Config.nSoftVersionDate = ReadInteger("Setup", "SoftVersionDate", M2Share.Config.nSoftVersionDate);
            nLoadInteger = ReadInteger("Setup", "CanOldClientLogon", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "CanOldClientLogon", M2Share.Config.boCanOldClientLogon);
            else
                M2Share.Config.boCanOldClientLogon = nLoadInteger == 1;
            if (ReadInteger("Setup", "ConsoleShowUserCountTime", -1) < 0)
                WriteInteger("Setup", "ConsoleShowUserCountTime", M2Share.Config.dwConsoleShowUserCountTime);
            M2Share.Config.dwConsoleShowUserCountTime = ReadInteger("Setup", "ConsoleShowUserCountTime", M2Share.Config.dwConsoleShowUserCountTime);
            if (ReadInteger("Setup", "ShowLineNoticeTime", -1) < 0)
                WriteInteger("Setup", "ShowLineNoticeTime", M2Share.Config.dwShowLineNoticeTime);
            M2Share.Config.dwShowLineNoticeTime = ReadInteger("Setup", "ShowLineNoticeTime", M2Share.Config.dwShowLineNoticeTime);
            if (ReadInteger("Setup", "LineNoticeColor", -1) < 0)
                WriteInteger("Setup", "LineNoticeColor", M2Share.Config.nLineNoticeColor);
            M2Share.Config.nLineNoticeColor = ReadInteger("Setup", "LineNoticeColor", M2Share.Config.nLineNoticeColor);
            if (ReadInteger("Setup", "ItemSpeedTime", -1) < 0)
                WriteInteger("Setup", "ItemSpeedTime", M2Share.Config.ClientConf.btItemSpeed);
            M2Share.Config.ClientConf.btItemSpeed = Read<byte>("Setup", "ItemSpeedTime", M2Share.Config.ClientConf.btItemSpeed);
            if (ReadInteger("Setup", "MaxHitMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxHitMsgCount", M2Share.Config.nMaxHitMsgCount);
            M2Share.Config.nMaxHitMsgCount = ReadInteger("Setup", "MaxHitMsgCount", M2Share.Config.nMaxHitMsgCount);
            if (ReadInteger("Setup", "MaxSpellMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxSpellMsgCount", M2Share.Config.nMaxSpellMsgCount);
            M2Share.Config.nMaxSpellMsgCount = ReadInteger("Setup", "MaxSpellMsgCount", M2Share.Config.nMaxSpellMsgCount);
            if (ReadInteger("Setup", "MaxRunMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxRunMsgCount", M2Share.Config.nMaxRunMsgCount);
            M2Share.Config.nMaxRunMsgCount = ReadInteger("Setup", "MaxRunMsgCount", M2Share.Config.nMaxRunMsgCount);
            if (ReadInteger("Setup", "MaxWalkMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxWalkMsgCount", M2Share.Config.nMaxWalkMsgCount);
            M2Share.Config.nMaxWalkMsgCount = ReadInteger("Setup", "MaxWalkMsgCount", M2Share.Config.nMaxWalkMsgCount);
            if (ReadInteger("Setup", "MaxTurnMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxTurnMsgCount", M2Share.Config.nMaxTurnMsgCount);
            M2Share.Config.nMaxTurnMsgCount = ReadInteger("Setup", "MaxTurnMsgCount", M2Share.Config.nMaxTurnMsgCount);
            if (ReadInteger("Setup", "MaxSitDonwMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxSitDonwMsgCount", M2Share.Config.nMaxSitDonwMsgCount);
            M2Share.Config.nMaxSitDonwMsgCount = ReadInteger("Setup", "MaxSitDonwMsgCount", M2Share.Config.nMaxSitDonwMsgCount);
            if (ReadInteger("Setup", "MaxDigUpMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxDigUpMsgCount", M2Share.Config.nMaxDigUpMsgCount);
            M2Share.Config.nMaxDigUpMsgCount = ReadInteger("Setup", "MaxDigUpMsgCount", M2Share.Config.nMaxDigUpMsgCount);
            if (ReadInteger("Setup", "SpellSendUpdateMsg", -1) < 0)
                WriteBool("Setup", "SpellSendUpdateMsg", M2Share.Config.boSpellSendUpdateMsg);
            M2Share.Config.boSpellSendUpdateMsg = ReadBool("Setup", "SpellSendUpdateMsg", M2Share.Config.boSpellSendUpdateMsg);
            if (ReadInteger("Setup", "ActionSendActionMsg", -1) < 0)
                WriteBool("Setup", "ActionSendActionMsg", M2Share.Config.boActionSendActionMsg);
            M2Share.Config.boActionSendActionMsg = ReadBool("Setup", "ActionSendActionMsg", M2Share.Config.boActionSendActionMsg);
            if (ReadInteger("Setup", "OverSpeedKickCount", -1) < 0)
                WriteInteger("Setup", "OverSpeedKickCount", M2Share.Config.nOverSpeedKickCount);
            M2Share.Config.nOverSpeedKickCount = ReadInteger("Setup", "OverSpeedKickCount", M2Share.Config.nOverSpeedKickCount);
            if (ReadInteger("Setup", "DropOverSpeed", -1) < 0)
                WriteInteger("Setup", "DropOverSpeed", M2Share.Config.dwDropOverSpeed);
            M2Share.Config.dwDropOverSpeed = ReadInteger("Setup", "DropOverSpeed", M2Share.Config.dwDropOverSpeed);
            if (ReadInteger("Setup", "KickOverSpeed", -1) < 0)
                WriteBool("Setup", "KickOverSpeed", M2Share.Config.boKickOverSpeed);
            M2Share.Config.boKickOverSpeed = ReadBool("Setup", "KickOverSpeed", M2Share.Config.boKickOverSpeed);
            nLoadInteger = ReadInteger("Setup", "SpeedControlMode", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "SpeedControlMode", M2Share.Config.btSpeedControlMode);
            else
                M2Share.Config.btSpeedControlMode = nLoadInteger;
            if (ReadInteger("Setup", "HitIntervalTime", -1) < 0)
                WriteInteger("Setup", "HitIntervalTime", M2Share.Config.dwHitIntervalTime);
            M2Share.Config.dwHitIntervalTime =
                ReadInteger("Setup", "HitIntervalTime", M2Share.Config.dwHitIntervalTime);
            if (ReadInteger("Setup", "MagicHitIntervalTime", -1) < 0)
                WriteInteger("Setup", "MagicHitIntervalTime", M2Share.Config.dwMagicHitIntervalTime);
            M2Share.Config.dwMagicHitIntervalTime = ReadInteger("Setup", "MagicHitIntervalTime", M2Share.Config.dwMagicHitIntervalTime);
            if (ReadInteger("Setup", "RunIntervalTime", -1) < 0)
                WriteInteger("Setup", "RunIntervalTime", M2Share.Config.dwRunIntervalTime);
            M2Share.Config.dwRunIntervalTime = ReadInteger("Setup", "RunIntervalTime", M2Share.Config.dwRunIntervalTime);
            if (ReadInteger("Setup", "WalkIntervalTime", -1) < 0)
                WriteInteger("Setup", "WalkIntervalTime", M2Share.Config.dwWalkIntervalTime);
            M2Share.Config.dwWalkIntervalTime = ReadInteger("Setup", "WalkIntervalTime", M2Share.Config.dwWalkIntervalTime);
            if (ReadInteger("Setup", "TurnIntervalTime", -1) < 0)
                WriteInteger("Setup", "TurnIntervalTime", M2Share.Config.dwTurnIntervalTime);
            M2Share.Config.dwTurnIntervalTime = ReadInteger("Setup", "TurnIntervalTime", M2Share.Config.dwTurnIntervalTime);
            nLoadInteger = ReadInteger("Setup", "ControlActionInterval", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "ControlActionInterval", M2Share.Config.boControlActionInterval);
            else
                M2Share.Config.boControlActionInterval = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "ControlWalkHit", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "ControlWalkHit", M2Share.Config.boControlWalkHit);
            else
                M2Share.Config.boControlWalkHit = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "ControlRunLongHit", -1);
            if (nLoadInteger < 0)
            {
                WriteBool("Setup", "ControlRunLongHit", M2Share.Config.boControlRunLongHit);
            }
            else
            {
                M2Share.Config.boControlRunLongHit = nLoadInteger == 1;
            }
            nLoadInteger = ReadInteger("Setup", "ControlRunHit", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "ControlRunHit", M2Share.Config.boControlRunHit);
            else
                M2Share.Config.boControlRunHit = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "ControlRunMagic", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "ControlRunMagic", M2Share.Config.boControlRunMagic);
            else
                M2Share.Config.boControlRunMagic = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "ActionIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "ActionIntervalTime", M2Share.Config.dwActionIntervalTime);
            else
                M2Share.Config.dwActionIntervalTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "RunLongHitIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "RunLongHitIntervalTime", M2Share.Config.dwRunLongHitIntervalTime);
            else
                M2Share.Config.dwRunLongHitIntervalTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "RunHitIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "RunHitIntervalTime", M2Share.Config.dwRunHitIntervalTime);
            else
                M2Share.Config.dwRunHitIntervalTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "WalkHitIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "WalkHitIntervalTime", M2Share.Config.dwWalkHitIntervalTime);
            else
                M2Share.Config.dwWalkHitIntervalTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "RunMagicIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "RunMagicIntervalTime", M2Share.Config.dwRunMagicIntervalTime);
            else
                M2Share.Config.dwRunMagicIntervalTime = nLoadInteger;
            if (ReadInteger("Setup", "DisableStruck", -1) < 0)
                WriteBool("Setup", "DisableStruck", M2Share.Config.boDisableStruck);
            M2Share.Config.boDisableStruck =
                ReadBool("Setup", "DisableStruck", M2Share.Config.boDisableStruck);
            if (ReadInteger("Setup", "DisableSelfStruck", -1) < 0)
                WriteBool("Setup", "DisableSelfStruck", M2Share.Config.boDisableSelfStruck);
            M2Share.Config.boDisableSelfStruck =
                ReadBool("Setup", "DisableSelfStruck", M2Share.Config.boDisableSelfStruck);
            if (ReadInteger("Setup", "StruckTime", -1) < 0)
                WriteInteger("Setup", "StruckTime", M2Share.Config.dwStruckTime);
            M2Share.Config.dwStruckTime = ReadInteger("Setup", "StruckTime", M2Share.Config.dwStruckTime);
            nLoadInteger = ReadInteger("Setup", "AddUserItemNewValue", -1);
            if (nLoadInteger < 0)
            {
                WriteBool("Setup", "AddUserItemNewValue", M2Share.Config.boAddUserItemNewValue);
            }
            else
            {
                M2Share.Config.boAddUserItemNewValue = nLoadInteger == 1;
            }
            nLoadInteger = ReadInteger("Setup", "TestSpeedMode", -1);
            if (nLoadInteger < 0)
            {
                WriteBool("Setup", "TestSpeedMode", M2Share.Config.boTestSpeedMode);
            }
            else
            {
                M2Share.Config.boTestSpeedMode = nLoadInteger == 1;
            }
            // 气血石开始
            if (ReadInteger("Setup", "HPStoneStartRate", -1) < 0)
            {
                WriteInteger("Setup", "HPStoneStartRate", M2Share.Config.HPStoneStartRate);
            }
            M2Share.Config.HPStoneStartRate = Read<byte>("Setup", "HPStoneStartRate", M2Share.Config.HPStoneStartRate);
            if (ReadInteger("Setup", "MPStoneStartRate", -1) < 0)
            {
                WriteInteger("Setup", "MPStoneStartRate", M2Share.Config.MPStoneStartRate);
            }
            M2Share.Config.MPStoneStartRate = Read<byte>("Setup", "MPStoneStartRate", M2Share.Config.MPStoneStartRate);
            if (ReadInteger("Setup", "HPStoneIntervalTime", -1) < 0)
            {
                WriteInteger("Setup", "HPStoneIntervalTime", M2Share.Config.HPStoneIntervalTime);
            }
            M2Share.Config.HPStoneIntervalTime = ReadInteger("Setup", "HPStoneIntervalTime", M2Share.Config.HPStoneIntervalTime);
            if (ReadInteger("Setup", "MPStoneIntervalTime", -1) < 0)
            {
                WriteInteger("Setup", "MPStoneIntervalTime", M2Share.Config.MPStoneIntervalTime);
            }
            M2Share.Config.MPStoneIntervalTime = ReadInteger("Setup", "MPStoneIntervalTime", M2Share.Config.MPStoneIntervalTime);
            if (ReadInteger("Setup", "HPStoneAddRate", -1) < 0)
            {
                WriteInteger("Setup", "HPStoneAddRate", M2Share.Config.HPStoneAddRate);
            }
            M2Share.Config.HPStoneAddRate = Read<byte>("Setup", "HPStoneAddRate", M2Share.Config.HPStoneAddRate);
            if (ReadInteger("Setup", "MPStoneAddRate", -1) < 0)
            {
                WriteInteger("Setup", "MPStoneAddRate", M2Share.Config.MPStoneAddRate);
            }
            M2Share.Config.MPStoneAddRate = Read<byte>("Setup", "MPStoneAddRate", M2Share.Config.MPStoneAddRate);
            if (ReadInteger("Setup", "HPStoneDecDura", -1) < 0)
            {
                WriteInteger("Setup", "HPStoneDecDura", M2Share.Config.HPStoneDecDura);
            }
            M2Share.Config.HPStoneDecDura = ReadInteger("Setup", "HPStoneDecDura", M2Share.Config.HPStoneDecDura);
            if (ReadInteger("Setup", "MPStoneDecDura", -1) < 0)
            {
                WriteInteger("Setup", "MPStoneDecDura", M2Share.Config.MPStoneDecDura);
            }
            M2Share.Config.MPStoneDecDura = ReadInteger("Setup", "MPStoneDecDura", M2Share.Config.MPStoneDecDura);

            // 气血石结束
            nLoadInteger = ReadInteger("Setup", "WeaponMakeUnLuckRate", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeUnLuckRate", M2Share.Config.nWeaponMakeUnLuckRate);
            }
            else
            {
                M2Share.Config.nWeaponMakeUnLuckRate = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WeaponMakeLuckPoint1", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint1", M2Share.Config.nWeaponMakeLuckPoint1);
            }
            else
            {
                M2Share.Config.nWeaponMakeLuckPoint1 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WeaponMakeLuckPoint2", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint2", M2Share.Config.nWeaponMakeLuckPoint2);
            }
            else
            {
                M2Share.Config.nWeaponMakeLuckPoint2 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WeaponMakeLuckPoint3", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint3", M2Share.Config.nWeaponMakeLuckPoint3);
            }
            else
            {
                M2Share.Config.nWeaponMakeLuckPoint3 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WeaponMakeLuckPoint2Rate", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint2Rate", M2Share.Config.nWeaponMakeLuckPoint2Rate);
            }
            else
            {
                M2Share.Config.nWeaponMakeLuckPoint2Rate = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WeaponMakeLuckPoint3Rate", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint3Rate", M2Share.Config.nWeaponMakeLuckPoint3Rate);
            }
            else
            {
                M2Share.Config.nWeaponMakeLuckPoint3Rate = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "CheckUserItemPlace", -1);
            if (nLoadInteger < 0)
            {
                WriteBool("Setup", "CheckUserItemPlace", M2Share.Config.boCheckUserItemPlace);
            }
            else
            {
                M2Share.Config.boCheckUserItemPlace = nLoadInteger == 1;
            }
            nLoadInteger = ReadInteger("Setup", "LevelValueOfTaosHP", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "LevelValueOfTaosHP", M2Share.Config.nLevelValueOfTaosHP);
            }
            else
            {
                M2Share.Config.nLevelValueOfTaosHP = nLoadInteger;
            }
            var nLoadFloatRate = Read<double>("Setup", "LevelValueOfTaosHPRate", 0);
            if (nLoadFloatRate == 0)
            {
                WriteInteger("Setup", "LevelValueOfTaosHPRate", M2Share.Config.nLevelValueOfTaosHPRate);
            }
            else
            {
                M2Share.Config.nLevelValueOfTaosHPRate = nLoadFloatRate;
            }
            nLoadInteger = ReadInteger("Setup", "LevelValueOfTaosMP", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "LevelValueOfTaosMP", M2Share.Config.nLevelValueOfTaosMP);
            }
            else
            {
                M2Share.Config.nLevelValueOfTaosMP = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "LevelValueOfWizardHP", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "LevelValueOfWizardHP", M2Share.Config.nLevelValueOfWizardHP);
            }
            else
            {
                M2Share.Config.nLevelValueOfWizardHP = nLoadInteger;
            }
            nLoadFloatRate = Read<double>("Setup", "LevelValueOfWizardHPRate", 0);
            if (nLoadFloatRate == 0)
            {
                WriteInteger("Setup", "LevelValueOfWizardHPRate", M2Share.Config.nLevelValueOfWizardHPRate);
            }
            else
            {
                M2Share.Config.nLevelValueOfWizardHPRate = nLoadFloatRate;
            }
            nLoadInteger = ReadInteger("Setup", "LevelValueOfWarrHP", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "LevelValueOfWarrHP", M2Share.Config.nLevelValueOfWarrHP);
            }
            else
            {
                M2Share.Config.nLevelValueOfWarrHP = nLoadInteger;
            }
            nLoadFloatRate = Read<double>("Setup", "LevelValueOfWarrHPRate", 0);
            if (nLoadFloatRate == 0)
            {
                WriteInteger("Setup", "LevelValueOfWarrHPRate", M2Share.Config.nLevelValueOfWarrHPRate);
            }
            else
            {
                M2Share.Config.nLevelValueOfWarrHPRate = nLoadFloatRate;
            }
            nLoadInteger = ReadInteger("Setup", "ProcessMonsterInterval", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "ProcessMonsterInterval", M2Share.Config.nProcessMonsterInterval);
            }
            else
            {
                M2Share.Config.nProcessMonsterInterval = nLoadInteger;
            }
            if (ReadInteger("Setup", "StartCastleWarDays", -1) < 0)
            {
                WriteInteger("Setup", "StartCastleWarDays", M2Share.Config.nStartCastleWarDays);
            }
            M2Share.Config.nStartCastleWarDays = ReadInteger("Setup", "StartCastleWarDays", M2Share.Config.nStartCastleWarDays);
            if (ReadInteger("Setup", "StartCastlewarTime", -1) < 0)
            {
                WriteInteger("Setup", "StartCastlewarTime", M2Share.Config.nStartCastlewarTime);
            }
            M2Share.Config.nStartCastlewarTime = ReadInteger("Setup", "StartCastlewarTime", M2Share.Config.nStartCastlewarTime);
            if (ReadInteger("Setup", "ShowCastleWarEndMsgTime", -1) < 0)
            {
                WriteInteger("Setup", "ShowCastleWarEndMsgTime", M2Share.Config.dwShowCastleWarEndMsgTime);
            }
            M2Share.Config.dwShowCastleWarEndMsgTime = ReadInteger("Setup", "ShowCastleWarEndMsgTime", M2Share.Config.dwShowCastleWarEndMsgTime);
            if (ReadInteger("Server", "ClickNPCTime", -1) < 0)
            {
                WriteInteger("Server", "ClickNPCTime", M2Share.Config.dwClickNpcTime);
            }
            M2Share.Config.dwClickNpcTime = ReadInteger("Server", "ClickNPCTime", M2Share.Config.dwClickNpcTime);
            if (ReadInteger("Setup", "CastleWarTime", -1) < 0)
            {
                WriteInteger("Setup", "CastleWarTime", M2Share.Config.dwCastleWarTime);
            }
            M2Share.Config.dwCastleWarTime = ReadInteger("Setup", "CastleWarTime", M2Share.Config.dwCastleWarTime);
            nLoadInteger = ReadInteger("Setup", "GetCastleTime", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "GetCastleTime", M2Share.Config.dwGetCastleTime);
            }
            else
            {
                M2Share.Config.dwGetCastleTime = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "GuildWarTime", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "GuildWarTime", M2Share.Config.dwGuildWarTime);
            }
            else
            {
                M2Share.Config.dwGuildWarTime = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryCount", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryCount", M2Share.Config.nWinLotteryCount);
            }
            else
            {
                M2Share.Config.nWinLotteryCount = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "NoWinLotteryCount", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "NoWinLotteryCount", M2Share.Config.nNoWinLotteryCount);
            }
            else
            {
                M2Share.Config.nNoWinLotteryCount = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel1", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel1", M2Share.Config.nWinLotteryLevel1);
            }
            else
            {
                M2Share.Config.nWinLotteryLevel1 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel2", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel2", M2Share.Config.nWinLotteryLevel2);
            }
            else
            {
                M2Share.Config.nWinLotteryLevel2 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel3", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel3", M2Share.Config.nWinLotteryLevel3);
            }
            else
            {
                M2Share.Config.nWinLotteryLevel3 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel4", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel4", M2Share.Config.nWinLotteryLevel4);
            }
            else
            {
                M2Share.Config.nWinLotteryLevel4 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel5", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel5", M2Share.Config.nWinLotteryLevel5);
            }
            else
            {
                M2Share.Config.nWinLotteryLevel5 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel6", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel6", M2Share.Config.nWinLotteryLevel6);
            }
            else
            {
                M2Share.Config.nWinLotteryLevel6 = nLoadInteger;
            }
        }

        /// <summary>
        /// 保存游戏变量和彩票中奖数据
        /// </summary>
        public void SaveVariable()
        {
            WriteInteger("Setup", "ItemNumber", M2Share.Config.nItemNumber);
            WriteInteger("Setup", "ItemNumberEx", M2Share.Config.nItemNumberEx);
            WriteInteger("Setup", "WinLotteryCount", M2Share.Config.nWinLotteryCount);
            WriteInteger("Setup", "NoWinLotteryCount", M2Share.Config.nNoWinLotteryCount);
            WriteInteger("Setup", "WinLotteryLevel1", M2Share.Config.nWinLotteryLevel1);
            WriteInteger("Setup", "WinLotteryLevel2", M2Share.Config.nWinLotteryLevel2);
            WriteInteger("Setup", "WinLotteryLevel3", M2Share.Config.nWinLotteryLevel3);
            WriteInteger("Setup", "WinLotteryLevel4", M2Share.Config.nWinLotteryLevel4);
            WriteInteger("Setup", "WinLotteryLevel5", M2Share.Config.nWinLotteryLevel5);
            WriteInteger("Setup", "WinLotteryLevel6", M2Share.Config.nWinLotteryLevel6);
        }

    }
}