using System;
using SystemModule.Common;

namespace LoginGate.Conf
{
    public class ConfigManager : IniFile
    {
        public GateConfig GateConfig;
        public TGameGateList[] m_xGameGateList;

        public ConfigManager(string szFileName) : base(szFileName)
        {
            Load();
            GateConfig = new GateConfig();
            m_xGameGateList = new TGameGateList[32];
            for (var i = m_xGameGateList.GetLowerBound(0); i <= m_xGameGateList.GetUpperBound(0); i++)
            {
                m_xGameGateList[i].sServerAdress = "127.0.0.1";
                m_xGameGateList[i].nServerPort = 5500;
                m_xGameGateList[i].nGatePort = 7000 + i;
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
            GateConfig.m_fOverSpeedSendBack = ReadBool("Switch", "OverSpeedSendBack",GateConfig. m_fOverSpeedSendBack);
            GateConfig.m_fDefenceCCPacket = ReadBool("Switch", "DefenceCCPacket", GateConfig.m_fDefenceCCPacket);
            GateConfig.m_fKickOverSpeed = ReadBool("Switch", "KickOverSpeed", GateConfig.m_fKickOverSpeed);
            GateConfig.m_fKickOverPacketSize = ReadBool("Switch", "KickOverPacketSize", GateConfig.m_fKickOverPacketSize);
            GateConfig.m_tBlockIPMethod = Enum.Parse<TBlockIPMethod>(ReadString("Method", "BlockIPMethod", GateConfig.m_tBlockIPMethod.ToString()));
            GateConfig.m_nGateCount = ReadInteger("GameGate", "Count", GateConfig.m_nGateCount);
            GateConfig.m_nGateCount = ReadInteger("LoginGate", "Count", GateConfig.m_nGateCount);
            for (int i = 0; i < GateConfig.m_nGateCount; i++)
            {
                m_xGameGateList[i].sServerAdress = ReadString("GameGate", "ServerAddr" + i, m_xGameGateList[i].sServerAdress);
                m_xGameGateList[i].nServerPort = ReadInteger("GameGate", "ServerPort" + i, m_xGameGateList[i].nServerPort);
                m_xGameGateList[i].nGatePort = ReadInteger("GameGate", "GatePort" + i, m_xGameGateList[i].nGatePort);
            }
        }
    }
    
    public struct TGameGateList
    {
        public string sServerAdress;
        public int nServerPort;
        public int nGatePort;
    }
}