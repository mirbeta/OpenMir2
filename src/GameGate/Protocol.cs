using System.IO;
using SystemModule;

namespace GameGate
{
    public class TSvrCmdPack : Packets
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

        public TSvrCmdPack(byte[] buff) : base(buff)
        {
            Flag = ReadUInt32();
            SockID = ReadInt32();
            Seq = ReadUInt16();
            Cmd = ReadUInt16();
            GGSock = ReadInt32();
            DataLen = ReadInt32();
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Flag);
            writer.Write(SockID);
            writer.Write(Seq);
            writer.Write(Cmd);
            writer.Write(GGSock);
            writer.Write(DataLen);
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
        public const uint RUNGATECODE = 0xAA55AA55 + 0x00450045;
        public const int FIRST_PAKCET_MAX_LEN = 254;
        public const int MAGIC_NUM = 0128;
        public const int DELAY_BUFFER_LEN = 1024;
    }
}