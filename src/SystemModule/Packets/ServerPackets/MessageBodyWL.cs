using MemoryPack;
using System.Runtime.InteropServices;

namespace SystemModule.Packets.ServerPackets;

[MemoryPackable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial record struct MessageBodyWL
{
    public int Param1;
    public int Param2;
    public int Tag1;
    public int Tag2;
}