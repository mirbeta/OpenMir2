using MemoryPack;
using System.IO;

namespace SystemModule.Packets.ClientPackets
{
    [MemoryPackable]
    public partial struct ShortMessage 
    {
        public ushort Ident;
        public ushort wMsg;
    }
}
