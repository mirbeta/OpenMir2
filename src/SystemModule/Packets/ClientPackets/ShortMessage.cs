using MemoryPack;

namespace SystemModule.Packets
{
    [MemoryPackable]
    public partial record struct ShortMessage 
    {
        public ushort Ident;
        public ushort wMsg;
    }
}
