using System;
using System.IO;
using SystemModule.Common;

namespace LoginGate.Conf
{
    public class ConfigManager : IniFile
    {
        public GateConfig GateConfig;
        public GameGateInfo[] GameGateList;
        private static string sConfitFile = Path.Combine(AppContext.BaseDirectory, "config.conf");

        private static readonly ConfigManager instance = new ConfigManager(sConfitFile);

        public static ConfigManager Instance
        {
            get { return instance; }
        }

        public ConfigManager(string szFileName) : base(szFileName)
        {
            Load();
            GateConfig = new GateConfig();
            GameGateList = new GameGateInfo[32];
            for (var i = GameGateList.GetLowerBound(0); i <= GameGateList.GetUpperBound(0); i++)
            {
                GameGateList[i].sServerAdress = "127.0.0.1";
                GameGateList[i].nServerPort = 5500;
                GameGateList[i].nGatePort = 7000 + i;
            }
        }

        public void LoadConfig()
        {
            GateConfig.m_nClientTimeOutTime = ReadInteger("Integer", "ClientTimeOutTime", GateConfig.m_nClientTimeOutTime);
            if (GateConfig.m_nClientTimeOutTime < 10 * 1000)
            {
                GateConfig.m_nClientTimeOutTime = 10 * 1000;
                WriteInteger("Integer", "ClientTimeOutTime", GateConfig.m_nClientTimeOutTime);
            }
            GateConfig.m_nMaxConnectOfIP = ReadInteger("Integer", "MaxConnectOfIP", GateConfig.m_nMaxConnectOfIP);
            GateConfig.m_nCheckNewIDOfIP = ReadInteger("Integer", "CheckNewIDOfIP", GateConfig.m_nCheckNewIDOfIP);
            GateConfig.m_nClientTimeOutTime = ReadInteger("Integer", "ClientTimeOutTime", GateConfig.m_nClientTimeOutTime);
            GateConfig.m_nNomClientPacketSize = ReadInteger("Integer", "NomClientPacketSize", GateConfig.m_nNomClientPacketSize);
            GateConfig.m_nMaxClientPacketCount = ReadInteger("Integer", "MaxClientPacketCount", GateConfig.m_nMaxClientPacketCount);
            GateConfig.m_fCheckNewIDOfIP = ReadBool("Switch", "CheckNewIDOfIP", GateConfig.m_fCheckNewIDOfIP);
            GateConfig.m_fCheckNullSession = ReadBool("Switch", "CheckNullSession", GateConfig.m_fCheckNullSession);
            GateConfig.m_fOverSpeedSendBack = ReadBool("Switch", "OverSpeedSendBack", GateConfig.m_fOverSpeedSendBack);
            GateConfig.m_fDefenceCCPacket = ReadBool("Switch", "DefenceCCPacket", GateConfig.m_fDefenceCCPacket);
            GateConfig.m_fKickOverSpeed = ReadBool("Switch", "KickOverSpeed", GateConfig.m_fKickOverSpeed);
            GateConfig.m_fKickOverPacketSize = ReadBool("Switch", "KickOverPacketSize", GateConfig.m_fKickOverPacketSize);
            GateConfig.m_tBlockIPMethod = Enum.Parse<TBlockIPMethod>(ReadString("Method", "BlockIPMethod", GateConfig.m_tBlockIPMethod.ToString()));
            GateConfig.m_nGateCount = ReadInteger("LoginGate", "Count", GateConfig.m_nGateCount);
            GateConfig.m_nShowLogLevel = ReadInteger("LoginGate", "ShowLogLevel", GateConfig.m_nShowLogLevel);
            GateConfig.ShowDebugLog = ReadBool("LoginGate", "ShowDebugLog", GateConfig.ShowDebugLog);
            for (int i = 0; i < GateConfig.m_nGateCount; i++)
            {
                GameGateList[i].sServerAdress = ReadString("GameGate", "ServerAddr" + i, GameGateList[i].sServerAdress);
                GameGateList[i].nServerPort = ReadInteger("GameGate", "ServerPort" + i, GameGateList[i].nServerPort);
                GameGateList[i].nGatePort = ReadInteger("GameGate", "GatePort" + i, GameGateList[i].nGatePort);
            }
        }
    }

    public struct GameGateInfo
    {
        public string sServerAdress;
        public int nServerPort;
        public int nGatePort;
    }
}