using MemoryPack;

namespace SystemModule.Packets.ServerPackets
{
    [MemoryPackable]
    public partial record struct MessageBodyWL
    {
        public int Param1;
        public int Param2;
        public int Tag1;
        public int Tag2;
    }
}