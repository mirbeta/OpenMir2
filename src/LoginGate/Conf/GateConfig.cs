
namespace LoginGate.Conf
{
    /// <summary>
    /// 网关配置类
    /// </summary>
    public class GateConfig
    {
        public string m_szTitle;
        public int m_nShowLogLevel;
        public int m_nGateCount;
        public TGameGateList[] m_xGameGateList;
        public bool m_fCheckNewIDOfIP;
        public bool m_fCheckNullSession;
        public bool m_fOverSpeedSendBack;
        public bool m_fDefenceCCPacket;
        public bool m_fKickOverSpeed;
        public bool m_fKickOverPacketSize;
        public int m_nCheckNewIDOfIP;
        public int m_nMaxConnectOfIP;
        public int m_nClientTimeOutTime;
        public int m_nNomClientPacketSize;
        public int m_nMaxClientPacketCount;
        public TBlockIPMethod m_tBlockIPMethod;

        public GateConfig()
        {
            m_nGateCount = 1;
            m_xGameGateList = new TGameGateList[32];
            for (int i = 0; i < m_xGameGateList.Length; i++)
            {
                m_xGameGateList[i].sServerAdress = "127.0.0.1";
                m_xGameGateList[i].nServerPort = 5500;
                m_xGameGateList[i].nGatePort = 7000 + i;
            }

            m_fCheckNewIDOfIP = true;
            m_fCheckNullSession = true;
            m_fOverSpeedSendBack = false;
            m_fDefenceCCPacket = false;
            m_fKickOverSpeed = false;
            m_fKickOverPacketSize = true;

            m_nNomClientPacketSize = 400;
            m_nMaxConnectOfIP = 20;
            m_nCheckNewIDOfIP = 5;
            m_nClientTimeOutTime = 60 * 1000;
            m_nMaxClientPacketCount = 2;
        }
    }

    public enum TBlockIPMethod : byte
    {
        mDisconnect,
        mBlock,
        mBlockList
    }
}