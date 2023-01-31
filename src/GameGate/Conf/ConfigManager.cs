using NLog;
using System;
using System.IO;
using SystemModule.Common;

namespace GameGate.Conf
{
    public class ConfigManager : IniFile
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly string fileName = Path.Combine(AppContext.BaseDirectory, "config.conf");
        private static readonly ConfigManager instance = new ConfigManager(fileName);
        public static ConfigManager Instance => instance;
        public GateConfig GateConfig;
        public GameGateInfo[] GateList;

        private ConfigManager(string fileName) : base(fileName)
        {
            Load();
            GateConfig = new GateConfig();
            GateList = new GameGateInfo[32];
            for (int i = 0; i < GateList.Length; i++)
            {
                GateList[i] = new GameGateInfo();
                GateList[i].ServiceId = (byte) i;
                GateList[i].ServerAdress = "127.0.0.1";
                GateList[i].ServerPort = 5000;
                GateList[i].GateAddress = "127.0.0.1";
                GateList[i].GatePort = 7200 + i;
                GateList[i].ThreadId = Guid.NewGuid().ToString("N");
            }
        }

        public void LoadConfig()
        {
            GateConfig.UseCloudGate = ReadBool("Cloud", "UseCloudGate", GateConfig.UseCloudGate);
            if (GateConfig.UseCloudGate)
            {
                GateConfig.CloudAddr = ReadString("Cloud", "CloudAddr", GateConfig.CloudAddr);
                GateConfig.CloudPort = ReadInteger("Cloud", "CloudPort", GateConfig.CloudPort);
                GateConfig.LicenseCode = ReadString("Cloud", "LicenseCode", GateConfig.LicenseCode);
            }
            GateConfig.SpaceMoveCommand = ReadString("Strings", "CMDSpaceMove", GateConfig.SpaceMoveCommand);
            GateConfig.ClientOverCntMsg = ReadString("Strings", "OverClientCntMsg", GateConfig.ClientOverCntMsg);
            GateConfig.HWIDBlockedMsg = ReadString("Strings", "HWIDBlockedMsg", GateConfig.HWIDBlockedMsg);
            GateConfig.ChatFilterReplace = ReadString("Strings", "ChatFilterReplace", GateConfig.ChatFilterReplace);
            GateConfig.OverSpeedSendBack = ReadString("Strings", "OverSpeedSendBack", GateConfig.OverSpeedSendBack);
            GateConfig.PacketDecryptFailed = ReadString("Strings", "PacketDecryptFailed", GateConfig.PacketDecryptFailed);
            GateConfig.BlockHWIDFileName = ReadString("Strings", "BlockHWIDFileName", GateConfig.BlockHWIDFileName);
            GateConfig.m_fAddLog = ReadBool("Switch", "AddLog", GateConfig.m_fAddLog);
            GateConfig.PunishMoveInterval = ReadInteger("Integer", "PunishMoveInterval", GateConfig.PunishMoveInterval);
            GateConfig.PunishSpellInterval = ReadInteger("Integer", "PunishSpellInterval", GateConfig.PunishSpellInterval);
            GateConfig.PunishAttackInterval = ReadInteger("Integer", "PunishAttackInterval", GateConfig.PunishAttackInterval);
            GateConfig.MaxItemSpeed = ReadInteger("Integer", "MaxItemSpeed", GateConfig.MaxItemSpeed);
            GateConfig.MaxItemSpeedRate = ReadInteger("Integer", "MaxItemSpeedRate", GateConfig.MaxItemSpeedRate);
            GateConfig.MaxConnectOfIP = ReadInteger("Integer", "MaxConnectOfIP", GateConfig.MaxConnectOfIP);
            GateConfig.MaxClientCount = ReadInteger("Integer", "MaxClientCount", GateConfig.MaxClientCount);
            GateConfig.ClientTimeOutTime = ReadInteger("Integer", "ClientTimeOutTime", GateConfig.ClientTimeOutTime);
            if (GateConfig.ClientTimeOutTime < 10 * 1000)
            {
                GateConfig.ClientTimeOutTime = 10 * 1000;
                WriteInteger("Integer", "ClientTimeOutTime", GateConfig.ClientTimeOutTime);
            }
            GateConfig.ClientTimeOutTime = ReadInteger("Integer", "ClientTimeOutTime", GateConfig.ClientTimeOutTime);
            GateConfig.NomClientPacketSize = ReadInteger("Integer", "NomClientPacketSize", GateConfig.NomClientPacketSize);
            GateConfig.MaxClientPacketSize = ReadInteger("Integer", "MaxClientPacketSize", GateConfig.MaxClientPacketSize);
            GateConfig.MaxClientPacketCount = ReadInteger("Integer", "MaxClientPacketCount", GateConfig.MaxClientPacketCount);
            GateConfig.ChatInterval = ReadInteger("Integer", "ChatInterval", GateConfig.ChatInterval);
            GateConfig.TurnInterval = ReadInteger("Integer", "TurnInterval", GateConfig.TurnInterval);
            GateConfig.MoveInterval = ReadInteger("Integer", "MoveInterval", GateConfig.MoveInterval);
            GateConfig.SpellNextInterval = ReadInteger("Integer", "SpellNextInterval", GateConfig.SpellNextInterval);
            GateConfig.AttackInterval = ReadInteger("Integer", "AttackInterval", GateConfig.AttackInterval);
            GateConfig.ButchInterval = ReadInteger("Integer", "ButchInterval", GateConfig.ButchInterval);
            GateConfig.SitDownInterval = ReadInteger("Integer", "SitDownInterval", GateConfig.SitDownInterval);
            GateConfig.PickupInterval = ReadInteger("Integer", "PickupInterval", GateConfig.PickupInterval);
            GateConfig.EatInterval = ReadInteger("Integer", "EatInterval", GateConfig.EatInterval);
            GateConfig.MoveNextSpellCompensate = ReadInteger("Integer", "MoveNextSpellCompensate", GateConfig.MoveNextSpellCompensate);
            GateConfig.MoveNextAttackCompensate = ReadInteger("Integer", "MoveNextAttackCompensate", GateConfig.MoveNextAttackCompensate);
            GateConfig.AttackNextMoveCompensate = ReadInteger("Integer", "AttackNextMoveCompensate", GateConfig.AttackNextMoveCompensate);
            GateConfig.AttackNextSpellCompensate = ReadInteger("Integer", "AttackNextSpellCompensate", GateConfig.AttackNextSpellCompensate);
            GateConfig.SpellNextMoveCompensate = ReadInteger("Integer", "SpellNextMoveCompensate", GateConfig.SpellNextMoveCompensate);
            GateConfig.SpellNextAttackCompensate = ReadInteger("Integer", "SpellNextAttackCompensate", GateConfig.SpellNextAttackCompensate);
            GateConfig.SpaceMoveNextPickupInterval = ReadInteger("Integer", "SpaceMoveNextPickupInterval", GateConfig.SpaceMoveNextPickupInterval);
            GateConfig.PunishBaseInterval = ReadInteger("Integer", "PunishBaseInterval", GateConfig.PunishBaseInterval);
            GateConfig.ClientMoveSpeedRate = ReadInteger("Integer", "ClientMoveSpeedRate", GateConfig.ClientMoveSpeedRate);
            GateConfig.ClientSpellSpeedRate = ReadInteger("Integer", "ClientSpellSpeedRate", GateConfig.ClientSpellSpeedRate);
            GateConfig.ClientAttackSpeedRate = ReadInteger("Integer", "ClientAttackSpeedRate", GateConfig.ClientAttackSpeedRate);
            // Method
            GateConfig.OverSpeedPunishMethod = (TPunishMethod)ReadInteger("Method", "OverSpeedPunishMethod", (int)GateConfig.OverSpeedPunishMethod);
            GateConfig.BlockIPMethod = (TBlockIPMethod)ReadInteger("Method", "BlockIPMethod", (int)GateConfig.BlockIPMethod);
            GateConfig.ChatFilterMethod = (ChatFilterMethod)ReadInteger("Method", "ChatFilterMethod", (int)GateConfig.ChatFilterMethod);
            GateConfig.SpeedHackWarnMethod = (OverSpeedMsgMethod)ReadInteger("Method", "SpeedHackWarnMethod", (int)GateConfig.SpeedHackWarnMethod);
            // Boolean
            GateConfig.CheckNullSession = ReadBool("Switch", "CheckNullSession", GateConfig.CheckNullSession);
            GateConfig.IsOverSpeedSendBack = ReadBool("Switch", "OverSpeedSendBack", GateConfig.IsOverSpeedSendBack);
            GateConfig.IsDefenceCCPacket = ReadBool("Switch", "DefenceCCPacket", GateConfig.IsDefenceCCPacket);
            GateConfig.IsKickOverSpeed = ReadBool("Switch", "KickOverSpeed", GateConfig.IsKickOverSpeed);
            GateConfig.IsDoMotaeboSpeedCheck = ReadBool("Switch", "DoMotaeboSpeedCheck", GateConfig.IsDoMotaeboSpeedCheck);
            GateConfig.IsDenyPresend = ReadBool("Switch", "DenyPresend", GateConfig.IsDenyPresend);
            GateConfig.IsItemSpeedCompensate = ReadBool("Switch", "ItemSpeedCompensate", GateConfig.IsItemSpeedCompensate);
            GateConfig.IsKickOverPacketSize = ReadBool("Switch", "KickOverPacketSize", GateConfig.IsKickOverPacketSize);
            GateConfig.IsChatFilter = ReadBool("Switch", "ChatFilter", GateConfig.IsChatFilter);
            GateConfig.IsChatInterval = ReadBool("Switch", "ChatInterval", GateConfig.IsChatInterval);
            GateConfig.IsChatCmdFilter = ReadBool("Switch", "ChatCmdFilter", GateConfig.IsChatCmdFilter);
            GateConfig.IsTurnInterval = ReadBool("Switch", "TurnInterval", GateConfig.IsTurnInterval);
            GateConfig.IsMoveInterval = ReadBool("Switch", "MoveInterval", GateConfig.IsMoveInterval);
            GateConfig.IsSpellInterval = ReadBool("Switch", "SpellInterval", GateConfig.IsSpellInterval);
            GateConfig.IsAttackInterval = ReadBool("Switch", "AttackInterval", GateConfig.IsAttackInterval);
            GateConfig.IsButchInterval = ReadBool("Switch", "ButchInterval", GateConfig.IsButchInterval);
            GateConfig.IsSitDownInterval = ReadBool("Switch", "SitDownInterval", GateConfig.IsSitDownInterval);
            GateConfig.IsSpaceMoveNextPickupInterval = ReadBool("Switch", "SpaceMoveNextPickupInterval", GateConfig.IsSpaceMoveNextPickupInterval);
            GateConfig.IsPickupInterval = ReadBool("Switch", "PickupInterval", GateConfig.IsPickupInterval);
            GateConfig.IsEatInterval = ReadBool("Switch", "EatInterval", GateConfig.IsEatInterval);
            GateConfig.IsProcClientHardwareID = ReadBool("Switch", "ProcClientCount", GateConfig.IsProcClientHardwareID);
            GateConfig.ProClientHardwareKey = ReadString("Switch", "ProClientHardwareKey", GateConfig.ProClientHardwareKey);
            GateConfig.ClientShowHintNewType = ReadBool("Switch", "ClientShowHintNewType", GateConfig.ClientShowHintNewType);
            GateConfig.OpenClientSpeedRate = ReadBool("Switch", "OpenClientSpeedRate", GateConfig.OpenClientSpeedRate);
            GateConfig.SyncClientSpeed = ReadBool("Switch", "SyncClientSpeed", GateConfig.SyncClientSpeed);
            GateConfig.PunishIntervalRate = ReadFloat("Float", "PunishIntervalRate", GateConfig.PunishIntervalRate);
            GateConfig.ServerWorkThread = (byte)ReadInteger("GameGate", "ServerWorkThread", 1);
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
                GateList[i].ServerAdress = ReadString("GameGate", "ServerAddr" + (i + 1), GateList[i].ServerAdress);
                GateList[i].ServerPort = ReadInteger("GameGate", "ServerPort" + (i + 1), GateList[i].ServerPort);
                GateList[i].GateAddress = ReadString("GameGate", "GateAddress" + (i + 1), GateList[i].GateAddress);
                GateList[i].GatePort = ReadInteger("GameGate", "GatePort" + (i + 1), GateList[i].GatePort);
                if (string.IsNullOrEmpty(GateList[i].ServerAdress) || GateList[i].ServerPort <= 0)
                {
                    logger.Info($"配置文件节点[ServerAddr{i}]配置错误或端口错误.", 1);
                    continue;
                }
                if (string.IsNullOrEmpty(GateList[i].GateAddress) || GateList[i].GatePort <= 0)
                {
                    logger.Info($"配置文件节点[GateAddress{i}]配置错误或端口错误.", 1);
                    continue;
                }
            }
            GateConfig.MessageWorkThread = ReadInteger("GameGate", "MessageWorkThread", 1);
            if (GateConfig.MessageWorkThread <= 0)
            {
                GateConfig.MessageWorkThread = 1;
            }
            if (GateConfig.MessageWorkThread > byte.MaxValue)
            {
                GateConfig.MessageWorkThread = byte.MaxValue;
            }
            GateConfig.ShowLogLevel = ReadInteger("GameGate", "ShowLogLevel", GateConfig.ShowLogLevel);
            GateConfig.ShowDebugLog = ReadBool("GameGate", "ShowDebugLog", GateConfig.ShowDebugLog);
            //魔法间隔控制
            for (var i = 0; i < TableDef.MaigicDelayTimeList.Length; i++)
            {
                if (!string.IsNullOrEmpty(TableDef.MaigicNameList[i]))
                {
                    TableDef.MaigicDelayTimeList[i] = ReadInteger("MagicInterval", TableDef.MaigicNameList[i], TableDef.MaigicDelayTimeList[i]);
                }
            }
        }

        public void ReLoadConfig()
        {
            Clear();
            Load();
            LoadConfig();
        }
    }

    public struct GameGateInfo
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