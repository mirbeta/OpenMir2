using MemoryPack;
using System.IO;

namespace SystemModule.Packets.ClientPackets
{
    [MemoryPackable]
    public partial struct CharDesc
    {
        public int Feature;
        public int Status;
    }
}

