using MemoryPack;

namespace SystemModule.Packets.ServerPackets
{
    [MemoryPackable]
    public partial record struct CharDesc
    {
        public int Feature;
        public int Status;
    }
}