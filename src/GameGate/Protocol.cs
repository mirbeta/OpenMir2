using System.IO;

namespace GameGate
{
    public class TSvrCmdPack
    {
        public uint Flag;
        public int SockID;
        public ushort Seq;
        public ushort Cmd;
        public int GGSock;
        public int DataLen;

        public const int PackSize = 20;

        public TSvrCmdPack()
        {
            
        }

        public TSvrCmdPack(byte[] buff)
        {
            var binaryReader = new BinaryReader(new MemoryStream(buff));
            Flag = binaryReader.ReadUInt32();
            SockID = binaryReader.ReadInt32();
            Seq = binaryReader.ReadUInt16();
            Cmd = binaryReader.ReadUInt16();
            GGSock = binaryReader.ReadInt32();
            DataLen = binaryReader.ReadInt32();
        }
        
        public byte[] GetPacket()
        {
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            backingStream.Write(Flag);
            backingStream.Write(SockID);
            backingStream.Write(Seq);
            backingStream.Write(Cmd);
            backingStream.Write(GGSock);
            backingStream.Write(DataLen);
            var stream = backingStream.BaseStream as MemoryStream;
            return stream?.ToArray();
        }
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

    public enum TBlockIPMethod
    {
        mDisconnect,
        mBlock,
        mBlockList
    }

    public enum TPunishMethod
    {
        ptTurnPack,
        ptDropPack,
        ptNullPack,
        ptDelaySend
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
        public const string _STR_CMD_FILTER = "%s 此命令禁止使用！";
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

