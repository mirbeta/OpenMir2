using MemoryPack;
using System;

namespace SystemModule.Packets.ServerPackets
{
    [MemoryPackable]
    public partial class LoginSvrPacket
    {
        public int ConnectionId { get; set; }
        public short PackLen { get; set; }
        public byte[] ClientPacket { get; set; }

        public int GetPacketSize()
        {
            return 6 + ClientPacket.Length;
        }
    }
}