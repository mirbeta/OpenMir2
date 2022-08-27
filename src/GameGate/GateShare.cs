using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SystemModule.Common;

namespace GameGate
{
    public class GateShare
    {
        public static bool ShowLog = true;
        /// <summary>
        /// 单线程最大用户数
        /// </summary>
        public const int MaxSession = 10000;
        /// <summary>
        ///  网关游戏服务器之间检测超时时间
        /// </summary>
        public const long CheckServerTimeOutTime = 3 * 60 * 1000;
        /// <summary>
        /// 会话超时时间
        /// </summary>
        public const long SessionTimeOutTime = 15 * 24 * 60 * 60 * 1000;
        /// <summary>
        /// 禁止连接IP列表
        /// </summary>
        public static StringList BlockIPList = null;
        /// <summary>
        /// 临时禁止连接IP列表
        /// </summary>
        public static IList<string> TempBlockIPList = null;
        /// <summary>
        /// 聊天过滤命令列表
        /// </summary>
        public static ConcurrentDictionary<string, byte> ChatCommandFilter;
        public static Dictionary<string, ClientSession> PunishList;
        public static HardwareFilter HWFilter;

        public static void Initialization()
        {
            BlockIPList = new StringList();
            TempBlockIPList = new List<string>();
            PunishList = new Dictionary<string, ClientSession>();
            ChatCommandFilter = new ConcurrentDictionary<string, byte>(StringComparer.OrdinalIgnoreCase);
        }
    }

}