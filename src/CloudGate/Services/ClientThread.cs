using CloudGate.Conf;
using System.Net;
using SystemModule;
using SystemModule.Packet.ClientPackets;

namespace CloudGate.Services
{
    /// <summary>
    /// 网关客户端(GameGate-GameSvr)
    /// </summary>
    public class ClientThread
    {
        /// <summary>
        /// 网关ID
        /// </summary>
        public readonly string ClientId;
        /// <summary>
        /// 用户会话
        /// </summary>
        public readonly TSessionInfo[] SessionArray = new TSessionInfo[GateShare.MaxSession];
        /// <summary>
        ///  网关游戏服务器之间检测是否失败（超时）
        /// </summary>
        public bool CheckServerFail = false;
        /// <summary>
        /// 网关游戏服务器之间检测是否失败次数
        /// </summary>
        public int CheckServerFailCount = 0;
        /// <summary>
        /// 独立Buffer分区
        /// </summary>
        private byte[] _receiveBuffer = null;
        /// <summary>
        /// 上次剩下多少字节未处理
        /// </summary>
        private int _buffLen = 0;
        /// <summary>
        /// 网关是否就绪
        /// </summary>
        public bool GateReady = false;
        /// <summary>
        /// 是否链接成功
        /// </summary>
        private bool _connected = false;
        /// <summary>
        /// 历史最高会话数
        /// </summary>
        private int _counter = 0;
        /// <summary>
        /// 发送总字节数
        /// </summary>
        public int SendBytes;
        /// <summary>
        /// 接收总字节数
        /// </summary>
        public int ReceiveBytes;
        private int _checkRecviceTick = 0;
        private int _checkServerTick = 0;
        private int _checkServerTimeMin = 0;
        private int _checkServerTimeMax = 0;
        /// <summary>
        /// Session管理
        /// </summary>
        private static SessionManager SessionManager => SessionManager.Instance;
        /// <summary>
        /// 日志
        /// </summary>
        private static MirLog LogQueue => MirLog.Instance;

        public ClientThread(string clientId, IPEndPoint endPoint, GameGateInfo gameGate)
        {
            ClientId = clientId;
            ReceiveBytes = 0;
            SendBytes = 0;
        }

        public string GetSessionCount()
        {
            var count = 0;
            for (var i = 0; i < SessionArray.Length; i++)
            {
                if (SessionArray[i] != null && SessionArray[i].Socket != null)
                {
                    count++;
                }
            }
            if (count > _counter)
            {
                _counter = count;
            }
            return count + "/" + _counter;
        }

        public TSessionInfo[] GetSession()
        {
            return SessionArray;
        }

        public void RestSessionArray()
        {
            for (var i = 0; i < GateShare.MaxSession; i++)
            {
                if (SessionArray[i] != null)
                {
                    SessionArray[i].Socket = null;
                    SessionArray[i].nUserListIndex = 0;
                    SessionArray[i].dwReceiveTick = HUtil32.GetTickCount();
                    SessionArray[i].SckHandle = 0;
                    SessionArray[i].SessionId = 0;
                }
            }
        }

        public void SendServerMsg(ushort nIdent, ushort wSocketIndex, int nSocket, ushort nUserListIndex, int nLen,
            string Data)
        {
            if (!string.IsNullOrEmpty(Data))
            {
                var strBuff = HUtil32.GetBytes(Data);
                SendServerMsg(nIdent, wSocketIndex, nSocket, nUserListIndex, nLen, strBuff);
            }
            else
            {
                SendServerMsg(nIdent, wSocketIndex, nSocket, nUserListIndex, nLen, (byte[])null);
            }
        }

        /// <summary>
        /// 玩家进入游戏
        /// </summary>
        public void UserEnter(ushort socketIndex, int nSocket, string Data)
        {
            SendServerMsg(Grobal2.GM_OPEN, socketIndex, nSocket, 0, Data.Length, Data);
        }

        /// <summary>
        /// 玩家退出游戏
        /// </summary>
        public void UserLeave(int scoket)
        {
            SendServerMsg(Grobal2.GM_CLOSE, 0, scoket, 0, 0, "");
        }

        private void SendServerMsg(ushort nIdent, ushort wSocketIndex, int nSocket, ushort nUserListIndex, int nLen,
            byte[] Data)
        {
            var GateMsg = new GameServerPacket();
            GateMsg.PacketCode = Grobal2.RUNGATECODE;
            GateMsg.Socket = nSocket;
            GateMsg.SessionId = wSocketIndex;
            GateMsg.Ident = nIdent;
            GateMsg.ServerIndex = nUserListIndex;
            GateMsg.PackLength = nLen;
        }
    }
}