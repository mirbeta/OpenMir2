using System.Net.Sockets;

namespace RunGate
{
    public class TSessionInfo
    {
        public Socket Socket;
        public string sSocData;
        public string sSendData;
        public int nUserListIndex;
        public int nPacketIdx;
        /// <summary>
        /// 数据包序号重复计数（客户端用封包发送数据检测）
        /// </summary>
        public int nPacketErrCount;
        public bool boStartLogon;
        public bool boSendLock;
        public int nCheckSendLength;
        public bool boSendAvailable;
        public bool boSendCheck;
        public long dwTimeOutTime;
        public long dwReceiveTick;
        public int nSckHandle;
        public string sRemoteAddr;
        /// <summary>
        /// 发言间隔控制
        /// </summary>
        public long dwSayMsgTick;
    }
}