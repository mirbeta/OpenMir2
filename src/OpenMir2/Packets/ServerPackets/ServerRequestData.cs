using System.Runtime.InteropServices;
using MemoryPack;

namespace OpenMir2.Packets.ServerPackets
{
    [MemoryPackable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public partial class ServerRequestData
    {
        public int QueryId { get; set; }
        /// <summary>
        /// 消息头
        /// </summary>
        public byte[] Message { get; set; }
        /// <summary>
        /// 消息封包
        /// </summary>
        public byte[] Packet { get; set; }
        /// <summary>
        /// 验签
        /// </summary>
        public byte[] Sign { get; set; }
    }

    [MemoryPackable]
    public partial class LoadPlayerDataPacket
    {
        public string ChrName { get; set; }
        public CharacterDataInfo HumDataInfo { get; set; }
    }
}