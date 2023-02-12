using MemoryPack;

namespace SystemModule.Packets.ClientPackets
{
    [MemoryPackable]
    public partial struct MessageBodyWL
    {
        public int Param1;
        public int Param2;
        public int Tag1;
        public int Tag2;
    }
}