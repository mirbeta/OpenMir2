using SystemModule.Common;

namespace SelGate.Conf
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
            GateConfig.ShowLogLevel = ReadInteger("SelGate", "ShowLogLevel", GateConfig.ShowLogLevel);
            GateConfig.ShowDebugLog = ReadBool("SelGate", "ShowDebugLog", GateConfig.ShowDebugLog);
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

            //Boolean
            GateConfig.m_fCheckNewIDOfIP = ReadBool("Switch", "CheckNewIDOfIP", GateConfig.m_fCheckNewIDOfIP);
            GateConfig.m_fCheckNullSession = ReadBool("Switch", "CheckNullSession", GateConfig.m_fCheckNullSession);
            GateConfig.m_fOverSpeedSendBack = ReadBool("Switch", "OverSpeedSendBack", GateConfig.m_fOverSpeedSendBack);
            GateConfig.m_fDefenceCCPacket = ReadBool("Switch", "DefenceCCPacket", GateConfig.m_fDefenceCCPacket);
            GateConfig.m_fKickOverSpeed = ReadBool("Switch", "KickOverSpeed", GateConfig.m_fKickOverSpeed);
            GateConfig.m_fKickOverPacketSize = ReadBool("Switch", "KickOverPacketSize", GateConfig.m_fKickOverPacketSize);
            //???????????? 2018-09-06
            GateConfig.m_fAllowGetBackChr = ReadBool("Switch", "AllowGetBackChr", GateConfig.m_fAllowGetBackChr);
            GateConfig.m_fAllowDeleteChr = ReadBool("Switch", "AllowDeleteChr", GateConfig.m_fAllowDeleteChr);
            GateConfig.m_fNewChrNameFilter = ReadBool("Switch", "NewChrNameFilter", GateConfig.m_fNewChrNameFilter);
            GateConfig.m_fDenyNullChar = ReadBool("Switch", "DenyNullChar", GateConfig.m_fDenyNullChar);
            GateConfig.m_fDenyAnsiChar = ReadBool("Switch", "DenyAnsiChar", GateConfig.m_fDenyAnsiChar);
            GateConfig.m_fDenySpecChar = ReadBool("Switch", "DenySpecChar", GateConfig.m_fDenySpecChar);
            GateConfig.m_fDenyHellenicChars = ReadBool("Switch", "DenyHellenicChars", GateConfig.m_fDenyHellenicChars);
            GateConfig.m_fDenyRussiaChar = ReadBool("Switch", "DenyRussiaChar", GateConfig.m_fDenyRussiaChar);
            GateConfig.m_fDenySpecNO1 = ReadBool("Switch", "DenySpecNO1", GateConfig.m_fDenySpecNO1);
            GateConfig.m_fDenySpecNO2 = ReadBool("Switch", "DenySpecNO2", GateConfig.m_fDenySpecNO2);
            GateConfig.m_fDenySpecNO3 = ReadBool("Switch", "DenySpecNO3", GateConfig.m_fDenySpecNO3);
            GateConfig.m_fDenySpecNO4 = ReadBool("Switch", "DenySpecNO4", GateConfig.m_fDenySpecNO4);
            GateConfig.m_fDenySBCChar = ReadBool("Switch", "DenySBCChar", GateConfig.m_fDenySBCChar);
            GateConfig.m_fDenykanjiChar = ReadBool("Switch", "DenykanjiChar", GateConfig.m_fDenykanjiChar);
            GateConfig.m_fDenyTabsChar = ReadBool("Switch", "DenyTabsChar", GateConfig.m_fDenyTabsChar);
            //GateConfig.m_tBlockIPMethod = TBlockIPMethod(ReadInteger("Method", "BlockIPMethod", Integer(GateConfig.m_tBlockIPMethod)));

            GateConfig.m_nGateCount = ReadInteger("SelGate", "Count", GateConfig.m_nGateCount);
            for (int i = 0; i < GateConfig.m_nGateCount; i++)
            {
                m_xGameGateList[i].sServerAdress = ReadString("SelGate", "ServerAddr" + i, m_xGameGateList[i].sServerAdress);
                m_xGameGateList[i].nServerPort = ReadInteger("SelGate", "ServerPort" + i, m_xGameGateList[i].nServerPort);
                m_xGameGateList[i].nGatePort = ReadInteger("SelGate", "GatePort" + i, m_xGameGateList[i].nGatePort);
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