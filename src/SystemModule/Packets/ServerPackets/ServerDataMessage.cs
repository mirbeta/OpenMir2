using System.Runtime.InteropServices;
using MemoryPack;

namespace SystemModule.Packets.ServerPackets;

[MemoryPackable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial struct ServerDataPacket
{
    /// <summary>
    /// 封包标识码
    /// </summary>
    public uint PacketCode { get; set; }
    /// <summary>
    /// 封包总长度
    /// </summary>
    public ushort PacketLen { get; set; }

    /// <summary>
    /// 消息头固定大小
    /// </summary>
    public const int FixedHeaderLen = 6;
}

[MemoryPackable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial struct ServerDataMessage
{
    public ServerDataType Type { get; set; }
    public int SocketId { get; set; }
    public short DataLen { get; set; }
    public byte[] Data { get; set; }
}

public enum ServerDataType : byte
{
    Enter = 0,
    Leave = 1,
    Data = 2,
    KeepAlive = 3
}