using ProtoBuf;

namespace SystemModule.Packet.ServerPackets
{
    /// <summary>
    /// 服务端消息
    /// </summary>
    [ProtoContract]
    public class ServerMessagePacket
    {
        [ProtoMember(1)] 
        public int Recog { get; set; }
        [ProtoMember(2)] 
        public ushort Ident { get; set; }
        [ProtoMember(3)] 
        public ushort Param { get; set; }
        [ProtoMember(4)] 
        public ushort Tag { get; set; }
        [ProtoMember(5)] 
        public ushort Series { get; set; }

        public ServerMessagePacket()
        {
        }

        public ServerMessagePacket(int ident, int recog, int param, int tag, int series)
        {
            Recog = recog;
            Ident = (ushort)ident;
            Param = (ushort)param;
            Tag = (ushort)tag;
            Series = (ushort)series;
        }
    }
}