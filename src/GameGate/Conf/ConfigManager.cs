using System;
using SystemModule.Common;

namespace GameGate
{
    public partial class ConfigManager : IniFile
    {
        public GateConfig GateConfig;
        public TGameGateList[] m_xGameGateList;

        public ConfigManager(string fileName) : base(fileName)
        {
            Load();
            GateConfig = new GateConfig();
            m_xGameGateList = new TGameGateList[32];
        }

        public void LoadConfig()
        {
            GateShare.nShowLogLevel = ReadInteger(GateShare.GateClass, "ShowLogLevel", GateShare.nShowLogLevel);
            GateShare.AddMainLogMsg("正在加载配置信息...", 3);
            GateConfig.m_szCMDSpaceMove = ReadString("Strings", "CMDSpaceMove", GateConfig.m_szCMDSpaceMove);
            GateConfig.m_szOverClientCntMsg = ReadString("Strings", "OverClientCntMsg", GateConfig.m_szOverClientCntMsg);
            GateConfig.m_szHWIDBlockedMsg = ReadString("Strings", "HWIDBlockedMsg", GateConfig.m_szHWIDBlockedMsg);
            GateConfig.m_szChatFilterReplace = ReadString("Strings", "ChatFilterReplace", GateConfig.m_szChatFilterReplace);
            GateConfig.m_szOverSpeedSendBack = ReadString("Strings", "OverSpeedSendBack", GateConfig.m_szOverSpeedSendBack);
            GateConfig.m_szPacketDecryptFailed = ReadString("Strings", "PacketDecryptFailed", GateConfig.m_szPacketDecryptFailed);
            GateConfig.m_szBlockHWIDFileName = ReadString("Strings", "BlockHWIDFileName", GateConfig.m_szBlockHWIDFileName);
            GateConfig.m_fAddLog = ReadBool("Switch", "AddLog", GateConfig.m_fAddLog);
            GateConfig.m_nShowLogLevel = ReadInteger("Integer", "ShowLogLevel", GateConfig.m_nShowLogLevel);
            GateConfig.m_nPunishMoveInterval = ReadInteger("Integer", "PunishMoveInterval", GateConfig.m_nPunishMoveInterval);
            GateConfig.m_nPunishSpellInterval = ReadInteger("Integer", "PunishSpellInterval", GateConfig.m_nPunishSpellInterval);
            GateConfig.m_nPunishAttackInterval = ReadInteger("Integer", "PunishAttackInterval", GateConfig.m_nPunishAttackInterval);
            GateConfig.m_nMaxItemSpeed = ReadInteger("Integer", "MaxItemSpeed", GateConfig.m_nMaxItemSpeed);
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
            GateConfig.m_nChatInterval = ReadInteger("Integer", "ChatInterval", GateConfig.m_nChatInterval);
            GateConfig.m_nTurnInterval = ReadInteger("Integer", "TurnInterval", GateConfig.m_nTurnInterval);
            GateConfig.m_nMoveInterval = ReadInteger("Integer", "MoveInterval", GateConfig.m_nMoveInterval);
            GateConfig.m_nSpellNextInterval = ReadInteger("Integer", "SpellNextInterval", GateConfig.m_nSpellNextInterval);
            GateConfig.m_nAttackInterval = ReadInteger("Integer", "AttackInterval", GateConfig.m_nAttackInterval);
            GateConfig.m_nButchInterval = ReadInteger("Integer", "ButchInterval", GateConfig.m_nButchInterval);
            GateConfig.m_nSitDownInterval = ReadInteger("Integer", "SitDownInterval", GateConfig.m_nSitDownInterval);
            GateConfig.m_nPickupInterval = ReadInteger("Integer", "PickupInterval", GateConfig.m_nPickupInterval);
            GateConfig.m_nEatInterval = ReadInteger("Integer", "EatInterval", GateConfig.m_nEatInterval);
            GateConfig.m_nMoveNextSpellCompensate = ReadInteger("Integer", "MoveNextSpellCompensate", GateConfig.m_nMoveNextSpellCompensate);
            GateConfig.m_nMoveNextAttackCompensate = ReadInteger("Integer", "MoveNextAttackCompensate", GateConfig.m_nMoveNextAttackCompensate);
            GateConfig.m_nAttackNextMoveCompensate = ReadInteger("Integer", "AttackNextMoveCompensate", GateConfig.m_nAttackNextMoveCompensate);
            GateConfig.m_nAttackNextSpellCompensate = ReadInteger("Integer", "AttackNextSpellCompensate", GateConfig.m_nAttackNextSpellCompensate);
            GateConfig.m_nSpellNextMoveCompensate = ReadInteger("Integer", "SpellNextMoveCompensate", GateConfig.m_nSpellNextMoveCompensate);
            GateConfig.m_nSpellNextAttackCompensate = ReadInteger("Integer", "SpellNextAttackCompensate", GateConfig.m_nSpellNextAttackCompensate);
            GateConfig.m_nSpaceMoveNextPickupInterval = ReadInteger("Integer", "SpaceMoveNextPickupInterval", GateConfig.m_nSpaceMoveNextPickupInterval);
            GateConfig.m_nPunishBaseInterval = ReadInteger("Integer", "PunishBaseInterval", GateConfig.m_nPunishBaseInterval);
            GateConfig.m_nClientMoveSpeedRate = ReadInteger("Integer", "ClientMoveSpeedRate", GateConfig.m_nClientMoveSpeedRate);
            GateConfig.m_nClientSpellSpeedRate = ReadInteger("Integer", "ClientSpellSpeedRate", GateConfig.m_nClientSpellSpeedRate);
            GateConfig.m_nClientAttackSpeedRate = ReadInteger("Integer", "ClientAttackSpeedRate", GateConfig.m_nClientAttackSpeedRate);
            // Method
            GateConfig.m_tOverSpeedPunishMethod = (TPunishMethod)ReadInteger("Method", "OverSpeedPunishMethod", (int)GateConfig.m_tOverSpeedPunishMethod);
            GateConfig.m_tBlockIPMethod = (TBlockIPMethod)ReadInteger("Method", "BlockIPMethod", (int)GateConfig.m_tBlockIPMethod);
            GateConfig.m_tChatFilterMethod = (TChatFilterMethod)ReadInteger("Method", "ChatFilterMethod", (int)GateConfig.m_tChatFilterMethod);
            GateConfig.m_tSpeedHackWarnMethod = (TOverSpeedMsgMethod)ReadInteger("Method", "SpeedHackWarnMethod", (int)GateConfig.m_tSpeedHackWarnMethod);
            // Boolean
            GateConfig.m_fCheckNullSession = ReadBool("Switch", "CheckNullSession", GateConfig.m_fCheckNullSession);
            GateConfig.m_fOverSpeedSendBack = ReadBool("Switch", "OverSpeedSendBack", GateConfig.m_fOverSpeedSendBack);
            GateConfig.m_fDefenceCCPacket = ReadBool("Switch", "DefenceCCPacket", GateConfig.m_fDefenceCCPacket);
            GateConfig.m_fKickOverSpeed = ReadBool("Switch", "KickOverSpeed", GateConfig.m_fKickOverSpeed);
            GateConfig.m_fDoMotaeboSpeedCheck = ReadBool("Switch", "DoMotaeboSpeedCheck", GateConfig.m_fDoMotaeboSpeedCheck);
            GateConfig.m_fDenyPresend = ReadBool("Switch", "DenyPresend", GateConfig.m_fDenyPresend);
            GateConfig.m_fItemSpeedCompensate = ReadBool("Switch", "ItemSpeedCompensate", GateConfig.m_fItemSpeedCompensate);
            GateConfig.m_fKickOverPacketSize = ReadBool("Switch", "KickOverPacketSize", GateConfig.m_fKickOverPacketSize);
            GateConfig.m_fChatFilter = ReadBool("Switch", "ChatFilter", GateConfig.m_fChatFilter);
            GateConfig.m_fChatInterval = ReadBool("Switch", "ChatInterval", GateConfig.m_fChatInterval);
            GateConfig.m_fChatCmdFilter = ReadBool("Switch", "ChatCmdFilter", GateConfig.m_fChatCmdFilter);
            GateConfig.m_fTurnInterval = ReadBool("Switch", "TurnInterval", GateConfig.m_fTurnInterval);
            GateConfig.m_fMoveInterval = ReadBool("Switch", "MoveInterval", GateConfig.m_fMoveInterval);
            GateConfig.m_fSpellInterval = ReadBool("Switch", "SpellInterval", GateConfig.m_fSpellInterval);
            GateConfig.m_fAttackInterval = ReadBool("Switch", "AttackInterval", GateConfig.m_fAttackInterval);
            GateConfig.m_fButchInterval = ReadBool("Switch", "ButchInterval", GateConfig.m_fButchInterval);
            GateConfig.m_fSitDownInterval = ReadBool("Switch", "SitDownInterval", GateConfig.m_fSitDownInterval);
            GateConfig.m_fSpaceMoveNextPickupInterval = ReadBool("Switch", "SpaceMoveNextPickupInterval", GateConfig.m_fSpaceMoveNextPickupInterval);
            GateConfig.m_fPickupInterval = ReadBool("Switch", "PickupInterval", GateConfig.m_fPickupInterval);
            GateConfig.m_fEatInterval = ReadBool("Switch", "EatInterval", GateConfig.m_fEatInterval);
            GateConfig.m_fProcClientHWID = ReadBool("Switch", "ProcClientCount", GateConfig.m_fProcClientHWID);
            GateConfig.m_fClientShowHintNewType = ReadBool("Switch", "ClientShowHintNewType", GateConfig.m_fClientShowHintNewType);
            GateConfig.m_fOpenClientSpeedRate = ReadBool("Switch", "OpenClientSpeedRate", GateConfig.m_fOpenClientSpeedRate);
            GateConfig.m_fSyncClientSpeed = ReadBool("Switch", "SyncClientSpeed", GateConfig.m_fSyncClientSpeed);
            GateConfig.m_nPunishIntervalRate = ReadFloat("Float", "PunishIntervalRate", GateConfig.m_nPunishIntervalRate);
            GateConfig.m_nGateCount = ReadInteger("GameGate", "Count", GateConfig.m_nGateCount);
            for (var i = 0; i <= GateConfig.m_nGateCount; i++)
            {
                m_xGameGateList[i] = new TGameGateList();
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
            GateShare.AddMainLogMsg("配置信息加载完成...", 3);
            GateShare.LoadAbuseFile();
            GateShare.LoadBlockIPFile();
        }
    }
}