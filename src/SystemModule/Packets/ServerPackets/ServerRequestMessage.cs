using MemoryPack;

namespace SystemModule.Packets.ServerPackets
{
    /// <summary>
    /// 请求消息定义
    /// </summary>
    [MemoryPackable]
    public partial class ServerRequestMessage : ServerPacket
    {
        public int Recog { get; set; }
        public ushort Ident { get; set; }
        public ushort Param { get; set; }
        public ushort Tag { get; set; }
        public ushort Series { get; set; }

        public ServerRequestMessage(int ident, int recog, int param, int tag, int series)
        {
            Recog = recog;
            Ident = (ushort)ident;
            Param = (ushort)param;
            Tag = (ushort)tag;
            Series = (ushort)series;
        }

        public override int GetPacketSize()
        {
            throw new System.NotImplementedException();
        }
    }
}