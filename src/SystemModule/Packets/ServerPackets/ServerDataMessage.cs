using MemoryPack;
using System;

namespace SystemModule.Packets.ServerPackets
{
    [MemoryPackable]
    public partial class ServerDataMessage 
    {
        public uint PacketCode { get; set; }
        public int PacketLen { get; set; }
        public ServerDataType Type { get; set; }
        public int SocketId { get; set; }
        public short DataLen { get; set; }
        public byte[] Data { get; set; }

        /// <summary>
        /// 消息头固定大小
        /// </summary>
        public const int HeaderPacketSize = 8;

        public int GetPacketSize()
        {
            return HeaderPacketSize + 7 + (Data?.Length ?? 0);
        }
    }

    public enum ServerDataType : byte
    {
        Enter = 0,
        Leave = 1,
        Data = 2,
        KeepAlive = 3
    }
}