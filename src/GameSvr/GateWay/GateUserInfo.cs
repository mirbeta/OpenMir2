using GameSvr.Player;
using GameSvr.Services;
using GameSvr.World;
using SystemModule.Data;

namespace GameSvr.GateWay
{
    public class GateUserInfo
    {
        /// <summary>
        /// 人物对象
        /// </summary>
        public PlayObject PlayObject;
        public int nSessionID;
        /// <summary>
        /// 账号
        /// </summary>
        public string sAccount;
        public ushort SocketId;
        /// <summary>
        /// 玩家IP
        /// </summary>
        public string sIPaddr;
        /// <summary>
        /// 认证是否通过
        /// </summary>
        public bool boCertification;
        /// <summary>
        /// 玩家名称
        /// </summary>
        public string sChrName;
        /// <summary>
        /// 客户端版本号
        /// </summary>
        public int nClientVersion;
        /// <summary>
        /// 当前会话信息
        /// </summary>
        public TSessInfo SessInfo;
        public int nSocket;
        public TFrontEngine FrontEngine;
        public WorldServer UserEngine;
        public int dwNewUserTick;
    }
}