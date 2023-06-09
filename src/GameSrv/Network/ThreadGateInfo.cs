using GameSrv.Services;
using GameSrv.Word;
using M2Server.Player;
using System.Net.Sockets;
using SystemModule.Data;

namespace GameSrv.Network
{
    public class ThreadGateInfo
    {
        /// <summary>
        /// 网关是否已启用
        /// </summary>
        public bool BoUsed;
        public Socket Socket;
        /// <summary>
        /// Socket链接ID
        /// </summary>
        public string SocketId;
        /// <summary>
        /// 玩家列表
        /// </summary>
        public IList<SessionUser> UserList;
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

    public class SessionUser
    {
        /// <summary>
        /// 人物对象
        /// </summary>
        public PlayObject PlayObject;
        public int SessionID;
        /// <summary>
        /// 账号
        /// </summary>
        public string Account;
        public ushort SocketId;
        /// <summary>
        /// 玩家IP
        /// </summary>
        public string sIPaddr;
        /// <summary>
        /// 认证是否通过
        /// </summary>
        public bool Certification;
        /// <summary>
        /// 玩家名称
        /// </summary>
        public string sChrName;
        /// <summary>
        /// 客户端版本号
        /// </summary>
        public int ClientVersion;
        /// <summary>
        /// 当前会话信息
        /// </summary>
        public PlayerSession SessInfo;
        public int nSocket;
        public FrontEngine FrontEngine;
        public WorldServer WorldEngine;
        public int dwNewUserTick;
    }
}