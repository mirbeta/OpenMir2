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
            Load();
            GateConfig = new GateConfig();
            GameGateList = new GameGateInfo[32];
            for (int i = 0; i < GameGateList.Length; i++)
            {
                GameGateList[i] = new GameGateInfo();
                GameGateList[i].ServerAdress = "127.0.0.1";
                GameGateList[i].ServerPort = 5000;
                GameGateList[i].GateAddress = "127.0.0.1";
                GameGateList[i].GatePort = 7200 + i;
            }
        }

        public void LoadConfig()
        {
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
            // Method
            GateConfig.OverSpeedPunishMethod = (TPunishMethod)ReadInteger("Method", "OverSpeedPunishMethod", (int)GateConfig.OverSpeedPunishMethod);
            GateConfig.BlockIPMethod = (TBlockIPMethod)ReadInteger("Method", "BlockIPMethod", (int)GateConfig.BlockIPMethod);
            GateConfig.ChatFilterMethod = (TChatFilterMethod)ReadInteger("Method", "ChatFilterMethod", (int)GateConfig.ChatFilterMethod);
            GateConfig.SpeedHackWarnMethod = (TOverSpeedMsgMethod)ReadInteger("Method", "SpeedHackWarnMethod", (int)GateConfig.SpeedHackWarnMethod);
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
            GateConfig.GateCount = ReadInteger("GameGate", "Count", GateConfig.GateCount);
            for (var i = 0; i <= GateConfig.GateCount; i++)
            {
                GameGateList[i].ServerAdress = ReadString("GameGate", "ServerAddr" + (i + 1), GameGateList[i].ServerAdress);
                GameGateList[i].ServerPort = ReadInteger("GameGate", "ServerPort" + (i + 1), GameGateList[i].ServerPort);
                GameGateList[i].GateAddress = ReadString("GameGate", "GateAddress" + (i + 1), GameGateList[i].GateAddress);
                GameGateList[i].GatePort = ReadInteger("GameGate", "GatePort" + (i + 1), GameGateList[i].GatePort);
            }
            //魔法间隔控制
            for (var i = 0; i <= TableDef.MaigicDelayTimeList.GetUpperBound(0); i++)
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

    public class GameGateInfo
    {
        public string ServerAdress;
        public int ServerPort;
        public string GateAddress;
        public int GatePort;
    }
}