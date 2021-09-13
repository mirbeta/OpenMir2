using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Channels;

namespace GameSvr
{
    public class TGateInfo
    {
        /// <summary>
        /// 网关是否已启用
        /// </summary>
        public bool boUsed;
        public Socket Socket;
        public string SocketId;
        public int GateIndex;
        /// <summary>
        /// 网关IP
        /// </summary>
        public string sAddr;
        /// <summary>
        /// 端口
        /// </summary>
        public int nPort;
        /// <summary>
        /// 玩家列表
        /// </summary>
        public IList<TGateUserInfo> UserList;
        /// <summary>
        /// 在线人数
        /// </summary>
        public int nUserCount;
        public byte[] Buffer;
        public int nBuffLen;
        public Channel<byte[]> BufferChannel;
        public bool boSendKeepAlive;
        public int nSendChecked;
        public int nSendBlockCount;
        /// <summary>
        /// 列队数据
        /// </summary>
        public int nSendMsgCount;
        /// <summary>
        /// 剩余数据
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