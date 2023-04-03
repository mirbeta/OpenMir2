using SystemModule.Common;

namespace SelGate.Conf
{
    public class ConfigManager : ConfigFile
    {
        public GateConfig GateConfig;
        public TGameGateList[] m_xGameGateList;

        public ConfigManager(string szFileName) : base(szFileName)
        {
            Load();
            GateConfig = new GateConfig();
            m_xGameGateList = new TGameGateList[32];
            for (var i = 0; i < m_xGameGateList.Length; i++)
            {
                m_xGameGateList[i].sServerAdress = "127.0.0.1";
                m_xGameGateList[i].nServerPort = 5100;
                m_xGameGateList[i].nGatePort = 7100 + i;
            }
        }

        public void ReLoadConfig()
        {
            ReLoad();
            LoadConfig();
        }

        public void LoadConfig()
        {
            GateConfig.ShowLogLevel = ReadWriteInteger("SelGate", "ShowLogLevel", GateConfig.ShowLogLevel);
            GateConfig.ShowDebug = ReadWriteBool("SelGate", "ShowDebug", GateConfig.ShowDebug);
            GateConfig.m_nClientTimeOutTime = ReadWriteInteger("Integer", "ClientTimeOutTime", GateConfig.m_nClientTimeOutTime);
            if (GateConfig.m_nClientTimeOutTime < 10 * 1000)
            {
                GateConfig.m_nClientTimeOutTime = 10 * 1000;
                WriteInteger("Integer", "ClientTimeOutTime", GateConfig.m_nClientTimeOutTime);
            }
            GateConfig.m_nMaxConnectOfIP = ReadWriteInteger("Integer", "MaxConnectOfIP", GateConfig.m_nMaxConnectOfIP);
            GateConfig.m_nCheckNewIDOfIP = ReadWriteInteger("Integer", "CheckNewIDOfIP", GateConfig.m_nCheckNewIDOfIP);
            GateConfig.m_nClientTimeOutTime = ReadWriteInteger("Integer", "ClientTimeOutTime", GateConfig.m_nClientTimeOutTime);
            GateConfig.m_nNomClientPacketSize = ReadWriteInteger("Integer", "NomClientPacketSize", GateConfig.m_nNomClientPacketSize);
            GateConfig.m_nMaxClientPacketCount = ReadWriteInteger("Integer", "MaxClientPacketCount", GateConfig.m_nMaxClientPacketCount);

            //Boolean
            GateConfig.m_fCheckNewIDOfIP = ReadWriteBool("Switch", "CheckNewIDOfIP", GateConfig.m_fCheckNewIDOfIP);
            GateConfig.m_fCheckNullSession = ReadWriteBool("Switch", "CheckNullSession", GateConfig.m_fCheckNullSession);
            GateConfig.m_fOverSpeedSendBack = ReadWriteBool("Switch", "OverSpeedSendBack", GateConfig.m_fOverSpeedSendBack);
            GateConfig.m_fDefenceCCPacket = ReadWriteBool("Switch", "DefenceCCPacket", GateConfig.m_fDefenceCCPacket);
            GateConfig.m_fKickOverSpeed = ReadWriteBool("Switch", "KickOverSpeed", GateConfig.m_fKickOverSpeed);
            GateConfig.m_fKickOverPacketSize = ReadWriteBool("Switch", "KickOverPacketSize", GateConfig.m_fKickOverPacketSize);
            //???????????? 2018-09-06
            GateConfig.m_fAllowGetBackChr = ReadWriteBool("Switch", "AllowGetBackChr", GateConfig.m_fAllowGetBackChr);
            GateConfig.m_fAllowDeleteChr = ReadWriteBool("Switch", "AllowDeleteChr", GateConfig.m_fAllowDeleteChr);
            GateConfig.m_fNewChrNameFilter = ReadWriteBool("Switch", "NewChrNameFilter", GateConfig.m_fNewChrNameFilter);
            GateConfig.m_fDenyNullChar = ReadWriteBool("Switch", "DenyNullChar", GateConfig.m_fDenyNullChar);
            GateConfig.m_fDenyAnsiChar = ReadWriteBool("Switch", "DenyAnsiChar", GateConfig.m_fDenyAnsiChar);
            GateConfig.m_fDenySpecChar = ReadWriteBool("Switch", "DenySpecChar", GateConfig.m_fDenySpecChar);
            GateConfig.m_fDenyHellenicChars = ReadWriteBool("Switch", "DenyHellenicChars", GateConfig.m_fDenyHellenicChars);
            GateConfig.m_fDenyRussiaChar = ReadWriteBool("Switch", "DenyRussiaChar", GateConfig.m_fDenyRussiaChar);
            GateConfig.m_fDenySpecNO1 = ReadWriteBool("Switch", "DenySpecNO1", GateConfig.m_fDenySpecNO1);
            GateConfig.m_fDenySpecNO2 = ReadWriteBool("Switch", "DenySpecNO2", GateConfig.m_fDenySpecNO2);
            GateConfig.m_fDenySpecNO3 = ReadWriteBool("Switch", "DenySpecNO3", GateConfig.m_fDenySpecNO3);
            GateConfig.m_fDenySpecNO4 = ReadWriteBool("Switch", "DenySpecNO4", GateConfig.m_fDenySpecNO4);
            GateConfig.m_fDenySBCChar = ReadWriteBool("Switch", "DenySBCChar", GateConfig.m_fDenySBCChar);
            GateConfig.m_fDenykanjiChar = ReadWriteBool("Switch", "DenykanjiChar", GateConfig.m_fDenykanjiChar);
            GateConfig.m_fDenyTabsChar = ReadWriteBool("Switch", "DenyTabsChar", GateConfig.m_fDenyTabsChar);
            //GateConfig.m_tBlockIPMethod = TBlockIPMethod(ReadWriteInteger("Method", "BlockIPMethod", Integer(GateConfig.m_tBlockIPMethod)));

            GateConfig.m_nGateCount = ReadWriteInteger("SelGate", "Count", GateConfig.m_nGateCount);
            for (int i = 0; i < GateConfig.m_nGateCount; i++)
            {
                m_xGameGateList[i].sServerAdress = ReadWriteString("SelGate", "ServerAddr" + i, m_xGameGateList[i].sServerAdress);
                m_xGameGateList[i].nServerPort = ReadWriteInteger("SelGate", "ServerPort" + i, m_xGameGateList[i].nServerPort);
                m_xGameGateList[i].nGatePort = ReadWriteInteger("SelGate", "GatePort" + i, m_xGameGateList[i].nGatePort);
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