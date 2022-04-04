using System.Net.Sockets;

namespace GameGate
{
    public struct TMessageData
    {
        public byte[] Buffer;
        public int BufferLen;
        public int MessageId;
    }

    public class TSessionInfo
    {
        public Socket Socket;
        public int SessionId;
        public int SckHandle;
        public ushort nUserListIndex;
        public int dwReceiveTick;
        public string sAccount;
        public string sChrName;
    }

    public class TDelayMsg
    {
        public int dwDelayTime;
        public int nMag;
        public int nCmd;
        public int nDir;
        public int nBufLen;
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
        ctReplaceAll,
        ctReplaceOne,
        ctDropconnect
    }

    public enum TOverSpeedMsgMethod
    {
        ptSysmsg,
        ptMenuOK
    }

    public class Protocol
    {
        public static bool g_fServiceStarted = false;
        public const string _STR_GRID_INDEX = "网关";
        public const string _STR_GRID_IP = "网关地址";
        public const string _STR_GRID_PORT = "端口";
        public const string _STR_GRID_CONNECT_STATUS = "连接状态";
        public const string _STR_GRID_IO_SEND_BYTES = "发送";
        public const string _STR_GRID_IO_RECV_BYTES = "接收";
        public const string _STR_KEEP_ALIVE = "**";
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