using MemoryPack;

namespace SystemModule.Packets
{
    [MemoryPackable]
    public partial record struct CharDesc
    {
        public int Feature;
        public int Status;
    }
}

