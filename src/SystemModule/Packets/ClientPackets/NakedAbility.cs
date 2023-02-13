using System.IO;
using MemoryPack;

namespace SystemModule.Packets.ClientPackets
{
    [MemoryPackable]
    public partial struct NakedAbility
    {
        public ushort DC{ get; set; }
        public ushort MC{ get; set; }
        public ushort SC{ get; set; }
        public ushort AC{ get; set; }
        public ushort MAC{ get; set; }
        public ushort HP{ get; set; }
        public ushort MP{ get; set; }
        public byte Hit{ get; set; }
        public int Speed{ get; set; }
        public byte Reserved{ get; set; }
    }
}