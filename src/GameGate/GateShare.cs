using GameGate.Filters;
using GameGate.Services;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using SystemModule.Common;
using SystemModule.Packets;

namespace GameGate
{
    public class GateShare
    {
        public static bool ShowLog = true;
        /// <summary>
        /// 单线程最大用户数
        /// </summary>
        public const int MaxSession = 6000;
        /// <summary>
        /// 消息头加密固定长度
        /// </summary>
        public const byte CommandFixedLength = 16;
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
        public static ConcurrentDictionary<string, byte> ChatCommandFilterMap;
        public static readonly ArrayPool<byte> BytePool = ArrayPool<byte>.Shared;
        public static Dictionary<string, ClientSession> PunishList;
        public static HardwareFilter HardwareFilter;
        public static AbusiveFilter AbusiveFilter;
        public static ChatCommandFilter ChatCommandFilter;
        public const int HeaderMessageSize = ServerMessage.PacketSize;

        public static void Initialization()
        {
            BlockIPList = new StringList();
            TempBlockIPList = new List<string>();
            AbusiveFilter = AbusiveFilter.Instance;
            ChatCommandFilter = ChatCommandFilter.Instance;
            PunishList = new Dictionary<string, ClientSession>(StringComparer.OrdinalIgnoreCase);
            ChatCommandFilterMap = new ConcurrentDictionary<string, byte>(StringComparer.OrdinalIgnoreCase);
        }

        public static void Load()
        {
            AbusiveFilter.LoadChatFilterList();
            ChatCommandFilter.LoadChatCommandFilterList();
        }
    }

    public readonly struct SessionMessage
    {
        public byte[] Buffer { get; }
        public int PacketLen { get; }
        public int SessionId { get; }
        public byte ServiceId { get; }

        public SessionMessage(byte[] Buffer, int PacketLen, int SessionId, byte ServiceId)
        {
            this.Buffer = Buffer;
            this.PacketLen = PacketLen;
            this.SessionId = SessionId;
            this.ServiceId = ServiceId;
        }
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
        /// <summary>
        /// 整句替换
        /// </summary>
        ReplaceAll,
        /// <summary>
        /// 替换脏字
        /// </summary>
        ReplaceOne,
        /// <summary>
        /// 丢弃
        /// </summary>
        Dropconnect
    }

    public enum OverSpeedMsgMethod
    {
        ptSysmsg,
        ptMenuOK
    }

    public class Protocol
    {
        public const string CmdFilter = "{0} 此命令禁止使用！";
    }
}