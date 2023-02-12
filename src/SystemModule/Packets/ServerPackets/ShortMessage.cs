using MemoryPack;

namespace SystemModule.Packets.ServerPackets
{
    [MemoryPackable]
    public partial record struct ShortMessage
    {
        public ushort Ident;
        public ushort wMsg;
    }
}