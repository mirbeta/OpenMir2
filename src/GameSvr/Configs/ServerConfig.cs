using System.IO;
using SystemModule;
using SystemModule.Common;

namespace GameSvr.Configs
{
    public class ServerConfig
    {
        private readonly IniFile _config;

        public ServerConfig()
        {
            _config = new IniFile(Path.Combine(M2Share.sConfigPath, M2Share.sConfigFileName));
            _config.Load();
        }

        public void LoadConfig()
        {
            var nLoadInteger = 0;
            var sLoadString = string.Empty;
            //数据库设置
            if (_config.ReadString("DataBase", "ConnctionString", "") == "")
            {
                _config.WriteString("DataBase", "ConnctionString", M2Share.g_Config.sConnctionString);
            }
            M2Share.g_Config.sConnctionString = _config.ReadString("DataBase", "ConnctionString", M2Share.g_Config.sConnctionString);
            //数据库类型设置
            if (_config.ReadString("DataBase", "DbType", "") == "")
            {
                _config.WriteString("DataBase", "DbType", M2Share.g_Config.sDBType);
            }
            M2Share.g_Config.sDBType = _config.ReadString("DataBase", "DbType", M2Share.g_Config.sDBType);
            // 服务器设置
            if (_config.ReadInteger("Server", "ServerIndex", -1) < 0)
            {
                _config.WriteInteger("Server", "ServerIndex", M2Share.nServerIndex);
            }
            M2Share.nServerIndex = _config.ReadInteger("Server", "ServerIndex", M2Share.nServerIndex);
            if (_config.ReadString("Server", "ServerName", "") == "")
            {
                _config.WriteString("Server", "ServerName", M2Share.g_Config.sServerName);
            }
            M2Share.g_Config.sServerName = _config.ReadString("Server", "ServerName", M2Share.g_Config.sServerName);
            if (_config.ReadInteger("Server", "ServerNumber", -1) < 0)
                _config.WriteInteger("Server", "ServerNumber", M2Share.g_Config.nServerNumber);
            M2Share.g_Config.nServerNumber = _config.ReadInteger("Server", "ServerNumber", M2Share.g_Config.nServerNumber);
            if (_config.ReadString("Server", "VentureServer", "") == "")
                _config.WriteString("Server", "VentureServer", HUtil32.BoolToStr(M2Share.g_Config.boVentureServer));
            M2Share.g_Config.boVentureServer = _config.ReadString("Server", "VentureServer", "FALSE").ToLower().CompareTo("TRUE".ToLower()) == 0;
            if (_config.ReadString("Server", "TestServer", "") == "")
                _config.WriteString("Server", "TestServer", HUtil32.BoolToStr(M2Share.g_Config.boTestServer));
            M2Share.g_Config.boTestServer = _config.ReadString("Server", "TestServer", "FALSE").ToLower().CompareTo("TRUE".ToLower()) == 0;
            if (_config.ReadInteger("Server", "TestLevel", -1) < 0)
                _config.WriteInteger("Server", "TestLevel", M2Share.g_Config.nTestLevel);
            M2Share.g_Config.nTestLevel = _config.ReadInteger("Server", "TestLevel", M2Share.g_Config.nTestLevel);
            if (_config.ReadInteger("Server", "TestGold", -1) < 0)
                _config.WriteInteger("Server", "TestGold", M2Share.g_Config.nTestGold);
            M2Share.g_Config.nTestGold = _config.ReadInteger("Server", "TestGold", M2Share.g_Config.nTestGold);
            if (_config.ReadInteger("Server", "TestServerUserLimit", -1) < 0)
                _config.WriteInteger("Server", "TestServerUserLimit", M2Share.g_Config.nTestUserLimit);
            M2Share.g_Config.nTestUserLimit = _config.ReadInteger("Server", "TestServerUserLimit", M2Share.g_Config.nTestUserLimit);
            if (_config.ReadString("Server", "ServiceMode", "") == "")
                _config.WriteString("Server", "ServiceMode", HUtil32.BoolToStr(M2Share.g_Config.boServiceMode));
            M2Share.g_Config.boServiceMode = _config.ReadString("Server", "ServiceMode", "FALSE").ToLower().CompareTo("TRUE".ToLower()) == 0;
            if (_config.ReadString("Server", "NonPKServer", "") == "")
                _config.WriteString("Server", "NonPKServer", HUtil32.BoolToStr(M2Share.g_Config.boNonPKServer));
            M2Share.g_Config.boNonPKServer = _config.ReadString("Server", "NonPKServer", "FALSE").ToLower().CompareTo("TRUE".ToLower()) == 0;
            if (_config.ReadString("Server", "ViewHackMessage", "") == "")
                _config.WriteString("Server", "ViewHackMessage", HUtil32.BoolToStr(M2Share.g_Config.boViewHackMessage));
            M2Share.g_Config.boViewHackMessage = _config.ReadString("Server", "ViewHackMessage", "FALSE").ToLower().CompareTo("TRUE".ToLower()) == 0;
            if (_config.ReadString("Server", "ViewAdmissionFailure", "") == "")
            {
                _config.WriteString("Server", "ViewAdmissionFailure", HUtil32.BoolToStr(M2Share.g_Config.boViewAdmissionFailure));
            }
            M2Share.g_Config.boViewAdmissionFailure = _config.ReadString("Server", "ViewAdmissionFailure", "FALSE").ToLower().CompareTo("TRUE".ToLower()) == 0;
            if (_config.ReadString("Server", "GateAddr", "") == "")
            {
                _config.WriteString("Server", "GateAddr", M2Share.g_Config.sGateAddr);
            }
            M2Share.g_Config.sGateAddr = _config.ReadString("Server", "GateAddr", M2Share.g_Config.sGateAddr);
            if (_config.ReadInteger("Server", "GatePort", -1) < 0)
            {
                _config.WriteInteger("Server", "GatePort", M2Share.g_Config.nGatePort);
            }
            M2Share.g_Config.nGatePort = _config.ReadInteger("Server", "GatePort", M2Share.g_Config.nGatePort);
            if (_config.ReadString("Server", "DBAddr", "") == "")
                _config.WriteString("Server", "DBAddr", M2Share.g_Config.sDBAddr);
            M2Share.g_Config.sDBAddr = _config.ReadString("Server", "DBAddr", M2Share.g_Config.sDBAddr);
            if (_config.ReadInteger("Server", "DBPort", -1) < 0)
                _config.WriteInteger("Server", "DBPort", M2Share.g_Config.nDBPort);
            M2Share.g_Config.nDBPort = _config.ReadInteger("Server", "DBPort", M2Share.g_Config.nDBPort);
            if (_config.ReadString("Server", "IDSAddr", "") == "")
                _config.WriteString("Server", "IDSAddr", M2Share.g_Config.sIDSAddr);
            M2Share.g_Config.sIDSAddr = _config.ReadString("Server", "IDSAddr", M2Share.g_Config.sIDSAddr);
            if (_config.ReadInteger("Server", "IDSPort", -1) < 0)
                _config.WriteInteger("Server", "IDSPort", M2Share.g_Config.nIDSPort);
            M2Share.g_Config.nIDSPort = _config.ReadInteger("Server", "IDSPort", M2Share.g_Config.nIDSPort);
            if (_config.ReadString("Server", "MsgSrvAddr", "") == "")
                _config.WriteString("Server", "MsgSrvAddr", M2Share.g_Config.sMsgSrvAddr);
            M2Share.g_Config.sMsgSrvAddr = _config.ReadString("Server", "MsgSrvAddr", M2Share.g_Config.sMsgSrvAddr);
            if (_config.ReadInteger("Server", "MsgSrvPort", -1) < 0)
                _config.WriteInteger("Server", "MsgSrvPort", M2Share.g_Config.nMsgSrvPort);
            M2Share.g_Config.nMsgSrvPort = _config.ReadInteger("Server", "MsgSrvPort", M2Share.g_Config.nMsgSrvPort);
            if (_config.ReadString("Server", "LogServerAddr", "") == "")
                _config.WriteString("Server", "LogServerAddr", M2Share.g_Config.sLogServerAddr);
            M2Share.g_Config.sLogServerAddr = _config.ReadString("Server", "LogServerAddr", M2Share.g_Config.sLogServerAddr);
            if (_config.ReadInteger("Server", "LogServerPort", -1) < 0)
                _config.WriteInteger("Server", "LogServerPort", M2Share.g_Config.nLogServerPort);
            M2Share.g_Config.nLogServerPort = _config.ReadInteger("Server", "LogServerPort", M2Share.g_Config.nLogServerPort);
            if (_config.ReadString("Server", "DiscountForNightTime", "") == "")
                _config.WriteString("Server", "DiscountForNightTime", HUtil32.BoolToStr(M2Share.g_Config.boDiscountForNightTime));
            M2Share.g_Config.boDiscountForNightTime = _config.ReadString("Server", "DiscountForNightTime", "FALSE").ToLower().CompareTo("TRUE".ToLower()) == 0;
            if (_config.ReadInteger("Server", "HalfFeeStart", -1) < 0)
                _config.WriteInteger("Server", "HalfFeeStart", M2Share.g_Config.nHalfFeeStart);
            M2Share.g_Config.nHalfFeeStart = _config.ReadInteger("Server", "HalfFeeStart", M2Share.g_Config.nHalfFeeStart);
            if (_config.ReadInteger("Server", "HalfFeeEnd", -1) < 0)
                _config.WriteInteger("Server", "HalfFeeEnd", M2Share.g_Config.nHalfFeeEnd);
            M2Share.g_Config.nHalfFeeEnd = _config.ReadInteger("Server", "HalfFeeEnd", M2Share.g_Config.nHalfFeeEnd);
            if (_config.ReadInteger("Server", "HumLimit", -1) < 0)
                _config.WriteInteger("Server", "HumLimit", M2Share.g_dwHumLimit);
            M2Share.g_dwHumLimit = _config.ReadInteger("Server", "HumLimit", M2Share.g_dwHumLimit);
            if (_config.ReadInteger("Server", "MonLimit", -1) < 0)
                _config.WriteInteger("Server", "MonLimit", M2Share.g_dwMonLimit);
            M2Share.g_dwMonLimit = _config.ReadInteger("Server", "MonLimit", M2Share.g_dwMonLimit);
            if (_config.ReadInteger("Server", "ZenLimit", -1) < 0)
                _config.WriteInteger("Server", "ZenLimit", M2Share.g_dwZenLimit);
            M2Share.g_dwZenLimit = _config.ReadInteger("Server", "ZenLimit", M2Share.g_dwZenLimit);
            if (_config.ReadInteger("Server", "NpcLimit", -1) < 0)
                _config.WriteInteger("Server", "NpcLimit", M2Share.g_dwNpcLimit);
            M2Share.g_dwNpcLimit = _config.ReadInteger("Server", "NpcLimit", M2Share.g_dwNpcLimit);
            if (_config.ReadInteger("Server", "SocLimit", -1) < 0)
                _config.WriteInteger("Server", "SocLimit", M2Share.g_dwSocLimit);
            M2Share.g_dwSocLimit = _config.ReadInteger("Server", "SocLimit", M2Share.g_dwSocLimit);
            if (_config.ReadInteger("Server", "DecLimit", -1) < 0)
                _config.WriteInteger("Server", "DecLimit", M2Share.nDecLimit);
            M2Share.nDecLimit = _config.ReadInteger("Server", "DecLimit", M2Share.nDecLimit);
            if (_config.ReadInteger("Server", "SendBlock", -1) < 0)
                _config.WriteInteger("Server", "SendBlock", M2Share.g_Config.nSendBlock);
            M2Share.g_Config.nSendBlock = _config.ReadInteger("Server", "SendBlock", M2Share.g_Config.nSendBlock);
            if (_config.ReadInteger("Server", "CheckBlock", -1) < 0)
                _config.WriteInteger("Server", "CheckBlock", M2Share.g_Config.nCheckBlock);
            M2Share.g_Config.nCheckBlock = _config.ReadInteger("Server", "CheckBlock", M2Share.g_Config.nCheckBlock);
            if (_config.ReadInteger("Server", "SocCheckTimeOut", -1) < 0)
                _config.WriteInteger("Server", "SocCheckTimeOut", M2Share.g_dwSocCheckTimeOut);
            M2Share.g_dwSocCheckTimeOut = _config.ReadInteger("Server", "SocCheckTimeOut", M2Share.g_dwSocCheckTimeOut);
            if (_config.ReadInteger("Server", "AvailableBlock", -1) < 0)
                _config.WriteInteger("Server", "AvailableBlock", M2Share.g_Config.nAvailableBlock);
            M2Share.g_Config.nAvailableBlock = _config.ReadInteger("Server", "AvailableBlock", M2Share.g_Config.nAvailableBlock);
            if (_config.ReadInteger("Server", "GateLoad", -1) < 0)
                _config.WriteInteger("Server", "GateLoad", M2Share.g_Config.nGateLoad);
            M2Share.g_Config.nGateLoad = _config.ReadInteger("Server", "GateLoad", M2Share.g_Config.nGateLoad);
            if (_config.ReadInteger("Server", "UserFull", -1) < 0)
                _config.WriteInteger("Server", "UserFull", M2Share.g_Config.nUserFull);
            M2Share.g_Config.nUserFull = _config.ReadInteger("Server", "UserFull", M2Share.g_Config.nUserFull);
            if (_config.ReadInteger("Server", "ZenFastStep", -1) < 0)
                _config.WriteInteger("Server", "ZenFastStep", M2Share.g_Config.nZenFastStep);
            M2Share.g_Config.nZenFastStep = _config.ReadInteger("Server", "ZenFastStep", M2Share.g_Config.nZenFastStep);
            if (_config.ReadInteger("Server", "ProcessMonstersTime", -1) < 0)
                _config.WriteInteger("Server", "ProcessMonstersTime", M2Share.g_Config.dwProcessMonstersTime);
            M2Share.g_Config.dwProcessMonstersTime = _config.ReadInteger("Server", "ProcessMonstersTime", M2Share.g_Config.dwProcessMonstersTime);
            if (_config.ReadInteger("Server", "RegenMonstersTime", -1) < 0)
                _config.WriteInteger("Server", "RegenMonstersTime", M2Share.g_Config.dwRegenMonstersTime);
            M2Share.g_Config.dwRegenMonstersTime = _config.ReadInteger("Server", "RegenMonstersTime", M2Share.g_Config.dwRegenMonstersTime);
            if (_config.ReadInteger("Server", "HumanGetMsgTimeLimit", -1) < 0)
                _config.WriteInteger("Server", "HumanGetMsgTimeLimit", M2Share.g_Config.dwHumanGetMsgTime);
            M2Share.g_Config.dwHumanGetMsgTime = _config.ReadInteger("Server", "HumanGetMsgTimeLimit", M2Share.g_Config.dwHumanGetMsgTime);
            if (_config.ReadString("Share", "BaseDir", "") == "")
                _config.WriteString("Share", "BaseDir", M2Share.g_Config.sBaseDir);
            M2Share.g_Config.sBaseDir = _config.ReadString("Share", "BaseDir", M2Share.g_Config.sBaseDir);
            if (_config.ReadString("Share", "GuildDir", "") == "")
                _config.WriteString("Share", "GuildDir", M2Share.g_Config.sGuildDir);
            M2Share.g_Config.sGuildDir = _config.ReadString("Share", "GuildDir", M2Share.g_Config.sGuildDir);
            if (_config.ReadString("Share", "GuildFile", "") == "")
                _config.WriteString("Share", "GuildFile", M2Share.g_Config.sGuildFile);
            M2Share.g_Config.sGuildFile = _config.ReadString("Share", "GuildFile", M2Share.g_Config.sGuildFile);
            M2Share.g_Config.sGuildFile = Path.Combine(M2Share.g_Config.sBaseDir, M2Share.g_Config.sGuildFile);
            if (_config.ReadString("Share", "VentureDir", "") == "")
                _config.WriteString("Share", "VentureDir", M2Share.g_Config.sVentureDir);
            M2Share.g_Config.sVentureDir = _config.ReadString("Share", "VentureDir", M2Share.g_Config.sVentureDir);
            if (_config.ReadString("Share", "ConLogDir", "") == "")
                _config.WriteString("Share", "ConLogDir", M2Share.g_Config.sConLogDir);
            M2Share.g_Config.sConLogDir = _config.ReadString("Share", "ConLogDir", M2Share.g_Config.sConLogDir);
            if (_config.ReadString("Share", "CastleDir", "") == "")
                _config.WriteString("Share", "CastleDir", M2Share.g_Config.sCastleDir);
            M2Share.g_Config.sCastleDir = _config.ReadString("Share", "CastleDir", M2Share.g_Config.sCastleDir);
            if (_config.ReadString("Share", "CastleFile", "") == "")
                _config.WriteString("Share", "CastleFile", M2Share.g_Config.sCastleDir + "List.txt");
            M2Share.g_Config.sCastleFile = _config.ReadString("Share", "CastleFile", M2Share.g_Config.sCastleFile);
            M2Share.g_Config.sCastleFile = Path.Combine(M2Share.g_Config.sBaseDir, M2Share.g_Config.sCastleFile);
            if (_config.ReadString("Share", "EnvirDir", "") == "")
            {
                _config.WriteString("Share", "EnvirDir", M2Share.g_Config.sEnvirDir);
            }
            M2Share.g_Config.sEnvirDir = _config.ReadString("Share", "EnvirDir", M2Share.g_Config.sEnvirDir);
            M2Share.g_Config.sEnvirDir = Path.Combine(M2Share.g_Config.sBaseDir, M2Share.g_Config.sEnvirDir);
            if (_config.ReadString("Share", "MapDir", "") == "")
                _config.WriteString("Share", "MapDir", M2Share.g_Config.sMapDir);
            M2Share.g_Config.sMapDir = _config.ReadString("Share", "MapDir", M2Share.g_Config.sMapDir);
            M2Share.g_Config.sMapDir = Path.Combine(M2Share.g_Config.sBaseDir, M2Share.g_Config.sMapDir);
            if (_config.ReadString("Share", "NoticeDir", "") == "")
            {
                _config.WriteString("Share", "NoticeDir", M2Share.g_Config.sNoticeDir);
            }
            M2Share.g_Config.sNoticeDir = _config.ReadString("Share", "NoticeDir", M2Share.g_Config.sNoticeDir);
            M2Share.g_Config.sNoticeDir = Path.Combine(M2Share.g_Config.sBaseDir, M2Share.g_Config.sNoticeDir);
            sLoadString = _config.ReadString("Share", "LogDir", "");
            if (sLoadString == "")
            {
                _config.WriteString("Share", "LogDir", M2Share.g_Config.sLogDir);
            }
            else
            {
                M2Share.g_Config.sLogDir = sLoadString;
            }
            // ============================================================================
            // 名称设置
            if (_config.ReadString("Names", "HealSkill", "") == "")
                _config.WriteString("Names", "HealSkill", M2Share.g_Config.sHealSkill);
            M2Share.g_Config.sHealSkill = _config.ReadString("Names", "HealSkill", M2Share.g_Config.sHealSkill);
            if (_config.ReadString("Names", "FireBallSkill", "") == "")
                _config.WriteString("Names", "FireBallSkill", M2Share.g_Config.sFireBallSkill);
            M2Share.g_Config.sFireBallSkill = _config.ReadString("Names", "FireBallSkill", M2Share.g_Config.sFireBallSkill);
            if (_config.ReadString("Names", "ClothsMan", "") == "")
                _config.WriteString("Names", "ClothsMan", M2Share.g_Config.sClothsMan);
            M2Share.g_Config.sClothsMan = _config.ReadString("Names", "ClothsMan", M2Share.g_Config.sClothsMan);
            if (_config.ReadString("Names", "ClothsWoman", "") == "")
                _config.WriteString("Names", "ClothsWoman", M2Share.g_Config.sClothsWoman);
            M2Share.g_Config.sClothsWoman = _config.ReadString("Names", "ClothsWoman", M2Share.g_Config.sClothsWoman);
            if (_config.ReadString("Names", "WoodenSword", "") == "")
                _config.WriteString("Names", "WoodenSword", M2Share.g_Config.sWoodenSword);
            M2Share.g_Config.sWoodenSword = _config.ReadString("Names", "WoodenSword", M2Share.g_Config.sWoodenSword);
            if (_config.ReadString("Names", "Candle", "") == "")
                _config.WriteString("Names", "Candle", M2Share.g_Config.sCandle);
            M2Share.g_Config.sCandle = _config.ReadString("Names", "Candle", M2Share.g_Config.sCandle);
            if (_config.ReadString("Names", "BasicDrug", "") == "")
                _config.WriteString("Names", "BasicDrug", M2Share.g_Config.sBasicDrug);
            M2Share.g_Config.sBasicDrug = _config.ReadString("Names", "BasicDrug", M2Share.g_Config.sBasicDrug);
            if (_config.ReadString("Names", "GoldStone", "") == "")
                _config.WriteString("Names", "GoldStone", M2Share.g_Config.sGoldStone);
            M2Share.g_Config.sGoldStone = _config.ReadString("Names", "GoldStone", M2Share.g_Config.sGoldStone);
            if (_config.ReadString("Names", "SilverStone", "") == "")
                _config.WriteString("Names", "SilverStone", M2Share.g_Config.sSilverStone);
            M2Share.g_Config.sSilverStone = _config.ReadString("Names", "SilverStone", M2Share.g_Config.sSilverStone);
            if (_config.ReadString("Names", "SteelStone", "") == "")
                _config.WriteString("Names", "SteelStone", M2Share.g_Config.sSteelStone);
            M2Share.g_Config.sSteelStone = _config.ReadString("Names", "SteelStone", M2Share.g_Config.sSteelStone);
            if (_config.ReadString("Names", "CopperStone", "") == "")
                _config.WriteString("Names", "CopperStone", M2Share.g_Config.sCopperStone);
            M2Share.g_Config.sCopperStone = _config.ReadString("Names", "CopperStone", M2Share.g_Config.sCopperStone);
            if (_config.ReadString("Names", "BlackStone", "") == "")
                _config.WriteString("Names", "BlackStone", M2Share.g_Config.sBlackStone);
            M2Share.g_Config.sBlackStone = _config.ReadString("Names", "BlackStone", M2Share.g_Config.sBlackStone);
            if (_config.ReadString("Names", "Gem1Stone", "") == "")
                _config.WriteString("Names", "Gem1Stone", M2Share.g_Config.sGemStone1);
            M2Share.g_Config.sGemStone1 = _config.ReadString("Names", "Gem1Stone", M2Share.g_Config.sGemStone1);
            if (_config.ReadString("Names", "Gem2Stone", "") == "")
                _config.WriteString("Names", "Gem2Stone", M2Share.g_Config.sGemStone2);
            M2Share.g_Config.sGemStone2 = _config.ReadString("Names", "Gem2Stone", M2Share.g_Config.sGemStone2);
            if (_config.ReadString("Names", "Gem3Stone", "") == "")
                _config.WriteString("Names", "Gem3Stone", M2Share.g_Config.sGemStone3);
            M2Share.g_Config.sGemStone3 = _config.ReadString("Names", "Gem3Stone", M2Share.g_Config.sGemStone3);
            if (_config.ReadString("Names", "Gem4Stone", "") == "")
                _config.WriteString("Names", "Gem4Stone", M2Share.g_Config.sGemStone4);
            M2Share.g_Config.sGemStone4 = _config.ReadString("Names", "Gem4Stone", M2Share.g_Config.sGemStone4);
            if (_config.ReadString("Names", "Zuma1", "") == "")
                _config.WriteString("Names", "Zuma1", M2Share.g_Config.sZuma[0]);
            M2Share.g_Config.sZuma[0] = _config.ReadString("Names", "Zuma1", M2Share.g_Config.sZuma[0]);
            if (_config.ReadString("Names", "Zuma2", "") == "")
                _config.WriteString("Names", "Zuma2", M2Share.g_Config.sZuma[1]);
            M2Share.g_Config.sZuma[1] = _config.ReadString("Names", "Zuma2", M2Share.g_Config.sZuma[1]);
            if (_config.ReadString("Names", "Zuma3", "") == "")
                _config.WriteString("Names", "Zuma3", M2Share.g_Config.sZuma[2]);
            M2Share.g_Config.sZuma[2] = _config.ReadString("Names", "Zuma3", M2Share.g_Config.sZuma[2]);
            if (_config.ReadString("Names", "Zuma4", "") == "")
                _config.WriteString("Names", "Zuma4", M2Share.g_Config.sZuma[3]);
            M2Share.g_Config.sZuma[3] = _config.ReadString("Names", "Zuma4", M2Share.g_Config.sZuma[3]);
            if (_config.ReadString("Names", "Bee", "") == "")
                _config.WriteString("Names", "Bee", M2Share.g_Config.sBee);
            M2Share.g_Config.sBee = _config.ReadString("Names", "Bee", M2Share.g_Config.sBee);
            if (_config.ReadString("Names", "Spider", "") == "")
                _config.WriteString("Names", "Spider", M2Share.g_Config.sSpider);
            M2Share.g_Config.sSpider = _config.ReadString("Names", "Spider", M2Share.g_Config.sSpider);
            if (_config.ReadString("Names", "WomaHorn", "") == "")
                _config.WriteString("Names", "WomaHorn", M2Share.g_Config.sWomaHorn);
            M2Share.g_Config.sWomaHorn = _config.ReadString("Names", "WomaHorn", M2Share.g_Config.sWomaHorn);
            if (_config.ReadString("Names", "ZumaPiece", "") == "")
                _config.WriteString("Names", "ZumaPiece", M2Share.g_Config.sZumaPiece);
            M2Share.g_Config.sZumaPiece = _config.ReadString("Names", "ZumaPiece", M2Share.g_Config.sZumaPiece);
            if (_config.ReadString("Names", "Skeleton", "") == "")
                _config.WriteString("Names", "Skeleton", M2Share.g_Config.sSkeleton);
            M2Share.g_Config.sSkeleton = _config.ReadString("Names", "Skeleton", M2Share.g_Config.sSkeleton);
            if (_config.ReadString("Names", "Dragon", "") == "")
                _config.WriteString("Names", "Dragon", M2Share.g_Config.sDragon);
            M2Share.g_Config.sDragon = _config.ReadString("Names", "Dragon", M2Share.g_Config.sDragon);
            if (_config.ReadString("Names", "Dragon1", "") == "")
                _config.WriteString("Names", "Dragon1", M2Share.g_Config.sDragon1);
            M2Share.g_Config.sDragon1 = _config.ReadString("Names", "Dragon1", M2Share.g_Config.sDragon1);
            if (_config.ReadString("Names", "Angel", "") == "")
                _config.WriteString("Names", "Angel", M2Share.g_Config.sAngel);
            M2Share.g_Config.sAngel = _config.ReadString("Names", "Angel", M2Share.g_Config.sAngel);
            sLoadString = _config.ReadString("Names", "GameGold", "");
            if (sLoadString == "")
                _config.WriteString("Share", "GameGold", M2Share.g_Config.sGameGoldName);
            else
                M2Share.g_Config.sGameGoldName = sLoadString;
            sLoadString = _config.ReadString("Names", "GamePoint", "");
            if (sLoadString == "")
                _config.WriteString("Share", "GamePoint", M2Share.g_Config.sGamePointName);
            else
                M2Share.g_Config.sGamePointName = sLoadString;
            sLoadString = _config.ReadString("Names", "PayMentPointName", "");
            if (sLoadString == "")
                _config.WriteString("Share", "PayMentPointName", M2Share.g_Config.sPayMentPointName);
            else
                M2Share.g_Config.sPayMentPointName = sLoadString;
            if (M2Share.g_Config.nAppIconCrc != 1242102148) M2Share.g_Config.boCheckFail = true;
            // ============================================================================
            // 游戏设置
            if (_config.ReadInteger("Setup", "ItemNumber", -1) < 0)
                _config.WriteInteger("Setup", "ItemNumber", M2Share.g_Config.nItemNumber);
            M2Share.g_Config.nItemNumber = _config.ReadInteger("Setup", "ItemNumber", M2Share.g_Config.nItemNumber);
            M2Share.g_Config.nItemNumber = M2Share.g_Config.nItemNumber + M2Share.RandomNumber.Random(10000);
            if (_config.ReadInteger("Setup", "ItemNumberEx", -1) < 0)
                _config.WriteInteger("Setup", "ItemNumberEx", M2Share.g_Config.nItemNumberEx);
            M2Share.g_Config.nItemNumberEx = _config.ReadInteger("Setup", "ItemNumberEx", M2Share.g_Config.nItemNumberEx);
            if (_config.ReadString("Setup", "ClientFile1", "") == "")
                _config.WriteString("Setup", "ClientFile1", M2Share.g_Config.sClientFile1);
            M2Share.g_Config.sClientFile1 = _config.ReadString("Setup", "ClientFile1", M2Share.g_Config.sClientFile1);
            if (_config.ReadString("Setup", "ClientFile2", "") == "")
                _config.WriteString("Setup", "ClientFile2", M2Share.g_Config.sClientFile2);
            M2Share.g_Config.sClientFile2 = _config.ReadString("Setup", "ClientFile2", M2Share.g_Config.sClientFile2);
            if (_config.ReadString("Setup", "ClientFile3", "") == "")
                _config.WriteString("Setup", "ClientFile3", M2Share.g_Config.sClientFile3);
            M2Share.g_Config.sClientFile3 = _config.ReadString("Setup", "ClientFile3", M2Share.g_Config.sClientFile3);
            if (_config.ReadInteger("Setup", "MonUpLvNeedKillBase", -1) < 0)
                _config.WriteInteger("Setup", "MonUpLvNeedKillBase", M2Share.g_Config.nMonUpLvNeedKillBase);
            M2Share.g_Config.nMonUpLvNeedKillBase = _config.ReadInteger("Setup", "MonUpLvNeedKillBase", M2Share.g_Config.nMonUpLvNeedKillBase);
            if (_config.ReadInteger("Setup", "MonUpLvRate", -1) < 0)
            {
                _config.WriteInteger("Setup", "MonUpLvRate", M2Share.g_Config.nMonUpLvRate);
            }
            M2Share.g_Config.nMonUpLvRate = _config.ReadInteger("Setup", "MonUpLvRate", M2Share.g_Config.nMonUpLvRate);
            for (var i = M2Share.g_Config.MonUpLvNeedKillCount.GetLowerBound(0); i <= M2Share.g_Config.MonUpLvNeedKillCount.GetUpperBound(0); i++)
            {
                if (_config.ReadInteger("Setup", "MonUpLvNeedKillCount" + i, -1) < 0)
                {
                    _config.WriteInteger("Setup", "MonUpLvNeedKillCount" + i, M2Share.g_Config.MonUpLvNeedKillCount[i]);
                }
                M2Share.g_Config.MonUpLvNeedKillCount[i] = _config.ReadInteger("Setup", "MonUpLvNeedKillCount" + i, M2Share.g_Config.MonUpLvNeedKillCount[i]);
            }
            for (var i = M2Share.g_Config.SlaveColor.GetLowerBound(0); i <= M2Share.g_Config.SlaveColor.GetUpperBound(0); i++)
            {
                if (_config.ReadInteger("Setup", "SlaveColor" + i, -1) < 0)
                {
                    _config.WriteInteger("Setup", "SlaveColor" + i, M2Share.g_Config.SlaveColor[i]);
                }
                M2Share.g_Config.SlaveColor[i] = _config.ReadInteger<byte>("Setup", "SlaveColor" + i, M2Share.g_Config.SlaveColor[i]);
            }
            if (_config.ReadString("Setup", "HomeMap", "") == "")
            {
                _config.WriteString("Setup", "HomeMap", M2Share.g_Config.sHomeMap);
            }
            M2Share.g_Config.sHomeMap = _config.ReadString("Setup", "HomeMap", M2Share.g_Config.sHomeMap);
            if (_config.ReadInteger("Setup", "HomeX", -1) < 0)
            {
                _config.WriteInteger("Setup", "HomeX", M2Share.g_Config.nHomeX);
            }
            M2Share.g_Config.nHomeX = _config.ReadInteger<short>("Setup", "HomeX", M2Share.g_Config.nHomeX);
            if (_config.ReadInteger("Setup", "HomeY", -1) < 0)
            {
                _config.WriteInteger("Setup", "HomeY", M2Share.g_Config.nHomeY);
            }
            M2Share.g_Config.nHomeY = _config.ReadInteger<short>("Setup", "HomeY", M2Share.g_Config.nHomeY);
            if (_config.ReadString("Setup", "RedHomeMap", "") == "")
            {
                _config.WriteString("Setup", "RedHomeMap", M2Share.g_Config.sRedHomeMap);
            }
            M2Share.g_Config.sRedHomeMap = _config.ReadString("Setup", "RedHomeMap", M2Share.g_Config.sRedHomeMap);
            if (_config.ReadInteger("Setup", "RedHomeX", -1) < 0)
            {
                _config.WriteInteger("Setup", "RedHomeX", M2Share.g_Config.nRedHomeX);
            }
            M2Share.g_Config.nRedHomeX = _config.ReadInteger<short>("Setup", "RedHomeX", M2Share.g_Config.nRedHomeX);
            if (_config.ReadInteger("Setup", "RedHomeY", -1) < 0)
                _config.WriteInteger("Setup", "RedHomeY", M2Share.g_Config.nRedHomeY);
            M2Share.g_Config.nRedHomeY = _config.ReadInteger<short>("Setup", "RedHomeY", M2Share.g_Config.nRedHomeY);
            if (_config.ReadString("Setup", "RedDieHomeMap", "") == "")
                _config.WriteString("Setup", "RedDieHomeMap", M2Share.g_Config.sRedDieHomeMap);
            M2Share.g_Config.sRedDieHomeMap = _config.ReadString("Setup", "RedDieHomeMap", M2Share.g_Config.sRedDieHomeMap);
            if (_config.ReadInteger("Setup", "RedDieHomeX", -1) < 0)
                _config.WriteInteger("Setup", "RedDieHomeX", M2Share.g_Config.nRedDieHomeX);
            M2Share.g_Config.nRedDieHomeX = _config.ReadInteger<short>("Setup", "RedDieHomeX", M2Share.g_Config.nRedDieHomeX);
            if (_config.ReadInteger("Setup", "RedDieHomeY", -1) < 0)
                _config.WriteInteger("Setup", "RedDieHomeY", M2Share.g_Config.nRedDieHomeY);
            M2Share.g_Config.nRedDieHomeY = _config.ReadInteger<short>("Setup", "RedDieHomeY", M2Share.g_Config.nRedDieHomeY);
            if (_config.ReadInteger("Setup", "JobHomePointSystem", -1) < 0)
                _config.WriteBool("Setup", "JobHomePointSystem", M2Share.g_Config.boJobHomePoint);
            M2Share.g_Config.boJobHomePoint = _config.ReadBool("Setup", "JobHomePointSystem", M2Share.g_Config.boJobHomePoint);
            if (_config.ReadString("Setup", "WarriorHomeMap", "") == "")
                _config.WriteString("Setup", "WarriorHomeMap", M2Share.g_Config.sWarriorHomeMap);
            M2Share.g_Config.sWarriorHomeMap = _config.ReadString("Setup", "WarriorHomeMap", M2Share.g_Config.sWarriorHomeMap);
            if (_config.ReadInteger("Setup", "WarriorHomeX", -1) < 0)
                _config.WriteInteger("Setup", "WarriorHomeX", M2Share.g_Config.nWarriorHomeX);
            M2Share.g_Config.nWarriorHomeX = _config.ReadInteger<short>("Setup", "WarriorHomeX", M2Share.g_Config.nWarriorHomeX);
            if (_config.ReadInteger("Setup", "WarriorHomeY", -1) < 0)
                _config.WriteInteger("Setup", "WarriorHomeY", M2Share.g_Config.nWarriorHomeY);
            M2Share.g_Config.nWarriorHomeY = _config.ReadInteger<short>("Setup", "WarriorHomeY", M2Share.g_Config.nWarriorHomeY);
            if (_config.ReadString("Setup", "WizardHomeMap", "") == "")
                _config.WriteString("Setup", "WizardHomeMap", M2Share.g_Config.sWizardHomeMap);
            M2Share.g_Config.sWizardHomeMap = _config.ReadString("Setup", "WizardHomeMap", M2Share.g_Config.sWizardHomeMap);
            if (_config.ReadInteger("Setup", "WizardHomeX", -1) < 0)
                _config.WriteInteger("Setup", "WizardHomeX", M2Share.g_Config.nWizardHomeX);
            M2Share.g_Config.nWizardHomeX = _config.ReadInteger<short>("Setup", "WizardHomeX", M2Share.g_Config.nWizardHomeX);
            if (_config.ReadInteger("Setup", "WizardHomeY", -1) < 0)
                _config.WriteInteger("Setup", "WizardHomeY", M2Share.g_Config.nWizardHomeY);
            M2Share.g_Config.nWizardHomeY = _config.ReadInteger<short>("Setup", "WizardHomeY", M2Share.g_Config.nWizardHomeY);
            if (_config.ReadString("Setup", "TaoistHomeMap", "") == "")
                _config.WriteString("Setup", "TaoistHomeMap", M2Share.g_Config.sTaoistHomeMap);
            M2Share.g_Config.sTaoistHomeMap = _config.ReadString("Setup", "TaoistHomeMap", M2Share.g_Config.sTaoistHomeMap);
            if (_config.ReadInteger("Setup", "TaoistHomeX", -1) < 0)
                _config.WriteInteger("Setup", "TaoistHomeX", M2Share.g_Config.nTaoistHomeX);
            M2Share.g_Config.nTaoistHomeX = _config.ReadInteger<short>("Setup", "TaoistHomeX", M2Share.g_Config.nTaoistHomeX);
            if (_config.ReadInteger("Setup", "TaoistHomeY", -1) < 0)
                _config.WriteInteger("Setup", "TaoistHomeY", M2Share.g_Config.nTaoistHomeY);
            M2Share.g_Config.nTaoistHomeY = _config.ReadInteger<short>("Setup", "TaoistHomeY", M2Share.g_Config.nTaoistHomeY);
            nLoadInteger = _config.ReadInteger("Setup", "HealthFillTime", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "HealthFillTime", M2Share.g_Config.nHealthFillTime);
            else
                M2Share.g_Config.nHealthFillTime = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "SpellFillTime", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "SpellFillTime", M2Share.g_Config.nSpellFillTime);
            else
                M2Share.g_Config.nSpellFillTime = nLoadInteger;
            if (_config.ReadInteger("Setup", "DecPkPointTime", -1) < 0)
                _config.WriteInteger("Setup", "DecPkPointTime", M2Share.g_Config.dwDecPkPointTime);
            M2Share.g_Config.dwDecPkPointTime = _config.ReadInteger("Setup", "DecPkPointTime", M2Share.g_Config.dwDecPkPointTime);
            if (_config.ReadInteger("Setup", "DecPkPointCount", -1) < 0)
                _config.WriteInteger("Setup", "DecPkPointCount", M2Share.g_Config.nDecPkPointCount);
            M2Share.g_Config.nDecPkPointCount = _config.ReadInteger("Setup", "DecPkPointCount", M2Share.g_Config.nDecPkPointCount);
            if (_config.ReadInteger("Setup", "PKFlagTime", -1) < 0)
                _config.WriteInteger("Setup", "PKFlagTime", M2Share.g_Config.dwPKFlagTime);
            M2Share.g_Config.dwPKFlagTime = _config.ReadInteger("Setup", "PKFlagTime", M2Share.g_Config.dwPKFlagTime);
            if (_config.ReadInteger("Setup", "KillHumanAddPKPoint", -1) < 0)
                _config.WriteInteger("Setup", "KillHumanAddPKPoint", M2Share.g_Config.nKillHumanAddPKPoint);
            M2Share.g_Config.nKillHumanAddPKPoint = _config.ReadInteger("Setup", "KillHumanAddPKPoint", M2Share.g_Config.nKillHumanAddPKPoint);
            if (_config.ReadInteger("Setup", "KillHumanDecLuckPoint", -1) < 0)
                _config.WriteInteger("Setup", "KillHumanDecLuckPoint", M2Share.g_Config.nKillHumanDecLuckPoint);
            M2Share.g_Config.nKillHumanDecLuckPoint = _config.ReadInteger("Setup", "KillHumanDecLuckPoint", M2Share.g_Config.nKillHumanDecLuckPoint);
            if (_config.ReadInteger("Setup", "DecLightItemDrugTime", -1) < 0)
                _config.WriteInteger("Setup", "DecLightItemDrugTime", M2Share.g_Config.dwDecLightItemDrugTime);
            M2Share.g_Config.dwDecLightItemDrugTime = _config.ReadInteger("Setup", "DecLightItemDrugTime", M2Share.g_Config.dwDecLightItemDrugTime);
            if (_config.ReadInteger("Setup", "SafeZoneSize", -1) < 0)
                _config.WriteInteger("Setup", "SafeZoneSize", M2Share.g_Config.nSafeZoneSize);
            M2Share.g_Config.nSafeZoneSize =
                _config.ReadInteger("Setup", "SafeZoneSize", M2Share.g_Config.nSafeZoneSize);
            if (_config.ReadInteger("Setup", "StartPointSize", -1) < 0)
                _config.WriteInteger("Setup", "StartPointSize", M2Share.g_Config.nStartPointSize);
            M2Share.g_Config.nStartPointSize =
                _config.ReadInteger("Setup", "StartPointSize", M2Share.g_Config.nStartPointSize);
            for (var i = M2Share.g_Config.ReNewNameColor.GetLowerBound(0); i <= M2Share.g_Config.ReNewNameColor.GetUpperBound(0); i++)
            {
                if (_config.ReadInteger("Setup", "ReNewNameColor" + i, -1) < 0)
                {
                    _config.WriteInteger("Setup", "ReNewNameColor" + i, M2Share.g_Config.ReNewNameColor[i]);
                }
                M2Share.g_Config.ReNewNameColor[i] = _config.ReadInteger<byte>("Setup", "ReNewNameColor" + i, M2Share.g_Config.ReNewNameColor[i]);
            }
            if (_config.ReadInteger("Setup", "ReNewNameColorTime", -1) < 0)
                _config.WriteInteger("Setup", "ReNewNameColorTime", M2Share.g_Config.dwReNewNameColorTime);
            M2Share.g_Config.dwReNewNameColorTime = _config.ReadInteger("Setup", "ReNewNameColorTime", M2Share.g_Config.dwReNewNameColorTime);
            if (_config.ReadInteger("Setup", "ReNewChangeColor", -1) < 0)
                _config.WriteBool("Setup", "ReNewChangeColor", M2Share.g_Config.boReNewChangeColor);
            M2Share.g_Config.boReNewChangeColor = _config.ReadBool("Setup", "ReNewChangeColor", M2Share.g_Config.boReNewChangeColor);
            if (_config.ReadInteger("Setup", "ReNewLevelClearExp", -1) < 0)
                _config.WriteBool("Setup", "ReNewLevelClearExp", M2Share.g_Config.boReNewLevelClearExp);
            M2Share.g_Config.boReNewLevelClearExp = _config.ReadBool("Setup", "ReNewLevelClearExp", M2Share.g_Config.boReNewLevelClearExp);
            if (_config.ReadInteger("Setup", "BonusAbilofWarrDC", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWarrDC", M2Share.g_Config.BonusAbilofWarr.DC);
            M2Share.g_Config.BonusAbilofWarr.DC = _config.ReadInteger<ushort>("Setup", "BonusAbilofWarrDC", M2Share.g_Config.BonusAbilofWarr.DC);
            if (_config.ReadInteger("Setup", "BonusAbilofWarrMC", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWarrMC", M2Share.g_Config.BonusAbilofWarr.MC);
            M2Share.g_Config.BonusAbilofWarr.MC = _config.ReadInteger<ushort>("Setup", "BonusAbilofWarrMC", M2Share.g_Config.BonusAbilofWarr.MC);
            if (_config.ReadInteger("Setup", "BonusAbilofWarrSC", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWarrSC", M2Share.g_Config.BonusAbilofWarr.SC);
            M2Share.g_Config.BonusAbilofWarr.SC = _config.ReadInteger<ushort>("Setup", "BonusAbilofWarrSC", M2Share.g_Config.BonusAbilofWarr.SC);
            if (_config.ReadInteger("Setup", "BonusAbilofWarrAC", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWarrAC", M2Share.g_Config.BonusAbilofWarr.AC);
            M2Share.g_Config.BonusAbilofWarr.AC = _config.ReadInteger<ushort>("Setup", "BonusAbilofWarrAC", M2Share.g_Config.BonusAbilofWarr.AC);
            if (_config.ReadInteger("Setup", "BonusAbilofWarrMAC", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWarrMAC", M2Share.g_Config.BonusAbilofWarr.MAC);
            M2Share.g_Config.BonusAbilofWarr.MAC = _config.ReadInteger<ushort>("Setup", "BonusAbilofWarrMAC", M2Share.g_Config.BonusAbilofWarr.MAC);
            if (_config.ReadInteger("Setup", "BonusAbilofWarrHP", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWarrHP", M2Share.g_Config.BonusAbilofWarr.HP);
            M2Share.g_Config.BonusAbilofWarr.HP = _config.ReadInteger<ushort>("Setup", "BonusAbilofWarrHP", M2Share.g_Config.BonusAbilofWarr.HP);
            if (_config.ReadInteger("Setup", "BonusAbilofWarrMP", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWarrMP", M2Share.g_Config.BonusAbilofWarr.MP);
            M2Share.g_Config.BonusAbilofWarr.MP = _config.ReadInteger<ushort>("Setup", "BonusAbilofWarrMP", M2Share.g_Config.BonusAbilofWarr.MP);
            if (_config.ReadInteger("Setup", "BonusAbilofWarrHit", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWarrHit", M2Share.g_Config.BonusAbilofWarr.Hit);
            M2Share.g_Config.BonusAbilofWarr.Hit = _config.ReadInteger<byte>("Setup", "BonusAbilofWarrHit", M2Share.g_Config.BonusAbilofWarr.Hit);
            if (_config.ReadInteger("Setup", "BonusAbilofWarrSpeed", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWarrSpeed", M2Share.g_Config.BonusAbilofWarr.Speed);
            M2Share.g_Config.BonusAbilofWarr.Speed = _config.ReadInteger("Setup", "BonusAbilofWarrSpeed", M2Share.g_Config.BonusAbilofWarr.Speed);
            if (_config.ReadInteger("Setup", "BonusAbilofWarrX2", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWarrX2", M2Share.g_Config.BonusAbilofWarr.X2);
            M2Share.g_Config.BonusAbilofWarr.X2 = _config.ReadInteger<byte>("Setup", "BonusAbilofWarrX2", M2Share.g_Config.BonusAbilofWarr.X2);
            if (_config.ReadInteger("Setup", "BonusAbilofWizardDC", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWizardDC", M2Share.g_Config.BonusAbilofWizard.DC);
            M2Share.g_Config.BonusAbilofWizard.DC = _config.ReadInteger<ushort>("Setup", "BonusAbilofWizardDC", M2Share.g_Config.BonusAbilofWizard.DC);
            if (_config.ReadInteger("Setup", "BonusAbilofWizardMC", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWizardMC", M2Share.g_Config.BonusAbilofWizard.MC);
            M2Share.g_Config.BonusAbilofWizard.MC = _config.ReadInteger<ushort>("Setup", "BonusAbilofWizardMC", M2Share.g_Config.BonusAbilofWizard.MC);
            if (_config.ReadInteger("Setup", "BonusAbilofWizardSC", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWizardSC", M2Share.g_Config.BonusAbilofWizard.SC);
            M2Share.g_Config.BonusAbilofWizard.SC = _config.ReadInteger<ushort>("Setup", "BonusAbilofWizardSC", M2Share.g_Config.BonusAbilofWizard.SC);
            if (_config.ReadInteger("Setup", "BonusAbilofWizardAC", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWizardAC", M2Share.g_Config.BonusAbilofWizard.AC);
            M2Share.g_Config.BonusAbilofWizard.AC = _config.ReadInteger<ushort>("Setup", "BonusAbilofWizardAC", M2Share.g_Config.BonusAbilofWizard.AC);
            if (_config.ReadInteger("Setup", "BonusAbilofWizardMAC", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWizardMAC", M2Share.g_Config.BonusAbilofWizard.MAC);
            M2Share.g_Config.BonusAbilofWizard.MAC = _config.ReadInteger<ushort>("Setup", "BonusAbilofWizardMAC", M2Share.g_Config.BonusAbilofWizard.MAC);
            if (_config.ReadInteger("Setup", "BonusAbilofWizardHP", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWizardHP", M2Share.g_Config.BonusAbilofWizard.HP);
            M2Share.g_Config.BonusAbilofWizard.HP = _config.ReadInteger<ushort>("Setup", "BonusAbilofWizardHP", M2Share.g_Config.BonusAbilofWizard.HP);
            if (_config.ReadInteger("Setup", "BonusAbilofWizardMP", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWizardMP", M2Share.g_Config.BonusAbilofWizard.MP);
            M2Share.g_Config.BonusAbilofWizard.MP = _config.ReadInteger<ushort>("Setup", "BonusAbilofWizardMP", M2Share.g_Config.BonusAbilofWizard.MP);
            if (_config.ReadInteger("Setup", "BonusAbilofWizardHit", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWizardHit", M2Share.g_Config.BonusAbilofWizard.Hit);
            M2Share.g_Config.BonusAbilofWizard.Hit = _config.ReadInteger<byte>("Setup", "BonusAbilofWizardHit", M2Share.g_Config.BonusAbilofWizard.Hit);
            if (_config.ReadInteger("Setup", "BonusAbilofWizardSpeed", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWizardSpeed", M2Share.g_Config.BonusAbilofWizard.Speed);
            M2Share.g_Config.BonusAbilofWizard.Speed = _config.ReadInteger("Setup", "BonusAbilofWizardSpeed", M2Share.g_Config.BonusAbilofWizard.Speed);
            if (_config.ReadInteger("Setup", "BonusAbilofWizardX2", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofWizardX2", M2Share.g_Config.BonusAbilofWizard.X2);
            M2Share.g_Config.BonusAbilofWizard.X2 = _config.ReadInteger<byte>("Setup", "BonusAbilofWizardX2", M2Share.g_Config.BonusAbilofWizard.X2);
            if (_config.ReadInteger("Setup", "BonusAbilofTaosDC", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofTaosDC", M2Share.g_Config.BonusAbilofTaos.DC);
            M2Share.g_Config.BonusAbilofTaos.DC = _config.ReadInteger<byte>("Setup", "BonusAbilofTaosDC", M2Share.g_Config.BonusAbilofTaos.DC);
            if (_config.ReadInteger("Setup", "BonusAbilofTaosMC", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofTaosMC", M2Share.g_Config.BonusAbilofTaos.MC);
            M2Share.g_Config.BonusAbilofTaos.MC = _config.ReadInteger<byte>("Setup", "BonusAbilofTaosMC", M2Share.g_Config.BonusAbilofTaos.MC);
            if (_config.ReadInteger("Setup", "BonusAbilofTaosSC", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofTaosSC", M2Share.g_Config.BonusAbilofTaos.SC);
            M2Share.g_Config.BonusAbilofTaos.SC = _config.ReadInteger<byte>("Setup", "BonusAbilofTaosSC", M2Share.g_Config.BonusAbilofTaos.SC);
            if (_config.ReadInteger("Setup", "BonusAbilofTaosAC", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofTaosAC", M2Share.g_Config.BonusAbilofTaos.AC);
            M2Share.g_Config.BonusAbilofTaos.AC = _config.ReadInteger<byte>("Setup", "BonusAbilofTaosAC", M2Share.g_Config.BonusAbilofTaos.AC);
            if (_config.ReadInteger("Setup", "BonusAbilofTaosMAC", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofTaosMAC", M2Share.g_Config.BonusAbilofTaos.MAC);
            M2Share.g_Config.BonusAbilofTaos.MAC = _config.ReadInteger<ushort>("Setup", "BonusAbilofTaosMAC", M2Share.g_Config.BonusAbilofTaos.MAC);
            if (_config.ReadInteger("Setup", "BonusAbilofTaosHP", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofTaosHP", M2Share.g_Config.BonusAbilofTaos.HP);
            M2Share.g_Config.BonusAbilofTaos.HP = _config.ReadInteger<ushort>("Setup", "BonusAbilofTaosHP", M2Share.g_Config.BonusAbilofTaos.HP);
            if (_config.ReadInteger("Setup", "BonusAbilofTaosMP", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofTaosMP", M2Share.g_Config.BonusAbilofTaos.MP);
            M2Share.g_Config.BonusAbilofTaos.MP = _config.ReadInteger<ushort>("Setup", "BonusAbilofTaosMP", M2Share.g_Config.BonusAbilofTaos.MP);
            if (_config.ReadInteger("Setup", "BonusAbilofTaosHit", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofTaosHit", M2Share.g_Config.BonusAbilofTaos.Hit);
            M2Share.g_Config.BonusAbilofTaos.Hit = _config.ReadInteger<byte>("Setup", "BonusAbilofTaosHit", M2Share.g_Config.BonusAbilofTaos.Hit);
            if (_config.ReadInteger("Setup", "BonusAbilofTaosSpeed", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofTaosSpeed", M2Share.g_Config.BonusAbilofTaos.Speed);
            M2Share.g_Config.BonusAbilofTaos.Speed = _config.ReadInteger("Setup", "BonusAbilofTaosSpeed", M2Share.g_Config.BonusAbilofTaos.Speed);
            if (_config.ReadInteger("Setup", "BonusAbilofTaosX2", -1) < 0)
                _config.WriteInteger("Setup", "BonusAbilofTaosX2", M2Share.g_Config.BonusAbilofTaos.X2);
            M2Share.g_Config.BonusAbilofTaos.X2 = _config.ReadInteger<byte>("Setup", "BonusAbilofTaosX2", M2Share.g_Config.BonusAbilofTaos.X2);
            if (_config.ReadInteger("Setup", "NakedAbilofWarrDC", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWarrDC", M2Share.g_Config.NakedAbilofWarr.DC);
            M2Share.g_Config.NakedAbilofWarr.DC = _config.ReadInteger<ushort>("Setup", "NakedAbilofWarrDC", M2Share.g_Config.NakedAbilofWarr.DC);
            if (_config.ReadInteger("Setup", "NakedAbilofWarrMC", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWarrMC", M2Share.g_Config.NakedAbilofWarr.MC);
            M2Share.g_Config.NakedAbilofWarr.MC = _config.ReadInteger<ushort>("Setup", "NakedAbilofWarrMC", M2Share.g_Config.NakedAbilofWarr.MC);
            if (_config.ReadInteger("Setup", "NakedAbilofWarrSC", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWarrSC", M2Share.g_Config.NakedAbilofWarr.SC);
            M2Share.g_Config.NakedAbilofWarr.SC = _config.ReadInteger<ushort>("Setup", "NakedAbilofWarrSC", M2Share.g_Config.NakedAbilofWarr.SC);
            if (_config.ReadInteger("Setup", "NakedAbilofWarrAC", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWarrAC", M2Share.g_Config.NakedAbilofWarr.AC);
            M2Share.g_Config.NakedAbilofWarr.AC = _config.ReadInteger<ushort>("Setup", "NakedAbilofWarrAC", M2Share.g_Config.NakedAbilofWarr.AC);
            if (_config.ReadInteger("Setup", "NakedAbilofWarrMAC", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWarrMAC", M2Share.g_Config.NakedAbilofWarr.MAC);
            M2Share.g_Config.NakedAbilofWarr.MAC = _config.ReadInteger<ushort>("Setup", "NakedAbilofWarrMAC", M2Share.g_Config.NakedAbilofWarr.MAC);
            if (_config.ReadInteger("Setup", "NakedAbilofWarrHP", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWarrHP", M2Share.g_Config.NakedAbilofWarr.HP);
            M2Share.g_Config.NakedAbilofWarr.HP = _config.ReadInteger<ushort>("Setup", "NakedAbilofWarrHP", M2Share.g_Config.NakedAbilofWarr.HP);
            if (_config.ReadInteger("Setup", "NakedAbilofWarrMP", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWarrMP", M2Share.g_Config.NakedAbilofWarr.MP);
            M2Share.g_Config.NakedAbilofWarr.MP = _config.ReadInteger<ushort>("Setup", "NakedAbilofWarrMP", M2Share.g_Config.NakedAbilofWarr.MP);
            if (_config.ReadInteger("Setup", "NakedAbilofWarrHit", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWarrHit", M2Share.g_Config.NakedAbilofWarr.Hit);
            M2Share.g_Config.NakedAbilofWarr.Hit = _config.ReadInteger<byte>("Setup", "NakedAbilofWarrHit", M2Share.g_Config.NakedAbilofWarr.Hit);
            if (_config.ReadInteger("Setup", "NakedAbilofWarrSpeed", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWarrSpeed", M2Share.g_Config.NakedAbilofWarr.Speed);
            M2Share.g_Config.NakedAbilofWarr.Speed = _config.ReadInteger("Setup", "NakedAbilofWarrSpeed", M2Share.g_Config.NakedAbilofWarr.Speed);
            if (_config.ReadInteger("Setup", "NakedAbilofWarrX2", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWarrX2", M2Share.g_Config.NakedAbilofWarr.X2);
            M2Share.g_Config.NakedAbilofWarr.X2 = _config.ReadInteger<byte>("Setup", "NakedAbilofWarrX2", M2Share.g_Config.NakedAbilofWarr.X2);
            if (_config.ReadInteger("Setup", "NakedAbilofWizardDC", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWizardDC", M2Share.g_Config.NakedAbilofWizard.DC);
            M2Share.g_Config.NakedAbilofWizard.DC = _config.ReadInteger<ushort>("Setup", "NakedAbilofWizardDC", M2Share.g_Config.NakedAbilofWizard.DC);
            if (_config.ReadInteger("Setup", "NakedAbilofWizardMC", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWizardMC", M2Share.g_Config.NakedAbilofWizard.MC);
            M2Share.g_Config.NakedAbilofWizard.MC = _config.ReadInteger<ushort>("Setup", "NakedAbilofWizardMC", M2Share.g_Config.NakedAbilofWizard.MC);
            if (_config.ReadInteger("Setup", "NakedAbilofWizardSC", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWizardSC", M2Share.g_Config.NakedAbilofWizard.SC);
            M2Share.g_Config.NakedAbilofWizard.SC = _config.ReadInteger<ushort>("Setup", "NakedAbilofWizardSC", M2Share.g_Config.NakedAbilofWizard.SC);
            if (_config.ReadInteger("Setup", "NakedAbilofWizardAC", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWizardAC", M2Share.g_Config.NakedAbilofWizard.AC);
            M2Share.g_Config.NakedAbilofWizard.AC = _config.ReadInteger<ushort>("Setup", "NakedAbilofWizardAC", M2Share.g_Config.NakedAbilofWizard.AC);
            if (_config.ReadInteger("Setup", "NakedAbilofWizardMAC", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWizardMAC", M2Share.g_Config.NakedAbilofWizard.MAC);
            M2Share.g_Config.NakedAbilofWizard.MAC = _config.ReadInteger<ushort>("Setup", "NakedAbilofWizardMAC", M2Share.g_Config.NakedAbilofWizard.MAC);
            if (_config.ReadInteger("Setup", "NakedAbilofWizardHP", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWizardHP", M2Share.g_Config.NakedAbilofWizard.HP);
            M2Share.g_Config.NakedAbilofWizard.HP = _config.ReadInteger<ushort>("Setup", "NakedAbilofWizardHP", M2Share.g_Config.NakedAbilofWizard.HP);
            if (_config.ReadInteger("Setup", "NakedAbilofWizardMP", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWizardMP", M2Share.g_Config.NakedAbilofWizard.MP);
            M2Share.g_Config.NakedAbilofWizard.MP = _config.ReadInteger<ushort>("Setup", "NakedAbilofWizardMP", M2Share.g_Config.NakedAbilofWizard.MP);
            if (_config.ReadInteger("Setup", "NakedAbilofWizardHit", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWizardHit", M2Share.g_Config.NakedAbilofWizard.Hit);
            M2Share.g_Config.NakedAbilofWizard.Hit = _config.ReadInteger<byte>("Setup", "NakedAbilofWizardHit", M2Share.g_Config.NakedAbilofWizard.Hit);
            if (_config.ReadInteger("Setup", "NakedAbilofWizardSpeed", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWizardSpeed", M2Share.g_Config.NakedAbilofWizard.Speed);
            M2Share.g_Config.NakedAbilofWizard.Speed = _config.ReadInteger("Setup", "NakedAbilofWizardSpeed", M2Share.g_Config.NakedAbilofWizard.Speed);
            if (_config.ReadInteger("Setup", "NakedAbilofWizardX2", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofWizardX2", M2Share.g_Config.NakedAbilofWizard.X2);
            M2Share.g_Config.NakedAbilofWizard.X2 = _config.ReadInteger<byte>("Setup", "NakedAbilofWizardX2", M2Share.g_Config.NakedAbilofWizard.X2);
            if (_config.ReadInteger("Setup", "NakedAbilofTaosDC", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofTaosDC", M2Share.g_Config.NakedAbilofTaos.DC);
            M2Share.g_Config.NakedAbilofTaos.DC = _config.ReadInteger<ushort>("Setup", "NakedAbilofTaosDC", M2Share.g_Config.NakedAbilofTaos.DC);
            if (_config.ReadInteger("Setup", "NakedAbilofTaosMC", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofTaosMC", M2Share.g_Config.NakedAbilofTaos.MC);
            M2Share.g_Config.NakedAbilofTaos.MC = _config.ReadInteger<ushort>("Setup", "NakedAbilofTaosMC", M2Share.g_Config.NakedAbilofTaos.MC);
            if (_config.ReadInteger("Setup", "NakedAbilofTaosSC", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofTaosSC", M2Share.g_Config.NakedAbilofTaos.SC);
            M2Share.g_Config.NakedAbilofTaos.SC = _config.ReadInteger<ushort>("Setup", "NakedAbilofTaosSC", M2Share.g_Config.NakedAbilofTaos.SC);
            if (_config.ReadInteger("Setup", "NakedAbilofTaosAC", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofTaosAC", M2Share.g_Config.NakedAbilofTaos.AC);
            M2Share.g_Config.NakedAbilofTaos.AC = _config.ReadInteger<ushort>("Setup", "NakedAbilofTaosAC", M2Share.g_Config.NakedAbilofTaos.AC);
            if (_config.ReadInteger("Setup", "NakedAbilofTaosMAC", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofTaosMAC", M2Share.g_Config.NakedAbilofTaos.MAC);
            M2Share.g_Config.NakedAbilofTaos.MAC = _config.ReadInteger<ushort>("Setup", "NakedAbilofTaosMAC", M2Share.g_Config.NakedAbilofTaos.MAC);
            if (_config.ReadInteger("Setup", "NakedAbilofTaosHP", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofTaosHP", M2Share.g_Config.NakedAbilofTaos.HP);
            M2Share.g_Config.NakedAbilofTaos.HP = _config.ReadInteger<ushort>("Setup", "NakedAbilofTaosHP", M2Share.g_Config.NakedAbilofTaos.HP);
            if (_config.ReadInteger("Setup", "NakedAbilofTaosMP", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofTaosMP", M2Share.g_Config.NakedAbilofTaos.MP);
            M2Share.g_Config.NakedAbilofTaos.MP = _config.ReadInteger<ushort>("Setup", "NakedAbilofTaosMP", M2Share.g_Config.NakedAbilofTaos.MP);
            if (_config.ReadInteger("Setup", "NakedAbilofTaosHit", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofTaosHit", M2Share.g_Config.NakedAbilofTaos.Hit);
            M2Share.g_Config.NakedAbilofTaos.Hit = _config.ReadInteger<byte>("Setup", "NakedAbilofTaosHit", M2Share.g_Config.NakedAbilofTaos.Hit);
            if (_config.ReadInteger("Setup", "NakedAbilofTaosSpeed", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofTaosSpeed", M2Share.g_Config.NakedAbilofTaos.Speed);
            M2Share.g_Config.NakedAbilofTaos.Speed = _config.ReadInteger("Setup", "NakedAbilofTaosSpeed", M2Share.g_Config.NakedAbilofTaos.Speed);
            if (_config.ReadInteger("Setup", "NakedAbilofTaosX2", -1) < 0)
                _config.WriteInteger("Setup", "NakedAbilofTaosX2", M2Share.g_Config.NakedAbilofTaos.X2);
            M2Share.g_Config.NakedAbilofTaos.X2 = _config.ReadInteger<byte>("Setup", "NakedAbilofTaosX2", M2Share.g_Config.NakedAbilofTaos.X2);
            if (_config.ReadInteger("Setup", "GroupMembersMax", -1) < 0)
                _config.WriteInteger("Setup", "GroupMembersMax", M2Share.g_Config.nGroupMembersMax);
            M2Share.g_Config.nGroupMembersMax = _config.ReadInteger("Setup", "GroupMembersMax", M2Share.g_Config.nGroupMembersMax);
            if (_config.ReadInteger("Setup", "WarrAttackMon", -1) < 0)
                _config.WriteInteger("Setup", "WarrAttackMon", M2Share.g_Config.nWarrMon);
            M2Share.g_Config.nWarrMon = _config.ReadInteger("Setup", "WarrAttackMon", M2Share.g_Config.nWarrMon);
            if (_config.ReadInteger("Setup", "WizardAttackMon", -1) < 0)
                _config.WriteInteger("Setup", "WizardAttackMon", M2Share.g_Config.nWizardMon);
            M2Share.g_Config.nWizardMon = _config.ReadInteger("Setup", "WizardAttackMon", M2Share.g_Config.nWizardMon);
            if (_config.ReadInteger("Setup", "TaosAttackMon", -1) < 0)
                _config.WriteInteger("Setup", "TaosAttackMon", M2Share.g_Config.nTaosMon);
            M2Share.g_Config.nTaosMon = _config.ReadInteger("Setup", "TaosAttackMon", M2Share.g_Config.nTaosMon);
            if (_config.ReadInteger("Setup", "MonAttackHum", -1) < 0)
                _config.WriteInteger("Setup", "MonAttackHum", M2Share.g_Config.nMonHum);
            M2Share.g_Config.nMonHum = _config.ReadInteger("Setup", "MonAttackHum", M2Share.g_Config.nMonHum);
            if (_config.ReadInteger("Setup", "UPgradeWeaponGetBackTime", -1) < 0)
            {
                _config.WriteInteger("Setup", "UPgradeWeaponGetBackTime", M2Share.g_Config.dwUPgradeWeaponGetBackTime);
            }
            M2Share.g_Config.dwUPgradeWeaponGetBackTime = _config.ReadInteger("Setup", "UPgradeWeaponGetBackTime", M2Share.g_Config.dwUPgradeWeaponGetBackTime);
            if (_config.ReadInteger("Setup", "ClearExpireUpgradeWeaponDays", -1) < 0)
            {
                _config.WriteInteger("Setup", "ClearExpireUpgradeWeaponDays", M2Share.g_Config.nClearExpireUpgradeWeaponDays);
            }
            M2Share.g_Config.nClearExpireUpgradeWeaponDays = _config.ReadInteger("Setup", "ClearExpireUpgradeWeaponDays", M2Share.g_Config.nClearExpireUpgradeWeaponDays);
            if (_config.ReadInteger("Setup", "UpgradeWeaponPrice", -1) < 0)
                _config.WriteInteger("Setup", "UpgradeWeaponPrice", M2Share.g_Config.nUpgradeWeaponPrice);
            M2Share.g_Config.nUpgradeWeaponPrice = _config.ReadInteger("Setup", "UpgradeWeaponPrice", M2Share.g_Config.nUpgradeWeaponPrice);
            if (_config.ReadInteger("Setup", "UpgradeWeaponMaxPoint", -1) < 0)
                _config.WriteInteger("Setup", "UpgradeWeaponMaxPoint", M2Share.g_Config.nUpgradeWeaponMaxPoint);
            M2Share.g_Config.nUpgradeWeaponMaxPoint = _config.ReadInteger("Setup", "UpgradeWeaponMaxPoint", M2Share.g_Config.nUpgradeWeaponMaxPoint);
            if (_config.ReadInteger("Setup", "UpgradeWeaponDCRate", -1) < 0)
                _config.WriteInteger("Setup", "UpgradeWeaponDCRate", M2Share.g_Config.nUpgradeWeaponDCRate);
            M2Share.g_Config.nUpgradeWeaponDCRate = _config.ReadInteger("Setup", "UpgradeWeaponDCRate", M2Share.g_Config.nUpgradeWeaponDCRate);
            if (_config.ReadInteger("Setup", "UpgradeWeaponDCTwoPointRate", -1) < 0)
                _config.WriteInteger("Setup", "UpgradeWeaponDCTwoPointRate", M2Share.g_Config.nUpgradeWeaponDCTwoPointRate);
            M2Share.g_Config.nUpgradeWeaponDCTwoPointRate = _config.ReadInteger("Setup", "UpgradeWeaponDCTwoPointRate", M2Share.g_Config.nUpgradeWeaponDCTwoPointRate);
            if (_config.ReadInteger("Setup", "UpgradeWeaponDCThreePointRate", -1) < 0)
            {
                _config.WriteInteger("Setup", "UpgradeWeaponDCThreePointRate", M2Share.g_Config.nUpgradeWeaponDCThreePointRate);
            }
            M2Share.g_Config.nUpgradeWeaponDCThreePointRate = _config.ReadInteger("Setup", "UpgradeWeaponDCThreePointRate", M2Share.g_Config.nUpgradeWeaponDCThreePointRate);
            if (_config.ReadInteger("Setup", "UpgradeWeaponMCRate", -1) < 0)
            {
                _config.WriteInteger("Setup", "UpgradeWeaponMCRate", M2Share.g_Config.nUpgradeWeaponMCRate);
            }
            M2Share.g_Config.nUpgradeWeaponMCRate = _config.ReadInteger("Setup", "UpgradeWeaponMCRate", M2Share.g_Config.nUpgradeWeaponMCRate);
            if (_config.ReadInteger("Setup", "UpgradeWeaponMCTwoPointRate", -1) < 0)
            {
                _config.WriteInteger("Setup", "UpgradeWeaponMCTwoPointRate", M2Share.g_Config.nUpgradeWeaponMCTwoPointRate);
            }
            M2Share.g_Config.nUpgradeWeaponMCTwoPointRate = _config.ReadInteger("Setup", "UpgradeWeaponMCTwoPointRate", M2Share.g_Config.nUpgradeWeaponMCTwoPointRate);
            if (_config.ReadInteger("Setup", "UpgradeWeaponMCThreePointRate", -1) < 0)
                _config.WriteInteger("Setup", "UpgradeWeaponMCThreePointRate", M2Share.g_Config.nUpgradeWeaponMCThreePointRate);
            M2Share.g_Config.nUpgradeWeaponMCThreePointRate = _config.ReadInteger("Setup", "UpgradeWeaponMCThreePointRate", M2Share.g_Config.nUpgradeWeaponMCThreePointRate);
            if (_config.ReadInteger("Setup", "UpgradeWeaponSCRate", -1) < 0)
                _config.WriteInteger("Setup", "UpgradeWeaponSCRate", M2Share.g_Config.nUpgradeWeaponSCRate);
            M2Share.g_Config.nUpgradeWeaponSCRate =
                _config.ReadInteger("Setup", "UpgradeWeaponSCRate", M2Share.g_Config.nUpgradeWeaponSCRate);
            if (_config.ReadInteger("Setup", "UpgradeWeaponSCTwoPointRate", -1) < 0)
                _config.WriteInteger("Setup", "UpgradeWeaponSCTwoPointRate", M2Share.g_Config.nUpgradeWeaponSCTwoPointRate);
            M2Share.g_Config.nUpgradeWeaponSCTwoPointRate = _config.ReadInteger("Setup", "UpgradeWeaponSCTwoPointRate", M2Share.g_Config.nUpgradeWeaponSCTwoPointRate);
            if (_config.ReadInteger("Setup", "UpgradeWeaponSCThreePointRate", -1) < 0)
            {
                _config.WriteInteger("Setup", "UpgradeWeaponSCThreePointRate", M2Share.g_Config.nUpgradeWeaponSCThreePointRate);
            }
            M2Share.g_Config.nUpgradeWeaponSCThreePointRate = _config.ReadInteger("Setup", "UpgradeWeaponSCThreePointRate", M2Share.g_Config.nUpgradeWeaponSCThreePointRate);
            if (_config.ReadInteger("Setup", "BuildGuild", -1) < 0)
                _config.WriteInteger("Setup", "BuildGuild", M2Share.g_Config.nBuildGuildPrice);
            M2Share.g_Config.nBuildGuildPrice = _config.ReadInteger("Setup", "BuildGuild", M2Share.g_Config.nBuildGuildPrice);
            if (_config.ReadInteger("Setup", "MakeDurg", -1) < 0)
                _config.WriteInteger("Setup", "MakeDurg", M2Share.g_Config.nMakeDurgPrice);
            M2Share.g_Config.nMakeDurgPrice = _config.ReadInteger("Setup", "MakeDurg", M2Share.g_Config.nMakeDurgPrice);
            if (_config.ReadInteger("Setup", "GuildWarFee", -1) < 0)
                _config.WriteInteger("Setup", "GuildWarFee", M2Share.g_Config.nGuildWarPrice);
            M2Share.g_Config.nGuildWarPrice = _config.ReadInteger("Setup", "GuildWarFee", M2Share.g_Config.nGuildWarPrice);
            if (_config.ReadInteger("Setup", "HireGuard", -1) < 0)
                _config.WriteInteger("Setup", "HireGuard", M2Share.g_Config.nHireGuardPrice);
            M2Share.g_Config.nHireGuardPrice = _config.ReadInteger("Setup", "HireGuard", M2Share.g_Config.nHireGuardPrice);
            if (_config.ReadInteger("Setup", "HireArcher", -1) < 0)
                _config.WriteInteger("Setup", "HireArcher", M2Share.g_Config.nHireArcherPrice);
            M2Share.g_Config.nHireArcherPrice = _config.ReadInteger("Setup", "HireArcher", M2Share.g_Config.nHireArcherPrice);
            if (_config.ReadInteger("Setup", "RepairDoor", -1) < 0)
                _config.WriteInteger("Setup", "RepairDoor", M2Share.g_Config.nRepairDoorPrice);
            M2Share.g_Config.nRepairDoorPrice = _config.ReadInteger("Setup", "RepairDoor", M2Share.g_Config.nRepairDoorPrice);
            if (_config.ReadInteger("Setup", "RepairWall", -1) < 0)
                _config.WriteInteger("Setup", "RepairWall", M2Share.g_Config.nRepairWallPrice);
            M2Share.g_Config.nRepairWallPrice = _config.ReadInteger("Setup", "RepairWall", M2Share.g_Config.nRepairWallPrice);
            if (_config.ReadInteger("Setup", "CastleMemberPriceRate", -1) < 0)
                _config.WriteInteger("Setup", "CastleMemberPriceRate", M2Share.g_Config.nCastleMemberPriceRate);
            M2Share.g_Config.nCastleMemberPriceRate = _config.ReadInteger("Setup", "CastleMemberPriceRate", M2Share.g_Config.nCastleMemberPriceRate);
            if (_config.ReadInteger("Setup", "CastleGoldMax", -1) < 0)
                _config.WriteInteger("Setup", "CastleGoldMax", M2Share.g_Config.nCastleGoldMax);
            M2Share.g_Config.nCastleGoldMax = _config.ReadInteger("Setup", "CastleGoldMax", M2Share.g_Config.nCastleGoldMax);
            if (_config.ReadInteger("Setup", "CastleOneDayGold", -1) < 0)
                _config.WriteInteger("Setup", "CastleOneDayGold", M2Share.g_Config.nCastleOneDayGold);
            M2Share.g_Config.nCastleOneDayGold = _config.ReadInteger("Setup", "CastleOneDayGold", M2Share.g_Config.nCastleOneDayGold);
            if (_config.ReadString("Setup", "CastleName", "") == "")
                _config.WriteString("Setup", "CastleName", M2Share.g_Config.sCastleName);
            M2Share.g_Config.sCastleName = _config.ReadString("Setup", "CastleName", M2Share.g_Config.sCastleName);
            if (_config.ReadString("Setup", "CastleHomeMap", "") == "")
                _config.WriteString("Setup", "CastleHomeMap", M2Share.g_Config.sCastleHomeMap);
            M2Share.g_Config.sCastleHomeMap = _config.ReadString("Setup", "CastleHomeMap", M2Share.g_Config.sCastleHomeMap);
            if (_config.ReadInteger("Setup", "CastleHomeX", -1) < 0)
                _config.WriteInteger("Setup", "CastleHomeX", M2Share.g_Config.nCastleHomeX);
            M2Share.g_Config.nCastleHomeX = _config.ReadInteger("Setup", "CastleHomeX", M2Share.g_Config.nCastleHomeX);
            if (_config.ReadInteger("Setup", "CastleHomeY", -1) < 0)
                _config.WriteInteger("Setup", "CastleHomeY", M2Share.g_Config.nCastleHomeY);
            M2Share.g_Config.nCastleHomeY = _config.ReadInteger("Setup", "CastleHomeY", M2Share.g_Config.nCastleHomeY);
            if (_config.ReadInteger("Setup", "CastleWarRangeX", -1) < 0)
                _config.WriteInteger("Setup", "CastleWarRangeX", M2Share.g_Config.nCastleWarRangeX);
            M2Share.g_Config.nCastleWarRangeX = _config.ReadInteger("Setup", "CastleWarRangeX", M2Share.g_Config.nCastleWarRangeX);
            if (_config.ReadInteger("Setup", "CastleWarRangeY", -1) < 0)
                _config.WriteInteger("Setup", "CastleWarRangeY", M2Share.g_Config.nCastleWarRangeY);
            M2Share.g_Config.nCastleWarRangeY = _config.ReadInteger("Setup", "CastleWarRangeY", M2Share.g_Config.nCastleWarRangeY);
            if (_config.ReadInteger("Setup", "CastleTaxRate", -1) < 0)
                _config.WriteInteger("Setup", "CastleTaxRate", M2Share.g_Config.nCastleTaxRate);
            M2Share.g_Config.nCastleTaxRate = _config.ReadInteger("Setup", "CastleTaxRate", M2Share.g_Config.nCastleTaxRate);
            if (_config.ReadInteger("Setup", "CastleGetAllNpcTax", -1) < 0)
                _config.WriteBool("Setup", "CastleGetAllNpcTax", M2Share.g_Config.boGetAllNpcTax);
            M2Share.g_Config.boGetAllNpcTax = _config.ReadBool("Setup", "CastleGetAllNpcTax", M2Share.g_Config.boGetAllNpcTax);
            nLoadInteger = _config.ReadInteger("Setup", "GenMonRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "GenMonRate", M2Share.g_Config.nMonGenRate);
            else
                M2Share.g_Config.nMonGenRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "ProcessMonRandRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "ProcessMonRandRate", M2Share.g_Config.nProcessMonRandRate);
            else
                M2Share.g_Config.nProcessMonRandRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "ProcessMonLimitCount", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "ProcessMonLimitCount", M2Share.g_Config.nProcessMonLimitCount);
            else
                M2Share.g_Config.nProcessMonLimitCount = nLoadInteger;
            if (_config.ReadInteger("Setup", "HumanMaxGold", -1) < 0)
                _config.WriteInteger("Setup", "HumanMaxGold", M2Share.g_Config.nHumanMaxGold);
            M2Share.g_Config.nHumanMaxGold = _config.ReadInteger("Setup", "HumanMaxGold", M2Share.g_Config.nHumanMaxGold);
            if (_config.ReadInteger("Setup", "HumanTryModeMaxGold", -1) < 0)
                _config.WriteInteger("Setup", "HumanTryModeMaxGold", M2Share.g_Config.nHumanTryModeMaxGold);
            M2Share.g_Config.nHumanTryModeMaxGold = _config.ReadInteger("Setup", "HumanTryModeMaxGold", M2Share.g_Config.nHumanTryModeMaxGold);
            if (_config.ReadInteger("Setup", "TryModeLevel", -1) < 0)
                _config.WriteInteger("Setup", "TryModeLevel", M2Share.g_Config.nTryModeLevel);
            M2Share.g_Config.nTryModeLevel = _config.ReadInteger("Setup", "TryModeLevel", M2Share.g_Config.nTryModeLevel);
            if (_config.ReadInteger("Setup", "TryModeUseStorage", -1) < 0)
                _config.WriteBool("Setup", "TryModeUseStorage", M2Share.g_Config.boTryModeUseStorage);
            M2Share.g_Config.boTryModeUseStorage = _config.ReadBool("Setup", "TryModeUseStorage", M2Share.g_Config.boTryModeUseStorage);
            if (_config.ReadInteger("Setup", "ShutRedMsgShowGMName", -1) < 0)
                _config.WriteBool("Setup", "ShutRedMsgShowGMName", M2Share.g_Config.boShutRedMsgShowGMName);
            M2Share.g_Config.boShutRedMsgShowGMName = _config.ReadBool("Setup", "ShutRedMsgShowGMName", M2Share.g_Config.boShutRedMsgShowGMName);
            if (_config.ReadInteger("Setup", "ShowMakeItemMsg", -1) < 0)
                _config.WriteBool("Setup", "ShowMakeItemMsg", M2Share.g_Config.boShowMakeItemMsg);
            M2Share.g_Config.boShowMakeItemMsg = _config.ReadBool("Setup", "ShowMakeItemMsg", M2Share.g_Config.boShowMakeItemMsg);
            if (_config.ReadInteger("Setup", "ShowGuildName", -1) < 0)
                _config.WriteBool("Setup", "ShowGuildName", M2Share.g_Config.boShowGuildName);
            M2Share.g_Config.boShowGuildName = _config.ReadBool("Setup", "ShowGuildName", M2Share.g_Config.boShowGuildName);
            if (_config.ReadInteger("Setup", "ShowRankLevelName", -1) < 0)
                _config.WriteBool("Setup", "ShowRankLevelName", M2Share.g_Config.boShowRankLevelName);
            M2Share.g_Config.boShowRankLevelName = _config.ReadBool("Setup", "ShowRankLevelName", M2Share.g_Config.boShowRankLevelName);
            if (_config.ReadInteger("Setup", "MonSayMsg", -1) < 0)
                _config.WriteBool("Setup", "MonSayMsg", M2Share.g_Config.boMonSayMsg);
            M2Share.g_Config.boMonSayMsg = _config.ReadBool("Setup", "MonSayMsg", M2Share.g_Config.boMonSayMsg);
            if (_config.ReadInteger("Setup", "SayMsgMaxLen", -1) < 0)
                _config.WriteInteger("Setup", "SayMsgMaxLen", M2Share.g_Config.nSayMsgMaxLen);
            M2Share.g_Config.nSayMsgMaxLen = _config.ReadInteger("Setup", "SayMsgMaxLen", M2Share.g_Config.nSayMsgMaxLen);
            if (_config.ReadInteger("Setup", "SayMsgTime", -1) < 0)
                _config.WriteInteger("Setup", "SayMsgTime", M2Share.g_Config.dwSayMsgTime);
            M2Share.g_Config.dwSayMsgTime = _config.ReadInteger("Setup", "SayMsgTime", M2Share.g_Config.dwSayMsgTime);
            if (_config.ReadInteger("Setup", "SayMsgCount", -1) < 0)
                _config.WriteInteger("Setup", "SayMsgCount", M2Share.g_Config.nSayMsgCount);
            M2Share.g_Config.nSayMsgCount = _config.ReadInteger("Setup", "SayMsgCount", M2Share.g_Config.nSayMsgCount);
            if (_config.ReadInteger("Setup", "DisableSayMsgTime", -1) < 0)
                _config.WriteInteger("Setup", "DisableSayMsgTime", M2Share.g_Config.dwDisableSayMsgTime);
            M2Share.g_Config.dwDisableSayMsgTime = _config.ReadInteger("Setup", "DisableSayMsgTime", M2Share.g_Config.dwDisableSayMsgTime);
            if (_config.ReadInteger("Setup", "SayRedMsgMaxLen", -1) < 0)
                _config.WriteInteger("Setup", "SayRedMsgMaxLen", M2Share.g_Config.nSayRedMsgMaxLen);
            M2Share.g_Config.nSayRedMsgMaxLen = _config.ReadInteger("Setup", "SayRedMsgMaxLen", M2Share.g_Config.nSayRedMsgMaxLen);
            if (_config.ReadInteger("Setup", "CanShoutMsgLevel", -1) < 0)
                _config.WriteInteger("Setup", "CanShoutMsgLevel", M2Share.g_Config.nCanShoutMsgLevel);
            M2Share.g_Config.nCanShoutMsgLevel = _config.ReadInteger("Setup", "CanShoutMsgLevel", M2Share.g_Config.nCanShoutMsgLevel);
            if (_config.ReadInteger("Setup", "StartPermission", -1) < 0)
                _config.WriteInteger("Setup", "StartPermission", M2Share.g_Config.nStartPermission);
            M2Share.g_Config.nStartPermission = _config.ReadInteger("Setup", "StartPermission", M2Share.g_Config.nStartPermission);
            if (_config.ReadInteger("Setup", "SendRefMsgRange", -1) < 0)
                _config.WriteInteger("Setup", "SendRefMsgRange", M2Share.g_Config.nSendRefMsgRange);
            M2Share.g_Config.nSendRefMsgRange = _config.ReadInteger("Setup", "SendRefMsgRange", M2Share.g_Config.nSendRefMsgRange);
            if (_config.ReadInteger("Setup", "DecLampDura", -1) < 0)
                _config.WriteBool("Setup", "DecLampDura", M2Share.g_Config.boDecLampDura);
            M2Share.g_Config.boDecLampDura = _config.ReadBool("Setup", "DecLampDura", M2Share.g_Config.boDecLampDura);
            if (_config.ReadInteger("Setup", "HungerSystem", -1) < 0)
                _config.WriteBool("Setup", "HungerSystem", M2Share.g_Config.boHungerSystem);
            M2Share.g_Config.boHungerSystem = _config.ReadBool("Setup", "HungerSystem", M2Share.g_Config.boHungerSystem);
            if (_config.ReadInteger("Setup", "HungerDecHP", -1) < 0)
                _config.WriteBool("Setup", "HungerDecHP", M2Share.g_Config.boHungerDecHP);
            M2Share.g_Config.boHungerDecHP = _config.ReadBool("Setup", "HungerDecHP", M2Share.g_Config.boHungerDecHP);
            if (_config.ReadInteger("Setup", "HungerDecPower", -1) < 0)
                _config.WriteBool("Setup", "HungerDecPower", M2Share.g_Config.boHungerDecPower);
            M2Share.g_Config.boHungerDecPower = _config.ReadBool("Setup", "HungerDecPower", M2Share.g_Config.boHungerDecPower);
            if (_config.ReadInteger("Setup", "DiableHumanRun", -1) < 0)
                _config.WriteBool("Setup", "DiableHumanRun", M2Share.g_Config.boDiableHumanRun);
            M2Share.g_Config.boDiableHumanRun = _config.ReadBool("Setup", "DiableHumanRun", M2Share.g_Config.boDiableHumanRun);
            if (_config.ReadInteger("Setup", "RunHuman", -1) < 0)
                _config.WriteBool("Setup", "RunHuman", M2Share.g_Config.boRunHuman);
            M2Share.g_Config.boRunHuman = _config.ReadBool("Setup", "RunHuman", M2Share.g_Config.boRunHuman);
            if (_config.ReadInteger("Setup", "RunMon", -1) < 0)
                _config.WriteBool("Setup", "RunMon", M2Share.g_Config.boRunMon);
            M2Share.g_Config.boRunMon = _config.ReadBool("Setup", "RunMon", M2Share.g_Config.boRunMon);
            if (_config.ReadInteger("Setup", "RunNpc", -1) < 0)
                _config.WriteBool("Setup", "RunNpc", M2Share.g_Config.boRunNpc);
            M2Share.g_Config.boRunNpc = _config.ReadBool("Setup", "RunNpc", M2Share.g_Config.boRunNpc);
            nLoadInteger = _config.ReadInteger("Setup", "RunGuard", -1);
            if (nLoadInteger < 0)
                _config.WriteBool("Setup", "RunGuard", M2Share.g_Config.boRunGuard);
            else
                M2Share.g_Config.boRunGuard = nLoadInteger == 1;
            if (_config.ReadInteger("Setup", "WarDisableHumanRun", -1) < 0)
                _config.WriteBool("Setup", "WarDisableHumanRun", M2Share.g_Config.boWarDisHumRun);
            M2Share.g_Config.boWarDisHumRun = _config.ReadBool("Setup", "WarDisableHumanRun", M2Share.g_Config.boWarDisHumRun);
            if (_config.ReadInteger("Setup", "GMRunAll", -1) < 0)
            {
                _config.WriteBool("Setup", "GMRunAll", M2Share.g_Config.boGMRunAll);
            }
            M2Share.g_Config.boGMRunAll = _config.ReadBool("Setup", "GMRunAll", M2Share.g_Config.boGMRunAll);
            if (_config.ReadInteger("Setup", "SkeletonCount", -1) < 0)
            {
                _config.WriteInteger("Setup", "SkeletonCount", M2Share.g_Config.nSkeletonCount);
            }
            M2Share.g_Config.nSkeletonCount = _config.ReadInteger("Setup", "SkeletonCount", M2Share.g_Config.nSkeletonCount);
            for (var i = M2Share.g_Config.SkeletonArray.GetLowerBound(0); i <= M2Share.g_Config.SkeletonArray.GetUpperBound(0); i++)
            {
                if (_config.ReadInteger("Setup", "SkeletonHumLevel" + i, -1) < 0)
                {
                    _config.WriteInteger("Setup", "SkeletonHumLevel" + i, M2Share.g_Config.SkeletonArray[i].nHumLevel);
                }
                M2Share.g_Config.SkeletonArray[i].nHumLevel = _config.ReadInteger("Setup", "SkeletonHumLevel" + i, M2Share.g_Config.SkeletonArray[i].nHumLevel);
                if (_config.ReadString("Names", "Skeleton" + i, "") == "")
                {
                    _config.WriteString("Names", "Skeleton" + i, M2Share.g_Config.SkeletonArray[i].sMonName);
                }
                M2Share.g_Config.SkeletonArray[i].sMonName = _config.ReadString("Names", "Skeleton" + i, M2Share.g_Config.SkeletonArray[i].sMonName);
                if (_config.ReadInteger("Setup", "SkeletonCount" + i, -1) < 0)
                {
                    _config.WriteInteger("Setup", "SkeletonCount" + i, M2Share.g_Config.SkeletonArray[i].nCount);
                }
                M2Share.g_Config.SkeletonArray[i].nCount = _config.ReadInteger("Setup", "SkeletonCount" + i, M2Share.g_Config.SkeletonArray[i].nCount);
                if (_config.ReadInteger("Setup", "SkeletonLevel" + i, -1) < 0)
                {
                    _config.WriteInteger("Setup", "SkeletonLevel" + i, M2Share.g_Config.SkeletonArray[i].nLevel);
                }
                M2Share.g_Config.SkeletonArray[i].nLevel = _config.ReadInteger("Setup", "SkeletonLevel" + i, M2Share.g_Config.SkeletonArray[i].nLevel);
            }
            if (_config.ReadInteger("Setup", "DragonCount", -1) < 0)
            {
                _config.WriteInteger("Setup", "DragonCount", M2Share.g_Config.nDragonCount);
            }
            M2Share.g_Config.nDragonCount = _config.ReadInteger("Setup", "DragonCount", M2Share.g_Config.nDragonCount);
            for (var i = M2Share.g_Config.DragonArray.GetLowerBound(0); i <= M2Share.g_Config.DragonArray.GetUpperBound(0); i++)
            {
                if (_config.ReadInteger("Setup", "DragonHumLevel" + i, -1) < 0)
                {
                    _config.WriteInteger("Setup", "DragonHumLevel" + i, M2Share.g_Config.DragonArray[i].nHumLevel);
                }
                M2Share.g_Config.DragonArray[i].nHumLevel = _config.ReadInteger("Setup", "DragonHumLevel" + i, M2Share.g_Config.DragonArray[i].nHumLevel);
                if (_config.ReadString("Names", "Dragon" + i, "") == "")
                {
                    _config.WriteString("Names", "Dragon" + i, M2Share.g_Config.DragonArray[i].sMonName);
                }
                M2Share.g_Config.DragonArray[i].sMonName = _config.ReadString("Names", "Dragon" + i, M2Share.g_Config.DragonArray[i].sMonName);
                if (_config.ReadInteger("Setup", "DragonCount" + i, -1) < 0)
                {
                    _config.WriteInteger("Setup", "DragonCount" + i, M2Share.g_Config.DragonArray[i].nCount);
                }
                M2Share.g_Config.DragonArray[i].nCount = _config.ReadInteger("Setup", "DragonCount" + i, M2Share.g_Config.DragonArray[i].nCount);
                if (_config.ReadInteger("Setup", "DragonLevel" + i, -1) < 0)
                {
                    _config.WriteInteger("Setup", "DragonLevel" + i, M2Share.g_Config.DragonArray[i].nLevel);
                }
                M2Share.g_Config.DragonArray[i].nLevel = _config.ReadInteger("Setup", "DragonLevel" + i, M2Share.g_Config.DragonArray[i].nLevel);
            }
            if (_config.ReadInteger("Setup", "TryDealTime", -1) < 0)
                _config.WriteInteger("Setup", "TryDealTime", M2Share.g_Config.dwTryDealTime);
            M2Share.g_Config.dwTryDealTime = _config.ReadInteger("Setup", "TryDealTime", M2Share.g_Config.dwTryDealTime);
            if (_config.ReadInteger("Setup", "DealOKTime", -1) < 0)
                _config.WriteInteger("Setup", "DealOKTime", M2Share.g_Config.dwDealOKTime);
            M2Share.g_Config.dwDealOKTime = _config.ReadInteger("Setup", "DealOKTime", M2Share.g_Config.dwDealOKTime);
            if (_config.ReadInteger("Setup", "CanNotGetBackDeal", -1) < 0)
                _config.WriteBool("Setup", "CanNotGetBackDeal", M2Share.g_Config.boCanNotGetBackDeal);
            M2Share.g_Config.boCanNotGetBackDeal = _config.ReadBool("Setup", "CanNotGetBackDeal", M2Share.g_Config.boCanNotGetBackDeal);
            if (_config.ReadInteger("Setup", "DisableDeal", -1) < 0)
                _config.WriteBool("Setup", "DisableDeal", M2Share.g_Config.boDisableDeal);
            M2Share.g_Config.boDisableDeal = _config.ReadBool("Setup", "DisableDeal", M2Share.g_Config.boDisableDeal);
            if (_config.ReadInteger("Setup", "MasterOKLevel", -1) < 0)
                _config.WriteInteger("Setup", "MasterOKLevel", M2Share.g_Config.nMasterOKLevel);
            M2Share.g_Config.nMasterOKLevel = _config.ReadInteger("Setup", "MasterOKLevel", M2Share.g_Config.nMasterOKLevel);
            if (_config.ReadInteger("Setup", "MasterOKCreditPoint", -1) < 0)
                _config.WriteInteger("Setup", "MasterOKCreditPoint", M2Share.g_Config.nMasterOKCreditPoint);
            M2Share.g_Config.nMasterOKCreditPoint = _config.ReadInteger("Setup", "MasterOKCreditPoint", M2Share.g_Config.nMasterOKCreditPoint);
            if (_config.ReadInteger("Setup", "MasterOKBonusPoint", -1) < 0)
                _config.WriteInteger("Setup", "MasterOKBonusPoint", M2Share.g_Config.nMasterOKBonusPoint);
            M2Share.g_Config.nMasterOKBonusPoint = _config.ReadInteger("Setup", "MasterOKBonusPoint", M2Share.g_Config.nMasterOKBonusPoint);
            if (_config.ReadInteger("Setup", "PKProtect", -1) < 0)
                _config.WriteBool("Setup", "PKProtect", M2Share.g_Config.boPKLevelProtect);
            M2Share.g_Config.boPKLevelProtect = _config.ReadBool("Setup", "PKProtect", M2Share.g_Config.boPKLevelProtect);
            if (_config.ReadInteger("Setup", "PKProtectLevel", -1) < 0)
                _config.WriteInteger("Setup", "PKProtectLevel", M2Share.g_Config.nPKProtectLevel);
            M2Share.g_Config.nPKProtectLevel = _config.ReadInteger("Setup", "PKProtectLevel", M2Share.g_Config.nPKProtectLevel);
            if (_config.ReadInteger("Setup", "RedPKProtectLevel", -1) < 0)
                _config.WriteInteger("Setup", "RedPKProtectLevel", M2Share.g_Config.nRedPKProtectLevel);
            M2Share.g_Config.nRedPKProtectLevel = _config.ReadInteger("Setup", "RedPKProtectLevel", M2Share.g_Config.nRedPKProtectLevel);
            if (_config.ReadInteger("Setup", "ItemPowerRate", -1) < 0)
                _config.WriteInteger("Setup", "ItemPowerRate", M2Share.g_Config.nItemPowerRate);
            M2Share.g_Config.nItemPowerRate = _config.ReadInteger("Setup", "ItemPowerRate", M2Share.g_Config.nItemPowerRate);
            if (_config.ReadInteger("Setup", "ItemExpRate", -1) < 0)
                _config.WriteInteger("Setup", "ItemExpRate", M2Share.g_Config.nItemExpRate);
            M2Share.g_Config.nItemExpRate = _config.ReadInteger("Setup", "ItemExpRate", M2Share.g_Config.nItemExpRate);
            if (_config.ReadInteger("Setup", "ScriptGotoCountLimit", -1) < 0)
                _config.WriteInteger("Setup", "ScriptGotoCountLimit", M2Share.g_Config.nScriptGotoCountLimit);
            M2Share.g_Config.nScriptGotoCountLimit = _config.ReadInteger("Setup", "ScriptGotoCountLimit", M2Share.g_Config.nScriptGotoCountLimit);
            if (_config.ReadInteger("Setup", "HearMsgFColor", -1) < 0)
                _config.WriteInteger("Setup", "HearMsgFColor", M2Share.g_Config.btHearMsgFColor);
            M2Share.g_Config.btHearMsgFColor = _config.ReadInteger<byte>("Setup", "HearMsgFColor", M2Share.g_Config.btHearMsgFColor);
            if (_config.ReadInteger("Setup", "HearMsgBColor", -1) < 0)
                _config.WriteInteger("Setup", "HearMsgBColor", M2Share.g_Config.btHearMsgBColor);
            M2Share.g_Config.btHearMsgBColor = _config.ReadInteger<byte>("Setup", "HearMsgBColor", M2Share.g_Config.btHearMsgBColor);
            if (_config.ReadInteger("Setup", "WhisperMsgFColor", -1) < 0)
                _config.WriteInteger("Setup", "WhisperMsgFColor", M2Share.g_Config.btWhisperMsgFColor);
            M2Share.g_Config.btWhisperMsgFColor = _config.ReadInteger<byte>("Setup", "WhisperMsgFColor", M2Share.g_Config.btWhisperMsgFColor);
            if (_config.ReadInteger("Setup", "WhisperMsgBColor", -1) < 0)
                _config.WriteInteger("Setup", "WhisperMsgBColor", M2Share.g_Config.btWhisperMsgBColor);
            M2Share.g_Config.btWhisperMsgBColor = _config.ReadInteger<byte>("Setup", "WhisperMsgBColor", M2Share.g_Config.btWhisperMsgBColor);
            if (_config.ReadInteger("Setup", "GMWhisperMsgFColor", -1) < 0)
                _config.WriteInteger("Setup", "GMWhisperMsgFColor", M2Share.g_Config.btGMWhisperMsgFColor);
            M2Share.g_Config.btGMWhisperMsgFColor = _config.ReadInteger<byte>("Setup", "GMWhisperMsgFColor", M2Share.g_Config.btGMWhisperMsgFColor);
            if (_config.ReadInteger("Setup", "GMWhisperMsgBColor", -1) < 0)
                _config.WriteInteger("Setup", "GMWhisperMsgBColor", M2Share.g_Config.btGMWhisperMsgBColor);
            M2Share.g_Config.btGMWhisperMsgBColor = _config.ReadInteger<byte>("Setup", "GMWhisperMsgBColor", M2Share.g_Config.btGMWhisperMsgBColor);
            if (_config.ReadInteger("Setup", "CryMsgFColor", -1) < 0)
                _config.WriteInteger("Setup", "CryMsgFColor", M2Share.g_Config.btCryMsgFColor);
            M2Share.g_Config.btCryMsgFColor = _config.ReadInteger<byte>("Setup", "CryMsgFColor", M2Share.g_Config.btCryMsgFColor);
            if (_config.ReadInteger("Setup", "CryMsgBColor", -1) < 0)
                _config.WriteInteger("Setup", "CryMsgBColor", M2Share.g_Config.btCryMsgBColor);
            M2Share.g_Config.btCryMsgBColor = _config.ReadInteger<byte>("Setup", "CryMsgBColor", M2Share.g_Config.btCryMsgBColor);
            if (_config.ReadInteger("Setup", "GreenMsgFColor", -1) < 0)
                _config.WriteInteger("Setup", "GreenMsgFColor", M2Share.g_Config.btGreenMsgFColor);
            M2Share.g_Config.btGreenMsgFColor = _config.ReadInteger<byte>("Setup", "GreenMsgFColor", M2Share.g_Config.btGreenMsgFColor);
            if (_config.ReadInteger("Setup", "GreenMsgBColor", -1) < 0)
                _config.WriteInteger("Setup", "GreenMsgBColor", M2Share.g_Config.btGreenMsgBColor);
            M2Share.g_Config.btGreenMsgBColor = _config.ReadInteger<byte>("Setup", "GreenMsgBColor", M2Share.g_Config.btGreenMsgBColor);
            if (_config.ReadInteger("Setup", "BlueMsgFColor", -1) < 0)
                _config.WriteInteger("Setup", "BlueMsgFColor", M2Share.g_Config.btBlueMsgFColor);
            M2Share.g_Config.btBlueMsgFColor = _config.ReadInteger<byte>("Setup", "BlueMsgFColor", M2Share.g_Config.btBlueMsgFColor);
            if (_config.ReadInteger("Setup", "BlueMsgBColor", -1) < 0)
                _config.WriteInteger("Setup", "BlueMsgBColor", M2Share.g_Config.btBlueMsgBColor);
            M2Share.g_Config.btBlueMsgBColor = _config.ReadInteger<byte>("Setup", "BlueMsgBColor", M2Share.g_Config.btBlueMsgBColor);
            if (_config.ReadInteger("Setup", "RedMsgFColor", -1) < 0)
                _config.WriteInteger("Setup", "RedMsgFColor", M2Share.g_Config.btRedMsgFColor);
            M2Share.g_Config.btRedMsgFColor = _config.ReadInteger<byte>("Setup", "RedMsgFColor", M2Share.g_Config.btRedMsgFColor);
            if (_config.ReadInteger("Setup", "RedMsgBColor", -1) < 0)
                _config.WriteInteger("Setup", "RedMsgBColor", M2Share.g_Config.btRedMsgBColor);
            M2Share.g_Config.btRedMsgBColor = _config.ReadInteger<byte>("Setup", "RedMsgBColor", M2Share.g_Config.btRedMsgBColor);
            if (_config.ReadInteger("Setup", "GuildMsgFColor", -1) < 0)
                _config.WriteInteger("Setup", "GuildMsgFColor", M2Share.g_Config.btGuildMsgFColor);
            M2Share.g_Config.btGuildMsgFColor = _config.ReadInteger<byte>("Setup", "GuildMsgFColor", M2Share.g_Config.btGuildMsgFColor);
            if (_config.ReadInteger("Setup", "GuildMsgBColor", -1) < 0)
                _config.WriteInteger("Setup", "GuildMsgBColor", M2Share.g_Config.btGuildMsgBColor);
            M2Share.g_Config.btGuildMsgBColor = _config.ReadInteger<byte>("Setup", "GuildMsgBColor", M2Share.g_Config.btGuildMsgBColor);
            if (_config.ReadInteger("Setup", "GroupMsgFColor", -1) < 0)
                _config.WriteInteger("Setup", "GroupMsgFColor", M2Share.g_Config.btGroupMsgFColor);
            M2Share.g_Config.btGroupMsgFColor = _config.ReadInteger<byte>("Setup", "GroupMsgFColor", M2Share.g_Config.btGroupMsgFColor);
            if (_config.ReadInteger("Setup", "GroupMsgBColor", -1) < 0)
                _config.WriteInteger("Setup", "GroupMsgBColor", M2Share.g_Config.btGroupMsgBColor);
            M2Share.g_Config.btGroupMsgBColor = _config.ReadInteger<byte>("Setup", "GroupMsgBColor", M2Share.g_Config.btGroupMsgBColor);
            if (_config.ReadInteger("Setup", "CustMsgFColor", -1) < 0)
                _config.WriteInteger("Setup", "CustMsgFColor", M2Share.g_Config.btCustMsgFColor);
            M2Share.g_Config.btCustMsgFColor = _config.ReadInteger<byte>("Setup", "CustMsgFColor", M2Share.g_Config.btCustMsgFColor);
            if (_config.ReadInteger("Setup", "CustMsgBColor", -1) < 0)
                _config.WriteInteger("Setup", "CustMsgBColor", M2Share.g_Config.btCustMsgBColor);
            M2Share.g_Config.btCustMsgBColor = _config.ReadInteger<byte>("Setup", "CustMsgBColor", M2Share.g_Config.btCustMsgBColor);
            if (_config.ReadInteger("Setup", "MonRandomAddValue", -1) < 0)
                _config.WriteInteger("Setup", "MonRandomAddValue", M2Share.g_Config.nMonRandomAddValue);
            M2Share.g_Config.nMonRandomAddValue = _config.ReadInteger("Setup", "MonRandomAddValue", M2Share.g_Config.nMonRandomAddValue);
            if (_config.ReadInteger("Setup", "MakeRandomAddValue", -1) < 0)
                _config.WriteInteger("Setup", "MakeRandomAddValue", M2Share.g_Config.nMakeRandomAddValue);
            M2Share.g_Config.nMakeRandomAddValue = _config.ReadInteger("Setup", "MakeRandomAddValue", M2Share.g_Config.nMakeRandomAddValue);
            if (_config.ReadInteger("Setup", "WeaponDCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "WeaponDCAddValueMaxLimit", M2Share.g_Config.nWeaponDCAddValueMaxLimit);
            M2Share.g_Config.nWeaponDCAddValueMaxLimit = _config.ReadInteger("Setup", "WeaponDCAddValueMaxLimit", M2Share.g_Config.nWeaponDCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "WeaponDCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "WeaponDCAddValueRate", M2Share.g_Config.nWeaponDCAddValueRate);
            M2Share.g_Config.nWeaponDCAddValueRate = _config.ReadInteger("Setup", "WeaponDCAddValueRate", M2Share.g_Config.nWeaponDCAddValueRate);
            if (_config.ReadInteger("Setup", "WeaponMCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "WeaponMCAddValueMaxLimit", M2Share.g_Config.nWeaponMCAddValueMaxLimit);
            M2Share.g_Config.nWeaponMCAddValueMaxLimit = _config.ReadInteger("Setup", "WeaponMCAddValueMaxLimit", M2Share.g_Config.nWeaponMCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "WeaponMCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "WeaponMCAddValueRate", M2Share.g_Config.nWeaponMCAddValueRate);
            M2Share.g_Config.nWeaponMCAddValueRate = _config.ReadInteger("Setup", "WeaponMCAddValueRate", M2Share.g_Config.nWeaponMCAddValueRate);
            if (_config.ReadInteger("Setup", "WeaponSCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "WeaponSCAddValueMaxLimit", M2Share.g_Config.nWeaponSCAddValueMaxLimit);
            M2Share.g_Config.nWeaponSCAddValueMaxLimit = _config.ReadInteger("Setup", "WeaponSCAddValueMaxLimit", M2Share.g_Config.nWeaponSCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "WeaponSCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "WeaponSCAddValueRate", M2Share.g_Config.nWeaponSCAddValueRate);
            M2Share.g_Config.nWeaponSCAddValueRate = _config.ReadInteger("Setup", "WeaponSCAddValueRate", M2Share.g_Config.nWeaponSCAddValueRate);
            if (_config.ReadInteger("Setup", "DressDCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "DressDCAddValueMaxLimit", M2Share.g_Config.nDressDCAddValueMaxLimit);
            M2Share.g_Config.nDressDCAddValueMaxLimit = _config.ReadInteger("Setup", "DressDCAddValueMaxLimit", M2Share.g_Config.nDressDCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "DressDCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "DressDCAddValueRate", M2Share.g_Config.nDressDCAddValueRate);
            M2Share.g_Config.nDressDCAddValueRate = _config.ReadInteger("Setup", "DressDCAddValueRate", M2Share.g_Config.nDressDCAddValueRate);
            if (_config.ReadInteger("Setup", "DressDCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "DressDCAddRate", M2Share.g_Config.nDressDCAddRate);
            M2Share.g_Config.nDressDCAddRate = _config.ReadInteger("Setup", "DressDCAddRate", M2Share.g_Config.nDressDCAddRate);
            if (_config.ReadInteger("Setup", "DressMCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "DressMCAddValueMaxLimit", M2Share.g_Config.nDressMCAddValueMaxLimit);
            M2Share.g_Config.nDressMCAddValueMaxLimit = _config.ReadInteger("Setup", "DressMCAddValueMaxLimit", M2Share.g_Config.nDressMCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "DressMCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "DressMCAddValueRate", M2Share.g_Config.nDressMCAddValueRate);
            M2Share.g_Config.nDressMCAddValueRate = _config.ReadInteger("Setup", "DressMCAddValueRate", M2Share.g_Config.nDressMCAddValueRate);
            if (_config.ReadInteger("Setup", "DressMCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "DressMCAddRate", M2Share.g_Config.nDressMCAddRate);
            M2Share.g_Config.nDressMCAddRate = _config.ReadInteger("Setup", "DressMCAddRate", M2Share.g_Config.nDressMCAddRate);
            if (_config.ReadInteger("Setup", "DressSCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "DressSCAddValueMaxLimit", M2Share.g_Config.nDressSCAddValueMaxLimit);
            M2Share.g_Config.nDressSCAddValueMaxLimit = _config.ReadInteger("Setup", "DressSCAddValueMaxLimit", M2Share.g_Config.nDressSCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "DressSCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "DressSCAddValueRate", M2Share.g_Config.nDressSCAddValueRate);
            M2Share.g_Config.nDressSCAddValueRate = _config.ReadInteger("Setup", "DressSCAddValueRate", M2Share.g_Config.nDressSCAddValueRate);
            if (_config.ReadInteger("Setup", "DressSCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "DressSCAddRate", M2Share.g_Config.nDressSCAddRate);
            M2Share.g_Config.nDressSCAddRate = _config.ReadInteger("Setup", "DressSCAddRate", M2Share.g_Config.nDressSCAddRate);
            if (_config.ReadInteger("Setup", "NeckLace19DCAddValueMaxLimit", -1) < 0)
            {
                _config.WriteInteger("Setup", "NeckLace19DCAddValueMaxLimit", M2Share.g_Config.nNeckLace19DCAddValueMaxLimit);
            }
            M2Share.g_Config.nNeckLace19DCAddValueMaxLimit = _config.ReadInteger("Setup", "NeckLace19DCAddValueMaxLimit", M2Share.g_Config.nNeckLace19DCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "NeckLace19DCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace19DCAddValueRate", M2Share.g_Config.nNeckLace19DCAddValueRate);
            M2Share.g_Config.nNeckLace19DCAddValueRate = _config.ReadInteger("Setup", "NeckLace19DCAddValueRate", M2Share.g_Config.nNeckLace19DCAddValueRate);
            if (_config.ReadInteger("Setup", "NeckLace19DCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace19DCAddRate", M2Share.g_Config.nNeckLace19DCAddRate);
            M2Share.g_Config.nNeckLace19DCAddRate = _config.ReadInteger("Setup", "NeckLace19DCAddRate", M2Share.g_Config.nNeckLace19DCAddRate);
            if (_config.ReadInteger("Setup", "NeckLace19MCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace19MCAddValueMaxLimit", M2Share.g_Config.nNeckLace19MCAddValueMaxLimit);
            M2Share.g_Config.nNeckLace19MCAddValueMaxLimit = _config.ReadInteger("Setup", "NeckLace19MCAddValueMaxLimit", M2Share.g_Config.nNeckLace19MCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "NeckLace19MCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace19MCAddValueRate", M2Share.g_Config.nNeckLace19MCAddValueRate);
            M2Share.g_Config.nNeckLace19MCAddValueRate = _config.ReadInteger("Setup", "NeckLace19MCAddValueRate", M2Share.g_Config.nNeckLace19MCAddValueRate);
            if (_config.ReadInteger("Setup", "NeckLace19MCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace19MCAddRate", M2Share.g_Config.nNeckLace19MCAddRate);
            M2Share.g_Config.nNeckLace19MCAddRate = _config.ReadInteger("Setup", "NeckLace19MCAddRate", M2Share.g_Config.nNeckLace19MCAddRate);
            if (_config.ReadInteger("Setup", "NeckLace19SCAddValueMaxLimit", -1) < 0)
            {
                _config.WriteInteger("Setup", "NeckLace19SCAddValueMaxLimit", M2Share.g_Config.nNeckLace19SCAddValueMaxLimit);
            }
            M2Share.g_Config.nNeckLace19SCAddValueMaxLimit = _config.ReadInteger("Setup", "NeckLace19SCAddValueMaxLimit", M2Share.g_Config.nNeckLace19SCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "NeckLace19SCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace19SCAddValueRate", M2Share.g_Config.nNeckLace19SCAddValueRate);
            M2Share.g_Config.nNeckLace19SCAddValueRate = _config.ReadInteger("Setup", "NeckLace19SCAddValueRate", M2Share.g_Config.nNeckLace19SCAddValueRate);
            if (_config.ReadInteger("Setup", "NeckLace19SCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace19SCAddRate", M2Share.g_Config.nNeckLace19SCAddRate);
            M2Share.g_Config.nNeckLace19SCAddRate = _config.ReadInteger("Setup", "NeckLace19SCAddRate", M2Share.g_Config.nNeckLace19SCAddRate);
            if (_config.ReadInteger("Setup", "NeckLace202124DCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace202124DCAddValueMaxLimit", M2Share.g_Config.nNeckLace202124DCAddValueMaxLimit);
            M2Share.g_Config.nNeckLace202124DCAddValueMaxLimit = _config.ReadInteger("Setup", "NeckLace202124DCAddValueMaxLimit", M2Share.g_Config.nNeckLace202124DCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "NeckLace202124DCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace202124DCAddValueRate", M2Share.g_Config.nNeckLace202124DCAddValueRate);
            M2Share.g_Config.nNeckLace202124DCAddValueRate = _config.ReadInteger("Setup", "NeckLace202124DCAddValueRate", M2Share.g_Config.nNeckLace202124DCAddValueRate);
            if (_config.ReadInteger("Setup", "NeckLace202124DCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace202124DCAddRate", M2Share.g_Config.nNeckLace202124DCAddRate);
            M2Share.g_Config.nNeckLace202124DCAddRate = _config.ReadInteger("Setup", "NeckLace202124DCAddRate", M2Share.g_Config.nNeckLace202124DCAddRate);
            if (_config.ReadInteger("Setup", "NeckLace202124MCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace202124MCAddValueMaxLimit", M2Share.g_Config.nNeckLace202124MCAddValueMaxLimit);
            M2Share.g_Config.nNeckLace202124MCAddValueMaxLimit = _config.ReadInteger("Setup", "NeckLace202124MCAddValueMaxLimit", M2Share.g_Config.nNeckLace202124MCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "NeckLace202124MCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace202124MCAddValueRate", M2Share.g_Config.nNeckLace202124MCAddValueRate);
            M2Share.g_Config.nNeckLace202124MCAddValueRate = _config.ReadInteger("Setup", "NeckLace202124MCAddValueRate", M2Share.g_Config.nNeckLace202124MCAddValueRate);
            if (_config.ReadInteger("Setup", "NeckLace202124MCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace202124MCAddRate", M2Share.g_Config.nNeckLace202124MCAddRate);
            M2Share.g_Config.nNeckLace202124MCAddRate = _config.ReadInteger("Setup", "NeckLace202124MCAddRate", M2Share.g_Config.nNeckLace202124MCAddRate);
            if (_config.ReadInteger("Setup", "NeckLace202124SCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace202124SCAddValueMaxLimit", M2Share.g_Config.nNeckLace202124SCAddValueMaxLimit);
            M2Share.g_Config.nNeckLace202124SCAddValueMaxLimit = _config.ReadInteger("Setup", "NeckLace202124SCAddValueMaxLimit", M2Share.g_Config.nNeckLace202124SCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "NeckLace202124SCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace202124SCAddValueRate", M2Share.g_Config.nNeckLace202124SCAddValueRate);
            M2Share.g_Config.nNeckLace202124SCAddValueRate = _config.ReadInteger("Setup", "NeckLace202124SCAddValueRate", M2Share.g_Config.nNeckLace202124SCAddValueRate);
            if (_config.ReadInteger("Setup", "NeckLace202124SCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "NeckLace202124SCAddRate", M2Share.g_Config.nNeckLace202124SCAddRate);
            M2Share.g_Config.nNeckLace202124SCAddRate = _config.ReadInteger("Setup", "NeckLace202124SCAddRate", M2Share.g_Config.nNeckLace202124SCAddRate);
            if (_config.ReadInteger("Setup", "ArmRing26DCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "ArmRing26DCAddValueMaxLimit", M2Share.g_Config.nArmRing26DCAddValueMaxLimit);
            M2Share.g_Config.nArmRing26DCAddValueMaxLimit = _config.ReadInteger("Setup", "ArmRing26DCAddValueMaxLimit", M2Share.g_Config.nArmRing26DCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "ArmRing26DCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "ArmRing26DCAddValueRate", M2Share.g_Config.nArmRing26DCAddValueRate);
            M2Share.g_Config.nArmRing26DCAddValueRate = _config.ReadInteger("Setup", "ArmRing26DCAddValueRate", M2Share.g_Config.nArmRing26DCAddValueRate);
            if (_config.ReadInteger("Setup", "ArmRing26DCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "ArmRing26DCAddRate", M2Share.g_Config.nArmRing26DCAddRate);
            M2Share.g_Config.nArmRing26DCAddRate = _config.ReadInteger("Setup", "ArmRing26DCAddRate", M2Share.g_Config.nArmRing26DCAddRate);
            if (_config.ReadInteger("Setup", "ArmRing26MCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "ArmRing26MCAddValueMaxLimit", M2Share.g_Config.nArmRing26MCAddValueMaxLimit);
            M2Share.g_Config.nArmRing26MCAddValueMaxLimit = _config.ReadInteger("Setup", "ArmRing26MCAddValueMaxLimit", M2Share.g_Config.nArmRing26MCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "ArmRing26MCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "ArmRing26MCAddValueRate", M2Share.g_Config.nArmRing26MCAddValueRate);
            M2Share.g_Config.nArmRing26MCAddValueRate = _config.ReadInteger("Setup", "ArmRing26MCAddValueRate", M2Share.g_Config.nArmRing26MCAddValueRate);
            if (_config.ReadInteger("Setup", "ArmRing26MCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "ArmRing26MCAddRate", M2Share.g_Config.nArmRing26MCAddRate);
            M2Share.g_Config.nArmRing26MCAddRate = _config.ReadInteger("Setup", "ArmRing26MCAddRate", M2Share.g_Config.nArmRing26MCAddRate);
            if (_config.ReadInteger("Setup", "ArmRing26SCAddValueMaxLimit", -1) < 0)
            {
                _config.WriteInteger("Setup", "ArmRing26SCAddValueMaxLimit", M2Share.g_Config.nArmRing26SCAddValueMaxLimit);
            }
            M2Share.g_Config.nArmRing26SCAddValueMaxLimit = _config.ReadInteger("Setup", "ArmRing26SCAddValueMaxLimit", M2Share.g_Config.nArmRing26SCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "ArmRing26SCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "ArmRing26SCAddValueRate", M2Share.g_Config.nArmRing26SCAddValueRate);
            M2Share.g_Config.nArmRing26SCAddValueRate = _config.ReadInteger("Setup", "ArmRing26SCAddValueRate", M2Share.g_Config.nArmRing26SCAddValueRate);
            if (_config.ReadInteger("Setup", "ArmRing26SCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "ArmRing26SCAddRate", M2Share.g_Config.nArmRing26SCAddRate);
            M2Share.g_Config.nArmRing26SCAddRate = _config.ReadInteger("Setup", "ArmRing26SCAddRate", M2Share.g_Config.nArmRing26SCAddRate);
            if (_config.ReadInteger("Setup", "Ring22DCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "Ring22DCAddValueMaxLimit", M2Share.g_Config.nRing22DCAddValueMaxLimit);
            M2Share.g_Config.nRing22DCAddValueMaxLimit = _config.ReadInteger("Setup", "Ring22DCAddValueMaxLimit", M2Share.g_Config.nRing22DCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "Ring22DCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "Ring22DCAddValueRate", M2Share.g_Config.nRing22DCAddValueRate);
            M2Share.g_Config.nRing22DCAddValueRate = _config.ReadInteger("Setup", "Ring22DCAddValueRate", M2Share.g_Config.nRing22DCAddValueRate);
            if (_config.ReadInteger("Setup", "Ring22DCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "Ring22DCAddRate", M2Share.g_Config.nRing22DCAddRate);
            M2Share.g_Config.nRing22DCAddRate = _config.ReadInteger("Setup", "Ring22DCAddRate", M2Share.g_Config.nRing22DCAddRate);
            if (_config.ReadInteger("Setup", "Ring22MCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "Ring22MCAddValueMaxLimit", M2Share.g_Config.nRing22MCAddValueMaxLimit);
            M2Share.g_Config.nRing22MCAddValueMaxLimit = _config.ReadInteger("Setup", "Ring22MCAddValueMaxLimit", M2Share.g_Config.nRing22MCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "Ring22MCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "Ring22MCAddValueRate", M2Share.g_Config.nRing22MCAddValueRate);
            M2Share.g_Config.nRing22MCAddValueRate = _config.ReadInteger("Setup", "Ring22MCAddValueRate", M2Share.g_Config.nRing22MCAddValueRate);
            if (_config.ReadInteger("Setup", "Ring22MCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "Ring22MCAddRate", M2Share.g_Config.nRing22MCAddRate);
            M2Share.g_Config.nRing22MCAddRate = _config.ReadInteger("Setup", "Ring22MCAddRate", M2Share.g_Config.nRing22MCAddRate);
            if (_config.ReadInteger("Setup", "Ring22SCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "Ring22SCAddValueMaxLimit", M2Share.g_Config.nRing22SCAddValueMaxLimit);
            M2Share.g_Config.nRing22SCAddValueMaxLimit = _config.ReadInteger("Setup", "Ring22SCAddValueMaxLimit", M2Share.g_Config.nRing22SCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "Ring22SCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "Ring22SCAddValueRate", M2Share.g_Config.nRing22SCAddValueRate);
            M2Share.g_Config.nRing22SCAddValueRate = _config.ReadInteger("Setup", "Ring22SCAddValueRate", M2Share.g_Config.nRing22SCAddValueRate);
            if (_config.ReadInteger("Setup", "Ring22SCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "Ring22SCAddRate", M2Share.g_Config.nRing22SCAddRate);
            M2Share.g_Config.nRing22SCAddRate = _config.ReadInteger("Setup", "Ring22SCAddRate", M2Share.g_Config.nRing22SCAddRate);
            if (_config.ReadInteger("Setup", "Ring23DCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "Ring23DCAddValueMaxLimit", M2Share.g_Config.nRing23DCAddValueMaxLimit);
            M2Share.g_Config.nRing23DCAddValueMaxLimit = _config.ReadInteger("Setup", "Ring23DCAddValueMaxLimit", M2Share.g_Config.nRing23DCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "Ring23DCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "Ring23DCAddValueRate", M2Share.g_Config.nRing23DCAddValueRate);
            M2Share.g_Config.nRing23DCAddValueRate = _config.ReadInteger("Setup", "Ring23DCAddValueRate", M2Share.g_Config.nRing23DCAddValueRate);
            if (_config.ReadInteger("Setup", "Ring23DCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "Ring23DCAddRate", M2Share.g_Config.nRing23DCAddRate);
            M2Share.g_Config.nRing23DCAddRate = _config.ReadInteger("Setup", "Ring23DCAddRate", M2Share.g_Config.nRing23DCAddRate);
            if (_config.ReadInteger("Setup", "Ring23MCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "Ring23MCAddValueMaxLimit", M2Share.g_Config.nRing23MCAddValueMaxLimit);
            M2Share.g_Config.nRing23MCAddValueMaxLimit = _config.ReadInteger("Setup", "Ring23MCAddValueMaxLimit", M2Share.g_Config.nRing23MCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "Ring23MCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "Ring23MCAddValueRate", M2Share.g_Config.nRing23MCAddValueRate);
            M2Share.g_Config.nRing23MCAddValueRate = _config.ReadInteger("Setup", "Ring23MCAddValueRate", M2Share.g_Config.nRing23MCAddValueRate);
            if (_config.ReadInteger("Setup", "Ring23MCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "Ring23MCAddRate", M2Share.g_Config.nRing23MCAddRate);
            M2Share.g_Config.nRing23MCAddRate = _config.ReadInteger("Setup", "Ring23MCAddRate", M2Share.g_Config.nRing23MCAddRate);
            if (_config.ReadInteger("Setup", "Ring23SCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "Ring23SCAddValueMaxLimit", M2Share.g_Config.nRing23SCAddValueMaxLimit);
            M2Share.g_Config.nRing23SCAddValueMaxLimit = _config.ReadInteger("Setup", "Ring23SCAddValueMaxLimit", M2Share.g_Config.nRing23SCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "Ring23SCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "Ring23SCAddValueRate", M2Share.g_Config.nRing23SCAddValueRate);
            M2Share.g_Config.nRing23SCAddValueRate = _config.ReadInteger("Setup", "Ring23SCAddValueRate", M2Share.g_Config.nRing23SCAddValueRate);
            if (_config.ReadInteger("Setup", "Ring23SCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "Ring23SCAddRate", M2Share.g_Config.nRing23SCAddRate);
            M2Share.g_Config.nRing23SCAddRate = _config.ReadInteger("Setup", "Ring23SCAddRate", M2Share.g_Config.nRing23SCAddRate);
            if (_config.ReadInteger("Setup", "HelMetDCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "HelMetDCAddValueMaxLimit", M2Share.g_Config.nHelMetDCAddValueMaxLimit);
            M2Share.g_Config.nHelMetDCAddValueMaxLimit = _config.ReadInteger("Setup", "HelMetDCAddValueMaxLimit", M2Share.g_Config.nHelMetDCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "HelMetDCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "HelMetDCAddValueRate", M2Share.g_Config.nHelMetDCAddValueRate);
            M2Share.g_Config.nHelMetDCAddValueRate = _config.ReadInteger("Setup", "HelMetDCAddValueRate", M2Share.g_Config.nHelMetDCAddValueRate);
            if (_config.ReadInteger("Setup", "HelMetDCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "HelMetDCAddRate", M2Share.g_Config.nHelMetDCAddRate);
            M2Share.g_Config.nHelMetDCAddRate = _config.ReadInteger("Setup", "HelMetDCAddRate", M2Share.g_Config.nHelMetDCAddRate);
            if (_config.ReadInteger("Setup", "HelMetMCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "HelMetMCAddValueMaxLimit", M2Share.g_Config.nHelMetMCAddValueMaxLimit);
            M2Share.g_Config.nHelMetMCAddValueMaxLimit = _config.ReadInteger("Setup", "HelMetMCAddValueMaxLimit", M2Share.g_Config.nHelMetMCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "HelMetMCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "HelMetMCAddValueRate", M2Share.g_Config.nHelMetMCAddValueRate);
            M2Share.g_Config.nHelMetMCAddValueRate = _config.ReadInteger("Setup", "HelMetMCAddValueRate", M2Share.g_Config.nHelMetMCAddValueRate);
            if (_config.ReadInteger("Setup", "HelMetMCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "HelMetMCAddRate", M2Share.g_Config.nHelMetMCAddRate);
            M2Share.g_Config.nHelMetMCAddRate = _config.ReadInteger("Setup", "HelMetMCAddRate", M2Share.g_Config.nHelMetMCAddRate);
            if (_config.ReadInteger("Setup", "HelMetSCAddValueMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "HelMetSCAddValueMaxLimit", M2Share.g_Config.nHelMetSCAddValueMaxLimit);
            M2Share.g_Config.nHelMetSCAddValueMaxLimit = _config.ReadInteger("Setup", "HelMetSCAddValueMaxLimit", M2Share.g_Config.nHelMetSCAddValueMaxLimit);
            if (_config.ReadInteger("Setup", "HelMetSCAddValueRate", -1) < 0)
                _config.WriteInteger("Setup", "HelMetSCAddValueRate", M2Share.g_Config.nHelMetSCAddValueRate);
            M2Share.g_Config.nHelMetSCAddValueRate = _config.ReadInteger("Setup", "HelMetSCAddValueRate", M2Share.g_Config.nHelMetSCAddValueRate);
            if (_config.ReadInteger("Setup", "HelMetSCAddRate", -1) < 0)
                _config.WriteInteger("Setup", "HelMetSCAddRate", M2Share.g_Config.nHelMetSCAddRate);
            M2Share.g_Config.nHelMetSCAddRate = _config.ReadInteger("Setup", "HelMetSCAddRate", M2Share.g_Config.nHelMetSCAddRate);
            nLoadInteger = _config.ReadInteger("Setup", "UnknowHelMetACAddRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowHelMetACAddRate", M2Share.g_Config.nUnknowHelMetACAddRate);
            else
                M2Share.g_Config.nUnknowHelMetACAddRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowHelMetACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowHelMetACAddValueMaxLimit", M2Share.g_Config.nUnknowHelMetACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowHelMetACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowHelMetMACAddRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowHelMetMACAddRate", M2Share.g_Config.nUnknowHelMetMACAddRate);
            else
                M2Share.g_Config.nUnknowHelMetMACAddRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowHelMetMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowHelMetMACAddValueMaxLimit", M2Share.g_Config.nUnknowHelMetMACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowHelMetMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowHelMetDCAddRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowHelMetDCAddRate", M2Share.g_Config.nUnknowHelMetDCAddRate);
            else
                M2Share.g_Config.nUnknowHelMetDCAddRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowHelMetDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowHelMetDCAddValueMaxLimit", M2Share.g_Config.nUnknowHelMetDCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowHelMetDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowHelMetMCAddRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowHelMetMCAddRate", M2Share.g_Config.nUnknowHelMetMCAddRate);
            else
                M2Share.g_Config.nUnknowHelMetMCAddRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowHelMetMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowHelMetMCAddValueMaxLimit", M2Share.g_Config.nUnknowHelMetMCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowHelMetMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowHelMetSCAddRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowHelMetSCAddRate", M2Share.g_Config.nUnknowHelMetSCAddRate);
            else
                M2Share.g_Config.nUnknowHelMetSCAddRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowHelMetSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowHelMetSCAddValueMaxLimit", M2Share.g_Config.nUnknowHelMetSCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowHelMetSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowNecklaceACAddRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowNecklaceACAddRate", M2Share.g_Config.nUnknowNecklaceACAddRate);
            else
                M2Share.g_Config.nUnknowNecklaceACAddRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowNecklaceACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowNecklaceACAddValueMaxLimit", M2Share.g_Config.nUnknowNecklaceACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowNecklaceACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowNecklaceMACAddRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowNecklaceMACAddRate", M2Share.g_Config.nUnknowNecklaceMACAddRate);
            else
                M2Share.g_Config.nUnknowNecklaceMACAddRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowNecklaceMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowNecklaceMACAddValueMaxLimit", M2Share.g_Config.nUnknowNecklaceMACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowNecklaceMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowNecklaceDCAddRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowNecklaceDCAddRate", M2Share.g_Config.nUnknowNecklaceDCAddRate);
            else
                M2Share.g_Config.nUnknowNecklaceDCAddRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowNecklaceDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowNecklaceDCAddValueMaxLimit", M2Share.g_Config.nUnknowNecklaceDCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowNecklaceDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowNecklaceMCAddRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowNecklaceMCAddRate", M2Share.g_Config.nUnknowNecklaceMCAddRate);
            else
                M2Share.g_Config.nUnknowNecklaceMCAddRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowNecklaceMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowNecklaceMCAddValueMaxLimit", M2Share.g_Config.nUnknowNecklaceMCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowNecklaceMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowNecklaceSCAddRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowNecklaceSCAddRate", M2Share.g_Config.nUnknowNecklaceSCAddRate);
            else
                M2Share.g_Config.nUnknowNecklaceSCAddRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowNecklaceSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowNecklaceSCAddValueMaxLimit", M2Share.g_Config.nUnknowNecklaceSCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowNecklaceSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowRingACAddRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowRingACAddRate", M2Share.g_Config.nUnknowRingACAddRate);
            else
                M2Share.g_Config.nUnknowRingACAddRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowRingACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowRingACAddValueMaxLimit", M2Share.g_Config.nUnknowRingACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowRingACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowRingMACAddRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowRingMACAddRate", M2Share.g_Config.nUnknowRingMACAddRate);
            else
                M2Share.g_Config.nUnknowRingMACAddRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowRingMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowRingMACAddValueMaxLimit", M2Share.g_Config.nUnknowRingMACAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowRingMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowRingDCAddRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowRingDCAddRate", M2Share.g_Config.nUnknowRingDCAddRate);
            else
                M2Share.g_Config.nUnknowRingDCAddRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowRingDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowRingDCAddValueMaxLimit", M2Share.g_Config.nUnknowRingDCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowRingDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowRingMCAddRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowRingMCAddRate", M2Share.g_Config.nUnknowRingMCAddRate);
            else
                M2Share.g_Config.nUnknowRingMCAddRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowRingMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowRingMCAddValueMaxLimit", M2Share.g_Config.nUnknowRingMCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowRingMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowRingSCAddRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowRingSCAddRate", M2Share.g_Config.nUnknowRingSCAddRate);
            else
                M2Share.g_Config.nUnknowRingSCAddRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "UnknowRingSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UnknowRingSCAddValueMaxLimit", M2Share.g_Config.nUnknowRingSCAddValueMaxLimit);
            else
                M2Share.g_Config.nUnknowRingSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "MonOneDropGoldCount", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "MonOneDropGoldCount", M2Share.g_Config.nMonOneDropGoldCount);
            else
                M2Share.g_Config.nMonOneDropGoldCount = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "SendCurTickCount", -1);
            if (nLoadInteger < 0)
                _config.WriteBool("Setup", "SendCurTickCount", M2Share.g_Config.boSendCurTickCount);
            else
                M2Share.g_Config.boSendCurTickCount = nLoadInteger == 1;
            if (_config.ReadInteger("Setup", "MakeMineHitRate", -1) < 0)
                _config.WriteInteger("Setup", "MakeMineHitRate", M2Share.g_Config.nMakeMineHitRate);
            M2Share.g_Config.nMakeMineHitRate = _config.ReadInteger("Setup", "MakeMineHitRate", M2Share.g_Config.nMakeMineHitRate);
            if (_config.ReadInteger("Setup", "MakeMineRate", -1) < 0)
                _config.WriteInteger("Setup", "MakeMineRate", M2Share.g_Config.nMakeMineRate);
            M2Share.g_Config.nMakeMineRate = _config.ReadInteger("Setup", "MakeMineRate", M2Share.g_Config.nMakeMineRate);
            if (_config.ReadInteger("Setup", "StoneTypeRate", -1) < 0)
                _config.WriteInteger("Setup", "StoneTypeRate", M2Share.g_Config.nStoneTypeRate);
            M2Share.g_Config.nStoneTypeRate = _config.ReadInteger("Setup", "StoneTypeRate", M2Share.g_Config.nStoneTypeRate);
            if (_config.ReadInteger("Setup", "StoneTypeRateMin", -1) < 0)
                _config.WriteInteger("Setup", "StoneTypeRateMin", M2Share.g_Config.nStoneTypeRateMin);
            M2Share.g_Config.nStoneTypeRateMin = _config.ReadInteger("Setup", "StoneTypeRateMin", M2Share.g_Config.nStoneTypeRateMin);
            if (_config.ReadInteger("Setup", "GoldStoneMin", -1) < 0)
                _config.WriteInteger("Setup", "GoldStoneMin", M2Share.g_Config.nGoldStoneMin);
            M2Share.g_Config.nGoldStoneMin = _config.ReadInteger("Setup", "GoldStoneMin", M2Share.g_Config.nGoldStoneMin);
            if (_config.ReadInteger("Setup", "GoldStoneMax", -1) < 0)
                _config.WriteInteger("Setup", "GoldStoneMax", M2Share.g_Config.nGoldStoneMax);
            M2Share.g_Config.nGoldStoneMax = _config.ReadInteger("Setup", "GoldStoneMax", M2Share.g_Config.nGoldStoneMax);
            if (_config.ReadInteger("Setup", "SilverStoneMin", -1) < 0)
                _config.WriteInteger("Setup", "SilverStoneMin", M2Share.g_Config.nSilverStoneMin);
            M2Share.g_Config.nSilverStoneMin = _config.ReadInteger("Setup", "SilverStoneMin", M2Share.g_Config.nSilverStoneMin);
            if (_config.ReadInteger("Setup", "SilverStoneMax", -1) < 0)
                _config.WriteInteger("Setup", "SilverStoneMax", M2Share.g_Config.nSilverStoneMax);
            M2Share.g_Config.nSilverStoneMax = _config.ReadInteger("Setup", "SilverStoneMax", M2Share.g_Config.nSilverStoneMax);
            if (_config.ReadInteger("Setup", "SteelStoneMin", -1) < 0)
                _config.WriteInteger("Setup", "SteelStoneMin", M2Share.g_Config.nSteelStoneMin);
            M2Share.g_Config.nSteelStoneMin = _config.ReadInteger("Setup", "SteelStoneMin", M2Share.g_Config.nSteelStoneMin);
            if (_config.ReadInteger("Setup", "SteelStoneMax", -1) < 0)
                _config.WriteInteger("Setup", "SteelStoneMax", M2Share.g_Config.nSteelStoneMax);
            M2Share.g_Config.nSteelStoneMax = _config.ReadInteger("Setup", "SteelStoneMax", M2Share.g_Config.nSteelStoneMax);
            if (_config.ReadInteger("Setup", "BlackStoneMin", -1) < 0)
                _config.WriteInteger("Setup", "BlackStoneMin", M2Share.g_Config.nBlackStoneMin);
            M2Share.g_Config.nBlackStoneMin = _config.ReadInteger("Setup", "BlackStoneMin", M2Share.g_Config.nBlackStoneMin);
            if (_config.ReadInteger("Setup", "BlackStoneMax", -1) < 0)
                _config.WriteInteger("Setup", "BlackStoneMax", M2Share.g_Config.nBlackStoneMax);
            M2Share.g_Config.nBlackStoneMax = _config.ReadInteger("Setup", "BlackStoneMax", M2Share.g_Config.nBlackStoneMax);
            if (_config.ReadInteger("Setup", "StoneMinDura", -1) < 0)
                _config.WriteInteger("Setup", "StoneMinDura", M2Share.g_Config.nStoneMinDura);
            M2Share.g_Config.nStoneMinDura = _config.ReadInteger("Setup", "StoneMinDura", M2Share.g_Config.nStoneMinDura);
            if (_config.ReadInteger("Setup", "StoneGeneralDuraRate", -1) < 0)
                _config.WriteInteger("Setup", "StoneGeneralDuraRate", M2Share.g_Config.nStoneGeneralDuraRate);
            M2Share.g_Config.nStoneGeneralDuraRate = _config.ReadInteger("Setup", "StoneGeneralDuraRate", M2Share.g_Config.nStoneGeneralDuraRate);
            if (_config.ReadInteger("Setup", "StoneAddDuraRate", -1) < 0)
                _config.WriteInteger("Setup", "StoneAddDuraRate", M2Share.g_Config.nStoneAddDuraRate);
            M2Share.g_Config.nStoneAddDuraRate = _config.ReadInteger("Setup", "StoneAddDuraRate", M2Share.g_Config.nStoneAddDuraRate);
            if (_config.ReadInteger("Setup", "StoneAddDuraMax", -1) < 0)
                _config.WriteInteger("Setup", "StoneAddDuraMax", M2Share.g_Config.nStoneAddDuraMax);
            M2Share.g_Config.nStoneAddDuraMax = _config.ReadInteger("Setup", "StoneAddDuraMax", M2Share.g_Config.nStoneAddDuraMax);
            if (_config.ReadInteger("Setup", "WinLottery1Min", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery1Min", M2Share.g_Config.nWinLottery1Min);
            M2Share.g_Config.nWinLottery1Min = _config.ReadInteger("Setup", "WinLottery1Min", M2Share.g_Config.nWinLottery1Min);
            if (_config.ReadInteger("Setup", "WinLottery1Max", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery1Max", M2Share.g_Config.nWinLottery1Max);
            M2Share.g_Config.nWinLottery1Max = _config.ReadInteger("Setup", "WinLottery1Max", M2Share.g_Config.nWinLottery1Max);
            if (_config.ReadInteger("Setup", "WinLottery2Min", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery2Min", M2Share.g_Config.nWinLottery2Min);
            M2Share.g_Config.nWinLottery2Min = _config.ReadInteger("Setup", "WinLottery2Min", M2Share.g_Config.nWinLottery2Min);
            if (_config.ReadInteger("Setup", "WinLottery2Max", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery2Max", M2Share.g_Config.nWinLottery2Max);
            M2Share.g_Config.nWinLottery2Max = _config.ReadInteger("Setup", "WinLottery2Max", M2Share.g_Config.nWinLottery2Max);
            if (_config.ReadInteger("Setup", "WinLottery3Min", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery3Min", M2Share.g_Config.nWinLottery3Min);
            M2Share.g_Config.nWinLottery3Min = _config.ReadInteger("Setup", "WinLottery3Min", M2Share.g_Config.nWinLottery3Min);
            if (_config.ReadInteger("Setup", "WinLottery3Max", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery3Max", M2Share.g_Config.nWinLottery3Max);
            M2Share.g_Config.nWinLottery3Max = _config.ReadInteger("Setup", "WinLottery3Max", M2Share.g_Config.nWinLottery3Max);
            if (_config.ReadInteger("Setup", "WinLottery4Min", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery4Min", M2Share.g_Config.nWinLottery4Min);
            M2Share.g_Config.nWinLottery4Min = _config.ReadInteger("Setup", "WinLottery4Min", M2Share.g_Config.nWinLottery4Min);
            if (_config.ReadInteger("Setup", "WinLottery4Max", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery4Max", M2Share.g_Config.nWinLottery4Max);
            M2Share.g_Config.nWinLottery4Max = _config.ReadInteger("Setup", "WinLottery4Max", M2Share.g_Config.nWinLottery4Max);
            if (_config.ReadInteger("Setup", "WinLottery5Min", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery5Min", M2Share.g_Config.nWinLottery5Min);
            M2Share.g_Config.nWinLottery5Min = _config.ReadInteger("Setup", "WinLottery5Min", M2Share.g_Config.nWinLottery5Min);
            if (_config.ReadInteger("Setup", "WinLottery5Max", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery5Max", M2Share.g_Config.nWinLottery5Max);
            M2Share.g_Config.nWinLottery5Max = _config.ReadInteger("Setup", "WinLottery5Max", M2Share.g_Config.nWinLottery5Max);
            if (_config.ReadInteger("Setup", "WinLottery6Min", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery6Min", M2Share.g_Config.nWinLottery6Min);
            M2Share.g_Config.nWinLottery6Min = _config.ReadInteger("Setup", "WinLottery6Min", M2Share.g_Config.nWinLottery6Min);
            if (_config.ReadInteger("Setup", "WinLottery6Max", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery6Max", M2Share.g_Config.nWinLottery6Max);
            M2Share.g_Config.nWinLottery6Max = _config.ReadInteger("Setup", "WinLottery6Max", M2Share.g_Config.nWinLottery6Max);
            if (_config.ReadInteger("Setup", "WinLotteryRate", -1) < 0)
                _config.WriteInteger("Setup", "WinLotteryRate", M2Share.g_Config.nWinLotteryRate);
            M2Share.g_Config.nWinLotteryRate = _config.ReadInteger("Setup", "WinLotteryRate", M2Share.g_Config.nWinLotteryRate);
            if (_config.ReadInteger("Setup", "WinLottery1Gold", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery1Gold", M2Share.g_Config.nWinLottery1Gold);
            M2Share.g_Config.nWinLottery1Gold = _config.ReadInteger("Setup", "WinLottery1Gold", M2Share.g_Config.nWinLottery1Gold);
            if (_config.ReadInteger("Setup", "WinLottery2Gold", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery2Gold", M2Share.g_Config.nWinLottery2Gold);
            M2Share.g_Config.nWinLottery2Gold = _config.ReadInteger("Setup", "WinLottery2Gold", M2Share.g_Config.nWinLottery2Gold);
            if (_config.ReadInteger("Setup", "WinLottery3Gold", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery3Gold", M2Share.g_Config.nWinLottery3Gold);
            M2Share.g_Config.nWinLottery3Gold = _config.ReadInteger("Setup", "WinLottery3Gold", M2Share.g_Config.nWinLottery3Gold);
            if (_config.ReadInteger("Setup", "WinLottery4Gold", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery4Gold", M2Share.g_Config.nWinLottery4Gold);
            M2Share.g_Config.nWinLottery4Gold = _config.ReadInteger("Setup", "WinLottery4Gold", M2Share.g_Config.nWinLottery4Gold);
            if (_config.ReadInteger("Setup", "WinLottery5Gold", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery5Gold", M2Share.g_Config.nWinLottery5Gold);
            M2Share.g_Config.nWinLottery5Gold = _config.ReadInteger("Setup", "WinLottery5Gold", M2Share.g_Config.nWinLottery5Gold);
            if (_config.ReadInteger("Setup", "WinLottery6Gold", -1) < 0)
                _config.WriteInteger("Setup", "WinLottery6Gold", M2Share.g_Config.nWinLottery6Gold);
            M2Share.g_Config.nWinLottery6Gold = _config.ReadInteger("Setup", "WinLottery6Gold", M2Share.g_Config.nWinLottery6Gold);
            if (_config.ReadInteger("Setup", "GuildRecallTime", -1) < 0)
                _config.WriteInteger("Setup", "GuildRecallTime", M2Share.g_Config.nGuildRecallTime);
            M2Share.g_Config.nGuildRecallTime = _config.ReadInteger("Setup", "GuildRecallTime", M2Share.g_Config.nGuildRecallTime);
            if (_config.ReadInteger("Setup", "GroupRecallTime", -1) < 0)
                _config.WriteInteger("Setup", "GroupRecallTime", M2Share.g_Config.nGroupRecallTime);
            M2Share.g_Config.nGroupRecallTime = _config.ReadInteger("Setup", "GroupRecallTime", M2Share.g_Config.nGroupRecallTime);
            if (_config.ReadInteger("Setup", "ControlDropItem", -1) < 0)
                _config.WriteBool("Setup", "ControlDropItem", M2Share.g_Config.boControlDropItem);
            M2Share.g_Config.boControlDropItem = _config.ReadBool("Setup", "ControlDropItem", M2Share.g_Config.boControlDropItem);
            if (_config.ReadInteger("Setup", "InSafeDisableDrop", -1) < 0)
                _config.WriteBool("Setup", "InSafeDisableDrop", M2Share.g_Config.boInSafeDisableDrop);
            M2Share.g_Config.boInSafeDisableDrop = _config.ReadBool("Setup", "InSafeDisableDrop", M2Share.g_Config.boInSafeDisableDrop);
            if (_config.ReadInteger("Setup", "CanDropGold", -1) < 0)
                _config.WriteInteger("Setup", "CanDropGold", M2Share.g_Config.nCanDropGold);
            M2Share.g_Config.nCanDropGold = _config.ReadInteger("Setup", "CanDropGold", M2Share.g_Config.nCanDropGold);
            if (_config.ReadInteger("Setup", "CanDropPrice", -1) < 0)
                _config.WriteInteger("Setup", "CanDropPrice", M2Share.g_Config.nCanDropPrice);
            M2Share.g_Config.nCanDropPrice = _config.ReadInteger("Setup", "CanDropPrice", M2Share.g_Config.nCanDropPrice);
            nLoadInteger = _config.ReadInteger("Setup", "SendCustemMsg", -1);
            if (nLoadInteger < 0)
                _config.WriteBool("Setup", "SendCustemMsg", M2Share.g_Config.boSendCustemMsg);
            else
                M2Share.g_Config.boSendCustemMsg = nLoadInteger == 1;
            nLoadInteger = _config.ReadInteger("Setup", "SubkMasterSendMsg", -1);
            if (nLoadInteger < 0)
                _config.WriteBool("Setup", "SubkMasterSendMsg", M2Share.g_Config.boSubkMasterSendMsg);
            else
                M2Share.g_Config.boSubkMasterSendMsg = nLoadInteger == 1;
            if (_config.ReadInteger("Setup", "SuperRepairPriceRate", -1) < 0)
                _config.WriteInteger("Setup", "SuperRepairPriceRate", M2Share.g_Config.nSuperRepairPriceRate);
            M2Share.g_Config.nSuperRepairPriceRate = _config.ReadInteger("Setup", "SuperRepairPriceRate", M2Share.g_Config.nSuperRepairPriceRate);
            if (_config.ReadInteger("Setup", "RepairItemDecDura", -1) < 0)
                _config.WriteInteger("Setup", "RepairItemDecDura", M2Share.g_Config.nRepairItemDecDura);
            M2Share.g_Config.nRepairItemDecDura = _config.ReadInteger("Setup", "RepairItemDecDura", M2Share.g_Config.nRepairItemDecDura);
            if (_config.ReadInteger("Setup", "DieScatterBag", -1) < 0)
                _config.WriteBool("Setup", "DieScatterBag", M2Share.g_Config.boDieScatterBag);
            M2Share.g_Config.boDieScatterBag = _config.ReadBool("Setup", "DieScatterBag", M2Share.g_Config.boDieScatterBag);
            if (_config.ReadInteger("Setup", "DieScatterBagRate", -1) < 0)
                _config.WriteInteger("Setup", "DieScatterBagRate", M2Share.g_Config.nDieScatterBagRate);
            M2Share.g_Config.nDieScatterBagRate = _config.ReadInteger("Setup", "DieScatterBagRate", M2Share.g_Config.nDieScatterBagRate);
            if (_config.ReadInteger("Setup", "DieRedScatterBagAll", -1) < 0)
                _config.WriteBool("Setup", "DieRedScatterBagAll", M2Share.g_Config.boDieRedScatterBagAll);
            M2Share.g_Config.boDieRedScatterBagAll = _config.ReadBool("Setup", "DieRedScatterBagAll", M2Share.g_Config.boDieRedScatterBagAll);
            if (_config.ReadInteger("Setup", "DieDropUseItemRate", -1) < 0)
                _config.WriteInteger("Setup", "DieDropUseItemRate", M2Share.g_Config.nDieDropUseItemRate);
            M2Share.g_Config.nDieDropUseItemRate = _config.ReadInteger("Setup", "DieDropUseItemRate", M2Share.g_Config.nDieDropUseItemRate);
            if (_config.ReadInteger("Setup", "DieRedDropUseItemRate", -1) < 0)
                _config.WriteInteger("Setup", "DieRedDropUseItemRate", M2Share.g_Config.nDieRedDropUseItemRate);
            M2Share.g_Config.nDieRedDropUseItemRate = _config.ReadInteger("Setup", "DieRedDropUseItemRate", M2Share.g_Config.nDieRedDropUseItemRate);
            if (_config.ReadInteger("Setup", "DieDropGold", -1) < 0)
                _config.WriteBool("Setup", "DieDropGold", M2Share.g_Config.boDieDropGold);
            M2Share.g_Config.boDieDropGold = _config.ReadBool("Setup", "DieDropGold", M2Share.g_Config.boDieDropGold);
            if (_config.ReadInteger("Setup", "KillByHumanDropUseItem", -1) < 0)
                _config.WriteBool("Setup", "KillByHumanDropUseItem", M2Share.g_Config.boKillByHumanDropUseItem);
            M2Share.g_Config.boKillByHumanDropUseItem = _config.ReadBool("Setup", "KillByHumanDropUseItem", M2Share.g_Config.boKillByHumanDropUseItem);
            if (_config.ReadInteger("Setup", "KillByMonstDropUseItem", -1) < 0)
                _config.WriteBool("Setup", "KillByMonstDropUseItem", M2Share.g_Config.boKillByMonstDropUseItem);
            M2Share.g_Config.boKillByMonstDropUseItem = _config.ReadBool("Setup", "KillByMonstDropUseItem", M2Share.g_Config.boKillByMonstDropUseItem);
            if (_config.ReadInteger("Setup", "KickExpireHuman", -1) < 0)
                _config.WriteBool("Setup", "KickExpireHuman", M2Share.g_Config.boKickExpireHuman);
            M2Share.g_Config.boKickExpireHuman = _config.ReadBool("Setup", "KickExpireHuman", M2Share.g_Config.boKickExpireHuman);
            if (_config.ReadInteger("Setup", "GuildRankNameLen", -1) < 0)
                _config.WriteInteger("Setup", "GuildRankNameLen", M2Share.g_Config.nGuildRankNameLen);
            M2Share.g_Config.nGuildRankNameLen = _config.ReadInteger("Setup", "GuildRankNameLen", M2Share.g_Config.nGuildRankNameLen);
            if (_config.ReadInteger("Setup", "GuildNameLen", -1) < 0)
                _config.WriteInteger("Setup", "GuildNameLen", M2Share.g_Config.nGuildNameLen);
            M2Share.g_Config.nGuildNameLen = _config.ReadInteger("Setup", "GuildNameLen", M2Share.g_Config.nGuildNameLen);
            if (_config.ReadInteger("Setup", "GuildMemberMaxLimit", -1) < 0)
                _config.WriteInteger("Setup", "GuildMemberMaxLimit", M2Share.g_Config.nGuildMemberMaxLimit);
            M2Share.g_Config.nGuildMemberMaxLimit = _config.ReadInteger("Setup", "GuildMemberMaxLimit", M2Share.g_Config.nGuildMemberMaxLimit);
            if (_config.ReadInteger("Setup", "AttackPosionRate", -1) < 0)
                _config.WriteInteger("Setup", "AttackPosionRate", M2Share.g_Config.nAttackPosionRate);
            M2Share.g_Config.nAttackPosionRate = _config.ReadInteger("Setup", "AttackPosionRate", M2Share.g_Config.nAttackPosionRate);
            if (_config.ReadInteger("Setup", "AttackPosionTime", -1) < 0)
                _config.WriteInteger("Setup", "AttackPosionTime", M2Share.g_Config.nAttackPosionTime);
            M2Share.g_Config.nAttackPosionTime = _config.ReadInteger("Setup", "AttackPosionTime", M2Share.g_Config.nAttackPosionTime);
            if (_config.ReadInteger("Setup", "RevivalTime", -1) < 0)
                _config.WriteInteger("Setup", "RevivalTime", M2Share.g_Config.dwRevivalTime);
            M2Share.g_Config.dwRevivalTime = _config.ReadInteger("Setup", "RevivalTime", M2Share.g_Config.dwRevivalTime);
            nLoadInteger = _config.ReadInteger("Setup", "UserMoveCanDupObj", -1);
            if (nLoadInteger < 0)
                _config.WriteBool("Setup", "UserMoveCanDupObj", M2Share.g_Config.boUserMoveCanDupObj);
            else
                M2Share.g_Config.boUserMoveCanDupObj = nLoadInteger == 1;
            nLoadInteger = _config.ReadInteger("Setup", "UserMoveCanOnItem", -1);
            if (nLoadInteger < 0)
                _config.WriteBool("Setup", "UserMoveCanOnItem", M2Share.g_Config.boUserMoveCanOnItem);
            else
                M2Share.g_Config.boUserMoveCanOnItem = nLoadInteger == 1;
            nLoadInteger = _config.ReadInteger("Setup", "UserMoveTime", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "UserMoveTime", M2Share.g_Config.dwUserMoveTime);
            else
                M2Share.g_Config.dwUserMoveTime = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "PKDieLostExpRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "PKDieLostExpRate", M2Share.g_Config.dwPKDieLostExpRate);
            else
                M2Share.g_Config.dwPKDieLostExpRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "PKDieLostLevelRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "PKDieLostLevelRate", M2Share.g_Config.nPKDieLostLevelRate);
            else
                M2Share.g_Config.nPKDieLostLevelRate = nLoadInteger;
            if (_config.ReadInteger("Setup", "PKFlagNameColor", -1) < 0)
                _config.WriteInteger("Setup", "PKFlagNameColor", M2Share.g_Config.btPKFlagNameColor);
            M2Share.g_Config.btPKFlagNameColor = _config.ReadInteger<byte>("Setup", "PKFlagNameColor", M2Share.g_Config.btPKFlagNameColor);
            if (_config.ReadInteger("Setup", "AllyAndGuildNameColor", -1) < 0)
                _config.WriteInteger("Setup", "AllyAndGuildNameColor", M2Share.g_Config.btAllyAndGuildNameColor);
            M2Share.g_Config.btAllyAndGuildNameColor = _config.ReadInteger<byte>("Setup", "AllyAndGuildNameColor", M2Share.g_Config.btAllyAndGuildNameColor);
            if (_config.ReadInteger("Setup", "WarGuildNameColor", -1) < 0)
                _config.WriteInteger("Setup", "WarGuildNameColor", M2Share.g_Config.btWarGuildNameColor);
            M2Share.g_Config.btWarGuildNameColor = _config.ReadInteger<byte>("Setup", "WarGuildNameColor", M2Share.g_Config.btWarGuildNameColor);
            if (_config.ReadInteger("Setup", "InFreePKAreaNameColor", -1) < 0)
                _config.WriteInteger("Setup", "InFreePKAreaNameColor", M2Share.g_Config.btInFreePKAreaNameColor);
            M2Share.g_Config.btInFreePKAreaNameColor = _config.ReadInteger<byte>("Setup", "InFreePKAreaNameColor", M2Share.g_Config.btInFreePKAreaNameColor);
            if (_config.ReadInteger("Setup", "PKLevel1NameColor", -1) < 0)
                _config.WriteInteger("Setup", "PKLevel1NameColor", M2Share.g_Config.btPKLevel1NameColor);
            M2Share.g_Config.btPKLevel1NameColor = _config.ReadInteger<byte>("Setup", "PKLevel1NameColor", M2Share.g_Config.btPKLevel1NameColor);
            if (_config.ReadInteger("Setup", "PKLevel2NameColor", -1) < 0)
                _config.WriteInteger("Setup", "PKLevel2NameColor", M2Share.g_Config.btPKLevel2NameColor);
            M2Share.g_Config.btPKLevel2NameColor = _config.ReadInteger<byte>("Setup", "PKLevel2NameColor", M2Share.g_Config.btPKLevel2NameColor);
            if (_config.ReadInteger("Setup", "SpiritMutiny", -1) < 0)
                _config.WriteBool("Setup", "SpiritMutiny", M2Share.g_Config.boSpiritMutiny);
            M2Share.g_Config.boSpiritMutiny = _config.ReadBool("Setup", "SpiritMutiny", M2Share.g_Config.boSpiritMutiny);
            if (_config.ReadInteger("Setup", "SpiritMutinyTime", -1) < 0)
                _config.WriteInteger("Setup", "SpiritMutinyTime", M2Share.g_Config.dwSpiritMutinyTime);
            M2Share.g_Config.dwSpiritMutinyTime = _config.ReadInteger("Setup", "SpiritMutinyTime", M2Share.g_Config.dwSpiritMutinyTime);
            if (_config.ReadInteger("Setup", "SpiritPowerRate", -1) < 0)
                _config.WriteInteger("Setup", "SpiritPowerRate", M2Share.g_Config.nSpiritPowerRate);
            M2Share.g_Config.nSpiritPowerRate = _config.ReadInteger("Setup", "SpiritPowerRate", M2Share.g_Config.nSpiritPowerRate);
            if (_config.ReadInteger("Setup", "MasterDieMutiny", -1) < 0)
                _config.WriteBool("Setup", "MasterDieMutiny", M2Share.g_Config.boMasterDieMutiny);
            M2Share.g_Config.boMasterDieMutiny = _config.ReadBool("Setup", "MasterDieMutiny", M2Share.g_Config.boMasterDieMutiny);
            if (_config.ReadInteger("Setup", "MasterDieMutinyRate", -1) < 0)
                _config.WriteInteger("Setup", "MasterDieMutinyRate", M2Share.g_Config.nMasterDieMutinyRate);
            M2Share.g_Config.nMasterDieMutinyRate = _config.ReadInteger("Setup", "MasterDieMutinyRate", M2Share.g_Config.nMasterDieMutinyRate);
            if (_config.ReadInteger("Setup", "MasterDieMutinyPower", -1) < 0)
                _config.WriteInteger("Setup", "MasterDieMutinyPower", M2Share.g_Config.nMasterDieMutinyPower);
            M2Share.g_Config.nMasterDieMutinyPower = _config.ReadInteger("Setup", "MasterDieMutinyPower", M2Share.g_Config.nMasterDieMutinyPower);
            if (_config.ReadInteger("Setup", "MasterDieMutinyPower", -1) < 0)
                _config.WriteInteger("Setup", "MasterDieMutinyPower", M2Share.g_Config.nMasterDieMutinySpeed);
            M2Share.g_Config.nMasterDieMutinySpeed = _config.ReadInteger("Setup", "MasterDieMutinyPower", M2Share.g_Config.nMasterDieMutinySpeed);
            nLoadInteger = _config.ReadInteger("Setup", "BBMonAutoChangeColor", -1);
            if (nLoadInteger < 0)
                _config.WriteBool("Setup", "BBMonAutoChangeColor", M2Share.g_Config.boBBMonAutoChangeColor);
            else
                M2Share.g_Config.boBBMonAutoChangeColor = nLoadInteger == 1;
            nLoadInteger = _config.ReadInteger("Setup", "BBMonAutoChangeColorTime", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "BBMonAutoChangeColorTime", M2Share.g_Config.dwBBMonAutoChangeColorTime);
            else
                M2Share.g_Config.dwBBMonAutoChangeColorTime = nLoadInteger;
            if (_config.ReadInteger("Setup", "OldClientShowHiLevel", -1) < 0)
                _config.WriteBool("Setup", "OldClientShowHiLevel", M2Share.g_Config.boOldClientShowHiLevel);
            M2Share.g_Config.boOldClientShowHiLevel = _config.ReadBool("Setup", "OldClientShowHiLevel", M2Share.g_Config.boOldClientShowHiLevel);
            if (_config.ReadInteger("Setup", "ShowScriptActionMsg", -1) < 0)
                _config.WriteBool("Setup", "ShowScriptActionMsg", M2Share.g_Config.boShowScriptActionMsg);
            M2Share.g_Config.boShowScriptActionMsg = _config.ReadBool("Setup", "ShowScriptActionMsg", M2Share.g_Config.boShowScriptActionMsg);
            if (_config.ReadInteger("Setup", "RunSocketDieLoopLimit", -1) < 0)
                _config.WriteInteger("Setup", "RunSocketDieLoopLimit", M2Share.g_Config.nRunSocketDieLoopLimit);
            M2Share.g_Config.nRunSocketDieLoopLimit = _config.ReadInteger("Setup", "RunSocketDieLoopLimit", M2Share.g_Config.nRunSocketDieLoopLimit);
            if (_config.ReadInteger("Setup", "ThreadRun", -1) < 0)
                _config.WriteBool("Setup", "ThreadRun", M2Share.g_Config.boThreadRun);
            M2Share.g_Config.boThreadRun = _config.ReadBool("Setup", "ThreadRun", M2Share.g_Config.boThreadRun);
            if (_config.ReadInteger("Setup", "DeathColorEffect", -1) < 0)
                _config.WriteInteger("Setup", "DeathColorEffect", M2Share.g_Config.ClientConf.btDieColor);
            M2Share.g_Config.ClientConf.btDieColor = _config.ReadInteger<byte>("Setup", "DeathColorEffect", M2Share.g_Config.ClientConf.btDieColor);
            if (_config.ReadInteger("Setup", "ParalyCanRun", -1) < 0)
                _config.WriteBool("Setup", "ParalyCanRun", M2Share.g_Config.ClientConf.boParalyCanRun);
            M2Share.g_Config.ClientConf.boParalyCanRun = _config.ReadBool("Setup", "ParalyCanRun", M2Share.g_Config.ClientConf.boParalyCanRun);
            if (_config.ReadInteger("Setup", "ParalyCanWalk", -1) < 0)
                _config.WriteBool("Setup", "ParalyCanWalk", M2Share.g_Config.ClientConf.boParalyCanWalk);
            M2Share.g_Config.ClientConf.boParalyCanWalk = _config.ReadBool("Setup", "ParalyCanWalk", M2Share.g_Config.ClientConf.boParalyCanWalk);
            if (_config.ReadInteger("Setup", "ParalyCanHit", -1) < 0)
                _config.WriteBool("Setup", "ParalyCanHit", M2Share.g_Config.ClientConf.boParalyCanHit);
            M2Share.g_Config.ClientConf.boParalyCanHit = _config.ReadBool("Setup", "ParalyCanHit", M2Share.g_Config.ClientConf.boParalyCanHit);
            if (_config.ReadInteger("Setup", "ParalyCanSpell", -1) < 0)
                _config.WriteBool("Setup", "ParalyCanSpell", M2Share.g_Config.ClientConf.boParalyCanSpell);
            M2Share.g_Config.ClientConf.boParalyCanSpell = _config.ReadBool("Setup", "ParalyCanSpell", M2Share.g_Config.ClientConf.boParalyCanSpell);
            if (_config.ReadInteger("Setup", "ShowExceptionMsg", -1) < 0)
                _config.WriteBool("Setup", "ShowExceptionMsg", M2Share.g_Config.boShowExceptionMsg);
            M2Share.g_Config.boShowExceptionMsg = _config.ReadBool("Setup", "ShowExceptionMsg", M2Share.g_Config.boShowExceptionMsg);
            if (_config.ReadInteger("Setup", "ShowPreFixMsg", -1) < 0)
                _config.WriteBool("Setup", "ShowPreFixMsg", M2Share.g_Config.boShowPreFixMsg);
            M2Share.g_Config.boShowPreFixMsg = _config.ReadBool("Setup", "ShowPreFixMsg", M2Share.g_Config.boShowPreFixMsg);
            if (_config.ReadInteger("Setup", "MagTurnUndeadLevel", -1) < 0)
                _config.WriteInteger("Setup", "MagTurnUndeadLevel", M2Share.g_Config.nMagTurnUndeadLevel);
            M2Share.g_Config.nMagTurnUndeadLevel = _config.ReadInteger("Setup", "MagTurnUndeadLevel", M2Share.g_Config.nMagTurnUndeadLevel);
            nLoadInteger = _config.ReadInteger("Setup", "MagTammingLevel", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "MagTammingLevel", M2Share.g_Config.nMagTammingLevel);
            else
                M2Share.g_Config.nMagTammingLevel = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "MagTammingTargetLevel", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "MagTammingTargetLevel", M2Share.g_Config.nMagTammingTargetLevel);
            else
                M2Share.g_Config.nMagTammingTargetLevel = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "MagTammingTargetHPRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "MagTammingTargetHPRate", M2Share.g_Config.nMagTammingHPRate);
            else
                M2Share.g_Config.nMagTammingHPRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "MagTammingCount", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "MagTammingCount", M2Share.g_Config.nMagTammingCount);
            else
                M2Share.g_Config.nMagTammingCount = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "MabMabeHitRandRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "MabMabeHitRandRate", M2Share.g_Config.nMabMabeHitRandRate);
            else
                M2Share.g_Config.nMabMabeHitRandRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "MabMabeHitMinLvLimit", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "MabMabeHitMinLvLimit", M2Share.g_Config.nMabMabeHitMinLvLimit);
            else
                M2Share.g_Config.nMabMabeHitMinLvLimit = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "MabMabeHitSucessRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "MabMabeHitSucessRate", M2Share.g_Config.nMabMabeHitSucessRate);
            else
                M2Share.g_Config.nMabMabeHitSucessRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "MabMabeHitMabeTimeRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "MabMabeHitMabeTimeRate", M2Share.g_Config.nMabMabeHitMabeTimeRate);
            else
                M2Share.g_Config.nMabMabeHitMabeTimeRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "MagicAttackRage", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "MagicAttackRage", M2Share.g_Config.nMagicAttackRage);
            else
                M2Share.g_Config.nMagicAttackRage = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "DropItemRage", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "DropItemRage", M2Share.g_Config.nDropItemRage);
            else
                M2Share.g_Config.nDropItemRage = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "AmyOunsulPoint", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "AmyOunsulPoint", M2Share.g_Config.nAmyOunsulPoint);
            else
                M2Share.g_Config.nAmyOunsulPoint = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "DisableInSafeZoneFireCross", -1);
            if (nLoadInteger < 0)
                _config.WriteBool("Setup", "DisableInSafeZoneFireCross", M2Share.g_Config.boDisableInSafeZoneFireCross);
            else
                M2Share.g_Config.boDisableInSafeZoneFireCross = nLoadInteger == 1;
            nLoadInteger = _config.ReadInteger("Setup", "GroupMbAttackPlayObject", -1);
            if (nLoadInteger < 0)
                _config.WriteBool("Setup", "GroupMbAttackPlayObject", M2Share.g_Config.boGroupMbAttackPlayObject);
            else
                M2Share.g_Config.boGroupMbAttackPlayObject = nLoadInteger == 1;
            nLoadInteger = _config.ReadInteger("Setup", "PosionDecHealthTime", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "PosionDecHealthTime", M2Share.g_Config.dwPosionDecHealthTime);
            else
                M2Share.g_Config.dwPosionDecHealthTime = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "PosionDamagarmor", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "PosionDamagarmor", M2Share.g_Config.nPosionDamagarmor);
            else
                M2Share.g_Config.nPosionDamagarmor = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "LimitSwordLong", -1);
            if (nLoadInteger < 0)
                _config.WriteBool("Setup", "LimitSwordLong", M2Share.g_Config.boLimitSwordLong);
            else
                M2Share.g_Config.boLimitSwordLong = !(nLoadInteger == 0);
            nLoadInteger = _config.ReadInteger("Setup", "SwordLongPowerRate", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "SwordLongPowerRate", M2Share.g_Config.nSwordLongPowerRate);
            else
                M2Share.g_Config.nSwordLongPowerRate = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "FireBoomRage", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "FireBoomRage", M2Share.g_Config.nFireBoomRage);
            else
                M2Share.g_Config.nFireBoomRage = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "SnowWindRange", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "SnowWindRange", M2Share.g_Config.nSnowWindRange);
            else
                M2Share.g_Config.nSnowWindRange = nLoadInteger;
            if (_config.ReadInteger("Setup", "ElecBlizzardRange", -1) < 0)
                _config.WriteInteger("Setup", "ElecBlizzardRange", M2Share.g_Config.nElecBlizzardRange);
            M2Share.g_Config.nElecBlizzardRange = _config.ReadInteger("Setup", "ElecBlizzardRange", M2Share.g_Config.nElecBlizzardRange);
            if (_config.ReadInteger("Setup", "HumanLevelDiffer", -1) < 0)
                _config.WriteInteger("Setup", "HumanLevelDiffer", M2Share.g_Config.nHumanLevelDiffer);
            M2Share.g_Config.nHumanLevelDiffer = _config.ReadInteger("Setup", "HumanLevelDiffer", M2Share.g_Config.nHumanLevelDiffer);
            if (_config.ReadInteger("Setup", "KillHumanWinLevel", -1) < 0)
                _config.WriteBool("Setup", "KillHumanWinLevel", M2Share.g_Config.boKillHumanWinLevel);
            M2Share.g_Config.boKillHumanWinLevel = _config.ReadBool("Setup", "KillHumanWinLevel", M2Share.g_Config.boKillHumanWinLevel);
            if (_config.ReadInteger("Setup", "KilledLostLevel", -1) < 0)
                _config.WriteBool("Setup", "KilledLostLevel", M2Share.g_Config.boKilledLostLevel);
            M2Share.g_Config.boKilledLostLevel = _config.ReadBool("Setup", "KilledLostLevel", M2Share.g_Config.boKilledLostLevel);
            if (_config.ReadInteger("Setup", "KillHumanWinLevelPoint", -1) < 0)
                _config.WriteInteger("Setup", "KillHumanWinLevelPoint", M2Share.g_Config.nKillHumanWinLevel);
            M2Share.g_Config.nKillHumanWinLevel = _config.ReadInteger("Setup", "KillHumanWinLevelPoint", M2Share.g_Config.nKillHumanWinLevel);
            if (_config.ReadInteger("Setup", "KilledLostLevelPoint", -1) < 0)
                _config.WriteInteger("Setup", "KilledLostLevelPoint", M2Share.g_Config.nKilledLostLevel);
            M2Share.g_Config.nKilledLostLevel = _config.ReadInteger("Setup", "KilledLostLevelPoint", M2Share.g_Config.nKilledLostLevel);
            if (_config.ReadInteger("Setup", "KillHumanWinExp", -1) < 0)
                _config.WriteBool("Setup", "KillHumanWinExp", M2Share.g_Config.boKillHumanWinExp);
            M2Share.g_Config.boKillHumanWinExp = _config.ReadBool("Setup", "KillHumanWinExp", M2Share.g_Config.boKillHumanWinExp);
            if (_config.ReadInteger("Setup", "KilledLostExp", -1) < 0)
                _config.WriteBool("Setup", "KilledLostExp", M2Share.g_Config.boKilledLostExp);
            M2Share.g_Config.boKilledLostExp = _config.ReadBool("Setup", "KilledLostExp", M2Share.g_Config.boKilledLostExp);
            if (_config.ReadInteger("Setup", "KillHumanWinExpPoint", -1) < 0)
                _config.WriteInteger("Setup", "KillHumanWinExpPoint", M2Share.g_Config.nKillHumanWinExp);
            M2Share.g_Config.nKillHumanWinExp = _config.ReadInteger("Setup", "KillHumanWinExpPoint", M2Share.g_Config.nKillHumanWinExp);
            if (_config.ReadInteger("Setup", "KillHumanLostExpPoint", -1) < 0)
                _config.WriteInteger("Setup", "KillHumanLostExpPoint", M2Share.g_Config.nKillHumanLostExp);
            M2Share.g_Config.nKillHumanLostExp = _config.ReadInteger("Setup", "KillHumanLostExpPoint", M2Share.g_Config.nKillHumanLostExp);
            if (_config.ReadInteger("Setup", "MonsterPowerRate", -1) < 0)
                _config.WriteInteger("Setup", "MonsterPowerRate", M2Share.g_Config.nMonsterPowerRate);
            M2Share.g_Config.nMonsterPowerRate = _config.ReadInteger("Setup", "MonsterPowerRate", M2Share.g_Config.nMonsterPowerRate);
            if (_config.ReadInteger("Setup", "ItemsPowerRate", -1) < 0)
                _config.WriteInteger("Setup", "ItemsPowerRate", M2Share.g_Config.nItemsPowerRate);
            M2Share.g_Config.nItemsPowerRate = _config.ReadInteger("Setup", "ItemsPowerRate", M2Share.g_Config.nItemsPowerRate);
            if (_config.ReadInteger("Setup", "ItemsACPowerRate", -1) < 0)
                _config.WriteInteger("Setup", "ItemsACPowerRate", M2Share.g_Config.nItemsACPowerRate);
            M2Share.g_Config.nItemsACPowerRate = _config.ReadInteger("Setup", "ItemsACPowerRate", M2Share.g_Config.nItemsACPowerRate);
            if (_config.ReadInteger("Setup", "SendOnlineCount", -1) < 0)
                _config.WriteBool("Setup", "SendOnlineCount", M2Share.g_Config.boSendOnlineCount);
            M2Share.g_Config.boSendOnlineCount = _config.ReadBool("Setup", "SendOnlineCount", M2Share.g_Config.boSendOnlineCount);
            if (_config.ReadInteger("Setup", "SendOnlineCountRate", -1) < 0)
                _config.WriteInteger("Setup", "SendOnlineCountRate", M2Share.g_Config.nSendOnlineCountRate);
            M2Share.g_Config.nSendOnlineCountRate = _config.ReadInteger("Setup", "SendOnlineCountRate", M2Share.g_Config.nSendOnlineCountRate);
            if (_config.ReadInteger("Setup", "SendOnlineTime", -1) < 0)
                _config.WriteInteger("Setup", "SendOnlineTime", M2Share.g_Config.dwSendOnlineTime);
            M2Share.g_Config.dwSendOnlineTime = _config.ReadInteger("Setup", "SendOnlineTime", M2Share.g_Config.dwSendOnlineTime);
            if (_config.ReadInteger("Setup", "SaveHumanRcdTime", -1) < 0)
                _config.WriteInteger("Setup", "SaveHumanRcdTime", M2Share.g_Config.dwSaveHumanRcdTime);
            M2Share.g_Config.dwSaveHumanRcdTime = _config.ReadInteger("Setup", "SaveHumanRcdTime", M2Share.g_Config.dwSaveHumanRcdTime);
            if (_config.ReadInteger("Setup", "HumanFreeDelayTime", -1) < 0)
                _config.WriteInteger("Setup", "HumanFreeDelayTime", M2Share.g_Config.dwHumanFreeDelayTime);
            if (_config.ReadInteger("Setup", "MakeGhostTime", -1) < 0)
                _config.WriteInteger("Setup", "MakeGhostTime", M2Share.g_Config.dwMakeGhostTime);
            M2Share.g_Config.dwMakeGhostTime = _config.ReadInteger("Setup", "MakeGhostTime", M2Share.g_Config.dwMakeGhostTime);
            if (_config.ReadInteger("Setup", "ClearDropOnFloorItemTime", -1) < 0)
                _config.WriteInteger("Setup", "ClearDropOnFloorItemTime", M2Share.g_Config.dwClearDropOnFloorItemTime);
            M2Share.g_Config.dwClearDropOnFloorItemTime = _config.ReadInteger("Setup", "ClearDropOnFloorItemTime", M2Share.g_Config.dwClearDropOnFloorItemTime);
            if (_config.ReadInteger("Setup", "FloorItemCanPickUpTime", -1) < 0)
                _config.WriteInteger("Setup", "FloorItemCanPickUpTime", M2Share.g_Config.dwFloorItemCanPickUpTime);
            M2Share.g_Config.dwFloorItemCanPickUpTime = _config.ReadInteger("Setup", "FloorItemCanPickUpTime", M2Share.g_Config.dwFloorItemCanPickUpTime);
            if (_config.ReadInteger("Setup", "PasswordLockSystem", -1) < 0)
                _config.WriteBool("Setup", "PasswordLockSystem", M2Share.g_Config.boPasswordLockSystem);
            M2Share.g_Config.boPasswordLockSystem = _config.ReadBool("Setup", "PasswordLockSystem", M2Share.g_Config.boPasswordLockSystem);
            if (_config.ReadInteger("Setup", "PasswordLockDealAction", -1) < 0)
                _config.WriteBool("Setup", "PasswordLockDealAction", M2Share.g_Config.boLockDealAction);
            M2Share.g_Config.boLockDealAction = _config.ReadBool("Setup", "PasswordLockDealAction", M2Share.g_Config.boLockDealAction);
            if (_config.ReadInteger("Setup", "PasswordLockDropAction", -1) < 0)
                _config.WriteBool("Setup", "PasswordLockDropAction", M2Share.g_Config.boLockDropAction);
            M2Share.g_Config.boLockDropAction = _config.ReadBool("Setup", "PasswordLockDropAction", M2Share.g_Config.boLockDropAction);
            if (_config.ReadInteger("Setup", "PasswordLockGetBackItemAction", -1) < 0)
                _config.WriteBool("Setup", "PasswordLockGetBackItemAction", M2Share.g_Config.boLockGetBackItemAction);
            M2Share.g_Config.boLockGetBackItemAction = _config.ReadBool("Setup", "PasswordLockGetBackItemAction", M2Share.g_Config.boLockGetBackItemAction);
            if (_config.ReadInteger("Setup", "PasswordLockHumanLogin", -1) < 0)
                _config.WriteBool("Setup", "PasswordLockHumanLogin", M2Share.g_Config.boLockHumanLogin);
            M2Share.g_Config.boLockHumanLogin = _config.ReadBool("Setup", "PasswordLockHumanLogin", M2Share.g_Config.boLockHumanLogin);
            if (_config.ReadInteger("Setup", "PasswordLockWalkAction", -1) < 0)
                _config.WriteBool("Setup", "PasswordLockWalkAction", M2Share.g_Config.boLockWalkAction);
            M2Share.g_Config.boLockWalkAction = _config.ReadBool("Setup", "PasswordLockWalkAction", M2Share.g_Config.boLockWalkAction);
            if (_config.ReadInteger("Setup", "PasswordLockRunAction", -1) < 0)
                _config.WriteBool("Setup", "PasswordLockRunAction", M2Share.g_Config.boLockRunAction);
            M2Share.g_Config.boLockRunAction = _config.ReadBool("Setup", "PasswordLockRunAction", M2Share.g_Config.boLockRunAction);
            if (_config.ReadInteger("Setup", "PasswordLockHitAction", -1) < 0)
                _config.WriteBool("Setup", "PasswordLockHitAction", M2Share.g_Config.boLockHitAction);
            M2Share.g_Config.boLockHitAction = _config.ReadBool("Setup", "PasswordLockHitAction", M2Share.g_Config.boLockHitAction);
            if (_config.ReadInteger("Setup", "PasswordLockSpellAction", -1) < 0)
                _config.WriteBool("Setup", "PasswordLockSpellAction", M2Share.g_Config.boLockSpellAction);
            M2Share.g_Config.boLockSpellAction = _config.ReadBool("Setup", "PasswordLockSpellAction", M2Share.g_Config.boLockSpellAction);
            if (_config.ReadInteger("Setup", "PasswordLockSendMsgAction", -1) < 0)
                _config.WriteBool("Setup", "PasswordLockSendMsgAction", M2Share.g_Config.boLockSendMsgAction);
            M2Share.g_Config.boLockSendMsgAction = _config.ReadBool("Setup", "PasswordLockSendMsgAction", M2Share.g_Config.boLockSendMsgAction);
            if (_config.ReadInteger("Setup", "PasswordLockUserItemAction", -1) < 0)
                _config.WriteBool("Setup", "PasswordLockUserItemAction", M2Share.g_Config.boLockUserItemAction);
            M2Share.g_Config.boLockUserItemAction = _config.ReadBool("Setup", "PasswordLockUserItemAction", M2Share.g_Config.boLockUserItemAction);
            if (_config.ReadInteger("Setup", "PasswordLockInObModeAction", -1) < 0)
                _config.WriteBool("Setup", "PasswordLockInObModeAction", M2Share.g_Config.boLockInObModeAction);
            M2Share.g_Config.boLockInObModeAction = _config.ReadBool("Setup", "PasswordLockInObModeAction", M2Share.g_Config.boLockInObModeAction);
            if (_config.ReadInteger("Setup", "PasswordErrorKick", -1) < 0)
                _config.WriteBool("Setup", "PasswordErrorKick", M2Share.g_Config.boPasswordErrorKick);
            M2Share.g_Config.boPasswordErrorKick = _config.ReadBool("Setup", "PasswordErrorKick", M2Share.g_Config.boPasswordErrorKick);
            if (_config.ReadInteger("Setup", "PasswordErrorCountLock", -1) < 0)
                _config.WriteInteger("Setup", "PasswordErrorCountLock", M2Share.g_Config.nPasswordErrorCountLock);
            M2Share.g_Config.nPasswordErrorCountLock = _config.ReadInteger("Setup", "PasswordErrorCountLock", M2Share.g_Config.nPasswordErrorCountLock);
            if (_config.ReadInteger("Setup", "SoftVersionDate", -1) < 0)
                _config.WriteInteger("Setup", "SoftVersionDate", M2Share.g_Config.nSoftVersionDate);
            M2Share.g_Config.nSoftVersionDate = _config.ReadInteger("Setup", "SoftVersionDate", M2Share.g_Config.nSoftVersionDate);
            nLoadInteger = _config.ReadInteger("Setup", "CanOldClientLogon", -1);
            if (nLoadInteger < 0)
                _config.WriteBool("Setup", "CanOldClientLogon", M2Share.g_Config.boCanOldClientLogon);
            else
                M2Share.g_Config.boCanOldClientLogon = nLoadInteger == 1;
            if (_config.ReadInteger("Setup", "ConsoleShowUserCountTime", -1) < 0)
                _config.WriteInteger("Setup", "ConsoleShowUserCountTime", M2Share.g_Config.dwConsoleShowUserCountTime);
            M2Share.g_Config.dwConsoleShowUserCountTime = _config.ReadInteger("Setup", "ConsoleShowUserCountTime", M2Share.g_Config.dwConsoleShowUserCountTime);
            if (_config.ReadInteger("Setup", "ShowLineNoticeTime", -1) < 0)
                _config.WriteInteger("Setup", "ShowLineNoticeTime", M2Share.g_Config.dwShowLineNoticeTime);
            M2Share.g_Config.dwShowLineNoticeTime = _config.ReadInteger("Setup", "ShowLineNoticeTime", M2Share.g_Config.dwShowLineNoticeTime);
            if (_config.ReadInteger("Setup", "LineNoticeColor", -1) < 0)
                _config.WriteInteger("Setup", "LineNoticeColor", M2Share.g_Config.nLineNoticeColor);
            M2Share.g_Config.nLineNoticeColor = _config.ReadInteger("Setup", "LineNoticeColor", M2Share.g_Config.nLineNoticeColor);
            if (_config.ReadInteger("Setup", "ItemSpeedTime", -1) < 0)
                _config.WriteInteger("Setup", "ItemSpeedTime", M2Share.g_Config.ClientConf.btItemSpeed);
            M2Share.g_Config.ClientConf.btItemSpeed = _config.ReadInteger<byte>("Setup", "ItemSpeedTime", M2Share.g_Config.ClientConf.btItemSpeed);
            if (_config.ReadInteger("Setup", "MaxHitMsgCount", -1) < 0)
                _config.WriteInteger("Setup", "MaxHitMsgCount", M2Share.g_Config.nMaxHitMsgCount);
            M2Share.g_Config.nMaxHitMsgCount = _config.ReadInteger("Setup", "MaxHitMsgCount", M2Share.g_Config.nMaxHitMsgCount);
            if (_config.ReadInteger("Setup", "MaxSpellMsgCount", -1) < 0)
                _config.WriteInteger("Setup", "MaxSpellMsgCount", M2Share.g_Config.nMaxSpellMsgCount);
            M2Share.g_Config.nMaxSpellMsgCount = _config.ReadInteger("Setup", "MaxSpellMsgCount", M2Share.g_Config.nMaxSpellMsgCount);
            if (_config.ReadInteger("Setup", "MaxRunMsgCount", -1) < 0)
                _config.WriteInteger("Setup", "MaxRunMsgCount", M2Share.g_Config.nMaxRunMsgCount);
            M2Share.g_Config.nMaxRunMsgCount = _config.ReadInteger("Setup", "MaxRunMsgCount", M2Share.g_Config.nMaxRunMsgCount);
            if (_config.ReadInteger("Setup", "MaxWalkMsgCount", -1) < 0)
                _config.WriteInteger("Setup", "MaxWalkMsgCount", M2Share.g_Config.nMaxWalkMsgCount);
            M2Share.g_Config.nMaxWalkMsgCount = _config.ReadInteger("Setup", "MaxWalkMsgCount", M2Share.g_Config.nMaxWalkMsgCount);
            if (_config.ReadInteger("Setup", "MaxTurnMsgCount", -1) < 0)
                _config.WriteInteger("Setup", "MaxTurnMsgCount", M2Share.g_Config.nMaxTurnMsgCount);
            M2Share.g_Config.nMaxTurnMsgCount = _config.ReadInteger("Setup", "MaxTurnMsgCount", M2Share.g_Config.nMaxTurnMsgCount);
            if (_config.ReadInteger("Setup", "MaxSitDonwMsgCount", -1) < 0)
                _config.WriteInteger("Setup", "MaxSitDonwMsgCount", M2Share.g_Config.nMaxSitDonwMsgCount);
            M2Share.g_Config.nMaxSitDonwMsgCount = _config.ReadInteger("Setup", "MaxSitDonwMsgCount", M2Share.g_Config.nMaxSitDonwMsgCount);
            if (_config.ReadInteger("Setup", "MaxDigUpMsgCount", -1) < 0)
                _config.WriteInteger("Setup", "MaxDigUpMsgCount", M2Share.g_Config.nMaxDigUpMsgCount);
            M2Share.g_Config.nMaxDigUpMsgCount = _config.ReadInteger("Setup", "MaxDigUpMsgCount", M2Share.g_Config.nMaxDigUpMsgCount);
            if (_config.ReadInteger("Setup", "SpellSendUpdateMsg", -1) < 0)
                _config.WriteBool("Setup", "SpellSendUpdateMsg", M2Share.g_Config.boSpellSendUpdateMsg);
            M2Share.g_Config.boSpellSendUpdateMsg = _config.ReadBool("Setup", "SpellSendUpdateMsg", M2Share.g_Config.boSpellSendUpdateMsg);
            if (_config.ReadInteger("Setup", "ActionSendActionMsg", -1) < 0)
                _config.WriteBool("Setup", "ActionSendActionMsg", M2Share.g_Config.boActionSendActionMsg);
            M2Share.g_Config.boActionSendActionMsg = _config.ReadBool("Setup", "ActionSendActionMsg", M2Share.g_Config.boActionSendActionMsg);
            if (_config.ReadInteger("Setup", "OverSpeedKickCount", -1) < 0)
                _config.WriteInteger("Setup", "OverSpeedKickCount", M2Share.g_Config.nOverSpeedKickCount);
            M2Share.g_Config.nOverSpeedKickCount = _config.ReadInteger("Setup", "OverSpeedKickCount", M2Share.g_Config.nOverSpeedKickCount);
            if (_config.ReadInteger("Setup", "DropOverSpeed", -1) < 0)
                _config.WriteInteger("Setup", "DropOverSpeed", M2Share.g_Config.dwDropOverSpeed);
            M2Share.g_Config.dwDropOverSpeed = _config.ReadInteger("Setup", "DropOverSpeed", M2Share.g_Config.dwDropOverSpeed);
            if (_config.ReadInteger("Setup", "KickOverSpeed", -1) < 0)
                _config.WriteBool("Setup", "KickOverSpeed", M2Share.g_Config.boKickOverSpeed);
            M2Share.g_Config.boKickOverSpeed = _config.ReadBool("Setup", "KickOverSpeed", M2Share.g_Config.boKickOverSpeed);
            nLoadInteger = _config.ReadInteger("Setup", "SpeedControlMode", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "SpeedControlMode", M2Share.g_Config.btSpeedControlMode);
            else
                M2Share.g_Config.btSpeedControlMode = nLoadInteger;
            if (_config.ReadInteger("Setup", "HitIntervalTime", -1) < 0)
                _config.WriteInteger("Setup", "HitIntervalTime", M2Share.g_Config.dwHitIntervalTime);
            M2Share.g_Config.dwHitIntervalTime =
                _config.ReadInteger("Setup", "HitIntervalTime", M2Share.g_Config.dwHitIntervalTime);
            if (_config.ReadInteger("Setup", "MagicHitIntervalTime", -1) < 0)
                _config.WriteInteger("Setup", "MagicHitIntervalTime", M2Share.g_Config.dwMagicHitIntervalTime);
            M2Share.g_Config.dwMagicHitIntervalTime = _config.ReadInteger("Setup", "MagicHitIntervalTime", M2Share.g_Config.dwMagicHitIntervalTime);
            if (_config.ReadInteger("Setup", "RunIntervalTime", -1) < 0)
                _config.WriteInteger("Setup", "RunIntervalTime", M2Share.g_Config.dwRunIntervalTime);
            M2Share.g_Config.dwRunIntervalTime = _config.ReadInteger("Setup", "RunIntervalTime", M2Share.g_Config.dwRunIntervalTime);
            if (_config.ReadInteger("Setup", "WalkIntervalTime", -1) < 0)
                _config.WriteInteger("Setup", "WalkIntervalTime", M2Share.g_Config.dwWalkIntervalTime);
            M2Share.g_Config.dwWalkIntervalTime = _config.ReadInteger("Setup", "WalkIntervalTime", M2Share.g_Config.dwWalkIntervalTime);
            if (_config.ReadInteger("Setup", "TurnIntervalTime", -1) < 0)
                _config.WriteInteger("Setup", "TurnIntervalTime", M2Share.g_Config.dwTurnIntervalTime);
            M2Share.g_Config.dwTurnIntervalTime = _config.ReadInteger("Setup", "TurnIntervalTime", M2Share.g_Config.dwTurnIntervalTime);
            nLoadInteger = _config.ReadInteger("Setup", "ControlActionInterval", -1);
            if (nLoadInteger < 0)
                _config.WriteBool("Setup", "ControlActionInterval", M2Share.g_Config.boControlActionInterval);
            else
                M2Share.g_Config.boControlActionInterval = nLoadInteger == 1;
            nLoadInteger = _config.ReadInteger("Setup", "ControlWalkHit", -1);
            if (nLoadInteger < 0)
                _config.WriteBool("Setup", "ControlWalkHit", M2Share.g_Config.boControlWalkHit);
            else
                M2Share.g_Config.boControlWalkHit = nLoadInteger == 1;
            nLoadInteger = _config.ReadInteger("Setup", "ControlRunLongHit", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteBool("Setup", "ControlRunLongHit", M2Share.g_Config.boControlRunLongHit);
            }
            else
            {
                M2Share.g_Config.boControlRunLongHit = nLoadInteger == 1;
            }
            nLoadInteger = _config.ReadInteger("Setup", "ControlRunHit", -1);
            if (nLoadInteger < 0)
                _config.WriteBool("Setup", "ControlRunHit", M2Share.g_Config.boControlRunHit);
            else
                M2Share.g_Config.boControlRunHit = nLoadInteger == 1;
            nLoadInteger = _config.ReadInteger("Setup", "ControlRunMagic", -1);
            if (nLoadInteger < 0)
                _config.WriteBool("Setup", "ControlRunMagic", M2Share.g_Config.boControlRunMagic);
            else
                M2Share.g_Config.boControlRunMagic = nLoadInteger == 1;
            nLoadInteger = _config.ReadInteger("Setup", "ActionIntervalTime", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "ActionIntervalTime", M2Share.g_Config.dwActionIntervalTime);
            else
                M2Share.g_Config.dwActionIntervalTime = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "RunLongHitIntervalTime", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "RunLongHitIntervalTime", M2Share.g_Config.dwRunLongHitIntervalTime);
            else
                M2Share.g_Config.dwRunLongHitIntervalTime = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "RunHitIntervalTime", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "RunHitIntervalTime", M2Share.g_Config.dwRunHitIntervalTime);
            else
                M2Share.g_Config.dwRunHitIntervalTime = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "WalkHitIntervalTime", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "WalkHitIntervalTime", M2Share.g_Config.dwWalkHitIntervalTime);
            else
                M2Share.g_Config.dwWalkHitIntervalTime = nLoadInteger;
            nLoadInteger = _config.ReadInteger("Setup", "RunMagicIntervalTime", -1);
            if (nLoadInteger < 0)
                _config.WriteInteger("Setup", "RunMagicIntervalTime", M2Share.g_Config.dwRunMagicIntervalTime);
            else
                M2Share.g_Config.dwRunMagicIntervalTime = nLoadInteger;
            if (_config.ReadInteger("Setup", "DisableStruck", -1) < 0)
                _config.WriteBool("Setup", "DisableStruck", M2Share.g_Config.boDisableStruck);
            M2Share.g_Config.boDisableStruck =
                _config.ReadBool("Setup", "DisableStruck", M2Share.g_Config.boDisableStruck);
            if (_config.ReadInteger("Setup", "DisableSelfStruck", -1) < 0)
                _config.WriteBool("Setup", "DisableSelfStruck", M2Share.g_Config.boDisableSelfStruck);
            M2Share.g_Config.boDisableSelfStruck =
                _config.ReadBool("Setup", "DisableSelfStruck", M2Share.g_Config.boDisableSelfStruck);
            if (_config.ReadInteger("Setup", "StruckTime", -1) < 0)
                _config.WriteInteger("Setup", "StruckTime", M2Share.g_Config.dwStruckTime);
            M2Share.g_Config.dwStruckTime = _config.ReadInteger("Setup", "StruckTime", M2Share.g_Config.dwStruckTime);
            nLoadInteger = _config.ReadInteger("Setup", "AddUserItemNewValue", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteBool("Setup", "AddUserItemNewValue", M2Share.g_Config.boAddUserItemNewValue);
            }
            else
            {
                M2Share.g_Config.boAddUserItemNewValue = nLoadInteger == 1;
            }
            nLoadInteger = _config.ReadInteger("Setup", "TestSpeedMode", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteBool("Setup", "TestSpeedMode", M2Share.g_Config.boTestSpeedMode);
            }
            else
            {
                M2Share.g_Config.boTestSpeedMode = nLoadInteger == 1;
            }
            // 气血石开始
            if (_config.ReadInteger("Setup", "HPStoneStartRate", -1) < 0)
            {
                _config.WriteInteger("Setup", "HPStoneStartRate", M2Share.g_Config.HPStoneStartRate);
            }
            M2Share.g_Config.HPStoneStartRate = _config.ReadInteger<byte>("Setup", "HPStoneStartRate", M2Share.g_Config.HPStoneStartRate);
            if (_config.ReadInteger("Setup", "MPStoneStartRate", -1) < 0)
            {
                _config.WriteInteger("Setup", "MPStoneStartRate", M2Share.g_Config.MPStoneStartRate);
            }
            M2Share.g_Config.MPStoneStartRate = _config.ReadInteger<byte>("Setup", "MPStoneStartRate", M2Share.g_Config.MPStoneStartRate);
            if (_config.ReadInteger("Setup", "HPStoneIntervalTime", -1) < 0)
            {
                _config.WriteInteger("Setup", "HPStoneIntervalTime", M2Share.g_Config.HPStoneIntervalTime);
            }
            M2Share.g_Config.HPStoneIntervalTime = _config.ReadInteger("Setup", "HPStoneIntervalTime", M2Share.g_Config.HPStoneIntervalTime);
            if (_config.ReadInteger("Setup", "MPStoneIntervalTime", -1) < 0)
            {
                _config.WriteInteger("Setup", "MPStoneIntervalTime", M2Share.g_Config.MPStoneIntervalTime);
            }
            M2Share.g_Config.MPStoneIntervalTime = _config.ReadInteger("Setup", "MPStoneIntervalTime", M2Share.g_Config.MPStoneIntervalTime);
            if (_config.ReadInteger("Setup", "HPStoneAddRate", -1) < 0)
            {
                _config.WriteInteger("Setup", "HPStoneAddRate", M2Share.g_Config.HPStoneAddRate);
            }
            M2Share.g_Config.HPStoneAddRate = _config.ReadInteger<byte>("Setup", "HPStoneAddRate", M2Share.g_Config.HPStoneAddRate);
            if (_config.ReadInteger("Setup", "MPStoneAddRate", -1) < 0)
            {
                _config.WriteInteger("Setup", "MPStoneAddRate", M2Share.g_Config.MPStoneAddRate);
            }
            M2Share.g_Config.MPStoneAddRate = _config.ReadInteger<byte>("Setup", "MPStoneAddRate", M2Share.g_Config.MPStoneAddRate);
            if (_config.ReadInteger("Setup", "HPStoneDecDura", -1) < 0)
            {
                _config.WriteInteger("Setup", "HPStoneDecDura", M2Share.g_Config.HPStoneDecDura);
            }
            M2Share.g_Config.HPStoneDecDura = _config.ReadInteger("Setup", "HPStoneDecDura", M2Share.g_Config.HPStoneDecDura);
            if (_config.ReadInteger("Setup", "MPStoneDecDura", -1) < 0)
            {
                _config.WriteInteger("Setup", "MPStoneDecDura", M2Share.g_Config.MPStoneDecDura);
            }
            M2Share.g_Config.MPStoneDecDura = _config.ReadInteger("Setup", "MPStoneDecDura", M2Share.g_Config.MPStoneDecDura);

            // 气血石结束
            nLoadInteger = _config.ReadInteger("Setup", "WeaponMakeUnLuckRate", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "WeaponMakeUnLuckRate", M2Share.g_Config.nWeaponMakeUnLuckRate);
            }
            else
            {
                M2Share.g_Config.nWeaponMakeUnLuckRate = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "WeaponMakeLuckPoint1", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "WeaponMakeLuckPoint1", M2Share.g_Config.nWeaponMakeLuckPoint1);
            }
            else
            {
                M2Share.g_Config.nWeaponMakeLuckPoint1 = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "WeaponMakeLuckPoint2", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "WeaponMakeLuckPoint2", M2Share.g_Config.nWeaponMakeLuckPoint2);
            }
            else
            {
                M2Share.g_Config.nWeaponMakeLuckPoint2 = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "WeaponMakeLuckPoint3", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "WeaponMakeLuckPoint3", M2Share.g_Config.nWeaponMakeLuckPoint3);
            }
            else
            {
                M2Share.g_Config.nWeaponMakeLuckPoint3 = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "WeaponMakeLuckPoint2Rate", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "WeaponMakeLuckPoint2Rate", M2Share.g_Config.nWeaponMakeLuckPoint2Rate);
            }
            else
            {
                M2Share.g_Config.nWeaponMakeLuckPoint2Rate = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "WeaponMakeLuckPoint3Rate", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "WeaponMakeLuckPoint3Rate", M2Share.g_Config.nWeaponMakeLuckPoint3Rate);
            }
            else
            {
                M2Share.g_Config.nWeaponMakeLuckPoint3Rate = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "CheckUserItemPlace", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteBool("Setup", "CheckUserItemPlace", M2Share.g_Config.boCheckUserItemPlace);
            }
            else
            {
                M2Share.g_Config.boCheckUserItemPlace = nLoadInteger == 1;
            }
            nLoadInteger = _config.ReadInteger("Setup", "LevelValueOfTaosHP", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "LevelValueOfTaosHP", M2Share.g_Config.nLevelValueOfTaosHP);
            }
            else
            {
                M2Share.g_Config.nLevelValueOfTaosHP = nLoadInteger;
            }
            var nLoadFloatRate = _config.ReadInteger<double>("Setup", "LevelValueOfTaosHPRate", 0);
            if (nLoadFloatRate == 0)
            {
                _config.WriteInteger("Setup", "LevelValueOfTaosHPRate", M2Share.g_Config.nLevelValueOfTaosHPRate);
            }
            else
            {
                M2Share.g_Config.nLevelValueOfTaosHPRate = nLoadFloatRate;
            }
            nLoadInteger = _config.ReadInteger("Setup", "LevelValueOfTaosMP", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "LevelValueOfTaosMP", M2Share.g_Config.nLevelValueOfTaosMP);
            }
            else
            {
                M2Share.g_Config.nLevelValueOfTaosMP = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "LevelValueOfWizardHP", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "LevelValueOfWizardHP", M2Share.g_Config.nLevelValueOfWizardHP);
            }
            else
            {
                M2Share.g_Config.nLevelValueOfWizardHP = nLoadInteger;
            }
            nLoadFloatRate = _config.ReadInteger<double>("Setup", "LevelValueOfWizardHPRate", 0);
            if (nLoadFloatRate == 0)
            {
                _config.WriteInteger("Setup", "LevelValueOfWizardHPRate", M2Share.g_Config.nLevelValueOfWizardHPRate);
            }
            else
            {
                M2Share.g_Config.nLevelValueOfWizardHPRate = nLoadFloatRate;
            }
            nLoadInteger = _config.ReadInteger("Setup", "LevelValueOfWarrHP", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "LevelValueOfWarrHP", M2Share.g_Config.nLevelValueOfWarrHP);
            }
            else
            {
                M2Share.g_Config.nLevelValueOfWarrHP = nLoadInteger;
            }
            nLoadFloatRate = _config.ReadInteger<double>("Setup", "LevelValueOfWarrHPRate", 0);
            if (nLoadFloatRate == 0)
            {
                _config.WriteInteger("Setup", "LevelValueOfWarrHPRate", M2Share.g_Config.nLevelValueOfWarrHPRate);
            }
            else
            {
                M2Share.g_Config.nLevelValueOfWarrHPRate = nLoadFloatRate;
            }
            nLoadInteger = _config.ReadInteger("Setup", "ProcessMonsterInterval", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "ProcessMonsterInterval", M2Share.g_Config.nProcessMonsterInterval);
            }
            else
            {
                M2Share.g_Config.nProcessMonsterInterval = nLoadInteger;
            }
            if (_config.ReadInteger("Setup", "StartCastleWarDays", -1) < 0)
            {
                _config.WriteInteger("Setup", "StartCastleWarDays", M2Share.g_Config.nStartCastleWarDays);
            }
            M2Share.g_Config.nStartCastleWarDays = _config.ReadInteger("Setup", "StartCastleWarDays", M2Share.g_Config.nStartCastleWarDays);
            if (_config.ReadInteger("Setup", "StartCastlewarTime", -1) < 0)
            {
                _config.WriteInteger("Setup", "StartCastlewarTime", M2Share.g_Config.nStartCastlewarTime);
            }
            M2Share.g_Config.nStartCastlewarTime = _config.ReadInteger("Setup", "StartCastlewarTime", M2Share.g_Config.nStartCastlewarTime);
            if (_config.ReadInteger("Setup", "ShowCastleWarEndMsgTime", -1) < 0)
            {
                _config.WriteInteger("Setup", "ShowCastleWarEndMsgTime", M2Share.g_Config.dwShowCastleWarEndMsgTime);
            }
            M2Share.g_Config.dwShowCastleWarEndMsgTime = _config.ReadInteger("Setup", "ShowCastleWarEndMsgTime", M2Share.g_Config.dwShowCastleWarEndMsgTime);
            if (_config.ReadInteger("Server", "ClickNPCTime", -1) < 0)
            {
                _config.WriteInteger("Server", "ClickNPCTime", M2Share.g_Config.dwclickNpcTime);
            }
            M2Share.g_Config.dwclickNpcTime = _config.ReadInteger("Server", "ClickNPCTime", M2Share.g_Config.dwclickNpcTime);
            if (_config.ReadInteger("Setup", "CastleWarTime", -1) < 0)
            {
                _config.WriteInteger("Setup", "CastleWarTime", M2Share.g_Config.dwCastleWarTime);
            }
            M2Share.g_Config.dwCastleWarTime = _config.ReadInteger("Setup", "CastleWarTime", M2Share.g_Config.dwCastleWarTime);
            nLoadInteger = _config.ReadInteger("Setup", "GetCastleTime", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "GetCastleTime", M2Share.g_Config.dwGetCastleTime);
            }
            else
            {
                M2Share.g_Config.dwGetCastleTime = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "GuildWarTime", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "GuildWarTime", M2Share.g_Config.dwGuildWarTime);
            }
            else
            {
                M2Share.g_Config.dwGuildWarTime = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "WinLotteryCount", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "WinLotteryCount", M2Share.g_Config.nWinLotteryCount);
            }
            else
            {
                M2Share.g_Config.nWinLotteryCount = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "NoWinLotteryCount", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "NoWinLotteryCount", M2Share.g_Config.nNoWinLotteryCount);
            }
            else
            {
                M2Share.g_Config.nNoWinLotteryCount = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "WinLotteryLevel1", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "WinLotteryLevel1", M2Share.g_Config.nWinLotteryLevel1);
            }
            else
            {
                M2Share.g_Config.nWinLotteryLevel1 = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "WinLotteryLevel2", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "WinLotteryLevel2", M2Share.g_Config.nWinLotteryLevel2);
            }
            else
            {
                M2Share.g_Config.nWinLotteryLevel2 = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "WinLotteryLevel3", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "WinLotteryLevel3", M2Share.g_Config.nWinLotteryLevel3);
            }
            else
            {
                M2Share.g_Config.nWinLotteryLevel3 = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "WinLotteryLevel4", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "WinLotteryLevel4", M2Share.g_Config.nWinLotteryLevel4);
            }
            else
            {
                M2Share.g_Config.nWinLotteryLevel4 = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "WinLotteryLevel5", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "WinLotteryLevel5", M2Share.g_Config.nWinLotteryLevel5);
            }
            else
            {
                M2Share.g_Config.nWinLotteryLevel5 = nLoadInteger;
            }
            nLoadInteger = _config.ReadInteger("Setup", "WinLotteryLevel6", -1);
            if (nLoadInteger < 0)
            {
                _config.WriteInteger("Setup", "WinLotteryLevel6", M2Share.g_Config.nWinLotteryLevel6);
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
            _config.WriteInteger("Setup", "ItemNumber", M2Share.g_Config.nItemNumber);
            _config.WriteInteger("Setup", "ItemNumberEx", M2Share.g_Config.nItemNumberEx);
            _config.WriteInteger("Setup", "WinLotteryCount", M2Share.g_Config.nWinLotteryCount);
            _config.WriteInteger("Setup", "NoWinLotteryCount", M2Share.g_Config.nNoWinLotteryCount);
            _config.WriteInteger("Setup", "WinLotteryLevel1", M2Share.g_Config.nWinLotteryLevel1);
            _config.WriteInteger("Setup", "WinLotteryLevel2", M2Share.g_Config.nWinLotteryLevel2);
            _config.WriteInteger("Setup", "WinLotteryLevel3", M2Share.g_Config.nWinLotteryLevel3);
            _config.WriteInteger("Setup", "WinLotteryLevel4", M2Share.g_Config.nWinLotteryLevel4);
            _config.WriteInteger("Setup", "WinLotteryLevel5", M2Share.g_Config.nWinLotteryLevel5);
            _config.WriteInteger("Setup", "WinLotteryLevel6", M2Share.g_Config.nWinLotteryLevel6);
        }
    }
}