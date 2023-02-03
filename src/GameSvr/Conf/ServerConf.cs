﻿using SystemModule.Common;

namespace GameSvr.Conf
{
    public class ServerConf : ConfigFile
    {
        public ServerConf(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            //数据库设置
            M2Share.Config.ConnctionString = ReadWriteString("DataBase", "ConnctionString", M2Share.Config.ConnctionString);
            //数据库类型设置
            M2Share.Config.sDBType = ReadWriteString("DataBase", "DbType", M2Share.Config.sDBType);
            // 服务器设置
            M2Share.ServerIndex = ReadWriteByte("Server", "ServerIndex", M2Share.ServerIndex);
            M2Share.Config.CloseCountdown = ReadWriteInteger("Server", "CloseCountdown", M2Share.Config.CloseCountdown);
            M2Share.Config.ServerName = ReadWriteString("Server", "ServerName", M2Share.Config.ServerName);
            M2Share.Config.nServerNumber = ReadWriteInteger("Server", "ServerNumber", M2Share.Config.nServerNumber);
            M2Share.Config.VentureServer = string.Compare(ReadWriteString("Server", "VentureServer", "FALSE"), "TRUE", StringComparison.OrdinalIgnoreCase) == 0;
            M2Share.Config.PayMentMode = ReadWriteByte("Server", "PayMentMode", M2Share.Config.PayMentMode);
            M2Share.Config.TestServer = string.Compare(ReadWriteString("Server", "TestServer", "FALSE"), "TRUE", StringComparison.OrdinalIgnoreCase) == 0;
            M2Share.Config.TestLevel = ReadWriteInteger("Server", "TestLevel", M2Share.Config.TestLevel);
            M2Share.Config.TestGold = ReadWriteInteger("Server", "TestGold", M2Share.Config.TestGold);
            M2Share.Config.TestUserLimit = ReadWriteInteger("Server", "TestServerUserLimit", M2Share.Config.TestUserLimit);
            M2Share.Config.ServiceMode = ReadWriteString("Server", "ServiceMode", "FALSE").CompareTo("TRUE") == 0;
            M2Share.Config.PveServer = ReadWriteString("Server", "NonPKServer", "FALSE").CompareTo("TRUE") == 0;
            M2Share.Config.ViewHackMessage = ReadWriteString("Server", "ViewHackMessage", HUtil32.BoolToStr(M2Share.Config.ViewHackMessage)).CompareTo("TRUE") == 0;
            M2Share.Config.ViewAdmissionFailure = ReadWriteString("Server", "ViewAdmissionFailure", HUtil32.BoolToStr(M2Share.Config.ViewAdmissionFailure)).CompareTo("TRUE") == 0;
            M2Share.Config.sGateAddr = ReadWriteString("Server", "GateAddr", M2Share.Config.sGateAddr);
            M2Share.Config.nGatePort = ReadWriteInteger("Server", "GatePort", M2Share.Config.nGatePort);
            M2Share.Config.sDBAddr = ReadWriteString("Server", "DBAddr", M2Share.Config.sDBAddr);
            M2Share.Config.nDBPort = ReadWriteInteger("Server", "DBPort", M2Share.Config.nDBPort);
            M2Share.Config.sIDSAddr = ReadWriteString("Server", "IDSAddr", M2Share.Config.sIDSAddr);
            M2Share.Config.nIDSPort = ReadWriteInteger("Server", "IDSPort", M2Share.Config.nIDSPort);
            M2Share.Config.MsgSrvAddr = ReadWriteString("Server", "MsgSrvAddr", M2Share.Config.MsgSrvAddr);
            M2Share.Config.MsgSrvPort = ReadWriteInteger("Server", "MsgSrvPort", M2Share.Config.MsgSrvPort);
            M2Share.Config.LogServerAddr = ReadWriteString("Server", "LogServerAddr", M2Share.Config.LogServerAddr);
            M2Share.Config.LogServerPort = ReadWriteInteger("Server", "LogServerPort", M2Share.Config.LogServerPort);
            M2Share.Config.DiscountForNightTime = ReadWriteString("Server", "DiscountForNightTime", HUtil32.BoolToStr(M2Share.Config.DiscountForNightTime)).CompareTo("TRUE".ToLower()) == 0;
            M2Share.Config.HalfFeeStart = ReadWriteInteger("Server", "HalfFeeStart", M2Share.Config.HalfFeeStart);
            M2Share.Config.HalfFeeEnd = ReadWriteInteger("Server", "HalfFeeEnd", M2Share.Config.HalfFeeEnd);
            M2Share.HumLimit = ReadWriteInteger("Server", "HumLimit", M2Share.HumLimit);
            M2Share.MonLimit = ReadWriteInteger("Server", "MonLimit", M2Share.MonLimit);
            M2Share.ZenLimit = ReadWriteInteger("Server", "ZenLimit", M2Share.ZenLimit);
            M2Share.NpcLimit = ReadWriteInteger("Server", "NpcLimit", M2Share.NpcLimit);
            M2Share.SocLimit = ReadWriteInteger("Server", "SocLimit", M2Share.SocLimit);
            M2Share.DecLimit = ReadWriteInteger("Server", "DecLimit", M2Share.DecLimit);
            M2Share.Config.SendBlock = ReadWriteInteger("Server", "SendBlock", M2Share.Config.SendBlock);
            M2Share.Config.CheckBlock = ReadWriteInteger("Server", "CheckBlock", M2Share.Config.CheckBlock);
            M2Share.SocCheckTimeOut = ReadWriteInteger("Server", "SocCheckTimeOut", M2Share.SocCheckTimeOut);
            M2Share.Config.AvailableBlock = ReadWriteInteger("Server", "AvailableBlock", M2Share.Config.AvailableBlock);
            M2Share.Config.GateLoad = ReadWriteInteger("Server", "GateLoad", M2Share.Config.GateLoad);
            M2Share.Config.UserFull = ReadWriteInteger("Server", "UserFull", M2Share.Config.UserFull);
            M2Share.Config.ZenFastStep = ReadWriteInteger("Server", "ZenFastStep", M2Share.Config.ZenFastStep);
            M2Share.Config.ProcessMonstersTime = ReadWriteInteger("Server", "ProcessMonstersTime", M2Share.Config.ProcessMonstersTime);
            M2Share.Config.RegenMonstersTime = ReadWriteInteger("Server", "RegenMonstersTime", M2Share.Config.RegenMonstersTime);
            M2Share.Config.HumanGetMsgTime = ReadWriteInteger("Server", "HumanGetMsgTimeLimit", M2Share.Config.HumanGetMsgTime);
            M2Share.Config.BaseDir = ReadWriteString("Share", "BaseDir", M2Share.Config.BaseDir);
            M2Share.Config.GuildDir = ReadWriteString("Share", "GuildDir", M2Share.Config.GuildDir);
            M2Share.Config.GuildFile = ReadWriteString("Share", "GuildFile", M2Share.Config.GuildFile);
            M2Share.Config.VentureDir = ReadWriteString("Share", "VentureDir", M2Share.Config.VentureDir);
            M2Share.Config.ConLogDir = ReadWriteString("Share", "ConLogDir", M2Share.Config.ConLogDir);
            M2Share.Config.CastleDir = ReadWriteString("Share", "CastleDir", M2Share.Config.CastleDir);
            M2Share.Config.CastleFile = ReadWriteString("Share", "CastleFile", M2Share.Config.CastleDir + "List.txt");
            M2Share.Config.EnvirDir = ReadWriteString("Share", "EnvirDir", M2Share.Config.EnvirDir);
            M2Share.Config.MapDir = ReadWriteString("Share", "MapDir", M2Share.Config.MapDir);
            M2Share.Config.NoticeDir = ReadWriteString("Share", "NoticeDir", M2Share.Config.NoticeDir);
            M2Share.Config.LogDir = ReadWriteString("Share", "LogDir", M2Share.Config.LogDir);
            string sLoadString = string.Empty;
            // ============================================================================
            // 名称设置
            M2Share.Config.HealSkill = ReadWriteString("Names", "HealSkill", M2Share.Config.HealSkill);
            M2Share.Config.FireBallSkill = ReadWriteString("Names", "FireBallSkill", M2Share.Config.FireBallSkill);
            M2Share.Config.ClothsMan = ReadWriteString("Names", "ClothsMan", M2Share.Config.ClothsMan);
            M2Share.Config.ClothsWoman = ReadWriteString("Names", "ClothsWoman", M2Share.Config.ClothsWoman);
            M2Share.Config.WoodenSword = ReadWriteString("Names", "WoodenSword", M2Share.Config.WoodenSword);
            M2Share.Config.Candle = ReadWriteString("Names", "Candle", M2Share.Config.Candle);
            M2Share.Config.BasicDrug = ReadWriteString("Names", "BasicDrug", M2Share.Config.BasicDrug);
            M2Share.Config.GoldStone = ReadWriteString("Names", "GoldStone", M2Share.Config.GoldStone);
            M2Share.Config.SilverStone = ReadWriteString("Names", "SilverStone", M2Share.Config.SilverStone);
            M2Share.Config.SteelStone = ReadWriteString("Names", "SteelStone", M2Share.Config.SteelStone);
            M2Share.Config.CopperStone = ReadWriteString("Names", "CopperStone", M2Share.Config.CopperStone);
            M2Share.Config.BlackStone = ReadWriteString("Names", "BlackStone", M2Share.Config.BlackStone);
            M2Share.Config.GemStone1 = ReadWriteString("Names", "Gem1Stone", M2Share.Config.GemStone1);
            M2Share.Config.GemStone2 = ReadWriteString("Names", "Gem2Stone", M2Share.Config.GemStone2);
            M2Share.Config.GemStone3 = ReadWriteString("Names", "Gem3Stone", M2Share.Config.GemStone3);
            M2Share.Config.GemStone4 = ReadWriteString("Names", "Gem4Stone", M2Share.Config.GemStone4);
            M2Share.Config.Zuma[0] = ReadWriteString("Names", "Zuma1", M2Share.Config.Zuma[0]);
            M2Share.Config.Zuma[1] = ReadWriteString("Names", "Zuma2", M2Share.Config.Zuma[1]);
            M2Share.Config.Zuma[2] = ReadWriteString("Names", "Zuma3", M2Share.Config.Zuma[2]);
            M2Share.Config.Zuma[3] = ReadWriteString("Names", "Zuma4", M2Share.Config.Zuma[3]);
            M2Share.Config.Bee = ReadWriteString("Names", "Bee", M2Share.Config.Bee);
            M2Share.Config.Spider = ReadWriteString("Names", "Spider", M2Share.Config.Spider);
            M2Share.Config.WomaHorn = ReadWriteString("Names", "WomaHorn", M2Share.Config.WomaHorn);
            M2Share.Config.ZumaPiece = ReadWriteString("Names", "ZumaPiece", M2Share.Config.ZumaPiece);
            M2Share.Config.Skeleton = ReadWriteString("Names", "Skeleton", M2Share.Config.Skeleton);
            M2Share.Config.Dragon = ReadWriteString("Names", "Dragon", M2Share.Config.Dragon);
            M2Share.Config.Dragon1 = ReadWriteString("Names", "Dragon1", M2Share.Config.Dragon1);
            M2Share.Config.Angel = ReadWriteString("Names", "Angel", M2Share.Config.Angel);
            M2Share.Config.GameGoldName = ReadWriteString("Names", "GameGold", M2Share.Config.GameGoldName);
            M2Share.Config.GamePointName = ReadWriteString("Names", "GamePoint", M2Share.Config.GamePointName);
            M2Share.Config.PayMentPointName = ReadWriteString("Names", "PayMentPointName", M2Share.Config.PayMentPointName);

            // ============================================================================
            // 游戏设置
            M2Share.Config.ItemNumber = ReadWriteInteger("Setup", "ItemNumber", M2Share.Config.ItemNumber);
            M2Share.Config.ItemNumber += M2Share.RandomNumber.Random(10000);
            M2Share.Config.ItemNumberEx = ReadWriteInteger("Setup", "ItemNumberEx", M2Share.Config.ItemNumberEx);
            M2Share.Config.ClientFile1 = ReadWriteString("Setup", "ClientFile1", M2Share.Config.ClientFile1);
            M2Share.Config.ClientFile2 = ReadWriteString("Setup", "ClientFile2", M2Share.Config.ClientFile2);
            M2Share.Config.ClientFile3 = ReadWriteString("Setup", "ClientFile3", M2Share.Config.ClientFile3);
            M2Share.Config.MonUpLvNeedKillBase = ReadWriteInteger("Setup", "MonUpLvNeedKillBase", M2Share.Config.MonUpLvNeedKillBase);
            if (ReadWriteInteger("Setup", "MonUpLvRate", -1) < 0)
            {
                WriteInteger("Setup", "MonUpLvRate", M2Share.Config.MonUpLvRate);
            }
            M2Share.Config.MonUpLvRate = ReadWriteInteger("Setup", "MonUpLvRate", M2Share.Config.MonUpLvRate);
            for (int i = 0; i < M2Share.Config.MonUpLvNeedKillCount.Length; i++)
            {
                if (ReadWriteInteger("Setup", "MonUpLvNeedKillCount" + i, -1) < 0)
                {
                    WriteInteger("Setup", "MonUpLvNeedKillCount" + i, M2Share.Config.MonUpLvNeedKillCount[i]);
                }
                M2Share.Config.MonUpLvNeedKillCount[i] = ReadWriteInteger("Setup", "MonUpLvNeedKillCount" + i, M2Share.Config.MonUpLvNeedKillCount[i]);
            }
            for (int i = 0; i < M2Share.Config.SlaveColor.Length; i++)
            {
                if (ReadWriteInteger("Setup", "SlaveColor" + i, -1) < 0)
                {
                    WriteInteger("Setup", "SlaveColor" + i, M2Share.Config.SlaveColor[i]);
                }
                M2Share.Config.SlaveColor[i] = ReadWriteByte("Setup", "SlaveColor" + i, M2Share.Config.SlaveColor[i]);
            }

            M2Share.Config.HomeMap = ReadWriteString("Setup", "HomeMap", M2Share.Config.HomeMap);
            M2Share.Config.HomeX = ReadWriteInt16("Setup", "HomeX", M2Share.Config.HomeX);
            M2Share.Config.HomeY = ReadWriteInt16("Setup", "HomeY", M2Share.Config.HomeY);
            M2Share.Config.RedHomeMap = ReadWriteString("Setup", "RedHomeMap", M2Share.Config.RedHomeMap);
            M2Share.Config.RedHomeX = ReadWriteInt16("Setup", "RedHomeX", M2Share.Config.RedHomeX);
            M2Share.Config.RedHomeY = ReadWriteInt16("Setup", "RedHomeY", M2Share.Config.RedHomeY);
            M2Share.Config.RedDieHomeMap = ReadWriteString("Setup", "RedDieHomeMap", M2Share.Config.RedDieHomeMap);
            M2Share.Config.RedDieHomeX = ReadWriteInt16("Setup", "RedDieHomeX", M2Share.Config.RedDieHomeX);
            M2Share.Config.RedDieHomeY = ReadWriteInt16("Setup", "RedDieHomeY", M2Share.Config.RedDieHomeY);
            M2Share.Config.JobHomePoint = ReadWriteBool("Setup", "JobHomePointSystem", M2Share.Config.JobHomePoint);
            M2Share.Config.WarriorHomeMap = ReadWriteString("Setup", "WarriorHomeMap", M2Share.Config.WarriorHomeMap);
            M2Share.Config.WarriorHomeX = ReadWriteInt16("Setup", "WarriorHomeX", M2Share.Config.WarriorHomeX);
            M2Share.Config.WarriorHomeY = ReadWriteInt16("Setup", "WarriorHomeY", M2Share.Config.WarriorHomeY);
            M2Share.Config.WizardHomeMap = ReadWriteString("Setup", "WizardHomeMap", M2Share.Config.WizardHomeMap);
            M2Share.Config.WizardHomeX = ReadWriteInt16("Setup", "WizardHomeX", M2Share.Config.WizardHomeX);
            M2Share.Config.WizardHomeY = ReadWriteInt16("Setup", "WizardHomeY", M2Share.Config.WizardHomeY);
            M2Share.Config.TaoistHomeMap = ReadWriteString("Setup", "TaoistHomeMap", M2Share.Config.TaoistHomeMap);
            M2Share.Config.TaoistHomeX = ReadWriteInt16("Setup", "TaoistHomeX", M2Share.Config.TaoistHomeX);
            M2Share.Config.TaoistHomeY = ReadWriteInt16("Setup", "TaoistHomeY", M2Share.Config.TaoistHomeY);

            int nLoadInteger = 0;
            M2Share.Config.HealthFillTime = ReadWriteInteger("Setup", "HealthFillTime", M2Share.Config.HealthFillTime);
            M2Share.Config.SpellFillTime = ReadWriteInteger("Setup", "SpellFillTime", M2Share.Config.SpellFillTime);
            M2Share.Config.DecPkPointTime = ReadWriteInteger("Setup", "DecPkPointTime", M2Share.Config.DecPkPointTime);
            M2Share.Config.DecPkPointCount = ReadWriteInteger("Setup", "DecPkPointCount", M2Share.Config.DecPkPointCount);
            M2Share.Config.dwPKFlagTime = ReadWriteInteger("Setup", "PKFlagTime", M2Share.Config.dwPKFlagTime);
            M2Share.Config.KillHumanAddPKPoint = ReadWriteInteger("Setup", "KillHumanAddPKPoint", M2Share.Config.KillHumanAddPKPoint);
            M2Share.Config.KillHumanDecLuckPoint = ReadWriteInteger("Setup", "KillHumanDecLuckPoint", M2Share.Config.KillHumanDecLuckPoint);
            M2Share.Config.DecLightItemDrugTime = ReadWriteInteger("Setup", "DecLightItemDrugTime", M2Share.Config.DecLightItemDrugTime);
            M2Share.Config.SafeZoneSize = ReadWriteInteger("Setup", "SafeZoneSize", M2Share.Config.SafeZoneSize);
            M2Share.Config.StartPointSize = ReadWriteInteger("Setup", "StartPointSize", M2Share.Config.StartPointSize);

            for (int i = 0; i < M2Share.Config.ReNewNameColor.Length; i++)
            {
                if (ReadWriteInteger("Setup", "ReNewNameColor" + i, -1) < 0)
                {
                    WriteInteger("Setup", "ReNewNameColor" + i, M2Share.Config.ReNewNameColor[i]);
                }
                M2Share.Config.ReNewNameColor[i] = ReadWriteByte("Setup", "ReNewNameColor" + i, M2Share.Config.ReNewNameColor[i]);
            }

            M2Share.Config.ReNewNameColorTime = ReadWriteInteger("Setup", "ReNewNameColorTime", M2Share.Config.ReNewNameColorTime);
            M2Share.Config.ReNewChangeColor = ReadWriteBool("Setup", "ReNewChangeColor", M2Share.Config.ReNewChangeColor);
            M2Share.Config.ReNewLevelClearExp = ReadWriteBool("Setup", "ReNewLevelClearExp", M2Share.Config.ReNewLevelClearExp);
            M2Share.Config.BonusAbilofWarr.DC = ReadWriteUInt16("Setup", "BonusAbilofWarrDC", M2Share.Config.BonusAbilofWarr.DC);
            M2Share.Config.BonusAbilofWarr.MC = ReadWriteUInt16("Setup", "BonusAbilofWarrMC", M2Share.Config.BonusAbilofWarr.MC);
            M2Share.Config.BonusAbilofWarr.SC = ReadWriteUInt16("Setup", "BonusAbilofWarrSC", M2Share.Config.BonusAbilofWarr.SC);
            M2Share.Config.BonusAbilofWarr.AC = ReadWriteUInt16("Setup", "BonusAbilofWarrAC", M2Share.Config.BonusAbilofWarr.AC);
            M2Share.Config.BonusAbilofWarr.MAC = ReadWriteUInt16("Setup", "BonusAbilofWarrMAC", M2Share.Config.BonusAbilofWarr.MAC);
            M2Share.Config.BonusAbilofWarr.HP = ReadWriteUInt16("Setup", "BonusAbilofWarrHP", M2Share.Config.BonusAbilofWarr.HP);
            M2Share.Config.BonusAbilofWarr.MP = ReadWriteUInt16("Setup", "BonusAbilofWarrMP", M2Share.Config.BonusAbilofWarr.MP);
            M2Share.Config.BonusAbilofWarr.Hit = ReadWriteByte("Setup", "BonusAbilofWarrHit", M2Share.Config.BonusAbilofWarr.Hit);
            M2Share.Config.BonusAbilofWarr.Speed = ReadWriteInteger("Setup", "BonusAbilofWarrSpeed", M2Share.Config.BonusAbilofWarr.Speed);
            M2Share.Config.BonusAbilofWarr.Reserved = ReadWriteByte("Setup", "BonusAbilofWarrX2", M2Share.Config.BonusAbilofWarr.Reserved);
            M2Share.Config.BonusAbilofWizard.DC = ReadWriteUInt16("Setup", "BonusAbilofWizardDC", M2Share.Config.BonusAbilofWizard.DC);
            M2Share.Config.BonusAbilofWizard.MC = ReadWriteUInt16("Setup", "BonusAbilofWizardMC", M2Share.Config.BonusAbilofWizard.MC);
            M2Share.Config.BonusAbilofWizard.SC = ReadWriteUInt16("Setup", "BonusAbilofWizardSC", M2Share.Config.BonusAbilofWizard.SC);
            M2Share.Config.BonusAbilofWizard.AC = ReadWriteUInt16("Setup", "BonusAbilofWizardAC", M2Share.Config.BonusAbilofWizard.AC);
            M2Share.Config.BonusAbilofWizard.MAC = ReadWriteUInt16("Setup", "BonusAbilofWizardMAC", M2Share.Config.BonusAbilofWizard.MAC);
            M2Share.Config.BonusAbilofWizard.HP = ReadWriteUInt16("Setup", "BonusAbilofWizardHP", M2Share.Config.BonusAbilofWizard.HP);
            M2Share.Config.BonusAbilofWizard.MP = ReadWriteUInt16("Setup", "BonusAbilofWizardMP", M2Share.Config.BonusAbilofWizard.MP);
            M2Share.Config.BonusAbilofWizard.Hit = ReadWriteByte("Setup", "BonusAbilofWizardHit", M2Share.Config.BonusAbilofWizard.Hit);
            M2Share.Config.BonusAbilofWizard.Speed = ReadWriteInteger("Setup", "BonusAbilofWizardSpeed", M2Share.Config.BonusAbilofWizard.Speed);
            M2Share.Config.BonusAbilofWizard.Reserved = ReadWriteByte("Setup", "BonusAbilofWizardX2", M2Share.Config.BonusAbilofWizard.Reserved);
            M2Share.Config.BonusAbilofTaos.DC = ReadWriteUInt16("Setup", "BonusAbilofTaosDC", M2Share.Config.BonusAbilofTaos.DC);
            M2Share.Config.BonusAbilofTaos.MC = ReadWriteUInt16("Setup", "BonusAbilofTaosMC", M2Share.Config.BonusAbilofTaos.MC);
            M2Share.Config.BonusAbilofTaos.SC = ReadWriteUInt16("Setup", "BonusAbilofTaosSC", M2Share.Config.BonusAbilofTaos.SC);
            M2Share.Config.BonusAbilofTaos.AC = ReadWriteUInt16("Setup", "BonusAbilofTaosAC", M2Share.Config.BonusAbilofTaos.AC);
            M2Share.Config.BonusAbilofTaos.MAC = ReadWriteUInt16("Setup", "BonusAbilofTaosMAC", M2Share.Config.BonusAbilofTaos.MAC);
            M2Share.Config.BonusAbilofTaos.HP = ReadWriteUInt16("Setup", "BonusAbilofTaosHP", M2Share.Config.BonusAbilofTaos.HP);
            M2Share.Config.BonusAbilofTaos.MP = ReadWriteUInt16("Setup", "BonusAbilofTaosMP", M2Share.Config.BonusAbilofTaos.MP);
            M2Share.Config.BonusAbilofTaos.Hit = ReadWriteByte("Setup", "BonusAbilofTaosHit", M2Share.Config.BonusAbilofTaos.Hit);
            M2Share.Config.BonusAbilofTaos.Speed = ReadWriteInteger("Setup", "BonusAbilofTaosSpeed", M2Share.Config.BonusAbilofTaos.Speed);
            M2Share.Config.BonusAbilofTaos.Reserved = ReadWriteByte("Setup", "BonusAbilofTaosX2", M2Share.Config.BonusAbilofTaos.Reserved);
            M2Share.Config.NakedAbilofWarr.DC = ReadWriteUInt16("Setup", "NakedAbilofWarrDC", M2Share.Config.NakedAbilofWarr.DC);
            M2Share.Config.NakedAbilofWarr.MC = ReadWriteUInt16("Setup", "NakedAbilofWarrMC", M2Share.Config.NakedAbilofWarr.MC);
            M2Share.Config.NakedAbilofWarr.SC = ReadWriteUInt16("Setup", "NakedAbilofWarrSC", M2Share.Config.NakedAbilofWarr.SC);
            M2Share.Config.NakedAbilofWarr.AC = ReadWriteUInt16("Setup", "NakedAbilofWarrAC", M2Share.Config.NakedAbilofWarr.AC);
            M2Share.Config.NakedAbilofWarr.MAC = ReadWriteUInt16("Setup", "NakedAbilofWarrMAC", M2Share.Config.NakedAbilofWarr.MAC);
            M2Share.Config.NakedAbilofWarr.HP = ReadWriteUInt16("Setup", "NakedAbilofWarrHP", M2Share.Config.NakedAbilofWarr.HP);
            M2Share.Config.NakedAbilofWarr.MP = ReadWriteUInt16("Setup", "NakedAbilofWarrMP", M2Share.Config.NakedAbilofWarr.MP);
            M2Share.Config.NakedAbilofWarr.Hit = ReadWriteByte("Setup", "NakedAbilofWarrHit", M2Share.Config.NakedAbilofWarr.Hit);
            M2Share.Config.NakedAbilofWarr.Speed = ReadWriteInteger("Setup", "NakedAbilofWarrSpeed", M2Share.Config.NakedAbilofWarr.Speed);
            M2Share.Config.NakedAbilofWarr.Reserved = ReadWriteByte("Setup", "NakedAbilofWarrX2", M2Share.Config.NakedAbilofWarr.Reserved);
            M2Share.Config.NakedAbilofWizard.DC = ReadWriteUInt16("Setup", "NakedAbilofWizardDC", M2Share.Config.NakedAbilofWizard.DC);
            M2Share.Config.NakedAbilofWizard.MC = ReadWriteUInt16("Setup", "NakedAbilofWizardMC", M2Share.Config.NakedAbilofWizard.MC);
            M2Share.Config.NakedAbilofWizard.SC = ReadWriteUInt16("Setup", "NakedAbilofWizardSC", M2Share.Config.NakedAbilofWizard.SC);
            M2Share.Config.NakedAbilofWizard.AC = ReadWriteUInt16("Setup", "NakedAbilofWizardAC", M2Share.Config.NakedAbilofWizard.AC);
            M2Share.Config.NakedAbilofWizard.MAC = ReadWriteUInt16("Setup", "NakedAbilofWizardMAC", M2Share.Config.NakedAbilofWizard.MAC);
            M2Share.Config.NakedAbilofWizard.HP = ReadWriteUInt16("Setup", "NakedAbilofWizardHP", M2Share.Config.NakedAbilofWizard.HP);
            M2Share.Config.NakedAbilofWizard.MP = ReadWriteUInt16("Setup", "NakedAbilofWizardMP", M2Share.Config.NakedAbilofWizard.MP);
            M2Share.Config.NakedAbilofWizard.Hit = ReadWriteByte("Setup", "NakedAbilofWizardHit", M2Share.Config.NakedAbilofWizard.Hit);
            M2Share.Config.NakedAbilofWizard.Speed = ReadWriteInteger("Setup", "NakedAbilofWizardSpeed", M2Share.Config.NakedAbilofWizard.Speed);
            M2Share.Config.NakedAbilofWizard.Reserved = ReadWriteByte("Setup", "NakedAbilofWizardX2", M2Share.Config.NakedAbilofWizard.Reserved);
            M2Share.Config.NakedAbilofTaos.DC = ReadWriteUInt16("Setup", "NakedAbilofTaosDC", M2Share.Config.NakedAbilofTaos.DC);
            M2Share.Config.NakedAbilofTaos.MC = ReadWriteUInt16("Setup", "NakedAbilofTaosMC", M2Share.Config.NakedAbilofTaos.MC);
            M2Share.Config.NakedAbilofTaos.SC = ReadWriteUInt16("Setup", "NakedAbilofTaosSC", M2Share.Config.NakedAbilofTaos.SC);
            M2Share.Config.NakedAbilofTaos.AC = ReadWriteUInt16("Setup", "NakedAbilofTaosAC", M2Share.Config.NakedAbilofTaos.AC);
            M2Share.Config.NakedAbilofTaos.MAC = ReadWriteUInt16("Setup", "NakedAbilofTaosMAC", M2Share.Config.NakedAbilofTaos.MAC);
            M2Share.Config.NakedAbilofTaos.HP = ReadWriteUInt16("Setup", "NakedAbilofTaosHP", M2Share.Config.NakedAbilofTaos.HP);
            M2Share.Config.NakedAbilofTaos.MP = ReadWriteUInt16("Setup", "NakedAbilofTaosMP", M2Share.Config.NakedAbilofTaos.MP);
            M2Share.Config.NakedAbilofTaos.Hit = ReadWriteByte("Setup", "NakedAbilofTaosHit", M2Share.Config.NakedAbilofTaos.Hit);
            M2Share.Config.NakedAbilofTaos.Speed = ReadWriteInteger("Setup", "NakedAbilofTaosSpeed", M2Share.Config.NakedAbilofTaos.Speed);
            M2Share.Config.NakedAbilofTaos.Reserved = ReadWriteByte("Setup", "NakedAbilofTaosX2", M2Share.Config.NakedAbilofTaos.Reserved);
            M2Share.Config.GroupMembersMax = ReadWriteInteger("Setup", "GroupMembersMax", M2Share.Config.GroupMembersMax);
            M2Share.Config.WarrMon = ReadWriteInteger("Setup", "WarrAttackMon", M2Share.Config.WarrMon);
            M2Share.Config.WizardMon = ReadWriteInteger("Setup", "WizardAttackMon", M2Share.Config.WizardMon);
            M2Share.Config.TaosMon = ReadWriteInteger("Setup", "TaosAttackMon", M2Share.Config.TaosMon);
            M2Share.Config.MonHum = ReadWriteInteger("Setup", "MonAttackHum", M2Share.Config.MonHum);
            M2Share.Config.UPgradeWeaponGetBackTime = ReadWriteInteger("Setup", "UPgradeWeaponGetBackTime", M2Share.Config.UPgradeWeaponGetBackTime);
            M2Share.Config.ClearExpireUpgradeWeaponDays = ReadWriteInteger("Setup", "ClearExpireUpgradeWeaponDays", M2Share.Config.ClearExpireUpgradeWeaponDays);
            M2Share.Config.UpgradeWeaponPrice = ReadWriteInteger("Setup", "UpgradeWeaponPrice", M2Share.Config.UpgradeWeaponPrice);
            M2Share.Config.UpgradeWeaponMaxPoint = ReadWriteInteger("Setup", "UpgradeWeaponMaxPoint", M2Share.Config.UpgradeWeaponMaxPoint);
            M2Share.Config.UpgradeWeaponDCRate = ReadWriteInteger("Setup", "UpgradeWeaponDCRate", M2Share.Config.UpgradeWeaponDCRate);
            M2Share.Config.UpgradeWeaponDCTwoPointRate = ReadWriteInteger("Setup", "UpgradeWeaponDCTwoPointRate", M2Share.Config.UpgradeWeaponDCTwoPointRate);
            M2Share.Config.UpgradeWeaponDCThreePointRate = ReadWriteInteger("Setup", "UpgradeWeaponDCThreePointRate", M2Share.Config.UpgradeWeaponDCThreePointRate);
            M2Share.Config.UpgradeWeaponMCRate = ReadWriteInteger("Setup", "UpgradeWeaponMCRate", M2Share.Config.UpgradeWeaponMCRate);
            M2Share.Config.UpgradeWeaponMCTwoPointRate = ReadWriteInteger("Setup", "UpgradeWeaponMCTwoPointRate", M2Share.Config.UpgradeWeaponMCTwoPointRate);
            M2Share.Config.UpgradeWeaponMCThreePointRate = ReadWriteInteger("Setup", "UpgradeWeaponMCThreePointRate", M2Share.Config.UpgradeWeaponMCThreePointRate);
            M2Share.Config.UpgradeWeaponSCRate = ReadWriteInteger("Setup", "UpgradeWeaponSCRate", M2Share.Config.UpgradeWeaponSCRate);
            M2Share.Config.UpgradeWeaponSCTwoPointRate = ReadWriteInteger("Setup", "UpgradeWeaponSCTwoPointRate", M2Share.Config.UpgradeWeaponSCTwoPointRate);
            M2Share.Config.UpgradeWeaponSCThreePointRate = ReadWriteInteger("Setup", "UpgradeWeaponSCThreePointRate", M2Share.Config.UpgradeWeaponSCThreePointRate);
            M2Share.Config.BuildGuildPrice = ReadWriteInteger("Setup", "BuildGuild", M2Share.Config.BuildGuildPrice);
            M2Share.Config.MakeDurgPrice = ReadWriteInteger("Setup", "MakeDurg", M2Share.Config.MakeDurgPrice);
            M2Share.Config.GuildWarPrice = ReadWriteInteger("Setup", "GuildWarFee", M2Share.Config.GuildWarPrice);
            M2Share.Config.HireGuardPrice = ReadWriteInteger("Setup", "HireGuard", M2Share.Config.HireGuardPrice);
            M2Share.Config.HireArcherPrice = ReadWriteInteger("Setup", "HireArcher", M2Share.Config.HireArcherPrice);
            M2Share.Config.RepairDoorPrice = ReadWriteInteger("Setup", "RepairDoor", M2Share.Config.RepairDoorPrice);
            M2Share.Config.RepairWallPrice = ReadWriteInteger("Setup", "RepairWall", M2Share.Config.RepairWallPrice);
            M2Share.Config.CastleMemberPriceRate = ReadWriteInteger("Setup", "CastleMemberPriceRate", M2Share.Config.CastleMemberPriceRate);
            M2Share.Config.CastleGoldMax = ReadWriteInteger("Setup", "CastleGoldMax", M2Share.Config.CastleGoldMax);
            M2Share.Config.CastleOneDayGold = ReadWriteInteger("Setup", "CastleOneDayGold", M2Share.Config.CastleOneDayGold);
            M2Share.Config.CastleName = ReadWriteString("Setup", "CastleName", M2Share.Config.CastleName);
            M2Share.Config.CastleHomeMap = ReadWriteString("Setup", "CastleHomeMap", M2Share.Config.CastleHomeMap);
            M2Share.Config.CastleHomeX = ReadWriteInteger("Setup", "CastleHomeX", M2Share.Config.CastleHomeX);
            M2Share.Config.CastleHomeY = ReadWriteInteger("Setup", "CastleHomeY", M2Share.Config.CastleHomeY);
            M2Share.Config.CastleWarRangeX = ReadWriteInteger("Setup", "CastleWarRangeX", M2Share.Config.CastleWarRangeX);
            M2Share.Config.CastleWarRangeY = ReadWriteInteger("Setup", "CastleWarRangeY", M2Share.Config.CastleWarRangeY);
            M2Share.Config.CastleTaxRate = ReadWriteInteger("Setup", "CastleTaxRate", M2Share.Config.CastleTaxRate);
            M2Share.Config.GetAllNpcTax = ReadWriteBool("Setup", "CastleGetAllNpcTax", M2Share.Config.GetAllNpcTax);
            nLoadInteger = ReadWriteInteger("Setup", "GenMonRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "GenMonRate", M2Share.Config.MonGenRate);
            else
                M2Share.Config.MonGenRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "ProcessMonRandRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "ProcessMonRandRate", M2Share.Config.ProcessMonRandRate);
            else
                M2Share.Config.ProcessMonRandRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "ProcessMonLimitCount", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "ProcessMonLimitCount", M2Share.Config.ProcessMonLimitCount);
            else
                M2Share.Config.ProcessMonLimitCount = nLoadInteger;

            M2Share.Config.HumanMaxGold = ReadWriteInteger("Setup", "HumanMaxGold", M2Share.Config.HumanMaxGold);
            M2Share.Config.HumanTryModeMaxGold = ReadWriteInteger("Setup", "HumanTryModeMaxGold", M2Share.Config.HumanTryModeMaxGold);
            M2Share.Config.TryModeLevel = ReadWriteInteger("Setup", "TryModeLevel", M2Share.Config.TryModeLevel);
            M2Share.Config.TryModeUseStorage = ReadWriteBool("Setup", "TryModeUseStorage", M2Share.Config.TryModeUseStorage);
            M2Share.Config.ShutRedMsgShowGMName = ReadWriteBool("Setup", "ShutRedMsgShowGMName", M2Share.Config.ShutRedMsgShowGMName);
            M2Share.Config.ShowMakeItemMsg = ReadWriteBool("Setup", "ShowMakeItemMsg", M2Share.Config.ShowMakeItemMsg);
            M2Share.Config.ShowGuildName = ReadWriteBool("Setup", "ShowGuildName", M2Share.Config.ShowGuildName);
            M2Share.Config.ShowRankLevelName = ReadWriteBool("Setup", "ShowRankLevelName", M2Share.Config.ShowRankLevelName);
            M2Share.Config.MonSayMsg = ReadWriteBool("Setup", "MonSayMsg", M2Share.Config.MonSayMsg);
            M2Share.Config.SayMsgMaxLen = ReadWriteInteger("Setup", "SayMsgMaxLen", M2Share.Config.SayMsgMaxLen);
            M2Share.Config.SayMsgTime = ReadWriteInteger("Setup", "SayMsgTime", M2Share.Config.SayMsgTime);
            M2Share.Config.SayMsgCount = ReadWriteInteger("Setup", "SayMsgCount", M2Share.Config.SayMsgCount);
            M2Share.Config.DisableSayMsgTime = ReadWriteInteger("Setup", "DisableSayMsgTime", M2Share.Config.DisableSayMsgTime);
            M2Share.Config.SayRedMsgMaxLen = ReadWriteInteger("Setup", "SayRedMsgMaxLen", M2Share.Config.SayRedMsgMaxLen);
            M2Share.Config.CanShoutMsgLevel = ReadWriteInteger("Setup", "CanShoutMsgLevel", M2Share.Config.CanShoutMsgLevel);
            M2Share.Config.StartPermission = ReadWriteInteger("Setup", "StartPermission", M2Share.Config.StartPermission);
            M2Share.Config.SendRefMsgRange = (byte)ReadWriteInteger("Setup", "SendRefMsgRange", M2Share.Config.SendRefMsgRange);
            M2Share.Config.DecLampDura = ReadWriteBool("Setup", "DecLampDura", M2Share.Config.DecLampDura);
            M2Share.Config.HungerSystem = ReadWriteBool("Setup", "HungerSystem", M2Share.Config.HungerSystem);
            M2Share.Config.HungerDecHP = ReadWriteBool("Setup", "HungerDecHP", M2Share.Config.HungerDecHP);
            M2Share.Config.HungerDecPower = ReadWriteBool("Setup", "HungerDecPower", M2Share.Config.HungerDecPower);
            M2Share.Config.DiableHumanRun = ReadWriteBool("Setup", "DiableHumanRun", M2Share.Config.DiableHumanRun);
            M2Share.Config.boRunHuman = ReadWriteBool("Setup", "RunHuman", M2Share.Config.boRunHuman);
            M2Share.Config.boRunMon = ReadWriteBool("Setup", "RunMon", M2Share.Config.boRunMon);
            M2Share.Config.boRunNpc = ReadWriteBool("Setup", "RunNpc", M2Share.Config.boRunNpc);
            M2Share.Config.boRunGuard = ReadWriteBool("Setup", "RunGuard", M2Share.Config.boRunGuard);
            M2Share.Config.boWarDisHumRun = ReadWriteBool("Setup", "WarDisableHumanRun", M2Share.Config.boWarDisHumRun);
            M2Share.Config.boGMRunAll = ReadWriteBool("Setup", "GMRunAll", M2Share.Config.boGMRunAll);
            M2Share.Config.SkeletonCount = ReadWriteInteger("Setup", "SkeletonCount", M2Share.Config.SkeletonCount);
            for (int i = 0; i < M2Share.Config.SkeletonArray.Length; i++)
            {
                if (ReadWriteInteger("Setup", "SkeletonHumLevel" + i, -1) < 0)
                {
                    WriteInteger("Setup", "SkeletonHumLevel" + i, M2Share.Config.SkeletonArray[i].nHumLevel);
                }
                M2Share.Config.SkeletonArray[i].nHumLevel = ReadWriteInteger("Setup", "SkeletonHumLevel" + i, M2Share.Config.SkeletonArray[i].nHumLevel);
                if (ReadWriteString("Names", "Skeleton" + i, "") == "")
                {
                    WriteString("Names", "Skeleton" + i, M2Share.Config.SkeletonArray[i].sMonName);
                }
                M2Share.Config.SkeletonArray[i].sMonName = ReadWriteString("Names", "Skeleton" + i, M2Share.Config.SkeletonArray[i].sMonName);
                if (ReadWriteInteger("Setup", "SkeletonCount" + i, -1) < 0)
                {
                    WriteInteger("Setup", "SkeletonCount" + i, M2Share.Config.SkeletonArray[i].nCount);
                }
                M2Share.Config.SkeletonArray[i].nCount = ReadWriteInteger("Setup", "SkeletonCount" + i, M2Share.Config.SkeletonArray[i].nCount);
                if (ReadWriteInteger("Setup", "SkeletonLevel" + i, -1) < 0)
                {
                    WriteInteger("Setup", "SkeletonLevel" + i, M2Share.Config.SkeletonArray[i].nLevel);
                }
                M2Share.Config.SkeletonArray[i].nLevel = ReadWriteInteger("Setup", "SkeletonLevel" + i, M2Share.Config.SkeletonArray[i].nLevel);
            }
            M2Share.Config.DragonCount = ReadWriteInteger("Setup", "DragonCount", M2Share.Config.DragonCount);
            for (int i = 0; i < M2Share.Config.DragonArray.Length; i++)
            {
                if (ReadWriteInteger("Setup", "DragonHumLevel" + i, -1) < 0)
                {
                    WriteInteger("Setup", "DragonHumLevel" + i, M2Share.Config.DragonArray[i].nHumLevel);
                }
                M2Share.Config.DragonArray[i].nHumLevel = ReadWriteInteger("Setup", "DragonHumLevel" + i, M2Share.Config.DragonArray[i].nHumLevel);
                if (ReadWriteString("Names", "Dragon" + i, "") == "")
                {
                    WriteString("Names", "Dragon" + i, M2Share.Config.DragonArray[i].sMonName);
                }
                M2Share.Config.DragonArray[i].sMonName = ReadWriteString("Names", "Dragon" + i, M2Share.Config.DragonArray[i].sMonName);
                if (ReadWriteInteger("Setup", "DragonCount" + i, -1) < 0)
                {
                    WriteInteger("Setup", "DragonCount" + i, M2Share.Config.DragonArray[i].nCount);
                }
                M2Share.Config.DragonArray[i].nCount = ReadWriteInteger("Setup", "DragonCount" + i, M2Share.Config.DragonArray[i].nCount);
                if (ReadWriteInteger("Setup", "DragonLevel" + i, -1) < 0)
                {
                    WriteInteger("Setup", "DragonLevel" + i, M2Share.Config.DragonArray[i].nLevel);
                }
                M2Share.Config.DragonArray[i].nLevel = ReadWriteInteger("Setup", "DragonLevel" + i, M2Share.Config.DragonArray[i].nLevel);
            }
            M2Share.Config.TryDealTime = ReadWriteInteger("Setup", "TryDealTime", M2Share.Config.TryDealTime);
            M2Share.Config.DealOKTime = ReadWriteInteger("Setup", "DealOKTime", M2Share.Config.DealOKTime);
            M2Share.Config.CanNotGetBackDeal = ReadWriteBool("Setup", "CanNotGetBackDeal", M2Share.Config.CanNotGetBackDeal);
            M2Share.Config.DisableDeal = ReadWriteBool("Setup", "DisableDeal", M2Share.Config.DisableDeal);
            M2Share.Config.MasterOKLevel = ReadWriteInteger("Setup", "MasterOKLevel", M2Share.Config.MasterOKLevel);
            M2Share.Config.MasterOKCreditPoint = ReadWriteInteger("Setup", "MasterOKCreditPoint", M2Share.Config.MasterOKCreditPoint);
            M2Share.Config.nMasterOKBonusPoint = ReadWriteInteger("Setup", "MasterOKBonusPoint", M2Share.Config.nMasterOKBonusPoint);
            M2Share.Config.boPKLevelProtect = ReadWriteBool("Setup", "PKProtect", M2Share.Config.boPKLevelProtect);
            M2Share.Config.nPKProtectLevel = ReadWriteInteger("Setup", "PKProtectLevel", M2Share.Config.nPKProtectLevel);
            M2Share.Config.nRedPKProtectLevel = ReadWriteInteger("Setup", "RedPKProtectLevel", M2Share.Config.nRedPKProtectLevel);
            M2Share.Config.ItemPowerRate = ReadWriteInteger("Setup", "ItemPowerRate", M2Share.Config.ItemPowerRate);
            M2Share.Config.ItemExpRate = ReadWriteInteger("Setup", "ItemExpRate", M2Share.Config.ItemExpRate);
            M2Share.Config.ScriptGotoCountLimit = ReadWriteInteger("Setup", "ScriptGotoCountLimit", M2Share.Config.ScriptGotoCountLimit);
            M2Share.Config.btHearMsgFColor = ReadWriteByte("Setup", "HearMsgFColor", M2Share.Config.btHearMsgFColor);
            M2Share.Config.btHearMsgBColor = ReadWriteByte("Setup", "HearMsgBColor", M2Share.Config.btHearMsgBColor);
            M2Share.Config.btWhisperMsgFColor = ReadWriteByte("Setup", "WhisperMsgFColor", M2Share.Config.btWhisperMsgFColor);
            M2Share.Config.btWhisperMsgBColor = ReadWriteByte("Setup", "WhisperMsgBColor", M2Share.Config.btWhisperMsgBColor);
            M2Share.Config.btGMWhisperMsgFColor = ReadWriteByte("Setup", "GMWhisperMsgFColor", M2Share.Config.btGMWhisperMsgFColor);
            M2Share.Config.btGMWhisperMsgBColor = ReadWriteByte("Setup", "GMWhisperMsgBColor", M2Share.Config.btGMWhisperMsgBColor);
            M2Share.Config.CryMsgFColor = ReadWriteByte("Setup", "CryMsgFColor", M2Share.Config.CryMsgFColor);
            M2Share.Config.CryMsgBColor = ReadWriteByte("Setup", "CryMsgBColor", M2Share.Config.CryMsgBColor);
            M2Share.Config.GreenMsgFColor = ReadWriteByte("Setup", "GreenMsgFColor", M2Share.Config.GreenMsgFColor);
            M2Share.Config.GreenMsgBColor = ReadWriteByte("Setup", "GreenMsgBColor", M2Share.Config.GreenMsgBColor);
            M2Share.Config.BlueMsgFColor = ReadWriteByte("Setup", "BlueMsgFColor", M2Share.Config.BlueMsgFColor);
            M2Share.Config.BlueMsgBColor = ReadWriteByte("Setup", "BlueMsgBColor", M2Share.Config.BlueMsgBColor);
            M2Share.Config.RedMsgFColor = ReadWriteByte("Setup", "RedMsgFColor", M2Share.Config.RedMsgFColor);
            M2Share.Config.RedMsgBColor = ReadWriteByte("Setup", "RedMsgBColor", M2Share.Config.RedMsgBColor);
            M2Share.Config.GuildMsgFColor = ReadWriteByte("Setup", "GuildMsgFColor", M2Share.Config.GuildMsgFColor);
            M2Share.Config.GuildMsgBColor = ReadWriteByte("Setup", "GuildMsgBColor", M2Share.Config.GuildMsgBColor);
            M2Share.Config.GroupMsgFColor = ReadWriteByte("Setup", "GroupMsgFColor", M2Share.Config.GroupMsgFColor);
            M2Share.Config.GroupMsgBColor = ReadWriteByte("Setup", "GroupMsgBColor", M2Share.Config.GroupMsgBColor);
            M2Share.Config.CustMsgFColor = ReadWriteByte("Setup", "CustMsgFColor", M2Share.Config.CustMsgFColor);
            M2Share.Config.CustMsgBColor = ReadWriteByte("Setup", "CustMsgBColor", M2Share.Config.CustMsgBColor);
            M2Share.Config.MonRandomAddValue = ReadWriteInteger("Setup", "MonRandomAddValue", M2Share.Config.MonRandomAddValue);
            M2Share.Config.MakeRandomAddValue = ReadWriteInteger("Setup", "MakeRandomAddValue", M2Share.Config.MakeRandomAddValue);
            M2Share.Config.WeaponDCAddValueMaxLimit = ReadWriteInteger("Setup", "WeaponDCAddValueMaxLimit", M2Share.Config.WeaponDCAddValueMaxLimit);
            M2Share.Config.WeaponDCAddValueRate = ReadWriteInteger("Setup", "WeaponDCAddValueRate", M2Share.Config.WeaponDCAddValueRate);
            M2Share.Config.WeaponMCAddValueMaxLimit = ReadWriteInteger("Setup", "WeaponMCAddValueMaxLimit", M2Share.Config.WeaponMCAddValueMaxLimit);
            M2Share.Config.WeaponMCAddValueRate = ReadWriteInteger("Setup", "WeaponMCAddValueRate", M2Share.Config.WeaponMCAddValueRate);
            M2Share.Config.WeaponSCAddValueMaxLimit = ReadWriteInteger("Setup", "WeaponSCAddValueMaxLimit", M2Share.Config.WeaponSCAddValueMaxLimit);
            M2Share.Config.WeaponSCAddValueRate = ReadWriteInteger("Setup", "WeaponSCAddValueRate", M2Share.Config.WeaponSCAddValueRate);
            M2Share.Config.DressDCAddValueMaxLimit = ReadWriteInteger("Setup", "DressDCAddValueMaxLimit", M2Share.Config.DressDCAddValueMaxLimit);
            M2Share.Config.DressDCAddValueRate = ReadWriteInteger("Setup", "DressDCAddValueRate", M2Share.Config.DressDCAddValueRate);
            M2Share.Config.DressDCAddRate = ReadWriteInteger("Setup", "DressDCAddRate", M2Share.Config.DressDCAddRate);
            M2Share.Config.DressMCAddValueMaxLimit = ReadWriteInteger("Setup", "DressMCAddValueMaxLimit", M2Share.Config.DressMCAddValueMaxLimit);
            M2Share.Config.DressMCAddValueRate = ReadWriteInteger("Setup", "DressMCAddValueRate", M2Share.Config.DressMCAddValueRate);
            M2Share.Config.DressMCAddRate = ReadWriteInteger("Setup", "DressMCAddRate", M2Share.Config.DressMCAddRate);
            M2Share.Config.DressSCAddValueMaxLimit = ReadWriteInteger("Setup", "DressSCAddValueMaxLimit", M2Share.Config.DressSCAddValueMaxLimit);
            M2Share.Config.nDressSCAddValueRate = ReadWriteInteger("Setup", "DressSCAddValueRate", M2Share.Config.nDressSCAddValueRate);
            M2Share.Config.DressSCAddRate = ReadWriteInteger("Setup", "DressSCAddRate", M2Share.Config.DressSCAddRate);
            M2Share.Config.NeckLace19DCAddValueMaxLimit = ReadWriteInteger("Setup", "NeckLace19DCAddValueMaxLimit", M2Share.Config.NeckLace19DCAddValueMaxLimit);
            M2Share.Config.NeckLace19DCAddValueRate = ReadWriteInteger("Setup", "NeckLace19DCAddValueRate", M2Share.Config.NeckLace19DCAddValueRate);
            M2Share.Config.NeckLace19DCAddRate = ReadWriteInteger("Setup", "NeckLace19DCAddRate", M2Share.Config.NeckLace19DCAddRate);
            M2Share.Config.NeckLace19MCAddValueMaxLimit = ReadWriteInteger("Setup", "NeckLace19MCAddValueMaxLimit", M2Share.Config.NeckLace19MCAddValueMaxLimit);
            M2Share.Config.NeckLace19MCAddValueRate = ReadWriteInteger("Setup", "NeckLace19MCAddValueRate", M2Share.Config.NeckLace19MCAddValueRate);
            M2Share.Config.NeckLace19MCAddRate = ReadWriteInteger("Setup", "NeckLace19MCAddRate", M2Share.Config.NeckLace19MCAddRate);
            M2Share.Config.NeckLace19SCAddValueMaxLimit = ReadWriteInteger("Setup", "NeckLace19SCAddValueMaxLimit", M2Share.Config.NeckLace19SCAddValueMaxLimit);
            M2Share.Config.NeckLace19SCAddValueRate = ReadWriteInteger("Setup", "NeckLace19SCAddValueRate", M2Share.Config.NeckLace19SCAddValueRate);
            M2Share.Config.NeckLace19SCAddRate = ReadWriteInteger("Setup", "NeckLace19SCAddRate", M2Share.Config.NeckLace19SCAddRate);
            M2Share.Config.NeckLace202124DCAddValueMaxLimit = ReadWriteInteger("Setup", "NeckLace202124DCAddValueMaxLimit", M2Share.Config.NeckLace202124DCAddValueMaxLimit);
            M2Share.Config.NeckLace202124DCAddValueRate = ReadWriteInteger("Setup", "NeckLace202124DCAddValueRate", M2Share.Config.NeckLace202124DCAddValueRate);
            M2Share.Config.NeckLace202124DCAddRate = ReadWriteInteger("Setup", "NeckLace202124DCAddRate", M2Share.Config.NeckLace202124DCAddRate);
            M2Share.Config.NeckLace202124MCAddValueMaxLimit = ReadWriteInteger("Setup", "NeckLace202124MCAddValueMaxLimit", M2Share.Config.NeckLace202124MCAddValueMaxLimit);
            M2Share.Config.NeckLace202124MCAddValueRate = ReadWriteInteger("Setup", "NeckLace202124MCAddValueRate", M2Share.Config.NeckLace202124MCAddValueRate);
            M2Share.Config.NeckLace202124MCAddRate = ReadWriteInteger("Setup", "NeckLace202124MCAddRate", M2Share.Config.NeckLace202124MCAddRate);
            M2Share.Config.NeckLace202124SCAddValueMaxLimit = ReadWriteInteger("Setup", "NeckLace202124SCAddValueMaxLimit", M2Share.Config.NeckLace202124SCAddValueMaxLimit);
            M2Share.Config.NeckLace202124SCAddValueRate = ReadWriteInteger("Setup", "NeckLace202124SCAddValueRate", M2Share.Config.NeckLace202124SCAddValueRate);
            M2Share.Config.NeckLace202124SCAddRate = ReadWriteInteger("Setup", "NeckLace202124SCAddRate", M2Share.Config.NeckLace202124SCAddRate);
            M2Share.Config.ArmRing26DCAddValueMaxLimit = ReadWriteInteger("Setup", "ArmRing26DCAddValueMaxLimit", M2Share.Config.ArmRing26DCAddValueMaxLimit);
            M2Share.Config.ArmRing26DCAddValueRate = ReadWriteInteger("Setup", "ArmRing26DCAddValueRate", M2Share.Config.ArmRing26DCAddValueRate);
            M2Share.Config.ArmRing26DCAddRate = ReadWriteInteger("Setup", "ArmRing26DCAddRate", M2Share.Config.ArmRing26DCAddRate);
            M2Share.Config.ArmRing26MCAddValueMaxLimit = ReadWriteInteger("Setup", "ArmRing26MCAddValueMaxLimit", M2Share.Config.ArmRing26MCAddValueMaxLimit);
            M2Share.Config.ArmRing26MCAddValueRate = ReadWriteInteger("Setup", "ArmRing26MCAddValueRate", M2Share.Config.ArmRing26MCAddValueRate);
            M2Share.Config.ArmRing26MCAddRate = ReadWriteInteger("Setup", "ArmRing26MCAddRate", M2Share.Config.ArmRing26MCAddRate);
            M2Share.Config.ArmRing26SCAddValueMaxLimit = ReadWriteInteger("Setup", "ArmRing26SCAddValueMaxLimit", M2Share.Config.ArmRing26SCAddValueMaxLimit);
            M2Share.Config.ArmRing26SCAddValueRate = ReadWriteInteger("Setup", "ArmRing26SCAddValueRate", M2Share.Config.ArmRing26SCAddValueRate);
            M2Share.Config.ArmRing26SCAddRate = ReadWriteInteger("Setup", "ArmRing26SCAddRate", M2Share.Config.ArmRing26SCAddRate);
            M2Share.Config.Ring22DCAddValueMaxLimit = ReadWriteInteger("Setup", "Ring22DCAddValueMaxLimit", M2Share.Config.Ring22DCAddValueMaxLimit);
            M2Share.Config.Ring22DCAddValueRate = ReadWriteInteger("Setup", "Ring22DCAddValueRate", M2Share.Config.Ring22DCAddValueRate);
            M2Share.Config.Ring22DCAddRate = ReadWriteInteger("Setup", "Ring22DCAddRate", M2Share.Config.Ring22DCAddRate);
            M2Share.Config.Ring22MCAddValueMaxLimit = ReadWriteInteger("Setup", "Ring22MCAddValueMaxLimit", M2Share.Config.Ring22MCAddValueMaxLimit);
            M2Share.Config.Ring22MCAddValueRate = ReadWriteInteger("Setup", "Ring22MCAddValueRate", M2Share.Config.Ring22MCAddValueRate);
            M2Share.Config.Ring22MCAddRate = ReadWriteInteger("Setup", "Ring22MCAddRate", M2Share.Config.Ring22MCAddRate);
            M2Share.Config.Ring22SCAddValueMaxLimit = ReadWriteInteger("Setup", "Ring22SCAddValueMaxLimit", M2Share.Config.Ring22SCAddValueMaxLimit);
            M2Share.Config.Ring22SCAddValueRate = ReadWriteInteger("Setup", "Ring22SCAddValueRate", M2Share.Config.Ring22SCAddValueRate);
            M2Share.Config.Ring22SCAddRate = ReadWriteInteger("Setup", "Ring22SCAddRate", M2Share.Config.Ring22SCAddRate);
            M2Share.Config.Ring23DCAddValueMaxLimit = ReadWriteInteger("Setup", "Ring23DCAddValueMaxLimit", M2Share.Config.Ring23DCAddValueMaxLimit);
            M2Share.Config.Ring23DCAddValueRate = ReadWriteInteger("Setup", "Ring23DCAddValueRate", M2Share.Config.Ring23DCAddValueRate);
            M2Share.Config.Ring23DCAddRate = ReadWriteInteger("Setup", "Ring23DCAddRate", M2Share.Config.Ring23DCAddRate);
            M2Share.Config.Ring23MCAddValueMaxLimit = ReadWriteInteger("Setup", "Ring23MCAddValueMaxLimit", M2Share.Config.Ring23MCAddValueMaxLimit);
            M2Share.Config.Ring23MCAddValueRate = ReadWriteInteger("Setup", "Ring23MCAddValueRate", M2Share.Config.Ring23MCAddValueRate);
            M2Share.Config.Ring23MCAddRate = ReadWriteInteger("Setup", "Ring23MCAddRate", M2Share.Config.Ring23MCAddRate);
            M2Share.Config.Ring23SCAddValueMaxLimit = ReadWriteInteger("Setup", "Ring23SCAddValueMaxLimit", M2Share.Config.Ring23SCAddValueMaxLimit);
            M2Share.Config.Ring23SCAddValueRate = ReadWriteInteger("Setup", "Ring23SCAddValueRate", M2Share.Config.Ring23SCAddValueRate);
            M2Share.Config.Ring23SCAddRate = ReadWriteInteger("Setup", "Ring23SCAddRate", M2Share.Config.Ring23SCAddRate);
            M2Share.Config.HelMetDCAddValueMaxLimit = ReadWriteInteger("Setup", "HelMetDCAddValueMaxLimit", M2Share.Config.HelMetDCAddValueMaxLimit);
            M2Share.Config.HelMetDCAddValueRate = ReadWriteInteger("Setup", "HelMetDCAddValueRate", M2Share.Config.HelMetDCAddValueRate);
            M2Share.Config.HelMetDCAddRate = ReadWriteInteger("Setup", "HelMetDCAddRate", M2Share.Config.HelMetDCAddRate);
            M2Share.Config.HelMetMCAddValueMaxLimit = ReadWriteInteger("Setup", "HelMetMCAddValueMaxLimit", M2Share.Config.HelMetMCAddValueMaxLimit);
            M2Share.Config.HelMetMCAddValueRate = ReadWriteInteger("Setup", "HelMetMCAddValueRate", M2Share.Config.HelMetMCAddValueRate);
            M2Share.Config.HelMetMCAddRate = ReadWriteInteger("Setup", "HelMetMCAddRate", M2Share.Config.HelMetMCAddRate);
            M2Share.Config.HelMetSCAddValueMaxLimit = ReadWriteInteger("Setup", "HelMetSCAddValueMaxLimit", M2Share.Config.HelMetSCAddValueMaxLimit);
            M2Share.Config.HelMetSCAddValueRate = ReadWriteInteger("Setup", "HelMetSCAddValueRate", M2Share.Config.HelMetSCAddValueRate);
            M2Share.Config.HelMetSCAddRate = ReadWriteInteger("Setup", "HelMetSCAddRate", M2Share.Config.HelMetSCAddRate);
            M2Share.Config.UnknowHelMetACAddRate = ReadWriteInteger("Setup", "UnknowHelMetACAddRate", M2Share.Config.UnknowHelMetACAddRate);
            M2Share.Config.UnknowHelMetACAddValueMaxLimit = ReadWriteInteger("Setup", "UnknowHelMetACAddValueMaxLimit", M2Share.Config.UnknowHelMetACAddValueMaxLimit);

            M2Share.Config.UnknowHelMetMACAddRate = ReadWriteInteger("Setup", "UnknowHelMetMACAddRate", M2Share.Config.UnknowHelMetMACAddRate);
            M2Share.Config.UnknowHelMetMACAddValueMaxLimit = ReadWriteInteger("Setup", "UnknowHelMetMACAddValueMaxLimit", M2Share.Config.UnknowHelMetMACAddValueMaxLimit);
            M2Share.Config.UnknowHelMetDCAddRate = ReadWriteInteger("Setup", "UnknowHelMetDCAddRate", M2Share.Config.UnknowHelMetDCAddRate);
            M2Share.Config.UnknowHelMetDCAddValueMaxLimit = ReadWriteInteger("Setup", "UnknowHelMetDCAddValueMaxLimit", M2Share.Config.UnknowHelMetDCAddValueMaxLimit);
            M2Share.Config.UnknowHelMetMCAddRate = ReadWriteInteger("Setup", "UnknowHelMetMCAddRate", M2Share.Config.UnknowHelMetMCAddRate);
            M2Share.Config.UnknowHelMetMCAddValueMaxLimit = ReadWriteInteger("Setup", "UnknowHelMetMCAddValueMaxLimit", M2Share.Config.UnknowHelMetMCAddValueMaxLimit);
            M2Share.Config.UnknowHelMetSCAddRate = ReadWriteInteger("Setup", "UnknowHelMetSCAddRate", M2Share.Config.UnknowHelMetSCAddRate);
            M2Share.Config.UnknowHelMetSCAddValueMaxLimit = ReadWriteInteger("Setup", "UnknowHelMetSCAddValueMaxLimit", M2Share.Config.UnknowHelMetSCAddValueMaxLimit);
           
            nLoadInteger = ReadWriteInteger("Setup", "UnknowNecklaceACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceACAddRate", M2Share.Config.UnknowNecklaceACAddRate);
            else
                M2Share.Config.UnknowNecklaceACAddRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowNecklaceACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceACAddValueMaxLimit", M2Share.Config.UnknowNecklaceACAddValueMaxLimit);
            else
                M2Share.Config.UnknowNecklaceACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowNecklaceMACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMACAddRate", M2Share.Config.UnknowNecklaceMACAddRate);
            else
                M2Share.Config.UnknowNecklaceMACAddRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowNecklaceMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMACAddValueMaxLimit", M2Share.Config.UnknowNecklaceMACAddValueMaxLimit);
            else
                M2Share.Config.UnknowNecklaceMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowNecklaceDCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceDCAddRate", M2Share.Config.UnknowNecklaceDCAddRate);
            else
                M2Share.Config.UnknowNecklaceDCAddRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowNecklaceDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceDCAddValueMaxLimit", M2Share.Config.UnknowNecklaceDCAddValueMaxLimit);
            else
                M2Share.Config.UnknowNecklaceDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowNecklaceMCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMCAddRate", M2Share.Config.UnknowNecklaceMCAddRate);
            else
                M2Share.Config.UnknowNecklaceMCAddRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowNecklaceMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceMCAddValueMaxLimit", M2Share.Config.UnknowNecklaceMCAddValueMaxLimit);
            else
                M2Share.Config.UnknowNecklaceMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowNecklaceSCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceSCAddRate", M2Share.Config.UnknowNecklaceSCAddRate);
            else
                M2Share.Config.UnknowNecklaceSCAddRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowNecklaceSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowNecklaceSCAddValueMaxLimit", M2Share.Config.UnknowNecklaceSCAddValueMaxLimit);
            else
                M2Share.Config.UnknowNecklaceSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowRingACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingACAddRate", M2Share.Config.UnknowRingACAddRate);
            else
                M2Share.Config.UnknowRingACAddRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowRingACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingACAddValueMaxLimit", M2Share.Config.UnknowRingACAddValueMaxLimit);
            else
                M2Share.Config.UnknowRingACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowRingMACAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMACAddRate", M2Share.Config.UnknowRingMACAddRate);
            else
                M2Share.Config.UnknowRingMACAddRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowRingMACAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMACAddValueMaxLimit", M2Share.Config.UnknowRingMACAddValueMaxLimit);
            else
                M2Share.Config.UnknowRingMACAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowRingDCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingDCAddRate", M2Share.Config.UnknowRingDCAddRate);
            else
                M2Share.Config.UnknowRingDCAddRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowRingDCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingDCAddValueMaxLimit", M2Share.Config.UnknowRingDCAddValueMaxLimit);
            else
                M2Share.Config.UnknowRingDCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowRingMCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMCAddRate", M2Share.Config.UnknowRingMCAddRate);
            else
                M2Share.Config.UnknowRingMCAddRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowRingMCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingMCAddValueMaxLimit", M2Share.Config.UnknowRingMCAddValueMaxLimit);
            else
                M2Share.Config.UnknowRingMCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowRingSCAddRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingSCAddRate", M2Share.Config.UnknowRingSCAddRate);
            else
                M2Share.Config.UnknowRingSCAddRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "UnknowRingSCAddValueMaxLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UnknowRingSCAddValueMaxLimit", M2Share.Config.UnknowRingSCAddValueMaxLimit);
            else
                M2Share.Config.UnknowRingSCAddValueMaxLimit = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "MonOneDropGoldCount", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MonOneDropGoldCount", M2Share.Config.MonOneDropGoldCount);
            else
                M2Share.Config.MonOneDropGoldCount = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "SendCurTickCount", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "SendCurTickCount", M2Share.Config.SendCurTickCount);
            else
                M2Share.Config.SendCurTickCount = nLoadInteger == 1;
            if (ReadWriteInteger("Setup", "MakeMineHitRate", -1) < 0)
                WriteInteger("Setup", "MakeMineHitRate", M2Share.Config.MakeMineHitRate);
            M2Share.Config.MakeMineHitRate = ReadWriteInteger("Setup", "MakeMineHitRate", M2Share.Config.MakeMineHitRate);
            if (ReadWriteInteger("Setup", "MakeMineRate", -1) < 0)
                WriteInteger("Setup", "MakeMineRate", M2Share.Config.MakeMineRate);
            M2Share.Config.MakeMineRate = ReadWriteInteger("Setup", "MakeMineRate", M2Share.Config.MakeMineRate);
            if (ReadWriteInteger("Setup", "StoneTypeRate", -1) < 0)
                WriteInteger("Setup", "StoneTypeRate", M2Share.Config.StoneTypeRate);
            M2Share.Config.StoneTypeRate = ReadWriteInteger("Setup", "StoneTypeRate", M2Share.Config.StoneTypeRate);
            if (ReadWriteInteger("Setup", "StoneTypeRateMin", -1) < 0)
                WriteInteger("Setup", "StoneTypeRateMin", M2Share.Config.StoneTypeRateMin);
            M2Share.Config.StoneTypeRateMin = ReadWriteInteger("Setup", "StoneTypeRateMin", M2Share.Config.StoneTypeRateMin);
            if (ReadWriteInteger("Setup", "GoldStoneMin", -1) < 0)
                WriteInteger("Setup", "GoldStoneMin", M2Share.Config.GoldStoneMin);
            M2Share.Config.GoldStoneMin = ReadWriteInteger("Setup", "GoldStoneMin", M2Share.Config.GoldStoneMin);
            if (ReadWriteInteger("Setup", "GoldStoneMax", -1) < 0)
                WriteInteger("Setup", "GoldStoneMax", M2Share.Config.GoldStoneMax);
            M2Share.Config.GoldStoneMax = ReadWriteInteger("Setup", "GoldStoneMax", M2Share.Config.GoldStoneMax);
            if (ReadWriteInteger("Setup", "SilverStoneMin", -1) < 0)
                WriteInteger("Setup", "SilverStoneMin", M2Share.Config.SilverStoneMin);
            M2Share.Config.SilverStoneMin = ReadWriteInteger("Setup", "SilverStoneMin", M2Share.Config.SilverStoneMin);
            if (ReadWriteInteger("Setup", "SilverStoneMax", -1) < 0)
                WriteInteger("Setup", "SilverStoneMax", M2Share.Config.SilverStoneMax);
            M2Share.Config.SilverStoneMax = ReadWriteInteger("Setup", "SilverStoneMax", M2Share.Config.SilverStoneMax);
            if (ReadWriteInteger("Setup", "SteelStoneMin", -1) < 0)
                WriteInteger("Setup", "SteelStoneMin", M2Share.Config.SteelStoneMin);
            M2Share.Config.SteelStoneMin = ReadWriteInteger("Setup", "SteelStoneMin", M2Share.Config.SteelStoneMin);
            if (ReadWriteInteger("Setup", "SteelStoneMax", -1) < 0)
                WriteInteger("Setup", "SteelStoneMax", M2Share.Config.SteelStoneMax);
            M2Share.Config.SteelStoneMax = ReadWriteInteger("Setup", "SteelStoneMax", M2Share.Config.SteelStoneMax);
            if (ReadWriteInteger("Setup", "BlackStoneMin", -1) < 0)
                WriteInteger("Setup", "BlackStoneMin", M2Share.Config.BlackStoneMin);
            M2Share.Config.BlackStoneMin = ReadWriteInteger("Setup", "BlackStoneMin", M2Share.Config.BlackStoneMin);
            if (ReadWriteInteger("Setup", "BlackStoneMax", -1) < 0)
                WriteInteger("Setup", "BlackStoneMax", M2Share.Config.BlackStoneMax);
            M2Share.Config.BlackStoneMax = ReadWriteInteger("Setup", "BlackStoneMax", M2Share.Config.BlackStoneMax);
            if (ReadWriteInteger("Setup", "StoneMinDura", -1) < 0)
                WriteInteger("Setup", "StoneMinDura", M2Share.Config.StoneMinDura);
            M2Share.Config.StoneMinDura = ReadWriteInteger("Setup", "StoneMinDura", M2Share.Config.StoneMinDura);
            if (ReadWriteInteger("Setup", "StoneGeneralDuraRate", -1) < 0)
                WriteInteger("Setup", "StoneGeneralDuraRate", M2Share.Config.StoneGeneralDuraRate);
            M2Share.Config.StoneGeneralDuraRate = ReadWriteInteger("Setup", "StoneGeneralDuraRate", M2Share.Config.StoneGeneralDuraRate);
            if (ReadWriteInteger("Setup", "StoneAddDuraRate", -1) < 0)
                WriteInteger("Setup", "StoneAddDuraRate", M2Share.Config.StoneAddDuraRate);
            M2Share.Config.StoneAddDuraRate = ReadWriteInteger("Setup", "StoneAddDuraRate", M2Share.Config.StoneAddDuraRate);
            if (ReadWriteInteger("Setup", "StoneAddDuraMax", -1) < 0)
                WriteInteger("Setup", "StoneAddDuraMax", M2Share.Config.StoneAddDuraMax);
            M2Share.Config.StoneAddDuraMax = ReadWriteInteger("Setup", "StoneAddDuraMax", M2Share.Config.StoneAddDuraMax);
            if (ReadWriteInteger("Setup", "WinLottery1Min", -1) < 0)
                WriteInteger("Setup", "WinLottery1Min", M2Share.Config.WinLottery1Min);
            M2Share.Config.WinLottery1Min = ReadWriteInteger("Setup", "WinLottery1Min", M2Share.Config.WinLottery1Min);
            if (ReadWriteInteger("Setup", "WinLottery1Max", -1) < 0)
                WriteInteger("Setup", "WinLottery1Max", M2Share.Config.WinLottery1Max);
            M2Share.Config.WinLottery1Max = ReadWriteInteger("Setup", "WinLottery1Max", M2Share.Config.WinLottery1Max);
            if (ReadWriteInteger("Setup", "WinLottery2Min", -1) < 0)
                WriteInteger("Setup", "WinLottery2Min", M2Share.Config.WinLottery2Min);
            M2Share.Config.WinLottery2Min = ReadWriteInteger("Setup", "WinLottery2Min", M2Share.Config.WinLottery2Min);
            if (ReadWriteInteger("Setup", "WinLottery2Max", -1) < 0)
                WriteInteger("Setup", "WinLottery2Max", M2Share.Config.WinLottery2Max);
            M2Share.Config.WinLottery2Max = ReadWriteInteger("Setup", "WinLottery2Max", M2Share.Config.WinLottery2Max);
            if (ReadWriteInteger("Setup", "WinLottery3Min", -1) < 0)
                WriteInteger("Setup", "WinLottery3Min", M2Share.Config.WinLottery3Min);
            M2Share.Config.WinLottery3Min = ReadWriteInteger("Setup", "WinLottery3Min", M2Share.Config.WinLottery3Min);
            if (ReadWriteInteger("Setup", "WinLottery3Max", -1) < 0)
                WriteInteger("Setup", "WinLottery3Max", M2Share.Config.WinLottery3Max);
            M2Share.Config.WinLottery3Max = ReadWriteInteger("Setup", "WinLottery3Max", M2Share.Config.WinLottery3Max);
            if (ReadWriteInteger("Setup", "WinLottery4Min", -1) < 0)
                WriteInteger("Setup", "WinLottery4Min", M2Share.Config.WinLottery4Min);
            M2Share.Config.WinLottery4Min = ReadWriteInteger("Setup", "WinLottery4Min", M2Share.Config.WinLottery4Min);
            if (ReadWriteInteger("Setup", "WinLottery4Max", -1) < 0)
                WriteInteger("Setup", "WinLottery4Max", M2Share.Config.WinLottery4Max);
            M2Share.Config.WinLottery4Max = ReadWriteInteger("Setup", "WinLottery4Max", M2Share.Config.WinLottery4Max);
            if (ReadWriteInteger("Setup", "WinLottery5Min", -1) < 0)
                WriteInteger("Setup", "WinLottery5Min", M2Share.Config.WinLottery5Min);
            M2Share.Config.WinLottery5Min = ReadWriteInteger("Setup", "WinLottery5Min", M2Share.Config.WinLottery5Min);
            if (ReadWriteInteger("Setup", "WinLottery5Max", -1) < 0)
                WriteInteger("Setup", "WinLottery5Max", M2Share.Config.WinLottery5Max);
            M2Share.Config.WinLottery5Max = ReadWriteInteger("Setup", "WinLottery5Max", M2Share.Config.WinLottery5Max);
            if (ReadWriteInteger("Setup", "WinLottery6Min", -1) < 0)
                WriteInteger("Setup", "WinLottery6Min", M2Share.Config.WinLottery6Min);
            M2Share.Config.WinLottery6Min = ReadWriteInteger("Setup", "WinLottery6Min", M2Share.Config.WinLottery6Min);
            if (ReadWriteInteger("Setup", "WinLottery6Max", -1) < 0)
                WriteInteger("Setup", "WinLottery6Max", M2Share.Config.WinLottery6Max);
            M2Share.Config.WinLottery6Max = ReadWriteInteger("Setup", "WinLottery6Max", M2Share.Config.WinLottery6Max);
            if (ReadWriteInteger("Setup", "WinLotteryRate", -1) < 0)
                WriteInteger("Setup", "WinLotteryRate", M2Share.Config.WinLotteryRate);
            M2Share.Config.WinLotteryRate = ReadWriteInteger("Setup", "WinLotteryRate", M2Share.Config.WinLotteryRate);
            if (ReadWriteInteger("Setup", "WinLottery1Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery1Gold", M2Share.Config.WinLottery1Gold);
            M2Share.Config.WinLottery1Gold = ReadWriteInteger("Setup", "WinLottery1Gold", M2Share.Config.WinLottery1Gold);
            if (ReadWriteInteger("Setup", "WinLottery2Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery2Gold", M2Share.Config.WinLottery2Gold);
            M2Share.Config.WinLottery2Gold = ReadWriteInteger("Setup", "WinLottery2Gold", M2Share.Config.WinLottery2Gold);
            if (ReadWriteInteger("Setup", "WinLottery3Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery3Gold", M2Share.Config.WinLottery3Gold);
            M2Share.Config.WinLottery3Gold = ReadWriteInteger("Setup", "WinLottery3Gold", M2Share.Config.WinLottery3Gold);
            if (ReadWriteInteger("Setup", "WinLottery4Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery4Gold", M2Share.Config.WinLottery4Gold);
            M2Share.Config.WinLottery4Gold = ReadWriteInteger("Setup", "WinLottery4Gold", M2Share.Config.WinLottery4Gold);
            if (ReadWriteInteger("Setup", "WinLottery5Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery5Gold", M2Share.Config.WinLottery5Gold);
            M2Share.Config.WinLottery5Gold = ReadWriteInteger("Setup", "WinLottery5Gold", M2Share.Config.WinLottery5Gold);
            if (ReadWriteInteger("Setup", "WinLottery6Gold", -1) < 0)
                WriteInteger("Setup", "WinLottery6Gold", M2Share.Config.WinLottery6Gold);
            M2Share.Config.WinLottery6Gold = ReadWriteInteger("Setup", "WinLottery6Gold", M2Share.Config.WinLottery6Gold);
            if (ReadWriteInteger("Setup", "GuildRecallTime", -1) < 0)
                WriteInteger("Setup", "GuildRecallTime", M2Share.Config.GuildRecallTime);
            M2Share.Config.GuildRecallTime = ReadWriteInteger("Setup", "GuildRecallTime", M2Share.Config.GuildRecallTime);
            if (ReadWriteInteger("Setup", "GroupRecallTime", -1) < 0)
                WriteInteger("Setup", "GroupRecallTime", M2Share.Config.GroupRecallTime);
            M2Share.Config.GroupRecallTime = (short)ReadWriteInteger("Setup", "GroupRecallTime", M2Share.Config.GroupRecallTime);
            if (ReadWriteInteger("Setup", "ControlDropItem", -1) < 0)
                WriteBool("Setup", "ControlDropItem", M2Share.Config.ControlDropItem);
            M2Share.Config.ControlDropItem = ReadWriteBool("Setup", "ControlDropItem", M2Share.Config.ControlDropItem);
            if (ReadWriteInteger("Setup", "InSafeDisableDrop", -1) < 0)
                WriteBool("Setup", "InSafeDisableDrop", M2Share.Config.InSafeDisableDrop);
            M2Share.Config.InSafeDisableDrop = ReadWriteBool("Setup", "InSafeDisableDrop", M2Share.Config.InSafeDisableDrop);
            if (ReadWriteInteger("Setup", "CanDropGold", -1) < 0)
                WriteInteger("Setup", "CanDropGold", M2Share.Config.CanDropGold);
            M2Share.Config.CanDropGold = ReadWriteInteger("Setup", "CanDropGold", M2Share.Config.CanDropGold);
            if (ReadWriteInteger("Setup", "CanDropPrice", -1) < 0)
                WriteInteger("Setup", "CanDropPrice", M2Share.Config.CanDropPrice);
            M2Share.Config.CanDropPrice = ReadWriteInteger("Setup", "CanDropPrice", M2Share.Config.CanDropPrice);
            nLoadInteger = ReadWriteInteger("Setup", "SendCustemMsg", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "SendCustemMsg", M2Share.Config.SendCustemMsg);
            else
                M2Share.Config.SendCustemMsg = nLoadInteger == 1;
            nLoadInteger = ReadWriteInteger("Setup", "SubkMasterSendMsg", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "SubkMasterSendMsg", M2Share.Config.SubkMasterSendMsg);
            else
                M2Share.Config.SubkMasterSendMsg = nLoadInteger == 1;
            if (ReadWriteInteger("Setup", "SuperRepairPriceRate", -1) < 0)
                WriteInteger("Setup", "SuperRepairPriceRate", M2Share.Config.SuperRepairPriceRate);
            M2Share.Config.SuperRepairPriceRate = ReadWriteInteger("Setup", "SuperRepairPriceRate", M2Share.Config.SuperRepairPriceRate);
            if (ReadWriteInteger("Setup", "RepairItemDecDura", -1) < 0)
                WriteInteger("Setup", "RepairItemDecDura", M2Share.Config.RepairItemDecDura);
            M2Share.Config.RepairItemDecDura = ReadWriteInteger("Setup", "RepairItemDecDura", M2Share.Config.RepairItemDecDura);
            if (ReadWriteInteger("Setup", "DieScatterBag", -1) < 0)
                WriteBool("Setup", "DieScatterBag", M2Share.Config.DieScatterBag);
            M2Share.Config.DieScatterBag = ReadWriteBool("Setup", "DieScatterBag", M2Share.Config.DieScatterBag);
            if (ReadWriteInteger("Setup", "DieScatterBagRate", -1) < 0)
                WriteInteger("Setup", "DieScatterBagRate", M2Share.Config.DieScatterBagRate);
            M2Share.Config.DieScatterBagRate = ReadWriteInteger("Setup", "DieScatterBagRate", M2Share.Config.DieScatterBagRate);
            if (ReadWriteInteger("Setup", "DieRedScatterBagAll", -1) < 0)
                WriteBool("Setup", "DieRedScatterBagAll", M2Share.Config.DieRedScatterBagAll);
            M2Share.Config.DieRedScatterBagAll = ReadWriteBool("Setup", "DieRedScatterBagAll", M2Share.Config.DieRedScatterBagAll);
            if (ReadWriteInteger("Setup", "DieDropUseItemRate", -1) < 0)
                WriteInteger("Setup", "DieDropUseItemRate", M2Share.Config.DieDropUseItemRate);
            M2Share.Config.DieDropUseItemRate = ReadWriteInteger("Setup", "DieDropUseItemRate", M2Share.Config.DieDropUseItemRate);
            if (ReadWriteInteger("Setup", "DieRedDropUseItemRate", -1) < 0)
                WriteInteger("Setup", "DieRedDropUseItemRate", M2Share.Config.DieRedDropUseItemRate);
            M2Share.Config.DieRedDropUseItemRate = ReadWriteInteger("Setup", "DieRedDropUseItemRate", M2Share.Config.DieRedDropUseItemRate);
            if (ReadWriteInteger("Setup", "DieDropGold", -1) < 0)
                WriteBool("Setup", "DieDropGold", M2Share.Config.DieDropGold);
            M2Share.Config.DieDropGold = ReadWriteBool("Setup", "DieDropGold", M2Share.Config.DieDropGold);
            if (ReadWriteInteger("Setup", "KillByHumanDropUseItem", -1) < 0)
                WriteBool("Setup", "KillByHumanDropUseItem", M2Share.Config.KillByHumanDropUseItem);
            M2Share.Config.KillByHumanDropUseItem = ReadWriteBool("Setup", "KillByHumanDropUseItem", M2Share.Config.KillByHumanDropUseItem);
            if (ReadWriteInteger("Setup", "KillByMonstDropUseItem", -1) < 0)
                WriteBool("Setup", "KillByMonstDropUseItem", M2Share.Config.KillByMonstDropUseItem);
            M2Share.Config.KillByMonstDropUseItem = ReadWriteBool("Setup", "KillByMonstDropUseItem", M2Share.Config.KillByMonstDropUseItem);
            if (ReadWriteInteger("Setup", "KickExpireHuman", -1) < 0)
                WriteBool("Setup", "KickExpireHuman", M2Share.Config.KickExpireHuman);
            M2Share.Config.KickExpireHuman = ReadWriteBool("Setup", "KickExpireHuman", M2Share.Config.KickExpireHuman);
            if (ReadWriteInteger("Setup", "GuildRankNameLen", -1) < 0)
                WriteInteger("Setup", "GuildRankNameLen", M2Share.Config.GuildRankNameLen);
            M2Share.Config.GuildRankNameLen = ReadWriteInteger("Setup", "GuildRankNameLen", M2Share.Config.GuildRankNameLen);
            if (ReadWriteInteger("Setup", "GuildNameLen", -1) < 0)
                WriteInteger("Setup", "GuildNameLen", M2Share.Config.GuildNameLen);
            M2Share.Config.GuildNameLen = ReadWriteInteger("Setup", "GuildNameLen", M2Share.Config.GuildNameLen);
            if (ReadWriteInteger("Setup", "GuildMemberMaxLimit", -1) < 0)
                WriteInteger("Setup", "GuildMemberMaxLimit", M2Share.Config.GuildMemberMaxLimit);
            M2Share.Config.GuildMemberMaxLimit = ReadWriteInteger("Setup", "GuildMemberMaxLimit", M2Share.Config.GuildMemberMaxLimit);
            if (ReadWriteInteger("Setup", "AttackPosionRate", -1) < 0)
                WriteInteger("Setup", "AttackPosionRate", M2Share.Config.AttackPosionRate);
            M2Share.Config.AttackPosionRate = ReadWriteInteger("Setup", "AttackPosionRate", M2Share.Config.AttackPosionRate);
            if (ReadWriteInteger("Setup", "AttackPosionTime", -1) < 0)
                WriteInteger("Setup", "AttackPosionTime", M2Share.Config.AttackPosionTime);
            M2Share.Config.AttackPosionTime = (ushort)ReadWriteInteger("Setup", "AttackPosionTime", M2Share.Config.AttackPosionTime);
            if (ReadWriteInteger("Setup", "RevivalTime", -1) < 0)
                WriteInteger("Setup", "RevivalTime", M2Share.Config.RevivalTime);
            M2Share.Config.RevivalTime = ReadWriteInteger("Setup", "RevivalTime", M2Share.Config.RevivalTime);
            nLoadInteger = ReadWriteInteger("Setup", "UserMoveCanDupObj", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "UserMoveCanDupObj", M2Share.Config.boUserMoveCanDupObj);
            else
                M2Share.Config.boUserMoveCanDupObj = nLoadInteger == 1;
            nLoadInteger = ReadWriteInteger("Setup", "UserMoveCanOnItem", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "UserMoveCanOnItem", M2Share.Config.boUserMoveCanOnItem);
            else
                M2Share.Config.boUserMoveCanOnItem = nLoadInteger == 1;
            nLoadInteger = ReadWriteInteger("Setup", "UserMoveTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "UserMoveTime", M2Share.Config.dwUserMoveTime);
            else
                M2Share.Config.dwUserMoveTime = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "PKDieLostExpRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "PKDieLostExpRate", M2Share.Config.dwPKDieLostExpRate);
            else
                M2Share.Config.dwPKDieLostExpRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "PKDieLostLevelRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "PKDieLostLevelRate", M2Share.Config.nPKDieLostLevelRate);
            else
                M2Share.Config.nPKDieLostLevelRate = nLoadInteger;
            if (ReadWriteInteger("Setup", "PKFlagNameColor", -1) < 0)
                WriteInteger("Setup", "PKFlagNameColor", M2Share.Config.btPKFlagNameColor);
            M2Share.Config.btPKFlagNameColor = ReadWriteByte("Setup", "PKFlagNameColor", M2Share.Config.btPKFlagNameColor);
            if (ReadWriteInteger("Setup", "AllyAndGuildNameColor", -1) < 0)
                WriteInteger("Setup", "AllyAndGuildNameColor", M2Share.Config.btAllyAndGuildNameColor);
            M2Share.Config.btAllyAndGuildNameColor = ReadWriteByte("Setup", "AllyAndGuildNameColor", M2Share.Config.btAllyAndGuildNameColor);
            if (ReadWriteInteger("Setup", "WarGuildNameColor", -1) < 0)
                WriteInteger("Setup", "WarGuildNameColor", M2Share.Config.WarGuildNameColor);
            M2Share.Config.WarGuildNameColor = ReadWriteByte("Setup", "WarGuildNameColor", M2Share.Config.WarGuildNameColor);
            if (ReadWriteInteger("Setup", "InFreePKAreaNameColor", -1) < 0)
                WriteInteger("Setup", "InFreePKAreaNameColor", M2Share.Config.InFreePKAreaNameColor);
            M2Share.Config.InFreePKAreaNameColor = ReadWriteByte("Setup", "InFreePKAreaNameColor", M2Share.Config.InFreePKAreaNameColor);
            if (ReadWriteInteger("Setup", "PKLevel1NameColor", -1) < 0)
                WriteInteger("Setup", "PKLevel1NameColor", M2Share.Config.btPKLevel1NameColor);
            M2Share.Config.btPKLevel1NameColor = ReadWriteByte("Setup", "PKLevel1NameColor", M2Share.Config.btPKLevel1NameColor);
            if (ReadWriteInteger("Setup", "PKLevel2NameColor", -1) < 0)
                WriteInteger("Setup", "PKLevel2NameColor", M2Share.Config.btPKLevel2NameColor);
            M2Share.Config.btPKLevel2NameColor = ReadWriteByte("Setup", "PKLevel2NameColor", M2Share.Config.btPKLevel2NameColor);
            if (ReadWriteInteger("Setup", "SpiritMutiny", -1) < 0)
                WriteBool("Setup", "SpiritMutiny", M2Share.Config.SpiritMutiny);
            M2Share.Config.SpiritMutiny = ReadWriteBool("Setup", "SpiritMutiny", M2Share.Config.SpiritMutiny);
            if (ReadWriteInteger("Setup", "SpiritMutinyTime", -1) < 0)
                WriteInteger("Setup", "SpiritMutinyTime", M2Share.Config.SpiritMutinyTime);
            M2Share.Config.SpiritMutinyTime = ReadWriteInteger("Setup", "SpiritMutinyTime", M2Share.Config.SpiritMutinyTime);
            if (ReadWriteInteger("Setup", "SpiritPowerRate", -1) < 0)
                WriteInteger("Setup", "SpiritPowerRate", M2Share.Config.SpiritPowerRate);
            M2Share.Config.SpiritPowerRate = ReadWriteInteger("Setup", "SpiritPowerRate", M2Share.Config.SpiritPowerRate);
            if (ReadWriteInteger("Setup", "MasterDieMutiny", -1) < 0)
                WriteBool("Setup", "MasterDieMutiny", M2Share.Config.MasterDieMutiny);
            M2Share.Config.MasterDieMutiny = ReadWriteBool("Setup", "MasterDieMutiny", M2Share.Config.MasterDieMutiny);
            if (ReadWriteInteger("Setup", "MasterDieMutinyRate", -1) < 0)
                WriteInteger("Setup", "MasterDieMutinyRate", M2Share.Config.MasterDieMutinyRate);
            M2Share.Config.MasterDieMutinyRate = ReadWriteInteger("Setup", "MasterDieMutinyRate", M2Share.Config.MasterDieMutinyRate);
            if (ReadWriteInteger("Setup", "MasterDieMutinyPower", -1) < 0)
                WriteInteger("Setup", "MasterDieMutinyPower", M2Share.Config.MasterDieMutinyPower);
            M2Share.Config.MasterDieMutinyPower = ReadWriteInteger("Setup", "MasterDieMutinyPower", M2Share.Config.MasterDieMutinyPower);
            if (ReadWriteInteger("Setup", "MasterDieMutinyPower", -1) < 0)
                WriteInteger("Setup", "MasterDieMutinyPower", M2Share.Config.MasterDieMutinySpeed);
            M2Share.Config.MasterDieMutinySpeed = ReadWriteInteger("Setup", "MasterDieMutinyPower", M2Share.Config.MasterDieMutinySpeed);
            nLoadInteger = ReadWriteInteger("Setup", "BBMonAutoChangeColor", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "BBMonAutoChangeColor", M2Share.Config.BBMonAutoChangeColor);
            else
                M2Share.Config.BBMonAutoChangeColor = nLoadInteger == 1;
            nLoadInteger = ReadWriteInteger("Setup", "BBMonAutoChangeColorTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "BBMonAutoChangeColorTime", M2Share.Config.BBMonAutoChangeColorTime);
            else
                M2Share.Config.BBMonAutoChangeColorTime = nLoadInteger;
            if (ReadWriteInteger("Setup", "OldClientShowHiLevel", -1) < 0)
                WriteBool("Setup", "OldClientShowHiLevel", M2Share.Config.boOldClientShowHiLevel);
            M2Share.Config.boOldClientShowHiLevel = ReadWriteBool("Setup", "OldClientShowHiLevel", M2Share.Config.boOldClientShowHiLevel);
            if (ReadWriteInteger("Setup", "ShowScriptActionMsg", -1) < 0)
                WriteBool("Setup", "ShowScriptActionMsg", M2Share.Config.ShowScriptActionMsg);
            M2Share.Config.ShowScriptActionMsg = ReadWriteBool("Setup", "ShowScriptActionMsg", M2Share.Config.ShowScriptActionMsg);
            if (ReadWriteInteger("Setup", "RunSocketDieLoopLimit", -1) < 0)
                WriteInteger("Setup", "RunSocketDieLoopLimit", M2Share.Config.RunSocketDieLoopLimit);
            M2Share.Config.RunSocketDieLoopLimit = ReadWriteInteger("Setup", "RunSocketDieLoopLimit", M2Share.Config.RunSocketDieLoopLimit);
            if (ReadWriteInteger("Setup", "ThreadRun", -1) < 0)
                WriteBool("Setup", "ThreadRun", M2Share.Config.ThreadRun);
            M2Share.Config.ThreadRun = ReadWriteBool("Setup", "ThreadRun", M2Share.Config.ThreadRun);
            if (ReadWriteInteger("Setup", "ShowExceptionMsg", -1) < 0)
                WriteBool("Setup", "ShowExceptionMsg", M2Share.Config.ShowExceptionMsg);
            M2Share.Config.ShowExceptionMsg = ReadWriteBool("Setup", "ShowExceptionMsg", M2Share.Config.ShowExceptionMsg);
            if (ReadWriteInteger("Setup", "ShowPreFixMsg", -1) < 0)
                WriteBool("Setup", "ShowPreFixMsg", M2Share.Config.ShowPreFixMsg);
            M2Share.Config.ShowPreFixMsg = ReadWriteBool("Setup", "ShowPreFixMsg", M2Share.Config.ShowPreFixMsg);
            if (ReadWriteInteger("Setup", "MagTurnUndeadLevel", -1) < 0)
                WriteInteger("Setup", "MagTurnUndeadLevel", M2Share.Config.MagTurnUndeadLevel);
            M2Share.Config.MagTurnUndeadLevel = ReadWriteInteger("Setup", "MagTurnUndeadLevel", M2Share.Config.MagTurnUndeadLevel);
            nLoadInteger = ReadWriteInteger("Setup", "MagTammingLevel", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingLevel", M2Share.Config.MagTammingLevel);
            else
                M2Share.Config.MagTammingLevel = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "MagTammingTargetLevel", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingTargetLevel", M2Share.Config.MagTammingTargetLevel);
            else
                M2Share.Config.MagTammingTargetLevel = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "MagTammingTargetHPRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingTargetHPRate", M2Share.Config.MagTammingHPRate);
            else
                M2Share.Config.MagTammingHPRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "MagTammingCount", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagTammingCount", M2Share.Config.MagTammingCount);
            else
                M2Share.Config.MagTammingCount = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "MabMabeHitRandRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitRandRate", M2Share.Config.MabMabeHitRandRate);
            else
                M2Share.Config.MabMabeHitRandRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "MabMabeHitMinLvLimit", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitMinLvLimit", M2Share.Config.MabMabeHitMinLvLimit);
            else
                M2Share.Config.MabMabeHitMinLvLimit = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "MabMabeHitSucessRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitSucessRate", M2Share.Config.MabMabeHitSucessRate);
            else
                M2Share.Config.MabMabeHitSucessRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "MabMabeHitMabeTimeRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MabMabeHitMabeTimeRate", M2Share.Config.MabMabeHitMabeTimeRate);
            else
                M2Share.Config.MabMabeHitMabeTimeRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "MagicAttackRage", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "MagicAttackRage", M2Share.Config.MagicAttackRage);
            else
                M2Share.Config.MagicAttackRage = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "DropItemRage", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "DropItemRage", M2Share.Config.DropItemRage);
            else
                M2Share.Config.DropItemRage = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "AmyOunsulPoint", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "AmyOunsulPoint", M2Share.Config.AmyOunsulPoint);
            else
                M2Share.Config.AmyOunsulPoint = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "DisableInSafeZoneFireCross", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "DisableInSafeZoneFireCross", M2Share.Config.DisableInSafeZoneFireCross);
            else
                M2Share.Config.DisableInSafeZoneFireCross = nLoadInteger == 1;
            nLoadInteger = ReadWriteInteger("Setup", "GroupMbAttackPlayObject", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "GroupMbAttackPlayObject", M2Share.Config.GroupMbAttackPlayObject);
            else
                M2Share.Config.GroupMbAttackPlayObject = nLoadInteger == 1;
            nLoadInteger = ReadWriteInteger("Setup", "PosionDecHealthTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "PosionDecHealthTime", M2Share.Config.PosionDecHealthTime);
            else
                M2Share.Config.PosionDecHealthTime = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "PosionDamagarmor", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "PosionDamagarmor", M2Share.Config.PosionDamagarmor);
            else
                M2Share.Config.PosionDamagarmor = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "LimitSwordLong", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "LimitSwordLong", M2Share.Config.LimitSwordLong);
            else
                M2Share.Config.LimitSwordLong = !(nLoadInteger == 0);
            nLoadInteger = ReadWriteInteger("Setup", "SwordLongPowerRate", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "SwordLongPowerRate", M2Share.Config.SwordLongPowerRate);
            else
                M2Share.Config.SwordLongPowerRate = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "FireBoomRage", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "FireBoomRage", M2Share.Config.FireBoomRage);
            else
                M2Share.Config.FireBoomRage = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "SnowWindRange", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "SnowWindRange", M2Share.Config.SnowWindRange);
            else
                M2Share.Config.SnowWindRange = nLoadInteger;
            if (ReadWriteInteger("Setup", "ElecBlizzardRange", -1) < 0)
                WriteInteger("Setup", "ElecBlizzardRange", M2Share.Config.ElecBlizzardRange);
            M2Share.Config.ElecBlizzardRange = ReadWriteInteger("Setup", "ElecBlizzardRange", M2Share.Config.ElecBlizzardRange);
            if (ReadWriteInteger("Setup", "HumanLevelDiffer", -1) < 0)
                WriteInteger("Setup", "HumanLevelDiffer", M2Share.Config.HumanLevelDiffer);
            M2Share.Config.HumanLevelDiffer = ReadWriteInteger("Setup", "HumanLevelDiffer", M2Share.Config.HumanLevelDiffer);
            if (ReadWriteInteger("Setup", "KillHumanWinLevel", -1) < 0)
                WriteBool("Setup", "KillHumanWinLevel", M2Share.Config.IsKillHumanWinLevel);
            M2Share.Config.IsKillHumanWinLevel = ReadWriteBool("Setup", "KillHumanWinLevel", M2Share.Config.IsKillHumanWinLevel);
            if (ReadWriteInteger("Setup", "KilledLostLevel", -1) < 0)
                WriteBool("Setup", "KilledLostLevel", M2Share.Config.IsKilledLostLevel);
            M2Share.Config.IsKilledLostLevel = ReadWriteBool("Setup", "KilledLostLevel", M2Share.Config.IsKilledLostLevel);
            if (ReadWriteInteger("Setup", "KillHumanWinLevelPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanWinLevelPoint", M2Share.Config.KillHumanWinLevel);
            M2Share.Config.KillHumanWinLevel = ReadWriteInteger("Setup", "KillHumanWinLevelPoint", M2Share.Config.KillHumanWinLevel);
            if (ReadWriteInteger("Setup", "KilledLostLevelPoint", -1) < 0)
                WriteInteger("Setup", "KilledLostLevelPoint", M2Share.Config.KilledLostLevel);
            M2Share.Config.KilledLostLevel = ReadWriteInteger("Setup", "KilledLostLevelPoint", M2Share.Config.KilledLostLevel);
            if (ReadWriteInteger("Setup", "KillHumanWinExp", -1) < 0)
                WriteBool("Setup", "KillHumanWinExp", M2Share.Config.IsKillHumanWinExp);
            M2Share.Config.IsKillHumanWinExp = ReadWriteBool("Setup", "KillHumanWinExp", M2Share.Config.IsKillHumanWinExp);
            if (ReadWriteInteger("Setup", "KilledLostExp", -1) < 0)
                WriteBool("Setup", "KilledLostExp", M2Share.Config.IsKilledLostExp);
            M2Share.Config.IsKilledLostExp = ReadWriteBool("Setup", "KilledLostExp", M2Share.Config.IsKilledLostExp);
            if (ReadWriteInteger("Setup", "KillHumanWinExpPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanWinExpPoint", M2Share.Config.KillHumanWinExp);
            M2Share.Config.KillHumanWinExp = ReadWriteInteger("Setup", "KillHumanWinExpPoint", M2Share.Config.KillHumanWinExp);
            if (ReadWriteInteger("Setup", "KillHumanLostExpPoint", -1) < 0)
                WriteInteger("Setup", "KillHumanLostExpPoint", M2Share.Config.KillHumanLostExp);
            M2Share.Config.KillHumanLostExp = ReadWriteInteger("Setup", "KillHumanLostExpPoint", M2Share.Config.KillHumanLostExp);
            if (ReadWriteInteger("Setup", "MonsterPowerRate", -1) < 0)
                WriteInteger("Setup", "MonsterPowerRate", M2Share.Config.MonsterPowerRate);
            M2Share.Config.MonsterPowerRate = ReadWriteInteger("Setup", "MonsterPowerRate", M2Share.Config.MonsterPowerRate);
            if (ReadWriteInteger("Setup", "ItemsPowerRate", -1) < 0)
                WriteInteger("Setup", "ItemsPowerRate", M2Share.Config.ItemsPowerRate);
            M2Share.Config.ItemsPowerRate = ReadWriteInteger("Setup", "ItemsPowerRate", M2Share.Config.ItemsPowerRate);
            if (ReadWriteInteger("Setup", "ItemsACPowerRate", -1) < 0)
                WriteInteger("Setup", "ItemsACPowerRate", M2Share.Config.ItemsACPowerRate);
            M2Share.Config.ItemsACPowerRate = ReadWriteInteger("Setup", "ItemsACPowerRate", M2Share.Config.ItemsACPowerRate);
            if (ReadWriteInteger("Setup", "SendOnlineCount", -1) < 0)
                WriteBool("Setup", "SendOnlineCount", M2Share.Config.SendOnlineCount);
            M2Share.Config.SendOnlineCount = ReadWriteBool("Setup", "SendOnlineCount", M2Share.Config.SendOnlineCount);
            if (ReadWriteInteger("Setup", "SendOnlineCountRate", -1) < 0)
                WriteInteger("Setup", "SendOnlineCountRate", M2Share.Config.SendOnlineCountRate);
            M2Share.Config.SendOnlineCountRate = ReadWriteInteger("Setup", "SendOnlineCountRate", M2Share.Config.SendOnlineCountRate);
            if (ReadWriteInteger("Setup", "SendOnlineTime", -1) < 0)
                WriteInteger("Setup", "SendOnlineTime", M2Share.Config.SendOnlineTime);
            M2Share.Config.SendOnlineTime = ReadWriteInteger("Setup", "SendOnlineTime", M2Share.Config.SendOnlineTime);
            if (ReadWriteInteger("Setup", "SaveHumanRcdTime", -1) < 0)
                WriteInteger("Setup", "SaveHumanRcdTime", M2Share.Config.SaveHumanRcdTime);
            M2Share.Config.SaveHumanRcdTime = ReadWriteInteger("Setup", "SaveHumanRcdTime", M2Share.Config.SaveHumanRcdTime);
            if (ReadWriteInteger("Setup", "HumanFreeDelayTime", -1) < 0)
                WriteInteger("Setup", "HumanFreeDelayTime", M2Share.Config.HumanFreeDelayTime);
            if (ReadWriteInteger("Setup", "MakeGhostTime", -1) < 0)
                WriteInteger("Setup", "MakeGhostTime", M2Share.Config.MakeGhostTime);
            M2Share.Config.MakeGhostTime = ReadWriteInteger("Setup", "MakeGhostTime", M2Share.Config.MakeGhostTime);
            if (ReadWriteInteger("Setup", "ClearDropOnFloorItemTime", -1) < 0)
                WriteInteger("Setup", "ClearDropOnFloorItemTime", M2Share.Config.ClearDropOnFloorItemTime);
            M2Share.Config.ClearDropOnFloorItemTime = ReadWriteInteger("Setup", "ClearDropOnFloorItemTime", M2Share.Config.ClearDropOnFloorItemTime);
            if (ReadWriteInteger("Setup", "FloorItemCanPickUpTime", -1) < 0)
                WriteInteger("Setup", "FloorItemCanPickUpTime", M2Share.Config.FloorItemCanPickUpTime);
            M2Share.Config.FloorItemCanPickUpTime = ReadWriteInteger("Setup", "FloorItemCanPickUpTime", M2Share.Config.FloorItemCanPickUpTime);
            if (ReadWriteInteger("Setup", "PasswordLockSystem", -1) < 0)
                WriteBool("Setup", "PasswordLockSystem", M2Share.Config.PasswordLockSystem);
            M2Share.Config.PasswordLockSystem = ReadWriteBool("Setup", "PasswordLockSystem", M2Share.Config.PasswordLockSystem);
            if (ReadWriteInteger("Setup", "PasswordLockDealAction", -1) < 0)
                WriteBool("Setup", "PasswordLockDealAction", M2Share.Config.LockDealAction);
            M2Share.Config.LockDealAction = ReadWriteBool("Setup", "PasswordLockDealAction", M2Share.Config.LockDealAction);
            if (ReadWriteInteger("Setup", "PasswordLockDropAction", -1) < 0)
                WriteBool("Setup", "PasswordLockDropAction", M2Share.Config.LockDropAction);
            M2Share.Config.LockDropAction = ReadWriteBool("Setup", "PasswordLockDropAction", M2Share.Config.LockDropAction);
            if (ReadWriteInteger("Setup", "PasswordLockGetBackItemAction", -1) < 0)
                WriteBool("Setup", "PasswordLockGetBackItemAction", M2Share.Config.LockGetBackItemAction);
            M2Share.Config.LockGetBackItemAction = ReadWriteBool("Setup", "PasswordLockGetBackItemAction", M2Share.Config.LockGetBackItemAction);
            if (ReadWriteInteger("Setup", "PasswordLockHumanLogin", -1) < 0)
                WriteBool("Setup", "PasswordLockHumanLogin", M2Share.Config.LockHumanLogin);
            M2Share.Config.LockHumanLogin = ReadWriteBool("Setup", "PasswordLockHumanLogin", M2Share.Config.LockHumanLogin);
            if (ReadWriteInteger("Setup", "PasswordLockWalkAction", -1) < 0)
                WriteBool("Setup", "PasswordLockWalkAction", M2Share.Config.LockWalkAction);
            M2Share.Config.LockWalkAction = ReadWriteBool("Setup", "PasswordLockWalkAction", M2Share.Config.LockWalkAction);
            if (ReadWriteInteger("Setup", "PasswordLockRunAction", -1) < 0)
                WriteBool("Setup", "PasswordLockRunAction", M2Share.Config.LockRunAction);
            M2Share.Config.LockRunAction = ReadWriteBool("Setup", "PasswordLockRunAction", M2Share.Config.LockRunAction);
            if (ReadWriteInteger("Setup", "PasswordLockHitAction", -1) < 0)
                WriteBool("Setup", "PasswordLockHitAction", M2Share.Config.LockHitAction);
            M2Share.Config.LockHitAction = ReadWriteBool("Setup", "PasswordLockHitAction", M2Share.Config.LockHitAction);
            if (ReadWriteInteger("Setup", "PasswordLockSpellAction", -1) < 0)
                WriteBool("Setup", "PasswordLockSpellAction", M2Share.Config.LockSpellAction);
            M2Share.Config.LockSpellAction = ReadWriteBool("Setup", "PasswordLockSpellAction", M2Share.Config.LockSpellAction);
            if (ReadWriteInteger("Setup", "PasswordLockSendMsgAction", -1) < 0)
                WriteBool("Setup", "PasswordLockSendMsgAction", M2Share.Config.LockSendMsgAction);
            M2Share.Config.LockSendMsgAction = ReadWriteBool("Setup", "PasswordLockSendMsgAction", M2Share.Config.LockSendMsgAction);
            if (ReadWriteInteger("Setup", "PasswordLockUserItemAction", -1) < 0)
                WriteBool("Setup", "PasswordLockUserItemAction", M2Share.Config.LockUserItemAction);
            M2Share.Config.LockUserItemAction = ReadWriteBool("Setup", "PasswordLockUserItemAction", M2Share.Config.LockUserItemAction);
            if (ReadWriteInteger("Setup", "PasswordLockInObModeAction", -1) < 0)
                WriteBool("Setup", "PasswordLockInObModeAction", M2Share.Config.LockInObModeAction);
            M2Share.Config.LockInObModeAction = ReadWriteBool("Setup", "PasswordLockInObModeAction", M2Share.Config.LockInObModeAction);
            if (ReadWriteInteger("Setup", "PasswordErrorKick", -1) < 0)
                WriteBool("Setup", "PasswordErrorKick", M2Share.Config.PasswordErrorKick);
            M2Share.Config.PasswordErrorKick = ReadWriteBool("Setup", "PasswordErrorKick", M2Share.Config.PasswordErrorKick);
            if (ReadWriteInteger("Setup", "PasswordErrorCountLock", -1) < 0)
                WriteInteger("Setup", "PasswordErrorCountLock", M2Share.Config.PasswordErrorCountLock);
            M2Share.Config.PasswordErrorCountLock = ReadWriteInteger("Setup", "PasswordErrorCountLock", M2Share.Config.PasswordErrorCountLock);
            if (ReadWriteInteger("Setup", "SoftVersionDate", -1) < 0)
                WriteInteger("Setup", "SoftVersionDate", M2Share.Config.SoftVersionDate);
            M2Share.Config.SoftVersionDate = ReadWriteInteger("Setup", "SoftVersionDate", M2Share.Config.SoftVersionDate);
            nLoadInteger = ReadWriteInteger("Setup", "CanOldClientLogon", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "CanOldClientLogon", M2Share.Config.CanOldClientLogon);
            else
                M2Share.Config.CanOldClientLogon = nLoadInteger == 1;
            if (ReadWriteInteger("Setup", "ConsoleShowUserCountTime", -1) < 0)
                WriteInteger("Setup", "ConsoleShowUserCountTime", M2Share.Config.ConsoleShowUserCountTime);
            M2Share.Config.ConsoleShowUserCountTime = ReadWriteInteger("Setup", "ConsoleShowUserCountTime", M2Share.Config.ConsoleShowUserCountTime);
            if (ReadWriteInteger("Setup", "ShowLineNoticeTime", -1) < 0)
                WriteInteger("Setup", "ShowLineNoticeTime", M2Share.Config.ShowLineNoticeTime);
            M2Share.Config.ShowLineNoticeTime = ReadWriteInteger("Setup", "ShowLineNoticeTime", M2Share.Config.ShowLineNoticeTime);
            if (ReadWriteInteger("Setup", "LineNoticeColor", -1) < 0)
                WriteInteger("Setup", "LineNoticeColor", M2Share.Config.LineNoticeColor);
            M2Share.Config.LineNoticeColor = ReadWriteInteger("Setup", "LineNoticeColor", M2Share.Config.LineNoticeColor);

            if (ReadWriteInteger("Setup", "MaxHitMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxHitMsgCount", M2Share.Config.MaxHitMsgCount);
            M2Share.Config.MaxHitMsgCount = ReadWriteInteger("Setup", "MaxHitMsgCount", M2Share.Config.MaxHitMsgCount);
            if (ReadWriteInteger("Setup", "MaxSpellMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxSpellMsgCount", M2Share.Config.MaxSpellMsgCount);
            M2Share.Config.MaxSpellMsgCount = ReadWriteInteger("Setup", "MaxSpellMsgCount", M2Share.Config.MaxSpellMsgCount);
            if (ReadWriteInteger("Setup", "MaxRunMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxRunMsgCount", M2Share.Config.MaxRunMsgCount);
            M2Share.Config.MaxRunMsgCount = ReadWriteInteger("Setup", "MaxRunMsgCount", M2Share.Config.MaxRunMsgCount);
            if (ReadWriteInteger("Setup", "MaxWalkMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxWalkMsgCount", M2Share.Config.MaxWalkMsgCount);
            M2Share.Config.MaxWalkMsgCount = ReadWriteInteger("Setup", "MaxWalkMsgCount", M2Share.Config.MaxWalkMsgCount);
            if (ReadWriteInteger("Setup", "MaxTurnMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxTurnMsgCount", M2Share.Config.MaxTurnMsgCount);
            M2Share.Config.MaxTurnMsgCount = ReadWriteInteger("Setup", "MaxTurnMsgCount", M2Share.Config.MaxTurnMsgCount);
            if (ReadWriteInteger("Setup", "MaxSitDonwMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxSitDonwMsgCount", M2Share.Config.MaxSitDonwMsgCount);
            M2Share.Config.MaxSitDonwMsgCount = ReadWriteInteger("Setup", "MaxSitDonwMsgCount", M2Share.Config.MaxSitDonwMsgCount);
            if (ReadWriteInteger("Setup", "MaxDigUpMsgCount", -1) < 0)
                WriteInteger("Setup", "MaxDigUpMsgCount", M2Share.Config.MaxDigUpMsgCount);
            M2Share.Config.MaxDigUpMsgCount = ReadWriteInteger("Setup", "MaxDigUpMsgCount", M2Share.Config.MaxDigUpMsgCount);
            if (ReadWriteInteger("Setup", "SpellSendUpdateMsg", -1) < 0)
                WriteBool("Setup", "SpellSendUpdateMsg", M2Share.Config.SpellSendUpdateMsg);
            M2Share.Config.SpellSendUpdateMsg = ReadWriteBool("Setup", "SpellSendUpdateMsg", M2Share.Config.SpellSendUpdateMsg);
            if (ReadWriteInteger("Setup", "ActionSendActionMsg", -1) < 0)
                WriteBool("Setup", "ActionSendActionMsg", M2Share.Config.ActionSendActionMsg);
            M2Share.Config.ActionSendActionMsg = ReadWriteBool("Setup", "ActionSendActionMsg", M2Share.Config.ActionSendActionMsg);
            if (ReadWriteInteger("Setup", "OverSpeedKickCount", -1) < 0)
                WriteInteger("Setup", "OverSpeedKickCount", M2Share.Config.OverSpeedKickCount);
            M2Share.Config.OverSpeedKickCount = ReadWriteInteger("Setup", "OverSpeedKickCount", M2Share.Config.OverSpeedKickCount);
            if (ReadWriteInteger("Setup", "DropOverSpeed", -1) < 0)
                WriteInteger("Setup", "DropOverSpeed", M2Share.Config.DropOverSpeed);
            M2Share.Config.DropOverSpeed = ReadWriteInteger("Setup", "DropOverSpeed", M2Share.Config.DropOverSpeed);
            if (ReadWriteInteger("Setup", "KickOverSpeed", -1) < 0)
                WriteBool("Setup", "KickOverSpeed", M2Share.Config.KickOverSpeed);
            M2Share.Config.KickOverSpeed = ReadWriteBool("Setup", "KickOverSpeed", M2Share.Config.KickOverSpeed);
            nLoadInteger = ReadWriteInteger("Setup", "SpeedControlMode", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "SpeedControlMode", M2Share.Config.SpeedControlMode);
            else
                M2Share.Config.SpeedControlMode = nLoadInteger;
            if (ReadWriteInteger("Setup", "HitIntervalTime", -1) < 0)
                WriteInteger("Setup", "HitIntervalTime", M2Share.Config.HitIntervalTime);
            M2Share.Config.HitIntervalTime =
                ReadWriteInteger("Setup", "HitIntervalTime", M2Share.Config.HitIntervalTime);
            if (ReadWriteInteger("Setup", "MagicHitIntervalTime", -1) < 0)
                WriteInteger("Setup", "MagicHitIntervalTime", M2Share.Config.MagicHitIntervalTime);
            M2Share.Config.MagicHitIntervalTime = ReadWriteInteger("Setup", "MagicHitIntervalTime", M2Share.Config.MagicHitIntervalTime);
            if (ReadWriteInteger("Setup", "RunIntervalTime", -1) < 0)
                WriteInteger("Setup", "RunIntervalTime", M2Share.Config.RunIntervalTime);
            M2Share.Config.RunIntervalTime = ReadWriteInteger("Setup", "RunIntervalTime", M2Share.Config.RunIntervalTime);
            if (ReadWriteInteger("Setup", "WalkIntervalTime", -1) < 0)
                WriteInteger("Setup", "WalkIntervalTime", M2Share.Config.WalkIntervalTime);
            M2Share.Config.WalkIntervalTime = ReadWriteInteger("Setup", "WalkIntervalTime", M2Share.Config.WalkIntervalTime);
            if (ReadWriteInteger("Setup", "TurnIntervalTime", -1) < 0)
                WriteInteger("Setup", "TurnIntervalTime", M2Share.Config.TurnIntervalTime);
            M2Share.Config.TurnIntervalTime = ReadWriteInteger("Setup", "TurnIntervalTime", M2Share.Config.TurnIntervalTime);
            nLoadInteger = ReadWriteInteger("Setup", "ControlActionInterval", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "ControlActionInterval", M2Share.Config.boControlActionInterval);
            else
                M2Share.Config.boControlActionInterval = nLoadInteger == 1;
            nLoadInteger = ReadWriteInteger("Setup", "ControlWalkHit", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "ControlWalkHit", M2Share.Config.boControlWalkHit);
            else
                M2Share.Config.boControlWalkHit = nLoadInteger == 1;
            nLoadInteger = ReadWriteInteger("Setup", "ControlRunLongHit", -1);
            if (nLoadInteger < 0)
            {
                WriteBool("Setup", "ControlRunLongHit", M2Share.Config.boControlRunLongHit);
            }
            else
            {
                M2Share.Config.boControlRunLongHit = nLoadInteger == 1;
            }
            nLoadInteger = ReadWriteInteger("Setup", "ControlRunHit", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "ControlRunHit", M2Share.Config.boControlRunHit);
            else
                M2Share.Config.boControlRunHit = nLoadInteger == 1;
            nLoadInteger = ReadWriteInteger("Setup", "ControlRunMagic", -1);
            if (nLoadInteger < 0)
                WriteBool("Setup", "ControlRunMagic", M2Share.Config.boControlRunMagic);
            else
                M2Share.Config.boControlRunMagic = nLoadInteger == 1;
            nLoadInteger = ReadWriteInteger("Setup", "ActionIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "ActionIntervalTime", M2Share.Config.ActionIntervalTime);
            else
                M2Share.Config.ActionIntervalTime = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "RunLongHitIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "RunLongHitIntervalTime", M2Share.Config.RunLongHitIntervalTime);
            else
                M2Share.Config.RunLongHitIntervalTime = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "RunHitIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "RunHitIntervalTime", M2Share.Config.RunHitIntervalTime);
            else
                M2Share.Config.RunHitIntervalTime = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "WalkHitIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "WalkHitIntervalTime", M2Share.Config.WalkHitIntervalTime);
            else
                M2Share.Config.WalkHitIntervalTime = nLoadInteger;
            nLoadInteger = ReadWriteInteger("Setup", "RunMagicIntervalTime", -1);
            if (nLoadInteger < 0)
                WriteInteger("Setup", "RunMagicIntervalTime", M2Share.Config.RunMagicIntervalTime);
            else
                M2Share.Config.RunMagicIntervalTime = nLoadInteger;
            if (ReadWriteInteger("Setup", "DisableStruck", -1) < 0)
                WriteBool("Setup", "DisableStruck", M2Share.Config.DisableStruck);
            M2Share.Config.DisableStruck =
                ReadWriteBool("Setup", "DisableStruck", M2Share.Config.DisableStruck);
            if (ReadWriteInteger("Setup", "DisableSelfStruck", -1) < 0)
                WriteBool("Setup", "DisableSelfStruck", M2Share.Config.DisableSelfStruck);
            M2Share.Config.DisableSelfStruck =
                ReadWriteBool("Setup", "DisableSelfStruck", M2Share.Config.DisableSelfStruck);
            if (ReadWriteInteger("Setup", "StruckTime", -1) < 0)
                WriteInteger("Setup", "StruckTime", M2Share.Config.StruckTime);
            M2Share.Config.StruckTime = ReadWriteInteger("Setup", "StruckTime", M2Share.Config.StruckTime);
            nLoadInteger = ReadWriteInteger("Setup", "AddUserItemNewValue", -1);
            if (nLoadInteger < 0)
            {
                WriteBool("Setup", "AddUserItemNewValue", M2Share.Config.AddUserItemNewValue);
            }
            else
            {
                M2Share.Config.AddUserItemNewValue = nLoadInteger == 1;
            }
            nLoadInteger = ReadWriteInteger("Setup", "TestSpeedMode", -1);
            if (nLoadInteger < 0)
            {
                WriteBool("Setup", "TestSpeedMode", M2Share.Config.TestSpeedMode);
            }
            else
            {
                M2Share.Config.TestSpeedMode = nLoadInteger == 1;
            }
            // 气血石开始
            if (ReadWriteInteger("Setup", "HPStoneStartRate", -1) < 0)
            {
                WriteInteger("Setup", "HPStoneStartRate", M2Share.Config.HPStoneStartRate);
            }
            M2Share.Config.HPStoneStartRate = ReadWriteByte("Setup", "HPStoneStartRate", M2Share.Config.HPStoneStartRate);
            if (ReadWriteInteger("Setup", "MPStoneStartRate", -1) < 0)
            {
                WriteInteger("Setup", "MPStoneStartRate", M2Share.Config.MPStoneStartRate);
            }
            M2Share.Config.MPStoneStartRate = ReadWriteByte("Setup", "MPStoneStartRate", M2Share.Config.MPStoneStartRate);
            if (ReadWriteInteger("Setup", "HPStoneIntervalTime", -1) < 0)
            {
                WriteInteger("Setup", "HPStoneIntervalTime", M2Share.Config.HPStoneIntervalTime);
            }
            M2Share.Config.HPStoneIntervalTime = ReadWriteInteger("Setup", "HPStoneIntervalTime", M2Share.Config.HPStoneIntervalTime);
            if (ReadWriteInteger("Setup", "MPStoneIntervalTime", -1) < 0)
            {
                WriteInteger("Setup", "MPStoneIntervalTime", M2Share.Config.MpStoneIntervalTime);
            }
            M2Share.Config.MpStoneIntervalTime = ReadWriteInteger("Setup", "MPStoneIntervalTime", M2Share.Config.MpStoneIntervalTime);
            if (ReadWriteInteger("Setup", "HPStoneAddRate", -1) < 0)
            {
                WriteInteger("Setup", "HPStoneAddRate", M2Share.Config.HPStoneAddRate);
            }
            M2Share.Config.HPStoneAddRate = ReadWriteByte("Setup", "HPStoneAddRate", M2Share.Config.HPStoneAddRate);
            if (ReadWriteInteger("Setup", "MPStoneAddRate", -1) < 0)
            {
                WriteInteger("Setup", "MPStoneAddRate", M2Share.Config.MPStoneAddRate);
            }
            M2Share.Config.MPStoneAddRate = ReadWriteByte("Setup", "MPStoneAddRate", M2Share.Config.MPStoneAddRate);
            if (ReadWriteInteger("Setup", "HPStoneDecDura", -1) < 0)
            {
                WriteInteger("Setup", "HPStoneDecDura", M2Share.Config.HPStoneDecDura);
            }
            M2Share.Config.HPStoneDecDura = ReadWriteInteger("Setup", "HPStoneDecDura", M2Share.Config.HPStoneDecDura);
            if (ReadWriteInteger("Setup", "MPStoneDecDura", -1) < 0)
            {
                WriteInteger("Setup", "MPStoneDecDura", M2Share.Config.MPStoneDecDura);
            }
            M2Share.Config.MPStoneDecDura = ReadWriteInteger("Setup", "MPStoneDecDura", M2Share.Config.MPStoneDecDura);

            // 气血石结束
            nLoadInteger = ReadWriteInteger("Setup", "WeaponMakeUnLuckRate", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeUnLuckRate", M2Share.Config.WeaponMakeUnLuckRate);
            }
            else
            {
                M2Share.Config.WeaponMakeUnLuckRate = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "WeaponMakeLuckPoint1", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint1", M2Share.Config.WeaponMakeLuckPoint1);
            }
            else
            {
                M2Share.Config.WeaponMakeLuckPoint1 = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "WeaponMakeLuckPoint2", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint2", M2Share.Config.WeaponMakeLuckPoint2);
            }
            else
            {
                M2Share.Config.WeaponMakeLuckPoint2 = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "WeaponMakeLuckPoint3", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint3", M2Share.Config.WeaponMakeLuckPoint3);
            }
            else
            {
                M2Share.Config.WeaponMakeLuckPoint3 = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "WeaponMakeLuckPoint2Rate", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint2Rate", M2Share.Config.WeaponMakeLuckPoint2Rate);
            }
            else
            {
                M2Share.Config.WeaponMakeLuckPoint2Rate = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "WeaponMakeLuckPoint3Rate", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WeaponMakeLuckPoint3Rate", M2Share.Config.WeaponMakeLuckPoint3Rate);
            }
            else
            {
                M2Share.Config.WeaponMakeLuckPoint3Rate = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "CheckUserItemPlace", -1);
            if (nLoadInteger < 0)
            {
                WriteBool("Setup", "CheckUserItemPlace", M2Share.Config.CheckUserItemPlace);
            }
            else
            {
                M2Share.Config.CheckUserItemPlace = nLoadInteger == 1;
            }
            nLoadInteger = ReadWriteInteger("Setup", "LevelValueOfTaosHP", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "LevelValueOfTaosHP", M2Share.Config.nLevelValueOfTaosHP);
            }
            else
            {
                M2Share.Config.nLevelValueOfTaosHP = nLoadInteger;
            }
            double nLoadFloatRate = ReadWriteFloat("Setup", "LevelValueOfTaosHPRate", 0);
            if (nLoadFloatRate == 0)
            {
                WriteInteger("Setup", "LevelValueOfTaosHPRate", M2Share.Config.nLevelValueOfTaosHPRate);
            }
            else
            {
                M2Share.Config.nLevelValueOfTaosHPRate = nLoadFloatRate;
            }
            nLoadInteger = ReadWriteInteger("Setup", "LevelValueOfTaosMP", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "LevelValueOfTaosMP", M2Share.Config.nLevelValueOfTaosMP);
            }
            else
            {
                M2Share.Config.nLevelValueOfTaosMP = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "LevelValueOfWizardHP", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "LevelValueOfWizardHP", M2Share.Config.nLevelValueOfWizardHP);
            }
            else
            {
                M2Share.Config.nLevelValueOfWizardHP = nLoadInteger;
            }
            nLoadFloatRate = ReadWriteFloat("Setup", "LevelValueOfWizardHPRate", 0);
            if (nLoadFloatRate == 0)
            {
                WriteInteger("Setup", "LevelValueOfWizardHPRate", M2Share.Config.nLevelValueOfWizardHPRate);
            }
            else
            {
                M2Share.Config.nLevelValueOfWizardHPRate = nLoadFloatRate;
            }
            nLoadInteger = ReadWriteInteger("Setup", "LevelValueOfWarrHP", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "LevelValueOfWarrHP", M2Share.Config.nLevelValueOfWarrHP);
            }
            else
            {
                M2Share.Config.nLevelValueOfWarrHP = nLoadInteger;
            }
            nLoadFloatRate = ReadWriteFloat("Setup", "LevelValueOfWarrHPRate", 0);
            if (nLoadFloatRate == 0)
            {
                WriteInteger("Setup", "LevelValueOfWarrHPRate", M2Share.Config.nLevelValueOfWarrHPRate);
            }
            else
            {
                M2Share.Config.nLevelValueOfWarrHPRate = nLoadFloatRate;
            }
            nLoadInteger = ReadWriteInteger("Setup", "ProcessMonsterInterval", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "ProcessMonsterInterval", M2Share.Config.ProcessMonsterInterval);
            }
            else
            {
                M2Share.Config.ProcessMonsterInterval = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "ProcessMonsterMultiThreadLimit", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "ProcessMonsterMultiThreadLimit", M2Share.Config.ProcessMonsterMultiThreadLimit);
            }
            else
            {
                M2Share.Config.ProcessMonsterMultiThreadLimit = nLoadInteger;
            }
            if (ReadWriteInteger("Setup", "StartCastleWarDays", -1) < 0)
            {
                WriteInteger("Setup", "StartCastleWarDays", M2Share.Config.StartCastleWarDays);
            }
            M2Share.Config.StartCastleWarDays = ReadWriteInteger("Setup", "StartCastleWarDays", M2Share.Config.StartCastleWarDays);
            if (ReadWriteInteger("Setup", "StartCastlewarTime", -1) < 0)
            {
                WriteInteger("Setup", "StartCastlewarTime", M2Share.Config.StartCastlewarTime);
            }
            M2Share.Config.StartCastlewarTime = ReadWriteInteger("Setup", "StartCastlewarTime", M2Share.Config.StartCastlewarTime);
            if (ReadWriteInteger("Setup", "ShowCastleWarEndMsgTime", -1) < 0)
            {
                WriteInteger("Setup", "ShowCastleWarEndMsgTime", M2Share.Config.ShowCastleWarEndMsgTime);
            }
            M2Share.Config.ShowCastleWarEndMsgTime = ReadWriteInteger("Setup", "ShowCastleWarEndMsgTime", M2Share.Config.ShowCastleWarEndMsgTime);
            if (ReadWriteInteger("Server", "ClickNPCTime", -1) < 0)
            {
                WriteInteger("Server", "ClickNPCTime", M2Share.Config.ClickNpcTime);
            }
            M2Share.Config.ClickNpcTime = ReadWriteInteger("Server", "ClickNPCTime", M2Share.Config.ClickNpcTime);
            if (ReadWriteInteger("Setup", "CastleWarTime", -1) < 0)
            {
                WriteInteger("Setup", "CastleWarTime", M2Share.Config.CastleWarTime);
            }
            M2Share.Config.CastleWarTime = ReadWriteInteger("Setup", "CastleWarTime", M2Share.Config.CastleWarTime);
            nLoadInteger = ReadWriteInteger("Setup", "GetCastleTime", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "GetCastleTime", M2Share.Config.GetCastleTime);
            }
            else
            {
                M2Share.Config.GetCastleTime = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "GuildWarTime", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "GuildWarTime", M2Share.Config.GuildWarTime);
            }
            else
            {
                M2Share.Config.GuildWarTime = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "WinLotteryCount", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryCount", M2Share.Config.WinLotteryCount);
            }
            else
            {
                M2Share.Config.WinLotteryCount = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "NoWinLotteryCount", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "NoWinLotteryCount", M2Share.Config.NoWinLotteryCount);
            }
            else
            {
                M2Share.Config.NoWinLotteryCount = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "WinLotteryLevel1", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel1", M2Share.Config.WinLotteryLevel1);
            }
            else
            {
                M2Share.Config.WinLotteryLevel1 = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "WinLotteryLevel2", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel2", M2Share.Config.WinLotteryLevel2);
            }
            else
            {
                M2Share.Config.WinLotteryLevel2 = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "WinLotteryLevel3", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel3", M2Share.Config.WinLotteryLevel3);
            }
            else
            {
                M2Share.Config.WinLotteryLevel3 = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "WinLotteryLevel4", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel4", M2Share.Config.WinLotteryLevel4);
            }
            else
            {
                M2Share.Config.WinLotteryLevel4 = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "WinLotteryLevel5", -1);
            if (nLoadInteger < 0)
            {
                WriteInteger("Setup", "WinLotteryLevel5", M2Share.Config.WinLotteryLevel5);
            }
            else
            {
                M2Share.Config.WinLotteryLevel5 = nLoadInteger;
            }
            nLoadInteger = ReadWriteInteger("Setup", "WinLotteryLevel6", -1);
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