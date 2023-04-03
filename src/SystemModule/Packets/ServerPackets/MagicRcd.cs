using MemoryPack;
using System.Runtime.InteropServices;

namespace SystemModule.Packets.ServerPackets;

[MemoryPackable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class MagicRcd
{
    /// <summary>
    /// 技能ID
    /// </summary>
    public ushort MagIdx;
    /// <summary>
    /// 等级
    /// </summary>
    public byte Level;
    /// <summary>
    /// 技能快捷键
    /// </summary>
    public char MagicKey;
    /// <summary>
    /// 当前修练值
    /// </summary>
    public int TranPoint;
}