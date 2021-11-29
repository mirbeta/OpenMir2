using System;
using System.IO;

namespace GameGate
{

    public struct TSvrCmdPack
    {
        public double Flag;
        public double SockID;
        public ushort Seq;
        public ushort Cmd;
        public int GGSock;
        public int DataLen;
    }

    public struct _tagCmdHeader
    {
        public double Header;
        public ushort Cmd;
        public ushort Cmd1;
        public double Tail;
    }

    public struct TMagic
    {
        public char[] Reserved1;
        public ushort MagicID;
        public char[] Reserved2;
        public ushort Delay;
        public char[] Reserved3;
    }

    public struct TEnDeInfo
    {
        public double Head;
        public ushort Cmd;
        public ushort Cmd1;
        public double Tail;
    }

    public class TDelayMsg
    {
        public int dwDelayTime;
        public int nMag;
        public int nCmd;
        public int nDir;
        public int nBufLen;
        public byte[] pBuffer;
    } 

    public struct TPerIPAddr
    {
        public long IPaddr;
        public int Count;
    }

    public struct TIPArea
    {
        public double Low;
        public double High;
    } 

    public enum TBlockIPMethod
    {
        mDisconnect,
        mBlock,
        mBlockList
    } // end TBlockIPMethod

    public enum TPunishMethod
    {
        ptTurnPack,
        ptDropPack,
        ptNullPack,
        ptDelaySend
    } // end TPunishMethod

    public enum TChatFilterMethod
    {
        ctReplaceAll,
        ctReplaceOne,
        ctDropconnect
    } // end TChatFilterMethod

    public enum TOverSpeedMsgMethod
    {
        ptSysmsg,
        ptMenuOK
    }

    public enum TSockThreadStutas
    {
        stConnecting,
        stConnected,
        stTimeOut
    } 
}

namespace Protocol.Units
{
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
        public const string _STR_CMD_FILTER = "%s 此命令禁止使用！";
        public const string _STR_LIB_MMSYSTEM = "winmm.dll";
        public const string _STR_LIB_KERNEL32 = "kernel32.dll";
        public const string _STR_CONFIG_FILE = ".\\Config.ini";
        public const string _STR_BLOCK_FILE = ".\\BlockIPList.txt";
        public const string _STR_BLOCK_AREA_FILE = ".\\BlockIPAreaList.txt";
        public const string _STR_CHAT_FILTER_FILE = ".\\ChatFilter.txt";
        public const string _STR_CHAT_CMD_FILTER_FILE = ".\\CharCmdFilter.txt";
        public const string _STR_PUNISH_USER_FILE = ".\\PunishList.txt";
        public const uint RUNGATECODE = 0xAA55AA55 + 0x00450045;
        public const int FIRST_PAKCET_MAX_LEN = 254;
        public const int MAGIC_NUM = 0128;
        public const int DELAY_BUFFER_LEN = 1024;
    }
}

