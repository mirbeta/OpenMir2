using SystemModule.Actors;
using SystemModule.SubSystem;

namespace M2Server.Net
{
    public class GameGate
    {
        /// <summary>
        /// 网关是否连接
        /// </summary>
        public bool Connected;
        /// <summary>
        /// Socket链接Id
        /// </summary>
        public string ConnectionId;
        /// <summary>
        /// 玩家列表
        /// </summary>
        public IList<SessionUser> UserList;
        /// <summary>
        /// 在线人数
        /// </summary>
        public int UserCount;
        public bool SendKeepAlive;
        /// <summary>
        /// 是否检查发送心跳
        /// </summary>
        public bool CheckStatus;
        /// <summary>
        /// 发送间隔
        /// </summary>
        public int SendTick;
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
        public int SessionId;
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
        public int Socket;
        public IFrontEngine FrontEngine;
        public IWorldEngine WorldEngine;
    }
}