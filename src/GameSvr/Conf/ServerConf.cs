using SystemModule;
using SystemModule.Common;

namespace GameSvr.Conf
{
    public class ServerConf : IniFile
    {
        public ServerConf(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            //数据库设置
            if (ReadString("DataBase", "ConnctionString", "") == "")
            {
                WriteString("DataBase", "ConnctionString", M2Share.Config.ConnctionString);
            }
            M2Share.Config.ConnctionString = ReadString("DataBase", "ConnctionString", M2Share.Config.ConnctionString);
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
            M2Share.ServerIndex = (byte)ReadInteger("Server", "ServerIndex", M2Share.ServerIndex);
            if (ReadString("Server", "ServerName", "") == "")
            {
                WriteString("Server", "ServerName", M2Share.Config.ServerName);
            }
            if (ReadInteger("Server", "CloseCountdown", -1) < 0)
            {
                WriteInteger("Server", "CloseCountdown", M2Share.Config.CloseCountdown);
            }
            M2Share.Config.CloseCountdown = ReadInteger("Server", "CloseCountdown", M2Share.Config.CloseCountdown);
            M2Share.Config.ServerName = ReadString("Server", "ServerName", M2Share.Config.ServerName);
            if (ReadInteger("Server", "ServerNumber", -1) < 0)
                WriteInteger("Server", "ServerNumber", M2Share.Config.nServerNumber);
            M2Share.Config.nServerNumber = ReadInteger("Server", "ServerNumber", M2Share.Config.nServerNumber);
            if (ReadString("Server", "VentureServer", "") == "")
                WriteString("Server", "VentureServer", HUtil32.BoolToStr(M2Share.Config.VentureServer));
            M2Share.Config.VentureServer = string.Compare(ReadString("Server", "VentureServer", "FALSE"), "TRUE", StringComparison.OrdinalIgnoreCase) == 0;
            
            if (ReadInteger("Server", "PayMentMode", -1) == -1)
                WriteInteger("Server", "PayMentMode", M2Share.Config.PayMentMode);
            M2Share.Config.PayMentMode = (byte)ReadInteger("Server", "PayMentMode", M2Share.Config.PayMentMode);
            
            if (ReadString("Server", "TestServer", "") == "")
                WriteString("Server", "TestServer", HUtil32.BoolToStr(M2Share.Config.TestServer));
            M2Share.Config.TestServer = string.Compare(ReadString("Server", "TestServer", "FALSE"), "TRUE", StringComparison.OrdinalIgnoreCase) == 0;
            if (ReadInteger("Server", "TestLevel", -1) < 0)
                WriteInteger("Server", "TestLevel", M2Share.Config.TestLevel);
            M2Share.Config.TestLevel = ReadInteger("Server", "TestLevel", M2Share.Config.TestLevel);
            if (ReadInteger("Server", "TestGold", -1) < 0)
                WriteInteger("Server", "TestGold", M2Share.Config.TestGold);
            M2Share.Config.TestGold = ReadInteger("Server", "TestGold", M2Share.Config.TestGold);
            if (ReadInteger("Server", "TestServerUserLimit", -1) < 0)
                WriteInteger("Server", "TestServerUserLimit", M2Share.Config.TestUserLimit);
            M2Share.Config.TestUserLimit = ReadInteger("Server", "TestServerUserLimit", M2Share.Config.TestUserLimit);
            if (ReadString("Server", "ServiceMode", "") == "")
                WriteString("Server", "ServiceMode", HUtil32.BoolToStr(M2Share.Config.ServiceMode));
            M2Share.Config.ServiceMode = ReadString("Server", "ServiceMode", "FALSE").CompareTo("TRUE") == 0;
            if (ReadString("Server", "NonPKServer", "") == "")
                WriteString("Server", "NonPKServer", HUtil32.BoolToStr(M2Share.Config.PveServer));
            M2Share.Config.PveServer = ReadString("Server", "NonPKServer", "FALSE").CompareTo("TRUE") == 0;
            if (ReadString("Server", "ViewHackMessage", "") == "")
                WriteString("Server", "ViewHackMessage", HUtil32.BoolToStr(M2Share.Config.ViewHackMessage));
            M2Share.Config.ViewHackMessage = ReadString("Server", "ViewHackMessage", "FALSE").CompareTo("TRUE") == 0;
            if (ReadString("Server", "ViewAdmissionFailure", "") == "")
            {
                WriteString("Server", "ViewAdmissionFailure", HUtil32.BoolToStr(M2Share.Config.ViewAdmissionFailure));
            }
            M2Share.Config.ViewAdmissionFailure = ReadString("Server", "ViewAdmissionFailure", "FALSE").CompareTo("TRUE") == 0;
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
                WriteString("Server", "MsgSrvAddr", M2Share.Config.MsgSrvAddr);
            M2Share.Config.MsgSrvAddr = ReadString("Server", "MsgSrvAddr", M2Share.Config.MsgSrvAddr);
            if (ReadInteger("Server", "MsgSrvPort", -1) < 0)
                WriteInteger("Server", "MsgSrvPort", M2Share.Config.MsgSrvPort);
            M2Share.Config.MsgSrvPort = ReadInteger("Server", "MsgSrvPort", M2Share.Config.MsgSrvPort);
            if (ReadString("Server", "LogServerAddr", "") == "")
                WriteString("Server", "LogServerAddr", M2Share.Config.LogServerAddr);
            M2Share.Config.LogServerAddr = ReadString("Server", "LogServerAddr", M2Share.Config.LogServerAddr);
            if (ReadInteger("Server", "LogServerPort", -1) < 0)
                WriteInteger("Server", "LogServerPort", M2Share.Config.LogServerPort);
            M2Share.Config.LogServerPort = ReadInteger("Server", "LogServerPort", M2Share.Config.LogServerPort);
            if (ReadString("Server", "DiscountForNightTime", "") == "")
                WriteString("Server", "DiscountForNightTime", HUtil32.BoolToStr(M2Share.Config.DiscountForNightTime));
            M2Share.Config.DiscountForNightTime = ReadString("Server", "DiscountForNightTime", "FALSE").CompareTo("TRUE".ToLower()) == 0;
            if (ReadInteger("Server", "HalfFeeStart", -1) < 0)
                WriteInteger("Server", "HalfFeeStart", M2Share.Config.HalfFeeStart);
            M2Share.Config.HalfFeeStart = ReadInteger("Server", "HalfFeeStart", M2Share.Config.HalfFeeStart);
            if (ReadInteger("Server", "HalfFeeEnd", -1) < 0)
                WriteInteger("Server", "HalfFeeEnd", M2Share.Config.HalfFeeEnd);
            M2Share.Config.HalfFeeEnd = ReadInteger("Server", "HalfFeeEnd", M2Share.Config.HalfFeeEnd);
            if (ReadInteger("Server", "HumLimit", -1) < 0)
                WriteInteger("Server", "HumLimit", M2Share.HumLimit);
            M2Share.HumLimit = ReadInteger("Server", "HumLimit", M2Share.HumLimit);
            if (ReadInteger("Server", "MonLimit", -1) < 0)
                WriteInteger("Server", "MonLimit", M2Share.MonLimit);
            M2Share.MonLimit = ReadInteger("Server", "MonLimit", M2Share.MonLimit);
            if (ReadInteger("Server", "ZenLimit", -1) < 0)
                WriteInteger("Server", "ZenLimit", M2Share.ZenLimit);
            M2Share.ZenLimit = ReadInteger("Server", "ZenLimit", M2Share.ZenLimit);
            if (ReadInteger("Server", "NpcLimit", -1) < 0)
                WriteInteger("Server", "NpcLimit", M2Share.NpcLimit);
            M2Share.NpcLimit = ReadInteger("Server", "NpcLimit", M2Share.NpcLimit);
            if (ReadInteger("Server", "SocLimit", -1) < 0)
                WriteInteger("Server", "SocLimit", M2Share.SocLimit);
            M2Share.SocLimit = ReadInteger("Server", "SocLimit", M2Share.SocLimit);
            if (ReadInteger("Server", "DecLimit", -1) < 0)
                WriteInteger("Server", "DecLimit", M2Share.nDecLimit);
            M2Share.nDecLimit = ReadInteger("Server", "DecLimit", M2Share.nDecLimit);
            if (ReadInteger("Server", "SendBlock", -1) < 0)
                WriteInteger("Server", "SendBlock", M2Share.Config.SendBlock);
            M2Share.Config.SendBlock = ReadInteger("Server", "SendBlock", M2Share.Config.SendBlock);
            if (ReadInteger("Server", "CheckBlock", -1) < 0)
                WriteInteger("Server", "CheckBlock", M2Share.Config.CheckBlock);
            M2Share.Config.CheckBlock = ReadInteger("Server", "CheckBlock", M2Share.Config.CheckBlock);
            if (ReadInteger("Server", "SocCheckTimeOut", -1) < 0)
                WriteInteger("Server", "SocCheckTimeOut", M2Share.g_dwSocCheckTimeOut);
            M2Share.g_dwSocCheckTimeOut = ReadInteger("Server", "SocCheckTimeOut", M2Share.g_dwSocCheckTimeOut);
            if (ReadInteger("Server", "AvailableBlock", -1) < 0)
                WriteInteger("Server", "AvailableBlock", M2Share.Config.AvailableBlock);
            M2Share.Config.AvailableBlock = ReadInteger("Server", "AvailableBlock", M2Share.Config.AvailableBlock);
            if (ReadInteger("Server", "GateLoad", -1) < 0)
                WriteInteger("Server", "GateLoad", M2Share.Config.GateLoad);
            M2Share.Config.GateLoad = ReadInteger("Server", "GateLoad", M2Share.Config.GateLoad);
            if (ReadInteger("Server", "UserFull", -1) < 0)
                WriteInteger("Server", "UserFull", M2Share.Config.UserFull);
            M2Share.Config.UserFull = ReadInteger("Server", "UserFull", M2Share.Config.UserFull);
            if (ReadInteger("Server", "ZenFastStep", -1) < 0)
                WriteInteger("Server", "ZenFastStep", M2Share.Config.ZenFastStep);
            M2Share.Config.ZenFastStep = ReadInteger("Server", "ZenFastStep", M2Share.Config.ZenFastStep);
            if (ReadInteger("Server", "ProcessMonstersTime", -1) < 0)
                WriteInteger("Server", "ProcessMonstersTime", M2Share.Config.ProcessMonstersTime);
            M2Share.Config.ProcessMonstersTime = ReadInteger("Server", "ProcessMonstersTime", M2Share.Config.ProcessMonstersTime);
            if (ReadInteger("Server", "RegenMonstersTime", -1) < 0)
                WriteInteger("Server", "RegenMonstersTime", M2Share.Config.RegenMonstersTime);
            M2Share.Config.RegenMonstersTime = ReadInteger("Server", "RegenMonstersTime", M2Share.Config.RegenMonstersTime);
            if (ReadInteger("Server", "HumanGetMsgTimeLimit", -1) < 0)
                WriteInteger("Server", "HumanGetMsgTimeLimit", M2Share.Config.HumanGetMsgTime);
            M2Share.Config.HumanGetMsgTime = ReadInteger("Server", "HumanGetMsgTimeLimit", M2Share.Config.HumanGetMsgTime);
            if (ReadString("Share", "BaseDir", "") == "")
                WriteString("Share", "BaseDir", M2Share.Config.BaseDir);
            M2Share.Config.BaseDir = ReadString("Share", "BaseDir", M2Share.Config.BaseDir);
            if (ReadString("Share", "GuildDir", "") == "")
                WriteString("Share", "GuildDir", M2Share.Config.GuildDir);
            M2Share.Config.GuildDir = ReadString("Share", "GuildDir", M2Share.Config.GuildDir);
            if (ReadString("Share", "GuildFile", "") == "")
                WriteString("Share", "GuildFile", M2Share.Config.GuildFile);
            M2Share.Config.GuildFile = ReadString("Share", "GuildFile", M2Share.Config.GuildFile);
            if (ReadString("Share", "VentureDir", "") == "")
                WriteString("Share", "VentureDir", M2Share.Config.VentureDir);
            M2Share.Config.VentureDir = ReadString("Share", "VentureDir", M2Share.Config.VentureDir);
            if (ReadString("Share", "ConLogDir", "") == "")
                WriteString("Share", "ConLogDir", M2Share.Config.ConLogDir);
            M2Share.Config.ConLogDir = ReadString("Share", "ConLogDir", M2Share.Config.ConLogDir);
            if (ReadString("Share", "CastleDir", "") == "")
                WriteString("Share", "CastleDir", M2Share.Config.CastleDir);
            M2Share.Config.CastleDir = ReadString("Share", "CastleDir", M2Share.Config.CastleDir);
            if (ReadString("Share", "CastleFile", "") == "")
                WriteString("Share", "CastleFile", M2Share.Config.CastleDir + "List.txt");
            M2Share.Config.CastleFile = ReadString("Share", "CastleFile", M2Share.Config.CastleFile);
            if (ReadString("Share", "EnvirDir", "") == "")
            {
                WriteString("Share", "EnvirDir", M2Share.Config.EnvirDir);
            }
            M2Share.Config.EnvirDir = ReadString("Share", "EnvirDir", M2Share.Config.EnvirDir);
            if (ReadString("Share", "MapDir", "") == "")
                WriteString("Share", "MapDir", M2Share.Config.MapDir);
            M2Share.Config.MapDir = ReadString("Share", "MapDir", M2Share.Config.MapDir);
            if (ReadString("Share", "NoticeDir", "") == "")
            {
                WriteString("Share", "NoticeDir", M2Share.Config.NoticeDir);
            }
            M2Share.Config.NoticeDir = ReadString("Share", "NoticeDir", M2Share.Config.NoticeDir);
            string sLoadString = ReadString("Share", "LogDir", "");
            if (sLoadString == "")
            {
                WriteString("Share", "LogDir", M2Share.Config.LogDir);
            }
            else
            {
                M2Share.Config.LogDir = sLoadString;
            }
            // ============================================================================
            // 名称设置
            if (ReadString("Names", "HealSkill", "") == "")
                WriteString("Names", "HealSkill", M2Share.Config.HealSkill);
            M2Share.Config.HealSkill = ReadString("Names", "HealSkill", M2Share.Config.HealSkill);
            if (ReadString("Names", "FireBallSkill", "") == "")
                WriteString("Names", "FireBallSkill", M2Share.Config.FireBallSkill);
            M2Share.Config.FireBallSkill = ReadString("Names", "FireBallSkill", M2Share.Config.FireBallSkill);
            if (ReadString("Names", "ClothsMan", "") == "")
                WriteString("Names", "ClothsMan", M2Share.Config.ClothsMan);
            M2Share.Config.ClothsMan = ReadString("Names", "ClothsMan", M2Share.Config.ClothsMan);
            if (ReadString("Names", "ClothsWoman", "") == "")
                WriteString("Names", "ClothsWoman", M2Share.Config.ClothsWoman);
            M2Share.Config.ClothsWoman = ReadString("Names", "ClothsWoman", M2Share.Config.ClothsWoman);
            if (ReadString("Names", "WoodenSword", "") == "")
                WriteString("Names", "WoodenSword", M2Share.Config.WoodenSword);
            M2Share.Config.WoodenSword = ReadString("Names", "WoodenSword", M2Share.Config.WoodenSword);
            if (ReadString("Names", "Candle", "") == "")
                WriteString("Names", "Candle", M2Share.Config.Candle);
            M2Share.Config.Candle = ReadString("Names", "Candle", M2Share.Config.Candle);
            if (ReadString("Names", "BasicDrug", "") == "")
                WriteString("Names", "BasicDrug", M2Share.Config.BasicDrug);
            M2Share.Config.BasicDrug = ReadString("Names", "BasicDrug", M2Share.Config.BasicDrug);
            if (ReadString("Names", "GoldStone", "") == "")
                WriteString("Names", "GoldStone", M2Share.Config.GoldStone);
            M2Share.Config.GoldStone = ReadString("Names", "GoldStone", M2Share.Config.GoldStone);
            if (ReadString("Names", "SilverStone", "") == "")
                WriteString("Names", "SilverStone", M2Share.Config.SilverStone);
            M2Share.Config.SilverStone = ReadString("Names", "SilverStone", M2Share.Config.SilverStone);
            if (ReadString("Names", "SteelStone", "") == "")
                WriteString("Names", "SteelStone", M2Share.Config.SteelStone);
            M2Share.Config.SteelStone = ReadString("Names", "SteelStone", M2Share.Config.SteelStone);
            if (ReadString("Names", "CopperStone", "") == "")
                WriteString("Names", "CopperStone", M2Share.Config.CopperStone);
            M2Share.Config.CopperStone = ReadString("Names", "CopperStone", M2Share.Config.CopperStone);
            if (ReadString("Names", "BlackStone", "") == "")
                WriteString("Names", "BlackStone", M2Share.Config.BlackStone);
            M2Share.Config.BlackStone = ReadString("Names", "BlackStone", M2Share.Config.BlackStone);
            if (ReadString("Names", "Gem1Stone", "") == "")
                WriteString("Names", "Gem1Stone", M2Share.Config.GemStone1);
            M2Share.Config.GemStone1 = ReadString("Names", "Gem1Stone", M2Share.Config.GemStone1);
            if (ReadString("Names", "Gem2Stone", "") == "")
                WriteString("Names", "Gem2Stone", M2Share.Config.GemStone2);
            M2Share.Config.GemStone2 = ReadString("Names", "Gem2Stone", M2Share.Config.GemStone2);
            if (ReadString("Names", "Gem3Stone", "") == "")
                WriteString("Names", "Gem3Stone", M2Share.Config.GemStone3);
            M2Share.Config.GemStone3 = ReadString("Names", "Gem3Stone", M2Share.Config.GemStone3);
            if (ReadString("Names", "Gem4Stone", "") == "")
                WriteString("Names", "Gem4Stone", M2Share.Config.GemStone4);
            M2Share.Config.GemStone4 = ReadString("Names", "Gem4Stone", M2Share.Config.GemStone4);
            if (ReadString("Names", "Zuma1", "") == "")
                WriteString("Names", "Zuma1", M2Share.Config.Zuma[0]);
            M2Share.Config.Zuma[0] = ReadString("Names", "Zuma1", M2Share.Config.Zuma[0]);
            if (ReadString("Names", "Zuma2", "") == "")
                WriteString("Names", "Zuma2", M2Share.Config.Zuma[1]);
            M2Share.Config.Zuma[1] = ReadString("Names", "Zuma2", M2Share.Config.Zuma[1]);
            if (ReadString("Names", "Zuma3", "") == "")
                WriteString("Names", "Zuma3", M2Share.Config.Zuma[2]);
            M2Share.Config.Zuma[2] = ReadString("Names", "Zuma3", M2Share.Config.Zuma[2]);
            if (ReadString("Names", "Zuma4", "") == "")
                WriteString("Names", "Zuma4", M2Share.Config.Zuma[3]);
            M2Share.Config.Zuma[3] = ReadString("Names", "Zuma4", M2Share.Config.Zuma[3]);
            if (ReadString("Names", "Bee", "") == "")
                WriteString("Names", "Bee", M2Share.Config.Bee);
            M2Share.Config.Bee = ReadString("Names", "Bee", M2Share.Config.Bee);
            if (ReadString("Names", "Spider", "") == "")
                WriteString("Names", "Spider", M2Share.Config.Spider);
            M2Share.Config.Spider = ReadString("Names", "Spider", M2Share.Config.Spider);
            if (ReadString("Names", "WomaHorn", "") == "")
                WriteString("Names", "WomaHorn", M2Share.Config.WomaHorn);
            M2Share.Config.WomaHorn = ReadString("Names", "WomaHorn", M2Share.Config.WomaHorn);
            if (ReadString("Names", "ZumaPiece", "") == "")
                WriteString("Names", "ZumaPiece", M2Share.Config.ZumaPiece);
            M2Share.Config.ZumaPiece = ReadString("Names", "ZumaPiece", M2Share.Config.ZumaPiece);
            if (ReadString("Names", "Skeleton", "") == "")
                WriteString("Names", "Skeleton", M2Share.Config.Skeleton);
            M2Share.Config.Skeleton = ReadString("Names", "Skeleton", M2Share.Config.Skeleton);
            if (ReadString("Names", "Dragon", "") == "")
                WriteString("Names", "Dragon", M2Share.Config.Dragon);
            M2Share.Config.Dragon = ReadString("Names", "Dragon", M2Share.Config.Dragon);
            if (ReadString("Names", "Dragon1", "") == "")
                WriteString("Names", "Dragon1", M2Share.Config.Dragon1);
            M2Share.Config.Dragon1 = ReadString("Names", "Dragon1", M2Share.Config.Dragon1);
            if (ReadString("Names", "Angel", "") == "")
                WriteString("Names", "Angel", M2Share.Config.Angel);
            M2Share.Config.Angel = ReadString("Names", "Angel", M2Share.Config.Angel);
            sLoadString = ReadString("Names", "GameGold", "");
            if (sLoadString == "")
                WriteString("Share", "GameGold", M2Share.Config.GameGoldName);
            else
                M2Share.Config.GameGoldName = sLoadString;
            sLoadString = ReadString("Names", "GamePoint", "");
            if (sLoadString == "")
                WriteString("Share", "GamePoint", M2Share.Config.GamePointName);
            else
                M2Share.Config.GamePointName = sLoadString;
            sLoadString = ReadString("Names", "PayMentPointName", "");
            if (sLoadString == "")
                WriteString("Share", "PayMentPointName", M2Share.Config.PayMentPointName);
            else
                M2Share.Config.PayMentPointName = sLoadString;
            if (M2Share.Config.nAppIconCrc != 1242102148) M2Share.Config.boCheckFail = true;
            // ============================================================================
            // 游戏设置
            if (ReadInteger("Setup", "ItemNumber", -1) < 0)
                WriteInteger("Setup", "ItemNumber", M2Share.Config.ItemNumber);
            M2Share.Config.ItemNumber = ReadInteger("Setup", "ItemNumber", M2Share.Config.ItemNumber);
            M2Share.Config.ItemNumber = M2Share.Config.ItemNumber + M2Share.RandomNumber.Random(10000);
            if (ReadInteger("Setup", "ItemNumberEx", -1) < 0)
                WriteInteger("Setup", "ItemNumberEx", M2Share.Config.ItemNumberEx);
            M2Share.Config.ItemNumberEx = ReadInteger("Setup", "ItemNumberEx", M2Share.Config.ItemNumberEx);
            if (ReadString("Setup", "ClientFile1", "") == "")
                WriteString("Setup", "ClientFile1", M2Share.Config.ClientFile1);
            M2Share.Config.ClientFile1 = ReadString("Setup", "ClientFile1", M2Share.Config.ClientFile1);
            if (ReadString("Setup", "ClientFile2", "") == "")
                WriteString("Setup", "ClientFile2", M2Share.Config.ClientFile2);
            M2Share.Config.ClientFile2 = ReadString("Setup", "ClientFile2", M2Share.Config.ClientFile2);
            if (ReadString("Setup", "ClientFile3", "") == "")
                WriteString("Setup", "ClientFile3", M2Share.Config.ClientFile3);
            M2Share.Config.ClientFile3 = ReadString("Setup", "ClientFile3", M2Share.Config.ClientFile3);
            if (ReadInteger("Setup", "MonUpLvNeedKillBase", -1) < 0) 
                WriteInteger("Setup", "MonUpLvNeedKillBase", M2Share.Config.MonUpLvNeedKillBase);
            M2Share.Config.MonUpLvNeedKillBase = ReadInteger("Setup", "MonUpLvNeedKillBase", M2Share.Config.MonUpLvNeedKillBase);
            if (ReadInteger("Setup", "MonUpLvRate", -1) < 0)
            {
                WriteInteger("Setup", "MonUpLvRate", M2Share.Config.MonUpLvRate);
            }
            M2Share.Config.MonUpLvRate = ReadInteger("Setup", "MonUpLvRate", M2Share.Config.MonUpLvRate);
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
                WriteString("Setup", "HomeMap", M2Share.Config.HomeMap);
            }
            M2Share.Config.HomeMap = ReadString("Setup", "HomeMap", M2Share.Config.HomeMap);
            if (ReadInteger("Setup", "HomeX", -1) < 0)
            {
                WriteInteger("Setup", "HomeX", M2Share.Config.HomeX);
            }
            M2Share.Config.HomeX = Read<short>("Setup", "HomeX", M2Share.Config.HomeX);
            if (ReadInteger("Setup", "HomeY", -1) < 0)
            {
                WriteInteger("Setup", "HomeY", M2Share.Config.HomeY);
            }
            M2Share.Config.HomeY = Read<short>("Setup", "HomeY", M2Share.Config.HomeY);
            if (ReadString("Setup", "RedHomeMap", "") == "")
            {
                WriteString("Setup", "RedHomeMap", M2Share.Config.RedHomeMap);
            }
            M2Share.Config.RedHomeMap = ReadString("Setup", "RedHomeMap", M2Share.Config.RedHomeMap);
            if (ReadInteger("Setup", "RedHomeX", -1) < 0)
            {
                WriteInteger("Setup", "RedHomeX", M2Share.Config.RedHomeX);
            }
            M2Share.Config.RedHomeX = Read<short>("Setup", "RedHomeX", M2Share.Config.RedHomeX);
            if (ReadInteger("Setup", "RedHomeY", -1) < 0)
                WriteInteger("Setup", "RedHomeY", M2Share.Config.RedHomeY);
            M2Share.Config.RedHomeY = Read<short>("Setup", "RedHomeY", M2Share.Config.RedHomeY);
            if (ReadString("Setup", "RedDieHomeMap", "") == "")
                WriteString("Setup", "RedDieHomeMap", M2Share.Config.RedDieHomeMap);
            M2Share.Config.RedDieHomeMap = ReadString("Setup", "RedDieHomeMap", M2Share.Config.RedDieHomeMap);
            if (ReadInteger("Setup", "RedDieHomeX", -1) < 0)
                WriteInteger("Setup", "RedDieHomeX", M2Share.Config.RedDieHomeX);
            M2Share.Config.RedDieHomeX = Read<short>("Setup", "RedDieHomeX", M2Share.Config.RedDieHomeX);
            if (ReadInteger("Setup", "RedDieHomeY", -1) < 0)
                WriteInteger("Setup", "RedDieHomeY", M2Share.Config.RedDieHomeY);
            M2Share.Config.RedDieHomeY = Read<short>("Setup", "RedDieHomeY", M2Share.Config.RedDieHomeY);
            if (ReadInteger("Setup", "JobHomePointSystem", -1) < 0)
                WriteBool("Setup", "JobHomePointSystem", M2Share.Config.JobHomePoint);
            M2Share.Config.JobHomePoint = ReadBool("Setup", "JobHomePointSystem", M2Share.Config.JobHomePoint);
            if (ReadString("Setup", "WarriorHomeMap", "") == "")
                WriteString("Setup", "WarriorHomeMap", M2Share.Config.WarriorHomeMap);
            M2Share.Config.WarriorHomeMap = ReadString("Setup", "WarriorHomeMap", M2Share.Config.WarriorHomeMap);
            if (ReadInteger("Setup", "WarriorHomeX", -1) < 0)
                WriteInteger("Setup", "WarriorHomeX", M2Share.Config.WarriorHomeX);
            M2Share.Config.WarriorHomeX = Read<short>("Setup", "WarriorHomeX", M2Share.Config.WarriorHomeX);
            if (ReadInteger("Setup", "WarriorHomeY", -1) < 0)
                WriteInteger("Setup", "WarriorHomeY", M2Share.Config.WarriorHomeY);
            M2Share.Config.WarriorHomeY = Read<short>("Setup", "WarriorHomeY", M2Share.Config.WarriorHomeY);
            if (ReadString("Setup", "WizardHomeMap", "") == "")
                WriteString("Setup", "WizardHomeMap", M2Share.Config.WizardHomeMap);
            M2Share.Config.WizardHomeMap = ReadString("Setup", "WizardHomeMap", M2Share.Config.WizardHomeMap);
            if (ReadInteger("Setup", "WizardHomeX", -1) < 0)
                WriteInteger("Setup", "WizardHomeX", M2Share.Config.WizardHomeX);
            M2Share.Config.WizardHomeX = Read<short>("Setup", "WizardHomeX", M2Share.Config.WizardHomeX);
            if (ReadInteger("Setup", "WizardHomeY", -1) < 0)
                WriteInteger("Setup", "WizardHomeY", M2Share.Config.WizardHomeY);
            M2Share.Config.WizardHomeY = Read<short>("Setup", "WizardHomeY", M2Share.Config.WizardHomeY);
            if (ReadString("Setup", "TaoistHomeMap", "") == "")
                WriteString("Setup", "TaoistHomeMap", M2Share.Config.TaoistHomeMap);
            M2Share.Config.TaoistHomeMap = ReadString("Setup", "TaoistHomeMap", M2Share.Config.TaoistHomeMap);
            if (ReadInteger("Setup", "TaoistHomeX", -1) < 0)
                WriteInteger("Setup", "TaoistHomeX", M2Share.Config.TaoistHomeX);
            M2Share.Config.TaoistHomeX = Read<short>("Setup", "TaoistHomeX", M2Share.Config.TaoistHomeX);
            if (ReadInteger("Setup", "TaoistHomeY", -1) < 0)
                WriteInteger("Setup", "TaoistHomeY", M2Share.Config.TaoistHomeY);
            M2Share.Config.TaoistHomeY = Read<short>("Setup", "TaoistHomeY", M2Share.Config.TaoistHomeY);
            int nLoadInteger = ReadInteger("Setup", "HealthFillTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "HealthFillTime", M2Share.Config.HealthFillTime);
            else
                M2Share.Config.HealthFillTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "SpellFillTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "SpellFillTime", M2Share.Config.SpellFillTime);
            else
                M2Share.Config.SpellFillTime = nLoadInteger;
            if (ReadInteger("Setup", "DecPkPointTime", -1) < 0)
                WriteInteger("Setup", "DecPkPointTime", M2Share.Config.DecPkPointTime);
            M2Share.Config.DecPkPointTime = ReadInteger("Setup", "DecPkPointTime", M2Share.Config.DecPkPointTime);
            if (ReadInteger("Setup", "DecPkPointCount", -1) < 0)
                WriteInteger("Setup", "DecPkPointCount", M2Share.Config.DecPkPointCount);
            M2Share.Config.DecPkPointCount = ReadInteger("Setup", "DecPkPointCount", M2Share.Config.DecPkPointCount);
            if (ReadInteger("Setup", "PKFlagTime", -1) < 0)
                WriteInteger("Setup", "PKFlagTime", M2Share.Config.dwPKFlagTime);
            M2Share.Config.dwPKFlagTime = ReadInteger("Setup", "PKFlagTime", M2Share.Config.dwPKFlagTime);
            if (ReadInteger("Setup", "KillHumanAddPKPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanAddPKPoint", M2Share.Config.KillHumanAddPKPoint);
            M2Share.Config.KillHumanAddPKPoint = ReadInteger("Setup", "KillHumanAddPKPoint", M2Share.Config.KillHumanAddPKPoint);
            if (ReadInteger("Setup", "KillHumanDecLuckPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanDecLuckPoint", M2Share.Config.KillHumanDecLuckPoint);
            M2Share.Config.KillHumanDecLuckPoint = ReadInteger("Setup", "KillHumanDecLuckPoint", M2Share.Config.KillHumanDecLuckPoint);
            if (ReadInteger("Setup", "DecLightItemDrugTime", -1) < 0)
                WriteInteger("Setup", "DecLightItemDrugTime", M2Share.Config.DecLightItemDrugTime);
            M2Share.Config.DecLightItemDrugTime = ReadInteger("Setup", "DecLightItemDrugTime", M2Share.Config.DecLightItemDrugTime);
            if (ReadInteger("Setup", "SafeZoneSize", -1) < 0)
                WriteInteger("Setup", "SafeZoneSize", M2Share.Config.SafeZoneSize);
            M2Share.Config.SafeZoneSize =
                ReadInteger("Setup", "SafeZoneSize", M2Share.Config.SafeZoneSize);
            if (ReadInteger("Setup", "StartPointSize", -1) < 0)
                WriteInteger("Setup", "StartPointSize", M2Share.Config.StartPointSize);
            M2Share.Config.StartPointSize =
                ReadInteger("Setup", "StartPointSize", M2Share.Config.StartPointSize);
            for (var i = 0; i < M2Share.Config.ReNewNameColor.Length; i++)
            {
                if (ReadInteger("Setup", "ReNewNameColor" + i, -1) < 0)
                {
                    WriteInteger("Setup", "ReNewNameColor" + i, M2Share.Config.ReNewNameColor[i]);
                }
                M2Share.Config.ReNewNameColor[i] = Read<byte>("Setup", "ReNewNameColor" + i, M2Share.Config.ReNewNameColor[i]);
            }
            if (ReadInteger("Setup", "ReNewNameColorTime", -1) < 0)
                WriteInteger("Setup", "ReNewNameColorTime", M2Share.Config.ReNewNameColorTime);
            M2Share.Config.ReNewNameColorTime = ReadInteger("Setup", "ReNewNameColorTime", M2Share.Config.ReNewNameColorTime);
            if (ReadInteger("Setup", "ReNewChangeColor", -1) < 0)
                WriteBool("Setup", "ReNewChangeColor", M2Share.Config.ReNewChangeColor);
            M2Share.Config.ReNewChangeColor = ReadBool("Setup", "ReNewChangeColor", M2Share.Config.ReNewChangeColor);
            if (ReadInteger("Setup", "ReNewLevelClearExp", -1) < 0)
                WriteBool("Setup", "ReNewLevelClearExp", M2Share.Config.ReNewLevelClearExp);
            M2Share.Config.ReNewLevelClearExp = ReadBool("Setup", "ReNewLevelClearExp", M2Share.Config.ReNewLevelClearExp);
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
                WriteInteger("Setup", "BonusAbilofWarrX2", M2Share.Config.BonusAbilofWarr.Reserved);
            M2Share.Config.BonusAbilofWarr.Reserved = Read<byte>("Setup", "BonusAbilofWarrX2", M2Share.Config.BonusAbilofWarr.Reserved);
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
                WriteInteger("Setup", "BonusAbilofWizardX2", M2Share.Config.BonusAbilofWizard.Reserved);
            M2Share.Config.BonusAbilofWizard.Reserved = Read<byte>("Setup", "BonusAbilofWizardX2", M2Share.Config.BonusAbilofWizard.Reserved);
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
                WriteInteger("Setup", "BonusAbilofTaosX2", M2Share.Config.BonusAbilofTaos.Reserved);
            M2Share.Config.BonusAbilofTaos.Reserved = Read<byte>("Setup", "BonusAbilofTaosX2", M2Share.Config.BonusAbilofTaos.Reserved);
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
                WriteInteger("Setup", "NakedAbilofWarrX2", M2Share.Config.NakedAbilofWarr.Reserved);
            M2Share.Config.NakedAbilofWarr.Reserved = Read<byte>("Setup", "NakedAbilofWarrX2", M2Share.Config.NakedAbilofWarr.Reserved);
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
                WriteInteger("Setup", "NakedAbilofWizardX2", M2Share.Config.NakedAbilofWizard.Reserved);
            M2Share.Config.NakedAbilofWizard.Reserved = Read<byte>("Setup", "NakedAbilofWizardX2", M2Share.Config.NakedAbilofWizard.Reserved);
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
                WriteInteger("Setup", "NakedAbilofTaosX2", M2Share.Config.NakedAbilofTaos.Reserved);
            M2Share.Config.NakedAbilofTaos.Reserved = Read<byte>("Setup", "NakedAbilofTaosX2", M2Share.Config.NakedAbilofTaos.Reserved);
            if (ReadInteger("Setup", "GroupMembersMax", -1) < 0)
                WriteInteger("Setup", "GroupMembersMax", M2Share.Config.GroupMembersMax);
            M2Share.Config.GroupMembersMax = ReadInteger("Setup", "GroupMembersMax", M2Share.Config.GroupMembersMax);
            if (ReadInteger("Setup", "WarrAttackMon", -1) < 0)
                WriteInteger("Setup", "WarrAttackMon", M2Share.Config.WarrMon);
            M2Share.Config.WarrMon = ReadInteger("Setup", "WarrAttackMon", M2Share.Config.WarrMon);
            if (ReadInteger("Setup", "WizardAttackMon", -1) < 0)
                WriteInteger("Setup", "WizardAttackMon", M2Share.Config.WizardMon);
            M2Share.Config.WizardMon = ReadInteger("Setup", "WizardAttackMon", M2Share.Config.WizardMon);
            if (ReadInteger("Setup", "TaosAttackMon", -1) < 0)
                WriteInteger("Setup", "TaosAttackMon", M2Share.Config.TaosMon);
            M2Share.Config.TaosMon = ReadInteger("Setup", "TaosAttackMon", M2Share.Config.TaosMon);
            if (ReadInteger("Setup", "MonAttackHum", -1) < 0)
                WriteInteger("Setup", "MonAttackHum", M2Share.Config.MonHum);
            M2Share.Config.MonHum = ReadInteger("Setup", "MonAttackHum", M2Share.Config.MonHum);
            if (ReadInteger("Setup", "UPgradeWeaponGetBackTime", -1) < 0)
            {
                WriteInteger("Setup", "UPgradeWeaponGetBackTime", M2Share.Config.UPgradeWeaponGetBackTime);
            }
            M2Share.Config.UPgradeWeaponGetBackTime = ReadInteger("Setup", "UPgradeWeaponGetBackTime", M2Share.Config.UPgradeWeaponGetBackTime);
            if (ReadInteger("Setup", "ClearExpireUpgradeWeaponDays", -1) < 0)
            {
                WriteInteger("Setup", "ClearExpireUpgradeWeaponDays", M2Share.Config.ClearExpireUpgradeWeaponDays);
            }
            M2Share.Config.ClearExpireUpgradeWeaponDays = ReadInteger("Setup", "ClearExpireUpgradeWeaponDays", M2Share.Config.ClearExpireUpgradeWeaponDays);
            if (ReadInteger("Setup", "UpgradeWeaponPrice", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponPrice", M2Share.Config.UpgradeWeaponPrice);
            M2Share.Config.UpgradeWeaponPrice = ReadInteger("Setup", "UpgradeWeaponPrice", M2Share.Config.UpgradeWeaponPrice);
            if (ReadInteger("Setup", "UpgradeWeaponMaxPoint", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponMaxPoint", M2Share.Config.UpgradeWeaponMaxPoint);
            M2Share.Config.UpgradeWeaponMaxPoint = ReadInteger("Setup", "UpgradeWeaponMaxPoint", M2Share.Config.UpgradeWeaponMaxPoint);
            if (ReadInteger("Setup", "UpgradeWeaponDCRate", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponDCRate", M2Share.Config.UpgradeWeaponDCRate);
            M2Share.Config.UpgradeWeaponDCRate = ReadInteger("Setup", "UpgradeWeaponDCRate", M2Share.Config.UpgradeWeaponDCRate);
            if (ReadInteger("Setup", "UpgradeWeaponDCTwoPointRate", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponDCTwoPointRate", M2Share.Config.UpgradeWeaponDCTwoPointRate);
            M2Share.Config.UpgradeWeaponDCTwoPointRate = ReadInteger("Setup", "UpgradeWeaponDCTwoPointRate", M2Share.Config.UpgradeWeaponDCTwoPointRate);
            if (ReadInteger("Setup", "UpgradeWeaponDCThreePointRate", -1) < 0)
            {
                WriteInteger("Setup", "UpgradeWeaponDCThreePointRate", M2Share.Config.UpgradeWeaponDCThreePointRate);
            }
            M2Share.Config.UpgradeWeaponDCThreePointRate = ReadInteger("Setup", "UpgradeWeaponDCThreePointRate", M2Share.Config.UpgradeWeaponDCThreePointRate);
            if (ReadInteger("Setup", "UpgradeWeaponMCRate", -1) < 0)
            {
                WriteInteger("Setup", "UpgradeWeaponMCRate", M2Share.Config.UpgradeWeaponMCRate);
            }
            M2Share.Config.UpgradeWeaponMCRate = ReadInteger("Setup", "UpgradeWeaponMCRate", M2Share.Config.UpgradeWeaponMCRate);
            if (ReadInteger("Setup", "UpgradeWeaponMCTwoPointRate", -1) < 0)
            {
                WriteInteger("Setup", "UpgradeWeaponMCTwoPointRate", M2Share.Config.UpgradeWeaponMCTwoPointRate);
            }
            M2Share.Config.UpgradeWeaponMCTwoPointRate = ReadInteger("Setup", "UpgradeWeaponMCTwoPointRate", M2Share.Config.UpgradeWeaponMCTwoPointRate);
            if (ReadInteger("Setup", "UpgradeWeaponMCThreePointRate", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponMCThreePointRate", M2Share.Config.UpgradeWeaponMCThreePointRate);
            M2Share.Config.UpgradeWeaponMCThreePointRate = ReadInteger("Setup", "UpgradeWeaponMCThreePointRate", M2Share.Config.UpgradeWeaponMCThreePointRate);
            if (ReadInteger("Setup", "UpgradeWeaponSCRate", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponSCRate", M2Share.Config.UpgradeWeaponSCRate);
            M2Share.Config.UpgradeWeaponSCRate = ReadInteger("Setup", "UpgradeWeaponSCRate", M2Share.Config.UpgradeWeaponSCRate);
            if (ReadInteger("Setup", "UpgradeWeaponSCTwoPointRate", -1) < 0)
                WriteInteger("Setup", "UpgradeWeaponSCTwoPointRate", M2Share.Config.UpgradeWeaponSCTwoPointRate);
            M2Share.Config.UpgradeWeaponSCTwoPointRate = ReadInteger("Setup", "UpgradeWeaponSCTwoPointRate", M2Share.Config.UpgradeWeaponSCTwoPointRate);
            if (ReadInteger("Setup", "UpgradeWeaponSCThreePointRate", -1) < 0)
            {
                WriteInteger("Setup", "UpgradeWeaponSCThreePointRate", M2Share.Config.UpgradeWeaponSCThreePointRate);
            }
            M2Share.Config.UpgradeWeaponSCThreePointRate = ReadInteger("Setup", "UpgradeWeaponSCThreePointRate", M2Share.Config.UpgradeWeaponSCThreePointRate);
            if (ReadInteger("Setup", "BuildGuild", -1) < 0)
                WriteInteger("Setup", "BuildGuild", M2Share.Config.BuildGuildPrice);
            M2Share.Config.BuildGuildPrice = ReadInteger("Setup", "BuildGuild", M2Share.Config.BuildGuildPrice);
            if (ReadInteger("Setup", "MakeDurg", -1) < 0)
                WriteInteger("Setup", "MakeDurg", M2Share.Config.MakeDurgPrice);
            M2Share.Config.MakeDurgPrice = ReadInteger("Setup", "MakeDurg", M2Share.Config.MakeDurgPrice);
            if (ReadInteger("Setup", "GuildWarFee", -1) < 0)
                WriteInteger("Setup", "GuildWarFee", M2Share.Config.GuildWarPrice);
            M2Share.Config.GuildWarPrice = ReadInteger("Setup", "GuildWarFee", M2Share.Config.GuildWarPrice);
            if (ReadInteger("Setup", "HireGuard", -1) < 0)
                WriteInteger("Setup", "HireGuard", M2Share.Config.HireGuardPrice);
            M2Share.Config.HireGuardPrice = ReadInteger("Setup", "HireGuard", M2Share.Config.HireGuardPrice);
            if (ReadInteger("Setup", "HireArcher", -1) < 0)
                WriteInteger("Setup", "HireArcher", M2Share.Config.HireArcherPrice);
            M2Share.Config.HireArcherPrice = ReadInteger("Setup", "HireArcher", M2Share.Config.HireArcherPrice);
            if (ReadInteger("Setup", "RepairDoor", -1) < 0)
                WriteInteger("Setup", "RepairDoor", M2Share.Config.RepairDoorPrice);
            M2Share.Config.RepairDoorPrice = ReadInteger("Setup", "RepairDoor", M2Share.Config.RepairDoorPrice);
            if (ReadInteger("Setup", "RepairWall", -1) < 0)
                WriteInteger("Setup", "RepairWall", M2Share.Config.RepairWallPrice);
            M2Share.Config.RepairWallPrice = ReadInteger("Setup", "RepairWall", M2Share.Config.RepairWallPrice);
            if (ReadInteger("Setup", "CastleMemberPriceRate", -1) < 0)
                WriteInteger("Setup", "CastleMemberPriceRate", M2Share.Config.CastleMemberPriceRate);
            M2Share.Config.CastleMemberPriceRate = ReadInteger("Setup", "CastleMemberPriceRate", M2Share.Config.CastleMemberPriceRate);
            if (ReadInteger("Setup", "CastleGoldMax", -1) < 0)
                WriteInteger("Setup", "CastleGoldMax", M2Share.Config.CastleGoldMax);
            M2Share.Config.CastleGoldMax = ReadInteger("Setup", "CastleGoldMax", M2Share.Config.CastleGoldMax);
            if (ReadInteger("Setup", "CastleOneDayGold", -1) < 0)
                WriteInteger("Setup", "CastleOneDayGold", M2Share.Config.CastleOneDayGold);
            M2Share.Config.CastleOneDayGold = ReadInteger("Setup", "CastleOneDayGold", M2Share.Config.CastleOneDayGold);
            if (ReadString("Setup", "CastleName", "") == "")
                WriteString("Setup", "CastleName", M2Share.Config.CastleName);
            M2Share.Config.CastleName = ReadString("Setup", "CastleName", M2Share.Config.CastleName);
            if (ReadString("Setup", "CastleHomeMap", "") == "")
                WriteString("Setup", "CastleHomeMap", M2Share.Config.CastleHomeMap);
            M2Share.Config.CastleHomeMap = ReadString("Setup", "CastleHomeMap", M2Share.Config.CastleHomeMap);
            if (ReadInteger("Setup", "CastleHomeX", -1) < 0)
                WriteInteger("Setup", "CastleHomeX", M2Share.Config.CastleHomeX);
            M2Share.Config.CastleHomeX = ReadInteger("Setup", "CastleHomeX", M2Share.Config.CastleHomeX);
            if (ReadInteger("Setup", "CastleHomeY", -1) < 0)
                WriteInteger("Setup", "CastleHomeY", M2Share.Config.CastleHomeY);
            M2Share.Config.CastleHomeY = ReadInteger("Setup", "CastleHomeY", M2Share.Config.CastleHomeY);
            if (ReadInteger("Setup", "CastleWarRangeX", -1) < 0)
                WriteInteger("Setup", "CastleWarRangeX", M2Share.Config.CastleWarRangeX);
            M2Share.Config.CastleWarRangeX = ReadInteger("Setup", "CastleWarRangeX", M2Share.Config.CastleWarRangeX);
            if (ReadInteger("Setup", "CastleWarRangeY", -1) < 0)
                WriteInteger("Setup", "CastleWarRangeY", M2Share.Config.CastleWarRangeY);
            M2Share.Config.CastleWarRangeY = ReadInteger("Setup", "CastleWarRangeY", M2Share.Config.CastleWarRangeY);
            if (ReadInteger("Setup", "CastleTaxRate", -1) < 0)
                WriteInteger("Setup", "CastleTaxRate", M2Share.Config.CastleTaxRate);
            M2Share.Config.CastleTaxRate = ReadInteger("Setup", "CastleTaxRate", M2Share.Config.CastleTaxRate);
            if (ReadInteger("Setup", "CastleGetAllNpcTax", -1) < 0)
                WriteBool("Setup", "CastleGetAllNpcTax", M2Share.Config.GetAllNpcTax);
            M2Share.Config.GetAllNpcTax = ReadBool("Setup", "CastleGetAllNpcTax", M2Share.Config.GetAllNpcTax);
            nLoadInteger = ReadInteger("Setup", "GenMonRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "GenMonRate", M2Share.Config.MonGenRate);
            else
                M2Share.Config.MonGenRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "ProcessMonRandRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "ProcessMonRandRate", M2Share.Config.ProcessMonRandRate);
            else
                M2Share.Config.ProcessMonRandRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "ProcessMonLimitCount", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "ProcessMonLimitCount", M2Share.Config.ProcessMonLimitCount);
            else
                M2Share.Config.ProcessMonLimitCount = nLoadInteger;
            if (ReadInteger("Setup", "HumanMaxGold", -1) < 0)
                WriteInteger("Setup", "HumanMaxGold", M2Share.Config.HumanMaxGold);
            M2Share.Config.HumanMaxGold = ReadInteger("Setup", "HumanMaxGold", M2Share.Config.HumanMaxGold);
            if (ReadInteger("Setup", "HumanTryModeMaxGold", -1) < 0)
                WriteInteger("Setup", "HumanTryModeMaxGold", M2Share.Config.HumanTryModeMaxGold);
            M2Share.Config.HumanTryModeMaxGold = ReadInteger("Setup", "HumanTryModeMaxGold", M2Share.Config.HumanTryModeMaxGold);
            if (ReadInteger("Setup", "TryModeLevel", -1) < 0)
                WriteInteger("Setup", "TryModeLevel", M2Share.Config.TryModeLevel);
            M2Share.Config.TryModeLevel = ReadInteger("Setup", "TryModeLevel", M2Share.Config.TryModeLevel);
            if (ReadInteger("Setup", "TryModeUseStorage", -1) < 0)
                WriteBool("Setup", "TryModeUseStorage", M2Share.Config.TryModeUseStorage);
            M2Share.Config.TryModeUseStorage = ReadBool("Setup", "TryModeUseStorage", M2Share.Config.TryModeUseStorage);
            if (ReadInteger("Setup", "ShutRedMsgShowGMName", -1) < 0)
                WriteBool("Setup", "ShutRedMsgShowGMName", M2Share.Config.ShutRedMsgShowGMName);
            M2Share.Config.ShutRedMsgShowGMName = ReadBool("Setup", "ShutRedMsgShowGMName", M2Share.Config.ShutRedMsgShowGMName);
            if (ReadInteger("Setup", "ShowMakeItemMsg", -1) < 0)
                WriteBool("Setup", "ShowMakeItemMsg", M2Share.Config.ShowMakeItemMsg);
            M2Share.Config.ShowMakeItemMsg = ReadBool("Setup", "ShowMakeItemMsg", M2Share.Config.ShowMakeItemMsg);
            if (ReadInteger("Setup", "ShowGuildName", -1) < 0)
                WriteBool("Setup", "ShowGuildName", M2Share.Config.ShowGuildName);
            M2Share.Config.ShowGuildName = ReadBool("Setup", "ShowGuildName", M2Share.Config.ShowGuildName);
            if (ReadInteger("Setup", "ShowRankLevelName", -1) < 0)
                WriteBool("Setup", "ShowRankLevelName", M2Share.Config.ShowRankLevelName);
            M2Share.Config.ShowRankLevelName = ReadBool("Setup", "ShowRankLevelName", M2Share.Config.ShowRankLevelName);
            if (ReadInteger("Setup", "MonSayMsg", -1) < 0)
                WriteBool("Setup", "MonSayMsg", M2Share.Config.MonSayMsg);
            M2Share.Config.MonSayMsg = ReadBool("Setup", "MonSayMsg", M2Share.Config.MonSayMsg);
            if (ReadInteger("Setup", "SayMsgMaxLen", -1) < 0)
                WriteInteger("Setup", "SayMsgMaxLen", M2Share.Config.SayMsgMaxLen);
            M2Share.Config.SayMsgMaxLen = ReadInteger("Setup", "SayMsgMaxLen", M2Share.Config.SayMsgMaxLen);
            if (ReadInteger("Setup", "SayMsgTime", -1) < 0)
                WriteInteger("Setup", "SayMsgTime", M2Share.Config.SayMsgTime);
            M2Share.Config.SayMsgTime = ReadInteger("Setup", "SayMsgTime", M2Share.Config.SayMsgTime);
            if (ReadInteger("Setup", "SayMsgCount", -1) < 0)
                WriteInteger("Setup", "SayMsgCount", M2Share.Config.SayMsgCount);
            M2Share.Config.SayMsgCount = ReadInteger("Setup", "SayMsgCount", M2Share.Config.SayMsgCount);
            if (ReadInteger("Setup", "DisableSayMsgTime", -1) < 0)
                WriteInteger("Setup", "DisableSayMsgTime", M2Share.Config.DisableSayMsgTime);
            M2Share.Config.DisableSayMsgTime = ReadInteger("Setup", "DisableSayMsgTime", M2Share.Config.DisableSayMsgTime);
            if (ReadInteger("Setup", "SayRedMsgMaxLen", -1) < 0)
                WriteInteger("Setup", "SayRedMsgMaxLen", M2Share.Config.SayRedMsgMaxLen);
            M2Share.Config.SayRedMsgMaxLen = ReadInteger("Setup", "SayRedMsgMaxLen", M2Share.Config.SayRedMsgMaxLen);
            if (ReadInteger("Setup", "CanShoutMsgLevel", -1) < 0)
                WriteInteger("Setup", "CanShoutMsgLevel", M2Share.Config.CanShoutMsgLevel);
            M2Share.Config.CanShoutMsgLevel = ReadInteger("Setup", "CanShoutMsgLevel", M2Share.Config.CanShoutMsgLevel);
            if (ReadInteger("Setup", "StartPermission", -1) < 0)
                WriteInteger("Setup", "StartPermission", M2Share.Config.StartPermission);
            M2Share.Config.StartPermission = ReadInteger("Setup", "StartPermission", M2Share.Config.StartPermission);
            if (ReadInteger("Setup", "SendRefMsgRange", -1) < 0)
                WriteInteger("Setup", "SendRefMsgRange", M2Share.Config.SendRefMsgRange);
            M2Share.Config.SendRefMsgRange = (byte)ReadInteger("Setup", "SendRefMsgRange", M2Share.Config.SendRefMsgRange);
            if (ReadInteger("Setup", "DecLampDura", -1) < 0)
                WriteBool("Setup", "DecLampDura", M2Share.Config.DecLampDura);
            M2Share.Config.DecLampDura = ReadBool("Setup", "DecLampDura", M2Share.Config.DecLampDura);
            if (ReadInteger("Setup", "HungerSystem", -1) < 0)
                WriteBool("Setup", "HungerSystem", M2Share.Config.HungerSystem);
            M2Share.Config.HungerSystem = ReadBool("Setup", "HungerSystem", M2Share.Config.HungerSystem);
            if (ReadInteger("Setup", "HungerDecHP", -1) < 0)
                WriteBool("Setup", "HungerDecHP", M2Share.Config.HungerDecHP);
            M2Share.Config.HungerDecHP = ReadBool("Setup", "HungerDecHP", M2Share.Config.HungerDecHP);
            if (ReadInteger("Setup", "HungerDecPower", -1) < 0)
                WriteBool("Setup", "HungerDecPower", M2Share.Config.HungerDecPower);
            M2Share.Config.HungerDecPower = ReadBool("Setup", "HungerDecPower", M2Share.Config.HungerDecPower);
            if (ReadInteger("Setup", "DiableHumanRun", -1) < 0)
                WriteBool("Setup", "DiableHumanRun", M2Share.Config.DiableHumanRun);
            M2Share.Config.DiableHumanRun = ReadBool("Setup", "DiableHumanRun", M2Share.Config.DiableHumanRun);
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
                WriteInteger("Setup", "SkeletonCount", M2Share.Config.SkeletonCount);
            }
            M2Share.Config.SkeletonCount = ReadInteger("Setup", "SkeletonCount", M2Share.Config.SkeletonCount);
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
                WriteInteger("Setup", "DragonCount", M2Share.Config.DragonCount);
            }
            M2Share.Config.DragonCount = ReadInteger("Setup", "DragonCount", M2Share.Config.DragonCount);
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
                WriteInteger("Setup", "TryDealTime", M2Share.Config.TryDealTime);
            M2Share.Config.TryDealTime = ReadInteger("Setup", "TryDealTime", M2Share.Config.TryDealTime);
            if (ReadInteger("Setup", "DealOKTime", -1) < 0)
                WriteInteger("Setup", "DealOKTime", M2Share.Config.DealOKTime);
            M2Share.Config.DealOKTime = ReadInteger("Setup", "DealOKTime", M2Share.Config.DealOKTime);
            if (ReadInteger("Setup", "CanNotGetBackDeal", -1) < 0)
                WriteBool("Setup", "CanNotGetBackDeal", M2Share.Config.CanNotGetBackDeal);
            M2Share.Config.CanNotGetBackDeal = ReadBool("Setup", "CanNotGetBackDeal", M2Share.Config.CanNotGetBackDeal);
            if (ReadInteger("Setup", "DisableDeal", -1) < 0)
                WriteBool("Setup", "DisableDeal", M2Share.Config.DisableDeal);
            M2Share.Config.DisableDeal = ReadBool("Setup", "DisableDeal", M2Share.Config.DisableDeal);
            if (ReadInteger("Setup", "MasterOKLevel", -1) < 0)
                WriteInteger("Setup", "MasterOKLevel", M2Share.Config.MasterOKLevel);
            M2Share.Config.MasterOKLevel = ReadInteger("Setup", "MasterOKLevel", M2Share.Config.MasterOKLevel);
            if (ReadInteger("Setup", "MasterOKCreditPoint", -1) < 0)
                WriteInteger("Setup", "MasterOKCreditPoint", M2Share.Config.MasterOKCreditPoint);
            M2Share.Config.MasterOKCreditPoint = ReadInteger("Setup", "MasterOKCreditPoint", M2Share.Config.MasterOKCreditPoint);
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
                WriteInteger("Setup", "ItemPowerRate", M2Share.Config.ItemPowerRate);
            M2Share.Config.ItemPowerRate = ReadInteger("Setup", "ItemPowerRate", M2Share.Config.ItemPowerRate);
            if (ReadInteger("Setup", "ItemExpRate", -1) < 0)
                WriteInteger("Setup", "ItemExpRate", M2Share.Config.ItemExpRate);
            M2Share.Config.ItemExpRate = ReadInteger("Setup", "ItemExpRate", M2Share.Config.ItemExpRate);
            if (ReadInteger("Setup", "ScriptGotoCountLimit", -1) < 0)
                WriteInteger("Setup", "ScriptGotoCountLimit", M2Share.Config.ScriptGotoCountLimit);
            M2Share.Config.ScriptGotoCountLimit = ReadInteger("Setup", "ScriptGotoCountLimit", M2Share.Config.ScriptGotoCountLimit);
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
                WriteInteger("Setup", "CryMsgFColor", M2Share.Config.CryMsgFColor);
            M2Share.Config.CryMsgFColor = Read<byte>("Setup", "CryMsgFColor", M2Share.Config.CryMsgFColor);
            if (ReadInteger("Setup", "CryMsgBColor", -1) < 0)
                WriteInteger("Setup", "CryMsgBColor", M2Share.Config.CryMsgBColor);
            M2Share.Config.CryMsgBColor = Read<byte>("Setup", "CryMsgBColor", M2Share.Config.CryMsgBColor);
            if (ReadInteger("Setup", "GreenMsgFColor", -1) < 0)
                WriteInteger("Setup", "GreenMsgFColor", M2Share.Config.GreenMsgFColor);
            M2Share.Config.GreenMsgFColor = Read<byte>("Setup", "GreenMsgFColor", M2Share.Config.GreenMsgFColor);
            if (ReadInteger("Setup", "GreenMsgBColor", -1) < 0)
                WriteInteger("Setup", "GreenMsgBColor", M2Share.Config.GreenMsgBColor);
            M2Share.Config.GreenMsgBColor = Read<byte>("Setup", "GreenMsgBColor", M2Share.Config.GreenMsgBColor);
            if (ReadInteger("Setup", "BlueMsgFColor", -1) < 0)
                WriteInteger("Setup", "BlueMsgFColor", M2Share.Config.BlueMsgFColor);
            M2Share.Config.BlueMsgFColor = Read<byte>("Setup", "BlueMsgFColor", M2Share.Config.BlueMsgFColor);
            if (ReadInteger("Setup", "BlueMsgBColor", -1) < 0)
                WriteInteger("Setup", "BlueMsgBColor", M2Share.Config.BlueMsgBColor);
            M2Share.Config.BlueMsgBColor = Read<byte>("Setup", "BlueMsgBColor", M2Share.Config.BlueMsgBColor);
            if (ReadInteger("Setup", "RedMsgFColor", -1) < 0)
                WriteInteger("Setup", "RedMsgFColor", M2Share.Config.RedMsgFColor);
            M2Share.Config.RedMsgFColor = Read<byte>("Setup", "RedMsgFColor", M2Share.Config.RedMsgFColor);
            if (ReadInteger("Setup", "RedMsgBColor", -1) < 0)
                WriteInteger("Setup", "RedMsgBColor", M2Share.Config.RedMsgBColor);
            M2Share.Config.RedMsgBColor = Read<byte>("Setup", "RedMsgBColor", M2Share.Config.RedMsgBColor);
            if (ReadInteger("Setup", "GuildMsgFColor", -1) < 0)
                WriteInteger("Setup", "GuildMsgFColor", M2Share.Config.GuildMsgFColor);
            M2Share.Config.GuildMsgFColor = Read<byte>("Setup", "GuildMsgFColor", M2Share.Config.GuildMsgFColor);
            if (ReadInteger("Setup", "GuildMsgBColor", -1) < 0)
                WriteInteger("Setup", "GuildMsgBColor", M2Share.Config.GuildMsgBColor);
            M2Share.Config.GuildMsgBColor = Read<byte>("Setup", "GuildMsgBColor", M2Share.Config.GuildMsgBColor);
            if (ReadInteger("Setup", "GroupMsgFColor", -1) < 0)
                WriteInteger("Setup", "GroupMsgFColor", M2Share.Config.GroupMsgFColor);
            M2Share.Config.GroupMsgFColor = Read<byte>("Setup", "GroupMsgFColor", M2Share.Config.GroupMsgFColor);
            if (ReadInteger("Setup", "GroupMsgBColor", -1) < 0)
                WriteInteger("Setup", "GroupMsgBColor", M2Share.Config.GroupMsgBColor);
            M2Share.Config.GroupMsgBColor = Read<byte>("Setup", "GroupMsgBColor", M2Share.Config.GroupMsgBColor);
            if (ReadInteger("Setup", "CustMsgFColor", -1) < 0)
                WriteInteger("Setup", "CustMsgFColor", M2Share.Config.CustMsgFColor);
            M2Share.Config.CustMsgFColor = Read<byte>("Setup", "CustMsgFColor", M2Share.Config.CustMsgFColor);
            if (ReadInteger("Setup", "CustMsgBColor", -1) < 0)
                WriteInteger("Setup", "CustMsgBColor", M2Share.Config.CustMsgBColor);
            M2Share.Config.CustMsgBColor = Read<byte>("Setup", "CustMsgBColor", M2Share.Config.CustMsgBColor);
            if (ReadInteger("Setup", "MonRandomAddValue", -1) < 0)
                WriteInteger("Setup", "MonRandomAddValue", M2Share.Config.MonRandomAddValue);
            M2Share.Config.MonRandomAddValue = ReadInteger("Setup", "MonRandomAddValue", M2Share.Config.MonRandomAddValue);
            if (ReadInteger("Setup", "MakeRandomAddValue", -1) < 0)
                WriteInteger("Setup", "MakeRandomAddValue", M2Share.Config.MakeRandomAddValue);
            M2Share.Config.MakeRandomAddValue = ReadInteger("Setup", "MakeRandomAddValue", M2Share.Config.MakeRandomAddValue);
            if (ReadInteger("Setup", "WeaponDCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "WeaponDCAddValueMaxLimit", M2Share.Config.WeaponDCAddValueMaxLimit);
            M2Share.Config.WeaponDCAddValueMaxLimit = ReadInteger("Setup", "WeaponDCAddValueMaxLimit", M2Share.Config.WeaponDCAddValueMaxLimit);
            if (ReadInteger("Setup", "WeaponDCAddValueRate", -1) < 0)
                WriteInteger("Setup", "WeaponDCAddValueRate", M2Share.Config.WeaponDCAddValueRate);
            M2Share.Config.WeaponDCAddValueRate = ReadInteger("Setup", "WeaponDCAddValueRate", M2Share.Config.WeaponDCAddValueRate);
            if (ReadInteger("Setup", "WeaponMCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "WeaponMCAddValueMaxLimit", M2Share.Config.WeaponMCAddValueMaxLimit);
            M2Share.Config.WeaponMCAddValueMaxLimit = ReadInteger("Setup", "WeaponMCAddValueMaxLimit", M2Share.Config.WeaponMCAddValueMaxLimit);
            if (ReadInteger("Setup", "WeaponMCAddValueRate", -1) < 0)
                WriteInteger("Setup", "WeaponMCAddValueRate", M2Share.Config.WeaponMCAddValueRate);
            M2Share.Config.WeaponMCAddValueRate = ReadInteger("Setup", "WeaponMCAddValueRate", M2Share.Config.WeaponMCAddValueRate);
            if (ReadInteger("Setup", "WeaponSCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "WeaponSCAddValueMaxLimit", M2Share.Config.WeaponSCAddValueMaxLimit);
            M2Share.Config.WeaponSCAddValueMaxLimit = ReadInteger("Setup", "WeaponSCAddValueMaxLimit", M2Share.Config.WeaponSCAddValueMaxLimit);
            if (ReadInteger("Setup", "WeaponSCAddValueRate", -1) < 0)
                WriteInteger("Setup", "WeaponSCAddValueRate", M2Share.Config.WeaponSCAddValueRate);
            M2Share.Config.WeaponSCAddValueRate = ReadInteger("Setup", "WeaponSCAddValueRate", M2Share.Config.WeaponSCAddValueRate);
            if (ReadInteger("Setup", "DressDCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "DressDCAddValueMaxLimit", M2Share.Config.DressDCAddValueMaxLimit);
            M2Share.Config.DressDCAddValueMaxLimit = ReadInteger("Setup", "DressDCAddValueMaxLimit", M2Share.Config.DressDCAddValueMaxLimit);
            if (ReadInteger("Setup", "DressDCAddValueRate", -1) < 0)
                WriteInteger("Setup", "DressDCAddValueRate", M2Share.Config.DressDCAddValueRate);
            M2Share.Config.DressDCAddValueRate = ReadInteger("Setup", "DressDCAddValueRate", M2Share.Config.DressDCAddValueRate);
            if (ReadInteger("Setup", "DressDCAddRate", -1) < 0)
                WriteInteger("Setup", "DressDCAddRate", M2Share.Config.DressDCAddRate);
            M2Share.Config.DressDCAddRate = ReadInteger("Setup", "DressDCAddRate", M2Share.Config.DressDCAddRate);
            if (ReadInteger("Setup", "DressMCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "DressMCAddValueMaxLimit", M2Share.Config.DressMCAddValueMaxLimit);
            M2Share.Config.DressMCAddValueMaxLimit = ReadInteger("Setup", "DressMCAddValueMaxLimit", M2Share.Config.DressMCAddValueMaxLimit);
            if (ReadInteger("Setup", "DressMCAddValueRate", -1) < 0)
                WriteInteger("Setup", "DressMCAddValueRate", M2Share.Config.DressMCAddValueRate);
            M2Share.Config.DressMCAddValueRate = ReadInteger("Setup", "DressMCAddValueRate", M2Share.Config.DressMCAddValueRate);
            if (ReadInteger("Setup", "DressMCAddRate", -1) < 0)
                WriteInteger("Setup", "DressMCAddRate", M2Share.Config.DressMCAddRate);
            M2Share.Config.DressMCAddRate = ReadInteger("Setup", "DressMCAddRate", M2Share.Config.DressMCAddRate);
            if (ReadInteger("Setup", "DressSCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "DressSCAddValueMaxLimit", M2Share.Config.DressSCAddValueMaxLimit);
            M2Share.Config.DressSCAddValueMaxLimit = ReadInteger("Setup", "DressSCAddValueMaxLimit", M2Share.Config.DressSCAddValueMaxLimit);
            if (ReadInteger("Setup", "DressSCAddValueRate", -1) < 0)
                WriteInteger("Setup", "DressSCAddValueRate", M2Share.Config.nDressSCAddValueRate);
            M2Share.Config.nDressSCAddValueRate = ReadInteger("Setup", "DressSCAddValueRate", M2Share.Config.nDressSCAddValueRate);
            if (ReadInteger("Setup", "DressSCAddRate", -1) < 0)
                WriteInteger("Setup", "DressSCAddRate", M2Share.Config.DressSCAddRate);
            M2Share.Config.DressSCAddRate = ReadInteger("Setup", "DressSCAddRate", M2Share.Config.DressSCAddRate);
            if (ReadInteger("Setup", "NeckLace19DCAddValueMaxLimit", -1) < 0)
            {
                WriteInteger("Setup", "NeckLace19DCAddValueMaxLimit", M2Share.Config.NeckLace19DCAddValueMaxLimit);
            }
            M2Share.Config.NeckLace19DCAddValueMaxLimit = ReadInteger("Setup", "NeckLace19DCAddValueMaxLimit", M2Share.Config.NeckLace19DCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace19DCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19DCAddValueRate", M2Share.Config.NeckLace19DCAddValueRate);
            M2Share.Config.NeckLace19DCAddValueRate = ReadInteger("Setup", "NeckLace19DCAddValueRate", M2Share.Config.NeckLace19DCAddValueRate);
            if (ReadInteger("Setup", "NeckLace19DCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19DCAddRate", M2Share.Config.NeckLace19DCAddRate);
            M2Share.Config.NeckLace19DCAddRate = ReadInteger("Setup", "NeckLace19DCAddRate", M2Share.Config.NeckLace19DCAddRate);
            if (ReadInteger("Setup", "NeckLace19MCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "NeckLace19MCAddValueMaxLimit", M2Share.Config.NeckLace19MCAddValueMaxLimit);
            M2Share.Config.NeckLace19MCAddValueMaxLimit = ReadInteger("Setup", "NeckLace19MCAddValueMaxLimit", M2Share.Config.NeckLace19MCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace19MCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19MCAddValueRate", M2Share.Config.NeckLace19MCAddValueRate);
            M2Share.Config.NeckLace19MCAddValueRate = ReadInteger("Setup", "NeckLace19MCAddValueRate", M2Share.Config.NeckLace19MCAddValueRate);
            if (ReadInteger("Setup", "NeckLace19MCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19MCAddRate", M2Share.Config.NeckLace19MCAddRate);
            M2Share.Config.NeckLace19MCAddRate = ReadInteger("Setup", "NeckLace19MCAddRate", M2Share.Config.NeckLace19MCAddRate);
            if (ReadInteger("Setup", "NeckLace19SCAddValueMaxLimit", -1) < 0)
            {
                WriteInteger("Setup", "NeckLace19SCAddValueMaxLimit", M2Share.Config.NeckLace19SCAddValueMaxLimit);
            }
            M2Share.Config.NeckLace19SCAddValueMaxLimit = ReadInteger("Setup", "NeckLace19SCAddValueMaxLimit", M2Share.Config.NeckLace19SCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace19SCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19SCAddValueRate", M2Share.Config.NeckLace19SCAddValueRate);
            M2Share.Config.NeckLace19SCAddValueRate = ReadInteger("Setup", "NeckLace19SCAddValueRate", M2Share.Config.NeckLace19SCAddValueRate);
            if (ReadInteger("Setup", "NeckLace19SCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace19SCAddRate", M2Share.Config.NeckLace19SCAddRate);
            M2Share.Config.NeckLace19SCAddRate = ReadInteger("Setup", "NeckLace19SCAddRate", M2Share.Config.NeckLace19SCAddRate);
            if (ReadInteger("Setup", "NeckLace202124DCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "NeckLace202124DCAddValueMaxLimit", M2Share.Config.NeckLace202124DCAddValueMaxLimit);
            M2Share.Config.NeckLace202124DCAddValueMaxLimit = ReadInteger("Setup", "NeckLace202124DCAddValueMaxLimit", M2Share.Config.NeckLace202124DCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace202124DCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124DCAddValueRate", M2Share.Config.NeckLace202124DCAddValueRate);
            M2Share.Config.NeckLace202124DCAddValueRate = ReadInteger("Setup", "NeckLace202124DCAddValueRate", M2Share.Config.NeckLace202124DCAddValueRate);
            if (ReadInteger("Setup", "NeckLace202124DCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124DCAddRate", M2Share.Config.NeckLace202124DCAddRate);
            M2Share.Config.NeckLace202124DCAddRate = ReadInteger("Setup", "NeckLace202124DCAddRate", M2Share.Config.NeckLace202124DCAddRate);
            if (ReadInteger("Setup", "NeckLace202124MCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "NeckLace202124MCAddValueMaxLimit", M2Share.Config.NeckLace202124MCAddValueMaxLimit);
            M2Share.Config.NeckLace202124MCAddValueMaxLimit = ReadInteger("Setup", "NeckLace202124MCAddValueMaxLimit", M2Share.Config.NeckLace202124MCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace202124MCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124MCAddValueRate", M2Share.Config.NeckLace202124MCAddValueRate);
            M2Share.Config.NeckLace202124MCAddValueRate = ReadInteger("Setup", "NeckLace202124MCAddValueRate", M2Share.Config.NeckLace202124MCAddValueRate);
            if (ReadInteger("Setup", "NeckLace202124MCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124MCAddRate", M2Share.Config.NeckLace202124MCAddRate);
            M2Share.Config.NeckLace202124MCAddRate = ReadInteger("Setup", "NeckLace202124MCAddRate", M2Share.Config.NeckLace202124MCAddRate);
            if (ReadInteger("Setup", "NeckLace202124SCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "NeckLace202124SCAddValueMaxLimit", M2Share.Config.NeckLace202124SCAddValueMaxLimit);
            M2Share.Config.NeckLace202124SCAddValueMaxLimit = ReadInteger("Setup", "NeckLace202124SCAddValueMaxLimit", M2Share.Config.NeckLace202124SCAddValueMaxLimit);
            if (ReadInteger("Setup", "NeckLace202124SCAddValueRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124SCAddValueRate", M2Share.Config.NeckLace202124SCAddValueRate);
            M2Share.Config.NeckLace202124SCAddValueRate = ReadInteger("Setup", "NeckLace202124SCAddValueRate", M2Share.Config.NeckLace202124SCAddValueRate);
            if (ReadInteger("Setup", "NeckLace202124SCAddRate", -1) < 0)
                WriteInteger("Setup", "NeckLace202124SCAddRate", M2Share.Config.NeckLace202124SCAddRate);
            M2Share.Config.NeckLace202124SCAddRate = ReadInteger("Setup", "NeckLace202124SCAddRate", M2Share.Config.NeckLace202124SCAddRate);
            if (ReadInteger("Setup", "ArmRing26DCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "ArmRing26DCAddValueMaxLimit", M2Share.Config.ArmRing26DCAddValueMaxLimit);
            M2Share.Config.ArmRing26DCAddValueMaxLimit = ReadInteger("Setup", "ArmRing26DCAddValueMaxLimit", M2Share.Config.ArmRing26DCAddValueMaxLimit);
            if (ReadInteger("Setup", "ArmRing26DCAddValueRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26DCAddValueRate", M2Share.Config.ArmRing26DCAddValueRate);
            M2Share.Config.ArmRing26DCAddValueRate = ReadInteger("Setup", "ArmRing26DCAddValueRate", M2Share.Config.ArmRing26DCAddValueRate);
            if (ReadInteger("Setup", "ArmRing26DCAddRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26DCAddRate", M2Share.Config.ArmRing26DCAddRate);
            M2Share.Config.ArmRing26DCAddRate = ReadInteger("Setup", "ArmRing26DCAddRate", M2Share.Config.ArmRing26DCAddRate);
            if (ReadInteger("Setup", "ArmRing26MCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "ArmRing26MCAddValueMaxLimit", M2Share.Config.ArmRing26MCAddValueMaxLimit);
            M2Share.Config.ArmRing26MCAddValueMaxLimit = ReadInteger("Setup", "ArmRing26MCAddValueMaxLimit", M2Share.Config.ArmRing26MCAddValueMaxLimit);
            if (ReadInteger("Setup", "ArmRing26MCAddValueRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26MCAddValueRate", M2Share.Config.ArmRing26MCAddValueRate);
            M2Share.Config.ArmRing26MCAddValueRate = ReadInteger("Setup", "ArmRing26MCAddValueRate", M2Share.Config.ArmRing26MCAddValueRate);
            if (ReadInteger("Setup", "ArmRing26MCAddRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26MCAddRate", M2Share.Config.ArmRing26MCAddRate);
            M2Share.Config.ArmRing26MCAddRate = ReadInteger("Setup", "ArmRing26MCAddRate", M2Share.Config.ArmRing26MCAddRate);
            if (ReadInteger("Setup", "ArmRing26SCAddValueMaxLimit", -1) < 0)
            {
                WriteInteger("Setup", "ArmRing26SCAddValueMaxLimit", M2Share.Config.ArmRing26SCAddValueMaxLimit);
            }
            M2Share.Config.ArmRing26SCAddValueMaxLimit = ReadInteger("Setup", "ArmRing26SCAddValueMaxLimit", M2Share.Config.ArmRing26SCAddValueMaxLimit);
            if (ReadInteger("Setup", "ArmRing26SCAddValueRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26SCAddValueRate", M2Share.Config.ArmRing26SCAddValueRate);
            M2Share.Config.ArmRing26SCAddValueRate = ReadInteger("Setup", "ArmRing26SCAddValueRate", M2Share.Config.ArmRing26SCAddValueRate);
            if (ReadInteger("Setup", "ArmRing26SCAddRate", -1) < 0)
                WriteInteger("Setup", "ArmRing26SCAddRate", M2Share.Config.ArmRing26SCAddRate);
            M2Share.Config.ArmRing26SCAddRate = ReadInteger("Setup", "ArmRing26SCAddRate", M2Share.Config.ArmRing26SCAddRate);
            if (ReadInteger("Setup", "Ring22DCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring22DCAddValueMaxLimit", M2Share.Config.Ring22DCAddValueMaxLimit);
            M2Share.Config.Ring22DCAddValueMaxLimit = ReadInteger("Setup", "Ring22DCAddValueMaxLimit", M2Share.Config.Ring22DCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring22DCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring22DCAddValueRate", M2Share.Config.Ring22DCAddValueRate);
            M2Share.Config.Ring22DCAddValueRate = ReadInteger("Setup", "Ring22DCAddValueRate", M2Share.Config.Ring22DCAddValueRate);
            if (ReadInteger("Setup", "Ring22DCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring22DCAddRate", M2Share.Config.Ring22DCAddRate);
            M2Share.Config.Ring22DCAddRate = ReadInteger("Setup", "Ring22DCAddRate", M2Share.Config.Ring22DCAddRate);
            if (ReadInteger("Setup", "Ring22MCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring22MCAddValueMaxLimit", M2Share.Config.Ring22MCAddValueMaxLimit);
            M2Share.Config.Ring22MCAddValueMaxLimit = ReadInteger("Setup", "Ring22MCAddValueMaxLimit", M2Share.Config.Ring22MCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring22MCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring22MCAddValueRate", M2Share.Config.Ring22MCAddValueRate);
            M2Share.Config.Ring22MCAddValueRate = ReadInteger("Setup", "Ring22MCAddValueRate", M2Share.Config.Ring22MCAddValueRate);
            if (ReadInteger("Setup", "Ring22MCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring22MCAddRate", M2Share.Config.Ring22MCAddRate);
            M2Share.Config.Ring22MCAddRate = ReadInteger("Setup", "Ring22MCAddRate", M2Share.Config.Ring22MCAddRate);
            if (ReadInteger("Setup", "Ring22SCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring22SCAddValueMaxLimit", M2Share.Config.Ring22SCAddValueMaxLimit);
            M2Share.Config.Ring22SCAddValueMaxLimit = ReadInteger("Setup", "Ring22SCAddValueMaxLimit", M2Share.Config.Ring22SCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring22SCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring22SCAddValueRate", M2Share.Config.Ring22SCAddValueRate);
            M2Share.Config.Ring22SCAddValueRate = ReadInteger("Setup", "Ring22SCAddValueRate", M2Share.Config.Ring22SCAddValueRate);
            if (ReadInteger("Setup", "Ring22SCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring22SCAddRate", M2Share.Config.Ring22SCAddRate);
            M2Share.Config.Ring22SCAddRate = ReadInteger("Setup", "Ring22SCAddRate", M2Share.Config.Ring22SCAddRate);
            if (ReadInteger("Setup", "Ring23DCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring23DCAddValueMaxLimit", M2Share.Config.Ring23DCAddValueMaxLimit);
            M2Share.Config.Ring23DCAddValueMaxLimit = ReadInteger("Setup", "Ring23DCAddValueMaxLimit", M2Share.Config.Ring23DCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring23DCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring23DCAddValueRate", M2Share.Config.Ring23DCAddValueRate);
            M2Share.Config.Ring23DCAddValueRate = ReadInteger("Setup", "Ring23DCAddValueRate", M2Share.Config.Ring23DCAddValueRate);
            if (ReadInteger("Setup", "Ring23DCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring23DCAddRate", M2Share.Config.Ring23DCAddRate);
            M2Share.Config.Ring23DCAddRate = ReadInteger("Setup", "Ring23DCAddRate", M2Share.Config.Ring23DCAddRate);
            if (ReadInteger("Setup", "Ring23MCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring23MCAddValueMaxLimit", M2Share.Config.Ring23MCAddValueMaxLimit);
            M2Share.Config.Ring23MCAddValueMaxLimit = ReadInteger("Setup", "Ring23MCAddValueMaxLimit", M2Share.Config.Ring23MCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring23MCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring23MCAddValueRate", M2Share.Config.Ring23MCAddValueRate);
            M2Share.Config.Ring23MCAddValueRate = ReadInteger("Setup", "Ring23MCAddValueRate", M2Share.Config.Ring23MCAddValueRate);
            if (ReadInteger("Setup", "Ring23MCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring23MCAddRate", M2Share.Config.Ring23MCAddRate);
            M2Share.Config.Ring23MCAddRate = ReadInteger("Setup", "Ring23MCAddRate", M2Share.Config.Ring23MCAddRate);
            if (ReadInteger("Setup", "Ring23SCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "Ring23SCAddValueMaxLimit", M2Share.Config.Ring23SCAddValueMaxLimit);
            M2Share.Config.Ring23SCAddValueMaxLimit = ReadInteger("Setup", "Ring23SCAddValueMaxLimit", M2Share.Config.Ring23SCAddValueMaxLimit);
            if (ReadInteger("Setup", "Ring23SCAddValueRate", -1) < 0)
                WriteInteger("Setup", "Ring23SCAddValueRate", M2Share.Config.Ring23SCAddValueRate);
            M2Share.Config.Ring23SCAddValueRate = ReadInteger("Setup", "Ring23SCAddValueRate", M2Share.Config.Ring23SCAddValueRate);
            if (ReadInteger("Setup", "Ring23SCAddRate", -1) < 0)
                WriteInteger("Setup", "Ring23SCAddRate", M2Share.Config.Ring23SCAddRate);
            M2Share.Config.Ring23SCAddRate = ReadInteger("Setup", "Ring23SCAddRate", M2Share.Config.Ring23SCAddRate);
            if (ReadInteger("Setup", "HelMetDCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "HelMetDCAddValueMaxLimit", M2Share.Config.HelMetDCAddValueMaxLimit);
            M2Share.Config.HelMetDCAddValueMaxLimit = ReadInteger("Setup", "HelMetDCAddValueMaxLimit", M2Share.Config.HelMetDCAddValueMaxLimit);
            if (ReadInteger("Setup", "HelMetDCAddValueRate", -1) < 0)
                WriteInteger("Setup", "HelMetDCAddValueRate", M2Share.Config.HelMetDCAddValueRate);
            M2Share.Config.HelMetDCAddValueRate = ReadInteger("Setup", "HelMetDCAddValueRate", M2Share.Config.HelMetDCAddValueRate);
            if (ReadInteger("Setup", "HelMetDCAddRate", -1) < 0)
                WriteInteger("Setup", "HelMetDCAddRate", M2Share.Config.HelMetDCAddRate);
            M2Share.Config.HelMetDCAddRate = ReadInteger("Setup", "HelMetDCAddRate", M2Share.Config.HelMetDCAddRate);
            if (ReadInteger("Setup", "HelMetMCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "HelMetMCAddValueMaxLimit", M2Share.Config.HelMetMCAddValueMaxLimit);
            M2Share.Config.HelMetMCAddValueMaxLimit = ReadInteger("Setup", "HelMetMCAddValueMaxLimit", M2Share.Config.HelMetMCAddValueMaxLimit);
            if (ReadInteger("Setup", "HelMetMCAddValueRate", -1) < 0)
                WriteInteger("Setup", "HelMetMCAddValueRate", M2Share.Config.HelMetMCAddValueRate);
            M2Share.Config.HelMetMCAddValueRate = ReadInteger("Setup", "HelMetMCAddValueRate", M2Share.Config.HelMetMCAddValueRate);
            if (ReadInteger("Setup", "HelMetMCAddRate", -1) < 0)
                WriteInteger("Setup", "HelMetMCAddRate", M2Share.Config.HelMetMCAddRate);
            M2Share.Config.HelMetMCAddRate = ReadInteger("Setup", "HelMetMCAddRate", M2Share.Config.HelMetMCAddRate);
            if (ReadInteger("Setup", "HelMetSCAddValueMaxLimit", -1) < 0)
                WriteInteger("Setup", "HelMetSCAddValueMaxLimit", M2Share.Config.HelMetSCAddValueMaxLimit);
            M2Share.Config.HelMetSCAddValueMaxLimit = ReadInteger("Setup", "HelMetSCAddValueMaxLimit", M2Share.Config.HelMetSCAddValueMaxLimit);
            if (ReadInteger("Setup", "HelMetSCAddValueRate", -1) < 0)
                WriteInteger("Setup", "HelMetSCAddValueRate", M2Share.Config.HelMetSCAddValueRate);
            M2Share.Config.HelMetSCAddValueRate = ReadInteger("Setup", "HelMetSCAddValueRate", M2Share.Config.HelMetSCAddValueRate);
            if (ReadInteger("Setup", "HelMetSCAddRate", -1) < 0)
                WriteInteger("Setup", "HelMetSCAddRate", M2Share.Config.HelMetSCAddRate);
            M2Share.Config.HelMetSCAddRate = ReadInteger("Setup", "HelMetSCAddRate", M2Share.Config.HelMetSCAddRate);
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetACAddRate", M2Share.Config.UnknowHelMetACAddRate);
            else
                M2Share.Config.UnknowHelMetACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetACAddValueMaxLimit", M2Share.Config.UnknowHelMetACAddValueMaxLimit);
            else
                M2Share.Config.UnknowHelMetACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetMACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetMACAddRate", M2Share.Config.UnknowHelMetMACAddRate);
            else
                M2Share.Config.UnknowHelMetMACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetMACAddValueMaxLimit", M2Share.Config.UnknowHelMetMACAddValueMaxLimit);
            else
                M2Share.Config.UnknowHelMetMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetDCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetDCAddRate", M2Share.Config.UnknowHelMetDCAddRate);
            else
                M2Share.Config.UnknowHelMetDCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetDCAddValueMaxLimit", M2Share.Config.UnknowHelMetDCAddValueMaxLimit);
            else
                M2Share.Config.UnknowHelMetDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetMCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetMCAddRate", M2Share.Config.UnknowHelMetMCAddRate);
            else
                M2Share.Config.UnknowHelMetMCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetMCAddValueMaxLimit", M2Share.Config.UnknowHelMetMCAddValueMaxLimit);
            else
                M2Share.Config.UnknowHelMetMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetSCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetSCAddRate", M2Share.Config.UnknowHelMetSCAddRate);
            else
                M2Share.Config.UnknowHelMetSCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowHelMetSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowHelMetSCAddValueMaxLimit", M2Share.Config.UnknowHelMetSCAddValueMaxLimit);
            else
                M2Share.Config.UnknowHelMetSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceACAddRate", M2Share.Config.UnknowNecklaceACAddRate);
            else
                M2Share.Config.UnknowNecklaceACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceACAddValueMaxLimit", M2Share.Config.UnknowNecklaceACAddValueMaxLimit);
            else
                M2Share.Config.UnknowNecklaceACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceMACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMACAddRate", M2Share.Config.UnknowNecklaceMACAddRate);
            else
                M2Share.Config.UnknowNecklaceMACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMACAddValueMaxLimit", M2Share.Config.UnknowNecklaceMACAddValueMaxLimit);
            else
                M2Share.Config.UnknowNecklaceMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceDCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceDCAddRate", M2Share.Config.UnknowNecklaceDCAddRate);
            else
                M2Share.Config.UnknowNecklaceDCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceDCAddValueMaxLimit", M2Share.Config.UnknowNecklaceDCAddValueMaxLimit);
            else
                M2Share.Config.UnknowNecklaceDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceMCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMCAddRate", M2Share.Config.UnknowNecklaceMCAddRate);
            else
                M2Share.Config.UnknowNecklaceMCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMCAddValueMaxLimit", M2Share.Config.UnknowNecklaceMCAddValueMaxLimit);
            else
                M2Share.Config.UnknowNecklaceMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceSCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceSCAddRate", M2Share.Config.UnknowNecklaceSCAddRate);
            else
                M2Share.Config.UnknowNecklaceSCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowNecklaceSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceSCAddValueMaxLimit", M2Share.Config.UnknowNecklaceSCAddValueMaxLimit);
            else
                M2Share.Config.UnknowNecklaceSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingACAddRate", M2Share.Config.UnknowRingACAddRate);
            else
                M2Share.Config.UnknowRingACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingACAddValueMaxLimit", M2Share.Config.UnknowRingACAddValueMaxLimit);
            else
                M2Share.Config.UnknowRingACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingMACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMACAddRate", M2Share.Config.UnknowRingMACAddRate);
            else
                M2Share.Config.UnknowRingMACAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMACAddValueMaxLimit", M2Share.Config.UnknowRingMACAddValueMaxLimit);
            else
                M2Share.Config.UnknowRingMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingDCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingDCAddRate", M2Share.Config.UnknowRingDCAddRate);
            else
                M2Share.Config.UnknowRingDCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingDCAddValueMaxLimit", M2Share.Config.UnknowRingDCAddValueMaxLimit);
            else
                M2Share.Config.UnknowRingDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingMCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMCAddRate", M2Share.Config.UnknowRingMCAddRate);
            else
                M2Share.Config.UnknowRingMCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMCAddValueMaxLimit", M2Share.Config.UnknowRingMCAddValueMaxLimit);
            else
                M2Share.Config.UnknowRingMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingSCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingSCAddRate", M2Share.Config.UnknowRingSCAddRate);
            else
                M2Share.Config.UnknowRingSCAddRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "UnknowRingSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingSCAddValueMaxLimit", M2Share.Config.UnknowRingSCAddValueMaxLimit);
            else
                M2Share.Config.UnknowRingSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MonOneDropGoldCount", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MonOneDropGoldCount", M2Share.Config.MonOneDropGoldCount);
            else
                M2Share.Config.MonOneDropGoldCount = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "SendCurTickCount", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "SendCurTickCount", M2Share.Config.SendCurTickCount);
            else
                M2Share.Config.SendCurTickCount = nLoadInteger == 1;
            if (ReadInteger("Setup", "MakeMineHitRate", -1) < 0)
                WriteInteger("Setup", "MakeMineHitRate", M2Share.Config.MakeMineHitRate);
            M2Share.Config.MakeMineHitRate = ReadInteger("Setup", "MakeMineHitRate", M2Share.Config.MakeMineHitRate);
            if (ReadInteger("Setup", "MakeMineRate", -1) < 0)
                WriteInteger("Setup", "MakeMineRate", M2Share.Config.MakeMineRate);
            M2Share.Config.MakeMineRate = ReadInteger("Setup", "MakeMineRate", M2Share.Config.MakeMineRate);
            if (ReadInteger("Setup", "StoneTypeRate", -1) < 0)
                WriteInteger("Setup", "StoneTypeRate", M2Share.Config.StoneTypeRate);
            M2Share.Config.StoneTypeRate = ReadInteger("Setup", "StoneTypeRate", M2Share.Config.StoneTypeRate);
            if (ReadInteger("Setup", "StoneTypeRateMin", -1) < 0)
                WriteInteger("Setup", "StoneTypeRateMin", M2Share.Config.StoneTypeRateMin);
            M2Share.Config.StoneTypeRateMin = ReadInteger("Setup", "StoneTypeRateMin", M2Share.Config.StoneTypeRateMin);
            if (ReadInteger("Setup", "GoldStoneMin", -1) < 0)
                WriteInteger("Setup", "GoldStoneMin", M2Share.Config.GoldStoneMin);
            M2Share.Config.GoldStoneMin = ReadInteger("Setup", "GoldStoneMin", M2Share.Config.GoldStoneMin);
            if (ReadInteger("Setup", "GoldStoneMax", -1) < 0)
                WriteInteger("Setup", "GoldStoneMax", M2Share.Config.GoldStoneMax);
            M2Share.Config.GoldStoneMax = ReadInteger("Setup", "GoldStoneMax", M2Share.Config.GoldStoneMax);
            if (ReadInteger("Setup", "SilverStoneMin", -1) < 0)
                WriteInteger("Setup", "SilverStoneMin", M2Share.Config.SilverStoneMin);
            M2Share.Config.SilverStoneMin = ReadInteger("Setup", "SilverStoneMin", M2Share.Config.SilverStoneMin);
            if (ReadInteger("Setup", "SilverStoneMax", -1) < 0)
                WriteInteger("Setup", "SilverStoneMax", M2Share.Config.SilverStoneMax);
            M2Share.Config.SilverStoneMax = ReadInteger("Setup", "SilverStoneMax", M2Share.Config.SilverStoneMax);
            if (ReadInteger("Setup", "SteelStoneMin", -1) < 0)
                WriteInteger("Setup", "SteelStoneMin", M2Share.Config.SteelStoneMin);
            M2Share.Config.SteelStoneMin = ReadInteger("Setup", "SteelStoneMin", M2Share.Config.SteelStoneMin);
            if (ReadInteger("Setup", "SteelStoneMax", -1) < 0)
                WriteInteger("Setup", "SteelStoneMax", M2Share.Config.SteelStoneMax);
            M2Share.Config.SteelStoneMax = ReadInteger("Setup", "SteelStoneMax", M2Share.Config.SteelStoneMax);
            if (ReadInteger("Setup", "BlackStoneMin", -1) < 0)
                WriteInteger("Setup", "BlackStoneMin", M2Share.Config.BlackStoneMin);
            M2Share.Config.BlackStoneMin = ReadInteger("Setup", "BlackStoneMin", M2Share.Config.BlackStoneMin);
            if (ReadInteger("Setup", "BlackStoneMax", -1) < 0)
                WriteInteger("Setup", "BlackStoneMax", M2Share.Config.BlackStoneMax);
            M2Share.Config.BlackStoneMax = ReadInteger("Setup", "BlackStoneMax", M2Share.Config.BlackStoneMax);
            if (ReadInteger("Setup", "StoneMinDura", -1) < 0)
                WriteInteger("Setup", "StoneMinDura", M2Share.Config.StoneMinDura);
            M2Share.Config.StoneMinDura = ReadInteger("Setup", "StoneMinDura", M2Share.Config.StoneMinDura);
            if (ReadInteger("Setup", "StoneGeneralDuraRate", -1) < 0)
                WriteInteger("Setup", "StoneGeneralDuraRate", M2Share.Config.StoneGeneralDuraRate);
            M2Share.Config.StoneGeneralDuraRate = ReadInteger("Setup", "StoneGeneralDuraRate", M2Share.Config.StoneGeneralDuraRate);
            if (ReadInteger("Setup", "StoneAddDuraRate", -1) < 0)
                WriteInteger("Setup", "StoneAddDuraRate", M2Share.Config.StoneAddDuraRate);
            M2Share.Config.StoneAddDuraRate = ReadInteger("Setup", "StoneAddDuraRate", M2Share.Config.StoneAddDuraRate);
            if (ReadInteger("Setup", "StoneAddDuraMax", -1) < 0)
                WriteInteger("Setup", "StoneAddDuraMax", M2Share.Config.StoneAddDuraMax);
            M2Share.Config.StoneAddDuraMax = ReadInteger("Setup", "StoneAddDuraMax", M2Share.Config.StoneAddDuraMax);
            if (ReadInteger("Setup", "WinLottery1Min", -1) < 0)
                WriteInteger("Setup", "WinLottery1Min", M2Share.Config.WinLottery1Min);
            M2Share.Config.WinLottery1Min = ReadInteger("Setup", "WinLottery1Min", M2Share.Config.WinLottery1Min);
            if (ReadInteger("Setup", "WinLottery1Max", -1) < 0)
                WriteInteger("Setup", "WinLottery1Max", M2Share.Config.WinLottery1Max);
            M2Share.Config.WinLottery1Max = ReadInteger("Setup", "WinLottery1Max", M2Share.Config.WinLottery1Max);
            if (ReadInteger("Setup", "WinLottery2Min", -1) < 0)
                WriteInteger("Setup", "WinLottery2Min", M2Share.Config.WinLottery2Min);
            M2Share.Config.WinLottery2Min = ReadInteger("Setup", "WinLottery2Min", M2Share.Config.WinLottery2Min);
            if (ReadInteger("Setup", "WinLottery2Max", -1) < 0)
                WriteInteger("Setup", "WinLottery2Max", M2Share.Config.WinLottery2Max);
            M2Share.Config.WinLottery2Max = ReadInteger("Setup", "WinLottery2Max", M2Share.Config.WinLottery2Max);
            if (ReadInteger("Setup", "WinLottery3Min", -1) < 0)
                WriteInteger("Setup", "WinLottery3Min", M2Share.Config.WinLottery3Min);
            M2Share.Config.WinLottery3Min = ReadInteger("Setup", "WinLottery3Min", M2Share.Config.WinLottery3Min);
            if (ReadInteger("Setup", "WinLottery3Max", -1) < 0)
                WriteInteger("Setup", "WinLottery3Max", M2Share.Config.WinLottery3Max);
            M2Share.Config.WinLottery3Max = ReadInteger("Setup", "WinLottery3Max", M2Share.Config.WinLottery3Max);
            if (ReadInteger("Setup", "WinLottery4Min", -1) < 0)
                WriteInteger("Setup", "WinLottery4Min", M2Share.Config.WinLottery4Min);
            M2Share.Config.WinLottery4Min = ReadInteger("Setup", "WinLottery4Min", M2Share.Config.WinLottery4Min);
            if (ReadInteger("Setup", "WinLottery4Max", -1) < 0)
                WriteInteger("Setup", "WinLottery4Max", M2Share.Config.WinLottery4Max);
            M2Share.Config.WinLottery4Max = ReadInteger("Setup", "WinLottery4Max", M2Share.Config.WinLottery4Max);
            if (ReadInteger("Setup", "WinLottery5Min", -1) < 0)
                WriteInteger("Setup", "WinLottery5Min", M2Share.Config.WinLottery5Min);
            M2Share.Config.WinLottery5Min = ReadInteger("Setup", "WinLottery5Min", M2Share.Config.WinLottery5Min);
            if (ReadInteger("Setup", "WinLottery5Max", -1) < 0)
                WriteInteger("Setup", "WinLottery5Max", M2Share.Config.WinLottery5Max);
            M2Share.Config.WinLottery5Max = ReadInteger("Setup", "WinLottery5Max", M2Share.Config.WinLottery5Max);
            if (ReadInteger("Setup", "WinLottery6Min", -1) < 0)
                WriteInteger("Setup", "WinLottery6Min", M2Share.Config.WinLottery6Min);
            M2Share.Config.WinLottery6Min = ReadInteger("Setup", "WinLottery6Min", M2Share.Config.WinLottery6Min);
            if (ReadInteger("Setup", "WinLottery6Max", -1) < 0)
                WriteInteger("Setup", "WinLottery6Max", M2Share.Config.WinLottery6Max);
            M2Share.Config.WinLottery6Max = ReadInteger("Setup", "WinLottery6Max", M2Share.Config.WinLottery6Max);
            if (ReadInteger("Setup", "WinLotteryRate", -1) < 0)
                WriteInteger("Setup", "WinLotteryRate", M2Share.Config.WinLotteryRate);
            M2Share.Config.WinLotteryRate = ReadInteger("Setup", "WinLotteryRate", M2Share.Config.WinLotteryRate);
            if (ReadInteger("Setup", "WinLottery1Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery1Gold", M2Share.Config.WinLottery1Gold);
            M2Share.Config.WinLottery1Gold = ReadInteger("Setup", "WinLottery1Gold", M2Share.Config.WinLottery1Gold);
            if (ReadInteger("Setup", "WinLottery2Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery2Gold", M2Share.Config.WinLottery2Gold);
            M2Share.Config.WinLottery2Gold = ReadInteger("Setup", "WinLottery2Gold", M2Share.Config.WinLottery2Gold);
            if (ReadInteger("Setup", "WinLottery3Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery3Gold", M2Share.Config.WinLottery3Gold);
            M2Share.Config.WinLottery3Gold = ReadInteger("Setup", "WinLottery3Gold", M2Share.Config.WinLottery3Gold);
            if (ReadInteger("Setup", "WinLottery4Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery4Gold", M2Share.Config.WinLottery4Gold);
            M2Share.Config.WinLottery4Gold = ReadInteger("Setup", "WinLottery4Gold", M2Share.Config.WinLottery4Gold);
            if (ReadInteger("Setup", "WinLottery5Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery5Gold", M2Share.Config.WinLottery5Gold);
            M2Share.Config.WinLottery5Gold = ReadInteger("Setup", "WinLottery5Gold", M2Share.Config.WinLottery5Gold);
            if (ReadInteger("Setup", "WinLottery6Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery6Gold", M2Share.Config.WinLottery6Gold);
            M2Share.Config.WinLottery6Gold = ReadInteger("Setup", "WinLottery6Gold", M2Share.Config.WinLottery6Gold);
            if (ReadInteger("Setup", "GuildRecallTime", -1) < 0)
                WriteInteger("Setup", "GuildRecallTime", M2Share.Config.GuildRecallTime);
            M2Share.Config.GuildRecallTime = ReadInteger("Setup", "GuildRecallTime", M2Share.Config.GuildRecallTime);
            if (ReadInteger("Setup", "GroupRecallTime", -1) < 0)
                WriteInteger("Setup", "GroupRecallTime", M2Share.Config.GroupRecallTime);
            M2Share.Config.GroupRecallTime = ReadInteger("Setup", "GroupRecallTime", M2Share.Config.GroupRecallTime);
            if (ReadInteger("Setup", "ControlDropItem", -1) < 0)
                WriteBool("Setup", "ControlDropItem", M2Share.Config.ControlDropItem);
            M2Share.Config.ControlDropItem = ReadBool("Setup", "ControlDropItem", M2Share.Config.ControlDropItem);
            if (ReadInteger("Setup", "InSafeDisableDrop", -1) < 0)
                WriteBool("Setup", "InSafeDisableDrop", M2Share.Config.InSafeDisableDrop);
            M2Share.Config.InSafeDisableDrop = ReadBool("Setup", "InSafeDisableDrop", M2Share.Config.InSafeDisableDrop);
            if (ReadInteger("Setup", "CanDropGold", -1) < 0)
                WriteInteger("Setup", "CanDropGold", M2Share.Config.CanDropGold);
            M2Share.Config.CanDropGold = ReadInteger("Setup", "CanDropGold", M2Share.Config.CanDropGold);
            if (ReadInteger("Setup", "CanDropPrice", -1) < 0)
                WriteInteger("Setup", "CanDropPrice", M2Share.Config.CanDropPrice);
            M2Share.Config.CanDropPrice = ReadInteger("Setup", "CanDropPrice", M2Share.Config.CanDropPrice);
            nLoadInteger = ReadInteger("Setup", "SendCustemMsg", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "SendCustemMsg", M2Share.Config.SendCustemMsg);
            else
                M2Share.Config.SendCustemMsg = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "SubkMasterSendMsg", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "SubkMasterSendMsg", M2Share.Config.SubkMasterSendMsg);
            else
                M2Share.Config.SubkMasterSendMsg = nLoadInteger == 1;
            if (ReadInteger("Setup", "SuperRepairPriceRate", -1) < 0)
                WriteInteger("Setup", "SuperRepairPriceRate", M2Share.Config.SuperRepairPriceRate);
            M2Share.Config.SuperRepairPriceRate = ReadInteger("Setup", "SuperRepairPriceRate", M2Share.Config.SuperRepairPriceRate);
            if (ReadInteger("Setup", "RepairItemDecDura", -1) < 0)
                WriteInteger("Setup", "RepairItemDecDura", M2Share.Config.RepairItemDecDura);
            M2Share.Config.RepairItemDecDura = ReadInteger("Setup", "RepairItemDecDura", M2Share.Config.RepairItemDecDura);
            if (ReadInteger("Setup", "DieScatterBag", -1) < 0)
                WriteBool("Setup", "DieScatterBag", M2Share.Config.DieScatterBag);
            M2Share.Config.DieScatterBag = ReadBool("Setup", "DieScatterBag", M2Share.Config.DieScatterBag);
            if (ReadInteger("Setup", "DieScatterBagRate", -1) < 0)
                WriteInteger("Setup", "DieScatterBagRate", M2Share.Config.DieScatterBagRate);
            M2Share.Config.DieScatterBagRate = ReadInteger("Setup", "DieScatterBagRate", M2Share.Config.DieScatterBagRate);
            if (ReadInteger("Setup", "DieRedScatterBagAll", -1) < 0)
                WriteBool("Setup", "DieRedScatterBagAll", M2Share.Config.DieRedScatterBagAll);
            M2Share.Config.DieRedScatterBagAll = ReadBool("Setup", "DieRedScatterBagAll", M2Share.Config.DieRedScatterBagAll);
            if (ReadInteger("Setup", "DieDropUseItemRate", -1) < 0)
                WriteInteger("Setup", "DieDropUseItemRate", M2Share.Config.DieDropUseItemRate);
            M2Share.Config.DieDropUseItemRate = ReadInteger("Setup", "DieDropUseItemRate", M2Share.Config.DieDropUseItemRate);
            if (ReadInteger("Setup", "DieRedDropUseItemRate", -1) < 0)
                WriteInteger("Setup", "DieRedDropUseItemRate", M2Share.Config.DieRedDropUseItemRate);
            M2Share.Config.DieRedDropUseItemRate = ReadInteger("Setup", "DieRedDropUseItemRate", M2Share.Config.DieRedDropUseItemRate);
            if (ReadInteger("Setup", "DieDropGold", -1) < 0)
                WriteBool("Setup", "DieDropGold", M2Share.Config.DieDropGold);
            M2Share.Config.DieDropGold = ReadBool("Setup", "DieDropGold", M2Share.Config.DieDropGold);
            if (ReadInteger("Setup", "KillByHumanDropUseItem", -1) < 0)
                WriteBool("Setup", "KillByHumanDropUseItem", M2Share.Config.KillByHumanDropUseItem);
            M2Share.Config.KillByHumanDropUseItem = ReadBool("Setup", "KillByHumanDropUseItem", M2Share.Config.KillByHumanDropUseItem);
            if (ReadInteger("Setup", "KillByMonstDropUseItem", -1) < 0)
                WriteBool("Setup", "KillByMonstDropUseItem", M2Share.Config.KillByMonstDropUseItem);
            M2Share.Config.KillByMonstDropUseItem = ReadBool("Setup", "KillByMonstDropUseItem", M2Share.Config.KillByMonstDropUseItem);
            if (ReadInteger("Setup", "KickExpireHuman", -1) < 0)
                WriteBool("Setup", "KickExpireHuman", M2Share.Config.KickExpireHuman);
            M2Share.Config.KickExpireHuman = ReadBool("Setup", "KickExpireHuman", M2Share.Config.KickExpireHuman);
            if (ReadInteger("Setup", "GuildRankNameLen", -1) < 0)
                WriteInteger("Setup", "GuildRankNameLen", M2Share.Config.GuildRankNameLen);
            M2Share.Config.GuildRankNameLen = ReadInteger("Setup", "GuildRankNameLen", M2Share.Config.GuildRankNameLen);
            if (ReadInteger("Setup", "GuildNameLen", -1) < 0)
                WriteInteger("Setup", "GuildNameLen", M2Share.Config.GuildNameLen);
            M2Share.Config.GuildNameLen = ReadInteger("Setup", "GuildNameLen", M2Share.Config.GuildNameLen);
            if (ReadInteger("Setup", "GuildMemberMaxLimit", -1) < 0)
                WriteInteger("Setup", "GuildMemberMaxLimit", M2Share.Config.GuildMemberMaxLimit);
            M2Share.Config.GuildMemberMaxLimit = ReadInteger("Setup", "GuildMemberMaxLimit", M2Share.Config.GuildMemberMaxLimit);
            if (ReadInteger("Setup", "AttackPosionRate", -1) < 0)
                WriteInteger("Setup", "AttackPosionRate", M2Share.Config.AttackPosionRate);
            M2Share.Config.AttackPosionRate = ReadInteger("Setup", "AttackPosionRate", M2Share.Config.AttackPosionRate);
            if (ReadInteger("Setup", "AttackPosionTime", -1) < 0)
                WriteInteger("Setup", "AttackPosionTime", M2Share.Config.AttackPosionTime);
            M2Share.Config.AttackPosionTime = (ushort)ReadInteger("Setup", "AttackPosionTime", M2Share.Config.AttackPosionTime);
            if (ReadInteger("Setup", "RevivalTime", -1) < 0)
                WriteInteger("Setup", "RevivalTime", M2Share.Config.RevivalTime);
            M2Share.Config.RevivalTime = ReadInteger("Setup", "RevivalTime", M2Share.Config.RevivalTime);
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
                WriteInteger("Setup", "WarGuildNameColor", M2Share.Config.WarGuildNameColor);
            M2Share.Config.WarGuildNameColor = Read<byte>("Setup", "WarGuildNameColor", M2Share.Config.WarGuildNameColor);
            if (ReadInteger("Setup", "InFreePKAreaNameColor", -1) < 0)
                WriteInteger("Setup", "InFreePKAreaNameColor", M2Share.Config.InFreePKAreaNameColor);
            M2Share.Config.InFreePKAreaNameColor = Read<byte>("Setup", "InFreePKAreaNameColor", M2Share.Config.InFreePKAreaNameColor);
            if (ReadInteger("Setup", "PKLevel1NameColor", -1) < 0)
                WriteInteger("Setup", "PKLevel1NameColor", M2Share.Config.btPKLevel1NameColor);
            M2Share.Config.btPKLevel1NameColor = Read<byte>("Setup", "PKLevel1NameColor", M2Share.Config.btPKLevel1NameColor);
            if (ReadInteger("Setup", "PKLevel2NameColor", -1) < 0)
                WriteInteger("Setup", "PKLevel2NameColor", M2Share.Config.btPKLevel2NameColor);
            M2Share.Config.btPKLevel2NameColor = Read<byte>("Setup", "PKLevel2NameColor", M2Share.Config.btPKLevel2NameColor);
            if (ReadInteger("Setup", "SpiritMutiny", -1) < 0)
                WriteBool("Setup", "SpiritMutiny", M2Share.Config.SpiritMutiny);
            M2Share.Config.SpiritMutiny = ReadBool("Setup", "SpiritMutiny", M2Share.Config.SpiritMutiny);
            if (ReadInteger("Setup", "SpiritMutinyTime", -1) < 0)
                WriteInteger("Setup", "SpiritMutinyTime", M2Share.Config.SpiritMutinyTime);
            M2Share.Config.SpiritMutinyTime = ReadInteger("Setup", "SpiritMutinyTime", M2Share.Config.SpiritMutinyTime);
            if (ReadInteger("Setup", "SpiritPowerRate", -1) < 0)
                WriteInteger("Setup", "SpiritPowerRate", M2Share.Config.SpiritPowerRate);
            M2Share.Config.SpiritPowerRate = ReadInteger("Setup", "SpiritPowerRate", M2Share.Config.SpiritPowerRate);
            if (ReadInteger("Setup", "MasterDieMutiny", -1) < 0)
                WriteBool("Setup", "MasterDieMutiny", M2Share.Config.MasterDieMutiny);
            M2Share.Config.MasterDieMutiny = ReadBool("Setup", "MasterDieMutiny", M2Share.Config.MasterDieMutiny);
            if (ReadInteger("Setup", "MasterDieMutinyRate", -1) < 0)
                WriteInteger("Setup", "MasterDieMutinyRate", M2Share.Config.MasterDieMutinyRate);
            M2Share.Config.MasterDieMutinyRate = ReadInteger("Setup", "MasterDieMutinyRate", M2Share.Config.MasterDieMutinyRate);
            if (ReadInteger("Setup", "MasterDieMutinyPower", -1) < 0)
                WriteInteger("Setup", "MasterDieMutinyPower", M2Share.Config.MasterDieMutinyPower);
            M2Share.Config.MasterDieMutinyPower = ReadInteger("Setup", "MasterDieMutinyPower", M2Share.Config.MasterDieMutinyPower);
            if (ReadInteger("Setup", "MasterDieMutinyPower", -1) < 0)
                WriteInteger("Setup", "MasterDieMutinyPower", M2Share.Config.MasterDieMutinySpeed);
            M2Share.Config.MasterDieMutinySpeed = ReadInteger("Setup", "MasterDieMutinyPower", M2Share.Config.MasterDieMutinySpeed);
            nLoadInteger = ReadInteger("Setup", "BBMonAutoChangeColor", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "BBMonAutoChangeColor", M2Share.Config.BBMonAutoChangeColor);
            else
                M2Share.Config.BBMonAutoChangeColor = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "BBMonAutoChangeColorTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "BBMonAutoChangeColorTime", M2Share.Config.BBMonAutoChangeColorTime);
            else
                M2Share.Config.BBMonAutoChangeColorTime = nLoadInteger;
            if (ReadInteger("Setup", "OldClientShowHiLevel", -1) < 0)
                WriteBool("Setup", "OldClientShowHiLevel", M2Share.Config.boOldClientShowHiLevel);
            M2Share.Config.boOldClientShowHiLevel = ReadBool("Setup", "OldClientShowHiLevel", M2Share.Config.boOldClientShowHiLevel);
            if (ReadInteger("Setup", "ShowScriptActionMsg", -1) < 0)
                WriteBool("Setup", "ShowScriptActionMsg", M2Share.Config.ShowScriptActionMsg);
            M2Share.Config.ShowScriptActionMsg = ReadBool("Setup", "ShowScriptActionMsg", M2Share.Config.ShowScriptActionMsg);
            if (ReadInteger("Setup", "RunSocketDieLoopLimit", -1) < 0)
                WriteInteger("Setup", "RunSocketDieLoopLimit", M2Share.Config.RunSocketDieLoopLimit);
            M2Share.Config.RunSocketDieLoopLimit = ReadInteger("Setup", "RunSocketDieLoopLimit", M2Share.Config.RunSocketDieLoopLimit);
            if (ReadInteger("Setup", "ThreadRun", -1) < 0)
                WriteBool("Setup", "ThreadRun", M2Share.Config.ThreadRun);
            M2Share.Config.ThreadRun = ReadBool("Setup", "ThreadRun", M2Share.Config.ThreadRun);
            if (ReadInteger("Setup", "ShowExceptionMsg", -1) < 0)
                WriteBool("Setup", "ShowExceptionMsg", M2Share.Config.ShowExceptionMsg);
            M2Share.Config.ShowExceptionMsg = ReadBool("Setup", "ShowExceptionMsg", M2Share.Config.ShowExceptionMsg);
            if (ReadInteger("Setup", "ShowPreFixMsg", -1) < 0)
                WriteBool("Setup", "ShowPreFixMsg", M2Share.Config.ShowPreFixMsg);
            M2Share.Config.ShowPreFixMsg = ReadBool("Setup", "ShowPreFixMsg", M2Share.Config.ShowPreFixMsg);
            if (ReadInteger("Setup", "MagTurnUndeadLevel", -1) < 0)
                WriteInteger("Setup", "MagTurnUndeadLevel", M2Share.Config.MagTurnUndeadLevel);
            M2Share.Config.MagTurnUndeadLevel = ReadInteger("Setup", "MagTurnUndeadLevel", M2Share.Config.MagTurnUndeadLevel);
            nLoadInteger = ReadInteger("Setup", "MagTammingLevel", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingLevel", M2Share.Config.MagTammingLevel);
            else
                M2Share.Config.MagTammingLevel = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MagTammingTargetLevel", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingTargetLevel", M2Share.Config.MagTammingTargetLevel);
            else
                M2Share.Config.MagTammingTargetLevel = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MagTammingTargetHPRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingTargetHPRate", M2Share.Config.MagTammingHPRate);
            else
                M2Share.Config.MagTammingHPRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MagTammingCount", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingCount", M2Share.Config.MagTammingCount);
            else
                M2Share.Config.MagTammingCount = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MabMabeHitRandRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitRandRate", M2Share.Config.MabMabeHitRandRate);
            else
                M2Share.Config.MabMabeHitRandRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MabMabeHitMinLvLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitMinLvLimit", M2Share.Config.MabMabeHitMinLvLimit);
            else
                M2Share.Config.MabMabeHitMinLvLimit = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MabMabeHitSucessRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitSucessRate", M2Share.Config.MabMabeHitSucessRate);
            else
                M2Share.Config.MabMabeHitSucessRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MabMabeHitMabeTimeRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitMabeTimeRate", M2Share.Config.MabMabeHitMabeTimeRate);
            else
                M2Share.Config.MabMabeHitMabeTimeRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "MagicAttackRage", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagicAttackRage", M2Share.Config.MagicAttackRage);
            else
                M2Share.Config.MagicAttackRage = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "DropItemRage", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "DropItemRage", M2Share.Config.DropItemRage);
            else
                M2Share.Config.DropItemRage = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "AmyOunsulPoint", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "AmyOunsulPoint", M2Share.Config.AmyOunsulPoint);
            else
                M2Share.Config.AmyOunsulPoint = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "DisableInSafeZoneFireCross", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "DisableInSafeZoneFireCross", M2Share.Config.DisableInSafeZoneFireCross);
            else
                M2Share.Config.DisableInSafeZoneFireCross = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "GroupMbAttackPlayObject", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "GroupMbAttackPlayObject", M2Share.Config.GroupMbAttackPlayObject);
            else
                M2Share.Config.GroupMbAttackPlayObject = nLoadInteger == 1;
            nLoadInteger = ReadInteger("Setup", "PosionDecHealthTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "PosionDecHealthTime", M2Share.Config.PosionDecHealthTime);
            else
                M2Share.Config.PosionDecHealthTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "PosionDamagarmor", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "PosionDamagarmor", M2Share.Config.PosionDamagarmor);
            else
                M2Share.Config.PosionDamagarmor = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "LimitSwordLong", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "LimitSwordLong", M2Share.Config.LimitSwordLong);
            else
                M2Share.Config.LimitSwordLong = !(nLoadInteger == 0);
            nLoadInteger = ReadInteger("Setup", "SwordLongPowerRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "SwordLongPowerRate", M2Share.Config.SwordLongPowerRate);
            else
                M2Share.Config.SwordLongPowerRate = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "FireBoomRage", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "FireBoomRage", M2Share.Config.FireBoomRage);
            else
                M2Share.Config.FireBoomRage = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "SnowWindRange", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "SnowWindRange", M2Share.Config.SnowWindRange);
            else
                M2Share.Config.SnowWindRange = nLoadInteger;
            if (ReadInteger("Setup", "ElecBlizzardRange", -1) < 0)
                WriteInteger("Setup", "ElecBlizzardRange", M2Share.Config.ElecBlizzardRange);
            M2Share.Config.ElecBlizzardRange = ReadInteger("Setup", "ElecBlizzardRange", M2Share.Config.ElecBlizzardRange);
            if (ReadInteger("Setup", "HumanLevelDiffer", -1) < 0)
                WriteInteger("Setup", "HumanLevelDiffer", M2Share.Config.HumanLevelDiffer);
            M2Share.Config.HumanLevelDiffer = ReadInteger("Setup", "HumanLevelDiffer", M2Share.Config.HumanLevelDiffer);
            if (ReadInteger("Setup", "KillHumanWinLevel", -1) < 0)
                WriteBool("Setup", "KillHumanWinLevel", M2Share.Config.IsKillHumanWinLevel);
            M2Share.Config.IsKillHumanWinLevel = ReadBool("Setup", "KillHumanWinLevel", M2Share.Config.IsKillHumanWinLevel);
            if (ReadInteger("Setup", "KilledLostLevel", -1) < 0)
                WriteBool("Setup", "KilledLostLevel", M2Share.Config.IsKilledLostLevel);
            M2Share.Config.IsKilledLostLevel = ReadBool("Setup", "KilledLostLevel", M2Share.Config.IsKilledLostLevel);
            if (ReadInteger("Setup", "KillHumanWinLevelPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanWinLevelPoint", M2Share.Config.KillHumanWinLevel);
            M2Share.Config.KillHumanWinLevel = ReadInteger("Setup", "KillHumanWinLevelPoint", M2Share.Config.KillHumanWinLevel);
            if (ReadInteger("Setup", "KilledLostLevelPoint", -1) < 0)
                WriteInteger("Setup", "KilledLostLevelPoint", M2Share.Config.KilledLostLevel);
            M2Share.Config.KilledLostLevel = ReadInteger("Setup", "KilledLostLevelPoint", M2Share.Config.KilledLostLevel);
            if (ReadInteger("Setup", "KillHumanWinExp", -1) < 0)
                WriteBool("Setup", "KillHumanWinExp", M2Share.Config.IsKillHumanWinExp);
            M2Share.Config.IsKillHumanWinExp = ReadBool("Setup", "KillHumanWinExp", M2Share.Config.IsKillHumanWinExp);
            if (ReadInteger("Setup", "KilledLostExp", -1) < 0)
                WriteBool("Setup", "KilledLostExp", M2Share.Config.IsKilledLostExp);
            M2Share.Config.IsKilledLostExp = ReadBool("Setup", "KilledLostExp", M2Share.Config.IsKilledLostExp);
            if (ReadInteger("Setup", "KillHumanWinExpPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanWinExpPoint", M2Share.Config.KillHumanWinExp);
            M2Share.Config.KillHumanWinExp = ReadInteger("Setup", "KillHumanWinExpPoint", M2Share.Config.KillHumanWinExp);
            if (ReadInteger("Setup", "KillHumanLostExpPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanLostExpPoint", M2Share.Config.KillHumanLostExp);
            M2Share.Config.KillHumanLostExp = ReadInteger("Setup", "KillHumanLostExpPoint", M2Share.Config.KillHumanLostExp);
            if (ReadInteger("Setup", "MonsterPowerRate", -1) < 0)
                WriteInteger("Setup", "MonsterPowerRate", M2Share.Config.MonsterPowerRate);
            M2Share.Config.MonsterPowerRate = ReadInteger("Setup", "MonsterPowerRate", M2Share.Config.MonsterPowerRate);
            if (ReadInteger("Setup", "ItemsPowerRate", -1) < 0)
                WriteInteger("Setup", "ItemsPowerRate", M2Share.Config.ItemsPowerRate);
            M2Share.Config.ItemsPowerRate = ReadInteger("Setup", "ItemsPowerRate", M2Share.Config.ItemsPowerRate);
            if (ReadInteger("Setup", "ItemsACPowerRate", -1) < 0)
                WriteInteger("Setup", "ItemsACPowerRate", M2Share.Config.ItemsACPowerRate);
            M2Share.Config.ItemsACPowerRate = ReadInteger("Setup", "ItemsACPowerRate", M2Share.Config.ItemsACPowerRate);
            if (ReadInteger("Setup", "SendOnlineCount", -1) < 0)
                WriteBool("Setup", "SendOnlineCount", M2Share.Config.SendOnlineCount);
            M2Share.Config.SendOnlineCount = ReadBool("Setup", "SendOnlineCount", M2Share.Config.SendOnlineCount);
            if (ReadInteger("Setup", "SendOnlineCountRate", -1) < 0)
                WriteInteger("Setup", "SendOnlineCountRate", M2Share.Config.SendOnlineCountRate);
            M2Share.Config.SendOnlineCountRate = ReadInteger("Setup", "SendOnlineCountRate", M2Share.Config.SendOnlineCountRate);
            if (ReadInteger("Setup", "SendOnlineTime", -1) < 0)
                WriteInteger("Setup", "SendOnlineTime", M2Share.Config.SendOnlineTime);
            M2Share.Config.SendOnlineTime = ReadInteger("Setup", "SendOnlineTime", M2Share.Config.SendOnlineTime);
            if (ReadInteger("Setup", "SaveHumanRcdTime", -1) < 0)
                WriteInteger("Setup", "SaveHumanRcdTime", M2Share.Config.SaveHumanRcdTime);
            M2Share.Config.SaveHumanRcdTime = ReadInteger("Setup", "SaveHumanRcdTime", M2Share.Config.SaveHumanRcdTime);
            if (ReadInteger("Setup", "HumanFreeDelayTime", -1) < 0)
                WriteInteger("Setup", "HumanFreeDelayTime", M2Share.Config.HumanFreeDelayTime);
            if (ReadInteger("Setup", "MakeGhostTime", -1) < 0)
                WriteInteger("Setup", "MakeGhostTime", M2Share.Config.MakeGhostTime);
            M2Share.Config.MakeGhostTime = ReadInteger("Setup", "MakeGhostTime", M2Share.Config.MakeGhostTime);
            if (ReadInteger("Setup", "ClearDropOnFloorItemTime", -1) < 0)
                WriteInteger("Setup", "ClearDropOnFloorItemTime", M2Share.Config.ClearDropOnFloorItemTime);
            M2Share.Config.ClearDropOnFloorItemTime = ReadInteger("Setup", "ClearDropOnFloorItemTime", M2Share.Config.ClearDropOnFloorItemTime);
            if (ReadInteger("Setup", "FloorItemCanPickUpTime", -1) < 0)
                WriteInteger("Setup", "FloorItemCanPickUpTime", M2Share.Config.FloorItemCanPickUpTime);
            M2Share.Config.FloorItemCanPickUpTime = ReadInteger("Setup", "FloorItemCanPickUpTime", M2Share.Config.FloorItemCanPickUpTime);
            if (ReadInteger("Setup", "PasswordLockSystem", -1) < 0)
                WriteBool("Setup", "PasswordLockSystem", M2Share.Config.PasswordLockSystem);
            M2Share.Config.PasswordLockSystem = ReadBool("Setup", "PasswordLockSystem", M2Share.Config.PasswordLockSystem);
            if (ReadInteger("Setup", "PasswordLockDealAction", -1) < 0)
                WriteBool("Setup", "PasswordLockDealAction", M2Share.Config.LockDealAction);
            M2Share.Config.LockDealAction = ReadBool("Setup", "PasswordLockDealAction", M2Share.Config.LockDealAction);
            if (ReadInteger("Setup", "PasswordLockDropAction", -1) < 0)
                WriteBool("Setup", "PasswordLockDropAction", M2Share.Config.LockDropAction);
            M2Share.Config.LockDropAction = ReadBool("Setup", "PasswordLockDropAction", M2Share.Config.LockDropAction);
            if (ReadInteger("Setup", "PasswordLockGetBackItemAction", -1) < 0)
                WriteBool("Setup", "PasswordLockGetBackItemAction", M2Share.Config.LockGetBackItemAction);
            M2Share.Config.LockGetBackItemAction = ReadBool("Setup", "PasswordLockGetBackItemAction", M2Share.Config.LockGetBackItemAction);
            if (ReadInteger("Setup", "PasswordLockHumanLogin", -1) < 0)
                WriteBool("Setup", "PasswordLockHumanLogin", M2Share.Config.LockHumanLogin);
            M2Share.Config.LockHumanLogin = ReadBool("Setup", "PasswordLockHumanLogin", M2Share.Config.LockHumanLogin);
            if (ReadInteger("Setup", "PasswordLockWalkAction", -1) < 0)
                WriteBool("Setup", "PasswordLockWalkAction", M2Share.Config.LockWalkAction);
            M2Share.Config.LockWalkAction = ReadBool("Setup", "PasswordLockWalkAction", M2Share.Config.LockWalkAction);
            if (ReadInteger("Setup", "PasswordLockRunAction", -1) < 0)
                WriteBool("Setup", "PasswordLockRunAction", M2Share.Config.LockRunAction);
            M2Share.Config.LockRunAction = ReadBool("Setup", "PasswordLockRunAction", M2Share.Config.LockRunAction);
            if (ReadInteger("Setup", "PasswordLockHitAction", -1) < 0)
                WriteBool("Setup", "PasswordLockHitAction", M2Share.Config.LockHitAction);
            M2Share.Config.LockHitAction = ReadBool("Setup", "PasswordLockHitAction", M2Share.Config.LockHitAction);
            if (ReadInteger("Setup", "PasswordLockSpellAction", -1) < 0)
                WriteBool("Setup", "PasswordLockSpellAction", M2Share.Config.LockSpellAction);
            M2Share.Config.LockSpellAction = ReadBool("Setup", "PasswordLockSpellAction", M2Share.Config.LockSpellAction);
            if (ReadInteger("Setup", "PasswordLockSendMsgAction", -1) < 0)
                WriteBool("Setup", "PasswordLockSendMsgAction", M2Share.Config.LockSendMsgAction);
            M2Share.Config.LockSendMsgAction = ReadBool("Setup", "PasswordLockSendMsgAction", M2Share.Config.LockSendMsgAction);
            if (ReadInteger("Setup", "PasswordLockUserItemAction", -1) < 0)
                WriteBool("Setup", "PasswordLockUserItemAction", M2Share.Config.LockUserItemAction);
            M2Share.Config.LockUserItemAction = ReadBool("Setup", "PasswordLockUserItemAction", M2Share.Config.LockUserItemAction);
            if (ReadInteger("Setup", "PasswordLockInObModeAction", -1) < 0)
                WriteBool("Setup", "PasswordLockInObModeAction", M2Share.Config.LockInObModeAction);
            M2Share.Config.LockInObModeAction = ReadBool("Setup", "PasswordLockInObModeAction", M2Share.Config.LockInObModeAction);
            if (ReadInteger("Setup", "PasswordErrorKick", -1) < 0)
                WriteBool("Setup", "PasswordErrorKick", M2Share.Config.PasswordErrorKick);
            M2Share.Config.PasswordErrorKick = ReadBool("Setup", "PasswordErrorKick", M2Share.Config.PasswordErrorKick);
            if (ReadInteger("Setup", "PasswordErrorCountLock", -1) < 0)
                WriteInteger("Setup", "PasswordErrorCountLock", M2Share.Config.PasswordErrorCountLock);
            M2Share.Config.PasswordErrorCountLock = ReadInteger("Setup", "PasswordErrorCountLock", M2Share.Config.PasswordErrorCountLock);
            if (ReadInteger("Setup", "SoftVersionDate", -1) < 0)
                WriteInteger("Setup", "SoftVersionDate", M2Share.Config.SoftVersionDate);
            M2Share.Config.SoftVersionDate = ReadInteger("Setup", "SoftVersionDate", M2Share.Config.SoftVersionDate);
            nLoadInteger = ReadInteger("Setup", "CanOldClientLogon", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "CanOldClientLogon", M2Share.Config.CanOldClientLogon);
            else
                M2Share.Config.CanOldClientLogon = nLoadInteger == 1;
            if (ReadInteger("Setup", "ConsoleShowUserCountTime", -1) < 0)
                WriteInteger("Setup", "ConsoleShowUserCountTime", M2Share.Config.ConsoleShowUserCountTime);
            M2Share.Config.ConsoleShowUserCountTime = ReadInteger("Setup", "ConsoleShowUserCountTime", M2Share.Config.ConsoleShowUserCountTime);
            if (ReadInteger("Setup", "ShowLineNoticeTime", -1) < 0)
                WriteInteger("Setup", "ShowLineNoticeTime", M2Share.Config.ShowLineNoticeTime);
            M2Share.Config.ShowLineNoticeTime = ReadInteger("Setup", "ShowLineNoticeTime", M2Share.Config.ShowLineNoticeTime);
            if (ReadInteger("Setup", "LineNoticeColor", -1) < 0)
                WriteInteger("Setup", "LineNoticeColor", M2Share.Config.LineNoticeColor);
            M2Share.Config.LineNoticeColor = ReadInteger("Setup", "LineNoticeColor", M2Share.Config.LineNoticeColor);

            if (ReadInteger("Setup", "MaxHitMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxHitMsgCount", M2Share.Config.MaxHitMsgCount);
            M2Share.Config.MaxHitMsgCount = ReadInteger("Setup", "MaxHitMsgCount", M2Share.Config.MaxHitMsgCount);
            if (ReadInteger("Setup", "MaxSpellMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxSpellMsgCount", M2Share.Config.MaxSpellMsgCount);
            M2Share.Config.MaxSpellMsgCount = ReadInteger("Setup", "MaxSpellMsgCount", M2Share.Config.MaxSpellMsgCount);
            if (ReadInteger("Setup", "MaxRunMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxRunMsgCount", M2Share.Config.MaxRunMsgCount);
            M2Share.Config.MaxRunMsgCount = ReadInteger("Setup", "MaxRunMsgCount", M2Share.Config.MaxRunMsgCount);
            if (ReadInteger("Setup", "MaxWalkMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxWalkMsgCount", M2Share.Config.MaxWalkMsgCount);
            M2Share.Config.MaxWalkMsgCount = ReadInteger("Setup", "MaxWalkMsgCount", M2Share.Config.MaxWalkMsgCount);
            if (ReadInteger("Setup", "MaxTurnMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxTurnMsgCount", M2Share.Config.MaxTurnMsgCount);
            M2Share.Config.MaxTurnMsgCount = ReadInteger("Setup", "MaxTurnMsgCount", M2Share.Config.MaxTurnMsgCount);
            if (ReadInteger("Setup", "MaxSitDonwMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxSitDonwMsgCount", M2Share.Config.MaxSitDonwMsgCount);
            M2Share.Config.MaxSitDonwMsgCount = ReadInteger("Setup", "MaxSitDonwMsgCount", M2Share.Config.MaxSitDonwMsgCount);
            if (ReadInteger("Setup", "MaxDigUpMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxDigUpMsgCount", M2Share.Config.MaxDigUpMsgCount);
            M2Share.Config.MaxDigUpMsgCount = ReadInteger("Setup", "MaxDigUpMsgCount", M2Share.Config.MaxDigUpMsgCount);
            if (ReadInteger("Setup", "SpellSendUpdateMsg", -1) < 0)
                WriteBool("Setup", "SpellSendUpdateMsg", M2Share.Config.SpellSendUpdateMsg);
            M2Share.Config.SpellSendUpdateMsg = ReadBool("Setup", "SpellSendUpdateMsg", M2Share.Config.SpellSendUpdateMsg);
            if (ReadInteger("Setup", "ActionSendActionMsg", -1) < 0)
                WriteBool("Setup", "ActionSendActionMsg", M2Share.Config.ActionSendActionMsg);
            M2Share.Config.ActionSendActionMsg = ReadBool("Setup", "ActionSendActionMsg", M2Share.Config.ActionSendActionMsg);
            if (ReadInteger("Setup", "OverSpeedKickCount", -1) < 0)
                WriteInteger("Setup", "OverSpeedKickCount", M2Share.Config.OverSpeedKickCount);
            M2Share.Config.OverSpeedKickCount = ReadInteger("Setup", "OverSpeedKickCount", M2Share.Config.OverSpeedKickCount);
            if (ReadInteger("Setup", "DropOverSpeed", -1) < 0)
                WriteInteger("Setup", "DropOverSpeed", M2Share.Config.DropOverSpeed);
            M2Share.Config.DropOverSpeed = ReadInteger("Setup", "DropOverSpeed", M2Share.Config.DropOverSpeed);
            if (ReadInteger("Setup", "KickOverSpeed", -1) < 0)
                WriteBool("Setup", "KickOverSpeed", M2Share.Config.KickOverSpeed);
            M2Share.Config.KickOverSpeed = ReadBool("Setup", "KickOverSpeed", M2Share.Config.KickOverSpeed);
            nLoadInteger = ReadInteger("Setup", "SpeedControlMode", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "SpeedControlMode", M2Share.Config.SpeedControlMode);
            else
                M2Share.Config.SpeedControlMode = nLoadInteger;
            if (ReadInteger("Setup", "HitIntervalTime", -1) < 0)
                WriteInteger("Setup", "HitIntervalTime", M2Share.Config.HitIntervalTime);
            M2Share.Config.HitIntervalTime =
                ReadInteger("Setup", "HitIntervalTime", M2Share.Config.HitIntervalTime);
            if (ReadInteger("Setup", "MagicHitIntervalTime", -1) < 0)
                WriteInteger("Setup", "MagicHitIntervalTime", M2Share.Config.MagicHitIntervalTime);
            M2Share.Config.MagicHitIntervalTime = ReadInteger("Setup", "MagicHitIntervalTime", M2Share.Config.MagicHitIntervalTime);
            if (ReadInteger("Setup", "RunIntervalTime", -1) < 0)
                WriteInteger("Setup", "RunIntervalTime", M2Share.Config.RunIntervalTime);
            M2Share.Config.RunIntervalTime = ReadInteger("Setup", "RunIntervalTime", M2Share.Config.RunIntervalTime);
            if (ReadInteger("Setup", "WalkIntervalTime", -1) < 0)
                WriteInteger("Setup", "WalkIntervalTime", M2Share.Config.WalkIntervalTime);
            M2Share.Config.WalkIntervalTime = ReadInteger("Setup", "WalkIntervalTime", M2Share.Config.WalkIntervalTime);
            if (ReadInteger("Setup", "TurnIntervalTime", -1) < 0)
                WriteInteger("Setup", "TurnIntervalTime", M2Share.Config.TurnIntervalTime);
            M2Share.Config.TurnIntervalTime = ReadInteger("Setup", "TurnIntervalTime", M2Share.Config.TurnIntervalTime);
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
                WriteInteger("Setup", "ActionIntervalTime", M2Share.Config.ActionIntervalTime);
            else
                M2Share.Config.ActionIntervalTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "RunLongHitIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "RunLongHitIntervalTime", M2Share.Config.RunLongHitIntervalTime);
            else
                M2Share.Config.RunLongHitIntervalTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "RunHitIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "RunHitIntervalTime", M2Share.Config.RunHitIntervalTime);
            else
                M2Share.Config.RunHitIntervalTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "WalkHitIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "WalkHitIntervalTime", M2Share.Config.WalkHitIntervalTime);
            else
                M2Share.Config.WalkHitIntervalTime = nLoadInteger;
            nLoadInteger = ReadInteger("Setup", "RunMagicIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "RunMagicIntervalTime", M2Share.Config.RunMagicIntervalTime);
            else
                M2Share.Config.RunMagicIntervalTime = nLoadInteger;
            if (ReadInteger("Setup", "DisableStruck", -1) < 0)
                WriteBool("Setup", "DisableStruck", M2Share.Config.DisableStruck);
            M2Share.Config.DisableStruck =
                ReadBool("Setup", "DisableStruck", M2Share.Config.DisableStruck);
            if (ReadInteger("Setup", "DisableSelfStruck", -1) < 0)
                WriteBool("Setup", "DisableSelfStruck", M2Share.Config.DisableSelfStruck);
            M2Share.Config.DisableSelfStruck =
                ReadBool("Setup", "DisableSelfStruck", M2Share.Config.DisableSelfStruck);
            if (ReadInteger("Setup", "StruckTime", -1) < 0)
                WriteInteger("Setup", "StruckTime", M2Share.Config.StruckTime);
            M2Share.Config.StruckTime = ReadInteger("Setup", "StruckTime", M2Share.Config.StruckTime);
            nLoadInteger = ReadInteger("Setup", "AddUserItemNewValue", -1);
            if (nLoadInteger < 0)
            {
                WriteBool("Setup", "AddUserItemNewValue", M2Share.Config.AddUserItemNewValue);
            }
            else
            {
                M2Share.Config.AddUserItemNewValue = nLoadInteger == 1;
            }
            nLoadInteger = ReadInteger("Setup", "TestSpeedMode", -1);
            if (nLoadInteger < 0)
            {
                WriteBool("Setup", "TestSpeedMode", M2Share.Config.TestSpeedMode);
            }
            else
            {
                M2Share.Config.TestSpeedMode = nLoadInteger == 1;
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
                WriteInteger("Setup", "MPStoneIntervalTime", M2Share.Config.MpStoneIntervalTime);
            }
            M2Share.Config.MpStoneIntervalTime = ReadInteger("Setup", "MPStoneIntervalTime", M2Share.Config.MpStoneIntervalTime);
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
                WriteInteger("Setup", "WeaponMakeUnLuckRate", M2Share.Config.WeaponMakeUnLuckRate);
            }
            else
            {
                M2Share.Config.WeaponMakeUnLuckRate = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WeaponMakeLuckPoint1", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint1", M2Share.Config.WeaponMakeLuckPoint1);
            }
            else
            {
                M2Share.Config.WeaponMakeLuckPoint1 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WeaponMakeLuckPoint2", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint2", M2Share.Config.WeaponMakeLuckPoint2);
            }
            else
            {
                M2Share.Config.WeaponMakeLuckPoint2 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WeaponMakeLuckPoint3", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint3", M2Share.Config.WeaponMakeLuckPoint3);
            }
            else
            {
                M2Share.Config.WeaponMakeLuckPoint3 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WeaponMakeLuckPoint2Rate", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint2Rate", M2Share.Config.WeaponMakeLuckPoint2Rate);
            }
            else
            {
                M2Share.Config.WeaponMakeLuckPoint2Rate = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WeaponMakeLuckPoint3Rate", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint3Rate", M2Share.Config.WeaponMakeLuckPoint3Rate);
            }
            else
            {
                M2Share.Config.WeaponMakeLuckPoint3Rate = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "CheckUserItemPlace", -1);
            if (nLoadInteger < 0)
            {
                WriteBool("Setup", "CheckUserItemPlace", M2Share.Config.CheckUserItemPlace);
            }
            else
            {
                M2Share.Config.CheckUserItemPlace = nLoadInteger == 1;
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
                WriteInteger("Setup", "ProcessMonsterInterval", M2Share.Config.ProcessMonsterInterval);
            }
            else
            {
                M2Share.Config.ProcessMonsterInterval = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "ProcessMonsterMultiThreadLimit", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "ProcessMonsterMultiThreadLimit", M2Share.Config.ProcessMonsterMultiThreadLimit);
            }
            else
            {
                M2Share.Config.ProcessMonsterMultiThreadLimit = nLoadInteger;
            }
            if (ReadInteger("Setup", "StartCastleWarDays", -1) < 0)
            {
                WriteInteger("Setup", "StartCastleWarDays", M2Share.Config.StartCastleWarDays);
            }
            M2Share.Config.StartCastleWarDays = ReadInteger("Setup", "StartCastleWarDays", M2Share.Config.StartCastleWarDays);
            if (ReadInteger("Setup", "StartCastlewarTime", -1) < 0)
            {
                WriteInteger("Setup", "StartCastlewarTime", M2Share.Config.StartCastlewarTime);
            }
            M2Share.Config.StartCastlewarTime = ReadInteger("Setup", "StartCastlewarTime", M2Share.Config.StartCastlewarTime);
            if (ReadInteger("Setup", "ShowCastleWarEndMsgTime", -1) < 0)
            {
                WriteInteger("Setup", "ShowCastleWarEndMsgTime", M2Share.Config.ShowCastleWarEndMsgTime);
            }
            M2Share.Config.ShowCastleWarEndMsgTime = ReadInteger("Setup", "ShowCastleWarEndMsgTime", M2Share.Config.ShowCastleWarEndMsgTime);
            if (ReadInteger("Server", "ClickNPCTime", -1) < 0)
            {
                WriteInteger("Server", "ClickNPCTime", M2Share.Config.ClickNpcTime);
            }
            M2Share.Config.ClickNpcTime = ReadInteger("Server", "ClickNPCTime", M2Share.Config.ClickNpcTime);
            if (ReadInteger("Setup", "CastleWarTime", -1) < 0)
            {
                WriteInteger("Setup", "CastleWarTime", M2Share.Config.CastleWarTime);
            }
            M2Share.Config.CastleWarTime = ReadInteger("Setup", "CastleWarTime", M2Share.Config.CastleWarTime);
            nLoadInteger = ReadInteger("Setup", "GetCastleTime", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "GetCastleTime", M2Share.Config.GetCastleTime);
            }
            else
            {
                M2Share.Config.GetCastleTime = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "GuildWarTime", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "GuildWarTime", M2Share.Config.GuildWarTime);
            }
            else
            {
                M2Share.Config.GuildWarTime = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryCount", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryCount", M2Share.Config.WinLotteryCount);
            }
            else
            {
                M2Share.Config.WinLotteryCount = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "NoWinLotteryCount", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "NoWinLotteryCount", M2Share.Config.NoWinLotteryCount);
            }
            else
            {
                M2Share.Config.NoWinLotteryCount = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel1", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel1", M2Share.Config.WinLotteryLevel1);
            }
            else
            {
                M2Share.Config.WinLotteryLevel1 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel2", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel2", M2Share.Config.WinLotteryLevel2);
            }
            else
            {
                M2Share.Config.WinLotteryLevel2 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel3", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel3", M2Share.Config.WinLotteryLevel3);
            }
            else
            {
                M2Share.Config.WinLotteryLevel3 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel4", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel4", M2Share.Config.WinLotteryLevel4);
            }
            else
            {
                M2Share.Config.WinLotteryLevel4 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel5", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel5", M2Share.Config.WinLotteryLevel5);
            }
            else
            {
                M2Share.Config.WinLotteryLevel5 = nLoadInteger;
            }
            nLoadInteger = ReadInteger("Setup", "WinLotteryLevel6", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel6", M2Share.Config.WinLotteryLevel6);
            }
            else
            {
                M2Share.Config.WinLotteryLevel6 = nLoadInteger;
            }
        }

        /// <summary>
        /// 保存游戏变量和彩票中奖数据
        /// </summary>
        public void SaveVariable()
        {
            WriteInteger("Setup", "ItemNumber", M2Share.Config.ItemNumber);
            WriteInteger("Setup", "ItemNumberEx", M2Share.Config.ItemNumberEx);
            WriteInteger("Setup", "WinLotteryCount", M2Share.Config.WinLotteryCount);
            WriteInteger("Setup", "NoWinLotteryCount", M2Share.Config.NoWinLotteryCount);
            WriteInteger("Setup", "WinLotteryLevel1", M2Share.Config.WinLotteryLevel1);
            WriteInteger("Setup", "WinLotteryLevel2", M2Share.Config.WinLotteryLevel2);
            WriteInteger("Setup", "WinLotteryLevel3", M2Share.Config.WinLotteryLevel3);
            WriteInteger("Setup", "WinLotteryLevel4", M2Share.Config.WinLotteryLevel4);
            WriteInteger("Setup", "WinLotteryLevel5", M2Share.Config.WinLotteryLevel5);
            WriteInteger("Setup", "WinLotteryLevel6", M2Share.Config.WinLotteryLevel6);
        }
    }
}