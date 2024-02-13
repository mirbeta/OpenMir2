using MemoryPack;

namespace OpenMir2.Packets.ClientPackets
{
    [MemoryPackable]
    public partial record struct CharDesc
    {
        public int Feature;
        public int Status;
    }
}