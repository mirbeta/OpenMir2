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
            GateConfig.CheckNewIDOfIP = ReadWriteBool("Switch", "CheckNewIDOfIP", GateConfig.CheckNewIDOfIP);
            GateConfig.CheckNullSession = ReadWriteBool("Switch", "CheckNullSession", GateConfig.CheckNullSession);
            GateConfig.OverSpeedSendBack = ReadWriteBool("Switch", "OverSpeedSendBack", GateConfig.OverSpeedSendBack);
            GateConfig.DefenceCCPacket = ReadWriteBool("Switch", "DefenceCCPacket", GateConfig.DefenceCCPacket);
            GateConfig.KickOverSpeed = ReadWriteBool("Switch", "KickOverSpeed", GateConfig.KickOverSpeed);
            GateConfig.KickOverPacketSize = ReadWriteBool("Switch", "KickOverPacketSize", GateConfig.KickOverPacketSize);
            //???????????? 2018-09-06
            GateConfig.AllowGetBackChr = ReadWriteBool("Switch", "AllowGetBackChr", GateConfig.AllowGetBackChr);
            GateConfig.AllowDeleteChr = ReadWriteBool("Switch", "AllowDeleteChr", GateConfig.AllowDeleteChr);
            GateConfig.NewChrNameFilter = ReadWriteBool("Switch", "NewChrNameFilter", GateConfig.NewChrNameFilter);
            GateConfig.DenyNullChar = ReadWriteBool("Switch", "DenyNullChar", GateConfig.DenyNullChar);
            GateConfig.DenyAnsiChar = ReadWriteBool("Switch", "DenyAnsiChar", GateConfig.DenyAnsiChar);
            GateConfig.DenySpecChar = ReadWriteBool("Switch", "DenySpecChar", GateConfig.DenySpecChar);
            GateConfig.DenyHellenicChars = ReadWriteBool("Switch", "DenyHellenicChars", GateConfig.DenyHellenicChars);
            GateConfig.DenyRussiaChar = ReadWriteBool("Switch", "DenyRussiaChar", GateConfig.DenyRussiaChar);
            GateConfig.DenySpecNO1 = ReadWriteBool("Switch", "DenySpecNO1", GateConfig.DenySpecNO1);
            GateConfig.DenySpecNO2 = ReadWriteBool("Switch", "DenySpecNO2", GateConfig.DenySpecNO2);
            GateConfig.DenySpecNO3 = ReadWriteBool("Switch", "DenySpecNO3", GateConfig.DenySpecNO3);
            GateConfig.DenySpecNO4 = ReadWriteBool("Switch", "DenySpecNO4", GateConfig.DenySpecNO4);
            GateConfig.DenySBCChar = ReadWriteBool("Switch", "DenySBCChar", GateConfig.DenySBCChar);
            GateConfig.DenykanjiChar = ReadWriteBool("Switch", "DenykanjiChar", GateConfig.DenykanjiChar);
            GateConfig.DenyTabsChar = ReadWriteBool("Switch", "DenyTabsChar", GateConfig.DenyTabsChar);
            //GateConfig.m_tBlockIPMethod = TBlockIPMethod(ReadWriteInteger("Method", "BlockIPMethod", Integer(GateConfig.m_tBlockIPMethod)));

            GateConfig.GateCount = ReadWriteInteger("SelGate", "Count", GateConfig.GateCount);
            for (int i = 0; i < GateConfig.GateCount; i++)
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