using System;
using System.IO;
using SystemModule.Common;

namespace GameGate
{
    public class ConfigManager : IniFile
    {
        private static string fileName = Path.Combine(AppContext.BaseDirectory, "config.conf");

        private static readonly ConfigManager instance = new ConfigManager(fileName);

        public static ConfigManager Instance
        {
            get { return instance; }
        }
        
        public GateConfig GateConfig;
        public GameGateInfo[] m_xGameGateList;

        public ConfigManager(string fileName) : base(fileName)
        {
            Load();
            GateConfig = new GateConfig();
            m_xGameGateList = new GameGateInfo[32];
            for (int i = 0; i < m_xGameGateList.Length; i++)
            {
                m_xGameGateList[i] = new GameGateInfo();
                m_xGameGateList[i].sServerAdress = "127.0.0.1";
                m_xGameGateList[i].nGatePort = 7200 + i;
                m_xGameGateList[i].nServerPort = 5000;
            }
        }

        public void LoadConfig()
        {
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
            GateConfig.m_nMaxItemSpeedRate = ReadInteger("Integer", "MaxItemSpeedRate", GateConfig.m_nMaxItemSpeedRate);
            GateConfig.m_nMaxConnectOfIP = ReadInteger("Integer", "MaxConnectOfIP", GateConfig.m_nMaxConnectOfIP);
            GateConfig.m_nMaxClientCount = ReadInteger("Integer", "MaxClientCount", GateConfig.m_nMaxClientCount);
            GateConfig.m_nClientTimeOutTime = ReadInteger("Integer", "ClientTimeOutTime", GateConfig.m_nClientTimeOutTime);
            if (GateConfig.m_nClientTimeOutTime < 10 * 1000)
            {
                GateConfig.m_nClientTimeOutTime = 10 * 1000;
                WriteInteger("Integer", "ClientTimeOutTime", GateConfig.m_nClientTimeOutTime);
            }
            GateConfig.m_nClientTimeOutTime = ReadInteger("Integer", "ClientTimeOutTime", GateConfig.m_nClientTimeOutTime);
            GateConfig.m_nNomClientPacketSize = ReadInteger("Integer", "NomClientPacketSize", GateConfig.m_nNomClientPacketSize);
            GateConfig.m_nMaxClientPacketSize = ReadInteger("Integer", "MaxClientPacketSize", GateConfig.m_nMaxClientPacketSize);
            GateConfig.m_nMaxClientPacketCount = ReadInteger("Integer", "MaxClientPacketCount", GateConfig.m_nMaxClientPacketCount);
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
            GateConfig.m_fOverSpeedSendBack = ReadBool("Switch", "OverSpeedSendBack", GateConfig.m_fOverSpeedSendBack);
            GateConfig.m_fDefenceCCPacket = ReadBool("Switch", "DefenceCCPacket", GateConfig.m_fDefenceCCPacket);
            GateConfig.m_fKickOverSpeed = ReadBool("Switch", "KickOverSpeed", GateConfig.m_fKickOverSpeed);
            GateConfig.m_fDoMotaeboSpeedCheck = ReadBool("Switch", "DoMotaeboSpeedCheck", GateConfig.m_fDoMotaeboSpeedCheck);
            GateConfig.m_fDenyPresend = ReadBool("Switch", "DenyPresend", GateConfig.m_fDenyPresend);
            GateConfig.m_fItemSpeedCompensate = ReadBool("Switch", "ItemSpeedCompensate", GateConfig.m_fItemSpeedCompensate);
            GateConfig.m_fKickOverPacketSize = ReadBool("Switch", "KickOverPacketSize", GateConfig.m_fKickOverPacketSize);
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
                m_xGameGateList[i].sServerAdress = ReadString("GameGate", "ServerAddr" + i, m_xGameGateList[i].sServerAdress);
                m_xGameGateList[i].nServerPort = ReadInteger("GameGate", "ServerPort" + i, m_xGameGateList[i].nServerPort);
                m_xGameGateList[i].nGatePort = ReadInteger("GameGate", "GatePort" + i, m_xGameGateList[i].nGatePort);
            }
            //魔法间隔控制
            for (var i = 0; i <= TableDef.MAIGIC_DELAY_TIME_LIST.GetUpperBound(0); i++)
            {
                if (!string.IsNullOrEmpty(TableDef.MAIGIC_NAME_LIST[i]))
                {
                    TableDef.MAIGIC_DELAY_TIME_LIST[i] = ReadInteger("MagicInterval", TableDef.MAIGIC_NAME_LIST[i], TableDef.MAIGIC_DELAY_TIME_LIST[i]);
                }
            }
        }
    }

    public class GameGateInfo
    {
        public string sServerAdress;
        public int nServerPort;
        public int nGatePort;
    }
}