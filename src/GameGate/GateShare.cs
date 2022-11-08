using GameGate.Filters;
using GameGate.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using SystemModule.Common;

namespace GameGate
{
    public class GateShare
    {
        public static bool ShowLog = true;
        /// <summary>
        /// 单线程最大用户数
        /// </summary>
        public const int MaxSession = 5000;
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
        public static HardwareFilter HardwareFilter;

        public static void Initialization()
        {
            BlockIPList = new StringList();
            TempBlockIPList = new List<string>();
            PunishList = new Dictionary<string, ClientSession>();
            ChatCommandFilter = new ConcurrentDictionary<string, byte>(StringComparer.OrdinalIgnoreCase);
        }
    }

    public struct MessagePacket
    {
        public byte[] Buffer;
        public int BufferLen;
        public int SessionId;
    }

    public class SessionInfo
    {
        public Socket Socket;
        /// <summary>
        /// SocketId
        /// </summary>
        public int SckHandle;
        /// <summary>
        /// 会话ID
        /// </summary>
        public ushort SessionId;
        /// <summary>
        /// Soccket链接ID
        /// </summary>
        public string ConnectionId;
        /// <summary>
        /// 数据处理ThreadId
        /// </summary>
        public int ThreadId;
        public ushort UserListIndex;
        public int ReceiveTick;
        public string Account;
        public string ChrName;
    }

    public class DelayMessage
    {
        public int DelayTime;
        public int Mag;
        public int Cmd;
        public int Dir;
        public int BufLen;
        public byte[] Buffer;
    }

    public enum TBlockIPMethod
    {
        mDisconnect,
        mBlock,
        mBlockList
    }

    public enum TPunishMethod
    {
        /// <summary>
        /// 转换封包
        /// </summary>
        TurnPack,
        /// <summary>
        /// 丢去封包
        /// </summary>
        DropPack,
        /// <summary>
        /// 无效封包
        /// </summary>
        NullPack,
        /// <summary>
        /// 延时处理
        /// </summary>
        DelaySend
    }

    public enum ChatFilterMethod
    {
        ReplaceAll,
        ReplaceOne,
        Dropconnect
    }

    public enum OverSpeedMsgMethod
    {
        ptSysmsg,
        ptMenuOK
    }

    public class Protocol
    {
        public static bool g_fServiceStarted = false;
        public const string _STR_CMD_FILTER = "{0} 此命令禁止使用！";
        public const string _STR_CONFIG_FILE = ".\\Config.ini";
        public const string _STR_BLOCK_FILE = ".\\BlockIPList.txt";
        public const string _STR_BLOCK_AREA_FILE = ".\\BlockIPAreaList.txt";
        public const string _STR_CHAT_FILTER_FILE = ".\\ChatFilter.txt";
        public const string _STR_CHAT_CMD_FILTER_FILE = ".\\CharCmdFilter.txt";
        public const string _STR_PUNISH_USER_FILE = ".\\PunishList.txt";
        public const int FIRST_PAKCET_MAX_LEN = 254;
        public const int MAGIC_NUM = 0128;
        public const int DELAY_BUFFER_LEN = 1024;
    }
}