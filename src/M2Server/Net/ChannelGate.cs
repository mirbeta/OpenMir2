using System.Net.Sockets;
using SystemModule;
using SystemModule.Data;

namespace M2Server.Net
{
    public class ChannelGate
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
        public int UserCount;
        public bool SendKeepAlive;
        public int SendChecked;
        public int SendBlockCount;
        /// <summary>
        /// 列队数据
        /// </summary>
        public int SendMsgCount;
        /// <summary>
        /// 未处理数据
        /// </summary>
        public int SendRemainCount;
        /// <summary>
        /// 发送间隔
        /// </summary>
        public int SendTick;
        /// <summary>
        /// 发送消息字节
        /// </summary>
        public int SendMsgBytes;
        /// <summary>
        /// 发送的字节数量
        /// </summary>
        public int SendBytesCount;
        /// <summary>
        /// 发送数据
        /// </summary>
        public int SendedMsgCount;
        /// <summary>
        /// 发送数量
        /// </summary>
        public int SendCount;
        /// <summary>
        /// 上次心跳时间
        /// </summary>
        public int SendCheckTick;
    }

    public class SessionUser
    {
        /// <summary>
        /// 人物对象
        /// </summary>
        public IPlayerActor PlayObject;
        public int SessionID;
        /// <summary>
        /// 账号
        /// </summary>
        public string Account;
        public ushort SocketId;
        /// <summary>
        /// 玩家IP
        /// </summary>
        public string IPaddr;
        /// <summary>
        /// 认证是否通过
        /// </summary>
        public bool Certification;
        /// <summary>
        /// 玩家名称
        /// </summary>
        public string ChrName;
        /// <summary>
        /// 客户端版本号
        /// </summary>
        public int ClientVersion;
        /// <summary>
        /// 当前会话信息
        /// </summary>
        public AccountSession SessInfo;
        public int Socket;
        public IFrontEngine FrontEngine;
        public IWorldEngine WorldEngine;
        public int NewUserTick;
    }
}