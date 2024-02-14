using MemoryPack;

namespace OpenMir2.Packets.ClientPackets
{
    [MemoryPackable]
    public partial record struct ShortMessage
    {
        public ushort Ident;
        public ushort wMsg;
    }
}