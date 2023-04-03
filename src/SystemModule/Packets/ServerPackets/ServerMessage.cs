using MemoryPack;
using System.Runtime.InteropServices;
using SystemModule.Packets.ClientPackets;

namespace SystemModule.Packets.ServerPackets;

/// <summary>
/// 封包消息头
/// </summary>
[MemoryPackable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial struct ServerMessage
{
    public uint PacketCode { get; set; }
    /// <summary>
    /// SocketID
    /// </summary>
    public int Socket { get; set; }
    /// <summary>
    /// 会话ID
    /// </summary>
    public ushort SessionId { get; set; }
    public ushort Ident { get; set; }
    public int SessionIndex { get; set; }
    public int PackLength { get; set; }

    public const int PacketSize = 20;
}

[MemoryPackable]
public partial struct ClientOutMessage
{
    public ServerMessage MessagePacket;
    public CommandMessage CommandPacket;
    public byte[] Data;
}