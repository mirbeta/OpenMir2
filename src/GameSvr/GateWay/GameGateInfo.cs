using System.Net.Sockets;

namespace GameSvr.GameGate
{
    public class GameGateInfo
    {
        /// <summary>
        /// 网关是否已启用
        /// </summary>
        public bool boUsed;
        public Socket Socket;
        public string SocketId;
        /// <summary>
        /// 玩家列表
        /// </summary>
        public IList<GameGateUserInfo> UserList;
        /// <summary>
        /// 在线人数
        /// </summary>
        public int nUserCount;
        public bool boSendKeepAlive;
        public int nSendChecked;
        public int nSendBlockCount;
        /// <summary>
        /// 列队数据
        /// </summary>
        public int nSendMsgCount;
        /// <summary>
        /// 未处理数据
        /// </summary>
        public int nSendRemainCount;
        /// <summary>
        /// 发送间隔
        /// </summary>
        public int dwSendTick;
        /// <summary>
        /// 发送消息字节
        /// </summary>
        public int nSendMsgBytes;
        /// <summary>
        /// 发送的字节数量
        /// </summary>
        public int nSendBytesCount;
        /// <summary>
        /// 发送数据
        /// </summary>
        public int nSendedMsgCount;
        /// <summary>
        /// 发送数量
        /// </summary>
        public int nSendCount;
        /// <summary>
        /// 上次心跳时间
        /// </summary>
        public int dwSendCheckTick;
    }
}