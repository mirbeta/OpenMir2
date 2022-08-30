using System;
using System.IO;
using SystemModule.Common;

namespace CloudGate.Conf
{
    public class ConfigManager : IniFile
    {
        private static readonly string fileName = Path.Combine(AppContext.BaseDirectory, "config.conf");

        private static readonly ConfigManager instance = new ConfigManager(fileName);

        public static ConfigManager Instance => instance;

        public GateConfig GateConfig;
        public GameGateInfo[] GameGateList;

        public ConfigManager(string fileName) : base(fileName)
        {
            GateConfig = new GateConfig();
            GameGateList = new GameGateInfo[32];
            for (var i = 0; i < GameGateList.Length; i++)
            {
                GameGateList[i] = new GameGateInfo();
                GameGateList[i].ServerAdress = "127.0.0.1";
                GameGateList[i].ServerPort = 5000;
                GameGateList[i].GateAddress = "127.0.0.1";
                GameGateList[i].GatePort = 7900 + i;
            }
        }

        public void LoadConfig()
        {
            if (File.Exists(fileName))
            {
                Load();
            }
            GateConfig.MessageThread = ReadInteger("Integer", "MessageThread", GateConfig.MessageThread);
            if (GateConfig.MessageThread > 4)
            {
                GateConfig.MessageThread = 4;
            }
            GateConfig.m_szCMDSpaceMove = ReadString("Strings", "CMDSpaceMove", GateConfig.m_szCMDSpaceMove);
            GateConfig.m_szOverClientCntMsg = ReadString("Strings", "OverClientCntMsg", GateConfig.m_szOverClientCntMsg);
            GateConfig.m_szHWIDBlockedMsg = ReadString("Strings", "HWIDBlockedMsg", GateConfig.m_szHWIDBlockedMsg);
            GateConfig.m_szChatFilterReplace = ReadString("Strings", "ChatFilterReplace", GateConfig.m_szChatFilterReplace);
            GateConfig.m_szOverSpeedSendBack = ReadString("Strings", "OverSpeedSendBack", GateConfig.m_szOverSpeedSendBack);
            GateConfig.m_szPacketDecryptFailed = ReadString("Strings", "PacketDecryptFailed", GateConfig.m_szPacketDecryptFailed);
            GateConfig.m_szBlockHWIDFileName = ReadString("Strings", "BlockHWIDFileName", GateConfig.m_szBlockHWIDFileName);
            GateConfig.m_fAddLog = ReadBool("Switch", "AddLog", GateConfig.m_fAddLog);
            GateConfig.ShowLogLevel = ReadInteger("Integer", "ShowLogLevel", GateConfig.ShowLogLevel);
            GateConfig.ShowDebugLog = ReadBool("Integer", "ShowDebugLog", GateConfig.ShowDebugLog);
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
            GateConfig.OverSpeedPunishMethod = (TPunishMethod)ReadInteger("Method", "OverSpeedPunishMethod", (int)GateConfig.OverSpeedPunishMethod);
            GateConfig.BlockIPMethod = (TBlockIPMethod)ReadInteger("Method", "BlockIPMethod", (int)GateConfig.BlockIPMethod);
            GateConfig.ChatFilterMethod = (TChatFilterMethod)ReadInteger("Method", "ChatFilterMethod", (int)GateConfig.ChatFilterMethod);
            GateConfig.SpeedHackWarnMethod = (TOverSpeedMsgMethod)ReadInteger("Method", "SpeedHackWarnMethod", (int)GateConfig.SpeedHackWarnMethod);
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
            GateConfig.PunishIntervalRate = ReadFloat("Switch", "PunishIntervalRate", GateConfig.PunishIntervalRate);
            GateConfig.GateCount = ReadInteger("CloudGate", "Count", GateConfig.GateCount);
            for (var i = 0; i < GateConfig.GateCount; i++)
            {
                GameGateList[i].ServerAdress = ReadString("CloudGate", "ServerAddr" + (i + 1), GameGateList[i].ServerAdress);
                GameGateList[i].ServerPort = ReadInteger("CloudGate", "ServerPort" + (i + 1), GameGateList[i].ServerPort);
                GameGateList[i].GateAddress = ReadString("CloudGate", "GateAddress" + (i + 1), GameGateList[i].GateAddress);
                GameGateList[i].GatePort = ReadInteger("CloudGate", "GatePort" + (i + 1), GameGateList[i].GatePort);
            }
            //魔法间隔控制
            for (var i = 0; i < TableDef.MaigicDelayTimeList.Length; i++)
            {
                if (!string.IsNullOrEmpty(TableDef.MaigicNameList[i]))
                {
                    TableDef.MaigicDelayTimeList[i] = ReadInteger("MagicInterval", TableDef.MaigicNameList[i], TableDef.MaigicDelayTimeList[i]);
                }
            }

            if (ConfigCount <= 0)
            {
                SaveConfig();
                Save();
                ReLoadConfig();
            }
        }

        public void ReLoadConfig()
        {
            Clear();
            Load();
            LoadConfig();
        }

        private void SaveConfig()
        {
            WriteInteger("CloudGate", "Count", GateConfig.GateCount);
            for (var i = 0; i < GateConfig.GateCount; i++)
            {
                WriteString("CloudGate", "ServerAddr" + (i + 1), GameGateList[i].ServerAdress);
                WriteInteger("CloudGate", "ServerPort" + (i + 1), GameGateList[i].ServerPort);
                WriteString("CloudGate", "GateAddress" + (i + 1), GameGateList[i].GateAddress);
                WriteInteger("CloudGate", "GatePort" + (i + 1), GameGateList[i].GatePort);
            }
            WriteInteger("Integer", "MessageThread", GateConfig.MessageThread);
            WriteString("Strings", "CMDSpaceMove", GateConfig.m_szCMDSpaceMove);
            WriteString("Strings", "OverClientCntMsg", GateConfig.m_szOverClientCntMsg);
            WriteString("Strings", "HWIDBlockedMsg", GateConfig.m_szHWIDBlockedMsg);
            WriteString("Strings", "ChatFilterReplace", GateConfig.m_szChatFilterReplace);
            WriteString("Strings", "OverSpeedSendBack", GateConfig.m_szOverSpeedSendBack);
            WriteString("Strings", "PacketDecryptFailed", GateConfig.m_szPacketDecryptFailed);
            WriteString("Strings", "BlockHWIDFileName", GateConfig.m_szBlockHWIDFileName);
            WriteBool("Switch", "AddLog", GateConfig.m_fAddLog);
            WriteInteger("Integer", "ShowLogLevel", GateConfig.ShowLogLevel);
            WriteBool("Integer", "ShowDebugLog", GateConfig.ShowDebugLog);
            WriteInteger("Integer", "PunishMoveInterval", GateConfig.PunishMoveInterval);
            WriteInteger("Integer", "PunishSpellInterval", GateConfig.PunishSpellInterval);
            WriteInteger("Integer", "PunishAttackInterval", GateConfig.PunishAttackInterval);
            WriteInteger("Integer", "MaxItemSpeed", GateConfig.MaxItemSpeed);
            WriteInteger("Integer", "MaxItemSpeedRate", GateConfig.MaxItemSpeedRate);
            WriteInteger("Integer", "MaxConnectOfIP", GateConfig.MaxConnectOfIP);
            WriteInteger("Integer", "MaxClientCount", GateConfig.MaxClientCount);
            WriteInteger("Integer", "ClientTimeOutTime", GateConfig.ClientTimeOutTime);
            if (GateConfig.ClientTimeOutTime < 10 * 1000)
            {
                GateConfig.ClientTimeOutTime = 10 * 1000;
                WriteInteger("Integer", "ClientTimeOutTime", GateConfig.ClientTimeOutTime);
            }

            WriteInteger("Integer", "ClientTimeOutTime", GateConfig.ClientTimeOutTime);
            WriteInteger("Integer", "NomClientPacketSize", GateConfig.NomClientPacketSize);
            WriteInteger("Integer", "MaxClientPacketSize", GateConfig.MaxClientPacketSize);
            WriteInteger("Integer", "MaxClientPacketCount", GateConfig.MaxClientPacketCount);
            WriteInteger("Integer", "ChatInterval", GateConfig.ChatInterval);
            WriteInteger("Integer", "TurnInterval", GateConfig.TurnInterval);
            WriteInteger("Integer", "MoveInterval", GateConfig.MoveInterval);
            WriteInteger("Integer", "SpellNextInterval", GateConfig.SpellNextInterval);
            WriteInteger("Integer", "AttackInterval", GateConfig.AttackInterval);
            WriteInteger("Integer", "ButchInterval", GateConfig.ButchInterval);
            WriteInteger("Integer", "SitDownInterval", GateConfig.SitDownInterval);
            WriteInteger("Integer", "PickupInterval", GateConfig.PickupInterval);
            WriteInteger("Integer", "EatInterval", GateConfig.EatInterval);
            WriteInteger("Integer", "MoveNextSpellCompensate", GateConfig.MoveNextSpellCompensate);
            WriteInteger("Integer", "MoveNextAttackCompensate", GateConfig.MoveNextAttackCompensate);
            WriteInteger("Integer", "AttackNextMoveCompensate", GateConfig.AttackNextMoveCompensate);
            WriteInteger("Integer", "AttackNextSpellCompensate", GateConfig.AttackNextSpellCompensate);
            WriteInteger("Integer", "SpellNextMoveCompensate", GateConfig.SpellNextMoveCompensate);
            WriteInteger("Integer", "SpellNextAttackCompensate", GateConfig.SpellNextAttackCompensate);
            WriteInteger("Integer", "SpaceMoveNextPickupInterval", GateConfig.SpaceMoveNextPickupInterval);
            WriteInteger("Integer", "PunishBaseInterval", GateConfig.PunishBaseInterval);
            WriteInteger("Integer", "ClientMoveSpeedRate", GateConfig.ClientMoveSpeedRate);
            WriteInteger("Integer", "ClientSpellSpeedRate", GateConfig.ClientSpellSpeedRate);
            WriteInteger("Integer", "ClientAttackSpeedRate", GateConfig.ClientAttackSpeedRate);
            WriteInteger("Method", "OverSpeedPunishMethod", (int) GateConfig.OverSpeedPunishMethod);
            WriteInteger("Method", "BlockIPMethod", (int) GateConfig.BlockIPMethod);
            WriteInteger("Method", "ChatFilterMethod", (int) GateConfig.ChatFilterMethod);
            WriteInteger("Method", "SpeedHackWarnMethod", (int) GateConfig.SpeedHackWarnMethod);
            WriteBool("Switch", "CheckNullSession", GateConfig.CheckNullSession);
            WriteBool("Switch", "OverSpeedSendBack", GateConfig.IsOverSpeedSendBack);
            WriteBool("Switch", "DefenceCCPacket", GateConfig.IsDefenceCCPacket);
            WriteBool("Switch", "KickOverSpeed", GateConfig.IsKickOverSpeed);
            WriteBool("Switch", "DoMotaeboSpeedCheck", GateConfig.IsDoMotaeboSpeedCheck);
            WriteBool("Switch", "DenyPresend", GateConfig.IsDenyPresend);
            WriteBool("Switch", "ItemSpeedCompensate", GateConfig.IsItemSpeedCompensate);
            WriteBool("Switch", "KickOverPacketSize", GateConfig.IsKickOverPacketSize);
            WriteBool("Switch", "ChatFilter", GateConfig.IsChatFilter);
            WriteBool("Switch", "ChatInterval", GateConfig.IsChatInterval);
            WriteBool("Switch", "ChatCmdFilter", GateConfig.IsChatCmdFilter);
            WriteBool("Switch", "TurnInterval", GateConfig.IsTurnInterval);
            WriteBool("Switch", "MoveInterval", GateConfig.IsMoveInterval);
            WriteBool("Switch", "SpellInterval", GateConfig.IsSpellInterval);
            WriteBool("Switch", "AttackInterval", GateConfig.IsAttackInterval);
            WriteBool("Switch", "ButchInterval", GateConfig.IsButchInterval);
            WriteBool("Switch", "SitDownInterval", GateConfig.IsSitDownInterval);
            WriteBool("Switch", "SpaceMoveNextPickupInterval", GateConfig.IsSpaceMoveNextPickupInterval);
            WriteBool("Switch", "PickupInterval", GateConfig.IsPickupInterval);
            WriteBool("Switch", "EatInterval", GateConfig.IsEatInterval);
            WriteBool("Switch", "ProcClientCount", GateConfig.IsProcClientHardwareID);
            WriteString("Switch", "ProClientHardwareKey", GateConfig.ProClientHardwareKey);
            WriteBool("Switch", "ClientShowHintNewType", GateConfig.ClientShowHintNewType);
            WriteBool("Switch", "OpenClientSpeedRate", GateConfig.OpenClientSpeedRate);
            WriteBool("Switch", "SyncClientSpeed", GateConfig.SyncClientSpeed);
            WriteInteger("Switch", "PunishIntervalRate", GateConfig.PunishIntervalRate);
            //魔法间隔控制
            for (var i = 0; i < TableDef.MaigicDelayTimeList.Length; i++)
            {
                if (!string.IsNullOrEmpty(TableDef.MaigicNameList[i]))
                {
                    WriteInteger("MagicInterval", TableDef.MaigicNameList[i], TableDef.MaigicDelayTimeList[i]);
                }
            }
        }
    }

    public class GameGateInfo
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string ServerAdress;
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int ServerPort;
        /// <summary>
        /// 网关服务地址
        /// </summary>
        public string GateAddress;
        /// <summary>
        /// 网关服务端口
        /// </summary>
        public int GatePort;
    }
}