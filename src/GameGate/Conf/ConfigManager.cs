using NLog;
using System;
using System.IO;
using SystemModule.Common;

namespace GameGate.Conf
{
    public class ConfigManager : ConfigFile
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly string fileName = Path.Combine(AppContext.BaseDirectory, "config.conf");
        private static readonly ConfigManager instance = new ConfigManager(fileName);
        public static ConfigManager Instance => instance;
        public GateConfig GateConfig;
        public GameGateInfo[] GateList;

        private ConfigManager(string fileName) : base(fileName)
        {
            GateConfig = new GateConfig();
            GateList = new GameGateInfo[32];
            for (int i = 0; i < GateList.Length; i++)
            {
                GateList[i] = new GameGateInfo();
                GateList[i].ServiceId = (byte)i;
                GateList[i].ServerAdress = "127.0.0.1";
                GateList[i].ServerPort = 5000;
                GateList[i].GateAddress = "127.0.0.1";
                GateList[i].GatePort = 7200 + i;
                GateList[i].ThreadId = Guid.NewGuid().ToString("N");
            }
        }

        public void LoadConfig()
        {
            Load();

            GateConfig.ServerWorkThread = ReadWriteByte("GameGate", "ServerWorkThread", 1);
            if (GateConfig.ServerWorkThread <= 0)
            {
                GateConfig.ServerWorkThread = 1;
            }
            if (GateConfig.ServerWorkThread > 50)
            {
                GateConfig.ServerWorkThread = 50;
            }
            for (var i = 0; i < GateConfig.ServerWorkThread; i++)
            {
                GateList[i].ServerAdress = ReadWriteString("GameGate", "ServerAddr" + (i + 1), GateList[i].ServerAdress);
                GateList[i].ServerPort = ReadWriteInteger("GameGate", "ServerPort" + (i + 1), GateList[i].ServerPort);
                GateList[i].GateAddress = ReadWriteString("GameGate", "GateAddress" + (i + 1), GateList[i].GateAddress);
                GateList[i].GatePort = ReadWriteInteger("GameGate", "GatePort" + (i + 1), GateList[i].GatePort);
                if (string.IsNullOrEmpty(GateList[i].ServerAdress) || GateList[i].ServerPort <= 0)
                {
                    logger.Info("配置文件节点[ServerAddr{I}]配置错误或端口错误.", i);
                    continue;
                }
                if (string.IsNullOrEmpty(GateList[i].GateAddress) || GateList[i].GatePort <= 0)
                {
                    logger.Info("配置文件节点[GateAddress{I}]配置错误或端口错误.", i);
                    continue;
                }
            }
            GateConfig.MessageWorkThread = ReadWriteByte("GameGate", "MessageWorkThread", 1);
            if (GateConfig.MessageWorkThread <= 0)
            {
                GateConfig.MessageWorkThread = 1;
            }
            if (GateConfig.MessageWorkThread >= byte.MaxValue)
            {
                GateConfig.MessageWorkThread = byte.MaxValue;
            }
            GateConfig.LogLevel = ReadWriteByte("GameGate", "LogLevel", GateConfig.LogLevel);
            GateConfig.Debug = ReadWriteBool("GameGate", "Debug", GateConfig.Debug);

            GateConfig.UseCloudGate = ReadWriteBool("Cloud", "UseCloudGate", GateConfig.UseCloudGate);
            if (GateConfig.UseCloudGate)
            {
                GateConfig.CloudAddr = ReadWriteString("Cloud", "CloudAddr", GateConfig.CloudAddr);
                GateConfig.CloudPort = ReadWriteInteger("Cloud", "CloudPort", GateConfig.CloudPort);
                GateConfig.LicenseCode = ReadWriteString("Cloud", "LicenseCode", GateConfig.LicenseCode);
            }

            GateConfig.SpaceMoveCommand = ReadWriteString("Strings", "CMDSpaceMove", GateConfig.SpaceMoveCommand);
            GateConfig.ClientOverCntMsg = ReadWriteString("Strings", "OverClientCntMsg", GateConfig.ClientOverCntMsg);
            GateConfig.HWIDBlockedMsg = ReadWriteString("Strings", "HWIDBlockedMsg", GateConfig.HWIDBlockedMsg);
            GateConfig.ChatFilterReplace = ReadWriteString("Strings", "ChatFilterReplace", GateConfig.ChatFilterReplace);
            GateConfig.OverSpeedSendBack = ReadWriteString("Strings", "OverSpeedSendBack", GateConfig.OverSpeedSendBack);
            GateConfig.PacketDecryptFailed = ReadWriteString("Strings", "PacketDecryptFailed", GateConfig.PacketDecryptFailed);
            GateConfig.BlockHWIDFileName = ReadWriteString("Strings", "BlockHWIDFileName", GateConfig.BlockHWIDFileName);
            GateConfig.m_fAddLog = ReadWriteBool("Switch", "AddLog", GateConfig.m_fAddLog);
            GateConfig.PunishMoveInterval = ReadWriteInteger("Integer", "PunishMoveInterval", GateConfig.PunishMoveInterval);
            GateConfig.PunishSpellInterval = ReadWriteInteger("Integer", "PunishSpellInterval", GateConfig.PunishSpellInterval);
            GateConfig.PunishAttackInterval = ReadWriteInteger("Integer", "PunishAttackInterval", GateConfig.PunishAttackInterval);
            GateConfig.MaxItemSpeed = ReadWriteInteger("Integer", "MaxItemSpeed", GateConfig.MaxItemSpeed);
            GateConfig.MaxItemSpeedRate = ReadWriteInteger("Integer", "MaxItemSpeedRate", GateConfig.MaxItemSpeedRate);
            GateConfig.MaxConnectOfIP = ReadWriteInteger("Integer", "MaxConnectOfIP", GateConfig.MaxConnectOfIP);
            GateConfig.MaxClientCount = ReadWriteInteger("Integer", "MaxClientCount", GateConfig.MaxClientCount);
            GateConfig.ClientTimeOutTime = ReadWriteInteger("Integer", "ClientTimeOutTime", GateConfig.ClientTimeOutTime);
            if (GateConfig.ClientTimeOutTime < 10 * 1000)
            {
                GateConfig.ClientTimeOutTime = 10 * 1000;
                WriteInteger("Integer", "ClientTimeOutTime", GateConfig.ClientTimeOutTime);
            }
            GateConfig.ClientTimeOutTime = ReadWriteInteger("Integer", "ClientTimeOutTime", GateConfig.ClientTimeOutTime);
            GateConfig.NomClientPacketSize = ReadWriteInteger("Integer", "NomClientPacketSize", GateConfig.NomClientPacketSize);
            GateConfig.MaxClientPacketSize = ReadWriteInteger("Integer", "MaxClientPacketSize", GateConfig.MaxClientPacketSize);
            GateConfig.MaxClientPacketCount = ReadWriteInteger("Integer", "MaxClientPacketCount", GateConfig.MaxClientPacketCount);
            GateConfig.ChatInterval = ReadWriteInteger("Integer", "ChatInterval", GateConfig.ChatInterval);
            GateConfig.TurnInterval = ReadWriteInteger("Integer", "TurnInterval", GateConfig.TurnInterval);
            GateConfig.MoveInterval = ReadWriteInteger("Integer", "MoveInterval", GateConfig.MoveInterval);
            GateConfig.SpellNextInterval = ReadWriteInteger("Integer", "SpellNextInterval", GateConfig.SpellNextInterval);
            GateConfig.AttackInterval = ReadWriteInteger("Integer", "AttackInterval", GateConfig.AttackInterval);
            GateConfig.ButchInterval = ReadWriteInteger("Integer", "ButchInterval", GateConfig.ButchInterval);
            GateConfig.SitDownInterval = ReadWriteInteger("Integer", "SitDownInterval", GateConfig.SitDownInterval);
            GateConfig.PickupInterval = ReadWriteInteger("Integer", "PickupInterval", GateConfig.PickupInterval);
            GateConfig.EatInterval = ReadWriteInteger("Integer", "EatInterval", GateConfig.EatInterval);
            GateConfig.MoveNextSpellCompensate = ReadWriteInteger("Integer", "MoveNextSpellCompensate", GateConfig.MoveNextSpellCompensate);
            GateConfig.MoveNextAttackCompensate = ReadWriteInteger("Integer", "MoveNextAttackCompensate", GateConfig.MoveNextAttackCompensate);
            GateConfig.AttackNextMoveCompensate = ReadWriteInteger("Integer", "AttackNextMoveCompensate", GateConfig.AttackNextMoveCompensate);
            GateConfig.AttackNextSpellCompensate = ReadWriteInteger("Integer", "AttackNextSpellCompensate", GateConfig.AttackNextSpellCompensate);
            GateConfig.SpellNextMoveCompensate = ReadWriteInteger("Integer", "SpellNextMoveCompensate", GateConfig.SpellNextMoveCompensate);
            GateConfig.SpellNextAttackCompensate = ReadWriteInteger("Integer", "SpellNextAttackCompensate", GateConfig.SpellNextAttackCompensate);
            GateConfig.SpaceMoveNextPickupInterval = ReadWriteInteger("Integer", "SpaceMoveNextPickupInterval", GateConfig.SpaceMoveNextPickupInterval);
            GateConfig.PunishBaseInterval = ReadWriteInteger("Integer", "PunishBaseInterval", GateConfig.PunishBaseInterval);
            GateConfig.ClientMoveSpeedRate = ReadWriteInteger("Integer", "ClientMoveSpeedRate", GateConfig.ClientMoveSpeedRate);
            GateConfig.ClientSpellSpeedRate = ReadWriteInteger("Integer", "ClientSpellSpeedRate", GateConfig.ClientSpellSpeedRate);
            GateConfig.ClientAttackSpeedRate = ReadWriteInteger("Integer", "ClientAttackSpeedRate", GateConfig.ClientAttackSpeedRate);
            // Method
            GateConfig.OverSpeedPunishMethod = (PunishMethod)ReadWriteInteger("Method", "OverSpeedPunishMethod", (int)GateConfig.OverSpeedPunishMethod);
            GateConfig.BlockIPMethod = (BlockIPMethod)ReadWriteInteger("Method", "BlockIPMethod", (int)GateConfig.BlockIPMethod);
            GateConfig.ChatFilterMethod = (ChatFilterMethod)ReadWriteInteger("Method", "ChatFilterMethod", (int)GateConfig.ChatFilterMethod);
            GateConfig.SpeedHackWarnMethod = (OverSpeedMsgMethod)ReadWriteInteger("Method", "SpeedHackWarnMethod", (int)GateConfig.SpeedHackWarnMethod);
            // Boolean
            GateConfig.CheckNullSession = ReadWriteBool("Switch", "CheckNullSession", GateConfig.CheckNullSession);
            GateConfig.IsOverSpeedSendBack = ReadWriteBool("Switch", "OverSpeedSendBack", GateConfig.IsOverSpeedSendBack);
            GateConfig.IsDefenceCCPacket = ReadWriteBool("Switch", "DefenceCCPacket", GateConfig.IsDefenceCCPacket);
            GateConfig.IsKickOverSpeed = ReadWriteBool("Switch", "KickOverSpeed", GateConfig.IsKickOverSpeed);
            GateConfig.IsDoMotaeboSpeedCheck = ReadWriteBool("Switch", "DoMotaeboSpeedCheck", GateConfig.IsDoMotaeboSpeedCheck);
            GateConfig.IsDenyPresend = ReadWriteBool("Switch", "DenyPresend", GateConfig.IsDenyPresend);
            GateConfig.IsItemSpeedCompensate = ReadWriteBool("Switch", "ItemSpeedCompensate", GateConfig.IsItemSpeedCompensate);
            GateConfig.IsKickOverPacketSize = ReadWriteBool("Switch", "KickOverPacketSize", GateConfig.IsKickOverPacketSize);
            GateConfig.IsChatFilter = ReadWriteBool("Switch", "ChatFilter", GateConfig.IsChatFilter);
            GateConfig.IsChatInterval = ReadWriteBool("Switch", "ChatInterval", GateConfig.IsChatInterval);
            GateConfig.IsChatCmdFilter = ReadWriteBool("Switch", "ChatCmdFilter", GateConfig.IsChatCmdFilter);
            GateConfig.IsTurnInterval = ReadWriteBool("Switch", "TurnInterval", GateConfig.IsTurnInterval);
            GateConfig.IsMoveInterval = ReadWriteBool("Switch", "MoveInterval", GateConfig.IsMoveInterval);
            GateConfig.IsSpellInterval = ReadWriteBool("Switch", "SpellInterval", GateConfig.IsSpellInterval);
            GateConfig.IsAttackInterval = ReadWriteBool("Switch", "AttackInterval", GateConfig.IsAttackInterval);
            GateConfig.IsButchInterval = ReadWriteBool("Switch", "ButchInterval", GateConfig.IsButchInterval);
            GateConfig.IsSitDownInterval = ReadWriteBool("Switch", "SitDownInterval", GateConfig.IsSitDownInterval);
            GateConfig.IsSpaceMoveNextPickupInterval = ReadWriteBool("Switch", "SpaceMoveNextPickupInterval", GateConfig.IsSpaceMoveNextPickupInterval);
            GateConfig.IsPickupInterval = ReadWriteBool("Switch", "PickupInterval", GateConfig.IsPickupInterval);
            GateConfig.IsEatInterval = ReadWriteBool("Switch", "EatInterval", GateConfig.IsEatInterval);
            GateConfig.IsProcClientHardwareID = ReadWriteBool("Switch", "ProcClientCount", GateConfig.IsProcClientHardwareID);
            GateConfig.ProClientHardwareKey = ReadWriteString("Switch", "ProClientHardwareKey", GateConfig.ProClientHardwareKey);
            GateConfig.ClientShowHintNewType = ReadWriteBool("Switch", "ClientShowHintNewType", GateConfig.ClientShowHintNewType);
            GateConfig.OpenClientSpeedRate = ReadWriteBool("Switch", "OpenClientSpeedRate", GateConfig.OpenClientSpeedRate);
            GateConfig.SyncClientSpeed = ReadWriteBool("Switch", "SyncClientSpeed", GateConfig.SyncClientSpeed);
            GateConfig.PunishIntervalRate = ReadWriteFloat("Float", "PunishIntervalRate", GateConfig.PunishIntervalRate);

            //魔法间隔控制
            for (var i = 0; i < TableDef.MaigicDelayTimeList.Length; i++)
            {
                if (!string.IsNullOrEmpty(TableDef.MaigicNameList[i]))
                {
                    TableDef.MaigicDelayTimeList[i] = ReadWriteInteger("MagicInterval", TableDef.MaigicNameList[i], TableDef.MaigicDelayTimeList[i]);
                }
            }
        }

        public void ReLoadConfig()
        {
            Clear();
            Load();
            LoadConfig();
        }

        public void SaveConfig()
        {
            Save();
        }
    }

    public class GameGateInfo
    {
        public byte ServiceId;
        /// <summary>
        /// 网关ID
        /// </summary>
        public string ThreadId;
        public string ServerAdress;
        public int ServerPort;
        public string GateAddress;
        public int GatePort;
    }
}