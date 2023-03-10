using GameSrv.Player;
using GameSrv.Services;
using GameSrv.World;
using SystemModule.Data;

namespace GameSrv.GameGate {
    public class GateUserInfo {
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
        public WorldServer UserEngine;
        public int dwNewUserTick;
    }
}