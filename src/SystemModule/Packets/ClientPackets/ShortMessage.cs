using MemoryPack;

namespace SystemModule.Packets.ClientPackets
{
    [MemoryPackable]
    public partial record struct ShortMessage
    {
        public ushort Ident;
        public ushort wMsg;
    }
}