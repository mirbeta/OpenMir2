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
        private readonly GameGateInfo[] GameGateList;
        public bool m_fCheckNewIDOfIP;
        /// <summary>
        /// 检查空的会话数据
        /// </summary>
        public bool CheckNullSession;
        public bool OverSpeedSendBack;
        /// <summary>
        /// 是否启用CC封包检测
        /// </summary>
        public bool DefenceCCPacket;
        public bool m_fKickOverSpeed;
        public bool m_fKickOverPacketSize;
        public int m_nCheckNewIDOfIP;
        public int m_nMaxConnectOfIP;
        /// <summary>
        /// 会话超时时间
        /// </summary>
        public int ClientTimeOutTime;
        public int NomClientPacketSize;
        public int MaxClientPacketCount;
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
            CheckNullSession = true;
            OverSpeedSendBack = false;
            DefenceCCPacket = false;
            m_fKickOverSpeed = false;
            m_fKickOverPacketSize = true;
            NomClientPacketSize = 400;
            m_nMaxConnectOfIP = 20;
            m_nCheckNewIDOfIP = 5;
            ClientTimeOutTime = 60 * 1000;
            MaxClientPacketCount = 2;
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