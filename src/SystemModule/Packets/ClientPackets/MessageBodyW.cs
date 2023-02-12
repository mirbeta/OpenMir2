using MemoryPack;

namespace SystemModule.Packets.ClientPackets
{
    [MemoryPackable]
    public partial struct MessageBodyW
    {
        public ushort Param1;
        public ushort Param2;
        public ushort Tag1;
        public ushort Tag2;
    }
}

