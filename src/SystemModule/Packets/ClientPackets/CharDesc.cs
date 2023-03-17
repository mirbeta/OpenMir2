using MemoryPack;

namespace SystemModule.Packets.ClientPackets
{
    [MemoryPackable]
    public partial record struct CharDesc
    {
        public int Feature;
        public int Status;
    }
}

