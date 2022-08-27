using SystemModule.Common;

namespace LoginGate.Conf
{
    /// <summary>
    /// 网关配置类
    /// </summary>
    public class GateConfig : IniFile
    {
        public int ShowLogLevel;
        public bool ShowDebugLog;
        public int GateCount;
        private GameGateInfo[] GameGateList;
        public bool m_fCheckNewIDOfIP;
        public bool m_fCheckNullSession;
        public bool m_fOverSpeedSendBack;
        /// <summary>
        /// 是否启用CC封包检测
        /// </summary>
        public bool DefenceCCPacket;
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
            GateCount = 1;
            GameGateList = new GameGateInfo[32];
            for (var i = 0; i < GameGateList.Length; i++)
            {
                GameGateList[i].LoginAdress = "127.0.0.1";
                GameGateList[i].LoginPort = 5500;
                GameGateList[i].GatePort = 7000 + i;
            }
            ShowLogLevel = 5;
            m_fCheckNewIDOfIP = true;
            m_fCheckNullSession = true;
            m_fOverSpeedSendBack = false;
            DefenceCCPacket = false;
            m_fKickOverSpeed = false;
            m_fKickOverPacketSize = true;
            m_nNomClientPacketSize = 400;
            m_nMaxConnectOfIP = 20;
            m_nCheckNewIDOfIP = 5;
            m_nClientTimeOutTime = 60 * 1000;
            m_nMaxClientPacketCount = 2;
            ShowDebugLog = false;
        }
    }

    public enum TBlockIPMethod : byte
    {
        mDisconnect,
        mBlock,
        mBlockList
    }
}