using System;
using System.Net.Sockets;

namespace GameGate
{
    public struct ClientSessionPacket
    {
        public Memory<byte> Buffer;
        public int BufferLen;
        public int SessionId;
    }

    public struct ClientMessagePacket
    {
        public Memory<byte> Buffer;
        public int BufferLen;
        public int ConnectionId;
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
        public ushort nUserListIndex;
        public int dwReceiveTick;
        public string sAccount;
        public string sChrName;
    }

    public class DelayMessage
    {
        public int dwDelayTime;
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

    public enum TChatFilterMethod
    {
        ReplaceAll,
        ReplaceOne,
        Dropconnect
    }

    public enum TOverSpeedMsgMethod
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